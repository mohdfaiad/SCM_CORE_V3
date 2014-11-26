  using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using Microsoft.Reporting.WebForms;
using BAL;
using System.Data.SqlClient;

namespace ProjectSmartCargoManager
{
    public partial class DwellTimeConfig : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        protected void Page_Load(object sender, EventArgs e)
        {
            
            DataSet ds   = new DataSet();
            try
            {
                

                if (!IsPostBack)
                {
                    btnSave.Visible = true;
                    btnAdd.Visible = true;
                    btnDel.Visible = true;
                    btnSave.Enabled = true;
                    btnAdd.Enabled = true;
                    btnDel.Enabled = true;
                    grdCommCat.DataSource = null;
                    grdCommCat.DataBind();
                    Session["dsVal"] = null;
                    
                   ds = da.SelectRecords("SPGetCommcategory");
                   ddlstation.DataSource = ds.Tables[0];
                   ddlstation.DataTextField = "Airport";
                   ddlstation.DataValueField = "AirportCode";
                   ddlstation.DataBind();
                   ddlstation.Items.Insert(0, "Select");
                   

                    ddlCC.DataSource=ds.Tables[1];
                    ddlCC.DataTextField = "CommCategory";
                    ddlCC.DataValueField = "SerialNumber";
                    ddlCC.DataBind();
                    ddlCC.Items.Insert(0, "Select");
                    Session["DwellAirport"] = ds.Tables[0];
                    Session["DwellCommcategory"] = ds.Tables[1];

                    if (grdCommCat.Rows.Count == 0)
                    {
                        btnAdd_row(null, null);
                        btnSave.Visible = true;
                        btnAdd.Visible = true;
                        btnDel.Visible = true;
                        btnSave.Enabled = true;
                        btnAdd.Enabled = true;
                        btnDel.Enabled = true;
                        return;
                    }

                    btnSave.Visible = true;
                    btnAdd.Visible = true;
                    btnDel.Visible = true;
                    btnSave.Enabled = true;
                    btnAdd.Enabled = true;
                    btnDel.Enabled = true;

                }
            }
            catch (Exception ex)
            {
            }
        }

