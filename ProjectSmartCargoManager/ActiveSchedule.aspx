<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActiveSchedule.aspx.cs"
    MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.ActiveSchedule" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UP1" runat="server">
        <ContentTemplate>

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
            <div id="contentarea">
                <div class="msg">
                    <asp:Label ID="lblResult" runat="server" BackColor="White" Font-Bold="True" ForeColor="Red"></asp:Label>
                </div>
                <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                <h1>
                    Active Flights</h1>
                <div class="botline">
                    <asp:Panel ID="pnlSearch" runat="server">
                        <table width="100%">
                            <tr>
                                <td>
                                    Origin
                                </td>
                                <td>
                                    <%--  <asp:TextBox ID="txtAutoSource" runat="server" Width="100px"></asp:TextBox>
                         <asp:AutoCompleteExtender ID="txtAutoSource_AutoCompleteExtender" runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
               EnableCaching="true"  TargetControlID="txtAutoSource">
               </asp:AutoCompleteExtender>--%>
                                    <asp:DropDownList ID="ddlAutoSource" runat="server" Width="100px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Destination
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAutoDest" runat="server" Width="100px">
                                    </asp:DropDownList>
                                    <%-- <asp:TextBox ID="txtAutoDest" runat="server" Width="100px"></asp:TextBox>
                         <asp:AutoCompleteExtender ID="txtAutoDest_AutoCompleteExtender" 
                            runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  
                            Enabled="True" ServicePath="~/Home.aspx"  
               EnableCaching="true"  TargetControlID="txtAutoDest">
               </asp:AutoCompleteExtender>--%>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Flight #&nbsp;
                                </td>
                                <td>
                                    <%--<asp:DropDownList ID="ddlFlight" runat="server" 
                            onselectedindexchanged="ddlFlight_SelectedIndexChanged">
              </asp:DropDownList>--%>
                                    <asp:TextBox ID="txtFlightNo" runat="server" Width="100px"></asp:TextBox>
                                </td>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtendertxtFlightNo1" runat="server" ServiceMethod="GetFlightId"
                                    MinimumPrefixLength="2" Enabled="True" ServicePath="~/ListAirlineSchedule.aspx"
                                    EnableCaching="true" TargetControlID="txtFlightNo">
                                </asp:AutoCompleteExtender>
                                <td>
                                    AirCraftType
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAirCraftType" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td class="style1">
                                    Status
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server">
                                        <asp:ListItem Value="All"></asp:ListItem>
                                        <asp:ListItem Value="ACTIVE"></asp:ListItem>
                                        <asp:ListItem Value="CANCELLED"></asp:ListItem>
                                        <asp:ListItem Value="DRAFT"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOrigin" runat="server" AutoPostBack="True" Visible="False">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;From Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFlightFromdate" runat="server" Width="100px" ToolTip="Please enter date format: dd/MM/yyyy"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtFlightFromdate" PopupButtonID="imgFromDate">
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1"  controltovalidate="txtFlightFromdate"
                            runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                    </asp:CalendarExtender>
                                    <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                </td>
                                <td>
                                    &nbsp;To Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFlightToDate" runat="server" Width="100px" ToolTip="Please enter valid date format: dd/MM/yyyy"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2"  controltovalidate="txtFlightToDate"
                            runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                    <asp:CalendarExtender ID="txtFlightToDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                        Enabled="True" TargetControlID="txtFlightToDate" PopupButtonID="imgDate">
                                    </asp:CalendarExtender>
                                    <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDomestic" runat="server" AutoPostBack="True" Checked="True"
                                        Text="Domestic" ValidationGroup="A" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkInternational" runat="server" Checked="True" Text="International"
                                        ValidationGroup="A" />
                                </td>
                                <td colspan="2">
                                    <%--<asp:DropDownList ID="ddlOperationStatus" runat="server">
                         <asp:ListItem Value ="All"></asp:ListItem>
                         <asp:ListItem Value ="New"></asp:ListItem>
                         <asp:ListItem Value ="Departed"></asp:ListItem>
                         <asp:ListItem Value="Manifested"></asp:ListItem>
                         <asp:ListItem Value="Reopened"></asp:ListItem>
                        </asp:DropDownList>--%>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDestination" runat="server" AutoPostBack="True" Visible="False">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Operation&nbsp; Status
                                </td>
                                <td colspan="5">
                                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal">
                                        <%--<asp:ListItem Value ="All"></asp:ListItem>--%>
                                        <asp:ListItem Selected="True" Value="All"></asp:ListItem>
                                        <asp:ListItem Selected="True" Value="New"></asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Manifested"></asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Departed"></asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Reopened"></asp:ListItem>
                                    </asp:CheckBoxList>
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
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="Button1" runat="server" CssClass="button" OnClick="Button1_Click"
                                        Text="Datewise List" />
                                    <asp:Button ID="btnclear" runat="server" CssClass="button" Text="Clear" OnClick="btnclear_Click" />
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </asp:Panel>
                </div>
                <div style="float: left; width: 100%;">
                    <asp:Panel ID="Showlist" runat="server">
                        <%--<asp:UpdatePanel ID="updPandataGridList" runat="server">
      <ContentTemplate>--%>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                            AllowPaging="true" Width="100%" OnPageIndexChanging="GridView1_PageIndexChanging"
                            OnSelectedIndexChanging="GridView1_SelectedIndexChanging" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                            CellPadding="2" PageSize="10" OnRowCommand="GridView1_RowCommand" AlternatingRowStyle-CssClass="AltRowStyle"
                            CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle" PagerStyle-CssClass="PagerStyle"
                            RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                            <Columns>
                                <asp:TemplateField HeaderText="SrNo" Visible="false" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblScheduleID" runat="server" Width="50px" Text='<%# Eval("ScheduleID") %>'></asp:Label><%--Text='<%# Eval("ScheduleID") %>'--%>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Flight#" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFlight" runat="server" Text='<%# Eval("FlightID") %>'>
                                        </asp:Label>
                                        <%--<asp:TextBox  ID="txtSource" runat="server" Width="70px" ></asp:TextBox>--%>
                                        <%--<asp:TextBox ID="txtbankname" runat="server"></asp:TextBox>--%>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="From" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFromOrigin" runat="server" Text='<%# Eval("Source") %>'>
                                        </asp:Label>
                                        <%--<asp:TextBox  ID="txtSource" runat="server" Width="70px" ></asp:TextBox>--%>
                                        <%--<asp:TextBox ID="txtbankname" runat="server"></asp:TextBox>--%>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                                        <asp:Label ID="lblToDest" runat="server" Text='<%# Eval("Dest") %>'>>
                                        </asp:Label>
                                        <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dept Time" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFromDt" runat="server" Text='<%# Eval("SchDeptDay") %>'>>
                                        </asp:Label>
                                        <asp:Label ID="lblDepttime" runat="server" Text='<%# Eval("SchDeptTime") %>'>>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Arrival Time " HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblToDt" runat="server" Text='<%# Eval("SchArrDay") %>'>>
                                        </asp:Label>
                                        <asp:Label ID="lblArrtime" runat="server" Text='<%# Eval("SchArrTime") %>'>>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AirCraft Type" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAirCraft" runat="server" Text='<%# Eval("AircraftType") %>'>>
                                        </asp:Label>
                                        <%--<asp:DropDownList ID="ddlAirCraft" runat="server" 
                     AutoPostBack="True" OnSelectedIndexChanged="showCapacityInGrid"  >
              </asp:DropDownList>--%>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tail No" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTailNo" runat="server" Text='<%# Eval("TailNo") %>'>>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Capacity(Kg)" HeaderStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%--<asp:TextBox ID="txtCapacity" Width="50px" Text="" MaxLength="4" 
                            runat="server" ></asp:TextBox>--%>
                                        <asp:Label ID="lblCapacity" Width="50px" Text='<%# Eval("CargoCapacity1") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status " HeaderStyle-Wrap="false">
                                    <FooterTemplate>
                                        <%--                         <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="button"  OnClick ="Addrow" />
