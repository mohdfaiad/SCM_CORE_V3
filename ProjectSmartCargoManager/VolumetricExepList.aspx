<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VolumetricExepList.aspx.cs" Inherits="ProjectSmartCargoManager.VolumetricExepList" MasterPageFile="~/SmartCargoMaster.Master" %>

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
                List Volumetric Exemption
            </h1>
        </div>
        
        <div class="botline">
            <table style="width: 100%; height: 100%" border="0">
                
                <tr>
                    <td>
                        From Date</td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="90px"></asp:TextBox>
                        <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" 
                            Format="dd/MM/yyyy" PopupButtonID="btnFromDate" PopupPosition="BottomLeft" 
                            TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:ImageButton ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" 
                            ImageUrl="~/Images/calendar_2.png" />
                    </td>
                    <td>
                        To Date</td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" Width="90px"></asp:TextBox>
                        <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" 
                            Format="dd/MM/yyyy" PopupButtonID="btnToDate" PopupPosition="BottomLeft" 
                            TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:ImageButton ID="btnToDate" runat="server" ImageAlign="AbsMiddle" 
                            ImageUrl="~/Images/calendar_2.png" />
                    </td>
                    <td>
                        Parameter</td>
                    <td>
                        <asp:TextBox ID="txtParam" runat="server" Width ="110px"></asp:TextBox>
                    </td>
                      <td>
                        Status
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server">
                            <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                            <asp:ListItem Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                     <td colspan="3">
                        <asp:Button ID="btnList" runat="server" CssClass="button" 
                            OnClick="btnList_Click" Text="List" />&nbsp;
                     <asp:Button ID="btnClear" runat="server" CssClass="button" 
                            OnClick="btnClear_Click" Text="Clear" />&nbsp;
                        <asp:Button ID="btnExport" runat="server" CssClass="button" 
                            OnClick="btnExport_Click" Text="Export" />&nbsp;
                    </td>
                </tr>
                
            </table>
        </div>
        <h2>
            Volumetric Exemption Details
        </h2>
        <div class="divback">
            <asp:GridView ID="GRDList" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                Width="100%" onrowcommand="GRDList_RowCommand"
                AllowPaging="true" OnPageIndexChanging="GRDList_PageIndexChanging"
                        PageSize="30" Height="82px"
                         
                             HeaderStyle-CssClass="HeaderStyle"  CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle" PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                <Columns>
                    
                    <asp:TemplateField HeaderText="Start Date">
                        <ItemTemplate>
                            <asp:Label ID="LBLStartDate" runat="server" Text='<%# Eval("StartDate") %>'></asp:Label>
                            <asp:HiddenField ID="Hid" runat="server" Value='<%# Eval("SerialNumber") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" />
                    <asp:BoundField DataField="Params" HeaderText="Parameters" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                   
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
