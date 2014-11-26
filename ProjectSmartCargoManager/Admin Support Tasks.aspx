<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin Support Tasks.aspx.cs" Inherits="ProjectSmartCargoManager.Admin_Support_Tasks" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <div id="contentarea">
    <h1>
            &nbsp;Admin Support Tasks
    </h1>
    <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
             ForeColor="Red"></asp:Label>
     </p>
     
     <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" Font-Size="Medium"
            Width="1022px" Height="320px">
            
            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1" Width="620px"
                Height="27px">
                <HeaderTemplate>
                    Reopen Invoice
                </HeaderTemplate>
                <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                <div style="font-size: large"> Invoice Information </div>
                <table width="40%">
                            <tr>
                                <td>
                                    Invoice No *<br />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" Width="200px" ></asp:TextBox> 
                                    &nbsp; 
                                    <asp:Button ID="BtnOk" runat="server" Text="Ok" CssClass="button" 
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
                <div style="font-size: large"> Agent Code </div>
                <table width="100%">
                            <tr>
                                <td>
                                    AWB Number *&nbsp; &nbsp; 
                                   <asp:TextBox ID="txtAWBPrifix" runat="server" Width="37px" ></asp:TextBox> 
                                 &nbsp; &nbsp;
                                    <asp:TextBox ID="txtAWBNo" runat="server" Width="100px" ></asp:TextBox> 
                                    &nbsp; &nbsp;
                                   <%--Agent Code &nbsp; &nbsp;<asp:DropDownList ID="ddlOldAgentCode" runat="server" AutoPostBack="True" Width="100px"></asp:DropDownList>
                                    &nbsp; &nbsp;--%> 
                                    <asp:Button ID="BtnAgentList" runat="server" Text="OK" CssClass="button" 
                                        onclick="BtnAgentList_Click"/>
                                    &nbsp; &nbsp; 
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
         <div style="font-size: large"> AWB Date </div>
            <table width="90%">
                            <tr>
                                <td>
                                    AWB Number *
                                   &nbsp;&nbsp;
                                   <asp:TextBox ID="txtDateAWBPre" runat="server" Width="37px" ></asp:TextBox> 
                                   &nbsp;&nbsp;
                                   <asp:TextBox ID="txtDateAWBNo" runat="server" Width="100px"></asp:TextBox> 
                                   &nbsp;&nbsp;
                                   &nbsp;&nbsp;<asp:TextBox ID="txtAWBDate" runat="server" Width="100px"></asp:TextBox>
                                   <asp:CalendarExtender ID="txtAWBDate_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtAWBDate" Format="dd/MM/yyyy"> </asp:CalendarExtender> 
                                   &nbsp; &nbsp;
                                   <asp:Button ID="BtnAWBDateList" runat="server" Text="OK" CssClass="button" 
                                        onclick="BtnAWBDateList_Click"/>
                                   &nbsp; &nbsp;
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
          &nbsp;&nbsp
         <asp:Button ID="BtnChangeDt" runat="server" Text="Change Date" CssClass="button" 
                        onclick="BtnChangeDt_Click"/>
         
         
</asp:Panel>
 </ContentTemplate>
</asp:TabPanel>
           
    </asp:TabContainer>
    </div>
</asp:Content>