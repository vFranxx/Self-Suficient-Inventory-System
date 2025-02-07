namespace Self_Suficient_Inventory_System.LogHandling.ExceptionHandling
{
    public class UnhandledExceptionModel
    {
        public string ErrorMessage { get; set; }
        public string? StackTrace { get; set; }

        public UnhandledExceptionModel(Exception ex)
        {
            ErrorMessage = ex.Message;
            StackTrace = ex.StackTrace;
        }
    }
}
