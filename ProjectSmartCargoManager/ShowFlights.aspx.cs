using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;

/*

 2012-06-12  vinayak
 2012-06-19  vinayak
 2012-06-25  vinayak
 2012-07-24  vinayak
 2012-07-25  vinayak
 2012-07-30  vinayak
 2012-08-03  vinayak
 2012-08-06  vinayak
*/


namespace ProjectSmartCargoManager
{
    public partial class ShowFlights : System.Web.UI.Page
    {
        ShowFlightsBAL objBAL = new ShowFlightsBAL();
        DataSet dsShowFlight = new DataSet("ShowFlights_5");
        DateTime dtCurrentDate = DateTime.Now;
        int dateOffset = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            // FltNumber  FltOrigin  FltDestination   DeptTime  ArrTime  FltDate
            
            if (!IsPostBack)
            {
                Session["showFltShipDate"] = (DateTime)Session["IT"];
                Session["Destination"] = Request.QueryString["Dest"].ToString();

                if (Session["SingleRow"]==null)
                {
                    Session["SingleRow"] = "N";
                }
                dtCurrentDate = (DateTime)Session["showFltShipDate"];
                if (Request.QueryString != null)
                {
                    //Querystring SingleRow = Y means that this form is called from RouteDetails grid.
                    if (Request.QueryString["SingleRow"] != null && Request.QueryString["SingleRow"].ToString() == "Y")
                    {
                        Session["SingleRow"] = "Y";
                    }
                    else
                    {
                        Session["SingleRow"] = "N";
                    }
                    //Get shipment date from query string.
                    if (Request.QueryString["shipdate"] != null && Request.QueryString["shipdate"] != "")
                    {
                        if (DateTime.TryParseExact(Request.QueryString["shipdate"],"dd/MM/yyyy HH:mm",null, 
                            System.Globalization.DateTimeStyles.None,out dtCurrentDate))
                        {
                            Session["showFltShipDate"] = dtCurrentDate;
                        }
                    }
                }
                Session["ShowFlightFlag"] = "Y";
                FillGrid();
                if (Session["SingleRow"].ToString() == "Y")
                {
                    if (DateTime.TryParseExact(Session["FltDate"].ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtCurrentDate))
                    {
                        if (dtCurrentDate.ToString("dd/MM/yyyy") == Convert.ToDateTime(Session["IT"].ToString()).ToString("dd/MM/yyyy"))
                        {   //If flight date = todays date then set current time as flight date.
                            dtCurrentDate = Convert.ToDateTime(Session["IT"].ToString());
                        }
                        Session["showFltShipDate"] = dtCurrentDate;
                    } 
                    ShowFlightsInGridForSingleRow();
                }
                else
                {
                    ShowFlightsInGrid();
                }
                HideControlsOnDemoFlag();
            }
            if (Session["showFltShipDate"] != null)
            {
                dtCurrentDate = (DateTime)Session["showFltShipDate"];
            }

            HighlightDataGridRows();
        }

        public bool ShowFlightsInGrid()
        {
            try
            {
                if (Session["Origin"] != null)
                {
                    if (Session["Destination"] != null && Session["Destination"].ToString() != "")
                    {

                        string origin = Session["Origin"].ToString();
                        string destination = Session["Destination"].ToString();
                        DataSet dsResult = new DataSet("ShowFlights_6");
                        string errormessage = "";

                        if (objBAL.GetFlightList(origin, destination, dateOffset, ref dsResult, ref errormessage, dtCurrentDate))
                        {
                            Session["RateExists"] = dsResult.Tables[1].Rows[0][0].ToString();
                            FormatRecords(ref dsResult);
                            AddByRouteWise(origin, destination, ref dsResult, ref errormessage);
                            FormatRecords(ref dsResult);

                            if (dsResult == null)
                            {
                                LBLStatus.Text = "No record found";
                                return true;
                            }
                            else if (dsResult.Tables.Count==0)
                            {
                                LBLStatus.Text = "No record found";
                                return true;
                            }
                            else if (dsResult.Tables[0].Rows.Count==0)
                            {
                                LBLStatus.Text = "No record found";
                                return true;
                            }

                            GRDShowFlights.DataSource = dsResult.Copy();
                            GRDShowFlights.DataBind();

                            Session["dsShowFlights"] = dsResult.Copy();
                            dsShowFlight = dsResult.Copy();
                            return true;
                        }
                        else
                        {
                            LBLStatus.Text = "Error while getting flight list :" + errormessage;
                            return false;
                        }

                    }
                    else
                    {
                        LBLStatus.Text = "Destination is invalid.";
                        return false;
                    }
                }
                else
                {

                    LBLStatus.Text = "Origin is invalid.";
                    return false;
                }

            }
            catch (Exception ex)
            {
                LBLStatus.Text = "" + ex.Message;
                return false;
            }
        }

