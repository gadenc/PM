(function () {
    var DDSPEED = 3;
    var DDTIMER = 1;
    var menuLi = ["products", "solutions", "service", "training", "partner"]
    MenuId = null;
    for (var i = 0; i < menuLi.length; i++) {
        //new showDiv(menuLi[i]);
    }
    function showDiv(elem) {

        document.getElementById(elem).onmouseover = function () {
            for (var i = 0; i < menuLi.length; i++) {
                document.getElementById(menuLi[i]).className = "topLevel";
            }
            this.className += (this.className.length > 0 ? " " : "") + "current";
            ddMenu(elem, 1);
        }
        document.getElementById(elem).onmouseout = function () {
            ddMenu(elem, -1);

        }
    }
    window.onunload = function () {
        for (var i = 0; i < menuLi.length; i++) {
            //document.getElementById(menuLi[i] + "_menu").style.display = "none";
        }
    };
    // main function to handle the mouse events //
    function ddMenu(id, d) {
        var h = document.getElementById(id);
        var c = document.getElementById(id + '_menu');
        clearInterval(c.timer);
        if (d == 1) {
            clearTimeout(h.timer);
            if (c.maxh && c.maxh <= c.offsetHeight) { return }
            else if (!c.maxh) {
                c.style.display = 'block';
                c.style.height = 'auto';
                c.maxh = c.offsetHeight;
                c.style.height = '0px';
            }
            c.timer = setInterval(function () { ddSlide(c, 1) }, DDTIMER);
        } else {
            h.timer = setTimeout(function () { ddCollapse(c, "", id) }, 5);
        }
    }
    // collapse the menu //
    function ddCollapse(c, temp, id) {
        MenuId = id;
        //alert(MenuId);
        c.timer = setInterval(function () { ddSlide(c, -1) }, DDTIMER);
    }
    // cancel the collapse if a user rolls over the dropdown //
    function cancelHide(id) {
        var h = document.getElementById(id);
        var c = document.getElementById(id + '_menu');
        clearTimeout(h.timer);
        clearInterval(c.timer);
        if (c.offsetHeight < c.maxh) {
            c.timer = setInterval(function () { ddSlide(c, 1) }, DDTIMER);

        }
    }
    // incrementally expand/contract the dropdown and change the opacity //
    function ddSlide(c, d) {
        var currh = c.offsetHeight;
        var dist;
        if (d == 1) {
            dist = (Math.round((c.maxh - currh) / DDSPEED));
        } else {
            dist = (Math.round(currh / DDSPEED));
        }
        if (dist <= 1 && d == 1) {
            dist = 1;
        }
        c.style.height = currh + (dist * d) + 'px';
        c.style.opacity = currh / c.maxh;
        c.style.filter = 'alpha(opacity=' + (currh * 200 / c.maxh) + ')';

        if ((currh < 2 && d != 1) || (currh > (c.maxh - 2) && d == 1)) {
            clearInterval(c.timer);
        }
        if (dist == 0 && MenuId != null) {
            document.getElementById(MenuId).className = "topLevel";
            MenuId = null;
        }
    }


    //newsTab
    var tabs = ["latest", "solu", "suce", "servi", "about"];
    for (var i = 0; i < tabs.length; i++) {
        new showNews(tabs[i]);
    }

    function showNews(elem) {
        document.getElementById(elem).onmouseover = function () {
            if (this.className != "current")
                timerId = setTimeout(function () { tabSelected(elem); }, 200)
            else
                return false;

            document.getElementById(elem).onmouseout = function () {
                if (timerId)
                    clearTimeout(timerId);
            }
        }

        function tabSelected(elem) {
            for (var i = 0; i < tabs.length; i++) {
                document.getElementById(tabs[i]).className = "";
                document.getElementById(tabs[i] + "_news").className = "dn";
            }
            document.getElementById(elem).className += "current";
            document.getElementById(elem + "_news").className = document.getElementById(elem + "_news").className.replace(new RegExp("( ?|^)dn\\b"), "")
        }
    }




    //show users
    function GetCookie(sname) {
        var acookie = document.cookie.split(";");
        for (var i = 0; i < acookie.length; i++) {
            var arr = acookie[i].split("=");
            if (sname == trim(arr[0])) {
                if (arr.length > 1) {
                    userName = arr[1].substring(0, 8);
                    return unescape(userName);
                }
                else {
                    return "";
                }
            }
        }
        return "";
    }




    function trim(trimStr) {
        return trimStr.replace(/(^\s*)|(\s*$)/g, "");
    }
    function GetCurUrl() {
        var curUrl = window.location.href;
        var arrayStr = curUrl.split("?ResponseTicket");
        return arrayStr[0];
    }
    with (document) {
        if (GetCookie("USER_ID").length > 0) {
            /*write("<b>欢迎 <a href='/Home/Login/Default.htm' title='点击修改您的资料'><font color='#76A1CA'>"+GetCookie("USER_ID")+"</font></a></b><li>|</li>&nbsp;&nbsp;<a href='/Aspx/Home/Login/LoginOutPage.aspx?backurl="+GetCurUrl()+"' title='退出登录'>退出</a>");*/
            getElementById("loged").innerHTML = "<b>欢迎 <a href='/My_H3C/' title='点此进入“我的H3C”'><font color='#76A1CA'>" + GetCookie("USER_ID") + "</font>&nbsp;</a></b><li>|</li><a href='/My_H3C/My_ProfileCenter/' title='修改您的个人信息、密码'>修改信息</a> | <a href='/Aspx/Home/Login/LoginOutPage.aspx?backurl=" + GetCurUrl() + "' title='退出登录'>退出&nbsp;</a>";

            getElementById("loged").style.cssFloat = "left";
            getElementById("loged").style.styleFloat = "left";
        }
    }
})();
