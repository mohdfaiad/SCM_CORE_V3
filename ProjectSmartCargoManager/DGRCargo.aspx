<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DGRCargo.aspx.cs" Inherits="ProjectSmartCargoManager.DGRCargo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
      <script type="text/javascript">

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
         function closePage()
          {
             window.close();
         }
            
        
    </script>
    <link href="style/style.css" rel="stylesheet" type="text/css" />
</head>
<body class="divback">

    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div>
    
        <asp:Label ID="LBLStatus" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
        <br />
        <%--<div id="contentarea" >--%>
        <table width="30%">
         <tr>
          <td>
             <strong> <asp:Label ID="AWBNumber" runat="server" Text="AWB Number"></asp:Label></strong>
          </td>
          <td>
             <strong><asp:Label ID="lblAWBNumber" runat="server" ></asp:Label></strong>
          </td>
          <td>
          &nbsp;&nbsp;
          <asp:Button ID="btnclear" runat="server" CssClass="button" Text="Clear" 
                  onclick="btnclear_Click"/>
          </td>
         </tr>
        </table> 
        <%--</div>--%>
     
        
        
          <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">DGR</legend>
           
                 <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" 
                          ID="grdDGRCargo"                                    
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
                        
                        <asp:TemplateField HeaderText="UNID"  ControlStyle-Width="250"><ItemTemplate>
                         <asp:DropDownList ID="ddlUNID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUNID_SelectedIndexChanged">
                            </asp:DropDownList>
                            <%-- <asp:TextBox ID="txtUNID" runat="server"  Text='<%# Eval("UNID") %>' OnTextChanged="GetUNID" AutoPostBack="true"  ></asp:TextBox>
                             
                            <asp:AutoCompleteExtender ID="txtUNID_AutoCompleteExtender" runat="server" EnableCaching="true"
                                DelimiterCharacters="" Enabled="True" ServiceMethod="GetUnidNo" MinimumPrefixLength="1" ServicePath="~/DGRCargo.aspx" TargetControlID="txtUNID">
                            </asp:AutoCompleteExtender>--%>
                             
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Description" ControlStyle-Width="250">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDescr" runat="server" Text='<%# Eval("Description") %>' ReadOnly="true"></asp:TextBox>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>      
                        
                        <asp:TemplateField HeaderText="Pieces"  ControlStyle-Width="80">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPCS" runat="server" Text='<%# Eval("Pieces") %>'></asp:TextBox>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>      
                        
                        <asp:TemplateField HeaderText="Weight" ControlStyle-Width="80">
                        <ItemTemplate>
                            <asp:TextBox ID="txtWeight" runat="server" Text='<%# Eval("Weight") %>'></asp:TextBox>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="ERG Code"  ControlStyle-Width="80">
                        <ItemTemplate>
                            <asp:TextBox ID="txtERGCode" runat="server" Text='<%# Eval("ERGCode") %>'></asp:TextBox>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Packing Group" ControlStyle-Width="80">
                        <ItemTemplate>
                           <asp:DropDownList ID="ddlPG" runat="server"></asp:DropDownList>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>
                       <%-- <asp:TemplateField HeaderText="Flight Number">
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
                        --%>
                        </Columns>

                        <EditRowStyle CssClass="grdrowfont"></EditRowStyle>

                      
                     <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
              
             <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add" 
                 onclick="btnAdd_Click"/>
             &nbsp;<asp:Button ID="btnDelete" runat="server" CssClass="button" Text="Delete" 
                 onclick="btnDelete_Click"/>
          </fieldset> 
             
           <br />
        &nbsp;&nbsp;
        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" 
            onclick="btnSave_Click"/>
            &nbsp;<asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" OnClientClick="javascript:closePage()" />
        
       
    </div>
    </form>
</body>
</html>
