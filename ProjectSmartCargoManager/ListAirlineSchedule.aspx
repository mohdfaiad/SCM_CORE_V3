
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListAirlineSchedule.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.ListAirlineSchedule" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>

 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


 
    <style type="text/css">
        .style1
        {
        }
    </style>
 
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
    
     <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
    
     <div id="contentarea">
        <h1>
            <%--<img alt="" src="Images/flightshlist.png"  style="vertical-align:5"/>--%> 
            Flight Schedule List
            </h1>
       
          <div class="botline">
            <table width="100%">
                <tr>
                    <td>
                        Origin
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAutoSource" runat="server" Width="100px"></asp:DropDownList>                         
                    <td >
                        Destination</td>
                    <td >
                        <asp:DropDownList ID="ddlAutoDest" runat="server" Width="100px"></asp:DropDownList>                         
                    </td>
                    <td >
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Flight #&nbsp;
                    </td>
                    <td>
                    <%--<asp:DropDownList ID="ddlFlight" runat="server" 
                            onselectedindexchanged="ddlFlight_SelectedIndexChanged">
              </asp:DropDownList>--%>
              <asp:TextBox ID="txtFlightNo" runat="server" Width="100px"></asp:TextBox>
                    </td>
                     <asp:AutoCompleteExtender ID="AutoCompleteExtendertxtFlightNo1" 
                            runat="server"  ServiceMethod="GetFlightId" MinimumPrefixLength="2"  
                            Enabled="True" ServicePath="~/ListAirlineSchedule.aspx"  
               EnableCaching="true"  TargetControlID="txtFlightNo">
               </asp:AutoCompleteExtender>
                    
                    <td>
                        AirCraftType</td>
                    <td>
                        <asp:DropDownList ID="ddlAirCraftType" runat="server">
              </asp:DropDownList>
                        </td>
                    <td class="style1">
                        Status</td>
                    <td>
                          <asp:DropDownList ID="ddlStatus" runat="server">
                          <asp:ListItem Value ="All"></asp:ListItem>
                         <asp:ListItem Value ="ACTIVE"></asp:ListItem>
                         <asp:ListItem Value="CANCELLED"></asp:ListItem>
                         <asp:ListItem Value="DRAFT"></asp:ListItem>
                        </asp:DropDownList>
                        </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:DropDownList ID="ddlOrigin" runat="server" 
                            onselectedindexchanged="ddlOrigin_SelectedIndexChanged" 
                            AutoPostBack="True" Visible="False">
                        </asp:DropDownList>
                        </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        </td>
                </tr>
                <tr>
                    <td>Flight From*</td>
                    <td>
              <asp:TextBox ID="txtFlightFromdate" runat="server"  Width="100px" 
                            ToolTip="Please enter date format: dd/MM/yyyy"></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate" PopupButtonID="imgFromDate">
              </asp:CalendarExtender>
              <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                        </td>
                    <td >Flight To*</td>
                    <td >
                      
              <asp:TextBox ID="txtFlightToDate" runat="server" Width="100px" 
                          ToolTip="Please enter valid date format: dd/MM/yyyy" 
                 ></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightToDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightToDate" PopupButtonID="imgToDate">
              </asp:CalendarExtender>
              <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                    </td>
                    <td >
                        &nbsp;</td>
                    <td>
                        <asp:CheckBox ID="chkDomestic" runat="server" AutoPostBack="True" 
                            Checked="True" Text="Domestic" ValidationGroup="A" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkInternational" runat="server" Checked="True" 
                            Text="International" ValidationGroup="A" />
                    </td>
                    <td colspan="2">
                        <asp:Button ID="Button1" runat="server" onclick="Button1_Click"  CssClass="button"  Visible="false"
                            Text="Datewise List" />
                    </td>
                    
                    <td>
                        
                        <asp:DropDownList ID="ddlDestination" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlDestination_SelectedIndexChanged" 
                            Visible="False">
                        </asp:DropDownList>
                        
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                <td>
                        <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click" />
                                            
                                            <asp:Button ID="btnclear" runat="server" Text="Clear" 
                            CssClass="button" onclick="btnclear_Click" 
                                             />   
                                                                  
                          <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" OnClick="btnExport_Click" />
                                            
                    </td>
                </tr>
            </table>
        </div>
         <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
            Font-Size="Large" ForeColor="Red"></asp:Label>
            <asp:Panel ID="pnlMultiple" runat="server" Visible="false">
            <div class="ltfloat" style="width:100%;">
              <asp:GridView ID="grdFlight" runat="server"  Width="100%"
         AutoGenerateColumns="False" PageSize="10" AllowPaging="True" 
         onrowcommand="grdFlight_RowCommand" onselectedindexchanging="grdFlight_SelectedIndexChanging" 
         onpageindexchanging="grdFlight_PageIndexChanging"
           >
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
                <%-- <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="SrNo" Visible = "false" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblSrNo" runat="server" Width="50px" ></asp:Label ><%--Text='<%# Eval("ScheduleID") %>'--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" Width="50px" />
                </asp:TemplateField>
                
                <%-- <asp:ButtonField HeaderText="RouteID" DataTextField="RoueID"/>--%>
                
                 <asp:TemplateField HeaderText="Flight#" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:LinkButton ID="lblFlightID" runat="server" Width="50px" CommandName="FlightID" ></asp:LinkButton > <%--Text='<%# Eval("FlightID") %>'--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="RouteID" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblRouteID" runat="server" Width="50px"  ></asp:Label ><%--Text='<%# Eval("RouteID") %>'--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                
               
                
             
                
                <asp:TemplateField HeaderText="Source" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblSource" runat="server" Width="50px"  ></asp:Label > <%--Text='<%# Eval("Source") %>' --%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Dest" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblDest" runat="server" Width="50px" ></asp:Label > <%--Text='<%# Eval("Dest") %>'--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                   <asp:TemplateField HeaderText="From Date" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblFromDate" runat="server" Width="50px"  ></asp:Label ><%-- Text='<%# Eval("FromtDt") %>'--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="ToDate" HeaderStyle-Wrap="true" >
                    <ItemTemplate>
                          <asp:Label ID="lblToDate" runat="server" Width="50px"  ></asp:Label ><%-- Text='<%# Eval("ToDt") %>' --%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
           <%--     
                  <asp:TemplateField HeaderText="Frequency" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=40%>
                   
                    <ItemTemplate>
                        <asp:CheckBox ID="chkMon" runat="server" Checked="True" Text="Mo" />
                        <asp:CheckBox ID="chkTues" runat="server" Checked="True" Text="Tu" />
                        <asp:CheckBox ID="chkwed" runat="server" Checked="True" Text="We" />
                        <asp:CheckBox ID="chkThur" runat="server" Checked="True" 
                            Text="Th" />
                        <asp:CheckBox ID="chkFri" runat="server" Checked="True" Text="Fr" />
                        <asp:CheckBox ID="chkSat" runat="server" Checked="True" 
                            Text="Sa" />
                        <asp:CheckBox ID="chkSun" runat="server" Checked="True" Text="Su" />
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>--%>
                              
                
</Columns> 
                <EditRowStyle CssClass="grdrowfont" ></EditRowStyle>
                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
