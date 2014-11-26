<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" smartNavigation="true"  CodeBehind="frmScreeningDashboard.aspx.cs" Inherits="ProjectSmartCargoManager.frmScreeningDashboerd" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
		.black_overlaymsg{
			display: none;
			position: fixed;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 100%;
			background-color: black;
			z-index:1001;
			-moz-opacity: 0.8;
			opacity:.80;
			filter: alpha(opacity=80);
		}
		.white_contentmsg {
			display: none;
			position: fixed;
			top: 25%;
			left: 25%;
			width: 40%;
			height: 40%;
			padding: 16px;
			background-color: white;
			z-index:1002;
			overflow: auto;
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
			height: 2000px;
			background-color: black;
			z-index:1001;
			-moz-opacity:0.8;
			opacity:0.8;
			filter:alpha(opacity=80);
		}
		.white_content 
		{
		    margin:0 auto;
			display: none;
			position: absolute;			
			left: 20%;
			width: 53%;
			padding: 16px;
			border: 16px solid #ccdce3;
			background-color: white;
			z-index:1002;
			overflow: auto;
			top:40%;
            
		}

</style>
<script language="javascript" type="text/javascript">

    function ScrException(lnk) {
        var row = lnk.parentNode.parentNode;
        var rowIndex = row.rowIndex - 1;
        var grid = document.getElementById('ctl00_ContentPlaceHolder1_GVPopUp');
    }

    function Confirm() {
        try {
            var calType = document.getElementById("<%= hdnType.ClientID %>").value;
            var clic;
            if (calType == "UnScreen") {
                clic = confirm("Are You Sure for Marking these tags Screened");
            }
            else if (calType == "Screen") {
                clic = confirm("Are You Sure for Updating these Screened Tags");
            }
            else if (calType == "Reject") {
                clic = confirm("Are You Sure for Rejecting these Tags");
            }
            if (clic == true) {
                return true;
            }
            else
                return false;
        }
        catch (err) {
            return false;
        }
    }

    function enaBtn() {
        document.getElementById('<%=btnPopOK.ClientID %>').style.display = 'inherit';
        document.getElementById('btnPopCancel').style.display = 'inherit';
    }
    function disBtn() {
        document.getElementById('<%=btnPopOK.ClientID %>').style.display = 'none';
        document.getElementById('btnPopCancel').style.display = 'none';
    }
    function SelectNoneCheckboxes() {
        var ch = confirm("Are you Sure to Ignore these AWB ?");

        if (ch == true) {
            StopTimer();
            var elm = document.getElementById("<%=chkWarAWB.ClientID %>");
            var checkBoxes = elm.getElementsByTagName("input");
            var ch = 0;
            for (i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].checked == false) {

                }
                else {
                    ch = 1;
                    break;
                }
            }
            if (ch == 0) {
                callclose();
                alert("Please Select AWB for Ignore");
                StartTimer();
                return false;
            }
            else {

                return true;
            }
        } else
            callclose();
        StartTimer();
        return false;
    }


    function ChkUnScreenAll(oCheckbox, type) {
        var grd;
        if (type == "U") {
            grd = document.getElementById('<%= gvUnScreening.ClientID %>');
        } else {
            grd = document.getElementById('<%= gdvULDDetails.ClientID %>');
        }
        if (grd.rows.length > 0) {
            for (i = 1; i < grd.rows.length; i++) {
                grd.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
            }
        }
    }
    function ChkAllSOrNot(oCheckbox, type) {
        var grd;
        if (type == "U") {
            grd = document.getElementById('<%= gvUnScreening.ClientID %>');
        } else {
            grd = document.getElementById('<%= gdvULDDetails.ClientID %>');
        }
        var status = "0";


        if (oCheckbox.checked == true || oCheckbox == 'true') {
            for (i = 1; i < grd.rows.length; i++) {
                if (grd.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked == true) {

                }
                else {
                    status = "1";
                    break;
                }
            }
        }
        else {
            status = "1";
        }
        if (status == "1") {
            grd.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked = false;
        }
        else {
            grd.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked = true;
        }
    }
    function applydivval() {
        document.getElementById('<%= scrollValue.ClientID %>').value = document.getElementById('<%= leftBar.ClientID %>').scrollTop;
        document.getElementById('<%= scrollValueScreen.ClientID %>').value = document.getElementById('<%= Pnlgrd.ClientID %>').scrollTop;
    }
    function putdivval() {
        document.getElementById('<%= leftBar.ClientID %>').scrollTop = document.getElementById('<%= scrollValue.ClientID %>').value;
        document.getElementById('<%= Pnlgrd.ClientID %>').scrollTop = document.getElementById('<%= scrollValueScreen.ClientID %>').value;
    }
    function calbye() {
        document.getElementById('imgShowmag').src = "Images/Bye.gif";
        document.getElementById('msglight').style.display = 'block';
        document.getElementById('msgfade').style.display = 'block';
        document.getElementById("<%= msgshow.ClientID %>").innerHTML = "";
    }
    function calpage() {
        window.open("rptScreeningHistory.aspx", '_blank');
        window.focus();
    }
    function StartTimer() {
        var timer = $get('<%= tmrRef.ClientID %>').control;
        timer._startTimer();
    }
    function StopTimer() {
        var timer = $get('<%= tmrRef.ClientID %>').control;
        timer._stopTimer();
        document.getElementById('msglight').style.display = 'block';
        document.getElementById('msgfade').style.display = 'block';
        document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";
    }

    function ViewPanelSplit() {
        callclose();
        document.getElementById('Lightsplit').style.display = 'block';
        document.getElementById('fadesplit').style.display = 'block';
    }
    function HidePanelSplit() {
        var timer = $get('<%= tmrRef.ClientID %>').control;
        timer._startTimer();
        document.getElementById('Lightsplit').style.display = 'none';
        document.getElementById('fadesplit').style.display = 'none';
    }


    function checkvalid() {
        if (document.getElementById("<%= ddlDuartion.ClientID %>").selectedIndex == 0 && document.getElementById("<%= txtSDest.ClientID %>").value.trim() == "" && document.getElementById("<%= txtAWBSno.ClientID %>").value.trim() == "" && document.getElementById("<%= TextBoxdate.ClientID %>").value.trim() == "" && document.getElementById("<%= txtFlightID.ClientID %>").value.trim() == "") {
            alert("Atleast One parameter is Require to enter");
            return false;
        }
        if (document.getElementById("<%= ddlFlPre.ClientID %>").selectedIndex == 0 && document.getElementById("<%= txtFlightID.ClientID %>").value.trim() != "") {
            alert("Select Flight Prefix");
            return false;
        }
        //            if (document.getElementById("<%= ddlFlPre.ClientID %>").selectedIndex != 0 && document.getElementById("<%= txtFlightID.ClientID %>").value.trim() == "") {
        //                alert("Enter Flight Number");
        //                return false;
        //            }
        if (document.getElementById("<%= ddlListPre.ClientID %>").selectedIndex == 0 && document.getElementById("<%= txtAWBSno.ClientID %>").value.trim() != "") {
            alert("Select AWB Prefix");
            return false;
        }
        //            if (document.getElementById("<%= ddlListPre.ClientID %>").selectedIndex != 0 && document.getElementById("<%= txtAWBSno.ClientID %>").value.trim() == "") {
        //                alert("Enter AWB Number");
        //                return false;
        //            }
        if (document.getElementById("<%= ddlDuartion.ClientID %>").selectedIndex != 0 && document.getElementById("<%= TextBoxdate.ClientID %>").value.trim() != "") {
            alert("Cannot Select two Time Zone at a time(Duration and Flight Date)");
            return false;
        }
        if (document.getElementById("<%= ddlDuartion.ClientID %>").selectedIndex == 0 && document.getElementById("<%= txtSDest.ClientID %>").value.trim() != "" && document.getElementById("<%= txtAWBSno.ClientID %>").value.trim() == "" && document.getElementById("<%= TextBoxdate.ClientID %>").value.trim() == "" && document.getElementById("<%= txtFlightID.ClientID %>").value.trim() != "") {
            alert("Please Provide One Time Zone Filter");
            return false;
        }
        if (document.getElementById("<%= ddlDuartion.ClientID %>").selectedIndex == 0 && document.getElementById("<%= txtSDest.ClientID %>").value.trim() != "" && document.getElementById("<%= txtAWBSno.ClientID %>").value.trim() == "" && document.getElementById("<%= TextBoxdate.ClientID %>").value.trim() == "" && document.getElementById("<%= txtFlightID.ClientID %>").value.trim() == "") {
            alert("Please Provide One Time Zone Filter");
            return false;
        }
        if (document.getElementById("<%= ddlDuartion.ClientID %>").selectedIndex == 0 && document.getElementById("<%= txtSDest.ClientID %>").value.trim() == "" && document.getElementById("<%= txtAWBSno.ClientID %>").value.trim() == "" && document.getElementById("<%= TextBoxdate.ClientID %>").value.trim() == "" && document.getElementById("<%= txtFlightID.ClientID %>").value.trim() != "") {
            alert("Please Provide One Time Zone Filter");
            return false;
        }
        document.getElementById('msglight').style.display = 'block';
        document.getElementById('msgfade').style.display = 'block';
        document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Screen and UnScreen List .......";
    }
    function callShow() {
        document.getElementById('msglight').style.display = 'block';
        document.getElementById('msgfade').style.display = 'block';
        document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

    }
    function callclose() {
        document.getElementById('msglight').style.display = 'none';
        document.getElementById('msgfade').style.display = 'none';
    }
    function ViewPanellSplit() {
        document.getElementById('LightDiv').style.display = 'block';
        document.getElementById('fadeDiv').style.display = 'block';
        
    }
    function HidePanellSplit() {
        document.getElementById('LightDiv').style.display = 'none';
       document.getElementById('fadeDiv').style.display = 'none';
   }
   function ValidatePcstoRet(remainingpcs) {
       alert('Cannot return pcs');
       return false;
   }
   function clickButton() {
       if (event.keyCode == 13) {
           var Button = document.getElementById('<%=BtnList.ClientID%>');
           Button.focus();
           Button.click();
           return false;
       }
   }
