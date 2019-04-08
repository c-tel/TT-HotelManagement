using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using TTHotel.Contracts.Auth;

namespace TTHohel.ViewModels
{
    class PersonnelViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private string _login;
        private string _password;

        private UserRoles _selectedRole;

        private UserDTO _personnel;

        private PersonnelModel Model { get; }

        private ICommand _backCommand;
        private ICommand _addPersonnel;
        #endregion

        public PersonnelViewModel()
        {
            Model = new PersonnelModel();

            Personnel = new UserDTO();
            
        }

        #region Properties
        public UserDTO Personnel
        {
            get { return _personnel; }
            set
            {
                if (_personnel != value)
                {
                    _personnel = value;
                    InvokePropertyChanged(nameof(Personnel));
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    InvokePropertyChanged(nameof(Password));
                }
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    InvokePropertyChanged(nameof(Login));
                }
            }
        }

        public UserRoles SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                InvokePropertyChanged(nameof(SelectedRole));
            }
        }

        public List<DescriptionValueBinderPersonnel> RolesList
        {
            get
            {
                return Enum.GetValues(typeof(UserRoles)).Cast<Enum>().Select(value => new
                DescriptionValueBinderPersonnel
                {
                    Description = (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute))
                    as DescriptionAttribute).Description,
                    Value = (UserRoles)value
                }).ToList();
            }
        }
        #endregion

        #region Commands
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                    _backCommand = new RelayCommand<object>(BackExecute, BackCanExecute);
                return _backCommand;
            }
            set
            {
                _backCommand = value;
                InvokePropertyChanged(nameof(BackCommand));
            }
        }

        private bool BackCanExecute(object obj)
        {
            return true;
        }

        private void BackExecute(object obj)
        {
            Model.GoBack();
        }

        public ICommand AddPersonnel
        {
            get
            {
                if (_addPersonnel == null)
                    _addPersonnel = new RelayCommand<object>(AddPersonnelExecute, AddPersonnelCanExecute);
                return _addPersonnel;
            }
            set
            {
                _addPersonnel = value;
                InvokePropertyChanged(nameof(AddPersonnel));
            }
        }

        private bool AddPersonnelCanExecute(object obj)
        {
            return !(string.IsNullOrEmpty(Personnel.Name) ||
                    string.IsNullOrEmpty(Personnel.Surname) ||
                    string.IsNullOrEmpty(Personnel.EmplBook) ||
                    string.IsNullOrEmpty(Personnel.Patronym) ||
                    string.IsNullOrEmpty(Personnel.TelNumber) ||
                    string.IsNullOrEmpty(Password) ||
                    string.IsNullOrEmpty(Login));
        }

        private void AddPersonnelExecute(object obj)
        {
            //var res = Model.CreateNewClient(Client);
            //if (res == 1)
            //{
            //    MessageBox.Show("Персонал створено.");
            //    Model.GoBack();
            //}
            //else if (res == 2)
            //{
            //    MessageBox.Show("Персонал з такою трудовою книжкою вже є!", "Помилка");
            //}
            //else MessageBox.Show("Не вдалося створити клієнта", "Помилка");
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
        #endregion
    }
}
