using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;

namespace ProjectSmartCargoManager
{

    public class BLExpManifest
    {
        SQLServer  db = new SQLServer("");
       
        #region Variables
        string constr = "";
        #endregion Variables

        #region Constructor
        public BLExpManifest()
        {
           constr = Global.GetConnectionString();
        }
        #endregion Constructor

        #region Tab AWB Details

        public   DataSet GetAwbTabdetails( string POL,string FlightNo, DateTime FlightDate)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds=new DataSet ();
            string[] Pname = new string[3];
            object[] Pvalue = new object[3];
            SqlDbType[] Ptype = new SqlDbType[3];

            try
            {
                Pname[0] = "POL";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = POL;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNo;

                Pname[2] = "FltDate";
                Ptype[2] = SqlDbType.DateTime;
                Pvalue[2] = FlightDate;

                ds = da.SelectRecords("SPExpManiGetTabAWBdata", Pname, Pvalue, Ptype);
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
                da = null;
            }
        
        }

        public DataSet GetAwbTabdetails_GHA(string POL, string FlightNo, DateTime FlightDate,string AWBPrefix, string AWBNumber, string ULDNo)
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
                Ptype[2] = SqlDbType.DateTime;
                Pvalue[2] = FlightDate;

                Pname[3] = "AWBPrefix";
                Ptype[3] = SqlDbType.VarChar;
                Pvalue[3] = AWBPrefix;

                Pname[4] = "AWBNumber";
                Ptype[4] = SqlDbType.VarChar;
                Pvalue[4] = AWBNumber;

                Pname[5] = "ULDNo";
                Ptype[5] = SqlDbType.VarChar;
                Pvalue[5] = ULDNo;

                ds = da.SelectRecords("SPExpManiGetTabAWBdata_GHA_V1", Pname, Pvalue, Ptype);

