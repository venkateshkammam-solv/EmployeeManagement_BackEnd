using System;
using System.ComponentModel.DataAnnotations;

namespace AzureFunctions_Triggers.Models
{
    public class AddEmployeeRequest
    {
        public string id { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "FullName must contain only alphabets and spaces.")]
        public string FullName { get; set; }

        
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Department { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public DateTime DateOfJoining { get; set; }

        [Required]
        public string EmploymentType { get; set; }

        [Required]
        public string ReportingManager { get; set; }

        [Required]
        public string Location { get; set; }
    }
}
