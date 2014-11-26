<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmStockList.aspx.cs" MasterPageFile ="~/SmartCargoMaster.Master"  Inherits="ProjectSmartCargoManager.frmStockList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript">
  function ViewPanelSplit() {
             document.getElementById('Lightsplit').style.display = 'block';
             document.getElementById('fadesplit').style.display = 'block';
         }
         function HidePanelSplit() {
             document.getElementById('Lightsplit').style.display = 'none';
             document.getElementById('fadesplit').style.display = 'none';
         }
         </script>
 
<script type="text/javascript">
        function CheckOtherIsCheckedByGVID(spanChk) {

            var IsChecked = spanChk.checked;
            if (IsChecked) {
            }
            var CurrentRdbID = spanChk.id;
            var Chk = spanChk;
            Parent = document.getElementById("<%=gvAddressGrpFTP.ClientID%>");
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != CurrentRdbID && items[i].type == "radio") {
                    if (items[i].checked) {
                        items[i].checked = false;

                    }
                }
            }
        }
</script>

<style>
.black_overlaynew
		{
			display: none;
			position: absolute;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 200%;
			background-color: black;
			z-index:1001;
			-moz-opacity:0.8;
			opacity:0.4;
			filter:alpha(opacity=80);
		}
	.white_contentnew 
		{
		    margin:0 auto;
			display: none;
			position:fixed;
			top: 30%;
			left: 30%;
			padding: 16px;
			border: 16px solid #ccdce3;
			background-color: white;
			z-index:1002;
		
            
		}
		.black_overlaymsg{
			display: none;
			position: fixed;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 100%;
			background-color: White;
			z-index:1001;
			-moz-opacity: 0.8;
			opacity:.80;
			filter: alpha(opacity=80);
		}
		.white_contentmsg {
			display: none;
			position: fixed;
			top: 45%;
			left: 45%;
			width: 5%;
			height: 5%;
			padding: 16px;
			background-color: Transparent;
			z-index:1002;
			
		}
		
        .style6
        {
            width: 87px;
        }
		
    </style>
 
    <asp:ToolkitScriptManager ID="TookScriptManager" runat="server"></asp:ToolkitScriptManager>
    <div id="WorkPage">
    <div id="singlecol">
<div class="pagetitle">Available Stock List</div>
<br />
<asp:Label ID="lblStatus" runat="server" ForeColor="Red" 
            style="font-weight: 700; font-size: medium"></asp:Label>
<table cellpadding="3" cellspacing="3">


<tr>


<td >
    Station :
</td>
<td>
    <asp:DropDownList ID="ddlWareHouse" runat="server">
    </asp:DropDownList>
</td>

<td >
    ULD Type :
</td>
<td>
    <asp:DropDownList ID="ddlULDType" runat="server">
    </asp:DropDownList>
</td>

<td >
    ULD Status :
</td>
<td>
    <asp:DropDownList ID="ddlULDStatus" runat="server">
    </asp:DropDownList>
</td>

<td>
<asp:Button ID="btnSearch" runat="server" Font-Names="Verdana" CssClass="button"  
        Text="SEARCH" onclick="btnSearch_Click" /> 
    </td>
    
</tr>

</table>
<br />

<br />
   <asp:GridView ID="gvAddressGrpFTP" runat="server" AutoGenerateColumns="False" 
             ShowFooter="false"    HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass=
            "trcolor" 
             PagerStyle-CssClass="PagerStyle" 
            onselectedindexchanged="gvAddressGrpFTP_SelectedIndexChanged"      >
    <Columns>
    <asp:TemplateField  HeaderText="Station" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:Label ID="lblStation" runat="server" Text='<%# Eval("Station") %>'></asp:Label>
    </ItemTemplate></asp:TemplateField>
    <asp:TemplateField HeaderText="ULD Type" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:Label ID="txtULDType" runat="server" Text='<%# Eval("ULDType") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Min Req Avl Qty" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:Label ID="txtMinAvlQty" runat="server" Text='<%# Eval("MinAvlQuantity") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Available Qty" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:Label ID="txtAvlQty" runat="server" Text='<%# Eval("AvlQuantity") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Email ID" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:Label ID="txtEmailID" runat="server" Text='<%# Eval("EmailID") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField  ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Send" OnClick="btnSave_Click"  />
    </ItemTemplate>
    </asp:TemplateField >
    
    </Columns>
         <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                    <FooterStyle CssClass="grdrowfont"></FooterStyle>
                    <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                    <RowStyle CssClass="grdrowfont"></RowStyle>
    </asp:GridView>
    
</div>
<div id="Lightsplit" class="white_contentnew">
<div style="float:inherit;">
</div>
 <table cellpadding="6" cellspacing="6">
 <tr>
 <td><asp:Label ID="Label1" runat="server" Text="EmailID :"></asp:Label></td>
 <td><asp:TextBox ID="txtEmailIDPopUp" runat="server" ToolTip="Enter Additional Email ID Separated by (,)" TextMode="MultiLine" Width="328px" Height="61px"></asp:TextBox>
 
 </td>
 </tr>
 <tr>
 <td>&nbsp;</td>
 <td>
     <asp:Button ID="btnSend" runat="server" CssClass="button" 
         onclick="btnSend_Click" Text="Send" ValidationGroup="send" />
     <input id="btnSplitCancel" class="button" onclick="HidePanelSplit();" 
         size="150%" type="button" value="Close" /></td>
 </tr>
 <tr><asp:RequiredFieldValidator ID="RequiredFieldValidator_txtEmailID" runat="server" ErrorMessage="Please Enter Email ID" ControlToValidate="txtEmailIDPopUp" ValidationGroup="send"></asp:RequiredFieldValidator>
 <asp:RegularExpressionValidator ID="RegularExpression_txtEmailID" runat="server" ErrorMessage="Please Enter Email ID in correct format" ControlToValidate="txtEmailIDPopUp" ValidationGroup="send" ValidationExpression="^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([,.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$"></asp:RegularExpressionValidator></tr>
 
 </table>

</div>
		<div id="fadesplit" class="black_overlaynew"></div>

</div>
</asp:Content>

