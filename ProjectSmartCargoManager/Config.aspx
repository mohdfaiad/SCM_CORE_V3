<%@ Page Language="C#" Title="AWB Designator" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="ProjectSmartCargoManager.Config" MasterPageFile="~/SmartCargoMaster.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title></title>
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
       .legend
        {
         font-size:large;
         font-weight:bold;
        }
    </style>
 <script>
     function add() {
         var text = document.getElementById("<%= txtDesigCode.ClientID%>").value;
         if (text == "") {
             alert('Designator Code Cannot be Blank')
             return false;
         }
         else {
             var opt = document.createElement("option");
             document.getElementById("<%= listPrefix.ClientID%>").options.add(opt);
             opt.text = text;
             document.getElementById("<%= txtDesigCode.ClientID%>").value = "";
             var hdnvalue = document.getElementById("<%= prefixhdn.ClientID%>").value;
             document.getElementById("<%= prefixhdn.ClientID%>").value = hdnvalue + ',' + text;
             var finalval = document.getElementById("<%= prefixhdn.ClientID%>").value;
             finalval = finalval.slice(1, document.getElementById("<%= prefixhdn.ClientID%>").value.length);
             return false;
         }
     }
     function removeItem() {
         var i;
         var list = document.getElementById("<%= listPrefix.ClientID%>")
         for (i = list.length - 1; i >= 0; i--) {
             if (list.options[i].selected) {
                 list.remove(i);
                 return false;
             } 
         }
     }
     function addCnote() {
         var text = document.getElementById("<%= txtCnoteType.ClientID%>").value;
         if (text == "") {
             alert('CNote Type Cannot be blank');
             return false;
         }
         else {
             var opt = document.createElement("option");
             document.getElementById("<%= listCnote.ClientID%>").options.add(opt);
             opt.text = text;
             var cnotehdnval = document.getElementById("<%= hdnCnote.ClientID%>").value;
             document.getElementById("<%= hdnCnote.ClientID%>").value = cnotehdnval + ',' + text;
             var finalval = document.getElementById("<%= hdnCnote.ClientID%>").value;
             finalval = finalval.slice(1, document.getElementById("<%= hdnCnote.ClientID%>").value.length);
             document.getElementById("<%= txtCnoteType.ClientID%>").value = "";
             return false;
         } 
     }
     function removeCnote() {
         var i;
         var list = document.getElementById("<%= listCnote.ClientID%>")
         for (i = list.length - 1; i >= 0; i--) {
             if (list.options[i].selected) {
                 list.remove(i);
                 return false;
             }
         }
     }
 </script>
 </asp:Content>
 
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
 <asp:ToolkitScriptManager ID="scriptmgr1" runat="server">
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
    </style>


  <asp:UpdatePanel ID="UP1" runat="server">
  <ContentTemplate>
 
 <%--Airline--%>
 
 <div id="contentarea">
 <h1>Airline</h1>
 <br />
 <table width="100%"><tr><td valign="top">
 <fieldset runat="server">
 <legend class="legend">Airline</legend>
 <asp:Label runat="server" ID="lblStatus" Font-Bold="True"></asp:Label>

 <table width="100%">
 <tr>
 <td>Airline Prefix*</td>
 <td>
     <asp:TextBox ID="txtPrefix" runat="server" Width="37px"></asp:TextBox></td>
 </tr>
<tr>
<td>Designator Code*</td>
<td><asp:TextBox ID="txtDesigCode" runat="server" Width="37px"></asp:TextBox></td>
</tr>
<tr>
<td></td>
<td>
    <asp:ListBox ID="listPrefix" runat="server" Height="50px" Width="55px"></asp:ListBox>
</td>
<td><asp:Button ID="btnAdd" runat="server" Text="Add" OnClientClick="javascript:return add();" CssClass="button"/>
<br />
<asp:Button ID="btnRemove" runat="server" Text="Remove" OnClientClick="javascript:return removeItem();" CssClass="button"/>
</td>
</tr>
<tr>
<td>Airline Currency*</td>
<td>
    <asp:DropDownList ID="ddlCurrency" runat="server">
    </asp:DropDownList>
</td>
<td>
<%-- <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" CssClass="button"/>--%> 
</td>
</tr>
</table>
<asp:HiddenField runat="server" ID="prefixhdn" />
</fieldset>

</td><td valign="top">
 
 <%--CNOTE--%>
<fieldset runat="server">
<legend class="legend">CNOTE</legend>
<asp:Label ID="lblCnote" runat="server" Font-Font-Bold="true" Font-Bold="True"></asp:Label>
<br />
<table width="100%">
<tr>
<td>CNOTE Type*</td>
<td>
<asp:TextBox ID="txtCnoteType" runat="server" Width="37px"></asp:TextBox></td>
</tr>
<tr>
<td></td>
<td>
<asp:ListBox ID="listCnote" runat="server" Height="50px" Width="55px"></asp:ListBox>
</td>
<td>
<asp:Button ID="btnCnoteAdd" runat="server" Text="Add" OnClientClick="javascript:return addCnote()"
CssClass="button" />
<br />
<asp:Button ID="btnCnoteRemove" runat="server" Text="Remove" OnClientClick="javascript:return removeCnote()"
CssClass="button"/>
</td>
</tr>
<tr>
<td>
CNOTE Validation
</td>
<td>
    <asp:DropDownList ID="ddlCnote" runat="server">
    <asp:ListItem Text="Mod7"></asp:ListItem>
    </asp:DropDownList>
</td>
<td>
<%--<asp:Button ID="btnCnoteSave" runat="server" Text="Save" CssClass="button" onclick="btnCnoteSave_Click"/>--%>
</td>
</tr>
 </table>
 <asp:HiddenField ID="hdnCnote" runat="server" />
 </fieldset>
 </td></tr></table>
 <br />
 
 <%--Exchange Rate--%>
 <fieldset runat="server">
 <legend class="legend">Exchange Rates</legend>
 <asp:Label ID="lblExch" runat="server" Font-Bold="true"></asp:Label>
 <br />
 <table width="40%">
 <tr>
 <td>Exchange Rate Type*</td>
 <td>
     <asp:TextBox ID="txtExchng" runat="server" Width="37px"></asp:TextBox></td>
 </tr>
 <tr>
 <td></td>
 <td>
     <asp:ListBox ID="listExchng" runat="server"></asp:ListBox>
 </td>
 <td>
<asp:Button ID="btnExchAdd" runat="server" Text="Add" onclick="btnExchAdd_Click" CssClass="button"/>
<br />
<asp:Button ID="btnExchRemove" runat="server" Text="Remove" 
         onclick="btnExchRemove_Click" CssClass="button"/>
 </td>
 </tr>
 </table>
 </fieldset>
<asp:Button ID="btnSaveAll" runat="server" Text="Save" CssClass="button" 
onclick="btnSaveAll_Click" Height="32px" Width="60px"/><br /><br />
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
 </asp:UpdatePanel>
  </asp:Content>
 