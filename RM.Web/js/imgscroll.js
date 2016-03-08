(function ($) {
    $.scrollContent = function (obj) {
        var posList = [];
        var posNum = 0;
        var timer = 0;
        var direction = 0;
        var delay = 5000;
        var activeClass = "current"
        if (obj.delay) {
            delay = obj.delay;
        }
        function showPos(num) {
            //obj.content.fadeIn();
            //alert(obj.content.attr("class"));
            if (obj.content) {
                obj.content.fadeOut();
                obj.content.eq(num).fadeIn();
            }
            if (obj.btn) {
                obj.btn.removeClass(activeClass);
                obj.btn.eq(num).addClass(activeClass);
            }
        }
        function viewNext() {
            //alert(obj.content.length);
            posNum++;
            if (posNum >= obj.content.length) {
                posNum = 0;
            }
            showPos(posNum);
        }
        function viewPrev() {
            posNum--;
            if (posNum < 0) {
                posNum = obj.content.length - 1;
            }
            showPos(posNum);
        }
        function autoShow() {
            timer = setInterval(function () {
                //alert(direction + obj.direct);
                if (direction && obj.direct) {
                    viewPrev();
                }
                else {
                    viewNext();
                }
            }, delay);
        }
        function resetScroll() {
            clearInterval(timer);
            autoShow();
        }
        obj.content.bind("mouseover", function () {
            //alert(1);
            clearInterval(timer);
        }).bind("mouseout", function () {
            autoShow();
        });
        if (obj.btn) {
            obj.btn.each(function (i) {
                $(this).bind("click", function () {
                    showPos(i);
                    posNum = i;
                    resetScroll();
                });
            });
        }
        if (obj.next) {
            obj.next.bind("mouseover", function () {
                direction = 0;
                viewNext();
                resetScroll();
            });
        }
        if (obj.prev) {
            obj.prev.bind("mouseover", function () {
                direction = 1;
                viewPrev();
                resetScroll();
            });
        }
        showPos(0);
        autoShow();
    }
})(jQuery);


//使用方法
//$.scrollContent({
//    content: $("[rel=scroll_box_content]"),
//    btn: $("[rel=js_btn_list]"),
//    prev: $("[rel=js_btn_prev]"),
//    next: $("[rel=js_btn_next]"),
//    delay: 7000,
//    direct: true
//});