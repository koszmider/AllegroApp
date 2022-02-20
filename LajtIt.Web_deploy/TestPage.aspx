<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="LajtIt.Web.TestPage" EnableEventValidation="false" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.8/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js" type="text/javascript"></script>
    <script src="https://github.com/douglascrockford/JSON-js/raw/master/json2.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.8/jquery-ui.min.js"
        type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.8/i18n/jquery-ui-i18n.min.js"
        type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#<%= pn1.ClientID %> .sortable").sortable();
            $("#<%= pn1.ClientID %> .sortable").disableSelection();
            //$("#sortable input[type=text]").width($("#sortable img").width() - 10);

            $(".ContainerDiv").hover(
                function () {
                    $(this).find(".deleteClass").show();
                },
                function () {
                    $(this).find(".deleteClass").hide();
                });
            $(".deleteClass").click(function () {
                $(this).closest("li").remove();
            });
            $("#<%= pn1.ClientID %> .orderPhoto").click(function () {
                var photos = $.map($("#<%= pn1.ClientID %> li.ui-state-default"), function (item, index) {
                    var imgDetail = new Object();
                    imgDetail.Id = $(item).find("img").attr("id");
                    imgDetail.Caption = "";//$(item).find("label").html();
                    imgDetail.Order = index + 1;
                    return imgDetail;
                });

                alert("exec");
                var jsonPhotos = JSON.stringify(photos);
                $.ajax({
                    type: "POST",
                    contentType: "application/json",
                    data: "{photos:" + jsonPhotos + "}",
                    url: "AutoComplete.asmx/PhotoDetail",
                    dataType: "json",
                    success: function (data) {
                        alert(data.d);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        debugger;
                    }
                });
            });
        });
        $(function () {
            $("#<%= pn2.ClientID %> .sortable").sortable();
            $("#<%= pn2.ClientID %> .sortable").disableSelection();
            //$("#sortable input[type=text]").width($("#sortable img").width() - 10);

            $(".ContainerDiv").hover(
                function () {
                    $(this).find(".deleteClass").show();
                },
                function () {
                    $(this).find(".deleteClass").hide();
                });
            $(".deleteClass").click(function () {
                $(this).closest("li").remove();
            });
            $("#<%= pn2.ClientID %> .orderPhoto").click(function () {
                var photos = $.map($("#<%= pn2.ClientID %> li.ui-state-default"), function (item, index) {
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
                        alert(data.d);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        debugger;
                    }
                });
            });
        });
    </script>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" ID="pn1">
            <asp:ListView ID="ListView1" runat="server" GroupItemCount="15">
                <LayoutTemplate>
                    <ul class="sortable">
                        <li id="groupPlaceholder" runat="server">1</li>
                    </ul>
                </LayoutTemplate>
                <GroupTemplate>
                    <tr id="itemPlaceholderContainer" runat="server">
                        <td id="itemPlaceholder" runat="server"></td>
                    </tr>
                </GroupTemplate>
                <ItemTemplate>
                    <li class="ui-state-default">
                        <div class="ContainerDiv">
                            <div class="deleteClass">X</div>
                            <img id='<%#Eval("ImageId")%>' src='/images/productcatalog/<%#Eval("FileName")%>' width="100" alt="" />
                            <div style="height: 25px; margin-top: 3px">
                                <label>
                                    <%#Eval("ImageId")%></label>
                            </div>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <input type="button"   class="orderPhoto" value="Save change" />
        </asp:Panel>


         <asp:Panel runat="server" ID="pn2">
            <asp:ListView ID="ListView2" runat="server" GroupItemCount="15">
                <LayoutTemplate>
                    <ul class="sortable">
                        <li id="groupPlaceholder" runat="server">1</li>
                    </ul>
                </LayoutTemplate>
                <GroupTemplate>
                    <tr id="itemPlaceholderContainer" runat="server">
                        <td id="itemPlaceholder" runat="server"></td>
                    </tr>
                </GroupTemplate>
                <ItemTemplate>
                    <li class="ui-state-default">
                        <div class="ContainerDiv">
                            <div class="deleteClass">X</div>
                            <img id='<%#Eval("ImageId")%>' src='/images/productcatalog/<%#Eval("FileName")%>' width="100" alt="" />
                            <div style="height: 25px; margin-top: 3px">
                                <label>
                                    <%#Eval("ImageId")%></label>
                            </div>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <input type="button"   class="orderPhoto" value="Save change" />
        </asp:Panel>
    </form>
</body>
</html>
