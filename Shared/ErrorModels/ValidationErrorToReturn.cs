using System.Net;


namespace Shared.ErrorModels
{
    public class ValidationErrorToReturn
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;

        public string Message { get; set; } = "Validation Error";

        public List<ValidationError> Errors { get; set; } = [];
    }
}
