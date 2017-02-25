using System;
using System.Collections.Generic;
using System.Text;

namespace Sugon.CompanyRelation.WcfService.Common
{
    public class SqlGenerator
    {
        public static string GenerateUpdateSql(string tableName, Dictionary<string, string> fields, string keyField, bool needQuotes = true)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("update {0} set ", tableName);
            foreach (string fieldName in fields.Keys)
            {
                if (!fieldName.Equals(keyField))
                {
                    if (needQuotes)
                        sqlStr.AppendFormat(" {0} = '{1}',", fieldName, !string.IsNullOrEmpty(fields[fieldName]) ? fields[fieldName].Replace("'", "''") : fields[fieldName]);
                    else
                        sqlStr.AppendFormat(" {0} = {1},", fieldName, fields[fieldName]);
                }
            }

            sqlStr.Remove(sqlStr.Length - 1, 1); // 移除最后一个逗号
            sqlStr.AppendFormat(" where {0} = '{1}'", keyField, fields[keyField]);

            return sqlStr.ToString();
        }

        public static string GenerateInsertSql(string tableName, Dictionary<string, string> fields, bool needQuotes = true)
        {
            if (fields.Count == 0)
                return "";

            StringBuilder sqlStr = new StringBuilder();
            StringBuilder sqlValuesStr = new StringBuilder();

            sqlStr.AppendFormat("insert into {0} (", tableName);
            foreach (string fieldName in fields.Keys)
            {
                sqlStr.AppendFormat("{0},", fieldName);
                if (needQuotes)
                    sqlValuesStr.AppendFormat("'{0}',", !string.IsNullOrEmpty(fields[fieldName]) ? fields[fieldName].Replace("'", "''") : fields[fieldName]);
                else
                    sqlValuesStr.AppendFormat("{0},", fields[fieldName]);
            }

            sqlStr.Remove(sqlStr.Length - 1, 1); // 移除最后一个逗号
            sqlValuesStr.Remove(sqlValuesStr.Length - 1, 1);
            sqlStr.AppendFormat(") values({0})", sqlValuesStr);

            return sqlStr.ToString();
        }

        public static string SearchConditionSqlStr(List<KeyValuePair<string, string>> conditions)
        {
            StringBuilder sql = new StringBuilder();
            foreach (KeyValuePair<string, string> item in conditions)
            {
                string[] typeAndKey = item.Key.Split('|');
                switch (typeAndKey[0])
                {
                    case "e":
                        sql.AppendFormat(" and {0} = {1}", typeAndKey[1], item.Value);
                        break;
                    case "b":
                        sql.AppendFormat(" and ({0} > {1} and {0} < {2})", typeAndKey[1], item.Value.Split('|')[0], item.Value.Split('|')[1]);
                        break;
                    default:
                        sql.AppendFormat(" and {0} like '%{1}%'", item.Key, item.Value);
                        break;
                }
            }

            return sql.ToString();
        }
    }
}