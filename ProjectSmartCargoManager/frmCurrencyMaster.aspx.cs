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
    public partial class frmCurrencyMaster : System.Web.UI.Page
    {
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                btnAdd.Attributes.Add("onclick", "return CheckBlank();");
                //btnList_Click(sender, e);
                Session["dsCurrencyMaster"]=null;
                BindEmptyRow();
            }
        }
        #endregion

        //#region DropDownListFilling
        //public void FillddlCountryCode()
        //{
        //    BAL.BALCurrency objBLL = new BAL.BALCurrency();
        //    DataTable dt = objBLL.dsListCountries();
        //        if (dt != null)
        //        {
        //            drpCountryCode.DataSource = dt;
        //            drpCountryCode.DataMember = dt.TableName;
        //            drpCountryCode.DataTextField = "CountryCode";
        //            drpCountryCode.DataValueField = "CCD";
        //            drpCountryCode.DataBind();
        //        }
        //    }

        #region BindEmptyRow
        protected void BindEmptyRow()
        {
            DataTable dtEmpty = new DataTable();
            try
            {
                dtEmpty.Columns.Add("CurrencyCode");
                dtEmpty.Columns.Add("CurrencyName");
                dtEmpty.Columns.Add("IsActive");
                DataRow dr=dtEmpty.NewRow();
                dtEmpty.Rows.Add(dr);

                grdCurrency.DataSource=dtEmpty;
                grdCurrency.DataBind();

                grdCurrency.Columns[3].Visible=false;
            }
            catch (Exception ex)
            { }
            finally { }
        }
        #endregion

        #region List Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            DataSet dsCUR=new DataSet();
            try
            {
                BAL.BALCurrency objCUR = new BAL.BALCurrency();
                dsCUR = objCUR.dsCurrDetails(txtCurrCode.Text.Trim(), txtCurrName.Text.Trim());
                if (dsCUR != null && dsCUR.Tables[0].Rows.Count > 0)
                {
                    grdCurrency.DataSource = dsCUR.Tables[0];
                    grdCurrency.DataBind();
                    Session["dsCurrencyMaster"] = dsCUR;
                    grdCurrency.Columns[3].Visible = true;
                }
                else
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "No Records Found!";
                    Session["dsCurrencyMaster"] = null;
                    BindEmptyRow();
                }
            }
            catch (Exception ex) { }
            finally
            {
                if (dsCUR != null)
                    dsCUR.Dispose();
            }
        }
        #endregion

        #region Adding
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string createdby = Session["UserName"].ToString();
            DateTime now = DateTime.Now;
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            try
            {
                if (txtCurrCode.Text == "" || txtCurrName.Text == "")
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Entries Can't be Blank";
                }
                BAL.BALCurrency objCUR = new BAL.BALCurrency();
                if (objCUR.ModifyCurrency("insert", txtCurrCode.Text.Trim(), txtCurrName.Text.Trim(), now, createdby))
                {

                    #region for Master Audit Log
                    #region Prepare Parameters
                    object[] Paramsmaster = new object[7];
                    int count = 0;

                    //1

                    Paramsmaster.SetValue("Currency Master", count);
                    count++;

                    //2
                   Paramsmaster.SetValue(txtCurrCode.Text+"-"+txtCurrName.Text, count);
                  count++;

                    //3

                    Paramsmaster.SetValue("ADD", count);
                    count++;

                    //4

                    Paramsmaster.SetValue("", count);
                    count++;


                    //5

                    Paramsmaster.SetValue("", count);
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

                    txtCurrCode.Text = string.Empty;
                    txtCurrName.Text = string.Empty;
                    lblStatus.Text = string.Empty;
                    btnList_Click(sender, e);
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Text = "Currency Added Successfully";

                    // Enable List..
                    txtCurrCode.ReadOnly = false;
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
            if (txtCurrCode.Text == "" && txtCurrName.Text == "")
            {
                lblStatus.Text = "Select Currency code and Currency name";
                return;
            }
            string createdby = Session["UserName"].ToString();
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            DateTime now = DateTime.Now;
            try
            {
                BAL.BALCurrency objCUR = new BAL.BALCurrency();
                if (objCUR.ModifyCurrency("delete", txtCurrCode.Text.Trim(), txtCurrName.Text.Trim(), now, createdby))
                {

                    #region for Master Audit Log
                    #region Prepare Parameters
                    object[] Paramsmaster = new object[7];
                    int count = 0;

                    //1

                    Paramsmaster.SetValue("Currency Master", count);
                    count++;

                    //2
                    Paramsmaster.SetValue(txtCurrCode.Text + "-" + txtCurrName.Text, count);
                    count++;

                    //3

                    Paramsmaster.SetValue("DELETE", count);
                    count++;

                    //4

                    Paramsmaster.SetValue("", count);
                    count++;


                    //5

                    Paramsmaster.SetValue("", count);
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

                    btnClear_Click(sender, e);
                    btnList_Click(sender, e);
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Text = "Currency Deleted Successfully";
                    
                    // Enable List..
                    txtCurrCode.ReadOnly = false;
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
            txtCurrCode.Text = string.Empty;
            txtCurrName.Text = string.Empty;
            lblStatus.Text = string.Empty;
            grdCurrency.DataSource = null;
            grdCurrency.DataBind();
        }
        #endregion

        #region RowCommand
        protected void grdCurrency_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
       
                    lblStatus.Text = "";                    // Set Status Blank
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    Label lblCurrCode = (Label)grdCurrency.Rows[RowIndex].FindControl("txtgrdCurrCode");
                    Label lblCurrName = (Label)grdCurrency.Rows[RowIndex].FindControl("txtgrdCurrName");

                    txtCurrCode.Text = lblCurrCode.Text;
                    txtCurrName.Text = lblCurrName.Text;

                    lblStatus.Text = "";
                   
                    // Disable List..
                    txtCurrCode.ReadOnly = true;
                    btnList.Enabled = false;
                    btnClear.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in getting Currency";
            }
        }
        #endregion

        protected void grdCurrency_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //BAL.BALCurrency objCUR = new BAL.BALCurrency();
                DataSet dsCUR =(DataSet) Session["dsCurrencyMaster"];//objCUR.dsCurrDetails(txtCurrCode.Text.Trim(), txtCurrName.Text.Trim());
                grdCurrency.PageIndex = e.NewPageIndex;
                grdCurrency.DataSource = dsCUR.Copy();
                grdCurrency.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in Page Index Changing";
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
               DataSet dsExp = null;
            DataTable dt = null;
            try
            {
                if (Session["dsCurrencyMaster"] == null)
                {
                    btnList_Click(null, null);
                    BindEmptyRow();
                }
                lblStatus.Text = string.Empty;

                dsExp = (DataSet)Session["dsCurrencyMaster"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                {
                    dt = (DataTable)dsExp.Tables[0];

                    string attachment = "attachment; filename=CurrencyMaster.xls";
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
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found for the selected search criteria!');</SCRIPT>", false);
                    return;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dsExp != null)
                    dsExp = null;
                if (dt != null)
                    dt = null;
            }
        }
        }
    }
        

 