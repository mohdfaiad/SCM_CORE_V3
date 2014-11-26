<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmULDMovement.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmULDMovement" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">
        function SelectAllULD(headerchk) {
            var gvcheck = document.getElementById("<%=gvAddressGrpFTP.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    if (inputs[0].disabled == false)
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
<style>
.visf
{
    display :none;
}
</style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript">
        function CheckOtherIsCheckedByGVID(spanChk) {

            var IsChecked = spanChk.checked;
            if (IsChecked) {
            }
            var CurrentRdbID = spanChk.id;
            var Chk = spanChk;
            Parent = document.getElementById("<%=gvAddressGrpFTP.ClientID%>");
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != CurrentRdbID && items[i].type == "radio") {
                    if (items[i].checked) {
                        items[i].checked = false;

                    }
                }
            }
        }
</script>
    <asp:ToolkitScriptManager ID="TookScriptManager" runat="server"></asp:ToolkitScriptManager>
<div id="contentarea">
<h1>ULD Movement</h1>

<div class="botline">
<table cellpadding="3" cellspacing="3">
    <tr>
        <td colspan="12">
<asp:Label ID="lblStatus" runat="server" ForeColor="Red" 
            style="font-weight: 700; font-size: medium"></asp:Label>
        </td>
    </tr>
<tr>
    <td> Status </td>
    <td>
        <asp:DropDownList ID="drpMoveStatusNew" runat="server">
                </asp:DropDownList>
    </td>
<td>Origin</td>
<td>
    <asp:DropDownList ID="ddlOrigin" runat="server">
    </asp:DropDownList>
</td>

<!--<td>Dest</td>
<td>
    <asp:DropDownList ID="ddlDestination" runat="server">
    </asp:DropDownList>
</td>-->

<td>
    Flight #</td>
<td>
    <asp:TextBox ID="txtFltNo" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
    </td>
<td>
    Date</td>
<td>
    <asp:TextBox ID="txtFltdate" runat="server" Width="100px"></asp:TextBox>
    <asp:ImageButton ID="imgFltDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
    <asp:CalendarExtender ID="txtFltdate_CalendarExtender" runat="server" 
        Enabled="True" TargetControlID="txtFltdate" Format="dd/MM/yyyy" PopupButtonID="imgFltDate">
    </asp:CalendarExtender>
</td>

<td>Type</td>
<td>
    <asp:DropDownList ID="ddlMovementType" runat="server">
        <asp:ListItem Selected ="True" >SELECT</asp:ListItem>
        <asp:ListItem>IN</asp:ListItem>
        <asp:ListItem>OUT</asp:ListItem>
    </asp:DropDownList>
</td>

<td>
    ULD #
</td>
<td>
    <asp:TextBox ID="txtULDNo" runat="server" MaxLength="200" Width="250px" 
    ToolTip="Enter ULD no.If Multiple ULD's Enter comma separated.eg:AKE00211BA,AKE00212BA"></asp:TextBox>
    
    </td>
</tr>
<tr>
    <td colspan="4">
        <asp:Button ID="btnSearch" runat="server"  CssClass="button"  
        Text="List" onclick="btnSearch_Click"/>
        &nbsp;
        <asp:Button ID="btnClear" runat="server"  CssClass="button"  
        Text="Clear" onclick="btnClear_Click"/>
    </td>
</tr>
</table>
    </div>
<br />

<!--<asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add" Height="23px" 
        Visible="false" />
<asp:Button ID="btnEdit" runat="server" CssClass="button" Text="Edit" 
        Visible="false" />
<asp:Button ID="btnDelete" runat="server" CssClass="button" Text="Delete" 
        Visible="false" OnClientClick='return confirm("Are you sure you want to delete this stock?");' />
    -->
