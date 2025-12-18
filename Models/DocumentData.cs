
namespace AzureFunctions_Triggers.Models
{
    public class DocumentData
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string InstanceId { get; set; }
        public string EmployeeName { get; set; }
        public string DocumentType { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string EffectiveDate { get; set; }
        public string FileName { get; set; }
        public string TempFilePath { get; set; }
        public string UploadedBlobUrl { get; set; }
        public string  base64String { get; set; }
        public string SasUri { get; set; }
        public string Status { get; set; }
        public DateTime UploadedOn { get; set; }
        public string Type { get; set; }
    }
}
