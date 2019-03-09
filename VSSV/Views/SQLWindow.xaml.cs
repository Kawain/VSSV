using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using VSSV.Models;
using VSSV.ViewModels;

namespace VSSV.Views
{
    /// <summary>
    /// SQLWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SQLWindow : Window
    {
        private SQLWindowViewModel _vm;

        public SQLWindow(MainWindowViewModel vm)
        {
            InitializeComponent();

            _vm = new SQLWindowViewModel(vm);
            DataContext = _vm;

        }


        private void SQL_Button_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine(_vm.CurrentRowItem.SQL);
        }
    }
}
