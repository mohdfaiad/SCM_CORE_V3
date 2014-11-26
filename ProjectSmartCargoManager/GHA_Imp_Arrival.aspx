<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GHA_Imp_Arrival.aspx.cs" Inherits="ProjectSmartCargoManager.GHA_Imp_Arrival" MasterPageFile="~/SmartCargoMaster.Master"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


        
    <script type="text/javascript">
        function SelectAllgrdAddRate(CheckBoxControl) 
        {
            for (i = 0; i < document.forms[0].elements.length; i++) 
            {
                if (document.forms[0].elements[i].name.indexOf('check') > -1) {
                    document.forms[0].elements[i].checked = CheckBoxControl.checked;
                }
            }
        }
        function onCustomCodesPopulated() 
        {
            var completionList = $find("ACECommCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function GetCustomCodes(obj) 
        {
            var destination = obj;
            var CommodityName = obj.id;

            if (destination.value.indexOf("(") > 0) 
            {
                //                if (destination.value.length > 4) {
                var str = destination.value;
                var start = destination.value.indexOf("(");
                var end = destination.value.indexOf(")");

                obj.value = str.substring(0, start);
                document.getElementById(CommodityName).value = str.substring(0, start);
                document.getElementById(obj.id).focus();
             
            }
            return false;
        }

        function dimension(mybutton) 
        {

            //    code for getting data

            var strValue = mybutton.value;

            //         var i;

            var row = mybutton.parentNode.parentNode;

            var rowIndex = row.rowIndex - 1;
            var AWBID = row.cells[1].children[0].attributes[0].value;

            var strAwbNo = document.getElementById(AWBID).innerHTML;
            var TtPcsID = row.cells[4].children[0].attributes[0].value;

            var strTotalPcs = document.getElementById(TtPcsID).innerHTML;

            var TtWtID = row.cells[5].children[0].attributes[0].value;
            var strTotalWt = document.getElementById(TtWtID).innerHTML;

            var SHCID = "";
            var SHC = "";
            var targetcontrol = "";
            var DropDownList = "";

            var strCommCode = "";
            var TxtClientButtonID = "";
            var TxtClientObjectID = "";
            var TxtClientObjectVolID = "";
            var TxtClientObjectRemainingPcs = "";
            var TxtClientObjectRemainingWt = "";

            window.open('GHA_CargoDimensions.aspx?awbno=' + strAwbNo + '&SaveButton=' + TxtClientButtonID + '&TotalPcs=' + strTotalPcs + '&SHC=' + SHC + '&CommodityCode=' + strCommCode + '&TotalWt=' + strTotalWt + '&RecievedPcsTxt=' + TxtClientObjectID + '&RecievedWtTxt=' + TxtClientObjectVolID + '&RemainingPcsTxt=' + TxtClientObjectRemainingPcs + '&RemainingWtTxt=' + TxtClientObjectRemainingWt, '', 'left=0,top=0,width=800,height=550,toolbar=0,resizable=0');

            return false;

        }
        
        //This function shall show popup for showing/ adding AWB childs under ULD being added manually.
        function ShowULDChild(btnpopup)
        {
         
            //Get ULD #
            var targetcontrol = btnpopup.id.replace("btnULDChlidPopup", "GrdULDNo");
            var strULDValue = document.getElementById(targetcontrol).value;
         
            //Get Flight #
            targetcontrol = btnpopup.id.replace("btnULDChlidPopup", "GrdULDFlightNo");
            var strFlightNum = document.getElementById(targetcontrol).value;
         
            //Get Flight Dt
            targetcontrol = btnpopup.id.replace("btnULDChlidPopup", "GrdULDFlightDate");
            var strFlightDate = document.getElementById(targetcontrol).value;

            var awbCount = btnpopup.id.replace("btnULDChlidPopup", "GrdULDAWBCount");
            
            var ULDPcs = btnpopup.id.replace("btnULDChlidPopup", "GrdULDAWBPcs");
            
            var AWBWt = btnpopup.id.replace("btnULDChlidPopup", "GrdULDAWBWt");

            var ULDWt = btnpopup.id.replace("btnULDChlidPopup", "GrdULDULDWt");
            
            //Show popup
            window.open('frmAddChildAWBForULD.aspx?uld=' + strULDValue + '&flt=' + strFlightNum + '&dt=' + strFlightDate
            + '&AWBCount=' + awbCount + '&ULDPcs=' + ULDPcs + '&AWBWt=' + AWBWt + '&ULDWt=' + ULDWt, 
            'Child AWB', 'left=0,top=0,width=1024,height=800,toolbar=0,resizable=0', '');

            return false;
        }

        function SetOperationTime() 
        {
            //Show popup
            //window.open('frmOperationTime.aspx', 'Operation Time','left=400,top=200,width=400,height=200,toolbar=0,resizable=no');
            //window.open('frmOperationTime.aspx', '', 'width=400px,height=200px,left=400,top=200');
            document.getElementById('divOpsTimePopup').style.display = 'block';
            document.getElementById('blackening').style.display = 'block';
            return false;
        }



        function calculateActualWt(TextBox) 
        {
            //Get Delivered Pcs
            var valDeliveredPcs = TextBox.value;

            ctrlname = TextBox.id.replace('RcvPcs', 'MftPcs');
            var valExpectedPcs = document.getElementById(ctrlname).value;
            alert(valExpectedPcs);
//            if (parseInt(valExpectedPcs) < parseInt(valDeliveredPcs)) 
//            {
//                alert('Arrived Pcs can not be greater than Manifested Pcs.');
//                TextBox.value = "";
//                TextBox.focus();
//            }

            ctrlname = TextBox.id.replace('RcvWt', 'MftWt');
            var valExpectedPcs = document.getElementById(ctrlname).value;
//            if (parseInt(valExpectedPcs) < parseInt(valDeliveredPcs)) 
//            {
//                
//                alert('Arrived Weight can not be greater than Manifested Weight.');
//                TextBox.value = 0;
//                TextBox.focus();
//            }

            //Get expected wt
            var ctrlname = TextBox.id.replace('RcvWt', 'MftWt');
            var valExpectedWt = document.getElementById(ctrlname).value;

            //Calculate delivered wt
            var valDeliveredWt = (valExpectedWt / valExpectedPcs) * valDeliveredPcs;
            ctrlname = TextBox.id.replace('RcvPcs', 'RcvWt');
            document.getElementById(ctrlname).text = Math.round(valDeliveredWt, 4);
            //find remaining pieces
            var actualpcs = document.getElementById(TextBox.id).value;
            var totalpcs = document.getElementById(TextBox.id.replace('RcvPcs', 'Expectedpcs')).innerHTML;
            var rempcs = document.getElementById(TextBox.id.replace('RcvPcs', 'Remainingpcs')).value;
            var txtrempcs = document.getElementById(TextBox.id.replace('RcvPcs', 'Remainingpcs'));
            txtrempcs.value = (parseInt(totalpcs) - parseInt(actualpcs));
        }

        //Set received pcs & weight = remaining/ exp pcs & weight by default.
        function SetULDRecPcs(CheckBoxControl)
        {
            if (CheckBoxControl.checked == true) 
            {
                //If check box is selected, set received pcs & wt = expected pcs & wt
                //in respective row of grid.
                var expPcs = document.getElementById(CheckBoxControl.id.replace('checkULD', 'GrdULDExpectedpcs')).value;
                var expWt = document.getElementById(CheckBoxControl.id.replace('checkULD', 'GrdULDExpectedWeight')).value;
                document.getElementById(CheckBoxControl.id.replace('checkULD', 'GrdULDRcvPcs')).value = expPcs;
                document.getElementById(CheckBoxControl.id.replace('checkULD', 'GrdULDRcvWt')).value = expWt;
            }
        }

        function SelectAll(CheckBoxControl) 
        {

            if (CheckBoxControl.checked == true) 
             {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) 
                {
                    if ((document.forms[0].elements[i].type == 'checkbox')
        && (document.forms[0].elements[i].name.indexOf('grdStockUpd') > -1)) 
                    {
                        document.forms[0].elements[i].checked = true;
                    }
                }
             }
             else 
                {

                    for (i = 0; i < document.forms[0].elements.length; i++) 
                    {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
        (document.forms[0].elements[i].name.indexOf('grdStockUpd') > -1)) 
                    {
                        document.forms[0].elements[i].checked = false;
                    }
                    }
                }
        }

        function discrepancy(mybutton) 
        {

            //    code for getting data

            var strValue = mybutton.value;

            //         var i;

            var targetcontrol = mybutton.id.replace("btnDimensionsPopup", "ddlMaterialCommCode");

            var DropDownList = document.getElementById(targetcontrol);

            var strValue = DropDownList.options[DropDownList.selectedIndex].value;
            var strAwbNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;

            var grosswt = document.getElementById(TxtClientObjectID).value;


            var pcscount = document.getElementById(mybutton.id.replace("btnDimensionsPopup", "TXTPcs")).value;


            return;
        }
