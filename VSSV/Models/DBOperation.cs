using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Dynamic;
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
                    Name = "__ROWID__",
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

            //rowid というカラム名は使ってそうなので__ROWID__にしたが奇跡的に重複すれば不具合がでる
            string sql = $"SELECT _ROWID_ as __ROWID__, * FROM {table};";

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

            string sql = $"SELECT * FROM {table} WHERE _ROWID_ = {rowid};";

            using (var conn = new SQLiteConnection("Data Source=" + path))
            {
                obj = conn.QuerySingle(sql);
            }

            return obj;
        }


        /// <summary>
        /// 匿名型作成
        /// https://dobon.net/vb/bbs/log3-54/31793.html
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private static object CreateAnonymousType(string mode, List<PRAGMAModel> models)
        {

            dynamic tmp = new ExpandoObject();
            var dic = new Dictionary<string, object>();
            //内容取得
            foreach (var v in models)
            {
                var t = TypeClassification.Judge(v.Type);

                if (t == typeof(int))
                {
                    try
                    {
                        if (v.Pk > 0 && v.Content == "" && mode == "insert")
                        {
                            dic.Add(v.Name, null);
                        }
                        else if (v.Content == "")
                        {
                            dic.Add(v.Name, null);
                        }
                        else
                        {
                            dic.Add(v.Name, int.Parse(v.Content));
                        }
                    }
                    catch
                    {
                        throw new Exception("int.Parse 失敗：" + v.Name);
                    }
                }
                else if (t == typeof(double))
                {
                    try
                    {
                        if (v.Content == "")
                        {
                            dic.Add(v.Name, null);
                        }
                        else
                        {
                            dic.Add(v.Name, double.Parse(v.Content));
                        }
                    }
                    catch
                    {
                        throw new Exception("double.Parse 失敗：" + v.Name);
                    }
                }
                else
                {
                    if (v.Content == "")
                    {
                        dic.Add(v.Name, null);
                    }
                    else
                    {
                        dic.Add(v.Name, v.Content);
                    }
                }
            }

            IDictionary<string, object> wk = tmp;
            foreach (var item in dic)
            {
                wk.Add(item.Key, item.Value);
            }

            return (object)tmp;

        }
        /// <summary>
        /// 新規追加
        /// </summary>
        /// <param name="path"></param>
        /// <param name="table"></param>
        /// <param name="models"></param>
        public static void InsertRecord(string path, string table, List<PRAGMAModel> models)
        {

            //sql作成
            var sql = new StringBuilder($"INSERT INTO {table} VALUES(");

            //@カラム名を追加
            foreach (var v in models)
            {
                sql.Append("@");
                sql.Append(v.Name);
                sql.Append(",");
            }

            //最後の文字削除
            sql.Remove(sql.Length - 1, 1);
            sql.Append(");");

            var anonymous = CreateAnonymousType("insert", models);

            //https://dapper-tutorial.net/ja/tutorial/1000168/----

            using (var conn = new SQLiteConnection("Data Source=" + path))
            {
                var affectedRows = conn.Execute(sql.ToString(), anonymous);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="path"></param>
        /// <param name="table"></param>
        /// <param name="models"></param>
        /// <param name="rowid"></param>
        public static void UpdateRecord(string path, string table, List<PRAGMAModel> models, int rowid)
        {

            //sql作成
            var sql = new StringBuilder($"UPDATE {table} SET ");

            //SET内容追加
            foreach (var v in models)
            {
                sql.Append(v.Name);
                sql.Append("=");
                sql.Append("@");
                sql.Append(v.Name);
                sql.Append(",");
            }

            //最後の文字削除
            sql.Remove(sql.Length - 1, 1);
            sql.Append($" WHERE _ROWID_ = {rowid};");

            var anonymous = CreateAnonymousType("update", models);

            //https://dapper-tutorial.net/ja/tutorial/1000168/----

            using (var conn = new SQLiteConnection("Data Source=" + path))
            {
                var affectedRows = conn.Execute(sql.ToString(), anonymous);
            }

        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="path"></param>
        /// <param name="table"></param>
        /// <param name="rowid"></param>
        public static void DeleteRecord(string path, string table, int rowid)
        {
            //sql作成
            string sql = $"DELETE FROM {table} WHERE _ROWID_ = {rowid};";

            using (var conn = new SQLiteConnection("Data Source=" + path))
            {
                var affectedRows = conn.Execute(sql.ToString());
            }
        }


        /// <summary>
        /// SQLウインドウからのExecute（SELECT以外）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sql"></param>
        public static void SQLExecute(string path, string sql)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + path))
                {
                    var affectedRows = conn.Execute(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SQLウインドウからのQuery（SELECT）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<object> SQLQuery(string path, string sql)
        {
            var list = new List<object>();

            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + path))
                {
                    list = conn.Query<dynamic>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

    }
}
