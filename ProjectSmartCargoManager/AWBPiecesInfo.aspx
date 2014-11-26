<%--

2012-04-05  vinayak
2012-07-24  vinayak

--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AWBPiecesInfo.aspx.cs" Inherits="ProjectSmartCargoManager.AWBPiecesInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="style/style.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dimensions</title>

    <script language="javascript" type="text/javascript">
        function Error() {

            alert("Please check the values");

        }
        function NotInserted() {

            alert("Record Not Inserted Please try Again..");

        }

        function CloseWindow(selectedval, id, volume) 
        {
            window.close();            
        } 
        
    </script>

</head>
<body class="divback">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="True">
    </asp:ToolkitScriptManager>
    <div style="width: 400px; padding-left: 20px;">
        <br />
        <table style="height: 25px" width="300px">            
            <tr>
                <td style="font-size: medium;">
                    Pcs Count
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblPcsEntered" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="90px" valign="top" style="font-size: medium;">
                    Gross Wt.
                </td>
                <td valign="top">
                    <asp:Label ID="lblGrossWtEntered" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel runat="server" ID="UPDimension">
            <ContentTemplate>
                <br />
                <asp:Label ID="LBLStatus" runat="server" Text="" ForeColor="Red"></asp:Label>
                <br />
                <asp:GridView ID="grdAWBPieceDetails" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                    ShowFooter="True" Width="450px" Height="21px">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="CHKSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pieces" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPcs" runat="server" Width="80px" EnableViewState="true" Text='<%# Eval("Pieces") %>' ></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gross Wt." HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtGrossWt" runat="server" EnableViewState="true" Width="80px" Text='<%# Eval("GrossWt") %>' ></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PieceId" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPieceId" runat="server" Width="80px" Text='<%# Eval("PieceId") %>' AutoPostBack="True" OnTextChanged="txtPcs_TextChanged"></asp:TextBox>                                
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="false" />
                            <FooterTemplate>
                                &nbsp; &nbsp;
                                <asp:Button ID="btnAddRow" runat="server" CssClass="button" Text="Add" OnClick="btnAddRow_Click" />
                                &nbsp; &nbsp;
                                <asp:Button ID="btnDeleteRow" runat="server" CssClass="button" Text="Delete" OnClick="btnDeleteRow_Click" />
                            </FooterTemplate>
                        </asp:TemplateField>                        
                    </Columns>
                    <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
                </asp:GridView>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblTotalPieces" Style="font-size: medium; font-weight: bold;" runat="server"
                                Text="Total Pieces"></asp:Label>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="TXTFinalPieces" runat="server" Text="" Width="100px"></asp:TextBox>                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTotalGrossWt" Style="font-size: medium; font-weight: bold;" runat="server"
                                Text="Total Gross Wt."></asp:Label>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="TXTFinalWt" runat="server" Text="" Width="100px"></asp:TextBox>
                            &nbsp;Kg
                        </td>
                    </tr>
                </table>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click">
        </asp:Button>
        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
        </asp:Button>
    </div>
    </form>
</body>
</html>
