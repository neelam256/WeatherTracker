using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WT.Controllers;
using WT.Models;
using System.Web.Http.Results;
using WT.Repository;

namespace WeatherTracker.Tests
{
    [TestClass]
   public class MeasurementsController_Save_Test
    {


        [TestMethod]
        public void SaveMeasurementsForTime_BadRequest_ForNullObject()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act

            MeasurementVM measurementdata = null;
           var response = measurementController.SaveMeasurementsForTime(measurementdata);
            var actualResponse = response as BadRequestErrorMessageResult;


            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(actualResponse.Message, "Measurement cannot be added with invalid values");

        }


        [TestMethod]
        public void SaveMeasurementsForTime_BadRequest_ForWrongTimeStamp()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act
            MeasurementsRepository.ClearAllMeasurements();

            var  metricDataList  = MockData.PrepareMetricDataCollection();
             MeasurementVM measurementdata = new MeasurementVM
                                            {
                                                TimeStampStr = "7878",
                                                MeasurementValues = metricDataList
                                            };
            var response = measurementController.SaveMeasurementsForTime(measurementdata);
            var actualResponse = response as BadRequestErrorMessageResult;


            //Assert
            Assert.IsNotNull(response);        
            Assert.AreEqual(actualResponse.Message, "Measurement cannot be added with invalid values");

        }

        [TestMethod]
        public void SaveMeasurementsForTime_BadRequest_ForNullMetricData()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act
            MeasurementsRepository.ClearAllMeasurements();

            var metricDataList = MockData.PrepareMetricDataCollection();
            MeasurementVM measurementdata = new MeasurementVM
            {
                TimeStampStr = DateTime.UtcNow.ToString(),
                MeasurementValues = null
            };
            var response = measurementController.SaveMeasurementsForTime(measurementdata);
            var actualResponse = response as BadRequestErrorMessageResult;

            //Assert
            Assert.IsNotNull(response);            
            Assert.AreEqual(actualResponse.Message, "Measurement cannot be added with invalid values");

        }

        [TestMethod]
        public void SaveMeasurementsForTime_Created()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act
            MeasurementsRepository.ClearAllMeasurements();

            var metricDataList = MockData.PrepareMetricDataCollection();
            var timeValue = DateTime.UtcNow.ToString();
            MeasurementVM measurementdata = new MeasurementVM
            {
                TimeStampStr = timeValue,
                MeasurementValues = MockData.PrepareMetricDataCollection()
            };
            var response = measurementController.SaveMeasurementsForTime(measurementdata);
            var actualResponse = response as CreatedNegotiatedContentResult<string>;

            //Assert
            Assert.IsNotNull(actualResponse);
            Assert.IsInstanceOfType(response, typeof(CreatedNegotiatedContentResult<string>));
            Assert.AreEqual(actualResponse.Location, "api/Measurements", "Route should be api/Measurements");
            Assert.AreEqual(actualResponse.Content, timeValue,$"Measurement should be save for {timeValue}" );


        }
               

    }
}
