<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDriverMaster.aspx.cs" Inherits="ProjectSmartCargoManager.FrmDriverMaster"  MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

<div id="contentarea">
<h1>Drivers </h1>
<div class="botline">

 <table width="100%">
<tr>
<td colspan="2">
    <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
    </td>
    
</tr>

<tr>
<td>
<table cellpadding="3" cellspacing="3">

<tr>

<td class="style1">Name</td>
<td class="style1"> <asp:TextBox ID="TxtName" runat="server" MaxLength="15" Width="80px"></asp:TextBox></td>

<td class="style1"> Licence No</td>
<td class="style1"><asp:TextBox ID="TxtLicenceNo" runat="server" MaxLength="15" Width="80px"></asp:TextBox>  </td>
 
<td class="style1"> Vehicle No </td>
<td class="style1"><asp:TextBox ID="TxtVehicle" runat="server" MaxLength="15" Width="80px"></asp:TextBox>  </td>   
    
<td class="style1">Phone</td>
<td class="style1"> <asp:TextBox ID="TxtPhone" runat="server" MaxLength="15" Width="80px"></asp:TextBox>  </td>  
 
<td class="style1">Status</td>
<td class="style1"> 
<asp:DropDownList ID="DrpStatus" runat="server">
<asp:ListItem Value="0">Select</asp:ListItem>
<asp:ListItem>Active</asp:ListItem>
<asp:ListItem>InActive</asp:ListItem>
</asp:DropDownList>
</td>

   
</tr>

<tr>
<td>
       <asp:Button ID="btnList" runat="server" CssClass = "button" Text="List" 
           onclick="btnList_Click" />
            
       <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
           onclick="btnClear_Click" />
</td>
</tr>

</table>
</td>
</tr>
</table>
</div>

<div>
<h1>Driver Details</h1>

    <asp:GridView ID="GrdDriverDetails" runat="server" AutoGenerateColumns="False" Width="100%"
            AutoGenerateEditButton="false" style="margin-top: 0px" HeaderStyle-CssClass="HeaderStyle" 
            RowStyle-CssClass="RowStyle"  AlternatingRowStyle-CssClass="AltRowStyle" 
            PagerStyle-CssClass="PagerStyle" AllowPaging="True"  PageSize="10" 
        onpageindexchanging="GrdDriverDetails_PageIndexChanging">
            
     <RowStyle CssClass="RowStyle" HorizontalAlign ="Center" ></RowStyle>
           <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Driver Name">
                   <ItemTemplate>
                       <asp:TextBox ID="TxtDriver" runat="server" MaxLength="25" Width="100px" Text='<%#Eval("DriverName") %>'></asp:TextBox>
                   </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Licence No">
                   <ItemTemplate>
                       <asp:TextBox ID="TxtLicenceNo" runat="server" MaxLength="25" Width="100px" Text='<%#Eval("DLNumber") %>'></asp:TextBox>
                   </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Vehicle No">
                   <ItemTemplate>
                       <asp:TextBox ID="TxtVehicleNo" runat="server" MaxLength="25" Width="100px" Text='<%#Eval("VehicleNo") %>'></asp:TextBox>
                   </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Phone">
                   <ItemTemplate>
                       <asp:TextBox ID="TxtPhone" runat="server" MaxLength="25" Width="100px" Text='<%#Eval("Phone") %>'></asp:TextBox>
                   </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="IsActive">
                <ItemTemplate>
                    <asp:CheckBox ID="ChkStatus" runat="server" Checked='<%#Eval("IsActive")%>'/>
                </ItemTemplate>
                </asp:TemplateField>
                
            </Columns>
            
<PagerStyle CssClass="PagerStyle"></PagerStyle>

<HeaderStyle CssClass="HeaderStyle"></HeaderStyle>

<AlternatingRowStyle CssClass="AltRowStyle"></AlternatingRowStyle>
    
    </asp:GridView>
    
    <asp:Button ID="BtnAdd" runat="server" Text="Add" CssClass="button" 
        onclick="BtnAdd_Click" />
    <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button" 
        onclick="BtnSave_Click" />
    
</div>

</div>
</asp:Content>