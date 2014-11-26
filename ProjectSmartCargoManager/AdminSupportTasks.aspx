<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminSupportTasks.aspx.cs" Inherits="ProjectSmartCargoManager.Admin_Support_Tasks" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <div id="contentarea">
    <h1>
            Admin Support Tasks
    </h1>
    <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
             ForeColor="Red"></asp:Label>
     </p>
     
     <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Font-Size="Medium"
            Width="1022px" Height="320px">
            
            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1" Width="620px"
                Height="27px">
                <HeaderTemplate>
                    Reopen Invoice
                </HeaderTemplate>
                <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                    <br />
                <table>
                            <tr>
                                <td>
                                    Invoice # *
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" Width="200px" ></asp:TextBox> 
                                     
                                    <asp:Button ID="BtnOk" runat="server" Text="List" CssClass="button" 
                                        onclick="BtnOk_Click"/>
                                    
                                    <asp:Button ID="BtnClear" runat="server" Text="Clear" CssClass="button" 
                        onclick="BtnClear_Click" />
                                 </td>
                            </tr>
                </table>
                

  <div id="divPrint">
 <asp:GridView ID="grdInvoiceList" Width="60%" runat="server" CellPadding="2" 
          AutoGenerateColumns="False">
              <Columns>
                
               <asp:TemplateField HeaderText="InvoiceNumber">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("InvoiceNumber") %>' ></asp:Label >
                    </ItemTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Agent Code">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                
                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# Eval("InvoiceAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
        </Columns> 
</asp:GridView>

  </div>

                    <asp:Button ID="BtnReopen" runat="server" Text="Reopen" CssClass="button" 
                        onclick="BtnReopen_Click"/>

          </asp:Panel>
                
            </ContentTemplate>
                
                </asp:TabPanel>
                
            <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2" Width="500px">
                <HeaderTemplate>
                    Change Agent Code
                </HeaderTemplate>
    <ContentTemplate>
       <asp:Panel ID="Panel2" runat="server">
                <br />
                <table width="100%">
                            <tr>
                                <td>
                                    AWB: * 
                                   <asp:TextBox ID="txtAWBPrifix" runat="server" Width="37px" ></asp:TextBox>
                                   <asp:TextBoxWatermarkExtender ID="txtAWBPrifixWatermarkExtender" runat="server"
                                        TargetControlID="txtAWBPrifix" WatermarkText="Prefix" /> 
                                    <asp:TextBox ID="txtAWBNo" runat="server" Width="100px" ></asp:TextBox> 
                                    <asp:TextBoxWatermarkExtender ID="txtAWBNoWatermarkExtender" runat="server"
                                        TargetControlID="txtAWBNo" WatermarkText="AWB#" />
                                    
                                    <asp:Button ID="BtnAgentList" runat="server" Text="List" CssClass="button" 
                                        onclick="BtnAgentList_Click"/>
                                    
                                    <asp:Button ID="BtnClear2" runat="server" Text="Clear" CssClass="button" 
                                        onclick="BtnClear2_Click" />
                                </td>
                            </tr>
                </table>
                

  <div id="divPrint">
 <asp:GridView ID="grdAgentCode" Width="60%" runat="server" CellPadding="2" 
          AutoGenerateColumns="False">
 
             <Columns>
             <asp:TemplateField HeaderText="Agent Name">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("AgentName") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                                
                <asp:TemplateField HeaderText="Agent Code">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Origin">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Eval("OriginCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Destination">
                    <ItemTemplate>
                       <asp:Label ID="lblDestination" runat="server" Text='<%# Eval("DestinationCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="PC's">
                    <ItemTemplate>
                       <asp:Label ID="lblPCs" runat="server" Text='<%# Eval("PiecesCount") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Gross Weight">
                    <ItemTemplate>
                       <asp:Label ID="lblGWeight" runat="server" Text='<%# Eval("GrossWeight") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Chargable Weight">
                    <ItemTemplate>
                       <asp:Label ID="lblCWeight" runat="server" Text='<%# Eval("ChargedWeight") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
        </Columns> 
