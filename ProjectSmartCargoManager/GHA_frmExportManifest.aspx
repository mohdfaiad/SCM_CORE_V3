<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" EnableViewState="true" AutoEventWireup="True" CodeBehind="GHA_frmExportManifest.aspx.cs" Inherits="ProjectSmartCargoManager.frmExportManifest_GHA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

   

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">

        function CustomsPopup(Mode, AWBNo, FlightNo, FlightDate) {

            window.open('frmCustomsPopup.aspx?Mode=' + Mode + '&AWBNo=' + AWBNo + '&FlightNo=' + FlightNo + '&FlightDate=' + FlightDate, 'openPopUp', 'left=250,top=200,width=890,height=450,toolbar=0,resizable=1,scrollbars=1');
        }
        
        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }

        function SetOperationTime() {
            //Show popup for saving actual operation time.
            //window.open('frmOperationTime.aspx', 'Operation Time','left=400,top=200,width=400,height=200,toolbar=0,resizable=no');
//            window.open('frmOperationTime.aspx','', 'width=400px,height=200px,left=400,top=200');
            //            return false;
            document.getElementById('divOpsTimePopup').style.display = 'block';
            document.getElementById('msgfadeOps').style.display = 'block';
        }
        
        function ShowOffloadPopup(ManifestMode) {
            var FlightNo = document.getElementById("<%=txtFlightCode.ClientID %>").value + document.getElementById("<%=txtFlightID.ClientID %>").value;
            var FlightDt = document.getElementById("<%=TextBoxdate.ClientID %>").value;
            var Station = document.getElementById('ctl00_ContentPlaceHolder1_lblDepAirport').innerText;
            //alert(ManifestMode);
            window.open('frmAWBOffload.aspx?FltNo='+FlightNo + '&FltDt='+FlightDt + '&Station='+ Station + '&ManifestMode=' + ManifestMode,'', 'width=800px,height=450px,left=200,top=100');
            return false;
        }
        
        function RefreshList() {
            __doPostBack('RefreshList', '');
        }

        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=gdvULDDetails.ClientID %>");
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

        function SelectAllgdvULDLoadPlanAWB(headerchk) {
            var gvcheck = document.getElementById("<%=gdvULDLoadPlanAWB.ClientID %>");
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

        function SelectAllgdvULDLoadPlan(headerchk) {
            var gvcheck = document.getElementById("<%=gdvULDLoadPlan.ClientID %>");
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

        function popup() {

            var Fltdt = document.getElementById("<%= TextBoxdate.ClientID %>").value;
            var Fltcd = document.getElementById("<%= txtFlightCode.ClientID %>").value;
            
            var Fltid = document.getElementById("<%= txtFlightID.ClientID %>").value;
            var fltno = Fltcd + Fltid;
            
            
            window.open('UCRPopup.aspx?Type=New' + '&Mode=M' + '&FlightNo=' + fltno + '&FlightDate=' + Fltdt, '', 'left=0,top=0,width=1000,height=1000,toolbar=0,resizable=0');
        }
 
 
    function select() {

        //var hldyVal = document.getElementById("<%= ddlReason.ClientID%>").value;
        
        var e = document.getElementById("<%=ddlReason.ClientID%>");
        var hldyVal = e.options[e.selectedIndex].text;

        var txt = document.getElementById("<%= txtReason.ClientID%>");
        var TextReason = document.getElementById('ctl00_ContentPlaceHolder1_txtReason');
        //alert(hldyVal);
        //alert(txt);
        if (hldyVal == "Others") {
            TextReason.value = "";
            TextReason.disabled = false;

        }
        else if (hldyVal != "Others") {
        TextReason.value = "";
        TextReason.disabled = true;

        }
        return true;
    }
window.onload()=function(){
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
    
        function display_alertAWBULD()
          {
            alert("Please Select ULD from DDL and AWB from TAB");
        }

        function alertSelectAWBULD() 
        {
            alert("Please Select ULD from Tab And/Or AWB from TAB to add to Manifest");
        }

        function alertAWB() 
        {
            alert("Please Select Atleast One AWB ");
        }

          function Successfull() 
          {
              alert(" Version Save Successfull");
          }
          function AlreadyAvailable() 
          {
              alert(" ULD Already Available");
          }

          function ULDAWBAssocitionSuccessfull() 
          {
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

        function GenerateInvoices() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");

            var InvList = hfInvNos.value;

            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=WalkIn' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowWalkInAgentInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }
        
       
        
        function CloseWindow() {
            document.getElementById('divOpsTimePopup').style.display = 'none';
            document.getElementById('msgfadeOps').style.display = 'none';
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
 
<script language="javascript" type="text/javascript" >

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
          else if (ddl.value != 'Others') 
         {
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
        .black_overlay
            {
                  display: none;
                  position: absolute;
                  top: 0%;
                  left: 0%;
                  width: 100%;
                  height: 1000px;
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
                  top: 30%;
                  left: 35%;
                  width: 30%;
                  height: 45%;
                  padding: 16px;
                  border: 16px solid #ccdce3;
                  background-color: white;
                  z-index:1002;
                  overflow: auto;
            
            }
    </style>
   
        <style>
.ajax__calendar .ajax__calendar_invalid .ajax__calendar_day 
{
    background-color:gray;
    color:White; 
    text-decoration:none; 
    cursor:default;
}
</style>
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
</script>
 </asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"  EnableViewState="true">
   
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        
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
        .styleUpper
        {
            text-transform: uppercase;
        }
    </style>
        
        <div id="contentarea">
                       
                  <h1>Export Manifest </h1>
                      
                        <div class="botline">
                        Flight #&nbsp; <asp:TextBox ID="txtFlightCode" 
                                runat="server" Width="45px" MaxLength="2" CssClass="styleUpper"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server"
                                TargetControlID="txtFlightCode"
                                WatermarkText="Prefix"/>
                    <asp:TextBox ID="txtFlightID" runat="server" Width="55px" MaxLength="4" AutoPostBack="false"
                                OnTextChanged="txtFlightID_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ReqFltno" runat="server" 
                                ControlToValidate="txtFlightID" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server"
    TargetControlID="txtFlightID"
    WatermarkText="Flight ID"
     />
                            <asp:TextBox ID="TextBoxdate" runat="server" Width="85px" AutoPostBack="false" OnTextChanged="txtFlightID_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                            <asp:CalendarExtender ID="TextBoxdate_CalendarExtender" runat="server" PopupButtonID="imgDate" 
                                Enabled="True" Format="dd/MM/yyyy" TargetControlID="TextBoxdate">
                            </asp:CalendarExtender>
                    &nbsp;&nbsp;<asp:RequiredFieldValidator ID="ReqFielddt" runat="server" 
                                ControlToValidate="TextBoxdate" ErrorMessage="*"></asp:RequiredFieldValidator>  &nbsp;&nbsp;
                                  <asp:Label ID="Label2" runat="server" Text="OR" Font-Bold="true"></asp:Label> &nbsp;&nbsp;
                            AWB: <asp:TextBox ID ="txtAWBPrefix" runat="server" Text="" Width="55px"></asp:TextBox>  &nbsp;
                              <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
    TargetControlID="txtAWBPrefix"
    WatermarkText="Prefix"
     />
                            <asp:TextBox ID ="txtAWBNo" runat="server" Text="" Width="95px"></asp:TextBox>  &nbsp;&nbsp;
                            <asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
    TargetControlID="txtAWBNo"
    WatermarkText="AWB#"
     />
                           <%--<asp:Label ID="lblOption1" runat="server" Text="OR" Font-Bold="true"></asp:Label> 
                             &nbsp;&nbsp; ULD: --%>
                             <asp:DropDownList ID="ddlULDNo" runat="server" Visible="false">
                             <asp:ListItem >Select ULD</asp:ListItem>
                            </asp:DropDownList>  &nbsp;&nbsp;
                            Dep.Airport:&nbsp;
                    <asp:Label ID="lblDepAirport" runat="server" Font-Bold="True" 
                        Font-Names="Verdana"></asp:Label>
                        &nbsp;
                    <asp:ImageButton ID="imgNotebtn" ImageAlign="AbsMiddle" runat="server" 
                    OnClientClick="javascript:return OpenNotesChild();" ImageUrl="~/Images/noteicon.png" ToolTip="Notes" 
                    Height="20px" Width="34px" style="padding-bottom:2px;" />
                        <br />
                    <asp:Button ID="BtnList" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" CssClass="button" 
                                onclick="BtnList_Click" />
                    <asp:Button ID="BtnClear" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>"  
                      CssClass="button" onclick="BtnClear_Click" />   
                        </div>
                        
                        <div class="rout">
                        <img src="Images/txtflightdetails1.png" />
                        
                         <asp:Label ID="lblRoute" runat="server"></asp:Label>
                           <asp:Label ID="lblDate" runat="server"  ></asp:Label>
                            &nbsp;   &nbsp;  &nbsp;  &nbsp;  &nbsp; &nbsp;  &nbsp; &nbsp;  &nbsp;
                           Tail No
                            <asp:DropDownList ID="ddlTailNo" runat="server" Visible="false">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtTailNo" runat="server" MaxLength="15" Visible="true"></asp:TextBox>
                            &nbsp;  &nbsp;&nbsp;&nbsp;
                            EGM/IGM No.
                             <asp:TextBox ID="txtEMG" runat="server" MaxLength="15"></asp:TextBox>
                        </div>
                     
                        <div>
                        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
                        
                        </div>
                        
                        <div id="divdetail"  >

                   	  <div id="colleft"  style="width:280px">
                                     <%--<img alt="" src="Images/txt_flightbookinglist.png" />--%>
                                     <h2>Flight Booking List</h2>
                                   <div class="divback" style="width:260px; background:url(Images/brushed_alu.png);  min-height:355px; -moz-box-shadow:3px 3px 3px #ccc; -webkit-box-shadow:3px 3px 3px #ccc; " >
                                   
                                   <div>
                                                              
                                
                                    <asp:Label ID="lblPOU" runat="server" Text="POU"></asp:Label>
                                
                               
                                    <asp:DropDownList ID="ddlMainPOU" runat="server">
                                       </asp:DropDownList>
                                
                               
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                           ControlToValidate="txtPOU" ErrorMessage="*"></asp:RequiredFieldValidator>
                                
                                 <asp:Button ID="btnAssign" runat="server" 
                                        Text="Assign To ULD" CssClass="button" onclick="btnAssign_Click" 
                                           Visible="false"/>
                                        
                                    <asp:Button ID="BtnLoadPlanRefList" runat="server"  Text="<%$ Resources:LabelNames , LBL_BTN_LIST %>" 
                                        CssClass="button" onclick="BtnLoadPlanRefList_Click" />
                                
                                
                                    <asp:Button ID="BtnLoadPlanRefClear" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>" 
                                        CssClass="button" onclick="BtnLoadPlanRefClear_Click" />
                                       
                                         <asp:TextBox ID="txtPOU" runat="server" Width="41px" Visible="False">DEL</asp:TextBox>            
                                       
                                       
                                                     
                                   </div>
                                   
                                   
                                   <div style="margin-top:10px">
                                  
                                            <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="1" 
                                                Width="260px" Height="186px"  ScrollBars="Both">
                                        <asp:TabPanel ID="TabPanelULD" runat="server" Font-Bold="True"  ScrollBars="Both"
                                            HeaderText="ULD">
                                            <HeaderTemplate>
                                                ULD
                                            
                                            
</HeaderTemplate>
                                            
<ContentTemplate>
                                                <asp:GridView ID="gdvULDLoadPlan" runat="server" CellPadding="3" CellSpacing="2" 
                                                                          BorderStyle="None" Width="100%" 
                                                    AutoGenerateColumns="False" >
                                                    <Columns>
                                                        <asp:TemplateField >
                                                        <HeaderTemplate>
                                                           <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="javascript:SelectAllgdvULDLoadPlan(this);" />
                                                      </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Check1" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:TemplateField>                                                       
                                                        
                                                         <asp:BoundField AccessibleHeaderText="ULDno" DataField="ULDno" 
                                                            HeaderText="ULD" />
                                                    <%--    <asp:BoundField AccessibleHeaderText="POU" DataField="POU" 
                                                            HeaderText="POU" />--%>
                                                                  
                                                        <asp:BoundField AccessibleHeaderText="AWBCount" DataField="AWBCount" 
                                                            HeaderText="AWB Ct" />              
                                                                  
                                                        <asp:BoundField AccessibleHeaderText="PCS" DataField="PCS" 
                                                            HeaderText="AWB Pcs" />              
                                                        <asp:BoundField AccessibleHeaderText="Wgt" DataField="Wgt" 
                                                            HeaderText="AWB Wt" />                                                      
                                                        <asp:TemplateField AccessibleHeaderText="FltFlag" HeaderText="FltFlag" Visible="false" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblFltFlag" Text='<%# Eval("FltNo") %>' runat="server"></asp:Label>
                                                            
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="titlecolr" Wrap="False" />
                                                      
                                    <AlternatingRowStyle  Wrap="False"  />
                                    <EditRowStyle CssClass="grdrowfont" />
                                    <RowStyle CssClass="grdrowfont" Wrap="False" 
                                     HorizontalAlign="Center" />
                                    <FooterStyle CssClass="grdrowfont"/>
                                                </asp:GridView>
                                                
                                            
                                                
                                            
</ContentTemplate>
                                        
</asp:TabPanel>
                                        <asp:TabPanel ID="TabPanelAWB" runat="server" Font-Bold="True"  ScrollBars="Both"
                                            HeaderText="AWB" >
                                            <HeaderTemplate>
                                                AWB
                                            
                                            
</HeaderTemplate>
                                            
<ContentTemplate>
                                                <asp:GridView ID="gdvULDLoadPlanAWB" runat="server" AutoGenerateColumns="False" 
                                                    CellPadding="2" CellSpacing="1" style="z-index: 1" Width="100%" 
                                                    >
<AlternatingRowStyle Wrap="False" />
<Columns>
<asp:TemplateField>
<HeaderTemplate>
                                                           <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="javascript:SelectAllgdvULDLoadPlanAWB(this);" />
                                                      </HeaderTemplate>
<ItemTemplate>
                                                                <asp:CheckBox ID="Check2" runat="server" Enabled="true" />
                                                            
</ItemTemplate>

<HeaderStyle Wrap="False" />

<ItemStyle Wrap="False" />
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="CartNo" HeaderText="Cart No"><ItemTemplate>
                                                                <asp:Label ID="lblCartNumber" runat="server"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="AWBNo" HeaderText="AWB"><ItemTemplate>
                                                                <asp:Label ID="lblAWBno" runat="server"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="Pieces" HeaderText="Rem <br/>Pcs"><ItemTemplate>
                                                                <asp:Label ID="lblPieces" runat="server"></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="Weight" HeaderText="Rem <br/>Wt"><ItemTemplate>
                                                                <asp:Label ID="lblWeight" runat="server"></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="AvlPCS" HeaderText="Acc <br/>Pcs"><ItemTemplate>
                                                                <asp:Label ID="lblAvlPCS" runat="server"></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="AvlWgt" HeaderText="Acc <br/>Wt"><ItemTemplate>
                                                                <asp:Label ID="lblAvlWgt" runat="server"></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="TotPCS" HeaderText="Bkd <br/>Pcs"><ItemTemplate>
                                                                <asp:Label ID="lblTotPCS" runat="server"></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="TotWgt" HeaderText="Bkd <br/>Wt"><ItemTemplate>
                                                                <asp:Label ID="lblTotWgt" runat="server"></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField AccessibleHeaderText="FltFlag" HeaderText="FltFlag" 
        Visible="False" ><ItemTemplate>
                                                                <asp:Label ID="lblFltFlag" runat="server"></asp:Label>
                                                            
</ItemTemplate>
</asp:TemplateField>
</Columns>

<EditRowStyle CssClass="grdrowfont" />

<FooterStyle CssClass="grdrowfont" />

<HeaderStyle CssClass="titlecolr" Wrap="False" />

<RowStyle CssClass="grdrowfont"  HorizontalAlign="Center" Wrap="False" />
</asp:GridView>
                                            

                                            
</ContentTemplate>
                                        
                                        
</asp:TabPanel>
                                    </asp:TabContainer>
                                   
                                   
                                   </div>
                                   <div style="margin-top:10px";>
                                  <table cellspacing="9px" cellpadding="5px">
                                  <tr>
                                 <td>
                                    <asp:DropDownList ID="ddlSelectULD" runat="server" Height="17px" Width="92px" 
                                         AppendDataBoundItems="True" Enabled = "false" Visible="false">
                                        <asp:ListItem>Select ULD</asp:ListItem>                                                                             
                                     
                                    </asp:DropDownList>
                               </td>
                               <td>
                                   <%--            <Button ID="Button1"  OnClientClick=="javascript:PassValues();" Text="Button" />
--%>
                                        
                                    <asp:Button ID="btnAddULDToManifest" runat="server" Visible="false"
                                        Text="Add ULD Manifest" CssClass="button"  onclick="btnAddULDToManifest_Click"/>
                                        </td>
                               </tr>
                                <tr>
                               <td>
                                    <asp:Button ID="BtnAddtoManifest" runat="server"
                                        Text="Add To Manifest"  CssClass="button" 
                                       onclick="BtnAddtoManifest_Click" Width="200px"/>
                               </td>
                               <td>
                                   
                                        </td>
                              </tr>
                              
                               <tr>
                               <td>
                            <asp:Button ID="btnSplitAssign" runat="server" 
                                        Text="Split &amp; Assign"  CssClass="button" 
                                        onclick="btnSplitAssign_Click" Width="100px"/>
                                   <asp:Button ID="btnReAssign" runat="server" Width="100px"
                                        Text="Re-Assign" CssClass="button" OnClick="btnReAssign_Click" OnClientClick="return GetProcessFlag();"/>     
                                        
                               </td>
                                <td>
                                   
                               </td>
                              </tr>
                              <tr>
                              <td>
                              <asp:Button ID="btnReturnToShipper" runat="server" Width="200px"
                                        Text="Return To Shipper" CssClass="button" OnClick="btnReturnToShipper_Click" Visible="true" OnClientClick="return GetProcessFlag();"/>
                              </td></tr>
                                   </table>
                                   </div>
                                   
                                   </div>
                           
                     </div>
                     
                                                    
                    <div id="Exportcolright" style="width:730px;" >
                        <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
        </asp:Button>--%>
                      <h2><%--<img alt="" src="Images/txtshipping.png" /> --%>
                                        Shipment Details
                                       
                                    </h2>
                                            <div class="divback" style="min-height:380px; margin-left:5px; padding-left:7px;" >
                                    <div>

                                   Version
                                    <asp:DropDownList ID="ddlVersion" runat="server" >
                                        <asp:ListItem>Select</asp:ListItem>
                                        
                                    </asp:DropDownList>
                                
                                
                                <%--ULD--%>
                                    <asp:DropDownList ID="ddlULD" runat="server" Visible="false">
                                        <asp:ListItem Selected="True">Select</asp:ListItem>
                                       
                                    </asp:DropDownList>
                              
                               
                              POL
                                    <asp:DropDownList ID="ddlPOLDetails" runat="server" >
                                        <asp:ListItem Selected="True">Select</asp:ListItem>
                                       
                                    </asp:DropDownList>
                                
                               
                                POU
                                        <asp:DropDownList ID="ddlPOU" runat="server">
                                        <asp:ListItem Selected="True">Select</asp:ListItem>
                                       
                                    </asp:DropDownList>
                                
                                    <asp:Button ID="BtnListDetails" runat="server" CssClass="button" 
                                         
                                        Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" onclick="BtnListDetails_Click1"  />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                
                                <asp:Label ID="lblManifest" runat="server" Text="Current Version :"></asp:Label>
                                     <asp:Label ID="lblVersionID" runat="server" Text=""></asp:Label>
                                       </div>
                                       
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
                                            <asp:TextBox ID="lblFetchFltDate" runat="server" Text="" Enabled="false" Width="80px"></asp:TextBox>
                                            <asp:TextBox ID="lblFetchFlt" runat="server" Text="" Enabled="false" Width="40px"></asp:TextBox>
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
                                      
                                     <div style="margin-top:25px">
                                         <asp:Panel ID="Pnlgrd" runat="server" ScrollBars="Auto" Height="200px" 
                                             style="margin-top:20px"
                                             BorderStyle="Solid" BorderWidth="1px" Width="100%">
                                             <asp:GridView ID="gdvULDDetails"  
                              runat="server" CellPadding="3" 
                                            CellSpacing="2" 
                                            AutoGenerateColumns="False" style="z-index: 1" onselectedindexchanged="gdvULDDetails_SelectedIndexChanged">
                                                 <%-- onselectedindexchanged="gdvULDDetails_SelectedIndexChanged" --%>
                                                 <Columns>
                                                     <asp:TemplateField>
                                                     <HeaderTemplate>
                                                           <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);" />
                                                      </HeaderTemplate>
                                                         <ItemTemplate>
                                                             <asp:CheckBox ID="Check0" runat="server"  />
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <%--<asp:BoundField AccessibleHeaderText="POL" DataField="POL" 
                                                            HeaderText="POL" />--%>
                                                     <asp:TemplateField AccessibleHeaderText="POL" HeaderText="POL" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblPOL" Text='<%# Eval("POL") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <%--<asp:BoundField AccessibleHeaderText="POU" DataField="POU" 
                                                            HeaderText="POU" />--%>
                                                     <asp:TemplateField AccessibleHeaderText="POU" HeaderText="POU" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblPOU" Text='<%# Eval("POU") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <%--<asp:BoundField AccessibleHeaderText="ULD" DataField="ULDno" 
                                                            HeaderText="ULD" />--%>
                                                     <asp:TemplateField AccessibleHeaderText="ULD" HeaderText="ULD" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblULDno" Text='<%# Eval("ULDno") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <asp:TemplateField AccessibleHeaderText="Cart" HeaderText="Cart No" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblCartNumber" Text='<%# Eval("CartNumber") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <%--<asp:BoundField AccessibleHeaderText="AWBno" DataField="AWBno" 
                                                            HeaderText="AWB" />--%>                                                     
                                                     <asp:TemplateField AccessibleHeaderText="AWBno" HeaderText="AWBno" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblAWBno" Text='<%# Eval("AWBno") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <%--<asp:BoundField AccessibleHeaderText="BookedPCS" DataField="BookedPCS" 
                                                        HeaderText="Bkd Pcs"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true"/>--%>
                                                     <asp:TemplateField AccessibleHeaderText="BookedPCS" HeaderText="Bkd Pcs" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblBookedPCS" Text='<%# Eval("BookedPCS") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <%--<asp:BoundField AccessibleHeaderText="BookedWgt" DataField="BookedWgt" 
                                                        HeaderText="Bkd Wt"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true"/>--%>
                                                     <asp:TemplateField AccessibleHeaderText="BookedWgt" HeaderText="Bkd Wt" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblBookedWgt" Text='<%# Eval("BookedWgt") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <%--<asp:BoundField AccessibleHeaderText="StatedPCS" DataField="StatedPCS" 
                                                            HeaderText="Acc Pcs"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true"/>--%>
                                                     <asp:TemplateField AccessibleHeaderText="StatedPCS" HeaderText="Acc Pcs" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblStatedPCS" Text='<%# Eval("StatedPCS") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                            
                                                     <%--<asp:BoundField AccessibleHeaderText="StatedWgt" DataField="StatedWgt" 
                                                            HeaderText="Acc Wt"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true"/>--%>
                                                     <asp:TemplateField AccessibleHeaderText="StatedWgt" HeaderText="Acc Wt" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblStatedWgt" Text='<%# Eval("StatedWgt") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <%--<asp:BoundField AccessibleHeaderText="PCS" DataField="PCS" 
                                                            HeaderText="Mft Pcs"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true"/>--%>
                                                     <asp:TemplateField AccessibleHeaderText="MftPcs" HeaderText="Mft Pcs" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblMftPcs" Text='<%# Eval("PCS") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <%--<asp:BoundField AccessibleHeaderText="GrossWGT" DataField="GrossWGT" 
                                                            HeaderText="Mft Wt"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true" />--%>
                                                      <asp:TemplateField AccessibleHeaderText="MftWt" HeaderText="Mft Wt" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblMftWt" Text='<%# Eval("GrossWGT") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <%--<asp:BoundField AccessibleHeaderText="SCC" DataField="SCC" 
                                                            HeaderText="Comm Code" />--%>
                                                     <asp:TemplateField AccessibleHeaderText="SCC" HeaderText="SCC" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblSCC" Text='<%# Eval("SCC") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <%--<asp:BoundField AccessibleHeaderText="Desc" DataField="Desc" 
                                                            HeaderText="Comm Desc" />--%>
                                                     <asp:TemplateField AccessibleHeaderText="CommDesc" HeaderText="Comm Desc" Visible ="true" ItemStyle-Width="100px">
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblCommDesc" Text='<%# Eval("Desc") %>' runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <%--<asp:BoundField AccessibleHeaderText="Orign" DataField="Orign" 
                                                            HeaderText="Orign" />--%>
                                                     <asp:TemplateField AccessibleHeaderText="Origin" HeaderText="Origin" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblOrigin" Text='<%# Eval("Orign") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <%--<asp:BoundField AccessibleHeaderText="Dest" DataField="AWBDest" 
                                                            HeaderText="Dest" />--%>
                                                     <asp:TemplateField AccessibleHeaderText="Dest" HeaderText="Dest" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblAWBDest" Text='<%# Eval("AWBDest") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <%--<asp:BoundField AccessibleHeaderText="Bonded" DataField="Bonded" 
                                                            HeaderText="Bonded" />--%>
                                                     <asp:TemplateField AccessibleHeaderText="Bonded" HeaderText="Bonded" Visible ="true" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblBonded" Text='<%# Eval("Bonded") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <asp:TemplateField AccessibleHeaderText="Remark" HeaderText="Remarks">
                                                         <ItemTemplate>
                                                             <asp:TextBox ID="txtRemark" runat="server" Text='<%# Eval("Remark") %>' MaxLength="100"></asp:TextBox>
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Loading Priority">
                                                         <ItemTemplate>
                                                             <asp:DropDownList ID="ddlLoadingPriority"    runat="server">
                                                                 <asp:ListItem >First</asp:ListItem>
                                                                 <asp:ListItem >Second</asp:ListItem>
                                                                 <asp:ListItem >Third</asp:ListItem>
                                                             </asp:DropDownList>
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <asp:TemplateField AccessibleHeaderText="Location" HeaderText="Location">
                                                         <ItemTemplate>
                                                             <asp:TextBox ID="txtLocation" runat="server" Text='<%# Eval("Location") %>' MaxLength="50"></asp:TextBox>
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <%--<asp:BoundField AccessibleHeaderText="ULDdest" DataField="AWBDest" Visible="false" 
                                                            HeaderText="ULD Dest"  HeaderStyle-Width ="5px" HeaderStyle-Wrap="true" />--%>
                                                     <asp:TemplateField AccessibleHeaderText="AWBDest" HeaderText="AWBDest" Visible="false">
                                                         <ItemTemplate>
                                                             <asp:Label ID="lblAWBDest1" Text='<%# Eval("AWBDest") %>'  runat="server"></asp:Label>
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <%--<asp:BoundField AccessibleHeaderText="Counter" DataField="counter" ItemStyle-Width="0px"
                                                            HeaderText="Type" Visible="false"/>--%> 
                                                     <asp:TemplateField AccessibleHeaderText="counter" HeaderText="Counter" Visible="false">
                                                         <ItemTemplate>
                                                             <asp:Label ID="lblcounter" Text='<%# Eval("counter") %>'  runat="server"></asp:Label>
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                                                                         
                                                     <%--<asp:BoundField AccessibleHeaderText="VOL" DataField="VOL" 
                                                            HeaderText="VOL" Visible="false"/>--%>    
                                                     <asp:TemplateField AccessibleHeaderText="VOL" HeaderText="VOL" Visible="false">
                                                         <ItemTemplate>
                                                             <asp:Label ID="lblVOL" Text='<%# Eval("VOL") %>'  runat="server"></asp:Label>
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                                                                      
                                                       <%--<asp:BoundField AccessibleHeaderText="Dest" DataField="Manifested" 
                                                            HeaderText="Manifested"/>--%>
                                                     <asp:TemplateField AccessibleHeaderText="Manifested" HeaderText="Manifested" Visible="false">
                                                         <ItemTemplate>
                                                             <asp:Label ID="lblManifested" Text='<%# Eval("Manifested") %>'  runat="server"></asp:Label>
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                     
                                                     <asp:TemplateField AccessibleHeaderText="FltFlag" HeaderText="FltFlag" Visible ="false" >
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblFltFlag" Text='<%# Eval("FltNo") %>'  runat="server"></asp:Label>                                                            
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                 </Columns>
                                                 <HeaderStyle CssClass="titlecolr"/>
                                                 <AlternatingRowStyle CssClass="trcolor" Wrap="False" />
                                                 <EditRowStyle CssClass="grdrowfont" />
                                                 <RowStyle CssClass="grdrowfont" Wrap="False" HorizontalAlign="Center" />
                                                 <FooterStyle CssClass="grdrowfont"/>
                                             </asp:GridView>                                            
                                         </asp:Panel>
                                     
                                     
                                     </div>
                                     <div style="margin-top:10px";>
                                     
                                    <asp:Button ID="btnOffload" runat="server" CssClass="button" Visible="True"
                                        Text="<%$ Resources:LabelNames, LBL_BTN_OFFLOAD %>" Enabled="False" onclick="btnOffload_Click" OnClientClick="return GetProcessFlag();"/>
                               
                                    <asp:Button ID="btnUnassign" runat="server" CssClass="button" 
                                        Text="<%$ Resources:LabelNames, LBL_BTN_UNASSIGN   %>" Enabled="true" onclick="btnUnassign_Click" />
                                
                                    <asp:Button ID="btnSplitUnassign" runat="server" CssClass="button"  
                                        Text="<%$  Resources:LabelNames, LBL_BTN_SPLIT&UNASSIGN%>" onclick="btnSplitUnassign_Click" />
                                <br /><br />
                                <asp:Label ID="lblIrregularities" runat="server" Text="Irregularity Codes : "></asp:Label>
                                &nbsp;&nbsp;<asp:DropDownList ID="ddlIrregularityCode" runat="server" Width="345px"></asp:DropDownList>                            
                                     
                                     </div>
                                     <div id="sumamry" style="display:none;">
                                      <fieldset id="BulkSummary0" style="border:1px solid #69b3d8;" visible="false" >
                                        <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Summary </legend>
                                       <table style="width:100%;" cellspacing="3px" cellpadding="3px">
                                     <tr>
                                     <td> <img src="Images/txtsummary1.png" /></td>
                                     <td><b>ULDs :</b><asp:Label ID="lblULDs" runat="server" Height="16px" Text="1"></asp:Label>
                                     <b>AWBs</b> :<asp:Label ID="lblULDAWBs" runat="server" Text="2" Height="16px"></asp:Label> 
                                        
                                       
                                   &nbsp;<b>PCS :</b><asp:Label ID="lblULDPCS" runat="server" Text="50" Height="16px"></asp:Label>
                                                                              
                                    <b>&nbsp;Wt : </b>
                                        <asp:Label ID="lblULDWt" runat="server" Text="500kg" Height="18px"></asp:Label>
                                         
                                            <b>Vol :</b>
                                        <asp:Label ID="lblULDVol" runat="server" Text="50" Height="16px"></asp:Label>
                                     </td>
                                     <td><img src="Images/txtbulksummery1.png" /></td>
                                     
                                     <td> 
                                     <b>AWBs</b> :<asp:Label ID="lblAWBCnt" runat="server" Text="2" Height="16px"></asp:Label> 
                                        
                                       
                                   &nbsp;<b>PCS :</b><asp:Label ID="lblAWBPCS" runat="server" Text="50" Height="16px"></asp:Label>
                                                                              
                                    <b>&nbsp;Wt : </b>
                                        <asp:Label ID="lblAWBWt" runat="server" Text="500kg" Height="18px"></asp:Label>
                                         
                                            <b>Vol :</b>
                                        <asp:Label ID="lblAWBVol" runat="server" Text="50" Height="16px"></asp:Label> </td>
                                        </td>
                                      
                                        <td>
                                        <div>
                                    
                                   
                                        
                                </div>
                                        </td>
                                         </tr>
                                          </table>
                                    </fieldset>
                                     </div>
                                      </div>
                                    </div>
                           
                                          
                                        
        </div>
        
        <%--<div id = "fotbut1" class="ltfloat">
        <table width = "190%">
        <tr style = "width:100%">
        <td style = "width:25%">
        </td>
        
        <td align = "left">
         <asp:Label ID="lblIrregularities" runat="server" Text="Irregularity Codes : "></asp:Label>
        </td>
        <td>
         <asp:DropDownList ID="ddlIrregularityCode" runat="server" Width="345px"></asp:DropDownList>                            
        </td>
        </tr>
        </table>
        </div>--%>
        
                        <div id="fotbut">

                    <%--<input name="Save" type="button" value="Save" />--%> 
                   
                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SAVE %>"
                        CssClass="button" onclick="btnSave_Click" />
                        <asp:Button ID="btnFinalize" runat="server" 
                        Text="<%$ Resources:LabelNames,LBL_BTN_MANIFEST  %>" CssClass="button" 
                                onclick="btnFinalize_Click" />
                        <asp:Button ID="btnDepartFit" runat="server" 
                        Text="<%$ Resources:LabelNames, LBL_BTN_DEPARTFLT %>" CssClass="button" 
                                onclick="btnDepartFit_Click" />
                        <span style="vertical-align:bottom;"><asp:ImageButton ID="btnOpsTime" 
                                runat="server" ImageUrl="~/Images/timecalender.png" 
                           Enabled="true" onclick="btnOpsTime_Click"  CssClass="imgclock"/></span>  
                        &nbsp;   
                        <asp:Button ID="btnReopenFit" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_REOPENFLIGHT %>"
                                        CssClass="button" onclick="btnReopenFit_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSendFFM" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SENDFFM %>" 
                        CssClass="button" onclick="btnSendFFM_Click" />
                         <asp:Button ID="btnSendFBL" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SENDFBL  %>" 
                        CssClass="button" onclick="btnSendFBL_Click"  /> 
                         <asp:Button ID="btnSendFDM" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SENDFDM %>"
                          CssClass="button" onclick="btnSendFDM_Click"  />                   
                         <asp:Button ID="btnSendFRI" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SENDFRI %>" 
                          CssClass="button" onclick="btnSendFRI_Click"  /> 
                           <asp:Button ID="btnSendPRI" runat="server" Text="Send PRI" 
                          CssClass="button" onclick="btnSendPRI_Click"  />                  
                    <asp:Button ID="btnNOTOC" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_NOTOC %>" 
                        CssClass="button" onclick="btnNOTOC_Click" />                   
                    <asp:Button ID="btnPrintMFT" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_PRINTMFT %>" CssClass="button" onclick="btnPrintMFT_Click" />
                    <asp:Button ID="btnprintUCR" runat="server" 
                        Text="<%$ Resources:LabelNames, LBL_BTN_PRINTUCR %>" CssClass="button" OnClientClick="popup()" Visible="false"/>                   
  
                    <%--<asp:Button ID="btnClose" runat="server" Text="Close" 
                        CssClass="button" Enabled="False" Visible="false" />--%>
                   
                      <asp:Button ID="btnMailManifest" runat="server" 
                        Text="<%$ Resources:LabelNames, LBL_BTN_MAILMANIFEST %>" CssClass="button" Enabled="False" Visible="False" /> 
                        <%--<asp:Button ID="btnCommManifest" runat="server" 
                        Text="<%$ Resources:LabelNames, LBL_BTN_PICKLIST %>" CssClass="button"/>--%> 
                         <asp:Button ID="btnePouch" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_EPOUCH %>" CssClass="button" 
                                onclick="btnePouch_Click" />
                          <%--<asp:Button ID="btnAssignULD" runat="server" Text="Assign ULD" 
                CssClass="button" Enabled="False" Visible="False" />--%>
                
                 <%--<asp:Button ID="btnCloseCTM" runat="server" Text="CTM" 
                        CssClass="button" Enabled="False" Visible="False" />--%>
                 <%--<asp:Button ID="btnTesting" runat="server" Text="Testing" 
                        CssClass="button" Enabled="False" Visible="False" onclick="btnTesting_Click" />--%>
                    
                        
                        <asp:HiddenField ID="hfInvoiceNos" runat="server" Value="" />
                        <asp:HiddenField ID="hdnManifestFlag" runat="server" Value="" />
    
  </div>
  
   <asp:Panel ID="pnlGrid" runat="server"  BackColor="White" ScrollBars="Auto"  Visible="false"  
                      BorderStyle="Solid" Height="385px" Width="600px">
    <div style="margin:10px;"> 
        <asp:GridView ID="grdAWBs" runat="server" AutoGenerateColumns="False"
            ShowFooter="True" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="AWB">
                    
                    <ItemTemplate>
                        <asp:TextBox ID="txtAWBno" runat="server"  Width="80px" Enabled="false"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pieces" >
                    
                    <ItemTemplate>
                        <asp:TextBox ID="txtPCS" runat="server" MaxLength="4" Width="55px" Enabled="true"  ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Weight" >
                 
                    <ItemTemplate>
                        <asp:TextBox ID="txtweight" runat="server"  MaxLength="4" Enabled="true"
                            Width="55px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Manifested Pcs" HeaderStyle-Wrap="true" HeaderStyle-Width="10px">
                   
                    <ItemTemplate>
                        <asp:TextBox ID="txtAvlPCS" runat="server" Width="55px" Enabled="false" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Manifested Wgt"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true">
                    
                    <ItemTemplate>
                        <asp:TextBox ID="txtAwlWeight" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Origin"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true">
                    
                    <ItemTemplate>
                        <asp:TextBox ID="txtOrigin" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Destination"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true">
                    
                    <ItemTemplate>
                        <asp:TextBox ID="txtDestination" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                
                   <asp:TemplateField HeaderText="ULDNo"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true">
                    
                    <ItemTemplate>
                        <asp:Label ID="lblULDNO" runat="server" Width="55px" Enabled="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Cart No"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true">                    
                    <ItemTemplate>
                        <asp:Label ID="lblCartNumber" runat="server" Width="55px" Enabled="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
            </Columns>
            <HeaderStyle CssClass="titlecolr" />
            <RowStyle HorizontalAlign="Center" />
            <AlternatingRowStyle HorizontalAlign="Center" />
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
                    <asp:Button ID="btnAddRouteDetails" runat="server" Text="Add" CssClass="button" OnClick="btnAddRouteDetails_Click"/>
                    &nbsp;
                    <asp:Button ID="btnDeleteRouteDetails" runat="server" Text="Delete" CssClass="button"
                        OnClick="btnDeleteRoute_Click" />
                    &nbsp;
                    </h2>
                        <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" CssClass="grdrowfont"
                            Width="399px" ID="grdRouting" >
                    <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="CHKSelect" runat="server" />
                                <asp:HiddenField ID="HidScheduleID" runat="server"  />
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
                                <asp:TextBox ID="txtFltDest" runat="server" Width="55px" Text='<%# Eval("FltDestination") %>' CssClass="styleUpper" ontextchanged="txtFltDest_TextChanged" AutoPostBack="true">
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
                        
                        
                        <asp:TemplateField HeaderText="Flight    Date *" HeaderStyle-Width="10px"  HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFdate" runat="server" Width="80px" 
                                    Text='<%# Eval("FltDate") %>' AutoPostBack="True"
                                    ontextchanged="txtFdate_TextChanged" onblur="javascript:txtDatefocus();"></asp:TextBox>
                                <asp:CalendarExtender ID="TextBox7_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtFdate" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Flight #*" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlFltNum" runat="server" Width="90px" OnSelectedIndexChanged="txtFltNumber_TextChanged" AutoPostBack="false">
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
                                <asp:TextBox ID="txtRouteLocation" runat="server" Width="80px" MaxLength="50">
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
            
             

        </asp:UpdatePanel></div><%--</fieldset>--%>
        
        <table width="100%">
        <tr>
        <td> 
        <asp:Label id="lblNextFlight" Text="Asgn. to Nxt Flt" runat="server" Visible="false">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>&nbsp;&nbsp;&nbsp;
        
        <asp:TextBox ID="txtNextFlight" runat = "server" Visible="false" Width="85px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
        &nbsp;&nbsp;&nbsp; 
        <asp:Label id="lblNFltDate" Text="Nxt Flt Dt." runat="server" Visible="false">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtNFltDate" runat = "server" Visible="false" Width="100px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>        
        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtNFltDate">
         </asp:CalendarExtender>
        </td>
        </tr>
        <tr>
        <td>
        <asp:Label id="lblReason" Text="Reason" runat="server" Visible="false"></asp:Label>&nbsp;&nbsp;&nbsp;
    
                    <asp:DropDownList ID="ddlReason" 
                    runat="server" 
                 onchange="javascript:return select();" 
               >
            </asp:DropDownList>
   
       <asp:TextBox ID="txtReason" runat = "server" Width="335px"></asp:TextBox><%--       <input type="txtReason" id="other" name="other" style="display: none;"/>   
--%>
        </td>
        </tr>
        <tr>
        <td>
         <asp:Button ID="btnAddManifest" runat="server" Text="Add To Manifest" CssClass="button" OnClick="btnAddManifest_Click">
        </asp:Button>
       
<%--            <Button ID="Button1"  OnClientClick=="javascript:PassValues();" Text="Button" />
--%>          
                  <asp:Button ID="btnShowEAWB" runat="server" Text="Click Me" 
                CssClass="button" OnClientClick="callexport();"
            OnClick="btnShowEAWB_Click" Visible="False" />
            
            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CANCEL %>" CssClass="button" OnClick="btnCancel_Click">
        </asp:Button>
        </td>
            <td>
             <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
        </asp:Button>--%>
        </td>
        
        </tr>
        <tr>
        <td colspan="2">
            <asp:Label ID="Label1" runat="server"></asp:Label></td></tr></table></div></asp:Panel></div><div visible="false">
       
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
        <asp:HiddenField ID="HidFltFlag" runat="server" />
        
    </div>
     	
		<div id="fadesplit" class="black_overlay"></div>
		
		<div id="Lightsplit" class="white_content">
     	     <table>
		        <tr>
		            <td>
		                            <asp:Label ID="lblMsgType" runat="server" Text="Message Type :" ForeColor="Blue"></asp:Label></td><td>
		                            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Blue"></asp:Label></td></tr><tr>
                    <td>
                        <asp:Label ID="lblComm" runat="server" Text="Message Communication Type :" ForeColor="Blue"></asp:Label></td><td>
                        <asp:Label ID="lblMsgCommType" runat="server" Text="" ForeColor="Blue"></asp:Label></td></tr></table><table>
            <tr>
            <td>
            <asp:Label ID="lblEmail" runat="server" Text="To Email ID : (Comma Seprated EmailID)" ForeColor="Blue"></asp:Label></td></tr></table><table width="100%">
            <tr>
            
            <td>
            <asp:TextBox ID="txtEmailID" runat="server" TextMode="MultiLine"  Width="300px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
            </td>
            </tr>
              <tr>
             <td>
            <asp:TextBox ID="txtSITAHeader" runat="server" TextMode="MultiLine" visible ="false" 
                Height="50px" Width="600px" style="OVERFLOW:auto"  ></asp:TextBox></td></tr><tr>
             <td>
            <asp:TextBox ID="txtMessageBody" runat="server" TextMode="MultiLine"  
                Height="300px" Width="600px" style="OVERFLOW:auto"  ></asp:TextBox></td></tr></table><table>
          <tr>
                    <td>
                        <asp:Button ID="btnOK" CssClass="button" runat="server" Text="<%$ Resources:LabelNames,  LBL_BTN_SENDEMAIL%>" OnClick="btnOK_Click" />
                        </td>
                        <td>
                        <asp:Button ID="btnSitaUpload" CssClass="button" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SENDVIA_SITA%>" OnClick="btnSitaUpload_Click" />
                        </td>
                        <td>                        
                        <asp:Button ID="btnFTPUpload" CssClass="button" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_FTPUPLOAD %>" OnClick="btnFTPUpload_Click"/>
                        </td>
                        <td>
                        <input type="button" id="Button1" class="button" value="Cancel" onclick="HidePanelSplit();" />
                    </td>
                </tr>
            </table>
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
        <div id="msgfadeOps" class="black_overlay"></div>
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
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" 
                             ImageUrl="~/Images/calendar_2.png" />
    
    </td><td style="width:70px;" >
                        <asp:TextBox ID="txtOpsTimeHr" runat="server" DataTextField="" Width="70px" 
                           ></asp:TextBox></td>
                           <td style="width:120px;" valign="bottom">
                            <asp:TextBox ID="txtOpsTimeMin" runat="server" DataTextField="" Width="70px"></asp:TextBox>
                            (HR:MI)</td>
                        
                        <td>                
                        <asp:CalendarExtender ID="txtOpsDate_CalendarExtender" Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtOpsDate" PopupButtonID="ImageButton1" PopupPosition="BottomLeft">
                         </asp:CalendarExtender>
                        
                             <asp:NumericUpDownExtender ID="txtDeptTimeHr_NumericUpDownExtender" 
                                        runat="server" Maximum="23" Minimum="0" RefValues="00;01;02;03;04;05;06;07;08;09;10;11;12;13;14;15;16;17;18;19;20;21;22;23;" ServiceDownMethod="" 
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
	
</asp:Content>

