<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportLog.aspx.cs" Inherits="ProjectSmartCargoManager.ReportLog" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 </asp:Content>
 
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
  <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

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
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
<div id="contentarea">
<h1>Report Log</h1>
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
<div class="botline">
<table>
<tr>
    <td>From Date:</td>
    <td>
        <asp:TextBox ID="txtFromDate" runat="server" Width="70px"></asp:TextBox>
        <asp:CalendarExtender ID="ext_FromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="imgFltFromDt" Format="dd/MM/yyyy">
        </asp:CalendarExtender>
        <asp:ImageButton ID="imgFltFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
    </td>
    <td>To Date:</td>
    <td>
        <asp:TextBox ID="txtToDate" runat="server" Width="70px"></asp:TextBox>
        <asp:CalendarExtender ID="ext_ToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="imgFltToDt" Format="dd/MM/yyyy">
        </asp:CalendarExtender>
        <asp:ImageButton ID="imgFltToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
    </td>
    <td>User:</td>
    <td>
        <asp:DropDownList ID="ddlUserList" runat="server"></asp:DropDownList>
    </td>
     <td>Report:</td>
    <td>
        <asp:DropDownList ID="ddlRptList" runat="server"></asp:DropDownList>
    </td>
    <td>
        <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" onclick="btnList_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" />
    </td>
</tr>
</table>
</div>
</div>

<br />

<div class="ltfloat">

<asp:GridView ID="grdRptLog" runat="server" AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" 
HeaderStyle-CssClass="HeaderStyle" PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
SelectedRowStyle-CssClass="SelectedRowStyle" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" onpageindexchanging="grdRptLog_PageIndexChanging">

<Columns>
    <asp:TemplateField HeaderText="User Name">
    <ItemTemplate>
        <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName")%>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="IP Address">
    <ItemTemplate>
        <asp:Label ID="lblIPAddr" runat="server" Text='<%#Eval("IP")%>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Report">
    <ItemTemplate>
        <asp:Label ID="lblRptName" runat="server" Text='<%#Eval("Page")%>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Access Time">
    <ItemTemplate>
        <asp:Label ID="lblAccTime" runat="server" Text='<%#Eval("AccessTime")%>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Station">
    <ItemTemplate>
        <asp:Label ID="lblStn" runat="server" Text='<%#Eval("Station")%>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    </Columns>

</asp:GridView>

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
  
    
</ContentTemplate>
</asp:UpdatePanel>
  
</asp:Content>
