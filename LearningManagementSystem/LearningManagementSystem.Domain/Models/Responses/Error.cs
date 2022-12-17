namespace LearningManagementSystem.Domain.Models.Responses
{
    public class Error
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
    public enum ErrorCode
    {
        NotFound = 404,
        BadRequest = 400,
        InternalServerError = 500,
        Conflict = 409
    }
}
