using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using System.Reflection;
using Microsoft.Reporting.WinForms;
using System.IO;
using System.Configuration;
using clsDataLib;

namespace QID_SCM_Alert_winSrcModified
{
    public partial class StationwiseReport : Form
    {
        #region Variables
        DataSet dsData = new DataSet();
        DataSet dsDataSummary = new DataSet();
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        string strfromdate = "", strtodate = "";
        ReportBAL objReport = new ReportBAL();
        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        #endregion


        public StationwiseReport()
        {
            InitializeComponent();
        }
        public StationwiseReport(DataSet ds, string strfrom, string strto)
        {
            //dsData = AddSortingField(ds);

           // dsDataSummary = CreateSummary();
            strfromdate ="01/10/2012"; //strfrom;
            strtodate = "15/10/2012";//strto;
            InitializeComponent();
        }


        private void StationwiseReport_Load(object sender, EventArgs e)
        {

            try
            {


                string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";

                DateTime dtTo = new DateTime(); DateTime dtfrom = new DateTime();
                strfromdate = "01/10/2012"; //strfrom;
                strtodate = "15/10/2012";

                ToDt = strtodate;

                string day = strfromdate.Substring(0, 2);
                string mon = strfromdate.Substring(3, 2);
                string yr = strfromdate.Substring(6, 4);
                frmDate = yr + "-" + mon + "-" + day;
                dtfrom = Convert.ToDateTime(frmDate);
                frmDate = dtfrom.ToString();
                string dayTo = strtodate.Substring(0, 2);
                string monTo = strtodate.Substring(3, 2);
                string yrTo = strtodate.Substring(6, 4);
                ToDt = yrTo + "-" + monTo + "-" + dayTo;
                dtTo = Convert.ToDateTime(ToDt);
                ToDt = dtTo.ToString();


                //AgentCode = "All"; PaymentType = "All"; contrLocatorCode = "All"; 
                //level = "All"; levelCode = "All";


                dsData = objReport.GetStationWiseAWBSummary(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom, dtTo, AWBStatus);


                if (dsData != null)
                {
                    if (dsData.Tables[0].Rows.Count > 0)
                    {

                    

                        strfromdate = dtfrom.ToString();

                        reportViewer1.ProcessingMode = ProcessingMode.Local;

                        LocalReport rep2 = reportViewer1.LocalReport;

                        // rep1.ReportPath = "D:/";//Server.MapPath("/Reports/AWB.rdlc");
                        rep2.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptStationwiseReport.rdlc";
                        rds1.Name = "dsStationwisedata_DataTable1";// "dsShowEAWB";// "EMULDAWB_dtManifest";
                        rds1.Value = dsData.Tables[0];  
                        rep2.DataSources.Add(rds1);


                        reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);


                        Warning[] warnings = null;
                            //+ConfigurationManager.AppSettings["totime"].ToString()
                       string filepath = "D://" +"StationReport" + "_" + DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy") + "." + "xls";//"D://carreport_25-10-2010.xls";
                   
                       // string filepath = "D://" +"StationReport" + "_" + DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy") + "." + "xls";//"D://carreport_25-10-2010.xls";
                        File.Delete(filepath);
                        byte[] bytes;
                        string mimeType = "";
                        string encoding = "";
                        string extension = "";
                        string[] streamids = { "" };


                        if (filepath.Contains("xls"))
                            bytes = reportViewer1.LocalReport.Render("EXCEL", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                        else
                            bytes = reportViewer1.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);

                        FileStream fls = new FileStream(filepath, FileMode.Create);
                        fls.Write(bytes, 0, bytes.Length);

                        fls.Close();



                        #region Comment
                        /*                    
                        Warning[] warnings = null;
                        string filepath = "D://carreport.xls";
                        File.Delete(filepath);
                        byte[] bytes;
                        string mimeType = "";
                        string encoding = "";
                        string extension = "";
                        string[] streamids = {""};
                       

                        bytes = reportViewer1.LocalReport.Render("EXCEL","",out mimeType,out encoding,out extension,out streamids,out warnings);

                        FileStream fls = new FileStream(filepath, FileMode.Create);
                        fls.Write(bytes,0,bytes.Length);

                        fls.Close();
                 */

                        #endregion

                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Exception Occured in Sending Mail : " + ex.Message.ToString() + " @ " + DateTime.Now.ToString());

            }

            this.reportViewer1.RefreshReport();
            
        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsStationdata_DataTable2", dtTable2));

        }

    }
}
