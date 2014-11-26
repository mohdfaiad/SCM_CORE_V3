<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="BI_Admin.aspx.cs" Inherits="ProjectSmartCargoManager.BI_Admin" %>
<%@ Register assembly="ActiveReports.Server.ReportControls" namespace="ActiveReports.Server.ReportControls" tagprefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <br />
    <br />
   
<div>

    <cc1:Designer ID="Designer1" runat="server" SiteRootPath="http://scmcebureports.cloudapp.net/" Height="600px" Width="980px">
    </cc1:Designer>

</div>
</asp:Content>
