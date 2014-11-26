<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserCreation.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.UserCreation" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script language="javascript" type="text/javascript">

        function SelectAllStations() {

            var multi = document.getElementById("<%=chkListStation.ClientID %>");
            var chkBoxCnt = multi.getElementsByTagName("input");

            if (document.getElementById("<%=chkSelectAll.ClientID %>").checked) {
                for (i = 0; i < chkBoxCnt.length; i++) {
                    chkBoxCnt[i].checked = true;
                }
                return false;
            }
            else {
                for (i = 0; i < chkBoxCnt.length; i++) {
                    chkBoxCnt[i].checked = false;
                }
                return false;
            }
            return false;
        }

        function CheckPwd() {
            var regEx = /(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*]).{6,25}$/
            var EmailRegEx = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            var pwd = document.getElementById("<%= txtPassword.ClientID%>").value;
            var Email = document.getElementById("<%= txtEmail.ClientID%>").value;

//            if (!(regEx.test(pwd))) {
//                var error = 'Password must be minimum 6 letters and must contain 1 alphabet,1 Number and One of Special Characters "#?!@$%^&*"';
//                alert(error);
//                return false;
//            }
            if (!(EmailRegEx.test(Email))) {
                var error = 'Enter Valid Email ID';
                alert(error);
                return false;
            }
        }
    </script>
</asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
         CombineScripts="True">
     </asp:ToolkitScriptManager>
     
 <div id="contentarea" >

<h1>User Creation</h1> 
       
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" meta:resourcekey="lblStatusResource1"></asp:Label>

        <div class="divback"> 
          <table width="60%">
          <tr><td><table>
            <tr>
                <td>
                Login ID *
                </td>
                <td>
                    <asp:TextBox ID="txtLoginID" runat="server" Width="110px" 
                        meta:resourcekey="txtLoginIDResource1"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
                        onclick="btnList_Click" />&nbsp;&nbsp;<asp:CheckBox ID="chkIsActive" Text="Active" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                Password *
                </td>
                <td>
                    <asp:TextBox ID="txtPassword"  TextMode="Password"  runat="server" Width="110px" 
                      ToolTip="1 alphabet,1 Number and One of Special Characters '#?!@$%^&*'"   meta:resourcekey="txtPasswordResource1"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="reqPass" runat="server" 
                        ControlToValidate="txtPassword" ErrorMessage="*" ValidationGroup="val"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td>
                User Name *
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" Width="220px" 
                        meta:resourcekey="txtUserNameResource1"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="reqUser" runat="server" 
                        ControlToValidate="txtUserName" ErrorMessage="*" ValidationGroup="val"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td>
                E-Mail ID *
                </td>
                <td>
                    
                    <asp:TextBox ID="txtEmail" runat="server" Width="180px" 
                        meta:resourcekey="txtEmailResource1"></asp:TextBox>
                       <%-- <asp:RequiredFieldValidator ID="reqMail" runat="server" 
                        ControlToValidate="txtEmail" ErrorMessage="*" ValidationGroup="val" 
                        Enabled="False"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="txtEmail" ErrorMessage="Enter valid EmailID" 
                        SetFocusOnError="True" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        Enabled="false" meta:resourcekey="RegularExpressionValidator1Resource1"></asp:RegularExpressionValidator>--%>
                </td>
            </tr>
            <tr>
                <td>
                Role
                </td>
                <td>
                <%--
                    <asp:DropDownList ID="ddlRole" runat="server" Width="200px" 
                        meta:resourcekey="ddlRoleResource1">
                    </asp:DropDownList>--%>
                    <div style="overflow:scroll; width:200px; height:140px;border:solid">
                    <asp:CheckBoxList ID="chkListRole" runat="server" 
                        >
                        </asp:CheckBoxList>
                        
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                Agent Code
                </td>
                <td>
                    <asp:TextBox ID="txtAgentCode" runat="server" AutoPostBack="True" Width="140px" 
                        meta:resourcekey="txtAgentCodeResource1"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="ACEAgentCode" runat="server" 
                        ServiceMethod="GetAllAgentCode" MinimumPrefixLength="1"  Enabled="True"  
                 TargetControlID="txtAgentCode" DelimiterCharacters="" ServicePath="">
                    </asp:AutoCompleteExtender>
                </td>
                
            </tr>
            <tr>
            <td>
                 Mobile No
                </td>
                <td>
                    <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="10"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtMobileNo" ValidChars="0123456789"></asp:FilteredTextBoxExtender>
                    <%--<asp:RequiredFieldValidator ID="ValidatorMobileNo" runat="server" ErrorMessage="*"
                                        ControlToValidate="txtMobileNo" Enabled="False" ValidationGroup="val"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="reqMob" runat="server" 
                        ControlToValidate="txtMobileNo" ErrorMessage="*" ValidationGroup="val" 
                        Enabled="False"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td>Pwd Expires In</td>
                <td>
                    <asp:TextBox ID="txtPwdExpiry" runat="server"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="Filter_txtPwdExpiry" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtPwdExpiry" ValidChars="0123456789"></asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblPwdUpdatedOn" runat="server" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            </table>
            </td>
            <td>
            <table>
            <tr>
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
                 <asp:DropDownList ID ="drLevel" runat="server"  AutoPostBack="true" 
                    onselectedindexchanged="drLevel_SelectedIndexChanged" >
                        <asp:ListItem Value="2">Station</asp:ListItem>
                        <asp:ListItem Value="1">Country</asp:ListItem>
                </asp:DropDownList>
                    <br />
            Station: <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All"
                   OnChange="javascript:SelectAllStations();return false;"
                    />
                   <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
                    </td>
            </tr>
            <tr>
            <td align="center" >
                <DIV style="OVERFLOW-Y:scroll; WIDTH:200px; HEIGHT:140px;border:solid">
                    <asp:CheckBoxList ID="chkListStation" runat="server" 
                        meta:resourcekey="chkListStationResource1">
                        </asp:CheckBoxList>
                        
                </DIV>
                
            </td>
            </tr>
            </table>
            
            </td>
            </tr>
          </table>  
        </div>
         
        <div id="fotbut">
          <table>
              <tr>
                  <td>
                      <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
                        OnClientClick="javascript:return CheckPwd();"  onclick="btnSave_Click" meta:resourcekey="btnSaveResource1"/>
                  </td>
                  <td>
                      <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="button" 
                          onclick="btnUpdate_Click" Visible="False" ValidationGroup="val" />
                  </td>
                  <td>
                      <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="button" Visible="False" />
                  </td>
              </tr>
          </table>
        </div>
 </div>
</asp:Content>