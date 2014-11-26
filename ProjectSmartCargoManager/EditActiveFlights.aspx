<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="EditActiveFlights.aspx.cs" Inherits="ProjectSmartCargoManager.EditActiveFlights" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
 
 
    <style type="text/css">
        .style1
        {
        }
    </style>
 
 
 
</asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
     <div id="contentarea">

    <asp:Panel ID="grid" runat="server" ScrollBars="Horizontal" >            
           
      <asp:Panel ID="pnlDestDetails" runat="server" Visible="False" >
     <div class="botline" visible="false">
        <table width="80%">
          <tr>
          <td >
        
              Origin</td>
              <td>
                  <asp:DropDownList ID="ddlOrigin1" runat="server" AutoPostBack="True" 
                      onselectedindexchanged="ddlOrigin_SelectedIndexChanged">
                  </asp:DropDownList>
                  <asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox>
              </td>
              <td>
                  Destination</td>
                  <td>
                      <asp:DropDownList ID="ddlDestination0" runat="server" 
                          onselectedindexchanged="ddlDestination2_SelectedIndexChanged">
                      </asp:DropDownList>
                  </td>
<td>
    &nbsp;</td>
<td>&nbsp;</td>
          </tr>
         <tr>
          <td >
              AirCraftType  *</td>
          <td>
                       <asp:DropDownList ID="ddlLoadAirCraftType" runat="server" AutoPostBack="True" 
                           onselectedindexchanged="ddlLoadAirCraftType_SelectedIndexChanged">
              </asp:DropDownList></td>
          <td>
              Free Sale (kg)&nbsp; *</td>
          <td>
              <asp:TextBox ID="txtCargoCapacity" runat="server" Height="19px" MaxLength="7" width="114px" 
                 ></asp:TextBox>
          </td>
             <td>
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
          </tr>
                     
         <tr>
          <td>
              &nbsp;</td>
          <td>
              <asp:TextBox ID="txtFromdate" runat="server" Height="19px" Width="114px" 
                  ontextchanged="txtFromdate_TextChanged" 
                  ToolTip="Please enter date format: dd/MM/yyyy" Visible="False"></asp:TextBox>
              <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFromdate">
              </asp:CalendarExtender>
             </td>
          <td>
              &nbsp;</td>
          <td>
              <asp:TextBox ID="txtToDate" runat="server" Width="114px" AutoPostBack="True" 
                  ToolTip="Please enter date format: dd/MM/yyyy" Visible="False" 
                 ></asp:TextBox>
              <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtToDate">
              </asp:CalendarExtender>
             </td>
             <td>
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
          </tr>
                     
            <tr>
                <td>
                    <asp:RadioButton ID="chkDomestic0" runat="server" AutoPostBack="True" 
                        Checked="True" GroupName="B" oncheckedchanged="RadioButton1_CheckedChanged" 
                        Text="Domestic" ValidationGroup="B" Visible="False" />
                </td>
                <td colspan="2">
                    <asp:RadioButton ID="chkInternational0" runat="server" AutoPostBack="True" 
                        GroupName="B" oncheckedchanged="chkInternational0_CheckedChanged" 
                        Text="International" ValidationGroup="B" Visible="False" />
                </td>
                <td>
                    <asp:TextBox ID="txtSource1" runat="server" Enabled="False" height="19px" 
                        ontextchanged="txtSource1_TextChanged" width="114px" Visible="False"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtDestination" runat="server" Enabled="False" height="19px" 
                        width="114px" Visible="False"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
                     
        </table>
        
        
    </div>
    </asp:Panel> 
     
    <asp:Panel ID="pnlSchedule" runat ="server" Visible="False" >
   <h2>Route Details
       
            </h2> 
          
          <div>
    <table width="100%" visible="false">
    <tr>
    <td width="40%">
    
    </td>
    <td width="40%">
    
    </td>
    <td width="10%">
      <asp:Button ID="btnAddNewRow" runat="server" Text="Add New" CssClass="button"  Visible="false"
            onclick="btnAddNewRow_Click" />
    </td>
    <td width="10%">
      <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button" 
            onclick="btnDelete_Click" Visible="False"  />
    </td>
    </tr>
   
    </table>
    </div>
          
         
           <div>
     <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal" >            
           
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <asp:GridView ID="grdScheduleinfo" runat="server" AutoGenerateColumns="False" 
                                    ShowFooter="True"   Width="80%" 
                   onpageindexchanging="grdScheduleinfo_PageIndexChanging" 
                   onselectedindexchanging="grdScheduleinfo_SelectedIndexChanging" 
                   PageSize="40" AllowPaging="True" >
            <Columns>
            
              <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CHK" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
            
               
                <asp:TemplateField HeaderText="Flight# *" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label ID="lblFlight" runat="server">
                        </asp:Label>
                         <%--<asp:TextBox  ID="txtSource" runat="server" Width="70px" ></asp:TextBox>--%>
                        <%--<asp:TextBox ID="txtbankname" runat="server"></asp:TextBox>--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
                <asp:TemplateField HeaderText="From *" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlFromOrigin" runat="server" Width="45px">
                        </asp:DropDownList>
                         <%--<asp:TextBox  ID="txtSource" runat="server" Width="70px" ></asp:TextBox>--%>
                        <%--<asp:TextBox ID="txtbankname" runat="server"></asp:TextBox>--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="To *" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                         <asp:DropDownList ID="ddlToDest" runat="server" Width="45px">
                        </asp:DropDownList>
                        
                         <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                    
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
   
                <asp:TemplateField HeaderText="Dept Time  *" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=20%>
                    <ItemTemplate>
                    
                    Day <asp:TextBox ID="txtDeptDay" runat="server" Width="8px"  DataTextField=""></asp:TextBox>
                         <asp:NumericUpDownExtender ID="NumericUpDownExtender_DeptDay" runat="server" 
                TargetControlID="txtDeptDay"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "1"  Maximum = "2" />
                    
                       Hr <asp:TextBox ID="txtDeptTimeHr" runat="server" Width="8px"  DataTextField=""></asp:TextBox>
                         <asp:NumericUpDownExtender ID="txtDeptTimeHr_NumericUpDownExtender" runat="server" 
                TargetControlID="txtDeptTimeHr"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "23" /> :
                       
                         Min <asp:TextBox ID="txtDeptTimeMin" runat="server" Width="8px"  DataTextField=""></asp:TextBox>
                             <asp:NumericUpDownExtender ID="txtDeptTimeMin_NumericUpDownExtender1" runat="server" 
                TargetControlID="txtDeptTimeMin"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "1"  Maximum = "60" /> 
      
      
                    </ItemTemplate>
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Arrival Time  *" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=20%>
                    <ItemTemplate >  
                    
                      Day <asp:TextBox ID="txtArrivalDay" runat="server" Width="8px"  DataTextField=""></asp:TextBox>
                         <asp:NumericUpDownExtender ID="NumericUpDownExtender_ArrivalDay" runat="server" 
                TargetControlID="txtArrivalDay"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "1"  Maximum = "2" />
                    
                    
                        Hr<asp:TextBox  ID="txtArrivaltimeHr" runat="server" Width="8px" ></asp:TextBox>
                              <asp:NumericUpDownExtender ID="txtArrivaltimeHr_NumericUpDownExtender" runat="server" 
                TargetControlID="txtArrivaltimeHr"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "23" /> :  
     
                       Min<asp:TextBox  ID="txtArrivalTimeMin" runat="server" Width="8px" ></asp:TextBox>
                      <asp:NumericUpDownExtender ID="txtArrivalTimeMin_NumericUpDownExtender1" runat="server" 
                TargetControlID="txtArrivalTimeMin"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "1"  Maximum = "60" /> 
      
      
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbankguranteeamt"  ErrorMessage="Please Enter Numeric Value"  ValidationExpression="^\d+$" ValidationGroup="check">
                        </asp:RegularExpressionValidator>--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
   <%--             <asp:TemplateField HeaderText="Frequency  *" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=40%>
                   
                    <ItemTemplate>
                        <asp:CheckBox ID="chkMon" runat="server" Checked="True" Text="Mo" />
                        <asp:CheckBox ID="chkTues" runat="server" Checked="True" Text="Tu" />
                        <asp:CheckBox ID="chkwed" runat="server" Checked="True" Text="We" />
                        <asp:CheckBox ID="chkThur" runat="server" Checked="True" 
                            Text="Th" />
                        <asp:CheckBox ID="chkFri" runat="server" Checked="True" Text="Fr" />
                        <asp:CheckBox ID="chkSat" runat="server" Checked="True" 
                            Text="Sa" />
                        <asp:CheckBox ID="chkSun" runat="server" Checked="True" Text="Su" />
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>--%>
               
                  <asp:TemplateField HeaderText="AirCraft Type*" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=20%>
                   
                    <ItemTemplate>
                        
                    <asp:DropDownList ID="ddlAirCraft" runat="server" 
                     AutoPostBack="True" OnSelectedIndexChanged="showCapacityInGrid"  >
              </asp:DropDownList>
                   
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
                  <asp:TemplateField HeaderText="Capacity(Kg)*" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=20%>
                   
                    <ItemTemplate>
                        <asp:TextBox ID="txtCapacity" Width="50px" Text="" MaxLength="4" runat="server" ></asp:TextBox>
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
               
                <asp:TemplateField HeaderText="Status *" HeaderStyle-Wrap="false">
                 <FooterTemplate>
<%--                         <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="button"  OnClick ="Addrow" />
--%>                    </FooterTemplate>
                    <ItemTemplate>
                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                         <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="58px">
                         <asp:ListItem Value ="ACTIVE"></asp:ListItem>
                         <asp:ListItem Value="CANCELLED"></asp:ListItem>
                         <asp:ListItem Value="DRAFT"></asp:ListItem>
                        </asp:DropDownList>
                         <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                    
                    </ItemTemplate>
                     <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
   
               
           </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>
        </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger  ControlID="btnAddNewRow" EventName="click"/>
        <asp:AsyncPostBackTrigger  ControlID="btnDelete" EventName="click"/>
        
        </Triggers>
     </asp:UpdatePanel> 
     </asp:Panel>  
     
    </div>
    
    <div>
    
    </div>
    
    
    </asp:Panel>
    <div id="fotbut">
    <asp:Panel ID="pnlUpdate" runat="server" Visible="False" >
        <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="button" 
            onclick="btnSave_Click"  />
            </asp:Panel>
    </div>
     </asp:Panel> 
    </div>
   </asp:Content>