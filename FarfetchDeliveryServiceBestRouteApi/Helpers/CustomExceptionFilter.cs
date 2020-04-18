using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace FarfetchDeliveryServiceBestRouteApi.Helpers
{
    /// <summary>
    /// Filter to get and treat the exceptions
    /// </summary>
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
            context.Result = new JsonResult(context.Exception);

            //Log error
        }
    }
}
