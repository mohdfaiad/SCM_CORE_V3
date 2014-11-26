using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using clsDataLib;
using System.Drawing;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class frmDemurrage : System.Web.UI.Page
    {
        clsFillCombo cfc = new clsFillCombo();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblStatus.ForeColor = System.Drawing.Color.Red;
            lblStatus.Text = "";
            if (!IsPostBack)
            {
                try
                {
                    cfc.FillAllComboBoxes("tblDemStatusMaster", "Select", ddlStatus);
                    cfc.FillAllComboBoxes("AgentMaster", "Select", ddlAgentCode);
                    gvUCR.DataSource = null;
                    gvUCR.DataBind();
                }
                catch (Exception)
                {
                }
            }
        }

        protected void btnUpdtDem_Click(object sender, EventArgs e)
        {
            try
            {
                int ischk = 0;
                for (int i = 0; i < gvUCR.Rows.Count; i++)
                {
                    if (((CheckBox)(gvUCR.Rows[i].FindControl("lblchk"))).Checked == true)
                    {
                        ischk = 1;
                        break;
                    }
                }
                if (ischk == 0)
                {
                    lblStatus.Text = "Please select a row";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                for (int i = 0; i < gvUCR.Rows.Count; i++)
                {
                    if (((CheckBox)(gvUCR.Rows[i].FindControl("lblchk"))).Checked == true)
                    {
                        string[] paramname = new string[22];
                        paramname[0] = "SrNo";
                        paramname[1] = "UCRNo";
                        paramname[2] = "ULDNO";
                        paramname[3] = "Qty";
                        paramname[4] = "AgentCode";
                        paramname[5] = "NoofDays";
                        paramname[6] = "WareHouse";
                        paramname[7] = "SubWareHouse";
                        paramname[8] = "Source";
                        paramname[9] = "Status";
                        paramname[10] = "StartDate";
                        paramname[11] = "EndDate";
                        paramname[12] = "ULDStatus";
                        paramname[13] = "Charge";
                        paramname[14] = "Currency";
                        paramname[15] = "Comment";
                        paramname[16] = "Processor";
                        paramname[17] = "isInternal";
                        paramname[18] = "isCustomer";
                        paramname[19] = "Rates";
                        paramname[20] = "LocationType";
                        paramname[21] = "LocationID";

                        object[] paramvalue = new object[22];
                        paramvalue[0] = ((Label)(gvUCR.Rows[i].FindControl("lblSrNo"))).Text.ToString();
                        paramvalue[1] = ((Label)(gvUCR.Rows[i].FindControl("lblUCR"))).Text.ToString();
                        paramvalue[2] = ((Label)(gvUCR.Rows[i].FindControl("lblULDNo"))).Text.ToString();
                        paramvalue[3] =Convert.ToInt32( ((Label)(gvUCR.Rows[i].FindControl("lblQty"))).Text.ToString());
                        paramvalue[4] = ((Label)(gvUCR.Rows[i].FindControl("lblAgentCode"))).Text.ToString();
                        paramvalue[5] =Convert.ToInt32( ((Label)(gvUCR.Rows[i].FindControl("lblNoDays"))).Text.ToString());
                        paramvalue[6] = ((Label)(gvUCR.Rows[i].FindControl("lblWH"))).Text.ToString();
                        paramvalue[7] = ((Label)(gvUCR.Rows[i].FindControl("lblSubLoc"))).Text.ToString();
                        paramvalue[8] = ((Label)(gvUCR.Rows[i].FindControl("lblSource"))).Text.ToString();
                        paramvalue[9] = ((Label)(gvUCR.Rows[i].FindControl("lblStatus"))).Text.ToString();
                        paramvalue[10] =Convert.ToDateTime( ((Label)(gvUCR.Rows[i].FindControl("lblStrtDt"))).Text.ToString());
                        if (((Label)(gvUCR.Rows[i].FindControl("lblEndDate"))).Text.ToString().Trim() == "")
                        {
                            paramvalue[11] = Convert.ToDateTime("01/01/1800");
                        }
                        else
                        {
                            paramvalue[11] = Convert.ToDateTime(((Label)(gvUCR.Rows[i].FindControl("lblEndDate"))).Text.ToString());
                        }
                        paramvalue[12] = ((Label)(gvUCR.Rows[i].FindControl("lblULDStatus"))).Text.ToString();
                        paramvalue[13] = Convert.ToDecimal(((TextBox)(gvUCR.Rows[i].FindControl("lblCharges"))).Text.ToString());
                        paramvalue[14] = ((Label)(gvUCR.Rows[i].FindControl("lblCurrency"))).Text.ToString();
                        paramvalue[15] = ((TextBox)(gvUCR.Rows[i].FindControl("lblComment"))).Text.ToString();
                        paramvalue[16] = ((Label)(gvUCR.Rows[i].FindControl("lblPossessor"))).Text.ToString();
                        paramvalue[17] = ((Label)(gvUCR.Rows[i].FindControl("lblInternal"))).Text.ToString();
                        paramvalue[18] = ((Label)(gvUCR.Rows[i].FindControl("lblisCustomer"))).Text.ToString();
                        paramvalue[19] =Convert.ToDecimal( ((Label)(gvUCR.Rows[i].FindControl("lblRates"))).Text.ToString());
                        paramvalue[20] = ((Label)(gvUCR.Rows[i].FindControl("lblLocationType"))).Text.ToString();
                        paramvalue[21] = ((Label)(gvUCR.Rows[i].FindControl("lblLocationID"))).Text.ToString();

                        SqlDbType[] paramtype = new SqlDbType[22];
                        paramtype[0] = SqlDbType.BigInt;
                        paramtype[1] = SqlDbType.NVarChar;
                        paramtype[2] = SqlDbType.NVarChar;
                        paramtype[3] = SqlDbType.Int;
                        paramtype[4] = SqlDbType.NVarChar;
                        paramtype[5] = SqlDbType.Int;
                        paramtype[6] = SqlDbType.NVarChar;
                        paramtype[7] = SqlDbType.NVarChar;
                        paramtype[8] = SqlDbType.NVarChar;
                        paramtype[9] = SqlDbType.NVarChar;
                        paramtype[10] = SqlDbType.DateTime;
                        paramtype[11] = SqlDbType.DateTime;
                        paramtype[12] = SqlDbType.NVarChar;
                        paramtype[13] = SqlDbType.Real;
                        paramtype[14] = SqlDbType.NVarChar;
                        paramtype[15] = SqlDbType.NVarChar;
                        paramtype[16] = SqlDbType.NVarChar;
                        paramtype[17] = SqlDbType.Bit;
                        paramtype[18] = SqlDbType.Bit;
                        paramtype[19] = SqlDbType.Real;
                        paramtype[20] = SqlDbType.NVarChar;
                        paramtype[21] = SqlDbType.BigInt;

                        SQLServer dbPONo = new SQLServer(Global.GetConnectionString());
                        bool dsPO = dbPONo.InsertData("spUpdateDemurrage", paramname, paramtype, paramvalue);
                        if (dsPO == true)
                        {
                            btnSearch_Click(null, null);
                            lblStatus.Text = "Saved Successfully";
                            lblStatus.ForeColor = Color.Green;
                        }
                        else 
                        {
                            lblStatus.Text = "Save failed";
                            lblStatus.ForeColor = Color.Red;
                        }
                    }
                }
                 
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet ds = db.SelectRecords("spgetAllDemVal");
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvUCR.DataSource = ds.Tables[0];
                        gvUCR.DataBind();
                    }
                    else {
                        gvUCR.DataSource = null;
                        gvUCR.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
