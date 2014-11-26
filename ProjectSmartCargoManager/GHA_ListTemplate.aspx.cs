using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using BAL;
using System.Globalization;
using QID.DataAccess;
using System.IO;
using System.Drawing;
using System.Threading;
//using Office = Microsoft.Office.Core;


/*

  2012-07-04 vinayak
  2012-07-06  vinayak Edit/View

*/

namespace ProjectSmartCargoManager
{
    public partial class ListTemplate_GHA : System.Web.UI.Page
    {
        ListBookingBAL objBAL = new ListBookingBAL();
        BookingBAL objBLL = new BookingBAL();
        string errormessage = "";
        UserCreationBAL objBal = new UserCreationBAL();
        DateTime dtCurrentDate = DateTime.Now;

        string srno1 = "", tempi = "";
        #region Handlers

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadStationDropDowns();
                FillUserDropdown();
                LoadMatchingProductTypes();
                GetPrefix();
                CompareValidator1.ValueToCompare = DateTime.Now.ToString("yyyy-MM-dd"); //DateTime.Now.ToShortDateString();
                CompareValidator2.ValueToCompare = DateTime.Now.ToString("yyyy-MM-dd");
                txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                TextBox6.Text = DateTime.Now.ToString("yyyy-MM-dd");

                rbdsun.Visible = false;
                rbdmon.Visible = false;
                rbdtue.Visible = false;
                rbdwed.Visible = false;
                rbdthu.Visible = false;
                rbdfri.Visible = false;
                rbdsat.Visible = false;

                if (Convert.ToString(Session["AgentCode"]) != "")
                {
                    drpUsers.SelectedValue = Convert.ToString(Session["AgentCode"]);
                    drpUsers.Enabled = false;
                }
                //txtAWBFromDt.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                //txtAWBToDt.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");

                txtFltFromDt.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                txtFltToDt.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");

                //LoadCommodityDropdown();
               
