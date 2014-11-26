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
    public partial class AirlinePrefixMaster : System.Web.UI.Page
    {

        #region Variable

        AirPrefixBAL objBAL = new AirPrefixBAL();

        #endregion variable

        protected void Page_Load(object sender, EventArgs e)
        {

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
                        if (txtCode.Text == "" || txtPrefix.Text=="")
                        {
                            lblStatus.Text = "Please Enter Both Values";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        #region Prepare Parameters
                        object[] Prefix= new object[2];
                        int i = 0;
                        
                        //0
                        Prefix.SetValue(txtCode.Text, i);
                        i++;

                        //1
                        Prefix.SetValue(txtPrefix.Text, i);
                        i++;


                        #endregion Prepare Parameters

                        int ID = 0;
                        ID = objBAL.AddPrefix(Prefix);
                        if (ID >= 0)
                        {
                            btnClear_Click(sender,e);
                            btnList_Click(sender, e);
                            lblStatus.Text = "Record Added Successfully";
                            lblStatus.ForeColor = Color.Green;
                            btnSave.Text = "Save";
                            
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Pefix Insertion Failed..";
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
                        object[] updatePrefix = new object[5];
                        int i = 0;
                        string serialno = Session["SerialNo"].ToString();
                        //0
                        updatePrefix.SetValue(txtCode.Text.Trim(), i);
                        i++;

                        //1
                        updatePrefix.SetValue(txtPrefix.Text, i);
                        i++;

                        //3
                        updatePrefix.SetValue(serialno, i);
                        i++;
                        
                        #endregion Prepare Parameters

                        int UpdateID = 0;
                        UpdateID = objBAL.UpdatePrefix(updatePrefix);
                        if (UpdateID >= 0)
                        {
                            btnClear_Click(sender, e);
                            btnList_Click(sender, e);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Record Updated Sucessfully..";
                            btnSave.Text = "Save";
                         
                        }
                        else
                        {
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
            {

            }

        }
        # endregion btnSave_Click

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtCode.Text = string.Empty;
            txtPrefix.Text = string.Empty;
            btnSave.Text = "Save";
            lblStatus.Text = string.Empty;
        }
        #endregion btnClear_Click


        # region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                DataSet ds = new DataSet();
                ds = objBAL.GetProductTypeList();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                               grdAirPrefix.PageIndex = 0;
                               grdAirPrefix.DataSource = ds;
                               grdAirPrefix.DataMember = ds.Tables[0].TableName;
                               grdAirPrefix.DataBind();
                               grdAirPrefix.Visible = true;
                                ds.Clear();

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

        # region grdAirPrefix_RowCommand
        protected void grdAirPrefix_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);

                    Label lblAirCode = (Label)grdAirPrefix.Rows[RowIndex].FindControl("lblAirCode");
                    Label lblAirPrefix = (Label)grdAirPrefix.Rows[RowIndex].FindControl("lblAirPrefix");
                    Session["SerialNo"] = ((Label)(grdAirPrefix.Rows[RowIndex].FindControl("lblSerialNo"))).Text.ToString();


                   txtCode.Text = lblAirCode.Text;
                   txtPrefix.Text = lblAirPrefix.Text;
                    
                    btnSave.Text = "Update";

                }
            }
            catch (Exception ex)
            {

            }

        }
        # endregion grdAirPrefix_RowCommand


        # region grdAirPrefix_RowEditing
        protected void grdAirPrefix_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grdAirPrefix_RowEditing


        # region grdAirPrefix_PageIndexChanging
        protected void grdAirPrefix_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                lblStatus.Text = "";
            
                DataSet ds = new DataSet();
                ds = objBAL.GetProductTypeList();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                               grdAirPrefix.PageIndex = 0;
                               grdAirPrefix.DataSource = ds;
                               grdAirPrefix.DataMember = ds.Tables[0].TableName;
                               grdAirPrefix.DataBind();
                               grdAirPrefix.Visible = true;
                                // ds.Clear();
                            }
                        }
                    }
                }

               grdAirPrefix.PageIndex = e.NewPageIndex;
               grdAirPrefix.DataSource = ds.Copy();
               grdAirPrefix.DataBind();

            }
            catch (Exception ex)
            {
            }
        }

        # endregion grdAirPrefix_PageIndexChanging


    }
}
