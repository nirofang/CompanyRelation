using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sugon.CompanyRelation.WcfService.StockRights
{
    [DataContract]
    public class GetCompanyListRequest : BaseRequest
    {
        [DataMember]
        public string Keyword { get; set; }
    }
}