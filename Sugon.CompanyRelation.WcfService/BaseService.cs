using Sugon.CompanyRelation.WcfService.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Sugon.CompanyRelation.DataAccess;

namespace Sugon.CompanyRelation.WcfService
{
    public class BaseService
    {
        public BaseService()
        {
            this.dataAccess = DBCreator.GetDBAccess();
        }

        protected IDataAccess dataAccess;
        //protected UserManager UserManager { get; private set; }

        /// <summary>
        /// 验证用户信息及权限
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="rightID"></param>
        /// <returns></returns>
        protected async Task<KeyValuePair<bool, string>> ValidateAsync(string userName, string password, int rightID = -1)
        {
            KeyValuePair<bool, string> valid_info = new KeyValuePair<bool, string>(true, "");
            string sqlStr = string.Empty;
            if (rightID > 0)
            {
                // 验证基本信息、权限
                sqlStr = string.Format("select A.UserID from USERINFO A left join USER_ROLE B on A.UserID = B.UserID left join ROLE_RIGHT C on B.roleID = C.roleID where userName = '{0}' and userPwd = '{1}' and C.rightID = {2}", userName, password, rightID);
                IDataReader dataReaderObj = await dataAccess.GetDataAsync(sqlStr); // 执行sql语句返回结果
                if (!dataReaderObj.Read())
                {
                    valid_info = new KeyValuePair<bool, string>(false, "当前登录用户权限不足");
                    dataAccess.CloseConnection();
                    return valid_info;
                }
                dataAccess.CloseConnection();
            }

            // 基本信息验证
            sqlStr = string.Format("select Status, userExpireDate from USERINFO where userName = '{0}' and userPwd = '{1}' and Status = 1", userName, password);
            IDataReader dataReader = await dataAccess.GetDataAsync(sqlStr); // 执行sql语句返回结果
            if (dataReader.Read())
            {
                DateTime userExpireDate = Convert.ToDateTime(dataReader["userExpireDate"]);
                bool userStatus = Convert.ToBoolean(dataReader["Status"]);
                if (!userStatus)
                {
                    valid_info = new KeyValuePair<bool, string>(false, "用户已离职");
                }
                else if (userExpireDate < DateTime.Now)
                {
                    valid_info = new KeyValuePair<bool, string>(false, "用户已过期");
                }
            }
            else
            {
                valid_info = new KeyValuePair<bool, string>(false, "登录名或密码无效");
            }

            dataAccess.CloseConnection();

            return valid_info;
        }

        protected KeyValuePair<bool, string> Validate(string userName, string password, int rightID = -1)
        {
            KeyValuePair<bool, string> valid_info = new KeyValuePair<bool, string>(true, "");
            string sqlStr = string.Empty;
            if (rightID > 0)
            {
                // 验证基本信息、权限
                sqlStr = string.Format("select A.UserID from USERINFO A left join USER_ROLE B on A.UserID = B.UserID left join ROLE_RIGHT C on B.roleID = C.roleID where userName = '{0}' and userPwd = '{1}' and C.rightID = {2}", userName, password, rightID);
                IDataReader dataReaderObj = dataAccess.GetData(sqlStr); // 执行sql语句返回结果
                if (!dataReaderObj.Read())
                {
                    valid_info = new KeyValuePair<bool, string>(false, "当前登录用户权限不足");
                    dataAccess.CloseConnection();
                    return valid_info;
                }
                dataAccess.CloseConnection();
            }

            // 基本信息验证
            sqlStr = string.Format("select Status, userExpireDate from USERINFO where userName = '{0}' and userPwd = '{1}' and Status = 1", userName, password);
            IDataReader dataReader = dataAccess.GetData(sqlStr); // 执行sql语句返回结果
            if (dataReader.Read())
            {
                DateTime userExpireDate = Convert.ToDateTime(dataReader["userExpireDate"]);
                bool userStatus = Convert.ToBoolean(dataReader["Status"]);
                if (!userStatus)
                {
                    valid_info = new KeyValuePair<bool, string>(false, "用户已离职");
                }
                else if (userExpireDate < DateTime.Now)
                {
                    valid_info = new KeyValuePair<bool, string>(false, "用户已过期");
                }
            }
            else
            {
                valid_info = new KeyValuePair<bool, string>(false, "登录名或密码无效");
            }

            dataAccess.CloseConnection();

            return valid_info;
        }

        /// <summary>
        /// 验证用户名是否已经存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected async Task<bool> IsLoginNameExisting(string userName)
        {
            bool valid = false;
            string sqlStr = string.Format("select userID from USERINFO where UserName = {0}", userName);

            IDataReader dataReader = await dataAccess.GetDataAsync(sqlStr); // 执行sql语句返回结果
            if (dataReader.Read())
            {
                valid = true;
            }

            dataAccess.CloseConnection();
            return valid;
        }

        protected async Task LogMessageAsync(string methodName, string userLoginName, string message, string type = "SERVICE")
        {
            try
            {
                string sqlStr = string.Format("insert into LOGS (Time, Type, MethodName, UserLoginName, Message) values('{0}', '{1}', '{2}', '{3}', '{4}')",
                    DateTime.Now.ToString(), type, methodName, userLoginName, message.Replace("'", "''"));
                await this.dataAccess.ExecuteNonQuery(sqlStr);
            }
            catch
            { }
        }

