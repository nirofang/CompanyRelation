using System.Security.Cryptography;
using System.Text;

namespace Sugon.CompanyRelation.WcfService.Common
{
    public class SvcUtility
    {
        public static string StrToMD5(string str)
        {
            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] outBytes = md5.ComputeHash(data);

            string outString = "";
            for (int i = 0; i < outBytes.Length; i++)
            {
                outString += outBytes[i].ToString("x2");
            }

            return outString.ToUpper();
            //return OutString.ToLower();
        }
    }
}