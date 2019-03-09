using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSSV.Models;

namespace TestApp
{
    class Program
    {
        static void test1()
        {
            DBOperation.CreateDatabase(@"C:\Users\user\source\repos\testdb\02.db");
        }

        static void test2()
        {
            var list = DBOperation.SqliteMaster(@"C:\Users\user\source\repos\testdb\newnotes.db");
            foreach (var v in list)
            {
                Console.WriteLine(v.Type);
                Console.WriteLine(v.Name);
            }
        }

        static void test4()
        {
            var list = DBOperation.AllColumns(@"C:\Users\user\source\repos\testdb\newnotes2.db", "memos", true);

            Console.WriteLine(list);
        }

        static void test5()
        {
            var list = DBOperation.AllRecords(@"C:\Users\user\source\repos\testdb\newnotes2.db", "categories");

            Console.WriteLine(list);

        }

        static void Main(string[] args)
        {
            test5();

            Console.WriteLine("終了");
            Console.Read();
        }
    }
}
