using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MIS_CertificationApplication.Models
{
    public class SqlBulkcopy
    {
        public void SqlBulkCopy()
        {

            try
            {
                using (var db = new WebStationEntities())
                {
                    List<Certificates> entitys = db.Certificates.ToList(); //构建集合，到时候会将这个集合数据批量插入到


                    if (db.Database.Connection.State != ConnectionState.Open)
                    {
                        db.Database.Connection.Open(); //打开Connection连接
                    }

                    //调用BulkInsert方法,将entitys集合数据批量插入到数据库的tolocation表中
                    BulkInsert((SqlConnection)db.Database.Connection, "Certificates", entitys);

                    if (db.Database.Connection.State != ConnectionState.Closed)
                    {
                        db.Database.Connection.Close(); //关闭Connection连接
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T">泛型集合的类型</typeparam>
        /// <param name="conn">连接对象</param>
        /// <param name="tableName">将泛型集合插入到本地数据库表的表名</param>
        /// <param name="list">要插入大泛型集合</param>
        public static void BulkInsert<T>(SqlConnection conn, string tableName, IList<T> list)
        {
            var table = new DataTable();
            using (var db = new WebStationEntities())
            {
                using (var bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.BatchSize = list.Count;
                    bulkCopy.DestinationTableName = tableName;


                    var props = TypeDescriptor.GetProperties(typeof(T))
                        .Cast<PropertyDescriptor>()
                        .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                        .ToArray();

                    //将EF查询到的数据添加到DataTable
                    foreach (var propertyInfo in props)
                    {
                        bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                        table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                    }
                    string status = "";
                    object cert_ref = null;
                    object CertFile = null;
                    var values = new object[props.Length];
                    foreach (var item in list)
                    {
                        for (var i = 0; i < values.Length; i++)
                        {

                            if (props[i].Name == "Expiry")
                            {
                                if (props[i].GetValue(item) == null)
                                {
                                    status = "A";
                                }
                                else
                                {
                                    DateTime Expiry = DateTime.Parse(props[i].GetValue(item).ToString());
                                    DateTime now = DateTime.Now;
                                    if (Expiry.CompareTo(now) > 0)
                                    {
                                        status = "A";
                                    }
                                    //失效-已过有效期
                                    if (Expiry.CompareTo(now) < 0) //指定的数大于参数返回 1
                                    {
                                        status = "E";
                                    }
                                }
                                if (cert_ref.ToString() == "")
                                {
                                    //在办-新增了记录，但没有认证编号的
                                    status = "P";
                                }
                                if (CertFile == null)
                                {
                                    status = "P";
                                }
                                values[i] = props[i].GetValue(item);
                            }
                            else
                            {
                                if (props[i].Name != "Status")
                                {
                                    if (props[i].Name == "Cert_Ref")
                                    {
                                        cert_ref = props[i].GetValue(item);
                                    }
                                    if (props[i].Name == "CertFile")
                                    {
                                        CertFile = props[i].GetValue(item);
                                    }
                                    values[i] = props[i].GetValue(item);
                                }
                                else
                                {
                                    if (props[i].GetValue(item).ToString() == "D") //作废的情况下
                                    {
                                        values[i] = props[i].GetValue(item);
                                    }
                                    else
                                    {
                                        values[i] = status;
                                    }

                                }

                            }
                        }
                        table.Rows.Add(values);

                    }


                }
            }
            Update(conn, table);
        }

        /// <summary> 
        /// 批量更新数据(每批次5000) 
        /// </summary> 
        /// <param name="connString">数据库链接字符串</param> 
        /// <param name="table"></param> 
        public static void Update(SqlConnection conn, DataTable table)
        {
            SqlCommand comm = conn.CreateCommand();
            //comm.CommandTimeout = _CommandTimeOut;
            comm.CommandType = CommandType.Text;
            SqlDataAdapter adapter = new SqlDataAdapter(comm);
            SqlCommandBuilder commandBulider = new SqlCommandBuilder(adapter);
            commandBulider.ConflictOption = ConflictOption.OverwriteChanges;
            try
            {
                //设置批量更新的每次处理条数 
                adapter.UpdateBatchSize = table.Rows.Count;
                adapter.SelectCommand.Transaction = conn.BeginTransaction();/////////////////开始事务 
                WebStationEntities db = new WebStationEntities();
                adapter.SelectCommand.CommandText = "select * from  dbo.Certificates";
                db.Database.SqlQuery<DBNull>("truncate table dbo.Certificates ").SingleOrDefault();
                adapter.Update(table);
                adapter.SelectCommand.Transaction.Commit();/////提交事务 
            }
            catch (Exception ex)
            {
                if (adapter.SelectCommand != null && adapter.SelectCommand.Transaction != null)
                {
                    adapter.SelectCommand.Transaction.Rollback();
                }
                LogHelper.Write(ex.Message);
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

    }
}