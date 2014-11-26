using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class AirlineMasterBAL
    { 
        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion

        #region Save Airline Data

        public bool SaveAirline(object[] QueryValues)
        {
            try
            {

                string[] QueryNames = new string[35];
                QueryNames[0] = "PartnerName";
                QueryNames[1] = "PartnerLegalName";
                QueryNames[2] = "PartnerPrefix";
                QueryNames[3] = "DesignatorCode";
                QueryNames[4] = "PartnerLocationID";
                QueryNames[5] = "PartnerAccountingCode";
                QueryNames[6] = "RegistrationID";
                QueryNames[7] = "DigitalSignature";
                QueryNames[8] = "Suspend";
                QueryNames[9] = "President";
                QueryNames[10] = "CFO";
                QueryNames[11] = "CurrencyListing";
                QueryNames[12] = "Language";
                QueryNames[13] = "TaxRegistrationID";
                QueryNames[14] = "AdditionalTaxRegistrationID";
                QueryNames[15] = "SettlementMethod";
                QueryNames[16] = "Address";
                QueryNames[17] = "Country";
                QueryNames[18] = "City";
                QueryNames[19] = "PostalCode";
                QueryNames[20] = "CurrencyBilling";
                QueryNames[21] = "UpdatedBy";
                QueryNames[22] = "PartnerType";
                QueryNames[23] = "ValidFrom";
                QueryNames[24] = "ValidTo";
                QueryNames[25] = "IsScheduled";
                QueryNames[26] = "SITAID";
                QueryNames[27] = "EmailID";
                QueryNames[28] = "PartialAcceptance";
                QueryNames[29] = "Tolerance";
                QueryNames[30] = "OtherCharges";
                QueryNames[31] = "CustomsMsg";
                QueryNames[32] = "onbilling";
                QueryNames[33] = "AutoGenInvoice";
                QueryNames[34] = "BillType";

                SqlDbType[] QueryTypes = new SqlDbType[35];

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.VarChar;
                QueryTypes[7] = SqlDbType.VarChar;
                QueryTypes[8] = SqlDbType.VarChar;
                QueryTypes[9] = SqlDbType.VarChar;
                QueryTypes[10] = SqlDbType.VarChar;
                QueryTypes[11] = SqlDbType.VarChar;
                QueryTypes[12] = SqlDbType.VarChar;
                QueryTypes[13] = SqlDbType.VarChar;
                QueryTypes[14] = SqlDbType.VarChar;
                QueryTypes[15] = SqlDbType.VarChar;
                QueryTypes[16] = SqlDbType.VarChar;
                QueryTypes[17] = SqlDbType.VarChar;
                QueryTypes[18] = SqlDbType.VarChar;
                QueryTypes[19] = SqlDbType.VarChar;
                QueryTypes[20] = SqlDbType.VarChar;
                QueryTypes[21] = SqlDbType.VarChar;
                QueryTypes[22] = SqlDbType.VarChar;
                QueryTypes[23] = SqlDbType.DateTime;
                QueryTypes[24] = SqlDbType.DateTime;
                QueryTypes[25] = SqlDbType.VarChar;
                QueryTypes[26] = SqlDbType.VarChar;
                QueryTypes[27] = SqlDbType.VarChar;
                QueryTypes[28] = SqlDbType.VarChar;
                QueryTypes[29] = SqlDbType.Decimal;
                QueryTypes[30] = SqlDbType.VarChar;
                QueryTypes[31] = SqlDbType.VarChar;
                QueryTypes[32] = SqlDbType.VarChar;
                QueryTypes[33] = SqlDbType.Bit;
                QueryTypes[34] = SqlDbType.VarChar;


                if (db.UpdateData("SP_SavePartnerData", QueryNames, QueryTypes, QueryValues))
                { return true; }
                else
                { return false; }



               
            }
            catch (Exception ex)
            { return false; }
        }
        #endregion

        # region Get Partner List
        public DataSet GetPartnerList(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

                //1
                ColumnNames.SetValue("partnertype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("partnername", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("partnerprefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetPartnerList", ColumnNames, Values, DataType);
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
        # endregion Get Partner List

        # region Get Partner List For Id
        public DataSet GetPartnerListForId(string id)
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //1
                ColumnNames.SetValue("Id", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(id, i);
                i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetPartnerListForId", ColumnNames, Values, DataType);
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
        # endregion Get Partner List For Id

    }
}
