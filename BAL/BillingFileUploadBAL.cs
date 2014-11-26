using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BillingFileUploadBAL
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());


        # region Add AWB file data
        public string AddAWBFileData(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[16];
                SqlDbType[] DataType = new SqlDbType[16];
                Object[] Values = new object[16];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBDate", i);
                DataType.SetValue(SqlDbType.Date, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("AgentName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Pieces", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("GrossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("ChargableWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("FreightRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Surcharge", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TotalCharges", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //ColumnNames.SetValue("AgentCode", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(RateCardInfo.GetValue(i), i);
                //i++;


                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_InsertBillingAWBFileInvMatch", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Add AWB file data
    }
}
