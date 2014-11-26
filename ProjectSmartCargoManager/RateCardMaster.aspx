<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RateCardMaster.aspx.cs"
    MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.RateCardMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    <script type="text/javascript">


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





       
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
                </asp:ToolkitScriptManager>
   <div id="contentarea">
    <div>
        <div >
        <h1> 
           Rate Card <%--<img alt="" src="images/ratecard.gif" />--%>
        </h1>
        <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
        </p>
            <div class="botline">
                <table width="80%">
                   
                    <tr>
                        <td>
                            Rate Card Name
                        </td>
                        <td >
                            <asp:TextBox ID="txtRateCardName" runat="server" Width="120px"></asp:TextBox>
                            <span class="style63">*</span>
                        </td>
                        <td>
                            Rate Card Type
                        </td>
                        <td >
                            <asp:DropDownList ID="ddlRateCardType" runat="server" AutoPostBack="True" Width="120px">
                                <asp:ListItem Selected="True">IATA</asp:ListItem>
                                <asp:ListItem>Market</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                        <td>
                            Status
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="120px">
                                <asp:ListItem Selected="True">Active</asp:ListItem>
                                <asp:ListItem>Draft</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Valid From
                        </td>
                        <td >
                            <asp:TextBox ID="txtvalidfrom" runat="server" Width="120px"></asp:TextBox>
                            <span>*</span>
                            <asp:ImageButton ID="btnValidFrom" runat="server" ImageUrl="~/Images/calendar_2.png"
                                ImageAlign="AbsMiddle" />
                            <asp:CalendarExtender ID="CEValidFrom" Format="dd/MM/yyyy" TargetControlID="txtvalidfrom"
                                PopupButtonID="btnValidFrom" runat="server" PopupPosition="BottomLeft">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            &nbsp;Valid Till
                        </td>
                        <td >
                            <asp:TextBox ID="txtvalidtill" runat="server" Width="120px"></asp:TextBox>
                            <span class="style63">*</span>
                            <asp:ImageButton ID="imgBtnValidTill" runat="server" ImageUrl="~/Images/calendar_2.png"
                                ImageAlign="AbsMiddle" />
                            <asp:CalendarExtender ID="CalendarExtender1" Format="dd/MM/yyyy" TargetControlID="txtvalidtill"
                                PopupButtonID="imgBtnValidTill" runat="server" PopupPosition="BottomLeft">
                            </asp:CalendarExtender>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
             <br />
          
                <h2 >
                 Parameters
                </h2>
               
            <div class="divback">
         
                <table width="450">
                    <tr>
                        <td style="width: 100px">
                            Flight Number
                        </td>
                        <td >
                            <asp:TextBox ID="TXTFlightNumber" runat="server" Width="120px"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/list_bullets.png"
                                ImageAlign="AbsMiddle" OnClientClick="javascript:GetFlightNumber();return false;" />
                        </td>
                        <td style="width: 150px">
                            <asp:RadioButton ID="RBExFN" runat="server" Text="Exclude" GroupName="FN" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="RBIncFN" runat="server" Text="Include" GroupName="FN" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Flight Carrier
                        </td>
                        <td >
                            <asp:TextBox ID="TXTFlightCarrier" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td style="width: 150px">
                            <asp:RadioButton ID="RBExFC" runat="server" Text="Exclude" GroupName="FC" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="RBIncFC" runat="server" Text="Include" GroupName="FC" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Handling Code
                        </td>
                        <td >
                            <asp:TextBox ID="TXTHandlingCode" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td style="width: 150px">
                            <asp:RadioButton ID="RBExHC" runat="server" Text="Exclude" GroupName="HC" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="RBIncHC" runat="server" Text="Include" GroupName="HC" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Airline Code
                        </td>
                        <td >
                            <asp:TextBox ID="TXTAirLineCode" runat="server" Width="120px"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/Images/list_bullets.png"
                                ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirLineCode();return false;" />
                        </td>
                        <td style="width: 150px">
                            <asp:RadioButton ID="RBExAC" runat="server" Text="Exclude" GroupName="AC" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="RBIncAC" runat="server" Text="Include" GroupName="AC" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            IATA Comm. Code
                        </td>
                        <td >
                            <asp:TextBox ID="TXTIATAComCode" runat="server" Width="120px"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Images/list_bullets.png"
                                ImageAlign="AbsMiddle" OnClientClick="javascript:GetIATAComCode();return false;" />
                        </td>
                        <td style="width: 150px">
                            <asp:RadioButton ID="RBExCC" runat="server" Text="Exclude" GroupName="CC" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="RBIncCC" runat="server" Text="Include" GroupName="CC" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Agent Code
                        </td>
                        <td >
                            <asp:TextBox ID="TXTAgentCode" runat="server" Width="120px"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/Images/list_bullets.png"
                                ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
                        </td>
                        <td style="width: 150px">
                            <asp:RadioButton ID="RBExAD" runat="server" Text="Exclude" GroupName="AD" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="RBIncAD" runat="server" Text="Include" GroupName="AD" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Shipper Code
                        </td>
                        <td >
                            <asp:TextBox ID="TXTShipperCode" runat="server" Width="120px"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/Images/list_bullets.png"
                                ImageAlign="AbsMiddle" OnClientClick="javascript:GetShipperCode();return false;" />
                        </td>
                        <td style="width: 150px">
                            <asp:RadioButton ID="RBExSC" runat="server" Text="Exclude" GroupName="SC" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="RBIncSC" runat="server" Text="Include" GroupName="SC" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="fotbut">
            <table style="width: 930px;">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" />
                        &nbsp;<asp:Button ID="btnBack" runat="server" Text="Back" Visible="false" 
                            CssClass="button" onclick="btnBack_Click"/>
                        <asp:HiddenField ID="HidOrigin" runat="server" />
                        <asp:HiddenField ID="HidDest" runat="server" />
                        <asp:HiddenField ID="HidCurrency" runat="server" />
                        <asp:HiddenField ID="HidParam" runat="server" />
                        <asp:HiddenField ID="HidSrNo" runat="server" />
                    </td>
                </tr>
            </table>
            </div>
            <br />
        </div>
    </div>
    </div> 
</asp:Content>
