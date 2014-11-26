<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRejectionMemo.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmRejectionMemo" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
        function show_alert() {
            var msg = "RM Generated";
            alert(msg);
        }
        function show_alert1() {
            var msg = "RM Generation failed";
            alert(msg);
        }
        function show_alert2() {
            var msg = "Current & Revised Are Identical";
            alert(msg);
        }
</script>
    
  <%-- <asp:TextBox Width="100px" ID="txtRMNumber" runat="server"></asp:TextBox>--%>  
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ScriptMgr" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
                 
                  <h1> 
                      <img alt="" src="Images/txt_rejectionmemo.png" id="RM" /></h1>
         &nbsp;<div class="botline">
           <table width="100%" cellpadding="5px">
                 <tr>
                    <td>
                        <%-- <asp:TextBox Width="100px" ID="txtRMNumber" runat="server"></asp:TextBox>--%>
                         <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Blue"></asp:Label>
                     </td>
                     </tr>
                     <tr>
                     <td>
                         <asp:Label ID="lblRMNumber" runat="server" Text="Rejection Memo Number:" Visible="false"></asp:Label>&nbsp;&nbsp;
                         <asp:Label Width="200px" ID="txtRMNumber" Font-Bold="true" runat="server"></asp:Label>
                     </td>
                     <td>
                         <asp:Label ID="lblRMAmount" runat="server" Text="Rejected Amount:" Visible="false"></asp:Label>&nbsp;&nbsp;
                         <asp:Label Width="100px" ID="txtRMAmount" Font-Bold="true" runat="server"></asp:Label>&nbsp;&nbsp;
                         <asp:Label Width="100px" ID="txtRMType" Font-Bold="True" runat="server"></asp:Label>
                     </td>
                 </tr>
             </table>
           
         
         </div>
         <div class="botline"> 
         
               <table width="100%" cellpadding="5px">
                 <tr>
                     <td>
                         <asp:Label ID="lblAwbInv" runat="server" Text="AWB"></asp:Label>
                     </td>
                     <td>
                       <asp:TextBox ID="txtPreAWB" runat="server"  
                              Width="30px"></asp:TextBox>
                         <asp:TextBox Width="100px" ID="txtAWB" runat="server"  
                            ></asp:TextBox>
                     </td>
                     <td>
                         <asp:Label ID="lblInvoice" runat="server" Text="Invoice No."></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="100px" ID="txtInvoiceNo" runat="server" ></asp:TextBox>
                     </td>
                     <td>
                         From Date:</td>
                     <td>
                          <asp:TextBox Width="80px" ID="txtCCAFrom" runat="server"></asp:TextBox>
                          <asp:Image ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" 
                              ImageUrl="~/Images/calendar_2.png" />
                         <asp:CalendarExtender ID="txtCCAFrom_CalendarExtender" Format="dd-MM-yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtCCAFrom">
                         </asp:CalendarExtender>
                     </td>
                     <td>
                         To Date:</td>
                     <td>
                          <asp:TextBox Width="80px" ID="txtCCATo" runat="server"></asp:TextBox>
                         <asp:CalendarExtender ID="txtCCATo_CalendarExtender"  Format="dd-MM-yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtCCATo">
                         </asp:CalendarExtender>
                                                 <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" 
                              ImageUrl="~/Images/calendar_2.png" />
                         
                     </td>
                     <td>
                         <asp:CheckBox ID="chkInterline" Text="Interline" runat="server" />
                     </td>
                     <td>
                        
                        <asp:Button ID="btnList" runat="server" CssClass="button" 
                             onclick="btnList_Click" Text="List" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" 
                             onclick="btnClear_Click" />
                        
                     </td>
                 </tr>
             </table>
         
         </div> 
         <div class="divback" style="overflow:auto"> 
         <asp:GridView ID="GrdCCADetails" runat="server" AutoGenerateColumns="False" 
            ShowFooter="True"   Width="80%" AllowPaging="False" 
              PageSize="20" 
               >
            <Columns>
                
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                             
                <asp:TemplateField HeaderText="CCA No." HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblCCANo" runat="server" Text='<%# Eval("CCANumber") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
               <asp:TemplateField HeaderText="AWB No." HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAWBNumber" runat="server" Text='<%# Eval("AWBNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Invoice No." HeaderStyle-Wrap="false">
               <ItemTemplate>
                        <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("InvoiceNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Agent Code">
                     <ItemTemplate>
                         <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("AgentCode") %>' 
                            Width="80px">
                        </asp:Label>
                     </ItemTemplate>
                     <HeaderStyle Wrap="True" />
                     <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="CCA Date">
                  <ItemTemplate>
                        <asp:Label  ID="lblFromDate" runat="server" Text='<%# Eval("CCADate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                 </asp:TemplateField>
                 
                   <asp:TemplateField HeaderText="Status">
                  <ItemTemplate>
                        <asp:Label  ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                 </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Current Gross Wt" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblCurrentGrossWt" runat="server" Text='<%# Eval("GrossWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Current Ch Wt" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblCurrentChWt" runat="server" Text='<%# Eval("ChargableWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
   
               <asp:TemplateField HeaderText="Current Freight" HeaderStyle-Wrap="false" >
                    <ItemTemplate>
                        <asp:Label  ID="lblCurrentFreight" runat="server" Text='<%# Eval("FreightRate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
                 <asp:TemplateField HeaderText="Current OCDC" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblCurrentOCDC" runat="server" Text='<%# Eval("OCDC") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Current OCDA" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblCurrentOCDA" runat="server" Text='<%# Eval("OCDA") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
              <asp:TemplateField HeaderText="Current ST" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblCurrentTax" runat="server" Text='<%# Eval("ServiceTax") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
              <asp:TemplateField HeaderText="Current Total" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblCurrentTotal" runat="server" Text='<%# Eval("CurrentTotal") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Revised Gross Wt" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblRevisedGrossWt" runat="server" Text='<%# Eval("RevisedGrossWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Revised Ch Wt" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblRevisedChargableWt" runat="server" Text='<%# Eval("RevisedChargableWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                               
                <asp:TemplateField HeaderText="Revised Freight" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblRevisedFreight" runat="server" Text='<%# Eval("RevisedFreightRate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Revised OCDC" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblRevisedOCDC" runat="server" Text='<%# Eval("RevisedOCDC") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField> 
                
                <asp:TemplateField HeaderText="Revised OCDA" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblRevisedOCDA" runat="server" Text='<%# Eval("RevisedOCDA") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>      
                         
                          <asp:TemplateField HeaderText="Revised ST" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblRevisedST" runat="server" Text='<%# Eval("RevisedServiceTax") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>   
                
                <asp:TemplateField HeaderText="Revised Total" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblRevisedTotal" runat="server" Text='<%# Eval("RevisedTotal") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>  
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblSerialNo" runat="server" Text='<%# Eval("SerialNumber") %>' Visible="false">
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
           </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>

 
 </div>
             
       <asp:Button ID="btnGenerateRM" Text="Generate Rejection Memo" runat="server" Visible="false" 
            CssClass="button" onclick="btnGenerateRM_Click" />
           
            <asp:Button ID="btnRejectionMemo" Text="Print Rejection Memo" runat="server" Visible="false" 
            CssClass="button" onclick="btnRejectionMemo_Click" />
                       
            <asp:Button ID="btnRejectionMemoPerAWB" Text="Print Rejection Memo Per AWB" 
                      runat="server" Visible="false" 
            CssClass="button" onclick="btnRejectionMemoPerAWB_Click" />
        
        </div> 
                    
</asp:Content>