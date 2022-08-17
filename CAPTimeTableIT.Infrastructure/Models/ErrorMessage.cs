using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Models
{
    public class ErrorMessage
    {
        public int RowIndex { get; set; }
        public string PropertyName { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }
    }
}
