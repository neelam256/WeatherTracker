using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WT.Models;
using WT.Service.Interfaces;
using WT.Services;


namespace WT.Controllers
{
    public class MeasurementsController : ApiController
    {
        private readonly IMeasurementsServices _MeasurementsServices;
        private readonly IStatisticsService _StatisticsService;

        public MeasurementsController()
        {
            // I could use dependency injection to resolve this dependancy            
            _MeasurementsServices = new MeasurementsServices();
            _StatisticsService = new StatisticsService();

        }


        /// <summary>
        /// Get Measurement(s) for a day or a specified timestamp    
        ///If a day specified in the format of 'YYYY-MM-DD', a list of
        /// Measurements within that day is returned.Otherwise the Measurement
        /// matching the timestamp is returned  
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>

        [HttpGet, Route("api/Measurements/{date}")]
        public IHttpActionResult GetMeasurementsForTime(string date, string timeStamp = null)
        {
            DateTime inputDate;
            DateTime inputTimeStamp;
            IList<Measurement> measurements = null;

            if (DateTime.TryParse(date, out inputDate))
            {

                if (timeStamp != null && DateTime.TryParse(timeStamp, null, System.Globalization.DateTimeStyles.RoundtripKind, out inputTimeStamp))
                {
                    //Measurement with matching  inputTimeStamp is returned  
                    measurements = _MeasurementsServices.GetMeasurements(inputDate, inputTimeStamp);
                }
                else
                {
                    //List of measurements between inputDate and inputDate +1 is returned
                    measurements = _MeasurementsServices.GetMeasurements(inputDate);
                }

                if (measurements != null)
                    return Ok(measurements);
                else
                    return NotFound();
            }
            else
            {
                //If date string is not good 
                return BadRequest("Input date is not in correct Format");
            }

        }

        /// <summary>
        /// Add /Update a measurement  of a given timestamp
        /// Timestamp is required
        /// Any other metrics must be name and number collection
        /// </summary>
        /// <param name="measurement">
        /// StrtimeStamp: Timestamp required to add/Update a measurement (required)
        /// MetricData: List of MetricData 
        /// (1)  MetricName : Name of metric. e.g temperature, dewPoint, precipitation
        /// (2)  MetricValue: number value of metric. Value should be valid number
        /// </param>
        /// <returns>
        /// Return Status code 201 if successfully added 
        /// Else return 400
        /// </returns>
        [HttpPost, Route("api/Measurements")]
        public IHttpActionResult SaveMeasurementsForTime(MeasurementVM measurement)
        {
            bool isSaved = false;
            if (measurement != null)            
                isSaved = _MeasurementsServices.SaveMeasurement(measurement);
            
            if (isSaved)           
                return Created("api/Measurements", measurement.TimeStampStr);           
            else
                return BadRequest("Measurement cannot be added with invalid values");
        }



        /// <summary>
        /// Statistics on specified metric between fromDateTime and toDateTime
        /// </summary>
        /// <param name="metrics">
        /// List of metrics
        /// Values could be: temperature, dewPoint, precipitation
        /// </param>
        /// <param name="stats">
        /// List of stats 
        /// Supported values:   min, max, avg          
        /// </param>
        /// <param name="fromDateTime">Startdate [include startdate data]</param>
        /// <param name="toDateTime">EndDate [included enddate data]</param>
        /// <returns></returns>
        [HttpGet, Route("api/stats")]
        public IHttpActionResult GetMeasurementStatistics([FromUri] IList<string> metrics, [FromUri] IList<string> stats, string fromDateTime, string toDateTime)
        {
            if (metrics != null && metrics.Count > 0 && stats != null && stats.Count > 0)
            { 
            var statictsList = _StatisticsService.CalculateStaticts(metrics, stats, fromDateTime, toDateTime);
            if (statictsList != null && statictsList.Count > 0)
            {
                return Ok(statictsList);
            }
            else
                return Ok("No Data available");
            }

            return BadRequest("Parameters are not valid");
        }



    }
}