<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ProjectSmartCargoManager.Home" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register TagPrefix="tab" TagName="TabUserControl" Src="~/CustomControls/TabPanel.ascx"%>
<%@ Register TagPrefix="tabAgent" TagName="AgentUserControl" Src="~/CustomControls/CustomAgentTab.ascx" %>
<%@ Register TagPrefix="tabAcc" TagName="AccountsUserControl" Src="~/CustomControls/CustomAccountsTab.ascx" %>
<%@ Register TagPrefix="tabOperations" TagName="AccountsUserControl" Src="~/CustomControls/CustomOperationsTab.ascx" %>
<%@ Register TagPrefix="tabManagement" TagName="ManagementUserControl" Src="~/CustomControls/CustomManagementTab.ascx" %>
<%@ Register TagPrefix="tabIT" TagName="ITAdminUserControl" Src="~/CustomControls/CustomITAdminTab.ascx" %>
<%@ Register TagPrefix="tabPlanner" TagName="FltPlannerUserControl" Src="~/CustomControls/CustomFltPlannerTab.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
    
     

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
      
     <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            //document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }

               
     </script>
     <style type="text/css">
        .black_overlay
        {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 1000px;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: 0.8;
            filter: alpha(opacity=80);
        }
        .white_content
        {
            margin: 0 auto;
            display: none;
            position: absolute;
            top: 30%;
            left: 35%;
            width: 30%;
            height: 45%;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
        .black_overlaymsg
        {
            display: none;
            position: fixed;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: White;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }
        .white_contentmsg
        {
            display: none;
            position: fixed;
            top: 45%;
            left: 45%;
            width: 5%;
            height: 5%;
            padding: 16px;
            background-color: Transparent;
            z-index: 1002;
        }
        
        
    </style>
<asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
<div id="contentarea">
    
    
    <asp:Timer ID="Timer1" runat="server"  Interval="600000"  Enabled = "false">
    </asp:Timer>
    
    
   <asp:Panel ID="Panel1" runat="server" Height="500px" Width="1024px">
       <%-- <asp:Image ID="Image1" runat="server" Height="100%" 
            ImageUrl="~/Images/jetwatermark.png" Width="100%" />--%><div 
           class="divback" style="display:none;">    
    <table width="100%">        
 <tr>     
<td>      
 <a href="AirportMaster.aspx"> 
    <img style="margin: 0px 20px; border-bottom: 0px; border-left: 0px; border-top: 0px; border-right: 0px;" 
        src="Images/warehousecontact.png" complete="complete"/></a><a 
        href="frmAWBTrackingMaster.aspx"><img style="margin: 1px 33px 0px 20px; border: 0px;" 
        src="Images/txt_tracking.png" complete="complete"/></a><a 
        href="GHA_ConBooking.aspx"><img 
        style="margin: 1px 20px; border:0px;" src="Images/txt_bookcargo.png" 
        complete="complete"/></a><a 
        href="frmFlightCapacityPlanning.aspx?Flag=H"><img style="margin: 1px 20px; border:0px;" 
        src="Images/txt_capacityplanning.png" complete="complete"/></a><%--<a href="#"> <img style="margin:0 20px;" src="Images/txt_cargorate.png" /> </a>   --%><%--<a href="ListAirlineSchedule.aspx"> <img style="margin:0 20px;" src="Images/txt_flightdetails.png" />  </a> --%></td>       
  <td>          
   <img src="Images/txt_quicklink.png" /></td>    
 </tr>     
   </table>        
    </div>  
<br />      
<div>      
<table width="100%">
<tr>     
  
  <td style="width:50%;">          
  
  <%------------------------------Sumit--------------------------------------%><%--<img src="Images/txt_news.png" />
<br />
 <div style="margin:10px; padding:10px; border:1px solid red;">Today:</div>--%>
<%-- <tabAgent:AgentUserControl ID="tabAgent" runat="server" Visible="false" EnableUserControl="false" />
 <tabAcc:AccountsUserControl ID="tabAccounts" runat="server" Visible="false" />
<tabOperations:AccountsUserControl ID="tabOperation" runat="server" Visible="false" />
 <tabIT:ITAdminUserControl ID="tabITAdmin" runat="server" Visible="false" />
 <tabManagement:ManagementUserControl ID="tabManagement" runat="server" Visible="false" />
 <tabPlanner:FltPlannerUserControl ID="tabPlanner" runat="server" Visible="false" />
 <tab:TabUserControl ID="tabGeneral" runat="server" Visible="false"  />--%>
 <asp:PlaceHolder ID="UserControlPlaceHolder" runat="server">
 </asp:PlaceHolder>
 
 
 
  <%------------------------------Sumit--------------------------------------%></td>
   <td valign="top" style="width:35%">    
   <h2 style="padding:0px;">Notification </h2>    
        
    <div style=" padding-top:0px; border:1px solid #ccc; height:315px; overflow:auto; background:#fff;">
       <%--<table width=100%>
      
        <tr style="vertical-align:top; w" >
           <td>
           <asp:Label ID="Label1" runat="server" Text="Critical" Font-Bold="True" ></asp:Label></td>
           <td> <asp:Label ID="LblCritical" runat="server" Text="" ></asp:Label> </td>
       </tr>
       <tr><td colspan="2"><hr/></td></tr>
       
       <tr style="vertical-align:top">
           <td><asp:Label ID="Label3" runat="server" Text="High" Font-Bold="True" ></asp:Label></td>
           <td> <asp:Label ID="lblHigh" runat="server" Text="" ></asp:Label> </td>
       </tr>
       <tr><td colspan="2"><hr/></td></tr>
       
       <tr style="vertical-align:top">
           <td><asp:Label ID="Label5" runat="server" Text="Information" Font-Bold="True" ></asp:Label></td>
           <td> <asp:Label ID="lblInformation" runat="server" Text="" ></asp:Label> </td>
       </tr>
       <tr><td colspan="2"><hr/></td></tr>
       
       <tr style="vertical-align:top">
           <td><asp:Label ID="Label7" runat="server" Text="Maintenance" Font-Bold="True" ></asp:Label></td>
           <td><asp:Label ID="lblMaintenance" runat="server" Text="" ></asp:Label>  </td>
       </tr>
       </table>--%>
       
      <asp:GridView ID="grdNotifications" runat="server" ShowHeader="false" ShowFooter="false"   AutoGenerateColumns="false" GridLines="None" Width="100%">
       <Columns>
      <asp:TemplateField ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="msgicon mar"  >
       <ItemTemplate>
       <asp:Image ID="imgNotifications" runat="server" ImageUrl='<%#"~/Images/msg"+Eval("Importance")+".png"%>' />
       </ItemTemplate>
       </asp:TemplateField>
      
       <asp:TemplateField ItemStyle-Wrap="true" ItemStyle-CssClass="msgDetails" ItemStyle-Width="88%" >
       <ItemTemplate>
       <asp:Label ID="lblSubject"  runat="server" Font-Bold="true" Text='<%#Eval("Subject1")+"<br/>" %>'></asp:Label>
      
       <asp:Label ID="lblMessage"  runat="server" Text='<%#Eval("NotificationMsg") %>'></asp:Label>
      
       </ItemTemplate>
       </asp:TemplateField>
        </Columns>
       
       
       </asp:GridView>
       
       
   </div>
   
   <%------------------------------Amit--------------------------------------%>
   <%--<img src="Images/txt_downloads.png" />
   
   <div style="margin:10px; padding:10px; border:1px solid red;">
  <table>
     <tr>
       <td> <asp:Label ID="Label1" runat="server" Text="For Shipments within India :" Font-Bold="True"></asp:Label></td>
   </tr>
   
   <tr>
      <td>IDG 1 </td>
      <td>
          <asp:HyperLink ID="hyperIDG1" runat="server" NavigateUrl="~/Documents\IDG -1.jpg" Target="_blank" >Download</asp:HyperLink>
      </td>
   </tr>
   
   <tr>
      <td>IDG 2</td>
      <td>
      <asp:HyperLink ID="HyperIDG2" runat="server" NavigateUrl="~/Documents\IDG -2.jpg" Target="_blank">Download</asp:HyperLink>
      </td>
   </tr>
   
   <tr>
      <td>Security Declaration Form</td>
      <td>
        <asp:HyperLink ID="HyperSecDec" runat="server" NavigateUrl="~/Documents\Security Declaration.jpg" Target="_blank">Download</asp:HyperLink>
      </td>
   </tr>
   
   
   <tr>
       <td> <asp:Label ID="Label2" runat="server" Text="For DG Shipments:" Font-Bold="True"></asp:Label></td>
   </tr>
   
   
   <tr>
      <td>Dangerous Goods Checklist for a Non-Radioactive Shipment</td>
      <td>
          <asp:HyperLink ID="HyperDangerGoods" runat="server" NavigateUrl="~/Documents\Dangerous_Goods-Checklist-Non-Radioactive.pdf" Target="_blank">Download</asp:HyperLink>
       </td>
   </tr>
   
   <tr>
      <td>Acceptance Checklist for Dry Ice</td>
      <td>
          <asp:HyperLink ID="HyperAccList" runat="server" NavigateUrl="~/Documents\Acceptance-Checklist_DryIce.pdf" Target="_blank">Download</asp:HyperLink>
      </td>
   </tr>
   
   <tr>
      <td>Notoc Sheet 1</td>
      <td>
      <asp:HyperLink ID="HyperNotoc1" runat="server" NavigateUrl="~/Documents\Notoc 1.XLSX" Target="_blank">Download</asp:HyperLink>
      </td>
   </tr>
   
   <tr>
      <td>Notoc Sheet 2</td>
      <td>
      <asp:HyperLink ID="HyperNotoc2" runat="server" NavigateUrl="~/Documents\Notoc 2.xlsx" Target="_blank">Download</asp:HyperLink>
      </td>
   </tr>
  </table>
  </div>--%>
   <%------------------------------End code --------------------------------------%>
   
   
</td>  
</tr>
</table>  
    </div> 
    
    </asp:Panel>

    <%--
   <asp:TextBox ID="Country" runat="server"></asp:TextBox>
    
    
    <asp:AutoCompleteExtender ID="Country_AutoCompleteExtender" runat="server" 
        DelimiterCharacters="" Enabled="True" ServicePath="" ServiceMethod="GetStation" MinimumPrefixLength="1" EnableCaching="true" TargetControlID="Country"    >
    </asp:AutoCompleteExtender>--%><div visible="false">
        <asp:HiddenField ID="HideRoleID" runat="server" />
    </div>
    
  <%------------------------------Sumit--------------------------------------%>
  
    
  <%------------------------------Sumit--------------------------------------%>
    </div>
   
   <div id="msglight" class="white_contentmsg">
        <table>
            <tr>
                <td width="5%" align="center">
                    <br />
                    <img src="Images/loading.gif" />
                    <br />
                    <asp:Label ID="msgshow" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    
    <div id="msgfade" class="black_overlaymsg">
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
