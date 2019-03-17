﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private SettingsViewModel _settVM;

        public SettingsView()
        {
            InitializeComponent();
            _settVM = new SettingsViewModel();
            DataContext = _settVM;
        }
    }
}
