<%@ Page Title="Гараж" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CarProfile.aspx.cs" Inherits="CarDiaryWeb.CarProfile" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
        
    <br />

    
    <table style="width: 100%; height: 100%">
        <tr>
            <td><div style="float:left"><h2>Моите автомобили</h2></div></td>
            <td></td>
            <td><div style="float:left"><asp:Button ID="btnAddNew" OnClick="btnAddNew_Click" Text="Добави нов" Font-Size="22" runat="server" style="margin-left:20px" CssClass="btn"/></div></td>
        </tr>
    </table>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>

</asp:Content>


