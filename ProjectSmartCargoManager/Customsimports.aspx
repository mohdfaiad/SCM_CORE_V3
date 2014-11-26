<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="Customsimports.aspx.cs" Inherits="ProjectSmartCargoManager.Customsimports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"  EnableViewState="true">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <script src='http://code.jquery.com/jquery-latest.js'></script>
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

<script type="text/javascript">
    function CustomsPopup() {

        window.open('frmCustomsPopup.aspx?Mode=FRI&AWBNo=589-81515324&FlightNo=9W1181&FlightDate=30/01/2014', 'openPopUp', 'left=250,top=200,width=890,height=450,toolbar=0,resizable=1,scrollbars=1');
    }

    function ViewEmailSplit() {
        document.getElementById('MessagePreview').style.display = 'block';
        document.getElementById('HideMessagePreviewPopup').style.display = 'block';
        ShowAccordianDetails();
    }
    function HideEmailSplit() {
        document.getElementById('MessagePreview').style.display = 'none';
        document.getElementById('HideMessagePreviewPopup').style.display = 'none';
        ShowAccordianDetails()
    }
    function ViewEmailMessageSplit() {
        document.getElementById('EmailPopup').style.display = 'block';
        document.getElementById('HideEmailPopup').style.display = 'block';
        ShowAccordianDetails();
    }
    function HideEmailMessageSplit() {
        document.getElementById('EmailPopup').style.display = 'none';
        document.getElementById('HideEmailPopup').style.display = 'none';
    }
    function HideAMSDetails() {
        document.getElementById('divAMSDetails').style.display = "none";
    }
    function ShowAMSDetails() {

        document.getElementById('divAMSDetails').style.display = "block";
        
    }
    function ShowAccordianDetails() {
        var FunctionName = document.getElementById('<%= hdFSNMsgType.ClientID %>').value;
        ShowAMSDetails();
        window[FunctionName]();



    }
    function ValidatingMessages() {
        ShowAccordianDetails();
        ExpandAll();
    }

    function ChangeDate(dt) {
        var str = dt.value;
        if (str.Length > 6) {
            str = str.replace("-", "");
            dt.value = str.substring(0, 6);
        }
        else {

        }
    }

    function dateSelectionChanged(sender, args) {
        selectedDate = sender.get_selectedDate();
        var dt = selectedDate;
        alert(dt);
        var targetid = sender.get_element();
        dt = dt.replace("-", "");
        dt = dt.substring(0, 6);
        alert(dt);
   
    /* this sets the date on both the calendar and textbox */
 }

    function FRI() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "block";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('Agent').style.display = "block";
        document.getElementById('tabShipper').style.display = "block";
        document.getElementById('Consignee').style.display = "block";
        document.getElementById('Transfer').style.display = "block";
        document.getElementById('Description').style.display = "block";
        document.getElementById('FDA').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FRI";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FRI";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FRI";
        PackageTrackAWB();
        ShowArrival();
        CollapseAll();

    }
    function FRC() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "block";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "block";
        document.getElementById('Consignee').style.display = "block";
        document.getElementById('Transfer').style.display = "block";
        document.getElementById('Description').style.display = "block";
        document.getElementById('FDA').style.display = "block";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "block";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FRC";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FRC";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FRC";
        PackageTrackAWB();
        ShowArrival();
        CollapseAll();

    }

    function FXI() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "block";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "block";
        document.getElementById('tabShipper').style.display = "block";
        document.getElementById('Consignee').style.display = "block";
        document.getElementById('Transfer').style.display = "block";
        document.getElementById('Description').style.display = "block";
        document.getElementById('FDA').style.display = "block";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FXI";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FXI";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FXI";

        PackageTrackAWB();
        ShowArrival();
        CollapseAll();

    }

    function FXC() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "block";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "block";
        document.getElementById('tabShipper').style.display = "block";
        document.getElementById('Consignee').style.display = "block";
        document.getElementById('Transfer').style.display = "block";
        document.getElementById('Description').style.display = "block";
        document.getElementById('FDA').style.display = "block";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "block";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FXC";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FXC";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FXC";
//        document.getElementById('divFRI').style.background = "Red";
//        document.getElementById('divFRC').style.background = "Red";
//        document.getElementById('divFXI').style.background = "Red";
//        document.getElementById('divFXC').style.background = "Red";
//        document.getElementById('divFRX').style.background = "Red";
//        document.getElementById('divFXX').style.background = "Red";
//        document.getElementById('divFRH').style.background = "Red";
//        document.getElementById('divFXH').style.background = "Red";
//        document.getElementById('divFSI').style.background = "Red";
//        document.getElementById('divFDM').style.background = "Red";
//        document.getElementById('divFSQ').style.background = "Red";
//        document.getElementById('divFER').style.background = "Red";
//        document.getElementById('divFSN').style.background = "Red";
//        document.getElementById('divFSC').style.background = "Red";
        PackageTrackAWB();
        ShowArrival();
        CollapseAll();

    }

    function FRX() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "block";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FRX";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FRX";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FRX";
//        document.getElementById('divFRI').style.background = "Red";
//        document.getElementById('divFRC').style.background = "Red";
//        document.getElementById('divFXI').style.background = "Red";
//        document.getElementById('divFXC').style.background = "Red";
//        document.getElementById('divFRX').style.background = "Red";
//        document.getElementById('divFXX').style.background = "Red";
//        document.getElementById('divFRH').style.background = "Red";
//        document.getElementById('divFXH').style.background = "Red";
//        document.getElementById('divFSI').style.background = "Red";
//        document.getElementById('divFDM').style.background = "Red";
//        document.getElementById('divFSQ').style.background = "Red";
//        document.getElementById('divFER').style.background = "Red";
//        document.getElementById('divFSN').style.background = "Red";
//        document.getElementById('divFSC').style.background = "Red";
        PackageTrackAWB();
        HideArrival();
        CollapseAll();

    }


    function FXX() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "block";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FXX";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FXX";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FXX";
//        document.getElementById('divFRI').style.background = "Red";
//        document.getElementById('divFRC').style.background = "Red";
//        document.getElementById('divFXI').style.background = "Red";
//        document.getElementById('divFXC').style.background = "Red";
//        document.getElementById('divFRX').style.background = "Red";
//        document.getElementById('divFXX').style.background = "Red";
//        document.getElementById('divFRH').style.background = "Red";
//        document.getElementById('divFXH').style.background = "Red";
//        document.getElementById('divFSI').style.background = "Red";
//        document.getElementById('divFDM').style.background = "Red";
//        document.getElementById('divFSQ').style.background = "Red";
//        document.getElementById('divFER').style.background = "Red";
//        document.getElementById('divFSN').style.background = "Red";
//        document.getElementById('divFSC').style.background = "Red";
        PackageTrackAWB();
        HideArrival();
        CollapseAll();

    }


    function FRH() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "block";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FRH";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FRH";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FRH";
