<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllegroItems.aspx.cs" Inherits="LajtIt.Web.AllegroItems" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>
                Szukana fraza
            </td>
            <td>
                Numer aukcji
            </td>
            <td>
            </td>
            <td>
                Koniec od
            </td>
            <td>
                Koniec do
            </td>
            <td>Kategoria</td>
        </tr>
        <tr valign="top">
            <td>
                <asp:RadioButton runat="server" ID="rbtnSearchTypeQuery" GroupName="search" Checked="true"
                    OnCheckedChanged="rbtnSearchTypeQuery_Click" AutoPostBack="true" /><asp:TextBox runat="server"
                        Width="90" ID="txbSearchText" MaxLength="50" ValidationGroup="search"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbAllegroItem" MaxLength="20" ValidationGroup="search"
                    Width="90"></asp:TextBox><asp:RegularExpressionValidator runat="server" ValidationExpression="[\d]{0,20}"
                        ValidationGroup="search" ControlToValidate="txbAllegroItem" Text="*"></asp:RegularExpressionValidator>
            </td>
            <td>
            </td>
            <td>
                <asp:RadioButton runat="server" ID="rbtnSearchTypeDates" GroupName="search" OnCheckedChanged="rbtnSearchTypeDates_Click"
                    AutoPostBack="true" /><asp:TextBox runat="server" ID="txbDateFrom" Enabled="false"
                        Width="90"></asp:TextBox><asp:CalendarExtender ID="calFrom" runat="server" Format="yyyy/MM/dd"
                            TargetControlID="txbDateFrom">
                        </asp:CalendarExtender>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbDateTo" Enabled="false" Width="90"></asp:TextBox><asp:CalendarExtender
                    ID="calTo" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateTo">
                </asp:CalendarExtender>
            </td>
            <td colspan="2"> 
                    <asp:DropDownList runat="server" ID="ddlCategory" ValidationGroup="stat">
                        <asp:ListItem Value="0">Wszystkie</asp:ListItem>
                        <asp:ListItem Value="1" Selected="True">Oświetlenie</asp:ListItem>
                        <asp:ListItem Value="2" >Dom - Oświetlenie</asp:ListItem>
                        <asp:ListItem Value="3" >Ogród - Oświetlenie</asp:ListItem>
                        <asp:ListItem Value="4" >Pokój dziecięcy - Oświetlenie</asp:ListItem>
                    </asp:DropDownList></td>
        </tr>
        <tr>
            <td>
                Licytacja
            </td>
            <td>
                Sprzedający
            </td>
            <td>
                Trwające
            </td>
            <td>
                Ze sprzedażą
            </td>
            <td>
                Promowana
            </td>
            <td>
                Dostawca
            </td>
            <td>
                Kod
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="ddlIsAuction">
                    <asp:ListItem Value="-1">--</asp:ListItem>
                    <asp:ListItem Value="1">Tak</asp:ListItem>
                    <asp:ListItem Value="0">Nie</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbUserName"></asp:TextBox>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlIsFinished">
                    <asp:ListItem Value="-1">--</asp:ListItem>
                    <asp:ListItem Value="1">Tak</asp:ListItem>
                    <asp:ListItem Value="0">Nie</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlSoldItems">
                    <asp:ListItem Value="-1">--</asp:ListItem>
                    <asp:ListItem Value="1">Tak</asp:ListItem>
                    <asp:ListItem Value="0">Nie</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlIsPromoted">
                    <asp:ListItem Value="-1">--</asp:ListItem>
                    <asp:ListItem Value="1">Tak</asp:ListItem>
                    <asp:ListItem Value="0">Nie</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlSuppliers" AppendDataBoundItems="true" DataTextField="Name"
                    DataValueField="SupplierId">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txbRegExp" Text="[\d]{3,4}\w?\-?\w*"></asp:TextBox><asp:CheckBox
                    runat="server" ID="chbRegExp" />
            </td>
            <td style="width: 100px">
                <asp:Button ID="Button1" runat="server" ValidationGroup="search" Text="Szukaj" OnClick="btnSearch_Click" />
                <span style="position: absolute;">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                            <img src="Images/progress.gif" style="height: 20px" alt="" /></ProgressTemplate>
                    </asp:UpdateProgress>
                </span>
            </td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="gvAllegroItems" AutoGenerateColumns="false" Width="100%"
        OnRowDataBound="gvAllegroItems_OnRowDataBound" EmptyDataText="Brak danych">
        <Columns>
            <asp:ImageField DataImageUrlField="ImageUrl" ControlStyle-Width="60">
            </asp:ImageField>
            <asp:HyperLinkField DataNavigateUrlFields="ItemId" DataNavigateUrlFormatString="http://allegro.pl/show_item.php?item={0}"
                DataTextField="ItemId" HeaderText="Numer aukcji" Target="_blank" />
            <asp:HyperLinkField DataNavigateUrlFields="UserId" DataNavigateUrlFormatString="http://allegro.pl/listing/user/listing.php?us_id={0}"
                DataTextField="UserName" HeaderText="Sprzedający" Target="_blank" />
            <asp:BoundField DataField="Name" HeaderText="Nazwa aukcji" />
            <asp:TemplateField HeaderText="Cena">
                <ItemStyle HorizontalAlign="Right" />
                <ItemTemplate>
                    <asp:Image ImageUrl="/Images/licytacja.jpg" ID="imgIsAuction" ToolTip="Licytacja"
                        Visible="false" Width="20" runat="server" />&nbsp;<asp:Label runat="server" ID="lblPrice"
                            runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="BidCount" HeaderText="L.ofert" ItemStyle-HorizontalAlign="center" />
            <asp:BoundField DataField="HitCount" HeaderText="L.odsłon" ItemStyle-HorizontalAlign="center" />
            <asp:BoundField DataField="EndingDateTime" DataFormatString="{0:yyyy/MM/dd HH:mm}"
                HeaderText="Koniec" />
        </Columns>
    </asp:GridView>
    <asp:GridView runat="server" ID="gvSummary">
    </asp:GridView>
</asp:Content>
