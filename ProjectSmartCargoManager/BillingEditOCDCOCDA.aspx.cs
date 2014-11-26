using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class BillingEditOCDCOCDA : System.Web.UI.Page
    {
        ViewInfoInGridBAL objBAL = new ViewInfoInGridBAL();
        BillingInvoiceMatching objbill = new BillingInvoiceMatching();
        //BillingEditOCDCOCDA objBill = new BillingEditOCDCOCDA();
        MasterBAL objBal = new MasterBAL();
        double ST = 0.0;
        double OCTotal = 0.0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["ST"] != null)
                    ST = Convert.ToDouble(Session["ST"].ToString());
                else
                {
                    Session["ST"] = objBal.getServiceTax();
                    ST = Convert.ToDouble(Session["ST"].ToString());
                }

                if (!IsPostBack)
                {
                    string type = Request.QueryString["type"].ToString();
                    string CommCode = Request.QueryString["CommCode"].ToString();

                    DataSet dsDetails = (DataSet)Session["BillingOCDetails"];
                    string code_name = "";

                    if (!dsDetails.Tables[0].Columns.Contains("ChargeName"))
                        dsDetails.Tables[0].Columns.Add("ChargeName");

                    DataView dv = new DataView(dsDetails.Tables[0].Copy());
                    dv.RowFilter = "[Charge Type]='" + type + "' and [Commodity Code]='" + CommCode + "'";

                    DataTable dtDetailsCopy = dv.ToTable();

                    if (!dtDetailsCopy.Columns.Contains("IsNewRow"))
                        dtDetailsCopy.Columns.Add("IsNewRow");

                    for (int i = 0; i < dtDetailsCopy.Rows.Count; i++)
                    {
                        code_name = dtDetailsCopy.Rows[i]["Charge Head Code"].ToString();
                        dtDetailsCopy.Rows[i]["Charge Head Code"] = code_name.Substring(0, code_name.IndexOf('/'));
                        dtDetailsCopy.Rows[i]["ChargeName"] = code_name.Substring(code_name.IndexOf('/') + 1);

                        if (dtDetailsCopy.Rows[i]["IsNewRow"].ToString() != "1")
                            dtDetailsCopy.Rows[i]["IsNewRow"] = "0";
                    }

                    GRD.DataSource = dtDetailsCopy.Copy();
                    GRD.DataBind();
                    

                    Session["dtDetailsCopy"] = dtDetailsCopy.Copy();

                    FormatGRD();

                    string prevtax, prevtotal;
                    prevtax = prevtotal = "";

                    GetTotal(ref prevtotal, ref prevtax);

                    Session["prevtax"] = prevtax;
                    Session["prevtotal"] = prevtotal;
                }

                //if (Convert.ToString(Request.QueryString["Mode"]) == "View")
                //{
                //    btnSave.Visible = false;
                //    btnAdd.Visible = false;
                //    btnDelete.Visible = false;
                //    GRD.Columns[0].Visible = false;
                //}
                //else
                //{
                //    btnSave.Visible = true;
                //    btnAdd.Visible = true;
                //    btnDelete.Visible = true;
                //    GRD.Columns[0].Visible = true;
                //}

            }
            catch (Exception ex)
            {
                LBLStatus.Text = "" + ex.Message;
            }

        }

        public void GetTotal(ref string TotalCharge, ref string TotalTax)
        {
            DataTable dtDetailsCopy = (DataTable)Session["dtDetailsCopy"];
            float total, tax;
            total = tax = 0;

            for (int i = 0; i < dtDetailsCopy.Rows.Count; i++)
            {
                total += float.Parse(dtDetailsCopy.Rows[i]["Charge"].ToString());
                tax += float.Parse(dtDetailsCopy.Rows[i]["Tax"].ToString());
            }

            TotalCharge = "" + total;
            TotalTax = "" + tax;
        }

        public void getOCTotal()
        {
            string type = Request.QueryString["type"].ToString();
            string CommCode = Request.QueryString["CommCode"].ToString();

            DataTable dsDetails = (DataTable)Session["dtDetailsCopy"];

            DataView dv = new DataView(dsDetails.Copy());
            dv.RowFilter = "[Charge Type]='" + type + "' and [Commodity Code]='" + CommCode + "'";

            DataTable dtDetailsCopy = dv.ToTable();

            for (int i = 0; i < dtDetailsCopy.Rows.Count; i++)
            {
                OCTotal += float.Parse(dtDetailsCopy.Rows[i]["Charge"].ToString());
            }
            if(type == "DC")
                Session["BillingOCTotal"] = OCTotal;
            else
                Session["BillingOATotal"] = OCTotal;
        }



        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SaveNewRecords();

                DataTable dtDetailsCopy = (DataTable)Session["dtDetailsCopy"];
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

                DataTable dtDetailsCopy = (DataTable)Session["dtDetailsCopy"];
                DataTable dtDetailsCopyTemp = dtDetailsCopy.Clone();


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
            DataTable dtDetailsCopy = (DataTable)Session["dtDetailsCopy"];

            int i = 0;
            foreach (DataRow row in dtDetailsCopy.Rows)
            {
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
            DataTable dtDetailsCopy = (DataTable)Session["dtDetailsCopy"];

            DataSet dsResult = new DataSet();
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
                    ((TextBox)GRD.Rows[i].FindControl("TXTCharge")).Visible = true;
                    ((TextBox)GRD.Rows[i].FindControl("TXTTax")).Visible = true;
                    //((Label)GRD.Rows[i].FindControl("LBLCharge")).Visible = false;
                    //((Label)GRD.Rows[i].FindControl("LBLTax")).Visible = false;
                }
                else
                {

                    ((Label)GRD.Rows[i].FindControl("LBLChargehead")).Visible = false;
                    ((Label)GRD.Rows[i].FindControl("LBLCode")).Visible = false;
                    //((Label)GRD.Rows[i].FindControl("LBLCharge")).Visible = false;
                    //((Label)GRD.Rows[i].FindControl("LBLTax")).Visible = false;

                    ((DropDownList)GRD.Rows[i].FindControl("DDLChargeHead")).Visible = true;
                    ((Label)GRD.Rows[i].FindControl("LBLChargeHeadCode")).Visible = true;
                    ((TextBox)GRD.Rows[i].FindControl("TXTCharge")).Visible = true;
                    ((TextBox)GRD.Rows[i].FindControl("TXTTax")).Visible = true;


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

            DataTable dtDetailsCopyNew = (DataTable)Session["dtDetailsCopy"];

            int i = 0;
            foreach (DataRow row in dtDetailsCopyNew.Rows)
            {
                row["Charge Head Code"] = row["Charge Head Code"].ToString() + "/" + row["ChargeName"].ToString();
                i++;
            }

            string type = Request.QueryString["type"].ToString();
            string CommCode = Request.QueryString["CommCode"].ToString();

            DataSet dsDetails = (DataSet)Session["BillingOCDetails"];


            if (!dsDetails.Tables[0].Columns.Contains("ChargeName"))
                dsDetails.Tables[0].Columns.Add("ChargeName");

            DataView dv = new DataView(dsDetails.Tables[0].Copy());
            dv.RowFilter = "[Charge Type]<>'" + type + "' or [Commodity Code]<>'" + CommCode + "'";

            DataTable dtDetailsCopy = dv.ToTable();

            // DataTable dtDetailsCopyNew = (DataTable)Session["dtDetailsCopy"];

            dtDetailsCopy.Merge(dtDetailsCopyNew);

            DataSet ds = new DataSet();
            ds.Tables.Add(dtDetailsCopy.Copy());

            Session["BillingOCDetails"] = ds.Copy();

            if(type == "DC")
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindowDC('" + OCTotal + "');", true);
            else //DA
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindowDA('" + OCTotal + "');", true);


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
