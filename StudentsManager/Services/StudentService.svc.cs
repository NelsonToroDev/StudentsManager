using StudentsManager.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using StudentsManager.DataModel;
using log4net;
using System.Runtime.Caching;
using StudentsManager.Manager;
using System.Linq.Expressions;

namespace StudentsManager.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StudentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select StudentService.svc or StudentService.svc.cs at the Solution Explorer and start debugging.
    public class StudentService : IStudentService
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected static MemoryCache studentsCache = new MemoryCache("StudentsCache");

        public void SetupStudents(List<Student> students)
        {
            students.ForEach(s => StudentManager.CreateStudent(s));
        }

        public Guid CreateStudent(Student newStudent)
        {
            return StudentManager.CreateStudent(newStudent);
        }

        public void DeleteStudent(string studentId)
        {
            Guid parsedStudentId;
            if (Guid.TryParse(studentId, out parsedStudentId) == false)
            {
                logger.InfoFormat("Invalid Student id '{0}', student id should be in Guid format", studentId);
                throw new Exception(string.Format("Invalid Student id '{0}', student id should be in Guid format", studentId));
            }

            StudentManager.DeleteStudent(parsedStudentId);
        }

        public Student GetStudent(string studentId)
        {
            Guid parsedStudentId;
            if (Guid.TryParse(studentId, out parsedStudentId) == false)
            {
                logger.InfoFormat("Invalid Student id '{0}', student id should be in Guid format", studentId);
                throw new Exception(string.Format("Invalid Student id '{0}', student id should be in Guid format", studentId));
            }

            return StudentManager.GetStudent(parsedStudentId);
        }

        /// <summary>
        /// Dynamic filtering
        /// </summary>
        /// <param name="filterCriteria">In form 'Name like a sortby Name desc' 
        /// where Name is a property of Student, like the the operator followed by the other operand 'a' in this we are going to look for all Students which name starts with 'a'
        /// </param>
        /// <returns></returns>
        public List<Student> Filter(string filterCriteria)
        {
            string[] filters = filterCriteria.Replace("'", "").Replace(" sortby ", "~").Split('~');
            string predicate = filters[0];
            string ordering = filters[1];
            return StudentManager.Filter(predicate, ordering);
        }

        public List<Student> GetStudents()
        {
            return StudentManager.GetStudents();
        }
    }
}
