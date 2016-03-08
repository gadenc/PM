<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="DTcms.Web.admin.category.edit" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>编辑类别信息</title>
<link href="../scripts/ui/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
<link href="../images/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../scripts/jquery/jquery-1.3.2.min.js"></script>
<script type="text/javascript" src="../scripts/jquery/jquery.form.js"></script>
<script type="text/javascript" src="../scripts/jquery/jquery.validate.min.js"></script> 
<script type="text/javascript" src="../scripts/jquery/messages_cn.js"></script>
<script type="text/javascript" src="../scripts/ui/js/ligerBuild.min.js"></script>
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
            allowFileManager: true,
            items: [
						'source', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
						'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
						'insertunorderedlist', '|', 'image', 'link']
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
</script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
<div class="navigation"><a href="javascript:history.go(-1);" class="back">后退</a>首页 &gt; 类别管理 &gt; 编辑类别</div>
<div id="contentTab">
    <ul class="tab_nav">
        <li class="selected"><a onclick="tabs('#contentTab',0);" href="javascript:;">基本信息</a></li>
        <%--<li><a onclick="tabs('#contentTab',1);" href="javascript:;">扩展信息</a></li>--%>
    </ul>

    <div class="tab_con" style="display:block;">
        <table class="form_table">
            <col width="180px"><col>
            <tbody>
            <tr>
                <th>所属父类别：</th>
                <td><asp:DropDownList id="ddlParentId" CssClass="select2 required" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <th>排序数字：</th>
                <td><asp:TextBox ID="txtSortId" runat="server" CssClass="txtInput small required digits" maxlength="10">99</asp:TextBox></td>
            </tr>
            <tr>
                <th>url链接：</th>
                <td><asp:TextBox ID="txtCallIndex" runat="server" CssClass="txtInput normal ime-disabled" maxlength="50"></asp:TextBox><label>该类别的跳转链接</label></td>
            </tr>
            <tr>
                <th>类别名称：</th>
                <td><asp:TextBox ID="txtTitle" runat="server" CssClass="txtInput required" maxlength="100" style="width:350px;" /></td>
            </tr>
<%--
            <tr style="visibility:hidden">
                <th>SEO标题：</th>
                <td></td>
            </tr>
            <tr style="visibility:hidden">
                <th>SEO关健字：</th>
                <td></td>
            </tr>
            <tr style="visibility:hidden"> 
                <th>SEO描述：</th>
                <td></td>
            </tr>--%>
            </tbody>
        </table>
        <asp:TextBox ID="txtSeoTitle" runat="server" CssClass="txtInput" maxlength="255" style="width:350px;" Visible="false"/>
        <asp:TextBox ID="txtSeoKeywords" runat="server" maxlength="255" TextMode="MultiLine" CssClass="small"  Visible="false"/>
        <asp:TextBox ID="txtSeoDescription" runat="server" maxlength="255" TextMode="MultiLine" CssClass="small" Visible="false"/>
    </div>

    <div class="tab_con">
        <table class="form_table">
            <col width="180px"><col>
            <tbody>
            <tr>
                <th>URL链接：</th>
                <td><asp:TextBox ID="txtLinkUrl" runat="server" CssClass="txtInput normal" maxlength="255"></asp:TextBox><label>URL跳转地址</label></td>
            </tr>
            <tr>
                <th>显示图片：</th>
                <td>
                    <asp:TextBox ID="txtImgUrl" runat="server" CssClass="txtInput normal left" maxlength="255"></asp:TextBox>
                    <a href="javascript:;" class="files"><input type="file" id="FileUpload" name="FileUpload" onchange="Upload('SingleFile', 'txtImgUrl', 'FileUpload');" /></a>
                    <span class="uploading">正在上传，请稍候...</span>
                </td>
            </tr>
            <tr>
                <th valign="top">类别介绍：</th>
                <td>
                    <textarea id="txtContent" cols="100" rows="8" style="width:99%;height:250px;visibility:hidden;" runat="server"></textarea>
                </td>
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