--%>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" Width="80px" MaxLength="4" Text='<%# Eval("Status") %>'
                                            runat="server"></asp:Label>
                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                                        <%-- <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="58px">
                         <asp:ListItem Value ="ACTIVE"></asp:ListItem>
                         <asp:ListItem Value="CANCELLED"></asp:ListItem>
                         <asp:ListItem Value="DRAFT"></asp:ListItem>
                        </asp:DropDownList>--%>
                                        <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Operation Status" HeaderStyle-Wrap="false">
                                    <FooterTemplate>
                                        <%--                         <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="button"  OnClick ="Addrow" />
--%>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOperationStatus" Width="50px" Text='<%# Eval("OperationStatus") %>'
                                            runat="server"></asp:Label>
                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                                        <%-- <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="58px">
                         <asp:ListItem Value ="ACTIVE"></asp:ListItem>
                         <asp:ListItem Value="CANCELLED"></asp:ListItem>
                         <asp:ListItem Value="DRAFT"></asp:ListItem>
                        </asp:DropDownList>--%>
                                        <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" HeaderStyle-Wrap="false" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" Text="Edit" runat="server"></asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle Wrap="True"></HeaderStyle>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="titlecolr" />
                            <RowStyle HorizontalAlign="Center" />
                            <AlternatingRowStyle HorizontalAlign="Center" />
                        </asp:GridView>
                        <%--    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="click" />
    </Triggers>
       </asp:UpdatePanel>--%></asp:Panel>
                    <%--<asp:TextBox ID="txtCapacity" Width="50px" Text="" MaxLength="4" 
                            runat="server" ></asp:TextBox>--%>
                    <%--       <asp:UpdatePanel ID="updPanUpdateDetails" runat="server">
      <ContentTemplate>--%>
                    <asp:Panel ID="grid" runat="server" ScrollBars="Horizontal">
                        <asp:Panel ID="pnlDestDetails" runat="server">
                            <div class="botline">
                                <table>
                                    <tr>
                                        <td>
                                            Origin
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlOrigin1" runat="server" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox>
                                        </td>
                                        <td>
                                            Destination
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDestination0" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            AirCraftType *
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlLoadAirCraftType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLoadAirCraftType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Free Sale (kg)&nbsp; *
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCargoCapacity" runat="server" Height="19px" MaxLength="6" Width="114px"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Flight Date
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFromdate" runat="server" Height="19px" Width="114px" ToolTip="Please enter date format: dd/MM/yyyy"
                                                Enabled="False"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtFromdate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" runat="server" Width="114px" AutoPostBack="True" ToolTip="Please enter date format: dd/MM/yyyy"
                                                Visible="False"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtToDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="chkDomestic0" runat="server" AutoPostBack="True" Checked="True"
                                                GroupName="B" Text="Domestic" ValidationGroup="B" Visible="False" />
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButton ID="chkInternational0" runat="server" AutoPostBack="True" GroupName="B"
                                                Text="International" ValidationGroup="B" Visible="False" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSource1" runat="server" Enabled="False" Height="19px" Width="114px"
                                                Visible="False"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDestination" runat="server" Enabled="False" Height="19px" Width="114px"
                                                Visible="False"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlSchedule" runat="server">
                            <h2>
                                Route Details
                            </h2>
                            <div>
                                <table width="100%">
                                    <tr>
                                        <td width="40%">
                                        </td>
                                        <td width="40%">
                                        </td>
                                        <td width="10%">
                                            <asp:Button ID="btnAddNewRow" runat="server" Text="Add New" CssClass="button" Visible="false"
                                                OnClick="btnAddNewRow_Click" />
                                        </td>
                                        <td width="10%">
                                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button" OnClick="btnDelete_Click"
                                                Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal">
                                    <asp:GridView ID="grdScheduleinfo" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                        PageSize="40" AllowPaging="True">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CHK" runat="server" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Flight# *" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlight" runat="server">
                                                    </asp:Label>
                                                    <%--<asp:TextBox  ID="txtSource" runat="server" Width="70px" ></asp:TextBox>--%>
                                                    <%--<asp:TextBox ID="txtbankname" runat="server"></asp:TextBox>--%>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="From *" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlFromOrigin" runat="server" Width="45px">
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox  ID="txtSource" runat="server" Width="70px" ></asp:TextBox>--%>
                                                    <%--<asp:TextBox ID="txtbankname" runat="server"></asp:TextBox>--%>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="To *" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                                                    <asp:DropDownList ID="ddlToDest" runat="server" Width="45px">
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dept Time  *" HeaderStyle-Wrap="false" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    Day
                                                    <asp:TextBox ID="txtDeptDay" runat="server" Width="8px" DataTextField=""></asp:TextBox>
                                                    <asp:NumericUpDownExtender ID="NumericUpDownExtender_DeptDay" runat="server" TargetControlID="txtDeptDay"
                                                        Width="40" RefValues="" ServiceDownMethod="" ServiceUpMethod="" TargetButtonDownID=""
                                                        TargetButtonUpID="" Minimum="1" Maximum="2" />
                                                    Hr
                                                    <asp:TextBox ID="txtDeptTimeHr" runat="server" Width="8px" DataTextField=""></asp:TextBox>
                                                    <asp:NumericUpDownExtender ID="txtDeptTimeHr_NumericUpDownExtender" runat="server"
                                                        TargetControlID="txtDeptTimeHr" Width="40" RefValues="" ServiceDownMethod=""
                                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" Minimum="0" Maximum="23" />
                                                    : Min
                                                    <asp:TextBox ID="txtDeptTimeMin" runat="server" Width="8px" DataTextField=""></asp:TextBox>
                                                    <asp:NumericUpDownExtender ID="txtDeptTimeMin_NumericUpDownExtender1" runat="server"
                                                        TargetControlID="txtDeptTimeMin" Width="40" RefValues="" ServiceDownMethod=""
                                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" Minimum="1" Maximum="60" />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Arrival Time  *" HeaderStyle-Wrap="false" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    Day
                                                    <asp:TextBox ID="txtArrivalDay" runat="server" Width="8px" DataTextField=""></asp:TextBox>
                                                    <asp:NumericUpDownExtender ID="NumericUpDownExtender_ArrivalDay" runat="server" TargetControlID="txtArrivalDay"
                                                        Width="40" RefValues="" ServiceDownMethod="" ServiceUpMethod="" TargetButtonDownID=""
                                                        TargetButtonUpID="" Minimum="1" Maximum="2" />
                                                    Hr<asp:TextBox ID="txtArrivaltimeHr" runat="server" Width="8px"></asp:TextBox>
                                                    <asp:NumericUpDownExtender ID="txtArrivaltimeHr_NumericUpDownExtender" runat="server"
                                                        TargetControlID="txtArrivaltimeHr" Width="40" RefValues="" ServiceDownMethod=""
                                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" Minimum="0" Maximum="23" />
                                                    : Min<asp:TextBox ID="txtArrivalTimeMin" runat="server" Width="8px"></asp:TextBox>
                                                    <asp:NumericUpDownExtender ID="txtArrivalTimeMin_NumericUpDownExtender1" runat="server"
                                                        TargetControlID="txtArrivalTimeMin" Width="40" RefValues="" ServiceDownMethod=""
                                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" Minimum="1" Maximum="60" />
                                                    <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbankguranteeamt"  ErrorMessage="Please Enter Numeric Value"  ValidationExpression="^\d+$" ValidationGroup="check">
                        </asp:RegularExpressionValidator>--%>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <%--             <asp:TemplateField HeaderText="Frequency  *" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=40%>
                   
                    <ItemTemplate>
                        <asp:CheckBox ID="chkMon" runat="server" Checked="True" Text="Mo" />
                        <asp:CheckBox ID="chkTues" runat="server" Checked="True" Text="Tu" />
                        <asp:CheckBox ID="chkwed" runat="server" Checked="True" Text="We" />
                        <asp:CheckBox ID="chkThur" runat="server" Checked="True" 
                            Text="Th" />
                        <asp:CheckBox ID="chkFri" runat="server" Checked="True" Text="Fr" />
                        <asp:CheckBox ID="chkSat" runat="server" Checked="True" 
                            Text="Sa" />
                        <asp:CheckBox ID="chkSun" runat="server" Checked="True" Text="Su" />
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="AirCraft Type*" HeaderStyle-Wrap="false" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlAirCraft" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDropDownList_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tail No" HeaderStyle-Wrap="false" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlTailNo" runat="server" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Capacity(Kg)*" HeaderStyle-Wrap="false" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCapacity" Width="50px" Text="" MaxLength="6" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status *" HeaderStyle-Wrap="false">
                                                <FooterTemplate>
                                                    <%--                         <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="button"  OnClick ="Addrow" />
--%>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                                                    <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="58px">
                                                        <asp:ListItem Value="ACTIVE"></asp:ListItem>
                                                        <asp:ListItem Value="CANCELLED"></asp:ListItem>
                                                        <asp:ListItem Value="DRAFT"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="titlecolr" />
                                        <RowStyle HorizontalAlign="Center" />
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                            <div>
                            </div>
                        </asp:Panel>
                        <div id="fotbut">
                            <%--<a sp:Panel ID="pnlUpdate" runat="server" >--%>
                            <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="button" OnClick="btnSave_Click" />
                            <asp:Button ID="BtnCLose" runat="server" CssClass="button" Text="Close" OnClick="BtnCLose_Click1" />
                            <%--</asp:Panel>--%>
                        </div>
                    </asp:Panel>
                    <%--     </ContentTemplate>
     
     </asp:UpdatePanel>--%>
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
    </asp:UpdatePanel>
</asp:Content>
