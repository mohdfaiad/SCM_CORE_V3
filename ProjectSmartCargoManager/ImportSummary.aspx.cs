using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BAL;
using System.Drawing;
using QID.DataAccess;
using System.IO;

namespace ProjectSmartCargoManager
{
    public partial class ImportSummary : System.Web.UI.Page
    {
        #region Constructor
        BALException objBAL = new BALException();
        #endregion Constructor

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                GetOrigin();
            }
        }
        
        # region Get Origin List

        private void GetOrigin()
        {
            DataSet ds = null;
            try
            {
                ds = objBAL.GetOriginCodeList(ddlLocation.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlLocation.DataSource = ds;
                            ddlLocation.DataMember = ds.Tables[0].TableName;
                            ddlLocation.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;

                            ddlLocation.DataTextField = ds.Tables[0].Columns["Airport"].ColumnName;
                            ddlLocation.DataBind();
                            ddlLocation.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }

        # endregion GetOriginCode List

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            grdImportList.Visible = false;
            ddlLocation.SelectedIndex = 0;
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DateTime fromDate;
            DateTime ToDate;
            string Location;
            grdImportList.Visible = false;
            lblStatus.Text = string.Empty;
            if (txtFrmDate.Text == "" || txtToDate.Text == "")
            {
                lblStatus.Text = "Please select date";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            fromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
            ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
            if (ddlLocation.SelectedIndex > 0)
            {
                Location = ddlLocation.SelectedValue.ToString();
            }
            else
            {
                Location = "All";
            }
            #region ValidateDays
            ReportBAL objBal = new ReportBAL();
            string strResult = string.Empty;

            try
            {
                strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null));
            }
            catch
            {
                strResult = "";
            }
            finally
            {
                objBal = null;
            }
            if (strResult != "")
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = strResult;
                grdImportList.Visible = false;
                txtFrmDate.Focus();
                return;
            }
#endregion 
            #region import export

            #region Parameter
            string[] PName = new string[3];
            PName[0] = "Source";
            PName[1] = "frmDate";
            PName[2] = "ToDate";

            object[] PValue = new object[3];
            PValue[0] = Location;
            PValue[1] = fromDate;//txtFrmDate.Text;
            PValue[2] = ToDate;//txtToDate.Text;

            SqlDbType[] PType = new SqlDbType[3];
            PType[0] = SqlDbType.VarChar;
            PType[1] = SqlDbType.Date;
            PType[2] = SqlDbType.Date;

            #endregion
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet dsimpexp = null;
            
            dsimpexp = da.SelectRecords("SP_GetImport", PName, PValue, PType);
            try
            {
                if (dsimpexp != null)
                {
                    Session["dsimpexp"] = dsimpexp;
                    if (dsimpexp.Tables != null)
                    {
                        if (dsimpexp.Tables.Count > 0)
                        {
                            if (dsimpexp.Tables[0].Rows.Count > 0)
                            {
                                grdImportList.PageIndex = 0;
                                grdImportList.DataSource = dsimpexp;
                                grdImportList.DataMember = dsimpexp.Tables[0].TableName;
                                grdImportList.DataBind();
                                grdImportList.Visible = true;
                            }
                            else
                            {
                                lblStatus.Text = "No Records Found.";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }
            finally
            {
                if (dsimpexp != null)
                    dsimpexp.Dispose();
                PName = null;
                PType = null;
                PValue = null;
            }
            #endregion
        }

        # region grdImportList_PageIndexChanging
        protected void grdImportList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsimpexp = (DataSet)Session["dsimpexp"];
            grdImportList.PageIndex = e.NewPageIndex;
            grdImportList.DataSource = dsimpexp.Copy();
            grdImportList.DataBind();
            if (dsimpexp != null)
            {
                dsimpexp.Dispose();
            }
        }
        # endregion grdImportList_PageIndexChanging

    }
}
