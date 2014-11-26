using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration; 
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BALULDMaster
    {
        #region Variables

        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

        #endregion Variables

        #region Save Data with Max Gross Wt
        public DataSet SelectRecords(string ULDNo, string Owner, float PurchaseCost, string CostCurr, string EcoRprPt, 
            float Length, float Width, float Height, string TareWgt, string ULDStatus, string Certification, 
            string Remark, bool isForeign, string ULDTypeID, string ULDSerialNo, string LocSource, string UpdatedBy, 
            DateTime UpdatedOn, string LocType, float LocID, string DimWgVal, string TareWgVal, DateTime LocatedOn, 
            string ULDManu, string CargoIndicator, string Carrier, bool IsThirdParty, string ULDUseStatus, 
            string isReceived, string DollyWt, decimal MaxGrossWt)
        {
            SQLServer db = new SQLServer(constr);
            string[] paramname = new string[31];
            object[] paramvalue = new object[31];
            SqlDbType[] paramtype = new SqlDbType[31];

            try
            {
                paramname[0] = "ULDNo";
                paramname[1] = "Owner";
                paramname[2] = "PurchaseCost";
                paramname[3] = "CostCurr";
                paramname[4] = "EcoRprPt";
                paramname[5] = "Length";
                paramname[6] = "Width";
                paramname[7] = "Height";
                paramname[8] = "TareWgt";
                paramname[9] = "ULDStatus";
                paramname[10] = "Certification";
                paramname[11] = "Remark";
                paramname[12] = "isForeign";
                paramname[13] = "ULDTypeID";
                paramname[14] = "ULDSerialNo";
                paramname[15] = "LocSource";
                paramname[16] = "UpdatedBy";
                paramname[17] = "UpdatedOn";
                paramname[18] = "LocType";
                paramname[19] = "LocID";
                paramname[20] = "DimWgVal";
                paramname[21] = "TareWgVal";
                paramname[22] = "LocatedOn";
                paramname[23] = "ULDManu";
                paramname[24] = "CargoIndicator";
                paramname[25] = "Carrier";
                paramname[26] = "IsThirdParty";
                paramname[27] = "ULDUseStatus";
                paramname[28] = "IsReceived";
                paramname[29] = "DollyWt";
                paramname[30] = "MaxGrossWt";

                if (ULDStatus == "")
                    ULDStatus = "3";
                string LocaID = LocSource;
                //if(ULDTypeID Need to call function based on ULD Number.
                if (ULDTypeID == "")
                {
                    ULDTypeID = ULDNo.Trim().Substring(0, 3);
                    GetULDTypeID(ref ULDTypeID, ref LocaID);

                    if (LocaID != "")
                        LocID = float.Parse(LocaID);
                    else
                        LocID = 0;

                    LocSource = "Manual";
                }

                //if (LocType == "")
                LocType = "W";

                if (CostCurr == "0" || CostCurr == "")
                {
                    CostCurr = "USD";
                }

                if (DimWgVal == "0" || DimWgVal == "")
                {
                    DimWgVal = "Cubic cm";
                }

                if (TareWgVal == "0" || TareWgVal == "")
                {
                    TareWgVal = "Kg";
                }
                if (ULDUseStatus == "")
                {
                    ULDUseStatus = "2";
                }


                paramvalue[0] = ULDNo;
                paramvalue[1] = Owner;
                paramvalue[2] = PurchaseCost;
                paramvalue[3] = CostCurr;
                paramvalue[4] = EcoRprPt;
                paramvalue[5] = Length;
                paramvalue[6] = Width;
                paramvalue[7] = Height;
                paramvalue[8] = TareWgt;
                paramvalue[9] = ULDStatus;
                paramvalue[10] = Certification;
                paramvalue[11] = Remark;
                paramvalue[12] = "false";
                paramvalue[13] = ULDTypeID;
                paramvalue[14] = ULDSerialNo;
                paramvalue[15] = LocSource;
                paramvalue[16] = UpdatedBy;
                paramvalue[17] = UpdatedOn;
                paramvalue[18] = LocType;
                paramvalue[19] = LocID;
                paramvalue[20] = DimWgVal;
                paramvalue[21] = TareWgVal;
                paramvalue[22] = LocatedOn;
                paramvalue[23] = ULDManu;
                paramvalue[24] = CargoIndicator;
                paramvalue[25] = Carrier;
                paramvalue[26] = IsThirdParty;
                paramvalue[27] = ULDUseStatus;
                paramvalue[28] = isReceived;
                paramvalue[29] = DollyWt;
                paramvalue[30] = MaxGrossWt;

                paramtype[0] = SqlDbType.NVarChar;
                paramtype[1] = SqlDbType.NVarChar;
                paramtype[2] = SqlDbType.Real;
                paramtype[3] = SqlDbType.NVarChar;
                paramtype[4] = SqlDbType.NVarChar;
                paramtype[5] = SqlDbType.Real;
                paramtype[6] = SqlDbType.Real;
                paramtype[7] = SqlDbType.Real;
                paramtype[8] = SqlDbType.NVarChar;
                paramtype[9] = SqlDbType.NVarChar;
                paramtype[10] = SqlDbType.NVarChar;
                paramtype[11] = SqlDbType.NVarChar;
                paramtype[12] = SqlDbType.Bit;
                paramtype[13] = SqlDbType.NVarChar;
                paramtype[14] = SqlDbType.NVarChar;
                paramtype[15] = SqlDbType.NVarChar;
                paramtype[16] = SqlDbType.NVarChar;
                paramtype[17] = SqlDbType.DateTime;
                paramtype[18] = SqlDbType.NVarChar;
                paramtype[19] = SqlDbType.BigInt;
                paramtype[20] = SqlDbType.NVarChar;
                paramtype[21] = SqlDbType.NVarChar;
                paramtype[22] = SqlDbType.DateTime;
                paramtype[23] = SqlDbType.NVarChar;
                paramtype[24] = SqlDbType.NVarChar;
                paramtype[25] = SqlDbType.NVarChar;
                paramtype[26] = SqlDbType.Bit;
                paramtype[27] = SqlDbType.NVarChar;
                paramtype[28] = SqlDbType.NVarChar;
                paramtype[29] = SqlDbType.VarChar;
                paramtype[30] = SqlDbType.Decimal;

                DataSet ds = new DataSet();
                ds = db.SelectRecords("spInsertULDMaster", paramname, paramvalue, paramtype);
                return ds;

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                db = null;
                paramname = null;
                paramvalue = null;
                paramtype = null;
            }
        }

        #endregion #region Save Data

        #region Save Data Old Function kept for Backward Compatibility
        public DataSet SelectRecords(string ULDNo, string Owner, float PurchaseCost, string CostCurr, string EcoRprPt,
            float Length, float Width, float Height, string TareWgt, string ULDStatus, string Certification,
            string Remark, bool isForeign, string ULDTypeID, string ULDSerialNo, string LocSource, string UpdatedBy,
            DateTime UpdatedOn, string LocType, float LocID, string DimWgVal, string TareWgVal, DateTime LocatedOn,
            string ULDManu, string CargoIndicator, string Carrier, bool IsThirdParty, string ULDUseStatus,
            string isReceived, string DollyWt)
        {
            SQLServer db = new SQLServer(constr);
            string[] paramname = new string[31];
            object[] paramvalue = new object[31];
            SqlDbType[] paramtype = new SqlDbType[31];

            try
            {
                paramname[0] = "ULDNo";
                paramname[1] = "Owner";
                paramname[2] = "PurchaseCost";
                paramname[3] = "CostCurr";
                paramname[4] = "EcoRprPt";
                paramname[5] = "Length";
                paramname[6] = "Width";
                paramname[7] = "Height";
                paramname[8] = "TareWgt";
                paramname[9] = "ULDStatus";
                paramname[10] = "Certification";
                paramname[11] = "Remark";
                paramname[12] = "isForeign";
                paramname[13] = "ULDTypeID";
                paramname[14] = "ULDSerialNo";
                paramname[15] = "LocSource";
                paramname[16] = "UpdatedBy";
                paramname[17] = "UpdatedOn";
                paramname[18] = "LocType";
                paramname[19] = "LocID";
                paramname[20] = "DimWgVal";
                paramname[21] = "TareWgVal";
                paramname[22] = "LocatedOn";
                paramname[23] = "ULDManu";
                paramname[24] = "CargoIndicator";
                paramname[25] = "Carrier";
                paramname[26] = "IsThirdParty";
                paramname[27] = "ULDUseStatus";
                paramname[28] = "IsReceived";
                paramname[29] = "DollyWt";
                paramname[30] = "MaxGrossWt";

                if (ULDStatus == "")
                    ULDStatus = "3";
                string LocaID = LocSource;
                //if(ULDTypeID Need to call function based on ULD Number.
                if (ULDTypeID == "")
                {
                    ULDTypeID = ULDNo.Trim().Substring(0, 3);
                    GetULDTypeID(ref ULDTypeID, ref LocaID);

                    if (LocaID != "")
                        LocID = float.Parse(LocaID);
                    else
                        LocID = 0;

                    LocSource = "Manual";
                }

                //if (LocType == "")
                LocType = "W";

                if (CostCurr == "0" || CostCurr == "")
                {
                    CostCurr = "USD";
                }

                if (DimWgVal == "0" || DimWgVal == "")
                {
                    DimWgVal = "Cubic cm";
                }

                if (TareWgVal == "0" || TareWgVal == "")
                {
                    TareWgVal = "Kg";
                }
                if (ULDUseStatus == "")
                {
                    ULDUseStatus = "2";
                }


                paramvalue[0] = ULDNo;
                paramvalue[1] = Owner;
                paramvalue[2] = PurchaseCost;
                paramvalue[3] = CostCurr;
                paramvalue[4] = EcoRprPt;
                paramvalue[5] = Length;
                paramvalue[6] = Width;
                paramvalue[7] = Height;
                paramvalue[8] = TareWgt;
                paramvalue[9] = ULDStatus;
                paramvalue[10] = Certification;
                paramvalue[11] = Remark;
                paramvalue[12] = "false";
                paramvalue[13] = ULDTypeID;
                paramvalue[14] = ULDSerialNo;
                paramvalue[15] = LocSource;
                paramvalue[16] = UpdatedBy;
                paramvalue[17] = UpdatedOn;
                paramvalue[18] = LocType;
                paramvalue[19] = LocID;
                paramvalue[20] = DimWgVal;
                paramvalue[21] = TareWgVal;
                paramvalue[22] = LocatedOn;
                paramvalue[23] = ULDManu;
                paramvalue[24] = CargoIndicator;
                paramvalue[25] = Carrier;
                paramvalue[26] = IsThirdParty;
                paramvalue[27] = ULDUseStatus;
                paramvalue[28] = isReceived;
                paramvalue[29] = DollyWt;
                paramvalue[30] = 0;

                paramtype[0] = SqlDbType.NVarChar;
                paramtype[1] = SqlDbType.NVarChar;
                paramtype[2] = SqlDbType.Real;
                paramtype[3] = SqlDbType.NVarChar;
                paramtype[4] = SqlDbType.NVarChar;
                paramtype[5] = SqlDbType.Real;
                paramtype[6] = SqlDbType.Real;
                paramtype[7] = SqlDbType.Real;
                paramtype[8] = SqlDbType.NVarChar;
                paramtype[9] = SqlDbType.NVarChar;
                paramtype[10] = SqlDbType.NVarChar;
                paramtype[11] = SqlDbType.NVarChar;
                paramtype[12] = SqlDbType.Bit;
                paramtype[13] = SqlDbType.NVarChar;
                paramtype[14] = SqlDbType.NVarChar;
                paramtype[15] = SqlDbType.NVarChar;
                paramtype[16] = SqlDbType.NVarChar;
                paramtype[17] = SqlDbType.DateTime;
                paramtype[18] = SqlDbType.NVarChar;
                paramtype[19] = SqlDbType.BigInt;
                paramtype[20] = SqlDbType.NVarChar;
                paramtype[21] = SqlDbType.NVarChar;
                paramtype[22] = SqlDbType.DateTime;
                paramtype[23] = SqlDbType.NVarChar;
                paramtype[24] = SqlDbType.NVarChar;
                paramtype[25] = SqlDbType.NVarChar;
                paramtype[26] = SqlDbType.Bit;
                paramtype[27] = SqlDbType.NVarChar;
                paramtype[28] = SqlDbType.NVarChar;
                paramtype[29] = SqlDbType.VarChar;
                paramtype[30] = SqlDbType.Decimal;

                DataSet ds = new DataSet();
                ds = db.SelectRecords("spInsertULDMaster", paramname, paramvalue, paramtype);
                return ds;

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                db = null;
                paramname = null;
                paramvalue = null;
                paramtype = null;
            }
        }

        #endregion #region Save Data

        # region Search
        public DataSet Search(string ULDNo)
        {
            SQLServer db = new SQLServer(constr);
            string[] paramname = new string[1];
            object[] paramvalue = new object[1];
            SqlDbType[] paramtype = new SqlDbType[1];
            DataSet ds = null;

            try
            {
                paramname[0] = "ULDNo";

                //paramvalue[0] = ddlULDType.SelectedItem.Text.ToString() + txtULDNo.Text.Trim() + ConfigurationManager.AppSettings["CompName"].ToString();
                paramvalue[0] = ULDNo;

                paramtype[0] = SqlDbType.NVarChar;


                ds = db.SelectRecords("spSearchULDNo", paramname, paramvalue, paramtype);
                return ds;

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                db = null;
                paramname = null;
                paramvalue = null;
                paramtype = null;
            }
        }
        # endregion Search

        #region List FillULDType
        public DataSet FillULDType(string tblName, string defaultValue)
        {
            SQLServer db = new SQLServer(constr);
            string[] pname = new string[2];
            object[] pvalue = new object[2];
            SqlDbType[] ptype = new SqlDbType[2];

            try
            {
                pname[0] = "tblName";
                pname[1] = "defaultValue";

                pvalue[0] = "tblULDTypeMaster";
                pvalue[1] = "ALL";

                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                DataSet ds = new DataSet();
                ds = db.SelectRecords("spFillComboBoxMasters", pname, pvalue, ptype);
                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                db = null;
                pname = null;
                pvalue = null;
                ptype = null;
            }

        }

        #endregion List FillULDType

        #region List - Fill ULD Owner i.e. Airline Code

        public DataSet fillULDOwner(string tblName, string defaultValue)
        {
            SQLServer db = new SQLServer(constr);
            string[] pname = new string[2];
            object[] pvalue = new object[2];
            SqlDbType[] ptype = new SqlDbType[2];

            try
            {
                pname[0] = "tblName";
                pname[1] = "defaultValue";


                pvalue[0] = "tblAirlineMaster";
                pvalue[1] = "ALL";

                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                DataSet ds = new DataSet();
                ds = db.SelectRecords("spFillComboBoxMasters", pname, pvalue, ptype);
                return ds;
            }

            catch (Exception e)
            {
                return null;
            }
            finally
            {
                db = null;
                pname = null;
                pvalue = null;
                ptype = null;
            }
        }

        #endregion List - Fill ULD Owner i.e. Airline Code

        #region Search List ULD
        public DataSet SearchList(object[] QueryValues)
        {
            SQLServer db = new SQLServer(constr);
            string[] pname = new string[5];
            SqlDbType[] ptype = new SqlDbType[5];
            DataSet ds = new DataSet();

            try
            {
                pname[0] = "ULDType";
                pname[1] = "ULDOwner";
                pname[2] = "Origin";
                //pname[3] = "Destination";
                //pname[4] = "FromDate";
                //pname[5] = "ToDate";
                //pname[4] = "MovementType";
                pname[3] = "ULDNumber";
                pname[4] = "UseStatus";
                //pname[6] = "FlightNumber";

                ptype[0] = SqlDbType.NVarChar;
                ptype[1] = SqlDbType.NVarChar;
                ptype[2] = SqlDbType.NVarChar;
                ptype[3] = SqlDbType.NVarChar;
                ptype[4] = SqlDbType.VarChar;
                //ptype[4] = SqlDbType.NVarChar;
                //ptype[5] = SqlDbType.NVarChar;
                //ptype[4] = SqlDbType.NVarChar;
                //ptype[5] = SqlDbType.NVarChar;
                //ptype[6] = SqlDbType.VarChar;

                ds = db.SelectRecords("spListULDs", pname, QueryValues, ptype);
                return ds;

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                db = null;
                pname = null;
                ptype = null;
                QueryValues = null;
            }
        }

        #endregion Search List ULD

        #region Audit Trail Search

        public DataSet GetAuditDetails(string ULDNo, string fromDate, string toDate)
        {
                SQLServer db = new SQLServer(constr);
                string[] pname = new string[3];
                object[] pvalue = new object[3];
                SqlDbType[] ptype = new SqlDbType[3];
                DataSet ds = new DataSet();
            try
            {
                
                pname[0] = "ULDNo";
                pname[1] = "fromDate";
                pname[2] = "toDate";

                
                pvalue[0] = ULDNo;
                pvalue[1] = fromDate;
                pvalue[2] = toDate;

                
                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.DateTime;
                ptype[2] = SqlDbType.DateTime;

                ds = db.SelectRecords("spGetAuditDetails", pname, pvalue, ptype);
                return ds;

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                db = null;
                pname = null;
                ptype = null;
                pvalue = null;
            }
        }

        #endregion Audit Trail Search

        #region Get ULDDimensions
        public DataSet GetULDDimensions(string Typecode)
        {
            SQLServer db = new SQLServer(constr);
            string[] paramname = new string[1];
            object[] paramvalue = new object[1];
            SqlDbType[] paramtype = new SqlDbType[1];
            
            try
            {
                
                paramname[0] = "Typecode";
                
                paramvalue[0] = Typecode;
                
                paramtype[0] = SqlDbType.NVarChar;
                
                DataSet ds = new DataSet();
                ds = db.SelectRecords("SpGetDimensions", paramname, paramvalue, paramtype);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                db = null;
                paramname = null;
                paramvalue = null;
                paramtype = null;
            }
        }
        #endregion

        #region Save ULD Movement Details 
        public bool SaveMovementDetails(string ULDNumber, Int16 Status, Int64 LocationID, string LocationType, string MovedOn, string UpdatedBy, string flightid, string Origin, string Destination, string StatusCode, string MovType)
        {
            DataSet dsUpdateMovement = null;
            string[] QueryNames = new string[11];
            object[] QueryValues = new object[11];
            SqlDbType[] QueryTypes = new SqlDbType[11];
            SQLServer db = new SQLServer(constr);
            try
            {

                QueryNames[0] = "ULDNo";
                QueryNames[1] = "Status";
                QueryNames[2] = "LocationID";
                QueryNames[3] = "LocationType";
                QueryNames[4] = "MovedOn";
                QueryNames[5] = "UpdatedBy";
                QueryNames[6] = "FlightID";
                QueryNames[7] = "Origin";
                QueryNames[8] = "Destination";
                QueryNames[9] = "StatusCode";
                QueryNames[10] = "MovType";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.TinyInt;
                QueryTypes[2] = SqlDbType.BigInt;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.DateTime;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.VarChar;
                QueryTypes[7] = SqlDbType.VarChar;
                QueryTypes[8] = SqlDbType.VarChar;
                QueryTypes[9] = SqlDbType.VarChar;
                QueryTypes[10] = SqlDbType.VarChar;

                QueryValues[0] = ULDNumber;
                QueryValues[1] = Status;
                QueryValues[2] = LocationID;
                QueryValues[3] = LocationType;
                QueryValues[4] = Convert.ToDateTime(MovedOn);
                QueryValues[5] = UpdatedBy;
                QueryValues[6] = flightid;
                QueryValues[7] = Origin;
                QueryValues[8] = Destination;
                QueryValues[9] = StatusCode;
                QueryValues[10] = MovType;

                dsUpdateMovement = db.SelectRecords("spInsertULDMoveData", QueryNames, QueryValues, QueryTypes);
                if (dsUpdateMovement != null && dsUpdateMovement.Tables.Count > 0)
                {
                    if (dsUpdateMovement.Tables[0].Rows.Count > 0)
                    {
                        if (dsUpdateMovement.Tables[0].Rows[0][0].ToString() == "SUCCESS")
                        {
                            return true;
                        }
                        else if (dsUpdateMovement.Tables[0].Rows[0][0].ToString() == "ABSENT")
                        {
                            //lblStatus.Text = "Unknown ULD..!!";
                            return false;
                        }
                        else if (dsUpdateMovement.Tables[0].Rows[0][0].ToString() == "FLIGHT_ABSENT")
                        {
                            //lblStatus.Text = "Unknown FlightNo..!!";
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                //lblStatus.Text = "Error in Saving ULDMovement";
                return false;
            }
            finally
            {
                if (dsUpdateMovement != null)
                {
                    dsUpdateMovement.Dispose();
                }
                QueryNames = null;
                QueryValues = null;
                QueryTypes = null;
            }
        }
        #endregion

        #region Get ULD Type ID
        private int GetULDTypeID(ref string ULDPrefix, ref string LocationSource)
        {
            SQLServer db = new SQLServer(constr);

            DataSet ds = new DataSet();

            try
            {
                string strQuery = "SELECT ID FROM tblULDTypeMaster WHERE isActive = 1 AND TypeCode = '" + ULDPrefix + "'; ";

                strQuery = strQuery + "SELECT SerialNumber FROM dbo.AirportMaster WHERE AirportCode = '" + LocationSource + "'; ";

                ds = db.GetDataset(strQuery);

                if (ds != null && ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)
                {
                    ULDPrefix = ds.Tables[0].Rows[0][0].ToString();
                    //LocationSource = ds.Tables[1].Rows[0][0].ToString();
                }

                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    //ULDPrefix = ds.Tables[0].Rows[0][0].ToString();
                    LocationSource = ds.Tables[1].Rows[0][0].ToString();
                }

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                db = null;
                ds = null;
            }
        }
        #endregion

        #region UPDATE Movement Type Of Latest ULD
        public bool UpdateLatestULDMovementHistory(string ULDNumber, string UpdatedBy, string MoveType, string Origin, string Destination)
        { 
              DataSet dsUpdateMovement = null;
            string[] QueryNames = new string[5];
            object[] QueryValues = new object[5];
            SqlDbType[] QueryTypes = new SqlDbType[5];
            SQLServer db = new SQLServer(constr);
            try
            {

                QueryNames[0] = "ULDNumber";
                QueryNames[1] = "UpdatedBy";
                QueryNames[2] = "MoveType";
                QueryNames[3] = "Origin";
                QueryNames[4] = "Destination";                

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
               

                QueryValues[0] = ULDNumber;
                QueryValues[1] = UpdatedBy;
                QueryValues[2] = MoveType;
                QueryValues[3] = Origin;
                QueryValues[4] = Destination;


                dsUpdateMovement = db.SelectRecords("UpdateULDMovementLatestHistoryData", QueryNames, QueryValues, QueryTypes);
                if (dsUpdateMovement != null && dsUpdateMovement.Tables.Count > 0)
                {
                    if (dsUpdateMovement.Tables[0].Rows.Count > 0)
                    {
                        if (dsUpdateMovement.Tables[0].Rows[0][0].ToString() == "SUCCESS")
                        {
                            return true;
                        }
                        else 
                        {
                            //lblStatus.Text = "Unknown ULD..!!";
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                //lblStatus.Text = "Error in Saving ULDMovement";
                return false;
            }
            finally
            {
                if (dsUpdateMovement != null)
                {
                    dsUpdateMovement.Dispose();
                }
                QueryNames = null;
                QueryValues = null;
                QueryTypes = null;
            }
        }
        #endregion

        #region Delete ULD Master
        public bool DeleteULDMaster(string ULDNumber)
        {
            SQLServer db = new SQLServer(constr);
            string[] paramname = new string[1];
            object[] paramvalue = new object[1];
            SqlDbType[] paramtype = new SqlDbType[1];

            try
            {

                paramname[0] = "ULDno";

                paramvalue[0] = ULDNumber;

                paramtype[0] = SqlDbType.VarChar;

                db.UpdateData("spDelULDDetails", paramname, paramtype, paramvalue);

                return (true);
            }
            catch (Exception)
            {
                
            }
            finally
            {
                db = null;
                paramname = null;
                paramvalue = null;
                paramtype = null;
            }
            return false;
        }
        #endregion

    }
    
}
