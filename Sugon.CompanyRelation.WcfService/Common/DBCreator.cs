using System.Reflection;
using System.Configuration;
using Sugon.CompanyRelation.DataAccess;

namespace Sugon.CompanyRelation.WcfService.Common
{
    public class DBCreator
    {
        private static IDataAccess dbAccess = null;

        /// <summary>
        /// 从配置文件获取数据库类型，根据该类型实例化相应的数据库操作类
        /// </summary>
        /// <returns></returns>
        public static IDataAccess GetDBAccess()
        {
            if (null == dbAccess)
            {
                string assemblyName = "Sugon.CompanyRelation.DataAccess";
                string DB = ConfigurationManager.AppSettings["DB"]; // 从配置文件获取数据库类型
                string className = assemblyName + "." + DB + "Access";

                dbAccess = (IDataAccess)Assembly.Load(assemblyName).CreateInstance(className); // 从配置文件获取要实例化的对象名，用反射对其进行实例化
            }
            return dbAccess;
        }
    }
}