<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingAWBFileInvoice.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.BillingAWBFileInvoice" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

     
    </style>
    <script language="javascript" type="text/javascript">
        function pageScroll() {
            window.scrollBy(0, 480); // horizontal and vertical scroll increments
            //scrolldelay = setTimeout('pageScroll()', 100); // scrolls every 100 milliseconds
        }

    </script>
 </asp:Content> 
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
  <div id="contentarea">
  
    <h1>       
            <img src="Images/txt_billing.png" />
 <%--img alt="" src="images/BillingMaster.png" />--%>
 </h1> 
   <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
        </p>
  <div  class="botline"> 
  
  <table width=" 100%">
   <tr>
   
    <td>
     Agent Code 
    </td>
    
    <td>
        <asp:DropDownList ID="ddlAgentCode" runat="server" Width="80px" 
            AutoPostBack="True" onselectedindexchanged="ddlAgentCode_SelectedIndexChanged" >
        </asp:DropDownList>
    </td>
    <td>
     Agent Name
    </td>
    <td>
        <asp:DropDownList ID="ddlAgentName" runat="server" Width="170px" 
            AutoPostBack="True" onselectedindexchanged="ddlAgentName_SelectedIndexChanged" >
        </asp:DropDownList>
    </td>
    <td>
     Billing From *
    </td>
    <td>
        <asp:TextBox ID="txtbillingfrom" runat="server" Width="80px"></asp:TextBox>
        <asp:ImageButton ID="btnBillingFrom" runat="server" ImageUrl="~/Images/calendar_2.png"
           ImageAlign="AbsMiddle" />
        <asp:CalendarExtender ID="CEBillingFrom" Format="dd/MM/yyyy" TargetControlID="txtbillingfrom"
           PopupButtonID="btnBillingFrom" runat="server" PopupPosition="BottomLeft">
        </asp:CalendarExtender>
    </td>
    <td>
     Billing To *
    </td>
    <td>
     <asp:TextBox ID="txtbillingto" runat="server" Width="80px"></asp:TextBox>
     <asp:ImageButton ID="btnBillingTo" runat="server" ImageUrl="~/Images/calendar_2.png"
        ImageAlign="AbsMiddle" />
     <asp:CalendarExtender ID="CEBillingTo" Format="dd/MM/yyyy" TargetControlID="txtbillingto"
        PopupButtonID="btnBillingTo" runat="server" PopupPosition="BottomLeft">
     </asp:CalendarExtender>
    </td>
    <td>
    Flight #
    </td>
    <td>
    <asp:TextBox ID="txtFlightNo" runat="server" Width="70px"></asp:TextBox>
    </td>
   </tr>
   <tr><td colspan="11"></td></tr>
   <tr>
   <td>
    AWB #
    </td>
    <td>
    <asp:TextBox ID="txtAWBNumber" runat="server" Width="90px"></asp:TextBox>
    </td>    
   <td>
     Origin
    </td>
    <td>
        <asp:TextBox ID="txtOrigin" runat="server" Width="90px"></asp:TextBox>
        <asp:AutoCompleteExtender ID="txtOrigin_AutoCompleteExtender" runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
                 TargetControlID="txtOrigin">
               </asp:AutoCompleteExtender>
    </td>
    <td>
     Destination
    </td>
    <td>
        <asp:TextBox ID="txtDest" runat="server" Width="100px"></asp:TextBox>
        <asp:AutoCompleteExtender ID="txtDest_AutoCompleteExtender" runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
                 TargetControlID="txtDest">
               </asp:AutoCompleteExtender>
    </td>
    <td>
     Status
    </td>
    <td>
      <asp:DropDownList ID="ddlStatus" runat="server" Width="110px" >
          <asp:ListItem Text="All" Value=""></asp:ListItem>
          <asp:ListItem Text="New" Value="New"></asp:ListItem>
          <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
          <asp:ListItem Text="Confirmed" Value="Confirmed"></asp:ListItem>
          <asp:ListItem Text="Reopened" Value="Reopened"></asp:ListItem>
          <asp:ListItem Text="Final" Value="Final"></asp:ListItem>
          <asp:ListItem Text="Invoiced" Value="Invoiced"></asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
     Spot Rate
    </td>
    <td>
      <asp:DropDownList ID="ddlSpotRate" runat="server" Width="90px" >
          <asp:ListItem Text="All" Value=""></asp:ListItem>
          <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
          <asp:ListItem Text="No" Value="No"></asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
    Payment Mode
    </td>
    <td>
    <asp:DropDownList ID="ddlPayType" runat="server" Width="60px" >
        <asp:ListItem Text="All" Value=""></asp:ListItem>
        <asp:ListItem Text="PP" Value="PP"></asp:ListItem>
        <asp:ListItem Text="CC" Value="CC"></asp:ListItem>
        <asp:ListItem Text="PC" Value="PC"></asp:ListItem>
        <asp:ListItem Text="CP" Value="CP"></asp:ListItem>
        </asp:DropDownList>
    </td>
    
    <td>
        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Search" 
            onclick="btnSearch_Click" />
    </td>

   </tr>
  </table>  
  
  </div> 
  <br /><br /><br />
  <div>

  &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="ChkSelectAll" Text="Select All" 
          Font-Size="Medium" Font-Bold="true" runat="server"  Visible="false" 
          AutoPostBack="True" oncheckedchanged="ChkSelectAll_CheckedChanged" />
  
          &nbsp;&nbsp;&nbsp;<b> 
              <asp:Label ID="lblAWBCount" Visible= "false" runat="server"></asp:Label></b>
          </div>
