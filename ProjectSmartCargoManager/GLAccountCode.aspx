<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="GLAccountCode.aspx.cs" Inherits="ProjectSmartCargoManager.GLAccountCode" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style5
        {
            width: 108px;
        }
        .style6
        {
            width: 88%;
        }
    </style>
    <script type="text/javascript">
        function onListPopulated() {

            var completionList = $find("Freight").get_completionList();
            
            completionList.style.width = 'auto';


        }
        function onListPopulated1() {

            var completionList = $find("AutoCompleteExtender1").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated2() {

            var completionList = $find("AutoCompleteExtender2").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated3() {

            var completionList = $find("AutoCompleteExtender3").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated4() {

            var completionList = $find("AutoCompleteExtender4").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated5() {

            var completionList = $find("AutoCompleteExtender5").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated6() {

            var completionList = $find("AutoCompleteExtender6").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated7() {

            var completionList = $find("AutoCompleteExtender7").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated8() {

            var completionList = $find("AutoCompleteExtender8").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated9() {

            var completionList = $find("AutoCompleteExtender9").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated10() {

            var completionList = $find("AutoCompleteExtender10").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated11() {

            var completionList = $find("AutoCompleteExtender11").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated12() {

            var completionList = $find("AutoCompleteExtender12").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated13() {

            var completionList = $find("AutoCompleteExtender13").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated14() {

            var completionList = $find("AutoCompleteExtender14").get_completionList();

            completionList.style.width = 'auto';


        }
        function onListPopulated15() {

            var completionList = $find("AutoCompleteExtender15").get_completionList();

            completionList.style.width = 'auto';


        }
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
  <h1></h1>
  <h2></h2>
          <div class="botline">
            <table width="100%">
            <tr>
            <td colspan="2"><h3>GL Account Configuration</h3></td>
            <td></td>
            </tr>
                <tr>
                    <td class="style5">
                        &nbsp;Freight </td>
                    <td class="style6">
                        <asp:TextBox ID="ddlfreight" runat="server" >                        
                        </asp:TextBox> </td>
                    <asp:AutoCompleteExtender ID="Freight" BehaviorID="Freight" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlfreight" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated">
                                        </asp:AutoCompleteExtender>
                </tr>
                <tr>
                    <td class="style5">
                        OCDC (Due Carrier)</td>
                    <td class="style6">
                        <asp:TextBox ID="ddlOCDC" runat="server">                        
                        </asp:TextBox> <asp:AutoCompleteExtender ID="AutoCompleteExtender1" BehaviorID="AutoCompleteExtender1" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlOCDC" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated1">
                                        </asp:AutoCompleteExtender></td>
                   
                </tr>
               <tr>
           <td colspan="2">
           <table>
           <tr>
           <td style="width:50px;">
           &nbsp;
           </td>
           <td>
               Airport Charges
           </td><td>
           <asp:TextBox ID="ddlAirport" runat="server"></asp:TextBox>
           <asp:AutoCompleteExtender ID="AutoCompleteExtender2" BehaviorID="AutoCompleteExtender2" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlAirport" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated2">
                                        </asp:AutoCompleteExtender>
           </td>
           </tr>
            <tr>
           <td>
           &nbsp;
           </td>
           <td>
               Security Surcharge            </td><td>
           <asp:TextBox ID="ddlSecuritySur" runat="server"></asp:TextBox>
           <asp:AutoCompleteExtender ID="AutoCompleteExtender3" BehaviorID="AutoCompleteExtender3" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlSecuritySur" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated3">
                                        </asp:AutoCompleteExtender>
           </td>
           </tr>
            <tr>
           <td>
           &nbsp;
           </td>
           <td>
           Fuel Surcharge
           </td><td>
           <asp:TextBox ID="ddlFuelSur" runat="server"></asp:TextBox>
           <asp:AutoCompleteExtender ID="AutoCompleteExtender4" BehaviorID="AutoCompleteExtender4" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlFuelSur" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated4">
                                        </asp:AutoCompleteExtender>
           </td>
           </tr>
            <tr>
           <td>
               &nbsp;
           </td>
           <td>
               Cartage
           </td><td>
           <asp:TextBox ID="ddlCartage" runat="server"></asp:TextBox>
           <asp:AutoCompleteExtender ID="AutoCompleteExtender5" BehaviorID="AutoCompleteExtender5" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlCartage" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated5">
                                        </asp:AutoCompleteExtender>
           </td>
           </tr>
            <tr>
           <td>
           &nbsp;
           </td>
           <td>
               Screening Charges</td><td>
           <asp:TextBox ID="ddlScreenig" runat="server"></asp:TextBox>
           <asp:AutoCompleteExtender ID="AutoCompleteExtender6" BehaviorID="AutoCompleteExtender6" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlScreenig" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated6">
                                        </asp:AutoCompleteExtender>
                                        
           </td>
           </tr>
            <tr>
           <td>
           &nbsp;
           </td>
           <td>
               Misc
           </td><td>
           <asp:TextBox ID="ddlMiscCharge" runat="server"></asp:TextBox>
           <asp:AutoCompleteExtender ID="AutoCompleteExtender7" BehaviorID="AutoCompleteExtender7" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlMiscCharge" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated7">
                                        </asp:AutoCompleteExtender>
                                        
           </td>
           </tr>
           </table>
           </td>
           </tr>
                <tr>
                    <td class="style5">
                        OCDA (Due Agent)<td class="style6">
                        <asp:TextBox ID="ddlOCDA" runat="server">                        
                        </asp:TextBox> 
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender8" BehaviorID="AutoCompleteExtender8" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlOCDA" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated8">
                                        </asp:AutoCompleteExtender></td>
                   
                </tr>
                <tr>
                    <td class="style5">
                        Comission</td>
                    <td class="style6">
                        <asp:TextBox ID="ddlComission" runat="server">                        
                        </asp:TextBox><asp:AutoCompleteExtender ID="AutoCompleteExtender9" BehaviorID="AutoCompleteExtender9" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlComission" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated9">
                                        </asp:AutoCompleteExtender> </td>
                   
                </tr>
                <tr>
                    <td class="style5">
                        Discount</td>
                    <td class="style6">
                        <asp:TextBox ID="ddlDiscount" runat="server">                        
                        </asp:TextBox><asp:AutoCompleteExtender ID="AutoCompleteExtender10" BehaviorID="AutoCompleteExtender10" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlDiscount" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated10">
                                        </asp:AutoCompleteExtender> </td>
                   
                </tr>
                <tr>
                    <td class="style5">
                        Service Tax</td>
                    <td class="style6">
                        <asp:TextBox ID="ddlServiceTax" runat="server">                        
                        </asp:TextBox><asp:AutoCompleteExtender ID="AutoCompleteExtender11" BehaviorID="AutoCompleteExtender11" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlServiceTax" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated11">
                                        </asp:AutoCompleteExtender></td>
                   
                </tr>
               <tr>               
                <td colspan="2">
                  <table>
           <tr>
           <td style="width:50px">
           &nbsp;
           </td>
           <td >
               Service Tax on Freight            </td><td>
           <asp:TextBox ID="ddlTaxFreight" runat="server"></asp:TextBox>
           <asp:AutoCompleteExtender ID="AutoCompleteExtender12" BehaviorID="AutoCompleteExtender12" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlTaxFreight" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated12">
                                        </asp:AutoCompleteExtender>
           </td>
           </tr>
            <tr>
           <td>
           &nbsp;
           </td>
           <td >
               Tax on Commission            </td><td>
           <asp:TextBox ID="ddlTaxCommission" runat="server"></asp:TextBox>
           <asp:AutoCompleteExtender ID="AutoCompleteExtender13" BehaviorID="AutoCompleteExtender13" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlTaxCommission" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated13">
                                        </asp:AutoCompleteExtender>
           </td>
           </tr>
            <tr>
           <td>
           &nbsp;
           </td>
           <td>
               Tax on Discount
           </td><td>
           <asp:TextBox ID="ddlTaxDiscount" runat="server"></asp:TextBox>
           <asp:AutoCompleteExtender ID="AutoCompleteExtender14" BehaviorID="AutoCompleteExtender14" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlTaxDiscount" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated14">
                                        </asp:AutoCompleteExtender>
           </td>
           </tr>
            </table>
                </td>
                </tr>
                <tr>
                    <td class="style5">
                        TDS</td>
                    <td class="style6">
                        <asp:TextBox ID="ddlTDS" runat="server">                        
                        </asp:TextBox><asp:AutoCompleteExtender ID="AutoCompleteExtender15" BehaviorID="AutoCompleteExtender15" runat="server"
                                            ServiceMethod="GetGLCode" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="ddlTDS" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated15">
                                        </asp:AutoCompleteExtender> </td>
                   
                </tr>            
                <tr>
                <td class="style5"></td>
                   <td class="style6">
                    
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
                            onclick="btnSave_Click" />&nbsp;
                        <asp:Button ID="btnList" runat="server" Text="List" CssClass="button"  Visible="false"/>&nbsp;
                        <asp:Button ID="btnclear" runat="server" Text="Clear"  CssClass="button" 
                            onclick="btnclear_Click"/>                        
                        
                        </td>
                    
                    
                    
                </tr>                
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
             ForeColor="Red"></asp:Label></td>
                    
                </tr>
           </table>
    
   
    
    </div>
</asp:Content>
