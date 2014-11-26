<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master"
    CodeBehind="HistoryCapacity.aspx.cs" Inherits="ProjectSmartCargoManager.HistoryCapacity" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
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

        function onCommListPopulated() {

            var completionList = $find("ACECommCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function onCategoryListPopulated() {

            var completionList = $find("ACECategoryCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function GetCommodityCode(obj) {
            var destination = obj;
            
            if (destination.value.indexOf("(") > 0) {
                var str = destination.value;
                var start = destination.value.indexOf("(");
            
                obj.value = str.substring(0, start);
            }
            
            return false;
        }

        function GetCategoryCode(obj) {
            var destination = obj;

            if (destination.value.indexOf("(") > 0) {
                var str = destination.value;
                var start = destination.value.indexOf("(");

                obj.value = str.substring(0, start);
            }

            return false;
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
        .styleUpper
        {
            text-transform: uppercase;
        }
    </style>
    <asp:UpdatePanel runat="server" ID="updtPnl">
        <ContentTemplate>
            <div id="contentarea">
                <h1>
                    Historic Capacity
                </h1>
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                    ForeColor="Red"></asp:Label>
                <div class="botline">
                    <asp:Panel ID="pnlNew" runat="server">
                        <table width="100%" border="0" cellpadding="3" cellspacing="3">
                            <tr>
                                <td>
                                    Origin
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOrgin" runat="server" Width="120px"></asp:DropDownList>
                                </td>
                                <td>
                                    Destination
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDestination" runat="server" Width="120px"></asp:DropDownList>
                                </td>
                                <td>
                                    Flight No
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFlightNo" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                    Flight Dt.
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFlightDt" runat="server" Width="120px" AutoPostBack="false"></asp:TextBox>
                                    <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                    <asp:CalendarExtender ID="TextBoxdate_CalendarExtender" runat="server" PopupButtonID="imgDate"
                                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFlightDt">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Commodity
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCommodity" runat="server" Width="120px" CssClass="styleUpper" onchange="return GetCommodityCode(this);"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="ACECommCode" BehaviorID="ACECommCode" runat="server"
                                                        ServiceMethod="GetCommodityCodesWithName" CompletionInterval="0" EnableCaching="false"
                                                        CompletionSetCount="10" TargetControlID="txtCommodity" MinimumPrefixLength="1"
                                                        OnClientPopulated="onCommListPopulated">
                                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    Category
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCategory" runat="server" Width="120px" CssClass="styleUpper" onchange="return GetCategoryCode(this);"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="ACECategoryCode" BehaviorID="ACECategoryCode" runat="server"
                                                        ServiceMethod="GetCommodityCategoryCodes" CompletionInterval="0" EnableCaching="false"
                                                        CompletionSetCount="10" TargetControlID="txtCategory" MinimumPrefixLength="1"
                                                        OnClientPopulated="onCategoryListPopulated">
                                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    Day Of Week
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDayOfWeek" runat="server" Width="120px" AutoPostBack="false"></asp:DropDownList>
                                </td>
                                <td>
                                    Month
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMonth" runat="server" Width="120px" AutoPostBack="false"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Alloc. Capacity
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAvlCapacity" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                    Lower Act. Capacity
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLowerExpCapacity" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                    Act. Capacity
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExpCapacity" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                    Upper Act. Capacity
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUpperExpCapacity" runat="server" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Product Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProductType" runat="server" Width="120px"></asp:DropDownList>
                                </td>
                                <td colspan="6" align="right">
                                    <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnDelete" runat="server" CssClass="button" Text="Delete" 
                                        onclick="btnDelete_Click"/>
                                    <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" OnClick="btnClear_Click" />
                                    <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" OnClick="btnList_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                
                <asp:Panel ID="pnlGrid" runat="server">
                    <div class="ltfloat" style="width: 100%">
                        <asp:GridView ID="grvRegionList" runat="server" ShowFooter="false" Width="99%" AutoGenerateColumns="False"
                            BorderColor="#BFBEBE" BorderStyle="None" CellPadding="2" CellSpacing="3" PageSize="10"
                            AllowPaging="True" OnPageIndexChanging="grvRegionList_PageIndexChanging" OnRowCommand="grvRegionList_RowCommand"
                            onrowediting="grvRegionList_RowEditing" >
                            <Columns>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrigin" runat="server" Text='<%# Eval("Origin") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Destination" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDestination" runat="server" Text='<%# Eval("Destination") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Flight No" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFlightNo" runat="server" Text='<%# Eval("FlightNo") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Flight Dt." HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFlightDate" runat="server" Text='<%# Eval("FlightDt", "{0:dd/MM/yyyy}") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Commodity" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCommodity" runat="server" Text='<%# Eval("Commodity") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Category" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("Category") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Type" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductType" runat="server" Text='<%# Eval("ProductType") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Day Of Week" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDayOfWeek" runat="server" Text='<%# Eval("DayOfWeek") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Month" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("Month") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Alloc. Capacity" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAvailableCapacity" runat="server" Text='<%# Eval("AvailableCapacity") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lower Act. Capacity" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLowerExpCapacity" runat="server" Text='<%# Eval("LowerExpCapacity") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Act. Capacity" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpextedCapacity" runat="server" Text='<%# Eval("ExpextedCapacity") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Upper Act. Capacity" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUpperExpCapacity" runat="server" Text='<%# Eval("UpperExpCapacity") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SerialNo" HeaderStyle-Wrap="true" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrno" runat="server" Text='<%# Eval("Srno") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:ButtonField CommandName="Edit" Text="Edit">
                                    <ItemStyle Width="50px" />
                                </asp:ButtonField>
                            </Columns>
                            <HeaderStyle CssClass="titlecolr" />
                            <RowStyle HorizontalAlign="Center" />
                            <AlternatingRowStyle HorizontalAlign="Center" />
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
            <asp:HiddenField ID="hdnRowId" runat="server" />
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
    </asp:UpdatePanel>
</asp:Content>
