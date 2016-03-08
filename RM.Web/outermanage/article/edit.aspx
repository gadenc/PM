<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="DTcms.Web.admin.article.edit" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>编辑文章信息</title>
<link href="../scripts/ui/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
<link href="../images/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../scripts/jquery/jquery-1.3.2.min.js"></script>
<script type="text/javascript" src="../scripts/jquery/jquery.form.js"></script>
<script type="text/javascript" src="../scripts/jquery/jquery.validate.min.js"></script> 
<script type="text/javascript" src="../scripts/jquery/messages_cn.js"></script>
<script type="text/javascript" src="../scripts/ui/js/ligerBuild.min.js"></script>
<script type='text/javascript' src="../scripts/swfupload/swfupload.js"></script>
<script type='text/javascript' src="../scripts/swfupload/swfupload.queue.js"></script>
<script type="text/javascript" src="../scripts/swfupload/swfupload.handlers.js"></script>
<script type="text/javascript" src="../js/function.js"></script>
<script type="text/javascript" charset="utf-8" src="../editor/kindeditor-min.js"></script>
<script type="text/javascript" charset="utf-8" src="../editor/lang/zh_CN.js"></script>
<script src="../../Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
<script type="text/javascript">
    //加载编辑器
    $(function () {
        
        var editor = KindEditor.create('textarea[name="txtContent"]', {
            resizeType: 1,
            uploadJson: '../tools/upload_ajax.ashx?action=EditorFile&IsWater=1',
            fileManagerJson: '../tools/upload_ajax.ashx?action=ManagerFile',
            allowFileManager: true
        });


    });
    //表单验证
    $(function () {
        $("#form1").validate({
            invalidHandler: function (e, validator) {
                parent.jsprint("有 " + validator.numberOfInvalids() + " 项填写有误，请检查！", "", "Warning");
            },
            errorPlacement: function (lable, element) {
                //可见元素显示错误提示
                if (element.parents(".tab_con").css('display') != 'none') {
                    element.ligerTip({ content: lable.html(), appendIdTo: lable });
                }
            },
            success: function (lable) {
                lable.ligerHideTip();
            }
        });

    });
    //初始化上传控件
    $(function () {
        InitSWFUpload("../tools/upload_ajax.ashx", "Filedata", "10240", "../scripts/swfupload/swfupload.swf", 1, 1);

    });
