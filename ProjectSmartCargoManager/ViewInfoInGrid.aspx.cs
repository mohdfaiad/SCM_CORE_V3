using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
/*

 2012-05-04  vinayak
 2012-05-05  vinayak
 2012-06-25  vinayak
*/


namespace ProjectSmartCargoManager
{
    public partial class ViewInfoInGrid : System.Web.UI.Page
    {
        ViewInfoInGridBAL objBAL = new ViewInfoInGridBAL();
        MasterBAL objBal = new MasterBAL();
        double ST = 0.0;
        double OCTotal = 0.0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["ST"] != null)
                    ST = Convert.ToDouble(Session["ST"].ToString());
             
                if (!IsPostBack)
                {
                    //Session["ST"] = objBal.getServiceTax();
                    //ST = Convert.ToDouble(Session["ST"].ToString());



                    if (Session["AWBStatus"].ToString().ToUpper() == "E")
                    {
                        btnAdd.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        btnAdd.Enabled = true;
                        btnDelete.Enabled = true;
                    }
                    string type = Request.QueryString["type"].ToString();
                    string CommCode = Request.QueryString["CommCode"].ToString();
                    string curr = Request.QueryString["Currency"].ToString();
                    lblCurrency.Text = curr;
                    DataSet dsDetails = new DataSet("ViewInfoInGrid_1");
                        dsDetails = (DataSet)Session["OCDetails"];
                    string code_name = "";
                    if (type == "DA")
                    {
                        lbltype.Text = "Agent";
                    }
                    else if (type=="DC")
                    {
                        lbltype.Text = "Carrier";
                    }

                    if (!dsDetails.Tables[0].Columns.Contains("ChargeName"))
                        dsDetails.Tables[0].Columns.Add("ChargeName");

                    try
                    {
                        if (Request.QueryString["Org"].ToString().Length > 0 && Request.QueryString["Dest"].ToString().Length > 0)
                        {   
                            Session["ST"] = objBal.getServiceTax(Request.QueryString["Org"].ToString(), Request.QueryString["Dest"].ToString(), type);
                            if (Session["ST"].ToString().Length > 0)
                            {
                                ST = Convert.ToDouble(Session["ST"].ToString());
                            }
                            else
                            {
                                Session["ST"] = objBal.getServiceTax();
                                ST = Convert.ToDouble(Session["ST"].ToString());
                            }

                        }
                    }
                    catch (Exception ex) { }

                    DataView dv = new DataView(dsDetails.Tables[0].Copy());
                    dv.RowFilter = "[Charge Type]='" + type + "' and [Commodity Code]='" + CommCode + "'";

                    DataTable dtDetailsCopy = new DataTable("ViewInfoInGrid_5");
                        dtDetailsCopy = dv.ToTable();

                    if (!dtDetailsCopy.Columns.Contains("IsNewRow"))
                        dtDetailsCopy.Columns.Add("IsNewRow");

                    for (int i = 0; i < dtDetailsCopy.Rows.Count; i++)
                    {
                        code_name = dtDetailsCopy.Rows[i]["Charge Head Code"].ToString();
                        if (code_name.Trim().IndexOf('/') > 0)
                            dtDetailsCopy.Rows[i]["Charge Head Code"] = code_name.Substring(0, code_name.IndexOf('/'));
                        else
                            dtDetailsCopy.Rows[i]["Charge Head Code"] = code_name.Trim();
                        dtDetailsCopy.Rows[i]["ChargeName"] = code_name.Substring(code_name.IndexOf('/') + 1);

                        if (dtDetailsCopy.Rows[i]["IsNewRow"].ToString() != "1")
                            dtDetailsCopy.Rows[i]["IsNewRow"] = "0";
                    }

                    GRD.DataSource = dtDetailsCopy.Copy();
                    GRD.DataBind();

                    ////---------------------------Sumit-----------------------------//
                    ////--------------------Currancy Convert-------------------------//
                    //clsCurrencyConvert objCurrency = new clsCurrencyConvert();
                    //for (int i = 0; i < GRD.Rows.Count; i++)
                    //{

                    //    string strCharge = ((TextBox)GRD.Rows[i].FindControl("TXTCharge")).Text;
                    //    string strTax = ((TextBox)GRD.Rows[i].FindControl("TXTTax")).Text;
                    //    string[] paramarray = new string[] { strCharge, strTax };
                    //    string[] paramreturnarray = objCurrency.CurrencyConvert("INR", lblCurrency.Text.Trim().ToUpper(), paramarray);
                    //    if (paramreturnarray.Length > 0)
                    //    {
                    //        ((TextBox)GRD.Rows[i].FindControl("TXTCharge")).Text = paramreturnarray[0].Trim();
                    //        ((TextBox)GRD.Rows[i].FindControl("TXTTax")).Text = paramreturnarray[1].Trim();
                    //        dtDetailsCopy.Rows[i][3] = paramreturnarray[0].Trim();
                    //        dtDetailsCopy.Rows[i][5] = paramreturnarray[1].Trim();
                    //    }

                    //}
                    ////---------------------------Sumit-----------------------------//

                    Session["dtDetailsCopy"] = dtDetailsCopy.Copy();

                    string prevtax, prevtotal;
                    prevtax = prevtotal = "";

                    GetTotal(ref prevtotal, ref prevtax);

                    Session["prevtax"] = prevtax;
                    Session["prevtotal"] = prevtotal;
                }

