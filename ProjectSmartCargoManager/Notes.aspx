<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notes.aspx.cs" Inherits="ProjectSmartCargoManager.Notes" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link href="style/style.css" rel="stylesheet" type="text/css" />
    <link href="style/jetGridView.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    function Validate() {

//        var AWBPrefix = document.getElementById('ctl00_ContentPlaceHolder1_txtAWBPrefix').value;
//        var AWBNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;
//        var FltDt = document.getElementById('ctl00_ContentPlaceHolder1_txtFltDt').value;
//        var FltNO = document.getElementById('ctl00_ContentPlaceHolder1_txtFltNo').value;
//        var User = document.getElementById('ctl00_ContentPlaceHolder1_txtUser').value;
        var Comment = document.getElementById('ctl00_ContentPlaceHolder1_txtComments').value;

//        if (AWBPrefix == "") {
//            alert('Enter AWB Prefix');
//            return false;
//        }
//        if (AWBNo == "") {
//            alert('Enter AWN No');
//            return false;
//        }
//        if (FltDt == "") {
//            alert('Enter Flight Date');
//            return false;
//        }
//        if (FltNO == "") {
//            alert('Enter Flight No');
//            return false;
//        }
//        if (User == "All") {
//            alert('Select User');
//            return false;
        //        }

        if (Comment == "") {
            alert('Please enter your comment!!!');
            return false;
        }
        
    
    }
</script>

</head>
 
<body style="background:#fff;">
<form id="form1" runat="server">
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
<div id="contentarea" style="width:810px;">
<h1>Notes</h1>
<div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
<div class="botline">
<table width="80%" cellpadding="2" cellspacing="2">
    <tr>
        <td>AWB#</td>
        <td>
            <asp:TextBox ID="txtAWBPrefix" runat="server" Width="35px" MaxLength="5"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtAWBNo" runat="server" Width="80px" MaxLength="8"></asp:TextBox>
        </td>
        <td>Flight Dt</td>
        <td>
            <asp:TextBox ID="txtFltDt" runat="server" Width="75px"></asp:TextBox>
            <asp:CalendarExtender ID="txtFltDt_CalendarExtender" runat="server" TargetControlID="txtFltDt"
            Format="dd/MM/yyyy" PopupButtonID="imgFltDt">
            </asp:CalendarExtender>
            <asp:ImageButton ID="imgFltDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
        </td>
        <td>Flight #</td>
        <td>
            <asp:TextBox ID="txtFltNo" runat="server" Width="50px"></asp:TextBox>
        </td>
        <td>User</td>
        <td>
            <asp:TextBox ID="txtUser" runat="server" Width="80px"></asp:TextBox>
        </td>
        <td>Comments</td>
        <td>
            <asp:TextBox ID="txtComments" runat="server" Width="100px" TextMode="MultiLine"></asp:TextBox>
        </td>
        <%--<td>Active</td>
        <td>
            <asp:CheckBox ID="chkIsActive" runat="server" />
        </td>--%>
    </tr>
    <tr>
        <td colspan="14">
            <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" onclick="btnList_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" />
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" onclick="btnSave_Click" OnClientClick="javascript:return Validate();"/>
        </td>
    </tr>
</table>
</div>

<div class="ltfloat">

<asp:GridView ID="grdNoteList" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
     AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
     PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle" 
     onpageindexchanging="grdNoteList_PageIndexChanging" Width="100%">
     
     <Columns>
        <%--<asp:TemplateField HeaderText="SrNo" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("SrNO")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>--%>
        
        <asp:TemplateField HeaderText="AWB#" HeaderStyle-Width="80px">
            <ItemTemplate>
                <asp:Label ID="lblAWBNo" runat="server" Text='<%#Eval("AWBNo")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Flight Date" HeaderStyle-Width="80px">
            <ItemTemplate>
                <asp:Label ID="lblFltDt" runat="server" Text='<%#Eval("FlDt")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Flight No" HeaderStyle-Width="80px">
            <ItemTemplate>
                <asp:Label ID="lblFltNo" runat="server" Text='<%#Eval("FltNo")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="User" HeaderStyle-Width="80px">
            <ItemTemplate>
                <asp:Label ID="lblUser" runat="server" Text='<%#Eval("User")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Comments" HeaderStyle-Width="300px">
            <ItemTemplate>
                <asp:Label ID="lblComments" runat="server" Text='<%#Eval("Comments")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <%--<asp:TemplateField HeaderText="Active">
            <ItemTemplate>
                <asp:CheckBox ID="chkActive" runat="server" Checked='<%#Eval("isActive")%>' />
            </ItemTemplate>
        </asp:TemplateField>--%>
        
        <asp:TemplateField HeaderText="Inserted On" HeaderStyle-Width="150px">
            <ItemTemplate>
                <asp:Label ID="lblUpdatedOn" runat="server" Text='<%#Eval("UpdatedOn")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Updated By" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lblUpdatedBy" runat="server" Text='<%#Eval("UpdatedBy")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
     </Columns>
    </asp:GridView>

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
    
</ContentTemplate>
</asp:UpdatePanel>
 </form>
</body>
</html>
