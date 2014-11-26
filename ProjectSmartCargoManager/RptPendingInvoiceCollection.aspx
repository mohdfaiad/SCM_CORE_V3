<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptPendingInvoiceCollection.aspx.cs" Inherits="ProjectSmartCargoManager.RptPendingInvoiceCollection" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<%--<script type="text/javascript">

    function ValidateRemarks() {
        var PendingAmt = document.getElementById("<%= hdPendingAmt.ClientID %>");
        var CollectedAmt = document.getElementById("<%= txtCollectedAmount.ClientID %>");
        var ParitalRemarks = document.getElementById("<%= txtPPRemarks.ClientID %>");
        if (ParitalRemarks.value == "") {
            if (PendingAmt.value.charAt(0) != "-") {
                if (PendingAmt.value != "0") {
                    if (PendingAmt.value != CollectedAmt.value) {
                        alert("Please Enter Partial Payment Remarks !");
                        ParitalRemarks.focus();
                        return false;
                    }
                }
            }
        }
        return true;
    }

    function ValidateCardDetails() {

        var IsChecked = false;

        if (document.getElementById("<%= rbtnAE.ClientID %>").checked) {
            IsChecked = true;
        } else if (document.getElementById("<%= rbtnMC.ClientID %>").checked) {
            IsChecked = true;
        } else if (document.getElementById("<%= rbtnME.ClientID %>").checked) {
            IsChecked = true;
        } else if (document.getElementById("<%= rbtnVI.ClientID %>").checked) {
            IsChecked = true;
        }

        if (IsChecked == false) {
            alert("Please Select the type of Card !");            
            return false;
        }

        var cardNo = document.getElementById("<%= txtCardNumber.ClientID %>");

        if (cardNo.value == "") {
            alert("Please enter Card no to proceed!");
            cardNo.focus();
            return false;
        }

        if (cardNo.value.length < 16) {
            alert("Please enter valid Card no to proceed!");
            cardNo.focus();
            return false;
        }

        var e = document.getElementById("<%= ddlMonth.ClientID %>");
        var strMonth = e.options[e.selectedIndex].value;
        
        if (strMonth == "MM") {
            alert("Please enter valid Month to proceed!");
            e.focus();
            return false;
        }

        e = document.getElementById("<%= ddlYear.ClientID %>");
        strMonth = e.options[e.selectedIndex].value;

        if (strMonth == "YYYY") {
            alert("Please enter valid Year to proceed!");
            e.focus();
            return false;
        }

        var CVVNo = document.getElementById("<%= txtCVV.ClientID %>");

        if (CVVNo.value == "") {
            alert("Please enter CVV no to proceed!");
            CVVNo.focus();
            return false;
        }

        if (CVVNo.value.length < 3) {
            alert("Please enter valid CVV no to proceed!");
            CVVNo.focus();
            return false;
        }

        var Cardholdername = document.getElementById("<%= txtCardholdername.ClientID %>");

        if (Cardholdername.value == "") {
            alert("Please enter Card holder name to proceed!");
            Cardholdername.focus();
            return false;
        }

        return true;
    }
    
</script>--%>

</asp:Content> 

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contentarea">
  
<div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>  

<h1>Aging Report</h1> 

<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</asp:ToolkitScriptManager>

<div class="botline"> 
<table width="100%" cellpadding="3">
<tr>
<td>Invoice Dt *</td>
    <td>
        <asp:TextBox ID="txtInvoiceFrom" runat="server" Width="80px"></asp:TextBox>
        <asp:ImageButton ID="btnInvoiceFrom" runat="server" ImageUrl="~/Images/calendar_2.png" ImageAlign="AbsMiddle" />
        <asp:CalendarExtender ID="CEInvoiceFrom" Format="dd/MM/yyyy" TargetControlID="txtInvoiceFrom" PopupButtonID="btnInvoiceFrom" runat="server" PopupPosition="BottomLeft">
        </asp:CalendarExtender>
        &nbsp;
        <asp:TextBox ID="txtInvoiceTo" runat="server" Width="80px"></asp:TextBox>
        <asp:ImageButton ID="btnInvoiceTo" runat="server" ImageUrl="~/Images/calendar_2.png" ImageAlign="AbsMiddle" />
        <asp:CalendarExtender ID="CEInvoiceTo" Format="dd/MM/yyyy" TargetControlID="txtInvoiceTo" PopupButtonID="btnInvoiceTo" runat="server" PopupPosition="BottomLeft">
        </asp:CalendarExtender>
    </td>
    <td>Invoice Number</td>
    <td>
        <asp:TextBox ID="txtInvoiceNumber" runat="server" Width="90px"></asp:TextBox>
    </td>
    <td>Bill Type</td>
    <td>
        <asp:DropDownList ID="ddlBillType" runat="server" Width="120px" >
        <asp:ListItem Text="All" Value=""></asp:ListItem>
        <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
        <asp:ListItem Text="Fortnightly" Value="Fortnightly"></asp:ListItem>
        <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td>Agent</td>
    <td>
        <asp:DropDownList ID="ddlAgentCode" runat="server" Width="80px" AutoPostBack="True" onselectedindexchanged="ddlAgentCode_SelectedIndexChanged" >
        </asp:DropDownList>
        &nbsp;    
        <asp:DropDownList ID="ddlAgentName" runat="server" Width="170px" AutoPostBack="True" onselectedindexchanged="ddlAgentName_SelectedIndexChanged" >
        </asp:DropDownList>
    </td>
    <td>Origin</td>
    <td>
        <asp:TextBox ID="txtOrigin" runat="server" Width="90px"></asp:TextBox>
        <asp:AutoCompleteExtender ID="txtOrigin_AutoCompleteExtender" runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
        TargetControlID="txtOrigin">
        </asp:AutoCompleteExtender>
    </td>
    <td>AWB #</td>
    <td>
        <asp:TextBox ID="txtAWBPrefix" runat="server" Width="40px" MaxLength="3"></asp:TextBox>
        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" 
        runat="server" TargetControlID="txtAWBPrefix" WatermarkText="Prefix">
        </asp:TextBoxWatermarkExtender> 
        &nbsp;
        <asp:TextBox ID="txtAWBNumber" runat="server" Width="90px" MaxLength="10"></asp:TextBox>
    </td>
</tr>
<tr>
    <td colspan="3">
        <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" onclick="btnList_Click"/>
        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click" />
        <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" onclick="btnClear_Click"/>
    </td>
</tr>
</table>  
</div>

</div>

<div style=" border: thin solid #000000; float: left;">
    <rsweb:ReportViewer ID="rptPendingInvCollection" runat="server" Width="1024px" >
    </rsweb:ReportViewer>
</div>

</asp:Content>