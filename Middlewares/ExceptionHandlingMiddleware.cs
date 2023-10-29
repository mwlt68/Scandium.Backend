using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Scandium.Exceptions;
using Scandium.Model.BaseModels;

namespace Scandium.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            ServiceResponse response = new ServiceResponse();
            if (ex is CustomBaseException)
            {
                context.Response.StatusCode = ex switch
                {
                    BadRequestException => StatusCodes.Status400BadRequest,
                    NotFoundException => StatusCodes.Status404NotFound,
                    InternalServerException => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError,
                };
                response.ErrorContents = ((CustomBaseException)ex).ErrorContents;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.IsCustomException = false;
                response.ErrorContents = new List<ErrorResponseContent>()
                {
                    new ErrorResponseContent("Exception",ex.Message)
                };
            }
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(response, serializerSettings);
            await context.Response.WriteAsync(json);
        }

    }
}