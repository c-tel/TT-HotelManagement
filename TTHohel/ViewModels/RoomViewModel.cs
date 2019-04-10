using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TTHohel.Contracts.Bookings;
using TTHohel.Models;
using TTHohel.Tools;
using TTHotel.Contracts.Rooms;

namespace TTHohel.ViewModels
{
    class RoomViewModel : INotifyPropertyChanged
    {

        #region Private Fields
        private RoomType _selectedType;

        private bool _isCreation;
        private bool _isEditing;

        private RoomDTO _room;

        private RoomModel Model { get; }

        private ICommand _backCommand;
        private ICommand _addRoom;
        #endregion

        public RoomViewModel()
        {
            Model = new RoomModel();
            Room = Model.GetRoom();

            OnModeChanged(Model.GetMode());
        }


        public RoomDTO Room
        {
            get { return _room; }
            set
            {
                if (_room != value)
                {
                    _room = value;
                    InvokePropertyChanged(nameof(Room));
                }
            }
        }

        public RoomType SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (_selectedType != value)
                {
                    _selectedType = value;
                    InvokePropertyChanged(nameof(SelectedType));
                }
            }
        }

        public List<RoomType> TypeList
        {
            get
            {
                return Enum.GetValues(typeof(RoomType)).Cast<RoomType>().ToList();
            }
        }

        #region Modes Properties
        public bool IsCreation
        {
            get { return _isCreation; }
            set
            {
                _isCreation = value;
                InvokePropertyChanged(nameof(IsCreation));
            }
        }

        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                InvokePropertyChanged(nameof(IsEditing));
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

        public ICommand AddRoom
        {
            get
            {
                if (_addRoom == null)
                    _addRoom = new RelayCommand<object>(AddExecute, AddCanExecute);
                return _addRoom;
            }
            set
            {
                _addRoom = value;
                InvokePropertyChanged(nameof(AddRoom));
            }
        }

        private bool AddCanExecute(object obj)
        {
            return true;
        }

        private void AddExecute(object obj)
        {
            var res = Model.CreateNewRoom(Room, SelectedType);
            if (res == 1)
            {
                MessageBox.Show("Номер створено.");
                Model.GoBack();
            }
            else if (res == 2)
            {
                MessageBox.Show("Такий номер вже є!", "Помилка");
            }
            else MessageBox.Show("Не вдалося створити номер.", "Помилка");
        }

        #endregion

        private void OnModeChanged(DisplayModes mode)
        {
            IsCreation = mode.HasFlag(DisplayModes.Creation);
            IsEditing = mode.HasFlag(DisplayModes.Editing);
        }

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
