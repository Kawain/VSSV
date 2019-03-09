using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSSV.Models;
using System.Data;

namespace VSSV.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        //sqliteファイルパス
        private string _selectedPath = "";
        public string SelectedPath
        {
            get { return _selectedPath; }
            set { SetProperty(ref _selectedPath, value); }
        }

        //テーブルスキーマ
        private List<SqliteMasterModel> _sqliteMasters = new List<SqliteMasterModel>();
        public List<SqliteMasterModel> SqliteMasters
        {
            get { return _sqliteMasters; }
            set { SetProperty(ref _sqliteMasters, value); }
        }

        //動的タブ
        private ObservableCollection<TabModel> _tabItems = new ObservableCollection<TabModel>();
        public ObservableCollection<TabModel> TabItems
        {
            get { return _tabItems; }
            set { SetProperty(ref _tabItems, value); }
        }

        //現在のタブインデックス
        private int _tabIndex;
        public int TabIndex
        {
            get { return _tabIndex; }
            set
            {
                SetProperty(ref _tabIndex, value);
                TabNow();
            }
        }



        public MainWindowViewModel()
        {

        }


        //テーブル名のタブ作成
        public void MakeTabs(int index = 0)
        {

            TabItems = new ObservableCollection<TabModel>();
            SqliteMasters = DBOperation.SqliteMaster(SelectedPath);

            //テーブルのみ抽出済み
            int i = 0;
            foreach (var v in SqliteMasters)
            {
                TabItems.Add(new TabModel()
                {
                    Index = i,
                    Header = v.Name,
                    Content = null,
                });
                i++;
            }

            TabIndex = index;

        }

        //TabIndexのプロパティチェンジで動作
        private void TabNow()
        {
            if (TabIndex > -1)
            {
                TabDraw();
            }
        }

        //タブ描画
        private void TabDraw()
        {

            string tableName = TabItems[TabIndex].Header;

            //Content
            //DataTable初期化
            var table = new DataTable();

            //List<PRAGMAModel>
            var allColumns = DBOperation.AllColumns(SelectedPath, tableName, true);

            /*
             * SQLiteの型
             * NULL       NULL値
             * INTEGER    符号付整数。1, 2, 3, 4, 6, or 8 バイトで格納
             * REAL       浮動小数点数。8バイトで格納
             * TEXT       テキスト。UTF-8, UTF-16BE or UTF-16-LEのいずれかで格納
             * BLOB       Binary Large OBject。入力データをそのまま格納
             */

            //DataTableのColumns
            foreach (var v in allColumns)
            {
                //integer変換
                if (v.Type.ToLower() == "integer")
                {
                    table.Columns.Add(v.Name, typeof(int));
                }
                //double変換
                else if (v.Type.ToLower() == "real")
                {
                    table.Columns.Add(v.Name, typeof(double));
                }
                else
                {
                    table.Columns.Add(v.Name);
                }
            }

            //テーブル全レコード
            var allRecords = DBOperation.AllRecords(SelectedPath, tableName);

            foreach (var v1 in allRecords)
            {
                var row = table.NewRow();
                //DapperRowオブジェクトをキャスト
                var data = (IDictionary<string, object>)v1;
                var n = 0;
                foreach (var v2 in allColumns)
                {
                    if (v2.Type.ToLower() != "integer" && v2.Type.ToLower() != "real")
                    {
                        if (data[v2.Name] != null)
                        {
                            //改行削除
                            data[v2.Name] = data[v2.Name].ToString().Replace("\r", "").Replace("\n", "");
                            //長文を短くする
                            int len = data[v2.Name].ToString().Length;
                            if (len > 50)
                            {
                                data[v2.Name] = data[v2.Name].ToString().Substring(0, 50) + "…";
                            }
                        }
                    }
                    try
                    {
                        row[n] = data[v2.Name];
                    }
                    catch
                    {
                        row[n] = DBNull.Value;
                    }
                    n++;
                }
                table.Rows.Add(row);
            }

            TabItems[TabIndex].Content = table;

        }
    }
}
