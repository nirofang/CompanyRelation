using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sugon.CompanyRelation.Web.Models.StockRights
{
    public class CompanyViewModel
    {
        //公司名称
        [Required(ErrorMessage = "必填项")]
        public string CompanyName { get; set; }

        //成立日期
        [Required(ErrorMessage = "必填项")]
        public string CreateDate { get; set; }

        //注册资本
        [Required(ErrorMessage = "必填项")]
        //[Range(typeof(decimal),"0.00", "9999999999.00", ErrorMessage = "请输入正确的数字")]
        public double RegAmount { get; set; }

        //注册地址
        public string RegAddress { get; set; }

        //法定代表人
        [Required(ErrorMessage = "必填项")]
        public string Representative { get; set; }

        //主营业务
        public string MainBusiness { get; set; }

        //联系人
        public string Contact { get; set; }

        //联系电话
        public string ContactPhone { get; set; }

        //对外投资情况
        public string Investment { get; set; }

        //是否签订协议
        public string IsSigned { get; set; }
        public List<SelectListItem> IsSignedList { get; set; }

        //统一社会信用代码
        public string CreditCode { get; set; }

        //上次年检日期
        public string CheckDate { get; set; }

        //管理归属
        public string ControlCompany { get; set; }
        public List<SelectListItem> ControlCompanyList { get; set; }

        //指定管理人
        public string SpecManager { get; set; }

        //是否合并报表
        public string IsCombine { get; set; }
        public List<SelectListItem> IsCombineList { get; set; }

        //public string CombineType { get; set; }

        //权益比例
        //[Range(typeof(decimal), "0.00", "99.00", ErrorMessage = "请输入正确的数字")]
        public double RightsRate { get; set; }

        //曙光下一期入资日期
        public string NextInvestDate { get; set; }

        //下一期入资金额
        public string NextInvestAmount { get; set; }

        //经理
        public string MainManager { get; set; }

        //副经理
        public string ViceManager { get; set; }

        //财务总监
        public string CFO { get; set; }

        //董事长
        public string Chairman { get; set; }

        //副董事长
        public string ViceChairman { get; set; }

        //董秘
        public string ChairmanSecretary { get; set; }

        //董事
        public string Director { get; set; }

        //监事会主席
        public string SupervisorChairman { get; set; }

        //监事会副主席
        public string ViceSupervisorChairman { get; set; }

        //监事
        public string Supervisor { get; set; }

        //职工监事
        public string StaffSupervisor { get; set; }
    }
}