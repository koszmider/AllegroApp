<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllegroDeliveryCost.aspx.cs" Inherits="LajtIt.Web.AllegroDeliveryCost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Cennik dostawy</h1>
    <div style="text-align:right"><a href="AllegroDeliveries.aspx">Cenniki dostaw</a></div>
    <table>
        <tr>
            <td style="width:150px">
                Nazwa cennika dostawy
            </td>
            <td style="width:500px">
                <asp:TextBox runat="server" ID="txbName" MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator
                    runat="server" ControlToValidate="txbName" Text="*"></asp:RequiredFieldValidator>
            </td>
        </tr> 
        <tr>
            <td style="width:150px">
                Aktywny
            </td>
            <td style="width:500px">
               <asp:CheckBox runat="server" ID="chbIsActive" Text="TAK" />
            </td>
        </tr> 
        <tr>
            <td style="width:150px">
                Paczkomat
            </td>
            <td style="width:500px">
               <asp:CheckBox runat="server" ID="chbIsPaczkomatAvailable" Text="TAK" />
            </td>
        </tr> 
        <tr>
            <td colspan="2">
                <asp:GridView runat="server" ID="gvDeliveries" AutoGenerateColumns="false" DataKeyNames="FieldId"
                 OnRowDataBound="gvDeliveries_OnRowDataBound"             
                >
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Dostawa" />
                        <asp:BoundField DataField="FieldId" HeaderText="Id" Visible="false" />
                        <asp:TemplateField HeaderText="Koszt podstawowy">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txbBaseCost" Width="80"  style="text-align:right;"></asp:TextBox><asp:RegularExpressionValidator
                                    ID="rfv1" runat="server" ControlToValidate="txbBaseCost"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" Text="*" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ilość">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txbQunatity" Width="80"  style="text-align:right;"></asp:TextBox><asp:RegularExpressionValidator
                                    ID="rfv2" runat="server" ControlToValidate="txbQunatity"
                                    ValidationExpression="^[0-9]{1,4}$" Text="*" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Koszt nast. sztuki">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txbNextItemCost" Width="80"  style="text-align:right;"></asp:TextBox><asp:RegularExpressionValidator
                                    ID="rfv3" runat="server" ControlToValidate="txbNextItemCost"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" Text="*" /></ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Aktywny">
                            <ItemTemplate>
                               <asp:CheckBox runat="server" ID="chbIsActive" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Allegro ">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlAllegroDeliveryMethod" DataTextField="name" DataValueField="id" AppendDataBoundItems="true">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>

                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="txbSave" Text="Zapisz zmiany" OnClick="txbSave_Click" />
            </td>
        </tr>
        <tr><td colspan="2"><asp:Image ImageUrl="~/Images/ok.jpg" Width="50" ID="imgOK"  runat="server"/><asp:Image runat="server" ImageUrl="~/Images/false.jpg" Width="50" ID="imgFalse" /></td></tr>
        <tr>
            <td colspan="2">
                <asp:GridView runat="server" ID="gvAllegroUsers">

                </asp:GridView>
            </td>

        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnPublish" Text="Publikuj zmiany" OnClick="btnPublish_Click" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>
