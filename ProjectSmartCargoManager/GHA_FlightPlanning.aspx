<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="GHA_FlightPlanning.aspx.cs" Inherits="ProjectSmartCargoManager.GHA_FlightPlanning" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">

        function UserDeleteConfirmation() {
            return confirm("Are you sure you want to delete this ULD?");
        }

        function UserDeleteCartConfirmation() {
            return confirm("Are you sure you want to delete this Cart?");
        }

        function onDriverListPopulated() {

            var completionList = $find("ACEDriverName").get_completionList();
            completionList.style.width = 'auto';
        }

        function onCommListPopulated() {

            var completionList = $find("ACEDriverName").get_completionList();
            completionList.style.width = 'auto';
        }

        function select() {

            var hldyVal = document.getElementById("<%= ddlReason.ClientID%>").value;
            var txt = document.getElementById("<%= txtReason.ClientID%>");
            // alert(hldyVal);
            // alert(txt);
            if (hldyVal == "Others") {
                //alert('in');
                txt.value = "";
                txt.disabled = false;

            }
            else if (hldyVal != "Others") {
                //alert('in');
                txt.value = "";
                txt.disabled = true;

            }
            return true;
        }
        window.onload() = function() {
            alert('On Load');
            //  var hldyVal = document.getElementById("<%= ddlReason.ClientID%>").value;
            //        var txt = document.getElementById("<%= txtReason.ClientID%>");
            //        // alert(hldyVal);
            //        // alert(txt);
            //        if (hldyVal != "Others") 
            //        {
            //            //alert('in');
            //            txt.value = "";
            //            txt.disabled = True;
            //            }
        };
        function disable() {

            var hldyVal = document.getElementById("<%= ddlReason.ClientID%>").value;
            var txt = document.getElementById("<%= txtReason.ClientID%>");
            // alert(hldyVal);
            // alert(txt);
            if (hldyVal != "Others") {
                //alert('in');
                txt.value = "";
                txt.disabled = True;
            }
        }



        function GetProcessFlag() {
            var ProcessFlag = document.getElementById('<%= hdnManifestFlag.ClientID%>');

            if (ProcessFlag.value == "1") {
                alert("Kindly finalize the manifest to proceed !");
                return false;
            }
            return true;
        }

        function cllsa() {
            document.getElementById("<%=BtnList.ClientID %>").click();
        }

        function callexportULD() {
            window.open('frmULDToAWBAssoc.aspx', 'Send', 'left=0,top=0,width=700,height=500,toolbar=0,resizable=0');
        }

        function display_alertAWBULD() {
            alert("Please Select ULD from DDL and AWB from TAB");
        }

        function alertSelectAWBULD() {
            alert("Please Select ULD from Tab And/Or AWB from TAB to add to Manifest");
        }

        function alertAWB() {
            alert("Please Select Atleast One AWB ");
        }

        function Successfull() {
            alert(" Version Save Successfull");
        }
        function AlreadyAvailable() {
            alert(" ULD Already Available");
        }

        function ULDAWBAssocitionSuccessfull() {
            alert("AWB Assigned to ULD Successfully");
        }

        function LoopPrintMFT() {
            var hidPrintMFTControl = document.getElementById("<%= HidPrintMFT.ClientID %>");
            var hidPrintMFTVal = hidPrintMFTControl.value;
            var PrintMFTArr = hidPrintMFTVal.split("|");


            for (var i = 0; i < PrintMFTArr.length; i++) {
                window.open(PrintMFTArr[i]);
            }
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

        function GenerateInvoices() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");

            var InvList = hfInvNos.value;

            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=WalkIn' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowWalkInAgentInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }
        
    </script>

    <script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>

    <%--<script type="text/javascript">
 
  $(function() {     alert('not going');
         $('#<%=ddlReason.ClientID %>').change(function() {
           alert('s');
             var ddllevel = document.getElementById('<%=ddlReason.ClientID%>');
             var level = ddllevel.options[ddllevel.selectedIndex].value;
             alert(ddllevel);
             document.getElementById("<%=txtReason.ClientID %>").disabled = false;
             
             if (level == "Others") {
             alert('o');
                 document.getElementById("<%= txtReason.ClientID %>").disabled = true;
             }
            
         });
         }
 
</script>  --%>

    <script language="javascript" type="text/javascript">

        function AllAWBRowWise(headerchkbox) {
            //alert("Correct form");
            var gvcheckAllRow = document.getElementById("<%=gdvULDLoadPlanAWB.ClientID %>");
            var p;
            //alert("1");
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            var i;


            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchkbox.checked) {
                for (i = 0; i < gvcheckAllRow.rows.length; i++) {
                    var inputs = gvcheckAllRow.rows[i].getElementsByTagName('input');

                    for (var j = 0; j < inputs.length; j++) {
                        inputs[0].checked = true;
                        inputs[j].checked = true;
                    }
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheckAllRow.rows.length; i++) {
                    var inputs = gvcheckAllRow.rows[i].getElementsByTagName('input');

                    for (var j = 0; j < inputs.length; j++) {
                        inputs[0].checked = false;
                        inputs[j].checked = false;
                    }
                }
            }
        }

        function AllAssignedAWBRowWise(headerchkbox) {
            //alert("Correct form");
            var gvcheckAllRow = document.getElementById("<%=grdBulkAssignedAWB.ClientID %>");
            var p;
            //alert("1");
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            var i;


            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchkbox.checked) {
                for (i = 0; i < gvcheckAllRow.rows.length; i++) {
                    var inputs = gvcheckAllRow.rows[i].getElementsByTagName('input');

                    for (var j = 0; j < inputs.length; j++) {
                        inputs[0].checked = true;
                        inputs[j].checked = true;
                    }
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheckAllRow.rows.length; i++) {
                    var inputs = gvcheckAllRow.rows[i].getElementsByTagName('input');

                    for (var j = 0; j < inputs.length; j++) {
                        inputs[0].checked = false;
                        inputs[j].checked = false;
                    }
                }
            }
        }

        function AllCartsRowWise(headerchkbox) {
            //alert("Correct form");
            var gvcheckAllRow = document.getElementById("<%=grdCartList.ClientID %>");
            var p;
            //alert("1");
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            var i;


            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchkbox.checked) {
                for (i = 0; i < gvcheckAllRow.rows.length; i++) {
                    var inputs = gvcheckAllRow.rows[i].getElementsByTagName('input');

                    for (var j = 0; j < inputs.length; j++) {
                        inputs[0].checked = true;
                        inputs[j].checked = true;
                    }
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheckAllRow.rows.length; i++) {
                    var inputs = gvcheckAllRow.rows[i].getElementsByTagName('input');

                    for (var j = 0; j < inputs.length; j++) {
                        inputs[0].checked = false;
                        inputs[j].checked = false;
                    }
                }
            }
        }

        function AllULDsRowWise(headerchkbox) {
            //alert("Correct form");
            var gvcheckAllRow = document.getElementById("<%=grdULDList.ClientID %>");
            var p;
            //alert("1");
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            var i;


            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchkbox.checked) {
                for (i = 0; i < gvcheckAllRow.rows.length; i++) {
                    var inputs = gvcheckAllRow.rows[i].getElementsByTagName('input');

                    for (var j = 0; j < inputs.length; j++) {
                        inputs[0].checked = true;
                        inputs[j].checked = true;
                    }
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheckAllRow.rows.length; i++) {
                    var inputs = gvcheckAllRow.rows[i].getElementsByTagName('input');

                    for (var j = 0; j < inputs.length; j++) {
                        inputs[0].checked = false;
                        inputs[j].checked = false;
                    }
                }
            }
        }
        
        //Function to select/ unselect all rows in AWB Details grid.
        function SelectAllAWB(headerchk) {
            var gvcheck = document.getElementById("<%=gdvULDLoadPlanAWB.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    alert(i.toString() + "first");
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');

                    inputs[i].checked = true;
                    alert(i.toString() + "second");
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    alert(i.toString() + "first");
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');

                    inputs[i].checked = false;

                    alert(i.toString() + "second");

                }
            }
        }

        function SelectAllAWBBulk(headerchk) {
            var gvcheck = document.getElementById("<%=grdBulkAssignedAWB.ClientID %>");
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

        function ShowHideTextBox() {
            var ddl = document.getElementById("<%= ddlReason.ClientID %>").value;

            // var ddl = document.getElementById(ddlId.id);
            // alert(ddl);
            //var theControl = document.getElementById("txtReason");
            document.getElementById("txtReason").style.display = (ddl == "Others") ? "block" : "none";
            document.getElementById("<%= txtReason.ClientID %>").disabled = false;
            if (ddl.value == 'Others')  //your condition
            {
                // alert('others');
                //document.getElementById("txtReason").style.display = "none";
                document.getElementById("<%= txtReason.ClientID %>").disabled = false;
            }
            else if (ddl.value != 'Others') {
                //alert('No space');
                //  document.getElementById("txtReason").style.display = "block";
                document.getElementById("<%= txtReason.ClientID %>").disabled = true;
            }
        } 
    </script>

    <%--<script  type="text/javascript">
    window.onload = function() { document.getElementById('<%= txtReason.ClientID %>').style.display = "none"; };

</script>--%>
    <style type="text/css">
        
        .ajax__tab_xp .ajax__tab_body{width:342px; border:1px solid red;}
        
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
        /*Added from Capacity Planning.*/.tbcen
        {
            text-align: center;
        }
        .op
        {
            z-index: 99999;
        }
        .showh
        {
            display: none;
        }
        .highlight
        {
            background: #7eabfe !important;
            height: 30px;
            color: #fff;
        }
    </style>
    <style type="text/css">
        .styleUpper
        {
            text-transform: uppercase;
        }
    </style>
    <style>
        .ajax__calendar .ajax__calendar_invalid .ajax__calendar_day
        {
            background-color: gray;
            color: White;
            text-decoration: none;
            cursor: default;
        }
    </style>

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
        function expandcollapseULD(obj, row) {
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
                img.alt = "Close to view other HAWB details";
            }
            else {
                div.style.display = "none";
                if (row == 'alt') {
                    img.src = "plus.gif";
                }
                else {
                    img.src = "plus.gif";
                }
                img.alt = "Expand to view HAWB Details";
            }
        }
    </script>

    <script type="text/javascript">

        function FindSum(textbox) {
            var ValidatePcsID = textbox.id.replace("txtRemainingPcs", "lblPieces");
            var ValidateWeightID = textbox.id.replace("txtRemainingPcs", "lblWeight");
            var TotalPcs = parseInt(document.getElementById(ValidatePcsID).innerHTML);
            var TotalWt = parseFloat(document.getElementById(ValidateWeightID).innerHTML);
            var Index = textbox.id.indexOf("_GVSubHAWB");
            var TargetTextboxID = textbox.id.substring(0, Index);
            TargetTextboxID = TargetTextboxID + "_txtRemainingPcs";
            var TargetTextboxWtID = textbox.id.substring(0, Index);
            TargetTextboxWtID = TargetTextboxWtID + "_txtRemainingWt";
            var TargetTotalID = textbox.id.substring(0, Index);
            TargetTotalID = TargetTotalID + "_lblPieces";
            var TargetTotalWtID = textbox.id.substring(0, Index);
            TargetTotalWtID = TargetTotalWtID + "_lblWeight";
            var HAWBPcs = parseInt(document.getElementById(textbox.id).value);
            var HAWBWtID = textbox.id.replace("txtRemainingPcs", "txtRemainingWt");
            var HAWBWt = parseFloat(document.getElementById(HAWBWtID).value);
            var TotalHAWBPcs = 0;
            var TotalHAWBWt = 0;
            var TargetHAWBGridID = textbox.id.substring(0, Index);
            TargetHAWBGridID = TargetHAWBGridID + "_GVSubHAWB";
            var HAWBGrid = document.getElementById(TargetHAWBGridID);
            var TotalHAWBPieceCount = 0;
            var TotalHAWBWeightCount = 0;
            for (var i = 1; i < HAWBGrid.rows.length; i++) {
                TotalHAWBPieceCount += parseInt(HAWBGrid.rows[i].cells[3].children[0].innerHTML);
                TotalHAWBWeightCount += parseFloat(HAWBGrid.rows[i].cells[4].children[0].innerHTML);
                if (HAWBGrid.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked) {
                    TotalHAWBPcs += parseInt(HAWBGrid.rows[i].cells[7].children[0].value);
                    TotalHAWBWt += parseFloat(HAWBGrid.rows[i].cells[8].children[0].value);
                }
                if (HAWBPcs > TotalPcs) {
                    alert("Pieces cannot be greater than Total Pcs!");
                }
                else {

                    var TargetPcs = (TotalPcs - TotalHAWBPcs);

                    document.getElementById(TargetTextboxID).value = (parseInt(document.getElementById(TargetTotalID).innerHTML) - TotalHAWBPieceCount) + TotalHAWBPcs;

                    var TargetWt = (TotalWt - TotalHAWBWt);
                    document.getElementById(TargetTextboxWtID).value = (parseFloat(document.getElementById(TargetTotalWtID).innerHTML) - TotalHAWBWeightCount) + TotalHAWBWt;
                }

            }
        }
        function FindSumWgt(textbox) {
            var ValidatePcsID = textbox.id.replace("txtRemainingWt", "lblWeight");
            var TotalPcs = parseFloat(document.getElementById(ValidatePcsID).innerHTML);
            var Index = textbox.id.indexOf("_GVSubHAWB");
            var TargetTextboxID = textbox.id.substring(0, Index);
            TargetTextboxID = TargetTextboxID + "_txtRemainingWt";
            var TargetTotalID = textbox.id.substring(0, Index);
            TargetTotalID = TargetTotalID + "_lblWeight";
            var HAWBPcs = parseFloat(document.getElementById(textbox.id).value);
            if (HAWBPcs > TotalPcs) {
                alert("Weight cannot be greater than Total Weight!");
            }
            else {

                var TargetPcs = (TotalPcs - HAWBPcs);
                document.getElementById(TargetTextboxID).value = parseFloat(document.getElementById(TargetTotalID).innerHTML) - TargetPcs;

            }

        }

        function fnHAWBCheck(textbox) {

            var ValidatePcsID = textbox.id.replace("Check2", "lblPieces");
            var ValidateWeightID = textbox.id.replace("Check2", "lblWeight");
            var TotalPcs = parseInt(document.getElementById(ValidatePcsID).innerHTML);
            var TotalWt = parseFloat(document.getElementById(ValidateWeightID).innerHTML);
            var Index = textbox.id.indexOf("_GVSubHAWB");
            var TargetCheckboxID = textbox.id.substring(0, Index);
            TargetCheckboxID = TargetCheckboxID + "_Check2";
            var TargetTextboxID = textbox.id.substring(0, Index);
            TargetTextboxID = TargetTextboxID + "_txtRemainingPcs";
            var TargetTextboxWtID = textbox.id.substring(0, Index);
            TargetTextboxWtID = TargetTextboxWtID + "_txtRemainingWt";
            var TargetTotalID = textbox.id.substring(0, Index);
            TargetTotalID = TargetTotalID + "_lblPieces";
            var TargetTotalWtID = textbox.id.substring(0, Index);
            TargetTotalWtID = TargetTotalWtID + "_lblWeight";
            var HAWBPcs = parseInt(document.getElementById(textbox.id).value);
            var HAWBWtID = textbox.id.replace("Check2", "txtRemainingWt");
            var HAWBWt = parseFloat(document.getElementById(HAWBWtID).value);
            var TotalHAWBPcs = 0;
            var TotalHAWBWt = 0;
            var TargetHAWBGridID = textbox.id.substring(0, Index);
            TargetHAWBGridID = TargetHAWBGridID + "_GVSubHAWB";
            var HAWBGrid = document.getElementById(TargetHAWBGridID);
            var TotalHAWBPieceCount = 0;
            var TotalHAWBWeightCount = 0;
            for (var i = 1; i < HAWBGrid.rows.length; i++) {
                TotalHAWBPieceCount += parseInt(HAWBGrid.rows[i].cells[3].children[0].innerHTML);
                TotalHAWBWeightCount += parseFloat(HAWBGrid.rows[i].cells[4].children[0].innerHTML);
                if (HAWBGrid.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked) {
                    TotalHAWBPcs += parseInt(HAWBGrid.rows[i].cells[7].children[0].value);
                    TotalHAWBWt += parseFloat(HAWBGrid.rows[i].cells[8].children[0].value);
                }
                if (HAWBPcs > TotalPcs) {
                    alert("Pieces cannot be greater than Total Pcs!");
                }
                else {

                    var TargetPcs = (TotalPcs - TotalHAWBPcs);

                    document.getElementById(TargetTextboxID).value = (parseInt(document.getElementById(TargetTotalID).innerHTML) - TotalHAWBPieceCount) + TotalHAWBPcs;
                    var TargetWt = (TotalWt - TotalHAWBWt);
                    document.getElementById(TargetTextboxWtID).value = (parseFloat(document.getElementById(TargetTotalWtID).innerHTML) - TotalHAWBWeightCount) + TotalHAWBWt;
                    document.getElementById(TargetCheckboxID).checked = true;

                }

            }
        }

        //Function to open Notes.
        function OpenNotesChild() {

            var FltNo = document.getElementById("<%= txtFlightCode.ClientID %>").value + '' +
        document.getElementById("<%= txtFlightID.ClientID %>").value;

            var FltDate = document.getElementById("<%= TextBoxdate.ClientID %>").value;

            if (FltNo != '') {
                window.open("Notes.aspx?FltNo=" + FltNo + "&FltDate=" + FltDate, "", "status=0,toolbar=0, menubar=0, width=810, height=500");
            }
            else {
                window.open("Notes.aspx", "", "status=0,toolbar=0, menubar=0, width=810, height=500");
            }
            return false;
        }

        //Function to select/ unselect all rows in AWB Details grid.
        function SelectAllHAWB(headerchk) {
            var HAWBGridID = headerchk.id.replace("_Check2", "_GVSubHAWB");
            var gvcheck = document.getElementById(HAWBGridID);
            var i;
            var checkBoxes = gvcheck.getElementsByTagName("input");
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (var i = 0; i < checkBoxes.length; i++) {
                    if (checkBoxes[i].type == "checkbox") {
                        checkBoxes[i].checked = true;
                    }
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (var i = 0; i < checkBoxes.length; i++) {
                    if (checkBoxes[i].type == "checkbox") {
                        checkBoxes[i].checked = false;
                    }
                }
            }
        }

        function ShowOffloadPopup(ManifestMode) {
            var FlightNo = document.getElementById("<%=txtFlightCode.ClientID %>").value + document.getElementById("<%=txtFlightID.ClientID %>").value;
            var FlightDt = document.getElementById("<%=TextBoxdate.ClientID %>").value;
            var Station = document.getElementById('ctl00_ContentPlaceHolder1_lblDepAirport').innerText;
            //alert(ManifestMode);
            window.open('frmAWBOffload.aspx?FltNo=' + FlightNo + '&FltDt=' + FlightDt + '&Station=BOM&ManifestMode=' + ManifestMode, '', 'width=800px,height=450px,left=200,top=100');
            return false;
        }

        function RefreshList() {
            __doPostBack('RefreshList', '');
        }

        function ValidateRowSelected(action) {
            //alert("Correct form");
            
            var gvcheckAllRow;
            if(action == 'ValidateLoadPlan')
                gvcheckAllRow = document.getElementById("<%=gdvULDLoadPlanAWB.ClientID %>");
            if (action == 'ValidateAssigned')
                gvcheckAllRow = document.getElementById("<%=grdBulkAssignedAWB.ClientID %>");
            if (action == 'ValidateCart')
                gvcheckAllRow = document.getElementById("<%=grdCartList.ClientID %>");
            if (action == 'ValidateULD')
                gvcheckAllRow = document.getElementById("<%=grdULDList.ClientID %>");
            var i = 0;
            var k = 0;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            for (i = 0; i < gvcheckAllRow.rows.length; i++) {
                var inputs = gvcheckAllRow.rows[i].getElementsByTagName('input');
                for (var j = 0; j < inputs.length; j++) 
                {
                    if (inputs[0].checked == true) 
                    {
                        k = 1;
                        break;
                    }
                }
                if (k == 1) {
                    break;
                }
            }
            if (k != 1) {
                alert('Please select atleast one row from the list');
                return (false);
            }
            else {
                return (true);
            }
        }
    
    </script>

    <script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>

    <script type="text/javascript">

        // Function for get Keypress Event & Call List Button 
        $(function() {

            // used to specific Text Box
            //$("#<%=txtAWBNo.ClientID %>").keydown(function(e) {

            // used to $(":text"): Selects (ALL TextBoxes) only text elements on page (input[type=text])
            $("#<%=txtFlightID.ClientID %>").keydown(function(e) {
                if (e.keyCode == 13) { //if this is enter key
                    // Code to call Button
                    $("#<%=BtnList.ClientID %>").trigger('click');
                    //alert("Amit");
                    return true;
                }
            });
        });
        $(function() {
            $(':text').bind('keydown', function(e) {
                //on keydown for all textboxes
                if (e.target.className != "ddlSource")
                //excludes specific textbox like SearchTextBox
                {
                    if (e.keyCode == 13) { //if this is enter key
                        e.preventDefault();
                        return false;
                    }
                    else
                        return true;
                }
                else
                    return true;
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"
    EnableViewState="true">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
        <h1>
            Flight Planning
        </h1>
        <div class="botline">
            Flight :&nbsp; *<asp:TextBox ID="txtFlightCode" runat="server" Width="40px" MaxLength="4"></asp:TextBox>
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtFlightCode"
                WatermarkText="Prefix" />
            <asp:TextBox ID="txtFlightID" runat="server" Width="55px" MaxLength="6" OnClick="BtnList_Click"
                OnTextChanged="txtFlightID_TextChanged"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvFlightID" runat="server" ErrorMessage="*" ValidationGroup="btnList"
                ControlToValidate="txtFlightID">
            </asp:RequiredFieldValidator>
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtFlightID"
                WatermarkText="Flight ID" />
            <asp:TextBox ID="TextBoxdate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
            <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
            <asp:RequiredFieldValidator ID="rfvFlightDate" runat="server" ErrorMessage="*" ValidationGroup="btnList"
                ControlToValidate="TextBoxdate">
            </asp:RequiredFieldValidator>
            <asp:CalendarExtender ID="TextBoxdate_CalendarExtender" runat="server" Enabled="True"
                Format="dd/MM/yyyy" TargetControlID="TextBoxdate" PopupButtonID="imgDate">
            </asp:CalendarExtender>
            &nbsp;&nbsp;
            <asp:Label ID="Label2" runat="server" Text="OR" Font-Bold="true" Visible="false"></asp:Label>
            &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtAWBPrefix" runat="server" Text="" Width="55px"
                MaxLength="4" Visible="false"></asp:TextBox>
            &nbsp;
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAWBPrefix"
                WatermarkText="Prefix" />
            <asp:TextBox ID="txtAWBNo" runat="server" Text="" Width="95px" MaxLength="10" Visible="false"></asp:TextBox>
            &nbsp;&nbsp;
            <asp:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtAWBNo"
                WatermarkText="AWB#" />
            &nbsp; Dep.Airport:&nbsp;
            <asp:Label ID="lblDepAirport" runat="server" Font-Bold="True" Font-Names="Verdana"></asp:Label>
            &nbsp;
            <asp:ImageButton ID="imgNotebtn" ImageAlign="AbsMiddle" runat="server" OnClientClick="javascript:return OpenNotesChild();"
                ImageUrl="~/Images/noteicon.png" ToolTip="Notes" Height="20px" Width="34px" Style="padding-bottom: 2px;" />
            &nbsp; &nbsp; &nbsp;
            <img src="Images/txtflightdetails1.png" />
            <asp:Label ID="lblRoute" runat="server"></asp:Label>
            <asp:Label ID="lblDate" runat="server"></asp:Label>
            &nbsp; &nbsp; &nbsp; Tail No
            <asp:TextBox ID="txtTailNo" runat="server" MaxLength="15" Visible="true" Width="70px"></asp:TextBox>
            <br />
            &nbsp;&nbsp;
            <asp:Button ID="BtnList" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>"
                ValidationGroup="btnList" CssClass="button" OnClick="BtnList_Click" />
            <asp:Button ID="BtnClear" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>"
                CssClass="button" OnClick="BtnClear_Click" />
            <asp:Button ID="btnGetPaxCap" runat="server" Text="Get PAX Cap"
                CssClass="button" onclick="btnGetPaxCap_Click"  />
        </div>
        <div>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
        </div>
        <div id="divdetail">
            <div id="colleft" style="width: 380px">
                <%--<asp:Label ID="lblAWBHeader" Font-Size="Large" runat="server" Text="Bulk Load"></asp:Label>--%>
                
                <div id="DivBulk" runat="server" class="divback" style="width: 360px; background: url(Images/brushed_alu.png);
                    min-height: 300px; -moz-box-shadow: 3px 3px 3px #ccc; -webkit-box-shadow: 3px 3px 3px #ccc;">
                    <div style="margin-top: 0px">
                        <div style="float:right">
                                <asp:Label ID="lblCartHeader" runat="server" Text = "Cart#:"></asp:Label>
                                <asp:TextBox ID="txtCartNumber" runat="server" Text = "" Width="100px" MaxLength="20"></asp:TextBox>
                                <asp:Button ID="btnAssignToCartTop" runat="server" BackColor="Blue" ForeColor="White" Font-Bold="true" Text = "Assign" 
                                 OnClientClick="javascript:return ValidateRowSelected('ValidateLoadPlan');" OnClick="btnAssignBulk_Click"></asp:Button>
                        </div>
                        <asp:TabContainer ID="TabContainer2" runat="server" Width="360px"
                            Height="485px" ScrollBars="Both" Visible="true">
                            <div style="">
                            <asp:TabPanel ID="TabPanelAWB" runat="server" Font-Bold="True" ScrollBars="Both" Width="330px" HeaderText="">
                                <HeaderTemplate>
                                    <asp:Button ID="btnFlightNumber" runat="server" Text="AWB" Height="17" BorderColor="White" onclick="btnFlightNumber_Click"></asp:Button>
                                    <asp:Button ID="btnFltRoute" runat="server" Text="" Height="17" BorderColor="White" onclick="btnFltRoute_Click"></asp:Button>
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:GridView ID="gdvULDLoadPlanAWB" runat="server" AutoGenerateColumns="False" CellPadding="2"
                                        CellSpacing="1" Style="z-index: 1" Width="100%" OnRowCreated="gdvULDLoadPlanAWB_RowCreated">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <a href="javascript:expandcollapse('div<%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>', 'one');">
                                                        <img id="imgdiv<%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>"
                                                            alt="Click to show/hide HAWBs in AWB <%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>"
                                                            width="9px" border="0" src="plus.gif" />
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAllAWB" runat="server" onclick="javascript:AllAWBRowWise(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Check2" runat="server" Enabled="true" onclick="javascript:SelectAllHAWB(this);" />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="False" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="AWBNumber" HeaderText="AWB">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAWBno" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Accepted Pcs" HeaderText="Pcs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPieces" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Accepted Wt" HeaderText="Wt">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWeight" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Remaining Pcs" HeaderText="Rem<br/>Pcs">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRemainingPcs" runat="server" Width="30px"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Remaining Wt" HeaderText="Rem Wt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRemainingWt" runat="server" Width="50px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Comm Desc" HeaderText="Comm Desc">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblCommodityDesc" runat="server" Width="100px"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Shipper" HeaderText="Shipper">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblShipperName" runat="server" Width="80px"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Accept On" HeaderText="Accept On">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAcceptedOn" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Flt #" HeaderText="Flt #">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlightNumber" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Built Pcs" HeaderText="Built<br/>Pcs" 
                                                Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBuiltPcs" runat="server" Width="20px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Prod Type" HeaderText="Prod Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductType" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Location" HeaderText="Location">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocation" runat="server" Width="68px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30px" Wrap="True" />
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Comm Code" HeaderText="Comm Code" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCommodityCode" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Booked Pcs" Visible="False" HeaderText="Booked Pcs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedPcs" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Booked Wt" Visible="False" HeaderText="Booked Wt">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="FlightExists" HeaderText="FlightExists"
                                                Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlightExists" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Built Wt" HeaderText="Built Wt" 
                                                Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBuiltWt" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="SHC" HeaderText="SHC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSHC" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Flt Date" HeaderText="Flt Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFltDate" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td colspan="20" style="border-top: 0px;">
                                                            <div id='div<%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>'
                                                                alt='Click to show/hide AWBs in ULD <%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>'
                                                                style="display: none; position: relative; left: 15px; overflow: auto; width: 97%">
                                                                <strong>HAWB Details</strong><br />
                                                                <asp:GridView ID="GVSubHAWB" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                                    BorderColor="#BFBEBE" BorderStyle="None" CellPadding="2" CellSpacing="3" CssClass="GridViewStyle"
                                                                    EnableViewState="true" HeaderStyle-BackColor="#656667" HeaderStyle-CssClass="AltRowStyle"
                                                                    HeaderStyle-ForeColor="#ffffff" PagerStyle-CssClass="PagerStyle" PageSize="10"
                                                                    RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                                                                    <HeaderStyle CssClass="titlecolr" />
                                                                    <RowStyle HorizontalAlign="Center" />
                                                                    <%--<AlternatingRowStyle HorizontalAlign="Center" />--%>
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <%-- <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAllAWB" runat="server" onclick="javascript:SelectAllHAWB(this);" />
                                        </HeaderTemplate>--%>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="Check2" runat="server" Enabled="true" onclick="javascript:fnHAWBCheck(this)" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Wrap="False" />
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="HAWBNumber" HeaderText="HAWB">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblHAWBno" runat="server" Text='<%# Eval("HAWBNo") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="AWBNumber" HeaderText="AWB">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAWBno" runat="server" Text='<%# Eval("AWBNumber") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="Accepted Pcs" HeaderText="Bulk Pcs">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPieces" runat="server" Text='<%# Eval("TotalPieces") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="Accepted Wt" HeaderText="Bulk Wt">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblWeight" runat="server" Text='<%# Eval("TotalWeight") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="Built Pcs" HeaderText="Built Pcs">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblBuiltPcs" runat="server" Text='<%# Eval("BuiltPcs") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="Built Wt" HeaderText="Built Wt">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblBuiltWt" runat="server" Text='<%# Eval("BuiltWt") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="Remaining Pcs" HeaderText="Rem Pcs">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRemainingPcs" runat="server" onchange="javascript:FindSum(this);"
                                                                                    Text='<%# Eval("RemainingPcs") %>' Width="50px"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="Remaining Wt" HeaderText="Rem Wt">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRemainingWt" runat="server" onchange="javascript:FindSumWgt(this);"
                                                                                    Text='<%# Eval("RemainingWt") %>' Width="50px"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="Location" HeaderText="Location" Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("OriginCode") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="Booked Pcs" HeaderText="Booked Pcs" Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblBookedPcs" runat="server" Text='<%# Eval("BookedPieces") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="Booked Wt" HeaderText="Booked Wt" Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("BookedWeight") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField AccessibleHeaderText="FlightExists" HeaderText="FlightExists"
                                                                            Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFlightExists" runat="server" Text='<%# Eval("FlightExists") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="showh" />
                                                <ItemStyle CssClass="showh" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle CssClass="grdrowfont" />
                                        <FooterStyle CssClass="grdrowfont" />
                                        <HeaderStyle CssClass="titlecolr" Wrap="False" />
                                        <RowStyle CssClass="grdrowfont" HorizontalAlign="Center" Wrap="False" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:TabPanel>
                            </div>
                            
                    </asp:TabContainer>
                        <div style="float:right">
                                <asp:Label ID="lblULDHeader" runat="server" Text = "ULD#:"></asp:Label>
                                <asp:TextBox ID="txtULDNumber" runat="server" Text = "" Width="100px" MaxLength="10"></asp:TextBox>
                                <asp:Button ID="btnAssignToULDBottom" runat="server" BackColor="Blue" ForeColor="White" Text = "Assign" 
                                 OnClick="BtnAddtoManifest_Click" OnClientClick="javascript:return ValidateRowSelected('ValidateLoadPlan');"  ></asp:Button>
                        </div>
                    </div>
                    <div style="margin-top: 3px;">
                        <table cellspacing="9px" cellpadding="5px">
                            <tr>
                                <td>
                                    <asp:Button ID="btnAddULDToManifest" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_ADDULDMANIFEST %>"
                                        CssClass="button" Visible="false" />
                                    <asp:Button ID="BtnAddtoManifest" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_ASSIGNTOULD %>"
                                        CssClass="button" OnClick="BtnAddtoManifest_Click" Visible="false" />
                                    <asp:Button ID="btnAssignBulk" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_ASSIGNTOBULK %>"
                                        CssClass="button" OnClick="btnAssignBulk_Click" Visible="false" />
                                    <asp:Button ID="btnReAssign" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_REASSIGN %>"
                                        CssClass="button" Visible="true" CommandArgument="AWB" 
                                        OnClientClick="javascript:return ValidateRowSelected('ValidateLoadPlan');" OnClick="btnReAssign_Click" />
                                    <asp:Button ID="btnFltPln" runat="server" CssClass="button" Text="Flight Build Plan"
                                        Visible="true" OnClick="btnFltPln_Click" />
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div id="Exportcolright" style="width: 639px;">
                <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
        </asp:Button>--%>
                <div class="ltfloat" style="min-height: 300px; margin-left: 5px; padding-left: 7px;">
                                <div class="divback">
                                        <asp:UpdatePanel ID="updateFetchAWB" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblFetchStatus" runat="server" Text="" Font-Bold="true"></asp:Label>
                                            <br />
                                            <asp:Label ID="lblFetchAWBHeader" runat="server" Text="AWB:"></asp:Label>
                                            <asp:TextBox ID="txtFetchAWBPrefix" runat="server" Text = "" Width="30px"></asp:TextBox>
                                            <asp:TextBox ID="txtFetchAWBNumber" runat="server" Text = "" Width="60px"></asp:TextBox>
                                            <asp:Button ID="btnFetchAWB" runat="server" Text = "Get" CssClass="button" 
                                                onclick="btnFetchAWB_Click"></asp:Button>
                                            <asp:Button ID="btnFetchClear" runat="server" Text = "Clear" CssClass="button" onclick="btnFetchClear_Click" 
                                                ></asp:Button>
                                            &nbsp;<asp:Label ID="lblFetchFltHeader" runat="server" Text="Flt:" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="lblFetchFltDate" runat="server" Text="" Enabled="false" Width="70px"></asp:TextBox>
                                            <asp:TextBox ID="lblFetchFlt" runat="server" Text="" Enabled="false" Width="50px"></asp:TextBox>
                                            <asp:Label ID="lblFetchAvlPcs" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:Label ID="lblFetchAvlWt" runat="server" Text="" Visible="false"></asp:Label>
                                            &nbsp;<asp:Label ID="lblFetchPcsHeader" runat="server" Text="Pcs:" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtFetchPcs" runat="server" Text = "" Width="30px"></asp:TextBox>
                                            <asp:Label ID="lblFetchWtHeader" runat="server" Text = "Wt:" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtFetchWt" runat="server" Text = "" Width="45px"></asp:TextBox>
                                            <asp:Button ID="btnAssignFetchedAWB" runat="server" Text="Assign" 
                                                CssClass="button" onclick="btnAssignFetchedAWB_Click"></asp:Button>
                                        </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnFetchAWB" />
                                                <asp:PostBackTrigger ControlID="btnAssignFetchedAWB" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                     </div>
                    <div style="margin-top: 0px">
                        <h2>
                            <asp:Label ID="lbluld1" runat="server" Text="Assigned AWBs"></asp:Label></h2>
                        <asp:Panel ID="pnlBulkAssignedAWB" runat="server" ScrollBars="Auto" Height="200px"
                            Style="margin-top:5px" BorderStyle="Solid" BorderWidth="1px" Width="620px">
                            <asp:GridView ID="grdBulkAssignedAWB" runat="server" CellPadding="3" CellSpacing="2"
                                AutoGenerateColumns="False" Style="z-index: 1" OnRowCreated="grdBulkAssignedAWB_RowCreated">
                                <%--OnRowDataBound="grdBulkAssignedAWB_RowDataBound"--%>
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href="javascript:expandcollapseULD('div<%# "Bulk" +  Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>', 'one');">
                                                <img id="imgdiv<%# "Bulk" + Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>"
                                                    alt="Click to show/hide HAWBs in AWB <%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>"
                                                    width="9px" border="0" src="plus.gif" />
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAllAWB" runat="server" onclick="javascript:AllAssignedAWBRowWise(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Check0" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="AWBNo" HeaderText="AWBNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAWBNo" runat="server" Width="80px" Text='<%# Eval("AWBNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Built Pcs" HeaderText="Pcs">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBuiltPcs" runat="server" Width="30px" Text='<%# Eval("BuiltPcs") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Built Wt" HeaderText="Bulk Wt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBuiltWt" runat="server" Width="50px" Text='<%# Eval("BuiltWt") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Cart#" HeaderText="Cart#" Visible="true">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblCartNo" runat="server" Width="90px" Text='<%# Eval("CartNumber") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Cart#" HeaderText="OldCart#" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOldCartNo" runat="server" Text='<%# Eval("CartNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ULD#" HeaderText="ULDNo" Visible="true">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblULDNo" runat="server" Width="90px" Text='<%# Eval("ULDNo") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ULD#" HeaderText="OldULDNo" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOldULDNo" runat="server" Text='<%# Eval("ULDNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Flt#" HeaderText="Flt#" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFltNo" runat="server" Width="60px" Text='<%# Eval("FltNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Flt Date" HeaderText="Flt Date" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFltDate" runat="server" Width="70px" Text='<%# Eval("FltDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Comm Description" HeaderText="Comm Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCommodityDesc" runat="server" Text='<%# TruncateString(Eval("description").ToString(), 35) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Prod Type" HeaderText="Prod Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductType" runat="server" Width="60px" Text='<%# Eval("ProductType") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Comm Code" HeaderText="Comm Code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCommodityCode" runat="server" Width="40px" Text='<%# Eval("CommodityCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="SHC" HeaderText="SHC">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSHC" runat="server" Width="40px" Text='<%# Eval("SHC") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Loading Priority" HeaderText="Loading<br/>Priority"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAWBLoadingPriority" runat="server" Width="50px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Builder Name" HeaderText="Builder Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAWBBuilderName" runat="server" Width="80px" Text='<%# Eval("AWBBuilderName") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Cart#" HeaderText="Cart#" Visible="false">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlCart" runat="server">
                                            </asp:DropDownList>
                                            <%--<asp:Label ID="lblCartno" runat="server" Width="80px" Text='<%# Eval("CartNumber") %>'></asp:Label>                                                     --%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Booked Pcs" HeaderText="Bkd Pcs" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalPcs" runat="server" Width="50px" Text='<%# Eval("TotalPcs") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Booked Wt" HeaderText="Bkd Wt" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalWt" runat="server" Width="50px" Text='<%# Eval("TotalWt") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Accepted Pcs" HeaderText="Acc Pcs" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcceptedPcs" runat="server" Width="50px" Text='<%# Eval("AcceptedPcs") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Accepted Wt" HeaderText="Acc Wt" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcceptedWt" runat="server" Width="50px" Text='<%# Eval("AcceptedWt") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Location" HeaderText="Location" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAWBLocation" runat="server" Width="50px" Text='<%# Eval("Location") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="FlightExists" HeaderText="FlightExists"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFlightExists" runat="server" Width="80px" Text='<%# Eval("FlightExists") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <tr>
                                                <td colspan="20" style="border-top: 0px;">
                                                    <div id="div<%# "Bulk" +  Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>"
                                                        alt="Click to show/hide AWBs in ULD <%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>"
                                                        style="display: none; position: relative; left: 15px; overflow: auto; width: 97%">
                                                        <strong>HAWB Details</strong><br />
                                                        <asp:GridView ID="GVSubHAWB" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                            BorderColor="#BFBEBE" BorderStyle="None" CellPadding="2" CellSpacing="3" PageSize="10"
                                                            HeaderStyle-CssClass="AltRowStyle" HeaderStyle-BackColor="#656667" HeaderStyle-ForeColor="#ffffff"
                                                            CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" PagerStyle-CssClass="PagerStyle"
                                                            SelectedRowStyle-CssClass="SelectedRowStyle" EnableViewState="true">
                                                            <HeaderStyle CssClass="titlecolr" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                            <%--<AlternatingRowStyle HorizontalAlign="Center" />--%>
                                                            <Columns>
                                                                <asp:TemplateField Visible="false">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkSelectAllAWB" runat="server" onclick="javascript:SelectAllAWB(this);" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="Check2" runat="server" Enabled="true" Checked="true" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="HAWBNumber" HeaderText="HAWB">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblHAWBno" runat="server" Text='<%# Eval("HAWBNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="AWBNumber" HeaderText="AWB">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAWBno" runat="server" Text='<%# Eval("AWBNumber") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Accepted Pcs" HeaderText="Bulk Pcs">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPieces" runat="server" Text='<%# Eval("TotalPieces") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Accepted Wt" HeaderText="Bulk Wt">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblWeight" runat="server" Text='<%# Eval("TotalWeight") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Built Pcs" HeaderText="Built Pcs" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBuiltPcs" runat="server" Text='<%# Eval("BuiltPcs") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Built Wt" HeaderText="Built Wt" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBuiltWt" runat="server" Text='<%# Eval("BuiltWt") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Remaining Pcs" HeaderText="Rem Pcs" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemainingPcs" onchange="javascript:FindSum(this);" runat="server"
                                                                            Width="50px" Text='<%# Eval("RemainingPcs") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Remaining Wt" HeaderText="Rem Wt" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemainingWt" onchange="javascript:FindSumWgt(this);" runat="server"
                                                                            Width="50px" Text='<%# Eval("RemainingWt") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Location" HeaderText="Location" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("OriginCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Booked Pcs" Visible="False" HeaderText="Booked Pcs">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBookedPcs" runat="server" Text='<%# Eval("BookedPieces") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Booked Wt" Visible="False" HeaderText="Booked Wt">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("BookedWeight") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="FlightExists" HeaderText="FlightExists"
                                                                    Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFlightExists" runat="server" Text='<%# Eval("FlightExists") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="showh" />
                                        <ItemStyle CssClass="showh" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="titlecolr" />
                                <%--<AlternatingRowStyle CssClass="trcolor" Wrap="False" />--%>
                                <EditRowStyle CssClass="grdrowfont" />
                                <RowStyle CssClass="grdrowfont" Wrap="False" HorizontalAlign="Center" />
                                <FooterStyle CssClass="grdrowfont" />
                            </asp:GridView>
                        </asp:Panel>
                        <div style="margin-top: 10px;">
                            <asp:Button ID="btnSave" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_SAVE %>"
                                Enabled="true" ValidationGroup="btnSaveULD" OnClientClick="javascript:return ValidateRowSelected('ValidateAssigned');" 
                                OnClick="Save1_Click" />
                            <asp:Button ID="btnUnassign" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_UNASSIGN %>"
                                Enabled="true" OnClick="btnUnassign_Click" OnClientClick="javascript:return ValidateRowSelected('ValidateAssigned');" />
                            <asp:Button ID="btnSplitUnassign" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_SPLIT&UNASSIGN %>"
                                Visible="false" />
                            <asp:Button ID="btnSaveBulkAWB" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_SAVEBULKAWB %>"
                                Enabled="true" OnClick="btnSaveBulkAWB_Click" OnClientClick="javascript:return ValidateRowSelected('ValidateAssigned');" />
                            <asp:Button ID="btnUnAssignBulkAWB" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_UNASSIGNBULK %>"
                                Enabled="true" OnClick="btnUnAssignBulkAWB_Click" OnClientClick="javascript:return ValidateRowSelected('ValidateAssigned');" />
                        </div>
                        <h2>Cart Load</h2>
                        <asp:Panel ID="pnlCartList" runat="server" ScrollBars="Auto" Height="155px" Style="margin-top: 0px"
                            BorderStyle="Solid" BorderWidth="1px" Width="620px">
                            <asp:GridView ID="grdCartList" runat="server" CellPadding="3" CellSpacing="2" AutoGenerateColumns="False"
                                Style="z-index: 1">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAllCarts" runat="server" onclick="javascript:AllCartsRowWise(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="rdCartList" AutoPostBack="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cart#">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCartNo" runat="server" CssClass="styleUpper" Text='<%# Eval("CartNo") %>'
                                                Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Scale Wt" HeaderText="Scale Wt">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtScaleWeight" runat="server" Text='<%# Eval("ScaleWeight") %>'
                                                Width="40px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Cart Status" HeaderText="Est. Bulk Weight">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBulkWeight" runat="server" Text='<%# Eval("BulkWeight") %>' Width="50px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Loading Priority" HeaderText="Loading<br/>Priority">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCartLoadingPriority" runat="server" CssClass="styleUpper" Text='<%# Eval("LoadingPriority") %>'
                                                Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Cart Status" HeaderText="ULD Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCartStatus" runat="server" Text='<%# Eval("CartStatus") %>' Width="50px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Builder Name" HeaderText="Builder Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCartBuilderName" runat="server" CssClass="styleUpper" Text='<%# Eval("CartBuilderName") %>'
                                                Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Old Cart No" HeaderText="Old Cart No" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOldCartNo" runat="server" CssClass="styleUpper" Text='<%# Eval("CartNo") %>'
                                                Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="titlecolr" />
                                <%--<AlternatingRowStyle CssClass="trcolor" Wrap="False" />--%>
                                <EditRowStyle CssClass="grdrowfont" />
                                <RowStyle CssClass="grdrowfont" Wrap="False" HorizontalAlign="Center" />
                                <FooterStyle CssClass="grdrowfont" />
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <div style="margin-top: 10px;">
                        <asp:Button ID="btnNewCart" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_NEWCART %>"
                            Visible="false" CssClass="button" OnClick="btnNewCart_Click"></asp:Button>
                        <asp:Button ID="btnSaveCart" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_SAVECART %>"
                            Enabled="true" ValidationGroup="btnSaveULD" OnClick="btnSaveCart_Click" 
                            OnClientClick="javascript:return ValidateRowSelected('ValidateCart');" />
                        <asp:Button ID="btnDeleteCart" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_DELETECART %>"
                            Enabled="true" OnClientClick="if(ValidateRowSelected('ValidateCart')) { if ( ! UserDeleteCartConfirmation()) return false } else return (false);"
                            OnClick="btnDeleteCart_Click" />
                        <asp:Button ID="btnReassignCart" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_REASSIGN %>"
                            CssClass="button" OnClick="btnReAssignULD_Click" OnClientClick="javascript:return ValidateRowSelected('ValidateCart');" 
                            Visible="true" CommandArgument="CART" />
                    </div>
                    <div>
                        <h2>
                            <asp:Label ID="lbluld" runat="server" Text="ULD Load"></asp:Label></h2>
                        <asp:Panel ID="pnlULDList" runat="server" ScrollBars="Auto" Height="140px" Style="margin-top: 0px"
                            BorderStyle="Solid" BorderWidth="1px" Width="620px">
                            <asp:Label ID="lblULDStatus" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="grdULDList" runat="server" CellPadding="3" CellSpacing="2" AutoGenerateColumns="False"
                                Style="z-index: 1">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAllULDs" runat="server" onclick="javascript:AllULDsRowWise(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="rdULDList" AutoPostBack="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ULD#">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtULDNo" runat="server" CssClass="styleUpper" Text='<%# Eval("ULDNo") %>'
                                                Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="POL" HeaderText="POL">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtULDPOL" runat="server" CssClass="styleUpper" Text='<%# Eval("ULDOrigin") %>'
                                                Width="50px" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="POU" HeaderText="POU">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtULDPOU" runat="server" CssClass="styleUpper" Text='<%# Eval("ULDDest") %>'
                                                Width="50px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField AccessibleHeaderText="AWB Count" DataField="AWBCount" 
                                                            HeaderText="AWB Count" />--%>
                                    <asp:TemplateField AccessibleHeaderText="AWB Count" HeaderText="AWB Count" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAWBCount" runat="server" CssClass="styleUpper" Text='<%# Eval("AWBCount") %>'
                                                Width="50px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Scale Wt" HeaderText="Scale Wt">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtScaleWeight" runat="server" Text='<%# Eval("ScaleWeight") %>'
                                                Width="40px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Loading Priority" HeaderText="Loading<br/>Priority">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtULDLoadingPriority" runat="server" CssClass="styleUpper" Text='<%# Eval("LoadingPriority") %>'
                                                Width="40px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField AccessibleHeaderText="ULD Status" DataField="ULDStatus" 
                                                            HeaderText="ULD Status" />--%>
                                    <asp:TemplateField AccessibleHeaderText="ULD Status" HeaderText="ULD Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblULDStatus" runat="server" Text='<%# Eval("ULDStatus") %>' Width="50px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Location" HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtULDLocation" runat="server" CssClass="styleUpper" Text='<%# Eval("Location") %>'
                                                Width="50px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Builder Name" HeaderText="Builder Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtULDBuilderName" runat="server" CssClass="styleUpper" Text='<%# Eval("ULDBuilderName") %>'
                                                Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField AccessibleHeaderText="Tare Wt" DataField="TareWt" 
                                                            HeaderText="Tare Wt" /> 
                                                     <asp:BoundField AccessibleHeaderText="ULD Wt" DataField="ULDWt" 
                                                            HeaderText="AWB Wt" />--%>
                                    <asp:TemplateField AccessibleHeaderText="Tare Wt" HeaderText="Tare Wt" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTareWt" runat="server" Text='<%# Eval("TareWt") %>' Width="40px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ULD Wt" HeaderText="AWB Wt" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblULDWt" runat="server" Text='<%# Eval("ULDWt") %>' Width="40px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Container Type" HeaderText="Container Type"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlContainerType" runat="server">
                                                <asp:ListItem Text="Standard" Value="Standard"></asp:ListItem>
                                                <asp:ListItem Text="Fast" Value="Fast"></asp:ListItem>
                                                <asp:ListItem Text="FRAloco" Value="FRAloco"></asp:ListItem>
                                                <asp:ListItem Text="MAIL" Value="MAIL"></asp:ListItem>
                                                <asp:ListItem Text="XCS" Value="XCS"></asp:ListItem>
                                                <asp:ListItem Text="DGR" Value="DGR"></asp:ListItem>
                                                <asp:ListItem Text="PER" Value="PER"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Dolly Weight" HeaderText="Dolly Weight"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDollyWt" runat="server" Width="80px" Text="0"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="IsReceived" HeaderText="IsReceived" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsReceived" runat="server" Text='<%# Eval("IsReceived") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="AWBNumber" HeaderText="AWB #" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAWBNumber" runat="server" Text='<%# Eval("AWBNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ULD#" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOldULDNo" runat="server" CssClass="styleUpper" Text='<%# Eval("ULDNo") %>'
                                                Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="titlecolr" />
                                <%--<AlternatingRowStyle CssClass="trcolor" Wrap="False" />--%>
                                <EditRowStyle CssClass="grdrowfont" />
                                <RowStyle CssClass="grdrowfont" Wrap="False" HorizontalAlign="Center" />
                                <FooterStyle CssClass="grdrowfont" />
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <asp:Button ID="btnNewULD" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_NEWULD %>"
                        Visible="false" CssClass="button" OnClick="btnNewULD_Click" ></asp:Button>
                    <asp:Button ID="Save1" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_SAVEULD %>"
                        Enabled="true" ValidationGroup="btnSaveULD" OnClick="Save1_Click" OnClientClick="javascript:return ValidateRowSelected('ValidateULD');" />
                    <asp:Button ID="btnDeleteULD" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_DELETEULD %>" 
                    OnClientClick="if(ValidateRowSelected('ValidateULD')) { if ( ! UserDeleteConfirmation()) return false } else return (false);" 
                    Enabled="true" OnClick="btnDeleteULD_Click" />
                    <asp:Button ID="btnPrintWtStmt" runat="server"  OnClientClick="javascript:return ValidateRowSelected('ValidateULD');" Text="<%$ Resources:LabelNames, LBL_BTN_PRINTWTSTMT %>"
                        Enabled="false" CssClass="button" OnClick="btnPrintWtStmt_Click" />
                    <asp:Button ID="btnPrnULDPlan" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_PRINTULDPLAN %>"
                        CssClass="button" OnClick="btnPrnULDPlan_Click"  OnClientClick="javascript:return ValidateRowSelected('ValidateULD');"/>
                    <asp:Button ID="btnFinalizeULD" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_FINALIZEULD %>"
                        Enabled="true" OnClick="btnFinalizeULD_Click"  OnClientClick="javascript:return ValidateRowSelected('ValidateULD');"/>
                    <asp:Button ID="btnReOpenULD" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_REOPENULD %>"
                        Enabled="true" OnClick="btnReOpenULD_Click"  OnClientClick="javascript:return ValidateRowSelected('ValidateULD');"/>
                    <asp:Button ID="btnRemove" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_REASSIGN %>"
                        CssClass="button" OnClick="btnReAssignULD_Click" Visible="true" OnClientClick="javascript:return ValidateRowSelected('ValidateULD');" 
                        CommandArgument="ULD" />
                    <asp:Button ID="btnPrintLoadPlan" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_PRINTLOADPLAN %>"
                                CssClass="button" OnClick="btnPrintLoadPlan_Click" />
                    <asp:Button ID="btnExportToManifest" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_EXPORTTOMANIFEST %>"
                                CssClass="button" OnClick="btnExportToManifest_Click" />
                    <asp:Button ID="btnNoToc" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_NOTOC %>" 
                                CssClass="button" onclick="btnNoToc_Click"/>
                    <div style="margin-top: 25px">
                        <asp:Panel ID="Pnlgrd" runat="server" ScrollBars="Auto" Height="200px" Style="margin-top: 20px"
                            BorderStyle="Solid" BorderWidth="1px" Width="620px">
                            <fieldset id="BulkSummary0" style="border: 1px solid #69b3d8;">
                                <table style="width: 50%;" cellspacing="3px" cellpadding="3px">
                                    <tr>
                                        <!--<td><img src="Images/txtbulksummery1.png" /></td>-->
                                        <td>
                                            <b>AWBs : </b>
                                            <asp:Label ID="lblAWBCnt" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <b>PCS : </b>
                                            <asp:Label ID="lblAWBPCS" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <b>&nbsp;Wt : </b>
                                            <asp:Label ID="lblAWBWt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <%--<asp:GridView ID="gdvULDDetails" runat="server" CellPadding="3" CellSpacing="2" AutoGenerateColumns="False"
                                Style="z-index: 1" OnRowCreated="gdvULDDetails_RowCreated">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href="javascript:expandcollapseULD('div<%# "ULD" +  Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>', 'one');">
                                                <img id="imgdiv<%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>"
                                                    alt="Click to show/hide HAWBs in AWB <%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>"
                                                    width="9px" border="0" src="plus.gif" />
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Check0" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="AWBNo" HeaderText="AWB">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAWBNo" runat="server" Width="80px" Text='<%# Eval("AWBNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Built Pcs" HeaderText="Pcs">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBuiltPcs" runat="server" Width="40px" Text='<%# Eval("BuiltPcs") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Flt#" HeaderText="Flt#">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFltNo" runat="server" Width="60px" Text='<%# Eval("FltNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Flt Date" HeaderText="Flt Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFltDate" runat="server" Width="70px" Text='<%# Eval("FltDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Comm Description" HeaderText="Comm Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCommodityDesc" runat="server" Text='<%# TruncateString(Eval("description").ToString(), 35) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Prod Type" HeaderText="Prod Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductType" runat="server" Width="60px" Text='<%# Eval("ProductType") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Comm Code" HeaderText="Comm Code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCommodityCode" runat="server" Width="40px" Text='<%# Eval("CommodityCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="SHC" HeaderText="SHC">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSHC" runat="server" Width="40px" Text='<%# Eval("SHC") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Builder Name" HeaderText="Builder Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAWBBuilderName" runat="server" Width="80px" Text='<%# Eval("AWBBuilderName") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="ULD#" HeaderText="ULD" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblULDNo" runat="server" Width="80px" Text='<%# Eval("ULDNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Booked Pcs" HeaderText="Bkd Pcs" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalPcs" runat="server" Width="40px" Text='<%# Eval("TotalPcs") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Booked Wt" HeaderText="Bkd Wt" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalWt" runat="server" Width="40px" Text='<%# Eval("TotalWt") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Accepted Pcs" HeaderText="Bulk Pcs" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcceptedPcs" runat="server" Width="40px" Text='<%# Eval("AcceptedPcs") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Accepted Wt" HeaderText="Bulk Wt" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcceptedWt" runat="server" Width="40px" Text='<%# Eval("AcceptedWt") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Built Wt" HeaderText="Built Wt" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBuiltWt" runat="server" Width="40px" Text='<%# Eval("BuiltWt") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Location" HeaderText="Location" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAWBLocation" runat="server" Width="50px" Text='<%# Eval("Location") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Loading Priority" HeaderText="Loading Priority"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAWBLoadingPriority" runat="server" Width="80px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="FlightExists" HeaderText="FlightExists"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFlightExists" runat="server" Width="80px" Text='<%# Eval("FlightExists") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <tr>
                                                <td colspan="20" style="border-top: 0px;">
                                                    <div id="div<%# "ULD" + Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>"
                                                        alt="Click to show/hide AWBs in ULD <%# Eval("FltNo") + "," + Eval("FltDate")+ "," + Session["Station"].ToString() + "," + Eval("AWBNo") %>"
                                                        style="display: none; position: relative; left: 15px; overflow: auto; width: 97%">
                                                        <strong>HAWB Details</strong><br />
                                                        <asp:GridView ID="GVSubHAWB" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                            BorderColor="#BFBEBE" BorderStyle="None" CellPadding="2" CellSpacing="3" PageSize="10"
                                                            HeaderStyle-CssClass="AltRowStyle" HeaderStyle-BackColor="#656667" HeaderStyle-ForeColor="#ffffff"
                                                            CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" PagerStyle-CssClass="PagerStyle"
                                                            SelectedRowStyle-CssClass="SelectedRowStyle" EnableViewState="true">
                                                            <HeaderStyle CssClass="titlecolr" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                            <AlternatingRowStyle HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField Visible="false">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkSelectAllAWB" runat="server" onclick="javascript:SelectAllAWB(this);" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="Check2" runat="server" Enabled="true" Checked="true" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="HAWBNumber" HeaderText="HAWB">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblHAWBno" runat="server" Text='<%# Eval("HAWBNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="AWBNumber" HeaderText="AWB">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAWBno" runat="server" Text='<%# Eval("AWBNumber") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Accepted Pcs" HeaderText="Bulk Pcs">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPieces" runat="server" Text='<%# Eval("TotalPieces") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Accepted Wt" HeaderText="Bulk Wt">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblWeight" runat="server" Text='<%# Eval("TotalWeight") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Built Pcs" HeaderText="Built Pcs">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBuiltPcs" runat="server" Text='<%# Eval("BuiltPcs") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Built Wt" HeaderText="Built Wt">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBuiltWt" runat="server" Text='<%# Eval("BuiltWt") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Remaining Pcs" HeaderText="Rem Pcs" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemainingPcs" onchange="javascript:FindSum(this);" runat="server"
                                                                            Width="50px" Text='<%# Eval("RemainingPcs") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Remaining Wt" HeaderText="Rem Wt" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemainingWt" onchange="javascript:FindSumWgt(this);" runat="server"
                                                                            Width="50px" Text='<%# Eval("RemainingWt") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Location" HeaderText="Location" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("OriginCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Booked Pcs" Visible="False" HeaderText="Booked Pcs">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBookedPcs" runat="server" Text='<%# Eval("BookedPieces") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="Booked Wt" Visible="False" HeaderText="Booked Wt">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("BookedWeight") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField AccessibleHeaderText="FlightExists" HeaderText="FlightExists"
                                                                    Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFlightExists" runat="server" Text='<%# Eval("FlightExists") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="showh" />
                                        <ItemStyle CssClass="showh" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="titlecolr" />
                                <AlternatingRowStyle CssClass="trcolor" Wrap="False" />
                                <EditRowStyle CssClass="grdrowfont" />
                                <RowStyle CssClass="grdrowfont" Wrap="False" HorizontalAlign="Center" />
                                <FooterStyle CssClass="grdrowfont" />
                            </asp:GridView>--%>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />
    <div id="fotbut">
        <%--<input name="Save" type="button" value="Save" />--%>
        <asp:HiddenField ID="hfULDNumber" runat="server" Value="" />
        <asp:HiddenField ID="hfInvoiceNos" runat="server" Value="" />
        <asp:HiddenField ID="hdnManifestFlag" runat="server" Value="" />
        <asp:HiddenField ID="hfCartNumber" runat="server" Value="" />
    </div>
    <asp:Panel ID="pnlGrid" runat="server" BackColor="White" ScrollBars="Auto" Visible="false"
        BorderStyle="Solid" Height="385px" Width="600px">
        <div style="margin: 10px;">
            <asp:GridView ID="grdAWBs" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="AWB">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAWBno" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pieces">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPCS" runat="server" MaxLength="4" Width="55px" Enabled="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Weight">
                        <ItemTemplate>
                            <asp:TextBox ID="txtweight" runat="server" MaxLength="4" Enabled="true" Width="55px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Manifested Pcs" HeaderStyle-Wrap="true" HeaderStyle-Width="10px">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAvlPCS" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Gross Wt" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAwlWeight" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Origin" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                        <ItemTemplate>
                            <asp:TextBox ID="txtOrigin" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Destination" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDestination" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ULD/Cart #" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                        <ItemTemplate>
                            <asp:Label ID="lblULDNO" runat="server" Width="80px" Enabled="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="titlecolr" />
                <RowStyle HorizontalAlign="Center" />
                <%--<AlternatingRowStyle HorizontalAlign="Center" />--%>
            </asp:GridView>
            <%--<table width="100%">
         <tr>
         <td>
           AWBNumber
         </td>
         <td>
          <asp:TextBox ID="txtAWBNumberRoute" runat="server" Text="" Width="100px" Enabled="false"></asp:TextBox>
         </td>
           <td>
            Origin
           </td>
           <td>
            <asp:TextBox ID="txtOrigin" runat="server" Text="" Width="100px" Enabled="false"></asp:TextBox>
           </td>
           <td>
            Destination 
           </td>
           <td>
             <asp:TextBox ID="txtDestination" runat="server" Text="" Width="100px" Enabled="false"></asp:TextBox>
           </td>
           </tr>
           <tr>
           <td>
            Pieces
           </td>
           <td>
             <asp:TextBox ID="txtPiecesRoute" runat="server" Text="" Width="100px" Enabled="false"></asp:TextBox>
           </td>
           <td>
            Weight
           </td>
           <td>
            <asp:TextBox ID="txtWeightRoute" runat="server" Text="" Width="100px" Enabled="false"></asp:TextBox>
           </td>
           
         </tr>
        </table>--%>
            <%-- <fieldset style="width: 600px">  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Route Details</legend>
--%>
            <div style="float: left" id="Update">
                <asp:UpdatePanel ID="UpdatePanelRouteDetails" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="LBLRouteStatus" runat="server" ForeColor="Red"></asp:Label>
                        <h2 style="width: 600px">
                            Route Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnAddRouteDetails" runat="server" Text="Add" CssClass="button" OnClick="btnAddRouteDetails_Click" />
                            &nbsp;
                            <asp:Button ID="btnDeleteRouteDetails" runat="server" Text="Delete" CssClass="button"
                                OnClick="btnDeleteRoute_Click" />
                            &nbsp;
                        </h2>
                        <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" CssClass="grdrowfont"
                            Width="399px" ID="grdRouting">
                            <%--<AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>--%>
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CHKSelect" runat="server" />
                                        <asp:HiddenField ID="HidScheduleID" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Flight Origin *" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFltOrig" runat="server" Width="55px" CssClass="styleUpper" onchange="javascript:getFlightNumbers(this);"
                                            Text='<%# Eval("FltOrigin") %>'> 
                                        </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Flight Destination*" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFltDest" runat="server" Width="55px" Text='<%# Eval("FltDestination") %>'
                                            CssClass="styleUpper" OnTextChanged="txtFltDest_TextChanged" AutoPostBack="true">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Partner Type">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlPartnerType" OnSelectedIndexChanged='ddlPartnerType_SelectionChange'
                                            runat="server" AutoPostBack="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Partner Code">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlPartner" OnSelectedIndexChanged='ddlPartner_SelectionChange'
                                            runat="server" AutoPostBack="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Flight    Date *" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFdate" runat="server" Width="80px" Text='<%# Eval("FltDate") %>'
                                            AutoPostBack="True" OnTextChanged="txtFdate_TextChanged" onblur="javascript:txtDatefocus();"></asp:TextBox>
                                        <asp:CalendarExtender ID="TextBox7_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtFdate" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Flight #*" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlFltNum" runat="server" Width="90px" OnSelectedIndexChanged="txtFltNumber_TextChanged"
                                            AutoPostBack="false">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtFlightID" runat="server" Visible="false" AutoPostBack="false"
                                            Width="90px"></asp:TextBox>
                                        <asp:TextBox ID="NewFlightID" runat="server" Visible="false"></asp:TextBox>
                                        <asp:HiddenField ID="hdnFltNum" runat="server" Value='<%# Eval("FltNumber") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pcs">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPcs" runat="server" Width="70px" Text='<%# Eval("Pcs") %>' MaxLength="5">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gross Wt">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtWt" runat="server" Width="80px" Text='<%# Eval("Wt") %>' MaxLength="9">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtLocation" runat="server" Width="80px" MaxLength="50">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                            <FooterStyle CssClass="grdrowfont"></FooterStyle>
                            <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                            <RowStyle CssClass="grdrowfont"></RowStyle>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <%--</fieldset>--%>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblNextFlight" Text="Asgn. to Nxt Flt" runat="server" Visible="false">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtNextFlight" runat="server" Visible="false" Width="85px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblNFltDate" Text="Nxt Flt Dt." runat="server" Visible="false">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtNFltDate" runat="server" Visible="false" Width="100px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/yyyy"
                            TargetControlID="txtNFltDate">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblReason" Text="Reason" runat="server" Visible="false"></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlReason" runat="server" onchange="javascript:return select();">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtReason" runat="server" Width="335px"></asp:TextBox><%--       <input type="txtReason" id="other" name="other" style="display: none;"/>   
--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAddManifest" runat="server" Text="Add To Manifest" CssClass="button"
                            OnClick="btnAddManifest_Click"></asp:Button>
                        <%--            <Button ID="Button1"  OnClientClick=="javascript:PassValues();" Text="Button" />
--%>
                        <asp:Button ID="btnShowEAWB" runat="server" Text="Click Me" CssClass="button" OnClientClick="callexport();"
                            Visible="False" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
                        </asp:Button>
                    </td>
                    <td>
                        <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
        </asp:Button>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    </div><div visible="false">
        <asp:HiddenField ID="HidSource" runat="server" />
        <asp:HiddenField ID="HidDest" runat="server" />
        <asp:HiddenField ID="HidPcsCount" runat="server" />
        <asp:HiddenField ID="HidVia" runat="server" />
        <asp:HiddenField ID="HidWt" runat="server" />
        <asp:HiddenField ID="HidDimension" runat="server" />
        <asp:HiddenField ID="HidFlightsChanged" runat="server" />
        <asp:HiddenField ID="HidAWBNumber" runat="server" />
        <asp:HiddenField ID="HidChangeDate" runat="server" />
        <asp:HiddenField ID="HidScheduleID" runat="server" />
        <asp:HiddenField ID="HidPrintMFT" runat="server" />
    </div>
    <div id="fadesplit" class="black_overlay">
    </div>
    <div id="Lightsplit" class="white_content">
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
                    <asp:Label ID="lblEmail" runat="server" Text="To Email ID : (Comma Seprated EmailID)"
                        ForeColor="Blue"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <asp:TextBox ID="txtEmailID" runat="server" TextMode="MultiLine" Width="300px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
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
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnOK" CssClass="button" runat="server" Text="Send Email" />
                </td>
                <td>
                    <asp:Button ID="btnSitaUpload" CssClass="button" runat="server" Text="Send via SITA" />
                </td>
                <td>
                    <asp:Button ID="btnFTPUpload" CssClass="button" runat="server" Text="FTP Upload" />
                </td>
                <td>
                    <input type="button" id="Button1" class="button" value="Cancel" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdRemainingPcs" runat="server" />
    <asp:HiddenField ID="hdRemainingWt" runat="server" />
</asp:Content>
