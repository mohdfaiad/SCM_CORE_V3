<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GHA_Imp_DockDelivery.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.GHA_Imp_DockDelivery" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<%--<script language="javascript" type="text/javascript">
 
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
        
</script>--%>   

 <%--<script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>--%>
    <%--<script  type="text/javascript">
    window.onload = function() { document.getElementById('<%= txtReason.ClientID %>').style.display = "none"; };

</script>--%>
 
<%--<script language="javascript" type="text/javascript" >

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
  </script>--%>

<%--<asp:TextBox ID="txtAgentName" runat="server" Width="95px" ></asp:TextBox>--%>
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
<%--<script type ="text/javascript">
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
</script>--%>
 </asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"  EnableViewState="true">
    <body></body>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <div id="contentarea">
                       
                  <h1>Dock Delivery </h1>
                      
                        <div class="botline">
                        <table width="100%" >
        <tr>
        <td>
            Flight Date 
         </td >
        
        <td>
            <asp:TextBox ID="txtflightdate" runat="server" Width="70px" ></asp:TextBox>
            <asp:CalendarExtender ID="txtflightdate_CalendarExtender" runat="server" 
                TargetControlID="txtflightdate" Format="dd/MM/yyyy" PopupButtonID="imgDate"> 
            </asp:CalendarExtender>
            <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
            </td>
        <td>
            Flight No:</td>
        <td>
            <asp:DropDownList ID="DdlFlightno" runat="server" AppendDataBoundItems="True" 
                Width="80px">
            </asp:DropDownList>
        </td>
        <td>
            Agent Name:</td>
        <td >
            <asp:DropDownList ID="ddlAgentName" runat="server" Width="100px" >
            </asp:DropDownList>
            <%--<asp:TextBox ID="txtAgentName" runat="server" Width="95px" ></asp:TextBox>--%>
        </td>
         <td>
            <asp:CheckBox ID="chkReprint" runat="server" Checked="False" Width="2px" 
                Visible="False">
            </asp:CheckBox>
        </td>
        </tr>
        <tr>
        <td>
        ULD No:
        </td>
        <td>
         <asp:TextBox ID="txtULDNo" runat="server"></asp:TextBox>
        </td>
        <td>
            AWB NO:</td >
        <td>
        <asp:TextBox ID="txtprefix" runat="server" Width="30px" ></asp:TextBox>
         <asp:TextBox ID="txtAWBNo" runat="server" Width="70px" MaxLength="8"  ></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="btnList" runat="server" Text="List"  CssClass="button" 
               CausesValidation="False"/>
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" /> 
        
      <asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" 
              />
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
                                     <h2>AWB Details</h2>
                                   <div class="divback" style="width:260px; background:url(Images/brushed_alu.png);  min-height:300px; -moz-box-shadow:3px 3px 3px #ccc; -webkit-box-shadow:3px 3px 3px #ccc; " >
                                   
                                   <div style="margin-top:10px">
                                  
                                            <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="1" 
                                                Width="260px" Height="186px"  ScrollBars="Both" Visible="true">
                                        <asp:TabPanel ID="TabPanelULD" runat="server" Font-Bold="True"  ScrollBars="Both"
                                            HeaderText="ULD" Visible="true">
                                            <HeaderTemplate>
                                                ULD
                                            
                                            
</HeaderTemplate>
                                            
<ContentTemplate>
                                                <asp:GridView ID="gdvULDLoadPlan" runat="server" AutoGenerateColumns="False" 
                                                    CellPadding="2" CellSpacing="1" style="z-index: 1" Width="100%">
           
                                                <Columns>
                                            <asp:TemplateField >
                                                        
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Check1" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:TemplateField>
             <asp:TemplateField HeaderText="AWB" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAWBNo" runat="server" Text = '<%# Eval("AWBNumber") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="PCS" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblPCS" runat="server" Text = '<%# Eval("PiecesCount") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
                         
             <asp:TemplateField HeaderText="Wt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblWt" runat="server" Text = '<%# Eval("GrossWeight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
                      
            </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
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
                                                    CellPadding="2" CellSpacing="1" style="z-index: 1" Width="100%">
           
            <Columns>
