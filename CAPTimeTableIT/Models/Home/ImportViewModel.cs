using CAPTimeTableIT.Domain;
using CAPTimeTableIT.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAPTimeTableIT.Models.Home
{
    public class ImportViewModel 
    {
        public List<string> Semesters { get; set; }
        public List<Semester> SemestersList { get; set; }

    }
}