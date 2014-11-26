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

namespace ProjectSmartCargoManager
{
    public partial class AirCraftTonnageMaster : System.Web.UI.Page
    {
        AircraftTonnageMasterBAL objBAL = new AircraftTonnageMasterBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetOriginCode();
                GetDestCode();
                GetAircraftType();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            
            //txtAircraftType.Text = "";
            //txtDestination.Text = "";
            //txtOrigin.Text = "";
            lblStatus.Text = "";
            txtvalidfrom.Enabled = true;
            txtvalidto.Enabled = true;
            ddlAircraftType.Enabled = true;
            txtvalidfrom.Text = "";
            txtvalidto.Text = "";
            txtVolume.Text = "";
            txtWeight.Text = "";
            chkActive.Checked = false;
            ddlVolMeasure.SelectedIndex = 0;
            ddlDest.SelectedIndex = 0;
            ddlOrigin.SelectedIndex = 0;
            ddlAircraftType.SelectedIndex = 0;
            grvArcraftTonnageList.DataSource = null;
            grvArcraftTonnageList.DataBind();
            btnSave.Text = "Save";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                # region Save
                if (btnSave.Text == "Save")
                {
                    try
                    {
                        if (ddlOrigin.SelectedItem.Text == ddlDest.SelectedItem.Text)
                        {
                            lblStatus.Text = "Origin Destination are should not Same.";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (ddlAircraftType.SelectedIndex==0)
                        {
                            lblStatus.Text = "Please Enter Aircraft Type Code";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        if (ddlDest.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please Enter Destination";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (ddlOrigin.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please Enter Origin";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        
                        if (txtWeight.Text == "")
                        {
                            lblStatus.Text = "Please Enter Weight";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (txtVolume.Text == "")
                        {
                            lblStatus.Text = "Please Enter Volume";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (txtvalidfrom.Text == "")
                        {
                            lblStatus.Text = "Please Enter Valid From Date";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (txtvalidto.Text == "")
                        {
                            lblStatus.Text = "Please Enter Valid TO Date";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        

                        #region Prepare Parameters
                        object[] CountrycodeInfo = new object[3];
                        int j = 0;

                        //0
                        CountrycodeInfo.SetValue(ddlAircraftType.SelectedItem.Text.Trim(), j);
                        j++;

                        //1
                        CountrycodeInfo.SetValue(txtvalidfrom.Text.Trim(), j);
                        j++;

                        //2
                        CountrycodeInfo.SetValue(txtvalidto.Text.Trim(), j);
                        j++;


                        #endregion Prepare Parameters

                        DataSet ds = new DataSet();
                        ds = objBAL.chkAircraftTonnageList(CountrycodeInfo);


                        if (ds != null)
                        {
                            if (ds.Tables != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Aircraft Tonnage Already Exist ";
                                }
                                else
                                {

                                    #region Prepare Parameters
                                    object[] countryInfo = new object[9];
                                    int i = 0;
                                    double vollen;

                                    //0
                                    countryInfo.SetValue(ddlAircraftType.SelectedItem.Text.Trim(), i);
                                    i++;

                                    //1
                                    countryInfo.SetValue(ddlOrigin.SelectedItem.Text.Trim(), i);
                                    i++;

                                    //2
                                    countryInfo.SetValue(ddlDest.SelectedItem.Text.Trim(), i);
                                    i++;

                                    //3
                                    countryInfo.SetValue(txtvalidfrom.Text.Trim(), i);
                                    i++;

                                    //4
                                    countryInfo.SetValue(txtvalidto.Text.Trim(), i);
                                    i++;

                                    //5
                                    //countryInfo.SetValue(txtVolume.Text.Trim(), i);

                                    //i++;

                                    countryInfo.SetValue(txtWeight.Text.Trim(), i);
                                    i++;

                                    //6
                                    
                                    if (ddlVolMeasure.SelectedItem.Text == "Inch")
                                    {
                                        vollen = (Convert.ToDouble(txtVolume.Text) / (0.061024));


                                        countryInfo.SetValue(vollen, i);
                                        i++;
                                        //AcInfo.SetValue(volbre, i);
                                        //i++;
                                        //AcInfo.SetValue(volhei, i);
                                    }
                                    else
                                    {

                                        countryInfo.SetValue(txtVolume.Text, i);
                                        i++;

                                    }
                                    //7
                                    if (chkActive.Checked == true)
                                    {
                                        countryInfo.SetValue("True", i);
                                        i++;
                                    }
                                    else
                                    {
                                        countryInfo.SetValue("False", i);
                                        i++;
                                    }
                                    countryInfo.SetValue(ddlVolMeasure.SelectedItem.Text, i);
                                    

                                    #endregion Prepare Parameters

                                    int ID = 0;
                                    ID = objBAL.AddAircraftTonnageRecords(countryInfo);
                                    if (ID >= 0)
                                    {
                                        btnClear_Click(null, null);
                                        btnList_Click(null, null);
                                        lblStatus.ForeColor = Color.Green;
                                        lblStatus.Text = "Aircraft Tonnage Added Sucessfully..";
                                        btnSave.Text = "Save";
                                    }
                                    else
                                    {
                                        lblStatus.ForeColor = Color.Red;
                                        lblStatus.Text = "Aircraft Tonnage Insertion Failed..";
                                    }

                                }

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
                        if (ddlOrigin.SelectedItem.Text == ddlDest.SelectedItem.Text)
                        {
                            lblStatus.Text = "Origin Destination are should not Same.";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (ddlAircraftType.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please Enter Aircraft Type Code";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        if (ddlDest.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please Enter Destination";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (ddlOrigin.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please Enter Origin";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }


                        if (txtWeight.Text == "")
                        {
                            lblStatus.Text = "Please Enter Weight";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (txtVolume.Text == "")
                        {
                            lblStatus.Text = "Please Enter Volume";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (txtvalidfrom.Text == "")
                        {
                            lblStatus.Text = "Please Enter Valid From Date";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (txtvalidto.Text == "")
                        {
                            lblStatus.Text = "Please Enter Valid TO Date";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        #region Prepare Parameters
                        object[] updatecountryInfo = new object[10];
                        int i = 0;
                        double volume;

                        //0
                        updatecountryInfo.SetValue(ddlAircraftType.SelectedItem.Text.Trim(), i);
                        i++;

                        //1
                        updatecountryInfo.SetValue(ddlOrigin.SelectedItem.Text.Trim(), i);
                        i++;

                        //2
                        updatecountryInfo.SetValue(ddlDest.SelectedItem.Text.Trim(), i);
                        i++;

                        //3
                        updatecountryInfo.SetValue(txtvalidfrom.Text.Trim(), i);
                        i++;

                        //4
                        updatecountryInfo.SetValue(txtvalidto.Text.Trim(), i);
                        i++;

                        //5
                        updatecountryInfo.SetValue(txtWeight.Text.Trim(), i);
                        i++;
                        

                        //6
                        if (ddlVolMeasure.SelectedItem.Text == "Inch")
                        {
                            volume = (Convert.ToDouble(txtVolume.Text) / (0.061024));

                            updatecountryInfo.SetValue(volume, i);
                            i++;
                        }
                        else
                        {
                            updatecountryInfo.SetValue(txtVolume.Text.Trim(), i);
                            i++;
                        }

                        //7
                        
                        if (chkActive.Checked == true)
                        {
                            updatecountryInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            updatecountryInfo.SetValue("False", i);
                            i++;
                        }
                        //8
                        updatecountryInfo.SetValue(ddlVolMeasure.SelectedItem.Text, i);
                        i++;

                        //9
                        int srno = int.Parse(Session["srno"].ToString());
                        updatecountryInfo.SetValue(srno, i);
                        i++;

                        #endregion Prepare Parameters

                        int UpdateID = 0;
                        UpdateID = objBAL.UpdateAircraftTonnageRecords(updatecountryInfo);
                        if (UpdateID >= 0)
                        {
                            btnClear_Click(null, null);
                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Aircraft Tonnage Updated Sucessfully..";
                            btnSave.Text = "Save";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Aircraft Tonnage Updation Failed..";
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

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                grvArcraftTonnageList.DataSource = null;
                grvArcraftTonnageList.DataBind();
                #region Prepare Parameters
                object[] countryListInfo = new object[6];
                int i = 0;

                //0
                countryListInfo.SetValue(ddlAircraftType.SelectedItem.Text.Trim(), i);
                i++;

                //1
                countryListInfo.SetValue(ddlOrigin.SelectedItem.Text.Trim(), i);
                i++;

                //2
                countryListInfo.SetValue(ddlDest.SelectedItem.Text.Trim(), i);
                i++;

                //3
                if (txtvalidfrom.Text == "")
                {
                    //DateTime validfrom
                        string validfrom = "1900-01-01 00:00:00.000";
                        countryListInfo.SetValue(validfrom,i);
                        i++;
                }
                else
                {
                    countryListInfo.SetValue(txtvalidfrom.Text.Trim(), i);
                    i++;
                }

                //4
                if (txtvalidto.Text == "")
                {
                    string validto = "5000-12-30 00:00:00.000";
                    countryListInfo.SetValue(validto, i);
                    i++;
                }
                else
                {
                    countryListInfo.SetValue(txtvalidto.Text.Trim(), i);
                    i++;
                }
                //5
                if (chkActive.Checked == true)
                {
                    countryListInfo.SetValue("True", i);
                }
                else
                {
                    countryListInfo.SetValue("False", i);
                }



                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.spGetAircraftTonnageList(countryListInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grvArcraftTonnageList.PageIndex = 0;
                                grvArcraftTonnageList.DataSource = ds;
                                grvArcraftTonnageList.DataMember = ds.Tables[0].TableName;
                                grvArcraftTonnageList.DataBind();
                                grvArcraftTonnageList.Visible = true;
                                Session["aircrafttonnage"] = ds;
                                //ds.Clear();

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }

        }

        # region grvArcraftTonnageList_RowCommand
        protected void grvArcraftTonnageList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                   Session["srno"]=((Label)grvArcraftTonnageList.Rows[RowIndex].FindControl("lblSrNo")).Text;
                    Label lblAircraftType = (Label)grvArcraftTonnageList.Rows[RowIndex].FindControl("lblAircraftType");
                    Label lblValidFrom = (Label)grvArcraftTonnageList.Rows[RowIndex].FindControl("lblValidFrom");
                    Label lblValidTo = (Label)grvArcraftTonnageList.Rows[RowIndex].FindControl("lblValidTo");
                    Label lblOrigin = (Label)grvArcraftTonnageList.Rows[RowIndex].FindControl("lblOrigin");
                    Label lblDestination = (Label)grvArcraftTonnageList.Rows[RowIndex].FindControl("lblDestination");
                    Label lblWeight = (Label)grvArcraftTonnageList.Rows[RowIndex].FindControl("lblWeight");
                    Label lblVolume = (Label)grvArcraftTonnageList.Rows[RowIndex].FindControl("lblVolume");
                    Label lblstat = (Label)grvArcraftTonnageList.Rows[RowIndex].FindControl("lblStatus");
                    Label lblunit = (Label)grvArcraftTonnageList.Rows[RowIndex].FindControl("lblVolUnit");

                    //ddlAircraftType.Text = lblAircraftType.Text;
                    //ddlDest.Text = lblDestination.Text;
                    //ddlOrigin.Text = lblOrigin.Text;
                    ddlAircraftType.SelectedIndex = ddlAircraftType.Items.IndexOf(((ListItem)ddlAircraftType.Items.FindByText(lblAircraftType.Text)));
                    ddlDest.SelectedIndex = ddlDest.Items.IndexOf(((ListItem)ddlDest.Items.FindByText(lblDestination.Text)));
                    ddlOrigin.SelectedIndex = ddlOrigin.Items.IndexOf(((ListItem)ddlOrigin.Items.FindByText(lblOrigin.Text)));
                    txtvalidfrom.Text = lblValidFrom.Text;
                    txtvalidto.Text = lblValidTo.Text;
                    txtVolume.Text = lblVolume.Text;
                    txtWeight.Text = lblWeight.Text;
                    txtvalidfrom.Enabled = false;
                    txtvalidto.Enabled = false;
                    ddlAircraftType.Enabled = false;

                    if (lblstat.Text == "True")
                    {
                        chkActive.Checked = true;
                    }

                    if (lblstat.Text == "False")
                    {
                        chkActive.Checked = false;
                    }
                    ddlVolMeasure.SelectedIndex = ddlVolMeasure.Items.IndexOf(((ListItem)ddlVolMeasure.Items.FindByText(lblunit.Text)));
                    btnSave.Text = "Update";

                }
            }
            catch (Exception ex)
            {

            }

        }
        # endregion grvArcraftTonnageList_RowCommand

        # region grvArcraftTonnageList_RowEditing
        protected void grvArcraftTonnageList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grvArcraftTonnageList_RowEditing

        # region grvArcraftTonnageList_PageIndexChanging
        protected void grvArcraftTonnageList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {


            //#region Prepare Parameters
            //object[] countryListInfo = new object[6];
            //int i = 0;

            ////0
            //countryListInfo.SetValue(ddlAircraftType.SelectedItem.Text.Trim(), i);
            //i++;

            ////1
            //countryListInfo.SetValue(ddlDest.SelectedItem.Text.Trim(), i);
            //i++;

            ////2
            //countryListInfo.SetValue(ddlOrigin.SelectedItem.Text.Trim(), i);
            //i++;

            ////3
            //countryListInfo.SetValue(txtvalidfrom.Text.Trim(), i);
            //i++;

            ////4
            //countryListInfo.SetValue(txtvalidto.Text.Trim(), i);
            //i++;

            ////5
            //if (chkActive.Checked == true)
            //{
            //    countryListInfo.SetValue("True", i);
            //}
            //else
            //{
            //    countryListInfo.SetValue("False", i);
            //}


            //#endregion Prepare Parameters

            //DataSet ds = new DataSet();
            //ds = objBAL.spGetAircraftTonnageList(countryListInfo);
            //if (ds != null)
            //{
            //    if (ds.Tables != null)
            //    {
            //        if (ds.Tables.Count > 0)
            //        {
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                grvArcraftTonnageList.PageIndex = 0;
            //                grvArcraftTonnageList.DataSource = ds;
            //                grvArcraftTonnageList.DataMember = ds.Tables[0].TableName;
            //                grvArcraftTonnageList.DataBind();
            //                grvArcraftTonnageList.Visible = true;
            //                //ds.Clear();

            //            }
            //        }
            //    }
            //}

            DataSet ds = (DataSet)Session["aircrafttonnage"];
            grvArcraftTonnageList.PageIndex = e.NewPageIndex;
            grvArcraftTonnageList.DataSource = ds.Tables[0]; ;
            grvArcraftTonnageList.DataBind();
        }
        # endregion grvArcraftTonnageList_PageIndexChanging

        protected void grvArcraftTonnageList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        # region GetOriginCode List

        private void GetOriginCode()
        {
            try
            {
                DataSet ds = objBAL.GetOriginCodeList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = ds.Tables[0].Columns["Code"].ColumnName;

                            ddlOrigin.DataTextField = ds.Tables[0].Columns["Code"].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion GetOriginCode List
        # region GetDestCode List

        private void GetDestCode()
        {
            try
            {
                DataSet ds = objBAL.GetDestCodeList(ddlDest.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlDest.DataSource = ds;
                            ddlDest.DataMember = ds.Tables[0].TableName;
                            ddlDest.DataValueField = ds.Tables[0].Columns["Code"].ColumnName;

                            ddlDest.DataTextField = ds.Tables[0].Columns["Code"].ColumnName;
                            ddlDest.DataBind();
                            ddlDest.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion GetOriginCode List

        # region GetAircrafteType List

        private void GetAircraftType()
        {
            try
            {
                DataSet dsair = objBAL.GetAircraftType(ddlAircraftType.SelectedValue);
                if (dsair != null)
                {
                    if (dsair.Tables != null)
                    {
                        if (dsair.Tables.Count > 0)
                        {

                            ddlAircraftType.DataSource = dsair;
                            ddlAircraftType.DataMember = dsair.Tables[0].TableName;
                            ddlAircraftType.DataValueField = dsair.Tables[0].Columns["Code"].ColumnName;

                            ddlAircraftType.DataTextField = dsair.Tables[0].Columns["Code"].ColumnName;
                            ddlAircraftType.DataBind();
                            ddlAircraftType.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion GetAircraftType List
    }
}
