<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VolumetricException.aspx.cs" Inherits="ProjectSmartCargoManager.VolumetricException" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
    <script type="text/javascript">

        function GetSpecialHandlingCode() {
           
            var TxtClientObject = '<%= TXTHandlingCode.ClientID %>';
            var TxtClientValue = document.getElementById('ctl00_ContentPlaceHolder1_TabContainer_TBPParameters_TXTHandlingCode').value;
//            var TxtClientValue = document.getElementById('ctl00_ContentPlaceHolder1_TXTHandlingCode').value;
            window.open('ListMultipleSpecialHandlingCode.aspx?TargetTXT=' + TxtClientObject + '&txtValue=' + TxtClientValue, '',
                        'left=' + (screen.availWidth / 5) + ',top=' + (screen.availHeight / 8) +
            ',width=800,height=550,toolbar=0,resizable=0');
        }

        function GetAirportORGCode() {
            var TXTORGClobject = '<%=TXTParamORG.ClientID %>';
            var value = document.getElementById('<%=TXTParamORG.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesORG&level=A&param=Origin&TargetTXT=' + TXTORGClobject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
         }
         function GetAirportDestCode() {
             var TXTDestClobject = '<%=TXTParamDest.ClientID %>';
             var value = document.getElementById('<%=TXTParamDest.ClientID %>').value;
             window.open('ListMultipleSelect.aspx?Parent=MaintainRatesORG&level=A&param=Origin&TargetTXT=' + TXTDestClobject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
         }

         function GetProductType() {
             var TXTPTClobject = '<%=TXTParamPT.ClientID %>';
             
             window.open('ListMultipleSelectProductType.aspx?Parent=MaintainRatesORG&level=A&param=Origin&TargetTXT=' + TXTPTClobject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
         }
         function GetOtherChargeCode() {
             var TxtClientObject = '<%= TxTOCCode.ClientID %>';
             var value = document.getElementById('<%=TxTOCCode.ClientID %>').value;
             window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=OCCode&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
       
          }
         function GetCountry() {
             var TXTConClobject = '<%=TXTParamCountry.ClientID %>';
             var value = document.getElementById('<%=TXTParamCountry.ClientID %>').value;
             window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=Country&TargetTXT=' + TXTConClobject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
         }
         function GetCountryD() {
             var TXTConClobject = '<%=TXTParamCountryD.ClientID %>';
             var value = document.getElementById('<%=TXTParamCountryD.ClientID %>').value;
             window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=Country&TargetTXT=' + TXTConClobject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
         }

         function GetHandler() {
             var TXTHanClobject = '<%=TXTParamHand.ClientID %>';
             var value = document.getElementById('<%=TXTParamHand.ClientID %>').value;
             window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=Handler&TargetTXT=' + TXTHanClobject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
         }


        function GetFlightNumber() {

            var TxtClientObject = '<%= TXTFlightNumber.ClientID %>';
            var value = document.getElementById('<%=TXTFlightNumber.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=FlightNumber&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }

        function GetAirLineCode() {

            var TxtClientObject = '<%= TXTAirLineCode.ClientID %>';
            var value = document.getElementById('<%=TXTAirLineCode.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=AirlineCode&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetFlightCarrier() {

            var TxtClientObject = '<%= TXTFlightCarrier.ClientID %>';
            var value = document.getElementById('<%=TXTFlightCarrier.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=FlightCarrier&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetIssueCarrier() {

            var TxtClientObject = '<%= TXTIssueCarrier.ClientID %>';
            var value = document.getElementById('<%=TXTIssueCarrier.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=FlightCarrier&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }

        function GetIATAComCode() {

            var TxtClientObject = '<%= TXTIATAComCode.ClientID %>';
            var value = document.getElementById('<%=TXTIATAComCode.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=CommCode&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetEquipType() {
            var TxtClientObject = '<%= TXTEquipmentType.ClientID %>';
            var value = document.getElementById('<%=TXTEquipmentType.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=EquipType&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');

        }

        function GetAgentCode() {

            var TxtClientObject = '<%= TXTAgentCode.ClientID %>';
            var value = document.getElementById('<%=TXTAgentCode.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=AgentCode&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetShipperCode() {

            var TxtClientObject = '<%= TXTShipperCode.ClientID %>';
            var value = document.getElementById('<%=TXTShipperCode.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=Shipper&TargetTXT=' + TxtClientObject+'&Values='+value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function DivShow() {
            document.getElementById("divSlab").style.display = 'block';
           
        }

        function DivHide() {
            document.getElementById("divSlab").style.display = 'none';

        }

        function onOCCodePopulated() {
            var completionList = $find("ACEOCCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function GetOCHeadCode(obj) {
            var destination = obj;
            if (destination.value.indexOf("-") > 0) {

                var str = destination.value;
                var start = destination.value.indexOf("-");
             
                obj.value = str.substring(0, start);             
            }

            return false;
        }        
    
    </script>
    
 <script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>
 
 </asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
        <h1>
          Volumetric Exemption
        </h1>
        <asp:HiddenField ID="HidSrNo" runat="server" /> 
        <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>
              <div class="botline">
            <table width="100%">
                <tr>
                    <td>
                        Start Date
                    </td>
                    <td>
                        <asp:TextBox ID="TXTStartDate" runat="server" Width="80px"></asp:TextBox>
                        &nbsp;<asp:ImageButton ID="IBStartDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                            ImageAlign="AbsMiddle" Height="16px" Width="16px" />
                        <asp:CalendarExtender ID="CEStartDate" Format="yyyy-MM-dd" TargetControlID="TXTStartDate"
                            PopupButtonID="IBStartDate" runat="server" PopupPosition="BottomLeft">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        End Date
                    </td>
                    <td>
                        <asp:TextBox ID="TXTEndDate" runat="server" Width="80px"></asp:TextBox>
                        &nbsp;<asp:ImageButton ID="IBEndDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                            ImageAlign="AbsMiddle" Height="16px" Width="16px" />
                        <asp:CalendarExtender ID="CEEndDate" Format="yyyy-MM-dd" TargetControlID="TXTEndDate"
                            PopupButtonID="IBEndDate" runat="server" PopupPosition="BottomLeft">
                        </asp:CalendarExtender>
                    </td>                   
                    <td>Active</td>
                    <td><asp:CheckBox ID="chkStatus" runat="server" /></td>
                    <td></td>
                    <td></td>
                </tr>                
            </table>
        </div>
        <br />
        <table width="100%">
            <tr>            
                <td  valign="top">
                    <div class="divback">
                        <asp:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="0" 
                             ScrollBars="Auto">
                            <asp:TabPanel ID="TBPParameters" runat="server" HeaderText="TabPanel2">
                                <HeaderTemplate>
                                    Parameters </HeaderTemplate>
                              <ContentTemplate>
                                    <br />
                                    <table style="width:100%">
                                        <tr><td>
                                        <table style="background:#f6f6f6; width:100%" cellpadding="3" cellspacing="3">
                                        <tr>
                                            <td style="width:150px;">
                                                Flight Carrier
                                            </td>
                                            <td style="width:264px;">
                                                <asp:TextBox ID="TXTFlightCarrier" runat="server" Width="120px" Enabled="false"></asp:TextBox>

                                                <asp:ImageButton ID="ImageButton3" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetFlightCarrier();return false;" Enabled="false" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExFC" runat="server" Text="Exclude" GroupName="FC" Enabled="false" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncFC" runat="server" Text="Include" GroupName="FC" Enabled="false" />

                                            </td>
                                        </tr>
                                          <tr>
                                            <td>
                                                Issue Carrier
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTIssueCarrier" runat="server" Width="120px" Enabled="false"></asp:TextBox>

                                                <asp:ImageButton ID="ImageButton4" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetIssueCarrier();return false;" Enabled="false" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbExIS" runat="server" Text="Exclude" GroupName="IC" Enabled="false" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="rbIncIS" runat="server" Text="Include" GroupName="IC" Enabled="false" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Airline Code
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTAirLineCode" runat="server" Width="120px" Enabled="false"></asp:TextBox>

                                                <asp:ImageButton ID="IBAC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirLineCode();return false;" Enabled="false" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExAC" runat="server" Text="Exclude" GroupName="AC" Enabled="false" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncAC" runat="server" Text="Include" GroupName="AC" Enabled="false" />

                                            </td>
                                        </tr>
                                        </table></td></tr>
                                        <tr><td><table>
                                        <tr>
                                            <td style="width:150px;">
                                                Origin
                                            </td>
                                            <td style="width:264px;">
                                                <asp:TextBox ID="TXTParamORG" runat="server" Width="120px" Enabled="false"></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirportORGCode();return false;" Enabled="false" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExORG" runat="server" Text="Exclude" GroupName="OR" Enabled="false" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncORG" runat="server" Text="Include" GroupName="OR" Enabled="false" />

                                            </td>
                                        </tr>
                                        <tr>
                                             <td>
                                                 Country Source</td>
                                             <td>
                                                 <asp:TextBox ID="TXTParamCountry" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                                 <asp:ImageButton ID="ImgCountry" runat="server" ImageAlign="AbsMiddle" 
                                                     ImageUrl="~/Images/list_bullets.png" 
                                                     OnClientClick="javascript:GetCountry();return false;" Enabled="false" />
                                             </td>
                                             <td>
                                                 <asp:RadioButton ID="RBExCountry" runat="server" GroupName="ConS" Text="Exclude" Enabled="false" />
                                                 &nbsp;&nbsp;
                                                 <asp:RadioButton ID="RBIncCountry" runat="server" GroupName="ConS" Text="Include" Enabled="false" />
                                             </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                Destination
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTParamDest" runat="server" Width="120px" Enabled="false"></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                        ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirportDestCode();return false;" Enabled="false" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExDest" runat="server" Text="Exclude" GroupName="DT" Enabled="false" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncDest" runat="server" Text="Include" GroupName="DT" Enabled="false" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Country Destination</td>
                                           
                                            <td>
                                                <asp:TextBox ID="TXTParamCountryD" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="ImgCountry0" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetCountryD();return false;" Enabled="false" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExCountryD" runat="server" GroupName="ConD" 
                                                    Text="Exclude" Enabled="false" />
                                                    &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncCountryD" runat="server" GroupName="ConD" 
                                                    Text="Include" Enabled="false" />
                                             </td>
                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                Flight Number
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTFlightNumber" runat="server" Width="120px" Enabled="false" ></asp:TextBox>

                                                <asp:ImageButton ID="IBFN" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetFlightNumber();return false;" Enabled="false"/>

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExFN" runat="server" Text="Exclude" GroupName="FN" Enabled="false" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncFN" runat="server" Text="Include" GroupName="FN" Enabled="false" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:157px;">
                                                Days Of Week
                                            </td>
                                            <td style="width:268px;">
                                                <asp:CheckBoxList ID="cblWeekdays" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                                    <asp:ListItem Text="Mon" Value="Mon"></asp:ListItem>
                                                    <asp:ListItem Text="Tue" Value="Tue"></asp:ListItem>
                                                    <asp:ListItem Text="Wed" Value="Wed"></asp:ListItem>
                                                    <asp:ListItem Text="Thu" Value="Thu"></asp:ListItem>
                                                    <asp:ListItem Text="Fri" Value="Fri"></asp:ListItem>
                                                    <asp:ListItem Text="Sat" Value="Sat"></asp:ListItem>
                                                    <asp:ListItem Text="Sun" Value="Sun"></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbWeekdaysExclude" runat="server" GroupName="Weekdays" 
                                                    Text="Exclude" Enabled="false" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="rbWeekdaysInclude" runat="server" GroupName="Weekdays" 
                                                    Text="Include" Enabled="false" />
                                            </td>
                                        </tr>
                                        </table></td></tr>
                                        <tr><td>
                                        <table style="background:#f6f6f6; width:100%" cellpadding="3" cellspacing="3">
                                        
                                        <tr>
                                            <td style="width:150px;">
                                                Agent Code
                                            </td>
                                            <td style="width:264px;">
                                                <asp:TextBox ID="TXTAgentCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBAD" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExAD" runat="server" Text="Exclude" GroupName="AD" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncAD" runat="server" Text="Include" GroupName="AD" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Shipper Code
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTShipperCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBSC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetShipperCode();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExSC" runat="server" Text="Exclude" GroupName="SC" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncSC" runat="server" Text="Include" GroupName="SC" />

                                            </td>
                                        </tr>
                                        
                                        
                                        <tr>
                                            <td>
                                                IATA Comm. Code
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTIATAComCode" runat="server" Width="120px" Enabled="true"></asp:TextBox>

                                                <asp:ImageButton ID="IBCC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetIATAComCode();return false;" Enabled="true" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExCC" runat="server" Text="Exclude" GroupName="CC" Enabled="true" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncCC" runat="server" Text="Include" GroupName="CC" Enabled="true" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Product Type</td>
                                            <td>
                                                <asp:TextBox ID="TXTParamPT" runat="server" Width="120px"></asp:TextBox>
                                                <asp:ImageButton ID="ImgPT" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetProductType();return false;" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExPT" runat="server" GroupName="PT" Text="Exclude" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncPT" runat="server" GroupName="PT" Text="Include" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                SPL Handling Code
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTHandlingCode" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="IBSHC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetSpecialHandlingCode();return false;" Enabled="false" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExHC" runat="server" Text="Exclude" GroupName="HC" Enabled="false" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncHC" runat="server" Text="Include" GroupName="HC" Enabled="false" />

                                            </td>
                                        </tr>
                                          
                                         </table></td></tr>
                                         <tr><td><table >
                                         
                                        
                                        <tr>
                                            <td style="width:157px;">
                                                Handler</td>
                                            <td  style="width: 268px">
                                                <asp:TextBox ID="TXTParamHand" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="ImgHand" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetHandler();return false;" Enabled="false" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExHand" runat="server" GroupName="Han" 
                                                    Text="Exclude" Enabled="false" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncHand" runat="server" GroupName="Han" 
                                                    Text="Include" Enabled="false" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                Equipment Type</td>
                                            <td>
                                                <asp:TextBox ID="TXTEquipmentType" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="imgEquipType" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetEquipType();return false;" Enabled="false"/>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExET" runat="server" GroupName="ET" 
                                                    Text="Exclude" Enabled="false"/>
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncET" runat="server" GroupName="ET" 
                                                    Text="Include" Enabled="false"/>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                OCCode</td>
                                            <td>
                                                <asp:TextBox ID="TxTOCCode" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="ImageButton5" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetOtherChargeCode();return false;" Enabled="false" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExOC" runat="server" GroupName="OC" 
                                                    Text="Exclude" Enabled="false" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncOC" runat="server" GroupName="OC" 
                                                    Text="Include" Enabled="false" />
                                            </td>
                                        </tr>
                                        </table></td></tr>
                                        
                                    </table>
                                
                                
</ContentTemplate> 
                            </asp:TabPanel>
                            <asp:TabPanel ID="TBPExceptions" runat="server" HeaderText="TabPanel2" Visible="false">
                                <HeaderTemplate>
                                    Exceptions </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanelExceptions" runat="server">
                                        <ContentTemplate>
                                            <table style=" overflow:scroll;">
                                                <%--<tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add" OnClick="btnAdd_Click" />
                                                        &nbsp;
                                                        <asp:Button ID="btnDelete" runat="server" CssClass="button" Text="Delete" OnClick="btnDelete_Click" />
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="GRDException" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Agent">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="CHKSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Agent">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="DDLAgent" runat="server">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Commision%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="TXTCommision" Width="60px" runat="server" Text='<%# Eval("Commision") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Discount%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="TXTDiscount" Width="60px" runat="server" Text='<%# Eval("Discount") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="TXTTax" Width="60px" runat="server" Text='<%# Eval("Tax") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="titlecolr" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                            <AlternatingRowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </ContentTemplate>
                            </asp:TabPanel>
                             <asp:TabPanel ID="TBTrigger" runat="server" HeaderText="TabPanel2" Visible="false"> 
                                <HeaderTemplate>
                                    Trigger Point </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdateTriggerPanel" runat="server">
                                        <ContentTemplate>
                                            <table style=" overflow:scroll;">
                                             <tr>
                                                <td class="style2">
                                                       Trigger Point
                                                    </td>
                                                    <td style="width: 170px">
                                                     <asp:DropDownList ID="ddlTriggerPoint" runat="server" >
                                                            <asp:ListItem Text="" Value="E"></asp:ListItem>                                                     
                                                            <asp:ListItem Text="Booking" Value="B"></asp:ListItem>
                                                            <asp:ListItem Text="Confirmation" Value="C"></asp:ListItem>
                                                            <asp:ListItem Text="Acceptance" Value="A"></asp:ListItem>
                                                            <asp:ListItem Text="Manifest" Value="M"></asp:ListItem>
                                                            <asp:ListItem Text="Arrival" Value="R"></asp:ListItem>
                                                            <asp:ListItem Text="Delivery" Value="D"></asp:ListItem>                                                            
                                                        </asp:DropDownList>
                                                        
                                                    </td>
                                                  
                                                </tr>
                                              <tr>
                                              <td class="style2">
                                              Applicable After
                                              </td>
                                                <td style="width:170px;">                                                     
                                                    <asp:TextBox ID="txtHrs" runat="server" Text="0" Width="60px" CssClass="alignrgt"></asp:TextBox>                                                     
                                                    Hrs   
                                                 </td>
                                              </tr>
                                              <tr>
                                              <td>
                                              Week Ends
                                              </td>
                                               <td style="width: 170px">
                                                <asp:RadioButton ID="RBExWE" runat="server" Text="Exclude" GroupName="WE" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBInWE" runat="server" Text="Include" GroupName="WE" />

                                            </td>
                                            
                                              </tr>
                                              <tr>
                                              <td>
                                            Public Holidays
                                            </td>
                                               <td style="width: 170px">
                                                <asp:RadioButton ID="RBExPH" runat="server" Text="Exclude" GroupName="PH" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBInPH" runat="server" Text="Include" GroupName="PH" />

                                            </td>
                                            </tr>
                                              <tr>
                                            <td>
                                            Company Holidays
                                            </td>
                                            
                                               <td style="width: 170px">
                                                <asp:RadioButton ID="RBExCH" runat="server" Text="Exclude" GroupName="CH" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBInCH" runat="server" Text="Include" GroupName="CH" />

                                            </td>
                                              </tr>
                                              <tr>
                                                <td>
                                           Station Holiday
                                            </td>
                                               <td style="width: 170px">
                                                <asp:RadioButton ID="RBExSH" runat="server" Text="Exclude" GroupName="SH" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBInSH" runat="server" Text="Include" GroupName="SH" />

                                            </td>
                                              </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>
                    </div>
                </td>
                
                
                
            </tr>
                        
        </table>
        
        
        <div style="overflow:scroll;  width:100%;">
<asp:Repeater ID="RepDetails" runat="server">
<HeaderTemplate>
<table style=" border:1px solid #1F2B53; width:100%" cellpadding="0">
<tr style="background-color:#1F2B53; color:White">
<td colspan="2">
<b>Remarks</b>
</td>
</tr>
</HeaderTemplate>
<ItemTemplate>
<tr style="background-color:#EBEFF0">
<td>
<table style="background-color:#EBEFF0;border-top:1px dotted #1F2B53; width:100%" >

</table>
</td>
</tr>
<tr>
<td>
<asp:Label ID="lblComment" runat="server" Text='<%#Eval("comments") %>'/>
</td>
</tr>
<tr>
<td>
<table style="background-color:#EBEFF0;border-top:1px dotted #1F2B53;border-bottom:1px solid #1F2B53; width:500px" >
<tr>
<td>Post By: <asp:Label ID="lblUser" runat="server" Font-Bold="true" Text='<%#Eval("name") %>'/></td>
<td>Created Date:<asp:Label ID="lblDate" runat="server" Font-Bold="true" Text='<%#Eval("date") %>'/></td>
</tr>
</table>
</td>
</tr>
<tr>
<td colspan="2">&nbsp;</td>
</tr>
</ItemTemplate>
<FooterTemplate>
</table>
</FooterTemplate>
</asp:Repeater>
</div>
        
        
        
        
        
        <table width="100%">
        
            <tr>
                <td align="justify">
                    &nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
                        onclick="btnSave_Click" Width="61px" />
                    &nbsp;</td>
            </tr>
        </table>
        <asp:HiddenField ID="HidOrigin" runat="server" />
        <asp:HiddenField ID="HidDest" runat="server" />
        <asp:HiddenField ID="HidLocation" runat="server" />
        <asp:HiddenField ID="HidCurrency" runat="server" />
        <asp:HiddenField ID="HidCode" runat="server" />
    </div>
</asp:Content>
