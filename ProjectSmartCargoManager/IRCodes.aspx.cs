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
    public partial class IRCodes : System.Web.UI.Page
    {
        string serialnumber = null;

        #region Variable

        BALIRCodes objBAL = new BALIRCodes();

        #endregion variable

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ddlExpImp.SelectedIndex = 1;
                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    grvCommodityList.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
            }
        }

        
        # region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                # region Save
                if (btnSave.Text == "Save")
                {
                    try
                    {
                        if (txtIRCode.Text == "" || txtIRDes.Text=="")
                        {
                            lblStatus.Text = "Please Enter Irregularity Code and Description";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        #region Prepare Parameters
                        object[] IRCInfo = new object[4];
                        int i = 0;

                        //0
                        IRCInfo.SetValue(txtIRCode.Text.Trim(), i);
                        i++;

                        //1
                        IRCInfo.SetValue(txtIRDes.Text.Trim().ToUpper(), i);
                        i++;

                        //2
                        if (chkActive.Checked == true)
                        {
                            IRCInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            IRCInfo.SetValue("False", i);
                            i++;
                        }

                        //3
                        string ei = null;
                        if (ddlExpImp.SelectedItem.Text == "Export")
                            ei = "E";
                        else if (ddlExpImp.SelectedItem.Text == "Import")
                            ei = "I";
                        IRCInfo.SetValue(ei, i);
                        i++;

                        #endregion Prepare Parameters

                        int ID = 0;
                        ID = objBAL.AddIRCode(IRCInfo);
                        if (ID >= 0)
                        {
                            txtIRCode.Text = txtIRDes.Text = string.Empty;
                            ddlExpImp.SelectedIndex = 0;
                            chkActive.Checked = false;
                            lblStatus.Text = string.Empty;

                            btnList_Click(sender, e);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "IR Code Added Sucessfully..";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "IR Code Insertion Failed..";
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
                        object[] updateIRCInfo = new object[5];
                        int i = 0;
                        string serno = Session["sno"].ToString();

                        //0
                        updateIRCInfo.SetValue(txtIRCode.Text.Trim(), i);
                        i++;

                        //1
                        updateIRCInfo.SetValue(txtIRDes.Text.Trim().ToUpper(), i);
                        i++;

                        //2
                        if (chkActive.Checked == true)
                        {
                            updateIRCInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            updateIRCInfo.SetValue("False", i);
                            i++;
                        }

                        //3
                        string expimp = null;
                        if (ddlExpImp.SelectedItem.Text == "Export")
                            expimp = "E";
                        else
                            expimp = "I";
                        updateIRCInfo.SetValue(expimp, i);
                        i++;
                          
                        //4
                        updateIRCInfo.SetValue(serno, i);
                        i++;

                        #endregion Prepare Parameters

                        int UpdateID = 0;
                        UpdateID = objBAL.UpdateIRCode(updateIRCInfo);
                        if (UpdateID >= 0)
                        {
                            txtIRCode.Text = txtIRDes.Text = string.Empty;
                            ddlExpImp.SelectedIndex = 0;
                            chkActive.Checked = false;
                            lblStatus.Text = string.Empty;
                            btnSave.Text = "Save";
                            btnList_Click(sender, e);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "IR Code Updated Sucessfully..";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "IR Code Updation Failed..";
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
        # endregion btnSave_Click

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtIRCode.Text = string.Empty;
            txtIRDes.Text = string.Empty;
            chkActive.Checked = false;
            btnSave.Text = "Save";
            ddlExpImp.SelectedIndex = 0;
            lblStatus.Text = string.Empty;

            grvCommodityList.DataSource = null;
            grvCommodityList.DataBind();
        }
        #endregion btnClear_Click

        # region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                #region Prepare Parameters
                object[] IRInfo = new object[4];
                int i = 0;

                string ircval=null;
                if(txtIRCode.Text=="")
                  ircval="All";
                else 
                 ircval=txtIRCode.Text;

                IRInfo.SetValue(ircval, i);
                i++;

                IRInfo.SetValue(ddlExpImp.SelectedValue, i);
                i++;
                IRInfo.SetValue(txtIRDes.Text, i);

                i++;
                IRInfo.SetValue(chkActive.Checked, i);
                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.GetIRCOdesList(IRInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grvCommodityList.PageIndex = 0;
                                grvCommodityList.DataSource = ds;
                                grvCommodityList.DataMember = ds.Tables[0].TableName;
                                grvCommodityList.DataBind();
                                grvCommodityList.Visible = true;
                                Session["ds"] = ds;

                                txtIRCode.Text = string.Empty;
                                txtIRDes.Text = string.Empty;
                                chkActive.Checked = false;
                                btnSave.Text = "Save";
                                ddlExpImp.SelectedIndex = 0;
                                lblStatus.Text = string.Empty;

                                //ds.Clear();

                                for (int j = 0; j < grvCommodityList.Rows.Count; j++)
                                {
                                    if (((Label)(grvCommodityList.Rows[j].FindControl("lblStat"))).Text.ToString() == "True")
                                    {
                                        ((Label)(grvCommodityList.Rows[j].FindControl("lblStat"))).Text = "Active";
                                    }
                                    //string a = ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString();
                                    else if (((Label)(grvCommodityList.Rows[j].FindControl("lblStat"))).Text.ToString() == "False")
                                    {
                                        ((Label)(grvCommodityList.Rows[j].FindControl("lblStat"))).Text = "InActive";
                                    }
                                }
                                for (int j = 0; j < grvCommodityList.Rows.Count; j++)
                                {
                                    if (((Label)(grvCommodityList.Rows[j].FindControl("lblExpImp"))).Text.ToString() == "E")
                                    {
                                        ((Label)(grvCommodityList.Rows[j].FindControl("lblExpImp"))).Text = "Export";
                                    }

                                    else if (((Label)(grvCommodityList.Rows[j].FindControl("lblExpImp"))).Text.ToString() == "I")
                                    {
                                        ((Label)(grvCommodityList.Rows[j].FindControl("lblExpImp"))).Text = "Import";
                                    }
                                }

                            }
                            else
                            {
                                
                                btnClear_Click(null,null);
                                lblStatus.Text = "No Records Found";
                                grvCommodityList.DataSource = null;
                                grvCommodityList.DataBind();
                                
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

        # region grvCommodityList_RowCommand
        protected void grvCommodityList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    lblStatus.Text = "";
                    
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    Label lblIRCode = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblIRCode");
                    Label lblIRDes = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblIRCDesc");
                    Label lblstat = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblStat");
                    Label lblei = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblExpImp");
                  Label serialnum = ((Label)grvCommodityList.Rows[RowIndex].FindControl("lblSerialNo"));
                  serialnumber = serialnum.Text;
                  Session["sno"] = serialnumber;
                    txtIRCode.Text = lblIRCode.Text;
                    txtIRDes.Text = lblIRDes.Text;

                    
                    if (lblei.Text=="")
                    { ddlExpImp.SelectedIndex = ddlExpImp.Items.IndexOf(((ListItem)ddlExpImp.Items.FindByText("Export"))); }
                    else
                        ddlExpImp.SelectedIndex = ddlExpImp.Items.IndexOf(((ListItem)ddlExpImp.Items.FindByText(lblei.Text)));   

                  
                    if (lblstat.Text == "Active")
                    {
                        chkActive.Checked = true;
                    }

                    if (lblstat.Text == "InActive")
                    {
                        chkActive.Checked = false;
                    }
                    btnSave.Text = "Update";

                }
            }
            catch (Exception ex)
            {

            }

        }
        # endregion grvCommodityList_RowCommand

        # region grvCommodityList_RowEditing
        protected void grvCommodityList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grvCommodityList_RowEditing

        # region grvCommodityList_PageIndexChanging
        protected void grvCommodityList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                lblStatus.Text = "";

                //#region Prepare Parameters
                //#endregion Prepare Parameters

                //DataSet ds = new DataSet();
                //ds = objBAL.GetIRCOdesList();
                
                //if (ds != null)
                //{
                //    if (ds.Tables != null)
                //    {
                //        if (ds.Tables.Count > 0)
                //        {
                //            if (ds.Tables[0].Rows.Count > 0)
                //            {
                //                grvCommodityList.PageIndex = 0;
                //                grvCommodityList.DataSource = ds;
                //                grvCommodityList.DataMember = ds.Tables[0].TableName;
                //                grvCommodityList.DataBind();
                //                grvCommodityList.Visible = true;
                //                 }
                //        }
                //    }
                //}

                DataSet ds = (DataSet)Session["ds"];
                grvCommodityList.PageIndex = e.NewPageIndex;
                grvCommodityList.DataSource = ds.Tables[0];
                grvCommodityList.DataBind();


                for (int j = 0; j < grvCommodityList.Rows.Count; j++)
                {
                    if (((Label)(grvCommodityList.Rows[j].FindControl("lblStat"))).Text.ToString() == "True")
                    {
                        ((Label)(grvCommodityList.Rows[j].FindControl("lblStat"))).Text = "Active";
                    }
                    //string a = ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString();
                    else if (((Label)(grvCommodityList.Rows[j].FindControl("lblStat"))).Text.ToString() == "False")
                    {
                        ((Label)(grvCommodityList.Rows[j].FindControl("lblStat"))).Text = "InActive";
                    }
                }
                for (int j = 0; j < grvCommodityList.Rows.Count; j++)
                {
                    if (((Label)(grvCommodityList.Rows[j].FindControl("lblExpImp"))).Text.ToString() == "E")
                    {
                        ((Label)(grvCommodityList.Rows[j].FindControl("lblExpImp"))).Text = "Export";
                    }

                    else if (((Label)(grvCommodityList.Rows[j].FindControl("lblExpImp"))).Text.ToString() == "I")
                    {
                        ((Label)(grvCommodityList.Rows[j].FindControl("lblExpImp"))).Text = "Import";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion grvCommodityList_PageIndexChanging

    }
}
