using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class OCMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    grdOc.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
                btnAdd.Attributes.Add("onclick", "return CheckBlank();");
               // btnList_Click(sender, e);

            }
        }
      
        #region List Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            BAL.BALOC objCUR = new BAL.BALOC();

            try
            {
                DataSet dsCUR = objCUR.dsOcDetails(txtOcCode.Text.Trim(), txtOcDesc.Text.Trim());

                if (dsCUR != null && dsCUR.Tables.Count > 0 && dsCUR.Tables[0].Rows.Count > 0)
                {
                    grdOc.DataSource = dsCUR.Tables[0];
                    grdOc.DataBind();
                    ViewState["dsOc"] = dsCUR;

                    for (int i = 0; i < grdOc.Rows.Count; i++)
                    {
                        if (((Label)(grdOc.Rows[i].FindControl("lblisact"))).Text.ToString() == "True")
                        {
                            ((Label)(grdOc.Rows[i].FindControl("lblisact"))).Text = "Active";
                        }
                        else if (((Label)(grdOc.Rows[i].FindControl("lblisact"))).Text.ToString() == "False")
                        {
                            grdOc.Rows[i].Visible = false;
                            //grdOc.Rows[i].Enabled = false;
                            ((Label)(grdOc.Rows[i].FindControl("lblisact"))).Text = "In-Active";
                        }
                    }
                    //if (grdOc.Rows.Count <= 1)
                    //{
                    //    lblStatus.ForeColor = System.Drawing.Color.Red;
                    //    lblStatus.Text = "No Records Found!";
                    //    //grdOc.Visible = false;
                    //    grdOc.DataSource = null;
                    //    grdOc.DataBind();
                    //}
                    //else
                    //{
                    //    lblStatus.Text = "";
                    //}
                }
                else
                {
                    grdOc.DataSource = null;
                    grdOc.DataBind();
                    lblStatus.Text = "No Records Found!";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    //lblStatus.ForeColor = System.Drawing.Color.Red;
                    //lblStatus.Text = "Error in Listing OC!";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text ="Error:" +ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
            
        }
        #endregion

        #region Adding
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string createdby = Session["UserName"].ToString();
            DateTime now = DateTime.Now;
            try
            {
                if (txtOcCode.Text == "" || txtOcDesc.Text == "")
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Entries Can't be Blank";
                }
                BAL.BALOC objCUR = new BAL.BALOC();
                if (objCUR.ModifyOc("insert", txtOcCode.Text.Trim(), txtOcDesc.Text.Trim(), now, createdby))
                {
                    #region For Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Params = new object[7];
                    int j = 0;

                    //1
                    Params.SetValue("OC(Master)", j);
                    j++;

                    //2
                    Params.SetValue(txtOcCode.Text, j);
                    j++;

                    //3

                    Params.SetValue("ADD", j);
                    j++;

                    //4

                    Params.SetValue("", j);
                    j++;


                    //5

                    Params.SetValue("", j);
                    j++;

                    //6

                    Params.SetValue(Session["UserName"], j);
                    j++;

                    //7
                    Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), j);
                    j++;


                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Params);
                    #endregion

                    btnClear_Click(sender, e);
                    btnList_Click(sender, e);

                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Text = "OC Updated Successfully";

                    // Enable List..
                    txtOcCode.ReadOnly = false;
                    btnList.Enabled = true;
                    btnClear.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in Adding";
            }
        }
        #endregion

        #region Delete
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string createdby = Session["UserName"].ToString();
            DateTime now = DateTime.Now;
            try
            {
                BAL.BALOC objCUR = new BAL.BALOC();
                if (objCUR.ModifyOc("delete", txtOcCode.Text.Trim(), txtOcDesc.Text.Trim(), now, createdby))
                {
                    #region For Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Params = new object[7];
                    int j = 0;

                    //1
                    Params.SetValue("OC(Master)", j);
                    j++;

                    //2
                    Params.SetValue(txtOcCode.Text, j);
                    j++;

                    //3

                    Params.SetValue("DELETE", j);
                    j++;

                    //4

                    Params.SetValue("", j);
                    j++;


                    //5

                    Params.SetValue("", j);
                    j++;

                    //6

                    Params.SetValue(Session["UserName"], j);
                    j++;

                    //7
                    Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), j);
                    j++;


                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Params);
                    #endregion

                    btnClear_Click(sender, e);
                    btnList_Click(sender, e);
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "OC Deleted Successfully";

                    // Enable List..
                    txtOcCode.ReadOnly = false;
                    btnList.Enabled = true;
                    btnClear.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error in Deleting";
            }
        }
        #endregion

        #region Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtOcDesc.Text = string.Empty;
            txtOcCode.Text = string.Empty;
            //btnList_Click(sender,e);
            lblStatus.Text = "";
            grdOc.DataSource = null;
            grdOc.DataBind();
            btnList.Enabled = true;
        }
        #endregion

        #region RowCommand
        protected void grdOc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
        }
        #endregion

        #region Page Index
        protected void grdOc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //BAL.BALCurrency objCUR = new BAL.BALCurrency();
                //DataSet dsCUR = objCUR.dsCurrDetails(txtOcCode.Text.Trim(), txtOcDesc.Text.Trim());

                DataSet ds = new DataSet();
                ds=(DataSet)ViewState["dsOc"];
                grdOc.PageIndex = e.NewPageIndex;
                grdOc.DataSource = ds;
                grdOc.DataBind();

                for (int i = 0; i < grdOc.Rows.Count; i++)
                {
                    if (((Label)(grdOc.Rows[i].FindControl("lblisact"))).Text.ToString() == "True")
                    {
                        ((Label)(grdOc.Rows[i].FindControl("lblisact"))).Text = "Active";
                    }
                    else if (((Label)(grdOc.Rows[i].FindControl("lblisact"))).Text.ToString() == "False")
                    {
                        grdOc.Rows[i].Visible = false;
                        //grdOc.Rows[i].Enabled = false;
                        ((Label)(grdOc.Rows[i].FindControl("lblisact"))).Text = "In-Active";
                    }
                }

            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in Page Index Changing";
            }
        }
        #endregion Page Index

        protected void grdOc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (e.CommandName == "Edit")
                //{
                
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(grdOc.SelectedIndex);
                    Label lblOcCode = (Label)grdOc.Rows[RowIndex].FindControl("lblOcCode");
                    Label lblOcDes = (Label)grdOc.Rows[RowIndex].FindControl("lblOCDesc");

                    txtOcCode.Text = lblOcCode.Text;
                    txtOcDesc.Text = lblOcDes.Text;

                    lblStatus.Text = "";

                    // Disable List..
                    txtOcCode.ReadOnly = true;
                    btnList.Enabled = false;
                    //  btnClear.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in getting OC";
            }
        }
    }
}
