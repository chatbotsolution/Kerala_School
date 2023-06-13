<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.master" AutoEventWireup="true" CodeFile="JournalVchrList.aspx.cs" Inherits="Accounts_JournalVchrList" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script language="Javascript" type="text/javascript">
    function CnfDelete() {

        if (confirm("You are going to delete a record. Do you want to continue?")) {

            return true;
        }
        else {

            return false;
        }
    }  
    
    </script>
<asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="350" align="left" valign="middle" style="background-color: ">
                        <div class="headingcontainor">
                            <h1>
                                Journal
                            </h1>
                            <h2>
                                Voucher List</h2>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="98%">
                <tr>
                    <td align="left" colspan="2">
                        <asp:Label ID="lblToDt" runat="server" Text="From Date : "></asp:Label>
                        <asp:TextBox ID="txtFromDt" runat="server" ReadOnly="true" Width="80px" TabIndex="1"></asp:TextBox>
                        <rjs:PopCalendar ID="dtpfromdt" runat="server" Control="txtFromDt" Format="dd mmm yyyy">
                        </rjs:PopCalendar>
                        &nbsp;<asp:Label ID="lblFrmDt" runat="server" Text="To Date : "></asp:Label>
                        <asp:TextBox ID="txtToDt" runat="server" ReadOnly="true" Width="80px" TabIndex="2"></asp:TextBox>
                        <rjs:PopCalendar ID="dtptodt" runat="server" Control="txtToDt" Format="dd mmm yyyy">
                        </rjs:PopCalendar>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnView" runat="server" TabIndex="3" Text="View List" OnClick="btnView_Click" />
                        &nbsp;<asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add New" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td align="right">
                        <asp:Label ID="lblRecord" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:GridView ID="grdJrnl" runat="server"
                            DataKeyNames="JE_TransId" Width="100%" PageSize="20" AllowPaging="true" AutoGenerateColumns="false"
                            AllowSorting="True" onpageindexchanging="grdJrnl_PageIndexChanging" 
                            onrowdeleting="grdJrnl_RowDeleting" ondatabound="grdJrnl_DataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Action" >
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblJrnlId" runat="server" Text='<%#Eval("JE_TransId") %>' Visible="false"></asp:Label>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/images/icon_delete.gif"
                                                OnClientClick="return CnfDelete()" Visible="false" /></center>
                                    </ItemTemplate>
                                    <HeaderStyle Width="60px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Ag_Code" Visible="False"></asp:BoundField>
                                <asp:TemplateField HeaderText="Transaction Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<% #Bind("TransDateStr")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                               
                                 <asp:BoundField DataField="Narration" HeaderText="Description" HeaderStyle-HorizontalAlign="Left" ></asp:BoundField>
                                 <asp:BoundField DataField="TransAmtStr" HeaderText="Total Amount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Right"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

