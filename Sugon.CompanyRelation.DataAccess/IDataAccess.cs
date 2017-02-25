using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Sugon.CompanyRelation.DataAccess
{
    /// <summary>
    /// 数据库操作接口
    /// </summary>
    public interface IDataAccess
    {
        Task<bool> OpenConnection();

        Task<bool> OpenConnection(string connectionStr);

        bool CloseConnection();

        Task<IDataReader> GetDataAsync(string sqlStr);
        IDataReader GetData(string sqlStr);

        DataTable GetDataTable(string sqlStr);

        Task<int> ExecuteNonQuery(string sqlStr);

        Task<int> ExecuteNonQueryWithTransaction(List<string> sqlStrList);

        Task<bool> SaveClob(string sqlStr, string buffer);

        Task<string> GetClob(string tableName, string fieldName, string where);

        void ImportDataToDataBase(DataTable dt, Dictionary<string, string> columns, string tableName);
    }
}
