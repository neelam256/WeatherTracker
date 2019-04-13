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
    public class MeasurementsController_Get_Test
    {
        [TestMethod]
        public void GetMeasurementsForTime_BadRequest_ForNullDate()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act            
            var response = measurementController.GetMeasurementsForTime(null);
            var actualResponse = response as BadRequestErrorMessageResult;


            //Assert
            Assert.IsNotNull(actualResponse);
         
            Assert.AreEqual(actualResponse.Message, "Input date is not in correct Format");

        }

        [TestMethod]
        public void GetMeasurementsForTime_BadRequest_ForFackDate()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act      
            
            var response = measurementController.GetMeasurementsForTime("6354635");
            var actualResponse = response as BadRequestErrorMessageResult;
            

            //Assert
            Assert.IsNotNull(actualResponse);           
            Assert.AreEqual(actualResponse.Message, "Input date is not in correct Format");

        }

        [TestMethod]
        public void GetMeasurementsForTime_NowDate_ForDate()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act            
            MeasurementsRepository.ClearAllMeasurements();

            var response = measurementController.GetMeasurementsForTime(DateTime.Now.ToShortDateString());
            var actualResponse = response as NotFoundResult;
           

            //Assert
            Assert.IsNotNull(actualResponse);

        }

        [TestMethod]
        public void GetMeasurementsForTime_ProperData_ForDate()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act  
            MeasurementsRepository.ClearAllMeasurements();

            var timeValue = DateTime.UtcNow.ToString();
            MockData.SaveMeasurement(timeValue);
            var response = measurementController.GetMeasurementsForTime(timeValue);
            var actualResponse = response as OkNegotiatedContentResult<IList<Measurement>>;
            var returnValue  = actualResponse.Content as IList<Measurement>;

            //Assert
            Assert.IsNotNull(actualResponse);
            Assert.IsTrue(returnValue.Count== 1, "Return value should be 1 because Added one measurement.");

        }

        [TestMethod]
        public void GetMeasurementsForTime_InRange_ForDate()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act  
            MeasurementsRepository.ClearAllMeasurements();

            var fromtimeValue = DateTime.UtcNow;
            MockData.SaveMeasurement(fromtimeValue.ToString());

            var totimeValue = DateTime.UtcNow.AddSeconds(3);
            MockData.SaveMeasurement(totimeValue.ToString());

            var response = measurementController.GetMeasurementsForTime(fromtimeValue.ToString(), null);
            var actualResponse = response as OkNegotiatedContentResult<IList<Measurement>>;
            var returnValue = actualResponse.Content as IList<Measurement>;

            //Assert
            Assert.IsNotNull(actualResponse);
            Assert.IsTrue(returnValue.Count == 2, "Return value should be 2 because Added 2 measurement with in time range");

        }


        [TestMethod]
        public void GetMeasurementsForTime_Particular_ForDate()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act  
            MeasurementsRepository.ClearAllMeasurements();

            var fromtimeValue = DateTime.UtcNow;
            MockData.SaveMeasurement(fromtimeValue.ToString());

            var totimeValue = DateTime.UtcNow.AddSeconds(3);
            MockData.SaveMeasurement(totimeValue.ToString());

            var response = measurementController.GetMeasurementsForTime(fromtimeValue.ToString(), totimeValue.ToString());
            var actualResponse = response as OkNegotiatedContentResult<IList<Measurement>>;
            var returnValue = actualResponse.Content as IList<Measurement>;

            //Assert
            Assert.IsNotNull(actualResponse);
            Assert.IsTrue(returnValue.Count == 1, "Count should be 1");

        }


        [TestMethod]
        public void GetMeasurementsForTime_NoValue_ForDate()
        {
            // Arrange
            MeasurementsController measurementController = new MeasurementsController();

            // Act  
            MeasurementsRepository.ClearAllMeasurements();

            var fromtimeValue = DateTime.UtcNow;
            MockData.SaveMeasurement(fromtimeValue.ToString());

           

            var totimeValue = DateTime.UtcNow.AddSeconds(3);
            MockData.SaveMeasurement(totimeValue.ToString());

            var response = measurementController.GetMeasurementsForTime(fromtimeValue.ToString(), totimeValue.AddHours(1).ToString());
            var actualResponse = response as NotFoundResult;
          
            //Assert
            Assert.IsNotNull(actualResponse);
           

        }

    

    }
}
