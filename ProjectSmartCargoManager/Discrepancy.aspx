<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Discrepancy.aspx.cs" Inherits="ProjectSmartCargoManager.Discrepancy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" >
    <div>
    
    <div style="overflow:auto">
            <asp:GridView ID="Grdiscrepancy" runat="server"
             AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
             CellPadding="2" CellSpacing="3" >
                    <Columns>
                   <asp:TemplateField HeaderText="Discrepancy Reason" HeaderStyle-Wrap="false" 
                            ItemStyle-Wrap="false">
                     <ItemTemplate>
                        <asp:DropDownList ID="ddlDiscrerpancy" runat="server">
                        </asp:DropDownList>
                     </ItemTemplate>
             
                  </asp:TemplateField>
                  
                   <asp:TemplateField HeaderText="Pieces" HeaderStyle-Wrap="false" 
                            ItemStyle-Wrap="false">
                     <ItemTemplate>
                         <asp:TextBox ID="txtpieces" runat="server"></asp:TextBox>
                     </ItemTemplate>
                    <ItemStyle Wrap="False">
                    </ItemStyle>
                  </asp:TemplateField>
                  
                   <asp:TemplateField HeaderText="Remaining Pieces" HeaderStyle-Wrap="false" 
                            ItemStyle-Wrap="false">
                     
                     <ItemTemplate>
                         <asp:TextBox ID="txtRemainingpieces" runat="server"></asp:TextBox>
                     </ItemTemplate>
                      <FooterTemplate>
                     <asp:Button ID="btnAdd" runat="server" AutoPostBack="true" CssClass="button" 
                      Text="Add New" ValidationGroup="Gen" onclick="btnAdd_Click" Height="26px" 
                      Width="62px"/>
                     </FooterTemplate>

<HeaderStyle Wrap="False"></HeaderStyle>

<ItemStyle Wrap="False"></ItemStyle>
                  </asp:TemplateField>
                  
                   
                  
             </Columns> 
              <PagerStyle BorderStyle="Groove" />
              <%--<HeaderStyle CssClass="titlecolrforExport" />
              <AlternatingRowStyle CssClass="trcolor" />
              <EditRowStyle CssClass="grdrowfont" />
              <RowStyle CssClass="grdrowfont" HorizontalAlign="Center"/>
             <FooterStyle CssClass="grdrowfont"/>--%>
          </asp:GridView> 
     </div> 
     <table width="41%">
     <tr>
     <td>
      <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="button" />
     </td>
     </tr>
     </table>  
     
    </div>
    </form>
</body>
</html>
