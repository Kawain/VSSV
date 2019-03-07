using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSSV.Models
{
    public class SqliteMasterModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string TblName { get; set; }
        public int Rootpage { get; set; }
        public string Sql { get; set; }
    }
}
