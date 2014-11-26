<%--

   2012-05-10 vinayak


--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true"
    CodeBehind="SpotRateTransaction.aspx.cs" Inherits="ProjectSmartCargoManager.SpotRateTransaction" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contentarea">
        <h1>
            <%--<img alt="" src="Images/flightshnew.png" style="vertical-align: 5" />--%>
            Spot Rate Management
        </h1>
        <div class="botline">
            <asp:ToolkitScriptManager ID="TSM0" runat="server">
            </asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="UPanelSearch" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="PNL" runat="server" GroupingText="Search" Width="830px">
                        <br />
                        &nbsp;&nbsp;
                        <asp:Label ID="LBLStatus" runat="server" ForeColor="Red"></asp:Label>
                        <br />
                        &nbsp;&nbsp; <span style="font-size: 14px; font-weight: bold;"><u>General Search</u></span>
                        <br />
                        &nbsp;&nbsp;
                        <div style="width: 700px;" class="divback">
                            <table style="width: 680px">
                                <tr>
                                    <td width="30px">
                                    </td>
                                    <td align="left">
                                        Origin
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="DDLOrigin">
                                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td align="left">
                                        Destination
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="DDLDestination">
                                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td align="left">
                                        Agent Code
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="DDLAgentCode">
                                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td align="left">
                                        Flight
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="DDLFlight">
                                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30px">
                                    </td>
                                    <td align="left">
                                        From Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTFromDate" runat="server" Width="90px"></asp:TextBox>
                                        <asp:ImageButton ID="btnFromDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                            ImageAlign="AbsMiddle" />
                                        <asp:CalendarExtender ID="CEFromDate" Format="yyyy-MM-dd" TargetControlID="TXTFromDate"
                                            PopupButtonID="btnFromDate" runat="server" PopupPosition="BottomLeft">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td align="left">
                                        To Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTToDate" runat="server" Width="90px"></asp:TextBox>
                                        <asp:ImageButton ID="btnToDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                            ImageAlign="AbsMiddle" />
                                        <asp:CalendarExtender ID="CEToDate" Format="yyyy-MM-dd" TargetControlID="TXTToDate"
                                            PopupButtonID="btnToDate" runat="server" PopupPosition="BottomLeft">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td align="right">
                                    </td>
                                    <td>
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td align="right">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30px">
                                    </td>
                                    <td colspan="11" height="1px" style="border-bottom: solid 1px gray">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30px">
                                    </td>
                                    <td colspan="11" align="right">
                                        <asp:Button ID="btnSearchGeneral" runat="server" Text="Fetch" CssClass="button" Width="60px" />
                                        &nbsp;
                                        <asp:Button ID="btnClearGeneral" runat="server" Text="Clear" CssClass="button" Width="60px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        &nbsp;&nbsp; <span style="font-size: 14px; font-weight: bold;"><u>Specific Search</u></span>
                        <br />
                        &nbsp;&nbsp;
                        <div style="width: 700px;" class="divback">
                            <table style="width: 680px">
                                <tr>
                                    <td width="30px">
                                    </td>
                                    <td align="left">
                                        AWBNumber
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="TXTAWBNumber" Width="114px" OnTextChanged="TXTAWBNumber_TextChanged"
                                            AutoPostBack="true"></asp:TextBox>
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td align="left">
                                        Comm.Code
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="DDLCommodityCode">
                                            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td width="300px">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30px">
                                    </td>
                                    <td colspan="6" height="1px" style="border-bottom: solid 1px gray">
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30px">
                                    </td>
                                    <td colspan="6" align="right">
                                        <asp:Button ID="btnSearchSpecific" runat="server" Text="Fetch" CssClass="button"
                                            Width="60px" OnClick="btnSearchSpecific_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnClearSpecific" runat="server" Text="Clear" CssClass="button" Width="60px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <br />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            <h2>
                AWB Details
            </h2>
            <div class="divback" style="width: 800px">
                <asp:UpdatePanel ID="UPAWBDetails" runat="server">
                    <ContentTemplate>
                        <table frame="void">
                            <tr>
                                <td width="20px">
                                </td>
                                <td valign="top" style="vertical-align: top; width: 700px">
                                    <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" ID="GRDAWBDetails" EnableViewState="true"
                                        Width="700px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CHKSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AWB Number">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="LBLAWBNumber" Text='<%# Eval("AWBNumber") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Commodity Code">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="LBLCommCode" Text='<%# Eval("CommCode") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Freight IATA">
                                                <ItemTemplate>
                                                    <asp:Label ID="LBLFrIATA" runat="server" Text='<%# Eval("FrIATA") %>' Width="60px" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Freight Mkt">
                                                <ItemTemplate>
                                                    <asp:Label ID="LBLFrMKT" runat="server" Text='<%# Eval("FrMKT") %>' Width="60px" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OC Due Car">
                                                <FooterTemplate>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="LBLOcDueCar" runat="server" Text='<%# Eval("OcDueCar") %>' Width="65px"
                                                        ReadOnly="True" />
                                                </ItemTemplate>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OC Due Agent">
                                                <FooterTemplate>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="LBLOcDueAgent" runat="server" Enabled="false" Text='<%# Eval("OcDueAgent") %>'
                                                        Width="65px" ReadOnly="True" />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="False" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Spot Rate">
                                                <FooterTemplate>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="LBLSpotRate" runat="server" Text='<%# Eval("SpotRate") %>' Width="65px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dyn Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="LBLDynRate" runat="server" Text='<%# Eval("DynRate") %>' Width="65px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serv Tax">
                                                <ItemTemplate>
                                                    <asp:Label ID="LBLServiceTax" runat="server" Text='<%# Eval("ServTax") %>' Width="65px" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                                        <FooterStyle CssClass="grdrowfont"></FooterStyle>
                                        <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                                        <RowStyle CssClass="grdrowfont"></RowStyle>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearchSpecific" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <br />
            <br />
            <asp:Panel ID="PNLSpotRate" runat="server" GroupingText="Spot Rate Details" Width="810px">
                <br />
                &nbsp;&nbsp; <span style="font-size: 14px; font-weight: bold;"><u>Spot Rate</u></span>
                <div class="divback" style="width: 800px">
                    <table width="800px">
                        <tr>
                            <td width="30px">
                            </td>
                            <td>
                                Spot Category
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="DDLSpotRateCategory">
                                    <asp:ListItem Text="Spot Rate" Value="S"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                Spot Rate<span style=" color:Red" >*</span>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="TXTSpotRate" Width="100px"></asp:TextBox>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                Currency<span style=" color:Red" >*</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="DDLCurrency">
                                    <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                    <asp:ListItem Text="INR" Value="INR"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                Station<span style=" color:Red" >*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLStation" runat="server">
                                    <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="5px">
                            </td>
                        </tr>
                        <tr>
                            <td width="30px">
                            </td>
                            <td>
                                Req Date<span style=" color:Red" >*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="TXTReqDate" runat="server" Width="90px"></asp:TextBox>
                                <asp:ImageButton ID="btnReqDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CEregDate" Format="yyyy-MM-dd" TargetControlID="TXTReqDate"
                                    PopupButtonID="btnReqDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                FWD Name
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="TXTFWDName" Width="130px"></asp:TextBox>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                Remarks
                            </td>
                            <td colspan="4">
                                <asp:TextBox runat="server" ID="TXTRemarks" TextMode="MultiLine" Width="200px" Height="39px"></asp:TextBox>
                            </td>
                            <td width="5px">
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                &nbsp;&nbsp; <span style="font-size: 14px; font-weight: bold;"><u>Carrier Details</u></span>
                <div class="divback" style="width: 800px">
                    <table width="800px">
                        <tr>
                            <td width="30px">
                            </td>
                            <td>
                                Issued By
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="TXTIssuedBy" Width="100px"></asp:TextBox>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                Issued Date
                            </td>
                            <td width="130px">
                                <asp:TextBox runat="server" ID="TXTIssuedDate" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="btnIssuedDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CEIssuedDate" Format="yyyy-MM-dd" TargetControlID="TXTIssuedDate"
                                    PopupButtonID="btnIssuedDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                Authorized By
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="TXTAuthorizedBy" Width="100px"></asp:TextBox>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                Authorized Date
                            </td>
                            <td width="130px">
                                <asp:TextBox runat="server" ID="TXTAuthorizedDate" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="btnAuthorizedDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CEAuthorizedDate" Format="yyyy-MM-dd" TargetControlID="TXTAuthorizedDate"
                                    PopupButtonID="btnAuthorizedDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td width="5px">
                            </td>
                        </tr>
                        <tr>
                            <td width="30px">
                            </td>
                            <td>
                                Valid From
                            </td>
                            <td width="130px">
                                <asp:TextBox ID="TXTValidFrom" runat="server" Width="90px"></asp:TextBox>
                                <asp:ImageButton ID="btnValidFrom" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CEValidFrom" Format="yyyy-MM-dd" TargetControlID="TXTValidFrom"
                                    PopupButtonID="btnValidFrom" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                                Valid To
                            </td>
                            <td width="130px">
                                <asp:TextBox ID="TXTValidTo" runat="server" Width="90px"></asp:TextBox>
                                <asp:ImageButton ID="btnValidTo" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CEValidTo" Format="yyyy-MM-dd" TargetControlID="TXTValidTo"
                                    PopupButtonID="btnValidTo" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td width="5px">
                            </td>
                            <td>
                            </td>
                            <td colspan="4">
                            </td>
                            <td width="5px">
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <br />
            <table width="800px">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
                            Width="60px" onclick="btnSave_Click" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" Width="60px" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
