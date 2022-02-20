<%@ Page Language="C#" CodeBehind="AllegroGoal.aspx.cs" Inherits="LajtIt.Web.AllegroGoal"
    MasterPageFile="~/Site.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="float:left; width:130px"><asp:Image runat="server" ID="imgUrl" style="float:left;" /></div>
<div >
    <table >
        <colgroup>
            <col width="25%" />
            <col width="25%" />
            <col width="40%" />
            <col width="10%" />
        </colgroup>
        <tr>
            <td>
                <b>Nazwa</b>
            </td>
            <td>
                <asp:Literal runat="server" ID="litName" />
            </td>
            <td>
                <b>Wystawionych/ze sprzedażą/l.przedmiotów </b>
            </td>
            <td>
                <asp:Literal runat="server" ID="litItemsCreated" />
            </td>
        </tr>
        <tr>
            <td>
                <b>Grupa/Produkt</b>
            </td>
            <td>
                <asp:Literal runat="server" ID="litProductCatalog" />
            </td>
            <td>
                <b>Aktywnych/ze sprzedażą/l.przedmiotów </b>
            </td>
            <td>
                <asp:Literal runat="server" ID="litItemsActive" />
            </td>
        </tr>
        <tr>
            <td>
                <b>Aukcja</b>
            </td>
            <td>
                <asp:HyperLink runat="server" ID="hlAllegroItem"></asp:HyperLink>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <b>Aktywny</b>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chbIsActive" Enabled="false" />
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
    </div>
    <asp:CheckBox runat="server" ID="cbhIsActive" Checked="true" AutoPostBack="true"  
        Text="Tylko aktywne" OnCheckedChanged="cbhIsActive_OnCheckedChanged" />
    <asp:GridView runat="server" ID="gvItems" DataKeyNames="ItemId" AutoGenerateColumns="false"
        OnRowDataBound="gvItems_OnRowDataBound" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="Produkt" ItemStyle-Width="460px">
                <ItemTemplate>
                    <asp:HyperLink ID="hlProductAllegroName" Target="_blank" runat="server"></asp:HyperLink></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="LastUpdateDate" DataFormatString="{0:yyyy/MM/dd HH:mm}"
                HeaderText="Ostatnia akt." />
            <asp:BoundField DataField="BuyNowPrice" DataFormatString="{0:C}" HeaderText="Cena" />
            <asp:BoundField DataField="BidCount" HeaderText="Ofert" />
            <asp:BoundField DataField="QuantityOrdered" HeaderText="Przedmiotów" />
        </Columns>
    </asp:GridView>
</asp:Content>
