<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllegroTest.aspx.cs" Inherits="LajtIt.Web.AllegroTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="up" RenderMode="Block">
        <ContentTemplate>
           <h1>&nbsp;<span style="position: absolute;"><asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="up">
                <ProgressTemplate>
                    
                        <img src="Images/progress.gif" style="width:20px" alt="" />Wczytywanie
                </ProgressTemplate>
            </asp:UpdateProgress></span></h1>
            <asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="Login" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
