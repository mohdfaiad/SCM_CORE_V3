<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DCMGenerate.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.DCMGenerate" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
    function show_alert() {
        var msg = "DCM Generated";
        alert(msg);
    }
    function show_alert1() {
        var msg = "DCM Generation failed";
        alert(msg);
    }
    function show_alert2() {
        var msg = "Current & Revised Are Identical";
        alert(msg);
    }
</script>
    
  <%-- <asp:TextBox Width="100px" ID="txtDCMNumber" runat="server"></asp:TextBox>--%>  
    <style type="text/css">
        .auto-style1 {
            height: 34px;
        }
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    dd<asp:ToolkitScriptManager ID="ScriptMgr" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
                 
        <h1> 
           New Debit Credit Memo (DCM)      
        </h1>
         <div class="botline">
           <table width="100%" cellpadding="5px">
                 <tr>
                    <td class="auto-style1">
                        <%-- <asp:TextBox Width="100px" ID="txtDCMNumber" runat="server"></asp:TextBox>--%>
                         <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Blue"></asp:Label>
                     </td>
                  </tr>
                  <tr>
                     <td>
                         <asp:Label ID="lblDCMNumber" runat="server" Text="DCM Number:"></asp:Label>&nbsp;&nbsp;
                         <asp:Label Width="200px" ID="txtDCMNumber" Font-Bold="true" runat="server"></asp:Label>
                         &nbsp;
                         <asp:Label ID="lblDCMAmount" runat="server" Text="DCM Amount:"></asp:Label>&nbsp;&nbsp;
                         <asp:Label Width="100px" ID="txtDCMAmount" Font-Bold="true" runat="server"></asp:Label>&nbsp;&nbsp;
                         <asp:Label Width="100px" ID="txtDCMType" Font-Bold="true" runat="server"></asp:Label>
                     </td>
                 </tr>
             </table>
           
         
         </div>
         <div class="botline">
           <table width="80%" cellpadding="5px">
                  <tr>
                    <td colspan="3">
                        <asp:RadioButton ID="rbDCMAWB" runat="server" Text="DCM against AWB" 
                            GroupName="DCMType" AutoPostBack="True" Checked="True" 
                            oncheckedchanged="rbDCMAWB_CheckedChanged"  />
                         <asp:RadioButton ID="rbDCMDeals" runat="server" Text="DCM against Deals/PLI" 
                             GroupName="DCMType" AutoPostBack="True" 
                             oncheckedchanged="rbDCMDeals_CheckedChanged" />
                     </td>
                  </tr>
                 <tr>
                     <td>
                         <asp:Label ID="lblAwbInv" runat="server" Text="AWB #"></asp:Label>
                     </td>
                     <td>
                       <asp:TextBox ID="txtPreAWB" runat="server"  
                              Width="40px"></asp:TextBox>
                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" 
                            runat="server" TargetControlID="txtPreAWB" WatermarkText="Prefix">
                        </asp:TextBoxWatermarkExtender> 
                        &nbsp;
                         <asp:TextBox Width="90px" ID="txtAWB" runat="server" ></asp:TextBox>
                     </td>
                     <td>
                         <asp:Label ID="lblInvoice" runat="server" Text="Invoice #"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="120px" ID="txtInvoiceNo" runat="server" ></asp:TextBox>
                     </td>
                     <td>
                         <asp:Label ID="lblFlightNo" runat="server" Text="Flight #"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox ID="txtFlightPrefix" runat="server" Width="40px" MaxLength="3"></asp:TextBox>
                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" 
                                runat="server" TargetControlID="txtFlightPrefix" WatermarkText="Prefix">
                            </asp:TextBoxWatermarkExtender> 
                        &nbsp;
                        <asp:TextBox Width="80px" ID="txtFlightNo" runat="server" MaxLength="4"></asp:TextBox>
                        &nbsp;
                         <asp:TextBox Width="80px" ID="txtFlightDate" runat="server"   
                            ></asp:TextBox>
                            <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtFlightDate">
                         </asp:CalendarExtender>
