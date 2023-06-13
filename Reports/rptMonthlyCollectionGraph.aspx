<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="rptMonthlyCollectionGraph.aspx.cs" Inherits="Reports_rptMonthlyCollectionGraph" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<script runat="server">

    protected void Chart1_Load(object sender, EventArgs e)
    {

    }
</script>
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
                                Month Wise Fee Collection
                            </h1>
                        </div>
                    </td>
                    <td height="35" align="left" valign="middle">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-left:10px">
                        Session Year :
                        <asp:DropDownList ID="ddlSession" runat="server">
                            <asp:ListItem Text="2012-13" Value="2012-13" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="2013-14" Value="2013-14"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp; Fee Type :&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlFeeType" runat="server">
                            <asp:ListItem Text="--All Fee--" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Student Fee" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Miscellaneous Fee" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp; Chat Type :&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlChartType" runat="server">
                            <asp:ListItem Text="Column" Value="Column" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Bar" Value="Bar"></asp:ListItem>
                            <asp:ListItem Text="Line" Value="Line"></asp:ListItem>
                            <asp:ListItem Text="BoxPlot" Value="BoxPlot"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnShow" runat="server" Text="Show Chart" OnClick="btnShow_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rdbtnOrientation" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Normal View" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="3D View" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <div class="spacer">
                <img src="../Images/mask.gif" height="10" width="10" /></div>
            <div>
                <asp:Chart ID="Chart1" runat="server" Height="296px" Width="412px" BorderDashStyle="Solid"
                    BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" BackColor="211, 223, 240"
                    BorderColor="#1A3B69" onload="Chart1_Load">
                    <Series>
                        <asp:Series Name="Series1" ChartArea="MainChartArea" BorderWidth="2">                            
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="MainChartArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                            BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                            BackGradientStyle="TopBottom" Area3DStyle-Enable3D="true" >
                            <%--<Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                WallWidth="0" IsClustered="False"></Area3DStyle>--%>
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
                        <asp:Title Text="Fee Collection Report" ForeColor="Green">
                        </asp:Title>
                    </Titles>
                    <%--<Legends>
                <asp:Legend LegendItemOrder="SameAsSeriesOrder">
                    <CellColumns>
                        <asp:LegendCellColumn>
                        </asp:LegendCellColumn>
                    </CellColumns>
                    <CustomItems>
                        <asp:LegendItem>                            
                        </asp:LegendItem>
                    </CustomItems>
                </asp:Legend>
            </Legends>--%>
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

