<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShopExportFile.aspx.cs" Inherits="LajtIt.Web.ShopExportFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Kreator ekspotowania danych sklepu</h1>
    <table>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="ddlShop" DataValueField="ShopId" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlShop_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
<tr>
    <td>
        <style>
            .tabShop tr td {padding:3px;}
        </style>
        <table class="tabShop">
            <tr>
                <td>Format pliku</td>
                <td><asp:DropDownList runat="server" ID="ddlExportFileFormatType" DataValueField="ExportFileFormatTypeId" DataTextField="Name" Width="300">

                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Typ ceny</td>
                <td><asp:DropDownList runat="server" ID="ddlPriceTypeId" Width="300">
                    <asp:ListItem Value="1">Cena sklepowa</asp:ListItem>
                    <asp:ListItem Value="2">Cena allegrowa (z dodatkową prowizją)</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Nazwa pliku</td>
                <td><asp:TextBox runat="server" id="txbExportFileName" MaxLength="50" ValidationGroup="save" Width="300"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="save" ErrorMessage="wymagane" ControlToValidate="txbExportFileName" runat="server"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Parametry URL</td>
                <td><asp:TextBox runat="server" id="txbUrlParameters"  ValidationGroup="save" Width="300"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Zakres cen</td>
                <td>od<asp:TextBox runat="server" id="txbPriceFrom"  ValidationGroup="save" Width="130"></asp:TextBox> do<asp:TextBox runat="server" id="txbPriceTo"  ValidationGroup="save" Width="130"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Tylko z EAN</td>
                <td><asp:CheckBox runat="server" id="chbExportFileEanRequired" Text=""  ValidationGroup="save"></asp:CheckBox></td>
            </tr>
            <tr>
                <td>Przekazuj ceny</td>
                <td><asp:RadioButtonList runat="server" id="rblPrice"  RepeatLayout="Flow" RepeatColumns="2" >
                    <asp:ListItem Value="1" Selected="True">Brutto</asp:ListItem>
                    <asp:ListItem Value="2" >Netto</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <table>
                        <tr>
                            
                            <td><asp:CheckBox runat="server" ID="chbFilterByLampType" Text="Filtruj typ lampy" /></td>
                        </tr>
                        <tr valign="top">
     
                            <td>
                                <asp:GridView runat="server" ID="gvLampTypes" AutoGenerateColumns="false" OnRowDataBound="gvLampTypes_RowDataBound" DataKeyNames="AttributeId">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="" ItemStyle-Width="200" />
                                        <asp:TemplateField HeaderText="Exportuj">
                                            <HeaderTemplate>
                                                 <br />
                                                <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'gvLampTypes');" />
                                            </HeaderTemplate>
                                            <ItemStyle Width="100" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chbExportEnabled" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView></td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnSave" Text="Zapisz" OnClick="btnSave_Click" ValidationGroup="save" />
                    <asp:Label runat="server" ID="lbInfo"></asp:Label>

                </td>
            </tr>

        </table>


    </td>
</tr>
    </table>

</asp:Content>
