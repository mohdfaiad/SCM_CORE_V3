<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="ListCCA.aspx.cs" Inherits="ProjectSmartCargoManager.ListCCA" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
    <style type="text/css">
     
    </style>
    
    <script language="javascript" type="text/javascript">
        function printCCAList() {
            var hfInvNos = document.getElementById("<%= hfCCANo.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCCAPrint.aspx?CCANO=" + invArr[i] + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function printCCAperAWBList() {
            var hfInvNos = document.getElementById("<%= hfAWBNo.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCCAperAWBPrint.aspx?AWBFlightNO=" + invArr[i] + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
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
     <script type="text/javascript">
         function SelectheaderCheckboxes(headerchk) {
             var gvcheck = document.getElementById("<%=GrdCCADetails.ClientID %>");
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
                      Charges Correction Advise (CCA)
                      </h1>
    
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red" Visible="false"></asp:Label>
       
    <div class="botline">
               <table width="100%" cellpadding="5px">
                 <tr>
                     <td>
                         <asp:Label ID="lblCCAFrom" runat="server" Text="CCA Dt"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="80px" ID="txtCCAFrom" runat="server"></asp:TextBox>
                         <asp:CalendarExtender ID="txtCCAFrom_CalendarExtender" Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtCCAFrom" PopupButtonID="btnFromDate" PopupPosition="BottomLeft">
                         </asp:CalendarExtender>
                        <asp:ImageButton ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" 
                              ImageUrl="~/Images/calendar_2.png"  />
                         &nbsp;
                         <asp:TextBox Width="80px" ID="txtCCATo" runat="server"></asp:TextBox>
                         <asp:CalendarExtender ID="txtCCATo_CalendarExtender"  Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtCCATo" PopupButtonID="Image1" PopupPosition="BottomLeft">
                         </asp:CalendarExtender>
                                                   <asp:ImageButton ID="Image1" runat="server" ImageAlign="AbsMiddle" 
                              ImageUrl="~/Images/calendar_2.png" />
                         
                     </td>
                     <td>
                         <asp:Label ID="lblCCA" runat="server" Text="CCA Number" ></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox ID="txtCCA" runat="server" Width="140px"></asp:TextBox>  
                     </td>
                     <td><asp:Label ID="lblCCAType" runat="server" Text="CCA Type"></asp:Label>
                     </td>
                     <td>
                         <asp:DropDownList ID="ddlCCAType" runat="server" Width="90px">
                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                            <asp:ListItem Text="Debit" Value="Debit"></asp:ListItem>
                            <asp:ListItem Text="Credit" Value="Credit"></asp:ListItem>
                         </asp:DropDownList>
                     </td>
                    <td>Status</td>
                     <td>
                         <asp:DropDownList ID="ddlStatus" runat="server">
                         <asp:ListItem>All</asp:ListItem>
                         <asp:ListItem>Pending</asp:ListItem>
                         <asp:ListItem>Accepted</asp:ListItem>
                         <asp:ListItem>Rejected</asp:ListItem>
                         </asp:DropDownList>
                     </td>
                 </tr>
                 <tr>
                    <td>
                         <asp:Label ID="lblAgent" runat="server" Text="Agent Code"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="100px" ID="txtAgent" runat="server" ></asp:TextBox>
                     </td>
                     <td>
                        <asp:Label ID="lblAwb" runat="server" Text="AWB Number"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="30px" ID="txtPreAWB" runat="server" ReadOnly = "false" 
                          MaxLength="3" ></asp:TextBox>
                          <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" 
                            runat="server" TargetControlID="txtPreAWB" WatermarkText="Prefix">
                        </asp:TextBoxWatermarkExtender> 
                          &nbsp;
                         <asp:TextBox Width="90px" ID="txtAWB" runat="server"  
                            ></asp:TextBox></td>
                     <td>
                         <asp:Label ID="lblInvoice" runat="server" Text="Invoice #"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox Width="140px" ID="txtInvoiceNo" runat="server" ></asp:TextBox>
                     </td>    
             </tr>
             <tr>
                     <td>
                         <asp:Button ID="btnList" runat="server" CssClass="button" 
                             onclick="btnList_Click" Text="List" />
                            &nbsp;
                         <asp:Button ID="btnClear" runat="server" CssClass="button" 
                             onclick="btnClear_Click" Text="Clear" />
                     </td>
                     
                 </tr>
             </table>
         
           </div> 
  
   <div style="overflow:auto; float:left; width:1024px;"> 
         <asp:GridView ID="GrdCCADetails" runat="server" AutoGenerateColumns="False" 
            ShowFooter="True"   Width="80%" 
             onpageindexchanging="GrdCCADetails_PageIndexChanging" 
             onrowcommand="GrdCCADetails_RowCommand" 
             onrowediting="GrdCCADetails_RowEditing" 
             onselectedindexchanged="GrdCCADetails_SelectedIndexChanged" >
            <Columns>
                
                <asp:TemplateField>
                <HeaderTemplate>
                       <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);"/>
                   </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                             
                <asp:TemplateField HeaderText="CCA No." HeaderStyle-Wrap="true">
                    <ItemTemplate>
              <asp:LinkButton ID="lnkCCANo" runat="server" Text='<%# Eval("CCANumber") %>' CommandName="CCADetails" CommandArgument='<%# Eval("AWBPrefix")+""+Eval("AWBNumber") + "," + Eval("CCANumber") + "," + Eval("FlightNo") + "," + Eval("FDate") %>'></asp:LinkButton>
          
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
               <asp:TemplateField HeaderText="AWB No." HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAWBPrefix" runat="server" Text='<%# Eval("AWBPrefix") %>'></asp:Label>
                        <asp:Label ID="Label1" runat="server" Text="-"></asp:Label>
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
                
                 <asp:TemplateField HeaderText="Flight No" HeaderStyle-Wrap="false">
               <ItemTemplate>
                        <asp:Label ID="lblFlightNo" runat="server" Text='<%# Eval("FlightNo") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Flight Date" HeaderStyle-Wrap="false">
               <ItemTemplate>
                        <asp:Label ID="lblFlightDate" runat="server" Text='<%# Eval("FlightDate") %>'></asp:Label>
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
                        <asp:Button ID="btnPrintCCA" Visible="false" runat="server" 
                            Text="Print CCA Summary" CssClass="button" onclick="btnPrintCCA_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnPrintAWBCCA" Visible="false" runat="server" 
                            Text="Print CCA per AWB" CssClass="button" onclick="btnPrintAWBCCA_Click" />
                        <asp:Button ID="btnAccept" runat="server" CssClass="button" 
                            Text="Accept" Visible="false" onclick="btnAccept_Click" />
                        <asp:Button ID="btnReject" Visible="false" runat="server" CssClass="button" 
                             Text="Reject" onclick="btnReject_Click" />
                    </td>
                    <td><asp:HiddenField ID="hfCCANo" runat="server" Value="" /></td>
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
