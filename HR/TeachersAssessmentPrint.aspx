<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TeachersAssessmentPrint.aspx.cs" Inherits="HR_TeachersAssessmentPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Teacher's Assessment Report</title>

    <script language="javascript" type="text/javascript">
        function Print() { document.body.offsetHeight; window.print(); }			
    </script>

</head>
<body onload="Print();">
    <form id="form1" runat="server">
    <center>
        <div>
            <asp:Label ID="lblReport" runat="server"></asp:Label>
        </div>
    </center>
    </form>
</body>
</html>
