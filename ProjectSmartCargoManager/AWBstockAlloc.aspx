<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="AWBstockAlloc.aspx.cs" Inherits="ProjectSmartCargoManager.AWBstockAlloc" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
--%>

   
    <script language="javascript" type="text/javascript">

        function ValidatioofGreater() {
            alert("From Must be Small than To");

        }


        function EmptyMsg() {
            alert("Please Fill Data");

        }

        function EmptyDataMsg(City) {
            alert("No City Available With This " + City);

        }
                 
    
    function expandcollapse(obj,row)
    {
        var div = document.getElementById(obj);
        var img = document.getElementById('img' + obj);
        
        if (div.style.display == "none")
        {
            div.style.display = "block";
            if (row == 'alt')
            {
                img.src = ".//Images/minus.gif";
            }
            else
            {
                img.src = ".//Images/minus.gif";
            }
            img.alt = "Close to view other Customers";
        }
        else
        {
            div.style.display = "none";
            if (row == 'alt')
            {
                img.src = "plus.gif";
            }
            else
            {
                img.src = "plus.gif";
            }
            img.alt = "Expand to show Orders";
        }
    } 
    </script>
    
     <style>
		.black_overlay{
			display: none;
			position: absolute;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 100%;
			background-color: black;
			z-index:1001;
			-moz-opacity: 0.8;
			
		}
		.white_content {
			display: none;
			position: absolute;
			top: 25%;
			left: 5%;
			width: 85%;
			height: 10%;
			padding: 16px;
			border: 16px Solid Blue;
			background-color: white;
			z-index:1002;
			overflow: auto;
		}
	</style>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
 
 <div style="float:left;">
<table>
<tr>
<td colspan="3"></td>

</tr>

<tr>
<%--<td  align="center" style="width:20%" valign="top">
    <asp:ListBox ID="lstCity" runat="server" AppendDataBoundItems="True" 
        AutoPostBack="True" onselectedindexchanged="lstCity_SelectedIndexChanged" 
        Width="84px" Height="295px"></asp:ListBox>
    
        
    </td>--%>
