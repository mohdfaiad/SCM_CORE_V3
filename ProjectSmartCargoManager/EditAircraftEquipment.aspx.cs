using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class EditAircraftEquipment : System.Web.UI.Page
    {
        AircraftBAL objBAL = new AircraftBAL(); 
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    
                    //txtTailNo.Enabled = true;
                    //txtAcType.Enabled = true;
                    //txtAcVer.Enabled = true;
                    object[] EqInfo = new object[14];
                    EqInfo = (object[])Session["AcEq"];
                    LoadAcEq(EqInfo);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void LoadAcEq(object[] EqInfo)
        {
            txtManuf.Text = EqInfo[0].ToString();
            txtAcType.Text = EqInfo[1].ToString();
            txtAcVer.Text = EqInfo[2].ToString();
            txtPCap.Text = EqInfo[3].ToString();
            txtLWt.Text = EqInfo[4].ToString();
            txtCarCap.Text = EqInfo[5].ToString();
            txtMTOW.Text = EqInfo[6].ToString();
            txtTailNo.Text = EqInfo[7].ToString();
            ddlStatus.SelectedValue = EqInfo[8].ToString();
            txtRl.Text = EqInfo[10].ToString();
            txtRb.Text = EqInfo[11].ToString();
            txtRh.Text = EqInfo[12].ToString();
            ddldimension.SelectedValue = EqInfo[13].ToString();
            //txtRl.Text = EqInfo[9].ToString();
            //txtRb.Text = EqInfo[10].ToString();
            //txtRh.Text = EqInfo[11].ToString();
            //txtVolLen.Text = EqInfo[12].ToString();
            txtVol.Text = EqInfo[14].ToString();
            ddlVol.SelectedValue = EqInfo[15].ToString();
            ddlAIdentity.SelectedValue = EqInfo[16].ToString();
            //lblStatus.Text = "";

            //txtManuf.Text = EqInfo[0].ToString();
            //txtAcType.Text = EqInfo[1].ToString();
            //txtAcVer.Text = EqInfo[2].ToString();
            //txtPCap.Text = EqInfo[3].ToString();
            //txtLWt.Text = EqInfo[4].ToString();
            //txtCarCap.Text = EqInfo[5].ToString();
            //txtMTOW.Text = EqInfo[6].ToString();
            //txtTailNo.Text = EqInfo[7].ToString();
            //ddlStatus.SelectedValue = EqInfo[8].ToString();
            //txtRl.Text = EqInfo[9].ToString();
            //txtRb.Text = EqInfo[10].ToString();
            //txtRh.Text = EqInfo[11].ToString();
            //txtVolLen.Text = EqInfo[12].ToString();
            //txtVolBreadth.Text = EqInfo[13].ToString();
            //txtVolHeight.Text = EqInfo[14].ToString();

           //lblStatus.Text = "";
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
                int valid = 1;
                try
                {
                    int srno = Convert.ToInt32(Request.QueryString["Srno"].ToString());
                    object[] EqInfo = new object[17];
                    int i=0;

                        EqInfo.SetValue(txtManuf.Text, i);
                        i++;
                        EqInfo.SetValue(txtAcType.Text, i);
                        i++;
                        EqInfo.SetValue(txtAcVer.Text, i);
                        i++;
                        EqInfo.SetValue(txtPCap.Text, i);
                        i++;
                        EqInfo.SetValue(txtLWt.Text, i);
                        i++;
                        EqInfo.SetValue(txtCarCap.Text, i);
                        i++;
                        EqInfo.SetValue(txtMTOW.Text, i);
                        i++;
                        EqInfo.SetValue(txtTailNo.Text, i);
                        i++;
                        EqInfo.SetValue(ddlStatus.SelectedValue.ToString(), i);
                        i++;
                        
                    //
                        try
                        {
                            double inch;
                            double base1;
                            double height;
                            double vollen;
                            //double volbre;
                            //double volhei;

                            if (ddldimension.SelectedItem.Text == "Inc")
                            {
                                inch = (Convert.ToDouble(txtRl.Text) * (2.54));
                                base1 = (Convert.ToDouble(txtRb.Text) * (2.54));
                                height = (Convert.ToDouble(txtRh.Text) * (2.54));
                                EqInfo.SetValue(inch, i);
                                i++;
                                EqInfo.SetValue(base1, i);
                                i++;
                                EqInfo.SetValue(height, i);
                                i++;
                            }
                            else
                            {

                                EqInfo.SetValue(txtRl.Text, i);
                                i++;
                                EqInfo.SetValue(txtRb.Text, i);
                                i++;
                                EqInfo.SetValue(txtRh.Text, i);
                                i++;
                            }
                            if (ddlVol.SelectedItem.Text == "Inch")
                            {
                                vollen = (Convert.ToDouble(txtVol.Text) / (0.061024));
                                decimal d = Convert.ToDecimal(vollen);
                                decimal rounded = Decimal.Round(d, 2);
                                //volbre = (Convert.ToDouble(txtVolBreadth.Text) * (2.54));
                                //volhei = (Convert.ToDouble(txtVolHeight.Text) * (2.54));
                                //i++;
                                EqInfo.SetValue(rounded, i);
                                i++;
                                //i++;
                                //EqInfo.SetValue(volbre, i);
                                //i++;
                                //EqInfo.SetValue(volhei, i);
                            }
                            else
                            {

                                EqInfo.SetValue(txtVol.Text, i);
                                i++;
                                //i++;
                                //EqInfo.SetValue(txtVolBreadth.Text, i);
                                //i++;
                                //EqInfo.SetValue(txtVolHeight.Text, i);
                            }
                            EqInfo.SetValue(ddldimension.SelectedItem.Text, i);
                            i++;
                            EqInfo.SetValue(ddlVol.SelectedItem.Text, i);
                            i++;
                            EqInfo.SetValue(srno, i);
                            i++;
                        }
                        catch (Exception ex)
                        { }

                        EqInfo.SetValue(ddlAIdentity.SelectedItem.Text, i);
                        i++;
                           
                        
                        //EqInfo.SetValue(txtRl.Text, i);
                        //i++;
                        //EqInfo.SetValue(txtRb.Text, i);
                        //i++;
                        //EqInfo.SetValue(txtRh.Text, i);
                        //i++;
                        //EqInfo.SetValue(txtVolLen.Text, i);
                        //i++;
                        //EqInfo.SetValue(txtVolBreadth.Text, i);
                        //i++;
                        //EqInfo.SetValue(txtVolHeight.Text, i);
                       
                        valid = objBAL.EditEquipment(EqInfo);


                        if (valid == 0)
                        {
                            Clear();
                            lblstatusmsg.ForeColor = System.Drawing.Color.Green;
                            lblstatusmsg.Text = "Aircraft Equipment Saved Successfully";
                            //lblStatus.ForeColor = System.Drawing.Color.Green;
                            //lblStatus.Text = "Aircraft Equipment Saved Successfuly";
                           // ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Save()", true);
                        }
                  

                }
                catch (Exception ex)
                { }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListAircraftEquipment.aspx");
        }

        protected void Clear()
        {
            txtAcType.Text = txtAcVer.Text = txtCarCap.Text = txtLWt.Text = txtTailNo.Text = string.Empty;
            txtManuf.Text = txtMTOW.Text = txtPCap.Text = string.Empty;
            txtRb.Text = txtRl.Text = txtRh.Text = txtVol.Text = string.Empty;
            ddldimension.SelectedIndex = ddlVol.SelectedIndex = 0;
            lblstatusmsg.Text = string.Empty;
        }
    }
}
