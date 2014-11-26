using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class FrmAuditTrail : System.Web.UI.Page
    {
        

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        #endregion Load
      
        #region List
        protected void btnList_Click(object sender, EventArgs e)
        {
            BALAuditTrail objAuditTrail = new BALAuditTrail();
            DataSet ds=null;
            try
            {
                string AWBNumber, AWBPrefix;


                if (txtAWBNumber.Text != "")
                {
                    AWBNumber = txtAWBNumber.Text;
                }
                else
                {
                    lblStatus.Text = "Please Enter AWBNumber.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtprefix.Text != "")
                {
                    AWBPrefix = txtprefix.Text;
                }
                else
                {
                    lblStatus.Text = "Please Enter AWBNumber.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                ds = objAuditTrail.GetAuditTrail(AWBNumber, AWBPrefix);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GrdAWBSummary.DataSource = ds.Tables[0];
                    GrdAWBSummary.DataBind();
                }
                else
                {
                    GrdAWBSummary.DataSource = null;
                    GrdAWBSummary.DataBind();
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    GrdRouteDetails.DataSource = ds.Tables[1];
                    GrdRouteDetails.DataBind();
                }
                else
                {
                    GrdRouteDetails.DataSource = null;
                    GrdRouteDetails.DataBind();

                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    GrdRateMaster.DataSource = ds.Tables[2];
                    GrdRateMaster.DataBind();
                }
                else
                {
                    GrdRateMaster.DataSource = null;
                    GrdRateMaster.DataBind();
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    GrdManifestSummary.DataSource = ds.Tables[3];
                    GrdManifestSummary.DataBind();
                }
                else
                {
                    GrdManifestSummary.DataSource = null;
                    GrdManifestSummary.DataBind();
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    GrdArrival.DataSource = ds.Tables[4];
                    GrdArrival.DataBind();
                }
                else
                {
                    GrdArrival.DataSource = null;
                    GrdArrival.DataBind();
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    GrdDelivery.DataSource = ds.Tables[5];
                    GrdDelivery.DataBind();
                }
                else
                {
                    GrdDelivery.DataSource = null;
                    GrdDelivery.DataBind();
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    GrdBillingDetails.DataSource = ds.Tables[6];
                    GrdBillingDetails.DataBind();

                }
                else
                {
                    GrdBillingDetails.DataSource = null;
                    GrdBillingDetails.DataBind();
                }
                try
                {
                    if (ds.Tables[7].Rows.Count > 0)
                    {
                        GrdULDAWB.DataSource = ds.Tables[7];
                        GrdULDAWB.DataBind();

                    }
                    else
                    {
                        GrdULDAWB.DataSource = null;
                        GrdULDAWB.DataBind();

                    }

                    if (ds.Tables[8].Rows.Count > 0)
                    {
                        GrdBreakULD.DataSource = ds.Tables[8];
                        GrdBreakULD.DataBind();

                    }
                    else
                    {
                        GrdBreakULD.DataSource = null;
                        GrdBreakULD.DataBind();
                    }
                }
                catch (Exception ex) { }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                objAuditTrail = null;
            }
        }
        #endregion List

        protected void BtnPrint_Click(object sender, EventArgs e)
        { }

        #region ListManifest
        protected void BtnListManifest_Click(object sender, EventArgs e)
        {
            BALAuditTrail objAuditTrail = new BALAuditTrail();
            DataSet ds = null;
            try
            {
                string DtTime;
                string FlightNumber;
                
                lblStatus.Text = "";
                if (txtFlightNumber.Text == "" || txtFlightDate.Text == "")
                {
                    lblStatus.Text = "Please Provide Flight Number and Flight Date to Search";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                DtTime = txtFlightDate.Text;
                FlightNumber = txtFlightNumber.Text;

                ds = objAuditTrail.GetAuditTrailManifest(DtTime, FlightNumber);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GrdManifstSummaryDetails.DataSource = ds.Tables[0];
                    GrdManifstSummaryDetails.DataBind();
                }
                else
                {
                    GrdManifstSummaryDetails.DataSource = null;
                    GrdManifstSummaryDetails.DataBind();
                }

            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                objAuditTrail = null;
            }


        }
        #endregion ListManifest

        #region Clear Button
        protected void btnclear_Click(object sender, EventArgs e)
        {
            try 
            {
                txtprefix.Text = "";
                txtAWBNumber.Text = "";
                GrdAWBSummary.DataSource = null;
                GrdAWBSummary.DataBind();
                GrdArrival.DataSource = null;
                GrdArrival.DataBind();
                GrdBillingDetails.DataSource = null;
                GrdBillingDetails.DataBind();
                GrdBreakULD.DataSource = null;
                GrdBreakULD.DataBind();
                GrdDelivery.DataSource = null;
                GrdDelivery.DataBind();
                GrdManifestSummary.DataSource = null;
                GrdManifestSummary.DataBind();
                GrdManifstSummaryDetails.DataSource = null;
                GrdManifstSummaryDetails.DataBind();
                GrdRateMaster.DataSource = null;
                GrdRateMaster.DataBind();
                GrdRouteDetails.DataSource = null;
                GrdRouteDetails.DataBind();
                GrdULDAWB.DataSource = null;
                GrdULDAWB.DataBind();

            }
            catch (Exception ex) { }
        }
        #endregion

    }
}
