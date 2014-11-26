<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmStockMaster.aspx.cs" Inherits="ProjectSmartCargoManager.FrmStockMaster" %>
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="StockMaster.aspx.cs" Inherits="ProjectSmartCargoManager.StockMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
    function SelectAll(CheckBoxControl) {

        if (CheckBoxControl.checked == true) {
            var i;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox')
        && (document.forms[0].elements[i].name.indexOf('grdStockUpd') > -1)) {
                    document.forms[0].elements[i].checked = true;
                }
            }
        }
        else {

            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
        (document.forms[0].elements[i].name.indexOf('grdStockUpd') > -1)) {
                    document.forms[0].elements[i].checked = false;
                }
            }
        }
    }
   
</script>  
 
 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div>--%>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
    <div id="contentarea">
        <div id="divdetail" >

        <div>
        <h1> <img alt="" src="Images/maitainstock.png" /></h1>
        <div class="divback" style="margin-top:15px;">
        <table width="100%" border="0">
            
            <tr>
                <td>
                    
                    Stock Holder Type*</td>
                <td>
                    
                    <asp:DropDownList ID="DropDownList1" runat="server">
                    </asp:DropDownList>
                    
                </td>
                <td>
                    
                    Stock Holder Code*</td>
                <td>
                    
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="txtCommon"></asp:TextBox>
                    
                </td>
                <td>
                    
                    Stock Holder Name*</td>
                <td>
                    
                    <asp:TextBox ID="TextBox2" runat="server" CssClass="txtCommon"></asp:TextBox>
                    
                </td>
            </tr>
            <tr>
                <td>
                    
                    Contact Privillege*</td>
                <td>
                    
                    <asp:TextBox ID="TextBox3" runat="server" CssClass="txtCommon"></asp:TextBox>
                    
                </td>
                <td>
                    
                    Contact</td>
                <td>
                    
                    <asp:TextBox ID="TextBox4" runat="server" CssClass="txtCommon"></asp:TextBox>
                    
                </td>
                <td>
                    
                </td>
                <td>
                    
                    
                </td>
            </tr>
        </table>
        </div>
        </div>
        
       </div>
         <div style="margin-top:15px;">
        <asp:Button ID="Button1" runat="server" Text="Show Detail" CssClass="button" />
        </div>             
       
        <div  style="margin-top:15px;" >
        <asp:GridView ID="grdStock" runat="server" AutoGenerateColumns="False" 
         ShowFooter="True"  onrowcommand="grdStock_RowCommand" BorderStyle="None"
         CellPadding="4" CellSpacing="4" CssClass="grdrowfont" >
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <input type="checkbox" name = "checkall"  onclick="javascript:SelectAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                            <asp:CheckBox ID="check" runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="BtnAddRow" runat="server" CommandName= "AddRow" 
                                                 Height="15px" ImageUrl="~/Images/morerow.png" ToolTip="Click Here To Add New Row" 
                                                 Width="15px" ValidationGroup="addrow" />
                                       
                                            </FooterTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DocType|SubType" HeaderStyle-Wrap="false">
                                            
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DropDownList2" runat="server">
                                                    <asp:ListItem Selected="True" Value="AWB">AWB</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="DropDownList3" runat="server">
                                                    <asp:ListItem Selected="True" Value="Normal">Normal</asp:ListItem>                                                    
                                                </asp:DropDownList>                                                
                                            </ItemTemplate>
                                            
                                        <HeaderStyle Wrap="False"></HeaderStyle>

                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approver " HeaderStyle-Wrap="false">

                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server" Text="West" Width="85px"></asp:TextBox>                                                
                                            </ItemTemplate>
                                            
                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reorder Level" HeaderStyle-Wrap="false">
                                            
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBox3" runat="server" Text="" Width="95px"></asp:TextBox>                                                
                                            </ItemTemplate>

                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reorder Qty" HeaderStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBox4" runat="server" Text="10" Width="95px"></asp:TextBox>                                                
                                            </ItemTemplate>

                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reorder Alert" HeaderStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                            </ItemTemplate>

                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Auto Stock Request" HeaderStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox2" runat="server" />
                                            </ItemTemplate>

                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Autoprocess Quantity" HeaderStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBox6" runat="server" Text="" Width="135px"></asp:TextBox>
                                            </ItemTemplate>

                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBox7" runat="server" Text="" Width="176px"></asp:TextBox>
                                            </ItemTemplate>

                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="titlecolr"/>
                                    <RowStyle  HorizontalAlign="Center"/>
                                    <AlternatingRowStyle  HorizontalAlign="Center" />
                                    
                            </asp:GridView>
        </div>
        
        <div id="fotbut">

                    <%--<input name="Save" type="button" value="Save" />--%>
  <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" />
  <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" /> 
  <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button" /> 
  
  </div>

    </div>
</asp:Content>


