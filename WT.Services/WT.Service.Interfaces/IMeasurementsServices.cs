using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WT.Models;

namespace WT.Service.Interfaces
{
    public interface IMeasurementsServices
    {
        /// <summary>
        /// Get measurement(s) 
        /// </summary>
        /// <param name="startDay">startday to get the measurement</param>
        /// <param name="timeStamp">If not null then return measurement for this specific timestamp</param>
        /// <returns>Collection of measurement</returns>
        IList<Measurement> GetMeasurements(DateTime startDay, DateTime? timeStamp= null);


        /// <summary>
        ///  Add/Update measurement for given timestamp
        /// </summary>
        /// <param name="measurement"></param>
        /// <returns>Return true if successfull added else false</returns>
        Boolean SaveMeasurement(MeasurementVM measurement);


    }
}
