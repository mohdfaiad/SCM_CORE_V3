<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaintainAgents.aspx.cs" MasterPageFile ="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.MaintainAgents" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
 
     <div class="botline" >
     <table  style="width:100%">
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp; Airline Code</td>
                <td valign="middle">
                    <asp:TextBox ID="TextBox2" runat="server" CssClass="txtCommon" 
                        Width="61px" ></asp:TextBox>
                    &nbsp;
                    <asp:ImageButton ID="btnlistCustCode" runat="server"  
                ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
               <td>                 
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnList" runat="server" Text="Display" CssClass="button" />
                    &nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button"/>
                </td>
            </tr>
            </table>
      
      </div>             
             
   <tr>
    <td>
    
    
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" 
        Height="700px" Width="1661px">
            <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                <HeaderTemplate>Agent Details</HeaderTemplate>
                

<ContentTemplate><div><div class="botline" ><table><tr><td>&nbsp;&nbsp;&nbsp; IATA Agent Code </td><tr><td style="vertical-align:top" class="style18"><fieldset id="Fieldset2" style="border:1px solid #69b3d8;" ><legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Valid </legend><table ><tr>
    <td class="style76" ><asp:Label ID="Label1" runat="server" Text="From"></asp:Label></td><td><asp:TextBox ID="TextBox3" runat="server"></asp:TextBox><asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" /></td><td><asp:Label ID="Label2" runat="server" Text="To"></asp:Label></td><td ><asp:TextBox ID="TextBox4" runat="server" Width="108px"></asp:TextBox><asp:ImageButton ID="ImageButton2" runat="server" 
                                ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" Width="24px" 
                                Height="19px" /></td><tr><td  valign="middle" 
            class="style76">Customer Code </td><td>&nbsp;&nbsp;<asp:TextBox ID="txtCommodityTypeNo0" runat="server" CssClass="txtCommon" 
                        Width="55px" ></asp:TextBox></td><td valign="middle">Station </td><td>&nbsp;<asp:TextBox ID="TextBox5" runat="server" CssClass="txtCommon" 
                        Width="55px" ></asp:TextBox></td><td><asp:CheckBox runat="server" Text="Own Sales" /></td></tr><tr>
    <td class="style76"><asp:Label ID="Label3" runat="server" Text="Airline Code"></asp:Label></td><td><asp:TextBox ID="TextBox6" runat="server" Height="21px" Width="69px"></asp:TextBox><asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" /></td><td><asp:Label ID="Label4" runat="server" Text="Country"></asp:Label></td><td><asp:TextBox ID="TextBox7" runat="server" Height="16px" Width="107px"></asp:TextBox><asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" /></td><td><asp:Label ID="Label5" runat="server" Text="City"></asp:Label></td><td><asp:TextBox ID="TextBox8" runat="server"></asp:TextBox></td><td><asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" /></td></tr><tr>
    <td valign="middle" class="style76">Sales ID </td><td>&nbsp;<asp:TextBox ID="TextBox9" runat="server" CssClass="txtCommon" 
                        Width="55px" ></asp:TextBox></td><td valign="middle">Agent Type </td><td><asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList></td><td><asp:Label ID="Label6" runat="server" Text="Holding Company"></asp:Label></td><td class="style13"><asp:TextBox ID="TextBox10" runat="server"></asp:TextBox></td></tr><tr>
    <td class="style76">EORI No </td><td>&nbsp;<asp:TextBox ID="TextBox11" runat="server" CssClass="txtCommon" 
                        Width="55px" ></asp:TextBox></td></tr><tr>
        <td style="vertical-align:top" class="style76"><fieldset id="Fieldset1" style="border:1px solid #69b3d8;" ><legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Agent Attribute </legend><table ><tr><td><asp:CheckBox Text="Export" ID="CheckBox1" runat="server" /><asp:CheckBox Text="Import" ID="CheckBox3" runat="server" /><asp:CheckBox Text="Sales" ID="CheckBox2" runat="server" /></td></tr>
                        
                </tr><tr><td style="vertical-align:top"><fieldset id="Fieldset3" style="border:1px solid #69b3d8;" ><legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">GSA Details </legend><table ><tr><td class="style42"><asp:Label ID="Label7" runat="server" Text="GAS Type" ></asp:Label></td><td class="style41"><asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList></td><td><asp:Label ID="Label8" runat="server" Text="GSA Comm Type"></asp:Label></td><td><asp:DropDownList ID="DropDownList3" runat="server"></asp:DropDownList></td><td><asp:Label ID="Label9" runat="server" Text="Fixed"></asp:Label></td><td><asp:TextBox ID="TextBox12" runat="server"></asp:TextBox></td><td><asp:Label ID="Label10" runat="server" Text="Percentage"></asp:Label></td><td><asp:TextBox ID="TextBox13" runat="server"></asp:TextBox></td></tr>
                        
                </tr><tr><td><asp:Label ID="Label11" runat="server" Text="Office Adress"></asp:Label></td><td><asp:TextBox ID="TextBox14" runat="server" TextMode="MultiLine"></asp:TextBox></td><td><asp:Label ID="Label12" runat="server" Text="Remarks"></asp:Label></td><td><asp:TextBox ID="TextBox15" runat="server" TextMode="MultiLine"></asp:TextBox></td></tr></tr><tr><td align="right"><asp:Button ID="Button1" runat="server" Text="Save" /></td><td><asp:Button ID="Button2" runat="server" Text="Close" /></td></tr></table>
               </table></div></asp:panel></table></div><tr></tr></ContentTemplate>
                
             

