namespace Self_Suficient_Inventory_System.Models.LogModels
{
    public class ResponseLogEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string RequestUrl { get; set; }
        public string HttpMethod { get; set; }
        public int? StatusCode { get; set; }
        public string? ResponseBody { get; set; }
        public string? Headers { get; set; }
        public string? UserId { get; set; }
        public string? Origin { get; set; }
    }
}
