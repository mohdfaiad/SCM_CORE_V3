<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListEmbargoDetails.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master"   Inherits="ProjectSmartCargoManager.ListEmbargoDetails" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
    
    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">                
     <h1> 
     List Embargo
     </h1>
    <div class="msg">
            <asp:Label ID="LbLStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
             ForeColor="Red"></asp:Label></div>
    
     <div class="botline">
     
     <table width="100%">
     <tr>
      <td>
          Origin Type</td>
      <td>
          <asp:DropDownList ID="ddlorigintype" runat="server" AutoPostBack="true"  
                   onselectedindexchanged="ddlorigintype_SelectedIndexChanged">
                   <asp:ListItem Text="Select"></asp:ListItem>  
                <asp:ListItem Text="Airport"  Value="A"></asp:ListItem>
                                    <asp:ListItem Text="City" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="N"></asp:ListItem>
               </asp:DropDownList></td>
      <td>
          Origin</td>
      <td>
          <asp:DropDownList ID="ddlOrigin" runat="server">
               </asp:DropDownList></td>
      <td>
          Destination Type</td>
      <td>
         <asp:DropDownList ID="ddldestinationType" runat="server" AutoPostBack="true" 
               onselectedindexchanged="ddldestinationType_SelectedIndexChanged" >
               <asp:ListItem Text="Select" Selected="True"></asp:ListItem>    
                <asp:ListItem Text="Airport" Value="A"></asp:ListItem>
                <asp:ListItem Text="City" Value="C"></asp:ListItem>
                <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                <asp:ListItem Text="Country" Value="N"></asp:ListItem>
           </asp:DropDownList></td>
      <td>
          Destination</td>
      <td>
          <asp:DropDownList ID="ddlDestination" runat="server">
           </asp:DropDownList></td>
     </tr>
     <tr>
      <td>
        Embargo Ref No
      </td>
      <td>
          <asp:TextBox ID="txtEmbargoRefNo" runat="server"></asp:TextBox>
      </td>
      <td>
       Level
      </td>
      <td>
          <asp:DropDownList ID="ddlLevel" runat="server">
          <asp:ListItem Text="Select"></asp:ListItem> 
          <asp:ListItem  Text="Warning"></asp:ListItem>   
          <asp:ListItem Text="Error"></asp:ListItem> 
          </asp:DropDownList>
      </td>
      <td>
       Parameter Code
      </td>
      <td>
        <asp:DropDownList ID="ddlParameter" runat="server" 
                >
                          <asp:ListItem Text="Select" Selected="True" ></asp:ListItem>   
                          <asp:ListItem Text="Payment Type"></asp:ListItem> 
                           <asp:ListItem Text="Origin"></asp:ListItem> 
                           <asp:ListItem Text="Destination"></asp:ListItem>
                           <asp:ListItem Text="Commodity"></asp:ListItem>  
        </asp:DropDownList>
      </td>
      <td>
       Parameter Value
      </td>
      <td>
          <asp:TextBox ID="txtParametervalue" runat="server"></asp:TextBox>
      </td>
     </tr>
     <tr>
     <td>
      From Date
     </td>
     <td>
         <asp:TextBox ID="txtFromDate" runat="server" Width="80px"></asp:TextBox>
         <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Format="MM/dd/yyyy"
            TargetControlID="txtFromDate" PopupButtonID="btnFromDate">
        </asp:CalendarExtender>
         <asp:ImageButton ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
     </td>
     <td>
     To Date
     </td>
     <td>
         <asp:TextBox ID="txtToDate" runat="server" Width="80px"></asp:TextBox>
         <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Format="MM/dd/yyyy"
            TargetControlID="txtToDate" PopupButtonID="btnToDate">
        </asp:CalendarExtender>
         <asp:ImageButton ID="btnToDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
     </td>
     <td>
      Status
     </td>
     <td>
      <asp:DropDownList ID="ddlStatus" runat="server">
                                    <asp:ListItem Value="All">All</asp:ListItem>
                                    <asp:ListItem Value="ACT">Active</asp:ListItem>
                                    <asp:ListItem Value="INA">Inactive</asp:ListItem>
                                    <asp:ListItem Value="DRF">Draft</asp:ListItem>
       </asp:DropDownList>
     </td>
         <td>
         </td>
         <td>
         </td>
     </tr>
     <tr>
     <td colspan="8">
       <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
             onclick="btnList_Click"  />
        &nbsp;
         <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
             onclick="btnClear_Click"  />
        </td>
        
    
     </tr>
     </table> 
     
     </div>
      <div class="ltfloat" style="overflow:auto; width:100%;">
            <asp:GridView ID="GrdEmbargoDetails" runat="server"  CellPadding="3" CellSpacing="3" AutoGenerateColumns="false" OnRowCommand="GrdEmbargoDetails_RowCommand" 
            OnPageIndexChanging="GrdEmbargoDetails_PageIndexChanging"> 
                    <Columns>
                   <%-- <asp:TemplateField >
                     <HeaderTemplate>
                     <input type="checkbox" name = "checkall" onclick="javascript:SelectAllgrdAddRate(this);" />
                     </HeaderTemplate>
                     <ItemTemplate>
                     <asp:CheckBox ID="check" runat="server" />
                     </ItemTemplate>
                     </asp:TemplateField>--%>
                      <asp:ButtonField CommandName="Edit" Text="Edit">
                                        <ItemStyle  />
                                    </asp:ButtonField>
                     
                     <asp:TemplateField HeaderText="Ref Number" ItemStyle-Wrap="false">
                      <ItemTemplate>
                          <asp:Label ID="lblEmbargoRefNo" runat="server" Text='<%# Eval("ReferenceNumber")%>' ></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>
                      
                       <asp:TemplateField HeaderText="Origin Type" >
                      <ItemTemplate>
                          <asp:Label ID="lbloriginType" runat="server" Text='<%# Eval("OriginType")%>'  ></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>
                      
                      <asp:TemplateField HeaderText="Origin"  >
                      <ItemTemplate>
                          <asp:Label ID="lblorigin" runat="server" Text='<%# Eval("Origin")%>'  ></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>
                      
                      <asp:TemplateField HeaderText="Destination Type" >
                      <ItemTemplate>
                          <asp:Label ID="lblDestinationType" runat="server"  Text='<%# Eval("DestinationType")%>'></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>
                      
                      
                       <asp:TemplateField HeaderText="Destination" >
                      <ItemTemplate>
                          <asp:Label ID="lblDestination" runat="server" Text='<%# Eval("Destination")%>'></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>
                      
                       <asp:TemplateField HeaderText="Level" >
                      <ItemTemplate>
                          <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("RefLevel")%>' ></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="Embargo Desc" >
                      <ItemTemplate>
                          <asp:Label ID="lblEmbargoDesc" runat="server" Text='<%# Eval("Discription")%>'></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>
                      
                      
                      <asp:TemplateField HeaderText="Start Date" >
                      <ItemTemplate>
                          <asp:Label ID="lblstartdate" runat="server" Text='<%# Eval("startDate")%>' ></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>
                      
                      <asp:TemplateField HeaderText="End Date"  HeaderStyle-Wrap="false">
                      <ItemTemplate>
                          <asp:Label ID="lblenddate" runat="server" Text='<%# Eval("EndDate")%>'></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>
                      
                       <asp:TemplateField HeaderText="Remarks" >
                      <ItemTemplate>
                          <asp:Label ID="lblRemarks" runat="server"  Text='<%# Eval("Remarks")%>'></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>
                      
                        <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                      <ItemTemplate>
                          <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
                      </ItemTemplate>
                      </asp:TemplateField>
                      
                     
                     </Columns> 
                      <HeaderStyle CssClass="titlecolr"/>
                        <RowStyle  HorizontalAlign="Center"/>
                        <AlternatingRowStyle  HorizontalAlign="Center"/>
           </asp:GridView>
   </div>  
     
  </div>     
  </asp:Content>
