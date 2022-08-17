//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CAPTimeTableIT.Models
{
    using CAPTimeTableIT.Domain;
    using System;
    using System.Collections.Generic;
    
    public class TimeTable : BaseEntity
    {
        
        public string TimeTableCode { get; set; }
        public string TimeTableFile { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SemesterId { get; set; }
        public Nullable<int> UserID { get; set; }

        /*public virtual AspNetUser AspNetUser { get; set; }*/
        public virtual Semester Semester { get; set; }
        public virtual List<Class> Class { get; set; }
    }
}
