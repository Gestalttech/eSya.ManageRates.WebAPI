using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;
using System.IO;
using System.Text;

namespace eSya.ManageRates.WebAPI.Utility
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        //var error = context.Features.Get<IExceptionHandlerFeature>() as ExceptionHandlerFeature;
                        //var requestPath = error.Path;

                        var httpRequestFeature = context.Features.Get<IHttpRequestFeature>();
                        var requestPath = httpRequestFeature.Path;
                        var queryString = httpRequestFeature.QueryString;
                        var bodyString = "";
                        using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                        {
                            bodyString = reader.ReadToEnd();
                        }

                        //new BL_LogError().Insert_LogError(LogErrorType.Error, requestPath, CustomException.GetExceptionMessage(contextFeature.Error));

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            Status = false,
                            StatusCode = context.Response.StatusCode,
                            Message = CustomException.GetExceptionMessage(contextFeature.Error)
                        }.ToString());
                    }
                });
            });
        }
    }

    public class ErrorDetails
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
