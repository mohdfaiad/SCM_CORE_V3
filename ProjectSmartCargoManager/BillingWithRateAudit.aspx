<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingWithRateAudit.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.BillingWithRateAudit" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">

     
    </style>
    <script language="javascript" type="text/javascript">

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
        <asp:DropDownList ID="ddlAgentCode" runat="server" Width="90px" 
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
        <asp:TextBox ID="txtbillingfrom" runat="server" Width="90px"></asp:TextBox>
        <asp:ImageButton ID="btnBillingFrom" runat="server" ImageUrl="~/Images/calendar_2.png"
           ImageAlign="AbsMiddle" />
        <asp:CalendarExtender ID="CEBillingFrom" Format="yyyy-MM-dd" TargetControlID="txtbillingfrom"
           PopupButtonID="btnBillingFrom" runat="server" PopupPosition="BottomLeft">
        </asp:CalendarExtender>
    </td>
    <td>
     Billing To *
    </td>
    <td>
     <asp:TextBox ID="txtbillingto" runat="server" Width="90px"></asp:TextBox>
     <asp:ImageButton ID="btnBillingTo" runat="server" ImageUrl="~/Images/calendar_2.png"
        ImageAlign="AbsMiddle" />
     <asp:CalendarExtender ID="CEBillingTo" Format="yyyy-MM-dd" TargetControlID="txtbillingto"
        PopupButtonID="btnBillingTo" runat="server" PopupPosition="BottomLeft">
     </asp:CalendarExtender>
    </td>
    <td>
    Flight #
    </td>
    <td>
    <asp:TextBox ID="txtFlightNo" runat="server" Width="90px"></asp:TextBox>
    </td>
    <td>
    </td>
    
   </tr>
   <tr><td colspan="11"></td></tr>
   <tr>
   <td>
     Origin
    </td>
    <td>
        <asp:TextBox ID="txtOrigin" runat="server" Width="90px"></asp:TextBox>
    </td>
    <td>
     Destination
    </td>
    <td>
        <asp:TextBox ID="txtDest" runat="server" Width="90px"></asp:TextBox>
    </td>
    <td>
     Status
    </td>
    <td>
      <asp:DropDownList ID="ddlStatus" runat="server" Width="90px" >
          <asp:ListItem Text="All" Value=""></asp:ListItem>
          <asp:ListItem Text="New" Value="New"></asp:ListItem>
          <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
          <asp:ListItem Text="Billable" Value="Billable"></asp:ListItem>
          <asp:ListItem Text="Confirmed" Value="Confirmed"></asp:ListItem>
          <asp:ListItem Text="Proforma" Value="Proforma"></asp:ListItem>
          <asp:ListItem Text="Final" Value="Final"></asp:ListItem>
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
    <asp:DropDownList ID="ddlPayType" runat="server" Width="90px" >
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
      &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="ChkSelectAll" Text="Select All" Font-Size="Medium" Font-Bold="true" runat="server" 
          oncheckedchanged="ChkSelectAll_CheckedChanged"  Visible="false" 
          AutoPostBack="True" /></div>
<div class="divback">
<asp:GridView ID="grdBillingInfo" runat="server"  Width="100%"
         AutoGenerateColumns="False"
        onrowcommand="grdBillingInfo_RowCommand" AllowPaging="True" 
        onpageindexchanging="grdBillingInfo_PageIndexChanging" 
        onselectedindexchanged="grdBillingInfo_SelectedIndexChanged" >
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
                 <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SrNo" Visible = "false" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblSrNo" runat="server" Width="50px" Text='<%# Eval("SerialNumber") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" Width="50px" />
                </asp:TemplateField>
                
                 <asp:ButtonField HeaderText="AWB No." CommandName = "AWBClick" DataTextField="AWBNumber" ItemStyle-HorizontalAlign="Center"/>
                
                <asp:TemplateField HeaderText="ChargedWt" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblchargewt" runat="server" Width="50px" Text='<%# Eval("ChargedWeight") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="IATA" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblIATA" runat="server" Width="50px"  Text='<%# Eval("FrtIATA") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="MKT/Spot" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                          <asp:Label ID="lblmarket" runat="server" Width="50px"  Text='<%# Eval("FrtMKT") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="OCDA" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblocda" runat="server" Width="50px"  Text='<%# Eval("OCDueAgent") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="OCDC" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblocdc" runat="server" Width="50px"  Text='<%# Eval("OCDueCar") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Total" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltotal" runat="server" Width="50px" Text='<%# Eval("TotalT") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Discount" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblDiscount" runat="server" Width="50px"  Text='<%# Eval("DiscountAmt") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="TAD" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltotalafterdiscount" runat="server" Width="50px"  Text='<%# Eval("TAD") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="TAD-Comm" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblrevised" runat="server" Width="50px" Text='<%# Eval("RevisedTotal") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Tax" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltax" runat="server" Width="50px" Text='<%# Eval("TDSAmt") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Final" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblfinal" runat="server" Width="50px" Text='<%# Eval("Final") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblconfirmed" runat="server" Width="50px"  Text='<%# Eval("Confirmed") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flag" Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblFlag" runat="server" Width="40px"  Text='<%# Eval("Flag") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="InvoiceNo" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNo" runat="server" Width="40px"  Text='<%# Eval("InvoiceNumber") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                
