using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions_Triggers.Models
{
    public class ApprovalResult
    {
        public string InstanceId { get; set; }
        public bool IsApproved { get; set; }
        public string ReviewedBy { get; set; } = "Venkatesh Kammam";
        public string Comments { get; set; }
    }

}
