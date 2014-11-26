<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="MailBookingNew.aspx.cs" Inherits="ProjectSmartCargoManager.MailBookingNew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" language="javascript">

    function SelectAllAWB(headerchk) {
        var gvcheck = document.getElementById("<%=grdMailBooking.ClientID %>");
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
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
<div id="contentarea"><div class="msg"><asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label></div>
    <h1> 
            New Mail Booking
         </h1>
  
                <asp:Panel ID="pnlNew" runat="server">
                    <div class="botline">
                    <table class="ltfloat" width="30%;">
                    <tr>
                    <td>
                    Consignment ID
                    </td>
                    <td>
                    <asp:TextBox ID="txtConsignment" runat="server" Width="100px"></asp:TextBox>
                    </td>
                    <td>
                    <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
                            onclick="btnList_Click" />
                    <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" />
                    </td>
                    </tr>
                    </table> <div style="float:right;"> <strong>CN38</strong></div></div>
                    <div class="ltfloat" style="width:100%;">
                    <div class="divleftdvd">

                    <table cellpadding="3" cellspacing="3" width="100%">
                    <tr>
                    <td>
                    PostalAdministration of Origin
                    </td>
                    <td>
                    <asp:DropDownList runat="server" ID="ddlPostalAdmin" OnSelectedIndexChanged="ddlPostalAdmin_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    Office of Origin of Bill
                    </td>
                    <td>
                    <asp:DropDownList ID="ddlofcOrg" runat="server"></asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    Office of Destination Bill*
                    </td>
                    <td>
                    <asp:DropDownList ID="ddlofcDest" runat="server"></asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    Flight*
                    </td>
                    <td>
                    <asp:TextBox runat="server" ID="txtFlightDate" Width="80px" OnTextChanged="txtFlightDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" 
                                 ImageUrl="~/Images/calendar_2.png" />
                             <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                                 Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgDate" 
                                 TargetControlID="txtFlightDate">
                             </asp:CalendarExtender>
                    <asp:TextBox ID="txtFlightPrefix" runat="server" Width="30px"></asp:TextBox>
                    <asp:DropDownList ID="ddlFlight" runat="server"></asp:DropDownList>
                    </td>
                    </tr>
                    </table>
                    </div>
                    <div class="divrightdvd">
                    <table cellpadding="3" cellspacing="3" width="80%">
                    <tr>
                    <td>
                    Priority
                    </td>
                    <td>
                    <asp:CheckBox ID="chkPrority" runat="server" Checked="true" />
                    </td>
                    </tr>
                    <tr>
                    <td>
                    Airmail
                    </td>
                    <td>
                    <asp:CheckBox ID="chkAirmail" runat="server" />
                    </td>
                    </tr>
                    <tr>
                    <td>
                    Seal#
                    </td>
                    <td>
                    <asp:TextBox ID="txtSealNumber" runat="server" Width="100px" />
                    </td>
                    </tr>
                    <tr>
                    <td>
                    Status
                    </td>
                    <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" >
                    <asp:ListItem Text="Confirmed" Value="C"></asp:ListItem>
                                        <asp:ListItem Text="Queued" Value="Q" Selected="True">
                                        </asp:ListItem>
                                        </asp:DropDownList>
                    </td>
                    </tr>
                    </table></div>
                    </div>
                    </asp:Panel>
                    <div class="ltfloat" style="overflow:auto; width:100%;" >
                                    <asp:GridView ID="grdMailBooking" runat="server" 
                                             AutoGenerateColumns="False" Width="100%" AllowPaging="true" 
                                             PageSize="10" OnRowCommand="grdMailBooking_RowCommand">
                                <Columns>
                                   
                                    <asp:TemplateField>
                                             <HeaderTemplate>
                                           <asp:CheckBox ID="chk" runat="server" onclick="javascript:SelectAllAWB(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="check" runat="server" />
                                        </ItemTemplate>
                                        </asp:TemplateField>  
                                        
                                    <asp:TemplateField HeaderText="Mail Org" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlMailOrg" runat="server" Width="100px"></asp:DropDownList>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False"/>
                                        
                                    </asp:TemplateField>        
                                    <asp:TemplateField HeaderText="Mail Dest" HeaderStyle-Wrap="true" Visible="true">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlMailDest" runat="server" Width="100px" ></asp:DropDownList>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False"/>
                                    </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Letter PCS" HeaderStyle-Wrap="true" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLetterPCS" runat="server" Text='<%# Eval("LetterPCS") %>' Width="100px" ></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False"/>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Letter Wt" HeaderStyle-Wrap="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox  ID="txtLetter" runat="server" Width="100px" Text='<%# Eval("LetterWt") %>' >
                                            </asp:TextBox>
                                           
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField HeaderText="CP PCS" HeaderStyle-Wrap="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox  ID="txtCPPCS" runat="server" Width="100px" Text='<%# Eval("CPPCS") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                       
                                   <asp:TemplateField HeaderText="CP Wt" HeaderStyle-Wrap="false" Visible="false" >
                                        <ItemTemplate>
                                            <asp:TextBox  ID="txtCPWt" runat="server" Width="70px" Text='<%# Eval("CPWt") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField HeaderText="Empty Bags PCS" HeaderStyle-Wrap="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox  ID="txtEmpPCS" runat="server" Width="100px" Text='<%# Eval("EmptyBagPCS") %>'>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                   
                                                 
                                    
                                     <asp:TemplateField HeaderText="Empty Bags Wt" HeaderStyle-Wrap="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox  ID="txtEmpWt" runat="server" Width="100px" Text='<%# Eval("EmptyBagWt") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Total PCS" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:TextBox  ID="lblTotalPCS" runat="server" Width="80px" Text='<%# Eval("ToatalPCS") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Total Wt" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:TextBox  ID="lblTotalWt" runat="server" Width="80px" Text='<%# Eval("TotalWt") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="AWB#" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:LinkButton  ID="lblAWBNumber" runat="server" Width="80px" Text='<%# Eval("AWBNumber") %>' CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="AWB Status" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label  ID="lblStatus" runat="server" Width="80px" Text='<%# Eval("AWBStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Flight Date" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label  ID="lblFlightDt" runat="server" Width="80px" Text='<%# Eval("FlightDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Flight#" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label  ID="lblFlightNumber" runat="server" Width="80px" Text='<%# Eval("FlightNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="ULD#" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label  ID="lblULD" runat="server" Width="80px" Text='<%# Eval("ULD") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-Wrap="true" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsrno" runat="server" Width="100px" Text='<%# Eval("SerialNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="True"></HeaderStyle>
                                        <ItemStyle Wrap="False"/>
                                        
                                    </asp:TemplateField>
                                    
                               </Columns>
                                <HeaderStyle CssClass="titlecolr"/>
                                <RowStyle  HorizontalAlign="Center"/>
                                <AlternatingRowStyle  HorizontalAlign="Center"/>
                            </asp:GridView>
                            <asp:Button ID="btnAddRow" runat="server" CssClass="button" Text="Add" OnClick="btnAdd_Click"  />
                                                    
                                                    <%--<asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save"  />--%>
                                                    
                                                    <asp:Button ID="btnDeleteRow" runat="server" CssClass="button" Text="Delete" OnClick="btnDel_Click"  />
                                                    <asp:FileUpload ID="UpldMailBooking" runat="server" />
                                                    <asp:Button ID="btnUploadRates" runat="server" Text="Upload" Enabled="false" CssClass="button"/>
                                                    <asp:HyperLink ID="hlnk" Text="Download Template" NavigateUrl="~/Templates/MailBooking.xlsx" runat="server" />
                                                    </br>
                    </br>
                    </div>
                    
                    
                    
                    <asp:Button ID="btnSaveAll" runat="server" CssClass="button" 
        Text="Save" onclick="btnSaveAll_Click" />
                    <asp:Button ID="btnFinalize" runat="server" CssClass="button" 
        Text="Finalize" onclick="btnFinalize_Click"  />
                    <asp:Button ID="btnDelete" runat="server" CssClass="button" Text="Delete"  />
 </div>
</asp:Content>
