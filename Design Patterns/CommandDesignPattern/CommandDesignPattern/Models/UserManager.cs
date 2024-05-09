using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandDesignPattern.Models
{

    public class UserManager
    {
        public static ObservableCollection<User> UserList = new ObservableCollection<User>()
    {
        new User {Name="Vrushabh",Email="vrushabhmagdum8108@gmail.com"},
        new User{Name="Pratik",Email="pratik@gmail.com"}
    };

        public static ObservableCollection<User> GetAllUsers()
        {
            return UserList;
        }

        public static void AddUser(User user)
        {
            UserList.Add(user);
        }
    }


}
