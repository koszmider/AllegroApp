<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Orders.aspx.cs" Inherits="LajtIt.Web.AllegroOrders" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table style="width: 100%; border: 0; padding: 0; margin: 0;">
            <tr valign="top">
                <td style="width: 200px">
                    <asp:ListBox runat="server" ID="lsbOrderStatus" SelectionMode="Multiple" DataTextField="StatusName" DataValueField="OrderStatusId" Rows="9"></asp:ListBox>
                </td>

                <td style="width: 240px">

                    <asp:TextBox runat="server" ID="txbName" MaxLength="50" onclick="this.select();" Width="220" /><asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
                        TargetControlID="txbName"
                        WatermarkText="Email, nazwa, adres"
                        WatermarkCssClass="watermarked" />
                    <br />
                    Nr zamówienia:<br />
                    <asp:TextBox runat="server" ID="txbOrderId" MaxLength="50" onclick="this.select();" Width="220"/>
                    <br />
                    Data zamówienia:<br />
                    <asp:TextBox runat="server" ID="txbDateFrom" Width="70"></asp:TextBox><asp:CalendarExtender
                        ID="calDateFrom" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateFrom">
                    </asp:CalendarExtender>
                    -<asp:TextBox runat="server" ID="txbDateTo" Width="70"></asp:TextBox><asp:CalendarExtender
                        ID="calDateTo" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDateTo">
                    </asp:CalendarExtender><br />Faktura/paragon nr:<br />
                    <asp:DropDownList runat="server" ID="ddlInvoice">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Z fakturą</asp:ListItem>
                        <asp:ListItem>Bez faktury</asp:ListItem>
                        <asp:ListItem>Z paragonem</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox runat="server" MaxLength="20" ID="txbInvoiceNumber" Width="120"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                        TargetControlID="txbInvoiceNumber"
                        WatermarkText="numer faktury/paragonu"
                        WatermarkCssClass="watermarked" />
                    <asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnShowAll" />
                            <asp:PostBackTrigger ControlID="lbtnProductCatalog" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td style="width: 170px">Dostawa:<br />
                    <asp:ListBox runat="server" ID="chblShippingCompany" SelectionMode="Multiple" DataTextField="Name" RepeatColumns="2" DataValueField="ShippingCompanyId" Width="170"></asp:ListBox><br />
            
                    <asp:CheckBox runat="server" ID="chbPaid1" Text="Z wpł." />
                    <asp:CheckBox runat="server" ID="chbPaid0" Text="Bez wpł." /><br />
                    <asp:CheckBox runat="server" ID="chbPayOnDelivery" Text="Pobranie" />


                </td>
                <td> Źródło zamówienia:<br />
                    <asp:ListBox runat="server" ID="lbxShop" SelectionMode="Multiple" Rows="8" DataValueField="ShopId" DataTextField="Name" Width="170"></asp:ListBox>

                   
                </td>

            </tr>
            <tr>
                <td style="text-align: right; vertical-align: bottom;" colspan="4">
                    <asp:Literal runat="server" ID="litProductCatalog" Visible="false"></asp:Literal>
                    <asp:LinkButton runat="server" ID="lbtnProductCatalog" Text="usuń" OnClick="lbtnProductCatalog_Click" Visible="false"></asp:LinkButton></td>
            </tr>
            <tr><td style="text-align:center" colspan="4">
                    <asp:Button runat="server" ID="btnShowAll" Width="950" OnClick="btnShowAll_Click" Text="                    Szukaj                   " />

                </td></tr>
        </table>
    </div>
    <br />
    <div>
        <style>
            .red_row {
                background-color: #ffcc99;
            }
            .hidd{white-space: nowrap; overflow: hidden;text-overflow: ellipsis;}
            .tab{table-layout:auto;}
        </style>
        <asp:GridView runat="server" ID="gvOrders" AutoGenerateColumns="false" CssClass="tab" PageSize="20" Width="100%" OnRowDataBound="gvOrders_OnRowDataBound" ShowFooter="true"
            AllowPaging="true" OnPageIndexChanging="gvOrders_OnPageIndexChanging">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemStyle Width="30" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LitId"></asp:Literal>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nr zam./źródło" ItemStyle-Width="100" > 
                    <ItemStyle   HorizontalAlign="Center" Width="50" CssClass="hidd"/>
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl="Order.aspx?id={0}#order" ID="hlOrderId">
                            
                        <asp:Literal runat="server" ID="litOrderId"></asp:Literal>
                            <br />
                        <asp:Label runat="server" ID="lblNick"></asp:Label>
                        <br />
                        <asp:Image runat="server" ID="imgSource" Width="50" /></asp:HyperLink>
                            <br /><asp:Image runat="server" ID="imgFlag" Width="30" />
                    </ItemTemplate>

                </asp:TemplateField> 
                <asp:BoundField   DataField="Client" HeaderText="Klient" HtmlEncode="false"  Visible="false" />
                <asp:TemplateField HeaderText="Klient">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblClient"></asp:Label>
                       
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:BoundField  ItemStyle-Width="120" DataField="StatusName" HeaderText="Status" ItemStyle-HorizontalAlign="Center" />              
                <asp:TemplateField HeaderText="Przesyłka">  
                    <ItemStyle   HorizontalAlign="Center"  />
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblShippingMode" ></asp:Label> 
                        <asp:Label runat="server" ID="lblShippingCompany"></asp:Label><br />
                        <asp:Label runat="server" ID="lblPayOnDelivery" Visible="false" Text="płatnośc przy odbiorze"></asp:Label><br />
                        <asp:Label runat="server" ID="lblSendFromExternalWerehouse" Visible="false" Text="z mag. zew."></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>               
                <asp:TemplateField HeaderText="Płatność"> 
                    <ItemStyle   HorizontalAlign="right"  Width="120"/>
                    <FooterStyle   HorizontalAlign="right"  Width="120"/>
                    <ItemTemplate>
                        <b><asp:Label runat="server" ID="lblAmountToPay" ToolTip="Wartość zamówienia"></asp:Label></b><br />
                        <asp:Label runat="server" ID="lblAmountPaid" ToolTip="Kwota zapłacona"></asp:Label><br />
                        <asp:Label runat="server" ID="lblAmountBalance" ToolTip="Pozostało do zapłacenia"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:BoundField Visible="false" DataField="AmountToPay" HeaderText="Do zapłacenia" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80" DataFormatString="{0:C}" />
                <asp:BoundField Visible="false" DataField="AmountPaid" HeaderText="Zapłacono" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80" DataFormatString="{0:C}" />
                <asp:BoundField Visible="false" DataField="AmountBalance" HeaderText="Balans" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80" DataFormatString="{0:C}" />
                <asp:BoundField DataField="CreateDate" HeaderText="Data<br>zamówienia" ItemStyle-Width="80" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy-MM-dd<br>HH:mm}" />
                <asp:BoundField DataField="LastStatusChangeDate" HeaderText="Data<br>zmiany statusu" ItemStyle-Width="120" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:yyyy-MM-dd<br>HH:mm}" />
                <asp:BoundField DataField="Comment" HeaderText="Komentarz" ItemStyle-Width="170" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
