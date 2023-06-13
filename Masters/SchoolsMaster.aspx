<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="SchoolsMaster.aspx.cs" Inherits="Masters_SchoolsMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<SCRIPT language="javascript" type="text/javascript">

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

    function ShowMe(e) {
        if (e.checked) {
            document.getElementById("divgrddesg").style.display = "block";
        }
        else {
            document.getElementById("divgrddesg").style.display = "none";
        }
    }
            </SCRIPT>
<TABLE style="VERTICAL-ALIGN: top; HEIGHT: 340px" width="100%"><TBODY><TR><TD style="WIDTH: 100%" vAlign=top align=center bgColor=#dfdfdf colSpan=3><STRONG><asp:Label id="lblTitle" runat="server" Text="Schools">
                                </asp:Label> </STRONG></TD></TR><TR><TD vAlign=top align=center width="100%" colSpan=3><asp:Label id="lblerr" runat="server" ForeColor="Red">
                            </asp:Label> </TD></TR><TR><TD style="WIDTH: 90%" vAlign=top align=center colSpan=3>
        <asp:GridView id="GridView1" runat="server" AllowPaging="true" 
            OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="5" 
            AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand" TabIndex="8">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input name="toggleAll" onclick='ToggleAll(this)' type="checkbox" value="ON" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input name="Checkb" type="checkbox" value='<%#Eval("SchoolID")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle BackColor="#DFDFDF"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" Width="15px"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="School Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSchool" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SchoolID")%>'
                                                Text='<%#Eval("SchoolName")%>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" BackColor="#DFDFDF"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" Width="900px" VerticalAlign="Middle"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Address1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress1" runat="server" Text='<%#Eval("Address1")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" BackColor="#DFDFDF"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" Width="150px" VerticalAlign="Middle"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Address2">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress2" runat="server" Text='<%#Eval("Address2")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" BackColor="#DFDFDF"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" Width="100px" VerticalAlign="Middle"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Principal">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrincipal" runat="server" Text='<%#Eval("PrincipalName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" BackColor="#DFDFDF"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" Width="100px" VerticalAlign="Middle"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Telephone">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTele" runat="server" Text='<%#Eval("SchoolTelNo")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" BackColor="#DFDFDF"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" Width="90px" VerticalAlign="Middle"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView> </TD></TR><TR><TD><TABLE width="100%"><TBODY><TR align=center><TD style="WIDTH: 40%" align=right>&nbsp;School Name</TD><TD style="WIDTH: 55%" align=left colSpan=2>
        <asp:TextBox id="txtcat" runat="server" Width="221px" TabIndex="1"></asp:TextBox> <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txtcat" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator> </TD></TR><TR><TD style="WIDTH: 40%" align=right>Address1</TD><TD style="WIDTH: 55%" align=left colSpan=2>
            <asp:TextBox id="txtdesc" runat="server" Width="219px" TabIndex="2"></asp:TextBox> <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ControlToValidate="txtdesc" ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator> </TD></TR><TR><TD style="WIDTH: 40%" align=right>Address2</TD><TD style="WIDTH: 55%" align=left colSpan=2>
            <asp:TextBox id="txtlat" runat="server" Width="220px" TabIndex="3"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 40%" align=right>Principal Name</TD><TD style="WIDTH: 55%" align=left>
            <asp:TextBox id="txtedt" runat="server" Width="219px" TabIndex="4"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 40%" align=right>School Tel No</TD><TD style="WIDTH: 55%" align=left>
            <asp:TextBox id="txteat" runat="server" Width="70px" TabIndex="5"></asp:TextBox> </TD></TR><TR><TD style="WIDTH: 40%" align=right>School Fax No</TD><TD style="WIDTH: 55%" align=left>
            <asp:TextBox id="txtldt" runat="server" Width="70px" TabIndex="6"></asp:TextBox> </TD></TR></TBODY></TABLE></TD></TR><TR><TD align=center>
        <asp:Button id="btnSaveAddNew" onclick="btnSaveAddNew_Click" runat="server" 
            Text="Submit" TabIndex="7">
                            </asp:Button><asp:Button id="btnDelete" 
            onclick="btnDelete_Click" runat="server" Text="Delete" CausesValidation="false" 
            TabIndex="9"></asp:Button><asp:Button id="btncancel" 
            onclick="btncancel_Click" runat="server" Text="Cancel" CausesValidation="False" 
            TabIndex="10"></asp:Button></TD></TR><TR><TD align=center><asp:HiddenField id="hdnschool" runat="server"></asp:HiddenField> </TD></TR></TBODY></TABLE>
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

