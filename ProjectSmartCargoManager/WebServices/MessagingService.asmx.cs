using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;

namespace ProjectSmartCargoManager.WebServices
{
    /// <summary>
    /// Summary description for MessagingService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MessagingService : System.Web.Services.WebService
    {
  #region Constructor
        public MessagingService()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    #endregion

    #region checkInternet
    [WebMethod]
    public bool checkInternet() 
    {
        return true;
    }
    #endregion

    }
}
