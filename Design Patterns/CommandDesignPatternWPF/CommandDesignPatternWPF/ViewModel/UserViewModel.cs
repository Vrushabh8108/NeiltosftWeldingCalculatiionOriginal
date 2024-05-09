using CommandDesignPattern;
using CommandDesignPattern.Models;
using CommandDesignPatternWPF.DatabaseService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UserServiceReference;

namespace CommandDesignPatternWPF.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public string? Name { get; set; }
        public string? Email { get; set; }

        private ObservableCollection<User1> _user { get; set; }
        public ObservableCollection<User1> Users
        {
            get { return _user; }
            set
            {
                _user = value;
                NotifyPropertyChanged("Users");
            }
        }

        public ICommand AddUserCommand {  get; set; }

        public UserViewModel() { 
            DataBaseService userService=new DataBaseService();
            Users = new ObservableCollection<User1>(userService.GetUserList().Select(x => new User1 { Name = x.Name, Email = x.Email }).ToList());
            AddUserCommand = new RelayCommand(AddUser,CanAddUser);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanAddUser(object obj)
        {
           return true;
        }

        private void AddUser(object obj)
        {
            DataBaseService userService = new DataBaseService();
            int dataInserted=userService.AddUser(new User { Name = Name,Email=Email });
            if(dataInserted>0)
            {
                MessageBox.Show("Added SuccessFully");
                Users = new ObservableCollection<User1>( userService.GetUserList().Select(x=>new User1 { Name=x.Name,Email=x.Email}).ToList()); 

            }
            else
            {
                MessageBox.Show("User Not Added");

            }

        }

        //private void RemoveUser(object obj)
        //{
        //    UserManager.UserList.Remove(new User)
        //}
    }
}
