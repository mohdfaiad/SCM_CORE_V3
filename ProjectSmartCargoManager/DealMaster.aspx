<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealMaster.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master"  Inherits="ProjectSmartCargoManager.DealMaster" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function SelectAllgrdAddRate(CheckBoxControl) {
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if (document.forms[0].elements[i].name.indexOf('check') > -1) {
                    document.forms[0].elements[i].checked = CheckBoxControl.checked;
                }
            }
        }

            
        
        
   </script>   

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
    
    <h1><img src="Images/txt_agent.png" />
    <br />
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
             ForeColor="Red"></asp:Label>
     </h1>
     
     <div class="botline">
     <table  style="width:50%">
        <tr>
         <td>
         Deal ID
         </td>
         <td>
             <asp:TextBox ID="txtDealid" runat="server" Enabled="False"></asp:TextBox>
             <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
                 ToolTip="Clear All" onclick="btnClear_Click"/>
         </td>
         <td>
           <%--<asp:Button ID="btnList" runat="server" Text="List" CssClass="button"/>--%>
           &nbsp;</td>
         
        </tr>
     </table> 
    </div>
     <div>
      <br /> <br /> 
      <h2>Agent Details</h2>
    <table width="60%">
     <tr>
      <td>
       Agent Code
      </td>
      <td>
          <asp:DropDownList ID="ddlAgentCode" runat="server" Width="95px" >
          </asp:DropDownList>
      </td>
      
      <td>
       Applicabe From
      </td>
      <td>
          
          &nbsp;<asp:TextBox ID="txtaplicablefrom" runat="server" Width="70px"></asp:TextBox>
           <asp:CalendarExtender ID="txtaplicablefrom_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtaplicablefrom" Format="dd-MM-yyyy" >
          </asp:CalendarExtender>
         
      </td>
      
       <td>
       Applicabe To
      </td>
      <td>
          
          <asp:TextBox ID="txtApplicableto" runat="server" Width="70px"></asp:TextBox>
          <asp:CalendarExtender ID="txtApplicableto_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtApplicableto" Format="dd-MM-yyyy" >
          </asp:CalendarExtender>
      </td>
     <%-- <td>
       <asp:Button ID="BtnList" runat="server" Text="List" CssClass="button" 
              onclick="BtnList_Click" />
       &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
              ToolTip="Clear All" onclick="btnClear_Click"/>
      </td>--%>
     </tr>
     </table> 
    </fieldset> 
    </div>
     <div>
     <div>
          <table width="100%">
    <tr>
     <td>
         <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Tonnage</legend>  
         <table width="100%">
          <tr>
           <td>
             Origin Type
           </td>
           <td>
               <asp:DropDownList ID="ddlorigintype" runat="server" AutoPostBack="true"  
                   onselectedindexchanged="ddlorigintype_SelectedIndexChanged">
                   <asp:ListItem Text="Select"></asp:ListItem>  
                <asp:ListItem Text="Airport"  Value="A"></asp:ListItem>
                                    <asp:ListItem Text="City" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="N"></asp:ListItem>
               </asp:DropDownList>
           </td>
           <td>
            Origin
           </td>
           <td>
              <%-- <asp:TextBox ID="txtorigin" runat="server"></asp:TextBox>
               <asp:AutoCompleteExtender ID="txtorigin_AutoCompleteExtender" runat="server" 
                   TargetControlID="txtorigin" ServicePath="~/Home.aspx" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True"  >
               </asp:AutoCompleteExtender>--%>
               
               <asp:DropDownList ID="ddlOrigin" runat="server">
               <asp:ListItem Selected="True" Text="Select"></asp:ListItem>  
               </asp:DropDownList>
           </td>
            <td>
         Destination Type
        </td>
       <td>
           <asp:DropDownList ID="ddldestinationType" runat="server" AutoPostBack="true" 
               onselectedindexchanged="ddldestinationType_SelectedIndexChanged" >
               <asp:ListItem Text="Select" Selected="True"></asp:ListItem>    
                <asp:ListItem Text="Airport" Value="A"></asp:ListItem>
                <asp:ListItem Text="City" Value="C"></asp:ListItem>
                <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                <asp:ListItem Text="Country" Value="N"></asp:ListItem>
           </asp:DropDownList>
       </td>
       <td>
        Destination
       </td>
       <td>
          <%-- <asp:TextBox ID="txtdestination" runat="server"></asp:TextBox>--%>
           <asp:DropDownList ID="ddlDestination" runat="server">
           <asp:ListItem Selected="True" Text="Select"></asp:ListItem>  
           </asp:DropDownList>
       </td>
       <td>
       Flight Number
       </td>
       <td>
       <asp:DropDownList ID="ddlFlightNumber" runat="server">
       <asp:ListItem Selected="True" Text="Select"></asp:ListItem> 
       </asp:DropDownList>
            
       </td>
       
       <td>
       Commodity
       </td>
       <td>
       <asp:DropDownList ID="ddlCommodity" runat="server">
       </asp:DropDownList>
       
       </td>
          </tr>
          
         </table> 
         
          </fieldset>  
     </td>
     <td>
         &nbsp;</td>
    </tr>
   </table> 
     </div>
    </div>
    
     <div  style="overflow:auto">   
     <table width="60%">
     <tr>
     
     <td>
     <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" 
                          ID="grdTonnage"                                    
                                      Width="100%" CssClass="grdrowfont" PageSize="4" 
             AllowPaging="True" onselectedindexchanging="grdTonnage_SelectedIndexChanging" > 
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
                        
                        <asp:TemplateField HeaderText="Tonnage"><ItemTemplate>
                            <%--<asp:DropDownList ID="ddlUNID" runat="server">
                            </asp:DropDownList>--%>
                             <asp:TextBox ID="txtTonnage" runat="server" Width="95px"   ></asp:TextBox>
                          
                   <%--          Text='<%# Eval("UNID") %>'--%>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRate" runat="server" Width="95px"></asp:TextBox>
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
     </td>
     
     <td valign=top style="width:150px;">
        
             <asp:Button ID="btnAdd0" runat="server" CssClass="button" 
                 onclick="btnAdd_Click" Text="Add" />
             &nbsp;
             <asp:Button ID="btnDelete0" runat="server" CssClass="button" 
                 onclick="btnDelete_Click" Text="Delete" />
         
         </td>
     
     </tr>
     </table> 
    </div> 
     <div>
      <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">Parameters</legend>
       <table width="90%" cellpadding=6 cellspacing=6>
       
       <tr><td>
       <table width="80%" cellpadding=6 cellspacing=3>
       <tr><td>Slab Of Rate</td><td><asp:CheckBox ID="chkM" runat="server" Text="M"  />&nbsp; &nbsp;&nbsp;<asp:CheckBox ID="chkN" runat="server" Text="N"  /> &nbsp; &nbsp;&nbsp;<asp:CheckBox ID="chkA" runat="server" Text="+45"  /> &nbsp; &nbsp;&nbsp;<asp:CheckBox ID="chkB" runat="server" Text="+100"  /></td>
       </tr>
       <tr><td>Spot Rate Tonnage</td><td><asp:RadioButton ID="rdbIncluded" runat="server" Text="Included"  GroupName="A" />&nbsp;&nbsp;&nbsp;
       <asp:RadioButton ID="rbExcluded" runat="server" Text="Excluded" GroupName="A"  />
       </td></tr>
       <tr><td>Commisionable</td><td><asp:RadioButton ID="rdbyesCommisionable" runat="server" Text="Yes" GroupName="B"   />&nbsp;&nbsp;&nbsp;
       <asp:RadioButton ID="rdbNoCommissionable" runat="server" Text="No" GroupName="B"/>
       </td></tr>
       </table>
       </td>
       <td valign="top">
       <table width="80%" cellpadding=6 cellspacing=3><tr><td>
           Kickback To Agent(Amt) </td><td>
             <asp:TextBox ID="txtkickbackamount" runat="server" Width="92px" ></asp:TextBox>
               </td></tr>
               <tr><td>Threshold</td><td><asp:TextBox ID="txtthrshold" runat="server" Width="92px"></asp:TextBox>
       
       
       </td></tr>
               <tr><td>&nbsp;</td><td>&nbsp;</td></tr>
               </table>
       </td>
       </tr>
       </table>
       
       
       
       
       
      </fieldset> 
      </div>
     <div align="right">
           <%-- <asp:Button ID="btnNewBooking" runat="server" CssClass="button" Text="New Booking"
                PostBackUrl="~/ConBooking.aspx" />--%>
            &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="button" 
                Text="Save" onclick="btnSave_Click" />
            &nbsp;<asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" PostBackUrl="~/Home.aspx" />
        </div>
     </div> 
 </asp:Content>
