using System;
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
        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void SetupStudents(List<Student> students);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Guid CreateStudent(Student newStudent);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/{studentId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Student GetStudent(string studentId);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/{studentId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void DeleteStudent(string studentId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/?filter={filterCriteria}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Student> Filter(string filterCriteria);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Student> GetStudents();

        //[OperationContract]
        ////[WebInvoke(Method = "GET", UriTemplate = "/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //List<Student> SearchByName(string name, int sorting);

        //[OperationContract]
        //List<Student> SearchByType(string studentType, int sorting);

        //[OperationContract]
        //List<Student> SearchByTypeAndGender(string type, string geneder, int sorting);
    }
}