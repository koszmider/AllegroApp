<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TimeTracker.aspx.cs" Inherits="LajtIt.Web.TimeTracker" %>


<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Raportowanie czasu pracy</h2>
    <asp:Panel runat="server" GroupingText="Raport dzienny">
        <table>
            <tr>
                <td>
                    
                
                <asp:DropDownList runat="server" ID="ddlUserName" Width="200" AppendDataBoundItems="true" DataTextField="UserName" DataValueField="UserName">
               
                </asp:DropDownList>
                    <asp:TextBox runat="server" ID="txbDate" MaxLength="10" ValidationGroup="add" Style="text-align: center;"
                        Width="70"></asp:TextBox><asp:RegularExpressionValidator runat="server" ControlToValidate="txbDate" Enabled="false"
                            ID="RegularExpressionValidator3" ValidationExpression="201[\d]{1}-[\d]{2}-[\d]{2}"
                            ValidationGroup="add" Text="*" /><asp:CalendarExtender
                                ID="calDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txbDate">
                            </asp:CalendarExtender>
                    <asp:Button runat="server" ID="btnShow" OnClick="btnShow_Click" Text="Pokaż" />
                </td>

            </tr>

        </table>
        <asp:GridView runat="server" ID="gvTimeTracker" AutoGenerateColumns="false"
            DataKeyNames="Id" OnRowEditing="gvTimeTracker_OnRowEditing" OnRowCancelingEdit="gvTimeTracker_OnRowCancelingEdit"
            OnRowUpdating="gvTimeTracker_OnRowUpdating" Style="width: 100%" ShowFooter="true"
            OnRowDataBound="gvTimeTracker_RowDataBound">
            <Columns>

                <asp:CommandField ItemStyle-Width="70" ShowCancelButton="true" ShowEditButton="true"
                    ButtonType="Image" EditImageUrl="~/Images/edit.jpg" UpdateImageUrl="~/Images/save.jpg"
                    CancelImageUrl="~/Images/cancel.jpg" />
                <asp:BoundField DataField="InsertDate" HeaderText="Data wpisu" ReadOnly="true" DataFormatString="{0:yyyy/MM/dd HH:mm}" ItemStyle-Width="100" ItemStyle-Height="50" />
                <asp:TemplateField HeaderText="Czas w godz." ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblHours"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txbHours" Width="50" ValidationGroup="edit"></asp:TextBox><asp:RegularExpressionValidator runat="server"
                            ControlToValidate="txbHours" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="edit" Text="zły format (np: 1 lub 1,5)"></asp:RegularExpressionValidator>

                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:Label runat="server" ID="lblHours"></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Opis">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblComment"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txbComment" TextMode="MultiLine" Rows="3" Columns="30" ValidationGroup="edit"></asp:TextBox><asp:RequiredFieldValidator runat="server"
                            ControlToValidate="txbComment" ValidationGroup="edit" Text="pole wymagane"></asp:RequiredFieldValidator>

                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lokalizacja" ItemStyle-Width="70" Visible="false"    >
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblLocation"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlLocation" DataValueField="LocationTypeId" DataTextField="Name" AppendDataBoundItems="false"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Zadanie" ItemStyle-Width="300"     >
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlTask" Target="_blank" NavigateUrl="UserTask.aspx?id={0}"></asp:HyperLink>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlTask" DataValueField="LocationTypeId" DataTextField="Name" AppendDataBoundItems="false" Visible="false"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
        <asp:Panel runat="server" ID="pnNew" GroupingText="Dziennik aktywności">
            <table>
                <tr>
                    <td>Czas w godz.</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbHours" Width="50" ValidationGroup="new"></asp:TextBox><asp:RegularExpressionValidator runat="server" Display="Dynamic"
                            ControlToValidate="txbHours" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="new" Text="zły format (np: 1 lub 1,5)"></asp:RegularExpressionValidator><asp:RequiredFieldValidator runat="server"
                           Display="Dynamic"     ControlToValidate="txbHours" ValidationGroup="new" Text="pole wymagane"></asp:RequiredFieldValidator>
                        <asp:Label runat="server" ID="lblHourLimits"></asp:Label>
                    </td>
                </tr><!--
                <tr>
                    <td>Lokalizacja</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlLocation" DataValueField="LocationTypeId" DataTextField="Name" AppendDataBoundItems="false"></asp:DropDownList></td>
                </tr>-->
                <tr>
                    <td>Zadanie</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlTaskTracker" DataValueField="TaskId" DataTextField="Title" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">-- bez przypisanego zadania --</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>


                <tr>
                    <td>Opis</td>
                    <td>
                        <asp:TextBox runat="server" ID="txbComment" TextMode="MultiLine" Rows="3" Columns="70" ValidationGroup="new"></asp:TextBox><asp:RequiredFieldValidator runat="server"
                            ControlToValidate="txbComment" ValidationGroup="new" Text="pole wymagane"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" ValidationGroup="new" Text="Zapisz" OnClientClick="if(Page_ClientValidate('new')) confirm('Czy zapisać wprowadzone informacje?');" /></td>
                </tr>
            </table>
        </asp:Panel>
        
                <asp:Chart ID="Chart2" runat="server" Width="900"><Titles ><asp:Title  Font="Times New Roman, 18pt, style=Bold, Italic"  Text="Wykres aktywności" ForeColor="Green" > </asp:Title></Titles>
                  
                    <ChartAreas >
                        <asp:ChartArea Name="ChartArea1">
                            <AxisX   IntervalAutoMode="VariableCount" >
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>

    </asp:Panel>

    <asp:Panel runat="server" GroupingText="Raport miesięczny" Visible="false">
        <asp:GridView runat="server" ID="gvReport" AutoGenerateColumns="false">
            <Columns>

                <asp:BoundField DataField="Year" HeaderText="Rok" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Month" HeaderText="Miesiąc" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="WorkingHours" HeaderText="L.godzin" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" />

            </Columns>

        </asp:GridView>
    </asp:Panel>
</asp:Content>
