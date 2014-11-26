<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OCMaster.aspx.cs"  Title="Other Charges Master" Inherits="ProjectSmartCargoManager.OCMaster" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
    <script type="text/javascript">
        function CheckBlank() {
            var OcCode = document.getElementById('ctl00_ContentPlaceHolder1_txtOcCode').value;
            var OcDesc = document.getElementById('ctl00_ContentPlaceHolder1_txtOcDesc').value;
            if (OcCode == '' || OcDesc == '') {
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
             OC Master
         </h1>
         <div id = "New" class = "botline">
<table width="80%" border="0">
                <tr>
                    <td>
                      OC Code
                    </td>
                    <td>
                        <asp:TextBox ID="txtOcCode" runat="server" Width="110px" 
                            ToolTip="OC Code" AutoPostBack="false"></asp:TextBox>
                    </td>
                    <td>
                        OC Description
                    </td>
                    <td>
                        <asp:TextBox ID="txtOcDesc" runat="server" Width="110px" 
                            ToolTip="OC Desciption" AutoPostBack="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                   <td>
                   
                   </td>
                </tr>
                
                <tr>
                   <td>
                   
                   </td>
                </tr>
                
                <tr>
                    <td colspan="4">
                        <asp:Button ID="btnAdd" runat="server" CssClass="button" onclick="btnAdd_Click" 
                            Text="Add/Update" Visible="true" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnDelete0" runat="server" CssClass="button" 
                            onclick="btnDelete_Click" Text="Delete" Visible="true" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnClear" runat="server" CssClass="button" 
                            onclick="btnClear_Click" Text="Clear" Visible="true" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnList" runat="server" CssClass="button" 
                            onclick="btnList_Click" Text="List" Visible="true" />
                    </td>
                </tr>
                </table>    
</div>
<div class = "ltfloat" style = "width:80%">
        <asp:GridView ID="grdOc" runat="server" AllowPaging="True" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" onpageindexchanging="grdOc_PageIndexChanging" 
            onselectedindexchanged="grdOc_SelectedIndexChanged" Width="100%">
            <Columns>
                <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="OC Code">
                    <ItemTemplate>
                        <asp:Label ID="lblOcCode" runat="server" Text='<%# Eval("OCCode") %>' />
                    </ItemTemplate>
                    <HeaderStyle Wrap="True" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="OC Description">
                    <ItemTemplate>
                        <asp:Label ID="lblOcDesc" runat="server" Text='<%# Eval("OCDesc") %>' />
                    </ItemTemplate>
                    <HeaderStyle Wrap="True" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Active" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblisact" runat="server" Text='<%# Eval("IsActiove") %>' />
                    </ItemTemplate>
                    <HeaderStyle Wrap="True" />
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Country Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="txtPCode" runat="server" Text = '<%# Eval("CountryCode") %>'/>
                    </ItemTemplate>
<HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>--%><asp:CommandField SelectText="Edit" 
                    ShowSelectButton="True" />
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
    </asp:UpdatePanel>
    
</asp:Content>

