<%@ Page Title="RouteList" Language="C#" AutoEventWireup="true" CodeBehind="ListRoute.aspx.cs" Inherits="ProjectSmartCargoManager.ListRoute" MasterPageFile="~/SmartCargoMaster.Master"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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
            .style1
            {width:20px;}
            
	</style>
	<script type="text/javascript">
	    function check() {
	        var ddlOrigin = document.getElementById("<%= ddlOrigin.ClientID%>").value;
	        var ddlDestination = document.getElementById("<%= ddlDest.ClientID%>").value;
	        if (ddlOrigin != "Select Origin" && ddlDestination == "Select Destination") {
	            alert('Select Both Origin and Destination');
	            return false;
	        }
	        if (ddlOrigin == "Select Origin" && ddlDestination != "Select Destination") {
	            alert('Select Both Origin and Destination');
	            return false;
	        }
	      
	    }
	</script>

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
     <br /><br /><br />
     <div class="msg">
     <asp:Label ID="lblstatus" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
     </div>
     <h1>Route List</h1>
     <div>
     <br />
     <table>
     <tr>
     <td>Origin:</td>
     <td>
     <asp:DropDownList ID="ddlOrigin" runat="server"></asp:DropDownList>
     </td>
     <td class="style1"></td>
     <td>Destination:</td>
     <td>
     <asp:DropDownList ID="ddlDest" runat="server"></asp:DropDownList>
     </td>
     <td class="style1"></td>
     </tr>
     <tr>
     <td colspan=6>
     <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click"/>
     <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click" />
     </td>
     </tr>
     </table>
     <br />
     <div>
         <asp:GridView ID="ListGrd" runat="server" AutoGenerateColumns="false" Width="100%"
         AllowPaging="true" PageSize="10" onpageindexchanging="ListGrd_PageIndexChanging"
          HeaderStyle-CssClass="titlecolr">
         <Columns>
         
         <asp:TemplateField HeaderText="Serial No." ItemStyle-HorizontalAlign="Center" Visible="false" >
    <ItemTemplate>
    <asp:Label runat="server" ID="lblsrno" Text='<%#Eval("SerialNumber")%>'></asp:Label>
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

