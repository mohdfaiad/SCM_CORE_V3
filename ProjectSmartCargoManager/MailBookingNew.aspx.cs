using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using BAL;
using System.IO;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class MailBookingNew : System.Web.UI.Page
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());
        DateTime dtCurrentDate = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            //DataTable 
            DataSet dsCountry = new DataSet("mailBk_ds1");
            DataSet dsAirport = new DataSet("mailBk_ds2");
            if (!IsPostBack)
            {
                dsCountry = db.SelectRecords("SP_GetAllStationCodeName","level","Country",SqlDbType.VarChar);

                    if (dsCountry != null)
                    {
                        if (dsCountry.Tables != null)
                        {

                            try
                            {
                                ddlPostalAdmin.DataSource = dsCountry;
                                ddlPostalAdmin.DataMember = dsCountry.Tables[0].TableName;
                                ddlPostalAdmin.DataTextField = dsCountry.Tables[0].Columns["CountryDesc"].ColumnName;
                                ddlPostalAdmin.DataValueField = dsCountry.Tables[0].Columns["CountryCode"].ColumnName;
                                ddlPostalAdmin.DataBind();
                                ddlPostalAdmin.Items.Insert(0, "Select");
                            }
                            catch (Exception ex)
                            { }
                        }
                    }
                dsAirport = db.SelectRecords("spGetAirportCodes");
                
                    if (dsAirport != null)
                    {
                        if (dsAirport.Tables != null)
                        {
                            Session["ddlOrg"] = dsAirport.Tables[0];
                            try
                            {
                                ddlofcOrg.DataSource = dsAirport;
                                ddlofcOrg.DataMember = dsAirport.Tables[0].TableName;
                                ddlofcOrg.DataTextField = dsAirport.Tables[0].Columns["Airport"].ColumnName;
                                ddlofcOrg.DataValueField = dsAirport.Tables[0].Columns["AirportCode"].ColumnName;
                                ddlofcOrg.DataBind();
                                ddlofcOrg.Items.Insert(0, "Select");
                                ddlofcOrg.SelectedValue=Session["Station"].ToString();
                            }
                            catch (Exception ex)
                            { }
                            try
                            {
                                ddlofcDest.DataSource = dsAirport;
                                ddlofcDest.DataMember = dsAirport.Tables[0].TableName;
                                ddlofcDest.DataTextField = dsAirport.Tables[0].Columns["Airport"].ColumnName;
                                ddlofcDest.DataValueField = dsAirport.Tables[0].Columns["AirportCode"].ColumnName;
                                ddlofcDest.DataBind();
                                ddlofcDest.Items.Insert(0, "Select");
                                
                                
                            }
                            catch (Exception ex)
                            { }
                        }
                    }
                    txtFlightPrefix.Text = Session["AirlinePrefix"].ToString();
                    DataSet Country = db.SelectRecords("sp_getcountrycodefromairport", "code", Session["Station"].ToString(), SqlDbType.VarChar);
                    Session["Country"] = Country.Tables[0].Rows[0][0].ToString();
                    ddlPostalAdmin.SelectedValue = Session["Country"].ToString();
                    grdMailBooking.DataSource = null;
                    grdMailBooking.DataBind();
                    Session["dsgrdMail"] = null;
                    GetAllStationsForCountry(ddlPostalAdmin.SelectedValue);                    
                    if (grdMailBooking.Rows.Count == 0)
                    {
                        btnAdd_Click(null, null);
                       // return;
                    }
                    if (Request.QueryString["command"] != null && (Request.QueryString["command"].Contains("Edit")))
                    {

                        txtConsignment.Text= Request.QueryString["ConID"].ToString().Trim(); 
                        ListConsigment();
                        if (ddlStatus.SelectedItem.Text == "Confirmed")
                        {
                            EnableDisabled();
                        }
                    }
            }
                
            }
        #region GetFlightRouteData
        protected void txtFlightDate_TextChanged(object sender, EventArgs e)
        {
            DataSet dsresult = new DataSet("mailBk_ds13");
            try
            {
                string strPartnerCode = Session["AirlinePrefix"].ToString();//txtPartnercode.Text.Trim();//((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartner")).Text.Trim();

                string errormessage = "";
                // DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);
                dsresult = GetFlightList(ddlofcOrg.SelectedValue.ToUpper(), ddlofcDest.SelectedValue.ToUpper(), txtFlightDate.Text, 0, 0, 0, ref errormessage, strPartnerCode);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    DataSet ds = (DataSet)Session["Flt"];
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
                                        DataTable dt = new DataTable("mailBk_dt1");
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
                    

                    ddlFlight.DataTextField = "FltNumber";
                    ddlFlight.DataValueField = "ArrTime";
                    ddlFlight.DataSource = dsresult.Tables[0].Copy();
                    ddlFlight.DataBind();
                    //fltID.Items.Add("Select");
                    ddlFlight.Items.Insert(0, new ListItem("Select", "Select"));
                    //fltID.SelectedItem.Text = "Select";
                    //fltID.SelectedIndex = dsresult.Tables[0].Rows.Count - 1;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                    //HidProcessFlag.Value = "1";
                }
                else
                {
                    lblStatus.Text = "no record found";
                    ddlFlight.Items.Clear();
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
            DataSet dsResult = new DataSet("mailBk_ds5");
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

            dsResult = new DataSet("mailBk_ds6");
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

            dsResult = new DataSet("mailBk_ds7");
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveGrid();
            DataTable dtNewList = (DataTable)Session["dsgrdMail"];

            if (dtNewList == null)
            {
                dtNewList = new DataTable("mailBk_dt2");
                dtNewList.Columns.Add("MailOrg");
                dtNewList.Columns.Add("MailDest");
                dtNewList.Columns.Add("LetterPCS");
                dtNewList.Columns.Add("LetterWt");
                dtNewList.Columns.Add("CPPCS");
                dtNewList.Columns.Add("CPWt");
                dtNewList.Columns.Add("EmptyBagPCS");
                dtNewList.Columns.Add("EmptyBagWt");
                dtNewList.Columns.Add("ToatalPCS");
                dtNewList.Columns.Add("TotalWt");
                dtNewList.Columns.Add("AWBNumber");
                dtNewList.Columns.Add("AWBStatus");
                dtNewList.Columns.Add("FlightDate");
                dtNewList.Columns.Add("FlightNumber");
                dtNewList.Columns.Add("ULD");
                dtNewList.Columns.Add("SerialNumber");
            }
            
            lblStatus.Text = "";
            try
            {
                DataRow l_Datarow = dtNewList.NewRow();
                l_Datarow["MailOrg"] = "";
                l_Datarow["MailDest"] = "";
                l_Datarow["LetterPCS"] = "0";

                l_Datarow["LetterWt"] = "0";

                l_Datarow["CPPCS"] = "0";
                l_Datarow["CPWt"] = "0";
                l_Datarow["EmptyBagPCS"] = "0";
                l_Datarow["EmptyBagWt"] = "0";
                l_Datarow["ToatalPCS"] = "0";
                l_Datarow["TotalWt"] = "0";
                l_Datarow["AWBNumber"] = "";
                l_Datarow["AWBStatus"] = "";
                l_Datarow["FlightDate"] = "";
                l_Datarow["FlightNumber"] = "";
                l_Datarow["ULD"] = "";
                l_Datarow["SerialNumber"] = "0";



                dtNewList.Rows.Add(l_Datarow);

                grdMailBooking.DataSource = dtNewList;
                grdMailBooking.DataBind();
                Session["dsgrdMail"] = dtNewList;
                
                if (dtNewList != null)
                {
                    if (dtNewList.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtNewList.Rows.Count; i++)
                        {



                            DropDownList ddlcommcat = (DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailOrg");

                            ddlcommcat.DataSource = (DataTable)Session["ddlOrg"];
                            ddlcommcat.DataTextField = "Airport";
                            ddlcommcat.DataValueField = "AirportCode";
                            ddlcommcat.DataBind();
                            ddlcommcat.Items.Insert(0, "Select");
                            if (dtNewList.Rows[i]["MailOrg"] == null || dtNewList.Rows[i]["MailOrg"].ToString() == "")
                            {
                                ddlcommcat.SelectedValue = Session["Station"].ToString();
                            }
                            else
                            {
                                ddlcommcat.SelectedValue = dtNewList.Rows[i]["MailOrg"].ToString();
                            }
                            
                            //ddlcommcat.SelectedIndex = ddlcommcat.Items.IndexOf(ddlcommcat.Items.FindByText(Session["Station"].ToString()));
                            //if (ddlcommcat.SelectedItem.Text != "Select")
                            //    ddlcommcat.Enabled = false;

                            DropDownList ddlcommcategory = (DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailDest");

                            ddlcommcategory.DataSource = (DataTable)Session["ddlOrg"];
                            ddlcommcategory.DataTextField = "Airport";
                            ddlcommcategory.DataValueField = "AirportCode";
                            ddlcommcategory.DataBind();
                            ddlcommcategory.Items.Insert(0, "Select");

                            //ddlcommcategory.SelectedIndex = ddlcommcategory.Items.IndexOf(ddlcommcategory.Items.FindByText(dtNewList.Rows[i]["CommCategory"].ToString()));
                            //if (dtNewList.Rows[i]["MailOrg"] == null || dtNewList.Rows[i]["MailDest"] == "")
                            //{
                            //    ddlcommcategory.SelectedValue = Session["Station"].ToString();
                            //}
                            //else
                            //{
                            //    ddlcommcategory.SelectedValue = dtNewList.Rows[i]["MailDest"].ToString();
                            //}
                            ddlcommcategory.SelectedValue = dtNewList.Rows[i]["MailDest"].ToString();
                            //if (ddlcommcategory.SelectedItem.Text != "Select")
                            //{
                            //    ddlcommcategory.Enabled = false;
                            //    ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Enabled = false;
                            //}
                            //if (((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Text.Length > 0)
                            //    ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Enabled = false;

                        }


                    }

                }
                
            }
            catch { }
            finally
            {
                if (dtNewList != null)
                    dtNewList.Dispose();

            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            SaveGrid();
            DataTable dsMail = (DataTable)Session["dsgrdMail"];
            DataTable newdsMail = dsMail.Clone();
            try
            {
                

                for (int i = 0; i < grdMailBooking.Rows.Count; i++)
                {
                    if (!((CheckBox)grdMailBooking.Rows[i].FindControl("check")).Checked)
                    {
                        DataRow row = newdsMail.NewRow();
                        row["MailOrg"] = "" + dsMail.Rows[i]["MailOrg"].ToString();
                        row["MailDest"] = "" + dsMail.Rows[i]["MailDest"].ToString();
                        row["LetterPCS"] = "" + dsMail.Rows[i]["LetterPCS"].ToString();
                        row["LetterWt"] = "" + dsMail.Rows[i]["LetterWt"].ToString();
                        row["CPPCS"] = "" + dsMail.Rows[i]["CPPCS"].ToString();
                        row["CPWt"] = "" + dsMail.Rows[i]["CPWt"].ToString();
                        row["EmptyBagPCS"] = "" + dsMail.Rows[i]["EmptyBagPCS"].ToString();
                        row["EmptyBagWt"] = "" + dsMail.Rows[i]["EmptyBagWt"].ToString();
                        row["ToatalPCS"] = "" + dsMail.Rows[i]["ToatalPCS"].ToString();
                        row["TotalWt"] = "" + dsMail.Rows[i]["TotalWt"].ToString();
                        row["AWBNumber"] = "" + dsMail.Rows[i]["AWBNumber"].ToString();
                        row["AWBStatus"] = "" + dsMail.Rows[i]["AWBStatus"].ToString();
                        row["FlightDate"] = "" + dsMail.Rows[i]["FlightDate"].ToString();
                        row["FlightNumber"] = "" + dsMail.Rows[i]["FlightNumber"].ToString();
                        row["ULD"] = "" + dsMail.Rows[i]["ULD"].ToString();
                        row["SerialNumber"] = "" + dsMail.Rows[i]["SerialNumber"].ToString();
                        

                        newdsMail.Rows.Add(row);
                    }
                }
                grdMailBooking.DataSource = newdsMail.Copy();
                grdMailBooking.DataBind();
                if (newdsMail != null)
                {
                    if (newdsMail.Rows.Count > 0)
                    {
                        for (int i = 0; i < newdsMail.Rows.Count; i++)
                        {



                            DropDownList ddlcommcat = (DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailOrg");

                            ddlcommcat.DataSource = (DataTable)Session["ddlOrg"];
                            ddlcommcat.DataTextField = "Airport";
                            ddlcommcat.DataValueField = "AirportCode";
                            ddlcommcat.DataBind();
                            ddlcommcat.Items.Insert(0, "Select");
                            ddlcommcat.SelectedValue = newdsMail.Rows[i]["MailOrg"].ToString();
                            //ddlcommcat.SelectedIndex = ddlcommcat.Items.IndexOf(ddlcommcat.Items.FindByText(newdsMail.Rows[i]["AirportCode"].ToString()));
                            //if (ddlcommcat.SelectedItem.Text != "Select")
                            //    ddlcommcat.Enabled = false;

                            DropDownList ddlcommcategory = (DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailDest");

                            ddlcommcategory.DataSource = (DataTable)Session["ddlOrg"];
                            ddlcommcategory.DataTextField = "Airport";
                            ddlcommcategory.DataValueField = "AirportCode";
                            ddlcommcategory.DataBind();
                            ddlcommcategory.Items.Insert(0, "Select");
                            ddlcommcategory.SelectedValue = newdsMail.Rows[i]["MailDest"].ToString();
                            //ddlcommcategory.SelectedIndex = ddlcommcategory.Items.IndexOf(ddlcommcategory.Items.FindByText(newdsMail.Rows[i]["AirportCode"].ToString()));

                            //if (ddlcommcategory.SelectedItem.Text != "Select")
                            //{
                            //    ddlcommcategory.Enabled = false;
                            //    ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Enabled = false;
                            //}
                            //if (((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Text.Length > 0)
                            //    ((TextBox)grdCommCat.Rows[i].FindControl("grdCommCode")).Enabled = false;

                        }


                    }

                }
                Session["dsgrdMail"] = newdsMail.Copy();

            }
            catch (Exception ex)
            { }
        }

        public void SaveGrid()
        {
            try
            {
                DataTable Dssavetop = (DataTable)Session["dsgrdMail"];

                for (int i = 0; i < grdMailBooking.Rows.Count; i++)
                {

                    Dssavetop.Rows[i]["MailOrg"] = ((DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailOrg")).SelectedValue;
                    Dssavetop.Rows[i]["MailDest"] = ((DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailDest")).SelectedValue;
                    Dssavetop.Rows[i]["LetterPCS"] = ((TextBox)grdMailBooking.Rows[i].FindControl("txtLetterPCS")).Text;
                    Dssavetop.Rows[i]["LetterWt"] = ((TextBox)grdMailBooking.Rows[i].FindControl("txtLetter")).Text;
                    Dssavetop.Rows[i]["CPPCS"] = ((TextBox)grdMailBooking.Rows[i].FindControl("txtCPPCS")).Text;
                    Dssavetop.Rows[i]["CPWt"] = ((TextBox)grdMailBooking.Rows[i].FindControl("txtCPWt")).Text;
                    Dssavetop.Rows[i]["EmptyBagPCS"] = ((TextBox)grdMailBooking.Rows[i].FindControl("txtEmpPCS")).Text;
                    Dssavetop.Rows[i]["EmptyBagWt"] = ((TextBox)grdMailBooking.Rows[i].FindControl("txtEmpWt")).Text;
                    Dssavetop.Rows[i]["ToatalPCS"] = ((TextBox)grdMailBooking.Rows[i].FindControl("lblTotalPCS")).Text;
                    Dssavetop.Rows[i]["TotalWt"] = ((TextBox)grdMailBooking.Rows[i].FindControl("lblTotalWt")).Text;
                    Dssavetop.Rows[i]["AWBNumber"] = ((LinkButton)grdMailBooking.Rows[i].FindControl("lblAWBNumber")).Text;
                    Dssavetop.Rows[i]["AWBStatus"] = ((Label)grdMailBooking.Rows[i].FindControl("lblStatus")).Text;
                    Dssavetop.Rows[i]["FlightDate"] = ((Label)grdMailBooking.Rows[i].FindControl("lblFlightDt")).Text;
                    Dssavetop.Rows[i]["FlightNumber"] = ((Label)grdMailBooking.Rows[i].FindControl("lblFlightNumber")).Text;
                    Dssavetop.Rows[i]["ULD"] = ((Label)grdMailBooking.Rows[i].FindControl("lblULD")).Text;
                    Dssavetop.Rows[i]["SerialNumber"] = ((Label)grdMailBooking.Rows[i].FindControl("lblsrno")).Text;
                    
                }
                if (Dssavetop != null)
                {
                    if (Dssavetop.Rows.Count > 0)
                    {
                        for (int i = 0; i < Dssavetop.Rows.Count; i++)
                        {



                            DropDownList ddlcommcat = (DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailOrg");

                            ddlcommcat.DataSource = (DataTable)Session["ddlOrg"];
                            ddlcommcat.DataTextField = "Airport";
                            ddlcommcat.DataValueField = "AirportCode";
                            ddlcommcat.DataBind();
                            ddlcommcat.Items.Insert(0, "Select");

                            ddlcommcat.SelectedIndex = ddlcommcat.Items.IndexOf(ddlcommcat.Items.FindByText(Dssavetop.Rows[i]["MailOrg"].ToString()));
                            ddlcommcat.SelectedValue = Dssavetop.Rows[i]["MailOrg"].ToString();
                            //if (ddlcommcat.SelectedItem.Text != "Select")
                            //    ddlcommcat.Enabled = false;

                            DropDownList ddlcommcategory = (DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailDest");

                            ddlcommcategory.DataSource = (DataTable)Session["ddlOrg"];
                            ddlcommcategory.DataTextField = "Airport";
                            ddlcommcategory.DataValueField = "AirportCode";
                            ddlcommcategory.DataBind();
                            ddlcommcategory.Items.Insert(0, "Select");

                            ddlcommcategory.SelectedIndex = ddlcommcategory.Items.IndexOf(ddlcommcategory.Items.FindByText(Dssavetop.Rows[i]["MailDest"].ToString()));
                            ddlcommcategory.SelectedValue = Dssavetop.Rows[i]["MailDest"].ToString();

                            

                        }


                    }

                }

                try
                {
                    Session["dsgrdMail"] = Dssavetop.Copy();
                }

                catch (Exception ex)
                { }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            string[] PName = new string[13];
            object[] PValue = new object[13];
            SqlDbType[] PType = new SqlDbType[13];

            string[] PNameSummary = new string[14];
            object[] PValueSummary = new object[14];
            SqlDbType[] PTypeSummary = new SqlDbType[14];

            //string[] PNameCON = new string[4];
            //object[] PValueCON = new object[4];
            //SqlDbType[] PTypeCON = new SqlDbType[4];

            //#region CON
            //    PNameCON[0] = "strRotationId";
            //    PNameCON[1] = "intYear";
            //    PNameCON[2] = "strUserName";
            //    PNameCON[3] = "strOutput";

            //    PValueCON[0] = "CON";
            //    PValueCON[1] = 0;//txtFrmDate.Text;
            //    PValueCON[2] = Session["UserName"].ToString();//txtToDate.Text;
            //    PValueCON[3] = "";

            //    PTypeCON[0] = SqlDbType.VarChar;
            //    PTypeCON[1] = SqlDbType.SmallInt;
            //    PTypeCON[2] = SqlDbType.VarChar;
            //    PTypeCON[3] = SqlDbType.VarChar;
            //#endregion  

            //    DataSet CON = db.SelectRecords("spGenerateRotationNoNEW",PNameCON,PValueCON,PTypeCON);
            if (!ValidateData())
                return; 
            for (int i = 0; i < grdMailBooking.Rows.Count; i++)
            {
               string Orgin = ((DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailOrg")).SelectedValue;
               string Desti = ((DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailDest")).SelectedValue;
                if (Orgin == "Select")
                {
                    lblStatus.Text = "Please Select Origin.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    return;
                }
                if (Desti == "Select")
                {
                    lblStatus.Text = "Please Select Destination.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    return;
                }
            }
            
            try
            {

                PNameSummary[0] = "PostalOrg";
                PNameSummary[1] = "OfcMailOrg";
                PNameSummary[2] = "OfcMailDest";
                PNameSummary[3] = "FlightDate";
                PNameSummary[4] = "FlightNo";
                PNameSummary[5] = "Priority";
                PNameSummary[6] = "Aimail";
                PNameSummary[7] = "SealNumber";
                PNameSummary[8] = "Status";
                PNameSummary[9] = "UpdatedOn";
                PNameSummary[10] = "UpdatedBy";
                PNameSummary[11] = "CreatedOn";
                PNameSummary[12] = "CreatedBy";
                PNameSummary[13] = "CON";

                PValueSummary[0] = ddlPostalAdmin.SelectedValue;
                PValueSummary[1] = ddlofcOrg.SelectedValue;
                PValueSummary[2] = ddlofcDest.SelectedValue;
                PValueSummary[3] = txtFlightDate.Text.Trim();
                PValueSummary[4] = ddlFlight.SelectedItem.Text.Trim();
                PValueSummary[5] = chkPrority.Checked;
                PValueSummary[6] = chkAirmail.Checked;
                PValueSummary[7] = txtSealNumber.Text.Trim();
                PValueSummary[8] = ddlStatus.SelectedItem.Text.Trim();
                PValueSummary[9] = Session["IT"].ToString();
                PValueSummary[10] = Session["UserName"].ToString();
                PValueSummary[11] = Session["IT"].ToString();
                PValueSummary[12] = Session["UserName"].ToString();
                PValueSummary[13] = txtConsignment.Text.Trim();//Session["Country"].ToString() + Session["Station"].ToString() + CON.Tables[0].Rows[0][0].ToString();

                PTypeSummary[0] = SqlDbType.VarChar;
                PTypeSummary[1] = SqlDbType.VarChar;
                PTypeSummary[2] = SqlDbType.VarChar;
                PTypeSummary[3] = SqlDbType.VarChar;
                PTypeSummary[4] = SqlDbType.VarChar;
                PTypeSummary[5] = SqlDbType.Bit;
                PTypeSummary[6] = SqlDbType.Bit;
                PTypeSummary[7] = SqlDbType.VarChar;
                PTypeSummary[8] = SqlDbType.VarChar;
                PTypeSummary[9] = SqlDbType.DateTime;
                PTypeSummary[10] = SqlDbType.VarChar;
                PTypeSummary[11] = SqlDbType.DateTime;
                PTypeSummary[12] = SqlDbType.VarChar;
                PTypeSummary[13] = SqlDbType.VarChar;


                DataSet dsmailinsert = db.SelectRecords("Sp_MailInsertInMailSummary", PNameSummary, PValueSummary, PTypeSummary);
                if (dsmailinsert != null && dsmailinsert.Tables.Count > 0 && dsmailinsert.Tables[0].Rows.Count > 0)
                {
                    lblStatus.Text = dsmailinsert.Tables[0].Rows[0][0].ToString();
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    
                }

            }
            catch (Exception ex)
            { }

            try
            {
                DataSet dsinsert = new DataSet("mailBk_ds8");
                for (int i = 0; i < grdMailBooking.Rows.Count; i++)
                {

                    string Org = "", Dest,TotalPCS="",TotalWt="",AWBNumber="",AWBStatus="",FlightDate="",FlightNo="",ULDNo="",srno="";
                    Org = ((DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailOrg")).SelectedValue;
                    Dest = ((DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailDest")).SelectedValue;
                    TotalPCS = ((TextBox)grdMailBooking.Rows[i].FindControl("lblTotalPCS")).Text;
                    TotalWt = ((TextBox)grdMailBooking.Rows[i].FindControl("lblTotalWt")).Text;
                    AWBNumber = ((LinkButton)grdMailBooking.Rows[i].FindControl("lblAWBNumber")).Text;
                    AWBStatus = ((Label)grdMailBooking.Rows[i].FindControl("lblStatus")).Text;
                    FlightDate = ((Label)grdMailBooking.Rows[i].FindControl("lblFlightDt")).Text;
                    FlightNo = ((Label)grdMailBooking.Rows[i].FindControl("lblFlightNumber")).Text;
                    ULDNo = ((Label)grdMailBooking.Rows[i].FindControl("lblULD")).Text;
                    srno = ((Label)grdMailBooking.Rows[i].FindControl("lblsrno")).Text;

                    
                    PName[0] = "MailOrg";
                    PName[1] = "MailDest";
                    PName[2] = "TotalPCS";
                    PName[3] = "TotalWt";
                    PName[4] = "AWBNumber";
                    PName[5] = "AWBStatus";
                    //PName[6] = "FlightDate";
                    //PName[7] = "FlightNo";
                    PName[6] = "ULDNo";
                    PName[7] = "UpdatedOn";
                    PName[8] = "UpdatedBy";
                    PName[9] = "CON";
                    PName[10] = "CreatedOn";
                    PName[11] = "CreatedBy";
                    PName[12] = "SerialNumber";

                    PValue[0] = Org;
                    PValue[1] = Dest;
                    PValue[2] = TotalPCS;
                    PValue[3] = TotalWt;
                    PValue[4] = AWBNumber;
                    PValue[5] = AWBStatus;
                    //PValue[6] = FlightDate;
                    //PValue[7] = FlightNo;
                    PValue[6] = ULDNo;
                    PValue[7] = Session["IT"].ToString();
                    PValue[8] = Session["UserName"].ToString();
                    PValue[9] = txtConsignment.Text.Trim();//Session["Country"].ToString() + Session["Station"].ToString() + CON.Tables[0].Rows[0][0].ToString();
                    PValue[10] = Session["IT"].ToString();
                    PValue[11] = Session["UserName"].ToString();
                    PValue[12] = srno;

                    PType[0] = SqlDbType.VarChar;
                    PType[1] = SqlDbType.VarChar;
                    PType[2] = SqlDbType.VarChar;
                    PType[3] = SqlDbType.VarChar;
                    PType[4] = SqlDbType.VarChar;
                    PType[5] = SqlDbType.VarChar;
                    //PType[6] = SqlDbType.VarChar;
                    //PType[7] = SqlDbType.VarChar;
                    PType[6] = SqlDbType.VarChar;
                    PType[7] = SqlDbType.DateTime;
                    PType[8] = SqlDbType.VarChar;
                    PType[9] = SqlDbType.VarChar;
                    PType[10] = SqlDbType.DateTime;
                    PType[11] = SqlDbType.VarChar;
                    PType[12] = SqlDbType.VarChar;


                    dsinsert = db.SelectRecords("Sp_MailInsertInMailDetails", PName, PValue, PType);
                    if (dsinsert != null && dsinsert.Tables.Count > 0 && dsinsert.Tables[0].Rows.Count > 0)
                    {
                        lblStatus.Text = dsinsert.Tables[0].Rows[0][0].ToString();
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        
                        //txtConsignment.Text = //Session["Country"].ToString() + Session["Station"].ToString() + CON.Tables[0].Rows[0][0].ToString();
                    }
                }
                #region
                try
                {
                    if (dsinsert.Tables[1].Rows.Count > 0)
                    {
                        grdMailBooking.DataSource = dsinsert.Tables[1];
                        grdMailBooking.DataBind();
                        //Session["dsgrdMail"] = ds.Tables[1];
                        for (int j = 0; j < dsinsert.Tables[1].Rows.Count; j++)
                        {



                            DropDownList ddlcommcat = (DropDownList)grdMailBooking.Rows[j].FindControl("ddlMailOrg");

                            ddlcommcat.DataSource = (DataTable)Session["ddlOrg"];
                            ddlcommcat.DataTextField = "Airport";
                            ddlcommcat.DataValueField = "AirportCode";
                            ddlcommcat.DataBind();
                            ddlcommcat.Items.Insert(0, "Select");

                            ddlcommcat.SelectedIndex = ddlcommcat.Items.IndexOf(ddlcommcat.Items.FindByText(dsinsert.Tables[1].Rows[j]["MailOrg"].ToString()));
                            ddlcommcat.SelectedValue = dsinsert.Tables[1].Rows[j]["MailOrg"].ToString();

                            DropDownList ddlcommcategory = (DropDownList)grdMailBooking.Rows[j].FindControl("ddlMailDest");

                            ddlcommcategory.DataSource = (DataTable)Session["ddlOrg"];
                            ddlcommcategory.DataTextField = "Airport";
                            ddlcommcategory.DataValueField = "AirportCode";
                            ddlcommcategory.DataBind();
                            ddlcommcategory.Items.Insert(0, "Select");

                            ddlcommcategory.SelectedIndex = ddlcommcategory.Items.IndexOf(ddlcommcategory.Items.FindByText(dsinsert.Tables[1].Rows[j]["MailDest"].ToString()));
                            ddlcommcategory.SelectedValue = dsinsert.Tables[1].Rows[j]["MailDest"].ToString();



                        }


                    }
                }
                catch (Exception ex)
                {
                }
                #endregion
            }
            catch (Exception ex)
            { }
        }

        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            try 
            {
                if (!ValidateData())
                    return;
                btnSaveAll_Click(null, null);
            }
            catch (Exception ex) { }
            try 
            {
                DateTime dtd=Convert.ToDateTime(Session["IT"].ToString());
                string[] PName = new string[2]
                {
                    "ConsigID",
                    "ExecDate"
                };
                object[] PValue = new object[2]
                {
                    txtConsignment.Text.ToString(),
                    dtd.ToString("dd/MM/yyyy")
                };
                SqlDbType[] PType = new SqlDbType[2]
                {
                    SqlDbType.VarChar,
                    SqlDbType.VarChar
                };
                //if(db.ExecuteProcedure("spFinalisePOMAIL",PName,PType,PValue))
                string str = db.GetStringByProcedure("spFinalisePOMAIL", PName, PValue, PType);
                if (str.Length > 20) 
                {
                    lblStatus.Text = str;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                 
                }
                else
                {
                    lblStatus.Text = "Consigment Finalized.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    ListConsigment();
                    if (ddlStatus.SelectedItem.Text == "Confirmed")
                    {
                        EnableDisabled();
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void ListConsigment() 
        {
            try 
            {
                DataSet ds = db.SelectRecords("spGetPOMAILConsigmentInfo", "ConsigNo", txtConsignment.Text.Trim(), SqlDbType.VarChar);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0) 
                    {
                        if (ds.Tables[0].Rows.Count > 0) 
                        {
                            DataRow dr=ds.Tables[0].Rows[0];
                            txtFlightDate.Text = dr["FltDt"].ToString();
                            ddlFlight.Items.Clear();
                            ddlFlight.Items.Add(new ListItem(dr["FltNo"].ToString()));
                            ddlofcDest.SelectedValue = dr["PostOfficeDest"].ToString();
                            ddlofcOrg.SelectedValue = dr["PostOfficeOrg"].ToString();
                            ddlStatus.SelectedItem.Text = dr["Status"].ToString();
                            chkAirmail.Checked = Convert.ToBoolean(dr["ShipmentAirmail"].ToString());
                            chkPrority.Checked = Convert.ToBoolean(dr["ShipmentPriority"].ToString());
                            txtSealNumber.Text=dr["SealNo"].ToString();
                            ddlPostalAdmin.SelectedValue = dr["PostalAdminOrg"].ToString();
                        }
                        if (ds.Tables[1].Rows.Count > 0) 
                        {
                            grdMailBooking.DataSource = ds.Tables[1];
                            grdMailBooking.DataBind();
                            Session["dsgrdMail"] = ds.Tables[1];
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {



                                DropDownList ddlcommcat = (DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailOrg");

                                ddlcommcat.DataSource = (DataTable)Session["ddlOrg"];
                                ddlcommcat.DataTextField = "Airport";
                                ddlcommcat.DataValueField = "AirportCode";
                                ddlcommcat.DataBind();
                                ddlcommcat.Items.Insert(0, "Select");

                                ddlcommcat.SelectedIndex = ddlcommcat.Items.IndexOf(ddlcommcat.Items.FindByText(ds.Tables[1].Rows[i]["MailOrg"].ToString()));
                                ddlcommcat.SelectedValue = ds.Tables[1].Rows[i]["MailOrg"].ToString();
                                //if (ddlcommcat.SelectedItem.Text != "Select")
                                //    ddlcommcat.Enabled = false;

                                DropDownList ddlcommcategory = (DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailDest");

                                ddlcommcategory.DataSource = (DataTable)Session["ddlOrg"];
                                ddlcommcategory.DataTextField = "Airport";
                                ddlcommcategory.DataValueField = "AirportCode";
                                ddlcommcategory.DataBind();
                                ddlcommcategory.Items.Insert(0, "Select");

                                ddlcommcategory.SelectedIndex = ddlcommcategory.Items.IndexOf(ddlcommcategory.Items.FindByText(ds.Tables[1].Rows[i]["MailDest"].ToString()));
                                ddlcommcategory.SelectedValue = ds.Tables[1].Rows[i]["MailDest"].ToString();



                            }

                            
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try 
            {
                if (txtConsignment.Text.Length > 0)
                {
                    ListConsigment();
                    //SaveGrid();
                }
                else 
                {
                    lblStatus.Text = "Please Enter valid Consigmnet Details";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex) { }
        }


        public void EnableDisabled()
        {
            try
            {
                ddlPostalAdmin.Enabled = false;
                ddlofcOrg.Enabled = false;
                ddlofcDest.Enabled = false;
                txtFlightDate.Enabled = false;
                txtFlightPrefix.Enabled = false;
                ddlFlight.Enabled = false;
                chkPrority.Enabled = false;
                chkAirmail.Enabled = false;
                txtSealNumber.Enabled = false;
                ddlStatus.Enabled = false;
                txtConsignment.Enabled = false;
                btnAddRow.Enabled = false;
                btnDeleteRow.Enabled = false;

                for (int i = 0; i < grdMailBooking.Rows.Count; i++)
                {
                    grdMailBooking.Rows[i].Cells[0].Enabled = false;
                    grdMailBooking.Rows[i].Cells[1].Enabled = false;
                    grdMailBooking.Rows[i].Cells[2].Enabled = false;
                    grdMailBooking.Rows[i].Cells[9].Enabled = false;
                    grdMailBooking.Rows[i].Cells[10].Enabled = false;
                }
                ////for (int i = 0; i < grdMailBooking.Rows.Count; i++)
                ////{
                ////    if (((Label)grdMailBooking.Rows[i].FindControl("lblStatus")).Text == "Confirmed")
                ////    {
                ////        grdMailBooking.Rows[i].Cells[0].Enabled = false;
                ////        grdMailBooking.Rows[i].Cells[9].Enabled = false;
                ////        grdMailBooking.Rows[i].Cells[10].Enabled = false;
                ////        //grdMailBooking.Rows[i].Cells[12].Enabled = false;
                ////        //grdMailBooking.Rows[i].Cells[13].Enabled = false;
                ////        //grdMailBooking.Rows[i].Cells[14].Enabled = false;
                ////        //grdMailBooking.Rows[i].Cells[0].Enabled = false;
                ////    }
                ////}
            }
            catch (Exception ex)
            { }
        }

        protected void grdMailBooking_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string Cmd = "View";
            Response.Redirect("~//GHA_ConBooking.aspx?command=" + Cmd + "&AWBNumber=" + ((LinkButton)grdMailBooking.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblAWBNumber")).Text.Trim());

        }

        #region Drop Down Country
        protected void ddlPostalAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            try
            {
                GetAllStationsForCountry(ddlPostalAdmin.SelectedValue);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region GetAllStationsForCountry
        protected void GetAllStationsForCountry(string CountryCode)
        {
            UserCreationBAL objUserBAL = new UserCreationBAL();
            DataSet ds = new DataSet("mailBk_ds9");
            try
            {
               
                ds = objUserBAL.GetAllStationsForCountry(CountryCode);
                
                if (ds != null)
                {
                    ddlofcOrg.DataSource = ds;
                    ddlofcOrg.DataMember = ds.Tables[0].TableName;
                    ddlofcOrg.DataTextField = ds.Tables[0].Columns["AirportName"].ColumnName;
                    ddlofcOrg.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                    ddlofcOrg.DataBind();
                    ddlofcOrg.Items.Insert(0, "Select");
                    try
                    {
                        ddlofcOrg.SelectedValue = Session["Station"].ToString();
                    }
                    catch (Exception ex)
                    { }

                    //ddlofcDest.DataSource = ds;
                    //ddlofcDest.DataMember = ds.Tables[0].TableName;
                    //ddlofcDest.DataTextField = ds.Tables[0].Columns["AirportName"].ColumnName;
                    //ddlofcDest.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                    //ddlofcDest.DataBind();
                    //ddlofcDest.Items.Insert(0, "Select");
                    //ds.Dispose();


                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {



                                DropDownList ddlcommcat = (DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailOrg");

                                ddlcommcat.DataSource = ds;
                                ddlcommcat.DataTextField = "AirportName";
                                ddlcommcat.DataValueField = "AirportCode";
                                ddlcommcat.DataBind();
                                ddlcommcat.Items.Insert(0, "Select");
                                if (ds.Tables[0].Rows[i]["AirportCode"] == null || ds.Tables[0].Rows[i]["AirportCode"].ToString() == "")
                                {
                                    ddlcommcat.SelectedValue = Session["Station"].ToString();
                                }
                                ////else
                                ////{
                                ////    ddlcommcat.SelectedValue = ds.Tables[0].Rows[i]["AirportCode"].ToString();
                                ////}

                                

                                //DropDownList ddlcommcategory = (DropDownList)grdMailBooking.Rows[i].FindControl("ddlMailDest");

                                //ddlcommcategory.DataSource = ds;
                                //ddlcommcategory.DataTextField = "AirportName";
                                //ddlcommcategory.DataValueField = "AirportCode";
                                //ddlcommcategory.DataBind();
                                //ddlcommcategory.Items.Insert(0, "Select");


                            }


                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }

            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }
        #endregion

        public bool ValidateData()
        {
            if (txtFlightDate.Text == "")
            {
                lblStatus.Text = "Please Enter Date.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            if (ddlFlight.SelectedItem.Text == "Select")
            {
                lblStatus.Text = "Please Select Flight Number.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            if (txtConsignment.Text == "" || txtConsignment.Text.Length < 1)
            {
                lblStatus.Text = "Please Enter Consigment ID.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            return true;
        }

    }
}
