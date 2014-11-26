<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExchRateMaster.aspx.cs"  Title="Exchange Rate" Inherits="ProjectSmartCargoManager.ExchRateMaster" MasterPageFile="~/SmartCargoMaster.Master"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <style>
.styleUpper
        {
            text-transform: uppercase;
        }
</style>
<title></title>
</asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:UpdatePanel ID="UP1" runat="server">
<ContentTemplate>

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
    </style>

<asp:ToolkitScriptManager ID="ScriptMgr1" runat="server"></asp:ToolkitScriptManager>
<div id="contentarea"> 

<div class="msg">
<asp:Label ID="lblStatus" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="Large"></asp:Label>
</div>
<h1>Exchange Rate Master</h1>

 
<div class="botline">
<table width="100%">
<tr>
<td>Base Currency:</td>

<td style="font-size:large;font-weight:bold">USD</td>

<td>Currency:</td>

<td>
<asp:DropDownList ID="ddlCurr" runat="server"></asp:DropDownList>
</td>

<td>Exchange Rate:</td>

<td>
<asp:TextBox ID="txtExchRt" runat="server" Width="40px"></asp:TextBox>
    <asp:RequiredFieldValidator ID="txtExchRt_validate" runat="server" ErrorMessage="*" ControlToValidate="txtExchRt"></asp:RequiredFieldValidator>
     <asp:RegularExpressionValidator ID="txtExchRt_regex" runat="server" ErrorMessage="Only Digits"
ControlToValidate="txtExchRt" ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>
</td>
<td></td>
</tr>
<tr>
<td>Rate Type:</td>

<td>
<asp:DropDownList ID="ddlRtType" runat="server">
</asp:DropDownList>
</td>

<td>Valid From:</td>
<td>
<asp:TextBox ID="txtDtFrm" runat="server" Width="67px"></asp:TextBox>
<asp:CalendarExtender ID="ext_txtDtFrm" runat="server" TargetControlID="txtDtFrm" PopupButtonID="imgFromDt" Format="dd/MM/yyyy">
</asp:CalendarExtender>
<asp:ImageButton ID="imgFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
<asp:RequiredFieldValidator ID="valid_txtDtFrm" runat="server" ErrorMessage="*" ControlToValidate="txtDtFrm"></asp:RequiredFieldValidator>
</td>

<td>Valid To:</td>
<td>
<asp:TextBox ID="txtDtTo" runat="server" Width="67px"></asp:TextBox>
<asp:CalendarExtender ID="ext_txtDtTo" runat="server" TargetControlID="txtDtTo" PopupButtonID="imgToDt" Format="dd/MM/yyyy">
</asp:CalendarExtender>
<asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
<asp:RequiredFieldValidator ID="validate_txtDtTo" runat="server" ErrorMessage="*" ControlToValidate="txtDtTo"></asp:RequiredFieldValidator>
</td>
</tr>
</table>

<div class="ltfloat">
<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click"/>
<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" CausesValidation="false"/>
<asp:Button ID="btnList" runat="server" Text="List" onclick="btnList_Click" CssClass="button" CausesValidation="false"/>
<asp:Button ID="btnExport" runat="server" Text="Export" onclick="btnExport_Click" CssClass="button" CausesValidation="false" />
</div>

</div>
<div class="ltfloat" style="width:100%">
<asp:GridView ID="exchGrd" runat="server" AllowPaging="true" AutoGenerateColumns="false" PageSize="10"
OnPageIndexChanging="exchGrd_PageIndexChanging" OnRowCommand="exchGrd_RowCommand" OnRowEditing="exchGrd_RowEditing" Width="100%"
AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
<Columns>
<asp:TemplateField HeaderText="ID" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblGrdId" runat="server" Text='<%#Eval("ICRID")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Currency" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblGrdCurr" runat="server" Text='<%#Eval("CurrencyCode")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<%--<asp:TemplateField HeaderText="USD Rate" ItemStyle-HorizontalAlign="Right" >
<ItemTemplate>
<asp:Label ID="lblGrdUSRt" runat="server" Text='<%#Eval("USDRate")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>--%>

<asp:TemplateField HeaderText="IATA Rate" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblGrdCurrIRt" runat="server" Text='<%#Eval("CurrencyIATARate")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<%--<asp:TemplateField HeaderText="USD Conversion" ItemStyle-HorizontalAlign="Right">
<ItemTemplate>
<asp:Label ID="lblGrdUSConv" runat="server" Text='<%#Eval("USDConversion")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>--%>

<%--<asp:TemplateField HeaderText="INR Conversion" ItemStyle-HorizontalAlign="Right">
<ItemTemplate>
<asp:Label ID="lblGrdINConv" runat="server" Text='<%#Eval("INRConversion")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>--%>

<%--<asp:TemplateField HeaderText="Base Rate" ItemStyle-HorizontalAlign="Right">
<ItemTemplate>
<asp:Label ID="lblGrdCurrBaseRt" runat="server" Text='<%#Eval("CurrencyBaseRate")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>--%>

<asp:TemplateField HeaderText="Valid From" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblGrdDtFrm" runat="server" Text='<%#Eval("ValidFrom")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Valid To" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblGrdDtTo" runat="server" Text='<%#Eval("ValidTo")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Type" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblGrdType" runat="server" Text='<%#Eval("Type")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<%--<asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblisAct" runat="server" Text='<%#Eval("isActive")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>--%>


<asp:ButtonField CommandName="Edit" Text="Edit">
<ItemStyle Width="50px"/>
</asp:ButtonField>
<asp:ButtonField CommandName="DeleteRecord" Text="Delete">
<ItemStyle Width="50px"/>
</asp:ButtonField>
</Columns>
</asp:GridView></div>
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


</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="btnExport" />
</Triggers>
</asp:UpdatePanel>
</asp:Content>