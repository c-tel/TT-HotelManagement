using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TTHohel.Manager;
using TTHohel.Models;
using TTHohel.Tools;
using TTHotel.Contracts.Cleanings;

namespace TTHohel.ViewModels
{
    public class MaidMainViewModel: INotifyPropertyChanged
    {
        private ICommand _cleanCommand;
        public MaidMainModel Model { get; private set; }

        public MaidMainViewModel()
        {
            Model = new MaidMainModel();
            ReinitCleanings();
        }


        private void ReinitCleanings()
        {
            Cleanings = new ObservableCollection<CleaningDTO>(Model.GetCleanings()); 
        }

        public ICommand CleanCommand
        {
            get
            {
                if (_cleanCommand == null)
                {
                    _cleanCommand = new RelayCommand<object>(CleanExecute, CleanCanExecute);
                }
                return _cleanCommand;
            }
            set
            {
                _cleanCommand = value;
                InvokePropertyChanged(nameof(CleanCommand));
            }
        }

        private void CleanExecute(object obj)
        {
            Model.CreateCleaning(SelectedCleaning);
            ReinitCleanings();
        }

        private bool CleanCanExecute(object obj) => SelectedCleaning != null;

        #region ViewParams
        private CleaningDTO _selectedCleaning;
        public CleaningDTO SelectedCleaning
        {
            get => _selectedCleaning;
            set
            {
                _selectedCleaning = value;
                InvokePropertyChanged(nameof(SelectedCleaning));
            }
        }

        private ObservableCollection<CleaningDTO> _cleanings;
        private ICommand _exitCommand;

        public ObservableCollection<CleaningDTO> Cleanings
        {
            get => _cleanings;
            set
            {
                _cleanings = value;
                InvokePropertyChanged(nameof(Cleanings));
            }
        }

        #endregion

        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand<object>(_ => NavigationManager.Instance.Navigate(ModesEnum.Login), _ => true);
                }
                return _exitCommand;
            }
            set
            {
                _exitCommand = value;
                InvokePropertyChanged(nameof(ExitCommand));
            }
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

