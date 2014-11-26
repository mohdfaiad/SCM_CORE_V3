
<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptAWBSummarisedBilling.aspx.cs" Inherits="ProjectSmartCargoManager.rptAWBSummarisedBilling" %>


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
    <div id="contentarea">
        <h1>
            <img alt="Revenue" src="Images/stationwisereport.png" /> </h1>
       

     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    
        <table width="70%" cellpadding="2" cellspacing="2" >
            <tr>
                <td>
                    AgentCode</td>
                <td>
                    <asp:TextBox ID="txtAgentCode" runat="server" Width="112px"></asp:TextBox>
                </td>
                <td>
                    Payment Type</td>
                <td class="style1">
                    <asp:DropDownList ID="ddlPaymentType" runat="server">
                     <asp:ListItem>All</asp:ListItem>
                    <asp:ListItem>PP</asp:ListItem>
                        <asp:ListItem>CC</asp:ListItem>
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
                        <asp:ListItem>City</asp:ListItem>
                        <asp:ListItem>Region</asp:ListItem>
                        <asp:ListItem>Country</asp:ListItem>
                      
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlCode" runat="server">
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
    <dd:webreportviewer ID="RptViewerRevenue_Station" runat="server" Height="450px" 
                Width="1024px" xmlns:dd="datadynamics.reports.web" />
   <br />
    </div>
    </div>

</asp:Content>
