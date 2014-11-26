<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentReceipt.aspx.cs"  MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.PaymentReceipt" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script language="javascript" type="text/javascript">

    function SelectAllgrdAddRate(CheckBoxControl)
     {
         for (i = 0; i < document.forms[0].elements.length; i++) 
        {
            if (document.forms[0].elements[i].name.indexOf('check') > -1) 
            {
                document.forms[0].elements[i].checked = CheckBoxControl.checked;
            }
        }
    }
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
   <div id="contentarea">
   
    <h1> 
     Payment Receipt
     </h1>
     <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
        </p>
    <div class="botline">
     <asp:Panel ID="Panel4" runat="server" EnableViewState="true">
       <table width="60%">
    <tr>
    <td width="198px">
      Payment Advice Number
        *</td>
    <td width="150px" >
       
        <asp:TextBox ID="txtpaymentadvice" runat="server" Width="120px" ></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtpaymentadvice" ErrorMessage="*"></asp:RequiredFieldValidator>
    </td>
    <td width="50px" >
        <asp:Button ID="btnList" runat="server" Text="List" Width="50px" CssClass="button"/>
    </td>
    <td>
        <asp:Button ID="BtnClear" runat="server" Text="Clear" Width="50px" 
            CssClass="button" onclick="BtnClear_Click" />
    
    </td>
    </tr>
    </table>
     </asp:Panel>
      </div>
        <br />
    Service Details
          
    </div>
    
    <div class="botline">
        <asp:Panel ID="Panel1" runat="server" EnableViewState="true">
        
     <table width="60%" enableviewstate="true">
     <tr>
     <td >
      Service Code*
     </td>
     <td width="150px" >
         <asp:TextBox ID="txtServiceCode" runat="server"></asp:TextBox>
     </td>
     <td>
         <asp:Button ID="btnListAttribute" runat="server" Text="List Attribute" CssClass="button"/>
     </td>
     </tr>
     </table>
     <table width="40%" >
     <tr>
     <td>
     
      <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" 
                          ID="grdDeliveryDetails"                                    
                                      Width="85%" CssClass="grdrowfont"> 
                                  <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField>
                        
                    <ItemTemplate>
                        <asp:CheckBox ID="chk" runat="server" />
                    </ItemTemplate>
                    </asp:TemplateField>                    
                    
                        <asp:TemplateField HeaderText="Attribute Name">
                        <ItemTemplate>
                            <asp:Label ID="lblattributename" runat="server"   
                            Width="110px" CssClass="grdrowfont">
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>
                        
                         <asp:TemplateField HeaderText="Value">
                        <ItemTemplate>
                            <asp:Label ID="lblvalue" runat="server"   
                            Width="110px" CssClass="grdrowfont">
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>
                            
                     </Columns>
                      <EditRowStyle CssClass="grdrowfont"></EditRowStyle>

                        <FooterStyle CssClass="grdrowfont"></FooterStyle>

                        <HeaderStyle CssClass="titlecolr"></HeaderStyle>

                        <RowStyle CssClass="grdrowfont"></RowStyle>
                </asp:GridView>
     </td>
       </table>
     
   <table> 
    
    <tr>
    <td>
        <asp:Button ID="btnAddService" runat="server" Text="Add Service" CssClass="button"/>
    </td>
    <td>
     <asp:Button ID="btnDeleteService" runat="server" Text="Delete Service" CssClass="button"/>
    </td>
    </tr>
    </table>
  
     
     <tr>
     <td>
         <asp:Button ID="btnCharges" runat="server" Text="Calculate Charges" CssClass="button" />
     </td>
     </tr>
     </table>
       </asp:Panel>
      <asp:Panel ID="Panel2" runat="server" EnableViewState="true" >
      <table width="100%">
     <tr>
      <td>
        <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" 
                          ID="GrdpaaymentReceipt"                                    
                                      Width="100%" CssClass="grdrowfont" EnableViewState="true" > 
                                  <%--<AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>--%>
                        <Columns>
                                          
                     <asp:TemplateField>
                         <HeaderTemplate>
                       <asp:CheckBox ID="chk" runat="server" onclick="javascript:SelectAllgrdAddRate(this);"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="check" runat="server" />
                    </ItemTemplate>
                    </asp:TemplateField>  
                    
                    
                        <asp:TemplateField HeaderText="Service Name">
                        <ItemTemplate>
                            <asp:Label ID="lblServiceName" runat="server" Text="" 
                            Width="110px" CssClass="grdrowfont">
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>    
                          
                        <asp:TemplateField HeaderText="Charge Name"><ItemTemplate>
                            <asp:Label ID="lblchargename" runat="server" Text=""
                            Width="80px" CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        
                       
                        <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblamount" runat="server" Text="" Width="55px">
                            </asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Waive">
                        <ItemTemplate>
                                       <asp:Label ID="lblwaive" runat="server" Text=""  Width="65px">
                                        </asp:Label>
                        <%--<asp:CheckBox ID="chkwaivei" runat="server" />--%>
                        </ItemTemplate>
                        </asp:TemplateField>
                       
                        
                        <asp:TemplateField HeaderText="GST">
                        <ItemTemplate>
                            <asp:Label ID="lblgst" runat="server" Text=""  Width="60px">
                            </asp:Label>                
                        </ItemTemplate>
                        </asp:TemplateField>
                        
                       <asp:TemplateField HeaderText="Net Amount"><ItemTemplate>
                            <asp:Label  ID="lblnetamount" runat="server" Width="100px" Text="" EnableViewState="true"   >
                            </asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>
                        
                        
                        <asp:TemplateField HeaderText="Currency">
                        <ItemTemplate>
                            <asp:Label ID="lblcurrency" runat="server" Width="100px" Text="">
                            </asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>
                                               
                         <asp:TemplateField HeaderText="Remark">
                         <ItemTemplate>
                            <asp:TextBox ID="txtremark" runat="server" Width="180px" Height="30px"  EnableViewState="true" TextMode="MultiLine">
                            </asp:TextBox >
                        </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>

                        <EditRowStyle CssClass="grdrowfont"></EditRowStyle>

                        <FooterStyle CssClass="grdrowfont"></FooterStyle>

                        <HeaderStyle CssClass="titlecolr"></HeaderStyle>

                        <RowStyle CssClass="grdrowfont"></RowStyle>
                    </asp:GridView>
      </td>
     </tr>
     </table> 
     </asp:Panel>
     <div>
         <asp:Panel ID="Panel3" runat="server" EnableViewState="true">
         
      <table width="100%">
       <tr>
       <td  >
        Total
       </td>
       <td>
           <asp:TextBox ID="txttotal" runat="server" ToolTip="Enter Total" ></asp:TextBox>
       </td>
        <td>
       Final Amount
       </td>
       <td>
           <asp:TextBox ID="txtfinalAmt" runat="server" ToolTip="Final Amount" ></asp:TextBox>
       </td>
       <td >
       </td> 
       </tr>
       <tr>
       <td>
       GST On Total
       </td>
       <td>
           <asp:TextBox ID="txtGSTTotal" runat="server" ToolTip="GST on Total" ></asp:TextBox>
       </td>
        <td>
       Balance Given
       </td>
       <td>
           <asp:TextBox ID="txtbalancegiven" runat="server" ToolTip="Enter Balance Given" ></asp:TextBox>
       </td>
       <td >
       
       </td>
       </tr>
       <tr>
       <td>
        TDS
       </td>
       <td >
           <asp:TextBox ID="txttds" runat="server" ToolTip="Enter TDS" ></asp:TextBox>
       </td>
      <%-- <td >
           
       </td>--%>
       <td>
       <asp:CheckBox ID="chkwaive" runat="server"  Text="Waive" TextAlign="Left" />
       </td>
       </tr>
       <tr>
       <td>
       Grand Total
       </td>
       <td>
           <asp:TextBox ID="txtgrandtotal" runat="server" ToolTip="Grand Total" ></asp:TextBox>
       </td>
       <td>
       Rounded off Amt.
       </td>
       <td>
           <asp:TextBox ID="txtrondedamt" runat="server" ToolTip="Rounded off Amount" ></asp:TextBox>
       </td>
       </tr>
       <tr>
       <td>
       Customer Code*
       </td>
       <td>
           <asp:TextBox ID="txtCustomerCode" runat="server" ToolTip="Enter Customer Code" > </asp:TextBox>
       </td>
       <td>
          Customer Name
       </td>
       <td>
           <asp:TextBox ID="txtcustomername" runat="server" ToolTip="Please Enter Customer Name" ></asp:TextBox>
       </td>
      <td></td>
       </tr>
       <tr>
      <td>
      Payment Details
      </td>
      <td>
          <asp:TextBox ID="txtpaymentdeetail" runat="server" ToolTip="Enter Payment Details" ></asp:TextBox>
      </td>
       <td>
       Remarks
       </td>
       <td >
           <asp:TextBox ID="txtremark" runat="server" TextMode="MultiLine" Height="28px"  ToolTip="Please Enter Remark" 
               Width="160px"  ></asp:TextBox>
       </td>
      <td></td> 
       </tr>
       
       
      </table> 
      </asp:Panel>
      <table width="100%" enableviewstate="true" >
      <tr>
      <td>
          <asp:Button ID="btnprintrecipt" runat="server" Text="Print Receipt" CssClass="button"/>
     
           &nbsp;<asp:Button ID="btnaddchargecode" runat="server" Text="Add Charge Code" CssClass="button" />
     
          &nbsp;<asp:Button ID="BtnSave" runat="server" Text="Save Provisional" 
              CssClass="button" onclick="BtnSave_Click"/>
     
          &nbsp;<asp:Button ID="btnpaymetDetails" runat="server" Text="Payment Details" CssClass="button"/>
     
          &nbsp;<asp:Button ID="btnAcceptpayment" runat="server" Text="Accept Payment" 
              CssClass="button"/>
     
          &nbsp;<asp:Button ID="btnclose" runat="server" Text="Close" CssClass="button" 
              onclick="btnclose_Click"/>
      </td>
      </tr>
      </table>  
     </div>  
    </div>
   
   
  
    
    
</asp:Content>