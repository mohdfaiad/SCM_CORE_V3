<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="StockAllocation.aspx.cs" Inherits="ProjectSmartCargoManager.StockAllocation" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
        
    <script type="text/javascript">

      Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
       Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

       function customOpen(url) {
        
           var w = window.open(url, '', 'width=1000,height=600,toolbar=0,status=0,location=0,menubar=0,directories=0,resizable=1,scrollbars=1');
           w.focus();

       }


        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';


        }
        
          function Range() {

                var AWBFrom = document.getElementById("<%=txtAWBFrom.ClientID%>").value;
                var AWBTo = document.getElementById("<%=txtAWBTo.ClientID%>").value;
                var Count = document.getElementById("<%=txtCount.ClientID%>").value;

                if (AWBFrom != "" && Count != "") {
                    AWBTo = parseInt(Count) + (parseInt(AWBFrom) - 1);
                    document.getElementById("<%=txtAWBTo.ClientID%>").value = AWBTo;

                }
                if (AWBTo != "" && AWBFrom != "") {
                    Count = "";
                    if (parseInt(AWBTo) >= parseInt(AWBFrom)) {
                        Count = (parseInt(AWBTo) - parseInt(AWBFrom)) + 1;
                        document.getElementById("<%=txtCount.ClientID%>").value = Count;
                    }
                }
                

            }
        
        function validateAWBFrom() {

            var AWBFrom = document.getElementById("<%=txtAWBFrom.ClientID%>");

            var emailPat = /^([0-9]{7})$/;
            var emailid = AWBFrom.value;

            if (emailPat.test(emailid)) {
              
                Range();
            }
            else {
                alert("Please Enter 7 digit From AWB Number.");
                document.getElementById("<%=txtAWBFrom.ClientID%>").focus();

            }
            return false;
        }
        function validateAWBTo() {
            var AWBTo = document.getElementById("<%=txtAWBTo.ClientID%>");

            var emailPat1 = /^([0-9]{7})$/;
            var emailid1 = AWBTo.value;

            if (emailPat1.test(emailid1)) {
                Range();
            }
            else {
                alert("Please Enter 7 digit To AWB Number.");
                document.getElementById("<%=txtAWBTo.ClientID%>").focus();
            }

            return false;
        }
        function CountEntered() {
            var Count = document.getElementById("<%=txtCount.ClientID%>").value;

            if (Count != 0) {
                Range();
            }
            else {

                alert("Count should be greater than 0");
                document.getElementById("<%=txtCount.ClientID%>").focus();

            }
              return false;
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
       .style1
       {
           width: 121px;
       }
       .style2
       {
           height: 28px;
           width: 9%;
       }
       .style4
       {
           height: 28px;
           width: 139px;
       }
       .style5
       {
           width: 139px;
       }
       .style7
       {
       }
       .style8
       {
           height: 28px;
           width: 3%;
       }
       .style10
       {
           width: 3%;
       }
       .style11
       {
           height: 28px;
           width: 173px;
       }
       .style12
       {
           height: 28px;
           width: 73px;
       }
       .style13
       {
           height: 28px;
           width: 121px;
       }
       .style16
       {
           width: 173px;
       }
       .style17
       {
           width: 73px;
       }
       .style18
       {}
       .style20
       {
           width: 8%;
       }
       .style21
       {
           width: 9%;
       }
       .style22
       {
           width: 137px;
           height: 28px;
       }
       .style23
       {
           width: 137px;
       }
       .style24
       {
           width: 78px;
       }
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl"> 
    <Triggers>
    <asp:PostBackTrigger ControlID="btnExportStock" />
    </Triggers>
    <ContentTemplate>
    
    <div id="contentarea">
      
         <%--<h1> <img alt="" src="Images/txtawbstockallocation.png" /> </h1>--%>
         <h1>AWB Stock Allocation</h1>
       
    <div class="botline">
           <asp:Label ID="lblStatus" runat="server" Text="" Font-Bold="True" Font-Size="Large"></asp:Label>

        <table style="width: 90%; height: 33px;">
            <tr>
                <td>
                    Stock Holder Type*
                </td>
                <td>
                 
                    <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlType_SelectedIndexChanged" Width="100px"
                        >
                        <asp:ListItem>Select</asp:ListItem>
                        <asp:ListItem>HO</asp:ListItem>
                        <asp:ListItem>Region</asp:ListItem>
                        <asp:ListItem>City</asp:ListItem>
                        <asp:ListItem>Agent</asp:ListItem>
                        <asp:ListItem>SubAgent</asp:ListItem>
                    </asp:DropDownList>
                       
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="ddlType" ErrorMessage="*" InitialValue="Select" 
                        ValidationGroup="grpAvail"></asp:RequiredFieldValidator>
                    
                    
                </td>
                <td>
                    Stock Holder Code*&nbsp;
                    </td>
                <td>
                <asp:UpdatePanel runat="server" ID="updddlCode">
                <ContentTemplate>
               
                    <asp:DropDownList ID="ddlCode" runat="server" Width="100px" 
                        ValidationGroup="grpAvail">
                        <asp:ListItem>Select</asp:ListItem>
                    </asp:DropDownList>
                    
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="ddlCode" ErrorMessage="*" InitialValue="Select" 
                        ValidationGroup="grpAvail"></asp:RequiredFieldValidator>
                    
                </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlType"  EventName="selectedindexchanged" />
                </Triggers>
                     </asp:UpdatePanel>     
                </td>
                 <td>
                     &nbsp;Cnote Type&nbsp;
                     </td>
                    <td>
                     <asp:DropDownList ID="ddlCNType" runat="server" 
                        Width="100px" ValidationGroup="grpAvail" 
                        >
                      
                
                   </asp:DropDownList>
                   </td>
       
                <td colspan="2">
                   
                    &nbsp; 
                       <asp:Label ID="lblAWBStatus" runat="server" Font-Bold="True" 
                        Font-Size="12pt"></asp:Label>
                    &nbsp;&nbsp;&nbsp;</td>
            </tr>

            <tr>
                     <td>
                     &nbsp;Stock Type&nbsp;
                     </td>
                
                <td>
                
                 &nbsp;
                    
                    
                    <asp:DropDownList ID="ddlStockAWBtype" runat="server">
                    </asp:DropDownList>
                    
                    
                </td>
              <td>
                    AWB Type
                    </td>
                <td>
                    <asp:DropDownList ID="ddlAWBTypeList" runat="server">
                    <asp:ListItem>All</asp:ListItem>
                    <asp:ListItem Text="Physical" Value="PAWB"></asp:ListItem>
                    <asp:ListItem Text="Electronic" Value="EAWB"></asp:ListItem>
                    
                    </asp:DropDownList>
                </td>

                     <td>
                         AWBNumber:</td>
                     <td>
                         <asp:TextBox ID="txtAWBNo" runat="server" Width="100px" MaxLength="8" 
                             ToolTip="Enter AWB Number"></asp:TextBox>
                     </td>

                <td>
                    <asp:Button ID="btnList" runat="server" CssClass="button" 
                        onclick="btnList_Click" Text="List" />
                    <asp:Button ID="btnClear" runat="server" CssClass="button" 
                        onclick="btnClear_Click" Text="Clear" />
                     <asp:Button ID="btnExportStock" runat="server" Text="Export" CssClass="button" 
                        onclick="btnExportStock_Click" />
                     </td>
                <td>
                    &nbsp;</td>
            </tr>

        </table>
    </div>
   
   <br />
    
    
   
        
   
   <div Class="ltfloat" style="Width:100">
   <h2>
   <asp:Label ID="lblParentLevel" runat="server"></asp:Label>
   </h2>
   
    <asp:GridView ID="grdStockAllocation"  runat="server" AutoGenerateColumns="False"   
           Width="100%" Height="82px" CellSpacing="2" CellPadding="2"   AllowSorting="True" PageSize="10" AllowPaging="true" ShowFooter="True"
            onsorting="grdStockAllocation_Sorting" OnRowCommand="grdStockAllocation_RowCommand" OnPageIndexChanging="grdStockAllocation_PageIndexChanging"  >
    <AlternatingRowStyle ></AlternatingRowStyle>
            <Columns>
                <asp:BoundField DataField="ALevel" HeaderText="Level" />
                <asp:BoundField DataField="APrefix" HeaderText="AWB Prefix" />               
                <asp:BoundField DataField="AFrom" HeaderText="From" />
                <%--<asp:BoundField DataField="APrefix" HeaderText="AWB Prefix" />--%>
                <asp:BoundField DataField="ATo" HeaderText="To" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="Atime" HeaderText="Allocation Time" />
                <asp:BoundField DataField="AUser" HeaderText="Allocated By" />
                <asp:BoundField DataField="Available" HeaderText="Available AWB" />
               <asp:BoundField DataField="Last" HeaderText="Last Allocated" />
               <asp:BoundField DataField="cntype" HeaderText="Cnote Type" />
               <asp:BoundField DataField="AWBstockType" HeaderText="AWB Type" />
               <asp:BoundField DataField="AWBType" HeaderText="P / E AWB" />
              <asp:BoundField DataField="TotAllocated" HeaderText="Allocated" />              
               <asp:ButtonField Text="List" CommandName="List" />
               
                </Columns>
            <HeaderStyle CssClass="titlecolr" />
            <RowStyle HorizontalAlign="Center" />
   </asp:GridView>
    <br/>
    <h2>
        <asp:Label ID="lblChildLevel" runat="server"></asp:Label>
        </h2>
 
     <asp:GridView ID="grdStockAllocationChild" runat="server" 
           AutoGenerateColumns="false"  Width="100%" CellSpacing="2" CellPadding="2" 
          
           OnRowCommand="grdStockAllocationChild_RowCommand" OnPageIndexChanging="grdStockAllocationChild_PageIndexChanging" AllowSorting="True" PageSize="10" AllowPaging="true" ShowFooter="True">
    <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle >
    
    <Columns>
  <asp:TemplateField>
               
                <HeaderTemplate>
            <asp:ImageButton ID="grdStockAllocationChildShowHide" runat="server"  ImageUrl="~/Images/plus.gif" OnClick="grdStockAllocationChild_Show" Visible="false"
            OnRowCommand="grdStockAllocationChild_RowCommand" />
            </HeaderTemplate>
            </asp:TemplateField>

                 <asp:BoundField DataField="ALevel" HeaderText="Level" />
                 <asp:BoundField DataField="APrefix" HeaderText="AWB Prefix" />
                  <asp:BoundField DataField="AFrom" HeaderText="From" />
                  <%--<asp:BoundField DataField="APrefix" HeaderText="AWB Prefix" />--%>
                  <asp:BoundField DataField="ATo" HeaderText="To" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="Atime" HeaderText="Allocation Time" />
                <asp:BoundField DataField="AUser" HeaderText="Allocated By" />
                <asp:BoundField DataField="Available" HeaderText="Available AWB" />
                 <asp:BoundField DataField="Last" HeaderText="Last Allocated" />
               <asp:BoundField DataField="cntype" HeaderText="Cnote Type" />
                <asp:BoundField DataField="AWBstockType" HeaderText="AWB Type" />
                <asp:BoundField DataField="AWBType" HeaderText="P / E AWB" />
              <asp:BoundField DataField="TotAllocated" HeaderText="Allocated" />
              
                  <asp:ButtonField Text="List" CommandName="List" />
                </Columns>
            <HeaderStyle CssClass="titlecolr" />
            <RowStyle HorizontalAlign="Center" />
    </asp:GridView>
    <br/>
   
     <fieldset>  <legend style=" font-weight:bold;  color:#999999;"  xml:lang="">Allocation</legend>
  <table width="100%">
  <tr>
  <td class="style22">
      Stock Holder Type*
  </td>
   <td class="style13">
       <asp:DropDownList ID="ddlTypeAllocation" runat="server" AutoPostBack="True" 
           onselectedindexchanged="ddlTypeAllocation_SelectedIndexChanged" 
           ValidationGroup="grpAlloc" Width="100px">
           <asp:ListItem>Select</asp:ListItem>
           <asp:ListItem>HO</asp:ListItem>
           <asp:ListItem>Region</asp:ListItem>
           <asp:ListItem>City</asp:ListItem>
           <asp:ListItem>Agent</asp:ListItem>
           <asp:ListItem>SubAgent</asp:ListItem>
       </asp:DropDownList>
       <asp:RequiredFieldValidator ID="StkTypeRequired" runat="server" 
           ControlToValidate="ddlTypeAllocation" ErrorMessage="*" 
           InitialValue="Select" ValidationGroup="grpAlloc" ></asp:RequiredFieldValidator>
  </td>
   <td class="style4">
       Stock Holder Code*  
  </td>
   <td class="style11">
    <asp:UpdatePanel runat="server" ID="UpdateddlCodeAllocation">
                <ContentTemplate>
                    <asp:DropDownList ID="ddlCodeAllocation" runat="server" Width="100px"  
                        ValidationGroup="grpAlloc" 
                        onselectedindexchanged="ddlCodeAllocation_SelectedIndexChanged" 
                        AutoPostBack="True">
                        <asp:ListItem>Select</asp:ListItem>
                    </asp:DropDownList>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ErrorMessage="*" ValidationGroup="grpAlloc" 
                        ControlToValidate="ddlCodeAllocation" InitialValue="Select"></asp:RequiredFieldValidator>
                    </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTypeAllocation"  EventName="selectedindexchanged" />
                </Triggers>
                     </asp:UpdatePanel>   
  </td>
  
   <td class="style12">
   
       AWB Type</td>
  
   <td class="style13">
   <asp:UpdatePanel runat="server" ID="UpdateAWBtype">
                <ContentTemplate>
       <asp:DropDownList ID="ddlAWBtype" runat="server" Enabled="false" AutoPostBack="False" 
                        onselectedindexchanged="ddlAWBtype_SelectedIndexChanged">
           <asp:ListItem>Physical</asp:ListItem>
           <asp:ListItem>Electronic</asp:ListItem>
       </asp:DropDownList>
       </ContentTemplate>
       <Triggers>
       <asp:AsyncPostBackTrigger ControlID="ddlTypeAllocation" EventName="selectedindexchanged" />
       </Triggers>
       </asp:UpdatePanel>
      </td>
<td class="style24">
                     &nbsp;Stock Type&nbsp;
                     </td>
                
                <td>
                
                    
                    <asp:DropDownList ID="ddlStockAWBtypeAllocation" runat="server" 
                        AutoPostBack="false" 
                        onselectedindexchanged="ddlStockAWBtypeAllocation_SelectedIndexChanged">
                    </asp:DropDownList>
                    
                    
                </td>
  
  
   <td class="style8">
       Count</td>
  
   <td class="style2">
   <asp:UpdatePanel runat="server" ID="UpdatetxtCount">
                <ContentTemplate>
       <asp:TextBox ID="txtCount" runat="server" Width="85px" onchange="CountEntered()" ontextchanged="txtCount_TextChanged" 
                        AutoPostBack="false"></asp:TextBox>
            
                    </ContentTemplate>
                <Triggers>
              <%--  <asp:AsyncPostBackTrigger ControlID="txtAWBFrom"  EventName="textchanged" />
                <asp:AsyncPostBackTrigger ControlID="txtAWBTo"  EventName="textchanged" />
--%>
                </Triggers>
                     </asp:UpdatePanel>
      
      </td>
  
  </tr>
  
  <tr>
  <td class="style23">
                   
                   &nbsp;Cnote Type&nbsp;</td>
   <td class="style1">
    
    <asp:DropDownList ID="ddlCNTypeAllocate" runat="server" AutoPostBack="false" 
                         Width="100px" 
           onselectedindexchanged="ddlCNTypeAllocate_SelectedIndexChanged"  
                        >
                        
                       
                
                   </asp:DropDownList>
  </td>
   <td class="style5">
                   <asp:Label ID="lblAWBFrom0" runat="server" Text="AWB From"></asp:Label>
      </td>
   <td class="style16">
   
       <asp:UpdatePanel ID="UpdatePanelStock" runat="server">
           <ContentTemplate>
               <asp:TextBox ID="txtAWBFrType" runat="server" width="30px"></asp:TextBox>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                        ErrorMessage="*" ControlToValidate="txtAWBFrType" ></asp:RequiredFieldValidator>
              
               <asp:TextBox ID="txtAWBFrom" runat="server" AutoPostBack="true" MaxLength="7" 
                   onchange="validateAWBFrom()" ontextchanged="txtAWBFrom_TextChanged" Width="85px"></asp:TextBox>
                    
               <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtAWBFrom" 
                   runat="server" ControlToValidate="txtAWBFrom" ErrorMessage="*" 
                   ValidationExpression="\d{7}"></asp:RegularExpressionValidator>
           </ContentTemplate>
       </asp:UpdatePanel>
   
  </td>
  <td class="style17">
      
                      <asp:Label ID="lblAWBTo" runat="server" Text="AWB To"></asp:Label>
      
                      </td>
  
   <td class="style18" colspan="2">
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>
               <asp:TextBox ID="txtAWBToType" runat="server" width="30px"></asp:TextBox>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                        ErrorMessage="*" ControlToValidate="txtAWBToType" ></asp:RequiredFieldValidator>
              
               <asp:TextBox ID="txtAWBTo" runat="server" AutoPostBack="false" MaxLength="7" 
                   onchange="validateAWBTo()" ontextchanged="txtAWBTo_TextChanged" width="85px"></asp:TextBox>
               <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtAWBTo" 
                   runat="server" ControlToValidate="txtAWBTo" ErrorMessage="*" 
                   ValidationExpression="\d{7}"></asp:RegularExpressionValidator>
               <asp:CompareValidator ID="CompareValidator1" runat="server" 
                   ControlToCompare="txtAWBFrType" ControlToValidate="txtAWBToType" 
                   ErrorMessage="*"></asp:CompareValidator>
               <%-- <asp:CompareValidator ID="CompareValidator2" runat="server" 
                         ControlToCompare="txtAWBFrom" ControlToValidate="txtAWBTo" Operator="LessThan" 
                         ErrorMessage="*"  ></asp:CompareValidator>
                         <asp:BaseCompareValidator Type="ValidationDataType" />--%>
           </ContentTemplate>
           <Triggers>
               <%-- Mod 7 Jun 2013--%>
               <%--
                <asp:AsyncPostBackTrigger  ControlID="cbAWBTypeInt" EventName="checkedchanged"/>
                <asp:AsyncPostBackTrigger  ControlID="cbAWBTypeDom" EventName="checkedchanged"/>--%>
               <%--<asp:AsyncPostBackTrigger ControlID="txtCount" EventName="textchanged" />--%>
           </Triggers>
       </asp:UpdatePanel>
      </td>
  
                       <td class="style20">
                           &nbsp;</td>
                      <td class="style10">
                          &nbsp;</td> 
                      <td class="style21">
                          &nbsp;</td>
  
  </tr>
      <tr>
          <td class="style7" colspan="9">
              &nbsp;<asp:Button ID="btnAllocateAWB" runat="server" CssClass="button" 
                  onclick="btnAllocateAWB_Click" Text="Allocate" ValidationGroup="grpAlloc" />
              &nbsp;&nbsp;&nbsp;&nbsp;
              <asp:Button ID="btnBlackListAWB" runat="server" CssClass="button" 
                  onclick="btnBlackListAWB_Click" Text="Blacklist" ValidationGroup="grpAlloc" />
              &nbsp;&nbsp;&nbsp;&nbsp;
              <asp:Button ID="btnReturnAWB" runat="server" CssClass="button" 
                  onclick="btnReturnAWB_Click" Text="Return" ValidationGroup="grpAlloc" />
              &nbsp;&nbsp;&nbsp;&nbsp;
              <asp:Button ID="btnRevokeAWB" runat="server" CssClass="button" 
                  onclick="btnRevokeAWB_Click" Text="Revoke" ValidationGroup="grpAlloc" />
          </td>
      </tr>
  </table>
  
        
           </fieldset>
    <fieldset>
     <legend  id="AWBHistoryDetails" style=" font-weight:bold;   color:#999999; padding:5px;" xml:lang="">History
        </legend>
         
       <table width="100%">
            <tr>
           
                <td >
                    Stock Holder Type*</td>
                <td >
                    <asp:DropDownList ID="ddlTypeHistory" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlTypeHistory_SelectedIndexChanged" Width="100px" 
                        ValidationGroup="grpHist">
                        <asp:ListItem>Select</asp:ListItem>
                        <asp:ListItem>HO</asp:ListItem>
                        <asp:ListItem>Region</asp:ListItem>
                        <asp:ListItem>City</asp:ListItem>
                        <asp:ListItem>Agent</asp:ListItem>
                        <asp:ListItem>SubAgent</asp:ListItem>
                    </asp:DropDownList>
                       
                   
       <asp:RequiredFieldValidator ID="StkTypeRequired0" runat="server" 
           ControlToValidate="ddlTypeHistory" ErrorMessage="*" 
           InitialValue="Select" ValidationGroup="grpHist" Enabled="false"></asp:RequiredFieldValidator>
                       
                   
                </td>
                <td>
                    Stock Holder Code*</td>
                <td>
                   <asp:UpdatePanel runat="server" ID="UpdateddlCodeHistory">
                   <ContentTemplate>
                    <asp:DropDownList ID="ddlCodeHistory" runat="server" Width="100px" ValidationGroup="grpHist">
                        <asp:ListItem>Select</asp:ListItem>
                    </asp:DropDownList>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                        ErrorMessage="*" ValidationGroup="grpHist" 
                        ControlToValidate="ddlCodeHistory" InitialValue="Select" Enabled="false"></asp:RequiredFieldValidator>
                      </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTypeHistory"  EventName="selectedindexchanged" />
                </Triggers>
                     </asp:UpdatePanel>   
                    </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="From"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtDateFrom" runat="server" Width="85px"></asp:TextBox>  
                    <asp:CalendarExtender ID="txtDateFrom_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy" 
                        >
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="To"></asp:Label>   </td>
                <td>
                
                    <asp:TextBox ID="txtDateTo" runat="server" Width="85px"></asp:TextBox>   
                    <asp:CalendarExtender ID="txtDateTo_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy"  >
                    </asp:CalendarExtender>
                </td>
                </tr>
                <tr>
                <td>
                    AWB No</td>
                <td>
                    <asp:TextBox ID="txtAWBNumber" runat="server" MaxLength="8" Width="100px"></asp:TextBox>
                </td>
                <td>
                AWB From
                </td>
                <td>
                    <asp:TextBox ID="txtFromAWB" runat="server" MaxLength="8" Width="100px"></asp:TextBox>
                </td>
                <td>
                AWB To
                </td>
                <td>
                <asp:TextBox ID="txtToAWB" runat="server" MaxLength="8" Width="100px"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    
                    &nbsp;</td>
                 <td>
                     &nbsp;</td>
            </tr>

            <tr>
                <td colspan="9">
                    <asp:Button ID="btnListHistory" runat="server" CssClass="button" 
                        onclick="btnListHistory_Click" Text="List" ValidationGroup="grpHist" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnClearHistory" runat="server" CssClass="button" 
                        onclick="btnClearHistory_Click" Text="Clear" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnPrintHistory" runat="server" CssClass="button" 
                        Enabled="False" OnClientClick="javascript:CallPrint('divPrint')" text="Print" 
                        Visible="False" onclick="btnPrintHistory_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnExport" runat="server" CssClass="button" 
                        onclick="GenerateHistoryReport" Text="Print" ValidationGroup="grpHist" Visible="false" />
                </td>
            </tr>

        </table>
       <div id="divPrint">
       
           
         <asp:GridView ID="GridViewHistory" runat="server" AutoGenerateColumns="false"   Width="100%"  CellSpacing="2" CellPadding="2"
         OnPageIndexChanging="GridViewHistory_PageIndexChanging" AllowSorting="True" PageSize="10" AllowPaging="true" ShowFooter="True">
    <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
    
            <Columns>
            
                <asp:BoundField DataField="ALevel" HeaderText="Level" />
                <asp:BoundField DataField="AFrom" HeaderText="From" />
                <asp:BoundField DataField="ATo" HeaderText="To" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="Atime" HeaderText="Allocation Time" />
                <asp:BoundField DataField="AUser" HeaderText="Allocated By" />
                <asp:BoundField DataField="Available" HeaderText="Available AWB" />
                
                </Columns>
            <HeaderStyle CssClass="titlecolr" />
            <RowStyle HorizontalAlign="Center" />
    </asp:GridView>
        </div>
                
    </fieldset>
    
    
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
