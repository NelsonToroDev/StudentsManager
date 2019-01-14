﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentsManager.DataModel;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace StudentsManager.ServiceContracts
{
    [ServiceContract]
    public interface IStudentService
    {
        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //void CreateStudents(List<Student> newStudents);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void CreateStudent(Student newStudent);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/{studentId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Student GetStudent(string studentId);

        [OperationContract]
        [WebInvoke(Method = "Delete", UriTemplate = "/{studentId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void DeleteStudent(string studentId);

        //[OperationContract]
        ////[WebInvoke(Method = "GET", UriTemplate = "/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //List<Student> SearchByName(string name, int sorting);

        //[OperationContract]
        //List<Student> SearchByType(string studentType, int sorting);

        //[OperationContract]
        //List<Student> SearchByTypeAndGender(string type, string geneder, int sorting);
    }
}