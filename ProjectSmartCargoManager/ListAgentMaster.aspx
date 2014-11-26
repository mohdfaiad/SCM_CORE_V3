<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListAgentMaster.aspx.cs"
    MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.ListAgentMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">

        function setAgentName(DropDown) {
            var AgentName = DropDown.id.replace('ddlagentcode', 'txtagentname');
            var e = document.getElementById(DropDown.id);
            var strUser = e.options[e.selectedIndex].value;
            document.getElementById(AgentName).value = strUser;
            return false;
        }

        function NoData() {
            alert("No Data Found");
            return false;
        }
    </script>

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
            <h1>Agent List</h1>
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                    ForeColor="Red"></asp:Label>
                <div class="botline">
                    <asp:UpdatePanel ID="UPFirst" runat="server">
                        <ContentTemplate>
                            <table border="0">
                                <tr>
                                    <td>
                                        Agent Code
                                    </td>
                                    <td style="width: 150">
                                        <%--<asp:TextBox ID="txtAgentCode" runat="server" Width="110px" ReadOnly="True"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddlagentcode" runat="server" Width="140px" MaxLength="20" onchange="javascript:setAgentName(this);">
                                            <asp:ListItem Selected="True">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                   ControlToValidate="ddlagentcode" ErrorMessage="*" 
                   Font-Bold="True" Font-Italic="False"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td style="width: 90px">
                                        &nbsp; Agent Name
                                    </td>
                                    <td style="width: 209px">
                                        <asp:TextBox ID="txtagentname" runat="server" Enabled="False" Width="95%"></asp:TextBox>
                                    </td>
                                    <td>
                                        Airport Code
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAirportCode" runat="server" Width="170px"> 
                                        <asp:ListItem Selected="True">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        
                                        <%--<asp:TextBox ID="txtcity" runat="server" EnableViewState="true"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="txtcity_AutoCompleteExtender" runat="server" ServiceMethod="GetStation"
                                            MinimumPrefixLength="1" Enabled="True" ServicePath="~/Home.aspx" EnableCaching="true"
                                            TargetControlID="txtcity">
                                        </asp:AutoCompleteExtender>--%>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click1" />
                                        &nbsp;
                                        <asp:Button ID="btntextclear" runat="server" Text="Clear" CssClass="button" OnClick="btntextclear_Click" />
                                     &nbsp;
                                     <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" 
                                            onclick="btnExport_Click"  />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnList" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div style="font-size: larger">
                    <br />
                    Agent Details                    
                    
                </div>
                <div>
                    <asp:UpdatePanel ID="UPSecond" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdCreditdetails" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                      Width="100%" OnRowCommand="grdCreditdetails_RowCommand" AllowPaging="True" AlternatingRowStyle-CssClass="AltRowStyle" 
                    OnPageIndexChanging="grdCreditdetails_PageIndexChanging" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle" 
PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                                <Columns>
                                    <%--<asp:BoundField DataField="SerialNumber" HeaderText="" ItemStyle-ForeColor="White" 
                                        ItemStyle-BorderColor="Black" Visible="true">
                                        <ItemStyle BorderColor="Black" ForeColor="white"></ItemStyle>
                                    </asp:BoundField>--%>
                                    
                                    <asp:TemplateField HeaderText="SrNo" HeaderStyle-Wrap="true" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%# Eval("SerialNumber") %>' Width="80px"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField HeaderText="Agent Code" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>' Width="80px"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Agent Name" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblagentname" runat="server" Text='<%# Eval("AgentName") %>' Width="330px"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                                   
                                    
                                    <asp:TemplateField HeaderText="Customer Code" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerCode" runat="server" ValidationGroup="check" Width="75px"
                                                Text='<%# Eval("CustomerCode") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IATA Agent Code" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIATAAgentCode" runat="server" Width="90px" Text='<%# Eval("IATAAgentCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="City" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCity" runat="server" Width="50px" Text='<%# Eval("City") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Controlling Locator" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblControllingLocator" runat="server" Width="60px" Text='<%# Eval("ControllingLocator") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valid From" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblvalidFrom" runat="server" Width="60px" Text='<%# Eval("ValidFrom") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valid To" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblValidTo" runat="server" Width="65px" Text='<%# Eval("ValidTo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    <%--  <asp:TemplateField HeaderText="Email" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblEmail" runat="server" Width="130px" Text='<%# Eval("Email") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>--%>
                                    <%--  <asp:TemplateField HeaderText="IATA Agent Code" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lbliatacode" runat="server" Width="70px" Text='<%# Eval("IATAAgentCode") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>--%>
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
        <triggers>
          
          <asp:PostBackTrigger ControlID="btnExport"/>
          </triggers>
    </asp:UpdatePanel>
</asp:Content>
