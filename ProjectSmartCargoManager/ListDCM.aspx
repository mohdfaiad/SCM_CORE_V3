<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="ListDCM.aspx.cs" Inherits="ProjectSmartCargoManager.ListDCM" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
    <style type="text/css">
     
    </style>
    
    <script language="javascript" type="text/javascript">
        function printDCMList() {
            var hfInvNos = document.getElementById("<%= hfDCMNo.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowDCMPrint.aspx?DCMNO=" + invArr[i] + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function printDCMperAWBList() {
            var hfInvNos = document.getElementById("<%= hfAWBNo.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowDCMperAWBPrint.aspx?AWBNO=" + invArr[i] + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }
    </script>
    
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            //document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }
     </script>
    
    <style type="text/css">
        .black_overlay
        {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 1000px;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: 0.8;
            filter: alpha(opacity=80);
        }
        .white_content
        {
            margin: 0 auto;
            display: none;
            position: absolute;
            top: 30%;
            left: 35%;
            width: 30%;
            height: 45%;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
        .black_overlaymsg
        {
            display: none;
            position: fixed;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: White;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }
        .white_contentmsg
        {
            display: none;
            position: fixed;
            top: 45%;
            left: 45%;
            width: 5%;
            height: 5%;
            padding: 16px;
            background-color: Transparent;
            z-index: 1002;
        }
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
    
    <div id="contentarea">
    
                      <h1> 
                      <img alt="" src="Images/debitcredit.png" id="DCM" /></h1>
    
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red" Visible="false"></asp:Label>
       
    <div class="botline">
               <table width="100%" cellpadding="5px">
                 <tr>
                     <td>
                         <asp:Label ID="lblDCM" runat="server" Text="DCM Number" ></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox ID="txtDCM" runat="server" Width="140px"></asp:TextBox>  
                     </td>
                     <td>
                        <asp:Label ID="lblAwb" runat="server" Text="AWB Number"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="25px" ID="txtPreAWB" runat="server" ReadOnly = "true" 
                            ></asp:TextBox>&nbsp;
                         <asp:TextBox Width="100px" ID="txtAWB" runat="server"  
                            ></asp:TextBox></td>
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
                    <td>
                         <asp:Label ID="lblDCMFrom" runat="server" Text="DCM From Dt"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="80px" ID="txtDCMFrom" runat="server"></asp:TextBox>
                         <asp:CalendarExtender ID="txtDCMFrom_CalendarExtender" Format="dd-MM-yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtDCMFrom">
                         </asp:CalendarExtender>
                     </td>
                     
                     <td>
                        <asp:Label ID="lblAgentTo" runat="server" Text="DCM To Dt"></asp:Label>
                     </td>
                     <td> 
                         <asp:TextBox Width="80px" ID="txtDCMTo" runat="server"></asp:TextBox>
                         <asp:CalendarExtender ID="txtDCMTo_CalendarExtender"  Format="dd-MM-yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtDCMTo">
                         </asp:CalendarExtender>
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
                     <td><asp:Button ID="btnList" runat="server" CssClass="button" 
                             onclick="btnList_Click" Text="List" /></td>
                     <td>
                        <asp:Button ID="btnClear" runat="server" CssClass="button" 
                             onclick="btnClear_Click" Text="Clear" />
                     </td>
                     
                 </tr>
             </table>
         
           </div> 
  
   <div class="divback" style="overflow:auto"> 
         <asp:GridView ID="GrdDCMDetails" runat="server" AutoGenerateColumns="False" 
            ShowFooter="True"   Width="80%" AllowPaging="False" 
             onpageindexchanging="GrdDCMDetails_PageIndexChanging" PageSize="20" >
            <Columns>
                
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                             
                <asp:TemplateField HeaderText="DCM No." HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblDCMNo" runat="server" Text='<%# Eval("DCMNumber") %>'></asp:Label>
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
                        <asp:Button ID="btnPrintDCM" Visible="false" runat="server" 
                            Text="Print DCM Summary" CssClass="button" onclick="btnPrintDCM_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnPrintAWBDCM" Visible="false" runat="server" 
                            Text="Print DCM per AWB" CssClass="button" onclick="btnPrintAWBDCM_Click" />
                    </td>
                    <td><asp:HiddenField ID="hfDCMNo" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hfAWBNo" runat="server" Value="" /></td>
                </tr>
            </table>
            </div>
      
   </div>
   
    <div id="msglight" class="white_contentmsg">
        <table>
            <tr>
                <td width="5%" align="center">
                    <br />
                    <img src="Images/loading.gif" />
                    <br />
                    <asp:Label ID="msgshow" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    
    <div id="msgfade" class="black_overlaymsg">
    </div>
    
    </ContentTemplate>
    </asp:UpdatePanel>
    
   </asp:Content>
