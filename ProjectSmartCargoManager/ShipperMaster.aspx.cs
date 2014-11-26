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
using System.Configuration;

namespace ProjectSmartCargoManager
{
    public partial class ShipperMaster : System.Web.UI.Page
    {
        BALShipperMaster objBal = new BALShipperMaster();
        AgentBAL objBLL = new AgentBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                btnExport.Enabled = false;
                LoadCCSFCode();
                LoadAWBMasterData();
            }
        }

        #region Button Save
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                MasterAuditBAL ObjMAL = new MasterAuditBAL();
                # region Save
                if (btnSave.Text == "Save")
                {
                    try
                    {
                        //if (ddlType.SelectedIndex == 0)
                        //{
                        //    lblStatus.Text = "Select Participation Type";
                        //    lblStatus.ForeColor = Color.Red;
                        //    return;
                        //}
                        //else if (ddlType.SelectedIndex > 0)
                        //{

                            #region Prepare Parameters
                            object[] Params = new object[24];
                            int i = 0;
                            string uname = Session["UserName"].ToString();
                            DateTime time = DateTime.Parse(Session["IT"].ToString());

                            //1
                            Params.SetValue(txtAccCode.Text, i);
                            i++;

                            //2
                            Params.SetValue(txtAccName.Text, i);
                            i++;

                            //3
                            Params.SetValue(txtAdr1.Text, i);
                            i++;

                            //4
                            Params.SetValue(txtAdr2.Text, i);
                            i++;

                            //5
                            Params.SetValue(txtCity.Text, i);
                            i++;

                            //6
                            Params.SetValue(txtState.Text, i);
                            i++;

                            //7
                            Params.SetValue(ddlCountry.SelectedValue, i);
                            i++;

                            //8
                            if (!string.IsNullOrEmpty(txtZipCode.Text))
                            {
                                int zip = int.Parse(txtZipCode.Text);
                                Params.SetValue(zip, i);
                            }
                            else
                                Params.SetValue(0, i);
                            i++;

                            //9
                            Params.SetValue(txtPhNo.Text, i);
                            i++;

                            //10
                            Params.SetValue(txtMobNo.Text, i);
                            i++;

                            //11
                            Params.SetValue(txtFax.Text, i);
                            i++;

                            //12
                            Params.SetValue(txtEmail.Text, i);
                            i++;

                            //13
                            Params.SetValue(txtIATANo.Text, i);
                            i++;

                            //14
                            if (chkAct.Checked == true)
                            {
                                Params.SetValue(true, i);
                                i++;
                            }
                            else
                            {
                                Params.SetValue(false, i);
                                i++;
                            }

                            //15
                            Params.SetValue(time, i);
                            i++;


                            //16
                            Params.SetValue(uname, i);
                            i++;

                            //17
                            //Params.SetValue(time, i);
                            //i++;

                            //18
                            //Params.SetValue(uname, i);
                            //i++;

                            //19
                        if (ddlType.SelectedIndex == 0)
                            Params.SetValue("Shipper",i);
                        else
                            Params.SetValue(ddlType.SelectedItem.Text, i);
                            i++;

                            //20
                            Params.SetValue(TXTAgentCode.Text, i);
                            i++;

                            //21
                            if (ddlCCSFCode.SelectedIndex == 0)
                                Params.SetValue("", i);
                            else
                                Params.SetValue(ddlCCSFCode.SelectedItem.Text, i);
                            i++;

                            //22
                            Params.SetValue(txtCreditAccNo.Text, i);
                            i++;
                            //23
                            if (chkKnownShipper.Checked == true)
                            {
                                Params.SetValue(true, i);
                                i++;
                            }
                            else
                            {
                                Params.SetValue(false, i);
                                i++;
                            }
                            //newly added parameters
                            //24
                            Params.SetValue(txttin.Text, i);

                            i++;

                            //25
                            Params.SetValue(txtperson.Text, i);
                            i++;
                            //26
                            if (chkVAT.Checked == true)
                            {
                                Params.SetValue(true, i);
                                i++;
                            }
                            else
                            {
                                Params.SetValue(false, i);
                                i++;
                            }

                            #endregion Prepare Parameters

                            int ID = 0;
                            ID = objBal.AddShipperDetail(Params);
                            if (ID >= 0)
                            {

                                #region for Master Audit Log
                                #region Prepare Parameters
                                object[] Paramsmaster = new object[7];
                                int count = 0;

                                //1

                                Paramsmaster.SetValue("Shipper-Consignee Master", count);
                                count++;

                                //2
                                Paramsmaster.SetValue(txtAccCode.Text, count);
                                count++;

                                //3

                                Paramsmaster.SetValue("SAVE", count);
                                count++;

                                //4
                                Paramsmaster.SetValue("New Shipper Consignee", count);
                                count++;


                                //5
                                string Desc = "ShipperCode:" + txtAccCode.Text + "/ShipperName:" + txtAccName.Text + "/Type:" + ddlType.SelectedItem.Text;
                                Paramsmaster.SetValue(Desc, count);
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

                                //btnClear_Click(null, null);
                                btnList_Click(null, null);

                                lblStatus.Text = "Record Added Successfully";
                                lblStatus.ForeColor = Color.Green;
                                btnSave.Text = "Save";

                            }
                            else
                            {
                                btnList_Click(null, null);
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Record Insertion Failed..";

                            }
                       // }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                # endregion Save

                # region Update
                if (btnSave.Text == "Update")
                {
                    lblStatus.Text = "";

                    try
                    {
                        //if (ddlType.SelectedIndex == 0)
                        //{
                        //    lblStatus.ForeColor = Color.Red;
                        //    lblStatus.Text = "Select Participation Type";
                        //    return;
                        //}
                        //else if (ddlType.SelectedIndex > 0)
                        //{
                            #region Prepare Parameters
                            object[] Params = new object[27];
                            int i = 0;
                            string uname = Session["UserName"].ToString();
                            DateTime time = DateTime.Parse(Session["IT"].ToString());
                            long serialnumber = long.Parse(Session["SrNum"].ToString());
                            DateTime creationtime = DateTime.Parse(Session["IT"].ToString());
                            string createdby = Session["UserName"].ToString();

                            //1
                            Params.SetValue(txtAccCode.Text, i);
                            i++;

                            //2
                            Params.SetValue(txtAccName.Text, i);
                            i++;

                            //3
                            Params.SetValue(txtAdr1.Text, i);
                            i++;

                            //4
                            Params.SetValue(txtAdr2.Text, i);
                            i++;

                            //5
                            Params.SetValue(txtCity.Text, i);
                            i++;

                            //6
                            Params.SetValue(txtState.Text, i);
                            i++;

                            //7
                            Params.SetValue(ddlCountry.SelectedValue, i);
                            i++;

                            //8
                            if (!string.IsNullOrEmpty(txtZipCode.Text))
                            {
                                int zip = int.Parse(txtZipCode.Text);
                                Params.SetValue(zip, i);
                            }
                            else
                                Params.SetValue(0, i);
                            i++;

                            //9
                            Params.SetValue(txtPhNo.Text, i);
                            i++;

                            //10
                            Params.SetValue(txtMobNo.Text, i);
                            i++;

                            //11
                            Params.SetValue(txtFax.Text, i);
                            i++;

                            //12
                            Params.SetValue(txtEmail.Text, i);
                            i++;

                            //13
                            Params.SetValue(txtIATANo.Text, i);
                            i++;

                            //14
                            if (chkAct.Checked == true)
                            {
                                Params.SetValue(true, i);
                                i++;
                            }
                            else
                            {
                                Params.SetValue(false, i);
                                i++;
                            }

                            //15
                            Params.SetValue(creationtime, i);
                            i++;


                            //16
                            Params.SetValue(createdby, i);
                            i++;

                            //17
                            Params.SetValue(time, i);
                            i++;

                            //18
                            Params.SetValue(uname, i);
                            i++;

                            //19
                            Params.SetValue(serialnumber, i);
                            i++;

                            //20

                            if (ddlType.SelectedIndex == 0)
                                Params.SetValue("Shipper", i);
                            else
                                Params.SetValue(ddlType.SelectedItem.Text, i);
                            i++;

                            //21
                            Params.SetValue(TXTAgentCode.Text, i);
                            i++;

                            //22
                            if (ddlCCSFCode.SelectedIndex == 0)
                                Params.SetValue("", i);
                            else
                                Params.SetValue(ddlCCSFCode.SelectedItem.Text, i);
                            i++;

                            //23
                            Params.SetValue(txtCreditAccNo.Text, i);
                            i++;

                            if (chkKnownShipper.Checked == true)
                            {
                                Params.SetValue(true, i);
                                i++;
                            }
                            else
                            {
                                Params.SetValue(false, i);
                                i++;
                            }

                            //newly added parameters
                            //24
                            Params.SetValue(txttin.Text, i);
                            i++;

                            //25
                            Params.SetValue(txtperson.Text, i);
                            i++;

                            //26
                            if (chkVAT.Checked == true)
                            {
                                Params.SetValue(true, i);
                                i++;
                            }
                            else
                            {
                                Params.SetValue(false, i);
                                i++;
                            }

                            #endregion Prepare Parameters

                            int ID = 0;
                            ID = objBal.UpdateShipperDetail(Params);
                            if (ID >= 0)
                            {
                                #region for Master Audit Log
                                #region Prepare Parameters
                                object[] Paramsmaster = new object[7];
                                int count = 0;

                                //1

                                Paramsmaster.SetValue("Shipper-Consignee Master", count);
                                count++;

                                //2
                                Paramsmaster.SetValue(txtAccCode.Text, count);
                                count++;

                                //3

                                Paramsmaster.SetValue("UPDATE", count);
                                count++;

                                //4
                                Paramsmaster.SetValue("Shipper Consignee Update", count);
                                count++;


                                //5
                                string Desc = "ShipperCode:" + txtAccCode.Text + "/ShipperName:" + txtAccName.Text + "/Type:" + ddlType.SelectedItem.Text;
                                Paramsmaster.SetValue(Desc, count);
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


                                btnList_Click(null, null);

                                lblStatus.Visible = true;
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
                       // }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                # endregion Save
            }

            catch (Exception ex)
            {

            }
        }
        #endregion
       
        #region Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtAccCode.Text = string.Empty; txtAccName.Text = string.Empty;
            txtAdr1.Text = string.Empty; txtAdr2.Text = string.Empty;
            txtCity.Text = string.Empty; //txtCountry.Text = string.Empty;
            txtEmail.Text = string.Empty; txtFax.Text = string.Empty;
            txtIATANo.Text = string.Empty; txtMobNo.Text = string.Empty;
            txtPhNo.Text = string.Empty; txtState.Text = string.Empty;
            txtZipCode.Text = string.Empty;
            TXTAgentCode.Text = string.Empty;
            TXTCustomerCode.Text = string.Empty;
            txtAgentName.Text = string.Empty;
            txtperson.Text = string.Empty;
            txttin.Text  = string.Empty;
            chkAct.Checked = false;
            chkVAT.Checked = false;
            ddlType.SelectedIndex = 0;
            lblStatus.Text = string.Empty;
            btnSave.Text = "Save";
            txtCreditAccNo.Text = string.Empty;
            ddlCCSFCode.SelectedIndex = 0;
            chkKnownShipper.Checked = false;
            GrdShipper.DataSource = null;
            GrdShipper.DataBind();
            btnExport.Enabled = false;

            try
            {
                string countrycode = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "DefaultCountry");
                ddlCountry.SelectedValue = countrycode;
            }
            catch (Exception ex) { }

        }
        #endregion Clear

        #region Shipper List
        protected void btnList_Click(object sender, EventArgs e)
        {
            btnExport.Enabled = true;
            try
            {
                lblStatus.Text = "";
                #region Prepare Parameters

                object[] Params = new object[3];
                int i = 0;

                //1
                string acccode = null;
                if (txtAccCode.Text == "")
                    acccode = "All";
                else
                    acccode = txtAccCode.Text;
                Params.SetValue(acccode, i);
                i++;

                //2
                string accname = null;
                if (txtAccName.Text == "")
                    accname = "All";
                else
                   // acccode = txtAccName.Text;
                accname = txtAccName.Text;
                Params.SetValue(accname, i);
                i++;

                //3
                string type = null;
                if (ddlType.SelectedIndex == 0)
                    type = "All";
                else type = ddlType.SelectedItem.Text;
                Params.SetValue(type, i);
                i++;

                #endregion Prepare Parameters



                DataSet ds = new DataSet();
                ds = objBal.GetShipperList(Params);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Session["ShipperConsignee"] = ds;
                                btnExport.Enabled = true;
                                //btnClear_Click(null, null);
                                GrdShipper.PageIndex = 0;
                                GrdShipper.DataSource = ds.Tables[0];
                               // GrdShipper.DataMember = ds.Tables[0].TableName;
                                GrdShipper.DataBind();

                                GrdShipper.Visible = true;

                                ViewState["ds"] = ds;
                                //ds.Clear();

                            }
                            else if (ds.Tables[0].Rows.Count == 0)
                            {
                                lblStatus.Text = "No Records Exists";
                                btnExport.Enabled = false;
                                lblStatus.ForeColor = Color.Red;
                                GrdShipper.DataSource = null;
                                GrdShipper.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion Shipper List

        #region ExportShipperList
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataTable dt = null;
            //Session["ShipperConsignee"] = null;
            try
            {
                //GetData();

                if (Session["ShipperConsignee"] == null)
                {
                    lblStatus.Text = "No Record Found";
                    return;
                }
                ds = (DataSet)Session["ShipperConsignee"];
                dt = (DataTable)ds.Tables[1];   
                string attachment = "attachment; filename=ShipperConsignee.xls";
                Response.ClearContent();
                Response.ContentEncoding = System.Text.Encoding.UTF8;
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
                String exception = ex.ToString();
            }
            finally
            {
                ds = null;
                dt = null;
            }


        }
        public void GetData()
        {
            try
            {
                lblStatus.Text = "";
                #region Prepare Parameters

                object[] Params = new object[3];
                int i = 0;

                //1
                string acccode = null;
                if (txtAccCode.Text == "")
                    acccode = "All";
                else
                    acccode = txtAccCode.Text;
                Params.SetValue(acccode, i);
                i++;

                //2
                string accname = null;
                if (txtAccName.Text == "")
                    accname = "All";
                else
                    // acccode = txtAccName.Text;
                    accname = txtAccName.Text;
                Params.SetValue(accname, i);
                i++;

                //3
                string type = null;
                if (ddlType.SelectedIndex == 0)
                    type = "All";
                else type = ddlType.SelectedItem.Text;
                Params.SetValue(type, i);
                i++;

                #endregion Prepare Parameters



                DataSet ds = new DataSet();
                ds = objBal.GetShipperList(Params);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                Session["ShipperConsignee"] = ds;
                                lblStatus.Text = "";
                                //ds.Clear();

                            }
                            else if (ds.Tables[1].Rows.Count == 0)
                            {
                                lblStatus.Text = "No Records Exists";
                                lblStatus.ForeColor = Color.Red;
                                //GrdShipper.DataSource = null;
                                //GrdShipper.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        #endregion

        #region Load CCSF Code
        public void LoadCCSFCode()
        {
            try
            {
                DataSet ds = objBLL.GetCCSFCode();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlCCSFCode.Items.Clear();

                            ddlCCSFCode.DataSource = ds.Tables[0];
                            ddlCCSFCode.DataTextField = "ApprovalNo";
                            ddlCCSFCode.DataValueField = "ApprovalNo";
                            ddlCCSFCode.DataBind();
                            ddlCCSFCode.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

        }
        #endregion

        # region GrdShipper_RowCommand
        protected void GrdShipper_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                MasterAuditBAL ObjMAL = new MasterAuditBAL();


                #region Edit
                if (e.CommandName == "Edit")
                {

                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);

                    Session["SrNum"] = ((Label)(GrdShipper.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString();
                    Label AccCode = (Label)GrdShipper.Rows[RowIndex].FindControl("lblAccCode");
                    Label AccName = (Label)GrdShipper.Rows[RowIndex].FindControl("lblAccName");
                    Label lblAdr1 = (Label)GrdShipper.Rows[RowIndex].FindControl("lblAdr1");
                    Label lblAdr2 = (Label)GrdShipper.Rows[RowIndex].FindControl("lblAdr2");
                    Label lblcity = (Label)GrdShipper.Rows[RowIndex].FindControl("lblCity");
                    Label lblstate = (Label)GrdShipper.Rows[RowIndex].FindControl("lblState");
                    Label lblzip = (Label)GrdShipper.Rows[RowIndex].FindControl("lblZip");
                    Label lblcountry = (Label)GrdShipper.Rows[RowIndex].FindControl("lblCountry");
                    Label lblph = (Label)GrdShipper.Rows[RowIndex].FindControl("lblPhNo");
                    Label lblmob = (Label)GrdShipper.Rows[RowIndex].FindControl("lblMobNo");
                    Label lblfax = (Label)GrdShipper.Rows[RowIndex].FindControl("lblFax");
                    Label lblemail = (Label)GrdShipper.Rows[RowIndex].FindControl("lblEmail");
                    Label lbliatano = (Label)GrdShipper.Rows[RowIndex].FindControl("lblIATANo");
                    Label lblactive = (Label)GrdShipper.Rows[RowIndex].FindControl("lblAct");
                    string type = ((Label)GrdShipper.Rows[RowIndex].FindControl("lblParticipation")).Text;
                    Label lblagentcode = (Label)GrdShipper.Rows[RowIndex].FindControl("lblAgentCode");
                    Session["creationtime"] = ((Label)GrdShipper.Rows[RowIndex].FindControl("lblCreateOn")).Text.ToString();
                    Session["createdby"] = ((Label)GrdShipper.Rows[RowIndex].FindControl("lblCreateBy")).Text.ToString();
                    string CCSFCode = ((Label)GrdShipper.Rows[RowIndex].FindControl("CCSFCode")).Text;
                    string creditaccno = ((Label)GrdShipper.Rows[RowIndex].FindControl("lblCreditAccNo")).Text;
                    Label lblshipper = (Label)GrdShipper.Rows[RowIndex].FindControl("lblshipper");
                    Label tin = (Label)GrdShipper.Rows[RowIndex].FindControl("lbltin");
                    Label lblContact = (Label)GrdShipper.Rows[RowIndex].FindControl("lblContactPerson");
                    Label lblVATExemp = (Label)GrdShipper.Rows[RowIndex].FindControl("lblVATExemption");

                    txtAccCode.Text = AccCode.Text; txtAccName.Text = AccName.Text;
                    txtAdr1.Text = lblAdr1.Text; txtAdr2.Text = lblAdr2.Text; txtCity.Text = lblcity.Text;
                    txtState.Text = lblstate.Text; txtZipCode.Text = lblzip.Text;
                    
                    try
                    {
                        ddlCountry.SelectedValue = lblcountry.Text; txtPhNo.Text = lblph.Text;                        
                    }
                    catch (Exception ex) {
                        string countrycode = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "DefaultCountry");
                        ddlCountry.SelectedValue = countrycode;
                    }

                    txtMobNo.Text = lblmob.Text; txtFax.Text = lblfax.Text; txtEmail.Text = lblemail.Text;
                    txtIATANo.Text = lbliatano.Text;
                    ddlType.SelectedIndex = ddlType.Items.IndexOf(((ListItem)ddlType.Items.FindByText(type)));
                    TXTAgentCode.Text = lblagentcode.Text;
                    ddlCCSFCode.SelectedIndex = ddlCCSFCode.Items.IndexOf(((ListItem)ddlCCSFCode.Items.FindByText(CCSFCode)));
                    txtCreditAccNo.Text = creditaccno;
                    txttin.Text=tin.Text;
                    txtperson.Text = lblContact.Text;
                    if (lblactive.Text == "True")
                    {
                        chkAct.Checked = true;
                    }

                    if (lblactive.Text == "False")
                    {
                        chkAct.Checked = false;
                    }


                    if (lblshipper.Text == "True")
                    {
                     chkKnownShipper.Checked = true;
                    }

                    if (lblshipper.Text == "False")
                    {
                        chkKnownShipper.Checked = false;
                    }
                    if (lblVATExemp.Text == "True")
                    {
                        chkVAT.Checked = true;
                    }

                    if (lblVATExemp.Text == "False")
                    {
                        chkVAT.Checked = false;
                    }
                    btnSave.Text = "Update";
                    btnExport.Enabled = false;

                    # region Agent Details
                    try
                    {
                        #region Prepare Parameters
                        DataSet ds = new DataSet();
                        object[] Params = new object[1];
                        int i = 0;

                        //1
                        Params.SetValue(TXTAgentCode.Text, i);
                        i++;

                        #endregion Prepare Parameters

                        int ID = 0;
                        ds = objBal.GetAgentDetails(Params);
                        if (ds != null)
                        {
                            if (ds.Tables != null)

                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        txtAgentName.Text = ds.Tables[0].Rows[0][0].ToString();
                                        TXTCustomerCode.Text = ds.Tables[0].Rows[0][1].ToString();
                                        //GrdShipper.PageIndex = 0;
                                        //GrdShipper.DataSource = ds;
                                        //GrdShipper.DataMember = ds.Tables[0].TableName;
                                        //GrdShipper.DataBind();
                                        //GrdShipper.Visible = true;
                                        //ds.Clear();

                                    }
                                }
                        }
                    }

                    catch (Exception ex)
                    {

                    }

                    # endregion Agent Details
                }
                #endregion Edit

                #region Delete

                if (e.CommandName == "DeleteRecord")
                {
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    int srno = int.Parse(((Label)(GrdShipper.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString());
                    string code = ((Label)(GrdShipper.Rows[RowIndex].FindControl("lblAccCode"))).Text.ToString();
                    string name = ((Label)(GrdShipper.Rows[RowIndex].FindControl("lblAccName"))).Text.ToString();
                    string type = ((Label)(GrdShipper.Rows[RowIndex].FindControl("lblParticipation"))).Text.ToString();
                    # region Delete
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
                        int res = objBal.DeleteShipperDetail(Params);
                        if (res == 0)
                        {

                            #region for Master Audit Log
                            #region Prepare Parameters
                            object[] Paramsmaster = new object[7];
                            int count = 0;

                            //1

                            Paramsmaster.SetValue("Shipper-Consignee Master", count);
                            count++;

                            //2
                            Paramsmaster.SetValue(code, count);
                            count++;

                            //3

                            Paramsmaster.SetValue("DELETE", count);
                            count++;

                            //4
                            Paramsmaster.SetValue("Shipper Consignee Delete", count);
                            count++;


                            //5
                            string Desc = "ShipperCode:" + code + "/ShipperName:" + name + "/Type:" + type;
                            Paramsmaster.SetValue(Desc, count);
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

                            btnClear_Click(null, null);
                            btnList_Click(null, null);
                            lblStatus.Text = "Record Deleted Successfully";
                            lblStatus.ForeColor = Color.Red;
                            btnExport.Enabled = false;
                        }

                    }

                    catch (Exception ex)
                    {

                    }
                }
                    # endregion Delete

                #endregion Delete
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }
        # endregion GrdShipper_RowCommand

        # region GrdShipper_RowEditing
        protected void GrdShipper_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion GrdShipper_RowEditing

        # region GrdShipper_PageIndexChanging
        protected void GrdShipper_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)ViewState["ds"];
            GrdShipper.PageIndex = e.NewPageIndex;
            GrdShipper.DataSource = ds;
            GrdShipper.DataBind();
            btnExport.Enabled = true;
        }

        # endregion GrdShipper_PageIndexChanging

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCodeWithName(string prefixText, int count)
        {

            // string[] orgdest = new ConBooking().GetOrgDest();

            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string"); 
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode + '(' + AgentName + ')' + '$' + CustomerCode from dbo.AgentMaster where (AgentName like '%" + prefixText + "%' or AgentCode like '%" + prefixText + "%')", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }

        private void LoadAWBMasterData()
        {
            BookingBAL objBAL = new BookingBAL();
            DataSet objDS = new DataSet("ShipperConsignee_1");
            objDS = objBAL.LoadAWBMasterData();

            try
            {
                if (objDS != null)
                {
                    //Loading Country Master codes
                    if (objDS.Tables.Count > 2 && objDS.Tables[2].Rows.Count > 0)
                    {
                        objDS.Tables[2].Rows.Add("", "");
                        ddlCountry.DataSource = objDS;
                        ddlCountry.DataMember = objDS.Tables[2].TableName;
                        ddlCountry.DataTextField = "CountryName";
                        ddlCountry.DataValueField = "CountryCode";
                        ddlCountry.DataBind();
                        ddlCountry.SelectedValue = "";

                        try
                        {
                            string countrycode = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "DefaultCountry");
                            ddlCountry.SelectedValue = countrycode;
                            ddlCountry.SelectedValue = countrycode;
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            catch (Exception ex) { }
            finally
            {
                if(objDS != null)
                    objDS.Dispose();
            }
        }
    }
}
