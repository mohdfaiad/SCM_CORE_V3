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

namespace ProjectSmartCargoManager
{
    public partial class CCFSMaster : System.Web.UI.Page
    {

        #region Variables
     BALCCSFMaster objBAL = new BALCCSFMaster();
        MasterAuditBAL ObjMAL = new MasterAuditBAL();

        #endregion    //Changes for test

        #region Page Laod
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Page.Title = "Notifications";
                Session["CCFSMaster"] = null;
            
                CreateEmptyRow();
                TxtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
       
              
              
            }
        }

        #endregion

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {

            {
                try
                {
                    lblStatus.Text = string.Empty;

                    try
                    {
                        if (TxtFrmDt.Text == "" || txtToDt.Text == "")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please Enter From Date and To Date ";
                            TxtFrmDt.Focus();
                            return;
                        }
                        if (TxtFrmDt.Text.Trim() != "" || txtToDt.Text.Trim() != "")
                        {
                            DateTime dt1 = DateTime.ParseExact(TxtFrmDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                            DateTime dt2 = DateTime.ParseExact(txtToDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);
                          
                        
                            int chk = DateTime.Compare(dt1, dt2);
                            if (chk > 0)
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Please enter valid To date";
                                //TxtFrmDt.Focus();
                                return;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                        //txtFromdate.Focus();
                        return;
                    }
                    DateTime frdt = DateTime.ParseExact(TxtFrmDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime todt = DateTime.ParseExact(txtToDt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);
                          
                    string fromdate = frdt.ToString("MM/dd/yyyy");
                    string todate =todt.ToString("MM/dd/yyyy");
                    #region Prepare Parameters
                    object[] CCSFMasterListInfo = new object[4];
                    int i = 0;

                    //0
                    CCSFMasterListInfo.SetValue(txtIACCode.Text, i);
                    i++;

                    CCSFMasterListInfo.SetValue(txtApprovalNo.Text, i);
                    i++;
                   
                    CCSFMasterListInfo.SetValue(frdt, i);
                    i++;
                  
                    CCSFMasterListInfo.SetValue(todt, i);
                    i++;
                    #endregion Prepare Parameters

                    DataSet ds = new DataSet();
                    ds = objBAL.ListCCSFDetails(CCSFMasterListInfo);
                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    grvCCSFMaster.PageIndex = 0;
                                    grvCCSFMaster.DataSource = ds;
                                    grvCCSFMaster.DataMember = ds.Tables[0].TableName;
                                    grvCCSFMaster.DataBind();
                                    grvCCSFMaster.Visible = true;
                                    Session["CCFSMaster"] = ds.Tables[0];

                               

                                    //ds.Clear();
                                   // for (int j = 0; j < grvCCSFMaster.Rows.Count; j++)
                                   //{
                                   //    ((TextBox)(grvCCSFMaster.Rows[j].FindControl("txtCCSFName"))).Enabled = false;


                                   //}


                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Visible = true;
                                    lblStatus.Text = "No records found...";
                                    grvCCSFMaster.PageIndex = 0;
                                }
                            }
                        }
                    }
                    btnSave.Enabled = true;
                    btnAdd.Enabled = true;
                    
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion

        #region Button Save

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool CheckForSelect = false;
            lblStatus.Text = string.Empty;
            int parmcount = 0;
            for (int i = 0; i < grvCCSFMaster.Rows.Count; i++)
            {
                CheckBox chkBox = (CheckBox)grvCCSFMaster.Rows[i].FindControl("chkSelect");
                if (chkBox.Checked)
                {
                    CheckForSelect = true;
                   
                    parmcount++;
                    TextBox txtCCSFCode = (TextBox)grvCCSFMaster.Rows[i].FindControl("txtCCSFCode");
                    TextBox txtCCSFName = (TextBox)grvCCSFMaster.Rows[i].FindControl("txtCCSFName");
                    TextBox txtIACCode  = (TextBox)grvCCSFMaster.Rows[i].FindControl("txtIACCode");
                    TextBox txtIACName = (TextBox)grvCCSFMaster.Rows[i].FindControl("txtIACName");
                    TextBox txtApprovalNo = (TextBox)grvCCSFMaster.Rows[i].FindControl("txtApprovalNo");
                    TextBox txtCity = (TextBox)grvCCSFMaster.Rows[i].FindControl("txtCity");
                    TextBox txtStreetAddress = (TextBox)grvCCSFMaster.Rows[i].FindControl("txtStreetAddress");
                    TextBox txtState = (TextBox)grvCCSFMaster.Rows[i].FindControl("txtState");
                    TextBox txtCountry = (TextBox)grvCCSFMaster.Rows[i].FindControl("txtCountry");
                    TextBox txtZip = (TextBox)grvCCSFMaster.Rows[i].FindControl("txtZip");
                    TextBox txtExpDate = (TextBox)grvCCSFMaster.Rows[i].FindControl("txtExpDate");
                    CheckBox chkIsActive = ((CheckBox)grvCCSFMaster.Rows[i].FindControl("chkboxSelect"));
                    //txtUpdatedBy.Text = Session["UserName"].ToString();
                    //txtUpdatedOn.Text = Session["IT"].ToString();
                    string currentdt = txtExpDate.Text;
                     DateTime Dt;
                     try
                     {
                         Dt = DateTime.ParseExact(currentdt, "dd/MM/yyyy", null);
                     }
                     catch (Exception ex)
                     {
                         lblStatus.Text = "Enter date in dd/MM/yyyy format";
                         lblStatus.ForeColor = Color.Red;
                         return;
                     }
                    string ExpDt = Dt.ToString("MM/dd/yyyy");
                    if (txtApprovalNo.Text == "")
                    {
                        lblStatus.Text = "Please enter Approval Number";
                        return;
                    }

                    #region Prepare Parameters
                    object[] CCSFMasterListInfo = new object[14];
                    int j = 0;

                    CCSFMasterListInfo.SetValue(txtCCSFCode.Text, j);
                    j++;
                    CCSFMasterListInfo.SetValue(txtCCSFName.Text, j);
                    j++;
                    CCSFMasterListInfo.SetValue(txtIACCode.Text, j);
                    j++;
                    CCSFMasterListInfo.SetValue(txtIACName.Text, j);
                    j++;
                    CCSFMasterListInfo.SetValue(txtApprovalNo.Text, j);
                    j++;
                    CCSFMasterListInfo.SetValue(txtCity.Text, j);
                    j++;
                    CCSFMasterListInfo.SetValue(txtState.Text, j);
                    j++;
                    CCSFMasterListInfo.SetValue(txtCountry.Text, j);
                    j++;
                    CCSFMasterListInfo.SetValue(txtZip.Text, j);
                    j++;
                    CCSFMasterListInfo.SetValue(chkIsActive.Checked, j);
                    j++;
                    CCSFMasterListInfo.SetValue(Dt, j);
                    j++; 
                    CCSFMasterListInfo.SetValue(txtStreetAddress.Text, j);
                    j++;
                    CCSFMasterListInfo.SetValue(Session["IT"], j);
                    j++;
                    CCSFMasterListInfo.SetValue(Session["UserName"], j);
                    j++; 
               

                   
                    #endregion Prepare Parameters

                    DataSet dsCategory = objBAL.InsertCCSFCode(CCSFMasterListInfo);

                    if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                    {
                        if (dsCategory.Tables[0].Rows[0][0].ToString() == "INSERTED")
                        {
                          
                      
                            #region Update User Details
                           //object[] CCSFMasterInfo = new object[3];
                           //int c = 0;
                           //CCSFMasterInfo.SetValue(txtApprovalNo.Text, c);
                           //c++;
                            
                           // CCSFMasterInfo.SetValue(Convert.ToDateTime(Session["IT"].ToString()), c);
                           //c++;


                           //CCSFMasterInfo.SetValue(Session["UserName"].ToString(), c);
                           //c++;

                       
                           //objBAL.CCSFMasterInsert(CCSFMasterInfo);

                           #endregion

                           #region for Master Audit Log
                           #region Prepare Parameters
                           object[] Paramsmaster = new object[7];
                           int count = 0;

                           //1

                           Paramsmaster.SetValue("CCSF Master", count);
                           count++;

                           //2
                           Paramsmaster.SetValue(txtApprovalNo.Text, count);
                           count++;

                           //3

                           Paramsmaster.SetValue("SAVE", count);
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

                           btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Visible = true;
                            lblStatus.Text = "CCSF Code Inserted Successfully !!";

                        }
                        else if (dsCategory.Tables[0].Rows[0][0].ToString() == "UPDATED")
                        {
                           

                            #region Update User Details
                            object[] CCSFMasterInfo = new object[3];
                            int c = 0;
                            CCSFMasterInfo.SetValue(txtApprovalNo.Text, c);
                                c++;
                                CCSFMasterInfo.SetValue(Convert.ToDateTime(Session["IT"].ToString()), c);
                                c++;
                                CCSFMasterInfo.SetValue(Session["UserName"].ToString(), c);
                            c++;

                           

                            objBAL.CCSFMasterUpdate(CCSFMasterInfo);

                            #endregion

                            #region for Master Audit Log
                            #region Prepare Parameters
                            object[] Paramsmaster = new object[7];
                            int count = 0;

                            //1

                            Paramsmaster.SetValue("CCSF Master", count);
                            count++;

                            //2
                            Paramsmaster.SetValue(txtApprovalNo.Text, count);
                            count++;

                            //3

                            Paramsmaster.SetValue("UPDATE", count);
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

                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Visible = true;
                            lblStatus.Text = "CCFS Code Updated Successfully !!";

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
            if (parmcount == 0)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Visible = true;
                lblStatus.Text = "Please select a checkbox !!";
                return;
            }
            btnSave.Enabled = true;
            btnAdd.Enabled = true;
          
        }

        #endregion

        #region Add Click
        private void ADD()
        {
            DataTable dtNewList = new DataTable();
            if (Session["CCFSMaster"] == null)
            {
                dtNewList = null;
            }
            else
            {
                dtNewList = (DataTable)Session["CCFSMaster"];
            }
            if (dtNewList == null)
            {
                dtNewList = new DataTable();
                //dtNewList.Columns.Add("CostCenterID");
                //dtNewList.Columns.Add("CostCenterName");
                //dtNewList.Columns.Add("CostCenterDescription");
                //dtNewList.Columns.Add("UpdatedBy");
                //dtNewList.Columns.Add("UpdatedOn");
                //dtNewList.Columns.Add("IsActive", typeof(bool));

                dtNewList.Columns.Add("CCSFCode");
                dtNewList.Columns.Add("CCSFName");
                dtNewList.Columns.Add("IACCode");
                dtNewList.Columns.Add("IACName");
                dtNewList.Columns.Add("ApprovalNo");
                dtNewList.Columns.Add("City");
                dtNewList.Columns.Add("ExpirationDt");
                dtNewList.Columns.Add("StreetAddress");
                dtNewList.Columns.Add("State");
                dtNewList.Columns.Add("Country");
                dtNewList.Columns.Add("IsActive",typeof(bool));
                dtNewList.Columns.Add("Zip");

            }
            //DataSet dtNewList = null;
            lblStatus.Text = "";
            try
            {
                string currentdt = System.DateTime.Now.ToString("dd/MM/yyyy");
                DateTime Dt = DateTime.ParseExact(currentdt, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);              
                string expdate = Dt.ToString("MM/dd/yyyy");

                DataRow l_Datarow = dtNewList.NewRow();

                l_Datarow["CCSFCode"] = "";
                l_Datarow["CCSFName"] = "";
                l_Datarow["IACCode"] = "";
                l_Datarow["IACName"] = "";
                l_Datarow["ApprovalNo"] = "";
                l_Datarow["City"] = "";
                l_Datarow["ExpirationDt"] = currentdt; //Session["IT"].ToString();
                l_Datarow["StreetAddress"] = "";
                l_Datarow["State"] = "";
                l_Datarow["Country"] = "";
                l_Datarow["IsActive"] = false;
                l_Datarow["Zip"] = "";

                dtNewList.Rows.Add(l_Datarow);

                grvCCSFMaster.DataSource = dtNewList;
                grvCCSFMaster.DataBind();
                Session["CCFSMaster"] = dtNewList;

              

            }

            catch (Exception ex)
            {
            }
            finally
            {
                if (dtNewList != null)
                    dtNewList.Dispose();
            }
        }
        #endregion

        #region Button Add New Row

        protected void btnAdd_Click(object sender, EventArgs e)
        {
           lblStatus.Text = string.Empty;

           ADD();
          
        }

        #endregion

        #region Button Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                //Response.Redirect("~/IACMaster.aspx");
                TxtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtIACCode.Text = txtApprovalNo.Text = string.Empty;
                Session["CCFSMaster"] = null;

                CreateEmptyRow();
                
            }
            catch (Exception ex)
            { }
        }

        #endregion

        #region Create Empty Row 
        public void CreateEmptyRow()
        {
          
            DataTable DsAddNewRow = null;

            try
            {
                string currentdt = System.DateTime.Now.ToString("dd/MM/yyyy");
                DateTime Dt = DateTime.ParseExact(currentdt, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);              
                string expdate =Dt.ToString("MM/dd/yyyy");

                //Deepak Modified Code
                #region Add New row to grid
                if (Session["CCFSMaster"] != null)
                {
                    DsAddNewRow = ((DataTable)Session["CCFSMaster"]).Copy();
                    DataRow row = DsAddNewRow.NewRow();
                    row["CCSFCode"] = "";
                    row["CCSFName"]="";
                    row["IACCode"] = "";
                    row["IACName"] = "";
                    row["ApprovalNo"] = "";
                    row["City"] = "";
                    row["ExpirationDt"] = currentdt;//Session["IT"].ToString();
                    row["StreetAddress"] = "";
                    row["State"] = "";
                    row["Country"] = "";
                    row["IsActive"] = false;
                    row["Zip"] = "";

                    DsAddNewRow.Rows.Add(row);

                    Session["CCFSMaster"] = DsAddNewRow.Copy();

                    grvCCSFMaster.DataSource = DsAddNewRow.Copy();
                    grvCCSFMaster.DataBind();


                    DsAddNewRow.Dispose();
                }
                else
                {

                    DataTable dt = new DataTable();

                    dt.Columns.Add("CCSFCode");
                    dt.Columns.Add("CCSFName");
                    dt.Columns.Add("IACCode");
                    dt.Columns.Add("IACName");
                    dt.Columns.Add("ApprovalNo");
                    dt.Columns.Add("City");
                    dt.Columns.Add("ExpirationDt");
                    dt.Columns.Add("StreetAddress");
                    dt.Columns.Add("State");
                    dt.Columns.Add("Country");
                    dt.Columns.Add("IsActive");
                    dt.Columns.Add("Zip");

                    DataRow dr = dt.NewRow();
                    dr["CCSFCode"] = "";
                    dr["CCSFName"] = "";
                    dr["IACCode"] = "";
                    dr["IACName"] = "";
                    dr["ApprovalNo"] = "";
                    dr["City"] = "";
                    dr["ExpirationDt"] = currentdt; //Session["IT"].ToString();
                    dr["State"] = "";
                    dr["StreetAddress"] = "";
                    dr["Country"] = "";
                    dr["IsActive"] = false;
                    dr["Zip"] = "";
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();

                    grvCCSFMaster.DataSource = dt;
                    grvCCSFMaster.DataBind();
                    dt.Dispose();


                }



                #endregion

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
          
        }

        #endregion

        #region grvCCSFMaster_PageIndexChanging
        protected void grvCCSFMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable ds = (DataTable)Session["CCFSMaster"];
                grvCCSFMaster.PageIndex = e.NewPageIndex;
                grvCCSFMaster.DataSource = ds;
                grvCCSFMaster.DataBind();

              
           
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        protected void btnDelete_Click(object sender, EventArgs e)
        {

            {
                DataSet dsDel = null;
                try
                {
                    bool chkdel = false;
                    for (int i = 0; i < grvCCSFMaster.Rows.Count; i++)
                    {

                        if (((CheckBox)grvCCSFMaster.Rows[i].FindControl("chkSelect")).Checked == true)
                        {
                            chkdel = true;
                            string txtCCSFName = ((TextBox)(grvCCSFMaster.Rows[i].FindControl("txtCCSFName"))).Text;

                            #region Prepare Parameters
                            object[] CCSFMasterListInfo = new object[1];
                            int j = 0;
                            //0
                            CCSFMasterListInfo.SetValue(txtCCSFName, j);
                            j++;
                            #endregion Prepare Parameters

                            //DataSet dsDel = objBAL.DeleteCategory(CCSFMasterListInfo);

                            dsDel = objBAL.DeleteCCSFCode(CCSFMasterListInfo);

                            if (dsDel != null && dsDel.Tables.Count > 0)
                            {
                                if (dsDel.Tables[0].Rows.Count > 0)
                                {
                                    if ((dsDel.Tables[0].Rows[0][0].ToString()) == "DELETED")
                                    {
                                        btnList_Click(null, null);
                                        lblStatus.Visible = true;
                                        lblStatus.Text = "Record Deleted Successfully...";
                                        lblStatus.ForeColor = Color.Red;
                                    

                                        #region for Master Audit Log
                                        #region Prepare Parameters
                                        object[] Paramsmaster = new object[7];
                                        int count = 0;

                                        //1

                                        Paramsmaster.SetValue("CCSF Master", count);
                                        count++;

                                        //2
                                        Paramsmaster.SetValue(txtCCSFName.ToString(), count);
                                      
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

                                      
                                    }
                                    else
                                    {
                                        lblStatus.Text = "Record doesn't exist...";
                                        lblStatus.ForeColor = Color.Red;
                                    }
                                }
                            }

                        }
                    }
                    if (chkdel != true)
                    {
                        lblStatus.Text = "Please select Record(s).";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                }
                catch (Exception ex)
                { }
            }
        }
    }
}

