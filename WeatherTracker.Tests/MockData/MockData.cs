using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WT.Controllers;
using WT.Models;

namespace WeatherTracker.Tests
{
    public static class MockData
    {
        public static IList<MetricData> PrepareMetricDataCollection()
        {
            IList<MetricData> metricDataList = new List<MetricData>();
            metricDataList.Add(PrepareMetricData_Temperature("temperature", 45.2M));
            metricDataList.Add(PrepareMetricData_Temperature("DewPoin", 20));
            metricDataList.Add(PrepareMetricData_Temperature("Precipitation", 5));
           

            return metricDataList;
        }


        public static MetricData PrepareMetricData_Temperature(string metricName, decimal metricValue)
        {
            MetricData metricMockData = new MetricData
            {
                MetricName = metricName,
                MetricValue = metricValue
            };

            return metricMockData;
        }

        public static void SaveMeasurement(String timeValue)
        {
            MeasurementsController measurementController = new MeasurementsController();

            MeasurementVM measurementdata = new MeasurementVM
            {
                TimeStampStr = timeValue,
                MeasurementValues = PrepareMetricDataCollection()
            };
            var response = measurementController.SaveMeasurementsForTime(measurementdata);
        }

        public static IList<String> PrepareStatsData()
        {
            List<string> stats = new List<string> { "Max", "Min", "Avg" };
            return stats;

        }

        public static IList<String> PrepareMetricData()
        {

            List<string> metric = new List<string> { "temperature", "DewPoin", "Precipitation" };
            return metric;

        }
    }
}
