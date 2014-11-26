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
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class ExchRateMaster : System.Web.UI.Page
    {
        BALExchRate objBal = new BALExchRate();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCurrList();
                GetExchRateType();
                Session["dsExchRateMaster"] = null;
                BindEmptyRow();
            }
        }

        #region Bind Empty Row
        protected void BindEmptyRow()
        {
            DataTable dtEmpty = new DataTable();
            try
            {
                dtEmpty.Columns.Add("ICRID");
                dtEmpty.Columns.Add("CurrencyCode");
                dtEmpty.Columns.Add("CurrencyIATARate");
                dtEmpty.Columns.Add("ValidFrom");
                dtEmpty.Columns.Add("ValidTo");
                dtEmpty.Columns.Add("Type");

                DataRow dr = dtEmpty.NewRow();
                dtEmpty.Rows.Add(dr);

                exchGrd.DataSource = dtEmpty;
                exchGrd.DataBind();

                exchGrd.Columns[6].Visible = false;
                exchGrd.Columns[7].Visible = false;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dtEmpty != null)
                    dtEmpty.Dispose();
            }
        }
        #endregion

        #region Curr List
        private void GetCurrList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = objBal.GetCurrList();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ddlCurr.DataSource = ds;
                                ddlCurr.DataMember = ds.Tables[0].TableName;
                                ddlCurr.DataValueField = ds.Tables[0].Columns["Code"].ColumnName;

                                ddlCurr.DataTextField = ds.Tables[0].Columns["CurrencyCode"].ColumnName;
                                ddlCurr.DataBind();
                                ddlCurr.Items.Insert(0, new ListItem("Select", "Select"));
                                ddlCurr.SelectedIndex = -1;
                                ds.Clear();
                            }
                            else if (ds.Tables[0].Rows.Count == 0)
                            {
                                //Page_Load(null, null);
                                //btnList_Click(null, null);
                                //lblStatus.Text = "No Records Exists";
                                //lblStatus.ForeColor = Color.Red;

                            }
                        }
                    }
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
        #endregion Exch Rate List

        #region Get Exch Rate Types
        private void GetExchRateType()
        {
            try
            {
                DataSet ds = objBal.GetExchrate();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlRtType.DataSource = ds;
                            ddlRtType.DataMember = ds.Tables[0].TableName;
                            ddlRtType.DataValueField = ds.Tables[0].Columns["Currencytype"].ColumnName;

                            ddlRtType.DataTextField = ds.Tables[0].Columns["Currencytype"].ColumnName;
                            ddlRtType.DataBind();
                            ddlRtType.Items.Insert(0, new ListItem("Select", "Select"));
                            ddlRtType.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Get Exch Rate Types

        #region Exch Rate List
        private void GetExchList()
        {

            DataSet ds = new DataSet();
            lblStatus.Text = string.Empty;
            try
            {
                #region Params
                object[] Params = new object[4];
                int i = 0;

                //0
                if (ddlCurr.SelectedIndex == 0)
                    Params.SetValue("", i);
                else
                    Params.SetValue(ddlCurr.SelectedValue.ToString(), i);
                i++;

                //1
                if (ddlRtType.SelectedIndex == 0)
                    Params.SetValue("", i);
                else
                    Params.SetValue(ddlRtType.SelectedItem.Text.ToString(), i);
                i++;

                //2
                Params.SetValue(txtDtFrm.Text.Trim(), i);
                i++;

                //3
                Params.SetValue(txtDtTo.Text.Trim(), i);

                #endregion


                ds = objBal.GetExchList(Params);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                exchGrd.PageIndex = 0;
                                exchGrd.DataSource = ds.Tables[0];
                                exchGrd.DataBind();
                                ViewState["ds"] = ds;
                                Session["dsExchRateMaster"] = ds;
                                exchGrd.Columns[6].Visible = true;
                                exchGrd.Columns[7].Visible = true;
                            }
                            else if (ds.Tables[0].Rows.Count == 0)
                            {
                                lblStatus.Text = "No Records Exists";
                                lblStatus.ForeColor = Color.Red;
                                Session["dsExchRateMaster"] = null;
                                BindEmptyRow();
                            }
                        }
                    }
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
        #endregion Exch Rate List

        #region Add Exch Rate
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateDate() == false)
                    return;

                if (ddlRtType.SelectedIndex == 0)
                {
                    lblStatus.Text = "Select Rate Type";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (ddlCurr.SelectedIndex == 0)
                {
                    lblStatus.Text = "Select Currency";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                # region Save
                if (btnSave.Text == "Save")
                {
                    try
                    {
                        if (ddlRtType.SelectedIndex > 0)
                        {
                            #region Prepare Parameters
                            object[] Params = new object[7];
                            int i = 0;
                            string uname = Session["UserName"].ToString();
                            DateTime time = DateTime.Parse(Session["IT"].ToString());

                            //1
                            Params.SetValue(ddlCurr.SelectedValue.ToString(), i);
                            i++;

                            //2
                            Params.SetValue(txtExchRt.Text, i);
                            i++;

                            //3
                            Params.SetValue(txtDtFrm.Text, i);
                            i++;

                            //4
                            Params.SetValue(txtDtTo.Text, i);
                            i++;

                            //5
                            Params.SetValue(Session["UserName"].ToString(), i);
                            i++;

                            //6
                            DateTime dtnow = (DateTime)Session["IT"];
                            Params.SetValue(dtnow, i);
                            i++;

                            //7
                            Params.SetValue(ddlRtType.SelectedItem.Text, i);
                            i++;

                            #endregion Prepare Parameters

                            int ID = 0;
                            ID = objBal.AddExchRate(Params);
                            if (ID >= 0)
                            {
                                btnClear_Click(null, null);
                                GetExchList();
                                lblStatus.Text = "Record Added Successfully";
                                lblStatus.ForeColor = Color.Green;
                                btnSave.Text = "Save";

                            }
                            else
                            {
                                GetExchList();
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Record Insertion Failed..";

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                # endregion Save

                # region Update
                if (btnSave.Text == "Update")
                {

                    try
                    {

                        #region Prepare Parameters
                        object[] Params = new object[8];
                        int i = 0;
                        string uname = Session["UserName"].ToString();
                        DateTime time = DateTime.Parse(Session["IT"].ToString());
                        long serialnumber = long.Parse(Session["SrNum"].ToString());
                        //DateTime creationtime = DateTime.Parse(Session["creationtime"].ToString());
                        //string createdby = Session["createdby"].ToString();

                        //0
                        Params.SetValue(serialnumber, i);
                        i++;

                        //1
                        Params.SetValue(ddlCurr.SelectedValue.ToString(), i);
                        i++;

                        //2
                        Params.SetValue(txtExchRt.Text, i);
                        i++;

                        //3
                        Params.SetValue(txtDtFrm.Text, i);
                        i++;

                        //4
                        Params.SetValue(txtDtTo.Text, i);
                        i++;

                        //5
                        Params.SetValue(Session["UserName"].ToString(), i);
                        i++;

                        //6
                        DateTime dtnow = (DateTime)Session["IT"];
                        Params.SetValue(dtnow, i);
                        i++;

                        //7
                        Params.SetValue(ddlRtType.SelectedItem.Text, i);
                        i++;

                        #endregion Prepare Parameters

                        int ID = 0;
                        ID = objBal.UpdateExchDetail(Params);
                        if (ID >= 0)
                        {
                            btnClear_Click(null, null);
                            GetExchList();
                            lblStatus.Text = "Record Updated Successfully";
                            lblStatus.ForeColor = Color.Green;
                            btnSave.Text = "Save";


                        }
                        else
                        {
                            GetExchList();
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Record Updation Failed..";

                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
                # endregion Update
            }
            catch (Exception ex)
            { }


        }
        #endregion Add Exch Rate

        #region validate
        protected bool ValidateDate()
        {
            try
            {
                DateTime dtFrom = DateTime.ParseExact(txtDtFrm.Text, "dd/MM/yyyy", null);
                DateTime dtTo = DateTime.ParseExact(txtDtTo.Text, "dd/MM/yyyy", null);

                if (DateTime.Compare(dtFrom, dtTo) > 0)
                {
                    lblStatus.Text = "Enter Correct Dates";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Enter Dates in dd/MM/yyyy format";
                lblStatus.ForeColor = Color.Red;
                return false;
            }
            return true;
        }
        #endregion

        #region Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtDtFrm.Text = txtDtTo.Text = txtExchRt.Text = string.Empty;
            ddlCurr.SelectedIndex = ddlRtType.SelectedIndex = 0;
            btnSave.Text = "Save";
            lblStatus.Text = string.Empty;
            BindEmptyRow();
        }
        #endregion Clear

        # region exchGrd_RowCommand
        protected void exchGrd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                #region Edit
                if (e.CommandName == "Edit")
                {

                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);

                    Session["SrNum"] = ((Label)(exchGrd.Rows[RowIndex].FindControl("lblGrdId"))).Text.ToString();
                    string CurrCode = ((Label)exchGrd.Rows[RowIndex].FindControl("lblGrdCurr")).Text;
                    Label USRate = (Label)exchGrd.Rows[RowIndex].FindControl("lblGrdUSRt");
                    string IATARt = ((Label)exchGrd.Rows[RowIndex].FindControl("lblGrdCurrIRt")).Text;
                    Label USConv = (Label)exchGrd.Rows[RowIndex].FindControl("lblGrdUSConv");
                    Label INRConv = (Label)exchGrd.Rows[RowIndex].FindControl("lblGrdINConv");
                    Label BaseRt = (Label)exchGrd.Rows[RowIndex].FindControl("lblGrdCurrBaseRt");
                    string type = ((Label)exchGrd.Rows[RowIndex].FindControl("lblGrdType")).Text;
                    Label active = (Label)exchGrd.Rows[RowIndex].FindControl("lblisAct");

                    btnSave.Text = "Update";

                    ddlRtType.SelectedIndex = ddlRtType.Items.IndexOf(((ListItem)ddlRtType.Items.FindByText(type)));
                    ddlCurr.SelectedIndex = ddlCurr.Items.IndexOf(((ListItem)ddlCurr.Items.FindByValue(CurrCode)));
                    txtExchRt.Text = IATARt;
                    txtDtFrm.Text = ((Label)exchGrd.Rows[RowIndex].FindControl("lblGrdDtFrm")).Text;
                    txtDtTo.Text = ((Label)exchGrd.Rows[RowIndex].FindControl("lblGrdDtTo")).Text;

                }
                #endregion Edit
                #region Delete

                if (e.CommandName == "DeleteRecord")
                {

                    int RowIndex = Convert.ToInt32(e.CommandArgument);

                    int srno = int.Parse(((Label)(exchGrd.Rows[RowIndex].FindControl("lblGrdId"))).Text.ToString());
                    # region Delete
                    try
                    {
                        #region Prepare Parameters
                        DataSet ds = new DataSet();
                        object[] Params = new object[1];
                        int i = 0;

                        //1
                        Params.SetValue(srno, i);
                        i++;

                        #endregion Prepare Parameters

                        int ID = 0;
                        int res = objBal.DeleteExchDetail(Params);
                        if (res == 0)
                        {
                            btnClear_Click(null, null);
                            GetExchList();
                            lblStatus.Text = "Record Deleted Successfully";
                            lblStatus.ForeColor = Color.Green;
                        }

                    }

                    catch (Exception ex)
                    {

                    }
                }
                    # endregion Delete

                #endregion Delete
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }

        }
        # endregion exchGrd_RowCommand

        # region exchGrd_RowEditing
        protected void exchGrd_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion exchGrd_RowEditing

        # region exchGrd_PageIndexChanging
        protected void exchGrd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)ViewState["ds"];
            exchGrd.PageIndex = e.NewPageIndex;
            exchGrd.DataSource = ds;
            exchGrd.DataBind();
        }
        # endregion exchGrd_PageIndexChanging

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            lblStatus.Text = string.Empty;
            try
            {
                if (Session["dsExchRateMaster"] == null)
                {
                    btnList_Click(null, null);
                    BindEmptyRow();
                }
                

                dsExp = (DataSet)Session["dsExchRateMaster"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                {
                    dt = (DataTable)dsExp.Tables[0];

                    if (dt.Columns.Contains("ICRID"))
                        dt.Columns.Remove("ICRID");

                    string attachment = "attachment; filename=ExchRateMaster.xls";
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
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found for the selected search criteria!');</SCRIPT>", false);
                    return;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dsExp != null)
                    dsExp = null;
                if (dt != null)
                    dt = null;
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            #region Validate date
            try
            {
                if (txtDtFrm.Text != "" || txtDtTo.Text != "")
                {
                    DateTime dtFrom = DateTime.ParseExact(txtDtFrm.Text, "dd/MM/yyyy", null);
                    DateTime dtTo = DateTime.ParseExact(txtDtTo.Text, "dd/MM/yyyy", null);

                    if (DateTime.Compare(dtFrom, dtTo) > 0)
                    {
                        lblStatus.Text = "Enter Correct Dates";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Enter Dates in dd/MM/yyyy format";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            #endregion
            GetExchList();
        }

        //protected void ddlRtType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlRtType.SelectedItem.Text == "IATA")
        //    {
        //        ext_txtDtTo.SelectedDate = DateTime.Now.AddDays(30);
        //    }
        //    if (ddlRtType.SelectedItem.Text == "BLR")
        //    {
        //        ext_txtDtTo.SelectedDate = DateTime.Now.AddDays(5);
        //    }
        //    if (ddlRtType.SelectedItem.Text == "FDR")
        //    {
        //        ext_txtDtTo.SelectedDate = DateTime.Now.AddDays(20);
        //    }
        //   // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'></script>", false);
        //}
    }
}
