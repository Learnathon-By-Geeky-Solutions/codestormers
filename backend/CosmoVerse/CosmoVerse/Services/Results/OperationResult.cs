namespace CosmoVerse.Services.Results
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int? ErrorCode { get; set; }
    }
}
