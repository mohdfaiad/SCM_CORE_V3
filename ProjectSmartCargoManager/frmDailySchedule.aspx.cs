using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using System.Drawing;
using QID.DataAccess;


namespace ProjectSmartCargoManager
{
    public partial class frmDailySchedule : System.Web.UI.Page
    {
        AirlineScheduleBAL OBJasb = new AirlineScheduleBAL();
        ListBookingBAL objBAL = new ListBookingBAL();
        public static int scheduleID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                try
                {
                    GetFlights();
                    GetAirports();
                    //getFligtNoSourceDestWise("All", "All");


                 //   txtFlightDate.Text = "";
                   // pnlSchedule.Visible = true;
                   // txtFlightDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }

                catch (Exception ex)
                {

                }
            }
            
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                PnlGrid.Visible = false;
                if (txtFlightDate.Text.Trim() == "" || txtFlightDate.Text.Trim() == null)
                {
                    lblStatus.Text = "Kindly Enter date for Schedule";
                    return;
                }
                if (ddlFlight.SelectedValue == "Select")
                {
                    lblStatus.Text = "Kindly Select Flight from list";
                    return;
                }
                lblStatus.Text = "";
                //   LoadGridSchedule();
                //LoadSourceInGridview();

                //LoadDestinationInGridview();

