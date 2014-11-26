<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptUnbilledAWBs.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.rptUnbilledAWBs" %>

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
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="msg">
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True"></asp:Label>
</div>
    <div id="contentarea">
        <h1>
           Unbilled AWB report
       </h1>
     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    
        <table width="50%" cellpadding="4" cellspacing="4">
            <tr>
                <td>From Date</td>
                <td>
                    <asp:TextBox ID="txtFromdate" runat="server" Width="112px"></asp:TextBox>
                    <asp:ImageButton ID="imgAWBFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgAWBFromDt"
                        Enabled="True" TargetControlID="txtFromdate">
                    </asp:CalendarExtender>
                </td>
                <td>To Date</td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" Width="112px"></asp:TextBox>
                    <asp:ImageButton ID="imgAWBToDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy" PopupButtonID="imgAWBToDate"
                        Enabled="True" TargetControlID="txtTodate">
                    </asp:CalendarExtender>
                </td>
                </tr>
                <tr>
           <td colspan="4">
         <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" Text="List" />
        <asp:Button ID="btnExport" runat="server" CssClass="button" Text="Export" onclick="btnExport_Click"/>
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" />
             </td> 
            </tr>
        </table>
    
    </div>
    <%--<div class="rout" visible="false">
    <img src="Images/report.png"  />
        Note-Filter Criterion will be displayed here</div>--%>
       <%-- <div style="float:left;">
    <dd:webreportviewer ID="RptViewerRevenue_Station" runat="server" Height="450px" 
                Width="1024px" xmlns:dd="datadynamics.reports.web" />
   <br />
    </div>--%>
     <div style="border: thin solid #000000;float:left;">
         <rsweb:ReportViewer ID="RptUnbilledViewer" runat="server" Width="1024px" Height="450px">
         </rsweb:ReportViewer>      
  
    </div>
    </div>

</asp:Content>
     
