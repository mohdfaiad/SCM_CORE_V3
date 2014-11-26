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
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using QID.DataAccess;
using Microsoft.Reporting.WebForms;


namespace ProjectSmartCargoManager
{
    public partial class Report_Void : System.Web.UI.Page
    {
        ArrayList arFlight = new ArrayList();
        ReportBAL OBJasb = new ReportBAL();
        private DataSet Dataset1=new DataSet("VoidAWB_dsDataset1");
        private DataSet Dataset2=new DataSet("VoidAWB_dsDataset2");
        ReportBAL objReport = new ReportBAL();
        ReportBAL rptBAl = new ReportBAL();
        DataSet ds=new DataSet("VoidAWB_ds");
        static string AgentCode = "";
        public static string CurrTime = "";
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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

                    LoadOrgDropDown();


                    if (Session["awbPrefix"] != null)
                        txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                    else
                    {
                        MasterBAL objBal = new MasterBAL();
                        Session["awbPrefix"] = objBal.awbPrefix();
                        txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                    }
                    //AirCraftType();
                }
            }
            catch (Exception)
            {
            }
        }

        protected void LoadOrgDropDown()
        {
            DataSet ds = new DataSet("VoidAWB_dsAirportCodes");
        try
        { 
            ds = da.SelectRecords("spGetAirportCodes");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ddlOrg.DataSource = ds.Tables[0];
                ddlOrg.DataTextField = "Airport";
                ddlOrg.DataValueField = "AirportCode";
                ddlOrg.DataBind();
                ddlOrg.Items.Insert(0,new ListItem("Select","All"));
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
            DataSet ds = new DataSet("VoidAWB_dsVoidAWBs");
            string ErrorLog = string.Empty;
          //  lblStatus.Text = string.Empty;
            if(Validate()==false)
            {
                ReportViewer1.Visible = false;
                return;
            }
            try
            {
                if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                {
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
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                return;
            }



            try
            {
                int rowIndex = 0;

                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                string strfromdate = "";
                string strtodate = "";
                // DateTime dtto;
                try
                {

                    if (txtFlightFromdate.Text != "")
                    {
                        //string day = txtFlightFromdate.Text.Substring(0, 2);
                        //string mon = txtFlightFromdate.Text.Substring(3, 2);
                        //string yr = txtFlightFromdate.Text.Substring(6, 4);
                        //strfromdate = yr + "-" + mon + "-" + day;
                        ////Flightfromdate = strtodate;
                        //Flightfromdate= Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss");
                        ////dtto = Convert.ToDateTime(strtodate);
                        dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);

                        Flightfromdate = dt1.ToString("dd/MM/yyyy");
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        //    string day = txtFlightToDate.Text.Substring(0, 2);
                        //    string mon = txtFlightToDate.Text.Substring(3, 2);
                        //    string yr = txtFlightToDate.Text.Substring(6, 4);
                        //    strtodate = yr + "-" + mon + "-" + day;
                        //    FlightToDate = Convert.ToDateTime(strtodate).ToString("yyyy-MM-dd HH:mm:ss");
                        dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                        FlightToDate = dt2.ToString("dd/MM/yyyy");


                    }
                }
                catch (Exception ex)
                {

                }


                string FlightNo = "";
                if (txtFlightNo.Text == "")
                    FlightNo = "All";
                else
                    FlightNo = txtFlightNo.Text;

                string strdomestic = "";
                strdomestic = "All";
                string Source = "", Dest = "All";
                if (ddlOrg.SelectedIndex > 0)
                    Source = ddlOrg.SelectedValue.ToString();
                //if (txtAutoSource.Text.Trim() != "")
                //{
                //    Source = txtAutoSource.Text;
                //}
                if (txtAutoDest.Text.Trim() != "")
                {
                    Dest = txtAutoDest.Text.Trim();
                }
                string country = "All";
                if (txtCountry.Text.Trim() != "")
                {
                    country = txtCountry.Text;
                }


                string Region = "All";
                if (txtRegion.Text.Trim() != "")
                {
                    Region = txtRegion.Text;
                }

                string AWBNumber = "";
                if (txtAWBNo.Text.Trim() != "")
                {
                    AWBNumber = txtAWBNo.Text.Trim();
                }

                string Aircraft = "All";
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;

                ds = OBJasb.GetAWBVoidData(Source, AgentCode, Flightfromdate, FlightToDate, AWBNumber);

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {


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

                            DataTable myDataTable = new DataTable();
                            DataColumn myDataColumn;
                            DataSet Ds = new DataSet();

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "station";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "agent";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "frmDate";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ToDate";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "AwbNo";
                            myDataTable.Columns.Add(myDataColumn);

                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "region";
                            //myDataTable.Columns.Add(myDataColumn);
                            lblStatus.Text = string.Empty;

                            DataRow dr;
                            dr = myDataTable.NewRow();
                            dr["station"] = ddlOrg.SelectedValue.ToString(); //txtAutoSource.Text.Trim();
                            dr["agent"] = txtFlightNo.Text;
                            dr["frmDate"] = txtFlightFromdate.Text.Trim();
                            dr["ToDate"] = txtFlightToDate.Text;// "9";
                            dr["AwbNo"] = txtAWBNo.Text.Trim();
                            //dr["region"] = txtRegion.Text;

                            myDataTable.Rows.Add(dr);
                            DataSet dschk = new DataSet();
                            dschk.Tables.Add(myDataTable);
                            Dataset2 = dschk.Copy();


                            Dataset1 = ds.Copy();

                            Session["DsExp"] = ds;

                            //rptViewerAWBMovement.Visible = true;
                            //info = new FileInfo(Server.MapPath("/Reports/rptVoidAWBs.rdlx"));
                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                            //rptViewerAWBMovement.SetReport(runtime);
                            ReportViewer1.Visible = true;
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report_Void.rdlc");
                            ReportDataSource datasource = new ReportDataSource("dsReport_Void_dtReport_Void", ds.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(datasource);
                            SaveUserActivityLog("");
                            btnExport.Visible = true;
                        }
                        else
                        {
                            ReportViewer1.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                          txtFlightFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
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
                
                if (Dataset2 != null)
                {
                    Dataset2.Dispose();
                }
                if (Dataset1 != null)
                {
                    Dataset1.Dispose();
                }
                if (ds != null)
                {
                    ds  .Dispose();
                }
            }

        }

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Origin:" + ddlOrg.SelectedValue + ", Valid From:" + txtFlightFromdate.Text.ToString() + ",Valid To:" + txtFlightToDate.Text.ToString()+",AWB Number:"+txtAWBNo.Text.ToString();
            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Void AWB", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
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
        //    if (dname == "DataSetValue")
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
            txtAutoDest.Text = "";
            ddlOrg.SelectedIndex = 0;
            //txtAutoSource.Text = "";
            txtCountry.Text = "";
            txtFlightNo.Text = "";
            txtFlightToDate.Text = "";
            txtRegion.Text = "";
            txtFlightFromdate.Text = "";
            lblStatus.Text = "";
            txtAWBNo.Text = "";
        }

        protected void txtCountry_TextChanged(object sender, EventArgs e)
        {
           
        }

        private bool Validate()
        {
            try
            {
                try
                {
                    if (txtFlightFromdate.Text.Trim() != "" ||txtFlightToDate.Text.Trim() != "")
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
  
        #region Auto Populate
        
        [System.Web.Services.WebMethod()]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetStation(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'

            SqlDataAdapter dad = new SqlDataAdapter("SELECT AirportCode from AirportMaster where AirportName like '" + prefixText + "%' or AirportCode like '" + prefixText + "%'", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }

        #endregion

        public void Getdata()
        {
            DataSet ds =new DataSet("VoidAWB_dsGetData");
            string ErrorLog = string.Empty;
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
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                return;
            }



            try
            {
                int rowIndex = 0;

                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                string strfromdate = "";
                string strtodate = "";
                // DateTime dtto;
                try
                {

                    if (txtFlightFromdate.Text != "")
                    {
                        //string day = txtFlightFromdate.Text.Substring(0, 2);
                        //string mon = txtFlightFromdate.Text.Substring(3, 2);
                        //string yr = txtFlightFromdate.Text.Substring(6, 4);
                        //strfromdate = yr + "-" + mon + "-" + day;
                        ////Flightfromdate = strtodate;
                        //Flightfromdate= Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss");
                        ////dtto = Convert.ToDateTime(strtodate);
                        dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);

                        Flightfromdate = dt1.ToString("dd/MM/yyyy");
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        //    string day = txtFlightToDate.Text.Substring(0, 2);
                        //    string mon = txtFlightToDate.Text.Substring(3, 2);
                        //    string yr = txtFlightToDate.Text.Substring(6, 4);
                        //    strtodate = yr + "-" + mon + "-" + day;
                        //    FlightToDate = Convert.ToDateTime(strtodate).ToString("yyyy-MM-dd HH:mm:ss");
                        dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                        FlightToDate = dt2.ToString("dd/MM/yyyy");


                    }
                }
                catch (Exception ex)
                {

                }


                string FlightNo = "";
                if (txtFlightNo.Text == "")
                    FlightNo = "All";
                else
                    FlightNo = txtFlightNo.Text;

                string strdomestic = "";
                strdomestic = "All";
                string Source = "", Dest = "All";
                if (ddlOrg.SelectedIndex > 0)
                    Source = ddlOrg.SelectedValue.ToString();
                //if (txtAutoSource.Text.Trim() != "")
                //{
                //    Source = txtAutoSource.Text;
                //}
                if (txtAutoDest.Text.Trim() != "")
                {
                    Dest = txtAutoDest.Text.Trim();
                }
                string country = "All";
                if (txtCountry.Text.Trim() != "")
                {
                    country = txtCountry.Text;
                }


                string Region = "All";
                if (txtRegion.Text.Trim() != "")
                {
                    Region = txtRegion.Text;
                }

                string AWBNumber = "";
                if (txtAWBNo.Text.Trim() != "")
                {
                    AWBNumber = txtAWBNo.Text.Trim();
                }

                //string Aircraft = "All";
                //FileInfo info;
                //ReportRuntime runtime;
                //ReportDefinition definition;

                ds = OBJasb.GetAWBVoidData(Source, AgentCode, Flightfromdate, FlightToDate, AWBNumber);

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {


                            DataTable myDataTable = new DataTable();
                            // DataColumn myDataColumn;
                            DataSet Ds = new DataSet("VoidAWB_dsExport");

                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "station";
                            //myDataTable.Columns.Add(myDataColumn);


                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "agent";
                            //myDataTable.Columns.Add(myDataColumn);


                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "frmDate";
                            //myDataTable.Columns.Add(myDataColumn);


                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "ToDate";
                            //myDataTable.Columns.Add(myDataColumn);


                            //myDataColumn = new DataColumn();
                            //myDataColumn.DataType = Type.GetType("System.String");
                            //myDataColumn.ColumnName = "AwbNo";
                            //myDataTable.Columns.Add(myDataColumn);

                            ////myDataColumn = new DataColumn();
                            ////myDataColumn.DataType = Type.GetType("System.String");
                            ////myDataColumn.ColumnName = "region";
                            ////myDataTable.Columns.Add(myDataColumn);


                            //DataRow dr;
                            //dr = myDataTable.NewRow();
                            //dr["station"] = txtAutoSource.Text.Trim();
                            //dr["agent"] = txtFlightNo.Text;
                            //dr["frmDate"] = txtFlightFromdate.Text.Trim();
                            //dr["ToDate"] = txtFlightToDate.Text;// "9";
                            //dr["AwbNo"] = txtAWBNo.Text.Trim();
                            ////dr["region"] = txtRegion.Text;

                            //myDataTable.Rows.Add(dr);
                            //DataSet dschk = new DataSet();
                            //dschk.Tables.Add(myDataTable);
                            //Dataset2 = dschk.Copy();


                            //Dataset1 = ds.Copy();



                            //rptViewerAWBMovement.Visible = true;
                            //info = new FileInfo(Server.MapPath("/Reports/rptVoidAWBs.rdlx"));
                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                            //rptViewerAWBMovement.SetReport(runtime);

                            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report_Void.rdlc");
                            //ReportDataSource datasource = new ReportDataSource("dsReport_Void_dtReport_Void", ds.Tables[0]);
                            //ReportViewer1.LocalReport.DataSources.Clear();
                            //ReportViewer1.LocalReport.DataSources.Add(datasource);
                            Session["DsExp"] = ds;
                            SaveUserActivityLog("");
                            btnExport.Visible = true;
                        }
                        else
                        {
                            // ReportViewer1.Visible = false;
                            Session["DsExp"] = null;
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
                        Session["DsExp"] = null;

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
                
                if (Dataset2 != null)
                {
                    Dataset2.Dispose();
                }
                if (Dataset1 != null)
                {
                    Dataset1.Dispose();
                }
                if (ds != null)
                {
                    ds.Dispose();
                }
            }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = new DataSet("VoidAWB_dsExportData");
            DataTable dt = new DataTable("VoidAWB_dsExportData");

            try
            {
               // if ((DataSet)Session["dsExp"] == null)
                    //if(ds == null)
                    Getdata();
                    

                dsExp = (DataSet)Session["dsExp"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                {
                    lblStatus.Visible = false;
                    dt = (DataTable)dsExp.Tables[0];

                }
                else
                    return;
                dt = (DataTable)dsExp.Tables[0];
                //dt = city.GetAllCity();//your datatable 
                string attachment = "attachment; filename=Report_Void.xls";
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
            {
                dsExp = null;
                dt = null;
            }
        }
    }
}
