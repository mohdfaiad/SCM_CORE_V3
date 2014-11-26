<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreditMaster.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.CreditMaster" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
     function InsertFailure() {

         alert("Insertion Failed Please Try Again");

     }
     function NumericValidation() {

         alert("Please Insesrt Numeric Value");

     }
     function selectrow() {

         alert("Please Select a Row");

     }
     function Insert() {

         alert("Record Inserted Succssfully");

     }
     function SelectRow() {

         alert("Please Select Row for Save");

     }

     function Check() {

         alert("Invoice Value Does Not Greater Than Credit Value");

     }
   
 </script>
    
    <style type="text/css">
        .style2
        {
            width: 172px;
        }
    </style>
    
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
    <div id="contentarea">
                       
                  <h1> 
                      <img alt="" src="images/CreditMaster.png" /></h1>
   
  <div class="botline">
        <table width="55%" border="0">
            
           <tr>
           <td>
           Agent Name*
               .</td>
           <td>
           <asp:DropDownList ID="ddlagentcode" runat="server" Width="140px" >
               <asp:ListItem Selected="True">Select</asp:ListItem>
                 </asp:DropDownList>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                   ControlToValidate="ddlagentcode" ErrorMessage="RequiredFieldValidator" 
                   Font-Bold="True" Font-Italic="False">*</asp:RequiredFieldValidator>
           </td>
           <td  >
             <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
                   onclick="btnList_Click" />
             &nbsp;<asp:Button ID="btntextclear" runat="server" Text="Clear" CssClass="button" onclick="btntextclear_Click" 
                       />
             </td>
               <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbankguranteeamt"  ErrorMessage="Please Enter Numeric Value"  ValidationExpression="^\d+$" ValidationGroup="check">
                        </asp:RegularExpressionValidator>--%>
            </tr>
               
           </table>
</div>
  <table style="height:12px"  > 
    <tr>
    <td>
    </td>
    <td></td><td style="width:100px"></td><td style="width:100px"></td><td></td><td></td><td style="width:100px"></td><td></td><td></td>
    <td colspan="5"  align="center" >
        <asp:Button ID="btnaddroow" runat="server" Text="Add Row" CssClass="button" 
            onclick="btnaddroow_Click"  />
    </td>
    </tr>
    
 </table>
  <div class="divback" style="width:900px; height:204px;"> 
<asp:GridView ID="grdCreditinfo" runat="server" AutoGenerateColumns="False" 
                                    ShowFooter="True"   Width="100%" Height="80px" >
            <Columns>
               
                <asp:TemplateField HeaderText="Bank Name" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlbankname" runat="server">
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="txtbankname" runat="server"></asp:TextBox>--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Bank Gurantee Number" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:TextBox  ID="txtbankgurantee" runat="server" ValidationGroup="check" Width="50px" ></asp:TextBox>
                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                        
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbankgurantee"  ErrorMessage="Please Enter Numeric Value"  ValidationExpression="^\d+$" ValidationGroup="check">
                        </asp:RegularExpressionValidator>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
   
               <asp:TemplateField HeaderText="Bank Gurantee Amount" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:TextBox  ID="txtbankguranteeamt" runat="server" Width="70px"  OnTextChanged="TotalCredit" AutoPostBack="true"  ></asp:TextBox>
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbankguranteeamt"  ErrorMessage="Please Enter Numeric Value"  ValidationExpression="^\d+$" ValidationGroup="check">
                        </asp:RegularExpressionValidator>--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
  
                <asp:TemplateField HeaderText="Valid From" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:TextBox ID="txtsatrtdate" runat="server" Width="70px"  DataTextField=""></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtsatrtdate" >
                        </asp:CalendarExtender>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Valid To" HeaderStyle-Wrap="false">
                    <FooterTemplate>
                   <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="button" OnClientClick ="Addrow" />                    
<%--                     <asp:Button ID="Button1" runat="server" Text="Add New" CssClass="button"  OnClick ="Addrow" />
--%>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:TextBox  ID="txtvalidto" runat="server" Width="70px" DataTextField="Charge"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtvalidto" >
                        </asp:CalendarExtender>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
               
           </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>
        
        <table width="100%" >
<tr>
<td>
Credit Amount
</td>
<td>
    <asp:TextBox ID="txtcredit" runat="server" Enabled="false" ></asp:TextBox>
</td>
<td>
Invoice Balance
</td>
<td style="width:50px"  >
    <asp:TextBox ID="txtinvoice" runat="server" 
          ontextchanged="txtinvoice_TextChanged" AutoPostBack="True"></asp:TextBox>
</td>
<td>
Credit Remaining
</td>
<td>
    <asp:TextBox ID="txtcreditremain" runat="server"></asp:TextBox>
&nbsp;
</td>
</tr>

<tr>
<td>
<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" onclick="btnSave_Click" />
                    
  &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" />

</td>
</tr>
</table>
</div> 
                  <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%> 
<div>Credit Details</div> 
   <div class="divgrd">
  <asp:GridView ID="grdCreditdetails" runat="server" AutoGenerateColumns="False" 
                                       PageSize="6"  PagerSettings-Mode="Numeric"  
          AllowPaging="true" onpageindexchanging="grdCreditdetails_PageIndexChanging" 
          onsorting="grdCreditdetails_Sorting" AllowSorting="True" Width="923px"  >
            <Columns>
               
                
                <asp:TemplateField HeaderText="Agent Name" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="txtbankname" runat="server" Text='<%# Eval("AgentName") %>'></asp:Label>
                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                        
                        
                        
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
   
               <asp:TemplateField HeaderText="Bank Name" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblbankname" runat="server" Text='<%# Eval("bankName") %>'></asp:Label>
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbankguranteeamt"  ErrorMessage="Please Enter Numeric Value"  ValidationExpression="^\d+$" ValidationGroup="check">
                        </asp:RegularExpressionValidator>--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
  
                <asp:TemplateField HeaderText="Credit Amount" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblcreditamount" runat="server" Text='<%# Eval("CreditAmount") %>'></asp:Label>
                        
                    </ItemTemplate>
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Credit Remaining" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label   ID="txtvalidto" runat="server"  Text='<%# Eval("CreditRemaining") %>'></asp:Label>
                       
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
 </div> 

</asp:Content>