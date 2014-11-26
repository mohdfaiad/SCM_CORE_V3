<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingInvoiceCollection.aspx.cs"
    MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.BillingInvoiceCollection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function RefreshParent() {
            if (window.opener != null && !window.opener.closed) {
                window.opener.refresh();
            }
        }
        window.onbeforeunload = RefreshParent;
        function GenerateInvoicesPDF() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>").value;
            var hfInvType = document.getElementById("<%= hfInvoiceType.ClientID %>").value;
            var invArr = hfInvNos.split(',');
            for (var i = 0; i < invArr.length; i++) {
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=' + hfInvType + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=500, height=500");
            }
        }

        function GenerateInvoicesExcel() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>").value;
            var hfInvType = document.getElementById("<%= hfInvoiceType.ClientID %>").value;
            var invArr = hfInvNos.split(',');
            for (var i = 0; i < invArr.length; i++) {
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=' + hfInvType + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=500, height=500");
            }
        }

        function ValidateRemarks() {
            var ddl = document.getElementById("<%= ddlInvoiceType.ClientID %>");
            var InvType = ddl.options[ddl.selectedIndex].value;
            var PendingAmt = document.getElementById("<%= hdPendingAmt.ClientID %>");
            var CollectedAmt = document.getElementById("<%= txtCollectedAmount.ClientID %>");
            var TDSAmt = document.getElementById("<%= txtTDS.ClientID %>");
            var VATAmt = document.getElementById("<%= txtVAT.ClientID %>");
            var ParitalRemarks = document.getElementById("<%= txtPPRemarks.ClientID %>");

            //to restrict under payment for walkin invoices
            if (InvType == "2" && document.getElementById('<%=hdnPartialPP.ClientID%>').value == "N") {
                if (parseFloat(PendingAmt.value) > parseFloat(CollectedAmt.value) + parseFloat(TDSAmt.value) + parseFloat(VATAmt.value)) {
                    alert("Please collect full amount " + PendingAmt.value + " !");
                    return false;
                }
            }
            //to restrict under payment for dest invoices
            if (InvType == "3" && document.getElementById('<%=hdnPartialCC.ClientID%>').value == "N") {
                if (parseFloat(PendingAmt.value) > parseFloat(CollectedAmt.value) + parseFloat(TDSAmt.value) + parseFloat(VATAmt.value)) {
                    alert("Please collect full amount " + PendingAmt.value + " !");
                    return false;
                }
            }
            
            if (ParitalRemarks.value == "") {
                if (PendingAmt.value.charAt(0) != "-") {
                    if (PendingAmt.value != "0") {
                        if (parseFloat(PendingAmt.value) > parseFloat(CollectedAmt.value) + parseFloat(TDSAmt.value) + parseFloat(VATAmt.value)) {
                            alert("Please Enter Partial Payment Remarks !");
                            ParitalRemarks.focus();
                            return false;
                        }
                    }
                }
            }
            
            //to restrict Over payment for walkin invoices
            if (InvType == "2" || InvType == "3") {
                if (parseFloat(PendingAmt.value) < parseFloat(CollectedAmt.value) + parseFloat(TDSAmt.value) + parseFloat(VATAmt.value)) {
                    alert("Amount to be paid is only " + PendingAmt.value + " !");
                    return false;
                }
            }

            if (document.getElementById("<%= txtDepositDate.ClientID %>").value == "") {
                alert("Please enter deposit date");
                return false;
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

    <script language="javascript" type="text/javascript">

        //Function to select/ unselect all rows in Invoice list grid.
        function SelectAllAWB(headerchk) {
            var gvcheck = document.getElementById("<%=grdInvoiceList.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }

        //Function to select/ unselect rows in Invoice list grid.
        function SelectAWB(chk) {
            var grid = document.getElementById("<%=grdInvoiceList.ClientID %>");
            var i;

            //deselect all radiobutton when checkbox is selected
            //            for (i = 0; i < grid.rows.length; i++) {
            //                var rdselect = grid.rows[i].cells[3].children[0];
            //                rdselect.checked = false;
            //            }

            var RowIndex = chk.parentElement.parentElement.rowIndex;

            var CollectedAmt = document.getElementById("<%= txtCollectedAmount.ClientID %>");

            if (CollectedAmt.value == "") {

                CollectedAmt.value = "0"
            }

            var AddAmt = grid.rows[RowIndex].cells[18].children[0]; //PendingAmt column
            var PPRemarks = grid.rows[RowIndex].cells[20].children[0]; //PP remarks
            var TINNum = grid.rows[RowIndex].cells[24].children[0]; //TIN number

            var totalamt;
            if (chk.checked) {
                totalamt = parseFloat(CollectedAmt.value) + parseFloat(AddAmt.innerHTML);
                document.getElementById("<%= txtCollectedAmount.ClientID %>").value = totalamt.toFixed(2);
                document.getElementById("<%= hdPendingAmt.ClientID %>").value = totalamt.toFixed(2);
                document.getElementById("<%= txtPPRemarks.ClientID %>").value = PPRemarks.innerHTML;
                document.getElementById("<%= txtTINNumber.ClientID %>").value = TINNum.innerHTML;
            }
            else {
                totalamt = parseFloat(CollectedAmt.value) - parseFloat(AddAmt.innerHTML);
                document.getElementById("<%= txtCollectedAmount.ClientID %>").value = totalamt.toFixed(2);
                document.getElementById("<%= hdPendingAmt.ClientID %>").value = totalamt.toFixed(2);
                document.getElementById("<%= txtPPRemarks.ClientID %>").value = "";
                document.getElementById("<%= txtTINNumber.ClientID %>").value = "";
            }

            document.getElementById("<%= txtTDS.ClientID %>").value = "0";
            document.getElementById("<%= txtVAT.ClientID %>").value = "0";
            document.getElementById("<%= txt194C.ClientID %>").value = "0";

        }

        function DoPostBackWithRowIndex(rowIndex) {
            if (document.getElementById('<%=HdnSelectedRowIndex.ClientID%>') != null) {
                document.getElementById('<%=HdnSelectedRowIndex.ClientID%>').value = rowIndex;
            }
            return true;
        }

        function DisableTextBox() {

            var sel = document.getElementById("ddlPaymentType");
            if (sel.options[sel.selectedIndex].value == "Cash") {

            }
        }

        function CallPopulateClick() {

            var hfInvNos = document.getElementById("<%= HdnBooking.ClientID %>");

            var InvList = hfInvNos.value;

            if (InvList == "1") {
                //self.close();
            }
        }
        function CreditCardPopup() {

            //Implementing Validation before opening the card pop-up

            var PendingAmt = document.getElementById("<%= hdPendingAmt.ClientID %>");
            var CollectedAmt = document.getElementById("<%= txtCollectedAmount.ClientID %>");
            var ParitalRemarks = document.getElementById("<%= txtPPRemarks.ClientID %>");
            var TDS = document.getElementById("<%= txtTDS.ClientID %>");
            var VAT = document.getElementById("<%= txtVAT.ClientID %>");

            if (CollectedAmt.value == "" || CollectedAmt.value == "0") {
                alert("Please Enter amount to be collected !");
                CollectedAmt.focus();
                return false;
            }

            if (TDS.value == "") {
                TDS.value = "0";
            }

            if (VAT.value == "") {
                VAT.value = "0";
            }

            if (parseFloat(CollectedAmt.value) + parseFloat(TDS.value) + parseFloat(VAT.value) > parseFloat(PendingAmt.value)) {
                alert("Amount to be paid is only " + PendingAmt.value + " !");
                CollectedAmt.focus();
                return false;
            }

            if (ParitalRemarks.value == "") {
                if (PendingAmt.value.charAt(0) != "-") {
                    if (PendingAmt.value != "0") {
                        if (parseFloat(PendingAmt.value) != parseFloat(CollectedAmt.value) + parseFloat(TDS.value) + parseFloat(VAT.value)) {
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

        function ChangedGrdPaymentType(grd) {
            var ibCredit;
            var grid = document.getElementById('<%=grdAWBCollect.ClientID %>');
            var RowIndex = grd.parentElement.parentElement.rowIndex;
            ibCredit = grid.rows[RowIndex].cells[7].children[1];

            if (grid.rows[RowIndex].cells[7].children[0].value == "Card") {
                ibCredit.disabled = false;
            }
            else {
                ibCredit.disabled = true;
            }
        }

        function AWBCollectCreditCardPopup(grd) {
            var grid = document.getElementById('<%=grdAWBCollect.ClientID %>');
            var RowIndex = grd.parentElement.parentElement.rowIndex;

            var CollectedAmt = grid.rows[RowIndex].cells[3].children[0];
            var TDSAmt = grid.rows[RowIndex].cells[4].children[0];
            var VATAmt = grid.rows[RowIndex].cells[5].children[0];
            var Amt194CAmt = grid.rows[RowIndex].cells[6].children[0];

            if ((CollectedAmt.value == "" || CollectedAmt.value == "0") && (TDSAmt.value == "" || TDSAmt.value == "0") && (VATAmt.value == "" || VATAmt.value == "0") && (Amt194CAmt.value == "" || Amt194CAmt.value == "0")) {
                alert("Please Enter amount to be collected !");
                CollectedAmt.focus();
                return false;
            }

            if (CollectedAmt.value == "") {
                CollectedAmt.value = "0";
            }

            if (TDSAmt.value == "") {
                TDSAmt.value = "0";
            }

            if (VATAmt.value == "") {
                VATAmt.value = "0";
            }

            if (Amt194CAmt.value == "") {
                Amt194CAmt.value = "0";
            }

            //Validations complete

            if (document.getElementById('<%=hfAWBCollectRowIndex.ClientID%>') != null) {
                document.getElementById('<%=hfAWBCollectRowIndex.ClientID%>').value = RowIndex;
            }

            document.getElementById('divAWBCollect').style.display = 'block';
            document.getElementById('CreditLight').style.display = 'block';
            document.getElementById('CreditFade').style.display = 'block';

        }


        function UpdateRunningCollectionAmount(grd) {
            var grid = document.getElementById('<%=grdAWBCollect.ClientID %>');
            var CollectedAmt; var TDSAmt; var VATAmt; var RunningCollectionAmt = 0;
            for (i = 1; i < grid.rows.length - 1; i++) {
                if (grid.rows[i].cells[14].children[0].innerHTML == '') {
                    CollectedAmt = grid.rows[i].cells[3].children[0];
                    TDSAmt = grid.rows[i].cells[4].children[0];
                    VATAmt = grid.rows[i].cells[5].children[0];

                    if (CollectedAmt.value == "") {
                        CollectedAmt.value = "0";
                    }

                    if (TDSAmt.value == "") {
                        TDSAmt.value = "0";
                    }

                    if (VATAmt.value == "") {
                        VATAmt.value = "0";
                    }

                    RunningCollectionAmt = parseFloat(RunningCollectionAmt) + parseFloat(CollectedAmt.value) + parseFloat(TDSAmt.value) + parseFloat(VATAmt.value);
                }
            }
            document.getElementById('<%=lblRunningCollectionAmt.ClientID %>').innerHTML = parseFloat(RunningCollectionAmt).toFixed(2);
        }

        function CloseWindow() {
            //clear historical card details
            //            ctl00_ContentPlaceHolder1_rbtnAE.checked == false;
            //            ctl00_ContentPlaceHolder1_rbtnMC.checked == false;
            //            ctl00_ContentPlaceHolder1_rbtnME.checked == false;
            //            ctl00_ContentPlaceHolder1_rbtnVI.checked == false;
            document.getElementById("<%= txtCardNumber.ClientID %>").value = "";
            document.getElementById("<%= txtCVV.ClientID %>").value = "";
            document.getElementById("<%= txtCardholdername.ClientID %>").value = "";
            document.getElementById("<%= lblError.ClientID %>").innerHTML = "";

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
        function ViewPanelAWBCollect() {
            document.getElementById('divAWBCollect').style.display = 'block';
        }
        function HidePanelAWBCollect() {
            document.getElementById('divAWBCollect').style.display = 'none';
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
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 1000px;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: 0.4;
            filter: alpha(opacity=80);
            float: left;
        }
        .white_contentnew
        {
            margin: 0 auto;
            display: none;
            position: absolute; /*fixed*/
            top: 5%;
            left: 2%;
            height: 81%;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
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
        .black_overlaynew_Credit
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
            opacity: 0.4;
            filter: alpha(opacity=80);
            float: left;
        }
        .white_contentnew_Credit
        {
            margin: 0 auto;
            display: none;
            position: fixed;
            top: 5%;
            left: 35%;
            height: 30%;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
   <%-- <asp:UpdatePanel runat="server" ID="updtPnl">
        <ContentTemplate>--%>
            <div id="contentarea">
                <h1>
                    Invoice Collection
                </h1>
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
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
                                Invoice Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlInvoiceType" runat="server" Width="80px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Invoice #
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceNumber" runat="server" Width="90px"></asp:TextBox>
                            </td>
                            <td>
                                Bill Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBillType" runat="server" Width="120px">
                                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                                    <asp:ListItem Text="Fortnightly" Value="Fortnightly"></asp:ListItem>
                                    <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Agent
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAgentCode" runat="server" Width="80px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlAgentCode_SelectedIndexChanged">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:DropDownList ID="ddlAgentName" runat="server" Width="170px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlAgentName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Origin
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrigin" runat="server" Width="90px"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txtOrigin_AutoCompleteExtender" runat="server" ServiceMethod="GetStation"
                                    MinimumPrefixLength="1" Enabled="True" ServicePath="~/Home.aspx" TargetControlID="txtOrigin">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                AWB #
                            </td>
                            <td>
                                <asp:TextBox ID="txtAWBPrefix" runat="server" Width="40px" MaxLength="3"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtAWBPrefix"
                                    WatermarkText="Prefix">
                                </asp:TextBoxWatermarkExtender>
                                &nbsp;
                                <asp:TextBox ID="txtAWBNumber" runat="server" Width="90px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td>
                                Collection
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollection" runat="server">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Partial Collection" Value="Partial Collection"></asp:ListItem>
                                    <asp:ListItem Text="Complete Collection" Value="Complete Collection"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" OnClick="btnList_Click" />
                                &nbsp;
                                <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" OnClick="btnClear_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblOpngBal" runat="server" Text="Opening Balance:" />
                            <asp:Label ID="lblOpeningBalance" runat="server" Text="" />
                        </td>
                    </tr>
                </table>
                <div id="divPrint">
                    <!--<div id="divPrint" style="float:left;">-->
                    <div style="overflow-x: auto; width: 100%">
                        <asp:GridView ID="grdInvoiceList" Width="80%" runat="server" AutoGenerateColumns="False"
                            AllowPaging="True" PageSize="10" OnRowDataBound="grdInvoiceList_RowDataBound"
                            OnPageIndexChanging="grdInvoiceList_PageIndexChanging" ShowFooter="true" OnRowCommand="grdInvoiceList_RowCommand">
                            <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Agent Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%# Eval("SrNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                                    Visible="false">
                                    <%--<HeaderTemplate>
                        <asp:CheckBox ID="chkSelectAllAWB" runat="server" onclick="javascript:SelectAllAWB(this);" />
                    </HeaderTemplate>--%>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" Checked="false" runat="server" onclick="javascript:SelectAWB(this);" />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rbSelect" AutoPostBack="true" Checked="false" runat="server"
                                            OnCheckedChanged="rbSelect_CheckedChanged" />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Agent Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAgentName" runat="server" Text='<%# TruncateString(Eval("AgentName").ToString(), 30) %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGTotalText" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Central Agent" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCentralAgent" runat="server" Text='<%# Eval("CentralAgent") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Local Agent" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocalAgent" runat="server" Text='<%# Eval("LocalAgent") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Number" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Wrap="true" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Eval("InvoiceNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Number" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAWBCollect" runat="server" Text='<%# Eval("InvoiceNumber") %>'
                                            CommandArgument='<%# Eval("InvoiceNumber") %>' CommandName="AWBCollect"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("InvoiceDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Amount" ItemStyle-HorizontalAlign="Center"
                                    FooterStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# Eval("InvoiceAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGTotalInvoiceAmt" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Collected Amount" ItemStyle-HorizontalAlign="Center"
                                    FooterStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCollectedAmount" runat="server" Text='<%# Eval("CollectedAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGTotalCollectedAmt" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTDSAmount" runat="server" Text='<%# Eval("TDSAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="VAT" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVATAmount" runat="server" Text='<%# Eval("VATAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="194C" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right"
                                    HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl194CAmount" runat="server" Text='<%# Eval("Amt194C") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGTotal194CAmt" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payment Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentType" runat="server" Text='<%# Eval("PaymentType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DCM Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDCMAmount" runat="server" Text='<%# Eval("DCMAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DCM Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDCMType" runat="server" Text='<%# Eval("DCMType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cheque#/DD#/ RTGS#" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChequeDdNo" runat="server" Text='<%# Eval("ChequeDdNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cheque Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChequeDate" runat="server" Text='<%# Eval("ChequeDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bank Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBankName" runat="server" Text='<%# Eval("BankName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payment Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentDt" runat="server" Text='<%# Eval("PaymentDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Deposit Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepositDate" runat="server" Text='<%# Eval("DepositDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pending Amount" ItemStyle-HorizontalAlign="Center"
                                    FooterStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPendingAmount" runat="server" Text='<%# Eval("PendingAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGTotalPendingAmt" runat="server" />
                                    </FooterTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Aging" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAging" runat="server" Text='<%# Eval("Aging") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PPRemarks" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPPRemarks" runat="server" Text='<%# Eval("PPRemarks") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("RateLineCurrency") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ORNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblORNumber" runat="server" CommandName="ORNumber" CommandArgument='<%# Eval("ORRecieptNo")%>'
                                            Text='<%# Eval("ORRecieptNo") %>' Enabled='<%# Eval("OREnabled").Equals("Y") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RePrint" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblRePrint" runat="server" CommandName="RePrint" CommandArgument='<%# Eval("ORRecieptNo")%>'
                                            Text="Re-Print"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TIN#" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTINNumber" runat="server" Text='<%# Eval("TINNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Details" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="InvoiceDetails" runat="server" Text="Details" CommandArgument='<%# Eval("InvoiceNumber") %>'
                                            CommandName="Detail"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                            <FooterStyle BackColor="#336699" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                            <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                            <RowStyle CssClass="grdrowfont"></RowStyle>
                        </asp:GridView>
                    </div>
                    <asp:Panel runat="server" ID="pnlAddCollection" GroupingText="Collection" Visible="false">
                        <div>
                            <table width="100%">
                                <tr>
                                    <td>
                                        Collected Amount
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCollectedAmount" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Tax
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTDS" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="LabelVAT" runat="server" Text="VAT"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVAT" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Payment Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPaymentType" runat="server" Width="100px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="btnCreditPopup" runat="server" ImageUrl="~/Images/creditcardicon1.png"
                                            ImageAlign="AbsMiddle" OnClientClick="javascript:CreditCardPopup();return false;"
                                            Enabled="false" />
                                    </td>
                                    <td>
                                        <asp:Label ID="LabelDepositDate" runat="server" Text="Deposit Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDepositDate" runat="server" Width="65px">
                                        </asp:TextBox>
                                        <asp:CalendarExtender ID="CEDepositDate" runat="server" Enabled="True" TargetControlID="txtDepositDate"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Cheque Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChequeDate" runat="server" Width="80px">
                                        </asp:TextBox>
                                        <asp:CalendarExtender ID="CEChequeDate" runat="server" Enabled="True" TargetControlID="txtChequeDate"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        Bank Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBankName" runat="server" Width="170px"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        Cheque/DD#/RTGS#/REF#
                                        <asp:TextBox ID="txtChequeDdNo" runat="server" Width="100px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        TIN#
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTINNumber" runat="server" Width="100px"></asp:TextBox>
                                    </td>
                                    <td>
                                        PPRemarks
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPPRemarks" runat="server" Width="170px"></asp:TextBox>
                                    </td>
                                    <td>
                                        194C
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt194C" runat="server" Width="80px" Text="0"></asp:TextBox>
                                    </td>
                                    <td>
                                        OR Receipt No.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtORNumber" runat="server" Width="110px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click"
                                            Text="Add" OnClientClick="return ValidateRemarks()" />
                                        &nbsp;
                                        <asp:Button ID="btnEdit" runat="server" CssClass="button" OnClick="btnEdit_Click"
                                            Text="Update" />
                                        &nbsp;
                                        <asp:Button ID="btnDelete" runat="server" CssClass="button" OnClick="btnDelete_Click"
                                            Text="Delete" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <br />
                    <asp:Panel runat="server" ID="pnlDCMAdjustment" GroupingText="DCM Adjustment" Visible="false">
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
                                        <asp:DropDownList ID="ddlDCMType" runat="server" Width="100px">
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
                                    <td>
                                        <asp:Button ID="btnSaveDCM" runat="server" CssClass="button" Text="Save" OnClick="btnSaveDCM_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlchargeswaveoff" GroupingText="Charges Wave-Off"
                        Visible="false">
                        <div>
                            <table width=" 95%">
                                <tr>
                                    <td>
                                        Invoice Number
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTInvoiceNo" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Amount
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTWAmount" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Issued By.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTIssueBy" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Reason
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTWreason" runat="server" Width="150px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSaveWaveOFF" runat="server" CssClass="button" Text="Save" OnClick="btnSaveWaveOFF_Click" />
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
                                <asp:Button ID="btnExport" Visible="false" runat="server" Text="Export" CssClass="button"
                                    OnClick="btnExport_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnPosttoTreasury" Visible="false" runat="server" Text="Post To Treasury"
                                    CssClass="button" OnClick="btnPosttoTreasury_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnPrintPDF" runat="server" CssClass="button" Text="Print PDF" OnClick="btnPrintPDF_Click" />
                            </td>
                            <td>
                                <asp:HiddenField ID="HdnSelectedRowIndex" runat="server" Value="" />
                            </td>
                            <td>
                                <asp:HiddenField ID="HdnBooking" runat="server" Value="" />
                            </td>
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
            <asp:HiddenField ID="hfInvoiceNos" runat="server" />
            <asp:HiddenField ID="hfInvoiceType" runat="server" />
            <asp:HiddenField ID="hdTransactionId" runat="server" />
            <div id="Lightsplit" class="white_contentnew" style="overflow: auto;">
                <div>
                    <h3 style="text-align: center;">
                        AWB Details</h3>
                    <asp:GridView ID="grdInvoiceDetails" runat="server" AutoGenerateColumns="false" HeaderStyle-Wrap="false">
                        <Columns>
                            <asp:TemplateField HeaderText="AWB No">
                                <ItemTemplate>
                                    <%--        <asp:Label ID="lblAWBPrefix" runat="server" Text='<%# Eval("AWBPrefix") %>'></asp:Label>
        <asp:Label ID="Label1" runat="server" Text="-"></asp:Label>--%>
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
                            <asp:TemplateField HeaderText="Freight">
                                <ItemTemplate>
                                    <asp:Label ID="lblFreightPP" runat="server" Text='<%# Eval("PPFreight") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="OCDC">
                                <ItemTemplate>
                                    <asp:Label ID="lblOCDCPP" runat="server" Text='<%# Eval("PPOCDC") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="OCDA">
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
                            <asp:TemplateField HeaderText="Spot Rate" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSpotRate" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Spot Freight" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSpotFreight" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle Wrap="false" />
                        <EditRowStyle CssClass="grdrowfont" />
                        <FooterStyle CssClass="grdrowfont" />
                        <HeaderStyle CssClass="titlecolr" Wrap="False" />
                        <RowStyle CssClass="grdrowfont" HorizontalAlign="Center" Wrap="False" />
                    </asp:GridView>
                    <h3 style="text-align: center;">
                        Charges Summary</h3>
                    <div>
                        <table border="1px" align="center">
                            <tr>
                                <td align="right">
                                    Total Charges Due Airline :
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="txtTotalChargesDueAirline" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Total Base Amount for Service Tax :
                                </td>
                                <td>
                                    <asp:Label ID="txtBaseAmtServiceTax" runat="server"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Total Service Tax Due Airline :
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="txtTaxDueAirline" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    TDS On Commission :
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="txtTDSOnComm" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Commissionable Sales :
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="txtCommSales" runat="server"></asp:Label>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Agent&#39;s Commission :
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="txtAgentComm" runat="server"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Other Charges Due Agent :
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="txtOCDA" runat="server"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Service Tax On Commission :
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="txtSTOnComm" runat="server"></asp:Label>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Total Deductions :
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="txtTotalDeductions" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    NET DUE AIRLINES/AGENT :
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="txtNetCharges" runat="server" ReadOnly="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <table align="center">
                    <tr>
                        <td>
                            <asp:Button ID="btnClose" runat="server" OnClientClick="javascript:HidePanelSplit()"
                                CssClass="button" Text="Close" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divAWBCollect" class="white_contentnew" style="overflow: auto;">
                <asp:Label ID="lblCollectAWBStatus" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblAWBCollectInvoiceNumber" Font-Bold="true" Font-Size="Larger" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblAWBCollectCurrency" Font-Bold="true" Font-Size="Larger" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblRunningCollection" Text="Running collection amount: " Font-Bold="true"
                    Font-Size="Larger" runat="server"></asp:Label>
                <asp:Label ID="lblRunningCollectionAmt" Font-Size="Larger" runat="server"></asp:Label>
                <div>
                    <asp:GridView ID="grdAWBCollect" runat="server" AutoGenerateColumns="false" HeaderStyle-Wrap="false"
                        OnRowCommand="grdAWBCollect_RowCommand" OnRowDataBound="grdAWBCollect_RowDataBound"
                        ShowFooter="true">
                        <Columns>
                            <asp:TemplateField HeaderText="SrNo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSrNo" runat="server" Text='<%# Eval("SrNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AWB #">
                                <ItemTemplate>
                                    <asp:Label ID="lblAWBNo" runat="server" Text='<%# Eval("AWBNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agent</br>Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount</br>Due">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("TotalAWBCharges") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblAWBCollectTotal" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Collected</Br>Amt" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCollectedAmount" runat="server" Text='<%# Eval("CollectedAmount") %>'
                                        Width="60px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Collected</Br>Amt">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCollectedAmount" runat="server" Text='<%# Eval("CollectedAmount") %>'
                                        Width="60px"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblAWBCollectCollectedAmount" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TDS</br>Amt" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblTDSAmount" runat="server" Text='<%# Eval("TDSAmount") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TDS</br>Amt">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTDSAmount" runat="server" Text='<%# Eval("TDSAmount") %>' Width="60px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VAT</br>Amt" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblVATAmount" runat="server" Text='<%# Eval("VATAmount") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VAT</br>Amt">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtVATAmount" runat="server" Text='<%# Eval("VATAmount") %>' Width="60px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="194C</br>Amt" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl194CAmount" runat="server" Text='<%# Eval("Amt194C") %>' Width="60px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="194C</br>Amt">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt194CAmount" runat="server" Text='<%# Eval("Amt194C") %>' Width="30px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment</br>Type">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPaymentType" DataSource="<%# fillgrdAWBCollectPaymentType() %>"
                                        DataTextField="PaymentType" DataValueField="PaymentType" runat="server" Width="90px"
                                        onclick="javascript:ChangedGrdPaymentType(this)">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="btnAWBCollectCreditPopup" runat="server" ImageUrl="~/Images/creditcardicon1.png"
                                        ImageAlign="AbsMiddle" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DCM Amt">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDCMAmount" runat="server" Text='<%# Eval("DCMAmount") %>' Width="60px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DCM</br>Type" Visible="false">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlDCMType" runat="server" Width="60px">
                                        <%--        <asp:ListItem Text="Debit" Value="1"></asp:ListItem>
        <asp:ListItem Text="Credit" Value="2"></asp:ListItem>--%>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cheque/DD/</br>RTGS/Card #">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtChequeDDNo" runat="server" Text='<%# Eval("ChequeDdNo") %>' Width="120px"
                                        MaxLength="30"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cheque</br>Date">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtChequeDate" runat="server" Text='<%# Eval("ChequeDate") %>' Width="70px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtChequeDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtChequeDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bank Name">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtBankName" runat="server" Text='<%# Eval("BankName") %>' Width="140px"
                                        MaxLength="30"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment</br>Date" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPaymentDate" runat="server" Text='<%# Eval("PaymentDate") %>'
                                        Width="70px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtPaymentDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtPaymentDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Deposit</br>Date">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDepositDate" runat="server" Text='<%# Eval("DepositDate") %>'
                                        Width="70px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtDepositDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtDepositDate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pending</br>Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblPendingAmount" runat="server" Text='<%# Eval("PendingAmount") %>'
                                        Width="60px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblAWBCollectPendingAmount" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" Text='<%# Eval("Remarks") %>'
                                        Width="100px" MaxLength="200"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ORNumber" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblORNumber" runat="server" Text='<%# Eval("ORRecieptNo") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True"></HeaderStyle>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnAddNewRow" runat="server" CommandName="AddNewRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        Text="Add" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle Wrap="false" />
                        <EditRowStyle CssClass="grdrowfont" />
                        <FooterStyle BackColor="#336699" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="titlecolr" Wrap="False" />
                        <RowStyle CssClass="grdrowfont" HorizontalAlign="Center" Wrap="False" />
                    </asp:GridView>
                </div>
                <table align="center">
                    <tr>
                        <td>
                            <asp:Button ID="btnSaveAWBCollect" OnClick="btnSaveAWBCollect_Click" runat="server"
                                CssClass="button" Text="Save" />
                        </td>
                        <td>
                            <asp:Button ID="btnCancelAWBCollect" runat="server" OnClientClick="javascript:HidePanelAWBCollect()"
                                CssClass="button" Text="Close" />
                        </td>
                        <td>
                            <asp:HiddenField ID="hfAWBCollectRowIndex" runat="server" Value="" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="fadesplit" class="black_overlaynew">
            </div>
            <div id="CreditFade" class="black_overlaynew_Credit">
            </div>
            <div id="CreditLight" class="white_contentnew_Credit">
                <div>
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="rbtnAE" runat="server" GroupName="Credit" /><img src="Images/americanexpress.gif"
                                    width="50" />
                                <asp:RadioButton ID="rbtnMC" runat="server" GroupName="Credit" /><img src="Images/mastercard.gif"
                                    width="50" />
                                <asp:RadioButton ID="rbtnME" runat="server" GroupName="Credit" /><img src="Images/maestro.gif"
                                    width="50" />
                                <asp:RadioButton ID="rbtnVI" runat="server" GroupName="Credit" /><img src="Images/visacard.gif"
                                    width="50" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="divback">
                    <table>
                        <tr>
                            <td>
                                Card Number
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCardNumber" MaxLength="16"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                    TargetControlID="txtCardNumber" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Expiry Date
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMonth" runat="server">
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
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                CVV
                            </td>
                            <td>
                                <asp:TextBox ID="txtCVV" MaxLength="3" runat="server" Width="40px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                    TargetControlID="txtCVV" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Cardholder's Name
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtCardholdername"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnPostTransaction" runat="server" CssClass="button" Text="Save"
                                    OnClick="btnPostTransaction_Click" OnClientClick="return ValidateCardDetails();" />
                                <asp:Button ID="btnCancelTransaction" runat="server" CssClass="button" Text="Cancel"
                                    OnClientClick="javascript:CloseWindow();return false;" />
                            </td>
                            <td>
                                <asp:Label ID="lblError" Text="" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hdnSerialIds" runat="server" />
                <asp:HiddenField ID="hdnPartialPP" runat="server" />
                <asp:HiddenField ID="hdnPartialCC" runat="server" />
            </div>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
