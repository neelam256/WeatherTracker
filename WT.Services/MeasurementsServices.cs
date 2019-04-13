using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WT.Models;
using WT.Repository;
using WT.Service.Interfaces;

namespace WT.Services
{
    public class MeasurementsServices : IMeasurementsServices
    {
        /// <summary>
        /// Get measurement(s) 
        /// </summary>
        /// <param name="startDay">startday to get the measurement</param>
        /// <param name="timeStamp">If not null then return measurement for this specific timestamp</param>
        /// <returns></returns>
        public IList<Measurement> GetMeasurements(DateTime startDay, DateTime? timeStamp = null)
        {
            IList<Measurement> measurements = null;
            if (timeStamp == null)
            {
                var endDate = GetEndDate(startDay);
                measurements = GetMeasurementInRange(startDay, endDate);
            }
            else
            {
                var measurement = GetMeasurementForTimeStamp(timeStamp);
                if (measurement != null)
                {
                    measurements = new List<Measurement>();
                    measurements.Add(measurement);
                }
            }
            return measurements;

        }

        /// <summary>
        /// Add/Update measurement for given timestamp
        /// </summary>
        /// <param name="measurement">return true if successfully save else false</param>
        public Boolean SaveMeasurement(MeasurementVM measurementVM)
        {
            bool isSaved = false;
            var timeStamp = GetDateTime(measurementVM.TimeStampStr);

            //Check if valid metricData collection && ValidDateTime
            if (measurementVM.MeasurementValues != null && measurementVM.MeasurementValues.Count > 1 && IsValidDate(timeStamp))
            {
                var measurement = CreateMeasurementObject(measurementVM.MeasurementValues, timeStamp);
                //Add measurement in cache
                MeasurementsRepository.SaveMeasurements((DateTime)timeStamp, measurement);
                isSaved = true;

            }
            return isSaved;
        }

        private Measurement CreateMeasurementObject(IList<MetricData> metricDataList, DateTime? timeStamp)
        {
            Measurement measurementObject = new Measurement
            {
                MeasurementValues = metricDataList,
                TimeStamp = (DateTime)timeStamp
            };

            return measurementObject;
        }

        private DateTime? GetDateTime(String StrtimeStamp)
        {
            DateTime time;
            if (StrtimeStamp != null)
            {
                DateTime.TryParse(StrtimeStamp, null, System.Globalization.DateTimeStyles.RoundtripKind, out time);
                return time;
            }
            return null;
        }

        private bool IsValidDate(DateTime? dateTimeStamp)
        {
            bool isValid = false;
            if (dateTimeStamp != null)
            {
                //1/1/0001 12:00:00 AM
                DateTime dt = (DateTime)dateTimeStamp;
                var year = dt.Year;
                if (year != 0001)
                    isValid = true;
            }
            return isValid;

        }

        /// <summary>
        /// Return Enddate which is startdate + 1 day
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        private DateTime GetEndDate(DateTime startDate)
        {
            var startDateAsUtc = startDate.ToUniversalTime();
            var endDate = startDateAsUtc.AddDays(1).ToUniversalTime();
            return endDate;
        }

        /// <summary>
        /// Return list of measurement between start 
        /// it will return null if no data found
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private IList<Measurement> GetMeasurementInRange(DateTime startDate, DateTime endDate)
        {

            var mesurementValues = MeasurementsRepository.GetMeasurementInRange(startDate, endDate);
            if (mesurementValues != null && mesurementValues.Count > 0)
                return mesurementValues;
            else
                return null;
        }

        /// <summary>
        /// Return measurement for timeStamp
        /// If timestamp is null it will return null        
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private Measurement GetMeasurementForTimeStamp(DateTime? timeStamp)
        {
            if (timeStamp != null)
            {
                var mesurementValues = MeasurementsRepository.GetMeasurementsForTimeStamp((DateTime)timeStamp);
                return mesurementValues;
            }
            else
                return null;

        }
    }
}

