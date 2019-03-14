using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTHohel.Manger;
using TTHohel.Services;

namespace TTHohel.Models
{
    class LoginModel
    {

        public bool Login(string login, string pwd)
        {
            if(HotelApiClient.GetInstance().Login(login, pwd))
            {
                NavigationManager.Instance.Navigate(ModesEnum.Main);
                return true;
            }
            return false;
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
