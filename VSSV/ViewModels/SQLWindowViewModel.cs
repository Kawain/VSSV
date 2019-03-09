using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSSV.Models;

namespace VSSV.ViewModels
{
    public class SQLWindowViewModel : BindableBase
    {
        private MainWindowViewModel MWviewModel;

        //SQLの選択行
        private History _currentRowItem;
        public History CurrentRowItem
        {
            get { return _currentRowItem; }
            set { SetProperty(ref _currentRowItem, value); }
        }

        public ObservableCollection<History> Histories { get; set; }


        public SQLWindowViewModel(MainWindowViewModel vm)
        {
            MWviewModel = vm;

            Histories = MWviewModel.Histories;

        }

    }
}