</asp:GridView>
            
            </div>
                  <div>
                  <asp:Button ID="btnEdit" runat="server" CssClass="button" Enabled="False" 
                     onclick="btnEdit_Click" Text="Edit" />
                  </div> 
            </asp:Panel>
            
            
      <asp:Panel ID="pnlDestDetails" runat="server" Visible="False" >
     <div class="botline" visible="false">
        <table width="80%">
          <tr>
          <td >
        
              Origin</td>
              <td>
                  <asp:DropDownList ID="ddlOrigin1" runat="server" AutoPostBack="True" 
                      onselectedindexchanged="ddlOrigin_SelectedIndexChanged">
                  </asp:DropDownList>
                  <asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox>
              </td>
              <td>
                  Destination</td>
                  <td>
                      <asp:DropDownList ID="ddlDestination0" runat="server" 
                          onselectedindexchanged="ddlDestination2_SelectedIndexChanged">
                      </asp:DropDownList>
                  </td>
<td>
    &nbsp;</td>
<td>&nbsp;</td>
          </tr>
         <tr>
          <td >
              AirCraftType  *</td>
          <td>
                       <asp:DropDownList ID="ddlLoadAirCraftType" runat="server" AutoPostBack="True" 
                           onselectedindexchanged="ddlLoadAirCraftType_SelectedIndexChanged">
              </asp:DropDownList></td>
          <td>
              Free Sale (kg)&nbsp; *</td>
          <td>
              <asp:TextBox ID="txtCargoCapacity" runat="server" Height="19px" MaxLength="7" width="114px" 
                 ></asp:TextBox>
          </td>
             <td>
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
          </tr>
                     
         <tr>
          <td>
              From date *</td>
          <td>
              <asp:TextBox ID="txtFromdate" runat="server" Height="19px" Width="114px" 
                  ontextchanged="txtFromdate_TextChanged" 
                  ToolTip="Please enter date format: dd/MM/yyyy"></asp:TextBox>
              <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFromdate">
              </asp:CalendarExtender>
             </td>
          <td>
              To Date  *</td>
          <td>
              <asp:TextBox ID="txtToDate" runat="server" Width="114px" AutoPostBack="True" 
                  ToolTip="Please enter date format: dd/MM/yyyy" 
                 ></asp:TextBox>
              <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtToDate">
              </asp:CalendarExtender>
             </td>
             <td>
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
          </tr>
                     
            <tr>
                <td>
                    <asp:RadioButton ID="chkDomestic0" runat="server" AutoPostBack="True" 
                        Checked="True" GroupName="B" oncheckedchanged="RadioButton1_CheckedChanged" 
                        Text="Domestic" ValidationGroup="B" />
                </td>
                <td colspan="2">
                    <asp:RadioButton ID="chkInternational0" runat="server" AutoPostBack="True" 
                        GroupName="B" oncheckedchanged="chkInternational0_CheckedChanged" 
                        Text="International" ValidationGroup="B" />
                </td>
                <td>
                    <asp:TextBox ID="txtSource1" runat="server" Enabled="False" height="19px" 
                        ontextchanged="txtSource1_TextChanged" width="114px" Visible="False"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtDestination" runat="server" Enabled="False" height="19px" 
                        width="114px" Visible="False"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
                     
        </table>
        
        
    </div>
    </asp:Panel> 
     
    <asp:Panel ID="pnlSchedule" runat ="server" Visible="False" >
   <h2>Route Details
       
            </h2> 
          
          <div>
    <table width="100%" visible="false">
    <tr>
    <td width="40%">
    
    </td>
    <td width="40%">
    
    </td>
    <td width="10%">
      <asp:Button ID="btnAddNewRow" runat="server" Text="Add New" CssClass="button"  Visible="false"
            onclick="btnAddNewRow_Click" />
    </td>
    <td width="10%">
      <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button" 
            onclick="btnDelete_Click" Visible="False"  />
    </td>
    </tr>
   
    </table>
    </div>
          
          
          
           <div>
     <asp:Panel ID="grid" runat="server" ScrollBars="Horizontal" >            
           
    <asp:GridView ID="grdScheduleinfo" runat="server" AutoGenerateColumns="False" 
                                    ShowFooter="True"   Width="80%" 
                   onpageindexchanging="grdScheduleinfo_PageIndexChanging" 
                   onselectedindexchanging="grdScheduleinfo_SelectedIndexChanging" 
                   PageSize="10" AllowPaging="True" >
            <Columns>
            
              <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CHK" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
            
               
                <asp:TemplateField HeaderText="Flight# *" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblFlight" runat="server">
                        </asp:Label>
                         <%--<asp:TextBox  ID="txtSource" runat="server" Width="70px" ></asp:TextBox>--%>
                        <%--<asp:TextBox ID="txtbankname" runat="server"></asp:TextBox>--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
                <asp:TemplateField HeaderText="From *" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlFromOrigin" runat="server" Width="55px">
                        </asp:DropDownList>
                         <%--<asp:TextBox  ID="txtSource" runat="server" Width="70px" ></asp:TextBox>--%>
                        <%--<asp:TextBox ID="txtbankname" runat="server"></asp:TextBox>--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="To *" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                         <asp:DropDownList ID="ddlToDest" runat="server" Width="55px">
                        </asp:DropDownList>
                        
                         <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                    
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
   
    <asp:TemplateField HeaderText="From Date" HeaderStyle-Wrap="false"  Visible="true">
                    <ItemTemplate>
                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                        <asp:Label ID="lblFromDate" runat="server" Text=""></asp:Label>
                         <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                    
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                
                 <asp:TemplateField HeaderText="To Date" HeaderStyle-Wrap="false" Visible="true" >
                    <ItemTemplate>
                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                           <asp:Label ID="lblToDate" runat="server" Text=""></asp:Label>
                        
                         <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                    
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
   
                <asp:TemplateField HeaderText="Dept Time  *" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=20%>
                    <ItemTemplate>
                    
                    Day <asp:TextBox ID="txtDeptDay" runat="server" Width="8px"  DataTextField=""></asp:TextBox>
                         <asp:NumericUpDownExtender ID="NumericUpDownExtender_DeptDay" runat="server" 
                TargetControlID="txtDeptDay"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "1"  Maximum = "2" />
                    
                       Hr <asp:TextBox ID="txtDeptTimeHr" runat="server" Width="8px"  DataTextField=""></asp:TextBox>
                         <asp:NumericUpDownExtender ID="txtDeptTimeHr_NumericUpDownExtender" runat="server" 
                TargetControlID="txtDeptTimeHr"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "23" /> :
                       
                         Min <asp:TextBox ID="txtDeptTimeMin" runat="server" Width="8px"  DataTextField=""></asp:TextBox>
                             <asp:NumericUpDownExtender ID="txtDeptTimeMin_NumericUpDownExtender1" runat="server" 
                TargetControlID="txtDeptTimeMin"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "1"  Maximum = "60" /> 
      
      
                    </ItemTemplate>
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Arrival Time  *" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=20%>
                    <ItemTemplate >  
                    
                      Day <asp:TextBox ID="txtArrivalDay" runat="server" Width="8px"  DataTextField=""></asp:TextBox>
                         <asp:NumericUpDownExtender ID="NumericUpDownExtender_ArrivalDay" runat="server" 
                TargetControlID="txtArrivalDay"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "-1"  Maximum = "2" />
                    
                    
                        Hr<asp:TextBox  ID="txtArrivaltimeHr" runat="server" Width="8px" ></asp:TextBox>
                              <asp:NumericUpDownExtender ID="txtArrivaltimeHr_NumericUpDownExtender" runat="server" 
                TargetControlID="txtArrivaltimeHr"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "23" /> :  
     
                       Min<asp:TextBox  ID="txtArrivalTimeMin" runat="server" Width="8px" ></asp:TextBox>
                      <asp:NumericUpDownExtender ID="txtArrivalTimeMin_NumericUpDownExtender1" runat="server" 
                TargetControlID="txtArrivalTimeMin"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "1"  Maximum = "60" /> 
      
      
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbankguranteeamt"  ErrorMessage="Please Enter Numeric Value"  ValidationExpression="^\d+$" ValidationGroup="check">
                        </asp:RegularExpressionValidator>--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Frequency  *" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=40%>
                   
                    <ItemTemplate>
                        <asp:CheckBox ID="chkMon" runat="server" Checked="True" Text="Mo" />
                        <asp:CheckBox ID="chkTues" runat="server" Checked="True" Text="Tu" />
                        <asp:CheckBox ID="chkwed" runat="server" Checked="True" Text="We" />
                        <asp:CheckBox ID="chkThur" runat="server" Checked="True" 
                            Text="Th" />
                        <asp:CheckBox ID="chkFri" runat="server" Checked="True" Text="Fr" />
                        <asp:CheckBox ID="chkSat" runat="server" Checked="True" 
                            Text="Sa" />
                        <asp:CheckBox ID="chkSun" runat="server" Checked="True" Text="Su" />
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
                  <asp:TemplateField HeaderText="AirCraft Type*" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=20%>
                   
                    <ItemTemplate>
                        
                    <asp:DropDownList ID="ddlAirCraft" runat="server" 
                     AutoPostBack="True" OnSelectedIndexChanged="showCapacityInGrid"  >
              </asp:DropDownList>
                   
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Tail No" ItemStyle-Width="30%">
                     <ItemTemplate>
                            <asp:DropDownList ID="ddlTailNo" runat="server">     
                            <asp:ListItem Text="Select" Value="0">   </asp:ListItem>
                            </asp:DropDownList>
                      </ItemTemplate>
                  <FooterStyle HorizontalAlign="Right" />
                  <HeaderStyle Wrap="true" />
                  <ItemStyle Wrap="false" />
                </asp:TemplateField>
               
                  <asp:TemplateField HeaderText="Capacity(Kg)*" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=20%>
                    <ItemTemplate>
                        <asp:TextBox ID="txtCapacity" Width="50px" Text="" MaxLength="6" runat="server" ></asp:TextBox>
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
               
                <asp:TemplateField HeaderText="Status *" HeaderStyle-Wrap="false">
                 <FooterTemplate>
<%--                         <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="button"  OnClick ="Addrow" />
--%>                    </FooterTemplate>
                    <ItemTemplate>
                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                         <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="58px">
                         <asp:ListItem Value ="ACTIVE"></asp:ListItem>
                         <asp:ListItem Value="CANCELLED"></asp:ListItem>
                         <asp:ListItem Value="DRAFT"></asp:ListItem>
                        </asp:DropDownList>
                         <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                    
                    </ItemTemplate>
                     <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
   
               
           </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>
     </asp:Panel>  
     
    </div>
    
    <div>
    
    </div>
    
    
    </asp:Panel>
    <div id="fotbut">
    <asp:Panel ID="pnlUpdate" runat="server" Visible="False" >
        <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="button" 
            onclick="btnSave_Click"  />
            </asp:Panel>
    </div>
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
    
    </ContentTemplate>
    <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    </asp:Content>