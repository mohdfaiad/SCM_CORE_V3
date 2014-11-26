<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ProjectSmartCargoManager.WebForm1" MasterPageFile="~/SmartCargoMaster.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
 <br />
 <br />
 <br />
 <br />
 <br />
 <br />
 <br />
 <br />
 <br />
 <br />
 <br />
 <br />
 
     <asp:TextBox ID="txtSearchLink1" runat="server" width="140px" 
                            ToolTip="Search for Link" AutoPostBack="true" MaxLength="8" ></asp:TextBox>
                          <asp:TextBoxWatermarkExtender ID="txtSearchLink1_TextBoxWatermarkExtender" 
                            runat="server" TargetControlID="txtSearchLink1" WatermarkText="Search your page">
                         </asp:TextBoxWatermarkExtender> 
 
                       <%-- <asp:AutoCompleteExtender ID="txtSearchLink_AutoCompleteExtender"  CompletionInterval="1000" CompletionSetCount="10" ServicePath="~/Home.aspx"
                       runat="server" Enabled="True"   EnableCaching="true"   ServiceMethod="GetMenu"   MinimumPrefixLength="1" FirstRowSelected="true"
                         TargetControlID="txtSearchLink" >         
                        </asp:AutoCompleteExtender>
                        --%>
                        
                        
  <asp:AutoCompleteExtender ID="txtSearchLink1_AutoCompleteExtender"  CompletionInterval="1000" CompletionSetCount="10"    runat="server" DelimiterCharacters=";, :" 
   Enabled="True"   EnableCaching="true"           
  ServiceMethod="GetMenu"   MinimumPrefixLength="1" FirstRowSelected="true"      
  TargetControlID="txtSearchLink1"  UseContextKey="true" ContextKey="NA">                </asp:AutoCompleteExtender>
    
</asp:Content>