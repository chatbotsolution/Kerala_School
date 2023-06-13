<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.master" AutoEventWireup="true" CodeFile="MasterControl.aspx.cs" Inherits="Administrations_MasterControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<center><asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>&emsp;&emsp;<asp:Label ID="lblMsg2" runat="server" Text=""></asp:Label></center>
<br /> <br />
    To Generate Bus/Hostel Fee Ledger, If No Entry Exists In Ps_AdFeeLedger & BusHostelChoice Table For Current Session
    <br />
    <asp:Button ID="btnGenBus" runat="server" Text="Generate Bus/Hostel Fee Ledger" 
        onclick="btnGenBus_Click" />
    <br />
    <br />
    To Generate General Ledger Records In case of New Module Installation In Old School 
    <br />(In The Code Behind Anudan Account Head Is Hardcoded Create One Anudan Account Head Before Generating Ledger)
    <br />(The date to fetch Receipt no has also been hard coded please change before executing)
    <br /><asp:Button ID="btnGenLedger" runat="server" 
        Text="Generate General Ledger For Old School" onclick="btnGenLedger_Click" />
        <br />
        <br />
     To Generate Fee Ledger Records For Late Fine In case of New Module Installation In Old School  
     <br />( Fee Head For Late Fine & School Id are also HardCoded In the Store Procedure Please change FeeId Before Generating Ledger )
     <br />( The receipt no starting part has been hard coded to fetch Receiptno please change if required)
     <br /><asp:Button ID="btnGenFine" runat="server" Text="Generate Fee Ledger For Fine" 
        onclick="btnGenFine_Click" />
        <br />
        <br />
      To Generate Fee Ledger Records(Pending Entry) For Late Fine In case of New Module Installation In Old School  
     <br />( Fee Head For Late Fine & Sessionyear are HardCoded In the Store Procedure Please change FeeId Before Generating Ledger )
     
     <br /><asp:Button ID="btnFineUnpaid" runat="server" 
        Text="Generate Fee Ledger For Fine(Pending)" onclick="btnFineUnpaid_Click" />
</asp:Content>

