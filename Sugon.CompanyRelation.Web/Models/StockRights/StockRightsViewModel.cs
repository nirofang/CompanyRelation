using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sugon.CompanyRelation.Web.Models.StockRights
{
    public class StockRightsViewModel
    {
        public List<KeyValuePair<string, string>> CompanyList { get; set; }

        public List<Dictionary<string, string>> SubCompanyList { get; set; }

        public string Keyword { get; set; }
    }
}