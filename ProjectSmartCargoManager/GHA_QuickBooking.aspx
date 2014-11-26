<%--

 2012-04-05  vinayak
 2012-05-05  vinayak
 2012-05-23  vinayak
 2012-06-04  vinayak
 2012-06-05  vinayak
 2012-06-12  vinayak
 2012-06-18  vinayak
 2012-06-25  vinayak
 2012-07-06  vinayak Edit/View
 2012-07-09  vinayak Special Commodity
 2012-07-23  vinayak Special Commodity with three charge types
 2012-07-24  vinayak
 2012-07-25  vinayak
 2012-07-30  vinayak
 2012-08-03  vinayak

--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="GHA_QuickBooking.aspx.cs"
    Inherits="ProjectSmartCargoManager.GHA_QuickBooking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register TagPrefix="DateControl" TagName="CustomDateControl" Src="~/CustomControls/DateControl.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">

        function usernameonly(e, decimal) {
            var key;
            var keychar;

            if (window.event) {
               key = window.event.keyCode;
            }
            else if (e) {
               key = e.which;
            }
            else {
               return true;
            }
            keychar = String.fromCharCode(key);

            if ((key==null) || (key==0) || (key==8) ||  (key==9) || (key==13) || (key==27) ) {
               return true;
            }
            else if ((("!@#$%^&*()_+-=?/.><,';:`~").indexOf(keychar) > -1)) {
               return false;
            }
            //else
            //   return false;
        }

        function ValidationsOfSaveAccept(mybutton) {
            var strValue = mybutton.value;
            
            var grosswt = document.getElementById("<%= txtGrossWt.ClientID %>").value;
            var pcscount = document.getElementById("<%= txtPieces.ClientID %>").value;
            var AgentCode = document.getElementById("<%= TXTAgentCode.ClientID %>").value;
            var CoomodityCode = document.getElementById("<%= txtCommodityCode.ClientID %>").value;
            var Destination = document.getElementById('<%= ddlDest.ClientID %>');
            Destination = Destination.options[Destination.selectedIndex].text;

            if (Destination == "Select") {
                alert('Destination code is mandatory.');
                callclose();
                document.getElementById("<%= ddlDest.ClientID %>").focus();
                return false;
            }
            
            if (CoomodityCode == "") {
                alert('Commodity code is mandatory.');
                callclose();
                document.getElementById("<%= txtCommodityCode.ClientID %>").focus();
                return false;
            }
            
            if (AgentCode == "") {
                alert('Agent code is mandatory.');
                callclose();
                document.getElementById("<%= TXTAgentCode.ClientID %>").focus();
                return false;
            }

            if (pcscount == "" || pcscount == "0") {
                alert('Please enter shipment pieces.');
                callclose();
                document.getElementById("<%= txtPieces.ClientID %>").focus();
                return false;
            }

            if (grosswt == "" || grosswt == "0") {
                alert('Please enter shipment weight.');
                callclose();
                document.getElementById("<%= txtGrossWt.ClientID %>").focus();
                return false;
            }            
            
            //Validation for Shipper Details
            var shippercode = document.getElementById('ctl00_ContentPlaceHolder1_TXTShipper').value;
            var shipperTel = document.getElementById('ctl00_ContentPlaceHolder1_TXTShipTelephone').value;
            var shipperAddr = document.getElementById('ctl00_ContentPlaceHolder1_TXTShipAddress').value;
            var e = document.getElementById('ctl00_ContentPlaceHolder1_ddlShipCountry');
            var country = e.options[e.selectedIndex].value;

            if (shippercode == "" || shipperTel == "" || shipperAddr == "" || country == "") {
                callclose();
                alert("Please enter Shipper details.");
                return false;
            }
            
            //Validation for Consignee Details
            var Consigneecode = document.getElementById('ctl00_ContentPlaceHolder1_TXTConsignee').value;
            var ConsigneeTel = document.getElementById('ctl00_ContentPlaceHolder1_TXTConTelephone').value;
            var ConsigneeAddr = document.getElementById('ctl00_ContentPlaceHolder1_TXTConAddress').value;
            var e = document.getElementById('ctl00_ContentPlaceHolder1_ddlConCountry');
            var country = e.options[e.selectedIndex].value;

            if (Consigneecode == "" || ConsigneeTel == "" || ConsigneeAddr == "" || country == "") {
                callclose();
                alert("Please enter Consignee details.");
                return false;
            }
            
            //Validation for Payment Mode
            var e = document.getElementById('ctl00_ContentPlaceHolder1_ddlPaymentMode');
            var PaymentType = e.options[e.selectedIndex].text;
            
            if (PaymentType == "Select") {
                callclose();
                alert("Please enter Payment Mode.");
                e.focus();
                return false;
            }
            
            //Validation for Shipment Status
            e = document.getElementById('ctl00_ContentPlaceHolder1_ddlAWBStatus');
            var Status = e.options[e.selectedIndex].value;
             
            if (Status != "C") {
                alert('Shippment is not Confirmed.');
                callclose();
                e.focus();
                return false;
            }
            
            return true;
        }
        
        //Validate Accepted Pieces with Booking Pieces

        function popupUCR() {
            var AWBNumber = document.getElementById('ctl00_ContentPlaceHolder1_txtAwbPrefix').value + document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;
            var UCRNo = document.getElementById("<%= hdUCRNo.ClientID %>").value;
            if (UCRNo == "")
            { UCRNo = "NO"; }
            AWBNumber = AWBNumber.replace('-', '');
            window.open('UCRPopup.aspx?Type=New' + '&Mode=A' + '&AWBNo=' + AWBNumber + '&pg=Acc&UCRNo=' + UCRNo, '', 'left=0,top=0,width=1000,height=1000,toolbar=0,resizable=0');
        }
        
       function SetOriginDestination() {
            var HidOrigin = document.getElementById('<%= HidOrigin.ClientID%>');
            var HidDestination = document.getElementById('<%= HidDestination.ClientID%>');
            
            var DropDownOrigin = document.getElementById('<%= ddlOrg.ClientID %>');
            var DropDownDestination = document.getElementById('<%= ddlDest.ClientID %>');
            
//            alert(DropDownOrigin.options[DropDownOrigin.selectedIndex].text);
//            alert(DropDownDestination.options[DropDownDestination.selectedIndex].text);
            
            HidOrigin.value = DropDownOrigin.options[DropDownOrigin.selectedIndex].text;
            HidDestination.value = DropDownDestination.options[DropDownDestination.selectedIndex].text;
            
            return true;
        }
        
        function ValidateIACandCCSF() {

            var IACandCCSF = document.getElementById("<%= HidCCSFFlag.ClientID %>");
            var IACandCCSFValue = IACandCCSF.value;
            
            if(IACandCCSFValue=="0")
                return false;
            else
                {
                    callShow();
                    __doPostBack('TXTAgentCode', '');
                }
            return true;
        }
        
        function FlightCheckinTime() {
            var x=document.getElementById("<%= HidCheckin.ClientID %>").value;
            
            if(x!="")
            {
                if (confirm(x + "\nDo you want to continue (Y/N)?") == true) {
                    document.getElementById("<%= HidCheckin.ClientID %>").value="";
                    callShow(); 
                    return true;
                } else {
                    return false;
                }
            }
            else
            return true;
        }

        function CheckAcceptedPieces(chk) {
            var tbPcs;
            var tbWt;
            var grid = document.getElementById('<%=grdRouting.ClientID %>');
            //var RowIndex = chk.parentElement.parentElement.rowIndex;

            for (var RowIndex = 0; RowIndex < grid.rows.length; RowIndex++) {
                tbPcs = grid.rows[RowIndex].cells[14].children[0];
                tbWt = grid.rows[RowIndex].cells[15].children[0];
                grid.rows[RowIndex].cells[14].children[0].value = 0;
                grid.rows[RowIndex].cells[15].children[0].value = 0;
                tbPcs.disabled = false;
                tbWt.disabled = false;
            }
            return false;
        }

        //End

        //Label Printing...
        function PrintLabels() {


            var AWBNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAwbPrefix').value + document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;
            var Origin = document.getElementById('ctl00_ContentPlaceHolder1_ddlOrg').value;
            var Dest = document.getElementById('ctl00_ContentPlaceHolder1_ddlDest').value;
            //var Dest = document.getElementById('HidDest').value;
            var Pieces = document.getElementById('ctl00_ContentPlaceHolder1_GRDRates_ctl02_TXTPcs').value;
            var FromPcs = "1";

            window.open('LabelPrinting.htm?param=' + Origin + ';' + Dest + ';' + Pieces + ';' + AWBNo + ';' + FromPcs + ';' + Pieces + ';' + 'BK;0;0;0;0;', 'Print',
                         'left=0,top=0,width=260,height=220,toolbar=0,resizable=0'); //Working

            //            window.open('LabelPrinting.htm?param=' + Origin + ';' + Dest + ';' + Pieces + ';' + AWBNo + ';' + FromPcs + ';' + Pieces + ';' + 'XR;Poorna;Loc;Ship;Consig;', 'Print',
            //                         'left=0,top=0,width=260,height=220,toolbar=0,resizable=0'); //Working for Xray printing

            return false;

        }

        //Print Selected Labels
        function PrintSelLabels() {


            var AWBNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAwbPrefix').value + document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;
            var Origin = document.getElementById('ctl00_ContentPlaceHolder1_ddlOrg').value;
            var Dest = document.getElementById('ctl00_ContentPlaceHolder1_ddlDest').value;
            //var Dest = document.getElementById('HidDest').value;
            var Pieces = document.getElementById('ctl00_ContentPlaceHolder1_GRDRates_ctl02_TXTPcs').value;
            var FromPcs = document.getElementById("<%= txtPrnLblFrom.ClientID%>").value;
            var ToPcs = document.getElementById("<%= txtPrnLblTo.ClientID%>").value;

            window.open('LabelPrinting.htm?param=' + Origin + ';' + Dest + ';' + Pieces + ';' + AWBNo + ';' + FromPcs + ';' + ToPcs + ';' + 'BK;0;0;0;0;', 'Print',
                         'left=0,top=0,width=260,height=220,toolbar=0,resizable=0'); //Working

            //            window.open('LabelPrinting.htm?param=' + Origin + ';' + Dest + ';' + Pieces + ';' + AWBNo + ';' + FromPcs + ';' + Pieces + ';' + 'XR;Poorna;Loc;Ship;Consig;', 'Print',
            //                         'left=0,top=0,width=260,height=220,toolbar=0,resizable=0'); //Working for Xray printing

            return false;

        }

        function popup(unid, org, dest, awb, pcs, totpcs, frmpcs, toPcs) {

            //            var unid = document.getElementById("<%= HdnUNIDForDGRLbl.ClientID %>").value;
            //            alert(unid);
            //            var org = document.getElementById("<%= ddlOrg.ClientID %>").value;
            //            var dest = document.getElementById("<%= ddlDest.ClientID %>").value;
            //            var awb = document.getElementById("<%= txtAwbPrefix.ClientID %>").value + document.getElementById("<%= txtAWBNo.ClientID %>").value
            //        var pcs=document.getElementById("<%= HdnPcsForDGRLbl.ClientID %>").value;

            window.open('DGRLabel.aspx?UNID=' + unid + '&AWB=' + awb + '&org=' + org + '&dest=' + dest + '&TotPcs=' + totpcs + '&Frm=' + frmpcs + '&To=' + toPcs, '', 'left=0,top=0,width=1000,height=600,toolbar=0,resizable=0');
        }

        function DGRAlert() {
            alert('UNID Details not found.');
            return false;
        }

        //    <script language="javascript" type="text/javascript">
        function Error() {



        }

        function GenerateInvoices() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            //alert(hfInvNos.value);
            var InvList = hfInvNos.value;

            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=WalkIn' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowWalkInAgentInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }
