using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using QID.DataAccess;
using System.Data.SqlClient;

namespace ProjectSmartCargoManager
{
    public partial class frmULDToAWBAssoc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (!IsPostBack)
            {
                fillDropinAssoc();

                if (Request.QueryString["ULD"] != null && Request.QueryString["ULD"] == "B")
                {
                    Session["MP"] = Request.QueryString["ULD"];
                    if (Session["AWBULD"] != null)
                    {
                        GVAssULD.DataSource = Session["AWBULD"];
                        GVAssULD.DataBind();
                    }
                    else
                    {
                        Session["AWBULD"] = null;
                        AssociationFirstLine();
                    }
                }
                else
                {
                    Session["MP"] = null;
                    if (Session["dtPopAllData"] != null)
                    {
                        GVAssULD.DataSource = (DataTable)Session["dtPopAllData"];
                        GVAssULD.DataBind();
                    }
                    else
                    {
                        Session["dtPopAllData"] = null;
                        AssociationFirstLine();
                    }
                }
                //Session["AWBForULDAssoc"] = Session["AWBForULDAssoc"];
                gvAWBULDAssoc.DataSource = Session["AWBForULDAssoc"];
                gvAWBULDAssoc.DataBind();
                //allocFirstLine(); 
                
            }
        }

        public void ddlAWB_getAvailData(object sender, EventArgs e)
        {
            //Get the button that raised the event
            DropDownList btn = (DropDownList)sender;

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            //Get rowindex
            int rowindex = gvr.RowIndex;
            if (((DropDownList)GVAssULD.Rows[rowindex].FindControl("lblAWBNo")).SelectedItem.Text.Trim() != "Select")
            {
                DataTable dtAWBN = (DataTable)Session["AWBForULDAssoc"];
                DataRow[] dr = dtAWBN.Select("AWB='" + ((DropDownList)GVAssULD.Rows[rowindex].FindControl("lblAWBNo")).SelectedItem.Text.Trim() + "'");
                ((TextBox)GVAssULD.Rows[rowindex].FindControl("lblFlightNo")).Text = dr[0]["FlightNumber"].ToString();
                ((TextBox)GVAssULD.Rows[rowindex].FindControl("lblFlightDate")).Text = dr[0]["FliightDate"].ToString();
                ((TextBox)GVAssULD.Rows[rowindex].FindControl("lblULDOrigin")).Text = dr[0]["Origin"].ToString();
                ((TextBox)GVAssULD.Rows[rowindex].FindControl("lblULDDest")).Text = dr[0]["Destination"].ToString();
            }
        }

        protected DataTable fillDropinAssoc()
        {
            DataTable dtAWB = new DataTable();
            DataTable dtAWBN = new DataTable();

            try
            {
                dtAWBN = (DataTable)Session["AWBForULDAssoc"];
                // dtAWB.Select("AWB<>''");
                dtAWB.Columns.Add("AWB");
                dtAWB.Rows.Add("Select");
                for (int i = 0; i < dtAWBN.Rows.Count; i++)
                {
                    dtAWB.Rows.Add(dtAWBN.Rows[i]["AWB"].ToString());
                }
            }
            catch (Exception ex)
            {
                dtAWB.Columns.Add("AWB");
                dtAWB.Rows.Add("Select");
            }
            return dtAWB;
        }

        protected void allocFirstLine()
        {
            try
            {
                DataTable dtCreditInfoA = new DataTable();
                dtCreditInfoA.Columns.Add("AWB");
                dtCreditInfoA.Columns.Add("TotPcs");
                dtCreditInfoA.Columns.Add("TotWgt");
                dtCreditInfoA.Columns.Add("PcsToAssign");
                dtCreditInfoA.Columns.Add("WgttoAssign");
                dtCreditInfoA.Columns.Add("Origin");
                dtCreditInfoA.Columns.Add("Destination");
                dtCreditInfoA.Columns.Add("FlightNumber");
                dtCreditInfoA.Columns.Add("FliightDate");


                DataRow rw = dtCreditInfoA.NewRow();
                rw["AWB"] = "77586667678";
                rw["TotPcs"] = "50";
                rw["TotWgt"] = "200";
                rw["PcsToAssign"] = "50";
                rw["WgttoAssign"] = "200";
                rw["Origin"] = "BOM";
                rw["Destination"] = "DEL";
                rw["FlightNumber"] = "IT57";
                rw["FliightDate"] = "21-02-2013";
                dtCreditInfoA.Rows.Add(rw);
                Session["AWBForULDAssoc"] = dtCreditInfoA;
                gvAWBULDAssoc.DataSource = dtCreditInfoA;
                gvAWBULDAssoc.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
        protected void AssociationFirstLine()
        {
            try
            {
                DataTable dtCreditInfo = new DataTable();
                dtCreditInfo.Columns.Add("SrNo");
                dtCreditInfo.Columns.Add("ULD");
                dtCreditInfo.Columns.Add("AWB");
                dtCreditInfo.Columns.Add("Type");
                dtCreditInfo.Columns.Add("PosInFlight");
                dtCreditInfo.Columns.Add("Pcs");
                dtCreditInfo.Columns.Add("Wgt");
                dtCreditInfo.Columns.Add("FlightNo");
                dtCreditInfo.Columns.Add("FlightDate");
                dtCreditInfo.Columns.Add("ULDOrigin");
                dtCreditInfo.Columns.Add("ULDDest");

                DataRow rw = dtCreditInfo.NewRow();
                rw["SrNo"] = "";
                rw["ULD"] = "";
                rw["AWB"] = "Select";
                rw["Type"] = "";
                rw["PosInFlight"] = "";
                rw["Pcs"] = "0";
                rw["Wgt"] = "0.0";
                rw["FlightNo"] = "";
                rw["FlightDate"] = "";
                rw["ULDOrigin"] = "";
                rw["ULDDest"] = "";
                dtCreditInfo.Rows.Add(rw);

                GVAssULD.DataSource = dtCreditInfo;
                GVAssULD.DataBind();
                Session["dtPopAllData"] = dtCreditInfo;
                Session["AWBULD"] = dtCreditInfo;
                GVAssULD.Rows[0].Visible = false;
            }
            catch (Exception ex)
            {
            }
        }
        public void addNewRow_Click(object sender, EventArgs e)
        {
            try
            {
                Session["dtPopAllData"] = null;
                DataTable dtcalc = new DataTable();
                dtcalc.Columns.Add("ULDNo");
                dtcalc.Columns.Add("AWBCnt");
                dtcalc.Columns.Add("PcsCnt");
                dtcalc.Columns.Add("WgtCnt");

                DataTable dtCreditInfoAdd = new DataTable();
                dtCreditInfoAdd.Columns.Add("SrNo");
                dtCreditInfoAdd.Columns.Add("ULD");
                dtCreditInfoAdd.Columns.Add("AWB");
                dtCreditInfoAdd.Columns.Add("Type");
                dtCreditInfoAdd.Columns.Add("PosInFlight");
                dtCreditInfoAdd.Columns.Add("Pcs", typeof(int));
                dtCreditInfoAdd.Columns.Add("Wgt", typeof(decimal));
                dtCreditInfoAdd.Columns.Add("FlightNo");
                dtCreditInfoAdd.Columns.Add("FlightDate");
                dtCreditInfoAdd.Columns.Add("ULDOrigin");
                dtCreditInfoAdd.Columns.Add("ULDDest");


                if (GVAssULD.Rows.Count > 0)
                {
                    for (int i = 0; i < GVAssULD.Rows.Count; i++)
                    {
                        if (((TextBox)GVAssULD.Rows[i].FindControl("lblULDNo")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblType")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblPosInFlt")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblAssULDPcs")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblAssULDWgt")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblFlightNo")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblFlightDate")).Text.Trim() != "" && ((DropDownList)GVAssULD.Rows[i].FindControl("lblAWBNo")).SelectedItem.Text.Trim() != "Select")
                        {
                            DataRow rw = dtCreditInfoAdd.NewRow();
                            rw["SrNo"] = ((Label)GVAssULD.Rows[i].FindControl("lblSrNo")).Text.Trim();
                            rw["ULD"] = ((TextBox)GVAssULD.Rows[i].FindControl("lblULDNo")).Text.Trim();
                            rw["AWB"] = ((DropDownList)GVAssULD.Rows[i].FindControl("lblAWBNo")).SelectedItem.Text.Trim();
                            rw["Type"] = ((TextBox)GVAssULD.Rows[i].FindControl("lblType")).Text.Trim();
                            rw["PosInFlight"] = ((TextBox)GVAssULD.Rows[i].FindControl("lblPosInFlt")).Text.Trim();
                            rw["Pcs"] = Convert.ToInt32(((TextBox)GVAssULD.Rows[i].FindControl("lblAssULDPcs")).Text.Trim());
                            rw["Wgt"] = Convert.ToDecimal(((TextBox)GVAssULD.Rows[i].FindControl("lblAssULDWgt")).Text.Trim());
                            rw["FlightNo"] = ((TextBox)GVAssULD.Rows[i].FindControl("lblFlightNo")).Text.Trim();
                            rw["FlightDate"] = ((TextBox)GVAssULD.Rows[i].FindControl("lblFlightDate")).Text.Trim();
                            rw["ULDOrigin"] = ((TextBox)GVAssULD.Rows[i].FindControl("lblULDOrigin")).Text.Trim();
                            rw["ULDDest"] = ((TextBox)GVAssULD.Rows[i].FindControl("lblULDDest")).Text.Trim();

                            dtCreditInfoAdd.Rows.Add(rw);
                        }
                    }
                    DataRow rwN = dtCreditInfoAdd.NewRow();
                    rwN["SrNo"] = "";
                    rwN["ULD"] = "";
                    rwN["AWB"] = "Select";
                    rwN["Type"] = "";
                    rwN["PosInFlight"] = "";
                    rwN["Pcs"] = "0";
                    rwN["Wgt"] = "0.0";
                    rwN["FlightNo"] = "";
                    rwN["FlightDate"] = "";
                    rwN["ULDOrigin"] = "";
                    rwN["ULDDest"] = "";
                    dtCreditInfoAdd.Rows.Add(rwN);
                }
                else
                {
                    DataRow rw = dtCreditInfoAdd.NewRow();
                    rw["SrNo"] = "";
                    rw["ULD"] = "";
                    rw["AWB"] = "Select";
                    rw["Type"] = "";
                    rw["PosInFlight"] = "";
                    rw["Pcs"] = "0";
                    rw["Wgt"] = "0.0";
                    rw["FlightNo"] = "";
                    rw["FlightDate"] = "";
                    rw["ULDOrigin"] = "";
                    rw["ULDDest"] = "";
                    dtCreditInfoAdd.Rows.Add(rw);
                }
                Session["dtPopAllData"] = dtCreditInfoAdd;
                GVAssULD.DataSource = dtCreditInfoAdd.Copy();
                GVAssULD.DataBind();
                ArrayList arl = new ArrayList();
                for (int i = 0; i < dtCreditInfoAdd.Rows.Count; i++)
                {
                    if (((TextBox)GVAssULD.Rows[i].FindControl("lblULDNo")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblType")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblPosInFlt")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblAssULDPcs")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblAssULDWgt")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblFlightNo")).Text.Trim() != "" && ((TextBox)GVAssULD.Rows[i].FindControl("lblFlightDate")).Text.Trim() != "" && ((DropDownList)GVAssULD.Rows[i].FindControl("lblAWBNo")).Text.Trim() != "")
                    {
                        if (arl.Contains(dtCreditInfoAdd.Rows[i]["ULD"].ToString()))
                        {
                            continue;
                        }
                        else
                        {
                            arl.Add(dtCreditInfoAdd.Rows[i]["ULD"].ToString());
                        }
                        object ojcntAWB = dtCreditInfoAdd.Compute("Count(AWB)", "ULD='" + dtCreditInfoAdd.Rows[i]["ULD"].ToString() + "'");
                        object ojSumPcs = dtCreditInfoAdd.Compute("Sum(Pcs)", "ULD='" + dtCreditInfoAdd.Rows[i]["ULD"].ToString() + "'");
                        object ojSumWgt = dtCreditInfoAdd.Compute("Sum(Wgt)", "ULD='" + dtCreditInfoAdd.Rows[i]["ULD"].ToString() + "'");
                        dtcalc.Rows.Add(dtCreditInfoAdd.Rows[i]["ULD"].ToString(), ojcntAWB, ojSumPcs, ojSumWgt);
                    }
                }
                Session["PopCalc"] = dtcalc;
            }
            catch (Exception ex)
            {
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["MP"] != null && Convert.ToString(Session["MP"]) == "B")
                {
                    PrepareULDInformation();
                    ClientScript.RegisterStartupScript(this.GetType(), "", "DoneClickforBooking();", true);
                    return;
                }

                for (int i = 0; i < GVAssULD.Rows.Count; i++)
                {
                    string[] paramname = new string[13];
                    paramname[0] = "ULDNo";
                    paramname[1] = "AWBNo";
                    paramname[2] = "Pieces";
                    paramname[3] = "Weight";
                    paramname[4] = "FlightNo";
                    paramname[5] = "FlightDate";
                    paramname[6] = "PosInFlight";
                    paramname[7] = "ULDType";
                    paramname[8] = "ULDOrigin";
                    paramname[9] = "ULDDest";
                    paramname[10] = "CreatedOn";
                    paramname[11] = "CreatedBy";
                    paramname[12] = "SrNo";


                    object[] paramvalue = new object[13];
                    paramvalue[0] = ((TextBox)GVAssULD.Rows[i].FindControl("lblULDNo")).Text.Trim();
                    paramvalue[1] = ((DropDownList)GVAssULD.Rows[i].FindControl("lblAWBNo")).SelectedItem.Text.Trim();
                    paramvalue[2] = Convert.ToInt32(((TextBox)GVAssULD.Rows[i].FindControl("lblAssULDPcs")).Text.Trim());
                    paramvalue[3] = Convert.ToDecimal(((TextBox)GVAssULD.Rows[i].FindControl("lblAssULDWgt")).Text.Trim());
                    paramvalue[4] = ((TextBox)GVAssULD.Rows[i].FindControl("lblFlightNo")).Text.Trim();
                    paramvalue[5] = ((TextBox)GVAssULD.Rows[i].FindControl("lblFlightDate")).Text.Trim();
                    paramvalue[6] = ((TextBox)GVAssULD.Rows[i].FindControl("lblPosInFlt")).Text.Trim();
                    paramvalue[7] = ((TextBox)GVAssULD.Rows[i].FindControl("lblType")).Text.Trim();
                    paramvalue[8] = ((TextBox)GVAssULD.Rows[i].FindControl("lblULDOrigin")).Text.Trim();
                    paramvalue[9] = ((TextBox)GVAssULD.Rows[i].FindControl("lblULDDest")).Text.Trim();
                    paramvalue[10] = Session["IT"].ToString();
                    paramvalue[11] = Session["UserName"].ToString();
                    paramvalue[12] = ((Label)GVAssULD.Rows[i].FindControl("lblSrNo")).Text.Trim();

                    SqlDbType[] paramtype = new SqlDbType[13];
                    paramtype[0] = SqlDbType.VarChar;
                    paramtype[1] = SqlDbType.VarChar;
                    paramtype[2] = SqlDbType.Int;
                    paramtype[3] = SqlDbType.Decimal;
                    paramtype[4] = SqlDbType.VarChar;
                    paramtype[5] = SqlDbType.VarChar;
                    paramtype[6] = SqlDbType.VarChar;
                    paramtype[7] = SqlDbType.VarChar;
                    paramtype[8] = SqlDbType.VarChar;
                    paramtype[9] = SqlDbType.VarChar;
                    paramtype[10] = SqlDbType.DateTime;
                    paramtype[11] = SqlDbType.VarChar;
                    paramtype[12] = SqlDbType.NVarChar;
                    SQLServer sq = new SQLServer(Global.GetConnectionString());
                    bool res = sq.InsertData("spInsertAndUpdateULDtoAWBAssoc", paramname, paramtype, paramvalue);
                    if (i == (GVAssULD.Rows.Count - 1))
                    {
                        lblMsg.Text = "Action Done Successfully";
                        ClientScript.RegisterStartupScript(this.GetType(), "", "DoneClick();", true);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetULDs(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            SqlDataAdapter dad = new SqlDataAdapter("SELECT Distinct ULDNumber from tblULDMaster where ULDNumber like '" + prefixText + "%'", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());
            }
            return list.ToArray();
        }

        protected void PrepareULDInformation()
        {
            try
            {
                DataTable dtCreditInfo = new DataTable();
                dtCreditInfo.Columns.Add("SrNo");
                dtCreditInfo.Columns.Add("ULD");
                dtCreditInfo.Columns.Add("AWB");
                dtCreditInfo.Columns.Add("Type");
                dtCreditInfo.Columns.Add("PosInFlight");
                dtCreditInfo.Columns.Add("Pcs");
                dtCreditInfo.Columns.Add("Wgt");
                dtCreditInfo.Columns.Add("FlightNo");
                dtCreditInfo.Columns.Add("FlightDate");
                dtCreditInfo.Columns.Add("ULDOrigin");
                dtCreditInfo.Columns.Add("ULDDest");

                for (int intCount = 0; intCount < GVAssULD.Rows.Count; intCount++)
                {
                    DataRow rw = dtCreditInfo.NewRow();
                    rw["SrNo"] = intCount + 1;
                    rw["ULD"] = ((TextBox)GVAssULD.Rows[intCount].FindControl("lblULDNo")).Text.Trim();
                    rw["AWB"] = ((DropDownList)GVAssULD.Rows[intCount].FindControl("lblAWBNo")).SelectedItem.Text.Trim();
                    rw["Type"] = ((TextBox)GVAssULD.Rows[intCount].FindControl("lblType")).Text.Trim();
                    rw["PosInFlight"] = ((TextBox)GVAssULD.Rows[intCount].FindControl("lblPosInFlt")).Text.Trim();
                    rw["Pcs"] = Convert.ToInt32(((TextBox)GVAssULD.Rows[intCount].FindControl("lblAssULDPcs")).Text.Trim());
                    rw["Wgt"] = Convert.ToDecimal(((TextBox)GVAssULD.Rows[intCount].FindControl("lblAssULDWgt")).Text.Trim());
                    rw["FlightNo"] = ((TextBox)GVAssULD.Rows[intCount].FindControl("lblFlightNo")).Text.Trim();
                    rw["FlightDate"] = ((TextBox)GVAssULD.Rows[intCount].FindControl("lblFlightDate")).Text.Trim();
                    rw["ULDOrigin"] = ((TextBox)GVAssULD.Rows[intCount].FindControl("lblULDOrigin")).Text.Trim();
                    rw["ULDDest"] = ((TextBox)GVAssULD.Rows[intCount].FindControl("lblULDDest")).Text.Trim();
                    dtCreditInfo.Rows.Add(rw);
                }

                Session["AWBULD"] = dtCreditInfo;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
