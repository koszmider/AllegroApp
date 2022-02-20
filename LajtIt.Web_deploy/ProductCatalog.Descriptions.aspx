<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true" CodeBehind="ProductCatalog.Descriptions.aspx.cs"
    Inherits="LajtIt.Web.ProductDescriptions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/ProductCatalogMenu.ascx" TagName="ProductMenu" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upShop" runat="server">
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <uc:ProductMenu runat="server" SetTab="td14"></uc:ProductMenu>
    <asp:Panel runat="server">
        <asp:UpdatePanel runat="server" ID="upAllegro">
            <ContentTemplate>
                <table>
                    <tr><td><asp:DropDownList runat="server" ID="ddlShops" RepeatDirection="Horizontal" DataValueField="ShopId" DataTextField="Name"
                        AutoPostBack="true" OnSelectedIndexChanged="rblShops_SelectedIndexChanged"
                        ></asp:DropDownList></td></tr>
                    <tr>
                        <td>
                <h1>Długi opis</h1>
                            Dozwolone tagi allegro:  &lt;h1>, &lt;h2>, &lt;p>, &lt;ul>, &lt;ol>, &lt;li> oraz &lt;b> oraz ich domknięcia.
          <asp:RegularExpressionValidator runat="server" ID="revTags"
              ValidationExpression="^([^<]|<h1>|<h2>|<p>|<ul>|<ol>|<li>|<b>|</h1>|</h2>|</p>|</ul>|</ol>|</li>|</b>|a z|A Z|1 9|(.\.))*$" Text="Niewłaściwe tagi html"
                                  
Enabled="false"
                                ControlToValidate="txtLongDescription" ErrorMessage="Niewłaściwe tagi html"></asp:RegularExpressionValidator>
                <asp:TextBox
                    ID="txtLongDescription"
                    TextMode="MultiLine"
                    Columns="180" 
                    Rows="30"
                    runat="server" />

                <asp:HtmlEditorExtender EnableSanitization="false"
                    Id="htmlLong"
                    DisplayPreviewTab="true"
                    DisplaySourceTab="true"
                    
                    TargetControlID="txtLongDescription"
                    runat="server" />

                <br />
                            <asp:LinkButton runat="server" ID="lbnGenerateLong" Text="Generuj losowy opis" OnClick="lbnGenerateLong_Click" CausesValidation="false"></asp:LinkButton>
                <br />

                <h2>Krótki opis</h2>
                <asp:TextBox
                    ID="txtShortDescription"
                    TextMode="MultiLine"
                    Columns="180"
                    Rows="20"
                    runat="server" />

                <asp:HtmlEditorExtender EnableSanitization="false"
                    Id="htmlShort"
                    DisplayPreviewTab="true"
                    DisplaySourceTab="true"
                    TargetControlID="txtShortDescription"
                    runat="server" />
                            </td>
                   
                    </tr>
                    <tr>
                        <td><br /><br />
            <asp:UpdatePanel runat="server" ID="upS">
                <ContentTemplate>
                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Zapisz" />
                            <asp:CheckBox runat="server" Id="chbSavaAndPublish" Text="jedoncześnie publikuj" Checked="false"  Visible="false"/>
                            
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="position: absolute;">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upS">
                    <ProgressTemplate>
                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>

                        </td>
                        <td>
            <asp:UpdatePanel runat="server" ID="upP">
                <ContentTemplate>
                <asp:LinkButton runat="server" ID="lbtnPublish" OnClick="lbtnPublish_Click" Text="Publikuj" />

    
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="position: absolute;">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upP">
                    <ProgressTemplate>
                        <img src="Images/progress.gif" style="height: 20px" alt="" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>


                        </td>
                    </tr>
                </table> 

            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
