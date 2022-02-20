<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductCatalogGroup.ascx.cs" Inherits="LajtIt.Web.Controls.ProductCatalogGroupControl" %>


<table style="margin: 0; padding: 0">
    <tr>
        <td width="310">Rodzina</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlProductCatalogFamily" ValidationGroup="client" DataValueField="FamilyId" DataTextField="FamilyName" AutoPostBack="true" Width="300"
                OnSelectedIndexChanged="ddlProductCatalogFamily_SelectedIndexChanged">
            </asp:DropDownList></td>
        <td>
            <asp:TextBox runat="server" ID="txbFamilyName"></asp:TextBox>
            <asp:ImageButton ImageUrl="~/Images/add.png" runat="server" ID="lbtnProductCatalogFamilyGroupSave" OnClick="lbtnProductCatalogFamilyGroupSave_Click" /></td> 
        
    </tr>
    <tr>
        <td>Grupa/linia</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlProductCatalogGroup" ValidationGroup="client" DataValueField="ProductCatalogGroupId" DataTextField="GroupName"  Width="300">
            </asp:DropDownList><asp:RequiredFieldValidator
                ID="RequiredFieldValidator2" ValidationGroup="client" ControlToValidate="ddlProductCatalogGroup"
                Text="*" runat="server"></asp:RequiredFieldValidator></td>
        <td>
            <asp:TextBox runat="server" ID="txbGroupName"></asp:TextBox>  
            <asp:ImageButton ImageUrl="~/Images/add.png" runat="server" ID="ImageButton1" OnClick="lbtnProductCatalogGroupSave_Click" />
        </td>
    </tr>

</table>
