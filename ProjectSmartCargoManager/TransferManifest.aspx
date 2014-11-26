<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransferManifest.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.TransferManifest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div style="font-size:medium">
Search Criteria
</div>
 <table width="100%">
 <tr>
 <td>
 CTM Ref.
 </td>
 <td>
     <asp:TextBox ID="TextBox1" runat="server" Width="60px" ></asp:TextBox>
     
 </td>
 <td>
 CTM Type
 </td>
 <td>
     <asp:DropDownList ID="DropDownList1" runat="server">
     </asp:DropDownList>
 </td>
 <td>
 Creation Date
 </td>
 <td>
     <asp:TextBox ID="TextBox2" runat="server"  Width="60px" ></asp:TextBox>
       <asp:ImageButton ID="btnCountryCode" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" />
 </td>
 <td>
 Airport
 </td>
 <td>
     <asp:TextBox ID="TextBox3" runat="server" Width="60px" ></asp:TextBox>
 </td>
 </tr>
 <tr>
 <td>
 From Carrier/Flight
 </td>
 <td>
     <asp:TextBox ID="TextBox4" runat="server" Width="60px" ></asp:TextBox>
     &nbsp;&nbsp;&nbsp;
     <asp:TextBox ID="TextBox5" runat="server" Width="50px" ></asp:TextBox>
 </td>
 <td>
 Date
 </td>
 <td>
  <asp:TextBox ID="TextBox6" runat="server" Width="60px" ></asp:TextBox>
   &nbsp;<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" />
 </td>
 <td>
 To Carrier/Flight
 </td>
 <td>
     <asp:TextBox ID="TextBox7" runat="server" Width="50px" ></asp:TextBox>
     &nbsp;&nbsp;&nbsp;
     <asp:TextBox ID="TextBox8" runat="server" Width="50px" ></asp:TextBox>
 </td>
 <td>
 Date
 </td>
 <td>
  <asp:TextBox ID="TextBox9" runat="server" Width="60px" ></asp:TextBox>
   &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" />
 </td>
 </tr>
 </table> 
 

<table width="100%">
<tr>
<td>
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
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
                
               <asp:TemplateField HeaderText="AWB Number" HeaderStyle-Wrap="true">
                    
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Owner Code" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Std Pcs" HeaderStyle-Wrap="false">
                    
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Std Wt." HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Wt Unit" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Std vol" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vol Unit" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Act Pcs" HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Act Wt." HeaderStyle-Wrap="false">
                   
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Wt Unit" HeaderStyle-Wrap="false">
                   
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
    <td align="right">
      <asp:Button ID="Button1" runat="server" Text="Add/Update" CssClass="button" 
            onclick="Button1_Click"/>
      
       &nbsp;<asp:Button ID="Button2" runat="server" Text="Delete"  CssClass="button"/>
    </td>
    
   </tr>
   <tr>
   <td align="right" >
       <asp:Button ID="Button3" runat="server" Text="Dupicate" CssClass="button" />
        &nbsp;<asp:Button ID="Button4" runat="server" Text="Print" CssClass="button" />
         &nbsp;<asp:Button ID="Button5" runat="server" Text="Save" CssClass="button" />
          &nbsp;<asp:Button ID="Button6" runat="server" Text="Close" CssClass="button" />
   </td>
   </tr>
   </table> 
</asp:Content> 