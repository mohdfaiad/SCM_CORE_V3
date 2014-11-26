<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="MasterAuditLog.aspx.cs" Inherits="ProjectSmartCargoManager.MasterAuditLog" %>
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
    
    
    
    <div id="contentarea" ><div class="msg">
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
    </div>
   <h1> 
            Master Audit Log
         </h1>
         
                
           
 
<asp:Panel ID="pnlNew" runat="server">
<div class="botline">
<table width="100%" border="0">
<tr>
<td>
    Master
</td>
<td>
   <asp:DropDownList ID="ddlMaster" runat="server"></asp:DropDownList></td>

       <td>
             Master value</td>
            <td>
                <asp:TextBox ID="txtMasterValue" runat="server" Width="80px" style="text-align:left"></asp:TextBox> 
                
                </td>
            
            <td>
               From Date
            </td>
            <td>
                <asp:TextBox ID="txtValidFrm" runat="server" Width="80px"></asp:TextBox>
                <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtValidFrm" PopupButtonID="imgFromDate">
                      </asp:CalendarExtender>
                <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" 
                    ImageUrl="~/Images/calendar_2.png" />
               
            </td>
            <td>
             To Date 
            </td>
            <td>
                <asp:TextBox ID="txtValidTo" runat="server" Width="80px"></asp:TextBox>
                  <asp:CalendarExtender ID="txtToate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtValidTo" PopupButtonID="imgToDate">
                      </asp:CalendarExtender>
                   <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" 
                    ImageUrl="~/Images/calendar_2.png" />
                 
                   </td>
                <td>
       
        
    </td>
                
        </tr>
        <tr>
        <td>
          <asp:Button ID="btnList" runat="server" CssClass="button" 
             Text="List" CausesValidation="false" OnClick="btnList_Click"  />
        
        <asp:Button ID="btnClear" runat="server" CssClass="button" 
             Text="Clear" CausesValidation="false" onclick="btnClear_Click"/>
        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click" 
            />
        </td></tr>
        </table>
 <table style="float:right;">
        <tr>
         
    <td>
        <%--<asp:Button ID="btnImoprt" runat="server" Text="Import" CssClass="button" 
            onclick="btnImoprt_Click" />--%>
        
    </td>
        </tr>
   
    </table>
   </div>
</asp:Panel>
<asp:Panel ID="pnlGrid"  runat="server">
<div class="ltfloat" style="width:100%;">
    <asp:GridView ID="grdMasterAuditLog" runat="server" ShowFooter="false" Width="100%" 
 AutoGenerateColumns="False" CellPadding="2" 
 CellSpacing="3" PageSize="10" AllowPaging="True" 
 AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
        SelectedRowStyle-CssClass="SelectedRowStyle" 
        onpageindexchanging="grdMasterAuditLog_PageIndexChanging">
           
            <Columns>
            <asp:TemplateField HeaderText="Master" Visible="true">
             <ItemTemplate>
             <asp:Label ID="lblMasterKey" runat="server" Text = '<%# Eval("MasterKey") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Master Value" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblMasterValue" runat="server" Text = '<%# Eval("MasterValue") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
             
            <asp:TemplateField HeaderText="Action" Visible="true" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Label ID="lblAction" runat="server" Text = '<%# Eval("Action") %>'/>
            </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Message" Visible="true" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Label ID="lblMessage" runat="server" Text = '<%# Eval("Message") %>'/>
            </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Description" Visible="true" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Label ID="lblDescription" runat="server" Text = '<%# Eval("Description") %>'/>
            </ItemTemplate>
            </asp:TemplateField>
             
             
             
             <asp:TemplateField HeaderText="Updated By" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblUpdatedBy" runat="server" Text = '<%# Eval("UpdatedBy") %>'/>
             </ItemTemplate>
             </asp:TemplateField>   
             
             <asp:TemplateField HeaderText="Updated On" ItemStyle-HorizontalAlign="Center" Visible="true">
             <ItemTemplate>
             <asp:Label ID="lblUpdatedOn" runat="server" Text = '<%# Eval("UpdatedOn") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
            
            
            
            
            </Columns>
             <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    </div>
</asp:Panel>
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
</asp:Content>
