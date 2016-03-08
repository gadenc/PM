<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Single.aspx.cs" Inherits="RM.Web.Single" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.s_content{ line-height:20px; padding:10px 15px; padding-bottom:40px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" runat="server">
<img src="../images/20126893251.jpg" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
<div class="s_content">
<%=content%>
</div>
</asp:Content>
