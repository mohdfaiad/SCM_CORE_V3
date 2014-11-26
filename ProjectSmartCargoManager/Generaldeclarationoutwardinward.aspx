<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="Generaldeclarationoutwardinward.aspx.cs" Inherits="ProjectSmartCargoManager.Generaldeclarationoutwardinward" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="customsAWBinfo" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
    <h1>General Declaration(outward/Inward)</h1>
    <div class="botline">
    <asp:Label ID="lblStatus" runat="server"></asp:Label><br />
                        <table width="100%"><tr><td>
                            Flight #&nbsp; </td>
                            <td><asp:TextBox ID="txtFlightNo" runat="server" Width="43px"></asp:TextBox>
                            <%--<asp:TextBox ID="txtFlightID" runat="server" Width="44px"></asp:TextBox>--%>
                            </td>
                            <td>Flight Date</td><td><asp:TextBox ID="txtFltDt" runat="server" Width="85px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFltDt_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFltDt">
                    </asp:CalendarExtender></td>
                           <td> Departure from:</td><td>
                               <asp:DropDownList ID="ddlDepFrm" runat="server">
                               </asp:DropDownList>
                    <%--<asp:Label ID="lblDepAirportValue" runat="server" Font-Bold="True" 
                        Font-Names="Verdana" Text="BOM"></asp:Label>--%>
                   </td> <td>Arrival at</td>
                   <td>
                   <%--<asp:TextBox ID="TextBox1" runat="server" Width="80px"></asp:TextBox>--%>
                       <asp:DropDownList ID="ddlArrivalAt" runat="server">
                       </asp:DropDownList>
                   </td>
                   </tr><tr><td>
                            Owner Or Operator</td>
                            <td colspan="3"><asp:TextBox ID="txtOperator" runat="server" Width="250px"></asp:TextBox></td>
                           <td> Marks Of Nationality and registration</td><td colspan="3">
                            <asp:TextBox ID="txtNationality" runat="server" Width="250px"></asp:TextBox></td> 
                            </tr>
                            <tr><td>
                                <asp:Button ID="btnRetrieve" runat="server" Text="Retrieve" CssClass="button" 
                                    onclick="btnRetrieve_Click" />
                                </td></tr>
                            </table>
                 
                       
                        </div>
     
       <div>
                        <h2>Flight Routing</h2><table width="90%" cellspacing="3">
                        <tr>
                        <td>
                       
                        
                          
                        </td>
                        <%--<td>Place</td><td><asp:TextBox ID="TextBox4" runat="server" Width="250px"></asp:TextBox></td> <td>Total Number of Crew</td><td><asp:TextBox ID="TextBox5" runat="server" Width="250px"></asp:TextBox></td>--%>
                        </tr>
                        <tr>
                            <td colspan="2"><h3>Departure Place</h3></td> <td colspan="2"><h3>Arrival 
                            Place</h3></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            </tr>
                            <tr><td>Embarking</td><td>
                            <asp:TextBox ID="txtEmbarking" runat="server" Width="250px"></asp:TextBox></td> <td>Disembarking</td><td>
                            <asp:TextBox ID="txtDisembarking" runat="server" Width="250px"></asp:TextBox></td><td>
                                    &nbsp;</td><td>
                                    &nbsp;</td></tr><tr><td>Through on Same Flight</td><td><asp:TextBox ID="txtDepThrough" runat="server" Width="250px"></asp:TextBox></td> <td>
                            Through on Same flight</td><td><asp:TextBox ID="txtArrThrough" runat="server" Width="250px"></asp:TextBox></td><td>
                            &nbsp;</td><td>&nbsp;</td></tr><tr>
                        <td colspan="2"><h3>Number Of Sed&#39;s and Awb&#39;s</h3></td> <td>
                            &nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>SED&#39;s</td><td> <asp:TextBox ID="txtSED" runat="server" Width="80px"></asp:TextBox></td> <td>
                            AWB&#39;s</td><td> <asp:TextBox ID="txtAWBs" runat="server" Width="80px"></asp:TextBox></td><td> 
                        &nbsp;</td><td> &nbsp;</td></tr><tr><td>&nbsp;</td><td> &nbsp;</td> <td>
                            &nbsp;</td><td> &nbsp;</td><td> &nbsp;</td><td> &nbsp;</td></tr><tr><td>Declaration of Health</td><td> 
                        <asp:TextBox ID="txtHealthDeclaration" runat="server" Width="80px"></asp:TextBox></td> <td>
                           Other Condition</td><td> <asp:TextBox ID="txtOtherCondition" runat="server" Width="80px"></asp:TextBox></td><td> 
                        Treatment</td><td> <asp:TextBox ID="txtTreatment" runat="server" Width="80px"></asp:TextBox></td></tr></table>
                            <br />
                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" 
                                onclick="btnAdd_Click"/>
                        <asp:Button ID="btnDel" runat="server" Text="Delete" CssClass="button" 
                                onclick="btnDel_Click"/>
                                <br />     
                              <asp:GridView ID="grdRouting" runat="server" AutoGenerateColumns="false" Width="100%"
                             AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" 
                             HeaderStyle-CssClass="HeaderStyle" PagerStyle-CssClass="PagerStyle" 
                             RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                            <Columns>
                            <%--<asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="false"/>--%>
                            <asp:TemplateField HeaderText="Place">
                            <ItemTemplate>
                            <asp:TextBox ID="txtPlace" runat="server" Text='<%#Eval("PLACE")%>'></asp:TextBox>
                            </ItemTemplate>                            
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Total Of Crew">
                            <ItemTemplate>
                            <asp:TextBox ID="txtCrewNo" runat="server" Text='<%#Eval("TOTALCREW")%>'></asp:TextBox>
                            </ItemTemplate>                            
                            </asp:TemplateField>
                            
                            </Columns>
                            </asp:GridView>
                        </div>
                        <div>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" onclick="btnSave_Click" 
                               />
                            <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" 
                                onclick="btnExport_Click" />
                        </div>
    </div>
    
    </asp:Content>
