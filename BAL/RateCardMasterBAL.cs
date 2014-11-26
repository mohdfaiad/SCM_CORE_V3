using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class RateCardMasterBAL
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());


        # region AddRateCard
        public string AddRateCard(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[21];
                SqlDbType[] DataType = new SqlDbType[21];
                Object[] Values = new object[21];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("RateCardName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("RateCardType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("StartDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("EndDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("Status", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("FlightNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightCarrier", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("HandlingCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("AirlineCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("IATACommCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("ShipperCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("FNInc", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FCInc", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("HCInc", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("ACInc", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("CCInc", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("ADInc", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SCInc", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);

                i++;

                ColumnNames.SetValue("rateCardSrNo", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);

                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_InsertRateCard", ColumnNames, Values, DataType);
                return res;

                //if (!db.ExecuteProcedure("SP_InsertRateCard", ColumnNames, DataType, Values))
                //    return (-1);
                //else
                //{
                //    return (0);
                //}
                

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion AddRateCard

        #region GetRateCard
        public DataSet GetRateCard(string rateCardName)
        {
            //int rateCardId = Convert.ToInt32(rateCardName);
            string[] ColumnNames = new string[1];
            SqlDbType[] DataType = new SqlDbType[1];
            Object[] Values = new object[1];

            ColumnNames[0] = "RateCardId";
            DataType[0] = SqlDbType.VarChar;
            Values[0] = rateCardName;

            DataSet dsRateCard =  new DataSet();

            dsRateCard = db.SelectRecords("SP_GetRateCard",ColumnNames, Values, DataType);
            return dsRateCard ;
        }
        #endregion GetRateCard

    }
}