                //if (Session["awbPrefix"] != null)
                //    txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                //else
                //{
                //    MasterBAL objBal = new MasterBAL();
                //    Session["awbPrefix"] = objBal.awbPrefix();
                //    txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                //}
            }
        }
        #region Get Current Instance Prefix
        public void GetPrefix()
        {
            SQLServer objSQL = new SQLServer(Global.GetConnectionString());
            DataSet dsInstance = new DataSet();

            dsInstance = objSQL.SelectRecords("GetCurrentInstance");
            string current = dsInstance.Tables[0].Rows[0][0].ToString();
         txtFlightCode.Text = current;

        }
        #endregion

        #region Load Commodity Dropdown
        //public void LoadCommodityDropdown()
        //{
        //    DataSet ds = objBLL.GetCommodityList("");
        //    DropDownList ddl = new DropDownList();
        //    ddl = ddlCommCode;
           
               
        //        if (ds != null)
        //        {
        //            ddl.DataSource = ds;
        //            ddl.DataMember = ds.Tables[0].TableName;
        //            ddl.DataTextField = "CommodityCode";
        //            ddl.DataValueField = "CommodityCode";
        //            ddl.DataBind();
        //            ddl.Items.Insert(0, "All");
        //            Session["Description"] = ds;
        //        }
            
        //}
        #endregion Load Commodity Dropdown

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValide())
                    return;

                DataSet dsResult = new DataSet();

                string source, dest, flight, fromdate, awbnumber, awbprefix, todate, CommodityCode,user;
                source = dest = flight = fromdate = todate = awbnumber = user="";
                
                source=ddlSource.SelectedItem.Text.Trim()=="All"?"":ddlSource.SelectedItem.Text.Trim();
                dest=ddlDest.SelectedItem.Text.Trim()=="All"?"":ddlDest.SelectedItem.Text.Trim();
                //flight=ddlFlight.SelectedItem.Text.Trim()=="All"?"":ddlFlight.SelectedItem.Text.Trim();
                if (txtFlightCode.Text.Trim() != "" && txtFlightID.Text.Trim() != "")
                    flight = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();

                awbnumber = "";// txtAWBNo.Text.Trim();
                user = drpUsers.SelectedItem.Text.Trim() == "All" ? "" : drpUsers.SelectedItem.Text.Trim();

                
                string FltFromdt = txtFltFromDt.Text.Trim();
                string FltTodt = txtFltToDt.Text.Trim();
                
                string AgentCode = Convert.ToString(Session["ACode"]);

                CommodityCode = txtCommodityCode.Text.Trim();

                if (objBAL.GetBookingTemplate(source, dest, flight, FltFromdt, FltTodt, TXTAgentCode.Text.Trim(), CommodityCode, user, ref dsResult, ref errormessage))
                {
                    GRDBooking.DataSource = dsResult.Copy();
                    GRDBooking.DataBind();

                    Session["dsListBooking"] = dsResult.Copy();
                    
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;

            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        }

        //protected void GRDBooking_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (true)//(e.CommandName == "Edit" || e.CommandName == "View")
        //    {  
        //        //Response.Redirect("~//GHA_ConBooking.aspx?command=" + e.CommandName + "&AWBNumber=" +GRDBooking.Rows[int.Parse(e.CommandArgument.ToString())].Cells[1].Text.Trim());
        //        Response.Redirect("~//GHA_ConBooking.aspx?command=TemplateBooking&TemplateID=" + ((TextBox)GRDBooking.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("txtTemplateID")).Text.Trim() + "&GHA=False");

        //    }

        //}

        //protected void ddlDest_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GetFlights();
        //}

        //protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GetFlights();
        //}

        protected void GRDBooking_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult= (DataSet)Session["dsListBooking"];

            GRDBooking.PageIndex = e.NewPageIndex;
            GRDBooking.DataSource = dsResult.Copy();
            GRDBooking.DataBind();

        }

        #endregion

        #region UserDefined

        public void LoadStationDropDowns()
        {
            DataSet dsResult = new DataSet();
            try
            {
                if (objBAL.GetAllStaions(ref dsResult, ref errormessage))
                {

                    DataRow row = dsResult.Tables[0].NewRow();
                    row["Code"] = "All";
                    dsResult.Tables[0].Rows.Add(row);

                    ddlSource.DataSource = dsResult.Tables[0].Copy();
                    ddlSource.DataTextField = "Code";
                    ddlSource.DataValueField = "Code";
                    ddlSource.DataBind();

                    ddlDest.DataSource = dsResult.Tables[0].Copy();
                    ddlDest.DataTextField = "Code";
                    ddlDest.DataValueField = "Code";
                    ddlDest.DataBind();

                    //ddlFlight.Items.Clear();
                    //ddlFlight.Items.Add(new ListItem("All", "All"));

                    ddlSource.SelectedValue = "All";
                    ddlDest.SelectedValue = "All";

                }
                else
                {
                    lblStatus.Text = "" + errormessage;
                }
            }
            catch(Exception ex)
            {
                dsResult = null;
            }
            finally
            {
                if(dsResult!=null)
                    dsResult.Dispose();
            }

        }

        public bool IsValide()
        {
            lblStatus.Text = "";
            DateTime dtDatetime;

            if (txtFltFromDt.Text.Trim() != "")
            {
                try
                {
                    dtDatetime = DateTime.ParseExact(txtFltFromDt.Text.Trim(), "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Please enter valid Flight From date.";
                    txtFltFromDt.Focus();
                    return false;
                }
            }

            if (txtFltToDt.Text.Trim() != "")
            {
                try
                {
                    dtDatetime = DateTime.ParseExact(txtFltToDt.Text.Trim(), "dd/MM/yyyy", null);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Please enter valid Flight To date.";
                    txtFltToDt.Focus();
                    return false;
                }
            }

            
            if (txtFltFromDt.Text.Trim() != "" && txtFltToDt.Text.Trim() != "")
            {
                if (DateTime.ParseExact(txtFltFromDt.Text.Trim(), "dd/MM/yyyy", null) > DateTime.ParseExact(txtFltToDt.Text.Trim(), "dd/MM/yyyy", null))
                {
                    lblStatus.Text = "Flight From date should be smaller than Flight to date.";
                    return false;
                }
            }

            return true;
        }

        #endregion

       
        //protected void btnPrint_Click(object sender, EventArgs e)
        //{
        //    DataSet ds = null;
        //    DataTable dt = null;

        //    try
        //    {
        //        if ((DataSet)Session["dsListBooking"] == null)
        //            return;
                
        //        ds = (DataSet)Session["dsListBooking"];
        //        dt = (DataTable)ds.Tables[0];
        //        //dt = city.GetAllCity();//your datatable 
        //        string attachment = "attachment; filename=ListBooking.xls";
        //        Response.ClearContent();
        //        Response.AddHeader("content-disposition", attachment);
        //        Response.ContentType = "application/vnd.ms-excel";
        //        string tab = "";
        //        foreach (DataColumn dc in dt.Columns)
        //        {
        //            Response.Write(tab + dc.ColumnName);
        //            tab = "\t";
        //        }
        //        Response.Write("\n");
        //        int i;
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            tab = "";
        //            for (i = 0; i < dt.Columns.Count; i++)
        //            {
        //                Response.Write(tab + dr[i].ToString());
        //                tab = "\t";
        //            }
        //            Response.Write("\n");
        //        }
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        ds = null;
        //        dt = null;
        //    }
        //    finally
        //    {
        //        if (ds != null)
        //            ds.Dispose();
        //        if (dt != null)
        //            dt.Dispose();
        //    }
            
        //}

        #region Fill User Dropdown
        private void FillUserDropdown()
        {
            DataSet dsUsers = new DataSet();
            string user;
            user = Session["UserName"].ToString().ToUpper();
         //   user.ToUpper();
            try
            {
                dsUsers = objBal.GetUserListData("", 0, "");
                if (dsUsers != null)
                {
                    drpUsers.DataSource = dsUsers;
                    drpUsers.DataMember = dsUsers.Tables[0].TableName;
                    drpUsers.DataTextField = "LoginID";
                    drpUsers.DataValueField = "UserName";
                    drpUsers.DataBind();
                    drpUsers.Items.Insert(0, new ListItem("All", ""));
                    drpUsers.SelectedItem.Text = user;
                   // drpUsers.SelectedIndex = drpUsers.Items.IndexOf(drpUsers.Items.FindByText(user.ToString()));
                       
                }
            }
            catch (Exception ex) 
            {
                dsUsers = null;
            }
            finally 
            {
                if (dsUsers != null)
                    dsUsers.Dispose();
            }
        }
        #endregion

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/GHA_ListTemplate.aspx");
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetCommodityCodesWithName(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string"); 
            SqlDataAdapter dad = null;
            DataSet ds = new DataSet();
            dad = new SqlDataAdapter("SELECT CommodityCode + '(' + Description + ')' from CommodityMaster where (Description like '%" + prefixText + "%' or CommodityCode like '%" + prefixText + "%')", con);
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());
            }
            dad.Dispose();
            if (ds != null)
            {
                ds.Dispose();
            }
            
            
            return list.ToArray();
        }
        protected void pop_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = 0;

                for (int i = 0; i < GRDBooking.Rows.Count; i++)
                {
                    if (((CheckBox)GRDBooking.Rows[i].FindControl("CHKSelect")).Checked == true)
                    {
                        cnt = cnt + 1;
                    }
                }
                if (cnt > 1 || cnt == 0)
                {
                    lblStatus.Text = "Please select only one record.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                rbdsun.Visible = false;
                rbdmon.Visible = false;
                rbdtue.Visible = false;
                rbdwed.Visible = false;
                rbdthu.Visible = false;
                rbdfri.Visible = false;
                rbdsat.Visible = false;
                //rbdnDaily.Checked = true;
                //RadioButton3.Checked = true;
                for (int i = 0; i < GRDBooking.Rows.Count; i++)
                {
                    if (((CheckBox)GRDBooking.Rows[i].FindControl("CHKSelect")).Checked == true)
                    {
                       // txtorigin.Text = GRDBooking.Rows[i].Cells[3].Controls[0].ToString();
                        srno1 = ((Label)GRDBooking.Rows[i].FindControl("lblsrno")).Text;
                        tempi = ((TextBox)GRDBooking.Rows[i].FindControl("txtTemplateID")).Text;
                        txtorigin.Text = GRDBooking.Rows[i].Cells[2].Text;
                        txtdest.Text = GRDBooking.Rows[i].Cells[3].Text;
                        TextBox5.Text = GRDBooking.Rows[i].Cells[9].Text;
                        txtweight.Text = GRDBooking.Rows[i].Cells[10].Text;
                        TextBox3.Text = GRDBooking.Rows[i].Cells[7].Text;
                        txthndlnginfo.Text = ((Label)GRDBooking.Rows[i].FindControl("lblHandlingInfo")).Text;
                        //TextBox4.Text = GRDBooking.Rows[i].Cells[8].Text;
                        //Commented by Deepak for ToolTip
                        TextBox4.Text = ((Label)GRDBooking.Rows[i].FindControl("lblCommodityDesc")).ToolTip;

                        string[] paramname = new string[2];
                        paramname[0] = "SerialNumber";
                        paramname[1] = "TemplateID";

                        object[] paramvalue = new object[2];
                        paramvalue[0] = srno1;

                        paramvalue[1] = tempi;

                        SqlDbType[] paramtype = new SqlDbType[2];
                        paramtype[0] = SqlDbType.VarChar;
                        paramtype[1] = SqlDbType.VarChar;

                        SQLServer sq = new SQLServer(Global.GetConnectionString());
                        DataSet ds = sq.SelectRecords("SP_GetTemplateID", paramname, paramvalue, paramtype);

                        if (ds != null)
                        {
                            if (ds.Tables != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    txtPartnerType.Text = ds.Tables[0].Rows[0]["PartnerType"].ToString();
                                    txtPartnercode.Text = ds.Tables[0].Rows[0]["Carrier"].ToString();
                                    //txtfltdt.Text = ds.Tables[0].Rows[0]["FltDate"].ToString();
                                    //fltID.Items.Add(ds.Tables[0].Rows[0]["FltNumber"].ToString());
                                    //txtProductType.Text = ds.Tables[1].Rows[0]["ProductType"].ToString();
                                    ddlProductType.SelectedValue = ds.Tables[1].Rows[0]["ProductType"].ToString();
                                    
                                    //TextBox3.Text = ds.Tables[2].Rows[0]["CommodityCode"].ToString();
                                    //TextBox4.Text = ds.Tables[2].Rows[0]["CommodityDesc"].ToString();
                                    
                                    fltID.Items.Clear();
                                    txtfltdt.Text = "";
                                    lblstatuspop.Text = "";
                                    Textbox31.Text = "";
                                    txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                                    TextBox6.Text = DateTime.Now.ToString("yyyy-MM-dd");
                                    rbdnDaily.Checked = false;
                                    rbdnMonthly.Checked = false;
                                    rbdnWeekly.Checked = false;
                                    chkmonday.Checked = false;
                                    ChkTuesday.Checked = false;
                                    ChkWednesday.Checked = false;
                                    ChkThursday.Checked = false;
                                    ChkFriday.Checked = false;
                                    ChkSaturday.Checked = false;
                                    chksunday.Checked = false;

                                    rbdsun.Checked = true;
                                    rbdmon.Checked = true;
                                    rbdtue.Checked = true;
                                    rbdwed.Checked = true;
                                    rbdthu.Checked = true;
                                    rbdfri.Checked = true;
                                    rbdsat.Checked = true;
                                    //rbdnDaily_CheckedChanged(null, null);
                                    //rbdnMonthly_CheckedChanged(null, null);
                                    //rbdnWeekly_CheckedChanged(null, null);
                                    
                                }
                            }
                        }
                    }
                }
                #region load flight on origin dest
                DataSet dsresult = null;
                try
                {
                    string strPartnerCode = txtPartnercode.Text.Trim();//((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartner")).Text.Trim();

                    string errormessage = "";
                    // DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);
                    dsresult = GetFlightList(txtorigin.Text.ToUpper(), txtdest.Text.ToUpper(), System.DateTime.Now.ToString("dd/MM/yyyy"), 0, 0, 0, ref errormessage, strPartnerCode);

                    if (dsresult != null && dsresult.Tables.Count != 0)
                    {
                        DataSet ds = (DataSet)Session["FltID"];
                        if (ds != null)
                        {
                            string name = "Table" + 0;
                            try
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    try
                                    {
                                        if (ds.Tables[name] != null && ds.Tables[name].Rows.Count > 0)
                                        {
                                            ds.Tables.Remove(name);
                                            DataTable dt = new DataTable();
                                            dt = dsresult.Tables[0].Copy();
                                            dt.TableName = name;
                                            ds.Tables.Add(dt);
                                            ds.AcceptChanges();
                                            Session["FltID"] = ds.Copy();
                                        }
                                    }
                                    catch (Exception ex) { }

                                }
                                else if (ds.Tables.Count == 1)
                                {
                                    Session["FltID"] = dsresult.Copy();
                                }
                            }
                            catch (Exception ex) { }

                        }
                        else
                        {
                            Session["FltID"] = dsresult.Copy();
                        }
                        //DataRow row = dsresult.Tables[0].NewRow();
                        //if (Convert.ToBoolean(Session["FRV"]))
                        //{
                        //    row["FltNumber"] = "Select";
                        //    row["ArrTime"] = "Select";
                        //}
                        //else
                        //{
                        //    row["FltNumber"] = " ";
                        //    row["ArrTime"] = " ";
                        //}

                        //dsresult.Tables[0].Rows.Add(row);

                        ddlFltID.DataTextField = "FltNumber";
                        ddlFltID.DataValueField = "ArrTime";
                        ddlFltID.DataSource = dsresult.Tables[0].Copy();
                        ddlFltID.DataBind();
                        //fltID.Items.Add("Select");
                        ddlFltID.Items.Insert(0, new ListItem("Select", "Select"));
                        //fltID.SelectedItem.Text = "Select";
                        //fltID.SelectedIndex = dsresult.Tables[0].Rows.Count - 1;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                        //HidProcessFlag.Value = "1";
                    }
                    else
                    {
                        lblStatus.Text = "no record found";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        ddlFltID.Items.Clear();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    dsresult = null;
                }
                finally
                {
                    if (dsresult != null)
                        dsresult.Dispose();
                }
                #endregion

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
            }
            catch (Exception ex) { }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = 0;

                for (int i = 0; i < GRDBooking.Rows.Count; i++)
                {
                    if (((CheckBox)GRDBooking.Rows[i].FindControl("CHKSelect")).Checked == true)
                    {
                        cnt = cnt + 1;
                    }
                }
                if (cnt > 1 || cnt == 0)
                {
                    lblStatus.Text = "Please select only one record.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                string[] paramname = new string[3];
                paramname[0] = "SerialNumber";
                paramname[1] = "TemplateID";
                paramname[2] = "Flg";
                string tempid = "", srno = "";
                for (int i = 0; i < GRDBooking.Rows.Count; i++)
                {
                    if (((CheckBox)GRDBooking.Rows[i].FindControl("CHKSelect")).Checked == true)
                    {
                        srno = ((Label)GRDBooking.Rows[i].FindControl("lblsrno")).Text;
                        tempid = ((TextBox)GRDBooking.Rows[i].FindControl("txtTemplateID")).Text;
                        
                        
                    }
                }
                object[] paramvalue = new object[3];
                paramvalue[0] = srno;

                paramvalue[1] = tempid;
                paramvalue[2] = "UP";
                SqlDbType[] paramtype = new SqlDbType[3];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;

                SQLServer sq = new SQLServer(Global.GetConnectionString());
                DataSet ds = sq.SelectRecords("SP_SaveTemplateID", paramname, paramvalue, paramtype);

                if (ds !=null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0][0].ToString() == "TRUE")
                            {
                                btnList_Click(null, null);
                                lblStatus.Text = "Record Save Successfully.";
                                lblStatus.ForeColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                lblStatus.Text = "Record not saved.";
                                lblStatus.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        #region Load Matching Product Types
        private void LoadMatchingProductTypes()
        {
            DataSet ds = null;
            try
            {
                BALProductType objBAL = new BALProductType();
                ds = objBAL.GetMatchingProductType(txtorigin.Text, ddlDest.SelectedValue, "", "", "", "", 0,"");

                if (ds != null)
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 1)
                        {
                            ddlProductType.DataSource = ds.Tables[1];
                            ddlProductType.DataValueField = "SerialNumber";
                            ddlProductType.DataTextField = "ProductType";
                            ddlProductType.DataBind();
                        }
                        else
                        {
                            if (ds.Tables.Count > 2)
                            {
                                if (ds.Tables[2].Rows.Count > 1)
                                {
                                    ddlProductType.DataSource = ds.Tables[2];
                                    ddlProductType.DataValueField = "SerialNumber";
                                    ddlProductType.DataTextField = "ProductType";
                                    ddlProductType.DataBind();
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex) { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion
        protected void Button1_Click(object sender, EventArgs e)
        {
            int j = 0;
            string tempid = "", srno = "", Agent = "";
            string awb = "";
            
            try
            {
                if (TextBox5.Text == "")
                {
                    lblstatuspop.Text = "Please enter Pieces & try again..";
                    lblstatuspop.ForeColor = Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                    return;
                }
                if (txtweight.Text == "")
                {
                    lblstatuspop.Text = "Please enter weight & try again..";
                    lblstatuspop.ForeColor = Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                    return;
                }
                int testPcs=0;
                float testWt=0;
                if (!int.TryParse(TextBox5.Text, out testPcs))
                {
                    lblstatuspop.Text = "Invalid Pieces!";
                    lblstatuspop.ForeColor = Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                    return;
                }
                if (!float.TryParse(txtweight.Text, out testWt))
                {
                    lblstatuspop.Text = "Invalid Weight!";
                    lblstatuspop.ForeColor = Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                    return;
                }
                if (fltID.SelectedIndex == 0)
                {
                    lblStatus.Text = "Please Select Flight";
                    lblstatuspop.ForeColor = Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                    return;
                }
                if (rbdnDaily.Checked == true || rbdnMonthly.Checked == true || rbdnWeekly.Checked == true)
                {
                    if (ddlFltID.SelectedItem.Text == "Select")
                    {
                        lblstatuspop.Text = "Please select Flight in Reccurence Section.";
                        lblstatuspop.ForeColor = System.Drawing.Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                        return;
                    }
                }
                if (rbdnDaily.Checked == true || rbdnMonthly.Checked == true || rbdnWeekly.Checked == true)
                {
                    if (RadioButton2.Checked != true)
                    {
                        lblstatuspop.Text = "Please Select occurrence(s).";
                        lblstatuspop.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                        return;
                    }
                }
                if (rbdnDaily.Checked == true || rbdnMonthly.Checked == true || rbdnWeekly.Checked == true)
                {
                    if (RadioButton2.Checked == true)
                    {
                        if (Textbox31.Text == "")
                        {
                            lblstatuspop.Text = "Please Enter occurrence(s).";
                            lblstatuspop.ForeColor = Color.Red;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                            return;
                        }
                    }
                }
                if (rbdnDaily.Checked == true && RadioButton2.Checked == true)
                {
                    if (Convert.ToInt32(Textbox31.Text.Trim()) > 90)
                    {
                        lblstatuspop.Text = "Occurrence(s) are not greater than 90 for Daily.";
                        lblstatuspop.ForeColor = System.Drawing.Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                        return;

                    }
                }
                if (rbdnWeekly.Checked == true && RadioButton2.Checked == true)
                {
                    if (Convert.ToInt32(Textbox31.Text.Trim()) > 12)
                    {
                        lblstatuspop.Text = "Occurrence(s) are not greater than 12 for Weekly.";
                        lblstatuspop.ForeColor = System.Drawing.Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                        return;

                    }
                }
                if (rbdnMonthly.Checked == true && RadioButton2.Checked == true)
                {
                    if (Convert.ToInt32(Textbox31.Text.Trim()) > 3)
                    {
                        lblstatuspop.Text = "Occurrence(s) are not greater than 3 for Weekly.";
                        lblstatuspop.ForeColor = System.Drawing.Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                        return;

                    }
                }
                
                //if (txtFltFromDt.Text.Trim() != "" && TextBox6.Text.Trim() != "")
                //    txtfltdt.Text = "";
                if (rbdnDaily.Checked == true)
                {
                    if(RadioButton2.Checked ==true)
                    {
                        
                        for (j = 0; j < Convert.ToInt32(Textbox31.Text.Trim()); j++)
                        {
                            #region for daily booking
                            string[] paramname = new string[11];
                            paramname[0] = "SerialNumber";
                            paramname[1] = "TemplateID";
                            paramname[2] = "Pcs";
                            paramname[3] = "Wt";
                            paramname[4] = "Flg";
                            paramname[5] = "FltDt";
                            paramname[6] = "FltId";
                            paramname[7] = "Agent";
                            paramname[8] = "Station";
                            paramname[9] = "ExecutionDate";
                            paramname[10] = "HandlingInfo";

                            if (srno == "")
                            {
                                for (int i = 0; i < GRDBooking.Rows.Count; i++)
                                {
                                    if (((CheckBox)GRDBooking.Rows[i].FindControl("CHKSelect")).Checked == true)
                                    {
                                        srno = ((Label)GRDBooking.Rows[i].FindControl("lblsrno")).Text;
                                        tempid = ((TextBox)GRDBooking.Rows[i].FindControl("txtTemplateID")).Text;
                                        Agent = GRDBooking.Rows[i].Cells[4].Text;
                                       
                                    }

                                }
                            }
                            
                            string dtd = System.DateTime.Now.ToString("dd/MM/yyyy"); //dsResult.Tables[0].Rows[0]["ExecutionDate"].ToString();
                            try
                            {
                                DateTime dt = DateTime.Parse(Session["IT"].ToString());
                                dtd = dt.ToString("dd/MM/yyyy");
                            }
                            catch (Exception ex)
                            {
                                dtd = System.DateTime.Now.ToString("dd/MM/yyyy");
                            }

                            //string dtflt = Convert.ToDateTime(txtFromDate.Text).ToString("dd/MM/yyyy");
                            DateTime dtfltt = DateTime.ParseExact(txtFromDate.Text, "yyyy-MM-dd", null);//Convert.ToDateTime(dtflt);
                            DateTime dffltnew = dtfltt.AddDays(j);
                            object[] paramvalue = new object[11];
                            paramvalue[0] = srno;
                            paramvalue[1] = tempid;
                            paramvalue[2] = Convert.ToInt32(TextBox5.Text);
                            paramvalue[3] = float.Parse(txtweight.Text);
                            paramvalue[4] = "AD";
                            paramvalue[5] = dffltnew.ToString("dd/MM/yyyy");
                            paramvalue[6] = ddlFltID.SelectedItem.Text;
                            paramvalue[7] = Agent;
                            paramvalue[8] = txtorigin.Text.Trim();
                            paramvalue[9] = dtd;//DateTime.Parse(Session["IT"].ToString()).ToString;
                            paramvalue[10] = txthndlnginfo.Text;



                            SqlDbType[] paramtype = new SqlDbType[11];
                            paramtype[0] = SqlDbType.VarChar;
                            paramtype[1] = SqlDbType.VarChar;
                            paramtype[2] = SqlDbType.Int;
                            paramtype[3] = SqlDbType.Float;
                            paramtype[4] = SqlDbType.VarChar;
                            paramtype[5] = SqlDbType.VarChar;
                            paramtype[6] = SqlDbType.VarChar;
                            paramtype[7] = SqlDbType.VarChar;
                            paramtype[8] = SqlDbType.VarChar;
                            paramtype[9] = SqlDbType.VarChar;
                            paramtype[10] = SqlDbType.VarChar;

                            SQLServer sq = new SQLServer(Global.GetConnectionString());
                            DataSet ds = sq.SelectRecords("SP_SaveTemplateID", paramname, paramvalue, paramtype);
                            
                            if (ds != null)
                            {
                                if (ds.Tables != null)
                                {
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows[0][0].ToString() == "TRUE")
                                        {
                                            btnList_Click(null, null);
                                            awb = awb + "," +ds.Tables[1].Rows[0][0].ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);
                                            lblStatus.Text = "AWB(s):" + Textbox31.Text + " " + "are save Successfully.";
                                            lblStatus.ForeColor = System.Drawing.Color.Green;
                                            lblstatuspop.Text = "";

                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);
                                            lblStatus.Text = "Record not saved.";
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        }
                }
                else if (rbdnMonthly.Checked == true)
                {
                    if (RadioButton2.Checked == true)
                    {
                        for (j = 0; j < Convert.ToInt32(Textbox31.Text.Trim()); j++)
                        {
                            #region monthly booking

                            string[] paramname = new string[11];
                            paramname[0] = "SerialNumber";
                            paramname[1] = "TemplateID";
                            paramname[2] = "Pcs";
                            paramname[3] = "Wt";
                            paramname[4] = "Flg";
                            paramname[5] = "FltDt";
                            paramname[6] = "FltId";
                            paramname[7] = "Agent";
                            paramname[8] = "Station";
                            paramname[9] = "ExecutionDate";
                            paramname[10] = "HandlingInfo";

                            if (srno == "")
                            {
                                for (int i = 0; i < GRDBooking.Rows.Count; i++)
                                {
                                    if (((CheckBox)GRDBooking.Rows[i].FindControl("CHKSelect")).Checked == true)
                                    {
                                        srno = ((Label)GRDBooking.Rows[i].FindControl("lblsrno")).Text;
                                        tempid = ((TextBox)GRDBooking.Rows[i].FindControl("txtTemplateID")).Text;
                                        Agent = GRDBooking.Rows[i].Cells[4].Text;

                                    }

                                }
                            }

                            string dtd = System.DateTime.Now.ToString("dd/MM/yyyy"); //dsResult.Tables[0].Rows[0]["ExecutionDate"].ToString();
                            try
                            {
                                DateTime dt = DateTime.Parse(Session["IT"].ToString());
                                dtd = dt.ToString("dd/MM/yyyy");
                            }
                            catch (Exception ex)
                            {
                                dtd = System.DateTime.Now.ToString("dd/MM/yyyy");
                            }

                            //string dtflt = Convert.ToDateTime(txtFromDate.Text).ToString("dd/MM/yyyy");
                            DateTime dtfltt = DateTime.ParseExact(txtFromDate.Text, "yyyy-MM-dd", null);//Convert.ToDateTime(dtflt);
                            DateTime dffltnew = dtfltt.AddMonths(j);
                            object[] paramvalue = new object[11];
                            paramvalue[0] = srno;
                            paramvalue[1] = tempid;
                            paramvalue[2] = Convert.ToInt32(TextBox5.Text);
                            paramvalue[3] = float.Parse(txtweight.Text);
                            paramvalue[4] = "AD";
                            paramvalue[5] = dffltnew.ToString("dd/MM/yyyy");
                            paramvalue[6] = ddlFltID.SelectedItem.Text;
                            paramvalue[7] = Agent;
                            paramvalue[8] = txtorigin.Text.Trim();
                            paramvalue[9] = dtd;//DateTime.Parse(Session["IT"].ToString()).ToString;
                            paramvalue[10] = txthndlnginfo.Text;



                            SqlDbType[] paramtype = new SqlDbType[11];
                            paramtype[0] = SqlDbType.VarChar;
                            paramtype[1] = SqlDbType.VarChar;
                            paramtype[2] = SqlDbType.Int;
                            paramtype[3] = SqlDbType.Float;
                            paramtype[4] = SqlDbType.VarChar;
                            paramtype[5] = SqlDbType.VarChar;
                            paramtype[6] = SqlDbType.VarChar;
                            paramtype[7] = SqlDbType.VarChar;
                            paramtype[8] = SqlDbType.VarChar;
                            paramtype[9] = SqlDbType.VarChar;
                            paramtype[10] = SqlDbType.VarChar;

                            SQLServer sq = new SQLServer(Global.GetConnectionString());
                            DataSet ds = sq.SelectRecords("SP_SaveTemplateID", paramname, paramvalue, paramtype);

                            if (ds != null)
                            {
                                if (ds.Tables != null)
                                {
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows[0][0].ToString() == "TRUE")
                                        {
                                            btnList_Click(null, null);
                                            awb =awb+ "," + ds.Tables[1].Rows[0][0].ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);
                                            lblStatus.Text = "AWB(s):" + Textbox31.Text + " " + "are save Successfully.";
                                            lblStatus.ForeColor = System.Drawing.Color.Green;
                                            lblstatuspop.Text = "";

                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);
                                            lblStatus.Text = "Record not saved.";
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
                else if (rbdnWeekly.Checked == true)
                {
                    
                    if (RadioButton2.Checked == true)
                    {
                        
                        DateTime dffltnew = DateTime.ParseExact(txtFromDate.Text, "yyyy-MM-dd", null);//Convert.ToDateTime(dtflt);
                        for (j = 0; j < Convert.ToInt32(Textbox31.Text.Trim()); j++)
                        {
                            #region monthly booking

                            string[] paramname = new string[11];
                            paramname[0] = "SerialNumber";
                            paramname[1] = "TemplateID";
                            paramname[2] = "Pcs";
                            paramname[3] = "Wt";
                            paramname[4] = "Flg";
                            paramname[5] = "FltDt";
                            paramname[6] = "FltId";
                            paramname[7] = "Agent";
                            paramname[8] = "Station";
                            paramname[9] = "ExecutionDate";
                            paramname[10] = "HandlingInfo";

                            if (srno == "")
                            {
                                for (int i = 0; i < GRDBooking.Rows.Count; i++)
                                {
                                    if (((CheckBox)GRDBooking.Rows[i].FindControl("CHKSelect")).Checked == true)
                                    {
                                        srno = ((Label)GRDBooking.Rows[i].FindControl("lblsrno")).Text;
                                        tempid = ((TextBox)GRDBooking.Rows[i].FindControl("txtTemplateID")).Text;
                                        Agent = GRDBooking.Rows[i].Cells[4].Text;

                                    }

                                }
                            }

                            string dtd = System.DateTime.Now.ToString("dd/MM/yyyy"); //dsResult.Tables[0].Rows[0]["ExecutionDate"].ToString();
                            try
                            {
                                DateTime dt = DateTime.Parse(Session["IT"].ToString());
                                dtd = dt.ToString("dd/MM/yyyy");
                            }
                            catch (Exception ex)
                            {
                                dtd = System.DateTime.Now.ToString("dd/MM/yyyy");
                            }

                            
                            #region find out date from week days
                            if (rbdsun.Checked)
                            {
                                dffltnew = NextDate(dffltnew, DayOfWeek.Sunday);
                            }
                            if (rbdmon.Checked)
                            {
                                dffltnew = NextDate(dffltnew, DayOfWeek.Monday);
                            }
                            if (rbdtue.Checked)
                            {
                                dffltnew = NextDate(dffltnew, DayOfWeek.Tuesday);
                            }
                            if (rbdwed.Checked)
                            {
                                dffltnew = NextDate(dffltnew, DayOfWeek.Wednesday);
                            }
                            if (rbdthu.Checked)
                            {
                                dffltnew = NextDate(dffltnew, DayOfWeek.Thursday);
                            }
                            if (rbdfri.Checked)
                            {
                                dffltnew = NextDate(dffltnew, DayOfWeek.Friday);
                            }
                            if (rbdsat.Checked)
                            {
                                dffltnew = NextDate(dffltnew, DayOfWeek.Saturday);
                            }
                            //List<DateTime> dates = new List<DateTime>();
                            //int year = dffltnew.Year;
                            //int month = dffltnew.Month;
                            //int date = dffltnew.Day;
                            //DayOfWeek day = DayOfWeek.Sunday;
                            //System.Globalization.CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                            //for (int k = 1; k <= currentCulture.Calendar.GetDaysInMonth(year, month); k++)
                            //{
                            //    DateTime d = new DateTime(year, month, k);
                            //    if (d.DayOfWeek == day)
                            //    {
                            //        if (dffltnew <= d)
                            //        {
                            //            dates.Add(d);
                            //        }
                            //    }
                            //}

                            #endregion

                            
                            object[] paramvalue = new object[11];
                            paramvalue[0] = srno;
                            paramvalue[1] = tempid;
                            paramvalue[2] = Convert.ToInt32(TextBox5.Text);
                            paramvalue[3] = float.Parse(txtweight.Text);
                            paramvalue[4] = "AD";
                            paramvalue[5] = dffltnew.ToString("dd/MM/yyyy");
                            paramvalue[6] = ddlFltID.SelectedItem.Text;
                            paramvalue[7] = Agent;
                            paramvalue[8] = txtorigin.Text.Trim();
                            paramvalue[9] = dtd;//DateTime.Parse(Session["IT"].ToString()).ToString;
                            paramvalue[10] = txthndlnginfo.Text;



                            SqlDbType[] paramtype = new SqlDbType[11];
                            paramtype[0] = SqlDbType.VarChar;
                            paramtype[1] = SqlDbType.VarChar;
                            paramtype[2] = SqlDbType.Int;
                            paramtype[3] = SqlDbType.Float;
                            paramtype[4] = SqlDbType.VarChar;
                            paramtype[5] = SqlDbType.VarChar;
                            paramtype[6] = SqlDbType.VarChar;
                            paramtype[7] = SqlDbType.VarChar;
                            paramtype[8] = SqlDbType.VarChar;
                            paramtype[9] = SqlDbType.VarChar;
                            paramtype[10] = SqlDbType.VarChar;

                            SQLServer sq = new SQLServer(Global.GetConnectionString());
                            DataSet ds = sq.SelectRecords("SP_SaveTemplateID", paramname, paramvalue, paramtype);

                            if (ds != null)
                            {
                                if (ds.Tables != null)
                                {
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows[0][0].ToString() == "TRUE")
                                        {
                                            btnList_Click(null, null);
                                            awb = awb + "," + ds.Tables[1].Rows[0][0].ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);
                                            lblStatus.Text = "AWB(s):" + Textbox31.Text + " " + "are save Successfully.";
                                            lblStatus.ForeColor = System.Drawing.Color.Green;
                                            lblstatuspop.Text = "";

                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);
                                            lblStatus.Text = "Record not saved.";
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
                else
                {
                    #region for Single booking
                    string[] paramname = new string[11];
                    paramname[0] = "SerialNumber";
                    paramname[1] = "TemplateID";
                    paramname[2] = "Pcs";
                    paramname[3] = "Wt";
                    paramname[4] = "Flg";
                    paramname[5] = "FltDt";
                    paramname[6] = "FltId";
                    paramname[7] = "Agent";
                    paramname[8] = "Station";
                    paramname[9] = "ExecutionDate";
                    paramname[10] = "HandlingInfo";

                    //string tempid = "", srno = "", Agent = "";
                    for (int i = 0; i < GRDBooking.Rows.Count; i++)
                    {
                        if (((CheckBox)GRDBooking.Rows[i].FindControl("CHKSelect")).Checked == true)
                        {
                            srno = ((Label)GRDBooking.Rows[i].FindControl("lblsrno")).Text;
                            tempid = ((TextBox)GRDBooking.Rows[i].FindControl("txtTemplateID")).Text;
                            Agent = GRDBooking.Rows[i].Cells[4].Text;
                        }
                    }

                    string dtd = System.DateTime.Now.ToString("dd/MM/yyyy"); //dsResult.Tables[0].Rows[0]["ExecutionDate"].ToString();
                    try
                    {
                        DateTime dt = DateTime.Parse(Session["IT"].ToString());
                        dtd = dt.ToString("dd/MM/yyyy");
                    }
                    catch (Exception ex)
                    {
                        dtd = System.DateTime.Now.ToString("dd/MM/yyyy");
                    }


                    object[] paramvalue = new object[11];
                    paramvalue[0] = srno;
                    paramvalue[1] = tempid;
                    paramvalue[2] = Convert.ToInt32(TextBox5.Text);
                    paramvalue[3] = float.Parse(txtweight.Text);
                    paramvalue[4] = "AD";
                    if (txtfltdt.Text.Trim() != "")
                    {
                        paramvalue[5] = txtfltdt.Text.Trim();
                    }
                    else
                    {
                        paramvalue[5] = dtd;
                    }
                    paramvalue[6] = fltID.SelectedItem.Text;
                    paramvalue[7] = Agent;
                    paramvalue[8] = txtorigin.Text.Trim();
                    paramvalue[9] = dtd;//DateTime.Parse(Session["IT"].ToString()).ToString;
                    paramvalue[10] = txthndlnginfo.Text;



                    SqlDbType[] paramtype = new SqlDbType[11];
                    paramtype[0] = SqlDbType.VarChar;
                    paramtype[1] = SqlDbType.VarChar;
                    paramtype[2] = SqlDbType.Int;
                    paramtype[3] = SqlDbType.Float;
                    paramtype[4] = SqlDbType.VarChar;
                    paramtype[5] = SqlDbType.VarChar;
                    paramtype[6] = SqlDbType.VarChar;
                    paramtype[7] = SqlDbType.VarChar;
                    paramtype[8] = SqlDbType.VarChar;
                    paramtype[9] = SqlDbType.VarChar;
                    paramtype[10] = SqlDbType.VarChar;

                    SQLServer sq = new SQLServer(Global.GetConnectionString());
                    DataSet ds = sq.SelectRecords("SP_SaveTemplateID", paramname, paramvalue, paramtype);

                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0][0].ToString() == "TRUE")
                                {
                                    btnList_Click(null, null);
                                    awb = ds.Tables[1].Rows[0][0].ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);
                                    lblStatus.Text = "AWB:" + awb + " " + "Save Successfully.";
                                    lblStatus.ForeColor = System.Drawing.Color.Green;
                                    lblstatuspop.Text = "";

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);
                                    lblStatus.Text = "Record not saved.";
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
        }
        #region GetFlightRouteData
        protected void txtfltdt_TextChanged(object sender, EventArgs e)
        {
            DataSet dsresult = null;
            try
            {
                string strPartnerCode = txtPartnercode.Text.Trim();//((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartner")).Text.Trim();

                string errormessage = "";
                // DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);
                dsresult = GetFlightList(txtorigin.Text.ToUpper(), txtdest.Text.ToUpper(), txtfltdt.Text, 0, 0, 0, ref errormessage, strPartnerCode);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    DataSet ds = (DataSet)Session["Flt"];
                    if (ds != null)
                    {
                        string name = "Table" +0;
                        try
                        {
                            if (ds.Tables.Count > 0)
                            {
                                try
                                {
                                    if (ds.Tables[name] != null && ds.Tables[name].Rows.Count > 0)
                                    {
                                        ds.Tables.Remove(name);
                                        DataTable dt = new DataTable();
                                        dt = dsresult.Tables[0].Copy();
                                        dt.TableName = name;
                                        ds.Tables.Add(dt);
                                        ds.AcceptChanges();
                                        Session["Flt"] = ds.Copy();
                                    }
                                }
                                catch (Exception ex) { }

                            }
                            else if (ds.Tables.Count == 1)
                            {
                                Session["Flt"] = dsresult.Copy();
                            }
                        }
                        catch (Exception ex) { }

                    }
                    else
                    {
                        Session["Flt"] = dsresult.Copy();
                    }
                    //DataRow row = dsresult.Tables[0].NewRow();
                    //if (Convert.ToBoolean(Session["FRV"]))
                    //{
                    //    row["FltNumber"] = "Select";
                    //    row["ArrTime"] = "Select";
                    //}
                    //else
                    //{
                    //    row["FltNumber"] = " ";
                    //    row["ArrTime"] = " ";
                    //}

                    //dsresult.Tables[0].Rows.Add(row);

                    fltID.DataTextField = "FltNumber";
                    fltID.DataValueField = "ArrTime";
                    fltID.DataSource = dsresult.Tables[0].Copy();
                    fltID.DataBind();
                    //fltID.Items.Add("Select");
                    fltID.Items.Insert(0,new ListItem("Select","Select"));
                    //fltID.SelectedItem.Text = "Select";
                    //fltID.SelectedIndex = dsresult.Tables[0].Rows.Count - 1;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                    //HidProcessFlag.Value = "1";
                }
                else
                {
                    lblStatus.Text = "no record found";
                    fltID.Items.Clear();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    return;
                }

            }
            catch (Exception ex)
            {
                dsresult = null;
            }
            finally
            {
                if (dsresult != null)
                    dsresult.Dispose();
            }

        }
        #endregion

        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage, string PartnerCode)
        {
            DataSet dsResult = new DataSet();
            bool blnSelfAirline = false;
            DataSet dsAWBPrefixs = CommonUtility.AWBPrefixMaster;

            if (PartnerCode != "")
            {
                if (dsAWBPrefixs != null && dsAWBPrefixs.Tables.Count > 0 && dsAWBPrefixs.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < dsAWBPrefixs.Tables[0].Rows.Count; intCount++)
                    {
                        if (PartnerCode.ToUpper() == Convert.ToString(dsAWBPrefixs.Tables[0].Rows[intCount]["AirlinePrefix"]).ToUpper())
                        {
                            blnSelfAirline = true;
                            dsAWBPrefixs = null;
                            break;
                        }
                    }
                }
            }

            if (strdate.Trim() == "")
            {
                if (blnSelfAirline)
                {
                    if (new ShowFlightsBAL().GetFlightListforDay(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (new ShowFlightsBAL().GetPartnerFlightList(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {

                string[] splitdate = strdate.Split(new char[] { '/' });
                int year = int.Parse(splitdate[2]);
                int month = int.Parse(splitdate[1]);
                int day = int.Parse(splitdate[0]);
                DateTime dt = new DateTime(year, month, day);

                int diff = (dt - dtCurrentDate.Date).Days;

                if (blnSelfAirline)
                {
                    if (new ShowFlightsBAL().GetFlightListforDay(Origin, Dest, ref dsResult, ref errormessage, dt, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (new ShowFlightsBAL().GetPartnerFlightList(Origin, Dest, ref dsResult, ref errormessage, dt, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }
        public void FormatRecords(string org, string dest, ref DataSet dsResult, int PrevHr, int PrevMin, int AllowedHr)
        {
            int i = 0;
            string ScheduleID = "";
            DataSet dsNewResult = dsResult.Clone();
            bool blOrignFlound, blDestFound;
            blOrignFlound = blDestFound = false;

            foreach (DataRow row in dsResult.Tables[0].Rows)
            {
                if (ScheduleID == "")
                {
                    if (row["FltOrigin"].ToString() != org)
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                    }

                    ScheduleID = row["ScheduleID"].ToString();
                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == dest)
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);

                }
                else if (ScheduleID.Trim() == row["ScheduleID"].ToString())
                {
                    if (!blDestFound)
                    {
                        dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["FltDestination"] = row["FltDestination"].ToString();
                        dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["ArrTime"] = row["ArrTime"].ToString();

                        if (row["FltDestination"].ToString() == dest)
                        {
                            blDestFound = true;
                        }

                    }

                }
                else
                {
                    if (row["FltOrigin"].ToString() != org)
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                        blDestFound = false;
                    }

                    ScheduleID = row["ScheduleID"].ToString();


                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == dest)
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);
                }

                i++;

            }

            dsResult = dsNewResult.Copy();
            DataView dv = new DataView(dsResult.Tables[0].Copy());
            dv.Sort = "DeptTime";

            dsResult = new DataSet();
            dsResult.Tables.Add(dv.ToTable().Copy());


            
            DataTable dt = dsResult.Tables[0].Clone();
            foreach (DataRow row in dsResult.Tables[0].Rows)
            {
                string depttime = row["DeptTime"].ToString();
                int hr = int.Parse(depttime.Substring(0, depttime.IndexOf(":")));
                int min = int.Parse(depttime.Substring(depttime.IndexOf(":") + 1));

                string[] strDate = row["FltDate"].ToString().Split('/');
                int intFltDate = int.Parse(strDate[0]);
                int intCurrentDt = dtCurrentDate.Day;

                bool canAdd = true;
                

                if (canAdd)
                {
                    DataRow rw = dt.NewRow();

                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        rw[k] = row[k];
                    }

                    dt.Rows.Add(rw);
                }
            }

            dsResult = new DataSet();
            dsResult.Tables.Add(dt);

            try
            {
                if (dsNewResult != null)
                    dsNewResult.Dispose();
                if (dt != null)
                    dt.Dispose();
            }
            catch (Exception ex)
            {
                dt = null;
                dsNewResult = null;
            }
        }
        protected void ddlFltID_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dssession = (DataSet)Session["FltID"];
            try
            {
                string flltid = ddlFltID.SelectedItem.Text.Trim();
                string starr = dssession.Tables[0].Rows[0]["Frequency"].ToString();
                string[] frq = starr.Split(',');
                if (frq[0] == "1")
                {
                    chkmonday.Checked = true;
                }
                else
                {
                    chkmonday.Enabled = false;
                }
                if (frq[1] == "1")
                {
                    ChkTuesday.Checked = true;
                }
                else
                {
                    ChkTuesday.Enabled = false;
                }
                if (frq[2] == "1")
                {
                    ChkWednesday.Checked = true;
                }
                else
                {
                    ChkWednesday.Enabled = false;
                }
                if (frq[3] == "1")
                {
                    ChkThursday.Checked = true;
                }
                else
                {
                    ChkThursday.Enabled = false;
                }
                if (frq[4] == "1")
                {
                    ChkFriday.Checked = true;
                }
                else
                {
                    ChkFriday.Enabled = false;
                }
                if (frq[5] == "1")
                {
                    ChkSaturday.Checked = true;
                }
                else
                {
                    ChkSaturday.Enabled = false;
                }
                if (frq[6] == "1")
                {
                    chksunday.Checked = true;
                }
                else
                {
                    chksunday.Enabled = false;
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
            }
            
            catch(Exception ex)
            {}
        }
        protected void rbdnDaily_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbdnDaily.Checked == true)
                {
                    chkmonday.Enabled = false;
                    ChkTuesday.Enabled = false;
                    ChkWednesday.Enabled = false;
                    ChkThursday.Enabled = false;
                    ChkFriday.Enabled = false;
                    ChkSaturday.Enabled = false;
                    chksunday.Enabled = false;

                    rbdsun.Visible = false;
                    rbdmon.Visible = false;
                    rbdtue.Visible = false;
                    rbdwed.Visible = false;
                    rbdthu.Visible = false;
                    rbdfri.Visible = false;
                    rbdsat.Visible = false;
                    RadioButton2.Checked = true;

                    chkmonday.Visible = true;
                    ChkTuesday.Visible = true;
                    ChkWednesday.Visible = true;
                    ChkThursday.Visible = true;
                    ChkFriday.Visible = true;
                    ChkSaturday.Visible = true;
                    chksunday.Visible = true;
                    RadioButton3.Enabled = false;
                    TextBox6.Enabled = false;
                    //CalendarExtender3.Enabled = false;

                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
            }
            catch (Exception ex)
            { }
        }
        protected void rbdnMonthly_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdnMonthly.Checked == true)
            {
                chkmonday.Enabled = false;
                ChkTuesday.Enabled = false;
                ChkWednesday.Enabled = false;
                ChkThursday.Enabled = false;
                ChkFriday.Enabled = false;
                ChkSaturday.Enabled = false;
                chksunday.Enabled = false;

                chkmonday.Checked = false;
                ChkTuesday.Checked = false;
                ChkWednesday.Checked = false;
                ChkThursday.Checked = false;
                ChkFriday.Checked = false;
                ChkSaturday.Checked = false;
                chksunday.Checked = false;
                RadioButton2.Checked = true;
                TextBox6.Enabled = false;
                RadioButton3.Enabled = false;

                rbdsun.Visible = false;
                rbdmon.Visible = false;
                rbdtue.Visible = false;
                rbdwed.Visible = false;
                rbdthu.Visible = false;
                rbdfri.Visible = false;
                rbdsat.Visible = false;

                chkmonday.Visible = true;
                ChkTuesday.Visible = true;
                ChkWednesday.Visible = true;
                ChkThursday.Visible = true;
                ChkFriday.Visible = true;
                ChkSaturday.Visible = true;
                chksunday.Visible = true;

                RadioButton3.Enabled = false;
                TextBox6.Enabled = false;
                //CalendarExtender3.Enabled = false;


            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
        }
        protected void rbdnWeekly_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdnWeekly.Checked == true)
            {
                chkmonday.Enabled = false;
                ChkTuesday.Enabled = false;
                ChkWednesday.Enabled = false;
                ChkThursday.Enabled = false;
                ChkFriday.Enabled = false;
                ChkSaturday.Enabled = false;
                chksunday.Enabled = false;

                rbdsun.Visible = true;
                rbdmon.Visible = true;
                rbdtue.Visible = true;
                rbdwed.Visible = true;
                rbdthu.Visible = true;
                rbdfri.Visible = true;
                rbdsat.Visible = true;

                chkmonday.Visible = false;
                ChkTuesday.Visible = false;
                ChkWednesday.Visible = false;
                ChkThursday.Visible = false;
                ChkFriday.Visible = false;
                ChkSaturday.Visible = false;
                chksunday.Visible = false;

                RadioButton3.Enabled = false;
                TextBox6.Enabled = false;
                //CalendarExtender3.Enabled = false;

            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);

        }
        public DateTime NextDate(DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCodeWithName(string prefixText, int count)
        {
            try
            {
                string[] orgdest = new ListTemplate_GHA().GetOrgDest();

                string con = Global.GetConnectionString();
                //// SqlConnection con = new SqlConnection("connection string"); 
                SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode + '(' + AgentName + ')' from dbo.AgentMaster where (AgentName like '%" + prefixText + "%' or AgentCode like '%" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
                DataSet ds = new DataSet();
                dad.Fill(ds);

                //DataSet ds = new DataSet();
                //AgentBAL bal = new AgentBAL();
                //ds = bal.GetAgentList(prefixText, orgdest[0], System.DateTime.Now.ToString("dd/MM/yyyy"));
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
        public string[] GetOrgDest()
        {
            string[] result = { "", "" };
            if (Session["Station"] != null)
            {
                result[0] = Session["Station"].ToString();
            }
            if (Session["Destination"] != null)
            {
                result[1] = Session["Destination"].ToString();
            }

            return result;
        }
    }
}
