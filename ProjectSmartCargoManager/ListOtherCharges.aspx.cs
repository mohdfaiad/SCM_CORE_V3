using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class ListOtherCharges : System.Web.UI.Page
    {

        ListOtherChargesBAL objBAL = new ListOtherChargesBAL();

        # region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OriginList();
                DestinationList();
                //btnExport.Enabled = false;
                //FillGetListOC_ALL();
                //LoadChargeType();
                //LoadCurrency();

                //string agentCode = Convert.ToString(Session["ACode"]);
                //if (agentCode != "")
                //{
                //    GRDList.Columns[14].Visible = false;
                //}
               //txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
              //txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

                HomeBL objHome = new HomeBL();
                int RoleId = Convert.ToInt32(Session["RoleID"]);
                DataSet objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
                objHome = null;

                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
                    {
                        if (objDS.Tables[0].Rows[i]["ControlId"].ToString() == "OtherChargeEdit")
                        {
                            GRDList.Columns[14].Visible = false;
                            break;
                        }
                    }
                }
                objDS = null;

                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    GRDList.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
            }
        }
        # endregion Page_Load

        # region FillGetListOC_ALL
        public void FillGetListOC_ALL()
        {
            try
            {
                DataSet ds = objBAL.GetListOC_ALL();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            GRDList.DataSource = ds;
                            GRDList.DataMember = ds.Tables[0].TableName;
                            GRDList.DataBind();
                            Session["ExportOtherCharges"] = ds;
                        }
                    }
                }
            }

            catch
            {

            }
        }

        # endregion FillGetListRateLine_ALL

        #region Origin List

        private void OriginList()
        {
            try
            {
                DataSet ds = objBAL.GetOriginList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }



        #endregion Origin List

        #region Destination List

        private void DestinationList()
        {
            try
            {
                DataSet ds = objBAL.GetDestinationList(ddlDestination.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlDestination.DataBind();
                            ddlDestination.Items.Insert(0, new ListItem("Select", string.Empty));

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Destination List
        #region UpdateOC
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string sr_no = "";
            DateTime dtfrm, dtTo;
            string[] param = new string[3];
            SqlDbType[] dbtypes = new SqlDbType[3];
            object[] values = new object[3];
            if (string.IsNullOrEmpty(txtUpdtFromDate.Text.ToString()) && string.IsNullOrEmpty(txtUpdtToDate.Text.ToString()))
            {
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.Text = "Please enter Valid From To date";
                lblUpdateStatus.ForeColor = Color.Red;
                return;
            }
            if (!string.IsNullOrEmpty(txtUpdtFromDate.Text.ToString()))
            {
                try
                {
                    dtfrm = DateTime.ParseExact(txtUpdtFromDate.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    lblUpdateStatus.Visible = true;
                    lblUpdateStatus.Text = "Date format error";
                    return;

                }
            }
            else
            {
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.Text = "Please enter Valid From date";
                lblUpdateStatus.ForeColor = Color.Red;
                return;
            }

            if (!string.IsNullOrEmpty(txtUpdtToDate.Text.ToString()))
            {
                try
                {
                    dtTo = DateTime.ParseExact(txtUpdtToDate.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    lblUpdateStatus.Visible = true;
                    lblUpdateStatus.Text = "Date format error";
                    return;
                }
            }
            else
            {
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.Text = "Please enter Valid To date";
                lblUpdateStatus.ForeColor = Color.Red;
                return;
            }
            //if (grdListStock.DataSource != null)
            //{
            DataSet ds = (DataSet)Session["LstOtherCharges"];
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                CheckBox chkal = (CheckBox)GRDList.HeaderRow.FindControl("checkAll");
                if (!chkal.Checked)
                {
                    for (int i = 0; i < GRDList.Rows.Count; i++)
                    {
                        CheckBox chkBox = (CheckBox)GRDList.Rows[i].FindControl("chkUpdate");
                        if (chkBox.Checked)
                        {
                            LinkButton lbSrNo = new LinkButton();
                            lbSrNo = (LinkButton)GRDList.Rows[i].FindControl("lblSrNo");
                            sr_no = sr_no + lbSrNo.Text + ",";
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        // CheckBox chkBox  = (CheckBox)grdListStock.Rows[i].FindControl("chkUpdate");
                        //
                        //LinkButton lbSrNo = new LinkButton();
                        //lbSrNo = (LinkButton)grdListStock.Rows[i].FindControl("lblSrNo");
                        sr_no = sr_no + ds.Tables[0].Rows[i]["SerialNumber"].ToString() + ",";
                        //sr_no = sr_no + lbSrNo.Text + ",";


                    }
                }
            }

            else
            {
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.Text = "Other Charges updation failed";
                lblUpdateStatus.ForeColor = Color.Red;
                return;
            }
            if (sr_no == null || sr_no == "")
            {
                lblUpdateStatus.Visible = true;
                lblUpdateStatus.Text = "Please select OC id to update";
                lblUpdateStatus.ForeColor = Color.Red;
            }
            else
            {

                values[0] = sr_no;
                values[1] = dtfrm;
                values[2] = dtTo;

                bool val = objBAL.UpdatOtherCharges(values);
                if (val == true)
                {
                    lblUpdateStatus.Visible = true;
                    lblUpdateStatus.Text = "Other Charges Updated Successfully";
                    lblUpdateStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblUpdateStatus.Visible = true;
                    lblUpdateStatus.Text = "Other Charges updation failed";
                    lblUpdateStatus.ForeColor = Color.Red;
                    return;
                }
                #region For Master Audit Log
                MasterAuditBAL ObjMAL = new MasterAuditBAL();
                #region Prepare Parameters
                object[] Params = new object[7];
                int j = 0;

                //1
                Params.SetValue("Other Charges", j);
                j++;

                //2
                string Value = txtUpdtFromDate.Text + "-" + txtUpdtToDate.Text;
                Params.SetValue(Value, j);
                j++;

                //3

                Params.SetValue("UPDATE", j);
                j++;

                //4

                Params.SetValue("", j);
                j++;


                //5

                Params.SetValue("Updation of Valid from and Valid to dates", j);
                j++;

                //6

                Params.SetValue(Session["UserName"], j);
                j++;

                //7
                Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), j);
                j++;


                #endregion Prepare Parameters
                ObjMAL.AddMasterAuditLog(Params);
                #endregion For Master Audit Log
                //code to update dates for specified srNo's

            }
        }

#endregion UpdateOC 
        # region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {

            try
            {
                //if (txtFromDate.Text == "" || txtToDate.Text == "")
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please Enter From Date and To Date ";
                //    txtFromDate.Focus();
                //    return;
                //}
                if (txtFromDate.Text.Trim() != "" || txtToDate.Text.Trim() != "")
                {
                    DateTime dt1 = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);


                    int chk = DateTime.Compare(dt1, dt2);
                    if (chk > 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To date";

                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                txtFromDate.Focus();
                return;
            }


            #region Prepare Parameters
            object[] RateLineInfo = new object[17];
            int i = 0;
            RateLineInfo.SetValue(ddlOrigin.SelectedValue, i);

            i++;
            RateLineInfo.SetValue(ddlDestination.SelectedValue, i);

            i++;


            DateTime frdt;
            if (txtFromDate.Text.ToString() != "")
            {
                try
                {
                    frdt = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                string fromdate = frdt.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(fromdate, i); 
                i++;
            }
            else
            {
                RateLineInfo.SetValue("", i);
                i++;
            }
           

            DateTime todt;

            if (txtToDate.Text != "")
            {
                try
                {
                    todt = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                string todate = todt.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(todate, i);
                i++;
            }
            else
            {
                RateLineInfo.SetValue("", i);
                i++;
            }
      

            RateLineInfo.SetValue(ddlStatus.SelectedValue, i);
            i++;
            RateLineInfo.SetValue("", i);
            i++;


            RateLineInfo.SetValue(DDLChargeType.SelectedValue, i);

            i++;

            RateLineInfo.SetValue(txtChargeName.Text, i);
            i++;

            RateLineInfo.SetValue(txtParam.Text, i);
            i++;


            RateLineInfo.SetValue(txtagent.Text, i);
            i++;

            RateLineInfo.SetValue(txtShipper.Text, i);
            i++;
            RateLineInfo.SetValue(txtComm.Text, i);

            i++;

            RateLineInfo.SetValue(txtproduct.Text, i);

            i++;

            //Expires Dates
            if (txtExpfrm.Text.Trim() != "")
            {
                DateTime dtfrom;

                try
                {
                   // dtfrom = Convert.ToDateTime(txtExpfrm.Text);
                    dtfrom = DateTime.ParseExact(txtExpfrm.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtExpfrm.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string Frmdate = dtfrom.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(Frmdate, i);
            }
            else
            {
                RateLineInfo.SetValue("", i);
            }



            i++;

            if (txtExpTo.Text.Trim() != "")
            {
                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtExpTo.Text);
                    dtto = DateTime.ParseExact(txtExpTo.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtExpTo.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string Todate = dtto.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(Todate, i);
            }
            else
            {
                RateLineInfo.SetValue("", i);
            }

            i++;
            if (ddlChargedAt.SelectedIndex == 1)
                RateLineInfo.SetValue("Dep", i);
            else
                if (ddlChargedAt.SelectedIndex == 2)
                    RateLineInfo.SetValue("Del", i);
                else
                    RateLineInfo.SetValue("", i);

            i++;
            RateLineInfo.SetValue(txtOCID.Text.Trim(), i);
            i++;


            #endregion Prepare Parameters


            DataSet ds = objBAL.GetListRateLine(RateLineInfo);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GRDList.DataSource = ds;
                            GRDList.DataMember = ds.Tables[0].TableName;
                            GRDList.DataBind();
                            Session["LstOtherCharges"] = ds;
                            //btnExport.Enabled = true;
                            lblStatus.Text = "";
                            GRDList.Visible = true;
                            lblUpdateStatus.Text = "";
                            txtUpdtFromDate.Text = "";
                            txtUpdtToDate.Text = "";
                        }
                        else
                        {
                            lblStatus.Text = "No Data Found";
                            lblStatus.ForeColor = Color.Red;
                            GRDList.Visible = false;
                            lblUpdateStatus.Text = "";
                            //btnExport.Enabled = false;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No Data Found";
                        lblStatus.ForeColor = Color.Red;
                        GRDList.Visible = false;
                        lblUpdateStatus.Text = "";
                    }

                }
            }
        }

        # endregion btnList_Click

        #region Get Data to Export
        public string GetData()
        {
            string values = "";

            try
            {
                //if (txtFromDate.Text == "" || txtToDate.Text == "")
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please Enter From Date and To Date ";
                //    txtFromDate.Focus();
                //    return;
                //}
                if (txtFromDate.Text.Trim() != "" || txtToDate.Text.Trim() != "")
                {
                    DateTime dt1 = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);


                    int chk = DateTime.Compare(dt1, dt2);
                    if (chk > 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To date";

                        return null ;
                    }

                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                txtFromDate.Focus();
                return null;
            }


            #region Prepare Parameters
            object[] RateLineInfo = new object[17];
            int i = 0;
            RateLineInfo.SetValue(ddlOrigin.SelectedValue, i);

            i++;
            RateLineInfo.SetValue(ddlDestination.SelectedValue, i);

            i++;


            DateTime frdt;
            if (txtFromDate.Text.ToString() != "")
            {
                try
                {
                    frdt = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return null;
                }
                string fromdate = frdt.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(fromdate, i);
                i++;
            }
            else
            {
                RateLineInfo.SetValue("", i);
                i++;
            }


            DateTime todt;

            if (txtToDate.Text != "")
            {
                try
                {
                    todt = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return null;
                }
                string todate = todt.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(todate, i);
                i++;
            }
            else
            {
                RateLineInfo.SetValue("", i);
                i++;
            }


            RateLineInfo.SetValue(ddlStatus.SelectedValue, i);
            i++;
            RateLineInfo.SetValue("", i);
            i++;


            RateLineInfo.SetValue(DDLChargeType.SelectedValue, i);

            i++;

            RateLineInfo.SetValue(txtChargeName.Text, i);
            i++;

            RateLineInfo.SetValue(txtParam.Text, i);
            i++;


            RateLineInfo.SetValue(txtagent.Text, i);
            i++;

            RateLineInfo.SetValue(txtShipper.Text, i);
            i++;
            RateLineInfo.SetValue(txtComm.Text, i);

            i++;

            RateLineInfo.SetValue(txtproduct.Text, i);

            i++;

            //Expires Dates
            if (txtExpfrm.Text.Trim() != "")
            {
                DateTime dtfrom;

                try
                {
                    // dtfrom = Convert.ToDateTime(txtExpfrm.Text);
                    dtfrom = DateTime.ParseExact(txtExpfrm.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return null;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtExpfrm.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string Frmdate = dtfrom.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(Frmdate, i);
            }
            else
            {
                RateLineInfo.SetValue("", i);
            }



            i++;

            if (txtExpTo.Text.Trim() != "")
            {
                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtExpTo.Text);
                    dtto = DateTime.ParseExact(txtExpTo.Text, "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return null;
                }

                //RateLineInfo.SetValue(Convert.ToDateTime(txtExpTo.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                string Todate = dtto.ToString("MM/dd/yyyy");
                RateLineInfo.SetValue(Todate, i);
            }
            else
            {
                RateLineInfo.SetValue("", i);
            }

            i++;
            if (ddlChargedAt.SelectedIndex == 1)
                RateLineInfo.SetValue("Dep", i);
            else
                if (ddlChargedAt.SelectedIndex == 2)
                    RateLineInfo.SetValue("Del", i);
                else
                    RateLineInfo.SetValue("", i);

            i++;
            RateLineInfo.SetValue(txtOCID.Text.Trim(), i);
            i++;


            #endregion Prepare Parameters


            DataSet ds = objBAL.GetListRateLine(RateLineInfo);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    //extract the TextBox values
                                    //LinkButton lblSrNo = (LinkButton)grdListStock.Rows[j].Cells[0].FindControl("lblSrNo");
                                    //values = values + lblSrNo.Text + ","; ;
                                    values = values + (string)(ds.Tables[0].Rows[j][0].ToString()) + ",";

                                }
                            
                        }
                        else
                        {
                            lblStatus.Text = "No Data Found";
                            lblStatus.ForeColor = Color.Red;
                            GRDList.Visible = false;
                        }
                    }
                }
            }
            if (values != null)
                return values;
            else
                return null;
        }
        #endregion

        #region grid row commmand "Edit" and "View"

        protected void GRDList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit" || e.CommandName == "View")
                {
                    string SrNo = ((HiddenField)GRDList.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("Hid")).Value;
                    Response.Redirect("~/OtherCharges.aspx?cmd=" + e.CommandName + "&SrNo=" + SrNo + "");
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }


        #endregion grid row commmand "Edit" and "View"

        protected void GRDList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult = (DataSet)Session["LstOtherCharges"];

            GRDList.PageIndex = e.NewPageIndex;
            GRDList.DataSource = dsResult.Copy();
            GRDList.DataBind();
           // btnExport.Enabled = true;

        }

        //#region ChargeType
        //private void LoadChargeType()
        //{
        //    try
        //    {
        //        OtherChargesBAL objBALL = new OtherChargesBAL();
        //        DataSet oth = objBALL.GetChargeType();
        //        if (oth != null)
        //        {
        //            if (oth.Tables.Count > 0)
        //            {
        //                DDLCharge.DataSource = oth.Tables[0];
        //                DDLCharge.DataTextField = "ChargeText";
        //                DDLCharge.DataValueField = "ChargeValue";
        //                DDLCharge.DataBind();
        //                DDLCharge.Items.Insert(0, new ListItem("Select", string.Empty));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //}
        //#endregion

        #region ddldestinationLoad
        protected void DDLDestinationLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
            DataSet dsResult = new DataSet();
            string errormessage = "";

            string level = DDLDestinationLevel.SelectedItem.Value;
            if (objDataViewBAL.FillDdl("Origin", ref dsResult, ref errormessage, new string[] { level }))
            {
                ddlDestination.DataSource = dsResult;
                ddlDestination.DataMember = dsResult.Tables[0].TableName;
                ddlDestination.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlDestination.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlDestination.DataBind();
                ddlDestination.Items.Insert(0, new ListItem("Select", string.Empty));
            }

        }
        #endregion

        #region ddloriginLoad
        protected void DDLOriginLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListDataViewBAL objDataViewBAL = new ListDataViewBAL();
            DataSet dsResult = new DataSet();
            string errormessage = "";

            string level = DDLOriginLevel.SelectedItem.Value;
            if (objDataViewBAL.FillDdl("Origin", ref dsResult, ref errormessage, new string[] { level }))
            {
                ddlOrigin.DataSource = dsResult;
                ddlOrigin.DataMember = dsResult.Tables[0].TableName;
                ddlOrigin.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlOrigin.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlOrigin.DataBind();
                ddlOrigin.Items.Insert(0, new ListItem("Select", string.Empty));
            }

        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string values = "";
            DataSet ds = null;
            DataTable dt = null;
            //Session["Rateline"] = null;
            try
            {
                values = GetData();
                if (values != null &&values!="")
                {
                    GetExportData(values);
                    if (Session["ExportOtherCharges"] != null)
                    {
                        ds = (DataSet)Session["ExportOtherCharges"];
                        dt = (DataTable)ds.Tables[0];
                        string attachment = "attachment; filename=OtherCharges.xls";
                        Response.ClearContent();
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.AddHeader("content-disposition", attachment);
                        Response.ContentType = "application/vnd.ms-excel";
                        string tab = "";
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.ToString() != "Serialnumber")
                            {
                                Response.Write(tab + dc.ColumnName);
                                //tab = "\t";
                                tab = "\t";
                            }
                        }
                        Response.Write("\n");
                        int i;
                        foreach (DataRow dr in dt.Rows)
                        {
                            tab = "";
                            for (i = 0; i < dt.Columns.Count; i++)
                            {
                                if (dr[i].ToString().Trim() == ",")
                                    dr[i] = "";
                               
                                Response.Write(tab + dr[i].ToString());
                                //tab = "\t";
                                tab = "\t";
                            }
                            Response.Write("\n");
                        }
                        Response.End();

                    }
                    else
                    {
                        lblStatus.Text = "No record found";
                        lblStatus.ForeColor = Color.Red;
                        //btnExport.Enabled = false;

                    }
                }
                else
                {
                    lblStatus.Text = "No record found";
                    lblStatus.ForeColor = Color.Red;
                }

            }

            catch (Exception ex)
            {

            }
            finally
            {
                ds = null;
                Session["ExportOtherCharges"] = null;
                dt = null;
            }

        }
        public void GetExportData(string val)
        {
            DataSet ds = objBAL.getExpOC(val);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //grdListStock.DataSource = ds;
                            //grdListStock.DataMember = ds.Tables[0].TableName;
                            //grdListStock.DataBind();
                            //grdListStock.Visible = true;
                            //grdListStock.Columns[0].Visible = false;
                            //btnClear_Click(null, null);
                            Session["ExportOtherCharges"] = ds;
                            lblStatus.Text = "";
                        }
                        else
                        {
                            lblStatus.Text = "No record available for given Search criteria";
                            lblStatus.ForeColor = Color.Red;
                            GRDList.DataMember = null;
                            GRDList.DataBind();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No record available for given Search criteria";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblStatus.Text = "No record available for given Search criteria";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                lblStatus.Text = "No record available for given Search criteria";
                lblStatus.ForeColor = Color.Red;
            }
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ListOtherCharges.aspx",false);
        }

        protected void GRDList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
        //#region Currency
        //private void LoadCurrency()
        //{
        //    try
        //    {
        //        //OtherChargesBAL objBALL = new OtherChargesBAL();
        //        DataSet dsoth = new DataSet();
        //        dsoth = objBAL.GetCurrencyCode(ddlCurrency.SelectedItem);
        //        if (dsoth != null)
        //        {
        //            if (dsoth.Tables.Count > 0)
        //            {
        //                DDLCharge.DataSource = dsoth.Tables[0];
        //                DDLCharge.DataTextField = "Currency";
        //                DDLCharge.DataValueField = "CurrencyValue";
        //                DDLCharge.DataBind();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //}
        //#endregion
    }
}
