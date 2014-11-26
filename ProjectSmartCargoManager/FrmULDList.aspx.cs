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
using System.Data.SqlClient;
using System.Configuration;

namespace ProjectSmartCargoManager
{
    public partial class FrmULDList : System.Web.UI.Page
    {
        
        #region variables
        BALULDMaster uld = new BALULDMaster();
        clsFillCombo cfc = new clsFillCombo();
        clsFillCombo clsCombo = new clsFillCombo();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FillULDType();
                    
                    clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", ddlOrigin);
                    ddlOrigin.SelectedIndex = ddlOrigin.Items.IndexOf(ddlOrigin.Items.FindByText(Session["Station"].ToString()));
                    
                    Session["WareHouses"] = clsCombo.ReturnDataset("tblWarehouseMaster", "SELECT").Copy();
                    Session["ULDStatuses"] = clsCombo.ReturnDataset("tblULDStatusMaster", "SELECT").Copy();
                    cfc.FillAllComboBoxes("tblULDOwner", "ALL", drpOwner);

                    if (Request.QueryString["StationId"] != null)
                    {
                        if (Request.QueryString["uldtype"] != null)
                        {
                            ddlOrigin.SelectedIndex = ddlOrigin.Items.IndexOf(ddlOrigin.Items.FindByText(Request.QueryString["StationId"].ToString()));
                            drpULDType.SelectedIndex = drpULDType.Items.IndexOf(drpULDType.Items.FindByText(Request.QueryString["uldtype"].ToString()));
                            btnSearch_Click(sender, e);
                        }
                    }
                    else
                    {
                        btnSearch_Click(sender, e);
                        DataTable dt = new DataTable("ULDLST_DT1");
                        dt.Columns.Add("ULDNumber");
                        dt.Columns.Add("ULDStatus");
                        dt.Columns.Add("Location");
                        dt.Columns.Add("UseStatus");
                        dt.Columns.Add("LocatedOn");
                        gvResult.EmptyDataText = "Please Search Records.";
                        gvResult.DataSource = dt;
                        gvResult.DataBind();
                    }
                }
            }
            catch (Exception ex)
            { 
            
            }
        }
        #endregion Page_Load

        #region Constructor
        public FrmULDList()
        {
           uld = new BALULDMaster();
        }
        #endregion Constructor

        #region Fill Uldtype
        public  void FillULDType()
        {
            DataSet ds = new DataSet("ULDLST_DS1");

            try
            {
                
                ds = uld.FillULDType("tblName", "defaultValue");

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        drpULDType.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        drpULDType.DataSource = ds.Tables[0];
                        drpULDType.DataTextField = "Code";
                        drpULDType.DataValueField = "ID";
                        drpULDType.DataBind();
                        drpULDType.SelectedIndex = 0;
                    }
                    else
                    {
                        drpULDType.Items.Clear();
                        drpULDType.SelectedIndex = 0;
                    }
                }
                else
                {
                    drpULDType.Items.Clear();
                    drpULDType.Items.Add("ALL");
                    drpULDType.SelectedIndex = 0;
                }
            }

            catch (Exception ex)
            {
                lblStatus.Visible = true;
                lblStatus.Text = ex.Message;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion

        #region Fill ULD Owner i.e. Airline Code
        private void fillULDOwner()
        {
            DataSet ds = new DataSet("ULDLST_DS2");
            try
            {

                ds = uld.fillULDOwner("tblName", "defaultValue");

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        drpOwner.Items.Clear();
                        drpOwner.DataSource = ds.Tables[0];
                        drpOwner.DataTextField = "Code";
                        drpOwner.DataValueField = "ID";
                        drpOwner.DataBind();
                        drpOwner.SelectedIndex = 0;
                    }
                    else
                    {
                        drpOwner.Items.Clear();
                        drpOwner.SelectedIndex = 0;
                    }
                }
                else
                {
                    drpOwner.Items.Clear();
                    drpOwner.Items.Add("ALL");
                    drpOwner.SelectedIndex = 0;
                }
            }

            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion Fill ULD Owner i.e. Airline Code

        #region Search Button Click Operations
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //object[] QueryValues = new object[5];
            DataSet ds = new DataSet("ULDLST_DS3");
            btnDelete.Visible = false;
            lblStatus.Text = "";
            try
            {
                #region Commented
                //if (drpULDType.SelectedIndex == 0)
                //{
                //    QueryValues[0] = "";
                //}
                //else
                //{ QueryValues[0] = drpULDType.SelectedItem.Text; }

                //if (drpOwner.SelectedIndex == 0)
                //{
                //    QueryValues[1] = "";
                //}
                //else
                //{ QueryValues[1] = drpOwner.SelectedItem.Text; }

                //if (ddlOrigin.SelectedIndex == 0)
                //{
                //    QueryValues[2] = "";
                //}
                //else
                //{
                //    QueryValues[2] = ddlOrigin.SelectedItem.Text;

                //}

                //QueryValues[3] = txtULDNo.Text.Trim();

                //if (drpUseStatus.SelectedIndex == 0)
                //{
                //    QueryValues[4] = "";
                //}
                //else
                //{
                //    QueryValues[4] = drpUseStatus.SelectedValue.ToString();
                //}

                ////QueryValues[6] = txtFltNo.Text.Trim();

                //ds = uld.SearchList(QueryValues);
                #endregion
                ds = getULDList();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        lblStatus.Text = "No records found.";
                        lblStatus.ForeColor = Color.Red;
                        gvResult.DataSource = ds.Tables[0];
                        gvResult.EmptyDataText = "No records found.";
                        gvResult.DataBind();
                        btnULDMovementHistory.Visible = false;
                    }
                    else if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblStatus.Text = "";
                        lblStatus.ForeColor = Color.Green;
                        gvResult.DataSource = ds.Tables[0];
                        gvResult.DataBind();
                        btnULDMovementHistory.Visible = true;
                        btnDelete.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Green;
            }
            finally
            {
                //QueryValues = null;
                if (ds != null)
                    ds.Dispose();
            }

        }
        #endregion

        #region gvResult_RowCommand
        protected void gvResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ShowULDDeatils")
                {

                    string ULDNO = e.CommandArgument.ToString();
                    Response.Redirect("FrmULDMaster.aspx?ULDNO=" + ULDNO.ToString(),false);
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion gvResult_RowCommand

        #region Button ULD Movement History
        protected void btnULDMovementHistory_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                string ULDNumber = string.Empty;
                int count = 0;
                for (int i = 0; i < gvResult.Rows.Count; i++)
                {
                    if (((CheckBox)gvResult.Rows[i].FindControl("chkList")).Checked)
                    {
                        count++;
                        ULDNumber += "," + ((LinkButton)gvResult.Rows[i].FindControl("lblULDNo")).Text.Trim();
                        
                    }
                }
                if (count == 0)
                {
                    for (int i = 0; i < gvResult.Rows.Count; i++)
                    {

                        ULDNumber += ","+ ((LinkButton)gvResult.Rows[i].FindControl("lblULDNo")).Text.Trim();

                    }
                }
                if (ULDNumber != "")
                {
                    Session["ULDMovementList"] = ULDNumber;
                    Response.Redirect("~/frmULDMovementHistory.aspx", false);
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region gvResult_PageIndexChanging
        protected void gvResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            object[] QueryValues = new object[5];
            DataSet ds = new DataSet("ULDLST_DS4");

            try
            {
                if (drpULDType.SelectedIndex == 0)
                {
                    QueryValues[0] = "";
                }
                else
                { QueryValues[0] = drpULDType.SelectedItem.Text; }

                if (drpOwner.SelectedIndex == 0)
                {
                    QueryValues[1] = "";
                }
                else
                { QueryValues[1] = drpOwner.SelectedItem.Text; }

                if (ddlOrigin.SelectedIndex == 0)
                {
                    QueryValues[2] = "";
                }
                else
                {
                    QueryValues[2] = ddlOrigin.SelectedItem.Text;
                }

                QueryValues[3] = txtULDNo.Text.Trim();
                //QueryValues[6] = txtFltNo.Text.Trim();

                if (drpUseStatus.SelectedIndex == 0)
                {
                    QueryValues[4] = "";
                }
                else
                {
                    QueryValues[4] = drpUseStatus.SelectedValue.ToString();
                }


                ds = uld.SearchList(QueryValues);

                gvResult.PageIndex = e.NewPageIndex;
                gvResult.DataSource = ds.Tables[0];
                gvResult.DataBind();

            }

            catch (Exception ex)
            {

            }
            finally
            {
                QueryValues = null;
                if (ds != null)
                    ds.Dispose();
            }

        }
        #endregion gvResult_PageIndexChanging

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmULDList.aspx", false);
        }
        #endregion btnClear_Click

        #region btnDelete_Click
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            lblStatus.ForeColor = Color.Green;
            try
            {
                if (hdnSelectedRows.Value == null || hdnSelectedRows.Value == "")
                {
                    lblStatus.Text = "Please select atleast one row";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                bool isDeleteAllowed = false;
                string ULDDeleteAllowedFor = "";
                ULDDeleteAllowedFor = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AllowULDDeleteForRole");
                if (ULDDeleteAllowedFor !=  null && ULDDeleteAllowedFor != "")
                {
                    string[] arrayAllowDel;
                    if (ULDDeleteAllowedFor.Contains(","))
                    {   //Split ULD Numbers..
                        arrayAllowDel = ULDDeleteAllowedFor.Split(',');
                        if (arrayAllowDel != null && arrayAllowDel.Length > 0)
                        {
                            foreach (var item in arrayAllowDel)
                            {
                                if (item.ToUpper() == Session["RoleName"].ToString().ToUpper())
                                {
                                    isDeleteAllowed = true;
                                    break;
                                }
                            }
                        }
                        arrayAllowDel = null;
                    }
                    else
                    {
                        if (ULDDeleteAllowedFor.ToUpper() == Session["RoleName"].ToString().ToUpper())
                        {
                            isDeleteAllowed = true;
                        }
                    }
                }

                if (isDeleteAllowed)
                {
                    string[] arrayRowIndex = hdnSelectedRows.Value.Split(';');
                    if (arrayRowIndex != null && arrayRowIndex.Length > 0)
                    {
                        foreach (var item in arrayRowIndex)
                        {
                            if (item != "")
                            {
                                if (!uld.DeleteULDMaster(((LinkButton)gvResult.Rows[int.Parse(item) - 1].FindControl("lblULDNo")).Text))
                                {
                                    btnSearch_Click(this, new EventArgs());
                                    lblStatus.Text = "ULD(s) could not be deleted. Please try again.";
                                    lblStatus.ForeColor = Color.Red;
                                    return;
                                }
                            }
                        }
                        btnSearch_Click(this, new EventArgs());
                        lblStatus.Text = "ULD(s) deleted successfully.";
                        lblStatus.ForeColor = Color.Green;
                        return;
                    }
                }
                else
                {
                    lblStatus.Text = "You are not allowed to delete ULD(s).";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception)
            {
            }
            lblStatus.Text = "ULD(s) could not be deleted. Please try again.";
            lblStatus.ForeColor = Color.Red;
        }
        #endregion btnDelete_Click

        #region Export Click
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("ULDLST_DS5");
            btnDelete.Visible = false;
            lblStatus.Text = "";
            try
            {
                ds = getULDList();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        lblStatus.Text = "No records found.";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable("ULDLST_DT2");
                        dt = (DataTable)ds.Tables[0];
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string attachment = "attachment; filename=ULDList.xls";
                            Response.ClearContent();
                            Response.ContentEncoding = System.Text.Encoding.UTF8;
                            Response.AddHeader("content-disposition", attachment);
                            Response.ContentType = "application/vnd.ms-excel";
                            string tab = "";
                            foreach (DataColumn dc in dt.Columns)
                            {
                                Response.Write(tab + dc.ColumnName);
                                //tab = "\t";
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
                                    //tab = "\t";
                                    tab = "\t";
                                }
                                Response.Write("\n");
                            }
                            Response.End();

                        }
                        else
                        {
                            lblStatus.Text = "No record found";
                            lblStatus.ForeColor = Color.Red;
                            Session["Rateline"] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Green;
            }
            finally
            {
                //QueryValues = null;
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion

        #region getULDList
        private DataSet getULDList()
        {
            object[] QueryValues = new object[5];
            DataSet dsData = new DataSet("ULDLST_DS6");
            try
            {
                if (drpULDType.SelectedIndex == 0)
                {
                    QueryValues[0] = "";
                }
                else
                { QueryValues[0] = drpULDType.SelectedItem.Text; }

                if (drpOwner.SelectedIndex == 0)
                {
                    QueryValues[1] = "";
                }
                else
                { QueryValues[1] = drpOwner.SelectedItem.Text; }

                if (ddlOrigin.SelectedIndex == 0)
                {
                    QueryValues[2] = "";
                }
                else
                {
                    QueryValues[2] = ddlOrigin.SelectedItem.Text;

                }

                QueryValues[3] = txtULDNo.Text.Trim();

                if (drpUseStatus.SelectedIndex == 0)
                {
                    QueryValues[4] = "";
                }
                else
                {
                    QueryValues[4] = drpUseStatus.SelectedValue.ToString();
                }

                //QueryValues[6] = txtFltNo.Text.Trim();

                dsData = uld.SearchList(QueryValues);

            }
            catch (Exception ex) { }
            return dsData;
        }
        #endregion

    }
}


