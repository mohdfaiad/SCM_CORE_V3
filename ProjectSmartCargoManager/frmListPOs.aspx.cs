using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using clsDataLib;
using BAL;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class frmListPOs : System.Web.UI.Page
    {

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            txtFrmDt.Attributes.Add("readonly", "readonly");
            txtToDt.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                lblEmpty.Visible = false;
                lblError.Visible = false;
                txtFrmDt.Text = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                FilldrpWWR();
            }
        }
        #endregion Page_Load

        #region ButtonSearchClick Operations
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dtNewFrmDt, dtNewToDt;
                if ((DateTime.TryParseExact(txtFrmDt.Text,"dd/MM/yyyy",null, System.Globalization.DateTimeStyles.None,
                    out dtNewFrmDt)) && (DateTime.TryParseExact(txtToDt.Text, "dd/MM/yyyy", null, 
                    System.Globalization.DateTimeStyles.None, out dtNewToDt)))
                {
                    lblError.Visible = false;
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Please put valid date time entry..";
                    return;
                }
                string[] pname = new string[3]
                {   
                    "frmDate",
                    "toDate",
                    "WWRegionCode"
                };
                object[] pvalue = new object[3]
                {
                    dtNewFrmDt,
                    dtNewToDt,
                    drpWWR.SelectedItem.Text
                };
                SqlDbType[] ptype = new SqlDbType[3]
                {
                    SqlDbType.DateTime,
                    SqlDbType.DateTime,
                    SqlDbType.NVarChar
                };

                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet ds = null;
                ds = db.SelectRecords("spListPOs", pname, pvalue, ptype);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        lblEmpty.Visible = true;
                        lblEmpty.Text = "No records found.";
                        gvResult.DataSource = ds.Tables[0];
                        gvResult.DataBind();
                    }
                    else if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblEmpty.Visible = false;
                        gvResult.DataSource = ds.Tables[0];
                        gvResult.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }            
        }
        #endregion

        #region Fill Combobox
        private void FilldrpWWR()
        {
            try
            {
                string[] pname = new string[2];
                pname[0] = "tblName";
                pname[1] = "defaultValue";


                object[] pvalue = new object[2];
                pvalue[0] = "tblWWRegionMaster";
                pvalue[1] = "ALL";


                SqlDbType[] ptype = new SqlDbType[3];
                ptype[0] = SqlDbType.VarChar;
                ptype[1] = SqlDbType.VarChar;

                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet dsdrpWWR = db.SelectRecords("spFillComboBoxMasters", pname, pvalue, ptype);
                if (dsdrpWWR != null && dsdrpWWR.Tables.Count > 0)
                {
                    if (dsdrpWWR.Tables[0].Rows.Count > 0)
                    {
                        drpWWR.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        drpWWR.DataSource = dsdrpWWR.Tables[0];
                        drpWWR.DataTextField = "Code";
                        drpWWR.DataValueField = "ID";
                        drpWWR.DataBind();
                        drpWWR.SelectedIndex = 0;
                    }
                    else
                    {
                        drpWWR.Items.Clear();
                        drpWWR.SelectedIndex = 0;
                    }
                }
                else
                {
                    drpWWR.Items.Clear();
                    drpWWR.Items.Add("ALL");
                    drpWWR.SelectedIndex = 0;
                }
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        #endregion

        protected void gvResult_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                string POno= ((Label)gvResult.Rows[e.NewEditIndex].FindControl("lblPONo")).Text;
                Response.Redirect("frmPurchaseOrder.aspx?type=Search&PONo="+POno, false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;   
            }
        }

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/frmListPOs.aspx");
        }
        #endregion btnClear_Click

    }
}
