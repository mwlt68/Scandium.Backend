using FastEndpoints;
using Scandium.Model.BaseModels;

namespace Scandium.Actions
{
    public class FastEndpointsAction
    {
        public static Action<Config> GetConfigActions => c =>
            {
                c.Errors.ResponseBuilder = (failures, ctx) =>
                {
                    return new ServiceResponse()
                    {
                        IsCustomException = false,
                        IsSuccess = false,
                        IsValidationError = true,
                        ErrorContents = failures.Select(x =>
                            new ErrorResponseContent(x.PropertyName, x.ErrorMessage)
                        ).ToList()
                    };
                };
            };
    }
}