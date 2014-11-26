using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using System.Data.SqlClient;

namespace ProjectSmartCargoManager
{
    public partial class UNIDDetails : System.Web.UI.Page
    {
        UNIDBL objBAL = new UNIDBL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Save Button
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!validateData())
                //    return;

                lblStatus.Text = "";
                lblStatus.Visible = false;

                #region Prepare Parameters
                object[] UNIDInfo = new object[28];
                int irow = 0;

                //0
                UNIDInfo.SetValue(txtUnidNo.Text.Trim(), irow);
                irow++;

                //1
                UNIDInfo.SetValue(chkRadioactive.Checked, irow);
                irow++;
                
                //2
                UNIDInfo.SetValue(txtRmc.Text.Trim(), irow);
                irow++;

                //3
                UNIDInfo.SetValue(txtShippingName.Text.Trim(), irow);
                irow++;
                
                //4
                if (!string.IsNullOrEmpty(txtClassDiv.Text.ToString()) )
                    UNIDInfo.SetValue(Convert.ToDouble(txtClassDiv.Text.Trim()), irow);
                else
                    UNIDInfo.SetValue(0, irow);
                irow++;

                //5
                UNIDInfo.SetValue(txtImpCode.Text.Trim(), irow);
                irow++;

                //6
                UNIDInfo.SetValue(chkTechnical.Checked, irow);
                irow++;

                //7
                UNIDInfo.SetValue(txtSubRisk.Text.Trim(), irow);
                irow++;

                //8
                UNIDInfo.SetValue(txtHazardLabels.Text.Trim(), irow);
                irow++;

                //9
                UNIDInfo.SetValue(txtDescription.Text.Trim(), irow);
                irow++;

                //10
                UNIDInfo.SetValue(txtPg.Text.Trim(), irow);
                irow++;

                //11
                UNIDInfo.SetValue(txtSp.Text.Trim(), irow);
                irow++;

                //12
                UNIDInfo.SetValue(txtErgCode.Text.Trim(), irow);
                irow++;

                //13
                UNIDInfo.SetValue(chkForbiddenPCA.Checked, irow);
                irow++;

                //14
                UNIDInfo.SetValue(chkNoLimitLQ.Checked, irow);
                irow++;

                //15
                if (!string.IsNullOrEmpty(txtPILQ.Text.ToString()))
                    UNIDInfo.SetValue(Convert.ToDouble(txtPILQ.Text.Trim()), irow);
                else
                    UNIDInfo.SetValue(0, irow);
                irow++;

                //16
                if (!string.IsNullOrEmpty(txtMaxQtyLQ.Text.ToString()))
                    UNIDInfo.SetValue(Convert.ToDouble(txtMaxQtyLQ.Text.Trim()), irow);
                else
                    UNIDInfo.SetValue(0, irow);
                irow++;

                //17
                UNIDInfo.SetValue(chkNoLimitULQ.Checked, irow);
                irow++;

                //18
                if (!string.IsNullOrEmpty(txtPIULQ.Text.ToString()))
                    UNIDInfo.SetValue(Convert.ToDouble(txtPIULQ.Text.Trim()), irow);
                else
                    UNIDInfo.SetValue(0, irow);
                irow++;

                //19
                if (!string.IsNullOrEmpty(txtMaxQtyULQ.Text.ToString()))
                    UNIDInfo.SetValue(Convert.ToDouble(txtMaxQtyULQ.Text.Trim()), irow);
                else
                    UNIDInfo.SetValue(0, irow);
                irow++;

                //20
                UNIDInfo.SetValue(chkForbiddenCA.Checked, irow);
                irow++;

                //21
                UNIDInfo.SetValue(chkNoLimitCA.Checked, irow);
                irow++;

                //22
                if (!string.IsNullOrEmpty(txtPICA.Text.ToString()))
                    UNIDInfo.SetValue(Convert.ToDouble(txtPICA.Text.Trim()), irow);
                else
                    UNIDInfo.SetValue(0, irow);
                irow++;
               
                //23
                if (!string.IsNullOrEmpty(txtMaxQtyCA.Text.ToString()))
                    UNIDInfo.SetValue(Convert.ToDouble(txtMaxQtyCA.Text.Trim()), irow);
                else
                    UNIDInfo.SetValue(0, irow);
                irow++;

                //24
                UNIDInfo.SetValue(Session["UserName"].ToString(), irow);
                irow++;

                //25
                UNIDInfo.SetValue("I", irow);
                irow++;

                //26
                UNIDInfo.SetValue(ChkActive.Checked, irow);
                irow++;

                //27 Result Parameter send blank
                UNIDInfo.SetValue("", irow);
                irow++;

                #endregion Prepare Parameters

                string res = "";
                res = objBAL.AddModifyUNIDDetails(UNIDInfo);


