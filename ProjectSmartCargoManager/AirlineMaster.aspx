<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="AirlineMaster.aspx.cs" Inherits="ProjectSmartCargoManager.AirlineMaster" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ContentPlaceHolderID="head" ID="Content1" runat="server">
    <%-- <td>
    <asp:ListBox ID="listPrefix" runat="server" Height="50px" Width="55px"></asp:ListBox>
         </td>
     <td>
         <asp:Button ID="btnAdd" runat="server" Text="Add" OnClientClick="javascript:return add();" CssClass="button"/>
<asp:Button ID="btnRemove" runat="server" Text="Remove" OnClientClick="javascript:return removeItem();" CssClass="button"/>
     </td>--%> 
     
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content2" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
<div id="contentarea">
   <h1> 
            Partner Master
         </h1>
        
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
               
     <div class="botline">                         
                <table width="80%" cellspacing="3" cellspacing="3">
 <tr>
 <td>Partner Type *</td>
 <td>
    <asp:DropDownList ID="ddlPartnerType" runat="server">
    </asp:DropDownList>

     
     </td>
 <td colspan="2">
     &nbsp;</td>
 <td>
     &nbsp;</td>

 <td>
     &nbsp;</td>
     <td>
         &nbsp;</td>
 </tr>
 <tr>
 <td>Partner Name *</td>
 <td>
     <asp:TextBox ID="txtAirlineName" runat="server" Width="80px"></asp:TextBox>
               <asp:RequiredFieldValidator ControlToValidate="txtAirlineName" ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

     
     </td>
 <td colspan="2">
     Legal Name *</td>
 <td>
     <asp:TextBox ID="txtLegalName" runat="server" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtLegalName" ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

     </td>

 <td>
     &nbsp;</td>
 </tr>
 <tr>
 <td>Partner Prefix *</td>
 <td>
     <asp:TextBox ID="txtPrefix" runat="server" Width="80px" MaxLength="3"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtPrefix" ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

     </td>

 <td colspan="2">
     Designator Code* </td>
 <td>
     <asp:TextBox ID="txtDesigCode" runat="server" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtDesigCode" ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

     </td>
 </tr>
 <tr>
 <td>Valid From *</td>
 <td>
     <asp:TextBox ID="txtValidFrom" runat="server" Width="80px" ></asp:TextBox>
     <asp:ImageButton ID="imgFrmDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
     
     <asp:CalendarExtender ID="txtValidFrom_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtValidFrom" PopupButtonID="imgFrmDt">  </asp:CalendarExtender>
                   
    <asp:RequiredFieldValidator ControlToValidate="txtValidFrom" ID="RequiredFieldValidator13" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>


     </td>

 <td colspan="2">
     Valid To *</td>
 <td>
     <asp:TextBox ID="txtValidTo" runat="server" Width="80px"></asp:TextBox>
     <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
     
     <asp:CalendarExtender ID="txtValidTo_CalendarExtender" runat="server" 
         Enabled="True" TargetControlID="txtValidTo" PopupButtonID="imgToDt">  </asp:CalendarExtender>
         
  <asp:RequiredFieldValidator ControlToValidate="txtValidTo" ID="RequiredFieldValidator12" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

     </td>
 </tr>
<tr>
<td>Partner Location ID* </td>
<td>
    <asp:TextBox ID="txtAirlineLocId" runat="server" Width="80px"></asp:TextBox>
              <asp:RequiredFieldValidator ControlToValidate="txtAirlineLocId" ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

<asp:RegularExpressionValidator ControlToValidate="txtAirlineLocId" 
        ID="RegularExpressionValidatorAccount0" runat="server" 
        ErrorMessage="Digits Only !" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>

    </td>
<td colspan="2">Partner Accounting Code* </td>
<td>
    <asp:TextBox ID="txtAccountingCode" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
              <asp:RequiredFieldValidator ControlToValidate="txtAccountingCode" ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ControlToValidate="txtAccountingCode" ID="RegularExpressionValidatorAccount" runat="server" ErrorMessage="Digits Only !" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
    </td>
</tr>
<tr>
<td>Company Registration ID *</td>
<td>
    <asp:TextBox ID="txtRegistrationID" runat="server" Width="80px"></asp:TextBox>
                  <asp:RequiredFieldValidator ControlToValidate="txtRegistrationID" ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

</td>
<td>
    <asp:CheckBox ID="chkDigitalSign" runat="server" Text="Digital Signature" />
    
    </td>
<td>
    <asp:CheckBox ID="chkSuspend" runat="server" Text="Suspend" /></td>
<td>
    <asp:CheckBox ID="chkIsScheduled" runat="server" Text="Is Scheduled" />
    </td>
</tr>
<tr>
<td>Company President </td>
<td>
    <asp:TextBox ID="txtPresident" runat="server" Width="80px"></asp:TextBox>
    </td>
