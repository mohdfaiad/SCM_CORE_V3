<%@ Page Title="Build Prefered Route" Language="C#" AutoEventWireup="true" CodeBehind="BuildRoute.aspx.cs" ValidateRequest="false" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.BuildRoute" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
	    function check() {
	        var text = document.getElementById("<%= txtRoute.ClientID%>").value;
	        var ddlOrigin = document.getElementById("<%= ddlOrigin.ClientID%>").value;
	        var ddlDest = document.getElementById("<%= ddlDest.ClientID%>").value;
	        var myRegExp = /^[A-Za-z]{3}[-]*[A-Za-z]{0,3}$/;
	        if (ddlOrigin == "Select Origin" || ddlDest == "Select Destination") {
	            alert('Select Both Origin and Destination');
	            return false;
	        }

	        else {
	            if (myRegExp.test(text)) {
	                return true;
	            }
	            else {
	                alert("Enter in correct format");
	                return false;
	            }
	            return true;
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
                  z-index:1001;
                  -moz-opacity:0.8;
                  opacity:0.8;
                  filter:alpha(opacity=80);
            }
            .white_content 
            {
                margin:0 auto;
                  display: none;
                  position: absolute;
                  top: 30%;
                  left: 35%;
                  width: 30%;
                  height: 45%;
                  padding: 16px;
                  border: 16px solid #ccdce3;
                  background-color: white;
                  z-index:1002;
                  overflow: auto;
            
            }
            
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="toolscript1" runat="server">
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
        .style1
        {
            height: 36px;
        }
    </style>
    
    <asp:UpdatePanel ID="UP1" runat="server">
     <ContentTemplate>
     <div id="contentarea">
     <div class="msg">
     <asp:Label ID="lblstatus" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
     </div>
     <h1>Build Prefered Route</h1>
     <div class="botline">
     <table width="65%">
     <tr>
     <td>Origin</td>
     <td>
     <asp:DropDownList ID="ddlOrigin" runat="server" Width="70px" Height="20px"
        onselectedindexchanged="ddlOrigin_SelectedIndexChanged">  
     </asp:DropDownList>
     </td>
     <td>Destination</td>
      <td>
     <asp:DropDownList ID="ddlDest" runat="server" Width="70px" Height="20px" 
      onselectedindexchanged="ddlDest_SelectedIndexChanged">
     </asp:DropDownList>
     </td>
     <td>Via</td>
     <td>
      <asp:DropDownList ID="ddlVia" runat="server" Width="70px" Height="20px">
     </asp:DropDownList>
     </td>
     <td>
     <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" onclick="btnAdd_Click"/>
     <asp:Button ID="btnDel" runat="server" Text="Delete" CssClass="button" onclick="btnDel_Click"/>
     </td> 
     <td>
     <%--<asp:Label ID="srclbl" runat="server"></asp:Label>--%>
     <asp:TextBox ID="txtRoute" runat="server" Height="20px" Width="120px" ReadOnly="true"></asp:TextBox>
     <%--<asp:Label ID="destlbl" runat="server"></asp:Label>--%>
     </td>
     <td>
         <asp:CheckBox ID="chkAct" runat="server" Text="Active"/>
     </td>
     <%--<td>Active</td>--%>
     
     </tr>
     <tr>
     <td colspan=9>
     <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="button" OnClick="btnsave_Click"/>
     <asp:Button ID="tnlist" runat="server" Text="List" CssClass="button" OnClick="btnList_Click"/>
     <asp:Button ID="btnClear" runat="server" Text="Clear" onclick="btnClear_Click" CssClass="button"/>
     <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click" />
     </td>
     </tr>
    </td>
     </table>
     </div>
     <br /><br />
     <div class="ltfloat" style="width:100%">
    <asp:GridView ID="listRouteGrid" runat="server" AutoGenerateColumns="false" 
   AllowPaging="true" PageSize="10" BorderColor="#BFBEBE"  HeaderStyle-CssClass="titlecolr"
   onpageindexchanging="listRouteGrid_PageIndexChanging" 
             OnRowCommand="listRouteGrid_RowCommand" Width="100%" 
             onrowediting="listRouteGrid_RowEditing">
    <Columns>
    
    <asp:TemplateField HeaderText="Serial No." ItemStyle-HorizontalAlign="Center" Visible="false">
    <ItemTemplate>
    <asp:Label runat="server" ID="lblsrnoo" Text='<%#Eval("SerialNumber")%>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Source" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
    <ItemTemplate>
    <asp:Label runat="server" ID="lblsource" Text='<%#Eval("Source")%>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Destination" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
    <ItemTemplate>
    <asp:Label runat="server" ID="lbldest" Text='<%#Eval("Destination")%>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Route" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="130px">
    <ItemTemplate>
    <asp:Label runat="server" ID="lblroute" Text='<%#Eval("Route")%>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
    <ItemTemplate>
    <asp:Label runat="server" ID="lblact" Text='<%#Eval("IsActive")%>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:ButtonField CommandName="Edit" Text="Edit">
    <ItemStyle Width="50px"/>
    </asp:ButtonField>
    
    <asp:ButtonField CommandName="DeleteRecord" Text="Delete">
    <ItemStyle Width="50px"/>
    </asp:ButtonField>
    
    </Columns>
    </asp:GridView></div>
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
    
