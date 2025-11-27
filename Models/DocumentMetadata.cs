namespace AzureFunctions.Models
{
    public class DocumentMetadata
    {
        public string id { get; set; } = string.Empty;
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string? DocumentType { get; set; }
        public string? Description { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; }
        public string? EffectiveDate { get; set; }   
        public string FileName { get; set; } = string.Empty;
        public string TempFilePath { get; set; } = string.Empty;
        public string? UploadedBlobUrl { get; set; }
        public string Status { get; set; } = "Pending Verification";
        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;
        public string type { get; set; } = "Document";

    }
}
