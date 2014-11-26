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
    public partial class BillingEditFlightDetails : System.Web.UI.Page
    {
        string AWBNumber = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    AWBNumber = Request.QueryString["AWBNumber"].ToString();
                    DataSet dsDetails = (DataSet)Session["FltDetails"];
                    DataView dv = new DataView(dsDetails.Tables[0].Copy());

                    DataTable dtDetailsCopy = dv.ToTable();

                    GRD.DataSource = dtDetailsCopy.Copy();
                    GRD.DataBind();

                    Session["dtDetailsCopy"] = dtDetailsCopy.Copy();
                    FormatGRD();
                }
            }
            catch
            {
                
                throw;
            }
        }

        public void FormatGRD()
        {
            DataTable dtDetailsCopy = (DataTable)Session["dtDetailsCopy"];

            DataSet dsResult = new DataSet();
            
            int i = 0;
            foreach (DataRow row in dtDetailsCopy.Rows)
            {
                ((TextBox)GRD.Rows[i].FindControl("TXTFlightNo")).Visible = true;
                ((TextBox)GRD.Rows[i].FindControl("TXTFlightDate")).Visible = true;
                ((TextBox)GRD.Rows[i].FindControl("TXTChWeight")).Visible = true;
                ((TextBox)GRD.Rows[i].FindControl("TXTRate")).Visible = true;
                ((TextBox)GRD.Rows[i].FindControl("TXTFreight")).Visible = true;
                
                i++;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            double ChWeightTotal = 0;
            double RateTotal = 0;
            double FreightTotal = 0;

            //NewFlightDetails
            DataSet dsFltDetails = new DataSet();
            dsFltDetails.Tables.Add();
            dsFltDetails.Tables[0].TableName = "FltDetails";
            dsFltDetails.Tables[0].Columns.Add("FlightNumber");
            dsFltDetails.Tables[0].Columns.Add("NewFlightNumber");
            dsFltDetails.Tables[0].Columns.Add("FlightDate");
            dsFltDetails.Tables[0].Columns.Add("ChargedWeight");
            dsFltDetails.Tables[0].Columns.Add("RatePerKg");
            dsFltDetails.Tables[0].Columns.Add("Freight");

            dsFltDetails.Tables[0].Columns.Add("Pieces");
            dsFltDetails.Tables[0].Columns.Add("Origin");
            dsFltDetails.Tables[0].Columns.Add("Destination");

            dsFltDetails.Tables[0].Columns.Add("RateType");



            //NewAWBFlightDetails
            for (int i = 0; i < GRD.Rows.Count; i++)
            {
                DataRow newrow = dsFltDetails.Tables[0].NewRow();
                newrow["FlightNumber"] = ((Label)GRD.Rows[i].FindControl("LBLFlightNo")).Text;
                newrow["NewFlightNumber"] = ((TextBox)GRD.Rows[i].FindControl("TXTFlightNo")).Text;
                newrow["FlightDate"] = ((TextBox)GRD.Rows[i].FindControl("TXTFlightDate")).Text;
                newrow["ChargedWeight"] = ((TextBox)GRD.Rows[i].FindControl("TXTChWeight")).Text;
                newrow["RatePerKg"] = ((TextBox)GRD.Rows[i].FindControl("TXTRate")).Text;
                newrow["Freight"] = ((TextBox)GRD.Rows[i].FindControl("TXTFreight")).Text;

                newrow["Pieces"] = ((TextBox)GRD.Rows[i].FindControl("TXTPcs")).Text;
                newrow["Origin"] = ((Label)GRD.Rows[i].FindControl("LBLOrigin")).Text;
                newrow["Destination"] = ((Label)GRD.Rows[i].FindControl("LBLDest")).Text;

                newrow["RateType"] = ((Label)GRD.Rows[i].FindControl("LBLRateType")).Text;


                dsFltDetails.Tables[0].Rows.Add(newrow);

                ChWeightTotal = ChWeightTotal + Convert.ToDouble(((TextBox)GRD.Rows[i].FindControl("TXTChWeight")).Text);
                //RateTotal = RateTotal + Convert.ToDouble(((TextBox)GRD.Rows[i].FindControl("TXTRate")).Text);
                FreightTotal = FreightTotal + Convert.ToDouble(((TextBox)GRD.Rows[i].FindControl("TXTFreight")).Text);
                if (ChWeightTotal == 0)
                    RateTotal = 0;
                else
                    RateTotal = Math.Round(FreightTotal / ChWeightTotal, 2);
            }

            Session["ChWeightTotal"] = ChWeightTotal;
            Session["RateTotal"] = RateTotal;
            Session["FreightTotal"] = FreightTotal;

            Session["FltDetails"] = dsFltDetails.Copy();

            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + FreightTotal + "');", true);

        }

        protected void TXTChWeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < GRD.Rows.Count; i++)
                {
                    if ((TextBox)(GRD.Rows[i].FindControl("TXTChWeight")) == ((TextBox)sender))
                    {
                        ((TextBox)GRD.Rows[i].FindControl("TXTFreight")).Text = "" + float.Parse(((TextBox)GRD.Rows[i].FindControl("TXTChWeight")).Text) * float.Parse(((TextBox)GRD.Rows[i].FindControl("TXTRate")).Text);
                    }
                }
                LBLStatus.Text = "";
            }
            catch (Exception)
            {
                LBLStatus.Text = "Enter valid Charged weight";
            }
            
        }

        protected void TXTRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < GRD.Rows.Count; i++)
                {
                    if ((TextBox)(GRD.Rows[i].FindControl("TXTRate")) == ((TextBox)sender))
                    {
                        ((TextBox)GRD.Rows[i].FindControl("TXTFreight")).Text = "" + float.Parse(((TextBox)GRD.Rows[i].FindControl("TXTChWeight")).Text) * float.Parse(((TextBox)GRD.Rows[i].FindControl("TXTRate")).Text);
                    }
                }
                LBLStatus.Text = "";
            }
            catch
            {
                LBLStatus.Text = "Enter valid Rate per Kg";
            }
            
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDetailsCopy = (DataTable)Session["dtDetailsCopy"];
                DataRow row = dtDetailsCopy.NewRow();

                row["FlightNumber"] = "";
                row["NewFlightNumber"] = "";
                row["FlightDate"] = "";
                row["ChargedWeight"] = "0";
                row["RatePerKg"] = "0";
                row["Freight"] = "0";

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

                DataTable dtDetailsCopy = (DataTable)Session["dtDetailsCopy"];
                DataTable dtDetailsCopyTemp = dtDetailsCopy.Clone();


                for (int i = 0; i < dtDetailsCopy.Rows.Count; i++)
                {
                    if (!((CheckBox)GRD.Rows[i].FindControl("CHKSelect")).Checked || dtDetailsCopy.Rows[i]["FlightNumber"].ToString() != "")
                    {
                        DataRow rw = dtDetailsCopyTemp.NewRow();
                        DataRow row = dtDetailsCopy.Rows[i];

                        for (int j = 0; j < dtDetailsCopy.Columns.Count; j++)
                        {
                            rw[j] = row[j];
                        }

                        dtDetailsCopyTemp.Rows.Add(rw);

                    }
                    if (((CheckBox)GRD.Rows[i].FindControl("CHKSelect")).Checked && dtDetailsCopy.Rows[i]["FlightNumber"].ToString() != "")
                    {
                        LBLStatus.Text = "Present flight details cannot be deleted.";
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
    }
}
