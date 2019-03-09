using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSSV.Models
{
    public class PRAGMAModel
    {
        public int Cid { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Notnull { get; set; }
        public string DfltValue { get; set; }
        public int Pk { get; set; }
        //これはSubWindowのみ
        public string Content { get; set; }
    }
}
