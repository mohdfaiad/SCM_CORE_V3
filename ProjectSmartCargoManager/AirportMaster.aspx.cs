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
    public partial class AirportMaster : System.Web.UI.Page
    {
        #region variable
        BALAirportMaster objBAL = new BALAirportMaster();
        //SqlDataAdapter da = new SqlDataAdapter();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet ds = new DataSet();

        #endregion variable

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GetCountryCode();
                    GetRegionCode();
                   GetTimeZoneCode();
                    GetCurrency();
                    LoadDropDown(ddlGLAccountCode);
                 

                }
            }
            catch (Exception ex)
            { }

        }
        #endregion Page_Load

        # region GetCountryCode List
        private void GetCountryCode()
        {
            try
            {
                DataSet ds = objBAL.GetCountryCodeList(ddlCountryCode.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlCountryCode.DataSource = ds;
                            ddlCountryCode.DataMember = ds.Tables[0].TableName;
                            ddlCountryCode.DataValueField = ds.Tables[0].Columns["CountryCode"].ColumnName;

                            ddlCountryCode.DataTextField = ds.Tables[0].Columns["CountryDesc"].ColumnName;
                            ddlCountryCode.DataBind();
                            ddlCountryCode.Items.Insert(0, new ListItem("Select", "Select"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetCurrencyCode List 

        # region GetRegionCode
        private void GetRegionCode()
        {
            try
            {
                DataSet ds = objBAL.GetRegionCode();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlRegion.DataSource = ds;
                            ddlRegion.DataMember = ds.Tables[0].TableName;
                            ddlRegion.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlRegion.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlRegion.DataBind();
                            ddlRegion.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetRegionCode

        //new code for timezones
        # region GetTimeZoneCode
        private void GetTimeZoneCode()
        {
            try
            {
                DataSet ds = objBAL.GetTimeZoneCode();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlTimeZone.DataSource = ds;
                            ddlTimeZone.DataMember = ds.Tables[0].TableName;
                            //ddlTimeZone.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            //ddlTimeZone.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlTimeZone.DataTextField = "TimeZoneDesc";
                            ddlTimeZone.DataValueField = "TimeZone";
                            ddlTimeZone.DataBind();
                            ddlTimeZone.Items.Insert(0,"Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetTimeZoneCode

        # region ddlCountryCode_SelectedIndexChanged
        protected void ddlCountryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;

                #region Prepare Parameters
                object[] RegioncodeInfo = new object[1];
                int i = 0;

                //0
                RegioncodeInfo.SetValue(ddlCountryCode.SelectedValue, i);

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.GetRegionCodeList(RegioncodeInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                ddlRegion.Items.Clear();
                                ddlRegion.DataSource = ds;
                                ddlRegion.DataMember = ds.Tables[0].TableName;
                                ddlRegion.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                                ddlRegion.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                                ddlRegion.DataBind();
                                ddlRegion.Items.Insert(0, "Select");
                            }
                           
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }

        }
        # endregion ddlCountryCode_SelectedIndexChanged

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                MasterAuditBAL ObjMAL = new MasterAuditBAL();
                if (txtTransitTimeHr.Text.Length == 0)
                {
                    txtTransitTimeHr.Text="00";
                }
                if (txtTransitTimeMin.Text.Length == 0)
                {
                    txtTransitTimeMin.Text = "00";
                }
                if (txtCutOffTimeHr.Text.Length == 0)
                {
                    txtCutOffTimeHr.Text = "00";
                }
                if (txtCutOffTimeMin.Text.Length == 0)
                {
                    txtCutOffTimeMin.Text = "00";
                }
                if (txtTransitTimeHr.Text.Length < 2)
                {
                    txtTransitTimeHr.Text = '0' + txtTransitTimeHr.Text;
                }
                if (txtTransitTimeMin.Text.Length < 2)
                {
                    txtTransitTimeMin.Text = '0' + txtTransitTimeMin.Text;
                }
                if (txtCutOffTimeHr.Text.Length < 2)
                {
                    txtCutOffTimeHr.Text = '0' + txtCutOffTimeHr.Text;
                }
                if (txtCutOffTimeMin.Text.Length < 2)
                {
                    txtCutOffTimeMin.Text = '0' + txtCutOffTimeMin.Text;
                }
                string TransitTime = txtTransitTimeHr.Text + ":" + txtTransitTimeMin.Text;//CtlTransitTime.Hour.ToString().PadLeft(2, '0') + ":" + CtlTransitTime.Minute.ToString().PadLeft(2, '0');
                if (txtTransitTimeHr.Text == "" && txtTransitTimeMin.Text == "")
                    TransitTime = "";
                string CutOffTime = txtCutOffTimeHr.Text + ":" + txtCutOffTimeMin.Text; //CtlCutOffTime.Hour.ToString().PadLeft(2, '0') + ":" + CtlCutOffTime.Minute.ToString().PadLeft(2, '0');
                if (txtCutOffTimeHr.Text == "" && txtCutOffTimeMin.Text == "")
                    CutOffTime = "";
                # region Save
                if (btnSave.Text == "Save")
                {
                    try
                    {
                        if (!ValidateFields())
                            return;
                        if (txtAirportCode.Text == "")
                        {
                            lblStatus.Text = "Please Enter Airport Code";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (txtAirportName.Text == "")
                        {
                            lblStatus.Text = "Please Enter Airport Name";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        if (ddlCountryCode.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please Select Country Code.";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (ddlRegion.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please select Region.";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (ddlTimeZone.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please Select timezone.";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        
                        #region Prepare Parameters
                        object[] AirportInfo = new object[46];
                        int i = 0;

                        //0
                        AirportInfo.SetValue(txtAirportCode.Text.Trim().ToUpper(), i);
                        i++;

                        //1
                        AirportInfo.SetValue(txtAirportName.Text.Trim(), i);
                        i++;

                        //2
                        AirportInfo.SetValue(ddlCountryCode.SelectedValue.ToString(), i);
                        i++;

                        //3
                        AirportInfo.SetValue(ddlRegion.SelectedItem.Text, i);
                        i++;
                       
                       
                        //4
                        AirportInfo.SetValue(txtCity.Text.Trim(), i);
                        i++;

                        //5
                        if (chkActive.Checked == true)
                        {
                            AirportInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            AirportInfo.SetValue("False", i);
                            i++;
                        }
                        
                        //6
                        AirportInfo.SetValue(txtStationEmail.Text.Trim(), i);
                        i++;

                        //7
                        AirportInfo.SetValue(txtManagerName.Text.Trim().ToUpper(), i);
                        i++;

                        //8
                        AirportInfo.SetValue(txtManagerEmail.Text.Trim(), i);
                        i++;

                        //9
                        AirportInfo.SetValue(txtShiftMobNo.Text.Trim(), i);
                        i++;

                        //10
                        AirportInfo.SetValue(txtLandlineNo.Text.Trim(), i);
                        i++;

                        //11
                        AirportInfo.SetValue(txtManagerMobNo.Text.Trim(), i);
                        i++;

                        //12
                        AirportInfo.SetValue(txtCounter.Text.Trim(), i);
                        i++;

                        //13
                        AirportInfo.SetValue(txtGHAName.Text.Trim(), i);
                        i++;

                        //14
                        AirportInfo.SetValue(txtGHAAddress.Text.Trim(), i);
                        i++;

                        //15
                        AirportInfo.SetValue(txtGHAPhone.Text.Trim(), i);
                        i++;

                        //16
                        AirportInfo.SetValue(txtGHAMobNo.Text.Trim(), i);
                        i++;

                        //17
                        AirportInfo.SetValue(txtGHAFaxNo.Text.Trim(), i);
                        i++;

                        //18
                        AirportInfo.SetValue(txtGHAEmailID.Text.Trim(), i);
                        i++;

                        //19
                        AirportInfo.SetValue(txtGSAName.Text.Trim(), i);
                        i++;

                        //20
                        AirportInfo.SetValue(txtGSAAddress.Text.Trim(), i);
                        i++;

                        //21
                        AirportInfo.SetValue(txtGSAPhone.Text.Trim(), i);
                        i++;

                        //22
                        AirportInfo.SetValue(txtGSAMobNo.Text.Trim(), i);
                        i++;

                        //23
                        AirportInfo.SetValue(txtGSAFaxNo.Text.Trim(), i);
                        i++;

                        //24
                        AirportInfo.SetValue(txtGSAEmailID.Text.Trim(), i);
                        i++;

                        //25
                        AirportInfo.SetValue(txtAPMName.Text.Trim(), i);
                        i++;

                        //26
                        AirportInfo.SetValue(txtAPMAddress.Text.Trim(), i);
                        i++;

                        //27
                        AirportInfo.SetValue(txtAPMPhone.Text.Trim(), i);
                        i++;

                        //28
                        AirportInfo.SetValue(txtAPMMobNo.Text.Trim(), i);
                        i++;

                        //29
                        AirportInfo.SetValue(txtAPMFaxNo.Text.Trim(), i);
                        i++;

                        //30
                        AirportInfo.SetValue(txtAPMEmailID.Text.Trim(), i);
                        i++;

                        //31
                        AirportInfo.SetValue(txtAdditionalInfo.Text.Trim(), i);
                        i++;

                        //32
                        AirportInfo.SetValue(TransitTime, i);
                        i++;

                        //33
                        AirportInfo.SetValue(CutOffTime, i);
                        
                        i++;
                        
                        //34
                        if (chkExempted.Checked == true)
                        {
                            AirportInfo.SetValue("True", i);
                            i++;
                            
                        }
                        else
                        {
                            AirportInfo.SetValue("False", i);
                            i++;
                            
                        }
                        //35
                        AirportInfo.SetValue(ddlBookingCurrency.SelectedValue.ToString(), i);
                        i++;

                        //36
                        AirportInfo.SetValue(ddlBookingCurrencyType.SelectedValue.ToString(), i);
                        i++;

                        //37
                        AirportInfo.SetValue(ddlInvoiceCurrency.SelectedValue.ToString(), i);
                        i++;

                        //38
                        AirportInfo.SetValue(ddlInvoiceCurrencyType.SelectedItem.Text, i);
                        i++;

                        //39
                        if(isMetro.Checked==true)
                        AirportInfo.SetValue("Metro", i);
                        else if (isMetro.Checked == false)
                            AirportInfo.SetValue("NonMetro", i);
                        i++;

                        //40
                        if (ddlGLAccountCode.SelectedIndex == 0)
                            AirportInfo.SetValue("", i);
                        else
                        AirportInfo.SetValue(ddlGLAccountCode.Text, i);
                        i++;

                        //41 for timezone
                        AirportInfo.SetValue(ddlTimeZone.SelectedItem.Value, i);
                        i++;
                     
                        //42 for UTCtimeDiff
                        string[] split = ddlTimeZone.SelectedItem.Text.Split(')');
                        string timeZone = split[0].Replace("(UTC", string.Empty);
                        AirportInfo.SetValue(timeZone.Trim(), i);
                        i++;

                        //43
                        AirportInfo.SetValue(chkIsULDEnabled.Checked, i);
                        i++;

                        //44
                        AirportInfo.SetValue(txtLatitude.Text.Trim(), i);
                        i++;

                        //45
                        AirportInfo.SetValue(txtLongitude.Text.Trim(), i);
                        i++;

                        #endregion Prepare Parameters

                        int ID = 0;
                        ID = objBAL.AddAirport(AirportInfo);
                        if (ID >= 0)
                        {

                            string value = "SAVE";
                            MasterLog(value);

                            btnList_Click(sender, e);
                            
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Airport Added Sucessfully..";
                            btnSave.Text = "Save";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Airport Insertion Failed..";
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
                        if (txtAirportCode.Text == "")
                        {
                            lblStatus.Text = "Please Enter Airport Code";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (txtAirportName.Text == "")
                        {
                            lblStatus.Text = "Please Enter Airport Name";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        if (ddlCountryCode.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please Select Country Code.";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (ddlRegion.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please select Region.";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        //for timezones
                        if (ddlTimeZone.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please Select timezone.";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        #region Prepare Parameters
                        object[] updateAirportInfo = new object[46];
                        int i = 0;

                        //0
                        updateAirportInfo.SetValue(txtAirportCode.Text.Trim().ToUpper(), i);
                        i++;
                        
                        //1
                        updateAirportInfo.SetValue(txtAirportName.Text.Trim(), i);
                        i++;

                        //2
                        updateAirportInfo.SetValue(ddlCountryCode.SelectedValue.ToString(), i);
                        i++;

                        //3
                        updateAirportInfo.SetValue(ddlRegion.SelectedItem.Text, i);
                        i++;

                        //4
                        updateAirportInfo.SetValue(txtCity.Text, i);
                        i++;

                        //5
                        if (chkActive.Checked == true)
                        {
                            updateAirportInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            updateAirportInfo.SetValue("False", i);
                            i++;
                        }

                        //6
                        updateAirportInfo.SetValue(txtStationEmail.Text.Trim(), i);
                        i++;

                        //7
                        updateAirportInfo.SetValue(txtManagerName.Text.Trim().ToUpper(), i);
                        i++;

                        //8
                        updateAirportInfo.SetValue(txtManagerEmail.Text.Trim(), i);
                        i++;

                        //9
                        updateAirportInfo.SetValue(txtShiftMobNo.Text.Trim(), i);
                        i++;

                        //10
                        updateAirportInfo.SetValue(txtLandlineNo.Text.Trim(), i);
                        i++;

                        //11
                        updateAirportInfo.SetValue(txtManagerMobNo.Text.Trim(), i);
                        i++;

                        //12
                        updateAirportInfo.SetValue(txtCounter.Text.Trim(), i);
                        i++;

                        //13
                        updateAirportInfo.SetValue(txtGHAName.Text.Trim(), i);
                        i++;

                        //14
                        updateAirportInfo.SetValue(txtGHAAddress.Text.Trim(), i);
                        i++;

                        //15
                        updateAirportInfo.SetValue(txtGHAPhone.Text.Trim(), i);
                        i++;

                        //16
                        updateAirportInfo.SetValue(txtGHAMobNo.Text.Trim(), i);
                        i++;

                        //17
                        updateAirportInfo.SetValue(txtGHAFaxNo.Text.Trim(), i);
                        i++;

                        //18
                        updateAirportInfo.SetValue(txtGHAEmailID.Text.Trim(), i);
                        i++;

                        //19
                        updateAirportInfo.SetValue(txtGSAName.Text.Trim(), i);
                        i++;

                        //20
                        updateAirportInfo.SetValue(txtGSAAddress.Text.Trim(), i);
                        i++;

                        //21
                        updateAirportInfo.SetValue(txtGSAPhone.Text.Trim(), i);
                        i++;

                        //22
                        updateAirportInfo.SetValue(txtGSAMobNo.Text.Trim(), i);
                        i++;

                        //23
                        updateAirportInfo.SetValue(txtGSAFaxNo.Text.Trim(), i);
                        i++;

                        //24
                        updateAirportInfo.SetValue(txtGSAEmailID.Text.Trim(), i);
                        i++;

                        //25
                        updateAirportInfo.SetValue(txtAPMName.Text.Trim(), i);
                        i++;

                        //26
                        updateAirportInfo.SetValue(txtAPMAddress.Text.Trim(), i);
                        i++;

                        //27
                        updateAirportInfo.SetValue(txtAPMPhone.Text.Trim(), i);
                        i++;

                        //28
                        updateAirportInfo.SetValue(txtAPMMobNo.Text.Trim(), i);
                        i++;

                        //29
                        updateAirportInfo.SetValue(txtAPMFaxNo.Text.Trim(), i);
                        i++;

                        //30
                        updateAirportInfo.SetValue(txtAPMEmailID.Text.Trim(), i);
                        i++;

                        //31
                        updateAirportInfo.SetValue(txtAdditionalInfo.Text.Trim(), i);
                        i++;

                        //32
                        updateAirportInfo.SetValue(TransitTime, i);
                        i++;

                        //33
                        updateAirportInfo.SetValue(CutOffTime, i);
                        i++;

                        //34
                        if (chkExempted.Checked == true)
                        {
                            updateAirportInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            updateAirportInfo.SetValue("False", i);
                            i++;
                        }
                        //35
                        updateAirportInfo.SetValue(ddlBookingCurrency.SelectedValue.ToString(), i);
                        i++;

                        //36
                        updateAirportInfo.SetValue(ddlBookingCurrencyType.SelectedItem.Text, i);
                        i++;

                        //37
                        updateAirportInfo.SetValue(ddlInvoiceCurrency.SelectedValue.ToString(), i);
                        i++;

                        //38
                        updateAirportInfo.SetValue(ddlInvoiceCurrencyType.SelectedItem.Text, i);
                        i++;

                        //39
                        if(isMetro.Checked==true)
                        updateAirportInfo.SetValue("Metro", i);
                        else if (isMetro.Checked == false)
                            updateAirportInfo.SetValue("NonMetro", i);
                        i++;

                        //40
                        if(ddlGLAccountCode.Items.Count==0)
                        updateAirportInfo.SetValue("", i);
                        else
                            updateAirportInfo.SetValue(ddlGLAccountCode.SelectedItem.Text, i);
                        i++;


                        //41 for timezones
                        updateAirportInfo.SetValue(ddlTimeZone.SelectedItem.Value, i);
                        i++;

                        //42 for UtcTimeDIFF
                        string[] split1 = ddlTimeZone.SelectedItem.Text.Split(')');
                        string timeZone = split1[0].Replace("(UTC", string.Empty);
                        updateAirportInfo.SetValue(timeZone.Trim(), i);
                        i++;

                        //43
                        updateAirportInfo.SetValue(chkIsULDEnabled.Checked, i);
                        i++;

                        //44
                        updateAirportInfo.SetValue(txtLatitude.Text.Trim(), i);
                        i++;

                        //45
                        updateAirportInfo.SetValue(txtLongitude.Text.Trim(), i);
                        i++;
                        
                  
                        #endregion Prepare Parameters

                        int UpdateID = 0;
                        UpdateID = objBAL.UpdateAirport(updateAirportInfo);
                        if (UpdateID >= 0)
                        {


                            string value = "UPDATE";
                            MasterLog(value);
                            btnList_Click(sender, e);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Airport Updated Sucessfully..";
                            btnSave.Text = "Save";
                            
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Airport Updation Failed..";
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                # endregion Update
            }

            catch (Exception ex)
            {

            }

        }
        #endregion btnSave_Click

        # region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                #region Prepare Parameters
                object[] AirportListInfo = new object[4];
                int i = 0;

                //0
                if (ddlCountryCode.SelectedIndex == 0)
                {
                    AirportListInfo.SetValue("", i);
                    i++;
                }
                else
                {
                    AirportListInfo.SetValue(ddlCountryCode.SelectedValue.ToString(), i);
                    i++;
                }

                //1
                if (ddlRegion.Text == "Select")
                {
                    AirportListInfo.SetValue("", i);

                }
                else
                {
                    AirportListInfo.SetValue(ddlRegion.SelectedItem.Text, i);
                }
                i++;


                //2
                 AirportListInfo.SetValue(txtAirportCode.Text.Trim(),i);
                 i++;

                 AirportListInfo.SetValue(chkActive.Checked, i);
                    i++;
               
                 
                 //if (ddlGLAccountCode.Text == "Select")
                 //{
                 //    AirportListInfo.SetValue("", i);

                 //}
                 //else
                 //{
                 //    AirportListInfo.SetValue(ddlGLAccountCode.SelectedItem.Text, i);

                 //}
                #endregion Prepare Parameters

                ds = objBAL.GetAirportList(AirportListInfo);
                
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Session["grvAirportList"] = ds.Tables[0];
                                grvAirportList.PageIndex = 0;
                                grvAirportList.DataSource = ds;
                                grvAirportList.DataMember = ds.Tables[0].TableName;
                                grvAirportList.DataBind();
                                grvAirportList.Visible = true;
                                //If only 1 record found then show data directly in the fields.
                                if (ds.Tables[0].Rows.Count == 1)
                                {
                                    ShowData(ds.Tables[0], 0);
                                }
                                //updtPnl.Update();
                                //ds.Clear();
                                btnExport.Visible = true;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion btnList_Click

        #region Show Data
        private void ShowData(DataTable dt, int RowIndex)
        {
            try
            {
                lblStatus.Text = "";
                Label lblContCode = (Label)grvAirportList.Rows[RowIndex].FindControl("lblCountryCode");
               
                Label lblReg = (Label)grvAirportList.Rows[RowIndex].FindControl("lblRegion");
                Label lblAirName = (Label)grvAirportList.Rows[RowIndex].FindControl("lblAirportName");
                Label lblAirCode = (Label)grvAirportList.Rows[RowIndex].FindControl("lblAirportCode");
                Label lblCityName = (Label)grvAirportList.Rows[RowIndex].FindControl("lblCity");
                Label lblstat = (Label)grvAirportList.Rows[RowIndex].FindControl("lblStatus");
                Label lblStationName = (Label)grvAirportList.Rows[RowIndex].FindControl("lblStation");
                Label lblManagerName = (Label)grvAirportList.Rows[RowIndex].FindControl("lblManager");
                Label lblManEmailId = (Label)grvAirportList.Rows[RowIndex].FindControl("lblManagerEmailId");
                Label lblShiftMob = (Label)grvAirportList.Rows[RowIndex].FindControl("lblShiftMobNo");
                Label lblLandline = (Label)grvAirportList.Rows[RowIndex].FindControl("lblLandlineNo");
                Label lblManagerMob = (Label)grvAirportList.Rows[RowIndex].FindControl("lblManagerMobNo");
                Label lblCounter = (Label)grvAirportList.Rows[RowIndex].FindControl("lblCounter");
                Label lblTransitTime = (Label)grvAirportList.Rows[RowIndex].FindControl("lblTransitTime");
                Label lblCutOffTime = (Label)grvAirportList.Rows[RowIndex].FindControl("lblCutOffTime");
                Label lblCitytype = (Label)grvAirportList.Rows[RowIndex].FindControl("lblCityType");
                Label lblGLAccountCode = (Label)grvAirportList.Rows[RowIndex].FindControl("lblGLAccount");
                //Label lblTimeZones = (Label)grvAirportList.Rows[RowIndex].FindControl("lblTimeZones");

                try
                {
                    try
                    {
                        ddlCountryCode.Text = lblContCode.Text;
                    }
                    catch
                    {
                        ddlCountryCode.SelectedIndex = 0;
                    }
                    try
                    {
                        ddlRegion.Text = lblReg.Text;
                    }
                    catch
                    {
                        ddlRegion.SelectedIndex = 0;
                    }
                    
                    txtAirportName.Text = lblAirName.Text;
                    txtAirportCode.Text = lblAirCode.Text;
                    txtCity.Text = lblCityName.Text;
                    txtStationEmail.Text = lblStationName.Text;
                    txtManagerName.Text = lblManagerName.Text;
                    txtManagerEmail.Text = lblManEmailId.Text;
                    txtShiftMobNo.Text = lblShiftMob.Text;
                    txtLandlineNo.Text = lblLandline.Text;
                    txtManagerMobNo.Text = lblManagerMob.Text;
                    txtCounter.Text = lblCounter.Text;
                    ddlGLAccountCode.Text = lblGLAccountCode.Text;
                // ddlTimeZone.Text = lblTimeZones.Text;
                    if (lblCitytype.Text == "Metro")
                        isMetro.Checked = true;
                }
                catch (Exception ex)
                {
                }
                try
                {

                    //string DepInt = dsRateDetails.Tables[1].Rows[8]["ParamValue"].ToString();
                    string CutOffTime = lblCutOffTime.Text;
                    string TransitTime = lblTransitTime.Text;

                    txtTransitTimeHr.Text = TransitTime.Substring(0, 2);
                    txtTransitTimeMin.Text = TransitTime.Substring(3, 2);

                    txtCutOffTimeHr.Text = CutOffTime.Substring(0, 2);
                    txtCutOffTimeMin.Text = CutOffTime.Substring(3, 2);

                    //if (Convert.ToInt16(TransitTime.Substring(0, 2)) > 11)
                    //    CtlTransitTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    //else
                    //    CtlTransitTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;

                    //CtlTransitTime.Hour = Convert.ToInt16(TransitTime.Substring(0, 2));
                    //CtlTransitTime.Minute = Convert.ToInt16(TransitTime.Substring(3, 2));

                    //if (Convert.ToInt16(CutOffTime.Substring(0, 2)) > 11)
                    //    CtlCutOffTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    //else
                    //    CtlCutOffTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;

                    //CtlCutOffTime.Hour = Convert.ToInt16(CutOffTime.Substring(0, 2));
                    //CtlCutOffTime.Minute = Convert.ToInt16(CutOffTime.Substring(3, 2));

                    //rbDepIntInclude.Checked = bool.Parse(dsRateDetails.Tables[1].Rows[8]["IsInclude"].ToString());

                    //txtTransitTime.Text = lblTransitTime.Text;
                    //txtCutOffTime.Text = lblCutOffTime.Text;
                }
                catch (Exception ex)
                { }
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (txtAirportCode.Text == dr["AirportCode"].ToString())
                        {

                            txtGHAName.Text = dr["GHAName"].ToString();
                            txtGHAAddress.Text = dr["GHAAddress"].ToString();
                            txtGHAPhone.Text = dr["GHAPhoneNo"].ToString();
                            txtGHAMobNo.Text = dr["GHAMobileNo"].ToString();
                            txtGHAFaxNo.Text = dr["GHAFAXNo"].ToString();
                            txtGHAEmailID.Text = dr["GHAEmailID"].ToString();
                            txtGSAName.Text = dr["GSAName"].ToString();
                            txtGSAAddress.Text = dr["GSAAddress"].ToString();
                            txtGSAPhone.Text = dr["GSAPhoneNo"].ToString();
                            txtGSAMobNo.Text = dr["GSAMobileNo"].ToString();
                            txtGSAFaxNo.Text = dr["GSAFAXNo"].ToString();
                            txtGSAEmailID.Text = dr["GSAEmailID"].ToString();
                            txtAPMName.Text = dr["APMName"].ToString();
                            txtAPMAddress.Text = dr["APMAddress"].ToString();
                            txtAPMPhone.Text = dr["APMPhoneNo"].ToString();
                            txtAPMMobNo.Text = dr["APMMobileNo"].ToString();
                            txtAPMFaxNo.Text = dr["APMFAXNo"].ToString();
                            txtAPMEmailID.Text = dr["APMEmailID"].ToString();
                            txtAdditionalInfo.Text = dr["AdditionalInfo"].ToString();
                            Label1.Text = dr["IsTaxExempted"].ToString();
                            txtLatitude.Text = dr["Latitude"].ToString().Trim();
                            txtLongitude.Text = dr["Longitude"].ToString().Trim();

                            if (dr["IsULDEnabled"].ToString().ToUpper().Trim() == "TRUE")
                                chkIsULDEnabled.Checked = true;
                            else
                                chkIsULDEnabled.Checked = false;

                            if (Label1.Text == "True")
                            {
                                chkExempted.Checked = true; //= dr["IsTaxExempted"].ToString();
                            }
                            else
                            {
                                chkExempted.Checked = false;
                            }

                            try
                            {
                                ddlBookingCurrency.Text = dr["BookingCurrrency"].ToString();
                            }
                            catch
                            {
                                ddlBookingCurrency.SelectedIndex = 0;
                            }
                            try
                            {
                                ddlBookingCurrencyType.Text = dr["BookingType"].ToString();
                            }
                            catch
                            {
                                ddlBookingCurrencyType.SelectedIndex = 0;
                            }
                            try
                            {
                                ddlInvoiceCurrency.Text = dr["InvoiceCurrrency"].ToString();
                            }
                            catch
                            {
                                ddlInvoiceCurrency.SelectedIndex = 0;
                            }
                            try
                            {
                               ddlInvoiceCurrencyType.Text = dr["InvoiceType"].ToString();
                                //ddlTimeZone.SelectedValue = dr["TimeZones"].ToString();
                            }
                            catch
                            {
                                ddlInvoiceCurrencyType.SelectedIndex = 0;
                            }
                            //for timeZones
                            try
                            {
                                ddlTimeZone.SelectedValue = dr["TimeZones"].ToString();
                            }
                            catch
                            {
                                ddlTimeZone.SelectedIndex = 0;
                            }
                           
                            break;
                        }
                    }
                }
                catch (Exception ex)
                { }
                try
                {

                    if (lblstat.Text == "True")
                    {
                        chkActive.Checked = true;
                    }

                    if (lblstat.Text == "False")
                    {
                        chkActive.Checked = false;
                    }
                }
                catch (Exception ex)
                {

                }
                btnSave.Text = "Update";
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Show Data

        public void MasterLog(string value)
        {
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            #region for Master Audit Log
            #region Prepare Parameters
            object[] Paramsmaster = new object[7];
            int count = 0;

            //1

            Paramsmaster.SetValue("Airport Master", count);
            count++;

            //2
            Paramsmaster.SetValue(txtAirportCode.Text+"-"+txtAirportName.Text , count);
            count++;

            //3

            Paramsmaster.SetValue(value, count);
            count++;

            //4

            Paramsmaster.SetValue("", count);
            count++;


            //5

            Paramsmaster.SetValue("", count);
            count++;

            //6

            Paramsmaster.SetValue(Session["UserName"], count);
            count++;

            //7
            Paramsmaster.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), count);
            count++;


            #endregion Prepare Parameters
            ObjMAL.AddMasterAuditLog(Paramsmaster);
            #endregion

        }

        # region grvAirportList_RowCommand
        protected void grvAirportList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Edit")
                {
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    Label lblContCode = (Label)grvAirportList.Rows[RowIndex].FindControl("lblCountryCode");
                    Label lblReg = (Label)grvAirportList.Rows[RowIndex].FindControl("lblRegion");
                    Label lblAirName = (Label)grvAirportList.Rows[RowIndex].FindControl("lblAirportName");
                    Label lblAirCode = (Label)grvAirportList.Rows[RowIndex].FindControl("lblAirportCode");
                    Label lblCityName = (Label)grvAirportList.Rows[RowIndex].FindControl("lblCity");
                    Label lblstat = (Label)grvAirportList.Rows[RowIndex].FindControl("lblStatus");
                    Label lblStationName = (Label)grvAirportList.Rows[RowIndex].FindControl("lblStation");
                    Label lblManagerName = (Label)grvAirportList.Rows[RowIndex].FindControl("lblManager");
                    Label lblManEmailId = (Label)grvAirportList.Rows[RowIndex].FindControl("lblManagerEmailId");
                    Label lblShiftMob = (Label)grvAirportList.Rows[RowIndex].FindControl("lblShiftMobNo");
                    Label lblLandline = (Label)grvAirportList.Rows[RowIndex].FindControl("lblLandlineNo");
                    Label lblManagerMob = (Label)grvAirportList.Rows[RowIndex].FindControl("lblManagerMobNo");
                    Label lblCounter = (Label)grvAirportList.Rows[RowIndex].FindControl("lblCounter");
                    Label lblTransitTime = (Label)grvAirportList.Rows[RowIndex].FindControl("lblTransitTime");
                    Label lblCutOffTime = (Label)grvAirportList.Rows[RowIndex].FindControl("lblCutOffTime");
                    Label lblCitytype = (Label)grvAirportList.Rows[RowIndex].FindControl("lblCityType");
                    //Label lblServiceTax = (Label)grvAirportList.Rows[RowIndex].FindControl("lblServiceTax");
                    Label lblGLAccountCode = (Label)grvAirportList.Rows[RowIndex].FindControl("lblGLAccount");
                   //Label lbltimezone = (Label)grvAirportList.Rows[RowIndex].FindControl("lbltimezone");
               
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["grvAirportList"];
                    try
                    {
                        try
                        {
                            ddlCountryCode.Text = lblContCode.Text;
                        }
                        catch
                        {
                            ddlCountryCode.SelectedIndex = 0;
                        }
                        try
                        {
                            ddlRegion.Text = lblReg.Text;
                        }
                        catch
                        {
                            ddlRegion.SelectedIndex = 0;
                        }
                        txtAirportName.Text = lblAirName.Text;
                        txtAirportCode.Text = lblAirCode.Text;
                        txtCity.Text = lblCityName.Text;
                        txtStationEmail.Text = lblStationName.Text;
                        txtManagerName.Text = lblManagerName.Text;
                        txtManagerEmail.Text = lblManEmailId.Text;
                        txtShiftMobNo.Text = lblShiftMob.Text;
                        txtLandlineNo.Text = lblLandline.Text;
                        txtManagerMobNo.Text = lblManagerMob.Text;
                        txtCounter.Text = lblCounter.Text;
                        ddlGLAccountCode.Text = lblGLAccountCode.Text;
                        if (lblCitytype.Text == "Metro")
                            isMetro.Checked = true;
                        
                    }
                    catch (Exception ex)
                    { }
                    try
                    {

                        //string DepInt = dsRateDetails.Tables[1].Rows[8]["ParamValue"].ToString();
                        string CutOffTime = lblCutOffTime.Text;
                        string TransitTime = lblTransitTime.Text;

                        txtTransitTimeHr.Text = TransitTime.Substring(0, 2);
                        txtTransitTimeMin.Text = TransitTime.Substring(3, 2);

                        txtCutOffTimeHr.Text = CutOffTime.Substring(0, 2);
                        txtCutOffTimeMin.Text = CutOffTime.Substring(3, 2);

                        //if (Convert.ToInt16(TransitTime.Substring(0, 2)) > 11)
                        //    CtlTransitTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        //else
                        //    CtlTransitTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;

                        //CtlTransitTime.Hour = Convert.ToInt16(TransitTime.Substring(0, 2));
                        //CtlTransitTime.Minute = Convert.ToInt16(TransitTime.Substring(3, 2));

                        //if (Convert.ToInt16(CutOffTime.Substring(0, 2)) > 11)
                        //    CtlCutOffTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        //else
                        //    CtlCutOffTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;

                        //CtlCutOffTime.Hour = Convert.ToInt16(CutOffTime.Substring(0, 2));
                        //CtlCutOffTime.Minute = Convert.ToInt16(CutOffTime.Substring(3, 2));

                        //rbDepIntInclude.Checked = bool.Parse(dsRateDetails.Tables[1].Rows[8]["IsInclude"].ToString());

                        //txtTransitTime.Text = lblTransitTime.Text;
                        //txtCutOffTime.Text = lblCutOffTime.Text;
                    }
                    catch (Exception ex)
                    { }
                    try
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (txtAirportCode.Text == dr["AirportCode"].ToString())
                            {

                                txtGHAName.Text = dr["GHAName"].ToString();
                                txtGHAAddress.Text = dr["GHAAddress"].ToString();
                                txtGHAPhone.Text = dr["GHAPhoneNo"].ToString();
                                txtGHAMobNo.Text = dr["GHAMobileNo"].ToString();
                                txtGHAFaxNo.Text = dr["GHAFAXNo"].ToString();
                                txtGHAEmailID.Text = dr["GHAEmailID"].ToString();
                                txtGSAName.Text = dr["GSAName"].ToString();
                                txtGSAAddress.Text = dr["GSAAddress"].ToString();
                                txtGSAPhone.Text = dr["GSAPhoneNo"].ToString();
                                txtGSAMobNo.Text = dr["GSAMobileNo"].ToString();
                                txtGSAFaxNo.Text = dr["GSAFAXNo"].ToString();
                                txtGSAEmailID.Text = dr["GSAEmailID"].ToString();
                                txtAPMName.Text = dr["APMName"].ToString();
                                txtAPMAddress.Text = dr["APMAddress"].ToString();
                                txtAPMPhone.Text = dr["APMPhoneNo"].ToString();
                                txtAPMMobNo.Text = dr["APMMobileNo"].ToString();
                                txtAPMFaxNo.Text = dr["APMFAXNo"].ToString();
                                txtAPMEmailID.Text = dr["APMEmailID"].ToString();
                                txtAdditionalInfo.Text = dr["AdditionalInfo"].ToString();
                                Label1.Text = dr["IsTaxExempted"].ToString();
                                txtLatitude.Text = dr["Latitude"].ToString().Trim();
                                txtLongitude.Text = dr["Longitude"].ToString().Trim();

                                if (dr["IsULDEnabled"].ToString().ToUpper().Trim() == "TRUE")
                                    chkIsULDEnabled.Checked = true;
                                else
                                    chkIsULDEnabled.Checked = false;

                                if (Label1.Text == "True")
                                {
                                    chkExempted.Checked = true; //= dr["IsTaxExempted"].ToString();
                                }
                                else
                                {
                                    chkExempted.Checked = false;
                                }

                                try
                                {
                                    ddlBookingCurrency.Text = dr["BookingCurrrency"].ToString();
                                }
                                catch
                                {
                                    ddlBookingCurrency.SelectedIndex = 0;
                                }
                                try
                                {
                                    ddlBookingCurrencyType.Text = dr["BookingType"].ToString();
                                }
                                catch
                                {
                                    ddlBookingCurrencyType.SelectedIndex = 0;
                                }
                                try
                                {
                                    ddlInvoiceCurrency.Text = dr["InvoiceCurrrency"].ToString();
                                }
                                catch
                                {
                                    ddlInvoiceCurrency.SelectedIndex = 0;
                                }
                                try
                                {
                                    ddlInvoiceCurrencyType.Text = dr["InvoiceType"].ToString();
                                    ddlTimeZone.SelectedValue = dr["TimeZones"].ToString();
                                }
                                catch
                                {
                                    ddlInvoiceCurrencyType.SelectedIndex = 0;
                                }

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                    try
                    {

                        if (lblstat.Text == "True")
                        {
                            chkActive.Checked = true;
                        }

                        if (lblstat.Text == "False")
                        {
                            chkActive.Checked = false;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    btnSave.Text = "Update";

                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion grvAirportList_RowCommand

        # region grvAirportList_RowEditing
        protected void grvAirportList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grvAirportList_RowEditing

        # region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            txtAirportCode.Text = "";
            txtAirportName.Text = "";
            txtCity.Text = "";
            isMetro.Checked = false;
            ddlCountryCode.SelectedIndex = 0;
            ddlRegion.SelectedIndex = 0;
            chkActive.Checked = false;
            txtStationEmail.Text = "";
            txtManagerName.Text = "";
            txtManagerEmail.Text = "";
            txtShiftMobNo.Text = "";
            txtLandlineNo.Text = "";
            txtManagerMobNo.Text = "";
            txtCounter.Text = "";
            btnSave.Text = "Save";
            txtGHAName.Text = "";
            txtGHAAddress.Text = "";
            txtGHAPhone.Text = "";
            txtGHAMobNo.Text = "";
            txtGHAFaxNo.Text = "";
            txtGHAEmailID.Text = "";
            txtGSAName.Text = "";
            txtGSAAddress.Text = "";
            txtGSAPhone.Text = "";
            txtGSAMobNo.Text = "";
            txtGSAEmailID.Text = "";
            txtGSAFaxNo.Text = "";
            txtAPMAddress.Text = "";
            txtAPMName.Text = "";
            txtAPMPhone.Text = "";
            txtAPMMobNo.Text = "";
            txtAPMFaxNo.Text = "";
            txtAPMEmailID.Text = "";
            txtAdditionalInfo.Text = "";
            txtCutOffTimeHr.Text = "";
            txtTransitTimeHr.Text = "";
            txtCutOffTimeMin.Text = "";
            txtTransitTimeMin.Text = "";
            grvAirportList.DataSource = null;
            grvAirportList.DataBind();
            ddlGLAccountCode.SelectedIndex = 0;
            btnExport.Visible = false;
            txtCutOffTimeHr.Text = txtCutOffTimeMin.Text = string.Empty;
            txtTransitTimeHr.Text = txtTransitTimeMin.Text = string.Empty;
            chkExempted.Checked = false;

            ddlBookingCurrency.SelectedIndex = ddlBookingCurrencyType.SelectedIndex = 0;
            ddlInvoiceCurrency.SelectedIndex = ddlInvoiceCurrencyType.SelectedIndex = 0;

            ddlTimeZone.SelectedIndex = 0;

            txtLatitude.Text = string.Empty;
            txtLongitude.Text = string.Empty;
            chkIsULDEnabled.Checked = false;
        }
        # endregion btnClear_Click

        # region grvAirportList_PageIndexChanging
        protected void grvAirportList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                #region Prepare Parameters
                object[] AirportListInfo = new object[3];
                int i = 0;

                if (ddlCountryCode.Text == "Select")
                {
                    AirportListInfo.SetValue("", i);
                    i++;
                }
                else
                {
                    AirportListInfo.SetValue(ddlCountryCode.SelectedItem.Text, i);
                    i++;
                }

                //1
                if (ddlRegion.Text == "Select")
                {
                    AirportListInfo.SetValue("", i);

                }
                else
                {
                    AirportListInfo.SetValue(ddlRegion.SelectedItem.Text, i);
                }
                i++;
                AirportListInfo.SetValue(txtAirportCode.Text.Trim(), i);


                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.GetAirportList(AirportListInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grvAirportList.PageIndex = 0;
                                grvAirportList.DataSource = ds;
                                grvAirportList.DataMember = ds.Tables[0].TableName;
                                grvAirportList.DataBind();
                                grvAirportList.Visible = true;
                               // ds.Clear();

                            }
                        }
                    }
                }


                grvAirportList.PageIndex = e.NewPageIndex;
                grvAirportList.DataSource = ds.Copy();
                grvAirportList.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
        # endregion grvAirportList_PageIndexChanging

        protected void txtGSAPhone_TextChanged(object sender, EventArgs e)
        {

        }

        public void GetCurrency()
        {
            try
            {

                DataSet dsResult = new DataSet();

                dsResult = da.SelectRecords("spGetCurrenyCode"); //da.SelectRecords("SP_GetBookingCurrency");
                
                DropDownList ddl = ((DropDownList)AirportMasterTab.FindControl("ddlBookingCurrency"));
                DropDownList ddl1 = ((DropDownList)AirportMasterTab.FindControl("ddlInvoiceCurrency"));
                
                
                
                ddlBookingCurrency.Items.Add("All");
                ddlBookingCurrency.DataSource = dsResult;
                ddlBookingCurrency.DataMember = dsResult.Tables[0].TableName;
                ddlBookingCurrency.DataValueField = "Code";  //dsResult.Tables[0].Columns[1].ColumnName;
                ddlBookingCurrency.DataTextField = "CurrencyCode"; //dsResult.Tables[0].Columns[1].ColumnName;
                ddlBookingCurrency.DataBind();
                ddlBookingCurrency.SelectedIndex = 0;

                ddlInvoiceCurrency.Items.Add("All");
                ddlInvoiceCurrency.DataSource = dsResult;
                ddlInvoiceCurrency.DataMember = dsResult.Tables[0].TableName;
                ddlInvoiceCurrency.DataValueField = "Code"; //dsResult.Tables[0].Columns[1].ColumnName;
                ddlInvoiceCurrency.DataTextField = "CurrencyCode"; //dsResult.Tables[0].Columns[1].ColumnName;
                ddlInvoiceCurrency.DataBind();
                ddlInvoiceCurrency.SelectedIndex = 0;

                DataSet dsResult1 = new DataSet();
                dsResult1 = da.SelectRecords("SP_GetBookingCurrencyType");
                DropDownList ddl2 = ((DropDownList)AirportMasterTab.FindControl("ddlBookingCurrencyType"));
                DropDownList ddl3 = ((DropDownList)AirportMasterTab.FindControl("ddlInvoiceCurrencyType"));

                ddlBookingCurrencyType.Items.Add("All");
                ddlBookingCurrencyType.DataSource = dsResult1;
                ddlBookingCurrencyType.DataMember = dsResult1.Tables[0].TableName;
                ddlBookingCurrencyType.DataValueField = dsResult1.Tables[0].Columns[1].ColumnName;
                ddlBookingCurrencyType.DataTextField = dsResult1.Tables[0].Columns[1].ColumnName;
                ddlBookingCurrencyType.DataBind();
                ddlBookingCurrencyType.SelectedIndex = 0;

                ddlInvoiceCurrencyType.Items.Add("All");
                ddlInvoiceCurrencyType.DataSource = dsResult1;
                ddlInvoiceCurrencyType.DataMember = dsResult1.Tables[0].TableName;
                ddlInvoiceCurrencyType.DataValueField = dsResult1.Tables[0].Columns[1].ColumnName;
                ddlInvoiceCurrencyType.DataTextField = dsResult1.Tables[0].Columns[1].ColumnName;
                ddlInvoiceCurrencyType.DataBind();
                ddlInvoiceCurrencyType.SelectedIndex = 0;
            }
            catch (Exception ex)
            { }
        }

        #region newMethod
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetStation(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'

            SqlDataAdapter dad = new SqlDataAdapter("SELECT AirportName  +'   ('+ AirportCode +')' as AirportCode from AirportMaster where AirportName like '" + prefixText + "%' or AirportCode like '" + prefixText + "%'", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }

        #endregion

        public bool ValidateFields()
        {
            try
            {
                bool IsDuplicate = false;
                string errormessage = "";


                object[] values = { txtAirportCode.Text
                                  };

                if (objBAL.CheckDuplicate(values, ref IsDuplicate, ref errormessage))
                {
                    if (IsDuplicate)
                    {
                        lblStatus.Text = "Airport Code already exists.";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                }
                else
                {
                    lblStatus.Text = "" + errormessage;
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region Load DropDowns
        public void LoadDropDown(DropDownList ddlGLAccount)
        {
            try
            {
                DataSet ds = da.SelectRecords("SP_GetGLAccountCode");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlGLAccount.DataSource = ds;
                            ddlGLAccountCode.DataTextField = "GLAccountCode";
                            ddlGLAccountCode.DataValueField = "GLAccountCode";
                            ddlGLAccount.DataBind();
                            ddlGLAccount.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        
        #region Button Export
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["grvAirportList"] != null)
                {

                    DataTable dt = (DataTable)Session["grvAirportList"];
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            ExportToExcel(dt, "AirportMasterReport.xls");
                            
                        }
                    }
                }



            }
            catch (Exception ex)
            { }

        }
        #endregion

        // Coded By : Deepak Walmiki  
        #region Export to DataTable
        public void ExportToExcel(DataTable dt, string FileName)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    string filename = FileName;
                    System.IO.StringWriter tw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                    DataGrid dgGrid = new DataGrid();
                    dgGrid.DataSource = dt;
                    dgGrid.DataBind();

                    //Get the HTML for the control.
                    dgGrid.RenderControl(hw);
                    //Write the HTML back to the browser.
                    //Response.ContentType = application/vnd.ms-excel;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                    this.EnableViewState = false;
                    Response.Write(tw.ToString());
                    Response.End();
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion
       
    }
}
