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
    public partial class rptImportWareHouseInv : System.Web.UI.Page
    {

        #region Variables
        BALException objBAL = new BALException();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtvalidfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtvalidto.Text = DateTime.Now.ToString("dd/MM/yyyy"); 
                GetAirportCode();
                //LoadGridUserList();
                btnExport.Visible = true;
            }
        }

        #region Add New Row to Grid
        public void LoadGridUserList()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AWBPrefix";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AWBNumber";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Origin";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Destination";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltDate";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltNo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "RCVPcs";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "RCVWt";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DeliveredPcs";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DeliveredWt";
            myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            dr["AWBPrefix"] = "";
            dr["AWBNumber"] = "";
            dr["Origin"] = "";
            dr["Destination"] = "";
            dr["FltDate"] = "";
            dr["FltNo"] = "";
            dr["RCVPcs"] = "";
            dr["RCVWt"] = "";
            dr["DeliveredPcs"] = "";
            dr["DeliveredWt"] = "";


            myDataTable.Rows.Add(dr);

            grdimpList.DataSource = null;
            grdimpList.DataSource = myDataTable;
            grdimpList.DataBind();
            Session["dsdata"] = myDataTable.Copy();

        }
        #endregion


        #region Get Origin List
        private void GetAirportCode()
        {
            DataSet ds = null;
            try
            {
                ds = objBAL.GetAirportCodeList(ddlAirport.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlAirport.DataSource = ds;
                            ddlAirport.DataMember = ds.Tables[0].TableName;
                            ddlAirport.DataValueField ="AirportCode";

                            ddlAirport.DataTextField = "Airport";
                            ddlAirport.DataBind();
                            ddlAirport.Items.Insert(0, "All");
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
        #endregion GetOriginCode List

        protected void btnList_Click(object sender, EventArgs e)
        {
            string[] PName = new string[3];
            object[] PValue = new object[3];
            SqlDbType[] PType = new SqlDbType[3];
            DataSet dsimp = null;
            lblStatus.Text = string.Empty;
            if (txtvalidfrom.Text == "" || txtvalidto.Text == "")
            {
                lblStatus.Text = "Please select date";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            #region ValidateDays
            ReportBAL objBal = new ReportBAL();
            string strResult = string.Empty;

            try
            {
                strResult = objBal.GetReportInterval(DateTime.ParseExact(txtvalidfrom.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtvalidto.Text.Trim(), "dd/MM/yyyy", null));
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
                grdimpList.Visible = false;
                txtvalidfrom.Focus();
                return ;
            }
            #endregion
            try
            {
                string Airport;
                string frmdt = "", todt = "";
                if (ddlAirport.SelectedItem.Text.Trim() == "All")
                {
                    Airport = "";
                }
                else
                Airport = ddlAirport.SelectedValue.ToString();
                #region Parameter

                PName[0] = "AirportCode";
                PName[1] = "Frmdt";
                PName[2] = "Todt";

                PValue[0] = Airport;
                if (txtvalidfrom.Text != "")
                {
                    frmdt = txtvalidfrom.Text.Trim();
                    PValue[1] = frmdt;
                }

                if (txtvalidto.Text != "")
                {
                    todt = txtvalidto.Text.Trim();
                    PValue[2] = todt;
                }

                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;
                PType[2] = SqlDbType.VarChar;

                #endregion

                dsimp = da.SelectRecords("[GetImpWareHouseInvList]", PName, PValue, PType);
                if (dsimp != null)
                {
                    Session["dsimp"] = dsimp;
                    if (dsimp.Tables != null)
                    {
                        if (dsimp.Tables.Count > 0)
                        {
                            if (dsimp.Tables[0].Rows.Count > 0)
                            {
                                grdimpList.PageIndex = 0;
                                grdimpList.DataSource = dsimp;
                                grdimpList.DataMember = dsimp.Tables[0].TableName;
                                grdimpList.DataBind();
                                grdimpList.Visible = true;
                                btnExport.Visible = true;
                                SaveUserActivityLog("");
                            }
                            else
                            {
                                lblStatus.Text = "No data found for given search criteria";
                                lblStatus.ForeColor = Color.Red;
                                grdimpList.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               String ErrorLog = ex.Message;
                SaveUserActivityLog(ErrorLog);
            }
            finally
            {
                if (dsimp != null)
                    dsimp.Dispose();
                PName = null;
                PValue = null;
                PType = null;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            try
            {
                if ((DataSet)Session["dsimp"] == null)
                {
                    SaveUserActivityLog(lblStatus.Text);
                    return;
                }

                dsExp = (DataSet)Session["dsimp"];

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
            catch (Exception)
            { }
            finally
            {
                if (dsExp != null)
                    dsExp.Dispose();
                if (dt != null)
                    dt.Dispose();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtvalidfrom.Text = "";
            txtvalidto.Text = "";
           ddlAirport.SelectedIndex = 0;
           grdimpList.Visible = false;
           lblStatus.Text = string.Empty;
        }

        #region grdimpList_PageIndexChanging
        protected void grdimpList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string[] PName = new string[3];
            object[] PValue = new object[3];
            SqlDbType[] PType = new SqlDbType[3];
            DataSet dsimp = null;
            try
            {
                string Airport;
                string frmdt = "", todt = "";
                if (ddlAirport.SelectedItem.Text.Trim() == "All")
                    Airport = "";
                else
                    Airport = ddlAirport.SelectedValue.ToString().Trim();
                //Airport = ddlAirport.SelectedItem.Text.Trim() == "All" ? "" : ddlAirport.SelectedItem.Text.Trim();
                #region Parameter

                PName[0] = "AirportCode";
                PName[1] = "Frmdt";
                PName[2] = "Todt";

                PValue[0] = Airport;
                if (txtvalidfrom.Text != "")
                {
                    frmdt = txtvalidfrom.Text.Trim();
                    PValue[1] = frmdt; //Convert.ToDateTime(frmdt).ToString("dd/MM/yyyy");
                }
                if (txtvalidto.Text != "")
                {
                    todt = txtvalidto.Text.Trim();
                    PValue[2] = todt; //Convert.ToDateTime(todt).ToString("dd/MM/yyyy");
                }

                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;
                PType[2] = SqlDbType.VarChar;

                #endregion

                dsimp = da.SelectRecords("GetImpWareHouseInvList", PName, PValue, PType);
                if (dsimp != null)
                {
                    if (dsimp.Tables != null)
                    {
                        if (dsimp.Tables.Count > 0)
                        {
                            if (dsimp.Tables[0].Rows.Count > 0)
                            {
                                grdimpList.PageIndex = 0;
                                grdimpList.DataSource = dsimp;
                                grdimpList.DataMember = dsimp.Tables[0].TableName;
                                grdimpList.DataBind();
                                grdimpList.Visible = true;
                            }
                        }
                    }
                }

                grdimpList.PageIndex = e.NewPageIndex;
                grdimpList.DataSource = dsimp.Copy();
                grdimpList.DataBind();

            }
            catch (Exception)
            { }
            finally
            {
                if (dsimp != null)
                {
                    dsimp.Dispose();
                }
                PName = null;
                PValue = null;
                PType = null;
            }
        }
        # endregion grdimpList_PageIndexChanging

        protected void grdimpList_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Station:" + ddlAirport.Text.ToString() + ", FrmDt:" + txtvalidfrom.Text.ToString() + ", ToDt:" + txtvalidto.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), " WareHouse Inventory Import", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }

    }
}
