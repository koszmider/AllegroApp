<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderLog.ascx.cs" Inherits="LajtIt.Web.Controls.OrderLog" %>
<asp:GridView runat="server" ID="gvOrderLog" AutoGenerateColumns="false" EmptyDataText="Brak wpisów">
    <Columns>
        <asp:BoundField DataField="InsertDate" DataFormatString="{0:yy/MM/dd HH:mm}" HeaderText="Data" />
        <asp:BoundField DataField="InsertUser" HeaderText="InsertUser" />
        <asp:BoundField DataField="TableName" HeaderText="Tabela" />
        <asp:BoundField DataField="Comment" HeaderText="Komentarz" />
    </Columns>
</asp:GridView>
