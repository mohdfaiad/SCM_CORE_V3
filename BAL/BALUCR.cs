using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BALUCR
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        #endregion Variables

        #region GetDetailsUCRULD
        public DataSet GetDetailsUCRULD(string SPType, string UCRNo, string ULDNo, string RecNo, bool isDamaged, string ReturnedAtWHCode, DateTime ReturnedOn, string AWBPrefix, string AWBNo, bool isLoaded)
        {
            SQLServer da = new SQLServer(constr);
            string[] pname = null;
            object[] pvalue = null;
            SqlDbType[] ptype = null;

            try
            {
                pname = new string[10]
            {   
                "SPType",
                "UCRNo",
                "ULDNo",
                "RecNo",
                "isDamaged",
                "ReturnedAtWHCode",
                "ReturnedOn",
                "AWBPrefix",
                "AWBNo",
                "isLoaded"
            };
                pvalue = new object[10]
            {
                SPType,//"GET",
                UCRNo,//""
                ULDNo,//""
                RecNo,//""
                isDamaged,//1,
                ReturnedAtWHCode,//"",
                ReturnedOn,//DateTime.Now
                AWBPrefix,
                AWBNo,
                isLoaded
            };
                ptype = new SqlDbType[10]
            {
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.Bit,
                SqlDbType.VarChar,
                SqlDbType.DateTime,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.Bit
            };
                DataSet dsUCRULDDetails = da.SelectRecords("spADDUCRULDDetails", pname, pvalue, ptype);
                return dsUCRULDDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                pname = null;
                pvalue = null;
                ptype = null;
            }
        }
        #endregion

        #region GetDetailsUCRULDbool
        public bool GetDetailsUCRULDbool(string SPType, string UCRNo, string ULDNo, string RecNo, bool isDamaged, string ReturnedAtWHCode, DateTime ReturnedOn, string AWBPrefix, string AWBNo, bool isLoaded)
        {
            SQLServer da = new SQLServer(constr);
            string[] pname = null;
            object[] pvalue = null;
            SqlDbType[] ptype = null;

            try
            {

                pname = new string[10]
            {   
                "SPType",
                "UCRNo",
                "ULDNo",
                "RecNo",
                "isDamaged",
                "ReturnedAtWHCode",
                "ReturnedOn",
                "AWBPrefix",
                "AWBNo",
                "isLoaded"
            };
                pvalue = new object[10]
            {
                SPType,//"GET",
                UCRNo,//""
                ULDNo,//""
                RecNo,//""
                isDamaged,//1,
                ReturnedAtWHCode,//"",
                ReturnedOn,//DateTime.Now
                AWBPrefix,
                AWBNo,
                isLoaded
            };
                ptype = new SqlDbType[10]
            {
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.Bit,
                SqlDbType.VarChar,
                SqlDbType.DateTime,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.Bit
            };
                if (da.UpdateData("spADDUCRULDDetails", pname, ptype, pvalue))
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
                return false;
            }
            finally
            {
                da = null;
                pname = null;
                pvalue = null;
                ptype = null;
            }
        }
        #endregion

        #region Add_UCR
        public DataSet AddUCRDetails(string SPType, string UCRNo, string TraDt, string ActTraDt, string ActRecDt, 
            string TraCar, string TraCarText, string RecCar, string RecCarText, string TraWH, Int64 TraSubWHID, 
            string FinWH, Int64 FinSubWHID, string Remarks, byte[] Picture, bool isLoaded, string AWBNo, 
            string AWBPrefix, string UCRMode, string UpdatedBy, DateTime UpdatedOn)
        {
            SQLServer da = new SQLServer(constr);
            try
            {


                string[] pname = new string[21]
            {   
                "SPType",
                "UCRNo",
                "TraDt",
                "ActTraDt",
                "ActRecDt",
                "TraCar",
                "TraCarText",
                "RecCar",
                "RecCarText",
                "TraWH",
                "TraSubWHID",
                "FinWH",
                "FinSubWHID",
                "Remarks",
                "Picture",
                "isLoaded",
                "AWBNo",
                "AWBPrefix",
                "UCRMode",
                "UpdatedBy",
                "UpdatedOn"
            };

                object[] pvalue = new object[21]
            {
                 SPType,
                 UCRNo,
                 TraDt,
                 ActTraDt,
                 ActRecDt,
                 TraCar,
                 TraCarText,
                 RecCar,
                 RecCarText,
                 TraWH,
                 TraSubWHID,
                 FinWH,
                 FinSubWHID,
                 Remarks,
                 Picture,
                 isLoaded,
                 AWBNo,
                 AWBPrefix,
                 UCRMode,
                 UpdatedBy,
                 UpdatedOn
                };
                SqlDbType[] ptype = new SqlDbType[21]
            {
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.DateTime,
                SqlDbType.DateTime,
                SqlDbType.DateTime,
                SqlDbType.BigInt,
                SqlDbType.VarChar,
                SqlDbType.BigInt,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.BigInt,
                SqlDbType.VarChar,
                SqlDbType.BigInt,
                SqlDbType.Text,
                SqlDbType.VarBinary,
                SqlDbType.Bit,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.DateTime
            };

                DataSet ds = da.SelectRecords("spcreateUCR", pname, pvalue, ptype);
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
        
        public string generateUCR(string AWBno, string ULDs, string Isloaded, string finalLoc, string remarks,
            DateTime currtime,string UpdatedBy, DateTime UpdatedOn)
        {
            string UCR = "";
            try
            {
                bool isload=false;
                if(Isloaded=="true")
                    isload = true;
                string[] ULDNos = ULDs.Split(',');
                string[] finLoc = finalLoc.Split(',');
                byte[] str = null; 

                DateTime dt1 = new DateTime();
                dt1 = currtime;
                DateTime dt2 = new DateTime();
                dt2 = currtime;
                DateTime dt3 = new DateTime();
                dt3 = currtime;

               
                //AddUCRDetails("", "", "", "", "", "", "", "", "", "", 0, finalLoc, 0, remarks, 0, isload, AWBno);
                foreach (string lc in finLoc)
                {
                    DataSet ds = AddUCRDetails("INSERT", "", dt1.ToString("MM/dd/yyyy"), dt2.ToString("MM/dd/yyyy"), 
                        dt3.ToString("MM/dd/yyyy"), "0", "", "0", "YY", "", 0, lc, 0, remarks, str, isload, AWBno, "","",
                        UpdatedBy, UpdatedOn);
                    UCR = ds.Tables[0].Rows[0][0].ToString();
                }
                bool resultbool = AddAccessoriesToDB("ADD", UCR, "Released", "0", "0", "0", "0");
                bool resultbool1 = AddAccessoriesToDB("ADD", UCR, "Returned", "0", "0", "0", "0");
                bool resultbool2 = AddAccessoriesToDB("ADD", UCR, "Damaged", "0", "0", "0", "0");
              //DateTime dt = new DateTime();
              //dt = currtime;
                foreach (string U in ULDNos)
                {
                    GetDetailsUCRULDbool("ADD", UCR, U, "", false, "", currtime,"","",false);
                }
                return UCR;
            }
                
            catch (Exception ex)
            {
                
            }
            return UCR;
        }
        
        public DataSet AddUCRDetails(string SPType, string UCRNo, string TraDt, string ActTraDt, string ActRecDt, 
            string TraCar, string TraCarText, string RecCar, string RecCarText, string TraWH, Int64 TraSubWHID, 
            string FinWH, Int64 FinSubWHID, string Remarks, DBNull Picture, bool isLoaded, string AWBNo, string AWBPrefix, 
            string UCRMode, string UpdatedBy, DateTime UpdatedOn)
        {
            SQLServer da = new SQLServer(constr);
            try
            {

                string[] pname = new string[21]
            {   
                "SPType",
                "UCRNo",
                "TraDt",
                "ActTraDt",
                "ActRecDt",
                "TraCar",
                "TraCarText",
                "RecCar",
                "RecCarText",
                "TraWH",
                "TraSubWHID",
                "FinWH",
                "FinSubWHID",
                "Remarks",
                "Picture",
                "isLoaded",
                "AWBNo",
                "AWBPrefix",
                "UCRMode",
                "UpdatedBy",
                "UpdatedOn"
            };

                object[] pvalue = new object[21]
            {
                 SPType,
                 UCRNo,
                 TraDt,
                 ActTraDt,
                 ActRecDt,
                 TraCar,
                 TraCarText,
                 RecCar,
                 RecCarText,
                 TraWH,
                 TraSubWHID,
                 FinWH,
                 FinSubWHID,
                 Remarks,
                 Picture,
                 isLoaded,
                 AWBNo,
                 AWBPrefix,
                 UCRMode,
                 UpdatedBy,
                 UpdatedOn
                };
                SqlDbType[] ptype = new SqlDbType[21]
            {
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.DateTime,
                SqlDbType.DateTime,
                SqlDbType.DateTime,
                SqlDbType.BigInt,
                SqlDbType.VarChar,
                SqlDbType.BigInt,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.BigInt,
                SqlDbType.VarChar,
                SqlDbType.BigInt,
                SqlDbType.Text,
                SqlDbType.VarBinary,
                SqlDbType.Bit,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.DateTime
            };

                DataSet ds = da.SelectRecords("spcreateUCR", pname, pvalue, ptype);
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
        #endregion

        #region GetUCRDetails
        public DataSet getUCRDetails(string UCRNo)
        {
            SQLServer da = new SQLServer(constr);
            string[] pname = null;
            object[] pvalue = null;
            SqlDbType[] ptype = null;

            try
            {
                pname = new string[1] { "UCRNo" };
                pvalue = new object[1] { UCRNo };
                ptype = new SqlDbType[1] { SqlDbType.VarChar };
                return da.SelectRecords("spGetDetailsUCR", pname, pvalue, ptype);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                pname = null;
                pvalue = null;
                ptype = null;
            }
        }
        #endregion

        #region List UCRS
        public DataSet ListUCRs(string UCRNo,string ULDNo,DateTime fromDt,DateTime toDate,string TraWHCode,Int64 TraCar,string TraCarText,string FinWHCode,Int64 RecCar,string RecCarText)
        {
            SQLServer da = new SQLServer(constr);
            string[] pname = null;
            object[] pvalue = null;
            SqlDbType[] ptype = null;

            try
            {
                pname = new string[10]
            {   
                "UCRNo",
                "ULDNo",
                "frmDate",
                "toDate",
                "TraWHCode",
                "TraCar",
                "TraCarText",
                "FinWHCode",
                "RecCar",
                "RecCarText",
            };
                pvalue = new object[10]
            {
                UCRNo,
                ULDNo,
                fromDt,
                toDate,
                TraWHCode,
                TraCar,
                TraCarText,
                FinWHCode,
                RecCar,
                RecCarText,
            };
                ptype = new SqlDbType[10]
            {
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.DateTime,
                SqlDbType.DateTime,
                SqlDbType.VarChar,
                SqlDbType.BigInt,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.BigInt,
                SqlDbType.VarChar,
            };
                DataSet dsGetListUCRs = da.SelectRecords("spListUCRs", pname, pvalue, ptype);
                return dsGetListUCRs;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                pname = null;
                pvalue = null;
                ptype = null;
            }
        }
        #endregion

        #region Add UCR Accessories
        public bool AddAccessoriesToDB(string SPType,string UCRNo,string Status, string Nets,string Doors,string Straps,string Fittings)
        {
            SQLServer da = new SQLServer(constr);
            try
            {
                string[] pname = new string[7]
            {   
                "SPType",
                "UCRNo",
                "Status",
                "Nets",
                "Doors",
                "Straps",
                "Fittings"                
            };

                object[] pvalue = new object[7]
            {
                 SPType,
                 UCRNo,
                 Status,
                 Nets,
                 Doors,
                 Straps,
                 Fittings
             };

                SqlDbType[] ptype = new SqlDbType[7]
            {
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.TinyInt,
                SqlDbType.TinyInt,
                SqlDbType.TinyInt,
                SqlDbType.TinyInt
            };

                if (!da.UpdateData("spADDUCRAccessoriesDetails", pname, ptype, pvalue))
                {
                    return false;
                }
                return true;
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

        public DataSet GetAccessoriesFromDB(string SPType, string UCRNo, string Status, string Nets, string Doors, string Straps, string Fittings)
        {
            SQLServer da = new SQLServer(constr);
            try
            {
                string[] pname = new string[7]
            {   
                "SPType",
                "UCRNo",
                "Status",
                "Nets",
                "Doors",
                "Straps",
                "Fittings"                
            };

                object[] pvalue = new object[7]
            {
                 SPType,
                 UCRNo,
                 Status,
                 Nets,
                 Doors,
                 Straps,
                 Fittings
             };

                SqlDbType[] ptype = new SqlDbType[7]
            {
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.TinyInt,
                SqlDbType.TinyInt,
                SqlDbType.TinyInt,
                SqlDbType.TinyInt
            };

                DataSet ds = da.SelectRecords("spADDUCRAccessoriesDetails", pname, pvalue, ptype);
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
        #endregion

        #region Get UCR Report
        public DataSet GetUCRReport(string UCR)
        {
            SQLServer da = new SQLServer(constr);
            try
            {
                string[] pname = new string[1]
            {   
                "UCRNo"
            };

                object[] pvalue = new object[1]
            {
                UCR
            };

                SqlDbType[] ptype = new SqlDbType[1]
            {
                SqlDbType.VarChar
            };
                DataSet dsgetUCRRpt = da.SelectRecords("spGetUCRReport", pname, pvalue, ptype);
                return dsgetUCRRpt;
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

        #region Get UCR ReportNew
        public DataSet GetUCRReportNew(string UCR)
        {
            SQLServer da = new SQLServer(constr);
            string[] pname = null;
            object[] pvalue = null;
            SqlDbType[] ptype = null;

            try
            {
                pname = new string[1]
            {   
                "UCRNo"
            };

                pvalue = new object[1]
            {
                UCR
            };

                ptype = new SqlDbType[1]
            {
                SqlDbType.VarChar
            };
                DataSet dsgetUCRRpt = da.SelectRecords("spGetUCRReportNew", pname, pvalue, ptype);
                return dsgetUCRRpt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                pname = null;
                pvalue = null;
                ptype = null;
            }
        }
        #endregion

        #region SubIATARPT
        public DataSet GetIATACodesReport(string UCR)
        {
            SQLServer da = new SQLServer(constr);
            string[] pname = null;
            object[] pvalue = null;
            SqlDbType[] ptype = null;

            try
            {

                pname = new string[1]
            {   
                "UCRNo"
            };

                pvalue = new object[1]
            {
                UCR
            };

                ptype = new SqlDbType[1]
            {
                SqlDbType.VarChar
            };
                DataSet dsgetUCRRpt = da.SelectRecords("spSubRptIATACodes", pname, pvalue, ptype);
                return dsgetUCRRpt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                pname = null;
                pvalue = null;
                ptype = null;
            }
        }
        #endregion

        #region SubIATARPT
        public DataSet GetUCRULDRpt(string UCR)
        {
            SQLServer da = new SQLServer(constr);
            string[] pname = null;
            object[] pvalue = null;
            SqlDbType[] ptype = null;

            try
            {
                pname = new string[1]
            {   
                "UCRNo"
            };

                pvalue = new object[1]
            {
                UCR
            };

                ptype = new SqlDbType[1]
            {
                SqlDbType.VarChar
            };
                DataSet dsgetUCRRpt = da.SelectRecords("spSubRptUCRULDs", pname, pvalue, ptype);
                return dsgetUCRRpt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                pname = null;
                pvalue = null;
                ptype = null;
            }
        }
        #endregion

        #region Get ULDInfo From OperationULD
        public DataSet GetULDInfo(string AWBNo, string Mode, string FlightNo, string FlightDate)
        {
            SQLServer da = new SQLServer(constr);
            string[] pname = null;
            object[] pvalue = null;
            SqlDbType[] ptype = null;

            try
            {
                pname = new string[4]
            {   
                "AWBNo",
                "Mode",
                "FlightNo",
                "FlightDate"
            };
                pvalue = new object[4]
            {
                AWBNo,//"GET",
                Mode,//""
                FlightNo,
                FlightDate
            };
                ptype = new SqlDbType[4]
            {
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.DateTime
            };
                DataSet dsgetUCRRpt = da.SelectRecords("sp_GetULDInfo", pname, pvalue, ptype);
                return dsgetUCRRpt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                da = null;
                pname = null;
                pvalue = null;
                ptype = null;
            }
        }
        #endregion

    }
}

