using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Models
{
    public class ValidationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool HasConfirm { get; set; }
    }
}
