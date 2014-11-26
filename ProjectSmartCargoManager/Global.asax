<%@ Application Language="C#" %>
<%@ Import Namespace="ActiveReports.Server.ReportControls.Servicing" %>


<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        //DG.Square.ASP.NET.Ajax.ChatControl.ChatBox cb = new DG.Square.ASP.NET.Ajax.ChatControl.ChatBox();
        //cb.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
        //cb.DataProvider = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ProviderName;
        //cb.WatchSessionExpire(20); 
        // Session Timeout - default will be 20 if you didnt changed the timeout time. 

        // Code that runs on application startup
        
        //ARS
        ActiveReports.Server.ReportControls.Servicing.ServiceProxyBase.ResolveRemoteEndpoint += ResolveRemoteEndpoint;
       

    }

    //ARS
    static void ResolveRemoteEndpoint(RemoteEndpoint remoteEndpoint)
    {
        if (SessionStorage.Address == null)
        {
            return;
        }

        remoteEndpoint.Address = SessionStorage.Address;
        remoteEndpoint.SecurityToken = SessionStorage.SecurityToken;
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
      // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
