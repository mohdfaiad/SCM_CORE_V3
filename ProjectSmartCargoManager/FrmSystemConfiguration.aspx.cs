using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using QID.DataAccess;
using BAL;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;

namespace ProjectSmartCargoManager
{
    public partial class FrmSystemConfiguration : System.Web.UI.Page
    {
        #region Variables

        BALSystemConfig ObjBAL = new BALSystemConfig();

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        { 
            try
            {
                if (!IsPostBack)
                {
                    lblStatus.Visible = false;
                    Loadddlparam();
                    getSystemConfiguration();

                    #region Define PageSize for grid as per configuration
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        grdResult.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                        objConfig = null;
                        grdResult.AllowPaging = true;
                        grdResult.PageSize = 10;
                    }
                    catch (Exception ex)
                    { }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                lblStatus.Visible = true;
                lblStatus.Text = ex.Message;
            }

        }
        protected void Loadddlparam()
        {
            DataSet ds1 = ObjBAL.getList();
            try 
            {

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    ddlparam.DataSource = ds1.Tables[0];
                    ddlparam.DataValueField = "Parameter";
                    ddlparam.DataTextField = "Parameter";
                    ddlparam.DataBind();
                    ddlparam.Items.Insert(0, "All");
                    

                }
            
            
            }
            catch (Exception ex)
            { }
        }
        protected void getSystemConfiguration()
        {
            try
            {
                txtPara.Text = "";
                txtValue.Text = "";
                txtAppkey.Text = "";
                txtDescription.Text = "";
                 string vallue="";
                DataSet ds = new DataSet();
                if (ddlparam.SelectedValue=="All")
                    //ddlparam.SelectedValue = "";
                    vallue = "";
                else
                    vallue = ddlparam.SelectedValue.ToString();
                ds = ObjBAL.SearchList(vallue,txtsearchkey.Text);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            grdResult.DataSource = ds.Tables[0];
                            grdResult.DataBind();
                            Session["dsList"] = ds.Tables[0];
                        }
                        else
                        {
                            lblStatus.Visible = true;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Record Found!!!";
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void grdResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //DataSet ds = (DataSet)Session["dsList"];
            if (txtsearchkey.Text == "")
            {
                string PValue = "";
                if (ddlparam.SelectedValue == "All")
                    PValue = "";
                else
                    PValue = ddlparam.SelectedValue.ToString();
                DataSet ds = new DataSet();
                ds = ObjBAL.SearchList(PValue,txtsearchkey.Text);
                grdResult.PageIndex = e.NewPageIndex;
                grdResult.DataSource = ds.Tables[0];
                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    grdResult.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
                grdResult.AllowPaging = true;
                grdResult.PageSize = 10;
                grdResult.DataBind();
            }
            else
            {
                string po = txtsearchkey.Text;
                DataSet ds1 = new DataSet();
                try
                {

                    lblStatus.Text = string.Empty;

                    #region Prepare Parameters
                    object[] SystemInforma = new object[1];
                    int i = 0;


                    SystemInforma.SetValue(txtsearchkey.Text.Trim(), i);
                    i++;

                    //1


                    #endregion Prepare Parameters

                    //DataSet ds = new DataSet();
                    ds1 = ObjBAL.SearchListByKey(SystemInforma);
                    #region Define PageSize for grid as per configuration
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        grdResult.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                        objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion

                    //ds1 = ObjBAL.SearchListByKey();
                    grdResult.PageIndex = e.NewPageIndex;
                    grdResult.DataSource = ds1.Tables[0];

                    #region Define PageSize for grid as per configuration
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        grdResult.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                        objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion
                    grdResult.AllowPaging = true;
                    grdResult.PageSize = 10;
                    //grdResult.DataBind();
                    grdResult.DataBind();

                }
                catch (Exception ex)
                { }

            }
        }

        #region Save Button
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                int CheckForSelect = 0;
                for (int i = 0; i < grdResult.Rows.Count; i++)
                {
                    CheckBox chkBox = (CheckBox)grdResult.Rows[i].FindControl("chkList");
                    if (chkBox.Checked)
                    {
                        CheckForSelect++;
                        TextBox txtCategoryName = (TextBox)grdResult.Rows[i].FindControl("txtPara");
                        TextBox txtCategoryDesc = (TextBox)grdResult.Rows[i].FindControl("txtValue");
                        CheckBox chkIsActive = (CheckBox)grdResult.Rows[i].FindControl("txtApp_Key");
                        TextBox txtDescription = (TextBox)grdResult.Rows[i].FindControl("txtDescription");
                        ds = ObjBAL.SaveData(txtPara.Text.ToString(), txtValue.Text.ToString(), txtAppkey.Text.ToString(),txtDescription.Text.ToString());

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0][0].ToString() == "INSERTED")
                            {
                                #region for Master Audit Log
                                MasterAuditBAL ObjMAL = new MasterAuditBAL();
                                #region Prepare Parameters
                                object[] Paramsss = new object[7];
                                int k = 0;

                                //1
                                Paramsss.SetValue("System", k);
                                k++;

                                //2
                                string Value = txtPara.Text.ToString() + "-" + txtValue.Text.ToString();
                                Paramsss.SetValue(Value, k);
                                k++;

                                //3
                                Paramsss.SetValue("ADD", k);
                                k++;

                                //4
                                string Msg = "New System Config";
                                Paramsss.SetValue(Msg, k);
                                k++;

                                //5
                                string Desc = "";
                                Paramsss.SetValue(Desc, k);
                                k++;

                                //6

                                Paramsss.SetValue(Session["UserName"], k);
                                k++;

                                //7
                                Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                                k++;

                                #endregion Prepare Parameters
                                ObjMAL.AddMasterAuditLog(Paramsss);
                                #endregion
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Visible = true;
                                lblStatus.Text = "Record Inserted Successfully !!";
                            }
                            else if (ds.Tables[0].Rows[0][0].ToString() == "UPDATED")
                            {
                                #region for Master Audit Log
                                MasterAuditBAL ObjMAL = new MasterAuditBAL();
                                #region Prepare Parameters
                                object[] Paramsss = new object[7];
                                int k = 0;

                                //1
                                Paramsss.SetValue("System", k);
                                k++;

                                //2
                                string Value = txtPara.Text.ToString() + "-" + txtValue.Text.ToString();
                                Paramsss.SetValue(Value, k);
                                k++;

                                //3
                                Paramsss.SetValue("Update", k);
                                k++;

                                //4
                                string Msg = "System Config Updated";
                                Paramsss.SetValue(Msg, k);
                                k++;

                                //5
                                string Desc = "";
                                Paramsss.SetValue(Desc, k);
                                k++;

                                //6

                                Paramsss.SetValue(Session["UserName"], k);
                                k++;

                                //7
                                Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                                k++;

                                #endregion Prepare Parameters
                                ObjMAL.AddMasterAuditLog(Paramsss);
                                #endregion
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Visible = true;
                                lblStatus.Text = "Record Updated Successfully !!";
                            }
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Visible = true;
                            lblStatus.Text = "Error In Inserting Data !!";
                        }
                        


                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Visible = true;
                lblStatus.Text = ex.Message;
            }

        }

        #endregion

        protected void rbSelect_CheckedChanged(object sender, EventArgs e)
        {
            lblStatus.Text = "";

            foreach (GridViewRow oldrow in grdResult.Rows)
            {
                ((RadioButton)oldrow.FindControl("rbSelect")).Checked = false;
            }

            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rb.NamingContainer;
            ((RadioButton)row.FindControl("rbSelect")).Checked = true;

            //Get the values in textboxes from selected row
            txtPara.Text = ((Label)row.FindControl("lblParameter")).Text;
            txtValue.Text = ((Label)row.FindControl("lblValue")).Text;
            txtAppkey.Text = ((Label)row.FindControl("lblApp_Key")).Text;
            txtDescription.Text = ((Label)row.FindControl("lblDescription")).Text;

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            try
            {
                string res = "";
                res = ObjBAL.addUpdateSystemConfiguration(txtPara.Text.ToString(), txtValue.Text.ToString(), txtAppkey.Text.ToString(), "A",txtDescription.Text.ToString());
                if(res == "INSERTED")
                {
                    if (txtPara.Text.Trim().ToUpper() == "ULDACTIVATE" && txtAppkey.Text.Trim().ToUpper() == "ULDACTIVE")
                    {
                        Session["ULDACT"] = txtValue.Text.Trim().ToUpper();
                    }
                    if (txtPara.Text.Trim().ToUpper() == "AcceptPartnerAWB" && txtAppkey.Text.Trim().ToUpper() == "Booking")
                    {
                        Session["AcceptPartnerAWB"] = txtValue.Text.Trim().ToUpper();
                    }


                    #region for Master Audit Log
                    //MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Paramsss = new object[7];
                    int k = 0;

                    //1
                    Paramsss.SetValue("System", k);
                    k++;

                    //2
                    string Value = txtPara.Text.ToString() + "-" + txtValue.Text.ToString();
                    Paramsss.SetValue(Value, k);
                    k++;

                    //3
                    Paramsss.SetValue("ADD", k);
                    k++;

                    //4
                    string Msg = "New System Config";
                    Paramsss.SetValue(Msg, k);
                    k++;

                    //5
                    string Desc = "";
                    Paramsss.SetValue(Desc, k);
                    k++;

                    //6

                    Paramsss.SetValue(Session["UserName"], k);
                    k++;

                    //7
                    Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                    k++;

                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Paramsss);
                    #endregion
                    getSystemConfiguration();
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Visible = true;
                    lblStatus.Text = "Record inserted successfully !!";
                    

                }
                else if (res == "UPDATED")
                {
                    if (txtPara.Text.Trim().ToUpper() == "ULDACTIVATE" && txtAppkey.Text.Trim().ToUpper() == "ULDACTIVE")
                    {
                        Session["ULDACT"] = txtValue.Text.Trim().ToUpper();
                    }
                    if (txtPara.Text.Trim().ToUpper() == "AcceptPartnerAWB" && txtAppkey.Text.Trim().ToUpper() == "Booking")
                    {
                        Session["AcceptPartnerAWB"] = txtValue.Text.Trim().ToUpper();
                    }

                    #region for Master Audit Log
                    //MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Paramsss = new object[7];
                    int k = 0;

                    //1
                    Paramsss.SetValue("System", k);
                    k++;

                    //2
                    string Value = txtPara.Text.ToString() + "-" + txtValue.Text.ToString();
                    Paramsss.SetValue(Value, k);
                    k++;

                    //3
                    Paramsss.SetValue("Update", k);
                    k++;

                    //4
                    string Msg = "System Config Updated";
                    Paramsss.SetValue(Msg, k);
                    k++;

                    //5
                    string Desc = "";
                    Paramsss.SetValue(Desc, k);
                    k++;

                    //6

                    Paramsss.SetValue(Session["UserName"], k);
                    k++;
                    DateTime dt = DateTime.Parse(Session["IT"].ToString()) ;
                    //7
                    Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                    k++;

                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Paramsss);
                    #endregion
                    getSystemConfiguration();
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Visible = true;
                    lblStatus.Text = "Record updated successfully !!";

                    
                    
                }
                else
                {
                }

            }
            catch (Exception ex)
            {
               
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string res = "";
                res = ObjBAL.addUpdateSystemConfiguration(txtPara.Text.ToString(), txtValue.Text.ToString(), txtAppkey.Text.ToString(), "D",txtDescription.Text.ToString());
                if (res == "DELETED")
                {
                    #region for Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Params = new object[7];
                    int i = 0;

                    //1
                    Params.SetValue("System", i);
                    i++;

                    //2
                    Params.SetValue(txtPara.Text+"-"+txtValue.Text.Trim(), i);
                    i++;

                    //3

                    Params.SetValue("DELETE", i);
                    i++;

                    //4

                    Params.SetValue("", i);
                    i++;


                    //5

                    Params.SetValue("", i);
                    i++;

                    //6

                    Params.SetValue(Session["UserName"], i);
                    i++;

                    //7
                    Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), i);
                    i++;


                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Params);
                    #endregion
                    getSystemConfiguration();
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Visible = true;
                    lblStatus.Text = "Record deleted successfully !!";
                    
                }
                else
                {
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                
                lblStatus.Text = string.Empty;

                #region Prepare Parameters
                object[] SystemInfo = new object[2];
                int i = 0;

                //0

                //int CatSrNo = 0;
                //if (ddlparam.SelectedIndex > 0)
                //    CatSrNo = int.Parse(ddlparam.SelectedValue);
                //SystemInfo.SetValue(CatSrNo, i);
                if (ddlparam.SelectedValue.ToString() == "All")
                {
                    SystemInfo.SetValue("", i);

                    i++;
                }
                else
                {
                    SystemInfo.SetValue(ddlparam.SelectedValue.ToString(), i);

                    i++;
                }

                SystemInfo.SetValue(txtsearchkey.Text.Trim(), i);
                i++;

                //1
               

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = ObjBAL.GetParameterList(SystemInfo);
                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    grdResult.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
              
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdResult.PageIndex = 0;
                                grdResult.DataSource = ds;
                                grdResult.DataMember = ds.Tables[0].TableName;
                                grdResult.AllowPaging = true;
                                grdResult.PageSize = 10;
                                grdResult.DataBind();
                                grdResult.Visible = true;
                                Session["ds"] = ds;
                                //btnClear_Click(null,null);
                                //ds.Clear();

                                //for (int j = 0; j < grdResult.Rows.Count; j++)
                                //{
                                //    if (((Label)(grdResult.Rows[j].FindControl("lblStatus"))).Text.ToString() == "True")
                                //    {
                                //        ((Label)(grdResult.Rows[j].FindControl("lblStatus"))).Text = "Active";
                                //    }
                                //    //string a = ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString();
                                //    else if (((Label)(grdResult.Rows[j].FindControl("lblStatus"))).Text.ToString() == "False")
                                //    {
                                //        ((Label)(grdResult.Rows[j].FindControl("lblStatus"))).Text = "InActive";
                                //    }
                                //}
                               

                            }
                            else
                            {
                                lblStatus.Text = "No records found";
                                lblStatus.ForeColor = Color.Red;
                                grdResult.DataSource = null;
                                grdResult.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        //public void MasterLog(string value)
        //{
        //    MasterAuditBAL ObjMAL = new MasterAuditBAL();
        //    #region for Master Audit Log
        //    #region Prepare Parameters
        //    object[] Paramsmaster = new object[7];
        //    int count = 0;

        //    //1

        //    Paramsmaster.SetValue("System", count);
        //    count++;

        //    //2
        //    if (txtsearchkey.Text == "")
        //    {
        //        if (ddlparam.SelectedItem.ToString() == "Select")
        //        { txtsearchkey.Text = grdResult.Columns[3].ToString() + grdResult.Columns[1].ToString(); }
        //    }
        //    Paramsmaster.SetValue(txtsearchkey.Text+ddlparam.SelectedItem.ToString(), count);
        //    count++;

        //    //3

        //    Paramsmaster.SetValue(value, count);
        //    count++;

        //    //4

        //    Paramsmaster.SetValue("", count);
        //    count++;


        //    //5

        //    Paramsmaster.SetValue("", count);
        //    count++;

        //    //6

        //    Paramsmaster.SetValue(Session["UserName"], count);
        //    count++;

        //    //7
        //    Paramsmaster.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), count);
        //    count++;


        //    #endregion Prepare Parameters
        //    ObjMAL.AddMasterAuditLog(Paramsmaster);
        //    #endregion

        //}
    }
}
