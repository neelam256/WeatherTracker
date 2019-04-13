using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WT.Controllers;
using WT.Models;
using System.Web.Http.Results;
using WT.Repository;

namespace WeatherTracker.Tests
{
    [TestClass]
    public class MeasurementsController_Statistics
    {
        [TestMethod]
        public void GetMeasurementStatistics_BadRequest_ForNullValues()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act            
            var response = measurementController.GetMeasurementStatistics(null,null, null,null);
            var actualResponse = response as BadRequestErrorMessageResult;


            //Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreEqual(actualResponse.Message, "Parameters are not valid");

        }

        [TestMethod]
        public void GetMeasurementStatistic_fromtimeValue_RestNullValues()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act     
            var fromtimeValue = DateTime.UtcNow;
            MockData.SaveMeasurement(fromtimeValue.ToString());

            var totimeValue = DateTime.UtcNow.AddSeconds(3);
            MockData.SaveMeasurement(totimeValue.ToString());

            var response = measurementController.GetMeasurementStatistics(null, null, fromtimeValue.ToString(), null);
            var actualResponse = response as BadRequestErrorMessageResult;


            //Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreEqual(actualResponse.Message, "Parameters are not valid");

        }

        [TestMethod]
        public void GetMeasurementStatistic_totimeValue_RestNullValues()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act    
            MeasurementsRepository.ClearAllMeasurements();

            var fromtimeValue = DateTime.UtcNow;
            MockData.SaveMeasurement(fromtimeValue.ToString());

            var totimeValue = DateTime.UtcNow.AddSeconds(3);
            MockData.SaveMeasurement(totimeValue.ToString());

            var response = measurementController.GetMeasurementStatistics(null, null, null, totimeValue.ToString());
            var actualResponse = response as BadRequestErrorMessageResult;


            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(actualResponse.Message, "Parameters are not valid");

        }

        [TestMethod]
        public void GetMeasurementStatistic_StatsNull()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act     
            MeasurementsRepository.ClearAllMeasurements();

            var fromtimeValue = DateTime.UtcNow;
            MockData.SaveMeasurement(fromtimeValue.ToString());

            var totimeValue = DateTime.UtcNow.AddSeconds(3);
            MockData.SaveMeasurement(totimeValue.ToString());

            var response = measurementController.GetMeasurementStatistics(MockData.PrepareMetricData(), null, fromtimeValue.ToString(), totimeValue.ToString());
            var actualResponse = response as BadRequestErrorMessageResult;


            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(actualResponse.Message, "Parameters are not valid");

        }


        [TestMethod]
        public void GetMeasurementStatistic_NoData_Statistics()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act     
            MeasurementsRepository.ClearAllMeasurements();

            var fromtimeValue = DateTime.UtcNow;
            MockData.SaveMeasurement(fromtimeValue.ToString());

            var totimeValue = DateTime.UtcNow.AddSeconds(3);
            MockData.SaveMeasurement(totimeValue.ToString());

            var response = measurementController.GetMeasurementStatistics(MockData.PrepareMetricData(), MockData.PrepareStatsData(), fromtimeValue.AddDays(1).ToString(), totimeValue.AddDays(3).ToString());
            var actualResponse = response as OkNegotiatedContentResult<String>;

           

            //Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreEqual(actualResponse.Content, "No Data available");

        }

        [TestMethod]
        public void GetMeasurementStatistic_Statistics()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act     
            MeasurementsRepository.ClearAllMeasurements();

            var fromtimeValue = DateTime.UtcNow;
            MockData.SaveMeasurement(fromtimeValue.ToString());

            var totimeValue = DateTime.UtcNow.AddSeconds(3);
            MockData.SaveMeasurement(totimeValue.ToString());

            var response = measurementController.GetMeasurementStatistics(MockData.PrepareMetricData(), MockData.PrepareStatsData(), fromtimeValue.ToString(), totimeValue.ToString());
            var actualResponse = response as OkNegotiatedContentResult<IList<Statistic>>;

            var returnResponse = (IList<Statistic>)actualResponse.Content;

            //Assert
            Assert.IsNotNull(response);
           Assert.IsTrue(returnResponse.Count> 1, "It should contain statistics ");

        }
    }
}
