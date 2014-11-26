using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using System.Text;
using System.Drawing; 

namespace ProjectSmartCargoManager
{
    //19-6-2012
    //12-7-2012
    public partial class PrintDeliveryForm : System.Web.UI.Page
    {
        int incr=0;
        ClsImageConversion ImgConversion = new ClsImageConversion();
        BLArrival ObjArr = new BLArrival();
        private List<Point> m_listPoints = new List<Point>();
        private Graphics m_gfxCanvas;
        static private Pen m_penLine = new Pen(Color.Black);
        Graphics M_Graphics;
        Plotter ObjPlotter;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtagent = new DataTable();
        DateTime DtCurrentDate = DateTime.Now;
        MasterBAL objBal = new MasterBAL();
        double ST = 0.0;

        #region Sessiondata
        public void 
            sessiondata(int ro)
        {
            try
            {                
                dt = (DataTable)Session["abc"];
                  
                if (ro >= dt.Rows.Count)
                {
                    return;
                }

                //lblawbno.Text = dt.Rows[ro][1].ToString();
                lblnohawb.Text = dt.Rows[ro][2].ToString();
                //lblflightno.Text = dt.Rows[ro][3].ToString();
                string IssuedTo = dt.Rows[ro][12].ToString();
                
                    lblissuedto.Text = dt.Rows[ro][4].ToString();
              
                    lblissuename.Text = IssuedTo;
                
                lblissuename.Text = dt.Rows[ro][5].ToString();
                lblreciversname.Text = dt.Rows[ro][6].ToString();
                lbldnno.Text = dt.Rows[ro][0].ToString();
               // lbldnpieces.Text = dt.Rows[ro][7].ToString();
                //lbldnweight.Text =dt.Rows[ro][8].ToString();
                //lblflightdate.Text = dt.Rows[ro][9].ToString();
                //lblflightdate.Text = Convert.ToDateTime(dt.Rows[ro][9].ToString()).ToString("dd-MMM-yyyy");
                lblagentname.Text=dt.Rows[ro][10].ToString() ;


                (((Label)grdeliveryOrder.Rows[0].FindControl("lblconsigneeNo")).Text) = dt.Rows[ro][1].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblnopkgs")).Text) = dt.Rows[ro][7].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblweight")).Text) = dt.Rows[ro][8].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblfltno")).Text) = dt.Rows[ro][3].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblfltdate")).Text) = dt.Rows[ro][9].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lbldiscription")).Text) = dt.Rows[ro][11].ToString();
                txtcharge.Text = dt.Rows[ro][13].ToString();
                lbladdlhandling.Text = dt.Rows[ro][15].ToString();

                lblcc.Text = dt.Rows[ro][14].ToString();
                int totalpcs = 0;
                int totalweight = 0;
                
                for(int i=0;i<dt.Rows.Count;i++)  
                {
                    totalpcs = totalpcs + Convert.ToInt16(dt.Rows[i][7].ToString());

                    lblgrandtotal.Text = Convert.ToString(totalpcs);

                    totalweight = totalweight + Convert.ToInt32(dt.Rows[i][8].ToString());    
                }
               

               

                //Hide Next button if there are no further records..
                if (ro + 1 == dt.Rows.Count)
                {
                    btnnext.Visible = false;
                }
                else
                {
                    btnnext.Visible = true;
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion Sessiondata
        #region TrackingDo
        public void TrackingDo(int ro)
        {
            try
            {
                dt = (DataTable)Session["DoPrintTracking"];
                if (ro >= dt.Rows.Count)
                {
                    return;
                }

                //lblawbno.Text = dt.Rows[ro][1].ToString();
                lblnohawb.Text = dt.Rows[ro]["HAWBNumber"].ToString();
                //lblflightno.Text = dt.Rows[ro][2].ToString();
                lblissuedto.Text = dt.Rows[ro]["IssuedTo"].ToString();
                lblissuename.Text = dt.Rows[ro]["IssueName"].ToString();
                lblreciversname.Text = dt.Rows[ro]["ReciversName"].ToString();
                lblagentname.Text = dt.Rows[ro]["AgentName"].ToString();
                // lbldnpieces.Text = dt.Rows[ro][3].ToString();
                lbldnno.Text = dt.Rows[ro]["DONumber"].ToString();
                lbladdlhandling.Text = dt.Rows[ro]["Desc"].ToString();

                (((Label)grdeliveryOrder.Rows[0].FindControl("lblconsigneeNo")).Text) = dt.Rows[ro]["AWBNumber"].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblnopkgs")).Text) = dt.Rows[ro]["TotalPieces"].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblweight")).Text) = dt.Rows[ro]["ActualWeight"].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblfltno")).Text) = dt.Rows[ro]["FlightNumber"].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblfltdate")).Text) = Convert.ToDateTime(dt.Rows[ro]["FltDate"]).ToString("dd/MM/yyyy");
                (((Label)grdeliveryOrder.Rows[0].FindControl("lbldiscription")).Text) = dt.Rows[ro]["Desc"].ToString();
                lblcc.Text = dt.Rows[ro]["PType"].ToString();
                lblpaymentmode.Text = dt.Rows[ro]["PType"].ToString();
                txtcharge.Text = dt.Rows[ro]["CCAmount"].ToString();

                string AWB = dt.Rows[ro]["AWBNumber"].ToString();
                DataSet ds= ObjArr.GetSignature(AWB);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    System.Drawing.Image img;// = new System.Drawing.Image();
                    byte[] objbyte = new byte[0];
                    int i = 0;
                    
                    string Sign=ds.Tables[0].Rows[0]["Sign"].ToString();
                    //objbyte.SetValue(Sign,i);
                    StringtoImage(Sign);
                    objbyte =Encoding.ASCII.GetBytes(Sign);
                    
                   // Byte[] bytes = (Byte[])ds.Tables[0].Rows[0]["Sign"];
                    //--------------------------------------A--------------------
           


                    //-------------------------------------------------------------
                    //img = ImgConversion.byteArrayToImage(objbyte);
                    //Response.Buffer = true;
                    //Response.Charset = "";
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.ContentType = dt.Rows[0]["ContentType"].ToString();
                    //Response.AddHeader("content-disposition", "attachment;filename="
                    //+ dt.Rows[0]["Name"].ToString());
                    //Response.BinaryWrite(bytes);
                    //Response.Flush();
                    //Response.End();

                }             
                //lblflightdate.Text = dt.Rows[ro][9].ToString();
                //lblflightdate.Text = Convert.ToDateTime(dt.Rows[ro][9].ToString()).ToString("dd-MMM-yyyy");
                //Hide Next button if there are no further records..

                if (ro + 1 == dt.Rows.Count)
                {
                    btnnext.Visible = false;
                }
                else
                {
                    btnnext.Visible = true;
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion TrackingDo

        #region testing mode
        public void StringtoImage(string signature)
        {
            try
            {
                string path = "~/Images/" + " " + System.DateTime.Now.ToString("MM-dd-yyyy") + ".bmp";
                //if (File.Exists(Server.MapPath(path)))
                //{
                //    //string Imgfile = Server.MapPath(path);
                //    //FileAttributes attributes = File.GetAttributes(Imgfile);

                //    //if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                //    //    File.SetAttributes(Imgfile, attributes ^ FileAttributes.ReadOnly);
                //    //File.Delete(Server.MapPath(path));
                //}
                //else
                //{



                    float xTo = 0;
                    float xFrom = 0;
                    float yTo = 0;
                    float yFrom = 0;

                    //string signature = "79,173,78,175,79,173,80,171,80,169,81,166,82,162,82,160,83,156,83,154,84,150,84,148,84,145,84,141,84,138,84,135,83,133,83,130,83,128,82,126,82,124,81,120,79,117,77,114,75,113,73,116,74,119,76,122,78,125,80,128,82,131,85,134,88,137,90,140,93,142,95,145,97,148,99,152,100,155,100,158,99,160,97,163,94,165,92,165,89,164,87,164,85,163,83,162,80,159,78,157,77,153,77,149,76,142,77,135,77,129,78,120,79,115,80,112,81,109,81,107,82,109,81,112,81,114,80,122,81,134,97,134,92,141,92,144,92,146,94,149,96,147,97,145,97,143,97,140,97,138,97,140,99,140,101,140,102,138,103,136,103,134,103,132,103,127,102,125,101,119,101,114,100,111,100,114,100,117,102,123,105,128,108,133,109,136,110,138,110,138;104,131,106,129,108,126,109,124,110,121,110,119,110,116,110,114,110,112,109,109,110,112,110,114,112,116,113,118,115,121,117,124,119,126,120,128,123,128,125,127,127,125,129,124,131,122,133,121,135,119,136,117,135,114,133,113,131,113,129,116,129,118,129,120,130,122,132,121,134,118,135,115,136,113,136,110,136,107,136,105,135,102,135,100,134,98,133,96,133,98,133,101,133,103,134,105,135,108,135,112,137,117,138,119,139,121,139,121;103,174,105,172,107,169,109,166,112,162,113,160,115,157,117,155,119,153,121,150,124,147,126,144,128,141,131,138,133,135,135,132,137,129,139,127,140,125,141,123,143,121,145,118,148,117,150,116,152,117,152,117";
                    //string []signcods=strsign .Split (',');
                    List<Point> seriesOfPoints = new List<Point>();
                    Pen m_penLine = new Pen(Color.Black, 2);

                    Bitmap b = new Bitmap(500, 500);

                    Graphics G = Graphics.FromImage(b);
                    G.Clear(Color.White);



                    string[] signParts = signature.Split(';');//collect all dotSignFile-parts as strings

                    foreach (string signPart in signParts)
                    {
                        string[] arrPoints = signPart.Split(',');//collect all x\y numbers as strings

                        for (int i = 1; i < arrPoints.Length; )
                        {
                            string strX = arrPoints[i - 1];
                            string strY = arrPoints[i];
                            i = i + 2;

                            Point p = new Point(Convert.ToInt32(strX), Convert.ToInt32(strY));
                            seriesOfPoints.Add(p);
                        }


                        for (int i = 1; i < seriesOfPoints.Count; i++)
                        {
                            try
                            {
                                //string strX = arrPoints[i - 1];
                                //string strY = arrPoints[i];
                                //i = i + 2;


                                //co-ordinates of starting point
                                xFrom = seriesOfPoints[i - 1].X;
                                yFrom = seriesOfPoints[i - 1].Y;
                                //co-ordinates of ending point
                                xTo = seriesOfPoints[i].X;
                                yTo = seriesOfPoints[i].Y;
                                G.DrawLine(m_penLine, (int)xTo, (int)yTo, (int)xFrom, (int)yFrom);
                            }
                            catch (Exception)
                            {
                                int errorline = i;
                            }
                        }

                    }
                    G.Dispose();


                    b.Save(Server.MapPath(path), System.Drawing.Imaging.ImageFormat.Bmp);
               // }





                Image2.ImageUrl = path;



            }
            catch (Exception ex)
            {
                // MessageBox.Show(Ex.Message, "test", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            }
        }
        #endregion
        #region DeliverySession
        public void Deliverysessiondata(int ro)
        {
            try
            {
                dt = (DataTable)Session["DeliveryDetails"];
                if (ro >= dt.Rows.Count)
                {
                    return;
                }

                //lblawbno.Text = dt.Rows[ro][1].ToString();
                lblnohawb.Text = dt.Rows[ro][8].ToString();
                //lblflightno.Text = dt.Rows[ro][2].ToString();
                lblissuedto.Text = dt.Rows[ro][6].ToString();
                lblissuename.Text = dt.Rows[ro][5].ToString();
                lblreciversname.Text = dt.Rows[ro][10].ToString();
                lblagentname.Text = dt.Rows[ro][0].ToString();
               // lbldnpieces.Text = dt.Rows[ro][3].ToString();
                lbldnno.Text = dt.Rows[ro][13].ToString();
                lbladdlhandling.Text = dt.Rows[ro][14].ToString();

                (((Label)grdeliveryOrder.Rows[0].FindControl("lblconsigneeNo")).Text) = dt.Rows[ro][1].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblnopkgs")).Text) = dt.Rows[ro][3].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblweight")).Text) = dt.Rows[ro][4].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblfltno")).Text) = dt.Rows[ro][2].ToString();
                (((Label)grdeliveryOrder.Rows[0].FindControl("lblfltdate")).Text) = Convert.ToDateTime(dt.Rows[ro]["FltDate"]).ToString("dd/MM/yyyy")  ;
                (((Label)grdeliveryOrder.Rows[0].FindControl("lbldiscription")).Text) = dt.Rows[ro][14].ToString();
                lblcc.Text = dt.Rows[ro][16].ToString();
                lblpaymentmode.Text = dt.Rows[ro][16].ToString();
                txtcharge.Text = dt.Rows[ro][15].ToString();  
                    
                    

                //lblflightdate.Text = dt.Rows[ro][9].ToString();
                //lblflightdate.Text = Convert.ToDateTime(dt.Rows[ro][9].ToString()).ToString("dd-MMM-yyyy");
                //Hide Next button if there are no further records..

                if (ro + 1 == dt.Rows.Count)
                {
                    btnnext.Visible = false;
                }
                else
                {
                    btnnext.Visible = true;
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion DeliverySession

        #region LoadGrid
        public void LoadGrid()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("AWBNumber");
            dt.Columns.Add("ActualPieces");
            dt.Columns.Add("ActualWeight");
            dt.Columns.Add("FlightNumber");
            dt.Columns.Add("IssueDate");
            dt.Columns.Add("Discription");  

            DataRow row;
            row = dt.NewRow();
            row["AWBNumber"] = "";
            row["ActualPieces"] = "";
            row["ActualWeight"] = "";
            row["FlightNumber"] = "";
            row["IssueDate"] = "";
            row["Discription"] = ""; 

            dt.Rows.Add(row);


            grdeliveryOrder.DataSource = null;
            grdeliveryOrder.DataSource = dt.Copy();
            grdeliveryOrder.DataBind();

            Session["dtDeliveryOrder"] = dt.Copy();

        }

        #endregion LoadGrid
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ST"] != null)
                ST = Convert.ToDouble(Session["ST"].ToString());
            else
            {
                Session["ST"] = objBal.getServiceTax();
                ST = Convert.ToDouble(Session["ST"].ToString());
            }

            txtservicetax.Text = ST.ToString();

            DtCurrentDate = (DateTime)Session["IT"];

            if (!IsPostBack)
            {
                try
                {
                    LoadGrid();
                    if (Session["Agent"] != null)
                    {
                        lblagentname.Text = Session["Agent"].ToString();
                    }
                    lblstaffid.Text = Session["UserName"].ToString();
                    //lblawbno.Text=   Session["AWBNO"].ToString() ;
                   
                //Session["Agent"] = txtAgentName.Text;
                //Session["FligtNo"] = DdlFlightno.SelectedItem.Text.Trim();
                //Session["AWBNO"] = txtAWBNo.Text;
                //Session["Flightdate"]
                    //if (Session["Fltdate"] != null)
                    //{
                    //    lblflightdatetime.Text=Session["Fltdate"].ToString();
                    //}
                    //else if (Session["Flightdate"]!=null)
                    //{
                    //     lblflightdatetime.Text=Session["Flightdate"].ToString() ;
                    //}

                    //if (Session["FligtNo"] != null)
                    //{
                    //    lblflightid.Text = Session["FligtNo"].ToString();
                    //}

                    //else if (Session["flight"]!=null)
                    //{
                    //   lblflightid.Text= Session["flight"].ToString(); 
                    //}



                    if (Session["abc"] != null)
                    {
                        dt = (DataTable)Session["abc"];
                        lblstation.Text = Session["Station"].ToString();
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                        lbldate.Text = dtIndianTime.ToString(); 
                        sessiondata(incr);
                        //Session["abc"] = null;
                    }
                    else if (Session["DeliveryDetails"] != null)
                    {
                        dt1 = (DataTable)Session["DeliveryDetails"];
                        lblstation.Text = Session["Station"].ToString();
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                        lbldate.Text = dtIndianTime.ToString(); 
                        Deliverysessiondata(incr);
                    
                        lblstaffid.Text = Session["UserName"].ToString();
                       // Session["DeliveryDetails"] = null;
                    }
                    else if (Session["DoPrintTracking"] != null)
                    {
                        dt1 = (DataTable)Session["DoPrintTracking"];
                        lblstation.Text = Session["Station"].ToString();
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                        lbldate.Text = dtIndianTime.ToString(); 
                        TrackingDo(incr);

                        lblstaffid.Text = Session["UserName"].ToString();
                    
                    }
                    else
                    {
                        lblStatus.Text = "Error";
                    }
                }
                catch (Exception)
                {
                    lblStatus.Text="Error";  
                }

                AutopopulateData();
            }
        }
        #endregion PageLoad
        #region Print
        protected void btnprint_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "printpage();", true);
            //btnClear.Visible = false;
            //btnnext.Visible = false;
            //btnprint.Visible = false;
            //lblCopy.Visible = true;
            ////Show lblCopy
            //for (int i = 0; i < 2; i++)
            //{
            //    switch (i)
            //    {
            //        case 0:
            //            lblCopy.Text = "(Agent Copy)";
            //            break;
            //        case 1:
            //            lblCopy.Text = "(JET Copy)";
            //            break;
            //    }
            //    ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "printpage();", true);
            //}
        }
        #endregion Print
        #region  Next
        protected void btnnext_Click(object sender, EventArgs e)
        {
            try
            {
                AutopopulateData();
                incr = incr + 1;
                sessiondata(incr);
                Deliverysessiondata(incr);
            }
            catch (Exception ex)
            { }
        }
        #endregion Next

        #region AgentNames
        public void AgentData(int ro2)
        {
            dtagent = (DataTable)Session["AgentId"];

            if (ro2 >= dtagent.Rows.Count)
            {
                return;
            }
           
        }
        #endregion Agentnames
        #region Autopopulate
        public void AutopopulateData()
        {
            try
            {
                string agentcode = Session["CurrentAgent"].ToString();
                if (agentcode == "")
                {
                    DataTable ds = (DataTable)Session["abc"];
                    DataTable dstemp = ds.Clone();

                    agentcode = ds.Rows[0][10].ToString();
                    foreach (DataRow row in ds.Rows)
                    {
                        if (row[10].ToString() == agentcode)
                        {
                            DataRow rw = dstemp.NewRow();
                            for (int i = 0; i < dstemp.Columns.Count; i++)
                            {
                                rw[i] = row[i];

                            }
                            dstemp.Rows.Add(rw);
                        }
                        grdeliveryOrder.DataSource = dstemp;
                        grdeliveryOrder.DataBind();
                    }


                }
                else
                {

                    //DataTable ds = (DataTable)Session["abc"];
                    //DataTable dstemp = ds.Clone();

                    //agentcode = ds.Rows[0][10].ToString();
                    //foreach (DataRow row in ds.Rows)
                    //{
                    //    if (row[10].ToString() == agentcode)
                    //    {
                    //        DataRow rw = dstemp.NewRow();
                    //        for (int i = 0; i < dstemp.Columns.Count; i++)
                    //        {
                    //            rw[i] = row[i];

                    //        }
                    //        dstemp.Rows.Add(rw);
                    //    }
                    //    grdeliveryOrder.DataSource = dstemp;
                    //    grdeliveryOrder.DataBind();


                    //}
                }
            }
            catch (Exception ex) { }

        }
        #endregion Autopopulate
    }
}
