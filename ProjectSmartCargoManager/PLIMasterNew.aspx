<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PLIMasterNew.aspx.cs" Inherits="ProjectSmartCargoManager.PLIMasterNew" MasterPageFile ="~/SmartCargoMaster.Master"  %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
        function SelectAllgrdAddRate(CheckBoxControl) {
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if (document.forms[0].elements[i].name.indexOf('check') > -1) {
                    document.forms[0].elements[i].checked = CheckBoxControl.checked;
                }
            }
        }

        function GetFlightNumber() {
            var TxtClientObject = '<%= TXTFlightNumber.ClientID %>';
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=FlightNumber&TargetTXT=' + TxtClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }

        function GetAirLineCode() {
            var TxtClientObject = '<%= TXTAirLineCode.ClientID %>';
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=AirlineCode&TargetTXT=' + TxtClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }

        function GetIATAComCode() {
            var TxtClientObject = '<%= TXTIATAComCode.ClientID %>';
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=CommCode&TargetTXT=' + TxtClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }

        function GetAgentCode() {
            var TxtClientObject = '<%= TXTAgentCode.ClientID %>';
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=AgentCode&TargetTXT=' + TxtClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }

        function GetShipperCode() {
            var TxtClientObject = '<%= TXTShipperCode.ClientID %>';
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=Shipper&TargetTXT=' + TxtClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }

        function GetAirportDestCode() {
            var TXTDestClobject = '<%=TXTParamDest.ClientID %>';
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesORG&level=A&param=Origin&TargetTXT=' + TXTDestClobject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }

        function GetAirportORGCode() {
            var TXTORGClobject = '<%=TXTParamORG.ClientID %>';
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesORG&level=A&param=Origin&TargetTXT=' + TXTORGClobject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
         
        
        
        
</script>   

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
    
    <h1><img src="Images/txt_agent.png" />
    <br />
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
             ForeColor="Red"></asp:Label>
     </h1>
     
     <div class="botline">
     <table  style="width:50%">
        <tr>
         <td>
         <b>PLI ID</b>
         </td>
         <td>
             <asp:TextBox ID="txtDealid" runat="server" Enabled="true"></asp:TextBox>
             &nbsp;
             <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
                 ToolTip="List" onclick="btnList_Click"/>
             &nbsp;
             <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
                 ToolTip="Clear All" onclick="btnClear_Click"/>
         </td>
         <td>
           <%--<asp:Button ID="btnList" runat="server" Text="List" CssClass="button"/>--%>
           &nbsp;</td>
         
        </tr>
     </table> 
    </div>
     <div>
      <br /> <br /> 
      <h2>Agent Details</h2>
    <table width="60%">
     <tr>
      <td>
       <b>Agent Code</b>
      </td>
      <td>
          <asp:DropDownList ID="ddlAgentCode" runat="server" Width="95px" AutoPostBack ="true" 
              OnSelectedIndexChanged="ddlAgentCode_SelectedIndexChanged" >
          </asp:DropDownList>
      </td>
      
      <td>
        <b>Agent Name</b>
       </td>
      <td>
          <asp:TextBox ID="txtAgentName" runat="server" Width="180px"></asp:TextBox>
      </td>
      
       
      <td>
          
          &nbsp;</td>
     <%-- <td>
       <asp:Button ID="BtnList" runat="server" Text="List" CssClass="button" 
              onclick="BtnList_Click" />
       &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
              ToolTip="Clear All" onclick="btnClear_Click"/>
      </td>--%>
     </tr>
     </table> 
    </fieldset> 
    </div>
     <div>
     <div>
          <table width="100%">
    <tr>
     <td>
         <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">
             Performance Parameters</legend>  
         <table width="100%">
          <tr>
                <td>
               <b>From Date</b> 
                &nbsp;
                    <asp:TextBox ID="txtFromDt" runat="server" Width="90px" ></asp:TextBox>
                    <asp:CalendarExtender ID="CEFromDate" runat="server" Format="dd/MM/yyyy" 
                                        PopupButtonID="btnFromDate" PopupPosition="BottomLeft" 
                                        TargetControlID="txtFromDt">
                                    </asp:CalendarExtender>
                </td>
                <td style="width:auto">
               <b> To Date </b> 
                &nbsp;
                    <asp:TextBox ID="txtToDt" runat="server" Width="90px" ></asp:TextBox>
                    <asp:CalendarExtender ID="CEToDt" runat="server" Format="dd/MM/yyyy" 
                                        PopupButtonID="btnFromDate" PopupPosition="BottomLeft" 
                                        TargetControlID="txtToDt">
                    </asp:CalendarExtender>
                </td>
                
          </tr>
          <tr>
          
          <td>
          <br />
          <b>Target</b>
          </td>
            <td>
          <br />
          <br />
          </td>
          </tr>
             
          <tr>
            <td>
              <asp:RadioButton ID="rbTonnage" runat="server" Text = "Tonnage" 
                    GroupName = "TonType" oncheckedchanged="rbTonnage_CheckedChanged" Checked = "true" AutoPostBack = "true"/>
            </td>
            <td>
              <asp:RadioButton ID="rbStandard" runat="server" Text = "Standard" 
                    GroupName = "TonType" oncheckedchanged="rbStandard_CheckedChanged" Checked = "false" AutoPostBack = "true"/>
            </td>
          </tr>
          
          <tr>
          <td>
          
          <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" ID="grdTonnage" Width="100%" CssClass="grdrowfont" PageSize="4" AllowPaging="True" onselectedindexchanging="grdTonnage_SelectedIndexChanging"> 
          
          <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                        <Columns>
                        <asp:TemplateField>
                         <HeaderTemplate>
                            <asp:CheckBox ID="chk" runat="server" onclick="javascript:SelectAllgrdAddRate(this);"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="check" runat="server" />
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText = "Tonnage">
                        <ItemTemplate>
                            <%--<asp:DropDownList ID="ddlUNID" runat="server">
                            </asp:DropDownList>--%>
                            <asp:TextBox ID="txtTonnage" runat="server" Width="95px" Text='<%# Eval("Tonnage") %>'></asp:TextBox>
                          
                        <%--Text='<%# Eval("UNID") %>'--%>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRate" runat="server" Width="95px" Text='<%# Eval("Rate") %>'></asp:TextBox>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>      
                        
                      
                        </Columns>

                        <EditRowStyle CssClass="grdrowfont"></EditRowStyle>

                      
                     <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
     </td>
     
     <td valign=top style="width:150px;">
        <%--onclick="btnAdd_Click"--%>
             <asp:Button ID="btnAdd" runat="server" CssClass="button" 
                  Text="Add" onclick="btnAdd_Click" />
             &nbsp;
             <%--onclick="btnDelete_Click"--%>
             <asp:Button ID="btnDelete" runat="server" CssClass="button" 
                  Text="Delete" onclick="btnDelete_Click"/>
     </td>
     </tr>
    
     <tr>
        <td>
        <br />
        </td>
     </tr>
          <tr>
           <td>
              Origin Type &nbsp;
               <asp:DropDownList ID="ddlorigintype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlorigintype_SelectedIndexChanged"
                   >
                   <asp:ListItem Text="Select"></asp:ListItem>  
                <asp:ListItem Text="Airport"  Value="A"></asp:ListItem>
                                    <asp:ListItem Text="City" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="N"></asp:ListItem>
               </asp:DropDownList>
           </td>
           <td>
           <%--onselectedindexchanged="ddlorigintype_SelectedIndexChanged"--%>
               Origin &nbsp;
               
               <asp:DropDownList ID="ddlOrigin" runat="server">
               <asp:ListItem Selected="True" Text="Select"></asp:ListItem>  
               </asp:DropDownList>
           </td>
           <td>
         Destination Type &nbsp;
           <asp:DropDownList ID="ddldestinationType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddldestinationType_SelectedIndexChanged"
               >
               <asp:ListItem Text="Select" Selected="True"></asp:ListItem>    
                <asp:ListItem Text="Airport" Value="A"></asp:ListItem>
                <asp:ListItem Text="City" Value="C"></asp:ListItem>
                <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                <asp:ListItem Text="Country" Value="N"></asp:ListItem>
           </asp:DropDownList>
           </td>
       <td>
        Destination 
        &nbsp;
        <asp:DropDownList ID="ddlDestination" runat="server">
           <asp:ListItem Selected="True" Text="Select"></asp:ListItem>  
        </asp:DropDownList>
       </td>
       <td>
       Flight Number
       &nbsp;
       <asp:DropDownList ID="ddlFlightNumber" runat="server">
            <asp:ListItem Selected="True" Text="Select"></asp:ListItem> 
       </asp:DropDownList>      
       </td>
       
       <td>
       Commodity
       &nbsp;
       <asp:DropDownList ID="ddlCommodity" runat="server">
       </asp:DropDownList>
       </td>
   </tr>
  </table> 
         
          </fieldset>  
     </td>
     <td>
         &nbsp;</td>
    </tr>
   </table> 
     </div>
    </div>
     <div>
      <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">
          Incentives</legend>
       <table width="90%" cellpadding=6 cellspacing=6>
       <tr>
       <td>
       <b>Applicable On</b>
       </td>
       <td>
       </td>
       <td>
       </td>
       <td>
       </td>
       </tr>
       <tr>
       <td>
           <asp:RadioButton ID="rbEntireTonnage" runat="server" Text = "Entire Tonnage Achieved" GroupName = "RBApplicability" Checked = "true" />
       </td>
       <td>
           <asp:RadioButton ID="rbAdditionalTonTarget" runat="server" Text = "Addditional Tonnage To Target" GroupName = "RBApplicability" />&nbsp;
       </td>
       <td>
          <b> Rate/KG &nbsp;</b>
           <asp:TextBox ID="txtRateDecide" runat="server"></asp:TextBox>
       </td>
       <td style="width: 80px" align = "right">
            <b>  Spot Rate </b> 
       </td>
       <%--<td style="width: 50px">
            <asp:TextBox Visible ="false" ID="TextBox2" runat="server" Width="120px"></asp:TextBox>
       </td>--%>
       <td style="width: 150px">
            <asp:RadioButton ID="rbSpotExclude" runat="server" Text="Exclude" GroupName="SR" Checked = "true"/>&nbsp;&nbsp;
            <asp:RadioButton ID="rbSpotInclude" runat="server" Text="Include" GroupName="SR" />
       </td>
       </tr>
       </table>
       
       
       
       
       
      </fieldset> 
      </div>
      
      <div>
      <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">
          Exceptions</legend>
       <table style="width:550px">
                                        <tr>
                                            <td style="width: 50px">
                                               <b> Flight Number </b>
                                            </td>
                                            <td style="width: 50px">
                                                <asp:TextBox ID="TXTFlightNumber" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBFN" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetFlightNumber();return false;" />

                                            </td>
                                            <td style="width: 150px">
                                                <asp:RadioButton ID="RBExFN" runat="server" Text="Exclude" GroupName="FN" 
                                                    Checked="true" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncFN" runat="server" Text="Include" GroupName="FN" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50px">
                                               <b> Flight Carrier </b>
                                            </td>
                                            <td style="width: 50px">
                                                <asp:TextBox ID="TXTFlightCarrier" runat="server" Width="120px"></asp:TextBox>

                                            </td>
                                            <td style="width: 150px">
                                                <asp:RadioButton ID="RBExFC" runat="server" Text="Exclude" GroupName="FC" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncFC" runat="server" Text="Include" GroupName="FC" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50px">
                                              <b> Handling Code </b>
                                            </td>
                                            <td style="width: 50px">
                                                <asp:TextBox ID="TXTHandlingCode" runat="server" Width="120px"></asp:TextBox>

                                            </td>
                                            <td style="width: 150px">
                                                <asp:RadioButton ID="RBExHC" runat="server" Text="Exclude" GroupName="HC" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncHC" runat="server" Text="Include" GroupName="HC" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50px">
                                               <b>Airline Code</b> 
                                            </td>
                                            <td style="width: 50px">
                                                <asp:TextBox ID="TXTAirLineCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBAC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirLineCode();return false;" />

                                            </td>
                                            <td style="width: 150px">
                                                <asp:RadioButton ID="RBExAC" runat="server" Text="Exclude" GroupName="AC" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncAC" runat="server" Text="Include" GroupName="AC" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50px">
                                               <b>IATA Comm. Code</b> 
                                            </td>
                                            <td style="width: 50px">
                                                <asp:TextBox ID="TXTIATAComCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBCC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetIATAComCode();return false;" />

                                            </td>
                                            <td style="width: 150px">
                                                <asp:RadioButton ID="RBExCC" runat="server" Text="Exclude" GroupName="CC" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncCC" runat="server" Text="Include" GroupName="CC" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50px">
                                               <b>Agent Code</b> 
                                            </td>
                                            <td style="width: 50px">
                                                <asp:TextBox ID="TXTAgentCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBAD" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />

                                            </td>
                                            <td style="width: 150px">
                                                <asp:RadioButton ID="RBExAD" runat="server" Text="Exclude" GroupName="AD" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncAD" runat="server" Text="Include" GroupName="AD" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50px">
                                               <b>Shipper Code</b> 
                                            </td>
                                            <td style="width: 50px">
                                                <asp:TextBox ID="TXTShipperCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBSC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetShipperCode();return false;" />

                                            </td>
                                            <td style="width: 150px">
                                                <asp:RadioButton ID="RBExSC" runat="server" Text="Exclude" GroupName="SC" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncSC" runat="server" Text="Include" GroupName="SC" />

                                            </td>
                                        </tr>
                                          <tr>
                                            <td style="width: 50px">
                                               <b>Origin</b> 
                                            </td>
                                            <td style="width: 50px">
                                                <asp:TextBox ID="TXTParamORG" runat="server" Width="120px"></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirportORGCode();return false;" />

                                            </td>
                                            <td style="width: 150px">
                                                <asp:RadioButton ID="RBExORG" runat="server" Text="Exclude" GroupName="OR" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncORG" runat="server" Text="Include" GroupName="OR" />

                                            </td>
                                        </tr>
                                         <tr>
                                            <td style="width: 50px">
                                               <b>  Destination </b> 
                                            </td>
                                            <td style="width: 50px">
                                                <asp:TextBox ID="TXTParamDest" runat="server" Width="120px"></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                        ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirportDestCode();return false;" />

                                            </td>
                                            <td style="width: 150px">
                                                <asp:RadioButton ID="RBExDest" runat="server" Text="Exclude" GroupName="DT" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncDest" runat="server" Text="Include" GroupName="DT" />

                                            </td>
                                        </tr>
                                        
                                         <%--<tr>
                                            <td style="width: 50px">
                                                &nbsp;</td>
                                            <td style="width: 50px">
                                                <asp:TextBox Visible ="false" ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                                            </td>
                                            <td style="width: 150px">
                                                <asp:RadioButton ID="rbInclude" runat="server" Text="Exclude" GroupName="DT" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="rbExclude" runat="server" Text="Include" GroupName="DT" />
                                            </td>
                                        </tr>--%>
                                    </table>
      </fieldset> 
      </div>
     <div align="right">
           <%-- <asp:Button ID="btnNewBooking" runat="server" CssClass="button" Text="New Booking"
                PostBackUrl="~/ConBooking.aspx" /> onclick="btnSave_Click"  --%>
            <asp:Button ID="btnApplyCreateCredit" runat="server" CssClass="button" 
                Text="Apply and Create Credit Note"/>
            &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="button" 
                Text="Save" onclick="btnSave_Click" />
            &nbsp;    
            <asp:Button ID="btnEdit" runat="server" CssClass="button" 
                Text="Enable Edit" onclick="btnEdit_Click"/>
            &nbsp;<asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" PostBackUrl="~/Home.aspx" />
        </div>
     </div> 
 </asp:Content>