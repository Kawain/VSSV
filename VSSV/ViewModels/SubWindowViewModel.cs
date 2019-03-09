using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSSV.Models;

namespace VSSV.ViewModels
{
    public class SubWindowViewModel : BindableBase
    {
        //sqliteファイルパス
        private string _selectedPath;
        public string SelectedPath
        {
            get { return _selectedPath; }
            set { SetProperty(ref _selectedPath, value); }
        }

        private string _table;
        public string Table
        {
            get { return _table; }
            set { SetProperty(ref _table, value); }
        }

        private int _rowid;
        public int RowID
        {
            get { return _rowid; }
            set { SetProperty(ref _rowid, value); }
        }

        private List<PRAGMAModel> _columns;
        public List<PRAGMAModel> Columns
        {
            get { return _columns; }
            set { SetProperty(ref _columns, value); }
        }

        private bool _newBtn;
        public bool NewBtn
        {
            get { return _newBtn; }
            set { SetProperty(ref _newBtn, value); }
        }

        private bool _editBtn;
        public bool EditBtn
        {
            get { return _editBtn; }
            set { SetProperty(ref _editBtn, value); }
        }

        private bool _dialogResultFlag = false;
        public bool DialogResultFlag
        {
            get { return _dialogResultFlag; }
            set { SetProperty(ref _dialogResultFlag, value); }
        }



        public SubWindowViewModel(string path, string table, int rowid)
        {
            SelectedPath = path;
            Table = table;
            RowID = rowid;

            //List<PRAGMAModel>
            Columns = DBOperation.AllColumns(SelectedPath, Table, false);

            //新規追加
            if (rowid == 0)
            {
                NewBtn = true;
                EditBtn = false;

                foreach (var v in Columns)
                {
                    v.Content = "";
                }
            }
            //編集削除
            else
            {
                NewBtn = false;
                EditBtn = true;

                dynamic one = DBOperation.OneRecord(SelectedPath, Table, RowID);

                var data = (IDictionary<string, object>)one;

                foreach (var v in Columns)
                {

                    foreach (KeyValuePair<string, object> pair in data)
                    {
                        if (v.Name == pair.Key)
                        {
                            try
                            {
                                v.Content = pair.Value.ToString();
                            }
                            catch
                            {
                                v.Content = "";
                            }
                        }
                    }
                }
            }

        }
    }
}
