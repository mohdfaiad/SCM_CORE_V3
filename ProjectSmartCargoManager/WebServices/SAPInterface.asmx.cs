using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace ProjectSmartCargoManager
{
    /// <summary>
    /// Summary description for SAPInterface
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SAPInterface : System.Web.Services.WebService
    {

        [WebMethod]
        public string GenerateSAPXML(DateTime GenerationDate)
        {
            string strResult = string.Empty;
            SAP objSAP = new SAP(GenerationDate);
           // strResult = objSAP.ToXML();
            strResult = objSAP.ToXMLNode(GenerationDate, GenerationDate);
            return strResult;
        }
    }
}
