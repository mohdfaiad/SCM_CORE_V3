using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager.CustomControls
{
    public partial class CustomITAdminTab : System.Web.UI.UserControl
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                LoadDestination();
                LoadTabs();
                lblStatus.Text = string.Empty;
                
            }

        }

        #region OnActiveTab_Changed
        protected void OnActiveTab_Changed(object sender, EventArgs e)
        {
            try
            {
                GetData_Click(null, null);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>CallPopulateClick();</script>", false);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        //# region grdImportList_PageIndexChanging
        //protected void grdImportList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    //DateTime fromDate;
        //    //DateTime ToDate;
        //    DateTime fromDate = DateTime.Now, ToDate = DateTime.Now;
        //    //txtFrmDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
        //    //txtToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
        //    fromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
        //    ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
        //    string Location;
        //    if (ddlLocation.SelectedIndex > 0)
        //    {
        //        Location = ddlLocation.SelectedItem.Text.Trim();
        //    }
        //    else
        //        //import and Export 
        //        Location = Session["Station"].ToString();
        //    //string Source = ddlLocation.SelectedItem.Text.Trim() == "All" ? "" : ddlLocation.SelectedItem.Text.Trim();
        //    //string Source = ddlLocation.SelectedItem.Text.Trim();
        //    #region Parameter
        //    string[] PName = new string[3];
        //    PName[0] = "Source";
        //    PName[1] = "frmDate";
        //    PName[2] = "ToDate";

        //    object[] PValue = new object[3];
        //    PValue[0] = Location;
        //    PValue[1] = fromDate;//txtFrmDate.Text;
        //    PValue[2] = ToDate;//txtToDate.Text;

        //    SqlDbType[] PType = new SqlDbType[3];
        //    PType[0] = SqlDbType.VarChar;
        //    PType[1] = SqlDbType.Date;
        //    PType[2] = SqlDbType.Date;

        //    #endregion
        //    DataSet dsimpexp = da.SelectRecords("SP_GetImport", PName, PValue, PType);


        //    try
        //    {
        //        if (dsimpexp != null)
        //        {
        //            if (dsimpexp.Tables != null)
        //            {
        //                if (dsimpexp.Tables.Count > 0)
        //                {
        //                    if (dsimpexp.Tables[0].Rows.Count > 0)
        //                    {
        //                        grdImportList.PageIndex = 0;
        //                        grdImportList.DataSource = dsimpexp;
        //                        grdImportList.DataMember = dsimpexp.Tables[0].TableName;
        //                        grdImportList.DataBind();
        //                        grdImportList.Visible = true;
        //                        //dsFLAB.Clear();

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //    grdImportList.PageIndex = e.NewPageIndex;
        //    grdImportList.DataSource = dsimpexp.Copy();
        //    grdImportList.DataBind();



        //}
        //# endregion grvNFLABList_PageIndexChanging

        //# region grdexportList_PageIndexChanging
        //protected void grdexportList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    //DateTime fromDate;
        //    //DateTime ToDate;
        //    DateTime fromDate = DateTime.Now, ToDate = DateTime.Now;
        //    //txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        //    //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        //    fromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
        //    ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
        //    string Location;
        //    if (ddlLocation.SelectedIndex > 0)
        //    {
        //        Location = ddlLocation.SelectedItem.Text.Trim();
        //    }
        //    else
        //        //import and Export 
        //        Location = Session["Station"].ToString();
        //    //string Source = ddlLocation.SelectedItem.Text.Trim() == "All" ? "" : ddlLocation.SelectedItem.Text.Trim();
        //    //string Source = ddlLocation.SelectedItem.Text.Trim();
        //    #region Parameter
        //    string[] PName = new string[3];
        //    PName[0] = "Source";
        //    PName[1] = "frmDate";
        //    PName[2] = "ToDate";

        //    object[] PValue = new object[3];
        //    PValue[0] = Location;
        //    PValue[1] = fromDate;//txtFrmDate.Text;
        //    PValue[2] = ToDate;//txtToDate.Text;

        //    SqlDbType[] PType = new SqlDbType[3];
        //    PType[0] = SqlDbType.VarChar;
        //    PType[1] = SqlDbType.Date;
        //    PType[2] = SqlDbType.Date;

        //    #endregion
        //    DataSet dsimpexp = da.SelectRecords("SP_GetExport", PName, PValue, PType);


        //    try
        //    {
        //        if (dsimpexp != null)
        //        {
        //            if (dsimpexp.Tables != null)
        //            {
        //                if (dsimpexp.Tables.Count > 0)
        //                {
        //                    if (dsimpexp.Tables[0].Rows.Count > 0)
        //                    {
        //                        grdexportList.PageIndex = 0;
        //                        grdexportList.DataSource = dsimpexp;
        //                        grdexportList.DataMember = dsimpexp.Tables[0].TableName;
        //                        grdexportList.DataBind();
        //                        grdexportList.Visible = true;
        //                        //dsFLAB.Clear();

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //    grdexportList.PageIndex = e.NewPageIndex;
        //    grdexportList.DataSource = dsimpexp.Copy();
        //    grdexportList.DataBind();



        //}
        //# endregion grvNFLABList_PageIndexChanging

        #region validateUser
        private bool validateUser()
        {
            bool flag = false;
            lblStatus.Text = string.Empty;
            try
            {
                HomeBL objHome = new HomeBL();
                int RoleId = Convert.ToInt32(Session["RoleID"]);
                DataSet objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
                objHome = null;

                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < objDS.Tables[0].Rows.Count; j++)
                    {
                        if (objDS.Tables[0].Rows[j]["ControlId"].ToString() == "FlightDashboard")
                        {
                            //string userLoc = "";
                            //userLoc = Session["Station"].ToString();
                            //ddlLocation.Text = userLoc;
                            //ddlLocation.Enabled = false;
                            flag = true;
                            break;
                        }
                    }
                }
                objDS = null;

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            
            }
            return flag;
        }
        #endregion

        #region Load Location Dropdown
        /// <summary>
        /// Lodas location list in Origin and Destination dropdowns.
        /// </summary>
        public void LoadDestination()
        {
            try
            {
                BookingBAL objBLL = new BookingBAL();
                DataSet ds = objBLL.GetDestinationsForSource("");
                if (ds != null)
                {
                    ddlLocation.Items.Clear();
                    DataRow row = ds.Tables[0].NewRow();

                    ddlLocation.Items.Add("All");
                    ddlLocation.DataSource = ds;
                    ddlLocation.DataMember = ds.Tables[0].TableName;
                    ddlLocation.DataTextField = "AirportCode";
                    ddlLocation.DataValueField = "AirportCode";
                    ddlLocation.DataBind();

                    ddlLocation.SelectedIndex = 0;


                    // ----------- For ddlLocationAg------------

                    //ddlLocationAg.Items.Add("All");
                    //ddlLocationAg.DataSource = ds;
                    //ddlLocationAg.DataMember = ds.Tables[0].TableName;
                    //ddlLocationAg.DataTextField = "AirportCode";
                    //ddlLocationAg.DataValueField = "AirportCode";
                    //ddlLocationAg.DataBind();

                    //ddlLocationAg.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Load Location Dropdown

        #region btnGetDataFlights_Click
        protected void GetData_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTabs();
            }

           catch (Exception ex)
           {
               lblStatus.Text = "Error:" + ex.Message;
               lblStatus.ForeColor = Color.Red;
           }
           
        }
        #endregion

        #region Agent/External User Gridview
        public void LoadExternalUserGrid()
        {
            BALDashboard objDash = new BALDashboard();
            DataSet dsExt = new DataSet();
            lblStatus.Text = string.Empty;

            try
            {
                string Location = "";

                if (ddlLocation.SelectedIndex > 0)
                {
                    Location = ddlLocation.SelectedItem.Text.Trim();
                }
                else
                {
                    Location = "";
                }

                dsExt = objDash.GetExternalUsers(Location, "E");

                if (dsExt != null)
                {
                    if (dsExt.Tables[0].Rows.Count > 0)
                    {
                        grdExternal.DataSource = dsExt.Tables[0];
                        grdExternal.DataBind();
                    }
                }

                else
                {
                    lblStatus.Text = "No Record Found";
                    lblStatus.ForeColor = Color.Red;
                    grdExternal.DataSource = null;
                    grdExternal.DataBind();
                }
                
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Internal User Gridview
        public void LoadInternalUserGrid()
        {
            BALDashboard objDash = new BALDashboard();
            DataSet dsInt = new DataSet();
            lblStatus.Text = string.Empty;

            try
            {
                string Location = "";

                if (ddlLocation.SelectedIndex > 0)
                {
                    Location = ddlLocation.SelectedItem.Text.Trim();
                }
                else
                {
                    Location = "";
                }

                //dsInt = objDash.GetInternalUsers("", "I");

                dsInt = objDash.GetInternalUsers(Location, "I");

                if (dsInt != null)
                {
                    if (dsInt.Tables[0].Rows.Count > 0)
                    {
                        grdInternal.DataSource = dsInt.Tables[0];
                        grdInternal.DataBind();
                        Session["Internal"] = dsInt;
                    }
                }

                else
                {
                    lblStatus.Text = "No Record Found";
                    lblStatus.ForeColor = Color.Red;
                    grdInternal.DataSource = null;
                    grdInternal.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Report Audit Log
        public void LoadReportAuditLogGrid()
        {
            BALDashboard objDash = new BALDashboard();
            DataSet dsRpt = new DataSet();
            lblStatus.Text = string.Empty;

            try
            {
                string fromdt = txtFrmDate.Text;
                string Todt = txtToDate.Text;
                

                //dsRpt = objDash.GetReportAuditLog(fromdt,Todt, "ALL", "ALL");
                dsRpt = objDash.GetReportAuditLog(fromdt, Todt, Session["UserName"].ToString(), "ALL");

                if (dsRpt != null)
                {
                    if (dsRpt.Tables[0].Rows.Count > 0)
                    {
                        grdRptAuditLog.DataSource = dsRpt.Tables[0];
                        grdRptAuditLog.DataBind();
                        Session["dsITADminRptLog"] = dsRpt;
                    }
                }

                else
                {
                    lblStatus.Text = "No Record Found";
                    lblStatus.ForeColor = Color.Red;
                    grdRptAuditLog.DataSource = null;
                    grdRptAuditLog.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }


        #endregion

        #region grdInternal_PageIndexChanging
        protected void grdInternal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BALDashboard objDash = new BALDashboard();
            lblStatus.Text = string.Empty;
            
            try
            {
                DataSet dsint = (DataSet)Session["Internal"];
                
                grdInternal.PageIndex = e.NewPageIndex;
                grdInternal.DataSource = dsint.Tables[0];
                grdInternal.DataBind();
            }
            catch (Exception ex)
            {
               
            }
        }

        #endregion

        #region grdRptAuditLog_PageIndexChanging
        protected void grdRptAuditLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            lblStatus.Text = string.Empty;

            DataSet dsrpt = (DataSet)Session["dsITADminRptLog"];
            try
            {
                grdRptAuditLog.PageIndex = e.NewPageIndex;
                grdRptAuditLog.DataSource = dsrpt.Tables[0];
                grdRptAuditLog.DataBind();

            }
            catch (Exception ex)
            {

            }
        
        }

        #endregion

        public void LoadTabs()
        {
            try
            {
                if (TabContainer1.ActiveTabIndex == 0)
                {
                    LoadInternalUserGrid();
                    
                }
                if (TabContainer1.ActiveTabIndex == 1)
                {
                    LoadExternalUserGrid();

                }
                if (TabContainer1.ActiveTabIndex == 2)
                {
                    LoadReportAuditLogGrid();
                  
                }

            }
            catch (Exception ex)
            {
               
            }
        }
    }
}