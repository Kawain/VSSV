using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSSV.Models
{
    public class History : BindableBase
    {
        private string _sQL;
        public string SQL
        {
            get { return _sQL; }
            set { SetProperty(ref _sQL, value); }
        }
    }
}
