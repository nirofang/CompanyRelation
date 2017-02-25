using Sugon.CompanyRelation.WcfService.Membership;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sugon.CompanyRelation.WcfService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MembershipService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MembershipService.svc or MembershipService.svc.cs at the Solution Explorer and start debugging.
    public class MembershipService : BaseService, IMembershipService
    {
        // 登录
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();

            try
            {
                string sqlStr = string.Format("select A.*, B.roleID from USERINFO A left join USER_ROLE B on A.UserID = B.UserID where UserName = '{0}' and userPwd = '{1}' and Deleted = 0", request.UserName, request.Password);

                IDataReader dataReader = await this.dataAccess.GetDataAsync(sqlStr); // 执行sql语句返回结果
                if (!dataReader.Read())
                {
                    response.Error = "登录名或密码无效！";
                    this.dataAccess.CloseConnection();
                    return response;
                }

                if (dataReader["Status"] == null || dataReader["Status"] == DBNull.Value || !Convert.ToBoolean(dataReader["Status"]))
                {
                    response.Error = "用户信息已经失效！";
                    this.dataAccess.CloseConnection();
                    return response;
                }

                if (dataReader["userExpireDate"] == null || dataReader["userExpireDate"] == DBNull.Value || Convert.ToDateTime(dataReader["userExpireDate"]) < DateTime.Now)
                {
                    response.Error = "用户信息已过期！";
                    this.dataAccess.CloseConnection();
                    return response;
                }

                response.UserID = dataReader["userID"].ToString();
                response.UserName = dataReader["userName"].ToString();
                response.TrueName = dataReader["TrueName"].ToString();
                response.UserPwd = dataReader["userPwd"].ToString();

                response.UserRoles = new StringBuilder();
                if (dataReader["roleID"] != DBNull.Value)
                {
                    response.UserRoles.AppendFormat("[{0}]", dataReader["roleID"]);
                }

                while (dataReader.Read())
                {
                    if (dataReader["roleID"] != DBNull.Value)
                    {
                        response.UserRoles.AppendFormat("[{0}]", dataReader["roleID"]);
                    }
                }

                response.Success = true;
                this.dataAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                this.dataAccess.CloseConnection();
            }

            return response;
        }

        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request)
        {
            ChangePasswordResponse response = new ChangePasswordResponse();
            try
            {
                string sqlStr = string.Format("update USERINFO set UserPwd = '{0}' where UserID = '{1}' and UserPwd = '{2}'",
                    request.NewPwd, request.UserID, request.Password);
                int rowAffected = await this.dataAccess.ExecuteNonQuery(sqlStr);
                response.Success = rowAffected > 0 ? true : false;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }

            return response;
        }
    }
}
