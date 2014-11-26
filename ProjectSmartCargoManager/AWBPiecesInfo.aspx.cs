using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data; 
using QID.DataAccess;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Configuration;
using System.Data.SqlClient;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class AWBPiecesInfo : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BookingBAL objBLL = new BookingBAL();
        DataSet dsDimensions;

        protected void Page_Load(object sender, EventArgs e)
        {
            //
            if (!IsPostBack)
            {
                lblPcsEntered.Text = Request.QueryString["PcsCount"].ToString();
                lblGrossWtEntered.Text = Request.QueryString["GrossWt"].ToString();
                if (Request.QueryString["Route"].ToString().Contains("Data")) 
                {
                    Session["RowIndex"] = Request.QueryString["RowIndex"].ToString();
                } 
                else 
                {
                    Session["RowIndex"] = Request.QueryString["RowIndex"].ToString();
                }
                

                LoadGridDimensionDetails();

            }
        }

        public void LoadGridDimensionDetails()
        {
            try
            {
                DataSet dsDimensionsTemp = null;
                if (Request.QueryString["Route"].ToString().Contains("Data"))
                {
                    dsDimensionsTemp = (DataSet)Session["dsRouteds"];
                    Session["RouteORG"] = dsDimensionsTemp.Copy();
                    if (dsDimensionsTemp != null && dsDimensionsTemp.Tables[0].Rows.Count > 0)
                    {

                        if (Convert.ToInt16(dsDimensionsTemp.Tables[0].Rows[0]["RowIndex"].ToString()) == Convert.ToInt16(Session["RowIndex"].ToString()))
                        {
                            grdAWBPieceDetails.DataSource = dsDimensionsTemp.Copy();
                            grdAWBPieceDetails.DataBind();
                        }
                        else
                        {   DataRow dt = dsDimensionsTemp.Tables[0].NewRow();
                            dsDimensionsTemp.Tables[0].Rows.Clear();
                            dsDimensionsTemp.Tables[0].Rows.Add(dt);
                            grdAWBPieceDetails.DataSource = dsDimensionsTemp.Copy();
                            grdAWBPieceDetails.DataBind();
                        }
                        Session["dsRouteds"] = dsDimensionsTemp.Copy();
                        txtPcs_TextChanged(new object(), new EventArgs());
                    }
                    else
                    {
                        DataRow dt = dsDimensionsTemp.Tables[0].NewRow();
                        dsDimensionsTemp.Tables[0].Rows.Clear();
                        dsDimensionsTemp.Tables[0].Rows.Add(dt);
                        grdAWBPieceDetails.DataSource = dsDimensionsTemp.Copy();
                        grdAWBPieceDetails.DataBind();

                        Session["dsRouteds"] = dsDimensionsTemp.Copy();
                        txtPcs_TextChanged(new object(), new EventArgs());
                    }          
                }
                else 
                {
                    dsDimensionsTemp = (DataSet)Session["dsPiecesDet"];
                    if (dsDimensionsTemp != null && dsDimensionsTemp.Tables[0].Rows.Count > 0)
                    {
                        dsDimensionsTemp.Tables[0].Rows[0]["RowIndex"] = Session["RowIndex"].ToString();

                        grdAWBPieceDetails.DataSource = dsDimensionsTemp.Copy();
                        grdAWBPieceDetails.DataBind();

                        Session["dsPiecesDet"] = dsDimensionsTemp.Copy();
                        txtPcs_TextChanged(new object(), new EventArgs());
                    }
                    else
                    {
                        DataRow dt = dsDimensionsTemp.Tables[0].NewRow();
                        
                        dsDimensionsTemp.Tables[0].Rows.Add(dt);
                        grdAWBPieceDetails.DataSource = dsDimensionsTemp.Copy();
                        grdAWBPieceDetails.DataBind();

                        Session["dsPiecesDet"] = dsDimensionsTemp.Copy();
                        txtPcs_TextChanged(new object(), new EventArgs());
                    }          
                }
                //if(dsDimensionsTemp !=null && dsDimensionsTemp.Tables.Count>0 && dsDimensionsTemp.Tables[0].Rows.Count=0)


                   

            }
            catch (Exception ex)
            {
                LBLStatus.Text=""+ex.Message;
                return;
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Response.Redirect("FrmConBooking.aspx", false);
            ScriptManager.RegisterStartupScript(btnCancel, btnCancel.GetType(), "HidUnhide", "window.close();", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (!IsInputValid())
                {
                    TXTFinalPieces.Text = "";
                    TXTFinalWt.Text = "";
                    return;
                }

                if (!CheckTotalPcsCount())
                    return;

                decimal wt = 0, grosswt = 0;

                if (Request.QueryString["GrossWt"].ToString().Trim() == "")
                {
                    grosswt = 0;
                }
                else
                {
                    try
                    {
                        grosswt = decimal.Parse(Request.QueryString["GrossWt"].ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        grosswt = 0;
                    }

                }

                if (TXTFinalWt.Text.Trim() != "")
                {
                    try
                    {
                        wt = decimal.Parse(TXTFinalWt.Text);

                    }
                    catch (Exception ex)
                    {
                        wt = 0;
                    }

                    if (grosswt > wt)
                    {
                        wt = grosswt;
                    }
                    else
                    {
                        //
                    }

                }

                DataSet dsDimensionsTemp = null;
                if (Request.QueryString["Route"].ToString().Contains("Data"))
                {
                    dsDimensionsTemp = (DataSet)Session["dsRouteds"];
                    DataSet dsRouteAll = (DataSet)Session["RouteORG"];
                    dsRouteAll.Merge(dsDimensionsTemp);
                    Session["dsRouteds"] = dsRouteAll;

                }
                else 
                {
                    dsDimensionsTemp = (DataSet)Session["dsPiecesDet"];
                   
                    DataView dv = new DataView(dsDimensionsTemp.Tables[0].Copy());
                    dv.RowFilter = "RowIndex <> " + Session["RowIndex"];


                    DataSet dsDimensionsAll = new DataSet();
                    dsDimensionsAll.Tables.Add(dv.ToTable().Copy());


                    dsDimensions = ((DataSet)Session["dsPiecesDet"]).Copy();

                    dsDimensionsTemp = dsDimensionsAll.Copy();
                    dsDimensionsTemp.Merge(dsDimensions);

                    Session["dsPiecesDet"] = dsDimensionsTemp.Copy();
                }


               

                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + wt + "','" + wt + "','" + TXTFinalWt.Text.Trim() + "');", true);
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Error()", true);
            }

        }

        protected void grdDimension_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                SaveGrid();
                if (Request.QueryString["Route"].ToString().Contains("Data"))
                    dsDimensions = (DataSet)Session["dsRouteds"];
                else
                    dsDimensions = (DataSet)Session["dsPiecesDet"];

                DataRow row = dsDimensions.Tables[0].NewRow();
                row["RowIndex"] = Session["RowIndex"].ToString();
                dsDimensions.Tables[0].Rows.Add(row);

                grdAWBPieceDetails.DataSource = dsDimensions.Tables[0];
                grdAWBPieceDetails.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        public bool CheckTotalPcsCount()
        {
            try
            {
                if (Convert.ToInt16(lblPcsEntered.Text.Trim()) != Convert.ToInt16(TXTFinalPieces.Text.Trim()) ||
                    Convert.ToDecimal(lblGrossWtEntered.Text.Trim()) != Convert.ToDecimal(TXTFinalWt.Text.Trim()))
                {
                    LBLStatus.Text = "Final pieces or Gross Wt. is not matching.";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                //LBLStatus.Text = ""+ex.Message;
                return false;
            }

        }

        public void SaveGrid()
        {
            try
            {
                Decimal AllowedWT = GetHeavyWtDefinedInSystem();
                
                if (Request.QueryString["Route"].ToString().Contains("Data"))
                     dsDimensions = (DataSet)Session["dsRouteds"];
                else
                    dsDimensions = (DataSet)Session["dsPiecesDet"]; 

                for (int i = 0; i < grdAWBPieceDetails.Rows.Count; i++)
                {

                    dsDimensions.Tables[0].Rows[i]["Pieces"] = ((TextBox)grdAWBPieceDetails.Rows[i].FindControl("txtPcs")).Text;
                    dsDimensions.Tables[0].Rows[i]["GrossWt"] = ((TextBox)grdAWBPieceDetails.Rows[i].FindControl("txtGrossWt")).Text;
                    dsDimensions.Tables[0].Rows[i]["PieceId"] = ((TextBox)grdAWBPieceDetails.Rows[i].FindControl("txtPieceId")).Text;
                    dsDimensions.Tables[0].Rows[i]["RowIndex"] = Session["RowIndex"].ToString();
                    try { dsDimensions.Tables[0].Rows[i]["isHeavy"] = Convert.ToDecimal(((TextBox)grdAWBPieceDetails.Rows[i].FindControl("txtGrossWt")).Text.ToString()) / Convert.ToDecimal(((TextBox)grdAWBPieceDetails.Rows[i].FindControl("txtPcs")).Text.ToString()) >= AllowedWT ? "True" : "False"; }
                    catch (Exception ex) { }
                }
                if (Request.QueryString["RowIndex"].ToString().Contains("Route")) 
                    Session["dsRouteds"] = dsDimensions.Copy();
                else
                    Session["dsPiecesDet"] = dsDimensions.Copy();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        #region GetHeavyWtDefinedInSystem
        private decimal GetHeavyWtDefinedInSystem()
        {
            decimal val = decimal.MaxValue;
            try
            {
                DataSet ds = da.SelectRecords("spGetHeavyWt");
                if (ds != null) 
                {
                    if (ds.Tables.Count > 0) 
                    {
                        if (ds.Tables[0].Rows.Count > 0) 
                        {
                            val = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex) 
            { }
            return val;
        }
        #endregion

        
        public bool IsInputValid()
        {
            try
            {
                int Pieces = 0;
                decimal GrossWt = 0; //, height = 0, pcscount = 0, totalpcscount=0;

                for (int i = 0; i < grdAWBPieceDetails.Rows.Count; i++)
                {
                    try
                    {
                        Pieces = int.Parse(((TextBox)grdAWBPieceDetails.Rows[i].FindControl("txtPcs")).Text);
                    }
                    catch
                    {
                        ((TextBox)grdAWBPieceDetails.Rows[i].FindControl("txtPcs")).Focus();
                        return false;
                    }

                    try
                    {
                        GrossWt = Convert.ToDecimal(((TextBox)grdAWBPieceDetails.Rows[i].FindControl("txtGrossWt")).Text);

                    }
                    catch
                    {
                        ((TextBox)grdAWBPieceDetails.Rows[i].FindControl("txtGrossWt")).Text = "";
                        ((TextBox)grdAWBPieceDetails.Rows[i].FindControl("txtGrossWt")).Focus();
                        return false;
                    }                    
                }

                LBLStatus.Text = "";
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void txtPcs_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (!IsInputValid())
                {
                    TXTFinalPieces.Text = "";
                    TXTFinalWt.Text = "";
                    return;
                }


                SaveGrid();

                if (Request.QueryString["Route"].ToString().Contains("Data"))
                    dsDimensions = (DataSet)Session["dsRouteds"];
                else
                    dsDimensions = (DataSet)Session["dsPiecesDet"]; 

                int FinalPieces = 0;
                decimal FinalWeight = 0;

                for (int i = 0; i < dsDimensions.Tables[0].Rows.Count; i++)
                {
                    if (dsDimensions.Tables[0].Rows[i]["Pieces"].ToString().Trim() != "" && dsDimensions.Tables[0].Rows[i]["GrossWt"].ToString().Trim() != "")
                    {
                        FinalPieces = FinalPieces + Convert.ToInt16(dsDimensions.Tables[0].Rows[i]["Pieces"].ToString().Trim());
                        FinalWeight = FinalWeight + Convert.ToDecimal(dsDimensions.Tables[0].Rows[i]["GrossWt"].ToString().Trim());
                    }                    
                }
                TXTFinalPieces.Text = FinalPieces.ToString();
                TXTFinalWt.Text = FinalWeight.ToString();
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        protected void btnDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                SaveGrid();
                if (Request.QueryString["Route"].ToString().Contains("Data"))
                    dsDimensions = (DataSet)Session["dsRouteds"];
                else
                    dsDimensions = (DataSet)Session["dsPiecesDet"]; 
                //dsDimensions = (DataSet)Session["dsPiecesDet"];
                DataSet newdsDimensions = dsDimensions.Clone();

                for (int i = 0; i < grdAWBPieceDetails.Rows.Count; i++)
                {
                    if (!((CheckBox)grdAWBPieceDetails.Rows[i].FindControl("CHKSelect")).Checked)
                    {
                        DataRow row=newdsDimensions.Tables[0].NewRow();
                        row["Pieces"] = "" + dsDimensions.Tables[0].Rows[i]["Pieces"].ToString();
                        row["GrossWt"] = "" + dsDimensions.Tables[0].Rows[i]["GrossWt"].ToString();
                        row["PieceId"] = "" + dsDimensions.Tables[0].Rows[i]["PieceId"].ToString();
                        row["RowIndex"] = Session["RowIndex"].ToString();

                        newdsDimensions.Tables[0].Rows.Add(row);
                    }
                }


                grdAWBPieceDetails.DataSource = newdsDimensions.Copy();
                grdAWBPieceDetails.DataBind();

                if (Request.QueryString["RowIndex"].ToString().Contains("Route"))
                    Session["dsRouteds"] = newdsDimensions.Copy();
                else
                    Session["dsPiecesDet"] = newdsDimensions.Copy();


                txtPcs_TextChanged(sender, e);

            }catch(Exception ex)
            {
                //LBLStatus.Text = "" + ex.Message;
                return;
            }
        }

    }
}