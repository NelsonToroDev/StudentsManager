using StudentsClient.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            StudentClient studentClient = new StudentClient();
            List<Student> students = LoadStudents(studentClient, "input.csv");
            studentClient.SetupStudents(students);
            var retrievedStudents = studentClient.GetStudents();
            var filteredStudents = studentClient.Filter("Name like k sortby Name desc");
            filteredStudents.ForEach(s => Console.WriteLine(s));
            Console.ReadKey();
        }

        private static List<Student> LoadStudents(StudentClient studentClient, string inputFile)
        {
            List<Student> students = new List<Student>();
            StreamReader file = new StreamReader(inputFile);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                Student st = Student.CreateFromString(line);
                if (st != null)
                {
                    students.Add(st);
                }
            }

            return students;
        }
    }
}
