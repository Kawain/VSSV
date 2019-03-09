using System;
using System.Collections.Generic;
using System.Dynamic;
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
            var list = DBOperation.AllColumns(@"C:\Users\user\source\repos\testdb\pk2.db", "test2", true);

            Console.WriteLine(list);
        }

        static void test5()
        {
            var list = DBOperation.AllRecords(@"C:\Users\user\source\repos\testdb\pk2.db", "test2");

            Console.WriteLine(list);

        }

        static void test6()
        {
            var list = DBOperation.AllColumns(@"C:\Users\user\source\repos\testdb\newnotes2.db", "memos", false);

            DBOperation.InsertRecord(@"C:\Users\user\source\repos\testdb\newnotes2.db", "memos", list);

        }

        static void test7()
        {
            //https://dobon.net/vb/bbs/log3-54/31793.html

            //string Name = "Taro";
            //int Id = 0;
            //var person = new { Name, Id };

            //Console.WriteLine(person.Name); // Taroと表示   
            //Console.WriteLine(person.Id);  // 0と表示

            dynamic tmp2 = new ExpandoObject();

            var dic = new Dictionary<string, object>();
            dic.Add("name", "hogehoge");
            dic.Add("age", 10);


            IDictionary<string, object> wk = tmp2;
            foreach (var item in dic)
            {
                wk.Add(item.Key, item.Value);
            }

            Console.WriteLine("----- tmp2 -----");
            Console.WriteLine(tmp2.name);
            Console.WriteLine(tmp2.age);
        }

        static void Main(string[] args)
        {
            test4();

            Console.WriteLine("終了");
            Console.Read();
        }
    }
}
