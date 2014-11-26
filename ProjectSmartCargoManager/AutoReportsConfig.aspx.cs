using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using QID.DataAccess;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Configuration;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class AutoReportsConfig : System.Web.UI.Page
    {
        #region Variables
        BALAutoReportsConfig objARC = new BALAutoReportsConfig();
        #endregion Variables

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCurrentReportConfig("", "", "");
            }
        }

        #region Load grid TDS on Commission Intial Row
        public void LoadGridReportConfig()
        {
            try
            {
                DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;
                DataSet Ds = new DataSet();

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "SrNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ReportName";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ToEmail";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FromDate";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ToDate";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "SPName";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Frequency";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "IsActive";
                myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["SrNo"] = 0;
                dr["ReportName"] = "";
                dr["ToEmail"] = "";
                dr["FromDate"] = ""; //Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                dr["ToDate"] = ""; //Convert.ToDateTime(Session["IT"]).AddDays(1).ToString("dd/MM/yyyy");
                dr["SPName"] = "";
                //dr["Frequency"] = "Daily";
                dr["IsActive"] = true;

                myDataTable.Rows.Add(dr);
                ViewState["vsReportInfo"] = myDataTable;

                grdReportList.DataSource = null;
                grdReportList.DataSource = myDataTable;
                grdReportList.DataBind();

                Session["dtReportInfo"] = myDataTable.Copy();

                for (int i = 0; i < myDataTable.Rows.Count; i++)
                {
                    ((DropDownList)grdReportList.Rows[i].FindControl("ddlReportFrequency")).SelectedValue = myDataTable.Rows[i]["Frequency"].ToString();

                    if (myDataTable.Rows[i]["IsActive"].ToString() == "True")
                    {
                        ((CheckBox)grdReportList.Rows[i].FindControl("chkisactive")).Checked = true;
                    }
                }
            }
            catch (Exception e) { }
        }
        #endregion Load grid TDS on Commission Intial Row

        #region Load Current Report Config
        public void LoadCurrentReportConfig(string ReportName, string FromDate, string ToDate)
        {
            try
            {
                DataSet dsARC = objARC.getCurrentReportConfig(ReportName, FromDate, ToDate);

                if (dsARC != null && dsARC.Tables[0].Rows.Count > 0)
                {
                    grdReportList.DataSource = dsARC.Tables[0];
                    grdReportList.DataBind();

                    Session["dtReportInfo"] = dsARC.Tables[0];

                    for (int i = 0; i < dsARC.Tables[0].Rows.Count; i++)
                    {
                        ((DropDownList)grdReportList.Rows[i].FindControl("ddlReportFrequency")).SelectedValue = dsARC.Tables[0].Rows[i]["Frequency"].ToString();

                        if (dsARC.Tables[0].Rows[i]["IsActive"].ToString() == "True")
                        {
                            ((CheckBox)grdReportList.Rows[i].FindControl("chkisactive")).Checked = true;
                        }
                    }
                }
                else
                {
                    grdReportList.DataSource = null;
                    grdReportList.DataBind();

                    Session["dtReportInfo"] = null;

                    //LoadGridReportConfig();
                }

            }
            catch (Exception ex)
            {
            }
        }
        #endregion Load Current Report Config

        #region Save Auto report email configuration
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateOnSave())
                    return;

                bool res = false;
                int SrNo = 0;
                string ReportName = "", ToEmail = "", FromDate = "", ToDate = "", SPName = "", Frequency = "", strResult = "", Updatedby = "", Updatedon = "";
                bool isActive = true;

                Updatedby = Convert.ToString(Session["Username"]);
                Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");


                for (int k = 0; k < grdReportList.Rows.Count; k++)
                {
                    SrNo = int.Parse(((Label)grdReportList.Rows[k].FindControl("lblSrNo")).Text);
                    ReportName = ((TextBox)grdReportList.Rows[k].FindControl("txtReportName")).Text;
                    ToEmail = ((TextBox)grdReportList.Rows[k].FindControl("txtToEmail")).Text;
                    FromDate = ((TextBox)grdReportList.Rows[k].FindControl("txtFromDate")).Text;
                    ToDate = ((TextBox)grdReportList.Rows[k].FindControl("txtToDate")).Text;
                    SPName = ((TextBox)grdReportList.Rows[k].FindControl("txtSPName")).Text;
                    Frequency = ((DropDownList)grdReportList.Rows[k].FindControl("ddlReportFrequency")).SelectedValue;
                    isActive = ((CheckBox)grdReportList.Rows[k].FindControl("chkisactive")).Checked;

                    DataSet dsResult = objARC.SaveAutoReportsConfig(SrNo, ReportName, ToEmail, FromDate, ToDate, SPName, Frequency, isActive, Updatedby, Updatedon, ref strResult);

                    if (dsResult != null)
                    {
                        if (dsResult.Tables != null)
                        {
                            if (dsResult.Tables.Count > 0)
                            {
                                if (dsResult.Tables[0].Rows[0][0].ToString() == "Configuration saved successfully")
                                {
                                    res = true;
                                }
                                else
                                {
                                    res = false;
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Configuration not saved";
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Configuration not saved";
                        return;
                    }
                }
                if (res == true)
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Configuration saved successfully";
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Save Auto report email configuration

        #region Validate before save
        protected bool ValidateOnSave()
        {
            string strfromdate = "";
            DateTime fromdt;
            string strtodate = "";
            DateTime todt;
            List<DateTime> frmList = new List<DateTime>();
            List<DateTime> toList = new List<DateTime>();
            for (int i = 0; i < grdReportList.Rows.Count; i++)
            {
                if (((TextBox)grdReportList.Rows[i].FindControl("txtReportName")).Text.Trim() == "")
                {
                    lblStatus.Text = "Enter Report Name";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                if (((TextBox)grdReportList.Rows[i].FindControl("txtToEmail")).Text.Trim() == "")
                {
                    lblStatus.Text = "Enter To Email";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                if (((TextBox)grdReportList.Rows[i].FindControl("txtFromDate")).Text.Trim() == "" || ((TextBox)grdReportList.Rows[i].FindControl("txtToDate")).Text.Trim() == "")
                {
                    lblStatus.Text = "Enter From Date and To Date";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                if (((TextBox)grdReportList.Rows[i].FindControl("txtSPName")).Text.Trim() == "")
                {
                    lblStatus.Text = "Enter SP Name";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                try
                {
                    string day = ((TextBox)grdReportList.Rows[i].FindControl("txtFromDate")).Text.Substring(0, 2);
                    string mon = ((TextBox)grdReportList.Rows[i].FindControl("txtFromDate")).Text.Substring(3, 2);
                    string yr = ((TextBox)grdReportList.Rows[i].FindControl("txtFromDate")).Text.Substring(6, 4);
                    strfromdate = yr + "-" + mon + "-" + day;
                    fromdt = Convert.ToDateTime(strfromdate);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Please enter valid From Date";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                try
                {
                    string day = ((TextBox)grdReportList.Rows[i].FindControl("txtToDate")).Text.Substring(0, 2);
                    string mon = ((TextBox)grdReportList.Rows[i].FindControl("txtToDate")).Text.Substring(3, 2);
                    string yr = ((TextBox)grdReportList.Rows[i].FindControl("txtToDate")).Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    todt = Convert.ToDateTime(strtodate);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Please enter valid To Date";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                
                if (fromdt > todt)
                {
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }

            return true;
        }
        #endregion Validate before save

        protected void BtnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            LoadCurrentReportConfig(txtReportName.Text, txtFromDate.Text, txtToDate.Text);
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            txtReportName.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            Session["dtReportInfo"] = null;
            grdReportList.DataSource = null;
            grdReportList.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtReports = new DataTable();

                if (Session["dtReportInfo"] != null)
                {
                    dtReports = (DataTable)Session["dtReportInfo"];
                }
                else
                {
                    DataColumn myDataColumn;
                    DataSet Ds = new DataSet();

                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType("System.String");
                    myDataColumn.ColumnName = "SrNo";
                    dtReports.Columns.Add(myDataColumn);

                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType("System.String");
                    myDataColumn.ColumnName = "ReportName";
                    dtReports.Columns.Add(myDataColumn);

                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType("System.String");
                    myDataColumn.ColumnName = "ToEmail";
                    dtReports.Columns.Add(myDataColumn);

                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType("System.String");
                    myDataColumn.ColumnName = "FromDate";
                    dtReports.Columns.Add(myDataColumn);

                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType("System.String");
                    myDataColumn.ColumnName = "ToDate";
                    dtReports.Columns.Add(myDataColumn);

                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType("System.String");
                    myDataColumn.ColumnName = "SPName";
                    dtReports.Columns.Add(myDataColumn);

                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType("System.String");
                    myDataColumn.ColumnName = "Frequency";
                    dtReports.Columns.Add(myDataColumn);

                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType("System.String");
                    myDataColumn.ColumnName = "IsActive";
                    dtReports.Columns.Add(myDataColumn);
                }

                //Code to add value of rows which are already available in grid
                if (dtReports.Rows.Count > 0)
                {
                    string srno = "", ReportName = "", ToEmail = "", fromdate = "", todate = "", SPName = "", Frequency = "";
                    bool IsActive = false;

                    for (int i = 0; i < grdReportList.Rows.Count; i++)
                    {
                        srno = ((Label)grdReportList.Rows[i].FindControl("lblSrNo")).Text;
                        ReportName = ((TextBox)grdReportList.Rows[i].FindControl("txtReportName")).Text;
                        ToEmail = ((TextBox)grdReportList.Rows[i].FindControl("txtToEmail")).Text;
                        fromdate = ((TextBox)grdReportList.Rows[i].FindControl("txtFromDate")).Text;
                        todate = ((TextBox)grdReportList.Rows[i].FindControl("txtTodate")).Text;
                        SPName = ((TextBox)grdReportList.Rows[i].FindControl("txtSPName")).Text;
                        Frequency = ((DropDownList)grdReportList.Rows[i].FindControl("ddlReportFrequency")).SelectedValue;
                        IsActive = ((CheckBox)grdReportList.Rows[i].FindControl("chkisactive")).Checked;

                        dtReports.Rows[i]["SrNo"] = srno;
                        dtReports.Rows[i]["ReportName"] = ReportName;
                        dtReports.Rows[i]["ToEmail"] = ToEmail;
                        dtReports.Rows[i]["FromDate"] = fromdate;
                        dtReports.Rows[i]["ToDate"] = todate;
                        dtReports.Rows[i]["SPName"] = SPName;
                        dtReports.Rows[i]["Frequency"] = Frequency;
                        dtReports.Rows[i]["IsActive"] = IsActive;
                    }
                }

                DataRow dr;
                dr = dtReports.NewRow();
                dr["SrNo"] = 0;
                dr["ReportName"] = "";
                dr["ToEmail"] = "";
                dr["FromDate"] = "";
                dr["ToDate"] = "";
                dr["SPName"] = "";
                //dr["Frequency"] = "Daily";
                dr["IsActive"] = true;

                dtReports.Rows.Add(dr);
                ViewState["vsReportInfo"] = dtReports;

                grdReportList.DataSource = null;
                grdReportList.DataSource = dtReports;
                grdReportList.DataBind();

                Session["dtReportInfo"] = dtReports.Copy();

                for (int i = 0; i < dtReports.Rows.Count; i++)
                {
                    ((DropDownList)grdReportList.Rows[i].FindControl("ddlReportFrequency")).SelectedValue = dtReports.Rows[i]["Frequency"].ToString();

                    if (dtReports.Rows[i]["IsActive"].ToString() == "True")
                    {
                        ((CheckBox)grdReportList.Rows[i].FindControl("chkisactive")).Checked = true;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
