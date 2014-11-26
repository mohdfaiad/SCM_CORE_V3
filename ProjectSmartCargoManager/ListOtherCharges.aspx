<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true"
    CodeBehind="ListOtherCharges.aspx.cs" Inherits="ProjectSmartCargoManager.ListOtherCharges" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>

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
        function GetAgentCode() {

            var TxtClientObject = '<%= txtagent.ClientID %>';
            var value = document.getElementById('<%=txtagent.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=AgentCode&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetShipperCode() {

            var TxtClientObject = '<%= txtShipper.ClientID %>';
            var value = document.getElementById('<%=txtShipper.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=Shipper&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetIATAComCode() {

            var TxtClientObject = '<%= txtComm.ClientID %>';
            var value = document.getElementById('<%=txtComm.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=CommCode&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetProductType() {
            var TXTPTClobject = '<%=txtproduct.ClientID %>';

            window.open('ListMultipleSelectProductType.aspx?Parent=MaintainRatesORG&level=A&param=Origin&TargetTXT=' + TXTPTClobject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=GRDList.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
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
    <asp:UpdatePanel runat="server" ID="updtPnl">
        <ContentTemplate>
            <div id="contentarea">
                <div class="botline">
                    <h2>
                        List Other Charges
                    </h2>
                </div>
                <p>
                    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
                </p>
                <div class="botline">
                    <table style="width: 100%; height: 100%" border="0">
                        <tr>
                            <td>
                                Origin Level
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLOriginLevel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLOriginLevel_SelectedIndexChanged">
                                    <asp:ListItem Text="Airport" Selected="True" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="City" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp; Origin
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOrigin" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Destination Level
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLDestinationLevel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLDestinationLevel_SelectedIndexChanged">
                                    <asp:ListItem Text="Airport" Selected="True" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="City" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Destination"></asp:Label>
                                &nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDestination" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Status
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server">
                                    <asp:ListItem Value="All" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="ACT">Active</asp:ListItem>
                                    <asp:ListItem Value="INACT">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Charge Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargeName" runat="server" Width="70px"></asp:TextBox>
                            </td>
                            <td>
                                Parameter
                            </td>
                            <td>
                                <asp:TextBox ID="txtParam" runat="server" Width="90px"></asp:TextBox>
                            </td>
                            <td>
                                From Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="70px"></asp:TextBox>
                                <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="btnFromDate" PopupPosition="BottomLeft" TargetControlID="txtFromDate">
                                </asp:CalendarExtender>
                                <asp:ImageButton ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                            </td>
                            <td>
                                To Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" Width="70px"></asp:TextBox>
                                <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                    PopupButtonID="btnToDate" PopupPosition="BottomLeft" TargetControlID="txtToDate">
                                </asp:CalendarExtender>
                                <asp:ImageButton ID="btnToDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                            </td>
                            <td>
                                Charge Type
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLChargeType" runat="server">
                                    <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                                    <asp:ListItem Text="Due Agent" Value="DA"></asp:ListItem>
                                    <asp:ListItem Text="Due Carrier" Value="DC"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Agent Code
                            </td>
                            <td>
                                <asp:TextBox ID="txtagent" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="IBAD" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle"
                                    OnClientClick="javascript:GetAgentCode();return false;" />
                            </td>
                            <td>
                                ShipperCode
                            </td>
                            <td>
                                <asp:TextBox ID="txtShipper" runat="server" Width="70px"> </asp:TextBox>
                                <asp:ImageButton ID="IBSC" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle"
                                    OnClientClick="javascript:GetShipperCode();return false;" />
                            </td>
                            <td>
                                IATA Comm.Code
                            </td>
                            <td>
                                <asp:TextBox ID="txtComm" runat="server" Width="70px"> </asp:TextBox>
                                <asp:ImageButton ID="IBCC" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle"
                                    OnClientClick="javascript:GetIATAComCode();return false;" />
                            </td>
                            <td>
                                Product Type
                            </td>
                            <td>
                                <asp:TextBox ID="txtproduct" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="ImgPT" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                    OnClientClick="javascript:GetProductType();return false;" />
                            </td>
                            <td class="style2">
                                ChargedAt
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlChargedAt" runat="server" Enabled="true">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Export" Value="Dep"></asp:ListItem>
                                    <asp:ListItem Text="Import" Value="Del"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                OC&nbsp; ID
                            </td>
                            <td>
                                <asp:TextBox ID="txtOCID" runat="server" Width="70px"></asp:TextBox>
                            </td>
                            <td>
                                Expires From Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtExpfrm" runat="server" Width="70px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" PopupButtonID="btnExpfrmdt"
                                    PopupPosition="BottomLeft" TargetControlID="txtExpfrm">
                                </asp:CalendarExtender>
                                <asp:ImageButton ID="btnExpfrmdt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                            </td>
                            <td>
                                Expires To Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtExpTo" runat="server" Width="70px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" PopupButtonID="btnExptodt"
                                    PopupPosition="BottomLeft" TargetControlID="txtExpTo">
                                </asp:CalendarExtender>
                                <asp:ImageButton ID="btnExptodt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnList" runat="server" CssClass="button" OnClick="btnList_Click"
                                    Text="List" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnClear" runat="server" CssClass="button" OnClick="btnClear_Click"
                                    Text="Clear" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnExport" runat="server" CssClass="button" OnClick="btnExport_Click"
                                    Text="Export" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <h2>
                    Other Charges Details
                </h2>
                <div class="divback" style="width: 1024px; overflow: auto;">
                    <asp:GridView ID="GRDList" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                        Width="100%" OnRowCommand="GRDList_RowCommand" AllowPaging="true" OnPageIndexChanging="GRDList_PageIndexChanging"
                        PageSize="15" Height="82px" HeaderStyle-CssClass="HeaderStyle" CssClass="GridViewStyle"
                        RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle" PagerStyle-CssClass="PagerStyle"
                        SelectedRowStyle-CssClass="SelectedRowStyle">
                        <Columns>
                                                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                            <asp:CheckBox ID="chkUpdate" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hid" runat="server" Value='<%# Eval("SerialNumber") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:ButtonField CommandName="Edit" Text="Edit">
                                <ItemStyle Width="50px" />
                            </asp:ButtonField>
                            <asp:ButtonField CommandName="View" Text="View">
                                <ItemStyle Width="50px" />
                            </asp:ButtonField>
                            <asp:TemplateField Visible="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblSrNo" runat="server" Text='<%#Eval("SerialNumber")%>' CommandName="Edit"
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Origin" HeaderText="Origin">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Destination" HeaderText="Destination" />
                            <asp:BoundField DataField="ChargeHeadCode" HeaderText="Code" />
                            <asp:BoundField DataField="ChargeHeadName" HeaderText="Charge Name">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ChargedAt" HeaderText="Charged At" />
                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" />
                            <asp:BoundField DataField="EndDate" HeaderText="End Date" />
                            <asp:BoundField DataField="ChargeType" HeaderText="Charge Type" />
                            <asp:BoundField DataField="ServiceTax" HeaderText="Service Tax" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:TemplateField HeaderText="Parameters">
                                <ItemTemplate>
                                    <asp:Label ID="lblParams" runat="server" Text='<%#Eval("OtherParams").ToString().Length>50?Eval("OtherParams").ToString().Substring(0,50):Eval("OtherParams") %>'
                                        ToolTip='<%#Eval("OtherParams") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Slabs">
                                <ItemTemplate>
                                    <asp:Label ID="lblSlabs" runat="server" Text='<%#Eval("Slabs").ToString().Length>50?Eval("Slabs").ToString().Substring(0,50):Eval("Slabs") %>'
                                        ToolTip='<%#Eval("Slabs") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="titlecolr" />
                        <RowStyle HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
                <div class="botline">
                    <table style="width: 100%; height: 100%" border="0">
                        <tr>
                            <td colspan="6">
                   
                    Valid From 
                    &nbsp;&nbsp;
                    <asp:TextBox ID="txtUpdtFromDate" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="btnUpFromDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CEUpFrmDate" Format="dd/MM/yyyy" TargetControlID="txtUpdtFromDate"
                                    PopupButtonID="btnUpFromDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                                &nbsp;&nbsp;
                                Valid To
                                 &nbsp;&nbsp;
                    <asp:TextBox ID="txtUpdtToDate" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="btnUpToDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CEUpToDate" Format="dd/MM/yyyy" TargetControlID="txtUpdtToDate"
                                    PopupButtonID="btnUpToDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                                &nbsp;&nbsp;
                                <asp:Button ID="btnUpdate" Text="Update" CssClass="button" runat="server" OnClick="btnUpdate_Click"/>
                               <asp:Label ID="lblUpdateStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Medium" Visible="false"></asp:Label>
                                </td>
                                </tr>
                                </table>
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
