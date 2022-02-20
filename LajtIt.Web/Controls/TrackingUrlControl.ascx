<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackingUrlControl.ascx.cs" Inherits="LajtIt.Web.Controls.TrackingUrlControl" %>


<asp:UpdatePanel runat="server" ID="upPrint">
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <asp:ImageButton runat="server" ID="imgbPrint" ImageUrl="~/Images/print.png" OnClick="imgbPrint_Click" Width="20" CausesValidation="false" />
                </td><td>
                    <asp:Label runat="server" ID="lblTrackingNumber" Text="Brak numeru przesyłki" Width="250"></asp:Label>
                    <asp:HyperLink ID="hlTrackingNumber" Visible="false" Target="_blank" runat="server" Width="250"></asp:HyperLink>
                    <asp:TextBox runat="server" ID="txbTrackingNumber" Width="250"></asp:TextBox></td>
                
                <td>
                    <asp:LinkButton runat="server" Text="Zmień" ID="imgbEdit" OnClick="imgbEdit_Click" />
                    <asp:LinkButton runat="server" Text="Anuluj" ID="imgbCancel" OnClick="imgbCancel_Click" ForeColor="Silver" />
                    <asp:LinkButton runat="server" Text="Zapisz" ID="imgbSave" OnClick="imgbSave_Click" /></td>
            </tr>
        </table>

    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="imgbPrint" />
    </Triggers>
</asp:UpdatePanel>