                if (res != "error")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                    string value = "SAVE";
                    MasterLog(value);
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;

                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Saving UNID details failed";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Saving UNID details failed";
                lblStatus.ForeColor = Color.Red;
            }

        }

        #endregion

        public void MasterLog(string value)
        {
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            #region for Master Audit Log
            #region Prepare Parameters
            object[] Paramsmaster = new object[7];
            int count = 0;

            //1

            Paramsmaster.SetValue("UNID Details", count);
            count++;

            //2
            Paramsmaster.SetValue(txtUnidNo.Text, count);
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

        #region Validate Data

        //protected bool validateData()
        //{
        //    try
        //    {
        //        if (Convert.ToInt32(txtUnidNo.Text.Trim()) > 0)
        //        {

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Visible = true;
        //        lblStatus.Text = "Please insert AlphaNumberic UNID No";
        //        lblStatus.ForeColor = Color.Red;
        //        return false;
        //    }
        //    try
        //    {
        //        if (Convert.ToDouble(txtClassDiv.Text.Trim()) > 0)
        //        {
                    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Visible = true;
        //        lblStatus.Text = "Class/Division number must be numeric";
        //        lblStatus.ForeColor = Color.Red;
        //        return false;
        //    }
        //    try
        //    {
        //        if (Convert.ToDouble(txtPILQ.Text.Trim()) > 0)
        //        {
                    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Visible = true;
        //        lblStatus.Text = "PI number must be numeric";
        //        lblStatus.ForeColor = Color.Red;
        //        return false;
        //    }
        //    try
        //    {
        //        if (Convert.ToDouble(txtMaxQtyLQ.Text.Trim()) > 0)
        //        {
                    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Visible = true;
        //        lblStatus.Text = "Max net quantity per kg must be numeric";
        //        lblStatus.ForeColor = Color.Red;
        //        return false;
        //    }
        //    try
        //    {
        //        if (Convert.ToDouble(txtPIULQ.Text.Trim()) > 0)
        //        {
                    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Visible = true;
        //        lblStatus.Text = "PI number must be numeric";
        //        lblStatus.ForeColor = Color.Red;
        //        return false;
        //    }
        //    try
        //    {
        //        if (Convert.ToDouble(txtMaxQtyULQ.Text.Trim()) > 0)
        //        {
                    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Visible = true;
        //        lblStatus.Text = "Max net quantity per kg must be numeric";
        //        lblStatus.ForeColor = Color.Red;
        //        return false;
        //    }
        //    try
        //    {
        //        if (Convert.ToDouble(txtPICA.Text.Trim()) > 0)
        //        {
                    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Visible = true;
        //        lblStatus.Text = "PI number must be numeric";
        //        lblStatus.ForeColor = Color.Red;
        //        return false;
        //    }
        //    try
        //    {
        //        if (Convert.ToDouble(txtMaxQtyCA.Text.Trim()) > 0)
        //        {
                    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Visible = true;
        //        lblStatus.Text = "Max net quantity per kg must be numeric";
        //        lblStatus.ForeColor = Color.Red;
        //        return false;
        //    }

        //    return true;
        //}

        #endregion

        #region List Button
        protected void btnList_Click(object sender, EventArgs e)
        {
            getUNIDDetails();
        }

        #endregion

        protected void getUNIDDetails()
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";
                string UnidNo;
                UnidNo = "";

                if (txtUnidNo.Text.Trim() != "")
                {
                    UnidNo = txtUnidNo.Text.Trim();
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please enter UNID number";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                    
                }


                DataSet DSUnidData = objBAL.GetUNIDDetails(UnidNo);

                if (DSUnidData != null && DSUnidData.Tables.Count > 0 && DSUnidData.Tables[0].Rows.Count > 0)
                {
                    Session["DSUnidData"] = DSUnidData;
                    lblStatus.Visible = false;
                    fillUnidDetails();
                }
                else
                {
                    lblStatus.Focus();
                    lblStatus.Visible = true;
                    lblStatus.Text = "UNID details does not exist";
                    lblStatus.ForeColor = Color.Blue;
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void fillUnidDetails()
        {
            DataSet dsUNID = (DataSet)Session["DSUnidData"];
            txtUnidNo.Text = dsUNID.Tables[0].Rows[0]["UNIDNo"].ToString();
            if (dsUNID.Tables[0].Rows[0]["RadioActive"].ToString() == "True")
            {
                chkRadioactive.Checked = true;
            }
            else
            {
                chkRadioactive.Checked = false;
            }
            txtRmc.Text = dsUNID.Tables[0].Rows[0]["RMC"].ToString();
            txtShippingName.Text = dsUNID.Tables[0].Rows[0]["ShippingName"].ToString();
            txtClassDiv.Text = dsUNID.Tables[0].Rows[0]["ClassDiv"].ToString();
            txtImpCode.Text = dsUNID.Tables[0].Rows[0]["IMPCode"].ToString();
            if (dsUNID.Tables[0].Rows[0]["Technical"].ToString() == "True")
            {
                chkTechnical.Checked = true;
            }
            else
            {
                chkTechnical.Checked = false;
            }
            txtSubRisk.Text = dsUNID.Tables[0].Rows[0]["SubRisk"].ToString();
            txtHazardLabels.Text = dsUNID.Tables[0].Rows[0]["HazardLabels"].ToString();
            txtDescription.Text = dsUNID.Tables[0].Rows[0]["Description"].ToString();
            txtPg.Text = dsUNID.Tables[0].Rows[0]["PG"].ToString();
            txtSp.Text = dsUNID.Tables[0].Rows[0]["SP"].ToString();
            txtErgCode.Text = dsUNID.Tables[0].Rows[0]["ERGCode"].ToString();
            if (dsUNID.Tables[0].Rows[0]["ForbiddenPCA"].ToString() == "True")
            {
                chkForbiddenPCA.Checked = true;
            }
            else
            {
                chkForbiddenPCA.Checked = false;
            }
            if (dsUNID.Tables[0].Rows[0]["NoLimitLQ"].ToString() == "True")
            {
                chkNoLimitLQ.Checked = true;
            }
            else
            {
                chkNoLimitLQ.Checked = false;
            }
            txtPILQ.Text = dsUNID.Tables[0].Rows[0]["PILQ"].ToString();
            txtMaxQtyLQ.Text = dsUNID.Tables[0].Rows[0]["MaxQtyLQ"].ToString();
            if (dsUNID.Tables[0].Rows[0]["NoLimitULQ"].ToString() == "True")
            {
                chkNoLimitULQ.Checked = true;
            }
            else
            {
                chkNoLimitULQ.Checked = false;
            }
            txtPIULQ.Text = dsUNID.Tables[0].Rows[0]["PIULQ"].ToString();
            txtMaxQtyULQ.Text = dsUNID.Tables[0].Rows[0]["MaxQtyULQ"].ToString();
            if (dsUNID.Tables[0].Rows[0]["ForbiddenCA"].ToString() == "True")
            {
                chkForbiddenCA.Checked = true;
            }
            else
            {
                chkForbiddenCA.Checked = false;
            }
            if (dsUNID.Tables[0].Rows[0]["NoLimitCA"].ToString() == "True")
            {
                chkNoLimitCA.Checked = true;
            }
            else
            {
                chkNoLimitCA.Checked = false;
            }

            txtPICA.Text = dsUNID.Tables[0].Rows[0]["PICA"].ToString();
            txtMaxQtyCA.Text = dsUNID.Tables[0].Rows[0]["MaxQtyCA"].ToString();

            if (dsUNID.Tables[0].Rows[0]["IsActive"].ToString() == "True")
            {
                ChkActive.Checked = true;
            }
            else
            {
                ChkActive.Checked = false;
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetUNIDCodeWithDesc(string prefixText, int count)
        {
            try
            {
                string[] orgdest = new ConBooking_GHA().GetOrgDest();

                string con = Global.GetConnectionString();
                //SqlConnection con = new SqlConnection("connection string"); 
                SqlDataAdapter dad = new SqlDataAdapter("SELECT CONVERT(VARCHAR,UNIDNo)+'-'+SUBSTRING(ShippingName,0,20) AS UNID  FROM dbo.UNIDDetails where UNIDNo like '%" + prefixText + "%'", con);
                DataSet ds = new DataSet();
                dad.Fill(ds);
               
                List<string> list = new List<string>(ds.Tables[0].Rows.Count);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(dr[0].ToString());

                }

                if (ds != null)
                    ds.Dispose();
                //bal = null;
                return list.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #region Clear Button
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UNIDDetails.aspx");
        }

        #endregion

        protected void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";
                string UnidNo;
                UnidNo = "";

                if (txtUnidNo.Text.Trim() != "")
                {
                    UnidNo = txtUnidNo.Text.Trim();
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please enter UNID number";
                    lblStatus.ForeColor = Color.Blue;
                    return;

                }

                DataSet DelUnidData = objBAL.DelUNIDDetails(UnidNo);

                if (DelUnidData != null && DelUnidData.Tables.Count > 0 && DelUnidData.Tables[0].Rows.Count > 0)
                {
                    if (DelUnidData.Tables[0].Rows[0][0].ToString() == "D")
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "UNID details deleted successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "No record found";
                        lblStatus.ForeColor = Color.Red;
                    }

                }
                
                else
                {
                    lblStatus.Focus();
                    lblStatus.Visible = true;
                    lblStatus.Text = "UNID details does not exist";
                    lblStatus.ForeColor = Color.Blue;
                }

            }
            catch (Exception ex)
            {

            }
 
        }
    }
}
