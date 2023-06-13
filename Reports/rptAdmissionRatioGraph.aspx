<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptAdmissionRatioGraph.aspx.cs" Inherits="Reports_rptAdmissionRatioGraph" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function beginRequest(sender, args) {
            // show the popup
            $find('<%=mdlloading.ClientID %>').show();

        }

        function endRequest(sender, args) {
            //  hide the popup
            $find('<%=mdlloading.ClientID %>').hide();

        }
                
    </script>
    
    <asp:UpdatePanel runat="server" ID="updadddetail" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="background-color: #ededed;">
                    <td width="200" align="left" style="background-color: ">
                        <div class="headingcontainor">
                            <h1>
                                Student Admission Graph
                            </h1>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-left:10px">
                        <div>
                            <div style="float:left;width:270px;">
                                <asp:RadioButtonList ID="rdbtnAdmission" runat="server" AutoPostBack="true" 
                                    RepeatDirection="Horizontal" onselectedindexchanged="rdbtnAdmission_SelectedIndexChanged">                                                                        
                                    <asp:ListItem Text="All Classes In a Session" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Session Wise" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>&nbsp;&nbsp;&nbsp;
                            </div>
                            <div id="dvSessiionYr" runat="server" style="float:left;">
                                Session Year :
                                <asp:DropDownList ID="ddlSession" runat="server">
                                    <%--<asp:ListItem Text="2012-13" Value="2012-13" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="2013-14" Value="2013-14"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-left:10px">
                        <div>
                            <div style="float:left;width:200px;">
                                <asp:RadioButtonList ID="rdbtnOrientation" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Normal View" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="3D View" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="float:left; width:20px;padding-top:7px;">
                                |
                            </div>
                            <div style="float:left;">
                                &nbsp;&nbsp;&nbsp; Chat Type :&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlChartType" runat="server">
                                    <asp:ListItem Text="Column" Value="Column" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Bar" Value="Bar"></asp:ListItem>
                                    <asp:ListItem Text="Line" Value="Line"></asp:ListItem>
                                    <asp:ListItem Text="BoxPlot" Value="BoxPlot"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnShow" runat="server" Text="Show Chart" onclick="btnShow_Click" />                                    
                            </div>
                        </div>
                    </td>
                </tr>                
            </table>
            <div class="spacer">
                <img src="../Images/mask.gif" height="10" width="10" /></div>
            <div>
                <asp:Chart ID="Chart1" runat="server" Height="296px" Width="412px" BorderDashStyle="Solid"
                    BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" BackColor="211, 223, 240"
                    BorderColor="#1A3B69">
                    <Series>
                        <asp:Series Name="Series1" ChartArea="MainChartArea" BorderWidth="2" BackHatchStyle="BackwardDiagonal">                            
                        </asp:Series>
                        <%--<asp:Series Name="Series2" ChartArea="MainChartArea" BorderWidth="2" Color="Green">                            
                        </asp:Series>--%>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="MainChartArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                            BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                            BackGradientStyle="TopBottom" Area3DStyle-Enable3D="true" >                            
                            <AxisY LineColor="64, 64, 64, 64">
                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                <MajorGrid LineColor="64, 64, 64, 64" />
                            </AxisY>
                            <AxisX LineColor="64, 64, 64, 64">
                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                <MajorGrid LineColor="64, 64, 64, 64" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                    <Titles>
                        <asp:Title Text="Student Admission Ratio" ForeColor="Green">
                        </asp:Title>
                    </Titles>
                </asp:Chart>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="mdlloading" runat="server" TargetControlID="pnlloading"
                PopupControlID="pnlloading" BackgroundCssClass="Background" />
            <asp:Panel ID="pnlloading" runat="server" Style="display: none">
                <div align="center" style="margin-top: 13px;">
                    <img src="../Images/loading.gif" />
                    <span>Loading ...</span>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

