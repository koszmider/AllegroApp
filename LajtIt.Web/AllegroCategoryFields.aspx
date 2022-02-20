<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllegroCategoryFields.aspx.cs" Inherits="LajtIt.Web.ShopCategoryFields" EnableEventValidation="false" %>

<%@ Register Src="~/Controls/ShopCategoryControlJson.ascx" TagName="ShopCategoryControl" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Edytor pól </h1>

    <asp:UpdatePanel runat="server" ID="up">
        <ContentTemplate>
            <table>
                <tr>
                    <td>Sklep</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlShopType" DataValueField="ShopTypeId" AutoPostBack="true" DataTextField="Name" OnSelectedIndexChanged="ddlShopType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc:ShopCategoryControl runat="server" ID="ucShopCategoryControl" /><asp:Button runat="server" Text="Pokaż" OnClick="btnShowFields_Click" ID="btnShowFields"  />
                    </td>
                </tr>
            

            </table>
    <asp:GridView runat="server" ID="gvShopCategoryFields" AutoGenerateColumns="false" EmptyDataText="Brak pól"
        OnRowEditing="gvShopCategoryFields_OnRowEditing" OnRowCancelingEdit="gvShopCategoryFields_OnRowCancelingEdit"
        OnRowUpdating="gvShopCategoryFields_OnRowUpdating" ShowFooter="true"
        OnRowDeleting="gvShopCategoryFields_OnRowDeleting"
        DataKeyNames="Id" OnRowDataBound="gvShopCategoryFields_OnDataBound">
        <Columns>
            <asp:CommandField ItemStyle-Width="100" ShowCancelButton="true" ShowDeleteButton="true" ShowEditButton="true" />
            
            <asp:BoundField DataField="CategoryFieldId" HeaderText="Id parametru"  ItemStyle-Width="100"/>
            <asp:TemplateField HeaderText="Typ pola" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" ControlStyle-Width="100">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblFieldTypeId"></asp:Label> 
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="ddlFieldTypeId" Width="100">
                        <asp:ListItem Value="1">Int (grupa)</asp:ListItem>
                        <asp:ListItem Value="2">String (wartość stała, pole systemowe, atrybut text)</asp:ListItem>
                        <asp:ListItem Value="3">Float (atrybut float)</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Description" ItemStyle-Width="305" HeaderText="Opis" />
            <asp:TemplateField HeaderText="Wartość domyślna" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:CheckBox runat="server" Enabled="false" ID="chbUseDefaultValue" />
                    <asp:Label runat="server" ID="lblUseDefaultValue"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBox runat="server" ID="chbUseDefaultValue" />
                    <asp:TextBox runat="server" ID="txbUseDefaultValue" Style="width: 50px"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Pole">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblFieldName"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList runat="server" Width="100" ID="ddlFieldNames" DataValueField="SystemFieldId" DataTextField="FieldName" AppendDataBoundItems="true">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Atrybut Grupa">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblAttributeGroup"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList runat="server" Width="100" ID="ddlAttributeGroup" DataValueField="AttributeGroupId" DataTextField="Name" AppendDataBoundItems="true">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Atrybut">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblAttribute"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList runat="server" Width="100" ID="ddlAttribute" DataValueField="AttributeId" DataTextField="Name" AppendDataBoundItems="true">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField DataField="PassToShop" HeaderText="Wyślij do sklepu" ItemStyle-HorizontalAlign="Center" />
            <asp:CheckBoxField DataField="UpdateParameter" HeaderText="Akt.par." ItemStyle-HorizontalAlign="Center" />
            <asp:CheckBoxField DataField="IsRequired" HeaderText="Wymagany" ItemStyle-HorizontalAlign="Center" />
            <asp:CheckBoxField DataField="UseDefaultAttribute" HeaderText="Domyślny<br>atrybut" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>
            
    <asp:Panel runat="server" ID="pnNew" GroupingText="Dodaj nowe pole">
        <table>
            <tr>
                <td>FieldId</td>
                <td>
                    <asp:TextBox runat="server" ID="txbCategoryFieldId" ValidationGroup="new"></asp:TextBox><asp:RequiredFieldValidator runat="server"
                        ControlToValidate="txbCategoryFieldId" Text="pole wymagane" ValidationGroup="new"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server"
                        ControlToValidate="txbCategoryFieldId" Text="wartość liczbowa" ValidationExpression="[\d]{1,}" ValidationGroup="new"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>Typ pola</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFieldTypeId">
                        <asp:ListItem Value="1">Int (grupa)</asp:ListItem>
                        <asp:ListItem Value="2">String (wartość stała, pole systemowe, atrybut text)</asp:ListItem>
                        <asp:ListItem Value="3">Float (atrybut float)</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Opis</td>
                <td>
                    <asp:TextBox runat="server" ID="txbDescription" ValidationGroup="new"></asp:TextBox><asp:RequiredFieldValidator runat="server"
                        ControlToValidate="txbDescription" Text="pole wymagane" ValidationGroup="new"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td>Wartość domyślna</td>
                <td>
                    <asp:CheckBox runat="server" ID="chbUseDefaultValue" /><asp:TextBox runat="server" ID="txbUseDefaultValue"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Akutalizuj jako parametr</td>
                <td>
                    <asp:CheckBox runat="server" ID="chbUpdateParameter" /></td>
            </tr>
            <tr>
                <td>Wymagany</td>
                <td>
                    <asp:CheckBox runat="server" ID="chbIsRequired" /></td>
            </tr>
            <tr>
                <td>Wyślij do sklepu</td>
                <td>
                    <asp:CheckBox runat="server" ID="chbPassToAllegro" /></td>
            </tr>
            <tr>
                <td>Użyj domyślnej wartości atrybutu</td>
                <td>
                    <asp:CheckBox runat="server" ID="chbUseDefaultAttribute" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ValidationGroup="new" ID="btnNew" Text="Dodaj" OnClick="btnNew_Click" /></td>
            </tr>
        </table>


    </asp:Panel>
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlShopType" />
            <asp:PostBackTrigger ControlID="btnShowFields" />
            <asp:PostBackTrigger ControlID="gvShopCategoryFields" />
            <asp:PostBackTrigger ControlID="btnNew" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Panel runat="server" ID="pnFields" GroupingText="Specyfikacja Allegro">
        
        <asp:LinkButton runat="server" OnClick="lbtnFieldsSpec_Click" Text="Pokaż"></asp:LinkButton>
        <asp:LinkButton runat="server" OnClick="lbtnFieldsSpecImport_Click" Text="Importuj parametry" OnClientClick="return confirm('Czy dodać brakujące parametry?')"></asp:LinkButton>
        <asp:GridView ID="gvParametersAllegro" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvParametersAllegro_RowDataBound">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="Id" />
                <asp:BoundField DataField="source" HeaderText="Żródło parametru" />
                <asp:BoundField DataField="name" HeaderText="Nazwa" />
                <asp:TemplateField HeaderText="Typ">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblType"></asp:Label><br />
                        <asp:Label runat="server" ID="lblRestrictions"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="required" HeaderText="Wymagany" />
                <asp:TemplateField HeaderText="Słownik" ItemStyle-Width="500">
                    <ItemTemplate>
                        <div style="max-height: 300px; overflow: scroll;">
                            <asp:GridView ID="gvDictionary" runat="server" AutoGenerateColumns="false" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="id" ItemStyle-Width="20%" />
                                    <asp:BoundField DataField="value" />

                                </Columns>

                            </asp:GridView>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
           <asp:GridView ID="gvParemtersProductAllegro" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvParametersAllegro_RowDataBound">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="Id" />
                <asp:BoundField DataField="source" HeaderText="Żródło parametru" />
                <asp:BoundField DataField="name" HeaderText="Nazwa" />
                <asp:TemplateField HeaderText="Typ">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblType"></asp:Label><br />
                        <asp:Label runat="server" ID="lblRestrictions"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="required" HeaderText="Wymagany" />
                <asp:TemplateField HeaderText="Słownik" ItemStyle-Width="500">
                    <ItemTemplate>
                        <div style="max-height: 300px; overflow: scroll;">
                            <asp:GridView ID="gvDictionary" runat="server" AutoGenerateColumns="false" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="id" ItemStyle-Width="20%" />
                                    <asp:BoundField DataField="value" />

                                </Columns>

                            </asp:GridView>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:GridView ID="gvParametersEmag" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvParametersEmag_RowDataBound">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="Id" />
                <asp:BoundField DataField="name" HeaderText="Nazwa" />
                <asp:TemplateField HeaderText="Typ">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblType"></asp:Label><br />
                        <asp:Label runat="server" ID="lblRestrictions"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Wymagany" ItemStyle-Width="50" >
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chbIsRequired" Enabled="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Słownik" ItemStyle-Width="500">
                    <ItemTemplate>
                        <div style="max-height: 300px; overflow: scroll;">
                            <asp:GridView ID="gvDictionary" runat="server" AutoGenerateColumns="true" Width="100%" ShowHeader="false">
                                <Columns> 
                                   

                                </Columns>

                            </asp:GridView>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:TextBox runat="server" ID="txbFieldsSpec" TextMode="MultiLine" Rows="40" Columns="100" Visible="false"></asp:TextBox>
        <br />

    </asp:Panel>
</asp:Content>
