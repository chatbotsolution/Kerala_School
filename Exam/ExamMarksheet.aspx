<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExamMarksheet.aspx.cs" Inherits="Exam_ExamMarksheet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mark Sheet</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table border="1">
    <tr><td style="height:10px;border:0;"></td></tr>
    <tr>
    <td style="border:0;" colspan="4">
        <strong>
        <asp:Label runat="server" ID="lblName" Font-Size="Large"></asp:Label><br />
        Exam&nbsp;   :&nbsp;<asp:Label runat="server" ID="lblExam"></asp:Label><br />
        School :&nbsp;<asp:Label runat="server" ID="lblSchool"></asp:Label><br />
        Medha Exam Roll No. :&nbsp;<asp:Label runat="server" ID="lblRoll"></asp:Label><br />
        </strong>
     </td>
     <td valign="bottom" style="border:0" colspan="2">
        <strong>
        Result&nbsp;   :&nbsp;<asp:Label runat="server" ID="lblResult"></asp:Label><br />
        Total Marks   :&nbsp;<asp:Label runat="server" ID="lblTotMarks"></asp:Label><br />
        </strong>
     </td>
     </tr>
     <tr>
     <td align="center" colspan="6" style="background-color:InactiveBorder;">
       <h2>MARK SHEET</h2>
     </td>
     </tr>
     <tr>
     <td colspan="3" align="center">
        <strong> PAPER I</strong>
     </td>
     <td colspan="3" align="center">
        <strong> PAPER II</strong>
     </td>
     </tr>
     <tr>
         <td >
            <strong>SUBJECT</strong>
         </td>
         <td>
            <strong>FULL MARKS</strong>
         </td>
         <td>
            <strong>MARKS SECURED</strong>
         </td>
         <td>
            <strong>SUBJECT</strong>
         </td>
         <td>
            <strong>FULL MARKS</strong>
         </td>
         <td>
            <strong>MARKS SECURED</strong>
         </td>
     </tr>
     <tr>
         <td>
            Mathematics :
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblMathFM"></asp:Label>
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblMath"></asp:Label>
         </td>
         <td>
            M.I.L. :
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblMILFM"></asp:Label>
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblMIL"></asp:Label>
         </td>
     </tr>
     <tr>
        <td>
            Science : 	
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblScFM"></asp:Label>
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblSc"></asp:Label>
         </td>
         <td>
            English : 	
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblEngFM"></asp:Label>
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblEng"></asp:Label>
         </td>
     </tr>
     <tr>
        <td>
            Pratyutpanamotitwa : 	 	
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblPratFM"></asp:Label>
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblPrat"></asp:Label>
         </td>
         <td>
            S.S.T. : 	
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblSSTFM"></asp:Label>
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblSST"></asp:Label>
         </td>
     </tr>
     <tr>
        <td>
            General Knowledge : 	 	
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblGKFM"></asp:Label>
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblGK"></asp:Label>
         </td>
         <td>
            Cultural Knowledge: 	
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblCulFM"></asp:Label>
         </td>
         <td align="right">
            <asp:Label runat="server" ID="lblCul"></asp:Label>
         </td>
     </tr>
     <tr>
        <td>
           <b>Paper I Total :</b>
         </td>
         <td align="right">
            <b><asp:Label runat="server" ID="lblTot1FM"></asp:Label></b>
         </td>
         <td align="right">
           <b><asp:Label runat="server" ID="lblTot1"></asp:Label></b>
         </td>
         <td>
            <b>Paper II Total :</b>	
         </td>
         <td align="right">
            <b><asp:Label runat="server" ID="lblTot2FM"></asp:Label></b>
         </td>
         <td align="right">
            <b><asp:Label runat="server" ID="lblTot2"></asp:Label></b>
         </td>
     </tr>
     </table>
    </div>
    </form>
</body>
</html>
