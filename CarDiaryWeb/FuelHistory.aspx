<%@ Page Title="История на зарежданията" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FuelHistory.aspx.cs" Inherits="CarDiaryWeb.FuelHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <table style="width: 100%; max-width:750px;min-width:600px;">
        <tr>
            <td style="width: 300px" rowspan="3">
                <asp:Image ID="imgProfile" runat="server" Width="300" Height=" 150" Style="min-height: 150px; min-width: 100px;" />
            </td>
            <td style="width: 35%">
                <asp:Label ID="lbCarTitle" runat="server" Width="100%" Style="text-align: center" Font-Size="20"></asp:Label>
            </td>
            <td style="width: 35%">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 60%">
                            <asp:Label ID="lbConsumptionInfo" Width="100%" runat="server" Style="text-align: center" Font-Size="13"></asp:Label>
                        </td>
                        <td style="width: 40%; background-color: lightyellow">
                            <asp:Label ID="lbDistanceInfo" runat="server" Width="100%" Font-Size="12" Style="text-align: right"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60%">
                            <asp:Label ID="lbConsumption" runat="server" Width="100%" Style="text-align: center" Font-Size="15"></asp:Label>
                        </td>
                        <td style="width: 40%; background-color: lightyellow;">
                            <asp:Label ID="lbLiters" runat="server" Font-Size="12" Width="100%" Style="text-align: right"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td></td>
        </tr>
    </table>
    <br />

    <asp:Menu ID="Menu1"
        runat="server"
        Orientation="Horizontal"
        StaticEnableDefaultPopOutImage="False"
        OnMenuItemClick="Menu1_MenuItemClick"
        cssClass="menu">
        <Items>
            <asp:MenuItem
                Text="Зареждания" Value="0"></asp:MenuItem>
            <asp:MenuItem
                Text="Други разходи" Value="1"></asp:MenuItem>
        </Items>
    </asp:Menu>
    <asp:MultiView
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">
        <asp:View ID="Tab1" runat="server">
            <asp:Panel ID="Panel1" runat="server" Width="60%" Style="margin-right: 30%"></asp:Panel>
        </asp:View>
        <asp:View ID="Tab2" runat="server">
            <asp:Panel ID="Panel2" runat="server" Width="60%" Style="margin-right: 30%"></asp:Panel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
