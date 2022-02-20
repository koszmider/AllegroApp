<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InpostExport.ascx.cs" Inherits="LajtIt.Web.Controls.InpostExport" %>
<asp:UpdatePanel runat="server" ID="upExport">
    <ContentTemplate>
        <asp:Panel ID="pnInpost" runat="server" Style="width: 100px">
            <asp:DropDownList runat="server" ID="ddlInpost" DataValueField="Name" DataTextField="Description" Width="100"></asp:DropDownList>
            <asp:CompareValidator runat="server" ValueToCompare="0" ControlToValidate="ddlInpost" Text="wybierz" Operator="NotEqual"></asp:CompareValidator><br />
            <asp:Button runat="server" ID="btnExport" Text="eksportuj" OnClick="lbtnExport_Click" Enabled="true" OnClientClick="return confirm('Czy utworzyć etykiety?');" />
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
<span style="position: absolute;">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upExport">
        <ProgressTemplate>
            <img src="Images/progress.gif" style="height: 20px" alt="" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</span>
