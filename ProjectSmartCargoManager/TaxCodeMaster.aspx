<%@ Page Title="TaxCode Master" Language="C#" AutoEventWireup="true" CodeBehind="TaxCodeMaster.aspx.cs" Inherits="ProjectSmartCargoManager.TaxCodeMaster" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
    <script type="text/javascript">
        function CheckBlank() 
        {
            var TaxCode = document.getElementById('ctl00_ContentPlaceHolder1_txtOcCode').value;
            var TaxCodeDesc = document.getElementById('ctl00_ContentPlaceHolder1_txtOcDesc').value;

            if (TaxCode == '' || TaxCodeDesc == '')
             {
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
  

    <asp:UpdatePanel ID="UpdateWhole" runat = "server">
    <ContentTemplate>
    
    <div id="contentarea">
    <div class="msg">
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
    </div>
         <h1> 
             Tax Code Master
         </h1>
<div id = "New" class = "botline">
<table width="80%" border="0">
                <tr>
                    <td>
                      Tax Code
                    </td>
                    <td>
                        <asp:TextBox ID="txtOcCode" runat="server" Width="110px" 
                            ToolTip="Tax Code" AutoPostBack="false"></asp:TextBox>
                    </td>
                    <td>
                        TaxCode Description
                    </td>
                    <td>
                        <asp:TextBox ID="txtOcDesc" runat="server" Width="110px" 
                            ToolTip="TaxCode Desciption" AutoPostBack="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
                            Visible="true" onclick="btnList_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add/Update" 
                            Visible="true" onclick="btnAdd_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnDelete0" runat="server" CssClass="button" Text="Delete" 
                            Visible="true" onclick="btnDelete0_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" 
                            Visible="true" onclick="btnClear_Click" />
                    </td>
                </tr>
                </table>    
</div>

<div class = "ltfloat" style = "width:80%">
        <asp:GridView ID="grdTaxCode" runat="server" ShowFooter="false" Width="100%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
            AutoGenerateSelectButton="True" onrowcommand="grdTaxCode_RowCommand" 
            onpageindexchanging="grdTaxCode_PageIndexChanging">
            
            <Columns>
             <asp:TemplateField HeaderText="Tax Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblOcCode" runat="server" Text = '<%# Eval("TaxCode") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="TaxCode Description" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblOcDesc" runat="server" Text = '<%# Eval("TaxCodeDescription") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>


             <asp:TemplateField HeaderText="Active" HeaderStyle-Wrap="true" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblisact" runat="server" Text = '<%# Eval("isActive") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>

             
             <%--<asp:TemplateField HeaderText="Country Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="txtPCode" runat="server" Text = '<%# Eval("CountryCode") %>'/>
                    </ItemTemplate>
<HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>--%>
             
            </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
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
    </asp:UpdatePanel>
    
</asp:Content>