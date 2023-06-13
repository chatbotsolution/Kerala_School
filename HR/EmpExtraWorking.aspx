<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpExtraWorking.aspx.cs" Inherits="HR_EmpExtraWorking" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">

    function CnfSv() {

        if (confirm("You are going to mark Extra working for this date. Do you want to continue?")) {
            return true;
        }
        else {
            return false;
        }
    }

    function CnfDel() {

        if (confirm("You are going to delete this record. Do you want to continue?")) {
            return true;
        }
        else {
            return false;
        }
    }

    function IsValid() {

        var Emp = document.getElementById("<%=drpEmp.ClientID %>").value;
        var Dt = document.getElementById("<%=txtDate.ClientID %>").value;
        var Rmks = document.getElementById("<%=txtRemarks.ClientID %>").value;
        if (Emp == "0") {
            alert("Please Select Employee !");
            document.getElementById("<%=drpEmp.ClientID %>").focus();
            return false;
        }
        if (Dt.trim() == "") {
            alert("Please Select Date !");
            document.getElementById("<%=txtDate.ClientID %>").focus();
            return false;
        }
        if (Rmks.trim() == "") {
            alert("Please Enter Remarks !");
            document.getElementById("<%=txtRemarks.ClientID %>").focus();
            return false;
        }
        if (!CnfSv()) {
            return false;
        }
        else {
            return true;
        }
    }
    
</script>


    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Attendance For Extra Working</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlAdd" runat="server">
                <table>
                     <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Date
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server" Width="100px" AutoPostBack="True" ReadOnly="True"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpDate" runat="server" Control="txtDate" To-Today="true"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Employee
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpEmp">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Remarks
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="250px" runat="server" MaxLength="99"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td >
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return CnfSv();" />
                            &nbsp;
                            <asp:Button ID="btnList" runat="server" Text="View List" OnClick="btnList_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlList" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>
                            Employee:<asp:DropDownList runat="server" ID="drpEmpList">
                            </asp:DropDownList>
                        </td>
                        <td>
                            From Date:<asp:TextBox ID="txtFrmDt" runat="server" Width="100px" AutoPostBack="True"
                                ReadOnly="True"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpFrmDt" runat="server" Control="txtFrmDt" AutoPostBack="True"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                        </td>
                        <td>
                            To Date:<asp:TextBox ID="txtToDt" runat="server" Width="100px" AutoPostBack="True"
                                ReadOnly="True"></asp:TextBox>
                            <rjs:PopCalendar ID="dtpToDt" runat="server" Control="txtToDt" AutoPostBack="True"
                                Format="dd mmm yyyy"></rjs:PopCalendar>
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" 
                                onclick="btnSearch_Click" />
                            &nbsp;
                            <asp:Button ID="btnAddEW" runat="server" Text="Add Extra Working" 
                                onclick="btnAddEW_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            &nbsp;
                            <asp:Label ID="lblMsg1" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="grdEW" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" AlternateText="Delete" ImageUrl="~/images/icon_delete.gif"
                                                ToolTip="Click to Delete" OnClientClick="return CnfDel();" OnClick="btnDelete_Click" />
                                            <asp:HiddenField ID="hfEWId" runat="server" Value='<%#Eval("EW_Id")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                    </asp:TemplateField>
                                     <asp:BoundField DataField="SevName" HeaderText="Employee">
                                        <HeaderStyle HorizontalAlign="Left" Width="160px" />
                                        <ItemStyle HorizontalAlign="Left" Width="160px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EW_DateStr" HeaderText="Date">
                                        <HeaderStyle HorizontalAlign="Left" Width="140px" />
                                        <ItemStyle HorizontalAlign="Left" Width="140px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="HolidayName" HeaderText="Holiday">
                                        <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks">
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record(s)
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