</script>

    </asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"  EnableViewState="true">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:Panel ID="pnlWhole" DefaultButton="BtnList" runat="server">
       <%--<input type="hidden" id="hdnScrollTop" runat="server" value="0"/>--%>
         <asp:Timer ID="tmrRef" runat="server" Enabled="False" ontick="tmrRef_Tick" 
        Interval="300000"></asp:Timer>
<%--<input id="scrollPos" runat="server" type="hidden" value="0" />--%>
                       
       <%-- <asp:UpdatePanel ID="upFull" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
 <input id="scrollValue" type="hidden" runat="server" value="0"/>
    <input id="scrollValueScreen" type="hidden" runat="server" value="0"/>
    <input id="hdnType" type="hidden" runat="server" value="0"/>
    
        <div id="contentarea">
                       <asp:Label ID="lblParaStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
                     
                  <h1><%--<img alt="" src="Images/txt_screeningdashbord.png" />--%>Cargo Screening</h1>
                      
                        <div class="botline">
                        <table>
                        <tr>
                           <td> Flight # </td>
                            <td>
                            <asp:TextBox ID="ddlFlPre" runat="server" MaxLength="4" Width="45px" 
                            ToolTip="Flight Prefix"></asp:TextBox>
                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" 
                                runat="server" TargetControlID="ddlFlPre" WatermarkText="Prefix">
                            </asp:TextBoxWatermarkExtender> 
                            <%--POU--%>
                            <asp:TextBox ID="txtFlightID" runat="server" Width="55px" MaxLength="6"
                             ToolTip="Flight #"></asp:TextBox>
                            <asp:TextBoxWatermarkExtender ID="txtFlightNo_TextBoxWatermarkExtender" 
                                runat="server" TargetControlID="txtFlightID" WatermarkText="Flight #">
                            </asp:TextBoxWatermarkExtender>
                            &nbsp;<asp:TextBox ID="TextBoxdate" runat="server" Width="85px" MaxLength="10"
                             ToolTip="Flight Date"></asp:TextBox>
                            <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                            <asp:CalendarExtender ID="TextBoxdate_CalendarExtender" runat="server" PopupButtonID="imgDate" 
                                Enabled="True" Format="dd/MM/yyyy" TargetControlID="TextBoxdate">
                            </asp:CalendarExtender>
                            </td>
                            
      <td>AWB # :</td>
      <td> <asp:TextBox ID="ddlListPre" runat="server" MaxLength="4" Width="45px" 
                    ToolTip="AWB Prefix"></asp:TextBox> 
                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" 
                                runat="server" TargetControlID="ddlListPre" WatermarkText="Prefix">
                            </asp:TextBoxWatermarkExtender> 
      <asp:TextBox ID="txtAWBSno" runat="server" Width="80px" MaxLength="10" 
                        ToolTip = "AWB Number"></asp:TextBox> 
                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" 
                                runat="server" TargetControlID="txtAWBSno" 
                                WatermarkText="AWB #">
                            </asp:TextBoxWatermarkExtender>
                            </td>
                           
            <td> Destination</td>            
             <td> <asp:TextBox ID="txtSDest" runat="server" Width="80px" MaxLength="7"></asp:TextBox> </td>
           <td>Duration</td> 
           <td>
                            <%--<input name="Save" type="button" value="Save" />--%>
                            <asp:DropDownList ID="ddlDuartion" runat="server">
                                <asp:ListItem Selected="True">HH:mm</asp:ListItem>
                                <asp:ListItem>00:30</asp:ListItem>
                                <asp:ListItem>01:00</asp:ListItem>
                                <asp:ListItem>01:30</asp:ListItem>
                                <asp:ListItem>02:00</asp:ListItem>
                                <asp:ListItem>02:30</asp:ListItem>
                                <asp:ListItem>03:00</asp:ListItem>
                                <asp:ListItem>03:30</asp:ListItem>
                                <asp:ListItem>04:00</asp:ListItem>
                                <asp:ListItem>04:30</asp:ListItem>
                                <asp:ListItem>05:00</asp:ListItem>
                                <asp:ListItem>05:30</asp:ListItem>
                                <asp:ListItem>06:00</asp:ListItem>
                                <asp:ListItem>06:30</asp:ListItem>
                                <asp:ListItem>07:00</asp:ListItem>
                                <asp:ListItem>07:30</asp:ListItem>
                                <asp:ListItem>08:00</asp:ListItem>
                                <asp:ListItem>08:30</asp:ListItem>
                                <asp:ListItem>09:00</asp:ListItem>
                                <asp:ListItem>09:30</asp:ListItem>
                                <asp:ListItem>10:00</asp:ListItem>
                                <asp:ListItem>10:30</asp:ListItem>
                                <asp:ListItem>11:00</asp:ListItem>
                                <asp:ListItem>11:30</asp:ListItem>
                                <asp:ListItem>12:00</asp:ListItem>
                                <asp:ListItem>12:30</asp:ListItem>
                                <asp:ListItem>13:00</asp:ListItem>
                                <asp:ListItem>13:30</asp:ListItem>
                                <asp:ListItem>14:00</asp:ListItem>
                                <asp:ListItem>14:30</asp:ListItem>
                                <asp:ListItem>15:00</asp:ListItem>
                                <asp:ListItem>15:30</asp:ListItem>
                                <asp:ListItem>16:00</asp:ListItem>
                                <asp:ListItem>16:30</asp:ListItem>
                                <asp:ListItem>17:00</asp:ListItem>
                                <asp:ListItem>17:30</asp:ListItem>
                                <asp:ListItem>18:00</asp:ListItem>
                                <asp:ListItem>18:30</asp:ListItem>
                                <asp:ListItem>19:00</asp:ListItem>
                                <asp:ListItem>19:30</asp:ListItem>
                                <asp:ListItem>20:00</asp:ListItem>
                                <asp:ListItem>20:30</asp:ListItem>
                                <asp:ListItem>21:00</asp:ListItem>
                                <asp:ListItem>21:30</asp:ListItem>
                                <asp:ListItem>22:00</asp:ListItem>
                                <asp:ListItem>22:30</asp:ListItem>
                                <asp:ListItem>23:00</asp:ListItem>
                                <asp:ListItem>23:30</asp:ListItem>
                            </asp:DropDownList>
                            </td> 
                            <%--            <Button ID="Button1"  OnClientClick=="javascript:PassValues();" Text="Button" />
