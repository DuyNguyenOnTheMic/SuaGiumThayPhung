﻿//------------------------------------------------------------------------------
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

    public class ClassDetail : BaseEntity
    {
        public int ClassDetailsId { get; set; }
        public Nullable<int> AmountSVDK { get; set; }
        public Nullable<int> Blank { get; set; }
        public Nullable<int> CapacityToContain { get; set; }
        public Nullable<int> Credits { get; set; }
        public Nullable<int> NumberOfTKB { get; set; }
        public string PH { get; set; }
        public string PH_X { get; set; }
        public string SchoolWeek2 { get; set; }
        public Nullable<int> SoTietDaXep { get; set; }
        public string StatusLHP { get; set; }
        public Nullable<int> ThuS { get; set; }
        public Nullable<int> TietS { get; set; }
        public Nullable<int> TMSH { get; set; }
        public Nullable<int> ClassId { get; set; }

        /* public virtual Class Class { get; set; }*/
    }
}