//Vijay Changes integrated by jayant on 30/06/2014
        function WalkInCollection() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;

            var AWBPrefix = document.getElementById('<%= txtAwbPrefix.ClientID %>').value;
            
            //alert(AWBPrefix);

            var AWBNo = document.getElementById('<%= HidAWBNumber.ClientID %>').value;

            var Origin = document.getElementById('<%= HidSource.ClientID %>').value;
            
            //Exit Parameter to show Exit Button and Hide "Post To Treasury"
            //window.open("BillingWalkInAgentCollection.aspx?INVNO=" + InvList + '&Type=WalkIn' + '&AWBPrefix=' + AWBPrefix + '&AWBNo=' + AWBNo + '&Origin=' + Origin+'&Exit=true', "", "status=0,toolbar=0, menubar=0, width=1100, height=650");
            window.open("BillingInvoiceCollection.aspx?INVNO=" + InvList + '&Type=WalkIn' + '&AWBPrefix=' + AWBPrefix + '&AWBNo=' + AWBNo + '&Origin=' + Origin + '&Exit=true', "", "status=0,toolbar=0, menubar=0, width=1100, height=650, scrollbars=yes, top=0, left=95");
            return false;
        }


        function GetDocumentList() {
            var TxtClientObject = '<%= txtAttchDoc.ClientID %>';
            var TxtClientValue = document.getElementById('ctl00_ContentPlaceHolder1_txtAttchDoc').value;

            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=Document&TargetTXT=' + TxtClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');

        }
        function GetSpecialHandlingCode() {

            var TxtClientObject = '<%= txtSpecialHandlingCode.ClientID %>';
            var TxtClientValue = document.getElementById('ctl00_ContentPlaceHolder1_txtSpecialHandlingCode').value;

            window.open('ListMultipleSpecialHandlingCode.aspx?TargetTXT=' + TxtClientObject + '&txtValue=' + TxtClientValue, '',
            'left=' + (screen.availWidth / 5) + ',top=' + (screen.availHeight / 8) + 
            ',width=800,height=550,toolbar=0,resizable=0');
        }

        function GetPackagingChargeCodes() {

            var TxtClientObject = '<%= txtPackingInfo.ClientID %>';
            var TxtClientValue = document.getElementById('ctl00_ContentPlaceHolder1_txtPackingInfo').value;
            var TxtPieces = document.getElementById('ctl00_ContentPlaceHolder1_txtPieces');

            if (TxtPieces.value == "" || TxtPieces.value == "0") {
                alert("Please enter the pieces");
                TxtPieces.focus();
                return false;
            }

            window.open('ListMultiplePackagingCharges.aspx?TargetTXT=' + TxtClientObject + '&txtValue=' + TxtClientValue + '&txtPieces=' + TxtPieces.value, '',
            'left=' + (screen.availWidth / 5) + ',top=' + (screen.availHeight / 8) +
            ',width=800,height=550,toolbar=0,resizable=0');
        }

        function onListPopulated() {

            var completionList = $find("AutoCompleteEx").get_completionList();
            completionList.style.width = 'auto';
        }

        function CheckDVValue() {

            var DVCarriage = document.getElementById('<%= TXTDvForCustoms.ClientID %>').value;
            var DVCustom = document.getElementById('<%= TXTDvForCarriage.ClientID %>').value;
            var SumChrgblWt = document.getElementById('<%= txtChargeableWt.ClientID %>').value;
            var SHC = document.getElementById('<%= txtSpecialHandlingCode.ClientID %>').value;
            if (SumChrgblWt < 1) {
                SumChrgblWt = document.getElementById('<%= txtGrossWt.ClientID %>').value;
            }
            if ((DVCarriage / SumChrgblWt) > 199)
            {
                if (SHC.indexOf("VAL") < 0)
                {
                    SHC = "VAL," + SHC;
                }
            }
            else
            {
                if (SHC.indexOf("VAL") >-1) {
                    SHC = SHC.replace("VAL", "");                    
                }
            }
                document.getElementById('<%= txtSpecialHandlingCode.ClientID %>').value = SHC;
            
        }


    </script>

    <script type="text/javascript">
        function ViewPanel() {
            document.getElementById('light').style.display = 'block';
            document.getElementById('fade').style.display = 'block';
        }
        function HidePanel() {
            document.getElementById('light').style.display = 'none';
            document.getElementById('fade').style.display = 'none';
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

    <script type="text/javascript">

        function callexportULD() {
            window.open('frmULDToAWBAssoc.aspx?ULD=B', '', 'left=0,top=0,width=800,height=600,toolbar=0');
        }

        function onListPopulated() {

            var completionList = $find("ACEAgentCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function onCommListPopulated() {

            var completionList = $find("ACECommCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function onShipperListPopulated() {

            var completionList = $find("ACESHPCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function onConsigneeListPopulated() {

            var completionList = $find("ACEConCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function onDriverListPopulated() {

            var completionList = $find("ACEDriverName").get_completionList();
            completionList.style.width = 'auto';
        }

        function onIACPopulated() {

            var completionList = $find("ACEIACCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function onShipperPopulated() {

            var completionList = $find("ACEShipperCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function onAWBPrefixPopulated() {
            var completionList = $find("ACEAwbPrefix").get_completionList();
            completionList.style.width = 'auto';
        }

        function callShow1() {
            document.getElementById('blackening').style.display = 'block';
            document.getElementById('whitening').style.display = 'block';
            document.getElementById('gvHAWBDetails').style.visibility = "visible";
        }
        function callclose1() {
            document.getElementById('blackening').style.display = 'none';
            document.getElementById('whitening').style.display = 'none';
        }

        function ValidateHAWBWt() {
            alert("The Given Weight Exceeds Main AWB Total Weight !!");
        }

        function ValidateHAWBPcs() {
            alert("The Given No. of Pieces Exceeds Main AWB Total No. of Pieces !!");
        }    
        
              function DownloadHAWB() {
           window.open('Download.aspx?Mode=HAWB', 'Download', 'left=100,top=100,width=800,height=420,toolbar=0,resizable=1');
            callShow1();
            
        }
    </script>

    <%----Swapnil For BrowserBack space disable--%>

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callexport1() {
            window.open('DGRCargo.aspx', '', 'left=200,top=200,width=900,height=300,toolbar=0,resizable=0,scrollbars=1');
        }
        function callexport() {
            window.open('ShowEAWB.aspx', 'Send');
        }
        function sendFFR() {
            window.open('SendFFR.aspx', 'Send', 'left=0,top=0,width=700,height=500,toolbar=0,resizable=0');
        }
        
        function dimension(mybutton) {
            var Origin = document.getElementById('ctl00_ContentPlaceHolder1_ddlOrg').value;            
            var Dest = document.getElementById('ctl00_ContentPlaceHolder1_ddlDest').value;
            
            var strValue = mybutton.value;
            var DropDownList = document.getElementById("<%= txtCommodityCode.ClientID %>");
            var strValue = DropDownList.value;
            var strAwbNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;
            var strAgentCode = document.getElementById("<%= TXTAgentCode.ClientID %>").value;
                        
            var ExecDt = document.getElementById('ctl00_ContentPlaceHolder1_txtExecutionDate1_txtDate').value;

            var shipper = document.getElementById("<%= txtShipperCode.ClientID %>").value;
            var Consignee = document.getElementById("<%= txtConsigneeCode.ClientID %>").value;

            var productList = document.getElementById("<%= ddlProductType.ClientID %>");
            var ProductType = productList.options[productList.selectedIndex].text; ;            
            
            var TxtClientObjectID = 'ctl00_ContentPlaceHolder1_txtChargeableWt';
            var TxtClientObjectVolID = 'ctl00_ContentPlaceHolder1_txtVolume';
            var grosswt = document.getElementById("<%= txtGrossWt.ClientID %>").value;
            var pcscount = document.getElementById("<%= txtPieces.ClientID %>").value;

            if (pcscount == "" || pcscount == "0") {

                alert('Fill Pcs first.');
                return;
            }
            if (grosswt == "" || grosswt == "0") {

                alert('Fill Gross Wt.');
                return;
            }
            setFocus();
            var rowindex = 1;
            var sessionValue = '<%=Session["FocusShipper"]%>'
            if (sessionValue == true) {
                document.getElementById("<%= txtShipperCode.ClientID %>").focus();
            }
            var HidObject = '<%= HidDimension.ClientID %>';

            window.open('GHA_Dimensions.aspx?awbno=' + strAwbNo + '&commodity=' + strValue + '&TargetTXT=' + TxtClientObjectID + '&Hid=' + HidObject + '&GrossWt=' + grosswt + '&PcsCount=' + pcscount + '&RowIndex=' + rowindex + '&VolumeTXT=' + TxtClientObjectVolID + '&Mode=B' + '&Origin=' + Origin + '&Destination=' + Dest + '&AgtCode=' + strAgentCode + '&Shipper=' + shipper + '&Consignee=' + Consignee + '&Product=' + ProductType + '&ExecDt=' + ExecDt, '', 'left=50,top=50,width=850,height=560,toolbar=0,resizable=0,scrollbars=yes');
       
            return false;

        }

        function setFocus() {
            document.getElementById("<%= txtShipperCode.ClientID %>").focus();
        }

        function FlightDetailDimension(control) {

            //var mybutton = 'ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_btnDimensionsPopup';

            var DropDownList = document.getElementById('ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_ddlMaterialCommCode');

            var strValue = DropDownList.value;

            var strAwbNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;

            var TxtClientObjectID = document.getElementById('ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_txtCommChargedWt').id;
            //smybutton.id.replace("btnDimensionsPopup", "txtCommChargedWt");

            var TxtClientObjectVolID = document.getElementById('ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_txtCommVolWt').id;
            //mybutton.id.replace("btnDimensionsPopup", "txtCommVolWt");

            // var TxtClientObject = 'ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_txtCommChargedWt';
            var grosswt = document.getElementById('ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_txtCommGrossWt').value;
            //document.getElementById(mybutton.id.replace("btnDimensionsPopup", "txtCommGrossWt")).value;


            var pcscount = document.getElementById('ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_TXTPcs').value;
            //document.getElementById(mybutton.id.replace("btnDimensionsPopup", "TXTPcs")).value;

            var FlightNo = document.getElementById(control.id.replace("btnRoutePieces", "ddlFltNum")).value;

            var FlightDate = document.getElementById(control.id.replace("btnRoutePieces", "txtFdate")).value;

            if (pcscount == "" || pcscount == "0") {

                alert('Fill Pcs first.');
                return;
            }

            var rowindex = document.getElementById('ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_HidRowIndex').value;
            //document.getElementById(mybutton.id.replace("btnDimensionsPopup", "HidRowIndex")).value;

            var HidObject = '<%= HidDimension.ClientID %>';

            window.open('GHA_Dimensions.aspx?awbno=' + strAwbNo + '&commodity=' + strValue + '&TargetTXT=' + TxtClientObjectID + '&Hid=' + HidObject + '&GrossWt=' + grosswt + '&PcsCount=' + pcscount + '&RowIndex=' + rowindex + '&VolumeTXT=' + TxtClientObjectVolID + '&FltNo=' + FlightNo + '&FltDate=' + FlightDate + '&Mode=B', '', 'left=50,top=50,width=810,height=430,toolbar=0,resizable=0,scrollbars=yes');

            return false;

        }


        function AWBPieces(mybutton) {

            //    code for getting data

            var strValue = mybutton.value;

            //         var i;

            var targetcontrol = mybutton.id.replace("btnPiecesPopup", "ddlMaterialCommCode");

            var DropDownList = document.getElementById(targetcontrol);

            var strValue = DropDownList.value;
            var strAwbNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;


            var TxtClientObjectID = mybutton.id.replace("btnPiecesPopup", "txtCommChargedWt");
            var TxtClientObjectVolID = mybutton.id.replace("btnPiecesPopup", "txtCommGrossWt");

            // var TxtClientObject = 'ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_txtCommChargedWt';
            var grosswt = document.getElementById(mybutton.id.replace("btnPiecesPopup", "txtCommGrossWt")).value;


            var pcscount = document.getElementById(mybutton.id.replace("btnPiecesPopup", "TXTPcs")).value;

            if (pcscount == "" || pcscount == "0") {

                alert('Please fill Piece count to proceed.');
                return;
            }

            if (grosswt == "" || grosswt == "0") {

                alert('Please fill Gross Wt. to proceed.');
                return;
            }

            var rowindex = document.getElementById(mybutton.id.replace("btnPiecesPopup", "HidPcsRowIndex")).value;

            var HidObject = '<%= HidAWBPieces.ClientID %>';

            window.open('AWBPiecesInfo.aspx?awbno=' + strAwbNo + '&TargetTXT=' + TxtClientObjectID + '&Hid=' + HidObject + '&GrossWt=' + grosswt + '&PcsCount=' + pcscount + '&Route=Piece&RowIndex=' + rowindex, '', 'left=0,top=0,width=600,height=450,toolbar=0,resizable=0');

            return false;

        }

        function RoutePieces(mybutton) {
            //btnRoutePieces
            var strValue = mybutton.value;
            var row = mybutton.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;

            var strAwbNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;


            var TxtClientObjectID = mybutton.id.replace("btnRoutePieces", "txtChrgWt");
            var TxtClientObjectVolID = mybutton.id.replace("btnRoutePieces", "txtPcs");
            var grosswt = document.getElementById(mybutton.id.replace("btnRoutePieces", "txtChrgWt")).value;
            var pcscount = document.getElementById(mybutton.id.replace("btnRoutePieces", "txtPcs")).value;

            if (pcscount == "" || pcscount == "0") {

                alert('Please fill Piece count to proceed.');
                return;
            }

            if (grosswt == "" || grosswt == "0") {

                alert('Please fill Gross Wt. to proceed.');
                return;
            }

            var HidObject = '<%= HidAWBPieces.ClientID %>';

            window.open('AWBPiecesInfo.aspx?awbno=' + strAwbNo + '&TargetTXT=' + TxtClientObjectID + '&Hid=' + HidObject + '&GrossWt=' + grosswt + '&PcsCount=' + pcscount + '&Route=Data&RowIndex= ' + rowIndex, '', 'left=0,top=0,width=600,height=450,toolbar=0,resizable=0');

            return false;
        }

        //---Swapnil
        function ProcessRates(mybutton) {
            var strValue = mybutton.value;
            
            var grosswt = document.getElementById("<%= txtGrossWt.ClientID %>").value;
            var pcscount = document.getElementById("<%= txtPieces.ClientID %>").value;
            var AgentCode = document.getElementById("<%= TXTAgentCode.ClientID %>").value;
            var CoomodityCode = document.getElementById("<%= txtCommodityCode.ClientID %>").value;

            if (CoomodityCode == "") {
                alert('Commodity code is mandatory.');
                callclose();
                document.getElementById("<%= txtCommodityCode.ClientID %>").focus();
                return false;
            }

            if (pcscount == "" || pcscount == "0") {
                alert('Fill Pcs first.');
                callclose();
                document.getElementById("<%= txtPieces.ClientID %>").focus();
                return false;
            }

            if (grosswt == "" || grosswt == "0") {
                alert('Fill Gross Weight first.');
                callclose();
                document.getElementById("<%= txtGrossWt.ClientID %>").focus();
                return false;
            }            
            
            if (AgentCode == "") {
                alert('Agent code is mandatory.');
                callclose();
                document.getElementById("<%= TXTAgentCode.ClientID %>").focus();
                return false;
            }
            
            return true;
        }

        function AirlineCodeValidate(mybutton) {

            var strValue = mybutton.value;

            var AirlineCode = document.getElementById('ctl00_ContentPlaceHolder1_txtAirlineCode').value;

            if (AirlineCode == "") {

                alert('Please Enter Airline Code.');
                return false;
            }
            return true;
        }



        function CreateXMLHttpRequest() {
            if (typeof XMLHttpRequest != "undefined") {            //All modern browsers (IE7+, Firefox, Chrome, Safari, and Opera) uses XMLHttpRequest object
                return new XMLHttpRequest();
            }
            else if (typeof ActiveXObject != "undefined") {            // Internet Explorer (IE5 and IE6) uses an ActiveX object
                return new ActiveXObject("Microsoft.XMLHTTP");
            }
            else {
                throw new Error("XMLHttpRequest not supported");
            }
        }

        //Gets flight list based on origin and destination and displays it in Flight Number dropdown.
        function getFlightNumbers(textBox) {
            var origin;
            var destination;
            var controlname;

            if (textBox.id.indexOf('txtFltDest') > -1) {
                //Function called by flight destination text box.
                destination = textBox.value;
                //alert("Destination: " + destination);
                controlname = textBox.id.replace('txtFltDest', 'txtFltOrig');
                //alert(controlname);
                origin = document.getElementById(controlname).value;
                //alert("Origin: " + origin);
                controlname = textBox.id.replace('txtFltDest', 'ddlFltNum');
                //alert(controlname);
            }
            else {
                //Function called by flight Origin text box.
                origin = textBox.value;
                //alert("Origin: " + origin);
                controlname = textBox.id.replace('txtFltOrig', 'txtFltDest');
                //alert(controlname);
                destination = document.getElementById(controlname).value;
                //alert("Destination: " + destination);
                controlname = textBox.id.replace('txtFltOrig', 'ddlFltNum');
                //alert(controlname);
            }

            var objXMLHttpRequest = CreateXMLHttpRequest();

            objXMLHttpRequest.open("POST", "Webservices/QuickSearch.asmx", false);

            objXMLHttpRequest.setRequestHeader("Content-Type", "text/xml; charset=utf-8");

            objXMLHttpRequest.setRequestHeader("SOAPAction", "http://tempuri.org/GetFlightList");

            var packet = '<?xml version="1.0" encoding="utf-8"?><soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/"><soap:Body><GetFlightList xmlns="http://tempuri.org/"><Origin>' + origin + '</Origin><Dest>' + destination + '</Dest></GetFlightList></soap:Body></soap:Envelope>';

            //alert(packet);

            objXMLHttpRequest.send(packet);

            var objXML = objXMLHttpRequest.responseXML;
            if (objXML.childNodes.length > 0) {
                var text = objXML.childNodes[objXML.childNodes.length - 1].text;

                if (text.length == 0) {
                    return;
                }
                //alert(text);

                var flightList = text.split(';');
                document.getElementById(controlname).options.length = 0;
                for (i = 0; i < flightList.length; i++) {
                    AddItem(flightList[i], flightList[i], controlname);
                }
            }
        }

        //Adds value to dropdown.
        function AddItem(Text, Value, ControlName) {
            if (Text.length < 1) {
                return;
            }

            // Create an Option object                
            var opt = document.createElement("option");

            // Add an Option object to Drop Down/List Box
            document.getElementById(ControlName).options.add(opt);        // Assign text and value to Option object
            opt.text = Text;
            opt.value = Value;
        }

        //Set selected value of flight number in hidden field.
        function setFlightValue(flightlist) {
            //alert(flightlist.selectedIndex);
            var hdnfeildid = flightlist.id.replace('ddlFltNum', 'hdnFltNum');
            //alert(hdnfeildid);
            document.getElementById(hdnfeildid).value = flightlist.options[flightlist.selectedIndex].value;
        }

        //Prompt to change weight after editing Pcs.
        function promptWeightChange(textbox) {
            if (document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').disabled == true) {
                alert('Please don\'t forget to update Gross Weight !');
                //Set focus to respective gross weight text box in grid.
                var wttext = textbox.id.replace('TXTPcs', 'txtCommGrossWt');
                document.getElementById(wttext).focus();
            }
        }

        //Open popup for label printing...
        function OpenLabelPrintPopup() {
            window.open('PrintLabels.aspx', 'Print',
                         'left=0,top=0,width=260,height=220,toolbar=0,resizable=0');
            return false;
        }

        function OpenSearchPopup(type, val1, val2) {
            window.open("SearchPopup.aspx?type=" + type + "&val1=" +
                         val1 + "&val2=" + val2 + "", "Search",
                         "scrollbars=yes,resizable=no,width=300,height=500");
            return false;
        }

        function SelectAllgrdAddItems(CheckBoxControl) {

            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox')
        && (document.forms[0].elements[i].name.indexOf('grdAddItems') > -1)) {
                        document.forms[0].elements[i].checked = true;
                    }
                }
            }
            else {

                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
        (document.forms[0].elements[i].name.indexOf('grdAddItems') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }

        function updateFlightStation(mytext) {
            var strValue = mytext.options[mytext.selectedIndex].value;
            var i;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                var targetcontrol = 'txtFltOrig';
                if (mytext.name == 'ctl00$ContentPlaceHolder1$ddlOrg') {
                    targetcontrol = 'txtFltOrig';
                }
                if (mytext.name == 'ctl00$ContentPlaceHolder1$ddlDest') {
                    targetcontrol = 'txtFltDest';
                }
                if (document.forms[0].elements[i].name.indexOf(targetcontrol) > -1) {
                    if (strValue == ' Select') {
                        strValue = '';
                    }
                    document.forms[0].elements[i].value = strValue;
                    //Load flight dropdown list based on selected value...
                    getFlightNumbers(document.forms[0].elements[i]);
                    break;
                }
            }
        }

        //Sets value of agent name from selected value of agent code dropdown. 
        function setAgentName(DropDownList) {
            var strValue = DropDownList.options[DropDownList.selectedIndex].value;
            document.getElementById('ctl00_ContentPlaceHolder1_txtAgentName').value = strValue;
        }

        //Sets value of chargeable wt from gross wt value in Rate grids.
        function setChargedWt(mytext) {
            var strValue = mytext.value;
            var i;
            var targetcontrol = mytext.id.replace("txtCommGrossWt", "txtCommChargedWt");
            document.getElementById(targetcontrol).value = strValue;
            //        for (i = 0; i < document.forms[0].elements.length; i++) 
            //{
            //            if (document.forms[0].elements[i].name == mytext.name)
            // {
            //            
            //            }
            //            if (document.forms[0].elements[i].name.indexOf('txtCommChargedWt') > -1)
            // {                
            //                document.forms[0].elements[i].value = strValue;
            //                break;
            //            }
            //        }
        }

        function SelectAllgrdAddRate(CheckBoxControl) {

            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox')
        && (document.forms[0].elements[i].name.indexOf('grdAddRate') > -1)) {
                        document.forms[0].elements[i].checked = true;
                    }
                }
            }
            else {

                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
                    (document.forms[0].elements[i].name.indexOf('grdAddRate') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            //            document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }
        function ShowViability() {
            document.getElementById('blackening').style.display = 'block';
            document.getElementById('dvViability').style.display = 'block';

        }
        function HideViability() {
            document.getElementById('blackening').style.display = 'none';
            document.getElementById('dvViability').style.display = 'none';
        }
        function SumVolume(id) {

            __doPostBack(id, "OnTextChanged");
        }

        
    </script>

    <script type="text/javascript">

        function getOCDADetails(button) {
           // document.getElementById('Applicablecharges').style.display = 'block';
        
            var commcode = document.getElementById('<%= txtCommodityCode.ClientID %>').value;
            var TxtSubTotalID = 'ctl00_ContentPlaceHolder1_txtSubTotal';
            var TxtChargeID = 'ctl00_ContentPlaceHolder1_txtOCDueAgent';
            var TxtTaxID = 'ctl00_ContentPlaceHolder1_txtServiceTax';
            var TxtTotalID = 'ctl00_ContentPlaceHolder1_txtTotalAmount';
            var e = document.getElementById('<%= drpCurrency.ClientID %>');
            var curr = e.options[e.selectedIndex].text;
            var origin = document.getElementById('<%= ddlOrg.ClientID %>').value;
            var dest = document.getElementById('<%= ddlDest.ClientID %>').value;
            var AWBNo = document.getElementById("<%=txtAWBNo.ClientID %>").value;
            //javascript: window.open('ViewInfoInGrid.aspx?type=DA&CommCode=' + commcode + '&TxtChargeID=' + TxtChargeID + '&TxtTaxID=' + TxtTaxID + '&TxtTotalID=' + TxtTotalID + '&Currency=' + curr + '', '', 'location=no,left=200,top=200,width=630,height=330,toolbar=0,resizable=0');
            javascript: window.open('ViewInfoInGrid.aspx?type=DA&CommCode=' + commcode + '&TxtSubTotalID=' + TxtSubTotalID + '&TxtChargeID=' + TxtChargeID + '&TxtTaxID=' + TxtTaxID + '&TxtTotalID=' + TxtTotalID + '&Currency=' + curr + '&Org=' + origin + '&Dest=' + dest +'&AWBNo=' +AWBNo+'', '', 'location=no,left=200,top=200,width=630,height=330,toolbar=0,resizable=0');


            return false;
        }

        function getOCDCDetails(button) {
          //  document.getElementById('Applicablecharges').style.display = 'block';
        
            var commcode = document.getElementById('<%= txtCommodityCode.ClientID %>').value;

            var TxtChargeID = 'ctl00_ContentPlaceHolder1_txtOCDueCar';
            var TxtTaxID = 'ctl00_ContentPlaceHolder1_txtServiceTax';
            var TxtTotalID = 'ctl00_ContentPlaceHolder1_txtTotalAmount';
            var TxtSubTotalID = 'ctl00_ContentPlaceHolder1_txtSubTotal';
            var AWBNo = document.getElementById("<%=txtAWBNo.ClientID %>").value;
            
            var e = document.getElementById('<%= drpCurrency.ClientID %>');
            var curr = e.options[e.selectedIndex].text;
            var origin = document.getElementById('<%= ddlOrg.ClientID %>').value;
            var dest = document.getElementById('<%= ddlDest.ClientID %>').value;
            //javascript: window.open('ViewInfoInGrid.aspx?type=DC&CommCode=' + commcode + '&TxtChargeID=' + TxtChargeID + '&TxtTaxID=' + TxtTaxID + '&TxtTotalID=' + TxtTotalID + '&Currency=' + curr + '', '', 'location=no,left=200,top=200,width=630,height=330,toolbar=0,resizable=0');
            javascript: window.open('ViewInfoInGrid.aspx?type=DC&CommCode=' + commcode + '&TxtSubTotalID=' + TxtSubTotalID + '&TxtChargeID=ctl00_ContentPlaceHolder1_txtOCDueCar' + '&TxtTaxID=' + TxtTaxID + '&TxtTotalID=' + TxtTotalID + '&Currency=' + curr + '&Org=' + origin + '&Dest=' + dest +'&AWBNo='+AWBNo +'', '', 'location=no,left=200,top=200,width=630,height=330,toolbar=0,resizable=0');

            return false;
        }

        function showFlights(button) {
            var Hid = '<%= HidFlightsChanged.ClientID %>';
            var Destination = document.getElementById('<%= HidDestination.ClientID %>').value;
            window.open('ShowFlights.aspx?Hid=' + Hid + '&Dest=' + Destination, '', 'left=200,top=200,width=850,height=300,toolbar=0,resizable=0');
            return false;
        }

        function CalledFromShowFlights() {

            //PageMethods.LoadGridWithFlights(document.getElementById('<%=grdRouting.ClientID%>'), OnSuccess, OnFail);
            //window.location.reload(true);
            __doPostBack('', '');
        }

        function OnSuccess() {

        }
        function OnFail() {

        }

        function ShowHideShipperDetials() {
            var DivDisplayStatus;
            DivDisplayStatus = (document.getElementById('DivShipperCon').style.display == "block" ? "none" : "block");
            //            DivDisplayStatus = (document.getElementById('Consigneedetails').style.display == "block" ? "none" : "block");
            //            DivDisplayStatus = (document.getElementById('cargodetails').style.display == "block" ? "none" : "block");



            document.getElementById('DivShipperCon').style.display = DivDisplayStatus;

            if (DivDisplayStatus == "none") {
                document.getElementById('imgPlus').style.display = "block";
                document.getElementById('imgMinus').style.display = "none";
            }
            else {
                document.getElementById('imgMinus').style.display = "block";
                document.getElementById('imgPlus').style.display = "none";
            }
        }

        function ShowHideRouteDetials() {
            var DivDisplayStatus;
            DivDisplayStatus = (document.getElementById('ctl00_ContentPlaceHolder1_Routedetails').style.display == "block" ? "none" : "block");
            
            document.getElementById('ctl00_ContentPlaceHolder1_Routedetails').style.display = DivDisplayStatus;

            document.getElementById("<%= hdnRouteDetVisible.ClientID %>").value = DivDisplayStatus;
            

            if (DivDisplayStatus == "none") {
                document.getElementById('imgPlus1').style.display = "block";
                document.getElementById('imgMinus1').style.display = "none";
            }
            else {
                document.getElementById('imgMinus1').style.display = "block";
                document.getElementById('imgPlus1').style.display = "none";
            }
        }

        function ShowHideotherAction() {
            var DivDisplayStatus;
            //DivDisplayStatus = (document.getElementById('DivShipperCon').style.display == "block" ? "none" : "block");
            //DivDisplayStatus = (document.getElementById('Consigneedetails').style.display == "block" ? "none" : "block");
            DivDisplayStatus = (document.getElementById('fotbut').style.display == "block" ? "none" : "block");

            document.getElementById('fotbut').style.display = DivDisplayStatus;

            if (DivDisplayStatus == "none") {
                document.getElementById('imgPlus2').style.display = "block";
                document.getElementById('imgMinus2').style.display = "none";
            }
            else {
                document.getElementById('imgMinus2').style.display = "block";
                document.getElementById('imgPlus2').style.display = "none";
            }
        }


        function PrintDO()
        { window.open('showCapacity.aspx', 'Print', 'toolbar=1,resizable=1,menubar=1'); return false; }

        function checkChangedNew(chk) {
            var tbPcs;
            var tbWt;
            var grid = document.getElementById('<%=grdRouting.ClientID %>');
            var RowIndex = chk.parentElement.parentElement.rowIndex;

            if (grid.rows[RowIndex].cells[13].children[0].checked) {
                tbPcs = grid.rows[RowIndex].cells[14].children[0];
                tbWt = grid.rows[RowIndex].cells[15].children[0];
                grid.rows[RowIndex].cells[14].children[0].value = 0;
                grid.rows[RowIndex].cells[15].children[0].value = 0;
                tbPcs.disabled = false;
                tbWt.disabled = false;
            }
            else {
                tbPcs = grid.rows[RowIndex].cells[14].children[0];
                tbWt = grid.rows[RowIndex].cells[15].children[0];
                grid.rows[RowIndex].cells[14].children[0].value = '';
                grid.rows[RowIndex].cells[15].children[0].value = '';
                tbPcs.disabled = true;
                tbWt.disabled = true;
            }
        }
        
    </script>

    <script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>

    <script type="text/javascript">

        $(function() {
            $('#<%=  ddlProductType.ClientID %>').change(function() {
                var ddllevel = document.getElementById('<%=ddlProductType.ClientID%>');
                var level = ddllevel.options[ddllevel.selectedIndex].text;
                var SHC = document.getElementById('<%= txtSpecialHandlingCode.ClientID %>').value;
                var str = "";

                if (level == "PERISHABLE") {
                    if (SHC.indexOf("PER") != 1) {
                        str = "PER,";

                        document.getElementById('<%= txtSpecialHandlingCode.ClientID %>').value = str;
                    }
                }
                if (level == "VALUABLE") {
                    if (SHC.indexOf("VAL") != 1) {
                        str = "VAL,";

                        document.getElementById('<%= txtSpecialHandlingCode.ClientID %>').value = str;
                    }
                }

            });
        });
    </script>

    <script type="text/javascript">

        function txtDatefocus() {            
            var HidObject = '<%= HidChangeDate.ClientID %>';            
            document.getElementById(HidObject).value = "Y";
        }
     
    </script>

    <script type="text/javascript">
        function ValidateShipperDetails() {
            return true;

        }
        function ValidateShipperDetailsOld() {

            var ShipperName = document.getElementById('<%=TXTShipper.ClientID%>');
            var ShipperAddress = document.getElementById('<%=TXTShipAddress.ClientID%>');
            var ConsigneeName = document.getElementById('<%=TXTConsignee.ClientID%>');
            var ShipperMobile = document.getElementById('<%=TXTShipTelephone.ClientID%>');
            var ConsigneeAddress = document.getElementById('<%=TXTConAddress.ClientID%>');
            var ConMobile = document.getElementById('<%=TXTConTelephone.ClientID%>');

            var HandlingInfo = document.getElementById('ctl00_ContentPlaceHolder1_txtHandling').value;
            var CommCode = document.getElementById('ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_ddlMaterialCommCode').value;
            var Acceptedchk = document.getElementById('ctl00_ContentPlaceHolder1_grdRouting_ctl02_chkAccepted').value;
            var Status = document.getElementById('ctl00_ContentPlaceHolder1_grdRouting_ctl02_ddlStatus').value;
            //alert(Acceptedchk);
            //alert(Status);

            if (ctl00_ContentPlaceHolder1_grdRouting_ctl02_chkAccepted.checked == true) {
                //alert('checked');
                if (Status != "C") {
                    alert('Shippment is not Confirmed.');
                    callclose();
                    document.getElementById('ctl00_ContentPlaceHolder1_grdRouting_ctl02_ddlStatus').focus();
                    return false;

                }
            }
            if (CommCode == "Perishables") {
                if (HandlingInfo == "") {
                    alert('Please Fill Handling Info');
                    callclose();
                    document.getElementById('ctl00_ContentPlaceHolder1_txtHandling').focus();
                    return false;
                }
            }

            if (ShipperName.value == '' || ShipperAddress.value == '' || ShipperMobile.value == '' || ConsigneeName.value == '' || ConMobile.value == '' || ConsigneeAddress.value == '') {
                alert("Please Enter Shipper/Consignee Details");
                ShowHideShipperDetials(); return false;
                document.getElementById('DivShipperCon').scrollIntoView();
                return false;
            }

            return true;
        }
    </script>

    <script type="text/javascript">
        function selectAllCheckboxInGrid(ddl) {
            var txtid = ddl.id;
            var finalid = txtid.substring(0, 51) + "txtMaterialCommDesc";
            var textbox = document.getElementById(finalid);
            var code = ddl.value;
            textbox.value = code;
            return false;
        }

    </script>

    <script type="text/javascript">


        function GetAgentCode(obj) {
            var destination = obj;
            var AgentName = document.getElementById('<%= txtAgentName.ClientID%>');
            var CustCode = "";
            var ProcessFlag = document.getElementById('<%= HidProcessFlag.ClientID%>');

            if (destination.value.indexOf("(") > 0) {
                //                if (destination.value.length > 4) {
                var start = destination.value.indexOf("(");
                var end = destination.value.lastIndexOf(")");
                var str = destination.value;
                var CustStart = destination.value.indexOf("-");
                var CustEND = destination.value.indexOf("$");
                var Defaultpaymode = destination.value.indexOf(">");
                destination.value = str.substring(0, start);
                AgentName.value = str.substring(start + 1, end);
                CustCode.value = str.substring(CustStart + 1, CustEND);
               
                var PaymodeList = str.substring(Defaultpaymode + 1, str.length);
                if(PaymodeList.length>0){
                    var paymode = PaymodeList.split(",");
                    var ddlPaymode = document.getElementById('<%= ddlPaymentMode.ClientID %>');
                    ddlPaymode.options.length=0;
                    ddlPaymode.innerHTML = "";
                    var item = document.createElement('option');
                    item.text = item.value = 'Select';
                    ddlPaymode.add(item, 0);
                    for(var i=0;i<paymode.length;i++)
                    {
                        var item = document.createElement('option');
                        item.text = item.value =paymode[i];
                        ddlPaymode.add(item, (i+1));
                    }
                }
                document.getElementById('<%= ddlPaymentMode.ClientID %>').value = str.substring(CustEND + 1, Defaultpaymode);

                //alert(str.substring(Defaultpaymode + 1, str.length));
                ProcessFlag.value = "1";
                //                }
            }
        }

        function GetCommodityCode(obj) {
            var destination = obj;
            var CommodityName = 'ctl00_ContentPlaceHolder1_txtCommodityName';
            var ProcessFlag = document.getElementById('<%= HidProcessFlag.ClientID%>');
            var SHCCode = document.getElementById('<%= txtSpecialHandlingCode.ClientID%>');

            if (destination.value.indexOf("(") > 0) {
                //                if (destination.value.length > 4) {
                var str = destination.value;
                var start = destination.value.indexOf("(");
                var end = destination.value.indexOf(")");
                var SHCStart = destination.value.indexOf("$");

                obj.value = str.substring(0, start);
                document.getElementById(CommodityName).value = str.substring(start + 1, end);
                document.getElementById(CommodityName).focus();
                SHCCode.value = str.substring(SHCStart + 1, str.length);
                ProcessFlag.value = "1";
                //                }
            }
            SetProcessFlag();
            return false;
        }

//        function GetShipperCode(obj) {
//            var destination = obj;

//            if (destination.value.indexOf("(") > 0) {
//                var str = destination.value;
//                var start = destination.value.indexOf("(");

//                obj.value = str.substring(0, start);
//            }

//            return false;
//        }

        function GetShipperCode(obj) {
            var destination = obj.value;

            var objString = destination.split("|");
            var AccountCode = objString[0];
            var AccountName = objString[1];

            var Telephone = objString[2];
            var Address1 = objString[3];
            var Country = objString[4];

            var Adress2 = objString[5];
            var City = objString[6];
            var State = objString[7];
            var Email = objString[8];
            var ZipCode = objString[9];
            
            if (objString.length > 1) {
            
                setTimeout(function() {
            
                    document.getElementById('<%= TXTShipper.ClientID%>').value = AccountName;
                    document.getElementById('<%= TXTShipTelephone.ClientID%>').value = Telephone.trim();
                    document.getElementById('<%= TXTShipAddress.ClientID%>').value = Address1;
                    var e = document.getElementById('<%= ddlShipCountry.ClientID%>');
                    //alert(Country );
                    e.value = Country;
                    obj.value = AccountCode;

                    document.getElementById('<%= TXTShipperAdd2.ClientID%>').value = Adress2;
                    document.getElementById('<%= TXTShipperCity.ClientID%>').value = City;
                    document.getElementById('<%= TXTShipperState.ClientID%>').value = State;
                    document.getElementById('<%= TXTShipperEmail.ClientID%>').value = Email;
                    document.getElementById('<%= TXTShipPinCode.ClientID%>').value = ZipCode;
                                    
                    CheckShipper();
                    
                },0);
            }
            else {
            
                setTimeout(function() {
                    document.getElementById('<%= TXTShipper.ClientID%>').value = "";
                    document.getElementById('<%= TXTShipTelephone.ClientID%>').value = "";
                    document.getElementById('<%= TXTShipAddress.ClientID%>').value = "";

                    document.getElementById('<%= TXTShipperAdd2.ClientID%>').value = "";
                    document.getElementById('<%= TXTShipperCity.ClientID%>').value = "";
                    document.getElementById('<%= TXTShipperState.ClientID%>').value = "";
                    document.getElementById('<%= TXTShipperEmail.ClientID%>').value = "";
                    document.getElementById('<%= TXTShipPinCode.ClientID%>').value = "";

                    CheckShipper();
                },0);
            }

            return false;
        }

        //        function GetConsigneeCode(obj) {
        //            var destination = obj;

        //            if (destination.value.indexOf("(") > 0) {
        //                var str = destination.value;
        //                var start = destination.value.indexOf("(");

        //                obj.value = str.substring(0, start);
        //            }
        //        }

        function GetConsigneeCode(obj) {
            var destination = obj.value;

            var objString = destination.split("|");
            var AccountCode = objString[0];
            var AccountName = objString[1];
            var Telephone = objString[2];
            var Address1 = objString[3];
            var Country = objString[4];

            var Adress2 = objString[5];
            var City = objString[6];
            var State = objString[7];
            var Email = objString[8];
            var ZipCode = objString[9];

            if (objString.length > 1) {
            
                setTimeout(function() {
                    document.getElementById('<%= TXTConsignee.ClientID%>').value = AccountName;
                    document.getElementById('<%= TXTConTelephone.ClientID%>').value = Telephone.trim();
                    document.getElementById('<%= TXTConAddress.ClientID%>').value = Address1;
                    var e = document.getElementById('<%= ddlConCountry.ClientID%>');
                    e.value = Country;
                    obj.value = AccountCode;

                    document.getElementById('<%= TXTConsigAdd2.ClientID%>').value = Adress2;
                    document.getElementById('<%= TXTConsigCity.ClientID%>').value = City;
                    document.getElementById('<%= TXTConsigState.ClientID%>').value = State;
                    document.getElementById('<%= TXTConsigEmail.ClientID%>').value = Email;
                    document.getElementById('<%= TXTConsigPinCode.ClientID%>').value = ZipCode;

                    CheckConsignee();
                },0);
            }
            else {
            
                setTimeout(function() {
                    document.getElementById('<%= TXTConsignee.ClientID%>').value = "";
                    document.getElementById('<%= TXTConTelephone.ClientID%>').value = "";
                    document.getElementById('<%= TXTConAddress.ClientID%>').value = "";

                    document.getElementById('<%= TXTConsigAdd2.ClientID%>').value = "";
                    document.getElementById('<%= TXTConsigCity.ClientID%>').value = "";
                    document.getElementById('<%= TXTConsigState.ClientID%>').value = "";
                    document.getElementById('<%= TXTConsigEmail.ClientID%>').value = "";
                    document.getElementById('<%= TXTConsigPinCode.ClientID%>').value = "";

                    CheckConsignee();
                },0);
            }

            return false;
        }

        function GetDriverDetails(obj) {
            var destination = obj;
            var CommodityName = obj.id.replace('txtDriverName', 'txtDriverDL');


            if (destination.value.indexOf("(") > 0) {

                var str = destination.value;
                var start = destination.value.indexOf("(");
                var end = destination.value.indexOf(")");

                obj.value = str.substring(0, start);
                document.getElementById(CommodityName).value = str.substring(start + 1, end);
                document.getElementById(obj.id.replace('txtDriverName', 'txtDriverDL')).focus();
            }

            return false;
        }

        function GetPartnerCodeforPrefix(obj) {
            var destination = obj;
            if (destination.value.indexOf("-") > 0) {

                var str = destination.value;
                var start = destination.value.indexOf("-");
                var begi = destination.value.indexOf("(");
                var end = destination.value.indexOf(")");

                obj.value = str.substring(0, start);
                document.getElementById('ctl00_ContentPlaceHolder1_ddlAirlineCode').value = str.substring(begi + 1, end);


            }

            return true;
        }

        function GetShipperDetails(obj) {
            var destination = obj;

            if (destination.value.indexOf("(") > 0) {

                var str = destination.value;
                var start = destination.value.indexOf("(");
                var end = destination.value.indexOf(")");

                obj.value = str.substring(start + 1, end);
            }

            return false;
        }

        function SetProcessFlag() {
            var ProcessFlag = document.getElementById('<%= HidProcessFlag.ClientID%>');
            ProcessFlag.value = "1";

            var EAWBFlag = document.getElementById('<%= HidEAWB.ClientID%>');
            EAWBFlag.value = "1";

            return true;
        }

        function GetProcessFlag() {
            var ProcessFlag = document.getElementById('<%= HidProcessFlag.ClientID%>');

            //            if (ProcessFlag.value == "1") {
            //                alert("Kindly Process the Rates to proceed !");
            //                return false;
            //            }
            return true;
        }

        function GetEAWBFlag() {
            var EAWBFlag = document.getElementById('<%= HidEAWB.ClientID%>');

            if (EAWBFlag.value == "1") {
                alert("Kindly Save AWB details to proceed !");
                return false;
            }
            return true;
        }
        
    </script>

    <%--Select Label PopUp--%>

    <script type="text/javascript">
        function ViewPanelSplit_SelLbl() {
            document.getElementById('DivSelLbl').style.display = 'block';
            document.getElementById('DivSelLbl1').style.display = 'block';
            return false;
        }
        function HidePanelSplit_SelLbl() {
            document.getElementById('DivSelLbl').style.display = 'none';
            document.getElementById('DivSelLbl1').style.display = 'none';
            return false;
        }
    </script>

    <script>
        jQuery(document).ready(function() {
            var allPanels = jQuery('.accordion > dd').hide();

            jQuery('.accordion > dt > a').click(function() {
                jQuery(this).parent().next().slideUp();

                if (jQuery(this).parent().next().is(':hidden')) {
                    jQuery(this).parent().next().slideDown();
                }
                return false;
            });
        });

        function CollapseAll() {

            var allPanels = $('.accordion > dd').hide();
            return false;
        };
        function ExpandAll() {

            var allPanels = $('.accordion > dd').show();
            allPanels.slideDown();
            return false;
        };
    </script>

    <style type="text/css">
        .black_overlay
        {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
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
            left: 31%;
            width: auto;
            height: auto;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
        
                        .white_content_HAWB
        {
            margin: 0 auto;
            display: none;
            position: absolute;
            top: 30%;
            width: auto;
            height: auto;
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
        .button
        {
            margin-bottom: 3px;
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
    <style type="text/css">
        .modalPopup
        {
            background-color: #696969;
            filter: alpha(opacity=40);
            opacity: 0.7;
            xindex: -1;
        }
       
    </style>
    
    <style type="text/css">
            .CompletionListCssClass
{
    color: #000;
    padding: 3px 5px;
    border: 1px solid #999;
    width: auto !important;
    float: left;
    background-color: white;
    z-index: 100009 !important;
    position:absolute;
    margin-left:0px;
    display:block;
    
}
    </style>

    <script type="text/javascript">
        function DisableButton() {
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }
        window.onbeforeunload = DisableButton;

        function callShowCapacity() {
            window.open('showCapacity.aspx', '', 'left=0,top=0,width=800,height=600,toolbar=0,resizable=0');
        }

        function ViewPanellSplit() {
            document.getElementById('LightDiv').style.display = 'block';
            document.getElementById('fadeDiv').style.display = 'block';
        }
        function HidePanellSplit() {
            document.getElementById('LightDiv').style.display = 'none';
            document.getElementById('fadeDiv').style.display = 'none';
        }
    
    </script>

    <script type="text/javascript">
        function OnReadAWB(objCntl) {

            var AWBNowithPrefix = objCntl.value;

            if (AWBNowithPrefix.length < 10) {
                return false;
            }

            var AWBPrefix = AWBNowithPrefix.replace(AWBNowithPrefix.substring(AWBNowithPrefix.length - 8), '');
            var AWBNo = AWBNowithPrefix.replace(AWBPrefix, '');

            document.getElementById("<%=txtAwbPrefix.ClientID %>").value = AWBPrefix;
            document.getElementById("<%=txtAWBNo.ClientID %>").value = AWBNo;

            document.getElementById("<%=btnListAgentStock.ClientID %>").click();

            return true;
        }

        // WRITE THE VALIDATION SCRIPT IN THE HEAD TAG.
        function isNumber(evt) {
            var AWBNo = document.getElementById("<%=txtAWBNo.ClientID %>").value;
            if (AWBNo.length > 7)
                return false;

            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;

            return true;
        }
        
    </script>

    <%--Java script to calculate AWB Pieces & weight at client side--%>

    <script type="text/javascript">

        function CallPopulateClick() {
            window.opener.cllsa();
            self.close();

        }

        function CheckQueueStatus() {
        
            var AWBStatusDropDown = document.getElementById('<%= ddlAWBStatus.ClientID %>');
            var AWBStatus = AWBStatusDropDown.options[AWBStatusDropDown.selectedIndex].value;
            
            var RateFlag = document.getElementById('<%= HidProcessFlag.ClientID %>').value;
            
            if (RateFlag == '1') {
                alert('Please save the booking before Check-in.');
                return false;
            }
            
            if (AWBStatus != 'C') {
                alert('Shipment is not yet Confirmed.');
                AWBStatusDropDown.focus();
                return false;
            }
            
            var PaymentDropDown = document.getElementById('<%= ddlPaymentMode.ClientID %>');
            var PaymentMode = PaymentDropDown.options[PaymentDropDown.selectedIndex].text;
            
            if (PaymentMode == 'Select') {
                alert('Please select Payment mode.');
                PaymentDropDown.focus();
                return false;
            }
            
            if(FlightCheckinTime()==false)
                return false;
                
            return true;
        }

        function sumAWBPackagingDetails() {
            var txts = null;
            var labels = null;

            var sumPackages = 0;
            var SumGrossWt = 0;
            var SumChrgblWt = 0;

            for (var i = 0; i < txts.length; i++) {

                if (txts[i].type === 'text') {
                    var TextBox = txts[i].id;
                    var TextBoxVal = 0;

                    if (TextBox.indexOf("TXTPcs") > 0) {
                        TextBoxVal = txts[i].value;
                        if (TextBoxVal == "") {
                            TextBoxVal = 0;
                        }
                        sumPackages += Number(TextBoxVal);
                    }
                    else if (TextBox.indexOf("txtCommGrossWt") > 0) {
                        TextBoxVal = txts[i].value;
                        if (TextBoxVal == "") {
                            TextBoxVal = 0;
                        }
                        SumGrossWt += Number(TextBoxVal);

                        for (var CWt = 0; CWt < txts.length; CWt++) {
                            if (txts[CWt].id.indexOf("txtCommChargedWt") > 0) {
                                if (SumGrossWt > txts[CWt].value) {
                                    document.getElementById(txts[CWt].id).value = SumGrossWt;
                                }
                            }
                        }
                    }
                    else if (TextBox.indexOf("txtCommChargedWt") > 0) {
                        TextBoxVal = txts[i].value;
                        if (TextBoxVal == "") {
                            TextBoxVal = 0;
                        }
                        SumChrgblWt += Number(TextBoxVal);
                    }
                } //End of If
            } //End Of For

            for (var i = 0; i < labels.length; i++) {

                if (labels[i].id.indexOf("LBLTotalPcs") > 0) {
                    var TotalPcs = document.getElementById(labels[i].id);
                    TotalPcs.innerText = sumPackages;
                }
                if (labels[i].id.indexOf("LBLTotalGrWt") > 0) {
                    var TotalGWt = document.getElementById(labels[i].id);
                    TotalGWt.innerText = SumGrossWt;
                }
                if (labels[i].id.indexOf("LBLTotalChargedWt") > 0) {
                    var TotalCWt = document.getElementById(labels[i].id);
                    TotalCWt.innerText = SumChrgblWt;
                }
            }
            SetProcessFlag();
        } //End Of Function

        function checkAlphabetDecimal(alpha) {
            var strNum = alpha;
            var blnError = false;

            for (n = strNum.length - 1; n >= 0; n--) {
                var cDigit = strNum.charAt(n);
                if (!isDecimal(cDigit)) {
                    blnError = true;
                    break;
                }
                else {
                    blnError = false;
                }
            }

            if (blnError == true)
                return true;
            else
                return false;

        } // checkAlphabet Ends

        function isDecimal(c) {
            var strAllowed = "1234567890";
            return (strAllowed.indexOf(c) != -1);
        }

        //Get matching product type based on Origin and Destination.
        function GetMatchingProductTypes() {
            var originValue = document.getElementById('<%= ddlOrg.ClientID %>').value;
            var destValue = document.getElementById('<%= ddlDest.ClientID %>').value;
            var TxtClientObject = '<%= ddlProductType.ClientID %>';
            window.open('ListSingleSelectProductType.aspx?origin=' + originValue + '&destination=' + destValue +
             '&TxtClientObject=' + TxtClientObject, '', 'left=' + (screen.availWidth / 5) + ',top=' +
             (screen.availHeight / 8) + ',width=800,height=550,toolbar=0,resizable=0');
        }
    
    </script>
    
    <script type="text/javascript">
        function ViewPanel_shipperPopUp() {
            document.getElementById('DivShipperCon').style.display = 'block';
            document.getElementById('DivShipperCon1').style.display = 'block';
            document.getElementById('<%=TXTShipper.ClientID %>').focus();
            
            return false;
        }
        function HidePanel_shipperPopUp() {
            document.getElementById('DivShipperCon').style.display = 'none';
            document.getElementById('DivShipperCon1').style.display = 'none';
            document.getElementById('<%=ImageButton2.ClientID %>').focus();
            return false;
        }
        function ViewPanel_ConsigneePopUp() {
            document.getElementById('Consigneedetails').style.display = 'block';
            document.getElementById('Consigneedetails1').style.display = 'block';
            document.getElementById('<%=TXTConsignee.ClientID %>').focus();
            
            return false;
        }
        function HidePanel_ConsigneePopUp() {
            document.getElementById('Consigneedetails').style.display = 'none';
            document.getElementById('Consigneedetails1').style.display = 'none';
            document.getElementById('<%=ImageButton3.ClientID %>').focus();
            
            return false;
        }
        
        function ViewPanel_CargoPopUp() {
            document.getElementById('cargodetails').style.display = 'block';
            document.getElementById('cargodetails1').style.display = 'block';
            document.getElementById('<%=ddlShipmentType.ClientID %>').focus();
            return false;
        }
                function HidePanel_CargoPopUp() {
            document.getElementById('cargodetails').style.display = 'none';
            document.getElementById('cargodetails1').style.display = 'none';
            document.getElementById('<%=ImageButton4.ClientID %>').focus();
            document.getElementById('ctl00_ContentPlaceHolder1_txtShipmentPriority').value = '';
            
            if (                    
                    document.getElementById('ctl00_ContentPlaceHolder1_ddlShipmentType').value != 'Bulk'||
                    document.getElementById('ctl00_ContentPlaceHolder1_chkTBScreened').checked != '1'||
                    document.getElementById('ctl00_ContentPlaceHolder1_ddlServiceclass').value != '5'||                    
                    document.getElementById('ctl00_ContentPlaceHolder1_ddlIrregularityCode').value != '0'||
                    
                    document.getElementById('ctl00_ContentPlaceHolder1_CHKConsole').checked == true || 
                    document.getElementById('ctl00_ContentPlaceHolder1_CHKBonded').checked == true || 
                    document.getElementById('ctl00_ContentPlaceHolder1_CHKExportShipment').checked == true || 
                    document.getElementById('ctl00_ContentPlaceHolder1_CHKAsAggred').checked == true || 
                    document.getElementById('ctl00_ContentPlaceHolder1_CHKAllIn').checked == true ||
                    
                    document.getElementById('ctl00_ContentPlaceHolder1_TXTDvForCustoms').value != '' || 
                    document.getElementById('ctl00_ContentPlaceHolder1_txtEURIN').value != '' ||                     
                    document.getElementById('ctl00_ContentPlaceHolder1_txtSLAC').value != '' || 
                    document.getElementById('ctl00_ContentPlaceHolder1_txtCustoms').value != '' || 
                    document.getElementById('ctl00_ContentPlaceHolder1_txtIACCode').value != '' || 
                    document.getElementById('ctl00_ContentPlaceHolder1_txtCCSF').value != '' || 
                    
                    document.getElementById('ctl00_ContentPlaceHolder1_txtShippingAWB').value != '' || 
                    document.getElementById('ctl00_ContentPlaceHolder1_ddlHandler').value != ''||
                    document.getElementById('ctl00_ContentPlaceHolder1_txtDriverName').value != '' || 
                    document.getElementById('ctl00_ContentPlaceHolder1_txtPhoneNo').value != '' || 
                    document.getElementById('ctl00_ContentPlaceHolder1_txtDriverDL').value != '' || 
                    document.getElementById('ctl00_ContentPlaceHolder1_txtVehicleNo').value != ''
                 ) 
                 {               
                document.getElementById('ctl00_ContentPlaceHolder1_txtShipmentPriority').value = 'Y';
            }
            return false;
        }


        function ViewPanel_ApplicablechargesPopUp() {
            document.getElementById('Applicablecharges').style.display = 'block';
            document.getElementById('Applicablecharges1').style.display = 'block';
            document.getElementById('<%=txtVolume.ClientID %>').focus();
            return false;
        }

        function HidePanel_ApplicablechargesPopUp() {
            document.getElementById('Applicablecharges').style.display = 'none';
            document.getElementById('Applicablecharges1').style.display = 'none';
            document.getElementById('<%=ImageButton6.ClientID %>').focus();
            
            return false;
        }
        
//        function HidePanel_CargoPopUp() {
//            document.getElementById('Applicablecharges').style.display = 'none';
//            document.getElementById('Applicablecharges1').style.display = 'none';
//            return false;
        //        }

        function CheckShipper() {
            var shippercode = document.getElementById('ctl00_ContentPlaceHolder1_TXTShipper').value;
            var shipperTel = document.getElementById('ctl00_ContentPlaceHolder1_TXTShipTelephone').value;
            var shipperAddr = document.getElementById('ctl00_ContentPlaceHolder1_TXTShipAddress').value;
            var e = document.getElementById('ctl00_ContentPlaceHolder1_ddlShipCountry');
            var country = e.options[e.selectedIndex].value;

            if (shippercode == "" || shipperTel == "" || shipperAddr == "" || country == "") {
                HidePanel_shipperPopUp();
                document.getElementById('ctl00_ContentPlaceHolder1_imgShipperTick').style.display = "none";
                document.getElementById('ctl00_ContentPlaceHolder1_imgShipperCross').style.display = "inline";
                return false;
            }
            else {
                HidePanel_shipperPopUp();
                document.getElementById('ctl00_ContentPlaceHolder1_imgShipperTick').style.display = "inline";
                document.getElementById('ctl00_ContentPlaceHolder1_imgShipperCross').style.display = "none";
                return false;
            }
        }

        function CheckConsignee() {
            var Consigneecode = document.getElementById('ctl00_ContentPlaceHolder1_TXTConsignee').value;
            var ConsigneeTel = document.getElementById('ctl00_ContentPlaceHolder1_TXTConTelephone').value;
            var ConsigneeAddr = document.getElementById('ctl00_ContentPlaceHolder1_TXTConAddress').value;
            var e = document.getElementById('ctl00_ContentPlaceHolder1_ddlConCountry');
            var country = e.options[e.selectedIndex].value;

            if (Consigneecode == "" || ConsigneeTel == "" || ConsigneeAddr == "" || country == "") {
                HidePanel_ConsigneePopUp();
                document.getElementById('ctl00_ContentPlaceHolder1_imgConCross').style.display = "inline";
                document.getElementById('ctl00_ContentPlaceHolder1_imgConTick').style.display = "none";

                return false;
            }
            else {
                HidePanel_ConsigneePopUp();
                document.getElementById('ctl00_ContentPlaceHolder1_imgConCross').style.display = "none";
                document.getElementById('ctl00_ContentPlaceHolder1_imgConTick').style.display = "inline";
                return false;
            }
        }
        
    </script>
    
    <style type="text/css">
        .black_overlay
        {
            display: none;
            position:fixed;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
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
            position:fixed;
            top: 30%;
            left: 30%;
            width: 33%;
            height: auto;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
        
    </style>
    
    <script type="text/javascript">


        function dimensionAcceptance() {            
            var DropDownList = document.getElementById("<%= txtCommodityCode.ClientID %>");
            var strValue = DropDownList.value;
            var strAwbNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;
            var strAgentCode = document.getElementById("<%= TXTAgentCode.ClientID %>").value;
            //var ExecDt = document.getElementById("<%= txtExecutionDate1.ClientID %>").value;
            var ExecDt = document.getElementById('ctl00_ContentPlaceHolder1_txtExecutionDate1_txtDate').value;

            var TxtClientObjectID = 'ctl00_ContentPlaceHolder1_txtChargeableWt';
            var TxtClientObjectVolID = 'ctl00_ContentPlaceHolder1_txtVolume';
            var grosswt = document.getElementById("<%= txtAccpWeight.ClientID %>").value;
            var pcscount = document.getElementById("<%= txtAccpPieces.ClientID %>").value;

            var Origin = document.getElementById('ctl00_ContentPlaceHolder1_ddlOrg').value;
            var Dest = document.getElementById('ctl00_ContentPlaceHolder1_ddlDest').value;

            if (pcscount == "" || pcscount == "0") {

                alert('Fill Pcs first.');
                return;
            }

            var rowindex = 1;

            var HidObject = '<%= HidDimension.ClientID %>';

            window.open('GHA_Dimensions.aspx?awbno=' + strAwbNo + '&commodity=' + strValue + '&TargetTXT=' + TxtClientObjectID + '&Hid=' + HidObject + '&GrossWt=' + grosswt + '&PcsCount=' + pcscount + '&RowIndex=' + rowindex + '&VolumeTXT=' + TxtClientObjectVolID + '&Mode=B' + '&QBA=Y' + '&Origin=' + Origin + '&Destination=' + Dest + '&AgtCode=' + strAgentCode + '&ExecDt=' + ExecDt, '', 'left=50,top=50,width=810,height=430,toolbar=0,resizable=0,scrollbars=yes');

            return false;

        }

        //Function to open Notes.
        function OpenNotesChild() {

            var Prefix = document.getElementById("<%= txtAwbPrefix.ClientID %>").value;
            var No = document.getElementById("<%= txtAWBNo.ClientID %>").value;

            if (No != '') {
                window.open("Notes.aspx?No=" + No + "&Prefix=" + Prefix, "", "status=0,toolbar=0, menubar=0, width=810, height=500");
            }
            else {
                window.open("Notes.aspx", "", "status=0,toolbar=0, menubar=0, width=810, height=500");
            }
            return false;
        }


        function RadioCheck(rb) {
            var gv = document.getElementById("<%=gvHAWBDetails.ClientID%>");
            var rbs = gv.getElementsByTagName("input");

            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        break;
                    }

                }
            }
        }

        function PopulateShipperConsigneeDetailsHAWB(rb) {
            var ShipperName = document.getElementById(rb.id.replace("check", "hdShipperName")).value;
            var ShipperTelephone = document.getElementById(rb.id.replace("check", "hdShipperTelephone")).value;
            var ShipperAddress = document.getElementById(rb.id.replace("check", "hdShipperAddress")).value;
            var ShipperCity = document.getElementById(rb.id.replace("check", "hdShipperCity")).value;
            var ShipperState = document.getElementById(rb.id.replace("check", "hdShipperState")).value;
            var ShipperCountry = document.getElementById(rb.id.replace("check", "hdShipperCountry")).value;
            var ShipperPinCode = document.getElementById(rb.id.replace("check", "hdShipperPinCode")).value;
            var ShipperEmail = document.getElementById(rb.id.replace("check", "hdShipperEmail")).value;

            var ConsigneeName = document.getElementById(rb.id.replace("check", "hdConsigneeName")).value;
            var ConsigneeTelephone = document.getElementById(rb.id.replace("check", "hdConsigneeTelephone")).value;
            var ConsigneeAddress = document.getElementById(rb.id.replace("check", "hdConsigneeAddress")).value;
            var ConsigneeCity = document.getElementById(rb.id.replace("check", "hdConsigneeCity")).value;
            var ConsigneeState = document.getElementById(rb.id.replace("check", "hdConsigneeState")).value;
            var ConsigneeCountry = document.getElementById(rb.id.replace("check", "hdConsigneeCountry")).value;
            var ConsigneePinCode = document.getElementById(rb.id.replace("check", "hdConsigneePinCode")).value;
            var ConsigneeEmail = document.getElementById(rb.id.replace("check", "hdConsigneeEmail")).value;

            document.getElementById("<%=txtShipperNameHAWB.ClientID %>").value = ShipperName;
            document.getElementById("<%=txtShipperTelephoneHAWB.ClientID %>").value = ShipperTelephone;
            document.getElementById("<%=txtShipperAddressHAWB.ClientID %>").value = ShipperAddress;
            document.getElementById("<%=txtShipperStateHAWB.ClientID %>").value = ShipperState;
            if (ShipperCountry != "") {
                document.getElementById("<%=ddlShipperCountryHAWB.ClientID %>").value = ShipperCountry;
            }
            else {
                document.getElementById("<%=ddlShipperCountryHAWB.ClientID %>").selectedIndex = 0;
            }
            document.getElementById("<%=txtShipperCityHAWB.ClientID %>").value = ShipperCity;
            document.getElementById("<%=txtShipperPinCodeHAWB.ClientID %>").value = ShipperPinCode;
            document.getElementById("<%=txtShipperEmailHAWB.ClientID %>").value = ShipperEmail;

            document.getElementById("<%=txtConsigneeNameHAWB.ClientID %>").value = ConsigneeName;
            document.getElementById("<%=txtConsigneeTelephoneHAWB.ClientID %>").value = ConsigneeTelephone;
            document.getElementById("<%=txtConsigneeAddressHAWB.ClientID %>").value = ConsigneeAddress;
            document.getElementById("<%=txtConsigneeStateHAWB.ClientID %>").value = ConsigneeState;

            if (ConsigneeCountry != "") {
                document.getElementById("<%=ddlConsigneeCountryHAWB.ClientID %>").value = ConsigneeCountry;
            }
            else {
                document.getElementById("<%=ddlConsigneeCountryHAWB.ClientID %>").selectedIndex = 0;
            }
            document.getElementById("<%=txtConsigneeCityHAWB.ClientID %>").value = ConsigneeCity;
            document.getElementById("<%=txtConsigneePincodeHAWB.ClientID %>").value = ConsigneePinCode;
            document.getElementById("<%=txtConsigneeEmailHAWB.ClientID %>").value = ConsigneeEmail;

        }

        function ValidateHAWBWt() {
            alert("The Given Weight Exceeds Main AWB Total Weight !!");
        }

        function ValidateHAWBPcs() {
            alert("The Given No. of Pieces Exceeds Main AWB Total No. of Pieces !!");
        }

        function ValidateTotalHAWBPcsWt() {
            var table = document.getElementById('<%= gvHAWBDetails.ClientID%>');
            var sum = 0;
            var TotalPcs = document.getElementById('<%= lblMAWBTotPcs.ClientID %>').innerHTML;
            var TotalWt = document.getElementById('<%= lblMAWBTotWt.ClientID %>').innerHTML;
            var TotalHAWBPcs = 0;
            var TotalHAWBWt = 0;
            for (var i = 1; i < table.rows.length; i++) //setting the incrementor=0, but if you have a header set it to 1
            {
                TotalHAWBPcs += parseInt(table.rows[i].cells[2].children[0].value);
                TotalHAWBWt += parseFloat(table.rows[i].cells[3].children[0].value);
            }
            if (TotalHAWBPcs < TotalPcs || TotalHAWBWt < TotalWt) {
                if (confirm("Total HAWB Pcs/Wt are less than AWB Pcs/Wt. Do you want to continue?"))
                { return true; }
                else
                    return false;
            }
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="True">
    </asp:ToolkitScriptManager>
    
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msgLight_New').style.display = 'block';
            document.getElementById('msgFade_New').style.display = 'block';
            //document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msgLight_New').style.display = 'none';
            document.getElementById('msgFade_New').style.display = 'none';
        }

        function callShow1() {
            document.getElementById('blackening_HAWB').style.display = 'block';
            document.getElementById('whitening').style.display = 'block';


        }


        function GetShipperCodeHAWB(obj) {
            var destination = obj.value;

            var objString = destination.split("|");
            var AccountCode = objString[0];
            var AccountName = objString[1];
            var Telephone = objString[2];
            var Address1 = objString[3];
            var Country = objString[4];

            var Adress2 = objString[5];
            var City = objString[6];
            var State = objString[7];
            var Email = objString[8];
            var ZipCode = objString[9];

            if (objString.length > 1) {

                setTimeout(function() {
                    document.getElementById('<%= txtShipperNameHAWB.ClientID%>').value = AccountName;
                    document.getElementById('<%= txtShipperTelephoneHAWB.ClientID%>').value = Telephone.trim();
                    document.getElementById('<%= txtShipperAddressHAWB.ClientID%>').value = Address1;
                    var e = document.getElementById('<%= ddlShipperCountryHAWB.ClientID%>');
                    e.value = Country;
                    obj.value = AccountName;

                    document.getElementById('<%= txtShipperCityHAWB.ClientID%>').value = City;
                    document.getElementById('<%= txtShipperStateHAWB.ClientID%>').value = State;
                    document.getElementById('<%= txtShipperEmailHAWB.ClientID%>').value = Email;
                    document.getElementById('<%= txtShipperPinCodeHAWB.ClientID%>').value = ZipCode;


                }, 0);
            }
            else {

                setTimeout(function() {

                    document.getElementById('<%= txtShipperNameHAWB.ClientID%>').value = "";
                    document.getElementById('<%= txtShipperTelephoneHAWB.ClientID%>').value = "";
                    document.getElementById('<%= txtShipperAddressHAWB.ClientID%>').value = "";

                    document.getElementById('<%= txtShipperCityHAWB.ClientID%>').value = "";
                    document.getElementById('<%= txtShipperStateHAWB.ClientID%>').value = "";
                    document.getElementById('<%= txtShipperEmailHAWB.ClientID%>').value = "";
                    document.getElementById('<%= txtShipperPinCodeHAWB.ClientID%>').value = "";


                }, 0);
            }

        }

        function GetConsigneeCodeHAWB(obj) {
            var destination = obj.value;

            var objString = destination.split("|");
            var AccountCode = objString[0];
            var AccountName = objString[1];
            var Telephone = objString[2];
            var Address1 = objString[3];
            var Country = objString[4];

            var Adress2 = objString[5];
            var City = objString[6];
            var State = objString[7];
            var Email = objString[8];
            var ZipCode = objString[9];

            if (objString.length > 1) {

                setTimeout(function() {
                    document.getElementById('<%= txtConsigneeNameHAWB.ClientID%>').value = AccountName;
                    document.getElementById('<%= txtConsigneeTelephoneHAWB.ClientID%>').value = Telephone.trim();
                    document.getElementById('<%= txtConsigneeAddressHAWB.ClientID%>').value = Address1;
                    var e = document.getElementById('<%= ddlConsigneeCountryHAWB.ClientID%>');
                    e.value = Country;
                    obj.value = AccountName;

                    document.getElementById('<%= txtConsigneeCityHAWB.ClientID%>').value = City;
                    document.getElementById('<%= txtConsigneeStateHAWB.ClientID%>').value = State;
                    document.getElementById('<%= txtConsigneeEmailHAWB.ClientID%>').value = Email;
                    document.getElementById('<%= txtConsigneePincodeHAWB.ClientID%>').value = ZipCode;


                }, 0);
            }
            else {

                setTimeout(function() {

                    document.getElementById('<%= txtConsigneeNameHAWB.ClientID%>').value = "";
                    document.getElementById('<%= txtConsigneeTelephoneHAWB.ClientID%>').value = "";
                    document.getElementById('<%= txtConsigneeAddressHAWB.ClientID%>').value = "";

                    document.getElementById('<%= txtConsigneeCityHAWB.ClientID%>').value = "";
                    document.getElementById('<%= txtConsigneeStateHAWB.ClientID%>').value = "";
                    document.getElementById('<%= txtConsigneeEmailHAWB.ClientID%>').value = "";
                    document.getElementById('<%= txtConsigneePincodeHAWB.ClientID%>').value = "";


                }, 0);
            }

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
            left: 31%;
            width: auto;
            height: auto;
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
        .style1
        {
            height: 36px;
        }
    </style>
    
    <div id="contentarea">
        <h1>
            <asp:Label ID="lblPageName" runat="server" Text="Quick Booking" />
            <%--Incoming Booking Details--%>
            <%--<img alt="" src="images/bookingdetail_txt.png" />--%></h1>
        <%--<asp:UpdatePanel runat="server" ID="upHeader">
        <ContentTemplate>--%>
        
        <div class="botline">
       
            <table width="100%">
                <tr>
            <td>
            <asp:DropDownList ID="ddlDocType" runat="server" TabIndex="20">
                <%--<asp:ListItem>AWB</asp:ListItem>
                <asp:ListItem>CBV</asp:ListItem>
                <asp:ListItem>CN36</asp:ListItem>
                <asp:ListItem>CN37</asp:ListItem>
                <asp:ListItem>CN56</asp:ListItem>onchange="return GetPartnerCodeforPrefix(this);"--%>
            </asp:DropDownList>
            &nbsp;&nbsp;
            <asp:TextBox ID="txtAwbPrefix" runat="server" MaxLength="3" Width="40px" CssClass="alignrgt"
                AutoPostBack="true" OnTextChanged="txtAwbPrefix_TextChanged" TabIndex="30"></asp:TextBox>
            <asp:AutoCompleteExtender ID="ACEAwbPrefix" BehaviorID="ACEAwbPrefix" runat="server"
                ServiceMethod="GetPartnerPrefix" CompletionInterval="0" EnableCaching="false"
                CompletionSetCount="10" TargetControlID="txtAwbPrefix" MinimumPrefixLength="1"
                OnClientPopulated="onAWBPrefixPopulated">
            </asp:AutoCompleteExtender>
           <%--onChange="javascript:OnReadAWB(this);"OnClick ="btnListAgentStock_Click"--%>
            <asp:TextBox ID="txtAWBNo" runat="server" MaxLength="8" TabIndex="40" CssClass="alignrgt" AutoPostBack="false" onkeypress="return usernameonly(event, false)"></asp:TextBox>
            <%--<asp:TextBox ID="txtAirlineCode" runat="server" ReadOnly="True" Width="30px" CssClass="lablebk"
                TabIndex="200"></asp:TextBox>--%>
            <asp:DropDownList ID="ddlAirlineCode" runat="server" Width="50px" TabIndex="50" 
                    onselectedindexchanged="ddlAirlineCode_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
            &nbsp;<asp:Button ID="btnSearchAWB" runat="server" Text="OK" CssClass="button" OnClick="btnSearchAWB_Click"
                Visible="false" TabIndex="60" />
            &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"
                Visible="false" TabIndex="70" />
            &nbsp;<asp:Button ID="btnListAgentStock" runat="server" Text="List" CssClass="button"
                OnClick="btnListAgentStock_Click" OnClientClick="callShow();" TabIndex="80" />
            &nbsp;<asp:Button ID="btnClearAgentStock" runat="server" Text="Clear" CssClass="button"
                OnClick="btnClearAgentStock_Click" TabIndex="90" /> 
                        <asp:CheckBox ID="chkInterline" runat="server" Text="Interline" Visible="false" TabIndex="10" />
                        &nbsp;
                    <asp:ImageButton ID="imgNotebtn" ImageAlign="AbsMiddle" runat="server" 
                    OnClientClick="javascript:return OpenNotesChild();" ImageUrl="~/Images/noteicon.png" ToolTip="Notes" 
                    Height="20px" Width="34px" style="padding-bottom:2px;" />
                    <asp:ImageButton ID="imgHAWB" runat="server" OnClick="btnHABDetails_Click" ImageUrl="~/Images/StackFiles.png" ToolTip="HAWB" Visible="false" Height="23px" Width="25px" style="vertical-align:bottom;"  />
                </td>    
                
                <td><div style="float:right;">
                <asp:UpdatePanel runat="server">
                <ContentTemplate>
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
            ForeColor="Red"></asp:Label></ContentTemplate></asp:UpdatePanel></div></td>            
                </tr>
            </table>
            
               
        </div>
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
        <div id="divdetail" style="height: auto;">
        
            <div id="colleft" style="height: auto; width:480px;">
                <strong>Consignment Detail</strong>
                <div style="width: 478px;">
                    <asp:UpdatePanel ID="UPFirst" runat="server">
                        <ContentTemplate>
                            <table border="0" cellspacing="2" cellpadding="2" width="100%">
                                <tr>
                                    <td>
                                        Origin*
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrg" runat="server" Width="110px"
                                            TabIndex="100" OnTextChanged="ddlOrg_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        &nbsp;
                                        <br />
                                    </td>
                                    <td>
                                        Dest*
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDest" runat="server" Width="110px" AutoPostBack="false" MaxLength="3"
                                            TabIndex="110" onchange="SetOriginDestination(); SetProcessFlag();">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Commodity Code *
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCommodityCode" runat="server" Width="110px" CssClass="styleUpper"
                                            onchange="GetCommodityCode(this); SetProcessFlag();" TabIndex="120"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="ACECommCode" BehaviorID="ACECommCode" runat="server"
                                                        ServiceMethod="GetCommodityCodesWithName" CompletionInterval="0" EnableCaching="false"
                                                        CompletionSetCount="10" TargetControlID="txtCommodityCode" MinimumPrefixLength="1"
                                                        OnClientPopulated="onCommListPopulated" FirstRowSelected="true">
                                                    </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        Description
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCommodityName" runat="server" Width="110px" Enabled="true" TabIndex="130"></asp:TextBox>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        Agent Code *
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTAgentCode" runat="server" Width="110px" TabIndex="140" CssClass="styleUpper"
                                            onchange="GetAgentCode(this);SetProcessFlag(); return ValidateIACandCCSF();" 
                                            AutoPostBack="true" ontextchanged="TXTAgentCode_TextChanged"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACEAgentCode" BehaviorID="ACEAgentCode" runat="server"
                                            ServiceMethod="GetAgentCodeWithName" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="TXTAgentCode" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated" FirstRowSelected="true">
                                        </asp:AutoCompleteExtender>
                                        &nbsp;
                                        <br />
                                    </td>
                                    <td>
                                       Agent Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAgentName" runat="server" Width="110px" Enabled="true" TabIndex="150"></asp:TextBox>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Prod. Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProductType" runat="server" CssClass="alignrgt" onchange="SetProcessFlag();"
                                            Width="90px" TabIndex="160">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="imgProductType" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="javascript:GetMatchingProductTypes();return false;" 
                                            TabIndex="165" CssClass="InputImageFocus"></asp:ImageButton>
                                    </td>
                                  
                                    <td>
                                        SHC
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpecialHandlingCode" runat="server" onchange="SetProcessFlag();"
                                            TabIndex="170" Width="90px"></asp:TextBox>
                                        <asp:ImageButton ID="ISHC" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="javascript:GetSpecialHandlingCode();return false;"  
                                            TabIndex="175"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Pieces *
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPieces" runat="server" Width="110px" TabIndex="180" onchange="SetProcessFlag();"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers" TargetControlID="txtPieces" />
                                    </td>
                                    <td>
                                        Gross Wt. *
                                        <asp:TextBox ID="txtUOM" runat="server" Width="15px" ReadOnly="true" Font-Bold="true">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGrossWt" runat="server" Width="90px" TabIndex="190" onchange="SetProcessFlag();"></asp:TextBox>
                                        <asp:ImageButton ID="btnDimensionsPopup" runat="server" 
                                            cssclass="InputImageFocus" ImageUrl="~/Images/list_bullets.png"
                                            ImageAlign="AbsMiddle" 
                                            OnClientClick="javascript:dimension(this);return false;" TabIndex="195" />
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers, Custom"
                                            ValidChars="." TargetControlID="txtGrossWt" />
                                    </td>
                                </tr>                                                               
                                
                                <tr>
                                    <td>
                                        Shipper
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipperCode" runat="server" MaxLength="50" TabIndex="200" Width="77px" onchange="SetProcessFlag();GetShipperCode(this);" 
                                         AutoPostBack="false"> </asp:TextBox>
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="javascript:ViewPanel_shipperPopUp();return false;" 
                                            TabIndex="205" CssClass="InputImageFocus" ></asp:ImageButton>
                                        <asp:Image ID="imgShipperCross" runat="server" ImageUrl="~/Images/wrong.png" ImageAlign="AbsBottom" Height="15px" style="display:inline;" />
                                           <asp:Image ID="imgShipperTick" runat="server" ImageUrl="~/Images/right.png" style="display:none;" Height="15px" />
                                        
                                        <asp:AutoCompleteExtender ID="ACESHPCode" runat="server" BehaviorID="ACESHPCode"
                                                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                                                        MinimumPrefixLength="1" ServiceMethod="GetShipperCode" 
                                                        TargetControlID="txtShipperCode" OnClientPopulated="onShipperListPopulated" FirstRowSelected="true">
                                                    </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        Consignee
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConsigneeCode" runat="server" AutoPostBack="false" MaxLength="50"
                                            onchange="SetProcessFlag();GetConsigneeCode(this);" TabIndex="210"
                                            Width="77px"> </asp:TextBox>
                                            <asp:ImageButton ID="ImageButton3" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="javascript:ViewPanel_ConsigneePopUp();return false;" 
                                            TabIndex="215" CssClass="InputImageFocus" ></asp:ImageButton>
                                        <asp:Image ID="imgConCross" runat="server" ImageUrl="~/Images/wrong.png" ImageAlign="AbsBottom" Height="15px" style="display:inline;" />
                                           <asp:Image ID="imgConTick" runat="server" ImageUrl="~/Images/right.png" style="display:none;" Height="15px" />
                                        <asp:AutoCompleteExtender ID="ACEConCode" runat="server" BehaviorID="ACEConCode"
                                                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                                                        MinimumPrefixLength="1" ServiceMethod="GetShipperCode" 
                                                        TargetControlID="txtConsigneeCode" OnClientPopulated="onConsigneeListPopulated" FirstRowSelected="true">
                                                    </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                
                                
                                <tr>
                                <%--<td>Shp Date</td><td> <asp:TextBox runat="server" ID="txtShipmentDate"  Width="90px"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/calendar_2.png"
                                        ImageAlign="AbsMiddle" />
                                    
                                </td>--%>
                                
                                <td>Shipment Date
                                  </td>
                                    <td> 
                                     <%-- <asp:TextBox ID="txtShipmentDate" runat="server" Width="90px" AutoPostBack="true" 
                                        onchange="SetProcessFlag(); callShow();" ontextchanged="txtShipmentDate_TextChanged" TabIndex="216"></asp:TextBox>
                                    <asp:ImageButton ID="imgShipmentDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                        ImageAlign="AbsMiddle" TabIndex="218" />
                                    <asp:CalendarExtender ID="extShipmentDate" Format="dd/MM/yyyy" TargetControlID="txtShipmentDate"
                                        PopupButtonID="imgShipmentDate" runat="server" PopupPosition="BottomLeft">
                                    </asp:CalendarExtender>--%>
                                    <DateControl:CustomDateControl ID="txtShipmentDate1" runat="server" Width="90px" onchange="SetProcessFlag(); callShow();" 
                                        ontextchanged="txtShipmentDate_TextChanged" SetCurrentDate="true" />
                                    </td>
                                    
                                  
                                <td>
                                    Attach Doc.
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAttchDoc" runat="server" Width="90px" TabIndex="222"></asp:TextBox>
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/list_bullets.png"
                                        ImageAlign="AbsMiddle" 
                                        OnClientClick="javascript:GetDocumentList();return false;" TabIndex="223" 
                                        Height="16px" />
                                </td>
                                    
                                    </tr>
                                    <tr>
                                    <td>
                                        Addtl. Info.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipmentPriority" runat="server" Width="90px" Enabled="true"
                                            TabIndex="224"></asp:TextBox>
                                            <asp:ImageButton ID="ImageButton4" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="ViewPanel_CargoPopUp();return false;" TabIndex="225" 
                                            CssClass="InputImageFocus" />
                                    </td>
                                    <td>
                                        Handling Info.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHandling" runat="server" Width="110px" Enabled="true"
                                            TabIndex="226"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                        Packaging Info.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPackingInfo" runat="server" onchange="SetProcessFlag();"
                                            TabIndex="227" Width="90px"></asp:TextBox>
                                        <asp:ImageButton ID="ImageButton5" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="javascript:GetPackagingChargeCodes();return false;"  
                                            TabIndex="228" CssClass="InputImageFocus"/>
                                    </td>
                                    
                                     <td>
                                        DV For Carriage
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTDvForCarriage" runat="server" CssClass="alignrgt" TabIndex="230" onchange="javascript:CheckDVValue(),SetProcessFlag();"
                                            Width="110px"></asp:TextBox>
                                    </td>
                                    
                                </tr>
                                
                                <tr>
                                    <td colspan="3">
                                        <asp:Button ID="btnSaveBooking" runat="server" Text="Save Booking" Visible="false"
                                            CssClass="button" onclick="btnSaveBooking_Click" OnClientClick="callShow();return ProcessRates(this);" TabIndex="260" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save Booking" CssClass="button" OnClick="btnSave_Click"
                                            TabIndex="265" OnClientClick="return ProcessRates(this); return ValidateShipperDetails();return AirlineCodeValidate(this);" />
                                        <asp:Button ID="btnPrintAWB" runat="server" Text="<%$ Resources:LabelNames, LBL_QB_BTN_EAWB %>" OnClick="btnShowEAWB_Click"
                                            Enabled="true" TabIndex="520" OnClientClick="return GetEAWBFlag();" CssClass="button"/>
                                            
                                    </td>                                    
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                
            </div>
            
            <div id="colright" style="width: 500px; height: auto;">
            <asp:UpdatePanel ID="UpdatePanelRouteDetails" runat="server">
                <ContentTemplate>
                    <asp:Label ID="LBLRouteStatus" runat="server" ForeColor="Red"></asp:Label>
                    <asp:HyperLink ID="HyperLink1" runat="server" TabIndex="300" onclick="javascript:ShowHideRouteDetials(); return flase; "><img src="Images/plus.gif" /> <img src="Images/minus.gif" style="display:none;" />&nbsp <strong>Route Details </strong></asp:HyperLink>
                    <div id="Routedetails" style="display:block;" runat="server">
                    <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" CssClass="grdrowfont"
                        ID="grdRouting" Width="100%">
                        <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CHKSelect" runat="server" />
                                    <asp:HiddenField ID="HidScheduleID" runat="server" Value='<%# Eval("ScheduleID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFltOrig" runat="server" Width="35px" CssClass="styleUpper" onchange="javascript:getFlightNumbers(this);"
                                        Text='<%# Eval("FltOrigin") %>' TabIndex="310"> 
                                    </asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dest" HeaderStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFltDest" runat="server" Width="27px" Text='<%# Eval("FltDestination") %>'
                                        AutoPostBack="true" OnTextChanged="txtFltDest_TextChanged" CssClass="styleUpper"
                                        onChange="callShow();" TabIndex="320"><%--TabIndex="39"--%>
                                    </asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interline" Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CHKInterline" runat="server" AutoPostBack="false" TabIndex="330" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Partner Type">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPartnerType" OnSelectedIndexChanged='ddlPartnerType_SelectionChange'
                                        runat="server" AutoPostBack="true" TabIndex="340" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Partner Code">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPartner" OnSelectedIndexChanged='ddlPartner_SelectionChange'
                                        runat="server" AutoPostBack="true" TabIndex="350" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                                <ItemTemplate>
                                    <%--<asp:TextBox ID="txtFdate" runat="server" Width="70px" Text='<%# Eval("FltDate") %>'
                                        AutoPostBack="True" OnTextChanged="txtFdate_TextChanged" onblur="javascript:txtDatefocus();"
                                        onChange="SetProcessFlag();callShow();" CssClass="alignrgt" TabIndex="360"></asp:TextBox>
                                    <asp:CalendarExtender ID="TextBox7_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFdate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>--%>
                                    <DateControl:CustomDateControl ID="txtFdate" runat="server" Width="65px" DateFormatDDMMYYYY='<%# Eval("FltDate") %>'
                                        onChange="txtDatefocus();" OnTextChanged="txtFdate_TextChanged" TabIndex="360" SetCurrentDate="false"
                                        />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Flight" HeaderStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlFltNum" runat="server" Width="70px" onchange="SetProcessFlag();"
                                        TabIndex="370">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="NewFlightID" runat="server" Visible="false" TabIndex="380"></asp:TextBox>
                                    <asp:TextBox ID="txtFlightID" runat="server" Visible="false" AutoPostBack="false"
                                        Width="90px" TabIndex="390"></asp:TextBox>
                                    <asp:ImageButton ID="btnShowFlightPopup" runat="server" ImageUrl="~/Images/list_bullets.png" Enabled="false"
                                        ImageAlign="AbsMiddle" OnClick="btnShowFlights_Click" Width="12" ToolTip="Flight Details"  TabIndex="400"/>
                                    <asp:HiddenField ID="hdnFltNum" runat="server" Value='<%# Eval("FltNumber") %>' />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pcs">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPcs" runat="server" Width="30px" Text='<%# Eval("Pcs") %>' MaxLength="5"
                                        AutoPostBack="true" OnTextChanged="txtPcs_TextChanged1" onChange="callShow();" CssClass="alignrgt" TabIndex="410">                                        
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                        TargetControlID="txtPcs" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Wt">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtWt" runat="server" Width="60px" Text='<%# Eval("Wt","{0:n}") %>'
                                        MaxLength="9" CssClass="alignrgt" TabIndex="420">
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="txtWt" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Chargeable Wt" HeaderStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtChrgWt" runat="server" Width="55px" Text='<%# Eval("ChrgWt","{0:n}") %>'
                                        MaxLength="9" OnTextChanged="txtAccPcsWt_TextChanged" CssClass="alignrgt" TabIndex="430">
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Numbers, Custom" ValidChars="." TargetControlID="txtChrgWt" />
                                    <asp:ImageButton ID="btnRoutePieces" runat="server" ImageUrl="~/Images/list_bullets.png" Width="12" AlternateText="view details" Enabled="false"
                                        ImageAlign="AbsMiddle" OnClientClick="javascript:FlightDetailDimension(this);return false;" ToolTip="Dimensions" TabIndex="440" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                        <FooterStyle CssClass="grdrowfont"></FooterStyle>
                        <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                        <RowStyle CssClass="grdrowfont"></RowStyle>
                    </asp:GridView>
                    <asp:Button ID="btnAddRouteDetails" runat="server" Text="Add" CssClass="button" OnClick="btnAddRouteDetails_Click"
                        OnClientClick="callShow();" TabIndex="450" />
                    <asp:Button ID="btnDeleteRouteDetails" runat="server" Text="Delete" CssClass="button"
                        OnClick="btnDeleteRoute_Click" TabIndex="460" />
                    <asp:Button ID="btnShowFlights" runat="server" Text="Show Flights" CssClass="button"
                        OnClientClick="javascript:showFlights(this);" TabIndex="470" />
                    <asp:Button ID="btnShowCapacity" runat="server" CssClass="button" OnClick="btnShowCapacity_Click"
                        Text="Capacity" Visible="false" TabIndex="480" /></div>
                        
                        </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlDest" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            
            <asp:UpdatePanel ID="updatepanelRateDetails" runat="server">
            <ContentTemplate>
            <div id="divRatesAcceptance" runat="server" style="margin-top:5px; width:550px;">
            <strong>Process Rates <asp:ImageButton ID="imgProcessRates" runat="server" 
                    ImageAlign="AbsMiddle" ImageUrl="~/Images/process.png"
                    width="21" onclick="imgProcessRates_Click" 
                    /></strong>
                     <asp:Label ID="lblRateStatus" runat="server" Font-Bold="false" Font-Size="Small"></asp:Label>
                    <br />
            <div>
            <table>
            <tr><td>
            <div class="bkdark" >
            <table cellpadding="1" cellspacing="1">
            <tr>
                                          <td>Rate
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRatePerKG" runat="server" Width="109px" TabIndex="23" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="btnRateDetails" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/msgInformationsmall.png"/>
                                            </td>               
                                            <td>
                                                <asp:HoverMenuExtender ID="HE1" runat="server" TargetControlID="btnRateDetails" PopupControlID="PopupMenu"
                                                    PopupPosition="left" OffsetX="330" OffsetY="20" PopDelay="50" />
                                                <asp:Panel ID="PopupMenu" runat="server" Style="display: none" BackColor="White" BorderColor="Black" BorderWidth="2px">
                                                    <asp:GridView ID="GrdRateDetails" runat="server" AutoGenerateColumns="False" 
                                                    ShowFooter="false">
                                                        <Columns>
                                                            <asp:BoundField DataField="FltOrigin" HeaderText="Origin">
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FltDestination" HeaderText="Destination">
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FltNumber" HeaderText="Flight No.">
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FltDate" HeaderText="Flight Dt.">
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Pcs" HeaderText="Pieces">
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="GWt" HeaderText="Gross Wt.">
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Chrgbl. Wt.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPopupCWt" runat="server" 
                                                                    Text='<%# Eval("CWt") %>' Width="40px">
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="IsPrime" HeaderText="Is Prime">
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Freight">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPopupFreight" runat="server" MaxLength="10" 
                                                                    Text='<%# Eval("Freight") %>' Width="70px">
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tax">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPopupFreightTax" runat="server" MaxLength="10" 
                                                                    Text='<%# Eval("FreightTax") %>' Width="50px" > </asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rate">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPopupRatePerKg" runat="server" MaxLength="5" 
                                                                    AutoPostBack="true" Text='<%# Eval("RatePerKg") %>' Width="40px" 
                                                                    EnableViewState="true" OnTextChanged="PopupRateChanged" >
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="IsHeavy" HeaderText="Heavy Cargo">
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Proration" HeaderText="Proration">
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AgentComm" HeaderText="Agent Commission">
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Discount" HeaderText="Discount">
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="titlecolr" />
                                                        <RowStyle HorizontalAlign="Center" />
                                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                                    </asp:GridView>
                                                    <asp:Button ID="btnPopupSave" runat="server" Text="Save" CssClass="button" 
                                                        onclick="btnPopupSave_Click" OnClientClick="callShow();" Visible="false"/>
                                                    <asp:Button ID="btnPopupCancel" runat="server" Text="Cancel" CssClass="button" 
                                                        onclick="btnPopupCancel_Click" Visible="false"/>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                            <tr> <td>
                                                Spot Rate
                                            </td><td>
                                                <asp:TextBox ID="txtSpotRate" runat="server" Width="110px" TabIndex="485"></asp:TextBox>
                                            </td> <td></td> <td></td> <td></td>
                                           </tr>
                                            
                                           <tr> 
                                            <td>Spot Id</td><td><asp:TextBox ID="txtspotid" runat="server" Width="110px"  Enabled="false"></asp:TextBox></td>
                                        </tr>
            <tr>
                                            
                                            <td>
                                                Amount Due
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalAmount" runat="server" Width="110px" TabIndex="30" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="ImageButton6" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="ViewPanel_ApplicablechargesPopUp();return false;"  />
                                            </td>
                                            <td></td> <td></td> <td></td> <td></td>
                                            </tr>
                                     <tr>
                                    <td>
                                        Payment Mode
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPaymentMode" runat="server" Width="110px" TabIndex="490">
                                            <asp:ListItem Selected="True">Select</asp:ListItem>
                                            <asp:ListItem>PP</asp:ListItem>
                                            <asp:ListItem>CC</asp:ListItem>
                                            <asp:ListItem>FOC</asp:ListItem>
                                        </asp:DropDownList>&nbsp;<img src="Images/creditcardicon1.png" />
                                    </td><td>
                                        </td>
                                         <td></td> <td></td> <td></td> <td></td>
                                        </tr><tr>
                                    <td>
                                        Currency
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpCurrency" runat="server" Width="110px" Enabled="false" TabIndex="500">
                                        </asp:DropDownList>
                                    </td><td></td>
                                     <td></td> <td></td> <td></td> <td></td>
                                </tr>
                                
                                <tr><td colspan="7">
                                <div width="100%">
                                 <asp:Button ID="btnExecute" runat="server" Text="<%$ Resources:LabelNames, LBL_QB_BTN_FINALIZE %>" CssClass="button ltfloat" OnClick="btnExecute_Click"
                    TabIndex="570" OnClientClick="return CheckQueueStatus();" />
                    <asp:Button ID="btnShowEAWB" runat="server" Text="<%$ Resources:LabelNames, LBL_QB_BTN_EAWB %>" CssClass="button ltfloat" OnClick="btnShowEAWB_Click"
                    Enabled="true" TabIndex="520" OnClientClick="return GetEAWBFlag();"/>
                <asp:Button ID="btnPrintLabels" runat="server" Text="<%$ Resources:LabelNames, LBL_QB_BTN_PRINTLABELS%>" CssClass="button ltfloat"
                    OnClick="btnPrnSelLbl_Click" TabIndex="530"/>
                    </div></td>
                                </tr></table>
             </div>
             </td>
             
             <td valign="top">
              <div class="bkdark" >
             <table cellpadding="1" cellspacing="1">
                
             <tr><td>AWB Status</td><td><asp:DropDownList ID="ddlAWBStatus" runat="server" Width="154px" TabIndex="540">
                </asp:DropDownList></td> <td></td> <td></td></tr>
                <tr><td>Accepted Pcs.</td>
                <td>
                    <asp:TextBox runat="server" Width="50px" ID="txtAccpPieces">
                    </asp:TextBox>&nbsp;&nbsp;Wt.&nbsp;&nbsp;
                <asp:TextBox runat="server" width="50px" ID="txtAccpWeight"></asp:TextBox>
                <asp:ImageButton ID="imgAcceptance" runat="server" ImageAlign="AbsMiddle" 
                                      ImageUrl="~/Images/list_bullets.png" OnClientClick="javascript:dimensionAcceptance();return false;"  
                                      TabIndex="560" />
                </td>
                
                </tr>
             <tr>
             <td></td>
             <td></td>
                          <tr>
                              <td>
                                  Remark</td>
                              <td>
                                  <asp:TextBox ID="txtComment" runat="server" Columns="45" Height="94" Rows="5" 
                                      TabIndex="550" Width="153px" />
                                  <asp:ImageButton ID="ImageButton8" runat="server" ImageAlign="AbsMiddle" 
                                      ImageUrl="~/Images/list_bullets.png" OnClick="btnShowRemarks_Click" 
                                      TabIndex="560" />
                              </td>
                              <td>
                              </td>
                              <td>
                              </td>
                 </tr>
                                      </tr>
             </table>
             
             <asp:Button ID="btnCargoReceipt" runat="server" Text="Cargo Receipt" CssClass="button" Visible="false"
                    OnClick="btnCargoReceipt_Click" TabIndex="580" />
                <asp:Button ID="btnCollect" runat="server" Text="Collect Amount" CssClass="button"
                    Enabled="false" OnClientClick="javascript:return WalkInCollection();" TabIndex="590" />
                    <asp:Button ID="btnSaveAcceptance" runat="server" CssClass="button" Text="Accept" 
                        onclick="btnSaveAcceptance_Click" OnClientClick="return FlightCheckinTime();"/>
                    
               </div>     
             </td>
             
             </tr>
              
             
             </table>
             </div>
             </div>
             
               </ContentTemplate>
               <Triggers>
               <asp:PostBackTrigger ControlID="btnShowEAWB" />
               </Triggers>
                <%--<Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlDest" EventName="SelectedIndexChanged" />
                </Triggers>--%>
            </asp:UpdatePanel>
            
        </div>
            
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
              
                        <div style="display: none; margin-top:15px;" id="DivShipperCon" class="white_content">
                        <strong>Shipper Details</strong>
                            <table cellpadding="3" cellspacing="3" width="100%">
                                <%--<tr>
                                                <td>
                                                    Account Code
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtShipperCode" runat="server" MaxLength="50" TabIndex="16"> </asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="txtShipperCodeExtender" runat="server" 
                                                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                                                        MinimumPrefixLength="1" ServiceMethod="GetShipperCode" 
                                                        TargetControlID="txtShipperCode">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>--%>
                                <tr>
                                    <td>
                                        Name*
                                    </td>
                                    <td>
                                        <asp:TextBox id="TXTShipper" runat="server" AutoPostBack="false" MaxLength="50" TabIndex="1016"> </asp:TextBox>
                                        <%--<asp:AutoCompleteExtender ID="ACEShipper" runat="server" CompletionInterval="0" CompletionSetCount="10"
                                            EnableCaching="false" MinimumPrefixLength="1" ServiceMethod="GetShipper" TargetControlID="TXTShipper">
                                        </asp:AutoCompleteExtender>--%>
                                    </td>
                                    <td>
                                        Telephone#*
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTShipTelephone" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="1017" Width="129px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Address1*
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TXTShipAddress" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="1018" Width="372px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Address2
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TXTShipperAdd2" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="1019" Width="372px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        City
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTShipperCity" runat="server" AutoPostBack="false" MaxLength="20"
                                            TabIndex="1021"></asp:TextBox>
                                    </td>
                                    <td>
                                        State
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTShipperState" runat="server" AutoPostBack="false" MaxLength="21"
                                            TabIndex="1022" Width="130px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Country*
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlShipCountry" runat="server" TabIndex="1023">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Pin Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTShipPinCode" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="1024" Width="129px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Email
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTShipperEmail" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="1025" Width="150px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Known Shipper
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtKnownShipper" runat="server" onchange="return GetShipperDetails(this);"
                                            TabIndex="1026" Width="129px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACEShipperCode" runat="server" BehaviorID="ACEShipperCode"
                                            CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" MinimumPrefixLength="1"
                                            OnClientPopulated="onShipperPopulated" ServiceMethod="GetKnownShipperDetails"
                                            TargetControlID="txtKnownShipper">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                <td>CCSF#
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtShipperCCSF" runat="server" TabIndex="150" Width="150px"></asp:TextBox>
                                                </td>
                                </tr>
                            </table>
                            <%--HidePanel_shipperPopUp()  OnClick="btnListAgentStock_Click"--%>
                            <%--OnClientClick="CheckShipper();return false;"--%>
                <asp:Button ID="Button2" runat="server" Text="Save" CssClass="button" OnClientClick="CheckShipper();HidePanel_shipperPopUp(); return false;" TabIndex="1027"/>
                  
                <asp:Button ID="Button3" runat="server" Text="Cancel" CssClass="button"
                OnClientClick="HidePanel_shipperPopUp();return false;" TabIndex="1028"/>
                        </div>
                         <div id="DivShipperCon1" class="black_overlay"></div>
                        <br />
                       
                        <div style="display: none;" id="Consigneedetails" class="white_content">
                        <strong>Consignee Details</strong>
                            <table cellpadding="3" cellspacing="3" width="100%">
                                <%--<tr>
                                                <td>
                                                    Account Code
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtConsigneeCode" runat="server" AutoPostBack="true" 
                                                        MaxLength="50" onchange="callShow();" OnTextChanged="ShipperCodeDetailsChanged" 
                                                        TabIndex="25"> </asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" 
                                                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                                                        MinimumPrefixLength="1" ServiceMethod="GetShipperCode" 
                                                        TargetControlID="txtConsigneeCode">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>--%>
                                <tr>
                                    <td>
                                        Name*
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTConsignee" runat="server" AutoPostBack="false" MaxLength="50"
                                            TabIndex="1029"></asp:TextBox>
                                        <%--<asp:AutoCompleteExtender ID="ACEConsignee" runat="server" CompletionInterval="0"
                                            CompletionSetCount="10" EnableCaching="false" MinimumPrefixLength="1" ServiceMethod="GetConsignee"
                                            TargetControlID="TXTConsignee">
                                        </asp:AutoCompleteExtender>--%>
                                    </td>
                                    <td>
                                        Telephone#*
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTConTelephone" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="1031" Width="129px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Address1*
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TXTConAddress" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="1032" Width="373px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Address2
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TXTConsigAdd2" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="1033" Width="373px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        City
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTConsigCity" runat="server" AutoPostBack="false" MaxLength="25"
                                            TabIndex="1034"></asp:TextBox>
                                    </td>
                                    <td>
                                        State
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTConsigState" runat="server" AutoPostBack="false" MaxLength="25"
                                            TabIndex="1035" Width="130px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Country*
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlConCountry" runat="server" TabIndex="1036">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Pin Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTConsigPinCode" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="1037" Width="129px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Email
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTConsigEmail" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="1038" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <%--HidePanel_ConsigneePopUp() OnClientClick="CheckConsignee(); return false;"--%>
                             <asp:Button ID="Button5" runat="server" Text="Save" CssClass="button" TabIndex="1039"
                             OnClientClick="CheckConsignee(); HidePanel_ConsigneePopUp(); return false;"/>
                
                <asp:Button ID="Button6" runat="server" Text="Cancel" CssClass="button" OnClientClick="HidePanel_ConsigneePopUp(); return false;" TabIndex="1041"/>
                        </div>
                        
                        <div id="Consigneedetails1" class="black_overlay"></div>
                        <br />
                        
                        <div id="cargodetails" style="display: none; top:15%; width:35%;" class="white_content">
                        <strong>Cargo Details</strong>
                            <table border="0" style="width: 100%" cellpadding="2" cellspacing="2">
                                <tr>
                                    <td>
                                        Shipment Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlShipmentType" runat="server" Width="110px" TabIndex="1042">
                                            <asp:ListItem Selected="True">Bulk</asp:ListItem>
                                            <asp:ListItem>Bags</asp:ListItem>
                                            <asp:ListItem>ULD</asp:ListItem>
                                            <asp:ListItem>Mixed</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                              <td>
                                        Req Screening?
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkTBScreened" runat="server" Checked="true" TabIndex="1043" />
                                    </td>
                                    </tr>
                                    <tr>
                                    
                                    <td>
                                        Service Cargo Class
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlServiceclass" runat="server" TabIndex="1044" OnSelectedIndexChanged="ddlServiceclass_SelectedIndexChanged"
                                            onchange="SetProcessFlag();">
                                            <asp:ListItem Value="1">Void</asp:ListItem>
                                            <asp:ListItem Value="4">FOC</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="5">Cargo</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                      <td>Irreg. Code</td>
                                <td colspan="2">
                                   
                                    <asp:DropDownList ID="ddlIrregularityCode" runat="server" Width="100px" TabIndex="1045">
                                    </asp:DropDownList>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                    Shipment Time
                                </td>
                                <td colspan="2">
                                       <asp:DropDownList ID="ddlShipmentTime" runat="server" TabIndex="1048">
                                        <asp:ListItem Text="Select" Value="25:00" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                </tr>
                                <tr>
                                   
                                    <td colspan="4">
                                        <asp:CheckBox ID="CHKConsole" runat="server" Text="Console" TabIndex="1049" onchange="SetProcessFlag();" />&nbsp;
                                        <asp:CheckBox ID="CHKBonded" runat="server" Text="Bonded" TabIndex="1050" onchange="SetProcessFlag();" />&nbsp;
                                        <asp:CheckBox ID="CHKExportShipment" runat="server" Text="Export Shipment" TabIndex="1051"
                                            OnCheckedChanged="CHKExportShipment_CheckedChanged" onchange="SetProcessFlag();" />
                                        <asp:CheckBox ID="CHKAsAggred" runat="server" Text="As Agreed" TabIndex="1052" />
                                        <asp:CheckBox ID="CHKAllIn" runat="server" Text="AllIn Rate"  />
                                    </td>
                                </tr>
                                <tr><td colspan="4">
                                <hr />
                                </td></tr>
                                <tr>
                                    <td>
                                        DV For Customs
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTDvForCustoms" runat="server" CssClass="alignrgt" onchange="javascript:CheckDVValue(),SetProcessFlag();"
                                            Width="120px" TabIndex="1053"></asp:TextBox>
                                    </td>
                                   <%-- <td>
                                        DV For Carriage
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTDvForCarriage" runat="server" CssClass="alignrgt" TabIndex="1054" onchange="javascript:CheckDVValue(),SetProcessFlag();"
                                            Width="120px"></asp:TextBox>
                                    </td>--%>
                                    <td>
                                        EURIN
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEURIN" runat="server" CssClass="alignrgt" TabIndex="1054" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                 <td>
                                  <asp:Label ID="lblSCI" runat="server" Text="SCI" Visible="true"></asp:Label>
                                </td>
                                <td>
                                   <asp:TextBox ID="txtSCI" runat="server" Width="90px"></asp:TextBox>
                                </td>
                                    <td>
                                        SLAC
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSLAC" runat="server" TabIndex="1055" Width="120px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Customs
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustoms" runat="server" TabIndex="1056" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        IAC Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIACCode" runat="server" TabIndex="1057" Width="120px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACEIACCode" runat="server" BehaviorID="ACEIACCode"
                                            CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" MinimumPrefixLength="1"
                                            OnClientPopulated="onIACPopulated" ServiceMethod="GetIACDetails" TargetControlID="txtIACCode">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                     <td>
                                        CCSF#
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCCSF" runat="server" TabIndex="1058" Width="120px"></asp:TextBox>
                                    </td>
                                    
                                    <%--<td>
                                    Prod. Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProductType" runat="server" CssClass="alignrgt" 
                                        onchange="SetProcessFlag();" Width="120px">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="imgProductType" runat="server" ImageAlign="AbsMiddle" 
                                        ImageUrl="~/Images/list_bullets.png" 
                                        OnClientClick="javascript:GetMatchingProductTypes();return false;" >
                                    </asp:ImageButton>
                                </td>
                                <td>
                                    Special Handling Code
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSpecialHandlingCode" runat="server" CssClass="alignrgt" 
                                        onchange="SetProcessFlag();" TabIndex="14" Width="120px"></asp:TextBox>
                                    <asp:ImageButton ID="ISHC" runat="server" ImageAlign="AbsMiddle" 
                                        ImageUrl="~/Images/list_bullets.png" 
                                        OnClientClick="javascript:GetSpecialHandlingCode();return false;" />
                                </td>--%>
                                    <td>
                                        <asp:CheckBox ID="chkDlvOnHAWB" runat="server" Text="Delivery On HAWB" Visible="false" TabIndex="1059" />
                                    </td>
                                </tr>

                                <tr><td colspan="4"><hr /></td></tr>
                                <tr>
                                <td>
                                    Shp Bill No.
                                </td>
                                <td>
                                    <asp:TextBox ID="txtShippingAWB" runat="server" Width="81px" TabIndex="1061"></asp:TextBox>
                                </td>
                                <td>
                                        GHA
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlHandler" runat="server" Width="100px" Height="23px" TabIndex="1062">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtRemarks" runat="server" Width="30px" Height="23px" TabIndex="1063"
                                            TextMode="MultiLine" MaxLength="35" Visible="false"></asp:TextBox>
                                    </td>
                           
                          
                            </tr>
                            </table>
                         <div class="divback"> 
                            <table cellpadding="3" cellspacing="6" width="100%">
                                <tr>
                                    <td>
                                        Driver Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDriverName" runat="server" onchange="return GetDriverDetails(this);"
                                            TabIndex="1064" Width="120px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACEDriverName" runat="server" BehaviorID="ACEDriverName"
                                            CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" MinimumPrefixLength="1"
                                            OnClientPopulated="onCommListPopulated" ServiceMethod="GetDriverDetails" TargetControlID="txtDriverName">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        Driver DL
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDriverDL" runat="server" TabIndex="1065" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Phone
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPhoneNo" runat="server" TabIndex="1066" Width="120px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Vehicle No#:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVehicleNo" runat="server" TabIndex="1067" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--<tr>
                                
                                <td>
                                        EURIN
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEURIN" runat="server" CssClass="alignrgt" TabIndex="14" Width="120px"></asp:TextBox>
                                    </td>
                                
                                    
                                    <td>
                                        Known Shipper
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtKnownShipper" runat="server" onchange="return GetShipperDetails(this);"
                                            TabIndex="14" Width="120px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACEShipperCode" runat="server" BehaviorID="ACEShipperCode"
                                            CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" MinimumPrefixLength="1"
                                            OnClientPopulated="onShipperPopulated" ServiceMethod="GetKnownShipperDetails"
                                            TargetControlID="txtKnownShipper">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>--%>
                            </table></div>
                            <br />
                               <asp:Button ID="Button7" runat="server" Text="Save" CssClass="button"
                OnClick="btnListAgentStock_Click" OnClientClick="HidePanel_CargoPopUp(); return false;" TabIndex="1068" />
                
                <asp:Button ID="Button8" runat="server" Text="Cancel" CssClass="button"
                OnClientClick="HidePanel_CargoPopUp(); return false;" TabIndex="1069"/>
                        </div>
                        <div id="cargodetails1" class="black_overlay"></div>
                        
                        <br />
                        <div id="Applicablecharges" style="display:none; margin:10px;" class="white_content">
                            <strong>Applicable Charges</strong>
                           <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                   <table border="0" cellspacing="3" cellpadding="3" width="100%">
                                    
                                    <tr>
                                    <td>
                                        Volume *
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVolume" runat="server" Width="110px" TabIndex="1070" onchange="SetProcessFlag();"></asp:TextBox>
                                        
                                        <asp:HiddenField ID="HidRowIndex" runat="server" Value='<%# Eval("RowIndex") %>' />
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers, Custom"
                                            ValidChars="." TargetControlID="txtVolume" />
                                    </td>
                                    <td>
                                        Chargeable Wt.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChargeableWt" runat="server" Width="110px" Enabled="true" TabIndex="1071" onchange="SetProcessFlag();"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers, Custom"
                                            ValidChars="." TargetControlID="txtChargeableWt" />
                                    </td>
                                </tr>
                                    
                                    
                                        <tr>
                                            <td>
                                                Freight IATA
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFreightIATA" runat="server" Width="110px" TabIndex="1072" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                Freight MKT
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFreightMKT" runat="server" Width="110px" TabIndex="1073" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>
                                                OC Due Car
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOCDueCar" runat="server" Width="90px" TabIndex="1076" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="btnOcDueCar" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:getOCDCDetails(this);return false;" TabIndex="1077" CssClass="InputImageFocus" />
                                            </td>
                                            <td>
                                                OC Due Agent
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOCDueAgent" runat="server" Width="90px" TabIndex="1078" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="btnOcDueAgent" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:getOCDADetails(this);return false;" TabIndex="1079" CssClass="InputImageFocus" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Sub Total
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSubTotal" runat="server" Width="110px" TabIndex="1080" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                Service Tax
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServiceTax" runat="server" Width="110px" TabIndex="1081" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Rate Per KG & Class
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRatePerKGInt" runat="server" Width="55px" TabIndex="1082" Enabled="false"></asp:TextBox>
                                                <asp:TextBox ID="txtRateClass" runat="server" Width="50px" TabIndex="1083" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                Currency
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCurrency" runat="server" Width="110px" TabIndex="1084" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                       
                                <tr><td>Account Info</td><td  colspan="3"> <asp:TextBox runat="server" Width="318px"  TabIndex="1085"></asp:TextBox></td>
                                    <td></td><td></td></tr>
                                       <tr>
                                           <td><asp:TextBox runat="server" ID="txtSpotFreight" Visible="false" /></td>
                                           <td><asp:TextBox runat="server" ID="txtSpotTax" Visible="false" /></td>
                                           <td><asp:TextBox runat="server" ID="txtSpotStatus" Visible="false" /></td>
                                          

                                       </tr>
                                    </table>
                                    <asp:Button ID="Button9" runat="server" Text="Cancel" CssClass="button"
                                    OnClientClick="HidePanel_ApplicablechargesPopUp(); return false" TabIndex="1086" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="Applicablecharges1" class="black_overlay"></div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        
        <div id="divSaveAccept" runat="server" style="border-top: 1px solid #a9acb0; margin-top:2px; padding-top:2px; clear: both;">
            <table width="100%">
                <tr><td align="center">
            <asp:Button ID="btnSaveAndAccept" runat="server" Text="Save and Accept" CssClass="button" OnClick="btnSaveAndAccept_Click"
                                            TabIndex="266" OnClientClick="callShow(); return ValidationsOfSaveAccept(this);" />
            </td></tr>
            </table>
        </div>
                 
        <asp:UpdatePanel ID="UpSipperCon" runat="server">
        </asp:UpdatePanel>
        
        <div style="float: left; margin-bottom: 5px;">
            <asp:UpdatePanel ID="UPprocess" runat="server">
                <Triggers>
                    <%--<asp:PostBackTrigger ControlID="btnAddUld" />--%>
                    <asp:PostBackTrigger ControlID="btnHABDetails"></asp:PostBackTrigger>
                </Triggers>
                <ContentTemplate>
                    <div style="font-size: 18pt; width: 721px;">
                        <asp:HiddenField ID="HidProcessFlag" runat="server" />
                        <asp:HiddenField ID="HidEAWB" runat="server" />
                        <asp:HiddenField ID="HidCheckin" runat="server" />
                        <asp:HiddenField ID="HidCCSFFlag" runat="server" />
                        <asp:HiddenField ID="HidOrigin" runat="server" />
                        <asp:HiddenField ID="HidDestination" runat="server" />
                        <asp:HiddenField ID="HidBookingError" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        
        <div class="ltfloat">
        
            <table width="100%">
                            <tr>
                                <td>
                                    Executed By
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExecutedBy" runat="server" Width="100px" Enabled="false">QIDTECH</asp:TextBox>
                                </td>
                                <td>
                                    At
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExecutedAt" runat="server" Width="60px" Enabled="false">PUNE</asp:TextBox>
                                </td>
                                <td>
                                    on
                                </td>
                                <td>
                                    <%--<asp:TextBox ID="txtExecutionDate" runat="server" Width="90px" OnTextChanged="CHKExportShipment_CheckedChanged"
                                        onchange="SetProcessFlag();"></asp:TextBox>
                                    <asp:ImageButton ID="btnExecutionDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                        ImageAlign="AbsMiddle" />
                                    <asp:CalendarExtender ID="CEExecutionDate" Format="dd/MM/yyyy" TargetControlID="txtExecutionDate"
                                        PopupButtonID="btnExecutionDate" runat="server" PopupPosition="BottomLeft">
                                    </asp:CalendarExtender>--%>
                                    <DateControl:CustomDateControl ID="txtExecutionDate1" runat="server" Width="90px" onchange="SetProcessFlag();" 
                                        SetCurrentDate="true" />
                                </td>
                            </tr>
                        </table>
                        </div>
                       
        <div style="border-top: 1px solid #a9acb0; margin-top:5px; padding-top:5px; clear: both;">
               <asp:HyperLink ID="HyperLink2" runat="server" onclick="javascript:ShowHideotherAction(); return flase; "><img src="Images/plus.gif" />&nbsp;<strong>Other Actions</strong></asp:HyperLink>
        
        
        <div id="fotbut" style="width: 100%; border-top: 1px solid #ccc; margin-top:5px; clear: both; display:none;">
       
            <div style="width:245px;  float: left; border-right: 1px solid #ccc; margin-right: 5px;">
                 <asp:Button ID="btnReopen" runat="server" Text="Reopen" CssClass="button" OnClick="btnReopen_Click"
                    TabIndex="58" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button" OnClick="btnDelete_Click"
                    TabIndex="61" />
                 <asp:Button ID="btnSaveTemplate" runat="server" Text="Save as Template" CssClass="button"
                    OnClick="btnSaveTemplate_Click" Enabled="true" TabIndex="63" />
            </div>
            <div style=" float: left; margin-right: 5px; border-right: 1px solid #ccc;">
                <asp:Button ID="btnDgr" runat="server" Text="Dgr" CssClass="button" OnClick="btnDgr_Click" />
                <asp:Button ID="btnHABDetails" runat="server" Text="HAWB" CssClass="button" OnClick="btnHABDetails_Click" />
                <asp:Button ID="btnePouch" runat="server" Text="ePouch" CssClass="button" Enabled="true"
                    TabIndex="60" OnClick="btnePouch_Click" />
            </div>
           
            <div style=" float: left; border-right: 1px solid #ccc;  margin-right: 5px;">
                <asp:Button ID="btnSendFFR" runat="server" Text="Send FFR" CssClass="button" OnClick="btnSendFFR_Click" />
                <asp:Button ID="btnSendFHL" runat="server" Text="Send FHL" CssClass="button" OnClick="btnSendFHL_Click" />
                <asp:Button ID="btnSenfwb" runat="server" Text="Send FWB" CssClass="button" OnClick="btnSenfwb_Click" />
            </div>
            <div style=" float: left;   
                margin-right: 5px;">
                
                <asp:Button ID="btnDGRLbl" runat="server" Text="DGR Label" CssClass="button" OnClick="btnDGRLbl_Click" />
                
               
                <asp:Button ID="btnPrintShipper" runat="server" Text="Print Shipper Dec Form" CssClass="button"
                    Visible="False" />
                <asp:Button ID="sendIATA" runat="server" Text="Send IATA Message" CssClass="button"
                    Visible="false" />
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" TabIndex="62"
                    PostBackUrl="~/Home.aspx" Visible="false" />
                <asp:Button ID="btnfinalize" runat="server" Text="OLD Finalize" CssClass="button"
                    Visible="False" />
                <asp:Button ID="btnGenerateTracer" runat="server" Text="Generate Tracer" CssClass="button"
                    Visible="False" />
                    <td>
             <asp:Button ID="btnPrint" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_ACCEPTUCR %>" 
             OnClientClick="javascript:popupUCR();return false;" />

         </td>
            </div>
                                    
                                    <div id="fadeDiv" class="black_overlay">
                                    </div>
                                
        </div></div>
        
        <asp:UpdatePanel ID="HfUpdatePanel" runat="server">
        <ContentTemplate>
        <div visible="false">
            <asp:HiddenField ID="HidAWBP1" runat="server" />
            <asp:HiddenField ID="HidAWBP2" runat="server" />
            <asp:HiddenField ID="HidSource" runat="server" />
            <asp:HiddenField ID="HidDest" runat="server" />
            <asp:HiddenField ID="HidPcsCount" runat="server" />
            <asp:HiddenField ID="HidVia" runat="server" />
            <asp:HiddenField ID="HidWt" runat="server" />
            <asp:HiddenField ID="HidDimension" runat="server" />
            <asp:HiddenField ID="HidFlightsChanged" runat="server" />
            <asp:HiddenField ID="HidAWBNumber" runat="server" />
            <asp:HiddenField ID="HidChangeDate" runat="server" />
            <asp:HiddenField ID="HidIsManual" runat="server" />
            <asp:HiddenField ID="hfInvoiceNos" runat="server" Value="" />
            <asp:HiddenField ID="HidAWBPieces" runat="server" />
            <asp:HiddenField ID="HdnPcsForDGRLbl" runat="server" />
            <asp:HiddenField ID="HdnUNIDForDGRLbl" runat="server" />
            <asp:HiddenField ID="hdnBookingType" runat="server" />
            <asp:HiddenField ID="hdnRouteDetVisible" runat="server" />
            <asp:HiddenField ID="HidAWBPrefix" runat="server" Value=""/>
            <asp:HiddenField ID="HidAcceptance" runat="server" Value=""/>
            <asp:HiddenField ID="hdUCRNo" runat="server" />
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
        
        
        <%--</div>--%>
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
            <%--</div>--%>
        </div>
        <div id="msgfade" class="black_overlaymsg">
        </div>
        <div id="Lightsplit" class="white_content" style="width: 600px; left: 27%;">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblMsgType" runat="server" Text="Message Type :" ForeColor="Blue"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Blue"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblComm" runat="server" Text="Message Communication Type :" ForeColor="Blue"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMsgCommType" runat="server" Text="" ForeColor="Blue"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblEmail" runat="server" Text="To Email ID (Comma Seprated EmailID):"
                            ForeColor="Blue"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%">
                <tr>
                    <td>
                        <asp:TextBox ID="txtEmailID" runat="server" TextMode="MultiLine" Width="300px" Height="50px"> </asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtSITAHeader" runat="server" TextMode="MultiLine" Visible="false"
                            Height="50px" Width="600px" Style="overflow: auto"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtMessageBody" runat="server" TextMode="MultiLine" Height="300px"
                            Width="600px" Style="overflow: auto"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnOK" CssClass="button" runat="server" Text="Send Email" OnClick="btnOK_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnSitaUpload" CssClass="button" runat="server" Text="Send via SITA"
                            OnClick="btnSitaUpload_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnFTPUpload" CssClass="button" runat="server" Text="FTP Upload"
                            OnClick="btnFTPUpload_Click" />
                    </td>
                    <td>
                        <input type="button" id="btnCancel" class="button" value="Cancel" onclick="HidePanelSplit();" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="fadesplit" class="black_overlay">
        </div>
        <div id="blackening" class="black_overlay">
        </div>
        
        <asp:UpdatePanel ID="upViability" runat="server">
            <ContentTemplate>
                <div id="dvViability" class="white_content" style="width: 600px; left: 27%;">
                    <asp:Label ID="lblEMsg" runat="server" Text=''></asp:Label>
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="GrDVia" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                    Style="z-index: 1">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="check" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sr #">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Origin">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTOrigin" runat="server" Width="65px" Text='<%# Eval("Origin") %>'
                                                    CssClass="grdrowfont"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Destination">
                                            <ItemTemplate>
                                                <asp:TextBox ID="dest" runat="server" Width="65px" Text='<%#Eval("Destination") %>'
                                                    CssClass="grdrowfont">
                                                </asp:TextBox></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LegCarrier">
                                            <ItemTemplate>
                                                <asp:TextBox ID="legcarrier" runat="server" Width="65px" Text='<%#Eval("LegCarrier") %>'
                                                    CssClass="grdrowfont">
                                                </asp:TextBox></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Flight No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="flight" runat="server" Width="65px" Text='<%#Eval("Flight") %>'
                                                    CssClass="grdrowfont">
                                                </asp:TextBox></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Charge">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Charge" runat="server" Width="65px" Text='<%#Eval("Charge") %>'
                                                    CssClass="grdrowfont">
                                                </asp:TextBox></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tax">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Tax" runat="server" Width="65px" Text='<%#Eval("ServTax") %>' CssClass="grdrowfont">
                                                </asp:TextBox></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Currency">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Currency" runat="server" Width="65px" Text='<%#Eval("Currency") %>'
                                                    CssClass="grdrowfont">
                                                </asp:TextBox></ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="GrdDynamic" runat="server" AutoGenerateColumns="false" Visible="false">
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <table width="30%">
                        <tr>
                            <td>
                                <asp:Button ID="btnSelect" CssClass="button" runat="server" Text="Select" OnClick="btnViabilitySelect_Click" />
                            </td>
                            <td>
                                <input type="button" id="Button4" class="button" value="CANCEL" onclick="HideViability();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Select Labels PopUp to Print--%>
                <div id="DivSelLbl" class="white_content" style="overflow: auto; height: 170px; width: 310px;">
                    <div style="margin: 5px;">
                        <h1 style="width: 305px;">
                            Select Pieces</h1>
                        <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
                        <%--<asp:Label ID="lblAWBNoForPrintPopUp" runat="server" Visible="false"></asp:Label>&nbsp;&nbsp;
             <asp:Label ID="lblPcsForPrintPopUp" runat="server" Visible="false"></asp:Label>--%>
                        <br />
                        <table cellpadding="3" cellspacing="3">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="radPrnAllLbl" runat="server" GroupName="PrnLblGrp" />
                                </td>
                                <td>
                                    All
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="radPrnSelLbl" runat="server" GroupName="PrnLblGrp" />
                                </td>
                                <td>
                                    From:<asp:TextBox ID="txtPrnLblFrom" runat="server" Width="65px"></asp:TextBox>
                                </td>
                                <td>
                                    To:<asp:TextBox ID="txtPrnLblTo" runat="server" Width="65px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Button ID="btnPrintSelLbl" runat="server" CssClass="button" Text="Print" OnClick="btnPrintSelLbl_Click" />
                                    <asp:Button ID="btnPrintSelDGRLbl" runat="server" CssClass="button" Text="Print DGR"
                                        OnClick="btnPrintSelDGRLbl_Click" />
                                    <asp:Button ID="btnCancelPopUp" runat="server" CssClass="button" Text="Cancel" OnClientClick="javascript:return HidePanelSplit_SelLbl();" />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </div>
                <div id="DivSelLbl1" class="black_overlay">
                </div>
                
                 <div id="msgLight_New" class="white_contentmsg">
            <table>
                <tr>
                    <td width="5%" align="center">
                        <br />
                        <img src="Images/loading.gif" />
                        <br />
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <%--</div>--%>
        </div>
        <div id="msgFade_New" class="black_overlaymsg">
                
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPrintSelLbl" />
            </Triggers>            
        </asp:UpdatePanel>
        </div>
    <asp:HiddenField ID="hdAWBShipmentType" runat="server" />
    
          <div id="blackening_HAWB" class="black_overlay">
  </div>

    <div id="whitening" class="white_content_HAWB">
            <asp:Label ID="Label4" runat="server" Text=''></asp:Label>
            
            <div style="overflow: auto; width: 600px;">
                <table width="100%">
                    <tr>
                        <td>
                            AWB No : &nbsp;
                                       <asp:Label ID="lblMAWB" runat="server" Text="-"></asp:Label>
                        </td>
                        <td>
                            Total Pieces : &nbsp;
                            <asp:Label ID="lblMAWBTotPcs" runat="server" Text="-"></asp:Label>
                        </td>
                        <td>
                            Total Weight : &nbsp;
                            <asp:Label ID="lblMAWBTotWt" runat="server" Text="-"></asp:Label>
                        </td>
                        <td>
                        UOM : &nbsp;
                        <asp:Label ID="lblUOMHAWB" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            
            <asp:GridView ID="gvHAWBDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False" Width="700px">
                <Columns>
                    <asp:TemplateField>

                        <ItemTemplate>
                            <asp:RadioButton ID="check" runat="server" onclick="javascript:RadioCheck(this);PopulateShipperConsigneeDetailsHAWB(this);" />
                        </ItemTemplate>
                    </asp:TemplateField>
         
                    <asp:TemplateField HeaderText="HAWB #">
                        <ItemTemplate>
                            <asp:TextBox ID="lblHAWBNo" runat="server" Text='<%# Eval("HAWBNo") %>' Width="100px" MaxLength="16"></asp:TextBox>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HAWB Pcs">
                        <ItemTemplate>
                            <asp:TextBox ID="lblHAWBPcs" runat="server" Text='<%# Eval("HAWBPcs") %>' Width="60px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HABWt">
                        <ItemTemplate>
                            <asp:TextBox ID="lblHAWBWt" runat="server" Text='<%# Eval("HAWBWt") %>'  Width="50px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description" FooterStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:TextBox ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Shipper Name">
                        <ItemTemplate>
                            <asp:TextBox ID="lblCustName" runat="server" Text='<%# Eval("CustName") %>' Width="100px" onchange="javascript:GetShipperCodeHAWB(this);"></asp:TextBox>
                              <asp:AutoCompleteExtender ID="ACESHPCodeHAWB" runat="server" 
                              CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                              MinimumPrefixLength="1" ServiceMethod="GetShipperCode" 
                              TargetControlID="lblCustName"  CompletionListCssClass="CompletionListCssClass">
                            </asp:AutoCompleteExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                                      
                    <asp:TemplateField HeaderText="Con. Name">
                        <ItemTemplate>
                            <asp:TextBox ID="lblConsigneeName" runat="server" Text='<%# Eval("ConsigneeName") %>' Width="100px" onchange="javascript:GetConsigneeCodeHAWB(this);"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="ACEConCodeHAWB" runat="server" 
                            CompletionInterval="0" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="10" EnableCaching="false" 
                            MinimumPrefixLength="1" ServiceMethod="GetShipperCode" 
                           TargetControlID="lblConsigneeName">
                          </asp:AutoCompleteExtender>
                        </ItemTemplate>
                     
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Origin">
                        <ItemTemplate>
                            <asp:TextBox ID="lblOrigin" runat="server" Text='<%# Eval("Origin") %>' Width="50px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Destination">
                        <ItemTemplate>
                            <asp:TextBox ID="lblDestination" runat="server" Text='<%# Eval("Destination") %>' Width="50px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SHC">
                        <ItemTemplate>
                            <asp:TextBox ID="lblSHC" runat="server" Text='<%# Eval("SHC") %>' Width="60px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SLAC">
                        <ItemTemplate>
                            <asp:TextBox ID="lblSLAC" runat="server" Text='<%# Eval("SLAC") %>' Width="60px"></asp:TextBox>
                            
                            <asp:HiddenField ID="hdShipperAddress" runat="server" Value='<%# Eval("CustAddress") %>' />
                            <asp:HiddenField ID="hdShipperCity" runat="server" Value='<%# Eval("CustCity") %>' />
                            <asp:HiddenField ID="hdShipperState" runat="server" Value='<%# Eval("CustState") %>' />
                            <asp:HiddenField ID="hdShipperName" runat="server" Value='<%# Eval("CustName") %>' />
                            <asp:HiddenField ID="hdShipperPinCode" runat="server" Value='<%# Eval("ZipCode") %>' />
                            <asp:HiddenField ID="hdShipperTelephone" runat="server" Value='<%# Eval("ShipperTelephone") %>' />
                            <asp:HiddenField ID="hdShipperCountry" runat="server" Value='<%# Eval("CustCountry") %>' />
                            <asp:HiddenField ID="hdShipperEmail" runat="server" Value='<%# Eval("ShipperEmail") %>' />
                            
                             <asp:HiddenField ID="hdConsigneeAddress" runat="server" Value='<%# Eval("ConsigneeAddress") %>' />
                            <asp:HiddenField ID="hdConsigneeCity" runat="server" Value='<%# Eval("ConsigneeCity") %>' />
                            <asp:HiddenField ID="hdConsigneeState" runat="server" Value='<%# Eval("ConsigneeState") %>' />
                            <asp:HiddenField ID="hdConsigneeName" runat="server" Value='<%# Eval("ConsigneeName") %>' />
                            <asp:HiddenField ID="hdConsigneePinCode" runat="server" Value='<%# Eval("ConsigneePostalCode") %>' />
                            <asp:HiddenField ID="hdConsigneeTelephone" runat="server" Value='<%# Eval("ConsigneeTelephone") %>' />
                            <asp:HiddenField ID="hdConsigneeCountry" runat="server" Value='<%# Eval("ConsigneeCountry") %>' />
                            <asp:HiddenField ID="hdConsigneeEmail" runat="server" Value='<%# Eval("ConsigneeEmail") %>' />
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField Visible="false">
                    <ItemTemplate>
                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"></asp:LinkButton>
                    </ItemTemplate>
                    </asp:TemplateField>
                  
                    
                </Columns>
                <HeaderStyle CssClass="titlecolr" />
                <AlternatingRowStyle CssClass="trcolor" Wrap="False" />
                <EditRowStyle CssClass="grdrowfont" />
                <RowStyle CssClass="grdrowfont" HorizontalAlign="Center" Wrap="False" />
                <FooterStyle CssClass="grdrowfont" />
            </asp:GridView>

            <div id="DivShipperConHAWB" style="background: #FFFFFF;
                 float: left">

                            <table frame="box" width="882px">

                                <tr>
                                <td></td>
                                <td style="font-size: large; font-weight: bold">Shipper</td>
                                <td style="font-size: large; font-weight: bold">Consignee</td>
                                </tr>
                                <tr>
                                    <td>
                                        Name*
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtShipperNameHAWB" runat="server" AutoPostBack="false" MaxLength="50" TabIndex="2000" Width="129px" > </asp:TextBox>
                       
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConsigneeNameHAWB" runat="server" AutoPostBack="false" MaxLength="50" TabIndex="2008"
                                          Width="129px" ></asp:TextBox>


                                    </td>
                                   
                                </tr>
                                <tr>
                                 <td>
                                        Telephone#*
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipperTelephoneHAWB" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="2001" Width="129px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConsigneeTelephoneHAWB" runat="server" AutoPostBack="false" MaxLength="100"
                                             Width="129px" TabIndex="2009"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Address*
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipperAddressHAWB" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="2002" Width="129px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                   <td>
                                        <asp:TextBox ID="txtConsigneeAddressHAWB" runat="server" AutoPostBack="false" MaxLength="100"
                                             TabIndex="2010" Width="129px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        City
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipperCityHAWB" runat="server" AutoPostBack="false" MaxLength="20"
                                            TabIndex="2003" Width="129px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConsigneeCityHAWB" runat="server" AutoPostBack="false" MaxLength="25"
                                            TabIndex="2011" Width="129px"></asp:TextBox>
                                    </td>
                                    </tr>
                                <tr>
                                    <td>
                                        State
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipperStateHAWB" runat="server" AutoPostBack="false" MaxLength="21"
                                             TabIndex="2004" Width="129px"></asp:TextBox>
                                    </td>
                                     <td>
                                        <asp:TextBox ID="txtConsigneeStateHAWB" runat="server" AutoPostBack="false" MaxLength="25"
                                          TabIndex="2012" Width="129px"></asp:TextBox>
                                    </td>
                                     
                                </tr>
                                <tr>
                                    <td>
                                        Country*
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlShipperCountryHAWB" runat="server"  TabIndex="2005" Width="129px">
                                        </asp:DropDownList>
                                        
                                    </td>
                                     <td>
                                        <asp:DropDownList ID="ddlConsigneeCountryHAWB" runat="server"  TabIndex="2013" Width="129px">
                                        </asp:DropDownList>
                                    </td>
                                    </tr>
                                <tr>
                                    <td>
                                        Pin Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipperPinCodeHAWB" runat="server" AutoPostBack="false" MaxLength="100"
                                             TabIndex="2006" Width="129px"></asp:TextBox>
                                    </td>
                                      <td>
                                        <asp:TextBox ID="txtConsigneePincodeHAWB" runat="server" AutoPostBack="false" MaxLength="100"
                                             TabIndex="2014" Width="129px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Email
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipperEmailHAWB" runat="server" AutoPostBack="false" MaxLength="100"
                                             TabIndex="2007" Width="129px"></asp:TextBox>
                                    </td>
                                     <td>
                                        <asp:TextBox ID="txtConsigneeEmailHAWB" runat="server" AutoPostBack="false" MaxLength="100"
                                            TabIndex="2015" Width="129px"></asp:TextBox>
                                    </td>
                                   </tr>
                                 
                            </table>

            <div style="float:left;">
            <asp:Button ID="btnADDHAWB" runat="server" CssClass="button" OnClick="btnADDHAWB_Click"
                                Text="Add HAWB" />
                        <asp:Button ID="btnDeleteAWB" CssClass="button" runat="server" Text="Delete" OnClick="btnDeleteAWB_Click" />
                     <asp:Button ID="btnSaveHAWB" CssClass="button" runat="server" Text="Save" OnClick="btnSaveHAWB_Click" OnClientClick="var b = ValidateTotalHAWBPcsWt(); return b"/>
                    
                    
                        <%--<input type="button" id="btnHAWBCancel" class="button" value="Cancel" onclick="callclose1();" />--%>
                   <asp:Button ID="btnHAWBCancel" CssClass="button" Text="Cancel" runat="server"  OnClick="btnHAWBCancel_Click"/>
                    <asp:FileUpload ID="uploadHAWB" runat="server" Visible="false"/> 
                   
                    <asp:Button ID="btnUpload" runat="server" Text="Upload"  CssClass="button" Visible="false" />
 
        </div>   
                            
        </div>
        
            
     
    </div>
   
   <div style="overflow: auto; height: auto; width: auto;" class="white_content" id="LightDiv">
                                        <asp:Repeater ID="RepDetails" runat="server">
                                            <HeaderTemplate>
                                                <table style="border: 1px solid #1F2B53; width: auto" cellpadding="0">
                                                    <tr style="background-color: #1F2B53; color: White">
                                                        <td colspan="2">
                                                            <b>Remarks</b>
                                                        </td>
                                                    </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr style="background-color: #EBEFF0">
                                                    <td>
                                                        <table style="background-color: #EBEFF0; border-top: 1px dotted #1F2B53; width: auto">
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblComment" runat="server" Text='<%#Eval("comments") %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table style="background-color: #EBEFF0; border-top: 1px dotted #1F2B53; border-bottom: 1px solid #1F2B53;
                                                            width: 500px">
                                                            <tr>
                                                                <td>
                                                                    Post By:
                                                                    <asp:Label ID="lblUser" runat="server" Font-Bold="true" Text='<%#Eval("name") %>' />
                                                                </td>
                                                                <td>
                                                                    Created Date:<asp:Label ID="lblDate" runat="server" Font-Bold="true" Text='<%#Eval("date") %>' />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        <input type="button" id="Button1" class="button" value="Cancel" onclick="HidePanellSplit();" />
                                    </div>
    
</asp:Content>
