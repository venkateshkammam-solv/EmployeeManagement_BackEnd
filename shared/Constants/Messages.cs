namespace AzureFunctions_Triggers.Shared.Constants
{
    public static class Messages
    {
        // Request lifecycle
        public const string RequestReceived = "AddEmployeeDetails request received";
        public const string EmployeeCreated = "Employee record created successfully.";

        // Validation errors
        public const string InvalidRequest = "Invalid request body.";
        public const string DateOfBirthRequired = "Date of Birth is required.";
        public const string DobAgeMismatch = "Date of Birth does not match provided Age.";

        // Conflict errors
        public const string EmailAlreadyExists = "Email already exists.";
        public const string PhoneAlreadyExists = "Phone number already exists.";

        // Server errors
        public const string InternalServerError = "Internal server error.";
        public const string ProcessingError = "Error occurred while processing AddEmployeeDetails request.";

        public const string ApprovedMessage = "Approval processed successfully";

        public const string DocumentIsPendingForHumanApprovalMsg = "Document is pending human approval.";

        public const string DocumentWorkFlowCmpltMsg = "Document workflow completed.";

        public const string BlobNotFoundMsg = "Blob NOT FOUND in storage.";
        public static string FileToBase64Msg = "Error converting File to Base64.";
        public static string BirthdayEmployeeFoundMsg = "Birthday employees found.";
        public static string BirthdayEmployeeNotFoundMsg = "No employee birthdays today.";
        public static string BirthdayEmailSentMsg = "Birthday email sent to employee.";
        public static string BirthdayEmailSentErrorMsg = "Error occurred during birthday email process.";
        public static string EmployeeDocumentProcessorMsg = "EmployeeDocumentProcessor triggered for file.";
        public static string RecordAddedSuccessMsg = "Employee record added to database successfully.";
        public static string OnBoardEmailSuccessMsg = "Onboarding email sent successfully.";
        public static string ExcelProcessingMsg = "Excel processing completed.";
        public static string ExcelErrorProcessingMsg = "An error occurred during Excel data processing.";

        // Fetch operations
        public const string FetchingAllEmployeeRecords = "Fetching all employee records.";
        public const string EmployeeDataFetched = "Employee data fetched successfully.";
        public const string FetchingEmployeeById = "Fetching employee record by ID.";
        public const string EmployeeRecordRetrieved = "Record retrieved successfully.";
        public const string EmployeeRecordNotFound = "Record not found for given Id.";

        // Update operations
        public const string UpdatingEmployee = "Updating employee record.";
        public const string InvalidUpdateRequest = "Invalid request body for update operation.";
        public const string EmployeeUpdated = "Employee details updated successfully.";

        // Delete operations
        public const string EmployeeDeleted = "Employee record deleted successfully.";
        public const string DeleteSuccessMessage = "Deleted successfully";

        // Queue processing
        public const string QueueMessageReceived = "Queue message received.";
        public const string QueueDeserializationFailed = "Queue message could not be deserialized to EmployeeDetailsDto.";
        public const string ProcessingOnboardingWorkflow = "Processing onboarding workflow.";
        public const string OnboardingEmailSent = "Onboarding email sent successfully.";

      
        public const string QueueDeserializationError = "Failed to deserialize queue message.";
        public const string QueueProcessingError = "An unexpected error occurred while processing the queue message.";

        
        public const string NoFileUploaded = "No file uploaded.";
        public const string DocumentWorkflowStarted = "Document uploaded and workflow started.";
        public const string DocumentOrchestratorStarted = "Document Orchestrator started. DocumentId: {DocumentId}, InstanceId: {InstanceId}";

        
        public const string MissingContentTypeHeader = "Missing Content-Type header.";
        public const string MissingMultipartBoundary = "Missing multipart boundary.";
        public const string InvalidMultipartContentType = "Content-Type must be multipart/form-data.";

        public const string UploadDocumentError = "Error in Uploaded Document";

        public const string DocumentIdAlreadyExist = "Document ID must already exist";


    }
}
