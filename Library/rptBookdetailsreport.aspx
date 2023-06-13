<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Library.master" AutoEventWireup="true" CodeFile="rptBookdetailsreport.aspx.cs" Inherits="Library_rptBookdetailsreport" %>
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
                    Book Details Report</h2>
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

            <div id="trgrd" runat="server" style="height: 450px; width: 100%; overflow: scroll;">
               
                <table style="width: 100%; height: 450px;overflow: scroll; table-layout: fixed;" class="tbltxt">
                    <tr>
                        <td style="width: 150px; vertical-align: top;">
                        <asp:CheckBox ID="cbAll" runat="server" Text="All" 
                                oncheckedchanged="cbAll_CheckedChanged"  Checked="true" />
                            <br />
                            <asp:CheckBox ID="cbAccNo" runat="server" Text="Accession No." 
                                oncheckedchanged="cbAccNo_CheckedChanged" />
                            <br />  
                             <asp:CheckBox ID="cbAccDate" runat="server" Text="Accession Date." 
                                oncheckedchanged="cbAccDate_CheckedChanged" />
                            <br />                             
                           <%-- <asp:CheckBox ID="cbBkId" runat="server" Text="Admn Date" 
                                oncheckedchanged="cbBkId_CheckedChanged"/>
                            <br />--%>
                          <%--  <asp:CheckBox ID="cbCatCode" runat="server" Text="Admn Session Year" 
                                oncheckedchanged="cbCatCode_CheckedChanged"/>
                            <br />--%>
                            <asp:CheckBox ID="cbBrfNm" runat="server" Text="Brief Name" 
                                oncheckedchanged="cbBrfNm_CheckedChanged"/>
                            <br />
                           <%-- <asp:CheckBox ID="cbSubjectId" runat="server" Text="Present Session" 
                                oncheckedchanged="cbSubjectId_CheckedChanged"/>
                            <br />--%>
                            <asp:CheckBox ID="cbSubNm" runat="server" Text="Subject" 
                                oncheckedchanged="cbSubNm_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbBkTitle" runat="server" Text="BookTitle" oncheckedchanged="cbBkTitle_CheckedChanged"/>
                            <br />
                             <asp:CheckBox ID="cbBkSubTitle" runat="server" Text="SubTitle" oncheckedchanged="cbBkSubTitle_CheckedChanged"/>
                            <br />
                             <asp:CheckBox ID="cbAuthNm1" runat="server" Text="AuthorName1" 
                                oncheckedchanged="cbAuthNm1_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbAuthNm2" runat="server" Text="AuthorName2" 
                                oncheckedchanged="cbAuthNm2_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbAuthNm3" runat="server" Text="AuthorName3" 
                                oncheckedchanged="cbAuthNm3_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbClassi_No" runat="server" Text="Classification" 
                                oncheckedchanged="cbClassi_No_CheckedChanged"/>
                                  <br />
                            <asp:CheckBox ID="cbDimen" runat="server" Text="Dimension" 
                                oncheckedchanged="cbDimen_CheckedChanged"/>
                                  <br />
                            <asp:CheckBox ID="cbBkno" runat="server" Text="Book No" 
                                oncheckedchanged="cbBkno_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbPublishNm" runat="server" Text="Publisher" 
                                oncheckedchanged="cbPublishNm_CheckedChanged"/>
                              <br />
                               <asp:CheckBox ID="cbPubPlace" runat="server" Text="Publisher Place" 
                                oncheckedchanged="cbPubPlace_CheckedChanged"/>
                              <br />
                            <asp:CheckBox ID="cbVol" runat="server" Text="Volume" 
                                oncheckedchanged="cbVol_CheckedChanged"/>
                                  <br />
                            <asp:CheckBox ID="cbISBN" runat="server" Text="ISBN" 
                                oncheckedchanged="cbISBN_CheckedChanged"/>
                                  <br />
                            <asp:CheckBox ID="cbPubYr" runat="server" Text="Pub Year" 
                                oncheckedchanged="cbPubYr_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbEdi" runat="server" Text="Edition" 
                                oncheckedchanged="cbEdi_CheckedChanged"/>
                            <br />
                            <asp:CheckBox ID="cbPage" runat="server" Text="Pages" 
                                oncheckedchanged="cbPage_CheckedChanged"/>
                                 <br />
                                 <asp:CheckBox ID="cbSourcetype" runat="server" Text="SourceType" 
                                oncheckedchanged="cbSourcetype_CheckedChanged"/>
                                 <br />
                                 <asp:CheckBox ID="cbSource" runat="server" Text="Source" 
                                oncheckedchanged="cbSource_CheckedChanged"/>
                                 <br />
                                 <asp:CheckBox ID="cbBillNo" runat="server" Text="Bill No" 
                                oncheckedchanged="cbBillNo_CheckedChanged"/>
                                 <br />
                                 <asp:CheckBox ID="cbBillDate" runat="server" Text="Bill Date" 
                                oncheckedchanged="cbBillDate_CheckedChanged"/>
                                 <br />
                                 <asp:CheckBox ID="cbReference" runat="server" Text="Reference" 
                                oncheckedchanged="cbReference_CheckedChanged"/>
                                 <br />
                                  <asp:CheckBox ID="cbPrice" runat="server" Text="Price/Piece" 
                                oncheckedchanged="cbPrice_CheckedChanged"/>
                                 <br />
                            <asp:CheckBox ID="cbstatus" runat="server" Text="Status" 
                                oncheckedchanged="cbstatus_CheckedChanged"/>
                            <br />
                        </td>
                        <td style="width:auto">
                            <asp:Label ID="lblReport" runat="server"> </asp:Label></td>
                    </tr>
                </table>
            </div>

          <%--  <div style="text-align: left; padding-left:0px; padding-right:0px;" class="tbltxt">
                <asp:Label ID="lblReport" runat="server"></asp:Label>
            </div>--%>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>

