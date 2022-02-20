<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllegroSellMatrix.aspx.cs" Inherits="LajtIt.Web.AllegroSellMatrix" %>

<%@ Register Src="~/Controls/AllegroSellMatrixControl.ascx" TagName="AllegroSellMatrixControl"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Timer runat="server" OnTick="Anime_Tick" ID="tmAnime" Interval="500" Enabled="false">
    </asp:Timer>
    <span style="position: absolute;">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <img src="Images/progress.gif" style="height: 20px" alt="" /></ProgressTemplate>
        </asp:UpdateProgress>
    </span>
    <table>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" Text="Włącz autoodświeżanie" OnClick="btnAnime_Click" />
            </td>
        </tr>
        <tr valign="top">
            <td>
                <uc:AllegroSellMatrixControl runat="server" ID="ucAllegroSellMatrixControl1" />
            </td>
            <td>
                <uc:AllegroSellMatrixControl runat="server" ID="ucAllegroSellMatrixControl2" />
            </td>
        </tr>
    </table>
</asp:Content>
