using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using System.Data.Sql;
using System.Data.SqlClient;
using QID.DataAccess;


namespace ProjectSmartCargoManager
{
    public partial class GLAccountList : System.Web.UI.Page
    {
        #region Variables

        DataSet dsSlabs = new DataSet();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BalGLAAcctCode obj = new BalGLAAcctCode();

        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ds"] = null;
                lblStatus.Text = "";
                AddRow();
                
                //  LoadGLgrid();
            }
        }

        #endregion
        private void AddRow()
        {
            lblStatus.Text = "";
            DataTable dtNewRow = new DataTable();
            if (Session["ds"] == null)
            {
                dtNewRow = null;
            }
            else
            {
              dtNewRow =((DataTable)Session["ds"]);

                //DataSet ds = new DataSet();
                //ds = (DataSet)Session["ds"];
                //////  DataTable dtNewRow = new DataTable();
                //dtNewRow = ds.Tables[0];
            //  //dtNewRow = ((DataView)Session["ds"]).Table;
            }

            if (dtNewRow == null)
            {
                dtNewRow = new DataTable();
                dtNewRow.Columns.Add("GLAccountCode");
                dtNewRow.Columns.Add("GLAccountDescription");
                dtNewRow.Columns.Add("GLAccountName");

                dtNewRow.Columns.Add("IsActive", typeof(bool));

            }
            try
            {
                DataRow l_Datarow = dtNewRow.NewRow();
                l_Datarow["GLAccountCode"] = "";
                l_Datarow["GLAccountDescription"] = "";
                l_Datarow["GLAccountName"] = "";
                //l_Datarow["UpdatedBy"] = "";
                //l_Datarow["UpdatedOn"] = "";
                l_Datarow["IsActive"] = 1;
                dtNewRow.Rows.Add(l_Datarow);

                GridView1.DataSource = dtNewRow;
                GridView1.DataBind();
                Session["ds"] = dtNewRow;


                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    ((TextBox)(GridView1.Rows[i].FindControl("txtGLADesc"))).Enabled = true;
                    ((TextBox)(GridView1.Rows[i].FindControl("txtAcctCode"))).ReadOnly = false;
                    ((TextBox)(GridView1.Rows[i].FindControl("txtAccName"))).ReadOnly = false;
                    ((CheckBox)(GridView1.Rows[i].FindControl("ChkIsActive"))).Checked = true;
                    //((TextBox)(GridView1.Rows[j].FindControl("grdtxtUpdatedOn"))).Enabled = false;
                    //((TextBox)(GridView1.Rows[j].FindControl("grdtxtUpdatedBy"))).Enabled = false;

                }

            }

            catch (Exception ex)
            {
            }
            finally
            {
                if (dtNewRow != null)
                    dtNewRow.Dispose();
            }


        }
        #region Load DropDowns
        public void LoadGLgrid()
        {
            try
            {
                DataSet ds = da.SelectRecords("spgetGLAccountdata");

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            GridView1.PageIndex = 0;
                            GridView1.DataSource = ds;
                            GridView1.DataMember = ds.Tables[0].TableName;
                            GridView1.DataBind();
                            GridView1.Visible = true;
                            Session["ds"] = ds;//.Tables[0];

                            //Session["ds"] = ds.Tables[0];

                            for (int i = 0; i < GridView1.Rows.Count; i++)
                            {

                                ((TextBox)(GridView1.Rows[i].FindControl("txtGLADesc"))).Enabled = false;
                                ((TextBox)(GridView1.Rows[i].FindControl("txtAcctCode"))).ReadOnly = true;
                                ((TextBox)(GridView1.Rows[i].FindControl("txtAccName"))).ReadOnly = true;
                                ((CheckBox)(GridView1.Rows[i].FindControl("ChkIsActive"))).Checked = true;
                                ////foreach (GridViewRow gvrw in GridView1.Rows)
                                //{

                                //  CheckBox chk = ((CheckBox)(GridView1.Rows[i].FindControl("ChkIsActive")));//.Checked;
                                //    if (chk.Checked)
                                //    {
                                //        ((CheckBox)(GridView1.Rows[i].FindControl("ChkIsActive"))).Checked = true;
                                //    }
                                //    else
                                //    {
                                //        ((CheckBox)(GridView1.Rows[i].FindControl("ChkIsActive"))).Checked = false;
                                //    }
                                //}


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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            DataSet dss = (DataSet)Session["ds"];
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = dss.Copy();
            GridView1.DataBind();



        }

        protected void GRDList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                string SrNo = ((HiddenField)GridView1.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("Hid")).Value;
                string gldesc = (((TextBox)(GridView1.Rows[Convert.ToInt32(SrNo)].FindControl("txtGLADesc"))).Text).ToString();
                string glcode = (((TextBox)(GridView1.Rows[Convert.ToInt32(SrNo)].FindControl("txtAcctCode"))).Text).ToString();
                string actName = (((TextBox)(GridView1.Rows[Convert.ToInt32(SrNo)].FindControl("txtAccName"))).Text).ToString();
                bool isactive = (((CheckBox)(GridView1.Rows[Convert.ToInt32(SrNo)].FindControl("ChkIsActive"))).Checked);
                SAVEGLCode(gldesc, glcode, actName, isactive);
                //Response.Redirect("~/OtherCharges.aspx?cmd=" + e.CommandName + "&SrNo=" + SrNo + "");

            }
            catch (Exception ex)
            {
                //lblStatus.Text = "" + ex.Message;
                //lblStatus.ForeColor = Color.Red;
            }

        }

        #region Update Button
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //try 
            //{
            //    lblStatus.Text = "";
            //    Button btn = (Button)sender;

            //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            //    if (gvr.RowIndex < 0)
            //        return;

            //    int rowindex = 0;
            //    rowindex= gvr.RowIndex;

            //    int gridIndex = 0;
            //    gridIndex = rowindex;

            //    string gldesc = (((TextBox)(GridView1.Rows[Convert.ToInt32(rowindex)].FindControl("txtGLADesc"))).Text).ToString();
            //    string glcode = (((TextBox)(GridView1.Rows[Convert.ToInt32(rowindex)].FindControl("txtAcctCode"))).Text).ToString();
            //    string actName = (((TextBox)(GridView1.Rows[Convert.ToInt32(rowindex)].FindControl("txtAccName"))).Text).ToString();
            //    bool isactive = (((CheckBox)(GridView1.Rows[Convert.ToInt32(rowindex)].FindControl("ChkIsActive"))).Checked);
            //    SAVEGLCode(gldesc, glcode,actName,isactive);
            //    LoadGLgrid();
            //    lblStatus.ForeColor = System.Drawing.Color.Green;
            //    lblStatus.Text = "Record Saved Successfully";
            //    ((TextBox)(GridView1.Rows[rowindex].FindControl("txtGLADesc"))).ReadOnly = true;
            //    ((TextBox)(GridView1.Rows[rowindex].FindControl("txtAcctCode"))).ReadOnly = true;
            //    ((TextBox)(GridView1.Rows[rowindex].FindControl("txtAccName"))).ReadOnly = true;



            //}
            //catch (Exception ex) { }
        }
        #endregion

        #region DBCall Save
        private void SAVEGLCode(string Desc, string Code, string AccountName, bool IsActive)
        {
            try
            {
                string[] PName = new string[] 
                {
                   "GLCode",
	               "Description",
                   "AccountName",
                   "IsActive"
                };

                SqlDbType[] PType = new SqlDbType[] 
                {
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.Bit
                };

                object[] PValue = new object[]
                {     
                    Code,
                    Desc,
                    AccountName,
                    IsActive
                };
                if (da.ExecuteProcedure("spAddGLcodes", PName, PType, PValue))
                { }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region Add Button
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //lblStatus.Text = "";

            string value = "ADD";
            MasterLog(value);
            AddRow();
            //    try 
            //    {

            //        DataSet ds = new DataSet();
            //        ds=(DataSet)Session["ds"];
            //        DataTable dt = new DataTable();
            //           dt= ds.Tables[0];
            //        DataRow dr = dt.NewRow();
            //        dt.Rows.Add(dr);

            //        GridView1.DataSource = dt;
            //        GridView1.DataBind();
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            if (dt.Rows[i + GridView1.PageIndex * GridView1.PageSize][1].ToString().Trim() != "")
            //            {
            //                ((TextBox)(GridView1.Rows[i].FindControl("txtGLADesc"))).ReadOnly = false;
            //                ((TextBox)(GridView1.Rows[i].FindControl("txtAcctCode"))).ReadOnly = true;
            //                ((TextBox)(GridView1.Rows[i].FindControl("txtAccName"))).ReadOnly = true;
            //                ((CheckBox)(GridView1.Rows[i].FindControl("ChkIsActive"))).Checked = true;

            //            }
            //            else
            //            {
            //                ((TextBox)(GridView1.Rows[i].FindControl("txtGLADesc"))).Enabled = true;
            //                ((TextBox)(GridView1.Rows[i].FindControl("txtAcctCode"))).Enabled = true;
            //                ((TextBox)(GridView1.Rows[i].FindControl("txtAccName"))).Enabled = true;
            //                ((CheckBox)(GridView1.Rows[i].FindControl("ChkIsActive"))).Checked = true;
            //            }
            //        }
            //        ds = null;

            //        ds.Tables.Add(dt);
            //        ds.AcceptChanges();
            //        Session["ds"] = ds;

            //    }
            //    catch (Exception ex) 
            //    { 

            //    }
        }

        #endregion

        #region Edit Button
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //string value = "EDIT";
            //MasterLog(value);

            try
            {
                lblStatus.Text = "";

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {

                    ((TextBox)(GridView1.Rows[i].FindControl("txtGLADesc"))).Enabled = false;
                    ((TextBox)(GridView1.Rows[i].FindControl("txtAcctCode"))).ReadOnly = true;
                    ((TextBox)(GridView1.Rows[i].FindControl("txtAccName"))).ReadOnly = true;


                    if (((RadioButton)(GridView1.Rows[i].FindControl("rdbGLAUpdate"))).Checked)
                    {
                        ((TextBox)(GridView1.Rows[i].FindControl("txtGLADesc"))).Enabled = true;
                        ((TextBox)(GridView1.Rows[i].FindControl("txtAcctCode"))).ReadOnly = false;
                        ((TextBox)(GridView1.Rows[i].FindControl("txtAccName"))).ReadOnly = false;

                        return;
                    }

                }
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Please Select GLA to Edit";
            }

            catch (Exception ex)
            { }
        }

        #endregion

        #region Delete Button
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
               // lblStatus.Text = "";
                //bool DelRes;// = false;
                //int cnt = 0;
                //DataSet ds = new DataSet();
                //ds = (DataSet)Session["ds"];
                //DataTable dtNewRow = new DataTable();
                //dtNewRow = ds.Tables[0];
                //dtNewRow = ((DataTable)Session["ds"]);

              //  DataSet ds = (DataSet)Session["ds"];
               // DataTable dt = ds.Tables[0];

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    if (((RadioButton)(GridView1.Rows[i].FindControl("rdbGLAUpdate"))).Checked)
                    {

                        string GLCode = ((TextBox)GridView1.Rows[i].FindControl("txtAcctCode")).Text;
                        string GLDesc = ((TextBox)GridView1.Rows[i].FindControl("txtGLADesc")).Text;
                        //string AccName = ((TextBox)GridView1.Rows[i].FindControl("txtAccName")).Text;
                        //bool isactive = ((CheckBox)GridView1.Rows[i].FindControl("ChkIsActive")).Checked;
                        object[] PValue = new object[] 
                        { 
                         GLCode, GLDesc
                         //,AccName,isactive
                        };
                        string[] PName = new string[]
                        {
                         "GLCode","GLDesc"
                         //,"AccName","isactive"
                        };
                        SqlDbType[] PType = new SqlDbType[]
                        {
                         SqlDbType.VarChar, SqlDbType.VarChar
                        };
                        if (da.ExecuteProcedure("spDeleteGLcodes", PName, PType, PValue))
                        {

                            string value = "DELETE";
                            MasterLog(value);


                            btnList_Click(null, null);
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                            lblStatus.Text = "Record Deleted Successfully";
                        }
                       
                    }
                }
                btnList_Click(null, null);
                //if (DelRes)
                //{
                //    // LoadGLgrid();
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Text = "Record Deleted Successfully";
                //}


            }
            catch (Exception ex)
            { }
        }
        #endregion

        protected void btnList_Click(object sender, EventArgs e)
        {
            
            DataSet ds = new DataSet();
            object[] paramValue = new object[5];
            int i = 0;
            paramValue.SetValue(txtGlAccount.Text.Trim(), i);
            i++;
            paramValue.SetValue(txtAccName.Text.Trim(), i);
            i++;
            //paramValue.SetValue(txtGlDesc.Text.Trim(),i);
            //i++;
            
            
            //paramValue.SetValue(ChkIsActive.Checked, i);

            string[] paramNames = { "GLCode", "AccountName" };
            SqlDbType[] paramType = { SqlDbType.VarChar, SqlDbType.VarChar };

            //string[] paramNames = { "GLCode", "AccountName", "Description", "IsActive" };
            //SqlDbType[] paramType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit };

            ds = da.SelectRecords("ListGLCodes", paramNames, paramValue, paramType);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                lblStatus.Text = "";
                GridView1.DataSource = ds.Tables[0];
                GridView1.DataBind();

                //txtAcctCode.Text = txtGLADesc.Text = txtAccName.Text = string.Empty;

               // txtGlDesc.Text = "";
                txtGlAccount.Text = "";
                txtAccName.Text = "";
                ChkIsActive.Checked = true;
                Session["ds"] = ds.Tables[0];
                //BindGridDropDown(ds.Tables[0]);
            }
            else
            {
                lblStatus.Text = "Data not found";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                Session["ds"] = null;
                GridView1.DataSource = null;
                GridView1.DataBind();
                AddRow();
                return;
            }

           // LoadGLgrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            txtAccName.Text = "";
            txtGlAccount.Text = "";
            //txtGlDesc.Text = "";
            ChkIsActive.Checked = true;
            Session["ds"] = null;
            AddRow();
        }

        protected void btnSave_Click1(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                int count = 0;
                bool result = false;

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    if (((RadioButton)GridView1.Rows[i].FindControl("rdbGLAUpdate")).Checked)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    lblStatus.Text = "Select atleast one record";

                    return;
                }

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    if (((RadioButton)GridView1.Rows[i].FindControl("rdbGLAUpdate")).Checked)
                    {
                        string value = "SAVE";
                        MasterLog(value);

                     object [] paramValue=new object[4];
                     paramValue[0] = ((TextBox)GridView1.Rows[i].FindControl("txtGLADesc")).Text.Trim();
                     paramValue[1] = ((TextBox)GridView1.Rows[i].FindControl("txtAcctCode")).Text.Trim();
                     paramValue[2] = ((TextBox)GridView1.Rows[i].FindControl("txtAccName")).Text.Trim();
                     paramValue[3] = ((CheckBox)GridView1.Rows[i].FindControl("ChkIsActive")).Checked;

                     string[] paramName = {"Description","GLCode","AccountName", "IsActive" };
                     SqlDbType[] paramType = {  SqlDbType.VarChar,  SqlDbType.VarChar,SqlDbType.VarChar,  SqlDbType.Bit};

                     result = da.ExecuteProcedure("spAddGLcodes", paramName, paramType, paramValue);
                    }
                }

                btnList_Click(null, null);
                if (result)
                {
                    lblStatus.Text = "Record added Successfully";
                   lblStatus.ForeColor =System.Drawing.Color.Green;
                }

            }
            catch (Exception ex)
            { }
        }




        public void MasterLog(string value)
        {
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            #region for Master Audit Log
            #region Prepare Parameters
            object[] Paramsmaster = new object[7];
            int count = 0;

            //1

            Paramsmaster.SetValue("GLAccount Master", count);
            count++;

            //2
            string data = string.Empty;

            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                if (((RadioButton)(GridView1.Rows[j].FindControl("rdbGLAUpdate"))).Checked == true)

                    data = (((TextBox)(GridView1.Rows[j].FindControl("txtAccName"))).Text.ToString());

                Paramsmaster.SetValue(data, count);
            }



            count++;

            //3

            Paramsmaster.SetValue(value, count);
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
    }
}
