using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTHohel.Manger;

namespace TTHohel.Models
{
    class LoginModel
    {
        public void Login(string login)
        {
            NavigationManager.Instance.Navigate(ModesEnum.Main);
            //if (!_storage.Users.ContainsKey(login))
            //{
            //    MessageBox.Show("Login or password is wrong");
            //}
            //else
            //{
            //    if (_storage.Users[login].Password != password)
            //    {
            //        MessageBox.Show("Login or password is wrong");
            //    }
            //    else
            //    {
            //        NavigationManager.Instance.Navigate(ModesEnum.Main);
            //    }
            //}
        }
    }
}
