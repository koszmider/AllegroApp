<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadImageAllegroUser.ascx.cs" Inherits="LajtIt.Web.Controls.UploadImageAllegroUser" %>
<table>
 
    <tr>
        <td>Dodaj zdjęcie
        </td>
        <td>
            <table style="width: 100%;">
                <tr valign="top">

                    <td>
                        <asp:UpdatePanel runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSave" />
                            </Triggers>
                            <ContentTemplate>
                                <input type="file" id="myfile" accept="image/*" multiple="single" name="myfile" runat="server" size="100" />&nbsp;&nbsp;&nbsp;
                                     
                                    <asp:Button ID="btnSave" runat="server" Text="Zapisz pliki" OnClick="btnSave_Click" CausesValidation="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                     
                </tr>
            </table>
        </td>
    </tr>

</table>
