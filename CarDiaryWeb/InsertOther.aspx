<%@ Page Title="Въведете друг разход" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InsertOther.aspx.cs" Inherits="CarDiaryWeb.InsertOther" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $("[id$=txtDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://www.google.com/help/hc/images/sites_icon_calendar_small.gif'
            });
            <%--var value = document.getElementById('<%=txtDate.ClientID%>').value--%>
        });
    </script>

    <h2>Информация за разход</h2>
    <hr />

    <br />

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" BackColor="Snow" Width="450" ForeColor="Red" />

    <table style="width: 100%;min-width:680px">
        <tr style="height: 30px;">
            <td style="width: 20%; height: 30px;">
                <div style="height: 30px;">Дата <font color="red">*</font>:</div>
            </td>
            <td style="width: 20%; height: 30px;">
                <div style="height: 30px;">Категория <font color="red">*</font>: </div>
            </td>
            <td style="width: 60%; height: 30px;"></td>
        </tr>
        <tr style="height: 30px;">
            <td style="height: 30px;">
                <div style="height: 30px;">
                    <asp:TextBox ID="txtDate" runat="server" Width="50%" ReadOnly="true" CssClass="textbox"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtDate" ErrorMessage="Моля въведете стойност за Дата!" Display="None" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Моля въведете коректна стойност за Дата (мм/дд/гггг)" ForeColor="Red" ControlToValidate="txtDate" ValidationExpression="^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$" Display="None"></asp:RegularExpressionValidator>
                </div>
            </td>
            <td style="height: 30px;">
                <div style="height: 30px;">
                    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlCategory" runat="server" Height="100%" Style="text-align: left;" CssClass="dropdown"></asp:DropDownList></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 30px;">Километраж : 
            </td>
            <td style="height: 30px;">Цена <font color="red">*</font>:</td>
        </tr>
        <tr>
            <td style="height: 30px;">
                <asp:TextBox ID="txtMileage" runat="server" Width="50%" CssClass="textbox"/>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Моля въведете коректна стойност за Километраж (напр. 113450)" ForeColor="Red" ControlToValidate="txtMileage" ValidationExpression="^[0-9]+$" Display="None"></asp:RegularExpressionValidator>
            </td>
            <td style="height: 30px;">
                <asp:TextBox ID="txtPrice" runat="server" Width="50%" CssClass="textbox" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtPrice" ErrorMessage="Моля въведете стойност за Цена!" Display="None" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Моля въведете коректна стойност за Цена (напр.1,98)" ForeColor="Red" ControlToValidate="txtPrice" ValidationExpression="^[0-9]+[\.,\*]*?[0-9]+$" Display="None"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>Бележки </td>
        </tr>
        <tr>
            <td style="height: 60px">
                <div style="width: 100%; height: 100%; margin: 0; padding: 0;">
                    <asp:TextBox ID="txtNotes" runat="server" Width="100%" Height="90" CssClass="textbox" TextMode="MultiLine"/>
                </div>
            </td>
        </tr>
    </table>

    <br />

    <table style="width: 100%;min-width:680px">
        <tr>
            <td style="width: 20%; height: 30px"></td>
            <td style="width: 20%; height: 30px">
                <asp:Button ID="btnAddCost" Width="100%" runat="server" Text="Добави разход" OnClick="btnAddCost_Click" CssClass="btn"/></td>
            <td style="width: 60%; height: 30px"></td>
        </tr>
    </table>


</asp:Content>
