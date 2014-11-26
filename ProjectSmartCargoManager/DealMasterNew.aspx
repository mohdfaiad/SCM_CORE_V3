<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealMasterNew.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.DealMasterNew" %>

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
        function GetAirportORGMetro() {
            var TXTMetroClobject = '<%=TXTParamMinMetro.ClientID %>';
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesMetro&Metro=A&param=Metro&TargetTXT=' + TXTMetroClobject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }

        function GetAirportORGNonMetro() {
            var TXTNonMetroClobject = '<%=TXTParamMinNonMetro.ClientID %>';
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesNonMetro&NonMetro=A&param=NonMetro&TargetTXT=' + TXTNonMetroClobject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        
   </script>   

    <script type="text/javascript">
    function displayValidationMetro() {
        if (typeof (Page_Validators) == "undefined") return;
        var RequiredName = document.getElementById('<%= MinMetro.ClientID%>');
        ValidatorValidate(RequiredName);
        if (!RequiredName.isvalid) {
            alert("Please enter valid numeric range!");
        }
        else {

        }
    }

    function displayValidationNonMetro() {
        if (typeof (Page_Validators) == "undefined") return;
        var RequiredName = document.getElementById('<%= MinNonMetro.ClientID%>');
        ValidatorValidate(RequiredName);
        if (!RequiredName.isvalid) {
            alert("Please enter valid numeric range!");
        }
        else {

        }
    }

    function DealType(ddl) {
        var Selection = ddl.options[ddl.selectedIndex].text;
        var ImgButton = document.getElementById('<%= ImageButton1.ClientID%>');
        var Textbox = document.getElementById('<%= TXTParamORG.ClientID%>');
        var exclude = document.getElementById('<%= RBExORG.ClientID%>');
        var include = document.getElementById('<%= RBIncORG.ClientID%>');
        if (Selection == "Local") {
            ImgButton.setAttribute('disabled', true);
            Textbox.setAttribute('disabled', true);
            exclude.setAttribute('disabled', true);
            include.setAttribute('disabled', true);

        }
        else
            if (Selection == "National") {
                ImgButton.disabled = false;
                Textbox.disabled = false;
                exclude.disabled = false;
                include.disabled = false;
        }
    }
</script>
    
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
    
    <h1><img src="Images/txt_agent.png" />
    </h1>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
             ForeColor="Red"></asp:Label>
     
     
     <div class="botline">
     <table  style="width:100%">
        <tr>
         <td>
         Deal ID
         </td>
         <td>
             <asp:TextBox ID="txtDealid" runat="server" Enabled="False"></asp:TextBox>
             <%--<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
                 ToolTip="Clear All" onclick="btnClear_Click"/>
         </td>
         <td>
           <asp:Button ID="btnList" runat="server" Text="List" CssClass="button"/>
           --%>
           </td>
         
       
      <td>
       Agent Code
      </td>
      <td>
          <asp:DropDownList ID="ddlAgentCode" runat="server" Width="95px" >
          </asp:DropDownList>
      </td>
      
      <td>
       Applicabe From
      </td>
      <td>
          
          &nbsp;<asp:TextBox ID="txtaplicablefrom" runat="server" Width="70px"></asp:TextBox>
                          <asp:ImageButton ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" 
                              ImageUrl="~/Images/calendar_2.png" />
           <asp:CalendarExtender ID="txtaplicablefrom_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtaplicablefrom" Format="dd-MM-yyyy" PopupButtonID="btnFromDate" PopupPosition="BottomLeft">
          </asp:CalendarExtender>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtaplicablefrom" ErrorMessage="*"></asp:RequiredFieldValidator>
         
      </td>
      
       <td>
       Applicabe To
      </td>
      <td>
          
          <asp:TextBox ID="txtApplicableto" runat="server" Width="70px"></asp:TextBox>
                          <asp:ImageButton ID="btnFromDate0" runat="server" ImageAlign="AbsMiddle" 
                              ImageUrl="~/Images/calendar_2.png" />
          <asp:CalendarExtender ID="txtApplicableto_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtApplicableto" Format="dd-MM-yyyy" PopupButtonID="btnFromDate0" PopupPosition="BottomLeft">
          </asp:CalendarExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtApplicableto" ErrorMessage="*"></asp:RequiredFieldValidator>

      </td>
     <%-- <td>
       <asp:Button ID="BtnList" runat="server" Text="List" CssClass="button" 
              onclick="BtnList_Click" />
       &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
              ToolTip="Clear All" onclick="btnClear_Click"/>
      </td>--%>
      <td>
          
          Deal Type</td>
          <td>
          <asp:DropDownList ID="ddlDealType" runat="server" Width="95px" onchange="javascript:DealType(this)" >
          <asp:ListItem Text="National" Value="0"></asp:ListItem>
          <asp:ListItem Text="Local" Value="1"></asp:ListItem>
          </asp:DropDownList>
      </td>
            <td>
                Currency</td>
            <td>
             <asp:DropDownList ID="ddlCurrency" runat="server">
             </asp:DropDownList>
            </td>
     </tr>
     </table>
     
    </div>
     
      
      <table width="50%"><tr><td><h3>Target</h3></td><td><asp:RadioButton ID="rbTonnage" runat="server" Text = "Tonnage" 
                    GroupName = "TonType" Checked = "true" AutoPostBack = "true" 
                    oncheckedchanged="rbTonnage_CheckedChanged" Visible="false"/>
              <asp:RadioButton ID="rbStandard" runat="server" Text = "Standard" 
                    GroupName = "TonType" Checked = "false" AutoPostBack = "true" 
                    oncheckedchanged="rbStandard_CheckedChanged" Visible="false"/></td><td></td><td><asp:Button ID="btnAdd0" runat="server" CssClass="button" 
                  Text="Add" onclick="btnAdd0_Click" />
             
             <asp:Button ID="btnDelete0" runat="server" CssClass="button" 
                  Text="Delete" onclick="btnDelete0_Click" /></td></tr></table>
    
     
    
     <div  style="overflow:auto">   
     <table width="60%">     
     <td>
     <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" 
                          ID="grdTonnage"                                    
                                      Width="100%" CssClass="grdrowfont" PageSize="4" 
             AllowPaging="True" > 
                                  <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                        <Columns>
                                           
                        
                        <asp:TemplateField HeaderText="Tonnage"><ItemTemplate>
                            <%--<asp:DropDownList ID="ddlUNID" runat="server">
                            </asp:DropDownList>--%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator41" ValidationExpression="^[0-9]+$" runat="server" ControlToValidate="txtTonnage" ErrorMessage="Numeric Digits Only"></asp:RegularExpressionValidator>

                             <asp:TextBox ID="txtTonnage" runat="server" Width="95px"   ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator81" runat="server" ControlToValidate="txtTonnage" ErrorMessage="*"></asp:RequiredFieldValidator>
                                               <%--          Text='<%# Eval("UNID") %>'--%>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ValidationExpression="^[0-9]+$" runat="server" ControlToValidate="txtRate" ErrorMessage="Numeric Digits Only"></asp:RegularExpressionValidator>

                            <asp:TextBox ID="txtRate" runat="server" Width="95px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="txtRate" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
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
        
             
         
         </td>
     
     </tr>
     </table> 
    </div> 
     <div>
      <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Parameters</legend>
       <table witdth="100%" cellpadding=3 cellspacing=3>
        <tr><td valign="top">
       <table width="100%" cellpadding=3 cellspacing=3>
       <tr><td>Slab Of Rate</d><td><asp:CheckBox ID="chkM" runat="server" Text="M"  />&nbsp; &nbsp;&nbsp;<asp:CheckBox ID="chkN" runat="server" Text="N"  /> &nbsp; &nbsp;&nbsp;<asp:CheckBox ID="chkA" runat="server" Text="+45"  /> &nbsp; &nbsp;&nbsp;<asp:CheckBox ID="chkB" runat="server" Text="+100"  /></td>
       </tr>
       <tr><td>Spot Rate Tonnage</td><td><asp:RadioButton ID="rdbIncluded" runat="server" Text="Included"  GroupName="A" />&nbsp;&nbsp;&nbsp;
       <asp:RadioButton ID="rbExcluded" runat="server" Text="Excluded" GroupName="A"  />
       </td></tr>
       <tr><td>Commisionable</td><td><asp:RadioButton ID="rdbyesCommisionable" runat="server" Text="Yes" GroupName="B"   />&nbsp;&nbsp;&nbsp;
       <asp:RadioButton ID="rdbNoCommissionable" runat="server" Text="No" GroupName="B"/>
       </td></tr>
       </table>
       </td>
       <td></td><td></td><td></td>
       <td valign="top">
       <table width="100%" cellpadding=3 cellspacing=3><tr><td>
           Incentive Amount(Tonnage)</td><td>
             <asp:TextBox ID="txtkickbackamount" runat="server" Width="92px" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtkickbackamount" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ValidationExpression="^[0-9]+$" ID="RegularExpressionValidator3" ErrorMessage="Enter Valid Numeric Format!" ControlToValidate="txtkickbackamount" runat="server" ></asp:RegularExpressionValidator>
               </td></tr>
               <tr><td>Threshold</td><td><asp:TextBox ID="txtthrshold" runat="server" Width="92px"></asp:TextBox>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtthrshold" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ValidationExpression="^[0-9]+$" ID="RegularExpressionValidator1" ErrorMessage="Enter Valid Numeric Format!" ControlToValidate="txtthrshold" runat="server" ></asp:RegularExpressionValidator>
       </td></tr>
               <tr><td>Incentive Amount(AWB)</td><td>
             <asp:TextBox ID="txtFlatAmount" runat="server" Width="92px" ></asp:TextBox>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtFlatAmount" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                         <asp:RegularExpressionValidator ValidationExpression="^[0-9]+$" ID="RegularExpressionValidator2" ErrorMessage="Enter Valid Numeric Format!" ControlToValidate="txtFlatAmount" runat="server" ></asp:RegularExpressionValidator>
                   </td></tr>
               </table>
       </td>
       </tr>
       </table>    
      </fieldset> 
      </div>
      <div>
      <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">
          Exceptions</legend>
       <table width="60%">  <tr> <td> <b> Flight Number </b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTFlightNumber" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBFN" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetFlightNumber();return false;" />

                                            </td>
                                            <td>
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
                                            <td >
                                               <b>Airline Code</b> 
                                            </td>
                                            <td >
                                                <asp:TextBox ID="TXTAirLineCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBAC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirLineCode();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExAC" runat="server" Text="Exclude" GroupName="AC" 
                                                    Checked="True" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncAC" runat="server" Text="Include" GroupName="AC" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Prime Flight</b></td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                     <asp:RadioButton ID="RBExPF" runat="server" Text="Exclude" GroupName="PF" 
                                                    Checked="True" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncPF" runat="server" Text="Include" GroupName="PF" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                               <b>IATA Comm. Code</b> 
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTIATAComCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBCC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetIATAComCode();return false;" />

                                            </td>
                                            <td>
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
                                            <td>
                                               <b>Origin</b> 
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTParamORG" runat="server" Width="120px"></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirportORGCode();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExORG" runat="server" Text="Exclude" GroupName="OR" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncORG" runat="server" Text="Include" GroupName="OR" />

                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                               <b>Destination </b> 
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTParamDest" runat="server" Width="120px"></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                        ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirportDestCode();return false;" />

                                            </td>
                                            <td>
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
                                            <td>
                                                <b>Product Type</b></td>
                                            <td>
                                                <asp:TextBox ID="TXTParamProdType" runat="server" Width="120px"></asp:TextBox>
                                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                        ImageAlign="AbsMiddle" OnClientClick="javascript:GetProductType();return false;" />

                                                 </td>
                                            <td>
                                                <asp:RadioButton ID="RBExProdType" runat="server" Text="Exclude" GroupName="Prod" 
                                                    Checked="True" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncProdType" runat="server" Text="Include" GroupName="Prod" />

                                            </td>
                                        </tr>
                                        
                                         <tr>
                                            <td>
                                               <b> Heavy Cargo</b></td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                <asp:RadioButton ID="RBExHeavyCargo" runat="server" Checked="True" GroupName="Heavy" 
                                                    Text="Exclude" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncHeavyCargo" runat="server" GroupName="Heavy" Text="Include" />
                                             </td>
                                        </tr>
                                        
                                         <tr>
                                            <td>
                                                <b>Metro cities</b></td>
                                            <td>
                                                <asp:TextBox ID="TXTParamMinMetro" runat="server" Width="120px" ></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" 
                                                    OnClientClick="javascript:GetAirportORGMetro();return false;" />

                                                 </td>
                                            <td>
                                                <asp:RadioButton ID="RBExMinMetro" runat="server" Checked="True" GroupName="Metro" 
                                                    Text="Exclude" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncMinMetro" runat="server" GroupName="Metro" Text="Include" />
                                             </td>
                                        </tr>
                                        
                                         <tr>
                                            <td>
                                                <b>Non-Metro cities</b></td>
                                            <td>
                                                <asp:TextBox ID="TXTParamMinNonMetro" runat="server" Width="120px" ></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" 
                                                    OnClientClick="javascript:GetAirportORGNonMetro();return false;" />


                                                 </td>
                                            <td>
                                                <asp:RadioButton ID="RBExMinNonMetro" runat="server" Checked="True" GroupName="NonMetro" 
                                                    Text="Exclude" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncMinNonMetro" runat="server" GroupName="NonMetro" Text="Include" />
                                             </td>
                                        </tr>
                                        
                                         <tr>
                                            <td>
                                            <b>Minimum Shipment (Metro)</b></td>
                                            <td class="style1">
                                                <asp:TextBox ID="TXTParamMinMetroValue" runat="server" Width="120px" 
                                                    onchange="javascript:displayValidationMetro();"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="TXTParamMinMetroValue" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                

                                                 </td>
                                            <td>
                                                <asp:RegularExpressionValidator ID="MinMetro" ValidationExpression="^[0-9]+$" ControlToValidate="TXTParamMinMetroValue" runat="server"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        
                                         <tr>
                                            <td>
                                                <b>Minimum Shipment(Non-Metros)</b></td>
                                            <td>
                                                <asp:TextBox ID="TXTParamMinNonMetroValue" runat="server" Width="120px" 
                                                    onchange="javascript:displayValidationNonMetro();"></asp:TextBox>
                                                                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="TXTParamMinNonMetroValue" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                

                                                 </td>
                                            <td>
                                            <asp:RegularExpressionValidator ValidationExpression="^[0-9]+$"  ID="MinNonMetro" ControlToValidate="TXTParamMinNonMetroValue" runat="server" ></asp:RegularExpressionValidator></td>
                                        </tr>
                                        <tr visible="false">
                                            <td visible="false">
                                               <%--<b>Agent Code</b> --%>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTAgentCode" runat="server" Width="120px" Visible="false"></asp:TextBox>

                                                <asp:ImageButton ID="IBAD" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" Visible="false" />

                                            </td>
                                            <td>
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
