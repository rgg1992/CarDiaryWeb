<%@ Page Title="Вход" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CarDiaryWeb.Account.Login" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>

    <div class="row">
                <div class="col-md-4">
            <asp:Image ID="ImageCar" runat="server" ImageUrl="~/Images/car.png" />
            <ul style="list-style-type:none">
                <li><h6>Създаване на профил на автомобил</h6></li>
                <li><h6>Въвеждай разходите за гориво за автомобила си</h6>
                    <ul style="list-style-type:none">
                        <li>
                            <asp:Label ID="Label1" runat="server" Text="Ръчно" Font-Size="9" ForeColor="#2956B2" Font-Bold="true"></asp:Label>
                        </li>
                        <li>
                            <asp:Label ID="Label2" runat="server" Text="От снимка на касов бон" ForeColor="#2956B2" Font-Bold="true" Font-Size="9"></asp:Label>
                        </li>
                    </ul>

                </li>

                <li><h6>Въвеждай разходите по поддръжка за автомобила си</h6></li>
                <li><h6>Следи история на всички разходи за автомобила си</h6></li>
            </ul>

        </div> 

        <hr />

        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                    <h4>Моля въведете потребителско име и парола.</h4>
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-md-2 control-label">Потребителско име</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="UserName" CssClass="textbox" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                CssClass="text-danger" ErrorMessage="Моля въведете потребителско име." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Парола</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="textbox" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="Моля въведете парола." />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <div class="checkbox">
                                <asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe">Запомни ме</asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="LogIn" Text="Вход" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
                <p>
                    <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Регистрирайте се </asp:HyperLink>
                    ако все още нямате акаунт.
                </p>
            </section>
        </div>

        <%--        <div class="col-md-4">
            <section id="socialLoginForm">
                <uc:openauthproviders runat="server" id="OpenAuthLogin" />
            </section>
        </div>--%>
    </div>
</asp:Content>
