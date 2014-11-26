using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class FrmAWBTrackFromLogin : System.Web.UI.Page
    {
        BALAWBTracLDetails BATD = new BALAWBTracLDetails();
         static string FinalAwbList = "";
         protected void Page_Load(object sender, EventArgs e)
         {
             LabelStatus.Text = "";
             if (!IsPostBack)
             {
                 string trackAWB = "";
                 if (Request.QueryString != null && Request.QueryString["AWBNo"] != null
                        && Request.QueryString["AWBNo"] != "" && Request.QueryString["AWBPrefix"] != null
                        && Request.QueryString["AWBPrefix"] != "")
                 {   //Extract AWB Numbers from
                     trackAWB = Request.QueryString["AWBPrefix"] + "|" + Request.QueryString["AWBNo"];
                 }
                 else
                 {
                     if (Session["TrackAWB"] != null)
                     {
                         trackAWB = Session["TrackAWB"].ToString();
                     }
                 }
                 if (trackAWB != null && trackAWB != "")
                 {
                     string[] spltPrefix = trackAWB.Split('|');
                     txtPrefix.Text = spltPrefix[0].Trim();
                     string[] AWBNos = spltPrefix[1].Trim().Split(',');
                     string prefix = spltPrefix[0].ToString();
                     FinalAwbList = "";
                     for (int i = 0; i < AWBNos.Length; i++)
                     {
                         FinalAwbList += AWBNos[i].ToString();
                         if (i < AWBNos.Length - 1)
                         {
                             FinalAwbList += ",";
                         }
                     }
                     TextBoxAWBno.Text = FinalAwbList;
                     ButtonGO_Click(null, null);

                 }
             }
         }

        protected void ButtonGO_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPrefix.Text.Trim() != "" && TextBoxAWBno.Text.Trim() != "")
                {
                    string message = "";
                    DataSet dsget = BATD.dsgetAWBRecord(txtPrefix.Text.Trim(), TextBoxAWBno.Text.Trim(), ref message);
                    GridViewAwbTracking.DataSource = dsget;
                    GridViewAwbTracking.DataBind();
                    LabelStatus.Text = message;
                }
                else
                {
                    LabelStatus.Text = "Prefix and AWBNo both are required";
                }
            }
            catch (Exception ex)
            {
                LabelStatus.Text = "Error During fetching Record";
            }
        }
    }
}
