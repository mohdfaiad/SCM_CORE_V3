using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BillingInterlineInvoiceBAL
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());

        #endregion Variables

        public DataSet GetAllCarriers()
        {
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SP_GetPartnerCodeName");
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
            catch (Exception)
            {
            }
            return (null);
        }

        # region Get Interline Invoice AWB List
        public DataSet GetInterlineInvoiceAWBList(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[10];
                SqlDbType[] DataType = new SqlDbType[10];
                Object[] Values = new object[10];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Status", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("SpotRate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("PaymentMode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);



                DataSet ds = db.SelectRecords("SP_GetInterlineInvoiceAWBList", ColumnNames, Values, DataType);
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

            catch (Exception)
            {

            }
            return (null);
        }

        # endregion Get Interline Invoice AWB List

        # region Get Interline Credit Note AWB List
        public DataSet GetInterlineCreditNoteAWBList(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[10];
                SqlDbType[] DataType = new SqlDbType[10];
                Object[] Values = new object[10];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Status", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("SpotRate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("PaymentMode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);



                DataSet ds = db.SelectRecords("SP_GetInterlineCreditNoteAWBList", ColumnNames, Values, DataType);
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

            catch (Exception)
            {

            }
            return (null);
        }

        # endregion Get Interline Credit Note AWB List

        # region Get Interline Invoice AWB Commodity List
        public DataSet GetInterlineInvoiceAWBCommodityList(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PartnerCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                

                DataSet ds = db.SelectRecords("SP_GetInterlineInvoiceAWBCommodityList", ColumnNames, Values, DataType);
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

            catch (Exception)
            {

            }
            return (null);
        }

        # endregion Get Interline Invoice AWB Commodity List

        # region Get Interline Credit Note AWB Commodity List
        public DataSet GetInterlineCreditNoteAWBCommodityList(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PartnerCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);


                DataSet ds = db.SelectRecords("SP_GetInterlineCreditNoteAWBCommodityList", ColumnNames, Values, DataType);
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

            catch (Exception)
            {

            }
            return (null);
        }

        # endregion Get Interline Credit Note AWB Commodity List

        # region ConfirmSingleInterLineInvoiceAWB
        public string ConfirmSingleInterLineInvoiceAWB(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[25];
                SqlDbType[] DataType = new SqlDbType[25];
                Object[] Values = new object[25];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CommodityCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Dimensions", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("GrossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("ChargableWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("FreightRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("OCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Total", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Discount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DiscountAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TAD", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Commission", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("CommissionAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RevisedTotal", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreight", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreightAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSOnComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnCommAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FinalAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PartnerCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_ConfirmSingleInterLineInvoiceAWB", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion ConfirmSingleInterLineInvoiceAWB

        # region ConfirmSingleInterLineCreditNoteAWB
        public string ConfirmSingleInterLineCreditNoteAWB(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[25];
                SqlDbType[] DataType = new SqlDbType[25];
                Object[] Values = new object[25];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CommodityCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Dimensions", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("GrossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("ChargableWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("FreightRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("OCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Total", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Discount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DiscountAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TAD", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Commission", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("CommissionAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RevisedTotal", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreight", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreightAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSOnComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnCommAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FinalAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PartnerCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_ConfirmSingleInterLineCreditNoteAWB", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion ConfirmSingleInterLineCreditNoteAWB

        # region FinalizeSingleInterLineInvoiceAWB
        public string FinalizeSingleInterLineInvoiceAWB(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[24];
                SqlDbType[] DataType = new SqlDbType[24];
                Object[] Values = new object[24];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CommodityCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Dimensions", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("GrossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("ChargableWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("FreightRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("OCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Total", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Discount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DiscountAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TAD", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Commission", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("CommissionAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RevisedTotal", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreight", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreightAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSOnComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnCommAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FinalAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);




                string res = db.GetStringByProcedure("SP_FinalizeSingleInterLineInvoiceAWB", ColumnNames, Values, DataType);
                return res;


            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion FinalizeSingleInterLineInvoiceAWB

        # region FinalizeSingleInterLineCreditNoteAWB
        public string FinalizeSingleInterLineCreditNoteAWB(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[24];
                SqlDbType[] DataType = new SqlDbType[24];
                Object[] Values = new object[24];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CommodityCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Dimensions", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("GrossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("ChargableWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("FreightRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("OCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Total", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Discount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DiscountAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TAD", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Commission", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("CommissionAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RevisedTotal", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreight", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreightAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSOnComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnCommAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FinalAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);




                string res = db.GetStringByProcedure("SP_FinalizeSingleInterLineCreditNoteAWB", ColumnNames, Values, DataType);
                return res;


            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion FinalizeSingleInterLineCreditNoteAWB

        # region FinalizeSelectedInterLineInvoiceAWB
        public string FinalizeSelectedInterLineInvoiceAWB(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);




                string res = db.GetStringByProcedure("SP_FinalizeSelectedInterLineInvoiceAWB", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion FinalizeSelectedInterLineInvoiceAWB

        # region FinalizeSelectedInterLineCreditNoteAWB
        public string FinalizeSelectedInterLineCreditNoteAWB(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);




                string res = db.GetStringByProcedure("SP_FinalizeSelectedInterLineCreditNoteAWB", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion FinalizeSelectedInterLineCreditNoteAWB

        # region UndoFinalizeSingleInterLineInvoiceAWB
        public string UndoFinalizeSingleInterLineInvoiceAWB(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = db.GetStringByProcedure("SP_UndoFinalizeSingleInterLineInvoiceAWB", ColumnNames, Values, DataType);
                return res;


            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UndoFinalizeSingleInterLineInvoiceAWB

        # region UndoFinalizeSingleInterLineCreditNoteAWB
        public string UndoFinalizeSingleInterLineCreditNoteAWB(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = db.GetStringByProcedure("SP_UndoFinalizeSingleInterLineCreditNoteAWB", ColumnNames, Values, DataType);
                return res;


            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UndoFinalizeSingleInterLineCreditNoteAWB

        # region UndoFinalizeSelectedInterLineInvoiceAWB
        public string UndoFinalizeSelectedInterLineInvoiceAWB(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = db.GetStringByProcedure("SP_UndoFinalizeSelectedInterLineInvoiceAWB", ColumnNames, Values, DataType);
                return res;


            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UndoFinalizeSelectedInterLineInvoiceAWB

        # region UndoFinalizeSelectedInterLineCreditNoteAWB
        public string UndoFinalizeSelectedInterLineCreditNoteAWB(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = db.GetStringByProcedure("SP_UndoFinalizeSelectedInterLineCreditNoteAWB", ColumnNames, Values, DataType);
                return res;


            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UndoFinalizeSelectedInterLineCreditNoteAWB

        # region GenerateBunchProformaInterlineInvoiceNumber
        public string GenerateBunchProformaInterlineInvoiceNumber(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;



                string res = db.GetStringByProcedure("SP_GenerateBunchProInterLineInvoiceNumInvMatch", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion GenerateBunchProformaInterlineInvoiceNumber

        # region GenerateBunchProformaInterlineCreditNoteNumber
        public string GenerateBunchProformaInterlineCreditNoteNumber(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;



                string res = db.GetStringByProcedure("SP_GenerateBunchProInterLineCreditNoteNumInvMatch", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion GenerateBunchProformaInterlineCreditNoteNumber

        # region GetProformaInterlineInvoiceNumber
        public DataSet GetProformaInterlineInvoiceNumber(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                i++;
                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);


                DataSet ds = db.SelectRecords("SP_GetProformaInterlineInvoiceNumber", ColumnNames, Values, DataType);
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

            catch (Exception)
            {

            }
            return (null);
        }
        # endregion GetProformaInterlineInvoiceNumber

        # region GetProformaInterlineCreditNoteNumber
        public DataSet GetProformaInterlineCreditNoteNumber(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                i++;
                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);


                DataSet ds = db.SelectRecords("SP_GetProformaInterlineCreditNoteNumber", ColumnNames, Values, DataType);
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

            catch (Exception)
            {

            }
            return (null);
        }
        # endregion GetProformaInterlineCreditNoteNumber

        # region UpdateBillingProformaInterlineInvoiceSummary
        public string UpdateBillingProformaInterlineInvoiceSummary(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                

                string res = db.GetStringByProcedure("SP_UpdateBillingProformaInterlineInvoiceSummary", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UpdateBillingProformaInterlineInvoiceSummary

        # region UpdateBillingProformaInterlineCreditNoteSummary
        public string UpdateBillingProformaInterlineCreditNoteSummary(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                

                string res = db.GetStringByProcedure("SP_UpdateBillingProformaInterlineCreditNoteSummary", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UpdateBillingProformaInterlineCreditNoteSummary

        # region GenerateBunchInterlineInvoiceNumber
        public string GenerateBunchInterlineInvoiceNumber(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;



                string res = db.GetStringByProcedure("SP_GenerateBunchInterLineInvoiceNumInvMatch", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion GenerateBunchInterlineInvoiceNumber

        # region GenerateBunchInterlineCreditNoteNumber
        public string GenerateBunchInterlineCreditNoteNumber(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;



                string res = db.GetStringByProcedure("SP_GenerateBunchInterLineCreditNoteNumInvMatch", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion GenerateBunchInterlineCreditNoteNumber

        # region GetInterlineInvoiceNumber
        public DataSet GetInterlineInvoiceNumber(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                i++;
                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);


                DataSet ds = db.SelectRecords("SP_GetInterlineInvoiceNumber", ColumnNames, Values, DataType);
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

            catch (Exception)
            {

            }
            return (null);
        }
        # endregion GetInterlineInvoiceNumber

        # region GetInterlineCreditNoteNumber
        public DataSet GetInterlineCreditNoteNumber(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                i++;
                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);


                DataSet ds = db.SelectRecords("SP_GetInterlineCreditNoteNumber", ColumnNames, Values, DataType);
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

            catch (Exception)
            {

            }
            return (null);
        }
        # endregion GetInterlineCreditNoteNumber

        # region UpdateBillingInterlineInvoiceSummary
        public string UpdateBillingInterlineInvoiceSummary(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = db.GetStringByProcedure("SP_UpdateBillingInterlineInvoiceSummary", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UpdateBillingInterlineInvoiceSummary

        # region UpdateBillingInterlineCreditNoteSummary
        public string UpdateBillingInterlineCreditNoteSummary(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_UpdateBillingInterlineCreditNoteSummary", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UpdateBillingInterlineCreditNoteSummary

    }
}
