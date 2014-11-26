using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Reflection;
using System.Configuration;
//using clsDataLib;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using ProjectSmartCargoManager;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Service : System.Web.Services.WebService
{

    #region Constructor
    public Service () 
    {
        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer da = new SQLServer(Global.GetConnectionString());

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    #endregion

    #region checkInternet
    [WebMethod]
    public bool checkInternet()
    {
        bool flag = false;
        try
        {
            flag = true;
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    #endregion

    #region To chk GPRS/WIFI connection
    [WebMethod]
    //Checks If WebServer is Reachable
    public String connCheck()
    {
        return "CONNECTED";
    }
    #endregion To chk GPRS/WIFI connection

    #region check User Authentication for HH
    [WebMethod]
    public DataSet checkUserAuth(string UserName, string Password, string Station)
    {
        DataSet dsChkAuth = new DataSet();
        try
        {
            SQLServer dbAuth = new SQLServer(Global.GetConnectionString());

            string[] QueryPname = new string[3];
            QueryPname[0] = "UserName";
            QueryPname[1] = "Password";
            QueryPname[2] = "Station";

            object[] QueryValue = new object[3];
            QueryValue[0] = UserName.Trim();
            QueryValue[1] = Password.Trim();
            QueryValue[2] = Station.Trim();

            SqlDbType[] QueryType = new SqlDbType[3];
            QueryType[0] = SqlDbType.NVarChar;
            QueryType[1] = SqlDbType.NVarChar;
            QueryType[2] = SqlDbType.NVarChar;

            dsChkAuth = dbAuth.SelectRecords("spGetUserDetailsTest", QueryPname, QueryValue, QueryType);
        }
        catch (Exception ex)
        {
            return dsChkAuth;
        }
        return dsChkAuth;
    }
    #endregion

    #region get Arrival Transaction with 3 parameter
    [WebMethod]
    public DataSet getArrivalTransaction(string flightNo,string flightDate,string stationCode)
    {
        DataSet dsArrTrans = new DataSet();
        try
        {
            SQLServer dbArrTrans = new SQLServer(Global.GetConnectionString());

            string[] QueryPname = new string[3];
            QueryPname[0] = "Fltno";
            QueryPname[1] = "FltDate";
            QueryPname[2] = "StationCode";

            object[] QueryValue = new object[3];
            QueryValue[0] = flightNo.Trim();
            QueryValue[1] = flightDate.Trim();
            QueryValue[2] = stationCode.Trim();

            SqlDbType[] QueryType = new SqlDbType[3];
            QueryType[0] = SqlDbType.NVarChar;
            QueryType[1] = SqlDbType.NVarChar;
            QueryType[2] = SqlDbType.NVarChar;

            dsArrTrans = dbArrTrans.SelectRecords("Sp_GetArrival", QueryPname, QueryValue, QueryType);
        }
        catch (Exception ex)
        {
            return dsArrTrans;
        }
        return dsArrTrans;
    }
    #endregion

    #region checking status of AWB 
    [WebMethod]
    public DataSet checkParallelAWB(DataSet dsData)
    {
        DataSet dschkAWB = new DataSet();
        DataSet allData = new DataSet();
        try
        {
            SQLServer dbchkAWB = new SQLServer(Global.GetConnectionString());
            DataTable dt = new DataTable();
            dt.Columns.Add("AWBNo");
            dt.Columns.Add("operationType");
            dt.Columns.Add("FltNo");


            for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
            {
                string[] QueryPname = new string[2];
                QueryPname[0] = "AWBNo";
                QueryPname[1] = "Station";

                object[] QueryValue = new object[2];
                QueryValue[0] = dsData.Tables[0].Rows[i][0].ToString();
                QueryValue[1] = dsData.Tables[0].Rows[i][1].ToString();

                SqlDbType[] QueryType = new SqlDbType[2];
                QueryType[0] = SqlDbType.NVarChar;
                QueryType[1] = SqlDbType.NVarChar;

                dschkAWB = dbchkAWB.SelectRecords("spchkParallelAWBforHH", QueryPname, QueryValue, QueryType);
                if (dschkAWB.Tables[0].Rows.Count > 0)
                {
                    //allData.Merge(dschkAWB.Tables[0]);
                    for(int j=0;j<dschkAWB.Tables[0].Rows.Count ;j++)
                    {
                    DataRow dr = dt.NewRow();
                    dr["AWBNo"] = dschkAWB.Tables[0].Rows[j][0].ToString();
                    dr["operationType"] = dschkAWB.Tables[0].Rows[j][1].ToString();
                    dr["FltNo"] = dschkAWB.Tables[0].Rows[j][2].ToString();
                    dt.Rows.Add(dr);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return allData;
        }
        return allData;
    }

    #endregion

    #region Upload Updated Arrival
    [WebMethod]
    public DataSet UploadUpdatedArrival(string FltNo, string AWBNo,int RecPcs)
    {
        DataSet dsArrTrans = new DataSet();
        try
        {
            SQLServer dbArrTrans = new SQLServer(Global.GetConnectionString());

            string[] QueryPname = new string[4];
            QueryPname[0] = "FltNo";
            QueryPname[1] = "AWBNo";
            QueryPname[2] = "RecPcs";
            QueryPname[3] = "UpdatedOn";

            object[] QueryValue = new object[4];
            QueryValue[0] = FltNo.Trim();
            QueryValue[1] = AWBNo.Trim();
            QueryValue[2] =Convert.ToInt32(RecPcs);
            QueryValue[3] = System.DateTime.Now.ToString();


            SqlDbType[] QueryType = new SqlDbType[4];
            QueryType[0] = SqlDbType.NVarChar;
            QueryType[1] = SqlDbType.NVarChar;
            QueryType[2] = SqlDbType.Int;
            QueryType[3] = SqlDbType.DateTime;

            dsArrTrans = dbArrTrans.SelectRecords("spUpdateArrivalByHH", QueryPname, QueryValue, QueryType);
        }
        catch (Exception ex)
        {
            return dsArrTrans;
        }
        return dsArrTrans;
    }
    #endregion

    #region get Arrival Transaction with 2 parameter
    [WebMethod]
    public DataSet getArrivalTransactionAcctoDateAndStation(string maxDate, string stationCode)
    {
        DataSet dsArrTrans = new DataSet();
        try
        {
            SQLServer dbArrTrans = new SQLServer(Global.GetConnectionString());

            string[] QueryPname = new string[2];
            QueryPname[0] = "maxDate";
            QueryPname[1] = "StationCode";

            object[] QueryValue = new object[2];
            QueryValue[0] = maxDate.Trim();
            QueryValue[1] = stationCode.Trim();

            SqlDbType[] QueryType = new SqlDbType[2];
            QueryType[0] = SqlDbType.NVarChar;
            QueryType[1] = SqlDbType.NVarChar;

            dsArrTrans = dbArrTrans.SelectRecords("spgetArrivalTransForHH", QueryPname, QueryValue, QueryType);
            //if (dsArrTrans != null)
            //{
            //    if (dsArrTrans.Tables.Count > 0)
            //    {
            //        if (dsArrTrans.Tables[0].Rows.Count > 0)
            //        {
            //            DataTable table1 = dsArrTrans.Tables[0];
            //            DataView view1 = table1.DefaultView;
            //            view1.Sort = "AWBno";
            //            dsArrTrans.Tables.Remove(dsArrTrans.Tables[0]);
            //            dsArrTrans.Tables.Add(view1.ToTable());


            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            return dsArrTrans;
        }
        return dsArrTrans;
    }
    #endregion

    #region get All Flight for HH
    [WebMethod]
    public DataSet getAllFlights(string maxDate,string Station)
    {
        DataSet dsFlight = new DataSet();
        try
        {
            SQLServer dbFlight = new SQLServer(Global.GetConnectionString());
            
            string[] QueryPname = new string[2];
            QueryPname[0] = "station";
            QueryPname[1] = "maxDate";

            object[] QueryValue = new object[2];
            QueryValue[0] = Station.Trim();
            QueryValue[1] = maxDate.Trim();

            SqlDbType[] QueryType = new SqlDbType[2];
            QueryType[0] = SqlDbType.NVarChar;
            QueryType[1] = SqlDbType.NVarChar;

            dsFlight = dbFlight.SelectRecords("spgetflightbyStationHH", QueryPname, QueryValue, QueryType);
        }
        catch (Exception ex)
        {
            return dsFlight;
        }
        return dsFlight;
    }
    #endregion

    #region get All user of Station for HH
    [WebMethod]
    public DataSet getAllUser(string maxDate, string Station)
    {
        DataSet dsUser = new DataSet();
        try
        {
            SQLServer dbUser = new SQLServer(Global.GetConnectionString());

            string[] QueryPname = new string[2];
            QueryPname[0] = "maxDate";
            QueryPname[1] = "station";

            object[] QueryValue = new object[2];
            QueryValue[0] = maxDate.Trim();
            QueryValue[1] = Station.Trim();

            SqlDbType[] QueryType = new SqlDbType[2];
            QueryType[0] = SqlDbType.NVarChar;
            QueryType[1] = SqlDbType.NVarChar;

            dsUser = dbUser.SelectRecords("spgetAllUserbyDateforHH", QueryPname, QueryValue, QueryType);
        }
        catch (Exception ex)
        {
            return dsUser;
        }
        return dsUser;
    }
    #endregion

    #region Upload DOMaster
    [WebMethod]
    public bool uploadDO(string AgentName,string AWBNumber,string FlightNumber,int TotalPieces,int ExpectedPieces,int ActualPieces,string DONumber,string ReciversName,decimal ActualWeight,string IssuedTo,string IssueName,DateTime IssueDate,DateTime UpdatedOn,string UpdatedBy,int HAWBNumber,string Consignee,string Status,string OperationType,string remark,string sign,string Contenttype,string RemainingPieces,decimal ExpectedWeight)
    {
        bool res = false;
        try
        {
            SQLServer dbRes = new SQLServer(Global.GetConnectionString());

            string[] QueryPname = new string[23];
            QueryPname[0] = "AgentName";
            QueryPname[1] = "AWBNumber";
            QueryPname[2] = "FlightNumber";
            QueryPname[3] = "TotalPieces";
            QueryPname[4] = "ExpectedPieces";
            QueryPname[5] = "ActualPieces";
            QueryPname[6] = "DONumber";
            QueryPname[7] = "ReciversName";
            QueryPname[8] = "ActualWeight";
            QueryPname[9] = "IssuedTo";
            QueryPname[10] = "IssueName";
            QueryPname[11] = "IssueDate";
            QueryPname[12] = "UpdatedOn";
            QueryPname[13] = "UpdatedBy";
            QueryPname[14] = "HAWBNumber";
            QueryPname[15] = "Consignee";
            QueryPname[16] = "Status";
            QueryPname[17] = "OperationType";
            QueryPname[18] = "remark";
            QueryPname[19] = "sign";
            QueryPname[20] = "Contenttype";
            QueryPname[21] = "RemainingPieces";
            QueryPname[22] = "ExpectedWeight";


            object[] QueryValue = new object[23];
            QueryValue[0] = AgentName.Trim();
            QueryValue[1] = AWBNumber.Trim();
            QueryValue[2] = FlightNumber.Trim();
            QueryValue[3] = TotalPieces;
            QueryValue[4] = ExpectedPieces;
            QueryValue[5] = ActualPieces;
            QueryValue[6] = DONumber.Trim();
            QueryValue[7] = ReciversName.Trim();
            QueryValue[8] = ActualWeight;
            QueryValue[9] = IssuedTo.Trim();
            QueryValue[10] = IssueName.Trim();
            QueryValue[11] = IssueDate;
            QueryValue[12] = UpdatedOn;
            QueryValue[13] = UpdatedBy.Trim();
            QueryValue[14] = HAWBNumber;
            QueryValue[15] = Consignee.Trim();
            QueryValue[16] = Status.Trim();
            QueryValue[17] = OperationType.Trim();
            QueryValue[18] = remark.Trim();
            QueryValue[19] = sign.Trim();
            QueryValue[20] = Contenttype.Trim();
            QueryValue[21] = RemainingPieces.Trim();
            QueryValue[22] = ExpectedWeight;



            SqlDbType[] QueryType = new SqlDbType[23];
            QueryType[0] = SqlDbType.NVarChar;
            QueryType[1] = SqlDbType.NVarChar;
            QueryType[2] = SqlDbType.NVarChar;
            QueryType[3] = SqlDbType.Int;
            QueryType[4] = SqlDbType.Int;
            QueryType[5] = SqlDbType.Int;
            QueryType[6] = SqlDbType.NVarChar;
            QueryType[7] = SqlDbType.NVarChar;
            QueryType[8] = SqlDbType.Decimal;
            QueryType[9] = SqlDbType.NVarChar;
            QueryType[10] = SqlDbType.NVarChar;
            QueryType[11] = SqlDbType.DateTime;
            QueryType[12] = SqlDbType.DateTime;
            QueryType[13] = SqlDbType.NVarChar;
            QueryType[14] = SqlDbType.Int;
            QueryType[15] = SqlDbType.NVarChar;
            QueryType[16] = SqlDbType.NVarChar;
            QueryType[17] = SqlDbType.NVarChar;
            QueryType[18] = SqlDbType.NVarChar;
            QueryType[19] = SqlDbType.NVarChar;
            QueryType[20] = SqlDbType.NVarChar;
            QueryType[21] = SqlDbType.NVarChar;
            QueryType[22] = SqlDbType.Decimal;

            res = dbRes.InsertData("spinsertDOMaster", QueryPname, QueryType, QueryValue);
        }
        catch (Exception ex)
        {
            res = false;
        }
        return res;
    }
    #endregion

    #region get data for DO which is arrived by web
    [WebMethod]
    public DataSet getDataforDOforWebArrival(string fltdate, string fltNo, string Station)
    {
        DataSet dsArrDO = new DataSet();
        try
        {
            SQLServer dbArrDo = new SQLServer(Global.GetConnectionString());

            string[] QueryPname = new string[5];
            QueryPname[0] = "DONo";
            QueryPname[1] = "AWBNo";
            QueryPname[2] = "FlightNo";
            QueryPname[3] = "StationCode";
            QueryPname[4] = "Flightdate";

            object[] QueryValue = new object[5];
            QueryValue[0] = "";
            QueryValue[1] = "";
            QueryValue[2] = fltNo.Trim();
            QueryValue[3] = Station.Trim();
            QueryValue[4] = fltdate.Trim();

            SqlDbType[] QueryType = new SqlDbType[5];
            QueryType[0] = SqlDbType.NVarChar;
            QueryType[1] = SqlDbType.NVarChar;
            QueryType[2] = SqlDbType.NVarChar;
            QueryType[3] = SqlDbType.NVarChar;
            QueryType[4] = SqlDbType.NVarChar;

            dsArrDO = dbArrDo.SelectRecords("SpGetDeliveryNew", QueryPname, QueryValue, QueryType);
        }
        catch (Exception ex)
        {
            return dsArrDO;
        }
        return dsArrDO;
    }
    #endregion

    #region get Agent list for HH
    [WebMethod]
    public DataSet getAgentList(string maxDate,string Station)
    {
        DataSet dsAgent = new DataSet();
        try
        {
            SQLServer dbAgent = new SQLServer(Global.GetConnectionString());

            string[] QueryPname = new string[2];
            QueryPname[0] = "station";
            QueryPname[1] = "maxDate";

            object[] QueryValue = new object[2];
            QueryValue[0] = Station.Trim();
            QueryValue[1] = maxDate.Trim();

            SqlDbType[] QueryType = new SqlDbType[2];
            QueryType[0] = SqlDbType.NVarChar;
            QueryType[1] = SqlDbType.NVarChar;

            dsAgent = dbAgent.SelectRecords("spgetAgentbyStationHH", QueryPname, QueryValue, QueryType);
        }
        catch (Exception ex)
        {
            return dsAgent;
        }
        return dsAgent;
    }
    #endregion

    #region addOffloadDataHH
    [WebMethod]
    public bool addOffloadDataHH(DataSet ds)
    {
        bool flag = false;
        try
        {
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        SQLServer dtb = new SQLServer(Global.GetConnectionString());
                        string procedure = "SP_AddOffloadDAtaHH";
                        string[] paramname = new string[11];
                        object[] paramvalue = new object[11];
                        SqlDbType[] paramtype = new SqlDbType[11];

                        #region ParamPrepare
                        paramname[0] = "AWBNo";
                        paramname[1] = "PieceID";
                        paramname[2] = "OffloadFrom";
                        paramname[3] = "LoadedInto";
                        paramname[4] = "DeviceID";
                        paramname[5] = "Location";
                        paramname[6] = "OffloadedOn";
                        paramname[7] = "Latitude";
                        paramname[8] = "Longitude";
                        paramname[9] = "OffloadFltDate";
                        paramname[10] = "ActFlightDate";

                        paramtype[0] = SqlDbType.VarChar;
                        paramtype[1] = SqlDbType.VarChar;
                        paramtype[2] = SqlDbType.VarChar;
                        paramtype[3] = SqlDbType.VarChar;
                        paramtype[4] = SqlDbType.VarChar;
                        paramtype[5] = SqlDbType.VarChar;
                        paramtype[6] = SqlDbType.DateTime;
                        paramtype[7] = SqlDbType.VarChar;
                        paramtype[8] = SqlDbType.VarChar;
                        paramtype[9] = SqlDbType.DateTime;
                        paramtype[10] = SqlDbType.DateTime;
                        #endregion

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            paramvalue[0] = dr[1].ToString();
                            paramvalue[1] = dr[2].ToString();
                            paramvalue[2] = dr[3].ToString();
                            paramvalue[3] = dr[4].ToString();
                            paramvalue[4] = dr[5].ToString();
                            paramvalue[5] = dr[6].ToString();
                            paramvalue[6] = dr[7].ToString();
                            paramvalue[7] = dr[8].ToString();
                            paramvalue[8] = dr[9].ToString();
                            paramvalue[9] = dr[10].ToString();
                            paramvalue[10] = dr[11].ToString();

                            flag = dtb.InsertData(procedure, paramname, paramtype, paramvalue);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    #endregion

    //    #region Get Placement Tag Details

//    [WebMethod]

//    public DataSet GetTagDetails(string txtval, string updatedby)
//    {
//        DataSet ds = new DataSet();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database  da = new Database ();

//        try
//        {

//            string[] QueryPname = new string[1];
//            object[] QueryValue = new object[1];
//            SqlDbType[] QueryType = new SqlDbType[1];


//            QueryPname[0] = "TagId";
//            //QueryPname[1] = "UpdatedOn";

//            QueryType[0] = SqlDbType.VarChar;
//            //QueryType[1] = SqlDbType.DateTime;
//            QueryValue[0] = txtval;

//            ds = da.SelectRecords("SP_GetTagDetailsHH", QueryPname, QueryValue, QueryType);

//            return ds;
//        }
//        catch (Exception ex)
//        {
//            return ds;
//        }

//    }

//    #endregion Get Placement Tag Details

//    #region Get Location master data from central DB to send to device DB

//    [WebMethod]

//    public DataSet GetLocationMasterData(string  maxdate)
//    {
//        DataSet DSLocMastData = new DataSet();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database  da = new Database ();
//        try
//        {
//            string[] QueryPname = new string[1];
//            object[] QueryValue = new object[1];
//            SqlDbType[] QueryType = new SqlDbType[1];


//            QueryPname[0] = "Maxdate";
//            QueryType[0] = SqlDbType.VarChar ;
//            QueryValue[0] = maxdate;

//            DSLocMastData = da.SelectRecords("SP_GetLocationMasterDataforHH", QueryPname, QueryValue, QueryType);

//            return DSLocMastData;
//        }
//        catch (Exception ex)
//        {
//            return DSLocMastData;
//        }
//    }

//    #endregion Get Location master data from central DB to send to device DB

//    #region Get Brand master data from central DB to send to device DB

//    [WebMethod]

//    public DataSet GetBrandMasterData(string maxdate)
//    {
//        DataSet DSBrandMastData = new DataSet();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database da = new Database();
//        try
//        {
//            string[] QueryPname = new string[1];
//            object[] QueryValue = new object[1];
//            SqlDbType[] QueryType = new SqlDbType[1];


//            QueryPname[0] = "Maxdate";
//            QueryType[0] = SqlDbType.VarChar;
//            QueryValue[0] = maxdate;

//            DSBrandMastData = da.SelectRecords("SP_GetBrandMasterDataforHH", QueryPname, QueryValue, QueryType);

//            return DSBrandMastData;
//        }
//        catch (Exception ex)
//        {
//            return DSBrandMastData;
//        }
//    }

//    #endregion Get Brand master data from central DB to send to device DB

//    #region Confirm/Save Bin/Pallet Placement
//    //to uploadlocal device db to central and confirm the location placement 
//    [WebMethod]

//    public bool UpLoadLocationPlacementData(DataSet DSConfirmplacement)
//    {
//        //to upload local device DB to central
//        //DataSet ds=new DataSet ();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database  da = new Database ();
//        Boolean Result = false;
//        try
//        {

//            string[] QueryPname = new string[10];
//            object[] QueryValue = new object[10];
//            SqlDbType[] QueryType = new SqlDbType[10];

            
//            //select palletnumber,LocationCode,LocationDate,updatedby,updatedon,Rowlevel,forkliftdriver,expectedlocation,isdiverted,Isrelocated
//            QueryPname[0] = "palletnumber";
//            QueryPname[1] = "LocationCode";
//            QueryPname[2] = "LocationDate";
//            QueryPname[3] = "updatedby";
//            QueryPname[4] = "updatedon";
//            QueryPname[5] = "Rowlevel";
//            QueryPname[6] = "forkliftdriver";
//            QueryPname[7] = "expectedlocation";
//            QueryPname[8] = "isdiverted";
//            QueryPname[9] = "Isrelocated";

//            QueryType[0] = SqlDbType.VarChar;
//            QueryType[1] = SqlDbType.VarChar;
//            QueryType[2] = SqlDbType.DateTime;
//            QueryType[3] = SqlDbType.VarChar;
//            QueryType[4] = SqlDbType.DateTime;
//            QueryType[5] = SqlDbType.VarChar;
//            QueryType[6] = SqlDbType.VarChar;
//            QueryType[7] = SqlDbType.VarChar;
//            QueryType[8] = SqlDbType.Bit ;
//            QueryType[9] = SqlDbType.Bit;

//            for (int i = 0; i < DSConfirmplacement.Tables[0].Rows.Count; i++)
//            {
//                QueryValue[0] = DSConfirmplacement.Tables[0].Rows[i][0].ToString();
//                QueryValue[1] = DSConfirmplacement.Tables[0].Rows[i][1].ToString();
//                QueryValue[2] = DSConfirmplacement.Tables[0].Rows[i][2];
//                QueryValue[3] = DSConfirmplacement.Tables[0].Rows[i][3].ToString();
//                QueryValue[4] = DSConfirmplacement.Tables[0].Rows[i][4];
//                QueryValue[5] = DSConfirmplacement.Tables[0].Rows[i][5].ToString();
//                QueryValue[6] = DSConfirmplacement.Tables[0].Rows[i][6].ToString();
//                QueryValue[7] = DSConfirmplacement.Tables[0].Rows[i][7].ToString();
//                QueryValue[8] = DSConfirmplacement.Tables[0].Rows[i][8].ToString();
//                QueryValue[9] = DSConfirmplacement.Tables[0].Rows[i][9].ToString();



//                Result = da.UpdateData("SP_ConfirmPalletPlacementHH", QueryPname, QueryValue, QueryType);
//            }
//            return Result;
//        }
//        catch (Exception ex)
//        {
//            return Result;
//        }

//    }

//    #endregion Confirm/Save Bin Placement
    
//    #region  UpLoad Location Movement Data
//    //to uploadlocal device db to central and confirm the location movement /Relocation 
//    [WebMethod]

//    public bool UpLoadLocationMovementData(DataSet DSConfirmplacement)
//    {
//        //to upload local device DB to central
//        //DataSet ds=new DataSet ();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database da = new Database();
//        Boolean Result = false;
//        try
//        {

//            string[] QueryPname = new string[7];
//            object[] QueryValue = new object[7];
//            SqlDbType[] QueryType = new SqlDbType[7];

//            //select palletnumber,InventoryID,PrevLocCode,CurrLocCode,updatedby,updatedon,Rowlevel from tblMaterialMovement ";
            
//            QueryPname[0] = "palletnumber";
//            QueryPname[1] = "InventoryID";
//            QueryPname[2] = "PrevLocCode";
//            QueryPname[3] = "CurrLocCode";
//            QueryPname[4] = "updatedby";
//            QueryPname[5] = "updatedon";
//            QueryPname[6] = "Rowlevel";
            
//            QueryType[0] = SqlDbType.VarChar;
//            QueryType[1] = SqlDbType.VarChar;
//            QueryType[2] = SqlDbType.VarChar;
//            QueryType[3] = SqlDbType.VarChar;
//            QueryType[4] = SqlDbType.VarChar;
//            QueryType[5] = SqlDbType.DateTime;
//            QueryType[6] = SqlDbType.VarChar;
           
//            for (int i = 0; i < DSConfirmplacement.Tables[0].Rows.Count; i++)
//            {
//                QueryValue[0] = DSConfirmplacement.Tables[0].Rows[i][0].ToString();
//                QueryValue[1] = DSConfirmplacement.Tables[0].Rows[i][1].ToString();
//                QueryValue[2] = DSConfirmplacement.Tables[0].Rows[i][2].ToString();
//                QueryValue[3] = DSConfirmplacement.Tables[0].Rows[i][3].ToString();
//                QueryValue[4] = DSConfirmplacement.Tables[0].Rows[i][4].ToString();
//                QueryValue[5] = DSConfirmplacement.Tables[0].Rows[i][5];
//                QueryValue[6] = DSConfirmplacement.Tables[0].Rows[i][6].ToString();
                
//                Result = da.UpdateData("SP_UploadRelocationDataHH", QueryPname, QueryValue, QueryType);
//            }
//            return Result;
//        }
//        catch (Exception ex)
//        {
//            return Result;
//        }

//    }

//    #endregion UpLoad Location Movement Data

//    #region  UpLoad Load Plan/pickup Data
//    //to uploadlocal device db to central and confirm the location movement /Relocation 
//    [WebMethod]

//    public bool UploadLoadPlanData(DataSet DSloadplandata)
//    {
//        //to upload local device DB to central
//        //DataSet ds=new DataSet ();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database da = new Database();
//        Boolean Result = false;
//        try
//        {

//            string[] QueryPname = new string[9];
//            object[] QueryValue = new object[9];
//            SqlDbType[] QueryType = new SqlDbType[9];

//            //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

//            QueryPname[0] = "palletnumber";
//            QueryPname[1] = "brand";
//            QueryPname[2] = "batchno";
//            QueryPname[3] = "Actualquantity";
//            QueryPname[4] = "ispicked";
//            QueryPname[5] = "isplanned";
//            QueryPname[6] = "updatedby";
//            QueryPname[7] = "updatedon";
//            QueryPname[8] = "slipnumber";

            
//            QueryType[0] = SqlDbType.VarChar;
//            QueryType[1] = SqlDbType.VarChar;
//            QueryType[2] = SqlDbType.VarChar;
//            QueryType[3] = SqlDbType.Int;
//            QueryType[4] = SqlDbType.Bit;
//            QueryType[5] = SqlDbType.Bit ;
//            QueryType[6] = SqlDbType.VarChar;
//            QueryType[7] = SqlDbType.DateTime ;
//            QueryType[8] = SqlDbType.Int;

//            for (int i = 0; i < DSloadplandata.Tables[0].Rows.Count; i++)
//            {
//                QueryValue[0] = DSloadplandata.Tables[0].Rows[i][0].ToString();
//                QueryValue[1] = DSloadplandata.Tables[0].Rows[i][1].ToString();
//                QueryValue[2] = DSloadplandata.Tables[0].Rows[i][2].ToString();
//                QueryValue[3] = DSloadplandata.Tables[0].Rows[i][3];
//                QueryValue[4] = DSloadplandata.Tables[0].Rows[i][4];
//                QueryValue[5] = DSloadplandata.Tables[0].Rows[i][5];
//                QueryValue[6] = DSloadplandata.Tables[0].Rows[i][6].ToString();
//                QueryValue[7] = DSloadplandata.Tables[0].Rows[i][7];
//                QueryValue[8] = DSloadplandata.Tables[0].Rows[i][8];

//                Result = da.UpdateData("SP_UploadLoadPlanDataHH", QueryPname, QueryValue, QueryType);
//            }
//            return Result;
//        }
//        catch (Exception ex)
//        {
//            return Result;
//        }

//    }

//    #endregion UpLoad Load Plan/pickup Data

//    #region Get Loading sheet  Details

//    [WebMethod]

//    public DataSet GetLoadingSheetDetails(string txtval)
//    {
//        DataSet ds = new DataSet();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database da = new Database();

//        try
//        {

//            string[] QueryPname = new string[1];
//            object[] QueryValue = new object[1];
//            SqlDbType[] QueryType = new SqlDbType[1];


//            QueryPname[0] = "TagId";
//            //QueryPname[1] = "UpdatedOn";

//            QueryType[0] = SqlDbType.VarChar;
//            //QueryType[1] = SqlDbType.DateTime;
//            QueryValue[0] = txtval;

//            ds = da.SelectRecords("SP_GetLoadingSheetDetailsHH", QueryPname, QueryValue, QueryType);

//            return ds;
//        }
//        catch (Exception ex)
//        {
//            return ds;
//        }

//    }

//    #endregion  Get Loading Sheet Details

//    #region Get Pickup Slip Number Details

//    [WebMethod]

//    public DataSet GetPickupSlipDetails(string txtval)
//    {
//        DataSet ds = new DataSet();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database da = new Database();

//        try
//        {

//            string[] QueryPname = new string[1];
//            object[] QueryValue = new object[1];
//            SqlDbType[] QueryType = new SqlDbType[1];


//            QueryPname[0] = "TagId";
//            //QueryPname[1] = "UpdatedOn";

//            QueryType[0] = SqlDbType.VarChar;
//            //QueryType[1] = SqlDbType.DateTime;
//            QueryValue[0] = txtval;

//            ds = da.SelectRecords("SP_GetPickupSlipDetailsHH", QueryPname, QueryValue, QueryType);

//            return ds;
//        }
//        catch (Exception ex)
//        {
//            return ds;
//        }

//    }

//    #endregion  Get Pickup Slip Number Details

//    #region Get Pickup Pallet Number  Details

//    [WebMethod]

//    public DataSet GetPickupPalletDetails(string txtval)
//    {
//        DataSet ds = new DataSet();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database da = new Database();

//        try
//        {

//            string[] QueryPname = new string[1];
//            object[] QueryValue = new object[1];
//            SqlDbType[] QueryType = new SqlDbType[1];


//            QueryPname[0] = "TagId";
//            //QueryPname[1] = "UpdatedOn";

//            QueryType[0] = SqlDbType.VarChar;
//            //QueryType[1] = SqlDbType.DateTime;
//            QueryValue[0] = txtval;

//            ds = da.SelectRecords("SP_GetPickupPalletDetailsHH", QueryPname, QueryValue, QueryType);

//            return ds;
//        }
//        catch (Exception ex)
//        {
//            return ds;
//        }

//    }

//    #endregion  Get Pickup Pallet Number Details    

//    #region Get Loading Sheet Pallet Number  Details

//    [WebMethod]

//    public DataSet GetLoadingSheetPalletDetails(string txtval,string txt2val)
//    {
//        DataSet ds = new DataSet();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database da = new Database();

//        try
//        {

//            string[] QueryPname = new string[2];
//            object[] QueryValue = new object[2];
//            SqlDbType[] QueryType = new SqlDbType[2];


//            QueryPname[0] = "TagId";
//            QueryPname[1] = "Slipnumber";

//            QueryType[0] = SqlDbType.VarChar;
//            QueryType[1] = SqlDbType.VarChar;

//            QueryValue[0] = txtval;
//            QueryValue[1] = txt2val;

//            ds = da.SelectRecords("SP_GetLoadingSheetPalletDetailsHH", QueryPname, QueryValue, QueryType);

//            return ds;
//        }
//        catch (Exception ex)
//        {
//            return ds;
//        }

//    }

//    #endregion  Get Loading Sheet Pallet Number Details
    
//    #region  UpLoad Loading Sheet details Data
//    //to uploadlocal device db to central and confirm the location movement /Relocation 
//    [WebMethod]

//    public bool UploadLoadingSheetdeatailsData(DataSet DSloadplandata)
//    {
//        //to upload local device DB to central
//        //DataSet ds=new DataSet ();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database da = new Database();
//        Boolean Result = false;
//        try
//        {

//            string[] QueryPname = new string[7];
//            object[] QueryValue = new object[7];
//            SqlDbType[] QueryType = new SqlDbType[7];

//            //slipnumber,palletno,loadedcases,breakagebtls,rejectedbtls,updatedby,updatedon

//            QueryPname[0] = "slipnumber";
//            QueryPname[1] = "palletno";
//            QueryPname[2] = "loadedcases";
//            QueryPname[3] = "breakagebtls";
//            QueryPname[4] = "rejectedbtls";
//            QueryPname[5] = "updatedby";
//            QueryPname[6] = "updatedon";
//            //QueryPname[7] = "updatedon";

//            QueryType[0] = SqlDbType.VarChar;
//            QueryType[1] = SqlDbType.VarChar;
//            QueryType[2] = SqlDbType.Int;
//            QueryType[3] = SqlDbType.Int;
//            QueryType[4] = SqlDbType.Int;
//            QueryType[5] = SqlDbType.VarChar;
//            QueryType[6] = SqlDbType.VarChar;
//           // QueryType[7] = SqlDbType.DateTime;

//            for (int i = 0; i < DSloadplandata.Tables[0].Rows.Count; i++)
//            {
//                QueryValue[0] = DSloadplandata.Tables[0].Rows[i][0].ToString();
//                QueryValue[1] = DSloadplandata.Tables[0].Rows[i][1].ToString();
//                QueryValue[2] = DSloadplandata.Tables[0].Rows[i][2];
//                QueryValue[3] = DSloadplandata.Tables[0].Rows[i][3];
//                QueryValue[4] = DSloadplandata.Tables[0].Rows[i][4];
//                QueryValue[5] = DSloadplandata.Tables[0].Rows[i][5].ToString() ;
//                QueryValue[6] = DSloadplandata.Tables[0].Rows[i][6].ToString();
//                //QueryValue[7] = DSloadplandata.Tables[0].Rows[i][7];

//                Result = da.UpdateData("SP_UploadLoadingSheetdeatailsDataHH", QueryPname, QueryValue, QueryType);
//            }
//            return Result;
//        }
//        catch (Exception ex)
//        {
//            return Result;
//        }

//    }

//    #endregion UpLoad Loading Sheet details Data

//    #region  UpLoad Rejection Combination details Data
//    //to uploadlocal device db to central and confirm the location movement /Relocation 
//    [WebMethod]

//    public bool UploadRejectionCombinationData(DataSet DSRejCombidata)
//    {
//        //to upload local device DB to central
//        //DataSet ds=new DataSet ();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database da = new Database();
//        Boolean Result = false;
//        try
//        {

//            string[] QueryPname = new string[10];
//            object[] QueryValue = new object[10];
//            SqlDbType[] QueryType = new SqlDbType[10];

//            //select slipnumber,brand,batchno,rejectedbtls,combined,createdby,createdon,updatedby,updatedon,iscombined from tblrejectioncombination 

//            QueryPname[0] = "slipnumber";
//            QueryPname[1] = "brand";
//            QueryPname[2] = "batchno";
//            QueryPname[3] = "rejectedbtls";
//            QueryPname[4] = "combined";
//            QueryPname[5] = "createdby";
//            QueryPname[6] = "createdon";
//            QueryPname[7] = "updatedby";
//            QueryPname[8] = "updatedon";
//            QueryPname[9] = "iscombined";

//            QueryType[0] = SqlDbType.VarChar;
//            QueryType[1] = SqlDbType.VarChar;
//            QueryType[2] = SqlDbType.VarChar;
//            QueryType[3] = SqlDbType.Int;
//            QueryType[4] = SqlDbType.Int;
//            QueryType[5] = SqlDbType.VarChar;
//            QueryType[6] = SqlDbType.DateTime;
//            QueryType[7] = SqlDbType.VarChar;
//            QueryType[8] = SqlDbType.DateTime;
//            QueryType[9] = SqlDbType.Bit;

//            for (int i = 0; i < DSRejCombidata.Tables[0].Rows.Count; i++)
//            {
//                QueryValue[0] = DSRejCombidata.Tables[0].Rows[i][0].ToString();
//                QueryValue[1] = DSRejCombidata.Tables[0].Rows[i][1].ToString();
//                QueryValue[2] = DSRejCombidata.Tables[0].Rows[i][2].ToString();
//                QueryValue[3] = DSRejCombidata.Tables[0].Rows[i][3];
//                QueryValue[4] = DSRejCombidata.Tables[0].Rows[i][4];
//                QueryValue[5] = DSRejCombidata.Tables[0].Rows[i][5].ToString();
//                QueryValue[6] = DSRejCombidata.Tables[0].Rows[i][6];
//                QueryValue[7] = DSRejCombidata.Tables[0].Rows[i][7].ToString();
//                QueryValue[8] = DSRejCombidata.Tables[0].Rows[i][8];
//                QueryValue[9] = DSRejCombidata.Tables[0].Rows[i][9].ToString();

//                Result = da.UpdateData("SP_UploadRejectionCombiDataHH", QueryPname, QueryValue, QueryType);
//            }
//            return Result;
//        }
//        catch (Exception ex)
//        {
//            return Result;
//        }

//    }

//    #endregion UpLoad Rejection Combination details Data

//    #region Get GetLocation Details for validation
//    //this is to chk that if a pallet is already placed and the next pallet to be placed above the already placed one are both of same brand,batch and dest.
//    [WebMethod]

//    public DataSet GetLocationDetails(string Zone, string Lane,string Column)
//    {
//        DataSet ds = new DataSet();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database da = new Database();

//        try
//        {

//            string[] QueryPname = new string[3];
//            object[] QueryValue = new object[3];
//            SqlDbType[] QueryType = new SqlDbType[3];


//            QueryPname[0] = "Zone";
//            QueryPname[1] = "Lane";
//            QueryPname[2] = "Column";

//            QueryType[0] = SqlDbType.VarChar;
//            QueryType[1] = SqlDbType.VarChar;
//            QueryType[2] = SqlDbType.VarChar;

//            QueryValue[0] = Zone;
//            QueryValue[1] = Lane;
//            QueryValue[2] = Column;

//            ds = da.SelectRecords("SP_GetLocTagBrandBatchDestColumnValidHH", QueryPname, QueryValue, QueryType);

//            return ds;
//        }
//        catch (Exception ex)
//        {
//            return ds;
//        }

//    }

//    #endregion Get Placement Tag Details

//    #region Upload PalletSlip Inventory Entry Data
//    //to uploadlocal device db to central and confirm the inventry entry for pallet slip 
//    [WebMethod]

//    public string  UploadPalletSlipInventoryEntryData(DataSet DSConfirmplacement)
//    {


//        //to upload local device DB to central
//        //DataSet ds=new DataSet ();
//        string Constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//        Database da = new Database();
//        //Boolean Result = false;
//        string Result = "";
//        try
//        {


           
//            string[] QueryPname = new string[17];
//            object[] QueryValue = new object[17];
//            SqlDbType[] QueryType = new SqlDbType[17];


 
////select BrandId,BatchNumber,BatchDate,palletnumber,CasesCount,LocationCode,LocationDate,
////createdby,createdon,updatedby,updatedon,IsDispatched,IsPint,Rowlevel,forkliftdriver,Destination,Type

//            QueryPname[0] = "BrandId";
//            QueryPname[1] = "BatchNumber";
//            QueryPname[2] = "BatchDate";
//            QueryPname[3] = "palletnumber";
//            QueryPname[4] = "CasesCount";
//            QueryPname[5] = "LocationCode";
//            QueryPname[6] = "LocationDate";
//            QueryPname[7] = "createdby";
//            QueryPname[8] = "createdon";
//            QueryPname[9] = "updatedby";
//            QueryPname[10] = "updatedon";
//            QueryPname[11] = "IsDispatched";
//            QueryPname[12] = "IsPint";
//            QueryPname[13] = "Rowlevel";
//            QueryPname[14] = "forkliftdriver";
//            QueryPname[15] = "Destination";
//            QueryPname[16] = "Type";
            
//            QueryType[0] = SqlDbType.Int;
//            QueryType[1] = SqlDbType.Int;
//            QueryType[2] = SqlDbType.DateTime;
//            QueryType[3] = SqlDbType.VarChar;
//            QueryType[4] = SqlDbType.Int;
//            QueryType[5] = SqlDbType.VarChar;
//            QueryType[6] = SqlDbType.VarChar;
//            QueryType[7] = SqlDbType.VarChar;
//            QueryType[8] = SqlDbType.VarChar;
//            QueryType[9] = SqlDbType.VarChar;
//            QueryType[10] = SqlDbType.VarChar;
//            QueryType[11] = SqlDbType.Bit;
//            QueryType[12] = SqlDbType.Bit;
//            QueryType[13] = SqlDbType.VarChar;
//            QueryType[14] = SqlDbType.VarChar;
//            QueryType[15] = SqlDbType.VarChar;
//            QueryType[16] = SqlDbType.VarChar;
            
//            for (int i = 0; i < DSConfirmplacement.Tables[0].Rows.Count; i++)
//            {
//                QueryValue[0] = DSConfirmplacement.Tables[0].Rows[i][0];
//                QueryValue[1] = DSConfirmplacement.Tables[0].Rows[i][1];
//                QueryValue[2] = DSConfirmplacement.Tables[0].Rows[i][2];
//                QueryValue[3] = DSConfirmplacement.Tables[0].Rows[i][3].ToString();
//                QueryValue[4] = DSConfirmplacement.Tables[0].Rows[i][4];
//                QueryValue[5] = DSConfirmplacement.Tables[0].Rows[i][5].ToString();
//                QueryValue[6] = DSConfirmplacement.Tables[0].Rows[i][6].ToString();
//                QueryValue[7] = DSConfirmplacement.Tables[0].Rows[i][7].ToString();
//                QueryValue[8] = DSConfirmplacement.Tables[0].Rows[i][8].ToString();
//                QueryValue[9] = DSConfirmplacement.Tables[0].Rows[i][9].ToString();
//                QueryValue[10] = DSConfirmplacement.Tables[0].Rows[i][10].ToString();
//                QueryValue[11] = DSConfirmplacement.Tables[0].Rows[i][11];
//                QueryValue[12] = DSConfirmplacement.Tables[0].Rows[i][12];
//                QueryValue[13] = DSConfirmplacement.Tables[0].Rows[i][13].ToString();
//                QueryValue[14] = DSConfirmplacement.Tables[0].Rows[i][14].ToString();
//                QueryValue[15] = DSConfirmplacement.Tables[0].Rows[i][15].ToString();
//                QueryValue[16] = DSConfirmplacement.Tables[0].Rows[i][16].ToString();



//                 Result = UpdateData("SP_UploadPalletSlipInventoryEntryHH", QueryPname, QueryValue, QueryType);
                
//            }
//            return Result;
//        }
//        catch (Exception ex)
//        {
//            return Result;
//        }

//    }

//    #endregion Upload PalletSlip Inventory Entry Data

//    #region UpdateStoreProcedureRecord
//    public string  UpdateData( string UpdateQuery, string[] QueryPName, object[] QueryValues, SqlDbType[] QueryTypes)
//    {
//        bool status = false;
//        string  res = "";
//        try
//        {

//            string  ConnectionString = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
//            SqlCommand command = new SqlCommand();
//            SqlConnection connection = new SqlConnection();
//            SqlDataAdapter adapter = new SqlDataAdapter();
//            DataSet dataSet = new DataSet();
//            connection.ConnectionString = ConnectionString;



//            //command.CommandText = SelectQuery;
//            command.CommandType = CommandType.StoredProcedure;
//            command.Connection = connection;
//            //adapter.SelectCommand = this.command;
//            command.CommandText = UpdateQuery;
//            command.Connection = connection;
//            adapter.InsertCommand = command;

//            for (int i = 0; i < QueryValues.Length; i++)
//            {
//                command.Parameters.Add(QueryPName[i], QueryTypes[i]).Value = QueryValues[i];
//            }

//            SqlParameter countParameter = new SqlParameter("@ResOutput", SqlDbType.VarChar, 100);
//            countParameter.Direction = ParameterDirection.Output;
//            command.Parameters.Add(countParameter);


//            if (connection.State == ConnectionState.Closed)
//                connection.Open();
//             command.ExecuteNonQuery();
//            //adapter.Fill(dataSet);
             
//            res = Convert.ToString(command.Parameters["@ResOutput"].Value.ToString());

//           // status = true;
//        }
//        catch (SqlException SqlEx)
//        {
//            Console.Out.WriteLine("Sql Exception : " + SqlEx.Message + SqlEx.StackTrace);
//        }
//        catch (Exception ex)
//        {
//            Console.Out.WriteLine("Exception : " + ex.Message + ex.StackTrace);
//        }
//        return res;
//        //return status;
//    }

//    #endregion UpdateStoreProcedureRecord

}
