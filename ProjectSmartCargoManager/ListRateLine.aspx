<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListRateLine.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master"
    Inherits="ProjectSmartCargoManager.ListRateLine" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
    function GetAgentCode() {
        var level = 'AgentCode';
        var TxtOriginClientObject = '<%=txtParam.ClientID %>';
        
        window.open('ListDataView.aspx?Parent=AgentCode&level=' + level + '&TargetTXT=' + TxtOriginClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        return false;
    }
</script>

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

        function GetIATAComCode() {

            var TxtClientObject = '<%= TXTIATAComCode.ClientID %>';
            var value = document.getElementById('<%= TXTIATAComCode.ClientID %>').value;
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

        function GetProductType() {

            var TxtClientObject = '<%= TXTProductType.ClientID %>';
            var TxtClientValue = document.getElementById('ctl00_ContentPlaceHolder1_TXTProductType').value;

            window.open('ListMultipleSelectProductType.aspx?TargetTXT=' + TxtClientObject + '&txtValue=' + TxtClientValue, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=350,height=300,toolbar=0,resizable=0');
        }
        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=grdListStock.ClientID %>");
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
        <h1>
            <img alt="" src="images/listrateline.gif" />
        </h1>
        <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
        </p>
        <div class="botline">
            <asp:UpdatePanel ID="UPFirst" runat="server">
                <ContentTemplate>
                    <table style="width: 100%; height: 100%" border="0">
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Origin Level"></asp:Label>
                            </td>
                            <td class="style11">
                                <asp:DropDownList ID="DDLOriginLevel" runat="server" AutoPostBack="true" 
                                    OnSelectedIndexChanged="DDLOriginLevel_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="Airport" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="City" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Origin
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOrigin" runat="server" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Destination Level"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLDestinationLevel" runat="server" AutoPostBack="true" 
                                    OnSelectedIndexChanged="DDLDestinationLevel_SelectedIndexChanged" 
                                    Width="100px">
                                    <asp:ListItem Selected="True" Text="Airport" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="City" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Destination"></asp:Label>
                                &nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDestination" runat="server" Width="100px">
                                </asp:DropDownList>
                                &nbsp;
                            </td>
                           
                            
                        </tr>
                        <tr>
                            <td>
                                From Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="btnFromDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CEFromDate" Format="dd/MM/yyyy" TargetControlID="txtFromDate"
                                    PopupButtonID="btnFromDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                To Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="btnToDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CEToDate" Format="dd/MM/yyyy" TargetControlID="txtToDate"
                                    PopupButtonID="btnToDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                Status
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server">
                                    <asp:ListItem Value="All">All</asp:ListItem>
                                    <asp:ListItem Value="ACT">Active</asp:ListItem>
                                    <asp:ListItem Value="INA">Inactive</asp:ListItem>
                                    <asp:ListItem Value="DRF">Draft</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                
                                Rate Card Name</td>
                            <td>
                            
                                <asp:DropDownList ID="DdlRateCardName" runat="server">
                                </asp:DropDownList>
                            
                            </td>
                           
                        </tr>
                        <tr>
                        <td>Parameter
                                </td>
                                 <td>
                                <asp:TextBox ID="txtParam" runat="server" Width="90px"></asp:TextBox>
                                <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
                                </td>
                            <td>
                                TACT Rate</td>
                            <td>
                                <asp:CheckBox ID="ckhTACTRate" runat="server" />
                            </td>
                            
                            
                                 <td>
                                     ULD Rate</td>
                            <td>
                                <asp:CheckBox ID="ckhULDRate" runat="server" />
                            </td>
                            
                            
                            <td>
                                Heavy Applicable</td>
                                <td>
                                    <asp:CheckBox ID="ckhHeavyRate" runat="server" />
                            </td>
                                 <td>
                                All-In Rate<asp:CheckBox ID="ckhAllIn" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Agent Code</td>
                            <td class="style13">
                                <asp:TextBox ID="TXTAgentCode" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton7" runat="server" ImageAlign="AbsMiddle" 
                                    ImageUrl="~/Images/list_bullets.png" 
                                    OnClientClick="javascript:GetAgentCode();return false;" />
                            </td>
                            <td>
                                Shipper Code</td>
                            <td>
                                <asp:TextBox ID="TXTShipperCode" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton5" runat="server" ImageAlign="AbsMiddle" 
                                    ImageUrl="~/Images/list_bullets.png" 
                                    OnClientClick="javascript:GetShipperCode();return false;" />
                            </td>
                            <td>
                                IATA Comm. Code</td>
                            <td>
                                <asp:TextBox ID="TXTIATAComCode" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton6" runat="server" ImageAlign="AbsMiddle" 
                                    ImageUrl="~/Images/list_bullets.png" 
                                    OnClientClick="javascript:GetIATAComCode();return false;" />
                            </td>
                            <td>
                                Product Type</td>
                            <td>
                                <asp:TextBox ID="TXTProductType" runat="server" Width="100px"></asp:TextBox>
                                <asp:ImageButton ID="IProd" runat="server" ImageAlign="AbsMiddle" 
                                    ImageUrl="~/Images/list_bullets.png" 
                                    OnClientClick="javascript:GetProductType();return false;" />
                            </td>
                           
                        </tr>
                        <tr>
                        <td>
                        Rate ID
                        </td>
                        <td>
                        <asp:TextBox ID="txtRateID" runat="server" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                                Expires From Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtExpfrm" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="btnExpfrmdt" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CalendarExtender1" Format="dd/MM/yyyy" TargetControlID="txtExpfrm"
                                    PopupButtonID="btnExpfrmdt" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                Expires To Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtExpTo" runat="server" Width="70px"></asp:TextBox>
                                <asp:ImageButton ID="btnExptodt" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CalendarExtender2" Format="dd/MM/yyyy" TargetControlID="txtExpTo"
                                    PopupButtonID="btnExptodt" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
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
                        <tr>
                            
                            <td colspan="2">
                                &nbsp;&nbsp;<asp:Button ID="btnList" runat="server" CssClass="button" 
                                    OnClick="btnList_Click" Text="List" />
                                <asp:Button ID="btnClear" runat="server" CssClass="button" 
                                    OnClick="btnClear_Click" Text="Clear" />
                                <asp:Button ID="btnExport" runat="server" CssClass="button" 
                                    OnClick="btnExport_Click" Text="Export" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <h2>
            Rate Line Details
        </h2>
        <div class="divback" >
            <asp:UpdatePanel ID="UPSecond" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdListStock" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                        Width="100%" OnRowCommand="grdListStock_RowCommand" AllowPaging="true" OnPageIndexChanging="grdListStock_PageIndexChanging"
                        PageSize="10" Height="10px" HeaderStyle-CssClass="HeaderStyle"  CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle" PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                        <Columns>
                           <%-- <asp:BoundField DataField="SerialNumber" HeaderText="" ItemStyle-ForeColor="White" Visible="false"
                                ItemStyle-BorderColor="Black">
                                <ItemStyle Width="0px" />
                            </asp:BoundField>--%>
                            <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="checkAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                            <asp:CheckBox ID="chkUpdate" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="true">
                            <ItemTemplate>
                            
                                <asp:LinkButton ID="lblSrNo" runat="server" Text='<%#Eval("SerialNumber")%>' CommandName="Edit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton >
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="OriginLevel" HeaderText="Origin Level" />
                            <asp:BoundField DataField="Origin" HeaderText="Origin" />
                            <asp:BoundField DataField="DestinationLevel" HeaderText="Destination Level" />
                            <asp:BoundField DataField="Destination" HeaderText="Destination" />
                            <asp:BoundField DataField="RateCardname" HeaderText="Rate Card Name" />
                            <asp:BoundField DataField="Parameter" HeaderText="Parameter">
                            <ItemStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Slabs" HeaderText="Slabs">
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
                             <asp:ButtonField CommandName="Edit" Text="Edit" >
                                <ItemStyle Width="50px"/>
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
                
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnList" EventName="Click" />
                
                </Triggers>
            </asp:UpdatePanel>
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
