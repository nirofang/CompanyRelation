using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace Sugon.CompanyRelation.Web.Common
{
    public class ConstRes
    {
        public const string USERID = "UserID";
        public const string USERNAME = "UserName";
        public const string USERPWD = "UserPwd";
        public const string TRUENAME = "TrueName";
        public const string USERROLES = "UserRoles";

        public const string FILESERVERPATH = "FileServerPath";

        public const string DATESTYLE = "yyyy/MM/dd";
        public const string DATETIMESTYLE = "yyyy/MM/dd HH:mm:ss";

        public const string DOCCODE_USER = "USRK"; // 用户
        public const string DOCCODE_COMPANY = "COMK"; // 公司
        public const string DOCCODE_DEPARTMENT = "DEPK"; // 部门
        public const string DOCCODE_POSITION = "POSK"; // 岗位
        public const string SHAREHOLDER = "SHLD"; // 股东
    }
}