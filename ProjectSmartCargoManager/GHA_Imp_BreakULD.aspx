<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GHA_Imp_BreakULD.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.GHA_Imp_BreakULD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

   

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script language="javascript" type="text/javascript">

    function RadioCheck(rb) {
        var gv = document.getElementById("<%=gdvULDLoadPlan.ClientID%>");
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
    <body></body>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <div id="contentarea">
                       
                  <h1>Break ULD</h1>
                      
                        <div class="botline">
                        <table width="100%">
                        <tr>
                        <td>
                            Flight
                            </td>
                            <td>
                            <asp:TextBox ID="txtFlightPrefix" runat="server" Width="35px" MaxLength="4"></asp:TextBox> 
                    <asp:TextBox ID="txtFlightID" runat="server" Width="65px" MaxLength="6"></asp:TextBox>
                            
                                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server"
    TargetControlID="txtFlightID"
    WatermarkText="Flight ID"
     /><asp:TextBox ID="TextBoxdate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="TextBoxdate_CalendarExtender" runat="server" 
                                Enabled="True" Format="dd/MM/yyyy" TargetControlID="TextBoxdate" PopupButtonID="imgDate">
                            </asp:CalendarExtender>
                            <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                            </td>
                            <td>
                            AWB
                            </td>
                            <td><asp:TextBox ID ="txtAWBPrefix" runat="server" Text="" Width="35px" MaxLength="4"></asp:TextBox>  
                              <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
    TargetControlID="txtAWBPrefix"
    WatermarkText="Prefix"
     />
                            <asp:TextBox ID ="txtAWBNo" runat="server" Text="" Width="95px" MaxLength="10"></asp:TextBox>  
                            <asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
    TargetControlID="txtAWBNo"
    WatermarkText="AWB#"
     />
     </td>
     <td>
                              ULD#
                              </td>
                              <td><asp:TextBox ID="txtULDNo" runat="server" Width="100px"></asp:TextBox><asp:DropDownList ID="ddlULDNo" runat="server" Visible="false">
                             <asp:ListItem ></asp:ListItem>
                            </asp:DropDownList>
                            </td>
                            <td>
                            Arr.Airport:
                            </td>
                            <td>
                    <asp:Label ID="lblDepAirport" runat="server" Font-Bold="True" 
                        Font-Names="Verdana"></asp:Label>
                 
                    </td>
                    </tr>
                    </table>
                    <table>
                    <tr>
                    <td>
                    <asp:Button ID="BtnList" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" CssClass="button" onclick="BtnList_Click" 
                                />
                    <asp:Button ID="BtnClear" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>"  
                      CssClass="button" onclick="BtnClear_Click" />
                       <asp:DropDownList ID="ddlFlightCode" 
                                runat="server" Width="45px" Visible="false"></asp:DropDownList>
                                </td>
                                </tr>
                                </table>
                    
                        </div>
                        
                        <div>
                        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
                        
                        </div>
                        
                        <div id="divdetail"  >

                   	  <div id="colleft"  style="width:280px">
                                     <h2>ULD Details</h2>
                                   <div class="divback" style="width:260px; background:url(Images/brushed_alu.png);  min-height:300px; -moz-box-shadow:3px 3px 3px #ccc; -webkit-box-shadow:3px 3px 3px #ccc; " >
                                   <div style="margin-top:10px">
                                  
                                            <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" 
                                                Width="260px" Height="170px"  ScrollBars="Both" Visible="true">
                                        <asp:TabPanel ID="TabPanelULD" runat="server" Font-Bold="True"  ScrollBars="Both"
                                            HeaderText="ULD" Visible="true">
                                            <HeaderTemplate>ULD</HeaderTemplate>
                                            
<ContentTemplate><asp:GridView ID="gdvULDLoadPlan" runat="server" CellPadding="3" CellSpacing="2" 
                                                                          BorderStyle="None" Width="100%" 
                                                    AutoGenerateColumns="False"><Columns><asp:TemplateField ><ItemTemplate><asp:RadioButton ID="Check1" runat="server" onclick="javascript:RadioCheck(this);" /></ItemTemplate><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" /></asp:TemplateField>
                                                    <asp:BoundField AccessibleHeaderText="ULDno" DataField="ULDno" 
                                                            HeaderText="ULDno" />
                                                            <asp:BoundField AccessibleHeaderText="ULDType" DataField="ULDType" HeaderText="ULDType" />
                                                            <asp:BoundField AccessibleHeaderText="AWBCount" DataField="AWBCount" 
                                                            HeaderText="AWB Count" /><asp:BoundField AccessibleHeaderText="PCS" DataField="PCS" 
                                                            HeaderText="PCS" />
                                                            <asp:BoundField AccessibleHeaderText="Wgt" DataField="Wgt" 
                                                            HeaderText="Wgt" />
                                                            <asp:BoundField AccessibleHeaderText="Location" DataField="ULDLocation" HeaderText="Location" />
                                                            
                                                            </Columns><AlternatingRowStyle  Wrap="False"  /><EditRowStyle CssClass="grdrowfont" /><FooterStyle CssClass="grdrowfont"/><HeaderStyle CssClass="titlecolr" Wrap="False" /><RowStyle CssClass="grdrowfont" Wrap="False" 
                                     HorizontalAlign="Center" /></asp:GridView></ContentTemplate>
                                        
</asp:TabPanel>
                                        <asp:TabPanel ID="TabPanelAWB" runat="server" Font-Bold="True"  ScrollBars="Both"
                                            HeaderText="AWB" Visible="false" >
                                            <HeaderTemplate>AWB</HeaderTemplate>
                                            
<ContentTemplate><asp:GridView ID="gdvULDLoadPlanAWB" runat="server" AutoGenerateColumns="False" 
 CellPadding="2" CellSpacing="1" style="z-index: 1" Width="100%"><AlternatingRowStyle Wrap="False" /><Columns><asp:TemplateField><ItemTemplate><asp:CheckBox ID="Check2" runat="server" Enabled="true" /></ItemTemplate><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" /></asp:TemplateField><asp:TemplateField AccessibleHeaderText="AWBNo" HeaderText="AWB"><ItemTemplate><asp:Label ID="lblAWBno" runat="server"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField AccessibleHeaderText="Pieces" HeaderText="PCS"><ItemTemplate><asp:Label ID="lblPieces" runat="server"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField AccessibleHeaderText="Weight" HeaderText="Wgt"><ItemTemplate><asp:Label ID="lblWeight" runat="server"></asp:Label></ItemTemplate></asp:TemplateField></Columns><EditRowStyle CssClass="grdrowfont" /><FooterStyle CssClass="grdrowfont" /><HeaderStyle CssClass="titlecolr" Wrap="False" /><RowStyle CssClass="grdrowfont"  HorizontalAlign="Center" Wrap="False" /></asp:GridView></ContentTemplate>
                                        
                                        
</asp:TabPanel>
                                    </asp:TabContainer>
                                   
                                   
                                   </div>
                                   <div style="margin-top:10px";>
                                  <table cellspacing="9px" cellpadding="5px">
                                  <tr>
                                 <td>
                                    
                               </td>
                               <td>
                                   <%--            <Button ID="Button1"  OnClientClick=="javascript:PassValues();" Text="Button" />
--%>
                                        
                                    <asp:Button ID="btnAddULDToManifest" runat="server" 
                                        Text="Add ULD Manifest" CssClass="button"  Visible="false"/>
                                        </td>
                               </tr>
                                <tr>
                               <td>
                                    <asp:Button ID="BtnAddtoManifest" runat="server"
                                        Text="<%$ Resources:LabelNames, LBL_BTN_BREAKFROMULD %>"  CssClass="button" 
                                        onclick="BtnAddtoManifest_Click" />
                               </td>
                               <td>
                                    <asp:Button ID="btnSplitAssign" runat="server" 
                                            Text="<%$ Resources:LabelNames, LBL_BTN_SPLIT&ASSIGN %>"  CssClass="button" 
                                        onclick="btnSplitAssign_Click" Visible="false"/>
                                        </td>
                              </tr>
                              
                               
                              
                                   </table>
                                   </div>
                                   
                                   </div>
                           
                     </div>
                     
                                                    
                    <div id="Exportcolright" style="width:739px;">
                        <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
        </asp:Button>--%>
                     
                                            
                                    <div class="divback" style="min-height:300px; margin-left:5px; padding-left:7px;" >
                                    
                                     <div>
                                   <%--<table width="100%">
                                   <tr>
                                   <td>
                                    ULD No:
                                   </td>
                                   <td>
                                   <asp:TextBox ID="txtULDNumber" runat="server" Width="100px" />
                                   </td>
                                   <td>
                                    Loading Priority:
                                   </td>
                                   <td>
                                   <asp:TextBox ID="txtLoadingPriority" runat="server" Width="100px" />
                                   </td>
                                   <td>
                                    Location:
                                   </td>
                                   <td>
                                   <asp:TextBox ID="txtLocation" runat="server" Width="100px" />
                                   </td>
                                   </tr>
                                   </table>--%>
                                     </div> 
                                      
                                     <div style="margin-top:25px">
                                      <h2>AWB Details</h2>
                                         <asp:Panel ID="Pnlgrd" runat="server" ScrollBars="Auto" Height="200px" 
                                             style="margin-top:20px"
                                             BorderStyle="Solid" BorderWidth="1px" Width="720px">
                                             <asp:GridView ID="gdvULDDetails"  
                              runat="server" CellPadding="3" CellSpacing="2" AutoGenerateColumns="False" style="z-index: 1">
                                               
                                                 <Columns>
                                                     <asp:TemplateField>
                                                         <HeaderTemplate>
           <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);" />
      </HeaderTemplate>
                                                         <ItemTemplate>
                                                             <asp:CheckBox ID="Check0" runat="server"  />
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                      <asp:TemplateField Visible="false">
                                                         <ItemTemplate>
                                                             <asp:Label ID="lblULDNo" runat="server" Text='<%# Eval("ULDno") %>' Visible="false" ></asp:Label>
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <asp:TemplateField Visible="false">
                                                         <ItemTemplate>
                                                             <asp:Label ID="lblAWBNumber" runat="server" Text='<%# Eval("AWBNo") %>' Visible="false" ></asp:Label>
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <asp:BoundField AccessibleHeaderText="ULD" DataField="ULDno" 
                                                            HeaderText="ULD" Visible="false" />
                                                             <asp:BoundField AccessibleHeaderText="AWB#" DataField="AWBNo" 
                                                            HeaderText="AWB#" />
                                                     <asp:BoundField AccessibleHeaderText="Weight" DataField="POL" 
                                                            HeaderText="POL" />
                                                              <asp:BoundField AccessibleHeaderText="Pieces" DataField="POU" 
                                                            HeaderText="POU" />
                                                     <asp:BoundField AccessibleHeaderText="ArrPcs" DataField="ArrivedPcs" 
                                                            HeaderText="Arrived Pcs" />
                                                            <asp:BoundField AccessibleHeaderText="ArrWt" DataField="ArrivedWt" 
                                                            HeaderText="Arrived Wt" />
                                                     <asp:BoundField AccessibleHeaderText="location" DataField="Location" 
                                                            HeaderText="Location" Visible="false" />
                                                            <asp:TemplateField HeaderText="Location">
                                                            <ItemTemplate>
                                                            <asp:TextBox ID="txtLocation" runat="server" Text='<%# Eval("Location") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                            <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Eval("SerialNumber") %>'></asp:Label>
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
                                    <asp:Button ID="btnSave" runat="server" CssClass="button" 
                                        Text="<%$ Resources:LabelNames, LBL_BTN_SAVE %>" Enabled="true" onclick="btnSave_Click1" />
                                    <asp:Button ID="btnUnassign" runat="server" CssClass="button" 
                                        Text="<%$ Resources:LabelNames, LBL_BTN_UNASSIGN  %>" Enabled="true" onclick="btnUnassign_Click" />
                                 <asp:Button ID="btnReassign" runat="server" CssClass="button"  
                                        Text="Reassign"  Enabled="False" />
                                    <asp:Button ID="btnSplitUnassign" runat="server" CssClass="button"  
                                        Text="Split &amp; Unassign" onclick="btnSplitUnassign_Click" Visible="False" />
                                <%--<asp:Button ID="btnCloseCTM" runat="server" Text="Print Token" 
                        CssClass="button" Enabled="true" Visible="true" />--%>
                                     
                                     </div>
                                     
                                      </div>
                                    </div>
                           
                                          
                                        
                
                           
                                          
                                        
            
                           
                                          
                                        
                
                           
                                          
                                        
        </div>
        
        <div style="display:none">
        
        
        <fieldset id="BulkSummary0" style="border:1px solid #69b3d8;" >
                                        <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Bulk Summary</legend>
                                       <table style="width:50%;" cellspacing="3px" cellpadding="3px">
                                     <tr>
                                     
                                     <td><img src="Images/txtbulksummery1.png" /></td>
                                     
                                     <td> 
                                     <b>AWBs</b> :<asp:Label ID="lblAWBCnt" runat="server" Text="2" Height="16px"></asp:Label> 
                                    
                                           </td>
                                     <td><b>PCS :</b><asp:Label ID="lblAWBPCS" runat="server" Text="50" Height="16px"></asp:Label></td> 
                                     <td><b>&nbsp;Wt : </b>
                                        <asp:Label ID="lblAWBWt" runat="server" Text="500kg" Height="18px"></asp:Label></td> 
                                        <td> <b>Vol :</b>
                                        <asp:Label ID="lblAWBVol" runat="server" Text="50" Height="16px"></asp:Label> </td>  
                                          
                                         </tr>
                                          </table>
                                    </fieldset>
         <fieldset id="Fieldset1" style="border:1px solid #69b3d8;" >
                                        <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">ULD Summary</legend>
                                       <table style="width:50%;" cellspacing="3px" cellpadding="3px">
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
                                         </tr>
                                          </table>
                                    </fieldset>
        <br />
        <br />
        <div class="ltfloat">
       <table width="100%">
       <tr>
       <%--<td>
           <asp:DropDownList ID="ddlULD" runat="server">
           <asp:ListItem Text="Select ULD"></asp:ListItem>
           </asp:DropDownList>
       </td>--%>
       <td>Scale Weight:</td>
       <td>
           <asp:TextBox ID="txtScaleWeight" runat="server"></asp:TextBox>
       </td><td><td></td></td>
       <td>Unit:kg</td>
       <td></td><td></td><td></td><td></td><td></td><td></td>
       <td>Location:</td>
       <td>
           <asp:TextBox ID="txtLoc" runat="server"></asp:TextBox>
       </td>
       <td></td><td></td>
       <td>
           <asp:Button ID="btnSaveULD" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SAVE %>" CssClass="button" />
       </td>
       </tr>
       </table>
        </div>
        
        </div>
        
                        <div id="fotbut">

                    <%--<input name="Save" type="button" value="Save" />--%> 
                    
                 
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
                    <asp:Button ID="btnAddRouteDetails" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_ADD %>" CssClass="button" OnClick="btnAddRouteDetails_Click"/>
                    &nbsp;
                    <asp:Button ID="btnDeleteRouteDetails" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_DELETE %>" CssClass="button"
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
                                <asp:TextBox ID="txtFltDest" runat="server" Width="55px" 
                                    Text='<%# Eval("FltDestination") %>' CssClass="styleUpper" 
                                    ontextchanged="txtFltDest_TextChanged" AutoPostBack="true">
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
                        
                        
                        <asp:TemplateField HeaderText="Flight    Date *" HeaderStyle-Width="10px"  
                            HeaderStyle-Wrap="true">
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
                                <asp:DropDownList ID="ddlFltNum" runat="server" Width="90px" 
                                    OnSelectedIndexChanged="txtFltNumber_TextChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                   <asp:TextBox ID="txtFlightID" runat="server" Visible="false" AutoPostBack="false"
                              Width="90px"></asp:TextBox>
                                <asp:TextBox ID="NewFlightID" runat="server" Visible="false"></asp:TextBox>
                                <asp:HiddenField ID="hdnFltNum" runat="server" 
                                    Value='<%# Eval("FltNumber") %>' />
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Pcs">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPcs" runat="server" Width="70px" Text='<%# Eval("Pcs") %>' 
                                    MaxLength="5">
                                </asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gross Wt">
                            <ItemTemplate>
                                <asp:TextBox ID="txtWt" runat="server" Width="80px" Text='<%# Eval("Wt") %>' 
                                    MaxLength="9">
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
         <asp:Button ID="btnAddManifest" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_ADDTOMANIFEST %>" CssClass="button" OnClick="btnAddManifest_Click">
        </asp:Button>
       
<%--            <Button ID="Button1"  OnClientClick=="javascript:PassValues();" Text="Button" />
--%>          
                  <asp:Button ID="btnShowEAWB" runat="server" Text="Click Me" 
                CssClass="button" OnClientClick="callexport();" Visible="False" />
            
            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>" CssClass="button" OnClick="btnCancel_Click">
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
                        <asp:Button ID="btnOK" CssClass="button" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SENDEMAIL %>" OnClick="btnOK_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnSitaUpload" CssClass="button" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SENDVIA_SITA %>" OnClick="btnSitaUpload_Click" />
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
		
</asp:Content>