        public void MasterLog(string value)
        {
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            #region for Master Audit Log
            #region Prepare Parameters
            object[] Paramsmaster = new object[7];
            int count = 0;
     
            //1

            Paramsmaster.SetValue("Dwell-Time Config", count);
            count++;

            //2

            string data = null;

            for (int j = 0; j < grdCommCat.Rows.Count; j++)
            {
                if (((CheckBox)(grdCommCat.Rows[j].FindControl("checkCC"))).Checked == true)
                {
                    data = (((DropDownList)(grdCommCat.Rows[j].FindControl("grdddlCommCat"))).SelectedItem.Text.ToString()) + '-' + (((TextBox)(grdCommCat.Rows[j].FindControl("grdCommCode"))).Text.ToString());

                    Paramsmaster.SetValue(data, count);
                }
                Paramsmaster.SetValue("", count);
            }

         
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

        #region List Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet dsVal = null;
            string[] paramname = new string[3];
            object[] paramvalue = new object[3];
            SqlDbType[] paramtype = new SqlDbType[3];
            try
            {

                grdCommCat.DataSource = null;
                grdCommCat.DataBind();

                paramname[0] = "Station";
                paramname[1] = "CommCat";
                paramname[2] = "CommCode";

                paramvalue[0] = ddlstation.SelectedItem.Text.Trim();
                paramvalue[1] = ddlCC.SelectedValue.ToString();
                paramvalue[2] = txtCommCode.Text.Trim();
                
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;

                dsVal = da.SelectRecords("SPGetDwellTime", paramname, paramvalue, paramtype);
                Session["dsVal"] = dsVal.Tables[0];
                if (dsVal != null && dsVal.Tables.Count > 0)
                {
                    if (dsVal.Tables[0].Rows.Count > 0)
                    {
                        grdCommCat.DataSource = dsVal.Tables[0];
                        grdCommCat.DataBind();
                        lblStatus.Text = "";
                        for (int i = 0; i < grdCommCat.Rows.Count; i++)
                        {
                            DropDownList ddlcommcat = (DropDownList)grdCommCat.Rows[i].FindControl("grdddlStation");

                            ddlcommcat.DataSource = (DataTable)Session["DwellAirport"];
                            ddlcommcat.DataTextField = "Airport";
                            ddlcommcat.DataValueField = "AirportCode";
                            ddlcommcat.DataBind();
                            ddlcommcat.Items.Insert(0, "Select");

                            ddlcommcat.SelectedIndex = ddlcommcat.Items.IndexOf(ddlcommcat.Items.FindByText(dsVal.Tables[0].Rows[i]["StationCode"].ToString()));
                            if (ddlcommcat.SelectedItem.Text != "Select")
                                ddlcommcat.Enabled = false;
                            DropDownList ddlcommcategory = (DropDownList)grdCommCat.Rows[i].FindControl("grdddlCommCat");

                            ddlcommcategory.DataSource = (DataTable)Session["DwellCommcategory"];
                            ddlcommcategory.DataTextField = "CommCategory";
                            ddlcommcategory.DataValueField = "SerialNumber";
                            ddlcommcategory.DataBind();
                            ddlcommcategory.Items.Insert(0, "Select");

                            ddlcommcategory.SelectedIndex = ddlcommcategory.Items.IndexOf(ddlcommcategory.Items.FindByText(dsVal.Tables[0].Rows[i]["CommCategory"].ToString()));
                            if (ddlcommcategory.SelectedItem.Text != "Select")
                            {
                                ddlcommcategory.Enabled = false;
                                ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Enabled = false;
                            }
                            if (((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Text.Length > 0)
                                ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Enabled = false;

                            btnSave.Visible = true;
                            btnAdd.Visible = true;
                            btnDel.Visible = true;
                            btnSave.Enabled = true;
                            btnAdd.Enabled = true;
                            btnDel.Enabled = true;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No Record Found.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        if (grdCommCat.Rows.Count == 0)
                        {
                            btnAdd_Click(null, null);
                        }
                        btnSave.Visible = true;
                        btnAdd.Visible = true;
                        btnDel.Visible = true;
                        btnSave.Enabled = true;
                        btnAdd.Enabled = true;
                        btnDel.Enabled = true;
                    }

                }
                else
                {
                    lblStatus.Text = "Data Not Found.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    if (grdCommCat.Rows.Count == 0)
                    {
                        btnAdd_Click(null, null);
                    }
                    btnSave.Visible = true;
                    btnAdd.Visible = true;
                    btnDel.Visible = true;
                    btnSave.Enabled = true;
                    btnAdd.Enabled = true;
                    btnDel.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (dsVal != null)
                {
                    dsVal.Dispose();
                }
                paramname = null;
                paramtype = null;
                paramvalue = null;
            }
        }
        #endregion

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("DwellTimeConfig.aspx");
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //Session["dsVal"] = null;
            DataTable dst = new DataTable();
                dst=(DataTable)Session["dsVal"];
                DataRow Datarow = dst.NewRow();
                Datarow["CommCategorySrNo"] = "0";

                Datarow["CommCategory"] = "";

                Datarow["StationCode"] = "";
                Datarow["DwellTimeDays"] = "0";
                Datarow["CommCode"] = "";
                Datarow["HOURS"] = "0";
                Datarow["EmailRecipients"] = "";



                dst.Rows.Add(Datarow);

                grdCommCat.DataSource = dst;
                grdCommCat.DataBind();
                Session["dsVal"] = dst;
          if(dst!=null)
            {
              if (dst.Rows.Count > 0)
              {
            for(int i=0;i<dst.Rows.Count;i++)



            {


                DropDownList ddlcommcat = (DropDownList)grdCommCat.Rows[i].FindControl("grdddlStation");

                ddlcommcat.DataSource = (DataTable)Session["DwellAirport"];
                ddlcommcat.DataTextField = "Airport";
                ddlcommcat.DataValueField = "AirportCode";
                ddlcommcat.DataBind();
                ddlcommcat.Items.Insert(0, "Select");

                ddlcommcat.SelectedIndex = ddlcommcat.Items.IndexOf(ddlcommcat.Items.FindByText(dst.Rows[i]["StationCode"].ToString()));
                if (ddlcommcat.SelectedItem.Text != "Select")
                    ddlcommcat.Enabled = false;
                DropDownList ddlcommcategory = (DropDownList)grdCommCat.Rows[i].FindControl("grdddlCommCat");

                ddlcommcategory.DataSource = (DataTable)Session["DwellCommcategory"];
                ddlcommcategory.DataTextField = "CommCategory";
                ddlcommcategory.DataValueField = "SerialNumber";
                ddlcommcategory.DataBind();
                ddlcommcategory.Items.Insert(0, "Select");

                ddlcommcategory.SelectedIndex = ddlcommcategory.Items.IndexOf(ddlcommcategory.Items.FindByText(dst.Rows[i]["CommCategory"].ToString()));

            }
            }
        }


        }

        protected void btnAdd_row(object sender, EventArgs e)
        {
            DataTable dtNewList = (DataTable)Session["dsVal"];
            if (dtNewList == null)
            {
                dtNewList = new DataTable();
                dtNewList.Columns.Add("CommCategorySrNo");
                dtNewList.Columns.Add("CommCategory");
                dtNewList.Columns.Add("StationCode");
                dtNewList.Columns.Add("DwellTimeDays");
                dtNewList.Columns.Add("CommCode");
                dtNewList.Columns.Add("HOURS");
                dtNewList.Columns.Add("EmailRecipients");

            }
            //DataSet dtNewList = null;
            lblStatus.Text = "";
            try
            {
                DataRow l_Datarow = dtNewList.NewRow();
                l_Datarow["CommCategorySrNo"] = "0";

                l_Datarow["CommCategory"] = "";

                l_Datarow["StationCode"] = "";
                l_Datarow["DwellTimeDays"] = "0";
                l_Datarow["CommCode"] = "";
                l_Datarow["HOURS"] = "0";
                l_Datarow["EmailRecipients"] = "";
                


                dtNewList.Rows.Add(l_Datarow);

                grdCommCat.DataSource = dtNewList;
                grdCommCat.DataBind();
                Session["dsVal"] = dtNewList;
                string value = "ADD";
                MasterLog(value);
                if (dtNewList != null)
                {
                    if (dtNewList.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtNewList.Rows.Count; i++)
                        {
                           

                            
                                DropDownList ddlcommcat = (DropDownList)grdCommCat.Rows[i].FindControl("grdddlStation");

                                ddlcommcat.DataSource = (DataTable)Session["DwellAirport"];
                                ddlcommcat.DataTextField = "Airport";
                                ddlcommcat.DataValueField = "AirportCode";
                                ddlcommcat.DataBind();
                                ddlcommcat.Items.Insert(0, "Select");

                                ddlcommcat.SelectedIndex = ddlcommcat.Items.IndexOf(ddlcommcat.Items.FindByText(dtNewList.Rows[i]["StationCode"].ToString()));
                                if (ddlcommcat.SelectedItem.Text != "Select")
                                    ddlcommcat.Enabled = false;
                                DropDownList ddlcommcategory = (DropDownList)grdCommCat.Rows[i].FindControl("grdddlCommCat");

                                ddlcommcategory.DataSource = (DataTable)Session["DwellCommcategory"];
                                ddlcommcategory.DataTextField = "CommCategory";
                                ddlcommcategory.DataValueField = "SerialNumber";
                                ddlcommcategory.DataBind();
                                ddlcommcategory.Items.Insert(0, "Select");

                                ddlcommcategory.SelectedIndex = ddlcommcategory.Items.IndexOf(ddlcommcategory.Items.FindByText(dtNewList.Rows[i]["CommCategory"].ToString()));

                                if (ddlcommcategory.SelectedItem.Text != "Select")
                                {
                                    ddlcommcategory.Enabled = false;
                                    ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Enabled = false;
                                }
                                if (((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Text.Length > 0)
                                    ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Enabled = false;

                        }
                

                    }

                }
                btnSave.Enabled = true;
                btnAdd.Enabled = false;
                btnDel.Enabled = false;
            }
            catch { }
            finally
            {
                if (dtNewList != null)
                    dtNewList.Dispose();
                
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataSet dsSAVE = null;
            string[] paramnamesave = new string[6];
            object[] paramvaluesave = new object[6];
            SqlDbType[] paramtypesave = new SqlDbType[6];
            try
            {
                bool chkselect = false;
                for (int j = 0; j < grdCommCat.Rows.Count; j++)
                {
                    
                    if (((CheckBox)grdCommCat.Rows[j].FindControl("checkCC")).Checked == true)
                    {
                        chkselect = true;
                        if (((DropDownList)grdCommCat.Rows[j].FindControl("grdddlStation")).SelectedItem.Text == "Select")
                        {
                            lblStatus.Text = "Please select Station.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        //if (((DropDownList)grdCommCat.Rows[j].FindControl("grdddlCommCat")).SelectedItem.Text == "Select")
                        //{
                        //    //lblStatus.Text = "Please select Commodity Category.";
                        //    lblStatus.ForeColor = System.Drawing.Color.Red;
                        //    return;
                        //}
                        if (((TextBox)grdCommCat.Rows[j].FindControl("grdDwellDays")).Text == "")
                        {
                            lblStatus.Text = "Please Enter Dwell Time Days.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        int chkint = 0;
                        string dwelldays = ((TextBox)grdCommCat.Rows[j].FindControl("grdDwellDays")).Text;
                        if (!int.TryParse(dwelldays, out chkint))
                        {
                            lblStatus.Text = "Please Enter numeric value for Dwell Time Days.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        if (chkint == 0)
                        {
                            lblStatus.Text = "Please Enter Dwell Days greater than 0.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return;
                        }

                        int chkhrs = 0;
                        string dwellhours = ((TextBox)grdCommCat.Rows[j].FindControl("grdtxtHours")).Text;
                        if (!int.TryParse(dwellhours, out chkhrs))
                        {
                            lblStatus.Text = "Please enter numeric value for Hours.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        if (((TextBox)grdCommCat.Rows[j].FindControl("grdtxtEmail")).Text == "")
                        {
                            lblStatus.Text = "Please Enter Email Ids.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }
                if (chkselect != true)
                {
                    lblStatus.Text = "Please select Record(s).";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                for (int i = 0; i < grdCommCat.Rows.Count; i++)
                {
                    if (((CheckBox)grdCommCat.Rows[i].FindControl("checkCC")).Checked == true)
                    {
                        paramnamesave[0] = "Station";
                        paramnamesave[1] = "CommCat";
                        paramnamesave[2] = "CommCode";
                        paramnamesave[3] = "DwellTimeDays";
                        paramnamesave[4] = "Hours";
                        paramnamesave[5] = "Email";

                        paramvaluesave[0] = ((DropDownList)grdCommCat.Rows[i].FindControl("grdddlStation")).SelectedItem.Text;
                        if (((DropDownList)grdCommCat.Rows[i].FindControl("grdddlCommCat")).SelectedValue == "Select")
                        {
                            paramvaluesave[1] = 0;
                            //paramvaluesave[1]..SetValue("", i);
                        }
                        else
                        {
                            paramvaluesave[1] = Convert.ToInt32(((DropDownList)grdCommCat.Rows[i].FindControl("grdddlCommCat")).SelectedValue);
                        }
                        paramvaluesave[2] = ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Text;
                        paramvaluesave[3] = Convert.ToInt32(((TextBox)grdCommCat.Rows[i].FindControl("grdDwellDays")).Text);
                        paramvaluesave[4] = Convert.ToInt32(((TextBox)grdCommCat.Rows[i].FindControl("grdtxtHours")).Text);
                        paramvaluesave[5] = ((TextBox)grdCommCat.Rows[i].FindControl("grdtxtEmail")).Text;


                        paramtypesave[0] = SqlDbType.VarChar;
                        paramtypesave[1] = SqlDbType.Int;
                        paramtypesave[2] = SqlDbType.VarChar;
                        paramtypesave[3] = SqlDbType.Int;
                        paramtypesave[4] = SqlDbType.Int;
                        paramtypesave[5] = SqlDbType.VarChar;

                        dsSAVE = da.SelectRecords("SPDwellTimeDaysSave", paramnamesave, paramvaluesave, paramtypesave);
                        string value = "SAVE";
                        MasterLog(value);

                        btnList_Click(null, null);
                        if (dsSAVE != null && dsSAVE.Tables.Count > 0)
                        {
                            if (dsSAVE.Tables[0].Rows.Count > 0)
                            {
                                if ((dsSAVE.Tables[0].Rows[0][0].ToString()) == "TRUE")
                                {
                                    lblStatus.Text = "Record Saved Successfully...";
                                    lblStatus.ForeColor = System.Drawing.Color.Green;
                                    
                                }
                            }
                        }
                        
                    }
                }
                
            }
            catch (Exception ex)
            { }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            DataSet dsDel = null;
            string[] paramnamesave = new string[3];
            object[] paramvaluesave = new object[3];
            SqlDbType[] paramtypesave = new SqlDbType[3];
            try
            {
                bool chkdel = false;
                for (int i = 0; i < grdCommCat.Rows.Count; i++)
                {
                    
                    if (((CheckBox)grdCommCat.Rows[i].FindControl("checkCC")).Checked == true)
                    {
                        chkdel = true;
                        paramnamesave[0] = "Station";
                        paramnamesave[1] = "CommCat";
                        paramnamesave[2] = "CommCode";
                        //paramnamesave[3] = "DwellTimeDays";
                        //paramnamesave[4] = "Hours";
                        //paramnamesave[5] = "Email";

                        paramvaluesave[0] = ((DropDownList)grdCommCat.Rows[i].FindControl("grdddlStation")).SelectedItem.Text;
                        paramvaluesave[1] = Convert.ToInt32(((DropDownList)grdCommCat.Rows[i].FindControl("grdddlCommCat")).SelectedValue);
                        paramvaluesave[2] = ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Text;
                        //paramvaluesave[3] = Convert.ToInt32(((TextBox)grdCommCat.Rows[i].FindControl("grdDwellDays")).Text);
                        //paramvaluesave[4] = Convert.ToInt32(((TextBox)grdCommCat.Rows[i].FindControl("grdtxtHours")).Text);
                        //paramvaluesave[5] = ((TextBox)grdCommCat.Rows[i].FindControl("grdtxtEmail")).Text;


                        paramtypesave[0] = SqlDbType.VarChar;
                        paramtypesave[1] = SqlDbType.Int;
                        paramtypesave[2] = SqlDbType.VarChar;


                        dsDel = da.SelectRecords("SPDwellTimeDaysDel", paramnamesave, paramvaluesave, paramtypesave);
                        string value = "DELETE";
                        MasterLog(value);


                        if (dsDel != null && dsDel.Tables.Count > 0)
                        {
                            if (dsDel.Tables[0].Rows.Count > 0)
                            {
                                if ((dsDel.Tables[0].Rows[0][0].ToString()) == "TRUE")
                                {
                                    btnList_Click(null, null);
                                    lblStatus.Text = "Record Deleted Successfully...";
                                }
                            }
                        }

                    }
                }
                if (chkdel != true)
                {
                    lblStatus.Text = "Please select Record(s).";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                
            }
            catch (Exception ex)
            { }
        }

        //#region newMethod
        //[System.Web.Services.WebMethod]
        //[System.Web.Script.Services.ScriptMethod]
        //public static string[] GetCommCode(string prefixText, int count)
        //{
        //    string con = Global.GetConnectionString();

        //    SqlDataAdapter dad = new SqlDataAdapter("SELECT Description  +'   ('+ CommodityCode +')' as CommCode from CommodityMaster where Description like '" + prefixText + "%' or CommodityCode like '" + prefixText + "%'", con);
        //    DataSet ds = new DataSet();
        //    dad.Fill(ds);
        //    List<string> list = new List<string>(ds.Tables[0].Rows.Count);
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        list.Add(dr[0].ToString());

        //    }

        //    return list.ToArray();
        //}

        //#endregion
        //[System.Web.Script.Services.ScriptMethod()]
        //[System.Web.Services.WebMethod]
        //public static string[] GetCommodityCodesWithName(string prefixText, int count)
        //{
        //    string con = Global.GetConnectionString();
        //    // SqlConnection con = new SqlConnection("connection string"); 
        //    SqlDataAdapter dad = new SqlDataAdapter("SELECT CommodityCode + '(' + Description + ')' from CommodityMaster where (Description like '%" + prefixText + "%' or CommodityCode like '%" + prefixText + "%')", con);
        //    DataSet ds = new DataSet();
        //    dad.Fill(ds);
        //    List<string> list = new List<string>(ds.Tables[0].Rows.Count);
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        list.Add(dr[0].ToString());

        //    }
        //    dad = null;
        //    if (ds != null)
        //        ds.Dispose();

        //    return list.ToArray();
        //}
        //protected void grdCommCat_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        DataTable dsVal1 = (DataTable)Session["dsVal"];
        //        DataTable ds1 = (DataTable)Session["DwellAirport"];

        //        grdCommCat.PageIndex = e.NewPageIndex;
        //        grdCommCat.DataSource = dsVal1;
        //        grdCommCat.DataBind();
        //        if (dsVal1 != null && dsVal1.Rows.Count > 0)
        //        {
        //            if (dsVal1.Rows.Count > 0)
        //            {
        //                grdCommCat.DataSource = dsVal1;
        //                grdCommCat.DataBind();
        //                lblStatus.Text = "";
        //                for (int i = 0; i < grdCommCat.Rows.Count; i++)
        //                {
        //                    DropDownList ddlcommcat = (DropDownList)grdCommCat.Rows[i].FindControl("grdddlStation");
        //                    ddlcommcat.DataSource = ds1;
        //                    //ddlcommcat.DataSource = (DataTable)Session["dsVal"];
        //                    ddlcommcat.DataTextField = "Airport";
        //                    ddlcommcat.DataValueField = "Airportcode";
        //                    ddlcommcat.DataBind();
        //                    ddlcommcat.Items.Insert(0, "Select");

        //                    ddlcommcat.SelectedIndex = ddlcommcat.Items.IndexOf(ddlcommcat.Items.FindByText(dsVal1.Rows[i]["StationCode"].ToString()));
        //                    if (ddlcommcat.SelectedItem.Text != "Select")
        //                        ddlcommcat.Enabled = false;
        //                    DropDownList ddlcommcategory = (DropDownList)grdCommCat.Rows[i].FindControl("grdddlCommCat");

        //                    ddlcommcategory.DataSource = (DataTable)Session["DwellCommcategory"];
        //                    ddlcommcategory.DataTextField = "CommCategory";
        //                    ddlcommcategory.DataValueField = "SerialNumber";
        //                    ddlcommcategory.DataBind();
        //                    ddlcommcategory.Items.Insert(0, "Select");

        //                    ddlcommcategory.SelectedIndex = ddlcommcategory.Items.IndexOf(ddlcommcategory.Items.FindByText(dsVal1.Rows[i]["CommCategory"].ToString()));
        //                    if (ddlcommcategory.SelectedItem.Text != "Select")
        //                    {
        //                        ddlcommcategory.Enabled = false;
        //                        ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Enabled = false;
        //                    }
        //                    if (((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Text.Length > 0)

        //                        ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Enabled = false;
        //                    //dsVal1.Rows.Add(ddlcommcat.SelectedItem.Value);

        //                }
        //            }
        //            else
        //            { }


        //        }
        //    }

        //    catch (Exception ex)
        //    {

        //    }

        
        //}
    }
}
