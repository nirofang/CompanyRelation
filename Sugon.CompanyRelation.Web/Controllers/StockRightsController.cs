using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Sugon.CompanyRelation.WcfService.StockRights;
using Sugon.CompanyRelation.Web.ServiceChannel;
using Sugon.CompanyRelation.Web.Models.StockRights;
using Sugon.CompanyRelation.WcfService.Common;
using Sugon.CompanyRelation.Web.Common;
using System.Text;

namespace Sugon.CompanyRelation.Web.Controllers
{
    public class StockRightsController : Controller
    {
        private IStockRightsService stockRightsSvc;

        public StockRightsController()
        {
            this.stockRightsSvc = ServiceFactory.Create<IStockRightsService>();
        }

        public async Task<ActionResult> StockRights(string keyword)
        {
            Utility.CheckSession();

            return PartialView();
        }
    }
}