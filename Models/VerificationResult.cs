namespace AzureFunctions_Triggers.Models
{
    public class VerificationResult
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string EmployeeName {  get; set; }
        public string Email { get; set; } = string.Empty; 
        public string FileName { get; set; }
        public string Status { get; set; } = "Pending";        
        public string Message { get; set; } = string.Empty;    
        public string BlobUrl { get; set; } = string.Empty;    
        public DateTime VerifiedDate { get; set; } = DateTime.UtcNow;
    }
}
