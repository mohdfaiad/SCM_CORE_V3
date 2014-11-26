<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="AWBAcceptance.aspx.cs" Inherits="ProjectSmartCargoManager.AWBAcceptance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="customsAWBinfo" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    
    <div id="contentarea">
    <h1>AWB Acceptance</h1>
    <div class="botline">
    <asp:Label ID="lblStatus" runat="server"></asp:Label>
    <table>
 <tr><td>AWB</td><td><asp:TextBox ID="txtAwbPrefix" runat="server" MaxLength="3" Width="40px" CssClass="alignrgt"></asp:TextBox>
     <asp:TextBox ID="txtAWBNo" runat="server" MaxLength="11" CssClass="alignrgt" onChange="javascript:OnReadAWB(this);"></asp:TextBox></td>
               
               <td><asp:Button ID="btnListAgentStock" runat="server" Text="Retrieve" 
                       CssClass="button" onclick="btnListAgentStock_Click"/></td> </tr></table>
    
    </div>
    <div class="ltfloat">
   
    <table width="100%"><tr><td>Routing</td><td colspan="3"><asp:TextBox ID="TextBox3" runat="server" Width="30px" AutoPostBack="true" MaxLength="3"
                                           />&nbsp;<asp:TextBox ID="TextBox4" runat="server" Width="50px" MaxLength="3"
                                           />&nbsp;<asp:TextBox ID="TextBox5" runat="server" Width="50px" MaxLength="3"
                                           />&nbsp;<asp:TextBox ID="TextBox6" runat="server" Width="50px" MaxLength="3"
                                           />&nbsp;<asp:TextBox ID="TextBox7" runat="server" Width="50px" MaxLength="3"
                                           />&nbsp;<asp:TextBox ID="TextBox8" runat="server" Width="50px" MaxLength="3"
                                           />&nbsp;<img src="Images/search1.png" /></td><td>Transferring&nbsp; from</td>
                                           <td><asp:TextBox ID="txtTransFrm" runat="server"  Width="60px" ></asp:TextBox>
                <%--<asp:CalendarExtender ID="CalendarExtender1" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate"></asp:CalendarExtender>--%>
              </td><td>Transferring To</td><td><asp:TextBox ID="txtTransTo" runat="server"  Width="60px" ></asp:TextBox>
                <%--<asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate"></asp:CalendarExtender>--%>
              </td><td>Origin</td><td>
              <%--<asp:TextBox ID="TextBox1" runat="server" Width="60px" AutoPostBack="true" MaxLength="3"/>--%>
                  <asp:DropDownList ID="ddlOrg" runat="server">
                  </asp:DropDownList>
              </td><td>Dest</td><td>
                 <%--<asp:TextBox ID="txtDest" runat="server" Width="60px" AutoPostBack="true" MaxLength="3"/>--%>
                  <asp:DropDownList ID="ddlDest" runat="server">
                  </asp:DropDownList>
                 </td>
   </tr>
   
   <tr>
   <td>Description</td>
   <td><asp:TextBox ID="txtDescription" runat="server" Width="120px" /> 
      </td>
   <td>Product<asp:TextBox ID="txtProduct" runat="server" Width="50px" />&nbsp;<img src="Images/search1.png" /></td>
    <td>Priority</td>
    <td><asp:TextBox ID="txtPriority" runat="server" Width="60px" />
        Status</td>
    <td><asp:TextBox ID="txtStatus" runat="server" Width="60px" /></td>
    <td align="right">Pad</td>
    <td><asp:TextBox ID="txtPad" runat="server" Width="60px" /></td>
    <td>EU Customs</td>
    <td><asp:TextBox ID="txtCustoms" runat="server" Width="60px" /></td>
    <td>Customs Station</td><td><asp:TextBox ID="txtCustStn" runat="server" Width="60px" /></td>
    
    </tr></table>
    
     <h2>AirWay Bill</h2>
    <table width="100%">
    <tr>
    <td>Accepted Pieces</td>
    <td><asp:TextBox ID="txtAccPcs" runat="server" Width="120px" /> </td>
    <td>Accepted Weight</td>
    <td><asp:TextBox ID="txtAccWt" runat="server" Width="120px" /> </td>
    <td>Volume</td><td><asp:TextBox ID="txtVol" runat="server" Width="120px" /> </td>
    <td>Calculate Volume</td><td><img src="Images/calculator.png" height="18px"/></td><td><asp:CheckBox ID="chkSluld" runat="server" Text="Sluld" /></td>
    </tr>
    <tr>
    <td>Total Pieces</td>
    <td><asp:TextBox ID="txtTotPcs" runat="server" Width="120px" /> </td>
    <td>Total Weight</td>
    <td><asp:TextBox ID="txtTotWt" runat="server" Width="120px" /> </td>
    <td>Total Volume</td><td><asp:TextBox ID="txtTotVol" runat="server" Width="120px" /> </td>
     <td colspan="2">
         <asp:RadioButtonList ID="radbtn" runat="server">
         <asp:ListItem Text="Contour"></asp:ListItem>  
         <asp:ListItem Text="Pallets"></asp:ListItem> 
         <asp:ListItem Text="LD2"></asp:ListItem>   
         </asp:RadioButtonList>
         <asp:TextBox ID="txtCountourVal" runat="server"></asp:TextBox>
     <%--<asp:RadioButton ID="radContour" runat="server" Text="Contour" /> 
     <asp:RadioButton ID="radPallets" runat="server" Text="Pallets" />
     <asp:RadioButton ID="radLD2" runat="server" Text="LD2" />--%>
     </td>
     <td><asp:CheckBox ID="chkSluldWt" runat="server" Text="Verified Sluld Weight" /></td>
    </tr>
    </table>
    <h3>Special Handling Codes</h3>
    <table width="100%">
    <tr><td></td><td colspan="3"><asp:TextBox ID="TextBox22" runat="server" Width="30px" MaxLength="3"
                                           />&nbsp;<asp:TextBox ID="TextBox23" runat="server" Width="50px" MaxLength="3"
                                           />&nbsp;<asp:TextBox ID="TextBox24" runat="server" Width="50px" MaxLength="3"
                                           />&nbsp;<asp:TextBox ID="TextBox25" runat="server" Width="50px" MaxLength="3"
                                           />&nbsp;<asp:TextBox ID="TextBox26" runat="server" Width="50px" MaxLength="3"
                                           />&nbsp;<img src="Images/search1.png" /></td><td>
