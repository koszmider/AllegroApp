<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductImages.ascx.cs"
    Inherits="LajtIt.Web.Controls.ProductImages" %>
  <script type="text/javascript">
     
        $(function () {
            $("#<%= pn1.ClientID %> ul.sortable").sortable({
                update: function (event, ui) {
                   
                    var photos = $.map($("#<%= pn1.ClientID %> li.ui-state-default"), function (item, index) {
                        var imgDetail = new Object();
                        imgDetail.Id = $(item).find("img").attr("id");
                        imgDetail.Caption = "";//$(item).find("label").html();
                        imgDetail.Order = index + 1;
                        return imgDetail;
                    });
                    var jsonPhotos = JSON.stringify(photos);
           
                    $.ajax({
                        type: "POST",
                        contentType: "application/json",
                        data: "{photos:" + jsonPhotos + "}",
                        url: "AutoComplete.asmx/PhotoDetail",
                        dataType: "json",
                        success: function (data) {
                            if (data.d == false)
                                alert("Nie udało się zapisać zmian");
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                            alert(errorThrown);
                            debugger;
                        }
                    });
                 //   alert('update: ' + jsonPhotos)
                }
            });
            $("#<%= pn1.ClientID %> ul.sortable").disableSelection();
            //$("#sortable input[type=text]").width($("#sortable img").width() - 10);

            $("#<%= pn1.ClientID %> div.ContainerDiv").hover(
                function () {
                    $(this).find(".deleteClass").show();
                },
                function () {
                    $(this).find(".deleteClass").hide();
                });
            $("#<%= pn1.ClientID %> div.deleteClass").click(function () {
            
                $(this).closest("li").remove();
                var photos = $.map($("#<%= pn1.ClientID %> li.ui-state-default"), function (item, index) {
                    var imgDetail = new Object();
                    imgDetail.Id = $(item).find("img").attr("id");
                    imgDetail.Caption = "";//$(item).find("label").html();
                    imgDetail.Order = index + 1;
                    return imgDetail;
                });
                var jsonPhotos = JSON.stringify(photos);

        
                $.ajax({
                    type: "POST",
                    contentType: "application/json",
                    data: "{photos:" + jsonPhotos + "}",
                    url: "AutoComplete.asmx/PhotoDetail",
                    dataType: "json",
                    success: function (data) {
                        if (data.d == false)
                            alert("Nie udało się zapisać zmian");
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                        alert(errorThrown);
                        debugger;
                    }
                });
            });
            
        });
       
  </script>
   
 <asp:Panel runat="server" ID="pn1">
            <asp:ListView ID="ListView1" runat="server" GroupItemCount="15">
                <LayoutTemplate>
                    <ul class="sortable">
                        <li id="groupPlaceholder" runat="server">1</li>
                    </ul>
                </LayoutTemplate>
                <GroupTemplate>
                    
                        <div id="itemPlaceholder" runat="server"></div>
                   
                </GroupTemplate>
                <ItemTemplate>
                    <li class="ui-state-default">
                        <div class="ContainerDiv">
                            <div class="deleteClass">X</div>
                            <img id='<%#Eval("ImageId")%>' src='/images/productcatalog/<%#Eval("FileName")%>' width="100" alt="" />
                   
                        </div>
                    </li>
                </ItemTemplate>
            </asp:ListView> 
        </asp:Panel>
<asp:Repeater runat="server" ID="rpImages" OnItemDataBound="rpImages_ItemDataBound" Visible="false">
    <HeaderTemplate>
        <div>
    </HeaderTemplate>
    <FooterTemplate></div></FooterTemplate>
    <ItemTemplate>
        <div style="float:left; border:solid 1px silver; margin:2px;">
            <asp:ImageButton runat="server" ID="imgbImage" Width="50" OnClick="imgbImage_Click" />
            <asp:Image runat="server" ID="imgImage" Width="50" Visible="false" />
        <div style="text-align:center">
            
            <asp:ImageButton runat="server" ID="imgbDelete"  Width="15" OnClick="imgbDelete_Click"  ImageUrl="~/Images/cancel.png"/></div></div>
    </ItemTemplate>
</asp:Repeater>
