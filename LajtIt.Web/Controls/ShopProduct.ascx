<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopProduct.ascx.cs" Inherits="LajtIt.Web.Controls.ShopProduct" %>

<style>
    table.shops tr td, th {padding: 3px;}
</style>
<asp:GridView runat="server" ID="gvShops" CssClass="shops" AutoGenerateColumns="false" OnRowDataBound="gvShops_RowDataBound">
    <Columns>
        <asp:TemplateField HeaderText="Sklep">
            <ItemStyle Width="300" />
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="hlUrl" Target="_blank"></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>       
        <asp:TemplateField HeaderText="Dostawca<br>aktywny<br>w sklepie">
            <ItemStyle HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="chbSupplierActive" Enabled="false" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Produkt<br>włączony<br>w sklepie">
            <ItemStyle HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="chbIsPSAvailable" Enabled="false"  /><asp:ImageButton runat="server"
                    id="imbAvailable" ImageUrl="~/Images/reload.ico" Width="12" OnClick="imbAvailable_Click" OnClientClick="return confirm('Czy chcesz zmienić widoczność produktu w sklepie?');" />
            </ItemTemplate>
        </asp:TemplateField>
         <asp:CheckBoxField HeaderText="Aktywny<br>w sklepie" DataField="IsPSActive"  ItemStyle-HorizontalAlign="Center"/>
        <asp:TemplateField HeaderText="Cena w sklepie">
            <HeaderTemplate>
                Cena prom/kat<br />
                <asp:Label ID="lblPriceBruttoPromo" Style="font-weight:bold;" runat="server"></asp:Label>
                <asp:Label ID="lblPriceBrutto" runat="server"></asp:Label><br />
                Cena w sklepie

            </HeaderTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <ItemTemplate>
                <asp:Label ID="lblPriceBruttoPromo" Style="font-weight:bold;" runat="server"></asp:Label>
                <asp:Label ID="lblPriceBrutto" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>    
        <asp:TemplateField HeaderText="Różnica względem ceny<br>katalogowej/promocyjnej">
            <ItemStyle HorizontalAlign="Right" />
            <ItemTemplate>
                <asp:Label ID="lblPriceBruttoDiff"    runat="server"></asp:Label> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Produkt w sklepie" ItemStyle-Width="250">
            <ItemStyle HorizontalAlign="Right" />
            <ItemTemplate>

                <asp:GridView runat="server" ID="gvProducts" AutoGenerateColumns="false" ShowHeader="false" OnRowDataBound="gvProducts_RowDataBound" BorderWidth="0">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlShopProduct" Target="_blank"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="150">
                            <ItemTemplate>

                                <asp:ImageButton ImageUrl="~/Images/camera.png" Width="20" ToolTip="Odśwież zdjęcia"
                                    runat="server" ID="btnUpdateImages" Text="Zaktualizuj zdjęcia" OnClick="btnUpdateImages_Click" Visible="false"
                                    OnClientClick="return confirm('Zaktualizować zdjęcia w sklepie?');" />&nbsp;&nbsp;<asp:ImageButton ImageUrl="~/Images/reload.ico" ToolTip="Odśwież wszystko"
                                        runat="server" ID="btnUpdate" Text="Aktualizuj produkt w sklepie" OnClick="btnUpdate_Click" Width="20"
                                        OnClientClick="return confirm('Zaktualizować produkt w sklepie?');" Visible="false" />

                                <asp:ImageButton ImageUrl="~/Images/add.png" Width="20" ToolTip="Utwórz produkt"
                                    runat="server" ID="imbCreate" Text="Utwórz produkt" OnClick="btnCreate_Click" Visible="false"
                                    OnClientClick="return confirm('Utworzyć produkt w sklepie?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ItemTemplate>

        </asp:TemplateField>

    </Columns>
</asp:GridView>
