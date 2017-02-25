using Sugon.CompanyRelation.WcfService.Membership;
using Sugon.CompanyRelation.Web.Common;
using Sugon.CompanyRelation.Web.Models.Login;
using Sugon.CompanyRelation.Web.ServiceChannel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sugon.CompanyRelation.Web.Controllers
{
    public class LoginController : Controller
    {
        public IMembershipService membershipSvc { get; private set; }

        public LoginController()
        {
            membershipSvc = ServiceFactory.Create<IMembershipService>();
        }

        // GET: 登录
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: 登录
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid) // 验证输入信息是否正确
            {
                // 异步验证、获取用户信息
                LoginResponse response = await membershipSvc.LoginAsync(new LoginRequest()
                {
                    UserName = HttpUtility.HtmlEncode(model.UserName),
                    //Password = HttpUtility.HtmlEncode(model.Password)
                    Password = DESEncrypt.Encrypt(model.Password)
                });

                if (response.Success)
                {
                    Session[ConstRes.USERID] = response.UserID;
                    Session[ConstRes.USERNAME] = response.UserName;
                    Session[ConstRes.TRUENAME] = response.TrueName;
                    Session[ConstRes.USERPWD] = response.UserPwd;
                    Session[ConstRes.USERROLES] = response.UserRoles;
                    //Session.Timeout = 120;
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", response.Error);
                }
            }

            return View(model);
        }

        ////
        //// POST: 退出
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            if (Session[ConstRes.USERID] != null)
            {
                Session.Clear();
                //Session[ConstRes.USERID] = null;
                //Session[ConstRes.USERLOGINNAME] = null;
                //Session[ConstRes.USERPWD] = null;
            }

            //AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Login");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(string pwd)
        {
            string newPwd = Request.Form["newPwd"].Trim();
            string confirmPwd = Request.Form["confirmPwd"].Trim();

            if (string.IsNullOrEmpty(newPwd) || newPwd.Length > 8)
            {
                ModelState.AddModelError("", "新密码必须在8位以内。");
                return PartialView();
            }

            if (newPwd == confirmPwd)
            {
                newPwd = DESEncrypt.Encrypt(newPwd);
                ChangePasswordResponse response = await membershipSvc.ChangePasswordAsync(new ChangePasswordRequest()
                {
                    UserID = Session[ConstRes.USERID].ToString(),
                    UserName = Session[ConstRes.USERNAME].ToString(),
                    Password = Session[ConstRes.USERPWD].ToString(),
                    NewPwd = newPwd
                });

                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Error);
                }
                else
                {
                    Session[ConstRes.USERPWD] = newPwd;
                    ViewBag.SuccessMsg = "密码修改成功。";
                }
            }
            else
            {
                ModelState.AddModelError("", "确认密码与新密码不一致。");
            }

            return PartialView();
        }
    }
}