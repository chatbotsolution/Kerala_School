<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExamRemarks.aspx.cs" Inherits="Exam_ExamRemarks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Set Exam Remarks</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-top: 3px;">
        <h2 style="font-family: Trebuchet MS, Arial, Helvetica, sans-serif; font-size: 18px;
            color: #090; display: inline;">
            Set Remarks</h2>
    </div>
    <br />
    <table align="center" width="100%">
        <tr>
            <td align="left" valign="baseline">
                Student Name&nbsp;:&nbsp;<asp:Label ID="lblStudentName" runat="server"></asp:Label>
            </td>
            <td align="right" valign="baseline">
                Admission No&nbsp;:&nbsp;<asp:Label ID="lblAdmnNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" valign="baseline" colspan="2">
                Enter Remarks
            </td>
        </tr>
        <tr>
            <td align="left" valign="baseline" colspan="2">
                <asp:TextBox runat="server" ID="txtRemarks" TextMode="MultiLine" Width="100%" Height="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" valign="baseline" colspan="2">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                    onfocus="active(this);" onblur="inactive(this);" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
