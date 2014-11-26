using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class FrmUserActivityLog : System.Web.UI.Page
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblStatus.Text = "";
                    #region Define PageSize for grid as per configuration
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        GrdUserLog.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                        objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion
                }
            }
            catch (Exception ex)
            { }
        }

        #region function
        //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'


        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetUser(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'

            SqlDataAdapter dad = new SqlDataAdapter("SELECT distinct UserId from UserActivityLog where UserId like '" + prefixText + "%' or  UserId like '" + prefixText + "%'", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }
            if (ds != null)
                ds.Dispose();
            dad = null;

            return list.ToArray();
        }


        #endregion function
        
        #region List
        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            UserCreationBAL ObjUser = new UserCreationBAL();
            try
            {
                Session["UserActivityLog"] = null;
                lblStatus.Text = "";
                string UserName = "";
                DateTime FromDt = new DateTime(), ToDt = new DateTime();

                if (txtUser.Text.Trim() == "" && txtToDate.Text.Trim() == "" && txtFromDate.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Provide Search Criteria...";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtUser.Text.Trim() != "")
                {
                    UserName = txtUser.Text.Trim();
                }
                if (txtFromDate.Text.Trim() != "")
                {
                    //FromDt = DateTime.ParseExact(txtFromDate.Text.Trim(), "yyyy-MM-dd", null);
                    FromDt = DateTime.ParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", null);
                    //FromDt = (DateTime.Parse(txtFromDate.Text.Trim())).ToString("MM/dd/yyyy");
                }
                else
                {
                    lblStatus.Text = "Please Provide From Date.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtToDate.Text.Trim() != "")
                {
                    //ToDt = DateTime.ParseExact(txtToDate.Text.Trim(), "yyyy-MM-dd", null);
                    ToDt = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy",null);
                    //ToDt = DateTime.Parse(txtToDate.Text.ToString());
                    //ToDt = (DateTime.Parse(txtToDate.Text.Trim())).ToString("MM/dd/yyyy");
                }
                else
                {
                    lblStatus.Text = "Please Provide To Date.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                ds = ObjUser.GetUserLogActivity(UserName, FromDt, ToDt);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    Session["UserActivityLog"] = ds;
                    GrdUserLog.DataSource = ds.Tables[0];
                    GrdUserLog.DataBind();
                }
                else
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    GrdUserLog.DataSource = null;
                    GrdUserLog.DataBind();
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                ObjUser = null;
            }
        }
        #endregion List

        #region Gridview Paging
        protected void GrdUserLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                DataSet dsResult = (DataSet)Session["UserActivityLog"];

                GrdUserLog.PageIndex = e.NewPageIndex;
                GrdUserLog.DataSource = dsResult.Copy();
                GrdUserLog.DataBind();
            }
            catch (Exception ex)
            { }
        }
        #endregion

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
                GrdUserLog.DataSource = null;
                GrdUserLog.DataBind();
                lblStatus.Text = string.Empty;
            }
            catch (Exception ex) { }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

            DataSet dsExp=new DataSet("UserActivity_dsExp");
             DataSet ds = null;
             DataTable dtExp = new DataTable("UserActivity_dtExp");
            UserCreationBAL ObjUser = new UserCreationBAL();
            try
            {
                Session["UserActivityLog"] = null;
                lblStatus.Text = "";
                string UserName = "";
                DateTime FromDt = new DateTime(), ToDt = new DateTime();

                if (txtUser.Text.Trim() == "" && txtToDate.Text.Trim() == "" && txtFromDate.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Provide Search Criteria...";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtUser.Text.Trim() != "")
                {
                    UserName = txtUser.Text.Trim();
                }
                if (txtFromDate.Text.Trim() != "")
                {
                    //FromDt = DateTime.ParseExact(txtFromDate.Text.Trim(), "yyyy-MM-dd", null);
                    FromDt = DateTime.ParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", null);
                    //FromDt = (DateTime.Parse(txtFromDate.Text.Trim())).ToString("MM/dd/yyyy");
                }
                else
                {
                    lblStatus.Text = "Please Provide From Date.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtToDate.Text.Trim() != "")
                {
                    //ToDt = DateTime.ParseExact(txtToDate.Text.Trim(), "yyyy-MM-dd", null);
                    ToDt = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
                    //ToDt = DateTime.Parse(txtToDate.Text.ToString());
                    //ToDt = (DateTime.Parse(txtToDate.Text.Trim())).ToString("MM/dd/yyyy");
                }
                else
                {
                    lblStatus.Text = "Please Provide To Date.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                ds = ObjUser.GetUserLogActivity(UserName, FromDt, ToDt);
                Session["UserActivityLog_exp"] = ds;

                dsExp = (DataSet)Session["UserActivityLog_exp"];
                dtExp = (DataTable)dsExp.Tables[0];
                if (dtExp.Rows.Count < 1)
                {
                    if (Session["UserActivityLog_exp"] == null && dtExp == null)
                    {
                        lblStatus.Text = "No records found";
                        lblStatus.ForeColor = Color.Red;
                        //SaveUserActivityLog(lblStatus.Text);
                        //ReportViewer1.Visible = false;
                        return;
                    }
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;


                }
                else
                {

                    if (dtExp.Columns.Contains("Logo"))
                    { dtExp.Columns.Remove("Logo"); }
                    lblStatus.Text = "";
                    string attachment = "attachment; filename=Report.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in dtExp.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int i;
                    foreach (DataRow dr in dtExp.Rows)
                    {
                        tab = "";
                        for (i = 0; i < dtExp.Columns.Count; i++)
                        {
                            Response.Write(tab + dr[i].ToString());
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
