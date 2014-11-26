<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAWBOffload.aspx.cs"
    Inherits="ProjectSmartCargoManager.frmAWBOffload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="style/style.css" rel="stylesheet" type="text/css" />
<link href="style/jetGridView.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .divback
        {
            background: url(images/divback.png) repeat-x scroll left bottom;
            border: 1px solid #d2cfca;
            border-radius: 6px;
            padding: 10px;
            margin: 0px;
            width: 546px;
            height: 76px;
        }
        .divgrd
        {
            overflow: scroll;
        }

        .button
        {
            background: url(images/buton.png) repeat-x;
            border-radius: 6px;
            color: #FFFFFF;
            font-weight: bold;
            font-family: Calibri, Arial, Helvetica, sans-serif;
            font-size: 13px;
        }
        .button:hover
        {
            background: url(images/butin.png) repeat-x;
            border-radius: 6px;
            color: #FFFFFF;
            font-weight: bold;
            font-family: Calibri, Arial, Helvetica, sans-serif;
            font-size: 13px;
        }
        .buttonSearch
        {
            background: none;
        }
        .botline
        {
            border-bottom: 1px solid #a9acb0;
            padding-bottom: 6px;
            float: left;
            width: 718px;
            padding-top: 6px;
        }
    </style>

    <script type="text/javascript">

        function CloseWindow() {
            
            window.close();
            opener.RefreshList();
        }
        
    </script>

</head>
<body style="background: #FFFFFF;">
    <form id="form1" runat="server">
    <div style="width: 750px; margin: 10px auto;">
        <h1 style="width: 90%;">
            AWB Offload
        </h1>
        <asp:Label ID="lblStatus" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
        <table width="95%">
        <tr>
        <td>
        <asp:Label ID="lblFlight" Text="Flight No:" runat="server" />        
        </td>
        <td>
        <asp:Label ID="lblFlightNo" runat="server" />        
        </td>
        
        <td>
        <asp:Label ID="lblFlightDt" Text="Flight Date:" runat="server" />        
        </td>
        <td>
        <asp:Label ID="lblFlightDate" runat="server" />        
        </td>
        
        <td>
        <asp:Label ID="lblStation" Text="Station:" runat="server" />        
        </td>
        <td>
        <asp:Label ID="lblAirport" runat="server" />        
        </td>
        
        </tr>
        </table>
<%--        <asp:Panel ID="pnlGrid" runat="server" BackColor="White" ScrollBars="Auto" Visible="true"
            BorderStyle="Solid" Height="450px" Width="95%">--%>
            <div style="margin: 10px; overflow:auto;height:190px">
                <asp:GridView ID="grdAWBs" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                    Width="95%">
                    <Columns>
                        <asp:TemplateField HeaderText="AWB">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAWBno" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pieces">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPCS" runat="server" MaxLength="4" Width="55px" Enabled="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Weight">
                            <ItemTemplate>
                                <asp:TextBox ID="txtweight" runat="server" MaxLength="4" Enabled="true" Width="55px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Manifested Pcs" HeaderStyle-Wrap="true" HeaderStyle-Width="10px">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAvlPCS" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Manifested Wgt" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAwlWeight" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Origin" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtOrigin" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Destination" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDestination" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ULDNo" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:Label ID="lblULDNO" runat="server" Width="55px" Enabled="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cart No" HeaderStyle-Width="50px" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:Label ID="lblCartNumber" runat="server" Width="80px" Enabled="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
                </asp:GridView>
    </div>
                <div style="float: left" id="Update">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                    </asp:ToolkitScriptManager>
                    <asp:UpdatePanel ID="UpdatePanelRouteDetails" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="LBLRouteStatus" runat="server" ForeColor="Red"></asp:Label>
                            <h2 style="width: 600px">
                                Route Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnAddRouteDetails" runat="server" Text="Add" CssClass="button" OnClick="btnAddRouteDetails_Click" Visible="false" />
                                &nbsp;
                                <asp:Button ID="btnDeleteRouteDetails" runat="server" Text="Delete" CssClass="button"
                                    OnClick="btnDeleteRoute_Click" Visible="false"/>
                                &nbsp;
                            </h2>
                            <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" 
                                Width="93%" ID="grdRouting">
                                <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CHKSelect" runat="server" />
                                            <asp:HiddenField ID="HidScheduleID" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flight Origin *" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFltOrig" runat="server" Width="55px" CssClass="styleUpper" onchange="javascript:getFlightNumbers(this);"
                                                Text='<%# Eval("FltOrigin") %>'> 
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flight Destination*" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFltDest" runat="server" Width="55px" Text='<%# Eval("FltDestination") %>'
                                                CssClass="styleUpper" OnTextChanged="txtFltDest_TextChanged" AutoPostBack="true">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partner Type">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlPartnerType" OnSelectedIndexChanged='ddlPartnerType_SelectionChange'
                                                runat="server" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partner Code">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlPartner" OnSelectedIndexChanged='ddlPartner_SelectionChange'
                                                runat="server" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flight    Date *" HeaderStyle-Width="10px" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFdate" runat="server" Width="80px" Text='<%# Eval("FltDate") %>'
                                                AutoPostBack="True" OnTextChanged="txtFdate_TextChanged" onblur="javascript:txtDatefocus();"></asp:TextBox>
                                            <asp:CalendarExtender ID="TextBox7_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtFdate" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flight #*" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlFltNum" runat="server" Width="90px" OnSelectedIndexChanged="txtFltNumber_TextChanged"
                                                AutoPostBack="false">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtFlightID" runat="server" Visible="false" AutoPostBack="false"
                                                Width="90px"></asp:TextBox>
                                            <asp:TextBox ID="NewFlightID" runat="server" Visible="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdnFltNum" runat="server" Value='<%# Eval("FltNumber") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pcs">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPcs" runat="server" Width="70px" Text='<%# Eval("Pcs") %>' MaxLength="5">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gross Wt">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWt" runat="server" Width="80px" Text='<%# Eval("Wt") %>' MaxLength="9">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRouteLocation" runat="server" Width="80px" MaxLength="50">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                    <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%--</fieldset>--%>
                <table width="100%">
                <tr>
                        <td>
                            <asp:Button ID="btnAddManifest" runat="server" Text="Add To Manifest" CssClass="button"
                                OnClick="btnAddManifest_Click"></asp:Button>
    
                            <asp:Button ID="Button1" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CANCEL %>"
                                CssClass="button" OnClick="btnCancel_Click"></asp:Button>
                        </td>
                        <td>
                         
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNextFlight" Text="Asgn. to Nxt Flt" runat="server" Visible="false">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtNextFlight" runat="server" Visible="false" Width="85px">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblNFltDate" Text="Nxt Flt Dt." runat="server" Visible="false">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtNFltDate" runat="server" Visible="false" Width="100px">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                TargetControlID="txtNFltDate"></asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblReason" Text="Reason" runat="server" Visible="false"></asp:Label>&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlReason" runat="server" onchange="javascript:return select();" Visible="false">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtReason" runat="server" Width="335px" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            
        <%--</asp:Panel>  --%>      
    </div>
    </form>
</body>
</html>
