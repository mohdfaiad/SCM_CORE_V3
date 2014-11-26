<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="AirCargoManifest.aspx.cs" Inherits="ProjectSmartCargoManager.AirCargoManifest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="customsAWBinfo" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
     <div id="contentarea">
    <h1>Air Cargo Manifest</h1>
    <div class="botline">
        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                        <table width="100%"><tr><td>
                            Flight #&nbsp; </td>
                            <td>
                            <%--<asp:TextBox ID="txtFlightCode" runat="server" Width="43px">IT</asp:TextBox><--%>
                            <asp:TextBox ID="txtFlightNo" runat="server" Width="44px"></asp:TextBox>
                            </td>
                            <td>Flight Date</td><td>
                            <asp:TextBox ID="txtFltDt" runat="server" Width="85px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFltDt_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFltDt">
                    </asp:CalendarExtender></td>
                           <td> Port Of Lading</td><td>
                    <asp:Label ID="lblDepAirport" runat="server" Font-Bold="True" 
                        Font-Names="Verdana"></asp:Label>
                   </td> <td>Port Of Unlading</td>
                   <td>
                   <%--<asp:TextBox ID="TextBox1" runat="server" Width="80px"></asp:TextBox>--%>
                       <asp:DropDownList ID="ddlUnlading" runat="server">
                       </asp:DropDownList>
                   </td></tr><tr><td>
                            Owner Or Operator</td>
                            <td colspan="3"><asp:TextBox ID="txtOperator" runat="server" Width="250px"></asp:TextBox></td>
                           <td> Marks Of Nationality and registration</td><td colspan="3">
                            <asp:TextBox ID="txtNationality" runat="server" Width="250px"></asp:TextBox></td> </tr>
                            <tr>
                            <td>Consolidator</td>
                            <td>
                                <asp:TextBox ID="txtConsolidator" runat="server"></asp:TextBox>
                            </td>
                            <td>De-Consolidator</td>
                            <td>
                            <asp:TextBox ID="txtDeconsolidator" runat="server"></asp:TextBox>
                            </td>
                            <td></td>
                            <td>
                          <asp:Button ID="btnRetrieve" runat="server" Text="Retrieve" CssClass="button" OnClick="btnRetrieve_Click" />
                          <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click"/>
                          <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" OnClick="btnExport_Click" />
                            </td>
                            </tr>
                            </table>
                        </div>
                        <br />
                        <div class="ltfloat">
                            <asp:GridView ID="grdCargoManifest" runat="server" AutoGenerateColumns="false"
                            AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" 
                            HeaderStyle-CssClass="HeaderStyle"  PagerStyle-CssClass="PagerStyle" 
                            PageSize="10" RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                            <Columns>
                            
                            <asp:TemplateField HeaderText="AWB#">
                            <ItemTemplate>
                            <asp:Label ID="lblAWBNo" runat="server" Text='<%#Eval("")%>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                             <asp:TemplateField HeaderText="Pieces">
                            <ItemTemplate>
                            <asp:Label ID="lblPcs" runat="server" Text='<%#Eval("")%>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                             <asp:TemplateField HeaderText="Weight">
                            <ItemTemplate>
                            <asp:Label ID="lblWt" runat="server" Text='<%#Eval("")%>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                             <asp:TemplateField HeaderText="HAWBs#">
                            <ItemTemplate>
                            <asp:Label ID="lblHAWB" runat="server" Text='<%#Eval("")%>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                             <asp:TemplateField HeaderText="Shipper">
                            <ItemTemplate>
                            <asp:Label ID="lblShipper" runat="server" Text='<%#Eval("")%>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                             <asp:TemplateField HeaderText="Consignee">
                            <ItemTemplate>
                            <asp:Label ID="lblConsignee" runat="server" Text='<%#Eval("")%>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                             <asp:TemplateField HeaderText="Nature of Goods">
                            <ItemTemplate>
                            <asp:Label ID="lblGoods" runat="server" Text='<%#Eval("")%>'></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                            </Columns>
                            
                            </asp:GridView>
                        
                        </div>
                        
                        
                        
                        <%--<div class="divback">
                        <h3>Consolidator</h3>
                        <table width="100%"><tr><td>
                            AWB #</td>
                            <td><asp:TextBox ID="TextBox4" runat="server" Width="43px">IT</asp:TextBox><asp:TextBox ID="TextBox5" runat="server" Width="44px"></asp:TextBox>
                            </td>
                            <td>No Of Pieces</td><td><asp:TextBox ID="TextBox6" runat="server" Width="85px"></asp:TextBox>
                    
                    </td>
                           <td> Weight(Kg/Lb)</td><td>
                    <asp:TextBox ID="TextBox10" runat="server" Width="80px"></asp:TextBox>
                   </td> <td>No Of HAWB's</td><td><asp:TextBox ID="TextBox7" runat="server" Width="80px"></asp:TextBox></td></tr><tr><td>
                            Shipper Name</td>
                            <td colspan="3"><asp:TextBox ID="TextBox8" runat="server" Width="250px"></asp:TextBox></td>
                           <td> Shipper Address</td><td colspan="3">
                            <asp:TextBox ID="TextBox9" runat="server" Width="250px"></asp:TextBox></td> </tr></table>
                        
                        
                        </div>
                        
                        <div class="divback">
                        <h3>De-Consolidator</h3>
                        <table width="100%">
                        <tr><td>
                            Natures Of Goods</td>
                            <td><asp:TextBox ID="TextBox11" runat="server" Width="43px">IT</asp:TextBox><asp:TextBox ID="TextBox12" runat="server" Width="44px"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td><td>&nbsp;</td>
                           <td> &nbsp;</td><td>
                            &nbsp;</td> <td>&nbsp;</td><td>&nbsp;</td></tr>
                   <tr><td>
                            Consignee Name</td>
                            <td colspan="3"><asp:TextBox ID="TextBox16" runat="server" Width="250px"></asp:TextBox></td>
                           <td>Consignee  Address</td><td colspan="3">
                            <asp:TextBox ID="TextBox18" runat="server" Width="250px"></asp:TextBox></td> </tr></table>
                        </div>--%>
     </div>
    
    
    </asp:Content>