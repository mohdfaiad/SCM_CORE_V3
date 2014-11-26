
<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptBillingTonnageReport.aspx.cs" Inherits="ProjectSmartCargoManager.rptBillingTonnageReport" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>
     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contentarea">
       <%-- <h1>
            <img alt="Revenue" src="Images/stationwisereport.png" /> 
            
            </h1>--%>
       <h2>
           Billing Tonnage Details
       </h2>

     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    
        <table width="50%" cellpadding="2" cellspacing="2" >
            <tr>
                <td>
                    Flight Date</td>
                <td>
                    <asp:TextBox ID="txtFromdate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate">
                    </asp:CalendarExtender>
                </td>
                <td>
                    To Date</td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate">
                    </asp:CalendarExtender>
                </td>
                <td>
    
        <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" 
                        Text="List" />
    
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
    
                    &nbsp;</td>
            </tr>
            <tr>
            <td colspan="3">
            
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red"></asp:Label>
            </td>
            </tr>
            
        </table>
    
    </div>
    <%--<div class="rout" visible="false">
    <img src="Images/report.png"  />
        Note-Filter Criterion will be displayed here</div>--%>
        <div style="float:left;">
    
   <br />
            <dd:WebReportViewer ID="WebReportViewer1" runat="server" height="500px" 
                width="983px" />
    </div>
    </div>

</asp:Content>

