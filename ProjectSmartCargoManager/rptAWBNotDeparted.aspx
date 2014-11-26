<%@ Page Title="AWB Accepted But Not Departed" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptAWBNotDeparted.aspx.cs" Inherits="ProjectSmartCargoManager.rptAWBNotDeparted" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>
     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
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
    <div id="contentarea">
       <%-- <h1>
            <img alt="Revenue" src="Images/stationwisereport.png" /> 
            
            </h1>--%>
            <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
       <h1>
         AWB Acceepted But Not Departed
       </h1>

     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    
        <table width="80%" cellpadding="2" cellspacing="2" >
            <tr>
                <td>
                    Origin</td>
                <td>
    <asp:DropDownList ID="ddlOrigin" runat="server">
    </asp:DropDownList>
                </td>
                <td>
                    Destination</td>
                <td>
    <asp:DropDownList ID="ddlDest" runat="server">
    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                        
                                            
    
                    &nbsp;</td>
               
            </tr>
            <tr>
                <td>
                    AgentCode</td>
                <td>
                    <asp:TextBox ID="txtAgentCode" runat="server" Width="112px"></asp:TextBox>
                    <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
                         ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
                </td>
           <%-- </tr>
            <tr>--%>
                <%-- </tr>
            <tr>--%>
                <td>
                    From Date</td>
                <td>
                    <asp:TextBox ID="txtFromdate" runat="server" Width="74px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate" PopupButtonID="imgFromDt">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                </td>
                <td>
                    To Date</td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" Width="74px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate" PopupButtonID="imgToDt">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                </td>
                <td>
                        
                                            
    
                </td>
            <%--</tr>
            <tr>--%>
               
            </tr>
            <tr>
             <td colspan=7>
                <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" Text="List" />
                 <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
                     onclick="btnClear_Click" />
                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click" />
                </td>
            </tr>
            <tr>
            <td colspan="2">
            
           <%-- <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red"></asp:Label>--%>
            </td>
            </tr>
            
        </table>
    
    </div>
    <%--<div class="rout" visible="false">
    <img src="Images/report.png"  />
        Note-Filter Criterion will be displayed here</div>--%>
        <div style=" border: thin solid #000000; float: left;">
    <%--<dd:webreportviewer ID="RptViewerStockControl" runat="server" Height="450px" 
                Width="1024px" xmlns:dd="datadynamics.reports.web" />--%>
            <rsweb:ReportViewer ID="AWBNotDepartedRptViewer" runat="server" Width="1024px" 
              >
            </rsweb:ReportViewer>           

    </div>
    </div>

</asp:Content>

