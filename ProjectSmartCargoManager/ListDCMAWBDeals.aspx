<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListDCMAWBDeals.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.ListDCMAWBDeals" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
     
    </style>
    <script language="javascript" type="text/javascript">

        function printDCMperAWBList() {
            var hfInvNos = document.getElementById("<%= hfAWBNo.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowDCMperAWBPrint.aspx?AWBFlightNO=" + invArr[i] + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function printDCMDealsList() {
            var hfInvNos = document.getElementById("<%= hfAWBNo.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowDCMAWBDealsPrint.aspx?InvoiceNo=" + invArr[i] + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }
        
    </script>   
    <script type="text/javascript">
        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=GrdDCMDetails.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
            return (false);
        }
    </script> 
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  <div id="contentarea">
    
  <h1> 
  Debit Credit Memo (DCM)
  </h1>
     <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red" Visible="false"></asp:Label>    
    </div>   
    <div class="botline">
               <table width="80%" cellpadding="5px">
                 <tr>
                    <td colspan="2">
                        <asp:RadioButton ID="rbDCMAWB" runat="server" Text="DCM against AWB" 
                            GroupName="DCMType" AutoPostBack="True" Checked="True" 
                            oncheckedchanged="rbDCMAWB_CheckedChanged"  />
                            &nbsp;
                            <asp:RadioButton ID="rbDCMDeals" runat="server" Text="DCM against Deals/PLI" 
                             GroupName="DCMType" AutoPostBack="True" 
                             oncheckedchanged="rbDCMDeals_CheckedChanged" />
                     </td>
                  </tr>
                 <tr>
                     <td>
                         <asp:Label ID="lblDCMFrom" runat="server" Text="DCM Dt *"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="80px" ID="txtDCMFrom" runat="server"></asp:TextBox>
                         <asp:CalendarExtender ID="txtDCMFrom_CalendarExtender"  Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtDCMFrom" PopupButtonID="btnFromDate" PopupPosition="BottomLeft">
                         </asp:CalendarExtender>
                                                   <asp:ImageButton ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" 
                              ImageUrl="~/Images/calendar_2.png" />
                         &nbsp;
                         <asp:TextBox Width="80px" ID="txtDCMTo" runat="server"></asp:TextBox>
                         <asp:CalendarExtender ID="txtDCMTo_CalendarExtender"  Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtDCMTo" PopupButtonID="Image1" PopupPosition="BottomLeft">
                         </asp:CalendarExtender>
                                                  <asp:ImageButton ID="Image1" runat="server" ImageAlign="AbsMiddle" 
                              ImageUrl="~/Images/calendar_2.png" />
                     </td>
                     <td>
                         <asp:Label ID="lblDCM" runat="server" Text="DCM Number" ></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox ID="txtDCM" runat="server" Width="140px"></asp:TextBox>  
                     </td>
                     <td><asp:Label ID="lblDCMType" runat="server" Text="DCM Type"></asp:Label>
                     </td>
                     <td>
                         <asp:DropDownList ID="ddlDCMType" runat="server" Width="90px">
                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                            <asp:ListItem Text="Debit" Value="Debit"></asp:ListItem>
                            <asp:ListItem Text="Credit" Value="Credit"></asp:ListItem>
                         </asp:DropDownList>
                     </td>
                     
                 </tr>
                 <tr>
                     <td>
                        <asp:Label ID="lblAwb" runat="server" Text="AWB #"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="40px" ID="txtPreAWB" runat="server" 
                          MaxLength="3"></asp:TextBox>
                          <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" 
                            runat="server" TargetControlID="txtPreAWB" WatermarkText="Prefix">
                        </asp:TextBoxWatermarkExtender>
                          &nbsp;
                         <asp:TextBox Width="90px" ID="txtAWB" runat="server" MaxLength="10" ></asp:TextBox></td>
                     <td>
                         <asp:Label ID="lblInvoice" runat="server" Text="Invoice Number"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="140px" ID="txtInvoiceNo" runat="server" ></asp:TextBox>
                     </td>                    
                     <td>
                         <asp:Label ID="lblAgent" runat="server" Text="Agent Code"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="100px" ID="txtAgent" runat="server" ></asp:TextBox>
                     </td>
                     
                 </tr>
                 <tr>
                 <td><asp:Button ID="btnList" runat="server" CssClass="button" 
                             onclick="btnList_Click" Text="List" />
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" CssClass="button" 
                             onclick="btnClear_Click" Text="Clear" />
                     </td>
                 </tr>
             </table>
         
           </div> 
  
   <div style="overflow:auto; float:left; width:1024px;"> 
         <asp:GridView ID="GrdDCMDetails" runat="server" AutoGenerateColumns="False" 
            ShowFooter="True"   Width="80%" 
             onpageindexchanging="GrdDCMDetails_PageIndexChanging" PageSize="20" 
             onrowediting="GrdDCMDetails_RowEditing" 
             onselectedindexchanged="GrdDCMDetails_SelectedIndexChanged" >
            <Columns>

                <asp:TemplateField>
                 <HeaderTemplate>
                       <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);"/>
                   </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                             
                <asp:TemplateField HeaderText="DCM #" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblDCMNo" runat="server" Text='<%# Eval("DCMNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AWB #" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAWBPrefix" runat="server" Text='<%# Eval("AWBPrefix") %>'></asp:Label>
                        <asp:Label ID="Label1" runat="server" Text="-"></asp:Label>
                        <asp:Label ID="lblAWBNumber" runat="server" Text='<%# Eval("AWBNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
              <%-- <asp:TemplateField HeaderText="Revised Flight #" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblFlightNumber" runat="server" Text='<%# Eval("FlightNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>--%>
                
                <asp:TemplateField HeaderText="Invoice #" HeaderStyle-Wrap="false">
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
                
                 <asp:TemplateField HeaderText="DCM Date">
                  <ItemTemplate>
                        <asp:Label  ID="lblFromDate" runat="server" Text='<%# Eval("DCMDate") %>'></asp:Label>
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
                <asp:CommandField SelectText="View" ShowSelectButton="True" />
                <asp:CommandField ShowEditButton="True" />
           </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>

 
 </div> 
  <div id="fotbut">
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnPrintAWBDCM" Visible="false" runat="server" 
                            Text="Print DCM" CssClass="button" 
                            onclick="btnPrintAWBDCM_Click" />
                        <asp:Button ID="btnExportToERP" runat="server" CssClass="button" 
                            Text="ExportToERP" onclick="btnExportToERP_Click"  Visible="false" />
                    </td>
                    <td><asp:HiddenField ID="hfDCMNo" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hfAWBNo" runat="server" Value="" /></td>
                </tr>
            </table>
            </div>
      
   </div>    
   </asp:Content>
