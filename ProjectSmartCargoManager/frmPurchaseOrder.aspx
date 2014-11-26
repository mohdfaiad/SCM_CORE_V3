<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPurchaseOrder.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmPurchaseOrder" Title="Purchase Order"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
       .sdsh
       {
           display:none;
           }
   </style>
      <script type="text/javascript" language="javascript">
          function GetBalanceQty(val) {
              //in val u get dropdown list selected value
              var id = val;
              if (id.indexOf("-") !== -1) {
                  var n = id.split("-");
                  document.getElementById('<%=txtLastAlloc.ClientID%>').innerText = n[1];
               }
              else {
                  document.getElementById('<%=txtLastAlloc.ClientID%>').innerText = "0";
              }
          }

          
//          function InitValidators() {

//              var uldtyp = document.getElementById('<%=ddlAddUldType.ClientID%>');
//              var frm = document.getElementById('<%=txtFrom.ClientID%>')
//              var to = document.getElementById('<%=txtTo.ClientID%>');
//              var uldprt = document.getElementById('<%=ddlULDPart.ClientID%>')
//              var cnt = document.getElementById('<%=txtCount.ClientID%>')
//              alert(9);
//              var RB1 = document.getElementById("<%=rblType.ClientID%>");
//              var radio = RB1.getElementsByTagName("input");
//              var label = RB1.getElementsByTagName("label");
////              for (var i = 0; i < radio.length; i++) 
//              {
//                  if (radio[0].checked) {
//                      var RB2 = document.getElementById("<%=rblSM.ClientID%>");
//                      var radio1 = RB2.getElementsByTagName("input");
//                      var label1 = RB2.getElementsByTagName("label");
////                      for (var j = 0; j < radio1.length; j++)
//                       {
//                          if (radio1[0].checked) {
//                              alert(0);
//                              ValidatorEnable(document.getElementById('<%= RequiredFieldValidator3.ClientID%>'), true);
//                              ValidatorEnable(document.getElementById('<%= RequiredFieldValidator5.ClientID%>'), true);
//                              ValidatorEnable(document.getElementById('<%= RequiredFieldValidator6.ClientID%>'), false);
//                              document.getElementById('<%= RequiredFieldValidator6.ClientID%>').innerHTML = "";
//                          }
//                          else if (radio1[1].checked) {
//                              alert(1);
//                              ValidatorEnable(document.getElementById('<%= RequiredFieldValidator3.ClientID%>'), true);
//                              ValidatorEnable(document.getElementById('<%= RequiredFieldValidator5.ClientID%>'), true);
//                              ValidatorEnable(document.getElementById('<%= RequiredFieldValidator6.ClientID%>'), true);
//                          }
//                      }
//                      ValidatorEnable(document.getElementById('<%= RequiredFieldValidator7.ClientID%>'), false);
//                      ValidatorEnable(document.getElementById('<%= RequiredFieldValidator8.ClientID%>'), false);
//                  }
//                  else if (radio[1].checked) {
//                      ValidatorEnable(document.getElementById('<%= RequiredFieldValidator7.ClientID%>'), true);
//                      ValidatorEnable(document.getElementById('<%= RequiredFieldValidator8.ClientID%>'), true);
//                      ValidatorEnable(document.getElementById('<%= RequiredFieldValidator3.ClientID%>'), false);
//                      ValidatorEnable(document.getElementById('<%= RequiredFieldValidator5.ClientID%>'), false);
//                      ValidatorEnable(document.getElementById('<%= RequiredFieldValidator6.ClientID%>'), false);
//                  }
//              }
//          }
    </script>
   <%-- <script type="text/javascript">
        function hideto() {
            document.getElementById('txtTo').style.display = 'none';
        }

        function showto() {
            document.getElementById('txtTo').style.display = 'block';
        }

        function GetSelectedItem() {
            var RB1 = document.getElementById("<%=rblType.ClientID%>");
            var radio = RB1.getElementsByTagName("input");
            var label = RB1.getElementsByTagName("label");
            for (var i = 0; i < radio.length; i++) {
                alert(label[i].innerHTML);
                if (radio[i].checked) {
                    if (label[i].innerHTML == "ULD Type") {
                        document.getElementById('uldtype').style.display = 'block';
                        document.getElementById('uldpart').style.display = 'none';
                    }
                    else if (label[i].innerHTML == "ULD Part") {
                        document.getElementById('uldtype').style.display = 'none';
                        document.getElementById('uldpart').style.display = 'block';
                    }
                }
               
            }
        }


        </script>--%>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" >
    </asp:ToolkitScriptManager>
    <div id="contentarea">
    <h1>Purchase order</h1>
    
    
    <div class="botline">
    <asp:Label ID="lblerror" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
    <table width="100%">
    <tr>
    <td>Purchase Order #
        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
            ControlToValidate="txtPONo" ErrorMessage="*" ValidationGroup="SEARCH" 
            SetFocusOnError="True"></asp:RequiredFieldValidator>
        </td>
    <td>PO Date</td>
    <td>Manufacturer
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="ddlManu" ErrorMessage="*" InitialValue="0" 
            ValidationGroup="SAVE" SetFocusOnError="True"></asp:RequiredFieldValidator>
        </td>
    <td>Region
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            ControlToValidate="ddlRegion" ErrorMessage="*" InitialValue="0" 
            ValidationGroup="SAVE" SetFocusOnError="True"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
    <td>
        <asp:TextBox ID="txtPONo" runat="server" MaxLength="15"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="List" CssClass="button"
            onclick="btnSearch_Click" ValidationGroup="SEARCH"/>&nbsp;
         <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button"
            ValidationGroup="SEARCH" onclick="btnClear_Click"/>
                    
    
        </td>
    <td>
        <asp:TextBox ID="txtPODate" runat="server" CssClass="inputbgmed"></asp:TextBox>
        <asp:CalendarExtender ID="txtPODate_CalendarExtender" runat="server" Format="MM/dd/yyyy"  
            Enabled="True" TargetControlID="txtPODate">
        </asp:CalendarExtender>
        </td>
    <td>
        <asp:DropDownList ID="ddlManu" runat="server" >
        </asp:DropDownList>
        </td>
    <td>
      <asp:DropDownList ID="ddlRegion" runat="server">
        </asp:DropDownList>
        </td>
    </tr>
    </table>
    </div>
    <div id="rightcol">
    
        <div>
    <h2>Order Details</h2>
    <asp:gridview ID="GVOrdDetails" runat="server" Width="100%" 
            AutoGenerateColumns="False" 
HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="AltRowStyle" 
             PagerStyle-CssClass="PagerStyle" onrowdatabound="GVOrdDetails_RowDataBound" 
                onrowdeleting="GVOrdDetails_RowDeleting" 
                onrowediting="GVOrdDetails_RowEditing" >
<RowStyle CssClass="RowStyle"></RowStyle>
<Columns>
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
<asp:TemplateField HeaderText="ULDs" ItemStyle-HorizontalAlign="Center" ControlStyle-BorderStyle="None">
    <ItemTemplate>
       <asp:GridView ID="grdChildGridNormal"  runat="server" AutoGenerateColumns="False"  OnPageIndexChanging="grdChildGridEdit_PageIndexChanging"
                        AllowPaging="True" AllowSorting="True" CellPadding="3" CellSpacing="1" GridLines="None"  PageSize="5" >
                            <FooterStyle />
                            <Columns>
                            <asp:TemplateField>
                            <ItemTemplate>
<asp:Label ID="chldgrdDelivery" runat="server" Text='<%# Eval("ULDRan") %>'></asp:Label>
                            
                            </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
     </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="ID">
<ItemTemplate>
<asp:Label ID="grdisRec" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
 <asp:ButtonField  CommandName="Delete" Text="Delete">
                        <ItemStyle Width="50px" />
                    </asp:ButtonField>
                    <asp:ButtonField  CommandName="Edit" Text="Edit">
                        <ItemStyle Width="50px" />
                    </asp:ButtonField>
