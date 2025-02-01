namespace API.Models.LogModels
{
    public class ExceptionLogEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string? ExceptionType { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public int? StatusCode { get; set; }
        public string? RequestUrl { get; set; }
        public string? DeveloperMessage { get; set; }
        public string? UserId { get; set; }
        public string? Origin { get; set; }
    }
}