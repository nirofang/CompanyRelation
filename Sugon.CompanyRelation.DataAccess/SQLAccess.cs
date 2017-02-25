using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugon.CompanyRelation.DataAccess
{
    public class SQLAccess : IDataAccess
    {
        //private static object _object = new object();
        private const string SQLCONNECTION = "SQLConnection";
        private SqlConnection connection;

        //public void Dispose()
        //{
        //    Dispose(true);

        //    GC.SuppressFinalize(this);
        //}

        /// <summary>
        /// 从配置文件获取连接字符串，调用OpenConnection(string connectionStr)方法打开数据库连接，无异常则返回true
        /// </summary>
        /// <returns>true or false</returns>
        public async Task<bool> OpenConnection()
        {
            string connectionStr = ConfigurationManager.ConnectionStrings[SQLCONNECTION].ConnectionString;

            return await this.OpenConnection(connectionStr);
        }

        /// <summary>
        /// 打开数据库连接，无异常则返回true
        /// </summary>
        /// <param name="connectionStr">连接字符串</param>
        /// <returns>true or false</returns>
        public async Task<bool> OpenConnection(string connectionStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(connectionStr))
                {
                    connection = new SqlConnection(connectionStr); // 根据连接字符串与数据库建立连接
                    connection.Open(); // 打开通道
                }
                else
                {
                    throw new Exception("连接字符串为空！请配置正确的连接字符串。");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <returns>true or false</returns>
        public bool CloseConnection()
        {
            try
            {
                if (connection != null && connection.State.ToString().ToUpper().Equals("OPEN"))
                {
                    connection.Close(); // 关闭数据库通道
                }

                connection.Dispose(); // 释放数据库链接
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// 异步执行指定的SQL语句（查询），返回从数据库取得的结果
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns>SqlDataReader类型的查询结果</returns>
        public async Task<IDataReader> GetDataAsync(string sqlStr)
        {
            try
            {
                await this.OpenConnection(); // 创建并打开数据库链接

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlStr;
                cmd.Connection = connection;

                return await cmd.ExecuteReaderAsync(); // 执行命令并返回数据
            }
            catch (Exception ex)
            {
                this.CloseConnection();
                throw ex;
            }
        }

        /// <summary>
        /// 执行指定的SQL语句（查询），返回从数据库取得的结果
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns>SqlDataReader类型的查询结果</returns>
        public IDataReader GetData(string sqlStr)
        {
            try
            {
                this.OpenConnection(); // 创建并打开数据库链接

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlStr;
                cmd.Connection = connection;

                return cmd.ExecuteReader(); // 执行命令并返回数据
            }
            catch (Exception ex)
            {
                this.CloseConnection();
                throw ex;
            }
        }

        public DataTable GetDataTable(string sqlStr)
        {
            DataTable dt = new DataTable();
            try
            {
                string connectionStr = ConfigurationManager.ConnectionStrings[SQLCONNECTION].ConnectionString;
                this.connection = new SqlConnection(connectionStr); // 根据连接字符串与数据库建立连接
                this.connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, this.connection))
                {
                    adapter.Fill(dt);
                }
                this.CloseConnection();
            }
            catch (Exception ex)
            {
                this.CloseConnection();
                throw ex;
            }

            return dt;
        }

        /// <summary>
        /// 执行指定的SQL语句（增，删，改），返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQuery(string sqlStr)
        {
            try
            {
                await this.OpenConnection(); // 创建并打开数据库链接

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlStr;
                cmd.Connection = connection;

                return cmd.ExecuteNonQuery();//执行命令并返回受影响的行数
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConnection();
            }
        }


        public async Task<int> ExecuteNonQueryWithTransaction(List<string> sqlStrList)
        {
            int row = 1;
            try
            {
                await this.OpenConnection(); // 创建并打开数据库链接

                SqlTransaction transation = this.connection.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Transaction = transation;
                cmd.Connection = connection;

                foreach (string sqlStr in sqlStrList)
                {
                    cmd.CommandText = sqlStr;
                    cmd.ExecuteNonQuery();
                    row++;
                }

                transation.Commit();
                transation.Dispose();
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("第{0}行附近的数据有问题，请检查数据格式是否正确。附加信息：{1}", row, ex.Message));
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// 将数据批量导入到数据库中
        /// </summary>
        /// <param name="dt">要导入的数据</param>
        /// <param name="columns">源列名与数据库中的字段名</param>
        /// <param name="tableName">表名</param>
        public void ImportDataToDataBase(DataTable dt, Dictionary<string, string> columns, string tableName)
        {
            try
            {
                //this.OpenConnection();
                string sqlConnectionString = ConfigurationManager.ConnectionStrings[SQLCONNECTION].ConnectionString;
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnectionString))
                {
                    foreach (string columnName in columns.Keys)
                    {
                        bulkCopy.ColumnMappings.Add(columnName, columns[columnName]);
                    }

                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = dt.Rows.Count; // 每次传输的行数
                    //bulkCopy.NotifyAfter = 100;//进度提示的行数
                    bulkCopy.WriteToServer(dt); // 将大批量的数据复制到相应的表中
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConnection();
            }

        }


        public async Task<bool> SaveClob(string sqlStr, string buffer)
        {
            bool result = false;
            //byte[] buffer = null;

            if (await this.OpenConnection())
            {
                SqlTransaction trunsaction = connection.BeginTransaction();//开始事务
                SqlCommand cmd = connection.CreateCommand();
                cmd.Transaction = trunsaction;

                try
                {
                    //UnicodeEncoding encoding = new UnicodeEncoding();
                    //buffer = new byte[content.Length];
                    //buffer = encoding.GetBytes(content);//将要保存的内容从String类型放到byte类型的变量中

                    if (!string.IsNullOrEmpty(buffer))
                    {
                        cmd.Parameters.Add("@text", SqlDbType.Text);//添加一个参数用于存放大文件内容
                        cmd.Parameters["@text"].Value = buffer;//将大文件内容放到参数值中
                    }

                    cmd.CommandText = sqlStr;

                    cmd.ExecuteNonQuery();//执行sql语句

                    trunsaction.Commit();//执行事务
                    result = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    this.CloseConnection();
                    buffer = null;
                }

                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取LOB数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">LOB字段名</param>
        /// <param name="where">查询条件</param>
        /// <returns>LOB内容</returns>
        public async Task<string> GetClob(string tableName, string fieldName, string where)
        {
            string lobContent = "";
            SqlDataReader dataReader = null;
            byte[] buffer = null;

            if (await this.OpenConnection())
            {
                SqlCommand cmd = connection.CreateCommand();

                try
                {
                    int buffSize = 4096;//每次按一定的大小读取(4096=4K)
                    long startIndex = 0;//每次读取LOB的起始位置，默认从0开始
                    int bufferStartIndex = 0;
                    long numRead = 0;

                    cmd.CommandText = "select " + fieldName + " from " + tableName + " where " + where;
                    dataReader = cmd.ExecuteReader();
                    dataReader.Read();

                    buffer = new byte[buffSize];

                    //GetBytes 5个参数的含义：第一列 || 从LOB对象的某一位置读取 || 用于存放读取出来的数据的buffer || buffer中写入操作开始位置的索引 || 要复制到buffer中的最大长度
                    //返回实际读取的字节数
                    numRead = dataReader.GetBytes(1, startIndex, buffer, 0, buffSize);

                    //只要读取到的字节数等于指定的数就说明还没读完，需要继续读取
                    while (numRead == buffSize)
                    {
                        startIndex += buffSize;
                        bufferStartIndex += buffSize;
                        numRead = dataReader.GetBytes(1, startIndex, buffer, bufferStartIndex, buffSize);
                    }

                    UnicodeEncoding encoding = new UnicodeEncoding();
                    lobContent = encoding.GetString(buffer);//将读取到的buffer数组转换成string
                    return lobContent;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    this.CloseConnection();
                    buffer = null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 修改LOB文件
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">LOB字段名</param>
        /// <param name="where">查询条件</param>
        /// <param name="content">LOB的内容</param>
        /// <returns>成功true，失败false</returns>
        public async Task<bool> UpdateClob(string tableName, string fieldName, string where, string content)
        {
            bool result = false;
            byte[] buffer = null;

            if (await this.OpenConnection())
            {
                SqlTransaction trunsaction = connection.BeginTransaction();//开始事务
                SqlCommand cmd = connection.CreateCommand();
                cmd.Transaction = trunsaction;

                try
                {
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    buffer = new byte[content.Length];
                    buffer = encoding.GetBytes(content);//将要保存的内容从String类型放到byte类型的变量中

                    string sqlStr = "update " + tableName + " set " + fieldName + " =@text  where " + where;

                    cmd.CommandText = sqlStr;
                    cmd.Parameters.Add("@text", SqlDbType.Text);//添加一个参数用于存放大文件内容
                    cmd.Parameters["@text"].Value = buffer;//将大文件内容放到参数值中

                    cmd.ExecuteNonQuery();//执行sql语句
                    cmd.Parameters.Clear();

                    trunsaction.Commit();//执行事务
                    result = true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    this.CloseConnection();
                    buffer = null;
                }

                return result;
            }
            else
            {
                return false;
            }
        }
    }
}