Cross Reference</td><td><asp:TextBox ID="txtCrossRef" runat="server"  Width="60px" ></asp:TextBox>
               </td><td>Transferring To</td><td><asp:TextBox ID="txtSCHTransTo" runat="server"  Width="60px" ></asp:TextBox>
                </td><td>Origin</td><td>
                <%--<asp:TextBox ID="TextBox30" runat="server" Width="60px" AutoPostBack="true" MaxLength="3" />--%>
                    <asp:DropDownList ID="ddlSCHOrg" runat="server">
                    </asp:DropDownList>
                </td><td>Dest</td><td>
                <%--<asp:TextBox ID="TextBox31" runat="server" Width="60px" AutoPostBack="true" MaxLength="3"/>--%>
                <asp:DropDownList ID="ddlSCHDest" runat="server">
                    </asp:DropDownList>
                </td>
   </tr></table>
   <h3>Acceptance</h3>
   <table width="100%" cellspacing="2">
   <tr>
   <td>Warehouse Location</td>
   <td> <asp:TextBox ID="txtWareHouseLoc" runat="server" Width="60px" /></td>
   <td>&nbsp;</td>
   <td>Container Id</td><td> <asp:TextBox ID="txtContainerId" runat="server" Width="60px" /></td>
   <td>Flight</td><td><asp:TextBox ID="txtFlightNo" Width="80" runat="server"></asp:TextBox><img src="Images/search1.png" /></td>
   <td>Offload</td>
   <td colspan="2"><asp:TextBox ID="txtOffload" runat="server" Width="60px" /><img src="Images/search1.png" /></td>
   <td> &nbsp;</td>
   </tr>
   <tr>
   <td>Handling Remarks</td>
   <td><asp:TextBox ID="txtRemarks" runat="server" Width="120px" /></td>
   <td>&nbsp;</td>
   <td>Labels</td><td><asp:TextBox ID="txtLabels" runat="server" Width="60px" /></td>
       <td>&nbsp;</td><td colspan="4"><asp:CheckBox ID="chkCarrHold" runat="server" Text="Carrier Hold" /> <asp:CheckBox ID="chkDocRec" runat="server" Text="Documents Received" /></td>
       <td>&nbsp;</td>
   </tr>
   
   <tr>
   <td>Actual Drop Off </td>
   <td><asp:TextBox ID="txtDropOff" runat="server" Width="60px"/>
       <asp:CalendarExtender ID="dropoffext" runat="server" TargetControlID="txtDropOff" Format="dd/MM/yyyy">
       </asp:CalendarExtender>
   </td>
   <td>&nbsp;</td>
   <td>By</td><td><asp:TextBox ID="txtDropBy" runat="server" Width="60px" /></td><td>&nbsp;</td>
       <td colspan="4">&nbsp;</td>
       <td>&nbsp;</td>
   </tr>
   
   <tr>
   <td>In Bond?</td>
   <td><asp:CheckBox ID="chkBonded" runat="server" /></td>
   <td>&nbsp;</td>
   <td>Arrival Port</td>
   <td>
       <asp:TextBox ID="txtArrPort" runat="server"></asp:TextBox>
       </td>
   <td>Dest Station</td><td><asp:TextBox ID="txtDestStn" 
           runat="server"></asp:TextBox></td>
       <td>Import Flight</td>
       <td><asp:TextBox ID="txtImportFlt" runat="server"></asp:TextBox></td>
       <td>Part Arrival?</td>
       <td><asp:CheckBox ID="chkPartArr" runat="server" /></td>
   </tr>
   
   </table>
    </div>
    <div id="fotbut">
