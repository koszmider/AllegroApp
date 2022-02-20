<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderShippingSimple.ascx.cs" Inherits="LajtIt.Web.Controls.OrderShippingSimple" %>

<asp:Panel runat="server" ID="pnCourier" Visible="false">
    <asp:UpdatePanel runat="server" ID="upCourier">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <table>
                <asp:Repeater runat="server" ID="rpParcels" OnItemDataBound="rpParcels_ItemDataBound">
                    <HeaderTemplate>

                        <tr>
                            <td>Waga</td>
                            <td>Wysokość</td>
                            <td>Szerokość</td>
                            <td>Długość</td>
                        </tr>
                    </HeaderTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                    <ItemTemplate>
                        <tr valign="top">
                            <td>
                                <asp:TextBox runat="server" ID="txbWeight"   Width="100" MaxLength="50" ValidationGroup="os"></asp:TextBox><br />
                                <asp:RequiredFieldValidator
                                    ValidationGroup="os" runat="server" ControlToValidate="txbWeight" ID="rfWeight"
                                    Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RangeValidator Visible="false" runat="server" ID="rgWeight" MaximumValue="31" MinimumValue="1" ControlToValidate="txbWeight" ValidationGroup="os"  
                                       
                                    Text="min 1kg - max 31kg"></asp:RangeValidator>

                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txbHeight"  Width="100"  ValidationGroup="os" MaxLength="50"></asp:TextBox><br />
                                <asp:RequiredFieldValidator ID="rfvHeight" Visible="false"
                                    ValidationGroup="os" runat="server" ControlToValidate="txbHeight"
                                    Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator>
                          

                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txbWidth"  Width="100"  ValidationGroup="os" MaxLength="50"></asp:TextBox><br />
                                <asp:RequiredFieldValidator ID="rfvWidth" Visible="false"
                                    ValidationGroup="os" runat="server" ControlToValidate="txbWidth"
                                    Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator></td>
                            <td>
                                <asp:TextBox runat="server" ID="txbLength" Width="100"  ValidationGroup="os" MaxLength="50"></asp:TextBox><br />
                                <asp:RequiredFieldValidator ID="rfvLength" Visible="false"
                                    ValidationGroup="os" runat="server" ControlToValidate="txbLength"
                                    Text="wymagane" Display="Dynamic"></asp:RequiredFieldValidator></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td colspan="4">
                        <asp:LinkButton runat="server" ID="lbtnParcelAdd" CausesValidation="false" Text="dodaj kolejną paczkę" OnClick="lbtnParcelAdd_Click"></asp:LinkButton><br /><br />
                        
                        <span style="position: absolute;">
                            <asp:UpdateProgress ID="upgCoutier" runat="server" AssociatedUpdatePanelID="upCourier">
                                <ProgressTemplate>
                                    <img src="Images/progress.gif" style="height: 20px" alt="" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </span>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:Panel runat="server" ID="pnInpost" Visible="false">
    <asp:UpdatePanel runat="server" ID="upInpost">
        <ContentTemplate>
            <table>
                <tr>
                    <td>Gabaryt</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlSize" ValidationGroup="os" Width="200">
                            <asp:ListItem Value="0">-- nie wybrano --</asp:ListItem>
                            <asp:ListItem Value="1">A</asp:ListItem>
                            <asp:ListItem Value="2">B</asp:ListItem>
                            <asp:ListItem Value="3">C</asp:ListItem>
                        </asp:DropDownList> 
                    </td> 
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CompareValidator ValueToCompare="0" Operator="NotEqual" runat="server"
                            ControlToValidate="ddlSize" Text="musisz wskazać gabaryt paczki" ValidationGroup="os"></asp:CompareValidator>
                    </td>
                    <td></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel runat="server" ID="pnExternal" Visible="false">
    <asp:UpdatePanel runat="server" ID="upExternal">
        <ContentTemplate>
            <table>
                <tr>
                    <td>Numer przesyłki</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbTrackingNumber" ValidationGroup="os"></asp:TextBox>
                    </td> 
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:RequiredFieldValidator  runat="server"
                            ControlToValidate="txbTrackingNumber" Text="podaj numer przesyłki" ValidationGroup="os"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
