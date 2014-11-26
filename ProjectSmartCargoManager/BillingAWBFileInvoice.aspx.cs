using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using QID.DataAccess;
using System.IO;
using System.Collections;
using BAL;
using System.Configuration;
using System.Data.OleDb;
using System.Drawing; 

namespace ProjectSmartCargoManager
{
    public partial class BillingAWBFileInvoice : System.Web.UI.Page
    {
        BillingAWBFileInvoiceBAL objBAL = new BillingAWBFileInvoiceBAL();
        MasterBAL objBal = new MasterBAL();
        Double ST = 0.0;
        DataSet ds;
        DataSet dsAgent;
        DataSet dsInvoices;
        static int rowind;

        decimal totalchargewt=0,totalIATA = 0, totalocda = 0, totalocdc = 0, totaltotal = 0, totaldiscount = 0, totaltad = 0, totalrevised = 0, totaltax = 0, totalfinal = 0;
        static decimal Gtotalchargewt = 0, GtotalIATA = 0, Gtotalocda = 0, Gtotalocdc = 0, Gtotaltotal = 0, Gtotaldiscount = 0, Gtotaltad = 0, Gtotalrevised = 0, Gtotaltax = 0, Gtotalfinal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ST"] != null)
                ST = Convert.ToDouble(Session["ST"].ToString());
            else
            {
                Session["ST"] = objBal.getServiceTax();
                ST = Convert.ToDouble(Session["ST"].ToString());
            }

            if (!IsPostBack)
            {
                Panel1.Visible = false;
                LoadAgentDropdown(); //AgentName 
                LoadAgentCodeDropdown(); //AgentCode 

                //Handler for discount
                string handlerDiscPer = ClientScript.GetPostBackEventReference(this.Button1, "");
                txtDiscount.Attributes.Add("onblur", handlerDiscPer);
                string handlerDiscAmt = ClientScript.GetPostBackEventReference(this.Button2, "");
                txtdiscamount.Attributes.Add("onblur", handlerDiscAmt);

                //Handler for commission
                string handlerCommPer = ClientScript.GetPostBackEventReference(this.Button3, "");
                txtcommission.Attributes.Add("onblur", handlerCommPer);
                string handlerCommAmt = ClientScript.GetPostBackEventReference(this.Button4, "");
                txtcommissionamt.Attributes.Add("onblur", handlerCommAmt);

                //Handler for Tds
                string handlerTdsPer = ClientScript.GetPostBackEventReference(this.Button5, "");
                txtTDSFrtPer.Attributes.Add("onblur", handlerTdsPer);

                //Handler for GrossWt
                string handlerGrossWt = ClientScript.GetPostBackEventReference(this.Button6, "");
                txtgrosswt.Attributes.Add("onblur", handlerGrossWt);

                //Handler for ChargeWt
                string handlerChargeWt = ClientScript.GetPostBackEventReference(this.Button7, "");
                txtchargablewt.Attributes.Add("onblur", handlerChargeWt);

                //Handler for frtRate
                string handlerFrtIATARate = ClientScript.GetPostBackEventReference(this.Button8, "");
                txtrate.Attributes.Add("onblur", handlerFrtIATARate);

                //Handler for ocda
                string handlerOcda = ClientScript.GetPostBackEventReference(this.Button9, "");
                txtocda.Attributes.Add("onblur", handlerOcda);

                //Handler for ocdc
                string handlerOcdc = ClientScript.GetPostBackEventReference(this.Button10, "");
                txtocdc.Attributes.Add("onblur", handlerOcdc); 

                //Code to Scroll Page to Bottom on click of AWB Number (in RowCommand Event)
            }
            
        }

        #region Load Agent Dropdown
        public void LoadAgentDropdown()
        {
            DataSet ds = objBAL.GetAllAgents();
            if (ds != null)
            {
                ddlAgentName.DataSource = ds;
                ddlAgentName.DataMember = ds.Tables[0].TableName;
                ddlAgentName.DataTextField = "AgentName";
                ddlAgentName.DataValueField = "AgentCode";
                ddlAgentName.DataBind();
                ddlAgentName.Items.Insert(0, new ListItem("All", ""));
                ddlAgentName.SelectedIndex = -1;
            }
        }
        #endregion LoadAgentDropdown

