<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductAttributes.ascx.cs"
    Inherits="LajtIt.Web.Controls.ProductAttributes" %>


<asp:UpdatePanel runat="server" ID="upProductAttributes">
    <ContentTemplate>



        <asp:Panel runat="server" ID="pnAttributeTypes">
            <asp:DropDownList runat="server" ID="ddlGrouping" DataValueField="AttributeGroupingId" DataTextField="Name" 
                OnSelectedIndexChanged="ddlGrouping_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
            <asp:CheckBox runat="server" AutoPostBack="true"   Checked="true" Text="Atrybuty wymagane" ID="chbRequried" />
            <asp:CheckBox runat="server" AutoPostBack="true"   Checked="true" Text="Wymagane na Allegro" ID="chbAllegro" />
            <asp:CheckBox runat="server" AutoPostBack="true"   Text="Atrybuty opcjonalne" ID="chbOption" />
            <asp:LinkButton runat="server" Text="zmień widok" ID="lbtnChangeView" OnClick="lbtnChangeView_Click"></asp:LinkButton>
        </asp:Panel>
        

        <asp:Panel runat="server" ID="pnEmpty" >
            <asp:DropDownList runat="server" ID="ddlGroupingEmpty" DataValueField="AttributeGroupingId" DataTextField="Name" 
                OnSelectedIndexChanged="ddlGroupingEmpty_SelectedIndexChanged"                AutoPostBack="true">
            </asp:DropDownList>
            <h2>Wybierz rodzaj produktu</h2>
            <asp:RadioButtonList runat="server" ID="rblAttributeEmptyGroups" RepeatColumns="6"  RepeatDirection="Horizontal"
                DataValueField="AttributeId" DataTextField="Name" ></asp:RadioButtonList>

            <asp:Button runat="server" ID="btnEmpty" Text="Zapisz i rozpocznij konfigurację" OnClick="btnEmpty_Click" />
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
<div style="position: absolute;">
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upProductAttributes">
        <ProgressTemplate>
            <img src="Images/progress.gif" style="height: 20px" alt="" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
<asp:Repeater ID="rpAttributeGroup" runat="server" OnItemDataBound="rpAttributeGroup_OnItemDataBound">
    <HeaderTemplate>
        <table id="table" style="width: 100%">
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr">
            <td runat="server" style="width: 70px; border: solid 1px silver">
                <asp:HyperLink NavigateUrl="/ProductCatalog.Attributes.aspx?idg={0}" Target="_blank" runat="server" ID="lblAttributeGroup"></asp:HyperLink>        <!--(<asp:Label runat="server" ID="lblAttributeGroupId"></asp:Label>)-->
                <br />
                <asp:DropDownList runat="server" ID="ddlAttributeDefault" DataValueField="AttributeId" Width="70"
                    DataTextField="Name">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Image runat="server" ID="imgAllegro" ImageUrl="~/Images/allegro.png" Width="60" ToolTip="Wymagany na Allegro" />


            </td>
            <td runat="server" style="border: solid 1px silver">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                 
                    <ContentTemplate>
                        <asp:HiddenField ID="hidGroupTypeId" runat="server" />
                        <asp:HiddenField ID="hidIsRequired" runat="server" />
                        <asp:HiddenField ID="hidGroupId" runat="server" />
                        <asp:CheckBoxList runat="server" ID="chblAttributes" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="chblAttributes_OnSelectedIndexChanged"
                            Visible="false">
                        </asp:CheckBoxList>
                        <asp:RadioButtonList runat="server" ID="rblAttributes" RepeatColumns="4" AutoPostBack="true" 
                            OnSelectedIndexChanged="chblAttributes_OnSelectedIndexChanged" RepeatDirection="Horizontal"
                            Visible="false">
                        </asp:RadioButtonList>
                        <asp:Repeater runat="server" ID="rpDecimalValues" OnItemDataBound="rpDecimalValues_ItemDataBound">
                            <HeaderTemplate>
                                <table style="width: 100%" class="mytable">
                            </HeaderTemplate>
                            <FooterTemplate></table></FooterTemplate>
                            <ItemTemplate>
                                <tr runat="server" id="tr">
                                    <td style="width: 70px; text-align: right;">
                                        <asp:HiddenField ID="hidAttributeId" runat="server" />
                                        <asp:HiddenField ID="hidAttributeTypeId" runat="server" />
                                        <asp:HiddenField ID="hidIsRequired" runat="server" />
                                        <asp:Label runat="server" ID="lblAttributeName"></asp:Label></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txbAttributeValue" ValidationGroup="new" Width="100%"></asp:TextBox><asp:RegularExpressionValidator
                                            ID="revDecimal" Enabled="false" runat="server" ControlToValidate="txbAttributeValue" ValidationGroup="new"
                                            ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" Text="Zły format np (2 lub 2,00)"></asp:RegularExpressionValidator></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:HyperLink runat="server" Target="_blank" ID="hlEmpty"  
                            Visible="false" Text="Brak wartości? Sprawdź konfigurację atrybutów"
                            NavigateUrl="/ProductCatalog.Attributes.Grouping.aspx"></asp:HyperLink>
                        <div style="text-align: right">
                            <asp:ImageButton runat="server" ID="ibtnDelete" Visible="false" ImageUrl="~/Images/false.jpg" Width="20" ToolTip="Wyczyść zaznaczenie" OnClick="ibtnDelete_Click" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td runat="server" style="width: 200px; text-align: right;" id="tdAddNew">
                <asp:TextBox runat="server" ID="txbAttributeNew" MaxLength="30"></asp:TextBox>
                <asp:LinkButton runat="server" ID="lbtnAttributeNewAdd" Text="Dodaj" OnClick="lbtnAttributeNewAdd_Click"
                    OnClientClick="return confirm('Czy chcesz dodać nowy atrybut?');"></asp:LinkButton>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