                return ds;

            }
            catch (Exception ex)
            {
                return ds;
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

        public DataSet GetAwbTabdetails_GHA(string POL, string FlightNo, DateTime FlightDate, string AWBPrefix, string AWBNumber, string ULDNo, string POU)
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
                Ptype[2] = SqlDbType.DateTime;
                Pvalue[2] = FlightDate;

                Pname[3] = "AWBPrefix";
                Ptype[3] = SqlDbType.VarChar;
                Pvalue[3] = AWBPrefix;

                Pname[4] = "AWBNumber";
                Ptype[4] = SqlDbType.VarChar;
                Pvalue[4] = AWBNumber;

                Pname[5] = "ULDNo";
                Ptype[5] = SqlDbType.VarChar;
                Pvalue[5] = ULDNo;

                Pname[5] = "POU";
                Ptype[5] = SqlDbType.VarChar;
                Pvalue[5] = POU;

                ds = da.SelectRecords("SPExpManiGetTabAWBdata_GHA_V1", Pname, Pvalue, Ptype);

                return ds;

            }
            catch (Exception ex)
            {
                return ds;
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

        #endregion Tab AWB Details
        
        #region Tab flight Details

        public DataSet GetFlightTabdetails(string Origin, string FlightNo, DateTime dtFlightDate)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string[] Pname = new string[3];
            object[] Pvalue = new object[3];
            SqlDbType[] Ptype = new SqlDbType[3];

            try
            {

                Pname[0] = "Origin";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = Origin;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNo;

                Pname[2] = "FlightDate";
                Ptype[2] = SqlDbType.DateTime;
                Pvalue[2] = dtFlightDate;

                ds = da.SelectRecords("SPExpManiGetRouteDetails", Pname, Pvalue, Ptype);

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
        #endregion Tab Flight Details


        #region Tab ULD Details

        public DataSet GetULDTabdetails(string POU, string FlightNo)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            try
            {

                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];


                Pname[0] = "POU";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = POU;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNo;


                ds = da.SelectRecords("SPExpManiGetTabULDdata", Pname, Pvalue, Ptype);


                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion Tab ULD Details

        #region Save/Add ULD AWb Association
        public bool AddULDAWBassocition(string FLTno, string POL, string POU, string ULDno, Double  ULDwgt, string AWBno, int PCS, double  WGT,
            int AVLPCS, double AVLWGT, string Updatedby, DateTime Updatedon, DateTime FltDate, string Identifier,ref string Result,
            string AWBPrefix, string FltFlag, string CartNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;

            string[] Pname = new string[17];
            object[] Pvalue = new object[17];
            SqlDbType[] Ptype = new SqlDbType[17];

            try
            {
                Pname[0] = "FLTno";
                Pname[1] = "POL";
                Pname[2] = "POU";
                Pname[3] = "ULDno";
                Pname[4] = "ULDwgt";
                Pname[5] = "AWBno";
                Pname[6] = "PCS";
                Pname[7] = "WGT";
                Pname[8] = "AVLPCS";
                Pname[9] = "AVLWGT";
                Pname[10] = "Updatedby";
                Pname[11] = "Updatedon";
                Pname[12] = "FltDate";
                Pname[13] = "Identifier";
                Pname[14] = "AWBPrefix";
                Pname[15] = "FltFlag";
                Pname[16] = "CartNumber";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.Float;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.Int;
                Ptype[7] = SqlDbType.Float;
                Ptype[8] = SqlDbType.Int;
                Ptype[9] = SqlDbType.Float;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.DateTime;
                Ptype[12] = SqlDbType.DateTime;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.NVarChar;
                Ptype[16] = SqlDbType.VarChar;

                Pvalue[0] = FLTno;
                Pvalue[1] = POL;
                Pvalue[2] = POU;
                Pvalue[3] = ULDno;
                Pvalue[4] = ULDwgt;
                Pvalue[5] = AWBno;
                Pvalue[6] = PCS;
                Pvalue[7] = WGT;
                Pvalue[8] = AVLPCS;
                Pvalue[9] = AVLWGT;
                Pvalue[10] = Updatedby;
                Pvalue[11] = Updatedon;
                Pvalue[12] = FltDate;
                Pvalue[13] = Identifier;
                Pvalue[14] = AWBPrefix;
                Pvalue[15] = FltFlag;
                Pvalue[16] = CartNumber;

                ds = da.SelectRecords("SPExpManiSaveULDAWBassociation", Pname, Pvalue, Ptype);

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
                
        #region Tab Get ULDAWB Association Details

        public DataSet GetULDAWBassocitionData(string SelectedULDnos)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            //Database db = new Database();
            string[] Pname = new string[1];
            object[] Pvalue = new object[1];
            SqlDbType[] Ptype = new SqlDbType[1];

            try
            {
                Pname[0] = "SelectedULDnos";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = SelectedULDnos;

                ds = da.SelectRecords("SPExpManiGetULDAWBassociationdata", Pname, Pvalue, Ptype);            
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
                da = null;
            }

        }
        #endregion Tab ULDAWB Association Details

        #region Tab Get ULD Details

        public DataSet GetULDDetails(string SelectedULDnos)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            //Database db = new Database();
            string[] Pname = new string[1];
            object[] Pvalue = new object[1];
            SqlDbType[] Ptype = new SqlDbType[1];

            try
            {
                Pname[0] = "SelectedULDnos";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = SelectedULDnos;

                ds = da.SelectRecords("SPGetULDDetails", Pname, Pvalue, Ptype);
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
                da = null;
            }

        }
        #endregion Tab ULD Details



        #region Get DDL Ver,ULD,data  for Flight and date

        public DataSet GetDDLVerULDPOUPOLdata(string FLTno, DateTime ManifestdateFrom, string ManifestdateTo, string DepartureAirport)
        {

            SQLServer da = new SQLServer(constr);
            DataSet Ds = new DataSet();
           // Database db = new Database();

            try
            {
                string[] Pname = new string[4];
                object[] Pvalue = new object[4];
                SqlDbType[] Ptype = new SqlDbType[4];


                Pname[0] = "FltNo";
                Pname[1] = "ManifestdateFrom";
                Pname[2] = "ManifestdateTo";
                Pname[3] = "DepartureAirport";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.DateTime;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;

                Pvalue[0] = FLTno;
                Pvalue[1] = ManifestdateFrom;
                Pvalue[2] = ManifestdateTo;
                //Pvalue[2] = ManifestdateTo;
                Pvalue[3] = DepartureAirport;


                Ds = da.SelectRecords("SPExpManiGetVerULDPOUPOLddlData", Pname, Pvalue, Ptype);


                return Ds;
            }

            catch (Exception ex)
            {
                return Ds;
            }

        }
        #endregion Get DDL Ver,ULD,data  for Flight and date

        //
        #region Get POU for flight

        public DataSet GetPOUAirlineSchedule(string FLTiD, string Source, DateTime dtFlightDate)
        {

            SQLServer da = new SQLServer(constr);
            DataSet Ds = new DataSet();
            string[] Pname = new string[3];
            object[] Pvalue = new object[3];
            SqlDbType[] Ptype = new SqlDbType[3];
            // Database db = new Database();

            try
            {
                Pname[0] = "FlightID";
                Pname[1] = "Source";
                Pname[2] = "FlightDate";


                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.DateTime;


                Pvalue[0] = FLTiD;
                Pvalue[1] = Source;
                Pvalue[2] = dtFlightDate;

                Ds = da.SelectRecords("spExpManiGetAirlineSch1", Pname, Pvalue, Ptype);

                if (Ds == null || Ds.Tables.Count < 1 || Ds.Tables[0].Rows.Count < 1)
                    Ds = da.SelectRecords("spExpManiGetAirlineSch2", Pname, Pvalue, Ptype);

                return Ds;
            }

            catch (Exception ex)
            {
                if (Ds != null)
                    Ds.Dispose();

                return null;
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
                da = null;
            }

        }
        #endregion Get POU for flight


        #region Get DGR Details

        public DataSet GetDGRDetails(string AWBNo)
        {

            SQLServer da = new SQLServer(constr);
            DataSet Ds = new DataSet();
            // Database db = new Database();

            try
            {
                string[] Pname = new string[1];
                object[] Pvalue = new object[1];
                SqlDbType[] Ptype = new SqlDbType[1];
          
                Pname[0] = "AWBNumber";
       
                Ptype[0] = SqlDbType.VarChar;
               
                Pvalue[0] = AWBNo;

                Ds = da.SelectRecords("GetDGRPiecesWeight", Pname, Pvalue, Ptype);
                
                return Ds;
            }

            catch (Exception ex)
            {
                return Ds;
            }

        }
        #endregion Get DGR details

        #region Get DGR Cargo Details

        public DataSet GetDGRCargoDetails(string AWBNo)
        {
            SQLServer da = new SQLServer(constr);
            DataSet Ds = new DataSet();
            string[] Pname = new string[1];
            object[] Pvalue = new object[1];
            SqlDbType[] Ptype = new SqlDbType[1];

            try
            {
                Pname[0] = "AWBNumber";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = AWBNo;

                Ds = da.SelectRecords("SpGetDGRCargo", Pname, Pvalue, Ptype);

                return Ds;
            }

            catch (Exception ex)
            {
                return Ds;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }
        #endregion Get DGR details

        #region Save Manifest Data
        public bool SaveManifestdata(string FLTno, string ULDno, string POU, string POL, string ULDDest, string counter, string AWBno, string SCC,
            double VOL, int PCS, double WGT, int AVLPCS, double AVLWGT, string Desc, string LoadingPriority, string Remark, string Updatedby, 
            string Updatedon, bool IsManifested, DateTime dtFLTDate, string CurrLoc, string AWBOrigin, Int64 IrregularityID,string IsBonded,string tailNo,
            string IMEINo,string AWBPrefix,string Location, string CartNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();

            string[] Pname = new string[28];
            object[] Pvalue = new object[28];
            SqlDbType[] Ptype = new SqlDbType[28];
            try
            {
                Pname[0] = "FLTno";
                Pname[1] = "ULDno";
                Pname[2] = "POL";
                Pname[3] = "POU";
                Pname[4] = "ULDDest";
                Pname[5] = "counter";
                Pname[6] = "AWBno";
                Pname[7] = "SCC";
                Pname[8] = "VOL";
                Pname[9] = "PCS";
                Pname[10] = "WGT";
                Pname[11] = "AVLPCS";
                Pname[12] = "AVLWGT";
                Pname[13] = "Desc";
                Pname[14] = "LoadingPriority";
                Pname[15] = "Remark";
                Pname[16] = "Updatedby";
                Pname[17] = "Updatedon";
                Pname[18] = "IsManifested";
                Pname[19] = "FLTDate";
                Pname[20] = "AWBOrigin";
                Pname[21] = "IrregularityID";
                Pname[22] = "IsBonded";
                Pname[23] = "TailNo";
                Pname[24] = "IMENO";
                Pname[25] = "AWBPrefix";
                Pname[26] = "Location";
                Pname[27] = "CartNumber";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.Float;
                Ptype[9] = SqlDbType.Int;
                Ptype[10] = SqlDbType.Float;
                Ptype[11] = SqlDbType.Int;
                Ptype[12] = SqlDbType.Float;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.VarChar;
                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.Bit;
                Ptype[19] = SqlDbType.DateTime;
                Ptype[20] = SqlDbType.VarChar;
                Ptype[21] = SqlDbType.BigInt;
                Ptype[22] = SqlDbType.VarChar;
                Ptype[23] = SqlDbType.VarChar;
                Ptype[24] = SqlDbType.VarChar;
                Ptype[25] = SqlDbType.VarChar;
                Ptype[26] = SqlDbType.VarChar;
                Ptype[27] = SqlDbType.VarChar;

                Pvalue[0] = FLTno;
                Pvalue[1] = ULDno;
                Pvalue[2] = POL;
                Pvalue[3] = POU;
                Pvalue[4] = ULDDest;
                Pvalue[5] = counter;
                Pvalue[6] = AWBno;
                Pvalue[7] = SCC;
                Pvalue[8] = VOL;
                Pvalue[9] = PCS;
                Pvalue[10] = WGT;
                Pvalue[11] = AVLPCS;
                Pvalue[12] = AVLWGT;
                Pvalue[13] = Desc;
                Pvalue[14] = LoadingPriority;
                Pvalue[15] = Remark;
                Pvalue[16] = Updatedby;
                Pvalue[17] = Updatedon;
                Pvalue[18] = IsManifested;
                Pvalue[19] = dtFLTDate;
                Pvalue[20] = AWBOrigin;
                Pvalue[21] = IrregularityID;
                Pvalue[22] = IsBonded;
                Pvalue[23] = tailNo;
                Pvalue[24] = IMEINo;
                Pvalue[25] = AWBPrefix;
                Pvalue[26] = Location;
                Pvalue[27] = CartNumber;

                bool res = da.InsertData("SPExpManiSaveManifest", Pname, Ptype, Pvalue);

                return res;
            }
            catch (Exception ex)
            {
                return false;
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

        #endregion Save Manifest Data


        #region Get Manifest Details for Flight and date

        public DataSet GetManifestDetails(string FLTno, DateTime ManifestdateFrom, string ManifestdateTo, string DepartureAirport)
        {
           
            SQLServer da = new SQLServer(constr);
            DataSet Ds = new DataSet();
            //Database db = new Database();
            string[] Pname = new string[4];
            object[] Pvalue = new object[4];
            SqlDbType[] Ptype = new SqlDbType[4];

            try
            {

                Pname[0] = "FltNo";
                Pname[1] = "ManifestdateFrom";
                Pname[2] = "ManifestdateTo";
                Pname[3] = "DepartureAirport";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.DateTime;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;

                Pvalue[0] = FLTno;
                Pvalue[1] = ManifestdateFrom;
                Pvalue[2] = ManifestdateTo;
                Pvalue[3] = DepartureAirport;

                //Pname[1] = "FlightNo";
                //Ptype[1] = SqlDbType.NVarChar;
                //Pvalue[1] = FlightNo;


                Ds = da.SelectRecords("SPExpManiGetManifestDetailsforViaFlight", Pname, Pvalue, Ptype);


                return Ds;
            }

            catch (Exception ex)
            {
                return Ds;
            }
            finally
            {
                da = null;                
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        
        }
        #endregion Get Manifest Details for Flight and date

        # region GetManifestDetailsRevised
        public DataSet GetManifestDetailsRevised(string FLTno, string ManifestdateFrom, string ManifestdateTo, string DepartureAirport,string Version,string ULDno,string POL,string POU)
        {
            SQLServer da = new SQLServer(constr);
            DataSet Ds = new DataSet();
            string[] Pname = new string[8];
            object[] Pvalue = new object[8];
            SqlDbType[] Ptype = new SqlDbType[8];

            try
            {
                Pname[0] = "FltNo";
                Pname[1] = "ManifestdateFrom";
                Pname[2] = "ManifestdateTo";
                Pname[3] = "DepartureAirport";
                Pname[4] = "Version";
                Pname[5] = "ULD";
                Pname[6] = "POL";
                Pname[7] = "POU";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;

                Pvalue[0] = FLTno;
                Pvalue[1] = ManifestdateFrom;
                Pvalue[2] = ManifestdateTo;
                Pvalue[3] = DepartureAirport;
                Pvalue[4] = Version;
                Pvalue[5] = ULDno;
                Pvalue[6] = POL;
                Pvalue[7] = POU;

                Ds = da.SelectRecords("SPExpManiGetManifestDetailsRevised", Pname, Pvalue, Ptype);

                return Ds;
            }

            catch (Exception ex)
            {
                return Ds;
            }
            finally
            {
                da = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }        
        }
        # endregion GetManifestDetailsRevised

        #region Commit Manifest                                   

        public bool CommitManifestdata(string FLTno, string ULDno, string POU, string POL, string ULDDest, string counter, string AWBno, string SCC, 
            double VOL, int PCS, double WGT, int AVLPCS, double AVLWGT, string Desc, string LoadingPriority, string Remark, string Updatedby, 
            string Updatedon, DateTime FLTDate, string AWBPrefix, string CartNumber)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[21];
                object[] Pvalue = new object[21];
                SqlDbType[] Ptype = new SqlDbType[21];


                Pname[0] = "FLTno";
                Pname[1] = "ULDno";
                Pname[2] = "POL";
                Pname[3] = "POU";
                Pname[4] = "ULDDest";
                Pname[5] = "counter";
                Pname[6] = "AWBno";
                Pname[7] = "SCC";
                Pname[8] = "VOL";
                Pname[9] = "PCS";
                Pname[10] = "WGT";
                Pname[11] = "AVLPCS";
                Pname[12] = "AVLWGT";
                Pname[13] = "Desc";
                Pname[14] = "LoadingPriority";
                Pname[15] = "Remark";
                Pname[16] = "Updatedby";
                Pname[17] = "Updatedon";
                Pname[18] = "FLTDate";
                Pname[19] = "AWBPrefix";
                Pname[20] = "CartNumber";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.Float;
                Ptype[9] = SqlDbType.Int;
                Ptype[10] = SqlDbType.Float;
                Ptype[11] = SqlDbType.Int;
                Ptype[12] = SqlDbType.Float;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.VarChar;
                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.DateTime;
                Ptype[19] = SqlDbType.VarChar;
                Ptype[20] = SqlDbType.VarChar;

                Pvalue[0] = FLTno;
                Pvalue[1] = ULDno;
                Pvalue[2] = POL;
                Pvalue[3] = POU;
                Pvalue[4] = ULDDest;
                Pvalue[5] = counter;
                Pvalue[6] = AWBno;
                Pvalue[7] = SCC;
                Pvalue[8] = VOL;
                Pvalue[9] = PCS;
                Pvalue[10] = WGT;
                Pvalue[11] = AVLPCS;
                Pvalue[12] = AVLWGT;
                Pvalue[13] = Desc;
                Pvalue[14] = LoadingPriority;
                Pvalue[15] = Remark;
                Pvalue[16] = Updatedby;
                Pvalue[17] = Updatedon;
                Pvalue[18] = FLTDate;
                Pvalue[19] = AWBPrefix;
                Pvalue[20] = CartNumber;

                bool res = da.InsertData("SPExpManiCommitManifest", Pname, Ptype, Pvalue);


                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CommitManifestdata(string FLTno, string ULDno, string POU, string POL, string ULDDest, string counter, string AWBno, string SCC,
        double VOL, int PCS, double WGT, int AVLPCS, double AVLWGT, string Desc, string LoadingPriority, string Remark, string Updatedby,
        string Updatedon, DateTime FLTDate, string AWBPrefix, string CartNumber, string TailNo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[22];
                object[] Pvalue = new object[22];
                SqlDbType[] Ptype = new SqlDbType[22];

                Pname[0] = "FLTno";
                Pname[1] = "ULDno";
                Pname[2] = "POL";
                Pname[3] = "POU";
                Pname[4] = "ULDDest";
                Pname[5] = "counter";
                Pname[6] = "AWBno";
                Pname[7] = "SCC";
                Pname[8] = "VOL";
                Pname[9] = "PCS";
                Pname[10] = "WGT";
                Pname[11] = "AVLPCS";
                Pname[12] = "AVLWGT";
                Pname[13] = "Desc";
                Pname[14] = "LoadingPriority";
                Pname[15] = "Remark";
                Pname[16] = "Updatedby";
                Pname[17] = "Updatedon";
                Pname[18] = "FLTDate";
                Pname[19] = "AWBPrefix";
                Pname[20] = "CartNumber";
                Pname[21] = "TailNo";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.Float;
                Ptype[9] = SqlDbType.Int;
                Ptype[10] = SqlDbType.Float;
                Ptype[11] = SqlDbType.Int;
                Ptype[12] = SqlDbType.Float;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.VarChar;
                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.DateTime;
                Ptype[19] = SqlDbType.VarChar;
                Ptype[20] = SqlDbType.VarChar;
                Ptype[21] = SqlDbType.VarChar;

                Pvalue[0] = FLTno;
                Pvalue[1] = ULDno;
                Pvalue[2] = POL;
                Pvalue[3] = POU;
                Pvalue[4] = ULDDest;
                Pvalue[5] = counter;
                Pvalue[6] = AWBno;
                Pvalue[7] = SCC;
                Pvalue[8] = VOL;
                Pvalue[9] = PCS;
                Pvalue[10] = WGT;
                Pvalue[11] = AVLPCS;
                Pvalue[12] = AVLWGT;
                Pvalue[13] = Desc;
                Pvalue[14] = LoadingPriority;
                Pvalue[15] = Remark;
                Pvalue[16] = Updatedby;
                Pvalue[17] = Updatedon;
                Pvalue[18] = FLTDate;
                Pvalue[19] = AWBPrefix;
                Pvalue[20] = CartNumber;
                Pvalue[21] = TailNo;

                bool res = da.InsertData("SPExpManiCommitManifest", Pname, Ptype, Pvalue);


                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
        #endregion Commit Manifest 

        #region Offload Shipment in Manifest

        public bool OffLoadShipmentinManifest(string ActFlightNo, string OffloadFltNo, string OffloadLoc, string AWBNo, int ActPcs, double ActWt, int OffloadPcs, double OffloadWt, string Offloadedby,
            string POL, string POU, string FlightVersion, DateTime OffLoadFltDt, string Remarks, string Mode, DateTime ActFlightDate, DateTime UpdatedOn,string PartnerCode,string PartnerType)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();

            string[] Pname = new string[19];
            object[] Pvalue = new object[19];
            SqlDbType[] Ptype = new SqlDbType[19];

            try
            {
                Pname[0] = "ActFLTno";
                Pname[1] = "OffloadFLTno";
                Pname[2] = "OffloadLoc";
                Pname[3] = "AWBno";
                Pname[4] = "PCS";
                Pname[5] = "WGT";
                Pname[6] = "OffloadPCS";
                Pname[7] = "OffloadWGT";
                Pname[8] = "Offloadeddby";
                Pname[9] = "Offloadedon";
                Pname[10] = "POL";
                Pname[11] = "POU";
                Pname[12] = "FltVersion";
                Pname[13] = "OffloadFLTDate";
                Pname[14] = "Remarks";
                Pname[15] = "Flag";
                Pname[16] = "ActFltDate";
                Pname[17] = "Carrier";
                Pname[18] = "PartnerType";


                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.Int;
                Ptype[5] = SqlDbType.Float;
                Ptype[6] = SqlDbType.Int;
                Ptype[7] = SqlDbType.Float;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.DateTime;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.DateTime;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.DateTime;
                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.VarChar;


                Pvalue[0] = ActFlightNo;
                Pvalue[1] = OffloadFltNo;
                Pvalue[2] = OffloadLoc;
                Pvalue[3] = AWBNo;
                Pvalue[4] = ActPcs;
                Pvalue[5] = ActWt;
                Pvalue[6] = OffloadPcs;
                Pvalue[7] = OffloadWt;
                Pvalue[8] = Offloadedby;
                Pvalue[9] = UpdatedOn.ToString("yyyy-MM-dd");
                Pvalue[10] = POL;
                Pvalue[11] = POU;
                Pvalue[12] = FlightVersion;
                Pvalue[13] = OffLoadFltDt;
                Pvalue[14] = Remarks;
                Pvalue[15] = Mode;
                Pvalue[16] = ActFlightDate;
                Pvalue[17] = PartnerCode;
                Pvalue[18] = PartnerType;

                bool res = da.InsertData("SPExpManiSaveOffload", Pname, Ptype, Pvalue);

                return res;
            }
            catch (Exception ex)
            {
                return false;
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


        #endregion Offload Shipment in Manifest

        #region Offload Shipment in Manifest For GHA

        public bool OffLoadShipmentinManifestForGHA(string ActFlightNo, string OffloadFltNo, string OffloadLoc, string AWBNo, int ActPcs, double ActWt, int OffloadPcs, double OffloadWt, string Offloadedby,
            string POL, string POU, string FlightVersion, DateTime OffLoadFltDt, string Remarks, string Mode, DateTime ActFlightDate, DateTime UpdatedOn, string PartnerCode, string PartnerType, string ULDNo,
            DateTime dtTimeStamp, string AWBPrefix, string Location, string FltFlag, string CartNumber)
        {
            SQLServer da = new SQLServer(constr);
            //DataSet ds = new DataSet();

            string[] Pname = new string[25];
            object[] Pvalue = new object[25];
            SqlDbType[] Ptype = new SqlDbType[25];

            try
            {
                Pname[0] = "ActFLTno";
                Pname[1] = "OffloadFLTno";
                Pname[2] = "OffloadLoc";
                Pname[3] = "AWBno";
                Pname[4] = "PCS";
                Pname[5] = "WGT";
                Pname[6] = "OffloadPCS";
                Pname[7] = "OffloadWGT";
                Pname[8] = "Offloadeddby";
                Pname[9] = "Offloadedon";
                Pname[10] = "POL";
                Pname[11] = "POU";
                Pname[12] = "FltVersion";
                Pname[13] = "OffloadFLTDate";
                Pname[14] = "Remarks";
                Pname[15] = "Flag";
                Pname[16] = "ActFltDate";
                Pname[17] = "Carrier";
                Pname[18] = "PartnerType";
                Pname[19] = "ULDNo";
                Pname[20] = "AWBPrefix";
                Pname[21] = "TimeStamp";
                Pname[22] = "Location";
                Pname[23] = "FltFlag";
                Pname[24] = "CartNumber";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.Int;
                Ptype[5] = SqlDbType.Float;
                Ptype[6] = SqlDbType.Int;
                Ptype[7] = SqlDbType.Float;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.DateTime;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.DateTime;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.DateTime;
                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.VarChar;
                Ptype[19] = SqlDbType.VarChar;
                Ptype[20] = SqlDbType.VarChar;
                Ptype[21] = SqlDbType.DateTime;
                Ptype[22] = SqlDbType.VarChar;
                Ptype[23] = SqlDbType.VarChar;
                Ptype[24] = SqlDbType.VarChar;

                Pvalue[0] = ActFlightNo;
                Pvalue[1] = OffloadFltNo;
                Pvalue[2] = OffloadLoc;
                Pvalue[3] = AWBNo;
                Pvalue[4] = ActPcs;
                Pvalue[5] = ActWt;
                Pvalue[6] = OffloadPcs;
                Pvalue[7] = OffloadWt;
                Pvalue[8] = Offloadedby;
                Pvalue[9] = UpdatedOn.ToString("yyyy-MM-dd");
                Pvalue[10] = POL;
                Pvalue[11] = POU;
                Pvalue[12] = FlightVersion;
                Pvalue[13] = OffLoadFltDt;
                Pvalue[14] = Remarks;
                Pvalue[15] = Mode;
                Pvalue[16] = ActFlightDate;
                Pvalue[17] = PartnerCode;
                Pvalue[18] = PartnerType;
                Pvalue[19] = ULDNo;
                Pvalue[20] = AWBPrefix;
                Pvalue[21] = dtTimeStamp;
                Pvalue[22] = Location;
                Pvalue[23] = FltFlag;
                Pvalue[24] = CartNumber;

                string SPName = string.Empty;

                if (ULDNo.Trim().ToUpper() == "BULK" || ULDNo.Trim().ToUpper() == "")
                    SPName = "SPExpManiSaveOffload";
                else
                    SPName = "SPExpManiSaveOffloadforULD";

                bool res = da.InsertData(SPName, Pname, Ptype, Pvalue);

                return res;
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

        public bool OffLoadShipmentinManifestForGHA(string ActFlightNo, string OffloadFltNo, string OffloadLoc, string AWBNo, int ActPcs, double ActWt, int OffloadPcs, double OffloadWt, string Offloadedby,
            string POL, string POU, string FlightVersion, DateTime OffLoadFltDt, string Remarks, string Mode, DateTime ActFlightDate, DateTime UpdatedOn, string PartnerCode, string PartnerType, string ULDNo,
            DateTime dtTimeStamp, string AWBPrefix, string Location, string FltFlag, string CartNumber, string Source)
        {
            SQLServer da = new SQLServer(constr);
            //DataSet ds = new DataSet();

            string[] Pname = new string[26];
            object[] Pvalue = new object[26];
            SqlDbType[] Ptype = new SqlDbType[26];

            try
            {
                Pname[0] = "ActFLTno";
                Pname[1] = "OffloadFLTno";
                Pname[2] = "OffloadLoc";
                Pname[3] = "AWBno";
                Pname[4] = "PCS";
                Pname[5] = "WGT";
                Pname[6] = "OffloadPCS";
                Pname[7] = "OffloadWGT";
                Pname[8] = "Offloadeddby";
                Pname[9] = "Offloadedon";
                Pname[10] = "POL";
                Pname[11] = "POU";
                Pname[12] = "FltVersion";
                Pname[13] = "OffloadFLTDate";
                Pname[14] = "Remarks";
                Pname[15] = "Flag";
                Pname[16] = "ActFltDate";
                Pname[17] = "Carrier";
                Pname[18] = "PartnerType";
                Pname[19] = "ULDNo";
                Pname[20] = "AWBPrefix";
                Pname[21] = "TimeStamp";
                Pname[22] = "Location";
                Pname[23] = "FltFlag";
                Pname[24] = "CartNumber";
                Pname[25] = "Source";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.Int;
                Ptype[5] = SqlDbType.Float;
                Ptype[6] = SqlDbType.Int;
                Ptype[7] = SqlDbType.Float;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.DateTime;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.DateTime;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.DateTime;
                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.VarChar;
                Ptype[19] = SqlDbType.VarChar;
                Ptype[20] = SqlDbType.VarChar;
                Ptype[21] = SqlDbType.DateTime;
                Ptype[22] = SqlDbType.VarChar;
                Ptype[23] = SqlDbType.VarChar;
                Ptype[24] = SqlDbType.VarChar;
                Ptype[25] = SqlDbType.VarChar;

                Pvalue[0] = ActFlightNo;
                Pvalue[1] = OffloadFltNo;
                Pvalue[2] = OffloadLoc;
                Pvalue[3] = AWBNo;
                Pvalue[4] = ActPcs;
                Pvalue[5] = ActWt;
                Pvalue[6] = OffloadPcs;
                Pvalue[7] = OffloadWt;
                Pvalue[8] = Offloadedby;
                Pvalue[9] = UpdatedOn.ToString("yyyy-MM-dd");
                Pvalue[10] = POL;
                Pvalue[11] = POU;
                Pvalue[12] = FlightVersion;
                Pvalue[13] = OffLoadFltDt;
                Pvalue[14] = Remarks;
                Pvalue[15] = Mode;
                Pvalue[16] = ActFlightDate;
                Pvalue[17] = PartnerCode;
                Pvalue[18] = PartnerType;
                Pvalue[19] = ULDNo;
                Pvalue[20] = AWBPrefix;
                Pvalue[21] = dtTimeStamp;
                Pvalue[22] = Location;
                Pvalue[23] = FltFlag;
                Pvalue[24] = CartNumber;
                Pvalue[25] = Source;

                string SPName = string.Empty;

                if (ULDNo.Trim().ToUpper() == "BULK" || ULDNo.Trim().ToUpper() == "")
                    SPName = "SPExpManiSaveOffload";
                else
                    SPName = "SPExpManiSaveOffloadforULD";

                bool res = da.InsertData(SPName, Pname, Ptype, Pvalue);

                return res;
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

        #endregion Offload Shipment in Manifest For GHA

        #region Return To Shipper

        public bool ReturnToShipper(string ActFlightNo, string OffloadFltNo, string OffloadLoc, string AWBNo, int ActPcs, double ActWt, int OffloadPcs, double OffloadWt, string Offloadedby,
            string POL, string POU, string FlightVersion, DateTime OffLoadFltDt, string Remarks, string Mode)
        {
            SQLServer da = new SQLServer(constr);
            //DataSet ds = new DataSet();

            string[] Pname = new string[16];
            object[] Pvalue = new object[16];
            SqlDbType[] Ptype = new SqlDbType[16];

            try
            {
                Pname[0] = "ActFLTno";
                Pname[1] = "ReturnFLTno";
                Pname[2] = "ReturnLoc";
                Pname[3] = "AWBno";
                Pname[4] = "PCS";
                Pname[5] = "WGT";
                Pname[6] = "ReturnPCS";
                Pname[7] = "ReturnWGT";
                Pname[8] = "Returneddby";
                Pname[9] = "Returnedon";
                Pname[10] = "POL";
                Pname[11] = "POU";
                Pname[12] = "FltVersion";
                Pname[13] = "ReturnFLTDate";
                Pname[14] = "Remarks";
                Pname[15] = "Flag";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.Int;
                Ptype[5] = SqlDbType.Float;
                Ptype[6] = SqlDbType.Int;
                Ptype[7] = SqlDbType.Float;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.DateTime;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.DateTime;
                Ptype[14] = SqlDbType.VarChar;
                Ptype[15] = SqlDbType.VarChar;

                Pvalue[0] = ActFlightNo;
                Pvalue[1] = OffloadFltNo;
                Pvalue[2] = OffloadLoc;
                Pvalue[3] = AWBNo;
                Pvalue[4] = ActPcs;
                Pvalue[5] = ActWt;
                Pvalue[6] = OffloadPcs;
                Pvalue[7] = OffloadWt;
                Pvalue[8] = Offloadedby;
                Pvalue[9] = DateTime.Now.ToString("yyyy-MM-dd");
                Pvalue[10] = POL;
                Pvalue[11] = POU;
                Pvalue[12] = FlightVersion;
                Pvalue[13] = OffLoadFltDt;
                Pvalue[14] = Remarks;
                Pvalue[15] = Mode;

                bool res = da.InsertData("SPExpManiReturnShipper", Pname, Ptype, Pvalue);

                return res;
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


        #endregion Return To Shipper

        #region Tab ULDAWB Dest,VOL,Desc Details
        public DataSet GetULDAWBData(string AWBnos)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            string[] Pname = new string[1];
            object[] Pvalue = new object[1];
            SqlDbType[] Ptype = new SqlDbType[1];

            try
            {
                Pname[0] = "AWBNo";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = AWBnos;

                ds = da.SelectRecords("SPExpGetManiDestVolAWBdata", Pname, Pvalue, Ptype);

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
        #endregion Tab ULDAWB Dest,VOL,Desc Details

        #region Re-Open Manifest

        public string ReOpenManifestdata(string FLTno, string ULDno, string POU, string POL, string Updatedby, DateTime Updatedon, string FlightStatus, DateTime FlightDate)
        {
            string res = "";
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();

            string[] Pname = new string[8];
            object[] Pvalue = new object[8];
            SqlDbType[] Ptype = new SqlDbType[8];
            try
            {
                Pname[0] = "FLTno";
                Pname[1] = "ULDno";
                Pname[2] = "POL";
                Pname[3] = "POU";
                Pname[4] = "blnFlag";
                Pname[5] = "UserName";
                Pname[6] = "dtUpdatedOn";
                Pname[7] = "FLTDate";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.DateTime;
                Ptype[7] = SqlDbType.DateTime;

                Pvalue[0] = FLTno;
                Pvalue[1] = ULDno;
                Pvalue[2] = POL;
                Pvalue[3] = POU;
                Pvalue[4] = FlightStatus;
                Pvalue[5] = Updatedby;
                Pvalue[6] = Updatedon;
                Pvalue[7] = FlightDate;
                DataSet str = new DataSet();
                str = da.SelectRecords("spFlightManifestReOpen", Pname, Pvalue, Ptype);

                if (str.Tables[0].Rows.Count > 0)
                {

                    res = str.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();
                }

                return res;
            }
            catch (Exception ex)
            {
                return res;
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

        #endregion Re-Open Manifest

        #region Re-Open Manifest with Tail No

        public string ReOpenManifestdata(string FLTno, string ULDno, string POU, string POL, string Updatedby, 
            DateTime Updatedon, string FlightStatus, DateTime FlightDate, string TailNo, string FltOrigin)
        {
            string res = "";
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();

            string[] Pname = new string[10];
            object[] Pvalue = new object[10];
            SqlDbType[] Ptype = new SqlDbType[10];
            try
            {
                Pname[0] = "FLTno";
                Pname[1] = "ULDno";
                Pname[2] = "POL";
                Pname[3] = "POU";
                Pname[4] = "blnFlag";
                Pname[5] = "UserName";
                Pname[6] = "dtUpdatedOn";
                Pname[7] = "FLTDate";
                Pname[8] = "TailNo";
                Pname[9] = "FltOrigin";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.DateTime;
                Ptype[7] = SqlDbType.DateTime;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;

                Pvalue[0] = FLTno;
                Pvalue[1] = ULDno;
                Pvalue[2] = POL;
                Pvalue[3] = POU;
                Pvalue[4] = FlightStatus;
                Pvalue[5] = Updatedby;
                Pvalue[6] = Updatedon;
                Pvalue[7] = FlightDate;
                Pvalue[8] = TailNo;
                Pvalue[9] = FltOrigin;

                DataSet str = new DataSet();
                str = da.SelectRecords("spFlightManifestReOpen", Pname, Pvalue, Ptype);

                if (str.Tables[0].Rows.Count > 0)
                {
                    res = str.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();
                }

                return res;
            }
            catch (Exception ex)
            {
                return res;
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

        #endregion Re-Open Manifest with Tail No


        #region Save Nil Manifest

        public bool SaveNilManifest(string FlightNo, string ULDNo, string POL, string POU, string UserName, DateTime FlightDate, DateTime SysDate,string tailNo)
        {
            SQLServer da = new SQLServer(constr);
            string[] Pname = new string[8];
            object[] Pvalue = new object[8];
            SqlDbType[] Ptype = new SqlDbType[8];

            try
            {
                Pname[0] = "FLTno";
                Pname[1] = "ULDno";
                Pname[2] = "POL";
                Pname[3] = "POU";
                Pname[4] = "Updatedby";
                Pname[5] = "Updatedon";
                Pname[6] = "FLTDate";
                Pname[7] = "tailNo";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.DateTime;
                Ptype[6] = SqlDbType.DateTime;
                Ptype[7] = SqlDbType.VarChar;

                Pvalue[0] = FlightNo;
                Pvalue[1] = ULDNo;
                Pvalue[2] = POL;
                Pvalue[3] = POU;
                Pvalue[4] = UserName;
                Pvalue[5] = SysDate;
                Pvalue[6] = FlightDate;
                Pvalue[7] = tailNo;

                bool res = da.InsertData("SPExpManiSaveNilManifest", Pname, Ptype, Pvalue);

                return res;
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


        #endregion Save Nil Manifest


        # region Get Departed PP AWB Data
        public DataSet GetDepartedPPAWBData(object[] RateCardInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AWBPrefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                
                DataSet ds = da.SelectRecords("SP_GetDepartedPPAWBData", ColumnNames, Values, DataType);
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
        # endregion Get Departed PP AWB Data


        # region Generate Bunch Invoice Numbers For WalkIN Agent AWBs
        public string GenerateBunchInvoiceNumWalkInAgent(object[] RateCardInfo)
        {
            try
            {
                //Check if UpdatedOn is received from calling function.
                int Count = RateCardInfo.Length + 1;
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[Count];
                SqlDbType[] DataType = new SqlDbType[Count];
                Object[] Values = new object[Count];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = da.GetStringByProcedure("SP_GenerateBunchInvoiceNumWalkInAgentNew", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Generate Bunch Invoice Numbers For WalkIN Agent AWBs


        # region InsertDepartedPPAWBData
        public string InsertDepartedPPAWBData(object[] RateCardInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = da.GetStringByProcedure("SP_InsertDepartedPPAWBData", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion InsertDepartedPPAWBData

         # region Get Departed PP AWB Data
        public DataSet GetOffloadReasons()
        {
            DataSet ds =new DataSet();
            SQLServer da = new SQLServer(constr);
            try
            {
                ds = da.SelectRecords("SpGetReasonForExpManifest");
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

        public DataSet GetReturnToShipperReasons()
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(constr);
            try
            {
                ds = da.SelectRecords("SpGetReasonForReturnToShipper");
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
          #endregion

        #region Delete Manifest Details
        public string DeleteManifestDetails(object[] ManifestDetails)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("FLTno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                i++;

                //1
                ColumnNames.SetValue("POL", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                i++;

                //2
                ColumnNames.SetValue("FLTDate", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                i++;

                //3
                ColumnNames.SetValue("VersionNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                i++;

                string res = da.GetStringByProcedure("spDeleteManifestDetails", ColumnNames, ManifestDetails, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Delete Manifest Details

        public DataSet GetFlightReopenTimeInterval(string FlightNo, DateTime FlightDt, string Station, string ReopenConfigflag)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet Ds = null;
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightNo, i);
                i++;

                ColumnNames.SetValue("FlightDt", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(FlightDt, i);
                i++;

                ColumnNames.SetValue("Station", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Station, i);
                i++;

                ColumnNames.SetValue("ConfigFlag", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ReopenConfigflag, i);

                Ds = da.SelectRecords("sp_GetFlightReopenTimeInterval", ColumnNames, Values, DataType);
                return Ds;                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Validate if next leg flight of AWB Number is not Departed
        public void IsNextLegReOpenedforOffload(string AWBNumber, string AWBPrefix, string FlightNo, DateTime FlightDate, 
            string ULDNumber, string CartNumber, out string Result)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[6];
                SqlDbType[] DataType = new SqlDbType[6];
                Object[] Values = new object[6];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBNumber, i);
                i++;

                ColumnNames.SetValue("AWBPrefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBPrefix, i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightNo, i);
                i++;

                ColumnNames.SetValue("FlightDate", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(FlightDate, i);
                i++;

                ColumnNames.SetValue("ULDNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ULDNumber, i);
                i++;

                ColumnNames.SetValue("CartNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CartNumber, i);
                i++;

                Result = da.GetStringByProcedure("spValidateNextLegForOffload", ColumnNames, Values, DataType);

            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
        }
        #endregion Validate if next leg flight of AWB Number is not Departed

        #region GetAWBForReassign
        public DataSet GetAWBForReassign(string AWBPrefix, string AWBNumber,string FlightNo,string FlightDate,string Station)
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
            string NewFlightNo,string NewFlightDate,int OldPcs, decimal OldWt,int NewPcs, decimal NewWt, string Station, 
            DateTime UpdatedOn, string UpdatedBy)
        {
            DataSet dsReassignAWBs = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[13];
                SqlDbType[] DataType = new SqlDbType[13];
                Object[] Values = new object[13];
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

        #region Get AWB Destination
        public String GetAWBDestination(string AWBPrefix, string AWBNumber)
        {
            string AWBDestination = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
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

                AWBDestination = da.GetStringByProcedure("spGetAWBDestination", ColumnNames, Values, DataType);
                
                da = null;

                return (AWBDestination);

            }
            catch (Exception)
            {
                return (null);
            }
        }
        #endregion Get AWB Destination

        #region Get Flight Status
        /// <summary>
        /// Gets Manifest status of flight
        /// </summary>
        /// <param name="FlightNo">Flight Number</param>
        /// <param name="FlightDate">Flight date in dd/MM/yyyy</param>
        /// <param name="POL">Origin station</param>
        /// <returns>Returns status of flight(M/D/R/N)</returns>
        public string GetFlightStatus(string FlightNo, string FlightDate, string POL)
        {
            string fltStatus = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("Source", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(POL, i);
                i++;

                ColumnNames.SetValue("Dest", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;

                ColumnNames.SetValue("FltNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightNo, i);
                i++;

                ColumnNames.SetValue("Date", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightDate, i);
                i++; 

                fltStatus = da.GetStringByProcedure("spGetFlightStatus", ColumnNames, Values, DataType);

                da = null;

                return (fltStatus);

            }
            catch (Exception)
            {
                return (null);
            }
        }
        #endregion Get Flight Status

    }
}
