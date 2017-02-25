using Sugon.CompanyRelation.WcfService.StockRights;
using Sugon.CompanyRelation.Web.Common;
using Sugon.CompanyRelation.Web.Models.StockRights;
using Sugon.CompanyRelation.Web.ServiceChannel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sugon.CompanyRelation.Web.Controllers
{
    public class ReportController : Controller
    {
        private IStockRightsService stockRightsSvc;

        public ReportController()
        {
            this.stockRightsSvc = ServiceFactory.Create<IStockRightsService>();
        }

        public async Task<ActionResult> CompanyReport()
        {
            Utility.CheckSession();
            StockRightsViewModel model = new StockRightsViewModel();
            GetCompanyListResponse response = await stockRightsSvc.GetCompanyListAsync(new GetCompanyListRequest()
            {
                UserID = HttpUtility.HtmlEncode(Session[ConstRes.USERID]),
                UserName = HttpUtility.HtmlEncode(Session[ConstRes.USERNAME]),
                Password = HttpUtility.HtmlEncode(Session[ConstRes.USERPWD])
            });

            if (response.Success)
            {
                model.CompanyList = response.CompanyList;
                model.SubCompanyList = response.SubCompanyList;
            }
            else
            {
                ModelState.AddModelError("", response.Error);
            }

            return PartialView(model);
        }

        public async Task<ActionResult> Reports()
        {
            Utility.CheckSession();

            return PartialView();
        }

        public async Task<ActionResult> DailyReport()
        {
            Utility.CheckSession();

            return PartialView();
        }
    }
}