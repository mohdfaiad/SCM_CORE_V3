using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data; 
using QID.DataAccess;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Configuration;
using System.Data.SqlClient;
using BAL;

/*

 2012-04-05  vinayak
 2012-07-24  vinayak
 
*/

namespace ProjectSmartCargoManager
{
    public partial class GHA_CargoDimensions : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BalGHADockAccp objBLL = new BalGHADockAccp();
        DataSet dsDimensions;
        DataSet dsDimensionsSave=new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    try
                    {
                        Session["AWBUOM"] = null;
                        Session["AWBSummaryDetails"] = null;
                        Session["dsDimesionAllAcceptanceSave"] = null;
                        Session["dsDimesionAllAcceptance"] = null;
                        grdAcceptance.DataSource = null;
                        grdAcceptance.DataBind();
                        Session["PieceDetails"] = null;
                        DataSet dSet = new DataSet();
                        DataSet Ds = new DataSet();
                        if (Request.QueryString["AWBNo"] != null)
                        {
                            string awbnumber = Request.QueryString["AWBNo"].ToString();
                            string[] AWBDetails = awbnumber.Split('-');
                            string flightno = Request.QueryString["FlightNo"].ToString();
                            string flightdate = Request.QueryString["FlightDate"].ToString();
                            int AcceptedPieces = Convert.ToInt32(Request.QueryString["RecievedPcsTxt"].ToString());
                            decimal AcceptedWeight = Convert.ToDecimal(Request.QueryString["TotalWt"].ToString());
                            object[] QueryValues = new object[5];
                            //Session["awbnumber"] = awbnumber;
                            QueryValues[0] = awbnumber;
                            if (Request.QueryString["TotalPcs"] != null)
                            {
                                QueryValues[1] = Convert.ToInt32(Request.QueryString["TotalPcs"].ToString());
                                LBLPcsCount.Text = Request.QueryString["TotalPcs"].ToString();
                            }
                            if (Request.QueryString["TotalWt"] != null)
                            {
                                QueryValues[2] = Convert.ToDecimal(Request.QueryString["TotalWt"].ToString());
                                lblGrossWt.Text = Request.QueryString["TotalWt"].ToString();
                            }
                            if (Request.QueryString["SHC"] != null)
                            {
                                QueryValues[3] = Request.QueryString["SHC"].ToString();
                            }
                            if (Request.QueryString["CommodityCode"] != null)
                            {
                                QueryValues[4] = Request.QueryString["CommodityCode"].ToString();

                            }
                            if (Request.QueryString["UOM"] != null)
                            {
                                Session["AWBUOM"] = Request.QueryString["UOM"].ToString();
                            }
                            else
                            {
                                LoginBL objHome = new LoginBL();
                                Session["AWBUOM"] = objHome.GetMasterConfiguration("MeasurmentUnit");
                                objHome = null;
                            }



                            #region Added Code to Maintain Session State
                            if (Session["dsDimesionAllAcceptance"] != null)
                            {
                                
                                Ds = (DataSet)Session["dsDimesionAllAcceptance"];
                                Session["dsDimesionAllAcceptanceSave"] = Ds;
                                if (Ds.Tables.Count > 0)
                                {
                                    if (Ds.Tables.Count > 1)
                                    {
                                        if (Ds.Tables[1].Rows[0]["AWBNumber"].ToString() == awbnumber && Ds.Tables[1].Rows[0]["FltNumber"].ToString() == flightno && Ds.Tables[1].Rows[0]["FlightDate"].ToString() == flightdate)
                                        {
                                            grdAcceptance.DataSource = Ds;
                                            grdAcceptance.DataBind();
                                            Session["AWBSummaryDetails"] = QueryValues;
                                            if (Ds != null)
                                            {
                                                if (Ds.Tables.Count > 0)
                                                {
                                                    if (Ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        #region Code for Partial Acceptance Old
                                                        //if (AcceptedPieces > Convert.ToInt32(QueryValues[1]))
                                                        //{

                                                        //    dSet = GenerateAWBDimensionsAcceptance(AWBDetails[1], AcceptedPieces, Ds, AcceptedWeight, false, AWBDetails[0], flightno, flightdate);
                                                        //    DClone.Tables.Add(dSet.Tables[0].Copy());
                                                        //    DClone.Tables.Add(Ds.Tables[1].Copy());
                                                        //    Ds = null;
                                                        //    Ds = DClone;
                                                        //    grdAcceptance.DataSource = Ds;
                                                        //    grdAcceptance.DataBind();
                                                        //}
                                                        //else
                                                        //{
                                                        //    DataTable Dt = Ds.Tables[0].Clone();
                                                        //    DataTable Dtable = Ds.Tables[1].Clone();
                                                        //    for (int T = 0; T < Convert.ToInt32(QueryValues[1]); T++)
                                                        //    {
                                                        //        //foreach (DataRow row in Ds.Tables[0].Rows)
                                                        //        //{
                                                        //            Dt.ImportRow(Ds.Tables[0].Rows[T]);
                                                        //        //}
                                                        //        //foreach (DataRow row in Ds.Tables[1].Rows)
                                                        //        //{
                                                        //            Dtable.ImportRow(Ds.Tables[0].Rows[T]);
                                                        //        //}
                                                        //    }
                                                        //    Ds = null;
                                                        //    Ds = new DataSet();
                                                        //    Ds.Tables.Add(Dt.Copy());
                                                        //    Ds.Tables.Add(Dtable.Copy());
                                                        //    grdAcceptance.DataSource = Ds;
                                                        //    grdAcceptance.DataBind();
                                                        //    Dt.Dispose();
                                                        //    Dtable.Dispose();



                                                        //}
                                                        #endregion

                                                        //
                                                        //grdAcceptance.DataSource = dSet;
                                                        //grdAcceptance.DataBind();


                                                        //grdAcceptance.DataSource = Ds;
                                                        //grdAcceptance.DataBind();
                                                        for (int i = 0; i < grdAcceptance.Rows.Count; i++)
                                                        {

                                                            ((DropDownList)grdAcceptance.Rows[i].FindControl("ddlPieceType")).Text = Ds.Tables[0].Rows[i]["PieceType"].ToString();
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

                                                        TXTTotal.Text = FinalTotal.ToString("0.00");
                                                        TXTVolume.Text = FinalVolume.ToString("0.00");
                                                        txtTotScaleWt.Text = FinalScaleWt.ToString("0.00");

                                                        decimal TotalVolume = 0;

                                                        if (TXTVolume.Text.Trim() != "")
                                                            TotalVolume = Convert.ToDecimal(TXTVolume.Text.Trim());

                                                        if (TotalVolume != 0)
                                                        {
                                                            txtMeterVolume.Text = (TotalVolume / 10000).ToString("0.00");
                                                        }
                                                        #endregion Calculate Total Vol,Weight and Scale Weight
                                                    }
                                                    if (Ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        lblAWB.Text = awbnumber;
                                                        lblCommodity.Text = Ds.Tables[1].Rows[0]["CommodityCode"].ToString();
                                                        lblFlightDate.Text = Ds.Tables[1].Rows[0]["FltDate"].ToString();
                                                        lblFlightNo.Text = Ds.Tables[1].Rows[0]["FltNumber"].ToString();
                                                        //lblGrossWt.Text = Ds.Tables[1].Rows[0]["GrossWeight"].ToString();
                                                        //LBLPcsCount.Text = Ds.Tables[1].Rows[0]["PiecesCount"].ToString();


                                                    }

                                                }
                                            }
                                        }
                                        #region Listing Through Database
                                        else
                                        {
                                            Ds = null;
                                            Session["AWBSummaryDetails"] = QueryValues;
                                            string[] PName = new string[3];
                                            PName[0] = "AWBNo";
                                            PName[1] = "FlightNo";
                                            PName[2] = "FlightDate";

                                            object[] PValue = new object[3];
                                            PValue[0] = awbnumber;
                                            PValue[1] = flightno;
                                            PValue[2] = flightdate;

                                            SqlDbType[] PType = new SqlDbType[3];
                                            PType[0] = SqlDbType.VarChar;
                                            PType[1] = SqlDbType.VarChar;
                                            PType[2] = SqlDbType.VarChar;
                                            DataSet DClone = new DataSet();

                                            Ds = da.SelectRecords("sp_getAWBDimensionForCargoAccp", PName, PValue, PType);

                                            if (Ds != null)
                                            {
                                                if (Ds.Tables.Count > 0)
                                                {
                                                    if (Ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        #region Code for Partial Acceptance Old
                                                        //if (AcceptedPieces > Convert.ToInt32(QueryValues[1]))
                                                        //{

                                                        //    dSet = GenerateAWBDimensionsAcceptance(AWBDetails[1], AcceptedPieces, Ds, AcceptedWeight, false, AWBDetails[0], flightno, flightdate);
                                                        //    DClone.Tables.Add(dSet.Tables[0].Copy());
                                                        //    DClone.Tables.Add(Ds.Tables[1].Copy());
                                                        //    Ds = null;
                                                        //    Ds = DClone;
                                                        //    grdAcceptance.DataSource = Ds;
                                                        //    grdAcceptance.DataBind();
                                                        //}
                                                        //else
                                                        //{
                                                        //    DataTable Dt = Ds.Tables[0].Clone();
                                                        //    DataTable Dtable = Ds.Tables[1].Clone();
                                                        //    for (int T = 0; T < Convert.ToInt32(QueryValues[1]); T++)
                                                        //    {
                                                        //        //foreach (DataRow row in Ds.Tables[0].Rows)
                                                        //        //{
                                                        //            Dt.ImportRow(Ds.Tables[0].Rows[T]);
                                                        //        //}
                                                        //        //foreach (DataRow row in Ds.Tables[1].Rows)
                                                        //        //{
                                                        //            Dtable.ImportRow(Ds.Tables[0].Rows[T]);
                                                        //        //}
                                                        //    }
                                                        //    Ds = null;
                                                        //    Ds = new DataSet();
                                                        //    Ds.Tables.Add(Dt.Copy());
                                                        //    Ds.Tables.Add(Dtable.Copy());
                                                        //    grdAcceptance.DataSource = Ds;
                                                        //    grdAcceptance.DataBind();
                                                        //    Dt.Dispose();
                                                        //    Dtable.Dispose();



                                                        //}
                                                        #endregion

                                                        #region Code for Partial Acceptance (19/01/2014)


                                                        dSet = GenerateAWBDimensionsAcceptance(AWBDetails[1], AcceptedPieces, Ds, AcceptedWeight, false, AWBDetails[0], flightno, flightdate);
                                                        DClone.Tables.Add(dSet.Tables[0].Copy());
                                                        DClone.Tables.Add(Ds.Tables[1].Copy());
                                                        Ds = null;
                                                        Ds = DClone;
                                                        grdAcceptance.DataSource = Ds;
                                                        grdAcceptance.DataBind();
                                                        DClone.Dispose();
                                                        Session["dsDimesionAllAcceptanceSave"] = Ds;
                                                        //Session["dsDimesionAllAcceptance"] = dSet;
                                                        //Session["dsDimesionAllAcceptance"] = Ds;

                                                        #endregion
                                                        //
                                                        //grdAcceptance.DataSource = dSet;
                                                        //grdAcceptance.DataBind();


                                                        //grdAcceptance.DataSource = Ds;
                                                        //grdAcceptance.DataBind();
                                                        for (int i = 0; i < grdAcceptance.Rows.Count; i++)
                                                        {

                                                            ((DropDownList)grdAcceptance.Rows[i].FindControl("ddlPieceType")).Text = Ds.Tables[0].Rows[i]["PieceType"].ToString();
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

                                                        TXTTotal.Text = FinalTotal.ToString("0.00");
                                                        TXTVolume.Text = FinalVolume.ToString("0.00");
                                                        txtTotScaleWt.Text = FinalScaleWt.ToString("0.00");

                                                        decimal TotalVolume = 0;

                                                        if (TXTVolume.Text.Trim() != "")
                                                            TotalVolume = Convert.ToDecimal(TXTVolume.Text.Trim());

                                                        if (TotalVolume != 0)
                                                        {
                                                            txtMeterVolume.Text = (TotalVolume / 10000).ToString("0.00");
                                                        }
                                                        #endregion Calculate Total Vol,Weight and Scale Weight
                                                    }
                                                    if (Ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        lblAWB.Text = awbnumber;
                                                        lblCommodity.Text = Ds.Tables[1].Rows[0]["CommodityCode"].ToString();
                                                        lblFlightDate.Text = Ds.Tables[1].Rows[0]["FltDate"].ToString();
                                                        lblFlightNo.Text = Ds.Tables[1].Rows[0]["FltNumber"].ToString();
                                                        lblGrossWt.Text = Ds.Tables[1].Rows[0]["GrossWeight"].ToString();
                                                        LBLPcsCount.Text = Ds.Tables[1].Rows[0]["PiecesCount"].ToString();


                                                    }

                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }

                            }
                            #endregion

                            #region Listing Through Database
                            else
                            {
                                Ds = null;
                                Session["AWBSummaryDetails"] = QueryValues;
                                string[] PName = new string[3];
                                PName[0] = "AWBNo";
                                PName[1] = "FlightNo";
                                PName[2] = "FlightDate";

                                object[] PValue = new object[3];
                                PValue[0] = awbnumber;
                                PValue[1] = flightno;
                                PValue[2] = flightdate;

                                SqlDbType[] PType = new SqlDbType[3];
                                PType[0] = SqlDbType.VarChar;
                                PType[1] = SqlDbType.VarChar;
                                PType[2] = SqlDbType.VarChar;
                                DataSet DClone = new DataSet();

                                Ds = da.SelectRecords("sp_getAWBDimensionForCargoAccp", PName, PValue, PType);

                                if (Ds != null)
                                {
                                    if (Ds.Tables.Count > 0)
                                    {
                                        if (Ds.Tables[1].Rows.Count > 0)
                                        {
                                            #region Code for Partial Acceptance Old
                                            //if (AcceptedPieces > Convert.ToInt32(QueryValues[1]))
                                            //{

                                            //    dSet = GenerateAWBDimensionsAcceptance(AWBDetails[1], AcceptedPieces, Ds, AcceptedWeight, false, AWBDetails[0], flightno, flightdate);
                                            //    DClone.Tables.Add(dSet.Tables[0].Copy());
                                            //    DClone.Tables.Add(Ds.Tables[1].Copy());
                                            //    Ds = null;
                                            //    Ds = DClone;
                                            //    grdAcceptance.DataSource = Ds;
                                            //    grdAcceptance.DataBind();
                                            //}
                                            //else
                                            //{
                                            //    DataTable Dt = Ds.Tables[0].Clone();
                                            //    DataTable Dtable = Ds.Tables[1].Clone();
                                            //    for (int T = 0; T < Convert.ToInt32(QueryValues[1]); T++)
                                            //    {
                                            //        //foreach (DataRow row in Ds.Tables[0].Rows)
                                            //        //{
                                            //            Dt.ImportRow(Ds.Tables[0].Rows[T]);
                                            //        //}
                                            //        //foreach (DataRow row in Ds.Tables[1].Rows)
                                            //        //{
                                            //            Dtable.ImportRow(Ds.Tables[0].Rows[T]);
                                            //        //}
                                            //    }
                                            //    Ds = null;
                                            //    Ds = new DataSet();
                                            //    Ds.Tables.Add(Dt.Copy());
                                            //    Ds.Tables.Add(Dtable.Copy());
                                            //    grdAcceptance.DataSource = Ds;
                                            //    grdAcceptance.DataBind();
                                            //    Dt.Dispose();
                                            //    Dtable.Dispose();



                                            //}
                                            #endregion

                                            #region Code for Partial Acceptance (19/01/2014)


                                            dSet = GenerateAWBDimensionsAcceptance(AWBDetails[1], AcceptedPieces, Ds, AcceptedWeight, false, AWBDetails[0], flightno, flightdate);
                                            DClone.Tables.Add(dSet.Tables[0].Copy());
                                            DClone.Tables.Add(Ds.Tables[1].Copy());
                                            Ds = null;
                                            Ds = DClone;
                                            grdAcceptance.DataSource = Ds;
                                            grdAcceptance.DataBind();
                                            DClone.Dispose();
                                            //Session["dsDimesionAllAcceptance"] = dSet;
                                            Session["dsDimesionAllAcceptance"] = Ds;
                                            Session["dsDimesionAllAcceptanceSave"] = Ds;

                                            #endregion
                                            //
                                            //grdAcceptance.DataSource = dSet;
                                            //grdAcceptance.DataBind();


                                            //grdAcceptance.DataSource = Ds;
                                            //grdAcceptance.DataBind();
                                            for (int i = 0; i < grdAcceptance.Rows.Count; i++)
                                            {

                                                ((DropDownList)grdAcceptance.Rows[i].FindControl("ddlPieceType")).Text = Ds.Tables[0].Rows[i]["PieceType"].ToString();
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

                                            TXTTotal.Text = FinalTotal.ToString("0.00");
                                            TXTVolume.Text = FinalVolume.ToString("0.00");
                                            txtTotScaleWt.Text = FinalScaleWt.ToString("0.00");

                                            decimal TotalVolume = 0;

                                            if (TXTVolume.Text.Trim() != "")
                                                TotalVolume = Convert.ToDecimal(TXTVolume.Text.Trim());

                                            if (TotalVolume != 0)
                                            {
                                                txtMeterVolume.Text = (TotalVolume / 10000).ToString("0.00");
                                            }
                                            #endregion Calculate Total Vol,Weight and Scale Weight
                                        }
                                        if (Ds.Tables[1].Rows.Count > 0)
                                        {
                                            lblAWB.Text = awbnumber;
                                            lblCommodity.Text = Ds.Tables[1].Rows[0]["CommodityCode"].ToString();
                                            lblFlightDate.Text = Ds.Tables[1].Rows[0]["FltDate"].ToString();
                                            lblFlightNo.Text = Ds.Tables[1].Rows[0]["FltNumber"].ToString();
                                            //lblGrossWt.Text = Ds.Tables[1].Rows[0]["GrossWeight"].ToString();
                                            //LBLPcsCount.Text = Ds.Tables[1].Rows[0]["PiecesCount"].ToString();


                                        }

                                    }
                                }
                            }
                            #endregion
                        }

                        //if (Request.QueryString["FltDate"] != null)
                        //    lblFlightDate.Text = Convert.ToString(Request.QueryString["FltDate"]);

                        //Session["RowIndex"] = Request.QueryString["RowIndex"].ToString();

                        //Session["dsDimesionAll"] = GenerateAWBDimensions("", Convert.ToInt32(LBLPcsCount.Text.Trim()), (DataSet)Session["dsDimesionAll"], Convert.ToDecimal(lblGrossWt.Text.Trim()), false);

                        //LoadGridDimensionDetails();
                        try
                        {
                            LoginBL objBAL = new LoginBL();
                            string unit = "";
                            unit = objBAL.GetMasterConfiguration("MeasurmentUnit");
                            if (unit.Length > 0)
                            {
                                if (unit.Equals("MKS", StringComparison.OrdinalIgnoreCase))
                                {
                                    lblUnit.Text = "kg";
                                }
                                if (unit.Equals("FPS", StringComparison.OrdinalIgnoreCase))
                                {
                                    lblUnit.Text = "lbs";
                                }
                            }
                            Session["SupportBag"] = "true";
                            try
                            {
                                Session["SupportBag"] = objBAL.GetMasterConfiguration("SupportBagging").ToString();
                            }
                            catch (Exception ez) { }
                        }
                        catch (Exception ex) { }
                    }
                    catch (Exception ex)
                    { }
                }
            }
            catch (Exception ex)
            { }


        }

        //public void LoadGridDimensionDetails()
        //{

        //    #region Comment

        //    //DataTable myDataTable = new DataTable();
        //    //DataColumn myDataColumn;
        //    //DataSet Ds = new DataSet();

        //    //myDataColumn = new DataColumn();
        //    //myDataColumn.DataType = Type.GetType("System.String");
        //    //myDataColumn.ColumnName = "Length";
        //    //myDataTable.Columns.Add(myDataColumn);

        //    //myDataColumn = new DataColumn();
        //    //myDataColumn.DataType = Type.GetType("System.String");
        //    //myDataColumn.ColumnName = "Breadth";
        //    //myDataTable.Columns.Add(myDataColumn);

        //    //myDataColumn = new DataColumn();
        //    //myDataColumn.DataType = Type.GetType("System.String");
        //    //myDataColumn.ColumnName = "Height";
        //    //myDataTable.Columns.Add(myDataColumn);

        //    //myDataColumn = new DataColumn();
        //    //myDataColumn.DataType = Type.GetType("System.String");
        //    //myDataColumn.ColumnName = "PcsCount";
        //    //myDataTable.Columns.Add(myDataColumn);


        //    //DataRow dr;
        //    //dr = myDataTable.NewRow();
        //    //dr["Length"] = "";//"5";
        //    //dr["Breadth"] = "";// "5";
        //    //dr["Height"] = "";// "9";
        //    //dr["PcsCount"] = "";
        //    //myDataTable.Rows.Add(dr);

        //    //grdAcceptance.DataSource = null;
        //    //grdAcceptance.DataSource = myDataTable;
        //    //grdAcceptance.DataBind();

        //    //dsDimensions = new DataSet();
        //    //dsDimensions.Tables.Add(myDataTable.Copy());

        //    //Session["dsDimensions"] = dsDimensions.Copy();

        //    #endregion

        //    try
        //    {

        //        DataSet dsDimensionsTemp = (DataSet)Session["dsDimesionAll"];
        //        DataView dv = new DataView(dsDimensionsTemp.Tables[0].Copy());
        //        dv.RowFilter = "RowIndex = " + Session["RowIndex"];

        //        dsDimensions=new DataSet();
        //        dsDimensions.Tables.Add(dv.ToTable().Copy());

        //        if (dsDimensions.Tables[0].Rows.Count == 0)
        //        {
        //            dsDimensions.Tables[0].Rows.Add(dsDimensions.Tables[0].NewRow());
        //            dsDimensions.Tables[0].Rows[0]["RowIndex"] = Session["RowIndex"].ToString();

        //            grdAcceptance.DataSource = dsDimensions.Copy();
        //            grdAcceptance.DataBind();

        //            Session["dsDimensions"] = dsDimensions.Copy();
        //        }
        //        else
        //        {
        //            grdAcceptance.DataSource = dv.ToTable().Copy();
        //            grdAcceptance.DataBind();

        //            //ddlUnit.SelectedIndex = (dv.ToTable().Rows[0]["Units"].ToString() == "cms" ? 1 : 0);

        //            if (dv.ToTable().Rows[0]["Units"].ToString().Trim() == "")
        //                ddlUnit.SelectedIndex = 1;
        //            else
        //            ddlUnit.SelectedValue = dv.ToTable().Rows[0]["Units"].ToString();

        //            LBLVolumeUnit.Text = "Cubic " + ddlUnit.Text;
        //            //if (ddlUnit.SelectedIndex == 0)
        //            //    LBLVolumeUnit.Text = "cubic Inch";
        //            //else
        //            //    LBLVolumeUnit.Text = "cubic cm";

        //            Session["dsDimensions"] = dsDimensions.Copy();

        //            for (int i = 0; i < grdAcceptance.Rows.Count; i++)
        //            {
        //                ((DropDownList)grdAcceptance.Rows[i].FindControl("ddlPieceType")).Text = dv.ToTable().Rows[i]["PieceType"].ToString();
        //            }

        //            CalculateTotal();
        //        }              

        //    }
        //    catch (Exception ex)
        //    {
        //        LBLStatus.Text=""+ex.Message;
        //        return;
        //    }

        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Response.Redirect("FrmConBooking.aspx", false);
            Session["PieceDetails"] = null;
            ScriptManager.RegisterStartupScript(btnCancel, btnCancel.GetType(), "HidUnhide", "window.close();", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LBLStatus.Text = string.Empty;
                string ULDNumber = string.Empty;
                string Location = string.Empty;
                string ULDDestination = string.Empty;
                string ULDOrigin = string.Empty;
                #region Deepak Commented Code
                //string commcode = Session["commCode"].ToString(), shc = Session["SHC"].ToString();
                //for (int k = 0; k < grdAcceptance.Rows.Count; k++)
                //{
                //    if (((RadioButton)grdAcceptance.Rows[k].FindControl("radSelectAWB")).Checked == true)
                //    {
                //        commcode = ((DropDownList)grdAcceptance.Rows[k].FindControl("ddlCommCode")).SelectedItem.Text;
                //        shc = ((DropDownList)grdAcceptance.Rows[k].FindControl("ddlSHC")).SelectedItem.Text;
                //    }
                //}
                object[] QueryValues = (object[])Session["AWBSummaryDetails"];
                string awbnumber = QueryValues[0].ToString(); //Session["awbnumber"].ToString();
                string pccount;
                int length, breadth, height, AccpPcs = 0;
                decimal vol, scwt, wt, AccpWt = 0.00M;
                //bool tamper = chkTamper.Checked;
                //bool packaging = chkPackaging.Checked;
                //bool visual = chkVisual.Checked;
                //bool smell = chkSmell.Checked;
                //bool dgr = chkDGR.Checked;
                //bool liveanimnal = chkLiveAnimal.Checked;
                bool result;
                bool ULDPresent = false;

                string date = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy", null);

                object[] Params = new object[29];
                object[][] DetailsValues = new object[grdAcceptance.Rows.Count][];
                if (grdAcceptance.Rows.Count > 0)
                {
                    for (int j = 0; j < grdAcceptance.Rows.Count; j++)
                    {

                        if (((DropDownList)grdAcceptance.Rows[j].FindControl("ddlPieceType")).SelectedItem.Text == "ULD")
                        {
                            string ULDNum = "";
                            string station = "";
                            string ULDDest = "";
                            string ULDOrg = "";

                            if (((TextBox)grdAcceptance.Rows[j].FindControl("txtULD")).Text != "")
                            {
                                ULDPresent = true;
                                ULDNum = ((TextBox)grdAcceptance.Rows[j].FindControl("txtULD")).Text;
                                if (((TextBox)grdAcceptance.Rows[j].FindControl("txtLocation")).Text != "")
                                { station = ((TextBox)grdAcceptance.Rows[j].FindControl("txtLocation")).Text; }
                                else
                                {
                                    station = Session["Station"].ToString();
                                }
                                ULDDest = ((Label)grdAcceptance.Rows[j].FindControl("lblDestination")).Text;
                                ULDOrg = ((Label)grdAcceptance.Rows[j].FindControl("lblOrigin")).Text;
                            }
                            else
                            {
                                LBLStatus.Text = "Please enter the ULD number where PieceType is ULD!";
                                LBLStatus.ForeColor = Color.Red;
                                return;
                            }

                            //Added Code to check ULD Validation(19/01/2014)
                            #region Added Code to check ULD Validation
                            BLBuildULD objBuildULD = new BLBuildULD();
                            if (objBuildULD.CheckULDValidity(ULDNum) == false)
                            {
                                LBLStatus.Text = "ULD number format not valid";
                                LBLStatus.ForeColor = Color.Red;
                                return;
                            }
                            #endregion


                            ULDNumber += "$" + ULDNum;

                            Location += "$" + station;
                            ULDOrigin += "$" + ULDOrg;
                            ULDDestination += "$" + ULDDest;
                        }
                        //if (((CheckBox)grdAcceptance.Rows[j].FindControl("chkAccept")).Checked == true)
                        //{
                            AccpPcs = AccpPcs + 1;
                            try
                            {
                                if (lblUnit.Text == "kg" && Session["AWBUOM"].ToString() != "K")
                                {
                                    AccpWt += Convert.ToDecimal(float.Parse(((TextBox)grdAcceptance.Rows[j].FindControl("txtWt")).Text) / 2.20462);
                                    

                                }
                                else

                                    if (lblUnit.Text != "kg" && Session["AWBUOM"].ToString() == "K")
                                    {
                                        AccpWt += Convert.ToDecimal(float.Parse(((TextBox)grdAcceptance.Rows[j].FindControl("txtWt")).Text) * 2.20462);
                                    }
                                    else
                                    { AccpWt += Convert.ToDecimal(float.Parse(((TextBox)grdAcceptance.Rows[j].FindControl("txtWt")).Text)); }
                                
                            }
                            catch (Exception ex)
                            { }
                            //AccpWt = AccpWt + decimal.Parse(((TextBox)grdAcceptance.Rows[j].FindControl("txtWt")).Text);
                            Session["AccpPcs"] = AccpPcs;
                            Session["AccpWt"] = AccpWt;
                            decimal totwt = decimal.Parse(lblGrossWt.Text);
                            //if (AccpWt > totwt)
                            //{
                            //    LBLStatus.Text = "Accepted Weight cannot be greater than Total Weight...";
                            //    return;
                            //}
                        //}
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

                        //13 tamper
                        Params.SetValue(chkTamper.Checked, i);
                        i++;
                        //14 packaging
                        Params.SetValue(chkPackaging.Checked, i);
                        i++;
                        //15 visual
                        Params.SetValue(chkVisual.Checked, i);
                        i++;
                        //16 smell
                        Params.SetValue(chkSmell.Checked, i);
                        i++;

                        //17 dgr
                        Params.SetValue(chkDGR.Checked, i);
                        i++;

                        //18 liveanimnal
                        Params.SetValue(chkLiveAnimal.Checked, i);
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

                        //22
                        if (Session["DockNoFromDockAccp"] != null)
                        {
                            Params.SetValue(Session["DockNoFromDockAccp"].ToString(), i);

                        }
                        else
                        {
                            {
                                Params.SetValue("", i);
                            }

                        }
                        i++;

                        //23
                        Params.SetValue(QueryValues[3], i);
                        i++;

                        //24
                        Params.SetValue(QueryValues[4], i);
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

                        string FlightNo = string.Empty;
                        FlightNo = lblFlightNo.Text.Trim();
                        Params.SetValue(FlightNo, i);
                        i++;
                        //DateTime fDate;
                        string FlightDate = string.Empty;
                        FlightDate = lblFlightDate.Text.Trim();
                        //if (FlightDate != "")
                        //{

                        //}

                        Params.SetValue(FlightDate, i);
                        i++;

                        DetailsValues[j] = new object[29];
                        for (int k = 0; k < 29; k++)
                        {
                            DetailsValues[j][k] = Params[k];
                        }

                        //result = objBLL.SaveAccepanceData(Params);
                        //if (result == true)
                        //{
                        //    LBLStatus.Text = "Record Added Successfully...";
                        //    LBLStatus.ForeColor = Color.Green;
                        //    //ddlTokenList.Enabled = true;
                        //}


                    }
                #endregion
                    Session["PieceDetails"] = DetailsValues;
                    #region Added Code to keep session data(20/01/2014)

                    //Commented Code for Storing Session
                    //DataSet dsDim = (DataSet)Session["dsDimesionAllAcceptance"];
                    DataSet dsDim = (DataSet)Session["dsDimesionAllAcceptanceSave"];
                    for (int intCount = 0; intCount < grdAcceptance.Rows.Count; intCount++)
                    {
                        //dsDim.Tables[0].Rows[intCount]["Length"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtLgth")).Text.Trim();
                        try
                        {
                            if (((TextBox)grdAcceptance.Rows[intCount].FindControl("txtLgth")).Text.Trim() != "")
                            {
                                dsDim.Tables[0].Rows[intCount]["Length"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtLgth")).Text.Trim();
                            }
                            else
                            {
                                dsDim.Tables[0].Rows[intCount]["Length"] = 0;
                            }
                        }
                        catch (Exception ex)
                        { dsDim.Tables[0].Rows[intCount]["Length"] = 0; }

                        try
                        {
                            if (((TextBox)grdAcceptance.Rows[intCount].FindControl("txtBreadth")).Text.Trim() != "")
                            {
                                dsDim.Tables[0].Rows[intCount]["Breadth"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtBreadth")).Text.Trim();
                            }
                            else
                            {
                                dsDim.Tables[0].Rows[intCount]["Breadth"] = 0;
                            }
                        }
                        catch (Exception ex)
                        { dsDim.Tables[0].Rows[intCount]["Breadth"] = 0; }


                        //dsDim.Tables[0].Rows[intCount]["Breadth"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtBreadth")).Text.Trim();

                        try
                        {
                            if (((TextBox)grdAcceptance.Rows[intCount].FindControl("txtHeight")).Text.Trim() != "")
                            {
                                dsDim.Tables[0].Rows[intCount]["Height"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtHeight")).Text.Trim();
                            }
                            else
                            {
                                dsDim.Tables[0].Rows[intCount]["Height"] = 0;
                            }
                        }
                        catch (Exception ex)
                        { dsDim.Tables[0].Rows[intCount]["Height"] = 0; }

                        try
                        {
                            if (((TextBox)grdAcceptance.Rows[intCount].FindControl("txtWt")).Text.Trim() != "")
                            {
                                dsDim.Tables[0].Rows[intCount]["Wt"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtWt")).Text.Trim();
                            }
                            else
                            {
                                dsDim.Tables[0].Rows[intCount]["Wt"] = 0;
                            }
                        }
                        catch (Exception ex)
                        { dsDim.Tables[0].Rows[intCount]["Wt"] = 0; }


                        try
                        {
                            if (((TextBox)grdAcceptance.Rows[intCount].FindControl("txtVol")).Text.Trim() != "")
                            {
                                dsDim.Tables[0].Rows[intCount]["Vol"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtVol")).Text.Trim();
                            }
                            else
                            {
                                dsDim.Tables[0].Rows[intCount]["Vol"] = 0;
                            }
                        }
                        catch (Exception ex)
                        { dsDim.Tables[0].Rows[intCount]["Vol"] = 0; }
                        
                        //dsDim.Tables[0].Rows[intCount]["Wt"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtWt")).Text.Trim();
                        //dsDim.Tables[0].Rows[intCount]["Vol"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtVol")).Text.Trim();
                        dsDim.Tables[0].Rows[intCount]["PieceType"] = ((DropDownList)grdAcceptance.Rows[intCount].FindControl("ddlPieceType")).Text.Trim();
                        dsDim.Tables[0].Rows[intCount]["BagNo"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtBagNo")).Text.Trim();
                        dsDim.Tables[0].Rows[intCount]["ULDNo"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtULD")).Text.Trim();
                        //dsDim.Tables[0].Rows[intCount]["ScaleWeight"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtScaleWt")).Text.Trim();

                        try
                        {
                            if (((TextBox)grdAcceptance.Rows[intCount].FindControl("txtScaleWt")).Text.Trim() != "")
                            {
                                dsDim.Tables[0].Rows[intCount]["ScaleWeight"] = ((TextBox)grdAcceptance.Rows[intCount].FindControl("txtScaleWt")).Text.Trim();
                            }
                            else
                            {
                                dsDim.Tables[0].Rows[intCount]["ScaleWeight"] = 0;
                            }
                        }
                        catch (Exception ex)
                        { dsDim.Tables[0].Rows[intCount]["ScaleWeight"] = 0; }
                    }

                    //Code to Check for Dulplicate ULD's
                    if (ULDPresent)
                    {
                        if (CheckDuplicateULD(dsDim))
                        {
                            LBLStatus.Text = "Duplicate ULD Number Present in details.Please enter valid ULD Number.";
                            LBLStatus.ForeColor = Color.Red;
                            return;
                        }
                    }

                    Session["dsDimesionAllAcceptance"] = dsDim;

                    
                    dsDim.Dispose();
                    #endregion


                }
               
               
                //string msg = "Cargo Accepted for AWB" + Session["awbnumber"].ToString();
                //msg += "\nPCS:" + hdnPcsCount.Value.ToString() + "\nWT:" + hdnWt.Value.ToString() + "\nUder Token No:";
                //msg += hdnTokenNo.Value.ToString() + "\nDated:" + hdnTokenDt.Value.ToString();

                //cls_BL.addMsgToOutBox("SCM", msg, "", "");
                if (ULDNumber != "")
                {
                    ULDNumber = ULDNumber.Substring(1);
                }
                if (Location != "")
                {
                    Location = Location.Substring(1);
                }
                string[] ULDLoc = new string[4];
                ULDLoc[0] = ULDNumber;
                ULDLoc[1] = Location;
                ULDLoc[2] = ULDOrigin;
                ULDLoc[3] = ULDDestination;
                Session["TotalScaleWeight"] = txtTotScaleWt.Text.Trim();
                Session["ULDLoc"] = ULDLoc;
                Session["RemPcs"] = Convert.ToInt32(QueryValues[1]) - AccpPcs;
                Session["RemWt"] = Decimal.Round(Convert.ToDecimal(QueryValues[2]) - AccpWt, 2);
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + (Convert.ToInt32(QueryValues[1]) - AccpPcs).ToString() + "','" + Convert.ToString(Decimal.Round(Convert.ToDecimal(QueryValues[2]) - AccpWt,2)) + "','" + AccpPcs + "','" + Decimal.Round(AccpWt,2) + "');", true);
                //ScriptManager.RegisterStartupScript(btnCancel, btnCancel.GetType(), "HidUnhide", "window.close();", true);
            }
            catch (Exception ex)
            { }
        }


        //protected void btnAddRow_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SaveGrid();

        //        dsDimensions = (DataSet)Session["dsDimensions"];

        //        DataRow row = dsDimensions.Tables[0].NewRow();
        //        row["RowIndex"] = Session["RowIndex"].ToString();
        //        dsDimensions.Tables[0].Rows.Add(row);

        //        grdAcceptance.DataSource = dsDimensions.Tables[0];
        //        grdAcceptance.DataBind();

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
        //    }
        //}

        //public bool CheckTotalPcsCount()
        //{
        //    try
        //    {
                
        //        int count = 0;

        //        for (int i = 0; i < grdAcceptance.Rows.Count; i++)
        //        {
        //           count+= int.Parse(((TextBox)grdAcceptance.Rows[i].FindControl("txtPcs")).Text);
        //        }

        //        if (grdAcceptance.Rows.Count > 0 && count != int.Parse(LBLPcsCount.Text))
        //        {
        //            LBLStatus.Text = "Total Pcs count should be " + LBLPcsCount.Text;
        //            return false;
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //LBLStatus.Text = ""+ex.Message;
        //        return false;
        //    }

        //}

        //public void SaveGrid()
        //{
        //    try
        //    {
        //        dsDimensions = (DataSet)Session["dsDimensions"];

        //        //if (dsDimensions == null && Session["dsDimesionAll"] != null)
        //        //{
        //        //    dsDimensions = ((DataSet)Session["dsDimesionAll"]).Copy();
        //        //}

        //        for (int i = 0; i < grdAcceptance.Rows.Count; i++)
        //        {

        //            dsDimensions.Tables[0].Rows[i]["Length"] = ((TextBox)grdAcceptance.Rows[i].FindControl("txtLength")).Text;
        //            dsDimensions.Tables[0].Rows[i]["Breadth"] = ((TextBox)grdAcceptance.Rows[i].FindControl("txtBreadth")).Text;
        //            dsDimensions.Tables[0].Rows[i]["Height"] = ((TextBox)grdAcceptance.Rows[i].FindControl("txtHeight")).Text;
        //            dsDimensions.Tables[0].Rows[i]["PcsCount"] = ((TextBox)grdAcceptance.Rows[i].FindControl("txtPcs")).Text;
        //            dsDimensions.Tables[0].Rows[i]["RowIndex"] = Session["RowIndex"].ToString();
        //        }

        //        Session["dsDimensions"] = dsDimensions.Copy();

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
        //    }
        //}

        //public bool IsInputValid()
        //{
        //    try
        //    {
        //        int length = 0, breadth = 0, height = 0, pcscount = 0, totalpcscount=0;

        //        for (int i = 0; i < grdAcceptance.Rows.Count; i++)
        //        {
        //            try
        //            {
        //                length = int.Parse(((TextBox)grdAcceptance.Rows[i].FindControl("txtLgth")).Text);

        //            }
        //            catch
        //            {

        //                //LBLStatus.Text = "Enter valid length row:" + i;
        //                //LBLStatus.ForeColor = Color.Red;
        //                //((TextBox)grdAcceptance.Rows[i].FindControl("txtLength")).Text = "";
        //                ((TextBox)grdAcceptance.Rows[i].FindControl("txtLgth")).Focus();
        //                return false;
        //            }

        //            try
        //            {
        //                breadth = int.Parse(((TextBox)grdAcceptance.Rows[i].FindControl("txtBreadth")).Text);

        //            }
        //            catch
        //            {
        //                //LBLStatus.Text = "Enter valid breadth row:" + i;
        //                //LBLStatus.ForeColor = Color.Red;
        //                ((TextBox)grdAcceptance.Rows[i].FindControl("txtBreadth")).Text = "";
        //                ((TextBox)grdAcceptance.Rows[i].FindControl("txtBreadth")).Focus();
        //                return false;
        //            }
        //            try
        //            {
        //                height = int.Parse(((TextBox)grdAcceptance.Rows[i].FindControl("txtHeight")).Text);

        //            }
        //            catch
        //            {
        //                //LBLStatus.Text = "Enter valid height row:" + i;
        //                //LBLStatus.ForeColor = Color.Red;
        //                ((TextBox)grdAcceptance.Rows[i].FindControl("txtHeight")).Text = "";
        //                ((TextBox)grdAcceptance.Rows[i].FindControl("txtHeight")).Focus();
        //                return false;
        //            }
        //            try
        //            {
        //                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtPcs")).Text.Trim() == "")
        //                {
        //                    //LBLStatus.Text = "Fill pcscount for row:" + i;
        //                    //LBLStatus.ForeColor = Color.Red;
        //                    return false;
        //                }
        //                pcscount = int.Parse(((TextBox)grdAcceptance.Rows[i].FindControl("txtPcs")).Text);

        //            }
        //            catch
        //            {
        //                //LBLStatus.Text = "Enter valid pcscount row:" + i;
        //                //LBLStatus.ForeColor = Color.Red;
        //                ((TextBox)grdAcceptance.Rows[i].FindControl("txtPcs")).Focus();
        //                return false;
        //            }

        //            totalpcscount += pcscount;

        //            if (totalpcscount > int.Parse(LBLPcsCount.Text))
        //            {
        //                LBLStatus.Text = "Total pcs count should be smaller than " + LBLPcsCount.Text;
        //                LBLStatus.ForeColor = Color.Red;
        //                return false;
        //            }

        //        }

        //        LBLStatus.Text = "";
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //LBLStatus.Text = "Error : while checking input validation.";
        //        //LBLStatus.ForeColor = Color.Red;
        //        return false;
        //    }
        //}

        //protected void txtPcs_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if (!IsInputValid())
        //        {
        //            TXTVolume.Text = "";
        //            TXTTotal.Text = "";
        //            return;
        //        }


        //        SaveGrid();

        //        dsDimensions = (DataSet)Session["dsDimensions"];

        //        decimal FinalTotal = 0;
        //        decimal FinalVolume = 0;

        //        for (int i = 0; i < dsDimensions.Tables[0].Rows.Count; i++)
        //        {
        //            if (dsDimensions.Tables[0].Rows[i]["Length"].ToString().Trim() != "" && dsDimensions.Tables[0].Rows[i]["Breadth"].ToString().Trim() != "" && dsDimensions.Tables[0].Rows[i]["Height"].ToString().Trim() != "")
        //            {
                        
        //                decimal total=0,volume=0;

        //                total = Convert.ToDecimal(dsDimensions.Tables[0].Rows[i]["Length"].ToString().Trim()) *
        //                        Convert.ToDecimal(dsDimensions.Tables[0].Rows[i]["Breadth"].ToString().Trim()) *
        //                        Convert.ToDecimal(dsDimensions.Tables[0].Rows[i]["Height"].ToString().Trim())*
        //                        Convert.ToDecimal(dsDimensions.Tables[0].Rows[i]["PcsCount"].ToString().Trim());

        //                volume = total;

        //                if (ddlUnit.Text.Trim() == "cms")
        //                {
        //                    total = total / 6000;
                            
        //                }
        //                else
        //                {
        //                    total = total / 366;
        //                }


        //                FinalTotal += total;
        //                FinalVolume += volume;
        //            }
                    
        //        }


        //        if (("" + FinalTotal).Contains('.'))
        //            TXTTotal.Text = ("" + FinalTotal).Substring(0, ("" + FinalTotal).IndexOf('.') +2);
        //        else
        //            TXTTotal.Text = "" + FinalTotal;

        //        TXTTotal.Text = Convert.ToDecimal(TXTTotal.Text).ToString("0.0");

        //        if (TXTTotal.Text.ToString().IndexOf(".") > 0 || Convert.ToInt32(TXTTotal.Text.Substring(0, TXTTotal.Text.ToString().IndexOf(".")))>0.0)
        //        {
        //            decimal dcRound = Convert.ToDecimal(TXTTotal.Text) % Convert.ToInt32(TXTTotal.Text.Substring(0, TXTTotal.Text.ToString().IndexOf(".")));

        //            if (dcRound <= 0.5m)
        //            {
        //                decimal dc = 0.5m - dcRound;
        //                TXTTotal.Text = (Convert.ToDecimal(TXTTotal.Text) + dc).ToString();
        //            }
        //            else
        //            {
        //                decimal dc = 1 - dcRound;
        //                TXTTotal.Text = (Convert.ToDecimal(TXTTotal.Text) + dc).ToString();
        //            }
        //        }

                
        //        //TXTTotal.Text = Convert.ToDecimal(TXTTotal.Text).ToString("0");


        //        if (("" + FinalVolume).Contains('.'))
        //            TXTVolume.Text = ("" + FinalVolume).Substring(0, ("" + FinalVolume).IndexOf('.') + 2);
        //        else
        //            TXTVolume.Text = "" + FinalVolume;

        //        TXTVolume.Text = "" + FinalVolume;

        //    }
        //    catch (Exception ex)
        //    {
        //        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
        //    }
        //}

        //protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //if (ddlUnit.SelectedIndex == 0)
        //    //    LBLVolumeUnit.Text = "Cubic Inch";
        //    //else if (ddlUnit.SelectedIndex == 1)
        //    //    LBLVolumeUnit.Text = "Cubic CentiMeter";
        //    //else
        //    //    LBLVolumeUnit.Text = "Cubic Meter";

        //    LBLVolumeUnit.Text = "Cubic " + ddlUnit.Text.Trim();

        //    for (int rowindex = 0; rowindex < grdAcceptance.Rows.Count; rowindex++)
        //    {
        //        int Length = 0, Breadth = 0, Height = 0;
        //        decimal Volume = 0, Weight = 0;

        //        if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLength")).Text.Trim() != "")
        //            Length = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLength")).Text.Trim());

        //        if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim() != "")
        //            Breadth = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim());

        //        if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim() != "")
        //            Height = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim());

        //        Volume = Length * Breadth * Height;

        //        if (ddlUnit.SelectedIndex == 1) //CMS
        //        {
        //            Weight = Volume / 6000;

        //        }
        //        else if (ddlUnit.SelectedIndex == 0) //Inches
        //        {
        //            Weight = Volume / 366;
        //        }
        //        else //Meters
        //            Weight = Volume / 0.006m;

        //        ((Label)grdAcceptance.Rows[rowindex].FindControl("lblVolume")).Text = Volume.ToString("0.00");
        //        ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWeight")).Text = Weight.ToString("0.00");
        //    }
            
        //    CalculateTotal();
        //}

        //protected void btnDeleteRow_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        SaveGrid();

        //        dsDimensions = (DataSet)Session["dsDimensions"];
        //        DataSet newdsDimensions = dsDimensions.Clone();

        //        for (int i = 0; i < grdAcceptance.Rows.Count; i++)
        //        {
        //            if (!((CheckBox)grdAcceptance.Rows[i].FindControl("CHKSelect")).Checked)
        //            {
        //                DataRow row=newdsDimensions.Tables[0].NewRow();
        //                row["Length"] = "" + dsDimensions.Tables[0].Rows[i]["Length"].ToString();
        //                row["Breadth"] = "" + dsDimensions.Tables[0].Rows[i]["Breadth"].ToString();
        //                row["Height"] = "" + dsDimensions.Tables[0].Rows[i]["Height"].ToString();
        //                row["PcsCount"] = "" + dsDimensions.Tables[0].Rows[i]["PcsCount"].ToString();
        //                row["RowIndex"] = Session["RowIndex"].ToString();

        //                newdsDimensions.Tables[0].Rows.Add(row);
        //            }
        //        }

        //        //if (newdsDimensions.Tables[0].Rows.Count == 0)
        //        //{
        //        //    DataRow row = newdsDimensions.Tables[0].NewRow();
        //        //    newdsDimensions.Tables[0].Rows.Add(row);
        //        //}

        //        grdAcceptance.DataSource = newdsDimensions.Copy();
        //        grdAcceptance.DataBind();

        //        Session["dsDimensions"] = newdsDimensions.Copy();

        //        //txtPcs_TextChanged(sender, e);

        //    }catch(Exception ex)
        //    {
        //        //LBLStatus.Text = "" + ex.Message;
        //        return;
        //    }
        //}

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

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim() != "")
                    Length = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim() != "")
                    Breadth = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim() != "")
                    Height = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim());

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
                   //conversion for KG to lbs
                   if (lblUnit.Text.Equals("lbs", StringComparison.OrdinalIgnoreCase))
                   {
                       Weight = Weight * 2.20m;
                   }
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
            decimal FinalTotal = 0, FinalVolume = 0, FinalScaleWt = 0, Volume = 0, Weight = 0, ScaleWeight = 0;

            for (int i = 0; i < grdAcceptance.Rows.Count; i++)
            {
                Volume = 0; Weight = 0;

                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim() != "")
                    Volume = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim() != "")
                    Weight = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim() != "")
                    ScaleWeight = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim());

                FinalVolume = FinalVolume + Volume;
                FinalTotal = FinalTotal + Weight;
                FinalScaleWt = FinalScaleWt + ScaleWeight;
            }

            TXTTotal.Text = FinalTotal.ToString("0.00");
            TXTVolume.Text = FinalVolume.ToString("0.00");
            txtTotScaleWt.Text = FinalScaleWt.ToString("0.00");

            decimal TotalVolume = 0;

            if (TXTVolume.Text.Trim() != "")
                TotalVolume = Convert.ToDecimal(TXTVolume.Text.Trim());

            if (TotalVolume != 0)
                txtMeterVolume.Text = (TotalVolume / 10000).ToString("0.00");

            else
                txtMeterVolume.Text = "0.00";

        }
        //private DataSet GenerateAWBDimensions(string AWBNumber, int AWBPieces, DataSet Dimensions, decimal AWBWt, bool IsCreate)
        //{
        //    DataSet ds = null;
        //    BookingBAL BAL = new BookingBAL();

