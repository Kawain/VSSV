using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSSV.Models;

namespace VSSV.ViewModels
{
    public class SQLWindowViewModel : BindableBase
    {
        public MainWindowViewModel MWviewModel;

        //sqliteファイルパス
        private string _selectedPath;
        public string SelectedPath
        {
            get { return _selectedPath; }
            set { SetProperty(ref _selectedPath, value); }
        }

        //SQLの選択行
        private History _currentRowItem;
        public History CurrentRowItem
        {
            get { return _currentRowItem; }
            set { SetProperty(ref _currentRowItem, value); }
        }

        //SQLの履歴
        public ObservableCollection<History> Histories { get; set; }

        private bool _dialogResultFlag = false;
        public bool DialogResultFlag
        {
            get { return _dialogResultFlag; }
            set { SetProperty(ref _dialogResultFlag, value); }
        }


        public SQLWindowViewModel(MainWindowViewModel vm)
        {
            MWviewModel = vm;

            SelectedPath = MWviewModel.SelectedPath;
            Histories = MWviewModel.Histories;
        }
    }
}