        public bool ShowFlightsInGridForSingleRow()
        {
            try
            {
                if (Session["FltOrigin"] != null)
                {
                    if (Session["FltDestination"] != null && Session["FltDestination"].ToString() != "")
                    {

                        string origin = Session["FltOrigin"].ToString();
                        string destination = Session["FltDestination"].ToString();
                        DataSet dsResult = new DataSet("ShowFlights_7");
                        string errormessage = "";

                        if (objBAL.GetFlightList(origin, destination,dateOffset, ref dsResult, ref errormessage, dtCurrentDate))
                        {
                            FormatRecordsSingleRow(ref dsResult);
                            if (dsResult == null)
                            {
                                LBLStatus.Text = "No record found";
                                return true;
                            }
                            else if (dsResult.Tables.Count == 0)
                            {
                                LBLStatus.Text = "No record found";
                                return true;
                            }
                            else if (dsResult.Tables[0].Rows.Count == 0)
                            {
                                LBLStatus.Text = "No record found";
                                return true;
                            }

                            GRDShowFlights.DataSource = dsResult.Copy();
                            GRDShowFlights.DataBind();

                            Session["dsShowFlights"] = dsResult.Copy();
                            dsShowFlight = dsResult.Copy();
                            return true;
                        }
                        else
                        {
                            LBLStatus.Text = "Error while getting flight list :" + errormessage;
                            return false;
                        }

                    }
                    else
                    {
                        LBLStatus.Text = "Destination is invalid.";
                        return false;
                    }
                }
                else
                {

                    LBLStatus.Text = "Origin is invalid.";
                    return false;
                }

            }
            catch (Exception ex)
            {
                LBLStatus.Text = "" + ex.Message;
                return false;
            }
        }

        public void CreateRouteTable()
        {
            DataSet dsRoute = new DataSet("ShowFlights_8");
            dsRoute.Tables.Add();
            dsRoute.Tables[0].Columns.Add("Source");
            dsRoute.Tables[0].Columns.Add("Destination");
            dsRoute.Tables[0].Columns.Add("Flight");
            dsRoute.Tables[0].Columns.Add("DeptTime");
            dsRoute.Tables[0].Columns.Add("ArrTime");
            dsRoute.Tables[0].Columns.Add("FltDate");
            dsRoute.Tables[0].Columns.Add("Serial");
            dsRoute.Tables[0].Columns.Add("RatePerKg");
            dsRoute.Tables[0].Columns.Add("Partner");

            Session["dsRoute"] = dsRoute.Copy();
        }

