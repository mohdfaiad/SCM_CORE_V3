using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class ReportLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    #region Define PageSize for grid as per configuration
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        grdRptLog.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                        objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion
                    GetDropDownData(); //Fill DropDown Lists
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void GetDropDownData()
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {
                ds = da.SelectRecords("sp_GetRptName");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlRptList.Items.Clear();
                        ddlRptList.DataSource = ds.Tables[0];
                        ddlRptList.DataTextField = "Page";
                        ddlRptList.DataBind();
                        ddlRptList.Items.Insert(0, new ListItem("All"));
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ddlUserList.Items.Clear();
                        ddlUserList.DataSource = ds.Tables[1];
                        ddlUserList.DataTextField = "LoginName";
                        ddlUserList.DataBind();
                        ddlUserList.Items.Insert(0, new ListItem("All"));
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
                if (da != null)
                    da = null;
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;

            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            ddlRptList.SelectedIndex = 0;
            ddlUserList.SelectedIndex = 0;

            grdRptLog.DataSource = null;
            grdRptLog.DataBind();
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet dsData=new DataSet();
            SQLServer da = new SQLServer(Global.GetConnectionString());

            string dtFrom1 = txtFromDate.Text;
            string dtTo1 = txtToDate.Text;

            string dtFrom = dtFrom1.Substring(0, 3) + dtFrom1.Substring(3, 3) + dtFrom1.Substring(6, 4);
            string dtTo = dtTo1.Substring(0, 3) + dtTo1.Substring(3, 3) + dtTo1.Substring(6, 4);

            try
            {
                lblStatus.Text = string.Empty;

                if (ChkDate() == false)
                    return;

                #region Parameters

                object[] Values = new object[4];
                int i = 0;

                //0
                //Values.SetValue(txtFromDate.Text.Trim(), i);

                Values.SetValue(dtFrom, i);
                i++;

                //1
                //Values.SetValue(txtToDate.Text.Trim(), i);

                Values.SetValue(dtTo, i);
                i++;

                //2
                Values.SetValue(ddlUserList.SelectedItem.Text, i);
                i++;

                //2
                Values.SetValue(ddlRptList.SelectedItem.Text, i);

                string[] ParamNames = { "FromDt", "ToDt", "User", "RptName" };
                SqlDbType[] ParamTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
               
                #endregion

                dsData = da.SelectRecords("sp_GetRptLogData", ParamNames, Values, ParamTypes);

                if (dsData != null)
                {
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        grdRptLog.DataSource = dsData.Tables[0];
                        grdRptLog.DataBind();
                        ViewState["dsRptLog"] = dsData;
                    }
                    else
                    {
                        lblStatus.Text = "No Records Found...";
                        lblStatus.ForeColor = Color.Red;
                        grdRptLog.DataSource = null;
                        grdRptLog.DataBind();
                        return;
                    }
                }
                else
                {
                    lblStatus.Text = "No Records Found...";
                    lblStatus.ForeColor = Color.Red;
                    grdRptLog.DataSource = null;
                    grdRptLog.DataBind();
                    return;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dsData != null)
                    dsData.Dispose();
                if (da != null)
                    da = null;
            }
        }

        protected bool ChkDate()
        {
            if (txtFromDate.Text.Trim() == "" || txtToDate.Text.Trim() == "")
            {
                lblStatus.Text = "Enter Dates";
                lblStatus.ForeColor = Color.Red;
                return false;
            }
            if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "")
            {
                DateTime dtFrom, dtTo;
                try
                {
                    dtFrom = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
                    dtTo = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Enter valid dates";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                int chk = DateTime.Compare(dtFrom, dtTo);
                if (dtTo<dtFrom)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid ToDate";
                    txtFromDate.Focus();
                    return false;
                }
                return true;
            }
            return true;
        }

        protected void grdRptLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsData = new DataSet();
            try
            {
                dsData = (DataSet)ViewState["dsRptLog"];
                grdRptLog.PageIndex = e.NewPageIndex;
                grdRptLog.DataSource = dsData;
                grdRptLog.DataBind();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dsData != null)
                    dsData.Dispose();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsData=new DataSet();
            DataTable dt = new DataTable();
            SQLServer da = new SQLServer(Global.GetConnectionString());
            Session["ReportLog_dsData"] = null;
            string dtFrom1 = txtFromDate.Text;
            string dtTo1 = txtToDate.Text;

            string dtFrom = dtFrom1.Substring(0, 3) + dtFrom1.Substring(3, 3) + dtFrom1.Substring(6, 4);
            string dtTo = dtTo1.Substring(0, 3) + dtTo1.Substring(3, 3) + dtTo1.Substring(6, 4);

            try
            {
                lblStatus.Text = string.Empty;

                if (ChkDate() == false)
                    return;

                #region Parameters

                object[] Values = new object[4];
                int i = 0;

                Values.SetValue(dtFrom, i);
                i++;

                Values.SetValue(dtTo, i);
                i++;

                //2
                Values.SetValue(ddlUserList.SelectedItem.Text, i);
                i++;

                //2
                Values.SetValue(ddlRptList.SelectedItem.Text, i);

                string[] ParamNames = { "FromDt", "ToDt", "User", "RptName" };
                SqlDbType[] ParamTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                #endregion

                dsData = da.SelectRecords("sp_GetRptLogData", ParamNames, Values, ParamTypes);
                //Session["ReportLog_dsData"]=dsData;
                dt = (DataTable)dsData.Tables[0];
                if (dt.Rows.Count < 1)
                {

                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;


                }
                else
                {
                    if (dt.Columns.Contains("Logo"))
                    { dt.Columns.Remove("Logo"); }
                    if (dt.Columns.Contains("userURL"))
                    { dt.Columns.Remove("userURL"); }
                    lblStatus.Text = "";
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
                    int p;
                    foreach (DataRow dr in dt.Rows)
                    {
                        tab = "";
                        for (p = 0; p< dt.Columns.Count; p++)
                        {
                            Response.Write(tab + dr[p].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();
                
                
                }

            }
            catch (Exception ex)
            { }
            finally { }
        }
    }
}
