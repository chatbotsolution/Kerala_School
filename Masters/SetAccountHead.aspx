<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterSetting.master" AutoEventWireup="true" CodeFile="SetAccountHead.aspx.cs" Inherits="Masters_SetAccountHead" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="Javascript">
        function CnfSave() {

            if (confirm("You are going to Set Account Head . Do you want to continue?")) {

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
            Set Additional Fees Account Head</h2>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <center>
                <asp:Label ID="lblMsg" runat="server" Font-Bold="True" CssClass="tbltxt"></asp:Label>
            </center>
            <asp:GridView ID="grdAdFees" Width="100%" runat="server" AutoGenerateColumns="false"
                CssClass="mGrid tbltxt" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                TabIndex="5" OnRowDataBound="grdAdFees_RowDataBound" DataKeyNames="Ad_Id">
                <Columns>
                <asp:BoundField HeaderText="Sl No." DataField="Ad_Id" HeaderStyle-Width="40px"/>
                    <asp:TemplateField HeaderText="Fee Name">
                        <ItemTemplate>
                            <asp:Label ID="lblname" runat="server" Text='<%#Eval("Ad_Description")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Account Heads">
                        <ItemTemplate>
                            <asp:DropDownList ID="drpAccHeads" runat="server" OnSelectedIndexChanged="drpAccHeads_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnkbtnAccHead" runat="server" Text="Save" OnClick="lnkbtnAccHead_Click"
                                OnClientClick="return CnfSave()" Visible="false"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>