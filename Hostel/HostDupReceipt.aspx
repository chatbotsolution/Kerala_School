<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Hostel.master" AutoEventWireup="true" CodeFile="HostDupReceipt.aspx.cs" Inherits="Hostel_HostDupReceipt" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" language="javascript">

    function isValid() {

        var RcptNo = document.getElementById("<%=drpReciept.ClientID%>").value;


        var student = document.getElementById("<%=drpstudent.ClientID%>").value;
        if (student == "" || student == "0") {
            alert("Please select a student !");
            document.getElementById("<%=drpstudent.ClientID%>").focus();
            return false;
        }

        if (RcptNo == "" || RcptNo == "0") {
            alert("Please Select Receipt Number !");
            document.getElementById("<%=drpReciept.ClientID%>").focus();
            return false;
        }
        else {

            return true;
        }
    }
        
    </script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_rep.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Duplicate Receipt</h2>
    </div>
    <div class="spacer">
        <img src="../images/mask.gif" height="8" width="10" /></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <fieldset id="fsStudDetails" runat="server">
                <legend style="background-color: window" id="lgprchs" class="tbltxt" runat="server">
                    Stusent Details :-</legend>
                <table border="0" cellspacing="2" cellpadding="2" width="100%">
                    <tr>
                        <td width="60" class="tbltxt">
                            Session
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td style="width: 100px">
                            <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSession_SelectedIndexChanged"
                                CssClass="vsmalltb" TabIndex="1">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" style="width: 34px">
                            Class
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="120">
                            <asp:DropDownList ID="drpClasses" runat="server" AutoPostBack="True" CssClass="vsmalltb"
                                OnSelectedIndexChanged="drpClasses_SelectedIndexChanged" TabIndex="4">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt">
                            Student
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpstudent" runat="server" AutoPostBack="True" CssClass="largetb"
                                OnSelectedIndexChanged="drpstudent_SelectedIndexChanged" TabIndex="2">
                            </asp:DropDownList>
                        </td>
                        <td class="tbltxt" style="width: 79px">
                            Admission No
                        </td>
                        <td class="tbltxt">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtadminno" runat="server" CssClass="vsmalltb" AutoPostBack="True"
                                OnTextChanged="txtadminno_TextChanged" TabIndex="5"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <table border="0" cellspacing="2" cellpadding="2" width="100%">
                    <tr>
                        <td width="60" class="tbltxt">
                            Receipt No
                        </td>
                        <td width="5" class="tbltxt">
                            :
                        </td>
                        <td width="160" class="tbltxt">
                            <asp:DropDownList ID="drpReciept" runat="server" CssClass="vsmalltb" Width="150px"
                                TabIndex="3">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return isValid();"
                                TabIndex="8" />&nbsp;
                            <asp:Button ID="btnPrint" runat="server" Text="Print Detail" TabIndex="9" OnClick="btnPrint_Click" />
                            <asp:Button ID="btnConsolidat" runat="server" Text="Print Consolidated" TabIndex="10"
                                OnClick="btnConsolidat_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div align="center" style="padding-top: 10px;">
                <asp:Label ID="lbldetail" runat="server"></asp:Label></div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="drpstudent" EventName="SelectedIndexChanged">
            </asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


