<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPLIMaster.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmPLIMaster" %>

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
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=AirlineDesigCode&TargetTXT=' + TxtClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
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

        function GetProductType() {
            var TXTProdClobject = '<%=TXTParamProdType.ClientID %>';
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesProd&Type=A&param=ProductType&TargetTXT=' + TXTProdClobject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        
   </script>   

    
    <style type="text/css">
        .style1
        {
            width: 258px;
        }
        .style3
        {
            width: 201px;
        }
        .style4
        {
            width: 304px;
        }
        .style5
        {
            width: 304px;
            font-weight: bold;
        }
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
    
    <h1><img src="Images/txt_performance.png" /><br />
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
             ForeColor="Red"></asp:Label>
     </h1>
     
     <div class="botline">
     <table  style="width:50%">
        <tr>
         <td>
             PLI ID
         </td>
         <td>
             <asp:TextBox ID="txtPLIid" runat="server" Enabled="False"></asp:TextBox>
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
      
    <table >
     <tr>
      
      <td>
          Agent Code :</td>
      
      <td>
          <asp:DropDownList ID="ddlAgentCode" runat="server">
          </asp:DropDownList>
      </td>
      
      <td>
          Effective&nbsp; Date :
      </td>
      <td>
          
          &nbsp;<asp:TextBox ID="txtaplicablefrom" runat="server" Width="70px"></asp:TextBox>
           <asp:CalendarExtender ID="txtaplicablefrom_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtaplicablefrom" Format="dd-MM-yyyy" >
          </asp:CalendarExtender>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtaplicablefrom" ErrorMessage="*"></asp:RequiredFieldValidator>
         
      </td>
      
       <td>
           Expiration&nbsp; Date :</td>
      <td>
          
          <asp:TextBox ID="txtApplicableto" runat="server" Width="70px"></asp:TextBox>
          <asp:CalendarExtender ID="txtApplicableto_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtApplicableto" Format="dd-MM-yyyy" >
          </asp:CalendarExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtApplicableto" ErrorMessage="*"></asp:RequiredFieldValidator>

      </td>
         <td>
             Currency :</td>
         <td>
             <asp:DropDownList ID="ddlCurrency" runat="server">
             </asp:DropDownList>
         </td>
     <%-- <td>
       <asp:Button ID="BtnList" runat="server" Text="List" CssClass="button" 
              onclick="BtnList_Click" />
       &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
              ToolTip="Clear All" onclick="btnClear_Click"/>
      </td>--%>
      </tr>
     </table> 
    
    </div>
     <div  style="overflow:auto">   
     <table width="60%">
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
              <asp:RadioButton ID="rbTonnage" runat="server" Text = "Freight" 
                    GroupName = "TonType" Checked = "true" AutoPostBack = "true" 
                    oncheckedchanged="rbTonnage_CheckedChanged"/>
              <asp:RadioButton ID="rbStandard" runat="server" Text = "Revenue" 
                    GroupName = "TonType" Checked = "false" AutoPostBack = "true" 
                    oncheckedchanged="rbStandard_CheckedChanged"/>
              <asp:RadioButton ID="rbBenchmark" runat="server" Text = "Benchmark" 
                    GroupName = "TonType" Checked = "false" AutoPostBack = "true" oncheckedchanged="rbBenchmark_CheckedChanged" 
                    />
            </td>
          </tr>
     <tr>
     
     <td>
     <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" 
                          ID="grdTonnage"                                    
                                      Width="100%" CssClass="grdrowfont" PageSize="4" 
             AllowPaging="True" > 
                                  <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                        <Columns>
                                           
                        
                        <asp:TemplateField HeaderText="Freight"><ItemTemplate>
                            <%--<asp:DropDownList ID="ddlUNID" runat="server">
                            </asp:DropDownList>--%>
                            
                             <asp:TextBox ID="txtTonnage" runat="server" Width="95px"   ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator81" runat="server" ControlToValidate="txtTonnage" ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator41" ValidationExpression="^[0-9]+$" runat="server" ControlToValidate="txtTonnage" ErrorMessage="Numeric Digits Only"></asp:RegularExpressionValidator>
                   <%--          Text='<%# Eval("UNID") %>'--%>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Commission(%)">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRate" runat="server" Width="95px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="txtRate" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ValidationExpression="^[0-9]+$" runat="server" ControlToValidate="txtRate" ErrorMessage="Numeric Digits Only"></asp:RegularExpressionValidator>
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
        
             <asp:Button ID="btnAdd0" runat="server" CssClass="button" 
                  Text="Add" onclick="btnAdd0_Click" />
             &nbsp;
             <asp:Button ID="btnDelete0" runat="server" CssClass="button" 
                  Text="Delete" onclick="btnDelete0_Click" />
         
         </td>
     
     </tr>
     </table> 
    </div>
    
    <div>
      <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">
          PLI Filters</legend>
       <table style="width:650px">
                                        <tr>
                                            <td class="style4">
                                               <b> Flight Number </b>
                                            </td>
                                            <td class="style1">
                                                <asp:TextBox ID="TXTFlightNumber" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBFN" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetFlightNumber();return false;" />

                                            </td>
                                            <td class="style3">
                                                <asp:RadioButton ID="RBExFN" runat="server" Text="Exclude" GroupName="FN" 
                                                    Checked="true" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncFN" runat="server" Text="Include" GroupName="FN" />

                                            </td>
                                        </tr>
                                        <!--<tr>
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
                                        </tr>-->
                                        <tr>
                                            <td class="style4">
                                               <b>Airline Code</b> 
                                            </td>
                                            <td class="style1">
                                                <asp:TextBox ID="TXTAirLineCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBAC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirLineCode();return false;" />

                                            </td>
                                            <td class="style3">
                                                <asp:RadioButton ID="RBExAC" runat="server" Text="Exclude" GroupName="AC" 
                                                    Checked="True" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncAC" runat="server" Text="Include" GroupName="AC" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style5">
                                                Prime Flight</td>
                                            <td class="style1">
                                                &nbsp;</td>
                                            <td class="style3">
                                                     <asp:RadioButton ID="RBExPF" runat="server" Text="Exclude" GroupName="PF" 
                                                    Checked="True" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncPF" runat="server" Text="Include" GroupName="PF" /></td>
                                        </tr>
                                        <tr>
                                            <td class="style4">
                                               <b>IATA Comm. Code</b> 
                                            </td>
                                            <td class="style1">
                                                <asp:TextBox ID="TXTIATAComCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBCC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetIATAComCode();return false;" />

                                            </td>
                                            <td class="style3">
                                                <asp:RadioButton ID="RBExCC" runat="server" Text="Exclude" GroupName="CC" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncCC" runat="server" Text="Include" GroupName="CC" />

                                            </td>
                                        </tr>
                                        
                                        <!--<tr>
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
                                        </tr>-->
                                          <tr>
                                            <td class="style4">
                                               <b>Origin</b> 
                                            </td>
                                            <td class="style1">
                                                <asp:TextBox ID="TXTParamORG" runat="server" Width="120px"></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirportORGCode();return false;" />

                                            </td>
                                            <td class="style3">
                                                <asp:RadioButton ID="RBExORG" runat="server" Text="Exclude" GroupName="OR" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncORG" runat="server" Text="Include" GroupName="OR" />

                                            </td>
                                        </tr>
                                         <tr>
                                            <td class="style4">
                                               <b>  Destination </b> 
                                            </td>
                                            <td class="style1">
                                                <asp:TextBox ID="TXTParamDest" runat="server" Width="120px"></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                        ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirportDestCode();return false;" />

                                            </td>
                                            <td class="style3">
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
                                             <tr>
                                            <td style="font-weight: 700;" class="style4">
                                                Product Type</td>
                                            <td class="style1">
                                                <asp:TextBox ID="TXTParamProdType" runat="server" Width="120px"></asp:TextBox>
                                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                        ImageAlign="AbsMiddle" OnClientClick="javascript:GetProductType();return false;" />

                                                 </td>
                                            <td class="style3">
                                                <asp:RadioButton ID="RBExProdType" runat="server" Text="Exclude" GroupName="Prod" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncProdType" runat="server" Text="Include" GroupName="Prod" />

                                            </td>
                                        </tr>
                                        
                                         <tr>
                                            <td style="font-weight: 700;" class="style4">
                                                Heavy Cargo</td>
                                            <td class="style1">
                                                &nbsp;</td>
                                            <td class="style3">
                                                <asp:RadioButton ID="RBExHeavyCargo" runat="server" Checked="True" GroupName="Heavy" 
                                                    Text="Exclude" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncHeavyCargo" runat="server" GroupName="Heavy" Text="Include" />
                                             </td>
                                        </tr>
                                        
                                         <tr visible="false">
                                            <td class="style4" visible="false">
                                               <%--<b>Agent Code</b> --%>
                                            </td>
                                            <td class="style1">
                                                <asp:TextBox ID="TXTAgentCode" runat="server" Width="120px" Visible="false"></asp:TextBox>

                                                <asp:ImageButton ID="IBAD" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" Visible="false" />

                                            </td>
                                            <td class="style3">
                                                <asp:RadioButton ID="RBExAD" runat="server" Text="Exclude" GroupName="AD" 
                                                    Checked="True" Visible="false" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncAD" runat="server" Text="Include" GroupName="AD" Visible="false" />

                                            </td>
                                        </tr>
                                        
                                         </table>
      </fieldset> 
      </div>
      
      
     <div align="right">
           <%-- <asp:Button ID="btnNewBooking" runat="server" CssClass="button" Text="New Booking"
                PostBackUrl="~/ConBooking.aspx" />--%>
            &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="button" 
                Text="Save" onclick="btnSave_Click" />
            &nbsp;<asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" PostBackUrl="~/Home.aspx" />
        </div>
     </div> 
     <div style="visibility:hidden">
      <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Parameters</legend>
       <table width="90%" cellpadding=6 cellspacing=6>
       
       <tr><td>
       <table width="80%" cellpadding=6 cellspacing=3>
       <tr><td>Slab Of Rate</td><td><asp:CheckBox ID="chkM" runat="server" Text="M"  />&nbsp; &nbsp;&nbsp;<asp:CheckBox ID="chkN" runat="server" Text="N"  /> &nbsp; &nbsp;&nbsp;<asp:CheckBox ID="chkA" runat="server" Text="+45"  /> &nbsp; &nbsp;&nbsp;<asp:CheckBox ID="chkB" runat="server" Text="+100"  /></td>
       </tr>
       <tr><td>Spot Rate Tonnage</td><td><asp:RadioButton ID="rdbIncluded" runat="server" Text="Included"  GroupName="A" />&nbsp;&nbsp;&nbsp;
       <asp:RadioButton ID="rbExcluded" runat="server" Text="Excluded" GroupName="A"  />
       </td></tr>
       <tr><td>Commisionable</td><td><asp:RadioButton ID="rdbyesCommisionable" runat="server" Text="Yes" GroupName="B"   />&nbsp;&nbsp;&nbsp;
       <asp:RadioButton ID="rdbNoCommissionable" runat="server" Text="No" GroupName="B"/>
       </td></tr>
       </table>
       </td>
       <td valign="top">
       <table width="80%" cellpadding=6 cellspacing=3><tr><td>
           Incentive Amount(Tonnage)</td><td>
             <asp:TextBox ID="txtkickbackamount" runat="server" Width="92px" ></asp:TextBox>
                <asp:RequiredFieldValidator Enabled="false" ID="RequiredFieldValidator3" ControlToValidate="txtkickbackamount" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                         <asp:RegularExpressionValidator Enabled="false" ValidationExpression="^[0-9]+$" ID="RegularExpressionValidator3" ErrorMessage="Enter Valid Numeric Format!" ControlToValidate="txtkickbackamount" runat="server" ></asp:RegularExpressionValidator>
             
               </td></tr>
               <tr><td>Threshold</td><td><asp:TextBox ID="txtthrshold" runat="server" Width="92px"></asp:TextBox>
                   <asp:RequiredFieldValidator Enabled="false" ID="RequiredFieldValidator1" ControlToValidate="txtthrshold" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator Enabled="false" ValidationExpression="^[0-9]+$" ID="RegularExpressionValidator1" ErrorMessage="Enter Valid Numeric Format!" ControlToValidate="txtthrshold" runat="server" ></asp:RegularExpressionValidator>
       </td></tr>
               <tr><td>Incentive Amount(AWB)</td><td>
             <asp:TextBox ID="txtFlatAmount" runat="server" Width="92px" ></asp:TextBox>
                   <asp:RequiredFieldValidator Enabled="false" ID="RequiredFieldValidator2" ControlToValidate="txtFlatAmount" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                         <asp:RegularExpressionValidator Enabled="false" ValidationExpression="^[0-9]+$" ID="RegularExpressionValidator2" ErrorMessage="Enter Valid Numeric Format!" ControlToValidate="txtFlatAmount" runat="server" ></asp:RegularExpressionValidator>
                   </td></tr>
               </table>
       </td>
       </tr>
       </table>
       
       
       
       
       
      </fieldset> 
      </div>
     <div style="visibility:hidden;">
     <div >
          <table width="100%">
    <tr>
     <td>
         <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Tonnage</legend>  
         <table width="100%">
          <tr>
           <td>
             Origin Type
           </td>
           <td>
               <asp:DropDownList ID="ddlorigintype" runat="server" AutoPostBack="true" 
                   onselectedindexchanged="ddlorigintype_SelectedIndexChanged">
                   <asp:ListItem Text="Select"></asp:ListItem>  
                <asp:ListItem Text="Airport"  Value="A"></asp:ListItem>
                                    <asp:ListItem Text="City" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="N"></asp:ListItem>
               </asp:DropDownList>
           </td>
           <td>
            Origin
           </td>
           <td>
              <%-- <asp:TextBox ID="txtorigin" runat="server"></asp:TextBox>
               <asp:AutoCompleteExtender ID="txtorigin_AutoCompleteExtender" runat="server" 
                   TargetControlID="txtorigin" ServicePath="~/Home.aspx" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True"  >
               </asp:AutoCompleteExtender>--%>
               
               <asp:DropDownList ID="ddlOrigin" runat="server">
               <asp:ListItem Selected="True" Text="Select"></asp:ListItem>  
               </asp:DropDownList>
           </td>
            <td>
         Destination Type
        </td>
       <td>
           <asp:DropDownList ID="ddldestinationType" runat="server" AutoPostBack="true" 
               onselectedindexchanged="ddldestinationType_SelectedIndexChanged" >
               <asp:ListItem Text="Select" Selected="True"></asp:ListItem>    
                <asp:ListItem Text="Airport" Value="A"></asp:ListItem>
                <asp:ListItem Text="City" Value="C"></asp:ListItem>
                <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                <asp:ListItem Text="Country" Value="N"></asp:ListItem>
           </asp:DropDownList>
       </td>
       <td>
        Destination
       </td>
       <td>
          <%-- <asp:TextBox ID="txtdestination" runat="server"></asp:TextBox>--%>
           <asp:DropDownList ID="ddlDestination" runat="server">
           <asp:ListItem Selected="True" Text="Select"></asp:ListItem>  
           </asp:DropDownList>
       </td>
       <td>
       Flight Number
       </td>
       <td>
       <asp:DropDownList ID="ddlFlightNumber" runat="server">
       <asp:ListItem Selected="True" Text="Select"></asp:ListItem> 
       </asp:DropDownList>
            
       </td>
       
       <td>
       Commodity
       </td>
       <td>
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
 </asp:Content>
