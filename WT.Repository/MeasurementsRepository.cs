using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WT.Models;
using System.Runtime.Caching;

namespace WT.Repository
{
    public static class MeasurementsRepository
    {
        //cache object to save in memory data
        public static ObjectCache _cache = MemoryCache.Default;


        /// <summary>
        /// Return measurent for specific timestamp
        /// if key not found then return null
        /// </summary>
        /// <param name="timeStampKey"></param>
        /// <returns></returns>
        public static Measurement GetMeasurementsForTimeStamp(DateTime timeStampKey)
        {
            var cacheValue = _cache[timeStampKey.ToString()];           
            return (Measurement)cacheValue;
        }

        /// <summary>
        /// Return the list of measurement between startdate and enddate 
        /// </summary>
        /// <param name="startDateTime">start timestamp [included]</param>
        /// <param name="enddateTime">end timestamp [included]</param>
        /// <returns></returns>
        public static IList<Measurement> GetMeasurementInRange(DateTime startDateTime, DateTime enddateTime)
        {
            var allCache = _cache.ToList();
            IList<Measurement> measurementInGivenRange = new List<Measurement>();
            foreach (var item in allCache)
            {
                //key in cache is string, it should be parse into datetime (ISO-8061) 
                DateTime cacheKey;
              
                if (DateTime.TryParse(item.Key, null, System.Globalization.DateTimeStyles.RoundtripKind, out cacheKey))
                {
                    // start date timestamp and end date timestamp, both included
                    if (cacheKey >= startDateTime && cacheKey <= enddateTime)
                    {
                        
                        measurementInGivenRange.Add((Measurement)item.Value);
                    }
                }
            }
            return measurementInGivenRange;
        }

        /// <summary>
        /// Save the data into in memory cache object
        /// if Key value pair
        /// timeStamp is key
        /// measurement is value
        /// </summary>
        /// <param name="timeStampKey">Key for cache object</param>
        /// <param name="measurement">Measurment is collection of metric data. 
        /// Metric data has  name of metric and
        /// value of metric.
        /// </param>
        public static void SaveMeasurements(DateTime timeStampKey, Measurement measurement)
        {

            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.Priority = CacheItemPriority.Default;
            _cache.Set(timeStampKey.ToString(), measurement, cacheItemPolicy);
           
        }

        /// <summary>
        /// Remove all in-memory data from cache object
        /// </summary>
        public static void ClearAllMeasurements()
        {
            var allCache = _cache.ToList();   
            
            foreach (var item in allCache)
            {
                _cache.Remove(item.Key);              
            }
        }

       


    }
}
