using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using BAL;
using System.IO;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class JournalAccountMaster : System.Web.UI.Page
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = null;
            if (!IsPostBack)
            {
                try
                {
                    txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                    txtFlightCode.Text = Session["AirlinePrefix"].ToString();
                    txtFltFromDt.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                    txtfrmdt.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                    txttodt.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                    btnExport.Visible = false;
                }
                catch (Exception)
                {
                  
                }
                
                try
                {
                    ds = db.SelectRecords("Sp_GetSCNAcctField");

                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            try
                            {
                                ddlSCMAcctField.DataSource = ds;
                                ddlSCMAcctField.DataMember = ds.Tables[0].TableName;
                                ddlSCMAcctField.DataTextField = ds.Tables[0].Columns["ChargeHeadCode"].ColumnName;
                                ddlSCMAcctField.DataValueField = ds.Tables[0].Columns["ChargeHeadCode"].ColumnName;
                                ddlSCMAcctField.DataBind();
                                ddlSCMAcctField.Items.Insert(0, "Select");
                            }
                            catch (Exception)
                            {

                            }

                            try
                            {
                                ddlChartAccID.DataSource = ds;
                                ddlChartAccID.DataMember = ds.Tables[3].TableName;
                                ddlChartAccID.DataTextField = ds.Tables[3].Columns["AccountID"].ColumnName;
                                ddlChartAccID.DataValueField = ds.Tables[3].Columns["AccountID"].ColumnName;
                                ddlChartAccID.DataBind();
                                ddlChartAccID.Items.Insert(0, "Select");
                            }
                            catch (Exception ex)
                            { }
                            //try
                            //{
                            //    ddlBlSCMAcc.DataSource = ds;
                            //    ddlBlSCMAcc.DataMember = ds.Tables[0].TableName;
                            //    ddlBlSCMAcc.DataTextField = ds.Tables[0].Columns["ChargeHeadCode"].ColumnName;
                            //    ddlBlSCMAcc.DataValueField = ds.Tables[0].Columns["ChargeHeadCode"].ColumnName;
                            //    ddlBlSCMAcc.DataBind();
                            //    ddlBlSCMAcc.Items.Insert(0, "Select");
                            //}
                            //catch (Exception ex)
                            //{ }
                            //try
                            //{
                            //    ddlDbAccID.DataSource = ds;
                            //    ddlDbAccID.DataMember = ds.Tables[3].TableName;
                            //    ddlDbAccID.DataTextField = ds.Tables[3].Columns["AccountID"].ColumnName;
                            //    ddlDbAccID.DataValueField = ds.Tables[3].Columns["AccountID"].ColumnName;
                            //    ddlDbAccID.DataBind();
                            //    ddlDbAccID.Items.Insert(0, "Select");
                            //}
                            //catch (Exception ex)
                            //{ }
                            //try
                            //{
                            //    ddlCrAccID.DataSource = ds;
                            //    ddlCrAccID.DataMember = ds.Tables[3].TableName;
                            //    ddlCrAccID.DataTextField = ds.Tables[3].Columns["AccountID"].ColumnName;
                            //    ddlCrAccID.DataValueField = ds.Tables[3].Columns["AccountID"].ColumnName;
                            //    ddlCrAccID.DataBind();
                            //    ddlCrAccID.Items.Insert(0, "Select");
                            //}
                            //catch (Exception ex)
                            //{ }

                            //try
                            //{
                            //    ddlBlAccID.DataSource = ds;
                            //    ddlBlAccID.DataMember = ds.Tables[3].TableName;
                            //    ddlBlAccID.DataTextField = ds.Tables[3].Columns["AccountID"].ColumnName;
                            //    ddlBlAccID.DataValueField = ds.Tables[3].Columns["AccountID"].ColumnName;
                            //    ddlBlAccID.DataBind();
                            //    ddlBlAccID.Items.Insert(0, "Select");
                            //}
                            //catch (Exception ex)
                            //{ }

                            try
                            {
                                DataSet dsentity = db.SelectRecords("Sp_GetEntityType");
                                ddlEntity.DataSource = dsentity;
                                ddlEntity.DataMember = dsentity.Tables[0].TableName;
                                ddlEntity.DataTextField = dsentity.Tables[0].Columns["EntityType"].ColumnName;
                                ddlEntity.DataValueField = dsentity.Tables[0].Columns["EntityType"].ColumnName;
                                ddlEntity.DataBind();
                                ddlEntity.Items.Insert(0, "Select");
                            }
                            catch (Exception ex)
                            { }

                        }
                    }
                }
                catch (Exception ex)
                { }
                finally
                {
                    if (ds != null)
                    {
                        ds.Dispose();
                    }
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/JournalAccountMaster.aspx");
            
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                
                #region Prepare Parameters

                string[] paramname = new string[13];
                paramname[0] = "AWBPrefix";
                paramname[1] = "AWBNumber";
                paramname[2] = "FltDate";
                paramname[3] = "FltNumber";
                paramname[4] = "DbSCMAcctField";
                paramname[5] = "FltPrefix";
                paramname[6] = "frndt";
                paramname[7] = "todt";
                paramname[8] = "AccountType";
                paramname[9] = "EntityType";
                paramname[10] = "DbCrAccountID";
                paramname[11] = "EntityID";
                paramname[12] = "ChartID";

                object[] paramvalue = new object[13];
                paramvalue[0] = txtAWBPrefix.Text.Trim();

                paramvalue[1] = txtAWBNo.Text.Trim();
                paramvalue[2] = txtFltFromDt.Text.Trim();
                if (txtFlightID.Text.Length > 0)
                {
                    paramvalue[3] = txtFlightCode.Text + txtFlightID.Text;
                }
                else
                {
                    paramvalue[3] = "";
                }
                
                if (ddlSCMAcctField.SelectedItem.Text == "Select")
                {
                    paramvalue[4] = "";
                }
                else
                {
                    paramvalue[4] = ddlSCMAcctField.SelectedItem.Text.Trim();
                }
               
                paramvalue[5] = txtFlightCode.Text.Trim();
                paramvalue[6] = txtfrmdt.Text.Trim();
                paramvalue[7] = txttodt.Text.Trim();
                if (ddlAccountType.SelectedItem.Text == "Select")
                {
                    paramvalue[8] = "";
                }
                else
                {
                    paramvalue[8] = ddlAccountType.SelectedItem.Text.Trim();
                }
                if (ddlEntity.SelectedItem.Text == "Select")
                {
                    paramvalue[9] = "";
                }
                else
                {
                    paramvalue[9] = ddlEntity.SelectedItem.Text.Trim();
                }
                paramvalue[10] = txtDbCrAccountID.Text.Trim();
                paramvalue[11] = txtEntityID.Text.Trim();
                if (ddlChartAccID.SelectedItem.Text == "Select")
                {
                    paramvalue[12] = "";
                }
                else
                {
                    paramvalue[12] = ddlChartAccID.SelectedItem.Text.Trim();
                }

                SqlDbType[] paramtype = new SqlDbType[13];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.VarChar;
                paramtype[7] = SqlDbType.VarChar;
                paramtype[8] = SqlDbType.VarChar;
                paramtype[9] = SqlDbType.VarChar;
                paramtype[10] = SqlDbType.VarChar;
                paramtype[11] = SqlDbType.VarChar;
                paramtype[12] = SqlDbType.VarChar;

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = db.SelectRecords("Sp_GetJournalAccountList", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdJournalAccount.PageIndex = 0;
                                grdJournalAccount.DataSource = ds;
                                grdJournalAccount.DataMember = ds.Tables[0].TableName;
                                grdJournalAccount.DataBind();
                                grdJournalAccount.Visible = true;
                                Session["grdJournalAccount"] = ds;
                                lblStatus.Text = "";
                                btnExport.Visible = true;
                                
                            }
                            else if (ds.Tables[0].Rows.Count <= 0)
                            {

                                grdJournalAccount.DataSource = null;
                                grdJournalAccount.DataBind();
                                lblStatus.Text = "Record does not exist";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void grdJournalAccount_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet dstemp = (DataSet)Session["grdJournalAccount"];
                grdJournalAccount.PageIndex = e.NewPageIndex;
                grdJournalAccount.DataSource = dstemp;
                grdJournalAccount.DataMember = dstemp.Tables[0].TableName;
                grdJournalAccount.DataBind();
                //for (int j = 0; j < grdChartAccount.Rows.Count; j++)
                //{
                //    if (((Label)(grdChartAccount.Rows[j].FindControl("lblIsActive"))).Text.ToString() == "True")
                //    {
                //        ((Label)(grdChartAccount.Rows[j].FindControl("lblIsActive"))).Text = "Active";
                //    }
                //    else if (((Label)(grdChartAccount.Rows[j].FindControl("lblIsActive"))).Text.ToString() == "False")
                //    {
                //        ((Label)(grdChartAccount.Rows[j].FindControl("lblIsActive"))).Text = "InActive";
                //    }
                //}
            }
            catch (Exception ex)
            {
            }
        }
        protected void grdJournalAccount_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AWBNumber")
            {
            DataSet dstempcmd = (DataSet)Session["grdJournalAccount"];
            int index = Convert.ToInt32(e.CommandArgument);
            string AWBNumber = ((LinkButton)grdJournalAccount.Rows[index].FindControl("lblAWBNumber")).Text.Trim();
            DataTable dr = dstempcmd.Tables[0].Select("DbCrAccountType='DEBIT' and AWBNumber='" + AWBNumber + "'").CopyToDataTable();

            DataTable dr1 = dstempcmd.Tables[0].Select("DbCrAccountType='CREDIT' and AWBNumber='" + AWBNumber + "'").CopyToDataTable();

            DataTable expdt = dstempcmd.Tables[0].Select("AWBNumber='" + AWBNumber + "'").CopyToDataTable();
            Session["MailBooking_Popgrd"] = expdt;
            grdDebit.DataSource = dr;
            grdDebit.DataBind();
            grdCredit.DataSource = dr1;
            grdCredit.DataBind();

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
            
            }

            if (e.CommandName == "View")
            {
                DataTable dr1 = null;
                DataTable dr = null;
                DataSet dstempcmd = (DataSet)Session["grdJournalAccount"];
                int index = Convert.ToInt32(e.CommandArgument);
                string AWBNumber = ((LinkButton)grdJournalAccount.Rows[index].FindControl("lblAWBNumber")).Text.Trim();
                string Type = ((Label)grdJournalAccount.Rows[index].FindControl("lblDbSCMAcctField")).Text.Trim();
                try
                {
                    dr = dstempcmd.Tables[0].Select("DbCrAccountType='DEBIT' and DbSCMAcctField='" + Type + "' and AWBNumber='" + AWBNumber + "'").CopyToDataTable();
                }
                catch (Exception ex)
                { }
                try
                {
                    dr1 = dstempcmd.Tables[0].Select("DbCrAccountType='CREDIT' and DbSCMAcctField='" + Type + "' and AWBNumber='" + AWBNumber + "'").CopyToDataTable();
                }
                catch (Exception ex)
                { }
                grdDebit.DataSource = dr;
                grdDebit.DataBind();
                grdCredit.DataSource = dr1;
                grdCredit.DataBind();
                DataTable expdt = dstempcmd.Tables[0].Select("DbSCMAcctField='" + Type + "' and AWBNumber='" + AWBNumber + "'").CopyToDataTable();
                Session["MailBooking_Popgrd"] = expdt;

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);

            }
                
           

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            lblStatus.Text = "";
            try
            {
                dsExp = (DataSet)Session["grdJournalAccount"];
                dt = (DataTable)dsExp.Tables[0];
               
                string attachment = "attachment; filename=Report.xls";
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

        protected void btnExportPopup_Click(object sender, EventArgs e)
        {
            
            DataTable dt = null;
            lblStatus.Text = "";
            try
            {
                
                dt = (DataTable)Session["MailBooking_Popgrd"];

                string attachment = "attachment; filename=Report.xls";
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
                
                dt = null;
            }
        }
    }
}
