using StudentsClient.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudentsClient
{
    public class StudentClient
    {
        // TODO: move to appsettings
        private string endPoint = "http://localhost:8000/students/";

        private TResp SendDataToServer<TSend, TResp>(string endpoint, string method, TSend data)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = method;
            
            if(typeof(TSend) != typeof(Object))
            {
                var serializer = new DataContractJsonSerializer(typeof(TSend));
                var requestStream = request.GetRequestStream();
                serializer.WriteObject(requestStream, data);
                requestStream.Close();
            }

            var response = request.GetResponse();
            var responseObject = default(TResp);

            if (response.ContentLength == 0)
            {
                response.Close();
                return responseObject;
            }

            if (typeof(TResp) != typeof(Object))
            {
                // Deserialize from JSON
                var respSerializer = new DataContractJsonSerializer(typeof(TResp));
                var responseStream = response.GetResponseStream();
                responseObject = (TResp)respSerializer.ReadObject(responseStream);
                responseStream.Close();
            }

            return responseObject;
        }

        private TResp GetDataFromServer<TResp>(string endpoint)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            request.Accept = "application/json";
            request.ContentType = "application/json";

            var response = request.GetResponse();

            if (response.ContentLength == 0)
            {
                response.Close();
                return default(TResp);
            }

            // Deserialize from JSON
            var respSerializer = new DataContractJsonSerializer(typeof(TResp));
            var responseStream = response.GetResponseStream();
            var responseObject = (TResp)respSerializer.ReadObject(responseStream);

            responseStream.Close();
            return responseObject;
        }

        public void SetupStudents(List<Student> students)
        {
            SendDataToServer<List<Student>, Object>(endPoint, "PUT", students);
        }

        public Guid CreateStudent(Student newStudent)
        {
            return SendDataToServer<Student, Guid>(endPoint, "POST", newStudent);
        }

        public Student GetStudent(string studentId)
        {
            return GetDataFromServer<Student>(endPoint + studentId);
        }

        public void DeleteStudent(string studentId)
        {
            SendDataToServer<Object, Object>(endPoint + studentId, "DELETE", null);
        }

        public List<Student> GetStudents()
        {
            return GetDataFromServer<List<Student>>(endPoint);
        }

        public List<Student> Filter(string filterCriteria)
        {
            return GetDataFromServer<List<Student>>($"{endPoint}?filter='{filterCriteria}'");
        }
    }
}
