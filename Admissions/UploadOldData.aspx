<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="UploadOldData.aspx.cs" Inherits="Admissions_UploadOldData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_fm.jpg" width="29" height="29">
    </div>
    <div style="padding-top: 5px;">
        <h2>
            Upload Student Excel</h2>
    </div>
     <div class="spacer">
    </div>
     <table width="100%" class="cnt-box" style="padding:0px !important";>
        <tr>
            <td style="width: 100%;" valign="middle" class="tbltxt">
                &nbsp; Upload OldData Excel File :
                <asp:FileUpload runat="server" ID="fileUpload" />
                  </td>
        </tr>
        <tr>
        <td style="width: 100%;" valign="middle" class="tbltxt">
        <asp:Button ID="Button" runat="Server" Text="Upload File" OnClick="Button_Clicked" />
            </td>
        </tr>
        
    </table>
</asp:Content>

