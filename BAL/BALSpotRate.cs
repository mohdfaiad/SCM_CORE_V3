using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
   public class BALSpotRate
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;
        DataSet ds;
        //SQLServer da = new SQLServer(constr);
        public BALSpotRate()
        {
           constr = Global.GetConnectionString();
        }
        string AWBNumber = "", Origin = "", Destination = "", AgentName = "", AgentCode = "", FlightNumber = "", Commodity = "", SpotCategory = "", station = "", FWDName = "", Remarks = "", Reason = "";
        string IssuedBy = "", AuthorisedBy = "";
        double Weight, volume, thresholdLimit;
        DateTime FltDate, ReqDate, IssueDate, AuthorisedDate, validfrom, validto;
        #region SavepotRate
        public DataSet SaveSpotRate(string AWBNumber, string Origin, string Destination, string AgentName, string currency,string AgentCode, string FlightNumber, string Commodity, string SpotCategory, string station, string FWDName, string Remarks, string Reason, int commissionable,
        string IssuedBy, string AuthorisedBy, string specialapproval, double Weight, double volume, double thresholdLimit, 
            double spotRate, DateTime FltDate, DateTime ReqDate, DateTime IssueDate, DateTime AuthorisedDate, 
            DateTime validfrom, DateTime validto, int NonCommissionable, string UpdatedBy, DateTime UpdatedOn,string WtCat)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                //DataSet ds = new DataSet();
                DataSet res=new DataSet();
                string[] Pname = new string[31];
                object[] Pvalue = new object[31];
                SqlDbType[] Ptype = new SqlDbType[31];

                Pname[0] = "AWBNumber";
                Pname[1] = "Origin";
                Pname[2] = "Destination";
                Pname[3] = "AgentCode";
                Pname[4] = "AgentName";
                Pname[5] = "Weight";
                Pname[6] = "Volume";
                Pname[7] = "FlightNumber";
                Pname[8] = "FlightDate";
                Pname[9] = "Commodity";
                Pname[10] = "SpotRate";
                Pname[11] = "Commissionable";
                Pname[12] = "Noncommissionable";
                Pname[13] = "ThresholdLimit";
                Pname[14] = "Currency";
                Pname[15] = "Station";
                Pname[16] = "RequestDate";
                Pname[17] = "FWDName";
                Pname[18] = "Remarks";
                Pname[19] = "SpecialApproval";
                Pname[20] = "Reason";
                Pname[21] = "IssuedBy";
                Pname[22] = "IssueDate";
                Pname[23] = "AuthorisedBy";
                Pname[24] = "AuthorisedDate";
                Pname[25] = "ValidFrom";
                Pname[26] = "ValidTo";
                Pname[27] = "spotRateCategory";
                Pname[28] = "UpdatedBy";
                Pname[29] = "UpdatedOn";
                Pname[30] = "WtCategory";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.Float;
                Ptype[6] = SqlDbType.Float;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.DateTime;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.Float;
                Ptype[11] = SqlDbType.Bit;
                Ptype[12] = SqlDbType.Bit;
                Ptype[13] = SqlDbType.Float;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.DateTime;
                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.VarChar;
                Ptype[19] = SqlDbType.VarChar;
                Ptype[20] = SqlDbType.VarChar;
                Ptype[21] = SqlDbType.VarChar;
                Ptype[22] = SqlDbType.DateTime;
                Ptype[23] = SqlDbType.VarChar;
                Ptype[24] = SqlDbType.DateTime;
                Ptype[25] = SqlDbType.DateTime;
                Ptype[26] = SqlDbType.DateTime;
                Ptype[27] = SqlDbType.VarChar;
                Ptype[28] = SqlDbType.VarChar;
                Ptype[29] = SqlDbType.DateTime;
                Ptype[30] = SqlDbType.VarChar;
                //-------------------------------------------------------


                Pvalue[0] = AWBNumber ;
                Pvalue[1] = Origin ;
                Pvalue[2] = Destination;
                Pvalue[3] = AgentCode ;
                Pvalue[4] = AgentName ;
                Pvalue[5] = Weight ;
                Pvalue[6] = volume;
                Pvalue[7] = FlightNumber;
                Pvalue[8] = FltDate;
                Pvalue[9] = Commodity; 
                Pvalue[10] = spotRate;
                Pvalue[11] = commissionable;
                Pvalue[12] = NonCommissionable;
                Pvalue[13] = thresholdLimit;
                Pvalue[14] = currency;
                Pvalue[15] = station;
                Pvalue[16] = ReqDate;
                Pvalue[17] = FWDName;
                Pvalue[18] = Remarks;
                Pvalue[19] = specialapproval;
                Pvalue[20] = Reason;
                Pvalue[21] = IssuedBy;
                Pvalue[22] = IssueDate;
                Pvalue[23] = AuthorisedBy;
                Pvalue[24] = AuthorisedDate;
                Pvalue[25] = validfrom;
                Pvalue[26] = validto;
                Pvalue[27] = SpotCategory;
                Pvalue[28] = UpdatedBy;
                Pvalue[29] = UpdatedOn;
                Pvalue[30] = WtCat;

                res = da.SelectRecords("Sp_SaveSpotRateNew", Pname, Pvalue, Ptype);
                 if (res != null)
                 {
                     if (res.Tables[0].Rows.Count > 0)
                     {
                         return res;
                     }
                 }
            }
            catch (Exception ex)
            {
                return res;
            }
            return res;
        }

        #endregion SptRate
        #region GetAWB
        public DataSet GetAWBDetails(string AWBNumber, string AWBPrefix)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] paramname = new string[2];
                paramname[0] = "AWBNumber";
                paramname[1] = "AWBPrefix";
                object[] paramvalue = new object[2];
                paramvalue[0] = AWBNumber ;
                paramvalue[1] = AWBPrefix;
                SqlDbType[] paramtype = new SqlDbType[2];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                ds = da.SelectRecords("SpGetAWBDetailsNew", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                return ds;
            }
            return ds;


        }

        #endregion GetAWB

        #region SpotRateDetails
        public DataSet GetSpotRateDetails(string Source, string destination, string FlightNo, string AgentCode, string FlightDate, string SpotRateID, int ID, string AWBNumber,
            string dtFromDt, string DtToDt, string Status)
        {
            try
            {
            
                SQLServer da = new SQLServer(constr);
                string[] paramname = new string[11];
                paramname[0] = "AgentCode";
                paramname[1] = "FlightNumber";
                paramname[2] = "FlightDate";
                paramname[3] = "Origin";
                paramname[4] = "Destination";
                paramname[5] = "SpotRateID";
                paramname[6] = "ID";
                paramname[7] = "AWBNumber";
                paramname[8] = "dtfromDt";
                paramname[9] = "dtToDt";
                paramname[10] = "Status";

                object[] paramvalue = new object[11];
                paramvalue[0] = AgentCode;
                paramvalue[1] = FlightNo;
                paramvalue[2] = FlightDate;
                paramvalue[3] = Source;
                paramvalue[4] = destination;
                paramvalue[5] = SpotRateID;
                paramvalue[6] = ID;
                paramvalue[7] =AWBNumber;
                paramvalue[8] = dtFromDt;
                paramvalue[9] = DtToDt;
                paramvalue[10] = Status;

                SqlDbType[] paramtype = new SqlDbType[11];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.Int;
                paramtype[7] = SqlDbType.VarChar;
                paramtype[8] = SqlDbType.VarChar;
                paramtype[9] = SqlDbType.VarChar;
                paramtype[10] = SqlDbType.VarChar;

                ds = da.SelectRecords("Sp_GetSpotRateDetailsNewV2", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                return ds;
            }
            return ds;


        }
        #endregion SpotRateDetails

        public bool UpdateStatusApproval(string AWBNumber, string spotrate, string UserName, DateTime UpdatedOn,string Status,string SpotID)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] paramname = new string[6];
                paramname[0] = "AWBNumber";
                paramname[1] = "Status";
                paramname[2] = "spotrate";
                paramname[3] = "UpdatedBy";
                paramname[4] = "UpdatedOn";
                paramname[5] = "SpotID"; 

                object[] paramvalue = new object[6];
                paramvalue[0] = AWBNumber;
                paramvalue[1] = Status;// "Approved";
                paramvalue[2] = spotrate;
                paramvalue[3] = UserName;
                paramvalue[4] = UpdatedOn;
                paramvalue[5] = SpotID;
 
                SqlDbType[] paramtype = new SqlDbType[6];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.DateTime;
                paramtype[5] = SqlDbType.VarChar;

                bool res = da.InsertData("SpUpdateApproveStatusNew", paramname, paramtype, paramvalue);   
                if (res == true)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        
        }

        #region Get Spot Rate Details
        public DataSet GetSpotRateDetails(string AWBNumber,string SpotID)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] paramname = new string[2];
                paramname[0] = "AWBNumber";
                paramname[1] = "SpotID";
                
                object[] paramvalue = new object[2];
                paramvalue[0] = AWBNumber;
                paramvalue[1] = SpotID;
                
                SqlDbType[] paramtype = new SqlDbType[2];
                paramtype[0] = SqlDbType.VarChar; 
                paramtype[1] = SqlDbType.VarChar;
                ds = da.SelectRecords("SpGetSpotRateDetails", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                return ds;
            }
            return ds;


        }

        #endregion Get Spot Rate Details
        #region Get Spot Rate Details
        public DataSet GetSpotRateDetails(string AWBNumber)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] paramname = new string[1];
                paramname[0] = "AWBNumber";
                object[] paramvalue = new object[1];
                paramvalue[0] = AWBNumber;
                SqlDbType[] paramtype = new SqlDbType[1];
                paramtype[0] = SqlDbType.VarChar;
                ds = da.SelectRecords("SpGetSpotRateDetails", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                return ds;
            }
            return ds;


        }

        #endregion Get Spot Rate Details


    }
}