</Columns>

<PagerStyle></PagerStyle>

<HeaderStyle></HeaderStyle>

<AlternatingRowStyle></AlternatingRowStyle>
</asp:gridview>
<asp:Button ID="btnSave" runat="server" Text="SAVE" CssClass="button" ValidationGroup="SAVE"
                onclick="btnSave_Click" />
                 <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" 
                        ConfirmText="Are you Sure for Save this PO ?" Enabled="True" 
                        TargetControlID="btnSave" ConfirmOnFormSubmit="false" >
                        </asp:ConfirmButtonExtender>
                
        <asp:Button ID="btnCancel" runat="server" Text="DELETE" CssClass="button" 
                onclick="btnCancel_Click" />
         <asp:ConfirmButtonExtender ID="ConfirmButtonExtender3" runat="server" 
                        ConfirmText="Are you Sure for Delete this PO ?" Enabled="True" 
                        TargetControlID="btnCancel" ConfirmOnFormSubmit="false" >
                        </asp:ConfirmButtonExtender>
                        
        <asp:Button ID="btnClear1" runat="server" Text="CLEAR" CssClass="button" 
                onclick="btnClear_Click" />
</div>

    </div>
   
    <div id="leftcol">
   <div style="float:right;">
       <asp:LinkButton ID="LBList" runat="server" PostBackUrl="~/frmListPOs.aspx">List Of PO's</asp:LinkButton>
   </div>
    <h2 class="rt" style="width:60%">ADD ULD(s)</h2>
    
    <table width="100%">
    <tr>
    <td>
        <asp:RadioButtonList ID="rblType" runat="server" AutoPostBack="True" 
            onselectedindexchanged="rblType_SelectedIndexChanged" 
            RepeatDirection="Horizontal" >
        <asp:ListItem Selected="True">ULD Type</asp:ListItem>
            <asp:ListItem>ULD Part</asp:ListItem>
        </asp:RadioButtonList>
       
    </td>
    
    </tr>
    
    
    </table>
    <div id="uldtype" runat="server">
    <table width="100%">
    <tr>
    <td colspan="2">
        <asp:RadioButtonList ID="rblSM" runat="server" RepeatDirection="Horizontal" 
            CellSpacing="0" CellPadding="5" AutoPostBack="True" 
            onselectedindexchanged="rblSM_SelectedIndexChanged">
            <asp:ListItem>Single</asp:ListItem>
            <asp:ListItem Selected="True">Multiple</asp:ListItem>
        </asp:RadioButtonList>
    </td>
    </tr>
    <tr>
    <td >ULD Type
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
            ControlToValidate="ddlAddUldType" ErrorMessage="*" InitialValue="0" 
            ValidationGroup="ADD" SetFocusOnError="True"></asp:RequiredFieldValidator>
        </td>
    <td>Last Allocated</td>
    </tr>
        <tr>
    <td >
        <asp:DropDownList ID="ddlAddUldType" runat="server" Width="100px" 
            onselectedindexchanged="ddlAddUldType_SelectedIndexChanged" onchange="GetBalanceQty(this.options[this.selectedIndex].value);">
        </asp:DropDownList>
        </td>
        <td>
            <asp:Label ID="txtLastAlloc" runat="server" Text="0"></asp:Label>
        
            </td>
    </tr>
        <tr>
    <td>
        <asp:Label ID="lblFrom" runat="server" Text="From"></asp:Label>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator5" 
            runat="server" ControlToValidate="txtFrom" ErrorMessage="*" 
            ValidationGroup="ADD" SetFocusOnError="True"></asp:RequiredFieldValidator>
            </td>
    <td>
        <asp:Label ID="lblTo" runat="server" Text="To"></asp:Label>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator6" 
            runat="server" ControlToValidate="txtTo" ErrorMessage="*" 
            ValidationGroup="ADD" SetFocusOnError="True"></asp:RequiredFieldValidator>
            &nbsp;<asp:CompareValidator ID="CompareValidator1" runat="server" 
            ControlToCompare="txtFrom" ControlToValidate="txtTo" ErrorMessage="> than frm" 
            Operator="GreaterThan" SetFocusOnError="True" Type="Double" ValidationGroup="ADD"></asp:CompareValidator>
            </td>
    </tr>
       <tr>
    <td>
        <asp:TextBox ID="txtFrom" runat="server" CssClass="inputbgmed" Width="100px"  
            MaxLength="5" ></asp:TextBox>
        
            <asp:FilteredTextBoxExtender ID="txtFrom_FilteredTextBoxExtender" 
            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFrom" 
            ValidChars="0123456789">
        </asp:FilteredTextBoxExtender>
        
            </td>
    <td>
        <asp:TextBox ID="txtTo" runat="server" CssClass="inputbgmed" Width="100px" 
            MaxLength="5"></asp:TextBox>
        
            <asp:FilteredTextBoxExtender ID="txtTo_FilteredTextBoxExtender" 
            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtTo" 
            ValidChars="0123456789">
        </asp:FilteredTextBoxExtender>
        
            </td>
    </tr>
    </table>
    </div>
    <div id="uldPart" runat="server">
     <table width="100%">
    <tr>
    <td>ULD Part 
        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
            ControlToValidate="ddlULDPart" ErrorMessage="*" InitialValue="0" 
            ValidationGroup="ADD" SetFocusOnError="True"></asp:RequiredFieldValidator>
        </td>
    <td>Count 
        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
            ControlToValidate="txtCount" ErrorMessage="*" ValidationGroup="ADD" 
            SetFocusOnError="True"></asp:RequiredFieldValidator>
        </td>
    </tr>
     <tr>
    <td>
            <asp:DropDownList ID="ddlULDPart" runat="server" Width="100px">
        </asp:DropDownList>
            </td>
    <td>
        <asp:TextBox ID="txtCount" runat="server" CssClass="inputbgmed"  Width="100px" 
            MaxLength="4"></asp:TextBox>
         <asp:FilteredTextBoxExtender ID="txtCount_FilteredTextBoxExtender" 
            runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCount" 
            ValidChars="0123456789">
        </asp:FilteredTextBoxExtender>
         </td>
    </tr>
    </table>
    </div>
    <table width="100%">
     
        <tr>
    <td colspan="2">
        Expected Delivery Date</td>
    </tr>
        <tr>
    <td colspan="2">
        <asp:TextBox ID="txtAddExpDelDate" runat="server" CssClass="inputbgmed" 
            Width="100px" ></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender_txtAddExpDelDate" runat="server" 
            Enabled="True" TargetControlID="txtAddExpDelDate" Format="MM/dd/yyyy">
        </asp:CalendarExtender>
            </td>
    </tr>
        <tr>
    <td colspan="2">
        Delivery WareHouse
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
            ControlToValidate="ddlWH" ErrorMessage="*" InitialValue="0" 
            ValidationGroup="ADD" SetFocusOnError="True"></asp:RequiredFieldValidator>
            </td>
    </tr>
        <tr>
    <td colspan="2">
        <asp:DropDownList ID="ddlWH" runat="server" Width="100px">
        </asp:DropDownList> 
            </td>
    </tr>
        <tr>
    <td colspan="2">
        <asp:Button runat="server" Text="ADD" CssClass="button" ID="btnAdd" ValidationGroup="ADD" 
            onclick="btnAdd_Click"/></td>
    </tr>
    </table>
   
    </div>
  
    
  
</div>
</asp:Content>