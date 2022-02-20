<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    CodeBehind="ProductCatalogForDb.aspx.cs" Inherits="LajtIt.Web.ProductCatalogForDb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/UploadImage.ascx" TagName="UploadImage" TagPrefix="uc" %>
<%@ Register Src="~/Controls/ProductAttributes.ascx" TagName="ProductAttributes" TagPrefix="uc" %>
<%@ Register Src="~/Controls/ProductImages.ascx" TagName="ProductImages" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:PostBackTrigger ControlID="gvProductCatalog" /> 
            <asp:PostBackTrigger ControlID="txbPageSize" /> 
            <asp:PostBackTrigger ControlID="txbPageNo" /> 

<%--            <asp:AsyncPostBackTrigger ControlID="chbAttributesSearch" EventName="CheckedChanged" />--%>
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0">
                <tr valign="top">
                    <td width="200">
                        <asp:ListBox Rows="8" runat="server" ID="lsbxStatus" SelectionMode="Multiple" Width="200">
                            <asp:ListItem Value="1" Selected="True">Aktywny - gotowy</asp:ListItem>
                            <asp:ListItem Value="8">Aktywny - niegotowy</asp:ListItem>
                            <asp:ListItem Value="9">Dostępny u producenta</asp:ListItem>
                            <asp:ListItem Value="10">Niedostępny u producenta</asp:ListItem>
                            <asp:ListItem Value="0">Nieaktywny</asp:ListItem>
                            <%-- <asp:ListItem Value="6">Sklep - aktywny</asp:ListItem>
                            <asp:ListItem Value="2">Allegro - aktywny</asp:ListItem>
                            <asp:ListItem Value="4">Allegro - gotowy</asp:ListItem>
                            <asp:ListItem Value="5">Allegro - niegotowy</asp:ListItem>--%>
                            <asp:ListItem Value="11">Skonfigurowany</asp:ListItem>
                            <asp:ListItem Value="12">Nieskonfigurowany</asp:ListItem>
                            <asp:ListItem Value="-1">Wycofany</asp:ListItem>
                            <asp:ListItem Value="-2">Niewycofany</asp:ListItem>
                            <asp:ListItem Value="-3">Ukryty</asp:ListItem>
                            <asp:ListItem Value="-4">Nieukryty</asp:ListItem> 

                        </asp:ListBox>

                    </td>
                    <td width="200">
                        <asp:ListBox Rows="8" runat="server" ID="lbxSearchSupplier" SelectionMode="Multiple" Width="200" DataTextField="DisplayName" DataValueField="SupplierId"></asp:ListBox>

                    </td>
                    <td width="200">
                        <asp:TextBox runat="server" ID="txbSearchName" MaxLength="50" Width="200"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TBWE2" runat="server"
                            TargetControlID="txbSearchName"
                            WatermarkText="Nazwa/Tytuł/Kod produktu/Ean"
                            WatermarkCssClass="watermarked" />
                        <br />
                         
                       <table>
                           <tr> 
                               <td>
                                 <asp:CheckBox runat="server" ID="chbAttributesSearch" Text="Edytuj atrybuty" />

                                   <asp:LinkButton runat="server" ID="lbtnAttributesEdit" Text="wybierz"></asp:LinkButton>
                                   <asp:ModalPopupExtender ID="mpeAttributesEdit" runat="server"
                                       TargetControlID="lbtnAttributesEdit"
                                       
                                       PopupControlID="pnAttributes2"
                                       BackgroundCssClass="modalBackground"
                                       DropShadow="true"
                                       CancelControlID="imbCancel1"
                                       PopupDragHandleControlID="Panel2" />

                                   <asp:Panel runat="server" ID="pnAttributes2" BackColor="White" Style="width: 900px; background-color: white; height: 600px; padding: 10px">
                                       <asp:Panel runat="server" ID="Panel2">
                                           <div style="text-align:right">
                                               <asp:ImageButton runat="server" ImageUrl="~/Images/cancel.png" Width="20" id="imbCancel1"/>
                                           </div>
                                           Wybór atrybutów do edycji

                                       </asp:Panel>
                                       <div style="width: 900px; height: 500px">
                                           <table>
                                            <tr>
                                                <td>
                                                    <asp:CheckBoxList runat="server" ID="chblAttributeGroups" RepeatColumns="6" DataTextField="Name" DataValueField="AttributeGroupId">

                                                    </asp:CheckBoxList>

                                                </td>
                                            </tr>
                                           </table>



                                       </div>
                                       


                                       
                <asp:CheckBox runat="server" ID="chbAttributesAll" Text="Zaznacz/odznacz wszystko" onclick="javascript:SelectAllCheckboxes(this, 'chblAttributeGroups');" />
                                   </asp:Panel>

                               </td>
                           </tr>
                           <tr>
                               <td>Atrybuty 
                                   <asp:LinkButton runat="server" ID="lbtnAttributes" Text="wybierz"></asp:LinkButton></td>
                               <td>
                                   
            <script type="text/javascript">

                function goAutoCompl() {
                    $("#<%=txbAttribute.ClientID %>").autocomplete({
                                source: function (request, response) {
                                    $.ajax({
                                        url: '<%=ResolveUrl("~/AutoComplete.asmx/GetAttribute") %>',
                                        data: "{ 'query': '" + request.term + "'}",
                                        dataType: "json",
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        success: function (data) {
                                            response($.map(data.d, function (item) {
                                                return {
                                                    label: item.split('|')[0],
                                                    val: item.split('|')[1]
                                                }
                                            }))
                                        },
                                        error: function (response) {
                                            alert(response.responseText);
                                        },
                                        failure: function (response) {
                                            alert(response.responseText);
                                        }
                                    });
                                },
                                select: function (e, i) {
                                    $("#<%=hfAttribute.ClientID %>").val(i.item.val);
                                },
                                minLength: 1
                            });

                            }

                            $(document).ready(function () {
                                goAutoCompl();
                            });
                            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                            function EndRequestHandler(sender, args) {
                                goAutoCompl();
                            }

            </script>


                                   <asp:ModalPopupExtender ID="mpeAttributes" runat="server"
                                       TargetControlID="lbtnAttributes"
                                       
                                       PopupControlID="pnAttributes1"
                                       BackgroundCssClass="modalBackground"
                                       DropShadow="true"
                                       CancelControlID="imbCancel"
                                       PopupDragHandleControlID="Panel1" />

                                   <asp:Panel runat="server" ID="pnAttributes1" BackColor="White" Style="width: 800px; background-color: white; height: 400px; padding: 10px">
                                       <asp:Panel runat="server" ID="Panel1">
                                           <div style="text-align:right">
                                               <asp:ImageButton runat="server" ImageUrl="~/Images/cancel.png" Width="20" id="imbCancel"/>
                                           </div>
                                           Wybór atrybutów

                                       </asp:Panel>
                                       <div style="width: 800px; height: 280px">
                                           <table>
                                               <tr valign="top">
                                                   <td>
                                                       <asp:TextBox runat="server" ID="txbAttribute" Width="400"></asp:TextBox></td>
                                                   <asp:HiddenField ID="hfAttribute" runat="server" />
                                                   <td>
                                                       <asp:DropDownList runat="server" ID="ddlAttributeExists">
                                                           <asp:ListItem Value="1">jest</asp:ListItem>
                                                           <asp:ListItem Value="0">brak</asp:ListItem>
                                                       </asp:DropDownList>
                                                   </td>
                                                   <td>
                                                       <asp:UpdatePanel runat="server" ID="upAdd">
                                                           <ContentTemplate>
                                                               <asp:ImageButton runat="server" ImageUrl="~/Images/add.png" OnClick="btnAttributeAdd_Click" ID="btnAttributeAdd" />

                                                           </ContentTemplate>
                                                       </asp:UpdatePanel>
                                                       <div style="position: absolute;">
                                                           <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upAdd">
                                                               <ProgressTemplate>
                                                                   <img src="Images/progress.gif" style="height: 20px" alt="" />
                                                               </ProgressTemplate>
                                                           </asp:UpdateProgress>
                                                       </div>
                                                   </td>
                                                   <td> </td>
                                               </tr>
                                               <tr valign="top">
                                                   <td colspan="3">
                                           <asp:GridView runat="server" ID="gvAttributes"
                                               OnRowEditing="gvAttributes_RowEditing"
                                               AutoGenerateColumns="false" OnRowDataBound="gvAttributes_RowDataBound" DataKeyNames="Id"
                                                OnRowCancelingEdit="gvAttributes_RowCancelingEdit"
