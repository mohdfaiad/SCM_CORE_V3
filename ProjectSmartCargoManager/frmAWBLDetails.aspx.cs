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
    public partial class frmAWBLDetails : System.Web.UI.Page
    {
        BALAWBTracLDetails BATD = new BALAWBTracLDetails();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            LabelStatus.Text = "";
            if (!IsPostBack)
            {
                if (Session["awbPrefix"] != null)
	            {
                    txtPrefix.Text = Session["awbPrefix"].ToString();
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
                    DataSet dsget = BATD.dsgetAWBRecord(txtPrefix.Text.Trim(), TextBoxAWBno.Text.Trim(),ref message);
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
