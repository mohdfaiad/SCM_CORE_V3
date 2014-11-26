using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
   public class SpecialHandlingCodeMasterBAL
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public SpecialHandlingCodeMasterBAL()
        {
            constr = Global.GetConnectionString();
        }

        # region chkSpecHandlingCodeList
        public DataSet chkSpecHandlingCodeList(object[] SpecHandlingcodeInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;


                //0
                ColumnNames.SetValue("SpecialHandlingCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(SpecHandlingcodeInfo.GetValue(i), i);
                i++;

                

                DataSet ds = new DataSet();
                ds = da.SelectRecords("SPCheckSpecialHandlingCodeList", ColumnNames, Values, DataType);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            return (ds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return (null);
        }

        # endregion chkSpecHandlingCodeList

        # region AddSpecHandlingCode
        public int AddSpecHandlingCode(object[] SpecHandlingInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;

                //0
                ColumnNames.SetValue("SpecialHandelingCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(SpecHandlingInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("SpecialHandlingDeacription", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(SpecHandlingInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(SpecHandlingInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("IsShipper", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(SpecHandlingInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("isNoToc", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(SpecHandlingInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("SpAddSpecHandlingCodeMaster", ColumnNames, DataType, Values))
                    return (-1);
                else
                {
                    return (0);
                }
            }
            catch (Exception ex)
            {
                return (-1);
            }

        }
        # endregion AddSpecHandlingCode

        # region UpdateSpecHandlingCode
        public int UpdateSpecHandlingCode(object[] UpdateCountryInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[6];
                SqlDbType[] DataType = new SqlDbType[6];
                Object[] Values = new object[6];
                int i = 0;

                //0
                ColumnNames.SetValue("SpecialHandelingCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("SpecialHandlingDeacription", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //3 
                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("IsShipper", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("isNoToc", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);

                if (!da.ExecuteProcedure("SpUpdateSpecHandlingCode", ColumnNames, DataType, Values))
                    return (-1);
                else
                {
                    return (0);
                }
            }
            catch (Exception ex)
            {
                return (-1);
            }

        }
        # endregion UpdateSpecHandlingCode

        # region Get spGetSpecialHandlllingCodeList List
        public DataSet spGetSpecialHandlllingCodeList(object[] SpecHandlingListInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                //0
                ColumnNames.SetValue("SpecialHandelingCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(SpecHandlingListInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("SpecialHandlingDeacription", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(SpecHandlingListInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(SpecHandlingListInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("isNoToc", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(SpecHandlingListInfo.GetValue(i), i);
                i++;


                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetSpecHandlingCodeList", ColumnNames, Values, DataType);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            return (ds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return (null);
        }

        # endregion Get spGetSpecialHandlllingCodeList List

        public bool CheckDuplicate(object[] values, ref bool IsDuplicate, ref string errormessage)
        {
            try
            {
                string[] param = {"SpecialHandelingCode"
                                };

                SqlDbType[] dbtypes = { SqlDbType.VarChar
                                      };

                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = da.SelectRecords("SP_CheckDuplicateSHCCode", param, values, dbtypes);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {
                            IsDuplicate = bool.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                            return true;
                        }
                        else
                        {
                            errormessage = "Error : (CheckDuplicate) Code III";
                            return false;
                        }
                    }
                    else
                    {
                        errormessage = "Error : (CheckDuplicate) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error : (CheckDuplicate) Code I";
                    return false;
                }

                return false;

            }
            catch (Exception ex)
            {
                errormessage = "Error :(CheckDuplicate)" + ex.Message;
                return false;
            }

        }
    }

    
}
