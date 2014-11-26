
<%@ Page Title="CurrencyMaster" Language="C#" AutoEventWireup="true" CodeBehind="frmCurrencyMaster.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmCurrencyMaster" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <script type="text/javascript">
        function CheckBlank() {
            var CurrCode = document.getElementById('ctl00_ContentPlaceHolder1_txtCurrCode').value;
            var CurrName = document.getElementById('ctl00_ContentPlaceHolder1_txtCurrName').value;
            if (CurrCode == '' || CurrName == '') {
                alert('Entries Cant be Blank');
                return false;
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

    <asp:UpdatePanel ID="UpdateWhole" runat = "server">
    <ContentTemplate>
    
    <div id="contentarea">
    <div class="msg">
     <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
    </div>
         <h1> 
             Currency Master
         </h1>
         
<div id = "New" class = "botline">
<table width="80%" border="0">
                <tr>
                    <td>
                        Currency Code* 
                    </td>
                    <td>
                        <asp:TextBox ID="txtCurrCode" runat="server" Width="110px" 
                            ToolTip="Currency Code" AutoPostBack="false"></asp:TextBox>
                    </td>
                    <td>
                        Currency Name * 
                    </td>
                    <td>
                        <asp:TextBox ID="txtCurrName" runat="server" Width="110px" 
                            ToolTip="Currency Name" AutoPostBack="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td colspan=4>
                <asp:Button ID="btnAdd" runat="server" CssClass="button" onclick="btnAdd_Click" Text="Add/Update" Visible="true" />
                <asp:Button ID="btnDelete0" runat="server" CssClass="button" onclick="btnDelete_Click" Text="Delete" Visible="true" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" Visible="true" onclick="btnClear_Click"/>
                    <asp:Button ID="btnList" runat="server" CssClass="button" 
                        onclick="btnList_Click" Text="List" Visible="true" />
                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click" />
                </td>
                </tr>
                </table>    
</div>

<div class = "ltfloat" style = "width:80%">
<asp:GridView ID="grdCurrency" runat="server" ShowFooter="false" Width="100%" AutoGenerateColumns="False" 
 CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" onrowcommand="grdCurrency_RowCommand"
 onpageindexchanging="grdCurrency_PageIndexChanging" AlternatingRowStyle-CssClass="AltRowStyle"  
CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle" PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
 SelectedRowStyle-CssClass="SelectedRowStyle">
            <Columns>
             <asp:TemplateField HeaderText="Currency Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="txtgrdCurrCode" runat="server" Text = '<%# Eval("CurrencyCode") %>'/>
                    </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Currency Name" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="txtgrdCurrName" runat="server" Text = '<%# Eval("CurrencyName") %>'/>
                    </ItemTemplate>
             </asp:TemplateField>


             <asp:TemplateField HeaderText="Active" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblisact" runat="server" Text = '<%# Eval("IsActive") %>'/>
                    </ItemTemplate>
             </asp:TemplateField>

             
             <%--<asp:TemplateField HeaderText="Country Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="txtPCode" runat="server" Text = '<%# Eval("CountryCode") %>'/>
                    </ItemTemplate>
             </asp:TemplateField>--%>
             
             <asp:ButtonField CommandName="Select" Text="Edit">
             </asp:ButtonField>
             
            </Columns>
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

