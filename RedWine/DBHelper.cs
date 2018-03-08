using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace RedWine
{
    public class DBHelper
    {
        //private static readonly string connString = "Data Source = 192.168.111.184:1521/XXB;user id = xgb; password = xgb123";
        //private static readonly string connString = "Data Source = 127.0.0.1:1521/ORCL;user id = xgb; password = xgb123";
        //private static readonly string connString = "Data Source = 192.168.103.45:1521/ORCL;user id = xgb; password = xgb123";
        //Server=192.168.111.184;UserId=root;Password=jncadmin;Database=fyxt;Charset=utf8
        private static readonly string connString = "Server=140.143.96.83;UserId=RedWine;Password=Dw123~;Database=RW;Charset=utf8";
        public static DbConnection GetDbConnection()
        {
            return new MySqlConnection(connString);
        }
        public static async Task<IEnumerable<T>> GetAll<T>() where T:class
        {
            using (var conn = GetDbConnection())
            {
                return await conn.GetAllAsync<T>();
            }
        }
        public static async Task<T> AsyncGet<T>(int id) where T : class
        {
            using (var conn = GetDbConnection())
            {
                return await conn.GetAsync<T>(id);
            }
        }
        public static async Task<T> AsyncGet<T>(string id) where T : class
        {
            using (var conn = GetDbConnection())
            {
                return await conn.GetAsync<T>(id);
            }
        }
        public static async Task<DataTable> AsyncTable(string sql, object param = null)
        {
            var t = Task.Run(() =>
            {
                using (var conn = new MySqlConnection(connString))
                {
                    using (var oda = new MySqlDataAdapter(sql, conn))
                    {
                        DataSet ds = new DataSet();
                        oda.Fill(ds, "tempdt");
                        return ds.Tables[0];
                    }
                }
            });
            return await t;
        }
       
        public static async Task<object> AsyncExecuteScalar(string sql, object param = null)
        {
            using (var conn = GetDbConnection())
            {
                return await conn.ExecuteScalarAsync(sql, param);
            }
        }
        public static async Task<IEnumerable<T>> AsyncQuery<T>(string sql, object param = null)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                using (var conn = GetDbConnection())
                {
                    return await conn.QueryAsync<T>(sql, param);
                }
            }
            return null;
        }
        public static async Task<IEnumerable<T>> AsyncQuery<T>(SqlAndParam sp)
        {
            return await AsyncQuery<T>(sp.Sql, sp.Param);
        }
        public static async Task<IEnumerable<dynamic>> AsyncQuery(string sql, object param = null)
        {
            using (var conn = GetDbConnection())
            {
                return await conn.QueryAsync(sql, param);
            }
        }
        public static int ExcuteWithTrans(string sql)
        {
            var sp = new SqlAndParam();
            sp.Sql = sql;
            return ExcuteWithTrans(sp);
        }
        public static int ExcuteWithTrans(SqlAndParam sp)
        {
            int rows = 0;
            using (var conn = GetDbConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        rows = conn.Execute(sp.Sql, sp.Param, tran, null, null);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        rows = 0;
                        tran.Rollback();
                        throw ex;
                    }
                }
            }
            return rows;
        }

        public static async Task<int> AsyncExcute(List<SqlAndParam> splst)
        {
            int i = 0;
            if (splst.Count > 0)
            {
                using (var conn = GetDbConnection())
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in splst)
                            {
                                i += await conn.ExecuteAsync(item.Sql, item.Param, tran, null, null);
                            }
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            i = 0;
                            tran.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            return i;
        }
        public static async Task<bool> AsyncInsert<T>(T model) where T : class
        {
            var i = false;
            using (var conn = GetDbConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var r = await conn.InsertAsync(model, tran);
                    i = r > 0;
                    tran.Commit();
                }
            }
            return i;
        }
        public static async Task<bool> AsyncInsert<T>(IEnumerable<T> model) where T : class
        {
            var i = false;
            using (var conn = GetDbConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var r = await conn.InsertAsync(model, tran);
                    i = r > 0;
                    tran.Commit();
                }
            }
            return i;
        }
        public static async Task<bool> AsyncUpdate<T>(T model) where T : class
        {
            var i = false;
            using (var conn = GetDbConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    i = await conn.UpdateAsync(model, tran);
                    tran.Commit();
                }
            }
            return i;
        }

        public static async Task<bool> AsyncUpdate<T>(IEnumerable<T> model)
        {
            var i = false;
            using (var conn = GetDbConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    i = await conn.UpdateAsync(model, tran);
                    tran.Commit();
                }
            }
            return i;
        }
        public static async Task<bool> AsyncDelete<T>(T model) where T : class
        {
            var i = false;
            using (var conn = GetDbConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    i = await conn.DeleteAsync(model, tran);
                    tran.Commit();
                }
            }
            return i;
        }
        public static async Task<bool> AsyncDelete<T>(IEnumerable<T> model) where T : class
        {
            var i = false;
            using (var conn = GetDbConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    i = await conn.DeleteAsync(model, tran);
                    tran.Commit();
                }
            }
            return i;
        }
    }
    public class SqlAndParam
    {
        public string Sql { get; set; }
        public object Param { get; set; }
    }
}
