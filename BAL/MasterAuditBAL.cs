using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
    
   public class MasterAuditBAL
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        
        public MasterAuditBAL()
        {
            constr = Global.GetConnectionString();
        }
        # region Add Master Audit Log
        public bool AddMasterAuditLog(object[] MasterInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[7];
                SqlDbType[] DataType = new SqlDbType[7];
                Object[] Values = new object[7];
                int i = 0;

                //1
                ColumnNames.SetValue("Master", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MasterInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("MasterValue", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MasterInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("Action", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MasterInfo.GetValue(i), i);
                i++;


                //4
                ColumnNames.SetValue("Message", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MasterInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("Description", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MasterInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("UpdatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MasterInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MasterInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("SPAddMasterAuditLog", ColumnNames, DataType, Values))
                    return false;
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        # endregion Add Master Audit Log

        # region Add AWB Audit Log
        public bool AddAWBAuditLog(string AWBPrefix,string AWBNumber,string Origin,string Destination,string Pieces,
            string Weight,string FlightNo,string FlightDate,string FlightOrigin,string FlightDestination, string Action,
            string Message,string Description,string UpdatedBy,string UpdatedOn,bool isInternal)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[15];
                SqlDbType[] DataType = new SqlDbType[15];
                Object[] Values = new object[15];

                int ValidateInteger = 0;
                decimal ValidateDecimal = 0;
                DateTime ValidateDateTime;
                int i = 0;

                //1
                ColumnNames.SetValue("AWBPrefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBPrefix, i);
                i++;

                //2
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBNumber, i);
                i++;

                //3
                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Origin, i);
                i++;

                //4
                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Destination, i);
                i++;

                //5
                Pieces = int.TryParse(Pieces, out ValidateInteger) == true ? Pieces : "0";
                ColumnNames.SetValue("Pieces", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Pieces, i);
                i++;

                //6
                Weight = decimal.TryParse(Weight, out ValidateDecimal) == true ? Weight : "0";
                ColumnNames.SetValue("Weight", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Weight, i);
                i++;

                //7
                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightNo, i);
                i++;

                //8
                //FlightDate = DateTime.TryParse(FlightDate, out ValidateDateTime) == true ? FlightDate : string.Empty;
                ColumnNames.SetValue("FlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightDate, i);
                i++;

                //9
                ColumnNames.SetValue("FlightOrigin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightOrigin, i);
                i++;

                //10
                ColumnNames.SetValue("FlightDestination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightDestination, i);
                i++;

                //11
                ColumnNames.SetValue("Action", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Action, i);
                i++;


                //12
                ColumnNames.SetValue("Message", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Message, i);
                i++;

                //13
                ColumnNames.SetValue("Description", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Description, i);
                i++;

                //14
                ColumnNames.SetValue("UpdatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdatedBy, i);
                i++;

                //15
                UpdatedOn = DateTime.TryParse(UpdatedOn, out ValidateDateTime) == true ? UpdatedOn : string.Empty;
                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdatedOn, i);
                i++;

                //16
                //ColumnNames.SetValue("isInternal", i);
                //DataType.SetValue(SqlDbType.Bit, i);
                //Values.SetValue(isInternal, i);
                //i++;

                if (!da.ExecuteProcedure("SPAddAWBAuditLog", ColumnNames, DataType, Values))
                    return false;
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        # endregion Add AWB Audit Log

        # region Add Billing Audit Log
        public bool AddBillingAuditLog(string AWBPrefix, string AWBNumber, string Origin, string Destination, string AgentCode,
            string Status, string FlightNo, string FlightDate, string ReferenceNo, string Freight, string OCDC,
            string OCDA, string ServiceTax,string Total, string UpdatedBy, string UpdatedOn)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[16];
                SqlDbType[] DataType = new SqlDbType[16];
                Object[] Values = new object[16];

                float ValidateFloat = 0;
                DateTime ValidateDateTime;
                int i = 0;

                //1
                ColumnNames.SetValue("AWBPrefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBPrefix, i);
                i++;

                //2
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBNumber, i);
                i++;

                //3
                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Origin, i);
                i++;

                //4
                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Destination, i);
                i++;


                //5
                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightNo, i);
                i++;

                //6
                FlightDate = DateTime.TryParse(FlightDate, out ValidateDateTime) == true ? FlightDate : string.Empty;
                ColumnNames.SetValue("FlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightDate, i);
                i++;

                //7
                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AgentCode, i);
                i++;

                //8
                ColumnNames.SetValue("Status", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Status, i);
                i++;

                //9
                ColumnNames.SetValue("ReferenceNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ReferenceNo, i);
                i++;

                //10
                Freight = float.TryParse(Freight, out ValidateFloat) == true ? Freight : "0";
                ColumnNames.SetValue("Freight", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Freight, i);
                i++;

                //11
                OCDC = float.TryParse(OCDC, out ValidateFloat) == true ? OCDC : "0";
                ColumnNames.SetValue("OCDC", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(OCDC, i);
                i++;

                //12
                OCDA = float.TryParse(OCDA, out ValidateFloat) == true ? OCDA : "0";
                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(OCDA, i);
                i++;

                //13
                ServiceTax = float.TryParse(ServiceTax, out ValidateFloat) == true ? ServiceTax : "0";
                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ServiceTax, i);
                i++;

                //14
                Total = float.TryParse(Total, out ValidateFloat) == true ? Total : "0";
                ColumnNames.SetValue("Total", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Total, i);
                i++;

               
                //15
                ColumnNames.SetValue("UpdatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdatedBy, i);
                i++;

                //16
                UpdatedOn = DateTime.TryParse(UpdatedOn, out ValidateDateTime) == true ? UpdatedOn : string.Empty;
                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdatedOn, i);
                i++;

               

                if (!da.ExecuteProcedure("SPAddBillingAuditLog", ColumnNames, DataType, Values))
                    return false;
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        # endregion Add Billing Audit Log
    }
}
