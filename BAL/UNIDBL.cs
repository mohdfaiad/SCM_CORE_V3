using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Configuration;

namespace BAL
{
    public class UNIDBL
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());

        #endregion Variables

        # region AddModifyUNIDDetails
        public string AddModifyUNIDDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[28];
                SqlDbType[] DataType = new SqlDbType[28];
                Object[] Values = new object[28];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("UNIDNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("RadioActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("RMC", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("ShippingName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("ClassDiv", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("ImpCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("Technical", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("SubRisk", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("HazardLabels", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("Description", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("PG", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //11
                ColumnNames.SetValue("SP", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //12
                ColumnNames.SetValue("ErgCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //13
                ColumnNames.SetValue("ForbiddenPCA", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //14
                ColumnNames.SetValue("NoLimitLQ", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //15
                ColumnNames.SetValue("PILQ", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //16
                ColumnNames.SetValue("MaxQtyLQ", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //17
                ColumnNames.SetValue("NoLimitULQ", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //18
                ColumnNames.SetValue("PIULQ", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //19
                ColumnNames.SetValue("MaxQtyULQ", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //20
                ColumnNames.SetValue("ForbiddenCA", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //21
                ColumnNames.SetValue("NoLimitCA", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //22
                ColumnNames.SetValue("PICA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //23
                ColumnNames.SetValue("MaxQtyCA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //24
                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //25
                ColumnNames.SetValue("Flag", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                //26
                ColumnNames.SetValue("IsAct", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                //27
                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                

                string res = db.GetStringByProcedure("SP_AddModifyUNIDDetails", ColumnNames,Values,DataType);
                return res;
            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion AddModifyUNIDDetails

        #region Get UNID Details
        public DataSet GetUNIDDetails(string LoginID)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryPname = new string[1];
                object[] QueryValue = new object[1];
                SqlDbType[] QueryType = new SqlDbType[1];

                //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

                QueryPname[0] = "UnidNo";
                QueryType[0] = SqlDbType.VarChar;
                QueryValue[0] = LoginID;

                ds = da.SelectRecords("SP_GetUNIDDetails", QueryPname, QueryValue, QueryType);


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

                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        #endregion Get UNID Details

        #region deleteUNID
        public DataSet DelUNIDDetails(string UNID)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryPname = new string[1];
                object[] QueryValue = new object[1];
                SqlDbType[] QueryType = new SqlDbType[1];

                //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

                QueryPname[0] = "UNID";
                QueryType[0] = SqlDbType.VarChar;
                QueryValue[0] = UNID;

                ds = da.SelectRecords("SpDeleteUNID", QueryPname, QueryValue, QueryType);


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

                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        #endregion deleteUNID
    }
}
