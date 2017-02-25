using System;
using System.Data;
using System.Threading.Tasks;
using Sugon.CompanyRelation.DataAccess;

namespace Sugon.CompanyRelation.WcfService.Common
{
    public class DocCode
    {
        public const string USER = "USRK"; // 用户
        public const string COMPANY = "COMK"; // 公司
        public const string DEPARTMENT = "DEPK"; // 部门
        public const string POSITION = "POSK"; // 岗位
        public const string SHAREHOLDER = "SHLD"; // 股东
    }

    public class IdGenerator
    {
        public async Task<string> GenerateID(string docCode)
        {
            IDataAccess dataAccess = DBCreator.GetDBAccess();
            string id = string.Empty;

            string sqlStr = string.Format("select CurrentNo, IdStart, IdEnd from DOCID where DocCode = '{0}'", docCode);
            IDataReader dataReader = await dataAccess.GetDataAsync(sqlStr);
            if (dataReader.Read())
            {
                long currentNo = Convert.ToInt64(dataReader["CurrentNo"]);
                long idStart = Convert.ToInt64(dataReader["IdStart"]);
                long idEnd = Convert.ToInt64(dataReader["IdEnd"]);

                if (currentNo < idEnd)
                {
                    currentNo++;
                    id = (idStart + currentNo).ToString();
                }
            }

            dataAccess.CloseConnection();
            return id;
        }

        public async Task IncreaseID(string docCode)
        {
            IDataAccess dataAccess = DBCreator.GetDBAccess();
            string sqlStr = string.Format("update DOCID set CurrentNo = (select CurrentNo from DOCID where DocCode = '{0}') + 1 where DocCode = '{0}'", docCode);
            await dataAccess.ExecuteNonQuery(sqlStr);
        }
    }
}