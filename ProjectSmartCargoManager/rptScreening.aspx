<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptScreening.aspx.cs" Inherits="ProjectSmartCargoManager.rptScreening" MasterPageFile="~/SmartCargoMaster.Master"%>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2" 
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
		.black_overlaymsg{
			display: none;
			position: fixed;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 100%;
			background-color: black;
			z-index:1001;
			-moz-opacity: 0.8;
			opacity:.80;
			filter: alpha(opacity=80);
		}
		.white_contentmsg {
			display: none;
			position: fixed;
			top: 25%;
			left: 25%;
			width: 40%;
			height: 40%;
			padding: 16px;
			background-color: white;
			z-index:1002;
			overflow: auto;
		}
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"  EnableViewState="true">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br /><br /> <br />
    <h1>X Ray Screening Report</h1>
    <br /><br />
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
   <br />
    <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" 
        onclick="btnExport_Click" />
     <dd:webreportviewer ID="RptViewerXRay" runat="server" Height="450px" 
         Width="1024px" xmlns:dd="datadynamics.reports.web" />
    </asp:Content>