using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using BAL;

using System.IO;
using System.Drawing;
using QID.DataAccess;
using Microsoft.Reporting.WebForms;




namespace ProjectSmartCargoManager
{
    public partial class rptAWBMovement : System.Web.UI.Page
    {
        ArrayList arFlight = new ArrayList();
        ReportBAL OBJasb = new ReportBAL();
        MasterBAL MaBal = new MasterBAL();
        BALException objBAL = new BALException();
        BookingBAL objBLL = new BookingBAL();
        private DataSet Dataset1 = new DataSet("rptAWBMovmnt_dsAWBmvmntds1");
        private DataSet Dataset2 = new DataSet("rptAWBMovmnt_dsAWBmvmntds2");
        ReportBAL objReport = new ReportBAL();
        ReportBAL rptBAl = new ReportBAL();
        DataSet ds;
        static string AgentCode = "";
        public static string CurrTime = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //if (Session["awbPrefix"] != null)
                //{
                //    txtAwbPrefix1.Text = Session["awbPrefix"].ToString();
                //}
                if (!IsPostBack)
                {
                    ReportViewer1.Visible = false;
                    AgentCode = Convert.ToString(Session["ACode"]);

                    int RoleId = Convert.ToInt32(Session["RoleID"]);

                    if (RoleId == 1 && AgentCode != "")
                    {
                        // txtAgentCode.Text = AgentCode;
                        //txtAgentCode.Enabled = false;
                    }


                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");

                    txtFlightFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFlightToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    btnExport.Visible = true;
                    ////rptViewerAWBMovement.Visible = false;
                    AirCraftType();
                    LoadCountry();
                    GetOrigin();
                    GetDestination();
                    LoadRegion();
                    LoadFlightPrefix();
                    GetFlights();
                    if (Session["awbPrefix"] != null)
                        txtAwbPrefix.Text = Session["awbPrefix"].ToString();
                    else
                    {
                        MasterBAL objBal = new MasterBAL();
                        Session["awbPrefix"] = objBal.awbPrefix();
                        txtAwbPrefix.Text = Session["awbPrefix"].ToString();
                    }
                    //AirCraftType();

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadCountry()
        {

            DataSet ds = new DataSet("rptAWBMovmnt_dsloadCountry");
            try
            {
                ds = objBAL.GetCountryCodeList(ddlCountry.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlCountry.DataSource = ds;
                            ddlCountry.DataMember = ds.Tables[0].TableName;
                            ddlCountry.DataValueField = "CountryCode";
                            ddlCountry.DataTextField = "Country";
                            ddlCountry.DataBind();
                            ddlCountry.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        
        }
        private void AirCraftType()
        {
            DataSet ds = new DataSet("rptAWBMovmnt_dsaircrftType");
            try
            {

                ds = OBJasb.GetAirCraftType();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //Aircraft in Gridview Dropdown
                            DataRow row = ds.Tables[0].NewRow();

                            row[ds.Tables[0].Columns[0].ColumnName] = "All";
                            ds.Tables[0].Rows.Add(row);


                            ddlAirCraftType.DataSource = ds;
                            ddlAirCraftType.DataMember = ds.Tables[0].TableName;
                            ddlAirCraftType.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlAirCraftType.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlAirCraftType.DataBind();
                            ddlAirCraftType.Text = "All";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        private void GetOrigin()
        {
            DataSet ds = new DataSet("rptAWBMovmnt_dsOrigin");
            try
            {
                ds = objBAL.GetOriginCodeList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = "AirportCode";// ds.Tables[2].Columns["Code"].ColumnName;

                            ddlOrigin.DataTextField = "Airport";//ds.Tables[2].Columns["Code"].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        
        }

        private void GetDestination()
        {

            DataSet ds = new DataSet("rptAWBMovmnt_dsDest");
            try
            {
                ds = objBAL.GetOriginCodeList(ddlDestination.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = "AirportCode";// ds.Tables[2].Columns["Code"].ColumnName;

                            ddlDestination.DataTextField = "Airport";// ds.Tables[2].Columns["Code"].ColumnName;
                            ddlDestination.DataBind();
                            ddlDestination.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        
        
        }
        private void LoadRegion()
        {
            StockAllocationBAL objBAL = new StockAllocationBAL();
            DataSet Region = new DataSet("rptAWBMovmnt_dsRegion");
            try
            {
                Region = objBAL.GetRegionCode();

                ddlRegion.Items.Clear();
                ddlRegion.Items.Add(new ListItem("All"));

                for (int intCount = 0; intCount < Region.Tables[0].Rows.Count; intCount++)
                {
                    ddlRegion.Items.Add(new ListItem(Region.Tables[0].Rows[intCount][0].ToString()));
                }
                ddlRegion.SelectedIndex = 0;
            }
            catch (Exception ex)
            { }
            finally
            {
                if (Region != null)
                    Region.Dispose();
            }
        
        
        }
        private void LoadFlightPrefix()
        {
            DataSet ds = new DataSet("rptAWBMovmnt_dsFltPrefix");
            try
            {
                ds = objBLL.GetFlightPrefixList();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlFlightPrefix.Items.Clear();
                            //ddlComodityCd.Items.Add("Select");
                            ddlFlightPrefix.DataSource = ds.Tables[0];
                            //ddlComodityCd.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            //ddlComodityCd.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlFlightPrefix.DataTextField = "PartnerCode";
                            ddlFlightPrefix.DataValueField = "PartnerCode";
                            ddlFlightPrefix.DataBind();
                            //ddlComodityCd.SelectedIndex = 0;
                            ddlFlightPrefix.Items.Insert(0, "SELECT");
                            // ddlComodityCd.SelectedValue = "GEN";
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        
        
        
        
        }
        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("rptAWBMovmnt_dsawbmovmntlist");
            string ErrorLog = string.Empty;
            lblStatus.Text = "";
            if(Validate()==false)
            {
                ReportViewer1.Visible = false;
                return;
            }
            try
            {
                if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                {
                    //lblStatus.ForeColor = Color.Red;
                    //lblStatus.Text = "Please enter From date and To date for datewise Schedule List";
                    //txtFlightFromdate.Focus();
                    //return;
                }
                if (txtFlightFromdate.Text.Trim() != "" || txtFlightToDate.Text.Trim() != "")
                {
                    DateTime dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    int chk = DateTime.Compare(dt1, dt2);
                    if (chk > 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To date";
                        //txtFromdate.Focus();
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                //txtFromdate.Focus();
                return;
            }



            try
            {
                int rowIndex = 0;

                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    //if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                    //{
                    //    lblStatus.ForeColor = Color.Red;
                    //    lblStatus.Text = "Please Insert From date and To date for datewise Schedule List";
                    //    txtFlightFromdate.Focus();
                    //    return;
                    //}

                    if (txtFlightFromdate.Text != "")
                    {
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("dd/MM/yyyy");
                        //  dt1 = DateTime.Parse(txtFlightFromdate.Text);//.ToString("dd/MM/yyyy");
                        //  Flightfromdate = txtFlightFromdate.Text;
                        //DateTime dtFromDate = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                        dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);

                        Flightfromdate = dt1.ToString("MM/dd/yyyy");
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        //FlightToDate = Convert.ToDateTime(txtFlightToDate.Text).ToString("dd/MM/yyyy");
                        // dt2 = DateTime.Parse(txtFlightToDate.Text);//.ToString("dd/MM/yyyy");
                        dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                        FlightToDate = dt2.ToString("MM/dd/yyyy");


                    }
                }
                catch (Exception ex)
                {

                }


                   string FlightNo = "ALL";
                // FlightNo = ddlFlight.SelectedValue;

                //FlightNo = ddlFlight.SelectedItem.Text; mod
                if (ddlFlightNumber.SelectedItem.Text.Trim()== "ALL")
                    FlightNo = "All";
                else
                    FlightNo = ddlFlightNumber.SelectedValue.ToString().Trim();//.Text;
                FlightNo = FlightNo.Replace(" ", string.Empty);

                 //FlightNo = "All";
                string strdomestic = "";
                //if (chkDomestic.Checked == true && chkInternational.Checked == true)
                //{
                strdomestic = "All";
                //}
                //else if (chkDomestic.Checked == true)
                //{
                //    strdomestic = "true";
                //}
                //else
                //{
                //    strdomestic = "False";

                //}

                // DataSet ds = OBJasb.GetAirlineSchedule(ddlOrigin.SelectedValue, ddlDestination.SelectedValue, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                string Source = "All", Dest = "All";
                if (ddlOrigin.SelectedItem.Text.Trim() != "")
                {
                    Source = ddlOrigin.SelectedValue.ToString().Trim();//.Text;
                }
                if (ddlDestination.SelectedItem.Text.Trim() != "")
                {
                    Dest = ddlDestination.SelectedValue.ToString().Trim();//.Text.Trim();
                }
                string country = "All";
                if (ddlCountry.SelectedItem.Text.Trim() != "")
                {
                    country = ddlCountry.SelectedValue.ToString().Trim();//.Text;
                }


                string Region = "All";
                if (ddlRegion.SelectedItem.Text.Trim() != "")
                {
                    Region = ddlRegion.SelectedValue.ToString().Trim();// Text;
                }

                 string  AwbNumber = "All";
                if (txtAwbNumber.Text.Trim() != "")
                {
                    AwbNumber = txtAwbNumber.Text.Trim();
                }
                string AwbPrefix = "";
                if (txtAwbPrefix.Text.Trim() != "")
                {
                    AwbPrefix = txtAwbPrefix.Text.Trim();
                }
                //string AwbNumber = "";
                //if (ddlAwbNumber.SelectedItem.Text.Trim() != "")
                //{
                //    AwbNumber = ddlAwbNumber.SelectedValue.ToString().Trim();//.Text.Trim();
                //}
                //string AwbPrefix = "";
                //if (ddlAwbPrefix.SelectedItem.Text.Trim() != "")
                //{ AwbPrefix = ddlAwbPrefix.SelectedValue.ToString().Trim(); }
                string Aircraft = "All";
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;

                bool chkStatus = MaBal.CheckConfiguration("AWBMovementReport", "ShowDataInOneline");

                if (!chkStatus)
                    ds = OBJasb.GetAWBMovement(country, Region, Source, Dest, FlightNo, Aircraft, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic, AwbNumber, AgentCode, AwbPrefix);
                else
                    ds = OBJasb.GetAWBMovemenInOneLine(country, Region, Source, Dest, FlightNo, Aircraft, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic, AwbNumber, AgentCode, AwbPrefix);

              

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["dsExp"] = ds;

                            DataTable myDataTable = new DataTable();
                            DataColumn myDataColumn;
                            DataSet Ds = new DataSet("rptAWBMovmnt_dsawbmovmntexpt");

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Country";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Region";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Origin";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Destination";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "FlightID";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "FromDt";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ToDt";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "AWBNo";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ReportDate";
                            myDataTable.Columns.Add(myDataColumn);

                            DataRow dr;
                            dr = myDataTable.NewRow();
                            dr["Country"] = ddlCountry.SelectedItem.Text.Trim();
                            dr["Region"] = ddlRegion.SelectedItem.Text.Trim(); //"5";
                            dr["Origin"] = ddlOrigin.SelectedItem.Text.Trim() + ":";// "5";
                            dr["Destination"] = ddlDestination.SelectedItem.Text.Trim();
                            dr["FlightID"] = ddlFlightNumber.SelectedItem.Text.Trim();

                            dr["FromDt"] = txtFlightFromdate.Text.Trim();
                            dr["ToDt"] = txtFlightToDate.Text;// "9";
                            dr["AWBNo"] = txtAwbNumber.Text;//ddlAwbNumber.SelectedItem.Text.Trim();//.txtAWBNo.Text.Trim();
                            dr["ReportDate"] = CurrTime;
                            myDataTable.Rows.Add(dr);

                            //  Ds.Tables.Add(myDataTable);

                            DataSet dschk = new DataSet("rptAWBMovmnt_dstempchk");
                            dschk.Tables.Add(myDataTable);
                            Dataset2 = dschk.Copy();


                            Dataset1 = ds.Copy();
                            #region old code
                            //rptViewerAWBMovement.Visible = true;
                            ////  System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "AWB.rdlc";
                            //info = new FileInfo(Server.MapPath("/Reports/rptABMovement.rdlx"));
                            //   // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"]+ "rptABMovement.rdlx");
                            //  definition = new ReportDefinition(info);
                            //  runtime = new ReportRuntime(definition);


                            //  //  Dataset2 = dschk;

                            //  runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                            // rptViewerAWBMovement.SetReport(runtime);
                            #endregion
                            ReportViewer1.Visible = true;

                            #region Adding Filters to Report
                            System.IO.MemoryStream Logo = null;
                            try
                            {
                                Logo = CommonUtility.GetImageStream(Page.Server);
                                //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                            }
                            catch (Exception ex)
                            {
                                Logo = new System.IO.MemoryStream();
                            }

                            if (ds.Tables[0].Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                                ds.Tables[0].Columns.Add(col1);
                            }


                            #endregion
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAWBMovement.rdlc");
                            ReportDataSource datasource = new ReportDataSource("dsrptAWBMovement_dtrptAWBMovement", ds.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(datasource);
                            ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                            btnExport.Visible = true;
                            SaveUserActivityLog("");

                            // dt.Rows[0]["From"]= txtAutoSource.Text.Trim();


                        }
                        else
                        {
                            // ReportViewer1.Visible = false;
                            Session["dsExp"] = null;

                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not available for given search criteria";
                          txtFlightFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        // ReportViewer1.Visible = false;
                        Session["dsExp"] = null;
                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        txtFlightFromdate.Focus();
                        return;


                    }
                }
                else
                {
                    ReportViewer1.Visible = false;

                }
            }


            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                SaveUserActivityLog(ErrorLog);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
                if (Dataset2 != null)
                {
                    Dataset2.Dispose();
                }
                if (Dataset1 != null)
                {
                    Dataset1.Dispose();
                }
                
            }



        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsrptAWBMovement_dtrptAWBMovementSUB", Dataset2.Tables[0]));
        }
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Country:" + ddlCountry.Text.ToString() + "Region:" + ddlRegion.Text.ToString() + "Origion:" + ddlOrigin.Text.ToString() + "AWB Prefix:" +txtAwbPrefix.Text.ToString() + "AWB Number:" +txtAwbNumber.Text.ToString() + "Flight Number:" + ddlFlightNumber.Text.ToString() + ", Flight From:" + txtFlightFromdate.Text.ToString() + ",Flight To:" + txtFlightToDate.Text.ToString();
            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "AWB Movement", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }


        public string GetReportInterval(DateTime FromDate, DateTime ToDate)
        {
            double DaysConfigured = 0;
            double intReportInterval = (ToDate - FromDate).TotalDays + 1;
            LoginBL objBL = new LoginBL();

            try
            {
                string strOutput = objBL.GetMasterConfiguration("ReportInterval");

                if (strOutput != "")
                    DaysConfigured = Convert.ToDouble(strOutput);
                else
                    DaysConfigured = 0;
            }
            catch
            {
                DaysConfigured = 0;
            }
            finally
            {
                objBL = null;
            }

            if (DaysConfigured > 0 && intReportInterval > DaysConfigured)
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            else
                return "";
        }
        //private void WRAXBDetails_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")
        //    if (dname == "dsShowAWBMovement")
        //    {
        //        e.Data = Dataset1;
        //    }
        //    else
        //    {
        //        e.Data = Dataset2;
        //    }
        //}

        protected void btnclear_Click(object sender, EventArgs e)
        {
            ReportViewer1.Visible = false;
            //txtAutoDest.Text = "";
            ddlDestination.SelectedIndex = 0;
           ddlOrigin.SelectedIndex=0;//.Text = "";
            ddlCountry.SelectedIndex = 0;// "";
           // txtFlightNo.Text = "";
            txtFlightToDate.Text = "";
            ddlRegion.SelectedIndex=0;//.Text = "";
            txtFlightFromdate.Text = "";
            lblStatus.Text = "";
        }

        private bool Validate()
        {
            try
            {
                try
                {
                    if (txtFlightFromdate.Text.Trim() != "" || txtFlightToDate.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtFlightFromdate.Focus();
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtFlightFromdate.Focus();
                    return false;
                }

               
            }
            catch (Exception ex)
            {


            }

            ReportBAL objBal = new ReportBAL();
            string strResult = string.Empty;

            try
            {
                strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFlightFromdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtFlightToDate.Text.Trim(), "dd/MM/yyyy", null));
            }
            catch
            {
                strResult = "";
            }
            finally
            {
                objBal = null;
            }

            if (strResult != "")
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = strResult;
                txtFlightFromdate.Focus();
                return false;
            }

            return true;
        }

        public void Getdata()
        {
            DataSet ds = new DataSet("rptAWBMovmnt_dsAWBmvmntlist");
            string ErrorLog = string.Empty; 
            Session["dsExp"] = null;
            ReportViewer1.Visible = false;
            if(Validate()==false)
            {
                Session["dsExp"] = null;
                ReportViewer1.Visible = false;
                return;
            }
            try
            {
                if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                {
                    //lblStatus.ForeColor = Color.Red;
                    //lblStatus.Text = "Please enter From date and To date for datewise Schedule List";
                    //txtFlightFromdate.Focus();
                    //return;
                }
                if (txtFlightFromdate.Text.Trim() != "" || txtFlightToDate.Text.Trim() != "")
                {
                    DateTime dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    int chk = DateTime.Compare(dt1, dt2);
                    if (chk > 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To date";
                        //txtFromdate.Focus();
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                //txtFromdate.Focus();
                return;
            }



            try
            {
                int rowIndex = 0;

                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    //if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                    //{
                    //    lblStatus.ForeColor = Color.Red;
                    //    lblStatus.Text = "Please Insert From date and To date for datewise Schedule List";
                    //    txtFlightFromdate.Focus();
                    //    return;
                    //}

                    if (txtFlightFromdate.Text != "")
                    {
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss");
                        // Flightfromdate = Convert.ToDateTime(txtFlightFromdate.Text).ToString("dd/MM/yyyy");
                        //  dt1 = DateTime.Parse(txtFlightFromdate.Text);//.ToString("dd/MM/yyyy");
                        //  Flightfromdate = txtFlightFromdate.Text;
                        //DateTime dtFromDate = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                        dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);

                        Flightfromdate = dt1.ToString("MM/dd/yyyy");
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        //FlightToDate = Convert.ToDateTime(txtFlightToDate.Text).ToString("dd/MM/yyyy");
                        // dt2 = DateTime.Parse(txtFlightToDate.Text);//.ToString("dd/MM/yyyy");
                        dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                        FlightToDate = dt2.ToString("MM/dd/yyyy");


                    }
                }
                catch (Exception ex)
                {

                }


                string FlightNo = "ALL";
                // FlightNo = ddlFlight.SelectedValue;

                //FlightNo = ddlFlight.SelectedItem.Text; mod
                if (ddlFlightNumber.SelectedItem.Text.Trim() == "ALL")
                    FlightNo = "All";
                else
                    FlightNo = ddlFlightNumber.SelectedValue.ToString().Trim();//.Text;

                //FlightNo = ddlFlight.SelectedItem.Text; mod
                //if (txtFlightNo.Text == "")
                //    FlightNo = "All";
                //else
                //    FlightNo = txtFlightNo.Text;

                // FlightNo = "All";
                string strdomestic = "";
                //if (chkDomestic.Checked == true && chkInternational.Checked == true)
                //{
                strdomestic = "All";
                //}
                //else if (chkDomestic.Checked == true)
                //{
                //    strdomestic = "true";
                //}
                //else
                //{
                //    strdomestic = "False";

                //}

                // DataSet ds = OBJasb.GetAirlineSchedule(ddlOrigin.SelectedValue, ddlDestination.SelectedValue, FlightNo, ddlAirCraftType.SelectedValue, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic);
                string Source = "All", Dest = "All";
                if (ddlOrigin.SelectedItem.Text.Trim() != "")
                {
                    Source = ddlOrigin.SelectedItem.Text;
                }
                if (ddlDestination.SelectedItem.Text.Trim() != "")
                {
                    Dest = ddlDestination.SelectedItem.Text.Trim();
                }
                string country = "All";
                if (ddlCountry.SelectedItem.Text.Trim() != "")
                {
                    country = ddlCountry.SelectedItem.Text;
                }


                string Region = "All";
                if (ddlRegion.SelectedItem.Text.Trim() != "")
                {
                    Region = ddlRegion.SelectedItem.Text;
                }

                string AwbNumber = "";
                if (txtAwbNumber.Text.Trim() != "")
                {
                    AwbNumber = txtAwbNumber.Text.Trim();
                }

                string AwbPrefix = "";
                {
                    AwbPrefix = txtAwbPrefix.Text.Trim();
                
                }
                //if (ddlAwbNumber.SelectedItem.Text.Trim() != "")
                //{
                //    AwbNumber = ddlAwbNumber.SelectedValue.ToString().Trim();//.Text.Trim();
                //}
                //string AwbPrefix = "";
                //if (ddlAwbPrefix.Text.Trim() != "")
                //{ AwbPrefix = ddlAwbPrefix.SelectedValue.ToString().Trim();
                //}//;//.Text.Trim(); }
                string Aircraft = "All";
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;

                bool chkStatus = MaBal.CheckConfiguration("AWBMovementReport", "ShowDataInOneline");

                if (!chkStatus)
                    ds = OBJasb.GetAWBMovement(country, Region, Source, Dest, FlightNo, Aircraft, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic, AwbNumber, AgentCode, AwbPrefix);
                else
                    ds = OBJasb.GetAWBMovemenInOneLine(country, Region, Source, Dest, FlightNo, Aircraft, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic, AwbNumber, AgentCode, AwbPrefix);


                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            Session["dsExp"] = ds;

                            DataTable myDataTable = new DataTable();
                            DataColumn myDataColumn;
                            DataSet Ds = new DataSet("rptAWBMovmnt_dsAWBmvmntgetExpt");

                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "Country";
                            //myDataTable.Columns.Add(myDataColumn);


                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "Region";
                            //myDataTable.Columns.Add(myDataColumn);

                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "Origin";
                            //myDataTable.Columns.Add(myDataColumn);

                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "Destination";
                            //myDataTable.Columns.Add(myDataColumn);

                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "FlightID";
                            //myDataTable.Columns.Add(myDataColumn);

                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "FromDt";
                            //myDataTable.Columns.Add(myDataColumn);

                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "ToDt";
                            //myDataTable.Columns.Add(myDataColumn);

                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "AWBNo";
                            //myDataTable.Columns.Add(myDataColumn);

                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "ReportDate";
                            //myDataTable.Columns.Add(myDataColumn);

                            //DataRow dr;
                            //dr = myDataTable.NewRow();
                            //dr["Country"] = txtCountry.Text.Trim();
                            //dr["Region"] = txtRegion.Text.Trim(); //"5";
                            //dr["Origin"] = txtAutoSource.Text.Trim() + ":";// "5";
                            //dr["Destination"] = txtAutoDest.Text.Trim();
                            //dr["FlightID"] = txtFlightNo.Text.Trim();

                            //dr["FromDt"] = txtFlightFromdate.Text.Trim();
                            //dr["ToDt"] = txtFlightToDate.Text;// "9";
                            //dr["AWBNo"] = txtAWBNo.Text.Trim();
                            //dr["ReportDate"] = CurrTime;
                            //myDataTable.Rows.Add(dr);

                            ////  Ds.Tables.Add(myDataTable);

                            //DataSet dschk = new DataSet();
                            //dschk.Tables.Add(myDataTable);
                            //Dataset2 = dschk.Copy();


                            //Dataset1 = ds.Copy();
                            #region old code
                            //rptViewerAWBMovement.Visible = true;
                            ////  System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "AWB.rdlc";
                            //info = new FileInfo(Server.MapPath("/Reports/rptABMovement.rdlx"));
                            //   // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"]+ "rptABMovement.rdlx");
                            //  definition = new ReportDefinition(info);
                            //  runtime = new ReportRuntime(definition);


                            //  //  Dataset2 = dschk;

                            //  runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                            // rptViewerAWBMovement.SetReport(runtime);
                            #endregion

                            #region Adding Filters to Report
                            //System.IO.MemoryStream Logo = null;
                            //try
                            //{
                            //    Logo = CommonUtility.GetImageStream(Page.Server);
                            //    //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                            //}
                            //catch (Exception ex)
                            //{
                            //    Logo = new System.IO.MemoryStream();
                            //}

                            //if (ds.Tables[0].Columns.Contains("Logo") == false)
                            //{
                            //    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                            //    col1.DefaultValue = Logo.ToArray();
                            //    ds.Tables[0].Columns.Add(col1);
                            //}


                            #endregion
                            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptAWBMovement.rdlc");
                            //ReportDataSource datasource = new ReportDataSource("dsrptAWBMovement_dtrptAWBMovement", ds.Tables[0]);
                            //ReportViewer1.LocalReport.DataSources.Clear();
                            //ReportViewer1.LocalReport.DataSources.Add(datasource);
                            //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            lblStatus.Text = string.Empty;
                            btnExport.Visible = true;
                            SaveUserActivityLog("");

                            // dt.Rows[0]["From"]= txtAutoSource.Text.Trim();


                        }
                        else
                        {
                            // ReportViewer1.Visible = false;

                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not available for given search criteria";
                            txtFlightFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        // ReportViewer1.Visible = false;

                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        txtFlightFromdate.Focus();
                        return;


                    }
                }
            }


            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                SaveUserActivityLog(ErrorLog);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
                if (Dataset2 != null)
                {
                    Dataset2.Dispose();
                }
                if (Dataset1 != null)
                {
                    Dataset1.Dispose();
                }

            }


        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = new DataSet("rptAWBMovmnt_dsAWBMvmntExpt");
            DataTable dt = new DataTable("rptAWBMovmnt_dtAWBMvmntExpt");

            try
            {
             //   if ((DataSet)Session["dsExp"] == null)
                    //if(ds == null)
                if (Validate() == false)
                {
                    Session["dsExp"] = null;

                    return;
                }
                    Getdata();


                    dsExp = (DataSet)Session["dsExp"];
                    if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                        dt = (DataTable)dsExp.Tables[0];
                    else
                        return;
            																						

                
                
                    dt.Columns["AWBPrefix"].ColumnName = "AWB Prefix";
                    dt.Columns["AWBNumber"].ColumnName = "AWB Number";
                    dt.Columns["FlightNo"].ColumnName = "Flight";
                    dt.Columns["FltDate"].ColumnName = "Flight Date";
                    dt.Columns["Origin"].ColumnName = "AWB Origin";
                    dt.Columns["Destination"].ColumnName = "AWB Destination";
                    dt.Columns["FlightOrigin"].ColumnName = "Flight Origin";
                    dt.Columns["FlightDestination"].ColumnName = "Flight Destination";
                    dt.Columns["Booked"].ColumnName = "Booked Pcs";
                    dt.Columns["BookedWeight"].ColumnName = "Booked Wt";
                    dt.Columns["AcceptedPcs"].ColumnName = "Accepted Pcs";
                    dt.Columns["AcceptedWgt"].ColumnName = "Accepted Wt";
                    dt.Columns["Manifested"].ColumnName = "Manifested Pcs";
                    dt.Columns["ManifestedWeight"].ColumnName = "Manifested Wt";
                    dt.Columns["Arrival"].ColumnName = "Arrival Pcs";
                    dt.Columns["ArrivalWeight"].ColumnName = "Arrival Wt";
                    dt.Columns["Delivered"].ColumnName = "Delivered Pcs";
                    dt.Columns["DeliveredWeight"].ColumnName = "Delivered Wt";
                    dt.Columns["DlvrDate"].ColumnName = "Delivery Date";
                    dt.Columns["DlvrTime"].ColumnName = "Delivery Time";
                    dt.Columns["IsVoid"].ColumnName = "Is Void";
                    dt.Columns["ExecutedBy"].ColumnName = "Booked By";
                    dt.Columns["CommodityDesc"].ColumnName = "Content";
                   
                string attachment = "attachment; filename=AWBMovementReport.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
            catch (Exception ex)
            { }
            finally
            {if(dsExp!=null)
                dsExp.Dispose();
                if(dt!=null)
                dt.Dispose();
            }
        }

        protected void ddlFlightPrefix_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FlightPrefixCode = ddlFlightPrefix.SelectedItem.Value.ToString();
            GetFlight(FlightPrefixCode);
        }
        public void GetFlight(string FlightPrefixCode)
        {
            DataSet dsResult = new DataSet("rptAWBMovmnt_dsawbmvmntFlt");
            try
            {
                if (ddlFlightPrefix.SelectedItem.Value.ToString() == "Select")
                {
                    ddlFlightNumber.DataSource = "";
                    ddlFlightNumber.DataBind();
                }


                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                dsResult = objSQL.SelectRecords("spGetAllFlightListPrefixWise", "FlightPrefix", FlightPrefixCode, SqlDbType.VarChar);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            ddlFlightNumber.Items.Clear();
                            ddlFlightNumber.DataSource = dsResult.Tables[0];
                            ddlFlightNumber.DataTextField = "FlightID";
                            ddlFlightNumber.DataValueField = "FlightID";
                            ddlFlightNumber.DataBind();
                            ddlFlightNumber.Items.Insert(0, new ListItem("ALL", ""));
                            ddlFlightNumber.SelectedIndex = -1;
                        }

                        else
                        {
                            ddlFlightNumber.Items.Clear();
                            lblStatus.Text = "No Flight for this Partner";
                            lblStatus.ForeColor = Color.Red;
                            ddlFlightNumber.DataSource = null;
                            ddlFlightNumber.DataBind();
                            ddlFlightNumber.Items.Insert(0, new ListItem("Select", null));
                        }
                    }
                }


            }
            catch (Exception ex)
            { }
            finally
            {
                if (dsResult != null)
                    dsResult.Dispose();
            }
         
        }
        public void GetFlights()
        {
             DataSet dsInstance = new DataSet("rptAWBMovmnt_dsAWBMvmntFltList");
            DataSet dsResult = new DataSet("rptAWBMovmnt_dsAWBMvmntGetFlt");
            try
            {
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
               
                //string FlightPrefix;
                dsInstance = objSQL.SelectRecords("GetCurrentInstance");
                string current = dsInstance.Tables[0].Rows[0][0].ToString();
                //  FlightPrefix = ddlFlightPrefix.SelectedValue.ToString().Trim();
                {
                    
                    string errormessage = "";
                    //if (objBAL.GetAllFlightsNew(source, dest, date, ref dsResult, ref errormessage))
                    string procedure = "sp_GetFlightPrefix";
                    dsResult = objSQL.SelectRecords(procedure);
                    if (dsResult != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                ddlFlightPrefix.Items.Clear();
                                ddlFlightPrefix.DataSource = dsResult.Tables[0];
                                ddlFlightPrefix.DataTextField = "PartnerCode";
                                ddlFlightPrefix.DataValueField = "PartnerCode";
                                ddlFlightPrefix.DataBind();

                                ddlFlightPrefix.SelectedValue = current;
                                GetFlight(current);

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
            finally
            { 
            if(dsInstance!=null)
                dsInstance.Dispose();
            if (dsResult != null)
                dsResult.Dispose();
            }

        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlDestination_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    
    }
}
