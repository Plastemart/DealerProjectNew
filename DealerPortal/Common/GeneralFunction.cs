using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DealerPortal.Common
{
    public class GeneralFunction
    {
        string strDBUserId = System.Configuration.ConfigurationManager.AppSettings["strDBUserId"];
        string strDBUserPassword = System.Configuration.ConfigurationManager.AppSettings["strDBUserPassword"];
        string strDBName = System.Configuration.ConfigurationManager.AppSettings["DBName"];
        string strDBServer = System.Configuration.ConfigurationManager.AppSettings["DBServer"];      


        public string StrSetConnection()
        {
            string MyString = "Database=" + strDBName + ";Server=" + strDBServer + ";User id=" + strDBUserId + ";password=" + strDBUserPassword + ";Connect Timeout=8000000";
            return MyString;
        }

        //public string StrSetConnectionDP()
        //{
        //    string MyString = "Database=" + strDBNameDP + ";Server=" + strDBServerDP + ";User id=" + strDBUserIdDP + ";password=" + strDBUserPasswordDP + ";Connect Timeout=8000000";
        //    return MyString;
        //}

        public bool IsValidColumn(string strName)
        {
            if (strName.Substring(0, 2).ToUpper() != "ZZ" && strName != "DUMMY" && strName != "ZZDumSeq")
                return true;
            else
                return false;
        }

        internal DbType ConvertNullableIntoDatatype(PropertyInfo PropInfoCol)
        {
            DbType dbt = new DbType();
            string strPropertyType = PropInfoCol.PropertyType.Name;
            if (PropInfoCol.PropertyType.Name.Contains("Nullable"))
            {
                if (PropInfoCol.Name.ToUpper() == "DbId" || PropInfoCol.Name.ToUpper() == "DBID")
                {
                    dbt = DbType.Int32;
                }
                else
                {
                    strPropertyType = PropInfoCol.PropertyType.ToString();
                    if (strPropertyType.Contains("DateTime"))
                    {
                        dbt = DbType.DateTime;
                    }
                    else if (strPropertyType.Contains("Int32"))
                    {
                        dbt = DbType.Int32;
                    }
                    else if (strPropertyType.Contains("Decimal"))
                    {
                        dbt = DbType.Decimal;
                    }

                    else if (strPropertyType.Contains("Byte"))
                    {
                        dbt = DbType.Byte;
                    }
                    else if (strPropertyType.Contains("bool") || strPropertyType.Contains("Bool") || strPropertyType.Contains("Boolean"))
                    {
                        dbt = DbType.Boolean;
                    }
                    else if (strPropertyType.Contains("String"))
                    {
                        dbt = DbType.String;
                    }
                    else if (strPropertyType.Contains("Char") || strPropertyType.Contains("char"))
                    {
                        dbt = DbType.String;
                    }
                    else if (strPropertyType.Contains("Time"))
                    {
                        dbt = DbType.Time;
                    }
                    else if (strPropertyType.Contains("Varchar") || strPropertyType.Contains("varchar") || strPropertyType.Contains("VarChar"))
                    {
                        dbt = DbType.String;
                    }
                    else
                    {
                        dbt = (DbType)Enum.Parse(typeof(DbType), PropInfoCol.PropertyType.Name);
                    }
                }
            }
            else
            {
                if (PropInfoCol.Name.ToUpper() == "DbId" || PropInfoCol.Name.ToUpper() == "DBID")
                {
                    dbt = DbType.Int32;
                }
                else
                {
                    if (strPropertyType.Contains("Byte"))
                    {
                        dbt = DbType.Byte;
                    }
                    else if (strPropertyType.Contains("Int32"))
                    {
                        dbt = DbType.Int32;
                    }
                    else if (strPropertyType.Contains("bool"))
                    {
                        dbt = DbType.Boolean;
                    }
                    else if (strPropertyType.Contains("Char") || strPropertyType.Contains("char"))
                    {
                        dbt = DbType.String;
                    }
                    else if (strPropertyType.Contains("Time"))
                    {
                        dbt = DbType.Time;
                    }
                    else
                    {
                        dbt = (DbType)Enum.Parse(typeof(DbType), PropInfoCol.PropertyType.Name);
                    }
                }
            }
            return dbt;
        }

        internal T ConvertTableToListNew<T>(DataRow dr) where T : new()
        {
            string pname = "";
            T objMaster = new T();
            try
            {

                foreach (PropertyInfo property in objMaster.GetType().GetProperties())
                {


                    if (dr.Table.Columns.Contains(property.Name.ToString()))
                    {
                        pname = property.Name;
                        if (dr[property.Name] == DBNull.Value)
                        {
                            property.SetValue(objMaster, null, null);
                        }
                        else if (property.PropertyType.ToString() == "System.Nullable`1[System.Decimal]")
                        {
                            Type Primitive = Nullable.GetUnderlyingType(property.PropertyType);
                            object Temp = Convert.ChangeType(dr[property.Name], Type.GetType(Primitive.FullName), CultureInfo.InvariantCulture);
                            property.SetValue(objMaster, Temp, null);

                        }
                        else
                        {
                            if (property.PropertyType.FullName.ToLower() == "system.char" || (Nullable.GetUnderlyingType(property.PropertyType) != null && Nullable.GetUnderlyingType(property.PropertyType).FullName.ToLower() == "system.char"))
                            {
                                property.SetValue(objMaster, Convert.ToChar(dr[property.Name]), null);
                            }
                            else if (property.PropertyType.FullName.ToLower() == "system.char")
                            {
                                property.SetValue(objMaster, Convert.ToChar(dr[property.Name]), null);
                            }
                            else
                            {
                                property.SetValue(objMaster, dr[property.Name], null);
                            }

                        }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objMaster;
        }
    }
}