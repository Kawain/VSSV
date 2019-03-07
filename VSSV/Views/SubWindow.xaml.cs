﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VSSV.ViewModels;

namespace VSSV.Views
{
    /// <summary>
    /// SubWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SubWindow : Window
    {
        private SubWindowViewModel _vm;

        public SubWindow(string path, string table, int rowid = 0)
        {
            InitializeComponent();

            _vm = new SubWindowViewModel(path, table, rowid);
            DataContext = _vm;

        }
    }
}
