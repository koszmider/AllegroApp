<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="ProductCatalog.Attributes.Grouping.aspx.cs" Inherits="LajtIt.Web.ProductCatalogAttributesGrouping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <h1>Grupowanie atrybutów</h1>
            <p><b>Mixer opisów</b> grupuje rodzaje produktów o podbnej charakterystyce na potrzeby systemu tworzącego opisy produktów</p>
            <p><b>Zarządzanie atrybutami</b> grupuje poszczególne atrybuty jak i całe grupy w wybranych kategoriach. Stosowane do zawężania wyświetlanych wartości do konkretnych zastosowań (np. Gwint żarówki do lamp a Rama do luster)</p>
            <div style="text-align: right;">

                <asp:LinkButton runat="server" ID="lbtnAttributesEdit" Text="Dodaj nowe grupowanie"></asp:LinkButton>
                <asp:ModalPopupExtender ID="mpeAttributesEdit" runat="server"
                    TargetControlID="lbtnAttributesEdit"
                    PopupControlID="pnAttributes2"
                    BackgroundCssClass="modalBackground"
                    DropShadow="true"
                    CancelControlID="imbCancel1"
                    PopupDragHandleControlID="Panel2" />

                <asp:Panel runat="server" ID="pnAttributes2" BackColor="White" Style="width: 800px; background-color: white; height: 400px; padding: 10px">
                    <asp:Panel runat="server" ID="Panel2">
                        <div style="text-align: right">
                            <asp:ImageButton runat="server" ImageUrl="~/Images/cancel.png" Width="20" ID="imbCancel1" />
                        </div>
                        <div style="text-align: left">
                            Nowe grupowanie
                        </div>

                    </asp:Panel>
                    <div style="width: 800px; height: 300px; text-align: left">
                        <table>
                            <tr>
                                <td>Rodzaj grupowania

                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlGroupingType" DataValueField="GroupingTypeId" DataTextField="Name"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>Nazwa</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txbName"></asp:TextBox><asp:RequiredFieldValidator ControlToValidate="txbName"
                                        runat="server" Text="*" ErrorMessage="wymagane" ValidationGroup="add"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button runat="server" ID="lbtnGroupingAdd" Text="Dodaj" OnClientClick="return confirm('Dodać?');" OnClick="lbtnGroupingAdd_Click" ValidationGroup="add" />
                                </td>
                            </tr>
                        </table>



                    </div>
                </asp:Panel>

            </div>
            <asp:RadioButtonList runat="server" ID="rblGroupingTypes" DataValueField="GroupingTypeId" DataTextField="Name" OnSelectedIndexChanged="rblGroupingTypes_SelectedIndexChanged"
                AutoPostBack="true" RepeatDirection="Horizontal">
            </asp:RadioButtonList>
            <asp:GridView runat="server" ID="gvGroupings" AutoGenerateColumns="false" DataKeyNames="AttributeGroupingId" Width="500">
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="AttributeGroupingId" DataNavigateUrlFormatString="ProductCatalog.Attributes.Grouping.Page.aspx?id={0}"
                        DataTextField="Name" HeaderText="Nazwa" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
