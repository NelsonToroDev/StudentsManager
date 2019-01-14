using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace HostProcessInvoker
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(StudentsManager.Services.StudentService)))
            {
                host.Open();
                
                foreach (var endpoints in host.Description.Endpoints)
                {
                    logger.InfoFormat("Service up and running at: {0}", endpoints.Address);
                }

                Console.ReadLine();
                host.Close();
            }
        }
    }
}
