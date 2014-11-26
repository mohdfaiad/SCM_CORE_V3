using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using QID.DataAccess;

using System.IO;
using System.Configuration;
using System.Drawing;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class rptStationwiseReport : System.Web.UI.Page
    {
        #region Variable
        private DataSet Dataset1;
        private DataSet Dataset2;
        StockAllocationBAL objBAL = new StockAllocationBAL();
        ReportBAL objReport = new ReportBAL();
        public static string CurrTime = "";
        UserCreationBAL objUserBAL = new UserCreationBAL();
        EMAILOUT objEmail = new EMAILOUT();
        ReportBAL objBal = new ReportBAL();
        string strCheckedStations;
        #endregion

        #region On page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    RptStnwiseViewer.Visible = false;

                    Session["dsStnWise"] = null;

                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");

              

                    DataSet City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "CityCode";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;
                    FillShipmentType();
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem listitem in chkListShipmentType.Items)
            {
                listitem.Selected = chkSelectAll.Checked;
            }
        }  

        protected string getCheckedStations()
        {
            if (chkSelectAll.Checked == true)
            {
                for (int i = 0; i < chkListShipmentType.Items.Count; i++)
                {
                    if (strCheckedStations == "")
                        strCheckedStations = chkListShipmentType.Items[i].Value;
                    else
                        strCheckedStations = strCheckedStations + "," + chkListShipmentType.Items[i].Value;
                }
            }
            else
            {
                for (int i = 0; i < chkListShipmentType.Items.Count; i++)
                {
                    if (chkListShipmentType.Items[i].Selected == true)
                    {
                        if (strCheckedStations == "")
                            strCheckedStations = chkListShipmentType.Items[i].Value;
                        else
                            strCheckedStations = strCheckedStations + "," + chkListShipmentType.Items[i].Value;
                    }
                }
            }


            return strCheckedStations;
        }
       
        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet oDs1 = new DataSet("Station_oDs1");
            DataSet oDs2 = new DataSet("Station_oDs2");
            DataSet res_Revenue = new DataSet("Station_res_Revenue");
            Session["dsStnWise"] = null;
            try
            {
                //Validate controls
                if (Validate() == false)
                {
                    RptStnwiseViewer.Visible = false;
                    return;
                }

                lblStatus.Text = string.Empty;
                RptStnwiseViewer.Visible = false;

                strCheckedStations = getCheckedStations();
                if (strCheckedStations == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select atleast one Station.";
                    return;
                }

            //    FileInfo info;
             //   ReportRuntime runtime;
            //    ReportDefinition definition;
                bool International = false, Domestic = false, POMail = false, DomInt = false, IntDom = false;
                //Domestic = chkDomestic.Checked;
                //International = chkInternational.Checked;
                //POMail = chkPOMail.Checked;
                Domestic = chkListShipmentType.Items[chkListShipmentType.Items.IndexOf(chkListShipmentType.Items.FindByText("Domestic"))].Selected;
                International = chkListShipmentType.Items[chkListShipmentType.Items.IndexOf(chkListShipmentType.Items.FindByText("International"))].Selected;
                POMail = chkListShipmentType.Items[chkListShipmentType.Items.IndexOf(chkListShipmentType.Items.FindByText("POMail"))].Selected;
                DomInt = chkListShipmentType.Items[chkListShipmentType.Items.IndexOf(chkListShipmentType.Items.FindByText("DOM-INT"))].Selected;
                IntDom = chkListShipmentType.Items[chkListShipmentType.Items.IndexOf(chkListShipmentType.Items.FindByText("INT-DOM"))].Selected;
                try
                {

                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All", AgentReferenceCode = "All";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    if (ddlPaymentType.SelectedItem.Value.ToString() != "All(Without FOC)")
                        PaymentType = ddlPaymentType.SelectedItem.Value.ToString();
                    if (ddlControlingLocator.SelectedItem.Value.ToString() != "All")
                        contrLocatorCode = ddlControlingLocator.SelectedItem.Value.ToString();
                    if (ddlType.SelectedItem.Value.ToString() != "All")
                        level = ddlType.SelectedItem.Value.ToString();
                    if (ddlCode.SelectedItem.Value.ToString() != "All")
                        levelCode = ddlCode.SelectedItem.Value.ToString();
                    if (ddlAWBStatus.SelectedItem.Value.ToString() != "All")
                        AWBStatus = ddlAWBStatus.SelectedItem.Value.ToString();
                    if (txtFromdate.Text.Trim() != "")
                        frmDate = txtFromdate.Text.Trim();

                    DateTime dtTo = new DateTime();
                    DateTime dtfrom = new DateTime();

                    if (txtTodate.Text.Trim() != "" && txtFromdate.Text.Trim() != "")
                    {
                        ToDt = txtTodate.Text.Trim();

                        string day = txtFromdate.Text.Substring(0, 2);
                        string mon = txtFromdate.Text.Substring(3, 2);
                        string yr = txtFromdate.Text.Substring(6, 4);
                        frmDate = yr + "-" + mon + "-" + day;
                        dtfrom = Convert.ToDateTime(frmDate);
                        frmDate = dtfrom.ToString();
                        string dayTo = txtTodate.Text.Substring(0, 2);
                        string monTo = txtTodate.Text.Substring(3, 2);
                        string yrTo = txtTodate.Text.Substring(6, 4);
                        ToDt = yrTo + "-" + monTo + "-" + dayTo;
                        dtTo = Convert.ToDateTime(ToDt);
                        ToDt = dtTo.ToString();
                    }

                    if (txtAgentReferenceCode.Text.Trim() != "")
                        AgentReferenceCode = txtAgentReferenceCode.Text.Trim();

                    res_Revenue = objReport.GetStationWiseAWBSummary(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom, dtTo, AWBStatus, Domestic, International, DomInt, IntDom, POMail, AgentReferenceCode);

                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Session["dsStnWise"] = res_Revenue;
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);

                            #region Old RDLX
                            //info = new FileInfo(Server.MapPath("/Reports/rptStationWiseTonnageReport.rdlx"));
                            //definition = new ReportDefinition(info);
                            //runtime = new ReportRuntime(definition);
                            //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            //RptViewerRevenue_Station.SetReport(runtime);
                            //btnExport.Visible = true;
                            //  oDs2 = LoadParamDataset();// just temperory used, not provide real value Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                            #endregion

                            //rpsdafsdf
                            
                            RptStnwiseViewer.Visible = true;

                            #region RDLC

                            //Logo
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

                            if (res_Revenue.Tables[0].Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                                res_Revenue.Tables[0].Columns.Add(col1);
                            }

                            RptStnwiseViewer.Visible = true;
                            RptStnwiseViewer.ProcessingMode = ProcessingMode.Local;
                            RptStnwiseViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/StationwiseRpt.rdlc");
                            //Customers dsCustomers = GetData("select top 20 * from customers");
                            ReportDataSource datasource = new ReportDataSource("dsStationwise_dtStationwise", res_Revenue.Tables[0]);
                            RptStnwiseViewer.LocalReport.DataSources.Clear();
                            RptStnwiseViewer.LocalReport.DataSources.Add(datasource);
                            RptStnwiseViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                            SaveUserActivityLog("");

                            #endregion
                        }
                        else
                        {
                            RptStnwiseViewer.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not available for given search criteria";
                            Session["dsStnWise"] = null;
                            SaveUserActivityLog(lblStatus.Text);
                            txtFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        RptStnwiseViewer.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        Session["dsStnWise"] = null;
                        SaveUserActivityLog(lblStatus.Text);
                        txtFromdate.Focus();
                        return;

                    }

                }
                catch (Exception ex)
                {
                    SaveUserActivityLog(ex.Message);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (oDs1 != null)
                    oDs1.Dispose();
                if (oDs2 != null)
                    oDs2.Dispose();
                if (res_Revenue != null)
                    res_Revenue.Dispose();
                if (Dataset1 != null)
                    Dataset1.Dispose();
                if (Dataset2 != null)
                    Dataset2.Dispose();
            }
        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsStationwise_dtStationwise_Sub", Dataset2.Tables[0]));
        }

        #region To show subreport
        //private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")    
        //    if (dname == "DataSet1")    
        //    {
        //        e.Data = Dataset1;

        //    }
        //    else
        //    {
        //        e.Data = Dataset2;
        //    }
        //}
        #endregion

        #region dropdown list to select list
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (ddlType.SelectedItem.Value.ToString() == "All")
                {
                    ddlCode.DataSource = "";
                    ddlCode.DataBind();
                }

                if (ddlType.SelectedItem.Value.ToString() == "All")
                {

                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }
                if (ddlType.SelectedItem.Value.ToString() == "Airport")
                {
                    DataSet City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "CityCode";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }

                if (ddlType.SelectedItem.Value.ToString() == "Airport")
                {
                    DataSet City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "CityCode";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }

                if (ddlType.SelectedItem.Value.ToString() == "Region")
                {
                    DataSet Region = objBAL.GetRegionCode();
                    ddlCode.DataSource = Region.Tables[0];
                    ddlCode.DataTextField = "RegionCode";
                    ddlCode.DataValueField = "RegionCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;
                }
                if (ddlType.SelectedItem.Value.ToString() == "City")
                {

                    DataSet City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "CityCode";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;
                }
                if (ddlType.SelectedItem.Value.ToString() == "Country")
                {

                    DataSet Agent = objBAL.GetCountryCode();
                    ddlCode.DataSource = Agent.Tables[0];
                    ddlCode.DataTextField = "CountryCode";
                    ddlCode.DataValueField = "CountryCode";

                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "All");
                    ddlCode.SelectedIndex = 0;

                }

            }
            catch (Exception ex)
            {


            }

        }
        #endregion

        #region Fill CheckBoxList
        protected void FillShipmentType()
        {
            DataSet dsShipType = new DataSet("Station_dsShipType");
            try
            {
                dsShipType = objReport.GetShipmentType();
                chkListShipmentType.DataSource = dsShipType;
                chkListShipmentType.DataMember = dsShipType.Tables[0].TableName;
                chkListShipmentType.DataTextField = "Code";
                //chkListShipmentType.DataValueField = "SrNo";
                chkListShipmentType.DataBind();
                chkListShipmentType.SelectedIndex = -1;
            }
            catch (Exception Ex)
            {
                lblStatus.Text = "Error in ShipmentType Filling.";
            }
            finally
            {
                if (dsShipType != null)
                    dsShipType.Dispose();
            }
        }
        #endregion

        #region Validate Controls
        private bool Validate()
        {
            try
            {
                try
                {
                    if (txtFromdate.Text.Trim() != "" || txtTodate.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtFromdate.Focus();
                            return false;
                        }


                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtFromdate.Focus();
                    return false;
                }


                if (ddlType.SelectedItem.Value.ToString() != "All" && ddlCode.SelectedItem.Value.ToString() == "All")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select level Type";
                    txtFromdate.Focus();
                    return false;
                }
                string strResult = string.Empty;
                try
                {
                    strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFromdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtTodate.Text.Trim(), "dd/MM/yyyy", null));
                }
                catch
                {
                    strResult = "";
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                    txtFromdate.Focus();
                    return false;
                }


            }
            catch (Exception ex)
            {


            }
            return true;
        }
        #endregion

        #region Fill Session For DDL
        private void FillSession()
        {
            DataSet Region=new DataSet("Staion_Region");
                DataSet City=new DataSet("Station_City");
                DataSet Agent = new DataSet("Staton_Agent");
                try
                {

                    Region = objBAL.GetRegionCode();
                    Session["Region"] = Region.Tables[0];
                    City = objBAL.GetCityCode();
                    Session["City"] = City.Tables[0];
                    Agent = objBAL.GetAgentCode();
                    Session["Agent"] = Agent.Tables[0];

                }
                catch (Exception)
                {
                }
                finally
                {
                    if (Region != null)
                        Region.Dispose();
                    if (City != null)
                        City.Dispose();
                    if (Agent != null)
                        Agent.Dispose();
                }
        }
        #endregion

        #region Fill Session For DDL
        private void FillPaymentType()
        {
            DataSet PaymentCode = new DataSet("Station_PaymentCode");
            try
            {
                PaymentCode = objReport.GetPaymentCode();
                ddlPaymentType.DataSource = PaymentCode.Tables[0];
                ddlPaymentType.DataTextField = "PayMode";
                ddlPaymentType.DataValueField = "PayMode";

                ddlPaymentType.DataBind();
                ddlPaymentType.Items.Insert(0, "All");
                ddlPaymentType.SelectedIndex = 0;
            }
            catch (Exception)
            { }
            finally
            {
                if (PaymentCode != null)
                    PaymentCode.Dispose();
            }
        }
        #endregion

        #region SearchCriteria
        private DataSet showSearchCriteria(string Agent, string PaymentType, string ControlLoc, string level, string levelCode, string FromDate, string Todate, string AWBStatus)
        {
            DataSet ds = new DataSet("Staion_ds");
            DataTable dtSearch = new DataTable("Station_dtSearch");
            DataColumn dcNew = new DataColumn();
            DataRow dr;
            try
            {
                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "AgentCode";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "PaymentType";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ControlingLoc";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "level";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "levelCode";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "FromDate";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ToDate";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "AWBStatus";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ReportDate";
                dtSearch.Columns.Add(dcNew);

                dr = dtSearch.NewRow();

                dr["AgentCode"] = Agent; //"5";
                dr["PaymentType"] = PaymentType;// "5";
                dr["ControlingLoc"] = ControlLoc;
                dr["level"] = level;
                dr["levelCode"] = levelCode;
                dr["FromDate"] = FromDate;
                dr["ToDate"] = Todate;// "9";
                dr["AWBStatus"] = AWBStatus;
                dr["ReportDate"] = CurrTime;
                dtSearch.Rows.Add(dr);

                ds.Tables.Add(dtSearch);
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                if (dtSearch != null)
                    dtSearch.Dispose();
                if (dcNew != null)
                    dcNew.Dispose();
            }
        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = new DataSet("Station_dsExp"); ;
            DataTable dt = null;
            Session["dsStnWise"] = null;
            lblStatus.Text = string.Empty;
            try
            {
               // if ((DataSet)Session["dsStnWise"] == null)
               
                    GetData();
              

                dsExp = (DataSet)Session["dsStnWise"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                    dt = (DataTable)dsExp.Tables[0];

                else
                {
                    lblStatus.Text = "Records does not exist for selected criteria";
                    lblStatus.ForeColor = Color.Red;
                    RptStnwiseViewer.Visible = false;
                    SaveUserActivityLog(lblStatus.Text);
                    return;
                }
                if (dt.Columns.Contains("Logo"))
                { dt.Columns.Remove("Logo"); }
                if(dt.Columns.Contains("CNetRevenue"))
                {dt.Columns.Remove("CNetRevenue");
                }
                if (dt.Columns.Contains("CNetYield"))
                {
                    dt.Columns.Remove("CNetYield");
                }
                if (dt.Columns.Contains("FromDate"))
                {

                    dt.Columns.Remove("FromDate");

                   }
                if (dt.Columns.Contains("ToDate"))
                {
                    dt.Columns.Remove("ToDate");
                }
                if (dt.Columns.Contains("GLCode"))
                {
                    dt.Columns.Remove("GLCode");
                
                }
                if (dt.Columns.Contains("NetRevenue"))
                {
                    dt.Columns.Remove("NetRevenue");
                
                }
                if (dt.Columns.Contains("NetYield"))
                {
                    dt.Columns.Remove("NetYield");

                }

                string attachment = "attachment; filename=StationwiseTonnage Report.xls";
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
                if (dsExp != null)
                    dsExp = null;
                if (dt != null)
                    dt = null;
            }
        }

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            try
            {
                // taking all parameters as user selected in report in one variable - "Param"
                string Param = "AgtCode:" + txtAgentCode.Text.ToString() + ", PayMode:" + ddlPaymentType.Text.ToString() + ",CntlName:" + ddlControlingLocator.Text.ToString() + ", Level:" + ddlType.Text.ToString() + ", Loc:" + ddlCode.Text.ToString() + ",FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" + txtTodate.Text.ToString();
                objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Station wise Tonnage", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
            }
            catch (Exception ex)
            { }
            finally
            {
                if (objBAL != null)
                    objBAL = null;
            }
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
                if(objBL!=null)
                objBL = null;
            }

            if (DaysConfigured > 0 && intReportInterval > DaysConfigured)
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            else
                return "";
        }

        public void GetData()
        {
            //DataSet oDs1 = new DataSet();
            //DataSet oDs2 = new DataSet();
            DataSet res_Revenue1 = new DataSet("Station_res_Revenue1");
            Session["dsStnWise"] = null;
            try
            {
                lblStatus.Text = string.Empty;
                RptStnwiseViewer.Visible = false;
                //Validate controls
                if (Validate() == false)
                {
                    Session["dsStnWise"] = null;
                    RptStnwiseViewer.Visible = false;
                    return;
                }
                strCheckedStations = getCheckedStations();
                if (strCheckedStations == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select atleast one Station.";
                    return;
                }

                 bool International = false, Domestic = false, POMail = false, DomInt = false, IntDom = false;
                //Domestic = chkDomestic.Checked;
                //International = chkInternational.Checked;
                //POMail = chkPOMail.Checked;
                Domestic = chkListShipmentType.Items[chkListShipmentType.Items.IndexOf(chkListShipmentType.Items.FindByText("Domestic"))].Selected;
                International = chkListShipmentType.Items[chkListShipmentType.Items.IndexOf(chkListShipmentType.Items.FindByText("International"))].Selected;
                POMail = chkListShipmentType.Items[chkListShipmentType.Items.IndexOf(chkListShipmentType.Items.FindByText("POMail"))].Selected;
                DomInt = chkListShipmentType.Items[chkListShipmentType.Items.IndexOf(chkListShipmentType.Items.FindByText("DOM-INT"))].Selected;
                IntDom = chkListShipmentType.Items[chkListShipmentType.Items.IndexOf(chkListShipmentType.Items.FindByText("INT-DOM"))].Selected;
                try
                {

                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All", AgentReferenceCode = "All";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    if (ddlPaymentType.SelectedItem.Value.ToString() != "All(Without FOC)")
                        PaymentType = ddlPaymentType.SelectedItem.Value.ToString();
                    if (ddlControlingLocator.SelectedItem.Value.ToString() != "All")
                        contrLocatorCode = ddlControlingLocator.SelectedItem.Value.ToString();
                    if (ddlType.SelectedItem.Value.ToString() != "All")
                        level = ddlType.SelectedItem.Value.ToString();
                    if (ddlCode.SelectedItem.Value.ToString() != "All")
                        levelCode = ddlCode.SelectedItem.Value.ToString();
                    if (ddlAWBStatus.SelectedItem.Value.ToString() != "All")
                        AWBStatus = ddlAWBStatus.SelectedItem.Value.ToString();
                    if (txtFromdate.Text.Trim() != "")
                        frmDate = txtFromdate.Text.Trim();
                    DateTime dtTo = new DateTime(); DateTime dtfrom = new DateTime();
                    if (txtTodate.Text.Trim() != "" && txtFromdate.Text.Trim() != "")
                    {
                        ToDt = txtTodate.Text.Trim();

                        string day = txtFromdate.Text.Substring(0, 2);
                        string mon = txtFromdate.Text.Substring(3, 2);
                        string yr = txtFromdate.Text.Substring(6, 4);
                        frmDate = yr + "-" + mon + "-" + day;
                        dtfrom = Convert.ToDateTime(frmDate);
                        frmDate = dtfrom.ToString();
                        string dayTo = txtTodate.Text.Substring(0, 2);
                        string monTo = txtTodate.Text.Substring(3, 2);
                        string yrTo = txtTodate.Text.Substring(6, 4);
                        ToDt = yrTo + "-" + monTo + "-" + dayTo;
                        dtTo = Convert.ToDateTime(ToDt);
                        ToDt = dtTo.ToString();
                    }

                    if (txtAgentReferenceCode.Text.Trim() != "")
                        AgentReferenceCode = txtAgentReferenceCode.Text.Trim();

                    res_Revenue1 = objReport.GetStationWiseAWBSummary(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom, dtTo, AWBStatus, Domestic, International, DomInt, IntDom, POMail, AgentReferenceCode);

                    if (res_Revenue1 != null)
                    {
                        if (res_Revenue1.Tables[0].Rows.Count > 0)
                        {
                            Session["dsStnWise"] = res_Revenue1;
                            Dataset1 = res_Revenue1;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom.ToString("dd/MM/yyyy"), dtTo.ToString("dd/MM/yyyy"), AWBStatus);
                            SaveUserActivityLog("");

                        }
                        else
                        {
                            RptStnwiseViewer.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not available for given search criteria";
                            Session["dsStnWise"] = null;
                            SaveUserActivityLog(lblStatus.Text);
                            txtFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        RptStnwiseViewer.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        Session["dsStnWise"] = null;
                        SaveUserActivityLog(lblStatus.Text);
                        txtFromdate.Focus();
                        return;

                    }

                }
                catch (Exception ex)
                {
                    SaveUserActivityLog(ex.Message);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //if (oDs1 != null)
                //    oDs1.Dispose();
                //if (oDs2 != null)
                //    oDs2.Dispose();
                if (res_Revenue1 != null)
                    res_Revenue1.Dispose();
            }
        }

        protected void chkSelectAll_CheckedChanged1(object sender, EventArgs e)
        {

        }

        protected void btnCLear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptStationwiseReport.aspx", false);
            RptStnwiseViewer.Visible = false;
        }
    }
}
