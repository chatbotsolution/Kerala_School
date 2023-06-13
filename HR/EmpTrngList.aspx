<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/HR.master" AutoEventWireup="true" CodeFile="EmpTrngList.aspx.cs" Inherits="HR_EmpTrngList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <script src="../Scripts/ModalPopups.js" type="text/javascript" language="javascript"></script>
<script language="Javascript">
    function ToggleAll(e) {
        if (e.checked) {
            CheckAll();
        }
        else {
            ClearAll();
        }
    }

    function CheckAll() {
        var ml = document.aspnetForm;
        var len = ml.elements.length;
        for (var i = 0; i < len; i++) {
            var e = ml.elements[i];
            if (e.name == "Checkb") {
                e.checked = true;
            }
        }
        ml.toggleAll.checked = true;
    }

    function ClearAll() {
        var ml = document.aspnetForm;
        var len = ml.elements.length;
        for (var i = 0; i < len; i++) {
            var e = ml.elements[i];
            if (e.name == "Checkb") {
                e.checked = false;
            }
        }
        ml.toggleAll.checked = false;
    }
    function valid() {
        var flag;
        var ml = document.aspnetForm;
        var len = ml.elements.length;
        for (var i = 0; i < len; i++) {
            var e = ml.elements[i];
            if (e.name == "Checkb") {

                if (e.checked) {
                    flag = true;
                    break;
                }
                else
                    flag = false;
            }
        }
        //alert(flag);
        if (flag == true)
            return true;
        else {
            alert("Please select any record");
            return false;
        }
    }
    function CnfDelete() {

        if (confirm("You are going to delete a record. Do you want to continue?")) {

            return true;
        }
        else {

            return false;
        }
    }
    </script>

<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Employee Training Master</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
        
        <div align="left">
                   <table width="825px">
                        <tr>
                            <td style="height: 10px" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="width: 825px; background-color: #666; padding: 2px; margin: 0 auto;">
                                    <div style="background-color: #FFF; padding: 10px;">
                                        <strong>Training:</strong> &nbsp;<asp:TextBox ID="txtTrngName"
                                            runat="server" Height="17px"></asp:TextBox>&nbsp;
                                        <asp:Button ID="btnShow" runat="server" CausesValidation="False" Text="Show" 
                                            onclick="btnShow_Click" />&nbsp;
                                        &nbsp;
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <div style="width: 825px; background-color: #666; padding: 2px; margin: 0 auto;">
                                    <div style="background-color: #FFF; padding: 10px;">
                                        <table width="800px">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="btnAddNew" runat="server" Text="Add New" 
                                                        onclick="btnAddNew_Click" />
                                                    &nbsp;
                                                    <asp:Button ID="btndelete" runat="server" CausesValidation="False" OnClientClick="return CnfDelete();"
                                                        Text="Delete Selected Records" onclick="btndelete_Click" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblNoOfRec" runat="server" Style="text-align: right"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:GridView ID="grdTrng" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        AllowPaging="True" PageSize="15" 
                                                        onpageindexchanging="grdTrng_PageIndexChanging">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <input type="checkbox" name="Checkb" value='<%# Eval("TrgId") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <HeaderTemplate>
                                                                    <input type="checkbox" value="ON" name="toggleAll" onclick='ToggleAll(this)' />
                                                                </HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <a href='EmpTrngMaster.aspx?TrngId=<%#Eval("TrgId")%>'>Edit</a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Training Name" DataField="TrgName"/>
                                                            <asp:BoundField HeaderText="Training Palce" DataField="TrgPlace"/>
                                                            <asp:TemplateField HeaderText="Duration">
                                                                <ItemTemplate>
                                                                    <%#Eval("FromDtStr")%> To <%#Eval("ToDtStr")%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Status" DataField="Status"/>
                                                            <asp:TemplateField HeaderText="Details">
                                                                <ItemTemplate>
                                                                    <a href="javascript:ModalPopupsAlert1('<%#Eval("TrgDetails")%>');">View</a>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No Record
                                                        </EmptyDataTemplate>
                                                        <EmptyDataRowStyle HorizontalAlign="Center" Font-Bold="true" />
                                                        <FooterStyle BackColor="#5e5e5e" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                                                        <HeaderStyle CssClass="datalisttopbar" />
                                                        <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                                                        <AlternatingRowStyle CssClass="datalistalternaterow" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    </div>
                    <script type="text/javascript" language="javascript">
                        function ModalPopupsAlert1(TrainingDetails) {
                            ModalPopups.Alert("jsAlert1",
                "Project Details",
                "<div style='padding:5px;width:600px;word-wrap: break-word;'>" + TrainingDetails + "<br/>" +
                "<br/></div>",
                {
                    okButtonText: "Close"
                }
            );
                        }  
    </script>
</asp:Content>