<asp:TemplateField >
                                                        
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Check2" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:TemplateField>
             <asp:TemplateField HeaderText="AWB">
                    <ItemTemplate>
                        <asp:Label ID="lblAWBNo" runat="server" Text = '<%# Eval("AWBNumber") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="PCS">
                    <ItemTemplate>
                        <asp:Label ID="lblPCS" runat="server" Text = '<%# Eval("PiecesCount") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
                         
             <asp:TemplateField HeaderText="Wt">
             <ItemTemplate>
             <asp:Label ID="lblWt" runat="server" Text = '<%# Eval("GrossWeight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
                      
            </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
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
                                        
                                    <asp:Button ID="btnAddULDToManifest" runat="server" 
                                        Text="Add ULD Manifest" CssClass="button"  onclick="btnAddULDToManifest_Click" Visible="false"/>
                                        </td>
                               </tr>
                                <tr>
                               <td>
                                    <asp:Button ID="BtnAddtoManifest" runat="server"
                                        Text="Add Shipment"  CssClass="button" 
                                       onclick="BtnAddtoManifest_Click" />
                               </td>
                               <td>
                                    <asp:Button ID="btnSplitAssign" runat="server" 
                                        Text="Split &amp; Assign"  CssClass="button" 
                                        onclick="btnSplitAssign_Click" Visible="false" />
                                        </td>
                              </tr>
                              
                               
                              
                                   </table>
                                   </div>
                                   
                                   </div>
                           
                     </div>
                     
                                                    
                    <div id="Exportcolright" style="width:730px;">
                        <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
        </asp:Button>--%>
                      <h2>Driver Details</h2>
                                            
                                    <div class="divback" style="min-height:300px; margin-left:5px; padding-left:7px;" >
                                    
                                     <div>
                                   <table>
                                   <tr>
                                   <td>
                                    Name:
                                   </td>
                                   <td>
                                   <asp:TextBox ID="txtDriverName" runat="server" Width="80px"/>
                                   </td>
                                   <td>
                                    DL#:
                                   </td>
                                   <td>
                                   <asp:TextBox ID="txtDLNumber" runat="server" Width="80px"/>
                                   </td>
                                   <td>
                                    Phone:
                                   </td>
                                   <td>
                                   <asp:TextBox ID="txtphone" runat="server" Width="80px"/>
                                   </td>
                                   <td>
                                    Vehicle No:
                                   </td>
                                   <td>
                                   <asp:TextBox ID="txtVehicleNo" runat="server" Width="80px"/>
                                   </td>
                                   </tr>
                                   <tr>
                                   <td>
                                    Dock# :
                                   </td>
                                   <td>
                                   <asp:TextBox ID="txtDockNo" runat="server" Width="80px"/>
                                   </td>
                                   </tr>
                                   </table>
                                     </div> 
                                      
                                     <div style="margin-top:25px">
                                         <asp:Panel ID="Pnlgrd" runat="server" ScrollBars="Auto" Height="200px" 
                                             style="margin-top:20px"
                                             BorderStyle="Solid" BorderWidth="1px" Width="650px">
                                             <asp:GridView ID="gdvULDDetails"  
                              runat="server" CellPadding="3" 
                                            CellSpacing="2" 
    AutoGenerateColumns="False" style="z-index: 1" onselectedindexchanged="gdvULDDetails_SelectedIndexChanged">
                                                 <%-- onselectedindexchanged="gdvULDDetails_SelectedIndexChanged" --%>
                                                 <Columns>
<asp:TemplateField >
                                                        
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Check1" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:TemplateField>
             <asp:TemplateField HeaderText="AWB" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAWBNo" runat="server" Text = '<%# Eval("AWBno") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="PCS" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblPCS" runat="server" Text = '<%# Eval("PCS") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
                         
             <asp:TemplateField HeaderText="Wt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblWt" runat="server" Text = '<%# Eval("WGT") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
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
                                        Text="Save" Visible="true" />
                                        
                                    <asp:Button ID="btnUnassign" runat="server" CssClass="button" 
                                        Text="Un Assign" Enabled="true" onclick="btnUnassign_Click" />
                                
                                    
                                <asp:Button ID="btnPrintPass" runat="server" Text="Print Gate Pass" 
                        CssClass="button" Enabled="true"/>
                                     
                                     </div>
                                                                          
                                     
                                      </div>
                                    </div>
                           
                                          
                                        
        </div>
        <br />
        <br />
        
        
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
<asp:TemplateField >
                                                        
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Check1" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:TemplateField>
             <asp:TemplateField HeaderText="AWB" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAWBNo" runat="server" Text = '<%# Eval("AWBno") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="PCS" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblPCS" runat="server" Text = '<%# Eval("PCS") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
                         
             <asp:TemplateField HeaderText="Wt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblWt" runat="server" Text = '<%# Eval("WGT") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
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
                    Width="399px" ID="grdRouting" 
                    >
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
                                <asp:DropDownList ID="ddlFltNum" runat="server" Width="90px" OnSelectedIndexChanged="txtFltNumber_TextChanged" AutoPostBack="true">
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
                <asp:DropDownList ID="ddlFlightCode" runat="server" Visible="false">
                </asp:DropDownList>
                <asp:TextBox ID="txtFlightID" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtfltdt" runat="server" Visible="false"></asp:TextBox>
                <asp:Label ID="lblDepAirport" runat="server" Visible="false"></asp:Label>
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
                        <asp:Button ID="btnOK" CssClass="button" runat="server" Text="Send Email" OnClick="btnOK_Click" />
                        </td>
                        <td>
                        <asp:Button ID="btnSitaUpload" CssClass="button" runat="server" Text="Send via SITA" OnClick="btnSitaUpload_Click" />
                        </td>
                        <td>                        
                        <asp:Button ID="btnFTPUpload" CssClass="button" runat="server" Text="FTP Upload" OnClick="btnFTPUpload_Click"/>
                        </td>
                        <td>
                        <input type="button" id="Button1" class="button" value="Cancel" onclick="HidePanelSplit();" />
                    </td>
                </tr>
            </table>
            </div>
		
</asp:Content>


