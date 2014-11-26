using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QID.DataAccess;
using System.Data;

/*

 2012-05-04  vinayak
 2012-05-05  vinayak
 2012-06-18  vinayak
 2012-07-09  vinayak Special Commodity
 2012-07-23  vinayak Special Commodity with three charge types
 2012-07-24  vinayak
 2012-07-25  vinayak
 2012-07-30  vinayak
 2012-08-03  vinayak
 
*/
namespace BAL
{
    public class BookingBAL
    {

        #region Variables
        string constr = "";
        public static string OrgStation = "", DestStation = "";
        DataSet dsOtherCharges = null;
        #endregion Variables

        #region Constructor
        public BookingBAL()
        {
           constr = Global.GetConnectionString();
        }
        #endregion Constructor

        #region Get Location List
        /// <summary>
        /// Get information of AWB based on passed values.
        /// </summary>
        /// <returns>Information as list.</returns>
        public DataSet GetAWBInfo(object[] AWBSearch)
        {
             SQLServer da = new SQLServer(constr);
            try
            {
                //Prepare column names and datatypes for search parameters...
                string[] paramNames = new string[4];
                SqlDbType[] dataTypes = new SqlDbType[4];
                int i = 0;

                //0
                paramNames.SetValue("DocumentType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //1
                paramNames.SetValue("AWBPrefix", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //2
                paramNames.SetValue("AWBNumber", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //3
                paramNames.SetValue("AgentCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //Get location names ...
               
                DataSet ds = da.SelectRecords("spGetAWBInfo", paramNames, AWBSearch, dataTypes);
                paramNames = null;
                dataTypes = null;
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
            {}
            finally 
            {
                da = null;
                AWBSearch = null;
            }

            return (null);
        }
        #endregion Get Location List

        #region Validate AWB Number
        /// <summary>
        /// Checks if AWB number is already issued.
        /// </summary>
        /// <returns>True if AWB is not used.</returns>
        public bool ValidateAWB(string AWBNumber,out string HolderName)
        {
            HolderName = "";
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            try
            {
                //Prepare column names and datatypes...
                string paramName = "AWBNumber";
                SqlDbType dataType = SqlDbType.VarChar;

                //Update AWB information.
                ds = da.SelectRecords("spValidateAWB", paramName, AWBNumber, dataType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int res = int.Parse(ds.Tables[0].Rows[0].ToString());
                            if (res > 0)
                            {   //AWB is used.
                                return false;
                            }
                            else
                            {   //AWB is not used.

                                return true;
                            }
                        }
                    }
                }


            }
            catch (Exception)
            {
                ds = null;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
            return (true);
        }
        #endregion Save AWB Summary

        #region Save AWB Summary
        /// <summary>
        /// Saves information of AWB.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SaveAWBSummary(object[] AWBInfo)
        {
            SQLServer da = new SQLServer(constr);
            try
            {
                //Prepare column names and datatypes...
                string[] paramNames = new string[39];
                SqlDbType[] dataTypes = new SqlDbType[39];
                int i = 0;

                //0
                paramNames.SetValue("BookingID", i);
                dataTypes.SetValue(SqlDbType.BigInt, i);
                i++;

                //1
                paramNames.SetValue("DocType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //2
                paramNames.SetValue("AWBPrefix", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //3
                paramNames.SetValue("AWBNumber", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("PiecesCount", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //5
                paramNames.SetValue("GrossWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //6
                paramNames.SetValue("VolumetricWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //7
                paramNames.SetValue("ChargedWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //8
                paramNames.SetValue("OriginCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //9
                paramNames.SetValue("DestinationCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //10
                paramNames.SetValue("AgentCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //11
                paramNames.SetValue("AgentName", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //12
                paramNames.SetValue("ServiceCargoClassId", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //13
                paramNames.SetValue("HandlingInfo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //14
                paramNames.SetValue("ExecutionDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //15
                paramNames.SetValue("ExecutedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //16
                paramNames.SetValue("ExecutedAt", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //17
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //18
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //19
                paramNames.SetValue("IsConsole", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //20
                paramNames.SetValue("IsBonded", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //21
                paramNames.SetValue("IsExport", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //22
                paramNames.SetValue("IsAgreed", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //23
                paramNames.SetValue("Remarks", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //24
                paramNames.SetValue("Interline", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //25
                paramNames.SetValue("SLAC", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //26
                paramNames.SetValue("Customs", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //27
                paramNames.SetValue("EURIN", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //28
                paramNames.SetValue("IrregId", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //29
                paramNames.SetValue("DVCustom", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //30
                paramNames.SetValue("DVCarriage", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //31
                paramNames.SetValue("SHCCodes", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //32
                paramNames.SetValue("ProductId", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //33
                paramNames.SetValue("DlvOnHAWB", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //34
                paramNames.SetValue("DesigCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //35

                paramNames.SetValue("CustomerCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //36

                paramNames.SetValue("Handler", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //37               
                paramNames.SetValue("ShippingAWB", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //38               
                paramNames.SetValue("Documents", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                //Update AWB information.
                bool res = da.UpdateData("spSaveAWBSummary", paramNames, dataTypes, AWBInfo);
                paramNames = null;
                dataTypes = null;
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            finally 
            {
                da = null;
                AWBInfo = null;
            }
            return (-1);
        }
        #endregion Save AWB Summary

        #region Save AWB Summary for GHA
        /// <summary>
        /// Saves information of AWB.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SaveAWBSummary_GHA(object[] AWBInfo, ref string ErrorMessage)
        {
            SQLServer da = new SQLServer(constr);
            try
            {
                //Prepare column names and datatypes...
                string[] paramNames = new string[50];
                SqlDbType[] dataTypes = new SqlDbType[50];
                int i = 0;

                //0
                paramNames.SetValue("BookingID", i);
                dataTypes.SetValue(SqlDbType.BigInt, i);
                i++;

                //1
                paramNames.SetValue("DocType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //2
                paramNames.SetValue("AWBPrefix", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //3
                paramNames.SetValue("AWBNumber", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("PiecesCount", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //5
                paramNames.SetValue("GrossWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //6
                paramNames.SetValue("VolumetricWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //7
                paramNames.SetValue("ChargedWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //8
                paramNames.SetValue("OriginCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //9
                paramNames.SetValue("DestinationCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //10
                paramNames.SetValue("AgentCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //11
                paramNames.SetValue("AgentName", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //12
                paramNames.SetValue("ServiceCargoClassId", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //13
                paramNames.SetValue("HandlingInfo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //14
                paramNames.SetValue("ExecutionDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //15
                paramNames.SetValue("ExecutedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //16
                paramNames.SetValue("ExecutedAt", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //17
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //18
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //19
                paramNames.SetValue("IsConsole", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //20
                paramNames.SetValue("IsBonded", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //21
                paramNames.SetValue("IsExport", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //22
                paramNames.SetValue("IsAgreed", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //23
                paramNames.SetValue("Remarks", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //24
                paramNames.SetValue("Interline", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //25
                paramNames.SetValue("SLAC", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //26
                paramNames.SetValue("Customs", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //27
                paramNames.SetValue("EURIN", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //28
                paramNames.SetValue("IrregId", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //29
                paramNames.SetValue("DVCustom", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //30
                paramNames.SetValue("DVCarriage", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //31
                paramNames.SetValue("SHCCodes", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //32
                paramNames.SetValue("ProductId", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //33
                paramNames.SetValue("DlvOnHAWB", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //34
                paramNames.SetValue("DesigCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //35

                paramNames.SetValue("CustomerCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //36

                paramNames.SetValue("Handler", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //37               
                paramNames.SetValue("ShippingAWB", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;
                //38               
                paramNames.SetValue("Documents", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;
                //39
                paramNames.SetValue("DriverName", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;
                //40
                paramNames.SetValue("DLNumber", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;
                //41
                paramNames.SetValue("PhoneNumber", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;
                //42
                paramNames.SetValue("VehicleNo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;
                //43
                paramNames.SetValue("IACCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;
                //44
                paramNames.SetValue("KnownShipper", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;
                //45
                paramNames.SetValue("CCSF", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;
                //46
                paramNames.SetValue("ToBeScreened", i);
                dataTypes.SetValue(SqlDbType.Bit, i);

                i++;
                //47
                paramNames.SetValue("ShipmentDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                //Added by Vishal - 04 MAY 2014 ******************** 
                //If AWBInfo is having UOM then pass it else ignore it.
                if (AWBInfo.Length > paramNames.Length)
                {
                    i++;
                    Array.Resize(ref paramNames, paramNames.Length + 1);
                    Array.Resize(ref dataTypes, dataTypes.Length + 1);
                    //48
                    paramNames.SetValue("UOM", i);
                    dataTypes.SetValue(SqlDbType.VarChar, i);
                }
                //******************** Added by Vishal - 04 MAY 2014 

                i++;
                //49
                paramNames.SetValue("PackagingInfo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;
                //50
                paramNames.SetValue("SCI", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                
                //Update AWB information.
                bool res = da.UpdateData("spSaveAWBSummary", paramNames, dataTypes, AWBInfo, ref ErrorMessage);
                paramNames = null;
                dataTypes = null;
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception Ex)
            {
            }
            finally 
            {
                da = null;
                AWBInfo = null;
            }
            return (-1);
        }
        #endregion Save AWB Summary

        #region Delete AWB Details
        /// <summary>
        /// Delete information of Material & Route.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int DeleteAWBDetails(string AWBNumber,string AWBPrefix)
        {
            SQLServer da = new SQLServer(constr);

            try
            {
                //Delete AWB Material information.
                string[] PName = new string[] { "AWBNumber", "AWBPrefix" };
                object[] PValue = new object[] { AWBNumber, AWBPrefix };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                //  bool res = da.UpdateData("spDeleteAWBDetails", "", SqlDbType.VarChar, AWBNumber);
                bool res = da.ExecuteProcedure("spDeleteAWBDetails", PName, PType, PValue);
                PName = null;
                PValue = null;
                PType = null;
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            finally 
            {
                da = null;
            }
            return (-1);
        }
        #endregion Save AWB Material

        #region Save AWB Material
        /// <summary>
        /// Saves information of Material.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SaveAWBMaterial(object[] MaterialInfo)
        {
            SQLServer da = new SQLServer(constr);
            try
            {
                //Prepare column names and datatypes...
                string[] paramNames = new string[11];
                SqlDbType[] dataTypes = new SqlDbType[11];
                int i = 0;

                //0
                paramNames.SetValue("AWBNumber", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //1
                paramNames.SetValue("CommodityCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //2
                paramNames.SetValue("Pieces", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //3
                paramNames.SetValue("Weight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //4
                paramNames.SetValue("Dimensions", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("VolumetricWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //6
                paramNames.SetValue("ChargedWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //7
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //8
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //9
                paramNames.SetValue("CommodityDesc", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //10
                paramNames.SetValue("AccountInfo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                //Update AWB information.
                bool res = da.UpdateData("spSaveAWBMaterial", paramNames, dataTypes, MaterialInfo);
                paramNames = null;
                dataTypes = null;
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            finally 
            {
                da = null;
                MaterialInfo = null;
            }
            return (-1);
        }

        public int SaveAWBMaterial_GHA(object[] MaterialInfo)
        {
            SQLServer da = new SQLServer(constr);
            try
            {
                //Prepare column names and datatypes...
                string[] paramNames = new string[15];
                SqlDbType[] dataTypes = new SqlDbType[15];
                int i = 0;

                //0
                paramNames.SetValue("AWBNumber", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //1
                paramNames.SetValue("CommodityCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //2
                paramNames.SetValue("Pieces", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //3
                paramNames.SetValue("Weight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //4
                paramNames.SetValue("Dimensions", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("VolumetricWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //6
                paramNames.SetValue("ChargedWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //7
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //8
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //9
                paramNames.SetValue("CommodityDesc", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //10
                paramNames.SetValue("AccountInfo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //11
                paramNames.SetValue("ShipmentType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //12
                paramNames.SetValue("Remarks", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //13
                paramNames.SetValue("AWBPrefix", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //14
                paramNames.SetValue("ShipmentPriority", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                
                //Update AWB information.
              
                bool res = da.UpdateData("spSaveAWBMaterial", paramNames, dataTypes, MaterialInfo);
                paramNames = null;
                dataTypes = null;
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            finally 
            {
                da = null;
                MaterialInfo = null;
            }
            return (-1);
        }

        #endregion Save AWB Material

        #region Save AWB Rates

        public bool SaveAWBRates(object[] values)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());                
            try
            {

                string[] param = { "AWBNumber", "CommCode", "PayMode", "Pcs", "Wt", "FrIATA", "FrMKT", "ValCharge", "OcDueCar", 
                                     "OcDueAgent", "SpotRate", "DynRate", "ServiceTax", "Total", "RatePerKg", "RateClass", 
                                     "Currency", "AWBPrefix", "SpotFreight", "IATATax", "MKTTax", "OATax", "OCTax", "SpotTax",
                                     "CommTax", "DiscTax", "Commission", "Discount", "CommPercent", "SpotStatus", "AllInRate",
                                     "IATARateID", "MKTRateID" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Float,
                                          SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, 
                                          SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Decimal,
                                          SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float, 
                                          SqlDbType.Float, SqlDbType.Float,SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, 
                                          SqlDbType.Float, SqlDbType.Float,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.VarChar };


                bool flag= db.ExecuteProcedure("SP_SaveAWBRates", param, dbtypes, values);
                param = null;
                dbtypes = null;
                return flag;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally 
            {
                db = null;
                values = null;
            }
        }

        #endregion

        #region Save AWB Dimensions
        public bool SaveAWBDimensions(object[] values)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            try
            {

                string[] param = { "AWBNumber", "RowIndex", "Length", "Breadth", "Height", "PcsCount", "MeasureUnit" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar };
                bool flag = db.ExecuteProcedure("SP_SaveAWBDimensions", param, dbtypes, values);
                param = null;
                dbtypes = null;
                return flag;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally 
            {
                db = null;
                values = null;
            }
        }

        #endregion

        #region Save AWB Piece Details
        public bool SaveAWBPiecedetails(object[] values)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            try
            {

                string[] param = { "AWBNumber", "PieceDetails", "AWBPrfix" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                bool flag = db.ExecuteProcedure("sp_SaveAWBPieceDetails", param, dbtypes, values);
                param = null;
                dbtypes = null;
                return flag;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally 
            {
                db = null;
                values = null;
            }
        }
        #endregion

        #region Save AWB Rates Details
        public bool SaveAWBRatesDetails(string type,object[] values)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            bool flag = false;
            try
            {
                string[] param = { "AWBNumber", "ChargeHeadCode", "ChargeType", "DiscountPercent", 
                                       "CommPercent", "TaxPercent", "Discount", "Comission", "Tax","Charge","CommCode","CCode","AWBPrefix","ChargeSrNo"};
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Float, 
                                            SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float,SqlDbType.Float,
                                            SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar};
                flag = db.ExecuteProcedure("SP_SaveAWBOCRatesDetails", param, dbtypes, values);
                param = null;
                dbtypes = null;
                values = null;
                return flag;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally 
            {
                db = null;
            }
        }

        public bool SaveAWBRatesDetails_QB(string AWBPrefix, string AWBNumber, string Commodity, string RateDetails)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            bool flag = false;

            try
            {
                string[] param = { "AWBPrefix", "AWBNumber", "CommodityCode", "RateDetails"};
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar};
                object[] values = { AWBPrefix, AWBNumber, Commodity, RateDetails };

                flag = db.ExecuteProcedure("sp_SaveAWBOtherCharges_QB", param, dbtypes, values);

                param = null;
                dbtypes = null;
                values = null;

                return flag;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                db = null;
            }
        }

        #endregion

        #region Save AWB Route
        /// <summary>
        /// Saves information of Route.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SaveAWBRoute(object[] RouteInfo)
        {
            SQLServer da = new SQLServer(constr);

            try
            {
                //Prepare column names and datatypes...
                string[] paramNames = new string[] { "AWBNumber", "FltOrigin", "FltDestination", "FltNumber", "FltDate", "Pcs", "Wt", "Status", "Accepted", "AcceptedPcs", "AcceptedWt", "UpdatedBy", "UpdatedOn", "ScheduleID", "ChrgWt", "Carrier", "PartnerType", "AWBPrefix", "ShipmentDescription" };
                SqlDbType[] dataTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };


                //Update AWB information.
                bool res = da.UpdateData("spSaveAWBRoute", paramNames, dataTypes, RouteInfo);
                paramNames = null;
                dataTypes = null;
                RouteInfo = null;
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            finally 
            {
                da = null;
            }
            return (-1);
        }
        #endregion Save AWB Route

        #region Save AWB Shipper/Consignee
        public bool SaveAWBShipperConsignee(object[] values)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            bool flag = false;
            try
            {

                string[] param = { "AWBNumber", "ShipperName", "ShipperAddress", "ShipperCountry", "ShipperTelephone", "ConsigneeName", 
                                     "ConsigneeAddress", "ConsigneeCountry", "ConsigneeTelephone", "ShipAdd2", "ShipCity", "ShipState", 
                                     "ShipPincode", "ConsigAdd2", "ConsigCity", "ConsigState", "ConsigPincode", 
                                     "ShipperAccCode","ConsigAccCode","ShipperEmailId","ConsigEmailId","AWBPrefix","ShipperCCSF","ConsigneeCCSF"};
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, 
                                          SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, 
                                          SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,
                                      SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar};

                flag = db.ExecuteProcedure("SP_InsertAWBShipperConsigneeDetails", param, dbtypes, values);
                param = null;
                dbtypes = null;
                values = null;
                return flag;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally 
            {
                db = null;
            }
        }
        #endregion

        #region Get Location List
        /// <summary>
        /// Get list of all the locations based on entered value.
        /// </summary>
        /// <returns>Location list as Array.</returns>
        public DataSet GetLocationList(string LocCode)
        {
            SQLServer da = new SQLServer(constr);
                DataSet ds=null;
                try
                {
                    //Get location names ...
                    ds = da.SelectRecords("spGetLocationsByPrefix", "StationCode", LocCode, SqlDbType.VarChar);
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
                    ds = null;
                }
                finally 
                {
                    if (ds != null)
                        ds.Dispose();
                    da = null;
                }
            return (null);
        }
        #endregion Get Location List

        #region Get Destination List
        /// <summary>
        /// Get list of all the locations based on entered value.
        /// </summary>
        /// <returns>Location list as Array.</returns>
        public DataSet GetDestinationsForSource(string source)
        {
             SQLServer da = new SQLServer(constr);
                DataSet ds=null;
                try
                {
                    //Get location names ...
                    ds = da.SelectRecords("SP_GetDestinations", "source", source, SqlDbType.VarChar);
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
                    ds = null;
                }
                finally 
                {
                    if (ds != null)
                        ds.Dispose();
                    da = null;
                }
            return (null);
        }
        #endregion Get Destination List

        #region Get Agent List
        /// <summary>
        /// Get list of all the agents based on entered value.
        /// </summary>
        /// <returns>Agent code list as Array.</returns>
        public DataSet GetAgentList(string agentCode)
        {
                 SQLServer da = new SQLServer(constr);
                DataSet ds=null;
                try
                {
                    //Get agent codes...
                    ds = da.SelectRecords("spGetAgentByPrefix", "AgentCode", agentCode, SqlDbType.VarChar);
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
                    ds = null;
                }
                finally
                {
                    if (ds != null)
                        ds.Dispose();
                }
            return (null);
        }
        #endregion Get Agent List

        #region Get Agent List
        /// <summary>
        /// Get list of all the agents based on entered value.
        /// </summary>
        /// <returns>Agent code list as Array.</returns>
        public DataSet GetAgentListByStation(string station)
        {
            SQLServer da = new SQLServer(constr);
                DataSet ds=null;
                try
                {
                    //Get agent codes...
                    ds = da.SelectRecords("spGetAgentByStation", "Station", station, SqlDbType.VarChar);
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
                    ds = null;
                }
                finally 
                {
                    if (ds != null)
                        ds.Dispose();
                    da = null;
                }
            return (null);
        }
        #endregion Get Agent List

        #region Get Commodity List
        /// <summary>
        /// Get list of all the commodities based on entered value.
        /// </summary>
        /// <returns>Commodity code list as Array.</returns>
        public DataSet GetCommodityList(string commodityCode)
        {
            SQLServer da = new SQLServer(constr);
                DataSet ds =null;
                try
                {
                    //Get commodity codes...
                    ds = da.SelectRecords("spGetCommodityByPrefix_New", "CommodityCode", commodityCode, SqlDbType.VarChar);
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
                    ds = null;
                }
                finally 
                {
                    if (ds != null)
                        ds.Dispose();
                    da = null;
                }
            return (null);
        }
        #endregion Get Commodity List

        #region GetFlightPrefix

        public DataSet GetFlightPrefixList()
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;
            try
            {
                //Get FlightPrefix codes...

                ds = da.SelectRecords("sp_GetFlightPrefix");
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
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
            return (null); 
        }
     #endregion GetFlightPrefix

        #region Get Flight List
        /// <summary>
        /// Get list of all the flights based on entered value.
        /// </summary>
        /// <returns>Flight code list as Array.</returns>
        public DataSet GetFlightList(string flightCode)
        {
                SQLServer da = new SQLServer("");
                DataSet ds =null;
                try
                {
                    //Get flight codes...
                    ds = da.SelectRecords("spGetFlightByPrefix", "FlightCode", flightCode, SqlDbType.VarChar);
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
                finally
                {
                    da = null;
                }
            return (null);
        }

        public bool GetFlightList(string origin,string destination,ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                //Get flight codes...
                SQLServer da = new SQLServer(Global.GetConnectionString());

                string[] param = { "Origin","Dest"};
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { origin, destination };

                dsResult = da.SelectRecords("spGetFlightListFromOrgToDest", param, values, sqldbtype);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            errormessage = "No record found";
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
            
        }
     


        #endregion Get Flight List

        #region Get Flight List By Route
        /// <summary>
        /// Gets list of flights based on Origin and Destination of flight.
        /// </summary>
        /// <param name="Origin">Origin location for flight.</param>
        /// <param name="Dest">Destination location for flight.</param>
        /// <returns>Returns list as string separated by ';'.</returns>
        public string GetFlightListByRoute(string Origin, string Dest)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            try
            {
                string[] colNames = new string[2];
                string[] values = new string[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

                colNames.SetValue("Origin", i);
                values.SetValue(Origin, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Destination", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                ds = da.SelectRecords("spGetFlightForRoute", colNames, values, dataTypes);
                string flightList = " Select";
                colNames = null;
                values = null;
                dataTypes = null;
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        flightList = "";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            flightList = flightList + dr["FlightNumber"].ToString() + ";";
                        }
                    }
                }

                return (flightList);

            }
            catch (Exception)
            {
                ds = null;
                return ("");
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }

        }
        #endregion Get Flight List By Route


        #region Get Next AWB Number
        /// <summary>
        /// Checks if AWB number is already issued.
        /// </summary>
        /// <returns>True if AWB is not used.</returns>
        public bool GetNextAWB(string AgentCode, ref string AWBNumber, ref string errormessage)
        {
            SQLServer da = new SQLServer(constr);
                DataSet ds =null;
                try
                {
                    //Prepare column names and datatypes...
                    string paramName = "AgentCode";
                    SqlDbType dataType = SqlDbType.VarChar;


                    //Update AWB information.
                    ds = da.SelectRecords("SP_GetNextAWBNumberForAgent", paramName, AgentCode, dataType);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0].ToString().Trim() != "0" || ds.Tables[0].Rows[0].ToString().Trim() != "")
                                {   //AWB is used.
                                    AWBNumber = ds.Tables[0].Rows[0][0].ToString();
                                    return true;
                                }
                                else
                                {   //AWB is not used.
                                    errormessage = "Stock not allocated.";
                                    return false;
                                }
                            }
                        }
                    }

                    return false;

                }
                catch (Exception ex)
                {
                    ds = null;
                    errormessage = "" + ex.Message;
                    return false;
                }
                finally 
                {
                    if (ds != null)
                        ds.Dispose();
                    da = null;
                }

        }

        #endregion Get Next AWB Number

        public bool GetAWBDetails(string AWBNumber,string AWBPrefix,ref DataSet dsResult,ref string errormessage)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] PName = new string[] { "AWBNumber", "AWBPrefix" };
                object[] PValue = new object[] { AWBNumber, AWBPrefix };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                dsResult = da.SelectRecords("SP_GetAWBDetailsPrefix", PName, PValue, PType);
                PName = null;
                PValue = null;
                PType = null;
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetAWBDetails) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAWBDetails) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "Error :(GetAWBDetails)" + ex.Message;
                return false;
            }
            finally 
            {
                da = null;
            }
        }

        #region Get Special Commodities
        /// <summary>
        /// Get list of all the locations based on entered value.
        /// </summary>
        /// <returns>Location list as Array.</returns>
        public DataSet GetSpecialCommodities()
        {
             SQLServer da = new SQLServer(constr);
                DataSet ds=null;
                try
                {
                    //Get location names ...
                    ds = da.SelectRecords("SP_GetSpecialCommodityCodes");
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
                finally 
                {
                    da = null;
                }
            return (null);
        }
        #endregion 
        
        #region AWBStatus

        public bool SetAWBStatus(string AWBNumber, string Status, ref string errormessage, string ExecutionDt,
            string UserName, DateTime CurrentDt, string AWBPrefix, bool ValidateData, string PaymentMode)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet dsResult = null;
            try
            {
                string[] param = { "AWBNumber", "Status", "ExecutionDt", "UserName", "TimeStamp", "AWBPrefix", "ValidateData", "PaymentMode"};
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.VarChar};
                object[] values = { AWBNumber, Status, ExecutionDt, UserName, CurrentDt, AWBPrefix, ValidateData, PaymentMode};

                dsResult = da.SelectRecords("SP_SetAWBStatus", param, values, sqldbtype);
                param = null;
                sqldbtype = null;
                values = null;
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {
                            if (dsResult.Tables[0].Rows[0][0].ToString() == "Y")
                                return true;
                            else
                            {
                                errormessage = "" + dsResult.Tables[0].Rows[0][1].ToString();
                                return false;
                            }
                        }
                        else
                        {
                            errormessage = "Error :(SetAWBStatus) Code III";
                            return false;
                        }

                    }
                    else
                    {
                        errormessage = "Error :(SetAWBStatus) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(SetAWBStatus) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                dsResult = null;
                errormessage = "" + ex.Message;
                return false;
            }
            finally 
            {
                if (dsResult != null)
                    dsResult.Dispose();
                da = null;
            }

        }

        public bool SetAWBStatus(string AWBNumber, string Status, ref string errormessage, string ExecutionDt,
            string UserName, DateTime CurrentDt, string AWBPrefix, bool ValidateData, string PaymentMode, DateTime ExecutionDateTime)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet dsResult = null;
            try
            {
                string[] param = { "AWBNumber", "Status", "ExecutionDt", "UserName", "TimeStamp", "AWBPrefix", "ValidateData", "PaymentMode", "ExecutionDateTime" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.VarChar, SqlDbType.DateTime };
                object[] values = { AWBNumber, Status, ExecutionDt, UserName, CurrentDt, AWBPrefix, ValidateData, PaymentMode, ExecutionDateTime };

                dsResult = da.SelectRecords("SP_SetAWBStatus", param, values, sqldbtype);
                param = null;
                sqldbtype = null;
                values = null;
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {
                            if (dsResult.Tables[0].Rows[0][0].ToString() == "Y")
                                return true;
                            else
                            {
                                errormessage = "" + dsResult.Tables[0].Rows[0][1].ToString();
                                return false;
                            }
                        }
                        else
                        {
                            errormessage = "Error :(SetAWBStatus) Code III";
                            return false;
                        }

                    }
                    else
                    {
                        errormessage = "Error :(SetAWBStatus) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(SetAWBStatus) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                dsResult = null;
                errormessage = "" + ex.Message;
                return false;
            }
            finally
            {
                if (dsResult != null)
                    dsResult.Dispose();
                da = null;
            }

        }

        public bool GetAWBStatus(string AWBNumber,string AWBPrefix,ref string Status, ref string errormessage)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds=null;
            try
            {

                string[] param = { "AWBNumber", "AWBPrefix" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { AWBNumber, AWBPrefix };

                ds = da.SelectRecords("SP_GetAWBStatus", param, values, sqldbtype);
                param = null;
                sqldbtype = null;
                values = null;
                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            Status = ds.Tables[0].Rows[0][0].ToString();
                            return true;
                        }
                        else
                        {
                            errormessage = "AWB Details not found.";
                            return false;
                        }

                    }
                    else
                    {
                        errormessage = "Error :(GetAWBStatus) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAWBStatus) Code I";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ds = null;
                errormessage = "" + ex.Message;
                return false;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }

        }

        #endregion 

        #region GetAWBStock
        public DataSet GetAWBStock(object[] AWB)
        {
            SQLServer da = new SQLServer(constr);
                DataSet ds=null;
                try
                {

                    string[] ParamNames = new string[] { "AWBNumber", "AWBPrefix", "Station" };
                    SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };

                    ds = da.SelectRecords("SpGetAgentStockDetails", ParamNames, AWB, ParamTypes);
                    ParamNames = null;
                    ParamTypes = null;
                    AWB = null;
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
                finally 
                {
                    da = null;
                }
            return (null);


        }
        #endregion

        public DataSet VerifyEmbargoCargo(DateTime Ondate, string Origin, string Destination, string Commodity, string PaymentType, string FlightNo)
        {
            SQLServer da = new SQLServer(constr);
            try
            {

                string[] Pname = new string[6];
                object[] Pvalue = new object[6];
                SqlDbType[] Ptype = new SqlDbType[6];

                Pname[0] = "dtOnDate";
                Pname[1] = "strOrigin";
                Pname[2] = "strDestination";
                Pname[3] = "strCommodity";
                Pname[4] = "strPaymentType";
                Pname[5] = "strFlightNo";

                Ptype[0] = SqlDbType.DateTime;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;

                Pvalue[0] = Ondate;
                Pvalue[1] = Origin;
                Pvalue[2] = Destination;
                Pvalue[3] = Commodity;
                Pvalue[4] = PaymentType;
                Pvalue[5] = FlightNo;

                DataSet objDS = da.SelectRecords("spValidateEMBARGOCargo", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;
                return objDS;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally 
            {
                da = null;
            }
        }
        
        public bool GetScheduleId(string Origin, string Destination, string FlightNo, ref int ScheduleId)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            try
            {
                ScheduleId = 0;
                string strQuery = "Select top 1 ScheduleID from AirlineScheduleRoute where FlightID='" + FlightNo.Trim() + "' and Source='" + Origin.Trim() + "' and Dest='" + Destination + "' order by ScheduleID desc";

                ds = da.GetDataset(strQuery);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    return true;
                }
                else
                {
                    ScheduleId = 0;
                    return false;
                }

            }
            catch (Exception ex)
            {
                ds = null;
                ScheduleId = 0;
                return false;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
        }

        public DataSet GetAWBDimensions(string AWBNumber, string AWBPrefix)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                string strQuery = "select SerialNumber, AWBNumber, RowIndex, MeasureUnit 'Units', Length, Breadth 'Breath', Height, PcsCount, PieceId 'PieceNo', IdentificationNo, Volume 'Vol', Weight 'Wt',PieceType, BagNo, ULDNo, '' Location, FlightNo, convert(varchar,FlightDate,103) FlightDate,IsBulk from dbo.AWBDimensions where AWBNumber='" + AWBNumber.Trim() + "' and AWBPrefix='" + AWBPrefix + "'; ";
                strQuery = strQuery + "select Piece as Pieces, GrossWt, PieceId, 0 as RowIndex from AWBPieceDetails where AWBNumber='" + AWBNumber.Trim() + "' and AWBPrefix='" + AWBPrefix + "'; ";

                DataSet ds = da.GetDataset(strQuery);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
            }
        }

        public bool GetFligthDepartureStatus(string AWBNumber)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            try
            {

                string strQuery = "Select Isnull(IsDeparted,'') IsDeparted from expmanifestsummary where SerialNumber in (Select top 1 ManifestId from Expmanifestdetails where AWBno = '";
                strQuery = strQuery + AWBNumber + "' Order by SerialNumber)";

                ds = da.GetDataset(strQuery);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["IsDeparted"].ToString().Trim() == "D")
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ds = null;
                return false;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
        }

        public decimal GetAWBRateAmount(string AWBNumber, string AWBPrefix, decimal CurrencyConversion)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            decimal PreviousAmt = 0;

            try
            {                
                //string strQuery = "Select Isnull(Sum(Total / "+ CurrencyConversion +"),0) Total from AWBratemaster where PayMode = 'PX' and AWBNumber = '" + AWBNumber + "' and AWBPrefix = '" + AWBPrefix + "'; ";
                //strQuery = strQuery + "SELECT * FROM dbo.tblTransaction WHERE AWBNumber = '" + AWBNumber + "';";

                string Query = "SELECT (D.Amount - C.Amount) Total FROM (SELECT ISNULL(SUM(Amount),0) Amount FROM dbo.tblTransaction WHERE TransactionType = 'Debit' and AWBNumber = '" + AWBNumber + "') D,";
                Query = Query + " (SELECT ISNULL(SUM(Amount),0) Amount FROM dbo.tblTransaction WHERE TransactionType = 'Credit' and AWBNumber = '" + AWBNumber + "') C";

                ds = da.GetDataset(Query);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    //{
                        PreviousAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["Total"]);
                    //}
                    //else
                    //    PreviousAmt = 0;
                }
                else
                {
                    PreviousAmt = 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }

            return PreviousAmt;
        }

        public bool GetAccountDetails(string AgentCode, DateTime TranDate, ref string TranAccount, ref decimal BGAmount,
            ref decimal BankGAmt, ref decimal ThrValue, ref bool ValidateBG)
        {
            SQLServer da = new SQLServer(constr);            
            DataSet objDS = null;

            try
            {
                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];

                Pname[0] = "AgentCode";
                Pname[1] = "TranDate";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.DateTime;

                Pvalue[0] = AgentCode;
                Pvalue[1] = TranDate;

                objDS = da.SelectRecords("sp_GetAgentGuaranteeAmount", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;
                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0 && objDS.Tables[0].Rows[0][0].ToString() != "")
                {
                    TranAccount = objDS.Tables[0].Rows[0]["AgentCode"].ToString();
                    BGAmount = Convert.ToDecimal(objDS.Tables[0].Rows[0]["BalAmount"]);
                    BankGAmt = Convert.ToDecimal(objDS.Tables[0].Rows[0]["BankGAmt"]);
                    ThrValue = Convert.ToDecimal(objDS.Tables[0].Rows[0]["ThrValue"]);
                    ValidateBG = Convert.ToBoolean(objDS.Tables[0].Rows[0]["ValidateBG"]);
                }
                else
                {
                    TranAccount = "";
                    BGAmount = 0;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                objDS = null;
                da = null;
                return false;
            }
            finally
            {
                if (objDS != null)
                    objDS.Dispose();
                da = null;
            }
        }

        public bool GetAccountDetails(string AgentCode, DateTime TranDate, ref string TranAccount, ref decimal BGAmount,
            ref decimal BankGAmt, ref decimal ThrValue, ref bool ValidateBG, ref string AgentCurrency)
        {
            SQLServer da = new SQLServer(constr);
            DataSet objDS = null;

            try
            {
                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];

                Pname[0] = "AgentCode";
                Pname[1] = "TranDate";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.DateTime;

                Pvalue[0] = AgentCode;
                Pvalue[1] = TranDate;

                objDS = da.SelectRecords("sp_GetAgentGuaranteeAmount", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;
                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0 && objDS.Tables[0].Rows[0][0].ToString() != "")
                {
                    TranAccount = objDS.Tables[0].Rows[0]["AgentCode"].ToString();
                    BGAmount = Convert.ToDecimal(objDS.Tables[0].Rows[0]["BalAmount"]);
                    BankGAmt = Convert.ToDecimal(objDS.Tables[0].Rows[0]["BankGAmt"]);
                    ThrValue = Convert.ToDecimal(objDS.Tables[0].Rows[0]["ThrValue"]);
                    ValidateBG = Convert.ToBoolean(objDS.Tables[0].Rows[0]["ValidateBG"]);
                    AgentCurrency = Convert.ToString(objDS.Tables[0].Rows[0]["AgentCurrency"]);
                }
                else
                {
                    TranAccount = "";
                    BGAmount = 0;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                objDS = null;
                da = null;
                return false;
            }
            finally
            {
                if (objDS != null)
                    objDS.Dispose();
                da = null;
            }
        }

        public bool CheckforFinalStatus(string AWBNumber,string AWBPrefix)
        {
            SQLServer da = new SQLServer(constr);            
            DataSet objDS = null;

            try
            {
                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];

                Pname[0] = "AWBNumber";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = AWBNumber;


                Pname[1] = "AWBPrefix";
                Ptype[1] = SqlDbType.VarChar;
                Pvalue[1] = AWBPrefix;

                objDS = da.SelectRecords("sp_CheckAWBforfinalStatus", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;
                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                    return Convert.ToBoolean(objDS.Tables[0].Rows[0][0]);
                else
                    return false;
            }
            catch (Exception ex)
            {
                objDS = null;
                da = null;
                return false;
            }
            finally
            {
                if (objDS != null)
                    objDS.Dispose();
                da = null;
            }
        }

        public void CheckforFinalStatusNew(string AWBNumber, string AWBPrefix, string FlightNo, string FlightDate, string OffloadPsc, string OffloadWgt)
        {
            SQLServer da = new SQLServer(constr);
            DataSet objDS = null;

            try
            {
                string[] Pname = new string[6];
                object[] Pvalue = new object[6];
                SqlDbType[] Ptype = new SqlDbType[6];

                Pname[0] = "AWBNumber";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = AWBNumber;


                Pname[1] = "AWBPrefix";
                Ptype[1] = SqlDbType.VarChar;
                Pvalue[1] = AWBPrefix;

                Pname[2] = "FlightNo";
                Ptype[2] = SqlDbType.VarChar;
                Pvalue[2] = FlightNo;

                Pname[3] = "FlightDate";
                Ptype[3] = SqlDbType.VarChar;
                Pvalue[3] = FlightDate;

                Pname[4] = "OffloadPCS";
                Ptype[4] = SqlDbType.VarChar;
                Pvalue[4] = OffloadPsc;

                Pname[5] = "OffloadWgt";
                Ptype[5] = SqlDbType.VarChar;
                Pvalue[5] = OffloadWgt;

                da.SelectRecords("sp_CheckAWBforfinalStatusNew", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;
                
            }
            catch (Exception ex)
            {
                objDS = null;
                da = null;
                
            }
            finally
            {
                if (objDS != null)
                    objDS.Dispose();
                da = null;
            }
        }

        public string CheckFlightDetails(string AWBNumber,string AWBPrefix, string FlightDetails)
        {
            SQLServer da = new SQLServer(constr);
            DataSet objDS = null;

            try
            {
                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];

                Pname[0] = "AWBNumber";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = AWBNumber;

                Pname[1] = "FltDetails";
                Ptype[1] = SqlDbType.NText;
                Pvalue[1] = FlightDetails;

                Pname[2] = "AWBPrefix";
                Ptype[2] = SqlDbType.VarChar;
                Pvalue[2] = AWBPrefix;

                objDS = da.SelectRecords("spCheckFlightDetails", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;

                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                    return Convert.ToString(objDS.Tables[0].Rows[0][0]);
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                objDS = null;
                da = null;
                return ex.Message;
            }
            finally
            {
                if (objDS != null) 
                    objDS.Dispose();

                da = null;
            }
        }

        public DataSet CheckPrimeRate(string Origin, string Destination, DateTime ExecutionDt, string FlightDts)
        {
            DataSet dsResult = null;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] param = { "Origin", "Destination", "ExecutionDt", "Flightdetails" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar };
                object[] values = { Origin, Destination, ExecutionDt, FlightDts };

                dsResult = da.SelectRecords("spCheckPrimerate", param, values, sqldbtype);
                param = null;
                sqldbtype = null;
                values = null;

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            return dsResult;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally 
            {
                da = null;
            }

        }

        public bool GetEmailFormat(string AWBNumber, string CommodityCode, DateTime ExecutionDt, ref string EmailBody, ref string EmailId)
        {
            SQLServer da = new SQLServer(constr);            
            DataSet objDS = null;

            try
            {
                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];

                Pname[0] = "AWBNumber";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = AWBNumber;

                Pname[1] = "Commodity";
                Ptype[1] = SqlDbType.VarChar;
                Pvalue[1] = CommodityCode;

                Pname[2] = "ExecutionDt";
                Ptype[2] = SqlDbType.DateTime;
                Pvalue[2] = ExecutionDt;

                objDS = da.SelectRecords("spGetEmailformatforCommodity", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;
                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    EmailBody = objDS.Tables[0].Rows[0][0].ToString();
                    EmailId = objDS.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                objDS = null;
                da = null;
                return false;
            }
            finally
            {
                if (objDS != null)
                    objDS.Dispose();
                da = null;
            }
        }

        #region Get All Country
        /// <summary>
        /// Get list of all the country based on entered value.
        /// </summary>
        /// <returns>country code list as Array.</returns>
        public DataSet GetAllCountry()
        {
            SQLServer da = new SQLServer(constr);
            try
            {
                DataSet ds = da.SelectRecords("SPGetCountryCodeName");
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
            finally 
            {
                da = null;
            }
            return (null);
        }
        #endregion Get All Country

        #region CheckforAgentException
        public bool CheckforAgentException(string AgentCode)
        {
            SQLServer da = new SQLServer(constr);
            DataSet objDS = null;
            string strQuery = string.Empty;

            try
            {
                strQuery = "Select * from AgentMaster where IsFOC = 1 and AgentCode = '" + AgentCode + "'";
                objDS = da.GetDataset(strQuery);

                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                objDS = null;
                da = null;
                return false;
            }
            finally
            {
                if(objDS!=null)
                objDS.Dispose();
                da = null;
            }
        }
        #endregion

        #region Get Airlince Code
        public DataSet GetAirlineCode()
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;

            try
            {
                ds = da.SelectRecords("sp_GetAirlineCode");//da.GetDataset(strQuery);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                ds = null;
                da = null;
            }
        }

        public DataSet GetAirlineCode(bool interline,string partner)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null, dret = new DataSet();
            DataTable dt = null;
            try
            {
                //string strQuery = "SELECT Distinct ParamValue FROM dbo.RateLineParams WHERE ParamName = 'FlightNum' AND ISNULL(ParamValue,'') !='' ORDER BY ParamValue asc";

                //string strQuery = "Exec sp_GetAirlineCode";
                string pname = "Partner";
                object pvalue = partner.Length>0?partner:"Airline";
                SqlDbType ptype = SqlDbType.VarChar;
                ds = da.SelectRecords("sp_GetAirlineCode",pname,pvalue,ptype);//da.GetDataset(strQuery);


                if (ds != null && ds.Tables.Count > 0)//&& ds.Tables[0].Rows.Count > 0)
                {
                    if (interline)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dt = ds.Tables[0].Copy();
                            dt.AcceptChanges();
                            dret.Tables.Add(dt);
                            dret.AcceptChanges();
                            return dret;
                        }
                        else
                            return null;
                    }
                    else
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            dt = ds.Tables[1].Copy();
                            dt.AcceptChanges();
                            dret.Tables.Add(dt);
                            dret.AcceptChanges();
                            return dret;
                        }
                        else
                            return null;

                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                ds = null;
                da = null;
            }
        }

        public DataSet GetPartnerType(bool interline)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null, dret = new DataSet();
            DataTable dt = null;
            try
            {
                ds = da.SelectRecords("sp_GetPartnerType");


                if (ds != null && ds.Tables.Count > 0)//&& ds.Tables[0].Rows.Count > 0)
                {
                    if (interline)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dt = ds.Tables[0].Copy();
                            dt.AcceptChanges();
                            dret.Tables.Add(dt);
                            dret.AcceptChanges();
                            return dret;
                        }
                        else
                            return null;
                    }
                    else
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            dt = ds.Tables[1].Copy();
                            dt.AcceptChanges();
                            dret.Tables.Add(dt);
                            dret.AcceptChanges();
                            return dret;
                        }
                        else
                            return null;
                    }
                }
                else
                    return null;
            
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                ds = null;
                da = null;
            }
        }

        #endregion

        #region CheckStockThreshold
        public DataSet CheckStockThreshold(string agentcode)
        {
            SQLServer objSQL = new SQLServer(constr);                
            DataSet ds = new DataSet();
            try
            {
                string pname = "agent";
                object pvalue = agentcode;
                SqlDbType ptype = SqlDbType.VarChar;
                ds = objSQL.SelectRecords("sp_CheckStockThreshold", pname, pvalue, ptype);
                return ds;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally 
            {
                objSQL = null;
            }
        }
        #endregion

        #region SaveAWBRateBreakUp
        public bool SaveAWBRateBreakUp(object[] values)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            bool flag = false;
            try
            {

                string[] param = { "AWBNumber", "CommCode", "PayMode", "Pcs", "Wt", "FrIATA", "FrMKT", "ValCharge", "OcDueCar", "OcDueAgent", "SpotRate", "DynRate", "ServiceTax", "Total", "RatePerKg", "CurrencyCode", "ProPer", "Carrier", "FlightNo" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Decimal, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                flag = db.ExecuteProcedure("SP_SaveAWBRateBreakup_BACK", param, dbtypes, values);
                param = null;
                dbtypes = null;
                values = null;
                return flag;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally 
            {
                db = null;
            }
        }
        #endregion

        #region GetPrimeFlight
        public DataSet GetPrimeFlight(string CurrentDate, string Origin, string destination)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;

            try
            {
             
                string strQuery = string.Empty;

                strQuery = "SELECT Distinct ParamValue, M.Origin FROM dbo.RateLineParams P, dbo.RateLineMaster M ";
                strQuery = strQuery + "WHERE P.RateLineSrNo = M.SerialNumber and ParamName = 'FlightNum' AND ISNULL(ParamValue,'') !='' AND ";
                strQuery = strQuery + "Origin = '" + Origin + "' AND Destination='" + destination + "' AND ";
                strQuery = strQuery + "M.StartDate <= '" + CurrentDate + "' AND EndDate >= '" + CurrentDate + "' ORDER BY ParamValue asc; ";

                strQuery = strQuery + "SELECT Distinct ParamValue, M.Origin FROM dbo.OtherchargesParams P, dbo.OtherchargesMaster M ";
                strQuery = strQuery + "WHERE P.ChargeHeadSrNo = M.SerialNumber and ParamName = 'FlightNum' AND ISNULL(ParamValue,'') !='' AND ";
                strQuery = strQuery + "Origin = '" + Origin + "' AND Destination='" + destination + "' AND ";
                strQuery = strQuery + "M.StartDate <= '" + CurrentDate + "' AND EndDate >= '" + CurrentDate + "' ORDER BY ParamValue asc;";

                ds = da.GetDataset(strQuery);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                ds = null;
                da = null;
            }
        }
        #endregion

        #region CheckAWBDimensions
        public string CheckAWBValidations(string FlightDetails, string Origin, string Destination, DateTime SystemDate, string Dimensions)
        {
            DataSet objDS = null;
            SQLServer da = new SQLServer(constr);
            string strResult = string.Empty;

            try
            {

                string[] Pname = new string[5];
                object[] Pvalue = new object[5];
                SqlDbType[] Ptype = new SqlDbType[5];

                Pname[0] = "FltDetails";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = FlightDetails;

                Pname[1] = "Origin";
                Ptype[1] = SqlDbType.VarChar;
                Pvalue[1] = Origin;

                Pname[2] = "Destination";
                Ptype[2] = SqlDbType.VarChar;
                Pvalue[2] = Destination;

                Pname[3] = "SystemTime";
                Ptype[3] = SqlDbType.DateTime;
                Pvalue[3] = SystemDate;

                Pname[4] = "Dimensions";
                Ptype[4] = SqlDbType.VarChar;
                Pvalue[4] = Dimensions;

                objDS = da.SelectRecords("sp_CheckAWBBookingValidations", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;
                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    strResult = Convert.ToString(objDS.Tables[0].Rows[0][0]);
                }

                return strResult;
            }
            catch (Exception ex)
            {
                objDS = null;
                da = null;
                return ex.Message;
            }
            finally
            {
                if(objDS!=null)
                objDS.Dispose();
                da = null;
            }
        }
        #endregion

        #region GetAirpotCurrency
        public bool GetAirpotCurrency(string airport, ref DataSet ds)
        {
            SQLServer db = new SQLServer(constr);               
            bool flag = false;
            try
            {
                ds = db.SelectRecords("spGetAirportCurrency", "airport", airport, SqlDbType.VarChar);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            flag = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                flag = false;
            }
            finally 
            {
                db = null;
            }
            return flag;
        }
         #endregion

        public DataSet getAvailableforCargoReceipt(string prefix, string awbno)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = new DataSet();
            try
            {

                string[] param = { "AWBPre", "AWBNo" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { prefix, awbno };

                ds = da.SelectRecords("spchkAvailableforCargoReceipt", param, values, sqldbtype);
                param = null;
                sqldbtype = null;
                values = null;
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally 
            {
                da = null;
            }
        }

        #region GetRateAgenctCapacity
        public DataSet GetRateAgenctCapacity(string agent, string flight, string commodity, string date)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet dsResult = null;
            try
            {
                string[] param = { "agent", "flight", "commodity", "date" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { agent, flight, commodity, date };

                dsResult = da.SelectRecords("spCheckAgentCapacityRate", param, values, sqldbtype);
                param = null;
                sqldbtype = null;
                values = null;
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0 || dsResult.Tables[1].Rows.Count > 0)
                        {
                            return dsResult;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally 
            {
                da = null;
            }

        }
        #endregion

        public DataSet GetAvailabePartners()
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet dsResult = new DataSet();

            try
            {
                string strQuery = "SELECT DISTINCT PartnerCode FROM dbo.tblPartnerMaster ORDER BY PartnerCode ASC";

                dsResult = da.GetDataset(strQuery);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return dsResult;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
            }
        }

        public DataSet GetAvailabeAWBPrefixs()
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet dsResult = new DataSet();

            try
            {
                string strQuery = "SELECT DISTINCT AirlinePrefix FROM AWBPrefixMaster ORDER BY AirlinePrefix ASC";

                dsResult = da.GetDataset(strQuery);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return dsResult;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
            }
        }

        public bool GetAvailabePartners(string Origin, string Destination, DateTime dtDate,string mode, ref DataSet dsResult, ref string errormessage)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                //Get flight codes...
                string[] param = { "Origin", "Dest", "Date", "dtCurrentDate", "PartnerType" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.DateTime, SqlDbType.VarChar };
                object[] values = { Origin, Destination, 0, dtDate, mode };

                dsResult = da.SelectRecords("spGetPartnerCodeFromOrgToDest", param, values, sqldbtype);
                param = null;
                sqldbtype = null;
                values = null;
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
            finally 
            {
                da = null;
            }

        }

        public bool SaveAWBRateDetails(string AWBNumber, string CreatedBy, DateTime CreatedDt, string RateDetails,string AWBPrefix)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] param = { "AWBNumber", "RateDetails", "CreatedBy", "CreatedOn", "AWBPrefix" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar };
                object[] values = { AWBNumber, RateDetails, CreatedBy, CreatedDt, AWBPrefix };

                bool blnResult = da.InsertData("sp_CreateAWBRateLogNew", param, sqldbtype, values);
                param = null;
                sqldbtype = null;
                values = null;
                return blnResult;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally 
            {
                da = null;
            }

        }

        public bool SaveAWBTaxDetails(string AWBNumber, string CreatedBy, DateTime CreatedDt, string TaxDetails, string AWBPrefix)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] param = { "AWBNumber", "TaxDetails", "CreatedBy", "CreatedOn", "AWBPrefix" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar };
                object[] values = { AWBNumber, TaxDetails, CreatedBy, CreatedDt, AWBPrefix };

                bool blnResult = da.InsertData("sp_CreateAWBTaxLog", param, sqldbtype, values);
                param = null;
                sqldbtype = null;
                values = null;
                return blnResult;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                da = null;
            }

        }
        
        public DataSet GetHandlerCode(string station, string Product)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            try
            {
                string[] PName = new string[] { "station", "product" };
                object[] PValue = new object[] { station, Product };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                ds = da.SelectRecords("spGetHandlerCode", PName, PValue, PType);
                PName = null;
                PValue = null;
                PType = null;
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally 
            {
                da = null;
            }
            return ds;
        }

        public DataSet GenerateAWBDimensions(string AWBNumber, int AWBPieces, DataSet Dimensions, decimal AWBWt, string UserName, DateTime TimeStamp, bool IsCreate,string AWBPrefix, bool IsBulk)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());                
            DataSet ds = null;
            try
            {
                System.Text.StringBuilder strDimensions = new System.Text.StringBuilder();

                if (Dimensions != null && Dimensions.Tables.Count > 0 && Dimensions.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < Dimensions.Tables[0].Rows.Count; intCount++)
                    {
                        strDimensions.Append("Insert into #tblPieceInfo(PieceNo, IdentificationNo, Length, Breath, Height, Vol, Wt, Units, PieceType, BagNo, ULDNo, Location, FlightNo, FlightDate,IsBulk) values (");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["PieceNo"]);
                        strDimensions.Append(",'");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["IdentificationNo"]);
                        strDimensions.Append("',");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Length"]);
                        strDimensions.Append(",");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Breath"]);
                        strDimensions.Append(",");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Height"]);
                        strDimensions.Append(",");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Vol"]);
                        strDimensions.Append(",");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Wt"]);
                        strDimensions.Append(",'");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Units"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["PieceType"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["BagNo"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["ULDNo"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Location"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["FlightNo"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["FlightDate"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["IsBulk"]);

                        strDimensions.Append("'); ");
                    }
                }

                string[] PName = new string[] { "AWBNumber", "Pieces", "PieceInfo", "UserName", "TimeStamp", "IsCreate", "AWBWeight", "AWBPrefix","IsBulk" };
                object[] PValue = new object[] { AWBNumber, AWBPieces, strDimensions.ToString(), UserName, TimeStamp, IsCreate, AWBWt, AWBPrefix,IsBulk };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.Bit, SqlDbType.Decimal, SqlDbType.VarChar,SqlDbType.Bit };
                ds = da.SelectRecords("sp_StoreCourierDetails", PName, PValue, PType);
                PName = null;
                PValue = null;
                PType = null;
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally 
            {
                da = null;
            }
            return ds;
        }

        public DataSet GenerateAWBDimensionsAcceptance(string AWBNumber, int AWBPieces, DataSet Dimensions, decimal AWBWt, string UserName, DateTime TimeStamp, bool IsCreate, string AWBPrefix, string FromAcceptance,string FlightNo,string FlightDate)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            try
            {
                System.Text.StringBuilder strDimensions = new System.Text.StringBuilder();

                if (Dimensions != null && Dimensions.Tables.Count > 0 && Dimensions.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < Dimensions.Tables[0].Rows.Count; intCount++)
                    {
                        strDimensions.Append("Insert into #tblPieceInfo(SrNo, PieceID, Length, Breadth, Height, Volume, Weight, Units, PieceType, BagNo, ULDNo, Location, FlightNo, FlightDate) values (");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["PieceNo"]);
                        strDimensions.Append(",'");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["IdentificationNo"]);
                        strDimensions.Append("',");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Length"]);
                        strDimensions.Append(",");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Breath"]);
                        strDimensions.Append(",");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Height"]);
                        strDimensions.Append(",");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Vol"]);
                        strDimensions.Append(",");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Wt"]);
                        strDimensions.Append(",'");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Units"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["PieceType"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["BagNo"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["ULDNo"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["Location"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["FlightNo"]);
                        strDimensions.Append("','");
                        strDimensions.Append(Dimensions.Tables[0].Rows[intCount]["FlightDate"]);
                        strDimensions.Append("'); ");
                    }
                }

                string[] PName = new string[] { "AWBNumber", "Pieces", "PieceInfo", "UserName", "TimeStamp", "IsCreate", "AWBWeight", "AWBPrefix","FromAcceptance","FlightNo","FlightDate" };
                object[] PValue = new object[] { AWBNumber, AWBPieces, strDimensions.ToString(), UserName, TimeStamp, IsCreate, AWBWt, AWBPrefix,FromAcceptance,FlightNo,FlightDate };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.Bit, SqlDbType.Decimal, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ds = da.SelectRecords("sp_StoreCourierDetails_V1", PName, PValue, PType);
                PName = null;
                PValue = null;
                PType = null;
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                da = null;
            }
            return ds;
        }

        public string getSITAID(string AirlineCode)
        {
            string SITAID = "";
            SQLServer da = new SQLServer(Global.GetConnectionString());

            try 
            {
                string query = "Select SitaID from  AirlinePrefixMaster where AirlineCode = '" + AirlineCode+"'";
                SITAID = da.GetString(query);
                da = null;

            }
            catch (Exception ex) { }
            return SITAID;
        }

        public bool ValidateUserToView(string AWBNumber,string AWBPrefix, string user, string station)
        {
            bool flag = false;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            try
            {
                string[] PName = new string[] { "AWBNumber", "User", "Station", "AWBPrefix" };
                object[] PValue = new object[] { AWBNumber, user, station, AWBPrefix };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                ds = da.SelectRecords("spValidateAWBViewForUser", PName, PValue, PType);
                PType = null;
                PName = null;
                PValue = null;
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            flag = bool.Parse(ds.Tables[0].Rows[0][0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
            return flag;
        }

        #region ProcessRates

        public DataSet ProcessRates(string AWBNo, string CommCode, decimal ChargedWt, string AWBPrefix)
        {

            try
            {

                // Result
                DataSet dsResult = new DataSet();
                dsResult.Tables.Add();
                dsResult.Tables[0].TableName = "Rates";
                dsResult.Tables[0].Columns.Add("FrIATA");
                dsResult.Tables[0].Columns.Add("FrMKT");
                dsResult.Tables[0].Columns.Add("OCDC");
                dsResult.Tables[0].Columns.Add("OCDA");
                dsResult.Tables[0].Columns.Add("ServTax");



                // OC
                DataSet dsOCDetails = new DataSet();
                dsOCDetails.Tables.Add();
                dsOCDetails.Tables[0].TableName = "OCDA";
                dsOCDetails.Tables[0].Columns.Add("Commodity Code");
                dsOCDetails.Tables[0].Columns.Add("Charge Head Code");
                dsOCDetails.Tables[0].Columns.Add("Charge Type");
                dsOCDetails.Tables[0].Columns.Add("Charge");
                dsOCDetails.Tables[0].Columns.Add("TaxPercent");
                dsOCDetails.Tables[0].Columns.Add("Tax");
                dsOCDetails.Tables[0].Columns.Add("DiscountPercent");
                dsOCDetails.Tables[0].Columns.Add("Discount");
                dsOCDetails.Tables[0].Columns.Add("CommPercent");
                dsOCDetails.Tables[0].Columns.Add("Commission");



                //Rates
                DataTable dtRates = new DataTable();
                dtRates.Columns.Add("CommCode");
                dtRates.Columns.Add("Pcs");
                dtRates.Columns.Add("Weight");
                dtRates.Columns.Add("FrIATA");
                dtRates.Columns.Add("FrMKT");
                dtRates.Columns.Add("ValCharge");
                dtRates.Columns.Add("PayMode");
                dtRates.Columns.Add("OcDueCar");
                dtRates.Columns.Add("OcDueAgent");
                dtRates.Columns.Add("SpotRate");
                dtRates.Columns.Add("DynRate");
                dtRates.Columns.Add("ServTax");
                dtRates.Columns.Add("Total");


                // Get AWB Info
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SP_GetAWBInfoToProcessRates", "AWBNumber", AWBNo, SqlDbType.VarChar);

                string Orgin, Dest, BookDt, AgentCode, FlightNum, errormessage = "";

                Orgin = ds.Tables[0].Rows[0][0].ToString();
                Dest = ds.Tables[0].Rows[0][1].ToString();
                BookDt = Convert.ToDateTime(ds.Tables[0].Rows[0][2].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                AgentCode = ds.Tables[0].Rows[0][3].ToString();
                FlightNum = ds.Tables[0].Rows[0][4].ToString();

                if (CommCode.Trim() != "" && ChargedWt > 0)
                {
                    ProcessRatesByCommodity(ref dsOCDetails, ref dtRates, CommCode, ChargedWt, Orgin, Dest, BookDt, AgentCode, FlightNum, ref errormessage, AWBPrefix);
                }
                else
                {

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        CommCode = row[0].ToString();
                        ChargedWt = decimal.Parse(row[1].ToString());

                        ProcessRatesByCommodity(ref dsOCDetails, ref dtRates, CommCode, ChargedWt, Orgin, Dest, BookDt, AgentCode, FlightNum, ref errormessage, AWBPrefix);
                    }
                }

                decimal friata, frmkt, ocda, ocdc, tax;
                friata = frmkt = ocda = ocdc = tax = 0;

                foreach (DataRow rw in dtRates.Rows)
                {
                    friata += decimal.Parse(rw["FrIATA"].ToString());
                    frmkt += decimal.Parse(rw["FrMKT"].ToString());
                    ocda += decimal.Parse(rw["OcDueAgent"].ToString());
                    ocdc += decimal.Parse(rw["OcDueCar"].ToString());
                    tax += decimal.Parse(rw["ServTax"].ToString());

                }

                DataRow dsResultRow = dsResult.Tables[0].NewRow();

                dsResultRow["FrIATA"] = friata;
                dsResultRow["FrMKT"] = frmkt;
                dsResultRow["OCDC"] = ocdc;
                dsResultRow["OCDA"] = ocda;
                dsResultRow["ServTax"] = tax;

                dsResult.Tables[0].Rows.Add(dsResultRow);

                return dsResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public void ProcessRatesByCommodity(ref DataSet dsOCDetails, ref DataTable dtRates, string CommCode, decimal ChargedWt, string Origin, string Dest, string BookDt, string AgentCode, string FlightNum, ref string errormessage, string AWBPrefix)
        {
            try
            {

                string RateCardType, AirlineCode, HandlingCode, ShipperCode;
                decimal GrossWt;
                decimal IATACharge, MKTCharge, ServiceTax, OCDC, OCDA,
                        TotalDiscount, TotalCommission,

                        DiscountPercentIATA, DiscountPercentMKT,
                        CommissionPercentIATA, CommissionPercentMKT,
                        TaxPercentIATA, TaxPercentMKT,

                        DiscountIATA, DiscountMKT,
                        CommissionIATA, CommissionMKT,
                        ServiceTaxIATA, ServiceTaxMKT;

                string OCDADetails, OCDCDetails, RateLineIATA = "", RateLineMKT = "";


                OCDADetails = OCDCDetails = "";
                IATACharge = MKTCharge = ServiceTax = ServiceTaxIATA = ServiceTaxMKT = OCDC = OCDA = TotalDiscount = TotalCommission = DiscountPercentIATA = CommissionPercentIATA = DiscountPercentMKT = CommissionPercentMKT = TaxPercentIATA = TaxPercentMKT = 0;
                DiscountIATA = DiscountMKT = CommissionIATA = CommissionMKT = ServiceTaxIATA = ServiceTaxMKT = 0;


                // Origin = ddlOrg.Text;
                // Dest = ddlDest.Text;
                // BookDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                // FlightNum = ((HiddenField)grdRouting.Rows[0].FindControl("hdnFltNum")).Value;
                // AgentCode = ddlAgtCode.SelectedItem.Text;
                // CommCode = ((DropDownList)grdMaterialDetails.Rows[rowindex].FindControl("ddlMaterialCommCode")).SelectedItem.Text;
                RateCardType = "IATA";
                AirlineCode = AWBPrefix;
                HandlingCode = "";
                ShipperCode = "";
                // GrossWt = Convert.ToDecimal(((TextBox)grdMaterialDetails.Rows[rowindex].FindControl("txtCommGrossWt")).Text);
                // ChargedWt = Convert.ToDecimal(((TextBox)grdMaterialDetails.Rows[rowindex].FindControl("txtCommChargedWt")).Text);

                //if (GrossWt < ChargedWt)
                GrossWt = ChargedWt;


                string[] param = { "org", "dest", "BookDt", "FlightNum", "AgentCode", "CommCode", "RateCardType", "GrossWt", "HandlingCode", "ShipperCode", "AirlineCode" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { Origin, Dest, BookDt, FlightNum, AgentCode, CommCode, RateCardType, GrossWt, HandlingCode, ShipperCode, AirlineCode };

                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet dsResultIATA = db.SelectRecords("SP_RateCalculation", param, values, dbtypes);
                values[6] = "Market";
                DataSet dsResultMKT = db.SelectRecords("SP_RateCalculation", param, values, dbtypes);

                // IATA

                if (dsResultIATA != null)
                {

                    if (dsResultIATA.Tables.Count != 0)
                    {

                        if (dsResultIATA.Tables[0].Rows.Count != 0)
                        {
                            // done

                            RateLineIATA = dsResultIATA.Tables[0].Rows[0]["RateLineID"].ToString();
                            IATACharge = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["Charge"].ToString());

                            ServiceTax = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["ServiceTax"].ToString());
                            TotalDiscount = TotalDiscount + Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["Discount"].ToString());
                            TotalCommission = TotalCommission + Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["Commission"].ToString());

                            DiscountPercentIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["DiscountPercent"].ToString());
                            CommissionPercentIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["CommissionPercent"].ToString());
                            TaxPercentIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["TaxPercent"].ToString());

                            DiscountIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["Discount"].ToString());
                            CommissionIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["Commission"].ToString());
                            ServiceTaxIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["ServiceTax"].ToString());


                        }
                        else
                            errormessage = "Error :Code-O03";

                    }
                    else
                        errormessage = "Error :Code-O02";
                }
                else
                    errormessage = "Error :Code-O01";


                // MKT

                if (dsResultMKT != null)
                {

                    if (dsResultMKT.Tables.Count != 0)
                    {

                        if (dsResultMKT.Tables[0].Rows.Count != 0)
                        {
                            // done

                            RateLineMKT = dsResultMKT.Tables[0].Rows[0]["RateLineID"].ToString();
                            MKTCharge = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["Charge"].ToString());

                            ServiceTax = ServiceTax + Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["ServiceTax"].ToString());
                            TotalDiscount = TotalDiscount + Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["Discount"].ToString());
                            TotalCommission = TotalCommission + Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["Commission"].ToString());

                            DiscountPercentMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["DiscountPercent"].ToString());
                            CommissionPercentMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["CommissionPercent"].ToString());
                            TaxPercentMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["TaxPercent"].ToString());

                            DiscountMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["Discount"].ToString());
                            CommissionMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["Commission"].ToString());
                            ServiceTaxMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["ServiceTax"].ToString());


                        }
                        else
                            errormessage = "Error :Code-O06";
                    }
                    else
                        errormessage = "Error :Code-O05";
                }
                else
                    errormessage = "Error :Code-O04";







                #region Other Charges

                param = new string[] { "org", "dest", "BookDt", "FlightNum", "AgentCode", "CommCode", "ChargeType", "GrossWt", "ChargedWt", "HandlingCode", "ShipperCode", "AirlineCode", "HAWB" };
                dbtypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int };
                values = new object[] { Origin, Dest, BookDt, FlightNum, AgentCode, CommCode, "DA", GrossWt, ChargedWt, HandlingCode, ShipperCode, AirlineCode, 0 };

                db = new SQLServer(Global.GetConnectionString());


                //OCDA

                DataSet dsResultOtherCharges = db.SelectRecords("SP_RateCalculationForOtherCharges", param, values, dbtypes);

                if (dsResultOtherCharges != null)
                {

                    if (dsResultOtherCharges.Tables.Count != 0)
                    {

                        if (dsResultOtherCharges.Tables[0].Rows.Count != 0)
                        {
                            // done
                            // TotalCharge Tax Discount Commission Details

                            OCDA = Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["TotalCharge"].ToString());
                            ServiceTax = ServiceTax + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Tax"].ToString());
                            OCDADetails = dsResultOtherCharges.Tables[0].Rows[0]["Details"].ToString();
                            TotalDiscount = TotalDiscount + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Discount"].ToString());
                            TotalCommission = TotalCommission + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Commission"].ToString());


                        }
                        else
                            errormessage = "Error :Code-O06";
                    }
                    else
                        errormessage = "Error :Code-O05";
                }
                else
                    errormessage = "Error :Code-O04";




                //OCDC

                values[6] = "DC";
                dsResultOtherCharges = db.SelectRecords("SP_RateCalculationForOtherCharges", param, values, dbtypes);

                if (dsResultOtherCharges != null)
                {

                    if (dsResultOtherCharges.Tables.Count != 0)
                    {

                        if (dsResultOtherCharges.Tables[0].Rows.Count != 0)
                        {
                            // done
                            // TotalCharge Tax Discount Commission Details

                            OCDC = Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["TotalCharge"].ToString());
                            ServiceTax = ServiceTax + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Tax"].ToString());
                            OCDCDetails = dsResultOtherCharges.Tables[0].Rows[0]["Details"].ToString();
                            TotalDiscount = TotalDiscount + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Discount"].ToString());
                            TotalCommission = TotalCommission + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Commission"].ToString());

                        }
                        else
                            errormessage = "Error :Code-O06";
                    }
                    else
                        errormessage = "Error :Code-O05";
                }
                else
                    errormessage = "Error :Code-O04";



                // Charge Head Code,Charge Type,Charge,Tax%,Tax,DiscountPercent,Discount,CommPercent,Commission

                //DataSet dsDetails = (DataSet)Session["OCDetails"];
                //if (dsDetails != null && dsDetails.Tables.Count != 0)
                //    dsDetails.Tables[0].Rows.Clear();



                //DataSet dsDetails = (DataSet)Session["OCDetails"];
                DataSet dsDetails = dsOCDetails;
                //if (dsDetails != null && dsDetails.Tables.Count != 0)
                //    dsDetails.Tables[0].Rows.Clear();


                // OCDA
                string[] strOCDADetails = OCDADetails.Split(new char[] { ',' });

                for (int i = 0; i < strOCDADetails.Length; i++)
                {
                    string[] strDAdetails = strOCDADetails[i].Split(new char[] { ':' });
                    DataRow rowDA = dsDetails.Tables[0].NewRow();

                    rowDA["Commodity Code"] = CommCode;
                    rowDA["Charge Head Code"] = strDAdetails[0].ToString();
                    rowDA["Charge Type"] = "OCDA";
                    rowDA["Charge"] = strDAdetails[1].ToString();
                    rowDA["TaxPercent"] = strDAdetails[2].ToString();
                    rowDA["Tax"] = strDAdetails[3].ToString();
                    rowDA["DiscountPercent"] = strDAdetails[4].ToString();
                    rowDA["Discount"] = strDAdetails[5].ToString();
                    rowDA["CommPercent"] = strDAdetails[6].ToString();
                    rowDA["Commission"] = strDAdetails[7].ToString();

                    dsDetails.Tables[0].Rows.Add(rowDA);
                }


                // OCDC
                string[] strOCDCDetails = OCDCDetails.Split(new char[] { ',' });

                for (int i = 0; i < strOCDCDetails.Length; i++)
                {
                    string[] strDCdetails = strOCDCDetails[i].Split(new char[] { ':' });
                    DataRow rowDC = dsDetails.Tables[0].NewRow();

                    rowDC["Commodity Code"] = CommCode;
                    rowDC["Charge Head Code"] = strDCdetails[0].ToString();
                    rowDC["Charge Type"] = "OCDC";
                    rowDC["Charge"] = strDCdetails[1].ToString();
                    rowDC["TaxPercent"] = strDCdetails[2].ToString();
                    rowDC["Tax"] = strDCdetails[3].ToString();
                    rowDC["DiscountPercent"] = strDCdetails[4].ToString();
                    rowDC["Discount"] = strDCdetails[5].ToString();
                    rowDC["CommPercent"] = strDCdetails[6].ToString();
                    rowDC["Commission"] = strDCdetails[7].ToString();

                    dsDetails.Tables[0].Rows.Add(rowDC);
                }

                #endregion


                // ADD FrIATA details

                DataRow dsDetailsRow = dsDetails.Tables[0].NewRow();

                dsDetailsRow["Commodity Code"] = CommCode;
                dsDetailsRow["Charge Head Code"] = RateLineIATA;
                dsDetailsRow["Charge Type"] = "RateLineIATA";
                dsDetailsRow["Charge"] = IATACharge;
                dsDetailsRow["TaxPercent"] = TaxPercentIATA;
                dsDetailsRow["Tax"] = ServiceTaxIATA;
                dsDetailsRow["DiscountPercent"] = DiscountPercentIATA;
                dsDetailsRow["Discount"] = DiscountIATA;
                dsDetailsRow["CommPercent"] = CommissionPercentIATA;
                dsDetailsRow["Commission"] = CommissionIATA;

                dsDetails.Tables[0].Rows.Add(dsDetailsRow);


                // ADD FrMKT details 

                dsDetailsRow = dsDetails.Tables[0].NewRow();

                dsDetailsRow["Commodity Code"] = CommCode;
                dsDetailsRow["Charge Head Code"] = (RateLineMKT.Trim() == "" ? "0" : RateLineMKT);
                dsDetailsRow["Charge Type"] = "RateLineMKT";
                dsDetailsRow["Charge"] = MKTCharge;
                dsDetailsRow["TaxPercent"] = TaxPercentMKT;
                dsDetailsRow["Tax"] = ServiceTaxMKT;
                dsDetailsRow["DiscountPercent"] = DiscountPercentMKT;
                dsDetailsRow["Discount"] = DiscountMKT;
                dsDetailsRow["CommPercent"] = CommissionPercentMKT;
                dsDetailsRow["Commission"] = CommissionMKT;

                dsDetails.Tables[0].Rows.Add(dsDetailsRow);


                //Session["OCDetails"] = dsDetails.Copy();



                //DataTable dt = ((DataTable)Session["dtRates"]).Copy();
                DataTable dt = dtRates;
                //dt.Rows.Clear();

                DataRow row;
                row = dt.NewRow();
                row["CommCode"] = CommCode;// "TV";
                //row["Pcs"] = ((TextBox)grdMaterialDetails.Rows[rowindex].FindControl("TXTPcs")).Text;
                row["Pcs"] = "";
                row["Weight"] = GrossWt;// "7KG";
                row["FrIATA"] = "" + IATACharge;// "IATA";
                row["FrMKT"] = "" + MKTCharge;// "IATA";
                row["ValCharge"] = "0";// "IATA";
                row["PayMode"] = "";// "COD";
                row["OcDueCar"] = "" + OCDC;// "200.00";
                row["OcDueAgent"] = "" + OCDA;// "200.00";
                row["SpotRate"] = "0";// "500.00";
                row["DynRate"] = "0";// "0";
                row["ServTax"] = "" + ServiceTax;// "0";
                row["Total"] = "" + (IATACharge + MKTCharge + ServiceTax + OCDA + OCDC);// "0";
                dt.Rows.Add(row);


                //Session["dtRates"] = dt.Copy();


                //GRDRates.DataSource = null;
                //GRDRates.DataSource = dt.Copy();
                //GRDRates.DataBind();


            }
            catch (Exception ex)
            {

            }

        }


        public DataSet ProcessRatesByFlightNo(string AWBNo, string CommCode, decimal ChargedWt, string FlightNo, string AWBPrefix)
        {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;
            try
            {

                // Result
                DataSet dsResult = new DataSet();
                dsResult.Tables.Add();
                dsResult.Tables[0].TableName = "Rates";
                dsResult.Tables[0].Columns.Add("FrIATA");
                dsResult.Tables[0].Columns.Add("FrMKT");
                dsResult.Tables[0].Columns.Add("OCDC");
                dsResult.Tables[0].Columns.Add("OCDA");
                dsResult.Tables[0].Columns.Add("ServTax");



                // OC
                DataSet dsOCDetails = new DataSet();
                dsOCDetails.Tables.Add();
                dsOCDetails.Tables[0].TableName = "OCDA";
                dsOCDetails.Tables[0].Columns.Add("Commodity Code");
                dsOCDetails.Tables[0].Columns.Add("Charge Head Code");
                dsOCDetails.Tables[0].Columns.Add("Charge Type");
                dsOCDetails.Tables[0].Columns.Add("Charge");
                dsOCDetails.Tables[0].Columns.Add("TaxPercent");
                dsOCDetails.Tables[0].Columns.Add("Tax");
                dsOCDetails.Tables[0].Columns.Add("DiscountPercent");
                dsOCDetails.Tables[0].Columns.Add("Discount");
                dsOCDetails.Tables[0].Columns.Add("CommPercent");
                dsOCDetails.Tables[0].Columns.Add("Commission");



                //Rates
                DataTable dtRates = new DataTable();
                dtRates.Columns.Add("CommCode");
                dtRates.Columns.Add("Pcs");
                dtRates.Columns.Add("Weight");
                dtRates.Columns.Add("FrIATA");
                dtRates.Columns.Add("FrMKT");
                dtRates.Columns.Add("ValCharge");
                dtRates.Columns.Add("PayMode");
                dtRates.Columns.Add("OcDueCar");
                dtRates.Columns.Add("OcDueAgent");
                dtRates.Columns.Add("SpotRate");
                dtRates.Columns.Add("DynRate");
                dtRates.Columns.Add("ServTax");
                dtRates.Columns.Add("Total");


                // Get AWB Info
            ds = da.SelectRecords("SP_GetAWBInfoToProcessRates", "AWBNumber", AWBNo, SqlDbType.VarChar);

                string Orgin, Dest, BookDt, AgentCode, FlightNum, errormessage = "";

                Orgin = ds.Tables[0].Rows[0][0].ToString();
                Dest = ds.Tables[0].Rows[0][1].ToString();
                BookDt = Convert.ToDateTime(ds.Tables[0].Rows[0][2].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                AgentCode = ds.Tables[0].Rows[0][3].ToString();
                FlightNum = FlightNo; // ds.Tables[0].Rows[0][4].ToString(); Based on Flight Number passed

                if (CommCode.Trim() != "" && ChargedWt > 0)
                {
                    ProcessRatesByCommodity(ref dsOCDetails, ref dtRates, CommCode, ChargedWt, Orgin, Dest, BookDt, AgentCode, FlightNum, ref errormessage, AWBPrefix);
                }
                else
                {

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        CommCode = row[0].ToString();
                        ChargedWt = decimal.Parse(row[1].ToString());

                        ProcessRatesByCommodity(ref dsOCDetails, ref dtRates, CommCode, ChargedWt, Orgin, Dest, BookDt, AgentCode, FlightNum, ref errormessage, AWBPrefix);
                    }
                }

                decimal friata, frmkt, ocda, ocdc, tax;
                friata = frmkt = ocda = ocdc = tax = 0;

                foreach (DataRow rw in dtRates.Rows)
                {
                    friata += decimal.Parse(rw["FrIATA"].ToString());
                    frmkt += decimal.Parse(rw["FrMKT"].ToString());
                    ocda += decimal.Parse(rw["OcDueAgent"].ToString());
                    ocdc += decimal.Parse(rw["OcDueCar"].ToString());
                    tax += decimal.Parse(rw["ServTax"].ToString());

                }

                DataRow dsResultRow = dsResult.Tables[0].NewRow();

                dsResultRow["FrIATA"] = friata;
                dsResultRow["FrMKT"] = frmkt;
                dsResultRow["OCDC"] = ocdc;
                dsResultRow["OCDA"] = ocda;
                dsResultRow["ServTax"] = tax;

                dsResult.Tables[0].Rows.Add(dsResultRow);

                return dsResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #endregion

        public bool CheckFlightDeparted(string POL,string POU,string FltNo,string date) 
        {
            bool flag = false;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            
            try 
            {
                string[] PName = new string[] 
                {
                    "Source",
                    "Dest",
                    "FltNo",
                    "Date"
                };
                object[] PValue = new object[] 
                {
                    POL,
                    POU,
                    FltNo,
                    date
                };
                SqlDbType[] PType = new SqlDbType[] 
                {
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar
                };
                string Stat = da.GetStringByProcedure("spGetFlightStatus", PName, PValue, PType);
                if (Stat.Equals("D", StringComparison.OrdinalIgnoreCase))
                    flag = true;
                PName = null;
                PType = null;
                PValue = null;
            }
            catch (Exception ex) 
            {
                flag = false;
            }
            return flag;
        }

        public bool CheckAWBExists(string AWBPrefix, string AWBNumber)
        {
            bool flag = false;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;

            try
            {
                string[] PName = new string[] {"AWBNumber","AWBPrefix" };
                object[] PValue = new object[] {AWBNumber,AWBPrefix };
                SqlDbType[] PType = new SqlDbType[]{SqlDbType.VarChar,SqlDbType.VarChar};
                ds=da.SelectRecords("SP_GetAWBDetailsPrefix",PName,PValue,PType);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
                {
                    flag = true;
                }
                PName = null;
                PValue = null;
                PType = null;
            }
            catch (Exception ex)
            {
                flag = false;
                ds = null;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
            return flag;
        }

        public string GetDesignatorCode(string Prefix) 
        {
            string Desig = "";
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try 
            {
                Desig = da.GetStringByProcedure("spGetDesignatorCode", "Prefix", Prefix, SqlDbType.VarChar);
            }
            catch (Exception ex) { }
            finally 
            {
                da = null;
            }
            return Desig;
        }

        public string ValidateULDFlow(string ULDNumbers, string FlightNo, DateTime FlightDt, string Origin,string AWBNumber,
            string AWBPrefix)
        {
            string Output = string.Empty;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;

            try
            {
                string[] PName = new string[] { "ULDNumber", "FlightNo", "FlightDate", "Origin", "AWBNumber", "AWBPrefix" };
                object[] PValue = new object[] { ULDNumbers, FlightNo, FlightDt, Origin, AWBNumber, AWBPrefix };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, 
                    SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.VarChar };
                
                ds = da.SelectRecords("sp_ValidateULDFlow", PName, PValue, PType);
                
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Output = ds.Tables[0].Rows[0][0].ToString();
                }
                PName = null;
                PValue = null;
                PType = null;
            }
            catch (Exception ex)
            {                
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
            return Output;
        }

        public DataSet LoadAWBMasterData()
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet objDS = null;
            try
            {
                objDS = da.GetDataset("spLoadAWBMasterData");
            }
            catch (Exception) { return null; }
            finally
            {
                da = null;
            }
            return objDS;
        }

        public bool SaveAsTemplate(string AWBPrefix, string AWBNumber, string strUserName)
        {
            bool flag = false;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] PName = new string[]
                {
                    "AWBPrefix",
                    "AWBNumber",
                    "User"
                };
                object[] PValue = new object[] 
                {
                    AWBPrefix,
                    AWBNumber,
                    strUserName
                };
                SqlDbType[] PTYpe = new SqlDbType[]
                {
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar
                };
                flag = da.ExecuteProcedure("spSaveAsTemplate",PName,PTYpe,PValue);
            }
            catch (Exception ex)
            {
                flag = false;
            }
            finally
            {
                da = null;
            }
            return flag;
        }

        public bool GetAWBTemplateDetails(string TemplateID, ref DataSet dsResult, ref string errormessage)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] PName = new string[] { "TemplateID"};
                object[] PValue = new object[] { TemplateID};
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar};
                dsResult = da.SelectRecords("SP_GetAWBTemplateDetails", PName, PValue, PType);
                PName = null;
                PValue = null;
                PType = null;
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetAWBDetails) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAWBDetails) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "Error :(GetAWBDetails)" + ex.Message;
                return false;
            }
            finally
            {
                da = null;
            }
        }

        #region get Configured Billing Event
        public string getConfiguredBillingEvent(string AWBNumber)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] PName = new string[]
                {
                    "AWBNumber"
                };
                object[] PValue = new object[] 
                {
                    AWBNumber
                };
                SqlDbType[] PTYpe = new SqlDbType[]
                {
                    SqlDbType.VarChar
                };

                string res = da.GetStringByProcedure("SP_GetConfiguredBillingEvent", PName, PValue, PTYpe);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion get Configured Billing Event

        #region Insert AWB Data In Billing
        public string InsertAWBDataInBilling(object[] AWBInfo)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("BillingFlag", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(AWBInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("StationCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBInfo.GetValue(i), i);

                string res = da.GetStringByProcedure("SP_InsertAWBDataInBilling_V2", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Insert AWB Data In Billing

        #region Insert AWB Data In Interline Invoice Billing
        public string InsertAWBDataInInterlineInvoice(object[] AWBInfo)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("BillingFlag", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(AWBInfo.GetValue(i), i);

                string res = da.GetStringByProcedure("SP_InsertAWBDataInInterlineInvoice", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Insert AWB Data In Interline Invoice Billing

        #region Insert AWB Data In Interline Credit Note Billing
        public string InsertAWBDataInInterlineCreditNote(object[] AWBInfo)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("BillingFlag", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(AWBInfo.GetValue(i), i);

                string res = da.GetStringByProcedure("SP_InsertAWBDataInInterlineCreditNote", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Insert AWB Data In Interline Credit Note Billing

        public string GetSHCCodesandDesc(string SHCCodes)
        {
            string Output = string.Empty;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;

            try
            {
                string[] PName = new string[] { "SHCCodes"};
                object[] PValue = new object[] { SHCCodes };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar};

                ds = da.SelectRecords("sp_GetSHCCodeDesc", PName, PValue, PType);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Output = ds.Tables[0].Rows[0][0].ToString();
                }
                PName = null;
                PValue = null;
                PType = null;
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
            return Output;
        }

        public DataSet GetAgentCodeDetails(string AgentCode, DateTime TimeStamp)
        {
            string Output = string.Empty;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;

            string[] PName = new string[] { "AgentCode", "TimeStamp" };
            object[] PValue = new object[] { AgentCode, TimeStamp };
            SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.DateTime };

            try
            {
                ds = da.SelectRecords("sp_GetAgentCodeDetails", PName, PValue, PType);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                PName = null;
                PValue = null;
                PType = null;
                da = null;
            }
            
            return ds;
        }

        //Added by Vishal - 04 MAY 2014 ********************
        #region GetAirpotCurrencyUOM
        public bool GetAirpotCurrencyWithUOM(string origin, string destination, ref DataSet ds)
        {
            SQLServer db = new SQLServer(constr);
            bool flag = false;
            try
            {
                string[] paramNames = new string[] { "origin", "destination" };
                object[] paramValues = new object[] { origin, destination };
                SqlDbType[] paramTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };

                ds = db.SelectRecords("spGetAirportCurrencyWithUOM",paramNames,paramValues,paramTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            flag = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                flag = false;
            }
            finally
            {
                db = null;
            }
            return flag;
        }
        #endregion
        //******************** Added by Vishal - 04 MAY 2014 

        #region Validate Known Shipper
        public bool ValidateKnownShipper(string origin, string destination,string shipperCode, string productType,
            string SpecialHandlingCode, string CommodityCode)
        {
            SQLServer db = new SQLServer(constr);
            bool flag = false;
            try
            {
                string[] paramNames = new string[] {"ShipperCode","OriginCode","DestinationCode","ProductType",
                        "SpecialHandlingCode","CommodityCode" };
                object[] paramValues = new object[] { shipperCode, origin, destination, productType, 
                        SpecialHandlingCode, CommodityCode};
                SqlDbType[] paramTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,
                        SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar};

                string returnValue = db.GetStringByProcedure("sp_ValidateKnownShipper", paramNames, paramValues, paramTypes);
                if (returnValue != null && returnValue != "")
                {
                    flag = Convert.ToBoolean(returnValue);
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            finally
            {
                db = null;
            }
            return flag;
        }
        #endregion

        public DataSet GetShipmentType(string Origin, string Destination)
        {
            string Output = string.Empty;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;

            string[] PName = new string[] { "Origin", "Destination" };
            object[] PValue = new object[] { Origin, Destination };
            SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };

            try
            {
                ds = da.SelectRecords("sp_GetShipmentType", PName, PValue, PType);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                PName = null;
                PValue = null;
                PType = null;
                da = null;
            }

            return ds;
        }

        #region GeteAWBPrintPrefence
        public string GeteAWBPrintPrefence(string agentcode, string AWBNumber, string AWBPrefix)
        {
            SQLServer db = new SQLServer(constr);
            string RatePref = "";
            try
            {
                string[] paramNames = new string[] { "agentcode", "AWBNumber", "AWBPrefix" };
                object[] paramValues = new object[] { agentcode, AWBNumber, AWBPrefix };
                SqlDbType[] paramTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                //ds = db.SelectRecords("spGeteAWBPrintPreference", paramNames, paramValues, paramTypes);
                RatePref = db.GetStringByProcedure("spGeteAWBPrintPreference", paramNames, paramValues, paramTypes);

            }
            catch (Exception ex)
            {
                RatePref = "";
            }
            return RatePref;
        }
        #endregion

        public bool CheckVolumetricExemption(string AgentCode, string Commodity, DateTime ExecutionDt,string ProductType,string Shipper,string Consignee)
        {
            bool blnOutput = false;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;

            string[] PName = new string[] { "Commodity", "AgentCode", "ExecDate","ProductType","Shipper","Consignee" };
            object[] PValue = new object[] { Commodity, AgentCode, ExecutionDt,ProductType,Shipper,Consignee };
            SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar };

            try
            {
                ds = da.SelectRecords("sp_CheckVolumetricExemption", PName, PValue, PType);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() != "0")
                        blnOutput = true;
                    else
                        blnOutput = false;
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                PName = null;
                PValue = null;
                PType = null;
                da = null;
            }

            return blnOutput;
        }

        //Added By Deepak Walmiki --04 JULY 2014*********************
        #region Get Original AWB Rates CCA
        public DataSet GetOriginalAWBRates(string AWBPrefix, string AWBNumber)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            string[] QueryNames = { "AWBPrefix", "AWBNumber" };
            object[] QueryValues = { AWBPrefix, AWBNumber };
            SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };
            try
            {

                DataSet ds = da.SelectRecords("spGetOrgAWBRatesCCA", QueryNames, QueryValues, QueryTypes);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                    return null;
            }
            catch (Exception ex)
            { return null; }
            finally
            {
                da = null;
                QueryNames = null;
                QueryValues = null;
                QueryTypes = null;

            }
        }
        #endregion

        public string CheckAWBValidationsforFlightCheckin(string FlightDetails, DateTime SystemDate)
        {
            DataSet objDS = null;
            SQLServer da = new SQLServer(constr);
            string strResult = string.Empty;

            try
            {
                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];

                Pname[0] = "FltDetails";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = FlightDetails;

                Pname[1] = "SystemTime";
                Ptype[1] = SqlDbType.DateTime;
                Pvalue[1] = SystemDate;

                objDS = da.SelectRecords("sp_CheckAWBBookingValidationsCheckIn", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;
                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    strResult = Convert.ToString(objDS.Tables[0].Rows[0][0]);
                }

                return strResult;
            }
            catch (Exception ex)
            {
                objDS = null;
                da = null;
                return ex.Message;
            }
            finally
            {
                if (objDS != null)
                    objDS.Dispose();
                da = null;
            }
        }

        public bool CheckDGRCargo(string Commodity,string SHC) 
        {
            bool flag = false;
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            try 
            {
                string[] PName = new string[] {"CommCode","SHC" };
                object[] PValue = new object[] { Commodity, SHC };
                SqlDbType[] PType = new SqlDbType[] {SqlDbType.VarChar,SqlDbType.VarChar };
                ds = da.SelectRecords("spCheckDGR", PName, PValue, PType);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
                {
                    flag = true;
                }

            }
            catch (Exception ex) { }
            return flag;
        }

        public decimal CurrencyConversion(string ConversionType, string BaseCurrency, string RequiredCurrency, DateTime ExchangeDate)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            decimal ExchangeRate = 0;
            
            string[] PName = new string[] { "CurrencyType", "BaseCurrency", "RequiredCurrency", "ExchangeDate" };
            object[] PValue = new object[] { ConversionType, BaseCurrency, RequiredCurrency, ExchangeDate };
            SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime };

            try
            {                
                ds = da.SelectRecords("sp_CurrencyConversion", PName, PValue, PType);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ExchangeRate = Convert.ToDecimal(ds.Tables[0].Rows[0][0]);
                }

            }
            catch (Exception ex) { }
            finally
            {
                PName = null; PValue = null; PType = null; ds = null;
            }

            return ExchangeRate;
        }

        public string GetAgentCurrency(string AgentCode)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string AgentCurrency = string.Empty;
            string Query = "SELECT CurrencyCode FROM dbo.AgentMaster WHERE AgentCode = '" + AgentCode + "'";

            try
            {
                ds = da.GetDataset(Query);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    AgentCurrency = Convert.ToString(ds.Tables[0].Rows[0][0]);
                }
            }
            catch (Exception ex) { }
            finally
            {
                ds = null;
            }

            return AgentCurrency;
        }

        public string GetAWBStatus(string AWBPrefix, string AWBNumber)
        {
            string Query = "SELECT AWBStatus FROM AWBSummaryMaster WHERE AWBNumber = '" + AWBNumber + "' AND AWBPrefix = '" + AWBPrefix + "'";

            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string AWBStatus = string.Empty;
            
            try
            {
                ds = da.GetDataset(Query);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    AWBStatus = Convert.ToString(ds.Tables[0].Rows[0][0]);
                }
                else
                    AWBStatus = "B";
            }
            catch (Exception ex) { }
            finally
            {
                ds = null;
            }

            return AWBStatus;
        }

        public bool ValidateAgentCode(string AgentCode, DateTime TimeStamp)
        {
            string Query = "select TOP 1 1 from AgentMaster where AgentCode ='" + AgentCode + "' AND ValidFrom <='" + TimeStamp
                + "' AND ValidTo >='" + TimeStamp + "'";

            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            bool IsValid = false;

            try
            {
                ds = da.GetDataset(Query);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    IsValid = true;
                }
                else
                    IsValid = false;
            }
            catch (Exception ex) { IsValid = false; }
            finally
            {
                ds = null;
            }

            return IsValid;
        }
    }    
}