--%>
 <td>Accpt. Status</td>  
        <td>
        <asp:DropDownList ID="ddlStatus" runat="server">
        <asp:ListItem Text="ALL" Value=""></asp:ListItem>
        <asp:ListItem Text="Partial" Value="P"></asp:ListItem>
        <asp:ListItem Text="Complete" Value="C"></asp:ListItem>
        </asp:DropDownList>
        </td> 
        <td>  
    
Dep.Airport:</td> 
<td>
<asp:Label ID="lblDepAirport" runat="server" Font-Bold="True" Font-Names="Verdana"></asp:Label>
                  </td>
 </tr>
 </table>
 <table>
 <tr>
 <td>
                    <asp:Button ID="BtnList" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" CssClass="button" onclick="BtnList_Click" OnClientClick="StopTimer();" /> 
                    </td><%--OnClientClick="return checkvalid();StopTimer();"--%>
                    <td>
                    <asp:Button ID="BtnClear" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>"  
                      CssClass="button" onclick="BtnClear_Click" OnClientClick="callShow();" />
                      </td>
                      </tr>
                      </table>
                       
                        </div>
                     
                        <div>
                        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
                        
                        </div>
                        
                        <div id="divdetail"  >

                   	  <div id="colleft"  style="width:345px">
                                    <h2> <%--<img alt="" src="Images/unscreenedcargo.png"/>--%>UnScreened Cargo
                                   <%-- <asp:Image ID="imgUnscreen" runat="server" ImageUrl="~/Images/unscreenedcargo.png" />--%>
                                    </h2>
                                    <asp:Label ID="lblCountUnT" runat="server" Visible="false"></asp:Label>
                                   <div class="divback" style="width:325px; background:url(Images/brushed_alu.png);  min-height:355px; -moz-box-shadow:3px 3px 3px #ccc; -webkit-box-shadow:3px 3px 3px #ccc; " >
                                   
                                  <%-- <div style="margin-top:10px;height:345px;overflow:auto" onscroll="applydivval();" id="leftBar" runat="server">--%>
 <div style="margin-top:10px;height:345px;overflow:auto;" id="leftBar" runat="server">
                                    <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" 
                                               style="margin-top:0px;height:335px;"  Visible="true">
                                        <asp:TabPanel ID="TabPanelULD" runat="server" Font-Bold="True"  ScrollBars="Both"
                                            HeaderText="Unscreened" Visible="true" Width="100%">
                                            <HeaderTemplate>
                                                AWB
                                            
                                            