        //    ds = BAL.GenerateAWBDimensions(AWBNumber, AWBPieces, Dimensions, AWBWt, Convert.ToString(Session["UserName"]),
        //        Convert.ToDateTime(Session["IT"]), IsCreate);

        //    BAL = null;
        //    Dimensions = null;
        //    return ds;
        //}

        protected void CopyDimensions(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                Button button = (Button)sender;
                GridViewRow grRow = (GridViewRow)button.NamingContainer;
                rowindex = grRow.RowIndex;

                int Length = 0, Breadth = 0, Height = 0, RowCount=0;
                decimal Volume = 0, Weight = 0;
                string PieceType = string.Empty, BagNo = string.Empty, ULDNo = string.Empty, Location = string.Empty, FlightNo = string.Empty, FlightDt = string.Empty;

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim() != "")
                    Length = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim() != "")
                    Breadth = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim() != "")
                    Height = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtVol")).Text.Trim() != "")
                    Volume = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtVol")).Text.Trim());

                if(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWt")).Text.Trim() !="")
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

                CalculateTotal();
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
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

        private DataSet GenerateAWBDimensionsAcceptance(string AWBNumber, int AWBPieces, DataSet Dimensions, decimal AWBWt, bool IsCreate, string AWBPrefix, string FlightNo,string FlightDate)
        {
            DataSet ds = null;
            BookingBAL BAL = new BookingBAL();

            ds = BAL.GenerateAWBDimensionsAcceptance(AWBNumber, AWBPieces, Dimensions, AWBWt, Convert.ToString(Session["UserName"]),
                Convert.ToDateTime(Session["IT"]), IsCreate, AWBPrefix, "1",FlightNo,FlightDate);

            BAL = null;
            Dimensions = null;
            return ds;
        }

        private bool CheckDuplicateULD(DataSet ds)
        {
            bool flag = false;
            try
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    for (int j = (i + 1); j < ds.Tables[0].Rows.Count; j++)
                    {

                        if (ds.Tables[0].Rows[i]["ULDNo"].ToString().Length > 0 && ds.Tables[0].Rows[j]["ULDNo"].ToString().Length > 0)
                        {
                            if (ds.Tables[0].Rows[i]["ULDNo"].ToString().Equals(ds.Tables[0].Rows[j]["ULDNo"].ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                flag = true;
                                i = ds.Tables[0].Rows.Count;
                                j = ds.Tables[0].Rows.Count;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                flag = true;
            }
            return flag;
        }

        protected void grdAcceptance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                foreach (DataControlField col in grdAcceptance.Columns)
                {
                    if (Session["ULDACT"].ToString().ToUpper() == "FALSE" && col.HeaderText == "ULD#")
                    {
                        col.Visible = false;
                        ((DropDownList)e.Row.Cells[10].FindControl("ddlPieceType")).Items.Remove("ULD");
                    }
                    if (Session["SupportBag"].ToString().Equals("FALSE", StringComparison.OrdinalIgnoreCase) && col.HeaderText == "Bag#")
                    {
                        col.Visible = false;
                        ((DropDownList)e.Row.Cells[10].FindControl("ddlPieceType")).Items.Remove("Bags");
                    }
                }
                //if (Session["ULDACT"].ToString().ToUpper() == "FALSE")
                //{
                //    e.Row.Cells[12].Visible = false;
                //    ((DropDownList)e.Row.Cells[10].FindControl("ddlPieceType")).Items.Remove("ULD");
                //}
            }
            catch (Exception ex)
            {
            }
        }

        


        
    }
}