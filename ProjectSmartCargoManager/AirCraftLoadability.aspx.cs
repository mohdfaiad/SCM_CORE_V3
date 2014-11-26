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
    public partial class AirCraftLoadability : System.Web.UI.Page
    {
        BALLoadability objBal = new BALLoadability();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { AircraftTypeList(); }

        }

        #region Aircraft Type List
        protected void AircraftTypeList()
        {
            try
            {
                lblStatus.Text = "";

                DataSet ds = new DataSet();
                ds = objBal.GetAircraftTypeList();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlType.DataSource = ds;
                            ddlType.DataMember = ds.Tables[0].TableName;
                            ddlType.DataValueField = ds.Tables[0].Columns["AircraftType"].ColumnName;

                            ddlType.DataTextField = ds.Tables[0].Columns["AircraftType"].ColumnName;
                            ddlType.DataBind();
                            ddlType.Items.Insert(0, "Select Type");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Aircraft Type List

        #region Loadability List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                #region Parameters
                object[] Params = new object[7];
                int i = 0;

                //1
                string org = null;
                if (ddlType.SelectedIndex != 0)
                    org = ddlType.SelectedItem.Text;
                else org = "";
                Params.SetValue(org, i);
                i++;

                //2
                if (txtValidFrm.Text != "")
                {
                    string frmdt = txtValidFrm.Text.ToString();
                    Params.SetValue(frmdt, i);
                    i++;
                }
                else
                {
                    Params.SetValue("", i);
                    i++;
                }

                //3
                if (txtValidTo.Text != "")
                {
                    string enddt = txtValidTo.Text.ToString();
                    Params.SetValue(enddt, i);
                    i++;
                }
                else
                {
                    Params.SetValue("", i);
                    i++;
                }

                Params.SetValue(txtTailId.Text.Trim(), i);
                i++;

                Params.SetValue(txtCompartmnt.Text.Trim(), i);
                i++;

                Params.SetValue(txtContainertype.Text.Trim(), i);
                i++;

                Params.SetValue(txtFloorLamination.Text.Trim(), i);
                i++;

                #endregion Parameters

                DataSet ds = new DataSet();
                ds = objBal.GetLoadablityList(Params);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                GrdLoadability.PageIndex = 0;
                                GrdLoadability.DataSource = ds;
                                GrdLoadability.DataMember = ds.Tables[0].TableName;
                                GrdLoadability.DataBind();
                                GrdLoadability.Visible = true;
                                Session["ds"] = ds;
                                btnClear_Click(null, null);
                                //ds.Clear();

                                for (int j = 0; j < GrdLoadability.Rows.Count; j++)
                                {
                                    if (((Label)(GrdLoadability.Rows[j].FindControl("lblAct"))).Text.ToString() == "True")
                                    {
                                        ((Label)(GrdLoadability.Rows[j].FindControl("lblAct"))).Text = "Active";
                                    }
                                    else if (((Label)(GrdLoadability.Rows[j].FindControl("lblAct"))).Text.ToString() == "False")
                                    {
                                        ((Label)(GrdLoadability.Rows[j].FindControl("lblAct"))).Text = "InActive";
                                    }
                                }

                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Records does not exists...";
                                Session["ds"] = null;
                                GrdLoadability.DataSource = null;
                                GrdLoadability.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion Loadability List

        #region Add Loadability Detail
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                # region Save
                if (btnSave.Text == "Save")
                {
                    if (ddlType.SelectedIndex > 0)
                    {
                        try
                        {
                            #region Prepare Parameters
                            object[] Params = new object[17];
                            int i = 0;

                            //1
                            DateTime frmdt = DateTime.ParseExact(txtValidFrm.Text, "dd/MM/yyyy", null);
                            Params.SetValue(frmdt, i);
                            i++;

                            //2
                            DateTime todt = DateTime.ParseExact(txtValidTo.Text, "dd/MM/yyyy", null);
                            Params.SetValue(todt, i);
                            i++;

                            //3
                            Params.SetValue(ddlType.SelectedItem.Text, i);
                            i++;

                            //4
                            decimal lgth = decimal.Parse(txtLgth.Text);
                            Params.SetValue(lgth, i);
                            i++;

                            //5
                            decimal width = decimal.Parse(txtWidth.Text);
                            Params.SetValue(width, i);
                            i++;

                            //6
                            decimal hght = decimal.Parse(txtHght.Text);
                            Params.SetValue(hght, i);
                            i++;

                            //7
                            Params.SetValue("CMS", i);
                            i++;

                            //8
                            if (chkAct.Checked == true)
                            {
                                Params.SetValue(true, i);
                                i++;
                            }
                            else if (chkAct.Checked == false)
                            {
                                Params.SetValue(false, i);
                                i++;
                            }

                            //9
                            Params.SetValue(txtTailId.Text.Trim(), i);
                            i++;

                            //10
                            Params.SetValue(txtCompartmnt.Text.Trim(), i);
                            i++;

                            //11
                            if (txtMaxWeight.Text.Trim() != "")
                                Params.SetValue(txtMaxWeight.Text.Trim(), i);
                            else
                                Params.SetValue(0, i);
                            i++;

                            //12
                            if (txtMaxVol.Text.Trim() != "")
                                Params.SetValue(txtMaxVol.Text.Trim(), i);
                            else
                                Params.SetValue(0, i);
                            i++;

                            //13
                            if (txtMaxContainers.Text.Trim() != "")
                                Params.SetValue(txtMaxContainers.Text.Trim(), i);
                            else
                                Params.SetValue(0, i);
                            i++;

                            //14
                            Params.SetValue(txtContainertype.Text.Trim(), i);                            
                            i++;

                            //15
                            if (txtMaxPallets96.Text.Trim() != "")
                                Params.SetValue(txtMaxPallets96.Text.Trim(), i);
                            else
                                Params.SetValue(0, i);
                            i++;

                            //16
                            if (txtMaxPallets88.Text.Trim() != "")
                                Params.SetValue(txtMaxPallets88.Text.Trim(), i);
                            else
                                Params.SetValue(0, i);
                            i++;

                            //17
                            Params.SetValue(txtFloorLamination.Text.Trim(), i);
                            i++;

                            #endregion Prepare Parameters

                            int ID = 0;
                            ID = objBal.AddLoadabilityDetail(Params);
                            if (ID >= 0)
                            {
                                btnClear_Click(null, null);
                                btnList_Click(null, null);
                                lblStatus.Text = "Record Added Successfully";
                                lblStatus.ForeColor = Color.Green;

                            }
                            else
                            {
                                btnList_Click(null, null);
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Record Insertion Failed..";

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Select Aircraft Type...";
                    }
                }

                # endregion Save

                # region Update
                if (btnSave.Text == "Update")
                {
                    if (ddlType.SelectedIndex > 0)
                    {
                        try
                        {
                            #region Prepare Parameters
                            object[] Params = new object[18];
                            int i = 0;
                            int srnum = int.Parse(Session["SrNum"].ToString());

                            //0
                            Params.SetValue(srnum, i);
                            i++;

                            //1
                            DateTime frmdt = DateTime.ParseExact(txtValidFrm.Text, "dd/MM/yyyy", null);
                            Params.SetValue(frmdt, i);
                            i++;

                            //2
                            DateTime todt = DateTime.ParseExact(txtValidTo.Text, "dd/MM/yyyy", null);
                            Params.SetValue(todt, i);
                            i++;

                            //3
                            Params.SetValue(ddlType.SelectedItem.Text, i);
                            i++;

                            //4
                            decimal lgth = decimal.Parse(txtLgth.Text);
                            Params.SetValue(lgth, i);
                            i++;

                            //5
                            decimal width = decimal.Parse(txtWidth.Text);
                            Params.SetValue(width, i);
                            i++;

                            //6
                            decimal hght = decimal.Parse(txtWidth.Text);
                            Params.SetValue(hght, i);
                            i++;

                            //7
                            Params.SetValue("CMS", i);
                            i++;

                            //8
                            if (chkAct.Checked == true)
                            {
                                Params.SetValue(true, i);
                                i++;
                            }
                            else if (chkAct.Checked == false)
                            {
                                Params.SetValue(false, i);
                                i++;
                            }

                            //9
                            Params.SetValue(txtTailId.Text.Trim(), i);
                            i++;

                            //10
                            Params.SetValue(txtCompartmnt.Text.Trim(), i);
                            i++;

                            //11
                            if (txtMaxWeight.Text.Trim() != "")
                                Params.SetValue(txtMaxWeight.Text.Trim(), i);
                            else
                                Params.SetValue(0, i);
                            i++;

                            //12
                            if (txtMaxVol.Text.Trim() != "")
                                Params.SetValue(txtMaxVol.Text.Trim(), i);
                            else
                                Params.SetValue(0, i);
                            i++;

                            //13
                            if (txtMaxContainers.Text.Trim() != "")
                                Params.SetValue(txtMaxContainers.Text.Trim(), i);
                            else
                                Params.SetValue(0, i);
                            i++;

                            //14
                            Params.SetValue(txtContainertype.Text.Trim(), i);
                            i++;

                            //15
                            if (txtMaxPallets96.Text.Trim() != "")
                                Params.SetValue(txtMaxPallets96.Text.Trim(), i);
                            else
                                Params.SetValue(0, i);
                            i++;

                            //16
                            if (txtMaxPallets88.Text.Trim() != "")
                                Params.SetValue(txtMaxPallets88.Text.Trim(), i);
                            else
                                Params.SetValue(0, i);
                            i++;

                            //17
                            Params.SetValue(txtFloorLamination.Text.Trim(), i);
                            i++;

                            #endregion Prepare Parameters

                            int ID = 0;
                            ID = objBal.UpdateLoadabilityDetail(Params);
                            if (ID >= 0)
                            {
                                btnClear_Click(null, null);
                                btnList_Click(null, null);
                                lblStatus.Text = "Record Updated Successfully";
                                lblStatus.ForeColor = Color.Green;
                                btnSave.Text = "Save";


                            }
                            else
                            {
                                btnList_Click(null, null);
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Record Updation Failed..";

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Select Aircraft Type...";
                    }
                }

                # endregion Update
            }

            catch (Exception ex)
            {

            }
        }
        #endregion Add Loadability Detail

        #region Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtValidFrm.Text = txtValidTo.Text = string.Empty;
            txtLgth.Text = txtHght.Text = txtWidth.Text = string.Empty;
            //txtUnit.Text = string.Empty;
            chkAct.Checked = false;
            ddlType.SelectedIndex = 0;
            btnSave.Text = "Save";
            lblStatus.Text = string.Empty;
            ddlType.Enabled = true;
            txtTailId.Text = "";
            txtCompartmnt.Text = "";
            txtMaxVol.Text = "";
            txtMaxContainers.Text = "";
            txtContainertype.Text = "";
            txtMaxPallets88.Text = "";
            txtMaxPallets96.Text = "";
            txtFloorLamination.Text = "";
            txtMaxWeight.Text = "";
        }
        #endregion Clear

        # region GrdLoadability_RowCommand
        protected void GrdLoadability_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                #region EDIT
                if (e.CommandName == "Edit")
                {

                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);

                    Session["SrNum"] = ((Label)(GrdLoadability.Rows[RowIndex].FindControl("lblSrNum"))).Text.ToString();
                    txtValidFrm.Text = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblValidFrm")).Text.Trim();
                    txtValidTo.Text = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblValiidTo")).Text.Trim();
                    string crafttype = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblType")).Text;
                    Label lgth = (Label)GrdLoadability.Rows[RowIndex].FindControl("lblLgth");
                    Label width = (Label)GrdLoadability.Rows[RowIndex].FindControl("lblWidth");
                    Label hgt = (Label)GrdLoadability.Rows[RowIndex].FindControl("lblHght");
                    Label unit = (Label)GrdLoadability.Rows[RowIndex].FindControl("lblUnit");
                    Label active = (Label)GrdLoadability.Rows[RowIndex].FindControl("lblAct");

                    ddlType.SelectedIndex = ddlType.Items.IndexOf((ListItem)ddlType.Items.FindByText(crafttype));
                    ddlType.Enabled = false;
                    txtLgth.Text = lgth.Text;
                    txtWidth.Text = width.Text;
                    txtHght.Text = hgt.Text;
                    //txtUnit.Text = unit.Text;
                    if (active.Text == "Active")
                    {
                        chkAct.Checked = true;
                    }

                    if (active.Text == "InActive")
                    {
                        chkAct.Checked = false;
                    }

                    txtTailId.Text = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblTailId")).Text.Trim();
                    txtCompartmnt.Text = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblCompartment")).Text.Trim();
                    txtMaxVol.Text = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblMaxVolumeinCMeter")).Text.Trim();
                    txtMaxContainers.Text = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblMaxContainers")).Text.Trim();
                    txtContainertype.Text = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblContainerType")).Text.Trim();
                    txtMaxPallets96.Text = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblMaxPallets96inch")).Text.Trim();
                    txtMaxPallets88.Text = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblMaxPallets88inch")).Text.Trim();
                    txtFloorLamination.Text = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblFloorLimitation")).Text.Trim();
                    txtMaxWeight.Text = ((Label)GrdLoadability.Rows[RowIndex].FindControl("lblMaxWeightinkg")).Text.Trim();
                    

                    btnSave.Text = "Update";

                }
                #endregion EDIT

                #region DELETE

                if (e.CommandName == "DeleteRecord")
                {
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    int srno = int.Parse(((Label)(GrdLoadability.Rows[RowIndex].FindControl("lblSrNum"))).Text.ToString());

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
                        int res = objBal.DeleteLoadDetail(Params);
                        if (res == 0)
                        {
                            btnClear_Click(null, null);
                            btnList_Click(null, null);
                            lblStatus.Text = "Record Deleted Successfully";
                            lblStatus.ForeColor = Color.Red;

                        }

                    }

                    catch (Exception ex)
                    {

                    }
                }

                #endregion DELETE
            }
            catch (Exception ex)
            {

            }

        }
        # endregion GrdLoadability_RowCommand

        # region GrdLoadability_RowEditing
        protected void GrdLoadability_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion GrdLoadability_RowEditing

        # region GrdLoadability_PageIndexChanging
        protected void GrdLoadability_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                DataSet dsnew = (DataSet)Session["ds"];
                GrdLoadability.PageIndex = e.NewPageIndex;
                GrdLoadability.DataSource = dsnew; // ds.Copy();
                GrdLoadability.DataBind();


                for (int j = 0; j < GrdLoadability.Rows.Count; j++)
                {
                    if (((Label)(GrdLoadability.Rows[j].FindControl("lblAct"))).Text.ToString() == "True")
                    {
                        ((Label)(GrdLoadability.Rows[j].FindControl("lblAct"))).Text = "Active";
                    }

                    else if (((Label)(GrdLoadability.Rows[j].FindControl("lblAct"))).Text.ToString() == "False")
                    {
                        ((Label)(GrdLoadability.Rows[j].FindControl("lblAct"))).Text = "InActive";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GrdLoadability_PageIndexChanging
    }
}
