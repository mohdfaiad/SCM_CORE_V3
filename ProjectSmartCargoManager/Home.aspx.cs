using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BAL;
using QID.DataAccess;
using ProjectSmartCargoManager.CustomControls;

namespace ProjectSmartCargoManager
{
    public partial class Home : System.Web.UI.Page
    {

        HomeBL objBLL = new HomeBL();
        BALNotifications objNoti = new BALNotifications();
        int roleid;
        SQLServer da = new SQLServer(Global.GetConnectionString());
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                validateUser();
            }
            catch (Exception ex)
            { }
        }
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Session["PwdExp"] != null)
                    {
                        DateTime dtPwdUpdate = Convert.ToDateTime(Session["PwdExp"].ToString());
                        DateTime dtToday = Convert.ToDateTime(Session["IT"].ToString());
                        if (dtToday >= dtPwdUpdate)
                        {
                            Response.Redirect("ChangePassword.aspx?Exp=true");
                        }
                    }
                }
                catch (Exception ex)
                { }
                getNotifications();
            }
            //if (!IsPostBack)
            //{
            //    try
            //    {
            //        //txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //        //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //        //LoadDestination();
            //        //bool flag = validateUser();
            //        //if (flag == false)
            //        //{
            //        //    //string userLoc = "";
            //        //    //userLoc = Session["Station"].ToString();
            //        //    //ddlLocation.Text = userLoc;
            //        //    //ddlLocation.Enabled = false;
            //        //    //pnlFlightDashboard.Visible = false;
            //        //    PanelGraph.Visible = false;
            //        //    Timer1.Enabled = false;
            //        //}
            //        //else
            //        //{
            //        //    GetData_Click(sender, e);

            //        //}
            //        ////DateTime FromDate = DateTime.Now, ToDate = DateTime.Now;
            //        //DateTime fromDate;
            //        //DateTime ToDate;
            //        ////txtFrmDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            //        ////txtToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            //        //fromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
            //        //ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
            //        ////import and Export 
            //        //string Location = Session["Station"].ToString();
            //        ////string Source = ddlLocation.SelectedItem.Text.Trim() == "All" ? "" : ddlLocation.SelectedItem.Text.Trim();
            //        ////string Source = ddlLocation.SelectedItem.Text.Trim();
            //        //#region Parameter
            //        //string[] PName = new string[3];
            //        //PName[0] = "Source";
            //        //PName[1] = "frmDate";
            //        //PName[2] = "ToDate";

            //        //object[] PValue = new object[3];
            //        //PValue[0] = Location;
            //        //PValue[1] = fromDate;//txtFrmDate.Text;
            //        //PValue[2] = ToDate;//txtToDate.Text;

            //        //SqlDbType[] PType = new SqlDbType[3];
            //        //PType[0] = SqlDbType.VarChar;
            //        //PType[1] = SqlDbType.Date;
            //        //PType[2] = SqlDbType.Date;

            //        //#endregion
            //        //DataSet dsimpexp = da.SelectRecords("SP_GetImport", PName, PValue, PType);
            //        //try
            //        //{
            //        //    if (dsimpexp != null)
            //        //    {
            //        //        if (dsimpexp.Tables != null)
            //        //        {
            //        //            if (dsimpexp.Tables.Count > 0)
            //        //            {
            //        //                if (dsimpexp.Tables[0].Rows.Count > 0)
            //        //                {
            //        //                    grdImportList.PageIndex = 0;
            //        //                    grdImportList.DataSource = dsimpexp;
            //        //                    grdImportList.DataMember = dsimpexp.Tables[0].TableName;
            //        //                    grdImportList.DataBind();
            //        //                    grdImportList.Visible = true;
            //        //                    //dsFLAB.Clear();

            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        }
            //        catch (Exception ex)
            //        { }
            //        //DateTime fromDate;
            //        //DateTime ToDate;
            //        //txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //        //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //        //fromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
            //        //ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
            //        //import and Export 
            //        //string Location = Session["Station"].ToString();
            //        //string Source = ddlLocation.SelectedItem.Text.Trim() == "All" ? "" : ddlLocation.SelectedItem.Text.Trim();
            //        //string Source = ddlLocation.SelectedItem.Text.Trim();
            //        #region Parameter
            //        string[] PName1 = new string[3];
            //        PName1[0] = "Source";
            //        PName1[1] = "frmDate";
            //        PName1[2] = "ToDate";

            //        object[] PValue1 = new object[3];
            //        PValue1[0] = Location;
            //        PValue1[1] = fromDate;//txtFrmDate.Text;
            //        PValue1[2] = ToDate;//txtToDate.Text;

            //        SqlDbType[] PType1 = new SqlDbType[3];
            //        PType1[0] = SqlDbType.VarChar;
            //        PType1[1] = SqlDbType.Date;
            //        PType1[2] = SqlDbType.Date;

            //        #endregion
            //        DataSet dsimp = da.SelectRecords("SP_GetExport", PName1, PValue1, PType1);
            //        try
            //        {
            //            if (dsimp != null)
            //            {
            //                if (dsimp.Tables != null)
            //                {
            //                    if (dsimp.Tables.Count > 0)
            //                    {
            //                        if (dsimp.Tables[0].Rows.Count > 0)
            //                        {
            //                            grdexportList.PageIndex = 0;
            //                            grdexportList.DataSource = dsimp;
            //                            grdexportList.DataMember = dsimp.Tables[0].TableName;
            //                            grdexportList.DataBind();
            //                            grdexportList.Visible = true;
            //                            //dsFLAB.Clear();

            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        { }
            //        //end import export
            //        //txtFrmDtAg.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //        //txtToDtAg.Text = DateTime.Now.ToString("dd/MM/yyyy");

            //        //Timer1.Enabled = true;
            //    }
            //    catch (Exception ex)
            //    { }
        }
        #endregion Load

        #region validateUser
        private bool validateUser()
        {
            bool flag = false;
            try
            {
                string RoleType = Session["RoleType"].ToString();
                if (RoleType == "AGT")
                {
                    CustomAgentTab agentTab = LoadControl("~/CustomControls/CustomAgentTab.ascx") as CustomAgentTab;
                    UserControlPlaceHolder.Controls.Add(agentTab);
                }
                else
                    if (RoleType == "OPS")
                    {
                        CustomOperationsTab opsTab = LoadControl("~/CustomControls/CustomOperationsTab.ascx") as CustomOperationsTab;
                        UserControlPlaceHolder.Controls.Add(opsTab);
                    }
                    else
                        if (RoleType == "MGT")
                        {
                            string strClientFlag = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ManagementTabClientSpecific");
                            if ((strClientFlag != string.Empty ? Convert.ToBoolean(strClientFlag) : false))
                            {
                                CustomManagementTab_CEBU managementTab = LoadControl("~/CustomControls/CustomManagementTab_CEBU.ascx") as CustomManagementTab_CEBU;
                                UserControlPlaceHolder.Controls.Add(managementTab);
                            }
                            else
                            {
                                CustomManagementTab managementTab = LoadControl("/CustomControls/CustomManagementTab.ascx") as CustomManagementTab;
                                UserControlPlaceHolder.Controls.Add(managementTab);
                            }
                            
                            
                            
                        }
                        else
                            if (RoleType == "IT")
                            {
                                CustomITAdminTab ITTab = LoadControl("~/CustomControls/CustomITAdminTab.ascx") as CustomITAdminTab;
                                UserControlPlaceHolder.Controls.Add(ITTab);
                            }
                            else
                                if (RoleType == "PLN")
                                {
                                    CustomFltPlannerTab plnTab = LoadControl("~/CustomControls/CustomFltPlannerTab.ascx") as CustomFltPlannerTab;
                                    UserControlPlaceHolder.Controls.Add(plnTab);
                                }
                                else
                                    if (RoleType == "ACC")
                                    {
                                        CustomAccountsTab accTab = LoadControl("~/CustomControls/CustomAccountsTab.ascx") as CustomAccountsTab;
                                        UserControlPlaceHolder.Controls.Add(accTab);
                                    }
                                        else
                                        if (RoleType == "None")
                                        { }
                                        else
                                        {
                                            TabPanel tabGeneral = LoadControl("~/CustomControls/TabPanel.ascx") as TabPanel;
                                            UserControlPlaceHolder.Controls.Add(tabGeneral);
                                        }
            }
            catch (Exception ex)
            { }
            return flag;
        }
        #endregion

        #region old code - Display Notifications
        //private void getNotifications()
        //{
        //    DataSet dsMsg = new DataSet();
        //    try
        //    {
        //        dsMsg = objNoti.GetNotificationList(System.DateTime.Now.ToString(), System.DateTime.Now.ToString(), "", Session["UserName"].ToString());
        //        if (dsMsg != null)
        //        {
        //            if (dsMsg.Tables[0].Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dsMsg.Tables[0].Rows.Count; i++)
        //                {
        //                    grdNotifications.DataSource = dsMsg;
        //                    grdNotifications.DataBind();
        //                }
        //                //LblCritical.Text = dsMsg.Tables[0].Rows[0]["NotificationMsg"].ToString() + "\n";
        //                ////LblHigh.Text = dsMsg.Tables[0].Rows[1]["NotificationMsg"].ToString();
        //                ////LblMaintenance.Text = dsMsg.Tables[0].Rows[2]["NotificationMsg"].ToString();
        //                ////LblInfo.Text = dsMsg.Tables[0].Rows[3]["NotificationMsg"].ToString();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        #endregion

        #region Display Notifications
        private void getNotifications()
        {
            DataSet dsMsg = new DataSet();
            try
            {
                dsMsg = objNoti.GetNotificationListForHome(Convert.ToDateTime(Session["IT"]).Date, Session["UserName"].ToString(), Session["Station"].ToString(),Session["RoleName"].ToString());
                
                    if (dsMsg!=null && dsMsg.Tables.Count>0 && dsMsg.Tables[0].Rows.Count > 0)
                    {
                        //foreach (DataRow dr in dsMsg.Tables[0].Rows)
                        //{
                        //    if (dr[0].ToString() == "Critical")
                        //    {
                        //        LblCritical.Text += dr[4].ToString() + "<br/>";
                        //    }

                        //    if (dr[0].ToString() == "High")
                        //    {
                        //        lblHigh.Text += dr[4].ToString() + "<br/>";
                        //    }

                        //    if (dr[0].ToString() == "Information")
                        //    {
                        //        lblInformation.Text += dr[4].ToString() + "<br/>";
                        //    }

                        //    if (dr[0].ToString() == "Maintenance")
                        //    {
                        //        lblMaintenance.Text += dr[4].ToString() + "<br/>";
                        //    }

                        //    // Label1.Text = dsMsg.Tables[0].Rows[0]["Importance"].ToString() + "\n";
                        //    //Label2.Text = dsMsg.Tables[0].Rows[1]["NotificationMsg"].ToString();
                        //    //Label3.Text = dsMsg.Tables[0].Rows[2]["NotificationMsg"].ToString();
                        //    //Label4.Text = dsMsg.Tables[0].Rows[3]["NotificationMsg"].ToString();

                        //}

                        grdNotifications.DataSource = dsMsg;
                        grdNotifications.DataBind();
                        
                    }

            }

            catch (Exception ex)
            {

            }
        }

        #endregion

        
    }


}

