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

using System.Data.SqlClient;

using System.Configuration;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class listProductivityRpt : System.Web.UI.Page
    {
        #region Variable
        private DataSet Dataset1;
        private DataSet Dataset2;
        StockAllocationBAL objBAL = new StockAllocationBAL();
        ReportBAL objReport = new ReportBAL();
        #endregion

        #region On page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ////RptViewerRevenue_Station.Visible = false;
                    //FillPaymentType();
                    DataSet City = objBAL.GetCityCode();
                    ddlCode.DataSource = City.Tables[0];
                    ddlCode.DataTextField = "CityCode";
                    ddlCode.DataValueField = "CityCode";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "Select");
                    ddlCode.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
            }
      }
        #endregion

        #region On List button Click show Report
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Validate controls
                Validate();


                DataSet oDs1 = new DataSet();
                DataSet oDs2 = new DataSet();
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;

                try
                {
                    string constr = Global.GetConnectionString();
                    string[] ParamNames = new string[] { };
                    SqlDbType[] ParamTypes = new SqlDbType[] { };
                    SQLServer da = new SQLServer(constr);
                    string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "";
                    if (txtAgentCode.Text.Trim() != "")
                        AgentCode = txtAgentCode.Text.Trim();
                    if (ddlPaymentType.SelectedItem.Value.ToString() != "Select")
                        PaymentType = ddlPaymentType.SelectedItem.Value.ToString();
                    if (ddlControlingLocator.SelectedItem.Value.ToString() != "Select")
                        contrLocatorCode = ddlControlingLocator.SelectedItem.Value.ToString();
                    if (ddlType.SelectedItem.Value.ToString() != "Select")
                        level = ddlType.SelectedItem.Value.ToString();
                    if (ddlCode.SelectedItem.Value.ToString() != "Select")
                        levelCode = ddlCode.SelectedItem.Value.ToString();
                    if (txtFromdate.Text.Trim() != "")
                        frmDate = txtFromdate.Text.Trim();
                    if (txtTodate.Text.Trim() != "")
                        ToDt = txtTodate.Text.Trim();
                 
                    string day = txtFromdate.Text.Substring(0, 2);
                    string mon = txtFromdate.Text.Substring(3, 2);
                    string yr = txtFromdate.Text.Substring(6, 4);
                    frmDate = yr + "-" + mon + "-" + day;
                   DateTime  dtfrom = Convert.ToDateTime(frmDate);

                   string dayTo = txtTodate.Text.Substring(0, 2);
                   string monTo = txtTodate.Text.Substring(3, 2);
                   string yrTo = txtTodate.Text.Substring(6, 4);
                   ToDt = yrTo + "-" + monTo + "-" + dayTo;
                   DateTime dtTo = Convert.ToDateTime(ToDt);



                    AgentCode = "All"; PaymentType = "All"; contrLocatorCode = "All"; level = "All"; levelCode = "All";
                    string AWBStatus = "All";

                    DataSet res_Revenue = objReport.GetAWBBookingSummary(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom, dtTo,AWBStatus);


                    if (res_Revenue != null)
                    {
                        if (res_Revenue.Tables[0].Rows.Count > 0)
                        {
                            Dataset1 = res_Revenue;
                            Dataset2 = showSearchCriteria(AgentCode, PaymentType, contrLocatorCode, level, levelCode, frmDate, ToDt);

                            ////RptViewerRevenue_Station.Visible = true;



                            info = new FileInfo(Server.MapPath("/Reports/Report1.rdlx"));
                           // info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");
             
                            ////definition = new ReportDefinition(info);
                            ////runtime = new ReportRuntime(definition);
                            ////runtime.LocateDataSource += WARCustWise_LocateDataSource;
                            ////RptViewerRevenue_Station.SetReport(runtime);
                            //  oDs2 = LoadParamDataset();// just temperory used, not provide real value                 Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
                        }
                        else
                        {
                            //RptViewerRevenue_Station.Visible = false;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data not avialable for given search criteria";
                            txtFromdate.Focus();
                            return;


                        }
                    }
                    else
                    {
                        ////RptViewerRevenue_Station.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not avialable for given search criteria";
                        txtFromdate.Focus();
                        return;

                    }

                }
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            {

            }


        }
        #endregion

        #region To show subreport
        ////private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)  
        //// {         string dname = e.DataSetName;    
        ////     //  if (dname.ToLower() == "DataSetValue")    
        ////     if (dname == "DataSet1")        
        ////     {        
        ////         e.Data = Dataset1;
  
        ////     }       
        ////     else     
        ////     {         
        ////         e.Data = Dataset2; 
        ////    }
        //// }
        #endregion


        #region dropdown list to select list
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
         {
             try
             {

                 if (ddlType.SelectedItem.Value.ToString() == "Select")
                 {
                     ddlCode.DataSource = "";
                     ddlCode.DataBind();
                 }

                 if (ddlType.SelectedItem.Value.ToString() == "Select")
                 {

                     ddlCode.Items.Insert(0, "Select");
                     ddlCode.SelectedIndex = 0;

                 }
                 if (ddlType.SelectedItem.Value.ToString() == "Airport")
                 {
                     DataSet City = objBAL.GetCityCode();
                     ddlCode.DataSource = City.Tables[0];
                     ddlCode.DataTextField = "CityCode";
                     ddlCode.DataValueField = "CityCode";
                     ddlCode.DataBind();
                     ddlCode.Items.Insert(0, "Select");
                     ddlCode.SelectedIndex = 0;

                 }


                 if (ddlType.SelectedItem.Value.ToString() == "Airport")
                 {
                     DataSet City = objBAL.GetCityCode();
                     ddlCode.DataSource = City.Tables[0];
                     ddlCode.DataTextField = "CityCode";
                     ddlCode.DataValueField = "CityCode";
                     ddlCode.DataBind();
                     ddlCode.Items.Insert(0, "Select");
                     ddlCode.SelectedIndex = 0;
                    
                 }


                 if (ddlType.SelectedItem.Value.ToString() == "Region")
                 {
                     DataSet Region = objBAL.GetRegionCode();
                     ddlCode.DataSource = Region.Tables[0];
                     ddlCode.DataTextField = "RegionCode";
                     ddlCode.DataValueField = "RegionCode";
                     ddlCode.DataBind();
                     ddlCode.Items.Insert(0, "Select");
                     ddlCode.SelectedIndex = 0;


                 }
                 if (ddlType.SelectedItem.Value.ToString() == "City")
                 {

                     DataSet City = objBAL.GetCityCode();
                     ddlCode.DataSource = City.Tables[0];
                     ddlCode.DataTextField = "CityCode";
                     ddlCode.DataValueField = "CityCode";
                     ddlCode.DataBind();
                     ddlCode.Items.Insert(0, "Select");
                     ddlCode.SelectedIndex = 0;
                 }
                 if (ddlType.SelectedItem.Value.ToString() == "Country")
                 {

                     DataSet Agent = objBAL.GetCountryCode();
                     ddlCode.DataSource = Agent.Tables[0];
                     ddlCode.DataTextField = "CountryCode";
                     ddlCode.DataValueField = "CountryCode";

                     ddlCode.DataBind();
                     ddlCode.Items.Insert(0, "Select");
                     ddlCode.SelectedIndex = 0;

                 }

             }
             catch (Exception)
             {


             }

         }
        #endregion


         #region Validate Controls
        private void Validate()
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
                             return;
                         }


                     }
                 }
                 catch (Exception ex)
                 {
                     lblStatus.ForeColor = Color.Red;
                     lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                     txtFromdate.Focus();
                     return;
                 }


                 if (ddlType.SelectedItem.Value.ToString() != "Select" && ddlCode.SelectedItem.Value.ToString() == "Select")
                 {
                     lblStatus.ForeColor = Color.Red;
                     lblStatus.Text = "Please select level Type";
                     txtFromdate.Focus();
                     return;
                 }


             }
             catch (Exception ex)
             {


             }

         }
         #endregion

         #region Fill Session For DDL
         private void FillSession()
         {
             try
             {

                 DataSet Region = objBAL.GetRegionCode();
                 Session["Region"] = Region.Tables[0];
                 DataSet City = objBAL.GetCityCode();
                 Session["City"] = City.Tables[0];
                 DataSet Agent = objBAL.GetAgentCode();
                 Session["Agent"] = Agent.Tables[0];

             }
             catch (Exception)
             {


             }
         }
         #endregion

         #region Fill Session For DDL
         private void FillPaymentType()
         {
             try
             {

                 DataSet PaymentCode = objReport.GetPaymentCode();
                 ddlPaymentType.DataSource = PaymentCode.Tables[0];
                 ddlPaymentType.DataTextField = "PayMode";
                 ddlPaymentType.DataValueField = "PayMode";

                 ddlPaymentType.DataBind();
                 ddlPaymentType.Items.Insert(0, "Select");
                 ddlPaymentType.SelectedIndex = 0;

               

             }
             catch (Exception)
             {


             }
         }
         #endregion

         #region SearchCriteria
         private DataSet showSearchCriteria(string Agent,string PaymentType,string ControlLoc,string level,string levelCode,string FromDate,string Todate)
         {
             DataSet ds = new DataSet();
             try
             {
                 DataTable dtSearch = new DataTable();
                 DataColumn dcNew;

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


                 DataRow dr;
                 dr = dtSearch.NewRow();

                 dr["AgentCode"] = Agent; //"5";
                 dr["PaymentType"] = PaymentType;// "5";
                 dr["ControlingLoc"] = ControlLoc;
                 dr["level"] = level;
                 dr["levelCode"] = levelCode;
                 dr["FromDate"] = FromDate;
                 dr["ToDate"] = Todate;// "9";

                 dtSearch.Rows.Add(dr);

                 
                 ds.Tables.Add(dtSearch);
                 return ds;
             }
             catch (Exception ex)
             {
                 return ds;
             }

         }
        #endregion

    }
}