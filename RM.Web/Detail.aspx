<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="RM.Web.Detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>安徽恒科信息技术有限公司</title>
    <link href="css/menu00.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="css/index.css" />
    <link rel="stylesheet" type="text/css" href="css/about.css" />
    <script type="text/javascript" src="js/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="js/imgscroll.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //导航二级菜单代码
            var st = 180;
            $(".navli").mouseenter(function () {
                $(this).parent().find(".stair").stop(false, true).fadeOut();
                $(this).find(".stair").stop(false, true).slideDown(st);
            }).mouseleave(function () {
                $(this).parent().find(".stair").stop(false, true).fadeOut();
                $(this).find(".stair").stop(false, true).slideUp(st);
            });

            //首页图片滚动效果
            //动态添加按钮
            var m_count = $(".imgswap").find("li").length;
            for (var i = 0; i < m_count; i++)
                $(".imgscrollmenu ul").append("<li></li>");

            //自动播放
            $.scrollContent({
                content: $(".imgswap li"),
                btn: $(".imgscrollmenu ul li"),
                direct: true
            });


        });
        //document.ready   end

        //新闻滚动js
        function NewScroll(obj) {
            if ($(obj).find("ul li").length > 2) {
                $(obj).find("ul").animate({
                    marginTop: "-25px"
                }, 500, function () {
                    $(this).css({ marginTop: "0px" }).find("li:first").appendTo(this);
                })
            }
        }
        var t1 = setInterval('NewScroll(".new_m")', 4000);
    </script>
    <style>
    .d_content{padding:10px;}
    .d_c_t{ height:30px; font-weight:bold; line-height:30px; font-size:14px; border-bottom:1px dashed #ccc; padding-bottom:10px;}
    .d_c_c{ margin-top:15px; line-height:18px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="top">
        <div class="logo fl">
            <img alt="安徽恒科信息技术有限公司" src="images/header_logo.png" /></div>
        <div class="headinfo fr">
            <span class="nowday " id="UCheader_lbl">
                <script type="text/javascript">
                    var newdate = new Date(); document.write(newdate);</script>
            </span>
            <div>
                <a href="#">资质荣誉</a> <span>|</span> <a href="#">企业文化</a> <span>|</span> <a href="#">
                    在线招聘</a> <span>|</span> <a href="#">联系我们</a> <span>|</span> <a href="#">登录</a>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="nav">
        <div id="wrap" align="left">
            <div id="menuss">
                <ul id="navss">
                    <li class="navli"><a class="b" href="Index.aspx"><font style="font-size: 14px">首页</font></a></li>
                    <asp:Literal runat="server" ID="nav"></asp:Literal>
                </ul>
            </div>
        </div>
    </div>
    <div style="width: 1002px; margin: 0 auto;">
        <div class="banner">
                <img src="../images/20126893251.jpg" />
        </div>
        <div style="width: 1002px; margin: 0 auto; height: 15px; background: url(images/menu_bj_3.gif) top center no-repeat;">
        </div>
        <div class="news">
            <div class="new_l fl">
                恒科动态：</div>
            <div class="new_m fl">
                <ul>
                    <li><a href="#">安徽恒科信息技术有限公司，正式上线……</a></li>
                    <li><a href="#">123123213123……</a></li>
                    <li><a href="#">121245fmjgf……</a></li>
                </ul>
            </div>
            <div class="new_r fl">
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="d_content">
            <div class="d_c_t"><%=title%></div>
            <div class="d_c_c"><%=content%></div>
    </div>
    </div>
    <div class="foot">
        <div class="copyright">
            <div class="H25">
                版权所有：&copy;2008-2014 安徽恒科信息科技有限公司 皖ICP备05001281号</div>
            <div class="H25">
                地址:合肥市安徽大学科技园电子楼206室</div>
            <div class="H25">
                总机：0551-5367428 传真：0551-5367448&nbsp;&nbsp;<script type="text/javascript" src="http://s16.cnzz.com/stat.php?id=4225021&web_id=4225021&show=pic"
                    language="JavaScript"></script>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
