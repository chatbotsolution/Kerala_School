<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true"
    CodeFile="BookIssueGrid.aspx.cs" Inherits="Library_BookIssueGrid" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
    function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
        var key;
        var isCtrl = false;
        var keychar;
        var reg;

        if (window.event) {
            key = e.keyCode;
            isCtrl = window.event.ctrlKey
        }
        else if (e.which) {
            key = e.which;
            isCtrl = e.ctrlKey;
        }

        if (isNaN(key)) return true;

        keychar = String.fromCharCode(key);

        // check for backspace or delete, or if Ctrl was pressed
        if (key == 8 || isCtrl) {
            return true;
        }

        reg = /\d/;
        var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
        var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

        return isFirstN || isFirstD || reg.test(keychar);
    }
</script>
 
    <asp:UpdatePanel ID="uppSubList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Group Issue</h2>
            </div>
            <div>
                <img src="../images/mask.gif" height="8" width="10" /></div>
            <div style="width: 100%; background-color: #666; padding: 1px; margin: 0 auto;">
                <div style="background-color: #FFF; padding: 10px;">
                    <table border="0" cellspacing="0" cellpadding="0" class="tbltxt">
                        <tr>
                            <td>
                                Class :
                                <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="true" CssClass="largetb"
                                    OnSelectedIndexChanged="ddlClass_SelectedIndexChanged">
                                    <asp:ListItem Text="---All---" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp; Section :
                                <asp:DropDownList ID="ddlSection" runat="server" AutoPostBack="true" CssClass="largetb"
                                    OnSelectedIndexChanged="ddlSection_SelectedIndexChanged">
                                    <asp:ListItem Text="---All---" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;
                                    Issue Date:
                                <asp:TextBox ID="txtIssueDt" runat="server" MaxLength="30" CssClass="smalltb" ReadOnly="true"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpIssueDt" runat="server" Control="txtIssueDt" AutoPostBack="True"
                                    Format="dd mmm yyyy" OnSelectionChanged="dtpIssueDt_SelectionChanged"></rjs:PopCalendar>
                                <asp:LinkButton ID="lnkcldate" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtIssueDt.value='';return false;"
                                    Text="Clear"></asp:LinkButton>
                                &nbsp;
                                Due Date:
                                 <asp:TextBox ID="txtDueDt" runat="server" MaxLength="30" CssClass="smalltb" ReadOnly="true"></asp:TextBox>
                                <rjs:PopCalendar ID="dtpDueDt" runat="server" Control="txtDueDt" AutoPostBack="False"
                                    Format="dd mmm yyyy"></rjs:PopCalendar>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="javascript:document.forms[0].ctl00_ContentPlaceHolder1_txtDueDt.value='';return false;"
                                    Text="Clear" ></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    </div>
                    </div>
                    <div id="dvIssue" runat="server" visible="false" style="width: 100%; background-color: #666;
                padding: 1px; margin: 0 auto; margin-top: 15px;">
                <div style="background-color: #FFF; padding: 10px;">
                    <asp:GridView ID="grdIssueList" runat="server" AutoGenerateColumns="False" Width="100%" 
                        CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <EmptyDataRowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="Member Id" Visible="false">
                                <ItemTemplate>
                                 <asp:Label ID="lblMId" runat="server" Text='<%#Eval("MemberId")%>'></asp:Label>
                                   
                                </ItemTemplate>
                                <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Admn No">
                                <ItemTemplate>
                                    <%#Eval("OldAdmnno")%>
                                </ItemTemplate>
                                <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Roll No">
                                <ItemTemplate>
                                    <%#Eval("RollNo")%>
                                </ItemTemplate>
                                <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Member Name">
                                <ItemTemplate>
                                    <%#Eval("MemberName")%>
                                </ItemTemplate>
                                <HeaderStyle Width="120px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Class">
                                <ItemTemplate>
                                    <%#Eval("ClassName")%>
                                </ItemTemplate>
                                <HeaderStyle Width="30px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Section">
                                <ItemTemplate>
                                    <%#Eval("Section")%>
                                </ItemTemplate>
                                <HeaderStyle Width="30px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Accession No">
                                <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtAccnNo" OnTextChanged="txtAccnNo_textChanged" onkeypress="return blockNonNumbers(this, event, false, false);" AutoPostBack="true"></asp:TextBox>
                                 <asp:HiddenField ID="hfAccNo" Value="" runat="server" /> 
                                    <%--<%#Eval("AccessionNo")%>
                                    <asp:HiddenField ID="hdnAccessionNo" runat="server" />--%>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="Book Details">
                                <ItemTemplate>
                                <asp:Label ID="label1" runat="server"></asp:Label>
                                    <%--<%#Eval("AccessionNo")%>
                                    <asp:HiddenField ID="hdnAccessionNo" runat="server" />--%>
                                </ItemTemplate>
                                
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" ">
                                <ItemTemplate>
                                <asp:Button ID="btnIssue" runat="server" Text="Issue" Enabled="false" OnClick="btnIssue_Clicked"></asp:Button>
                                    <%--<%#Eval("AccessionNo")%>
                                    <asp:HiddenField ID="hdnAccessionNo" runat="server" />--%>
                                </ItemTemplate>
                                 <HeaderStyle Width="50px" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Record
                        </EmptyDataTemplate>
                        <FooterStyle BackColor="#5e5e5e" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#5e5e5e" ForeColor="#FFFFFF" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#FFFFFF" />
                        <EditRowStyle BackColor="Black" Font-Bold="True" Font-Size="10pt" ForeColor="#FFFFFF" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
