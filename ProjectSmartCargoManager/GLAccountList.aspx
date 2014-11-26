<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="GLAccountList.aspx.cs" Inherits="ProjectSmartCargoManager.GLAccountList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
    function RadioCheck(rb) {
        var gv = document.getElementById("<%=GridView1.ClientID%>");
        var rbs = gv.getElementsByTagName("input");

        var row = rb.parentNode.parentNode;
        for (var i = 0; i < rbs.length; i++) {
            if (rbs[i].type == "radio") {
                if (rbs[i].checked && rbs[i] != rb) {
                    rbs[i].checked = false;
                    break;
                }

            }
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <br />
    <br />  
    <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>         
  <h1>GL Account Code&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
      </h1>
  <div class:"botline">
  <table>
  <tr>
  <td>
  GLAccount Code
  </td>
  <td>
  <asp:TextBox ID="txtGlAccount" runat="server" Width="200"></asp:TextBox>
  </td>
  <%--<td>
  GL Description
  </td>
  <td>
  <asp:TextBox ID="txtGlDesc" runat="server" Width="200"></asp:TextBox>
   </td>--%>
 <%--  <td>IsActive</td>--%>
   <td>GL AccountName </td>
   <td> <asp:TextBox ID="txtAccName" runat="server" Width="200"></asp:TextBox>
  </td>
  <td>
  IsActive
  </td>
  <td><asp:CheckBox ID="ChkIsActive" runat="server" /> </td>
   </tr>
  
 
   <tr>
  <td colspan="4"> 
     <asp:Button ID="btnList" runat="server" CssClass="button" 
                onclick="btnList_Click" Text="List" />
            <asp:Button ID="btnClear" runat="server" CssClass="button" 
                onclick="btnClear_Click" Text="Clear" /></td>
  </tr>
  
  </table>
   <hr />
  </div><div>
  <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"    
          HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="trcolor" PagerStyle-CssClass="PagerStyle"
            OnPageIndexChanging="GridView1_PageIndexChanging"        >
    <Columns>
    <asp:TemplateField  ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:RadioButton ID="rdbGLAUpdate" runat="server" onclick="javascript:RadioCheck(this)" GroupName="gr1"/>
    </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
    </asp:TemplateField >
    
    <asp:TemplateField HeaderText="GL Account Code" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtAcctCode" runat="server" Text='<%# Eval("GLAccountCode") %>'></asp:TextBox></ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
    </asp:TemplateField >
        <asp:TemplateField HeaderText="GL Account Name" ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
        <asp:TextBox ID ="txtAccName" runat="server" Text='<%# Eval("GLAccountName") %>'></asp:TextBox></ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
    <asp:TemplateField  HeaderText="GLA Description" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtGLADesc" runat="server" Text='<%# Eval("GLAccountDescription") %>' Enabled="false"></asp:TextBox><%--<asp:DropDownList ID="ddlStation" runat="server"></asp:DropDownList>--%></ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle>
    </asp:TemplateField >
    
        <asp:TemplateField HeaderText="IsActive"  ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
        <asp:CheckBox ID ="ChkIsActive" runat="server" Checked='<%#((bool)Eval("IsActive"))%>' Visible="true" />
        </ItemTemplate>
        
<ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
    <%--<asp:TemplateField  ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" OnClick="btnSave_Click"/>
    </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
    </asp:TemplateField >
    --%>
    
    </Columns>
         <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                    <FooterStyle CssClass="grdrowfont"></FooterStyle>

<PagerStyle CssClass="PagerStyle"></PagerStyle>

                    <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                    <RowStyle CssClass="grdrowfont"></RowStyle>

<AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
    </asp:GridView>
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add" onclick="btnAdd_Click" Visible="True"/>
                </td>
                <td>
                    <asp:Button ID="btnEdit" runat="server" CssClass="button" Text="Edit" onclick="btnEdit_Click" Visible="True"/>
                </td>
                <td>
                <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" 
                        Visible="true" onclick="btnSave_Click1" />
                    <asp:Button ID="btnDelete" runat="server" CssClass="button" Text="Delete" Visible="True"
        onclick="btnDelete_Click" OnClientClick='return confirm("Are you sure you want to delete this stock?");' />
                </td>
            </tr>
        </table>
</div>
  </asp:Content>
