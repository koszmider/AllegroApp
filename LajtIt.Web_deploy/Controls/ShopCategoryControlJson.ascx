<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopCategoryControlJson.ascx.cs"
    Inherits="LajtIt.Web.Controls.ShopCategoryControlJson" %>


<script type="text/javascript"> 

    function categories() {
        $(document).ready(function () {
            $(function () {
                $.ajax({
                    type: "POST",
                    url: '<%=ResolveUrl("~/AutoComplete.asmx/GetShopCategory") %>',
            data: "{ 'shopCategoryId':'" + $('#<%=hidStartParent.ClientID %>').val() + "', 'shopTypeId':'" + $('#<%=hidShopTypeId.ClientID %>').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {

                var lstCustomers = $('#<%=lsbCategories.ClientID %>');

                lstCustomers.empty();

                if ($('#<%=hidStartParent.ClientID %>').val() != "0")
                    lstCustomers.append($("<option></option>").val("0").html("[..]"));


                $.each(r.d, function () {

                    if ($('#<%=hidSelected.ClientID %>').val() == this['Value'])
                        lstCustomers.append($("<option selected></option>").val(this['Value']).html(this['Text']));
                    else
                        lstCustomers.append($("<option></option>").val(this['Value']).html(this['Text']));
                });

            },
            error:
                function (r) {

                    // alert(3);
                }
        });
     });
    });
        $(document).ready(function () {
            $('#<%=lsbCategories.ClientID %>').change(function () {
            $.ajax({
                type: 'POST',
                url: '<%=ResolveUrl("~/AutoComplete.asmx/GetShopCategory") %>',
                data: "{ 'shopCategoryId':'" + $(this).val() + "', 'shopTypeId':'" + $('#<%=hidShopTypeId.ClientID %>').val() + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {

                    id = $('#<%=lsbCategories.ClientID %> option:selected').val();

                    $('#<%=hidSelected.ClientID %>').val(id);


                    if (r.d.length == 0)
                        return;

                    var lstCustomers = $('#<%=lsbCategories.ClientID %>');
                    lstCustomers.empty();
                    if (id != "0")
                        lstCustomers.append($("<option></option>").val("0").html("[..]"));


                    $.each(r.d, function () {
                        lstCustomers.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });

                    $('#<%=hidStartParent.ClientID %>').val(id);
                },
                error:
                    function (r) {

                        //  alert(3); 
                    }
            });
        });
    });
    }


 
        $(document).ready(function () {
            $(function () {
                $.ajax({
                    type: "POST",
                    url: '<%=ResolveUrl("~/AutoComplete.asmx/GetShopCategory") %>',
                    data: "{ 'shopCategoryId':'" + $('#<%=hidStartParent.ClientID %>').val() + "', 'shopTypeId':'" + $('#<%=hidShopTypeId.ClientID %>').val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {

                        var lstCustomers = $('#<%=lsbCategories.ClientID %>');

                lstCustomers.empty();

                if ($('#<%=hidStartParent.ClientID %>').val() != "0")
                    lstCustomers.append($("<option></option>").val("0").html("[..]"));


                $.each(r.d, function () {

                    if ($('#<%=hidSelected.ClientID %>').val() == this['Value'])
                        lstCustomers.append($("<option selected></option>").val(this['Value']).html(this['Text']));
                    else
                        lstCustomers.append($("<option></option>").val(this['Value']).html(this['Text']));
                });

                    },
                    error:
                        function (r) {

                            // alert(3);
                        }
                });
            });
        });
        $(document).ready(function () {
            $('#<%=lsbCategories.ClientID %>').change(function () {
            $.ajax({
                type: 'POST',
                url: '<%=ResolveUrl("~/AutoComplete.asmx/GetShopCategory") %>',
                data: "{ 'shopCategoryId':'" + $(this).val() + "', 'shopTypeId':'" + $('#<%=hidShopTypeId.ClientID %>').val() + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {

                    id = $('#<%=lsbCategories.ClientID %> option:selected').val();

                    $('#<%=hidSelected.ClientID %>').val(id);


                    if (r.d.length == 0)
                        return;

                    var lstCustomers = $('#<%=lsbCategories.ClientID %>');
                    lstCustomers.empty();
                    if (id != "0")
                        lstCustomers.append($("<option></option>").val("0").html("[..]"));


                    $.each(r.d, function () {
                        lstCustomers.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });

                    $('#<%=hidStartParent.ClientID %>').val(id);
                },
                error:
                    function (r) {

                        //  alert(3); 
                    }
            });
            });
        });
 
</script>
<asp:UpdatePanel runat="server" ID="upCategory">
    <ContentTemplate>
        <asp:ListBox Font-Names="Courier" runat="server" Rows="8" ID="lsbCategories" Width="300"
            DataValueField="ShopCategoryId" DataTextField="Name">
            <asp:ListItem Value="0"></asp:ListItem>

        </asp:ListBox>
        <asp:HiddenField runat="server" ID="hidSelected" Value="0" />
        <asp:HiddenField runat="server" ID="hidStartParent" Value="0" />
        <asp:HiddenField runat="server" ID="hidShopTypeId" Value="0" />
    </ContentTemplate>
</asp:UpdatePanel>
