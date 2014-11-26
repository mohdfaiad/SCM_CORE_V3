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
using System.IO;
using QID.DataAccess;


namespace ProjectSmartCargoManager
{
    public partial class JournalEntrySummary : System.Web.UI.Page
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = null;
            if (!IsPostBack)
            {
                try
                {
                    txtfrmdt.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                    txttodt.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                    btnExport.Visible = true;
                }
                catch (Exception)
                {

                }

                try
                {
                    ds = db.SelectRecords("Sp_GetSCNAcctField");

                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                           
                            try
                            {
                                ddlChartAccID.DataSource = ds;
                                ddlChartAccID.DataMember = ds.Tables[3].TableName;
                                ddlChartAccID.DataTextField = ds.Tables[3].Columns["AccountID"].ColumnName;
                                ddlChartAccID.DataValueField = ds.Tables[3].Columns["AccountID"].ColumnName;
                                ddlChartAccID.DataBind();
                                ddlChartAccID.Items.Insert(0, "Select");
                            }
                            catch (Exception ex)
                            { }
                            
                           

                        }
                    }
                }
                catch (Exception ex)
                { }
                finally
                {
                    if (ds != null)
                    {
                        ds.Dispose();
                    }
                }
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/JournalAccountMaster.aspx");

        }

        #region Validate Controls
        private bool Validate()
        {
            try
            {
                try
                {
                    if (txtfrmdt.Text.Trim() != "" || txttodt.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtfrmdt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txttodt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtfrmdt.Focus();
                            return false;
                        }


                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtfrmdt.Focus();
                    return false;
                }


                //if (ddlLocCode.SelectedItem.Value.ToString() == "All")
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please select level Type";
                //    txtFromdate.Focus();
                //    return;
                //}


            }
            catch (Exception ex)
            {


            }
            ReportBAL objBal = new ReportBAL();
            string strResult = string.Empty;

            try
            {
                strResult = objBal.GetReportInterval(DateTime.ParseExact(txtfrmdt.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txttodt.Text.Trim(), "dd/MM/yyyy", null));
            }
            catch
            {
                strResult = "";
            }
            finally
            {
                objBal = null;
            }

            if (strResult != "")
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = strResult;

                txtfrmdt.Focus();
                return false;
            }

            return true;

        }
        #endregion

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                try
                {
                    if (txtfrmdt.Text.Trim() != "" || txttodt.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtfrmdt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txttodt.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtfrmdt.Focus();
                            return ;
                        }


                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtfrmdt.Focus();
                    return ;
                }



                #region Prepare Parameters

                string[] paramname = new string[3];
               
                paramname[0] = "frmdt";
                paramname[1] = "todt";
                paramname[2] = "ChartID";

                object[] paramvalue = new object[3];
                
                paramvalue[0] = txtfrmdt.Text.Trim();
                paramvalue[1] = txttodt.Text.Trim();
                
                
                if (ddlChartAccID.SelectedItem.Text == "Select")
                {
                    paramvalue[2] = "";
                }
                else
                {
                    paramvalue[2] = ddlChartAccID.SelectedItem.Text.Trim();
                }

                SqlDbType[] paramtype = new SqlDbType[3];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
            
                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = db.SelectRecords("Sp_GetJournalEntrySummary", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdJournalAccountSummary.PageIndex = 0;
                                grdJournalAccountSummary.DataSource = ds;
                                grdJournalAccountSummary.DataMember = ds.Tables[0].TableName;
                                grdJournalAccountSummary.DataBind();
                                grdJournalAccountSummary.Visible = true;
                                Session["grdJournalAccount"] = ds;
                                lblStatus.Text = "";
                                btnExport.Visible = true;

                            }
                            else if (ds.Tables[0].Rows.Count <= 0)
                            {

                                grdJournalAccountSummary.DataSource = null;
                                grdJournalAccountSummary.DataBind();
                                lblStatus.Text = "Record does not exist";
                                grdJournalAccountSummary.Visible = false;

                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        public void Getdata()
        {
            try
            {
                lblStatus.Text = "";


                #region Prepare Parameters

                string[] paramname = new string[3];

                paramname[0] = "frmdt";
                paramname[1] = "todt";
                paramname[2] = "ChartID";

                object[] paramvalue = new object[3];

                paramvalue[0] = txtfrmdt.Text.Trim();
                paramvalue[1] = txttodt.Text.Trim();


                if (ddlChartAccID.SelectedItem.Text == "Select")
                {
                    paramvalue[2] = "";
                }
                else
                {
                    paramvalue[2] = ddlChartAccID.SelectedItem.Text.Trim();
                }

                SqlDbType[] paramtype = new SqlDbType[3];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = db.SelectRecords("Sp_GetJournalEntrySummary", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //grdJournalAccountSummary.PageIndex = 0;
                               // grdJournalAccountSummary.DataSource = ds;
                                //grdJournalAccountSummary.DataMember = ds.Tables[0].TableName;
                                //grdJournalAccountSummary.DataBind();
                               // grdJournalAccountSummary.Visible = true;
                                Session["grdJournalAccount"] = ds;
                                lblStatus.Text = "";
                                btnExport.Visible = true;

                            }
                            else if (ds.Tables[0].Rows.Count <= 0)
                            {

                               // grdJournalAccountSummary.DataSource = null;
                                //grdJournalAccountSummary.DataBind();
                                lblStatus.Text = "Record does not exist";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
     

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            Session["grdJournalAccount"] = null;
           
            lblStatus.Text = string.Empty;

            try
            {
                //if (Validate() == false)
                //{
                //    Session["grdJournalAccount"] = null;

                //    return;
                //}
                Getdata();

                dsExp = (DataSet)Session["grdJournalAccount"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                    dt = (DataTable)dsExp.Tables[0];
                else
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    Session["grdJournalAccount"] = null;
                   
                  
                    return;
                }

                string attachment = "attachment; filename=JournalEntrySummary.xls";
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
            { }
            finally
            {
                dsExp = null;
                dt = null;
            }
        }
    }
}
