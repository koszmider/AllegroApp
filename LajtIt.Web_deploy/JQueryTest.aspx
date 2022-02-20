<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JQueryTest.aspx.cs" Inherits="LajtIt.Web.JQueryTest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="/scripts/jquery-ui-1.12.1.custom/jquery-1.12.4.js" type="text/javascript"></script>
    <link href="/Scripts/jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="/Scripts/jquery-ui-1.12.1.custom/jquery-ui.js"></script>

    <script>
        //$(document).ready(function () {
        //    if (jQuery) {
        //        // jQuery is loaded  
        //       // alert("Yeah!");
        //    } else {
        //        // jQuery is not loaded
        //        alert("Doesn't Work");
        //    }
        //});


        //$(function () {
        //    $("#rbl").click(function () { 


        //        //var t = $("input[name='rbl']:checked + label").text();

        //        //alert(v + " - " + t);
        //        //
        //        //list.append(new Option(t, v));
        //        var list = $("#ddlItems");
        //        list.find('option').remove();

        //        $.each($("input[name*='rbl']:checked"), function () {

        //            var v = $("input[name*='rbl']:checked label").text();
        //            //var t = $("input[name='rbl']:checked + label").text();
        //            list.append(new Option(v, v));
        //        });

        //    })
        //});



    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div>
                    <asp:CheckBoxList runat="server" ID="rbl" ClientIDMode="Static" RepeatColumns="5">
                    </asp:CheckBoxList>
                    <asp:DropDownList runat="server" ID="ddlItems" ClientIDMode="Static"></asp:DropDownList>
                  
             <%--       <asp:DynamicPopulateExtender ID="dp" runat="server"
                        TargetControlID="ddlItems"
                        ClearContentsDuringUpdate="true"
                        PopulateTriggerControlID="rbl"
                        ServiceMethod="DynamicPopulateMethod"  
                        UpdatingCssClass="dynamicPopulate_Updating" />--%>
                                    <asp:CascadingDropDown
                                        ID="Cascadingdropdown2"
                                        runat="server"
                                        ServicePath="AutoComplete.asmx"
                                        ServiceMethod="GetAttributes"
                                        TargetControlID="ddlItems"
                                        ParentControlID="b"
                                       
                                        Category="Attribute"
                                        EmptyText="Brak" />

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
