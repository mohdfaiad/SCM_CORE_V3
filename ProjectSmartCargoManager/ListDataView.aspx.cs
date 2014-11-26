using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class ListDataView : System.Web.UI.Page
    {

        ListDataViewBAL objBAL = new ListDataViewBAL();
        string errormessage = "";
        DataSet dsResult = new DataSet();
      
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string parentpage = Request.QueryString["Parent"].ToString();

                    switch (parentpage)
                    {
                        case "MaintainRatesORG":
                            //DataSet dsResult = new DataSet();
                            string level = Request.QueryString["level"].ToString();                            
                            if (objBAL.FillDdl("Origin", ref dsResult, ref errormessage, new string[] { level }))
                            {
                                GRD.DataSource = null;
                                GRD.DataSource = dsResult.Tables[0];
                                GRD.DataBind();
                            }

                            GRD.Columns[1].HeaderText = "Origin";
                            break;

                        case "MaintainRatesDEST":
                            //dsResult = new DataSet();
                            level = Request.QueryString["level"].ToString();
                            if (objBAL.FillDdl("Origin", ref dsResult, ref errormessage, new string[] { level }))
                            {
                                GRD.DataSource = null;
                                GRD.DataSource = dsResult.Tables[0];
                                GRD.DataBind();
                            }

                            GRD.Columns[1].HeaderText = "Origin";
                            break;


                        case "MaintainRatesCUR":
                            //dsResult = new DataSet();
                            if (objBAL.FillDdl("Currency", ref dsResult, ref errormessage, new string[] {}))
                            {
                                GRD.DataSource = null;
                                GRD.DataSource = dsResult.Tables[0];
                                GRD.DataBind();
                            }

                            GRD.Columns[1].HeaderText = "Currency";
                            break;

                        case "OtherChargesCode":
                            dsResult = new DataSet();
                            if (objBAL.FillDdl("Code", ref dsResult, ref errormessage, new string[] { }))
                            {
                                GRD.DataSource = null;
                                GRD.DataSource = dsResult.Tables[0];
                                GRD.DataBind();
                            }

                            GRD.Columns[1].HeaderText = "Code";
                            break;

                        case "AgentCode":
                            //DataSet dsResult = new DataSet();
                            level = Request.QueryString["level"].ToString();
                            if (objBAL.FillDdl("Param", ref dsResult, ref errormessage, new string[] { level }))
                            {
                                GRD.DataSource = null;
                                GRD.DataSource = dsResult.Tables[0];
                                GRD.DataBind();
                            }

                            GRD.Columns[1].HeaderText = "Agent Code";
                            Session["Agent"] = "Agent";
                            break;

                    }

                    if (Session["mode"].ToString() == "Edit" || Session["mode"].ToString() == "View")
                    {
                        showCheckSelection(dsResult);
                    }

                }

            }catch(Exception ex)
            {

            }

        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try { }
            catch (Exception ex) 
            { }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void select_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < GRD.Rows.Count; i++)
                {
                    if (((RadioButton)GRD.Rows[i].FindControl("RBT")) == (RadioButton)sender)
                    {
                        if (Session["Agent"] != null && Session["Agent"].ToString().ToUpper() == "AGENT")
                        {
                            try
                            {
                                if (Request.QueryString["AName"].ToString() != null)
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseAgentNameWindow('" + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text + "','" + ((HiddenField)GRD.Rows[i].FindControl("Hid")).Value + "');", true);
                                    break;
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseAgentWindow('" + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text + "','" + ((HiddenField)GRD.Rows[i].FindControl("Hid")).Value + "');", true);
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseAgentWindow('" + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text + "','" + ((HiddenField)GRD.Rows[i].FindControl("Hid")).Value + "');", true);
                                break;
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text + "','" + ((HiddenField)GRD.Rows[i].FindControl("Hid")).Value + "');", true);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { }
            finally
            {
                Session["Agent"] = null;
            }
        }

        #region  show radio button checked in edit and view
        protected void showCheckSelection(DataSet dsRes)
        {
            string origin = Session["Origin"].ToString();
            string destination = Session["Destination"].ToString();
            string currency = Session["Currency"].ToString();

            for (int i = 0; i < GRD.Rows.Count; i++)
            {
                string parentpage = Request.QueryString["Parent"].ToString();

                switch (parentpage)
                {
                    case "MaintainRatesORG":
                        if (dsRes.Tables[0].Rows[i][0].ToString() == origin || dsRes.Tables[0].Rows[i][0].ToString() == destination || dsRes.Tables[0].Rows[i][0].ToString() == currency)
                        {
                            ((RadioButton)GRD.Rows[i].FindControl("RBT")).Checked = true;
                        }
                        else
                        {
                            ((RadioButton)GRD.Rows[i].FindControl("RBT")).Checked = false;
                        }
                        break;

                    case "MaintainRatesDEST":
                        if (dsRes.Tables[0].Rows[i][0].ToString() == destination)
                        {
                            ((RadioButton)GRD.Rows[i].FindControl("RBT")).Checked = true;
                        }
                        else
                        {
                            ((RadioButton)GRD.Rows[i].FindControl("RBT")).Checked = false;
                        }
                        break;


                    case "MaintainRatesCUR":
                        if (dsRes.Tables[0].Rows[i][0].ToString() == currency)
                        {
                            ((RadioButton)GRD.Rows[i].FindControl("RBT")).Checked = true;
                        }
                        else
                        {
                            ((RadioButton)GRD.Rows[i].FindControl("RBT")).Checked = false;
                        }
                        break;
                }
                
           
                if (Session["mode"].ToString() == "View")
                {
                    ((RadioButton)GRD.Rows[i].FindControl("RBT")).Enabled = false;
                }
            }

        }

        #endregion  show radio button checked in edit and view

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < GRD.Rows.Count; i++)
                {
                    if (((RadioButton)GRD.Rows[i].FindControl("RBT")) == (RadioButton)sender)
                    {
                        if (Session["Agent"] != null && Session["Agent"].ToString().ToUpper() == "AGENT")
                        {
                            try
                            {
                                if (Request.QueryString["AName"].ToString() != null)
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseAgentNameWindow('" + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text + "','" + ((HiddenField)GRD.Rows[i].FindControl("Hid")).Value + "');", true);
                                    break;
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseAgentWindow('" + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text + "','" + ((HiddenField)GRD.Rows[i].FindControl("Hid")).Value + "');", true);
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseAgentWindow('" + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text + "','" + ((HiddenField)GRD.Rows[i].FindControl("Hid")).Value + "');", true);
                                break;
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + ((Label)GRD.Rows[i].FindControl("LBLValue")).Text + "','" + ((HiddenField)GRD.Rows[i].FindControl("Hid")).Value + "');", true);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { }
            finally
            {
                Session["Agent"] = null;
            }
        }
    }
}
