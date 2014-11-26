using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using nsBalInterface;
using System.Text;
using System.IO;

namespace ProjectSmartCargoManager
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {

        [WebMethod]
        public DataSet GetFlightDetails(String strFlightDate)
        {
            DataSet objDs = null;

            try
            {
                DateTime dtTemp=Convert.ToDateTime(strFlightDate );
                objDs =  GetFlightDetails(dtTemp, "" );

                if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
                {
                    byte[] raw = Encoding.UTF8.GetBytes(objDs.GetXml());
                    Stream decoded = new MemoryStream(raw);

                    string FileName = "IAMCEB_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".xml";
                    bool Uploaded = CommonUtility.UploadBlob(decoded, FileName);
                    cls_BL.DumpInterfaceInformation("IAMCEB", FileName, DateTime.Now, "IAMCEB", "", true, "IAMCEB", "IAMCEB",null,"");
                }
            }
            catch (Exception objEx)
            {
                objDs = null;
            }
            return objDs;
        }

        public DataSet GetFlightDetails(DateTime  dtFlightDate)
        {
            try
            {                
                return GetFlightDetails(dtFlightDate, "");
            }
            catch (Exception objEx)
            {
                // Err=" Error while getting flight information due to :- "+objEx.Message;
            }
            return null;
        }


        public DataSet  GetFlightDetails(DateTime objDate, String strFlightNo)
        {
            try
            {
                BalInterface objBal = new BalInterface();
                return objBal.GetFlightDetails(objDate, strFlightNo);
            }
            catch(Exception objEx)
            {
                // Err=" Error while getting flight information due to :- "+objEx.Message;
            }
            return null;
        }
    }
}
