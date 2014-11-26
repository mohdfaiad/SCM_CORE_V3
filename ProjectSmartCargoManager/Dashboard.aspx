<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master"  Inherits="ProjectSmartCargoManager.Dashboard" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">       

    <script language="javascript" type="text/javascript">
     function NoData() {

         alert("No Data Found..Please Try Again.");

     }
             </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
    
    <div id="contentarea">
     <h1>
            <img alt="" src="images/dashboard.gif" />  <br />
            </h1>
           
    <table width="100%">
   
    <tr>
    <td>
     <div class="botline" >
        <asp:Panel ID="pnl1" runat="server" >
        
        <table width="60%">
        <tr>
        <td width="60px" align="right">
            From Date:</td>
        <td style="width:50px"  >
            <asp:TextBox ID="txtfromdate" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="txtfromdate_CalendarExtender" runat="server" 
                TargetControlID="txtfromdate">
            </asp:CalendarExtender>
        </td>
        <td width="60px" align="right">
            To Date:</td >
        
        <td width="50px">
            <asp:TextBox ID="txttodate" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="txttodate_CalendarExtender" runat="server" 
                TargetControlID="txttodate">
            </asp:CalendarExtender>
        </td>
        
        
        <td  width="50px">
            <asp:Button ID="btnList" runat="server" Text="List"  CssClass="button" 
                onclick="btnList_Click" CausesValidation="False"/>
                
        
        </td>
        
        
         <td  width="50px">
            <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="button" 
                 onclick="btnClear_Click"/>
        </td>
        </tr>
        
        </table>
        </asp:Panel>
        </div>
        </td>
    </tr>
       </table>
    </div>
   <table width="90%">
  <tr>
  <td>
    <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" ID="grdDashboard" 
                      Width="1024px" AllowSorting="True"  
            BackColor="#EEEEEE" BorderColor="Black" BorderStyle="Groove">
                        <AlternatingRowStyle CssClass="trcolor" />
                        <EmptyDataRowStyle BorderStyle="Groove" />
                        <Columns>
                           
                            <asp:TemplateField HeaderText="AWB Number">
                                <ItemTemplate>
                                    <asp:LinkButton ID="Lnkawburl" runat="server" Text='<%# Eval("AWBNumber") %>' OnClick="AWBURL" >LinkButton</asp:LinkButton>
                                <%--<asp:HyperLink ID="hypawburl" runat="server"  Text='<%# Eval("AWBNumber") %>' NavigateUrl="~/FrmConBooking.aspx" >AWBNumber</asp:HyperLink>--%>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                           
                            
                             <asp:TemplateField HeaderText="Origin">
                                <ItemTemplate>
                                <asp:Label ID="txtfltorigin" runat="server" Width="70px" BorderStyle="Ridge" Text='<%# Eval("FltOrigin") %>' Enabled=false ></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            
                            
                             <asp:TemplateField HeaderText="Destination">
                                <ItemTemplate>
                                <asp:Label ID="txtfltdest" runat="server" Width="70px" BorderStyle="Ridge" Text='<%# Eval("FltDestination") %>' Enabled=false></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            
                             <asp:TemplateField HeaderText="Flight Number">
                                <ItemTemplate>
                                <asp:Label ID="txtfltno" runat="server" Width="70px" BorderStyle="Ridge" Text='<%# Eval("FltNumber") %>' Enabled=false  ></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            
                            
                            
                             <asp:TemplateField HeaderText="Flight Date">
                                <ItemTemplate>
                                <asp:Label ID="txtfltdate" runat="server" Width="140px" BorderStyle="Ridge" Text='<%# Eval("FltDate") %>' Enabled=false></asp:Label>
                                    
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Pieces Count">
                                <ItemTemplate>
                                <asp:Label ID="txtpiecescnt" runat="server" Width="70px" BorderStyle="Ridge" Text='<%# Eval("PiecesCount") %>' Enabled=false></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            
                            
                            <asp:TemplateField HeaderText="Gross Weight">
                                <ItemTemplate>
                                <asp:Label ID="txtgrosswt" runat="server" Width="70px" BorderStyle="Ridge" Text='<%# Eval("GrossWeight") %>' Enabled=false></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Volumetric Weight">
                                <ItemTemplate>
                                <asp:Label ID="txtvolumetricwt" runat="server" Width="70px" BorderStyle="Ridge" Text='<%# Eval("VolumetricWeight") %>' Enabled=false></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Charged Weight">
                                <ItemTemplate>
                                <asp:Label ID="txtchargedwt" runat="server" Width="70px" BorderStyle="Ridge" Text='<%# Eval("ChargedWeight") %>' Enabled=false></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            
                            <%-- <asp:TemplateField HeaderText="IsFFR">
                                <ItemTemplate>
                                <asp:TextBox ID="txtIsffr" runat="server" Width="70px" BorderStyle="Ridge" Text='<%# Eval("IsFFR") %>' Enabled=false></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>--%>
                            
                            <asp:TemplateField HeaderText="IsFFR">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkisffr" runat="server" Text='<%# Eval("IsFFR") %>'  />
                                <%--<asp:TextBox ID="txtIsffa" runat="server" Width="70px" Text='<%# Eval("IsFFA") %>'></asp:TextBox>--%>
                                </ItemTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="grdrowfont" BorderStyle="Groove" 
                            HorizontalAlign="Center"></EditRowStyle>
                        <FooterStyle CssClass="grdrowfont"></FooterStyle>
                        <PagerStyle BorderStyle="Groove" />
                        <SelectedRowStyle BackColor="#FFFFCC" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                        <RowStyle CssClass="grdrowfont"></RowStyle>
                    </asp:GridView>
   
  </td>
  </tr>
   </table>
   <table width="100%">
   <tr>
   <td>
       <asp:Button ID="btnsnedffr" runat="server" Text="Send FFR" CssClass="button"  />
   </td>
   </tr>
   </table>
   
     
   
   </asp:Content>
