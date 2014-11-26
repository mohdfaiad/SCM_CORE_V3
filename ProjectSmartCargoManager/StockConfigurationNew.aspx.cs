using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using BAL;
using System.IO;
using System.Configuration;
using QID.DataAccess;


namespace ProjectSmartCargoManager
{
    public partial class StockConfigurationNew : System.Web.UI.Page
    {
        #region Variables
        BALStockConfig objBAL = new BALStockConfig();
        clsFillCombo cfc = new clsFillCombo();
        EMAILOUT SendEmail = new EMAILOUT();
        int AvailQty;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (Session["UserName"] != null)
                    {
                        //((Label)Master.FindControl("lbluser")).Text = "Welcome : " + Session["ULDUserName"].ToString();
                        Session["GridDataforSCN"] = null;
                        //LoadingDropdownDetails(ddlWareHouse);
                        cfc.FillAllComboBoxes("tblWarehouseMaster", "ALL", ddlWareHouse);
                        cfc.FillAllComboBoxes("tblULDTypeMasters", "SELECT", ddlULDType);
                        cfc.FillAllComboBoxes("tblULDStatusMaster", "SELECT", ddlULDStatus);
                        
                        #region Loading Grid For the first time

                        try
                        {
                            lblStatus.Text = "";
                            //btnAdd.Visible = false;
                            //btnEdit.Visible = false;
                            //btnDelete.Visible = false;
                            //DataSet ds = LoadingDropdownDetails();
                            Session["StationList"] = cfc.ReturnDataset("tblWarehouseMaster", "SELECT");
                            Session["ULDStatusForSCN"] = cfc.ReturnDataset("tblULDStatusMaster", "SELECT");
                            Session["ULDTypeForSCN"] = cfc.ReturnDataset("tblULDTypeMasters", "SELECT");

                            btnSearch_Click(null, null);

                            //DataSet ds = 
                            //if (ds != null)
                            //{
                            //    if (ds.Tables.Count > 0)
                            //    {
                            //        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                            //        {
                            //            //Session["StationList"] = cfc.ReturnDataset("tblWarehouseMaster", "SELECT");
                            //            //Session["StationList"] = ds.Tables[0];

                            //            Session["GridDataForSCN"] = ds.Tables[1];
                            //            gvAddressGrpFTP.DataSource = ds.Tables[1];
                            //            gvAddressGrpFTP.DataBind();

                            //            for (int i = 0; i < gvAddressGrpFTP.Rows.Count; i++)
                            //            {
                            //                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataSource = (DataTable)Session["StationList"];
                            //                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataTextField = "WHCode";
                            //                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataValueField = "ID";
                            //                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataBind();
                            //                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Text = ds.Tables[1].Rows[i]["StationID"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                            //                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Enabled = false;
                            //                ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).ReadOnly = true;
                            //                ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtMinAvlQty"))).ReadOnly = true;
                            //                ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtEmailID"))).ReadOnly = true;
                                            
                            //                btnAdd.Visible = true;
                            //                btnEdit.Visible = true;
                            //                btnDelete.Visible = true;

                            //            }


                            //        }
                            //        else
                            //            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count == 0)
                            //            {
                            //                Session["StationList"] = ds.Tables[0];
                            //                DataTable dt = new DataTable();
                            //                dt.Columns.Add("ID");
                            //                dt.Columns.Add("StationID");
                            //                dt.Columns.Add("ULDType");
                            //                dt.Columns.Add("MinAvlQuantity");
                            //                dt.Columns.Add("AvlQuantity");
                            //                dt.Columns.Add("EmailID");
                            //                DataRow dr = dt.NewRow();
                            //                dt.Rows.Add(dr);
                            //                gvAddressGrpFTP.DataSource = dt;
                            //                gvAddressGrpFTP.DataBind();
                            //                Session["GridData"] = dt;

                            //                for (int i = 0; i < gvAddressGrpFTP.Rows.Count; i++)
                            //                {

                            //                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataSource = (DataTable)Session["StationList"];
                            //                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataTextField = "WHCode";
                            //                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataValueField = "ID";
                            //                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataBind();
                            //                    //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Text = ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString(); ;
                            //                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Enabled = true;
                            //                    ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).ReadOnly = false;
                            //                    ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtMinAvlQty"))).ReadOnly = false;
                            //                    ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtEmailID"))).ReadOnly = false;
                            //                    btnAdd.Visible = true;
                            //                    btnEdit.Visible = true;
                            //                    btnDelete.Visible = true;

                            //                }
                            //                lblStatus.ForeColor = System.Drawing.Color.Red;
                            //                lblStatus.Text = "No Records found";
                            //            }
                            //    }
                            //    else
                            //    {
                            //        lblStatus.ForeColor = System.Drawing.Color.Red;
                            //        lblStatus.Text = "No Records found";
                            //    }

                            //}



                        }
                        catch (Exception ex)
                        { }

                        #endregion

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Add Click
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                DataTable dt = (DataTable)Session["GridDataForSCN"];
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                gvAddressGrpFTP.DataSource = dt;
                gvAddressGrpFTP.DataBind();


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i + gvAddressGrpFTP.PageIndex*gvAddressGrpFTP.PageSize][1].ToString().Trim() != "")
                    {
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataSource = ((DataSet)Session["StationList"]).Tables[0];
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataTextField = "Code";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataValueField = "ID";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataBind();
                        //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Text = dt.Rows[i]["StationID"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//dt.Rows[i]["WHCodeID"].ToString();
                        DropDownList ddlULDTypeGrid = ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation")));
                        ddlULDTypeGrid.SelectedIndex = ddlULDTypeGrid.Items.IndexOf(ddlULDTypeGrid.Items.FindByText(dt.Rows[i + gvAddressGrpFTP.PageIndex * gvAddressGrpFTP.PageSize]["Station"].ToString()));
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Enabled = false;

                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataSource = ((DataSet)Session["ULDStatusForSCN"]).Tables[0];
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataTextField = "Code";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataValueField = "ID";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataBind();
                        //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Text = ds.Tables[0].Rows[i]["ULDStatus"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                        DropDownList ddlULDStatusGrid = ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus")));
                        ddlULDStatusGrid.SelectedIndex = ddlULDStatusGrid.Items.IndexOf(ddlULDStatusGrid.Items.FindByText(dt.Rows[i + gvAddressGrpFTP.PageIndex * gvAddressGrpFTP.PageSize]["ULDStatus"].ToString()));
                        ((DropDownList)(gvAddressGrpFTP.Rows[i - gvAddressGrpFTP.PageIndex * gvAddressGrpFTP.PageSize].FindControl("ddlULDStatus"))).Enabled = false;

                        //((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).ReadOnly = true;
                        ((DropDownList)(gvAddressGrpFTP.Rows[i - gvAddressGrpFTP.PageIndex * gvAddressGrpFTP.PageSize].FindControl("ddlULDType"))).DataSource = ((DataSet)Session["ULDTypeForSCN"]).Tables[0];
                        ((DropDownList)(gvAddressGrpFTP.Rows[i - gvAddressGrpFTP.PageIndex * gvAddressGrpFTP.PageSize].FindControl("ddlULDType"))).DataTextField = "Code";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i - gvAddressGrpFTP.PageIndex * gvAddressGrpFTP.PageSize].FindControl("ddlULDType"))).DataValueField = "ID";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i - gvAddressGrpFTP.PageIndex * gvAddressGrpFTP.PageSize].FindControl("ddlULDType"))).DataBind();
                        //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Text = ds.Tables[0].Rows[i]["ULDStatus"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                        DropDownList ddlULDTypesGrid = ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType")));
                        ddlULDTypesGrid.SelectedIndex = ddlULDTypesGrid.Items.IndexOf(ddlULDTypesGrid.Items.FindByText(dt.Rows[i + gvAddressGrpFTP.PageIndex * gvAddressGrpFTP.PageSize]["ULDType"].ToString()));
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).Enabled = false;

                        ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtMinAvlQty"))).ReadOnly = true;
                        ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtEmailID"))).ReadOnly = true;

                    }
                    else
                    {
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataSource = ((DataSet)Session["StationList"]).Tables[0];
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataTextField = "Code";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataValueField = "ID";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataBind();
                        //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Text = dt.Rows[i]["WHCodeID"].ToString();
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Enabled = true;

                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataSource = ((DataSet)Session["ULDStatusForSCN"]).Tables[0];
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataTextField = "Code";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataValueField = "ID";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataBind();
                        //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Text = ds.Tables[1].Rows[i]["ULDStatus"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Enabled = true;

                        //((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).ReadOnly = false;
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataSource = ((DataSet)Session["ULDTypeForSCN"]).Tables[0];
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataTextField = "Code";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataValueField = "ID";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataBind();
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).Enabled = true;


                        ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtMinAvlQty"))).ReadOnly = false;
                        ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtEmailID"))).ReadOnly = false;
                    }
                }
                Session["GridDataForSCN"] = dt;
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Edit Click
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                for (int i = 0; i < gvAddressGrpFTP.Rows.Count; i++)
                {
                    if (((RadioButton)(gvAddressGrpFTP.Rows[i].FindControl("rdbStockUpdate"))).Checked)
                    {
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Enabled = false;
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).Enabled = false;
                        //((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).ReadOnly = true;
                        ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtMinAvlQty"))).ReadOnly = false;
                        ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtEmailID"))).ReadOnly = false;
                        return;
                    }

                }
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Please Select Stock to Edit";
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Delte Click
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                bool flag = false;
                int count = 0;
                DataTable dt = (DataTable)Session["GridDataForSCN"];
                for (int i = 0; i < gvAddressGrpFTP.Rows.Count; i++)
                {


                    if (((RadioButton)(gvAddressGrpFTP.Rows[i].FindControl("rdbStockUpdate"))).Checked)
                    {
                        string ID = ((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();
                        if (ID != "")
                        { }
                        else
                        { ID = "0"; }
                        count++;
                        dt.Rows.Remove(dt.Rows[i]);

                        //if (((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).Text != "")
                        //if (((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).Text.Length >1)
                        //if (dt.Rows[i][1].ToString().Trim() != "")
                        
                            flag = objBAL.SaveStockDetails(ID, "Delete", "0", "", "0", "","0");
                            //flag = SaveStockDetails(((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).Text, "", "", "", "Delete");
                            if (flag == true)
                            {
                                lblStatus.ForeColor = System.Drawing.Color.Green;
                                lblStatus.Text = "Stock Deleted Successfully";
                            }
                            else
                            {
                                lblStatus.ForeColor = System.Drawing.Color.Red;
                                lblStatus.Text = "Operation failed";
                            }
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    gvAddressGrpFTP.DataSource = dt;
                    gvAddressGrpFTP.DataBind();
                }
                else
                {
                    DataRow dr = ((DataTable)(Session["GridDataForSCN"])).NewRow();
                    dt.Rows.Add(dr);
                    gvAddressGrpFTP.DataSource = dt;
                    gvAddressGrpFTP.DataBind();
                }
                for (int i = 0; i < gvAddressGrpFTP.Rows.Count; i++)
                {
                    //if (((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).Text != "")
                    //if (((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).Text.Length>=1)
                    //{

                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataSource = ((DataSet)Session["StationList"]).Tables[0];
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataTextField = "Code";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataValueField = "ID";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataBind();
                        // ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Text = ds.Tables[0].Rows[i]["Station"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                        DropDownList ddlULDTypeGrid = ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation")));
                        ddlULDTypeGrid.SelectedIndex = ddlULDTypeGrid.Items.IndexOf(ddlULDTypeGrid.Items.FindByText(dt.Rows[i]["Station"].ToString()));
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Enabled = false;

                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataSource = ((DataSet)Session["ULDStatusForSCN"]).Tables[0];
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataTextField = "Code";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataValueField = "ID";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataBind();
                        //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Text = ds.Tables[0].Rows[i]["ULDStatus"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                        DropDownList ddlULDStatusGrid = ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus")));
                        ddlULDStatusGrid.SelectedIndex = ddlULDStatusGrid.Items.IndexOf(ddlULDStatusGrid.Items.FindByText(dt.Rows[i]["ULDStatus"].ToString()));
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Enabled = false;

                        //((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).ReadOnly = true;

                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataSource = ((DataSet)Session["ULDTypeForSCN"]).Tables[0];
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataTextField = "Code";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataValueField = "ID";
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataBind();
                        //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Text = ds.Tables[0].Rows[i]["ULDStatus"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                        DropDownList ddlULDTypesGrid = ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType")));
                        ddlULDTypesGrid.SelectedIndex = ddlULDTypesGrid.Items.IndexOf(ddlULDTypesGrid.Items.FindByText(dt.Rows[i]["ULDType"].ToString()));
                        ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).Enabled = false;

                        ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtMinAvlQty"))).ReadOnly = true;
                        ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtEmailID"))).ReadOnly = true;
                    //}
                    //else
                    //{
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataSource = ((DataSet)Session["StationList"]).Tables[0];
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataTextField = "Code";
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataValueField = "ID";
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataBind();
                    //    //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Text = ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString(); ;
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Enabled = true;

                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataSource = ((DataSet)Session["ULDStatusForSCN"]).Tables[0];
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataTextField = "Code";
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataValueField = "ID";
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataBind();
                    //    //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Text = ds.Tables[1].Rows[i]["ULDStatus"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Enabled = true;

                    //    //((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).ReadOnly = false;
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataSource = ((DataSet)Session["ULDTypeForSCN"]).Tables[0];
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataTextField = "Code";
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataValueField = "ID";
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataBind();
                    //    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).Enabled = true;


                    //    ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtMinAvlQty"))).ReadOnly = false;
                    //    ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtEmailID"))).ReadOnly = false;
                    //}

                }
                Session["GridDataForSCN"] = dt;
                if (count > 0)
                { }
                else
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Please Select Stock to Delete";
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                bool flag = false;
                Button clickedButton = (Button)sender;
                GridViewRow row = (GridViewRow)clickedButton.Parent.Parent;
                int rowIndex = row.RowIndex;
                //TextBox ULDType = ((TextBox)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtULDType")));
                DropDownList ULDType = ((DropDownList)(gvAddressGrpFTP.Rows[rowIndex].FindControl("ddlULDType")));
                TextBox MinAvlQty = ((TextBox)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtMinAvlQty")));
                TextBox EmailID = ((TextBox)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtEmailID")));
                DropDownList StationID = ((DropDownList)(gvAddressGrpFTP.Rows[rowIndex].FindControl("ddlStation")));
                DropDownList ULDStatusID = ((DropDownList)(gvAddressGrpFTP.Rows[rowIndex].FindControl("ddlULDStatus")));    
                string ID = ((Button)(gvAddressGrpFTP.Rows[rowIndex].FindControl("btnSave"))).CommandArgument.ToString();
                if (ID != "")
                { }
                else
                { ID = "0"; }

                //if (((TextBox)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtULDType"))).Text != "")
                if (((DropDownList)(gvAddressGrpFTP.Rows[rowIndex].FindControl("ddlULDType"))).Text.Length >0)
                {
                    //if (((TextBox)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtULDType"))).ReadOnly)
                    if (((DropDownList)(gvAddressGrpFTP.Rows[rowIndex].FindControl("ddlULDType"))).Enabled==false)
                    {

                        flag = objBAL.SaveStockDetails(ID, "Edit", StationID.SelectedValue, ULDType.SelectedItem.Text.Trim(), MinAvlQty.Text, EmailID.Text, ULDStatusID.SelectedValue);
                        if (flag == true)
                        {
                            //LoadGrid();
                            btnSearch_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Stock Edited Successfully";
                            return;
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Operation failed";
                            return;
                        }
                    }
                    else
                        if (((DropDownList)(gvAddressGrpFTP.Rows[rowIndex].FindControl("ddlULDType"))).Enabled)

                        //if (((TextBox)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtULDType"))).ReadOnly == false)
                        {
                            flag = objBAL.SaveStockDetails(ID, "Add", StationID.SelectedValue, ULDType.SelectedItem.Text.Trim(), MinAvlQty.Text, EmailID.Text, ULDStatusID.SelectedValue);

                            if (flag == true)
                            {
                                //LoadGrid();
                                btnSearch_Click(null, null);
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = "Stock Added Successfully";
                                return;
                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Operation failed";
                                return;
                            }

                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Stock Saved Successfully";
                            return;

                        }
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter ULDNumber";
                    return;
                }



            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Button Send
        protected void btnSend_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    lblStatus.Text = "";
            //    bool flag = false;
            //    string frmEmailID = ConfigurationManager.AppSettings["FromEmailID"].ToString();
            //    string Password = ConfigurationManager.AppSettings["Password"].ToString();
            //    string[] multiEmail = txtEmailIDPopUp.Text.Split(',');
            //    string Subject = "ULD Stock Reduction Alert";
            //    string body = Session["Body"].ToString();
            //    for (int i = 0; i < multiEmail.Length; i++)
            //    {
            //        flag = SendEmail.sendMail(frmEmailID, multiEmail[i], Password, Subject, body, false);
            //        if (flag == true)
            //        {
            //            lblStatus.ForeColor = Color.Green;
            //            lblStatus.Text = "Message Sent Successfully";
            //        }
            //        else
            //        {
            //            lblStatus.ForeColor = Color.Red;
            //            lblStatus.Text = "Message Sending Failed";
            //            return;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{ }
            try
            {
                lblStatus.Text = "";
                bool flag = false;
                Session["Body"] = null;
                Button clickedButton = (Button)sender;
                GridViewRow row = (GridViewRow)clickedButton.Parent.Parent;
                int rowIndex = row.RowIndex;
                DropDownList ddlStationMail = (DropDownList)(gvAddressGrpFTP.Rows[rowIndex].FindControl("ddlStation"));
                DropDownList ddlULDStatusMail = (DropDownList)(gvAddressGrpFTP.Rows[rowIndex].FindControl("ddlULDStatus"));
                string Station = ddlStationMail.SelectedItem.Text;
                string ULDStatus = ddlULDStatusMail.SelectedItem.Text;
                //string ULDType = ((TextBox)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtULDType"))).Text;
                DropDownList ddlULDTypemail = (DropDownList)(gvAddressGrpFTP.Rows[rowIndex].FindControl("ddlULDType"));
                string ULDType = ddlULDTypemail.SelectedItem.Text;

                string MinAvlQty = ((TextBox)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtMinAvlQty"))).Text;
                string AvlQty = ((TextBox)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtAvlQty"))).Text;
                string EmailID = ((TextBox)(gvAddressGrpFTP.Rows[rowIndex].FindControl("txtEmailID"))).Text;
                string Body = "Hi,\n\nPlease take appropriate action for reduced ULD stock Mentioned below.\n\nStation:" + Station + "\nULD Type:" + ULDType + "\nULD Status: " + ULDStatus + "\nAvailable Quantity: " + AvlQty + "\nMinimum Required Quantity: " + MinAvlQty + "\n\nThanks,\nSmartKargo.";
                Session["Body"] = Body;
                txtEmailIDPopUp.Text = EmailID;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);

            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Button Send Popup
        protected void btnSendPop_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                bool flag = false;
                string frmEmailID = ConfigurationManager.AppSettings["FromEmailID"].ToString();
                string Password = ConfigurationManager.AppSettings["Password"].ToString();
                string[] multiEmail = txtEmailIDPopUp.Text.Split(',');
                string Subject = "SCM-ULD Stock Reduction Alert";
                string body = Session["Body"].ToString();
                for (int i = 0; i < multiEmail.Length; i++)
                {
                    //flag = SendEmail.sendMail(frmEmailID, multiEmail[i], Password, Subject, body, false);
                    flag = cls_BL.addMsgToOutBox(Subject, body, "", multiEmail[i]);
                    if (flag == true)
                    {
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "Message Sent Successfully";
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Message Sending Failed";
                        return;
                    }
                }
            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Loading Dropdown Lists
        public DataSet LoadingDropdownDetails()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = objBAL.GetStockConfigurationDetailsWithDropdownData();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            { return null; }

        }
        #endregion

        #region Loading Dropdown Lists - ddlWareHouse
        public void LoadingDropdownDetails(DropDownList ddlWareHouse)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = objBAL.GetDropdownDetailsforStockReport();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ddlWareHouse.DataSource = ds.Tables[1];
                            ddlWareHouse.DataTextField = "WHCode";
                            ddlWareHouse.DataValueField = "ID";
                            ddlWareHouse.DataBind();
                            ddlWareHouse.Items.Insert(0, "ALL");

                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        //#region Refresh Grid After Every Update
        //public void LoadGrid()
        //{
        //    try
        //    {
        //        #region Loading Grid

        //        try
        //        {
        //            lblStatus.Text = "";
        //            btnAdd.Visible = false;
        //            btnEdit.Visible = false;
        //            btnDelete.Visible = false;
        //            DataSet ds = LoadingDropdownDetails();
        //            if (ds != null)
        //            {
        //                if (ds.Tables.Count > 0)
        //                {
        //                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
        //                    {
        //                        Session["StationList"] = ds.Tables[0];

        //                        Session["GridData"] = ds.Tables[1];
        //                        gvAddressGrpFTP.DataSource = ds.Tables[1];
        //                        gvAddressGrpFTP.DataBind();

        //                        for (int i = 0; i < gvAddressGrpFTP.Rows.Count; i++)
        //                        {

        //                            ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataSource = (DataTable)Session["StationList"];
        //                            ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataTextField = "WHCode";
        //                            ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataValueField = "ID";
        //                            ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataBind();
        //                            ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Text = ds.Tables[1].Rows[i]["StationID"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
        //                            ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Enabled = false;
        //                            ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).ReadOnly = true;
        //                            ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtMinAvlQty"))).ReadOnly = true;
        //                            ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtEmailID"))).ReadOnly = true;
        //                            btnAdd.Visible = true;
        //                            btnEdit.Visible = true;
        //                            btnDelete.Visible = true;

        //                        }


        //                    }
        //                    else
        //                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count == 0)
        //                        {
        //                            Session["StationList"] = ds.Tables[0];
        //                            DataTable dt = new DataTable();
        //                            dt.Columns.Add("ID");
        //                            dt.Columns.Add("StationID");
        //                            dt.Columns.Add("ULDType");
        //                            dt.Columns.Add("MinAvlQuantity");
        //                            dt.Columns.Add("AvlQuantity");
        //                            dt.Columns.Add("EmailID");
        //                            DataRow dr = dt.NewRow();
        //                            dt.Rows.Add(dr);
        //                            gvAddressGrpFTP.DataSource = dt;
        //                            gvAddressGrpFTP.DataBind();
        //                            Session["GridData"] = dt;

        //                            for (int i = 0; i < gvAddressGrpFTP.Rows.Count; i++)
        //                            {

        //                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataSource = (DataTable)Session["StationList"];
        //                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataTextField = "WHCode";
        //                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataValueField = "ID";
        //                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataBind();
        //                                //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Text = ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString(); ;
        //                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Enabled = true;
        //                                ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).ReadOnly = false;
        //                                ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtMinAvlQty"))).ReadOnly = false;
        //                                ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtEmailID"))).ReadOnly = false;
        //                                btnAdd.Visible = true;
        //                                btnEdit.Visible = true;
        //                                btnDelete.Visible = true;

        //                            }
        //                            lblStatus.ForeColor = Color.Red;
        //                            lblStatus.Text = "No Records found";
        //                        }
        //                }
        //                else
        //                {
        //                    lblStatus.ForeColor = Color.Red;
        //                    lblStatus.Text = "No Records found";
        //                }

        //            }



        //        }
        //        catch (Exception ex)
        //        { }

        //        #endregion

        //    }
        //    catch (Exception ex)
        //    { }
        //}
        //#endregion

        #region Search Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                gvAddressGrpFTP.DataSource = null;
                gvAddressGrpFTP.DataBind();
                lblStatus.Text = "";

                string StationID = "";
                if (ddlWareHouse.SelectedIndex != 0)
                {
                    StationID = ddlWareHouse.SelectedValue;
                }
                else
                { StationID = "0"; }

                

                //int AvailableQty;

                //if (AvailQty != "" || AvailQty != null)
                //{
                //    AvailableQty = 
                //}

                //else
                //{
                string paramULDType = "";
                
                DataSet ds = GetStockList_SearchCriteria(StationID,ddlULDType.SelectedValue,ddlULDStatus.SelectedValue);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["GridDataForSCN"] = ds.Tables[0];
                            gvAddressGrpFTP.DataSource = ds;
                            gvAddressGrpFTP.DataBind();

                            for (int i = 0; i < gvAddressGrpFTP.Rows.Count; i++)
                            {
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataSource = ((DataSet)Session["StationList"]).Tables[0];
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataTextField = "Code";
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataValueField = "ID";
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataBind();
                                // ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Text = ds.Tables[0].Rows[i]["Station"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                                DropDownList ddlULDTypeGrid = ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation")));
                                ddlULDTypeGrid.SelectedIndex = ddlULDTypeGrid.Items.IndexOf(ddlULDTypeGrid.Items.FindByText(ds.Tables[0].Rows[i + gvAddressGrpFTP.PageIndex * gvAddressGrpFTP.PageSize]["Station"].ToString()));
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Enabled = false;

                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataSource = ((DataSet)Session["ULDStatusForSCN"]).Tables[0];
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataTextField = "Code";
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataValueField = "ID";
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataBind();
                                //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Text = ds.Tables[0].Rows[i]["ULDStatus"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                                DropDownList ddlULDStatusGrid = ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus")));
                                ddlULDStatusGrid.SelectedIndex = ddlULDStatusGrid.Items.IndexOf(ddlULDStatusGrid.Items.FindByText(ds.Tables[0].Rows[i + gvAddressGrpFTP.PageIndex * gvAddressGrpFTP.PageSize]["ULDStatus"].ToString()));
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Enabled = false;
                                
                                //((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).ReadOnly = true;
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataSource = ((DataSet)Session["ULDTypeForSCN"]).Tables[0];
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataTextField = "Code";
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataValueField = "ID";
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataBind();
                                //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Text = ds.Tables[0].Rows[i]["ULDStatus"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                                DropDownList ddlULDTypesGrid = ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType")));
                                ddlULDTypesGrid.SelectedIndex = ddlULDTypesGrid.Items.IndexOf(ddlULDTypesGrid.Items.FindByText(ds.Tables[0].Rows[i + gvAddressGrpFTP.PageIndex * gvAddressGrpFTP.PageSize]["ULDType"].ToString()));
                                //ddlULDTypesGrid.SelectedIndex = ddlULDTypesGrid.Items.IndexOf(ddlULDTypesGrid.Items.FindByText(ds.Tables[0].Rows[i][
                                ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).Enabled = false;
                                

                                ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtMinAvlQty"))).ReadOnly = true;
                                ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtEmailID"))).ReadOnly = true;

                                btnAdd.Visible = true;
                                btnEdit.Visible = true;
                                btnDelete.Visible = true;

                            }
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Records found";
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                //Session["StationList"] = ds.Tables[0];
                                DataTable dt = new DataTable();
                                dt.Columns.Add("ID");
                                dt.Columns.Add("StationID");
                                dt.Columns.Add("ULDType");
                                dt.Columns.Add("MinAvlQuantity");
                                dt.Columns.Add("AvlQuantity");
                                dt.Columns.Add("EmailID");
                                DataRow dr = dt.NewRow();
                                dt.Rows.Add(dr);
                                gvAddressGrpFTP.DataSource = dt;
                                gvAddressGrpFTP.DataBind();
                                Session["GridDataForSCN"] = dt;

                                for (int i = 0; i < gvAddressGrpFTP.Rows.Count; i++)
                                {

                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataSource = ((DataSet)Session["StationList"]).Tables[0];
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataTextField = "Code";
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataValueField = "ID";
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).DataBind();
                                    //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Text = ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString(); ;
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlStation"))).Enabled = true;

                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataSource = ((DataSet)Session["ULDStatusForSCN"]).Tables[0];
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataTextField = "Code";
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataValueField = "ID";
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).DataBind();
                                    //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Text = ds.Tables[1].Rows[i]["ULDStatus"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Enabled = true;

                                    //((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtUlDType"))).ReadOnly = false;
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataSource = ((DataSet)Session["ULDTypeForSCN"]).Tables[0];
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataTextField = "Code";
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataValueField = "ID";
                                    ((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).DataBind();
                                    //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDStatus"))).Text = ds.Tables[1].Rows[i]["ULDStatus"].ToString();//((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("LocationID"))).Text;//((Button)(gvAddressGrpFTP.Rows[i].FindControl("btnSave"))).CommandArgument.ToString();//ds.Tables[0].Rows[i]["WHCodeID"].ToString();
                                    //((DropDownList)(gvAddressGrpFTP.Rows[i].FindControl("ddlULDType"))).Enabled = true;

                                    ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtMinAvlQty"))).ReadOnly = false;
                                    ((TextBox)(gvAddressGrpFTP.Rows[i].FindControl("txtEmailID"))).ReadOnly = false;
                                   
                                    btnAdd.Visible = true;
                                    btnEdit.Visible = true;
                                    btnDelete.Visible = true;

                                }
                                lblStatus.ForeColor = System.Drawing.Color.Red;
                                lblStatus.Text = "No Records found";
                            }
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "No Records found";
                    }
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "No Records found";
                }

            }
            catch (Exception ex)
            { }

        }
        #endregion

        //public void getAvailableQty(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //string StationId =
        //        //string uldtype = ddlULDType.SelectedValue;
        //        Response.Redirect("FrmULDList.aspx?StationId="+ddlStation.SelectedValue+"& UldType="+ddlULDType.SelectedValue+"");
        //    }
        //    catch (Exception ex)
        //    { 
            
        //    }
        //}
        # region Link Button Code
        protected void ShowAvailQty_Click(object sender, EventArgs e)
        {
            try
            {
                 LinkButton btn = (LinkButton)sender;

                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                if (gvr.RowIndex < 0)
                    return;

                int rowindex = gvr.RowIndex;

               int  gridIndex = 0;
                gridIndex = rowindex;

                string Station = ((DropDownList)(gvAddressGrpFTP.Rows[rowindex].FindControl("ddlStation"))).SelectedItem.Text;
                //string uldtype = ((TextBox)(gvAddressGrpFTP.Rows[rowindex].FindControl("txtUlDType"))).Text;
                string uldtype = ((DropDownList)(gvAddressGrpFTP.Rows[rowindex].FindControl("ddlULDType"))).SelectedItem.Text;

                Response.Redirect("FrmULDList.aspx?StationId=" + Station +"&uldtype="+ uldtype);

                                
            }
            catch (Exception ex)
            { }
        }

        #endregion



        #region Getting Stock List as Per Search Criteria
        public DataSet GetStockList_SearchCriteria(string StationID, string ULDTypeID, string ULDStatusID)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = objBAL.GetStockList(StationID, ULDTypeID, ULDStatusID);//SelectRecords("spGetStockList", "StationID", StationID, SqlDbType.Int);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                    }
                }
                return ds;

            }
            catch (Exception ex)
            { return null; }

        }

        #endregion

        #region Grid Page Index Changed
        protected void gvAddressGrpFTP_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAddressGrpFTP.PageIndex = e.NewPageIndex;
                btnSearch_Click(null, null);
                //gvAddressGrpFTP.DataSource = ((DataTable)Session["GridDataforSCN"]);
                //gvAddressGrpFTP.DataBind();
            }
            catch (Exception ex)
            { 

            }
        }
        #endregion

        #region Button Send Stock Check Message
        protected void btnSendSCM_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;
                StockCheckMessage();
                
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Stock Check Message
        public void StockCheckMessage()
        {
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet ds = db.SelectRecords("spGetStationSCMMsg");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataSet dsSCMData = db.SelectRecords("spGetSCMDataPerStation", "Station", ds.Tables[0].Rows[i]["Station"].ToString(), SqlDbType.VarChar);
                        if (dsSCMData != null && dsSCMData.Tables.Count > 0 && dsSCMData.Tables[0].Rows.Count > 0 && dsSCMData.Tables.Count > 1 && dsSCMData.Tables[1].Rows.Count > 0)
                        {
                            if (!cls_BL.SCMEncodingForSend(dsSCMData, ds.Tables[0].Rows[i]["FromEmailID"].ToString(), ds.Tables[0].Rows[i]["ToEmailID"].ToString()))
                            {
                                lblStatus.Text = "Error: SCM Message sending failed! Please try again..";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                        }
                    }
                    lblStatus.Text = "SCM Message Sent Successfully!";
                    lblStatus.ForeColor = Color.Green;

                }
                else
                {
                    lblStatus.Text = "No records found to send SCM Message!";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion
    }
}
