using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using StudentsManager.DataModel;
using StudentsManager.Manager;

namespace UnitTestStudentsManager
{
    [TestClass]
    public class StudentManagerTest
    {
        [TestMethod]
        public void TestNoStudentsAfterFiltering()
        {
            List<Student> students = new List<Student>() {
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="ba2", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="a3", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="a1", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="b1", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="b2", Type="Kinder", LastUpdateDateTime=DateTime.Now }
            };

            StudentManager.ClearStudents();
            students.ForEach(s => StudentManager.CreateStudent(s));

            List<Student> filteredStudents = StudentManager.Filter("Gender like f", "Name desc");
            Assert.AreEqual(0, filteredStudents.Count);
        }

        [TestMethod]
        public void TestFilterFemaleStudent()
        {
            List<Student> students = new List<Student>() {
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="ba2", Type="kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="f", Name="a3", Type="kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="a1", Type="kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="b1", Type="kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="b2", Type="kinder", LastUpdateDateTime=DateTime.Now }
            };

            StudentManager.ClearStudents();
            students.ForEach(s => StudentManager.CreateStudent(s));

            List<Student> filteredStudents = StudentManager.Filter("Gender like f", "Name desc");
            Assert.AreEqual(1, filteredStudents.Count);
        }

        [TestMethod]
        public void TestFilterFemaleStudentInElementary()
        {
            List<Student> students = new List<Student>() {
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="ba2", Type="kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="f", Name="a3", Type="elementary", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="a1", Type="kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="b1", Type="kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="b2", Type="kinder", LastUpdateDateTime=DateTime.Now }
            };

            StudentManager.ClearStudents();
            students.ForEach(s => StudentManager.CreateStudent(s));

            List<Student> filteredStudents = StudentManager.Filter("Gender like f and Type like elementary", "Name desc");
            Assert.AreEqual(1, filteredStudents.Count);
        }

        [TestMethod]
        public void TestFilterMaleStudentsSortedByLasUpdateAsc()
        {
            List<Student> students = new List<Student>() {
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="ba2", Type="kinder", LastUpdateDateTime=DateTime.Now.AddYears(3)},
                new Student(){ Id = Guid.NewGuid(), Gender="f", Name="a3", Type="elementary", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="a1", Type="kinder", LastUpdateDateTime=DateTime.Now.AddYears(1) },
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="b1", Type="kinder", LastUpdateDateTime=DateTime.Now.AddYears(5) },
                new Student(){ Id = Guid.NewGuid(), Gender="m", Name="b2", Type="kinder", LastUpdateDateTime=DateTime.Now }
            };

            StudentManager.ClearStudents();
            students.ForEach(s => StudentManager.CreateStudent(s));

            List<Student> filteredStudents = StudentManager.Filter("Gender like m and Name like b", "LastUpdate");
            Assert.AreEqual(3, filteredStudents.Count);
            Assert.AreEqual("b2", filteredStudents[0].Name);
        }
    }
}
