using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
    public class BALPLIMaster
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res1;
        public BALPLIMaster()
        {
           constr = Global.GetConnectionString();
        }
        #region SaveData
        public bool savedata(int From,int To,double Percentage,string SpotRate)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[4];
                object[] Pvalue = new object[4];
                SqlDbType[] Ptype = new SqlDbType[4];

                Pname[0] = "FromRange";
                Pname[1] = "ToRange";
                Pname[2] = "Percentage";
                Pname[3] = "SpotRate"; ; 

                Pvalue[0] = From;
                Pvalue[1] = To;
                Pvalue[2] = Percentage;
                Pvalue[3] = SpotRate;  

                Ptype[0] = SqlDbType.Int;
                Ptype[1] = SqlDbType.Int;
                Ptype[2] = SqlDbType.Float;
                Ptype[3] = SqlDbType.VarChar;   

                bool res = da.InsertData("SpInsertPLIMaster", Pname, Ptype, Pvalue);

                return res;
            }
            catch (Exception ex)
            {
                return false;            
            }
        
        }
#endregion SaveData

        #region MakePLIInActive
        public bool MakePLIInActive()
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string strQuery = "Update tblPLImaster Set IsActive = 'False' Where IsActive = 'True'";

                bool res = da.GetBoolean(strQuery);

                if (res)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion MakePLIInActive

        #region GetPLI
        public DataSet GetPLI()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                
                string strQuery = "Select FromRange,ToRange,Percentage,SpotRate from dbo.tblPLIMaster where IsActive='True'";

                 res1 = da.GetDataset(strQuery);
                 if (res1 != null)
                 {
                     if (res1.Tables[0].Rows.Count > 0)
                     {
                         return res1;
                     }
                 }
                
            }
            catch (Exception ex)
            {
                return res1; ;
            }
            return res1;

        }
        #endregion GetPLI

        #region SavePLIMaster
        public DataSet SavePLIMaster(string Operation,string AgentCode, string OriginType, string Origin, string DestinationType, string Destination, string FlightNo, string Commodity, bool isStandard ,string Rate,double Tonnage, string Sale, double Percent ,string Applicability, string ApplicableRate, string SpotRateTonnageIsInclude, DateTime ApplicableFrom, DateTime ApplicableTo, string UpdatedBy, DateTime UpdatedOn, string PLIId)
        {
            DataSet res = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] Pname = new string[21];
                object[] Pvalue = new object[21];
                SqlDbType[] Ptype = new SqlDbType[21];

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.Bit;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.Float;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.Float;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.DateTime;
                Ptype[17] = SqlDbType.DateTime;
                Ptype[18] = SqlDbType.VarChar;
                Ptype[19] = SqlDbType.DateTime;
                Ptype[20] = SqlDbType.VarChar;

                Pname[0] = "Operation";
                Pname[1] = "AgentCode";
                Pname[2] = "OriginType";
                Pname[3] = "Origin";
                Pname[4] = "DestinationType";
                Pname[5] = "Destination";
                Pname[6] = "FlightNo";
                Pname[7] = "Commodity";
                Pname[8] = "isStandard";
                Pname[9] = "Rate";
                Pname[10] = "Tonnage";
                Pname[11] = "Sale";
                Pname[12] = "Percent";
                Pname[13] = "Applicability";
                Pname[14] = "ApplicableRate";
                Pname[15] = "SpotRateTonnageIsInclude";
                Pname[16] = "ApplicableFrom";
                Pname[17] = "ApplicableTo";
                Pname[18] = "UpdatedBy";
                Pname[19] = "UpdatedOn";
                Pname[20] = "PLIId";

                Pvalue[0] = Operation;
                Pvalue[1] = AgentCode;
                Pvalue[2] = OriginType;
                Pvalue[3] = Origin;
                Pvalue[4] = DestinationType;
                Pvalue[5] = Destination;
                Pvalue[6] = FlightNo;
                Pvalue[7] = Commodity;
                Pvalue[8] = isStandard;
                Pvalue[9] = Rate;
                Pvalue[10] = Tonnage;
                Pvalue[11] = Sale;
                Pvalue[12] = Percent;
                Pvalue[13] = Applicability;
                Pvalue[14] = ApplicableRate;
                Pvalue[15] = SpotRateTonnageIsInclude;
                Pvalue[16] = ApplicableFrom;
                Pvalue[17] = ApplicableTo;
                Pvalue[18] = UpdatedBy;
                Pvalue[19] = UpdatedOn;
                Pvalue[20] = PLIId;

                res = da.SelectRecords("SpSavePLIMaster", Pname, Pvalue, Ptype);

                return res;
            }
            catch (Exception ex)
            {
                return res;
            }
        }
        #endregion SaveAgentDeal

        #region SavePLIExceptions
        public bool SavePLIExceptions(string PLIId, string FlightNumber, bool FNInc, string FlightCarrier, bool FCInc, string HandlingCode, bool HCInc, string AirlineCode, bool ACInc, string IATACommCode, bool CCInc, string AgentCode, bool ADInc, string ShipperCode, bool SCInc, string ParamSource, bool SourceInc, string ParamDest, bool DestInc)
        {
            bool res = false;
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] Pname = new string[19];
                object[] Pvalue = new object[19];
                SqlDbType[] Ptype = new SqlDbType[19];

                Ptype[0] = SqlDbType.VarChar;

                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.Bit;

                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.Bit;

                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.Bit;

                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.Bit;

                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.Bit;

                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.Bit;

                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.Bit;

                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.Bit;

                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.Bit;

                Pname[0] = "PLIId";
                Pname[1] = "FlightNumber";
                Pname[2] = "FNInc";
                Pname[3] = "FlightCarrier";
                Pname[4] = "FCInc";
                Pname[5] = "HandlingCode";
                Pname[6] = "HCInc";
                Pname[7] = "AirlineCode";
                Pname[8] = "ACInc";
                Pname[9] = "IATACommCode";
                Pname[10] = "CCInc";
                Pname[11] = "AgentCode";
                Pname[12] = "ADInc";
                Pname[13] = "ShipperCode";
                Pname[14] = "SCInc";
                Pname[15] = "ParamSource";
                Pname[16] = "SourceInc";
                Pname[17] = "ParamDest";
                Pname[18] = "DestInc";

                Pvalue[0] = PLIId;
                Pvalue[1] = FlightNumber;
                Pvalue[2] = FNInc;
                Pvalue[3] = FlightCarrier;
                Pvalue[4] = FCInc;
                Pvalue[5] = HandlingCode;
                Pvalue[6] = HCInc;
                Pvalue[7] = AirlineCode;
                Pvalue[8] = ACInc;
                Pvalue[9] = IATACommCode;
                Pvalue[10] = CCInc;
                Pvalue[11] = AgentCode;
                Pvalue[12] = ADInc;
                Pvalue[13] = ShipperCode;
                Pvalue[14] = SCInc;
                Pvalue[15] = ParamSource;
                Pvalue[16] = SourceInc;
                Pvalue[17] = ParamDest;
                Pvalue[18] = DestInc;

                res = da.ExecuteProcedure("sp_InsertPLIExceptions", Pname, Ptype, Pvalue);

                return res;
            }
            catch (Exception ex)
            {
                return res;
            }
        }
        #endregion 

        #region List PLI Data
        public DataSet ListPLIData(string PLIId)
        {
            DataSet res = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] Pname = new string[1];
                object[] Pvalue = new object[1];
                SqlDbType[] Ptype = new SqlDbType[1];

                Ptype[0] = SqlDbType.VarChar;
                Pname[0] = "PLIId";
                Pvalue[0] = PLIId;
                res = da.SelectRecords("SpListPLIMaster", Pname, Pvalue, Ptype);
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion
    }
}
