<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EmailEditor.aspx.cs" Inherits="LajtIt.Web.EmailEditor" EnableEventValidation="false" ValidateRequest="false"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server"  >
     <Triggers><asp:PostBackTrigger ControlID="btnSave" /></Triggers>
     <ContentTemplate>
    <asp:DropDownList runat="server" ID="ddlEmailTemplates" DataValueField="EmailTemplateId"
        DataTextField="TemplateName" AppendDataBoundItems="true">
        <asp:ListItem>-- Wybierz --</asp:ListItem>
    </asp:DropDownList>
    <asp:Button runat="server" ID="btnOpen" OnClick="btnOpen_Click" Text="Edytuj" /><br />
    <br />
    <asp:Panel runat="server" ID="pnlEditor" Visible="false">
        <div style="width: 620px; float: left; padding: 20px">
            Od email:<br />
            <asp:TextBox runat="server" ID="txbFromEmail" MaxLength="100" Style="width: 500px" /><br />
            Od nazwa:<br />
            <asp:TextBox runat="server" ID="txbFromName" MaxLength="100" Style="width: 500px" /><br />
            Temat:<br />
            <asp:TextBox runat="server" ID="txbSubject" MaxLength="100" Style="width: 600px" /><br />
            Treść maila:<br />
            <asp:TextBox runat="server" Rows="15" Columns="50" ID="txbBody" TextMode="MultiLine"
                Style="width: 600px" /><br />
            <br />
       
            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Zapisz" />
            <asp:Button runat="server" ID="btnPreview" OnClick="btnPreview_Click" Text="Podgląd" />
        </div>
        <div style="padding: 200px 0 0 20px;">
            Wstaw tag do maila, zostanie on zastąpiony dynamiczne z docelową wartością:
            <ul>
                <li>[COMMENT] - treść oryginalnego komentarza</li>
                <li>[USER_NAME] - nazwa użytkownika</li>
                <li>[USER_FIRSTNAME] - imię</li>
                <li>[USER_LASTNAME] - nazwisko</li>
                <li>[EMAIL] - adres email</li>
                <li>[AUCTION_NAME] - tytuł aukcji</li>
                <li>[REBATE_CODE] - kod rabatu</li>
                <li>[REBATE_END_DATE] - koniec rabatu</li>
                <li>[REBATE_VALUE] - wartość rabatu</li>
            </ul>
        </div>
    <div style="clear: both;"></div>
      <asp:Panel runat="server" ID="pnlPreview" Visible = "false" GroupingText="Podgląd wiadomości">
    <asp:Literal runat="server" ID="litPreview" /> 
    </asp:Panel>
    </asp:Panel>
    <div style="clear: both;">
    </div>
    <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="Anuluj"
        Visible="false" />
        </ContentTemplate></asp:UpdatePanel>
</asp:Content>