<td >
 <div>
         <asp:GridView ID="GridView1" AllowPaging="True" BackColor="#F1F1F1" 
            AutoGenerateColumns="False" 
            style="Z-INDEX: 101" Width="100%" Font-Size="Small"
            Font-Names="Verdana" runat="server" GridLines="None" 
           OnRowDataBound="GridView1_RowDataBound"
         BorderStyle="Outset"
        AllowSorting="True" onrowupdating="GridView2_RowUpdating"  >
            <RowStyle BackColor="Gainsboro" />
            <AlternatingRowStyle BackColor="White" />
            <HeaderStyle BackColor="#36a3f8" ForeColor="White" HorizontalAlign="Left"/>
            <FooterStyle BackColor="White" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:expandcollapse('div<%# Eval("Agentname") %>', 'one');">
                            <img id="imgdiv<%# Eval("Agentname") %>" alt="Click to show/hide Orders for Customer <%# Eval("Agentname") %>"  width="9px" border="0" src="plus.gif"/>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AgentName" >
                    <ItemTemplate>
                        <asp:Label ID="lblagent" Text='<%# Eval("Agentname") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>
                
			    <asp:TemplateField>
			        <ItemTemplate>
			            <tr>
                            <td colspan="100%">
                                <div id="div<%# Eval("Agentname") %>" style="display:none;position:relative;left:15px;OVERFLOW: auto;WIDTH:97%" >
                                    <asp:GridView ID="GridView2" AllowPaging="True" AllowSorting="false" runat ="server" 
                                     BackColor="#F1F1F1" Width="100%" Font-Size="X-Small"
                                        AutoGenerateColumns="false" Font-Names="Verdana"   ShowFooter="false"
                                        OnPageIndexChanging="GridView2_PageIndexChanging"  
                                       
                                         OnRowEditing = "GridView2_RowEditing" GridLines="None"                                          
                                          OnRowDataBound = "GridView2_RowDataBound"
                                          OnRowUpdating="GridView2_RowUpdating" 
                                        OnRowUpdated = "GridView2_RowUpdated" OnRowCancelingEdit = "GridView2_CancelingEdit" 
                              OnSorting = "GridView2_Sorting" 
                                        BorderStyle="Double" BorderColor="#0083C1">
                                        <RowStyle BackColor="Gainsboro" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <HeaderStyle BackColor="#36a3f8" ForeColor="White" HorizontalAlign="Left"/>
                                        <FooterStyle BackColor="White" />
                                        <Columns>
                                        <%--<asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Image   ID="imgDevice" runat="server" ImageUrl="~/images/mc2-65.jpg"></asp:Image>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>

                                            <asp:TemplateField HeaderText="Level" SortExpression="OrderID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDevice" Text='<%# Eval("Levelcode") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblDevice" Text='<%# Eval("Levelcode") %>' runat="server"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="from">
                                                <ItemTemplate><%# Eval("fromawbno")%></ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtfrom" Text='<%# Eval("fromawbno")%>' runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="To" >
                                                <ItemTemplate><%# Eval("toawbno")%></ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtto" Text='<%# Eval("toawbno")%>' runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Remaining" >
                                                <ItemTemplate><%# Eval("Remaining")%></ItemTemplate>
                                                                                           </asp:TemplateField>
                                            --%>
                                            <asp:TemplateField HeaderText="SubLevel" >
                                                <ItemTemplate><%# Eval("sublevelcode")%></ItemTemplate>
                                                                                           </asp:TemplateField>
                                            
			                                <asp:CommandField HeaderText="Edit" ShowEditButton="True"  />
			                               <%-- <asp:TemplateField HeaderText="Delete">
                                                 <ItemTemplate>
                                                    <asp:LinkButton ID="linkDeleteCust" CommandName="Delete" runat="server">Delete</asp:LinkButton>
                                                 </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                   </asp:GridView>
                                </div>
                             </td>
                        </tr>
			        </ItemTemplate>			       
			    </asp:TemplateField>			    
			 	</Columns>
        </asp:GridView>   
    </div>



 <asp:Panel ID="pnlNewLink" runat="server"  Width="100%" EnableViewState ="true" >
     <div class="shadowidth530" style="width:100%">
    <div class="shadow1" align="center">
      <div  class="msg">
        <table width="" border="0">
          <tr >
            <td>
            <asp:Label ID="lblNew" runat="server" ForeColor="red" Text="For From and To to already Allocate Agents  "></asp:Label>
            </td>
          <td>
          <a href = "javascript:void(0)" onclick = "document.getElementById('light').style.display='block';document.getElementById('fade').style.display='block'"; >New Alloc</a>
           </td>
          
                      </tr>
                    
        </table>
      </div>
    </div>
  </div>
 
    </asp:Panel>





</td>
<%--<td  style="width:20%"></td>
--%>
</tr>

<tr>
<td colspan="3"></td>
</tr>

</table>

<div id="light" class="white_content">
<table>

<tr>
<%--<td width="12%">
    <asp:Label ID="lblDeviceName" runat="server" Text="Device Name"></asp:Label>
