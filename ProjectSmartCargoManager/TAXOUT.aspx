<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="TAXOUT.aspx.cs" Inherits="ProjectSmartCargoManager.TAXOUT" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
 
 </asp:ToolkitScriptManager>
     
 <script type="text/javascript">

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
     .style1
     {
         height: 29px;
     }
    </style>
    
 <%--<asp:UpdatePanel runat="server" ID="updtPnl">
 <ContentTemplate>--%>
     
 <div id="contentarea">
 <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
 <h1>TAX-OUT Report</h1>

<div class="botline">
<table>
<tr>
<td class="style1">Station</td>
<td class="style1"> 
<asp:DropDownList ID="ddlStation" runat="server"></asp:DropDownList>
</td>
<td class="style1"></td>
<td class="style1">Month*</td>
<td class="style1">
    <asp:DropDownList ID="ddlmonth" runat="server">
    <asp:ListItem Text="Select" Value="Select" Selected="True"></asp:ListItem>
    <asp:ListItem Text="Jan" Value="1"></asp:ListItem>
    <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
    <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
    <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
    <asp:ListItem Text="May" Value="5"></asp:ListItem>
    <asp:ListItem Text="Jun" Value="6"></asp:ListItem>
    <asp:ListItem Text="Jul" Value="7"></asp:ListItem>
    <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
    <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
    <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
    <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
    <asp:ListItem Text="Dec" Value="1"></asp:ListItem>
    </asp:DropDownList>
</td>
<td class="style1">Year*</td>
<td class="style1">
<asp:DropDownList ID="ddlYear" runat="server">
<asp:ListItem Text="Select" Value="Select"></asp:ListItem>
<asp:ListItem Text="2014" Value="2014"></asp:ListItem>
<asp:ListItem Text="2015" Value="2015"></asp:ListItem>
<asp:ListItem Text="2016" Value="2016"></asp:ListItem>
<asp:ListItem Text="2017" Value="2017"></asp:ListItem>
<asp:ListItem Text="2018" Value="2018"></asp:ListItem>
<asp:ListItem Text="2019" Value="2019"></asp:ListItem>
<asp:ListItem Text="2020" Value="2020"></asp:ListItem>
<asp:ListItem Text="2021" Value="2021"></asp:ListItem>
<asp:ListItem Text="2022" Value="2022"></asp:ListItem>
<asp:ListItem Text="2023" Value="2023"></asp:ListItem>
<asp:ListItem Text="2024" Value="2024"></asp:ListItem>
</asp:DropDownList>
</td>

<td class="style1"><asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
        onclick="btnList_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
        onclick="btnClear_Click"/></td>
</tr>
<tr>
<td colspan="9">
<table>
<tr>
<td>

</td>
</tr>
</table>
    </td>
</tr>
<tr>
<td>
</td>
</tr>
</table>
</div>
 

<div style="border: thin solid #000000; float: left">
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1022px">
    </rsweb:ReportViewer>
   
</div>

</div>
      
<div id="msglight" class="white_contentmsg">
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
    
 <%--</ContentTemplate>
 </asp:UpdatePanel>--%>
</asp:Content>