<td colspan="2">
    Company CFO </td>
<td>
        <asp:TextBox ID="txtCFO" runat="server" Width="80px"></asp:TextBox>
    </td>
</tr>
<tr>
<td>Currency of Listing *</td>
<td>
    <asp:DropDownList ID="ddlCurrency" runat="server">
    </asp:DropDownList>
</td>
<td colspan="2">
    Currency of Billing *</td>
<td>
    <asp:DropDownList ID="ddlBillingCurrency" runat="server">
    </asp:DropDownList>
    </td>
</tr>

<tr>
<td>Tax/VAT Registration ID *</td>
<td>
    <asp:TextBox ID="txtTaxRegID" runat="server" Width="80px"></asp:TextBox>
                  <asp:RequiredFieldValidator ControlToValidate="txtTaxRegID" ID="RequiredFieldValidator8" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

    </td>
<td colspan="2">
    Additional Tax/VAT Registration ID *</td>
<td>
    <asp:TextBox ID="txtAddTaxID" runat="server" Width="80px"></asp:TextBox>
                  <asp:RequiredFieldValidator ControlToValidate="txtAddTaxID" ID="RequiredFieldValidator9" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

    </td>
</tr>

<tr>
<td>Settlement Method </td>
<td>
    <asp:DropDownList ID="ddlSettlement" runat="server">
    <asp:ListItem>ICH</asp:ListItem>
    <asp:ListItem>ACH</asp:ListItem>
    </asp:DropDownList>
    </td>
<td colspan="2">
    Address *</td>
<td>
    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" ></asp:TextBox>
                  <asp:RequiredFieldValidator ControlToValidate="txtAddress" ID="RequiredFieldValidator10" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

    </td>
</tr>
<tr>
<td>Country * </td>
<td>
    <asp:DropDownList ID="ddlCountry" runat="server">
    </asp:DropDownList>
    </td>
<td colspan="2">
    City *</td>
<td>
    <asp:DropDownList ID="ddlCity" runat="server">
    </asp:DropDownList>
    </td>
</tr>
<tr>
<td>Postal Code *</td>
<td>
    <asp:TextBox ID="txtPostalCode" runat="server" Width="80px"></asp:TextBox>
                      <asp:RequiredFieldValidator ControlToValidate="txtPostalCode" ID="RequiredFieldValidator11" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

    </td>
<td colspan="2">
    Airline Language *</td>
<td>
    <asp:DropDownList ID="ddlLanguage" runat="server">
    </asp:DropDownList>
    </td>
</tr>
<tr>
<td>
    <asp:CheckBox ID="chkPartialAcceptance" runat="server" Text="Accept More/Less Pcs" Checked="true" />
    
    &nbsp;</td>
<td>
    <asp:CheckBox ID="chkOtherCharges" runat="server" 
        Text="Include Other Charges"  />
    
    </td>
<td colspan="2">
    <asp:CheckBox ID="chkCustomsMsg" runat="server" 
        Text="Auto Customs Msg"  />
    
    </td>
    <td colspan="2">
    Tolerance(%)*<asp:TextBox ID="txtTolerance" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
    <asp:RequiredFieldValidator ControlToValidate="txtTolerance" ID="RequiredFieldValidator14" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
          <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$" ErrorMessage="Float Only" ControlToValidate="txtTolerance"></asp:RegularExpressionValidator>


              </td>
</tr>
<tr>
<td>Billing On
</td>
<td>
<asp:DropDownList ID="ddlonbilling" runat="server">
<asp:ListItem Text="Departure" Value="DP"></asp:ListItem>
<asp:ListItem Text="Acceptance" Value="AC"></asp:ListItem>
<asp:ListItem Text="Execute" Value="EX"></asp:ListItem>
</asp:DropDownList>
</td>
<td>
Auto Generate Invoice
</td>
<td>
<asp:CheckBox ID="chkautogeninvoice" runat="server" />
</td>
</tr>

<tr>
<td> Bill Type </td>
<td>
    <asp:DropDownList ID="ddlBillType" runat="server">
                       <asp:ListItem Selected="True">Fortnightly</asp:ListItem>
                       <asp:ListItem>Daily</asp:ListItem>
                       <asp:ListItem>Monthly</asp:ListItem>
   </asp:DropDownList>
 </td>
</tr>


<tr>
<td>SITA ID </td>
<td colspan="4">
    <asp:TextBox ID="txtSITAID" runat="server" Width="382px" TextMode="MultiLine"></asp:TextBox>
                      </td>
</tr>
<tr>
<td>Email ID</td>
<td colspan="4">
    <asp:TextBox ID="txtEmailID" runat="server" Width="382px" TextMode="MultiLine"></asp:TextBox>
                      </td>
</tr>
</table>
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
        onclick="btnSave_Click" /></div> 
</div>
<asp:HiddenField runat="server" ID="prefixhdn" />
</asp:Content>
