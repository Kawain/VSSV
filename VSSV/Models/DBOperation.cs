using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSSV.Models
{
    public class DBOperation
    {

        /// <summary>
        /// 空のデータベースファイルを作成
        /// </summary>
        /// <param Name="path"></param>
        /// <returns></returns>
        public static void CreateDatabase(string path)
        {
            using (var conn = new SQLiteConnection("Data Source=" + path))
            {
                conn.Open();
            }
        }

        /// <summary>
        /// データベースのテーブルを返す
        /// ★ファイルがSQLiteファイルかどうかの確認
        /// </summary>
        /// <param Name="path"></param>
        /// <returns></returns>
        public static List<SqliteMasterModel> SqliteMaster(string path)
        {
            var list = new List<SqliteMasterModel>();

            string sql = "SELECT * FROM sqlite_master WHERE Type='table' ORDER BY Name;";

            using (var conn = new SQLiteConnection("Data Source=" + path))
            {
                try
                {
                    list = conn.Query<SqliteMasterModel>(sql).ToList();

                }
                catch
                {
                    throw new Exception("SQLiteファイルではありません");
                }
            }

            return list;
        }

        /// <summary>
        /// テーブルのカラム名を返す
        /// </summary>
        /// <param Name="path"></param>
        /// <param Name="table"></param>
        /// <returns></returns>
        public static List<PRAGMAModel> AllColumns(string path, string table, bool f)
        {
            var list = new List<PRAGMAModel>();

            string sql = $"PRAGMA table_info({table});";

            using (var conn = new SQLiteConnection("Data Source=" + path))
            {
                list = conn.Query<PRAGMAModel>(sql).ToList();
            }

            if (f)
            {
                //ROWIDを先頭に追加
                var rowid = new PRAGMAModel()
                {
                    Name = "ROWID",
                    Type = "INTEGER"
                };

                list.Insert(0, rowid);

            }

            return list;

        }

        /// <summary>
        /// それぞれのテーブルで行列全部返す
        /// </summary>
        /// <param Name="path"></param>
        /// <param Name="table"></param>
        /// <returns></returns>
        public static List<object> AllRecords(string path, string table)
        {
            var list = new List<object>();

            string sql = $"SELECT rowid as ROWID, * FROM {table};";

            using (var conn = new SQLiteConnection("Data Source=" + path))
            {
                list = conn.Query<dynamic>(sql).ToList();
            }

            return list;
        }

        /// <summary>
        /// ダブルクリックされたレコードを取得
        /// </summary>
        /// <param Name="path"></param>
        /// <param Name="table"></param>
        /// <param Name="rowid"></param>
        /// <returns></returns>
        public static dynamic OneRecord(string path, string table, int rowid)
        {
            dynamic obj;

            string sql = $"SELECT * FROM {table} WHERE rowid = {rowid};";

            using (var conn = new SQLiteConnection("Data Source=" + path))
            {
                obj = conn.QuerySingle(sql);
            }

            return obj;
        }
    }
}
