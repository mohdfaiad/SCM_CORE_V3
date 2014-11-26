<%@ Page MasterPageFile="~/SmartCargoMaster.Master" Language="C#" AutoEventWireup="true" CodeBehind="frmCCAProcessing.aspx.cs" Inherits="ProjectSmartCargoManager.frmCCAProcessing" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
function show_alert() {
var msg = "CCA Generated";
alert(msg);
}
function show_alert1() {
    var msg = "CCA Generation failed";
    alert(msg);
}
function show_alert2() {
    var msg = "Current & Revised Are Identical";
    alert(msg);
}
function ReCalculate() {
    var Calculate = document.getElementById("<%= btnCalculate.ClientID %>");
    var Accept = document.getElementById("<%= btnAccept.ClientID %>");
    var Reject = document.getElementById("<%= btnReject.ClientID %>");
    var Save = document.getElementById("<%= btnSave.ClientID %>");
    var Selection=document.getElementById("<%= hdSelection.ClientID %>").value;
//    alert("inside function");
//    alert(Selection);
    if (Selection == "WorkFlow") {
        Calculate.value = "Re-Calculate";
        Calculate.disabled = false;
        Accept.disabled = true;
        Reject.disabled = true;
    }
    if (Selection == "") {

        Calculate.value = "Re-Calculate";
        Calculate.disabled = false;
        Save.disabled = true;
        
    
    }
}


function ValidationWeightChrgsPP() {
    if (typeof (Page_Validators) == "undefined") return;
    var RequiredName = document.getElementById('<%= RegularExpressionValidator1.ClientID%>');
    ValidatorValidate(RequiredName);
    if (!RequiredName.isvalid) {
        alert("Invalid Format! Ex:(00.00)");
    }
    else {
       
    }
}

function ValidationWeightChrgsCC() {
    if (typeof (Page_Validators) == "undefined") return;
    var RequiredName = document.getElementById('<%= RegularExpressionValidator2.ClientID%>');
    ValidatorValidate(RequiredName);
    if (!RequiredName.isvalid) {
        alert("Invalid Format! Ex:(00.00)");
    }
    else {

    }
}

function ValidationValChargesPP() {
    if (typeof (Page_Validators) == "undefined") return;
    var RequiredName = document.getElementById('<%= RegularExpressionValidator3.ClientID%>');
    ValidatorValidate(RequiredName);
    if (!RequiredName.isvalid) {
        alert("Invalid Format! Ex:(00.00)");
    }
    else {

    }
}
function ValidationValChargesCC() {
    if (typeof (Page_Validators) == "undefined") return;
    var RequiredName = document.getElementById('<%= RegularExpressionValidator4.ClientID%>');
    ValidatorValidate(RequiredName);
    if (!RequiredName.isvalid) {
        alert("Invalid Format! Ex:(00.00)");
    }
    else {

    }
}

function ValidationCommission() {
    if (typeof (Page_Validators) == "undefined") return;
    var RequiredName = document.getElementById('<%= RegularExpressionValidator5.ClientID%>');
    ValidatorValidate(RequiredName);
    if (!RequiredName.isvalid) {
        alert("Invalid Format! Ex:(00.00)");
    }
    else {

    }
}


function ValidationIncentive() {
    if (typeof (Page_Validators) == "undefined") return;
    var RequiredName = document.getElementById('<%= RegularExpressionValidator6.ClientID%>');
    ValidatorValidate(RequiredName);
    if (!RequiredName.isvalid) {
        alert("Invalid Format! Ex:(00.00)");
    }
    else {

    }
}

function ValidationOCDAPP() {
    if (typeof (Page_Validators) == "undefined") return;
    var RequiredName = document.getElementById('<%= RegularExpressionValidator7.ClientID%>');
    ValidatorValidate(RequiredName);
    if (!RequiredName.isvalid) {
        alert("Invalid Format! Ex:(00.00)");
    }
    else {

    }
}


function ValidationOCDACC() {
    if (typeof (Page_Validators) == "undefined") return;
    var RequiredName = document.getElementById('<%= RegularExpressionValidator8.ClientID%>');
    ValidatorValidate(RequiredName);
    if (!RequiredName.isvalid) {
        alert("Invalid Format! Ex:(00.00)");
    }
    else {

    }
}


