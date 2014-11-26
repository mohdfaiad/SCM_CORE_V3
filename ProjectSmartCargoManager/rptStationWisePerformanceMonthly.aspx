<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptStationWisePerformanceMonthly.aspx.cs" Inherits="ProjectSmartCargoManager.rptStationWisePerformanceMonthly" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style type="text/css">
        .style7
        {
            width: 51px;
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
     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
            <h1>
            Station Performance Per Month</h1>
    <div class="botline">
    
        <table width="100%" cellpadding="2" cellspacing="2" >
            
            <tr>
               
                <td>
                    Flights</td>
                <td>
                    <asp:DropDownList ID="ddlStationType" runat="server">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem Value="DOM">Domestic</asp:ListItem>
                        <asp:ListItem Value="INT">International</asp:ListItem>
                    </asp:DropDownList>
                </td>

        <td __designer:mapid="10" class="style7">
            Station</td>
        <td __designer:mapid="11">
            <asp:DropDownList ID="ddlAirport" runat="server">
            </asp:DropDownList>
        </td>
                                 <td>
                    Month</td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server">
                    <asp:ListItem Text="January" Value="1"></asp:ListItem>
                    <asp:ListItem Text="February" Value="2"></asp:ListItem>
                    <asp:ListItem Text="March" Value="3"></asp:ListItem>
                    <asp:ListItem Text="April" Value="4"></asp:ListItem>
                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                    <asp:ListItem Text="June" Value="6"></asp:ListItem>
                    <asp:ListItem Text="July" Value="7"></asp:ListItem>
                    <asp:ListItem Text="August" Value="8"></asp:ListItem>
                    <asp:ListItem Text="September" Value="9"></asp:ListItem>
                    <asp:ListItem Text="October" Value="10"></asp:ListItem>
                    <asp:ListItem Text="November" Value="11"></asp:ListItem>
                    <asp:ListItem Text="December" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                Year
                </td>
                <td>
                <asp:DropDownList ID="ddlYear" runat="server">
                <asp:ListItem>2014</asp:ListItem>
                 <asp:ListItem>2015</asp:ListItem>
                  <asp:ListItem>2016</asp:ListItem>
                   <asp:ListItem>2017</asp:ListItem>
                    <asp:ListItem>2018</asp:ListItem>
                     <asp:ListItem>2019</asp:ListItem>
                      <asp:ListItem>2020</asp:ListItem>
                      <asp:ListItem>2021</asp:ListItem>
                      <asp:ListItem>2022</asp:ListItem>
                      <asp:ListItem>2023</asp:ListItem>
                      <asp:ListItem>2024</asp:ListItem>
                </asp:DropDownList>
                </td>
            </tr>
            <tr>
            <td colspan="3">
            
          <%--  <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red"></asp:Label>--%>
            <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" 
                        Text="List" />
        
                <asp:Button ID="btnClear" runat="server" CssClass="button" 
                    onclick="btnClear_Click" Text="Clear" />
        
        <asp:Button ID="btnExportReport" runat="server" CssClass="button"  
                        Text="Export"  onclick="btnExportReport_Click" />
            </td>
            </tr>
            
        </table>
    
    </div>
  
        <div style=" border: thin solid #000000; float: left;">
         
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1024px">
            </rsweb:ReportViewer>
            
            </div>
  
  </div>
    

</asp:Content>
