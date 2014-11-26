using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class VendorMaster : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropDowns();

                if (Request.QueryString["Command"] != null)
                {
                    string VendorCode = Request.QueryString["Code"].ToString();
                    DataSet ds = da.SelectRecords("Sp_GetVendorList", "VendorCode", VendorCode, SqlDbType.VarChar);
                    AutoPopulate(ds);
                    if (Request.QueryString["Command"].ToString() == "Edit")
                        btnAdd.Text = "Update";
                }

            }
        }

        protected void LoadDropDowns()
        {   
            DataSet ds = new DataSet();
            try
            {
                ds = da.SelectRecords("SP_GetGLAccountCode");
                ddlGLCode.DataSource = ds.Tables[0];
                ddlGLCode.DataValueField = "GLAccountCode";
                ddlGLCode.DataBind();
                ddlGLCode.Items.Insert(0, new ListItem("Select", ""));

                ds = da.SelectRecords("spGetCurrenyCode");
                ddlCurrency.DataSource=ds.Tables[0];
                ddlCurrency.DataTextField = "CurrencyCode";
                ddlCurrency.DataValueField = "Code";
                ddlCurrency.DataBind();
                ddlCurrency.Items.Insert(0, new ListItem("Select", ""));

                ds = da.SelectRecords("Sp_GetCountry");
                ddlCountry.DataSource = ds.Tables[0];
                ddlCountry.DataTextField = "CountryName";
                ddlCountry.DataValueField = "CountryCode";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("Select", ""));

                ds = da.SelectRecords("sp_GetCityList");
                ddlCity.DataSource = ds.Tables[0];
                ddlCity.DataValueField = "CityCode";
                ddlCity.DataTextField = "CityName";
                ddlCity.DataBind();
                ddlCity.Items.Insert(0, new ListItem("Select", ""));


            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            try
            {
                if (ValidateDate() == false)
                    return;
                
                if (btnAdd.Text == "Save")
                {
                    if (ChkDuplicate() == false)
                        return;
                    #region Save

                    #region Parameters
                    object[] PValues = new object[18];
                    string[] PName = new string[18];
                    SqlDbType[] PType = new SqlDbType[18];

                    int i = 0;

                    //0
                    PValues.SetValue(txtVendorCode.Text.Trim().ToUpper(), i);
                    PName.SetValue("VendorCode", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //1
                    PValues.SetValue(txtVendorName.Text.Trim().ToUpper(), i);
                    PName.SetValue("VendorName", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //2
                    PValues.SetValue(txtFromDate.Text.Trim(), i);
                    PName.SetValue("ValidFrm", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //3
                    PValues.SetValue(txtToDate.Text.Trim(), i);
                    PName.SetValue("ValidTo", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //4
                    PValues.SetValue(txtAddress.Text.Trim(), i);
                    PName.SetValue("Address", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //5
                    PValues.SetValue(txtContactPerson.Text.Trim(), i);
                    PName.SetValue("ContactPerson", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //6
                    PValues.SetValue(ddlCurrency.SelectedValue, i);
                    PName.SetValue("Currency", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //7
                    PValues.SetValue(ddlCity.SelectedValue, i);
                    PName.SetValue("City", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //8
                    PValues.SetValue(txtContctMail.Text.Trim(), i);
                    PName.SetValue("ContactEmail", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //9
                    PValues.SetValue(txtServiceTax.Text.Trim(), i);
                    PName.SetValue("ServTax", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //10
                    PValues.SetValue(ddlCountry.SelectedValue, i);
                    PName.SetValue("Country", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //11
                    PValues.SetValue(txtContactPh.Text.Trim(), i);
                    PName.SetValue("ContactPhone", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //12
                    PValues.SetValue(ddlBillType.SelectedItem.Text, i);
                    PName.SetValue("BillingFreq", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //13
                    PValues.SetValue(txtContactMob.Text.Trim(), i);
                    PName.SetValue("ContactMobile", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //14
                    PValues.SetValue(ddlGLCode.SelectedItem.Text, i);
                    PName.SetValue("GLCode", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //15
                    PValues.SetValue(chkIsAct.Checked, i);
                    PName.SetValue("isAct", i);
                    PType.SetValue(SqlDbType.Bit, i);
                    i++;

                    //16
                    DateTime dt=Convert.ToDateTime(Session["IT"].ToString());
                    PValues.SetValue(dt, i);
                    PName.SetValue("CreatedOn", i);
                    PType.SetValue(SqlDbType.DateTime, i);
                    i++;

                    //17
                    PValues.SetValue(Session["UserName"].ToString(), i);
                    PName.SetValue("CreatedBy", i);
                    PType.SetValue(SqlDbType.VarChar, i);

                    #endregion

                    bool Result = da.ExecuteProcedure("Sp_InsertVendorDetails", PName, PType, PValues);

                    if (Result)
                    {
                        #region for Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Params = new object[7];
                        int k = 0;
                        
                        //1
                        Params.SetValue("VendorMaster", k);
                        k++;

                        //2
                        Params.SetValue(txtVendorCode.Text.Trim()+"-"+txtVendorName.Text.Trim(), k);
                        k++;

                        //3
                        Params.SetValue("ADD", k);
                        k++;

                        //4
                        Params.SetValue("New Vendor Added", k);
                        k++;

                        //5
                        string desc = "Vendor Code:" + txtVendorCode.Text.Trim() + "/Vendor Name:" + txtVendorName.Text.Trim();
                        Params.SetValue(desc, k);
                        k++;

                        //6
                        Params.SetValue(Session["UserName"], k);
                        k++;

                        //7
                        Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                        k++;


                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Params);
                        #endregion
                        lblStatus.Text = "Record Added Successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Record Couldnot be Added";
                        lblStatus.ForeColor = Color.Red;
                    }
                    #endregion
                }
                if (btnAdd.Text == "Update")
                {
                    #region Update

                    #region Parameters
                    object[] PValues = new object[19];
                    string[] PName = new string[19];
                    SqlDbType[] PType = new SqlDbType[19];

                    int i = 0;

                    //0
                    PValues.SetValue(txtVendorCode.Text.Trim().ToUpper(), i);
                    PName.SetValue("VendorCode", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //1
                    PValues.SetValue(txtVendorName.Text.Trim().ToUpper(), i);
                    PName.SetValue("VendorName", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //2
                    PValues.SetValue(txtFromDate.Text.Trim(), i);
                    PName.SetValue("ValidFrm", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //3
                    PValues.SetValue(txtToDate.Text.Trim(), i);
                    PName.SetValue("ValidTo", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //4
                    PValues.SetValue(txtAddress.Text.Trim(), i);
                    PName.SetValue("Address", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //5
                    PValues.SetValue(txtContactPerson.Text.Trim(), i);
                    PName.SetValue("ContactPerson", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //6
                    PValues.SetValue(ddlCurrency.SelectedValue, i);
                    PName.SetValue("Currency", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //7
                    PValues.SetValue(ddlCity.SelectedValue, i);
                    PName.SetValue("City", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //8
                    PValues.SetValue(txtContctMail.Text.Trim(), i);
                    PName.SetValue("ContactEmail", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //9
                    PValues.SetValue(txtServiceTax.Text.Trim(), i);
                    PName.SetValue("ServTax", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //10
                    PValues.SetValue(ddlCountry.SelectedValue, i);
                    PName.SetValue("Country", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //11
                    PValues.SetValue(txtContactPh.Text.Trim(), i);
                    PName.SetValue("ContactPhone", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //12
                    PValues.SetValue(ddlBillType.SelectedItem.Text, i);
                    PName.SetValue("BillingFreq", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //13
                    PValues.SetValue(txtContactMob.Text.Trim(), i);
                    PName.SetValue("ContactMobile", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //14
                    PValues.SetValue(ddlGLCode.SelectedItem.Text, i);
                    PName.SetValue("GLCode", i);
                    PType.SetValue(SqlDbType.VarChar, i);
                    i++;

                    //15
                    int srno = int.Parse(Request.QueryString["ID"].ToString());
                    PValues.SetValue(srno, i);
                    PName.SetValue("SrNo", i);
                    PType.SetValue(SqlDbType.Int, i);
                    i++;

                    //16
                    PValues.SetValue(chkIsAct.Checked, i);
                    PName.SetValue("isAct", i);
                    PType.SetValue(SqlDbType.Bit, i);
                    i++;

                    //17
                    DateTime dt = Convert.ToDateTime(Session["IT"].ToString());
                    PValues.SetValue(dt, i);
                    PName.SetValue("CreatedOn", i);
                    PType.SetValue(SqlDbType.DateTime, i);
                    i++;

                    //18
                    PValues.SetValue(Session["UserName"].ToString(), i);
                    PName.SetValue("CreatedBy", i);
                    PType.SetValue(SqlDbType.VarChar, i);

                    #endregion

                    bool Result = da.ExecuteProcedure("Sp_UpdateVendor", PName, PType, PValues);

                    if (Result)
                    {
                        #region for Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Params = new object[7];
                        int k = 0;

                        //1
                        Params.SetValue("VendorMaster", k);
                        k++;

                        //2
                        Params.SetValue(txtVendorCode.Text.Trim() + "-" + txtVendorName.Text.Trim(), k);
                        k++;

                        //3
                        Params.SetValue("Updated", k);
                        k++;

                        //4
                        Params.SetValue("Vendor Updated", k);
                        k++;

                        //5
                        string desc = "Vendor Code:" + txtVendorCode.Text.Trim() + "/Vendor Name:" + txtVendorName.Text.Trim();
                        Params.SetValue(desc, k);
                        k++;

                        //6
                        Params.SetValue(Session["UserName"], k);
                        k++;

                        //7
                        Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                        k++;


                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Params);
                        #endregion
                        lblStatus.Text = "Record Updated Successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Record Couldnot be Updated";
                        lblStatus.ForeColor = Color.Red;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
            finally { }
        }

        protected bool ValidateDate()
        {
            try
            {
                try
                {
                    if (txtVendorCode.Text == "" || txtVendorName.Text == "")
                    {
                        lblStatus.Text = "Enter Vendor Code and Name";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                    if (txtFromDate.Text == "" || txtToDate.Text == "")
                    {
                        lblStatus.Text = "Enter Dates";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                    DateTime dtFrom = DateTime.ParseExact(txtFromDate.Text.Trim(),"dd/MM/yyyy",null);
                    DateTime dtTo = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);

                    if (dtFrom > dtTo)
                    {
                        lblStatus.Text = "To Date cannot be greater than From Date";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Enter dates in dd/MM/yyyy format";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }
            catch (Exception ex)
            { }

            return true;
        }

        protected void AutoPopulate(DataSet ds)
        {
            try
            {
                txtVendorCode.Text = ds.Tables[0].Rows[0]["VendorCode"].ToString();
                txtVendorName.Text = ds.Tables[0].Rows[0]["VendorName"].ToString();
                txtFromDate.Text = ds.Tables[0].Rows[0]["ValidFrom"].ToString();
                txtToDate.Text = ds.Tables[0].Rows[0]["ValidTo"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtContactPerson.Text = ds.Tables[0].Rows[0]["ContactPerson"].ToString();

                string Currency = ds.Tables[0].Rows[0]["Currency"].ToString();
                ddlCurrency.SelectedIndex = ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(Currency));

                string City = ds.Tables[0].Rows[0]["City"].ToString();
                ddlCity.SelectedIndex = ddlCity.Items.IndexOf(ddlCity.Items.FindByValue(City));

                txtContctMail.Text = ds.Tables[0].Rows[0]["ContactEmail"].ToString();
                txtServiceTax.Text = ds.Tables[0].Rows[0]["ServiceTax"].ToString();

                string Country = ds.Tables[0].Rows[0]["Country"].ToString();
                ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByValue(Country));

                txtContactPh.Text = ds.Tables[0].Rows[0]["ContactPhone"].ToString();

                string BillFrew = ds.Tables[0].Rows[0]["BillingFreq"].ToString();
                ddlBillType.SelectedIndex = ddlBillType.Items.IndexOf(ddlBillType.Items.FindByText(BillFrew));

                txtContactMob.Text = ds.Tables[0].Rows[0]["ContactMobile"].ToString();

                string GLCode = ds.Tables[0].Rows[0]["GLCode"].ToString();
                ddlGLCode.SelectedIndex = ddlGLCode.Items.IndexOf(ddlGLCode.Items.FindByText(GLCode));

                if (ds.Tables[0].Rows[0]["IsActive"].ToString().ToUpper() == "TRUE")
                    chkIsAct.Checked = true;
                else
                    chkIsAct.Checked = false;
            }
            catch (Exception ex)
            { }
            finally { }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            lblStatus.Text = string.Empty;
            try
            {
                ds = da.SelectRecords("Sp_GetVendorList", "VendorCode", txtVendorCode.Text.Trim(), SqlDbType.VarChar);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    AutoPopulate(ds);
                }
                else
                {
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                { ds.Dispose(); }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            btnAdd.Text = "Save";
            chkIsAct.Checked = false;
            txtVendorCode.Text = string.Empty;
            txtVendorName.Text = string.Empty;
            txtFromDate.Text = txtToDate.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtContactPerson.Text = string.Empty;
            ddlCurrency.SelectedIndex = 0;
            ddlCity.SelectedIndex = 0;
            txtContctMail.Text = string.Empty;
            txtServiceTax.Text = string.Empty;
            ddlCountry.SelectedIndex = 0;
            txtContactPh.Text = string.Empty;
            ddlCountry.SelectedIndex = 0;
            txtContactMob.Text = string.Empty;
            ddlGLCode.SelectedIndex = 0;
        }

        protected bool ChkDuplicate()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = da.SelectRecords("Sp_ChkDuplicateVendor", "VendorCode", txtVendorCode.Text.Trim(), SqlDbType.VarChar);
                bool res = Convert.ToBoolean(ds.Tables[0].Rows[0][0].ToString());
                if (res)
                {
                    lblStatus.Text = "Vendor Code already exists";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }
            catch (Exception ex)
            { }
            finally { }

            return true;
        }

        protected void DisableView()
        {
            txtVendorCode.Enabled = false;
            txtVendorName.Enabled = false;
            txtFromDate.Enabled = txtToDate.Enabled=false;
            txtAddress.Enabled = false;
            txtContactPerson.Enabled = false;
            ddlCurrency.Enabled = false;
            ddlCity.Enabled = false;
            txtContctMail.Enabled = false;
            txtServiceTax.Enabled = false;
            ddlCountry.Enabled = false;
            txtContactPh.Enabled = false;
            ddlCountry.Enabled = false;
            txtContactMob.Enabled = false;
            ddlGLCode.Enabled = false;
            btnAdd.Visible = false;
            btnList.Visible = btnClear.Visible = false;
        }

    }
}
