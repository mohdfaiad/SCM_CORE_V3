<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowDelivery.aspx.cs" Inherits="ProjectSmartCargoManager.ShowDelivery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<link href="style/style.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">

         function PrintDO() 
         {
             window.open('PrintDeliveryForm.aspx', 'Print', 'toolbar=0,resizable=1');
             return false;
         }

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
     
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
<%--    <link href="style/style.css" rel="stylesheet" type="text/css" />
--%>    </head>
<body class="divback">
    <form id="form1" runat="server">
    <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
    </p>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
    <div>
    <table >
    <tr>
    <td>
     Flight
    </td>
    <td>
        <asp:DropDownList ID="ddlflightnumber" runat="server">
        </asp:DropDownList>
    </td>
    <td>
        <asp:TextBox ID="txtflightDate" runat="server" Width="100px"></asp:TextBox>
        <asp:CalendarExtender ID="txtflightDate_CalendarExtender" runat="server" Format="yyyy-MM-dd" 
            TargetControlID="txtflightDate" PopupButtonID="imgDate">
        </asp:CalendarExtender>
        <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
    </td>
    <td>
     <asp:Button ID="btnList" runat="server" Text="List" onclick="btnList_Click" CssClass="button"/>
    </td>
    </tr>
    </table> 
    </div> 
    <div style="overflow:auto">
    <table>
    <tr>
    <td>
    <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" 
                          ID="grdDeliveryDetails"                                    
                                      Width="100%" CssClass="grdrowfont"> 
                                  <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                        <Columns>
                         <asp:TemplateField>
                         <HeaderTemplate>
                       <asp:CheckBox ID="chk" runat="server" onclick="javascript:SelectAllgrdAddRate(this);"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="check" runat="server" />
                    </ItemTemplate>
                    </asp:TemplateField>   
                    
                    
                    <asp:TemplateField HeaderText="Do Number">
                        <ItemTemplate>
                            <asp:Label ID="lblDoNo" runat="server" Text='<%# Eval("DONumber") %>' Width="110px" CssClass="grdrowfont">
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>                       
                    
                        <asp:TemplateField HeaderText="Agent Name">
                        <ItemTemplate>
                            <asp:Label ID="txtagentname" runat="server" Text='<%# Eval("AgentName") %>'  
                            Width="110px" CssClass="grdrowfont">
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>    
                          
                        <asp:TemplateField HeaderText="AWB No."><ItemTemplate>
                            <asp:Label ID="txtawbno" runat="server" Text='<%# Eval("AWBNumber") %>'
                            Width="80px" CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        
                       
                        <asp:TemplateField HeaderText="Total Pcs">
                        <ItemTemplate>
                            <asp:Label ID="txttotalpieces" runat="server" Text='<%# Eval("TotalPieces")%>'  Width="55px">
                            </asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Wt">
                        <ItemTemplate>
                                        <asp:Label ID="lbltotalwt" runat="server" Text='<%# Eval("ActualWeight") %>'  Width="65px">
                                        </asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>
                       
                        
                        <asp:TemplateField HeaderText="Flight No.">
                        <ItemTemplate>
                            <asp:Label ID="txtflightno" runat="server" Text='<%# Eval("FlightNumber") %>'  Width="60px" Enabled="false"  >
                            </asp:Label>                
                        </ItemTemplate>
                        </asp:TemplateField>
                        
                       <asp:TemplateField HeaderText="HAWB Number"><ItemTemplate>
                            <asp:Label  ID="lblhawbnumber" runat="server" Width="100px" Enabled="false" Text='<%# Eval("HAWBNumber") %>' EnableViewState="true"   >
                            </asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>
                        
                        
                        <asp:TemplateField HeaderText="Issue Date"><ItemTemplate>
                            <asp:Label ID="lblissuedate" runat="server" Width="100px" Text='<%# Eval("IssueDate") %>' Enabled="false">
                            </asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>
                                               
                         <asp:TemplateField HeaderText="Issued To"><ItemTemplate>
                            <asp:Label  ID="lblissuedto" runat="server" Width="100px" Enabled="false" Text='<%# Eval("IssuedTo") %>' EnableViewState="true"   >
                            </asp:Label >
                        </ItemTemplate>
                        </asp:TemplateField>
                        
                        
                         <asp:TemplateField HeaderText="Issue Name"><ItemTemplate>
                            <asp:Label ID="lblissuename" runat="server" Width="100px" Enabled="false" Text='<%# Eval("IssueName") %>' EnableViewState="true"   >
                            </asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>
                        
                         <asp:TemplateField HeaderText="Recivers Name"><ItemTemplate>
                            <asp:Label  ID="lblReciversName" runat="server" Width="100px" Enabled="false" Text='<%# Eval("ReciversName") %>' EnableViewState="true"   >
                            </asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>
                        
                         <asp:TemplateField HeaderText="Consignee Name"><ItemTemplate>
                            <asp:Label  ID="lblconsignee" runat="server" Width="100px" Enabled="false" Text='<%# Eval("Consignee") %>' EnableViewState="true"   >
                            </asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>
                        
                        
                        </Columns>

                        <EditRowStyle CssClass="grdrowfont"></EditRowStyle>

                       <%-- <FooterStyle CssClass="grdrowfont"></FooterStyle>

                        <HeaderStyle CssClass="titlecolr"></HeaderStyle>

                        <RowStyle CssClass="grdrowfont"></RowStyle>--%>
                     <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
    </td>
    
    
    </tr>
    <tr>
    <td class="divback">
        <asp:Button ID="BtnPrint" runat="server" Text="Re-Print DO" CssClass="button" 
            onclick="BtnPrint_Click"  />
    </td>
    </tr>
    </table> 
    
    
    
    </div>
    </form>
</body>
</html>