</td>
--%>

  <td> 
      <asp:Label ID="Label1" runat="server" Text="Agent"></asp:Label>
  </td>
  <td>
      <asp:DropDownList ID="DDLAgent" runat="server" AppendDataBoundItems="True">
      <asp:ListItem >Select </asp:ListItem>
      </asp:DropDownList>
  </td>
  <td> 
      <asp:Label ID="Label2" runat="server" Text="Level"></asp:Label>
  </td>
  
  
      
  <td>      
  <asp:UpdatePanel ID="UpdatePanel1" runat="server"> 
  <ContentTemplate  >
      <asp:DropDownList ID="DDLLevel" runat="server" 
          onselectedindexchanged="DDLLevel_SelectedIndexChanged" AutoPostBack ="true" >
          <asp:ListItem>Country</asp:ListItem>
          <asp:ListItem>Station</asp:ListItem>
          <asp:ListItem>Region</asp:ListItem>
          <asp:ListItem>City</asp:ListItem>
      </asp:DropDownList>
      </ContentTemplate>
         <Triggers >  
            <asp:AsyncPostBackTrigger controlid="DDLLevel" eventname="SelectedIndexChanged" />
         </Triggers>
      </asp:UpdatePanel>
  </td>
  <td>
  <asp:UpdatePanel ID="UpdatePanel2" runat="server"> 
  <ContentTemplate  >
      <asp:DropDownList ID="DDLSubLevel" runat="server">
      </asp:DropDownList>
   </ContentTemplate>
       <Triggers >  
            <asp:AsyncPostBackTrigger controlid="DDLLevel" eventname="SelectedIndexChanged" />
         </Triggers>
      </asp:UpdatePanel>
      
  </td>


<%--<td width="10%">
    <asp:Label ID="lblDeviceName" runat="server" Text=" Device Name"></asp:Label>
</td>--%>
<%--<td width="23%">  
    <asp:TextBox ID="txtDeviceName" runat="server" Text="" BorderStyle="Solid" BorderColor="Black"  BackColor="#ffb9b9"  Width="70%"></asp:TextBox>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtDeviceName" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

</td>--%>

<td width="10%">  
    <asp:Label ID="lblFromUBI" runat="server" Text="From AWB "></asp:Label>
</td>
<td width="20%">
<asp:TextBox ID="txtUBIDev" runat="server" Text="" MaxLength="8" BorderStyle="Solid" BorderColor="Black"  BackColor="#ffb9b9"  Width="70%"></asp:TextBox>
<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                                runat="server" Enabled="True" FilterInterval="250" FilterType="Numbers" 
                                                TargetControlID="txtUBIDev" ValidChars="0123456789">
                                            </asp:FilteredTextBoxExtender>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtUBIDev" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
</td>
<td width="10%">
    <asp:Label ID="lblTOUBI" runat="server" Text="To AWB"></asp:Label>
</td>
<td width="20%">
<asp:TextBox ID="txtToUBIDev" runat="server" Text="" MaxLength="8" BorderStyle="Solid" BorderColor="Black"  BackColor="#ffb9b9" Width="70%"></asp:TextBox>
<asp:FilteredTextBoxExtender ID="txtToUBIDev_FilteredTextBoxExtender" 
                                                runat="server" Enabled="True" FilterInterval="250" FilterType="Numbers" 
                                                TargetControlID="txtToUBIDev" ValidChars="0123456789">
                                            </asp:FilteredTextBoxExtender>
                                           <asp:RequiredFieldValidator ID="RFV" ControlToValidate="txtToUBIDev" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                            
</td>
<td width="5%">

<a href="#" class="redbutton red" style="margin-top:7px"><span>
                <asp:Button  ID="btnAlloc" runat="server" Text="Alloc" 
        Font-Names="Verdana" Font-Size="Small" ForeColor="red" 
        onclick="btnAlloc_Click" /></span></a>

</td>
<td width="5%">
		<%--<a href = "javascript:void(0)" onclick = "document.getElementById('light').style.display='none';document.getElementById('fade').style.display='none'">Close</a>--%>
		<a href = "javascript:void(0)" class="redbutton red" style="margin-top:7px" onclick = "document.getElementById('light').style.display='none';document.getElementById('fade').style.display='none'">
		<span>
		 <asp:Button  ID="btnClose" runat="server" Text="Close" Font-Names="Verdana" Font-Size="Small" ForeColor="red" />
		</span>
		</a>
		
</td>
</tr>
</table>
		</div>
		<div id="fade" class="black_overlay"></div>
</div>
</asp:Content>