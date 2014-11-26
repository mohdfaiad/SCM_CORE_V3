using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using clsDataLib;
using System.Text.RegularExpressions;
using System.Drawing;
using BAL;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class frmDemCharges : System.Web.UI.Page
    {
        // in palace of exception , make a grid with airline and agent and one text box and include exclude
        clsFillCombo cfc = new clsFillCombo();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                
                if (!IsPostBack)
                {
                    //Load currency values in currency dropdown.
                    FillCurrencyCodes(ddlCurr, "");

                    showhidebtn(true);
                    int querCnt = Request.QueryString.Count;
                    if (querCnt == 0)
                    {
                        forNewCharge();
                    }
                    else
                    {
                        if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() == "Edit")
                        {
                            if (Request.QueryString["ChargeCode"] != null &&
                                Request.QueryString["ChargeCode"].ToString() != "")
                            {
                                getValuesForEdit(Request.QueryString["ChargeCode"].ToString());
                            }
                            else
                            {
                                forNewCharge();
                            }
                            //getAllDemCharge();
                            //chargeList.Visible = true;
                            //chargeNew.Visible = false;
                            //showhidebtn(false);
                            
                        }
                        else
                        {
                            forNewCharge();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region Fill Currency
        private void FillCurrencyCodes(DropDownList drp, string SelectedCurrency)
        {
            try
            {
                BALCurrency BalCur = new BALCurrency();
                DataSet dsCur = BalCur.GetCurrencyCodeList("");
                if (dsCur != null && dsCur.Tables.Count > 0)
                {
                    if (dsCur.Tables[0].Rows.Count > 0)
                    {
                        drp.Items.Clear();                        
                        try
                        {
                            drp.DataSource = dsCur.Tables[0];
                            drp.DataTextField = "Code";
                            drp.DataValueField = "ID";
                            drp.DataBind();
                        }
                        catch (Exception ex) 
                        { }
                    }
                    else
                    {
                        drp.Items.Clear();
                        drp.SelectedIndex = 0;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        private void showhidebtn(bool res)
        {
            btnClear.Visible = res;
            //btnList.Visible = res;
            btnSave.Visible = res;
        }
        private void forNewCharge()
        {
            #region for new
            chargeList.Visible = false;
            chargeNew.Visible = true;
            txtChrCode.Enabled = true;

            if (ddllevel.SelectedIndex == 0)
                cfc.FillAllComboBoxes("tblCountryMaster", "Select", ddlStation);
            else
                cfc.FillAllComboBoxes("tblWarehouseMaster", "Select", ddlStation);

            #region Dem for Stuffs
            fillstuff();
            #endregion
            #endregion
        }

        #region getValuesForEdit
        public void getValuesForEdit(string ChargeCode)
        {

            try
            {
                lblStatus.Text = "";
                chargeNew.Visible = false;
                txtChrCode.Enabled = true;

                string[] paramnameRB = new string[1];
                paramnameRB[0] = "ChargeName";

                object[] paramvalueRB = new object[1];
                paramvalueRB[0] = ChargeCode.Trim();

                SqlDbType[] paramtypeRB = new SqlDbType[1];
                paramtypeRB[0] = SqlDbType.NVarChar;

                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet ds = db.SelectRecords("spgetDemChargeforEdit", paramnameRB, paramvalueRB, paramtypeRB);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        
                        chargeNew.Visible = true;
                        txtChrCode.Enabled = false;
                        txtChrCode.Text = ds.Tables[0].Rows[0][0].ToString();
                        ddllevel.SelectedIndex = ddllevel.Items.IndexOf(new ListItem(ds.Tables[0].Rows[0][2].ToString()));
                        ddllevel_SelectedIndexChanged(this, new EventArgs());
                        
                        for (int i = 0; i < ddlStation.Items.Count; i++)
                        {
                            if (ddlStation.Items[i].Text == ds.Tables[0].Rows[0][3].ToString())
                            {
                                ddlStation.SelectedIndex = i;
                            }
                        }
                        string num = Regex.Replace(ds.Tables[0].Rows[0][4].ToString(), @"\D", "");
                        string wor = Regex.Replace(ds.Tables[0].Rows[0][4].ToString(), @"\d", "");
                        txtGracePrd.Text = num;
                        ddlGraceWay.SelectedIndex = ddlGraceWay.Items.IndexOf(new ListItem(wor));

                        string[] day = ds.Tables[0].Rows[0][1].ToString().Split(',');
                        for (int i = 0; i < day.Count(); i++)
                        {
                            for (int j = 0; j < chkDays.Items.Count; j++)
                            {
                                if (day[i].ToString() == chkDays.Items[j].Text.ToString())
                                {
                                    chkDays.Items[j].Selected = true;
                                    break;
                                }
                            }
                        }
                        //Set currency in the dropdown.
                        if (ddlCurr.Items.FindByValue(ds.Tables[0].Rows[0]["Currency"].ToString()) != null)
                        {
                            ddlCurr.SelectedValue = ds.Tables[0].Rows[0]["Currency"].ToString();
                        }
                    }
                    gdMissingChrg.DataSource = ds.Tables[1];
                    gdMissingChrg.DataBind();

                    DataTable dtExc = new DataTable();
                    dtExc.Columns.Add("ExcType");
                    dtExc.Columns.Add("ExcCode");
                    dtExc.Rows.Add("Airline", "");
                    dtExc.Rows.Add("Agent", "");
                    dtExc.Rows.Add("ULDType", "");
                    gdException.DataSource = dtExc;
                    gdException.DataBind();

                    chargeList.Visible = false;
                    chargeNew.Visible = true;
                    showhidebtn(true);
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion getValuesForEdit

        public void getValuesForEdit(object sender, EventArgs e)
        {

            try
            {
                lblStatus.Text = "";
                chargeNew.Visible = false;
                txtChrCode.Enabled = true;

                LinkButton btn = (LinkButton)sender;

                //Get the row that contains this button
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                //Get rowindex
                int rowindex = gvr.RowIndex;

                string ChargeName = ((LinkButton)GVDemChargeList.Rows[rowindex].FindControl("HLChargeCode")).Text;

                string[] paramnameRB = new string[1];
                paramnameRB[0] = "ChargeName";

                object[] paramvalueRB = new object[1];
                paramvalueRB[0] = ChargeName.Trim(); 

                SqlDbType[] paramtypeRB = new SqlDbType[1];
                paramtypeRB[0] = SqlDbType.NVarChar;

                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet ds = db.SelectRecords("spgetDemChargeforEdit",paramnameRB,paramvalueRB,paramtypeRB);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        chargeNew.Visible = true;
                        txtChrCode.Enabled = false;
                        txtChrCode.Text = ds.Tables[0].Rows[0][0].ToString();
                        ddllevel.SelectedIndex = ddllevel.Items.IndexOf( new ListItem( ds.Tables[0].Rows[0][2].ToString()));
                        ddllevel_SelectedIndexChanged( sender,  e);
                      //  ddlStation.Text = ds.Tables[0].Rows[0][3].ToString();
                        //ddlStation.SelectedIndex = ddlStation.Items.IndexOf(new ListItem(ds.Tables[0].Rows[0][3].ToString()));

                        for (int i = 0; i < ddlStation.Items.Count; i++)
                        {
                            if (ddlStation.Items[i].Text == ds.Tables[0].Rows[0][3].ToString())
                            {
                                ddlStation.SelectedIndex = i;
                            }
                        }
                        string num = Regex.Replace(ds.Tables[0].Rows[0][4].ToString(), @"\D", "");
                        string wor = Regex.Replace(ds.Tables[0].Rows[0][4].ToString(), @"\d", "");
                        txtGracePrd.Text = num;
                        ddlGraceWay.SelectedIndex = ddlGraceWay.Items.IndexOf(new ListItem(wor));

                        string[] day= ds.Tables[0].Rows[0][1].ToString().Split(',');
                        for (int i = 0; i < day.Count(); i++)
                        {
                            for (int j = 0; j < chkDays.Items.Count; j++)
                            {
                                if (day[i].ToString() == chkDays.Items[j].Text.ToString())
                                {
                                    chkDays.Items[j].Selected = true;
                                    break;
                                }
                            }
                        }
                    }
                    gdMissingChrg.DataSource = ds.Tables[1];
                    gdMissingChrg.DataBind();

                    DataTable dtExc = new DataTable();
                    dtExc.Columns.Add("ExcType");
                    dtExc.Columns.Add("ExcCode");
                    dtExc.Rows.Add("Airline", "");
                    dtExc.Rows.Add("Agent", "");
                    dtExc.Rows.Add("ULDType", "");
                    gdException.DataSource = dtExc;
                    gdException.DataBind();

                    chargeList.Visible = false;
                    chargeNew.Visible = true;
                    showhidebtn(true);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void getAllDemCharge()
        {
            try
            {
                btnClear.Visible = false;
                btnSave.Visible = false;
                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet ds = db.SelectRecords("spgetAllDemCharge");
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GVDemChargeList.DataSource = ds.Tables[0];
                        GVDemChargeList.DataBind();
                        btnClear.Visible = true;
                        btnSave.Visible = true;
                    }
                    else
                    {
                        GVDemChargeList.DataSource = null;
                        GVDemChargeList.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void fillstuff()
        {
            DataTable dtExc = new DataTable();
            dtExc.Columns.Add("ExcType");
            dtExc.Columns.Add("ExcCode");
            dtExc.Rows.Add("Airline", "");
            dtExc.Rows.Add("Agent", "");
            dtExc.Rows.Add("ULDType", "");
            gdException.DataSource = dtExc;
            gdException.DataBind();

            DataTable dtStuff = new DataTable();
            dtStuff.Columns.Add("Type");
            dtStuff.Columns.Add("DailyDem");
            dtStuff.Columns.Add("Missing");

            dtStuff.Rows.Add("ULD", "0", "0");
            dtStuff.Rows.Add("Doors","0","0");
            dtStuff.Rows.Add("Fittings","0","0");
            dtStuff.Rows.Add("Nets","0","0");
            dtStuff.Rows.Add("Straps","0","0");
            gdMissingChrg.DataSource = dtStuff;
            gdMissingChrg.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                string savMode = "New";
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["type"].ToString() == "Edit")
                    {
                        savMode = "Edit";
                    }
                }
                string days = string.Empty;
                for (int i = 0; i < chkDays.Items.Count; i++)
                {
                    if (chkDays.Items[i].Selected == true)
                    {
                        if (days == string.Empty)
                            days = chkDays.Items[i].Text.ToString();
                        else
                            days = days + "," + chkDays.Items[i].Text.ToString();
                    }
                }
                string[] paramnameRB = new string[7];
                paramnameRB[0] = "ChargeCode";
                paramnameRB[1] = "ApplicableOn";
                paramnameRB[2] = "Level";
                paramnameRB[3] = "Location";
                paramnameRB[4] = "GracePrd";
                paramnameRB[5] = "ChargeMode";
                paramnameRB[6] = "Currency";

                object[] paramvalueRB = new object[7];
                paramvalueRB[0] = txtChrCode.Text.Trim().ToUpper();
                paramvalueRB[1] = days;
                paramvalueRB[2] = ddllevel.SelectedItem.Text.Trim();
                paramvalueRB[3] = ddlStation.SelectedItem.Text.ToString();
                paramvalueRB[4] = txtGracePrd.Text.Trim() + ddlGraceWay.SelectedItem.Text.Trim();
                paramvalueRB[5] = savMode;
                paramvalueRB[6] = ddlCurr.SelectedValue;

                SqlDbType[] paramtypeRB = new SqlDbType[7];
                paramtypeRB[0] = SqlDbType.NVarChar;
                paramtypeRB[1] = SqlDbType.NVarChar;
                paramtypeRB[2] = SqlDbType.NVarChar;
                paramtypeRB[3] = SqlDbType.NVarChar;
                paramtypeRB[4] = SqlDbType.NVarChar;
                paramtypeRB[5] = SqlDbType.NVarChar;
                paramtypeRB[6] = SqlDbType.NVarChar;

                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet ds = db.SelectRecords("spInsertDemCharge", paramnameRB, paramvalueRB, paramtypeRB);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            lblStatus.Text = "Charge code already defined";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        else if (ds.Tables[0].Rows[0][0].ToString() == "2")
                        {
                            lblStatus.Text = "Charge already defined for selected location";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                        {
                            lblStatus.Text = "Error while saving demurrage charge";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        string getid = ds.Tables[1].Rows[0][0].ToString();
                        for (int i = 0; i < gdMissingChrg.Rows.Count; i++)
                        {
                            string type = ((Label)gdMissingChrg.Rows[i].FindControl("lblType")).Text.Trim();
                            decimal daily=0;
                            decimal missing=0;
                            if (((TextBox)gdMissingChrg.Rows[i].FindControl("txtDaily")).Text.Trim() == "")
                            {
                                daily=0;
                            }
                            else
                            {
                                daily = Convert.ToDecimal(((TextBox)gdMissingChrg.Rows[i].FindControl("txtDaily")).Text.Trim());
                            }
                            if (((TextBox)gdMissingChrg.Rows[i].FindControl("txtDamage")).Text.Trim() == "")
                            {
                                missing = 0;
                            }
                            else
                            {
                                missing = Convert.ToDecimal(((TextBox)gdMissingChrg.Rows[i].FindControl("txtDamage")).Text.Trim());
                            }

                            string[] paramnameRBNew = new string[5];
                            paramnameRBNew[0] = "chargeID";
                            paramnameRBNew[1] = "type";
                            paramnameRBNew[2] = "daily";
                            paramnameRBNew[3] = "missing";
                            paramnameRBNew[4] = "ChargeMode";

                            object[] paramvalueRBNew = new object[5];
                            paramvalueRBNew[0] = getid;
                            paramvalueRBNew[1] = type;
                            paramvalueRBNew[2] = daily;
                            paramvalueRBNew[3] = missing;
                            paramvalueRBNew[4] = savMode;

                            SqlDbType[] paramtypeRBNew = new SqlDbType[5];
                            paramtypeRBNew[0] = SqlDbType.BigInt;
                            paramtypeRBNew[1] = SqlDbType.NVarChar;
                            paramtypeRBNew[2] = SqlDbType.Real;
                            paramtypeRBNew[3] = SqlDbType.Real;
                            paramtypeRBNew[4] = SqlDbType.NVarChar;

                            SQLServer dbn = new SQLServer(Global.GetConnectionString());
                            bool res = dbn.InsertData("spInsertDemChargeStuff", paramnameRBNew, paramtypeRBNew, paramvalueRBNew);
                            if (res == true)
                            {
                                if (i == (gdMissingChrg.Rows.Count - 1))
                                {
                                    //btnClear_Click( sender,  e);
                                    //forNewCharge();
                                    getValuesForEdit(txtChrCode.Text.Trim().ToUpper());
                                    lblStatus.Text = "Details saved successfully !";
                                    lblStatus.ForeColor = Color.Green;
                                }
                            }
                            else { 
                            
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        protected void ddllevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddllevel.SelectedIndex == 0)
                cfc.FillAllComboBoxes("tblCountryMaster", "Select", ddlStation);
            else
                cfc.FillAllComboBoxes("tblWarehouseMaster", "Select", ddlStation);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
          //  rblType.SelectedIndex = 0;
            try
            {
                lblStatus.Text = "";
                 int querCnt = Request.QueryString.Count;
                 if (querCnt > 0)
                 {
                     if (Request.QueryString["type"].ToString() == "Edit")
                     {
                         Response.Redirect("~/frmDemCharges.aspx?type=Edit", false);
                         return;
                     }
                 }
                
                gdMissingChrg.DataSource = null;
                gdMissingChrg.DataBind();
                ddlStation.SelectedIndex = 0;
                txtChrCode.Text = "";
                ddllevel.SelectedIndex = 0;
                txtGracePrd.Text = "";
                ddlGraceWay.SelectedIndex = 0;

                for (int i = 0; i < chkDays.Items.Count; i++)
                {
                    chkDays.Items[i].Selected = false;
                }
                fillstuff();
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {

            try
            {
                lblStatus.Text = "";
                gdMissingChrg.DataSource = null;
                gdMissingChrg.DataBind();
                gdException.DataSource = null;
                gdException.DataBind();
                ddlStation.SelectedIndex = 0;
                txtChrCode.Text = "";
                ddllevel.SelectedIndex = 0;
                txtGracePrd.Text = "";
                ddlGraceWay.SelectedIndex = 0;

                for (int i = 0; i < chkDays.Items.Count; i++)
                {
                    chkDays.Items[i].Selected = false;
                }
                fillstuff();
                chargeList.Visible = true;
                chargeNew.Visible = false;
                showhidebtn(false);

            }
            catch (Exception ex)
            {
            }
        }

      
    }
}