<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" onclick="btnSave_Click" />
<asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click" />
</div>
   
   <div class="ltfloat">
        <asp:GridView ID="grdAcceptanceList" runat="server" AutoGenerateColumns="false"
        AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
        PagerStyle-CssClass="PagerStyle" PageSize="15" RowStyle-CssClass="RowStyle" 
        SelectedRowStyle-CssClass="SelectedRowStyle">
        <Columns>
       
        <%--<asp:TemplateField HeaderText="Segment">
        <ItemTemplate>
        <asp:Label ID="lblSegment" runat="server" Text='<%#Eval("")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>--%>
       
        <asp:TemplateField HeaderText="Flight Number">
        <ItemTemplate>
        <asp:Label ID="lblFltNo" runat="server" Text='<%#Eval("FlightNo")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <%--Extra--%>
        <asp:TemplateField HeaderText="Flight Date">
        <ItemTemplate>
        <asp:Label ID="lblFltDt" runat="server" Text='<%#Eval("FlightDate")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Accepted Pieces">
        <ItemTemplate>
        <asp:Label ID="lblAccPcs" runat="server" Text='<%#Eval("AccpPieces")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
         
         <asp:TemplateField HeaderText="Accepted Weight">
        <ItemTemplate>
        <asp:Label ID="lblAccWt" runat="server" Text='<%#Eval("AccpWeight")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Volume">
        <ItemTemplate>
        <asp:Label ID="lblAccpVol" runat="server" Text='<%#Eval("AccpVolume")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <%--<asp:TemplateField HeaderText="ULDs">
        <ItemTemplate>
        <asp:Label ID="lblULD" runat="server" Text='<%#Eval("")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>--%>
        
        <asp:TemplateField HeaderText="Status">
        <ItemTemplate>
        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <%--Extra Columns--%>
        <asp:TemplateField HeaderText="Ware House Locn">
        <ItemTemplate>
        <asp:Label ID="lblWHLoc" runat="server" Text='<%#Eval("WHLocation")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Container ID">
        <ItemTemplate>
        <asp:Label ID="lblContId" runat="server" Text='<%#Eval("ContainerId")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Import Flight">
        <ItemTemplate>
        <asp:Label ID="lblImpFlt" runat="server" Text='<%#Eval("ImportFlight")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Import Flight Date">
        <ItemTemplate>
        <asp:Label ID="lblImpFltDt" runat="server" Text='<%#Eval("ImportFltDate")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        </Columns>
        </asp:GridView>
    </div>
     
    </div>
    
 
</asp:Content>