        protected async Task<Dictionary<string, string>> GetDataDict(string sqlStr, string keyFieldName, string valueFieldName, BaseResponse response, string userName, string password, int rightID = -1)
        {
            Dictionary<string, string> returnData = new Dictionary<string, string>();
            KeyValuePair<bool, string> valid_info = await this.ValidateAsync(userName, password);
            if (valid_info.Key)
            {
                IDataReader dataReader = await this.dataAccess.GetDataAsync(sqlStr);
                while (dataReader.Read())
                {
                    if (dataReader[keyFieldName] == null || dataReader[keyFieldName] == DBNull.Value)
                        continue;

                    returnData.Add(dataReader[keyFieldName].ToString(), dataReader[valueFieldName].ToString());
                }

                this.dataAccess.CloseConnection();
                response.Success = true;
            }
            else
            {
                response.Error = valid_info.Value;
            }

            return returnData;
        }

        protected async Task<List<Dictionary<string, string>>> GetDataList(string sqlStr, List<string> fields, BaseResponse response, string userName, string password, string commaField = "", int rightID = -1)
        {
            List<Dictionary<string, string>> returnData = new List<Dictionary<string, string>>();

            try
            {
                KeyValuePair<bool, string> valid_info = await this.ValidateAsync(userName, password);
                if (valid_info.Key)
                {
                    IDataReader dataReader = await this.dataAccess.GetDataAsync(sqlStr); // 执行sql语句返回结果
                    while (dataReader.Read())
                    {
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        foreach (string field in fields)
                        {
                            dict.Add(field, dataReader[field] != null && dataReader[field] != DBNull.Value ? dataReader[field].ToString() : "");
                        }

                        if (commaField.Length > 0 && dict.ContainsKey(commaField) && !string.IsNullOrEmpty(dict[commaField]))
                        {
                            dict[commaField] = dict[commaField].Substring(1);
                        }

                        returnData.Add(dict);
                    }

                    this.dataAccess.CloseConnection();
                    response.Success = true;
                }
                else
                {
                    response.Error = valid_info.Value;
                }
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }

            return returnData;
        }

        protected async Task<Dictionary<string, string>> GetDataInfoAsync(string sqlStr, List<string> fields, BaseResponse response, string userName, string password, int rightID = -1)
        {
            Dictionary<string, string> returnData = new Dictionary<string, string>();
            KeyValuePair<bool, string> valid_info = await this.ValidateAsync(userName, password);
            if (valid_info.Key)
            {
                IDataReader dataReader = await this.dataAccess.GetDataAsync(sqlStr); // 执行sql语句返回结果
                if (dataReader.Read())
                {
                    foreach (string field in fields)
                    {
                        returnData.Add(field, dataReader[field] != null && dataReader[field] != DBNull.Value ? dataReader[field].ToString() : "");
                    }
                }

                this.dataAccess.CloseConnection();
                response.Success = true;
            }
            else
            {
                response.Error = valid_info.Value;
            }

            return returnData;
        }

        protected Dictionary<string, string> GetDataInfo(string sqlStr, List<string> fields, BaseResponse response, string userName, string password, int rightID = -1)
        {
            Dictionary<string, string> returnData = new Dictionary<string, string>();
            KeyValuePair<bool, string> valid_info = this.Validate(userName, password);
            if (valid_info.Key)
            {
                IDataReader dataReader = this.dataAccess.GetData(sqlStr); // 执行sql语句返回结果
                if (dataReader.Read())
                {
                    foreach (string field in fields)
                    {
                        returnData.Add(field, dataReader[field] != null && dataReader[field] != DBNull.Value ? dataReader[field].ToString() : "");
                    }
                }

                this.dataAccess.CloseConnection();
                response.Success = true;
            }
            else
            {
                response.Error = valid_info.Value;
            }

            return returnData;
        }

        protected async Task CreateOrUpdateData(string sqlStr, string userName, string password, BaseResponse response, int rightID = -1)
        {
            KeyValuePair<bool, string> valid_info = await this.ValidateAsync(userName, password, rightID);
            if (valid_info.Key)
            {
                int rowAffected = await this.dataAccess.ExecuteNonQuery(sqlStr);
                response.Success = rowAffected > 0 ? true : false;
            }
            else
            {
                response.Error = valid_info.Value;
            }
        }

        protected async Task<string> GetLowerUserIDsSql(string userID)
        {
            string lowerRoleIDs = "''";
            // 获取当前用户的下级角色
            string sqlStr = string.Format("select C.LowerRoles from USERINFO A inner join USER_ROLE B on A.userID = B.userID inner join ROLES C on B.roleID = C.roleID where A.userID = '{0}' and B.IsDeleted = 0",
                userID);
            IDataReader dataReader = await this.dataAccess.GetDataAsync(sqlStr); // 执行sql语句返回结果
            while (dataReader.Read())
            {
                string roleID = dataReader["LowerRoles"].ToString();
                if (!string.IsNullOrEmpty(roleID))
                {
                    lowerRoleIDs = lowerRoleIDs + (roleID.StartsWith(",") ? roleID : "," + roleID);
                }
            }
            this.dataAccess.CloseConnection();

            return string.Format("select UserID from USER_ROLE where roleID in ({0})", lowerRoleIDs); // 获取当前角色的下级角色所对应的用户ID
        }
    }
}