//        document.getElementById('divFRI').style.background = "Red";
//        document.getElementById('divFRC').style.background = "Red";
//        document.getElementById('divFXI').style.background = "Red";
//        document.getElementById('divFXC').style.background = "Red";
//        document.getElementById('divFRX').style.background = "Red";
//        document.getElementById('divFXX').style.background = "Red";
//        document.getElementById('divFRH').style.background = "Red";
//        document.getElementById('divFXH').style.background = "Red";
//        document.getElementById('divFSI').style.background = "Red";
//        document.getElementById('divFDM').style.background = "Red";
//        document.getElementById('divFSQ').style.background = "Red";
//        document.getElementById('divFER').style.background = "Red";
//        document.getElementById('divFSN').style.background = "Red";
//        document.getElementById('divFSC').style.background = "Red";
        PackageTrackAWB();
        ShowArrival();
        CollapseAll();

    }


    function FXH() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "block";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FXH";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FXH";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FXH";
        
//        document.getElementById('divFRI').style.background = "Red";
//        document.getElementById('divFRC').style.background = "Red";
//        document.getElementById('divFXI').style.background = "Red";
//        document.getElementById('divFXC').style.background = "Red";
//        document.getElementById('divFRX').style.background = "Red";
//        document.getElementById('divFXX').style.background = "Red";
//        document.getElementById('divFRH').style.background = "Red";
//        document.getElementById('divFXH').style.background = "Red";
//        document.getElementById('divFSI').style.background = "Red";
//        document.getElementById('divFDM').style.background = "Red";
//        document.getElementById('divFSQ').style.background = "Red";
//        document.getElementById('divFER').style.background = "Red";
//        document.getElementById('divFSN').style.background = "Red";
//        document.getElementById('divFSC').style.background = "Red";
        PackageTrackAWB();
        ShowArrival();
        CollapseAll();

    }

    function FSI() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "block";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FSI";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FSI";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FSI";
//        document.getElementById('divFRI').style.background = "Red";
//        document.getElementById('divFRC').style.background = "Red";
//        document.getElementById('divFXI').style.background = "Red";
//        document.getElementById('divFXC').style.background = "Red";
//        document.getElementById('divFRX').style.background = "Red";
//        document.getElementById('divFXX').style.background = "Red";
//        document.getElementById('divFRH').style.background = "Red";
//        document.getElementById('divFXH').style.background = "Red";
//        document.getElementById('divFSI').style.background = "Red";
//        document.getElementById('divFDM').style.background = "Red";
//        document.getElementById('divFSQ').style.background = "Red";
//        document.getElementById('divFER').style.background = "Red";
//        document.getElementById('divFSN').style.background = "Red";
//        document.getElementById('divFSC').style.background = "Red";
        PackageTrackAWB();
        HideArrival();
        CollapseAll();

    }


    function FDM() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "none";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "none";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "block";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FDM";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FDM";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FDM";
        PackageTrackAWB();
        ShowArrival();
        CollapseAll();

    }


    function FSQ() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "none";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "block";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FSQ";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FSQ";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FSQ";
        ShowArrival();
        PartArrivalAWB();
        CollapseAll();

    }

    function FER() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "none";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "block";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FER";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FER";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FER";
        PackageTrackAWB();
        ShowArrival();
        CollapseAll();

    }
    function FSN() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "block";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FSN";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FSN";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FSN";
        PackageTrackAWB();
        HideArrival();
        CollapseAll();

    }

    function FSC() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "block";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "block";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "block";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "block";
        document.getElementById('StatusCondition').style.display = "block";
        document.getElementById('Text').style.display = "block";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FSC";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FSC";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FSC";
        PartArrivalAWB();
        HideArrival();
        CollapseAll();

    }

    function PartArrivalAWB() {
        document.getElementById('<%= txtPartArrival.ClientID %>').style.display = "block";
        document.getElementById('<%= lblPartArrival.ClientID %>').style.display = "block";
        document.getElementById('<%= lblpackagetrackid.ClientID %>').style.display = "none";
        document.getElementById('<%= txtpackagetrackid.ClientID %>').style.display = "none";

    }
    function PackageTrackAWB() {
        document.getElementById('<%= txtPartArrival.ClientID %>').style.display = "none";
        document.getElementById('<%= lblPartArrival.ClientID %>').style.display = "none";
        document.getElementById('<%= lblpackagetrackid.ClientID %>').style.display = "block";
        document.getElementById('<%= txtpackagetrackid.ClientID %>').style.display = "block";
    }

    function FSNInbox() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "block";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "FSN";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "FSN";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "FSNInbox";
        //        document.getElementById('divFRI').style.background = "Red";
        //        document.getElementById('divFRC').style.background = "Red";
        //        document.getElementById('divFXI').style.background = "Red";
        //        document.getElementById('divFXC').style.background = "Red";
        //        document.getElementById('divFRX').style.background = "Red";
        //        document.getElementById('divFXX').style.background = "Red";
        //        document.getElementById('divFRH').style.background = "Red";
        //        document.getElementById('divFXH').style.background = "Red";
        //        document.getElementById('divFSI').style.background = "Red";
        //        document.getElementById('divFDM').style.background = "Red";
        //        document.getElementById('divFSQ').style.background = "Red";
        //        document.getElementById('divFER').style.background = "Red";
        //        document.getElementById('divFSN').style.background = "Red";
        //        document.getElementById('divFSC').style.background = "Red";
        PackageTrackAWB();
        HideArrival();
        CollapseAll();

    }

    // AMS Region
    //BEGIN
    function PRI() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "block";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('Agent').style.display = "block";
        document.getElementById('tabShipper').style.display = "block";
        document.getElementById('Consignee').style.display = "block";
        document.getElementById('Transfer').style.display = "block";
        document.getElementById('Description').style.display = "block";
        document.getElementById('FDA').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "PRI";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "PRI";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "PRI";
        PackageTrackAWB();
        ShowArrival();
        CollapseAll();

    }


    function PSN() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "block";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "none";
        document.getElementById('StatusCarrier').style.display = "block";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "PSN";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "PSN";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "PSN";
        PackageTrackAWB();
        HideArrival();
        CollapseAll();

    }

    function PER() {
        document.getElementById('AccordianTab').style.display = "block";
        document.getElementById('AWB').style.display = "block";
        document.getElementById('WBL').style.display = "none";
        document.getElementById('Arrival').style.display = "none";
        document.getElementById('CED').style.display = "none";
        document.getElementById('tabShipper').style.display = "none";
        document.getElementById('Consignee').style.display = "none";
        document.getElementById('Transfer').style.display = "none";
        document.getElementById('Description').style.display = "none";
        document.getElementById('FDA').style.display = "none";
        document.getElementById('Agent').style.display = "none";
        document.getElementById('Reason').style.display = "none";
        document.getElementById('HLD').style.display = "none";
        document.getElementById('StatusCBP').style.display = "none";
        document.getElementById('DepartureDetails').style.display = "none";
        document.getElementById('StatusQuery').style.display = "none";
        document.getElementById('ErrorMessage').style.display = "block";
        document.getElementById('StatusCarrier').style.display = "none";
        document.getElementById('StatusCondition').style.display = "none";
        document.getElementById('Text').style.display = "none";
        document.getElementById('<%= txtMsgType.ClientID %>').value = "PER";
        document.getElementById('<%= hdMessageType.ClientID %>').value = "PER";
        document.getElementById('<%= hdFSNMsgType.ClientID %>').value = "PER";
        PackageTrackAWB();
        ShowArrival();
        CollapseAll();

    }
    //END
    
    function HideArrival() {
        document.getElementById('HideArrival').style.display = "none";
    }
    function ShowArrival() {
        document.getElementById('HideArrival').style.display = "";
    }
    function setErrorMessage(ddlError) {
        var txtMessage = document.getElementById('<%= txtErrorMessage.ClientID %>');
        txtMessage.value = ddlError.options[ddlError.selectedIndex].value;

    }
    function TimeValidation(txt) {
        var re = /^([0-9]|0[0-9]|1[0-9]|2[0-3])[0-5][0-9]$/;
        if (re.test(txt.value)) {
        }
        else {
            window.alert("Invalid Entry! Please Enter the value in HHMM format..");
            txt.focus();
            txt.value = "";
        }
    }
