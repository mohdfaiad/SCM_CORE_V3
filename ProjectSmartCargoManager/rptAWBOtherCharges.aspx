﻿<%@ Page  Title="AWB CUT Charges Report" Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="rptAWBOtherCharges.aspx.cs" Inherits="ProjectSmartCargoManager.rptAWBOtherCharges" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>



<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>
     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
     
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
<style type="text/css">
       .style2
        {
          
        }
    </style>
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div id="contentarea">
    
      <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>  <%--<h1>
            <img alt="Revenue" src="Images/stationwisereport.png" /> </h1>--%>
            <h1>AWB CUT Charges Report</h1>
       

     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    
        <table width="85%" cellpadding="2" cellspacing="2" >
            <tr>
                <td class="style2">
                    From Date</td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                        Enabled="True" TargetControlID="txtFromDate" PopupButtonID="imgFromDt"> 
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                </td>
                
                <td>
                TO Date
                </td>
                <td>
                <asp:TextBox ID="txtToDate" runat="server" Width="112px"></asp:TextBox>
                <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server"   Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtToDate" PopupButtonID="imgToDt">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                </td>
               <td>
    
        
    
                </td>
            </tr>
            <tr>
            <td colspan="8">
            <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" onclick="btnList_Click" Height="23px" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
                    onclick="btnClear_Click" />
        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click" />
            </td>
            </tr>
            <tr>
            <td colspan="8">
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
    <dd:webreportviewer ID="RptViwerAWBOtherCharges" runat="server" 
                Width="1024px" Visible="False"  />
  
    </div>
    <div style=" border: thin solid #000000; float: left;">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server"
            Width="1024px">
        </rsweb:ReportViewer></div>
    </div>

</asp:Content>
