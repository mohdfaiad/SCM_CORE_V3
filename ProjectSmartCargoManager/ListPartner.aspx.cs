using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using QID.DataAccess;
using System.Data;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class ListPartner : System.Web.UI.Page
    {
        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        AirlineMasterBAL ObjBAL = new AirlineMasterBAL();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillPartnerType();
                LoadGridUserList();
            }
        }

        #region Fill Partner Type Master
        public void FillPartnerType()
        {
            try
            {
                PartnerBAL objPM = new PartnerBAL();
                DataSet dsPartnerType = objPM.GetPartnerTypeMaster();
                if (dsPartnerType != null && dsPartnerType.Tables.Count > 0)
                {
                    if (dsPartnerType.Tables[0].Rows.Count > 0)
                    {
                        drpPartnerType.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        drpPartnerType.DataSource = dsPartnerType.Tables[0];
                        drpPartnerType.DataTextField = "Code";
                        drpPartnerType.DataValueField = "Description";
                        drpPartnerType.DataBind();
                        drpPartnerType.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in Fill PartnerType!";
            }
        }
        #endregion

      

        /*protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";

            #region Parameters
            object[] Params = new object[3];
            int i = 0;

            //1
            string org = null;
            if (ddlPartnerType.SelectedIndex != 0)
                org = ddlPartnerType.SelectedItem.Text;
            else org = "All";
            Params.SetValue(org, i);
            i++;

            //2
            if (txtName.Text != "")
            {
                Params.SetValue(txtName.Text, i);
                i++;
            }
            else
            {
                Params.SetValue("All", i);
                i++;
            }

            //3
            if (txPrefix.Text != "")
            {
                Params.SetValue(txPrefix.Text, i);
                i++;
            }
            else
            {
                Params.SetValue("All", i);
                i++;
            }
            #endregion Parameters

            DataSet ds = new DataSet();
            ds = ObjBAL.GetPartnerList(Params);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GrdPartnerList.DataSource = ds;
                            GrdPartnerList.DataMember = ds.Tables[0].TableName;
                            GrdPartnerList.DataBind();
                            GrdPartnerList.Visible = true;
                            Session["ds"] = ds;
                            btnClear_Click(null, null);
                            //ds.Clear();

                            //for (int j = 0; j < GrdPartnerList.Rows.Count; j++)
                            //{
                            //    if (((Label)(GrdPartnerList.Rows[j].FindControl("lblAct"))).Text.ToString() == "True")
                            //    {
                            //        ((Label)(GrdPartnerList.Rows[j].FindControl("lblAct"))).Text = "Active";
                            //    }
                            //    else if (((Label)(GrdPartnerList.Rows[j].FindControl("lblAct"))).Text.ToString() == "False")
                            //    {
                            //        ((Label)(GrdPartnerList.Rows[j].FindControl("lblAct"))).Text = "InActive";
                            //    }
                            //}

                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Records dooes not exists...";
                        }
                    }
                }
            }
        }*/

        # region GrdPartnerList_RowCommand
        protected void GrdPartnerList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                #region EDIT
                if (e.CommandName == "Edit")
                {
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);

                    string srno = ((Label)(GrdPartnerList.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString();
                    Response.Redirect("AirlineMaster.aspx?Command="+e.CommandName+"&Id=" + srno);

                    //DateTime validfrm = DateTime.Parse(((Label)GrdPartnerList.Rows[RowIndex].FindControl("lblValidFrm")).Text);
                    //DateTime validto = DateTime.Parse(((Label)GrdPartnerList.Rows[RowIndex].FindControl("lblValiidTo")).Text);
                    //string crafttype = ((Label)GrdPartnerList.Rows[RowIndex].FindControl("lblType")).Text;
                    //Label lgth = (Label)GrdPartnerList.Rows[RowIndex].FindControl("lblLgth");
                    //Label width = (Label)GrdPartnerList.Rows[RowIndex].FindControl("lblWidth");
                    //Label hgt = (Label)GrdPartnerList.Rows[RowIndex].FindControl("lblHght");
                    //Label unit = (Label)GrdPartnerList.Rows[RowIndex].FindControl("lblUnit");
                    //Label active = (Label)GrdPartnerList.Rows[RowIndex].FindControl("lblAct");

                    //txtValidFrm.Text = validfrm.ToShortDateString();
                    //txtValidTo.Text = validto.ToShortDateString();
                    //ddlType.SelectedIndex = ddlType.Items.IndexOf((ListItem)ddlType.Items.FindByText(crafttype));
                    //ddlType.Enabled = false;
                    //txtLgth.Text = lgth.Text;
                    //txtWidth.Text = width.Text;
                    //txtHght.Text = hgt.Text;
                    //txtUnit.Text = unit.Text;
                    //if (active.Text == "Active")
                    //{
                    //    chkAct.Checked = true;
                    //}

                    //if (active.Text == "InActive")
                    //{
                    //    chkAct.Checked = false;
                    //}
                    //btnSave.Text = "Update";

                }
                if (e.CommandName == "View")
                {
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);

                    string srno = ((Label)(GrdPartnerList.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString();
                    Response.Redirect("AirlineMaster.aspx?Command=" + e.CommandName + "&Id=" + srno);
                }
                #endregion EDIT

                //#region DELETE

                //if (e.CommandName == "DeleteRecord")
                //{
                //    int RowIndex = Convert.ToInt32(e.CommandArgument);
                //    int srno = int.Parse(((Label)(GrdPartnerList.Rows[RowIndex].FindControl("lblSrNum"))).Text.ToString());

                //    try
                //    {
                //        #region Prepare Parameters
                //        DataSet ds = new DataSet();
                //        object[] Params = new object[1];
                //        int i = 0;

                //        //1
                //        Params.SetValue(srno, i);
                //        i++;

                //        #endregion Prepare Parameters

                //        int ID = 0;
                //        int res = objBal.DeleteLoadDetail(Params);
                //        if (res == 0)
                //        {
                //            btnClear_Click(null, null);
                //            btnList_Click(null, null);
                //            lblStatus.Text = "Record Deleted Successfully";
                //            lblStatus.ForeColor = Color.Red;

                //        }

                //    }

                //    catch (Exception ex)
                //    {

                //    }
                //}

                //#endregion DELETE
            }
            catch (Exception ex)
            {

            }

        }
        # endregion GrdPartnerList_RowCommand

        # region GrdPartnerList_RowEditing
        protected void GrdPartnerList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion GrdPartnerList_RowEditing

        # region GrdPartnerList_PageIndexChanging
        protected void GrdPartnerList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsnew = null;
            try
            {
                dsnew = (DataSet)Session["ds"];
                GrdPartnerList.PageIndex = e.NewPageIndex;
                GrdPartnerList.DataSource = dsnew.Tables[0];
                GrdPartnerList.DataBind();


                //for (int j = 0; j < GrdPartnerList.Rows.Count; j++)
                //{
                //    if (((Label)(GrdPartnerList.Rows[j].FindControl("lblAct"))).Text.ToString() == "True")
                //    {
                //        ((Label)(GrdPartnerList.Rows[j].FindControl("lblAct"))).Text = "Active";
                //    }

                //    else if (((Label)(GrdPartnerList.Rows[j].FindControl("lblAct"))).Text.ToString() == "False")
                //    {
                //        ((Label)(GrdPartnerList.Rows[j].FindControl("lblAct"))).Text = "InActive";
                //    }
                //}
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GrdPartnerList_PageIndexChanging

        #region List Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dtTemp;
                if (txtValidFrom.Text != "" && !DateTime.TryParse(txtValidFrom.Text, out dtTemp))
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Please Put Valid Datetime or Leave It Blank!";
                    return;
                }

                if (txtValidTill.Text != "" && !DateTime.TryParse(txtValidTill.Text, out dtTemp))
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Please Put Valid Datetime or Leave It Blank!";
                    return;
                }
                
                PartnerBAL objPartner = new PartnerBAL();  // Create Partner BAL..
                DataSet ds = objPartner.ListPartners(drpPartnerType.SelectedItem.Text, txtPartnerName.Text, txtPartnerCode.Text, txtEmailID.Text, txtSitaID.Text, txtValidFrom.Text, txtValidTill.Text);
                Session["dsExp"] = ds;

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        lblStatus.Text = "";
                    }
                    else
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        lblStatus.Text = "No Records Found!";
                    }
                    
                    GrdPartnerList.DataSource = ds;
                    GrdPartnerList.DataBind();
                    GrdPartnerList.Columns[13].Visible = true;
                    GrdPartnerList.Columns[14].Visible = true;
                }
                else
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Error in listing partners!";
                }
                objPartner = null;

            }
            catch (Exception Ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in List Partners!";
            }
        }
        #endregion


        #region Export Button
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;
            lblStatus.Text = "";
            //Session["dsExp"] = null;

            try
            {
                if ((DataSet)Session["dsExp"] == null)
                {
                    return;
                }

                ds = (DataSet)Session["dsExp"];
                dt = (DataTable)ds.Tables[0];

                if (Session["dsExp"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                string attachment = "attachment;filename=Report.xls";
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
            catch (Exception ex)
            {

            }
            finally
            {
                ds = null;
                dt = null;
            }
        }

        #endregion

        #region Button Clear
        protected void btnClear_Click1(object sender, EventArgs e)
        {
            //drpPartnerType.SelectedIndex = 0;
            //txtEmailID.Text = string.Empty;
            //txtPartnerCode.Text = string.Empty;
            //txtPartnerName.Text = txtSitaID.Text = txtValidFrom.Text = txtValidTill.Text = string.Empty;
            ////chkbIsScheduled.Checked = false;
            //lblStatus.Text = string.Empty;
            Response.Redirect("~/ListPartner.aspx");

        }

        #endregion


        #region Add New Row to Grid
        public void LoadGridUserList()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "SrNo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "PartnerPrefix";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "PartnerCode";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "PartnerType";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "SITAiD";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "EmailiD";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ZoneId";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "BillingCurrency";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ListingCurrency";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "SettlementMethod";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "RegistrationID";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "TaxRegistrationID";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AdditionalTaxRegID";
            myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            dr["SrNo"] = "";
            dr["PartnerPrefix"] = "";
            dr["PartnerCode"] = "";
            dr["PartnerType"] = "";
            dr["SITAiD"] = "";
            dr["EmailiD"] = "";
            dr["ZoneId"] = "";
            dr["BillingCurrency"] = "";
            dr["ListingCurrency"] = "";
            dr["SettlementMethod"] = "";
            dr["RegistrationID"] = "";
            dr["TaxRegistrationID"] = "";
            dr["AdditionalTaxRegID"] = "";

            myDataTable.Rows.Add(dr);

            GrdPartnerList.DataSource = null;
            GrdPartnerList.DataSource = myDataTable;
            GrdPartnerList.DataBind();
            GrdPartnerList.Columns[13].Visible = false;
            GrdPartnerList.Columns[14].Visible = false;
            Session["dtUserInfo"] = myDataTable.Copy();

        }
        #endregion

        //protected Boolean IsVisible(string partnerType)
        //{
        //    bool flag = false;
        //    if (partnerType.Length > 0)
        //        flag = true;
        //    return flag;
        //}

        //protected void GrdPartnerList_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        //if (e.Row.RowType == DataControlRowType.DataRow)
        //        //{
        //        //    // Hide the edit button when some condition is true
        //        //    // for example, the row contains a certain property
        //        //    Label pcode = (Label)e.Row.FindControl("lblCode");
        //        //    if (pcode.Text.Length > 1)
        //        //    {
        //        //        e.Row.Cells[12].Visible = false;
        //        //        e.Row.Cells[13].Visible = false;

        //        //    }
        //        //}
        //    }
        //    catch (Exception ex) { }
        //}
    }
}
