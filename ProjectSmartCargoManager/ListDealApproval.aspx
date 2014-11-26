<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListDealApproval.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.ListDealApproval" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    
    <script language="javascript" type="text/javascript">

        function SelectAllgrdAddRate(CheckBoxControl) {
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if (document.forms[0].elements[i].name.indexOf('check') > -1) {
                    document.forms[0].elements[i].checked = CheckBoxControl.checked;
                }
            }
        }
    </script>
    
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
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
    
    <div id="contentarea">
        <h1>
            Deal Approval
        </h1>
        <p>
            &nbsp;<asp:UpdatePanel ID="UPFourth" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            <p>
            </p>
            <div class="botline">
                <asp:UpdatePanel ID="UPFirst" runat="server">
                    <ContentTemplate>
                        <table border="0" style="width: 1000px; height: 100%">
                            <tr>
                                <td>
                                    <fieldset>
                                        <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Deal 
                                            Search</legend>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    Origin Type
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlorigintype" runat="server" AutoPostBack="true" 
                                                        onselectedindexchanged="ddlorigintype_SelectedIndexChanged" Width="80px">
                                                        <asp:ListItem Text="Select"></asp:ListItem>
                                                        <asp:ListItem Text="Airport" Value="A"></asp:ListItem>
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
                                                    <asp:DropDownList ID="ddlOrigin" runat="server" Width="80px">
                                                        <asp:ListItem Selected="True" Text="Select"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Destination Type
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddldestinationType" runat="server" AutoPostBack="true" 
                                                        onselectedindexchanged="ddldestinationType_SelectedIndexChanged" Width="80px">
                                                        <asp:ListItem Selected="True" Text="Select"></asp:ListItem>
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
                                                    <asp:DropDownList ID="ddlDestination" runat="server" Width="80px">
                                                        <asp:ListItem Selected="True" Text="Select"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Flight Number
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlFlightNumber" runat="server" Width="80px">
                                                        <asp:ListItem Selected="True" Text="Select"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Agent Code
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlAgentCode" runat="server" Width="80px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Applicable From
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtApplicableFrom" runat="server" Width="80px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="txtApplicableFrom_CalendarExtender" runat="server" 
                                                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtApplicableFrom">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    Applicable To
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtApplicableTo" runat="server" Width="80px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="txtApplicableTo_CalendarExtender" runat="server" 
                                                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtApplicableTo">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    Deal Id
                                                </td>
                                                <td>
                                                    &nbsp;&nbsp;
                                                    <asp:TextBox ID="txtdealid" runat="server" Width="80px"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnList" runat="server" CssClass="button" 
                                                        onclick="btnList_Click" Text="List" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnList" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <br />
            <h2>
                Deal Details
            </h2>
            <asp:UpdatePanel ID="UPThird" runat="server">
                <ContentTemplate>
                    <asp:Label ID="LBLNoOfRecords" runat="server"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="divback">
                <asp:UpdatePanel ID="UPSecond" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdDealApproval" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" Height="160px" 
                            onpageindexchanging="grdDealApproval_PageIndexChanging" PageSize="8" 
                            Width="100%">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk" runat="server" 
                                            onclick="javascript:SelectAllgrdAddRate(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="check" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" BackColor="White" BorderColor="White" 
                                            BorderWidth="0px" ForeColor="White" ItemStyle-BorderColor="Black" 
                                            ItemStyle-ForeColor="White" Text='<%# Eval("SerialNumber") %>' Width="0px"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Agent Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>' 
                                            Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="OriginType">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOriginType" runat="server" Text='<%# Eval("OriginType") %>' 
                                            ValidationGroup="check" Width="80px">
                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Origin">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrigin" runat="server" Text='<%# Eval("Origin") %>' 
                                            Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="DestinationType">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDestinationType" runat="server" 
                                            Text='<%# Eval("DestinationType") %>' Width="80px">
                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Destinatin">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDest" runat="server" Text='<%# Eval("Destination") %>' 
                                            Width="80px">
                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="FlightNo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFlightNo" runat="server" Text='<%# Eval("FlightNo") %>' 
                                            Width="50px">
                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Commodity">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCommodity" runat="server" Text='<%# Eval("Commodity") %>' 
                                            Width="60px">
                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Rate">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRate" runat="server" Text='<%# Eval("Rate") %>' 
                                            Width="50px"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Threshold">
                                    <ItemTemplate>
                                        <asp:Label ID="lblThreshold" runat="server" Text='<%# Eval("Threshold") %>' 
                                            Width="60px"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Approval">
                                    <ItemTemplate>
                                        <asp:Label ID="lblaprval" runat="server" Text='<%# Eval("Approval") %>' 
                                            Width="60px"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="titlecolr" />
                            <RowStyle HorizontalAlign="Center" />
                            <AlternatingRowStyle HorizontalAlign="Center" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div align="right">
                <%-- <asp:Button ID="btnNewBooking" runat="server" CssClass="button" Text="New Booking"
                PostBackUrl="~/ConBooking.aspx" />--%> &nbsp;<asp:Button ID="btnApprove" 
                    runat="server" CssClass="button" onclick="btnApprove_Click" Text="Approve" />
                &nbsp;<asp:Button ID="btnClose" runat="server" CssClass="button" 
                    PostBackUrl="~/Home.aspx" Text="Close" />
            </div>
        </p>
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
    
    <div id="msgfade" class="black_overlaymsg"></div>
    
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