</div>
<br />
    <h1> ULD Movement Details</h1>
   <asp:GridView ID="gvAddressGrpFTP" runat="server" AutoGenerateColumns="False"  Width="100%"
             ShowFooter="false"    HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="trcolor" PagerStyle-CssClass="PagerStyle" >
    <Columns>
    
        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
            <HeaderTemplate >
                <asp:CheckBox ID="chkAll" runat="server" onclick="javascript:SelectAllULD(this);" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>

    <asp:TemplateField HeaderText="ULD Number" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:Table ID = "tblULDNumber" runat = "server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="lblULDNumber" runat="server" Text='<%# Eval("ULDNumber") %>' Width="120px" Enabled="false"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow Visible = "false" VerticalAlign ="Bottom" >
            <asp:TableCell VerticalAlign ="Bottom">
                <asp:TextBox ID="txtULDNumber" runat="server" Text='<%# Eval("ULDNumber") %>' Width="120px" Enabled="false"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    </ItemTemplate>
    
    </asp:TemplateField >

    <asp:TemplateField  HeaderText="Status" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:Table ID= "tblULDStatus" runat = "server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="lblULDStatus" Text='<%# Eval("ULDStatus") %>' runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow Visible = "false" VerticalAlign ="Bottom" >
            <asp:TableCell VerticalAlign ="Bottom">
                <asp:DropDownList ID="ddlULDStatus" runat="server" ></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField  HeaderText="Origin" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" >
    <ItemTemplate>
    <asp:Table ID= "tblOrigin" runat = "server" VerticalAlign ="Bottom">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="lblOrigin" Text='<%# Eval("Origin") %>' runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow Visible = "false" VerticalAlign ="Bottom">
            <asp:TableCell VerticalAlign ="Bottom">
                <asp:DropDownList ID="ddlOrigin" runat="server" CommandArgument="LoadFlightOrigin"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    </ItemTemplate>
    </asp:TemplateField>
    
     <asp:TemplateField  HeaderText="Destination" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" >
    <ItemTemplate>
    <asp:Table ID= "tblDestination" runat = "server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="lblDestination" Text='<%# Eval("Destination") %>' runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow Visible = "false" VerticalAlign ="Bottom">
            <asp:TableCell VerticalAlign ="Bottom">
                <asp:DropDownList ID="ddlDestination" runat="server" ></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField  HeaderText="Moved On" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:Table ID= "tblMovedOn" runat = "server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="lblMovedOn" runat="server" Text='<%# Eval("MovedOn") %>'  ReadOnly="true"></asp:Label>
                &nbsp
                <asp:Label ID="lblMoveTime" runat="server" Text='<%# Eval("MovedOnTime") %>'  ReadOnly="true"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow Visible = "false"  VerticalAlign ="Bottom">
            <asp:TableCell  VerticalAlign ="Bottom">
                <asp:TextBox Width="70px" ID="txtMovedOn" Text='<%# Eval("MovedOn") %>' runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="txtRecCarDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                    Enabled="True" TargetControlID="txtMovedOn">
                </asp:CalendarExtender>
                <asp:TextBox Width="50px" ID="txtMoveTime" Text='<%# Eval("MovedOnTime") %>' runat="server"></asp:TextBox>
                
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField  HeaderText="Flight" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" >
    <ItemTemplate>
    <asp:Table ID= "tblFlightID" runat = "server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="lblFlightID" runat="server" Width="100px" Text='<%# Eval("FlightID") %>'  CssClass="visf"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow Visible = "false"  VerticalAlign ="Bottom">
            <asp:TableCell VerticalAlign ="Bottom">
                <%--<asp:DropDownList ID="ddlFlightID" runat="server"></asp:DropDownList>--%>
                <asp:TextBox Width="100px" ID="txtFlightID" Text='<%# Eval("FlightID") %>' runat="server"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField  HeaderText="Type" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:Table ID= "tblMovementType" runat = "server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="lblMovementType" runat="server" Text='<%# Eval("MovementType") %>' Width="50px" ReadOnly="true"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow Visible = "false" VerticalAlign ="Bottom">
            <asp:TableCell VerticalAlign ="Bottom">
                <asp:DropDownList Width="50px" ID="ddlMovementType" runat="server">
                <asp:ListItem Selected = "True" >IN</asp:ListItem>
                <asp:ListItem>OUT</asp:ListItem>
                </asp:DropDownList>
                
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    </ItemTemplate>
    </asp:TemplateField>
    
    <%--<asp:TemplateField  HeaderText="Flight No" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:DropDownList ID="ddlFlightNo" runat="server"></asp:DropDownList>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField  HeaderText="Flight Date" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtFlightDt" runat="server" Text='<%# Eval("FlightDt") %>'>
    </asp:TextBox>
    <asp:CalendarExtender ID = "txtFlightDt_Calender" runat="server" TargetControlID = "txtFlightDt" >
    </asp:CalendarExtender>
    </ItemTemplate>
    </asp:TemplateField>--%>
    
    <asp:TemplateField  HeaderText="Updated By" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
        <asp:Label ID="lblUpdatedBy" runat="server" Text='<%# Eval("UpdatedBy") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField  ItemStyle-HorizontalAlign="Center" Visible="false">
    <ItemTemplate>
        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Move" OnClick="btnSave_Click" CommandArgument='<%# Eval("ULDNumber") %>' />
    </ItemTemplate>
    </asp:TemplateField >
    
    </Columns>
         <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                    <FooterStyle CssClass="grdrowfont"></FooterStyle>
                    <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                    <RowStyle CssClass="grdrowfont"></RowStyle>
    </asp:GridView>
    
    <asp:Panel ID="pnlMove" runat="server" Visible="false">
    <table cellpadding="3" cellspacing="3">
        <tr>
            
            <!--<td> Origin </td>
            <td>
                <asp:DropDownList ID="drpMoveOriginNew" runat="server">
                </asp:DropDownList>
            </td>-->

            <td> Destination </td>
            <td>
                <asp:DropDownList ID="drpMoveDestNew" runat="server">
                </asp:DropDownList>
            </td>

            <td> Flight # </td>
            <td>
                <asp:TextBox ID="txtMoveFlightNew" runat="server" Width="50px"></asp:TextBox>
            </td>
            <td> Flight Date </td>
            <td>
                <asp:TextBox Width="70px" ID="txtMoveFlightDate" Text="" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImgMoveFltDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                    Enabled="True" TargetControlID="txtMoveFlightDate" PopupButtonID="ImgMoveFltDate">
                </asp:CalendarExtender>
            </td>
            <td> Moved On </td>
            <td>
                <asp:TextBox Width="70px" ID="txtMovedOnNew" Text="" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="txtMovedOnNew_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                    Enabled="True" TargetControlID="txtMovedOnNew" PopupButtonID="ImgMoveOn">
                </asp:CalendarExtender>
                <asp:TextBox Width="35px" ID="txtMoveTimeNew" Text="00:00" runat="server"></asp:TextBox>
                <asp:ImageButton ID="ImgMoveOn" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
            </td>

            <!--<td> Type </td>
            <td>
                <asp:DropDownList ID="drpMoveTypeNew" runat="server">
                    <asp:ListItem Selected="True">IN</asp:ListItem>
                    <asp:ListItem>OUT</asp:ListItem>
                </asp:DropDownList>
            </td>-->
            <td>
                <asp:Button ID="Button1" Text="Move" runat="server" CssClass="button" OnClick="btnMoveNew_Click" />
            </td>
        </tr>
    </table>
    </asp:Panel>


</asp:Content>