<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserTask.aspx.cs" Inherits="LajtIt.Web.UserTask" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Zadania</h2>

    <asp:UpdatePanel runat="server" ID="up">
        <ContentTemplate>

            <table style="width: 100%">
                <tr>
                    <td style="width: 200px;">Przypisane do...</td>
                    <td style="width: 500px;">
                        <asp:CheckBoxList runat="server" ID="chblAssignedUser" RepeatColumns="6" DataTextField="UserName" DataValueField="UserId">
                        </asp:CheckBoxList></td>
                </tr>
                <tr>
                    <td style="width: 200px;">Priorytet</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbPriority" TextMode="Number" MaxLength="3" Width="80" Text="0" ValidationGroup="new"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txbPriority" ErrorMessage="pole wymagane" ValidationGroup="new"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 200px;">Data zakończenia</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbDate" MaxLength="10" Width="80" Style="text-align: center;" ValidationGroup="new"></asp:TextBox><asp:RegularExpressionValidator runat="server" ControlToValidate="txbDate" Enabled="false" ValidationGroup="new"
                            ID="RegularExpressionValidator3" ValidationExpression="201[\d]{1}-[\d]{2}-[\d]{2}" Text="*" /><asp:CalendarExtender
                                ID="calDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDate">
                            </asp:CalendarExtender><asp:RequiredFieldValidator id="rfvDate" Display="Dynamic" runat="server" ValidationGroup="new" ErrorMessage="wymagana data" ControlToValidate="txbDate"></asp:RequiredFieldValidator>
                         <asp:CheckBox runat="server" ID="chbIsRecurringTask" Text="Zadanie cykliczne" AutoPostBack="true" OnCheckedChanged="chbIsRecurringTask_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;">Kategoria</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlTaskType" Width="200" DataTextField="Name" DataValueField="TaskTypeId" ValidationGroup="new">
                        </asp:DropDownList>
                        <asp:CompareValidator runat="server" ID="cv" ControlToValidate="ddlTaskType" ValueToCompare="1" ErrorMessage="przypisz kategorię" ValidationGroup="new" Operator="NotEqual" Type="String"></asp:CompareValidator>

                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;">Tytuł</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbTitle" MaxLength="100" Width="80%" ValidationGroup="new"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txbTitle" ErrorMessage="pole wymagane" ValidationGroup="new"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 200px;">Opis</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbComment" TextMode="MultiLine" Rows="20" Width="80%" ValidationGroup="new"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txbComment" ErrorMessage="pole wymagane" ValidationGroup="new"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 200px;">Status</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlTaskTrackerStatus" Width="200" DataTextField="Name" DataValueField="TaskTrackerStatusId">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 200px;">Szacunkowy czas</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbWorkingHours" MaxLength="5" Width="40" ValidationGroup="new"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txbWorkingHours" ValidationGroup="new" Display="Dynamic" ErrorMessage="pole wymagane"></asp:RequiredFieldValidator><asp:RegularExpressionValidator runat="server" Display="Dynamic"
                            ValidationGroup="new" ControlToValidate="txbWorkingHours" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" Text="zły format (np: 1 lub 1,5)"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="width: 200px;">Uwagi do zmiany statusu</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbStatusComment" TextMode="MultiLine" Rows="5" Width="80%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Załączniki</td>
                    <td>
                        <asp:Label runat="server" ID="lblAtt" Text="Zapisz zadanie by móc dodawać załączniki"></asp:Label>
                        <asp:Panel runat="server" Visible="false" ID="pAtt">
                            <table>
                                <tr>
                                    <td>
                                        <table style="width: 100%;">
                                            <tr valign="top">

                                                <td>
                                                    <asp:UpdatePanel runat="server">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnAtt" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <input type="file" id="myfile" multiple="multiple" name="myfile" runat="server" size="100" />&nbsp;&nbsp;&nbsp;
                                     
                                    <asp:Button ID="btnAtt" runat="server" Text="Zapisz pliki" OnClick="btnAtt_Click" CausesValidation="false" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel runat="server">

                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="gvFiles" AutoGenerateColumns="false" OnRowDataBound="gvFiles_RowDataBound" ShowHeader="true" Width="100%">
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
                                            <asp:TemplateField HeaderText="Nazwa pliku">
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" ID="hlFile" NavigateUrl="/Files/UsersTasks/{0}/{1}" Target="_blank"></asp:HyperLink>
                                                    <asp:HiddenField runat="server" ID="hidFileName" />
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data dodania">
                                                <ItemStyle Width="150" />
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblData"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                    </asp:GridView>
                                    <asp:Button runat="server" Visible="false" ID="btnDelete" Text="Usuń pliki" OnClick="btnDelete_Click" OnClientClick=" return confirm('Usunąć zaznaczone pliki?');" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="position: absolute;">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="up">
                                <ProgressTemplate>
                                    <img src="Images/progress.gif" style="height: 20px" alt="" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Zapisz" ValidationGroup="new" OnClientClick="if(Page_ClientValidate('new')) return confirm('Czy zapisać wprowadzone informacje?');" />

                        &nbsp;&nbsp;&nbsp;<a href="UserTasks.aspx">Wróć do listy zadań</a>
                    </td>
                </tr>

            </table>
            <p></p>

            <asp:GridView runat="server" ID="gvTasks" AutoGenerateColumns="false" Width="100%" EmptyDataText="Brak historii zmian">
                <Columns>
                    <asp:BoundField DataField="InsertDate" HeaderText="Data dodania" DataFormatString="{0:yyyy/MM/dd<br>HH:mm}"
                        ItemStyle-Width="100" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="InsertUser" HeaderText="Zmiana przez..." ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="TaskStatusName" HeaderText="Status" ItemStyle-Width="200" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Comment" HeaderText="Uwagi" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                </Columns>

            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
