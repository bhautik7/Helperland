$(document).ready(function() {
    var scroll_start = 0;
    var startchange = $('#site-header');
    var offset = startchange.offset();
    $(document).scroll(function() {
        scroll_start = $(this).scrollTop();
        if (scroll_start > offset.top) {
            $('#nav').css({ "background-color": "rgba(82, 82, 82, 0.9)" });
            $('#navlogo').css({ "height": "54px", "width": "73px" })
        } else {
            $('#nav').css('background-color', 'transparent');
            $('#navlogo').css({ "height": "130px", "width": "175px" });
        }
    });

    $("#policy-button").click(function() {
        $("#policy").addClass("d-none");
    });
});
jQuery('.dropdown-menu li a').click(function() {
    var _this_img = jQuery(this).attr('data-img');
    jQuery(this).closest('.btn-group').find(' .dropdown-toggle img').attr('src', _this_img);
})