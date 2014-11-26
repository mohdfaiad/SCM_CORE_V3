<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingInvoiceMatching.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.BillingInvoiceMatching" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 86px;
        }
        .style3
        {
            width: 118px;
        }
        .style4
        {
            width: 128px;
        }
        .style5
        {
            width: 64px;
        }
        .style7
        {
            width: 99px;
        }
        .style8
        {
            width: 79px;
        }
        .style9
        {
            width: 95px;
        }
        .style10
        {
            width: 132px;
        }
        .style11
        {
            width: 137px;
        }

     
        .style12
        {
            width: 95px;
            height: 24px;
        }
        .style13
        {
            height: 24px;
        }
        .style14
        {
            width: 132px;
            height: 24px;
        }
        .style15
        {
            width: 137px;
            height: 24px;
        }
        .style16
        {
            width: 64px;
            height: 24px;
        }
        .style17
        {
            width: 99px;
            height: 24px;
        }
        .style18
        {
            width: 79px;
            height: 24px;
        }
        .style19
        {
            width: 128px;
            height: 24px;
        }
        .style20
        {
            width: 86px;
            height: 24px;
        }
        .style21
        {
            width: 118px;
            height: 24px;
        }
        </style>
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
    
    <style type="text/css">
        .style6
        {
            width: 100%;
            height: 25px;
        }
        .style8
        {
            width: 146px;
            height: 21px;
        }
        .style10
        {
            height: 21px;
        }
        .style11
        {
            width: 45px;
        }
        .styleUpper
        {
            text-transform: uppercase;
        }
    </style>
    
    <script language="javascript" type="text/javascript">

        function callShow1() {
            document.getElementById('blackening').style.display = 'block';
            document.getElementById('whitening').style.display = 'block';           
        }
        function callclose1() {
            document.getElementById('blackening').style.display = 'none';
            document.getElementById('whitening').style.display = 'none';
        }
        
        function getOCDADetails(button) {

            var commcode = document.getElementById(button.id.replace("btnOcDueAgent", "txtcommodity")).value;

            var TxtChargeID = button.id.replace("btnOcDueAgent", "txtocda");

            //TXTOcDueCar
            //TxtChargeID TxtTaxID TxtTotalID

            javascript: window.open('BillingEditOCDCOCDA.aspx?type=DA&CommCode=' + commcode + '&TxtChargeID=' + TxtChargeID + '&Mode=View', '', 'left=200,top=200,width=520,height=400,toolbar=0,resizable=0');

            return false;

        }

        function getOCDCDetails(button) {

            var commcode = document.getElementById(button.id.replace("btnOcDueCar", "txtcommodity")).value;

            var TxtChargeID = button.id.replace("btnOcDueCar", "txtocdc");
            
            //TXTOcDueCar
            //TxtChargeID TxtTaxID TxtTotalID


            javascript: window.open('BillingEditOCDCOCDA.aspx?type=DC&CommCode=' + commcode + '&TxtChargeID=' + TxtChargeID  + '&Mode=View', '', 'left=200,top=200,width=520,height=400,toolbar=0,resizable=0');

            return false;

        }

        function getFlightDetails(button) {

            var AWBNumber = document.getElementById(button.id.replace("btnFlightDetails", "txtAWBPrefixPnl")).value + document.getElementById(button.id.replace("btnFlightDetails", "txtAWBNo")).value;

            var TxtChargeID = button.id.replace("btnOcDueAgent", "txtfreightrate");

            //TXTOcDueCar
            //TxtChargeID TxtTaxID TxtTotalID

            javascript: window.open('BillingEditFlightDetails.aspx?AWBNumber=' + AWBNumber + '&TxtChargeID=' + TxtChargeID + '&Mode=View', '', 'left=200,top=200,width=1000,height=400,toolbar=0,resizable=0');

            return false;

        }

        function Changed() {
            document.getElementById('<%= txtspotrate.ClientID %>').focus();
            document.getElementById('<%= txtspotrate.ClientID %>').onblur();
            window.scrollBy(0, 620); 
            
        }
        
        function pageScroll1() {
            window.scrollBy(0, 480); // horizontal and vertical scroll increments
            //scrolldelay = setTimeout('pageScroll()', 100); // scrolls every 100 milliseconds
        }

        function pageScroll2() {
            window.scrollBy(0, 620); // horizontal and vertical scroll increments
            //scrolldelay = setTimeout('pageScroll()', 100); // scrolls every 100 milliseconds
        }

        function DivShow() {
        }
         
         function DivHide() {
         }
         
         
         function SelectheaderCheckboxes(headerchk) 
         {
            var gvcheck = document.getElementById("<%=grdBillingInfo.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) 
            {
                for (i = 0; i < gvcheck.rows.length; i++) 
                {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else 
            {
                for (i = 0; i < gvcheck.rows.length; i++) 
                {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
            return(false);
        }

    </script>
 </asp:Content> 
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
  <div id="contentarea">
  
    <h1>       
            Billing
 <%--img alt="" src="images/BillingMaster.png" />--%>
 </h1> 
               <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
        
  <div  class="botline"> 
  
  <table width="100%" cellpadding="5" >
   <tr>
    <td>
     Billing Dt 
    </td>
    <td>
        <asp:TextBox ID="txtbillingfrom" runat="server" Width="80px"></asp:TextBox>
        <asp:ImageButton ID="btnBillingFrom" runat="server" ImageUrl="~/Images/calendar_2.png"
           ImageAlign="AbsMiddle" />
        <asp:CalendarExtender ID="CEBillingFrom" Format="dd/MM/yyyy" TargetControlID="txtbillingfrom"
           PopupButtonID="btnBillingFrom" runat="server" PopupPosition="BottomLeft">
        </asp:CalendarExtender> &nbsp;
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
        <asp:TextBox ID="txtFlightPrefix" runat="server" Width="40px" MaxLength="3"></asp:TextBox>
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" 
                runat="server" TargetControlID="txtFlightPrefix" WatermarkText="Prefix">
            </asp:TextBoxWatermarkExtender> 
        &nbsp;
        <asp:TextBox ID="txtFlightNo" runat="server" Width="60px" MaxLength="4"></asp:TextBox>
    </td>
    <td>
     Status
    </td>
    <td>
      <asp:DropDownList ID="ddlStatus" runat="server" Width="80px" >
          <asp:ListItem Text="All" Value=""></asp:ListItem>
          <asp:ListItem Text="New" Value="New"></asp:ListItem>
<%--          <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
--%>          <asp:ListItem Text="Confirmed" Value="Confirmed"></asp:ListItem>
          <asp:ListItem Text="Reopened" Value="Reopened"></asp:ListItem>
          <asp:ListItem Text="Final" Value="Final"></asp:ListItem>
          <asp:ListItem Text="Invoiced" Value="Invoiced"></asp:ListItem>
        </asp:DropDownList>
    </td>
   </tr>
   
   <tr>
   <td>
   AWB #
   </td>
   <td>
   <asp:TextBox ID="txtAWBPrefix" runat="server" Width="40px"></asp:TextBox>
               <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" 
                runat="server" TargetControlID="txtAWBPrefix" WatermarkText="Prefix">
            </asp:TextBoxWatermarkExtender> 
        &nbsp;
    <asp:TextBox ID="txtAWBNumber" runat="server" Width="90px" MaxLength="10"></asp:TextBox>
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
   </tr>
  
  <tr>
    <td>
     Agent
    </td>
    <td>
        <asp:DropDownList ID="ddlAgentCode" runat="server" Width="80px" 
            AutoPostBack="True" onselectedindexchanged="ddlAgentCode_SelectedIndexChanged" >
        </asp:DropDownList>
        <asp:DropDownList ID="ddlAgentName" runat="server" Width="170px" 
            AutoPostBack="True" onselectedindexchanged="ddlAgentName_SelectedIndexChanged" ></asp:DropDownList>
    </td>
    <td>
    Payment Mode
    </td>
    <td>
    <asp:DropDownList ID="ddlPayType" runat="server" Width="90px" >
        <%--<asp:ListItem Text="All" Value=""></asp:ListItem><asp:ListItem Text="PP" Value="PP"></asp:ListItem><asp:ListItem Text="CC" Value="CC"></asp:ListItem><asp:ListItem Text="PC" Value="PC"></asp:ListItem><asp:ListItem Text="CP" Value="CP"></asp:ListItem>--%>
        </asp:DropDownList>
        </td><td>
     Spot Rate
    </td>
    <td>
      <asp:DropDownList ID="ddlSpotRate" runat="server" Width="90px" >
          <asp:ListItem Text="All" Value=""></asp:ListItem><asp:ListItem Text="Yes" Value="Yes"></asp:ListItem><asp:ListItem Text="No" Value="No"></asp:ListItem></asp:DropDownList>
          </td>
          <td>
          <asp:CheckBox ID="chkShipment" Text="Include Partial" runat="server" Checked="true" /> 
          </td>
          </tr>
          <tr>
    <td>
        ShipmentType</td>
    <td>
        <asp:DropDownList ID="ddlShipmentType" runat="server">
        <asp:ListItem Text="All" Value="" ></asp:ListItem>
        <asp:ListItem Text="Domestic" Value="DOM" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Inernational" Value="INT"></asp:ListItem>
        </asp:DropDownList></td>
    <td>
        &nbsp;</td>
    <td>
        &nbsp;</td><td>
          &nbsp;</td>
    <td>
        &nbsp;</td>
          <td>
              &nbsp;</td>
          </tr>
   
   <tr>
       <td colspan="5">
        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="List" 
            onclick="btnSearch_Click" />
            &nbsp;<asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" 
               onclick="btnClear_Click" />
            &nbsp;
            <asp:CheckBox ID="chkExpAll" Checked="false" runat="server" TextAlign="Right" Text="All" />
            &nbsp;<asp:Button ID="btnExportBilling"  runat="server" 
               Text="Export" CssClass="button" onclick="btnExportBilling_Click" />
                            
       </td>
        <td colspan="8">
       </td>
   </tr>
   
  </table>  
  
  </div> 
  <br /><br /><br />
  <div>

  &nbsp;&nbsp;&nbsp;
  
          <b> 
              <asp:Label ID="lblAWBCount" Visible= "false" runat="server"></asp:Label>
         </b>
         </div>
    <div style="overflow-x:auto;width:100%">

<asp:GridView ID="grdBillingInfo" runat="server"  Width="100%"
         AutoGenerateColumns="False" AllowPaging="True" ShowFooter="true" 
        PageSize="10" onpageindexchanging="grdBillingInfo_PageIndexChanging" 
        onrowcommand="grdBillingInfo_RowCommand" 
        onselectedindexchanged="grdBillingInfo_SelectedIndexChanged" 
            onrowdatabound="grdBillingInfo_RowDataBound" >

            <Columns>
                 <asp:TemplateField>
                   <HeaderTemplate>
                       <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);"/>
                   </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="SrNo" Visible = "false" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblSrNo" runat="server" Width="50px" Text='<%# Eval("SerialNumber") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" Width="50px" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="AWB #" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAWBPrefix" runat="server" Width="50px" Text='<%# Eval("AWBPrefix") %>'></asp:Label >    
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" Width="50px" />
                </asp:TemplateField>
                
                <asp:ButtonField HeaderText="AWB No." CommandName="AWBClick" DataTextField="AWBNumber" />
                
                <asp:TemplateField HeaderText="AWB Date" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblAWBDate" runat="server" Width="65px" Text='<%# Eval("AWBDate") %>'></asp:Label >
                       <asp:Label ID="lblAWBNumber" Visible="false" runat="server" Width="65px" Text='<%# Eval("AWBNumber") %>'></asp:Label >
                       <asp:Label ID="lblisFlownAsBooked" Visible="false" runat="server" Width="65px" Text='<%# Eval("isFlownAsBooked") %>'></asp:Label >
                       <asp:Label ID="lblisAllShipmentsFlown" Visible="false" runat="server" Width="65px" Text='<%# Eval("isAllShipmentsFlown") %>'></asp:Label >
                       </ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" Width="65px" />
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Agent Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shipper Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblShipperCode" runat="server" Text='<%# Eval("ShipperCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Comm" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblCommodity" runat="server" Text='<%# Eval("CommodityCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="ORG" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblOriginCode" runat="server" Text='<%# Eval("OriginCode") %>' ></asp:Label ></ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="DEST" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblDestinationCode" runat="server" Text='<%# Eval("DestinationCode") %>' ></asp:Label ></ItemTemplate><FooterTemplate>
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Charge Wt." ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblchargewt" runat="server" Text='<%# Eval("ChargedWeight","{0:#,###0.00}") %>' ></asp:Label ></ItemTemplate><FooterTemplate>
                        <%--<asp:Label ID="lblTotalChargedWeight" runat="server" /><br />--%>
                        <asp:Label ID="lblGTotalChargedWeight" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
               <asp:TemplateField HeaderText="Freight" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblIATA" runat="server"  Text='<%# Eval("FreightRate","{0:#,###0.00}") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <%--<asp:Label ID="lblTotalFreightRate" runat="server" /><br />--%>
                        <asp:Label ID="lblGTotalFreightRate" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Spot Freight" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblSpotRate" runat="server"  Text='<%# Eval("SpotRate","{0:#,###0.00}") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <%--<asp:Label ID="lblTotalSpotRate" runat="server" /><br />--%>
                        <asp:Label ID="lblGTotalSpotRate" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="OCDA" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblocda" runat="server"  Text='<%# Eval("OCDueAgent","{0:#,###0.00}") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <%--<asp:Label ID="lblTotalOCDA" runat="server" /><br />--%>
                        <asp:Label ID="lblGTotalOCDA" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="OCDC" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblocdc" runat="server" Text='<%# Eval("OCDueCar","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><FooterTemplate>
                        <%--<asp:Label ID="lblTotalOCDC" runat="server" /><br />--%>
                        <asp:Label ID="lblGTotalOCDC" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltotal" runat="server" Text='<%# Eval("TotalT","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><FooterTemplate>
                        <%--<asp:Label ID="lblTotalTotal" runat="server" /><br />--%>
                        <asp:Label ID="lblGTotalTotal" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Service Tax" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblST" runat="server"  Text='<%# Eval("ServiceTax","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><FooterTemplate>
                        <%--<asp:Label ID="lblTotalServiceTax" runat="server" /><br />--%>
                        <asp:Label ID="lblGTotalServiceTax" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Commission" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblCommAmt" runat="server" Text='<%# Eval("CommissionAmt","{0:#,###0.00}") %>' ></asp:Label ></ItemTemplate><FooterTemplate>
                        <%--<asp:Label ID="lblTotalCommissionAmt" runat="server" /><br />--%>
                        <asp:Label ID="lblGTotalCommissionAmt" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="ST on Comm" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblSTOnComm" runat="server" Text='<%# Eval("STOnCommAmt","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><FooterTemplate>
                        <%--<asp:Label ID="lblTotalSTOnCommAmt" runat="server" /><br />--%>
                        <asp:Label ID="lblGTotalSTOnCommAmt" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="TDS on Comm" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblTDSOnComm" runat="server" Text='<%# Eval("TDSOnCommAmt","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><FooterTemplate>
                       <%--<asp:Label ID="lblTotalTDSOnCommAmt" runat="server" /><br />--%>
                        <asp:Label ID="lblGTotalTDSOnCommAmt" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Final" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblfinal" runat="server" Text='<%# Eval("Final","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><FooterTemplate>
                        <%--<asp:Label ID="lblTotalFinal" runat="server" /><br />--%>
                        <asp:Label ID="lblGTotalFinal" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblconfirmed" runat="server"  Text='<%# Eval("Confirmed") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Spot Rate" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblSpotRateStatus" runat="server"  Text='<%# Eval("SpotRateStatus") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="InvoiceNo" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:HyperLink ID="lblInvoiceNo" runat="server" Text='<%# Eval("InvoiceNumber") %>' NavigateUrl='<%# "~/FrmInvoiceListing.aspx?invno="+ Eval("InvoiceNumber") %>'></asp:HyperLink>
                    </ItemTemplate><HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Mode" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblPayMode" runat="server"  Text='<%# Eval("PayMode") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Currency" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvMatchFlag" runat="server"  Text='<%# Eval("InvMatchFlag") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                   <asp:TemplateField HeaderText="Rate Reprocess" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblReprocess" runat="server"  Text='<%# Eval("Reprocess") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
            </Columns> 
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle CssClass="titlecolr" Font-Bold="True" HorizontalAlign="Right" />
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
</asp:GridView>

</div>  

<div>
<asp:Panel ID="pnlCommodityGrid" runat="server" Visible="false">
<p>
<asp:Label Text="AWB Rate Audit:" ID="lblCommodity" runat="server" 
         Font-Bold="True" Font-Size="Medium"></asp:Label></p><asp:GridView ID="grdCommodity" runat="server"  Width="100%" 
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
                       <asp:Label ID="lblSrNo" runat="server" Width="50px" Text='<%# Eval("SerialNumber") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" Width="50px" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="AWB Prefix" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblPrefix" runat="server" Width="50px" Text='<%# Eval("AWBPrefix") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" Width="50px" />
                </asp:TemplateField>
                
                 <asp:ButtonField HeaderText="AWB No." DataTextField="AWBNumber" ItemStyle-HorizontalAlign="Center"/>
                
                 
                 <asp:TemplateField HeaderText="Commodity Code" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                       <asp:Label ID="lblcommcode" runat="server" Width="50px" Text='<%# Eval("CommodityCode") %>' ></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="ChargedWt" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblchargewt" runat="server" Width="50px" Text='<%# Eval("ChargedWeight","{0:#,###0.00}") %>' ></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Freight" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                          <asp:Label ID="lblMKT" runat="server" Width="50px"  Text='<%# Eval("FreightRate","{0:#,###0.00}") %>' ></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Spot Freight" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                          <asp:Label ID="lblSpot" runat="server" Width="50px"  Text='<%# Eval("SpotFreight","{0:#,###0.00}") %>' ></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="OCDA" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblocda" runat="server" Width="50px"  Text='<%# Eval("OCDueAgent","{0:#,###0.00}") %>' ></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="OCDC" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblocdc" runat="server" Width="50px"  Text='<%# Eval("OCDueCar","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltotal" runat="server" Width="50px" Text='<%# Eval("TotalT","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Service Tax" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblServTax" runat="server" Width="50px"  Text='<%# Eval("ServiceTax","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Commission" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblCommission" runat="server" Width="50px"  Text='<%# Eval("CommissionAmt","{0:#,###0.00}") %>' ></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="ST on Comm" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblstoncommamt" runat="server" Width="50px" Text='<%# Eval("STOnCommAmt","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="TDS on Comm" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbltdsoncommamt" runat="server" Width="50px" Text='<%# Eval("TDSOnCommAmt","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Final" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblfinal" runat="server" Width="50px" Text='<%# Eval("Final","{0:#,###0.00}") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblconfirmed" runat="server" Width="50px"  Text='<%# Eval("Confirmed") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flag" Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblFlag" runat="server" Width="40px"  Text='<%# Eval("Flag") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="InvoiceNo" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("InvoiceNumber") %>'></asp:Label ></ItemTemplate><HeaderStyle Wrap="True">
                    </HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Spot Rate" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" Visible="false">
                    <ItemTemplate>
                       <asp:Label ID="lblSpotRate" Text='<%# Eval("isSpotRate") %>' runat="server" Width="40px"></asp:Label ></ItemTemplate><HeaderStyle Wrap="True">
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
 
 <div> 
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
        <asp:Button ID="btnUpload" runat="server" CssClass="button" Text="Upload" onclick="btnUpload_Click" 
          />
    </td>
    <td>
    <asp:Button ID="btnInvMatch" runat="server" Text="Match Invoice" Visible="false" 
                            CssClass="button" />
    </td>
    <td>
    <asp:Button ID="btnAllInvMatch" runat="server" Text="Match All Invoices" Visible="false" 
                            CssClass="button" onclick="btnAllInvMatch_Click" />
    </td>
    <td>
    <asp:Button ID="btnInvoiceMatching" runat="server" Text="Match Invoices" 
            CssClass="button" onclick="btnInvoiceMatching_Click" />
    </td>
   </tr>
   
  </table>
 </asp:Panel>
  </div> 
 
 
 
<div>

<asp:UpdatePanel ID="UPBillingAudit" runat="server">
<ContentTemplate>
<asp:Panel ID="Panel1" runat="server">
<p>
<asp:Label Text="Billing Details:" ID="lblBillingDetails" runat="server" 
         Font-Bold="True" Font-Size="Medium"></asp:Label>&nbsp;
<asp:TextBox ID="txtAgentName" runat="server" Width="216px" Enabled="False" ></asp:TextBox>&nbsp&nbsp            
<asp:Button ID ="btnReprocess" runat="server" Text ="Reprocess" CssClass="button" Visible="false" OnClick="btnReprocess_Click" />
</p>
<table>
<tr>
<td>AWB Prefix</td><td><asp:TextBox ID="txtAWBPrefixPnl" runat="server" Width="50px" Enabled="False" ></asp:TextBox></td><td class="style9">
   AWB No
  </td>
 <td>
     <asp:TextBox ID="txtAWBNo" runat="server" Width="70px" Enabled="False" ></asp:TextBox></td><td>AWB Date</td><td><asp:TextBox ID="txtAWBDatePnl" runat="server" Width="70px" Enabled="False" ></asp:TextBox></td><td></td><td></td><td></td><td></td><td></td><td></td>
</tr>
<tr>

<td>Payment Type</td><td><asp:TextBox ID="txtPaymentTypePnl" runat="server" Width="60px" Enabled="false" ></asp:TextBox></td><td class="style10">
Origin
</td>
<td>
<asp:TextBox ID="txtOrgPnl" runat="server" Width="60px" Enabled="False" ></asp:TextBox></td><td class="style11">
Destination
</td>
<td class="style5">
<asp:TextBox ID="txtDestPnl" runat="server" Width="60px" Enabled="False" ></asp:TextBox></td><td class="style7">
Flight #
</td>
<td colspan="2">
<asp:TextBox ID="txtFlightNoPnl" runat="server" Width="120px" Enabled="False" ></asp:TextBox><asp:ImageButton ID="btnFlightDetails" runat="server" ImageUrl="~/Images/list_bullets.png"
       ImageAlign="AbsMiddle" OnClientClick="javascript:getFlightDetails(this);" />
</td>
<td>Flight Date</td><td colspan="2">
  <asp:TextBox ID="txtFlightDatePnl" runat="server" Width="200px" Enabled="False" ></asp:TextBox></td></tr><tr>
 <td class="style12">
  Gross Wt.
 </td>
 <td class="style13">
  <asp:TextBox ID="txtgrosswt" runat="server" Width="60px" Enabled="false" ></asp:TextBox></td><td class="style14">
  Chargable Wt.
 </td>
 <td class="style13">
 <asp:TextBox ID="txtchargablewt" runat="server" Width="60px" Enabled="false"></asp:TextBox></td><td class="style15">
  Rate/KG.
 </td>
 <td class="style16">
  <asp:TextBox ID="txtRatePerKg" runat="server" Width="60px" ReadOnly="True"></asp:TextBox></td><td class="style17">
   Freight
  </td>
  <td class="style13">
  <asp:TextBox ID="txtfreightrate" runat="server" Width="60px" ReadOnly="True"></asp:TextBox></td><td class="style17">
   Spot Freight
  </td>
  <td class="style13">
  <asp:TextBox ID="txtspotrate" runat="server" Width="60px" ReadOnly="false"></asp:TextBox></td><td class="style18">
   OCDA
  </td>
  <td class="style19">
  <asp:TextBox ID="txtocda" runat="server" Width="60px" ReadOnly="True"></asp:TextBox><asp:ImageButton ID="btnOcDueAgent" runat="server" ImageUrl="~/Images/list_bullets.png"
       ImageAlign="AbsMiddle" OnClientClick="javascript:getOCDADetails(this);" />
  </td>
  

 </tr>
 <tr>
 <td class="style20">
   OCDC
  </td>
  <td class="style21">
  <asp:TextBox ID="txtocdc" runat="server" Width="60px" ReadOnly="True"></asp:TextBox><asp:ImageButton ID="btnOcDueCar" runat="server" ImageUrl="~/Images/list_bullets.png"
       ImageAlign="AbsMiddle" OnClientClick="javascript:getOCDCDetails(this);" />
  </td>
  
 <td class="style9">
  Commodity
 </td>
 <td>
  <asp:TextBox ID="txtcommodity" runat="server" Width="60px" Enabled="False"></asp:TextBox></td><td class="style10">
  Dimensions
 </td>
 <td>
  <asp:TextBox ID="txtdimensions" runat="server" Width="60px" Enabled="False"></asp:TextBox></td><td class="style11">
   Total Before ST
  </td>
  <td class="style5">
  <asp:TextBox ID="txtTotal" runat="server" Width="60px" Enabled="False"></asp:TextBox></td><td class="style7">
  Discount %
   </td>
   <td>
  <asp:TextBox ID="txtDiscount" runat="server" Width="60px" ></asp:TextBox></td><td class="style8">
   Amount
  </td>
  <td class="style4">
  <asp:TextBox ID="txtdiscamount" runat="server" Width="60px" Enabled="False" ></asp:TextBox></td></tr><tr>
 <td class="style1">
 Total After Discount
 </td>
 <td class="style3">
  <asp:TextBox ID="txttotalafterdiscount" runat="server" Width="60px" 
         Enabled="False"></asp:TextBox></td><td class="style9">
 Service Tax
 </td>
 <td>
  <asp:TextBox ID="txtServiceTax" runat="server" Width="60px"></asp:TextBox></td><td class="style10">
     Total AWB
     <br />
     Amount
 </td>
 <td>
 <asp:TextBox ID="txtTADST" runat="server" Width="60px" 
         Enabled="False"></asp:TextBox></td><td class="style11">
   Commission %
  </td>
 <td class="style5">
  <asp:TextBox ID="txtcommission" runat="server" Width="60px"></asp:TextBox></td><td class="style7">
     Commission
     <br />
     Amount
 </td>
 <td>
  <asp:TextBox ID="txtcommissionamt" runat="server" Width="60px" Enabled="False"></asp:TextBox></td><td class="style8">
    ST on Comm
 </td>
 <td class="style4">
  <asp:TextBox ID="txtSTOnComm" runat="server" Width="60px" Enabled="False"></asp:TextBox></td></tr><tr>
  <td class="style9">TDS on Comm %</td><td><asp:TextBox ID="txtTDSCommPer" runat="server" Width="60px"></asp:TextBox></td><td class="style10">TDS on Comm Amount</td><td><asp:TextBox ID="txtTDSCommAmt" runat="server" Width="60px" Enabled="False"></asp:TextBox></td><td class="style11">Billed Amount</td><td class="style5"><asp:TextBox ID="txtRevisedTotal" runat="server" Width="60px" Enabled="False"></asp:TextBox></td><td class="style4">
Spot Rate ID
</td>
<td colspan="2">
<asp:TextBox ID="txtSpotRateID" runat="server" Width="120px" Enabled="False" ></asp:TextBox></td><td>
  <asp:TextBox ID="txtTDSFrtPer" runat="server" Width="60px" Visible="false"></asp:TextBox></td><td class="style4">
  <asp:TextBox ID="txtTDSFrtAmt" runat="server" Width="60px" Enabled="False" Visible="false"></asp:TextBox></td><td class="style3"><asp:TextBox ID="txttotalaftertax" runat="server" Width="60px" Enabled="False" Visible="false"></asp:TextBox></td></tr></table>
  </asp:Panel>
  
</ContentTemplate>
</asp:UpdatePanel>
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
                        <asp:Button ID="btnProformaInvoice" Visible="false" runat="server" 
                            Text="Generate Proforma" CssClass="button" 
                            onclick="btnProformaInvoice_Click" />&nbsp
                        <asp:Button ID="btnGenerateInvoice" Visible="false" runat="server" 
                            Text="Generate Invoice" CssClass="button" onclick="btnGenerateInvoice_Click" 
                             />&nbsp
                        <asp:Button ID="btnGenerateSupplementaryInvoice" Visible="false" runat="server" 
                            Text="Generate Supp. Invoice" CssClass="button" onclick="btnGenerateSupplementaryInvoice_Click"
                             />&nbsp     
                        <asp:Button ID="btnRouteDetails"  Visible="false" runat="server" 
                            Text="Route Details" CssClass="button" onclick="btnRouteDetails_Click" />&nbsp
                        <asp:Button ID="btnTrackAWB" Visible="false" runat="server" 
                            Text="Track AWB" CssClass="button" />&nbsp
                        <asp:Button ID="btnPrint" Visible="false" runat="server" 
                            Text="Export" CssClass="button" onclick="btnPrint_Click" />
                            
                        <asp:Button ID="btnPanelCalc" Visible="false" runat="server" Text="Calc" 
                            CssClass="button" onclick="btnPanelCalc_Click"/>
                            
                        <asp:Button ID="btnSTChange" Visible="false" runat="server" Text="ST Change" 
                            CssClass="button" onclick="btnSTChange_Click"/>
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
                        <asp:HiddenField ID="hfIncludePartial" runat="server" Value="" />
                    </td>
                </tr>
            </table>
    <%--        </div>
            <br />
 </div>--%>
   <div id="blackening" class="black_overlay">
        </div>
 <div id="whitening" class="white_content" style="width: 600px; left: 27%;">
<asp:Panel ID="Panel2" runat="server">
<p>
<asp:Label Text="Reprocessed Billing Details:" ID="Label1" runat="server" 
         Font-Bold="True" Font-Size="Medium"></asp:Label>&nbsp&nbsp&nbsp
         <br />
<asp:Label Text="" ID="lblError" runat="server" Visible="false" ForeColor="Brown" 
         Font-Bold="True" Font-Size="Medium"></asp:Label>&nbsp;

</p>
<table>
 <tr>
 <td class="style12">
  Pcs.
 </td>
 <td class="style13">
  <asp:TextBox ID="txtPcs" runat="server" Width="60px" ></asp:TextBox></td><td class="style14">
  Chargable Wt.
 </td>
 <td class="style13">
 <asp:TextBox ID="txtCWt" runat="server" Width="60px" ReadOnly="true"></asp:TextBox></td><td class="style15">
  Rate/KG.
 </td>
 <td class="style16">
  <asp:TextBox ID="txtKGRate" runat="server" Width="60px" ReadOnly="True"></asp:TextBox></td><td class="style17">
   IATA Freight
  </td>
  <td class="style13">
  <asp:TextBox ID="txtFreight" runat="server" Width="60px" ReadOnly="True"></asp:TextBox></td></tr><tr>
   <td class="style17">
   MKT Freight
  </td>
  <td class="style13">
  <asp:TextBox ID="txtNewMKT" runat="server" Width="60px" ReadOnly="True"></asp:TextBox></td><td class="style18">
   OCDA
  </td>
  <td class="style19">
  <asp:TextBox ID="txtNewocda" runat="server" Width="60px" ReadOnly="True"></asp:TextBox></td><td class="style20">
   OCDC
  </td>
  <td class="style21">
  <asp:TextBox ID="txtNewOcdc" runat="server" Width="60px" ReadOnly="True"></asp:TextBox></td><td class="style11">
   Billed Amount
   </td>
  <td class="style5">
  <asp:TextBox ID="txtNewTotal" runat="server" Width="60px" Enabled="False"></asp:TextBox></td></tr><tr>
<td></td>
<td>
  <asp:Button ID="btnApply" runat="server" CssClass="button" Text="Apply" OnClick="btnApplyNewRate_Click" />
</td>
<td>
  <asp:Button ID="btnDiscard" runat="server" CssClass="button" Text="Discard" OnClick="btnDiscard_Click"/>
</td>
</tr>
</table>  
      </asp:Panel>

 </div>

     </div>
     </div>
</asp:Content> 