</HeaderTemplate>
                                        
<ContentTemplate>
                                        <div style="overflow:auto;">
                                                <asp:GridView ID="gvUnScreening" runat="server" 
                                                    CellPadding="2" CellSpacing="1" style="z-index: 1" Width="100%" 
                                                    AutoGenerateColumns="False"><Columns>
<asp:TemplateField>
<ItemTemplate>
                                                                <asp:CheckBox ID="chkUnScreen" runat="server" onClick="ChkAllSOrNot(this,'U');"/>
                                                            
</ItemTemplate>
<HeaderTemplate>
   <asp:CheckBox ID="UnChkAll" runat="server" onClick="ChkUnScreenAll(this,'U');" />
                                                            
</HeaderTemplate>

<HeaderStyle Wrap="False" />

<ItemStyle Wrap="False" />
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="AWBNo" HeaderText="AWB">
    <ItemTemplate>
                                                                <asp:Label ID="lblAWBno" runat="server" Text='<%# Eval("AWBNumber") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="Count" HeaderText="Scr Pcs" 
                                                            Visible="False">
    <ItemTemplate>
                                                                <asp:Label ID="lblcntAWBno" runat="server" Text='<%# Eval("CountTag") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="TagID" HeaderText="TagID">
    <ItemTemplate>
                                                                <asp:Label ID="lblTagID" runat="server" Text='<%# Eval("TagID") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="Dest" HeaderText="Dest">
    <ItemTemplate>
                                                                <asp:Label ID="lblDest" runat="server" Text='<%# Eval("DestinationCode") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="CCSF" HeaderText="CCSF" 
                                                            Visible="False">
    <ItemTemplate>
                                                                <asp:Label ID="lblCCSF" runat="server" Text='<%# Eval("CCSF") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="IsSubTag" HeaderText="IsSubTag" 
                                                            Visible="False">
    <ItemTemplate>
                                                                <asp:Label ID="lblSubTag" runat="server" Text='<%# Eval("IsSubTag") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="TagID" HeaderText="TagID" Visible="False">
    <ItemTemplate>
                                                                <asp:Label ID="lblfulTagID" runat="server" Text='<%# Eval("fulTagID") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="Rmrk" HeaderText="Remark" 
                                                            Visible="False">
    <ItemTemplate>
                                                                <asp:Label ID="lblremark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="AddRmrk" 
                                                            HeaderText="Additional Remark" Visible="False">
    <ItemTemplate>
                                                                <asp:Label ID="lbladdremark" runat="server" Text='<%# Eval("AdditionalRemark") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField  HeaderText="Bkd Pcs">
    <ItemTemplate>
                                                                <asp:Label ID="lblBookedPcs" runat="server" Text='<%# Eval("BookedPcs") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField  HeaderText="Bkd Wt">
    <ItemTemplate>
                                                                <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("BookedWt") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField  HeaderText="Acc Pcs">
    <ItemTemplate>
                                                                <asp:Label ID="lblAcceptedPcs" runat="server" Text='<%# Eval("AcceptedPcs") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField  HeaderText="Acc Wt">
    <ItemTemplate>
                                                                <asp:Label ID="lblAcceptedWt" runat="server" Text='<%# Eval("AcceptedWt") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField  HeaderText="Flt #">
    <ItemTemplate>
                                                                <asp:Label ID="lblFlightNo" runat="server" Text='<%# Eval("FltNumber") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField  HeaderText="Flt Date">
    <ItemTemplate>
                                                                <asp:Label ID="lblFlightDate" runat="server" Text='<%# Eval("FltDate") %>'></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField  HeaderText="Req Scr?">
<ItemTemplate>
<asp:Label ID="lblReqScr" runat="server" Text='<%# Eval("IsScrReq") %>'></asp:Label>
<asp:Label ID="txtLocation" Visible="false" runat="server" Text='<%# Eval("Location") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField  HeaderText="Scr Exception" Visible="false">
<ItemTemplate>
<asp:Label ID="lblScrExcep" runat="server" Text='<%# Eval("ScreeningException") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
</Columns>

<AlternatingRowStyle Wrap="False" />

<EditRowStyle CssClass="grdrowfont" />

<FooterStyle CssClass="grdrowfont" />

<HeaderStyle CssClass="titlecolr" Wrap="False" />

<RowStyle CssClass="grdrowfont"  HorizontalAlign="Center" Wrap="False" />
</asp:GridView>

                                                </div>
                                               
                                        
                                               
                                        
</ContentTemplate>
                                    
</asp:TabPanel>
                                    </asp:TabContainer>
                                   </div>
                                   </div>
                                   <div style="margin-top:10px";>
                                   <asp:Button ID="btnCallPnl" runat="server"
                                        Text="<%$ Resources:LabelNames, LBL_BTN_MARKSCREENED %>"  CssClass="button" onclick="btnCallPnl_Click" 
                                        OnClientClick="StopTimer();"/>
                                       
                                        <asp:Button ID="btnReject" runat="server"
                                        Text="<%$ Resources:LabelNames, LBL_BTN_REJECT %>"  CssClass="button" onclick="btnReject_Click" OnClientClick="StopTimer();"/>
                                       <%-- <asp:ConfirmButtonExtender ID="CBEReject" runat="server" 
                                        ConfirmText="Are you Sure for Rejecting this TagID ?" Enabled="True" 
                                        TargetControlID="btnReject" ConfirmOnFormSubmit="false" >
                                        </asp:ConfirmButtonExtender>--%>
                                        
                                         <asp:Button ID="btnScreenNotReq" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SKIP_SCREENING %>"  CssClass="button" OnClientClick="StopTimer();" onclick="btnScreenNotReq_Click"/>
                                         <asp:ConfirmButtonExtender ID="ConfirmButtonExtender3" runat="server"  ConfirmText="Are you Sure for Direct Mark as Screening ?" Enabled="True" TargetControlID="btnScreenNotReq" ConfirmOnFormSubmit="false" >
                                         </asp:ConfirmButtonExtender>
                                   
                                   </div>
                     </div>

