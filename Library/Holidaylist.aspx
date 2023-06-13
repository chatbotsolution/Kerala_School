<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="Holidaylist.aspx.cs" Inherits="Library_Holidaylist" %>


<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<script language="javascript" type="text/javascript">
    function isValid() {
        var CatName = document.getElementById("<%=txtCatName.ClientID %>").value;
        var CatDesc = document.getElementById("<%=txtDesc.ClientID %>").value;

        if (CatName.trim() == "") {
            alert("Please Fill Category Name!");
            document.getElementById("<%=txtCatName.ClientID %>").focus();
            document.getElementById("<%=txtCatName.ClientID %>").select();
            return false;
        }
        if (CatDesc.trim() == "") {
            alert("Please Fill Category Desciption!");
            document.getElementById("<%=txtDesc.ClientID %>").focus();
            document.getElementById("<%=txtDesc.ClientID %>").select();
            return false;
        }
        else {
            return true;
        }
    }
    </script>--%>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Add/Modify Holiday List</h2>
    </div>
    <div>
        <img src="../images/mask.gif" height="40" width="10" /></div>
     
    <div style="width: 470px; background-color: #666; padding:2px; margin: 0 auto;">
        <div style="background-color: #FFF; padding: 10px;">
            <table cellpadding="0px" cellspacing="5px" align="center" width="100%" class="tbltxt">
                <tr>
                <td colspan="2" height="20px">
                <span style=" color:Red;">Note: Select Same date for Single Holiday</span>
                <br />
                </td>
                   <%-- <td colspan="2" align="center">
                       <asp:RadioButtonList ID="RblDatePicker" runat="server" OnSelectedIndexChanged="RblDatePicker_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Selected="True">One Day</asp:ListItem>
                         <asp:ListItem Value="2">Vacation</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>--%>
                </tr>
                <tr>
                <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                   Date:&nbsp;&nbsp;
                    </td>
                    <td>
                      <asp:TextBox ID="txtHolDt" runat="server" Width="100px" MaxLength="30" placeholder="From Date"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpHolnDt" runat="server" Control="txtHolDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                        <asp:TextBox ID="txtHolDt2" runat="server" Width="100px" MaxLength="30" placeholder="To Date" ></asp:TextBox>
                        <rjs:PopCalendar ID="dtpHolnDt2" runat="server" Control="txtHolDt2" AutoPostBack="False" 
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                    </td>
                </tr>
                 <tr>
                    <td >
                        Day:&nbsp;&nbsp;</td>
                   <td> <asp:DropDownList ID="DdlDay" runat="server">
                         <asp:ListItem Value="0">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Monday</asp:ListItem>
                                <asp:ListItem Value="2">Tuesday</asp:ListItem>
                                <asp:ListItem Value="3">Wednesday</asp:ListItem>
                                <asp:ListItem Value="4">Thursday</asp:ListItem>
                                 <asp:ListItem Value="5">Friday</asp:ListItem>
                                <asp:ListItem Value="6">Saturday</asp:ListItem>
                                  <asp:ListItem Value="7">Vacation</asp:ListItem>
                        </asp:DropDownList>
                        <span style="color: Red; font-size: small;">*</span></td>
                </tr>
               
                <tr>
                    <td >
                        Holiday Description:&nbsp;&nbsp; 
                        
                    </td>
                    <td>
                    <asp:TextBox ID="txtDesc" runat="server" Width="200px" TextMode="MultiLine"
                                    Height="64px"></asp:TextBox>
                        <span style="color: Red; font-size: small;">*</span></td>
                    
                </tr>
                <tr>
                    <td colspan="2" style="height: 20px">
                        &nbsp;
                    </td>
                </tr>
                <%--<tr>
                    <td>
                        Category Image:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:FileUpload ID="fuCatImg" runat="server" />
                    </td>
                </tr>--%>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblImg" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
              
               
                  <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>

                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnSaveAddNew" runat="server" Text="Save & AddNew" Font-Bold="True"
                            OnClientClick="return isValid();" Font-Size="8pt" Width="120px" OnClick="btnSaveAddNew_Click" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" Font-Bold="True" Font-Size="8pt"
                            Width="60px" OnClick="btnCancel_Click" />&nbsp;
                        </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="hfUserId" runat="server" />
                    </td>
                </tr>
            </table >
            <table cellpadding="0px" cellspacing="0px" align="center" width="100%" class="tbltxt">
            <tr>
            <td>
            <asp:GridView ID="gridHolidayList" runat="server" AutoGenerateColumns="False"  Width="98%"
                                        AllowPaging="True" PageSize="10"
                                        CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr" 
                    AlternatingRowStyle-CssClass="alt" 
                    onselectedindexchanged="gridHolidayList_SelectedIndexChanged" 
                    onrowdeleting="gridHolidayList_RowDeleting">
             <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <Columns>
            
                <asp:TemplateField HeaderText="SL NO">
                <ItemTemplate>
                    <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" ></asp:Label>
                   <asp:Label ID="lblId" runat="server" Text='<%#Eval("ID")%>' Visible="false"></asp:Label>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DATE">
                <ItemTemplate>
                <asp:Label ID="lblDate" runat="server" Text='<%#Eval("HDate")%>'></asp:Label>
                   <asp:Label ID="lblFromDate" runat="server" Text='<%#Eval("FromDate")%>' Visible="false"></asp:Label>
                   <asp:Label ID="lblToDate" runat="server" Text='<%#Eval("ToDate")%>' Visible="false"></asp:Label>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DAY">
                 <ItemTemplate>
                <asp:Label ID="lblDay" runat="server" Text='<%#Eval("HDay")%>'></asp:Label>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DESCRIPTION">
                 <ItemTemplate>
                <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("Descrip")%>'></asp:Label>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField SelectText="Edit" ShowSelectButton="True" />
                <asp:CommandField ShowDeleteButton="True" />
            
            </Columns>
             <RowStyle BackColor="#EFEFEF" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle BackColor="#333333" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="#FDFDFD" />
                                        <EmptyDataTemplate>
                                            No Record
                                        </EmptyDataTemplate>
                        </asp:GridView>
            </td>
            </tr>
            </table>
        </div>
    </div>
  
</asp:Content>
