<%@ Page Title="AircraftLoadability" Language="C#" AutoEventWireup="true" CodeBehind="AirCraftLoadability.aspx.cs" Inherits="ProjectSmartCargoManager.AirCraftLoadability" MasterPageFile="~/SmartCargoMaster.Master"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script language="javascript" type="text/javascript">
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
        {  display: none;
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
        .white_content
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
<asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
  <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
    <div id="contentarea">
    <h1>
    Aircraft Loadability
    </h1>
    
        <asp:Label ID="lblStatus" runat="server" Font-Bold="true"></asp:Label>
        <div class="botline">
    <table width="99%">
    <tr>
    <td>
    Aircraft Type
    </td>
    <td>
        <asp:DropDownList ID="ddlType" runat="server">
        </asp:DropDownList>
    </td>
    <td>Tail Id</td>
    <td><asp:TextBox ID="txtTailId" runat="server"></asp:TextBox>
    </td>
     <td>Compartment</td>
    <td><asp:TextBox ID="txtCompartmnt" runat="server"></asp:TextBox>
</td>
<td>Active</td>
    <td>
        <asp:CheckBox ID="chkAct" runat="server" /></td>
        
    </tr>
    <tr>
    <td>Max. Length</td>
    <td>
        <asp:TextBox ID="txtLgth" runat="server" style="text-align:right"></asp:TextBox>
        </td>
        <td>Max. Width</td>
    <td>
        <asp:TextBox ID="txtWidth" runat="server" style="text-align:right"></asp:TextBox>
        </td>
        <td>Max. Height</td>
    <td>
        <asp:TextBox ID="txtHght" runat="server" style="text-align:right"></asp:TextBox>
        </td>
        <td>Max. Volume</td>
    <td>
    <asp:TextBox ID="txtMaxVol" runat="server" style="text-align:right"></asp:TextBox>
        </td>
    </tr>
    
    <tr>
    <td>Max. Containers</td>
    <td>
        <asp:TextBox ID="txtMaxContainers" runat="server" style="text-align:right"></asp:TextBox>       
        </td>
        <td>Container type</td>
    <td>
        <asp:TextBox ID="txtContainertype" runat="server" style="text-align:right"></asp:TextBox>
        </td>
        <td>Max. Pallets96</td>
    <td>
        <asp:TextBox ID="txtMaxPallets96" runat="server" style="text-align:right"></asp:TextBox>
        </td>
        <td>Max. Pallets88</td>
    <td>
    <asp:TextBox ID="txtMaxPallets88" runat="server" style="text-align:right"></asp:TextBox>
        </td>
    </tr>
    
    <tr>
    <td>Floor Limitation</td>
    <td>
        <asp:TextBox ID="txtFloorLamination" runat="server" style="text-align:right"></asp:TextBox>       
        </td>
        <td>Valid From</td>
    <td><asp:TextBox ID="txtValidFrm" runat="server"></asp:TextBox>
        <asp:ImageButton ID="btnValidFrom" runat="server" ImageAlign="AbsMiddle" 
            ImageUrl="~/Images/calendar_2.png" />
    <asp:CalendarExtender TargetControlID="txtValidFrm" ID="ext_txtValidFrm" runat="server" Format="dd/MM/yyyy"  PopupButtonID="btnValidFrom"></asp:CalendarExtender>
    </td>
     <td>Valid To</td>
    <td><asp:TextBox ID="txtValidTo" runat="server"></asp:TextBox>
        <asp:ImageButton ID="btnValidTo" runat="server" ImageAlign="AbsMiddle" 
            ImageUrl="~/Images/calendar_2.png" />
     <asp:CalendarExtender TargetControlID="txtValidTo" ID="ext_txtValidTo" runat="server" Format="dd/MM/yyyy" PopupButtonID="btnValidTo"></asp:CalendarExtender>
</td>
        <td>Max. Weight</td>
    <td><asp:TextBox ID="txtMaxWeight" runat="server"></asp:TextBox></td>
    </tr>
    
    <tr>
    <td colspan="2">
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click"/>
    <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click" OnClientClick="callShow();" CausesValidation="false"/>
    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" CausesValidation="false" 
            onclick="btnClear_Click" />
    </td>
    </tr>
    
    </table>
  </div>
  <div class="ltfloat" style="width:100%">
        <asp:GridView ID="GrdLoadability" runat="server" AutoGenerateColumns="false" Width="100%"
        OnRowCommand="GrdLoadability_RowCommand" OnRowEditing="GrdLoadability_RowEditing"
        OnPageIndexChanging="GrdLoadability_PageIndexChanging" AllowPaging="true" PageSize="10"
     HeaderStyle-CssClass="titlecolr">
        <Columns>
        <asp:TemplateField HeaderText="Sr No" ItemStyle-HorizontalAlign="Center" Visible="false">
        <ItemTemplate>
            <asp:Label ID="lblSrNum" runat="server" Text='<%#Eval("SerialNumber")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Aircraft Type" ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
            <asp:Label ID="lblType" runat="server" Text='<%#Eval("AirCraftType")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Tail Id" ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
            <asp:Label ID="lblTailId" runat="server" Text='<%#Eval("Tailid")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Compart." ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
            <asp:Label ID="lblCompartment" runat="server" Text='<%#Eval("Compartment")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Max. Length" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <asp:Label ID="lblLgth" runat="server" Text='<%#Eval("Length","{0:n}")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Max. Width" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <asp:Label ID="lblWidth" runat="server" Text='<%#Eval("Width","{0:n}")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Max. Height" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <asp:Label ID="lblHght" runat="server" Text='<%#Eval("Height","{0:n}")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Max. Volume" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <asp:Label ID="lblMaxVolumeinCMeter" runat="server" Text='<%#Eval("MaxVolumeinCMeter","{0:n}")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Max. Weight" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <asp:Label ID="lblMaxWeightinkg" runat="server" Text='<%#Eval("MaxWeightinkg","{0:n}")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Max. Containers" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <asp:Label ID="lblMaxContainers" runat="server" Text='<%#Eval("MaxContainers")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Container Type" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <asp:Label ID="lblContainerType" runat="server" Text='<%#Eval("ContainerType")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Max. Pallet96" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <asp:Label ID="lblMaxPallets96inch" runat="server" Text='<%#Eval("MaxPallets96inch")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Max. Pallet88" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <asp:Label ID="lblMaxPallets88inch" runat="server" Text='<%#Eval("MaxPallets88inch")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Floor Limination" ItemStyle-HorizontalAlign="Right">
        <ItemTemplate>
            <asp:Label ID="lblFloorLimitation" runat="server" Text='<%#Eval("FloorLimitation")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Valid From" ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
            <asp:Label ID="lblValidFrm" runat="server" Text='<%#Eval("ValidFrom", "{0:dd/MM/yyyy}")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Valid To" ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
            <asp:Label ID="lblValiidTo" runat="server" Text='<%#Eval("ValidTo", "{0:dd/MM/yyyy}")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <%--<asp:TemplateField HeaderText="unit" ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
            <asp:Label ID="lblUnit" runat="server" Text='<%#Eval("Unit")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>--%>
        
        <asp:TemplateField HeaderText="Acitve" ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
            <asp:Label ID="lblAct" runat="server" Text='<%#Eval("IsActive")%>'></asp:Label>
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
        <div id="msglight" class="white_content">
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
    
    <div id="msgfade" class="black_overlay">
    </div>
 
    
    </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
