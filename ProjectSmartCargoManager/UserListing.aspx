<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserListing.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.UserListing" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
     
    </style>
    <script language="javascript" type="text/javascript">

    </script>
</asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="True">
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
     
 <div id="contentarea" >
 
<div class="msg">
               <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
                Font-Size="Large" meta:resourcekey="lblStatusResource1"></asp:Label>
</div>

    <h1>       
         User Listing
    </h1> 
        <p>
            
        </p>
    
        <div class="botline"> 
          <table width="80%">
            <tr>
                <td>
                User ID
                </td>
                <td>
                    <asp:TextBox ID="txtUserID" runat="server" Width="140px" 
                        meta:resourcekey="txtUserNameResource1"></asp:TextBox>
                </td>
            
                <td>
                Role
                </td>
                <td>
                    <asp:DropDownList ID="ddlRole" runat="server" Width="200px" 
                        meta:resourcekey="ddlRoleResource1">
                    </asp:DropDownList>
                </td>
            
                <td>
                Station
                </td>
                <td>
                    <%--<asp:TextBox ID="txtStationCode" runat="server" AutoPostBack="True" Width="80px"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="txtStationCode_AutoCompleteExtender" runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
                    TargetControlID="txtStationCode">
                    </asp:AutoCompleteExtender>--%>
                    
                    <asp:DropDownList ID="ddlStations" runat="server" Width="100px"></asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
                        onclick="btnList_Click" /> &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="BtnClear" runat="server" CssClass="button" Text="Clear" 
                        onclick="BtnClear_Click" />
                </td>
            </tr>
          </table>  
        </div>
      <div Class="ltfloat" style="Width:100">
            <asp:GridView ID="grdUserList" Width="80%" runat="server"
         AutoGenerateColumns="False" AllowPaging="True" PageSize="20" 
                onrowdatabound="grdUserList_RowDataBound" 
                onrowcommand="grdUserList_RowCommand" 
                onpageindexchanging="grdUserList_PageIndexChanging" >
            <AlternatingRowStyle CssClass="trcolor">
            </AlternatingRowStyle>
            <Columns>
                
                
                <asp:TemplateField HeaderText="User ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblLoginID" runat="server" Text='<%# Eval("LoginID") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="User Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Def Stn" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblDefStn" runat="server" Text="" ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Lang" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblLang" runat="server" Text="" ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                                
                <asp:TemplateField HeaderText="Email" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblEmailID" runat="server" Text='<%# Eval("EmailID") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Agent Code" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="SU" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSU" Enabled="false" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkActive" Enabled="false" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="GSA" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkGSA" Enabled="false" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="All Stn" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkAllStn" Enabled="false" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField CommandName="Edit" Text="Edit">
                    <ItemStyle Width="50px" />
                </asp:ButtonField>
                <asp:ButtonField CommandName="View" Text="View">
                    <ItemStyle Width="50px" />
                </asp:ButtonField>
             </Columns> 
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
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
