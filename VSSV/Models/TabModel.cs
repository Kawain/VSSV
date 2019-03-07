using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace VSSV.Models
{
    public class TabModel
    {
        public int Index { get; set; }
        public string Header { get; set; }
        public DataTable Content { get; set; }
    }
}