</asp:GridView>

  </div>
    
           <asp:Label ID="LblNewAgentCcode" runat="server" Text="New Agent Code"></asp:Label>
            <asp:DropDownList ID="ddlNewAgtCode" runat="server" Width="100px">
           </asp:DropDownList>
                
         <asp:Button ID="BtnChgAgtCd" runat="server" Text="Change Agent Code" CssClass="button" 
                    onclick="BtnChgAgtCd_Click"/>
          </asp:Panel>
     
                </ContentTemplate>
           </asp:TabPanel>
           
   <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel2" Width="500px">
    <HeaderTemplate>  Change AWB Date</HeaderTemplate>
     <ContentTemplate>
       <asp:Panel ID="Panel3" runat="server">
            <table>
                            <tr>
                                <td class="style1">
                                    <br />
                                    AWB: *
                                   <asp:TextBox ID="txtDateAWBPre" runat="server" Width="37px" ></asp:TextBox>
                                   <asp:TextBoxWatermarkExtender ID="txtDateAWBPreWatermarkExtender" runat="server"
                                        TargetControlID="txtDateAWBPre" WatermarkText="Prefix" /> 
                                   <asp:TextBox ID="txtDateAWBNo" runat="server" Width="100px"></asp:TextBox> 
                                   <asp:TextBoxWatermarkExtender ID="txtDateAWBNoWatermarkExtender" runat="server"
                                        TargetControlID="txtDateAWBNo" WatermarkText="AWB#" />
                                     <asp:TextBox ID="txtAWBDate" runat="server" Visible="false" Width="100px"></asp:TextBox>
                                   <asp:CalendarExtender ID="txtAWBDate_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtAWBDate" Format="dd/MM/yyyy"> </asp:CalendarExtender> 
                                    
                                   <asp:Button ID="BtnAWBDateList" runat="server" Text="List" CssClass="button" 
                                        onclick="BtnAWBDateList_Click"/>
                                    
                                   <asp:Button ID="BtnClear3" runat="server" Text="Clear" CssClass="button" 
                                        onclick="BtnClear3_Click" />
                              </td>
                            </tr>
                </table>
                

  <div id="div2">
  <asp:GridView ID="grdAWBDate" Width="60%" runat="server" CellPadding="2" 
          AutoGenerateColumns="False">
 
             <Columns>
             <asp:TemplateField HeaderText="Agent Name">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("AgentName") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                                
                <asp:TemplateField HeaderText="Agent Code">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="AWB Date">
                    <ItemTemplate>
                       <asp:Label ID="lblAWbDate" runat="server" Text='<%# Eval("AWBDate") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Origin">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Eval("OriginCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Destination">
                    <ItemTemplate>
                       <asp:Label ID="lblDestination" runat="server" Text='<%# Eval("DestinationCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="PC's">
                    <ItemTemplate>
                       <asp:Label ID="lblPCs" runat="server" Text='<%# Eval("PiecesCount") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Gross Weight">
                    <ItemTemplate>
                       <asp:Label ID="lblGWeight" runat="server" Text='<%# Eval("GrossWeight") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Chargable Weight">
                    <ItemTemplate>
                       <asp:Label ID="lblCWeight" runat="server" Text='<%# Eval("ChargedWeight") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                </asp:TemplateField>
                
        </Columns> 
</asp:GridView>

  </div>          
           <asp:Label ID="LblNewAWBDt" runat="server" Text="New AWB Date"></asp:Label>
           <asp:TextBox ID="txtAWBNewDt" runat="server" Width="100px"></asp:TextBox> 
          <asp:CalendarExtender ID="txtAWBNewDt_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtAWBNewDt" Format="dd/MM/yyyy"> </asp:CalendarExtender>
           
         <asp:Button ID="BtnChangeDt" runat="server" Text="Change Date" CssClass="button" 
                        onclick="BtnChangeDt_Click"/>
         
         
