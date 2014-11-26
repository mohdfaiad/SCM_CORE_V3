<%@ Page Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="frmUpdateAWBLDetails.aspx.cs" Inherits="ProjectSmartCargoManager.frmUpdateAWBLDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"  EnableViewState="true">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
<div id="contentarea" style="margin-top:0px;">
     <h1>
            &nbsp;</h1>
    
    <div class="botline">
       <table width="100%" >
       
          <tr>
          <td>
        <h3> Enter AWB Status Details</h3> 
          
          </td>
          </tr> 
       
       <tr >
       <td valign="top" >
               <table style="width:40%;">
                    <tr>
                        <td valign="top">Prefix</td>
                        <td valign="top">
                            <asp:TextBox ID="txtPrefix"  runat="server" CssClass="inputbg60" MaxLength="4" 
                    Width="50px" Height="22px"></asp:TextBox>
                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txtPrefix" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td valign="top">AWB No</td>
                        <td valign="top"><asp:TextBox ID="TextBoxAWBno" runat="server" Height="22px"    
                    Width="140px"  MaxLength="10" ></asp:TextBox>
                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="TextBoxAWBno" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                </table>
              </td> 
       
       </tr>     
           
       
       <tr >
        <td  >
            <asp:Label ID="LabelStatus"  runat="server" ForeColor="#FF3300" Font-Bold="True" 
            Font-Size="Large" ></asp:Label>
           </td>        
       </tr>     
           
       
       </table>
        </div>
        <div style="float:left; margin-top:5px;">
   <%-- <asp:Panel ID="pnlShowData" runat="server" >--%>
       
      <br />
       <table width="100%" >
         
       <tr>        
          <td> 
              <asp:GridView ID="GridViewUpdateAWBLDetails" runat="server"   Width="100%"
         AutoGenerateColumns="False"   AllowPaging="false" AllowSorting="True"  >
              <Columns>
              <asp:TemplateField HeaderText="Message *">
              <ItemTemplate>
              <asp:DropDownList ID="ddlMessage" runat="server" >
              
              <asp:ListItem>Select</asp:ListItem>
              <asp:ListItem Text="DIS - Discrepancy" Value="Discrepancy"></asp:ListItem>
              <asp:ListItem Text="AWD - Arrival Documentation Delivered" Value="Arrival Documentation Delivered"> </asp:ListItem>
              <asp:ListItem Text="AWR - Arrival Documentation Received" Value="Arrival Documentation Received"></asp:ListItem>
              <asp:ListItem Text="NFD - Agent Notified of Arrival" Value="Agent Notified of Arrival"></asp:ListItem>
              <asp:ListItem Text="ARR - Arrived" Value="Arrived"></asp:ListItem>
              <asp:ListItem Text="BKD - Booked for Transport" Value="Booked for Transport"></asp:ListItem>
              <asp:ListItem Text="CCD - Customs Cleared" Value="Customs Cleared"></asp:ListItem>
              <asp:ListItem Text="TRM - Manifested / Transferred to carrier" Value="Manifested / Transferred to carrier"></asp:ListItem>
              <asp:ListItem Text="MAN - Manifested for Flight" Value="Manifested for Flight"></asp:ListItem>
              <asp:ListItem Text="DLV - Delivered" Value="DLV - Delivered"></asp:ListItem>
              <asp:ListItem Text="DDL - Delivered at Door" Value="Delivered at Door"></asp:ListItem>
              
              <asp:ListItem Text="RCF - Received from Flight" Value="Received from Flight"></asp:ListItem>
              <asp:ListItem Text="RCS - Ready for Carriage" Value="Ready for Carriage"></asp:ListItem>
              <asp:ListItem Text="RCT - Received from Carrier" Value="Received from Carrier"></asp:ListItem>
              <asp:ListItem Text="TFD - Transferred to Carrier" Value="Transferred to Carrier"></asp:ListItem>
              <asp:ListItem Text="PRE - Prepared for Loading" Value="Prepared for Loading"></asp:ListItem>
              <asp:ListItem Text="CRC - Reported to Customs for Clearance" Value="Reported to Customs for Clearance"></asp:ListItem>
              
              <asp:ListItem Text="TGC - Transferred to Customs" Value="Transferred to Customs"></asp:ListItem>
              <asp:ListItem Text="DEP - Departed" Value="Departed"></asp:ListItem>
              <asp:ListItem Text="FOH - On-Hand" Value="On-Hand"></asp:ListItem>
              <asp:ListItem Text="DOC - Documents received by handling agent" Value="Documents received by handling agent"></asp:ListItem>
              
              
              </asp:DropDownList>
              </ItemTemplate>
              </asp:TemplateField>
                   <asp:TemplateField HeaderText="Message Date (DD/MM/YYYY) *">
              <ItemTemplate>
              <asp:TextBox ID="txtDate" runat="server" Text='<%# Eval("MessageDate") %>'>
              </asp:TextBox>
              <asp:CalendarExtender ID="txtDate_CalenderExtender" runat="server" Enabled="true" Format="yyyy-MM-dd" TargetControlID="txtDate"></asp:CalendarExtender>
              </ItemTemplate>
              </asp:TemplateField>
                <asp:TemplateField HeaderText="Message Time (HH:MM) *">
              <ItemTemplate>
              
              <asp:TextBox ID="txtTime" Text='<%# Eval("MessageTime") %>' runat="server" MaxLength="10"></asp:TextBox>
              <%--<asp:NumericUpDownExtender ID="txtDeptTimeHr_NumericUpDownExtender" 
                                        runat="server" Maximum="23" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtTime" Width="40" />
              --%>                     
              </ItemTemplate>
              </asp:TemplateField>
                <asp:TemplateField HeaderText="Details *">
              <ItemTemplate>
              
              <asp:TextBox ID="txtDetails" runat="server" Width="300px" Text='<%# Eval("Details") %>' MaxLength="50"></asp:TextBox>
              </ItemTemplate>
              </asp:TemplateField>
              
              </Columns>
              
               <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
                <RowStyle HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="trcolor">
        
                 </AlternatingRowStyle>
               </asp:GridView> 
           </td>                
       </tr>
           
      </table>
      <asp:Button ID="BtnUpdate" runat="server" Text="Update" CssClass="button"
            onclick="BtnUpdate_Click" /> &nbsp;&nbsp;&nbsp;
       <asp:Button ID="BtnClear" runat="server" Text="Clear" CssClass="button" onclick="BtnClear_Click"/>

       <%--</asp:Panel>--%>

       </div>
       
       
 
       <div>
       
       </div>
    
    </div>
       
</asp:Content>