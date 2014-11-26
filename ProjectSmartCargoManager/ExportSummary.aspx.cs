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
    public partial class ExportSummary : System.Web.UI.Page
    {
        //BALException objBAL = new BALException();
        //SQLServer da = new SQLServer(Global.GetConnectionString());
        //DataSet dsimpexp = new DataSet();

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
            BALException objBAL = new BALException();
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
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                objBAL = null;
            }
        }

        # endregion GetOriginCode List

        
        protected void btnList_Click(object sender, EventArgs e)
        {
            DateTime fromDate;
            DateTime ToDate;
            string Location;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet dsimpexp = new DataSet();
            string[] PName = new string[3];
            object[] PValue = new object[3];
            SqlDbType[] PType = new SqlDbType[3];
            if (txtFrmDate.Text == "" || txtToDate.Text == "")
            {
                lblStatus.Text = "Please select date";
                lblStatus.ForeColor = Color.Red;
                grdexportList.Visible = false;
                return;
            }
            #region validateDays
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
                grdexportList.Visible = false;
                txtFrmDate.Focus();
                return;
            }
            #endregion
            try
            {
                //txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                fromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
                ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
                if (ddlLocation.SelectedIndex > 0)
                {
                    Location = ddlLocation.SelectedValue.ToString();               }
                else
                {
                    Location = "All";
                }

                #region import export

                #region Parameter

                PName[0] = "Source";
                PName[1] = "frmDate";
                PName[2] = "ToDate";

                PValue[0] = Location;
                PValue[1] = fromDate;//txtFrmDate.Text;
                PValue[2] = ToDate;//txtToDate.Text;


                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.Date;
                PType[2] = SqlDbType.Date;

                #endregion
                dsimpexp = da.SelectRecords("SP_GetExport", PName, PValue, PType);

                Session["dsimpexp"] = dsimpexp;

                if (dsimpexp != null)
                {
                    if (dsimpexp.Tables != null)
                    {
                        if (dsimpexp.Tables.Count > 0)
                        {
                            if (dsimpexp.Tables[0].Rows.Count > 0)
                            {
                                grdexportList.PageIndex = 0;
                                grdexportList.DataSource = dsimpexp;
                                grdexportList.DataMember = dsimpexp.Tables[0].TableName;
                                grdexportList.DataBind();
                                grdexportList.Visible = true;
                                //dsFLAB.Clear();
                                btnExport.Visible = true;
                                lblStatus.Text = "";

                            }
                            else
                            {
                                lblStatus.Text = "No Recoreds are Found.";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }
            finally
            {
                da = null;
                if (dsimpexp != null)
                    dsimpexp.Dispose();
                PName = null;
                PValue = null;
                PType = null;
            }
            
            #endregion
        }

        # region grdexportList_PageIndexChanging
        protected void grdexportList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
            DataSet dsimpexp = (DataSet)Session["dsimpexp"];
            grdexportList.PageIndex = e.NewPageIndex;
            grdexportList.DataSource = dsimpexp.Copy();
            grdexportList.DataBind();

            if (dsimpexp != null)
                dsimpexp.Dispose();
        }
        # endregion grvNFLABList_PageIndexChanging

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            grdexportList.Visible = false;
            ddlLocation.SelectedIndex = 0;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //ExportGridToExcel(grvNFLABList, "StudentMarks.xls");
            DataSet dsExp = null;
            DataTable dt = null;

            try
            {
                if ((DataSet)Session["dsimpexp"] == null)
                    //if(ds == null)
                    return;

                dsExp = (DataSet)Session["dsimpexp"];
                //dsExp = ds;
                dt = (DataTable)dsExp.Tables[0];
                //dt = city.GetAllCity();//your datatable 
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
                if (dsExp != null)
                    dsExp.Dispose();
                if (dt != null)
                    dt.Dispose();                 
            }
        }

        protected void txtFrmDate_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
