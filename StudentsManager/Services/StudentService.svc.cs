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

namespace StudentsManager.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StudentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select StudentService.svc or StudentService.svc.cs at the Solution Explorer and start debugging.
    public class StudentService : IStudentService
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected static MemoryCache studentsCache = new MemoryCache("StudentsCache");

        public void CreateStudent(Student newStudent)
        {
            newStudent.Id = Guid.NewGuid();
            lock (studentsCache)
            {
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy()
                {
                    UpdateCallback = (cacheEntryUpdateArguments) =>
                    {
                        logger.InfoFormat("Student '{0}' was removed from cache", cacheEntryUpdateArguments.Key);
                    }
                };

                studentsCache.Set(new CacheItem(newStudent.Id.ToString(), newStudent), cacheItemPolicy);
            }
        }

        public void DeleteStudent(string studentId)
        {
            throw new NotImplementedException();
        }

        public Student GetStudent(string studentId)
        {
            Guid parsedProcessGuid = new Guid(studentId);
            logger.InfoFormat("Querying student with id '{0}'", studentId);

            //lock (studentsCache)
            //{
            //    logger.DebugFormat("studentsCache containts '{0}'", studentsCache.GetCount());
            //    if (studentsCache.Contains(parsedProcessGuid.ToString()) == false)
            //    {
            //        logger.InfoFormat("Student with id '{0}' was not found maybe it was removed from cache", studentId);
            //        throw new Exception(string.Format("Student with id '{0}' was not found maybe it was removed from cache", studentId));
            //    }

            //    Student student = studentsCache.Get(parsedProcessGuid.ToString()) as Student;
            //    if (student != null)
            //    {
            //        logger.InfoFormat("Student with id '{0}'' was found", studentId);
            //    }

            //    return student;
            //}

            return new Student() { Id = Guid.NewGuid(), Gender = "M", Name = "a", Type = "Kinder", LastUpdate = DateTime.Now };
        }

        public List<Student> SearchByName(string name, int sorting)
        {
            throw new NotImplementedException();
        }

        public List<Student> SearchByType(string studentType, int sorting)
        {
            throw new NotImplementedException();
        }

        public List<Student> SearchByTypeAndGender(string type, string geneder, int sorting)
        {
            throw new NotImplementedException();
        }
    }
}
