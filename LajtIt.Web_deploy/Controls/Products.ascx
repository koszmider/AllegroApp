<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Products.ascx.cs" Inherits="LajtIt.Web.Controls.Products" %>
<asp:Panel runat="server" GroupingText="Nowy produkt">
    <asp:UpdatePanel runat="server" ID="upProducts">
        <ContentTemplate>
            <table>
                <tr>
                    <td style="width: 80px">
                        Dostawca:
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlSupplier" AutoPostBack="true" OnSelectedIndexChanged="ddlSupplier_OnSelectedIndexChanged"
                            ValidationGroup="pr" DataTextField="DisplayName" DataValueField="SupplierId">
                        </asp:DropDownList>
                        <span style="position: absolute;">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upProducts">
                                <ProgressTemplate>
                                    <img src="Images/progress.gif" style="height: 20px" alt="" /></ProgressTemplate>
                            </asp:UpdateProgress>
                        </span>
                    </td>
                </tr>
                <tr style="vertical-align:middle">
                    <td style="width: 80px">
                        Produkt:<br />&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlProducts" DataTextField="Name" DataValueField="ProductCatalogId"
                            AutoPostBack="true" ValidationGroup="pr" OnSelectedIndexChanged="ddlProducts_OnSelectedIndexChanged"
                            Style="font-family: 'Courier New' , Courier, Fixed, monospace; width:600px;" />
                        Ilość:
                        <asp:TextBox runat="server" Text="1" MaxLength="4" Width="20" ID="txbQuantity" ValidationGroup="pr" /><asp:Button
                            runat="server" ID="btnProductAdd" Text="Dodaj" OnClick="btnProductAdd_Click"
                            ValidationGroup="pr" /><asp:LinkButton runat="server" ID="lnbProductCancel" Text="Anuluj"
                                OnClick="lnbProductCancel_Click" /><br />
                        
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
