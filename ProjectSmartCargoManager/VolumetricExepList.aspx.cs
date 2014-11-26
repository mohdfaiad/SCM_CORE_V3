using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using BAL;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class VolumetricExepList : System.Web.UI.Page
    {

        ListOtherChargesBAL objBAL = new ListOtherChargesBAL();
        SQLServer da = new SQLServer(Global.GetConnectionString());

        # region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HomeBL objHome = new HomeBL();
                int RoleId = Convert.ToInt32(Session["RoleID"]);
                DataSet objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
                objHome = null;

                txtFromDate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                txtToDate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");

                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
                    {
                        if (objDS.Tables[0].Rows[i]["ControlId"].ToString() == "OtherChargeEdit")
                        {
                            GRDList.Columns[14].Visible = false;
                            break;
                        }
                    }
                }
                objDS = null;
            }
        }
        # endregion Page_Load

        # region btnList_Click
          protected void btnList_Click(object sender, EventArgs e)
        {
            
            #region Prepare Parameters
            
            object[] RateLineInfo = new object[4];
            string[] ColumnNames = new string[4];
            SqlDbType[] DataType = new SqlDbType[4];
            
            int i = 0;

            if (txtFromDate.Text.Trim() == "")
            {
                if (txtToDate.Text.Trim() != "")
                {
                    lblStatus.Text = "Please enter From date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
            }

            if (txtToDate.Text.Trim() == "")
            {
                if (txtFromDate.Text.Trim() != "")
                {
                    lblStatus.Text = "Please enter To date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
            }
            
            //1
            if (txtFromDate.Text.Trim() != "")
            {
                DateTime dtfrom;

                try
                {
                    dtfrom = DateTime.ParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                RateLineInfo.SetValue(DateTime.ParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", null), i);
            }
            else
            {
                RateLineInfo.SetValue("", i);
            }
            i++;

            //2
            if (txtToDate.Text.Trim() != "")
            {
                DateTime dtto;

                try
                {
                    dtto = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                RateLineInfo.SetValue(DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null), i);
            }
            else
            {
                RateLineInfo.SetValue("", i);
            }
            i++;

            //3
            RateLineInfo.SetValue(ddlStatus.SelectedValue, i);
            i++;

            //4
            RateLineInfo.SetValue(txtParam.Text, i);

            int j = 0;
            ColumnNames.SetValue("FromDt", j);
            DataType.SetValue(SqlDbType.DateTime, j);
            j++;

            ColumnNames.SetValue("ToDate", j);
            DataType.SetValue(SqlDbType.DateTime, j);
            j++;

            ColumnNames.SetValue("Status", j);
            DataType.SetValue(SqlDbType.VarChar, j);
            j++;

            ColumnNames.SetValue("Parameters", j);
            DataType.SetValue(SqlDbType.VarChar, j);

            #endregion

            DataSet ds = da.SelectRecords("sp_SearchVolumetricDetails", ColumnNames, RateLineInfo, DataType);
            if (ds != null)
            {
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {

                    GRDList.DataSource = ds;
                    GRDList.DataMember = ds.Tables[0].TableName;
                    GRDList.DataBind();
                    Session["OtherCharges"] = ds;
                    lblStatus.Text = string.Empty;
                }
                else
                {
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                    GRDList.DataSource = null;
                    GRDList.DataBind();
                }
            }
        }
        # endregion btnList_Click

        #region Grid Row Commmand "Edit" and "View"
         protected void GRDList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit" || e.CommandName == "View")
                {
                    string SrNo = ((HiddenField)GRDList.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("Hid")).Value;
                    Response.Redirect("~/VolumetricException.aspx?cmd=" + e.CommandName + "&SrNo=" + SrNo + "");
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        protected void GRDList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult = (DataSet)Session["OtherCharges"];

            GRDList.PageIndex = e.NewPageIndex;
            GRDList.DataSource = dsResult.Copy();
            GRDList.DataBind();

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;

            try
            {
                if ((DataSet)Session["OtherCharges"] == null)
                    return;

                ds = (DataSet)Session["OtherCharges"];
                dt = (DataTable)ds.Tables[0];
                //ExportToExcel(dt, "RateLines.xls");
                //dt = city.GetAllCity();//your datatable 
                string attachment = "attachment; filename=TaxLineList.xls";
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
                ds = null;
                dt = null;
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/VolumetricExepList.aspx", false);
        }

    }
}
