using System;
using System.Web;

namespace Sugon.CompanyRelation.Web.Common
{
    public class Utility
    {
        //判断Session是否有效
        public static void CheckSession()
        {
            if (HttpContext.Current.Session[ConstRes.USERNAME] == null)
            {
                HttpContext.Current.Response.Write("<script>alert('登录信息安全时限过期，请重新登录！');top.location='/Login'</script>");
                HttpContext.Current.Response.End(); //终止后面的所有的输出
            }
            //try
            //{
            //    if (HttpContext.Current.Session["UserLoginName"] == null)
            //    {
            //        HttpContext.Current.Response.Write("<script>alert('登录信息安全时限过期，请重新登录！');top.location='../Default.aspx'</script>");
            //        HttpContext.Current.Response.End(); //终止后面的所有的输出
            //    }
            //}
            //catch
            //{
            //    HttpContext.Current.Response.Write("<script>alert('登录信息安全时限过期，请重新登录！');top.location='../Default.aspx'</script>");
            //    HttpContext.Current.Response.End();  //终止后面的所有的输出
            //}
        }
    }
}