</asp:Panel>
 </ContentTemplate>
</asp:TabPanel>
           
            <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="TabPanel4">
            <HeaderTemplate>AWB Void With Zero Charges</HeaderTemplate>
   
                <ContentTemplate>
                    <br />
                    <table width="100%">
                        <tr>
                            <td>
                                AWB: *
                                <asp:TextBox ID="txtAWBPrifix1" runat="server" Width="37px"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="txtAWBPrifix1WatermarkExtender" runat="server"
                                    TargetControlID="txtAWBPrifix1" WatermarkText="Prefix" />
                                <asp:TextBox ID="txtAWBNo1" runat="server" Width="100px"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="txtAWBNo1WatermarkExtender" runat="server"
                                    TargetControlID="txtAWBNo1" WatermarkText="AWB#" />
                                
                                <asp:Button ID="BtnListAWB" runat="server" CssClass="button" 
                                    onclick="BtnVoidAWB_Click" Text="List" />
                                
                                   <asp:Button ID="btnClearVoid" runat="server" Text="Clear" CssClass="button" 
                                    onclick="btnClearVoid_Click" />    
                                
                            </td>
                        </tr>
                    </table>
                    <div ID="divPrint0">
                        <asp:GridView ID="grdAWBList" runat="server" Width="375px">
                        </asp:GridView>
                        <br />
                    </div>
                    
                                <asp:Button ID="BtnVoidListedAWB" runat="server" CssClass="button" 
                                    OnClick="BtnVoidListedAWB_Click" Text="Void" />
                </ContentTemplate>
   
            </asp:TabPanel>
           
            <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="TabPanel5">
            <HeaderTemplate>
                 Remove Spot rate for AWB
                </HeaderTemplate>
                <ContentTemplate>
                    <div style="font-size: large">
                        <table width="100%">
                            <tr>
                                <td style="font-size: small">
                                    <br />
                                    AWB: *
                                    <asp:TextBox ID="txtAWBPrifixSpotrate" runat="server" Width="37px"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtAWBPrifixSpotrateWatermarkExtender" runat="server"
                                        TargetControlID="txtAWBPrifixSpotrate" WatermarkText="Prefix" />
                                    <asp:TextBox ID="txtAWBNoSpotrate" runat="server" Width="100px"></asp:TextBox>
                                    <asp:TextBoxWatermarkExtender ID="txtAWBNoSpotrateWatermarkExtender" runat="server"
                                        TargetControlID="txtAWBNoSpotrate" WatermarkText="AWB#" />
                                   
                                    <asp:Button ID="btnListAWBDetailsSoptrate" runat="server" CssClass="button" 
                                        onclick="btnListAWBDetailsSoptrate_Click" Text="List" />
                                    
                                    <asp:Button ID="btnRemoveAWBSpotrate" runat="server" CssClass="button" 
                                        onclick="btnRemoveAWBSpotrate_Click" Text="Remove" />
                                    <br />
                                    <br />
                                    <asp:GridView ID="grdAWBListSpotrate" runat="server" Width="329px">
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
                
            <asp:TabPanel ID="TabPanel8" runat="server" HeaderText="TabPanel8">
              <HeaderTemplate>
                  Update Service Tax
                </HeaderTemplate>
                <ContentTemplate>
                    <br />
                    AWB: *
                    <asp:TextBox ID="txtAWBPrefixST" runat="server" Width="40px"></asp:TextBox>
                    <asp:TextBoxWatermarkExtender ID="txtAWBPrefixSTWatermarkExtender" runat="server"
                                    TargetControlID="txtAWBPrefixST" WatermarkText="Prefix" />
                    <asp:TextBox ID="txtAWBNoST" runat="server" Width="100px"></asp:TextBox>
                    <asp:TextBoxWatermarkExtender ID="txtAWBNoSTWatermarkExtender" runat="server"
                                    TargetControlID="txtAWBNoST" WatermarkText="AWB#" />
                    
                    <asp:Button ID="btnListAWBDetailsST" runat="server" CssClass="button" 
                        onclick="btnListAWBDetailsST_Click" Text="List" />
                    
                    <asp:Button ID="btnCorrectST" runat="server" CssClass="button" 
                        onclick="btnCorrectST_Click" Text="Correct" />
                    <br />
                    <br />
                    <asp:GridView ID="grdAWBListST" runat="server">
                    </asp:GridView>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="TabPanel6">
                <HeaderTemplate>
                    Rate Reprocess
                </HeaderTemplate>
                <ContentTemplate>
                    <br />
                    <table>
                        <tr>
                            <td>
                                AWB: *
                                <asp:TextBox ID="txtAWBPrifixReProcess" runat="server" Width="37px"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="txtAWBPrifixReProcessWatermarkExtender" runat="server"
                                    TargetControlID="txtAWBPrifixReProcess" WatermarkText="Prefix" />
                                <asp:TextBox ID="txtAWBNoReProcess" runat="server" Width="100px"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="txtAWBNoReProcessWatermarkExtender" runat="server"
                                    TargetControlID="txtAWBNoReProcess" WatermarkText="AWB#" />
                                <asp:Button ID="BtnListAWBReProcess" runat="server" CssClass="button" 
                                    onclick="BtnListAWBReProcess_Click" Text="List" />
                               
                                <asp:Label ID="lblNewRate" runat="server" Text="Enter New Rate PerKg *"></asp:Label>
                                <asp:TextBox ID="txtAWBNewRatePerKg" runat="server" Width="100px"></asp:TextBox>
                                <asp:Button ID="BtnRateReProcess" runat="server" CssClass="button" onclick="BtnRateReProcess_Click" 
                                    Text="Change Rate" />
                                <br />
                                <br />
                                <asp:GridView ID="GrdListAWBtoReProcess" runat="server" Width="368px">
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
           
            <asp:TabPanel ID="TabPanel7" runat="server" HeaderText="TabPanel7">
                <HeaderTemplate>
                   Remove DCM
                </HeaderTemplate>
                <ContentTemplate>
                    <br />
                    <table>
                        <tr>
                            <td>
                                AWB: *
                                <asp:TextBox ID="txtAWBPrefixDCM" runat="server" Width="37px"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="txtAWBPrefixDCMWatermarkExtender" runat="server"
                                    TargetControlID="txtAWBPrefixDCM" WatermarkText="Prefix" />
                                <asp:TextBox ID="txtAWBNoDCM" runat="server" Width="100px"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="txtAWBNoDCMWatermarkExtender" runat="server"
                                    TargetControlID="txtAWBNoDCM" WatermarkText="AWB#" />
                                Flight # *
                                <asp:TextBox ID="txtFlightNoDCM" runat="server" Width="60px"></asp:TextBox>
                                Flight Date * <asp:TextBox ID="txtFlightDateDCM" runat="server" Width="100px"></asp:TextBox>
                                <asp:CalendarExtender ID="txtFlightDateDCM_CalendarExtender" runat="server" 
                                    Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFlightDateDCM">
                                </asp:CalendarExtender>
                                
                                <asp:Button ID="btnListAWBDCM" runat="server" CssClass="button" 
                                    onclick="btnListAWBDCM_Click" Text="List" />
                                
                                <asp:Button ID="btnRemoveAWBDCM" runat="server" CssClass="button" 
                                    Text="Remove" onclick="btnRemoveAWBDCM_Click" />
                                <br />
                                <br />
                                <asp:GridView ID="grdListAWBDCM" runat="server" Width="366px">
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
           
           
         
       
           
           
         
           
    </asp:TabContainer>
    </div>
</asp:Content>