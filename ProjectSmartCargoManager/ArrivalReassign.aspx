<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArrivalReassign.aspx.cs" Inherits="ProjectSmartCargoManager.ArrivalReassign" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">
    <title></title>
     <script type="text/javascript">

         function SelectAllgrdAddRate(CheckBoxControl) {
             for (i = 0; i < document.forms[0].elements.length; i++) {
                 if (document.forms[0].elements[i].name.indexOf('check') > -1) {
                     document.forms[0].elements[i].checked = CheckBoxControl.checked;
                 }
             }
         }
         function closePage() {
             window.close();
         }

         function diabled() {
             var btn = document.getElementById('btnSave').value;
             document.getElementById('btnSave').disabled = true;
             __doPostBack("btnSave", "OnClick");

         } 

//         function Getdest() {
//             var dest = document.getElementById('txtDestination').value;
//             alert(dest);
//             var hid = document.getElementById('hiddest');
//             hid.value = dest;
//             
//             alert(hid.value);
//         }
            
        
    </script>
     <link href="style/style.css" rel="stylesheet" type="text/css" />
</head>
<body class="divback">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div>
     <br />
        <asp:Label ID="LBLStatus" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
        <br />
        
        <table width="100%">
          <tr>
         <%-- <td>
           <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Origin & Destination</legend>
           <table width="100%">
            <tr>
             <td>
               Flight Number
             </td>
             <td>
                 <asp:DropDownList ID="DropDownList1" runat="server">
                 </asp:DropDownList>
             </td>
            </tr>
           </table>  
           
           </fieldset> 
          
          
          </td>--%>
          
          <td>
            <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Origin & Destination</legend>
            <table width="95%" >
             <tr>
              <td>
               Origin
                  *</td>
              <td>
                  
                  <asp:TextBox ID="txtorigin" runat="server" Enabled="False" Width="80px" ></asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ControlToValidate="txtorigin" 
                      ErrorMessage="*"></asp:RequiredFieldValidator>
                  <asp:AutoCompleteExtender ID="txtorigin_AutoCompleteExtender" runat="server" 
                   TargetControlID="txtorigin" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
                   EnableCaching="true" >
                  </asp:AutoCompleteExtender>
              </td>
              <td>
                Destintion*
              </td>
              <td>
                  <asp:TextBox ID="txtDestination" runat="server" Width="80px"   
                      ontextchanged="txtDestination_TextChanged" AutoPostBack="true"></asp:TextBox>
                  <asp:TextBoxWatermarkExtender ID="txtDestination_TextBoxWatermarkExtender" 
                      runat="server" TargetControlID="txtDestination" WatermarkText="Enter Destination" >
                  </asp:TextBoxWatermarkExtender>
                  <asp:AutoCompleteExtender ID="txtDestination_AutoCompleteExtender" 
                      runat="server" TargetControlID="txtDestination" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
                   EnableCaching="true" >
                  </asp:AutoCompleteExtender>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDestination" 
                      ErrorMessage="*"></asp:RequiredFieldValidator>
              </td>
              <td>
               Flight No:
                  *</td>
              <td>
                  <asp:TextBox ID="txtflightno" runat="server"></asp:TextBox>
                  <asp:AutoCompleteExtender ID="txtflightno_AutoCompleteExtender" runat="server" 
                      TargetControlID="txtflightno" ServiceMethod="GetFlight" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/ArrivalReassign.aspx">
                  </asp:AutoCompleteExtender>
              </td>
              
              <%--<td>
                  <asp:DropDownList ID="ddlflightno" runat="server">
                  </asp:DropDownList>
              </td>--%>
              <td>
               Re-Assignment Date
                  *</td>
              <td>
                  <asp:TextBox ID="txtreassignmentdate" runat="server" Width="100px" ></asp:TextBox>
                  
                  <asp:CalendarExtender ID="txtreassignmentdate_CalendarExtender" runat="server" 
                      TargetControlID="txtreassignmentdate" Format="dd-MM-yyyy" >
                  </asp:CalendarExtender>
                  
              </td>
             </tr>
            </table> 
            </fieldset>  
          </td>
          
         </tr>
        </table> 
        
        <fieldset style="width: 1028px">  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Pieces & weight</legend>
           <div style="overflow:auto">
    <table>
    <tr>
    <td>
    <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" 
                          ID="grdArrivalReassign"                                    
                                      Width="100%" CssClass="grdrowfont"> 
                                  <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                        <Columns>
                        <%-- <asp:TemplateField>
                         <HeaderTemplate>
                       <asp:CheckBox ID="chk" runat="server" onclick="javascript:SelectAllgrdAddRate(this);"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="check" runat="server" />
                    </ItemTemplate>
                    </asp:TemplateField>  --%>                       
                        
                        <asp:TemplateField HeaderText="AWB No."><ItemTemplate>
                            <asp:Label ID="lblawbno" runat="server" Width="80px" CssClass="grdrowfont"  Text='<%# Eval("AWBNumber") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Pieces">
                        <ItemTemplate>
                            <asp:Label ID="lblpieces" runat="server" Width="110px" CssClass="grdrowfont" Text='<%# Eval("Pieces") %>'>>
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>      
                        
                        <asp:TemplateField HeaderText="Weight">
                        <ItemTemplate>
                            <asp:Label ID="lblweight" runat="server" Width="110px" CssClass="grdrowfont" Text='<%# Eval("Weight") %>'>>
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>
                        
                        
                        <asp:TemplateField HeaderText="Flight Number">
                        <ItemTemplate>
                            <asp:Label ID="lblflightno" runat="server" Width="110px" CssClass="grdrowfont" Text='<%# Eval("FlightNumber") %>'>>
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>  
                        
                        <asp:TemplateField HeaderText="Origin">
                        <ItemTemplate>
                            <asp:Label ID="lblorigin" runat="server" Width="110px" CssClass="grdrowfont" Text='<%# Eval("Origin") %>'>>
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>  
                       
                       <asp:TemplateField HeaderText="Destination">
                        <ItemTemplate>
                            <asp:Label ID="lbldestination" runat="server" Width="110px" CssClass="grdrowfont" Text='<%# Eval("Destination") %>'>>
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>  
                        
                        </Columns>

                        <EditRowStyle CssClass="grdrowfont"></EditRowStyle>

                      
                     <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
                   <asp:HiddenField ID="hiddest" runat="server" />
                   <input type="hidden" id="Hidddest" runat="server" />
          <%-- &nbsp;Pieces
              *</td>
          <td width="176px">
              <asp:TextBox ID="txtAWBPieces" runat="server"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                  ErrorMessage="*" ControlToValidate="txtAWBPieces" ></asp:RequiredFieldValidator>
          </td>
          <td>
           &nbsp;Weight
              *</td>
          <td>
              <asp:TextBox ID="AWBWeight" runat="server"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                  ErrorMessage="*" ControlToValidate="AWBWeight" ></asp:RequiredFieldValidator>--%>
                  
                  
          </td>
          
         </tr>
        </table>
           </div> 
   </fieldset>
   
   <br />
        &nbsp;&nbsp;
        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" 
            onclick="btnSave_Click" OnClientClick="javascript:diabled()"/>
            <asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" OnClientClick="javascript:closePage()" />
        
       
        
    </div>
    </form>
</body>
</html>
