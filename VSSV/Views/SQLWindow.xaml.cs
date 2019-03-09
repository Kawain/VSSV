using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

        //SQL実行ボタン
        private void SQL_Button_Click(object sender, RoutedEventArgs e)
        {
            //前方の空白文字を削除
            _vm.CurrentRowItem.SQL = _vm.CurrentRowItem.SQL.TrimStart();

            //SELECTに前方一致するか（大文字・小文字を区別しない）
            bool b = _vm.CurrentRowItem.SQL.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase);

            try
            {
                if (b)
                {
                    var list = DBOperation.SQLQuery(_vm.SelectedPath, _vm.CurrentRowItem.SQL);
                    if (list.Count > 0)
                    {
                        CreateDataTable(list);
                    }
                }
                else
                {
                    DBOperation.SQLExecute(_vm.SelectedPath, _vm.CurrentRowItem.SQL);
                    _vm.DialogResultFlag = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //DataTable作成
        private void CreateDataTable(List<object> list)
        {
            var table = new DataTable();

            //ここでは全部string

            //DataTableの列
            //DapperRowオブジェクトをキャスト
            var col = (IDictionary<string, object>)list[0];
            foreach (var v in col)
            {
                table.Columns.Add(v.Key);
            }

            //DataTableの行
            foreach (var v1 in list)
            {
                var data = (IDictionary<string, object>)v1;
                var row = table.NewRow();
                int i = 0;
                foreach (var v2 in data)
                {
                    if (v2.Value == null)
                    {
                        row[i] = "";
                    }
                    else
                    {
                        row[i] = v2.Value.ToString();
                    }
                    i++;
                }
                table.Rows.Add(row);
            }

            SelectDataGrid.DataContext = table;
            SelectDataGrid.Items.Refresh();

        }

        //ウインドウを閉じる直前
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = _vm.DialogResultFlag;
        }
    }
}
