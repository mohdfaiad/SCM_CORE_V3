using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using QID.DataAccess;
using BAL;
using System.Drawing;
using System.Data.SqlClient;

namespace ProjectSmartCargoManager
{
    public partial class FrmNotifications : System.Web.UI.Page
    {

        #region Variables
        BALNotifications objNot = new BALNotifications();
        #endregion

        #region Page Laod
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Page.Title = "Notifications";
                Session["Data"] = null;
                FillUserDropdown();
                CreateEmptyRow();
                FillStationDropdown();
                FillRoleDropdown();
                FillGridUserDropdown();
            }
        }

        #endregion
       
        #region Fill Updated By Dropdown
        private void FillUserDropdown()
        {
            DataSet dsUsers = new DataSet();
            try
            {
                dsUsers = objNot.GetUserList();

                if (dsUsers != null || dsUsers.Tables.Count > 0 || dsUsers.Tables[0].Rows.Count > 0)
                {
                    drpUpdatedBy.Items.Clear();
                    drpUpdatedBy.DataSource = dsUsers.Tables[0];
                    drpUpdatedBy.DataTextField = "UserName";
                    drpUpdatedBy.DataValueField = "UserName";
                    drpUpdatedBy.DataBind();
                    drpUpdatedBy.Items.Insert(0, "ALL");
                    //drpUpdatedBy.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                dsUsers = null;
            }
            finally
            {
                if (dsUsers != null)
                    dsUsers.Dispose();
            }
        }
        #endregion

        #region Fill Role Dropdown
        private void FillRoleDropdown()
        {
            DataSet dsRole = new DataSet();
            try
            {
                dsRole = objNot.GetRoleList();

                DrpUserRole.Items.Clear();
                DrpUserRole.DataSource = dsRole.Tables[0];
                DrpUserRole.DataTextField = "RoleName";
                DrpUserRole.DataValueField = "RoleName";
                DrpUserRole.DataBind();
                DrpUserRole.Items.Insert(0,"ALL");

                if (dsRole != null || dsRole.Tables.Count > 0 || dsRole.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < grdvNotification.Rows.Count; i++)
                    {
                        DropDownList ddlRole = (DropDownList)grdvNotification.Rows[i].FindControl("ddlRole");
                        ddlRole.Items.Clear();
                        ddlRole.DataSource = dsRole.Tables[0];
                        ddlRole.DataTextField = "RoleName";
                        ddlRole.DataValueField = "RoleName";
                        ddlRole.DataBind();
                        ddlRole.Items.Insert(0, "ALL");
                        //ddlRole.SelectedIndex = 0;
                    }
                    

                }
            }
            catch (Exception ex)
            {
                dsRole = null;
            }
            finally
            {
                if (dsRole != null)
                    dsRole.Dispose();
            }
        }
        #endregion

        #region Fill Grid User Dropdown
        private void FillGridUserDropdown()
        {
            DataSet dsUsers = new DataSet();
            try
            {
                dsUsers = objNot.GetUserList();

                if (dsUsers != null || dsUsers.Tables.Count > 0 || dsUsers.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < grdvNotification.Rows.Count; i++)
                    {
                        DropDownList ddlUser = (DropDownList)grdvNotification.Rows[i].FindControl("ddlUser");
                        ddlUser.Items.Clear();
                        ddlUser.DataSource = dsUsers.Tables[0];
                        ddlUser.DataTextField = "UserName";
                        ddlUser.DataValueField = "UserName";
                        ddlUser.DataBind();
                        ddlUser.Items.Insert(0, "ALL");
                        //ddlUser.SelectedIndex = 0;
                    }


                }
            }
            catch (Exception ex)
            {
                dsUsers = null;
            }
            finally
            {
                if (dsUsers != null)
                    dsUsers.Dispose();
            }
        }
        #endregion

        #region Fill Station Dropdown
        private void FillStationDropdown()
        {
            DataSet dsStations = new DataSet();
            try
            {
              dsStations = objNot.GetAllStations();

              if (dsStations != null || dsStations.Tables.Count > 0 || dsStations.Tables[0].Rows.Count > 0)
              {
                  DrpStation.DataSource = dsStations;
                  DrpStation.DataMember = dsStations.Tables[0].TableName;
                  DrpStation.DataTextField = dsStations.Tables[0].Columns["Airport"].ColumnName;
                  DrpStation.DataValueField = dsStations.Tables[0].Columns["AirportCode"].ColumnName;
                  DrpStation.DataBind();
                  //DrpStation.Items.Insert(0, new ListItem("Select", "0")); 
                  DrpStation.Items.Insert(0, "ALL");
                  DrpStation.SelectedIndex = -1;
              }


            }
            catch (Exception ex)
            {
               
            }
        }

        #endregion

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                Session["Data"] = null;

                //if (TxtFrmDt.Text == "")
                //{
                //    lblStatus.Text = "Please Select From Date";
                //    return;
                //}
                //if (txtToDt.Text == "")
                //{
                //    lblStatus.Text = "Please Select To Date";
                //    return;
                //}

                string stn = DrpStation.SelectedItem.Text;
                string station = DrpStation.SelectedItem.Value.ToString();
                station = station.Replace(stn," ");
                station = station.Trim(' ');


                DataSet dsList = new DataSet();


                //dsList = objNot.GetNotificationList(Convert.ToString(TxtFrmDt.Text.Trim()), Convert.ToString(txtToDt.Text.Trim()), drpImp.SelectedItem.Value, drpUpdatedBy.SelectedItem.Value,"");
                dsList = objNot.GetNotificationList(Convert.ToString(TxtFrmDt.Text.Trim()), Convert.ToString(txtToDt.Text.Trim()), drpImp.SelectedItem.Value, drpUpdatedBy.SelectedItem.Text, station,DrpUserRole.SelectedItem.Value);
                
                if (dsList != null)
                {
                    if (dsList.Tables[0].Rows.Count > 0)
                    {
                        
                        grdvNotification.DataSource = dsList.Tables[0];
                        grdvNotification.DataBind();
                        DataSet ds=objNot.GetRoleList();
                        DataSet ds1 = objNot.GetUserList();
                        for (int i = 0; i< grdvNotification.Rows.Count; i++)
                        {
                            DropDownList ddlImp = (DropDownList)grdvNotification.Rows[i].FindControl("ddlImportance");
                            ((DropDownList)grdvNotification.Rows[i].FindControl("ddlImportance")).Text = dsList.Tables[0].Rows[i]["Importance"].ToString();//drpImp.SelectedItem.Value;

                            DropDownList ddlRole1 = (DropDownList)grdvNotification.Rows[i].FindControl("ddlRole");
                            //((DropDownList)grdvNotification.Rows[i].FindControl("ddlRole")).Text = dsList.Tables[0].Rows[i]["Role"].ToString();
                            ddlRole1.Items.Clear();
                            ddlRole1.DataSource = ds.Tables[0];
                            ddlRole1.DataTextField = "RoleName";
                            ddlRole1.DataValueField = "RoleName";
                            ddlRole1.DataBind();
                            ddlRole1.Items.Insert(0, "ALL");

                            //ddlRole1.SelectedIndex = ddlRole1.Items.IndexOf(ddlRole1.Items.FindByValue(dsList.Tables[0].Rows[i]["Role"].ToString()));
                            ddlRole1.SelectedValue = dsList.Tables[0].Rows[i]["Role"].ToString();

                            DropDownList ddlUser = (DropDownList)grdvNotification.Rows[i].FindControl("ddlUser");
                            //((DropDownList)grdvNotification.Rows[i].FindControl("ddlUser")).Text = dsList.Tables[0].Rows[i]["UserName"].ToString();
                            ddlUser.Items.Clear();
                            ddlUser.DataSource = ds1.Tables[0];
                            ddlUser.DataTextField = "UserName";
                            ddlUser.DataValueField = "UserName";
                            ddlUser.DataBind();
                            ddlUser.Items.Insert(0, "ALL");

                            //ddlUser.SelectedIndex = ddlUser.Items.IndexOf(ddlUser.Items.FindByValue(dsList.Tables[0].Rows[i]["UserName"].ToString()));
                            ddlUser.SelectedValue = dsList.Tables[0].Rows[i]["UserName"].ToString();
                        }

                        Session["Data"] = dsList;
                    }
                }

                else if(dsList.Tables[0].Rows.Count ==0)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Record Found";
                    lblStatus.ForeColor = Color.Red;
                    grdvNotification.DataSource = dsList.Tables[0];
                    grdvNotification.DataBind();

                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion

        #region Button Save

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                MasterAuditBAL ObjMAL = new MasterAuditBAL();

                lblStatus.Text = "";
                int cnt = 0;
                bool result = false;
                int SucessCount = 0;
                int FailedCount = 0;

                for (int i = 0; i < grdvNotification.Rows.Count; i++)
                {
                    if (((CheckBox)grdvNotification.Rows[i].FindControl("chkSelect")).Checked == true)
                    {
                        cnt = cnt + 1;

                        string Importance = ((DropDownList)grdvNotification.Rows[i].FindControl("ddlImportance")).Text;
                        string NotificationType = ((TextBox)grdvNotification.Rows[i].FindControl("txtNotiType")).Text;
                        string Role = ((DropDownList)grdvNotification.Rows[i].FindControl("ddlRole")).SelectedItem.Text;
                        string User = ((DropDownList)grdvNotification.Rows[i].FindControl("ddlUser")).SelectedItem.Text;


                        string DD = ((TextBox)grdvNotification.Rows[i].FindControl("txtFrmDt1")).Text.Substring(0, 2);
                        string MM = ((TextBox)grdvNotification.Rows[i].FindControl("txtFrmDt1")).Text.Substring(3, 2);
                        string YY = ((TextBox)grdvNotification.Rows[i].FindControl("txtFrmDt1")).Text.Substring(6, 4);
                        
                        DateTime frmDt = DateTime.Parse(YY + "-" + MM + "-" + DD + " 00:00:00");
                        string frmdate = ((TextBox)grdvNotification.Rows[i].FindControl("txtFrmDt1")).Text;
                        

                        string DD1 = ((TextBox)grdvNotification.Rows[i].FindControl("txtToDt1")).Text.Substring(0, 2);
                        string MM1 = ((TextBox)grdvNotification.Rows[i].FindControl("txtToDt1")).Text.Substring(3, 2);
                        string YY1 = ((TextBox)grdvNotification.Rows[i].FindControl("txtToDt1")).Text.Substring(6, 4);

                        DateTime ToDate = DateTime.Parse(YY1 + "-" + MM1 + "-" + DD1 + " 00:00:00");
                        string todate1 = ((TextBox)grdvNotification.Rows[i].FindControl("txtToDt1")).Text;

                        string NotificationMsg = ((TextBox)grdvNotification.Rows[i].FindControl("txtNotiMsg")).Text;
                        bool isActive = ((CheckBox)grdvNotification.Rows[i].FindControl("chkboxSelect")).Checked;
                        string Subject = ((TextBox)grdvNotification.Rows[i].FindControl("txtSubject")).Text;
                        int RowId = Convert.ToInt32(((Label)grdvNotification.Rows[i].FindControl("txtRowID")).Text.Trim());
                                               
                        result = objNot.SetNotificationDetails(Importance, NotificationType, frmdate, todate1, NotificationMsg, isActive, Session["UserName"].ToString(), Subject, Role, User, RowId, Convert.ToDateTime(Session["IT"]));

                        if (result == true)
                        {

                            #region for Master Audit Log


                            #region Prepare Parameters

                            string NotificationMsg1 = ((TextBox)grdvNotification.Rows[i].FindControl("txtNotiMsg")).Text;
                            string Subject1 = ((TextBox)grdvNotification.Rows[i].FindControl("txtSubject")).Text;

                            object[] Paramsmaster = new object[7];
                            int count = 0;

                            //1

                            Paramsmaster.SetValue("Notification", count);
                            count++;

                            //2
                            Paramsmaster.SetValue(drpUpdatedBy.Text, count);
                            count++;

                            //3

                            Paramsmaster.SetValue("SAVE", count);
                            count++;

                            //4

                            Paramsmaster.SetValue(Subject1, count);
                            count++;


                            //5

                            Paramsmaster.SetValue(NotificationMsg1, count);
                            count++;

                            //6

                            Paramsmaster.SetValue(Session["UserName"], count);
                            count++;

                            //7
                            Paramsmaster.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), count);
                            count++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(Paramsmaster);
                            #endregion

                            //btnList_Click(null,null);
                            SucessCount = SucessCount + 1;
                            lblStatus.Text = "Record added Successfully";
                            lblStatus.ForeColor = Color.Green;
                        }

                        else
                        {
                            FailedCount = FailedCount + 1;
                            lblStatus.Visible = true;
                            lblStatus.Text = "Record Not Saved, Please try again";
                            lblStatus.ForeColor = Color.Red;
                            return;

                        }
                    }
                }

                if (SucessCount > 0 && FailedCount==0)
                {
                    btnList_Click(null, null);
                    lblStatus.Text = "Record(s) added Successfully";
                    lblStatus.ForeColor = Color.Green;
                }
                else if (SucessCount > 0 && FailedCount > 0)
                {
                    btnList_Click(null, null);
                    lblStatus.Text = "Some of the record(s) are failed to Save.";
                    lblStatus.ForeColor = Color.Green;
                }
                else if (SucessCount == 0 && FailedCount > 0)
                {
                    btnList_Click(null, null);
                    lblStatus.Text = "Record(s) are failed to Save.";
                    lblStatus.ForeColor = Color.Green;
                }

                if (cnt == 0)
                {
                    lblStatus.Text = "Please select record(s) to Insert / Update.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion

        #region Button Add New Row

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            DataSet DsAddNewRow = null;
            DataSet ds = objNot.GetRoleList();
            DataSet ds1 = objNot.GetUserList();
            MasterAuditBAL ObjMAL = new MasterAuditBAL();

            try
            {
                #region for Master Audit Log
                //#region Prepare Parameters
                //object[] Paramsmaster = new object[7];
                //int count = 0;

                ////1

                //Paramsmaster.SetValue("Notification", count);
                //count++;

                ////2
                //Paramsmaster.SetValue(drpUpdatedBy.Text, count);
                //count++;

                ////3

                //Paramsmaster.SetValue("ADD", count);
                //count++;

                ////4

                //Paramsmaster.SetValue("", count);
                //count++;


                ////5

                //Paramsmaster.SetValue("NotificationType", count);
                //count++;

                ////6

                //Paramsmaster.SetValue(Session["UserName"], count);
                //count++;

                ////7
                //Paramsmaster.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), count);
                //count++;


                //#endregion Prepare Parameters
                //ObjMAL.AddMasterAuditLog(Paramsmaster);
                #endregion

                //Deepak Modified Code
                #region Add New row to grid
                if (Session["Data"] != null)
                {
                    DsAddNewRow = ((DataSet)Session["Data"]).Copy();
                    DataRow row = DsAddNewRow.Tables[0].NewRow();

                    row["Importance"] = "";
                    row["NotificationType"] = "";
                    row["Role"] = "";
                    row["UserName"] = "";
                    row["FrmDate"] = System.DateTime.Now.ToString("dd/MM/yyyy");//Session["IT"].ToString();
                    row["ToDate"] = System.DateTime.Now.ToString("dd/MM/yyyy");//Session["IT"].ToString();
                    row["Subject1"] = "";
                    row["NotificationMsg"] = "";
                    row["IsActive"] = false;
                    row["ID"] = "0";

                    DsAddNewRow.Tables[0].Rows.Add(row);

                    Session["Data"] = DsAddNewRow.Copy();

                    grdvNotification.DataSource = DsAddNewRow.Copy();
                    grdvNotification.DataBind();

                    for (int i = 0; i < grdvNotification.Rows.Count; i++)
                    {
                        DropDownList ddlImp = (DropDownList)grdvNotification.Rows[i].FindControl("ddlImportance");
                        ((DropDownList)grdvNotification.Rows[i].FindControl("ddlImportance")).Text = DsAddNewRow.Tables[0].Rows[i]["Importance"].ToString();//drpImp.SelectedItem.Value;

                        DropDownList ddlRole1 = (DropDownList)grdvNotification.Rows[i].FindControl("ddlRole");
                        ddlRole1.Items.Clear();
                        ddlRole1.DataSource = ds.Tables[0];
                        ddlRole1.DataTextField = "RoleName";
                        ddlRole1.DataValueField = "RoleName";
                        ddlRole1.DataBind();
                        ddlRole1.Items.Insert(0, "ALL");
                        ddlRole1.SelectedValue = DsAddNewRow.Tables[0].Rows[i]["Role"].ToString();

                        DropDownList ddlUser = (DropDownList)grdvNotification.Rows[i].FindControl("ddlUser");
                        ddlUser.Items.Clear();
                        ddlUser.DataSource = ds1.Tables[0];
                        ddlUser.DataTextField = "UserName";
                        ddlUser.DataValueField = "UserName";
                        ddlUser.DataBind();
                        ddlUser.Items.Insert(0, "ALL");
                        ddlUser.SelectedValue = DsAddNewRow.Tables[0].Rows[i]["UserName"].ToString();

                    }
                    DsAddNewRow.Dispose();
                }
                else
                {

                    DataTable dt = new DataTable();


                    dt.Columns.Add("Importance");
                    dt.Columns.Add("NotificationType");
                    dt.Columns.Add("Role");
                    dt.Columns.Add("UserName");
                    dt.Columns.Add("FrmDate");
                    dt.Columns.Add("ToDate");
                    dt.Columns.Add("Subject1");
                    dt.Columns.Add("NotificationMsg");
                    dt.Columns.Add("IsActive");
                    dt.Columns.Add("ID");

                    DataRow dr = dt.NewRow();
                    dr["Importance"] = "";
                    dr["NotificationType"] = "";
                    dr["Role"] = "";
                    dr["UserName"] = "";
                    dr["FrmDate"] = System.DateTime.Now.ToString("dd/MM/yyyy"); //Session["IT"].ToString();
                    dr["ToDate"] = System.DateTime.Now.ToString("dd/MM/yyyy"); //Session["IT"].ToString();
                    dr["Subject1"] = "";
                    dr["NotificationMsg"] = "";
                    dr["IsActive"] = false;
                    dr["ID"] = "0";
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();

                    grdvNotification.DataSource = dt;
                    grdvNotification.DataBind();
                    dt.Dispose();


                }

               
               
                #endregion

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion

        #region Button Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/FrmNotifications.aspx",false);
            }
            catch (Exception ex)
            { }
        }

        #endregion

        #region Create Empty Row 
        public void CreateEmptyRow()
        {
            btnAdd_Click(null, null);
        }

        #endregion

        #region grdvNotification_PageIndexChanging
        protected void grdvNotification_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet ds = (DataSet)Session["Data"];
                grdvNotification.PageIndex = e.NewPageIndex;
                grdvNotification.DataSource = ds.Tables[0];
                grdvNotification.DataBind();

                DataSet dsR = objNot.GetRoleList();
                DataSet dsU = objNot.GetUserList();

                for (int i = 0; i < grdvNotification.Rows.Count; i++)
                {
                    DropDownList ddlImp = (DropDownList)grdvNotification.Rows[i].FindControl("ddlImportance");
                    ((DropDownList)grdvNotification.Rows[i].FindControl("ddlImportance")).Text = ds.Tables[0].Rows[i]["Importance"].ToString();//drpImp.SelectedItem.Value;

                    DropDownList ddlRole1 = (DropDownList)grdvNotification.Rows[i].FindControl("ddlRole");
                    ddlRole1.Items.Clear();
                    ddlRole1.DataSource = dsR.Tables[0];
                    ddlRole1.DataTextField = "RoleName";
                    ddlRole1.DataValueField = "RoleName";
                    ddlRole1.DataBind();
                    ddlRole1.Items.Insert(0, "ALL");
                    ddlRole1.SelectedValue = ds.Tables[0].Rows[i]["Role"].ToString();



                    DropDownList ddlUser = (DropDownList)grdvNotification.Rows[i].FindControl("ddlUser");
                    ddlUser.Items.Clear();
                    ddlUser.DataSource = dsU.Tables[0];
                    ddlUser.DataTextField = "UserName";
                    ddlUser.DataValueField = "UserName";
                    ddlUser.DataBind();
                    ddlUser.Items.Insert(0, "ALL");
                    ddlUser.SelectedValue = ds.Tables[0].Rows[i]["UserName"].ToString();

                }

            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}

