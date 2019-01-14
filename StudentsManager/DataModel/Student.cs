using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsManager.DataModel
{
    public class Student
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}