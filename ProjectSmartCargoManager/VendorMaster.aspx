<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorMaster.aspx.cs" Inherits="ProjectSmartCargoManager.VendorMaster" MasterPageFile="~/SmartCargoMaster.Master"%>

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
    .style1
    {
        width: 3px;
    }
    </style>
   
<asp:UpdatePanel runat="server" ID="updtPnl">
<ContentTemplate>

<div id="contentarea">
<div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
<h1>Vendor Master</h1>
<div class="botline">
<table>
    <tr>
        <td>Vendor Code:</td>
        <td>
            <asp:TextBox ID="txtVendorCode" runat="server"></asp:TextBox>
        </td>
        <td>&nbsp;</td>
        <td>Vendor Name:</td>
        <td>
            <asp:TextBox ID="txtVendorName" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" onclick="btnList_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" />
        </td>
    </tr>
</table>
</div>
<div class="ltfloat">
<table>
    <tr>
       <td>Validity:</td> 
       <td colspan="7">
           <asp:TextBox ID="txtFromDate" runat="server" Width="74px"></asp:TextBox>
            <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                 Enabled="True" TargetControlID="txtFromDate" PopupButtonID="imgFromDate">
            </asp:CalendarExtender>
            <asp:ImageButton ID="imgFromDate" runat ="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
           <asp:TextBoxWatermarkExtender ID="txtFromDate_Watermark" runat="server" TargetControlID="txtFromDate" WatermarkText="From">
           </asp:TextBoxWatermarkExtender>
           
           <asp:TextBox ID="txtToDate" runat="server" Width="74px"></asp:TextBox>
           <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                 Enabled="True" TargetControlID="txtToDate" PopupButtonID="imgToDate">
            </asp:CalendarExtender>
            <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
            <asp:TextBoxWatermarkExtender ID="txtToDate_Watermark" runat="server" TargetControlID="txtToDate" WatermarkText="To">
           </asp:TextBoxWatermarkExtender>
           &nbsp;
           &nbsp;
           <asp:CheckBox ID="chkIsAct" runat="server" Text="Active"/>
       </td>
    </tr>
    <tr>
        <td>Address:</td>
        <td>
            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine"></asp:TextBox>
        </td>
        <td>&nbsp;</td>
        <td>Contact Person:</td>
        <td>
            <asp:TextBox ID="txtContactPerson" runat="server"></asp:TextBox>
        </td>
        <td class="style1">&nbsp;</td>
        <td>Currency:</td>
        <td>
            <asp:DropDownList ID="ddlCurrency" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>City:</td>
        <td>
            <asp:DropDownList ID="ddlCity" runat="server">
            </asp:DropDownList>
        </td>
        <td>&nbsp;</td>
        <td>Contact Email:</td>
        <td>
            <asp:TextBox ID="txtContctMail" runat="server"></asp:TextBox>
        </td>
        <td class="style1">&nbsp;</td>
        <td>Service Tax:</td>
        <td>
            <asp:TextBox ID="txtServiceTax" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Country:</td>
        <td>
            <asp:DropDownList ID="ddlCountry" runat="server">
            </asp:DropDownList>
        </td>
        <td>&nbsp;</td>
        <td>Contact Phone:</td>
        <td>
            <asp:TextBox ID="txtContactPh" runat="server"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="Filter_txtContactPh" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtContactPh" ValidChars="0123456789"></asp:FilteredTextBoxExtender>
        </td>
        <td class="style1">&nbsp;</td>
        <td>Billing Frequency:</td>
        <td>
           <asp:DropDownList ID="ddlBillType" runat="server">
               <asp:ListItem Selected="True">Fortnightly</asp:ListItem>
               <asp:ListItem>Daily</asp:ListItem>
               <asp:ListItem>Monthly</asp:ListItem>
           </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Contact Mobile:</td>
        <td>
            <asp:TextBox ID="txtContactMob" runat="server"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="Filter_txtContactMob" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtContactMob" ValidChars="0123456789"></asp:FilteredTextBoxExtender>
        </td>
        <td class="style1">&nbsp;</td>
        <td>Account Code:</td>
        <td>
            <asp:DropDownList ID="ddlGLCode" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="8">
            <asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="button" onclick="btnAdd_Click" />
        </td>
    </tr>
</table>

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