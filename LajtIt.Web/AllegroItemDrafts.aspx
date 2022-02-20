<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllegroItemDrafts.aspx.cs" Inherits="LajtIt.Web.AllegroItemDrafts" %>

<%@ Register Src="~/Controls/ProductAttributes.ascx" TagName="ProductAttributes" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Zarządzanie ofertami Allegro</h1>
    <table>
        <tr>
            <td>Nazwa/kod/ean produktu</td>
            <td>
                <asp:CheckBox runat="server" ID="chbAttributesSearch" Text="Atrybuty"
                    AutoPostBack="true" /></td>
            <td>Status walidacji</td>
            <td>Dane</td>
            <td>Zdjęcia</td>
            <td>Konto</td>
            <td></td>
        </tr>
        <tr valign="top">
            <td>

                <asp:TextBox runat="server" ID="txbSearchName" MaxLength="50" Width="200"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                    TargetControlID="txbSearchName"
                    WatermarkText="Nazwa/Tytuł/Kod produktu/Ean"
                    WatermarkCssClass="watermarked" />
            </td>
            <td rowspan="2">
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" Width="120" ID="ddlAttributes" AppendDataBoundItems="true" DataTextField="Name" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlAttributes_SelectedIndexChanged"
                                DataValueField="AttributeGroupId">
                            </asp:DropDownList>
                            <asp:DropDownList runat="server" ID="ddlAttributesExists">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>jest</asp:ListItem>
                                <asp:ListItem>brak</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" Width="120" ID="ddlAttributesValue" DataTextField="Name" DataValueField="AttributeId">
                            </asp:DropDownList>
                            <asp:DropDownList runat="server" ID="ddlAttributesValueExists">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>jest</asp:ListItem>
                                <asp:ListItem>brak</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlAllegroOfferStatus" Width="200">
                    <asp:ListItem Value="ALL"></asp:ListItem>
                    <asp:ListItem Value="NOTCREATED" Selected="True">NOTCREATED - Nieutworzony</asp:ListItem>
                    <asp:ListItem Value="INACTIVE">INACTIVE - Nieaktywny</asp:ListItem>
                    <asp:ListItem Value="ACTIVATING">ACTIVATING - Aktywnowany</asp:ListItem>
                    <asp:ListItem Value="ENDED">ENDED - Zakończony</asp:ListItem>
                    <asp:ListItem Value="ACTIVE">ACTIVE - Aktywny</asp:ListItem>
                </asp:DropDownList></td>
            <td>
                <asp:DropDownList runat="server" ID="ddlIsValid" Width="130">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="1">OK</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">Błąd</asp:ListItem>
                </asp:DropDownList></td>
            <td>
                <asp:DropDownList runat="server" ID="ddlIsImageReady" Width="130">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="1">OK</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">Błąd</asp:ListItem>
                </asp:DropDownList></td>
            <td rowspan="2">
                <asp:ListBox runat="server" ID="lsbxUserId" DataValueField="UserId" DataTextField="UserName" Width="150" Rows="3" SelectionMode="Multiple"></asp:ListBox></td>
            <td>
                <asp:Button runat="server" Text="Szukaj" ID="btnSearch" OnClick="btnSearch_Click" /></td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" ID="chbCategory" Text="Kategorie niezgodne" /></td>
            <td>
                <asp:TextBox runat="server" ID="txbComment" MaxLength="50" Width="130"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
                    TargetControlID="txbComment"
                    WatermarkText="Uwagi dot. walidacji"
                    WatermarkCssClass="watermarked" />
            </td>
            <td></td>
        </tr>
    </table>   <div style="text-align: right">
                <asp:UpdatePanel runat="server" ID="upPage">
                    <ContentTemplate>
                        Produktów:
                        <asp:Label runat="server" ID="lblCount" Text="0"></asp:Label>
                        Na stronie:
                        <asp:TextBox runat="server" ID="txbPageSize" Text="15" AutoPostBack="true" OnTextChanged="btnSearch_Click" Width="45" TextMode="Number"></asp:TextBox>
                        Przejdź do strony:
                        <asp:TextBox runat="server" ID="txbPageNo" AutoPostBack="true" OnTextChanged="txbPageNo_TextChanged" Text="15" Width="45" TextMode="Number"></asp:TextBox><asp:Label runat="server" ID="lblPageNo"></asp:Label>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <div style="position: absolute;">
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upPage">
                        <ProgressTemplate>
                            <img src="Images/progress.gif" style="height: 20px" alt="" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>


            </div>

    <asp:GridView runat="server" ID="gvAllegroItems" PageSize="20" AllowPaging="true" DataKeyNames="ProductCatalogId,ItemId" Width="100%"
        OnRowDataBound="gvAllegroItems_RowDataBound" ShowFooter="true" EmptyDataText="Brak wyników" PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="TopAndBottom"
        OnPageIndexChanging="gvAllegroItems_PageIndexChanging" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField>
                <ItemStyle Width="50" HorizontalAlign="Right" />
                <ItemTemplate>
                    <itemtemplate><asp:Literal runat="server" ID="liId"></asp:Literal></itemtemplate>
                    <asp:Literal runat="server" ID="LitId"></asp:Literal>
                    <asp:CheckBox runat="server" ID="chbOrder" />
                </ItemTemplate>
                <HeaderTemplate>
                    <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Allegro">
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlProduct" Target="_blank" NavigateUrl="/ProductCatalog.Preview.aspx?id={0}">
                    </asp:HyperLink><br />
                    <asp:HyperLink runat="server" ID="hlProductImage" Target="_blank" NavigateUrl="/ProductCatalog.Preview.aspx?id={0}">
                        <asp:Image runat="server" ID="imgImage" Width="150" />
                    </asp:HyperLink>
                    <br>
                    <asp:HyperLink runat="server" ID="hlItem" NavigateUrl="http://allegro.pl/show_item.php?item={0}" Target="_blank"></asp:HyperLink>



                </ItemTemplate>
                <FooterTemplate>
                    <asp:Literal runat="server" ID="litTotal"></asp:Literal>
                </FooterTemplate>
            </asp:TemplateField>
             
            <asp:BoundField DataField="InsertDate" HeaderText="Data dodania" DataFormatString="{0:yy/MM/dd HH:mm}" />
            <asp:TemplateField HeaderText="Walid./zdj">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Image ImageUrl="~/Images/ok.jpg" Width="22" ID="imgVOK" runat="server" />
                    <asp:Image runat="server" ImageUrl="~/Images/false.jpg" Width="22" ID="imgVFalse" />
                    <asp:Image ImageUrl="~/Images/ok.jpg" Width="22" ID="imgOK" runat="server" />
                    <asp:Image runat="server" ImageUrl="~/Images/false.jpg" Width="22" ID="imgFalse" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ValidatedAt" HeaderText="Data walid." DataFormatString="{0:yy/MM/dd HH:mm}" />
            <asp:TemplateField HeaderText="Uwagi">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblComment"></asp:Label>
                    <asp:GridView runat="server" ID="gvErrors" AutoGenerateColumns="false" Width="100%">
                        <Columns>
                            <asp:BoundField HeaderText="Kod" DataField="code" />
                            <asp:BoundField HeaderText="Błąd" DataField="message" Visible="false" />
                            <asp:BoundField HeaderText="Info" DataField="userMessage" />
                        </Columns>
                    </asp:GridView>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Specyfikacja">
                <ItemStyle HorizontalAlign="Right" Width="312" />
                <ItemTemplate>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div style="overflow: auto; height: 200px; font-size: 7pt;">

                                <uc:ProductAttributes ID="ucProductAttributesProduct" runat="server"></uc:ProductAttributes>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ItemTemplate>

                <FooterTemplate>
                    <asp:UpdatePanel runat="server" ID="upAction1">
                        <ContentTemplate>
                            <asp:Button runat="server" Text="Zapisz" Width="100%" ID="btnAction1" OnClick="btnAction_Click1" />
                            <span style="position: absolute;">
                                <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="upAction1">
                                    <ProgressTemplate>
                                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </span>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>



    </asp:GridView>


    <asp:Panel runat="server" ID="pnDelete" Visible="false">
        <asp:Button runat="server" ID="btnDelete" Text="Zamknij wybrane oferty" OnClick="btnDelete_Click" OnClientClick="return cofirm('Czy chcesz zamknąć wybrane oferty?')" />
        <br />
        <asp:CheckBox runat="server" ID="chbDoNotReActive" Text="Blokuj wznawianie ofert" />
    </asp:Panel>
</asp:Content>
