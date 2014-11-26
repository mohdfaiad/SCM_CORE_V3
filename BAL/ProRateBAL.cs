using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
   public class ProRateBAL
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public ProRateBAL()
        {
            constr = Global.GetConnectionString();
        }

        # region Get Origin Code
        public DataSet GetOriginCodeList(string OriginCodeList)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("GetProRateOriginCodeList");
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
        # endregion Get Currency Code 

        # region Get Destination Code
        public DataSet GetDestinationCodeList(string DestinationCodeList)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("GetProRateDestinationCodeList");
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

        # endregion Get Currency Code 

        # region Get ProRate List
        public DataSet spGetProRateCodeList(object[] ProRateListInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                //0
                ColumnNames.SetValue("OriginCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProRateListInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("DestCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ProRateListInfo.GetValue(i), i);
                i++;

                



                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetProRateCodeList", ColumnNames, Values, DataType);
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
        # endregion Get Pro Rate List

        # region Delete Pro Rate
        public int DeleteProRate(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //1
                ColumnNames.SetValue("id", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spDeleteProRate", ColumnNames, DataType, Values))
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
        # endregion Delete Pro Rate

        # region Add Pro Rate
        public int AddProRate(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[7];
                SqlDbType[] DataType = new SqlDbType[7];
                Object[] Values = new object[7];
                int i = 0;

                //1
                ColumnNames.SetValue("origincode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("destcode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("proratefact", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;
                
                
                //4
                ColumnNames.SetValue("constrfact", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;
                
                //5
                ColumnNames.SetValue("validfrm", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("validto", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;
                
                //7
                ColumnNames.SetValue("isact", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spAddProRate", ColumnNames, DataType, Values))
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
        # endregion Add Pro Rate

        # region Update Pro Rate
        public int UpdateProRate(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[8];
                SqlDbType[] DataType = new SqlDbType[8];
                Object[] Values = new object[8];
                int i = 0;

                //0
                ColumnNames.SetValue("id", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("origincode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("destcode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("proratefact", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;


                //4
                ColumnNames.SetValue("constrfact", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("validfrm", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("validto", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("isact", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spUpdateProRate", ColumnNames, DataType, Values))
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
        # endregion Add Pro Rate


    }
}
