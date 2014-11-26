<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPOStatusChange.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmPOStatusChange" Title="PO Status Change" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript">
     function SelectAll(id) {

             var frm = document.forms[0];
         //var frm = document.getElementById("<%=GVOrdDetails.ClientID %>");

         for (i = 0; i < frm.elements.length; i++) {

             if (frm.elements[i].type == "checkbox") {
                 if (frm.elements[i].disabled == false)
                     frm.elements[i].checked = id.checked;
             }
         }
     }

     function JSheadAll() {
         alert("f");
         var GridView = document.getElementById('<%=GVOrdDetails.ClientID %>')
         alert(GridView.rows.length);
         if (GridView.rows.length > 0) {
             for (Row = 1; Row < GridView.rows.length; Row++) {
                 alert(Row);
                 if (GridView.rows[Row].cell[0].type == "checkbox") 
                 {
                     if (GridView.rows[Row].cell[2].childNodes[0].checked) 
                     {
                         GridView.rows[Row].cell[3].childNodes[0].disabled = false; // Enable your control here
                     }
                 }
                 alert("b");
             }
         }
     }
  </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager> 
    <div id="contentarea">
        <h1>Purchase Orders Status Change</h1>
        
        <div class="botline">
        <asp:Label ID="lblerror" runat="server" Font-Bold="true" Font-Size="Medium" ></asp:Label>
        <table width="100%">
        <tr>
        <td>Purchase Order #
            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                ControlToValidate="txtPONo" ErrorMessage="*" ValidationGroup="SEARCH" 
                SetFocusOnError="True"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
        <td>
            <asp:TextBox ID="txtPONo" runat="server" MaxLength="15"></asp:TextBox>
            &nbsp; 
            <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="List" OnClick="btnSearch_Click" ValidationGroup="SEARCH" />
            </td>
        </tr>
        </table>
        </div>
        
        <div id="singlecol">
         <strong>Order Details</strong>
            <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <asp:gridview ID="GVOrdDetails" runat="server" Width="100%" 
                        AutoGenerateColumns="False" 
            HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
                        AlternatingRowStyle-CssClass="AltRowStyle" 
                         PagerStyle-CssClass="PagerStyle" onrowdatabound="GVOrdDetails_RowDataBound" >
            <RowStyle CssClass="RowStyle"></RowStyle>
            <Columns>

                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:CheckBox ID="chkPOLine" runat="server" AutoPostBack="true" OnCheckedChanged="chkLineSD"></asp:CheckBox>
            </ItemTemplate>
            <HeaderTemplate>
            <asp:CheckBox ID="chkfullPO" runat="server"  AutoPostBack="true" OnCheckedChanged="chkHeadAll"></asp:CheckBox>
            </HeaderTemplate>

            </asp:TemplateField>
            <asp:TemplateField HeaderText="ULD Type">
            <ItemTemplate>
            <asp:Label ID="grdULDType" runat="server" Text='<%# Eval("ULDType") %>'></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="ULD Part">
            <ItemTemplate>
            <asp:Label ID="grdULDPartNo" runat="server" Text='<%# Eval("ULDPart") %>'></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity">
            <ItemTemplate>
            <asp:Label ID="grdQuantity" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Delivery Date">
            <ItemTemplate>
            <asp:Label ID="grdDelivery" runat="server" Text='<%# Eval("Delivery") %>'></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="WareHouse">
            <ItemTemplate>
            <asp:Label ID="grdWareHouse" runat="server" Text='<%# Eval("WareHouse") %>'></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="ID">
            <ItemTemplate>
            <asp:Label ID="grdisRec" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ControlStyle-BorderStyle="None" HeaderText="Received ULDs"  ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="None"  ItemStyle-CssClass="">
                <ItemTemplate>
	              <asp:CheckBoxList ID="chList" runat="server"  
	              Visible="True"  AutoPostBack="true" OnSelectedIndexChanged="chkChildSD" BorderStyle="None">
	              </asp:CheckBoxList>
                 </ItemTemplate>
            </asp:TemplateField>
            </Columns>

            <PagerStyle CssClass="PagerStyle"></PagerStyle>

            <HeaderStyle CssClass="HeaderStyle"></HeaderStyle>

            <AlternatingRowStyle CssClass="AltRowStyle"></AlternatingRowStyle>
            </asp:gridview>
            </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Button ID="btnSave" runat="server" Text="Recieve" CssClass="button" 
                ValidationGroup="SAVE" onclick="btnSave_Click" />
                 <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" 
                        ConfirmText="Once Updated can't be change,Are you Sure for Update this PO?" Enabled="True" 
                        TargetControlID="btnSave" ConfirmOnFormSubmit="false" >
                        </asp:ConfirmButtonExtender>
                
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" 
                />
    </div>
    </div>
</asp:Content>