<br />                                                    
                    <div id="Exportcolright" style="width:670px;">
                        <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
        </asp:Button>--%><h2>
        <%--<asp:Image ID="imgscreen" runat="server" ImageUrl="~/Images/txt_ScreenedCargo.png" />--%>Screened Cargo
        <%--<img alt="" src="Images/txt_ScreenedCargo.png" />--%></h2>
        <asp:Label ID="lblSCntTag" runat="server"></asp:Label>
        <div style="float:right;margin-bottom:5px;">Filter By X-Ray :   <asp:DropDownList ID="ddlxra" runat="server"></asp:DropDownList></div>
                                            <div class="divback" style="min-height:353px; margin-left:5px;" >

    <div style="margin-top:10px;height:335px;overflow:auto; width:100%; padding-left:2px !important; padding-right:2px !important;" id="Pnlgrd" runat="server" onscroll="applydivval();">
                                     
                                       <asp:GridView ID="gdvULDDetails" runat="server" AutoGenerateColumns="False" 
                                                    CellPadding="3" CellSpacing="2" style="z-index: 1" Width="100%">
                                                    <EmptyDataTemplate>
                                                        No Record Available
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkScreen" runat="server" onClick="ChkAllSOrNot(this,'S');" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="SChkAll" runat="server" onClick="ChkUnScreenAll(this,'S');" />
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        
                                                       <asp:TemplateField AccessibleHeaderText="AWBNo" HeaderText="AWB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSAWBno" runat="server" Text='<%# Eval("AWBNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Count" HeaderText="Scr Pcs"  Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblScntAWBno" runat="server" Text='<%# Eval("CountTag") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="TagID" HeaderText="TagID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSTagID" runat="server" Text='<%# Eval("TagID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Dest" HeaderText="Dest">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSDest" runat="server" Text='<%# Eval("DestinationCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="CCSF" HeaderText="CCSF" 
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSCCSF" runat="server" Text='<%# Eval("CCSF") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Total Scaned" 
                                                            HeaderText="Total Scaned">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSTotScan" runat="server" Text='<%# Eval("TotalCount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="X-Ray" HeaderText="X-Ray">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSXRayID" runat="server" Text='<%# Eval("XRayID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="X-Ray Count" HeaderText="X-Ray Count">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSXrayCount" runat="server" Text='<%# Eval("XrayCount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Time" HeaderText="Time" 
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSXRayTime" runat="server" Text='<%# Eval("XRayTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="K-9" HeaderText="K-9">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSCanineID" runat="server" Text='<%# Eval("CanineID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="K-9 Count" HeaderText="K-9 Count">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSK9Count" runat="server" Text='<%# Eval("K9Count") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Time" HeaderText="Time" 
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSK9Time" runat="server" Text='<%# Eval("CanineTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="ETD" HeaderText="ETD">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSETDID" runat="server" Text='<%# Eval("ETDID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="ETD Count" HeaderText="ETD Count">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSETDCount" runat="server" Text='<%# Eval("ETDCount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Time" HeaderText="Time" 
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSETDTime" runat="server" Text='<%# Eval("ETDTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Physical" HeaderText="Physical">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSPhysical" runat="server" Text='<%# Eval("PhysicalID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Physical Count" 
                                                            HeaderText="Physical Count">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSPhysicalCount" runat="server" 
                                                                    Text='<%# Eval("PhysicalCount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Time" HeaderText="Time" 
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSPhysicalTime" runat="server" 
                                                                    Text='<%# Eval("PhysicalTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="TagID" HeaderText="TagID" 
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSFullTagID" runat="server" Text='<%# Eval("fulTagID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField AccessibleHeaderText="ScanTime" HeaderText="ScanTime">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSUpdatedOn" runat="server" Text='<%# Eval("UpdatedOn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField AccessibleHeaderText="IsSubTag" HeaderText="IsSubTag" 
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSSubTag" runat="server" Text='<%# Eval("IsSubTag") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       
                                                       <%-- New Columns--%>
                                                        <asp:TemplateField AccessibleHeaderText="ScrStn" HeaderText="Screen Station" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSscrstn" runat="server" Text='<%# Eval("ScreenStation") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField AccessibleHeaderText="FltOrg" HeaderText="Flight Origin" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSfltorg" runat="server" Text='<%# Eval("FltOrigin") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       <asp:TemplateField AccessibleHeaderText="UpOn" HeaderText="Updated On" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSupdton" runat="server" Text='<%# Eval("UpdatedOn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField AccessibleHeaderText="FltNo" HeaderText="Flight Number">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSfltno" runat="server" Text='<%# Eval("FltNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="FltDt" HeaderText="Flight Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSfltdt" runat="server" Text='<%# Eval("FltDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       
                                                        <asp:TemplateField AccessibleHeaderText="Carr" HeaderText="Carrier" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblScarrier" runat="server" Text='<%# Eval("Carrier") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="AgCode" HeaderText="AgentCode" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSagcode" runat="server" Text='<%# Eval("AgentCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="GrWt" HeaderText="Gross Weight" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSgrwt" runat="server" Text='<%# Eval("GrossWeight") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField AccessibleHeaderText="rmrk" HeaderText="Remark" Visible="true">
                                                            <ItemTemplate>
                                                            <asp:Label ID="lblSRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="AddRmrk" HeaderText="Additional Remark" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSaddremark" runat="server" Text='<%# Eval("AdditionalRemark") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Returned Pcs" HeaderText="Returned Pieces" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSretpcs" runat="server" Text='<%#Eval("ReturnedPcs")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       <asp:TemplateField HeaderText="Xray lbl From" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSxrayfrm" runat="server" Text='<%#Eval("XrayLblFrm")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Xray lbl To" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSxrayto" runat="server" Text='<%#Eval("XrayLblTo")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                      <asp:TemplateField AccessibleHeaderText="ScrId" HeaderText="Scr ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSId" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Location">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtLocation" runat="server" Text='<%# Eval("Location") %>' ></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Req Scr?" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSScrReq" runat="server" Text='<%# Eval("IsScrReq") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Acc Pcs" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAcceptedPcs" runat="server" Text='<%# Eval("AcceptedPcs") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField  HeaderText="Scr Exception">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblScrExcep" runat="server" Text='<%# Eval("ScreeningException") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                       </Columns>
                                                    <HeaderStyle CssClass="titlecolr" />
                                                    <AlternatingRowStyle CssClass="trcolor" Wrap="False" />
                                                    <EditRowStyle CssClass="grdrowfont" />
                                                    <RowStyle CssClass="grdrowfont" HorizontalAlign="Center" Wrap="False" />
                                                    <FooterStyle CssClass="grdrowfont" />
                                                </asp:GridView>    
                                     
                                      </div>
                                     </div>
                                     <div style="margin-top:10px";>
                                     
                                <asp:Button ID="btnUnassign" runat="server" CssClass="button" 
                                Text="<%$ Resources:LabelNames, LBL_BTN_SKIP_SCREENING%>" Enabled="true" onclick="btnUnassign_Click" OnClientClick="StopTimer();"/>
                               <asp:Button ID="btnUpdateScreen" runat="server" CssClass="button" 
                                        Text="<%$ Resources:LabelNames, LBL_BTN_ADDSCREENING %>" Enabled="true"  OnClientClick="StopTimer();" 
                                             onclick="btnUpdateScreen_Click"/>
                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" 
                        ConfirmText="Are you Sure to Mark these as UnScreened ?" Enabled="True" TargetControlID="btnUnassign" ConfirmOnFormSubmit="false" >
                    </asp:ConfirmButtonExtender>
                         
                        <asp:Button ID="btnReturn" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_RETURN %>" CssClass="button" Enabled="true" 
                        Visible="true" onclick="btnReturn_Click" OnClientClick="StopTimer();"/>
                         <%--<asp:Button ID="btnClose" runat="server" Text="Close" 
                        CssClass="button" Enabled="true" OnClick="btnClose_Click" OnClientClick="StopTimer();" />--%>
                        <asp:Button ID="btnSendFSU" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SENDFSU %>" 
                        CssClass="button" Enabled="true" Visible="true" onclick="btnSendFSU_Click"/>
                       <asp:Button ID="btnPrintMFT" runat="server" Text=" <%$ Resources:LabelNames, LBL_BTN_PRINTSCRRPT %>" CssClass="button" Width="80px"
                           Visible="true" onclick="btnPrintMFT_Click"/>
                           <asp:Button ID="btnSendPRI" runat="server" CssClass="button" Text="Send PRI" 
                                             onclick="btnSendPRI_Click" />
                        <asp:HiddenField ID="hfInvoiceNos" runat="server" Value="" />
  
   <asp:Button ID="btnPrintLbl" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_PRINTLABLES %>" CssClass="button" onclick="btnPrintLbl_Click"/>
    <asp:Button ID="btnPrintAWBDet" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_PRINTACCEPTDETAILS %>" CssClass="button" OnClick="btnPrintAWBDet_Click" />
                                     </div>
                                     
                                      </div>
                                    </div>
                           
                                          
                                        
        </div>
         <div class="showWarC" id="showWar" runat="server" style="height:60px; display:none;">SEQUENCE BREAK WARNING 
        <table width="100%">
        <tr>
        <td width="85%">
         <div id="dptc" style="margin-top:2px;overflow:auto;height:40px;">
			 <table width="100%">
			 <tr>
			 <td>
			                <asp:CheckBoxList ID="chkWarAWB" runat="server" RepeatColumns="10" CellPadding="5" >
                            </asp:CheckBoxList>
			 </td>
			 
			 </tr>
			 </table>
                        </div>	
        </td>
        <td valign="middle"><asp:Button ID="Ignore" runat="server" CssClass="button"  
                Text="Ignore"  OnClientClick="return SelectNoneCheckboxes();" Visible="false" onclick="Ignore_Click"/>
                
                </td>
                
        </tr>
        </table>
			
                       
		</div>
        <div id="fotbut">

                    <%--<input name="Save" type="button" value="Save" />--%>
  </div>
  
  <div id="Lightsplit" class="white_content" style="width: 1150px; left: 5%;" >
