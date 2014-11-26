<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmULDMovementHistory.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmULDMovementHistory" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .style1
        {
            width: 110px;
        }
    </style>
<script language="javascript" type="text/javascript">
    function message() {
        alert("No Data Found");
    }
</script>
</asp:Content>


 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
     </asp:ToolkitScriptManager>
      

    <div id="contentarea">
    <div class="botline">
    <h1>ULD Movement History</h1>
<table width="100%">
<tr>
<td colspan="2">
    <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
    </td>
    
</tr>

<tr>
<td>
<table cellpadding="3" cellspacing="3">

<tr>

<td>Origin</td>
<td>
    <asp:DropDownList ID="ddlOrigin" runat="server" >
    </asp:DropDownList>
</td>
<td>Dest</td>
<td>
    <asp:DropDownList ID="ddlDestination" runat="server">
    </asp:DropDownList>
</td>

<td>
    Flight #</td>
<td>
    <asp:TextBox ID="txtFltNo" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
    </td>
<td>
    &nbsp;</td>
<td>
    &nbsp;</td>

<td>
    &nbsp;</td>
<td>
    &nbsp;</td>

<td>Type</td>
<td>
    <asp:DropDownList ID="ddlMovementType" runat="server">
        <asp:ListItem Selected ="True" >SELECT</asp:ListItem>
        <asp:ListItem>IN</asp:ListItem>
        <asp:ListItem>OUT</asp:ListItem>
    </asp:DropDownList>
</td>

<td>
    ULD #
</td>
<td class="style1">
    <asp:TextBox ID="txtULDNo" runat="server" MaxLength="15" Width="80px"></asp:TextBox>
    </td>

</tr>

<tr>

<td>
   From Date</td>
<td>
    <asp:TextBox ID="txtFromDate" runat="server" Width="100px"></asp:TextBox>
    <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" 
        Enabled="True" TargetControlID="txtFromDate" PopupButtonID="imgFrmDt" Format="dd/MM/yyyy">
    </asp:CalendarExtender>
    
    <asp:ImageButton ID="imgFrmDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
    
</td>

<td>
   To Date</td>
<td>
    <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox>
    <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" 
        Enabled="True" TargetControlID="txtToDate" PopupButtonID="imgToDt" Format="dd/MM/yyyy">
    </asp:CalendarExtender>
    <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
    
</td>

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
<td>
    &nbsp;</td>

<td>&nbsp;</td>
<td>
    &nbsp;</td>

<td>
    &nbsp;</td>
<td class="style1">
    &nbsp;</td>

</tr>

<tr>

<td colspan="14">
<asp:Button ID="btnList" runat="server" CssClass="button"  
        Text="List" onclick="btnList_Click"/>
    <asp:Button ID="Button1" runat="server" CssClass="button" 
        onclick="Button1_Click" Text="Clear" />
    <asp:Button ID="btnExport" runat="server" CssClass="button" 
        onclick="btnExport_Click" Text="Export" />
    </td>

</tr>

</table>
</td>
    
</tr>
</table>
</div>
        <div>
            <rsweb:ReportViewer ID="RptULDMovementHistory" runat="server" Height="450px" Width="1024px">
            </rsweb:ReportViewer>
        <%--<dd:webreportviewer ID="RptViewerULDHistory" runat="server" Height="450px" Width="1024px" xmlns:dd="datadynamics.reports.web" />--%>

    </div>
</div>

     </asp:Content>