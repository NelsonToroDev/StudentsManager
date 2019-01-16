using log4net;
using StudentsManager.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Runtime.Caching;
using System.Web;
using System.Collections.Concurrent;

namespace StudentsManager.Manager
{
    public class StudentManager
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static List<Student> students = new List<Student>();

        public static void ClearStudents()
        {
            students.Clear();
        }

        public static Guid CreateStudent(Student newStudent)
        {
            newStudent.Id = Guid.NewGuid();
            lock (students)
            {
                students.Add(newStudent);
            }
            
            return newStudent.Id;
        }

        public static void DeleteStudent(Guid studentId)
        {
            lock (students)
            {
                if (students.Any(s => s.Id == studentId) == false)
                {
                    logger.InfoFormat("Student with id '{0}' was not found maybe it was removed from cache", studentId);
                    throw new Exception(string.Format("Student with id '{0}' was not found maybe it was removed from cache", studentId));
                }

                var deletedStudents = students.RemoveAll(s => s.Id == studentId);
                if (deletedStudents > 0)
                {
                    logger.InfoFormat("Student with id '{0}'' was deleted", studentId);
                }
            }
        }

        public static Student GetStudent(Guid studentId)
        {
            logger.InfoFormat("Querying student with id '{0}'", studentId);

            lock (students)
            {
                if (students.Any(s => s.Id == studentId) == false)
                {
                    logger.InfoFormat("Student with id '{0}' was not found maybe it was removed from cache", studentId);
                    throw new Exception(string.Format("Student with id '{0}' was not found maybe it was removed from cache", studentId));
                }

                Student student = students.First(s => s.Id == studentId) as Student;
                if (student != null)
                {
                    logger.InfoFormat("Student with id '{0}'' was found", studentId);
                }

                return student;
            }
        }

        public static List<Student> Filter(string predicate, string ordering)
        {
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

        public static List<Student> GetStudents()
        {
            lock (students)
            {
                return students;
            }
        }
    }
}