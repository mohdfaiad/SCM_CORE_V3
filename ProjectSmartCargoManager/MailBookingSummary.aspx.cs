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

namespace ProjectSmartCargoManager
{
    public partial class MailBookingSummary : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropDowns();
                BindEmptyRow();

                txtFromDate.Text = txtToDate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
            }
        }

        protected void LoadDropDowns()
        {
            DataSet ds = new DataSet("mailBksumarry_ds13");

            try
            {
                ds = da.SelectRecords("spGetAirportCodes");
                ddlPOffOrg.DataSource = ds.Tables[0];
                ddlPOffOrg.DataTextField = "Airport";
                ddlPOffOrg.DataValueField = "AirportCode";
                ddlPOffOrg.DataBind();
                ddlPOffOrg.Items.Insert(0, new ListItem("Select", "All"));

                ddlPOffDest.DataSource = ds.Tables[0];
                ddlPOffDest.DataTextField = "Airport";
                ddlPOffDest.DataValueField = "AirportCode";
                ddlPOffDest.DataBind();
                ddlPOffDest.Items.Insert(0, new ListItem("Select", "All"));

                ds = da.SelectRecords("SP_GetAllStationCodeName", "level", "country", SqlDbType.VarChar);
                ddlPAdmOrg.DataSource = ds.Tables[0];
                ddlPAdmOrg.DataTextField = "CountryDesc";
                ddlPAdmOrg.DataValueField = "CountryCode";
                ddlPAdmOrg.DataBind();
                ddlPAdmOrg.Items.Insert(0, new ListItem("Select", "All"));
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void BindEmptyRow()
        {
            DataTable dt = new DataTable("mailBksumarry_dt1");
            dt.Columns.Add("ConsignmentID");
            dt.Columns.Add("PostalAdminOrg");
            dt.Columns.Add("PostOfficeOrg");
            dt.Columns.Add("PostOfficeDest");
            dt.Columns.Add("FltDt");
            dt.Columns.Add("FltNo");
            dt.Columns.Add("FltOrg");
            dt.Columns.Add("FltDest");

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            grdMailSummary.DataSource = null;
            grdMailSummary.DataSource = dt;
            grdMailSummary.DataBind();
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("mailBksumarry_ds11");
            lblStatus.Text = string.Empty;
            try
            {
                if (ValidateDate() == false)
                    return;

                #region Parameters
                object[] ParamValue = new object[8];
                string[] ParamName = new string[8];
                SqlDbType[] ParamType = new SqlDbType[8];

                int i = 0;

                //0
                ParamValue.SetValue(txtConID.Text.Trim(),i);
                ParamName.SetValue("ConsignmentID", i);
                ParamType.SetValue(SqlDbType.VarChar, i);
                i++;

                //1
                ParamValue.SetValue(txtFromDate.Text.Trim(), i);
                ParamName.SetValue("FrmDt", i);
                ParamType.SetValue(SqlDbType.VarChar, i);
                i++;

                //2
                ParamValue.SetValue(txtToDate.Text.Trim(), i);
                ParamName.SetValue("ToDt", i);
                ParamType.SetValue(SqlDbType.VarChar, i);
                i++;

                //3
                ParamValue.SetValue(txtFltDt.Text.Trim(), i);
                ParamName.SetValue("FltDt", i);
                ParamType.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                ParamValue.SetValue(txtFltPrefix.Text.Trim()+txtFltNo.Text.Trim(), i);
                ParamName.SetValue("FltNo", i);
                ParamType.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                ParamValue.SetValue(ddlPAdmOrg.SelectedValue, i);
                ParamName.SetValue("PostAdmOrg", i);
                ParamType.SetValue(SqlDbType.VarChar, i);
                i++;
                
                //6
                ParamValue.SetValue(ddlPOffOrg.SelectedValue, i);
                ParamName.SetValue("PostOffOrg", i);
                ParamType.SetValue(SqlDbType.VarChar, i);
                i++;

                //7
                ParamValue.SetValue(ddlPOffDest.SelectedValue, i);
                ParamName.SetValue("PostOffDest", i);
                ParamType.SetValue(SqlDbType.VarChar, i);
                i++;
                #endregion

                ds = da.SelectRecords("sp_GetMailBookingSummary", ParamName, ParamValue, ParamType);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    grdMailSummary.DataSource = ds.Tables[0];
                    grdMailSummary.DataBind();
                    ViewState["ds"] = ds;
                }
                else
                {
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                    BindEmptyRow();
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected bool ValidateDate()
        {
            if (txtFromDate.Text.Trim() == "" || txtToDate.Text.Trim() == "")
            {
                lblStatus.Text = "Enter From Date and To Date";
                lblStatus.ForeColor = Color.Red;
                return false;
            }
            try
            {
                DateTime dtFrm = DateTime.ParseExact(txtFromDate.Text.Trim(),"dd/MM/yyyy", null);
                DateTime dtTo = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
                if (txtFltDt.Text.Trim() != "")
                {
                    DateTime dtFltDt = DateTime.ParseExact(txtFltDt.Text.Trim(), "dd/MM/yyyy", null);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Enter Date in dd/MM/yyyy format";
                lblStatus.ForeColor = Color.Red;
                return false;
            }
            return true;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtConID.Text = string.Empty;

            txtFromDate.Text = txtToDate.Text = string.Empty;
            txtFltDt.Text = txtFltPrefix.Text = txtFltNo.Text = string.Empty;
            ddlPAdmOrg.SelectedIndex = 0;
            ddlPOffOrg.SelectedIndex = ddlPOffDest.SelectedIndex = 0;

            grdMailSummary.DataSource = null;
            grdMailSummary.DataBind();

            lblStatus.Text = string.Empty;
        }

        protected void grdMailSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet("mailBksumarry_ds112");
            try
            {
                ds = (DataSet)ViewState["ds"];
                grdMailSummary.PageIndex = e.NewPageIndex;
                grdMailSummary.DataSource = ds.Tables[0];
                grdMailSummary.DataBind();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void grdMailSummary_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }     
    }
}