        public void AddByRouteWise(string org,string dest,ref DataSet dsResult,ref string errormessage)
        {
            try
            {
                DataSet dsRoute = new DataSet("ShowFlights_9");
                CreateRouteTable();
                DataSet dsTempRoute = new DataSet("ShowFlights_10");
                dsTempRoute = ((DataSet)Session["dsRoute"]).Clone();
                int lastSerial = 1;
                
                if (objBAL.GetRouteData(org, dest, ref dsRoute, ref errormessage))
                {
                    foreach (DataRow dsRouteRow in dsRoute.Tables[0].Rows)
                    {
                        string[] strRoute = dsRouteRow[0].ToString().Split(new char[] { '-' });
                        string viadest = "";
                        string viaorg = "";
                        //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        //DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                        DateTime dtIndianTime = (DateTime)Session["showFltShipDate"];
                        int hr = dtIndianTime.Hour;
                        int min = dtIndianTime.Minute;

                        dsTempRoute = ((DataSet)Session["dsRoute"]).Clone();
                        bool found = true;
                        string flightlist = "", Partner = "";
                        DateTime dtValidDate = dtCurrentDate;
                        decimal Rate = 0;

                        for (int i = 0; i < strRoute.Length-1; i++)
                        {
                            viaorg = strRoute[i];
                            viadest = strRoute[i+1];
                            DataSet dsRouteFlight = new DataSet("ShowFlights_11");
                            
                            if (dtValidDate.Date == dtCurrentDate.Date)
                                dtValidDate = dtCurrentDate;
                            
                            if (objBAL.GetFlightList(viaorg, viadest,dateOffset, ref dsRouteFlight, ref errormessage, dtValidDate))
                            {
                                FormatRecords(viaorg,viadest,ref dsRouteFlight, hr, min, 1);

                                if (dsRouteFlight != null && dsRouteFlight.Tables.Count != 0 && dsRouteFlight.Tables[0].Rows.Count != 0)
                                {
                                    DataRow dsTempRouteRow = dsTempRoute.Tables[0].NewRow();

                                    dsTempRouteRow["Source"] = dsRouteFlight.Tables[0].Rows[0]["FltOrigin"].ToString();
                                    dsTempRouteRow["Destination"] = dsRouteFlight.Tables[0].Rows[0]["FltDestination"].ToString();
                                    dsTempRouteRow["Flight"] = dsRouteFlight.Tables[0].Rows[0]["FltNumber"].ToString();
                                    dsTempRouteRow["DeptTime"] = dsRouteFlight.Tables[0].Rows[0]["DeptTime"].ToString();
                                    dsTempRouteRow["ArrTime"] = dsRouteFlight.Tables[0].Rows[0]["ArrTime"].ToString();
                                    dsTempRouteRow["FltDate"] = dsRouteFlight.Tables[0].Rows[0]["FltDate"].ToString();
                                    dsTempRoute.Tables[0].Rows.Add(dsTempRouteRow);

                                    flightlist += " " + dsTempRouteRow["Flight"].ToString();

                                    Partner += dsRouteFlight.Tables[0].Rows[0]["Partner"].ToString() + "/";
                                    //Rate = Rate + Convert.ToDecimal(dsRouteFlight.Tables[0].Rows[0]["RatePerKg"]);

                                    if (Convert.ToString(Session["RateExists"]) == "1")
                                    {
                                        dsTempRouteRow["RatePerKg"] = dsRouteFlight.Tables[0].Rows[0]["RatePerKg"].ToString();
                                        Rate = Convert.ToDecimal(dsRouteFlight.Tables[0].Rows[0]["RatePerKg"]);
                                    }
                                    else
                                    {
                                        dsTempRouteRow["RatePerKg"] = Rate;
                                        if (dsRouteFlight.Tables[0].Rows[0]["RatePerKg"] != null &&
                                            dsRouteFlight.Tables[0].Rows[0]["RatePerKg"].ToString() != "")
                                        {
                                            Rate = Rate + Convert.ToDecimal(dsRouteFlight.Tables[0].Rows[0]["RatePerKg"]);
                                        }
                                    }

                                    string arrtime = dsTempRouteRow["ArrTime"].ToString();
                                    //dtCurrentDate = Convert.ToDateTime(dsTempRouteRow["FltDate"]);
                                    Array Arr = new Array[3];
                                    Arr = dsTempRouteRow["FltDate"].ToString().Split('/');
                                    string strDate = Arr.GetValue(1).ToString().PadLeft(2, '0') + "/" + Arr.GetValue(0).ToString().PadLeft(2, '0') + "/" + Arr.GetValue(2).ToString();
                                    dtValidDate = DateTime.ParseExact(strDate, "MM/dd/yyyy", null);
                                    hr = int.Parse(arrtime.Substring(0, arrtime.IndexOf(":")));
                                    min = int.Parse(arrtime.Substring(arrtime.IndexOf(":") + 1));
                                                                        
                                }
                                else
                                {
                                    found = false;
                                    break;
                                }
                            }
                            else
                            {
                                found = false;
                                break;
                            }

                        }

                        if (found)
                        {
                            //FltNumber  FltOrigin  FltDestination   DeptTime  ArrTime  FltDate
                            DataRow dsResultRow = dsResult.Tables[0].NewRow();

                            dsResultRow["ScheduleID"] = "0";
                            dsResultRow["FltNumber"] = flightlist;
                            dsResultRow["FltOrigin"] = org;
                            dsResultRow["FltDestination"] = dest;
                            dsResultRow["Serial"] = "" + lastSerial;
                            dsResultRow["RatePerKg"] = Rate;

                            if (Partner.Length > 0)
                                dsResultRow["Partner"] = Partner.Substring(0, Partner.Length - 1);
                            else
                                dsResultRow["Partner"] = "";

                            if (dsTempRoute != null && dsTempRoute.Tables.Count > 0 && dsTempRoute.Tables[0].Rows.Count > 0)
                            {
                                dsResultRow["DeptTime"] = dsTempRoute.Tables[0].Rows[0]["DeptTime"].ToString();
                                dsResultRow["ArrTime"] = dsTempRoute.Tables[0].Rows[dsTempRoute.Tables[0].Rows.Count - 1]["ArrTime"].ToString();
                                dsResultRow["FltDate"] = dsTempRoute.Tables[0].Rows[0]["FltDate"].ToString();
                            }
                            else
                            {
                                dsResultRow["DeptTime"] = "";
                                dsResultRow["ArrTime"] = "";
                                dsResultRow["FltDate"] = "";
                            }

                            dsResult.Tables[0].Rows.Add(dsResultRow);


                            foreach (DataRow dsTempRouteRow in dsTempRoute.Tables[0].Rows)
                                dsTempRouteRow["Serial"] = lastSerial;

                            lastSerial++;

                            DataSet dsRouteMain = new DataSet("ShowFlights_30");
                            dsRouteMain = ((DataSet)Session["dsRoute"]);
                            dsRouteMain.Merge(dsTempRoute);

                            Session["dsRoute"] = dsRouteMain;
                        }


                    }

                }


            }
            catch (Exception ex)
            {
                LBLStatus.Text = "Error :" + ex.Message;
            }
        }

