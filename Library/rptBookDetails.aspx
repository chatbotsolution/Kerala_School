<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="rptBookDetails.aspx.cs" Inherits="Library_rptBookDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function openwindow() {
            var pageurl = "rptBookDetailsPrint.aspx";
            window.open(pageurl, 'true', 'true');
        }
                
    </script>

    <asp:UpdatePanel ID="uppSubList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Stock Details Report</h2>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px">
                <table class="tbltxt" cellpadding="2px">
                    <tr>
                        <td>
                            Category&nbsp;:&nbsp;
                            <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" CssClass="smalltb"
                                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp;Subject&nbsp;:&nbsp;
                            <asp:DropDownList ID="ddlSubject" runat="server" CssClass="largetb">
                                <asp:ListItem Text="--All--" Value="0" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;Publisher&nbsp;:&nbsp;
                            <asp:DropDownList ID="ddlPublisher" runat="server" CssClass="largetb">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Author&nbsp;:&nbsp;
                            <asp:TextBox ID="txtAuthor" runat="server" MaxLength="50" CssClass="largetb" ontextchanged="txtAuthor_TextChanged"  AutoPostBack="true" ></asp:TextBox>&nbsp;
                          
                          <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1"  TargetControlID="txtAuthor" MinimumPrefixLength="1"  CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" runat="server" ContextKey="51"
  FirstRowSelected="false" ServiceMethod="AutoCompleteLib"  CompletionListCssClass="AutoExtender" CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"  ></ajaxToolkit:AutoCompleteExtender>
                          
                            <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" />&nbsp;
                            <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click" />&nbsp;
                            <asp:Button ID="btnPrint" runat="server" Text="Print" 
                                OnClientClick="openwindow();" onclick="btnPrint_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: left; padding-left:0px; padding-right:0px;" class="tbltxt">
                <asp:Label ID="lblReport" runat="server"></asp:Label>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

