namespace Scandium.Model.BaseModels
{
    public abstract class BaseResponse
    {
        public BaseResponse(bool success = true, bool customError = true, List<ErrorResponseContent>? errorContents = null)
        {
            Success = success;
            CustomError = customError;
            ErrorContents = errorContents;
        }

        public bool Success { get; set; }
        public bool CustomError { get; set; }
        public List<ErrorResponseContent>? ErrorContents { get; set; }
    }

    public class ErrorResponseContent
    {
        public ErrorResponseContent(string? title, string? content)
        {
            Title = title;
            Content = content;
        }

        public string? Title { get; set; }
        public string? Content { get; set; }

    }

    public class ServiceResponse<T> : BaseResponse
    {
        public T? Value { get; set; }

        public ServiceResponse(bool success = true, bool customError = true, List<ErrorResponseContent>? errorContents = null) : base(success, customError, errorContents)
        {
        }
        public ServiceResponse(T? value, bool success = true, bool customError = true, List<ErrorResponseContent>? errorContents = null) : base(success, customError, errorContents)
        {
            Value = value;
        }
    }

    public class ServiceResponse : BaseResponse
    {

        public ServiceResponse(bool success = true, bool customError = true, List<ErrorResponseContent>? errorContents = null) : base(success, customError, errorContents)
        {
        }
    }
}