<table width="100%">
<tr>
<td>
<asp:Label ID="PanelError" runat="server" ForeColor = "Red"></asp:Label>
</td>
</tr>
<tr>
<td>
 <%--<input type="hidden" id="hdnScrollTop" runat="server" value="0" />--%>
<div>
<%--onscroll="Func2();"  id="divScroll" runat="server"--%>
<%--onscroll="$get('hdnScrollTop').value = this.scrollTop;" id="divScroll"--%>
<%--onscroll='javascript:setScroll(this);' runat="server" id="autoUScroll"--%>
    <asp:GridView ID="GVPopUp" runat="server" AutoGenerateColumns="False"
                                               CellPadding="3" CellSpacing="2" style="display: block; width: 1150px; left: 5%;">
                                               <Columns>
                                                  <%-- <asp:TemplateField>
                                                       <ItemTemplate>
                                                           <asp:CheckBox ID="chkPopScreen" runat="server" />
                                                       </ItemTemplate>
                                                   </asp:TemplateField>--%>
                                                   <asp:TemplateField AccessibleHeaderText="AWBNo" HeaderText="AWB">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblPopAWBno" runat="server" Text='<%# Eval("AWBNumber") %>'></asp:Label>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                     <asp:TemplateField AccessibleHeaderText="Acc Pcs" HeaderText="Acc Pcs">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAcceptedPcs" runat="server" Text='<%# Eval("AcceptedPcs") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                   <asp:TemplateField AccessibleHeaderText="Count" HeaderText="Scr Pcs">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPopcntAWBno" runat="server" Text='<%# Eval("CountTag") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                   <asp:TemplateField AccessibleHeaderText="TagID" HeaderText="TagID">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblPopTagID" runat="server" Text='<%# Eval("TagID") %>'></asp:Label>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                  
                                                   <asp:TemplateField AccessibleHeaderText="Dest" HeaderText="Dest">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblPopDest" runat="server" Text='<%# Eval("DestinationCode") %>'></asp:Label>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField AccessibleHeaderText="CCSF" HeaderText="CCSF">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblPopCCSF" runat="server" Text='<%# Eval("CCSF") %>'></asp:Label>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="Total Scanned" HeaderText="Total Scanned" >
                                                       <ItemTemplate>
                                                           <asp:TextBox ID="lblPopTotScan" runat="server" Text='<%# Eval("TotalCount") %>' Width="50px" MaxLength="5" ValidationGroup="popok"></asp:TextBox>
                                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="lblPopTotScan" SetFocusOnError="true" ></asp:RequiredFieldValidator>
                                                            <asp:FilteredTextBoxExtender ID="lblPopTotScan_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" TargetControlID="lblPopTotScan" 
                                ValidChars="0123456789">
                            </asp:FilteredTextBoxExtender>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="TagID" HeaderText="TagID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPopfulTagID" runat="server" Text='<%# Eval("fulTagID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                   <asp:TemplateField AccessibleHeaderText="X-Ray" HeaderText="X-Ray">
                                                       <ItemTemplate>
                                                        <asp:DropDownList ID="lblPopXRayID" runat="server" 
                                                         DataSource="<%# fillDropinGridXray() %>" DataTextField="Xray" DataValueField="Xray" SelectedValue='<%# Eval("XRayID").ToString() %>'>
                                                        </asp:DropDownList>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="X-Ray Count" HeaderText="X-Ray Count" >
                                                       <ItemTemplate>
                                                           <asp:TextBox ID="lblPopXrayCount" runat="server" Text='<%# Eval("XrayCount") %>' Width="50px" MaxLength="5" ValidationGroup="popok"></asp:TextBox>
                                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="lblPopXrayCount" SetFocusOnError="true" ></asp:RequiredFieldValidator>
                                             <asp:FilteredTextBoxExtender ID="lblPopXrayCount_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="lblPopXrayCount" 
                                ValidChars="0123456789">
                            </asp:FilteredTextBoxExtender>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField AccessibleHeaderText="Time" HeaderText="Time" >
                                                       <ItemTemplate>
                                                         <asp:TextBox ID="txtXrayTime" runat="server" Width="120px" MaxLength="19" Text='<%# Eval("XrayTime") %>'></asp:TextBox>
                            <asp:CalendarExtender ID="txtXrayTime_CalendarExtender" runat="server" 
                                Enabled="True" Format="MM/dd/yyyy HH:mm:ss" TargetControlID="txtXrayTime" >
                            </asp:CalendarExtender>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField AccessibleHeaderText="K-9" HeaderText="K-9">
                                                       <ItemTemplate>
                                                           <asp:DropDownList ID="lblPopCanineID" runat="server" 
                                                               DataSource="<%# fillDropinGridK9() %>" DataTextField="K9" DataValueField="K9"  SelectedValue='<%# Eval("K9ID").ToString() %>'>
                                                           </asp:DropDownList>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="K-9 Count" HeaderText="K-9 Count" >
                                                       <ItemTemplate>
                                                           <asp:TextBox ID="lblPopK9Count" runat="server" Text='<%# Eval("K9Count") %>' Width="50px" MaxLength="5" ValidationGroup="popok"></asp:TextBox>
                                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="lblPopK9Count" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:FilteredTextBoxExtender ID="lblPopK9Count_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" TargetControlID="lblPopK9Count" 
                                ValidChars="0123456789">
                            </asp:FilteredTextBoxExtender>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField AccessibleHeaderText="Time" HeaderText="Time" >
                                                       <ItemTemplate>
                                                        <asp:TextBox ID="txtK9Time" runat="server" Width="120px" MaxLength="19"  Text='<%# Eval("K9Time") %>'></asp:TextBox>
                            <asp:CalendarExtender ID="txtK9Time_CalendarExtender" runat="server" 
                                Enabled="True" Format="MM/dd/yyyy HH:mm:ss" TargetControlID="txtK9Time">
                            </asp:CalendarExtender>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField AccessibleHeaderText="ETD" HeaderText="ETD">
                                                       <ItemTemplate>
                                                           <asp:DropDownList ID="lblPopETDID" runat="server" 
                                                               DataSource="<%# fillDropinGridETD() %>" DataTextField="ETD" DataValueField="ETD" SelectedValue='<%# Eval("ETDID").ToString() %>'>
                                                           </asp:DropDownList>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="ETD Count" HeaderText="ETD Count" >
                                                       <ItemTemplate>
                                                           <asp:TextBox ID="lblPopETDCount" runat="server" Text='<%# Eval("ETDCount") %>' Width="50px" MaxLength="5" ValidationGroup="popok"></asp:TextBox>
                                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="lblPopETDCount" SetFocusOnError="true" ></asp:RequiredFieldValidator>
                                                           <asp:FilteredTextBoxExtender ID="lblPopETDCount_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" TargetControlID="lblPopETDCount" 
                                ValidChars="0123456789">
                            </asp:FilteredTextBoxExtender> 
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="Time" HeaderText="Time">
                                                       <ItemTemplate>
                                                        <asp:TextBox ID="txtETDTime" runat="server" Width="120px" MaxLength="19" Text='<%# Eval("ETDTime") %>'></asp:TextBox>
                            <asp:CalendarExtender ID="txtETDTime_CalendarExtender" runat="server" 
                                Enabled="True" Format="MM/dd/yyyy HH:mm:ss" TargetControlID="txtETDTime">
                            </asp:CalendarExtender>
                                                         
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField AccessibleHeaderText="Physical" HeaderText="Physical">
                                                       <ItemTemplate>
                                                         <%--  <asp:DropDownList ID="lblPopPhysical" runat="server" 
                                                               DataSource="<%# fillDropinGridPhysical() %>" DataTextField="Physical" DataValueField="Physical" SelectedValue='<%# Eval("PhysicalID").ToString() %>'>
                                                           </asp:DropDownList>--%>
                                                           <asp:TextBox ID="lblPopPhysical" runat="server" Width="100px"></asp:TextBox>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                    <asp:TemplateField AccessibleHeaderText="Physical Count" HeaderText="Physical Count" >
                                                       <ItemTemplate>
                                                           <asp:TextBox ID="lblPopPhysicalCount" runat="server" Text='<%# Eval("PhysicalCount") %>' Width="50px" MaxLength="5" ValidationGroup="popok"></asp:TextBox>
                                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ControlToValidate="lblPopPhysicalCount" SetFocusOnError="true" ></asp:RequiredFieldValidator>
                                                            <asp:FilteredTextBoxExtender ID="lblPopPhysicalCount_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" TargetControlID="lblPopPhysicalCount" 
                                ValidChars="0123456789">
                            </asp:FilteredTextBoxExtender> 
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField AccessibleHeaderText="Time" HeaderText="Time" >
                                                       <ItemTemplate>
                                                       <asp:TextBox ID="txtPhysicalTime" runat="server" Width="120px" MaxLength="19" Text='<%# Eval("PhysicalTime") %>'></asp:TextBox>
                                                     <asp:CalendarExtender ID="txtPhysicalTime_CalendarExtender" runat="server" 
                                                      Enabled="True" Format="MM/dd/yyyy HH:mm:ss" TargetControlID="txtPhysicalTime">
                                              </asp:CalendarExtender>
                                                </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField AccessibleHeaderText="IsSubTag" HeaderText="IsSubTag" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPopSubTag" runat="server" Text='<%# Eval("IsSubTag") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Reject Reason" HeaderText="Reject Reason">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="lblPopReject" runat="server" Text="" TextMode="MultiLine"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--New Column--%>
                                                        <asp:TemplateField HeaderText="ScrReq" Visible="true">
                                                            <ItemTemplate>
                                                   <asp:Label ID="lblscrrq" runat="server" Text='<%#Eval("IsScrReq")%>'></asp:Label>             
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField AccessibleHeaderText="Remark" HeaderText="Remark">
                                                       <ItemTemplate>
                                                           <asp:DropDownList ID="ddlRemark" runat="server" DataSource="<%#fillDropinGridRmrk()%>" DataTextField="Remarks">
                                                           </asp:DropDownList>
                                                       </ItemTemplate>
                                                   </asp:TemplateField>
                                                   
                                                         <asp:TemplateField AccessibleHeaderText="Remark" HeaderText="Additional Remark">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAddRemrk" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                          <asp:TemplateField  HeaderText="Flt #">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFlightNo" runat="server" Text='<%# Eval("FltNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField  HeaderText="Flt Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFlightDate" runat="server" Text='<%# Eval("FltDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField AccessibleHeaderText="Exceptions" HeaderText="Exceptions">
                                                       <ItemTemplate>
                                                           <asp:DropDownList ID="ddlScrExceptions" runat="server" DataSource="<%#fillScrExceptionDropDown()%>" DataTextField="ExceptionCode" onchange="javascript:return ScrException(this);" >
                                                           </asp:DropDownList>
                                                       </ItemTemplate>
                                                       </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="Location">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLocation" runat="server" Text='<%# Eval("Location") %>' Width="100px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                               </Columns>
                                               <HeaderStyle CssClass="titlecolr" />
                                               <AlternatingRowStyle CssClass="trcolor" Wrap="False" />
                                               <EditRowStyle CssClass="grdrowfont" />
                                               <RowStyle CssClass="grdrowfont" HorizontalAlign="Center" Wrap="False" />
                                               <FooterStyle CssClass="grdrowfont" />
                                           </asp:GridView>
     
     </div>
     </td>
