<%@ Page Title="DwellTime Config" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="DwellTimeConfig.aspx.cs" Inherits="ProjectSmartCargoManager.DwellTimeConfig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
		.black_overlaymsg{
			display: none;
			position: fixed;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 100%;
			background-color: White;
			z-index:1001;
			-moz-opacity: 0.8;
			opacity:.80;
			filter: alpha(opacity=80);
		}
		.white_contentmsg {
			display: none;
			position: fixed;
			top: 45%;
			left: 45%;
			width: 5%;
			height: 5%;
			padding: 16px;
			background-color: Transparent;
			z-index:1002;
			
		}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel  runat="server">
 <ContentTemplate>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <br />
    <br />
    <br />
    <h1>Dwell-Time Config</h1>
    <asp:Label ID="lblStatus" runat="server" Text="" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
    <div style="margin-top:15px;" class="botline">
     <table width="100%">
     <tr>
     <td style="width:35%;">Station&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:DropDownList ID="ddlstation" runat="server" ></asp:DropDownList></td>
     <td style="width:35%;">Comm Category&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:DropDownList ID="ddlCC" runat="server">
         
         </asp:DropDownList></td>
     <td style="width:22%;">Comm Code&nbsp;&nbsp; 
     <asp:TextBox ID="txtCommCode" runat="server" Width="70px" MaxLength="10"></asp:TextBox>
         </td>
      
     <td>
         &nbsp;</td>
     </tr>
     <tr>
     <td colspan="4">
     <asp:Button ID="btnList" runat="server"  CssClass="button" 
             onclick="btnList_Click" OnClientClick="callShow();" Text="List" />
     &nbsp;&nbsp;&nbsp;&nbsp; <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
             CausesValidation="false" onclick="btnClear_Click" />
         </td>
     </tr>
     </table>
     </div>
     <div class="ltfloat" style="width:100%;">
     
     <asp:GridView ID="grdCommCat" runat="server"
               AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
               CellPadding="2" CellSpacing="3" Width="100%" >
               <Columns>
                   <asp:TemplateField>
                       <ItemTemplate>
                           <asp:CheckBox ID="checkCC" runat="server" />
                       </ItemTemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Station *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:DropDownList ID="grdddlStation" runat="server" Enabled="true"></asp:DropDownList>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Comm Category "  
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:DropDownList ID="grdddlCommCat" runat ="server" Enabled="true"></asp:DropDownList>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Comm Code" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="grdCommCode" runat="server" Text='<%# Eval("CommCode") %>' 
                               Width="60px" Enabled="true"></asp:TextBox>
                               <%--<asp:AutoCompleteExtender ID="CommCode1" runat="server" 
                        BehaviorID="CommCode1" CompletionInterval="0" CompletionSetCount="10" 
                         MinimumPrefixLength="1" ServiceMethod="GetCommodityCodesWithName" 
                        TargetControlID="grdCommCode"
                        EnableCaching="true">
                    </asp:AutoCompleteExtender>--%>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Dwell-Time Days *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="grdDwellDays" runat="server" Text='<%# Eval("DwellTimeDays") %>' 
                               Width="70px" Enabled="true"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Hours" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="grdtxtHours" runat="server" Text='<%# Eval("HOURS") %>' Width="40px" Enabled="true"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Email ID(s)*" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="grdtxtEmail" runat="server" Text='<%# Eval("EmailRecipients") %>' Width="200px" Enabled="true"></asp:TextBox>
                           <asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
    TargetControlID="grdtxtEmail"
    WatermarkText="Email ID (s) separated by comma"
     />
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
               </Columns>
               <HeaderStyle CssClass="titlecolr" />
               <RowStyle HorizontalAlign="Center" />
               <AlternatingRowStyle HorizontalAlign="Center" />
           </asp:GridView>
           
           <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" 
             CausesValidation="false" onclick="btnAdd_Click" />
             &nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
             CausesValidation="false" onclick="btnSave_Click" />
             &nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Button ID="btnDel" runat="server" Text="Delete" CssClass="button" 
             CausesValidation="false" onclick="btnDel_Click" />
     </div>
     
      <div id="msglight" class="white_contentmsg" >
<table>

<tr>

<td width="5%" align="center">
    <br />
    <img src="Images/loading.gif" />
    <br />
    <asp:Label ID="msgshow" runat="server" ></asp:Label>
</td>
</tr>
</table>
		</div>
		
		<div id="msgfade" class="black_overlaymsg"></div>
     
    </ContentTemplate>
     </asp:UpdatePanel>
</asp:Content>
