<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTracerExcel.aspx.cs" Inherits="MyKfCargo.FrmTracerExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
      <link type="text/css" href="SControls.css" rel="stylesheet" />
    
</head>
<body>
    <form id="form1" runat="server">
    
    
        <asp:GridView ID="grdReports" runat="server" CellPadding="3" 
            ForeColor="White" AutoGenerateColumns="False" 
            ShowFooter="True" BackColor="White" 
            BorderColor="#333333" BorderStyle="Solid" BorderWidth="1px">
            <RowStyle BackColor="White" ForeColor="#990000" CssClass="ctrlGrid" />
            
                    <FooterStyle BackColor="#990000" ForeColor="White" CssClass="ctrlGrid" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" CssClass="ctrlGrid" />
                    <SelectedRowStyle BackColor="White" ForeColor="#990000" CssClass="ctrlGrid" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" CssClass="ctrlGrid" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#990000" CssClass="ctrlGrid" />
                <Columns>
                        <asp:TemplateField HeaderText="Tracer#" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                                <ItemTemplate>
                                <asp:Label ID="lblTracerNoV" runat="server" Text='<%# Bind("TracerNo") %>'></asp:Label>
                                </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" HeaderStyle-Wrap="true" HeaderStyle-Width="75px">
                            
                            <ItemTemplate>
                                <asp:Label ID="lblAwbDate" runat="server" Text='<%# Bind("AWBDate") %>'></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="75px"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AWB#" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <asp:Label ID="lblAWBNo" runat="server" Text='<%# Bind("AWBNo") %>'></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <asp:Label ID="lblOrg" runat="server" Text='<%# Bind("Origin") %>'></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dest" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <asp:Label ID="lblDest" runat="server" Text='<%# Bind("Dest") %>'></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Flight#" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <asp:Label ID="lblFlightNo" runat="server" Text='<%# Bind("FltNo") %>'></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Contents" HeaderStyle-Wrap="true" HeaderStyle-Width="70px">
                             <ItemTemplate>
                                <asp:Label ID="Label11" runat="server" Text='<%# Bind("ContentType") %>'>
                                </asp:Label>
                             </ItemTemplate>
                             <HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                            </asp:TemplateField>
                                                        
                            <asp:TemplateField HeaderText="Total Pcs" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <asp:Label ID="lblSentPcs" runat="server" Text='<%# Bind("SentPcs") %>'></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                            </asp:TemplateField>
                            
                            <%--<asp:TemplateField HeaderText="Recd Pcs" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("RecdPcs") %>'></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                            </asp:TemplateField>--%>
                            
                            <asp:TemplateField HeaderText="Wt" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <asp:Label ID="lblChargeableWgt" runat="server" Text='<%# Bind("ChargebleWgt") %>'></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Short Detail">
                            <ItemTemplate>
                                <asp:Label ID="lblMissedPcs" runat="server" Text='<%# Bind("MissedPcs") %>'></asp:Label></ItemTemplate><EditItemTemplate>
                                <asp:TextBox ID="txtMissedPcs" runat="server" Width="50px" Text='<%# Bind("MissedPcs") %>'></asp:TextBox></EditItemTemplate>
                            </asp:TemplateField>
                            
                            <%--<asp:TemplateField HeaderText="AWB Status" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            
                             <ItemTemplate>
                                <asp:Label ID="Label11" runat="server" Text='<%# Bind("AWBStatus") %>'></asp:Label>
                             </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                            </asp:TemplateField>--%>
                            
                            <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            
                            <ItemTemplate>
                                <asp:Label ID="Label12" runat="server" Text='<%# Bind("LastRecdStatus") %>'></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                        </asp:TemplateField>
                        
                            
                            <asp:TemplateField HeaderText="Generate Tracer" HeaderStyle-Wrap="true" HeaderStyle-Width="100px">
                            <ItemTemplate >
                                <asp:Label ID="lblGenTracer" runat="server" Text='<%# Eval("IsGenTracer").ToString().Equals("True") ? " Tracer Generated " : " Tracer Not Generated " %>'></asp:Label>
                                
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                        </asp:TemplateField>
                        
                        
                        <asp:TemplateField HeaderText="Found Type" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <asp:Label ID="lblFoundType" runat="server" Text='<%# Bind("LastUpdatedStatus") %>'></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Found Pcs" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                             <ItemTemplate>
                                <asp:Label ID="lblFoundPcs" runat="server" Text='<%# Bind("FoundPcs") %>'></asp:Label>
                             </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Found Location" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            
                            <ItemTemplate>
                                <asp:Label ID="lblFoundLoc" Text='<%# Bind("FoundAtStcCode") %>' runat="server"></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ScanDate" HeaderStyle-Wrap="true" HeaderStyle-Width="65px">
                            <ItemTemplate>
                                <asp:Label ID="Label14" runat="server" Text='<%# Bind("LastupdateDatetime") %>'></asp:Label>
                            </ItemTemplate>

<HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                        </asp:TemplateField>
                      
                        <asp:TemplateField HeaderText="Close Tracer" HeaderStyle-Wrap="true" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Label ID="lblIsClosedTr" runat="server" 
                                    Text='<%# Eval("IsClosed").ToString().Equals("True") ? " Tracer Closed " : " Tracer Open " %>'></asp:Label>
                           </ItemTemplate><HeaderStyle Wrap="True" Width="70px"></HeaderStyle>
                        </asp:TemplateField>
                       
                        
                        
                        
                    </Columns>
        </asp:GridView>
    
    
    </form>
</body>
</html>