OnRowUpdating="gvAttributes_RowUpdating"
                                               OnRowDeleting="gvAttributes_RowDeleting">
                                               <Columns>
                                                   <asp:CommandField DeleteImageUrl="~/Images/cancel.png" ButtonType="Image" ShowEditButton="true"
                                                       EditImageUrl="~/Images/edit.png"
                                                       ShowDeleteButton="true" ControlStyle-Width="15" />
                                                   
                                                   <asp:TemplateField HeaderText="Nazwa atrybutu" ItemStyle-Width="300">
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblName" runat="server"></asp:Label>

                                                       </ItemTemplate>
                                                       <EditItemTemplate>
                                                           <asp:Label ID="lblName" runat="server"></asp:Label>

                                                           <asp:Panel runat="server" ID="pnRange" Visible="false">
                                                               od<asp:TextBox runat="server" Width="60" ID="txbFrom"></asp:TextBox>
                                                               do<asp:TextBox runat="server" Width="60" ID="txbTo"></asp:TextBox>
                                                           </asp:Panel>

                                                           
                                                           <asp:Panel runat="server" ID="pnText" Visible="false">
                                                               <asp:TextBox runat="server" Width="260" ID="txbText"></asp:TextBox> 
                                                           </asp:Panel>

                                                       </EditItemTemplate>
                                                   </asp:TemplateField>


                                                   <asp:CheckBoxField DataField="Exists" HeaderText="Jest/brak" ItemStyle-HorizontalAlign="Center" />

                                               </Columns>
                                           </asp:GridView></td>
                                                   <td>
                                                       <asp:ListBox Width="250" Rows="10" AutoPostBack="true" DataTextField="Title" OnSelectedIndexChanged="lbxSearchTable_SelectedIndexChanged"
                                                           DataValueField="SearchId" ID="lbxSearchTable" runat="server"></asp:ListBox>


                                                   </td>
                                               </tr>
                                           </table>



                                       </div>
                                       <asp:UpdatePanel runat="server" ID="upSave">
                                           <ContentTemplate>
                                               <table style="width: 100%">
                                                   <tr>
                                                       <td>Zapisz jako: <asp:TextBox runat="server" ID="txbSearchAttributeName"></asp:TextBox></td>
                                                   </tr>
                                                   <tr>
                                                       <td><asp:CheckBox runat="server" ID="chbSearchIsPublic" Text="Udostępnij dla wszystkich" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td style="width: 45%">
                                                           <asp:LinkButton runat="server" ID="btnSave" Text="Zapisz" OnClick="btnSave_Click"  OnClientClick="return confirm('Zapisać szablon?')"/></td>
                                                       <td style="text-align: center; width: 10%">

                                                           <div style="position: absolute;">
                                                               <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="upSave">
                                                                   <ProgressTemplate>
                                                                       <img src="Images/progress.gif" style="height: 20px" alt="" />
                                                                   </ProgressTemplate>
                                                               </asp:UpdateProgress>
                                                           </div>

                                                       </td>
                                                       <td style="text-align: right; width: 45%">
                                                           <asp:LinkButton runat="server" ID="lbtnCancel" Text="Wyczyść" OnClientClick="return confirm('Wyczyścić zapisany szablon ?')" OnClick="lbtnCancel_Click" /></td>

                                                   </tr>
                                               </table>

                                           </ContentTemplate>
                                       </asp:UpdatePanel>



                                   </asp:Panel>


                               </td>
                           </tr>
                       </table>
                    </td>

                    <td>


                        <table>
                            <tr>
                                <td style="font-weight: bold;">Dodatkowe zdjęcia:</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbImages" Text="TAK" /></td>
                                <td style="font-weight: bold;">Cena:</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbPrice0" Text="NIE" /><asp:CheckBox runat="server" ID="chbPrice1" Text="TAK" /></td>
                                <td style="font-weight: bold;">Tylko zestawy:</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbCombo0" Text="NIE" />
                                    <asp:CheckBox runat="server" ID="chbCombo1" Text="TAK" /></td>
                                
                            </tr>
                            <tr> 
                                   
                                <td style="font-weight: bold;">Zdjęcie:</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbImage0" Text="NIE" /><asp:CheckBox runat="server" ID="chbImage1" Text="TAK" /></td>
                                <td style="font-weight: bold;">Id Zew:</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbExtId0" Text="NIE" /><asp:CheckBox runat="server" ID="chbExtId1" Text="TAK" /></td>
                                 
                            
                                <td style="font-weight: bold;">Grupy:</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbGroup0" Text="NIE" />
                                    <asp:CheckBox runat="server" ID="chbGroup1" Text="TAK" /></td>
                            </tr> 
                            <tr>
                                <td colspan="4">
                                    
                        Szukaj w kontekście sklepu:<br />
                        <asp:DropDownList runat="server" ID="ddlShopsSearch" DataValueField="ShopId" DataTextField="Name" AppendDataBoundItems="true">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold;">W sklepie:</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbWithoutShopAssigment" Text="NIE" /><asp:CheckBox runat="server" ID="chbWithShopAssigment" Text="TAK" /></td>
                        
                               
                                <td style="font-weight: bold;">Opisy:</td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chbDescription0" Text="NIE" />
                                    <asp:CheckBox runat="server" ID="chbDescription1" Text="TAK" /></td>
                                
                            </tr>
                        </table>


                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" colspan="4">
                        <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Szukaj" Width="100%" /></td>
                </tr>

            </table>

            <div style="text-align: right">
                <asp:UpdatePanel runat="server" ID="upPage">
                    <ContentTemplate>
                        Produktów:
                        <asp:Label runat="server" ID="lblCount" Text="0"></asp:Label>
                        Na stronie:
                        <asp:TextBox runat="server" ID="txbPageSize" Text="15" AutoPostBack="true" OnTextChanged="btnSearch_Click" Width="45" TextMode="Number"></asp:TextBox>
                        Przejdź do strony:
                        <asp:TextBox runat="server" ID="txbPageNo" AutoPostBack="true" OnTextChanged="txbPageNo_TextChanged" Text="15" Width="45" TextMode="Number"></asp:TextBox><asp:Label runat="server" ID="lblPageNo"></asp:Label>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <div style="position: absolute;">
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upPage">
                        <ProgressTemplate>
                            <img src="Images/progress.gif" style="height: 20px" alt="" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>


            </div> 
             <style>
        .sortable {
            list-style-type: none;
            margin: 0;
            padding: 0;
        }

            .sortable li {
                position: relative;
                margin: 3px 3px 3px 0;
                padding: 1px;
                float: left;
                text-align: left;
            }

        .deleteClass {
            /* PhotoListItem  is relative so relative to it */
            position: absolute;
            top: 1px;
            right: 3px;
            background: black;
            color: Red;
            font-weight: bold;
            font-size: 12px;
            padding: 5px;
            opacity: 0.60;
            filter: alpha(opacity="60");
            margin-top: 3px;
            display: none;
            cursor: pointer;
        }

            .deleteClass:hover {
                opacity: 0.90;
                filter: alpha(opacity="90");
            }
            .ContainerDiv {height:100px}
            .ui-state-default {background-color: white; border: solid 1px silver; background:none;}
    </style>
            <asp:GridView runat="server" ID="gvProductCatalog" DataKeyNames="ProductCatalogId" Width="100%" BackColor="white" PagerSettings-Position="TopAndBottom" PagerSettings-Mode="NumericFirstLast"
                AllowPaging="true" OnPageIndexChanging="gvProductCatalog_OnPageIndexChanged" OnDataBound="gvProductCatalog_DataBound"
                OnRowCommand="gvProductCatalog_RowCommand"
                PageSize="25" ShowFooter="true" OnRowDataBound="gvProductCatalog_OnRowDataBound"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="50" HorizontalAlign="Right" />
                        <ItemTemplate>
                            <itemtemplate><asp:Literal runat="server" ID="liId"></asp:Literal></itemtemplate>
                            <asp:Literal runat="server" ID="LitId"></asp:Literal>
                            <asp:CheckBox runat="server" ID="chbOrder" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <asp:CheckBox runat="server" ID="chbOrder" onclick="javascript:SelectAllCheckboxes(this, 'chbOrder');" />
                        </HeaderTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Width="150" />
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlPreview" Target="_blank" NavigateUrl="/ProductCatalog.Preview.aspx?id={0}&idSupplier={1}">
                                <asp:Image runat="server" ID="imgImage" Width="150" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Produkt">
                        <ItemTemplate>
                            <div style="width: calc(100% - 70px); float: left;">
                                <asp:HyperLink runat="server" ID="hlProduct" NavigateUrl="ProductCatalog.Specification.aspx?id={0}"></asp:HyperLink><%--<br />
                                (<asp:HyperLink runat="server" ID="hlProductAllegro" NavigateUrl="ProductCatalog.Allegro.aspx?id={0}"></asp:HyperLink>)<br />
                                <asp:Label runat="server" ID="lblNewName"></asp:Label>--%>
                            </div>
                            
                            <div style="width: calc(100% - 70px); float: left;">
                            
                                        <uc:ProductImages ID="ucProductImages" runat="server" Visible="false"></uc:ProductImages></div>
                            
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="litTotal"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kod<br>(kod EAN)">
                        <ItemStyle HorizontalAlign="Right" Width="100" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCode"></asp:Label><br />
                            <asp:Label runat="server" ID="lblCode2"></asp:Label><br />
                            <asp:Label runat="server" ID="lblCodeSupplier"></asp:Label><br />
                            <asp:Label runat="server" ID="lblExternalId"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                
   
                    <asp:TemplateField HeaderText="Specyfikacja">
        
                        <ItemStyle HorizontalAlign="Right" Width="652" />
                        <ItemTemplate>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div style="overflow: auto; height: 200px; font-size: 7pt;">

                                        <uc:ProductAttributes ID="ucProductAttributesProduct" runat="server"></uc:ProductAttributes>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ItemTemplate>

                        <FooterTemplate>
                            <asp:UpdatePanel runat="server" ID="upAction1">
                                <ContentTemplate>
                                    <div style="bottom: 0px; background-color: silver; width: 652px; height:40px; padding: 10px; left: auto; position: fixed; top: -120; z-index: 5;">
                                        
                                    <asp:Button runat="server" Text="Zapisz" Width="100%" ID="btnAction1" OnClick="btnAction_Click1" />
                                        </div>
                                    <span style="position: absolute;">
                                        <asp:UpdateProgress ID="UpdateProgress11" runat="server" AssociatedUpdatePanelID="upAction1">
                                            <ProgressTemplate>
                                                <img src="Images/progress.gif" style="height: 20px" alt="" />
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                    </span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    Akcja:
    <asp:DropDownList runat="server" ID="ddlAction" AutoPostBack="true" OnSelectedIndexChanged="ddlAction_OnSelectedIndexChanged">
        <asp:ListItem Value="-1">-- wybierz --</asp:ListItem>
        <asp:ListItem Value="1">Aktywowanie/deaktywowanie produktów</asp:ListItem>
        <asp:ListItem Value="2">Aktualizacja produktów w sklepach</asp:ListItem>  
        <asp:ListItem Value="7">Katalog - Przypisz/zmień dostawców</asp:ListItem>
        <asp:ListItem Value="12">Katalog - Przypisywanie atrybutów</asp:ListItem>
        <asp:ListItem Value="14">Katalog - Przypisywanie produktów do grup</asp:ListItem>
        <asp:ListItem Value="15">Katalog - Wiązanie wzajemne produktów</asp:ListItem>
        <asp:ListItem Value="17">Katalog - Tworzenie nazw produktów</asp:ListItem>
        <asp:ListItem Value="19">Katalog - Wgrywanie zdjęć</asp:ListItem> 
        <asp:ListItem Value="21">Katalog - eksport do pliku</asp:ListItem>
        <asp:ListItem Value="23">Katalog - Czasy dostaw</asp:ListItem>
        <asp:ListItem Value="22">Sklep - tworzenie opisów</asp:ListItem>
    </asp:DropDownList>
    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional"
        ID="upAction">
        <ContentTemplate>
            <asp:Panel runat="server" GroupingText="Akcje" Visible="false" ID="pAction" ClientIDMode="Static">
                <table>
        

                    <tr>
                        <td colspan="2">
                          <asp:Panel ID="pnDelivery" runat="server" Visible="false">
                                Ustaw niestandardowe czasy dostaw dla produktów
                <asp:DropDownList runat="server" ID="ddlShopDelivery" DataTextField="Name" DataValueField="DeliveryId" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">-- ustalony na poziomie producenta -- </asp:ListItem>
                </asp:DropDownList>
                            </asp:Panel>
                            <asp:Panel ID="pnDescriptions" runat="server" Visible="false">
                                Wygeneruj opis i wyślij automatycznie do sklepu:
                                <asp:RadioButtonList runat="server" ID="rblShops" DataValueField="ShopId" DataTextField="Name"></asp:RadioButtonList>
                            </asp:Panel>
                             
                            <asp:Panel ID="pnStatus" runat="server" Visible="false">
                                Aktywuj/deaktywuj produkty.
                                 
                                <table>
                                    <tr valign="top">
                                        <td>
                                            <asp:ListBox runat="server" ID="lsbxSource" SelectionMode="Multiple" Rows="5">
                                                <asp:ListItem Value="1">Katalog produktów - aktywny</asp:ListItem>
                                                <asp:ListItem Value="5">Katalog produktów - skonfigurowany</asp:ListItem>
                                            
                                                <asp:ListItem Value="4">Katalog produktów - ukryty</asp:ListItem>
                                                <asp:ListItem Value="-1">Katalog produktów - wycofany</asp:ListItem>

                                            </asp:ListBox></td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlStatus">
                                                <asp:ListItem Value="0">--- nie zmieniaj nic ---</asp:ListItem>
                                                <asp:ListItem Value="1">aktywny</asp:ListItem>
                                                <asp:ListItem Value="2">nieaktywny</asp:ListItem>

                                            </asp:DropDownList></td>

                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnProductUpdate" runat="server" Visible="false">
                                Aktualizacja produktów                                 
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ListBox runat="server" ID="lsbxShops" SelectionMode="Multiple" DataValueField="ShopId" DataTextField="Name"> 
                                            </asp:ListBox></td>
                                        <td>
                                            <asp:CheckBoxList runat="server" ID="chblShopColumnType" RepeatDirection="Horizontal" DataValueField="ShopColumnTypeId" DataTextField="ColumnName"> 
                                            </asp:CheckBoxList></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                             
                            <asp:Panel ID="pnSupplier" runat="server" Visible="false">
                                Przypisz produkty do dostawcy
                                <asp:DropDownList runat="server" ID="ddlSupplierUpdate" DataValueField="SupplierId" DataTextField="DisplayName">
                                </asp:DropDownList>
                            </asp:Panel>
                   
                            <asp:Panel ID="pnAttributes" runat="server" Visible="false">
                                Przypisz atrybuty produktom
    <uc:ProductAttributes ID="ucProductAttributes" runat="server"></uc:ProductAttributes>

                            </asp:Panel>
                            <asp:Panel ID="pnUpdateShopImages" runat="server" Visible="false">
                                Aktualizuj zdjęcia w sklepie
                                
                            </asp:Panel>
                            <asp:Panel ID="pnCreateNames" runat="server" Visible="false">
                                Aktualizuj nazwy produktów  <br />
                                     <asp:ListBox runat="server" ID="lbxShops" DataValueField="ShopId" DataTextField="Name" SelectionMode="Multiple">
                       
                        </asp:ListBox>
                            </asp:Panel>

                            <asp:Panel ID="pnProductCatalogGroup" runat="server" Visible="false">
                                Przypisz grupę do produktów
                                <br />
                                <table>
                                    <tr>
                                        <td>Producent</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlProductCatalogGroupSupplier" Width="300">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Rodzina</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlProductCatalogGroupFamily" Width="300">
                                            </asp:DropDownList>
                                            <asp:TextBox runat="server" ID="txbFamilyName" Width="200"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server"
                            TargetControlID="txbFamilyName"
                            WatermarkText="Nowa rodzina"
                            WatermarkCssClass="watermarked" />
                                            

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Grupa</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlProductCatalogGroup" Width="300">
                                            </asp:DropDownList>
                                            <asp:TextBox runat="server" ID="txbGroupName" Width="200"></asp:TextBox><asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server"
                            TargetControlID="txbGroupName"
                            WatermarkText="Nowa grupa"
                            WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr> 
                                </table>


                                <asp:CascadingDropDown
                                    ID="ccdSuppliers"
                                    runat="server"
                                    ServicePath="AutoComplete.asmx"
                                    ServiceMethod="GetSuppliers"
                                    TargetControlID="ddlProductCatalogGroupSupplier"
                                    Category="Supplier" />



                                <asp:CascadingDropDown
                                    ID="ccdCountries"
                                    runat="server"
                                    ServicePath="AutoComplete.asmx"
                                    ServiceMethod="GetFamilies"
                                    TargetControlID="ddlProductCatalogGroupFamily"
                                    ParentControlID="ddlProductCatalogGroupSupplier"
                                    Category="Family"
                                    EmptyText="Brak rodzin" />


                                <asp:CascadingDropDown
                                    ID="ccdLeagues"
                                    runat="server"
                                    ServicePath="AutoComplete.asmx"
                                    ServiceMethod="GetGroups"
                                    TargetControlID="ddlProductCatalogGroup"
                                    ParentControlID="ddlProductCatalogGroupFamily"
                                    Category="Group"
                                    EmptyText="Brak grup" />


                                

                            </asp:Panel>
                            <asp:Panel ID="pnShopCreateUpdateProducts" runat="server" Visible="false">
                                <p>
                                    Utwórz/zaktualizuj wybrane produkty w sklepie lajtit.pl
                                </p>
                            </asp:Panel>
                            <asp:Panel ID="pnImages" runat="server" Visible="false">
                                <p>
                                    Utwórz/zaktualizuj wybrane produkty w sklepie lajtit.pl
                                </p>

                                <uc:UploadImage ID="ucUploadImage" runat="server"></uc:UploadImage>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnAction" />
                                </Triggers>

                            </asp:UpdatePanel>
                            <br /><br /><br />
                            <div style="bottom: 0px; background-color: silver; width: 1300px; height:40px; padding: 10px; left: auto; position: fixed; top: -120; z-index: 5;">
                                Zastosuj do <asp:RadioButton runat="server" GroupName="batch" ID="rbtnAddToBatchAll" Text="wszystkich wyszukanych produktów" />
                                <asp:RadioButton runat="server" GroupName="batch" ID="rbtnAddToBatchSelected" Checked="true" Text="wybranych produktów" />
                                <asp:Button runat="server" ID="btnAction" OnClick="btnAction_Click" OnClientClick="return confirm('Wykonać daną akcję?');"
                                    Text="Zapisz" />
                                <div style="text-align:right; height:10px;"><a href="#">na górę</a></div>
                                <span style="position: absolute;">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upAction">
                                        <ProgressTemplate>
                                            <img src="Images/progress.gif" style="height: 20px" alt="" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </span>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
