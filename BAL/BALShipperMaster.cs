using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Data.SqlClient;

namespace BAL
{

    public class BALShipperMaster
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALShipperMaster()
        {
            constr = Global.GetConnectionString();
        }

        # region Add Shipper Master
        public int AddShipperDetail(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[24];
                SqlDbType[] DataType = new SqlDbType[24];
                Object[] Values = new object[24];
                int i = 0;

                //1
                ColumnNames.SetValue("acccode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("accname", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("adr1", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("adr2", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("city", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("state", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("country", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("zipcode", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("phno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("mobno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //11
                ColumnNames.SetValue("fax", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //12
                ColumnNames.SetValue("email", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //13
                ColumnNames.SetValue("iataaccno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //14
                ColumnNames.SetValue("isact", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //15
                ColumnNames.SetValue("createon", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //16
                ColumnNames.SetValue("createby", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //17
                //ColumnNames.SetValue("updateon", i);
                //DataType.SetValue(SqlDbType.DateTime, i);
                //Values.SetValue(PrefixInfo.GetValue(i), i);
                //i++;

                //18
                //ColumnNames.SetValue("updateby", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(PrefixInfo.GetValue(i), i);
                //i++;

                //18
                ColumnNames.SetValue("type", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //19
                ColumnNames.SetValue("agentcode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //20
                ColumnNames.SetValue("ccsfcode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //21
                ColumnNames.SetValue("CreditAccNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //22
                ColumnNames.SetValue("IsShipper", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //23
                ColumnNames.SetValue("TIN", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //24
                ColumnNames.SetValue("ContactPerson", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //25
                ColumnNames.SetValue("VatExemp", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spAddShipperDetail", ColumnNames, DataType, Values))
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
        # endregion

        # region Update Shipper Master
        public int UpdateShipperDetail(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[27];
                SqlDbType[] DataType = new SqlDbType[27];
                Object[] Values = new object[27];
                int i = 0;

                //1
                ColumnNames.SetValue("acccode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("accname", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("adr1", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("adr2", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("city", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("state", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("country", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("zipcode", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("phno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("mobno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //11
                ColumnNames.SetValue("fax", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //12
                ColumnNames.SetValue("email", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //13
                ColumnNames.SetValue("iataaccno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //14
                ColumnNames.SetValue("isact", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //15
                ColumnNames.SetValue("createon", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //16
                ColumnNames.SetValue("createby", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //17
                ColumnNames.SetValue("updateon", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //18
                ColumnNames.SetValue("updateby", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //19
                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.BigInt, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //20
                ColumnNames.SetValue("type", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //21
                ColumnNames.SetValue("agentcode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //22
                ColumnNames.SetValue("ccsfcode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //23
                ColumnNames.SetValue("CreditAccNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //24
                ColumnNames.SetValue("IsShipper", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //25
                ColumnNames.SetValue("TIN", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //26
                ColumnNames.SetValue("ContactPerson", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //27
                ColumnNames.SetValue("VatExemp", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("spUpdateShipperDetail", ColumnNames, DataType, Values))
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
        # endregion

        # region Delete
        public int DeleteShipperDetail(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //1
                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("spDeleteShipperDetail", ColumnNames, DataType, Values))
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
        # endregion Delete

        # region Get Shipper List
        public DataSet GetShipperList(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

                //1
                ColumnNames.SetValue("acccode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("accname", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("participationtype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetShipperList", ColumnNames, Values, DataType);
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
        # endregion Get Shipper List

        # region Get Agent Details
        public DataSet GetAgentDetails(object[] PrefixInfo)
        {
            try
            {

                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //1
                ColumnNames.SetValue("agentcode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetAgentDetails", ColumnNames, Values, DataType);
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

        # endregion Get Agent Details
    }
}
