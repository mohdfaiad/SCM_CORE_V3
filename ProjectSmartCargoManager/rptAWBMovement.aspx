<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptAWBMovement.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.rptAWBMovement" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <style type="text/css">
        .style2
        {
            width: 182px;
        }
        .style3
        {
            width: 160px;
        }
    </style>
     <script type="text/javascript">
        function GetAgentCode() {
            var level = 'AgentCode';
            var TxtOriginClientObject = '<%=txtAgentCode.ClientID %>';
            window.open('ListDataView.aspx?Parent=AgentCode&level=' + level + '&TargetTXT=' + TxtOriginClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
            return false;
        }
    </script>
 
</asp:Content>

 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
     
     <%--<script type="text/javascript">

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
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>--%>
     
     <div id="contentarea">
   <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
    
    <h1>AWB Movement</h1>
            <%--<img alt="" src="Images/flightshlist.png"  style="vertical-align:5"/> </h1>--%>
       <h2>
        
       </h2>
          <div class="botline">
            <table width="100%">
                <tr>
                    <td>
                        Country</td>
                    <td ><asp:DropDownList ID="ddlCountry" runat="server" Width="130"
                            onselectedindexchanged="ddlCountry_SelectedIndexChanged"></asp:DropDownList>
                        
                         <%--  <asp:TextBoxWatermarkExtender ID="txtCountry_TextBoxWatermarkExtender"  WatermarkText="User Name" 
                    runat="server" TargetControlID="txtUserName">
                </asp:TextBoxWatermarkExtender>--%>
                    </td>
                    <td >
                        Region</td>
                    <td >
                        <asp:DropDownList ID="ddlRegion" runat="server"></asp:DropDownList>
                    </td>
                    <td >
                        Origin
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txtAutoSource" runat="server" Width="100px"></asp:TextBox>
                         <asp:AutoCompleteExtender ID="txtAutoSource_AutoCompleteExtender" runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
               EnableCaching="true"  TargetControlID="txtAutoSource">
               </asp:AutoCompleteExtender>--%>
                    <asp:DropDownList ID="ddlOrigin" runat="server" width="130"
                            
                            AutoPostBack="True" Visible="True">
                        </asp:DropDownList></td>
                    
                    <td>
                        Destination</td>
                    <td >
                      
                        <asp:DropDownList ID="ddlDestination" runat="server" Width="130" AutoPostBack="True" 
                         
                            Visible="true">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        Date From</td>
                    <td class="style3">
              <asp:TextBox ID="txtFlightFromdate" runat="server"  Width="100px" 
                            ToolTip="Please enter date format: dd/MM/yyyy"></asp:TextBox>
                            <asp:ImageButton ID="imgAWBFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
              <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server" PopupButtonID="imgAWBFromDt"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate">
              </asp:CalendarExtender>
                    </td>
                    <td >
                        Date To</td>
                    <td >
                      
              <asp:TextBox ID="txtFlightToDate" runat="server" Width="100px" 
                          ToolTip="Please enter valid date format: dd/MM/yyyy" 
                 ></asp:TextBox>
                 &nbsp;&nbsp;
                 <asp:ImageButton ID="imgAWBToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                 
              <asp:CalendarExtender ID="txtFlightToDate_CalendarExtender" runat="server" PopupButtonID="imgAWBToDt"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightToDate">
              </asp:CalendarExtender>
                    </td>
                    <td >
                        AirCraft Type</td>
                    <td>
                    <%--<asp:DropDownList ID="ddlFlight" runat="server" 
                            onselectedindexchanged="ddlFlight_SelectedIndexChanged">
              </asp:DropDownList>--%>
                        <asp:DropDownList ID="ddlAirCraftType" runat="server" Visible="true" 
                            AutoPostBack="True">
              </asp:DropDownList>
                    </td>
                    
                    <td>
                        AWB #</td>
                    <td class="style2">
                    <asp:TextBox ID="txtAwbPrefix" runat="server" Width="30"></asp:TextBox>
                    <asp:TextBox ID="txtAwbNumber" runat="server" Width="112"></asp:TextBox>
                        
                        </td>
                </tr>
                <tr>
                    <td>
                    
                        
                        
                        
                        Flight #</td>
                    <td class="style3">
                     <asp:DropDownList ID="ddlFlightPrefix" runat="server" 
                            onselectedindexchanged="ddlFlightPrefix_SelectedIndexChanged" 
                            AutoPostBack="True"></asp:DropDownList>
              <asp:DropDownList ID="ddlFlightNumber" runat="server"></asp:DropDownList>&nbsp;</td>
                            
                        </td>
                    <td >
                      
                       Status</td>
                    <td >
                      
                          <asp:DropDownList ID="ddlStatus" runat="server" Visible="True">
                          <asp:ListItem Value ="All"></asp:ListItem>
                         <asp:ListItem Value ="Complete"></asp:ListItem>
                         <asp:ListItem Value="InComplete"></asp:ListItem>
                         
                        </asp:DropDownList>
                    </td>
                    <td >
                        AgentCode</td>
                    <td>
                        <asp:TextBox ID="txtAgentCode" runat="server" Width="130" ></asp:TextBox>
                        <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
                             
                        </td>
                    <td>
                        </td>
                    <td >
                        
                    </td>
                        
                    </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click" />
                         <asp:Button ID="btnclear" runat="server" Text="Clear" 
                            CssClass="button" onclick="btnclear_Click" /> 
                        <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false"
                            CssClass="button" onclick="btnExport_Click" />
                    
                     </td>
                    <td >
                      
                        &nbsp;</td>
                    <td >
                      
                          &nbsp;</td>
                    <td >
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="style2">
                         &nbsp;</td>
                    
                </tr>
                <tr>
                    <td colspan="8">
                       <%-- <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
             ForeColor="Red"></asp:Label></td>--%>
                   
                </tr>
           </table>
    
   
    
    </div>
    
   
        
        <div style="border: thin solid #000000; float: left;">
            
           
            <rsweb:ReportViewer ID="ReportViewer1" runat="server"  
                Width="1000px" >
            </rsweb:ReportViewer>
            
            <dd:WebReportViewer ID="rptViewerAWBMovement" runat="server" Height="500px" Width="1000px" />
            
                    </div>  
            
            

  
         
         </div>
      
     <%--<div id="msglight" class="white_contentmsg">
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
    </asp:UpdatePanel>--%>
  
    </asp:Content>
