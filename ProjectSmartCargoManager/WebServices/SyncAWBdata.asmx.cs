using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;


namespace ProjectSmartCargoManager.WebServices
{
    /// <summary>
    /// Summary description for SyncAWBdata
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SyncAWBdata : System.Web.Services.WebService
    {
       // BAL.Log log = new Log();
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        
        [WebMethod]
        public bool SyncAWB(DataSet ds)
        {
            try
            {


                string ConStr = Global.GetConnectionString();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    #region Encode In FFM Format
                    if (bool.Parse(dr[13].ToString()) == false)
                    {
                        string preAWB = dr[0].ToString();//preAWB
                        string AWB = dr[1].ToString();//AWB
                        DateTime dt =Convert.ToDateTime(dr[2].ToString());//FltDate
                        string Origin = dr[3].ToString();//Origin
                        string Dest = dr[4].ToString();//Dest
                        string FltNo = dr[5].ToString();//FltNo
                        string Desc = dr[6].ToString();//Desc
                        string SHC = dr[7].ToString();//SHC
                        string Srno = dr[8].ToString();//Srno
                        string Pcs = dr[9].ToString();//Pcs
                        string Wgt = dr[10].ToString();//Wgt
                        string Vol = dr[11].ToString();//Vol
                        string POU = dr[12].ToString();//POU
                        string isprocessed = dr[13].ToString();//isprocessed
                        string FFM="";
                        Log.WriteLog("Saving AWB " + AWB + " to MailBox :[" + DateTime.Now + "]");

                        int res = SendFFM(preAWB, AWB, dt, Origin, Dest, FltNo, Desc, SHC, Pcs, Wgt, Vol, POU,ref FFM);
                        if (res == 0 && FFM != "")
                        {

                            string[] param = { "subject", "body", "numofirops", "updatedon" };
                            SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.DateTime };
                            object[] values = { "AWB " + AWB, FFM, 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                            SQLServer da = new SQLServer(ConStr);
                            if (da.InsertData("spSaveFFMmessage", param, sqldbtypes, values) == true)
                            {
                                Log.WriteLog("AWB " + AWB + " Saved to MailBox :[" + DateTime.Now + "]");

                                return true;
                            }
                            else
                            {
                                Log.WriteLog("Failed Saving AWB " + AWB + " to MailBox :[" + DateTime.Now + "]");

                                return false;
                            }
                        }
                        else
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                    #endregion


                }


               

                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        private int SendFFM(string preAWB, string AWB, DateTime dt, string Origin, string Dest, string FltNo, string Desc, string SHC, string Pcs, string Wgt, string Vol, string POU,ref string FFM)
        {
            try
            {


                 FFM = "";
                 FFM += "FFM/8\r\n1/" + FltNo.Substring(0, 2) + FltNo.Substring(2, 3) + "/" + dt.ToString("ddMMM").ToUpper() + "/";// +lblRoute.Text.Substring(0, 3).ToUpper();
                string constr = Global.GetConnectionString();
                string s = dt.ToShortDateString();
            
                List<string> list = new List<string>();
                string org, dest;
                org = dest = "";
            
               list.Add(POU + FltNo.PadRight(2, ' ').Substring(0, 2) + "-" + AWB + Origin + Dest + "/T" + Pcs + "K" + Wgt+ "CC" + Vol + "/" + Desc + "/" + SHC+ "\r\n");
               org += Origin;
               dest = Dest;
                 
                
              
                if (list.Count > 0)//No NILL FFM
                {
                    // "\r\n" mod For ffm Import in desktop
                    //in frm expmanifest its "\n" only -noted
                    FFM += org.Substring(0, 3).ToString() + "\r\n";//Adding Source
                    string tmp = "";
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (tmp != list[j].Substring(0, 3))
                        {

                            FFM += list[j].Substring(0, 3) + "\r\n";
                            FFM += list[j].Substring(3);

                        }
                        else
                        {

                            FFM += list[j].Substring(3);

                        }
                        tmp = list[j].Substring(0, 3);
                    }
                }
               
                FFM += "LAST";
             //   Session["FFM"] = FFM;
                return 0;// OK
            }
            catch (Exception)
            {

                return 1;
            }

        }
    }
}