function ValidationOCDCPP() {
    if (typeof (Page_Validators) == "undefined") return;
    var RequiredName = document.getElementById('<%= RegularExpressionValidator9.ClientID%>');
    ValidatorValidate(RequiredName);
    if (!RequiredName.isvalid) {
        alert("Invalid Format! Ex:(00.00)");
    }
    else {

    }
}


function ValidationOCDCCC() {
    if (typeof (Page_Validators) == "undefined") return;
    var RequiredName = document.getElementById('<%= RegularExpressionValidator10.ClientID%>');
    ValidatorValidate(RequiredName);
    if (!RequiredName.isvalid) {
        alert("Invalid Format! Ex:(00.00)");
    }
    else {

    }
}



</script>
    
  <%--<asp:TextBox ID="txtRevisedWeight" Enabled="false" runat="server" CssClass="style5" OnTextChanged="txtRevisedWeight_TextChanged"  AutoPostBack="true" 
                 ></asp:TextBox>--%>  
    <style type="text/css">
        .style4
        {
            text-align: center;
            font-weight: bold;
        }
        .style5
        {
            font-weight: bold;
            height: 19px;
        }
        .style6
        {
            height: 17px;
        }
        .style7
        {
            height: 24px;
        }
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ScriptMgr" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
                 
        <h1> 
            New Charges Correction Advise (CCA)                  
        </h1>
        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Blue" CssClass="style5"></asp:Label>
         <div class="botline">
         
        <table width="80%" cellpadding="5">
         <tr>
         <td>AWB No. *</td>
         <td> 
            <asp:TextBox ID="txtAWBPrefix" runat="server" Width="40px"></asp:TextBox>
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" 
                runat="server" TargetControlID="txtAWBPrefix" WatermarkText="Prefix">
            </asp:TextBoxWatermarkExtender> 
            &nbsp;
            <asp:TextBox ID="txtAWBNo" runat="server" Width="100px"></asp:TextBox>
        </td>
         <%--<td>Flight # *</td>--%>
         <td colspan="3">
            <asp:Button ID="btnList" runat="server" CssClass="button" 
                 onclick="btnList_Click" Text="List" CausesValidation="false" />
             <asp:Button ID="btnClear" runat="server" CssClass="button" 
                 Text="Clear" CausesValidation="false" onclick="btnClear_Click1" />
             <asp:TextBox ID="txtFlightPrefix" runat="server" Width="40px" MaxLength="3" Visible="false"></asp:TextBox>
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" 
                runat="server" TargetControlID="txtFlightPrefix" WatermarkText="Prefix">
            </asp:TextBoxWatermarkExtender> 
            &nbsp;
             <asp:TextBox ID="txtFlightNo" runat="server" Width="60px" MaxLength="4" Visible="false"></asp:TextBox>
            &nbsp;
             <asp:TextBox ID="txtFlightDate" runat="server" Width="100px" Visible="false"></asp:TextBox>
                  <asp:ImageButton ID="btnFlightDate" runat="server" ImageAlign="AbsMiddle" 
                             ImageUrl="~/Images/calendar_2.png" Visible="false" />
                 <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" runat="server" 
                 TargetControlID="txtFlightDate" Format="dd/MM/yyyy" PopupButtonID="btnFlightDate" PopupPosition="BottomLeft"></asp:CalendarExtender>
            &nbsp;
             &nbsp;
             </td>
         </tr>
         <tr>
         <td>CCA Number</td>
         <td> 
             <asp:TextBox ID="txtNumber" runat="server" Enabled="false" CssClass="style5" 
                 ></asp:TextBox>
             
             </td>
         <td>Status</td>
         <td>
             <asp:TextBox ID="txtStatus" runat="server" Enabled="false" CssClass="style5" ></asp:TextBox>
             </td>
         <td>Date Of AWB Issue</td>
         <td>
             <asp:TextBox ID="txtDateOfIssue" runat="server" Enabled="false" 
                 CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <td>Invoice No</td>
         <td>
             <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="style5" Enabled="false"></asp:TextBox>
             </td>
         <td>Invoice Date</td>
         <td>
             <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="style5" 
                 Enabled="false"></asp:TextBox>
             </td>
         <td>&nbsp;</td>
         <td>
             &nbsp;</td>
         </tr>
         </table>
         <br />
         
         <table width="100%">
         <tr>
         <td>Agent&#39;s Code</td>
         <td>
             <asp:TextBox ID="txtAgentCode" runat="server" Enabled="false" CssClass="style5"></asp:TextBox>
             </td>
         <td>Airline&#39;s Code</td>
         <td>
             <asp:TextBox ID="txtAirlineCode" runat="server" Enabled="false" 
                 CssClass="style5"></asp:TextBox>
             </td>
         <td>Origin </td>
         <td>
             <asp:TextBox ID="txtOrigin" runat="server" Enabled="false" CssClass="style5"></asp:TextBox>
             </td>
         <td>Destination </td>
         <td>
             <asp:TextBox ID="txtDestination" runat="server" Enabled="false" 
                 CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         </table> </div>
         <div class="divback">
         <table width="100%">
         <tr>
         <td colspan="3"><b>Air Waybill weight and / or charges have been corrected / added as 
             follows: </b> </td>
         </tr>
         <tr>
         <td colspan="3" class="style6">&nbsp;</td>
         </tr>
         <tr>
         <td>Weight Unit</td>
         <td>Revised/ Corrected Weight</td>
         <td>Original/Incorrect Weight</td>
         </tr>
         <tr>
         <td>
             <b>
             <asp:RadioButton ID="rdbKilo" runat="server" Text="Kilo" Enabled="False" />
             <asp:RadioButton ID="rdbPound" runat="server" Text="Pound" Enabled="False" />
             </b>
             </td>
         <td>
            <%--<asp:TextBox ID="txtRevisedWeight" Enabled="false" runat="server" CssClass="style5" OnTextChanged="txtRevisedWeight_TextChanged"  AutoPostBack="true" 
                 ></asp:TextBox>--%>
               <asp:TextBox ID="txtRevisedWeight" Enabled="false" runat="server" CssClass="style5"   
                 ></asp:TextBox>
             <asp:RequiredFieldValidator SetFocusOnError="true"  ValidationGroup="CCA" ID="RequiredFieldValidator12" runat="server" ErrorMessage="*" ControlToValidate="txtRevisedWeight"></asp:RequiredFieldValidator>
             </td>
         <td>
             <asp:TextBox ID="txtOriginalWeight" runat="server" Enabled="false" 
                 CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         </table></div>
         <div style="margin-top:10px;" > 
         <table width="100%" cellspacing="3">
         <tr>
         <td style="font-weight:bold; font-size:18px;">Charges</td>
         <td ></td>
         <td style="font-weight:bold; font-size:18px;" colspan="2" >Revised/Correct</td>
         <td></td>
         <td></td>
         <td  style="font-weight:bold; font-size:18px;" colspan="3">Original/Incorrect</td>
         </tr>
         <tr>
         <td></td>
             <td>
                 &nbsp;</td>
             <td>
                 <b>Prepaid</b></td>
             <td>
                 <b>Collect</b></td>
         <td class="style4" >&nbsp;</td>
         <td class="style4" >&nbsp;</td>
         <td colspan="2"><b>Prepaid</b></td>
         <td><b>Collect</b></td>
         </tr>
         <tr>
         <td>Currency</td>
             <td>
                 &nbsp;</td>
             <td colspan="7">
                 <asp:TextBox ID="txtCurrency" runat="server" Enabled="false" CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <td>Comm. Code</td>
             <td>
                 &nbsp;</td>
             <td colspan="3">
                 <asp:TextBox ID="txtCommodityCodeRev" runat="server" Enabled="false" 
                     CssClass="style5"></asp:TextBox>
             </td>
             <td>
                 &nbsp;</td>
             <td>
                 <asp:TextBox ID="txtCommodityCodeOrg" runat="server" Enabled="false" 
                     CssClass="style5"></asp:TextBox>
             </td>
             <td colspan="2">
                 &nbsp;</td>
         </tr>
         <tr>
         <td>Weight Charges</td>
             <td>
                 &nbsp;</td>
             <td>
             <asp:TextBox ID="txtRevWeightChrgsPP"  Enabled="false" runat="server" CssClass="style5"></asp:TextBox>
                 <%--<asp:TextBox ID="txtRevWeightChrgsPP" onchange="javascript:ValidationWeightChrgsPP();ReCalculate();" Enabled="false" runat="server" CssClass="style5"></asp:TextBox>--%>
                 
                 <asp:RegularExpressionValidator SetFocusOnError="true" ID="RegularExpressionValidator1" ValidationExpression="^(?:[1-9]\d*|0)?(?:\.\d+)?$" ControlToValidate="txtRevWeightChrgsPP" runat="server"   Display="Dynamic"></asp:RegularExpressionValidator>
             </td>
             <td>
             <asp:TextBox ID="txtRevWeightChrgsCC" Enabled="false"  runat="server" CssClass="style5"></asp:TextBox>
                 <%--<asp:TextBox ID="txtRevWeightChrgsCC" Enabled="false" onchange="javascript:ValidationWeightChrgsCC();ReCalculate();" runat="server" CssClass="style5"></asp:TextBox>--%>
                 
                 
                                  <asp:RegularExpressionValidator ID="RegularExpressionValidator2"  ValidationExpression="^(?:[1-9]\d*|0)?(?:\.\d+)?$" ControlToValidate="txtRevWeightChrgsCC" runat="server"  Display="Dynamic"></asp:RegularExpressionValidator>

             </td>
         <td >
             &nbsp;</td>
         <td >
             &nbsp;</td>
         <td colspan="2" >
             <asp:TextBox ID="txtOrgWgtChrgsPP" runat="server" 
                  Enabled="false" CssClass="style5"></asp:TextBox>
             </td>
         <td >
             <asp:TextBox ID="txtOrgWgtChrgsCC" runat="server" Enabled="false" 
                 CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <td>Commission</td>
             <td>
                 &nbsp;</td>
             <td colspan="2">
                <%-- <asp:TextBox ID="txtRevCommission" Enabled="false" runat="server" 
                     CssClass="style5" onchange="javascript:ValidationCommission();ReCalculate();"></asp:TextBox>--%>
                      <asp:TextBox ID="txtRevCommission" Enabled="false" runat="server" 
                     CssClass="style5" ></asp:TextBox>
                 <asp:RegularExpressionValidator SetFocusOnError="true" ValidationExpression="^(?:[1-9]\d*|0)?(?:\.\d+)?$" ID="RegularExpressionValidator5" runat="server"  ValidationGroup="CCA" ControlToValidate="txtRevCommission"></asp:RegularExpressionValidator>
             </td>
         <td >
                 </td>
         <td >
                 </td>
         <td colspan="3" >
                 <asp:TextBox ID="txtOrgCommission" runat="server" Enabled="false" 
                     CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <td>Incentive</td>
             <td>
                 &nbsp;</td>
             <td colspan="2">
             <asp:TextBox ID="txtRevisedIncentive"  Enabled="false" runat="server" CssClass="style5"></asp:TextBox>
                 <%--<asp:TextBox ID="txtRevisedIncentive" onchange="javascript:ValidationIncentive();ReCalculate();" Enabled="false" runat="server" CssClass="style5"></asp:TextBox>--%>
                 <asp:RegularExpressionValidator SetFocusOnError="true" ValidationExpression="^(?:[1-9]\d*|0)?(?:\.\d+)?$" ID="RegularExpressionValidator6" runat="server"  ValidationGroup="CCA" ControlToValidate="txtRevisedIncentive"></asp:RegularExpressionValidator>

             </td>
         <td >
             &nbsp;</td>
         <td >
             &nbsp;</td>
         <td colspan="3" >
             <asp:TextBox ID="txtOrgIncentive" runat="server" Enabled="false" 
                 CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <td class="style7">Net/Net Amount</td>
             <td class="style7">
                 </td>
             <td colspan="2" class="style7">
                 <asp:TextBox ID="txtRevNetAmt" Enabled="false" runat="server" CssClass="style5"></asp:TextBox>
             </td>
         <td class="style7" >
             </td>
         <td class="style7" >
             </td>
         <td colspan="3" class="style7" >
             <asp:TextBox ID="txtOrgNetAmt" runat="server" Enabled="false" CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         
         <tr>
         <td>TDSCommission</td>
             <td>
                 &nbsp;</td>
             <td colspan="2">
                 <asp:TextBox ID="txtRevTDSComm" Enabled="false" runat="server" 
                     CssClass="style5"></asp:TextBox>
             </td>
         <td >
             &nbsp;</td>
         <td >
             &nbsp;</td>
         <td colspan="3" >
                 <asp:TextBox ID="txtOrgTDSComm" Enabled="false" runat="server" 
                 CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <td>STCommission</td>
             <td>
                 &nbsp;</td>
             <td colspan="2">
                 <asp:TextBox ID="txtRevSTComm" Enabled="false" runat="server" CssClass="style5"></asp:TextBox>
             </td>
         <td >
             &nbsp;</td>
         <td >
             &nbsp;</td>
         <td colspan="3" >
                 <asp:TextBox ID="txtOrgSTComm" Enabled="false" runat="server" 
                 CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <td>Total Other Charges Due Agent</td>
             <td>
                 &nbsp;</td>
             <td>
                <%-- <asp:TextBox ID="txtRevOCDAPP" onchange="javascript:ValidationOCDAPP();ReCalculate();" Enabled="false" runat="server" CssClass="style5" 
                     ></asp:TextBox>--%>
                      <asp:TextBox ID="txtRevOCDAPP"  Enabled="false" runat="server" 
                     CssClass="style5" Height="23px" 
                     ></asp:TextBox>
                 <asp:RequiredFieldValidator ValidationGroup="CCA" ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" ControlToValidate="txtRevOCDAPP"></asp:RequiredFieldValidator> 
                 <asp:RegularExpressionValidator SetFocusOnError="true" ValidationExpression="^(?:[1-9]\d*|0)?(?:\.\d+)?$" ID="RegularExpressionValidator7" runat="server"  ControlToValidate="txtRevOCDAPP"></asp:RegularExpressionValidator>
             </td>
             <td>
                <%-- <asp:TextBox ID="txtRevOCDACC" onchange="javascript:ValidationOCDACC();ReCalculate();" runat="server" Enabled="false" CssClass="style5"></asp:TextBox>--%>
                 <asp:TextBox ID="txtRevOCDACC"  runat="server" Enabled="false" CssClass="style5"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="CCA" ID="RequiredFieldValidator9" runat="server" ErrorMessage="*" ControlToValidate="txtRevOCDACC"></asp:RequiredFieldValidator> 
                 <asp:RegularExpressionValidator SetFocusOnError="true" ValidationExpression="^(?:[1-9]\d*|0)?(?:\.\d+)?$" ID="RegularExpressionValidator8" runat="server"  ControlToValidate="txtRevOCDACC"></asp:RegularExpressionValidator>

             </td>
         <td >
                 &nbsp;</td>
         <td >
                 &nbsp;</td>
         <td colspan="2" >
                 <asp:TextBox ID="txtOrgOCDAPP" runat="server" Enabled="false" CssClass="style5"></asp:TextBox>
             </td>
         <td >
                 <asp:TextBox ID="txtOrgOCDACC" runat="server" Enabled="false" CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <td>Total Other Charges Due Airline</td>
             <td>
                 &nbsp;</td>
             <td>
                 <%--<asp:TextBox ID="txtRevOCDCPP" onchange="javascript:ValidationOCDCPP();ReCalculate();" Enabled="false" runat="server" CssClass="style5" ></asp:TextBox>--%>
                 <asp:TextBox ID="txtRevOCDCPP"  Enabled="false" runat="server" CssClass="style5" ></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="CCA" ID="RequiredFieldValidator10" runat="server" ErrorMessage="*" ControlToValidate="txtRevOCDCPP"></asp:RequiredFieldValidator> 
                 <asp:RegularExpressionValidator SetFocusOnError="true" ValidationExpression="^(?:[1-9]\d*|0)?(?:\.\d+)?$" ID="RegularExpressionValidator9" runat="server"  ControlToValidate="txtRevOCDCPP"></asp:RegularExpressionValidator>
             </td>
             <td>
                 <%--<asp:TextBox ID="txtRevOCDCCC" onchange="javascript:ValidationOCDCCC();ReCalculate();" Enabled="false" runat="server" CssClass="style5"></asp:TextBox>--%>
                 <asp:TextBox ID="txtRevOCDCCC"  Enabled="false" runat="server" CssClass="style5"></asp:TextBox>
                 <asp:RequiredFieldValidator ValidationGroup="CCA" ID="RequiredFieldValidator11" runat="server" ErrorMessage="*" ControlToValidate="txtRevOCDCCC"></asp:RequiredFieldValidator> 
                 <asp:RegularExpressionValidator SetFocusOnError="true" ValidationExpression="^(?:[1-9]\d*|0)?(?:\.\d+)?$" ID="RegularExpressionValidator10" runat="server"  ControlToValidate="txtRevOCDCCC"></asp:RegularExpressionValidator>

             </td>
         <td >
                 &nbsp;</td>
         <td >
                 &nbsp;</td>
         <td colspan="2" >
                 <asp:TextBox ID="txtOrgOCDCPP"  runat="server" Enabled="false" CssClass="style5"></asp:TextBox>
             </td>
         <td >
                 <asp:TextBox ID="txtOrgOCDCCC" runat="server" Enabled="false" CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <td>Total Payable to Airline (Ex:VAT)</td>
             <td>
                 &nbsp;</td>
             <td colspan="2">
                 <asp:TextBox ID="txtRevPayableAirline" Enabled="false" runat="server" CssClass="style5"></asp:TextBox>
             </td>
         <td >
                 &nbsp;</td>
         <td >
                 &nbsp;</td>
         <td colspan="3" >
                 <asp:TextBox ID="txtOrgPayableAirline" runat="server" CssClass="style5" 
                     Enabled="false"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <td>Service Tax</td>
             <td>
                 &nbsp;</td>
             <td colspan="2">
                 <asp:TextBox ID="txtRevServiceTax" Enabled="false" runat="server" 
                     CssClass="style5"></asp:TextBox>
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator11" 
                     runat="server" ControlToValidate="txtRevServiceTax" SetFocusOnError="true" 
                     ValidationExpression="^(?:[1-9]\d*|0)?(?:\.\d+)?$" ValidationGroup="CCA"></asp:RegularExpressionValidator>
             </td>
         <td >
             &nbsp;</td>
         <td >
             &nbsp;</td>
         <td colspan="3" >
                 <asp:TextBox ID="txtOrgServiceTax" Enabled="false" runat="server" 
                 CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <td>Total</td>
             <td>
                 &nbsp;</td>
             <td colspan="2">
                 <asp:TextBox ID="txtRevTotal" Enabled="false" runat="server" CssClass="style5"></asp:TextBox>
             </td>
         <td >
                 &nbsp;</td>
         <td >
                 &nbsp;</td>
         <td colspan="3" >
                 <asp:TextBox ID="txtOrgTotal" Enabled="false" runat="server" CssClass="style5"></asp:TextBox>
             </td>
         </tr>
         <tr>
         <%--<td>VAT/TAX</td>--%>
             <td>
                 &nbsp;</td>
             <td colspan="2">
                 <b>
                 <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />
                 </b>
             </td>
         <td >
             &nbsp;</td>
         <td >
             &nbsp;</td>
         <td colspan="3" >
             <b>
             <asp:CheckBox ID="CheckBox2" Visible="false" runat="server" />
                 </b>
                 <asp:TextBox ID="txtOrgVAT" runat="server" Visible="false" CssClass="style5" 
                 Enabled="false"></asp:TextBox>
             </td>
         </tr></table>
         <br />
         <table width="100%">
         <tr>
         <td valign="top"><h3>Reason for Correction</h3>
             <asp:TextBox ID="txtRemarksCorrection" runat="server" Height="95px" TextMode="MultiLine" 
                     Width="470px" CssClass="style5" ></asp:TextBox> </td>
             <td valign="top">
             <%--<h3>Reason for Rejection</h3>--%>
                 <asp:TextBox ID="txtRemarksRejection" runat="server" Height="95px" TextMode="MultiLine" 
                     Width="470px" CssClass="style5"  Visible="false"></asp:TextBox>
             </td>
         <tr style="visibility:hidden;">
         <td>Valuation Charges</td>
             <td>
                 &nbsp;</td>
             <td>
                 <%--<asp:TextBox ID="txtRevValChargesPP" onchange="javascript:ValidationValChargesPP();ReCalculate();" Enabled="false" runat="server" CssClass="style5"></asp:TextBox>--%>
                 <asp:TextBox ID="txtRevValChargesPP"  Enabled="false" runat="server" CssClass="style5"></asp:TextBox>
                 
                 <asp:RegularExpressionValidator SetFocusOnError="true" ValidationExpression="^(?:[1-9]\d*|0)?(?:\.\d+)?$" ID="RegularExpressionValidator3" runat="server"  Display="Dynamic" ControlToValidate="txtRevValChargesPP"></asp:RegularExpressionValidator>
             </td>
             
             <td>
                 <%--<asp:TextBox ID="txtRevValChargesCC" onchange="javascript:ValidationValChargesCC();ReCalculate();" Enabled="false" runat="server" CssClass="style5"></asp:TextBox>--%>
                 <asp:TextBox ID="txtRevValChargesCC"  Enabled="false" runat="server" CssClass="style5"></asp:TextBox>
                 <asp:RegularExpressionValidator SetFocusOnError="true" ValidationExpression="^(?:[1-9]\d*|0)?(?:\.\d+)?$" ID="RegularExpressionValidator4" runat="server"  Display="Dynamic" ControlToValidate="txtRevValChargesCC"></asp:RegularExpressionValidator>

             </td>
         <td >
             &nbsp;</td>
         <td >
             &nbsp;</td>
         <td colspan="2" >
             <asp:TextBox ID="txtOrgValChargesPP" runat="server" Enabled="false" 
                 CssClass="style5"></asp:TextBox>
             </td>
         <td >
             <asp:TextBox ID="txtOrgValChargesCC" runat="server" Enabled="false" 
                 CssClass="style5"></asp:TextBox>
             </td>
         </tr>
             
         </tr>
         
         </table>
         </div>
         
                  <b>
         
        <%-- <div class="botline"> 
         
               <table width="100%" cellpadding="5px">
                 <tr>
                     <td>
                         <asp:Label ID="lblAwbInv" runat="server" Text="AWB"></asp:Label>
                     </td>
                     <td>
                       <asp:TextBox ID="txtPreAWB" runat="server"  
                              Width="30px"></asp:TextBox>
                         <asp:TextBox Width="100px" ID="txtAWB" runat="server" ontextchanged="txtAWB_TextChanged"  
                            ></asp:TextBox>
                     </td>
                     <td>
                         Origin:</td>
                     <td>
                         <asp:TextBox Width="100px" ID="txtOrigin" runat="server" Enabled="false"></asp:TextBox>
                     </td>
                     <td>
                         Destination:</td>
                     <td>
                         <asp:TextBox Width="100px" ID="txtDestination" runat="server" Enabled="false"></asp:TextBox>
                     </td>
                     <td>
                         Date Of Issue:</td>
                     <td>
                         <asp:TextBox Width="100px" ID="txtDate" runat="server" Enabled="false"></asp:TextBox>
                     </td>
                     <td>
                         <asp:Label ID="lblInvoice" runat="server" Text="Invoice No."></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="100px" ID="txtInvoiceNo" runat="server" Enabled="false"></asp:TextBox>
                     </td>
                     <td>
                        
                        <asp:Button ID="btnList" runat="server" CssClass="button" 
                             onclick="btnList_Click" Text="List" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" 
                             onclick="btnClear_Click" />
                        
                     </td>
                 </tr>
             </table>
         
         </div> 
         <table width="100%" >
         <tr>
         <td> <fieldset>  <legend style=" font-weight:bold; color:#999999; padding:5px;" xml:lang="">
             Original / Incorrect </legend>
 <table width="100%" class="divback" cellpadding="5px">
                     <tr>
                         <td>
                <asp:Label ID="lblCGrossWt" runat="server" Text="Gross Wt"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCGrossWt" runat="server" Enabled="false" TabIndex="3" 
                                 Enabled="False" ></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCChargableWt" runat="server" Text="Chargable Wt"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCCharWt" runat="server" Enabled="false" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCFre" runat="server" Text="Freight"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCFreight" runat="server" Enabled="false" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCOCDC" runat="server" Text="OCDC"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCOCDC" runat="server" Enabled="false" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCOCDA" runat="server" Text="OCDA"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCOCDA" runat="server" Enabled="false" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             ServiceTax</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCServiceTax" runat="server" Enabled="false" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             Commission</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCCommission" runat="server" Enabled="false" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             TDS On Commission</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCTDSOnComm" runat="server" Enabled="false" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                     <td>
                             ST On Commission</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtSTOnComm" runat="server" Enabled="false" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCTot" runat="server" Text="Total"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCTot" runat="server" Enabled="false" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr> <td>
                             &nbsp;</td>
                         <td>
                             &nbsp;</td></tr>
                     <tr>
                         <td>
                             &nbsp;</td>
                         <td>
                             &nbsp;</td>
                     </tr>
                 </table>
     </fieldset></td>
         <td><fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Revised / Correct</legend> 
 <table width="100%" class="divback" cellpadding="5px">
                     <tr>
                         <td>
                <asp:Label ID="Label1" runat="server" Text="Gross Wt"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedGrosswt" runat="server" 
                                 TabIndex="3" ></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="Label2" runat="server" Text="Chargable Wt"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedChargableWt" runat="server" 
                                 TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="Label3" runat="server" Text="Freight"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedFreight" runat="server" 
                                 TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="Label4" runat="server" Text="OCDC"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevsedOCDC" runat="server" TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="Label5" runat="server" Text="OCDA"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedOCDA" runat="server" TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             ServiceTax</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedServiceTax" runat="server" 
                                 TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             Commission</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedComm" runat="server" TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             TDS On Commission</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedTDSOnComm" runat="server" 
                                 TabIndex="3" Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                     <td>
                             ST On Commission</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedSTOnComm" runat="server" 
                                 TabIndex="3" Enabled="False" ></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="Label6" runat="server" Text="Total"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedTotal" runat="server" TabIndex="3" Enabled="False">0</asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             Remarks</td>
                        <td>
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                     </tr>
                 </table>
  </fieldset></td>
         </tr>
        
         </table>  --%></b> 
        
    <div id="fotbut">
    
        <%--<asp:Button ID="btnSave" Text="Save" runat="server"  
        CssClass="button" onclick="btnSave_Click"   />
        <b>&nbsp;    
       </b>    
       <asp:Button ID="btnGenerateDCM" Text="Generate CCA" runat="server"  
        CssClass="button" onclick="btnGenerateCCA_Click"    />
        <b>&nbsp;    
       </b>    
       <asp:Button ID="btnTaxCalc" Text="Tax Calc" runat="server" Visible="false" 
            CssClass="button" onclick="btnTaxCalc_Click"/>
            <b>&nbsp;
            </b>
            <asp:Button ID="btnCommChange" Text="Comm change" runat="server" Visible="false" 
            CssClass="button" onclick="btnCommChange_Click"/>--%>
        <asp:Button ID="btnSave" runat="server" CssClass="button" 
            onclick="btnSave_Click" Text="Save" Visible="False" ValidationGroup="CCA" />
        <asp:Button ID="btnCalculate" runat="server" CssClass="button" 
            onclick="btnCalculate_Click" Text="Calculate" ValidationGroup="CCA" />
        <asp:Button ID="btnAccept" runat="server" CssClass="button" 
             Text="Accept" Visible="false" onclick="btnAccept_Click" ValidationGroup="CCA" />
        <asp:Button ID="btnReject" runat="server" CssClass="button" 
             Text="Reject" Visible="false" onclick="btnReject_Click" ValidationGroup="CCA" />
        <asp:Button ID="btnAirlineHandled" runat="server" CssClass="button" 
             Text="Airline Handled" Visible="false" />
        <asp:Button ID="btnAmend" runat="server" CssClass="button" Visible="false" 
             Text="Amend" />
        <asp:HiddenField ID="hdSelection" runat="server" />
        </div> 
  </div>                    
</asp:Content>