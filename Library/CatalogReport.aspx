<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="CatalogReport.aspx.cs" Inherits="Library_CatalogReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" language="javascript">

        function openwindow() {
            var pageurl = "rptBookdetailsreportPrint.aspx";
            window.open(pageurl, 'true', 'true');
        }
                
    </script>


     <asp:UpdatePanel ID="uppSubList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
                <img src="../images/icon_cp.jpg" width="29" height="29"></div>
            <div style="padding-top: 5px;">
                <h2>
                    Catalogue Report</h2>
            </div>
            <div style="width: 100%; text-align: left; border: solid 1px black; margin-top: 5px">
                <table class="tbltxt" cellpadding="2px">
                    <tr>
                     <td>
                            Accession No from &nbsp;:&nbsp;
                         <asp:TextBox ID="txtAccFrm" runat="server"></asp:TextBox>
                        </td>
                        <td> To &nbsp;:&nbsp;
                            <asp:TextBox ID="txtAccTo" runat="server"></asp:TextBox>
                        </td>

                        </tr>


                        <tr>
                        <td>
                            Catalogue Type&nbsp;:&nbsp;
                            <asp:DropDownList ID="ddlCatalogue" runat="server" AutoPostBack="true" CssClass="smalltb">
                               <%-- OnSelectedIndexChanged="ddlCatalogue_SelectedIndexChanged"--%>
                                <asp:ListItem Value="0">---Select One---</asp:ListItem>
                                <asp:ListItem Value="1">Subject Catalogue</asp:ListItem>
                                <asp:ListItem Value="2">Title Catalogue</asp:ListItem>
                                <asp:ListItem Value="3">First Author Catalogue</asp:ListItem>
                            </asp:DropDownList>
                           
                           
                        </td>
                  
                        <td align="left">                          
                             
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

