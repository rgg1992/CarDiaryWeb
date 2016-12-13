<%@ Page Title="Въведете автомобил" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="InsertCar.aspx.cs" Inherits="CarDiaryWeb.InsertCar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">
        function previewFile() {
            var preview = document.querySelector('#<%=profileImage.ClientID %>');
            var file = document.querySelector('#<%=profileImageUpload.ClientID %>').files[0];
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

    <div>

        <h2>Информация за автомобил</h2>
        <hr />
        <br />

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true" BackColor="Snow" Width="450" ForeColor="Red" />

        <asp:Panel ID="Panel1" runat="server">
            <%-- <asp:ScriptManager runat="server"></asp:ScriptManager>--%>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <table style="width: 100%;">
                        <tr>
                            <td class="cell">
                                <asp:Label ID="lbBrand" runat="server" Text="Марка<font color='red'> *</font>"></asp:Label>
                            </td>
                            <td class="cell">
                                <asp:Label ID="lbModel" runat="server" Text="<font color='black'>Модел</font><font color='red'> *</font>"></asp:Label>
                            </td>
                            <td style="width: 20%;">
                                <asp:Label ID="lbYear" runat="server" Text="<font color='black'>Година</font><font color='red'> *</font>"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td class="cell">
                                <asp:DropDownList ID="ddlBrand" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged" Width="75%" Height="100%" CssClass="dropdown">
                                    <asp:ListItem Text="--Изберете марка--" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="cell">
                                <asp:DropDownList ID="ddlModel" runat="server" AutoPostBack="true" Enabled="false" Width="75%" Height="100%" OnSelectedIndexChanged="ddlModel_SelectedIndexChanged" CssClass="dropdown">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 20%;">
                                <asp:TextBox ID="tbYear" runat="server" Width="20%" CssClass="textbox"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="tbYear" ErrorMessage="Моля въведете стойност за Година!" Display="None" />
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="tbYear" Type="Integer" ForeColor="red" Display="None"></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr runat="server" id="rowLabels" visible="false">
                            <td class="cell" runat="server" id="labelBrand">
                                <asp:Label ID="lbNewBrand" runat="server" Text="Нова марка" Visible="false"></asp:Label>
                            </td>
                            <td class="cell">
                                <asp:Label ID="lbNewModel" runat="server" Text="Нов модел"></asp:Label>
                            </td>
                        </tr>
                        <tr visible="false" runat="server" id="rowTextBox">
                            <td class="cell" runat="server" id="textBrand">
                                <asp:TextBox ID="tbNewBrand" runat="server" Width="75%" Visible="false" CssClass="textbox"></asp:TextBox>
                            </td>
                            <td class="cell">
                                <asp:TextBox ID="tbNewModel" runat="server" Width="75%" CssClass="textbox"></asp:TextBox>
                            </td>
                        </tr>

                    </table>
                    <hr />
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 5%;">
                                <asp:Label ID="lbFuel" runat="server" Text="Гориво<font color='red'> *</font>"></asp:Label>
                            </td>
                            <td style="width: 5%;">
                                <asp:Label ID="lbEngine" runat="server" Text="Двигател<font color='red'> *</font>"></asp:Label>
                            </td>
                            <td style="width: 20%;"></td>
                        </tr>
                        <tr style="height: 30px">
                            <td class="cell">
                                <asp:DropDownList ID="ddlFuel" runat="server" AutoPostBack="true" Width="85%" Height="100%" CssClass="dropdown">
                                    <asp:ListItem Text="--Изберете гориво--" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="cell">
                                <asp:TextBox ID="tbEngine" runat="server" Width="75%" CssClass="textbox"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="tbEngine" ErrorMessage="Моля въведете стойност за Двигател!" Display="None" />
                            </td>
                            <td style="width: 20%;"></td>
                        </tr>
                        <tr>
                            <td class="cell">
                                <asp:Label ID="lbHorsePowers" runat="server" Text="Конски сили<font color='red'> *</font>"></asp:Label>
                            </td>
                            <td class="cell"></td>
                            <td style="width: 20%;"></td>
                        </tr>
                        <tr>
                            <td class="cell">
                                <asp:TextBox ID="tbHorsePowers" runat="server" Width="75%" CssClass="textbox"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="tbHorsePowers" ErrorMessage="Моля въведете стойност за Конски сили!" Display="None" />
                                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="tbHorsePowers" Type="Integer" MinimumValue="1" MaximumValue="2000" ForeColor="red" ErrorMessage="Моля въведете коректна стойност за Конски сили (1 - 2000)" Display="None"></asp:RangeValidator>
                            </td>
                        </tr>
                    </table>

                    <hr />

                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>

    </div>

    
<%--<asp:FileUpload ID="avatarUpload" runat="server" />--%>
        <table style="height:100px">
        <tr style="height:50px">
            <td style="height:50px; width:5%">
                <label class="file-upload">
                    <span><strong>Изберете изображение</strong></span>
                <input ID="profileImageUpload" type="file" name="file" onchange="previewFile()"  runat="server"/>
                    </label>
            </td>
            <td style="height:50px; width:5%">
                <asp:Button ID="Upload" runat="server" Text="Качи изображение" OnClick="Upload_Click" CssClass="btn" />
            </td>
            <td style="width:90%"></td>
        </tr>
        <%--<asp:FileUpload ID="profileImageUpload" runat="server" />--%>
        <tr style="height:50px">
            <td style="height:50px">
        <asp:Label ID="lblStatus" runat="server" Style="color: red"></asp:Label>
                </td>
            </tr>
    </table>


    <asp:Image ID="profileImage" runat="server" Width="300px" Style="min-height: 150px; min-width: 100px;" Height="150px" ImageUrl="~/Images/no_photo.png" />

    <br />
    <br />
    <br />
    <br />

    <asp:Button ID="btnAddCar" runat="server" Text="Добави автомобил" OnClick="btnAddCar_Click" CssClass="btn" />

</asp:Content>


