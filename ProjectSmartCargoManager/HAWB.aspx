<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HAWB.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.HAWB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div>
Search Criteria
</div> 
<table width="100%">
<tr>
<td>
 AWB No.
</td>
<td>
    <asp:TextBox ID="TextBox1" runat="server" Width="45px" ></asp:TextBox>
    &nbsp;<asp:Label ID="Label1" runat="server" Text="-"></asp:Label>
    &nbsp;<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
</td>
<td>
   HAWB No.
</td>
<td>
    <asp:TextBox ID="TextBox3" runat="server" Width="80px" ></asp:TextBox>
</td>
<td>
    <asp:Button ID="Button1" runat="server" Text="List" CssClass="button"/>

    &nbsp;<asp:Button ID="Button2" runat="server" Text="Close" CssClass="button"/>
</td>

</tr>

</table> 
<div>
HAWB Details
</div> 
<table width="100%">
<tr>
<td>
 <asp:GridView ID="grdCountry" runat="server" AutoGenerateColumns="False" 
                                    ShowFooter="True"   Width="100%">
            <Columns>
             <asp:TemplateField>
                    <HeaderTemplate>
                        <input type="checkbox" name = "checkall"  onclick="javascript:SelectAll(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="check" runat="server" />
                    </ItemTemplate>
                    
                </asp:TemplateField>
               <asp:TemplateField HeaderText="HAWB No." HeaderStyle-Wrap="true">
                    
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Consol" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Std Pcs" HeaderStyle-Wrap="false">
                    
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Std Wt" HeaderStyle-Wrap="false">
                    
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SLAC pcs" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shipper" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Consignee" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Destination" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Remarks" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>
</td>
</tr>
</table>

<table width="100%">
<tr>
<td align ="right">
    <asp:Button ID="Button3" runat="server" Text="Send FHL" CssClass="button" />
    
    &nbsp;<asp:Button ID="Button4" runat="server" Text="House Consol" CssClass="button"/>
    &nbsp;<asp:Button ID="Button5" runat="server" Text="Details" CssClass="button"/>
    &nbsp;<asp:Button ID="Button6" runat="server" Text="Save" CssClass="button" />
</td>
</tr>
</table> 
</asp:Content>