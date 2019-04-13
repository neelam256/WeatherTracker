using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;

namespace WeatherTracker
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// When Error occured this method will be called        
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {           
            var httpResponseException = context.Exception as HttpResponseException;
            if (httpResponseException != null)
            {
                var response = httpResponseException.Response;
                context.Response = context.Request
                    .CreateErrorResponse(response.StatusCode, response.ReasonPhrase);
            }
            else
            {
                // Unhandled errors 
                //Log stack trace
                var msg = context.Exception;
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, msg);
            }
        }
    }
}