        #region Load Agent Code Dropdown
        public void LoadAgentCodeDropdown()
        {
            DataSet ds = objBAL.GetAllAgents();
            if (ds != null)
            {
                ddlAgentCode.DataSource = ds;
                ddlAgentCode.DataMember = ds.Tables[0].TableName;
                ddlAgentCode.DataTextField = "AgentCode";
                ddlAgentCode.DataValueField = "AgentName";
                ddlAgentCode.DataBind();
                ddlAgentCode.Items.Insert(0, new ListItem("All", ""));
                ddlAgentCode.SelectedIndex = -1;
            }
        }
        #endregion LoadAgentCodeDropdown
      

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //Confirm Single AWB number
            if (Panel1.Visible == true)
            {
                try
                {
                    if (txtDiscount.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount % ');</SCRIPT>");
                        lblStatus.Text = "Please enter Discount %";
                        lblStatus.ForeColor = Color.Blue;
                        txtDiscount.Focus();
                        return;
                    }

                    if (txtdiscamount.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount amounut');</SCRIPT>");
                        lblStatus.Text = "Please enter Discount amount";
                        lblStatus.ForeColor = Color.Blue;
                        txtdiscamount.Focus();
                        return;
                    }
                    if (txtTDSFrtPer.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                        lblStatus.Text = "Please enter TDS %";
                        lblStatus.ForeColor = Color.Blue;
                        txtTDSFrtPer.Focus();
                        return;
                    }


                    #region Prepare Parameters
                    object[] RateCardInfo = new object[23];
                    int i = 0;

                    //0
                    RateCardInfo.SetValue(txtAWBNo.Text, i);
                    i++;

                    //AgentCode and AWBDate calculation in SP

                    RateCardInfo.SetValue(txtcommodity.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtdimensions.Text, i);
                    i++;

                    //1
                    RateCardInfo.SetValue(txtgrosswt.Text, i);
                    i++;

                    //2
                    RateCardInfo.SetValue(txtchargablewt.Text, i);
                    i++;

                    //3
                    RateCardInfo.SetValue(txtrate.Text, i);
                    i++;

                    //4
                    RateCardInfo.SetValue(txtocdc.Text, i);
                    i++;


                    RateCardInfo.SetValue(txtocda.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTotal.Text, i);
                    i++;


                    RateCardInfo.SetValue(txtDiscount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtdiscamount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txttotalafterdiscount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtcommission.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtcommissionamt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtRevisedTotal.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSFrtPer.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSFrtAmt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSCommPer.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSCommAmt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtServiceTax.Text, i);
                    i++;

                    RateCardInfo.SetValue(txttotalaftertax.Text, i);

                    #endregion Prepare Parameters

                    string res = "";
                    res = objBAL.AddSingleBillingAWB(RateCardInfo);


                    if (res != "error")
                    {
                        ClearPanel();
                        bindGridView();

                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Red;
                    }

                }
                catch (Exception ex)
                {

                }

            }
            else // Confirm checked AWB Number
            {
                string awbList = "";
                ds = (DataSet)Session["dsDetails"];
                for (int y = 0; y < grdBillingInfo.Rows.Count; y++)
                {
                    if (((CheckBox)grdBillingInfo.Rows[y].FindControl("ChkSelect")).Checked)
                    {
                        if (ds.Tables[0].Rows[y]["Confirmed"].ToString() == "Pending" || ds.Tables[0].Rows[y]["Confirmed"].ToString() == "New")
                        {
                            if (awbList == "")
                            {
                                awbList = awbList + ds.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                            else
                            {
                                awbList = awbList + "," + ds.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                        }
                        else
                        {
                            //lblStatus.Text = "AWB's with status 'Pending' can only be 'Confirmed'";
                            //lblStatus.ForeColor = Color.Blue;
                            //return;
                        }
                    }

                }

                if (awbList != "")
                {
                    for (int i = 0; i < grdBillingInfo.Rows.Count; i++)
                    {
                        if (((CheckBox)grdBillingInfo.Rows[i].FindControl("ChkSelect")).Checked)
                        {
                            try
                            {
                                ds = (DataSet)Session["dsDetails"];

                                #region Prepare Parameters
                                object[] RateCardInfo = new object[21];
                                int irow = 0;

                                //0
                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["AWBNumber"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["CommodityDesc"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Dimensions"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["GrossWeight"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["ChargedWeight"].ToString(), irow);
                                irow++;

                                //RateCardInfo.SetValue(ds.Tables[0].Rows[i]["FrtIATA"].ToString(), irow);
                                //irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["FreightRate"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["OCDueCar"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["OCDueAgent"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TotalT"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Discount"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["DiscountAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TAD"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Commission"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["CommissionAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["RevisedTotal"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TDS"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TDSAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TDSOnComm"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TDSOnCommAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["ServiceTax"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Final"].ToString(), irow);


                                #endregion Prepare Parameters

                                string res = "";
                                res = objBAL.AddSingleBillingAWB(RateCardInfo);


                                if (res != "error")
                                {
                                    txtAWBNo.Text = "";
                                    txtOrgPnl.Text = "";
                                    txtDestPnl.Text = "";
                                    txtAgentName.Text = "";
                                    txtgrosswt.Text = "";
                                    txtchargablewt.Text = "";
                                    txtRatePerKg.Text = "";
                                    
                                    txtrate.Text = "";
                                    txtocdc.Text = "";
                                    txtocda.Text = "";
                                    txtcommodity.Text = "";
                                    txtdimensions.Text = "";
                                    txtTotal.Text = "";
                                    txtDiscount.Text = "";
                                    txtdiscamount.Text = "";
                                    txttotalafterdiscount.Text = "";
                                    txtTADST.Text = "";
                                    txtcommission.Text = "";
                                    txtcommissionamt.Text = "";
                                    txtRevisedTotal.Text = "";
                                    txtTDSFrtPer.Text = "";
                                    txtTDSFrtAmt.Text = "";
                                    txtTDSCommPer.Text = "";
                                    txtTDSCommAmt.Text = "";
                                    txtServiceTax.Text = "";
                                    txtSTOnComm.Text = "";
                                    txttotalaftertax.Text = "";
                                    Panel1.Visible = false;


                                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Blue;


                                }
                                else
                                {
                                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Red;
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                else
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select AWB Numbers to generate invoice');</SCRIPT>");
                    lblStatus.Text = "Select AWB numbers with status 'Pending' or 'New' to Confirm";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                bindGridView();
            }
        }

        protected void btnGenerateBill_Click(object sender, EventArgs e)
        {
            //Code to Update AWB's selected to status Final and change Finalization date in DB
            if (Panel1.Visible == true)
            {
                try
                {
                    if (txtDiscount.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount % ');</SCRIPT>");
                        lblStatus.Text = "Please enter Discount %";
                        lblStatus.ForeColor = Color.Blue;
                        txtDiscount.Focus();
                        return;
                    }

                    if (txtdiscamount.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount amounut');</SCRIPT>");
                        lblStatus.Text = "Please enter Discount amount";
                        lblStatus.ForeColor = Color.Blue;
                        txtdiscamount.Focus();
                        return;
                    }
                    if (txtTDSFrtPer.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                        lblStatus.Text = "Please enter TDS %";
                        lblStatus.ForeColor = Color.Blue;
                        txtTDSFrtPer.Focus();
                        return;
                    }


                    #region Prepare Parameters
                    object[] RateCardInfo = new object[23];
                    int i = 0;

                    //0
                    RateCardInfo.SetValue(txtAWBNo.Text, i);
                    i++;

                    //AgentCode and AWBDate calculation in SP

                    RateCardInfo.SetValue(txtcommodity.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtdimensions.Text, i);
                    i++;

                    //1
                    RateCardInfo.SetValue(txtgrosswt.Text, i);
                    i++;

                    //2
                    RateCardInfo.SetValue(txtchargablewt.Text, i);
                    i++;

                    //3
                    RateCardInfo.SetValue(txtrate.Text, i);
                    i++;

                    //4
                    RateCardInfo.SetValue(txtocdc.Text, i);
                    i++;


                    RateCardInfo.SetValue(txtocda.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTotal.Text, i);
                    i++;


                    RateCardInfo.SetValue(txtDiscount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtdiscamount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txttotalafterdiscount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtcommission.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtcommissionamt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtRevisedTotal.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSFrtPer.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSFrtAmt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSCommPer.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSCommAmt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtServiceTax.Text, i);
                    i++;

                    RateCardInfo.SetValue(txttotalaftertax.Text, i);

                    #endregion Prepare Parameters

                    string res = "";
                    res = objBAL.FinalizeSingleBillingAWB(RateCardInfo);


                    if (res != "error")
                    {
                        ClearPanel();
                        bindGridView();

                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Green;


                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Red;
                    }

                }
                catch (Exception ex)
                {

                }

            }
            else
            {
                string awbList = "";
                DataSet dsdetailsN = (DataSet)Session["dsDetails"];
                for (int y = 0; y < grdBillingInfo.Rows.Count; y++)
                {
                    if (((CheckBox)grdBillingInfo.Rows[y].FindControl("ChkSelect")).Checked)
                    {
                        if (dsdetailsN.Tables[0].Rows[y]["Confirmed"].ToString() == "New" || dsdetailsN.Tables[0].Rows[y]["Confirmed"].ToString() == "Pending" || dsdetailsN.Tables[0].Rows[y]["Confirmed"].ToString() == "Confirmed" || dsdetailsN.Tables[0].Rows[y]["Confirmed"].ToString() == "Reopened")
                        {
                            if (awbList == "")
                            {
                                awbList = awbList + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                            else
                            {
                                awbList = awbList + "," + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                        }
                        else
                        {
                            //lblStatus.Text = "AWB's with status 'Confirmed' can only be 'Finalized'";
                            //lblStatus.ForeColor = Color.Blue;
                            //return;
                        }
                    }

                }

                if (awbList != "")
                {

                    try
                    {
                        #region Prepare Parameters
                        object[] AWBInfo = new object[1];
                        int irow = 0;

                        AWBInfo.SetValue(awbList, irow);


                        #endregion Prepare Parameters

                        string res = "";
                        res = objBAL.FinalizeSelectedAWBs(AWBInfo);


                        if (res != "error")
                        {
                            bindGridView();
                            lblStatus.Text = res;
                            lblStatus.ForeColor = Color.Green;
                            return;
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                            lblStatus.Text = res;
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                    }
                    catch
                    {

                    }
                }
                else
                {
                    //lblStatus.Text = "Select AWB numbers with status 'Confirmed' to Finalize";
                    lblStatus.Text = "Invoiced AWBs can not be finalized";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
            }
        }


        protected void ddlAgentCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentName.SelectedIndex = ddlAgentCode.SelectedIndex;
        }

        protected void ddlAgentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentCode.SelectedIndex = ddlAgentName.SelectedIndex;
        }

        protected void grdBillingInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dst = (DataSet)Session["dsDetails"];
            grdBillingInfo.PageIndex = e.NewPageIndex;
            grdBillingInfo.DataSource = dst.Tables[0];
            grdBillingInfo.DataBind();
        }

        protected void grdBillingInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AWBClick")
            {
                Panel1.Visible = true;
                btnRouteDetails.Visible = true;
                rowind = Convert.ToInt32(e.CommandArgument);
                //int pageId = grdBillingInfo.PageIndex;
                //grdBillingInfo.Rows[rowindex].Cells[0].Font.Bold = true;
                fillBillingDetails(rowind + (grdBillingInfo.PageIndex * grdBillingInfo.PageSize));
                Page.ClientScript.RegisterStartupScript(GetType(), "ScrollKey", "pageScroll();", true);
            }   
        }

        protected void fillBillingDetails(int ind)
        {
            DataSet dsdetails = (DataSet)Session["dsDetails"];
            if (dsdetails.Tables[0].Rows[ind]["Confirmed"].ToString() == "Final")
            {
                Panel1.Enabled = false;
            }
            else
            {
                Panel1.Enabled = true;
            }
            txtAWBNo.Text = dsdetails.Tables[0].Rows[ind]["AWBNumber"].ToString();
            txtOrgPnl.Text = dsdetails.Tables[0].Rows[ind]["Origin"].ToString();
            txtDestPnl.Text = dsdetails.Tables[0].Rows[ind]["Destination"].ToString();
            txtAgentName.Text = dsdetails.Tables[0].Rows[ind]["AgentName"].ToString();
            txtgrosswt.Text = dsdetails.Tables[0].Rows[ind]["GrossWeight"].ToString();
            txtchargablewt.Text = dsdetails.Tables[0].Rows[ind]["ChargedWeight"].ToString();
            txtRatePerKg.Text = dsdetails.Tables[0].Rows[ind]["RatePerKG"].ToString();
            if (dsdetails.Tables[0].Rows[ind]["FreightRate"].ToString() != "")
            {
                if (Convert.ToInt32(dsdetails.Tables[0].Rows[ind]["FreightRate"].ToString()) > 0)
                {
                    txtrate.Text = dsdetails.Tables[0].Rows[ind]["FreightRate"].ToString();
                }
            }
            else
            {
                txtrate.Text = "";
            }

            txtocda.Text = dsdetails.Tables[0].Rows[ind]["OCDueAgent"].ToString();
            txtocdc.Text = dsdetails.Tables[0].Rows[ind]["OCDueCar"].ToString();

            txtcommodity.Text = dsdetails.Tables[0].Rows[ind]["CommodityDesc"].ToString();
            txtdimensions.Text = dsdetails.Tables[0].Rows[ind]["Dimensions"].ToString();

            txtTotal.Text = dsdetails.Tables[0].Rows[ind]["TotalT"].ToString();
            txtDiscount.Text = dsdetails.Tables[0].Rows[ind]["Discount"].ToString();
            txtdiscamount.Text = dsdetails.Tables[0].Rows[ind]["DiscountAmt"].ToString();
            txttotalafterdiscount.Text = "";
            txtTADST.Text = "";
            txtcommission.Text = dsdetails.Tables[0].Rows[ind]["Commission"].ToString();
            txtcommissionamt.Text = dsdetails.Tables[0].Rows[ind]["CommissionAmt"].ToString();
            //txtcommissionamt.Text = "0";
            //txtTDSPer.Text = "12.5";
            txtRevisedTotal.Text = "";

            //TDS on Freignt % and Amount
            txtTDSFrtPer.Text = dsdetails.Tables[0].Rows[ind]["TDS"].ToString();
            txtTDSFrtAmt.Text = dsdetails.Tables[0].Rows[ind]["TDSAmt"].ToString();

            //TDS on Commission % and Amount
            txtTDSCommPer.Text = dsdetails.Tables[0].Rows[ind]["TDSOnComm"].ToString();
            txtTDSCommAmt.Text = dsdetails.Tables[0].Rows[ind]["TDSOnCommAmt"].ToString();

            txtServiceTax.Text = dsdetails.Tables[0].Rows[ind]["ServiceTax"].ToString();
            txtSTOnComm.Text = dsdetails.Tables[0].Rows[ind]["STOnComm"].ToString();
            txttotalaftertax.Text = "";

            discountPerTextExit();
            discountAmtTextExit();
            commissionPerTextExit();
            commissionAmtTextExit();
        }

        protected void grdBillingInfo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdBillingInfo.PageIndex = 0;
            bindGridView();
            //pnlUpload.Visible = true;
            Panel1.Visible = false;
        }

        protected void bindGridView()
        {
            string strfromdate, strtodate;
            #region Prepare Parameters
            object[] AwbRateInfo = new object[10];
            int i = 0;

            //0
            AwbRateInfo.SetValue(ddlAgentName.SelectedValue, i);
            i++;


            //Validation for From date
            if (txtbillingfrom.Text == "")
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Valid date');</SCRIPT>");
                lblStatus.Text = "Please select Valid date";
                lblStatus.ForeColor = Color.Blue;
                return;
            }

            DateTime dt;

            try
            {
                //dt = Convert.ToDateTime(txtbillingfrom.Text);
                //Change 03082012
                string day = txtbillingfrom.Text.Substring(0, 2);
                string mon = txtbillingfrom.Text.Substring(3, 2);
                string yr = txtbillingfrom.Text.Substring(6, 4);
                strfromdate = yr + "-" + mon + "-" + day;
                dt = Convert.ToDateTime(strfromdate);

            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                lblStatus.Text = "Selected Date format invalid";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            AwbRateInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), i);
            i++;

            //Validation for To date
            if (txtbillingto.Text == "")
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Valid date');</SCRIPT>");
                lblStatus.Text = "Please select Valid date";
                lblStatus.ForeColor = Color.Blue;
                return;
            }

            DateTime dtto;

            try
            {
                //dtto = Convert.ToDateTime(txtbillingto.Text);
                //Change 03082012
                string day = txtbillingto.Text.Substring(0, 2);
                string mon = txtbillingto.Text.Substring(3, 2);
                string yr = txtbillingto.Text.Substring(6, 4);
                strtodate = yr + "-" + mon + "-" + day;
                dtto = Convert.ToDateTime(strtodate);
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                lblStatus.Text = "Selected Date format invalid";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (dtto < dt)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('To date should be greater than From date');</SCRIPT>");
                lblStatus.Text = "To date should be greater than From date";
                lblStatus.ForeColor = Color.Red;
                // MessageBox.Show("Please Enter FlightID's which is not Operated");

                return;
            }



            AwbRateInfo.SetValue(Convert.ToDateTime(strtodate).ToString("yyyy-MM-dd HH:mm:ss"), i);
            i++;

            AwbRateInfo.SetValue(txtFlightNo.Text.Trim(), i);
            i++;

            AwbRateInfo.SetValue(txtAWBNumber.Text.Trim(), i);
            i++;

            AwbRateInfo.SetValue(txtOrigin.Text.Trim(), i);
            i++;

            AwbRateInfo.SetValue(txtDest.Text.Trim(), i);
            i++;

            AwbRateInfo.SetValue(ddlStatus.SelectedValue, i);
            i++;

            AwbRateInfo.SetValue(ddlSpotRate.SelectedValue, i);
            i++;

            AwbRateInfo.SetValue(ddlPayType.SelectedValue, i);

            #endregion Prepare Parameters

            //Code to save filter value in Hidden Fields for Export to excel
            hfAgentCode.Value = ddlAgentName.SelectedValue;
            hfAgentName.Value = ddlAgentCode.SelectedValue;
            hfFromDate.Value = Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss");
            hfToDate.Value = Convert.ToDateTime(strtodate).ToString("yyyy-MM-dd HH:mm:ss");
            hfFlightNo.Value = txtFlightNo.Text.Trim();
            hfAWBNumber.Value = txtAWBNumber.Text.Trim();
            hfOrigin.Value = txtOrigin.Text.Trim();
            hfDestination.Value = txtDest.Text.Trim();
            hfStatus.Value = ddlStatus.SelectedValue;
            hfSPotRate.Value = ddlSpotRate.SelectedValue;
            hfPaymentMode.Value = ddlPayType.SelectedValue;
            ////////////////////////////////////////////////////////////////
            
            ds = objBAL.GetAWBImportRateList(AwbRateInfo);


            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            try
                            {

                                //Code to show Grant Total in Footer of Gridview.
                                Gtotalchargewt = 0; GtotalIATA = 0; Gtotalocda = 0; Gtotalocdc = 0; Gtotaltotal = 0; Gtotaldiscount = 0; Gtotaltad = 0; Gtotalrevised = 0; Gtotaltax = 0; Gtotalfinal = 0;
                                for (int cnt = 0; cnt < ds.Tables[0].Rows.Count; cnt++)
                                {
                                    Gtotalchargewt += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["ChargedWeight"].ToString());
                                    GtotalIATA += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["FreightRate"].ToString());
                                    Gtotalocda += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["OCDueAgent"].ToString());
                                    Gtotalocdc += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["OCDueCar"].ToString());
                                    Gtotaltotal += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["TotalT"].ToString());
                                    Gtotaldiscount += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["DiscountAmt"].ToString());
                                    Gtotaltad += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["TAD"].ToString());
                                    Gtotalrevised += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["RevisedTotal"].ToString());
                                    Gtotaltax += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["TDSAmt"].ToString());
                                    Gtotalfinal += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["Final"].ToString());
                                }
                                Session["dsDetails"] = ds;
                                grdBillingInfo.DataSource = ds.Tables[0];
                                grdBillingInfo.DataBind();
                                grdBillingInfo.Visible = true;


                                //visible all the buttons
                                btnConfirm.Visible = true;
                                btnGenerateBill.Visible = true;
                                btnUndoFinalize.Visible = true;
                                btnGenerateInvoice.Visible = true;
                                btnTrackAWB.Visible = true;
                                btnPrint.Visible = true;
                                btnRouteDetails.Visible = false;
                                ChkSelectAll.Visible = true;
                                ChkSelectAll.Checked = false;
                                lblAWBCount.Visible = true;
                                pnlUpload.Visible = true;
                                lblStatus.Text = "";
                                lblAWBCount.Text = "Total Count: " + ds.Tables[0].Rows.Count.ToString();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
                            lblStatus.Text = "No records found";
                            lblAWBCount.Text = "Total Count: " + "0";
                            lblStatus.ForeColor = Color.Blue;
                            btnConfirm.Visible = false;
                            btnGenerateBill.Visible = false;
                            btnUndoFinalize.Visible = false;
                            btnGenerateInvoice.Visible = false;
                            btnTrackAWB.Visible = false;
                            btnPrint.Visible = false;
                            btnRouteDetails.Visible = false;
                            grdBillingInfo.Visible = false;
                            ChkSelectAll.Visible = false;
                            lblAWBCount.Visible = false;
                            Panel1.Visible = false;
                            pnlUpload.Visible = false;
                            ClearPanel();
                            return;
                        }

                    }
                }
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            discountPerTextExit();
        }

        protected void discountPerTextExit()
        {
            if (txtDiscount.Text.Trim() != "")
            {
                DataSet dsdetails = (DataSet)Session["dsDetails"];
                if (Convert.ToDouble(txtDiscount.Text.Trim()) > 100)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid Discount Percent');</SCRIPT>");
                    lblStatus.Text = "Invalid discount percent";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                grdBillingInfo.Rows[rowind].Cells[4].Text = txtchargablewt.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[5].Text = txtrate.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[6].Text = txtocda.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[7].Text = txtocdc.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[8].Text = txtTotal.Text.Trim();

                double discAmt = (Convert.ToDouble(txtDiscount.Text.Trim()) / 100) * Convert.ToDouble(txtTotal.Text.Trim());
                txtdiscamount.Text = Math.Round(discAmt, 2).ToString();
                //save calculation of Discount in both dataset and gridview
                dsdetails.Tables[0].Rows[rowind]["Discount"] = txtdiscamount.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[9].Text = txtdiscamount.Text.Trim();
                txttotalafterdiscount.Text = Math.Round(Convert.ToDouble(txtTotal.Text.Trim()) - Convert.ToDouble(txtdiscamount.Text.Trim()), 2).ToString();
                txtTADST.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text.Trim()) + Convert.ToDouble(txtServiceTax.Text.Trim()), 2).ToString();
                //save calculation of TAD in both dataset and gridview
                dsdetails.Tables[0].Rows[rowind]["TAD"] = txttotalafterdiscount.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();
                dsdetails.AcceptChanges();
                Session["dsDetails"] = dsdetails;
                commissionAmtTextExit();
                commissionPerTextExit();
            }
        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            discountAmtTextExit();
        }

        protected void discountAmtTextExit()
        {
            if (txtdiscamount.Text.Trim() != "")
            {
                DataSet dsdetails = (DataSet)Session["dsDetails"];
                if (Convert.ToDouble(txtdiscamount.Text.Trim()) > Convert.ToDouble(txtTotal.Text.Trim()))
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid Discount Amount');</SCRIPT>");
                    lblStatus.Text = "Invalid discount amount";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                double discPer = (100 / Convert.ToDouble(txtTotal.Text.Trim())) * Convert.ToDouble(txtdiscamount.Text.Trim());
                txtDiscount.Text = Math.Round(discPer, 2).ToString();
                //save calculation of Discount in both dataset and gridview
                dsdetails.Tables[0].Rows[rowind]["Discount"] = txtdiscamount.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[9].Text = txtdiscamount.Text.Trim();
                txttotalafterdiscount.Text = Math.Round(Convert.ToDouble(txtTotal.Text.Trim()) - Convert.ToDouble(txtdiscamount.Text.Trim()), 2).ToString();
                txtTADST.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text.Trim()) + Convert.ToDouble(txtServiceTax.Text.Trim()), 2).ToString();
                //save calculation of TAD in both dataset and gridview
                dsdetails.Tables[0].Rows[rowind]["TAD"] = txttotalafterdiscount.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();
                dsdetails.AcceptChanges();
                Session["dsDetails"] = dsdetails;
                commissionAmtTextExit();
                commissionPerTextExit();

            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            commissionPerTextExit();
            
        }

        protected void commissionPerTextExit()
        {
            if (txtcommission.Text.Trim() != "")
            {
                DataSet dsdetails = (DataSet)Session["dsDetails"];
                if (Convert.ToDouble(txtcommission.Text.Trim()) > 100)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid Commission Percent');</SCRIPT>");
                    lblStatus.Text = "Invalid commission percent";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                

                double commAmt = (Convert.ToDouble(txtcommission.Text.Trim()) / 100) * Convert.ToDouble(txtrate.Text.Trim());
                txtcommissionamt.Text = Math.Round(commAmt, 2).ToString();
                //save calculation of Discount in dataset
                dsdetails.Tables[0].Rows[rowind]["Commission"] = txtcommissionamt.Text.Trim();
                //txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text.Trim()) - Convert.ToDouble(txtcommissionamt.Text.Trim()), 2).ToString();
                txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtTADST.Text.Trim()) - (Convert.ToDouble(txtcommissionamt.Text.Trim()) + Convert.ToDouble(txtSTOnComm.Text.Trim())) + Convert.ToDouble(txtTDSCommAmt.Text.Trim()), 2).ToString();
                grdBillingInfo.Rows[rowind].Cells[11].Text = txtRevisedTotal.Text.Trim();

                //Calculation for TDS amount and Total after tax
                double tdsAmt = (Convert.ToDouble(txtTDSFrtPer.Text.Trim()) / 100) * Convert.ToDouble(txtTADST.Text.Trim());
                txtTDSFrtAmt.Text = Math.Round(tdsAmt, 2).ToString();

                double tdsOnCommAmt = (Convert.ToDouble(txtTDSCommPer.Text.Trim()) / 100) * (Convert.ToDouble(txtcommissionamt.Text.Trim()) + Convert.ToDouble(txtSTOnComm.Text.Trim()));
                txtTDSCommAmt.Text = Math.Round(tdsOnCommAmt, 2).ToString();


                //save calculation of TDS amount and Total after tax in dataset and gridview
                //txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSAmt.Text.Trim()), 2).ToString();
                //txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSFrtAmt.Text.Trim()) + Convert.ToDouble(txtTDSCommAmt.Text.Trim()) + Convert.ToDouble(txtServiceTax.Text.Trim()), 2).ToString();
                txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) - Convert.ToDouble(txtTDSFrtAmt.Text.Trim()), 2).ToString();

                grdBillingInfo.Rows[rowind].Cells[12].Text = txtTDSFrtAmt.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[13].Text = txttotalaftertax.Text.Trim();

                //as columns loses its values again assign
                grdBillingInfo.Rows[rowind].Cells[4].Text = txtchargablewt.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[5].Text = txtrate.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[6].Text = txtocda.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[7].Text = txtocdc.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[8].Text = txtTotal.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[9].Text = txtdiscamount.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();

                //

                dsdetails.AcceptChanges();
                Session["dsDetails"] = dsdetails;
            }
        
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            commissionAmtTextExit();
            
        }

        protected void commissionAmtTextExit()
        {
            if (txtcommissionamt.Text.Trim() != "")
            {
                DataSet dsdetails = (DataSet)Session["dsDetails"];
                if (Convert.ToDouble(txtcommissionamt.Text.Trim()) > Convert.ToDouble(txttotalafterdiscount.Text.Trim()))
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid Commission Amount');</SCRIPT>");
                    lblStatus.Text = "Invalid commision amount";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                

                double commPer = (100 / Convert.ToDouble(txtrate.Text.Trim())) * Convert.ToDouble(txtcommissionamt.Text.Trim());
                txtcommission.Text = Math.Round(commPer, 2).ToString();
                //save calculation of Discount in dataset
                dsdetails.Tables[0].Rows[rowind]["Commission"] = txtcommissionamt.Text.Trim();

                //revised total = TAD - (Commission + ST on Commission) + TDS on commission
                //txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text.Trim()) - Convert.ToDouble(txtcommissionamt.Text.Trim()), 2).ToString();
                txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtTADST.Text.Trim()) - (Convert.ToDouble(txtcommissionamt.Text.Trim()) + Convert.ToDouble(txtSTOnComm.Text.Trim())) + Convert.ToDouble(txtTDSCommAmt.Text.Trim()), 2).ToString();
                grdBillingInfo.Rows[rowind].Cells[11].Text = txtRevisedTotal.Text.Trim();

                //Calculation for TDS amount and Total after tax
                //double tdsAmt = (Convert.ToDouble(txtTDSPer.Text.Trim()) / 100) * Convert.ToDouble(txtRevisedTotal.Text.Trim());
                //txtTDSAmt.Text = Math.Round(tdsAmt, 2).ToString();

                double tdsAmt = (Convert.ToDouble(txtTDSFrtPer.Text.Trim()) / 100) * Convert.ToDouble(txtTADST.Text.Trim());
                txtTDSFrtAmt.Text = Math.Round(tdsAmt, 2).ToString();

                double tdsOnCommAmt = (Convert.ToDouble(txtTDSCommPer.Text.Trim()) / 100) * (Convert.ToDouble(txtcommissionamt.Text.Trim()) + Convert.ToDouble(txtSTOnComm.Text.Trim()));
                txtTDSCommAmt.Text = Math.Round(tdsOnCommAmt, 2).ToString();


                //save calculation of TDS amount and Total after tax in dataset and gridview
                //txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSAmt.Text.Trim()), 2).ToString();
                //txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSFrtAmt.Text.Trim()) + Convert.ToDouble(txtTDSCommAmt.Text.Trim()) + Convert.ToDouble(txtServiceTax.Text.Trim()), 2).ToString();
                txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) - Convert.ToDouble(txtTDSFrtAmt.Text.Trim()), 2).ToString();
                grdBillingInfo.Rows[rowind].Cells[12].Text = txtTDSFrtAmt.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[13].Text = txttotalaftertax.Text.Trim();

                dsdetails.Tables[0].Rows[rowind]["RevisedTotal"] = txtRevisedTotal.Text.Trim();
                dsdetails.Tables[0].Rows[rowind]["TDS"] = txtTDSFrtAmt.Text.Trim();
                dsdetails.Tables[0].Rows[rowind]["Final"] = txttotalaftertax.Text.Trim();




                //as columns loses its values again assign
                grdBillingInfo.Rows[rowind].Cells[4].Text = txtchargablewt.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[5].Text = txtrate.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[6].Text = txtocda.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[7].Text = txtocdc.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[8].Text = txtTotal.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[9].Text = txtdiscamount.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();


                dsdetails.AcceptChanges();
                Session["dsDetails"] = dsdetails;
            }
        
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            tdsPerTextExit();
        }

        protected void tdsPerTextExit()
        {
            if (txtTDSFrtPer.Text.Trim() != "")
            {
                DataSet dsdetails = (DataSet)Session["dsDetails"];
                if (Convert.ToDouble(txtTDSFrtPer.Text.Trim()) > 100)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Invalid TDS Percent');</SCRIPT>");
                    lblStatus.Text = "Invalid TDS on Freight percent";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                double tdsAmt = (Convert.ToDouble(txtTDSFrtPer.Text.Trim()) / 100) * Convert.ToDouble(txtTADST.Text.Trim());
                txtTDSFrtAmt.Text = Math.Round(tdsAmt, 2).ToString();

                //save calculation of TDS amount and Total after tax in dataset and gridview
                //txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSAmt.Text.Trim()), 2).ToString();
                //txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) + Convert.ToDouble(txtTDSFrtAmt.Text.Trim()) + Convert.ToDouble(txtTDSCommAmt.Text.Trim()) + Convert.ToDouble(txtServiceTax.Text.Trim()), 2).ToString();
                txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) - Convert.ToDouble(txtTDSFrtAmt.Text.Trim()), 2).ToString();
                grdBillingInfo.Rows[rowind].Cells[11].Text = txtRevisedTotal.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[12].Text = txtTDSFrtAmt.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[13].Text = txttotalaftertax.Text.Trim();

                dsdetails.Tables[0].Rows[rowind]["RevisedTotal"] = txtRevisedTotal.Text.Trim();
                dsdetails.Tables[0].Rows[rowind]["TDS"] = txtTDSFrtAmt.Text.Trim();
                dsdetails.Tables[0].Rows[rowind]["Final"] = txttotalaftertax.Text.Trim();




                //as columns loses its values again assign
                grdBillingInfo.Rows[rowind].Cells[4].Text = txtchargablewt.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[5].Text = txtrate.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[6].Text = txtocda.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[7].Text = txtocdc.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[8].Text = txtTotal.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[9].Text = txtdiscamount.Text.Trim();
                grdBillingInfo.Rows[rowind].Cells[10].Text = txttotalafterdiscount.Text.Trim();


                dsdetails.AcceptChanges();
                Session["dsDetails"] = dsdetails;
            }
        
        }

        protected void GenerateInvoiceReport(string awbList)
        {
            #region Prepare Parameters
            object[] AwbRateInfo = new object[1];
            int i = 0;

            AwbRateInfo.SetValue(awbList, i);

            #endregion Prepare Parameters

            ds = objBAL.GetInvoiceDetailsAWBSImport(AwbRateInfo);

            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                Session["ShowExcel"] = "";

                                DataTable DTExport = new DataTable();

                                DTExport.Columns.Add("FrightCharges");
                                DTExport.Columns.Add("OCDC");
                                DTExport.Columns.Add("Servicetax");
                                DTExport.Columns.Add("TDSOnCommission");
                                DTExport.Columns.Add("ASubtotal");
                                // DTExport.Columns.Add("Less:-");
                                DTExport.Columns.Add("ACommission");
                                DTExport.Columns.Add("AServiceTax");
                                DTExport.Columns.Add("CCAWBs");
                                DTExport.Columns.Add("DebitNote");
                                DTExport.Columns.Add("TdsOnFreight");
                                DTExport.Columns.Add("BSubtotal");
                                DTExport.Columns.Add("NetDueToAirline");
                                DTExport.Columns.Add("AgentName");
                                DTExport.Columns.Add("Station");
                                DTExport.Columns.Add("InvoiceNumber");


                                
                                double FreightRate1, OCDC1, ServiceTax, tdsOnCommission, SubTotalA, commission, STonCommission, TDSOnFreight, SubTotalB;
                                double netDueToAirline; netDueToAirline = 0.0;

                                FreightRate1 = 0.0; OCDC1 = 0.0; tdsOnCommission = 0.0; SubTotalA = 0.0; commission = 0.0; STonCommission = 0.0; TDSOnFreight = 0.0; SubTotalB = 0.0;

                                FreightRate1 = Convert.ToDouble(ds.Tables[1].Rows[0]["FreightRate"].ToString());
                                OCDC1 = Convert.ToDouble(ds.Tables[1].Rows[0]["OCDueCar"].ToString());
                                
                                //ServiceTax = (12.3 * (FreightRate1 / 100)) + OCDC1 + Convert.ToDouble(ds.Tables[1].Rows[0]["OCDueAgent"].ToString());
                                ServiceTax = Convert.ToDouble(ds.Tables[1].Rows[0]["ServiceTax"].ToString()); //Service Tax is already there in DB thru Excel upload
                                //tdsOnCommission is 10% OF COMMISSION AMT
                                tdsOnCommission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString()) * 10 / 100;
                                SubTotalA = FreightRate1 + OCDC1 + ServiceTax + tdsOnCommission;

                                commission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString());
                                //Service tax on commission is ST% of commission amt
                                STonCommission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString()) * ST / 100;
                                TDSOnFreight = Convert.ToDouble(ds.Tables[1].Rows[0]["TDSAmt"].ToString());
                                SubTotalB = TDSOnFreight + commission + STonCommission;
                                netDueToAirline = SubTotalA - SubTotalB;


                                DTExport.Rows.Add(
                                    FreightRate1,
                                    OCDC1,
                                    ServiceTax.ToString(),
                                    tdsOnCommission.ToString(), //10% OF COMMISSION AMT
                                    SubTotalA.ToString(),
                                    commission.ToString(),
                                    STonCommission.ToString(), //ST% OF COMMISSION AMT, 
                                    "-", "-",
                                    TDSOnFreight,
                                    SubTotalB,
                                    netDueToAirline,
                                    ds.Tables[2].Rows[0]["AgentName"].ToString(), //agent name
                                    ds.Tables[2].Rows[0]["Station"].ToString(), //station
                                    ds.Tables[0].Rows[0]["InvoiceNumber"].ToString());


                                Session["ShowExcel"] = DTExport;

                                Session["ShowExcelHeader"] = "";

                                DataTable DTExportHeader = new DataTable();


                                DTExportHeader.Columns.Add("AWBNo");
                                DTExportHeader.Columns.Add("AWBDate");
                                DTExportHeader.Columns.Add("FlightNo");
                                DTExportHeader.Columns.Add("Origin");
                                DTExportHeader.Columns.Add("Dest");
                                DTExportHeader.Columns.Add("FreightType");
                                DTExportHeader.Columns.Add("NoOfPkgs");
                                DTExportHeader.Columns.Add("GrossWeight");
                                DTExportHeader.Columns.Add("ChargbleWeight");
                                DTExportHeader.Columns.Add("Rate");
                                DTExportHeader.Columns.Add("Freight");
                                DTExportHeader.Columns.Add("OCDC2");
                                DTExportHeader.Columns.Add("DueAgent");
                                DTExportHeader.Columns.Add("Total");
                                DTExportHeader.Columns.Add("ServiceTax2");
                                DTExportHeader.Columns.Add("TotalCharges");
                                DTExportHeader.Columns.Add("SrNo");

                                
                                double grossWt, chargableWt, FreightRate, OCDC, OCDCTotal, DueAgent, TotalT, ServiceTax1, ServiceTaxT, TotalCharges;
                                grossWt = 0.0; chargableWt = 0.0; FreightRate = 0.0; OCDC = 0.0; OCDCTotal = 0.0; DueAgent = 0.0; TotalT = 0.0; ServiceTaxT = 0.0; TotalCharges = 0.0;
                                double Total;
                                Total = 0.0;

                                for (int rowno = 0; rowno < ds.Tables[0].Rows.Count; rowno++)
                                {
                                    OCDC = Convert.ToDouble(ds.Tables[0].Rows[rowno]["Surcharge"].ToString()) + Convert.ToDouble(ds.Tables[0].Rows[rowno]["OCDueCar"].ToString());
                                    ServiceTax1 = (12.3 * (Convert.ToDouble(ds.Tables[0].Rows[rowno]["FreightRate"].ToString()) / 100)) + OCDC + Convert.ToDouble(ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString());
                                    Total = Convert.ToDouble(ds.Tables[0].Rows[rowno]["FreightRate"].ToString()) +
                                        OCDC +
                                        Convert.ToDouble(ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString());
                                    



                                    DTExportHeader.Rows.Add(
                                    ds.Tables[0].Rows[rowno]["AWBNumber"].ToString(),
                                    ds.Tables[0].Rows[rowno]["AWBDate"].ToString(),
                                    ds.Tables[0].Rows[rowno]["FlightNo"].ToString(), //flightno
                                    ds.Tables[0].Rows[rowno]["Origin"].ToString(), //origin
                                    ds.Tables[0].Rows[rowno]["Destination"].ToString(), //dest
                                    "", //frt type
                                    "", //noofpackings
                                    ds.Tables[0].Rows[rowno]["GrossWt"].ToString(),
                                    ds.Tables[0].Rows[rowno]["ChargableWt"].ToString(),
                                    "",
                                    ds.Tables[0].Rows[rowno]["FreightRate"].ToString(),
                                    OCDC,
                                    ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString(),
                                    Total.ToString(),
                                    ServiceTax1.ToString(),
                                    (Total + ServiceTax1).ToString(),
                                    rowno + 1);

                                    //Calculations to show Total of awbs at bottom.

                                    grossWt = grossWt + Convert.ToDouble(ds.Tables[0].Rows[rowno]["GrossWt"].ToString());
                                    chargableWt = chargableWt + Convert.ToDouble(ds.Tables[0].Rows[rowno]["ChargableWt"].ToString());
                                    FreightRate = FreightRate + Convert.ToDouble(ds.Tables[0].Rows[rowno]["FreightRate"].ToString());
                                    OCDCTotal = OCDCTotal + OCDC;
                                    DueAgent = DueAgent + Convert.ToDouble(ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString());
                                    TotalT = TotalT + Total;
                                    ServiceTaxT = ServiceTaxT + ServiceTax1;
                                    TotalCharges = TotalCharges + Total + ServiceTax1;

                                }

                                //to add row of sum of awbs
                                DTExportHeader.Rows.Add(
                                    "",
                                    "",
                                    "", //flightno
                                    "", //origin
                                    "", //dest
                                    "", //frt type
                                    "", //noofpackings
                                    grossWt.ToString(),
                                    chargableWt.ToString(),
                                    "",
                                    FreightRate.ToString(),
                                    OCDCTotal.ToString(),
                                    DueAgent.ToString(),
                                    TotalT.ToString(),
                                    ServiceTaxT.ToString(),
                                    TotalCharges.ToString(),
                                    "");


                                Session["ShowExcelHeader"] = DTExportHeader;

                                //Code to update AWBInvoice and CreditMaster with InvoiceAmount

                                string res = "";
                                #region Prepare Parameters
                                object[] AgentinvoiceInfo = new object[3];
                                int j = 0;

                                AgentinvoiceInfo.SetValue(ds.Tables[1].Rows[0]["AgentCode"].ToString(), j);
                                j++;

                                AgentinvoiceInfo.SetValue(ds.Tables[1].Rows[0]["InvoiceNumber"].ToString(), j);
                                j++;

                                AgentinvoiceInfo.SetValue(netDueToAirline, j);

                                #endregion Prepare Parameters
                                res = objBAL.UpdateAWBInvoiceImportCreditMaster(AgentinvoiceInfo);


                                Response.Write("<script>");
                                Response.Write("window.open('showBillingExcelImport.aspx','_blank')");
                                Response.Write("</script>");


                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                    }
                }
            }
        }

        //protected void GenerateAgentInvoiceReport(string awbList)
        //{
        //    #region Prepare Parameters
        //    object[] AwbRateInfo = new object[1];
        //    int i = 0;

        //    AwbRateInfo.SetValue(awbList, i);

        //    #endregion Prepare Parameters

        //    ds = objBAL.GetInvoiceDetailsAWBSImport(AwbRateInfo);

        //    if (ds != null)
        //    {
        //        if (ds.Tables != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    try
        //                    {
        //                        Session["ShowExcel"] = "";

        //                        DataTable DTExport = new DataTable();

        //                        DTExport.Columns.Add("FrightCharges");
        //                        DTExport.Columns.Add("AWBCharges");
        //                        DTExport.Columns.Add("FuelSurcharge");
        //                        DTExport.Columns.Add("DueCarrier");
        //                        DTExport.Columns.Add("Servicetax");
        //                        DTExport.Columns.Add("TDSOnCommission");
        //                        DTExport.Columns.Add("ASubtotal");
        //                        // DTExport.Columns.Add("Less:-");
        //                        DTExport.Columns.Add("ACommission");
        //                        DTExport.Columns.Add("AServiceTax");
        //                        DTExport.Columns.Add("CCAWBs");
        //                        DTExport.Columns.Add("DebitNote");
        //                        DTExport.Columns.Add("TdsOnFreight");
        //                        DTExport.Columns.Add("BSubtotal");
        //                        DTExport.Columns.Add("NetDueToAirline");
        //                        DTExport.Columns.Add("AgentName");
        //                        DTExport.Columns.Add("Station");
        //                        DTExport.Columns.Add("InvoiceNumber");


        //                        double FreightRate1, AWBCharges1, FuelSurcharge1, DueCarrier1, ServiceTax, tdsOnCommission, SubTotalA, commission, STonCommission, TDSOnFreight, SubTotalB;
        //                        double netDueToAirline; netDueToAirline = 0.0;

        //                        FreightRate1 = 0.0; AWBCharges1 = 0.0; FuelSurcharge1 = 0.0;
        //                        DueCarrier1 = 0.0; SubTotalA = 0.0; commission = 0.0; STonCommission = 0.0; TDSOnFreight = 0.0; SubTotalB = 0.0;

        //                        FreightRate1 = Convert.ToDouble(ds.Tables[1].Rows[0]["FreightRate"].ToString());
        //                        AWBCharges1 = Convert.ToDouble(ds.Tables[2].Rows[0]["Charge"].ToString());
        //                        FuelSurcharge1 = Convert.ToDouble(ds.Tables[2].Rows[2]["Charge"].ToString());
        //                        DueCarrier1 = Convert.ToDouble(ds.Tables[2].Rows[1]["Charge"].ToString());

        //                        ServiceTax = Convert.ToDouble(ds.Tables[2].Rows[0]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[2]["Tax"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[1]["Tax"].ToString());
        //                        //tdsOnCommission is 10% OF COMMISSION AMT
        //                        tdsOnCommission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString()) * 10 / 100;
        //                        SubTotalA = FreightRate1 + AWBCharges1 + FuelSurcharge1 + DueCarrier1 + ServiceTax + tdsOnCommission;

        //                        commission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString());
        //                        //Service tax on commission is ST% of commission amt
        //                        STonCommission = Convert.ToDouble(ds.Tables[1].Rows[0]["CommissionAmt"].ToString()) * ST / 100;
        //                        TDSOnFreight = Convert.ToDouble(ds.Tables[1].Rows[0]["TDSAmt"].ToString());
        //                        SubTotalB = TDSOnFreight + commission + STonCommission;
        //                        netDueToAirline = SubTotalA - SubTotalB;


        //                        DTExport.Rows.Add(
        //                            FreightRate1, //MKT + OCDC
        //                            AWBCharges1,
        //                            FuelSurcharge1,
        //                            DueCarrier1,
        //                            ServiceTax.ToString(), //COMING FROM AWB SUMMARY
        //                            tdsOnCommission.ToString(), //10% OF COMMISSION AMT
        //                            SubTotalA.ToString(),
        //                            commission.ToString(),
        //                            STonCommission.ToString(), //ST% OF COMMISSION AMT
        //                            "-", "-",
        //                            TDSOnFreight, //COMING FROM AGENT MASTER
        //                            SubTotalB,
        //                            netDueToAirline,
        //                            ds.Tables[4].Rows[0]["AgentName"].ToString(), //agent name
        //                            ds.Tables[4].Rows[0]["Station"].ToString(), //station
        //                            ds.Tables[0].Rows[0]["InvoiceNumber"].ToString());



        //                        Session["ShowExcel"] = DTExport;

        //                        Session["ShowExcelHeader"] = "";

        //                        DataTable DTExportHeader = new DataTable();


        //                        DTExportHeader.Columns.Add("AWBNo");
        //                        DTExportHeader.Columns.Add("AWBDate");
        //                        DTExportHeader.Columns.Add("FlightNo");
        //                        DTExportHeader.Columns.Add("Origin");
        //                        DTExportHeader.Columns.Add("Dest");
        //                        DTExportHeader.Columns.Add("FreightType");
        //                        DTExportHeader.Columns.Add("NoOfPkgs");
        //                        DTExportHeader.Columns.Add("GrossWeight");
        //                        DTExportHeader.Columns.Add("ChargbleWeight");
        //                        DTExportHeader.Columns.Add("Rate");
        //                        DTExportHeader.Columns.Add("Freight");
        //                        DTExportHeader.Columns.Add("AWBCharges2");
        //                        DTExportHeader.Columns.Add("FuelSurcharge2");
        //                        DTExportHeader.Columns.Add("DueCarrier2");
        //                        DTExportHeader.Columns.Add("DueAgent");
        //                        DTExportHeader.Columns.Add("Total");
        //                        DTExportHeader.Columns.Add("ServiceTax2");
        //                        DTExportHeader.Columns.Add("TotalCharges");
        //                        DTExportHeader.Columns.Add("SrNo");



        //                        double grossWt, chargableWt, FreightRate, AWBCharges, FuelSurcharge, DueCarrier, DueAgent, TotalT, ServiceTaxT, TotalCharges;
        //                        grossWt = 0.0; chargableWt = 0.0; FreightRate = 0.0; AWBCharges = 0.0; FuelSurcharge = 0.0;
        //                        DueCarrier = 0.0; DueAgent = 0.0; TotalT = 0.0; ServiceTaxT = 0.0; TotalCharges = 0.0;
        //                        double Total;
        //                        Total = 0.0;

        //                        for (int rowno = 0; rowno < ds.Tables[0].Rows.Count; rowno++)
        //                        {
        //                            ServiceTax = Convert.ToDouble(ds.Tables[6].Rows[rowno + (rowno * 3)]["Tax"].ToString())  //Tax for AWB charges
        //                                + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Tax"].ToString()) //Tax for Fuel charges
        //                                + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Tax"].ToString()); //Tax for Due carrier
        //                            Total = Convert.ToDouble(ds.Tables[0].Rows[rowno]["FreightRate"].ToString()) +
        //                                Convert.ToDouble(ds.Tables[6].Rows[rowno + (rowno * 3)]["Charge"].ToString()) +
        //                                Convert.ToDouble(ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Charge"].ToString()) +
        //                                Convert.ToDouble(ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Charge"].ToString()) +
        //                                Convert.ToDouble(ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString());


        //                            DTExportHeader.Rows.Add(
        //                            ds.Tables[0].Rows[rowno]["AWBNumber"].ToString(),
        //                            ds.Tables[0].Rows[rowno]["AWBDate"].ToString(),
        //                            ds.Tables[3].Rows[rowno]["FltNumber"].ToString(), //flightno
        //                            ds.Tables[3].Rows[rowno]["FltOrigin"].ToString(), //origin
        //                            ds.Tables[3].Rows[rowno]["FltDestination"].ToString(), //dest
        //                            ds.Tables[5].Rows[rowno]["Type"].ToString(), //frt type
        //                            "", //noofpackings
        //                            ds.Tables[0].Rows[rowno]["GrossWt"].ToString(),
        //                            ds.Tables[0].Rows[rowno]["ChargableWt"].ToString(),
        //                            "", //RATE
        //                            ds.Tables[0].Rows[rowno]["FreightRate"].ToString(),
        //                            ds.Tables[6].Rows[rowno + (rowno * 3)]["Charge"].ToString(), //AWB OCDC
        //                            ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Charge"].ToString(), //FUEL OCDC
        //                            ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Charge"].ToString(), //DUE CARRIER OCDC
        //                            ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString(),
        //                            Total.ToString(),
        //                            ServiceTax.ToString(),
        //                            (Total + ServiceTax).ToString(),
        //                            rowno + 1);

        //                            //Calculations to show Total of awbs at bottom.
        //                            grossWt = grossWt + Convert.ToDouble(ds.Tables[0].Rows[rowno]["GrossWt"].ToString());
        //                            chargableWt = chargableWt + Convert.ToDouble(ds.Tables[0].Rows[rowno]["ChargableWt"].ToString());
        //                            FreightRate = FreightRate + Convert.ToDouble(ds.Tables[0].Rows[rowno]["FreightRate"].ToString());
        //                            AWBCharges = AWBCharges + Convert.ToDouble(ds.Tables[6].Rows[rowno + (rowno * 3)]["Charge"].ToString());
        //                            FuelSurcharge = FuelSurcharge + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 2) + (rowno * 3)]["Charge"].ToString());
        //                            DueCarrier = DueCarrier + Convert.ToDouble(ds.Tables[6].Rows[(rowno + 1) + (rowno * 3)]["Charge"].ToString());
        //                            DueAgent = DueAgent + Convert.ToDouble(ds.Tables[0].Rows[rowno]["OCDueAgent"].ToString());
        //                            TotalT = TotalT + Total;
        //                            ServiceTaxT = ServiceTaxT + ServiceTax;
        //                            TotalCharges = TotalCharges + Total + ServiceTax;

        //                        }

        //                        //to add row of sum of awbs
        //                        DTExportHeader.Rows.Add(
        //                            "",
        //                            "",
        //                            "", //flightno
        //                            "", //origin
        //                            "", //dest
        //                            "", //frt type
        //                            "", //noofpackings
        //                            grossWt.ToString(),
        //                            chargableWt.ToString(),
        //                            "",
        //                            FreightRate.ToString(),
        //                            AWBCharges.ToString(),
        //                            FuelSurcharge.ToString(),
        //                            DueCarrier.ToString(),
        //                            DueAgent.ToString(),
        //                            TotalT.ToString(),
        //                            ServiceTaxT.ToString(),
        //                            TotalCharges.ToString(),
        //                            "");


        //                        Session["ShowExcelHeader"] = DTExportHeader;


        //                        //Code to update AWBInvoice and CreditMaster with InvoiceAmount

        //                        string res = "";
        //                        #region Prepare Parameters
        //                        object[] AgentinvoiceInfo = new object[3];
        //                        int j = 0;

        //                        AgentinvoiceInfo.SetValue(ds.Tables[1].Rows[0]["AgentCode"].ToString(), j);
        //                        j++;

        //                        AgentinvoiceInfo.SetValue(ds.Tables[1].Rows[0]["InvoiceNumber"].ToString(), j);
        //                        j++;

        //                        AgentinvoiceInfo.SetValue(netDueToAirline, j);

        //                        #endregion Prepare Parameters
        //                        res = objBAL.UpdateAWBInvoiceCreditMaster(AgentinvoiceInfo);


        //                        Response.Write("<script>");
        //                        Response.Write("window.open('showBillingExcel.aspx','_blank')");
        //                        Response.Write("</script>");


        //                    }
        //                    catch (Exception ex)
        //                    {

        //                    }
        //                }
        //                else
        //                {
        //                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
        //                    lblStatus.Text = "No records found";
        //                    lblStatus.ForeColor = Color.Blue;
        //                    return;
        //                }

        //            }
        //        }
        //    }
        //}

        protected void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSelectAll.Checked == true)
            {
                for (int i = 0; i < grdBillingInfo.Rows.Count; i++)
                {
                    ((CheckBox)grdBillingInfo.Rows[i].FindControl("ChkSelect")).Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < grdBillingInfo.Rows.Count; i++)
                {
                    ((CheckBox)grdBillingInfo.Rows[i].FindControl("ChkSelect")).Checked = false;
                }
            }
        }

        protected void btnInvMatch_Click(object sender, EventArgs e)
        {
            //Code to match Invoice for Selected AWB's in GridView with Agent file uploaded.

            DataSet dsMatch = (DataSet)Session["dsDetails"];

            int j;
            bool SelectedAWB;
            SelectedAWB = false;

            for (j = 0; j < grdBillingInfo.Rows.Count; j++)
            {
                if (((CheckBox)grdBillingInfo.Rows[j].FindControl("ChkSelect")).Checked)
                {
                    SelectedAWB = true;
                    //Code to check if AWBs of status New are only selected to verify.
                    if (dsMatch.Tables[0].Rows[j]["Confirmed"].ToString() == "New")
                    {
                        try
                        {
                            ds = (DataSet)Session["dsDetails"];

                            #region Prepare Parameters
                            object[] RateCardInfo = new object[2];
                            int irow = 0;

                            //0
                            RateCardInfo.SetValue(ds.Tables[0].Rows[j]["AWBNumber"].ToString(), irow);
                            irow++;

                            //RateCardInfo.SetValue(ds.Tables[0].Rows[j]["Final"].ToString(), irow);
                            RateCardInfo.SetValue(Convert.ToDouble(ds.Tables[0].Rows[j]["TAD"].ToString()) + Convert.ToDouble(ds.Tables[0].Rows[j]["ServiceTax"].ToString()) , irow);


                            #endregion Prepare Parameters

                            string res = "";
                            res = objBAL.MatchInvoiceWithAgentData(RateCardInfo);


                            if (res != "error")
                            {
                                ClearPanel();

                                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Blue;


                            }
                            else
                            {
                                lblStatus.Text = res;
                                lblStatus.ForeColor = Color.Red;
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        lblStatus.Text = "Only AWB's with status New can be verified.";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                }
            }

            bindGridView();

            if (SelectedAWB == false)
            {
                lblStatus.Text = "Select AWB's for Invoice Matching.";
                lblStatus.ForeColor = Color.Blue;
                return;
            }
        }

        protected void ClearPanel()
        {
            txtAWBNo.Text = "";
            txtOrgPnl.Text = "";
            txtDestPnl.Text = "";
            txtAgentName.Text = "";
            txtgrosswt.Text = "";
            txtchargablewt.Text = "";
            txtRatePerKg.Text = "";
            txtrate.Text = "";
            txtocdc.Text = "";
            txtocda.Text = "";
            txtcommodity.Text = "";
            txtdimensions.Text = "";
            txtTotal.Text = "";
            txtDiscount.Text = "";
            txtdiscamount.Text = "";
            txttotalafterdiscount.Text = "";
            txtTADST.Text = "";
            txtcommission.Text = "";
            txtcommissionamt.Text = "";
            txtRevisedTotal.Text = "";
            txtTDSFrtPer.Text = "";
            txtTDSFrtAmt.Text = "";
            txtTDSCommPer.Text = "";
            txtTDSCommAmt.Text = "";
            txtServiceTax.Text = "";
            txtSTOnComm.Text = "";
            txttotalaftertax.Text = "";
            Panel1.Visible = false;

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            #region Checking file extension

            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;
                String savePath = @"c:\SCMExcel\";

                if (FileExcelUpload.HasFile)
                {
                    //code to save the file on server
                    String fileName = FileExcelUpload.FileName;

                    // Append the name of the file to upload to the path.
                    savePath += fileName;


                    // Call the SaveAs method to save the 
                    // uploaded file to the specified path.
                    // This example does not perform all
                    // the necessary error checking.               
                    // If a file with the same name
                    // already exists in the specified path,  
                    // the uploaded file overwrites it.
                    FileExcelUpload.SaveAs(savePath);


                    Session["WorkingFile"] = FileExcelUpload.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    //filePath = Path.GetFullPath(Session["WorkingFile"].ToString()).ToLower();
                    //filePath = "C:\\AWBInvoice\\01aprto04apr2012final.xls";
                    filePath = @"C:\SCMExcel\" + FileExcelUpload.FileName;
                    String[] allowedExtensions = { ".xls", ".xlsx" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (FileExtension == allowedExtensions[i])
                        {
                            FileOK = true;
                        }
                    }
                }

                if (FileOK)
                {

                    if (!LoadExcelData(FileExtension, FileExcelUpload.FileName, filePath))
                    {
                        lblStatus.Text = "Error in data upload";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblStatus.Text = "File Uploaded Successfully...";
                        lblStatus.ForeColor = Color.Green;
                    }


                }
                else
                {
                    lblStatus.Text = "Cannot accept files of this type.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        #region Load DataSet
        private bool LoadExcelData(string ext, string filename, string filepath)
        {

            string connString = string.Empty;

            if (ext == ".xls")
            {
                //   connString = ConfigurationManager.ConnectionStrings["xls"].ConnectionString;
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=Excel 8.0;";

                //connString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                //          @"Data Source=" + filename + ";" +
                //          @"Extended Properties=" + Convert.ToChar(34).ToString() +
                //          @"Excel 8.0;HDR=YES" + Convert.ToChar(34).ToString();


            }
            else if (ext == ".xlsx")
            {
                // connString = ConfigurationManager.ConnectionStrings["xlsx"].ConnectionString;
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=Excel 12.0";

            }

            OleDbConnection oledbConn = new OleDbConnection(connString);
            oledbConn.Open();
            //string ExcelName = "Sheet1";
            char[] charsToTrim = { '$', '[', ']', '\'' };
            string ExcelName = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString().Trim(charsToTrim);

            string query = "select * from [" + ExcelName + "$]";


            // Create the connection object

            try
            {
                // Open connection


                // Create OleDbCommand object and select data from worksheet Sheet1
                OleDbCommand cmd = new OleDbCommand(query, oledbConn);

                // Create new OleDbDataAdapter
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                oleda.SelectCommand = cmd;

                // Create a DataSet which will hold the data extracted from the worksheet.
                DataSet dsPO = new DataSet();

                // Fill the DataSet from the data extracted from the worksheet.
                oleda.Fill(dsPO);

                // Upload data in database table

                bool res = loadFileDataInDB(dsPO);

                return res;

            }
            catch (Exception ex)
            {
                return false;

            }
            finally
            {
                // Close connection
                oledbConn.Close();
            }
        }
        #endregion

        #region load File data in Db
        protected bool loadFileDataInDB(DataSet dsData)
        {
            try
            {
                #region Prepare Parameters
                object[] RateCardInfo = new object[18];
                int i = 0;
                for (int row = 0; row < dsData.Tables[0].Rows.Count - 1; row++)
                {
                    i = 0;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][0].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][1].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][2].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][3].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][4].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][5].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][6].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][7].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][8].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][9].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][10].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][11].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][12].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][13].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][14].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][15].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][16].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][17].ToString(), i);


                #endregion Prepare Parameters

                    string res = "";
                    res = objBAL.AddAgentFileForInvoiceMatch(RateCardInfo);
                    //RateCardID=objBAL.AddRateCard(RateCardInfo);

                    if (res != "")
                    {
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Red;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }
        #endregion load File data in Db

        protected void Button6_Click(object sender, EventArgs e)
        {
            grossWtTextExit();
        }

        protected void grossWtTextExit()
        {
            try
            {
                if (Convert.ToDouble(txtgrosswt.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Gross Weight";
                    lblStatus.ForeColor = Color.Red;
                    return;

                }
                else
                {
                    //code to do calc if other calc depends on Gross Weight
                    DataSet dsdetails = (DataSet)Session["dsDetails"];
                    dsdetails.Tables[0].Rows[rowind]["GrossWeight"] = txtgrosswt.Text.Trim();
                    chargeWtTextExit();
                }
               
            }
            catch (Exception)
            {
                lblStatus.Text = "Enter valid Gross Weight";
                lblStatus.ForeColor = Color.Red;
                return;
            }

        }


        protected void Button7_Click(object sender, EventArgs e)
        {
            chargeWtTextExit();
        }

        protected void chargeWtTextExit()
        {

            try
            {
                if (Convert.ToDouble(txtchargablewt.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Chargable Weight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    //code to do calc if other calc depends on Charged Weight
                    DataSet dsdetails = (DataSet)Session["dsDetails"];

                    dsdetails.Tables[0].Rows[rowind]["ChargedWeight"] = txtchargablewt.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[4].Text = txtchargablewt.Text.Trim();

                    //DataSet dsRates = objBook.ProcessRates(dsdetails.Tables[0].Rows[rowind]["AWBNumber"].ToString(), "", 0);
                    //txtiatarate.Text = dsRates.Tables[0].Rows[0]["FrIATA"].ToString();
                    //txtmktrate.Text = dsRates.Tables[0].Rows[0]["FrMKT"].ToString();
                    //txtocda.Text = dsRates.Tables[0].Rows[0]["OCDA"].ToString();
                    //txtocdc.Text = dsRates.Tables[0].Rows[0]["OCDC"].ToString();
                    frtRateTextExit();
                }
           
            }
            catch (Exception)
            {
                lblStatus.Text = "Enter valid Chargable Weight";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            frtRateTextExit();
        }

        protected void frtRateTextExit()
        {
            try
            {

                if (Convert.ToDouble(txtrate.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Freight Rate";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    //code to do calc if other calc depends on Charged Weight
                    DataSet dsdetails = (DataSet)Session["dsDetails"];
                    //saving frtRate in frtIATA (not in frtMKT). so change IATA value
                    dsdetails.Tables[0].Rows[rowind]["FreightRate"] = txtrate.Text.Trim();
                    //txtTotal.Text = Math.Round(Convert.ToDouble(txtiatarate.Text.Trim()) + Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                    txtTotal.Text = Math.Round(Convert.ToDouble(txtrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                    dsdetails.Tables[0].Rows[rowind]["TotalT"] = txtTotal.Text.Trim();
                    dsdetails.AcceptChanges();
                    Session["dsDetails"] = dsdetails;
                    OCDATextExit();
                    OCDCTextExit();
                    discountPerTextExit();
                    //discountAmtTextExit();
                    commissionPerTextExit();
                    //commissionAmtTextExit();
                }
               
            }
            catch (Exception)
            {
                lblStatus.Text = "Enter valid IATA Rate";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            OCDATextExit();
        }

        protected void OCDATextExit()
        {
            try
            {
                if (Convert.ToDouble(txtocda.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid OCDA";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    //code to do calc if other calc depends on Charged Weight
                    DataSet dsdetails = (DataSet)Session["dsDetails"];

                    dsdetails.Tables[0].Rows[rowind]["OCDueAgent"] = txtocda.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[5].Text = txtrate.Text.Trim();
                    
                    grdBillingInfo.Rows[rowind].Cells[6].Text = txtocda.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[7].Text = txtocdc.Text.Trim();

                    //txtTotal.Text = Math.Round(Convert.ToDouble(txtiatarate.Text.Trim()) + Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                    txtTotal.Text = Math.Round(Convert.ToDouble(txtrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                    grdBillingInfo.Rows[rowind].Cells[8].Text = txtTotal.Text.Trim();
                    dsdetails.Tables[0].Rows[rowind]["TotalT"] = txtTotal.Text.Trim();


                    dsdetails.AcceptChanges();
                    Session["dsDetails"] = dsdetails;
                    discountPerTextExit();
                    //discountAmtTextExit();
                    commissionPerTextExit();
                    //commissionAmtTextExit();
                }
                                
            }
            catch (Exception)
            {
                lblStatus.Text = "Enter valid OCDA";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            OCDCTextExit();
        }

        protected void OCDCTextExit()
        {
            try
            {
                if (Convert.ToDouble(txtocdc.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid OCDC";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    //code to do calc if other calc depends on OCDC
                    DataSet dsdetails = (DataSet)Session["dsDetails"];

                    dsdetails.Tables[0].Rows[rowind]["OCDueCar"] = txtocdc.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[5].Text = txtrate.Text.Trim();
                    
                    grdBillingInfo.Rows[rowind].Cells[6].Text = txtocda.Text.Trim();
                    grdBillingInfo.Rows[rowind].Cells[7].Text = txtocdc.Text.Trim();

                    //txtTotal.Text = Math.Round(Convert.ToDouble(txtiatarate.Text.Trim()) + Convert.ToDouble(txtmktrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                    txtTotal.Text = Math.Round(Convert.ToDouble(txtrate.Text.Trim()) + Convert.ToDouble(txtocdc.Text.Trim()), 2).ToString();
                    grdBillingInfo.Rows[rowind].Cells[8].Text = txtTotal.Text.Trim();
                    dsdetails.Tables[0].Rows[rowind]["TotalT"] = txtTotal.Text.Trim();

                    dsdetails.AcceptChanges();
                    Session["dsDetails"] = dsdetails;
                    discountPerTextExit();
                    //discountAmtTextExit();
                    commissionPerTextExit();
                    //commissionAmtTextExit();
                }
            }
            catch (Exception)
            {
                lblStatus.Text = "Enter valid OCDC";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }

        protected void btnGenerateInvoice_Click(object sender, EventArgs e)
        {
            //Code to Generate Invoice for the AWB's with status Final.

            //Code to add AWB int AWBList with status Final.

            string awbList = "";
            DataSet dsdetailsN = (DataSet)Session["dsDetails"];
            for (int y = 0; y < grdBillingInfo.Rows.Count; y++)
            {
                if (dsdetailsN.Tables[0].Rows[y]["Confirmed"].ToString() == "Final")
                {
                    if (awbList == "")
                    {
                        awbList = awbList + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString();
                    }
                    else
                    {
                        awbList = awbList + "," + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString();
                    }
                }
                else
                {
                    //lblStatus.Text = "Invoice can be generated only for Finalized AWB's";
                    //lblStatus.ForeColor = Color.Blue;
                    //return;
                }
                
            }

            
            #region Prepare Parameters
            object[] AWBInfo = new object[1];
            int i = 0;

            AWBInfo.SetValue(awbList, i);

            #endregion Prepare Parameters

            if (awbList != "")
            {
                string res = "";
                try
                {
                    res = objBAL.GenerateBunchInvoiceNoAWBSImport(AWBInfo);
                    dsInvoices = objBAL.GetInvoiceNosOfAWBS(AWBInfo);

                    if (dsInvoices != null)
                    {
                        if (dsInvoices.Tables != null)
                        {
                            if (dsInvoices.Tables.Count > 0)
                            {
                                if (dsInvoices.Tables[0].Rows.Count > 0)
                                {
                                    for (int invCnt = 0; invCnt < dsInvoices.Tables[0].Rows.Count; invCnt++)
                                    {
                                        #region Prepare Parameters
                                        object[] InvInfo = new object[1];
                                        int inv = 0;

                                        InvInfo.SetValue(dsInvoices.Tables[0].Rows[invCnt]["InvoiceNumber"].ToString(), inv);

                                        #endregion Prepare Parameters

                                        try
                                        {
                                            //Code to update BillingInvoiceImport with sum of fields in BillingAWBImport
                                            res = objBAL.UpdateSumBillingInvoiceImport(InvInfo);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }


                    if (dsInvoices != null)
                    {
                        if (dsInvoices.Tables != null)
                        {
                            if (dsInvoices.Tables.Count > 0)
                            {
                                if (dsInvoices.Tables[0].Rows.Count > 0)
                                {
                                    for (int invCnt = 0; invCnt < dsInvoices.Tables[0].Rows.Count; invCnt++)
                                    {
                                        #region Prepare Parameters
                                        object[] InvInfo = new object[1];
                                        int inv = 0;

                                        InvInfo.SetValue(dsInvoices.Tables[0].Rows[invCnt]["InvoiceNumber"].ToString(), inv);

                                        #endregion Prepare Parameters

                                        try
                                        {
                                            //Code to Change Invoice Amount depending on CCA/DCM
                                            res = objBAL.UpdateInvoiceAmtFromCCADCM(InvInfo);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
                
                if (res != "")
                {
                    bindGridView();
                    lblStatus.Text = "Bunched invoices generated successfully";
                    lblStatus.ForeColor = Color.Green;
                    //GenerateInvoiceReport(awbList);
                    
                    
                }
                else
                {
                    lblStatus.Text = "Bunched invoice generation failed";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                lblStatus.Text = "Select AWB numbers with status Final to generate Invoice";
                lblStatus.ForeColor = Color.Blue;
            }

        }

       
        protected void grdBillingInfo_PageIndexChanged(object sender, EventArgs e)
        {
            //DataSet dst = (DataSet)Session["dsDetails"];
            //grdBillingInfo.DataSource = dst.Tables[0];
            //grdBillingInfo.DataBind();
        }

        protected void btnAllInvMatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdBillingInfo.Rows.Count > 0)
                {
                    #region Prepare Parameters
                    object[] AWBInfo = new object[2];
                    int irow = 0;

                    //Match All Invoices Of Selected Agent.
                    AWBInfo.SetValue(ddlAgentName.SelectedValue, irow);

                    #endregion Prepare Parameters
                    
                    dsAgent = objBAL.GetAllPendingInvOfAgent(AWBInfo);

                    if (dsAgent != null)
                    {
                        if (dsAgent.Tables != null)
                        {
                            if (dsAgent.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsAgent.Tables[0].Rows.Count; j++)
                                {
                                    string res = "";
                                    try
                                    {
                                        #region Prepare Parameters
                                        object[] RateCardInfo = new object[2];
                                        int irownum = 0;

                                        RateCardInfo.SetValue(dsAgent.Tables[0].Rows[j]["AWBNumber"].ToString(), irownum);
                                        irownum++;

                                        RateCardInfo.SetValue(Convert.ToDouble(dsAgent.Tables[0].Rows[j]["AWBTotalAmt"].ToString()), irownum);

                                        #endregion Prepare Parameters

                                        res = objBAL.MatchAllInvoicesOfAgent(RateCardInfo);

                                        ClearPanel();

                                    }
                                    catch (Exception)
                                    {
                                        lblStatus.Text = res;
                                        lblStatus.ForeColor = Color.Red;

                                    }

                                }

                                bindGridView();
                            }
                            else
                            {
                                lblStatus.Text = "No invoices with status 'New' are available for selected agent";
                                lblStatus.ForeColor = Color.Blue;
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void btnUndoFinalize_Click(object sender, EventArgs e)
        {
            if (Panel1.Visible == true)
            {
                try
                {

                    #region Prepare Parameters
                    object[] RateCardInfo = new object[1];
                    int irow = 0;

                    //0
                    RateCardInfo.SetValue(txtAWBNo.Text.Trim(), irow);

                    #endregion Prepare Parameters

                    string res = "";
                    res = objBAL.UndoFinalizeSingleAWB(RateCardInfo);


                    if (res != "error")
                    {
                        txtAWBNo.Text = "";
                        txtOrgPnl.Text = "";
                        txtDestPnl.Text = "";
                        txtAgentName.Text = "";
                        txtgrosswt.Text = "";
                        txtchargablewt.Text = "";
                        txtRatePerKg.Text = "";
                        txtrate.Text = "";
                        txtocdc.Text = "";
                        txtocda.Text = "";
                        txtcommodity.Text = "";
                        txtdimensions.Text = "";
                        txtTotal.Text = "";
                        txtDiscount.Text = "";
                        txtdiscamount.Text = "";
                        txttotalafterdiscount.Text = "";
                        txtTADST.Text = "";
                        txtcommission.Text = "";
                        txtcommissionamt.Text = "";
                        txtRevisedTotal.Text = "";
                        txtTDSFrtPer.Text = "";
                        txtTDSFrtAmt.Text = "";
                        txtTDSCommPer.Text = "";
                        txtTDSCommAmt.Text = "";
                        txtServiceTax.Text = "";
                        txtSTOnComm.Text = "";
                        txttotalaftertax.Text = "";
                        Panel1.Visible = false;


                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Blue;


                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Red;
                    }

                }
                catch (Exception ex)
                {

                }


            }
            else
            {
                string awbList = "";
                ds = (DataSet)Session["dsDetails"];
                for (int y = 0; y < grdBillingInfo.Rows.Count; y++)
                {
                    if (((CheckBox)grdBillingInfo.Rows[y].FindControl("ChkSelect")).Checked)
                    {
                        if (ds.Tables[0].Rows[y]["Confirmed"].ToString() == "Final")
                        {
                            if (awbList == "")
                            {
                                awbList = awbList + ds.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                            else
                            {
                                awbList = awbList + "," + ds.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                        }
                        else
                        {
                            //lblStatus.Text = "AWB's with status 'Pending' can only be 'Confirmed'";
                            //lblStatus.ForeColor = Color.Blue;
                            //return;
                        }
                    }

                }

                if (awbList != "")
                {
                    for (int i = 0; i < grdBillingInfo.Rows.Count; i++)
                    {
                        if (((CheckBox)grdBillingInfo.Rows[i].FindControl("ChkSelect")).Checked)
                        {
                            try
                            {
                                ds = (DataSet)Session["dsDetails"];

                                #region Prepare Parameters
                                object[] RateCardInfo = new object[1];
                                int irow = 0;

                                //0
                                RateCardInfo.SetValue(awbList, irow);

                                #endregion Prepare Parameters

                                string res = "";
                                res = objBAL.UndoFinalizeAWB(RateCardInfo);


                                if (res != "error")
                                {
                                    txtAWBNo.Text = "";
                                    txtOrgPnl.Text = "";
                                    txtDestPnl.Text = "";
                                    txtAgentName.Text = "";
                                    txtgrosswt.Text = "";
                                    txtchargablewt.Text = "";
                                    txtRatePerKg.Text = "";
                                    txtrate.Text = "";
                                    txtocdc.Text = "";
                                    txtocda.Text = "";
                                    txtcommodity.Text = "";
                                    txtdimensions.Text = "";
                                    txtTotal.Text = "";
                                    txtDiscount.Text = "";
                                    txtdiscamount.Text = "";
                                    txttotalafterdiscount.Text = "";
                                    txtTADST.Text = "";
                                    txtcommission.Text = "";
                                    txtcommissionamt.Text = "";
                                    txtRevisedTotal.Text = "";
                                    txtTDSFrtPer.Text = "";
                                    txtTDSFrtAmt.Text = "";
                                    txtTDSCommPer.Text = "";
                                    txtTDSCommAmt.Text = "";
                                    txtServiceTax.Text = "";
                                    txtSTOnComm.Text = "";
                                    txttotalaftertax.Text = "";
                                    Panel1.Visible = false;


                                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Blue;


                                }
                                else
                                {
                                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Red;
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                else
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select AWB Numbers to generate invoice');</SCRIPT>");
                    lblStatus.Text = "Undo Finalization can be done only for AWB numbers with status 'Final'";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
            }

                bindGridView();
            
        }

        protected void grdBillingInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblchargewt = (Label)e.Row.FindControl("lblchargewt");
                Label lblIATA = (Label)e.Row.FindControl("lblIATA");
                Label lblocda = (Label)e.Row.FindControl("lblocda");
                Label lblocdc = (Label)e.Row.FindControl("lblocdc");
                Label lbltotal = (Label)e.Row.FindControl("lbltotal");
                Label lblDiscount = (Label)e.Row.FindControl("lblDiscount");
                Label lblTAD = (Label)e.Row.FindControl("lbltotalafterdiscount");
                Label lblrevised = (Label)e.Row.FindControl("lblrevised");
                Label lbltax = (Label)e.Row.FindControl("lbltax");
                Label lblfinal = (Label)e.Row.FindControl("lblfinal");

                decimal chargewt = Decimal.Parse(lblchargewt.Text);
                decimal IATA = Decimal.Parse(lblIATA.Text);
                decimal ocda = Decimal.Parse(lblocda.Text);
                decimal ocdc = Decimal.Parse(lblocdc.Text);
                decimal total = Decimal.Parse(lbltotal.Text);
                decimal discount = Decimal.Parse(lblDiscount.Text);
                decimal TAD = Decimal.Parse(lblTAD.Text);
                decimal revised = Decimal.Parse(lblrevised.Text);
                decimal tax = Decimal.Parse(lbltax.Text);
                decimal final = Decimal.Parse(lblfinal.Text);

                totalchargewt += chargewt;
                totalIATA += IATA;
                totalocda += ocda;
                totalocdc += ocdc;
                totaltotal += total;
                totaldiscount += discount;
                totaltad += TAD;
                totalrevised += revised;
                totaltax += tax;
                totalfinal += final;

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalText = (Label)e.Row.FindControl("lblTotalText");
                Label lblGTotalText = (Label)e.Row.FindControl("lblGTotalText");
                Label lblTotalchargewt = (Label)e.Row.FindControl("lblTotalchargewt");
                Label lblGTotalchargewt = (Label)e.Row.FindControl("lblGTotalchargewt");
                Label lblTotalFreightRate = (Label)e.Row.FindControl("lblTotalFreightRate");
                Label lblGTotalFreightRate = (Label)e.Row.FindControl("lblGTotalFreightRate");
                Label lblTotalOCDA = (Label)e.Row.FindControl("lblTotalOCDA");
                Label lblGTotalOCDA = (Label)e.Row.FindControl("lblGTotalOCDA");
                Label lblTotalOCDC = (Label)e.Row.FindControl("lblTotalOCDC");
                Label lblGTotalOCDC = (Label)e.Row.FindControl("lblGTotalOCDC");
                Label lblTotalTotal = (Label)e.Row.FindControl("lblTotalTotal");
                Label lblGTotalTotal = (Label)e.Row.FindControl("lblGTotalTotal");
                Label lblTotalDiscount = (Label)e.Row.FindControl("lblTotalDiscount");
                Label lblGTotalDiscount = (Label)e.Row.FindControl("lblGTotalDiscount");
                Label lblTotalTAD = (Label)e.Row.FindControl("lblTotalTAD");
                Label lblGTotalTAD = (Label)e.Row.FindControl("lblGTotalTAD");
                Label lblTotalRevised = (Label)e.Row.FindControl("lblTotalRevised");
                Label lblGTotalRevised = (Label)e.Row.FindControl("lblGTotalRevised");
                Label lblTotalTax = (Label)e.Row.FindControl("lblTotalTax");
                Label lblGTotalTax = (Label)e.Row.FindControl("lblGTotalTax");
                Label lblTotalFinal = (Label)e.Row.FindControl("lblTotalFinal");
                Label lblGTotalFinal = (Label)e.Row.FindControl("lblGTotalFinal");

                lblTotalText.Text = "Total";
                lblGTotalText.Text = "GrantTotal";
                lblTotalchargewt.Text = totalchargewt.ToString();
                lblGTotalchargewt.Text = Gtotalchargewt.ToString();
                lblTotalFreightRate.Text = totalIATA.ToString();
                lblGTotalFreightRate.Text = GtotalIATA.ToString();
                lblTotalOCDA.Text = totalocda.ToString();
                lblGTotalOCDA.Text = Gtotalocda.ToString();
                lblTotalOCDC.Text = totalocdc.ToString();
                lblGTotalOCDC.Text = Gtotalocdc.ToString();
                lblTotalTotal.Text = totaltotal.ToString();
                lblGTotalTotal.Text = Gtotaltotal.ToString();
                lblTotalDiscount.Text = totaldiscount.ToString();
                lblGTotalDiscount.Text = Gtotaldiscount.ToString();
                lblTotalTAD.Text = totaltad.ToString();
                lblGTotalTAD.Text = Gtotaltad.ToString();
                lblTotalRevised.Text = totalrevised.ToString();
                lblGTotalRevised.Text = Gtotalrevised.ToString();
                lblTotalTax.Text = totaltax.ToString();
                lblGTotalTax.Text = Gtotaltax.ToString();
                lblTotalFinal.Text = totalfinal.ToString();
                lblGTotalFinal.Text = Gtotalfinal.ToString();

            } 
        }

        protected void btnRouteDetails_Click(object sender, EventArgs e)
        {
            if (Panel1.Visible == true)
            {
                Response.Write("<script>");
                Response.Write("window.open('ShowRouteDetails.aspx?AWBNumber=" + txtAWBNo.Text + "', 'window','HEIGHT=400,WIDTH=600,top=50,left=50,toolbar=yes,scrollbars=yes,resizable=yes')");
                Response.Write("</script>");
                //Response.Redirect("ShowRouteDetails?AWBNumber=" + txtAWBNo.Text);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            #region Prepare Parameters
            object[] AwbBulkInfo = new object[10];
            int i = 0;

            AwbBulkInfo.SetValue(hfAgentCode.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfFromDate.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfToDate.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfFlightNo.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfAWBNumber.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfOrigin.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfDestination.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfStatus.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfSPotRate.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfPaymentMode.Value, i);

            #endregion Prepare Parameters

            DataSet dsRes = objBAL.GetAWBImportBulkDataforExport(AwbBulkInfo);
            
            
            if (dsRes != null)
            {

                if (dsRes.Tables != null)
                {
                    if (dsRes.Tables.Count > 0)
                    {
                        if (dsRes.Tables[0].Rows.Count > 0)
                        {
                            try
                            {

                                DataRow dr = dsRes.Tables[0].NewRow();
                                dr = dsRes.Tables[1].Rows[0];
                                dsRes.Tables[0].ImportRow(dr);

                                Session["Filters"] = "";

                                DataTable DTFilters = new DataTable();

                                DTFilters.Columns.Add("AgentName");
                                DTFilters.Columns.Add("AgentCode");
                                DTFilters.Columns.Add("FromDate");
                                DTFilters.Columns.Add("ToDate");
                                DTFilters.Columns.Add("FlightNo");
                                DTFilters.Columns.Add("AWBNumber");
                                DTFilters.Columns.Add("Origin");
                                DTFilters.Columns.Add("Destination");
                                DTFilters.Columns.Add("Status");
                                DTFilters.Columns.Add("SpotRate");
                                DTFilters.Columns.Add("PaymentMode");

                                //DTFilters.Rows.Add(
                                //    ddlAgentName.SelectedItem.Text,
                                //    ddlAgentCode.SelectedItem.Text,
                                //    Convert.ToDateTime(txtbillingfrom.Text).ToShortDateString(),
                                //    Convert.ToDateTime(txtbillingto.Text).ToShortDateString(),
                                //    txtFlightNo.Text.Trim(), 
                                //    txtAWBNo.Text.Trim(), 
                                //    txtOrigin.Text.Trim(),
                                //    txtDest.Text.Trim(),
                                //    ddlStatus.SelectedItem.Text, 
                                //    ddlSpotRate.SelectedItem.Text, 
                                //    ddlPayType.SelectedItem.Text);


                                DTFilters.Rows.Add(
                                    hfAgentName.Value,
                                    hfAgentCode.Value,
                                    //hfFromDate.Value,
                                    txtbillingfrom.Text,
                                    //hfToDate.Value,
                                    txtbillingto.Text,
                                    hfFlightNo.Value,
                                    hfAWBNumber.Value,
                                    hfOrigin.Value,
                                    hfDestination.Value,
                                    hfStatus.Value,
                                    hfSPotRate.Value,
                                    hfPaymentMode.Value);


                                Session["Filters"] = DTFilters;

                                Session["BulkData"] = "";
                                DataTable DTBulkData = new DataTable();
                                DTBulkData = dsRes.Tables[0];
                                Session["BulkData"] = DTBulkData;

                                Response.Write("<script>");
                                Response.Write("window.open('ShowBillingBulkDataImport.aspx','_blank')");
                                Response.Write("</script>");


                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                    }
                }
            }
        }


    }
}
