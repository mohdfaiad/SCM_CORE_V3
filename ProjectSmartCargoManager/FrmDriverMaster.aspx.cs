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
    public partial class FrmDriverMaster : System.Web.UI.Page
    {
        #region Variables
        BALDriverMaster objBal = new BALDriverMaster();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region List Button
        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            Session["DriverList"] = null;

            try
            {
                DataSet dsList = new DataSet();

                string status = DrpStatus.SelectedValue.ToString();

                bool sts = false;

                if (status == "Active")
                {
                    sts = true;
                }
                else if (status == "InActive")
                {
                    sts = false;
                }


                dsList = objBal.GetDriverList(TxtName.Text.ToString(), TxtLicenceNo.Text.ToString(), TxtVehicle.Text.ToString(), TxtPhone.Text.ToString(), sts);

                if (dsList != null && dsList.Tables.Count > 0 && dsList.Tables[0].Rows.Count > 0)
                {
                    Session["DriverList"] = dsList;
                    GrdDriverDetails.DataSource = dsList.Tables[0];
                    GrdDriverDetails.DataBind();
                }
                else if (dsList.Tables[0].Rows.Count == 0)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Record Found";
                    lblStatus.ForeColor = Color.Red;
                    GrdDriverDetails.DataSource = dsList.Tables[0];
                    GrdDriverDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion

        #region Button Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/FrmDriverMaster.aspx");
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Create Empty Row
        public void CreateEmptyRow()
        {
            BtnAdd_Click(null, null);
        }

        #endregion

        #region Add New Row to Grid
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            DataSet DsAddNewRow = null;
            try
            {
                if (Session["DriverList"] != null)
                {
                    DsAddNewRow = ((DataSet)Session["DriverList"]).Copy();
                    DataRow row = DsAddNewRow.Tables[0].NewRow();

                    row["DriverName"] = "";
                    row["DLNumber"] = "";
                    row["VehicleNo"] = "";
                    row["Phone"] = "";
                    row["IsActive"] = false;

                    DsAddNewRow.Tables[0].Rows.Add(row);

                    Session["DriverList"] = DsAddNewRow.Copy();
                    GrdDriverDetails.DataSource = DsAddNewRow.Copy();
                    GrdDriverDetails.DataBind();
                    DsAddNewRow.Dispose();

                }
                else
                {
                    DataTable dt = new DataTable();

                    dt.Columns.Add("DriverName");
                    dt.Columns.Add("DLNumber");
                    dt.Columns.Add("VehicleNo");
                    dt.Columns.Add("Phone");
                    dt.Columns.Add("IsActive");

                    DataRow dr = dt.NewRow();

                    dr["DriverName"] = "";
                    dr["DLNumber"] = "";
                    dr["VehicleNo"] = "";
                    dr["Phone"] = "";
                    dr["IsActive"] = false;

                    dt.Rows.Add(dr);
                    dt.AcceptChanges();

                    GrdDriverDetails.DataSource = dt;
                    GrdDriverDetails.DataBind();
                    dt.Dispose();
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion


        #region Button Save
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                int cnt = 0;

                for (int i = 0; i < GrdDriverDetails.Rows.Count; i++)
                {
                    if (((CheckBox)GrdDriverDetails.Rows[i].FindControl("chkSelect")).Checked == true)
                    {
                        cnt = cnt + 1;

                        string DriverName = ((TextBox)GrdDriverDetails.Rows[i].FindControl("TxtDriver")).Text;
                        string LicenceNo = ((TextBox)GrdDriverDetails.Rows[i].FindControl("TxtLicenceNo")).Text;
                        string VehicleNo = ((TextBox)GrdDriverDetails.Rows[i].FindControl("TxtVehicleNo")).Text;
                        string Phone = ((TextBox)GrdDriverDetails.Rows[i].FindControl("TxtPhone")).Text;
                        bool isActive = ((CheckBox)GrdDriverDetails.Rows[i].FindControl("ChkStatus")).Checked;

                        DataSet dsSave = new DataSet();

                        dsSave = objBal.SetDriverDetails(DriverName, LicenceNo, VehicleNo, Phone, Session["UserName"].ToString(), isActive);

                        if (dsSave != null && dsSave.Tables.Count > 0 && dsSave.Tables[0].Rows.Count > 0)
                        {
                            GrdDriverDetails.DataSource = dsSave.Tables[0];
                            GrdDriverDetails.DataBind();
                            lblStatus.Visible = true;
                            lblStatus.Text = "Record added Successfully";
                            lblStatus.ForeColor = Color.Green;
                        }

                        else
                        {
                            lblStatus.Visible = true;
                            lblStatus.Text = "Record Not Saved, Please try again";
                            lblStatus.ForeColor = Color.Red;

                        }

                    }
                }

                if (cnt > 1 || cnt == 0)
                {
                    lblStatus.Text = "Please select only one record.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion


        #region GrdDriverDetails_PageIndexChanging
        protected void GrdDriverDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet ds = (DataSet)Session["DriverList"];
                GrdDriverDetails.PageIndex = e.NewPageIndex;
                GrdDriverDetails.DataSource = ds.Tables[0];
                GrdDriverDetails.DataBind();
            }
            catch (Exception ex)
            {
               
            }
        }

        #endregion
    }
}
