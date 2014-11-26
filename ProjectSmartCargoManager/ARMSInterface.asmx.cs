using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using nsBalInterface;

namespace IAMCEBInterface.asmx
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

        public DataSet GetFlightDetails(String strFlightNo)
        {
            try
            {
                BalInterface objBal = new BalInterface();
                DateTime fromDate = Convert.ToDateTime(strFlightNo);
                return objBal.GetFlightDetailsARMS(fromDate, fromDate, "9w100");
            }
            catch (Exception objEx)
            {
                // Err=" Error while getting flight information due to :- "+objEx.Message;
            }
            return null;
        }
        public DataSet GetFlightDetails(DateTime fromDate, DateTime toDate, String strFlightNo)
        {
            try
            {
                BalInterface objBal = new BalInterface();
                return objBal.GetFlightDetailsARMS(fromDate , toDate , strFlightNo);
            }
            catch (Exception objEx)
            {
                // Err=" Error while getting flight information due to :- "+objEx.Message;
            }
            return null;
        }
    }
}
