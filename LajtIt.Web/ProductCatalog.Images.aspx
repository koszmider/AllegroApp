<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductCatalog.Images.aspx.cs" 
    Inherits="LajtIt.Web.ProductCatalog_Images" %>

<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<%@ Register Src="~/Controls/UploadImage.ascx" TagName="UploadImage" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:ProductMenu runat="server" SetTab="td9"></uc:ProductMenu>
    <uc:UploadImage Id="ucUploadImage" runat="server"></uc:UploadImage>
    <asp:UpdatePanel ID="upImages" runat="server">
        <ContentTemplate>
            <asp:GridView runat="server" ID="gvImages" AutoGenerateColumns="false" OnRowDataBound="gvImages_OnRowDataBound"
                Width="100%" ShowHeader="true" ShowFooter="true">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="200">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlImage" Target="_blank">
                                <asp:Image runat="server" ID="imgImage" Width="200" BorderWidth="0" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hidImageId" />
                            <table style="width:100%">
                                <tr>
                                    <td style="width:100px;">Nazwa pliku:
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="litFileName"></asp:Literal>
                                        (
                                                            <asp:Literal runat="server" ID="litDate"></asp:Literal>,
                                                            <asp:Literal runat="server" ID="litSize"></asp:Literal>
                                        )
                                    </td>
                                </tr>
                             <%--   <tr>
                                    <td>Opis (pod zdjęciem oraz ALT)
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txbDescription" MaxLength="254" Width="300"></asp:TextBox></asp:TextBox><asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator3" runat="server" ControlToValidate="txbDescription"
                                            ErrorMessage="Uzupełnij opis zdjęcia" Text="*" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tytuł (dymek nad zdjęciem)
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txbTitle" MaxLength="254" Width="300"></asp:TextBox><asp:RequiredFieldValidator
                                            runat="server" ControlToValidate="txbTitle" ErrorMessage="Uzupełnij tytuł zdjęcia"
                                            Text="*" />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>Aktywny</td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chbIsActive" Text="" />
                                        <asp:CheckBox runat="server" ID="chbIsThumbnail" Text="Użyj jako miniaturki" Visible="false" />
                                    </td>
                                </tr>
                    <%--            <tr>
                                    <td>Typ zdjęcia:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlImageTypeId" Visible="false">
                                            <asp:ListItem Value="1">Zdjęcie główne (duże, lewa kolumna aukcji)</asp:ListItem>
                                            <asp:ListItem Value="2">Zdjęcie techniczne (małe, prawa kolumna aukcji)</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                   <%--             <tr>
                                    <td>Adres URL po kliknięciu na zdjęcie (opcjonalnie)
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txbLinkUrl" MaxLength="256" Width="300" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>Priorytet
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txbPriority" MaxLength="2" Width="40" TextMode="Number"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td style="text-align: right;">
                                        <asp:CheckBox runat="server" ID="chbDelete" Text="Usuń" ForeColor="Red" />
                                </tr>
                            </table>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button runat="server" ID="btnImages" OnClick="btnImages_Click" Text="Zapisz zmiany"
                                OnClientClick="return confirm('Zapisać zmiany?');" /><span style="position: absolute;">
                                    <asp:UpdateProgress ID="UpdateProgress222" runat="server" AssociatedUpdatePanelID="upImages">
                                        <ProgressTemplate>
                                            <img src="Images/progress.gif" style="height: 20px">
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                            <div style="text-align:right"> 
                                <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbDelete');" Text="Zaznacz wszystkie" ForeColor="Red"/>
                            </div>
                        </FooterTemplate>
                        <HeaderTemplate>
                            <div style="text-align:right"> 
                                <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbDelete');" Text="Zaznacz wszystkie" ForeColor="Red"/>
                              </div>
                        </HeaderTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
