using CommandDesignPattern.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using UserServiceReference;
using User = UserServiceReference.User;

namespace CommandDesignPatternWPF.DatabaseService
{
    public class DataBaseService
    {
        public string? ConnectionString = "Data Source=NSSEZ128\\SQLEXPRESS;Initial Catalog=master;Integrated Security=True;";

        public List<User> GetUserList()
        {
            List<User> getAllUsers = new List<User>();
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            var endPointAddress = new EndpointAddress("http://localhost:51393/UserService.svc");
            UserServiceClient proxy = new UserServiceClient(binding, endPointAddress);
            try
            {
                getAllUsers = proxy.GetAllUsers();              
            }
            catch (Exception ex)
            {

            }
            return getAllUsers;
        }

        public int AddUser(User user)
        {
            int dataInserted = 0;
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            var endPointAddress = new EndpointAddress("http://localhost:51393/UserService.svc");
            UserServiceClient proxy = new UserServiceClient(binding, endPointAddress);
            try
            {
                dataInserted = proxy.AddUser(user.Name,user.Email);

                proxy.Close();
            }
            catch (Exception ex)
            {

            }
            return dataInserted;
        }

    }
}