</script>

<script type="text/javascript">

    function RadioCheck(rb) {
        var gv = document.getElementById("<%=GRDBooking.ClientID%>");
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
</script>

<style>
    
     	 .HoverFunction 
  	     {   
  	     	/*  border-top: 4px solid #A9CDE4; background-color: #fff; */ 
  	     	color:#000000; 
  	     	padding-top: 6px; 
  	     	padding-bottom:6px; 
  	     	text-align:center;   
  	     	/* fallback */   
  	     	background-color: #313131;   
  	     	background: url(images/linear_bg_2.png);   
  	     	background-repeat: repeat-x;    
  	     	/* Safari 4-5, Chrome 1-9 */   
  	     	background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#313131), to(#cacaca));    
  	     	/* Safari 5.1, Chrome 10+ */   
  	     	background: -webkit-linear-gradient(top,#cacaca, #313131);    
  	     	/* Firefox 3.6+ */   
  	     	background: -moz-linear-gradient(top, #cacaca, #313131);    
  	     	/* IE 10 */   
  	     	background: -ms-linear-gradient(top, #cacaca, #313131);    
  	     	/* Opera 11.10+ */   
  	     	background: -o-linear-gradient(top, #cacaca, #313131);        
  	     }

  .FRI
  {
   /*  border-top: 4px solid #A9CDE4;
background-color: #fff;
*/
color:#ffffff;
padding-top: 6px;
padding-bottom:6px;
text-align:center;

 /* fallback */
  background-color: #1a82f7;
  background: url(images/linear_bg_2.png);
  background-repeat: repeat-x;

  /* Safari 4-5, Chrome 1-9 */
  background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#8f9091), to(#cacaca));

  /* Safari 5.1, Chrome 10+ */
  background: -webkit-linear-gradient(top,#cacaca, #8f9091);

  /* Firefox 3.6+ */
  background: -moz-linear-gradient(top, #cacaca, #8f9091);

  /* IE 10 */
  background: -ms-linear-gradient(top, #cacaca, #8f9091);

  /* Opera 11.10+ */
  background: -o-linear-gradient(top, #cacaca, #8f9091);
  

 
}
        .black_overlay_Preview
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
        .white_content_Preview
        {
            margin: 0 auto;
            display: none;
            position: absolute;
            top: 30%;
            left: 35%;
            width: 30%;
            height: 55%;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }

		
        </style>
    
    <div id="contentarea">
     <h1>Customs
      <br /> <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Medium"
             ForeColor="Red"></asp:Label>
             </h1>
     <div class="botline">
     <table><tr><td><asp:Label ID="Label9" runat="server" Height="16px" Text="Flight #"></asp:Label> </td><td>
            <%--<asp:DropDownList ID="ddlFightDesignator" runat="server">
            </asp:DropDownList>--%>
            <asp:TextBox ID="ddlFightDesignator" runat="server" Width="40px">
         </asp:TextBox>
                    <asp:TextBox ID="txtFlightID" runat="server" Width="44px" ></asp:TextBox></td>
                    <td>
         <asp:TextBox ID="txtFlightDate" runat="server" Width="85px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txtFlightDate" Format="dd/MM/yyyy" PopupButtonID="imgDate">
                    </asp:CalendarExtender></td>
                    <td><asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" /></td>
                    <td>
         </td>
                    <td>
         </td>
                    <td>
                        AWB #</td>
                    <td><asp:TextBox ID="txtPrefix" runat="server" Width="43px"></asp:TextBox>
                    <asp:TextBox ID="txtAirWayBillNo" runat="server"></asp:TextBox></td>
                   <td>
         </td>
                   <td></td>
                    <td><asp:Label ID="Label2" runat="server" Height="16px" Text="ULD"></asp:Label></td>
                    <td><asp:TextBox runat="server" ID="txtULD"></asp:TextBox> </td>
         <td>
         </td>
         <td></td>
                    <td><asp:Label ID="Label3" runat="server" Height="16px" Text="Dep.Airport"></asp:Label></td><td>
            <asp:DropDownList ID="ddlDeptAirport" runat="server">
            </asp:DropDownList>
                    </td><td>
                    <asp:Button ID="BtnList" runat="server" Text="List" CssClass="button" 
                        onclick="BtnList_Click" />
                    <asp:Button ID="BtnClear" runat="server" Text="Clear"  
                      CssClass="button" onclick="BtnClear_Click"  /></td>
                    
                    </tr></table>
     
     </div>
     <div class="ltfloat">
     <asp:GridView ID="GRDBooking" runat="server" AllowPaging="true" 
                            AutoGenerateColumns="False" Height="82px" 
                            OnPageIndexChanging="GRDBooking_PageIndexChanging" 
                            PageSize="10" ShowFooter="True" 
                            Width="100%" HeaderStyle-CssClass="HeaderStyle"  CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle" PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle" >
                            <Columns>
                            <asp:TemplateField Visible="false">
                            <ItemTemplate>
                            <asp:Label ID="AWBNo" runat="server" Text='<%# Eval("AWBNo") %>' />
                            <asp:Label ID="AWBPrefix" runat="server" Text='<%# Eval("AWBPrefix") %>' />
                             <asp:Label ID="FlightNo" runat="server" Text='<%# Eval("FlightNo") %>' />
                            <asp:Label ID="FlightDate" runat="server" Text='<%# Eval("FlightDate") %>' />
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                            <ItemTemplate>
                            <asp:RadioButton ID="rdbCheck" runat="server" onclick="javascript:RadioCheck(this)"/>
                            </ItemTemplate>
                            </asp:TemplateField>
                                <asp:BoundField DataField="AWBNumber" HeaderText="AWB Number">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OriginCode" HeaderText="Org">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DestinationCode" HeaderText="Dest">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AgentCode" HeaderText="Agent Code">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AWBDate" HeaderText="AWB Date">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PiecesCount" HeaderText="Pieces Count" ItemStyle-HorizontalAlign="Right">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GrossWeight" HeaderText="Gross Weight" ItemStyle-HorizontalAlign="Right" dataformatstring="{0:00.00}">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ChargedWeight" HeaderText="Charged Weight" ItemStyle-HorizontalAlign="Right" dataformatstring="{0:00.00}">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ProductType" HeaderText="Product Type">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ExecutedBy" HeaderText="ExecutedBy">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AWBStatus" HeaderText="AWBStatus">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                 <%--  <asp:BoundField DataField="FlightNo" HeaderText="FlightNo" Visible="false">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="FlightDate" HeaderText="FlightDate" Visible="false">
                                    <ItemStyle Width="100px" />
                                    </asp:BoundField>--%>
                                    
                            </Columns>
                           <%-- <HeaderStyle CssClass="titlecolr" />--%>
                            <RowStyle HorizontalAlign="Center" />
                            <AlternatingRowStyle HorizontalAlign="Center" />
                        </asp:GridView>
      <asp:Button ID="btnFetch" runat="server" CssClass="button" Text="Fetch AMS Data" 
             onclick="btnFetch_Click" Visible="false" />
             <asp:Button ID="btnFetchACAS" runat="server" CssClass="button" Text="Fetch ACAS Data" 
              Visible="false" onclick="btnFetchACAS_Click" />
      <asp:Button ID="btnAudit" runat="server" CssClass="button" Text="Customs Audit" 
             onclick="btnAudit_Click" Visible="false" />
             <br />
             <br />
             <asp:GridView ID="grdMessages" runat="server" AllowPaging="true" 
                            AutoGenerateColumns="False" Height="82px" 
                            PageSize="15" ShowFooter="True" 
                            Width="100%" HeaderStyle-CssClass="HeaderStyle"  
             CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" 
             AlternatingRowStyle-CssClass="AltRowStyle" PagerStyle-CssClass="PagerStyle" 
             SelectedRowStyle-CssClass="SelectedRowStyle" 
             onrowcommand="grdMessages_RowCommand" >
                            <Columns>
                            
                            <asp:TemplateField HeaderText="AWBNumber">
                            <ItemTemplate>
                            <asp:HyperLink  ID="lnkAWBNumber" runat="server" Text='<%# Eval("AWBNo") %>' NavigateUrl='<%# "GHA_ConBooking.aspx?command=Edit&AWBNumber=" + Eval("AWBNo")%>' Visible="false"></asp:HyperLink>
                             <asp:Label  ID="txtAWBNum" runat="server" Text='<%# Eval("AWBNo") %>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Message Type">
                            <ItemTemplate>
                            <asp:LinkButton ID="lnkMessageType" runat="server" Text='<%# Eval("MessageType") %>' CommandArgument='<%# Eval("SerialNumber") %>' CommandName="MessageType" ></asp:LinkButton>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                            <ItemTemplate>
                            <asp:Label ID="lblMessageNature" runat="server" Text='<%# Eval("MessageNature") %>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sent/Recieved On">
                            <ItemTemplate>
                            <asp:Label ID="lblUpdatedOn" runat="server" Text='<%# Eval("UpdatedOn") %>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sent/Recieved By">
                            <ItemTemplate>
                            <asp:Label ID="lblUpdatedBy" runat="server" Text='<%# Eval("UpdatedBy") %>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                            
                            <asp:TemplateField Visible="false">
                            <ItemTemplate>
                            </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                           <%-- <HeaderStyle CssClass="titlecolr" />--%>
                            <RowStyle HorizontalAlign="Center" />
                            <AlternatingRowStyle HorizontalAlign="Center" />
                        </asp:GridView>
         <br />
        
     </div>
     
     <div id="divAMSDetails" class="ltfloat" style="display:none;">
     <table>
     <tr>
     <td><asp:Label ID="lblMessageType" runat="server" Height="16px" Text="Message Type"></asp:Label></td>
     <td><asp:TextBox runat="server" ID="txtMsgType"></asp:TextBox></td><td></td>
      <td><asp:Label ID="lblControlLocation" runat="server" Height="16px" Text="Control Location"></asp:Label></td>
      <td><asp:TextBox runat="server" ID="txtContLocation"></asp:TextBox></td>
     </tr></table>
     
     <table width="100%" style="margin-top:20px;">
     <tr><td style="width:175px;" valign="top">
     <div id="otherbox">
     <ul>
        <li id="divFRI"><a href="#" onclick="FRI();">Freight Inbound</a></li>
        <li id="divFRC"><a href="#" onclick="FRC();">Freight Change</a></li>
        <li id="divFRX"><a href="#" onclick="FRX();">Freight Cancel</a></li>
        <li id="divFXI"><a href="#" onclick="FXI();">Express inbound </a></li>
        <li id="divFXC"><a href="#" onclick="FXC();">Express Change</a></li>
        <li id="divFXX"><a href="#" onclick="FXX();">Express Cancel</a></li>
        <li id="divFDM"><a href="#" onclick="FDM();">Flt Departure Msg</a></li>
        <li id="divFSQ"><a href="#" onclick="FSQ();">Status Query</a></li>
        <li id="divFSN"><a href="#" onclick="FSN();">Status Notification</a></li>
        <li id="divFER" style="margin-bottom:10px;"><a href="#" onclick="FER();">Error Report</a></li>
       
        <li id="divFRH"><a href="#" onclick="FRH();">Hold/Release/Deny</a></li>
        <li id="divFXH"><a href="#" onclick="FXH();">Exp Hld/Relse/Dny</a></li>
        <li id="divFSNIn"><a href="#" onclick="FSNInbox();">Status Notification (CBP)</a></li>
        <li id="divFSI"><a href="#" onclick="FSI();">Status Information</a></li>
        <li id="divFSC"><a href="#" onclick="FSC();">Status Condition</a></li>
        
        
        
        
        
    </ul>
    </div>
     

 </td><td valign="top">
 <div class="boxshadow" id="AccordianTab" style="display:none;">
     <dl  class="accordion">
        <dt id="AWB" style="display:none;">
        <a href="#">AWB</a> 
         <asp:Label ID="lblAWBStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="Lblawbprefix" runat="server" Height="16px" Text="AWB Prefix *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="30"  ID="txtawbprefix" Enabled="false"></asp:TextBox>
          </td>
        <td><asp:Label ID="Lblawbserialnumber" runat="server" Height="16px" Text="AWB Serial Number *" Enabled="false"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtawbserialnumber" MaxLength="8" Enabled="false"></asp:TextBox>
        <asp:FilteredTextBoxExtender runat="server" FilterType="Numbers" ID="txtawbNumberextender" TargetControlID="txtawbserialnumber"></asp:FilteredTextBoxExtender>
        </td>
        
        <td><asp:Label ID="Lblconsolidateid" runat="server" Height="16px" Text="Consolidation Identifier"></asp:Label></td><td>
        <asp:TextBox runat="server" Width="60" ID="txtconsolidateid" MaxLength="1"></asp:TextBox></td>
        </tr>
        <tr>
        <td><asp:Label ID="lblHAWB" runat="server" Height="16px" Text="HAWB Number"></asp:Label></td><td>
            <asp:DropDownList ID="ddlHAWB" runat="server">
            </asp:DropDownList>
            </td>
        <td><asp:Label ID="lblpackagetrackid" runat="server" Height="16px" style="display:none;" Text="Package Tracking Identifier"></asp:Label></td><td colspan="2">
        <asp:TextBox runat="server" Width="135" MaxLength="35" ID="txtpackagetrackid" style="display:none;"></asp:TextBox></td>
       <td><asp:Label ID="lblPartArrival" style="display:none;" runat="server" Height="16px" Text="Part Arrival Reference"></asp:Label></td>
       <td>
           <asp:TextBox ID="txtPartArrival" style="display:none;" MaxLength="1" runat="server"></asp:TextBox></td>
        </tr>
        </table>
        </dd>
        
         <dt id="WBL" style="display:none;">
        <a href="">WBL</a> 
         <asp:Label ID="lblWBLStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="lblOrigin" runat="server" Height="16px" Text="Origin *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="30" ID="TxtOrigin" MaxLength="3"></asp:TextBox></td>
        <td><asp:Label ID="lblDestination" runat="server" Height="16px" Text="Destination"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtDestination" MaxLength="3"></asp:TextBox></td>
        <td><asp:Label ID="lblShipmentDescriptionCode" runat="server" Height="16px" Text="Shipment Description Code *" ></asp:Label></td><td>
        <asp:TextBox runat="server" Width="60" ID="txtShipmentDescriptionCode" MaxLength="1" Text="T" Enabled="false"></asp:TextBox>
        </td>
        <td><asp:Label ID="lblPieces" runat="server" Height="16px" Text="Pieces *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtPieces" MaxLength="5"></asp:TextBox>
        </td>
        
        </tr>
        <tr>
        <td><asp:Label ID="lblWeightCodeWBL" runat="server" Height="16px" Text="Weight Code *"></asp:Label></td>
        <td>
          <asp:DropDownList ID="ddlWBLWeightCode" runat="server">
            <asp:ListItem Text="Kilo" Value="K"></asp:ListItem>
            <asp:ListItem Text="Pound" Value="L"></asp:ListItem>
            </asp:DropDownList>
        <asp:TextBox runat="server" Width="30" MaxLength="1" ID="txtWeightCode" Visible="false"></asp:TextBox>
        </td>
        <td><asp:Label ID="lblWeightWBL" runat="server" Height="16px" Text="Weight *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtWeight" MaxLength="7"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$"  ControlToValidate="txtWeight" runat="server" ErrorMessage="Invalid Format!(EX:00.00)"></asp:RegularExpressionValidator>
        </td>
        <td><asp:Label ID="lblDescription" runat="server" Height="16px" Text="Cargo Description *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtCargoDescription" MaxLength="35"></asp:TextBox>
        </td>
        <td><asp:Label ID="lblDateofArrival" runat="server" Height="16px" Text="Date of Arrival"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtDateofArrival" ></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                        Enabled="True" TargetControlID="txtDateofArrival"  Format="dd-MMM-yyyy">
                    </asp:CalendarExtender></td>
        
        </tr>
        </table>
        </dd>
        
        <dt id="Arrival" style="display:none;">
        <a href="">Arrival</a> 
         <asp:Label ID="lblArrivalStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="3">
        <tr>
        <td><asp:Label ID="lblImportingCarrier" runat="server" Height="16px" Text="Importing Carrier*"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="30" ID="txtImportingCarrier" MaxLength="3"></asp:TextBox>
            </td>
        <td><asp:Label ID="lblFlight" runat="server" Height="16px" Text="Flight *"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtFlightNumber" MaxLength="5"></asp:TextBox>
            </td>
        <td><asp:Label ID="lblScheduledArrivalDate" runat="server" Height="16px" Text="Scheduled Date*"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtScheduledArrDate" ></asp:TextBox>
               <asp:CalendarExtender ID="CalendarExtender5" runat="server" 
                        Enabled="True" TargetControlID="txtScheduledArrDate" Format="dd-MMM-yyyy">
                    </asp:CalendarExtender></td>
            
        <td><asp:Label ID="lblPartialReference" runat="server" Height="16px" Text="Part Ref."></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtPartArrivalReference" MaxLength="1"></asp:TextBox></td>
        
       </tr>
        
        <tr id="HideArrival" style="display:none;">
        
        <td><asp:Label ID="lblBoardedQuantity" runat="server" Height="16px" Text="Boarded Qty Identifier"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="30" ID="txtBoardedQtyIdentifier" MaxLength="1"></asp:TextBox></td>
        <td><asp:Label ID="lblBoardedPiece" runat="server" Height="16px" Text="Boarded Pcs Count"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtBoardedPieceCount" MaxLength="5"></asp:TextBox>
            <asp:FilteredTextBoxExtender runat="server" FilterType="Numbers" ID="FilteredTextBoxExtender1" TargetControlID="txtBoardedPieceCount"></asp:FilteredTextBoxExtender>
            </td>
        <td><asp:Label ID="lblWeightCodeArrival" runat="server" Height="16px" Text="Weight Code"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtArrWtCode" MaxLength="1" Visible="false"></asp:TextBox>
            <asp:DropDownList ID="ddlArrWeightCode" runat="server">
            <asp:ListItem Text="Kilo" Value="K"></asp:ListItem>
            <asp:ListItem Text="Pound" Value="L"></asp:ListItem>
            </asp:DropDownList></td>
        <td><asp:Label ID="lblWeightArrival" runat="server" Height="16px" Text="Weight"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtArrWeight" MaxLength="7"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$"  ControlToValidate="txtArrWeight" runat="server" ErrorMessage="Invalid Format!(EX:00.00)"></asp:RegularExpressionValidator>

            </td>
        
        </tr>
        </table>
        </dd>
        
        <dt id="Agent" style="display:none;">
        <a href="">Agent</a> 
         <asp:Label ID="lblAgentStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="50%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="Label22" runat="server" Height="16px" Text="Air AMS Participant Code *"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="30" ID="txtAgent" MaxLength="7"></asp:TextBox>
            </td>
        
        
       </tr>
        </table>
        </dd>
        
           <dt id="CED" style="display:none;">
        <a href="">CED</a> 
         <asp:Label ID="lblCEDStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="lbEntryType" runat="server" Height="16px" Text="Entry Type *"></asp:Label></td><td>
            <asp:DropDownList ID="ddlEntryType" runat="server">
            </asp:DropDownList></td>
        <td><asp:Label ID="lblEntryNo" runat="server" Height="16px" Text="Entry Number" ></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="TxtEntryNo" MaxLength="11"></asp:TextBox></td>
        </tr>
         </table>
        </dd>
        
        <dt id="tabShipper" style="display:none;">
        <a href="">Shipper</a> 
         <asp:Label ID="lblShipperStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd id="Shipper" style="display:none;">
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="Label23" runat="server" Height="16px" Text="Name of Shipper *"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="30" ID="txtShipperName" MaxLength="35"></asp:TextBox></td>
        <td><asp:Label ID="Label24" runat="server" Height="16px" Text="Street Address"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtShipperAddress" MaxLength="35"></asp:TextBox></td>
        <td><asp:Label ID="Label25" runat="server" Height="16px" Text="City, County, Township *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtCity" MaxLength="17"></asp:TextBox></td>
        </tr>
        <tr>
        <td><asp:Label ID="Label26" runat="server" Height="16px" Text="State / Province"></asp:Label></td>
        <td><asp:TextBox runat="server" ID="txtState" Width="60" MaxLength="9"></asp:TextBox></td>
        <td><asp:Label ID="Label27" runat="server" Height="16px" Text="Country Code *"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlCountryCode" runat="server">
            </asp:DropDownList>
        </td>
       <td><asp:Label ID="Label28" runat="server" Height="16px" Text="Postal Code"></asp:Label></td>
       <td><asp:TextBox runat="server" Width="135" ID="txtPostalCode" MaxLength="9"></asp:TextBox></td>
        </tr>
        </table>
        </dd>
        
        <dt id="Consignee" style="display:none;">
        <a href="">Consignee</a> 
         <asp:Label ID="lblConsigneeStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="Label29" runat="server" Height="16px" Text="Name*"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="30" ID="txtConsigneeName" MaxLength="35"></asp:TextBox></td>
        <td><asp:Label ID="Label30" runat="server" Height="16px" Text="Address"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtConStreetAddress" MaxLength="35"></asp:TextBox></td>
        <td><asp:Label ID="Label31" runat="server" Height="16px" Text="City*"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtConCity" MaxLength="17"></asp:TextBox></td>
        <td><asp:Label ID="Label32" runat="server" Height="16px" Text="State"></asp:Label></td><td>
            <asp:TextBox runat="server" ID="txtConState" Width="60" MaxLength="9"></asp:TextBox></td>
        
        </tr>
        <tr>
        <td><asp:Label ID="Label33" runat="server" Height="16px" Text="Country Code *"></asp:Label></td><td>
            <asp:DropDownList ID="ddlConCountryCode" runat="server">
            </asp:DropDownList> </td>
       <td><asp:Label ID="Label34" runat="server" Height="16px" Text="Postal Code"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="135" ID="txtConPostalCode" MaxLength="9"></asp:TextBox></td>
        <td><asp:Label ID="Label35" runat="server" Height="16px" Text="Telephone Number"></asp:Label></td><td>
            <asp:TextBox runat="server" ID="txtTelephoneNo" Width="60" MaxLength="14"></asp:TextBox></td>
        
        </tr>
        </table>
        </dd>
        
        <dt id="Transfer" style="display:none;">
        <a href="">Transfer</a> 
         <asp:Label ID="lblTransferStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="Label42" runat="server" Height="16px" Text="Destination Airport *"></asp:Label></td><td colspan="2">
            <asp:TextBox runat="server" Width="30" ID="txtTransferDestArpt" MaxLength="3"></asp:TextBox></td>
        <td colspan="2"><asp:Label ID="Label43" runat="server" Height="16px" Text="Dom / International"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtDomInt" MaxLength="1"></asp:TextBox></td>
        
        </tr>
        <tr>
        <td><asp:Label ID="Label48" runat="server" Height="16px" Text="Bonded Carrier ID"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtBondedCarrierID" MaxLength="12"></asp:TextBox>OR</td>
        <td><asp:Label ID="Label45" runat="server" Height="16px" Text="Onward Carrier"></asp:Label></td><td>
            <asp:TextBox runat="server" ID="txtOnwardCarrier" MaxLength="3" Width="60"></asp:TextBox></td>
        <td><asp:Label ID="Label46" runat="server" Height="16px" Text="Bonded Premises Id"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="90" ID="txtBondedPremIdentifier" MaxLength="4"></asp:TextBox>OR</td>
       <td><asp:Label ID="Label47" runat="server" Height="16px" Text="In-Bond Control No"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="90" ID="txtInbondControlNo" MaxLength="9"></asp:TextBox>
            <asp:FilteredTextBoxExtender runat="server" FilterType="Numbers" ID="FilteredTextBoxExtender2" TargetControlID="txtInbondControlNo"></asp:FilteredTextBoxExtender>
            </td>
        </tr>
        </table>
        </dd>
        
        <dt id="Description" style="display:none;">
        <a href="">Description</a> 
         <asp:Label ID="lblDescriptionStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="Label37" runat="server" Height="16px" Text="Origin of Goods"></asp:Label></td><td>
            <asp:DropDownList ID="ddlDescriptionCountryCode" runat="server">
            </asp:DropDownList>
</td>
        <td><asp:Label ID="Label38" runat="server" Height="16px" Text="Declared Value *"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="60" ID="txtDeclaredValue" MaxLength="12"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$"  ControlToValidate="txtDeclaredValue" runat="server" ErrorMessage="Invalid Format!(EX:00.00)"></asp:RegularExpressionValidator>

            
            </td>
        <td>&nbsp;</td><td>&nbsp;</td>
        </tr>
        <tr>
        <td><asp:Label ID="Label40" runat="server" Height="16px" Text="ISO Currency Code *"></asp:Label></td><td>
            <asp:DropDownList ID="ddlCurrencyCode" runat="server">
            </asp:DropDownList> 
            </td>
        <td><asp:Label ID="Label41" runat="server" Height="16px" Text="Harmonized Commodity Code"></asp:Label></td><td>
            <asp:TextBox runat="server" Width="135" ID="txtCommodityCode" MaxLength="10"></asp:TextBox>
           <asp:FilteredTextBoxExtender runat="server" FilterType="Numbers" ID="FilteredTextBoxExtender3" TargetControlID="txtCommodityCode"></asp:FilteredTextBoxExtender>

            </td>
       <td></td><td></td>
        </tr>
        </table>
        </dd>
        
        <dt id="FDA" style="display:none;">
        <a href="">FDA</a> 
         <asp:Label ID="lblFDAStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="50%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="lblLineIdentifier" runat="server" Height="16px" Text="Line Identifier *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="30" ID="txtLineIdentifier" MaxLength="3" Enabled="false" Text="FDA"></asp:TextBox></td>
       </tr>
        </table>
        </dd>
      
        
         <dt id="Reason" style="display:none;">
        <a href="">Reason</a> 
         <asp:Label ID="lblReasonStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="lblAmendmentCode" runat="server" Height="16px" Text="Amendment Code *"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlAmendmentCode" runat="server">
            </asp:DropDownList>
        </td>
        <td><asp:Label ID="lblExplanation" runat="server" Height="16px" Text="Explanation"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtExplanation" MaxLength="20"></asp:TextBox></td>
        </tr>
         </table>
        </dd>
        
          <dt id="HLD" style="display:none;">
        <a href="">HLD</a> 
         <asp:Label ID="lblHLDStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="LblTypeofRequest" runat="server" Height="16px" Text="Type of Request *"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlHoldType" runat="server">
            <asp:ListItem Text="Release" Value="1"></asp:ListItem>
            <asp:ListItem Text="Hold" Value="2"></asp:ListItem>
            <asp:ListItem Text="Denied Entry" Value="3"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td><asp:Label ID="lblRequestExplanation" runat="server" Height="16px" Text="Request Explanation"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtRequestExplanation" MaxLength="60"></asp:TextBox></td>
        </tr>
         </table>
        </dd>
        
         <dt id="StatusCBP" style="display:none;">
        <a href="">StatusfromCBP</a> 
         <asp:Label ID="lblStatusCBPStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="lblActionCode" runat="server" Height="16px" Text="Action Code *"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlActionCode" runat="server">
            </asp:DropDownList>
        </td>
        <td><asp:Label ID="lblNumberofPieces" runat="server" Height="16px" Text="NO of Pieces *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtNumberofPieces" MaxLength="5"></asp:TextBox></td>
       <asp:FilteredTextBoxExtender runat="server" FilterType="Numbers" ID="FilteredTextBoxExtender4" TargetControlID="txtNumberofPieces"></asp:FilteredTextBoxExtender>
</tr>
        <tr><td><asp:Label ID="lblTransactionDate" runat="server" Height="16px" Text="Transaction Date *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtTransactionDate" ></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                        Enabled="True" TargetControlID="txtTransactionDate" Format="dd-MMM-yyyy" >
                    </asp:CalendarExtender>
        </td>
        <td><asp:Label ID="lblTransactionTime" runat="server" Height="16px" Text="Transaction Time *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtTransactionTime" MaxLength="4" ToolTip="Enter Time(HHMM)" onchange="javascript:TimeValidation(this);"></asp:TextBox>
        
<%--       <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ValidationExpression="^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$"  ControlToValidate="txtTransactionTime" runat="server" ErrorMessage="Invalid Format!(EX:HH:MM)"></asp:RegularExpressionValidator>
--%>
        </td>
        
        </tr>
        <tr>
        <td>
           <asp:Label ID="lblEntryType" runat="server" Height="16px" Text="Entry Type"></asp:Label> 
        </td>
        <td><asp:DropDownList ID="ddlCBPEntryCodes" runat="server">
            </asp:DropDownList></td>
        <td><asp:Label ID="lblEntryNumber" runat="server" Height="16px" Text="Entry Number"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtEntryNumber" MaxLength="15"></asp:TextBox></td>
        </tr><td><asp:Label ID="lblRemarks" runat="server" Height="16px" Text="Remarks"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtremarks" MaxLength="20"></asp:TextBox></td>
        <td></td><td></td>
        
        </tr>
        </table>
        </dd>
        
        <dt id="DepartureDetails" style="display:none;">
        <a href="">DepartureDetails</a> 
         <asp:Label ID="lblDepartureStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="Label59" runat="server" Height="16px" Text="Importing Carrier *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="30" ID="txtDepDetailsImportingCarrier" MaxLength="3"></asp:TextBox></td>
        <td><asp:Label ID="Label60" runat="server" Height="16px" Text="Flight Number *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtDepDetailsFlightNumber" MaxLength="5"></asp:TextBox></td>
        <td><asp:Label ID="lblDateofScheduledArrival" runat="server" Height="16px" Text="DateofScheduledArrival *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtDateOfScheduledArrival" ></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtender3" runat="server" 
                        Enabled="True" TargetControlID="txtDateOfScheduledArrival" Format="dd-MMM-yyyy">
                    </asp:CalendarExtender>
        </td>
        <td><asp:Label ID="lblLiftoffDate" runat="server" Height="16px" Text="Liftoff Date"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtLiftoffDate" ></asp:TextBox>
         <asp:CalendarExtender ID="CalendarExtender4" runat="server" 
                        Enabled="True" TargetControlID="txtLiftoffDate" Format="dd-MMM-yyyy" >
                    </asp:CalendarExtender>
        </td>
        
        </tr>
        <tr>
        <td><asp:Label ID="lblLiftoffTime" runat="server" Height="16px" Text="Liftoff Time"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="30" ID="txtLiftoffTime" MaxLength="4" ToolTip="Enter Time(HHMM)"  onchange="javascript:TimeValidation(this);"></asp:TextBox>
        </td>
        <td><asp:Label ID="lblActualImportingCarrier" runat="server" Height="16px" Text="Actual Importing Carrier *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtActualImportingCarrier" MaxLength="3"></asp:TextBox></td>
        <td><asp:Label ID="lblActualFlightNumber" runat="server" Height="16px" Text="Actual Flight Number *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtActualFlightNumber" MaxLength="5"></asp:TextBox></td>
        <td></td><td></td>
        
        </tr>
        </table>
        </dd>
        
          <dt id="StatusQuery" style="display:none;">
        <a href="">StatusQuery</a> 
         <asp:Label ID="lblStatusQueryStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="lblStatusRequestCode" runat="server" Height="16px" Text="Status Request Code *"></asp:Label></td>
        <td><asp:DropDownList ID="ddlStatusRequestCode" runat="server">
            </asp:DropDownList></td>
        </tr>
         </table>
        </dd>
        
        <dt id="ErrorMessage" style="display:none;">
        <a href="">Error Message</a> 
         <asp:Label ID="lblErrorMessageStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
          <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="lblErrorCode" runat="server" Height="16px" Text="Error Code *"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlErrorCode" onchange="setErrorMessage(this);" runat="server">
            </asp:DropDownList>
            </td>
        <td><asp:Label ID="lblErrorMessage" runat="server" Height="16px" Text="Error Message *"></asp:Label></td>
        <td><asp:TextBox runat="server" MaxLength="40"  ID="txtErrorMessage"></asp:TextBox></td>
        </tr>
         </table>
         </dd>
        
        <dt id="StatusCarrier" style="display:none;">
        <a href="">Status from Carrier/Handler</a> 
         <asp:Label ID="lblStatusCarrierStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
         <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="lblStatusCode" runat="server" Height="16px" Text="Status Code *"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlStatusCode" runat="server">
            </asp:DropDownList>  
        </td>
        <td><asp:Label ID="lblActionExplanation" runat="server" Height="16px" Text="Action Explanation"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="60" ID="txtActionExplanation" MaxLength="20"></asp:TextBox></td>
        </tr>
         </table>
        </dd>
        <dt id="StatusCondition" style="display:none;">
        <a href="">Status Condition</a> 
         <asp:Label ID="lblStatusConditionStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
         <table width="100%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="lblStatusAnswerCode" runat="server" Height="16px" Text="Status Answer Code *"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlStatusAnswerCode" runat="server">
            </asp:DropDownList>
           </td>
        </tr>
         </table>
        </dd>
         <dt id="Text" style="display:none;">
        <a href="">Text</a> 
         <asp:Label ID="lblTextStatus" runat="server" ForeColor="Red"></asp:Label></dt>
        <dd>
        <table width="50%" cellpadding="3" cellspacing="6">
        <tr>
        <td><asp:Label ID="lblInformation" runat="server" Height="16px" Text="Information *"></asp:Label></td>
        <td><asp:TextBox runat="server" Width="30" ID="txtInformation" MaxLength="60"></asp:TextBox></td>
        </tr>
         </table>
        </dd>
    
     </dl>
                <asp:Button ID="btnUpdate" runat="server" Text="Update" 
         CssClass="button" onclick="btnUpdate_Click" />
                <asp:Button ID="btnPreviewMessage" runat="server" 
         Text="Preview Message" CssClass="button" onclick="btnPreviewMessage_Click" />
                <asp:Button ID="btnSendMessage" runat="server" Text="Send Message" 
         CssClass="button" onclick="btnSendMessage_Click" />
                <asp:Button ID="btnPrint" runat="server" Text="Print Message" 
         CssClass="button" onclick="btnPrint_Click" />
           <asp:Button ID="btnSendFDM" runat="server" Text="Send FDM" 
         CssClass="button" onclick="btnSendFDM_Click" Visible="false" />


        
        
     </div>
     
  </td></tr></table>

      <div id="MessagePreview" class="white_content_Preview">
        <table>
            <tr>
                <td>
                   <asp:TextBox ID="txtMessagePreview" runat="server" TextMode="MultiLine" 
                        Height="300px" Width="390px"  ></asp:TextBox>
                </td>
            </tr>
        </table>
        
        <table>
            <tr>
                <td>
                    <input type="button" id="btnCancel" class="button" value="Close" onclick="HideEmailSplit();" />
                </td>
            </tr>
        </table>
    </div>
    <div id="HideMessagePreviewPopup" class="black_overlay_Preview">
    </div>
    <div id="EmailPopup" class="white_content_Preview">
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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtEmailID" runat="server" ErrorMessage="Please Enter Email ID" ControlToValidate="txtEmailID" ValidationGroup="send"></asp:RequiredFieldValidator>
 <asp:RegularExpressionValidator ID="RegularExpression_txtEmailID" runat="server" ErrorMessage="Please Enter Email ID in correct format" ControlToValidate="txtEmailID" ValidationGroup="send" 
 ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"></asp:RegularExpressionValidator>
                    <%--<asp:RequiredFieldValidator "RequiredValidator_txtEmailID" runat="server" ControlToValidate="txtEmailID"></asp:RequiredFieldValidator>
                </td>--%>
                
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnOK" CssClass="button" runat="server" Text="OK" 
                        onclick="btnOK_Click" ValidationGroup="send"  />
                    <input type="button" id="Button1" class="button" value="Cancel" onclick="HideEmailMessageSplit();" />
                </td>
            </tr>
        </table>
    </div>
    <div id="HideEmailPopup" class="black_overlay_Preview">
    </div>
    
        <asp:HiddenField ID="hdMessageType" runat="server" />
        <asp:HiddenField ID="hdFSNMsgType" runat="server" />
        <asp:HiddenField ID="hdFlightNo" runat="server" />
        <asp:HiddenField ID="hdFlightDate" runat="server" />
        <asp:HiddenField ID="hdActualPcs" runat="server" />
        <asp:HiddenField ID="hdActualWt" runat="server" />
     </div>
    
    
    </asp:Content>