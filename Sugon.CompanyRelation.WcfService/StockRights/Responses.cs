
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Sugon.CompanyRelation.WcfService.StockRights
{
    [DataContract]
    public class GetCompanyListResponse : BaseResponse
    {
        public GetCompanyListResponse()
        {
            CompanyList = new List<KeyValuePair<string, string>>();
            SubCompanyList = new List<Dictionary<string, string>>();
        }

        [DataMember]
        public List<KeyValuePair<string, string>> CompanyList { get; set; }

        [DataMember]
        public List<Dictionary<string, string>> SubCompanyList { get; set; }
    }
}