</tr>
<tr>
<td>
<asp:Button ID="btnPopOK" CssClass="button" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_OK %>" OnClick="btnPopOK_Click"  OnClientClick="callShow();disBtn();" ValidationGroup="popok"/><%--OnClientClick="return Confirm();callShow();"--%>
 <%--<asp:ConfirmButtonExtender ID="ConfirmButtonExtender23" runat="server" 
                        ConfirmText="Are you Sure For your Action ?" Enabled="True" TargetControlID="btnPopOK" ConfirmOnFormSubmit="false" >
                   </asp:ConfirmButtonExtender>--%>
<input type="button" id="btnPopCancel" class="button" value="Cancel" onclick="HidePanelSplit();" /></td>
</tr>
</table>
		</div>
		
		<div id="fadesplit" class="black_overlay"></div>
		<div id="msglight" class="white_contentmsg" >
		 		 
		<div id="Div2" class="black_overlay"></div>
		<div id="Div3" class="white_contentmsg" ></div>
		
		
	
		 
<table>
<tr>
<td width="5%" align="center">
<br />
<img src="Images/loading.gif" id="imgShowmag"/>
<br />
<asp:Label ID="msgshow" runat="server" ></asp:Label>
</td>
</tr>
</table>

		</div>
		<div id="msgfade" class="black_overlaymsg"></div>
		
		<%--New popup grid--%>
