using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using System.Data.SqlClient;

namespace ProjectSmartCargoManager
{
    public partial class JQPlots : System.Web.UI.Page
    {
        Jquery objJquery = new Jquery();
        DataSet ds = new DataSet();
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GetData_Click(object sender, EventArgs e)
        {
            CallButtonGetData();
        }

        #region Timer1 Tick Event
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            GetData_Click(sender, e);
            //CallButtonGetData();
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Clear();</script>", false);
            //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('a');", true);
        }
        #endregion

        #region CallButtonGETDATA
        private void CallButtonGetData()
        {
            try
            {
                DateTime dt1, dt2 = new DateTime();

                dt1 = DateTime.ParseExact(txtFrmDate.Text.Trim(), "MM/dd/yyyy", null);
                dt2 = DateTime.ParseExact(txtToDate.Text.Trim(), "MM/dd/yyyy", null);

                ds = objJquery.GetTopAgent(dt1, dt2);

                //--------------------------------------------------------------------
                for (int i = ds.Tables[0].Rows.Count; i < 5; i++)   // To make sure of 5 rows
                {
                        ds.Tables[0].Rows.Add("",0);
                }
                lblAgent1.Text = ds.Tables[0].Rows[0]["AgentCode"].ToString();
                Label2.Text = ds.Tables[0].Rows[1]["AgentCode"].ToString();
                Label3.Text = ds.Tables[0].Rows[2]["AgentCode"].ToString();
                Label4.Text = ds.Tables[0].Rows[3]["AgentCode"].ToString();
                Label5.Text = ds.Tables[0].Rows[4]["AgentCode"].ToString();
                //--------------------------------------------------------------------
                Label6.Text = ds.Tables[0].Rows[0]["ChargedWeight"].ToString();
                Label7.Text = ds.Tables[0].Rows[1]["ChargedWeight"].ToString();
                Label8.Text = ds.Tables[0].Rows[2]["ChargedWeight"].ToString();
                Label9.Text = ds.Tables[0].Rows[3]["ChargedWeight"].ToString();
                Label10.Text = ds.Tables[0].Rows[4]["ChargedWeight"].ToString();
                //--------------------------------------------------------------------


                for (int i = ds.Tables[1].Rows.Count; i < 5; i++)   // To make sure of 5 rows
                {
                    ds.Tables[1].Rows.Add("", 0);
                }
                lblSector1.Text = ds.Tables[1].Rows[0]["Origin"].ToString();
                lblSector2.Text = ds.Tables[1].Rows[1]["Origin"].ToString();
                lblSector3.Text = ds.Tables[1].Rows[2]["Origin"].ToString();
                lblSector4.Text = ds.Tables[1].Rows[3]["Origin"].ToString();
                lblSector5.Text = ds.Tables[1].Rows[4]["Origin"].ToString();
                //-------------------------------------------------------------------
                lblTonnage1.Text = ds.Tables[1].Rows[0]["CTonnage"].ToString();
                lblTonnage2.Text = ds.Tables[1].Rows[1]["CTonnage"].ToString();
                lblTonnage3.Text = ds.Tables[1].Rows[2]["CTonnage"].ToString();
                lblTonnage4.Text = ds.Tables[1].Rows[3]["CTonnage"].ToString();
                lblTonnage5.Text = ds.Tables[1].Rows[4]["CTonnage"].ToString();
                //-------------------------------------------------------------------

                for (int i = ds.Tables[2].Rows.Count; i < 5; i++)   // To make sure of 5 rows
                {
                    ds.Tables[2].Rows.Add("", 0);
                }
                lblBtonage1.Text = ds.Tables[2].Rows[0]["AgentCode"].ToString();
                lblBtonage2.Text = ds.Tables[2].Rows[1]["AgentCode"].ToString();
                lblBtonage3.Text = ds.Tables[2].Rows[2]["AgentCode"].ToString();
                lblBtonage4.Text = ds.Tables[2].Rows[3]["AgentCode"].ToString();
                lblBtonage5.Text = ds.Tables[2].Rows[4]["AgentCode"].ToString();

                lblBotom1.Text = ds.Tables[2].Rows[0]["ChargedWeight"].ToString();
                lblBotom2.Text = ds.Tables[2].Rows[1]["ChargedWeight"].ToString();
                lblBotom3.Text = ds.Tables[2].Rows[2]["ChargedWeight"].ToString();
                lblBotom4.Text = ds.Tables[2].Rows[3]["ChargedWeight"].ToString();
                lblBotom5.Text = ds.Tables[2].Rows[4]["ChargedWeight"].ToString();
                //--------------------------------------------------------------------


                for (int i = ds.Tables[3].Rows.Count; i < 5; i++)   // To make sure of 5 rows
                {
                    ds.Tables[3].Rows.Add("", 0);
                }
                lblbotsector1.Text = ds.Tables[3].Rows[0]["BottomOrigin"].ToString();
                lblbotsector2.Text = ds.Tables[3].Rows[1]["BottomOrigin"].ToString();
                lblbotsector3.Text = ds.Tables[3].Rows[2]["BottomOrigin"].ToString();
                lblbotsector4.Text = ds.Tables[3].Rows[3]["BottomOrigin"].ToString();
                lblbotsector5.Text = ds.Tables[3].Rows[4]["BottomOrigin"].ToString();
                //--------------------------------------------------------------------
                lblbottonnage1.Text = ds.Tables[3].Rows[0]["BottomCTonnage"].ToString();
                lblbottonnage2.Text = ds.Tables[3].Rows[1]["BottomCTonnage"].ToString();
                lblbottonnage3.Text = ds.Tables[3].Rows[2]["BottomCTonnage"].ToString();
                lblbottonnage4.Text = ds.Tables[3].Rows[3]["BottomCTonnage"].ToString();
                lblbottonnage5.Text = ds.Tables[3].Rows[4]["BottomCTonnage"].ToString();
                //--------------------------------------------------------------------


                for (int i = ds.Tables[4].Rows.Count; i < 5; i++)   // To make sure of 5 rows
                {
                    ds.Tables[4].Rows.Add("", 0);
                }
                lblPendingInvoice1.Text = ds.Tables[4].Rows[0]["PendingInvoice"].ToString();
                lblPendingInvoice2.Text = ds.Tables[4].Rows[1]["PendingInvoice"].ToString();
                lblPendingInvoice3.Text = ds.Tables[4].Rows[2]["PendingInvoice"].ToString();
                lblPendingInvoice4.Text = ds.Tables[4].Rows[3]["PendingInvoice"].ToString();
                lblPendingInvoice5.Text = ds.Tables[4].Rows[4]["PendingInvoice"].ToString();
                //--------------------------------------------------------------------
                lblPendingInvoiceVal1.Text = ds.Tables[4].Rows[0]["PendingInvoiceVal"].ToString();
                lblPendingInvoiceVal2.Text = ds.Tables[4].Rows[1]["PendingInvoiceVal"].ToString();
                lblPendingInvoiceVal3.Text = ds.Tables[4].Rows[2]["PendingInvoiceVal"].ToString();
                lblPendingInvoiceVal4.Text = ds.Tables[4].Rows[3]["PendingInvoiceVal"].ToString();
                lblPendingInvoiceVal5.Text = ds.Tables[4].Rows[4]["PendingInvoiceVal"].ToString();
                //--------------------------------------------------------------------

                if (ds.Tables[5].Rows.Count > 0)
                {
                    if (ds.Tables[5].Rows[0]["TotPendAmount"].ToString() == "")
                    {
                        lblTotPendAmount1.Text = "0";
                    }
                    else
                    {
                        lblTotPendAmount1.Text = ds.Tables[5].Rows[0]["TotPendAmount"].ToString();
                    }

                    if (ds.Tables[5].Rows[0]["TotColAmount"].ToString() == "")
                    {
                        lblTotColAmount1.Text = "0";
                    }
                    else
                    {
                        lblTotColAmount1.Text = ds.Tables[5].Rows[0]["TotColAmount"].ToString();
                    }
                }
                // ClientScript.RegisterStartupScript(this.GetType(), "", "CallPopulateClick();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>CallPopulateClick();</script>", false);

            }
            catch (Exception ex)
            { }
        }
        #endregion
    }
}
