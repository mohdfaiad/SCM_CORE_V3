<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingDestAgentCollection.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.BillingDestAgentCollection" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

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
        function Download(count) {
            window.open('ORReceiptDownload.aspx', 'Download', 'left=100,top=100,width=800,height=420,toolbar=0,resizable=1');
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
        
</script>

 </asp:Content> 
 
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <style type="text/css">
     
    </style>
    
    <script language="javascript" type="text/javascript">

        function DoPostBackWithRowIndex(rowIndex) {
            if (document.getElementById('<%=HdnSelectedRowIndex.ClientID%>') != null) {
                document.getElementById('<%=HdnSelectedRowIndex.ClientID%>').value = rowIndex;
            }
            return true;
        }

        function DisableTextBox() {
            alert('in');
            var sel = document.getElementById("ddlPaymentType");
            if (sel.options[sel.selectedIndex].value == "Cash") {
                alert('y');
            }
        }
        
        function CreditCardPopup() {

            //Implementing Validation before opening the card pop-up

            var PendingAmt = document.getElementById("<%= hdPendingAmt.ClientID %>");
            var CollectedAmt = document.getElementById("<%= txtCollectedAmount.ClientID %>");
            var ParitalRemarks = document.getElementById("<%= txtPPRemarks.ClientID %>");
            var TDS = document.getElementById("<%= txtTDS.ClientID %>");

            if (CollectedAmt.value == "" || CollectedAmt.value == "0") {
                alert("Please Enter amount to be collected !");
                CollectedAmt.focus();
                return false;
            }

            if (TDS.value == "") {
                TDS.value = "0";
            }

            if (parseFloat(CollectedAmt.value) > parseFloat(PendingAmt.value)) {
                alert("Amount to be paid is only " + PendingAmt.value + " !");
                CollectedAmt.focus();
                return false;
            }

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

            //Validations complete

            document.getElementById('CreditLight').style.display = 'block';
            document.getElementById('CreditFade').style.display = 'block';
        }
        
        function CloseWindow() {
            document.getElementById('CreditLight').style.display = 'none';
            document.getElementById('CreditFade').style.display = 'none';
        }

    </script>
    
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            //document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }
        function ViewPanelSplit() {
            document.getElementById('Lightsplit').style.display = 'block';
            document.getElementById('fadesplit').style.display = 'block';
        }
        function HidePanelSplit() {
            document.getElementById('Lightsplit').style.display = 'none';
            document.getElementById('fadesplit').style.display = 'none';
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
    </style>
    
    <style>
.black_overlaynew
		{
			display: none;
			position:absolute;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 1000px;
			background-color: black;
			z-index:1001;
			-moz-opacity:0.8;
			opacity:0.4;
			filter:alpha(opacity=80);
			float:left;
		}
	.white_contentnew 
		{
		    margin:0 auto;
			display: none;
			position:fixed;
			top: 5%;
			left: 10%;
			height: 81%;
			padding: 16px;
			border: 16px solid #ccdce3;
			background-color: white;
			z-index:1002;
		
            
		}
		.black_overlaymsg{
			display: none;
			position: fixed;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 100%;
			background-color: White;
			z-index:1001;
			-moz-opacity: 0.8;
			opacity:.80;
			filter: alpha(opacity=80);
		}
		.white_contentmsg {
			display: none;
			position: fixed;
			top: 45%;
			left: 45%;
			width: 5%;
			height: 5%;
			padding: 16px;
			background-color: Transparent;
			z-index:1002;
			
		}
		
</style>

<style type="text/css">
       .black_overlaynew_Credit
		{
			display: none;
			position:absolute;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 1000px;
			background-color: black;
			z-index:1001;
			-moz-opacity:0.8;
			opacity:0.4;
			filter:alpha(opacity=80);
			float:left;
		}
     
        	.white_contentnew_Credit 
		{
		    margin:0 auto;
			display: none;
			position:fixed;
			top: 5%;
			left: 35%;
			height: 30%;
			padding: 16px;
			border: 16px solid #ccdce3;
			background-color: white;
			z-index:1002;
		
            
		}
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
    
    <div id="contentarea">
  
    <h1>       
            Destination Invoice Collection
    </h1> 
        
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
        
        
  <div class="botline"> 
  <table width="100%" cellpadding="3">
   <tr>
    <td>
     Invoice Dt *
    </td>
    <td>
        <asp:TextBox ID="txtInvoiceFrom" runat="server" Width="80px"></asp:TextBox>
        <asp:ImageButton ID="btnInvoiceFrom" runat="server" ImageUrl="~/Images/calendar_2.png"
           ImageAlign="AbsMiddle" />
        <asp:CalendarExtender ID="CEInvoiceFrom" Format="dd/MM/yyyy" TargetControlID="txtInvoiceFrom"
           PopupButtonID="btnInvoiceFrom" runat="server" PopupPosition="BottomLeft">
        </asp:CalendarExtender>
        &nbsp;
         <asp:TextBox ID="txtInvoiceTo" runat="server" Width="80px"></asp:TextBox>
         <asp:ImageButton ID="btnInvoiceTo" runat="server" ImageUrl="~/Images/calendar_2.png"
        ImageAlign="AbsMiddle" />
         <asp:CalendarExtender ID="CEInvoiceTo" Format="dd/MM/yyyy" TargetControlID="txtInvoiceTo"
            PopupButtonID="btnInvoiceTo" runat="server" PopupPosition="BottomLeft">
         </asp:CalendarExtender>
    </td>
    <td>
     Invoice #
    </td>
    <td>
        <asp:TextBox ID="txtInvoiceNumber" runat="server" Width="90px"></asp:TextBox>
    </td>
    <td>
     AWB #
    </td>
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
    <td>
     Agent
    </td>
    <td>
        <asp:DropDownList ID="ddlAgentCode" runat="server" Width="80px" 
            AutoPostBack="True" onselectedindexchanged="ddlAgentCode_SelectedIndexChanged" >
        </asp:DropDownList>
        &nbsp;
        <asp:DropDownList ID="ddlAgentName" runat="server" Width="170px" 
            AutoPostBack="True" onselectedindexchanged="ddlAgentName_SelectedIndexChanged" >
        </asp:DropDownList>
    </td>
     <td>
     Origin
    </td>
    <td>
        <asp:TextBox ID="txtOrigin" runat="server" Width="90px"></asp:TextBox>
        <asp:AutoCompleteExtender ID="txtOrigin_AutoCompleteExtender" runat="server" 
         ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
                 TargetControlID="txtOrigin">
               </asp:AutoCompleteExtender>
    </td>
    <td>
     Bill Type
    </td>
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
    <td>
        <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
            onclick="btnList_Click" />
        &nbsp;
        <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" 
            onclick="btnClear_Click" />
    </td>

<td></td>
   </tr>
  </table>  
</div>
<br />
<div id="divPrint" style="float:left;">
<asp:GridView ID="grdInvoiceList" Width="80%" runat="server"
         AutoGenerateColumns="False" AllowPaging="True" PageSize="10" 
        onrowdatabound="grdInvoiceList_RowDataBound" 
        onpageindexchanging="grdInvoiceList_PageIndexChanging" ShowFooter="true" 
        onrowcommand="grdInvoiceList_RowCommand">
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
               
                <asp:TemplateField HeaderText="Agent Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" Visible=false>
                    <ItemTemplate>
                       <asp:Label ID="lblSrNo" runat="server" Text='<%# Eval("SrNo") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                            
                 <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:RadioButton ID="rbSelect" AutoPostBack="true" Checked="false" runat="server" OnCheckedChanged="rbSelect_CheckedChanged" />
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                 </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Agent Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("AgentName") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblGTotalText" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Central Agent" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblCentralAgent" runat="server" Text='<%# Eval("CentralAgent") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Local Agent" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblLocalAgent" runat="server" Text='<%# Eval("LocalAgent") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Invoice Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Eval("InvoiceNumber") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Invoice Amount" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# Eval("InvoiceAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblGTotalInvoiceAmt" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Collected Amount" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblCollectedAmount" runat="server" Text='<%# Eval("CollectedAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblGTotalCollectedAmt" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="TDS" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblTDSAmount" runat="server" Text='<%# Eval("TDSAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="194C" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lbl194CAmount" runat="server" Text='<%# Eval("Amt194C") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblGTotal194CAmt" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Payment Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblPaymentType" runat="server" Text='<%# Eval("PaymentType") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="DCM Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblDCMAmount" runat="server" Text='<%# Eval("DCMAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="DCM Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblDCMType" runat="server" Text='<%# Eval("DCMType") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Cheque#/DD#/ RTGS#" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblChequeDdNo" runat="server" Text='<%# Eval("ChequeDdNo") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Cheque Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblChequeDate" runat="server" Text='<%# Eval("ChequeDate") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Bank Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblBankName" runat="server" Text='<%# Eval("BankName") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Payment Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblPaymentDt" runat="server" Text='<%# Eval("PaymentDate") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Pending Amount" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblPendingAmount" runat="server" Text='<%# Eval("PendingAmount") %>' ></asp:Label >
                    </ItemTemplate>
                     <FooterTemplate>
                        <asp:Label ID="lblGTotalPendingAmt" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Aging" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblAging" runat="server" Text='<%# Eval("Aging") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
            <%--    
                 <asp:TemplateField HeaderText="ORNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:LinkButton ID="lblORNumber" runat="server" CommandName="ORNumber" CommandArgument='<%# Eval("ORRecieptNo")%>' Text='<%# Eval("ORRecieptNo") %>' ></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>--%>
                
                 <asp:TemplateField HeaderText="Invoice Details" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                <asp:LinkButton ID="InvoiceDetails" runat="server" Text="Details" CommandArgument='<%# Eval("InvoiceNumber") %>' CommandName="Detail"></asp:LinkButton>
                </ItemTemplate>
                </asp:TemplateField>
                
        </Columns> 
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle BackColor="#336699" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
</asp:GridView>



<asp:Panel runat="server" ID="pnlAddCollection" GroupingText = "Collection" Visible="false">
<div>
  <table width=" 95%">
   <tr>
    <td>
     Collected Amount 
    </td>
    <td>
        <asp:TextBox ID="txtCollectedAmount" runat="server" Width="80px"></asp:TextBox>
    </td>
    <td>
     TDS 
    </td>
    <td>
        <asp:TextBox ID="txtTDS" runat="server" Width="80px"></asp:TextBox>
    </td>
    <td>
     Payment Type
    </td>
    <td>
        <asp:DropDownList ID="ddlPaymentType" runat="server" Width="100px" 
            AutoPostBack="True" 
            onselectedindexchanged="ddlPaymentType_SelectedIndexChanged" >
        </asp:DropDownList>
          <asp:ImageButton ID="btnCreditPopup" runat="server" ImageUrl="~/Images/creditcardicon1.png"
        ImageAlign="AbsMiddle" OnClientClick="javascript:CreditCardPopup();return false;" Enabled="false"/>
    </td>
    <td>
     Cheque/DD#/RTGS# 
    </td>
    <td>
        <asp:TextBox ID="txtChequeDdNo" runat="server" Width="80px"></asp:TextBox>
    </td>
    <td></td>
    </tr>
    <tr>
    <td>
     Cheque Date 
    </td>
    <td>
        <asp:TextBox ID="txtChequeDate" runat="server" Width="80px">
        </asp:TextBox>
        <asp:CalendarExtender ID="CEChequeDate" runat="server" Enabled="True" TargetControlID="txtChequeDate"  Format="dd-MM-yyyy">
            </asp:CalendarExtender>
    </td>
    <td>
     Bank Name 
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtBankName" runat="server" Width="180px"></asp:TextBox>
    </td>
    
    <td>
        
    </td>
    <td>
        
    </td>
    <td>
        
    </td>
    <td></td>
    </tr>
   
      <tr>
          <td>
              194C</td>
          <td>
              <asp:TextBox ID="txt194C" runat="server" Width="80px"></asp:TextBox>
          </td>
          <td>
              PPRemarks</td>
          <td colspan="2">
              <asp:TextBox ID="txtPPRemarks" runat="server" Width="180px"></asp:TextBox>
          </td>
          <td>
              &nbsp;</td>
          <td>
              &nbsp;</td>
          <td>
              <asp:Button ID="btnSave" runat="server" CssClass="button" 
                  onclick="btnSave_Click" Text="Add" OnClientClick="return ValidateRemarks()"/>
              &nbsp;
              <asp:Button ID="btnEdit" runat="server" CssClass="button" 
                  onclick="btnEdit_Click" Text="Update" />
              &nbsp;
              <asp:Button ID="btnDelete" runat="server" CssClass="button" 
                  onclick="btnDelete_Click" Text="Delete" />
          </td>
          <td>
              &nbsp;</td>
      </tr>
   
  </table>  
  </div>
</asp:Panel>

<br/>
<asp:Panel runat="server" ID="pnlDCMAdjustment" GroupingText = "DCM Adjustment" Visible="false">
<div>
  <table width=" 95%">
   <tr>
    <td>
     DCM Number 
    </td>
    <td>
        <asp:TextBox ID="txtDCMNumber" runat="server" Width="120px"></asp:TextBox>
    </td>
    <td>
     Amount 
    </td>
    <td>
        <asp:TextBox ID="txtDCMAmount" runat="server" Width="80px"></asp:TextBox>
    </td>
    <td>
     Type
    </td>
    <td>
        <asp:DropDownList ID="ddlDCMType" runat="server" Width="100px" >
          <asp:ListItem Text="Debit" Value="1"></asp:ListItem>
          <asp:ListItem Text="Credit" Value="2"></asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
     Reason 
    </td>
    <td>
        <asp:TextBox ID="txtReason" runat="server" Width="150px" TextMode="MultiLine"></asp:TextBox>
    </td>
        <td><asp:Button ID="btnSaveDCM" runat="server" CssClass="button" Text="Save" 
                onclick="btnSaveDCM_Click" />
            </td>
    </tr>
   
  </table>  
  </div>
</asp:Panel>

</div> 

  <div id="fotbut">
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnExport" Visible="false" runat="server" 
                            Text="Export" CssClass="button" onclick="btnExport_Click" />
                    </td>
                    <td><asp:HiddenField ID="HdnSelectedRowIndex" runat="server" Value="" /></td>
                </tr>
            </table>
            </div>
            <br />
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
    
    <div id="msgfade" class="black_overlaymsg">
    </div>
    <asp:HiddenField ID="hdPendingAmt" runat="server" />
    <asp:HiddenField ID="hdTransactionId" runat="server" />
    <div id="Lightsplit"  class="white_contentnew">
    <div >
    <h3 style="text-align:center;"> AWB Details</h3>
    <asp:GridView ID="grdInvoiceDetails" runat="server" AutoGenerateColumns="false" HeaderStyle-Wrap="false">
    <Columns>
    <asp:TemplateField HeaderText="AWB No">
    <ItemTemplate>
    <asp:Label ID="lblAWBPrefix" runat="server" Text='<%# Eval("AWBPrefix") %>'></asp:Label>
    <asp:Label ID="Label1" runat="server" Text="-"></asp:Label>
    <asp:Label ID="lblAWBNo" runat="server" Text='<%# Eval("AWBNumber") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField> 
    <asp:TemplateField HeaderText="AWB Date">
    <ItemTemplate>
    <asp:Label ID="lblAWBDate" runat="server" Text='<%# Eval("AWBDate") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField> 
     <asp:TemplateField HeaderText="Origin">
    <ItemTemplate>
    <asp:Label ID="lblOrigin" runat="server" Text='<%# Eval("Origin") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField> 
     <asp:TemplateField HeaderText="Destination">
    <ItemTemplate>
    <asp:Label ID="lblDestination" runat="server" Text='<%# Eval("Destination") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField> 
     <asp:TemplateField HeaderText="Chargable Wt">
    <ItemTemplate>
    <asp:Label ID="lblChargableWt" runat="server" Text='<%# Eval("ChargableWeight") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Tarrif">
    <ItemTemplate>
    <asp:Label ID="lblTarrifRate" runat="server" Text='<%# Eval("TariffRate") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Freight PP">
    <ItemTemplate>
    <asp:Label ID="lblFreightPP" runat="server" Text='<%# Eval("PPFreight") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
        <asp:TemplateField HeaderText="OCDC PP">
    <ItemTemplate>
    <asp:Label ID="lblOCDCPP" runat="server" Text='<%# Eval("PPOCDC") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
      <asp:TemplateField HeaderText="OCDA PP">
    <ItemTemplate>
    <asp:Label ID="lblOCDAPP" runat="server" Text='<%# Eval("PPOCDA") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
      <asp:TemplateField HeaderText="Freight CC">
    <ItemTemplate>
    <asp:Label ID="lblFreightCC" runat="server" Text='<%# Eval("CCFreight") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
        <asp:TemplateField HeaderText="OCDC CC">
    <ItemTemplate>
    <asp:Label ID="lblOCDCCC" runat="server" Text='<%# Eval("CCOCDC") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
      <asp:TemplateField HeaderText="OCDA CC">
    <ItemTemplate>
    <asp:Label ID="lblOCDACC" runat="server" Text='<%# Eval("CCOCDA") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
       <asp:TemplateField HeaderText="Service Tax">
    <ItemTemplate>
    <asp:Label ID="lblServiceTax" runat="server" Text='<%# Eval("ServiceTax") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
      <asp:TemplateField HeaderText="Total(AWB)">
    <ItemTemplate>
    <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("TotalAWBCharges") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
       <asp:TemplateField HeaderText="Spot Rate">
    <ItemTemplate>
    <asp:Label ID="lblSpotRate" runat="server" Text='<%# Eval("SpotRate") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Spot Freight">
    <ItemTemplate>
    <asp:Label ID="lblSpotFreight" runat="server" Text='<%# Eval("SpotFreight") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
      
    </Columns>
    <AlternatingRowStyle   Wrap="false"/>
                                                    <EditRowStyle CssClass="grdrowfont" />
                                                    <FooterStyle CssClass="grdrowfont" />
                                                    <HeaderStyle CssClass="titlecolr" Wrap="False" />
                                                    <RowStyle CssClass="grdrowfont"  HorizontalAlign="Center" Wrap="False" />
    </asp:GridView>
    <h3 style="text-align:center;">Charges Summary</h3>
    <div>
    <table border="1px" align="center">
    <tr>
    <td align="right">Total Charges Due Airline :</td>
    <td>
        &nbsp;</td>
    <td>&nbsp;</td>
    <td><asp:Label ID="txtTotalChargesDueAirline" runat="server"></asp:Label></td>
    </tr>
    <tr>
    <td align="right">Total Base Amount for Service Tax :</td>
    <td>
        <asp:Label ID="txtBaseAmtServiceTax" runat="server"></asp:Label></td>
    <td>
        &nbsp;</td>
    <td>
        &nbsp;</td>
    </tr>
    <tr>
    <td align="right">Total Service Tax Due Airline :</td>
    <td>
        &nbsp;</td>
    <td>
        &nbsp;</td>
    <td>
        <asp:Label ID="txtTaxDueAirline" runat="server"></asp:Label></td>
    </tr>
    <tr>
    <td align="right">TDS On Commission :</td>
    <td>
        &nbsp;</td>
    <td>
        &nbsp;</td>
    <td>
        <asp:Label ID="txtTDSOnComm" runat="server"></asp:Label></td>
    </tr>
    <tr>
    <td align="right">Commissionable Sales :</td>
    <td>
        </td>
    <td>
        <asp:Label ID="txtCommSales" runat="server" ></asp:Label></td>
    <td></td>
    </tr>
    <tr>
    <td align="right">Agent&#39;s Commission :</td>
    <td>
        &nbsp;</td>
    <td>
        <asp:Label ID="txtAgentComm" runat="server" ></asp:Label></td>
    <td>&nbsp;</td>
    </tr>
    <tr>
    <td align="right">Other Charges Due Agent :</td>
    <td>
        &nbsp;</td>
    <td>
        <asp:Label ID="txtOCDA" runat="server" ></asp:Label></td>
    <td>&nbsp;</td>
    </tr>
    <tr>
    <td align="right" >Service Tax On Commission :</td>
    <td >
        </td>
    <td>
        <asp:Label ID="txtSTOnComm" runat="server"></asp:Label></td>
    <td></td>
    </tr>
    <tr>
    <td align="right">Total Deductions :</td>
    <td>
        &nbsp;</td>
    <td>&nbsp;</td>
    <td>
        <asp:Label ID="txtTotalDeductions" runat="server" ></asp:Label></td>
    </tr>
    <tr>
    <td align="right">NET DUE AIRLINES/AGENT :</td>
    <td>
        &nbsp;</td>
    <td>&nbsp;</td>
    <td>
        <asp:Label ID="txtNetCharges" runat="server" ReadOnly="true"></asp:Label></td>
    </tr></table>
    </div>
    </div>
    <table align="center">
    <tr>
    <td>
    		<asp:Button ID="btnClose" runat="server" OnClientClick="javascript:HidePanelSplit()"  CssClass="button" Text="Close" />
    		</td>
    		</tr>
    		</table>
    		
    
		</div>
		<div id="fadesplit" class="black_overlaynew"></div>
		<div id="CreditFade" class="black_overlaynew_Credit"></div>
         <div id="CreditLight"  class="white_contentnew_Credit">
         <div>
            <table>
        <tr> 
        <td>
            <asp:RadioButton ID="rbtnAE" runat="server" GroupName="Credit"/><img src="Images/americanexpress.gif" width="50" />
            <asp:RadioButton ID="rbtnMC" runat="server" GroupName="Credit"/><img src="Images/mastercard.gif" width="50" />
            <asp:RadioButton ID="rbtnME" runat="server" GroupName="Credit" /><img src="Images/maestro.gif" width="50" /> 
            <asp:RadioButton ID="rbtnVI" runat="server"  GroupName="Credit"/><img src="Images/visacard.gif" width="50" />
         </td>
        </tr>
        </table>
        </div>
        <div class="divback">
        <table>
        
        <tr>
        <td>Card Number</td>
        <td>
            <asp:TextBox runat="server" ID="txtCardNumber" MaxLength="16"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
            TargetControlID="txtCardNumber" />
        </td>
        </tr>
        <tr><td>Expiry Date</td>
        <td><asp:DropDownList ID="ddlMonth" runat="server">
        <asp:ListItem>MM</asp:ListItem>
        <asp:ListItem>01</asp:ListItem>
        <asp:ListItem>02</asp:ListItem>
        <asp:ListItem>03</asp:ListItem>
        <asp:ListItem>04</asp:ListItem>
        <asp:ListItem>05</asp:ListItem>
        <asp:ListItem>06</asp:ListItem>
        <asp:ListItem>07</asp:ListItem>
        <asp:ListItem>08</asp:ListItem>
        <asp:ListItem>09</asp:ListItem>
        <asp:ListItem>10</asp:ListItem>
        <asp:ListItem>11</asp:ListItem>
        <asp:ListItem>12</asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="ddlYear" runat="server">
        <asp:ListItem>YYYY</asp:ListItem>
        <asp:ListItem>2014</asp:ListItem>
        <asp:ListItem>2015</asp:ListItem>
        <asp:ListItem>2016</asp:ListItem>
        <asp:ListItem>2017</asp:ListItem>
        <asp:ListItem>2018</asp:ListItem>
        <asp:ListItem>2019</asp:ListItem>
        <asp:ListItem>2020</asp:ListItem>
        <asp:ListItem>2021</asp:ListItem>
        <asp:ListItem>2022</asp:ListItem>
        <asp:ListItem>2023</asp:ListItem>
        <asp:ListItem>2024</asp:ListItem>
        <asp:ListItem>2025</asp:ListItem>
        <asp:ListItem>2026</asp:ListItem>
        <asp:ListItem>2027</asp:ListItem>
        <asp:ListItem>2028</asp:ListItem>
        <asp:ListItem>2029</asp:ListItem>
        <asp:ListItem>2030</asp:ListItem>
        </asp:DropDownList></td>
        </tr>
        <tr>
        <td>CVV</td>
        <td><asp:TextBox ID="txtCVV" MaxLength="3" runat="server" Width="40px"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
            TargetControlID="txtCVV" />
        </td>
        </tr>
        <tr><td>Cardholder's Name</td>
        <td><asp:TextBox runat="server" ID="txtCardholdername"></asp:TextBox></td></tr>
        <tr><td>
            <asp:Button ID="btnPostTransaction" runat="server" CssClass="button" 
                             Text="Save" onclick="btnPostTransaction_Click" OnClientClick="return ValidateCardDetails();"/> 
            <asp:Button ID="btnCancelTransaction" runat="server" CssClass="button" 
                            Text="Cancel" OnClientClick="javascript:CloseWindow();return false;" />
            </td>
            <td>
            <asp:Label ID="lblError" Text="" runat="server" />
            </td></tr>
        </table>
        </div>
		</div>
    </ContentTemplate>
    </asp:UpdatePanel>
    
  </asp:Content> 