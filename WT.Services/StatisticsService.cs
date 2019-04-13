using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WT.Models;
using WT.Repository;

namespace WT.Services
{
   public class StatisticsService : IStatisticsService
    {
        /// <summary>
        /// Return the List of statistic data 
        /// </summary>
        /// <param name="metrics"></param>
        /// <param name="stats"></param>
        /// <param name="fromDateTime"></param>
        /// <param name="toDateTime"></param>
        /// <returns></returns>
        public IList<Statistic> CalculateStaticts(IList<string> metrics, IList<string> stats, string fromDateTime, string toDateTime)
        {

            DateTime from;
            DateTime to;
            // parse date into ISO-8061 
            DateTime.TryParse(fromDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind, out from);
            DateTime.TryParse(toDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind, out to);

            //Get the measurment list between fromDateTime and to date
            var measurementList = MeasurementsRepository.GetMeasurementInRange(from, to);

            if (measurementList != null && measurementList.Count > 0)
            {
                var statisticList = calculateStatistics(metrics, stats, measurementList);
                return statisticList;
            }
            return null;
        }


        /// <summary>
        /// Prepare list of statistic for metric
        /// </summary>
        /// <param name="metrics">temperature, dewPoint, precipitation etc..</param>
        /// <param name="stats">Max,Min, AVG</param>
        /// <param name="measurementList"></param>
        /// <returns></returns>
        private IList<Statistic> calculateStatistics(IList<string> metrics, IList<string> stats, IList<Measurement> measurementList)
        {
            IList<Statistic> statisticList = new List<Statistic>();
            foreach (var metric in metrics)
            {
                //Filter the measurementlist for metric and Flattern list for metric data
                //return the list of metric data(Name of metric and value of metric)
                var metricDataList = measurementList.SelectMany(item => item.MeasurementValues.Where(t => t.MetricName.ToUpper().Equals(metric.Trim().ToUpper())));
                if (metricDataList != null && metricDataList.Any())
                {
                    foreach (var stat in stats)
                    {
                        //Statistic object 
                        var statsData = calculateStatForMetric(metric, stat, metricDataList, metric.ToUpper());
                        //add the Statistic object in list of Statistic collection
                        statisticList.Add(statsData);
                    }
                }
            }

            return statisticList;
        }

        /// <summary>
        /// return statistic data object
        /// if no matching case it will throw exception
        /// </summary>
        /// <param name="metric"></param>
        /// <param name="stat"></param>
        /// <param name="metricDataList"></param>
        /// <param name="metricName"></param>
        /// <returns></returns>
        private Statistic calculateStatForMetric(string metric, string stat, IEnumerable<MetricData> metricDataList, string metricName)
        {
            switch (stat.ToUpper())
            {
                case "MIN":
                    return GetMinMetric(metricDataList, metricName);
                case "MAX":
                    return GetMaxMetric(metricDataList, metricName);
                case "AVG":
                case "AVERAGE":
                    return GetAvgMetric(metricDataList, metricName);

                default:
                    throw new Exception("metric not supported");
            }
        }

        /// <summary>
        /// Get minimum value for metric
        /// </summary>
        /// <param name="metricDataList"></param>
        /// <param name="metric"></param>
        /// <returns></returns>
        private Statistic GetMinMetric(IEnumerable<MetricData> metricDataList, string metric)
        {
            var minValue = metricDataList.Min(i => i.MetricValue);
            Statistic statisticData = statisticData = new Statistic
            {
                Metric = metric,
                Stat = "MIN",
                Value = minValue

            };
            return statisticData;

        }

        /// <summary>
        /// Get maximum value for metric
        /// </summary>
        /// <param name="metricDataList"></param>
        /// <param name="metric"></param>
        /// <returns></returns>
        private Statistic GetMaxMetric(IEnumerable<MetricData> metricDataList, string metric)
        {
            var maxValue = metricDataList.Max(i => i.MetricValue);
            Statistic statisticData = statisticData = new Statistic
            {
                Metric = metric,
                Stat = "MAX",
                Value = Convert.ToDecimal(maxValue)

            };
            return statisticData;

        }

        /// <summary>
        /// Get average for metric 
        /// </summary>
        /// <param name="metricDataList"></param>
        /// <param name="metric"></param>
        /// <returns></returns>
        private Statistic GetAvgMetric(IEnumerable<MetricData> metricDataList, string metric)
        {

            var avgValues = metricDataList.Average(i => i.MetricValue);

            Statistic statisticData = statisticData = new Statistic
            {
                Metric = metric,
                Stat = "AVG",
                Value = Convert.ToDecimal(avgValues)

            };
            return statisticData;
        }
     
       
    }
}
