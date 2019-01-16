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
            for(int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].ToLower();
            }

            // TODO: use some library to parse arguments
            string filterCriteria;
            if (args.Length == 2 && args[1].Contains("name=")){
                string nameFilter = args[1].Replace("name=", "").ToLower();
                // 'Name like lei sortby Name'
                filterCriteria = $"Name like {nameFilter} sortby Name";
            }
            else if (args.Length== 2 && args[1].Contains("type="))
            {
                string typeFilter = args[1].Replace("type=", "").ToLower();
                // 'Type like kinder sortby LastUpdate desc'
                filterCriteria = $"Type like {typeFilter} sortby LastUpdate desc";
            }
            else if (args.Length == 3 && (args[1].Contains("type=") || args[1].Contains("gender=")) && (args[2].Contains("type=") || args[2].Contains("gender=")))
            {
                string typeFilter = args[1].Contains("type=") ? args[1].Replace("type=", "").ToLower() : args[2].Replace("type=", "").ToLower();
                string genderFilter = args[1].Contains("gender=") ? args[1].Replace("gender=", "").ToLower() : args[2].Replace("gender=", "").ToLower();
                genderFilter = genderFilter.Length >= 2 ? genderFilter.Remove(1) : genderFilter;
                // 'Type like kinder and Gender like f sortby LastUpdate desc'
                filterCriteria = $"Type like {typeFilter} and Gender like {genderFilter} sortby LastUpdate desc";
            }
            else
            {
                Console.WriteLine("Invalid number of arguments or they has been in the wrong format");
                Console.WriteLine("Valid invokation should be like:");
                Console.WriteLine("Ex: StudentSolution.exe input.csv name=leia");
                Console.WriteLine("Ex: StudentSolution.exe input.csv type=kinder");
                Console.WriteLine("Ex: StudentSolution.exe input.csv type=elementary gender=female");
                Console.WriteLine("");
                return;
            }

            string fileName = args[0];
            StudentClient studentClient = new StudentClient();
            List<Student> students = LoadStudents(studentClient, fileName);
            studentClient.SetupStudents(students);

            var filteredStudents = studentClient.Filter(filterCriteria);
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
