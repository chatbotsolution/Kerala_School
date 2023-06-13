<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Inner.master" AutoEventWireup="true" CodeFile="Online_Admn_XII.aspx.cs" Inherits="Admissions_Online_Admn_XII" %>
     <%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<script type="text/javascript">
    //variable that will store the id of the last clicked row
    var previousRow;
    var previousrowColor;

    function ChangeRowColor(row) {
        debugger;
        var color = document.getElementById(row).style.backgroundColor;
        //If last clicked row and the current clicked row are same
        if (previousRow == row)
            return; //do nothing
        //If there is row clicked earlier
        else if (previousRow != null)
        //change the color of the previous row back to white
            document.getElementById(previousRow).style.backgroundColor = previousrowColor;

        //change the color of the current row to light yellow

        document.getElementById(row).style.backgroundColor = "#f1d3c9";
        //assign the current row id to the previous row id
        //for next row to be clicked
        previousRow = row;
        previousrowColor = color;
    }
</script>

    <div style="width: 29px; height: 29px; float: left; margin-right: 5px;">
        <img src="../images/icon_cp.jpg" width="29" height="29"></div>
    <div style="padding-top: 5px;">
        <h2>
            Online Admission Apply List(XI-XII)</h2>
           <span style=" margin-left:150px;"><asp:Label ID="lblMsg" runat="server" Text="Label"></asp:Label></span> 
    </div>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="spacer"></div>
             <table width="100%">
             
                            <tr>

                                <td align="left" valign="top" class="tbltxt cnt-box2 " width="49%"   >
                                    
              
                                   <span class="ttl3"><asp:Label ID="lblSession" runat="server" Text="Session :"></asp:Label></span>
                                    <asp:DropDownList ID="drpSession" runat="server" AutoPostBack="True"  
                                        OnSelectedIndexChanged="drpSession_SelectedIndexChanged" CssClass="tbltxtbox largetb1 wdth-134"
                                        TabIndex="1">
                                    </asp:DropDownList><br /><br />
                                     
                                   <span class="ttl3"><asp:Label ID="lblClass" runat="server" Text="Class :"></asp:Label></span>
                                    <asp:DropDownList ID="drpClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpClass_SelectedIndexChanged"
                                        CssClass="tbltxtbox largetb1  wdth-134" TabIndex="2">
                                    </asp:DropDownList><br /><br />
                                                        <span class="ttl3"><asp:Label ID="Label2" runat="server" Text="Student Status :"></asp:Label></span>
                                    <asp:DropDownList ID="drpStudStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpStudStatus_SelectedIndexChanged"
                                        CssClass="tbltxtbox largetb1  wdth-134" TabIndex="4">
                                    </asp:DropDownList>


                                  <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" TabIndex="6" Text="Show" CssClass="btn" />
                                   
                            <%--         <span class="ttl3"><asp:Label ID="lblSection" runat="server" Text="Section :"></asp:Label></span>--%>
                                    <asp:DropDownList ID="drpSection" runat="server"   AutoPostBack="True" Visible="false"
                                        OnSelectedIndexChanged="drpSection_SelectedIndexChanged" CssClass="tbltxtbox largetb1  wdth-134"
                                        TabIndex="3">
                                    </asp:DropDownList><br /><br />
                                    
                         <%--            <span class="ttl3"><asp:Label ID="lblSelectStudent" runat="server" Text="Select Student :"></asp:Label></span>--%>
                                    <asp:DropDownList ID="drpSelectStudent" runat="server" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="drpSelectStudent_SelectedIndexChanged"
                                        CssClass="tbltxtbox largetb1  wdth-134" TabIndex="4">
                                    </asp:DropDownList><br /><br />

                                     
                   
                                       <asp:CheckBox ID="chkCasual" Text="Casual Student" runat="server" CssClass="chklst1" Visible="false" />                               
                                   
                                </td>
                                <td width="2%" align="center" valign="middle" class="tbltxt"></td>
                                <td align="left" valign="top" class="tbltxt cnt-box2" width="49%">
                                    <div >  
                                    <div class="tbltxt"> </div>
                                   <asp:DropDownList ID="drpSearch" runat="server" Visible="false"  CssClass="tbltxt" Width="40%" OnSelectedIndexChanged="drpSearch_SelectedIndexChanged" AutoPostBack="true" >
                    <asp:ListItem Value="0">--Select to Search--</asp:ListItem>
                     <asp:ListItem Value="1">Name</asp:ListItem>
                      <asp:ListItem Value="2">Class</asp:ListItem>
                       <asp:ListItem Value="3">Section</asp:ListItem>
                        <asp:ListItem Value="4">Stream</asp:ListItem>
                        <asp:ListItem Value="5">Session</asp:ListItem>
                          <asp:ListItem Value="6">Roll No</asp:ListItem>
                           <asp:ListItem Value="7">Admission No</asp:ListItem>
                           <asp:ListItem Value="21">Religion</asp:ListItem>
                           <asp:ListItem Value="22">Gender</asp:ListItem>
                            <asp:ListItem Value="23">Category</asp:ListItem>
                              <asp:ListItem Value="24">Status</asp:ListItem>
                              <asp:ListItem Value="25">2nd Language</asp:ListItem>
                               <asp:ListItem Value="26">6th Subject</asp:ListItem>

                         
                            <asp:ListItem Value="8">Admission Date</asp:ListItem>
                             <asp:ListItem Value="9">Date Of Birth</asp:ListItem>
                             <asp:ListItem Value="10">Place Of Birth</asp:ListItem>
                              <asp:ListItem Value="11">Blood Group</asp:ListItem>
                               <asp:ListItem Value="12">City</asp:ListItem>
                               <asp:ListItem Value="13">Father's Name</asp:ListItem> 
                                <asp:ListItem Value="14">Mother's Name</asp:ListItem>
                                <asp:ListItem Value="15">Father's Occupation</asp:ListItem>
                                   <asp:ListItem Value="16">Mother's Occupation</asp:ListItem>
                                  <asp:ListItem Value="17">Father's Designation</asp:ListItem>
                                   <asp:ListItem Value="18">Mother's designation</asp:ListItem>
                                   <asp:ListItem Value="19">Father's Organisation</asp:ListItem>
                                   <asp:ListItem Value="20">Mother's Organisation</asp:ListItem>
                                  </asp:DropDownList>
                                  <asp:TextBox ID="txtSearch" runat="server"  Visible="false" CssClass="tbltxt" Width="40%" placeholder="Text Here"  > </asp:TextBox>
                                    <asp:TextBox ID="txtDate" runat="server" Visible="false" CssClass="tbltxt" Width="40%"  > </asp:TextBox>
                                      <rjs:PopCalendar ID="PopCalAdmnDt" runat="server" Control="txtDate" Visible="false" ></rjs:PopCalendar>
                                   
                                        </div><br />
                                        <div> <asp:Button ID="BtnSearch" Visible="false" runat="server" Text="Search" OnClick="BtnSearch_Click"/></div>
                                        <br /><br />
                                    
                                    <asp:Button ID="btnDelete" OnClientClick="return CnfDelete()" Visible="false" Enabled="false" runat="server" OnClick="btnDelete_Click" Text="Delete Student" CssClass="btn"
                                        TabIndex="7" />
                                    <asp:Button ID="btnExport" runat="server" Text="Export Data For Website" Visible="false" OnClick="btnExport_Click" CssClass="btn"
                                        TabIndex="8"  />
                                        <div class="spacer"></div><div class="spacer"></div>
                                        <div>
                <asp:Label ID="Label1" runat="server" Font-Bold="true" CssClass="tbltxt" ForeColor="Red"></asp:Label></div>
                                </td>
                            </tr>
                            </table>
             <table width="100%">
    <tr>
    <td width="70%">
    <asp:Label ID="lblCount" runat="server" Visible="false" CssClass="tbltxt"></asp:Label>
    </td>
    <td>
     <label class="tbltxt"> TC : </label> <div style="height:15px;width:30px; display:inline-block; background-color:Yellow; "></div> &nbsp;&nbsp;</td>
     <td> <label class="tbltxt">PassOut :</label>  <div style="height:15px;width:30px; display:inline-block;  background-color:Pink "></div> &nbsp;&nbsp;</td>
      <td> <label class="tbltxt">Absent :</label>  <div style="height:15px;width:30px; display:inline-block;  background-color:Cyan; "></div> </td>
    </tr>
    <tr>
    <td colspan="4">
    <div style=" overflow:scroll; height:450px;">
                                        <asp:GridView ID="grdstuddet" runat="server" Width="100%" AutoGenerateColumns="False"
                                        OnRowDataBound="grdstuddet_RowDataBound"
                                        CssClass="mGrid tbltxt" AlternatingRowStyle-CssClass="alt"
                                        TabIndex="7">
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <Columns>
                                                       <asp:TemplateField HeaderText="From Number">
                                                <ItemTemplate>
                                                    <a href='https://kemsadmission.kemschampua.co.in/Admission/Nursery_to_Class_X'>
                                                    <asp:Label ID="lblclass" runat="server" Text='<%#Eval("Form_No")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                             <asp:BoundField HeaderText="Student Name" DataField="Student_Name"/>

                                            <asp:TemplateField HeaderText="Applied Class">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclass" runat="server" Text='<%#Eval("Class")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                   
                                            <asp:BoundField DataField="FName" HeaderText="Father Name" />
                                            <asp:BoundField DataField="MName" HeaderText="Mother Name" />
                                             <asp:BoundField DataField="Gender" HeaderText="Gender" />
                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldob" runat="server" Text='<%#Eval("DOB")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Father MOB">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltel" runat="server" Text='<%#Eval("F_Mob1")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mother MOB">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltel" runat="server" Text='<%#Eval("M_Mob1")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Mail ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltel" runat="server" Text='<%#Eval("G_Email1")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Aadhar Card No" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lbladmdt" runat="server" Text='<%#Eval("Aadhar_Card")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>   
                                             <asp:TemplateField HeaderText="Admission Status" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lbladmdt" runat="server" Text='<%#Eval("AdmnConformedStatus")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle BackColor="#EFEFEF" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle BackColor="#333333" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="#FDFDFD" />
                                        <EmptyDataTemplate>
                                            No Record
                                        </EmptyDataTemplate>
                                    </asp:GridView>
    </div>
    
    </td>
    </tr>
    </table>
         </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

