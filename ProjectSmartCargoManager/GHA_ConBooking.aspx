<%--

 2012-04-05  vinayak
 2012-05-05  vinayak
 2012-05-23  vinayak
 2012-06-04  vinayak
 2012-06-05  vinayak
 2012-06-12  vinayak
 2012-06-18  vinayak
 2012-06-25  vinayak
 2012-07-06  vinayak Edit/Viewe
 2012-07-09  vinayak Special Commodity
 2012-07-23  vinayak Special Commodity with three charge types
 2012-07-24  vinayak
 2012-07-25  vinayak
 2012-07-30  vinayak
 2012-08-03  vinayak

--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="GHA_ConBooking.aspx.cs" Inherits="ProjectSmartCargoManager.ConBooking_GHA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">

        //Validate Accepted Pieces with Booking Pieces

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

        function DateChange() {
            //alert("Inside Javascript");
            var Ctl = document.getElementById('<%=hdnDateChange.ClientID %>');
            Ctl.value = "1";
            
            return true;
        }

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

        function popup(unid, org, dest, awb, pcs,totpcs,frmpcs,toPcs) {
            
//            var unid = document.getElementById("<%= HdnUNIDForDGRLbl.ClientID %>").value;
//            alert(unid);
//            var org = document.getElementById("<%= ddlOrg.ClientID %>").value;
//            var dest = document.getElementById("<%= ddlDest.ClientID %>").value;
//            var awb = document.getElementById("<%= txtAwbPrefix.ClientID %>").value + document.getElementById("<%= txtAWBNo.ClientID %>").value
//        var pcs=document.getElementById("<%= HdnPcsForDGRLbl.ClientID %>").value;
        
            window.open('DGRLabel.aspx?UNID='+unid+'&AWB='+awb+'&org='+org+'&dest='+dest+'&TotPcs='+totpcs+'&Frm='+frmpcs+'&To='+toPcs, '', 'left=0,top=0,width=1000,height=600,toolbar=0,resizable=0');
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
            var InvList = hfInvNos.value;

            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=WalkIn' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowWalkInAgentInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function WalkInCollection() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;

            var AWBPrefix = document.getElementById('<%= txtAwbPrefix.ClientID %>').value;

            var AWBNo = document.getElementById('<%= HidAWBNumber.ClientID %>').value;
           
            var Origin = document.getElementById('<%= HidSource.ClientID %>').value;

            //Exit Parameter to show Exit Button and Hide "Post To Treasury"
            //window.open("BillingWalkInAgentCollection.aspx?INVNO=" + InvList + '&Type=WalkIn' + '&AWBPrefix=' + AWBPrefix + '&AWBNo=' + AWBNo + '&Origin=' + Origin + '&Exit=true', "", "status=0,toolbar=0, menubar=0, width=1100, height=650");
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

        function onListPopulated() {

            var completionList = $find("AutoCompleteEx").get_completionList();
            completionList.style.width = 'auto';
        }

        function CheckDVValue() {

            var DVCarriage = document.getElementById('<%= TXTDvForCustoms.ClientID %>').value;
            var DVCustom = document.getElementById('<%= TXTDvForCarriage.ClientID %>').value;
            var txts = document.getElementById('<%= grdMaterialDetails.ClientID %>').getElementsByTagName('input');
            var SumChrgblWt = 0;
            for (var i = 0; i < txts.length; i++) {

                if (txts[i].type === 'text') {
                    var TextBox = txts[i].id;
                    var TextBoxVal = 0;

                    if (TextBox.indexOf("txtCommChargedWt") > 0) {
                        TextBoxVal = txts[i].value;
                        if (TextBoxVal == "") {
                            TextBoxVal = 0;
                        }
                        SumChrgblWt += Number(TextBoxVal);
                    }
                }
            }
            document.getElementById('<%= txtSpecialHandlingCode.ClientID %>').value = '';
            if ((DVCarriage / SumChrgblWt) > 199)
                document.getElementById('<%= txtSpecialHandlingCode.ClientID %>').value = 'VAL';
            if ((DVCustom / SumChrgblWt) > 199)
                document.getElementById('<%= txtSpecialHandlingCode.ClientID %>').value = 'VAL';

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

        function onPriorityListPopulated() {

            var completionList = $find("ACEPriCode").get_completionList();
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
        function DownloadHAWB() {
            window.open('Download.aspx?Mode=HAWB', 'Download', 'left=100,top=100,width=800,height=420,toolbar=0,resizable=1');
            callShow1();
            
        }
        function ValidateHAWBWt() {
            alert("The Given Weight Exceeds Main AWB Total Weight !!");
        }

        function ValidateHAWBPcs() {
            alert("The Given No. of Pieces Exceeds Main AWB Total No. of Pieces !!");
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
        function dimensionnew(mybutton) {
           
            var origin = document.getElementById('<%= ddlOrg.ClientID %>').value;
            var Dest = document.getElementById('<%= ddlDest.ClientID %>').value;
            var strValue = mybutton.value;
            var targetcontrol = mybutton.id.replace("btnDimensionsPopup", "ddlMaterialCommCode");

            var DropDownList = document.getElementById(targetcontrol);

            var strValue = DropDownList.value;
            var strAwbNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;
            var strAgentCode = document.getElementById("<%= TXTAgentCode.ClientID %>").value;
            var ExecDt = document.getElementById("<%= txtExecutionDate.ClientID %>").value;
            var shipper = document.getElementById("<%= txtShipperCode.ClientID %>").value;
            var Consignee = document.getElementById("<%= txtConsigneeCode.ClientID %>").value;

            var productList = document.getElementById("<%= ddlProductType.ClientID %>");
            var ProductType = productList.options[productList.selectedIndex].text; ;


            var TxtClientObjectID = mybutton.id.replace("btnDimensionsPopup", "txtCommChargedWt");
            var TxtClientObjectVolID = mybutton.id.replace("btnDimensionsPopup", "txtCommVolWt");

            // var TxtClientObject = 'ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_txtCommChargedWt';
            var grosswt = document.getElementById(mybutton.id.replace("btnDimensionsPopup", "txtCommGrossWt")).value;
            var pcscount = document.getElementById(mybutton.id.replace("btnDimensionsPopup", "TXTPcs")).value;

            if (pcscount == "" || pcscount == "0") {

                alert('Fill Pcs first.');
                return;
            }
            if (grosswt == "" || grosswt == "0") {

                alert('Fill Gross Wt.');
                return;
            }
            var rowindex = 1;
            var HidObject = '<%= HidDimension.ClientID %>';

            window.open('GHA_Dimensions.aspx?awbno=' + strAwbNo + '&commodity=' + strValue + '&TargetTXT=' + TxtClientObjectID + '&Hid=' + HidObject + '&GrossWt=' + grosswt + '&PcsCount=' + pcscount + '&RowIndex=' + rowindex + '&VolumeTXT=' + TxtClientObjectVolID + '&Mode=B' + '&Origin=' + Origin + '&Destination=' + Dest + '&AgtCode=' + strAgentCode + '&Shipper=' + shipper + '&Consignee=' + Consignee + '&Product=' + ProductType + '&ExecDt=' + ExecDt, '', 'left=50,top=50,width=850,height=560,toolbar=0,resizable=0');

            return false;

        }

        function dimension(mybutton) {

            //    code for getting data

            var strValue = mybutton.value;

            //         var i;



            var DropDownList;
            var targetcontrol;
            var strValue;
            try {

                targetcontrol = mybutton.id.replace("btnDimensionsPopup", "ddlMaterialCommCode");
                DropDownList = document.getElementById(targetcontrol);
                strValue = DropDownList.value;
                
            }
            catch (Error) {
                
                targetcontrol = mybutton.id.replace("btnDimensionsPopup", "ddlMaterialCommCode1");
                DropDownList = document.getElementById(targetcontrol);
                strValue = DropDownList.value;
            }

//            var strValue = DropDownList.value;
            var strAwbNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;


            var TxtClientObjectID = mybutton.id.replace("btnDimensionsPopup", "txtCommChargedWt");
            var TxtClientObjectVolID = mybutton.id.replace("btnDimensionsPopup", "txtCommVolWt");

            // var TxtClientObject = 'ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_txtCommChargedWt';
            var grosswt = document.getElementById(mybutton.id.replace("btnDimensionsPopup", "txtCommGrossWt")).value;


            var pcscount = document.getElementById(mybutton.id.replace("btnDimensionsPopup", "TXTPcs")).value;

            if (pcscount == "" || pcscount == "0") {

                alert('Fill Pcs first.');
                return;
            }
            if (grosswt == "" || grosswt == "0") {

                alert('Fill Gross Wt.');
                return;
            }

            //            if (strValue == " Select") {

            //                alert('Select commodity first.');
            //                return;
            //            }

            var rowindex = document.getElementById(mybutton.id.replace("btnDimensionsPopup", "HidRowIndex")).value;

            var HidObject = '<%= HidDimension.ClientID %>';

            window.open('GHA_Dimensions.aspx?awbno=' + strAwbNo + '&commodity=' + strValue + '&TargetTXT=' + TxtClientObjectID + '&Hid=' + HidObject + '&GrossWt=' + grosswt + '&PcsCount=' + pcscount + '&RowIndex=' + rowindex + '&VolumeTXT=' + TxtClientObjectVolID + '&Mode=B', '', 'left=50,top=50,width=810,height=630,toolbar=0,resizable=0');

            return false;

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

            window.open('GHA_Dimensions.aspx?awbno=' + strAwbNo + '&commodity=' + strValue + '&TargetTXT=' + TxtClientObjectID + '&Hid=' + HidObject + '&GrossWt=' + grosswt + '&PcsCount=' + pcscount + '&RowIndex=' + rowindex + '&VolumeTXT=' + TxtClientObjectVolID + '&FltNo=' + FlightNo + '&FltDate=' + FlightDate + '&Mode=B', '', 'left=50,top=50,width=810,height=430,toolbar=0,resizable=0');

            return false;

        }


        function AWBPieces(mybutton) {

            //    code for getting data

            var strValue = mybutton.value;

            //         var i;

            var targetcontrol = mybutton.id.replace("btnPiecesPopup", "ddlMaterialCommCode");

            var DropDownList = document.getElementById(targetcontrol);

            strValue = DropDownList.value;
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

            var grosswt = document.getElementById('ctl00_ContentPlaceHolder1_grdRouting_ctl02_txtPcs').value;
            var pcscount = document.getElementById('ctl00_ContentPlaceHolder1_grdRouting_ctl02_txtWt').value;
            var drpdwnflt = document.getElementById('ctl00_ContentPlaceHolder1_grdRouting_ctl02_ddlFltNum').value;

            var AgentCode = document.getElementById('ctl00_ContentPlaceHolder1_TXTAgentCode').value;

            var bookingType = document.getElementById("<%= hdnBookingType.ClientID %>").value;
            var Ship = document.getElementById("<%= HidShipper.ClientID %>").value;
            var Consignee = document.getElementById("<%= HidConsignee.ClientID %>").value;

            if (bookingType == "N") {
                if (AgentCode == "") {
                    alert('Agent code is mandatory.');
                    callclose();
                    return false;
                }
            }

            if (pcscount == "" || pcscount == "0") {
                alert('Fill Pcs first.');
                callclose();
                return false;
            }
            if (grosswt == "" || grosswt == "0") {
                alert('Fill Gross Weight first.');
                callclose();
                return false;
            }

            if (Ship == "1") {
            
                if (document.getElementById("<%= TXTShipper.ClientID %>").value == "") {
                    alert('Shipper Name is mandatory.');
                    callclose();
                    document.getElementById("<%= TXTShipper.ClientID %>").focus();
                    return false;
                }

                if (document.getElementById("<%= TXTShipTelephone.ClientID %>").value == "") {
                    alert('Shipper Telephone number is mandatory.');
                    callclose();
                    document.getElementById("<%= TXTShipTelephone.ClientID %>").focus();
                    return false;
                }

                if (document.getElementById("<%= TXTShipAddress.ClientID %>").value == "") {
                    alert('Shipper Address is mandatory.');
                    callclose();
                    document.getElementById("<%= TXTShipAddress.ClientID %>").focus();
                    return false;
                }
                
                var ddlCon = document.getElementById("<%= ddlShipCountry.ClientID %>");
                if (ddlCon.options[ddlCon.selectedIndex].value == "") {
                    alert('Shipper Country is mandatory.');
                    callclose();
                    document.getElementById("<%= ddlShipCountry.ClientID %>").focus();
                    return false;
                }
            }
            if (Consignee == "1") {
                if (document.getElementById("<%= TXTConsignee.ClientID %>").value == "") {
                    alert('Consignee Name is mandatory.');
                    callclose();
                    document.getElementById("<%= TXTConsignee.ClientID %>").focus();
                    return false;
                }

                if (document.getElementById("<%= TXTConTelephone.ClientID %>").value == "") {
                    alert('Consignee Telephone number is mandatory.');
                    callclose();
                    document.getElementById("<%= TXTConTelephone.ClientID %>").focus();
                    return false;
                }

                if (document.getElementById("<%= TXTConAddress.ClientID %>").value == "") {
                    alert('Consignee Address is mandatory.');
                    callclose();
                    document.getElementById("<%= TXTConAddress.ClientID %>").focus();
                    return false;
                }

                var ddlCon = document.getElementById("<%= ddlConCountry.ClientID %>");
                if (ddlCon.options[ddlCon.selectedIndex].value == "") {
                    alert('Consignee Country is mandatory.');
                    callclose();
                    document.getElementById("<%= ddlConCountry.ClientID %>").focus();
                    return false;
                }
                
            }

            return true;
        }
        function SetAWBType() {
            var skillsSelect = document.getElementById('ctl00_ContentPlaceHolder1_ddlProductType');
            var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;

            var textToFind = 'AWB';
            if (selectedText == 'POMAIL') {
                textToFind = 'CN38';
             }
               

                var dd = document.getElementById('ctl00_ContentPlaceHolder1_ddlDocType');
                for (var i = 0; i < dd.options.length; i++) {
                    if (dd.options[i].text === textToFind) {
                        dd.selectedIndex = i;
                        break;
                    
                }
             }
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

            var commcode = document.getElementById(button.id.replace("btnOcDueAgent", "TXTCommCode")).value;

            var TxtChargeID = button.id.replace("btnOcDueAgent", "TXTOcDueAgent");
            var TxtTaxID = button.id.replace("btnOcDueAgent", "TXTServiceTax");
            var TxtTotalID = button.id.replace("btnOcDueAgent", "TXTTotal");
            var e = document.getElementById('<%= drpCurrency.ClientID %>');
            var curr = e.options[e.selectedIndex].text;
            var origin = document.getElementById('<%= ddlOrg.ClientID %>').value;
            var dest = document.getElementById('<%= ddlDest.ClientID %>').value;

            var TxtSubTotalID = button.id.replace("btnOcDueAgent", "TXTTotal");
            var AWBNo = document.getElementById("<%=txtAWBNo.ClientID %>").value;

            //javascript: window.open('ViewInfoInGrid.aspx?type=DA&CommCode=' + commcode + '&TxtChargeID=' + TxtChargeID + '&TxtTaxID=' + TxtTaxID + '&TxtTotalID=' + TxtTotalID + '&Currency=' + curr + '&Org=' + origin +'&Dest=' + dest + '', '', 'location=no,left=200,top=200,width=630,height=330,toolbar=0,resizable=0');
            javascript: window.open('ViewInfoInGrid.aspx?type=DA&CommCode=' + commcode + '&TxtSubTotalID=' + TxtSubTotalID + '&TxtChargeID=' + TxtChargeID + '&TxtTaxID=' + TxtTaxID + '&TxtTotalID=' + TxtTotalID + '&Currency=' + curr + '&Org=' + origin + '&Dest=' + dest +'&AWBNo=' +AWBNo+'', '', 'location=no,left=200,top=200,width=630,height=330,toolbar=0,resizable=0');

            return false;

        }

        function getOCDCDetails(button) {

            var commcode = document.getElementById(button.id.replace("btnOcDueCar", "TXTCommCode")).value;

            var TxtChargeID = button.id.replace("btnOcDueCar", "TXTOcDueCar");
            var TxtTaxID = button.id.replace("btnOcDueCar", "TXTServiceTax");
            var TxtTotalID = button.id.replace("btnOcDueCar", "TXTTotal");
            var e = document.getElementById('<%= drpCurrency.ClientID %>');
            var curr = e.options[e.selectedIndex].text;
            var origin = document.getElementById('<%= ddlOrg.ClientID %>').value;
            var dest = document.getElementById('<%= ddlDest.ClientID %>').value;

            var TxtSubTotalID = button.id.replace("btnOcDueCar", "TXTTotal");
            var AWBNo = document.getElementById("<%=txtAWBNo.ClientID %>").value;

            //javascript: window.open('ViewInfoInGrid.aspx?type=DC&CommCode=' + commcode + '&TxtChargeID=' + TxtChargeID + '&TxtTaxID=' + TxtTaxID + '&TxtTotalID=' + TxtTotalID + '&Currency=' + curr + '&Org=' + origin +'&Dest=' + dest + '', '', 'location=no,left=200,top=200,width=630,height=330,toolbar=0,resizable=0');
            javascript: window.open('ViewInfoInGrid.aspx?type=DC&CommCode=' + commcode + '&TxtSubTotalID=' + TxtSubTotalID + '&TxtChargeID=' + TxtChargeID + '&TxtTaxID=' + TxtTaxID + '&TxtTotalID=' + TxtTotalID + '&Currency=' + curr + '&Org=' + origin + '&Dest=' + dest + '&AWBNo=' + AWBNo + '', '', 'location=no,left=200,top=200,width=630,height=330,toolbar=0,resizable=0');
            
            return false;

        }

        function showFlightsByFltDate(button) {
            
            var Hid = '<%= HidFlightsChanged.ClientID %>';
            var shipdate = document.getElementById('<%= txtShipmentDate.ClientID %>').value;

            var shiptimectrl = document.getElementById('<%= ddlShipmentTime.ClientID %>');

            var shiptime = shiptimectrl.options[shiptimectrl.selectedIndex].text;

            if (shipdate != '')
                shipdate = shipdate + ' ' + shiptime;
            else
                shipdate = '';
            window.open('ShowFlights.aspx?Hid=' + Hid + '&shipdate=' + shipdate + '&SingleRow=Y', '', 
            'left=200,top=200,width=850,height=300,toolbar=0,resizable=0');
            return false;

        }

        function showFlights(button) {
            var Hid = '<%= HidFlightsChanged.ClientID %>';
            var Destination = document.getElementById('<%= ddlDest.ClientID %>').value;
            var shipdate = document.getElementById('<%= txtShipmentDate.ClientID %>').value;
            
            var shiptimectrl = document.getElementById('<%= ddlShipmentTime.ClientID %>');
            
            var shiptime = shiptimectrl.options[shiptimectrl.selectedIndex].text;
            
            if (shipdate != '')
                shipdate = shipdate + ' ' + shiptime;
            else
                shipdate = '';
            window.open('ShowFlights.aspx?Hid=' + Hid + '&Dest=' + Destination + '&shipdate=' + shipdate +'', '', 'left=200,top=200,width=850,height=300,toolbar=0,resizable=0');
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
            DivDisplayStatus = (document.getElementById('ctl00_ContentPlaceHolder1_DivShipperCon').style.display == "block" ? "none" : "block");

            document.getElementById('ctl00_ContentPlaceHolder1_DivShipperCon').style.display = DivDisplayStatus;

            if (DivDisplayStatus == "none") {
                document.getElementById('imgPlus').style.display = "block";
                document.getElementById('imgMinus').style.display = "none";                
            }
            else {
                document.getElementById('imgMinus').style.display = "block";
                document.getElementById('imgPlus').style.display = "none";                
            }
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
                grid.rows[RowIndex].cells[14].children[0].value = grid.rows[RowIndex].cells[7].children[0].value;  //0;
                grid.rows[RowIndex].cells[15].children[0].value = grid.rows[RowIndex].cells[8].children[0].value;  //0;
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
        function funcsum() {
            debugger;
            //alert("Working");

            var table = document.getElementById('<%= grdMaterialDetails.ClientID%>');
            var ddlMainPayType = document.getElementById('<%= ddlServiceclass.ClientID%>');
            var ddlPayMode = document.getElementById('ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_ddlPaymentMode');

            if (ddlMainPayType.options[ddlMainPayType.selectedIndex].text == 'FOC') {
                ddlPayMode.options[ddlPayMode.selectedIndex].text = 'FOC';
                ddlPayMode.disabled = true;
            }
            else {
                ddlPayMode.options[ddlPayMode.selectedIndex].text = 'PP';
                ddlPayMode.disabled = false;
            }



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
            var CustCode = document.getElementById('<%= TXTCustomerCode.ClientID%>');
            var ProcessFlag = document.getElementById('<%= HidProcessFlag.ClientID%>');

            if (destination.value.indexOf("(") > 0) {
                //                if (destination.value.length > 4) {
                var start = destination.value.indexOf("(");
                var end = destination.value.lastIndexOf(")");
                var str = destination.value;
                var CustStart = destination.value.indexOf("-");                
                var CustEND = destination.value.indexOf("$");

                destination.value = str.substring(0, start);
                AgentName.value = str.substring(start + 1, end);
                CustCode.value = str.substring(CustStart + 1, CustEND);
                document.getElementById('ctl00_ContentPlaceHolder1_grdMaterialDetails_ctl02_ddlPaymentMode').value = str.substring(CustEND + 1, str.length);
                //alert(str.substring(CustEND + 1, str.length));
                //                }
            }
        }


        function GetPriorityCode(obj) {
            var destination = obj;
            //var Priority = obj.id.replace('ddlMaterialCommCode', 'txtMaterialCommDesc');
           
            if (destination.value.Length > 0) {
                //                if (destination.value.length > 4) {
                var str = destination.value;
               
                obj.value = str.substring(0, start);
                //document.getElementById(CommodityName).value = str.substring(start + 1, end);
                //document.getElementById(obj.id.replace('ddlMaterialCommCode', 'txtMaterialCommDesc')).focus();
                //ProcessFlag.value = "1";
                //                }
            }
            return false;
        }

        function GetCommodityCode(obj) {
            var destination = obj;
            var CommodityName = obj.id.replace('ddlMaterialCommCode1', 'txtMaterialCommDesc');
            var ProcessFlag = document.getElementById('<%= HidProcessFlag.ClientID%>');

            if (destination.value.indexOf("(") > 0) {
                //                if (destination.value.length > 4) {
                var str = destination.value;
                var start = destination.value.indexOf("(");
                var end = destination.value.indexOf(")");

                obj.value = str.substring(0, start);
                document.getElementById(CommodityName).value = str.substring(start + 1, end);
                document.getElementById(obj.id.replace('ddlMaterialCommCode1', 'txtMaterialCommDesc')).focus();
                ProcessFlag.value = "1";
                //                }
            }
            SetProcessFlag();
            return false;
        }

        function GetShipperCode(obj) {
            var destination = obj;

            if (destination.value.indexOf("(") > 0) {
                var str = destination.value;
                var start = destination.value.indexOf("(");

                obj.value = str.substring(0, start);
            }
            
            return false;
        }

        function GetConsigneeCode(obj) {
            var destination = obj;

            if (destination.value.indexOf("(") > 0) {
                var str = destination.value;
                var start = destination.value.indexOf("(");

                obj.value = str.substring(0, start);
            }
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
            //funcsum();

            return true;
        }

        function GetProcessFlag() {
            var ProcessFlag = document.getElementById('<%= HidProcessFlag.ClientID%>');

            if (ProcessFlag.value == "1") {
                alert("Kindly Process the Rates to proceed !");
                return false;
            }
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
    
     <%--Select EAWB PopUp--%>
     <script type="text/javascript">
         function ViewPanelSplit_EAWBPopUp() {
             document.getElementById('DivEAWBPopUp').style.display = 'block';
             document.getElementById('DivEAWBPopUp1').style.display = 'block';
             return false;
         }
         function HidePanelSplit_EAWBPopUp() {
             document.getElementById('DivEAWBPopUp').style.display = 'none';
             document.getElementById('DivEAWBPopUp1').style.display = 'none';
             return false;
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
            height: 2500px;
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
            var DropDown = document.getElementById('<%= grdRouting.ClientID %>').getElementsByTagName('select');

            for (var i = 0; i < DropDown.length; i++) {
                if (DropDown[i].type === 'select-one') {
                    var AWBStatus = DropDown[i].id;
                    var AWBStatusVal = '';

                    if (AWBStatus.indexOf("ddlStatus") > 0) {
                        AWBStatusVal = document.getElementById(AWBStatus).value;
                        if (AWBStatusVal != 'C') {
                            alert('Shipment is not yet Confirmed.');
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        function sumAWBPackagingDetails() {
            var txts = document.getElementById('<%= grdMaterialDetails.ClientID %>').getElementsByTagName('input');
            var labels = document.getElementById('<%= grdMaterialDetails.ClientID %>').getElementsByTagName('span');

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
            //************* START: Fetch Flight Number selected from route details grid *************** 
            var table = document.getElementById('<%= grdRouting.ClientID%>');
            //Get flight # dropdown.
            var dropdowns = table.rows[1].cells[6].getElementsByTagName('select');
            //Get selected flight # from flight # dropdown
            var flightNum = '';
            if (dropdowns[0].selectedIndex >= 0 && dropdowns[0].options[dropdowns[0].selectedIndex].text != 'Select') {
                flightNum = dropdowns[0].options[dropdowns[0].selectedIndex].text;
            }
            //*************** END: Fetch Flight Number selected from route details grid. **************
            //************* START: Fetch Flight Date selected from route details grid ***************
            //Get flight date textbox.
            var flightDate = table.rows[1].cells[5].children[0].value;
            //************* END: Fetch Flight Date selected from route details grid ***************
            //************* START: Fetch Commodity Code selected from route details grid ***************
            var tableMaterial = document.getElementById('<%= grdMaterialDetails.ClientID%>');
            var commCode = tableMaterial.rows[1].cells[1].children[0].value;
            //************* END: Fetch Commodity Code selected from shipment details grid ***************
            //************* START: Fetch Shipment weight from shipment details grid ***************
            //Get weight textbox.
            var weight = tableMaterial.rows[1].cells[7].children[0].value;
            //************* END: Fetch weight from shipment details grid ***************

            var shipDate = document.getElementById('<%= txtShipmentDate.ClientID %>').value;

            var TxtClientObject = '<%= ddlProductType.ClientID %>';
            window.open('ListSingleSelectProductType.aspx?origin=' + originValue + '&destination=' + destValue +
             '&fltnum=' + flightNum + '&fltdt=' + flightDate + '&commcode=' + commCode +
             '&weight=' + weight + '&shipDate=' + shipDate + '&TxtClientObject=' + TxtClientObject, '',
             'left=' + (screen.availWidth / 5) + ',top=' +
             (screen.availHeight / 8) + ',width=800,height=550,toolbar=0,resizable=0');
        }
    
    </script>
    <script type="text/javascript">
        function PopupDescription(textbox) {
            var TargetControl = textbox.id.replace("btnDescription", "txtDescription");
            var ActText = document.getElementById(TargetControl).value;
            window.open("DescriptionPopup.aspx?DescriptionID=" + TargetControl + "&Text=" + ActText, "Description", "left=0,top=0,width=400,height=350,toolbar=0,resizable=0");
        }

    </script>
    <script type="text/javascript">


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
    
    
    
    
    <div id="contentarea">
        
            <asp:UpdatePanel runat="server"><ContentTemplate>
        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
            ForeColor="Red" CssClass="msg"></asp:Label>
            </ContentTemplate> </asp:UpdatePanel>
            <h1>
        <asp:Label ID="lblPageName" runat="server" Text="Incoming Booking Details" />
        <%--Incoming Booking Details--%>
            <%--<img alt="" src="images/bookingdetail_txt.png" />--%></h1>
        <div class="botline">
            <table>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkInterline" runat="server" Text="Interline" Visible="false" TabIndex="1"  />
                    </td>
                </tr>
            </table>
            <asp:DropDownList ID="ddlDocType" runat="server" TabIndex="2" Enabled="false" ForeColor="Gray">
                <%--<asp:ListItem>AWB</asp:ListItem>
                <asp:ListItem>CBV</asp:ListItem>
                <asp:ListItem>CN36</asp:ListItem>
                <asp:ListItem>CN37</asp:ListItem>
                <asp:ListItem>CN56</asp:ListItem>onchange="return GetPartnerCodeforPrefix(this);"--%>
            </asp:DropDownList>
            &nbsp;&nbsp;
            <asp:TextBox ID="txtAwbPrefix" runat="server" MaxLength="3" Width="40px" 
                  AutoPostBack="true"
                ontextchanged="txtAwbPrefix_TextChanged" TabIndex="10"></asp:TextBox>
            <asp:AutoCompleteExtender ID="ACEAwbPrefix" BehaviorID="ACEAwbPrefix" runat="server"
                                                        ServiceMethod="GetPartnerPrefix" CompletionInterval="0" EnableCaching="false"
                                                        CompletionSetCount="10" TargetControlID="txtAwbPrefix" MinimumPrefixLength="1"
                                                        OnClientPopulated="onAWBPrefixPopulated">
                                                    </asp:AutoCompleteExtender>
            <asp:TextBox ID="txtAWBNo" runat="server" MaxLength="8" TabIndex="15" 
                onChange="javascript:OnReadAWB(this);"></asp:TextBox>
            <%--<asp:TextBox ID="txtAirlineCode" runat="server" ReadOnly="True" Width="30px" CssClass="lablebk"
                TabIndex="200"></asp:TextBox>--%>
            <asp:DropDownList ID="ddlAirlineCode" runat="server" Width="50px" TabIndex="20">
            </asp:DropDownList>
            &nbsp;<asp:Button ID="btnSearchAWB" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_OK %>" CssClass="button" OnClick="btnSearchAWB_Click"
                Visible="false" TabIndex="25" />
            &nbsp;<asp:Button ID="btnClear" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>" CssClass="button" OnClick="btnClear_Click"
                Visible="false" TabIndex="30" />
            &nbsp;<asp:Button ID="btnListAgentStock" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" CssClass="button"
                OnClick="btnListAgentStock_Click" OnClientClick="callShow();" TabIndex="30" />
            &nbsp;<asp:Button ID="btnClearAgentStock" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>" CssClass="button"
                OnClick="btnClearAgentStock_Click" TabIndex="35" />
                <asp:ImageButton ID="imgHAWB" runat="server" OnClick="btnHABDetails_Click" ImageUrl="~/Images/StackFiles.png" ToolTip="HAWB" Visible="false" Height="23px" Width="25px" style="vertical-align:bottom;"  />
        </div> 
        <div id="DivConBook" runat="server" style="overflow:scroll; height:400px; width:1024px;">
        <div id="divdetail">
            <asp:Panel ID="pnlStockDetails" runat="server" Visible="false">
                <div class="divback">
                    <h2>
                        Stock Details
                    </h2>
                    <asp:GridView ID="grdStockAllocation" runat="server" AutoGenerateColumns="False"
                        Width="50%" Height="82px" CellSpacing="2" CellPadding="2">
                        <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="From" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblFrom" runat="server" Text='<%# Eval("AFrom") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True"></HeaderStyle>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblTo" runat="server" Text='<%# Eval("ATo") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True"></HeaderStyle>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Available" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblAvailable" runat="server" Text='<%# Eval("Available") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True"></HeaderStyle>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AWB Type" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblAWBType" runat="server" Text='<%# Eval("AWBType") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True"></HeaderStyle>
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                        <FooterStyle CssClass="grdrowfont"></FooterStyle>
                        <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                        <RowStyle CssClass="grdrowfont"></RowStyle>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <div id="colleft" style="height:168px">
                <h2>
                    <%--<img alt="" src="images/shipmentdetails.png" />--%>Consignment Details
                </h2>
                <div class="divback" style="width: 478px; height: 114px;">
                    <asp:UpdatePanel ID="UPFirst" runat="server">
                        <ContentTemplate>
                            <table border="0" cellspacing="3" cellpadding="3" width="100%">
                                <tr>
                                    <td>
                                        Origin*
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrg" runat="server" Width="110px" onchange="javascript:SetProcessFlag();updateFlightStation(this);"
                                            TabIndex="40" >
                                        </asp:DropDownList>
                                        &nbsp;
                                        <br />
                                    </td>
                                    <td>
                                        Destination*
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlDest" runat="server" Width="110px" AutoPostBack="true" MaxLength="3"
                                            TabIndex="45" OnSelectedIndexChanged="ddlDest_SelectedIndexChanged" onchange="callShow(); SetProcessFlag();">
                                        </asp:DropDownList>
                                        &nbsp;<%--onchange="javascript:updateFlightStation(this);"--%>
                                        <asp:TextBox ID="txtDest" runat="server" Width="105px" AutoPostBack="true" MaxLength="3"
                                            Visible="false" OnTextChanged="txtDest_TextChanged" TabIndex="50" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Agent Code *
                                    </td>
                                    <td>
                                        <%--<asp:DropDownList ID="ddlAgtCode" runat="server" Width="110px" MaxLength="20" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlAgtCode_SelectedIndexChanged">
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="TXTAgentCode" runat="server" Width="110px" TabIndex="55" CssClass="styleUpper"
                                            onchange="GetAgentCode(this);SetProcessFlag();callShow();" 
                                            AutoPostBack="true" ontextchanged="TXTAgentCode_TextChanged"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACEAgentCode" BehaviorID="ACEAgentCode" runat="server"
                                            ServiceMethod="GetAgentCodeWithName" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="TXTAgentCode" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated">
                                        </asp:AutoCompleteExtender>
                                        &nbsp;
                                        <br />
                                    </td>
                                    <td>
                                        Agent Name
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAgentName" runat="server" Width="110px" Enabled="false" TabIndex="60"></asp:TextBox>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        IATA Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTCustomerCode" runat="server" Width="110px" TabIndex="65" Enabled="false"></asp:TextBox>
                                        <br />
                                    </td>
                                    <td>
                                        Currency
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpCurrency" runat="server" Enabled="false" TabIndex="70">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                       <asp:Label ID="lblUOM" runat="server" Text="UOM"></asp:Label>
                                       &nbsp;
                                        <asp:TextBox ID="txtUOM" runat="server" Width="15px" ReadOnly="true" Font-Bold="true">
                                        </asp:TextBox>
                                    </td>
                            </tr></table>
                            <div>
                            <table cellspacing="3" cellpadding="3" width="100%">
                            <tr>
                            <td>
                                <asp:Label ID="lblIAC" runat="server" Text="IAC Code"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp; 
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIACCode" runat="server" TabIndex="140" Width="110px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="ACEIACCode" runat="server" 
                                        BehaviorID="ACEIACCode" CompletionInterval="0" CompletionSetCount="10" 
                                        EnableCaching="false" MinimumPrefixLength="1" 
                                        OnClientPopulated="onIACPopulated" ServiceMethod="GetIACDetails" 
                                        TargetControlID="txtIACCode">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td> <asp:Label ID="lblCCSF" runat="server" Text="CCSF#"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCCSF" runat="server" TabIndex="150" Width="100px"></asp:TextBox>
                                </td>
                                </tr>
                            </table></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <asp:UpdatePanel ID="UpTblRight" runat="server">
                <ContentTemplate>
                    <div id="colright" style="width: 420px; height: 145px">
                        <h2>
                            <%--<img alt="" src="images/txtcargodetails.png" />--%>Cargo Details
                        </h2>
                        <div class="divback" style="width: 499px; height: 114px">
                            <table border="0" style="width: 100%" cellpadding="3" cellspacing="3">
                                <tr>
                                    <td width="120px">
                                        Service Cargo Class
                                    </td>
                                    <td width="120px">
                                        <asp:DropDownList ID="ddlServiceclass" runat="server" TabIndex="80" OnSelectedIndexChanged="ddlServiceclass_SelectedIndexChanged"
                                            onchange="SetProcessFlag();funcsum();">
                                            <asp:ListItem Value="1">Void</asp:ListItem>
                                            <%--<asp:ListItem Value="2">Courier</asp:ListItem>
                                    <asp:ListItem Value="3">Mail</asp:ListItem>--%>
                                            <asp:ListItem Value="4">FOC</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="5">Cargo</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="120px">
                                        GHA
                                    </td>
                                    <td width="220px" valign="top">
                                        <asp:DropDownList ID="ddlHandler" runat="server" Width="130px" Height="23px" TabIndex="90" Enabled="false">
                                        </asp:DropDownList>
                                       
                                    </td>
                                </tr>
                                <tr>
                                <td width="120px">
                                Handling Info
                                </td>
                                <td>
                                 <asp:TextBox ID="txtHandling" runat="server" Width="130px" Height="23px" TabIndex="95"
                                            TextMode="MultiLine" MaxLength="35" Visible="true"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                    <td width="340px" colspan="2">
                                        <asp:CheckBox ID="CHKConsole" runat="server" Text="Console" TabIndex="100" onchange="SetProcessFlag();" Enabled="false" />&nbsp;
                                        <asp:CheckBox ID="CHKBonded" runat="server" Text="Bonded" TabIndex="105" onchange="SetProcessFlag();" Enabled="false" />&nbsp;
                                        <asp:CheckBox ID="CHKExportShipment" runat="server" Text="Export Shipment" TabIndex="110" Visible="false"
                                            OnCheckedChanged="CHKExportShipment_CheckedChanged" onchange="SetProcessFlag();" />
                                        <asp:CheckBox ID="CHKAsAggred" runat="server" Text="As Agreed" TabIndex="115" Enabled="false" />
                                        <asp:CheckBox ID="CHKAllIn" runat="server" Text="AllIn Rate" TabIndex="116" Enabled="false" />
                                         <asp:CheckBox ID="chkTBScreened" runat="server" Text="Req Screening?" Checked="true" TabIndex="155" Enabled="false" />
                                        
                                </tr>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
         <div class="divback" style="width: 100%; height: 60px;  visibility:hidden">
                        <table cellpadding="3" cellspacing="6" width="100%">
                            <tr style="visibility:hidden">
                                <td>
                                    Driver Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDriverName" runat="server" 
                                        onchange="return GetDriverDetails(this);" TabIndex="120" Width="120px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="ACEDriverName" runat="server" 
                                        BehaviorID="ACEDriverName" CompletionInterval="0" CompletionSetCount="10" 
                                        EnableCaching="false" MinimumPrefixLength="1" 
                                        OnClientPopulated="onCommListPopulated" ServiceMethod="GetDriverDetails" 
                                        TargetControlID="txtDriverName">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    Driver DL
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDriverDL" runat="server" TabIndex="125" Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                    Phone
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPhoneNo" runat="server" TabIndex="130" Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                   Vehicle No#:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVehicleNo" runat="server" TabIndex="135" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <%--<td>
                                    IAC Code
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIACCode" runat="server" TabIndex="140" Width="120px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="ACEIACCode" runat="server" 
                                        BehaviorID="ACEIACCode" CompletionInterval="0" CompletionSetCount="10" 
                                        EnableCaching="false" MinimumPrefixLength="1" 
                                        OnClientPopulated="onIACPopulated" ServiceMethod="GetIACDetails" 
                                        TargetControlID="txtIACCode">
                                    </asp:AutoCompleteExtender>
                                </td>--%>
                                Known Shipper
                                
                                <td>
                                    <asp:TextBox ID="txtKnownShipper" runat="server" 
                                        onchange="return GetShipperDetails(this);" TabIndex="145" Width="120px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="ACEShipperCode" runat="server" 
                                        BehaviorID="ACEShipperCode" CompletionInterval="0" CompletionSetCount="10" 
                                        EnableCaching="false" MinimumPrefixLength="1" 
                                        OnClientPopulated="onShipperPopulated" ServiceMethod="GetKnownShipperDetails" 
                                        TargetControlID="txtKnownShipper">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <%--<td>
                                    CCSF#
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCCSF" runat="server" TabIndex="150" Width="120px"></asp:TextBox>
                                </td>--%>
                          
                            </tr>
                        </table>
                        
                    </div>
        <asp:UpdatePanel ID="UpSipperCon" runat="server">
            <ContentTemplate>
            
                <div style="float: left; width:1006px;" >
                   
                    <div class="divback" style="width:100%; margin-top:10px; height: 60px;">
                        <table cellpadding="3" cellspacing="6" width="100%">
                            <tr>
                                <td>
                                    DV For Customs
                                </td>
                                <td>
                                    <asp:TextBox ID="TXTDvForCustoms" runat="server" onchange="javascript:CheckDVValue(),SetProcessFlag();" 
                                        TabIndex="160" Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                    DV For Carriage
                                </td>
                                <td>
                                    <asp:TextBox ID="TXTDvForCarriage" runat="server" onchange="javascript:CheckDVValue(),SetProcessFlag();"  
                                        TabIndex="165" Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                  <asp:Label ID="lblSCI" runat="server" Text="SCI" Visible="true"></asp:Label>
                                </td>
                                <td>
                                   <asp:TextBox ID="txtSCI" runat="server" Width="90px"></asp:TextBox>
                                </td>
                                  <td>
                                  <asp:Label ID="lblPackInfo" runat="server" Text="Packaging Info." Visible="false"></asp:Label>
                                       
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPackingInfo" runat="server" onchange="SetProcessFlag();"
                                             Width="90px" Visible="false"></asp:TextBox>
                                        <asp:ImageButton ID="ImageButton5" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="javascript:GetPackagingChargeCodes();return false;"  
                                            CssClass="InputImageFocus" Visible="false"/>
                                    </td>
                                
                            </tr>
                         
                            <tr>
                            <td>
                                    Special Handling Code
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSpecialHandlingCode" runat="server"  
                                        onchange="SetProcessFlag();" TabIndex="185" Width="120px"></asp:TextBox>
                                    <asp:ImageButton ID="ISHC" runat="server" ImageAlign="AbsMiddle" 
                                        ImageUrl="~/Images/list_bullets.png" TabIndex="190"
                                        OnClientClick="javascript:GetSpecialHandlingCode();return false;" />
                                </td>
                                <td>
                                    Shipment Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtShipmentDate" runat="server" Width="90px" AutoPostBack="true" 
                                        onchange="callShow();" ontextchanged="txtShipmentDate_TextChanged" TabIndex="195"></asp:TextBox>
                                    <asp:ImageButton ID="imgShipmentDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                        ImageAlign="AbsMiddle" TabIndex="200" />
                                    <asp:CalendarExtender ID="extShipmentDate" Format="dd/MM/yyyy" TargetControlID="txtShipmentDate"
                                        PopupButtonID="imgShipmentDate" runat="server" PopupPosition="BottomLeft">
                                    </asp:CalendarExtender>
                                    <asp:DropDownList ID="ddlShipmentTime" runat="server" TabIndex="205">
                                        <asp:ListItem Text="Select" Value="25:00" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Prod. Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProductType" runat="server"  
                                        onchange="SetAWBType();SetProcessFlag();" Width="120px" TabIndex="210" >
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="imgProductType" runat="server" ImageAlign="AbsMiddle" 
                                        ImageUrl="~/Images/list_bullets.png" 
                                        OnClientClick="javascript:GetMatchingProductTypes();return false;" TabIndex="215" >
                                    </asp:ImageButton>
                                </td>
                                
                            </tr>
                            <tr style="visibility:hidden">
                            <td>
                                    <asp:Label ID="lblSLAC" runat="server" Text="SLAC"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSLAC" runat="server" TabIndex="170" Width="120px"></asp:TextBox>
                                </td>
                              
                                <td>
                                    Customs
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustoms" runat="server" TabIndex="175" Width="120px"></asp:TextBox>
                                </td>
                            
                                <td>
                                    EURIN
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEURIN" runat="server"  TabIndex="180" 
                                        Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                 <asp:CheckBox ID="chkDlvOnHAWB" runat="server" Text="Delivery On HAWB" 
                                        Visible="false" TabIndex="220" />
                                </td>
                            </tr>                          
                        </table>
                    </div>
                    <%--<asp:Button ID="btnShipperCon" CssClass="button" runat="server" Text="Add Shipper/Consignee"
                                        OnClientClick="ShowHideShipperDetials(); return false; " TabIndex="15" />--%>
                    <table cellpadding="3"><tr><td valign="middle" >
                        <img id="imgPlus" src="plus.gif" onclick="ShowHideShipperDetials(); return false;" style="display:block;vertical-align:middle; padding-bottom:5px; padding-top:10px;cursor:pointer;" />
                        <img id="imgMinus" src="minus.gif" onclick="ShowHideShipperDetials(); return false;" style="display:none;vertical-align:middle; padding-bottom:5px; padding-top:10px;cursor:pointer;" /></td>
                        <%--<asp:Image ID="imgPlus" ImageUrl="~/Images/plus.gif" runat="server" />
                        <asp:Image ID="imgMinus" ImageUrl="~/Images/Minus.gif" runat="server" />--%>
                        <td>
                        <h2 style=" width:200px !important;"><asp:HyperLink ID="hlnShipperCon" runat="server" 
                            OnClick="ShowHideShipperDetials(); return false; " style="cursor:pointer;" 
                             Text="Add Shipper/Consignee" /></h2>
                      </td>
                       <td valign="middle"> <asp:ImageButton ID="imgcross" runat="server" Height="15px" 
                            ImageUrl="~/Images/Cross.jpg" Width="15px"  /></td><td><asp:Label ID="LBLShipperConStatus" runat="server" ForeColor="Red"></asp:Label></td>
                            </tr>
                            </table>
                    
                    
                </div>
                
                <div id="DivShipperCon" runat="server" 
                    style="background: #FFFFFF; width: 900px; display: none; float: left" >
                    <table style="width: 100%; height: 100%; left: 0%; top: 0%">
                        <tr>
                            <td>
                                <div ID="Div2" style="">
                                    <h2>
                                        Shipper Details
                                    </h2>
                                    <div class="divback" style="width: 450px; height: 200px;">
                                        <table cellpadding="3" cellspacing="3" width="100%">
                                            <tr>
                                                <td>
                                                    Account Code
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtShipperCode" runat="server" MaxLength="50" TabIndex="225" AutoPostBack="true"
                                                    onchange="callShow(); GetShipperCode(this);" OnTextChanged="ShipperCodeDetailsChanged" > </asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="ACESHPCode" runat="server" BehaviorID="ACESHPCode"
                                                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                                                        MinimumPrefixLength="1" ServiceMethod="GetShipperCode" 
                                                        TargetControlID="txtShipperCode" OnClientPopulated="onShipperListPopulated">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Name*
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTShipper" runat="server" AutoPostBack="false" MaxLength="50" 
                                                        OnTextChanged="ShipperConDetailsChenged" TabIndex="230"> </asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="ACEShipper" runat="server" CompletionInterval="0" 
                                                        CompletionSetCount="10" EnableCaching="false" MinimumPrefixLength="1" 
                                                        ServiceMethod="GetShipper" TargetControlID="TXTShipper">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    Telephone#*
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTShipTelephone" runat="server" AutoPostBack="false" 
                                                        MaxLength="100" OnTextChanged="ShipperConDetailsChenged" TabIndex="235" 
                                                        Width="129px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Address1*
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="TXTShipAddress" runat="server" AutoPostBack="false" 
                                                        MaxLength="100" OnTextChanged="ShipperConDetailsChenged" TabIndex="240" 
                                                        Width="372px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Address2
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="TXTShipperAdd2" runat="server" AutoPostBack="false" 
                                                        MaxLength="100" TabIndex="245" Width="372px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    City
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTShipperCity" runat="server" AutoPostBack="false" 
                                                        MaxLength="20" TabIndex="250"></asp:TextBox>
                                                </td>
                                                <td>
                                                    State
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTShipperState" runat="server" AutoPostBack="false" 
                                                        MaxLength="21" TabIndex="255" Width="130px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Country*
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlShipCountry" runat="server" TabIndex="260">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Pin Code
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTShipPinCode" runat="server" AutoPostBack="false" 
                                                        MaxLength="100" TabIndex="265" Width="129px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Email
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTShipperEmail" runat="server" AutoPostBack="false" 
                                                        MaxLength="100" TabIndex="270" Width="150px"></asp:TextBox>
                                                </td>
                                                <td>CCSF#
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtShipperCCSF" runat="server" TabIndex="150" Width="129px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div ID="Div3" style="">
                                    <h2>
                                        Consignee Details
                                    </h2>
                                    <div class="divback" style="width: 450px; height: 200px">
                                        <table cellpadding="3" cellspacing="3" width="100%">
                                            <tr>
                                                <td>
                                                    Account Code
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtConsigneeCode" runat="server" AutoPostBack="true" 
                                                        MaxLength="50" onchange="callShow(); GetConsigneeCode(this);" OnTextChanged="ShipperCodeDetailsChanged" 
                                                        TabIndex="275"> </asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="ACEConCode" runat="server" BehaviorID="ACEConCode"
                                                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                                                        MinimumPrefixLength="1" ServiceMethod="GetConsigneeCode"
                                                        TargetControlID="txtConsigneeCode" OnClientPopulated="onConsigneeListPopulated">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Name*
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTConsignee" runat="server" AutoPostBack="false" 
                                                        MaxLength="50" OnTextChanged="ShipperConDetailsChenged" TabIndex="280"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="ACEConsignee" runat="server" 
                                                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                                                        MinimumPrefixLength="1" ServiceMethod="GetConsignee" 
                                                        TargetControlID="TXTConsignee">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    Telephone#*
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTConTelephone" runat="server" AutoPostBack="false" 
                                                        MaxLength="100" OnTextChanged="ShipperConDetailsChenged" TabIndex="285" 
                                                        Width="129px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Address1*
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="TXTConAddress" runat="server" AutoPostBack="false" 
                                                        MaxLength="100" OnTextChanged="ShipperConDetailsChenged" TabIndex="290" 
                                                        Width="373px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Address2
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="TXTConsigAdd2" runat="server" AutoPostBack="false" 
                                                        MaxLength="100" TabIndex="295" Width="373px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    City
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTConsigCity" runat="server" AutoPostBack="false" 
                                                        MaxLength="25" TabIndex="300"></asp:TextBox>
                                                </td>
                                                <td>
                                                    State
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTConsigState" runat="server" AutoPostBack="false" 
                                                        MaxLength="25" TabIndex="305" Width="130px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Country*
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlConCountry" runat="server" TabIndex="310">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Pin Code
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTConsigPinCode" runat="server" AutoPostBack="false" 
                                                        MaxLength="100" TabIndex="315" Width="129px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Email
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTConsigEmail" runat="server" AutoPostBack="false" 
                                                        MaxLength="100" TabIndex="320" Width="150px"></asp:TextBox>
                                                </td>
                                              <%--<td>CCSF#
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtConsigneeCCSF" runat="server" TabIndex="150" Width="129px"></asp:TextBox>
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                
            </ContentTemplate>
        </asp:UpdatePanel>
        
        <br />
        <div style="float: left">
            <asp:UpdatePanel ID="UPMaterial" runat="server">
                <ContentTemplate>
                    <h2 style="width: 100%">
                        Shipment Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnAddMaterial" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_ADD %>" CssClass="button" OnClick="btnAddMaterial_Click"
                            Visible="false" TabIndex="325" />
                        &nbsp;
                        <asp:Button ID="btnDeleteMaterial" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_DELETE %>" CssClass="button"
                            OnClick="btnDeleteMaterial_Click" Visible="false" TabIndex="340" />
                    </h2>
                    <div>
                        <table frame="void">
                            <tr>
                                <td valign="top" style="vertical-align: top;">
                                    <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" ID="grdMaterialDetails"
                                        Width="100%">
                                        <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CHKSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comm Code *">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlMaterialCommCode" TabIndex="345" runat="server" Width="80px"
                                                        CssClass="styleUpper" onchange="callShow();" OnSelectedIndexChanged="ddlMaterialCommCode_SelectedIndexChanged" AutoPostBack="true" >
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="ddlMaterialCommCode1" TabIndex="345" runat="server" Width="80px"
                                                        CssClass="styleUpper" onchange="return GetCommodityCode(this);">
                                                    </asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="ACECommCode" BehaviorID="ACECommCode" runat="server"
                                                        ServiceMethod="GetCommodityCodesWithName" CompletionInterval="0" EnableCaching="false"
                                                        CompletionSetCount="10" TargetControlID="ddlMaterialCommCode1" MinimumPrefixLength="1"
                                                        OnClientPopulated="onCommListPopulated">
                                                    </asp:AutoCompleteExtender>
                                                </ItemTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comm Desc *">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMaterialCommDesc" runat="server" Width="100px" MaxLength="36"
                                                        TabIndex="350" CssClass="grdrowfont" Text='<%# Eval("CodeDescription") %>'>
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotal" runat="server" Text="Total :"></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pcs *">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTPcs" runat="server" Width="50px" MaxLength="5" Text='<%# Eval("Pieces") %>'
                                                        TabIndex="355"  onchange="javascript:sumAWBPackagingDetails();"> </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotalPcs" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gross Wt *" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCommGrossWt" runat="server" Width="70px" MaxLength="10" onchange="javascript:sumAWBPackagingDetails();"
                                                        Text='<%# Eval("GrossWeight","{0:n}") %>' TabIndex="360" >
                                                    </asp:TextBox>
                                                    
                                                    <asp:ImageButton ID="btnPiecesPopup" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                        Visible="false" ImageAlign="AbsMiddle" TabIndex="365" OnClientClick="javascript:AWBPieces(this);return false;" />
                                                    <asp:HiddenField ID="HidPcsRowIndex" runat="server" Value='<%# Eval("RowIndex") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotalGrWt" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <HeaderStyle Width="300px" />
                                                <ItemStyle HorizontalAlign="Right" Width="250px" />
                                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Piece Dt.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPieceCount" runat="server" Visible="false">
                                                </asp:Label>&nbsp;&nbsp;
                                                <asp:ImageButton ID="btnPiecesPopup" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:AWBPieces(this);return false;" />
                                                <asp:HiddenField ID="HidPcsRowIndex" runat="server" Value='<%# Eval("RowIndex") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Dim.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDimensions" runat="server" Text='<%# Eval("Dimensions") %>' Visible="false">
                                                    </asp:Label>&nbsp;&nbsp;
                                                    <asp:ImageButton ID="btnDimensionsPopup" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                        ImageAlign="AbsMiddle" TabIndex="370"  OnClientClick="javascript:dimension(this);return false;" />
                                                    <asp:HiddenField ID="HidRowIndex" runat="server" Value='<%# Eval("RowIndex") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Volume *">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCommVolWt" runat="server" Width="80px" AutoPostBack="true" Text='<%# Eval("VolumetricWeight") %>'
                                                        OnTextChanged="txtCommVolWt_TextChanged" OnChange="SumVolume(this);" TabIndex="375"
                                                        >
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotalVolume" runat="server" Text=""></asp:Label></FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Chargeable Wt *">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCommChargedWt" runat="server" Width="100px" Text='<%# Eval("ChargedWeight","{0:n}") %>'
                                                        onchange="javascript:sumAWBPackagingDetails();" TabIndex="380" >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotalChargedWt" runat="server" Text=""></asp:Label></FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pay Mode">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlPaymentMode" runat="server" Width="75px" CssClass="grdrowfont"
                                                        onchange="SetProcessFlag();" TabIndex="385">
                                                        <asp:ListItem>Select</asp:ListItem>
                                                        <asp:ListItem Selected="True">PP</asp:ListItem>
                                                        <asp:ListItem>CC</asp:ListItem>
                                                        <asp:ListItem>FOC</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account Info.">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAccountInfo" runat="server" Width="100px" MaxLength="100" Text='<%# Eval("AccountInfo") %>'
                                                        TabIndex="390" Enabled="false" ForeColor="Gray"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Shp. Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlShipmentType" runat="server" Width="60px" CssClass="grdrowfont"
                                                        TabIndex="400">                                                        
                                                        <asp:ListItem Selected="True" Enabled="false">Bulk</asp:ListItem>
                                                        <asp:ListItem>Bulk</asp:ListItem>                                                     
                                                       <%-- <asp:ListItem>Bags</asp:ListItem>
                                                        <asp:ListItem>ULD</asp:ListItem>
                                                        <asp:ListItem>Mixed</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Shp. Priority">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtShipmentPriority" runat="server" Width="100px" MaxLength="10" Text='<%# Eval("ShipmentPriority") %>'
                                                        TabIndex="405" onchange="return GetPriorityCode(this);">
                                                    </asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="ACEPriCode" BehaviorID="ACEPriCode" runat="server"
                                                        ServiceMethod="GetPriorityCodeService" CompletionInterval="0" EnableCaching="false"
                                                        CompletionSetCount="10" TargetControlID="txtShipmentPriority" MinimumPrefixLength="1"
                                                        OnClientPopulated="onPriorityListPopulated">
                                                    </asp:AutoCompleteExtender>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtShipmentRemarks" runat="server" Width="100px" MaxLength="100" Text='<%# Eval("Remarks") %>'
                                                        TabIndex="410"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                                        <FooterStyle CssClass="grdrowfont"></FooterStyle>
                                        <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                                        <RowStyle CssClass="grdrowfont"></RowStyle>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="float: left;width: 100%;">
            <asp:UpdatePanel ID="UpdatePanelRouteDetails" runat="server">
                <ContentTemplate>
                    <asp:Label ID="LBLRouteStatus" runat="server" ForeColor="Red"></asp:Label>
                    <h2 style="width: 600px">
                        Route Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                        
                    </h2>
                    <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" CssClass="grdrowfont" Width="100%"
                         ID="grdRouting">
                        <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CHKSelect" runat="server" TabIndex="415" />
                                    <asp:HiddenField ID="HidScheduleID" runat="server" Value='<%# Eval("ScheduleID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Flight Origin *" HeaderStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFltOrig" runat="server" Width="55px" CssClass="styleUpper" onchange="javascript:getFlightNumbers(this);"
                                        Text='<%# Eval("FltOrigin") %>' TabIndex="420"> <%--TabIndex="38"--%>
                                    </asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Flight Dest*" HeaderStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFltDest" runat="server" Width="55px" Text='<%# Eval("FltDestination") %>'
                                        AutoPostBack="true" OnTextChanged="txtFltDest_TextChanged" CssClass="styleUpper"
                                        onChange="callShow();" TabIndex="425"><%--TabIndex="39"--%>
                                    </asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interline" Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CHKInterline" TabIndex="430" runat="server" AutoPostBack="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Partner Type">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPartnerType" OnSelectedIndexChanged='ddlPartnerType_SelectionChange'
                                        runat="server" AutoPostBack="true" TabIndex="435" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Partner Code">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPartner" OnSelectedIndexChanged='ddlPartner_SelectionChange'
                                        runat="server" AutoPostBack="true" TabIndex="440" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Flight Date *" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFdate" runat="server" Width="70px" Text='<%# Eval("FltDate") %>'
                                        AutoPostBack="True" OnTextChanged="txtFdate_TextChanged" 
                                        onChange="DateChange();SetProcessFlag();callShow();"  TabIndex="445"></asp:TextBox>
                                    <asp:CalendarExtender ID="TextBox7_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFdate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Flight #*" HeaderStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlFltNum" runat="server" Width="90px" onchange="SetProcessFlag();"
                                        TabIndex="450">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="NewFlightID" runat="server" Visible="false" TabIndex="455"></asp:TextBox>
                                    <asp:TextBox ID="txtFlightID" runat="server" Visible="false" AutoPostBack="false" TabIndex="460"
                                        Width="90px"></asp:TextBox>
                                  
                                    <asp:ImageButton ID="btnShowFlightPopup" runat="server" ImageUrl="~/Images/list_bullets.png"
                                        ImageAlign="AbsMiddle" OnClick="btnShowFlights_Click" TabIndex="465" Visible="false"/>
                                    <asp:HiddenField ID="hdnFltNum" runat="server" Value='<%# Eval("FltNumber") %>' />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pcs">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPcs" runat="server" Width="30px" Text='<%# Eval("Pcs") %>' MaxLength="5"
                                        OnTextChanged="txtPcs_TextChanged1" AutoPostBack="true" onChange="callShow();"
                                         TabIndex="470">
                                    </asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Wt">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtWt" runat="server" Width="70px" Text='<%# Eval("Wt","{0:n}") %>'
                                        MaxLength="9"  TabIndex="475">
                                    </asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Chargeable Wt" HeaderStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtChrgWt" runat="server" Width="60px" Text='<%# Eval("ChrgWt","{0:n}") %>'
                                        MaxLength="9" OnTextChanged="txtAccPcsWt_TextChanged"  TabIndex="480">
                                    </asp:TextBox>
                                    <asp:ImageButton ID="btnRoutePieces" runat="server" ImageUrl="~/Images/list_bullets.png" Visible="false"
                                        ImageAlign="AbsMiddle" OnClientClick="javascript:FlightDetailDimension(this);return false;" TabIndex="485" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="100px" TabIndex="490">
                                        <asp:ListItem Text="Confirmed" Value="C"></asp:ListItem>
                                        <asp:ListItem Text="Queued" Value="Q" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Partner Status">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPartnerStatus" runat="server" Width="60px" Text="" MaxLength="5"
                                        Enabled="true" TabIndex="495">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FFR">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkFFR" runat="server" TabIndex="500" Enabled="false"></asp:CheckBox>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Accepted" Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAccepted" runat="server" onclick="javascript:checkChangedNew(this)"
                                        TabIndex="505"></asp:CheckBox>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Accepted Pcs" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAcceptedPcs" runat="server" Width="60px" Text='<%# Eval("AcceptedPcs") %>'
                                        MaxLength="5" Enabled="false"  TabIndex="510">
                                    </asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Accepted Wt" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Width="10px"
                                HeaderStyle-Wrap="true" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAcceptedWt" runat="server" Width="60px" Text='<%# Eval("AcceptedWt","{0:n}") %>'
                                        Enabled="false"  TabIndex="515">
                                    </asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Location" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Width="10px"
                                HeaderStyle-Wrap="true" Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtLocation" runat="server" Width="50px" Enabled="true"  TabIndex="520">
                                    </asp:TextBox>
                                    <asp:Button ID="btnLocation" runat="server" CssClass="button" Text="Add" TabIndex="525" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                        <FooterStyle CssClass="grdrowfont"></FooterStyle>
                        <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                        <RowStyle CssClass="grdrowfont"></RowStyle>
                    </asp:GridView>
                    <asp:Button ID="btnAddRouteDetails" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_ADD %>" CssClass="button" OnClick="btnAddRouteDetails_Click"
                            OnClientClick="callShow();" TabIndex="530" />
                        <asp:Button ID="btnDeleteRouteDetails" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_DELETE %>" CssClass="button"
                            OnClick="btnDeleteRoute_Click" TabIndex="535" />                        
                        <asp:Button ID="btnShowFlights" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SHOWFLIGHT %>" CssClass="button"
                            OnClientClick="javascript:showFlights(this);" TabIndex="540" />
                        <asp:Button ID="btnShowCapacity" runat="server" CssClass="button" OnClick="btnShowCapacity_Click"
                            Text="<%$ Resources:LabelNames, LBL_BTN_CAPACITY %>" Visible="false" TabIndex="545" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlDest" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <div style="float: left">
            <asp:UpdatePanel ID="UPprocess" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnAddUld" />
                    <asp:PostBackTrigger ControlID="btnHABDetails"></asp:PostBackTrigger>
                </Triggers>
                <ContentTemplate>
                    <div style="font-size: 18pt; width: 721px;">
                        <asp:HiddenField ID="HidProcessFlag" runat="server" />                        
                        <asp:HiddenField ID="HidEAWB" runat="server" />                        
                        <asp:HiddenField ID="HidShipper" runat="server" /> 
                        <asp:HiddenField ID="HidConsignee" runat="server" /> 
                       <h2>
                        Rate Details &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                        
                       </h2>
                    </div>
                    <div style="width:100%">
                        <table frame="void" width="100%">
                            <tr>
                                <td valign="top" style="vertical-align: top;">
                                    <asp:Label ID="rateprocessstatus" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    <asp:GridView ID="GRDRates" runat="server" ShowFooter="True" AutoGenerateColumns="False" Width="100%">
                                        <AlternatingRowStyle CssClass="trcolor" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="check" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Com. Code">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTCommCode" runat="server" Width="65px" Text='<%# Eval("CommCode") %>'
                                                        CssClass="grdrowfont" TabIndex="550">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pcs ">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTPcs" runat="server" Text='<%# Eval("Pcs") %>' Width="35px" TabIndex="555" >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotalPcs" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Wt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTWt" runat="server" Text='<%# Eval("Weight","{0:n}") %>' Width="45px"
                                                         TabIndex="560">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotalWt" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="C.Wt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTCWt" runat="server" Text='<%# Eval("ChargedWeight","{0:n}") %>'
                                                        Width="45px" TabIndex="565" >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLCTotalWt" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Freight IATA">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTFrIATA" TabIndex="570" runat="server" Text='<%# Eval("FrIATA","{0:n}") %>' Width="60px">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotalFrIATA" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Freight Mkt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTFrMKT" runat="server" TabIndex="575" Text='<%# Eval("FrMKT","{0:n}") %>' Width="60px"
                                                        >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotalFrMKT" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRatePerKg" runat="server" TabIndex="580" Text='<%# Eval("RatePerKg") %>' Width="40px"
                                                        ReadOnly="true" >
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalRate" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Val Chgs" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTValCharg" runat="server" Text='<%# Eval("ValCharge","{0:n}") %>'
                                                        Width="45px" TabIndex="585" >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pay Mode">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlPayMode" runat="server" Enabled="false" TabIndex="590">
                                                        <asp:ListItem Enabled="true" Selected="True" Value="PP">
                                                        PP
                                                        </asp:ListItem>
                                                        <asp:ListItem Value="CC">CC
                                                        </asp:ListItem>
                                                        <asp:ListItem Value="FOC">FOC
                                                        </asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OC Due Car">
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotalOCDC" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTOcDueCar" runat="server" Text='<%# Eval("OcDueCar","{0:n}") %>'
                                                        Width="65px" TabIndex="595" ReadOnly="false" >
                                                   </asp:TextBox>
                                                    <asp:ImageButton ID="btnOcDueCar" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                        ImageAlign="AbsMiddle" TabIndex="600" OnClientClick="javascript:getOCDCDetails(this);" />
                                                </ItemTemplate>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OC Due Agent">
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotalOCDA" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTOcDueAgent" runat="server" Text='<%# Eval("OcDueAgent") %>' Width="65px"
                                                        ReadOnly="false" TabIndex="605" >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                    <asp:ImageButton ID="btnOcDueAgent" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                        ImageAlign="AbsMiddle" TabIndex="610" OnClientClick="javascript:getOCDADetails(this);" />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="False" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Spot Freight" >
                                                <FooterTemplate>
                                                    <asp:Label ID="lblSpotFreight" runat="server" Text="" Width="80px">
                                                    </asp:Label>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTSpotFreight" runat="server" TabIndex="615" Text='<%# Eval("SpotFreight","{0:n}") %>' Width="80px">
                                                       <%-- onchange="SetProcessFlag();"--%> 
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Spot<br/>Rate" >
                                                <FooterTemplate>
                                                    <asp:Label ID="lblSpotRate" runat="server" Text="" Width="40px">
                                                    </asp:Label>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTSpotRate" runat="server" TabIndex="615" Text='<%# Eval("SpotRate","{0:n}") %>' Width="40px">
                                                       <%-- onchange="SetProcessFlag();"--%> 
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sp Rate Id.">
                                                <FooterTemplate>
                                                    <asp:Label ID="lblSpotRateID" runat="server" Text="" Width="55px">
                                                    </asp:Label>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTSpotRateID" TabIndex="620" runat="server" Text='<%# Eval("SpotRateID") %>' Width="65px"
                                                        ReadOnly="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dyn Rate" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTDynRate" TabIndex="625" runat="server" Text='<%# Eval("DynRate") %>' Width="50px"
                                                        >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serv Tax">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTServiceTax" TabIndex="630" runat="server" Text='<%# Eval("ServTax","{0:n}") %>'
                                                        Width="65px" >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotalTax" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTTotal" runat="server" TabIndex="635" Text='<%# Eval("Total","{0:n}") %>' Width="80px"
                                                        >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLTotal" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="R. Class" Visible="true">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTRateClass" ReadOnly="true" runat="server" Text='<%# Eval("RateClass") %>'
                                                        Width="40px" TabIndex="640">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLRateClass" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Currency">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTCurrency" runat="server" Width="60px" TabIndex="645">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="LBLCurrency" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            
                                             <asp:TemplateField HeaderText="IATATax" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTIATATax" TabIndex="625" runat="server" Text='<%# Eval("IATATax") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="MKTTax" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTMKTTax" TabIndex="625" runat="server" Text='<%# Eval("MKTTax") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="OCTax" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTOCTax" TabIndex="625" runat="server" Text='<%# Eval("OCTax") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="OATax" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTOATax" TabIndex="625" runat="server" Text='<%# Eval("OATax") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SpotTax" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTSpotTax" TabIndex="625" runat="server" Text='<%# Eval("SpotTax") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CommTax" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTCommTax" TabIndex="625" runat="server" Text='<%# Eval("CommTax") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DiscTax" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTDiscTax" TabIndex="625" runat="server" Text='<%# Eval("DiscTax") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Commission" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTCommission" TabIndex="625" runat="server" Text='<%# Eval("Commission") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="CommPercent" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTCommPercent" TabIndex="625" runat="server" Text='<%# Eval("CommPercent") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Discount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTDiscount" TabIndex="625" runat="server" Text='<%# Eval("Discount") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="SpotStatus" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTSpotStatus" TabIndex="625" runat="server" Text='<%# Eval("SpotStatus") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="IATARateID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTIATARateID" runat="server" Text='<%# Eval("IATARateID") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MKTRateID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="TXTMKTRateID" runat="server" Text='<%# Eval("MKTRateID") %>' Width="50px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle CssClass="grdrowfont" />
                                        <FooterStyle CssClass="grdrowfont" />
                                        <HeaderStyle CssClass="titlecolr" />
                                        <RowStyle CssClass="grdrowfont" />
                                    </asp:GridView>
                                    <asp:Button ID="btnProcess" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_PROCESSRATES %>" OnClick="ProcessRatesV5_Click"
                                         OnClientClick="callShow();return ProcessRates(this);" CssClass="button" TabIndex="650" />
                                    <asp:Button ID="btnRates" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_RATEDETAILS %>"  CssClass="button" TabIndex="655" />
                                    <asp:Button ID="btnAddUld" runat="server" CssClass="button" OnClick="btnAddUld_Click"
                                        Text="Add to ULD" Visible="false" TabIndex="660" />
                                    
                                    <asp:Button ID="BtnViability" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_VIABILITY %>" CssClass="button" OnClick="btnViabilityShow_Click" TabIndex="665" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:HoverMenuExtender ID="HE1" runat="server" TargetControlID="btnRates" PopupControlID="PopupMenu"
                                                    PopupPosition="left" OffsetX="720" OffsetY="-30" PopDelay="50" />
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
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%--<asp:UpdateProgress ID="UpdateProgress" runat="server">
                    <ProgressTemplate>
                        <asp:Image ID="Image1" ImageUrl="~/Images/Wait.gif" AlternateText="Processing" runat="server" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
                    PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
            <div style="float:left">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="style6">
                        <tr>
                            <td style="width: 120px">
                                Execution Date: 
                            </td>
                            <td class="style8">
                                <asp:TextBox ID="txtExecutionDate" runat="server" Width="90px" OnTextChanged="CHKExportShipment_CheckedChanged"
                                    onchange="SetProcessFlag();"></asp:TextBox>
                                <asp:ImageButton ID="btnExecutionDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CEExecutionDate" Format="dd/MM/yyyy" TargetControlID="txtExecutionDate"
                                    PopupButtonID="btnExecutionDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td style="width: 90px">
                                Executed By 
                            </td>
                            <td class="style8">
                                <asp:TextBox ID="txtExecutedBy" runat="server" Width="100px" Enabled="false">QIDTECH</asp:TextBox>
                            </td>
                            <td style="width: 90px">
                                Executed At 
                            </td>
                            <td style="width: 70px">
                                <asp:TextBox ID="txtExecutedAt" runat="server" Width="60px" Enabled="false">PUNE</asp:TextBox>
                            </td>
                            <td style="width: 90px">
                                <!--1Remarks-->
                            </td>
                            <td class="style10">
                                <!--<asp:TextBox ID="txtRemarks" runat="server" Width="250px" MaxLength="250"></asp:TextBox>-->
                            </td>
                        </tr>
                        
                        <tr visible="false">
                            <%--<td visible="false">
                                Shipping Bill No.
                            </td>--%>
                            <td>
                                <asp:TextBox ID="txtShippingAWB" runat="server" Width="130px" TabIndex="670" Visible="false"></asp:TextBox>
                            </td>
                            <%--<td visible="false">
                                Attach Doc.
                            </td>--%>
                            <td>
                                <asp:TextBox ID="txtAttchDoc" runat="server" Width="100px" TabIndex="675" Visible="false"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/list_bullets.png"
                                    ImageAlign="AbsMiddle" TabIndex="680" OnClientClick="javascript:GetDocumentList();return false;" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px">
                                Irreg. Code:
                            </td>
                            <td class="style8" colspan="3">
                                <asp:DropDownList ID="ddlIrregularityCode"  TabIndex="685" runat="server" Width="345px">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
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
                                    <input type="button" id="Button1" class="button" value="Cancel" tabindex="690" onclick="HidePanellSplit();" />
                                </div>
                                <div id="fadeDiv" class="black_overlay">
                                </div>
                                <div>
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                Remarks
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtComment" runat="server" Rows="5" TabIndex="695" Columns="20" Width="250px" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td colspan="2" valign="top">
                                <asp:Button ID="btnShowRemarks" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SHOW %>" CssClass="button" TabIndex="700"
                                    OnClick="btnShowRemarks_Click" />&nbsp; &nbsp;
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdnDateChange" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        </div>
        <div id="fotbut" style="width: 100%">
            <br />
            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SAVE %>" CssClass="button" OnClick="btnSave_Click"
                TabIndex="705" OnClientClick="return GetProcessFlag();return ValidateShipperDetails();return AirlineCodeValidate(this);" />            
            <asp:Button ID="btnExecute" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_EXECUTE %>" CssClass="button" OnClick="btnExecute_Click"
                TabIndex="710" OnClientClick="return CheckQueueStatus();" />                
            <asp:Button ID="btnReopen" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_REOPEN %>" CssClass="button" OnClick="btnReopen_Click"
                TabIndex="715" />
                <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_DELETE %>" CssClass="button" OnClick="btnDelete_Click"
                OnClientClick="javascript:return confirm('Are you sure you want to Delete AWB?')"  TabIndex="720" />
            <asp:Button ID="btnDgr" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_DGR %>" CssClass="button" OnClick="btnDgr_Click" TabIndex="725" />
            <asp:Button ID="btnHABDetails" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_HAWB %>" CssClass="button" OnClick="btnHABDetails_Click" TabIndex="730" />
            <asp:Button ID="btnePouch" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_EPOUCH %>" CssClass="button" Enabled="true"
                TabIndex="735" OnClick="btnePouch_Click" />
           
              
                
            <asp:Button ID="btnSendFFR" runat="server" Text="Send FFR" CssClass="button" OnClick="btnSendFFR_Click" TabIndex="740" />
            <asp:Button ID="btnSendFHL" runat="server" Text="Send FHL" CssClass="button" OnClick="btnSendFHL_Click" TabIndex="745" />
            <asp:Button ID="btnSenfwb" runat="server" Text="Send FWB" CssClass="button" OnClick="btnSenfwb_Click" TabIndex="750" />
            <asp:Button ID="btnSendPHL" runat="server" Text="Send PHL" CssClass="button" 
                OnClick="btnSendPHL_Click" TabIndex="745" />
            <asp:Button ID="btnSendPWB" runat="server" Text="Send PWB" CssClass="button" OnClick="btnSendPWB_Click" TabIndex="750" />

            <asp:Button ID="btnShowEAWB" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_EAWB %>" CssClass="button" OnClick="btnShowEAWB_Click"
                Enabled="true" TabIndex="755" OnClientClick="return GetEAWBFlag();" />
                <asp:ImageButton ID="imgbtnEAWBPopUp" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle"
                OnClick="imgbtnEAWBPopUp_Click" TabIndex="760"/>
            <asp:Button ID="btnDGRLbl" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_DGRLABEL %>" CssClass="button" 
                onclick="btnDGRLbl_Click" TabIndex="765" />
            <asp:Button ID="btnPrintLabels" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_PRINTLABELS %>" CssClass="button" TabIndex="770" onclick="btnPrnSelLbl_Click"/>
            <asp:Button ID="btnCargoReceipt" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CARGORECEIPT %>" CssClass="button" TabIndex="775"
                OnClick="btnCargoReceipt_Click" />
            <asp:Button ID="btnCollect" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_COLLECTAMOUNT %>" CssClass="button"
                Enabled="false" OnClientClick="javascript:return WalkInCollection();" TabIndex="780" />            
            <asp:Button ID="btnPrintShipper" runat="server" Text="Print Shipper Dec Form" CssClass="button"
                Visible="False" TabIndex="785" />
            <asp:Button ID="sendIATA" runat="server" Text="Send IATA Message" CssClass="button"
                Visible="false" TabIndex="790" />
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" 
                PostBackUrl="~/Home.aspx" TabIndex="795" Visible="false" />
            <asp:Button ID="btnfinalize" runat="server" Text="OLD Finalize" CssClass="button" TabIndex="800" Visible="False" />
            <asp:Button ID="btnGenerateTracer" runat="server" TabIndex="805" Text="Generate Tracer" CssClass="button"
                Visible="False" />
                 <asp:Button ID="btnSaveTemplate" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SAVEASTEMPLATE %>" CssClass="button"
                OnClick="btnSaveTemplate_Click" Enabled="true" TabIndex="810" />
        </div>
        <div visible="false">
            <asp:HiddenField ID="HidAWBP1" runat="server" />
            <asp:HiddenField ID="HidAWBP2" runat="server" />
            <asp:HiddenField ID="HidSource" runat="server" Value=""/>
            <asp:HiddenField ID="HidDest" runat="server" />
            <asp:HiddenField ID="HidPcsCount" runat="server" />
            <asp:HiddenField ID="HidVia" runat="server" />
            <asp:HiddenField ID="HidWt" runat="server" />
            <asp:HiddenField ID="HidDimension" runat="server" />
            <asp:HiddenField ID="HidFlightsChanged" runat="server" />
            <asp:HiddenField ID="HidAWBPrefix" runat="server" Value=""/>
            <asp:HiddenField ID="HidAWBNumber" runat="server" Value=""/>
            <asp:HiddenField ID="HidChangeDate" runat="server" EnableViewState="true"/>
            <asp:HiddenField ID="HidIsManual" runat="server" />
            <asp:HiddenField ID="hfInvoiceNos" runat="server" Value="" />
            <asp:HiddenField ID="HidAWBPieces" runat="server" />
            <asp:HiddenField ID="HdnPcsForDGRLbl" runat="server" />
            <asp:HiddenField ID="HdnUNIDForDGRLbl" runat="server" />
            <asp:HiddenField ID="hdnBookingType" runat="server" />
            
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
         <div style="margin:5px;">
         <h1 style="width:305px;">Select Pieces</h1>
             <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
            
             <%--<asp:Label ID="lblAWBNoForPrintPopUp" runat="server" Visible="false"></asp:Label>&nbsp;&nbsp;
             <asp:Label ID="lblPcsForPrintPopUp" runat="server" Visible="false"></asp:Label>--%>
             <br />
             <table cellpadding="3" cellspacing="3">
             <tr>
             <td>
             <asp:RadioButton ID="radPrnAllLbl" runat="server" GroupName="PrnLblGrp"/>
             </td>
             <td>All</td>
             </tr>
             <tr>
             <td>
             <asp:RadioButton ID="radPrnSelLbl" runat="server" GroupName="PrnLblGrp"/></td>
             <td>
                 From:<asp:TextBox ID="txtPrnLblFrom" runat="server" Width="65px"></asp:TextBox>
             </td>
             <td>
                 To:<asp:TextBox ID="txtPrnLblTo" runat="server" Width="65px"></asp:TextBox>
             </td>
             
             </tr>
             <tr><td colspan="3">
              <asp:Button ID="btnPrintSelLbl" runat="server" CssClass="button" Text="Print" OnClick="btnPrintSelLbl_Click" />
              <asp:Button ID="btnPrintSelDGRLbl" runat="server" CssClass="button" Text="Print DGR" OnClick="btnPrintSelDGRLbl_Click" />
             <asp:Button ID="btnCancelPopUp" runat="server" CssClass="button" Text="Cancel" OnClientClick="javascript:return HidePanelSplit_SelLbl();" />
             
             </td></tr>
             </table>
             <br />
            
         </div>
     </div>
     <div id="DivSelLbl1" class="black_overlay"></div>
     
     <%--e-AWB PopUp--%>
     <div id="DivEAWBPopUp" class="white_content" style="height: 195px; width: 265px;">
         <div style="margin:5px;">
         <h1 style="width:200px;">Select Rate</h1>
             <asp:Label ID="lblEAWBPopUpError" runat="server"></asp:Label>
             <br />
             <table cellpadding="3" cellspacing="3">
             <tr>
             <td>
             <asp:RadioButton ID="radIATADef" runat="server" GroupName="EAWBGrp"/>
             </td>
             <td>IATA Defined</td>
             </tr>
             <tr>
             <td>
             <asp:RadioButton ID="radMarket" runat="server" GroupName="EAWBGrp"/></td>
             <td>Market</td>
             </tr>
             <tr>
             <td>
             <asp:RadioButton ID="radAsAgree" runat="server" GroupName="EAWBGrp"/></td>
             <td>As Agreed</td>
             </tr>
             <tr>
             <td>
             <asp:RadioButton ID="radSpot" runat="server" GroupName="EAWBGrp" Enabled="false"/></td>
             <td>Spot</td>
             </tr>
             <tr>
             <td colspan="2">
              <asp:Button ID="btnPrintEAWB" runat="server" CssClass="button" Text="Print" OnClick="btnPrintEAWB_Click"/>
              <asp:Button ID="btnCancelEAB" runat="server" CssClass="button" Text="Cancel" OnClientClick="javascript:HidePanelSplit_EAWBPopUp(); return false;"/>
             </td>
             </tr>
             </table>
             <br />
            
         </div>
     </div>
     <div id="DivEAWBPopUp1" class="black_overlay"></div>
            </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID="btnPrintSelLbl" />
            </Triggers>
        </asp:UpdatePanel>
        </div>
        
        
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
                            <asp:TextBox ID="lblHAWBNo" runat="server" Text='<%# Eval("HAWBNo") %>' Width="100px" MaxLength="12"></asp:TextBox>
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
                              MinimumPrefixLength="1" ServiceMethod="GetShipperCodeHAWB" 
                              TargetControlID="lblCustName"  CompletionListCssClass="CompletionListCssClass">
                            </asp:AutoCompleteExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                                      
                    <asp:TemplateField HeaderText="Con. Name">
                        <ItemTemplate>
                            <asp:TextBox ID="lblConsigneeName" runat="server" Text='<%# Eval("ConsigneeName") %>' Width="100px" onchange="javascript:GetConsigneeCodeHAWB(this);"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="ACEConCodeHAWB" runat="server" 
                            CompletionInterval="0" CompletionListCssClass="CompletionListCssClass" CompletionSetCount="10" EnableCaching="false" 
                            MinimumPrefixLength="1" ServiceMethod="GetShipperCodeHAWB" 
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
                                        <asp:TextBox id="txtShipperNameHAWB" runat="server" AutoPostBack="false" MaxLength="50" TabIndex="2000" Width="129px"> </asp:TextBox>

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
     
        

</asp:Content>
