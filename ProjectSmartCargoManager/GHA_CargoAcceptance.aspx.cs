using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using BAL;
using QID.DataAccess;
using System.Web.Services;

namespace ProjectSmartCargoManager
{
    public partial class GHA_CargoAcceptance : System.Web.UI.Page
    {
        BalGHADockAccp objBAL = new BalGHADockAccp();
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                botlineGrid.Attributes.Add("style", "display:none");
                AccpGridDIV.Attributes.Add("style", "display:none");
                AccpTaskDiv.Attributes.Add("style", "display:none");

                DataSet dsTokenList = da.GetDataset("SELECT DISTINCT TokenNumber FROM dbo.GHAIncomingBooking ORDER BY TokenNumber ASC");
                if (dsTokenList != null)
                {
                    if (dsTokenList.Tables[0].Rows.Count > 0)
                    {
                        ddlTokenList.DataSource = dsTokenList.Tables[0];
                        ddlTokenList.DataTextField = "TokenNumber";
                        ddlTokenList.DataBind();
                        ddlTokenList.Items.Insert(0, new ListItem("Select"));
                    }
                }

                if (Request.QueryString["No"] != null)
                {
                    GetAWBDetails();
                    string tokenno = Request.QueryString["No"].ToString();
                    ddlTokenList.SelectedIndex = ddlTokenList.Items.IndexOf(((ListItem)ddlTokenList.Items.FindByText(tokenno)));
                    //ddlTokenList.Enabled = false;
                }
                LoadDropDown();
                try
                {
                    if (Session["ULDACT"].ToString().ToUpper() == "FALSE")
                    {
                        btnPrintUCR.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                }

            }
        }

        private void LoadDropDown()
        {
            DataSet ds = objBAL.GetDropDownData();
            if (ds.Tables.Count > 0)
            {
                ViewState["dt0"] = ds.Tables[0];
                ViewState["dt1"] = ds.Tables[1];

                foreach (GridViewRow row in grdAWBDetails.Rows)
                {
                    ((DropDownList)row.FindControl("ddlCommCode")).DataSource = ds.Tables[0];
                    ((DropDownList)row.FindControl("ddlCommCode")).DataTextField = "CommodityCode";
                    ((DropDownList)row.FindControl("ddlCommCode")).DataBind();
                    ((DropDownList)row.FindControl("ddlSHC")).DataSource = ds.Tables[1];
                    ((DropDownList)row.FindControl("ddlSHC")).DataTextField = "SHC";
                    ((DropDownList)row.FindControl("ddlSHC")).DataBind();
                }

            }
        }