<div class="divback" style="overflow-x:auto;width:100%">

<asp:GridView ID="grdBillingInfo" runat="server"  Width="100%"
         AutoGenerateColumns="False"
        onrowcommand="grdBillingInfo_RowCommand" AllowPaging="True" 
        onpageindexchanging="grdBillingInfo_PageIndexChanging" 
        onselectedindexchanged="grdBillingInfo_SelectedIndexChanged" 
        onpageindexchanged="grdBillingInfo_PageIndexChanged" ShowFooter="true" 
        PageSize="20" onrowdatabound="grdBillingInfo_RowDataBound" >
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
                 <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="SrNo" Visible = "false" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblSrNo" runat="server" Width="50px" Text='<%# Eval("SerialNumber") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" Width="50px" />
                </asp:TemplateField>
                
                 <asp:ButtonField HeaderText="AWB No." CommandName="AWBClick" DataTextField="AWBNumber"/>
                 
                 <asp:TemplateField HeaderText="Agent Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalText" runat="server" /><br />
                        <asp:Label ID="lblGTotalText" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Charge Wt." HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblchargewt" runat="server" Text='<%# Eval("ChargedWeight") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalchargewt" runat="server" /><br />
                        <asp:Label ID="lblGTotalchargewt" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
               <asp:TemplateField HeaderText="FreightRate" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblIATA" runat="server"  Text='<%# Eval("FreightRate") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalFreightRate" runat="server" /><br />
                        <asp:Label ID="lblGTotalFreightRate" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="OCDA" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblocda" runat="server"  Text='<%# Eval("OCDueAgent") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalOCDA" runat="server" /><br />
                        <asp:Label ID="lblGTotalOCDA" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="OCDC" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblocdc" runat="server" Text='<%# Eval("OCDueCar") %>'></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalOCDC" runat="server" /><br />
                        <asp:Label ID="lblGTotalOCDC" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Total" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltotal" runat="server" Text='<%# Eval("TotalT") %>'></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalTotal" runat="server" /><br />
                        <asp:Label ID="lblGTotalTotal" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Discount" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblDiscount" runat="server"  Text='<%# Eval("DiscountAmt") %>'></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalDiscount" runat="server" /><br />
                        <asp:Label ID="lblGTotalDiscount" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="TAD" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltotalafterdiscount" runat="server" Text='<%# Eval("TAD") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalTAD" runat="server" /><br />
                        <asp:Label ID="lblGTotalTAD" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="TAD-Comm" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblrevised" runat="server" Text='<%# Eval("RevisedTotal") %>'></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalRevised" runat="server" /><br />
                        <asp:Label ID="lblGTotalRevised" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Tax" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltax" runat="server" Text='<%# Eval("TDSAmt") %>'></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalTax" runat="server" /><br />
                        <asp:Label ID="lblGTotalTax" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Final" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblfinal" runat="server" Text='<%# Eval("Final") %>'></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalFinal" runat="server" /><br />
                        <asp:Label ID="lblGTotalFinal" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblconfirmed" runat="server"  Text='<%# Eval("Confirmed") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="InvoiceNo" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNo" runat="server"  Text='<%# Eval("InvoiceNumber") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
</Columns> 
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle BackColor="#336699" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
</asp:GridView>

</div>  
 
 <div  class="botline"> 
 <asp:Panel ID="pnlUpload" runat="server" Visible="false">
 <table width=" 60%">
   <tr>
    <td>
        Select File to upload
    </td>
    <td>
        <asp:FileUpload ID="FileExcelUpload" runat="server"/>
    </td>
    <td>
        <asp:Button ID="btnUpload" runat="server" CssClass="button" Text="Upload" 
            onclick="btnUpload_Click" />
    </td>
    <td>
    <asp:Button ID="btnInvMatch" runat="server" Text="Match Invoice" 
                            CssClass="button" onclick="btnInvMatch_Click" />
    </td>
    <td>
    <asp:Button ID="btnAllInvMatch" runat="server" Text="Match All Invoices" 
                            CssClass="button" onclick="btnAllInvMatch_Click" />
    </td>
   </tr>
   
  </table>
 </asp:Panel>
  </div> 
 
