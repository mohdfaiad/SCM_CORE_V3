using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using BAL;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;

namespace ProjectSmartCargoManager
{
    public partial class FrmULDAuditTrail : System.Web.UI.Page
    {
        #region variables

        BALULDMaster uld = new BALULDMaster();
        clsFillCombo cfc = new clsFillCombo();

        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    if (Session["UserName"] != null)
                    {
                        txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
               }
          }
            catch (Exception ex)
            {
                
            }

        }
        #endregion Page Load

        #region Button Search
        public void btnSearch_Click(object sender, EventArgs e)
        {
          try
            {
                try
                {
                    grdFlightHistory.DataSource = null;
                    grdFlightHistory.DataBind();
                    grdMovementHistory.DataSource = null;
                    grdMovementHistory.DataBind();
                    grdULDMovement.DataSource = null;
                    grdULDMovement.DataBind();
                    lblFlightHistory.Visible = false;
                    lblMovementHistory.Visible = false;
                    lblUCRHistory.Visible = false;

                    lblStatus.Text = "";
                    if (txtFromDate.Text == "" || txtToDate.Text == "")
                    {
                        //lblStatus.ForeColor = Color.Red;
                        //lblStatus.Text = "Please enter From date and To date for datewise Schedule List";
                        //txtFlightFromdate.Focus();
                        //return;
                    }
                    if (txtFromDate.Text.Trim() != "" || txtToDate.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            //txtFromdate.Focus();
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

                try
                {
                    string fromdate = "", ToDate = "";
                    DateTime dt1 = new DateTime();
                    DateTime dt2 = new DateTime();


                    try
                    {


                        if (txtFromDate.Text != "")
                        {
                            dt1 = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);

                            fromdate = dt1.ToString("MM/dd/yyyy");
                        }
                        if (txtToDate.Text != "")
                        {
                            dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
                            ToDate = dt2.ToString("MM/dd/yyyy");


                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    string ULDNo = txtULDNo.Text;
                    //DateTime toDate = new DateTime();
                    DataSet ds = new DataSet();
                    ds = uld.GetAuditDetails(ULDNo, fromdate, ToDate);

                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        grdULDMovement.DataSource = ds.Tables[0];
                                        grdULDMovement.DataBind();
                                        lblUCRHistory.Visible = true;
                                    }
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        grdFlightHistory.DataSource = ds.Tables[1];
                                        grdFlightHistory.DataBind();
                                        lblFlightHistory.Visible = true;
                                    }
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        grdMovementHistory.DataSource = ds.Tables[2];
                                        grdMovementHistory.DataBind();
                                        lblMovementHistory.Visible = true;
                                    }

                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "No Records Found";
                                }

                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "No Records Found";
                            }
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "No Records Found";
                    }
                }


                catch (Exception ex)
                {
                }
            }
          catch (Exception ex)
          { }

        }



        #endregion Button Search
    }
}
