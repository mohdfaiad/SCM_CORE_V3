<%@ Page Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true"  CodeBehind="UploadRates.aspx.cs"  Title="UploadRates" Inherits="ProjectSmartCargoManager.UploadRates" %>
  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    
   
    <style type="text/css">
        .style1
        {
            height: 39px;
        }
        .style6
        {
            height: 30px;
        }
    </style>
   
    
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       

     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
      <div id="contentarea">
      
      <table width="100%" >
<tr>
<td colspan="3">
    <asp:Label ID="lblError" runat="server"  CssClass="pageerror" ></asp:Label>
    <h1>Upload Masters</h1></td>

</tr>
<tr>
<td colspan="3">&nbsp;</td>

</tr>

<tr>
<td class="style1">&nbsp;</td>
<td class="style1" colspan="2">
<table width="100%">
   <tr>
<td width="10%">Rates</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="newRatesFileUpload" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnNewRateUpload" runat="server" Text="Upload" onclick="btnNewRateUpload_Click" 
             />
    </td>
   <td width="10%" class="style6">
   <asp:HyperLink ID="Hyperlink12" Text="Download Template" NavigateUrl="~/Templates/NewRatesTemplate.xlsx" runat="server" />
  
   </td>
    
 <td class="style6" >Uploaded<asp:CheckBox ID="chknewRate" runat="server" /></td>
    
    </tr>


 <tr>
<td width="10%">Airline Schedule</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="flieUploadAirlineschedule" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnAirlineSchedule" runat="server" Text="Upload" onclick="btnAirlineSchedule_Click" 
             />
    </td>
   
    <td width="10%" class="style6">
    <asp:HyperLink ID="HyperLink1" Text="Download Template" NavigateUrl="~/Templates/AirlineSchedule.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkAirschedule" runat="server" /></td>
    
    </tr>
  <tr>
<td width="10%">Exchange Rates</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="ExchangeRatesUpload" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnExchangeRatesUpload" runat="server" Text="Upload" onclick="btnExchangeRatesUpload_Click1" 
             />
    </td>
   
    <td width="10%" class="style6">
    <asp:HyperLink ID="HyperLink2" Text="Download Template" NavigateUrl="~/Templates/ExchangeRatesTemplate.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkExcRates" runat="server" /></td>
    
    </tr>
   
   <tr>
<td width="10%">Agents</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="AgentFileUpload" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnAgentUpload" runat="server" Text="Upload" onclick="btnAgentUpload_Click" />
    </td>
   
    <td width="10%" class="style6">
    <asp:HyperLink ID="hlnkAgentTemplate" Text="Download Template" NavigateUrl="~/Templates/AgentMaster_Excel.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkAgents" runat="server" /></td>
    
    </tr>
    
    <tr>
<td width="10%">Shipper / Consignee</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="FileUploadShipCon" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnUploadShipCon" runat="server" Text="Upload" onclick="btnUploadShipCon_Click" 
             />
    </td>
   
    <td width="10%" class="style6">
    <asp:HyperLink ID="HyperLink4" Text="Download Template" NavigateUrl="~/Templates/ShipperConsigneeTemplate.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkshipCon" runat="server" /></td>
    
    </tr>
    
    <tr>
<td width="10%">Other Charges</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="uploadOtherCharges" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="BtnUploadOtherCharges" runat="server" Text="Upload" onclick="BtnUploadOtherCharges_Click" 
             />
    </td>
   
    <td width="10%" class="style6">
    <asp:HyperLink ID="HyperLink6" Text="Download Template" NavigateUrl="~/Templates/OCBookV1.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkOC" runat="server" /></td>
    
    </tr>
    
    <tr>
<td width="10%">Airports</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="AirportFileUpload" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnAirportUpload" runat="server" Text="Upload" onclick="btnAirportUpload_Click" />
    </td>
   
    <td width="10%" class="style6">
    <asp:HyperLink ID="hlnkAirportTemplate" Text="Download Template" NavigateUrl="~/Templates/AirportMaster_Excel.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkAirport" runat="server" /></td>
    
    </tr>
    
    <tr>
<td width="10%">Partners</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="PartnerFileUpload" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnPartnerUpload" runat="server" Text="Upload" onclick="btnPartnerUpload_Click" />
    </td>
   
    <td width="10%" class="style6">
    <asp:HyperLink ID="hlnkPartnerTemplate" Text="Download Template" NavigateUrl="~/Templates/PartnerMaster_Excel.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkPartners" runat="server" /></td>
    
    </tr>
    
    <tr>
<td width="10%">Partner Schedule</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="flieUploadPartnerschedule" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnPartnerSchedule" runat="server" Text="Upload" onclick="btnPartnerSchedule_Click" 
             />
    </td>
   
    <td width="10%" class="style6">
    <asp:HyperLink ID="HyperLink9" Text="Download Template" NavigateUrl="~/Templates/PartnerSchedule.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkPartnersch" runat="server" /></td>
    
    </tr>
    
    <tr>
<td width="10%">GL Accounts</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="GLAccountsUpload" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnGLAccountsUpload" runat="server" Text="Upload" onclick="btnGLAccountsUpload_Click1" 
             />
    </td>
   
    <td width="10%" class="style6">
    <asp:HyperLink ID="HyperLink10" Text="Download Template" NavigateUrl="~/Templates/GLAccountsTemplate.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkGLAccount" runat="server" /></td>
    
    </tr>
    
    <tr>
<td width="10%">Budget</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="BudgetFileUpload" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnUploadBudget" runat="server" Text="Upload" 
            onclick="btnUploadBudget_Click" />
    </td>
   
    <td width="10%" class="style6">
    <asp:HyperLink ID="HyperLink11" Text="Download Template" NavigateUrl="~/Templates/Budget_Template.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkBudget" runat="server" /></td>
    
    </tr>
    <tr style="visibility:hidden">
<td width="15%">Rates</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="RatesExcelUpload" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnUploadRates" runat="server" Text="Upload" onclick="btnUploadRates_Click" 
            />
    </td>
   
    <td width="20%" class="style6">
    <asp:HyperLink ID="hlnRateTemplate" Text="Download Template" NavigateUrl="~/Templates/Rates_Template.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkRates" runat="server" />
    </td>
    
    </tr>
 
    </table>
    </td>

</tr>

</table>
      </div> 
      
</asp:Content>
