
<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptMISAWBBooking.aspx.cs" Inherits="ProjectSmartCargoManager.rptMISAWBBooking" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>
     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style type="text/css">
        .style1
        {
            width: 253px;
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
    <div id="contentarea">
    <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
</div>
        <%--<h1>
            <img alt="Revenue" src="Images/stationwisereport.png" /> </h1>--%>
            <h1>
            AWB Tonnage
            </h1>
     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    
        <table width="85%" cellpadding="2" cellspacing="2" >
            <tr>
                <td>
                    AgentCode</td>
                <td>
                    <asp:TextBox ID="txtAgentCode" runat="server" Width="112px"></asp:TextBox>
                    <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
                </td>
                <td>
                    Payment Type</td>
                <td class="style1">
                    <asp:DropDownList ID="ddlPaymentType" runat="server">
                    <asp:ListItem Value="ALL">ALL(without FOC)</asp:ListItem>
                    <asp:ListItem>PP</asp:ListItem>
                        <asp:ListItem>CC</asp:ListItem>
                         <asp:ListItem>PX</asp:ListItem>
                       <asp:ListItem>FOC</asp:ListItem>
                     
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    Controlling Location Code</td>
                <td>
                   
                       <asp:DropDownList ID="ddlControlingLocator" runat="server" >   
                        <asp:ListItem>All</asp:ListItem>
                       <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                       
                     
                    </asp:DropDownList>
                       
                </td>
                <td>
                    Level</td>
                <td >
                    <asp:DropDownList ID="ddlType" runat="server" 
                        onselectedindexchanged="ddlType_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem>Airport</asp:ListItem>
                       <%--<asp:ListItem>City</asp:ListItem>--%>
                        <asp:ListItem>Region</asp:ListItem>
                        <asp:ListItem>Country</asp:ListItem>
                        
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlCode" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="txtControllingCode" runat="server"  visible="false"  Width="112px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    AWB Status</td>
                <td>
                    <asp:DropDownList ID="ddlAWBStatus" runat="server" 
                        
                        onselectedindexchanged="ddlAWBStatus_SelectedIndexChanged">
                           <asp:ListItem Value="All">All</asp:ListItem>
                       <asp:ListItem Value="B">Booked</asp:ListItem>
                        <asp:ListItem Value="E">Executed</asp:ListItem>
                        <asp:ListItem Value="R">Reopened</asp:ListItem>
                       
                        <asp:ListItem Value="V">Void</asp:ListItem>
                      
                    </asp:DropDownList>
                </td>
                <td>
                    Based On :</td>
                <td>
                    <asp:DropDownList ID="ddlDateCriteria" runat="server">
                    <asp:ListItem>Execution Date</asp:ListItem>
                    <asp:ListItem>Flight Date</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
    
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    From Date</td>
                <td>
                    <asp:TextBox ID="txtFromdate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate" PopupButtonID="imgfromdate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgfromdate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" />
                </td>
                <td>
                    To Date</td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate" PopupButtonID="imgtodate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgtodate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" />
                </td>
                <td>
    
        
    
                </td>
            </tr>
            <tr>
            <td>
            <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" 
                        Text="List" />
        <asp:Button ID="btnExport" runat="server" CssClass="button"  
                        Text="Export" onclick="btnExport_Click" Visible="false" />
        <asp:Button ID="btnExportReport" runat="server" CssClass="button"  
                        Text="Export"  onclick="btnExportReport_Click" />
                <asp:Button ID="btnClear" runat="server" CssClass="button" 
                    onclick="btnClear_Click" Text="Clear" />
            </td>
            </tr>
            <tr>
            <td colspan="3">
            
            <%--<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red"></asp:Label>--%>
            </td>
            </tr>
            
        </table>
    
    </div>
    <%--<div class="rout" visible="false">
    <img src="Images/report.png"  />
        Note-Filter Criterion will be displayed here</div>--%>
        
    <%--<dd:webreportviewer ID="RptViewerRevenue_Station" runat="server" Height="450px" 
                Width="1024px" xmlns:dd="datadynamics.reports.web" />--%>
                
   
   <div style="border: thin solid #000000; float:left;">
    
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1022px" > 
            </rsweb:ReportViewer></div>
    
    </div>

</asp:Content>
