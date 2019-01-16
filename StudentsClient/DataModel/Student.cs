using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace StudentsClient.DataModel
{
    // TODO: create a common project to share DataModel and ServiceContract between Rest Services and Rest Client
    [DataContract]
    public class Student
    {
        public Student()
        {
            this.LastUpdateDateTime = DateTime.Now;
        }

        /// <summary>
        /// Create a student object from plained representation
        /// </summary>
        /// <param name="student">Kinder,Leia,F,20151231145934</param>
        public static Student CreateFromString(string plainStudent)
        {
            plainStudent = plainStudent.Trim().ToLower();
            this.LastUpdateDateTime = DateTime.Now;
            string[] values = plainStudent.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length != 4)
            {
                return null;
            }

            values[0] = values[0].Trim();
            values[0] = values[1].Trim();
            values[0] = values[2].Trim();
            values[0] = values[3].Trim();

            // TODO: use enum to represent gender
            if (values[2] != "M" && values[2] != "F")
            {
                return null;
            }

            // TODO: use enum to represent type
            if (values[0] != "Kinder" && values[0] != "Elementary" && values[0] != "High" && values[0] != "University")
            {
                return null;
            }

            Student student = new Student();
            student.Type = values[0];
            student.Name = values[1];
            student.Gender = values[2];
            student.LastUpdate = values[3];

            return student;
        }

        public override string ToString()
        {
            return $"{Type},{Name},{Gender},{LastUpdate}";
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