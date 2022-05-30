namespace LearningManagementSystem.Domain.Models.Responses
{
    public class Response<T>
    {
        public bool IsSuccessful { get; set; }
        public T? Data { get; set; } = default!;
        public string? ErrorMessage { get; set; }

        public static Response<T> Success(T data)
        {
            return new Response<T>()
            {
                IsSuccessful = true,
                Data = data
            };
        }
        public static Response<T> Error(string error)
        {
            return new Response<T>()
            {
                IsSuccessful = false,
                ErrorMessage = error
            };
        }
    }

    public class Response
    {
        public bool IsSuccessful { get; set; }
        public string? Error { get; set; }
    }
}