                //AirCraftTypeinGridview();
                AddRowToGrid();

            }
            catch (Exception ex)
            {

            }

        }

        #region  get Flight No as per Source and Dest
        private void getFligtNoSourceDestWise(string source, string Dest)
        {
            try
            {
                DataSet ds = OBJasb.GetFlightListAsPerSourceDest(source, Dest);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            

                            //flight dropdown
                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[1].ColumnName] = "Select";
                            ds.Tables[0].Rows.Add(row);

                            ddlFlight.DataSource = ds;
                            ddlFlight.DataMember = ds.Tables[0].TableName;
                            ddlFlight.DataValueField = ds.Tables[0].Columns[1].ColumnName;
                            ddlFlight.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                            ddlFlight.DataBind();
                            ddlFlight.Text = "Select";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region AirCraft type List in gridview

        private void AirCraftTypeinGridview()
        {
            try
            {

                DataSet ds = OBJasb.GetAirCraftType();

                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    DropDownList ddlAircraft = (DropDownList)grdScheduleinfo.Rows[i].FindControl("ddlAirCraft");
                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                //Aircraft in Gridview Dropdown
                                DataRow row = ds.Tables[0].NewRow();

                                //row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                                //ds.Tables[0].Rows.Add(row);


                                ddlAircraft.DataSource = ds;
                                ddlAircraft.DataMember = ds.Tables[0].TableName;
                                ddlAircraft.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                                ddlAircraft.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                                ddlAircraft.DataBind();
                                //ddlAircraft.Text = "Select";


                                DataSet dsCapacity = OBJasb.GetCargoCapacity(ds.Tables[0].Rows[0][0].ToString());
                                if (dsCapacity != null)
                                {
                                    if (dsCapacity.Tables != null)
                                    {
                                        if (dsCapacity.Tables.Count > 0 || dsCapacity.Tables[0].Rows.Count > 0)
                                        {
                                            //txtCargoCapacity.Text = dsCapacity.Tables[0].Rows[0][0].ToString();
                                            //txtCapacity
                                            //((TextBox)grdScheduleinfo.Rows[i].FindControl("txtCapacity")).Text = dsCapacity.Tables[0].Rows[0][0].ToString();
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Aircraft type List in gridview

        #region LoadSource in Gridview Dropdown
        public void LoadSourceInGridview()
        {
            try
            {
                DataSet ds = OBJasb.GetOriginList("");
                DropDownList ddl = new DropDownList();

                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));
                    if (ds != null)
                    {
                        //Source in Gridview Dropdown
                        DataRow row = ds.Tables[0].NewRow();

                        row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                        ds.Tables[0].Rows.Add(row);

                        ddl.DataSource = ds;
                        ddl.DataMember = ds.Tables[0].TableName;
                        ddl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataBind();
                        ddl.Text = "Select";
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        #endregion LoadSource Dropdown

        #region LoadDest in Gridview Dropdown
        public void LoadDestinationInGridview()
        {
            try
            {
                DataSet ds = OBJasb.GetDestinationList("");
                DropDownList ddl = new DropDownList();

                for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest")));
                    if (ds != null)
                    {
                        //LoadDest in Gridview Dropdown 
                        DataRow row = ds.Tables[0].NewRow();

                        row[ds.Tables[0].Columns[0].ColumnName] = "Select";
                        ds.Tables[0].Rows.Add(row);

                        ddl.DataSource = ds;
                        ddl.DataMember = ds.Tables[0].TableName;
                        ddl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                        ddl.DataBind();
                        ddl.Text = "Select";
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        #endregion LoadSource Dropdown


        #region Loadgrid SscheduleInfo Intial Row
        private void LoadGridSchedule()
        {
            try
            {
                DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;
                DataSet Ds = new DataSet();

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RowNumber";
                myDataTable.Columns.Add(myDataColumn);

                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "FlightID";
                //myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "From";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "To";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DeptTimeHr";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DeptTimeMin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ActDeptTimeHr";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ActDeptTimeMin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivalTimeHr";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivalTimeMin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ActArrivalTimeHr";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ActArrivalTimeMin";
                myDataTable.Columns.Add(myDataColumn);

             
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AircraftType";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Remark";
                myDataTable.Columns.Add(myDataColumn);




                DataRow dr;
                dr = myDataTable.NewRow();
                dr["RowNumber"] = 1;
                dr["From"] = "select";//"5";
                dr["To"] = "";// "5";
                dr["DeptTimeHr"] = "";
                dr["DeptTimeMin"] = "";
                dr["ActDeptTimeHr"] = "";
                dr["ActDeptTimeMin"] = "";
                dr["ArrivalTimeHr"] = "";// "9";
                dr["ArrivalTimeMin"] = "";
                dr["ActArrivalTimeHr"] = "";// "9";
                dr["ActArrivalTimeMin"] = "";
               
                dr["AircraftType"] = "";
                dr["Remark"] = "";
                // DateTime Flightfromdate = new DateTime();
                // DateTime FlightToDate = new DateTime();
                string Flightfromdate = "", FlightToDate = "";

                string FlightNo= ddlFlight.SelectedValue;
                DataSet ds = OBJasb.GetActiveFlightData(FlightNo, Flightfromdate);

                //  ds = OBJasb.GetAirlineSchedule(ddlOrigin.Text, ddlDestination.Text, FlightNo, ddlAirCraftType.Text, Flightfromdate, FlightToDate, ddlStatus.Text,strdomestic);


                myDataTable.Rows.Add(dr);
                ViewState["CurrentTable"] = myDataTable;//ds.Tables[0];
                //Bind the DataTable to the Grid

                grdScheduleinfo.DataSource = null;
                grdScheduleinfo.DataSource = myDataTable; // ds.Tables[0];
                grdScheduleinfo.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion LoadgridScheduleInfo Intial Row


        #region AddRow To Grid
        private void AddRowToGrid()
        {
            try
            {
                int rowIndex = 0;

                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    if (txtFlightDate.Text != "")
                    {
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("dd/MM/yyyy");
                        //  dt1 = DateTime.Parse(txtFlightFromdate.Text);//.ToString("dd/MM/yyyy");
                        Flightfromdate = txtFlightDate.Text;
                        DateTime dtFromDate = DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null);
                        Flightfromdate = dtFromDate.ToString("MM/dd/yyyy");
                    }
                   
                }
                catch (Exception ex)
                {

                }


                string FlightNo = "";
                // FlightNo = ddlFlight.SelectedValue;
                FlightNo = ddlFlight.SelectedItem.Text;


                DataSet ds = OBJasb.GetActiveFlightData(FlightNo, Flightfromdate);

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                PnlGrid.Visible = true;
                                           // grdScheduleinfo.Visible = true;
                                
                                    ViewState["CurrentTable"] = ds.Tables[0];
                                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                                    DataRow drCurrentRow = null;
                                    grdScheduleinfo.DataSource = dtCurrentTable;
                                    grdScheduleinfo.DataBind();

                                    LoadSourceInGridview();
                                    LoadDestinationInGridview();
                                    AirCraftTypeinGridview();


                                    if (dtCurrentTable.Rows.Count > 0)
                                    {
                                        //extract the TextBox values

                                             
                                        for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                                        {
                                            int coutgrid = grdScheduleinfo.Rows.Count;
                                            //extract the TextBox values

                                           // drCurrentRow = dtCurrentTable.NewRow();
                                  


                                            DropDownList ddlFromOrigin = new DropDownList();

                                            ddlFromOrigin = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin")));

                                            ddlFromOrigin.SelectedValue = dtCurrentTable.Rows[i][2].ToString();

                                            DropDownList ddlDest = new DropDownList();

                                            ddlDest = ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest")));

                                            ddlDest.SelectedValue = dtCurrentTable.Rows[i][3].ToString();


                                            //string FlightID = dtCurrentTable.Rows[i][1].ToString();

                                            //string CheckWeekdays = dtCurrentTable.Rows[i][7].ToString();

                                            
                                            string[] HrDept = dtCurrentTable.Rows[i][5].ToString().Split(':');

                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeHr")).Text = HrDept[0];
                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtDeptTimeMin")).Text = HrDept[1];

                                            string[] HrActDept = dtCurrentTable.Rows[i][6].ToString().Split(':');


                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtADTDeptTimeHr")).Text = HrActDept[0];
                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[2].FindControl("txtADTDeptTimeMin")).Text = HrActDept[1];

                                            string[] HrArr = dtCurrentTable.Rows[i][8].ToString().Split(':');
                                            string[] HrActArr = dtCurrentTable.Rows[i][9].ToString().Split(':');
                                           
                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeHr")).Text = HrArr[0];
                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtArrivaltimeMin")).Text = HrArr[1];

                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtATAArrivaltimeHr")).Text = HrActArr[0];
                                            ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtATAArrivaltimeMin")).Text = HrActArr[1];

                                            
               
                                            ((DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlAirCraft"))).SelectedValue = dtCurrentTable.Rows[i][11].ToString();
                                            try
                                            {
                                                if (dtCurrentTable.Rows[i]["Remark"].ToString() != "")
                                                {
                                                    ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtRemark")).Text = dtCurrentTable.Rows[i]["Remark"].ToString();

                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                          //  ((TextBox)(grdScheduleinfo.Rows[i].FindControl("txtCapacity"))).Text = dtCurrentTable.Rows[i][16].ToString();


                                        }

                                       // dtCurrentTable.Rows.Add(drCurrentRow);
                                        ViewState["CurrentTable"] = dtCurrentTable;

                                        //grdScheduleinfo.DataSource = dtCurrentTable;
                                        //grdScheduleinfo.DataBind();
                                    
                                }

                               
                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Brown;
                                lblStatus.Text = "Schedule Not Available for selected criteria.";
                              //  pnlSchedule.Visible = false;
                            }
                        }
                        else
                        {
                           // pnlSchedule.Visible = false;
                        }
                    }
                }


                //Set Previous Data on Postbacks
                // SetPreviousData();
            }
            catch (Exception ex)
            {
            }

        }
        #endregion Add new Row To Grid


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateData())
                {

                    return;
                }






                if (grdScheduleinfo.Rows.Count > 0)
                {
                    #region Insert RouteDetails
                    #region Prepare Parameters

                    int IsRouteInsert = 0;
                    for (int j = 0; j < grdScheduleinfo.Rows.Count; j++)
                    {
                        object[] RouteInfo = new object[13];
                        int i = 0;

                        //0
                        RouteInfo.SetValue(ddlFlight.SelectedValue, i);
                        i++;

                        DateTime dtDate = DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null);
                        //1
                        RouteInfo.SetValue(dtDate, i);
                        i++;

                        //2
                        string source = ((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlFromOrigin"))).SelectedValue;
                        RouteInfo.SetValue(source, i);
                        i++;

                        string Dest = ((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlToDest"))).SelectedValue;

                        //3
                        RouteInfo.SetValue(Dest, i);
                        i++;
                        string STD = ((TextBox)grdScheduleinfo.Rows[j].Cells[2].FindControl("txtDeptTimeHr")).Text + ":" + ((TextBox)grdScheduleinfo.Rows[j].Cells[2].FindControl("txtDeptTimeMin")).Text;

                        //4
                        RouteInfo.SetValue(STD, i);
                        i++;
                        string ATD = ((TextBox)grdScheduleinfo.Rows[j].Cells[2].FindControl("txtADTDeptTimeHr")).Text + ":" + ((TextBox)grdScheduleinfo.Rows[j].Cells[2].FindControl("txtADTDeptTimeMin")).Text;
                        //5
                        RouteInfo.SetValue(ATD, i);
                        i++;
                        string STA = ((TextBox)grdScheduleinfo.Rows[j].Cells[1].FindControl("txtArrivaltimeHr")).Text + ":" + ((TextBox)grdScheduleinfo.Rows[j].Cells[1].FindControl("txtArrivaltimeMin")).Text;

                        //6
                        RouteInfo.SetValue(STA, i);
                        i++;

                        string ATA = ((TextBox)grdScheduleinfo.Rows[j].Cells[1].FindControl("txtATAArrivaltimeHr")).Text + ":" + ((TextBox)grdScheduleinfo.Rows[j].Cells[1].FindControl("txtATAArrivaltimeMin")).Text;
                        //7
                        RouteInfo.SetValue(ATA, i);
                        i++;

                        scheduleID = Convert.ToInt32(((Label)grdScheduleinfo.Rows[j].FindControl("ScheduleID")).Text);

                        //8
                        RouteInfo.SetValue(scheduleID, i);
                        i++;

                        string AircraftType = "";
                        AircraftType = ((DropDownList)(grdScheduleinfo.Rows[j].FindControl("ddlAirCraft"))).SelectedValue;

                        //9
                        RouteInfo.SetValue(AircraftType, i);
                        i++;

                        //10
                        string RegistrationNo="";
                        RegistrationNo = ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtRegistration")).Text;
                        RouteInfo.SetValue(RegistrationNo, i);
                        i++;

                        

                        string Remark = "";
                        Remark = ((TextBox)grdScheduleinfo.Rows[j].FindControl("txtRemark")).Text;
                        //11
                        RouteInfo.SetValue(Remark, i);
                        i++;

                        string UserID = "QIDUser";// Session["UserName"].ToString();

                        //12
                        RouteInfo.SetValue(UserID, i);
                        //i++;


                    #endregion Prepare Parameters
                        IsRouteInsert = 0;
                        IsRouteInsert = OBJasb.SaveDailyActiveFlightDetails(RouteInfo);

                    #endregion

                    }
                    if (IsRouteInsert < 0)
                    {
                        lblStatus.Text = "Error Save Daily Schedule. Please try again...";
                        return;
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "Daily Schedule Save Successfully";
                    }



                }

            }
            catch (Exception ex)
            {

            }




        }




        #region Validate Data
        /// <summary>
        /// Validate data entered by user.
        /// </summary>
        /// <returns>Returns True if valid data is entered.</returns>
        private bool ValidateData()
        {
            try
            {
                #region validate flight
                if (ddlFlight.SelectedValue == "Select")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select valid Flight#";
                    ddlFlight.Focus();
                    return false;
                }
                // bool strflight = ddlFlight.Text.StartsWith("*");
                //    string strflight = ddlFlight.Text.Substring(1, ddlFlight.Text.Length);


                #endregion

                #region Validate From Date and To date
                if (txtFlightDate .Text == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid flight date";
                    txtFlightDate.Focus();
                    return false;
                }
                

                DateTime dt1 = DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                DateTime dt2 = DateTime.Now;//DateTime.Parse(txtToDate.Text);

                int chk = DateTime.Compare(dt2,dt1);
                if (dt1.ToShortDateString() != dt2.ToShortDateString())
                {
                    if (chk > 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Daily Schedule can not set for previous date";
                        txtFlightDate.Focus();
                        return false;
                    }

                }
              
                #endregion
            }
            catch (Exception ex)
            {

            }

            


            if (ddlFlight.SelectedValue == "Select")
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please select FlightID";
                ddlFlight.Focus();
                return false;
            }
           

            //Validate Route grid.
            if (grdScheduleinfo.Rows.Count < 1)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter Route Details";
                return false;
            }



            for (int i = 0; i < grdScheduleinfo.Rows.Count; i++)
            {
                DropDownList tempSource;

                //Validate FromSource code
                tempSource = (DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin"));
                if (tempSource.Text == "Select")//tempSource.SelectedIndex < 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select valid Origin Code from route (row :" + (i + 1) + ")";
                    tempSource.Focus();
                    return false;
                }

                DropDownList tempDest;

                //Validate FromSource code
                tempDest = (DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlToDest"));
                if (tempDest.Text == "Select")//tempSource.SelectedIndex < 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select valid Destination Code in route (row :" + (i + 1) + ")";
                    tempDest.Focus();
                    return false;
                }
                if (tempSource.SelectedValue == tempDest.SelectedValue)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Origin and Destination Code can not be same in route details. (row :" + (i + 1) + ")";
                    tempDest.Focus();
                    return false;
                }






                TextBox tempdeptTimeHr;

                //Validate code description
                tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtADTDeptTimeHr"));
                try
                {
                    if (tempdeptTimeHr.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATD in Hrs (row :" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                    if (int.Parse(tempdeptTimeHr.Text) > 24)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATD in Hrs (row :" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                    if (int.Parse(tempdeptTimeHr.Text) < 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATD in Hrs (row :" + (i + 1) + ")";
                        tempdeptTimeHr.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid ATD in Hrs (row :" + (i + 1) + ")";
                   // tempdeptTimeHr.Focus();
                    return false;
                }
                //tempdeptTimeHr = null;
                TextBox tempDeprtureTimeMin;

                //Validate code description
                tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtADTDeptTimeMin"));
                try
                {
                    if (tempDeprtureTimeMin.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATD in Min (row :" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;
                    }
                    else if (int.Parse(tempDeprtureTimeMin.Text) > 60)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATD in Min (row :" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;
                    }
                    else if (int.Parse(tempDeprtureTimeMin.Text) < 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATD in Min (row :" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid ATD in Min (row :" + (i + 1) + ")";
                  //  tempDeprtureTimeMin.Focus();
                    return false;
                }
                try
                {
                    if (int.Parse(tempDeprtureTimeMin.Text) == 0 && int.Parse(tempdeptTimeHr.Text) == 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATD time (row :" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;

                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid ATD time(row :" + (i + 1) + ")";
                    tempDeprtureTimeMin.Focus();
                    return false;
                }

                tempDeprtureTimeMin = null;
                tempdeptTimeHr = null;
               


                TextBox tempArrivalTimeHr;

                //Validate tempArrivalTimeHr
                tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtATAArrivaltimeHr"));
                try
                {
                    if (tempArrivalTimeHr.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATA in Hrs (row :" + (i + 1) + ")";
                        tempArrivalTimeHr.Focus();
                        return false;
                    }
                    else if (int.Parse(tempArrivalTimeHr.Text) > 24)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATA in Hrs (row :" + (i + 1) + ")";
                        tempArrivalTimeHr.Focus();
                        return false;

                    }
                    else if (int.Parse(tempArrivalTimeHr.Text) < 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATA in Hrs (row :" + (i + 1) + ")";
                        tempArrivalTimeHr.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid ATA in Hrs (row :" + (i + 1) + ")";
                    tempArrivalTimeHr.Focus();
                    return false;
                }
                //tempArrivalTimeHr = null;

                TextBox tempArrivalTimeMin;

                //Validate code description
                tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtATAArrivaltimeMin"));
                try
                {
                    if (tempArrivalTimeMin.Text == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATA in Min (row :" + (i + 1) + ")";
                        tempArrivalTimeMin.Focus();
                        return false;
                    }
                    else if (int.Parse(tempArrivalTimeMin.Text) > 60)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATA in Min (row :" + (i + 1) + ")";
                        tempArrivalTimeMin.Focus();
                        return false;
                    }
                    else if (int.Parse(tempArrivalTimeMin.Text) < 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATA in Min (row :" + (i + 1) + ")";
                        tempArrivalTimeMin.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid ATA in Min (row :" + (i + 1) + ")";
                    tempArrivalTimeMin.Focus();

                    return false;
                }

                try
                {
                    if (int.Parse(tempArrivalTimeMin.Text) == 0 && int.Parse(tempArrivalTimeHr.Text) == 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATA in (row :" + (i + 1) + ")";
                        tempDeprtureTimeMin.Focus();
                        return false;

                    }
                }
                catch (Exception)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid ATA in (row :" + (i + 1) + ")";
                    tempArrivalTimeMin.Focus();
                    return false;
                }

                tempArrivalTimeHr = null;
                tempArrivalTimeMin = null;



                
                try{
                    TextBox tempATDArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtADTDeptTimeHr"));
                    TextBox tempATDArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtADTDeptTimeMin"));
                    TextBox tempSTDArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeHr"));
                    TextBox tempSTDArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));

                    int ATDArrTimeHr = int.Parse(tempATDArrivalTimeHr.Text);
                    int STDArrTimeHr = int.Parse(tempSTDArrivalTimeHr.Text);
                    int ATDArrTimeMin = int.Parse(tempATDArrivalTimeMin.Text);
                    int STDArrTimeMin = int.Parse(tempSTDArrivalTimeMin.Text);


                    if (ATDArrTimeHr < STDArrTimeHr)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATD in Hr (row :" + (i + 1) + ")";
                        tempATDArrivalTimeHr.Focus();
                        return false;

                    }

                    if (ATDArrTimeHr == STDArrTimeHr)
                    {
                        if (ATDArrTimeMin < STDArrTimeMin)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid ATD in Min (row :" + (i + 1) + ")";
                            tempATDArrivalTimeMin.Focus();
                            return false;
                        }

                    }





                   

                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid ATD time";
                    //tempdeptTimeHr.Focus();
                    return false;
                }












                //Validate Arrival Departure time
                try
                {
                    tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtATAArrivaltimeHr"));
                    tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtADTDeptTimeHr"));
                   
                    tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtATAArrivaltimeMin"));
                    tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtADTDeptTimeMin"));


                    int ArrTimeHr = int.Parse(tempArrivalTimeHr.Text);
                    int DeptTimeHr = int.Parse(tempdeptTimeHr.Text);


                    TextBox tempATAArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtATAArrivaltimeHr"));
                    TextBox tempATAArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtATAArrivaltimeMin"));
                    TextBox tempSTAArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeHr"));
                    TextBox tempSTAArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtArrivaltimeMin"));

                    int ATAArrTimeHr = int.Parse(tempATAArrivalTimeHr.Text);
                    int STAArrTimeHr = int.Parse(tempSTAArrivalTimeHr.Text);
                    int ATAArrTimeMin = int.Parse(tempATAArrivalTimeMin.Text);
                    int STAArrTimeMin = int.Parse(tempSTAArrivalTimeMin.Text);


                    if (ATAArrTimeHr < STAArrTimeHr)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid ATA in Hr";
                        tempATAArrivalTimeHr.Focus();
                        return false;

                    }

                    if (ATAArrTimeHr == STAArrTimeHr)
                    {
                        if (ATAArrTimeMin < STAArrTimeMin)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid ATA in Min (row :" + (i + 1) + ")";
                            tempATAArrivalTimeMin.Focus();
                            return false;
                        }

                    }





                   

                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Departure and Arrival time in Min";
                    //tempdeptTimeHr.Focus();
                    return false;
                }

                //Validate Frequency



                TextBox txtRemark = ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtRemark"));
                   // ((TextBox)grdScheduleinfo.Rows[i].Cells[1].FindControl("txtRemark")).Text;// ((TextBox)grdScheduleinfo.Rows[i].FindControl("txtRemark"));
                try
                {


                    if (txtRemark.Text.Trim() == "")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter Remark in (row :" + (i + 1) + ")";
                      //  txtRemark.Focus();
                        return false;
                    }
                  
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter Remark in(row :" + (i + 1) + ")";
                   // txtRemark.Focus();
                    return false;

                }




                //Validate Source Wise Data from database
                try
                {


                    try
                    {

                        DateTime dtDate = DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        //DateTime dt1to = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

                        int dsCheckRoute = OBJasb.CheckDailyScheduleforflight(ddlFlight.SelectedValue,dtDate);
                        
                                if (dsCheckRoute> 0)
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Daily Flight Already configure for this date";
                                    return false;
                                }
                                else
                                {

                                }
                            
                        

                    }
                    catch (Exception ex)
                    {

                    }


                    if (grdScheduleinfo.Rows.Count > 1)
                    {
                        if (i > 0)
                        {
                            tempSource = (DropDownList)(grdScheduleinfo.Rows[i].FindControl("ddlFromOrigin"));
                            tempDest = (DropDownList)(grdScheduleinfo.Rows[i - 1].FindControl("ddlToDest"));

                            if (tempSource.Text != tempDest.Text)
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please Select From Source Code in route in row " + (i + 1);
                                tempSource.Focus();
                                return false;

                            }
                           

                            //tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtDeptTimeMin"));

                            //tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtArrivalTimeMin"));
                            //if (int.Parse(tempDeprtureTimeMin.Text) < int.Parse(tempArrivalTimeMin.Text))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid Departure Time in Min";
                            //    tempDeprtureTimeMin.Focus();
                            //    return false;
                            //}


                            tempArrivalTimeHr = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtATAArrivaltimeHr"));
                            tempdeptTimeHr = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtADTDeptTimeHr"));
                          
                            tempDeprtureTimeMin = (TextBox)(grdScheduleinfo.Rows[i].FindControl("txtADTDeptTimeMin"));

                            tempArrivalTimeMin = (TextBox)(grdScheduleinfo.Rows[i - 1].FindControl("txtATAArrivalTimeMin"));


                            int ArrTimeHr = int.Parse(tempArrivalTimeHr.Text);
                            int DeptTimeHr = int.Parse(tempdeptTimeHr.Text);
                            if (ArrTimeHr > DeptTimeHr)// && (arrday > DeptDay))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid ATD in Hr in row " + (i + 1);
                                tempdeptTimeHr.Focus();
                                return false;
                            }
                            //else if (ArrTimeHr > DeptTimeHr) //&& (arrday == DeptDay))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid ATD in Hr in row " + (i + 1);
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}
                            //else if (ArrTimeHr == DeptTimeHr)// && (arrday < DeptDay))
                            //{
                            //    lblStatus.ForeColor = Color.Red;
                            //    lblStatus.Text = "Please enter valid ATA  in Hr in row " + (i + 1);
                            //    tempdeptTimeHr.Focus();
                            //    return false;
                            //}
                            
                            else if ((ArrTimeHr == DeptTimeHr) && (int.Parse(tempDeprtureTimeMin.Text)) < (int.Parse(tempArrivalTimeMin.Text)))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid ATD  in Min in row " + (i + 1);
                                tempdeptTimeHr.Focus();
                                return false;
                            }
                            else if ((ArrTimeHr == DeptTimeHr) && (int.Parse(tempDeprtureTimeMin.Text)) == (int.Parse(tempArrivalTimeMin.Text)))
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid ATD in row " + (i + 1);
                                tempdeptTimeHr.Focus();
                                return false;
                            }





                        }


                    }

                   

                }
                catch (Exception ex)
                {

                }

                

            }

            return true;
        }
        #endregion Validate Data



        protected void ddlFlight_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region Get Org Dest
        protected void GetAirports()
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = da.SelectRecords("spGetAirportCodes");
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ddlDest.DataSource = ds;
                        ddlDest.DataMember = ds.Tables[0].TableName;
                        ddlDest.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                        ddlDest.DataTextField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                        ddlDest.DataBind();
                        ddlDest.Items.Insert(0, new ListItem("Select", "Select"));

                        ddlOrg.DataSource = ds;
                        ddlOrg.DataMember = ds.Tables[0].TableName;
                        ddlOrg.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                        ddlOrg.DataTextField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                        ddlOrg.DataBind();
                        ddlOrg.Items.Insert(0, new ListItem("Select", "Select"));
                    }
                }
            }
        }
        #endregion

        protected void ddlDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDest.SelectedIndex == 0)
            {
                GetFlights();
            }
            else
            {
                if (ddlOrg.SelectedIndex == 0)
                { return; }
                else
                {
                    string source = ddlOrg.SelectedItem.Text.Trim();
                    string dest = ddlDest.SelectedItem.Text.Trim();
                    DataSet dsResult = new DataSet();
                    string errormessage = "";
                    if (objBAL.GetAllFlightsNew(source, dest, "", ref dsResult, ref errormessage))
                    {

                        if (dsResult != null)
                        {
                            if (dsResult.Tables.Count > 0)
                            {
                                if (dsResult.Tables[0].Rows.Count > 0)
                                {
                                    ddlFlight.Items.Clear();
                                    ddlFlight.DataSource = dsResult.Tables[0];
                                    ddlFlight.DataTextField = "FltNumber";
                                    ddlFlight.DataValueField = "DeptTime";
                                    ddlFlight.DataBind();
                                    ddlFlight.Items.Insert(0, new ListItem("Select", ""));
                                    ddlFlight.SelectedIndex = -1;
                                }
                            }
                        }
                    }
                }
            }
        }

        #region getFlights
        public void GetFlights()
        {
            try
            {
                {
                    DataSet dsResult = new DataSet();
                    string errormessage = "";
                    //if (objBAL.GetAllFlightsNew(source, dest, date, ref dsResult, ref errormessage))
                    string procedure = "spGetAllFlightList";
                    SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                    dsResult = objSQL.SelectRecords(procedure);
                    if (dsResult != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                ddlFlight.Items.Clear();
                                ddlFlight.DataSource = dsResult.Tables[0];
                                ddlFlight.DataTextField = "FltNumber";
                                ddlFlight.DataValueField = "DeptTime";
                                ddlFlight.DataBind();
                                ddlFlight.Items.Insert(0, new ListItem("Select", ""));
                                ddlFlight.SelectedIndex = -1;
                            }
                        }
                    }
                    else
                    {
                        lblStatus.Text = "" + errormessage;
                        lblStatus.ForeColor = Color.Red;
                    }
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion
    }
}
