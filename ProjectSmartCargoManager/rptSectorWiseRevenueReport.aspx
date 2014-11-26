<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptSectorWiseRevenueReport.aspx.cs" Inherits="ProjectSmartCargoManager.rptSectorWiseRevenueReport" %>
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
         <script type="text/javascript">
        function DisableButton() {
            document.getElementById("<%=btnList.ClientID %>").disabled = true;
            document.getElementById("<%=btnClear.ClientID %>").disabled = true;
            document.getElementById("<%=btnExport.ClientID %>").disabled = true;
        }
        window.onbeforeunload = DisableButton;
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
     .style2
     {
         height: 29px;
         width: 67px;
     }
     .style4
     {
         height: 29px;
         width: 51px;
     }
     .style5
     {
         height: 29px;
         width: 140px;
     }
     .style9
     {
         height: 29px;
         width: 144px;
     }
    </style>
    
     
 <div id="contentarea">
 <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
 <h1>Cargo Revenue Tracking Report</h1>

<div class="botline">
<table width="100%">
<tr>
<td class="style2">From Date</td>
<td class="style9">
    <asp:TextBox ID="txtFromdate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate" PopupButtonID="imgfromdate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgfromdate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" />
</td>
<td class="style4">To Date</td>
<td class="style5"><asp:TextBox ID="txtTodate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate" PopupButtonID="imgtodate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgtodate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" /> </td>
<%--<td class="style1">
    Staion Type
</td>
<td class="style1">
<asp:DropDownList ID="ddlStationType" runat="server">
<asp:ListItem Value="All">All</asp:ListItem>
<asp:ListItem Value="DOM">Domestic</asp:ListItem>
<asp:ListItem Value="INT">International</asp:ListItem>
</asp:DropDownList></td>--%>
                <td>
                   Flights</td>
                <td>
                    <asp:DropDownList ID="ddlStationType" runat="server">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem Value="DOM">Domestic</asp:ListItem>
                        <asp:ListItem Value="INT">International</asp:ListItem>
                    </asp:DropDownList>
                </td>

<td>
    Station</td>
<td>
    <asp:DropDownList ID="ddlStation" runat="server">
    </asp:DropDownList>
</td>
                </tr>
                <tr>
<td>Carrier</td>
 <td class="style5">
<asp:DropDownList ID="ddlCarrier" runat="server">
 </asp:DropDownList>
</td>
<td>Revenue Type</td>
<td>
<asp:DropDownList ID="ddlRevenueType" runat="server">
<asp:ListItem Text="Station Wise" Value="ST"></asp:ListItem>
<asp:ListItem Text="Sector Wise" Value="SC"></asp:ListItem>
<asp:ListItem Text="Flight Wise" Value="FW"></asp:ListItem>
<asp:ListItem Text="Tail Wise" Value="FE"></asp:ListItem>
</asp:DropDownList>
</td>
</tr>
</table>
<table width="100%">
<tr>
<td>
    <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" onclick="btnList_Click" 
         />
    
    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" 
        />
      
        <asp:Button ID="btnExport" runat="server" CssClass="button"  
                        Text="Export" onclick="btnExport_Click"   />
</td>
</tr>
</table>
</div>
 

<div style="border: thin solid #000000; float: left">
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1024px">
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
</asp:Content>
