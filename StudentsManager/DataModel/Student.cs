using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentsManager.DataModel
{
    [DataContract]
    public class Student
    {
        public Student()
        {
            this.LastUpdateDateTime = DateTime.Now;
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public string LastUpdate
        {
            get
            {
                return LastUpdateDateTime.ToString("yyyyMMddHHmmss");
            }
            set
            {
                LastUpdateDateTime = DateTime.ParseExact(value, "yyyyMMddHHmmss", null);
            }
        }

        [IgnoreDataMember]
        public DateTime LastUpdateDateTime { get; set; }
    }
}