        public void FormatRecords(string org,string dest,ref DataSet dsResult,int hr,int min,int allowedhr)
        {
            int i = 0;
            string ScheduleID = "";
            DataSet dsNewResult = new DataSet("ShowFlights_12");
            dsNewResult = dsResult.Clone();
            bool blOrignFlound, blDestFound;
            blOrignFlound = blDestFound = false;

            for (int index = 0; index < dsResult.Tables[0].Rows.Count; index++)
            {

                DataRow row = dsResult.Tables[0].Rows[index];

                if (ScheduleID == "")
                {
                    if (row["FltOrigin"].ToString() != org) //Session["Origin"].ToString())
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                    }

                    ScheduleID = row["ScheduleID"].ToString();
                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == dest)//Session["Destination"].ToString())
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);

                }
                else if (ScheduleID.Trim() == row["ScheduleID"].ToString())
                {
                    dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["FltDestination"] = row["FltDestination"].ToString();
                    dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["ArrTime"] = row["ArrTime"].ToString();

                    if (row["FltDestination"].ToString() == dest)//Session["Destination"].ToString())
                    {
                        blDestFound = true;

                        for (int y = index; y < dsResult.Tables[0].Rows.Count; y++)
                        {
                            if (dsResult.Tables[0].Rows[y]["FltOrigin"].ToString() != Session["Origin"].ToString())
                                continue;
                            else
                            {
                                index = y - 1;
                                ScheduleID = "";
                                blOrignFlound = false;
                                blDestFound = false;
                                break;
                            }

                        }
                    }
                }
                else
                {
                    if (row["FltOrigin"].ToString() != org)//Session["Origin"].ToString())
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                        blDestFound = false;
                    }

                    ScheduleID = row["ScheduleID"].ToString();


                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == dest)//Session["Destination"].ToString())
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);
                }

                i++;

            }

            dsResult = dsNewResult.Copy();
            DataView dv = new DataView(dsResult.Tables[0].Copy());
            dv.Sort = "DeptTime";

            dsResult = new DataSet("ShowFlights_13");
            dsResult.Tables.Add(dv.ToTable().Copy());



            DataTable dt = new DataTable("ShowFlights_1");
            dt = dsResult.Tables[0].Clone();
            foreach (DataRow row in dsResult.Tables[0].Rows)
            {
                string depttime = row["DeptTime"].ToString();
                int curhr = int.Parse(depttime.Substring(0, depttime.IndexOf(":")));
                int curmin = int.Parse(depttime.Substring(depttime.IndexOf(":") + 1));
                int day = 0;//int.Parse(row["FltDate"].ToString().Trim() == "" ? DateTime.Now.ToString("dd/MM/yyyy") : row["FltDate"].ToString().Trim()).;

                if(row["FltDate"].ToString().Trim() == "")
                    day = dtCurrentDate.Day;
                else
                    day=int.Parse(row["FltDate"].ToString().Trim().Substring(0,row["FltDate"].ToString().IndexOf('/')));

                bool canAdd = false;

                if (day < (dtCurrentDate.Day + 1))
                {

                    if (curhr < (hr + allowedhr))
                    {
                        if (curhr > hr)
                        {
                            int hrdiff = hr - curhr;

                            if (((hrdiff * 60) - min + curmin) > (allowedhr * 60))
                                canAdd = true;
                        }
                    }
                    else
                        canAdd = true;

                }
                else
                    canAdd = true;

                if (canAdd)
                {
                    DataRow rw = dt.NewRow();

                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        rw[k] = row[k];
                    }

                    dt.Rows.Add(rw);
                }
                
            }

            dsResult = new DataSet("ShowFlights_14");
            dsResult.Tables.Add(dt);
        }
 
        public void FormatRecords(ref DataSet dsResult)
        {
            int i = 0;
            string ScheduleID = "";
            DataSet dsNewResult = new DataSet("ShowFlights_15");
            dsNewResult = dsResult.Clone();
            bool blOrignFlound, blDestFound;
            blOrignFlound = blDestFound = false;
                        
            string[] strPrvDt = null;
            int intPrevDay = 0;
            string[] strCrDate = null;

            //foreach (DataRow row in dsResult.Tables[0].Rows)
            //{
            for (int index = 0; index < dsResult.Tables[0].Rows.Count; index++)
            {   
                DataRow row = dsResult.Tables[0].Rows[index];

                if (row["ScheduleID"].ToString().Trim() != "" && row["ScheduleID"].ToString().Trim() != "0")
                {
                    strCrDate = row["FltDate"].ToString().Split('/');
                    int intCurentDay = int.Parse(strCrDate[0]);


                    if (intCurentDay != intPrevDay && ScheduleID.Trim() == row["ScheduleID"].ToString())
                        ScheduleID = "";

                    if (ScheduleID == "")
                    {
                        if (row["FltOrigin"].ToString() != Session["Origin"].ToString())
                        {
                            continue;
                        }
                        else
                        {
                            blOrignFlound = true;
                        }

                        ScheduleID = row["ScheduleID"].ToString();
                        DataRow rw = dsNewResult.Tables[0].NewRow();

                        for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                        {
                            rw[j] = row[j];
                        }

                        if (row["FltDestination"].ToString() == Session["Destination"].ToString())
                        {
                            blDestFound = true;
                        }

                        dsNewResult.Tables[0].Rows.Add(rw);

                        //ScheduleID = "";
                        strPrvDt = row["FltDate"].ToString().Split('/');
                        intPrevDay = int.Parse(strPrvDt[0]);

                    }
                    else if (ScheduleID.Trim() == row["ScheduleID"].ToString())
                    {
                        if (!blDestFound)
                        {
                            dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["FltDestination"] = row["FltDestination"].ToString();
                            dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["ArrTime"] = row["ArrTime"].ToString();

                            if (row["FltDestination"].ToString() == Session["Destination"].ToString())
                            {
                                blDestFound = true;

                                for (int y = index; y < dsResult.Tables[0].Rows.Count; y++)
                                {
                                    if (dsResult.Tables[0].Rows[y]["FltOrigin"].ToString() != Session["Origin"].ToString())
                                        continue;
                                    else
                                    {
                                        index = y - 1;
                                        ScheduleID = "";
                                        blOrignFlound = false;
                                        blDestFound = false;
                                        break;
                                    }

                                }
                            }

                        }

                    }
                    else
                    {
                        if (row["FltOrigin"].ToString() != Session["Origin"].ToString())
                        {
                            continue;
                        }
                        else
                        {
                            blOrignFlound = true;
                            blDestFound = false;
                        }

                        ScheduleID = row["ScheduleID"].ToString();


                        DataRow rw = dsNewResult.Tables[0].NewRow();

                        for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                        {
                            rw[j] = row[j];
                        }

                        if (row["FltDestination"].ToString() == Session["Destination"].ToString())
                        {
                            blDestFound = true;
                        }

                        dsNewResult.Tables[0].Rows.Add(rw);
                    }

                    i++;
                }
            }

            dsResult = dsNewResult.Copy();
            DataView dv = new DataView(dsResult.Tables[0].Copy());
            dv.Sort = "FltDate,DeptTime";

            dsResult = new DataSet("ShowFlights_16");
            dsResult.Tables.Add(dv.ToTable().Copy());

            DateTime dtIndianTime;
            if (Session["showFltShipDate"] != null)
                dtIndianTime = (DateTime)Session["showFltShipDate"];
            else
                dtIndianTime = (DateTime)Session["IT"];

            DataTable dt = new DataTable("ShowFlights_2");
                dt = dsResult.Tables[0].Clone();
            foreach (DataRow row in dsResult.Tables[0].Rows)
            {
                string depttime = row["DeptTime"].ToString();
                int hr = int.Parse(depttime.Substring(0, depttime.IndexOf(":")));
                int min = int.Parse(depttime.Substring(depttime.IndexOf(":") + 1));
                int day = 0;//int.Parse(row["FltDate"].ToString().Trim() == "" ? DateTime.Now.ToString("dd/MM/yyyy") : row["FltDate"].ToString().Trim()).;

                if(row["FltDate"].ToString().Trim() == "")
                    day = dtCurrentDate.Day;
                else
                    day=int.Parse(row["FltDate"].ToString().Trim().Substring(0,row["FltDate"].ToString().IndexOf('/')));

                bool canAdd = false;

                if (day != (dtCurrentDate.Day + 1))
                {
                    if (hr < (dtIndianTime.Hour + 2))
                    {
                        if (hr > dtIndianTime.Hour)
                        {
                            int hrdiff = dtIndianTime.Hour - hr;

                            if (((hrdiff * 60) - dtIndianTime.Minute + min) > 120)
                                canAdd = true;
                        }
                    }
                    else
                        canAdd = true;

                }
                else
                    canAdd = true;

                if (canAdd == false)
                    row["ColorFlag"] = "1";

                //Modification to show all the flights

                DataRow rw = dt.NewRow();

                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    rw[k] = row[k];
                }

                dt.Rows.Add(rw);

                //if (canAdd)       
                //{
                //    DataRow rw = dt.NewRow();

                //    for (int k = 0; k < dt.Columns.Count; k++)
                //    {
                //        rw[k] = row[k];
                //    }

                //    dt.Rows.Add(rw);
                //}
            }

            dsResult = new DataSet("ShowFlights_17");
            dsResult.Tables.Add(dt);
        }

        public void FormatRecordsSingleRow(ref DataSet dsResult)
        {
            int i = 0;
            string ScheduleID = "";
            DataSet dsNewResult = new DataSet("ShowFlights_18");
            dsNewResult = dsResult.Clone();
            bool blOrignFlound, blDestFound;
            blOrignFlound = blDestFound = false;

            string[] strPrvDt = null;
            int intPrevDay = 0;
            string[] strCrDate = null;

            //foreach (DataRow row in dsResult.Tables[0].Rows)
            //{
            for (int index = 0; index < dsResult.Tables[0].Rows.Count; index++)
            {

                DataRow row = dsResult.Tables[0].Rows[index];

                strCrDate = row["FltDate"].ToString().Split('/');
                int intCurentDay = int.Parse(strCrDate[0]);


                if (intCurentDay != intPrevDay && ScheduleID.Trim() == row["ScheduleID"].ToString())
                    ScheduleID = "";

                if (ScheduleID == "")
                {
                    if (row["FltOrigin"].ToString() != Session["FltOrigin"].ToString())
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                    }

                    ScheduleID = row["ScheduleID"].ToString();
                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == Session["FltDestination"].ToString())
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);

                    //ScheduleID = "";
                    strPrvDt = row["FltDate"].ToString().Split('/');
                    intPrevDay = int.Parse(strPrvDt[0]);

                }
                else if (ScheduleID.Trim() == row["ScheduleID"].ToString())
                {
                    if (!blDestFound)
                    {
                        dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["FltDestination"] = row["FltDestination"].ToString();
                        dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["ArrTime"] = row["ArrTime"].ToString();

                        if (row["FltDestination"].ToString() == Session["Destination"].ToString())
                        {
                            blDestFound = true;

                            for (int y = index; y < dsResult.Tables[0].Rows.Count; y++)
                            {
                                if (dsResult.Tables[0].Rows[y]["FltOrigin"].ToString() != Session["FltOrigin"].ToString())
                                    continue;
                                else
                                {
                                    index = y - 1;
                                    ScheduleID = "";
                                    blOrignFlound = false;
                                    blDestFound = false;
                                    break;
                                }

                            }
                        }

                    }

                }
                else
                {
                    if (row["FltOrigin"].ToString() != Session["FltOrigin"].ToString())
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                        blDestFound = false;
                    }

                    ScheduleID = row["ScheduleID"].ToString();


                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == Session["FltDestination"].ToString())
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);
                }

                i++;

            }

            dsResult = dsNewResult.Copy();
            DataView dv = new DataView(dsResult.Tables[0].Copy());
            dv.Sort = "FltDate,DeptTime";

            dsResult = new DataSet("ShowFlights_19");
            dsResult.Tables.Add(dv.ToTable().Copy());


            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById(Convert.ToString(Session["TZ"]));
            DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);


            DataTable dt = new DataTable("ShowFlights_3");
                dt = dsResult.Tables[0].Clone();
            foreach (DataRow row in dsResult.Tables[0].Rows)
            {
                string depttime = row["DeptTime"].ToString();
                int hr = int.Parse(depttime.Substring(0, depttime.IndexOf(":")));
                int min = int.Parse(depttime.Substring(depttime.IndexOf(":") + 1));
                int day = 0;//int.Parse(row["FltDate"].ToString().Trim() == "" ? DateTime.Now.ToString("dd/MM/yyyy") : row["FltDate"].ToString().Trim()).;

                if (row["FltDate"].ToString().Trim() == "")
                    day = dtCurrentDate.Day;
                else
                    day = int.Parse(row["FltDate"].ToString().Trim().Substring(0, row["FltDate"].ToString().IndexOf('/')));

                bool canAdd = false;

                //if (day != (dtCurrentDate.Day + 1))
                //If selected date is current not future date.
                if (day == dtCurrentDate.Day && dtCurrentDate <= dtIndianTime)
                {
                    if (hr < (dtIndianTime.Hour + 2))
                    {
                        if (hr > dtIndianTime.Hour)
                        {
                            int hrdiff = dtIndianTime.Hour - hr;

                            if (((hrdiff * 60) - dtIndianTime.Minute + min) > 120)
                                canAdd = true;
                        }
                    }
                    else
                        canAdd = true;

                }
                else
                    canAdd = true;

                if (canAdd == false)
                    row["ColorFlag"] = "1";

                DataRow rw = dt.NewRow();

                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    rw[k] = row[k];
                }

                dt.Rows.Add(rw);

                //if (canAdd)
                //{
                //    DataRow rw = dt.NewRow();

                //    for (int k = 0; k < dt.Columns.Count; k++)
                //    {
                //        rw[k] = row[k];
                //    }

                //    dt.Rows.Add(rw);
                //}
            }

            dsResult = new DataSet("ShowFlights_20");
            dsResult.Tables.Add(dt);
        }
        
        public void FillGrid()
        {
            try
            {
                DataSet dsShowFlights = new DataSet("ShowFlights_21");
                dsShowFlights.Tables.Add();

                dsShowFlights.Tables[0].Columns.Add("FltNumber");
                dsShowFlights.Tables[0].Columns.Add("FltOrigin");
                dsShowFlights.Tables[0].Columns.Add("FltDestination");
                dsShowFlights.Tables[0].Columns.Add("DeptTime");
                dsShowFlights.Tables[0].Columns.Add("ArrTime");
                dsShowFlights.Tables[0].Columns.Add("FltDate");
                dsShowFlights.Tables[0].Columns.Add("ScheduleID");
                dsShowFlights.Tables[0].Columns.Add("AcceptedWt");
                dsShowFlights.Tables[0].Columns.Add("Serial");
                dsShowFlights.Tables[0].Columns.Add("CargoCapacity");
                dsShowFlights.Tables[0].Columns.Add("EquipmentNo");
                dsShowFlights.Tables[0].Columns.Add("RatePerKg");
                dsShowFlights.Tables[0].Columns.Add("Partner");
                dsShowFlights.Tables[0].Columns.Add("ColorFlag");
                
                dsShowFlights.Tables[0].Rows.Add(new string[] { "", "", "", "", "", "","","","","","" });

                Session["dsShowFlights"] = dsShowFlights.Copy();

                GRDShowFlights.DataSource = dsShowFlights.Copy();
                GRDShowFlights.DataBind();

            }
            catch (Exception ex)
            {
                LBLStatus.Text = "" + ex.Message;
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            Session["FlightsChanged"] = "1";
            Session["shipDate"] = null;
        }

        protected void CHKSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DataSet dsSelectedFlights = new DataSet("ShowFlights_22");
                dsSelectedFlights.Tables.Add();
                dsSelectedFlights.Tables[0].Columns.Add("FltOrigin");
                dsSelectedFlights.Tables[0].Columns.Add("FltDestination");
                dsSelectedFlights.Tables[0].Columns.Add("FltNumber");
                dsSelectedFlights.Tables[0].Columns.Add("FltTime");                
                dsSelectedFlights.Tables[0].Columns.Add("FltDate");
                dsSelectedFlights.Tables[0].Columns.Add("Pcs");
                dsSelectedFlights.Tables[0].Columns.Add("Wt");
                dsSelectedFlights.Tables[0].Columns.Add("Status");
                dsSelectedFlights.Tables[0].Columns.Add("Accepted");
                dsSelectedFlights.Tables[0].Columns.Add("AcceptedPcs");
                dsSelectedFlights.Tables[0].Columns.Add("AcceptedWt");
                dsSelectedFlights.Tables[0].Columns.Add("ScheduleID");
                dsSelectedFlights.Tables[0].Columns.Add("ChrgWt");
                dsSelectedFlights.Tables[0].Columns.Add("Carrier");

                Session["dsSelectedFlights"] = dsSelectedFlights.Copy();


                dsSelectedFlights = (DataSet)Session["dsSelectedFlights"];
                dsSelectedFlights.Tables[0].Rows.Clear();

                for (int i = 0; i < GRDShowFlights.Rows.Count; i++)
                {
                    if (((CheckBox)GRDShowFlights.Rows[i].FindControl("CHKSelect")).Checked)
                    {

                        #region Direct Same Flight

                        if (((HiddenField)GRDShowFlights.Rows[i].FindControl("HidSerial")).Value.Trim() == "")
                        {
                            bool blGotOrigin, blGotDest;
                            blGotOrigin = blGotDest = false;

                            DataRow row = dsSelectedFlights.Tables[0].NewRow();
                            row["FltOrigin"] = ((Label)GRDShowFlights.Rows[i].FindControl("LBLFltOrg")).Text;
                            row["FltDestination"] = ((Label)GRDShowFlights.Rows[i].FindControl("LBLFltDest")).Text;
                            row["FltNumber"] = ((Label)GRDShowFlights.Rows[i].FindControl("LBLFltNum")).Text;
                            row["FltTime"] = ((Label)GRDShowFlights.Rows[i].FindControl("LBLArrTime")).Text;
                            row["FltDate"] = ((Label)GRDShowFlights.Rows[i].FindControl("LBLFltDate")).Text;
                            row["ScheduleID"] = ((HiddenField)GRDShowFlights.Rows[i].FindControl("HidScheduleID")).Value;
                            row["Pcs"] = "0";
                            row["Accepted"] = "N";
                            row["Carrier"] = ((Label)GRDShowFlights.Rows[i].FindControl("LBLCarrier")).Text;
                            if (Session["SingleRow"].ToString() == "Y")
                            {
                                if (row["FltOrigin"].ToString() == Session["FltOrigin"].ToString())
                                {
                                    dsSelectedFlights.Tables[0].Rows.Add(row);
                                    blGotOrigin = true;
                                }

                            }
                            else
                            {
                                if (row["FltOrigin"].ToString() == Session["Origin"].ToString())
                                {
                                    dsSelectedFlights.Tables[0].Rows.Add(row);
                                    blGotOrigin = true;
                                }
                            }

                            for (int j = (i + 1); j < GRDShowFlights.Rows.Count; j++)
                            {
                                if (((Label)GRDShowFlights.Rows[j].FindControl("LBLFltNum")).Text.Trim() == "")
                                {

                                    DataRow rw = dsSelectedFlights.Tables[0].NewRow();
                                    rw["FltOrigin"] = ((Label)GRDShowFlights.Rows[j].FindControl("LBLFltOrg")).Text;
                                    rw["FltDestination"] = ((Label)GRDShowFlights.Rows[j].FindControl("LBLFltDest")).Text;
                                    rw["FltNumber"] = ((Label)GRDShowFlights.Rows[i].FindControl("LBLFltNum")).Text;
                                    rw["FltDate"] = ((Label)GRDShowFlights.Rows[j].FindControl("LBLFltDate")).Text;
                                    row["ScheduleID"] = ((HiddenField)GRDShowFlights.Rows[j].FindControl("HidScheduleID")).Value;
                                    rw["Pcs"] = "0";
                                    rw["Accepted"] = "N";
                                    row["Carrier"] = ((Label)GRDShowFlights.Rows[i].FindControl("LBLCarrier")).Text;
                            
                                    //dsSelectedFlights.Tables[0].Rows.Add(rw);


                                    if (blGotOrigin && blGotDest)
                                    {


                                    }
                                    else if (blGotOrigin)
                                    {
                                        if (Session["SingleRow"].ToString() == "Y")
                                        {
                                            if (rw["FltDestination"].ToString() == Session["FltDestination"].ToString())
                                            {
                                                blGotDest = true;
                                            }
                                        }
                                        else
                                        {
                                            if (rw["FltDestination"].ToString() == Session["Destination"].ToString())
                                            {
                                                blGotDest = true;
                                            }
                                        }
                                        dsSelectedFlights.Tables[0].Rows.Add(rw);
                                    }
                                    else
                                    {
                                        if (Session["SingleRow"].ToString() == "Y")
                                        {
                                            if (rw["FltOrigin"].ToString() == Session["FltOrigin"].ToString())
                                            {
                                                dsSelectedFlights.Tables[0].Rows.Add(rw);
                                                blGotOrigin = true;

                                                if (rw["FltDestination"].ToString() == Session["FltDestination"].ToString())
                                                    blGotDest = true;
                                            }
                                        }
                                        else
                                        {
                                            if (rw["FltOrigin"].ToString() == Session["Origin"].ToString())
                                            {
                                                dsSelectedFlights.Tables[0].Rows.Add(rw);
                                                blGotOrigin = true;


                                                if (rw["FltDestination"].ToString() == Session["Destination"].ToString())
                                                    blGotDest = true;
                                            }
                                        }

                                    }

                                }
                                else
                                {
                                    i = (j - 1);
                                    break;
                                }
                            }

                        }

                        #endregion

                        #region Via Different Flight

                        if (((HiddenField)GRDShowFlights.Rows[i].FindControl("HidSerial")).Value.Trim() != "")
                        {
                            DataSet dsRoute = new DataSet("ShowFlights_23");
                            dsRoute = ((DataSet)Session["dsRoute"]);
                            DataView dv = new DataView(dsRoute.Tables[0].Copy());
                            dv.RowFilter = " Serial=" + ((HiddenField)GRDShowFlights.Rows[i].FindControl("HidSerial")).Value.Trim();
                            string[] CarrierInfo = ((Label)GRDShowFlights.Rows[i].FindControl("LBLCarrier")).Text.Trim('/').Split('/');
                            DataTable dt = new DataTable("ShowFlights_4");
                                dt = dv.ToTable();
                            int cnt = 0;
                            foreach (DataRow rw in dt.Rows)
                            {
                                DataRow dsSelectedFlightsRow = dsSelectedFlights.Tables[0].NewRow();
                                dsSelectedFlightsRow["FltOrigin"] = rw["Source"].ToString();
                                dsSelectedFlightsRow["FltDestination"] = rw["Destination"].ToString();
                                dsSelectedFlightsRow["FltNumber"] = rw["Flight"].ToString();
                                dsSelectedFlightsRow["FltTime"] = rw["DeptTime"].ToString();
                                dsSelectedFlightsRow["FltDate"] = rw["FltDate"].ToString();
                                dsSelectedFlightsRow["ScheduleID"] = "0";
                                dsSelectedFlightsRow["Pcs"] = "0";
                                dsSelectedFlightsRow["Accepted"] = "N";
                                try
                                {
                                    dsSelectedFlightsRow["Carrier"] = CarrierInfo[cnt++];
                                }
                                catch (Exception ex) 
                                {
                                    dsSelectedFlightsRow["Carrier"] = "9W-AIR";
                                }
                                dsSelectedFlights.Tables[0].Rows.Add(dsSelectedFlightsRow);
                            }
                        }

                        #endregion

                    }

                }

                Session["dsSelectedFlights"] = dsSelectedFlights.Copy();

            }
            catch (Exception ex)
            {

            }
        }

        private void HighlightDataGridRows()
        {   
            for (int intRow = 0; intRow < GRDShowFlights.Rows.Count; intRow++)
            {
                if (((Label)GRDShowFlights.Rows[intRow].FindControl("LBLColorFlag")).Text.Trim() == "1")
                {
                    GRDShowFlights.Rows[intRow].BackColor = CommonUtility.ColorHighlightedGrid;
                }
            }
        }

        private void HideControlsOnDemoFlag()
        {
            LoginBL objBL = new LoginBL();
            try
            {
                string IsDemoInstance = string.Empty;

                if (Session["DemoInstance"] == null)
                {
                    IsDemoInstance = objBL.GetMasterConfiguration("DemoInstance");

                    if (IsDemoInstance != "")
                        Session["DemoInstance"] = Convert.ToBoolean(IsDemoInstance);
                    else
                        Session["DemoInstance"] = true;
                }                

                if (!Convert.ToBoolean(Session["DemoInstance"]))
                {
                    GRDShowFlights.Columns[4].Visible = false; //Rate Per kg               
                }
            }
            catch (Exception ex) { }
            finally
            {
                objBL = null;
            }
        }
    }
}