</asp:TabPanel>
             
            
            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
            <HeaderTemplate>Biilling Details</HeaderTemplate>
               

<ContentTemplate><div class="botline" ><table><tr><td><asp:TextBox ID="TextBox1" runat="server" Height="23px" Width="87px"></asp:TextBox></td><td >&nbsp;&nbsp;&nbsp; Currency Code </td><td valign="middle"></td><td></td><td>&nbsp;Currency Number <asp:TextBox ID="TextBox16" runat="server" Height="19px" 
            Width="113px"></asp:TextBox></td><td></td><td>&nbsp;&nbsp;&nbsp; Settl Curr <asp:TextBox ID="TextBox18" runat="server"></asp:TextBox></td><td valign="middle">&nbsp;&nbsp;&nbsp; CASS <asp:TextBox ID="TextBox19" runat="server" CssClass="txtCommon" 
            Width="61px"></asp:TextBox>&nbsp;&nbsp;<asp:ImageButton ID="ImageButton7" runat="server" ImageAlign="AbsMiddle" 
            ImageUrl="~/Images/list_bullets.png"></asp:ImageButton></td><td><asp:TextBox ID="TextBox17" runat="server" CssClass="txtCommon" 
                        Width="61px" ></asp:TextBox>&nbsp;&nbsp; <asp:ImageButton ID="ImageButton6" runat="server"  
                ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><asp:DropDownList ID="DropDownList4" runat="server"></asp:DropDownList></td></tr><tr><td style="vertical-align:top" class="style66"><fieldset ID="Fieldset4" style="border:1px solid #69b3d8;"><table ><tr><td class="style71"><asp:CheckBox ID="CheckBox4" runat="server" Text="Sales Reporting Agent" /></td><td class="style72"><asp:CheckBox ID="CheckBox5" runat="server" Text="Controling LOC" /></td><td class="style73"><asp:CheckBox ID="CheckBox6" runat="server" Text="Selling LOC" /></td><td><asp:CheckBox ID="CheckBox7" runat="server" Text="Generate Performa Invoices" /></td></tr></table></fieldset> </td><td><asp:Label ID="Label13" runat="server" Text="Reporting Period"></asp:Label></td><td class="style65"><asp:DropDownList ID="DropDownList5" runat="server"></asp:DropDownList></td><td><asp:Label ID="Label14" runat="server" Text="Crdt Lmt"></asp:Label></td><td><asp:TextBox ID="TextBox20" runat="server"></asp:TextBox></td></tr><tr><td align="right" class="style66"></td><td><asp:Label ID="Label15" runat="server" Text="Billing Period"></asp:Label></td><td class="style65"><asp:TextBox ID="TextBox21" runat="server"></asp:TextBox></td><td><asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" /></td></tr><tr><td class="style66"></td><td><asp:Label ID="Label16" runat="server" Text="Normal Comm %"></asp:Label></td><td class="style65"><asp:TextBox ID="TextBox22" runat="server"></asp:TextBox></td><td><asp:Label ID="Label17" runat="server" Text="Fixed"></asp:Label></td><td><asp:TextBox ID="TextBox23" runat="server"></asp:TextBox></td></tr><tr><td style="vertical-align:top"><fieldset id="Fieldset5" style="border:1px solid #69b3d8;" ><legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">CL Details </legend><table ><tr><td><asp:Label ID="Label18" runat="server" Text="Agent Code" ></asp:Label></td><td><asp:TextBox ID="TextBox24" runat="server" CssClass="txtCommon" 
     Width="61px" ></asp:TextBox><asp:ImageButton ID="ImageButton9" runat="server"  
    ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td><td><asp:Label ID="Label19" runat="server" Text="Name"></asp:Label></td><td><asp:TextBox ID="TextBox25" runat="server"></asp:TextBox></td><td><asp:Label ID="Label20" runat="server" Text="Bill To"></asp:Label></td><td><asp:DropDownList ID="DropDownList6" runat="server"></asp:DropDownList></td><td style="vertical-align:top"><fieldset id="Fieldset6" style="border:1px solid #69b3d8;" ><table ><tr><td><asp:Label ID="Label21" runat="server" Text="Agent Account Code"></asp:Label></td><td><asp:TextBox ID="TextBox26" runat="server" Height="23px" Width="74px"></asp:TextBox></td>
    </td></tr>
                        </tr><tr><td style="vertical-align:top"><fieldset id="Fieldset7" style="border:1px solid #69b3d8;" ><legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Valid </legend><table ><tr><td class="style75" >
                        <asp:RadioButton ID="RadioButton1" runat="server" Text="AWB" />
                        
                        <asp:RadioButton ID="RadioButton2" runat="server" Text="Invoice" />
                        
                        </td>
                        <tr>
        <asp:GridView ID="grvAgent" runat="server" AutoGenerateColumns="False" 
                    Height="106px" ShowFooter="True" Width="927px"><AlternatingRowStyle HorizontalAlign="Center" /><Columns><asp:TemplateField HeaderText="AWB"><ItemTemplate><asp:TextBox ID="txtFromDate" runat="server">
                    </asp:TextBox></ItemTemplate><HeaderStyle Wrap="False" /></asp:TemplateField><asp:TemplateField HeaderText="ORC%"><ItemTemplate><asp:TextBox ID="txtToDate" runat="server">
                    </asp:TextBox></ItemTemplate><HeaderStyle Wrap="False" /></asp:TemplateField><asp:TemplateField HeaderText="Apply On"><ItemTemplate><asp:DropDownList ID="DropDownList7" runat="server"></asp:DropDownList></ItemTemplate><HeaderStyle Wrap="False" /></asp:TemplateField></Columns><HeaderStyle CssClass="titlecolr" /><RowStyle HorizontalAlign="Center" /></asp:GridView></tr>
  
                </td>
                </tr>
                <tr>
                <td style="vertical-align:top">
                <fieldset id="Adress" style="border:1px solid #69b3d8;" >
                <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Settlement Tolarance 
                </legend><table width="100%">
                <tr>
                <td>
                    <asp:CheckBox ID="CheckBox8" runat="server" Text="AWB" />
                </td>
                <td>
                    <asp:CheckBox ID="CheckBox9" runat="server" Text="Invoice" />
                </td>
                    <tr>
                        <td>
                            <asp:Label ID="Label22" runat="server" Text="Tolarance %"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox27" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label23" runat="server" Text="Tolarance Value"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox28" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label24" runat="server" Text="Max Value"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox29" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <td style="vertical-align:top">
                                <fieldset ID="Fieldset8" style="border:1px solid #69b3d8;">
                                    <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Billing 
                                        Adress </legend>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label25" runat="server" Text="Contact Person"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox30" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label26" runat="server" Text="Adress"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox31" runat="server" Width="152px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label27" runat="server" Text="City"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox32" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset></td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label28" runat="server" Text="State"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox33" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label29" runat="server" Text="Country"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox34" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label30" runat="server" Text="Mobile"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox35" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label31" runat="server" Text="Postal/Zip Code"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox36" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label32" runat="server" Text="Phone 1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox37" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label33" runat="server" Text="Phone 2"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox38" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label34" runat="server" Text="FAX"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox39" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label35" runat="server" Text="Email"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox40" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </tr>
                     </table>
                     </table>
                     </td>
                     </tr>
                     </tr>
                     </div>
                     </table>
                     </ContentTemplate>
                     </asp:TabPanel>
                     </asp:TabContainer>
                     </td>
                     </tr>
                     
                     <tr>
                     <td>
                         <asp:Button ID="Button3" runat="server" Text="Save" />
                     </td>
                     
                     <td valign ="middle" align="right" >
                         <asp:Button ID="Button4" runat="server" Text="Close" />
                     </td>
                     </tr>
                     </asp:Panel>
                     </asp:Content>
                     <asp:Content ID="Content2" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style76
        {
            width: 403px;
        }
        </style>

</asp:Content>
 