<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DesignatorMaster.aspx.cs" Inherits="ProjectSmartCargoManager.DesignatorMaster" MasterPageFile="~/SmartCargoMaster.Master" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 </asp:Content>
 
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
  <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
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
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
  <div id="contentarea">
   <h1> 
    Designator Master
         </h1>
         <br />
   <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large">
   </asp:Label>
 
<asp:Panel ID="pnlNew" runat="server"><table width="55%" border="0">
<tr>
<td>
AWB Prefix
</td>
    
<td>
<asp:TextBox ID="txtPrefix" runat="server"></asp:TextBox>
</td>
<td>
Designator Code
</td>

<td>
<asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
</td>
<td>
Active
</td>
<td>
<asp:CheckBox ID="chkAct" runat="server" />
</td>
</tr>
    <caption>
        <br />
        <tr>
         <td>
           <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" onclick="btnSave_Click"  />
                &nbsp;&nbsp; &nbsp;&nbsp;
          </td>
          <td>
                &nbsp;&nbsp;
                <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" onclick="btnClear_Click"  
                     />
                &nbsp;&nbsp;
                <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" onclick="btnList_Click" 
                     />
            </td>
            <td>
                
            </td>
        </tr>
    </caption>

</table>
</asp:Panel>  
<asp:Panel ID="pnlGrid"  runat="server">
    <asp:GridView ID="grdDesig" runat="server" ShowFooter="false" Width="80%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
        onrowcommand="grdDesig_RowCommand" 
        onpageindexchanging="grdDesig_PageIndexChanging" onrowediting="grdDesig_RowEditing">
        <Columns>
        <asp:TemplateField HeaderText="SerialNo" Visible="false">
        <ItemTemplate>
        <asp:Label runat="server" Text='<%#Eval("SerialNumber")%>' ID="lblSerialNo"></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="AWB Prefix" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblPrefix" runat="server" Text ='<%#Eval("AWBPrefix")%>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Designator Code" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblCode" runat="server" Text ='<%# Eval("DesignatorCode") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblIsAct" runat="server" Text ='<%# Eval("IsActive") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
            <asp:ButtonField CommandName="Edit" Text="Edit">
                                    <ItemStyle Width="50px" />
                                </asp:ButtonField>
            </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
</asp:Panel>
  </div>
    </ContentTemplate>
    </asp:UpdatePanel>
  
</asp:Content>
