using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Sugon.CompanyRelation.WcfService.StockRights;
using Sugon.CompanyRelation.WcfService.Common;

namespace Sugon.CompanyRelation.WcfService.Services
{
    // NOTE: In order to launch WCF Test Client for testing this service, please select CompanyRelationService.svc or CompanyRelationService.svc.cs at the Solution Explorer and start debugging.
    public class StockRightsService : BaseService, IStockRightsService
    {
        /// <summary>
        /// 根据编码获取其当前编号并加一
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GenerateIDResponse> GenerateIDAsync(GenerateIDRequest request)
        {
            GenerateIDResponse response = new GenerateIDResponse();
            try
            {
                KeyValuePair<bool, string> valid_info = await this.ValidateAsync(request.UserName, request.Password);
                if (valid_info.Key)
                {
                    IdGenerator generator = new IdGenerator();
                    response.ID = await generator.GenerateID(request.DocCode);
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

            return response;
        }

        /// <summary>
        /// 获取所有公司的列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetCompanyListResponse> GetCompanyListAsync(GetCompanyListRequest request)
        {
            GetCompanyListResponse response = new GetCompanyListResponse();
            KeyValuePair<bool, string> valid_info = await this.ValidateAsync(request.UserName, request.Password);
            if (valid_info.Key)
            {
                StringBuilder sqlStr = new StringBuilder();
                sqlStr.Append("Select BUKRS, BUTXT, COMPANY from COMPANY where Deleted = 0 and BEGDA <= GETDATE() and ENDDA >= GETDATE()");
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    //sqlStr.AppendFormat(" and (BUTXT like '%{0}%' or SPMANG like '%{0}%' or REPRES like '%{0}%' or NAMEV like '%{0}%')", request.Keyword);
                    sqlStr.AppendFormat(" and (BUTXT like '%{0}%' or SPMANG like '%{0}%')", request.Keyword);
                }

                sqlStr.Append(" order by COMPANY");

                IDataReader dataReader = await this.dataAccess.GetDataAsync(sqlStr.ToString()); // 执行sql语句返回结果
                while (dataReader.Read())
                {
                    string controlCompany = Convert.ToString(dataReader["COMPANY"]);
                    if (!string.IsNullOrEmpty(request.Keyword) || string.IsNullOrWhiteSpace(controlCompany))
                    {
                        response.CompanyList.Add(new KeyValuePair<string, string>(Convert.ToString(dataReader["BUKRS"]), Convert.ToString(dataReader["BUTXT"])));
                    }
                    else
                    {
                        Dictionary<string, string> item = new Dictionary<string, string>();
                        item.Add("BUKRS", Convert.ToString(dataReader["BUKRS"]));
                        item.Add("BUTXT", Convert.ToString(dataReader["BUTXT"]));
                        item.Add("COMPANY", controlCompany);
                        response.SubCompanyList.Add(item);
                    }
                }

                this.dataAccess.CloseConnection();
                response.Success = true;
            }
            else
            {
                response.Error = valid_info.Value;
            }

            return response;
        }
    }
}
