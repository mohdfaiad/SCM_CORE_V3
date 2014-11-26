<%--
  
  2012/05/24 vinayak
  2012-07-10 vinayak
  
--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaintainRates.aspx.cs"
    MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.MaintainRates" %>

<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="MKB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">




        function GetCurrency() {

            var TxtCurrencyClientObject = '<%= TXTCurrency.ClientID %>';
            var HidObject = '<%= HidCurrency.ClientID %>';

            window.open('ListDataView.aspx?Parent=MaintainRatesCUR&TargetTXT=' + TxtCurrencyClientObject + '&Hid=' + HidObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetEquipType() {
            var TxtClientObject = '<%= txtEquipmentType.ClientID %>';
            var value = document.getElementById('<%= txtEquipmentType.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=EquipType&TargetTXT=' + TxtClientObject+'&Values='+value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');

        }
        function GetSpecialHandlingCode() {

            var TxtClientObject = '<%= TXTHandlingCode.ClientID %>';
            var TxtClientValue = document.getElementById('ctl00_ContentPlaceHolder1_TXTHandlingCode').value;
//            var TxtClientValue = document.getElementById('ctl00_ContentPlaceHolder1_TabContainer_TBPParameters_TXTHandlingCode').value;
            var value = document.getElementById('ctl00_ContentPlaceHolder1_TXTHandlingCode').value;
            window.open('ListMultipleSpecialHandlingCode.aspx?TargetTXT=' + TxtClientObject + '&txtValue=' + TxtClientValue, '',
                        'left=' + (screen.availWidth / 5) + ',top=' + (screen.availHeight / 8) +
            ',width=800,height=550,toolbar=0,resizable=0');
        }

        function CheckComm() {

            var value = document.getElementById("<%= TXTComm.ClientID  %>").value;
            var regexp = /[0-9]*[.]?[0-9]*/g;

            value = value.trimEnd();
            value = value.trimStart();

            if (value.trim() == "")
                return;

            var result = regexp.exec(value);

            if ((result[0] == null) || (result[0].length != value.length)) {
                alert("Invalid Agent Comm.");
                document.getElementById("<%= TXTComm.ClientID  %>").value = "";
            }
            else if (parseFloat(result[0]) > 50) {
                alert("Invalid Agent Comm. must be less than 50.");
                document.getElementById("<%= TXTComm.ClientID  %>").value = "";
            }
        }

        function CheckMaxDiscount() {

            var value = document.getElementById("<%= TXTDiscount.ClientID  %>").value;
            var regexp = /[0-9]*[.]?[0-9]*/g;

            value = value.trimEnd();
            value = value.trimStart();

            if (value.trim() == "")
                return;

            var result = regexp.exec(value);

            if ((result[0] == null) || (result[0].length != value.length)) {
                alert("Invalid Max Discount");
                document.getElementById("<%= TXTDiscount.ClientID  %>").value = "";
            }
            else if (parseFloat(result[0]) > 50) {
                alert("Invalid Max Discount must be less than 50.");
                document.getElementById("<%= TXTDiscount.ClientID  %>").value = "";
            }
        }

        function CheckTax() {

            var value = document.getElementById("<%= TXTTax.ClientID  %>").value;
            var regexp = /[0-9]*[.]?[0-9]*/g;

            value = value.trimEnd();
            value = value.trimStart();

            if (value.trim() == "")
                return;

            var result = regexp.exec(value);

            if ((result[0] == null) || (result[0].length != value.length)) {
                alert("Invalid Tax");
                document.getElementById("<%= TXTTax.ClientID  %>").value = "";
            }
            else if (parseFloat(result[0]) > 20) {
                alert("Invalid Tax must be less than 20");
                document.getElementById("<%= TXTTax.ClientID  %>").value = "";
            }
        }




        function GetFlightNumber() {

            var TxtClientObject = '<%= TXTFlightNumber.ClientID %>';
            var value = document.getElementById('<%= TXTFlightNumber.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=FlightNumber&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetFlightCarrier() {

            var TxtClientObject = '<%= TXTFlightCarrier.ClientID %>';
            var value = document.getElementById('<%= TXTFlightCarrier.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=FlightCarrier&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetIssueingCarrier() {

            var TxtClientObject = '<%= TXTIssueingCarrier.ClientID %>';
            var value = document.getElementById('<%= TXTIssueingCarrier.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=FlightCarrier&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetAirLineCode() {

            var TxtClientObject = '<%= TXTAirLineCode.ClientID %>';
            var value = document.getElementById('<%= TXTAirLineCode.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=AirlineCode&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }

        function GetIATAComCode() {

            var TxtClientObject = '<%= TXTIATAComCode.ClientID %>';
            var value = document.getElementById('<%= TXTIATAComCode.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=CommCode&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetIATAComCodeSPLCOMM() {

            var TxtClientObject = '<%= splCommodity.ClientID %>';
            var value = document.getElementById('<%= splCommodity.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=CommCode&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        
         }

        function GetAgentCode() {

            var TxtClientObject = '<%= TXTAgentCode.ClientID %>';
            var value = document.getElementById('<%= TXTAgentCode.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=AgentCode&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetShipperCode() {

            var TxtClientObject = '<%= TXTShipperCode.ClientID %>';
            var value = document.getElementById('<%= TXTShipperCode.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=Shipper&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
                
        function CheckWeight(txt) {

            var value = txt.value;
            var regexp = /[0-9]*[.]?[0-9]*/g;

            value = value.trimEnd();
            value = value.trimStart();

            if (value.trim() == "")
                return;

            var result = regexp.exec(value);

            if ((result[0] == null) || (result[0].length != value.length)) {
                alert("Invalid Weight");
                txt.value = "";
            }

        }


        function CheckCharge(txt) {

            var value = txt.value;
            var regexp = /[0-9]*[.]?[0-9]*/g;

            value = value.trimEnd();
            value = value.trimStart();

            if (value.trim() == "")
                return;

            var result = regexp.exec(value);

            if ((result[0] == null) || (result[0].length != value.length)) {
                alert("Invalid Charge");
                txt.value = "";
            }

        }
        function CheckULDWeight(txt) {

            var value = txt.value;
            var regexp = /[0-9]*[.]?[0-9]*/g;

            value = value.trimEnd();
            value = value.trimStart();

            if (value.trim() == "")
                return;

            var result = regexp.exec(value);

            if ((result[0] == null) || (result[0].length != value.length)) {
                alert("Invalid Weight");
                txt.value = "";
            }

        }


        function CheckULDCharge(txt) {

            var value = txt.value;
            var regexp = /[0-9]*[.]?[0-9]*/g;

            value = value.trimEnd();
            value = value.trimStart();

            if (value.trim() == "")
                return;

            var result = regexp.exec(value);

            if ((result[0] == null) || (result[0].length != value.length)) {
                alert("Invalid Charge");
                txt.value = "";
            }

        }
        function GetFlightCarrier() {

            var TxtClientObject = '<%= TXTFlightCarrier.ClientID %>';
            var value = document.getElementById('<%= TXTFlightCarrier.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=FlightCarrier&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetProductType() {

            var TxtClientObject = '<%= TXTProductType.ClientID %>';
            var TxtClientValue = document.getElementById('ctl00_ContentPlaceHolder1_TXTProductType').value;

            window.open('ListMultipleSelectProductType.aspx?TargetTXT=' + TxtClientObject + '&txtValue=' + TxtClientValue , '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=350,height=300,toolbar=0,resizable=0');
        }
        function GetTransitStationCode() {

            var TxtClientObject = '<%= TXTTransitStation.ClientID %>';
            var TxtClientValue = document.getElementById('ctl00_ContentPlaceHolder1_TXTTransitStation').value;

            window.open('ListMulipleSelectTransitStation.aspx?TargetTXT=' + TxtClientObject + '&txtValue=' + TxtClientValue, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=350,height=300,toolbar=0,resizable=0');
        }
        function checknum() {
            var txtSPAMark = document.getElementById('ctl00_ContentPlaceHolder1_TXTSPAMarkup').value;
            //alert(txtSPAMark);
            if (isNaN(txtSPAMark)) {
                alert('Please Enter Number.');
            }
            
        }
//jayant
        function DivShow() {
            document.getElementById("divMap").style.display = 'block';
            document.getElementById("DivNormal").style.display = 'none';
         }

        function DivHide() {
            document.getElementById("divMap").style.display = 'none';
            document.getElementById("DivNormal").style.display = 'block';
         }
        function hideDiv(obj) {
            //alert("inside javascript");
            var qstr = '<%=Request.QueryString["cmd"]%>';
//            var qstr1 = '<%=Request.QueryString["RCName"]%>';
            //alert(qstr+qstr1);
        if (obj.checked ==false) {
            //            alert(qstr);
            DivHide();
            }
            else {
                //                alert('hello' + qstr);
                DivShow();
                
            
            }//end jayant code
//            

        }

        //window.onload(hideDiv());
        

        
    
    </script>
    
     <script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>
 <script type="text/javascript">

     $(function() {
     $('#<%=  DdlRateBasis.ClientID %>').change(function() {

             document.getElementById("divRelative").style.display = 'none';
             document.getElementById("DivNormal").style.display = 'block';
             document.getElementById("divParam").style.display = 'block';
             document.getElementById("HRate").style.display = 'block';
             
             
             if (this.value == "RC") {
                 document.getElementById("divRelative").style.display = 'block';
                 document.getElementById("DivNormal").style.display = 'none';
                 document.getElementById("divParam").style.display = 'none';
                 document.getElementById("HRate").style.display = 'none';
             }

         });
     });
 </script>

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
    </asp:ToolkitScriptManager>
    
    <div id="contentarea">
        <h1>
        Rate Line
        </h1>
        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
        <div class="botline">
            <table width="100%">
                <tr>
                    <td>
                        Rate Card Name
                    </td>
                    <td>
                        <asp:DropDownList ID="DdlRateCardName" runat="server" >
                        </asp:DropDownList>
                    </td>
                    <td colspan="6">
                    </td>
                </tr>
                <tr>
                    <td>
                        Origin*
                    </td>
                    <td>
                        <asp:DropDownList ID="DdlOriginLevel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlOriginLevel_SelectedIndexChanged">
                            <asp:ListItem Text="Airport" Selected="True" Value="A"></asp:ListItem>
                            <asp:ListItem Text="City" Value="C"></asp:ListItem>
                            <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                            <asp:ListItem Text="Country" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                        <asp:DropDownList ID="ddlOrigin" runat="server">
                        </asp:DropDownList>
                        
                    </td>
                    <td>
                        Destination*
                    </td>
                    <td>
                        <asp:DropDownList ID="DdlDestinationLevel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlDestinationLevel_SelectedIndexChanged">
                            <asp:ListItem Text="Airport" Selected="True" Value="A"></asp:ListItem>
                            <asp:ListItem Text="City" Value="C"></asp:ListItem>
                            <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                            <asp:ListItem Text="Country" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                        <asp:DropDownList ID="ddlDestination" runat="server">
                        </asp:DropDownList>
                        
                    </td>
                    <td>
                        Contr Ref
                    </td>
                    <td>
                        <asp:TextBox ID="TXTContrRef" runat="server" Width="60px" MaxLength="30"></asp:TextBox>
                    </td>
                    <td>
                        Currency
                    </td>
                    <td>
                     <asp:DropDownList ID="TXTCurrency" runat="server" Width="60px"></asp:DropDownList>
                        <%--<asp:TextBox ID="TXTCurrency" runat="server" Width="60px"></asp:TextBox>--%>
                       <%-- <span class="style63">*</span>
                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetCurrency();return false;" />--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        Status
                    </td>
                    <td>
                        <asp:DropDownList ID="DdlStatus" runat="server">
                            <asp:ListItem Text="Active" Value="ACT"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="INA"></asp:ListItem>
                            <asp:ListItem Text="Draft" Value="DRF"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        Rating Basis
                    </td>
                    <td>
                        <asp:DropDownList ID="DdlRateBasis" runat="server">
                            <asp:ListItem Text="Weight Break" Value="WB"></asp:ListItem>
                            <asp:ListItem Text="Flat Charge" Value="FC"></asp:ListItem>
                             <asp:ListItem Text="Piece Break" Value="PB"></asp:ListItem>
                             <asp:ListItem Text="Freight %" Value="WP"></asp:ListItem>
                             <asp:ListItem Text="Relative Charge" Value="RC"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        Valid From*
                    </td>
                    <td>
                        <asp:TextBox ID="TXTValidFrom" runat="server" Width="70px"></asp:TextBox>
                        
                        <asp:ImageButton ID="IBValidFrom" runat="server" ImageUrl="~/Images/calendar_2.png"
                            ImageAlign="AbsMiddle" />
                        <asp:CalendarExtender ID="CEValidFrom" Format="dd/MM/yyyy" TargetControlID="TXTValidFrom"
                            PopupButtonID="IBValidFrom" runat="server" PopupPosition="BottomLeft">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        Valid To*
                    </td>
                    <td>
                        <asp:TextBox ID="TXTValidTo" runat="server" Width="70px"></asp:TextBox>
                        
                        <asp:ImageButton ID="IBValidTo" runat="server" ImageUrl="~/Images/calendar_2.png"
                            ImageAlign="AbsMiddle" />
                        <asp:CalendarExtender ID="CEValidTo" Format="dd/MM/yyyy" TargetControlID="TXTValidTo"
                            PopupButtonID="IBValidTo" runat="server" PopupPosition="BottomLeft">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Agent Comm.
                    </td>
                    <td>
                        <asp:TextBox ID="TXTComm" runat="server" Width="60px" onchange="javascript:CheckComm();"
                            MaxLength="5" CssClass="alignrgt"></asp:TextBox>
                        %
                    </td>
                    <td>
                        Max Discount
                    </td>
                    <td>
                        <asp:TextBox ID="TXTDiscount" runat="server" Width="60px" onchange="javascript:CheckMaxDiscount();"
                            MaxLength="5" CssClass="alignrgt"></asp:TextBox>
                        %
                    </td>
                    <td>
                        Tax.*
                    </td>
                    <td>
                        <asp:TextBox ID="TXTTax" runat="server" Width="60px" onchange="javascript:CheckTax();"
                            MaxLength="5" CssClass="alignrgt"></asp:TextBox>
                        <span>%</span>
                    </td>
                    <td>
                        All-In Rate
                    </td>
                    <td>
                        <asp:CheckBox ID="ckhAllIn" runat="server"></asp:CheckBox>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        TACT Rate</td>
                    <td>
                        <asp:CheckBox ID="ckhTACTRate" runat="server" ></asp:CheckBox>
                    </td>
                    <td>
                        ULD Rate</td>
                    <td>
                        <asp:CheckBox ID="ckhULDRate" runat="server" 
                            oncheckedchanged="ckhULDRate_CheckedChanged" onClick="hideDiv(this)"></asp:CheckBox>
                    </td>
                    <td colspan="2" align="left">
                        Heavy Applicable<asp:CheckBox ID="ckhHeavyRate" runat="server" ></asp:CheckBox>
                    </td>
                    <td>
                        GL Account Code</td>
                    <td>
                        <asp:DropDownList ID="ddlGLCode" runat="server">
                        </asp:DropDownList>
                    </td>
                    
                </tr>
                <tr>
                 <td>
                                
                                Rate Type</td>
                            <td>
                            
                                <asp:DropDownList ID="ddlRateType" runat="server">
                                  <asp:ListItem Value="">ALL</asp:ListItem>
                                 <asp:ListItem Value="DOM">Domestic</asp:ListItem>
                                    <asp:ListItem Value="INT">International</asp:ListItem>
                                  
                                </asp:DropDownList>
                            
                            </td>
                </tr>
            </table>
        </div>
      
        <div style="float:left; width:100%;" id="DIVFull">
            <table width="100%" cellpadding="6" cellspacing="0">
              <tr>
                <td valign="top">
                      <div id="HRate">
                        <h2>
                            Rate Base
                        </h2>
                        </div>
                        <asp:UpdatePanel ID="UPSlabs" runat="server">
                            <ContentTemplate>
                                <div class="divback" id="DivNormal">
                                  
                                    <asp:GridView ID="GRDRateSlabs" runat="server" AutoGenerateColumns="False" ShowFooter="True"
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
                                                        <asp:ListItem Text="F" Value="F"></asp:ListItem>                                                        
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weight" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTWeight" runat="server" Width="60px" MaxLength="5" Text='<%# Eval("Weight") %>'
                                                        EnableViewState="true" onchange="javascript:CheckWeight(this);return false;"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Charge/Rate" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTCharge" runat="server" Width="60px" MaxLength="5" Text='<%# Eval("Charge") %>'
                                                        EnableViewState="true" onchange="javascript:CheckCharge(this);return false;"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Cost" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TXTCost" runat="server" Width="60px" MaxLength="5" Text='<%# Eval("Cost") %>'
                                                        EnableViewState="true"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Based" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DdlBased" runat="server" EnableViewState="true">
                                                        <asp:ListItem Text=" " Value="B"></asp:ListItem>
                                                        <asp:ListItem Text="IATA" Value="I"></asp:ListItem>   
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="titlecolr" />
                                        <RowStyle HorizontalAlign="Center" />
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                    <table style="width:100%;">
                                        <tr>
                                            <td>
                                                <%--<asp:LinkButton ID="LBNAdd" runat="server" Text="Add" OnClick="LBNAdd_OnClick"></asp:LinkButton>--%>
                                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" OnClick="LBNAdd_OnClick" />
                                                &nbsp;
                                                <%--<asp:LinkButton ID="LBNDelete" runat="server" Text="Delete" OnClick="LBNDelete_OnClick"></asp:LinkButton>--%>
                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="LBNDelete_OnClick"
                                                    CssClass="button" />
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </div>
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                        
                        <asp:UpdatePanel ID="ULDPanel" runat="server">
                            <ContentTemplate>
                                <div class="divback" id="divMap" >
                                    
                                    <asp:GridView ID="grdULDslabs" runat="server" AutoGenerateColumns="False" 
                                    ShowFooter="True" Width="100%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CHKULD" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="ULDType">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DdlULDType" runat="server" EnableViewState="true">
                                                    <asp:ListItem Text="AKE" Value="AKE"></asp:ListItem>
                                                    <asp:ListItem Text="UKA" Value="UKA"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Type">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DdlULDType1" runat="server" EnableViewState="true">
                                                    <asp:ListItem Text="M" Value="M"></asp:ListItem>
                                                    <asp:ListItem Text="OverPivot" Value="OverPivot"></asp:ListItem>
                                                    
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Weight">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTULDWeight" runat="server" CssClass="alignrgt" 
                                                    EnableViewState="true" MaxLength="5" 
                                                    onchange="javascript:CheckULDWeight(this);return false;" 
                                                    Text='<%# Eval("Weight") %>' Width="60px"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Charge/Rate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTULDCharge" runat="server" CssClass="alignrgt" 
                                                    EnableViewState="true" MaxLength="5" 
                                                    onchange="javascript:CheckULDCharge(this);return false;" 
                                                    Text='<%# Eval("Charge") %>' Width="60px"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="titlecolr" />
                                    <RowStyle HorizontalAlign="Center" />
                                    <AlternatingRowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                                <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <%--<asp:LinkButton ID="LBNAdd" runat="server" Text="Add" OnClick="LBNAdd_OnClick"></asp:LinkButton>--%>
                                                <asp:Button ID="btnULDAdd" runat="server" Text="Add" CssClass="button" 
                                                    onclick="btnULDAdd_Click" />
                                                &nbsp;
                                                <%--<asp:LinkButton ID="LBNDelete" runat="server" Text="Delete" OnClick="LBNDelete_OnClick"></asp:LinkButton>--%>
                                                <asp:Button ID="btnULDDel" runat="server" Text="Delete"
                                                    CssClass="button" onclick="btnULDDel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top">
                        <h2>Parameter</h2>
                     <div style="width:100%; border:1px solid #ccc;" id="divParam">
                        <table width="100%" cellpadding="3" cellspacing=3>
                        <tr><td>
                        <table style="background:#f6f6f6; width:100%;" cellpadding="3" cellspacing="3">
                        <tr><td>
                             Flight Carrier
                            </td>
                             <td style="width:264px;">
                                            <asp:TextBox ID="TXTFlightCarrier" runat="server" Width="120px" 
                                                ontextchanged="TXTFlightCarrier_TextChanged" AutoPostBack="True"></asp:TextBox>
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                ImageAlign="AbsMiddle" OnClientClick="javascript:GetFlightCarrier();return false;" />  <%--OnClientClick="javascript:GetFlightCarrier();return false;" --%>
                             </td>
                             <td>
                                            <asp:RadioButton ID="RBExFC" runat="server" Text="Exclude" GroupName="FC" />
                                            
                                            <asp:RadioButton ID="RBIncFC" runat="server" Text="Include" GroupName="FC" 
                                                oncheckedchanged="RBIncFC_CheckedChanged" AutoPostBack="True" />
                            </td>
                         </tr>
                         <tr>
                                <td>
                                    Issuing Carrier
                                </td>
                                <td>
                                            <asp:TextBox ID="TXTIssueingCarrier" runat="server" Width="120px" 
                                                ></asp:TextBox>
                                            <asp:ImageButton ID="ImgIssue" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                ImageAlign="AbsMiddle" 
                                        OnClientClick="javascript:GetIssueingCarrier();return false;" />  
                                </td>
                                <td>
                                    <asp:RadioButton ID="RBExIC" runat="server" GroupName="IC" Text="Exclude" />
                                    <asp:RadioButton ID="RBIncIC" runat="server" GroupName="IC" Text="Include" />
                                </td>
                         </tr>
                        
                        <tr>
                                    <td>
                                            Proration %</td>
                                    <td>
                                            <asp:TextBox ID="txtProPer" runat="server" Width="120px" Enabled = "false"></asp:TextBox>
                                    </td>
                                    <td>
                                            <asp:RadioButton ID="RBExPro" runat="server" Text="Exclude" GroupName="PRO" Visible= "false" Enabled = 'false'/>
                                            
                                            <asp:RadioButton ID="RBInPro" runat="server" Text="Include" GroupName="PRO" Visible= "false" Enabled = 'false'/>
                                    </td>
                          </tr>
                          <tr>
                                <td>
                                    SPA Markup(%)</td>
                                <td>
                                    <asp:TextBox ID="TXTSPAMarkup" runat="server" Width="120px" onChange="checknum()" MaxLength="2"></asp:TextBox>&nbsp;</td>
                                <td>
                                    &nbsp;</td>
                           </tr>
                         </table>
                        </td></tr>
                        <tr><td>
                        <table width="100%" cellpadding="3" cellspacing="3" >
                            <tr>
                                <td style="width:164px;">
                                    Flight Number
                                </td>
                                <td style="width:264px;">
                                    <asp:TextBox ID="TXTFlightNumber" runat="server" Width="120px"></asp:TextBox>
                                    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/list_bullets.png"
                                        ImageAlign="AbsMiddle" OnClientClick="javascript:GetFlightNumber();return false;" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="RBExFN" runat="server" Text="Exclude" GroupName="FN" />
                                    
                                    <asp:RadioButton ID="RBIncFN" runat="server" Text="Include" GroupName="FN" />
                                </td>
                            </tr>
                            
                           
                             
                              
               
                
                 
                  <tr>
                                <td>
                                    Days Of Week
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="cblWeekdays" runat="server" RepeatDirection="Horizontal" Width="70%">
                                        <asp:ListItem Text="Mon" Value="Mon" />
                                        <asp:ListItem Text="Tue" Value="Tue" />
                                        <asp:ListItem Text="Wed" Value="Wed" />
                                        <asp:ListItem Text="Thu" Value="Thu" />
                                        <asp:ListItem Text="Fri" Value="Fri" />
                                        <asp:ListItem Text="Sat" Value="Sat" />
                                        <asp:ListItem Text="Sun" Value="Sun" />
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbWeekdaysExclude" runat="server" Text="Exclude" GroupName="Weekdays" />
                                    
                                    <asp:RadioButton ID="rbWeekdaysInclude" runat="server" Text="Include" GroupName="Weekdays" />
                                </td>
                            </tr>                 
                <tr>
                                <td>
                                    Dep. Interval
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                From
                                            </td>
                                            <td>
                                                <MKB:TimeSelector ID="CtlTimeFrom" runat="server" DisplaySeconds="False" SelectedTimeFormat="TwentyFour">
                                                </MKB:TimeSelector>
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                To
                                            </td>
                                            <td>
                                                <MKB:TimeSelector ID="CtlTimeTo" runat="server" DisplaySeconds="False" SelectedTimeFormat="TwentyFour">
                                                </MKB:TimeSelector>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    
                                    
                                    <asp:RadioButton ID="rbDepIntExclude" runat="server" Text="Exclude" GroupName="DepInt" />
                                    
                                    <asp:RadioButton ID="rbDepIntInclude" runat="server" Text="Include" GroupName="DepInt" />
                                </td>
                            </tr>
               
                
                <tr>
                                <td>
                                    Transit Station</td>
                                <td>
                                    <asp:TextBox ID="TXTTransitStation" runat="server" Width="120px"></asp:TextBox>
                                    <asp:ImageButton ID="IAPC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetTransitStationCode();return false;" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="RBExTS" runat="server" GroupName="TS" Text="Exclude" />
                                   
                                    <asp:RadioButton ID="RBIncTS" runat="server" GroupName="TS" Text="Include" />
                                </td>
                            </tr></table>
                        </td></tr>
                        <tr><td>
                        <table style="background:#f6f6f6; width:100%" cellpadding="3" cellspacing="3">
                        <tr>
                                        <td style="width:164px;">
                                            Agent Code
                                        </td>
                                        <td style="width:264px;">
                                            <asp:TextBox ID="TXTAgentCode" runat="server" Width="120px"></asp:TextBox>
                                            <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RBExAD" runat="server" Text="Exclude" GroupName="AD" />
                                          
                                            <asp:RadioButton ID="RBIncAD" runat="server" Text="Include" GroupName="AD" />
                                        </td>
                                        
                                        
                                        
                                    </tr>
                         <tr>
                        <td>
                            Shipper Code
                        </td>
                        <td>
                            <asp:TextBox ID="TXTShipperCode" runat="server" Width="120px"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/list_bullets.png"
                                ImageAlign="AbsMiddle" OnClientClick="javascript:GetShipperCode();return false;" />
                        </td>
                        <td>
                            <asp:RadioButton ID="RBExSC" runat="server" Text="Exclude" GroupName="SC" />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RBIncSC" runat="server" Text="Include" GroupName="SC" />
                        </td>
                    </tr></table>
                        </td></tr>
                        <tr><td>
                         <table width="100%" cellpadding="3" cellspacing="3" >
            <tr>
                                <td style="width:164px;">
                                    IATA Comm. Code
                                </td>
                                <td style="width:264px;">
                                    <asp:TextBox ID="TXTIATAComCode" runat="server" Width="120px"></asp:TextBox>
                                    <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Images/list_bullets.png"
                                        ImageAlign="AbsMiddle" OnClientClick="javascript:GetIATAComCode();return false;" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="RBExCC" runat="server" Text="Exclude" GroupName="CC" />
                                    
                                    <asp:RadioButton ID="RBIncCC" runat="server" Text="Include" GroupName="CC" />
                                </td>
                            </tr>
                            
                            <tr style="display:none" >
                    <td>
                        Airline Code
                    </td>
                    <td >
                        <asp:TextBox ID="TXTAirLineCode" runat="server" Width="120px"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetAirLineCode();return false;" />
                    </td>
                    <td>
                        <asp:RadioButton ID="RBExAC" runat="server" Text="Exclude" GroupName="AC" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="RBIncAC" runat="server" Text="Include" GroupName="AC" />
                    </td>
                </tr>
                            
                            
                <tr>
                                <td>
                                    Product Type</td>
                                <td class="style1">
                                    <asp:TextBox ID="TXTProductType" runat="server" Width="120px"></asp:TextBox>
                                    <asp:ImageButton ID="IProd" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" OnClientClick="javascript:GetProductType();return false;" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="RBExPT" runat="server" GroupName="PT" Text="Exclude" />
                                   
                                    <asp:RadioButton ID="RBIncPT" runat="server" GroupName="PT" Text="Include" />
                                </td>
                            </tr>
                 <tr>
                                <td>
                                    SPL Handling Code
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="TXTHandlingCode" runat="server" Width="120px"></asp:TextBox>
                                    <asp:ImageButton ID="ISHC" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle"
                                    OnClientClick="javascript:GetSpecialHandlingCode();return false;" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="RBExHC" runat="server" Text="Exclude" GroupName="HC" />
                                    
                                    <asp:RadioButton ID="RBIncHC" runat="server" Text="Include" GroupName="HC" />
                                </td>
                            </tr>
                             <tr style="visibility:hidden">
                    <td>
                       Equipment Type
                    </td>
                    <td>
                        <asp:TextBox ID="txtEquipmentType" runat="server" Width="120px"></asp:TextBox>
                        <asp:ImageButton ID="ImgEquipmentList" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetEquipType();return false;" />
                    </td>
                    <td>
                        <asp:RadioButton ID="RBExET" runat="server" Text="Exclude" GroupName="ET" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="RBIncET" runat="server" Text="Include" GroupName="ET" />
                    </td>
                </tr>
                
               </table>
                        </td></tr>
                       </table>
                     </div>                 
                    </td>
                </tr>
                <tr><td>
                       <asp:UpdatePanel ID="Relative" runat="server">
                       <ContentTemplate>
                       <div id="divRelative" style="display:none;">
                        <table>
                        <tr>
                        <td>
                            Commodity Code
                        </td>
                        <td>
                           <asp:TextBox ID="splCommodity" runat="server" Width="120px"></asp:TextBox>
                            <asp:ImageButton ID="ImgSPC" runat="server" ImageUrl="~/Images/list_bullets.png"
                                ImageAlign="AbsMiddle" OnClientClick="javascript:GetIATAComCodeSPLCOMM();return false;" />                                   
                                              
                        </td>
                        </tr>
                        <tr>
                        <td>
                            Minimum
                        </td>
                        <td>
                            <asp:TextBox ID="txtRelativeMin" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                        <td>
                            Charge (%)
                        </td>
                        <td>
                            <asp:TextBox ID="txtRelativePer" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                        <td>
                            Applied ON
                        </td>
                        <td>
                             <asp:DropDownList ID="ddlAppliedOn" runat="server" Width="50px">
                                            <asp:ListItem Text="M" Value="M"></asp:ListItem>
                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="Q" Value="Q"></asp:ListItem>                            
                                        </asp:DropDownList>
                        </td>
                        </tr>
                        </table>
                        </div>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                </td></tr>               
                
            </table>
                <div>
                <table>
                <tr>
                <td>Remarks<br />
                <asp:TextBox ID="txtComment" runat="server" Rows="5" Columns="20" TextMode="MultiLine" Width="314px" Height="68px"/></td>
                <td>
                <div style="overflow:scroll; height:56px; width:700px;" >
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
                <%--<tr style="background-color:#EBEFF0">
                <td>
                <table style="background-color:#EBEFF0;border-top:1px dotted #1F2B53; " >

                </table>
                </td>
                </tr>--%>
                <tr>
                <td>
                <asp:Label ID="lblComment" runat="server" Text='<%#Eval("comments") %>'/>
                </td>
                </tr>
                <tr>
                <td>
                <table style="background-color:#EBEFF0;border-top:1px dotted #1F2B53;border-bottom:1px solid #1F2B53; width:100%;" >
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
                </div></td>
                </tr>

                </table>
                </div>   
         </div>
            
     
        
        <div id="fotbut">
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnAddAnother" runat="server" Text="Add Another" CssClass="button" OnClick="btnAddAnother_Click" />
                        &nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button"/>
                        &nbsp;<asp:Button ID="btnBack" runat="server" Text="Back" CssClass="button" OnClick="btnBack_Click" />
                        <asp:HiddenField ID="HidOrigin" runat="server" />
                        <asp:HiddenField ID="HidDest" runat="server" />
                        <asp:HiddenField ID="HidCurrency" runat="server" />
                        <asp:HiddenField ID="HidParam" runat="server" />
                        <asp:HiddenField ID="HidSrNo" runat="server" />
                    </td>
                </tr>
                
            </table>
            <div class="divback" >
            <asp:UpdatePanel ID="UPSecond" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdListStock" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                        Width="100%" AllowPaging="true" OnPageIndexChanging="grdListStock_PageIndexChanging"
                        PageSize="10" Height="10px" HeaderStyle-CssClass="HeaderStyle"  CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle" PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                        <Columns>
                           <%-- <asp:BoundField DataField="SerialNumber" HeaderText="" ItemStyle-ForeColor="White" Visible="false"
                                ItemStyle-BorderColor="Black">
                                <ItemStyle Width="0px" />
                            </asp:BoundField>--%>
                           
                            <asp:BoundField DataField="OriginLevel" HeaderText="Origin Level" />
                            <asp:BoundField DataField="Origin" HeaderText="Origin" />
                            <asp:BoundField DataField="DestinationLevel" HeaderText="Destination Level" />
                            <asp:BoundField DataField="Destination" HeaderText="Destination" />
                           <asp:BoundField DataField="RateCardSrNo" HeaderText="Rate Card Name" />
                            <asp:BoundField DataField="RateLineParams" HeaderText="Parameter">
                            <ItemStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RateLineSlabs" HeaderText="Slabs">
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="ContrRef" HeaderText="Contr Ref" />--%>
                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" />
                            <asp:BoundField DataField="EndDate" HeaderText="End Date" />
                            <asp:BoundField DataField="RateBase" HeaderText="Rate Base" />
                            <asp:BoundField DataField="AgentCommPercent" HeaderText="Agent Commision Percent">
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="MaxDiscountPercent" HeaderText="Max Discount Percent" />--%>
                            <asp:BoundField DataField="ServiceTax" HeaderText="Service Tax" />
                            
                            <%--<asp:BoundField DataField="TDSPercent" HeaderText="TDS Percent" />--%>
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="UpdatedOn" HeaderText="UpdatedOn" />
                            <asp:BoundField DataField="UpdatedBy" HeaderText="UpdatedBy" />
                            
                        </Columns>
                        <HeaderStyle CssClass="titlecolr" />
                        <RowStyle HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
                </ContentTemplate>
               
            </asp:UpdatePanel>
        </div>
        </div>
    </div>
</asp:Content>
