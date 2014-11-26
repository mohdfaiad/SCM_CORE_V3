
<%@ Page Title="Daily AWB Sales Report" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptDailyAWBSalesReport.aspx.cs" Inherits="ProjectSmartCargoManager.rptDailyAWBSalesReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>
     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style type="text/css">
        .style3
        {
            width: 29%;
        }
        .style4
        {
            width: 11%;
        }
        .style5
        {
            width: 3%;
        }
        .style6
        {
            width: 107px;
        }
        .style7
        {
            width: 276px;
        }
    </style>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contentarea">
        <%--<h1>
            <img alt="Revenue" src="Images/stationwisereport.png" /> </h1>--%>
            <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
            <h1>
            Daily AWB Sales
            </h1>
     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    
        <table width="85%" cellpadding="2" cellspacing="2" >
            
            <tr>
                <td class="style6">
                    From Date</td>
                <td class="style7">
                    <asp:TextBox ID="txtFromdate" runat="server" Width="112px" 
                        ></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate" PopupButtonID="imgFromDate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
                </td>
                <td class="style4">
                    To Date</td>
                <td class="style3">
                    <asp:TextBox ID="txtTodate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate" PopupButtonID="imgToDate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
                </td>
                <td class="style5">
    
        
    
                </td>
            </tr>
            <tr>
            <td class="style6">
            <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" 
                        Text="List" />
        
        <asp:Button ID="btnExportReport" runat="server" CssClass="button"  
                        Text="Export"  onclick="btnExportReport_Click" />
            </td>
            </tr>
            <tr>
            <td colspan="3">
            
          <%--  <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red"></asp:Label>--%>
            </td>
            </tr>
            
        </table>
    
    </div>
    <%--<div class="rout" visible="false">
    <img src="Images/report.png"  />
        Note-Filter Criterion will be displayed here</div>--%>
       <%--<div>
    <dd:webreportviewer ID="RptViewerRevenue_Station" runat="server" Height="450px" 
                Width="1024px" xmlns:dd="datadynamics.reports.web" /></div> --%>
                <div style=" border: thin solid #000000; float: left;">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1022px">
            </rsweb:ReportViewer></div>
  
  
    </div>

</asp:Content>
