using log4net;
using StudentsManager.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Runtime.Caching;
using System.Web;

namespace StudentsManager.Manager
{
    public class StudentManager
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static MemoryCache studentsCache = new MemoryCache("StudentsCache");

        public static Guid CreateStudent(Student newStudent)
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

                return newStudent.Id;
            }
        }

        public static void DeleteStudent(Guid studentId)
        {
            lock (studentsCache)
            {
                if (studentsCache.Contains(studentId.ToString()) == false)
                {
                    logger.InfoFormat("Student with id '{0}' was not found maybe it was removed from cache", studentId);
                    throw new Exception(string.Format("Student with id '{0}' was not found maybe it was removed from cache", studentId));
                }

                var student = studentsCache.Remove(studentId.ToString());
                if (student != null)
                {
                    logger.InfoFormat("Student with id '{0}'' was deleted", studentId);
                }
            }
        }

        public static Student GetStudent(Guid studentId)
        {
            logger.InfoFormat("Querying student with id '{0}'", studentId);

            lock (studentsCache)
            {
                if (studentsCache.Contains(studentId.ToString()) == false)
                {
                    logger.InfoFormat("Student with id '{0}' was not found maybe it was removed from cache", studentId);
                    throw new Exception(string.Format("Student with id '{0}' was not found maybe it was removed from cache", studentId));
                }

                Student student = studentsCache.Get(studentId.ToString()) as Student;
                if (student != null)
                {
                    logger.InfoFormat("Student with id '{0}'' was found", studentId);
                }

                return student;
            }
        }

        public static List<Student> Filter(string predicate, string ordering)
        {
            List<Student> students = new List<Student>() {
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="ba2", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="a3", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="a1", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="b1", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="b2", Type="Kinder", LastUpdateDateTime=DateTime.Now }
            };

            List<Student> filteredStudents = new List<Student>();
            List<string> whereValues = new List<string>();

            string[] conditionals = predicate.Split(new[] { "and", "or"}, StringSplitOptions.RemoveEmptyEntries);
            int countArg = 0;
            foreach(string conditional in conditionals)
            {
                string[] values = conditional.Split(new[] { "==", "!=", ">", ">=", "<", "<=", "like"}, StringSplitOptions.RemoveEmptyEntries);
                string value = values[1].Trim();
                whereValues.Add(value);
                string newConditional;
                if (conditional.Contains(" like "))
                {
                    // Name like "a" => Name.StartsWith("@0")
                    newConditional = conditional.Remove(conditional.LastIndexOf(" like ")) + ".StartsWith(@" + countArg++ + ")";
                }
                else
                {
                    newConditional = conditional.Remove(conditional.LastIndexOf(value)) + "@" + countArg++;
                }

                predicate = predicate.Replace(conditional, newConditional);
            }

            try
            {
                filteredStudents = students.Where(predicate, whereValues.ToArray()).OrderBy(ordering).ToList<Student>();
            }
            catch(Exception e)
            {
                logger.Error(e);
            }

            return filteredStudents;
        }
    }
}