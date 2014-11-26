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
    public partial class AccountCategoryMaster : System.Web.UI.Page
    {

        #region Variables

        DataSet dsSlabs = new DataSet();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BalGLAAcctCode obj = new BalGLAAcctCode();

        #endregion
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string value = "ADD";
            MasterLog(value);
            AddRow();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string value = "SAVE";
            MasterLog(value);
            //lblStatus.Text = "";
            try
            {
                int count = 0;
                //bool result = false;

                for (int i = 0; i < grvCategoryList.Rows.Count; i++)
                {
                    if (((RadioButton)grvCategoryList.Rows[i].FindControl("rdbchkRow")).Checked)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    lblStatus.Text = "Select atleast one record";

                    return;
                }

                for (int i = 0; i < grvCategoryList.Rows.Count; i++)
                {
                    if (((RadioButton)grvCategoryList.Rows[i].FindControl("rdbchkRow")).Checked)
                    {
                        //string value = "SAVE";
                        //MasterLog(value);

                        object[] paramValue = new object[4];
                        paramValue[0] = ((TextBox)grvCategoryList.Rows[i].FindControl("grdtxtCategoryID")).Text.Trim();
                        paramValue[1] = ((TextBox)grvCategoryList.Rows[i].FindControl("grdtxtCategoryName")).Text.Trim();
                        paramValue[2] = ((TextBox)grvCategoryList.Rows[i].FindControl("grdtxtCategoryDescription")).Text.Trim();
                        paramValue[3] = ((CheckBox)grvCategoryList.Rows[i].FindControl("chkActive")).Checked;
                      
                        string[] paramName = { "CategoryId", "CategoryName", "CategoryDescription", "IsActive" };
                        SqlDbType[] paramType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit };
                        DataSet dsSave = new DataSet();
                        dsSave = da.SelectRecords("spAddAccountMaster", paramName, paramValue, paramType);



                        if (dsSave != null && dsSave.Tables.Count > 0)
                        {
                            if (dsSave.Tables[0].Rows.Count > 0)
                            {
                                if ((dsSave.Tables[0].Rows[0][0].ToString()) == "INSERT")
                                {

                                    btnList_Click(null, null);
                                    lblStatus.Visible = true;
                                    lblStatus.Text = "Record Inserted Successfully...";
                                    lblStatus.ForeColor = System.Drawing.Color.Green;
                                   
                                }
                                if ((dsSave.Tables[0].Rows[0][0].ToString()) == "UPDATE")
                                {
                                    lblStatus.Visible = true;
                                    lblStatus.Text = "Record Updated Successfully...";
                                    lblStatus.ForeColor = System.Drawing.Color.Green;
                                }
                                
                            }
                        }
                       // result = da.ExecuteProcedure("spAddAccountMaster", paramName, paramType, paramValue);
                    }
                }

               // btnList_Click(null, null);
                //if (result)
                //{
                //    lblStatus.Text = "Record added Successfully";
                //    lblStatus.ForeColor = System.Drawing.Color.Green;
                //}

            }
            catch (Exception ex)
            { }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {


                for (int i = 0; i < grvCategoryList.Rows.Count; i++)
                {
                    if (((RadioButton)(grvCategoryList.Rows[i].FindControl("rdbchkRow"))).Checked)
                    {

                        string CategoryID = ((TextBox)grvCategoryList.Rows[i].FindControl("grdtxtCategoryID")).Text;
                        string CategoryName = ((TextBox)grvCategoryList.Rows[i].FindControl("grdtxtCategoryName")).Text;
                        //string AccName = ((TextBox)GridView1.Rows[i].FindControl("txtAccName")).Text;
                        //bool isactive = ((CheckBox)GridView1.Rows[i].FindControl("ChkIsActive")).Checked;
                        object[] PValue = new object[] 
                        { 
                         CategoryID, CategoryName
                         //,AccName,isactive
                        };
                        string[] PName = new string[]
                        {
                         "CategoryID","CategoryName"
                         //,"AccName","isactive"
                        };
                        SqlDbType[] PType = new SqlDbType[]
                        {
                         SqlDbType.VarChar, SqlDbType.VarChar
                        };
                       DataSet ds1 = new DataSet();
                       ds1 = da.SelectRecords("sp_DeleteAccountCategory", PName, PValue, PType);



                       if (ds1 != null && ds1.Tables.Count > 0)
                       {
                           if (ds1.Tables[0].Rows.Count > 0)
                           {
                               if ((ds1.Tables[0].Rows[0][0].ToString()) == "DELETE")
                               {
                                   btnList_Click(null, null);
                                   string value = "DELETE";
                                   MasterLog(value);

                                   lblStatus.Visible = true;
                                   lblStatus.Text = "Record Deleted Successfully...";
                                   lblStatus.ForeColor = System.Drawing.Color.Green;
                                 
                               }
                               if ((ds1.Tables[0].Rows[0][0].ToString()) == "NO")
                               {
                                   lblStatus.Visible = true;
                                   lblStatus.Text = "Record doesn't exist...";
                                   lblStatus.ForeColor = System.Drawing.Color.Green;
                               }
                               if ((ds1.Tables[0].Rows[0][0].ToString()) == "SYSTEM")
                               {
                                   lblStatus.Visible = true;
                                   lblStatus.Text = "Record cant be deleted...";
                                   lblStatus.ForeColor = System.Drawing.Color.Green;
                               
                               }
                           }
                       }
                       
                       ////if (da.ExecuteProcedure("sp_DeleteAccountCategory", PName, PType, PValue))
                       // {
                            
                       //     if (da.ToString() == "DELETE") { }


                       //     string value = "DELETE";
                       //     MasterLog(value);


                       //     btnList_Click(null, null);
                       //     lblStatus.ForeColor = System.Drawing.Color.Green;
                       //     lblStatus.Text = "Record Deleted Successfully";
                       // }

                       // //else
                       // {


                       //     string value = "DELETE";
                       //     MasterLog(value);


                       //     btnList_Click(null, null);
                       //     lblStatus.ForeColor = System.Drawing.Color.Green;
                       //     lblStatus.Text = "Record NOtDeleted ";
                       // }
                    }
                }
                //btnList_Click(null, null);
                lblStatus.Visible = true;
                //lblStatus.ForeColor = System.Drawing.Color.Green;
                //lblStatus.Text = "Record Deleted Successfully";
            }
            catch (Exception ex)
            { }
        }
      
        protected void btnList_Click(object sender, EventArgs e)
        {

            DataSet ds = new DataSet();
            object[] paramValue = new object[5];
            int i = 0;
            paramValue.SetValue(txtCategoryID.Text.Trim(), i);
            i++;
            paramValue.SetValue(txtCategoryName.Text.Trim(), i);
            i++;
            //paramValue.SetValue(txtGlDesc.Text.Trim(),i);
            //i++;


           paramValue.SetValue(chkActive.Checked, i);
           i++;
            string[] paramNames = { "CategoryID", "CategoryName","IsActive" };
            SqlDbType[] paramType = { SqlDbType.VarChar, SqlDbType.VarChar ,SqlDbType.Bit};

            //string[] paramNames = { "GLCode", "AccountName", "Description", "IsActive" };
            //SqlDbType[] paramType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit };

            ds = da.SelectRecords("spListAccountCategory", paramNames, paramValue, paramType);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                lblStatus.Text = "";
                grvCategoryList.DataSource = ds.Tables[0];
                grvCategoryList.DataBind();

                //txtAcctCode.Text = txtGLADesc.Text = txtAccName.Text = string.Empty;

                // txtGlDesc.Text = "";
                txtCategoryID.Text = "";
                txtCategoryName.Text = "";
                chkActive.Checked = true;
                Session["ds"] = ds.Tables[0];
                //BindGridDropDown(ds.Tables[0]);
            }
            else
            {
                lblStatus.Text = "Data not found";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                Session["ds"] = null;
                grvCategoryList.DataSource = null;
                grvCategoryList.DataBind();
                AddRow();
                return;
            }



        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            txtCategoryID.Text = "";
            txtCategoryName.Text = "";
            //txtGlDesc.Text = "";
            chkActive.Checked = true;
            Session["ds"] = null;
            AddRow();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {

            {
                //string value = "EDIT";
                //MasterLog(value);

                try
                {
                    lblStatus.Text = "";

                    for (int i = 0; i < grvCategoryList.Rows.Count; i++)
                    {

                        ((TextBox)(grvCategoryList.Rows[i].FindControl("grdtxtCategoryDescription"))).Enabled = false;
                        ((TextBox)(grvCategoryList.Rows[i].FindControl("grdtxtCategoryID"))).ReadOnly = true;
                        ((TextBox)(grvCategoryList.Rows[i].FindControl("grdtxtCategoryDescription"))).ReadOnly = true;


                        if (((RadioButton)(grvCategoryList.Rows[i].FindControl("rdbchkRow"))).Checked)
                        {
                            ((TextBox)(grvCategoryList.Rows[i].FindControl("grdtxtCategoryDescription"))).Enabled = true;
                            ((TextBox)(grvCategoryList.Rows[i].FindControl("grdtxtCategoryID"))).ReadOnly = false;
                            ((TextBox)(grvCategoryList.Rows[i].FindControl("grdtxtCategoryDescription"))).ReadOnly = false;

                            return;
                        }

                    }
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Please Select  to Edit";
                }

                catch (Exception ex)
                { }
            }

        }

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
                dtNewRow = ((DataTable)Session["ds"]);

                //DataSet ds = new DataSet();
                //ds = (DataSet)Session["ds"];
                //////  DataTable dtNewRow = new DataTable();
                //dtNewRow = ds.Tables[0];
                //  //dtNewRow = ((DataView)Session["ds"]).Table;
            }

            if (dtNewRow == null)
            {
                dtNewRow = new DataTable();
                dtNewRow.Columns.Add("CategoryID");
                
                dtNewRow.Columns.Add("CategoryName");
                dtNewRow.Columns.Add("CategoryDescription");

                dtNewRow.Columns.Add("IsActive", typeof(bool));

            }
            try
            {
                DataRow l_Datarow = dtNewRow.NewRow();
                l_Datarow["CategoryID"] = "";
                l_Datarow["CategoryName"] = "";
                l_Datarow["CategoryDescription"] = "";
                //l_Datarow["UpdatedBy"] = "";
                //l_Datarow["UpdatedOn"] = "";
                l_Datarow["IsActive"] = 1;
                dtNewRow.Rows.Add(l_Datarow);

                grvCategoryList.DataSource = dtNewRow;
                grvCategoryList.DataBind();
                Session["ds"] = dtNewRow;


                for (int i = 0; i < grvCategoryList.Rows.Count; i++)
                {
                    ((TextBox)(grvCategoryList.Rows[i].FindControl("grdtxtCategoryDescription"))).Enabled = true;
                    ((TextBox)(grvCategoryList.Rows[i].FindControl("grdtxtCategoryID"))).ReadOnly = false;
                    ((TextBox)(grvCategoryList.Rows[i].FindControl("grdtxtCategoryName"))).ReadOnly = false;
                    ((CheckBox)(grvCategoryList.Rows[i].FindControl("chkActive"))).Checked = true;
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


        public void MasterLog(string value)
        {
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            #region for Master Audit Log
            #region Prepare Parameters
            object[] Paramsmaster = new object[7];
            int count = 0;

            //1

            Paramsmaster.SetValue("Account Category Master", count);
            count++;

            //2
            string data = null;

            for (int j = 0; j < grvCategoryList.Rows.Count; j++)
            {
                if (((RadioButton)(grvCategoryList.Rows[j].FindControl("rdbchkRow"))).Checked == true)

                    data = (((TextBox)(grvCategoryList.Rows[j].FindControl("grdtxtCategoryID"))).Text.ToString());

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

        protected void grvCategoryList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dtnew =
                new DataTable();
            
              dtnew= (DataTable)Session["ds"];
            //DataSet ds = (DataSet)Session["ds"];
            //dtnew = ds.Tables[0];
            grvCategoryList.PageIndex = e.NewPageIndex;
            grvCategoryList.DataSource = dtnew.Copy();
            grvCategoryList.DataBind();

        
        
        }
        protected void grvCategoryList_RowCommand(object sender, GridViewCommandEventArgs e)
        { 
        
        
        }
    }
}