        private void GetAWBDetails()
        {
            string Id = Request.QueryString["No"].ToString();
            string dt = Request.QueryString["Dt"];
            hdnTokenNo.Value = Id;
            hdnTokenDt.Value = dt;
            DateTime TokenDt = DateTime.ParseExact(dt, "dd/MM/yyyy", null);
            object[] Params = { Id, TokenDt };
            DataSet ds = objBAL.GetAWBList(Params);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grdAWBDetails.DataSource = ds;
                    grdAWBDetails.DataBind();
                }
            }
            for (int i = 0; i < grdAWBDetails.Rows.Count; i++)
            {
                string shc, comcode;

                shc = ds.Tables[0].Rows[i]["SHCCodes"].ToString();
                comcode = ds.Tables[0].Rows[i]["CommodityCode"].ToString();

                if (shc != "")
                    ((DropDownList)grdAWBDetails.Rows[i].FindControl("ddlSHC")).SelectedValue = shc;
                if (comcode != "")
                    ((DropDownList)grdAWBDetails.Rows[i].FindControl("ddlCommCode")).SelectedValue = comcode;
            }
            botlineGrid.Attributes.Add("style", "display:block");
        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdAWBDetails.Rows)
            {
                if (((RadioButton)row.FindControl("radSelectAWB")).Checked == true)
                {
                    //hdnPcsCount.Value = ((Label)row.FindControl("lblPcs")).Text;
                    //hdnWt.Value = ((Label)row.FindControl("lblWt")).Text;

                    string awbnumber = ((Label)row.FindControl("lblAWB")).Text;
                    Session["awbnumber"] = awbnumber;

                    string[] PName = new string[1];
                    PName[0] = "AWBNo";

                    object[] PValue = new object[1];
                    PValue[0] = awbnumber;

                    SqlDbType[] PType = new SqlDbType[1];
                    PType[0] = SqlDbType.VarChar;

                    DataSet Ds = da.SelectRecords("sp_getAWBDimensionForCargoAccp", PName, PValue, PType);
                    if (Ds != null)
                    {
                        if (Ds.Tables.Count > 0)
                        {
                            if (Ds.Tables[0].Rows.Count > 0)
                            {
                                grdAcceptance.DataSource = Ds;
                                grdAcceptance.DataBind();
                                AccpTaskDiv.Attributes.Add("style", "display:block");
                                AccpGridDIV.Attributes.Add("style", "display:block");

                                for (int i = 0; i < grdAcceptance.Rows.Count; i++)
                                {
                                    if (((Label)grdAcceptance.Rows[i].FindControl("lblIsAccp")).Text == "True")
                                        ((CheckBox)grdAcceptance.Rows[i].FindControl("chkAccept")).Checked = true;
                                    else
                                        ((CheckBox)grdAcceptance.Rows[i].FindControl("chkAccept")).Checked = false;
                                }

                                if (Ds.Tables[0].Rows[0]["isTamper"].ToString() == "True")
                                    chkTamper.Checked = true;

                                if (Ds.Tables[0].Rows[0]["isPackaging"].ToString() == "True")
                                    chkPackaging.Checked = true;

                                if (Ds.Tables[0].Rows[0]["isVisual"].ToString() == "True")
                                    chkVisual.Checked = true;

                                if (Ds.Tables[0].Rows[0]["isSmell"].ToString() == "True")
                                    chkSmell.Checked = true;

                                if (Ds.Tables[0].Rows[0]["isDGR"].ToString() == "True")
                                    chkDGR.Checked = true;

                                if (Ds.Tables[0].Rows[0]["isLiveAnimal"].ToString() == "True")
                                    chkLiveAnimal.Checked = true;

                                #region Calculate Total Vol,Weight and Scale Weight

                                decimal FinalTotal = 0, FinalVolume = 0, FinalScaleWt = 0, Volume = 0, Weight = 0, ScaleWt = 0;
                                for (int i = 0; i < grdAcceptance.Rows.Count; i++)
                                {
                                    Volume = 0; Weight = 0;

                                    if (((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim() != "")
                                        Volume = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim());

                                    if (((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim() != "")
                                        Weight = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim());

                                    if (((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim() != "")
                                        ScaleWt = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim());

                                    FinalVolume = FinalVolume + Volume;
                                    FinalTotal = FinalTotal + Weight;
                                    FinalScaleWt = FinalScaleWt + ScaleWt;

                                }

                                txtTotVolWt.Text = FinalTotal.ToString("0.00");
                                txtTotVolCms.Text = FinalVolume.ToString("0.00");
                                txtTotScaleWt.Text = FinalScaleWt.ToString("0.00");

                                decimal TotalVolume = 0;

                                if (txtTotVolCms.Text.Trim() != "")
                                    TotalVolume = Convert.ToDecimal(txtTotVolCms.Text.Trim());

                                if (TotalVolume != 0)
                                {
                                    txtTotVolMtr.Text = (TotalVolume / 10000).ToString("0.00");

                                #endregion Calculate Total Vol,Weight and Scale Weight

                                }
                            }
                        }
                    }
                }
            }
        }

        protected void CalculateVolume(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                TextBox TextBox = (TextBox)sender;
                GridViewRow grRow = (GridViewRow)TextBox.NamingContainer;
                rowindex = grRow.RowIndex;

                int Length = 0, Breadth = 0, Height = 0;
                decimal Volume = 0, Weight = 0;

                //string txtlength;
                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim() != "")
                {
                    //txtlength = ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text;
                    Length = int.Parse(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim());
                }


                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim() != "")
                    Breadth = int.Parse(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim() != "")
                    Height = int.Parse(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim());

                Volume = Length * Breadth * Height;

                //if (ddlUnit.Text.Trim().ToUpper() == "CMS")
                //{
                Weight = Volume / 6000;

                //}
                //else if (ddlUnit.Text.Trim().ToUpper() == "INCHES")
                //{
                //    Weight = Volume / 366;
                //}
                //else if (ddlUnit.Text.Trim().ToUpper() == "METERS")
                //{
                //    Weight = Volume / 0.006m;
                //}

                ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtVol")).Text = Volume.ToString("0.00");
                ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWt")).Text = Weight.ToString("0.00");

                CalculateTotal();

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")) == ((TextBox)sender))
                    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Focus();
                else if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")) == ((TextBox)sender))
                    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Focus();
                else if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")) == ((TextBox)sender))
                    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWt")).Focus();

            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        private void CalculateTotal()
        {
            decimal FinalTotal = 0, FinalVolume = 0, Volume = 0, Weight = 0;

            for (int i = 0; i < grdAcceptance.Rows.Count; i++)
            {
                Volume = 0; Weight = 0;

                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim() != "")
                    Volume = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim() != "")
                    Weight = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim());

                FinalVolume = FinalVolume + Volume;
                FinalTotal = FinalTotal + Weight;
            }

            txtTotVolWt.Text = FinalTotal.ToString("0.00");
            txtTotVolCms.Text = FinalVolume.ToString("0.00");

            decimal TotalVolume = 0;

            if (txtTotVolCms.Text.Trim() != "")
                TotalVolume = Convert.ToDecimal(txtTotVolCms.Text.Trim());

            if (TotalVolume != 0)
            {
                txtTotVolMtr.Text = (TotalVolume / 10000).ToString("0.00");
                //else if (ddlUnit.Text.Trim().ToUpper() == "INCHES")
                //{
                //    txtMeterVolume.Text = (TotalVolume / 610.24m).ToString("0.00");
                //}
                //else if (ddlUnit.Text.Trim().ToUpper() == "METERS")
                //{
                //    txtMeterVolume.Text = TXTVolume.Text;
                //}
            }
            else
                txtTotVolMtr.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string commcode = null, shc = null;
            for (int k = 0; k < grdAWBDetails.Rows.Count; k++)
            {
                if (((RadioButton)grdAWBDetails.Rows[k].FindControl("radSelectAWB")).Checked == true)
                {
                    commcode = ((DropDownList)grdAWBDetails.Rows[k].FindControl("ddlCommCode")).SelectedItem.Text;
                    shc = ((DropDownList)grdAWBDetails.Rows[k].FindControl("ddlSHC")).SelectedItem.Text;
                }
            }
            string awbnumber = Session["awbnumber"].ToString();
            string pccount;
            int length, breadth, height, AccpPcs = 0;
            decimal vol, scwt, wt, AccpWt = 0.00M;
            bool tamper = chkTamper.Checked;
            bool packaging = chkPackaging.Checked;
            bool visual = chkVisual.Checked;
            bool smell = chkSmell.Checked;
            bool dgr = chkDGR.Checked;
            bool liveanimnal = chkLiveAnimal.Checked;
            bool result;

            string date = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy", null);

            object[] Params = new object[27];


            for (int j = 0; j < grdAcceptance.Rows.Count; j++)
            {

                if (((CheckBox)grdAcceptance.Rows[j].FindControl("chkAccept")).Checked == true)
                {
                    AccpPcs = AccpPcs + 1;
                    AccpWt = AccpWt + decimal.Parse(((TextBox)grdAcceptance.Rows[j].FindControl("txtWt")).Text);
                    hdnPcsCount.Value = AccpPcs.ToString();
                    hdnWt.Value = AccpWt.ToString();
                }
                int i = 0;

                //0
                Params.SetValue(awbnumber, i);
                i++;

                //1
                Params.SetValue(((Label)grdAcceptance.Rows[j].FindControl("lblPcsId")).Text, i);
                i++;

                //2
                Params.SetValue("Cms", i);
                i++;

                //3
                if (((TextBox)grdAcceptance.Rows[j].FindControl("txtLgth")).Text == "")
                    length = 0;
                else
                    length = int.Parse(((TextBox)grdAcceptance.Rows[j].FindControl("txtLgth")).Text);
                Params.SetValue(length, i);
                i++;

                //4
                if (((TextBox)grdAcceptance.Rows[j].FindControl("txtBreadth")).Text == "")
                    breadth = 0;
                else
                    breadth = int.Parse(((TextBox)grdAcceptance.Rows[j].FindControl("txtBreadth")).Text);
                Params.SetValue(breadth, i);
                i++;

                //5
                if (((TextBox)grdAcceptance.Rows[j].FindControl("txtHeight")).Text == "")
                    height = 0;
                else
                    height = int.Parse(((TextBox)grdAcceptance.Rows[j].FindControl("txtHeight")).Text);
                Params.SetValue(height, i);
                i++;

                //6
                if (((TextBox)grdAcceptance.Rows[j].FindControl("txtVol")).Text == "")
                    vol = 0.00M;
                else
                    vol = decimal.Parse(((TextBox)grdAcceptance.Rows[j].FindControl("txtVol")).Text);

                Params.SetValue(vol, i);
                i++;

                //7
                if (((TextBox)grdAcceptance.Rows[j].FindControl("txtWt")).Text == "")
                    wt = 0.00M;
                else
                    wt = decimal.Parse(((TextBox)grdAcceptance.Rows[j].FindControl("txtWt")).Text);
                Params.SetValue(wt, i);
                i++;

                //8
                if (((TextBox)grdAcceptance.Rows[j].FindControl("txtScaleWt")).Text == "")
                    scwt = 0.00M;
                else
                    scwt = decimal.Parse(((TextBox)grdAcceptance.Rows[j].FindControl("txtScaleWt")).Text);
                Params.SetValue(scwt, i);
                i++;

                //9
                Params.SetValue(((TextBox)grdAcceptance.Rows[j].FindControl("txtULD")).Text, i);
                i++;

                //10
                Params.SetValue(((DropDownList)grdAcceptance.Rows[j].FindControl("ddlPieceType")).SelectedItem.Text, i);
                i++;

                //11
                Params.SetValue(((TextBox)grdAcceptance.Rows[j].FindControl("txtBagNo")).Text, i);
                i++;

                //12
                Params.SetValue(((TextBox)grdAcceptance.Rows[j].FindControl("txtLocation")).Text, i);
                i++;

                //13
                Params.SetValue(tamper, i);
                i++;
                //14
                Params.SetValue(packaging, i);
                i++;
                //15
                Params.SetValue(visual, i);
                i++;
                //16
                Params.SetValue(smell, i);
                i++;

                //17
                Params.SetValue(dgr, i);
                i++;

                //18
                Params.SetValue(liveanimnal, i);
                i++;

                //19
                Params.SetValue(dt, i);
                i++;

                //20
                Params.SetValue(Session["UserName"].ToString(), i);
                i++;

                //21
                pccount = "1";
                Params.SetValue(pccount, i);
                i++;

                //22 Session["DockNoFromDockAccp"].ToString()
                Params.SetValue("Dock1", i);
                i++;

                //23
                Params.SetValue(shc, i);
                i++;

                //24
                Params.SetValue(commcode, i);
                i++;

                //25
                int srno = 0;
                srno = int.Parse(((Label)grdAcceptance.Rows[j].FindControl("lblSrno")).Text);
                Params.SetValue(srno, i);
                i++;

                //26
                bool accp = true;
                if (((CheckBox)grdAcceptance.Rows[j].FindControl("chkAccept")).Checked == true)
                {
                    accp = true;
                    Params.SetValue(accp, i);
                }
                else
                {
                    accp = false;
                    Params.SetValue(accp, i);
                }
                i++;

                result = objBAL.SaveAccepanceData(Params);
                if (result == true)
                {
                    lblStatus.Text = "Record Added Successfully...";
                    lblStatus.ForeColor = Color.Green;
                    //ddlTokenList.Enabled = true;
                }

            }
            string msg = "Cargo Accepted for AWB" + Session["awbnumber"].ToString();
            msg += "\nPCS:" + hdnPcsCount.Value.ToString() + "\nWT:" + hdnWt.Value.ToString() + "\nUder Token No:";
            msg += hdnTokenNo.Value.ToString() + "\nDated:" + hdnTokenDt.Value.ToString();

            cls_BL.addMsgToOutBox("SCM", msg, "", "");
        }

        protected void CalculateScaleWeight(object sender, EventArgs e)
        {
            int rowindex = 0;
            TextBox TextBox = (TextBox)sender;
            GridViewRow grRow = (GridViewRow)TextBox.NamingContainer;
            rowindex = grRow.RowIndex;

            decimal ScaleWt = 0, FinalScaleWt = 0;

            for (int i = 0; i < grdAcceptance.Rows.Count; i++)
            {
                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim() != "")
                    ScaleWt = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim());

                FinalScaleWt = FinalScaleWt + ScaleWt;
            }
            txtTotScaleWt.Text = FinalScaleWt.ToString("0.00");
        }

        protected void CopyDimensions(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                Button button = (Button)sender;
                GridViewRow grRow = (GridViewRow)button.NamingContainer;
                rowindex = grRow.RowIndex;

                int Length = 0, Breadth = 0, Height = 0, RowCount = 0;
                decimal Volume = 0, Weight = 0;

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim() != "")
                    Length = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim() != "")
                    Breadth = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim() != "")
                    Height = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtVol")).Text.Trim() != "")
                    Volume = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtVol")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWt")).Text.Trim() != "")
                    Weight = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWt")).Text.Trim());

                RowCount = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtCopy")).Text.Trim());

                for (int intRow = rowindex + 1; intRow <= RowCount + rowindex; intRow++)
                {
                    if (intRow < grdAcceptance.Rows.Count)
                    {
                        ((TextBox)grdAcceptance.Rows[intRow].FindControl("txtLgth")).Text = Length.ToString();
                        ((TextBox)grdAcceptance.Rows[intRow].FindControl("txtBreadth")).Text = Breadth.ToString();
                        ((TextBox)grdAcceptance.Rows[intRow].FindControl("txtHeight")).Text = Height.ToString();
                        ((TextBox)grdAcceptance.Rows[intRow].FindControl("txtVol")).Text = Volume.ToString();
                        ((TextBox)grdAcceptance.Rows[intRow].FindControl("txtWt")).Text = Weight.ToString();
                    }
                }

                //while (RowCount < grdAcceptance.Rows.Count)
                //{
                //    rowindex = rowindex + 1;

                //    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLength")).Text = Length.ToString();
                //    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text = Breadth.ToString();
                //    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text = Height.ToString();
                //    ((Label)grdAcceptance.Rows[rowindex].FindControl("lblVolume")).Text = Volume.ToString();
                //    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWeight")).Text = Weight.ToString();
                //}

                CalculateTotal();
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            if (ddlTokenList.SelectedIndex <= 0)
            {
                lblStatus.Text = "Select Token Number...";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            string tokenNo = ddlTokenList.SelectedItem.Text;

            string dt = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime date = DateTime.ParseExact(dt, "dd/MM/yyyy", null);

            object[] Params = { tokenNo, date };

            DataSet ds = objBAL.GetAWBList(Params);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grdAWBDetails.DataSource = ds;
                    grdAWBDetails.DataBind();

                    LoadDropDown();

                    for (int i = 0; i < grdAWBDetails.Rows.Count; i++)
                    {
                        string shc, comcode;

                        shc = ds.Tables[0].Rows[i]["SHCCodes"].ToString();
                        comcode = ds.Tables[0].Rows[i]["CommodityCode"].ToString();

                        if (shc != "")
                            ((DropDownList)grdAWBDetails.Rows[i].FindControl("ddlSHC")).SelectedValue = shc;
                        if (comcode != "")
                            ((DropDownList)grdAWBDetails.Rows[i].FindControl("ddlCommCode")).SelectedValue = comcode;
                    }
                    botlineGrid.Attributes.Add("style", "display:block");
                }
                else
                {
                    lblStatus.Text = "No Records Found...";
                    lblStatus.ForeColor = Color.Red;
                }
            }
        }

        protected void btnShowFlights_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = -1;
                for (int i = 0; i < grdAWBDetails.Rows.Count; i++)
                {
                    if (((ImageButton)grdAWBDetails.Rows[i].FindControl("btnDimensionsPopup")) == ((ImageButton)sender))
                    {
                        rowIndex = i;
                        break;
                    }
                }

                //if (((TextBox)grdAWBDetails.Rows[rowIndex].FindControl("txtFltOrig")).Text == "" ||
                //    ((TextBox)grdAWBDetails.Rows[rowIndex].FindControl("txtFltDest")).Text == "")
                //{
                //    return;
                //}

                string awbno = ((Label)grdAWBDetails.Rows[rowIndex].FindControl("lblAWB")).Text;
               Session["SHC"] = ((DropDownList)grdAWBDetails.Rows[rowIndex].FindControl("ddlSHC")).SelectedItem.Text;
                Session["commCode"] = ((DropDownList)grdAWBDetails.Rows[rowIndex].FindControl("ddlCommCode")).SelectedItem.Text;
                hdnAWBNo.Value = awbno;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>showDimension();</script>", false);

            }
            catch (Exception)
            {
            }
        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
            int count=0;
            for (int i = 0; i < grdAWBDetails.Rows.Count; i++)
            {
                if (((CheckBox)grdAWBDetails.Rows[i].FindControl("chkAcceptAWB")).Checked == true)
                {
                    count++;

                    int totPcs = int.Parse(((Label)grdAWBDetails.Rows[i].FindControl("lblPcs")).Text);
                    float totWt = float.Parse(((Label)grdAWBDetails.Rows[i].FindControl("lblWt")).Text);

                    object[] Params = new object[5];

                    int j = 0;

                    //0
                    string awb=((Label)grdAWBDetails.Rows[i].FindControl("lblAWB")).Text;
                    Params.SetValue(awb, j);
                    j++;

                    //1
                    int accpPcs = int.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).Text);
                    int accpPcsFromDimension=0;
                    //if(Session["AccpPcs"].ToString()!=null)
                    //accpPcsFromDimension = int.Parse(Session["AccpPcs"].ToString());
                    if (accpPcs > totPcs)
                    {
                        lblStatus.Text = "Received Pcs Cannot Be Greater Than Total Pcs...";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).BorderColor = Color.Red;
                        return;
                    }
                    else if (accpPcs <= 0)
                    {
                        lblStatus.Text = "Received Pcs Cannot Be 0...";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).BorderColor = Color.Red;
                        return;
                    }
                    //else if (accpPcsFromDimension < accpPcs || accpPcsFromDimension > accpPcs)
                    //{
                    //    lblStatus.Text = "Received Pcs Does No Match With Dimensions Entered...";
                    //    lblStatus.ForeColor = Color.Red;
                    //    ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).BorderColor = Color.Red;
                    //    return;
                    //}
                    else
                    {
                        ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).BorderColor = Color.Empty;
                        Params.SetValue(accpPcs, j);
                        j++;
                        hdnPcsCount.Value = accpPcs.ToString();
                    }

                    //2
                    float accpWt = float.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).Text);
                    float accpWtFromDimension=0.00F;
                    //if(Session["AccpWt"].ToString()!=null)
                    //accpWtFromDimension = float.Parse(Session["AccpWt"].ToString());
                    if (accpWt > totWt)
                    {
                        lblStatus.Text = "Received Wt Cannot Be Greater Than Total Wt...";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).BorderColor = Color.Red;
                        return;
                    }
                    else if (accpWt <= 0)
                    {
                        lblStatus.Text = "Received Wt Cannot Be 0...";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).BorderColor = Color.Red;
                        return;
                    }
                    //else if (accpWtFromDimension < accpWt || accpWtFromDimension > accpWt)
                    //{
                    //    lblStatus.Text = "Received Wt Does No Match With Dimensions Entered...";
                    //    lblStatus.ForeColor = Color.Red;
                    //    ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).BorderColor = Color.Red;
                    //    return;
                    //}
                    else
                    {
                        ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).BorderColor = Color.Empty;
                        Params.SetValue(accpWt, j);
                        j++;
                        hdnWt.Value = accpWt.ToString();
                    }

                    //3
                    string dt = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime date = DateTime.ParseExact(dt, "dd/MM/yyyy", null);
                    Params.SetValue(date, j);
                    j++;

                    //4
                    Params.SetValue(Session["UserName"].ToString(), j);
                    j++;

                    bool result = objBAL.SaveGHAAcceptanceData(Params);
                    if (result == true)
                    {
                        lblStatus.Text = "Records Added Successfully...";
                        lblStatus.ForeColor = Color.Green;

                        string msg = "Cargo Accepted for AWB" + awb;
                        msg += "\nPCS:" + hdnPcsCount.Value.ToString() + "\nWT:" + hdnWt.Value.ToString() + "\nUder Token No:";
                        msg += hdnTokenNo.Value.ToString() + "\nDated:" + hdnTokenDt.Value.ToString();

                        cls_BL.addMsgToOutBox("SCM", msg, "", "");
                    }
                    else
                    {
                        lblStatus.Text = "Error While Adding Record...";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
            }
            if (count == 0)
            {
                lblStatus.Text = "Select atlest one row...";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }


    }
}
