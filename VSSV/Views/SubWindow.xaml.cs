using System;
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
using VSSV.Models;
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

        //削除
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        //編集
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBOperation.UpdateRecord(_vm.SelectedPath, _vm.Table, _vm.Columns, _vm.RowID);
                _vm.DialogResultFlag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //新規追加
        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBOperation.InsertRecord(_vm.SelectedPath, _vm.Table, _vm.Columns);
                _vm.DialogResultFlag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //ウインドウを閉じる直前
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = _vm.DialogResultFlag;
        }
    }
}