</script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
<div class="navigation"><a href="javascript:history.go(-1);" class="back">后退</a>首页 &gt; 文章管理 &gt; 编辑信息</div>
<div id="contentTab">
    <ul class="tab_nav">
        <li class="selected"><a onclick="tabs('#contentTab',0);" href="javascript:;">基本信息</a></li>
        <li><a onclick="tabs('#contentTab',1);" href="javascript:;">详细描述</a></li>
        <li style="visibility:hidden"><a onclick="tabs('#contentTab',2);" href="javascript:;">SEO选项</a></li>
    </ul>

    <div class="tab_con" style="display:block;">
        <table class="form_table">
            <col width="150px"><col>
            <tbody>
            <tr>
                <th>所属类别：</th>
                <td><asp:DropDownList id="ddlCategoryId" CssClass="select2 required" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <th>标题名称：</th>
                <td><asp:TextBox ID="txtTitle" runat="server" CssClass="txtInput normal required" maxlength="100" /></td>
            </tr>
            <%--<tr >
                <th>推荐类型：</th>
                <td >
                    <asp:CheckBoxList ID="cblItem" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Value="1">允许评论</asp:ListItem>
                        <asp:ListItem Value="1">置顶</asp:ListItem>
                        <asp:ListItem Value="1">推荐</asp:ListItem>
                        <asp:ListItem Value="1">热点</asp:ListItem>
                        <asp:ListItem Value="1">幻灯</asp:ListItem>
                        <asp:ListItem Value="1">隐藏</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>--%>
            <tr>
                <th>文章作者：</th>
                <td><asp:TextBox ID="txtAuthor" runat="server" CssClass="txtInput normal" 
                        maxlength="100" >管理员</asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>文章来源：</th>
                <td><asp:TextBox ID="txtFrom" runat="server" CssClass="txtInput normal" 
                        maxlength="100" >本站</asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>文章摘要：</th>
                <td><asp:TextBox ID="txtZhaiyao" runat="server" maxlength="255" TextMode="MultiLine" CssClass="small" /><label>不填将自动截取内容255个字符</label></td>
            </tr>
            <tr>
                <th>排序数字：</th>
                <td><asp:TextBox ID="txtSortId" runat="server" CssClass="txtInput small required digits" maxlength="10">99</asp:TextBox></td>
            </tr>
            <tr>
                <th>浏览次数：</th>
                <td><asp:TextBox ID="txtClick" runat="server" CssClass="txtInput small required digits" maxlength="10">0</asp:TextBox></td>
            </tr>
            <tr>
                <th>自定义封面：</th>
                <td>
                    <asp:TextBox ID="txtImgUrl" runat="server" CssClass="txtInput normal left" maxlength="255"></asp:TextBox>
                    <a href="javascript:;" class="files"><input type="file" id="FileUpload" name="FileUpload" onchange="Upload('SingleFile', 'txtImgUrl', 'FileUpload');" /></a>
                    <span class="uploading">正在上传，请稍候...</span>
                </td>
            </tr>
            <tr style="visibility:hidden">
                <th valign="top" style="padding-top:10px;">图片相册：</th>
                <td>
                    <input type="text" class="txtInput normal left" />
                    <div class="upload_btn"><span id="upload"></span></div><label>可以上传多张图片。</label>
                    <div class="clear"></div>
                    <!--封面隐藏值.开始-->
                    <!--
                    <input type="hidden" name="focus_photo" id="focus_photo" value=""/>
                    -->
                    <asp:HiddenField ID="focus_photo" runat="server" />
                    <!--封面隐藏值.结束-->
                    <!--上传提示.开始-->
                    <div id="show"></div>
                    <!--上传提示.结束-->
                    <!--图片列表.开始-->
                    <div id="show_list">
                        <ul>
                          <asp:Literal ID="LitAlbumList" runat="server"></asp:Literal>
                        </ul>
                    </div>
                    <!--图片列表.结束-->
                </td>
            </tr>
            <!--
            <tr>
                <th>扩展属性：</th>
                <td>
                    <table border="0" cellspacing="0" cellpadding="0" class="border_table">
                        <tbody>
                        <col width="80px"><col>
                        <tr>
                            <th>属性一</th>
                            <td><input name="nav_url" type="text" value="" class="txtInput middle" /></td>
                        </tr>
                        <tr>
                            <th>属性二</th>
                            <td><input name="nav_url" type="text" value="" class="txtInput middle" /></td>
                        </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            -->
            <!--扩展属性.开始-->
            <asp:Literal ID="LitAttributeList" runat="server"></asp:Literal>
            <!--扩展属性.结束-->

            </tbody>
        </table>
    </div>

    <div class="tab_con">
        <table class="form_table">
            <col width="150px"><col>
            <tbody>
           <%-- <tr>
                <th>赞成人数：</th>
                <td><asp:TextBox ID="txtDiggGood" runat="server" CssClass="txtInput small required digits" maxlength="10">0</asp:TextBox></td>
            </tr>
            <tr>
                <th>反对人数：</th>
                <td><asp:TextBox ID="txtDiggBad" runat="server" CssClass="txtInput small required digits" maxlength="10">0</asp:TextBox></td>
            </tr>--%>
            <tr>
                <th>URL链接：</th>
                <td><asp:TextBox ID="txtLinkUrl" runat="server" CssClass="txtInput normal" maxlength="255"></asp:TextBox><label>URL跳转地址</label></td>
            </tr>
            <tr>
                <th valign="top">详细描述：</th>
                <td>
                    <textarea id="txtContent" cols="100" rows="8" style="width:99%;height:350px;visibility:hidden;" runat="server"></textarea>
                </td>
            </tr>
            </tbody>
        </table>
    </div>

    <div class="tab_con">
        <table class="form_table">
            <col width="150px"><col>
            <tbody>
            <tr>
                <th>SEO标题：</th>
                <td><asp:TextBox ID="txtSeoTitle" runat="server" maxlength="255" CssClass="txtInput normal" /></td>
            </tr>
            <tr>
                <th>SEO关健字：</th>
                <td><asp:TextBox ID="txtSeoKeywords" runat="server" maxlength="255" TextMode="MultiLine" CssClass="small" /></td>
            </tr>
            <tr>
                <th>SEO描述：</th>
                <td><asp:TextBox ID="txtSeoDescription" runat="server" maxlength="255" TextMode="MultiLine" CssClass="small" /></td>
            </tr>
            </tbody>
        </table>
    </div>

    <div class="foot_btn_box">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btnSubmit" onclick="btnSubmit_Click" />
    &nbsp;<input name="重置" type="reset" class="btnSubmit" value="重 置" />
    </div>
</div>
</form>
</body>
</html>