</Columns> 
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
</asp:GridView>
</div>

<div class="divback">
<asp:Panel ID="pnlCommodityGrid" runat="server" Visible="false">
<p>
<asp:Label Text="Commodity Details:" ID="lblCommodity" runat="server" 
         Font-Bold="True" Font-Size="Medium"></asp:Label>
</p>
<asp:GridView ID="grdCommodity" runat="server"  Width="100%" 
        AutoGenerateColumns="False" onrowcommand="grdCommodity_RowCommand" >
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
                 <asp:TemplateField Visible = "false">
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SrNo" Visible = "false" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblSrNo" runat="server" Width="50px" Text='<%# Eval("SerialNumber") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" Width="50px" />
                </asp:TemplateField>
                
                 <asp:ButtonField HeaderText="AWB No." DataTextField="AWBNumber" ItemStyle-HorizontalAlign="Center"/>
                 
                 <asp:TemplateField HeaderText="Code" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                       <asp:Label ID="lblcommcode" runat="server" Width="50px" Text='<%# Eval("CommodityCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="ChargedWt" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblchargewt" runat="server" Width="50px" Text='<%# Eval("ChargedWeight") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="IATA" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblIATA" runat="server" Width="50px"  Text='<%# Eval("FrtIATA") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="MKT/Spot" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                          <asp:Label ID="lblmarket" runat="server" Width="50px"  Text='<%# Eval("FrtMKT") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="OCDA" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblocda" runat="server" Width="50px"  Text='<%# Eval("OCDueAgent") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="OCDC" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblocdc" runat="server" Width="50px"  Text='<%# Eval("OCDueCar") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Total" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltotal" runat="server" Width="50px" Text='<%# Eval("TotalT") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Discount" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblDiscount" runat="server" Width="50px"  Text='<%# Eval("DiscountAmt") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="TAD" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltotalafterdiscount" runat="server" Width="50px"  Text='<%# Eval("TAD") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="TAD-Comm" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblrevised" runat="server" Width="50px" Text='<%# Eval("RevisedTotal") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Tax" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltax" runat="server" Width="50px" Text='<%# Eval("TDSAmt") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Final" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblfinal" runat="server" Width="50px" Text='<%# Eval("Final") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblconfirmed" runat="server" Width="50px"  Text='<%# Eval("Confirmed") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flag" Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblFlag" runat="server" Width="40px"  Text='<%# Eval("Flag") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="InvoiceNo" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNo" runat="server" Width="40px"  Text='<%# Eval("InvoiceNumber") %>'></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Spot Rate" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblSpotRate" Text='<%# Eval("isSpotRate") %>' runat="server" Width="40px"></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                
