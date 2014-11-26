using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization; 
using System.Data;
using QID.DataAccess;
using System.Drawing;
using BAL;
using System.Text;
using System.IO;

//using DataDynamics.Reports.Rendering.Pdf;
//using DataDynamics.Reports.Rendering.IO;
//using DataDynamics.Reports.Rendering.Excel;
using Microsoft.Reporting.WebForms;
namespace ProjectSmartCargoManager
{
    //18-6
    public partial class ShowDelivery : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BLArrival arr = new BLArrival();
        MasterBAL objBal = new MasterBAL();
        string preAWB = string.Empty;
        DataSet dset;
        ReportDataSource rds1 = new ReportDataSource();
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["awbPrefix"] != null)
                {
                    preAWB = Session["awbPrefix"].ToString();

                }
                else
                {
                    MasterBAL objBal = new MasterBAL();
                    Session["awbPrefix"] = objBal.awbPrefix();
                    preAWB = Session["awbPrefix"].ToString();
                }
                if (!IsPostBack)
                {
                    txtflightDate.Text = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd");
                    //Print();
                    LoadGrid();
                    getflightnumber();
                }
            }
            catch (Exception ex) { }  
        }
        #endregion Load
        #region BtnPrint
        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            double ST;
            string CustomerSupport; string DODate; string station; string staffid; 
            int DOChecked = 0;
            try
            {
                for (int j = 0; j < grdDeliveryDetails.Rows.Count; j++)
                {
                    if (((CheckBox)grdDeliveryDetails.Rows[j].FindControl("check")).Checked == true)
                    {
                        DOChecked = DOChecked + 1;
                    }
                }
                if (DOChecked == 0)
                {
                    lblStatus.Text = "Please Select DO to Reprint";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            { }
            for (int j = 0; j < grdDeliveryDetails.Rows.Count; j++)
            {
                if (((CheckBox)grdDeliveryDetails.Rows[j].FindControl("check")).Checked == true)
                {
                    DataTable dtSurface = new DataTable();
                    dtSurface.Columns.Add("AgentName", typeof(string));
                    dtSurface.Columns.Add("AWBNumber", typeof(string));
                    dtSurface.Columns.Add("FlightNumber", typeof(string));
                    dtSurface.Columns.Add("ActualPieces", typeof(string));
                    dtSurface.Columns.Add("ActualWeight", typeof(string));
                    dtSurface.Columns.Add("IssueName", typeof(string));
                    dtSurface.Columns.Add("IssuedTo", typeof(string));
                    dtSurface.Columns.Add("IssueDate", typeof(string));
                    dtSurface.Columns.Add("HAWBNo", typeof(string));
                    dtSurface.Columns.Add("Consignee", typeof(String));
                    dtSurface.Columns.Add("ReciverName", typeof(String));
                    //dtSurface.Columns.Add("IssueDate", typeof(DateTime));
                    //dtSurface.Columns.Add("Desc", typeof(String));
                    dtSurface.Columns.Add("DoNumber", typeof(String));
                    dtSurface.Columns.Add("CommCode", typeof(string));
                    dtSurface.Columns.Add("CCAmount", typeof(string));
                    dtSurface.Columns.Add("PayMode", typeof(string));
                    dtSurface.Columns.Add("Discription", typeof(string));
                    dtSurface.Columns.Add("CustomerSupport", typeof(string));
                    dtSurface.Columns.Add("Remarks", typeof(string));
                    dtSurface.Columns.Add("DODate", typeof(string));
                    dtSurface.Columns.Add("Station", typeof(string));
                    dtSurface.Columns.Add("StaffId", typeof(string));
                    dtSurface.Columns.Add("ServiceTax", typeof(string));


                    // for (int i = 0; i < grdDeliveryDetails.Rows.Count; i++)
                    {
                        DataSet ds = (DataSet)Session["DsDoDetails"];

                        DataRow drSurface = dtSurface.NewRow();
                        drSurface["AgentName"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("txtagentname")).Text; ;
                        string AWBNumber = ((Label)grdDeliveryDetails.Rows[j].FindControl("txtawbno")).Text;
                        drSurface["AWBNumber"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("txtawbno")).Text;
                        DataSet ds3 = arr.GetCCAmount(AWBNumber);

                        try
                        {
                            DataSet dsCustomerSupport = da.SelectRecords("Sp_GetCustomerSupportInfo");
                            if (dsCustomerSupport.Tables[0].Rows.Count > 0)
                            {
                                CustomerSupport = dsCustomerSupport.Tables[0].Rows[0]["CustomerSupport"].ToString();
                            }
                            else
                            {
                                CustomerSupport = "";
                            }
                        }
                        catch
                        {
                            CustomerSupport = "";
                        }

                        station = Session["Station"].ToString();
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                        DODate = dtIndianTime.ToString();
                        staffid = Session["UserName"].ToString();

                        if (Session["ST"] != null)
                            ST = Convert.ToDouble(Session["ST"].ToString());
                        else
                        {
                            Session["ST"] = objBal.getServiceTax();
                            ST = Convert.ToDouble(Session["ST"].ToString());
                        }

                        //drSurface["AWBNumber"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("txtawbno")).Text;
                        drSurface["FlightNumber"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("txtflightno")).Text;
                        drSurface["ActualPieces"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("txttotalpieces")).Text;
                        drSurface["ActualWeight"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("lbltotalwt")).Text;
                        drSurface["IssueName"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("lblissuename")).Text;
                        drSurface["IssuedTo"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("lblissuedto")).Text;
                        //drSurface["IssueDate"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("lblissuedate")).Text;
                        drSurface["HAWBNo"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("lblhawbnumber")).Text;
                        drSurface["Consignee"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("lblconsignee")).Text;
                        drSurface["ReciverName"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("lblReciversName")).Text;
                        //if (txtflightDate.Text == "")
                        //{
                        //    drSurface["IssueDate"] = DateTime.Now.ToShortDateString();
                        //}
                        //else
                        //{
                        //    drSurface["IssueDate"] = txtflightDate.Text;
                        //}
                        
                        if (txtflightDate.Text == "")
                        {
                            drSurface["IssueDate"] = DateTime.Now.ToShortDateString();
                        }
                        else
                        {
                            drSurface["IssueDate"] = Convert.ToDateTime(txtflightDate.Text).ToString("dd/MM/yyyy");
                        }
                        drSurface["DoNumber"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("lblDoNo")).Text;
                        //drSurface["Desc"] = ds.Tables[0].Rows[j]["Desc"].ToString();
                        drSurface["CommCode"] = ds.Tables[0].Rows[j]["SCC"].ToString();
                        drSurface["Discription"] = ds.Tables[0].Rows[j]["Desc"].ToString();
                        //drSurface["Remarks"] = ds.Tables[0].Rows[j]["Remarks"].ToString();


                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            drSurface["CCAmount"] = ds3.Tables[0].Rows[0]["Total"].ToString();
                        }
                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            drSurface["PayMode"] = "CC";
                        }
                        else
                        {
                            drSurface["PayMode"] = "PP";
                        }
                        if (CustomerSupport != "" && CustomerSupport != null)
                        {
                            drSurface["CustomerSupport"] = CustomerSupport;
                        }
                        else
                        {
                            drSurface["CustomerSupport"] = "";
                        }

                        drSurface["AWBNumber"] = ((Label)grdDeliveryDetails.Rows[j].FindControl("txtawbno")).Text.Trim();

                        drSurface["DODate"] = DODate;
                        drSurface["Station"] = station;
                        drSurface["StaffId"] = staffid;
                        drSurface["ServiceTax"] = ST;

                        //drSurface["RemainingPieces"] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtremainingpieces")).Text; 
                        dtSurface.Rows.Add(drSurface);
                        //If reprinting to be done...
                    }
                    Session["DeliveryDetails"] = dtSurface;

                    //Vijay
                    Session["DoPrintTracking"] = null;
                    Session["abc"] = null;

                }
                //Session["DeliveryDetails"];

            }
            //Response.Redirect("PrintDeliveryForm.aspx", false);
            PrintDeliveryOrderPDF();

        }
        
        #endregion BtnPrint

        #region Print Function
        //public void Print()
        //{
        //try
        //{
           
        //    //DataTable dtSurface = new DataTable();
        //    //dtSurface.Columns.Add("AgentName", typeof(string));
        //    //dtSurface.Columns.Add("AWBNumber", typeof(string));
        //    //dtSurface.Columns.Add("FlightNumber", typeof(string));
        //    //dtSurface.Columns.Add("TotalPieces", typeof(string));
        //    //dtSurface.Columns.Add("ActualWeight", typeof(string));
        //    //dtSurface.Columns.Add("IssueName", typeof(string));
        //    //dtSurface.Columns.Add("IssuedTo", typeof(string));
        //    //dtSurface.Columns.Add("IssueDate", typeof(DateTime));
        //    //dtSurface.Columns.Add("HAWBNumber", typeof(string));
        //    //dtSurface.Columns.Add("Consignee", typeof(String));
        //    //dtSurface.Columns.Add("ReciversName", typeof(String));

        //    string[] paramname = new string[2];
        //    paramname[0] = "FlightNo";
        //    paramname[1] = "Fltdate";

        //    object[] paramvalue = new object[2];
        //    paramvalue[0] = "";
        //    paramvalue[1] ="";

        //    SqlDbType[] paramtype = new SqlDbType[2];
        //    paramtype[0] = SqlDbType.NVarChar;
        //    paramtype[1] = SqlDbType.NVarChar;





        //    DataSet ds = da.SelectRecords("Sp_GetDODetails",paramname,paramvalue,paramtype);

        //    if (ds != null)
        //    {
        //        if (ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {

        //                //((TextBox)grdMaterialDetails.Rows[0].FindControl("txtbookingdate")).Text = System.DateTime.Now.ToString();
                        
        //                grdDeliveryDetails.DataSource = ds.Tables[0];
        //                grdDeliveryDetails.DataBind();
        //                for (int i = 0; i < grdDeliveryDetails.Rows.Count; i++)
        //                {
        //                        DataRow drSurface = dtSurface.NewRow();
        //                        drSurface["AgentName"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("txtagentname")).Text; ;
        //                        drSurface["AWBNumber"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("txtawbno")).Text;
        //                        // drSurface["AWBNumber"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("txtawbno")).Text;
        //                        drSurface["FlightNumber"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("txtflightno")).Text;
        //                        drSurface["TotalPieces"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("txttotalpieces")).Text;
        //                        drSurface["ActualWeight"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("lbltotalwt")).Text;
        //                        drSurface["IssueName"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("lblissuename")).Text;
        //                        drSurface["IssuedTo"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("lblissuedto")).Text;
        //                        drSurface["IssueDate"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("lblissuedate")).Text;
        //                        drSurface["HAWBNumber"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("lblhawbnumber")).Text;
        //                        drSurface["Consignee"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("lblconsignee")).Text;
        //                        drSurface["ReciversName"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("lblReciversName")).Text;
                             
        //                        //drSurface["RemainingPieces"] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtremainingpieces")).Text; 
        //                        dtSurface.Rows.Add(drSurface);
        //                        //If reprinting to be done...
                                    
                            
        //                }
                   
        //            }
        //            Session["DeliveryDetails"] = dtSurface;
                   
        //            //ClientScript.RegisterStartupScript(this.GetType(), "prndo", "PrintDO();", true);
        //        }
        //    }
        //    else
        //    {
        //        LoadGrid();
        //    }

        //}
        //catch (Exception ex)
        //{

        //}


        //}
        #endregion Print Function
        #region GetFlightDropdown
        protected void getflightnumber()
        {
            DataSet ds = da.SelectRecords("SpFlightNo");
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlflightnumber.Items.Clear();
                        ddlflightnumber.Items.Add("Select");
                        ddlflightnumber.DataSource = ds.Tables[0];
                        ddlflightnumber.DataTextField = "FlightNo";
                        ddlflightnumber.DataValueField = "FlightNo";
                        ddlflightnumber.DataBind();
                        ddlflightnumber.SelectedIndex = 0;
                    }
                }
            }
        }
        #endregion GetFlightDropdown
        #region List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtflightDate.Text == "" && ddlflightnumber.SelectedItem.Text == "Select" || ddlflightnumber.SelectedItem.Text == "")
                {
                    lblStatus.Text = "Please Provide Parameter to Search";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                string FlightNo = string.Empty;
                string Fltdate = string.Empty;

                string[] paramname = new string[3];
                paramname[0] = "FlightNo";
                paramname[1] = "Fltdate";
                paramname[2] = "StationCode";


                FlightNo = ddlflightnumber.SelectedItem.Text;
                if (FlightNo == "")
                {
                    FlightNo = "";
                }
                // DateTime sDate, eDate = new DateTime();
                Fltdate = txtflightDate.Text;
                if (Fltdate != "")
                {
                    Fltdate = DateTime.Parse(Fltdate).ToString("yyyy-MM-dd");
                }

                //else
                //{
                //    DateTime str = DateTime.Parse(txtflightDate.Text);
                //    Fltdate = str.ToString("dd-dd-yyyy");
                //}
                object[] paramvalue = new object[3];

                paramvalue[0] = FlightNo;
                paramvalue[1] = DateTime.ParseExact(Fltdate,"yyyy-MM-dd",null);
                paramvalue[2] = Session["Station"].ToString();

                SqlDbType[] paramtype = new SqlDbType[3];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.DateTime;
                paramtype[2] = SqlDbType.VarChar;

                Session["Fltdate"] = txtflightDate.Text;
                Session["flight"] = ddlflightnumber.SelectedItem.Text;
                DataSet ds = da.SelectRecords("Sp_GetDODetails", paramname, paramvalue, paramtype);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["DsDoDetails"] = ds;

                            //((TextBox)grdMaterialDetails.Rows[0].FindControl("txtbookingdate")).Text = System.DateTime.Now.ToString();

                            grdDeliveryDetails.DataSource = ds.Tables[0];
                            grdDeliveryDetails.DataBind();


                        }
                    }
                }
            }
            catch (Exception ex)
            { }
 
        }
        #endregion List
        #region LoadgridInfo
        public void LoadGrid()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AgentName";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AWBNumber";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "TotalPieces";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ActualWeight";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FlightNumber";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "HAWBNumber";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "IssueDate";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "IssuedTo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "IssueName";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ReciversName";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Consignee";
            myDataTable.Columns.Add(myDataColumn);

             myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DONumber";
            myDataTable.Columns.Add(myDataColumn);
            


        


            DataRow dr;
            dr = myDataTable.NewRow();
            dr["AgentName"] = "";
            dr["AWBNumber"] = "";//"5";
            dr["TotalPieces"] = "";// "5";
            dr["ActualWeight"] = "";// "9";
            dr["FlightNumber"] = "";
            dr["HAWBNumber"] = "";
            dr["IssueDate"] = "";
            dr["IssuedTo"] = "";
            dr["IssueName"] = "";
            dr["ReciversName"] = "";
            dr["Consignee"] = "";
            dr["DONumber"] = "";
            

            myDataTable.Rows.Add(dr);
            //ViewState["CurrentTable"] = myDataTable;
            //Bind the DataTable to the Grid

            grdDeliveryDetails.DataSource = null;
            grdDeliveryDetails.DataSource = myDataTable;
            grdDeliveryDetails.DataBind();
        }
        #endregion LoadgridInfo

        protected void PrintDeliveryOrderPDF()
        {
            try
            {
                try
                {
                    if (Session["DeliveryDetails"] != null)
                    {
                        dset = new DataSet("Table");
                        dset.Tables.Add((DataTable)Session["DeliveryDetails"]);
                        if (dset != null)
                        {
                            if (dset.Tables.Count > 0)
                            {
                                if (dset.Tables[0].Rows.Count > 0)
                                {
                                    ////Session["PLIData"] = dset;
                                    //DataDynamics.Reports.ReportDefinition _reportDef = new ReportDefinition();
                                    ////DataDynamics.Reports.ReportDefinition _reportDef1 = new ReportDefinition();
                                    //_reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/DeliveryOrder.rdlx")));
                                    //// _reportDef1 = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/ExcludedDetails.rdlx")));


                                    ////Dataset1.Tables.Add((DataTable)Session["abc"]);
                                    ////DataDynamics.Reports.ReportRuntime _reportRuntime1 = new DataDynamics.Reports.ReportRuntime(_reportDef1);

                                    //DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
                                    //_reportRuntime.LocateDataSource += WRAXBDetails_LocateDataSource;
                                    //// _reportRuntime1.LocateDataSource += WRAXBDetails_LocateDataSource;
                                    //string exportFile = System.IO.Path.GetTempFileName() + ".pdf";
                                    //System.IO.FileInfo myFile = new System.IO.FileInfo(exportFile);
                                    //System.Collections.Specialized.NameValueCollection settings = new System.Collections.Specialized.NameValueCollection();
                                    //settings.Add("hideToolbar", "True");
                                    //settings.Add("hideMenubar", "True");
                                    //settings.Add("hideWindowUI", "True");
                                    //settings.Add("MarginLeft", "0.1in");
                                    //settings.Add("MarginRight", "0.1in");
                                    //settings.Add("MarginTop", "0.1in");
                                    //settings.Add("MarginBottom", "0.1in");
                                    //settings.Add("PageWidth", "9in");
                                    //settings.Add("PageHeight", "7.5in");
                                    //settings.Add("FitWindow", "True");

                                    //PdfRenderingExtension _renderingExtension = new DataDynamics.Reports.Rendering.Pdf.PdfRenderingExtension();
                                    //FileStreamProvider _provider = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile.Directory, Path.GetFileNameWithoutExtension(myFile.Name));
                                    //_reportRuntime.Render(_renderingExtension, _provider, settings);
                                    //Response.Clear();
                                    //Response.ContentType = "application/pdf";
                                    //string filename = Session["UserName"].ToString() + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
                                    //string FullName = filename.Replace(" ", "");
                                    //Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
                                    //Response.BinaryWrite(File.ReadAllBytes(exportFile));
                                    //myFile.Delete();

                                    //ReportViewer ReportViewer1 = new ReportViewer();
                                    //ReportViewer1.Visible = true;



                                    //ReportViewer1.Reset();

                                    //ReportViewer1.ProcessingMode = ProcessingMode.Local;

                                    //LocalReport rep1 = ReportViewer1.LocalReport;
                                    //rep1.ReportPath = Server.MapPath("/Reports/DeliveryOrder.rdlc");
                                    ////rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "EXP_ULDArrival.rdlc";
                                    //rds1.Name = "dsDeliveryOrder_DataTable1";
                                    //rds1.Value = dset.Tables[0];
                                    //rep1.DataSources.Add(rds1);

                                    ReportViewer rptUCRReport = new ReportViewer();
                                    DataTable dtTable1 = new DataTable();
                                    dtTable1 = dset.Tables[0];
                                    rptUCRReport.ProcessingMode = ProcessingMode.Local;

                                    LocalReport rep1 = rptUCRReport.LocalReport;

                                    rep1.ReportPath = Server.MapPath("/Reports/DeliveryOrder.rdlc");

                                    rds1.Name = "dsDeliveryOrder_DataTable1";
                                    rds1.Value = dtTable1;
                                    rep1.DataSources.Add(rds1);




                                    //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                                    //ItemsSubreportProcessingEventHandler(Object, e);
                                    //#region "Print as PDF"

                                    //string reportType = "PDF";

                                    //string mimeType;

                                    //string encoding;

                                    //string fileNameExtension;



                                    ////The DeviceInfo settings should be changed based on the reportType

                                    ////http://msdn2.microsoft.com/en-us/library/ms155397.aspx

                                    //string deviceInfo = "<DeviceInfo><PageHeight>30cm</PageHeight><PageWidth>30cm</PageWidth></DeviceInfo>";

                                    ////"<DeviceInfo>" +

                                    ////"  <OutputFormat>PDF</OutputFormat>" +

                                    //////"  <PageWidth>8.5in</PageWidth>" +

                                    //////"  <PageHeight>11in</PageHeight>" +

                                    //////"  <MarginTop>0.01in</MarginTop>" +

                                    //////"  <MarginLeft>0.01in</MarginLeft>" +

                                    //////"  <MarginRight>0.01in</MarginRight>" +

                                    //////"  <MarginBottom>0.01in</MarginBottom>" +

                                    ////"</DeviceInfo>";



                                    //Warning[] warnings;

                                    //string[] streams;

                                    //byte[] renderedBytes;



                                    ////Render the report
                                    ////try
                                    ////{
                                    //renderedBytes = rep1.Render(

                                    //    reportType,

                                    //    deviceInfo,

                                    //    out mimeType,

                                    //    out encoding,

                                    //    out fileNameExtension,

                                    //    out streams,

                                    //    out warnings);
                                    ////}
                                    ////catch (Exception ex)
                                    ////{ }


                                    ////Clear the response stream and write the bytes to the outputstream

                                    ////Set content-disposition to "attachment" so that user is prompted to take an action

                                    ////on the file (open or save)

                                    //Response.Clear();

                                    //Response.ContentType = mimeType;

                                    //Response.AddHeader("content-disposition", "attachment; filename=" + "Arrival_Manifest" + "." + fileNameExtension);

                                    //Response.BinaryWrite(renderedBytes);

                                    //#endregion

                                    #region Render to PDF
                                    try
                                    {
                                        string reportType = "PDF";
                                        //string mimeType;
                                        //string encoding;
                                        string fileNameExtension;
                                        //string deviceInfo = "<DeviceInfo><PageHeight>20cm</PageHeight><PageWidth>20cm</PageWidth></DeviceInfo>";
                                        //string deviceInfo =
                                        //"<DeviceInfo>" +
                                        //"  <OutputFormat>PDF</OutputFormat>" +
                                        //"  <PageWidth>8.5in</PageWidth>" +
                                        //"  <PageHeight>11in</PageHeight>" +
                                        //"  <MarginTop>0.5in</MarginTop>" +
                                        //"  <MarginLeft>1in</MarginLeft>" +
                                        //"  <MarginRight>1in</MarginRight>" +
                                        //"  <MarginBottom>0.5in</MarginBottom>" +
                                        //"</DeviceInfo>";

                                        string deviceInfo = "<DeviceInfo><PageWidth>13.76in</PageWidth><PageHeight>12.3in</PageHeight></DeviceInfo>";


                                        //"<DeviceInfo>" +

                                        //"  <OutputFormat>PDF</OutputFormat>" +

                                        //"</DeviceInfo>";

                                        Warning[] warnings;
                                        string[] streamIds;
                                        string mimeType = string.Empty;
                                        string encoding = string.Empty;
                                        string extension = string.Empty;

                                        //Render the report
                                        // send it to the client to download
                                        byte[] bytes = rptUCRReport.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                                        Response.Buffer = true;
                                        Response.Clear();
                                        Response.ContentType = mimeType;
                                        string filename = Session["UserName"].ToString() + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
                                        string FullName = filename.Replace(" ", "");
                                        Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
                                        //Response.AddHeader("content-disposition", "attachment; filename=" + "DO" + "." + ".pdf");
                                        Response.BinaryWrite(bytes); // create the file
                                        Response.Flush();


                                        //Response.End();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    #endregion
                                }
                                else
                                {
                                    lblStatus.Text = "No AWB's selected to print DO";
                                    lblStatus.ForeColor = Color.Red;
                                    return;
                                }

                            }
                        }
                    }
                }
                catch
                {
                    lblStatus.Text = "Error while printing DO; Please try again";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #region Event for Loading Dataset into Report
        //private void WRAXBDetails_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")    
        //    if (dname == "DataSet1")
        //    {
        //        e.Data = dset;

        //    }

        //}
        #endregion
        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsDeliveryOrder_DataTable1", dset.Tables[0]));

        }
    }
}

