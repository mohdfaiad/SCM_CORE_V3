using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;


namespace BAL
{
   public class BLExpManifestGHA
    {
        SQLServer  db = new SQLServer("");
       
        #region Variables
        string constr = "";
        #endregion Variables

        #region Constructor
        public BLExpManifestGHA()
        {
           constr = Global.GetConnectionString();
        }
        #endregion Constructor

        #region Tab AWB Details

        public   DataSet GetAwbTabdetails( string POL,string FlightNo, DateTime FlightDate)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds=new DataSet ();
            try
            {

                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];


                Pname[0] = "POL";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = POL ;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNo;

                Pname[2] = "FltDate";
                Ptype[2] = SqlDbType.DateTime;
                Pvalue[2] = FlightDate;



                ds = da.SelectRecords("SPExpManiGetTabAWBdataGHA", Pname, Pvalue, Ptype);


                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }
        
        }

        public DataSet GetAwbTabdetails_GHA(string POL, string FlightNo, DateTime FlightDate)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            try
            {

                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];


                Pname[0] = "POL";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = POL;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNo;

                Pname[2] = "FltDate";
                Ptype[2] = SqlDbType.DateTime;
                Pvalue[2] = FlightDate;

                ds = da.SelectRecords("SPExpManiGetTabAWBdata_GHA_FlightMan", Pname, Pvalue, Ptype);

                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion Tab AWB Details
        
        #region Tab flight Details

        public DataSet GetFlightTabdetails(string Origin, string FlightNo, DateTime dtFlightDate)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            try
            {

                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];


                Pname[0] = "Origin";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = Origin;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNo;

                Pname[2] = "FlightDate";
                Ptype[2] = SqlDbType.DateTime;
                Pvalue[2] = dtFlightDate;
                
                ds = da.SelectRecords("SPExpManiGetRouteDetailsGHA", Pname, Pvalue, Ptype);


                return ds;

            }
            catch (Exception ex)
            {
                return ds;
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


                ds = da.SelectRecords("SPExpManiGetTabULDdataGHA", Pname, Pvalue, Ptype);


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
            int AVLPCS, double AVLWGT, string Updatedby, DateTime Updatedon, DateTime FltDate, string Identifier,ref string Result)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;
                
                    string[] Pname = new string[14];
                    object[] Pvalue = new object[14];
                    SqlDbType[] Ptype = new SqlDbType[14];


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
              
                    Ptype[0] = SqlDbType.VarChar;
                    Ptype[1] = SqlDbType.VarChar;
                    Ptype[2] = SqlDbType.VarChar;
                    Ptype[3] = SqlDbType.VarChar;
                    Ptype[4] = SqlDbType.Float  ;
                    Ptype[5] = SqlDbType.VarChar;
                    Ptype[6] = SqlDbType.Int ;
                    Ptype[7] = SqlDbType.Float ;
                    Ptype[8] = SqlDbType.Int;
                    Ptype[9] = SqlDbType.Float;
                    Ptype[10] = SqlDbType.VarChar ;
                    Ptype[11] = SqlDbType.DateTime;
                    Ptype[12] = SqlDbType.DateTime;
                    Ptype[13] = SqlDbType.VarChar;

                    Pvalue[0] = FLTno;        
                    Pvalue[1] = POL ;
                    Pvalue[2] = POU ;        
                    Pvalue[3] = ULDno;
                    Pvalue[4] = ULDwgt;        
                    Pvalue[5] = AWBno ;
                    Pvalue[6] = PCS ;        
                    Pvalue[7] = WGT  ;
                    Pvalue[8] = AVLPCS ;        
                    Pvalue[9] = AVLWGT  ;
                    Pvalue[10] = Updatedby ;        
                    Pvalue[11] = Updatedon  ;
                    Pvalue[12] = FltDate;
                    Pvalue[13] = Identifier;

                    ds =da.SelectRecords("SPExpManiSaveULDAWBassociationGHA", Pname,Pvalue, Ptype);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Result = ds.Tables[0].Rows[0][0].ToString();
                        ds = null;
                        return false;
                    }

                    ds = null;
                return true ;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
                
        #region Tab Get ULDAWB Association Details

        public DataSet GetULDAWBassocitionData(string SelectedULDnos)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            //Database db = new Database();
            


            try
            {

                string[] Pname = new string[1];
                object[] Pvalue = new object[1];
                SqlDbType[] Ptype = new SqlDbType[1];


                Pname[0] = "SelectedULDnos";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] =SelectedULDnos;

                //Pname[1] = "FlightNo";
                //Ptype[1] = SqlDbType.NVarChar;
                //Pvalue[1] = FlightNo;


                ds = da.SelectRecords("SPExpManiGetULDAWBassociationdataGHA", Pname, Pvalue, Ptype);
                //ds = db.SelectRecords("SPExpManiGetULDAWBassociationdata", Pname, Pvalue, Ptype);


                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion Tab ULDAWB Association Details

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


                Ds = da.SelectRecords("SPExpManiGetVerULDPOUPOLddlDataGHA", Pname, Pvalue, Ptype);


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
            // Database db = new Database();

            try
            {
                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];


                Pname[0] = "FlightID";
                Pname[1] = "Source";
                Pname[2] = "FlightDate";
             

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.DateTime;


                Pvalue[0] = FLTiD;
                Pvalue[1] = Source;
                Pvalue[2] = dtFlightDate;



                Ds = da.SelectRecords("spExpManiGetAirlineSch1GHA", Pname, Pvalue, Ptype);


                return Ds;
            }

            catch (Exception ex)
            {
                return Ds;
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

                Ds = da.SelectRecords("GetDGRPiecesWeightGHA", Pname, Pvalue, Ptype);//not find
                
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
            // Database db = new Database();

            try
            {
                string[] Pname = new string[1];
                object[] Pvalue = new object[1];
                SqlDbType[] Ptype = new SqlDbType[1];

                Pname[0] = "AWBNumber";

                Ptype[0] = SqlDbType.VarChar;

                Pvalue[0] = AWBNo;

                Ds = da.SelectRecords("SpGetDGRCargoGHA", Pname, Pvalue, Ptype);

                return Ds;
            }

            catch (Exception ex)
            {
                return Ds;
            }

        }
        #endregion Get DGR details

        #region Save Manifest Data
        //                             

        public bool SaveManifestdata(string FLTno, string ULDno, string POU, string POL, string ULDDest, string counter, string AWBno, string SCC,
            double VOL, int PCS, double WGT, int AVLPCS, double AVLWGT, string Desc, string LoadingPriority, string Remark, string Updatedby, 
            string Updatedon, bool IsManifested, DateTime dtFLTDate, string CurrLoc, string AWBOrigin, Int64 IrregularityID,string IsBonded,string tailNo,string IMEINo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[25];
                object[] Pvalue = new object[25];
                SqlDbType[] Ptype = new SqlDbType[25];


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
                // Pname[20] = "CurrentLoc";

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

                bool res = da.InsertData("SPExpManiSaveManifestGHA", Pname, Ptype, Pvalue);


                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion Save Manifest Data


        #region Get Manifest Details for Flight and date

        public DataSet GetManifestDetails(string FLTno, DateTime ManifestdateFrom, string ManifestdateTo, string DepartureAirport)
        {
           
            SQLServer da = new SQLServer(constr);
            DataSet Ds = new DataSet();
            //Database db = new Database();

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

                Pvalue[0] = FLTno ;
                Pvalue[1] = ManifestdateFrom;
                Pvalue[2] = ManifestdateTo;
                Pvalue[3] = DepartureAirport ;

                //Pname[1] = "FlightNo";
                //Ptype[1] = SqlDbType.NVarChar;
                //Pvalue[1] = FlightNo;


                Ds = da.SelectRecords("SPExpManiGetManifestDetailsforViaFlightGHA", Pname, Pvalue, Ptype);


                return Ds;
            }

            catch (Exception ex)
            {
                return Ds;
            }
        
        }
        #endregion Get Manifest Details for Flight and date

        # region GetManifestDetailsRevised
        public DataSet GetManifestDetailsRevised(string FLTno, string ManifestdateFrom, string ManifestdateTo, string DepartureAirport,string Version,string ULDno,string POL,string POU)
        {
            SQLServer da = new SQLServer(constr);
            DataSet Ds = new DataSet();
            //Database db = new Database();

            try
            {
                string[] Pname = new string[8];
                object[] Pvalue = new object[8];
                SqlDbType[] Ptype = new SqlDbType[8];


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


                //Pname[1] = "FlightNo";
                //Ptype[1] = SqlDbType.NVarChar;
                //Pvalue[1] = FlightNo;


                Ds = da.SelectRecords("SPExpManiGetManifestDetailsRevisedGHA", Pname, Pvalue, Ptype);


                return Ds;
            }

            catch (Exception ex)
            {
                return Ds;
            }
        
        }
        # endregion GetManifestDetailsRevised

        //

        #region Commit Manifest                                   

        public bool CommitManifestdata(string FLTno, string ULDno, string POU, string POL, string ULDDest, string counter, string AWBno, string SCC, 
            double VOL, int PCS, double WGT, int AVLPCS, double AVLWGT, string Desc, string LoadingPriority, string Remark, string Updatedby, string Updatedon, DateTime FLTDate)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[19];
                object[] Pvalue = new object[19];
                SqlDbType[] Ptype = new SqlDbType[19];


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


                bool res = da.InsertData("SPExpManiCommitManifestGHA", Pname, Ptype, Pvalue);


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
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[19];
                object[] Pvalue = new object[19];
                SqlDbType[] Ptype = new SqlDbType[19];

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
            


                bool res = da.InsertData("SPExpManiSaveOffloadGHA", Pname, Ptype, Pvalue);
                
                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion Offload Shipment in Manifest


        #region Offload Shipment in Manifest For GHA

        public bool OffLoadShipmentinManifestForGHA(string ActFlightNo, string OffloadFltNo, string OffloadLoc, string AWBNo, int ActPcs, double ActWt, int OffloadPcs, double OffloadWt, string Offloadedby,
            string POL, string POU, string FlightVersion, DateTime OffLoadFltDt, string Remarks, string Mode, DateTime ActFlightDate, DateTime UpdatedOn, string PartnerCode, string PartnerType, string ULDNo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[20];
                object[] Pvalue = new object[20];
                SqlDbType[] Ptype = new SqlDbType[20];

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


                bool res = da.InsertData("SPExpManiSaveOffloadGHA", Pname, Ptype, Pvalue);

                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion Offload Shipment in Manifest For GHA


        #region Return To Shipper

        public bool ReturnToShipper(string ActFlightNo, string OffloadFltNo, string OffloadLoc, string AWBNo, int ActPcs, double ActWt, int OffloadPcs, double OffloadWt, string Offloadedby,
            string POL, string POU, string FlightVersion, DateTime OffLoadFltDt, string Remarks, string Mode)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[16];
                object[] Pvalue = new object[16];
                SqlDbType[] Ptype = new SqlDbType[16];

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

                bool res = da.InsertData("SPExpManiReturnShipperGHA", Pname, Ptype, Pvalue);

                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion Return To Shipper




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

                //Pname[1] = "FlightNo";
                //Ptype[1] = SqlDbType.NVarChar;
                //Pvalue[1] = FlightNo;


                ds = da.SelectRecords("SPExpGetManiDestVolAWBdataGHA", Pname, Pvalue, Ptype);
                //ds = db.SelectRecords("SPExpManiGetULDAWBassociationdata", Pname, Pvalue, Ptype);


                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion Tab ULDAWB Dest,VOL,Desc Details

        #region Re-Open Manifest

        public string ReOpenManifestdata(string FLTno, string ULDno, string POU, string POL, string Updatedby, DateTime Updatedon, string FlightStatus, DateTime FlightDate)
        {
            string res = "";
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[8];
                object[] Pvalue = new object[8];
                SqlDbType[] Ptype = new SqlDbType[8];

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
                 str = da.SelectRecords ("spFlightManifestReOpenGHA", Pname,Pvalue, Ptype);

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
        }


        #endregion Re-Open Manifest

        #region Save Nil Manifest

        public bool SaveNilManifest(string FlightNo, string ULDNo, string POL, string POU, string UserName, DateTime FlightDate, DateTime SysDate,string tailNo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[8];
                object[] Pvalue = new object[8];
                SqlDbType[] Ptype = new SqlDbType[8];

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

                bool res = da.InsertData("SPExpManiSaveNilManifestGHA", Pname, Ptype, Pvalue);

                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion Save Nil Manifest


        # region Get Departed PP AWB Data
        public DataSet GetDepartedPPAWBData(object[] RateCardInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                
                DataSet ds = da.SelectRecords("SP_GetDepartedPPAWBDataGHA", ColumnNames, Values, DataType);
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
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = da.GetStringByProcedure("SP_GenerateBunchInvoiceNumWalkInAgentNewGHA", ColumnNames, Values, DataType);
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
                string[] ColumnNames = new string[14];
                SqlDbType[] DataType = new SqlDbType[14];
                Object[] Values = new object[14];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("OriginCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("DestinationCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("Pieces", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("GrossWeight", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("ChargedWeight", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("FrtIATA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FrtMKT", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("OCDueCar", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("OCDueAgent", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ServTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AWBDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = da.GetStringByProcedure("SP_InsertDepartedPPAWBDataGHA", ColumnNames, Values, DataType);
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
        {DataSet ds =new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);
                 ds = da.SelectRecords("SpGetReasonForExpManifestGHA");
                
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;

        }
          #endregion
    }
}
