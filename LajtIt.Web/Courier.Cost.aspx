<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Courier.Cost.aspx.cs" Inherits="LajtIt.Web.CourierCosts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Raport kosztów DPD/Inpost</h1>
    <table> 
        <tr>
            <td>Miesiąc<br />
                <asp:DropDownList runat="server" ID="ddlMonth">
                </asp:DropDownList>
            </td>
            <td>Koszt<br />od <asp:TextBox runat="server" ID="txbFrom" TextMode="Number"></asp:TextBox> do <asp:TextBox runat="server" ID="txbTo" TextMode="Number"></asp:TextBox></td>
            <td>Kurier<br />
                <asp:DropDownList runat="server" ID="ddlCourier">
                    <asp:ListItem Value="1">DPD</asp:ListItem>
                    <asp:ListItem Value="4">Inpost</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td><br /><asp:DropDownList runat="server" ID="ddlOrder">
                <asp:ListItem>wszystkie</asp:ListItem>
                <asp:ListItem>połączone z zamówieniem</asp:ListItem>
                <asp:ListItem>bez zamówienia</asp:ListItem>
                </asp:DropDownList></td>
            <td><br />
                <asp:Button ID="btnSearch" OnClick="btnSearch_Click" Text="Szukaj" runat="server" />
            </td>
            <td>
                <table>


                    <tr>
                        <td>
                            <asp:Panel ID="pnReport" GroupingText="Raport   " runat="server">
                                <asp:DropDownList runat="server" ID="ddlFileSource">
                                    <asp:ListItem Value="1">Specyfikacja DPD</asp:ListItem>
                                    <asp:ListItem Value="2">Specyfikacja Inpost</asp:ListItem>
                                    <asp:ListItem Value="3">Specyfikacja Allegro</asp:ListItem>
                                </asp:DropDownList>
                                <asp:UpdatePanel runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnRaportUpload" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <input type="file" id="myfile" accept="document/*" name="myfile" runat="server" size="100" />&nbsp;&nbsp;&nbsp;
                                     
                                        <asp:Button ID="btnRaportUpload" runat="server" Text="Wgraj plik" OnClick="btnRaportUpload_Click" CausesValidation="false" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:LinkButton runat="server" ID="lbtnAssign" Text="Połącz numery z zamówieniami" OnClick="lbtnAssign_Click"></asp:LinkButton></td>
                    </tr>
                </table></td>
        </tr>
    </table>
    <br />
    <br />
    <style>
        table.pad {
        }

            table.pad tr td {
                padding: 3px;
            }

                table.pad tr td.comment {
                    overflow-x: auto;
                    word-break: break-all;
                    word-wrap: break-word;
                    overflow-y: hidden;
                    width: 200px;
                }
    </style>
    <table style="width: 100%">
        <tr valign="top">
            <td style="width: 1100px">
                <asp:GridView runat="server" CssClass="pad" ID="gvParcels" DataKeyNames="Id" AutoGenerateColumns="false" ShowFooter="true" EmptyDataText="brak danych"
                    OnRowDataBound="gvDpd_RowDataBound">
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemStyle Width="100" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LitId"></asp:Literal>
                                <asp:CheckBox runat="server" ID="chbPOrder" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chbPOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbPOrder');" />
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField DataNavigateUrlFields="OrderId" DataTextField="OrderId" DataNavigateUrlFormatString="/Order.aspx?id={0}" HeaderText="Zamówienie"  Target="_blank"/>                        
                        
                        <asp:BoundField ItemStyle-Width="100" DataField="ParcelNumber"     HeaderText="Nr przesyłki" />
                        <asp:BoundField ItemStyle-Width="100" DataField="ParcelNumber2"  HeaderText="List pierwotny" />
                        <asp:BoundField ItemStyle-Width="100" DataField="SendDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data nadania" />
                        <asp:BoundField ItemStyle-Width="100" DataField="DeliveryDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="Data wykonania" />
                        <asp:BoundField ItemStyle-Width="120" ItemStyle-HorizontalAlign="right" DataField="Cost" DataFormatString="{0:C}" HeaderText="Cena netto" />
                        <asp:BoundField ItemStyle-Width="120" ItemStyle-HorizontalAlign="right" DataField="TotalCost" DataFormatString="{0:C}" HeaderText="Kwota" />
                        <asp:BoundField ItemStyle-Width="150" ItemStyle-HorizontalAlign="right" DataField="Weight" HeaderText="Waga" />
                        <asp:BoundField ItemStyle-Width="150" DataField="ParcelCount" HeaderText="Ilość pacz." />
                        <asp:BoundField ItemStyle-Width="150" DataField="InvoiceNumber" HeaderText="Nr fak." />
                        <asp:BoundField ItemStyle-Width="150" DataField="ServiceName" HeaderText="Usługa" />
                        <asp:BoundField ItemStyle-Width="150" DataField="Comments" HeaderText="Uwagi" />
                    </Columns>
                </asp:GridView>
               
            </td>


            <td>


            </td>
        </tr>
    </table>



</asp:Content>

