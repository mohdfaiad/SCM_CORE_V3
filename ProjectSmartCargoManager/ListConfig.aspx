<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListConfig.aspx.cs" Inherits="ProjectSmartCargoManager.ListConfig" MasterPageFile="~/SmartCargoMaster.Master" %>

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
        <div>
        <div class="msg">
        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
        </div>
            <h1>
                List Config
            </h1>
        </div>
        
        <div class="botline">
            <table style="width: 90%; height: 100%" border="0">
                <tr>
                    <td>
                        Origin Level</td>
                    <td><asp:DropDownList ID="DDLOriginLevel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLOriginLevel_SelectedIndexChanged">
                                    <asp:ListItem Text="Airport" Selected="True" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="City" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                    
                    </td>
                    <td>
                        &nbsp; Origin</td>
                    <td>
                        <asp:DropDownList ID="ddlOrigin" runat="server" Width="80px">
                        </asp:DropDownList>
                    </td>
                    <td>Destination Level
                        
                        
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
                    <asp:DropDownList ID="ddlDestination" runat="server" Width="80px">
                    </asp:DropDownList>
                    </td>
                    <%--<td>
                        Status
                    </td>--%>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Visible="false">
                            <asp:ListItem Value="All" Selected="True">All</asp:ListItem>
                            <asp:ListItem Value="ACT">Active</asp:ListItem>
                            <asp:ListItem Value="INA">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        Config Name</td>
                    <td>
                        <asp:TextBox ID="txtChargeName" runat="server" Width="70px"></asp:TextBox>
                    </td>
                    <td>
                        Parameter</td>
                    <td>
                        <asp:TextBox ID="txtParam" runat="server" Width ="90px"></asp:TextBox>
                    </td>
                    <td>
                        From Date</td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="70px"></asp:TextBox>
                        <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" 
                            Format="yyyy-MM-dd" PopupButtonID="btnFromDate" PopupPosition="BottomLeft" 
                            TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:ImageButton ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" 
                            ImageUrl="~/Images/calendar_2.png" />
                    </td>
                    <td>
                        To Date</td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Width="70px"></asp:TextBox>
                        <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" 
                            Format="yyyy-MM-dd" PopupButtonID="btnToDate" PopupPosition="BottomLeft" 
                            TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:ImageButton ID="btnToDate" runat="server" ImageAlign="AbsMiddle" 
                            ImageUrl="~/Images/calendar_2.png" />
                    </td>
                    <%--<td>Charge Type</td>--%>
                    <td>
                        <asp:DropDownList ID="DDLChargeType" runat="server" Visible="false">
                            <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                            <asp:ListItem Text="Due Agent" Value="DA"></asp:ListItem>
                            <asp:ListItem Text="Due Carrier" Value="DC"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="btnList" runat="server" CssClass="button" 
                            OnClick="btnList_Click" Text="List" />
                    </td>
                    <td>
                        <asp:Button ID="btnExport" runat="server" CssClass="button" 
                            OnClick="btnExport_Click" Text="Export" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </div>
        <h2>
            Config Details
        </h2>
        <div class="divback">
            <asp:GridView ID="GRDList" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                Width="100%" onrowcommand="GRDList_RowCommand"
                AllowPaging="true" OnPageIndexChanging="GRDList_PageIndexChanging"
                        PageSize="30" Height="82px"
                         
                             HeaderStyle-CssClass="HeaderStyle"  CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle" PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                <Columns>
                    
                    <asp:TemplateField HeaderText="Origin Level">
                        <ItemTemplate>
                            <asp:Label ID="LBLOriginLevel" runat="server" Text='<%# Eval("OriginLevel") %>'></asp:Label>
                            <asp:HiddenField ID="Hid" runat="server" Value='<%# Eval("SerialNumber") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Origin" HeaderText="Origin" />
                    <asp:BoundField DataField="DestinationLevel" HeaderText="Destination Level" />
                    <asp:BoundField DataField="Destination" HeaderText="Destination" />
                    <asp:BoundField DataField="ConfigCode" HeaderText="Config Code" />
                    <asp:BoundField DataField="ConfigDesc" HeaderText="Config Name" />                   
                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" />
                    <asp:BoundField DataField="Parameter" HeaderText="Parameter" />
                    <asp:BoundField DataField="Format" HeaderText="Format" />
                    <asp:BoundField DataField="DateFormat" HeaderText="Date Format" />
                    <asp:BoundField DataField="NextRoundOff" HeaderText="Next Round Off" />
                    <asp:BoundField DataField="DecimalAllow" HeaderText="Decimal Allow" />
                   
                    <asp:ButtonField CommandName="Edit" Text="Edit">
                        <ItemStyle Width="50px" />
                    </asp:ButtonField>
                    <asp:ButtonField CommandName="View" Text="View">
                        <ItemStyle Width="50px" />
                    </asp:ButtonField>
                </Columns>
                <HeaderStyle CssClass="titlecolr" />
                <RowStyle HorizontalAlign="Center" />
                <AlternatingRowStyle HorizontalAlign="Center" />
            </asp:GridView>
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