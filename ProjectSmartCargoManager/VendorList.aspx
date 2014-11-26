<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorList.aspx.cs" Inherits="ProjectSmartCargoManager.VendorList" MasterPageFile="~/SmartCargoMaster.Master"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">

        function setVendorName(DropDown) {
            var VendorName = DropDown.id.replace('ddlVendorCode', 'txtVendorName');
            var e = document.getElementById(DropDown.id);
            var strUser = e.options[e.selectedIndex].text;
            var s = strUser.split('-');
            document.getElementById(VendorName).value = s[1];
            return false;
        }
    </script>

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
    .style1
    {
        width: 3px;
    }
    </style>
 
 <asp:UpdatePanel ID="UP1" runat="server">
 <ContentTemplate>
 
<div id="contentarea">
<div class="msg">
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
<h1>List Vendor</h1>
<div class="botline">
<table>
    <tr>
        <td>Vendor Code</td>
        <td>
            <asp:DropDownList ID="ddlVendorCode" runat="server" onchange="javascript:setVendorName(this);">
            </asp:DropDownList>
        </td>
        <td>Vendor Name</td>
        <td>
            <asp:TextBox ID="txtVendorName" runat="server" Width="90px" Enabled="false"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
                onclick="btnList_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
                onclick="btnClear_Click" />
        </td>
    </tr>
</table>
</div>
<div class="ltfloat">
    <asp:GridView ID="grdVendorList" runat="server" AutoGenerateColumns="false" Width="100%"
     PageSize="10" AllowPaging="true" AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle"
     HeaderStyle-CssClass="HeaderStyle" PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
     SelectedRowStyle-CssClass="SelectedRowStyle" onpageindexchanging="grdVendorList_PageIndexChanging" 
     onrowcommand="grdVendorList_RowCommand" onrowediting="grdVendorList_RowEditing">
     
     <Columns>
         <asp:TemplateField Visible="false">
             <ItemTemplate>
                 <asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("SerialNumber")%>'></asp:Label>
             </ItemTemplate>
         </asp:TemplateField>
         
         <asp:TemplateField HeaderText="Vendor Code">
             <ItemTemplate>
                 <asp:Label ID="lblVendorCode" runat="server" Text='<%#Eval("VendorCode")%>'></asp:Label>
             </ItemTemplate>
         </asp:TemplateField>
         
         <asp:TemplateField HeaderText="Vendor Name">
             <ItemTemplate>
                 <asp:Label ID="lblVendorName" runat="server" Text='<%#Eval("VendorName")%>'></asp:Label>
             </ItemTemplate>
         </asp:TemplateField>
         
         <asp:TemplateField HeaderText="Valid From">
             <ItemTemplate>
                 <asp:Label ID="lblValidFrm" runat="server" Text='<%#Eval("ValidFrom")%>'></asp:Label>
             </ItemTemplate>
         </asp:TemplateField>
         
         <asp:TemplateField HeaderText="Valid To">
             <ItemTemplate>
                 <asp:Label ID="lblValidTo" runat="server" Text='<%#Eval("ValidTo")%>'></asp:Label>
             </ItemTemplate>
         </asp:TemplateField>
         
         <asp:TemplateField HeaderText="City">
             <ItemTemplate>
                 <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City")%>'></asp:Label>
             </ItemTemplate>
         </asp:TemplateField>
         
         <asp:ButtonField CommandName="Edit" Text="Edit" />
         <%--<asp:ButtonField CommandName="View" Text="View" />--%>
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
<div id="msgfade" class="black_overlaymsg"></div>

</ContentTemplate>
 </asp:UpdatePanel>

</asp:Content>

