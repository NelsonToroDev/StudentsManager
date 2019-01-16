using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using StudentsManager.DataModel;

namespace UnitTestStudentsManager
{
    [TestClass]
    public class StudentManagerTest
    {
        List<Student> students = new List<Student>() {
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="ba2", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="a3", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="a1", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="b1", Type="Kinder", LastUpdateDateTime=DateTime.Now },
                new Student(){ Id = Guid.NewGuid(), Gender="M", Name="b2", Type="Kinder", LastUpdateDateTime=DateTime.Now }
            };

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