<div style="overflow:scroll; height:200px; width:600px;" class="white_content" id="LightDiv">
 <br />
    <table width="100%">
    <tr>
    <td>
    <asp:Label ID="lblAWBNo" runat="server" Text="AWB Number" />    
    </td>
    <td>
    <asp:Label ID="lblAWBDisplay" runat="server" Text="" />    
    </td>
    <td>
    <asp:Label ID="lblAWBpcs" runat="server" Text="AWB Pieces" />    
    </td>
    <td>
    <asp:Label ID="lblAWBPcsDisplay" runat="server" Text="" />    
    </td>
    </tr>
    <%--<tr>
    <td>
    <asp:Label ID="lblAWBpcs" runat="server" Text="AWB Pieces" />    
    </td>
    <td>
    <asp:Label ID="lblAWBPcsDisplay" runat="server" Text="" />    
    </td>
    <td>
    <asp:Label ID="lblAWBWT" runat="server" Text="AWB Weight" />    
    </td>
    <td>
    <asp:Label ID="lblAWBWTDisplay" runat="server" Text="" />    
    </td>
    </tr>--%>
    <tr>
    <td>
    <asp:Label ID="lblReturnPcs" runat="server" Text="Ret. Pieces" />    
    </td>
    <td>
    <asp:TextBox ID="txtReturnPcs" runat="server" Text="" />    
    </td>
    <td>
    <asp:Label ID="lblReturnWt" runat="server" Text="Ret. Weight" />    
    </td>
    <td>
    <asp:TextBox ID="txtReturnWt" runat="server" Text="" />  
    </td>
    </tr>
    </table>
    <br />
    <asp:Button ID="unsOk" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_RETURNTOSHIPPER %>" CssClass="button" onclick="unsOk_Click"/>&nbsp;
    <input type="button" id="unsCancel" class="button" value="Cancel" onclick="HidePanellSplit();" />
</div>
<div id="fadeDiv" class="black_overlay">
        </div>
<%--New popup grid--%> 
		
<%--   </ContentTemplate>
        <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="tmrRef" EventName="Tick" />
                </Triggers>
        </asp:UpdatePanel> --%>
    <br />
    <br />
    </asp:Panel>
</asp:Content>

