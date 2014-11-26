using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BLBuildULD
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        #region Get ULD numbers
        public DataSet GetULDNumbers(string POL)
        {

            //Get agent codes...
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            try
            {
                string[] Pname = new string[1];
                object[] Pvalue = new object[1];
                SqlDbType[] Ptype = new SqlDbType[1];

                Pname[0] = "POL";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = POL;

                ds = da.SelectRecords("SPGetULDNumbers", Pname, Pvalue, Ptype);

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
                return ds;
            }

            return (null);
        }
        #endregion Get ULD numbers

        #region Tab AWB Details
        public DataSet GetAWBLoadPlanULDDetails(string POL, string FlightNo, string FlightDate, string AWBPrefix, string AWBNumber, string ULDNumber )
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

                ds = da.SelectRecords("SPGetAWBLoadPlanULDDetailsNew", Pname, Pvalue, Ptype);


                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                da = null;
                ds = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }

        }
        #endregion Tab AWB Details


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

        #region Save/Add ULD AWb Details
        public DataSet AddULDAWBDetails(string ULDno, string AWBPrefix, string AWBno, int PCS, double Weight, string FLTno, string FltDate, string PosInFlight, string UldType, string ULDOrigin, string ULDDest, string dtCurrentDate, string Updatedby, string Location, string LoadingPriority, string ULDLocation, string ULDLoadingPriority, string AWBBuilderName, string ULDBuilderName, double ULDScaleWeight, double ULDDollyWeight, string isReceived, ref string Result)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[22];
                object[] Pvalue = new object[22];
                SqlDbType[] Ptype = new SqlDbType[22];


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
                Pname[17] = "AWBBuilderName";
                Pname[18] = "ULDBuilderName";
                Pname[19] = "ScaleWeight";
                Pname[20] = "DollyWeight";
                Pname[21] = "isReceived";

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
                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.VarChar;
                Ptype[19] = SqlDbType.Float;
                Ptype[20] = SqlDbType.Float;
                Ptype[21] = SqlDbType.VarChar;

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
                Pvalue[13] = Location;
                Pvalue[14] = LoadingPriority;
                Pvalue[15] = ULDLocation;
                Pvalue[16] = ULDLoadingPriority;
                Pvalue[17] = AWBBuilderName;
                Pvalue[18] = ULDBuilderName;
                Pvalue[19] = ULDScaleWeight;
                Pvalue[20] = ULDDollyWeight;
                Pvalue[21] = isReceived;

                ds = da.SelectRecords("SPSaveULDAWBDetailsNew", Pname, Pvalue, Ptype);

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


        #region Delete ULD AWb Details
        public bool DeleteULDAWBDetails(string ULDno, string AWBno, int pcs, float weight, string FlightNo, string FlightDate, string Origin, string Destination, string UDFlag, ref string Result)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[9];
                object[] Pvalue = new object[9];
                SqlDbType[] Ptype = new SqlDbType[9];


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

                ds = da.SelectRecords("SPDelUpdateULDAWBDetails", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    ds = null;
                    return false;
                }

                ds = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Save/Add received ULD Details
        public DataSet AddReceivedULDDetails(string ULDno, string AWBPrefix, string AWBno, string FLTno, string FltDate, string isReceived, string dtCurrentDate, string Updatedby, ref string Result)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[8];
                object[] Pvalue = new object[8];
                SqlDbType[] Ptype = new SqlDbType[8];


                Pname[0] = "ULDno";
                Pname[1] = "AWBPrefix";
                Pname[2] = "AWBNo";
                Pname[3] = "FLTNo";
                Pname[4] = "FltDate";
                Pname[5] = "isReceived";
                Pname[6] = "dtCurrentDate";
                Pname[7] = "Updatedby";
              

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.Bit;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;


                Pvalue[0] = ULDno;
                Pvalue[1] = AWBPrefix;
                Pvalue[2] = AWBno;
                Pvalue[3] = FLTno;
                Pvalue[4] = FltDate;
                Pvalue[5] = isReceived;
                Pvalue[6] = dtCurrentDate;
                Pvalue[7] = Updatedby;


                ds = da.SelectRecords("SPSaveReceivedULDDetails", Pname, Pvalue, Ptype);

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

        #region Validate ULD
        public bool CheckULDValidity(string ULDNumber)
        {
            if (ULDNumber.Length >= 5)
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

        #region get ULD Details
        public DataSet GetULDDetails(string POL, string FlightNo, string FlightDate, string ULDNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string[] Pname = new string[4];
            object[] Pvalue = new object[4];
            SqlDbType[] Ptype = new SqlDbType[4];

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

                Pname[3] = "ULDNumber";
                Ptype[3] = SqlDbType.NVarChar;
                Pvalue[3] = ULDNumber;

                ds = da.SelectRecords("SPGetBuildULDDetails", Pname, Pvalue, Ptype);


                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                da = null;
                ds = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }

        }
        #endregion Tab AWB Details

    }
}
