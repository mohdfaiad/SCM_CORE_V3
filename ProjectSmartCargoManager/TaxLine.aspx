<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaxLine.aspx.cs" Inherits="ProjectSmartCargoManager.TaxLine" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
    <script type="text/javascript">

        function SetAll() {
            
            var chkBoxList = document.getElementById("<%= chkRateList.ClientID%>");
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            
            if (document.getElementById("<%= chkAll.ClientID%>").checked) {
                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = true;
                }
            }
            else {
                for (var i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = false;
                }
            }
            return false;
        }

        function GetOrigin() {

            var ddllevel = document.getElementById('<%=DDLOriginLevel.ClientID%>');
            var level = ddllevel.options[ddllevel.selectedIndex].value;
            var TxtOriginClientObject = '<%=TXTOrigin.ClientID %>';
            var HidObject = '<%= HidOrigin.ClientID %>';

            window.open('ListDataView.aspx?Parent=MaintainRatesORG&level=' + level + '&TargetTXT=' + TxtOriginClientObject + '&Hid=' + HidObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
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

        function GetDestination() {

            var ddllevel = document.getElementById('<%=DDLDestinationLevel.ClientID%>');
            var level = ddllevel.options[ddllevel.selectedIndex].value;
            var TxtDestinationClientObject = '<%= TXTDestination.ClientID %>';
            var HidObject = '<%= HidDest.ClientID %>';

            window.open('ListDataView.aspx?Parent=MaintainRatesDEST&level=' + level + '&TargetTXT=' + TxtDestinationClientObject + '&Hid=' + HidObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }


        function GetLocation() {

            var ddllevel = document.getElementById('<%=DDLLevel.ClientID%>');
            var level = ddllevel.options[ddllevel.selectedIndex].value;
            var TxtDestinationClientObject = '<%= TXTLocation.ClientID %>';
            var HidObject = '<%= HidLocation.ClientID %>';

            window.open('ListDataView.aspx?Parent=MaintainRatesDEST&level=' + level + '&TargetTXT=' + TxtDestinationClientObject + '&Hid=' + HidObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }


        function GetCurrency() {

            var TxtCurrencyClientObject = '<%= TXTCurrency.ClientID %>';
            var HidObject = '<%= HidCurrency.ClientID %>';

            window.open('ListDataView.aspx?Parent=MaintainRatesCUR&TargetTXT=' + TxtCurrencyClientObject + '&Hid=' + HidObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }

        function GetCode() {

            var TxtCurrencyClientObject = '<%= TXTTaxCode.ClientID %>';
            var HidObject = '<%= HidCode.ClientID %>';

            window.open('ListDataView.aspx?Parent=OtherChargesCode&TargetTXT=' + TxtCurrencyClientObject + '&Hid=' + HidObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
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
        function BaseRateSelect() {
            
            var ddllevel = document.getElementById('<%=ddlBasedOn.ClientID%>');
            var level = ddllevel.options[ddllevel.selectedIndex].value;
            document.getElementById("<%= TXTBaseRate.ClientID %>").disabled = true;
            if (level == "D") {
                document.getElementById("<%= ddlBasedOn.ClientID %>").disabled = false;
                  document.getElementById("<%= TXTBaseRate.ClientID %>").disabled = false;
               }
         }

         function DivViewSelect() {             
            var ddllevel = document.getElementById('<%=DDLHeadBasis.ClientID%>');
            var level = ddllevel.options[ddllevel.selectedIndex].value;
            document.getElementById("<%= TXTMinimum.ClientID %>").disabled = false;
            document.getElementById("<%= TXTCharge.ClientID %>").disabled = false;
            document.getElementById("<%= TXTMCost.ClientID %>").disabled = false;
            document.getElementById("<%= TXTCost.ClientID %>").disabled = false;
            
            document.getElementById("<%= DDLWtType.ClientID %>").disabled = false;
            document.getElementById("<%= ddlBasedOn.ClientID %>").disabled = true;
            document.getElementById("<%= DDLWtType.ClientID %>").visible = true;

            document.getElementById("<%= ddlBasedOn.ClientID %>").disabled = true;
            document.getElementById("<%= TXTBaseRate.ClientID %>").disabled = true;
            
            if (level == "K") {
                document.getElementById("<%= TXTMinimum.ClientID %>").disabled = true;
                document.getElementById("<%= TXTCharge.ClientID %>").disabled = true;

                document.getElementById("<%= TXTMCost.ClientID %>").disabled = true;
                document.getElementById("<%= TXTCost.ClientID %>").disabled = true;
                document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                DivShow();
            }
            if (level == "S") {
                document.getElementById("<%= TXTMinimum.ClientID %>").disabled = true;
                document.getElementById("<%= TXTCharge.ClientID %>").disabled = true;
                document.getElementById("<%= TXTMCost.ClientID %>").disabled = true;
                document.getElementById("<%= TXTCost.ClientID %>").disabled = true;
                document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                DivShow();
            }
            if (level == "F") {
                document.getElementById("<%= TXTMinimum.ClientID %>").disabled = true;
                document.getElementById("<%= TXTMCost.ClientID %>").disabled = true;
                
                document.getElementById("<%= DDLWtType.ClientID %>").value = "B";
                document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                DivHide();
            }
            if (level == "W") {
                //document.getElementById("<%= DDLWtType.ClientID %>").value = "G";
                DivHide();
            }
            if (level == "H") {
                document.getElementById("<%= DDLWtType.ClientID %>").value = "H";
                document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                document.getElementById("<%= TXTMinimum.ClientID %>").disabled = true;
                document.getElementById("<%= TXTMCost.ClientID %>").disabled = true;
                
                DivHide();
            }
            if (level == "P") {
                document.getElementById("<%= DDLWtType.ClientID %>").value = "A";
                document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                document.getElementById("<%= ddlBasedOn.ClientID %>").disabled = false;
                DivHide();
                BaseRateSelect();
            }
            if (level == "C") {
                document.getElementById("<%= DDLWtType.ClientID %>").value = "P";
                document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                DivHide();
            }

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

        
         //window.onload(DivHide());

//         function timelineMandatory() {
//             var ddlchrg = document.getElementById('ctl00_ContentPlaceHolder1_DDLHeadBasis').value;
//             alert(ddlchrg);
//             if(ddlchrg=="T") {
//                 if (document.getElementById('ctl00_ContentPlaceHolder1_TabContainer_TBTrigger_txtHrs').value == 0)
//                     alert('Please Enter Applicable After.');
//                 if (document.getElementById('ctl00_ContentPlaceHolder1_GRDRateSlabs_ctl02_TXTCost').value == 0 && document.getElementById('ctl00_ContentPlaceHolder1_GRDRateSlabs_ctl02_TXTCharge').value == 0 && document.getElementById('ctl00_ContentPlaceHolder1_GRDRateSlabs_ctl02_TXTWeight').value == 0)
//                     alert('Please Enter Minimum Weight/Count,Charge/Rate,Cost.');
//                 return false;
//             }
//             
//         }
    
    </script>
    
 <script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>
 
 <script type="text/javascript">

     $(function() {
         $('#<%=  ddlBasedOn.ClientID %>').change(function() {
             var ddllevel = document.getElementById('<%=ddlBasedOn.ClientID%>');
             var level = ddllevel.options[ddllevel.selectedIndex].value;
             document.getElementById("<%= TXTBaseRate.ClientID %>").disabled = true;
             if (level == "D") {
                 document.getElementById("<%= TXTBaseRate.ClientID %>").disabled = false;
             }
         });
         $('#<%=  DDLHeadBasis.ClientID %>').change(function() {

         document.getElementById("<%= TXTMinimum.ClientID %>").value = '0';
         document.getElementById("<%= TXTCharge.ClientID %>").value = '0';
         document.getElementById("<%= TXTMCost.ClientID %>").value = '0';
         document.getElementById("<%= TXTCost.ClientID %>").value = '0';
         
             document.getElementById("<%= TXTMinimum.ClientID %>").disabled = false;
             document.getElementById("<%= TXTCharge.ClientID %>").disabled = false;
             document.getElementById("<%= TXTMCost.ClientID %>").disabled = false;
             document.getElementById("<%= TXTCost.ClientID %>").disabled = false;
             
             document.getElementById("<%= DDLWtType.ClientID %>").disabled = false;
             document.getElementById("<%= ddlBasedOn.ClientID %>").disabled = true;
             document.getElementById("<%= DDLWtType.ClientID %>").visible = true;

             document.getElementById("<%= ddlBasedOn.ClientID %>").disabled = true;
             document.getElementById("<%= TXTBaseRate.ClientID %>").disabled = true;

             if (this.value == "K") {
                 document.getElementById("<%= TXTMinimum.ClientID %>").disabled = true;
                 document.getElementById("<%= TXTCharge.ClientID %>").disabled = true;
                 document.getElementById("<%= TXTMCost.ClientID %>").disabled = true;
                 document.getElementById("<%= TXTCost.ClientID %>").disabled = true;
                 document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                 DivShow();
             }
             if (this.value == "S") {
                 document.getElementById("<%= TXTMinimum.ClientID %>").disabled = true;
                 document.getElementById("<%= TXTCharge.ClientID %>").disabled = true;
                 document.getElementById("<%= TXTMCost.ClientID %>").disabled = true;
                 document.getElementById("<%= TXTCost.ClientID %>").disabled = true;
                 
                 document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                 DivShow();
             }
             if (this.value == "F") {
                 document.getElementById("<%= TXTMinimum.ClientID %>").disabled = true;
                 document.getElementById("<%= TXTMCost.ClientID %>").disabled = true;
                 
                 
                 document.getElementById("<%= DDLWtType.ClientID %>").value = "B";
                 document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                 DivHide();
             }
             if (this.value == "W") {
                 document.getElementById("<%= DDLWtType.ClientID %>").value = "G";
                 DivHide();
             }
             if (this.value == "H") {
                 document.getElementById("<%= DDLWtType.ClientID %>").value = "H";
                 document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                 document.getElementById("<%= TXTMinimum.ClientID %>").disabled = true;
                 document.getElementById("<%= TXTMCost.ClientID %>").disabled = true;
                 
                 DivHide();
             }
             if (this.value == "P") {
                 document.getElementById("<%= DDLWtType.ClientID %>").value = "A";
                 document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                 document.getElementById("<%= ddlBasedOn.ClientID %>").disabled = false;
                 //
                 DivHide();
             }
             if (this.value == "C") {
                 document.getElementById("<%= DDLWtType.ClientID %>").value = "P";
                 document.getElementById("<%= DDLWtType.ClientID %>").disabled = true;
                 DivHide();
             }
             if (this.value == "T") {
                 
                 DivShow();
             }

         });
     });
 </script>
 
    </asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
    
    <div class="msg">
      <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
   </div>
   
   
        <h1>
          Tax Line
        </h1>
        <div class="botline" align="left">
            <table style="width: 90%; height: 20px">
                <tr>
                    <td style="width: 130px">
                        Tax Code
                    </td>
                    <td align="left" style="width: 170px">
                        <asp:TextBox ID="TXTTaxCode" runat="server" Width="120px" ></asp:TextBox>
                          
                        <asp:HiddenField ID="HidSrNo" runat="server" />    
                    </td>
                    <td align="left" style="width: 130px">
                        <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
                            onclick="btnList_Click" Visible="false" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
                            onclick="btnClear_Click" Width="44px" Visible="false" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <%--<asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>--%>
              <div class="botline">
            <table width="100%">
               
                <tr>
                    <td>
                        Tax Name
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="TXTTaxName" runat="server" Width="220px"> </asp:TextBox>
                    </td>
                    <td>Tax Type*</td>
                    <td>
                        <asp:DropDownList ID="ddlTaxCode" runat="server">
                        <asp:ListItem Text="Select" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Start Date
                    </td>
                    <td>
                        <asp:TextBox ID="TXTStartDate" runat="server" Width="80px"></asp:TextBox>
                        &nbsp;<asp:ImageButton ID="IBStartDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                            ImageAlign="AbsMiddle" Height="16px" Width="16px" />
                        <asp:CalendarExtender ID="CEStartDate" Format="dd/MM/yyyy" TargetControlID="TXTStartDate"
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
                        <asp:CalendarExtender ID="CEEndDate" Format="dd/MM/yyyy" TargetControlID="TXTEndDate"
                            PopupButtonID="IBEndDate" runat="server" PopupPosition="BottomLeft">
                        </asp:CalendarExtender>
                    </td>
                    <%--<td>
                        Participation Type
                    </td>--%>
                    <td>
                        <asp:DropDownList ID="DDLParticipationType" runat="server" Width="90px" Visible="false">
                            <asp:ListItem Text="Normal" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <%--<td>
                       Charge  Refundable
                    </td>--%>
                    <td>
                    <asp:DropDownList ID="DDLCharge" runat="server" Width="90px" Visible="false">
                        <%-- <asp:ListItem Text="Normal" Value="N"></asp:ListItem>
                          <asp:ListItem Text="CUT" Value="C"></asp:ListItem>
                           <asp:ListItem Text="AAI" Value="A"></asp:ListItem>--%>
                        </asp:DropDownList>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        Level
                    </td>
                    <td>
                        <asp:DropDownList ID="DDLLevel" runat="server" Width="90px">
                            <asp:ListItem Text="Airport" Selected="True" Value="A"></asp:ListItem>
                            <asp:ListItem Text="City" Value="C"></asp:ListItem>
                            <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                            <asp:ListItem Text="Country" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        Location
                    </td>
                    <td>
                        <asp:TextBox ID="TXTLocation" runat="server" Width="60px"></asp:TextBox>
                        <asp:ImageButton ID="IBLoc" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetLocation();return false;" />
                    </td>
                   <%-- <td>
                        Payment Type
                    </td>--%>
                    <td>
                        <asp:DropDownList ID="DDLPaymentType" runat="server" Visible="false">
                            <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                            <asp:ListItem Text="PP" Value="PP"></asp:ListItem>
                            <asp:ListItem Text="CC" Value="CC"></asp:ListItem>
                            <asp:ListItem Text="CP" Value="CP"></asp:ListItem>
                            <asp:ListItem Text="PC" Value="PC"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <%--<td>
                        Charge Type
                    </td>--%>
                    <td>
                        <asp:DropDownList ID="DDLChargeType" runat="server" Visible="false">
                            <asp:ListItem Text="Due Carrier" Value="DC" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Due Agent" Value="DA"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Origin Level
                    </td>
                    <td>
                        <asp:DropDownList ID="DDLOriginLevel" runat="server" Width="90px">
                            <asp:ListItem Text="Airport" Selected="True" Value="A"></asp:ListItem>
                            <asp:ListItem Text="City" Value="C"></asp:ListItem>
                            <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                            <asp:ListItem Text="Country" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        Origin
                    </td>
                    <td>
                        <asp:TextBox ID="TXTOrigin" runat="server" Width="60px"></asp:TextBox>
                        <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetOrigin();return false;" />
                    </td>
                    <td>
                        Destination Level
                    </td>
                    <td>
                        <asp:DropDownList ID="DDLDestinationLevel" runat="server">
                            <asp:ListItem Text="Airport" Selected="True" Value="A"></asp:ListItem>
                            <asp:ListItem Text="City" Value="C"></asp:ListItem>
                            <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                            <asp:ListItem Text="Country" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        Destination
                    </td>
                    <td>
                        <asp:TextBox ID="TXTDestination" runat="server" Width="60px"></asp:TextBox>
                        <asp:ImageButton ID="IBDestination" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetDestination();return false;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Currency
                    </td>
                    <td>
                    <asp:DropDownList ID="TXTCurrency" runat="server" Width="90px"></asp:DropDownList>
                      <%--  <asp:TextBox ID="TXTCurrency" runat="server" Width="60px"></asp:TextBox>
                        <asp:ImageButton ID="IBCurrency" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetCurrency();return false;" />--%>
                    </td>
                    <td>
                        GL Account Code</td>
                    <td>
                    <asp:DropDownList ID="ddlGLCode" runat="server" Width="90px"></asp:DropDownList>
                    </td>
                    <td>Applied At</td>
                    <td>
                        <asp:DropDownList ID="ddlAppAt" runat="server">
                        <asp:ListItem Text="All" Value="A"></asp:ListItem>
                        <asp:ListItem Text="Booking" Value="B"></asp:ListItem>
                        <asp:ListItem Text="Billing" Value="I"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkAddInTotal" runat="server" Text="Add In Total" Checked="true"/>
                    </td>
                </tr>
                <tr>
                    <%--<td>
                        Discount
                    </td>--%>
                    <td>
                        <asp:TextBox ID="TXTDiscount" runat="server" Width="60px" CssClass="alignrgt" Visible="false"></asp:TextBox><%--%--%>
                    </td>
                   <%--<td>
                        Commision
                    </td>--%>
                    <td>
                        <asp:TextBox ID="TXTCommision" runat="server" Width="60px" CssClass="alignrgt" Visible="false"></asp:TextBox><%--%--%>
                    </td>
                    <%--<td>
                        Tax
                    </td>--%>
                    <td>
                        <asp:TextBox ID="TXTTax" runat="server" Width="60px" CssClass="alignrgt" Visible="false"></asp:TextBox><%--%--%>
                    </td>
                    <td><asp:CheckBox ID="CHKRefundable" runat="server" Text="" Visible="false"/></td>
                </tr>
            </table>
        </div>
        <br />
        <table width="100%">
            <tr>
            <td valign="top" style="width:30%;">
                <br />
                    <div class="divback">
                        <asp:UpdatePanel ID="UpPanelBase" runat="server">
                            <ContentTemplate>
                                <table style="width: 100%;>
                                    <tr>
                                        <td class="style2">
                                        </td>
                                    </tr>
                                            <caption>
                                                <tr ID="trMinimum">
                                                   <%-- <td class="style2">
                                                        Charge Head Basis
                                                    </td>--%>
                                                    <td>
                                                        <asp:DropDownList ID="DDLHeadBasis" runat="server" AppendDataBoundItems="true" Visible="false" 
                                                            AutoPostBack="false" OnSelectedIndexChanged="DDLHeadBasis_SelectedIndexChanged" >
                                                            <asp:ListItem Selected="True" Value="F">Flat Charge</asp:ListItem>
                                                            <asp:ListItem Value="W">Weight Base</asp:ListItem>
                                                            <asp:ListItem Value="H">HAWB Count</asp:ListItem>
                                                            <asp:ListItem Value="P">% Base</asp:ListItem>
                                                            <asp:ListItem Value="C">Pcs Count</asp:ListItem>
                                                            <asp:ListItem Value="K">Weight Slab</asp:ListItem>
                                                            <asp:ListItem Value="S">Pcs Slab</asp:ListItem>
                                                            <asp:ListItem Value="T">Time Base</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr ID="trMinimum">
                                                <td class="style2">Tax</td>
                                                <td>
                                                    <asp:TextBox ID="txtTaxPercent" runat="server" Width="70px" CssClass="alignrgt"></asp:TextBox>%
                                                </td>
                                                </tr>
                                                <tr ID="tr1">
                                                    <td class="style2">
                                                        Minimum
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TXTMinimum" runat="server" Enabled="true" ToolTip="Price"
                                                            Width="60px" CssClass="alignrgt"></asp:TextBox>
                                                            <asp:TextBoxWatermarkExtender ID="txtmin_TextBoxWatermarkExtender" 
                    runat="server" WatermarkText="Price" Enabled="True" TargetControlID="TXTMinimum">
                </asp:TextBoxWatermarkExtender>
                                                  <%--/--%>
                                                         <asp:TextBox ID="TXTMCost" runat="server" Enabled="false" ToolTip="Cost" Visible="false"
                                                            Width="60px" CssClass="alignrgt"></asp:TextBox>
                                                            <asp:TextBoxWatermarkExtender ID="txtminCost_TextBoxWatermarkExtender" 
                    runat="server" WatermarkText="Cost" Enabled="True" TargetControlID="TXTMCost">
                </asp:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                <td>Maximum</td>
                                                <td> 
                                                   <asp:TextBox ID="TXTMax" runat="server" Enabled="true" ToolTip="Price"
                                                            Width="60px" CssClass="alignrgt"></asp:TextBox>
                                                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" 
                                                        runat="server" WatermarkText="Price" Enabled="True" TargetControlID="TXTMax">
                                                </asp:TextBoxWatermarkExtender>
                                                </td>
                                                </tr>
                                                <tr>
                                                    <%--<td class="style2">
                                                        Charge
                                                    </td>--%>
                                                    <td>
                                                        <asp:TextBox ID="TXTCharge" runat="server" Width="60px" Visible="false" CssClass="alignrgt" ToolTip="Price"></asp:TextBox>
                                                        <asp:TextBoxWatermarkExtender ID="txtprice_TextBoxWatermarkExtender" 
                    runat="server" WatermarkText="Price" Enabled="True" TargetControlID="TXTCharge">
                </asp:TextBoxWatermarkExtender>
                                                        <%--/--%><asp:TextBox ID="TXTCost" runat="server" Width="60px" Visible="false" CssClass="alignrgt" ToolTip="Cost"></asp:TextBox>
                                                        <asp:TextBoxWatermarkExtender ID="txtcost_TextBoxWatermarkExtender" 
                    runat="server" WatermarkText="Cost" Enabled="True" TargetControlID="TXTCost">
                </asp:TextBoxWatermarkExtender>
                                                        <asp:DropDownList ID="DDLWtType" runat="server" Enabled="false" Visible="false" 
                                                            onselectedindexchanged="DDLWtType_SelectedIndexChanged">
                                                             <asp:ListItem Text=" " Value="B"></asp:ListItem>
                                                            <asp:ListItem Text="Gross Wt" Value="G"></asp:ListItem>
                                                            <asp:ListItem Text="Charged Wt" Value="C"></asp:ListItem>
                                                            <asp:ListItem Text="Per Piece" Value="P"></asp:ListItem>
                                                             <asp:ListItem Text="%" Value="A"></asp:ListItem>  
                                                             <asp:ListItem Text="Per HAWB" Value="H"></asp:ListItem>
                                                             <asp:ListItem Text="PerKG PerDay" Value="D"></asp:ListItem>   
                                                             <asp:ListItem Text="PerKG PerHour" Value="T"></asp:ListItem>                                                            
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                <td class="style2">
                                                Applied On</td>
                                                 <td>
                                                        <asp:CheckBox ID="chkAll" runat="server" Text="All" OnChange="javascript:SetAll();return false;" />
                                                     <asp:CheckBoxList ID="chkRateList" runat="server">
                                                     <asp:ListItem Text="IATA Freight" Value="I"></asp:ListItem>
                                                     <asp:ListItem Text="MKT Freight" Value="M"></asp:ListItem>
                                                     <asp:ListItem Text="OCDC" Value="O"></asp:ListItem>
                                                     <asp:ListItem Text="OCDA" Value="A"></asp:ListItem>
                                                     <asp:ListItem Text="Commission" Value="C"></asp:ListItem>                                                    
                                                     <asp:ListItem Text="Discount" Value="D"></asp:ListItem>
                                                      <asp:ListItem Text="Spot Freight" Value="S"></asp:ListItem>
                                                     </asp:CheckBoxList>
                                                 
                                                        <asp:DropDownList ID="ddlBasedOn" runat="server" Enabled="false" Visible="false">
                                                            <asp:ListItem Text="Freight" Value="F"></asp:ListItem>
                                                            <asp:ListItem Text="Declared Value" Value="D"></asp:ListItem>                                                            
                                                        </asp:DropDownList>
                                                        <%--BaseRate--%>
                                                        <asp:TextBox ID="TXTBaseRate" runat="server" Visible="false" Text="0" Width="60px" Enabled="false" CssClass="alignrgt"></asp:TextBox>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--<td class="style2">
                                                        ChargedAt</td>--%>
                                                    <td>
                                                        <asp:DropDownList ID="ddlType" runat="server" Enabled="true" Visible="false">
                                                            <asp:ListItem Text="At Departure" Value="Dep"></asp:ListItem>
                                                            <asp:ListItem Text="At Delivery" Value="Del"></asp:ListItem>
                                                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--<td class="style2">
                                                        Via Station</td>--%>
                                                    <td>
                                                        <asp:DropDownList ID="ddlViaStation" runat="server" Enabled="true" Visible="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style2" colspan="2">
                                                    </td>
                                                </tr>
                                            </caption>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="SlabUP" runat="server">
                        <ContentTemplate>
                        <div class="divback" id="divSlab">
                                    <table style="width:100%;">
                                        <tr>
                                            <td align="right">
                                                <%--<asp:LinkButton ID="LBNAdd" runat="server" Text="Add" OnClick="LBNAdd_OnClick"></asp:LinkButton>--%>
                                                <asp:Button ID="Button1" runat="server" Text="Add" CssClass="button" OnClick="LBNAdd_OnClick" Visible="false" />
                                                &nbsp;
                                                <%--<asp:LinkButton ID="LBNDelete" runat="server" Text="Delete" OnClick="LBNDelete_OnClick"></asp:LinkButton>--%>
                                                <asp:Button ID="Button2" runat="server" Text="Delete" OnClick="LBNDelete_OnClick"
                                                    CssClass="button" Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="GRDRateSlabs" runat="server" AutoGenerateColumns="False" ShowFooter="True" Visible="false"
                                        Width="100%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CHK" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DdlType" runat="server" EnableViewState="true">
                                                        <asp:ListItem Text="M" Value="M"></asp:ListItem>
                                                        <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                        <asp:ListItem Text="Q" Value="Q"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weight/Count" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTWeight" runat="server" Width="60px" MaxLength="5" Text='<%# Eval("Weight") %>'
                                                        EnableViewState="true" onchange="javascript:CheckWeight(this);return false;" CssClass="alignrgt"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Charge/Rate" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTCharge" runat="server" Width="60px" MaxLength="5" Text='<%# Eval("Charge") %>'
                                                        EnableViewState="true" onchange="javascript:CheckCharge(this);return false;" CssClass="alignrgt"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cost" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTCost" runat="server" Width="60px" MaxLength="5" Text='<%# Eval("Cost") %>'
                                                        EnableViewState="true" onchange="javascript:CheckCharge(this);return false;" CssClass="alignrgt"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>                                         
                                        </Columns>
                                        <HeaderStyle CssClass="titlecolr" />
                                        <RowStyle HorizontalAlign="Center" />
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                </div>

                                </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="RemarksUP" runat="server">
                                <ContentTemplate>
                                
<div>
<table>

<tr>
<td valign="top"><strong>Remarks</strong></td>
<td><asp:TextBox ID="txtComment" runat="server" Rows="5" Columns="20" TextMode="MultiLine" Width="300px"/></td>

</tr>

</table>
</div>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                                
                                
                    </div>
                    
                </td>
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
                                                <asp:TextBox ID="TXTFlightCarrier" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="ImageButton3" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetFlightCarrier();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExFC" runat="server" Text="Exclude" GroupName="FC" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncFC" runat="server" Text="Include" GroupName="FC" />

                                            </td>
                                        </tr>
                                          <tr>
                                            <td>
                                                Issue Carrier
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTIssueCarrier" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="ImageButton4" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetIssueCarrier();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbExIS" runat="server" Text="Exclude" GroupName="IC" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="rbIncIS" runat="server" Text="Include" GroupName="IC" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Airline Code
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTAirLineCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBAC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirLineCode();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExAC" runat="server" Text="Exclude" GroupName="AC" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncAC" runat="server" Text="Include" GroupName="AC" />

                                            </td>
                                        </tr>
                                        </table></td></tr>
                                        <tr><td><table>
                                        <tr>
                                            <td style="width:150px;">
                                                Origin
                                            </td>
                                            <td style="width:264px;">
                                                <asp:TextBox ID="TXTParamORG" runat="server" Width="120px"></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirportORGCode();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExORG" runat="server" Text="Exclude" GroupName="OR" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncORG" runat="server" Text="Include" GroupName="OR" />

                                            </td>
                                        </tr>
                                        <tr>
                                             <td>
                                                 Country Source</td>
                                             <td>
                                                 <asp:TextBox ID="TXTParamCountry" runat="server" Width="120px"></asp:TextBox>
                                                 <asp:ImageButton ID="ImgCountry" runat="server" ImageAlign="AbsMiddle" 
                                                     ImageUrl="~/Images/list_bullets.png" 
                                                     OnClientClick="javascript:GetCountry();return false;" />
                                             </td>
                                             <td>
                                                 <asp:RadioButton ID="RBExCountry" runat="server" GroupName="ConS" Text="Exclude" />
                                                 &nbsp;&nbsp;
                                                 <asp:RadioButton ID="RBIncCountry" runat="server" GroupName="ConS" Text="Include" />
                                             </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                Destination
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTParamDest" runat="server" Width="120px"></asp:TextBox>

                                                 <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                        ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirportDestCode();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExDest" runat="server" Text="Exclude" GroupName="DT" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncDest" runat="server" Text="Include" GroupName="DT" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Country Destination</td>
                                           
                                            <td>
                                                <asp:TextBox ID="TXTParamCountryD" runat="server" Width="120px"></asp:TextBox>
                                                <asp:ImageButton ID="ImgCountry0" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetCountryD();return false;" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExCountryD" runat="server" GroupName="ConD" 
                                                    Text="Exclude" />
                                                    &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncCountryD" runat="server" GroupName="ConD" 
                                                    Text="Include" />
                                             </td>
                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                Flight Number
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TXTFlightNumber" runat="server" Width="120px" ></asp:TextBox>

                                                <asp:ImageButton ID="IBFN" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetFlightNumber();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExFN" runat="server" Text="Exclude" GroupName="FN" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncFN" runat="server" Text="Include" GroupName="FN" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:157px;">
                                                Days Of Week
                                            </td>
                                            <td style="width:268px;">
                                                <asp:CheckBoxList ID="cblWeekdays" runat="server" RepeatDirection="Horizontal">
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
                                                    Text="Exclude" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="rbWeekdaysInclude" runat="server" GroupName="Weekdays" 
                                                    Text="Include" />
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
                                                <asp:TextBox ID="TXTIATAComCode" runat="server" Width="120px"></asp:TextBox>

                                                <asp:ImageButton ID="IBCC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetIATAComCode();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExCC" runat="server" Text="Exclude" GroupName="CC" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncCC" runat="server" Text="Include" GroupName="CC" />

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
                                                <asp:TextBox ID="TXTHandlingCode" runat="server" Width="120px"></asp:TextBox>
                                                <asp:ImageButton ID="IBSHC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetSpecialHandlingCode();return false;" />

                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExHC" runat="server" Text="Exclude" GroupName="HC" />

                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncHC" runat="server" Text="Include" GroupName="HC" />

                                            </td>
                                        </tr>
                                          
                                         </table></td></tr>
                                         <tr><td><table >
                                         
                                        
                                        <tr>
                                            <td style="width:157px;">
                                                Handler</td>
                                            <td  style="width: 268px">
                                                <asp:TextBox ID="TXTParamHand" runat="server" Width="120px"></asp:TextBox>
                                                <asp:ImageButton ID="ImgHand" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetHandler();return false;" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExHand" runat="server" GroupName="Han" 
                                                    Text="Exclude" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncHand" runat="server" GroupName="Han" 
                                                    Text="Include" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                Equipment Type</td>
                                            <td>
                                                <asp:TextBox ID="TXTEquipmentType" runat="server" Width="120px"></asp:TextBox>
                                                <asp:ImageButton ID="imgEquipType" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetEquipType();return false;" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExET" runat="server" GroupName="ET" 
                                                    Text="Exclude" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncET" runat="server" GroupName="ET" 
                                                    Text="Include" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                OCCode</td>
                                            <td>
                                                <asp:TextBox ID="TxTOCCode" runat="server" Width="120px"></asp:TextBox>
                                                <asp:ImageButton ID="ImageButton5" runat="server" ImageAlign="AbsMiddle" 
                                                    ImageUrl="~/Images/list_bullets.png" 
                                                    OnClientClick="javascript:GetOtherChargeCode();return false;" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RBExOC" runat="server" GroupName="OC" 
                                                    Text="Exclude" />
                                                &nbsp;&nbsp;
                                                <asp:RadioButton ID="RBIncOC" runat="server" GroupName="OC" 
                                                    Text="Include" />
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
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add" OnClick="btnAdd_Click" />
                                                        &nbsp;
                                                        <asp:Button ID="btnDelete" runat="server" CssClass="button" Text="Delete" OnClick="btnDelete_Click" />
                                                    </td>
                                                </tr>
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
