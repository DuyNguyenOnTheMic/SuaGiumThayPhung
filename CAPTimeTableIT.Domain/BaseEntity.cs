using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Domain
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public BaseEntity()
        {
            CreatedDateTime = DateTime.UtcNow;
            LastModifiedDateTime = DateTime.UtcNow;
        }
    }
}
