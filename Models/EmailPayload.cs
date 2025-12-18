using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions_Triggers.Models
{
    public class EmailPayload
    {
        public string Email { get; set; }
        public string EmployeeName { get; set; }
        public string Status { get; set; }
    }

}
