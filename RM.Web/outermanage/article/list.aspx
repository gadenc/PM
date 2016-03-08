<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="DTcms.Web.admin.article.list" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>文章管理</title>
<link type="text/css" rel="stylesheet" href="../scripts/ui/skins/Aqua/css/ligerui-all.css" />
<link type="text/css" rel="stylesheet" href="../images/style.css" />
<script type="text/javascript" src="../scripts/jquery/jquery-1.3.2.min.js"></script>
<%--<script type="text/javascript" src="../scripts/ui/js/ligerBuild.min.js"></script>--%>
<script type="text/javascript" src="../js/function.js"></script>
<script src="../../Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
    <div class="navigation">首页 &gt; 文章管理 &gt; 管理列表</div>
    <div class="tools_box">
	    <div class="tools_bar">
            <div class="search_box">
			    <asp:TextBox ID="txtKeywords" runat="server" CssClass="txtInput"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="搜 索" CssClass="btnSearch" onclick="btnSearch_Click" />
		    </div>
            <a href="edit.aspx?action=<%=DTEnums.ActionEnum.Add %>&channel_id=<%=this.channel_id %>" class="tools_btn"><span><b class="add">添加文章</b></span></a>
            <asp:LinkButton ID="btnSave" runat="server" CssClass="tools_btn" onclick="btnSave_Click"><span><b class="send">保存排序</b></span></asp:LinkButton>
		    <a href="javascript:void(0);" onclick="checkAll(this);" class="tools_btn"><span><b class="all">全选</b></span></a>
            <asp:LinkButton ID="btnDelete" runat="server" CssClass="tools_btn"  
                OnClientClick="return ExePostBack('btnDelete');" onclick="btnDelete_Click"><span><b class="delete">批量删除</b></span></asp:LinkButton>
        </div>
        <div class="select_box">
            请选择：<asp:DropDownList 
                ID="ddlCategoryId" runat="server" CssClass="select2" AutoPostBack="True" onselectedindexchanged="ddlCategoryId_SelectedIndexChanged"></asp:DropDownList>&nbsp;
            <asp:DropDownList ID="ddlProperty" runat="server" CssClass="select2" AutoPostBack="True" onselectedindexchanged="ddlProperty_SelectedIndexChanged" Visible="false">
                <asp:ListItem Value="" Selected="True">所有属性</asp:ListItem>
                <asp:ListItem Value="isMsg">评论</asp:ListItem>
                <asp:ListItem Value="isTop">置顶</asp:ListItem>
                <asp:ListItem Value="isRed">推荐</asp:ListItem>
                <asp:ListItem Value="isHot">热门</asp:ListItem>
                <asp:ListItem Value="isSlide">幻灯片</asp:ListItem>
            </asp:DropDownList>&nbsp;
            <asp:ImageButton ID="ibtnViewTxt" runat="server" ImageUrl="../images/ico-show-txt.png" ToolTip="文字列表视图" onclick="ibtnViewTxt_Click" />
            <asp:ImageButton ID="ibtnViewImg" runat="server" ImageUrl="../images/ico-show-img.png" ToolTip="图像列表视图" onclick="ibtnViewImg_Click" />
	    </div>
    </div>

    <!--列表展示.开始-->
    <asp:Repeater ID="rptList1" runat="server" onitemcommand="rptList_ItemCommand">
    <HeaderTemplate>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
      <tr>
        <th width="6%">选择</th>
        <th align="left">标题</th>
        <th width="13%" align="left">所属类别</th>
        <th width="16%" align="left">发布时间</th>
        <th width="60" align="left">排序</th>
       <%-- <th width="110" >属性</th>--%>
        <th width="8%">操作</th>
      </tr>
    </HeaderTemplate>
    <ItemTemplate>
      <tr>
        <td align="center"><asp:CheckBox ID="chkId" CssClass="checkall" runat="server" /><asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" /></td>
        <td><a href="edit.aspx?channel_id=<%#this.channel_id %>&action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>"><%#Eval("title")%></a></td>
        <td><%#new DTcms.BLL.category().GetTitle(Convert.ToInt32(Eval("category_id")))%></td>
        <td><%#string.Format("{0:g}",Eval("add_time"))%></td>
        <td align="center"><asp:TextBox ID="txtSortId" runat="server" Text='<%#Eval("sort_id")%>' CssClass="txtInput2 small2" onkeypress="return (/[\d]/.test(String.fromCharCode(event.keyCode)));" /></td>
       <%-- <td align="center" >
          <asp:ImageButton ID="ibtnMsg" CommandName="ibtnMsg" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("is_msg")) == 1 ? "../images/ico-0.png" : "../images/ico-0_.png"%>' ToolTip='<%# Convert.ToInt32(Eval("is_msg")) == 1 ? "取消评论" : "设置评论"%>' />
          <asp:ImageButton ID="ibtnTop" CommandName="ibtnTop" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("is_top")) == 1 ? "../images/ico-1.png" : "../images/ico-1_.png"%>' ToolTip='<%# Convert.ToInt32(Eval("is_top")) == 1 ? "取消置顶" : "设置置顶"%>' />
          <asp:ImageButton ID="ibtnRed" CommandName="ibtnRed" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("is_red")) == 1 ? "../images/ico-2.png" : "../images/ico-2_.png"%>' ToolTip='<%# Convert.ToInt32(Eval("is_red")) == 1 ? "取消推荐" : "设置推荐"%>' />
          <asp:ImageButton ID="ibtnHot" CommandName="ibtnHot" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("is_hot")) == 1 ? "../images/ico-3.png" : "../images/ico-3_.png"%>' ToolTip='<%# Convert.ToInt32(Eval("is_hot")) == 1 ? "取消热门" : "设置热门"%>' />
          <asp:ImageButton ID="ibtnSlide" CommandName="ibtnSlide" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("is_slide")) == 1 ? "../images/ico-4.png" : "../images/ico-4_.png"%>' ToolTip='<%# Convert.ToInt32(Eval("is_slide")) == 1 ? "取消幻灯片" : "设置幻灯片"%>' />
        </td>--%>
        <td align="center"><a href="edit.aspx?channel_id=<%#this.channel_id %>&action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>">修改</a></td>
      </tr>
    </ItemTemplate>
    <FooterTemplate>
      <%#rptList1.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"7\">暂无记录</td></tr>" : ""%>
      </table>
    </FooterTemplate>
    </asp:Repeater>
    <!--列表展示.结束-->

    <!--图片展示.开始-->
    <asp:Repeater ID="rptList2" runat="server" onitemcommand="rptList_ItemCommand">
    <HeaderTemplate>
    <div class="photo_list2 clearfix">
      <ul>
    </HeaderTemplate>
    <ItemTemplate>
      <li>
        <div class="box">
          <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" /><asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" />
          <a href="edit.aspx?channel_id=<%#this.channel_id %>&action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>">
          <img src="<%#Eval("img_url").ToString() != "" ? Eval("img_url").ToString() : "../images/noimg.gif"%>" width="96" height="96" class="cover" />
          </a>
          <dl>
            <dt><a href="edit.aspx?channel_id=<%#this.channel_id %>&action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>"><%#Eval("title")%></a></dt>
            <dd>作者：<%#Eval("author")%>&nbsp; 来源：<%#Eval("from")%>&nbsp; 人气：<%#Eval("click")%></dd>
            <dd>发布时间：<%#string.Format("{0:g}",Eval("add_time"))%></dd>
            <dd class="btns">
              <span class="right">
                <%--<asp:ImageButton ID="ibtnMsg" CommandName="ibtnMsg" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("is_msg")) == 1 ? "../images/ico-0.png" : "../images/ico-0_.png"%>' ToolTip='<%# Convert.ToInt32(Eval("is_msg")) == 1 ? "取消评论" : "设置评论"%>' />
                <asp:ImageButton ID="ibtnTop" CommandName="ibtnTop" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("is_top")) == 1 ? "../images/ico-1.png" : "../images/ico-1_.png"%>' ToolTip='<%# Convert.ToInt32(Eval("is_top")) == 1 ? "取消置顶" : "设置置顶"%>' />
                <asp:ImageButton ID="ibtnRed" CommandName="ibtnRed" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("is_red")) == 1 ? "../images/ico-2.png" : "../images/ico-2_.png"%>' ToolTip='<%# Convert.ToInt32(Eval("is_red")) == 1 ? "取消推荐" : "设置推荐"%>' />
                <asp:ImageButton ID="ibtnHot" CommandName="ibtnHot" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("is_hot")) == 1 ? "../images/ico-3.png" : "../images/ico-3_.png"%>' ToolTip='<%# Convert.ToInt32(Eval("is_hot")) == 1 ? "取消热门" : "设置热门"%>' />
                <asp:ImageButton ID="ibtnSlide" CommandName="ibtnSlide" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("is_slide")) == 1 ? "../images/ico-4.png" : "../images/ico-4_.png"%>' ToolTip='<%# Convert.ToInt32(Eval("is_slide")) == 1 ? "取消幻灯片" : "设置幻灯片"%>' />--%>
                <a href="edit.aspx?channel_id=<%#this.channel_id %>&action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>"><img src="../images/ico-6.png" style="vertical-align:middle;" title="修改" /></a>
              </span>
              排序：<asp:TextBox ID="txtSortId" runat="server" Text='<%#Eval("sort_id")%>' CssClass="txtInput2 small2" onkeypress="return (/[\d]/.test(String.fromCharCode(event.keyCode)));" />
            </dd>
          </dl>
          <div class="clear"></div>
        </div>
      </li>
    </ItemTemplate>
    <FooterTemplate>
      <%#rptList2.Items.Count == 0 ? "<div align=\"center\">暂无记录</div>" : ""%>
      </ul>
    </div>
    </FooterTemplate>
    </asp:Repeater>
    <!--图片展示.结束-->
    <div class="line15"></div>
    <div class="page_box">
      <div id="PageContent" runat="server" class="flickr right"></div>
      <div class="left">
         显示<asp:TextBox ID="txtPageNum" runat="server" CssClass="txtInput2 small2" onkeypress="return (/[\d]/.test(String.fromCharCode(event.keyCode)));" 
             ontextchanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox>条/页
      </div>
    </div>
    <div class="line10"></div>
</form>
</body>
</html>
