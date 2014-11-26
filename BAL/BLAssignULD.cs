using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;
namespace BAL
{
    public class BLAssignULD
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        #region Tab AWB Details
        public DataSet GetAWBLoadPlanULDDetails(string POL, string FlightNo, string FlightDate, string AWBPrefix,
            string AWBNumber, string ULDNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string[] Pname = new string[6];
            object[] Pvalue = new object[6];
            SqlDbType[] Ptype = new SqlDbType[6];

            try
            {
                Pname[0] = "POL";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = POL;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNo;

                Pname[2] = "FltDate";
                Ptype[2] = SqlDbType.VarChar;
                Pvalue[2] = FlightDate;

                Pname[3] = "AWBPrefix";
                Ptype[3] = SqlDbType.NVarChar;
                Pvalue[3] = AWBPrefix;

                Pname[4] = "AWBNumber";
                Ptype[4] = SqlDbType.NVarChar;
                Pvalue[4] = AWBNumber;

                Pname[5] = "ULDNumber";
                Ptype[5] = SqlDbType.NVarChar;
                Pvalue[5] = ULDNumber;

                ds = da.SelectRecords("SPGetAWBLoadPlanAssignULDDetailsNew", Pname, Pvalue, Ptype);

                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }
        #endregion Tab AWB Details

        #region Tab AWB Details
        public DataSet GetAWBLoadPlanULDDetails(string POL, string FlightNo, string FlightDate, string AWBPrefix, 
            string AWBNumber, string ULDNumber, bool ShowByDest)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string[] Pname = new string[7];
            object[] Pvalue = new object[7];
            SqlDbType[] Ptype = new SqlDbType[7];

            try
            {
                Pname[0] = "POL";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = POL;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNo;

                Pname[2] = "FltDate";
                Ptype[2] = SqlDbType.VarChar;
                Pvalue[2] = FlightDate;

                Pname[3] = "AWBPrefix";
                Ptype[3] = SqlDbType.NVarChar;
                Pvalue[3] = AWBPrefix;

                Pname[4] = "AWBNumber";
                Ptype[4] = SqlDbType.NVarChar;
                Pvalue[4] = AWBNumber;

                Pname[5] = "ULDNumber";
                Ptype[5] = SqlDbType.NVarChar;
                Pvalue[5] = ULDNumber;

                Pname[6] = "ShowByDest";
                Ptype[6] = SqlDbType.Bit;
                Pvalue[6] = ShowByDest;

                ds = da.SelectRecords("SPGetAWBLoadPlanAssignULDDetailsNew", Pname, Pvalue, Ptype);

                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }
        #endregion Tab AWB Details

        #region Get ULD List Details
        public DataSet GetULDListDetails(string POL, string FlightNo, string FlightDate, string AWBPrefix, string AWBNumber, string ULDNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            try
            {

                string[] Pname = new string[6];
                object[] Pvalue = new object[6];
                SqlDbType[] Ptype = new SqlDbType[6];


                Pname[0] = "POL";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = POL;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNo;

                Pname[2] = "FltDate";
                Ptype[2] = SqlDbType.VarChar;
                Pvalue[2] = FlightDate;

                Pname[3] = "AWBPrefix";
                Ptype[3] = SqlDbType.NVarChar;
                Pvalue[3] = AWBPrefix;

                Pname[4] = "AWBNumber";
                Ptype[4] = SqlDbType.NVarChar;
                Pvalue[4] = AWBNumber;

                Pname[5] = "ULDNumber";
                Ptype[5] = SqlDbType.NVarChar;
                Pvalue[5] = ULDNumber;

                ds = da.SelectRecords("SPGetULDListDetails", Pname, Pvalue, Ptype);


                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion Get ULD List Details

        #region Get AWB Details From ULD
        public DataSet GetAWBDetailsFromULD(string ULDNumber, string POL, string FLTNo, string FlightDate)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string[] Pname = new string[4];
            object[] Pvalue = new object[4];
            SqlDbType[] Ptype = new SqlDbType[4];

            try
            {
                Pname[0] = "ULDNumber";
                Pname[1] = "POL";
                Pname[2] = "FLTNo";
                Pname[3] = "FltDate";

                Ptype[0] = SqlDbType.NVarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;

                Pvalue[0] = ULDNumber;
                Pvalue[1] = POL;
                Pvalue[2] = FLTNo;
                Pvalue[3] = FlightDate;

                ds = da.SelectRecords("SPGetAWBDetailsFromULDNew", Pname, Pvalue, Ptype);

                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }
        #endregion Get AWB Details From ULD

        #region Tab ULDAWB Dest,VOL,Desc Details
        public DataSet GetULDAWBData(string AWBnos)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            try
            {


                string[] Pname = new string[1];
                object[] Pvalue = new object[1];
                SqlDbType[] Ptype = new SqlDbType[1];


                Pname[0] = "AWBNo";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = AWBnos;


                ds = da.SelectRecords("SPExpGetManiDestVolAWBdata", Pname, Pvalue, Ptype);

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
        #endregion Tab ULDAWB Dest,VOL,Desc Details

        #region Validate ULD
        public bool CheckULDValidity(string ULDNumber)
        {
            //if (ULDNumber.Length >= 5)
            if (ULDNumber.Length == 10)
            {
                string ULDType = ULDNumber.Substring(0, 3);
                string ULDOwner = ULDNumber.Substring(ULDNumber.Length - 2, 2);

                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];

                Pname[0] = "ULDType";
                Pname[1] = "ULDOwner";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;

                Pvalue[0] = ULDType;
                Pvalue[1] = ULDOwner;

                ds = da.SelectRecords("spULDValidate", Pname, Pvalue, Ptype);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString() == "OK")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            else
                return false;
        }
        #endregion

        #region Save/Add ULD AWb Details
        public DataSet AddULDAWBDetails(string ULDno, string AWBPrefix, string AWBno, int PCS, double Weight, string FLTno, string FltDate, string PosInFlight, string UldType, string ULDOrigin, string ULDDest, string dtCurrentDate, string Updatedby, string AWBLocation, string AWBLoadingPriority, string ULDLocation, string ULDLoadingPriority, double ScaleWeight, string ContainerType, string AWBBuilderName, string ULDBuilderName, string isReceived, ref string Result,string DollyWt)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[23];
                object[] Pvalue = new object[23];
                SqlDbType[] Ptype = new SqlDbType[23];


                Pname[0] = "ULDno";
                Pname[1] = "AWBPrefix";
                Pname[2] = "AWBNo";
                Pname[3] = "PCS";
                Pname[4] = "Weight";
                Pname[5] = "FLTNo";
                Pname[6] = "FltDate";
                Pname[7] = "PosInFlight";
                Pname[8] = "UldType";
                Pname[9] = "ULDOrigin";
                Pname[10] = "ULDDest";
                Pname[11] = "dtCurrentDate";
                Pname[12] = "Updatedby";
                Pname[13] = "AWBLocation";
                Pname[14] = "AWBLoadingPriority";
                Pname[15] = "ULDLocation";
                Pname[16] = "ULDLoadingPriority";
                Pname[17] = "ScaleWeight";
                Pname[18] = "ContainerType";
                Pname[19] = "AWBBuilderName";
                Pname[20] = "ULDBuilderName";
                Pname[21] = "isReceived";
                Pname[22] = "DollyWt";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.Int;
                Ptype[4] = SqlDbType.Float;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.VarChar;
                Ptype[17] = SqlDbType.Float;
                Ptype[18] = SqlDbType.VarChar;
                Ptype[19] = SqlDbType.VarChar;
                Ptype[20] = SqlDbType.VarChar;
                Ptype[21] = SqlDbType.VarChar;
                Ptype[22] = SqlDbType.VarChar;

                Pvalue[0] = ULDno;
                Pvalue[1] = AWBPrefix;
                Pvalue[2] = AWBno;
                Pvalue[3] = PCS;
                Pvalue[4] = Weight;
                Pvalue[5] = FLTno;
                Pvalue[6] = FltDate;
                Pvalue[7] = PosInFlight;
                Pvalue[8] = UldType;
                Pvalue[9] = ULDOrigin;
                Pvalue[10] = ULDDest;
                Pvalue[11] = dtCurrentDate;
                Pvalue[12] = Updatedby;
                Pvalue[13] = AWBLocation;
                Pvalue[14] = AWBLoadingPriority;
                Pvalue[15] = ULDLocation;
                Pvalue[16] = ULDLoadingPriority;
                Pvalue[17] = ScaleWeight;
                Pvalue[18] = ContainerType;
                Pvalue[19] = AWBBuilderName;
                Pvalue[20] = ULDBuilderName;
                Pvalue[21] = isReceived;
                Pvalue[22] = DollyWt;

                ds = da.SelectRecords("SPSaveAssignULDAWBDetailsNew", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Save/Add ULD Details
        public DataSet AddULDDetails(string ULDno, int PCS, double Weight, string FLTno, string FltDate, string PosInFlight, 
            string UldType, string ULDOrigin, string ULDDest, string dtCurrentDate, string Updatedby, string ULDLocation, 
            string ULDLoadingPriority, double ScaleWeight, string ContainerType, string ULDBuilderName, string isReceived,
            ref string Result, string DollyWt, string NewULDNo)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            string[] Pname = new string[19];
            object[] Pvalue = new object[19];
            SqlDbType[] Ptype = new SqlDbType[19];

            try
            {
                Pname[0] = "ULDno";
                Pname[1] = "PCS";
                Pname[2] = "Weight";
                Pname[3] = "FLTNo";
                Pname[4] = "FltDate";
                Pname[5] = "PosInFlight";
                Pname[6] = "UldType";
                Pname[7] = "ULDOrigin";
                Pname[8] = "ULDDest";
                Pname[9] = "dtCurrentDate";
                Pname[10] = "Updatedby";
                Pname[11] = "ULDLocation";
                Pname[12] = "ULDLoadingPriority";
                Pname[13] = "ScaleWeight";
                Pname[14] = "ContainerType";
                Pname[15] = "ULDBuilderName";
                Pname[16] = "isReceived";
                Pname[17] = "DollyWt";
                Pname[18] = "NewULDNo";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.Int;
                Ptype[2] = SqlDbType.Float;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.Float;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.VarChar;
                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.VarChar;

                Pvalue[0] = ULDno;
                Pvalue[1] = PCS;
                Pvalue[2] = Weight;
                Pvalue[3] = FLTno;
                Pvalue[4] = FltDate;
                Pvalue[5] = PosInFlight;
                Pvalue[6] = UldType;
                Pvalue[7] = ULDOrigin;
                Pvalue[8] = ULDDest;
                Pvalue[9] = dtCurrentDate;
                Pvalue[10] = Updatedby;
                Pvalue[11] = ULDLocation;
                Pvalue[12] = ULDLoadingPriority;
                Pvalue[13] = ScaleWeight;
                Pvalue[14] = ContainerType;
                Pvalue[15] = ULDBuilderName;
                Pvalue[16] = isReceived;
                Pvalue[17] = DollyWt;
                Pvalue[18] = NewULDNo;

                ds = da.SelectRecords("SPSaveAssignULDDetailsNew", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }

        #endregion

        #region Save/Add ULD Details BACKWARD COMPATIBLE
        public DataSet AddULDDetails(string ULDno, int PCS, double Weight, string FLTno, string FltDate, string PosInFlight,
            string UldType, string ULDOrigin, string ULDDest, string dtCurrentDate, string Updatedby, string ULDLocation,
            string ULDLoadingPriority, double ScaleWeight, string ContainerType, string ULDBuilderName, string isReceived,
            ref string Result, string DollyWt)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            string[] Pname = new string[18];
            object[] Pvalue = new object[18];
            SqlDbType[] Ptype = new SqlDbType[18];

            try
            {
                Pname[0] = "ULDno";
                Pname[1] = "PCS";
                Pname[2] = "Weight";
                Pname[3] = "FLTNo";
                Pname[4] = "FltDate";
                Pname[5] = "PosInFlight";
                Pname[6] = "UldType";
                Pname[7] = "ULDOrigin";
                Pname[8] = "ULDDest";
                Pname[9] = "dtCurrentDate";
                Pname[10] = "Updatedby";
                Pname[11] = "ULDLocation";
                Pname[12] = "ULDLoadingPriority";
                Pname[13] = "ScaleWeight";
                Pname[14] = "ContainerType";
                Pname[15] = "ULDBuilderName";
                Pname[16] = "isReceived";
                Pname[17] = "DollyWt";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.Int;
                Ptype[2] = SqlDbType.Float;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.Float;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.VarChar;
                Ptype[17] = SqlDbType.VarChar;

                Pvalue[0] = ULDno;
                Pvalue[1] = PCS;
                Pvalue[2] = Weight;
                Pvalue[3] = FLTno;
                Pvalue[4] = FltDate;
                Pvalue[5] = PosInFlight;
                Pvalue[6] = UldType;
                Pvalue[7] = ULDOrigin;
                Pvalue[8] = ULDDest;
                Pvalue[9] = dtCurrentDate;
                Pvalue[10] = Updatedby;
                Pvalue[11] = ULDLocation;
                Pvalue[12] = ULDLoadingPriority;
                Pvalue[13] = ScaleWeight;
                Pvalue[14] = ContainerType;
                Pvalue[15] = ULDBuilderName;
                Pvalue[16] = isReceived;
                Pvalue[17] = DollyWt;

                ds = da.SelectRecords("SPSaveAssignULDDetailsNew", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }

        #endregion

        #region Delete ULD AWb Details
        public DataSet DeleteULDAWBDetails(string ULDno, string AWBno, int pcs, float weight, string FlightNo, string FlightDate, string Origin, string Destination, string UDFlag, ref string Result)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            string[] Pname = new string[9];
            object[] Pvalue = new object[9];
            SqlDbType[] Ptype = new SqlDbType[9];

            try
            {
                Pname[0] = "ULDno";
                Pname[1] = "AWBNo";
                Pname[2] = "pcs";
                Pname[3] = "Weight";
                Pname[4] = "FLTNo";
                Pname[5] = "FltDate";
                Pname[6] = "ULDOrigin";
                Pname[7] = "ULDDest";
                Pname[8] = "DUFlag";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.Int;
                Ptype[3] = SqlDbType.Float;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;

                Pvalue[0] = ULDno;
                Pvalue[1] = AWBno;
                Pvalue[2] = pcs;
                Pvalue[3] = weight;
                Pvalue[4] = FlightNo;
                Pvalue[5] = FlightDate;
                Pvalue[6] = Origin;
                Pvalue[7] = Destination;
                Pvalue[8] = UDFlag;

                ds = da.SelectRecords("SPDelUpdateULDAWBAssignDetails", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }

                return ds;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                if (ds != null)
                    ds.Dispose();

                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }

        #endregion

        #region Delete ULD Bulk AWb Details
        public DataSet DeleteULDBulkAWBDetails(string ULDno, string AWBno, int pcs, float weight, string FlightNo, string FlightDate, string Origin, string Destination, string UDFlag, ref string Result)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            string[] Pname = new string[9];
            object[] Pvalue = new object[9];
            SqlDbType[] Ptype = new SqlDbType[9];

            try
            {
                Pname[0] = "ULDno";
                Pname[1] = "AWBNo";
                Pname[2] = "pcs";
                Pname[3] = "Weight";
                Pname[4] = "FLTNo";
                Pname[5] = "FltDate";
                Pname[6] = "ULDOrigin";
                Pname[7] = "ULDDest";
                Pname[8] = "DUFlag";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.Int;
                Ptype[3] = SqlDbType.Float;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;

                Pvalue[0] = ULDno;
                Pvalue[1] = AWBno;
                Pvalue[2] = pcs;
                Pvalue[3] = weight;
                Pvalue[4] = FlightNo;
                Pvalue[5] = FlightDate;
                Pvalue[6] = Origin;
                Pvalue[7] = Destination;
                Pvalue[8] = UDFlag;

                ds = da.SelectRecords("SPDelUpdateULDBulkAWBAssignDetails", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                if (ds != null)
                    ds.Dispose();

                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }

        #endregion

        #region Finalize ULD Details

        public bool SetFinalizeULD(string ULDno, string FLTNo, string FltDate, string ULDOrigin, string ULDDest, string dtCurrentDate, string Updatedby, string isFinalized, ref string Result)
        {
            SQLServer da = new SQLServer(constr);
            string[] Pname = new string[8];
            object[] Pvalue = new object[8];
            SqlDbType[] Ptype = new SqlDbType[8];

            try
            {
                //DataSet ds = null;
                bool ds = false;

                Pname[0] = "ULDno";
                Pname[1] = "FLTNo";
                Pname[2] = "FltDate";
                Pname[3] = "ULDOrigin";
                Pname[4] = "ULDDest";
                Pname[5] = "dtCurrentDate";
                Pname[6] = "Updatedby";
                Pname[7] = "isFinalized";

                Pvalue[0] = ULDno;
                Pvalue[1] = FLTNo;
                Pvalue[2] = FltDate;
                Pvalue[3] = ULDOrigin;
                Pvalue[4] = ULDDest;
                Pvalue[5] = dtCurrentDate;
                Pvalue[6] = Updatedby;
                Pvalue[7] = isFinalized;

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;

                ds = da.UpdateData("SpFinalizeULDDetails", Pname, Ptype, Pvalue);
                return ds;

            }

            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }
        #endregion

        #region get bulk assigned AWBs
        public DataSet GetBulkAssignedAWBDetails(string POL, string FlightNo, string FlightDate, string AWBPrefix, string AWBNumber, string ULDNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string[] Pname = new string[6];
            object[] Pvalue = new object[6];
            SqlDbType[] Ptype = new SqlDbType[6];

            try
            {
                Pname[0] = "POL";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = POL;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNo;

                Pname[2] = "FltDate";
                Ptype[2] = SqlDbType.VarChar;
                Pvalue[2] = FlightDate;

                Pname[3] = "AWBPrefix";
                Ptype[3] = SqlDbType.NVarChar;
                Pvalue[3] = AWBPrefix;

                Pname[4] = "AWBNumber";
                Ptype[4] = SqlDbType.NVarChar;
                Pvalue[4] = AWBNumber;

                Pname[5] = "ULDNumber";
                Ptype[5] = SqlDbType.NVarChar;
                Pvalue[5] = ULDNumber;

                ds = da.SelectRecords("SPGetBulkAssignedAWBDetails", Pname, Pvalue, Ptype);

                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }
        #endregion get bulk assigned AWBs

        #region Save/Add Bulk Assigned AWB Details
        public DataSet AddBulkAssignedAWBDetails(string AWBPrefix, string AWBno, int PCS, double Weight, string FLTno, string FltDate, string ULDOrigin, string ULDDest, string dtCurrentDate, string Updatedby, string AWBLocation, string AWBLoadingPriority, string AWBBuilderName, ref string Result,string CartNo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[14];
                object[] Pvalue = new object[14];
                SqlDbType[] Ptype = new SqlDbType[14];


                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNo";
                Pname[2] = "PCS";
                Pname[3] = "Weight";
                Pname[4] = "FLTNo";
                Pname[5] = "FltDate";
                Pname[6] = "ULDOrigin";
                Pname[7] = "ULDDest";
                Pname[8] = "dtCurrentDate";
                Pname[9] = "Updatedby";
                Pname[10] = "AWBLocation";
                Pname[11] = "AWBLoadingPriority";
                Pname[12] = "AWBBuilderName";
                Pname[13] = "CartNo";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.Int;
                Ptype[3] = SqlDbType.Float;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.VarChar;

                Pvalue[0] = AWBPrefix;
                Pvalue[1] = AWBno;
                Pvalue[2] = PCS;
                Pvalue[3] = Weight;
                Pvalue[4] = FLTno;
                Pvalue[5] = FltDate;
                Pvalue[6] = ULDOrigin;
                Pvalue[7] = ULDDest;
                Pvalue[8] = dtCurrentDate;
                Pvalue[9] = Updatedby;
                Pvalue[10] = AWBLocation;
                Pvalue[11] = AWBLoadingPriority;
                Pvalue[12] = AWBBuilderName;
                Pvalue[13] = CartNo;


                ds = da.SelectRecords("SPSaveBulkAssignedAWBDetailsNew", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Delete Bulk Assigned AWB Details
        public DataSet DeleteBulkAssignedAWBDetails(string AWBno, int pcs, float weight, string FlightNo, string FlightDate, string Origin, string Destination, string UDFlag, ref string Result)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            string[] Pname = new string[8];
            object[] Pvalue = new object[8];
            SqlDbType[] Ptype = new SqlDbType[8];

            try
            {
                Pname[0] = "AWBNo";
                Pname[1] = "pcs";
                Pname[2] = "Weight";
                Pname[3] = "FLTNo";
                Pname[4] = "FltDate";
                Pname[5] = "ULDOrigin";
                Pname[6] = "ULDDest";
                Pname[7] = "DUFlag";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.Int;
                Ptype[2] = SqlDbType.Float;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;

                Pvalue[0] = AWBno;
                Pvalue[1] = pcs;
                Pvalue[2] = weight;
                Pvalue[3] = FlightNo;
                Pvalue[4] = FlightDate;
                Pvalue[5] = Origin;
                Pvalue[6] = Destination;
                Pvalue[7] = UDFlag;

                ds = da.SelectRecords("SPDelUpdateBulkAssignedAWBDetails", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                if (ds != null)
                    ds.Dispose();

                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }

        #endregion

        #region Save/Add ULD Bulk AWb Details
        public DataSet AddULDBulkAWBDetails(string ULDno, string AWBPrefix, string AWBno, int PCS, double Weight, 
            string FLTno, string FltDate, string PosInFlight, string UldType, string ULDOrigin, string ULDDest, 
            string dtCurrentDate, string Updatedby, string AWBLocation, string AWBLoadingPriority, string ULDLocation, 
            string ULDLoadingPriority, double ScaleWeight, string ContainerType, string AWBBuilderName, 
            string ULDBuilderName, string isReceived, ref string Result, string DollyWt,string OldFltNo,string OldFltDate)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[25];
                object[] Pvalue = new object[25];
                SqlDbType[] Ptype = new SqlDbType[25];


                Pname[0] = "ULDno";
                Pname[1] = "AWBPrefix";
                Pname[2] = "AWBNo";
                Pname[3] = "PCS";
                Pname[4] = "Weight";
                Pname[5] = "FLTNo";
                Pname[6] = "FltDate";
                Pname[7] = "PosInFlight";
                Pname[8] = "UldType";
                Pname[9] = "ULDOrigin";
                Pname[10] = "ULDDest";
                Pname[11] = "dtCurrentDate";
                Pname[12] = "Updatedby";
                Pname[13] = "AWBLocation";
                Pname[14] = "AWBLoadingPriority";
                Pname[15] = "ULDLocation";
                Pname[16] = "ULDLoadingPriority";
                Pname[17] = "ScaleWeight";
                Pname[18] = "ContainerType";
                Pname[19] = "AWBBuilderName";
                Pname[20] = "ULDBuilderName";
                Pname[21] = "isReceived";
                Pname[22] = "DollyWt";
                Pname[23] = "OldFltNo";
                Pname[24] = "OldFltDate";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.Int;
                Ptype[4] = SqlDbType.Float;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.VarChar;
                Ptype[17] = SqlDbType.Float;
                Ptype[18] = SqlDbType.VarChar;
                Ptype[19] = SqlDbType.VarChar;
                Ptype[20] = SqlDbType.VarChar;
                Ptype[21] = SqlDbType.VarChar;
                Ptype[22] = SqlDbType.VarChar;
                Ptype[23] = SqlDbType.VarChar;
                Ptype[24] = SqlDbType.VarChar;

                Pvalue[0] = ULDno;
                Pvalue[1] = AWBPrefix;
                Pvalue[2] = AWBno;
                Pvalue[3] = PCS;
                Pvalue[4] = Weight;
                Pvalue[5] = FLTno;
                Pvalue[6] = FltDate;
                Pvalue[7] = PosInFlight;
                Pvalue[8] = UldType;
                Pvalue[9] = ULDOrigin;
                Pvalue[10] = ULDDest;
                Pvalue[11] = dtCurrentDate;
                Pvalue[12] = Updatedby;
                Pvalue[13] = AWBLocation;
                Pvalue[14] = AWBLoadingPriority;
                Pvalue[15] = ULDLocation;
                Pvalue[16] = ULDLoadingPriority;
                Pvalue[17] = ScaleWeight;
                Pvalue[18] = ContainerType;
                Pvalue[19] = AWBBuilderName;
                Pvalue[20] = ULDBuilderName;
                Pvalue[21] = isReceived;
                Pvalue[22] = DollyWt;
                Pvalue[23] = OldFltNo;
                Pvalue[24] = OldFltDate;

                ds = da.SelectRecords("SPSaveAssignULDBulkAWBDetailsNew", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion Save/Add ULD Bulk AWb Details

        #region Save/Add ULD Bulk AWB Details BACKWARD COMPATIBLE
        public DataSet AddULDBulkAWBDetails(string ULDno, string AWBPrefix, string AWBno, int PCS, double Weight,
            string FLTno, string FltDate, string PosInFlight, string UldType, string ULDOrigin, string ULDDest,
            string dtCurrentDate, string Updatedby, string AWBLocation, string AWBLoadingPriority, string ULDLocation,
            string ULDLoadingPriority, double ScaleWeight, string ContainerType, string AWBBuilderName,
            string ULDBuilderName, string isReceived, ref string Result, string DollyWt)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[23];
                object[] Pvalue = new object[23];
                SqlDbType[] Ptype = new SqlDbType[23];

                Pname[0] = "ULDno";
                Pname[1] = "AWBPrefix";
                Pname[2] = "AWBNo";
                Pname[3] = "PCS";
                Pname[4] = "Weight";
                Pname[5] = "FLTNo";
                Pname[6] = "FltDate";
                Pname[7] = "PosInFlight";
                Pname[8] = "UldType";
                Pname[9] = "ULDOrigin";
                Pname[10] = "ULDDest";
                Pname[11] = "dtCurrentDate";
                Pname[12] = "Updatedby";
                Pname[13] = "AWBLocation";
                Pname[14] = "AWBLoadingPriority";
                Pname[15] = "ULDLocation";
                Pname[16] = "ULDLoadingPriority";
                Pname[17] = "ScaleWeight";
                Pname[18] = "ContainerType";
                Pname[19] = "AWBBuilderName";
                Pname[20] = "ULDBuilderName";
                Pname[21] = "isReceived";
                Pname[22] = "DollyWt";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.Int;
                Ptype[4] = SqlDbType.Float;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.VarChar;
                Ptype[17] = SqlDbType.Float;
                Ptype[18] = SqlDbType.VarChar;
                Ptype[19] = SqlDbType.VarChar;
                Ptype[20] = SqlDbType.VarChar;
                Ptype[21] = SqlDbType.VarChar;
                Ptype[22] = SqlDbType.VarChar;

                Pvalue[0] = ULDno;
                Pvalue[1] = AWBPrefix;
                Pvalue[2] = AWBno;
                Pvalue[3] = PCS;
                Pvalue[4] = Weight;
                Pvalue[5] = FLTno;
                Pvalue[6] = FltDate;
                Pvalue[7] = PosInFlight;
                Pvalue[8] = UldType;
                Pvalue[9] = ULDOrigin;
                Pvalue[10] = ULDDest;
                Pvalue[11] = dtCurrentDate;
                Pvalue[12] = Updatedby;
                Pvalue[13] = AWBLocation;
                Pvalue[14] = AWBLoadingPriority;
                Pvalue[15] = ULDLocation;
                Pvalue[16] = ULDLoadingPriority;
                Pvalue[17] = ScaleWeight;
                Pvalue[18] = ContainerType;
                Pvalue[19] = AWBBuilderName;
                Pvalue[20] = ULDBuilderName;
                Pvalue[21] = isReceived;
                Pvalue[22] = DollyWt;

                ds = da.SelectRecords("SPSaveAssignULDBulkAWBDetailsNew", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion Save/Add ULD Bulk AWb Details

        #region check Valid dest
        public DataSet CheckValidDestination(string ULDno, string FLTno, string FltDate, string ULDOrigin, string ULDDest, ref string Result)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[5];
                object[] Pvalue = new object[5];
                SqlDbType[] Ptype = new SqlDbType[5];


                Pname[0] = "ULDno";
                Pname[1] = "FLTNo";
                Pname[2] = "FltDate";
                Pname[3] = "ULDOrigin";
                Pname[4] = "ULDDest";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                
                Pvalue[0] = ULDno;
                Pvalue[1] = FLTno;
                Pvalue[2] = FltDate;
                Pvalue[3] = ULDOrigin;
                Pvalue[4] = ULDDest;

                ds = da.SelectRecords("SPCheckValidDestination", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion check Valid dest

        # region check Valid ULD For Save
        public string checkValidULDForSave(string ULDno, string FLTno, string FltDate, string ULDOrigin, string ULDDest, 
            string POL,decimal ScaleWt)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                
                string[] Pname = new string[7];
                object[] Pvalue = new object[7];
                SqlDbType[] Ptype = new SqlDbType[7];


                Pname[0] = "ULDno";
                Pname[1] = "FLTNo";
                Pname[2] = "FltDate";
                Pname[3] = "ULDOrigin";
                Pname[4] = "ULDDest";
                Pname[5] = "POL";
                Pname[6] = "ScaleWt";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.Decimal;

                Pvalue[0] = ULDno;
                Pvalue[1] = FLTno;
                Pvalue[2] = FltDate;
                Pvalue[3] = ULDOrigin;
                Pvalue[4] = ULDDest;
                Pvalue[5] = POL;
                Pvalue[6] = ScaleWt;

                string res = db.GetStringByProcedure("SPCheckValidULDForSave", Pname, Pvalue, Ptype);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion check Valid ULD For Save

        # region check Flight status
        public string checkFlightStatus(string FLTno, string FltDate, string POL)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                
                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];


                Pname[0] = "FLTNo";
                Pname[1] = "FltDate";
                Pname[2] = "POL";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;

                Pvalue[0] = FLTno;
                Pvalue[1] = FltDate;
                Pvalue[2] = POL;

                string res = db.GetStringByProcedure("SPCheckFlightStatusFlightPlanning", Pname, Pvalue, Ptype);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion check Flight status

        #region Get ULD list From Location
        public DataSet GetULDListFromLocation(string Location)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string[] Pname = new string[1];
            object[] Pvalue = new object[1];
            SqlDbType[] Ptype = new SqlDbType[1];

            try
            {
                Pname[0] = "POL";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = Location;

                ds = da.SelectRecords("SPGetULDListFromLocation", Pname, Pvalue, Ptype);

                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }
        #endregion Get ULD list From Location

        # region Update ULD Planning Status
        public string UpdateULDPlanningStatus(string ULDno, string POL, string FLTno, string FltDate, string UpdatedOn, string UpdatedBy)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[6];
                object[] Pvalue = new object[6];
                SqlDbType[] Ptype = new SqlDbType[6];


                Pname[0] = "ULDno";
                Pname[1] = "POL";
                Pname[2] = "FLTNo";
                Pname[3] = "FltDate";
                Pname[4] = "dtCurrentDate";
                Pname[5] = "UpdatedBy";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;

                Pvalue[0] = ULDno;
                Pvalue[1] = POL;
                Pvalue[2] = FLTno;
                Pvalue[3] = FltDate;
                Pvalue[4] = UpdatedOn;
                Pvalue[5] = UpdatedBy;

                string res = db.GetStringByProcedure("SPUpdateULDExportStatus", Pname, Pvalue, Ptype);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion Update ULD Planning Status

        # region Update Bulk AWB Planning Status
        public string UpdateBulkAWBPlanningStatus(string AWBNumber, string POL, string FLTno, string FltDate, string UpdatedOn, string UpdatedBy)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[6];
                object[] Pvalue = new object[6];
                SqlDbType[] Ptype = new SqlDbType[6];


                Pname[0] = "AWBNumber";
                Pname[1] = "POL";
                Pname[2] = "FLTNo";
                Pname[3] = "FltDate";
                Pname[4] = "dtCurrentDate";
                Pname[5] = "UpdatedBy";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;

                Pvalue[0] = AWBNumber;
                Pvalue[1] = POL;
                Pvalue[2] = FLTno;
                Pvalue[3] = FltDate;
                Pvalue[4] = UpdatedOn;
                Pvalue[5] = UpdatedBy;

                string res = db.GetStringByProcedure("SPUpdateBulkAWBExportStatus", Pname, Pvalue, Ptype);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion Update Bulk AWB Planning Status

        # region Update AWB Flight to blank
        public string UpdateAWBFlightToBlank(string AWBNumber, string POL, string FLTno, string FltDate, string UpdatedOn, string UpdatedBy)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[6];
                object[] Pvalue = new object[6];
                SqlDbType[] Ptype = new SqlDbType[6];


                Pname[0] = "AWBNumber";
                Pname[1] = "POL";
                Pname[2] = "FLTNo";
                Pname[3] = "FltDate";
                Pname[4] = "dtCurrentDate";
                Pname[5] = "UpdatedBy";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;

                Pvalue[0] = AWBNumber;
                Pvalue[1] = POL;
                Pvalue[2] = FLTno;
                Pvalue[3] = FltDate;
                Pvalue[4] = UpdatedOn;
                Pvalue[5] = UpdatedBy;

                string res = db.GetStringByProcedure("SPUpdateAWBFlightToBlank", Pname, Pvalue, Ptype);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion Update AWB Flight to blank

        #region Getting HAWB Per AWB
        public DataSet GetChildHAWB(string AWBPrefix,string AWBNumber,string FlightNo, string FlightDate)
        {
            try
            {
                string[] QueryName = { "AWBPrefix", "AWBNumber", "FlightNo", "FlightDate" };
                SqlDbType[] QueryType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] QueryValue = { AWBPrefix, AWBNumber, FlightNo, FlightDate };

                DataSet ds = db.SelectRecords("spGetChildHAWB", QueryName, QueryValue, QueryType);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Delete ULD and AWB Details
        public DataSet DeleteULDAndAWBDetails(string ULDno, string AWBno, int pcs, float weight, string FlightNo, string FlightDate, string Origin, string Destination, string UDFlag, ref string Result)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            string[] Pname = new string[9];
            object[] Pvalue = new object[9];
            SqlDbType[] Ptype = new SqlDbType[9];

            try
            {
                Pname[0] = "ULDno";
                Pname[1] = "AWBNo";
                Pname[2] = "pcs";
                Pname[3] = "Weight";
                Pname[4] = "FLTNo";
                Pname[5] = "FltDate";
                Pname[6] = "ULDOrigin";
                Pname[7] = "ULDDest";
                Pname[8] = "DUFlag";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.Int;
                Ptype[3] = SqlDbType.Float;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;

                Pvalue[0] = ULDno;
                Pvalue[1] = AWBno;
                Pvalue[2] = pcs;
                Pvalue[3] = weight;
                Pvalue[4] = FlightNo;
                Pvalue[5] = FlightDate;
                Pvalue[6] = Origin;
                Pvalue[7] = Destination;
                Pvalue[8] = UDFlag;

                ds = da.SelectRecords("SPDelULDAWBAssignDetails", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                if (ds != null)
                    ds.Dispose();

                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }

        #endregion

        #region get bulk assigned AWBs from Cart
        public DataSet GetBulkAssignedAWBDetailsFromCart(string POL, string FlightNo, string FlightDate, string AWBPrefix, string AWBNumber, string CartNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string[] Pname = new string[6];
            object[] Pvalue = new object[6];
            SqlDbType[] Ptype = new SqlDbType[6];

            try
            {
                Pname[0] = "POL";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = POL;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNo;

                Pname[2] = "FltDate";
                Ptype[2] = SqlDbType.VarChar;
                Pvalue[2] = FlightDate;

                Pname[3] = "AWBPrefix";
                Ptype[3] = SqlDbType.NVarChar;
                Pvalue[3] = AWBPrefix;

                Pname[4] = "AWBNumber";
                Ptype[4] = SqlDbType.NVarChar;
                Pvalue[4] = AWBNumber;

                Pname[5] = "CartNumber";
                Ptype[5] = SqlDbType.NVarChar;
                Pvalue[5] = CartNumber;

                ds = da.SelectRecords("SPGetBulkAssignedAWBDetailsFromCart", Pname, Pvalue, Ptype);

                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }
        #endregion get bulk assigned AWBs from Cart

        #region Save/Add Bulk Assigned AWB Details from Cart
        public DataSet AddBulkAssignedAWBDetailsFromCart(string AWBPrefix, string AWBno, int PCS, double Weight, 
            string FLTno, string FltDate, string ULDOrigin, string ULDDest, string dtCurrentDate, string Updatedby, 
            string AWBLocation, string AWBLoadingPriority, string AWBBuilderName, ref string Result, string CartNo, 
            string CartLoadingPriority, string CartBuilderName, double ScaleWeight,string OldFltNo, string OldFltDate)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[19];
                object[] Pvalue = new object[19];
                SqlDbType[] Ptype = new SqlDbType[19];

                Pname[0] = "AWBPrefix";
                Pname[1] = "AWBNo";
                Pname[2] = "PCS";
                Pname[3] = "Weight";
                Pname[4] = "FLTNo";
                Pname[5] = "FltDate";
                Pname[6] = "ULDOrigin";
                Pname[7] = "ULDDest";
                Pname[8] = "dtCurrentDate";
                Pname[9] = "Updatedby";
                Pname[10] = "AWBLocation";
                Pname[11] = "AWBLoadingPriority";
                Pname[12] = "AWBBuilderName";
                Pname[13] = "CartNo";
                Pname[14] = "CartLoadingPriority";
                Pname[15] = "CartBuilderName";
                Pname[16] = "ScaleWeight";
                Pname[17] = "OldFltNo";
                Pname[18] = "OldFltDate";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.Int;
                Ptype[3] = SqlDbType.Float;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.Float;
                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.VarChar;

                Pvalue[0] = AWBPrefix;
                Pvalue[1] = AWBno;
                Pvalue[2] = PCS;
                Pvalue[3] = Weight;
                Pvalue[4] = FLTno;
                Pvalue[5] = FltDate;
                Pvalue[6] = ULDOrigin;
                Pvalue[7] = ULDDest;
                Pvalue[8] = dtCurrentDate;
                Pvalue[9] = Updatedby;
                Pvalue[10] = AWBLocation;
                Pvalue[11] = AWBLoadingPriority;
                Pvalue[12] = AWBBuilderName;
                Pvalue[13] = CartNo;
                Pvalue[14] = CartLoadingPriority;
                Pvalue[15] = CartBuilderName;
                Pvalue[16] = ScaleWeight;
                Pvalue[17] = OldFltNo;
                Pvalue[18] = OldFltDate;

                ds = da.SelectRecords("SPSaveBulkAssignedAWBDetailsFromCart", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Save/Add Cart Details
        public DataSet AddCartDetails(string Cartno, string FLTno, string FltDate, string POL, string dtCurrentDate, 
            string Updatedby, string CartLoadingPriority, string CartBuilderName, double ScaleWeight,
            ref string Result, string NewCartNo)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            string[] Pname = new string[10];
            object[] Pvalue = new object[10];
            SqlDbType[] Ptype = new SqlDbType[10];

            try
            {
                Pname[0] = "Cartno";
                Pname[1] = "FLTno";
                Pname[2] = "FltDate";
                Pname[3] = "POL";
                Pname[4] = "dtCurrentDate";
                Pname[5] = "Updatedby";
                Pname[6] = "CartLoadingPriority";
                Pname[7] = "CartBuilderName";
                Pname[8] = "ScaleWeight";
                Pname[9] = "NewCartNo";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.Float;
                Ptype[9] = SqlDbType.VarChar;

                Pvalue[0] = Cartno;
                Pvalue[1] = FLTno;
                Pvalue[2] = FltDate;
                Pvalue[3] = POL;
                Pvalue[4] = dtCurrentDate;
                Pvalue[5] = Updatedby;
                Pvalue[6] = CartLoadingPriority;
                Pvalue[7] = CartBuilderName;
                Pvalue[8] = ScaleWeight;
                Pvalue[9] = NewCartNo;

                ds = da.SelectRecords("SPSaveAssignCartDetails", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }

        #endregion

        #region Delete Bulk Assigned AWB Details from Cart
        public DataSet DeleteBulkAssignedAWBDetailsFromCart(string AWBno, int pcs, float weight, string FlightNo, string FlightDate, string Origin, string Destination, string UDFlag, string CartNumber, ref string Result)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            string[] Pname = new string[9];
            object[] Pvalue = new object[9];
            SqlDbType[] Ptype = new SqlDbType[9];

            try
            {
                Pname[0] = "AWBNo";
                Pname[1] = "pcs";
                Pname[2] = "Weight";
                Pname[3] = "FLTNo";
                Pname[4] = "FltDate";
                Pname[5] = "ULDOrigin";
                Pname[6] = "ULDDest";
                Pname[7] = "DUFlag";
                Pname[8] = "CartNo";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.Int;
                Ptype[2] = SqlDbType.Float;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;

                Pvalue[0] = AWBno;
                Pvalue[1] = pcs;
                Pvalue[2] = weight;
                Pvalue[3] = FlightNo;
                Pvalue[4] = FlightDate;
                Pvalue[5] = Origin;
                Pvalue[6] = Destination;
                Pvalue[7] = UDFlag;
                Pvalue[8] = CartNumber;

                ds = da.SelectRecords("SPDelUpdateBulkAssignedAWBDetailsFromCart", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                if (ds != null)
                    ds.Dispose();

                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }

        #endregion

        #region Delete Bulk Assigned AWB Details And Cart
        public DataSet DeleteBulkAssignedAWBDetailsAndCart(string AWBno, int pcs, float weight, string FlightNo, string FlightDate, string Origin, string Destination, string UDFlag, string CartNumber, ref string Result)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            string[] Pname = new string[9];
            object[] Pvalue = new object[9];
            SqlDbType[] Ptype = new SqlDbType[9];

            try
            {
                Pname[0] = "AWBNo";
                Pname[1] = "pcs";
                Pname[2] = "Weight";
                Pname[3] = "FLTNo";
                Pname[4] = "FltDate";
                Pname[5] = "ULDOrigin";
                Pname[6] = "ULDDest";
                Pname[7] = "DUFlag";
                Pname[8] = "CartNo";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.Int;
                Ptype[2] = SqlDbType.Float;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;

                Pvalue[0] = AWBno;
                Pvalue[1] = pcs;
                Pvalue[2] = weight;
                Pvalue[3] = FlightNo;
                Pvalue[4] = FlightDate;
                Pvalue[5] = Origin;
                Pvalue[6] = Destination;
                Pvalue[7] = UDFlag;
                Pvalue[8] = CartNumber;

                ds = da.SelectRecords("SPDelBulkAssignedAWBDetailsAndCart", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                if (ds != null)
                    ds.Dispose();

                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }

        #endregion

        #region Delete Cart
        public DataSet DeleteCart(string CartNumber, string FlightNo, string FlightDate, string POL, string Updatedby, string Updatedon, ref string Result)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            string[] Pname = new string[6];
            object[] Pvalue = new object[6];
            SqlDbType[] Ptype = new SqlDbType[6];

            try
            {
                Pname[0] = "CartNo";
                Pname[1] = "FLTNo";
                Pname[2] = "FltDate";
                Pname[3] = "POL";
                Pname[4] = "UpdatedBy";
                Pname[5] = "UpdatedOn";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;

                Pvalue[0] = CartNumber;
                Pvalue[1] = FlightNo;
                Pvalue[2] = FlightDate;
                Pvalue[3] = POL;
                Pvalue[4] = Updatedby;
                Pvalue[5] = Updatedon;


                ds = da.SelectRecords("SPDeleteCartFromOperationCartDetails", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                if (ds != null)
                    ds.Dispose();

                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }

        #endregion

        # region check Cart Valid For Save
        public string checkCartValidForSave(string Cartno, string FLTno, string FltDate, string POL)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[4];
                object[] Pvalue = new object[4];
                SqlDbType[] Ptype = new SqlDbType[4];


                Pname[0] = "Cartno";
                Pname[1] = "FLTNo";
                Pname[2] = "FltDate";
                Pname[3] = "POL";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;

                Pvalue[0] = Cartno;
                Pvalue[1] = FLTno;
                Pvalue[2] = FltDate;
                Pvalue[3] = POL;

                string res = db.GetStringByProcedure("SPCheckCartValidForSave", Pname, Pvalue, Ptype);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion check Cart Valid For Save

        # region Update Cart Planning Status
        public string UpdateCartPlanningStatus(string Cartno, string POL, string FLTno, string FltDate, string UpdatedOn, string UpdatedBy)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[6];
                object[] Pvalue = new object[6];
                SqlDbType[] Ptype = new SqlDbType[6];


                Pname[0] = "Cartno";
                Pname[1] = "POL";
                Pname[2] = "FLTNo";
                Pname[3] = "FltDate";
                Pname[4] = "dtCurrentDate";
                Pname[5] = "UpdatedBy";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;

                Pvalue[0] = Cartno;
                Pvalue[1] = POL;
                Pvalue[2] = FLTno;
                Pvalue[3] = FltDate;
                Pvalue[4] = UpdatedOn;
                Pvalue[5] = UpdatedBy;

                string res = db.GetStringByProcedure("SPUpdateCartExportStatus", Pname, Pvalue, Ptype);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion Update ULD Planning Status

        # region check Flight status
        public string checkFlightExportStatus(string FLTno, string FltDate, string POL)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];


                Pname[0] = "FLTNo";
                Pname[1] = "FltDate";
                Pname[2] = "POL";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;

                Pvalue[0] = FLTno;
                Pvalue[1] = FltDate;
                Pvalue[2] = POL;

                string res = db.GetStringByProcedure("SPCheckFlightExportStatus", Pname, Pvalue, Ptype);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion check Flight status

        #region Getting  info
        public DataSet GetHAWBInfo(string AWBPrefix, string AWBNumber, String ULDNo, String FlightNo)
        {
            try
            {
                string[] QueryName = { "AWBPrefix", "AWBNumber", "ULDNo" ,"FlightNo"};
                SqlDbType[] QueryType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] QueryValue = { AWBPrefix, AWBNumber, ULDNo, FlightNo };

                DataSet ds = db.SelectRecords("SP_getOperationAWBDetails", QueryName, QueryValue, QueryType);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region ValidateULD Advanced
        public string ValidateULDAdvanced(string ULDNumber)
        {
            try
            {
                string Result = string.Empty;
                DataSet ds = db.SelectRecords("spValidateULDAdvanced", "ULDNumber", ULDNumber, SqlDbType.VarChar);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0]["Result"].ToString();
                }
                return Result;

            }
            catch (Exception ex)
            { return ex.Message; }
        }
        #endregion

        #region Get ULD list From Location
        public DataSet GetULDListFromLocation(string Location,string ULDType,string ULDStatus,string ULDUseStatus)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string[] Pname = new string[4];
            object[] Pvalue = new object[4];
            SqlDbType[] Ptype = new SqlDbType[4];

            try
            {
                Pname[0] = "POL";
                Pname[1] = "ULDType";
                Pname[2] = "ULDStatus";
                Pname[3] = "ULDUseStatus";
                Ptype[0] = SqlDbType.NVarChar;
                Ptype[1] = SqlDbType.NVarChar;
                Ptype[2] = SqlDbType.NVarChar;
                Ptype[3] = SqlDbType.NVarChar;
                Pvalue[0] = Location;
                Pvalue[1] = ULDType;
                Pvalue[2] = ULDStatus;
                Pvalue[3] = ULDUseStatus;

                ds = da.SelectRecords("SPGetULDListFromLocationTracking", Pname, Pvalue, Ptype);

                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }
        #endregion Get ULD list From Location

        #region GetAWBForReassign
        public DataSet GetAWBForReassign(string AWBPrefix, string AWBNumber, string FlightNo, string FlightDate, string Station)
        {
            DataSet dsReassignAWBs = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBPrefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBPrefix, i);
                i++;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBNumber, i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightNo, i);
                i++;

                ColumnNames.SetValue("FlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightDate, i);
                i++;

                ColumnNames.SetValue("Station", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Station, i);

                dsReassignAWBs = da.SelectRecords("spGetAWBInfoForReassignment", ColumnNames, Values, DataType);
                //Console.WriteLine(db.LastErrorDescription);
                return (dsReassignAWBs);

            }
            catch (Exception)
            {
                return (null);
            }
            finally
            {
                if (dsReassignAWBs != null)
                {
                    dsReassignAWBs.Dispose();
                }
            }
        }
        #endregion GetAWBForReassign

        #region Reassign Fetched AWB
        public DataSet ReassignFetchedAWB(string AWBPrefix, string AWBNumber, string OldFlightNo, string OldFlightDate,
            string NewFlightNo, string NewFlightDate, int OldPcs, decimal OldWt, int NewPcs, decimal NewWt, string Station,
            DateTime UpdatedOn, string UpdatedBy, string PlanningStatus, string CartNumber, string ULDNumber, string POU)
        {
            DataSet dsReassignAWBs = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[17];
                SqlDbType[] DataType = new SqlDbType[17];
                Object[] Values = new object[17];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBPrefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBPrefix, i);
                i++;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBNumber, i);
                i++;

                ColumnNames.SetValue("OldFlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(OldFlightNo, i);
                i++;

                ColumnNames.SetValue("OldFlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(OldFlightDate, i);
                i++;

                ColumnNames.SetValue("NewFlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(NewFlightNo, i);
                i++;

                ColumnNames.SetValue("NewFlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(NewFlightDate, i);
                i++;

                ColumnNames.SetValue("OldPcs", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(OldPcs, i);
                i++;

                ColumnNames.SetValue("OldWt", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(OldWt, i);
                i++;

                ColumnNames.SetValue("NewPcs", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(NewPcs, i);
                i++;

                ColumnNames.SetValue("NewWt", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(NewWt, i);
                i++;

                ColumnNames.SetValue("Station", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Station, i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(UpdatedOn, i);
                i++;

                ColumnNames.SetValue("UpdatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdatedBy, i);
                i++;

                ColumnNames.SetValue("PlanningStatus", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PlanningStatus, i);
                i++;

                ColumnNames.SetValue("CartNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CartNumber, i);
                i++;

                ColumnNames.SetValue("ULDNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ULDNumber, i);
                i++;

                ColumnNames.SetValue("FlightDest", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(POU, i);
                i++;

                dsReassignAWBs = da.SelectRecords("spReassignFetchedAWB", ColumnNames, Values, DataType);
                //Console.WriteLine(db.LastErrorDescription);
                return (dsReassignAWBs);

            }
            catch (Exception)
            {
                return (null);
            }
            finally
            {
                if (dsReassignAWBs != null)
                {
                    dsReassignAWBs.Dispose();
                }
            }
        }
        #endregion GetAWBForReassign

        #region Get Flight Planning Status
        public string GetFlightPlanningStatus(string FLTno, string FltDate, string POL)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];


                Pname[0] = "FLTNo";
                Pname[1] = "FltDate";
                Pname[2] = "POL";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;

                Pvalue[0] = FLTno;
                Pvalue[1] = FltDate;
                Pvalue[2] = POL;

                string res = db.GetStringByProcedure("SPGetFlightPlanningStatus", Pname, Pvalue, Ptype);
                return res;

            }
            catch (Exception)
            {
                return "N";
            }
        }
        #endregion Get Flight Planning Status

    }
}
