

<%@ Page Title="Offload Report" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptOffloadReport.aspx.cs" Inherits="ProjectSmartCargoManager.rptOffloadReport" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>
     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
   
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contentarea">
       <%-- <h1>
            <img alt="Revenue" src="Images/stationwisereport.png" /> 
            
            </h1>--%>
            <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
</div>
       <h1>
           Offload Report
       </h1>

     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    
        <table width="65%" cellpadding="2" cellspacing="2" >
            <tr>
                <td>
                    Flight#</td>
                <td>
                                    <asp:DropDownList ID="ddlFlightPrefix" runat="server" AutoPostBack="True" 
                                        
                        onselectedindexchanged="ddlFlightPrefix_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlFlight" runat="server" 
                        AppendDataBoundItems="True">
                                    </asp:DropDownList>
               
                    </td>
                <td>
                    AWB No</td>
                <td>
                    <asp:TextBox ID="txtAWBPrefix" runat="server" Width="30px"></asp:TextBox>
<asp:TextBox ID="txtAWBNo" runat="server" Width="112px" 
                      ></asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="ddlType" runat="server" 
                       AutoPostBack="True" 
                        Visible="False">
                           <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem>Airport</asp:ListItem>
                      <%--  <asp:ListItem>City</asp:ListItem>--%>
                        <asp:ListItem>Region</asp:ListItem>
                        <asp:ListItem>Country</asp:ListItem>
                      
                    </asp:DropDownList>
                       </td>
            </tr>
            <tr>
                <td>
                    Location</td>
                <td>
                    <asp:DropDownList ID="ddlLocCode" runat="server" Width ="112">
                    </asp:DropDownList>
                       
                </td>
                <td>
                    &nbsp;</td>
                <td >
                    <asp:TextBox ID="txtAwbPrefix1" runat="server" Width="34px" Visible="False"></asp:TextBox>
                 </td>
                <td>
                    <asp:DropDownList ID="ddlAWBStatus" runat="server" Visible="False">
                           <asp:ListItem>All</asp:ListItem>
                       <asp:ListItem>Booked</asp:ListItem>
                        <asp:ListItem>Executed</asp:ListItem>
                        <asp:ListItem>Reopened</asp:ListItem>
                       
                        <asp:ListItem>Void</asp:ListItem>
                      
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    From Date</td>
                <td>
                    <asp:TextBox ID="txtFromdate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate" PopupButtonID="imgFromDt">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                </td>
                <td>
                    To Date</td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate" PopupButtonID="imgToDt">
                    </asp:CalendarExtender>
    
                    <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
    
                </td>
                <td>
    
        
    
                    </td>
            </tr>
            <tr>
                <td>
    <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" 
                        Text="List" />
                    <asp:Button ID="btnClear" runat="server" CssClass="button" 
                        onclick="btnClear_Click" Text="Clear" />
        <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false"
                            CssClass="button" onclick="btnExport_Click" />                    
                                            &nbsp;</td>
                <td>
                   </td>
                <td>
                    </td>
                <td>
                    </td>
                <td>
    
                    </td>
            </tr>
            <tr>
                <td colspan="5">
            
         <%--   <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red"></asp:Label>--%>
                </td>
            </tr>
                        
        </table>
    
    </div>
    <%--<div class="rout" visible="false">
    <img src="Images/report.png"  />
        Note-Filter Criterion will be displayed here</div>--%>
        <div style=" border: thin solid #000000; float: left;">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1024px" 
                >
            </rsweb:ReportViewer>
            
    <dd:webreportviewer ID="RptViewerRevenue_Station" runat="server" Height="500px" 
                Width="1024px" xmlns:dd="datadynamics.reports.web" />

    </div>
    </div>

</asp:Content>


