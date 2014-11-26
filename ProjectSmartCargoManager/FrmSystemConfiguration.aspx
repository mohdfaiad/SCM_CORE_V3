<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmSystemConfiguration.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.FrmSystemConfiguration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript">

    function SelectheaderCheckboxes(headerchk) {
        var gvcheck = document.getElementById("<%=grdResult.ClientID %>");
        var i;
        //Condition to check header checkbox selected or not if that is true checked all checkboxes
        if (headerchk.checked) {
            for (i = 0; i < gvcheck.rows.length; i++) {
                var inputs = gvcheck.rows[i].getElementsByTagName('input');
                inputs[0].checked = true;
            }
        }
        else {
            for (i = 0; i < gvcheck.rows.length; i++) {
                var inputs = gvcheck.rows[i].getElementsByTagName('input');
                inputs[0].checked = false;
            }
        } 
    }
        
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div id="contentarea">
    
    <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
    
    <h1>System Configuration</h1>
   
    <div class="botline">
    <table>
    <tr>
    <td>
     <asp:Label ID="lblparameter" runat="server" Text="Parameter"></asp:Label>   
      </td>
    <td> <asp:DropDownList ID="ddlparam" runat="server"></asp:DropDownList>
    </td>
    <td> <asp:Label ID="lblkey" runat="server" Text="AppKey"></asp:Label> 
    </td>
    <td><asp:TextBox ID="txtsearchkey" runat="server"></asp:TextBox>
    </td>
    </tr>
    <tr><td><asp:Button ID="btnlist" runat="server" Text="List" CssClass="button" 
        onclick="btnList_Click"/></td>
    </tr>
    </table></div>
    <div class="ltfloat" style="width:100%">
 <asp:GridView ID="grdResult" runat="server" AutoGenerateColumns="False" Width="100%"
            AutoGenerateEditButton="false" style="margin-top: 0px" HeaderStyle-CssClass="HeaderStyle" 
            RowStyle-CssClass="RowStyle"  AlternatingRowStyle-CssClass="AltRowStyle" 
            PagerStyle-CssClass="PagerStyle" AllowPaging="True" PageSize="10" 
            onpageindexchanging="grdResult_PageIndexChanging">
            
<RowStyle CssClass="RowStyle" HorizontalAlign ="Center" ></RowStyle>
                                 <Columns>
                                 <asp:TemplateField>
                                 <ItemTemplate>
                                 <asp:RadioButton ID="rbSelect" AutoPostBack="true" Checked="false" runat="server" OnCheckedChanged="rbSelect_CheckedChanged" />
                                 </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <%--<asp:TemplateField HeaderText="SerialNo" >    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblPara" runat="server" Text = '<%# Eval("SrNo") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>--%>
                                 
                                 <asp:TemplateField HeaderText="Parameter" >    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblParameter" runat="server" Text = '<%# Eval("Parameter") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Value">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblValue" runat="server" Text = '<%# Eval("Value") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="App_Key">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblApp_Key" runat="server" Text = '<%# Eval("App_Key") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                                                                       
                                   
                                 <asp:TemplateField HeaderText="Description">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblDescription" runat="server" Text = '<%# Eval("Description") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 </Columns>

<PagerStyle CssClass="PagerStyle"></PagerStyle>

<HeaderStyle CssClass="HeaderStyle"></HeaderStyle>

<AlternatingRowStyle CssClass="AltRowStyle"></AlternatingRowStyle>
</asp:GridView></div>
<asp:Panel runat="server" ID="pnlAddSystemConfiguration" GroupingText = "" 
            class="ltfloat" style="width:100%; margin-top: 13px;">
<div>
<table width="100%">
<tr>
<td> 
    <asp:Label ID="Label1" runat="server" Text="Parameter"></asp:Label>
</td>
<td> 
    <asp:TextBox ID="txtPara" runat="server" MaxLength = "30"></asp:TextBox></td>
<td> 
    <asp:Label ID="Label2" runat="server" Text="Value"></asp:Label>
</td>
<td>
    <asp:TextBox ID="txtValue" runat="server" MaxLength = "100"></asp:TextBox>
</td>
<td>
    <asp:Label ID="Label3" runat="server" Text="App Key" MaxLength = "30"></asp:Label>
</td>
<td>
    <asp:TextBox ID="txtAppkey" runat="server"></asp:TextBox>
</td>

<td>
    &nbsp;</td>
<td>
    &nbsp;</td>

</tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" MaxLength="100" Text="Description"></asp:Label>
        </td>
        <td colspan="3">
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" 
                Width="331px"></asp:TextBox>
        </td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
<tr>
<td>
</td>
<td>
    <asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="button" 
        onclick="btnAdd_Click"/>
    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button" 
        onclick="btnDelete_Click"/>
    <asp:Button ID="BtnSave" runat="server" Visible="false" Text="Save" CssClass="button" onclick="BtnSave_Click"/>
</td>
</tr>


</table>
</div>
</asp:Panel>

</div>
</asp:Content>
