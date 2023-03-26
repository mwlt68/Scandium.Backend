namespace Scandium.Model.BaseModels
{
    public abstract class BaseResponse
    {
        public BaseResponse(bool isSuccess = true, bool isCustomException = true,bool isValidationError = false ,List<ErrorResponseContent>? errorContents = null)
        {
            IsSuccess = isSuccess;
            IsCustomException = isCustomException;
            ErrorContents = errorContents;
        }

        public bool IsSuccess { get; set; }
        public bool IsCustomException { get; set; }
        public bool IsValidationError { get; set; }
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

        public ServiceResponse(bool isSuccess = true, bool isCustomException = true,bool isValidationError = false , List<ErrorResponseContent>? errorContents = null) : base(isSuccess, isCustomException,isValidationError,errorContents)
        {
        }
        public ServiceResponse(T? value, bool isSuccess = true, bool isCustomException = true,bool isValidationError = false , List<ErrorResponseContent>? errorContents = null) : base(isSuccess, isCustomException, isValidationError,errorContents)
        {
            Value = value;
        }
    }

    public class ServiceResponse : BaseResponse
    {

        public ServiceResponse(bool isSuccess = false, bool isCustomException = true, bool isValidationError = false ,List<ErrorResponseContent>? errorContents = null) : base(isSuccess, isCustomException,isValidationError, errorContents)
        {
        }
    }
}