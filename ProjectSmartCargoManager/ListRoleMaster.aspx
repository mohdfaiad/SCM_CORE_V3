<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListRoleMaster.aspx.cs"  MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.ListRoleMaster" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
    <script type="text/javascript">

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
    <div class="msg">
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red" Visible="false"></asp:Label>
    </div>
                      <h1> 
                      Roles List
                      </h1>
    
            
       
    <div class="botline">
               <table cellpadding="5px">
                 <tr>
                     <td>
                         Role </td>
                     <td>
                         <asp:DropDownList ID="ddlRole" runat="server">
                         </asp:DropDownList>
                     </td>
                     <td>
                         <asp:CheckBox ID="chkActive" runat="server" Text="IsActive" Checked="true" />
                     </td>
                     
                     <td>
                        <asp:Button ID="btnList" runat="server" CssClass="button" 
                             onclick="btnList_Click" Text="List" />
                         &nbsp;
                         <asp:Button ID="btnClear" runat="server" CssClass="button" 
                             onclick="btnClear_Click" Text="Clear" />
                     </td>
                 </tr>
                 </table>
                 <table>
                 <tr>
                     
                     <td>
                         
                     </td>
                     
                 </tr>
             </table>
         
           </div> 
  
   <div style="overflow:auto; float:left;"> 
         <asp:GridView ID="grdRoles" runat="server" AutoGenerateColumns="False" 
            ShowFooter="True"   Width="100%" AllowPaging="False" 
          >
            <Columns>
            
                <asp:TemplateField HeaderText="Role" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRoleName" runat="server" Text='<%# Eval("RoleName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Created By" HeaderStyle-Wrap="false">
               <ItemTemplate>
                        <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
               <asp:TemplateField HeaderText="Created On" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Eval("CreatedOn") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="IsActive" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblActive" runat="server" Text='<%# Eval("IsActive") %>'></asp:Label> 
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField>
                    <ItemTemplate>
                       <asp:HyperLink ID="hlnkEdit" runat="server" Text="Edit" NavigateUrl='<%# "RoleMaster.aspx?command=Edit&RoleID=" + Eval("RoleID")%>' Width="50px"></asp:HyperLink>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField>
                    <ItemTemplate>
                       <asp:HyperLink ID="hlnkView" runat="server" Text="View" NavigateUrl='<%# "RoleMaster.aspx?command=View&RoleID=" + Eval("RoleID")%>' Width="50px"></asp:HyperLink> 
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
               
           </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>
 
 </div> 
  
      
   </div>
   
    <div id="msglight" class="white_contentmsg">
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
    
    <div id="msgfade" class="black_overlaymsg">
    </div>
    
    </ContentTemplate>
    </asp:UpdatePanel>
    
   </asp:Content>
