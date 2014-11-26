using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Reporting.WinForms;
using System.IO;
using System.Configuration;
using clsDataLib;
using System.Data ;


namespace QID_SCM_Alert_winSrcModified
{
    public partial class AgentPerformanceReport : Form
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

        public AgentPerformanceReport()
        {
            InitializeComponent();
        }
        public AgentPerformanceReport(string strfrom, string strto)
        {
            //dsData = AddSortingField(ds);

           // dsDataSummary = CreateSummary();
            strfromdate ="01/10/2012"; //strfrom;
            strtodate = "15/10/2012";//strto;
            InitializeComponent();
        }

        private void AgentPerformanceReport_Load(object sender, EventArgs e)
        {


            try
            {

                string fromdt = "01/11/2012";
                string todt = "29/11/2012";
                string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";

                DateTime dtTo = new DateTime(); DateTime dtfrom = new DateTime();

                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                firstDay = firstDay.AddDays(-(firstDay.Day - 1));


                strfromdate = fromdt;//firstDay.ToString("dd/MM/yyyy"); //strfrom;
                strtodate = todt;//DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");


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

                AgentCode = "All"; PaymentType = "All"; contrLocatorCode = "All";
                level = "All"; levelCode = "All";


                dsData = objReport.GetAgentWiseAWBSummary(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom, dtTo, AWBStatus);


                if (dsData != null)
                {
                    if (dsData.Tables[0].Rows.Count > 0)
                    {



                        strfromdate = dtfrom.ToString();

                        reportViewer1.ProcessingMode = ProcessingMode.Local;

                        LocalReport rep2 = reportViewer1.LocalReport;

                        // rep1.ReportPath = "D:/";//Server.MapPath("/Reports/AWB.rdlc");
                        rep2.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptAgentwiseDataset.rdlc";
                        rds1.Name = "rptAgentwiseData_DataTable1";// "dsShowEAWB";// "EMULDAWB_dtManifest";
                        rds1.Value = dsData.Tables[0];
                        rep2.DataSources.Add(rds1);
                        //DataSet dsShowData = showSearchCriteria(dtfrom.ToShortDateString(), dtTo.ToShortDateString());
                        //dtTable2 = dsShowData.Tables[0];



                        //   reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);


                        Warning[] warnings = null;
                         string filepath = ConfigurationManager.AppSettings["FileLoc"].ToString() + " \\" + "Agent" + "_" + fromdt.Replace('/', '-') + " To " + todt.Replace('/', '-') + "." + "xls";

                        //string filepath = "D:\\Reports\\" + "AgentReport" + "_" + fromdt.Replace('/', '-') + "To" + todt.Replace('/', '-') + "." + "xls";
                        File.Delete(filepath);
                        byte[] bytes;
                        string mimeType = "";
                        string encoding = "";
                        string extension = "";
                        string[] streamids = { "" };


                        if (filepath.Contains("xls"))
                            bytes = reportViewer1.LocalReport.Render("EXCEL","", out mimeType, out encoding, out extension, out streamids, out warnings);
                        else
                            bytes = reportViewer1.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);

                        FileStream fls = new FileStream(filepath, FileMode.Create);
                        fls.Write(bytes, 0, bytes.Length);

                        fls.Close();
                        
                        //string  destfilepath = ConfigurationManager.AppSettings["FileLoc"].ToString() + " \\" + "AgentReport" + "_" + fromdt.Replace('/', '-') + "To" + todt.Replace('/', '-') + "." + "xlsx";
                        //File.re(filepath, destfilepath);
                        
                       


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


            }

            this.reportViewer1.RefreshReport();

        }
      
        public void LoadReport(string fromdt,string todt)
        {

            try
            {


                string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";

                DateTime dtTo = new DateTime(); DateTime dtfrom = new DateTime();
              
                 DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                firstDay = firstDay.AddDays(-(firstDay.Day - 1));


                strfromdate = fromdt;//firstDay.ToString("dd/MM/yyyy"); //strfrom;
                strtodate = todt;//DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");


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

                AgentCode = "All"; PaymentType = "All"; contrLocatorCode = "All";
                level = "All"; levelCode = "All";


                dsData = objReport.GetAgentWiseAWBSummary(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom, dtTo, AWBStatus);


                if (dsData != null)
                {
                    if (dsData.Tables[0].Rows.Count > 0)
                    {



                        strfromdate = dtfrom.ToString();

                        reportViewer1.ProcessingMode = ProcessingMode.Local;

                        LocalReport rep2 = reportViewer1.LocalReport;

                        // rep1.ReportPath = "D:/";//Server.MapPath("/Reports/AWB.rdlc");
                        rep2.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptAgentwiseDataset.rdlc";
                        rds1.Name = "rptAgentwiseData_DataTable1";// "dsShowEAWB";// "EMULDAWB_dtManifest";
                        rds1.Value = dsData.Tables[0];
                        rep2.DataSources.Add(rds1);
                        //DataSet dsShowData = showSearchCriteria(dtfrom.ToShortDateString(), dtTo.ToShortDateString());
                        //dtTable2 = dsShowData.Tables[0];



                        //   reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);


                        Warning[] warnings = null;
                       // string filepath = "D://" + "AgentReport" + "_" + DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy") + "." + "xls";//ConfigurationManager.AppSettings["extention"].ToString();//"D://carreport_25-10-2010.xls";
                        //+ConfigurationManager.AppSettings["totime"].ToString()
                        string filepath = ConfigurationManager.AppSettings["FileLoc"].ToString() + " \\" + "Agent" + "_" + fromdt.Replace('/', '-') + " To " + todt.Replace('/', '-') + "." + "xls";
                         
                        //string filepath = "D:\\Reports\\" + "AgentReport" + "_" + fromdt.Replace('/', '-') + "To" + todt.Replace('/', '-') + "." + "xls";
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


        public void LoadCurrentDayReport(string fromdt, string todt)
        {
            try
            {


                string AgentCode = "All", PaymentType = "All", contrLocatorCode = "All", level = "All", levelCode = "All", frmDate = "", ToDt = "", AWBStatus = "All";

                DateTime dtTo = new DateTime(); DateTime dtfrom = new DateTime();

                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                firstDay = firstDay.AddDays(-(firstDay.Day - 1));


                strfromdate = fromdt;//firstDay.ToString("dd/MM/yyyy"); //strfrom;
                strtodate = todt;//DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");


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

                AgentCode = "All"; PaymentType = "All"; contrLocatorCode = "All";
                level = "All"; levelCode = "All";


                dsData = objReport.GetAgentWiseAWBSummary(AgentCode, PaymentType, contrLocatorCode, level, levelCode, dtfrom, dtTo, AWBStatus);


                if (dsData != null)
                {
                    if (dsData.Tables[0].Rows.Count > 0)
                    {



                        strfromdate = dtfrom.ToString();

                        reportViewer1.ProcessingMode = ProcessingMode.Local;

                        LocalReport rep2 = reportViewer1.LocalReport;

                        // rep1.ReportPath = "D:/";//Server.MapPath("/Reports/AWB.rdlc");
                        rep2.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptAgentwiseDataset.rdlc";
                        rds1.Name = "rptAgentwiseData_DataTable1";// "dsShowEAWB";// "EMULDAWB_dtManifest";
                        rds1.Value = dsData.Tables[0];
                        rep2.DataSources.Add(rds1);
                        //DataSet dsShowData = showSearchCriteria(dtfrom.ToShortDateString(), dtTo.ToShortDateString());
                        //dtTable2 = dsShowData.Tables[0];



                        //   reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);


                        Warning[] warnings = null;
                      //  string filepath = "D://" + "AgentReport" + "_" + DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy") + "." + "xls";//ConfigurationManager.AppSettings["extention"].ToString();//"D://carreport_25-10-2010.xls";
                        string filepath = ConfigurationManager.AppSettings["FileLoc"].ToString() + " \\" + "Agent" + "_" + fromdt.Replace('/', '-') + " To " + todt.Replace('/', '-') + "." + "xls";
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
            e.DataSources.Add(new ReportDataSource("station_DataTable2", dtTable2));

        }

        #region SearchCriteria
        private DataSet showSearchCriteria(string FromDate, string Todate)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dtSearch = new DataTable();
                DataColumn dcNew;

               

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "FromDate";
                dtSearch.Columns.Add(dcNew);

                dcNew = new DataColumn();
                dcNew.DataType = Type.GetType("System.String");
                dcNew.ColumnName = "ToDate";
                dtSearch.Columns.Add(dcNew);

            
                DataRow dr;
                dr = dtSearch.NewRow();

                
                dr["FromDate"] = FromDate;
                dr["ToDate"] = Todate;// "9";
                
                dtSearch.Rows.Add(dr);


                ds.Tables.Add(dtSearch);
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion



    }
}
