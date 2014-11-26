<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTracerHistoryPopup.aspx.cs" Inherits="MyKFCargoNewProj.FrmTracerHistoryPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tracer History</title>
    <link type="text/css" href="SControls.css" rel="stylesheet" />
    
    <style type="text/css">
       
    .hiddencol1
    {
        display:none;
    }
    .viscol1
    {
        display:block;
    }
        </style>
</head>
<body>
    <form id="form1" method="post" runat="server">
                    <input id="hdnAwbNo" type="hidden" runat="server"/>
                    <input id="hdnTracerNo" type="hidden" runat="server"/>
    <div style="text-align: center">
        
        <asp:Label ID="txtErrorMsg" runat="server" CssClass="errMsgOn"></asp:Label>
        
        <asp:Panel ID= "PnlMain" runat="server" ScrollBars="Auto" Height="733px" 
            Width="800px">
            <asp:GridView ID="grdHistory" runat="server" AutoGenerateColumns="False" 
                onrowdatabound="grdHistory_RowDataBound" 
                onrowcommand="grdHistory_RowCommand" onrowcreated="grdHistory_RowCreated" 
                CssClass="ctrlGrid" AllowPaging="True"  BorderColor="#990000" BorderStyle="Solid" BorderWidth="1px" 
                CellPadding="3" CellSpacing="2"
                onpageindexchanging="grdHistory_PageIndexChanging" PageSize="25" 
                ShowFooter="True">
                <Columns>
                    <asp:TemplateField HeaderText="Activity Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("ActivityDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Activity">
                        <ItemTemplate>
                            <asp:Label ID="lblActivity" runat="server" Text='<%# Bind("Activity") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remarks">
                        <ItemTemplate>
                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Download">
                        <ItemTemplate>
                           
                           
                            <asp:LinkButton ID="btnDownload" runat="server"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                            CommandName="Download" Text='<%# Bind("Name") %>'>Download</asp:LinkButton>
                       </ItemTemplate>
                   </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MiMeType">
                        <ItemTemplate>
                            <asp:Label ID="lblMimeType" runat="server" Text='<%# Bind("MiMeType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    
                </Columns>
                        <RowStyle BackColor="White" ForeColor="Blue"  HorizontalAlign="Center" 
                            Font-Names="Verdana" Font-Size="Small"/>
                        <FooterStyle BackColor="#990000" ForeColor="White"  HorizontalAlign="Center" 
                            Font-Names="Verdana" Font-Size="Small" />
                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" 
                            Font-Names="Verdana" Font-Size="Small" />
                        <SelectedRowStyle BackColor="White" ForeColor="#990000" 
                            HorizontalAlign="Center" Font-Names="Verdana" Font-Size="Small" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" 
                            HorizontalAlign="Center" Font-Names="Verdana" Font-Size="Small" />
                        <AlternatingRowStyle BackColor="White" ForeColor="Blue" 
                            HorizontalAlign="Center" Font-Names="Verdana" Font-Size="Small" />
                    </asp:GridView>
        </asp:Panel>
        
    </div>
    </form>
</body>
</html>
