<%@ Page Title="Добави зареждане" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InsertFuel.aspx.cs" Inherits="CarDiaryWeb.InsertFuel" %>
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
        function showDiv() {

            document.getElementById('myHiddenDiv').style.display = "";

            setTimeout('document.images["myAnimatedImage"].src="Images/loader.gif"', 200);

        };

        function previewFile() {
            var preview = document.querySelector('#<%=invoiceImage.ClientID %>');
            var file = document.querySelector('#<%=invoiceUpload.ClientID %>').files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
                preview.src = reader.result;
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
            }
        }
    

    </script>

    <h2>Информация за зареждане</h2>
    <hr />
    
    <h5>Моля изберете начин на въвеждане на информацията : </h5>
    <asp:RadioButtonList ID="radioMethodGroup" runat="server" OnSelectedIndexChanged="radioMethodGroup_SelectedIndexChanged" AutoPostBack="true" RepeatLayout="Flow" RepeatDirection="Horizontal">
        <asp:ListItem>Ръчно въвеждане</asp:ListItem>
        <asp:ListItem style="margin-left: 45px">От снимка на касов бон</asp:ListItem>
    </asp:RadioButtonList>
    <hr />

    <div id="fromPhoto" runat="server" visible="false">
        <table style="height:100px">
        <tr style="height:50px">
            <td style="height:50px; width:5%">
                <label class="file-upload">
                    <span><strong>Изберете изображение</strong></span>
                    <input ID="invoiceUpload" type="file" name="file" onchange="previewFile()"  runat="server"/>
                </label>
            </td>
            <td style="height:50px; width:5%">
                <asp:Button ID="Upload" runat="server" Text="Започни обработка" OnClick="Upload_Click" OnClientClick="showDiv();" CssClass="btn" />
            </td>
            <td style="width:90%"></td>
        </tr>
        <tr style="height:50px">
            <td style="height:50px">
        <asp:Label ID="lblStatus" runat="server" Style="color: red"></asp:Label>
                </td>
            </tr>
    </table>

        <span id='myHiddenDiv' style='display: none'>

            <img src='' id='myAnimatedImage' align='absmiddle'>
            Loading ... 
        </span>
        <hr />

        <asp:Image ID="invoiceImage" runat="server" Width="250px" Style="min-height: 150px; min-width: 100px;" Height="200px" ImageUrl="~/Images/No_image.png" />

    </div>

    <div id="byHand" runat="server" visible="false">

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" BackColor="Snow" Width="450" ForeColor="Red"/>

        <table style="width: 100%;min-width:750px">
            <tr style="height: 30px;">
                <td style="width: 20%; height: 30px;">
                    <div style="height: 30px;">Дата <font color="red">*</font>:</div>
                </td>
                <td style="width: 20%; height: 30px;">
                    <div style="height: 30px;">Гориво <font color="red">*</font>: </div>
                </td>
                <td style="width: 60%; height: 30px;"></td>
            </tr>
            <tr style="height: 30px;">
                <td style="height: 30px;">
                    <div style="height: 30px;">
                        <asp:TextBox ID="txtDate" runat="server" Width="50%" ReadOnly="true" CssClass="textbox"/></div>
                    <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" controltovalidate="txtDate" errormessage="Моля въведете стойност за Дата!" Display="None" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Моля въведете коректна стойност за Дата (мм/дд/гггг)" ForeColor="Red" ControlToValidate="txtDate" ValidationExpression="^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$" Display="None"></asp:RegularExpressionValidator>
                </td>
                <td style="height: 30px;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div style="height: 30px;">
                                <asp:DropDownList ID="ddlFuel" Width="50%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFuel_SelectedIndexChanged" Height="100%" CssClass="dropdown"></asp:DropDownList></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <%--  <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 100%;min-width:750px">
                    <tr>
                        <td style="height: 30px;">Километраж : 
                        </td>
                        <td style="height: 30px;">
                            <asp:Label ID="lbQuantity" runat="server" Text="Количество"></asp:Label>
                            <font color="red">*</font>:</td>
                    </tr>
                    <tr>
                        <td style="height: 30px;">
                            <asp:TextBox ID="txtMileage" runat="server" Width="50%" OnTextChanged="txtMileage_TextChanged" AutoPostBack="True" CssClass="textbox" />
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Моля въведете коректна стойност за Километраж (напр. 113450)" ForeColor="Red" ControlToValidate="txtMileage" ValidationExpression="^[0-9]+$" Display="None"></asp:RegularExpressionValidator>
                        </td>
                        <td style="height: 30px;">
                            <asp:TextBox ID="txtQuantity" runat="server" Width="50%" OnTextChanged="txtQuantity_TextChanged" AutoPostBack="True" CssClass="textbox" />
                            <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtQuantity" errormessage="Моля въведете стойност за Количество!" Display="None" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="Моля въведете коректна стойност за Количество (напр.45,23)" ForeColor="Red" ControlToValidate="txtQuantity" ValidationExpression="^[0-9]+[\.,\*]*?[0-9]+$" Display="None"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px;">Изминато <font color="red">*</font>: 
                        </td>
                        <td style="height: 30px;">Цена <font color="red">*</font>: 
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; height: 30px;">
                            <asp:TextBox ID="txtDistance" runat="server" Width="50%" CssClass="textbox"/>
                            <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator2" controltovalidate="txtDistance" errormessage="Моля въведете стойност за Изминато!" Display="None" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Моля въведете коректна стойност за Изминато (напр. 450)" ForeColor="Red" ControlToValidate="txtDistance" ValidationExpression="^[0-9]+$" Display="None"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 20%; height: 30px;">
                            <asp:TextBox ID="txtSum" runat="server" Width="40%" OnTextChanged="txtSum_TextChanged" AutoPostBack="True" CssClass="textbox"/><asp:DropDownList ID="ddlPrice" Width="45%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPrice_SelectedIndexChanged" Height="100%" Style="text-align: left; margin-left: 15px" CssClass="dropdown"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator3" controltovalidate="txtSum" errormessage="Моля въведете стойност за Цена!" Display="None" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Моля въведете коректна стойност за Цена (напр.1,98)" ForeColor="Red" ControlToValidate="txtSum" ValidationExpression="^[0-9]+[\.,\*]*?[0-9]+$" Display="None"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 60%; height: 30px;">
                            <asp:Label ID="txtPriceInfo" runat="server" Style="color: red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>

        <table style="width: 100%; min-width:750px">
            <tr style="height: 30px;">
                <td style="width: 20%; height: 30px;"></td>
                <td style="width: 20%; height: 30px;">
                    <asp:Button ID="btnAddFuel" Width="100%" runat="server" Text="Добави зареждане" OnClick="btnAddFuel_Click" CssClass="btn" />
                </td>
                <td style="width: 60%; height: 30px;"></td>
            </tr>
        </table>
    </div>

</asp:Content>