//        function FoundFailure() 
//        {

//            alert("No Data Found Please provide correct date..");

//        }
//        function RecordInserted() 
//        {
//            alert("Record Inserted Succssfully....");
//        }

//        function SelectAtleastOne() 
//        {

//            alert("Please select atleast one Field to Search..");

//        }
//        function CheckSelected() 
//        {

//            alert("Please select atleast one Row From to Change Discrepancy..");

//        }
//        function NotInserted() 
//        {

//            alert("Record Not Inserted Please try Again..");

//        }
//        function Error() 
//        {

//            alert("Please check the values");
//        }
//        function AlertMessage() 
//        {

//            confirm("Do you want to reset this AWBNumber to initial Position?");
//        }


//        function GenerateInvoices() 
//        {

//            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
//            
//            var InvList = hfInvNos.value;

//            var invArr = InvList.split(',');
//            for (var i = 0; i < invArr.length; i++) 
//            {
//                window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Dest' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
//                //window.open("ShowDestAgentInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
//            }
//        }

    </script>   
    
    <script type ="text/javascript">
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
//        function callclose() {
//            document.getElementById('light').style.display = 'none';
//            document.getElementById('fade').style.display = 'none';
//        }
        function ViewPanelSplit2() {
            callclose();
            document.getElementById('Lightsplit').style.display = 'block';
            document.getElementById('fadesplit').style.display = 'block';
        }
        
        //SHOW HAWB POPUP
        function callShow1() {
            document.getElementById('blackening').style.display = 'block';
            document.getElementById('whitening').style.display = 'block';
            document.getElementById('gvHAWBDetails').style.visibility = "visible";
        }

        //CLOSE HAWB POPUP
        function callclose1() {
            document.getElementById('blackening').style.display = 'none';
            document.getElementById('whitening').style.display = 'none';
        }

        //Show Shipper Popup
        function ViewPanel_shipperPopUp() {
            document.getElementById('DivShipperCon').style.display = 'block';
            document.getElementById('DivShipperCon1').style.display = 'block';
            return false;
        }
        //Hide Shipper Popup
        function HidePanel_shipperPopUp() {
            document.getElementById('DivShipperCon').style.display = 'none';
            document.getElementById('DivShipperCon1').style.display = 'none';
            return false;
        }
        //Show Consignee Popup
        function ViewPanel_ConsigneePopUp() {
            document.getElementById('Consigneedetails').style.display = 'block';
            document.getElementById('Consigneedetails1').style.display = 'block';
            return false;
        }
        //Hide Consignee Popup
        function HidePanel_ConsigneePopUp() {
            document.getElementById('Consigneedetails').style.display = 'none';
            document.getElementById('Consigneedetails1').style.display = 'none';
            return false;
        }

        function CloseWindow() {
            document.getElementById('divOpsTimePopup').style.display = 'none';
            document.getElementById('blackening').style.display = 'none';
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
        .CompletionListCssClass
{
    color: #000;
    padding: 3px 5px;
    border: 1px solid #999;
    background: #fff;
    width: auto !important;
    float: left;
    z-index: 1;
    position:absolute;
    margin-left:0px;
    
}
.CompletionListCssClass.hover
{
    color: #000 !important;
    padding: 3px 5px;
    border: 1px solid #999;
    background: #fff;
    width: auto !important;
    float: left;
    z-index: 1;
    position:absolute;
    margin-left:0px;
    
}
   /*Added from Capacity Planning.*/
    .tbcen
	{
		text-align:center;
	}
		    
	.op
	{
		z-index:99999;
	}
	.showh
    {
        display:none;
    }
    
    .highlight{ background:#7eabfe !important; height:30px; color:#fff;}
 .style1
    {
        height: 29px;
    }
          
    </style>
    
       
       
       <script type="text/javascript">
           function SelectAllgrdAddRate(CheckBoxControl) {
               for (i = 0; i < document.forms[0].elements.length; i++) {
                   if (document.forms[0].elements[i].name.indexOf('check') > -1) {
                       document.forms[0].elements[i].checked = CheckBoxControl.checked;
                   }
               }
           }

           function callexport() {
               window.open('ArrivalReassign.aspx', '', 'left=0,top=0,width=800,height=600,toolbar=0,resizable=0');
           }

           function calculateActualWt(TextBox) {
               //Get Delivered Pcs
               var valDeliveredPcs = TextBox.value;
               //alert ('1 '+ valDeliveredPcs);
               //Get expected pcs
               //'txtactualpieces=RcvPcs', 'txtexpectedpieces=Expectedpcs'
               ctrlname = TextBox.id.replace('RcvPcs', 'MftPcs');
               var valExpectedPcs = document.getElementById(ctrlname).value;
//               if (parseInt(valExpectedPcs) < parseInt(valDeliveredPcs)) {
//                   alert('Arrived Pcs can not be greater than Manifested Pcs.');
//                   TextBox.value = "";
//                   TextBox.focus();
//               }

               ctrlname = TextBox.id.replace('RcvWt', 'MftWt');
               var valExpectedPcs = document.getElementById(ctrlname).value;
//               if (parseInt(valExpectedPcs) < parseInt(valDeliveredPcs)) {

//                   alert('Arrived Weight can not be greater than Manifested Weight.');
//                   TextBox.value = 0;
//                   TextBox.focus();
//               }

               //Get expected wt
               var ctrlname = TextBox.id.replace('RcvWt', 'MftWt');
               var valExpectedWt = document.getElementById(ctrlname).value;

               //Calculate delivered wt
               var valDeliveredWt = (valExpectedWt / valExpectedPcs) * valDeliveredPcs;
               ctrlname = TextBox.id.replace('RcvPcs', 'RcvWt');
               document.getElementById(ctrlname).text = Math.round(valDeliveredWt, 4);
               //find remaining pieces
               var actualpcs = document.getElementById(TextBox.id).value;
               var totalpcs = document.getElementById(TextBox.id.replace('RcvPcs', 'Expectedpcs')).innerHTML;
               var rempcs = document.getElementById(TextBox.id.replace('RcvPcs', 'Remainingpcs')).value;
               var txtrempcs = document.getElementById(TextBox.id.replace('RcvPcs', 'Remainingpcs'));
               txtrempcs.value = (parseInt(totalpcs) - parseInt(actualpcs));
           }

           function SelectAll(CheckBoxControl) {

               if (CheckBoxControl.checked == true) {
                   var i;
                   for (i = 0; i < document.forms[0].elements.length; i++) {
                       if ((document.forms[0].elements[i].type == 'checkbox')
                    && (document.forms[0].elements[i].name.indexOf('grdStockUpd') > -1)) {
                           document.forms[0].elements[i].checked = true;
                       }
                   }
               }
               else {

                   for (i = 0; i < document.forms[0].elements.length; i++) {
                       if ((document.forms[0].elements[i].type == 'checkbox') &&
                    (document.forms[0].elements[i].name.indexOf('grdStockUpd') > -1)) {
                           document.forms[0].elements[i].checked = false;
                       }
                   }
               }
           }

           //Set Received Pcs & Weight = Manifested Pcs & Weight for ULD
           function SelectAll(CheckBoxControl) {

               if (CheckBoxControl.checked == true) {
                   var i;
                   for (i = 0; i < document.forms[0].elements.length; i++) {
                       if ((document.forms[0].elements[i].type == 'checkbox')
        && (document.forms[0].elements[i].name.indexOf('grdStockUpd') > -1)) {
                           document.forms[0].elements[i].checked = true;
                       }
                   }
               }
               else {

                   for (i = 0; i < document.forms[0].elements.length; i++) {
                       if ((document.forms[0].elements[i].type == 'checkbox') &&
        (document.forms[0].elements[i].name.indexOf('grdStockUpd') > -1)) {
                           document.forms[0].elements[i].checked = false;
                       }
                   }
               }
           }

           function discrepancy(mybutton) {

               //    code for getting data
               var strValue = mybutton.value;

               var targetcontrol = mybutton.id.replace("btnDimensionsPopup", "ddlMaterialCommCode");

               var DropDownList = document.getElementById(targetcontrol);

               var strValue = DropDownList.options[DropDownList.selectedIndex].value;
               var strAwbNo = document.getElementById('ctl00_ContentPlaceHolder1_txtAWBNo').value;

               var grosswt = document.getElementById(TxtClientObjectID).value;

               var pcscount = document.getElementById(mybutton.id.replace("btnDimensionsPopup", "TXTPcs")).value;
               return;

           }
           function FoundFailure() {

               alert("No Data Found Please provide correct date..");

           }
           function RecordInserted() {
               alert("Record Inserted Succssfully....");
           }

           function SelectAtleastOne() {

               alert("Please select atleast one Field to Search..");

           }
           function CheckSelected() {

               alert("Please select atleast one Row From to Change Discrepancy..");

           }
           function NotInserted() {

               alert("Record Not Inserted Please try Again..");

           }
           function Error() {

               alert("Please check the values");


           }
           function AlertMessage() {

               confirm("Do you want to reset this AWBNumber to initial Position?");
           }


//           function GenerateInvoices() {

//               var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");

//               var InvList = hfInvNos.value;

//               var invArr = InvList.split(',');
//               for (var i = 0; i < invArr.length; i++) {

//                   window.open("ShowDestAgentInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
//               }
//           }

           function GenerateInvoices() {

               var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");

               var InvList = hfInvNos.value;

               var invArr = InvList.split(',');
               for (var i = 0; i < invArr.length; i++) {

                   //window.open("ShowDestAgentInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                   window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Dest' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
               }

           }

           function SetPOL(textbox) {

               var strOrigin = textbox.value;

               //Set POL as Origin by default.
               var targetcontrol = textbox.id.replace("GrdULDOrg", "GrdULDPOL");
               document.getElementById(targetcontrol).value = strOrigin;

           }
           //Sets booked pieces & weight as Accepted pieces & weight.
           function CopyRecievedPcs(textbox) {

               var WtValue = textbox.value;

               var targetcontrol = textbox.id.replace("RcvWt", "RcvPcs");

               var PcsValue = document.getElementById(targetcontrol).value;

               //Set received pcs as booked pcs by default.
               targetcontrol = textbox.id.replace("RcvWt", "BkdPcs");
               if (document.getElementById(targetcontrol).value == '')
                   document.getElementById(targetcontrol).value = PcsValue;

               //Set received weight as booked weight by default.
               targetcontrol = textbox.id.replace("RcvWt", "BkdWt");
               if (document.getElementById(targetcontrol).value == '')
                   document.getElementById(targetcontrol).value = WtValue;

               //Set booked pcs as accepted pcs by default.
               targetcontrol = textbox.id.replace("RcvWt", "StdPcs");
               if (document.getElementById(targetcontrol).value == '')
                   document.getElementById(targetcontrol).value = PcsValue;

               //Set booked weight as Accepted Weight by default.
               targetcontrol = textbox.id.replace("RcvWt", "StdWt");
               if (document.getElementById(targetcontrol).value == '')
                   document.getElementById(targetcontrol).value = WtValue;

               //Set booked pcs as manifested pcs by default.
               targetcontrol = textbox.id.replace("RcvWt", "MftPcs");
               if (document.getElementById(targetcontrol).value == '')
                   document.getElementById(targetcontrol).value = PcsValue;

               //Set booked weight as manifested weight by default.
               targetcontrol = textbox.id.replace("RcvWt", "MftWt");
               if (document.getElementById(targetcontrol).value == '')
                   document.getElementById(targetcontrol).value = WtValue;

               //Set booked pcs as remaining pcs by default.
               targetcontrol = textbox.id.replace("RcvWt", "Expectedpcs");
               if (document.getElementById(targetcontrol).value == '')
                   document.getElementById(targetcontrol).value = PcsValue;

               //Set booked weight as remaining weight by default.
               targetcontrol = textbox.id.replace("RcvWt", "ExpectedWeight");
               if (document.getElementById(targetcontrol).value == '')
                   document.getElementById(targetcontrol).value = WtValue;

               //Set arrived pcs as 0 by default.
               targetcontrol = textbox.id.replace("RcvWt", "txtArrivedPcs");
               if (document.getElementById(targetcontrol).value == '')
                   document.getElementById(targetcontrol).value = '0';

               //Set arrived weight as 0 by default.
               targetcontrol = textbox.id.replace("RcvWt", "txtArrivedWt");
               if (document.getElementById(targetcontrol).value == '')
                   document.getElementById(targetcontrol).value = '0';

           }

    </script>
    
    
    
    <script type ="text/javascript">
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


        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            //document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }

       
        
</script>


<script type="text/javascript">

    //Get shipper information based on shipper code.
    function GetShipperCode(obj) {
        var destination = obj;

        if (destination.value.indexOf("(") > 0) {
            var str = destination.value;
            var start = destination.value.indexOf("(");

            obj.value = str.substring(0, start);
        }

        return false;
    }

    //Get consignee information based on consignee code.
    function GetConsigneeCode(obj) {
        var destination = obj;

        if (destination.value.indexOf("(") > 0) {
            var str = destination.value;
            var start = destination.value.indexOf("(");

            obj.value = str.substring(0, start);
        }
    }
    
    //For shipper code autosuggestion.
    function onShipperListPopulated() {

        var completionList = $find("ACESHPCode").get_completionList();
        completionList.style.width = 'auto';
    }

    //For consignee code autosuggestion.
    function onConsigneeListPopulated() {

        var completionList = $find("ACEConCode").get_completionList();
        completionList.style.width = 'auto';
    }
    
    function popup() {

        var Fltdt = document.getElementById("<%= txtFlightDate.ClientID %>").value;
        var Fltcd = document.getElementById("<%= txtFlightPrefix.ClientID %>").value;

        var Fltid = document.getElementById("<%= txtFlightNo.ClientID %>").value;
        var fltno = Fltcd + Fltid;

        window.open('UCRPopup.aspx?Type=New' + '&Mode=M' + '&FlightNo=' + fltno + '&FlightDate=' + Fltdt + '&pg=Arr', '', 'left=0,top=0,width=1000,height=1000,toolbar=0,resizable=0');


    }

    function OpenNotesChild() {
        
        var FltNo = document.getElementById("<%= txtFlightPrefix.ClientID %>").value + '' +
        document.getElementById("<%= txtFlightNo.ClientID %>").value;
        
        var FltDate = document.getElementById("<%= txtFlightDate.ClientID %>").value;
        
        if (FltNo != '') 
        {
            window.open("Notes.aspx?FltNo=" + FltNo + "&FltDate=" + FltDate,"", "status=0,toolbar=0, menubar=0, width=810, height=500");
        }
        else {
            window.open("Notes.aspx", "", "status=0,toolbar=0, menubar=0, width=810, height=500");
        }
        return false;
    }

    //Function to select/ unselect all rows in ULD Details grid.
    function SelectAllULD(headerchk) {
        var gvcheck = document.getElementById("<%=grdULDDetails.ClientID %>");
        var i;
        //Condition to check header checkbox selected or not if that is true checked all checkboxes
        if (headerchk.checked) {
            for (i = 0; i < gvcheck.rows.length; i++) {
                var inputs = gvcheck.rows[i].getElementsByTagName('input');
                if (inputs[0].disabled == false)
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

    //Function to select/ unselect all rows in AWB Details grid.
    function SelectAllAWB(headerchk) {
        var gvcheck = document.getElementById("<%=GVArrDet.ClientID %>");
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

    function ValidateHAWBPcs() {
        alert("The Given No. of Pieces Exceeds Main AWB Total No. of Pieces !!");
    }

    function ValidateHAWBWt() {
        alert("The Given Weight Exceeds Main AWB Total Weight !!");
    }

    function expandcollapse(obj, row) {
        var div = document.getElementById(obj);
        var img = document.getElementById('img' + obj);

        if (div.style.display == "none") {
            div.style.display = "block";
            if (row == 'alt') {
                img.src = "minus.gif";
            }
            else {
                img.src = "minus.gif";
            }
            img.alt = "Close to view other ULD details";
        }
        else {
            div.style.display = "none";
            if (row == 'alt') {
                img.src = "plus.gif";
            }
            else {
                img.src = "plus.gif";
            }
            img.alt = "Expand to view ULD Details";
        }
    }
    
    
     </script>
 

    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);
    
    </script>
    

    
   <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
       
    
    

    
      <div id="contentarea">
     <h1> Arrival Details
 <%--    <img alt="" src="images/txtarrival.png" />--%>
     </h1>
     
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
            Font-Size="Large" ForeColor="Red"></asp:Label>
     
     <div class="botline">
     <table  style="width:70%" cellpadding="5px">
            <tr>
                <td width="10%">
                    Flight# <asp:Label ID="lblMandatory" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
                <td width="45%" >
                    <asp:TextBox ID="txtFlightPrefix" runat="server" Width="45px" MaxLength="4"></asp:TextBox> 
                    <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" 
                        runat="server" TargetControlID="txtFlightPrefix" WatermarkText="Prefix">
                    </asp:TextBoxWatermarkExtender> 
                    &nbsp;<asp:TextBox ID="txtFlightNo" runat="server" CssClass="txtCommon" 
                        Width="55px" MaxLength="6" ToolTip="Flight Number" ></asp:TextBox>
                    <asp:TextBoxWatermarkExtender ID="txtFlightNo_TextBoxWatermarkExtender" 
                        runat="server" TargetControlID="txtFlightNo" WatermarkText="Flight #">
                    </asp:TextBoxWatermarkExtender>
                    &nbsp;<asp:TextBox ID="txtFlightDate" runat="server" CssClass="txtCommon" 
                        ToolTip="Please select Flight Date"  Width="85px" MaxLength="10"></asp:TextBox>
                        <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" runat="server"  PopupButtonID="imgDate"
                        Enabled="True" TargetControlID="txtFlightDate" Format="dd/MM/yyyy" >
                    </asp:CalendarExtender>
                    &nbsp;
                    <asp:ImageButton ID="imgNotebtn" ImageAlign="AbsMiddle" runat="server" 
                    OnClientClick="javascript:return OpenNotesChild();" ImageUrl="~/Images/noteicon.png" ToolTip="Notes" 
                    Height="20px" Width="34px" style="padding-bottom:2px;" />
                </td>
                <td>
                    Arrival Status
                </td>
                <td>
                    <asp:DropDownList ID="ddlArrivalStatus" runat="server">
                        <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Not Arrived" Value="NULL"></asp:ListItem>
                        <asp:ListItem Text="Completed" Value="C"></asp:ListItem>
                        <asp:ListItem Text="Partial" Value="P"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnList" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" CssClass="button" 
                        onclick="btnList_Click"/>
                    &nbsp;<asp:Button ID="btnClear" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>" CssClass="button" 
                        onclick="btnClear_Click" ToolTip="Clear All"/>
                </td><td colspan="2"></td>
            </tr>
            </table>
     </div>
     <div class="rout">
                 <img src="Images/txtflightdetails1.png" />
                 <asp:Label ID="lblRoute" runat="server" ></asp:Label>
                 <asp:Label ID="lblDate" runat="server" Text= " 29-05-2012 | Arrival"> </asp:Label>
                       
      </div>
      <div class="divback" runat="server" id="TpMargin" style="width:1000px;" >
        <table style="width:30%">
            <tr>
                <td>GHA Vehicle Info</td>
                <td>
                    <asp:TextBox ID="txtGHAVehicle" runat="server" Width="140px"></asp:TextBox>
                </td>
                 <td>
                    <asp:Button ID="btnListFilter" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" Enabled="false" CssClass="button" 
                         onclick="btnListFilter_Click" Visible="False" />
                </td>
                <%--<td>
                    <asp:CheckBox ID="chkTruckArr" runat="server" Text="Manual Arrival" 
                         Visible="false" />
                </td>--%>
            </tr>
            </table>
       </div>
       
       <h2>AWB Details</h2>
        <div style="width:1024px;overflow:auto;">
            <div style="float:left">
            <asp:GridView ID="GVArrDet" runat="server" AllowPaging="False" 
               AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
               CellPadding="2" CellSpacing="3" 
               onpageindexchanging="GVArrDet_PageIndexChanging" 
               onrowcommand="GVArrDet_RowCommand" onrowcreated="GVArrDet_RowCreated" 
               onrowdatabound="GVArrDet_RowDataBound" PageSize="10">
               <Columns>
                   <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAllAWB" runat="server" onclick="javascript:SelectAllAWB(this);" />
                        </HeaderTemplate>
                       <ItemTemplate>
                           <asp:CheckBox ID="check" runat="server" />
                       </ItemTemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="AWB *<br />(Prefix-Number)" 
                       ItemStyle-Wrap="false" >
                       <ItemTemplate>
                           <asp:TextBox ID="AWB" runat="server" AutoPostBack="true" EnableViewState="true" MaxLength="14" 
                               OnTextChanged="Getdata" Text='<%# Eval("AWBno") %>' Width="90px"> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Flt# *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="FlightNo" runat="server" Text='<%# Eval("FltNo") %>' 
                             MaxLength="10"  Width="60px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Flt Dt *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="FltDate" runat="server" Text='<%# Eval("FltDate") %>' 
                             MaxLength="10"  Width="70px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Origin *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Origin" runat="server" Text='<%# Eval("Org") %>' Width="40px" MaxLength="7"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Dest *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Destn" runat="server" Text='<%# Eval("Dest") %>' Width="40px" MaxLength="7"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="POL *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="POL" runat="server" Text='<%# Eval("POL") %>' Width="40px" MaxLength="7"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Mft Pcs" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="MftPcs" runat="server" 
                               Text='<%# Eval("PCS") %>' Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Mft Wt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="MftWt" runat="server" Text='<%# Eval("GrossWgt") %>' 
                               Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Arr Pcs" 
                       ItemStyle-Wrap="false">
                       <EditItemTemplate>
                           <asp:TextBox ID="txtArrPcs" runat="server" Text="">
                             </asp:TextBox>
                       </EditItemTemplate>
                       <ItemTemplate>
                           <asp:TextBox ID="txtArrivedPcs" runat="server" 
                               Text='<%# Eval("ArrivedPieces") %>' Width="40px">
                    </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Arr Wt" 
                       ItemStyle-Wrap="false">
                       <EditItemTemplate>
                           <asp:TextBox ID="txtArrWt" runat="server" Text=""></asp:TextBox>
                       </EditItemTemplate>
                       <ItemTemplate>
                           <asp:TextBox ID="txtArrivedWt" runat="server" 
                               Text='<%# Eval("ArrivedWeight") %>' Width="40px">
                    </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Received Pcs *" 
                       ItemStyle-Wrap="false">
                       <EditItemTemplate>
                           <asp:TextBox ID="TextBox1" runat="server" Text="0"></asp:TextBox>
                       </EditItemTemplate>
                       <ItemTemplate>
                           <asp:TextBox ID="RcvPcs" runat="server" Text='<%# Eval("RcvPcs") %>' MaxLength="6"
                               onchange="javascript:calculateActualWt(this);" Width="40px"></asp:TextBox>
                               <asp:ImageButton ID="btnDimensionsPopup" Enabled="false" runat="server" ImageUrl="~/Images/list_bullets.png"
                                ImageAlign="AbsMiddle" OnClientClick="javascript:dimension(this);return false;"/>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Received Wt. *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="RcvWt" runat="server" Text='<%# Eval("RcvWt") %>' onchange="javascript:CopyRecievedPcs(this);"
                           MaxLength="15" Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                 
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Rem Pcs" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Expectedpcs" runat="server" Text='<%# Eval("ExpectedPcs") %>' 
                               Width="40px"></asp:TextBox>
                           <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="Expectedpcs">
                        </asp:RequiredFieldValidator>--%> <%--'<%# Eval("ExpectedPcs") %>'--%>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Rem Wt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="ExpectedWeight" runat="server" 
                               Text='<%# Eval("ExpectedWeight") %>' Width="40px"></asp:TextBox>
                           <%--'<%# Eval("ExpectedPcs") %>'--%>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="HAWB Ct" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="HAWBCt" runat="server" Enabled="false" Text='<%# Eval("HAWBCt") %>' 
                           Width="40px"></asp:TextBox>
                           <asp:ImageButton ID="btnHAWBPopup" Enabled="true" runat="server" 
                           ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" CommandName="HAWB" 
                            CommandArgument='<%# Container.DataItemIndex %>'/>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Rcv HAWB Ct" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="RCVHAWBCt" runat="server" Enabled="false" Text='<%# Eval("RCVHAWBCt") %>'
                           Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Bkd Pcs" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="BkdPcs" runat="server" Text='<%# Eval("BookedPcs") %>' 
                           Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Bkd Wt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="BkdWt" runat="server" Text='<%# Eval("BookedWt") %>' 
                              Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="true" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Acc Pcs" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="StdPcs" runat="server" Text='<%# Eval("StatedPCS") %>' 
                               Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Acc Wt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="StdWt" runat="server" Text='<%# Eval("StatedWgt") %>' 
                               Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="true" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Comm Code" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="CommCode" runat="server" 
                               Text='<%# Eval("SCC") %>' Width="40px"></asp:TextBox>
                           <%--'<%# Eval("ExpectedPcs") %>'--%>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Shipper" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Shipper" runat="server" 
                               Text='<%# Eval("Shipper") %>' Width="20px"></asp:TextBox>
                           <asp:ImageButton ID="btnShipperPopup" Enabled="true" runat="server" 
                           ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" CommandName="SHIPPER" 
                            CommandArgument='<%# Container.DataItemIndex %>'/> 
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Consignee" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Consignee" runat="server" 
                               Text='<%# Eval("Consignee") %>' Width="20px"></asp:TextBox>
                           <asp:ImageButton ID="btnConsigneePopup" Enabled="true" runat="server" 
                           ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" CommandName="CONSIGNEE" 
                            CommandArgument='<%# Container.DataItemIndex %>'/> 
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <%-- Below column used for printing on Arrival Manifest.--%>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Comm Desc" 
                       ItemStyle-Wrap="false" Visible="false">  
                       <ItemTemplate>
                           <asp:Label ID="CommDesc" runat="server" 
                               Text='<%# Eval("DESC") %>' Width="80px"></asp:Label>
                           <%--'<%# Eval("ExpectedPcs") %>'--%>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <%-- Above column used for printing on Arrival Manifest.--%>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="ULD Dstn" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="ULDDestn" runat="server" Text='<%# Eval("ULDdest") %>' 
                               Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Discrepancy" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:DropDownList ID="ddlDiscrepancy" runat="server">
                           </asp:DropDownList>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="ULD#" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="ULD" runat="server" Text='<%# Eval("ULDno") %>' Width="32px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Security Check" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:CheckBox ID="SecurityCheck" runat="server" Text="" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Custom Check" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:CheckBox ID="CustomCheck" runat="server" Text="" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Custom Status Code" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="CustomStatusCode" runat="server" 
                           Text='<%# Eval("CustomStatusCode") %>' onchange="return GetCustomCodes(this);" Width="55px"></asp:TextBox>
                             <asp:AutoCompleteExtender ID="ACECommCode"  runat="server"
                                                        ServiceMethod="GetCustomStatusCodes" CompletionInterval="0" EnableCaching="false"
                                                        CompletionSetCount="10" CompletionListCssClass="CompletionListCssClass" TargetControlID="CustomStatusCode" MinimumPrefixLength="1"
                                                         >
                              </asp:AutoCompleteExtender>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Remarks" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Remark" runat="server" Width="80px"
                           Text='<%# Eval("ArrivalRemarks") %>'> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Status" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Status" runat="server" Enabled="false" 
                               Text='<%# Eval("status") %>' Width="25px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Owner" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Owner" runat="server" Text="" Width="60px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Discription" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="lblDiscription" runat="server" Text='<%# Eval("Desc") %>'></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Reassign" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="statusreassign" runat="server" Enabled="false" 
                               Text='<%# Eval("statusreassign") %>' Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Remainingpcs" runat="server" 
                               Text='<%# Eval("RemainingPcs") %>' Width="0px"></asp:TextBox>
                           <%--'<%# Eval("ExpectedPcs") %>'--%>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="ArrivedPcs" runat="server" Enabled="false" 
                               Text='<%# Eval("ArrivedPieces") %>' Width="0px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Location" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="txtLocation" runat="server" 
                           EnableViewState="true" Text='<%# Eval("Location") %>' Width="80px"> </asp:TextBox>
                           <asp:HiddenField ID="hdnManualAWB" runat="server" Value='<%# Eval("ManualAWB") %>' Visible="false" />
                           <asp:HiddenField ID="hdIsManual" runat="server" Value='<%# Eval("IsManualArrival") %>' Visible="false" />
                           <asp:HiddenField ID="hdIsUSCustom" runat="server" Value='<%# Eval("IsUSCustom") %>' Visible="false" />
                           <asp:HiddenField ID="hdAutoCustom" runat="server" Value='<%# Eval("AutoCustom") %>' Visible="false" />
                           <asp:HiddenField ID="hdAutoACAS" runat="server" Value='<%# Eval("AutoACAS") %>' Visible="false" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:ButtonField CommandName="Edit" Text="Edit" Visible="false">
                       <ItemStyle Width="50px"/>
                   </asp:ButtonField>
               </Columns>
               <HeaderStyle CssClass="titlecolr" />
               <RowStyle HorizontalAlign="Center" />
               <AlternatingRowStyle HorizontalAlign="Center" />
           </asp:GridView>
                 <div style="text-align:left" >
            <asp:Button ID="lnkCreate" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_ADD %>"
            onclick="lnkCreate_Click" Visible="true"></asp:Button>
            &nbsp;
            <asp:Button ID="LnkModify" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_MODIFY %>" CssClass="button" 
                onclick="LnkModify_Click" ></asp:Button>
         </div>
             </div>
        </div>
       <h2><asp:Label ID="lblULD" runat="server" Text="ULD Details"></asp:Label></h2>
        <div id="UDLdiv" runat="server" style="width:1024px;overflow:auto;">
            <div style="float:left">
                 <asp:GridView ID="grdULDDetails" runat="server"
               AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
               CellPadding="2" CellSpacing="3" onrowdatabound="grdULDDetails_RowDataBound">
               <Columns>
                   <asp:TemplateField>
                        <ItemTemplate>
                            <a href="javascript:expandcollapse('div<%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("ULDNo") %>', 'one');">
                                <img id="imgdiv<%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("ULDNo") %>" alt="Click to show/hide AWBs in ULD <%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("ULDNo") %>"  width="9px" border="0" src="plus.gif"/>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField>
                       <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAllULD" runat="server" onclick="javascript:SelectAllULD(this);" />
                       </HeaderTemplate>
                       <ItemTemplate>
                           <asp:CheckBox ID="checkULD" runat="server" onclick="javascript:SetULDRecPcs(this);"/>
                       </ItemTemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="ULD *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDNo" runat="server" AutoPostBack="true" 
                           EnableViewState="true" OnTextChanged="GetdataULD" MaxLength="12"
                           Text='<%# Eval("ULDNo") %>' Width="80px" Enabled="false"> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Flt# *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDFlightNo" runat="server" Text='<%# Eval("FltNo") %>' 
                             MaxLength="10"  Width="60px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Flt Dt *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDFlightDate" runat="server" Text='<%# Eval("FltDate") %>' 
                             MaxLength="10" Width="70px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Origin *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDOrg" runat="server" Text='<%# Eval("Org") %>' Width="40px" 
                          MaxLength="7" onchange="javascript:SetPOL(this);" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Dest *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDDestn" runat="server" Text='<%# Eval("Dest") %>' Width="40px" Enabled="false" MaxLength="7"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="POL *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDPOL" runat="server" Text='<%# Eval("POL") %>' Width="40px" Enabled="false" MaxLength="7"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="ULD Wt *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDULDWt" runat="server" Text='<%# Eval("ULDWt") %>' MaxLength="15"
                               Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Scale Wt *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDScaleWt" runat="server" Text='<%# Eval("ScaleWt") %>' MaxLength="15"
                               Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="AWB Ct *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDAWBCount" runat="server" Text='<%# Eval("AWBCount") %>' 
                               Width="40px" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="btnULDChlidPopup" Enabled="True" runat="server" Visible="false" 
                            ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" 
                            OnClientClick="javascript:ShowULDChild(this);return false;"/>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="AWB Pcs *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDAWBPcs" runat="server" Text='<%# Eval("AWBPcs") %>' 
                               Width="40px" Enabled="false" MaxLength="6"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="AWB Wt *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDAWBWt" runat="server" Text='<%# Eval("AWBWt") %>' 
                             MaxLength="15"  Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Security Check" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:CheckBox ID="SecurityCheck" runat="server" Text="" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Custom Check" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:CheckBox ID="CustomCheck" runat="server" Text="" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="BUP" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:CheckBox ID="BUP" runat="server" Text="" Enabled="false" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Custom Status Code" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="CustomStatusCode" runat="server" Text='<%# Eval("ArrivalCustomStatusCode") %>' Width="55px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Remarks" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Remark" runat="server" Text='<%# Eval("ArrivalRemark") %>' Width="80px"> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Location" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDLocation" runat="server" 
                            EnableViewState="true" Text='<%# Eval("Location") %>' Width="80px"> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Arrived" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:CheckBox ID="isarrived" runat="server" Text="" Enabled="false" />
                           <asp:HiddenField ID="IsReceived" runat="server" Value='<%# Eval("IsReceived") %>' />
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   
                   <asp:TemplateField  HeaderStyle-CssClass="showh" ItemStyle-CssClass="showh">
			        <ItemTemplate>
			        <tr>
                            <td colspan="20" style="border-top:0px;">
                                <div id="div<%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("ULDNo") %>" 
                                alt="Click to show/hide AWBs in ULD <%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("ULDNo") %>" 
                                style="display:none;position:relative;left:15px;OVERFLOW: auto;WIDTH:97%" >
                                    <strong> AWB Details</strong><br />
                                    
                                    <asp:GridView ID="GVSubArrDet" runat="server" AllowPaging="False" 
                                       AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
                                       CellPadding="2" CellSpacing="3" PageSize="10" HeaderStyle-CssClass="AltRowStyle" 
                                       HeaderStyle-BackColor="#656667" HeaderStyle-ForeColor="#ffffff"  
                                       CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" PagerStyle-CssClass="PagerStyle" 
                                       SelectedRowStyle-CssClass="SelectedRowStyle">
                                       <HeaderStyle CssClass="titlecolr" />
                                       <RowStyle HorizontalAlign="Center" />
                                       <AlternatingRowStyle HorizontalAlign="Center" />
                                       <Columns>
                                           <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="AWB<br />(Prefix-Number)" 
                                               ItemStyle-Wrap="false" >
                                               <ItemTemplate>
                                                   <asp:Label ID="AWB" runat="server" AutoPostBack="true" EnableViewState="true" MaxLength="12" 
                                                       OnTextChanged="Getdata" Text='<%# Eval("AWBno") %>' Width="90px" > </asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="False" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Flt#" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="FlightNo" runat="server" Text='<%# Eval("FltNo") %>' 
                                                       Width="60px"></asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Flt Dt" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="FltDate" runat="server" Text='<%# Eval("FltDate") %>' 
                                                       Width="70px" ></asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Origin" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="Origin" runat="server" Text='<%# Eval("Org") %>' Width="40px"></asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="False" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Dest" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="Destn" runat="server" Text='<%# Eval("Dest") %>' Width="40px" ></asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="False" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="POL" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="POL" runat="server" Text='<%# Eval("POL") %>' Width="40px"></asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="False" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           
                                           <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Acc Pcs" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="StdPcs" runat="server" Text='<%# Eval("StatedPCS") %>' 
                                                       Width="40px" ></asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Acc Wt" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="StdWt" runat="server" Text='<%# Eval("StatedWgt") %>' 
                                                       Width="40px" ></asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="true" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Mft Pcs" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="MftPcs" runat="server" 
                                                       Text='<%# Eval("PCS") %>' Width="40px" ></asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Mft Wt" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="MftWt" runat="server" Text='<%# Eval("GrossWgt") %>' 
                                                       Width="40px" ></asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Arr Pcs" 
                                               ItemStyle-Wrap="false">
                                               <EditItemTemplate>
                                                   <asp:Label ID="txtArrPcs" runat="server" Text="">
                                                     </asp:Label>
                                               </EditItemTemplate>
                                               <ItemTemplate>
                                                   <asp:Label ID="txtArrivedPcs" runat="server" 
                                                       Text='<%# Eval("ArrivedPieces") %>' Width="40px">   
                                                    </asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Arr Wt" 
                                               ItemStyle-Wrap="false">
                                               <EditItemTemplate>
                                                   <asp:Label ID="txtArrWt" runat="server" Text=""></asp:Label>
                                               </EditItemTemplate>
                                               <ItemTemplate>
                                                   <asp:Label ID="txtArrivedWt" runat="server" 
                                                       Text='<%# Eval("ArrivedWeight") %>' Width="40px" >
                                                    </asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                         
                                           <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Rem Pcs" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="Expectedpcs" runat="server" Text='<%# Eval("ExpectedPcs") %>' 
                                                       Width="40px"></asp:Label>
                                                   <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="Expectedpcs">
                                                </asp:RequiredFieldValidator>--%> 
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Rem Wt" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="ExpectedWeight" runat="server" 
                                                       Text='<%# Eval("ExpectedWeight") %>' Width="40px"></asp:Label>
                                                   <%--'<%# Eval("ExpectedPcs") %>'--%>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           
                                           <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Comm Code" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="CommCode" runat="server" 
                                                       Text='<%# Eval("SCC") %>' Width="40px"></asp:Label>
                                                   <%--'<%# Eval("ExpectedPcs") %>'--%>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="False" />
                                           </asp:TemplateField>
                                           
                                           <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Custom Check" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:CheckBox ID="CustomCheck" runat="server" Text=""  Enabled="false"/>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="false" />
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Custom Status Code" 
                                               ItemStyle-Wrap="false">
                                               <ItemTemplate>
                                                   <asp:Label ID="CustomStatusCode" runat="server" 
                                                   Text='<%# Eval("CustomStatusCode") %>' Width="55px" ></asp:Label>
                                               </ItemTemplate>
                                               <HeaderStyle Wrap="true" />
                                               <ItemStyle Wrap="false" />
                                           </asp:TemplateField>
                                       </Columns>
                                   </asp:GridView>
                                </div>
                             </td>
                        </tr>
			        </ItemTemplate>			       
			    </asp:TemplateField>
			        
               </Columns>
               <HeaderStyle CssClass="titlecolr" />
               <RowStyle HorizontalAlign="Center" />
               <AlternatingRowStyle HorizontalAlign="Center" />
           </asp:GridView>
      
                 <div style="text-align:left" >
                <asp:Button ID="lnkULDCreate" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_ADD %>" 
                onclick="lnkULDCreate_Click" ></asp:Button>
            </div>
             </div>
        </div>
       
       
           <%-- <asp:Panel ID="Pnlgrd" runat="server" ScrollBars="Auto" 
                                             style="margin-top:20px"
                                        BorderStyle="Solid" BorderWidth="1px" Width="1024px"> 
             </asp:Panel>--%>
        
        
        <div  id="fotbut">
                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SAVE %>" CssClass="button" 
                          onclick="btnSave_Click"/>
                          <span style="vertical-align:bottom;">
                          <asp:ImageButton ID="btnOpsTime" CssClass="imgclock"
                            runat="server" ImageUrl="~/Images/timecalender.png" 
                           Enabled="true" onclick="btnOpsTime_Click"/></span>
                          &nbsp;<asp:Button ID="btnAutoArrive" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_AUTOARRIVE %>" 
                            CssClass="button" onclick="btnAutoArrive_Click"/>                        
                        &nbsp;<asp:Button ID="btnCloseFlt" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLOSEFLIGHT %>" 
                            CssClass="button" onclick="btnCloseFlt_Click"/>
                            <asp:Button ID="btnReopen" runat="server" Text="Reopen Flight" CssClass="button" 
                            ToolTip="<%$ Resources:LabelNames, LBL_BTN_REOPENFLIGHT %>" onclick="btnReopen_Click"/>
                        &nbsp;<asp:Button ID="btnReassign" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_REASSIGN %>" 
                            CssClass="button" onclick="btnReassign_Click" />
                        &nbsp;<asp:Button ID="btnDiscrepancy" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_DISRCEPANCY %>"  OnClientClick="" 
                        CssClass="button" Enabled="False" />
                        &nbsp;<asp:Button ID="btnNotify" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_NOTIFY %>" CssClass="button" 
                            Enabled="False"/>
                        &nbsp;
                        <asp:Button ID="btnGenTracer" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_GEN.TRACER %>" 
                            CssClass="button" Enabled="False"/>
                        &nbsp;
                        <asp:Button ID="btnSendAAR" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SENDFSA/ARR %>" 
                            CssClass="button" Enabled="True" onclick="btnSendAAR_Click" />
                        <asp:Button ID="btnSendFSN" runat="server" CssClass="button" Enabled="True" 
                            onclick="btnSendFSN_Click" Text="Send AMS Msg" />
                                 <asp:Button ID="btnSendACAS" runat="server" CssClass="button" Enabled="True" 
                             Text="Send ACAS Msg" onclick="btnSendACAS_Click" />
                        &nbsp;
                        <asp:Button ID="btnManifest" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_PRINTMANIFEST %>"  
                        CssClass="button" onclick="btnManifest_Click" />
                            &nbsp;
                            <asp:Button ID="btnprintUCR" runat="server" 
                        Text="<%$ Resources:LabelNames, LBL_BTN_ACCEPTUCR %>" CssClass="button" OnClientClick="popup()" 
                         />
                        <asp:HiddenField ID="hfInvoiceNos" runat="server" Value="" />

        </div>
        
        <div id="Lightsplit" class="white_content">
		    <table>
		        <tr>
		            <td>
		                <asp:Label ID="lblMsgType" runat="server" Text="Message Type: " ForeColor="Blue"></asp:Label>
		            </td>
		            <td>
                        <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Blue"></asp:Label>
		            </td>
		        </tr>
		    </table>
            <table>
            <tr>
                <td>
                    <asp:Label ID="lblEmail" runat="server" Text="To Email ID : (Comma Seprated EmailID)" ForeColor="Blue"></asp:Label>
                </td>
            </tr>
            </table>
            <br />
            <table width="100%">
            <tr>
            <td>
            <asp:TextBox ID="txtEmailID" runat="server" TextMode="MultiLine"  Width="300px" Height="50px"></asp:TextBox>
            </td>
            <td>
            </td>
            </tr>
            </table>
            <br />
            <table>
            <tr>
            <td>
            <asp:Button ID="btnOK" CssClass="button" runat="server" Text="OK" onclick="btnOK_Click"   />
<input type="button" id="btnCancel" class="button" value="Cancel" onclick="HidePanelSplit();"/>
            </td>
            </tr>
            </table>
        </div>
         
        <div id="fadesplit" class="black_overlay"></div>
         
    

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

    <div style="display:none; margin-top:15px;width:500px;" id="DivShipperCon" class="white_content">
    <strong>Shipper Details</strong>
        <table cellpadding="3" cellspacing="3" width="100%">
            <tr>
                <td>
                    AWB #
                </td>
                <td>
                    <asp:Label ID="lblShipperAWB" runat="server" ></asp:Label>
                </td>
                <td>
                    
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Account Code
                </td>
                <td>
                    <asp:TextBox ID="txtShipperAccountCode" runat="server" onchange="GetShipperCode(this);"  AutoPostBack="true" MaxLength="50" TabIndex="1015" OnTextChanged="ShipperCodeDetailsChanged"> 
                    <%--onchange="GetShipperCode(this);ViewPanel_shipperPopUp();" OnTextChanged="ShipperCodeDetailsChanged"--%>
                    </asp:TextBox>
                    <asp:AutoCompleteExtender ID="ACESHPCode" runat="server" BehaviorID="ACESHPCode"
                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                        MinimumPrefixLength="1" ServiceMethod="GetShipperCode" 
                        TargetControlID="txtShipperAccountCode" OnClientPopulated="onShipperListPopulated">
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
                    <asp:TextBox ID="TXTShipper" runat="server" AutoPostBack="False" MaxLength="50" TabIndex="1016"> 
                    </asp:TextBox>

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
                </td>
                <td>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnSaveShipper" runat="server" Text="Save" CssClass="button"
            TabIndex="1027" onclick="btnSaveShipper_Click"/>

        <asp:Button ID="btnCancelShipper" runat="server" Text="Cancel" CssClass="button"
         TabIndex="1028" onclick="btnCancelShipper_Click"/>
    </div>
    <div id="DivShipperCon1" class="black_overlay"></div>
    <br />
   
    <div style="display:none;width:500px;" id="Consigneedetails" class="white_content">
    <strong>Consignee Details</strong>
        <table cellpadding="3" cellspacing="3" width="100%">
            <tr>
                <td>
                    AWB #
                </td>
                <td>
                    <asp:Label ID="lblConsigneeAWB" runat="server" ></asp:Label>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Account Code
                </td>
                <td>
                    <asp:TextBox ID="txtConsigneeAccountCode" runat="server" AutoPostBack="False" MaxLength="50"
                    onchange="GetConsigneeCode(this);" OnTextChanged="ShipperCodeDetailsChanged">
                    </asp:TextBox>
                    <asp:AutoCompleteExtender ID="ACEConCode" runat="server" BehaviorID="ACEConCode"
                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                        MinimumPrefixLength="1" ServiceMethod="GetConsigneeCode"
                        TargetControlID="txtConsigneeAccountCode" OnClientPopulated="onConsigneeListPopulated">
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
                    <asp:TextBox ID="TXTConsignee" runat="server" AutoPostBack="false" MaxLength="50"
                        TabIndex="1029"></asp:TextBox>
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
         <asp:Button ID="btnSaveConsignee" runat="server" Text="Save" CssClass="button" 
            TabIndex="1039" onclick="btnSaveConsignee_Click"/>

        <asp:Button ID="btnCancelConsignee" runat="server" Text="Cancel" 
            CssClass="button" TabIndex="1041" onclick="btnCancelConsignee_Click"/>
    </div>
    
    <div id="Consigneedetails1" class="black_overlay"></div>
    <br />
    
    <%--START LIGHTBOX CONTROL FOR HAWB--%>
        <%--<div id="Div1" class="black_overlay">
        </div>--%>
        
        <div id="whitening" class="white_content" style="width: 1100px; left: 10%;">
            <asp:Label ID="lblHAWBStatus" runat="server" Text=''></asp:Label>
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
                    </tr>
                    <tr>
                        <td>
                            Origin : &nbsp;
                            <asp:Label ID="lblHAWBOrigin" runat="server" Text="-"></asp:Label>
                        </td>
                        <td>
                            Destination: &nbsp;
                            <asp:Label ID="lblHAWBDest" runat="server" Text="-"></asp:Label>
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Test" runat="server">
            <asp:GridView ID="gvHAWBDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                AutoGenerateDeleteButton="False" CellPadding="0" CellSpacing="0" ShowFooter="True" Visible="true"
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
                    <asp:TemplateField HeaderText="HAWB Prefix#">
                        <ItemTemplate>
                            <asp:Label ID="lblHAWBPrefix" runat="server" Text='<%# Eval("HAWBPrefix") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtHAWBPrefix" runat="server" MaxLength="12" Width="60px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HAWB #">
                        <ItemTemplate>
                            <asp:Label ID="lblHAWBNo" runat="server" Text='<%# Eval("HAWBNo") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtHAWBNo" runat="server" MaxLength="12" Width="60px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HAWB Pcs">
                        <ItemTemplate>
                            <asp:Label ID="lblHAWBPcs" runat="server" Text='<%# Eval("HAWBPcs") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtHAWBPcs" runat="server" MaxLength="4" Width="50px" Text="0"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtHAWBPcs_FilteredTextBoxExtender" runat="server"
                                Enabled="True" TargetControlID="txtHAWBPcs" ValidChars="0123456789">
                            </asp:FilteredTextBoxExtender>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HABWt">
                        <ItemTemplate>
                            <asp:Label ID="lblHAWBWt" runat="server" Text='<%# Eval("HAWBWt") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtHAWBWt" runat="server" MaxLength="10" Width="50px" Text="0"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtHAWBWt_FilteredTextBoxExtender" runat="server"
                                Enabled="True" TargetControlID="txtHAWBWt" ValidChars=".0123456789">
                            </asp:FilteredTextBoxExtender>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="15" Width="90px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CustomerID">
                        <ItemTemplate>
                            <asp:Label ID="lblCustID" runat="server" Text='<%# Eval("CustID") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtCustID" runat="server" MaxLength="15" Width="60px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer Name">
                        <ItemTemplate>
                            <asp:Label ID="lblCustName" runat="server" Text='<%# Eval("CustName") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtCustName" runat="server" MaxLength="30" Width="60px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer Address">
                        <ItemTemplate>
                            <asp:Label ID="lblCustAddress" runat="server" Text='<%# Eval("CustAddress") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtCustAddress" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City">
                        <ItemTemplate>
                            <asp:Label ID="lblCity" runat="server" Text='<%# Eval("CustCity") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtCity" runat="server" MaxLength="10" Width="50px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Zipcode">
                        <ItemTemplate>
                            <asp:Label ID="lblZipcode" runat="server" Text='<%# Eval("Zipcode") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtZipcode" runat="server" MaxLength="6" Width="50px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtZipcode_FilteredTextBoxExtender" runat="server"
                                Enabled="True" TargetControlID="txtZipcode" ValidChars="0123456789">
                            </asp:FilteredTextBoxExtender>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Origin">
                        <ItemTemplate>
                            <asp:Label ID="lblOrigin" runat="server" Text='<%# Eval("Origin") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtOrigin" runat="server" MaxLength="5" Width="50px"></asp:TextBox>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Destination">
                        <ItemTemplate>
                            <asp:Label ID="lblDestination" runat="server" Text='<%# Eval("Destination") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtDestination" runat="server" MaxLength="5" Width="50px"></asp:TextBox>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SHC">
                        <ItemTemplate>
                            <asp:Label ID="lblSHC" runat="server" Text='<%# Eval("SHC") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtSHC" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Arrival Status">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkArrivalStatusRow" runat="server" ></asp:CheckBox>
                            <asp:HiddenField ID="hdnManualHAWB" runat="server" Value='<%# Eval("ManualHAWB") %>' Visible="false" />                            
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:CheckBox ID="chkArrivalStatusFooter" runat="server" MaxLength="20" Width="80px"></asp:CheckBox>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <FooterTemplate>
                            <asp:Button ID="btnADDHAWB" runat="server" CssClass="button" OnClick="btnADDHAWB_Click"
                                Text="Add HAWB" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="titlecolr" />
                <AlternatingRowStyle CssClass="trcolor" Wrap="False" />
                <EditRowStyle CssClass="grdrowfont" />
                <RowStyle CssClass="grdrowfont" HorizontalAlign="Center" Wrap="False" />
                <FooterStyle CssClass="grdrowfont" />
            </asp:GridView>
            </div>
            <table width="30%">
                <tr>
                    <td>
                        <asp:Button ID="btnSaveHAWB" CssClass="button" runat="server" Text="SAVE" OnClick="btnSaveHAWB_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnDeleteAWB" CssClass="button" runat="server" Text="DELETE" OnClick="btnDeleteAWB_Click" />
                    </td>
                    <td>
                        <input type="button" id="btnHAWBCancel" class="button" value="CANCEL" onclick="callclose1();" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="blackening" class="black_overlay">
        </div>
    <%--END LIGHTBOX CONTROL FOR HAWB--%>
    </div>
    <div id="divOpsTimePopup" class="white_content">
            <div style="margin:10px;">
            <asp:Label ID="lblPnlError" runat="server" ForeColor="Red"></asp:Label>
            
               <h3><asp:Label ID="lblOperationDetails" Text="Actual Operation Time" runat="server" Font-Bold="true" Font-Size="Larger"></asp:Label>
                </h3> 
            <hr />
           
            <div style="width:350px;">
            <table width="100%" cellpadding="3" cellspacing="3">
                <tr>
                    <td>
                        Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtOpsDate" runat="server" Width="80px"></asp:TextBox>
                        
                        <asp:ImageButton ID="ImageButton3" runat="server" ImageAlign="AbsMiddle" 
                             ImageUrl="~/Images/calendar_2.png" />
    
    </td><td style="width:70px;" >
                        <asp:TextBox ID="txtOpsTimeHr" runat="server" DataTextField="" Width="70px" 
                           ></asp:TextBox></td>
                           <td style="width:120px;" valign="bottom">
                            <asp:TextBox ID="txtOpsTimeMin" runat="server" DataTextField="" Width="70px"></asp:TextBox>
                            (HR:MI)</td>
                        
                        <td>                
                        <asp:CalendarExtender ID="txtOpsDate_CalendarExtender" Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtOpsDate" PopupButtonID="ImageButton3" PopupPosition="BottomLeft">
                         </asp:CalendarExtender>
                        
                             <asp:NumericUpDownExtender ID="txtDeptTimeHr_NumericUpDownExtender" 
                                        runat="server" Maximum="23" Minimum="0" RefValues="00;01;02;03;04;05;06;07;08;09;10;11;12;13;14;15;16;17;18;19;20;21;22;23" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtOpsTimeHr" Width="40" />
                                        
                        <asp:NumericUpDownExtender ID="txtDeptTimeMin_NumericUpDownExtender1" 
                                        runat="server" Maximum="59" Minimum="0" RefValues="00;01;02;03;04;05;06;07;08;09;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31;32;33;34;35;36;37;38;39;40;41;42;43;44;45;46;47;48;49;50;51;52;53;54;55;56;57;58;59"
                                         ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtOpsTimeMin" Width="40" />
                        </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnOpsSave" runat="server" Text="Save" CssClass="button" onclick="btnOpsSave_Click" 
                              />
                    </td>
                    <td>&nbsp;</td>
                    <td>
                    <asp:Button ID="btnOpsCancel" runat="server" Text="Cancel" CssClass="button" 
                            onclick="btnOpsCancel_Click" />
                    </td>
              </tr>
            </table>
        </div>
	</div>
	    </div>
    </ContentTemplate>
        <Triggers>
    <asp:PostBackTrigger ControlID="btnManifest"/>
    </Triggers>
</asp:UpdatePanel>



</asp:Content>
