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
    /// Summary description for ARMSInterface
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ARMSInterface : System.Web.Services.WebService
    {
        [WebMethod]
        public DataSet GetFlightDetails(DateTime fromDate, DateTime toDate, String strFlightNo)
        {
            DataSet objDs = null;

            try
            {
                BalInterface objBal = new BalInterface();                
                objDs = objBal.GetFlightDetailsARMS(fromDate , toDate , strFlightNo);

                if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
                {
                    byte[] raw = Encoding.UTF8.GetBytes(objDs.GetXml());
                    Stream decoded = new MemoryStream(raw);

                    string FileName = "ARMS_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".xml";
                    bool Uploaded = CommonUtility.UploadBlob(decoded, FileName);
                    cls_BL.DumpInterfaceInformation("ARMS", FileName, DateTime.Now, "ARMS", "", true, "ARMS", "ARMS", null, "");
                }
            }
            catch (Exception objEx)
            {
                objDs = null;
            }
            return objDs;
        }
    }
}
