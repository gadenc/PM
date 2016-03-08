<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="RM.Web.list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.s_list{ padding-left:20px; padding-right:15px; margin-top:15px;}
 p.s_list_t{ height:25px; line-height:25px; background:url(images/li.gif) no-repeat scroll 2px 10px transparent; padding-left:10px;}
 p.s_list_t span{ display:block; float:right;}
 p.s_list_t a{color: #4E4E4E; font-size:14px; font-weight:bold;}
 p.s_list_t a:hover{color:Red; text-decoration:underline;}
.s_list ul li div{ font-size: 12px; color: #626262; line-height: 22px;margin: 10px 5px 10px 15px;word-wrap: break-word;}

/*分页样式*/
.list_page{ height:30px; line-height:30px; margin-bottom:25px; border-bottom:1px dashed #ccc;}
.flickr span{ display:block; float:left; height:30px; line-height:30px; margin-left:5px;}
.flickr a{ display:block; float:left; height:30px; line-height:30px; margin-left:5px;}
.flickr a:hover{ color:#EBA975;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" runat="server">
<img src="../images/20126893251.jpg" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
<!--带分页的列表-->
<div class="s_list">
    <ul>
    <asp:Repeater runat="server" ID="newslist">
    <ItemTemplate>
        <li>
            <p class="s_list_t"><span><%#Eval("add_time","{0:d}")%></span><a href="Detail.aspx?id=<%#Eval("id")%>"><%#Eval("title")%></a></p>
            <div><%#Eval("zhaiyao")%></div>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        <%if (newslist.Items.Count == 0 || newslist == null)
              Response.Write("<li>未添加数据！</li>");
             %>
    </FooterTemplate>
    </asp:Repeater>
    </ul>
    <div class="list_page">
        <div id="PageContent" runat="server" class="flickr right"></div>
    </div>
</div>
</asp:Content>