<%--                         <asp:RequiredFieldValidator ValidationGroup="List" ID="txtFlightDate_RequiredFieldValidator" ControlToValidate="txtFlightDate" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
--%>
                     </td>
                 </tr>
                 <tr>
                    <td>
                        <asp:Button ID="btnList" runat="server" CssClass="button" 
                             onclick="btnList_Click" Text="List" ValidationGroup="List"  />
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" 
                             onclick="btnClear_Click"  />
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
                <asp:Label ID="Label7" runat="server" Text="Flight #"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCurrentFltNo" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                        <tr>
                         <td>
                             Flight Date</td>
                         <td>
                         <asp:TextBox Width="100px" ID="txtCurrentFlightDate" runat="server"  
                           Enabled="False" ></asp:TextBox>
                            
                            <asp:CalendarExtender ID="txtCurrentFlightDate_CalendarExtender" Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtCurrentFlightDate">
                         </asp:CalendarExtender>
                         </td>
                     </tr>
                        
                     <tr>
                         <td>
                <asp:Label ID="lblCGrossWt" runat="server" Text="Gross Wt"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCGrossWt" runat="server" ReadOnly="True" TabIndex="3" 
                                 Enabled="False" ></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCChargableWt" runat="server" Text="Chargable Wt"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCCharWt" runat="server" ReadOnly="True" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCFre" runat="server" Text="Freight"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCFreight" runat="server" ReadOnly="True" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCOCDC" runat="server" Text="OCDC"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCOCDC" runat="server" ReadOnly="True" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCOCDA" runat="server" Text="OCDA"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCOCDA" runat="server" ReadOnly="True" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             ServiceTax</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCServiceTax" runat="server" ReadOnly="True" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             Commission</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCCommission" runat="server" ReadOnly="True" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                     <td>
                             ST On Commission</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtSTOnComm" runat="server" ReadOnly="True" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             TDS On Commission</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCTDSOnComm" runat="server" ReadOnly="True" TabIndex="3" 
                                 Enabled="False"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                <asp:Label ID="lblCTot" runat="server" Text="Total"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtCTot" runat="server" ReadOnly="True" TabIndex="3" 
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
         <td><fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Revised</legend> 
 <table width="100%" class="divback" cellpadding="5px">
 <tr>
                         <td>
                <asp:Label ID="Label8" runat="server" Text="Flight #"></asp:Label>
                         </td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedFltNo" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                        <tr>
                         <td>
                             Flight Date</td>
                         <td>
                         <asp:TextBox Width="100px" ID="txtRevisedFlightDate" runat="server"  
                            ></asp:TextBox>
                            <asp:CalendarExtender ID="txtRevisedFlightDate_CalendarExtender" Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtRevisedFlightDate">
                         </asp:CalendarExtender>
                         
                         </td>
                     </tr>
                        
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
                             ST On Commission</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedSTOnComm" runat="server" 
                                 TabIndex="3" ></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             TDS On Commission</td>
                         <td>
                <asp:TextBox Width="100px" ID="txtRevisedTDSOnComm" runat="server" 
                                 TabIndex="3"></asp:TextBox>
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
        
         </table>   
        
    <div id="fotbut">
    
        <asp:Button ID="btnSave" Text="Save" runat="server"  
        CssClass="button" onclick="btnSave_Click" />
       &nbsp;    
       <asp:Button ID="btnGenerateDCM" Text="Generate DCM" runat="server"  
        CssClass="button" onclick="btnGenerateDCM_Click" />
        &nbsp;    
       <asp:Button ID="btnTaxCalc" Text="Tax Calc" runat="server" Visible="false" 
            CssClass="button" onclick="btnTaxCalc_Click"/>
            <asp:Button ID="btnCommChange" Text="Comm change" runat="server" Visible="false" 
            CssClass="button" onclick="btnCommChange_Click"/>
            <asp:Button ID="btnSTChange" Text="ST change" runat="server" Visible="false" 
            CssClass="button" onclick="btnSTChange_Click"/>
            <asp:Button ID="btnSTOnCommChange" Text="ST on Comm change" runat="server" Visible="false" 
            CssClass="button" onclick="btnSTOnCommChange_Click"/>
            <asp:Button ID="btnTDSOnCommChange" Text="TDs on Comm change" 
            runat="server" Visible="false" 
            CssClass="button" onclick="btnTDSOnCommChange_Click"/>
            <asp:Button ID="btnAWBNumberChange" Text="AWBNo change" runat="server" Visible="false" 
            CssClass="button" onclick="btnAWBNumberChange_Click"/>
        </div> 
  </div>                    
</asp:Content>

