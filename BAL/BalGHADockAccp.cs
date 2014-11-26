using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class BalGHADockAccp
    {
       
        SQLServer db = new SQLServer(Global.GetConnectionString());

        #region Retrieving Records as per selection
        public DataSet GetDockAccpList(object[] ParamValues)
        {
            string[] QueryNames = new string[7];
            SqlDbType[] QueryTypes = new SqlDbType[7];
            try
            {
                QueryNames[0] = "TokenDt";
                QueryNames[1] = "FltNo";
                QueryNames[2] = "KnwShipper";
                QueryNames[3] = "IAC";
                QueryNames[4] = "ccsf";
                QueryNames[5] = "dockNo";
                QueryNames[6] = "tknNo";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.VarChar;

                DataSet ds = db.SelectRecords("sp_GetDockAcceptanceList", QueryNames, ParamValues, QueryTypes);
                return ds;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                QueryNames = null;
                QueryTypes = null;
                ParamValues = null;
            }
        }
        #endregion Retrieving Records as per selection

        #region Retrieving AWB Records as per Token No
        public DataSet GetAWBList(object[] ParamValues)
        {
            string[] QueryNames = new string[8];
            SqlDbType[] QueryTypes = new SqlDbType[8];

            try
            {
                QueryNames[0] = "tokenNo";
                QueryTypes[0] = SqlDbType.VarChar;

                QueryNames[1] = "tokenDt";
                QueryTypes[1] = SqlDbType.VarChar;

                QueryNames[2] = "AWBNumber";
                QueryTypes[2] = SqlDbType.VarChar;

                QueryNames[3] = "FlightNumber";
                QueryTypes[3] = SqlDbType.VarChar;

                QueryNames[4] = "FlightDate";
                QueryTypes[4] = SqlDbType.VarChar;

                QueryNames[5] = "Origin";
                QueryTypes[5] = SqlDbType.VarChar;

                QueryNames[6] = "DockNo";
                QueryTypes[6] = SqlDbType.VarChar;


                QueryNames[7] = "Status";
                QueryTypes[7] = SqlDbType.VarChar;

                DataSet ds = db.SelectRecords("sp_GetAWBListFromToken", QueryNames, ParamValues, QueryTypes);
                return ds;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                QueryNames = null;
                QueryTypes = null;
                ParamValues = null;
            }
        }
        #endregion Retrieving AWB Records as per Token No

        #region Save Acceptance Data
        public bool SaveAccepanceData(string AWBNumber,string PieceId,string MeasureUnit,
            float Length,float Breadth,float Height,decimal Volume,decimal Weight,decimal ScaleWeight,
            string ULDNo,string PieceType,string BagNo,string Location,bool isTamper,bool isPackaging,bool isVisual,
            bool isSmell,bool isDGR,bool isLiveAnimal,DateTime UpdatedOn,string UpdatedBy,int PcsCount,
            string DockNo,string SHCCode,string CommodityCode,Int64 SerialNo,bool isAcceptance,string FlightNo,string FlightDate)
        {
            string[] QueryNames = new string[29];
            SqlDbType[] QueryTypes = new SqlDbType[29];
            object[] QueryValues = new object[29];

            try
            {
                QueryNames[0] = "AWBNumber";
                QueryTypes[0] = SqlDbType.VarChar;
                QueryValues[0] = AWBNumber;

                QueryNames[1] = "PieceId";
                QueryTypes[1] = SqlDbType.VarChar;
                QueryValues[1] = PieceId;

                QueryNames[2] = "MeasureUnit";
                QueryTypes[2] = SqlDbType.VarChar;
                QueryValues[2] = MeasureUnit;

                QueryNames[3] = "Length";
                QueryTypes[3] = SqlDbType.Int;
                QueryValues[3] = Length;

                QueryNames[4] = "Breadth";
                QueryTypes[4] = SqlDbType.Int;
                QueryValues[4] = Breadth;

                QueryNames[5] = "Height";
                QueryTypes[5] = SqlDbType.Int;
                QueryValues[5] = Height;

                QueryNames[6] = "Volume";
                QueryTypes[6] = SqlDbType.Decimal;
                QueryValues[6] = Volume;

                QueryNames[7] = "Weight";
                QueryTypes[7] = SqlDbType.Decimal;
                QueryValues[7] = Weight;

                QueryNames[8] = "ScaleWeight";
                QueryTypes[8] = SqlDbType.Decimal;
                QueryValues[8] = ScaleWeight;

                QueryNames[9] = "ULDNo";
                QueryTypes[9] = SqlDbType.VarChar;
                QueryValues[9] = ULDNo;

                QueryNames[10] = "PieceType";
                QueryTypes[10] = SqlDbType.VarChar;
                QueryValues[10] = PieceType;

                QueryNames[11] = "BagNo";
                QueryTypes[11] = SqlDbType.VarChar;
                QueryValues[11] = BagNo;

                QueryNames[12] = "Location";
                QueryTypes[12] = SqlDbType.VarChar;
                QueryValues[12] = Location;

                QueryNames[13] = "isTamper";
                QueryTypes[13] = SqlDbType.Bit;
                QueryValues[13] = isTamper;

                QueryNames[14] = "isPackaging";
                QueryTypes[14] = SqlDbType.Bit;
                QueryValues[14] = isPackaging;

                QueryNames[15] = "isVisual";
                QueryTypes[15] = SqlDbType.Bit;
                QueryValues[15] = isVisual;

                QueryNames[16] = "isSmell";
                QueryTypes[16] = SqlDbType.Bit;
                QueryValues[16] = isSmell;

                QueryNames[17] = "isDGR";
                QueryTypes[17] = SqlDbType.Bit;
                QueryValues[17] = isDGR;

                QueryNames[18] = "isLiveAnimal";
                QueryTypes[18] = SqlDbType.Bit;
                QueryValues[18] = isLiveAnimal;

                QueryNames[19] = "UpdatedOn";
                QueryTypes[19] = SqlDbType.DateTime;
                QueryValues[19] = UpdatedOn;

                QueryNames[20] = "UpdatedBy";
                QueryTypes[20] = SqlDbType.VarChar;
                QueryValues[20] = UpdatedBy;

                QueryNames[21] = "pcCount";
                QueryTypes[21] = SqlDbType.VarChar;
                QueryValues[21] = PcsCount;

                QueryNames[22] = "DockNo";
                QueryTypes[22] = SqlDbType.VarChar;
                QueryValues[22] = DockNo;

                QueryNames[23] = "SHCCode";
                QueryTypes[23] = SqlDbType.VarChar;
                QueryValues[23] = SHCCode;

                QueryNames[24] = "CommCode";
                QueryTypes[24] = SqlDbType.VarChar;
                QueryValues[24] = CommodityCode;

                QueryNames[25] = "srno";
                QueryTypes[25] = SqlDbType.BigInt;
                QueryValues[25] = SerialNo;

                QueryNames[26] = "isAccp";
                QueryTypes[26] = SqlDbType.Bit;
                QueryValues[26] = isAcceptance;

                QueryNames[27] = "FlightNo";
                QueryTypes[27] = SqlDbType.VarChar;
                QueryValues[27] = FlightNo;

                QueryNames[28] = "FlightDate";
                QueryTypes[28] = SqlDbType.VarChar;
                QueryValues[28] = FlightDate;

                bool res = db.InsertData("sp_SaveAccepanceData", QueryNames, QueryTypes, QueryValues);
                return res;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                QueryNames = null;
                QueryTypes = null;
                //ParamValues = null;
            }
        }
        #endregion Save Acceptance Data

        #region Save Acceptance Data
        public bool SaveAccepanceData(object[] ParamValues)
        {
            string[] QueryNames = new string[29];
            SqlDbType[] QueryTypes = new SqlDbType[29];

            try
            {
                QueryNames[0] = "AWBNumber";
                QueryTypes[0] = SqlDbType.VarChar;

                QueryNames[1] = "PieceId";
                QueryTypes[1] = SqlDbType.VarChar;

                QueryNames[2] = "MeasureUnit";
                QueryTypes[2] = SqlDbType.VarChar;

                QueryNames[3] = "Length";
                QueryTypes[3] = SqlDbType.Int;

                QueryNames[4] = "Breadth";
                QueryTypes[4] = SqlDbType.Int;

                QueryNames[5] = "Height";
                QueryTypes[5] = SqlDbType.Int;

                QueryNames[6] = "Volume";
                QueryTypes[6] = SqlDbType.Decimal;

                QueryNames[7] = "Weight";
                QueryTypes[7] = SqlDbType.Decimal;

                QueryNames[8] = "ScaleWeight";
                QueryTypes[8] = SqlDbType.Decimal;

                QueryNames[9] = "ULDNo";
                QueryTypes[9] = SqlDbType.VarChar;

                QueryNames[10] = "PieceType";
                QueryTypes[10] = SqlDbType.VarChar;

                QueryNames[11] = "BagNo";
                QueryTypes[11] = SqlDbType.VarChar;

                QueryNames[12] = "Location";
                QueryTypes[12] = SqlDbType.VarChar;

                QueryNames[13] = "isTamper";
                QueryTypes[13] = SqlDbType.Bit;

                QueryNames[14] = "isPackaging";
                QueryTypes[14] = SqlDbType.Bit;

                QueryNames[15] = "isVisual";
                QueryTypes[15] = SqlDbType.Bit;

                QueryNames[16] = "isSmell";
                QueryTypes[16] = SqlDbType.Bit;

                QueryNames[17] = "isDGR";
                QueryTypes[17] = SqlDbType.Bit;

                QueryNames[18] = "isLiveAnimal";
                QueryTypes[18] = SqlDbType.Bit;

                QueryNames[19] = "UpdatedOn";
                QueryTypes[19] = SqlDbType.DateTime;

                QueryNames[20] = "UpdatedBy";
                QueryTypes[20] = SqlDbType.VarChar;

                QueryNames[21] = "pcCount";
                QueryTypes[21] = SqlDbType.VarChar;

                QueryNames[22] = "DockNo";
                QueryTypes[22] = SqlDbType.VarChar;

                QueryNames[23] = "SHCCode";
                QueryTypes[23] = SqlDbType.VarChar;

                QueryNames[24] = "CommCode";
                QueryTypes[24] = SqlDbType.VarChar;

                QueryNames[25] = "srno";
                QueryTypes[25] = SqlDbType.BigInt;

                QueryNames[26] = "isAccp";
                QueryTypes[26] = SqlDbType.Bit;

                QueryNames[27] = "FlightNo";
                QueryTypes[27] = SqlDbType.VarChar;

                QueryNames[28] = "FlightDate";
                QueryTypes[28] = SqlDbType.VarChar;

                bool res = db.InsertData("sp_SaveAccepanceData", QueryNames, QueryTypes, ParamValues);
                return res;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                QueryNames = null;
                QueryTypes = null;
                ParamValues = null;
            }
        }
        #endregion Save Acceptance Data

        # region Get DropDown Data
        public DataSet GetDropDownData()
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = new DataSet();
            try
            {
                ds = da.SelectRecords("spGetDropDownDataForAcceptance");
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
            finally
            {
                da = null;
            }
            return (null);
        }

        # endregion Get DropDown Data

        #region Save Received PCS And WT By GHA
        public bool SaveGHAAcceptanceData(object[] ParamValues)
        {
            try
            {
                string[] QueryNames = new string[10];
                SqlDbType[] QueryTypes = new SqlDbType[10];

                QueryNames[0] = "AWBNo";
                QueryTypes[0] = SqlDbType.VarChar;

                QueryNames[1] = "AccpPcs";
                QueryTypes[1] = SqlDbType.Int;

                QueryNames[2] = "AccpWt";
                QueryTypes[2] = SqlDbType.Float;

                QueryNames[3] = "UpdatedOn";
                QueryTypes[3] = SqlDbType.DateTime;

                QueryNames[4] = "UpdatedBy";
                QueryTypes[4] = SqlDbType.VarChar;

                QueryNames[5] = "SHC";
                QueryTypes[5] = SqlDbType.VarChar;

                QueryNames[6] = "CommodityCode";
                QueryTypes[6] = SqlDbType.VarChar;

                QueryNames[7] = "FromAcceptance";
                QueryTypes[7] = SqlDbType.VarChar;

                QueryNames[8] = "FlightNo";
                QueryTypes[8] = SqlDbType.VarChar;

                QueryNames[9] = "FlightDate";
                QueryTypes[9] = SqlDbType.VarChar;

                bool res = db.InsertData("sp_SaveGHAAcceptance_V1", QueryNames, QueryTypes, ParamValues);
                return res;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Save Received PCS And WT By GHA
        public bool SaveGHAAcceptanceData_V2(object[] ParamValues)
        {
            string[] QueryNames = new string[17];
            SqlDbType[] QueryTypes = new SqlDbType[17];

            try
            {
                QueryNames[0] = "AWBNo";
                QueryTypes[0] = SqlDbType.VarChar;

                QueryNames[1] = "AccpPcs";
                QueryTypes[1] = SqlDbType.Int;

                QueryNames[2] = "AccpWt";
                QueryTypes[2] = SqlDbType.Float;

                QueryNames[3] = "UpdatedOn";
                QueryTypes[3] = SqlDbType.DateTime;

                QueryNames[4] = "UpdatedBy";
                QueryTypes[4] = SqlDbType.VarChar;

                QueryNames[5] = "SHC";
                QueryTypes[5] = SqlDbType.VarChar;

                QueryNames[6] = "CommodityCode";
                QueryTypes[6] = SqlDbType.VarChar;

                QueryNames[7] = "FromAcceptance";
                QueryTypes[7] = SqlDbType.VarChar;

                QueryNames[8] = "FlightNo";
                QueryTypes[8] = SqlDbType.VarChar;

                QueryNames[9] = "FlightDate";
                QueryTypes[9] = SqlDbType.VarChar;

                QueryNames[10] = "Location";
                QueryTypes[10] = SqlDbType.VarChar;

                QueryNames[11] = "isTamper";
                QueryTypes[11] = SqlDbType.Bit;

                QueryNames[12] = "isPackaging";
                QueryTypes[12] = SqlDbType.Bit;

                QueryNames[13] = "isVisual";
                QueryTypes[13] = SqlDbType.Bit;

                QueryNames[14] = "isSmell";
                QueryTypes[14] = SqlDbType.Bit;

                QueryNames[15] = "isDGR";
                QueryTypes[15] = SqlDbType.Bit;

                QueryNames[16] = "isLiveAnimal";
                QueryTypes[16] = SqlDbType.Bit;

                bool res = db.InsertData("sp_SaveGHAAcceptance_V2", QueryNames, QueryTypes, ParamValues);
                return res;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                QueryNames = null;
                QueryTypes = null;
                ParamValues = null;
            }
        }
        #endregion

        public DataSet GetPartnerDetails(string PartnerCode)
        {
            try
            {
                DataSet dSet = new DataSet();
                SQLServer db = new SQLServer(Global.GetConnectionString());

                dSet = db.SelectRecords("sp_GetPartnerDetailsAcceptance", "Partner", PartnerCode, SqlDbType.VarChar);
                if (dSet != null)
                {
                    if (dSet.Tables.Count > 0)
                    {
                        if (dSet.Tables[0].Rows.Count > 0)
                        {
                            return dSet;
                        }
                        else
                            return null;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            { return null; }
        }
    }
}
