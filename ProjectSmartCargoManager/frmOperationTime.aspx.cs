using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCM.Common.Struct;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class frmOperationTime : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //Check if data is available for updating Time Stamp.
                    txtOpsDate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                    txtOpsTimeHr.Text = Convert.ToDateTime(Session["IT"]).ToString("HH");
                    txtOpsTimeMin.Text = Convert.ToDateTime(Session["IT"]).ToString("mm");

                    lblPnlError.Text = "Please select Actual Operation time (if different than current local time)";
                    lblPnlError.ForeColor = System.Drawing.Color.Blue;

                    //Check if only 1 row is available in session to show last Updated On time stamp.
                    if (Session["listOperationTime"] != null)
                    {
                        List<clsOperationTimeStamp> objListOpsTime = (List<clsOperationTimeStamp>)Session["listOperationTime"];
                        if (objListOpsTime != null)
                        {   //If only 1 row is available in the session then fetch Last Update On timestamp.
                            if (objListOpsTime.Count == 1)
                            {
                                //Call function to get previous time stamp.
                                BALCommon objCommon = new BALCommon();
                                DateTime dtLast = objCommon.GetLastOperationalTimeStamp(objListOpsTime);
                                if (dtLast != null)
                                {   //If valid previous udpate time received.
                                    if (dtLast != Convert.ToDateTime("01-JAN-1900"))
                                    {   //Show date time on screen.
                                        txtOpsDate.Text = dtLast.ToString("dd/MM/yyyy");
                                        txtOpsTimeHr.Text = dtLast.ToString("HH");
                                        txtOpsTimeMin.Text = dtLast.ToString("mm");
                                        
                                        lblPnlError.Text = "Last Operation time is as shown below:";
                                        lblPnlError.ForeColor = System.Drawing.Color.Blue;

                                    }
                                }
                                 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnOpsSave_Click(object sender, EventArgs e)
        {
            lblPnlError.Text = "";
            try
            {   
                //Validate Date
                DateTime dt = DateTime.Now;
                if (!DateTime.TryParseExact(txtOpsDate.Text + " " + txtOpsTimeHr.Text.PadLeft(2, '0') + ":" + 
                    txtOpsTimeMin.Text.PadLeft(2, '0') + ":00",
                    "dd/MM/yyyy HH:mm:ss",null, System.Globalization.DateTimeStyles.None, out dt))
                {
                    lblPnlError.Text = "Please enter valid Operation Date & Time.";
                    lblPnlError.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                if (dt > Convert.ToDateTime(Session["IT"].ToString()))
	            {
                    lblPnlError.Text = "Please enter date and time which is not a future date.";
                    lblPnlError.ForeColor = System.Drawing.Color.Red;
                    return;
	            }
                //Validate if Time is out of configured allowed variation time.
                string roles = "";
                roles = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "OpsTimeOverrideAllowedFor");
                if (!roles.Contains(Session["RoleName"].ToString()))
                {
                    int AllowedTimeDiff = 240;
                    roles = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "MaxOpsTimeDiffInMins");
                    if (int.TryParse(roles,out AllowedTimeDiff))
                    {   // Find out allowed time difference.
                        if(DateTime.Compare(Convert.ToDateTime(Session["IT"]).AddMinutes(-1 * AllowedTimeDiff),dt) > 0)
                        {
                            lblPnlError.Text = "You cannot select Time older than current time by " + AllowedTimeDiff.ToString() + " minutes";
                            return;
                        }
                    }
                }

                //Check if data is available for updating time stamp.
                if (Session["listOperationTime"] != null)
                {
                    List<clsOperationTimeStamp> objListOpsTime = (List<clsOperationTimeStamp>)Session["listOperationTime"];
                    if (objListOpsTime != null)
                    {
                        if (objListOpsTime.Count > 0)
                        {
                            ((clsOperationTimeStamp)objListOpsTime[0]).OperationDate = txtOpsDate.Text;
                            ((clsOperationTimeStamp)objListOpsTime[0]).OperationTime = txtOpsTimeHr.Text.PadLeft(2, '0')
                                + ":" + txtOpsTimeMin.Text.PadLeft(2, '0');
                            ((clsOperationTimeStamp)objListOpsTime[0]).UpdatedBy = Session["UserName"].ToString();
                            ((clsOperationTimeStamp)objListOpsTime[0]).UpdatedOn = Convert.ToDateTime(Session["IT"].ToString());
                            //Call function to save time stamp.
                            BALCommon objCommon = new BALCommon();
                            objCommon.SaveOperationalTimeStamp(objListOpsTime);
                            lblPnlError.Text = "Actual operation time saved successfully !";
                            lblPnlError.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "closeTimePopup", "<SCRIPT LANGUAGE='javascript'>CloseWindow();</script>", false);
            }
            catch (Exception ex)
            {
                lblPnlError.Text = "Error: " + ex.Message;
                lblPnlError.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                Session["listOperationTime"] = null;
            }
        }

        protected void btnOpsCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Session["listOperationTime"] = null;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "closetimePopup", "<SCRIPT LANGUAGE='javascript'>CloseWindow();</script>", false);
            }
            catch (Exception)
            {
            }
        }

    }
}
