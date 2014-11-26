using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;

namespace MyKfCargo
{
    public partial class FrmTracerExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Cache["TracerDs"].ToString() != null || Cache["TracerDs"].ToString() != string.Empty)
                {
                DataSet oDs = new DataSet();//(DataSet)Cache["TracerDs"];
                //oDs.ReadXml(Server.MapPath("~/XMLTracer.xml"));
                oDs = (DataSet)Cache["TracerDs"];
                    try
                    {
                        if (oDs != null)
                        {
                            if (oDs.Tables[0].Rows.Count >= 1)
                            {

                                grdReports.DataSource = oDs;
                                grdReports.DataBind();
                                grdReports.FooterRow.Cells[1].Text = "GRAND TOTAL:";
                                grdReports.FooterRow.Cells[2].Text = oDs.Tables[0].Rows.Count.ToString();
                                grdReports.FooterRow.Font.Bold = true;
                                grdReports.FooterRow.HorizontalAlign = HorizontalAlign.Left;
                                grdReports.HeaderRow.HorizontalAlign = HorizontalAlign.Left;

                                System.IO.StringWriter tw = new System.IO.StringWriter();
                                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                                HtmlForm frm = new HtmlForm();
                                Response.Clear();
                                Response.AddHeader("content-disposition", "attachment;filename=Tracer.xls");
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.ms-excel";
                                hw.WriteLine("<br><center><b><u><font face = 'Calibri' color = #000000 size='4'>Tracer System.</font></u></b></center>");
                                grdReports.RowStyle.HorizontalAlign = HorizontalAlign.Left;
                                EnableViewState = false;
                                Controls.Add(frm);
                                frm.Controls.Add(grdReports);
                                //frm.Controls.Add(Image1)
                                frm.RenderControl(hw);
                                Response.Write(tw.ToString());
                                Response.Flush();
                                Response.Close();
                                Response.End();
                                grdReports.Columns.Clear();

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error'" + ex.Message + ");</script>");


                    }
                }
            }
        }

    }
}
