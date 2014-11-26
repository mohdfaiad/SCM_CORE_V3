<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="CustomsAWBgoodslist.aspx.cs" Inherits="ProjectSmartCargoManager.CustomsAWBgoodslist" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="customsAWBinfo" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
<h1>Customs AWB Information-Goods List</h1>
<div class="botline">
<asp:Label ID="lblStatus" runat="server"></asp:Label>
 <table>
 <tr><td><asp:TextBox ID="txtAwbPrefix" runat="server" MaxLength="3" Width="40px" CssClass="alignrgt"></asp:TextBox>
            <asp:TextBox ID="txtAWBNo" runat="server" MaxLength="11" TabIndex="1" 
                CssClass="alignrgt" onChange="javascript:OnReadAWB(this);"></asp:TextBox></td>
                <td>House</td>
                <td><asp:TextBox ID="txtHouse" runat="server"></asp:TextBox></td> 
               
              <td>Country code</td><td><asp:DropDownList ID="countrycode" runat="server"></asp:DropDownList></td>
               <td><asp:Button ID="btnListAgentStock" runat="server" Text="Retrieve" 
                       CssClass="button" onclick="btnListAgentStock_Click"
                /></td> </tr></table>
                </div>
 <div class="ltfloat" style="width:100%;">
          <table width="100%">
                <tr>
                <td>Origin</td><td><asp:DropDownList ID="ddlOrg" runat="server" Width="60px" onchange="javascript:SetProcessFlag(); updateFlightStation(this);"
                                            TabIndex="2">
                                        </asp:DropDownList></td>
                <td>Destination</td><td><asp:DropDownList ID="ddlDest" runat="server" Width="60px" AutoPostBack="true" MaxLength="3"
                                            TabIndex="3"  onchange="callShow(); SetProcessFlag();">
                                        </asp:DropDownList>
                                        
                                        <asp:TextBox ID="txtDest" runat="server" Width="60px" AutoPostBack="true" MaxLength="3"
                                            Visible="false"/></td>
                <td>Customs</td><td><asp:TextBox ID="txtCustoms" runat="server" width="60"  ></asp:TextBox></td>
                <td>Pieces</td><td><asp:TextBox ID="txtPcs" runat="server" width="60"></asp:TextBox></td>
                <td>Weight</td><td><asp:TextBox ID="txtWt" runat="server" width="60"></asp:TextBox></td>
                 <td>Description</td><td><asp:TextBox ID="txtDescription" runat="server" width="160"  ></asp:TextBox></td>
                 <td>Consol</td><td><asp:TextBox ID="txtConsole" runat="server" width="60"></asp:TextBox></td>
                    <td>Quary Status</td><td><asp:TextBox ID="txtQueryStat" runat="server" width="60"></asp:TextBox></td>
                    </tr>
                <tr><td>Shipper</td><td><asp:TextBox ID="txtShipper" runat="server" width="60"></asp:TextBox></td>
                <td>Consingnee</td><td><asp:TextBox ID="txtConsignee" runat="server" width="60"></asp:TextBox></td>
                <td>Customs Value</td><td><asp:TextBox ID="txtCustVal" runat="server" width="60"></asp:TextBox></td>
                <td>currency</td><td><asp:TextBox ID="txtCurr" runat="server" width="60"></asp:TextBox></td>
                <td>FDA</td><td><asp:TextBox ID="txtFDA" runat="server" width="60"></asp:TextBox></td>
                <td></td><td></td>
                </tr></table></div><br />
                <div class="ltfloat" style="width:100%;">
                <table width="100%">
                <tr><td>&nbsp;</td><td>&nbsp;</td>
                <td>&nbsp;</td><td>
                    &nbsp;</td>
                <td>&nbsp;</td><td> &nbsp;</td>
                    <td>&nbsp;</td><td>&nbsp;</td>
                    <td>&nbsp;</td><td>&nbsp;</td>
                    <td>&nbsp;</td>
                    </tr>
                <tr><td>Flight#</td><td><asp:TextBox ID="txtFlightNo" runat="server"></asp:TextBox></td>
                <td>Date</td><td>
                <asp:TextBox ID="txtFlightFromdate" runat="server"  Width="100px" 
                            ToolTip="Please enter date format: dd/MM/yyyy"></asp:TextBox>
                <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate">
              </asp:CalendarExtender></td>
                <td>Arrival status</td><td> <asp:DropDownList ID="ddlArrState" runat="server" width="60" 
                        ToolTip="Please Select ">
                        <asp:ListItem Selected="True">ALL</asp:ListItem>
                    </asp:DropDownList> </td>
                    <td>Offload</td><td><asp:TextBox ID="txtOffload" runat="server" width="60"  ></asp:TextBox></td>
                    <td>Part</td><td><asp:TextBox ID="txtPart" runat="server" width="60"  ></asp:TextBox></td>
                    <td>&nbsp;</td>
                    </tr>
                <tr> <td>Shed</td><td><asp:TextBox ID="txtShed" runat="server" width="60"></asp:TextBox></td>
                                               <td>Agent</td><td><asp:TextBox ID="txtAgent" runat="server" width="60"></asp:TextBox></td>
                                            
              <td>Onward Carrier</td><td>
              <%--<asp:DropDownList ID="ddlOnwardCarr" runat="server" Width="60px" onchange="javascript:SetProcessFlag(); updateFlightStation(this);" TabIndex="2">
              </asp:DropDownList>--%>
                  <asp:TextBox ID="txtOnwardCarr" runat="server"></asp:TextBox>
              </td>
              <td>Transit Control</td><td>
              <%--<asp:DropDownList ID="ddlTransCtrl" runat="server" Width="60px" AutoPostBack="true" MaxLength="3"
                                            TabIndex="3"  onchange="callShow(); SetProcessFlag();">
                                        </asp:DropDownList>--%>
              <asp:TextBox ID="txtTransCtrl" runat="server" Width="60px"/></td>
                                            
                                            <td>Bond</td><td><asp:TextBox ID="txtBond" runat="server" width="60"></asp:TextBox></td>
                                               <td><asp:CheckBox ID="chkDel" runat="server" Text="Delete" /></td>
                                            
                                            </tr>
                
                </table>

