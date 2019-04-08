using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TTHohel.Models;
using TTHotel.Contracts.Auth;

namespace TTHohel.ViewModels
{
    class PersonnelViewModel
    {
        #region Private Fields
        private UserDTO _personnel;

        private PersonnelModel Model { get; }

        private ICommand _backCommand;
        private ICommand _addPersonnel;
        #endregion
    }
}