<div class="divback">
<asp:Panel ID="Panel1" runat="server">
<p>
<asp:Label Text="Billing Details:" ID="lblBillingDetails" runat="server" 
         Font-Bold="True" Font-Size="Medium"></asp:Label>
</p>
<table>
<tr>
<td>
   AWB No
  </td>
 <td>
     <asp:TextBox ID="txtAWBNo" runat="server" Width="70px" Enabled="False" ></asp:TextBox>
 </td>
 
<td>
Origin
</td>
<td>
<asp:TextBox ID="txtOrgPnl" runat="server" Width="70px" Enabled="False" ></asp:TextBox>
</td>
<td>
Destination
</td>
<td>
<asp:TextBox ID="txtDestPnl" runat="server" Width="70px" Enabled="False" ></asp:TextBox>
</td>
<td>
Agent Name
</td>
<td colspan="3">
<asp:TextBox ID="txtAgentName" runat="server" Width="216px" Enabled="False" ></asp:TextBox>
</td>
<td>
</td>
<td>
</td>
</tr>
 <tr>
 <td>
  Gross Wt.
 </td>
 <td>
  <asp:TextBox ID="txtgrosswt" runat="server" Width="60px" ></asp:TextBox>
 </td>
  <td>
  Rate/KG.
 </td>
 <td>
  <asp:TextBox ID="txtRatePerKg" runat="server" Width="60px" ></asp:TextBox>
 </td>
 
 <td>
  Chargable Wt.
 </td>
 <td>
 <asp:TextBox ID="txtchargablewt" runat="server" Width="60px"></asp:TextBox>
 </td>
 <td>
   Rate(IATA/MKT)
  </td>
  <td>
  <asp:TextBox ID="txtrate" runat="server" Width="60px"></asp:TextBox>
  </td>
  <td>
   OCDA
  </td>
  <td>
  <asp:TextBox ID="txtocda" runat="server" Width="60px"></asp:TextBox>
  </td>
  <td>
   OCDC
  </td>
  <td>
  <asp:TextBox ID="txtocdc" runat="server" Width="60px"></asp:TextBox>
  <asp:ImageButton ID="btnOCDC" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" />
  </td>

 </tr>
 <tr>
 
 <td>
  Commodity
 </td>
 <td>
  <asp:TextBox ID="txtcommodity" runat="server" Width="60px" Enabled="False"></asp:TextBox>
 </td>
 <td>
  Dimensions
 </td>
 <td>
  <asp:TextBox ID="txtdimensions" runat="server" Width="60px" Enabled="False"></asp:TextBox>
 </td>
   <td>
   Total Before ST
  </td>
  <td>
  <asp:TextBox ID="txtTotal" runat="server" Width="60px" Enabled="False"></asp:TextBox>
  </td>
  <td>
 Service Tax
 </td>
 <td>
  <asp:TextBox ID="txtServiceTax" runat="server" Width="60px" Enabled="False"></asp:TextBox>
 </td>
  <td>
  Discount %
   </td>
   <td>
  <asp:TextBox ID="txtDiscount" runat="server" Width="60px" ></asp:TextBox>
  </td>
  <td>
   Amount
  </td>
  <td>
  <asp:TextBox ID="txtdiscamount" runat="server" Width="60px" ></asp:TextBox>
  </td>
  
  
 </tr>
 <tr>
  
   <td>
 TAD
 </td>
 <td>
  <asp:TextBox ID="txttotalafterdiscount" runat="server" Width="60px" 
         Enabled="False"></asp:TextBox>  
 </td>
 <td>
 Total AWB Amount
 </td>
 <td>
 <asp:TextBox ID="txtTADST" runat="server" Width="60px" 
         Enabled="False"></asp:TextBox>
 </td>
 <td>
   Commission %
  </td>
 <td>
  <asp:TextBox ID="txtcommission" runat="server" Width="60px"></asp:TextBox> 
 </td> 
 <td>
  Commission Amount
 </td>
 <td>
  <asp:TextBox ID="txtcommissionamt" runat="server" Width="60px"></asp:TextBox> 
 </td>
   <td>
    ST on Comm
 </td>
 <td>
  <asp:TextBox ID="txtSTOnComm" runat="server" Width="60px" Enabled="False"></asp:TextBox>
 </td>
 <td></td>
 <td></td>
 </tr>
 
 <tr>
 
    <td>TDS on Comm %</td>
  <td><asp:TextBox ID="txtTDSCommPer" runat="server" Width="60px"></asp:TextBox></td>
  <td>TDS on Comm Amount</td>
  <td><asp:TextBox ID="txtTDSCommAmt" runat="server" Width="60px"></asp:TextBox></td>  
 <td>Revised (TAD-Comm)</td>
  <td><asp:TextBox ID="txtRevisedTotal" runat="server" Width="60px" Enabled="False"></asp:TextBox> </td>
  <td>
   TDS on Freight %
  </td>
  <td>
  <asp:TextBox ID="txtTDSFrtPer" runat="server" Width="60px"></asp:TextBox>
  </td>
  <td>
   TDS on Freight Amount
  </td>
  <td>
  <asp:TextBox ID="txtTDSFrtAmt" runat="server" Width="60px" Enabled="False"></asp:TextBox>
  </td>
  
  
 <td>Total After TAX</td>
 <td><asp:TextBox ID="txttotalaftertax" runat="server" Width="60px" Enabled="False"></asp:TextBox></td>


 </tr>