                string strQuickBooking = string.Empty;
                if (Session["QB"] != null && Convert.ToString(Session["QB"]) == "1")
                    strQuickBooking = Convert.ToString(Session["QB"]);

                if (Convert.ToString(Request.QueryString["Mode"]) == "View" || Request.QueryString["AWBNo"].ToString().Length < 1)//|| strQuickBooking == "1")
                {
                    btnSave.Visible = false;
                    btnAdd.Visible = false;
                    btnDelete.Visible = false;
                    GRD.Columns[0].Visible = false;
                }
                else
                {
                    btnSave.Visible = true;
                    btnAdd.Visible = true;
                    btnDelete.Visible = true;
                    GRD.Columns[0].Visible = true;
                }
            }
            catch (Exception ex)
            {
                LBLStatus.Text = "" + ex.Message;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SaveNewRecords();

                DataTable dtDetailsCopy = new DataTable("ViewInfoInGrid_6");
                    dtDetailsCopy = (DataTable)Session["dtDetailsCopy"];
                DataRow row = dtDetailsCopy.NewRow();

                row["IsNewRow"] = "1";
                row["Commodity Code"] = Request.QueryString["CommCode"].ToString();
                row["Charge Type"] = Request.QueryString["type"].ToString();
                row["TaxPercent"] = ST.ToString();
                row["DiscountPercent"] = "0";
                row["Discount"] = "0";
                row["CommPercent"] = "0";
                row["Commission"] = "0";

                dtDetailsCopy.Rows.Add(row);

                GRD.DataSource = dtDetailsCopy.Copy();
                GRD.DataBind();

                FormatGRD();

                Session["dtDetailsCopy"] = dtDetailsCopy.Copy();
            }
            catch (Exception ex)
            {
                LBLStatus.Text = "" + ex.Message;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LBLStatus.Text = "";

                SaveNewRecords();

                DataTable dtDetailsCopy = new DataTable("ViewInfoInGrid_7");
                    dtDetailsCopy = (DataTable)Session["dtDetailsCopy"];
                    DataTable dtDetailsCopyTemp = new DataTable("ViewInfoInGrid_8");
                    dtDetailsCopyTemp = dtDetailsCopy.Clone();


                for (int i = 0; i < dtDetailsCopy.Rows.Count; i++)
                {
                    if (!((CheckBox)GRD.Rows[i].FindControl("CHKSelect")).Checked || dtDetailsCopy.Rows[i]["IsNewRow"].ToString() == "0")
                    {
                        DataRow rw = dtDetailsCopyTemp.NewRow();
                        DataRow row = dtDetailsCopy.Rows[i];

                        for (int j = 0; j < dtDetailsCopy.Columns.Count; j++)
                        {
                            rw[j] = row[j];
                        }

                        dtDetailsCopyTemp.Rows.Add(rw);


                    }
                    if (((CheckBox)GRD.Rows[i].FindControl("CHKSelect")).Checked && dtDetailsCopy.Rows[i]["IsNewRow"].ToString() == "0")
                    {
                        LBLStatus.Text = "Preset other charges cannot be deleted.";
                    }

                }

                Session["dtDetailsCopy"] = dtDetailsCopyTemp.Copy();

                GRD.DataSource = dtDetailsCopyTemp.Copy();
                GRD.DataBind();

                FormatGRD();

            }
            catch (Exception ex)
            {
                LBLStatus.Text = "" + ex.Message;
            }
        }

        public void SaveNewRecords()
        {
            DataTable dtDetailsCopy = new DataTable("ViewInfoInGrid_10");
              dtDetailsCopy =  (DataTable)Session["dtDetailsCopy"];

            int i = 0;
            foreach (DataRow row in dtDetailsCopy.Rows)
            {
                //if (row["IsNewRow"].ToString() == "0")
                //{
                //}
                //else
                //{
                //    try
                //    {

                //        row["ChargeName"] = ((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")).SelectedItem.Text;
                //        row["Charge Head Code"] = ((Label)GRD.Rows[i].FindControl("LBLChargeHeadCode")).Text;// +"/" + row["ChargeName"].ToString();                   
                //        row["Charge"] = ((TextBox)GRD.Rows[i].FindControl("TXTCharge")).Text;
                //        row["Tax"] = ((TextBox)GRD.Rows[i].FindControl("TXTTax")).Text;

                //    }
                //    catch (Exception ex)
                //    {

                //    }

                //}

                if (row["IsNewRow"].ToString() == "0")
                {
                    try
                    {
                        row["ChargeName"] = ((Label)GRD.Rows[i].FindControl("LBLChargehead")).Text;
                        row["Charge Head Code"] = ((Label)GRD.Rows[i].FindControl("LBLCode")).Text;// +"/" + row["ChargeName"].ToString();                   
                        row["Charge"] = ((TextBox)GRD.Rows[i].FindControl("TXTCharge")).Text;
                        row["Tax"] = ((TextBox)GRD.Rows[i].FindControl("TXTTax")).Text;
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                else
                {
                    try
                    {

                        row["ChargeName"] = ((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")).SelectedItem.Text;
                        row["Charge Head Code"] = ((Label)GRD.Rows[i].FindControl("LBLChargeHeadCode")).Text;// +"/" + row["ChargeName"].ToString();                   
                        row["Charge"] = ((TextBox)GRD.Rows[i].FindControl("TXTCharge")).Text;
                        row["Tax"] = ((TextBox)GRD.Rows[i].FindControl("TXTTax")).Text;

                    }
                    catch (Exception ex)
                    {

                    }

                }

                i++;
            }

            Session["dtDetailsCopy"] = dtDetailsCopy;

            getOCTotal();

        }

        public void FormatGRD()
        {
            DataTable dtDetailsCopy = new DataTable("ViewInfoInGrid_11");
              dtDetailsCopy =  (DataTable)Session["dtDetailsCopy"];

            DataSet dsResult = new DataSet("ViewInfoInGrid_2");
            if (!GetAllOtherCharges(ref dsResult))
            {
                dsResult = null;
            }

            int i = 0;
            foreach (DataRow row in dtDetailsCopy.Rows)
            {
                if (row["IsNewRow"].ToString() == "0")
                {
                    ((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")).Visible = false;
                    ((Label)GRD.Rows[i].FindControl("LBLChargeHeadCode")).Visible = false;
                    ((Label)GRD.Rows[i].FindControl("LBLCharge")).Visible = false;
                    ((Label)GRD.Rows[i].FindControl("LBLTax")).Visible = false;
                  
                }
                else
                {

                    ((Label)GRD.Rows[i].FindControl("LBLChargehead")).Visible = false;
                    ((Label)GRD.Rows[i].FindControl("LBLCode")).Visible = false;
                    ((Label)GRD.Rows[i].FindControl("LBLCharge")).Visible = false;
                    ((Label)GRD.Rows[i].FindControl("LBLTax")).Visible = false;

                    ((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")).Visible = true;
                    ((Label)GRD.Rows[i].FindControl("LBLChargeHeadCode")).Visible = true;
                    ((TextBox)GRD.Rows[i].FindControl("TXTCharge")).Visible = true;
                    ((TextBox)GRD.Rows[i].FindControl("TXTTax")).Visible = true;
                    ((TextBox)GRD.Rows[i].FindControl("TXTCharge")).Enabled = true;
                    ((TextBox)GRD.Rows[i].FindControl("TXTTax")).Enabled = true;

                    ((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")).DataSource = dsResult.Tables[0].Copy();
                    ((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")).DataTextField = "ChargeHeadName";
                    ((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")).DataValueField = "ChargeHeadCode";
                    ((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")).DataBind();

                    ((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")).Text = row["ChargeName"].ToString();
                    ((Label)GRD.Rows[i].FindControl("LBLChargeHeadCode")).Text = row["Charge Head Code"].ToString();
                    ((TextBox)GRD.Rows[i].FindControl("TXTCharge")).Text = row["Charge"].ToString();
                    ((TextBox)GRD.Rows[i].FindControl("TXTTax")).Text = row["Tax"].ToString();
                }
                i++;
            }
        }

        public bool GetAllOtherCharges(ref DataSet dsResult)
        {
            string errormessage = "";

            bool result = objBAL.GetDistinctNamesOfAllCharges(Request.QueryString["type"].ToString(), ref dsResult, ref errormessage);

            if (!result)
            {
                LBLStatus.Text = "" + errormessage;
            }

            return result;
        }

        protected void DDLChargeHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < GRD.Rows.Count; i++)
            {
                if (((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")) == ((DropDownList)sender))
                {
                    ((Label)GRD.Rows[i].FindControl("LBLChargeHeadCode")).Text = ((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")).SelectedItem.Value;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            SaveNewRecords();


            DataTable dtDetailsCopyNew = new DataTable("ViewInfoInGrid_12");
            dtDetailsCopyNew = (DataTable)Session["dtDetailsCopy"];

            int i = 0;
            foreach (DataRow row in dtDetailsCopyNew.Rows)
            {
                row["Charge Head Code"] = row["Charge Head Code"].ToString() + "/" + row["ChargeName"].ToString();
                i++;
            }




            string type = Request.QueryString["type"].ToString();
            string CommCode = Request.QueryString["CommCode"].ToString();

            DataSet dsDetails = new DataSet("ViewInfoInGrid_3");
                dsDetails = (DataSet)Session["OCDetails"];

            if (!dsDetails.Tables[0].Columns.Contains("ChargeName"))
                dsDetails.Tables[0].Columns.Add("ChargeName");

            DataView dv = new DataView(dsDetails.Tables[0].Copy());
            dv.RowFilter = "[Charge Type]<>'" + type + "' or [Commodity Code]<>'" + CommCode + "'";

            DataTable dtDetailsCopy = new DataTable("ViewInfoInGrid_13");
                dtDetailsCopy = dv.ToTable();

            // DataTable dtDetailsCopyNew = (DataTable)Session["dtDetailsCopy"];

            dtDetailsCopy.Merge(dtDetailsCopyNew);

            DataSet ds = new DataSet("ViewInfoInGrid_4");
            ds.Tables.Add(dtDetailsCopy.Copy());

            Session["OCDetails"] = ds.Copy();



            //--------------------------------------------------------------------------------------------------
            DataTable dsRates = new DataTable("ViewInfoInGrid_14");
                dsRates = (DataTable)Session["dtRates"];
            string charge, tax, total, subtotal;
            charge = tax = total = subtotal = "";


            foreach (DataRow row in dsRates.Rows)
            {
                if (row["CommCode"].ToString() == CommCode)
                {
                    string Total, Tax;
                    Total = Tax = "";

                    GetTotal(ref Total, ref Tax);

                    //charge = tax = total

                    if (type == "DC")
                        row["OcDueCar"] = Total;
                    else
                        row["OcDueAgent"] = Total;


                    row["ServTax"] = "" + ((float.Parse(row["ServTax"].ToString())) - (float.Parse(Session["prevtax"].ToString())) + (float.Parse(Tax)));
                    row["Total"] = "" + ((float.Parse(row["FrIATA"].ToString() != "0" ? row["FrIATA"].ToString() : row["FrMKT"].ToString())) +
                                   (float.Parse(row["ServTax"].ToString())) +
                                   (float.Parse(row["OcDueAgent"].ToString())) +
                                   (float.Parse(row["OcDueCar"].ToString())));

                    //(IATACharge + ServiceTax + OCDA + OCDC);// "0";
                    subtotal = "" + ((float.Parse(row["FrIATA"].ToString() != "0" ? row["FrIATA"].ToString() : row["FrMKT"].ToString())) +                                  
                                   (float.Parse(row["OcDueAgent"].ToString())) +
                                   (float.Parse(row["OcDueCar"].ToString())));
                    charge = Total;
                    tax = row["ServTax"].ToString();
                    total = row["Total"].ToString();
                }
            }

            Session["dtRates"] = dsRates.Copy();


            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + charge + "','" + tax + "','" + total + "','" + subtotal + "');", true);

        }

        public void GetTotal(ref string TotalCharge, ref string TotalTax)
        {
            DataTable dtDetailsCopy = new DataTable("ViewInfoInGrid_15");
               dtDetailsCopy = (DataTable)Session["dtDetailsCopy"];
            string type = Request.QueryString["type"].ToString();
            float total, tax;
            total = tax = 0;

            for (int i = 0; i < dtDetailsCopy.Rows.Count; i++)
            {
                total += float.Parse(dtDetailsCopy.Rows[i]["Charge"].ToString());
                tax += float.Parse(dtDetailsCopy.Rows[i]["Tax"].ToString());
            }

            //vijay
            if (type == "DC")
                Session["OCTotal"] = total;
            else
                Session["OATotal"] = total;
            //vijay

            TotalCharge = "" + total;
            TotalTax = "" + tax;
        }

        public void getOCTotal()
        {
            string type = Request.QueryString["type"].ToString();
            string CommCode = Request.QueryString["CommCode"].ToString();

            DataTable dsDetails = new DataTable("ViewInfoInGrid_16");
                dsDetails = (DataTable)Session["dtDetailsCopy"];

            DataView dv = new DataView(dsDetails.Copy());
            dv.RowFilter = "[Charge Type]='" + type + "' and [Commodity Code]='" + CommCode + "'";

            DataTable dtDetailsCopy = new DataTable("ViewInfoInGrid_17");
               dtDetailsCopy = dv.ToTable();

            for (int i = 0; i < dtDetailsCopy.Rows.Count; i++)
            {
                OCTotal += float.Parse(dtDetailsCopy.Rows[i]["Charge"].ToString());
            }
            if (type == "DC")
                Session["OCTotal"] = OCTotal;
            else
                Session["OATotal"] = OCTotal;
        }

        protected void TXTCharge_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < GRD.Rows.Count; i++)
            {
                if (((TextBox)GRD.Rows[i].FindControl("TXTCharge")) == ((TextBox)sender))
                {
                    ((TextBox)GRD.Rows[i].FindControl("TXTTax")).Text = "" + (float.Parse(((TextBox)GRD.Rows[i].FindControl("TXTCharge")).Text) * ST / 100);
                }
            }
        }

    }
}
