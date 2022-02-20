<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="Product.New.aspx.cs" Inherits="LajtIt.Web.ProductNew" %>

<%@ Register Src="~/Controls/ProductCatalogGroup.ascx" TagName="ProductCatalogGroup" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Nowy produkt</h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <asp:Panel runat="server" GroupingText=" ">
                <table>
                    <colgroup>
                        <col width="250" />
                        <col width="650" />
                    </colgroup>
                    <tr>
                        <td>Dostawca
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSuppliers" DataTextField="DisplayName" DataValueField="SupplierId" AutoPostBack="true" OnSelectedIndexChanged="ddlSuppliers_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">

                            <uc:ProductCatalogGroup runat="server" ID="ucProductCatalogGroup" />

                        </td>
                    </tr>
                    <tr>
                        <td>Nazwa
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txbName" AutoPostBack="true" MaxLength="100" Width="500"
                                ValidationGroup="client"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator3" ValidationGroup="client" ControlToValidate="txbName"
                                    Text="*" runat="server"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Kod produktu
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txbCode" AutoPostBack="true" MaxLength="100" Width="500" OnTextChanged="txbCode_OnTextChanged"
                                ValidationGroup="client"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator2" ValidationGroup="client" ControlToValidate="txbCode"
                                    Text="*" runat="server"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Cena   (brutto)
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txbPriceBrutto" AutoPostBack="true" MaxLength="10"
                                Width="500" ValidationGroup="client"></asp:TextBox><asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator1" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" Display="Dynamic"
                                    ValidationGroup="client" ControlToValidate="txbPriceBrutto" Text="*" runat="server"></asp:RegularExpressionValidator><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator1" ValidationGroup="client" ControlToValidate="txbPriceBrutto"
                                        Text="*" runat="server"></asp:RequiredFieldValidator>
                        </td>
                    </tr> 
                    <tr>
                        <td>Rodzaj produktu
                        </td>
                        <td>
                            <asp:RadioButton runat="server" ID="rbProductTypeId1" Text="Zwykły" GroupName="rp" Checked="true" />
                            <asp:RadioButton runat="server" ID="rbProductTypeId3" Text="Zestaw" GroupName="rp"/>
                        </td>
                    </tr> 
                    <tr>
                        <td colspan="2">
                            <asp:Button runat="server" ID="btnProductNewSave" Text="Zapisz" OnClick="btnProductNewSave_Click" OnClientClick="if(confirm('Czy zapisać nowy produkt?')) return true;"
                                ValidationGroup="client" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