</table>  
      </asp:Panel>

 </div>
  <div id="fotbut">
            <table style="width: 930px;">
                <tr>
                    <td>
                    
                        <asp:Button ID="btnConfirm" Visible="false" runat="server" Text="Confirm" 
                            CssClass="button" onclick="btnConfirm_Click"/>&nbsp
                        <asp:Button ID="btnGenerateBill" Visible="false" runat="server" 
                            Text="Finalize" CssClass="button" onclick="btnGenerateBill_Click" />&nbsp
                        <asp:Button ID="btnUndoFinalize" Visible="false" runat="server" 
                            Text="Reopen" CssClass="button" onclick="btnUndoFinalize_Click" />&nbsp
                        <asp:Button ID="btnGenerateInvoice" Visible="false" runat="server" 
                            Text="Generate Invoice" CssClass="button" 
                            onclick="btnGenerateInvoice_Click" />&nbsp
                        <asp:Button ID="btnRouteDetails"  Visible="false" runat="server" 
                            Text="Route Details" CssClass="button" onclick="btnRouteDetails_Click" />&nbsp
                        <asp:Button ID="btnTrackAWB" Visible="false" runat="server" 
                            Text="Track AWB" CssClass="button" />&nbsp
                        <asp:Button ID="btnPrint" Visible="false" runat="server" 
                            Text="Export" CssClass="button" onclick="btnPrint_Click" />
                            
                        <asp:Button ID="Button1" Visible="false" runat="server" Text="Disc%" 
                            CssClass="button" onclick="Button1_Click"/>
                        <asp:Button ID="Button2" Visible="false" runat="server" Text="DiscAmt" 
                            CssClass="button" onclick="Button2_Click"/>
                        <asp:Button ID="Button3" Visible="false" runat="server" Text="Comm%" 
                            CssClass="button" onclick="Button3_Click"/>
                        <asp:Button ID="Button4" Visible="false" runat="server" Text="CommAmt" 
                            CssClass="button" onclick="Button4_Click"/>
                        <asp:Button ID="Button5" Visible="false" runat="server" Text="Tds%" 
                            CssClass="button" onclick="Button5_Click"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="Button6" Visible="false" runat="server" Text="GrossWt" 
                            CssClass="button" onclick="Button6_Click" />
                        <asp:Button ID="Button7" Visible="false" runat="server" Text="ChargeWt" 
                            CssClass="button" onclick="Button7_Click" />
                        <asp:Button ID="Button8" Visible="false" runat="server" Text="FrtRate" 
                            CssClass="button" onclick="Button8_Click" />
                        <asp:Button ID="Button9" Visible="false" runat="server" Text="ocda" 
                            CssClass="button" onclick="Button9_Click" />
                        <asp:Button ID="Button10" Visible="false" runat="server" Text="ocdc" 
                            CssClass="button" onclick="Button10_Click" />
                            
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hfAgentCode" runat="server" Value="" />
                        <asp:HiddenField ID="hfAgentName" runat="server" Value="" />
                        <asp:HiddenField ID="hfFromDate" runat="server" Value="" />
                        <asp:HiddenField ID="hfToDate" runat="server" Value="" />
                        <asp:HiddenField ID="hfFlightNo" runat="server" Value="" />
                        <asp:HiddenField ID="hfAWBNumber" runat="server" Value="" />
                        <asp:HiddenField ID="hfOrigin" runat="server" Value="" />
                        <asp:HiddenField ID="hfDestination" runat="server" Value="" />
                        <asp:HiddenField ID="hfStatus" runat="server" Value="" />
                        <asp:HiddenField ID="hfSPotRate" runat="server" Value="" />
                        <asp:HiddenField ID="hfPaymentMode" runat="server" Value="" />
                    </td>
                </tr>
            </table>
            </div>
            <br />
 </div>
  </asp:Content> 