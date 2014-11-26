<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="CCAProcessing.aspx.cs" Inherits="ProjectSmartCargoManager.CCAProcessing" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
function show_alert() {
var msg = "CCA Generated Successfully";
alert(msg);
}
function show_alert1() {
    var msg = "CCA Generation Failed  ";
    alert(msg);
}
function show_alert2() {
    var msg = "Current & Revised Fields Are Identical";
    alert(msg);
}
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ScriptMgr" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
                 
                  <h1> 
                      <img alt="" src="Images/txtcca.png" id="cca" /></h1>
         <div class="botline">
           <table width="100%" cellpadding="5px">
                 <tr>
                     <td width="10%">
                         <asp:Label ID="lblCCANumber" runat="server" Text="CCA Number"></asp:Label>
                     </td>
                     <td>
                        <%-- <asp:TextBox Width="100px" ID="txtCCANumber" runat="server"></asp:TextBox>--%>
                        <asp:Label Width="100px" ID="txtCCANumber" runat="server"></asp:Label>
                         <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Blue"></asp:Label>
                     </td>
                     
                 </tr>
             </table>
           
         
         </div>
         <div class="botline"> 
         
               <table width="100%" cellpadding="5px">
                 <tr>
                     <td>
                         <asp:Label ID="lblAwbInv" runat="server" Text="AWB"></asp:Label>
                     </td>
                     <td>
                       <asp:TextBox ID="txtPreAWB" runat="server"  
                              Width="30px"></asp:TextBox>
                         <asp:TextBox Width="100px" ID="txtAWB" runat="server"  
                            ></asp:TextBox>
                     </td>
                     <td>
                         <asp:Label ID="lblInvoice" runat="server" Text="Invoice No."></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="100px" ID="txtInvoiceNo" runat="server" ></asp:TextBox>
                     </td>
                  
                     
                     <td>
                          
                         <asp:Label ID="lblAgent" runat="server" Text="Agent"></asp:Label>
                     
                     </td>
                     
                    
                     <td>
                        
                         <asp:TextBox Width="100px" ID="txtAgent" runat="server" ></asp:TextBox>
                     </td>
                     
                    
                     <td>
                        
                         <asp:Label ID="lblAgentFrom" runat="server" Text="From"></asp:Label>
                     </td>
                     
                    
                     <td>
                        
                         
                         <asp:TextBox Width="100px" ID="txtAgentFrom" runat="server" ></asp:TextBox>
                         <asp:CalendarExtender ID="txtAgentFrom_CalendarExtender" Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtAgentFrom">
                         </asp:CalendarExtender>
                         <asp:RegularExpressionValidator ID="txtAgentFromRegularExpressionValidator" runat="server" 
                             ControlToValidate="txtAgentFrom" ErrorMessage="*" 
                             ValidationExpression="^\d{1,2}\/\d{1,2}\/\d{4}$"></asp:RegularExpressionValidator>
                     </td>
                     
                    <td>
                    <asp:Label ID="lblAgentTo" runat="server" Text="To"></asp:Label>
                    </td>
                     <td>
                        
                         <asp:TextBox Width="100px" ID="txtAgentTo" runat="server" ></asp:TextBox>
                         <asp:CalendarExtender ID="txtAgentTo_CalendarExtender"  Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtAgentTo" >                             
                         </asp:CalendarExtender>
                         <asp:RegularExpressionValidator ID="txtAgentToRegularExpressionValidator" runat="server" ErrorMessage="*"  ValidationExpression="^\d{1,2}\/\d{1,2}\/\d{4}$"
                         ControlToValidate="txtAgentTo"
                         >
                         </asp:RegularExpressionValidator>
                        
                        <asp:Button ID="btnList" runat="server" CssClass="button" 
                             onclick="btnList_Click" Text="List" />
                        
                     </td>
                 </tr>
             </table>
         
         </div> 
         <table width="100%" >
         <tr>
         <td> <fieldset>  <legend style=" font-weight:bold; color:#999999; padding:5px;" xml:lang="">Current </legend>
 <table width="100%" class="divback" cellpadding="5px">
                     <tr>
                         <td>
                <asp:Label ID="lblCGrossWt" runat="server" Text="Gross Wt"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCGrossWt" runat="server" ReadOnly="True" TabIndex="3" ></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCChargableWt" runat="server" Text="Chargable Wt"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCCharWt" runat="server" ReadOnly="True" TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCFre" runat="server" Text="Freight"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCFreight" runat="server" ReadOnly="True" TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCOCDC" runat="server" Text="OCDC"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCOCDC" runat="server" ReadOnly="True" TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCOCDA" runat="server" Text="OCDA"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCOCDA" runat="server" ReadOnly="True" TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             &nbsp;</td>
                         <td>
                             &nbsp;</td>
                     </tr>
                     <tr>
                         <td>
                             &nbsp;</td>
                         <td>
                             &nbsp;</td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCTax" runat="server" Text="Tax"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCTax" runat="server" ReadOnly="True" TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCTot" runat="server" Text="Total"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCTot" runat="server" ReadOnly="True" TabIndex="3"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             &nbsp;</td>
                         <td>
                             &nbsp;</td>
                     </tr>
                 </table></fieldset></td>
         <td><fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Revised</legend>

 
 <table width="100%" class="divback" cellpadding="5px">
                     <tr>
                         <td>
                <asp:Label ID="lblRGrossWt" runat="server" Text="Gross Wt"></asp:Label>
                         </td>
                         <td colspan="3">
                <asp:TextBox Width="100px" ID="txtRGrossWt" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblRChargableWt" runat="server" Text="Chargable Wt"></asp:Label>
                         </td>
                         <td colspan="3">
                <asp:TextBox Width="100px" ID="txtRCharWt" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblRevFreight" runat="server" Text="Freight"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRFreight" runat="server"></asp:TextBox>
                         </td>
                         <td rowspan="2">
                <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label>
                         </td>
                         <td rowspan="2">
                <asp:TextBox Width="100px" ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblROCDC" runat="server" Text="OCDC"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtROCDC" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblROCDA" runat="server" Text="OCDA"></asp:Label>
                         </td>
                         <td colspan="3">
                <asp:TextBox Width="100px" ID="txtROCDA" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblDebit" runat="server" Text="Debit"></asp:Label>
                         </td>
                         <td colspan="3">
                <asp:TextBox Width="100px" ID="txtRDeb" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCredit" runat="server" Text="Credit"></asp:Label>
                         </td>
                         <td colspan="3">
                <asp:TextBox Width="100px" ID="txtRCre" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblRTax" runat="server" Text="Tax"></asp:Label>
                         </td>
                         <td colspan="3">
                <asp:TextBox Width="100px" ID="txtRTax" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblRTotal" runat="server" Text="Total"></asp:Label>
                         </td>
                         <td colspan="3">
                         
                <asp:TextBox Width="100px" ID="txtRTot" runat="server"></asp:TextBox>
                    
                         </td>
                     </tr>
                     <tr>
                         <td>
                             &nbsp;</td>
                         <td colspan="3">
                             &nbsp;</td>
                     </tr>
                 </table>
 


  
  </fieldset></td>
         </tr>
         </table>   
        
    <div id="fotbut">
        <asp:Button ID="btnSave" Text="Generate CCA" runat="server"  
        CssClass="button" onclick="btnSave_Click"   />
       &nbsp;</div>     
  </div>                    
</asp:Content>