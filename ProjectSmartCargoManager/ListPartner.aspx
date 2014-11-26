<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="ListPartner.aspx.cs" Inherits="ProjectSmartCargoManager.ListPartner" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ContentPlaceHolderID="head" ID="Content1" runat="server">
     
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content2" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
  <div id="contentarea">
  
  <div class="msg">
               <asp:Label ID="lblStatus" runat="server" Font-Bold="true" Font-Size="Large" BackColor="White"></asp:Label>
</div>

  <h1>List Partner</h1>
  
  <%--<table>
  <tr>
  <td>Partner Type:</td>
  <td>
 <asp:DropDownList ID="ddlPartnerType" runat="server"></asp:DropDownList>
  </td>
  <td></td>
  <td>Partner Name:</td>
  <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
  <td></td>
  <td>Partner Prefix:</td>
  <td><asp:TextBox ID="txPrefix" runat="server"></asp:TextBox></td>
  </tr>
  <tr>
  <td>
      <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
          onclick="btnList_Click" />&nbsp;&nbsp;
      <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
          onclick="btnClear_Click" />
  </td>
  </tr>
  </table>--%>
  <div class="botline"> 
  <table width="100%" border="0" >
                <tr>
                    <td>
                        Partner Type*
                    </td>
                    <td>
                        <asp:DropDownList ID="drpPartnerType" runat="server" Width = "80px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Partner Name*
                    </td>
                    <td>
                        <asp:TextBox ID="txtPartnerName" runat="server" Width="110px" 
                            ToolTip="SitaID" AutoPostBack="false"></asp:TextBox>
                    </td>
                    <td>
                        Partner Code* 
                    </td>
                    <td>
                         <asp:TextBox ID="txtPartnerCode" runat="server" Width="110px" 
                            ToolTip="SitaID" AutoPostBack="false"></asp:TextBox>
                    </td>
                   <%-- <td>
                        
                        <asp:CheckBox ID="chkbIsScheduled" runat="server" Text="IsScheduled" />
                        
                    </td>--%>
                    
                </tr>
                <tr>
                    <td>
                       Email Id 
                    </td>
                    <td>
                         <asp:TextBox ID="txtEmailID" runat="server" Width="110px" 
                            ToolTip="EmailID" AutoPostBack="false"></asp:TextBox>
                    </td>
                    <td>
                        Sita ID
                    </td>
                    <td>
                        <asp:TextBox ID="txtSitaID" runat="server" Width="110px" 
                            ToolTip="SitaID" AutoPostBack="false"></asp:TextBox>
                    </td>
                
                    <td>
                        Valid From
                    </td>
                    <td>
                        <asp:TextBox ID="txtValidFrom" runat="server" Width="110px" 
                            ToolTip="Valid From" AutoPostBack="false"></asp:TextBox>
                        <asp:CalendarExtender ID="txtValidFrom_CalendarExtender" runat="server" 
                            Enabled="True" TargetControlID="txtValidFrom">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        Valid Till
                    </td>
                    <td>
                        <asp:TextBox ID="txtValidTill" runat="server" Width="110px" 
                            ToolTip="Valid Till" AutoPostBack="false"></asp:TextBox>
                        <asp:CalendarExtender ID="txtValidTill_CalendarExtender" runat="server" 
                            Enabled="True" TargetControlID="txtValidTill">
                        </asp:CalendarExtender>
                    </td>
                    </tr>
                <tr>
                    <td align ="left" colspan="8">
                    <asp:Button ID="btnList" runat="server" Text="List" CssClass="button"
                            Visible="true" onclick="btnList_Click" />&nbsp;&nbsp;
                            
                    <asp:Button ID="btnExport" runat="server" Text="Export" 
                            CssClass="button" onclick="btnExport_Click"/>&nbsp;&nbsp;
                    
                    
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button"
                             Visible="true" onclick="btnClear_Click1"/>
                    </td>
                </tr>
                </table>
   </div>
   <div class="ltfloat">
         <asp:GridView ID="GrdPartnerList" runat="server" AutoGenerateColumns="false" PageSize="10" 
      Width="98%" OnRowCommand="GrdPartnerList_RowCommand" 
      OnPageIndexChanging="GrdPartnerList_PageIndexChanging" OnRowEditing="GrdPartnerList_RowEditing"
      AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
      PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
      SelectedRowStyle-CssClass="SelectedRowStyle">
<RowStyle CssClass="RowStyle"></RowStyle>
      <Columns>
      
      <asp:TemplateField HeaderText="Sr No" Visible="false">
      <ItemTemplate>
      <asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("SrNo")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
      <asp:TemplateField HeaderText="Partner Prefix">
      <ItemTemplate>
      <asp:Label ID="lblPrefix" runat="server" Text='<%#Eval("PartnerPrefix")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
      <asp:TemplateField HeaderText="Partner Code">
      <ItemTemplate>
      <asp:Label ID="lblCode" runat="server" Text='<%#Eval("PartnerCode")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
      <asp:TemplateField HeaderText="Partner Type">
      <ItemTemplate>
      <asp:Label ID="lblType" runat="server" Text='<%#Eval("PartnerType")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
      <asp:TemplateField HeaderText="SITA ID">
      <ItemTemplate>
      <asp:Label ID="lblSitaId" runat="server" Text='<%#Eval("SITAiD")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
      <asp:TemplateField HeaderText="Email Id">
      <ItemTemplate>
      <asp:Label ID="lblemail" runat="server" Text='<%#Eval("EmailiD")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
      <asp:TemplateField HeaderText="Zone Id">
      <ItemTemplate>
      <asp:Label ID="lblzoneId" runat="server" Text='<%#Eval("ZoneId")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
      <asp:TemplateField HeaderText="Billing Currency">
      <ItemTemplate>
      <asp:Label ID="lblBillCurr" runat="server" Text='<%#Eval("BillingCurrency")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
      <asp:TemplateField HeaderText="Listing Currency">
      <ItemTemplate>
      <asp:Label ID="lblListCurr" runat="server" Text='<%#Eval("ListingCurrency")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
       <asp:TemplateField HeaderText="Settlement Method">
      <ItemTemplate>
      <asp:Label ID="lblSetMethod" runat="server" Text='<%#Eval("SettlementMethod")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
       <asp:TemplateField HeaderText="Reg ID">
      <ItemTemplate>
      <asp:Label ID="lblRegId" runat="server" Text='<%#Eval("RegistrationID")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
      <asp:TemplateField HeaderText="Tax Reg ID">
      <ItemTemplate>
     <asp:Label ID="lbltaxRegId" runat="server" Text='<%#Eval("TaxRegistrationID")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
      
      <asp:TemplateField HeaderText="Add Tax Reg ID">
      <ItemTemplate>
     <asp:Label ID="lblAddtaxRegId" runat="server" Text='<%#Eval("AdditionalTaxRegID")%>'></asp:Label>
      </ItemTemplate>
      </asp:TemplateField>
       
      <asp:ButtonField CommandName="Edit" Text="Edit"/>
      <asp:ButtonField CommandName="View" Text="View" />
      </Columns>

<PagerStyle CssClass="PagerStyle"></PagerStyle>

<SelectedRowStyle CssClass="SelectedRowStyle"></SelectedRowStyle>

<HeaderStyle CssClass="HeaderStyle"></HeaderStyle>

<AlternatingRowStyle CssClass="AltRowStyle"></AlternatingRowStyle>
      </asp:GridView>
      </div>
  </div>
  </asp:Content>