</Columns> 
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
</asp:GridView>
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
     <asp:TextBox ID="txtAWBNo" runat="server" Width="90px" Enabled="False" ></asp:TextBox>
 </td>
 <td>
  Gross Wt.
 </td>
 <td>
  <asp:TextBox ID="txtgrosswt" runat="server" Width="60px" Enabled="False" ></asp:TextBox>
 </td>
 <td>
  Chargable Wt.
 </td>
 <td>
 <asp:TextBox ID="txtchargablewt" runat="server" Width="60px"></asp:TextBox>
 </td>
 <td>
   IATA Rate
  </td>
  <td>
  <asp:TextBox ID="txtiatarate" runat="server" Width="60px" Enabled="False"></asp:TextBox>
  </td>
  <td>
   MKT Rate
  </td>
  <td>
  <asp:TextBox ID="txtmktrate" runat="server" Width="60px"></asp:TextBox>
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
 <td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td> <td></td>
  <td></td>
 </tr>
 <tr>
  <td>
   Total
  </td>
  <td>
  <asp:TextBox ID="txtTotal" runat="server" Width="60px" Enabled="False"></asp:TextBox>
  </td>
  <td>
  Discount %
   </td>
   <td>
  <asp:TextBox ID="txtDiscount" runat="server" Width="60px"
           ontextchanged="txtDiscount_TextChanged"></asp:TextBox>
  </td>
  <td>
   Amount
  </td>
  <td>
  <asp:TextBox ID="txtdiscamount" runat="server" Width="60px" 
          ontextchanged="txtdiscamount_TextChanged"></asp:TextBox>
  </td>
   <td>
 Total After Discount
 </td>
 <td>
  <asp:TextBox ID="txttotalafterdiscount" runat="server" Width="60px" 
         Enabled="False"></asp:TextBox>  
 </td>
 <td>
   Commission %
  </td>
 <td>
  <asp:TextBox ID="txtcommission" runat="server" Width="60px" Enabled="False"></asp:TextBox> 
 </td> 
 <td>
  Amount
 </td>
 <td>
  <asp:TextBox ID="txtcommissionamt" runat="server" Width="60px" Enabled="False"></asp:TextBox> 
 </td>
  <td>TDS %</td>
  <td><asp:TextBox ID="txtTDSPer" runat="server" Width="60px"></asp:TextBox> </td>
 </tr>
 <tr>
  <td>
   Revised(TAD-Comm)
  </td>
  <td>
  <asp:TextBox ID="txtRevisedTotal" runat="server" Width="60px" Enabled="False"></asp:TextBox>
  </td>
  <td>
   TDS
  </td>
  <td>
  <asp:TextBox ID="txtTDSAmt" runat="server" Width="60px" Enabled="False"></asp:TextBox>
  </td>
  <td>
 Total After TAX
 </td>
 <td>
  <asp:TextBox ID="txttotalaftertax" runat="server" Width="60px" Enabled="False"></asp:TextBox>
 </td>
 <td></td><td></td><td></td><td></td><td></td><td></td> <td></td>
  <td></td>
 </tr>
</table>  
      </asp:Panel>
 </div>
  <div id="fotbut">
            <table style="width: 930px;">
                <tr>
                    <td>
                        <asp:Button ID="btnVerify" Visible="false" runat="server" Text="Verify" 
                            CssClass="button" onclick="btnVerify_Click"/>&nbsp
                        <asp:Button ID="btnApproved" Visible="false" runat="server" Text="Approve" 
                            CssClass="button" onclick="btnApproved_Click" />&nbsp
                        <asp:Button ID="btnConfirm" Visible="false" runat="server" Text="Confirm" 
                            CssClass="button" onclick="btnConfirm_Click"/>
                        <asp:Button ID="btnGenerateProforma" Visible="false" runat="server" 
                            Text="Generate Proforma" CssClass="button" 
                            onclick="btnGenerateProforma_Click" />&nbsp
                        <asp:Button ID="btnConfirmInvoice" Visible="false" runat="server" Text="Confirm" 
                            CssClass="button" onclick="btnConfirmInvoice_Click" />&nbsp
                        <asp:Button ID="btnGenerateInvoice" Visible="false" runat="server" 
                            Text="Generate Invoice" CssClass="button" 
                            onclick="btnGenerateInvoice_Click" />
                        <asp:Button ID="Button1" Visible="false" runat="server" Text="Disc%" CssClass="button" onclick="Button1_Click"/>
                        <asp:Button ID="Button2" Visible="false" runat="server" Text="DiscAmt" CssClass="button" onclick="Button2_Click" />
                        <asp:Button ID="Button3" Visible="false" runat="server" Text="Comm%" CssClass="button" onclick="Button3_Click" />
                        <asp:Button ID="Button4" Visible="false" runat="server" Text="CommAmt" CssClass="button" onclick="Button4_Click" />
                        <asp:Button ID="Button5" Visible="false" runat="server" Text="Tds%" CssClass="button" onclick="Button5_Click" />
                        
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="Button6" Visible="false" runat="server" Text="GrossWt" 
                            CssClass="button" onclick="Button6_Click" />
                        <asp:Button ID="Button7" Visible="false" runat="server" Text="ChargeWt" 
                            CssClass="button" onclick="Button7_Click" />
                        <asp:Button ID="Button8" Visible="false" runat="server" Text="TATARate" 
                            CssClass="button" onclick="Button8_Click" />
                        <asp:Button ID="Button9" Visible="false" runat="server" Text="ocda" 
                            CssClass="button" onclick="Button9_Click" />
                        <asp:Button ID="Button10" Visible="false" runat="server" Text="ocdc" 
                            CssClass="button" onclick="Button10_Click" />
                        <asp:Button ID="Button11" Visible="false" runat="server" Text="MKTRate" 
                            CssClass="button" onclick="Button11_Click" />
                    </td>
                </tr>
            </table>
            </div>
            <br />
 </div>
  </asp:Content> 