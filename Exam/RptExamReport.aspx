<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Exam.master" AutoEventWireup="true" CodeFile="RptExamReport.aspx.cs" Inherits="Exam_RptExamReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 3px;">
        <h2>
            Exam Progress Report</h2>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" cellpadding="1px" style="background-color: #D3E7EE; outline-style: solid;
                outline-width: 1px">
               
                <tr>
                    <td align="left"
                        style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;">
                        Session&nbsp;:&nbsp;
                        <asp:DropDownList ID="drpSession" runat="server">
                        </asp:DropDownList>
                        Class&nbsp;:&nbsp;
                        <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpClass_SelectedIndexChanged">
                        </asp:DropDownList>
                        <%--Section&nbsp;:&nbsp;
                        <asp:DropDownList ID="drpSection" runat="server">
                        </asp:DropDownList>
                        Examination&nbsp;:&nbsp;<asp:DropDownList ID="drpExam" runat="server" AutoPostBack="true"
                            Height="16px" 
                            onselectedindexchanged="drpExam_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                        Student&nbsp;:&nbsp;
                        <asp:DropDownList ID="drpStud" runat="server" 
                            onselectedindexchanged="drpStud_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                    <td  align="center" 
                        style="padding: 5px; font-weight: bold; height: 3px; font-family: Tahoma, Geneva, sans-serif;">
                        <asp:Button ID="btnShow" runat="server" Text="Show" OnClientClick="return isValid();"
                            OnClick="btnShow_Click" onfocus="active(this);" onblur="inactive(this);" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="openwindow();"
                            onfocus="active(this);" onblur="inactive(this);" 
                            onclick="btnPrint_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <center>
                <asp:Label ID="lblReport" runat="server"></asp:Label></center>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="drpSession" />
            <asp:PostBackTrigger ControlID="drpClass" />
            <asp:PostBackTrigger ControlID="drpStud" />
            <asp:PostBackTrigger ControlID="btnShow" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