</div>
<div id="fotbut">
<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" onclick="btnSave_Click" />
<asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click" />
</div>
 <div class="ltfloat">
        <asp:GridView ID="grdGoodsList" runat="server" AutoGenerateColumns="false"
        AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
        PagerStyle-CssClass="PagerStyle" PageSize="15" RowStyle-CssClass="RowStyle" 
        SelectedRowStyle-CssClass="SelectedRowStyle">
        <Columns>
       
        <asp:TemplateField HeaderText="Flight No">
        <ItemTemplate>
        <asp:Label ID="lblFltNo" runat="server" Text='<%#Eval("FlightNo")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="Date">
        <ItemTemplate>
        <asp:Label ID="lblDt" runat="server" Text='<%#Eval("FlightDate")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
         <asp:TemplateField HeaderText="Reported Date">
        <ItemTemplate>
        <asp:Label ID="lblRepDt" runat="server" Text='<%#Eval("CreatedOn")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Part">
        <ItemTemplate>
        <asp:Label ID="lblPart" runat="server" Text='<%#Eval("Part")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Pieces">
        <ItemTemplate>
        <asp:Label ID="lblPcs" runat="server" Text='<%#Eval("Pieces")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
         
         <asp:TemplateField HeaderText="Weight">
        <ItemTemplate>
        <asp:Label ID="lblWt" runat="server" Text='<%#Eval("Weight")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Arrival">
        <ItemTemplate>
        <asp:Label ID="lblArr" runat="server" Text='<%#Eval("Arrival")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Offload">
        <ItemTemplate>
        <asp:Label ID="lblOffload" runat="server" Text='<%#Eval("Offload")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Customs Status">
        <ItemTemplate>
        <asp:Label ID="lblCustStatus" runat="server" Text='<%#Eval("Customs")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Shed">
        <ItemTemplate>
        <asp:Label ID="lblShed" runat="server" Text='<%#Eval("Shed")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Agent">
        <ItemTemplate>
        <asp:Label ID="lblAgent" runat="server" Text='<%#Eval("Agent")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Transit Control">
        <ItemTemplate>
        <asp:Label ID="lblTransCtrl" runat="server" Text='<%#Eval("TransitControl")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Onward Carrier">
        <ItemTemplate>
        <asp:Label ID="lblOnwcarr" runat="server" Text='<%#Eval("OnwardCarrier")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Bond">
        <ItemTemplate>
        <asp:Label ID="lblBond" runat="server" Text='<%#Eval("Bond")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <%--<asp:TemplateField HeaderText="Error">
        <ItemTemplate>
        <asp:Label ID="lblError" runat="server" Text='<%#Eval("")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>--%>
        
        <%--<asp:TemplateField HeaderText="Reported">
        <ItemTemplate>
        <asp:Label ID="lblReported" runat="server" Text='<%#Eval("")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>--%>
        
        <asp:TemplateField HeaderText="FDA">
        <ItemTemplate>
        <asp:Label ID="lblFDA" runat="server" Text='<%#Eval("FDA")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <%--<asp:TemplateField HeaderText="FSN">
        <ItemTemplate>
        <asp:Label ID="lblFSN" runat="server" Text='<%#Eval("")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>--%>
        
        </Columns>
        </asp:GridView>
    </div>
    </asp:Content>
