<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductFileImport.aspx.cs" Inherits="LajtIt.Web.ProductFileImport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Import produktów</h1>


    <asp:Panel runat="server" GroupingText="Wczytaj plik">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:DropDownList runat="server" ID="ddlSuppliers" DataValueField="SupplierId" DataTextField="Name" ValidationGroup="save"></asp:DropDownList></td>
                <td>
                    <asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:PostBackTrigger ControlID="lbtnTemplate" />
                        </Triggers>
                        <ContentTemplate>
                            <input type="file" id="myfile" name="myfile" runat="server" size="100" />&nbsp;&nbsp;&nbsp;
                                     
                                    <asp:Button ID="btnSave" runat="server" Text="Zapisz pliki" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="save" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="text-align: right">
                    <asp:LinkButton Visible="false" runat="server" ID="lbtnTemplate" OnClick="lbtnTemplate_Click">Pobierz szablon</asp:LinkButton>
                    <a href="LajtitImport.xls">Pobierz szablon</a>

                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:RadioButtonList runat="server" ID="rblActionType">
                        <asp:ListItem Selected="True">Dodwania/aktualizacja</asp:ListItem>
                        <asp:ListItem>Automatyczne przetwarzanie</asp:ListItem>

                    </asp:RadioButtonList></td>
            </tr>
            <tr><td colspan="3"><asp:TextBox runat="server" ID="txbComment" ValidationGroup="save"></asp:TextBox></td>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txbComment" ErrorMessage="*" Text="*" InitialValue="Komentarz"  Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txbComment" ErrorMessage="*" Text="*"  Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                    TargetControlID="txbComment"
                    WatermarkText="Komentarz" 
                    WatermarkCssClass="watermarked" /></tr>
        </table>
    </asp:Panel>

    <asp:Panel runat="server" GroupingText="Wczytaj plik">
        <table style="width: 100%">
            <tr>
                <td>Status:
                    <asp:DropDownList runat="server" ID="ddlStatus">
                        <asp:ListItem Value="1">Pliki aktywne</asp:ListItem>
                        <asp:ListItem Value="0">Pliki nieaktywne</asp:ListItem>

                    </asp:DropDownList>

                    <asp:Button ID="btnSearch" runat="server" Text="Zapisz pliki" OnClick="btnSearch_Click" CausesValidation="false" />

                </td>
                <td style="text-align: right"><a href="LajtitImport.xls">Pobierz szablon</a></td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:GridView runat="server" ID="gvFiles" AutoGenerateColumns="false">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="ProductCatalogFileId" DataTextField="InsertDate" DataNavigateUrlFormatString="/ProductFileImportProcessing.aspx?id={0}" HeaderText="Data importu" />
            <asp:BoundField DataField="InsertUser" HeaderText="Użytkownik" />
            <asp:BoundField DataField="InsertDate" HeaderText="Data" DataFormatString="{0:yyyy/MM/dd HH:ss}" />
            <asp:BoundField DataField="SupplierName" HeaderText="Dostawca" />
            <asp:BoundField DataField="Comment" HeaderText="Komentarz" />
            <asp:BoundField DataField="FileImportStatusName" HeaderText="Status" />
            <asp:HyperLinkField DataNavigateUrlFormatString="/Files/ImportFiles/{0}" DataNavigateUrlFields="FileName" DataTextField="FileName" HeaderText="Plik" />

        </Columns>

    </asp:GridView>
</asp:Content>
