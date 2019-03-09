using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VSSV.Models;
using VSSV.ViewModels;

namespace VSSV.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainWindowViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new MainWindowViewModel();
            DataContext = _vm;

        }

        /// Drag and drop files into WPF
        /// https://stackoverflow.com/questions/5662509/drag-and-drop-files-into-wpf
        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (File.Exists(files[0]))
                {
                    _vm.SelectedPath = files[0];
                    SelectedPathChange();
                }
            }
        }

        //新規作成
        private void MenuItem_Click_Create(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Title = "SQLiteファイルを新規作成";
            dialog.Filter = "SQLiteファイル|*.sqlite|SQLiteファイル|*.sqlite3|SQLiteファイル|*.db|All Files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                _vm.SelectedPath = dialog.FileName;
                DBOperation.CreateDatabase(_vm.SelectedPath);
            }
        }

        //開く
        private void MenuItem_Click_Open(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = "SQLiteファイルを開く";
            dialog.Filter = "SQLiteファイル|*.sqlite;*.sqlite3;*.db|All Files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                _vm.SelectedPath = dialog.FileName;
                SelectedPathChange();
            }
        }

        //終了
        private void MenuItem_Click_End(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //sqlite3.exe
        private void MenuItem_Click_Sqlite3(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedPath == "")
            {
                MessageBox.Show("SQLiteファイルが選択されていません");
                return;
            }

            //sqlite-tools-win32-x86 の sqlite3.exe をそのまま使用
            //細かいことは sqlite3.exe のコマンドラインで行ってくださいという仕様
            ProcessStartInfo info = new ProcessStartInfo("cmd.exe");
            // /kは実行後にもコマンドプロンプトが残る　/cは実行後に消える
            info.Arguments = $@"/c ..\..\..\sqlite3.exe {_vm.SelectedPath}";
            Process p = Process.Start(info);
            //終了まで待機
            p.WaitForExit();
            //終了したら再読込
            SelectedPathChange();

        }

        //ファイルが設定された直後
        public void SelectedPathChange()
        {
            try
            {
                _vm.SqliteMasters = DBOperation.SqliteMaster(_vm.SelectedPath);
                if (_vm.SqliteMasters.Count > 0)
                {
                    //テーブルがある場合
                    _vm.MakeTabs();
                }
                else
                {
                    _vm.TabItems = new ObservableCollection<TabModel>();
                    MessageBox.Show("テーブルがありません");
                }
            }
            catch (Exception ex)
            {
                _vm.TabItems = new ObservableCollection<TabModel>();
                _vm.SelectedPath = "";
                MessageBox.Show(ex.Message);
            }
        }

        //タブ内のDataGrid_MouseDoubleClick
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid == null)
            {
                return;
            }
            //https://plaza.rakuten.co.jp/pirorin55/diary/201602290001/
            //MessageBox.Show(dataGrid.Columns[0].Header.ToString() + ": " +((TextBlock)dataGrid.Columns[0].GetCellContent(dataGrid.SelectedItem)).Text);

            //ファイル
            string path = _vm.SelectedPath;
            //テーブル名
            string table = _vm.TabItems[_vm.TabIndex].Header;

            if (dataGrid.SelectedItem == null)
            {
                return;
            }
            //rowid
            int rowid = Convert.ToInt32(((TextBlock)dataGrid.Columns[0].GetCellContent(dataGrid.SelectedItem)).Text);

            Console.WriteLine(rowid);

            SubWindow win = new SubWindow(path, table, rowid);
            win.ShowDialog();

        }

        //新規追加ウインドウ
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ファイル
            string path = _vm.SelectedPath;
            //テーブル名
            string table = _vm.TabItems[_vm.TabIndex].Header;

            SubWindow win = new SubWindow(path, table);
            win.ShowDialog();
            //閉じたら
            _vm.MakeTabs(_vm.TabIndex);
        }
    }
}
