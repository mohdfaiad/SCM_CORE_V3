<%@ Page Title="ULDList" Language="C#" AutoEventWireup="true" CodeBehind="FrmULDList.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.FrmULDList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script language="javascript" type="text/javascript">

        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=gvResult.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }

        function ValidateRowSelected(action) {
            var gvcheckAllRow;
            gvcheckAllRow = document.getElementById("<%=gvResult.ClientID %>");
            var i = 0;
            var k = 0;
            var selectedRows = '';
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            for (i = 0; i < gvcheckAllRow.rows.length; i++) {
                var inputs = gvcheckAllRow.rows[i].getElementsByTagName('input');
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[0].checked == true) {
                        k = 1;
                        selectedRows = selectedRows + i + ';';
                    }
                }
            }
            document.getElementById("<%=hdnSelectedRows.ClientID %>").value = selectedRows;
            if (k <= 0) {
                alert('Please select atleast one row from the list');
                return (false);
            }
            else {
                return (true);
            }
        }

        function UserDeleteULDConfirmation() {
            return confirm("Are you sure you want to delete selected ULD(s)?");
        }
        
    </script>

    <style type="text/css">
        .style1
        {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
        <h1>
            List ULDs</h1>
        <%--<div class="botline">--%>
        <div class="botline">
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Size="Large" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="3" cellspacing="3">
                            <tr>
                                <td class="style1">
                                    Location
                                </td>
                                <td class="style1">
                                    <asp:DropDownList ID="ddlOrigin" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td class="style1">
                                    ULD #
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="txtULDNo" runat="server" MaxLength="15" Width="80px"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    ULD Type
                                </td>
                                <td class="style1">
                                    <asp:DropDownList ID="drpULDType" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td class="style1">
                                    ULD Owner
                                </td>
                                <td class="style1">
                                    <asp:DropDownList ID="drpOwner" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td class="style1">
                                    ULD Use Status
                                </td>
                                <td class="style1">
                                    <asp:DropDownList ID="drpUseStatus" runat="server">
                                        <asp:ListItem Value="0">ALL</asp:ListItem>
                                        <asp:ListItem Value="1">Empty</asp:ListItem>
                                        <asp:ListItem Value="2">Partial</asp:ListItem>
                                        <asp:ListItem Value="3">Full</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan=2>
                                    <asp:Button ID="btnSearch" runat="server" Text="List"
                                        CssClass="button" OnClick="btnSearch_Click" />
                                    &nbsp;<asp:Button ID="btnClear" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>"
                                        CssClass="button" OnClick="btnClear_Click" />
                                        &nbsp;<asp:Button ID="btnExport" runat="server" Text="Export"
                                        CssClass="button" OnClick="btnExport_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <h1>
                ULD Details</h1>
            <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" Width="100%"
                AutoGenerateEditButton="false" Style="margin-top: 0px" HeaderStyle-CssClass="HeaderStyle"
                RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle" PagerStyle-CssClass="PagerStyle"
                OnRowCommand="gvResult_RowCommand" AllowPaging="True" OnPageIndexChanging="gvResult_PageIndexChanging"
                PageSize="10" ShowHeaderWhenEmpty="True" EmptyDataText="No Data Found">
                <RowStyle CssClass="RowStyle" HorizontalAlign="Center"></RowStyle>
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkList" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ULD#">
                        <ItemTemplate>
                            <asp:LinkButton ID="lblULDNo" runat="server" Text='<%#Eval("ULDNumber") %>' CommandArgument='<%# Eval("ULDNumber") %>'
                                CommandName="ShowULDDeatils"></asp:LinkButton>
                            <%--<asp:Label ID="lblULDNo" runat="server" Text = '<%# Eval("ULDNumber") %>'></asp:Label>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ULD Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("ULDStatus") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ULD Use Status">
                        <ItemTemplate>
                            <asp:Label ID="lblUseStatus" runat="server" Text='<%# Eval("UseStatus") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Updated On">
                        <ItemTemplate>
                            <asp:Label ID="lblLocOn" runat="server" Text='<%# Eval("LocatedOn") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle"></EmptyDataRowStyle>
                <PagerStyle CssClass="PagerStyle"></PagerStyle>
                <HeaderStyle CssClass="HeaderStyle"></HeaderStyle>
                <AlternatingRowStyle CssClass="AltRowStyle"></AlternatingRowStyle>
            </asp:GridView>
            <asp:Button ID="btnULDMovementHistory" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_GETMOVEMENTHISTORY %>"
                CssClass="button" OnClick="btnULDMovementHistory_Click" Visible="False" />
            &nbsp;<asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button" OnClick="btnDelete_Click"
                OnClientClick="if(ValidateRowSelected('ValidateULD')) { if ( ! UserDeleteULDConfirmation()) return false } else return (false);"
                Visible="false" />
            <asp:HiddenField ID="hdnSelectedRows" runat="server" />
        </div>
    </div>
</asp:Content>
