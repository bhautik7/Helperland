$(document).ready(function () {

    var url = window.location.href;
    // hightlight active link
    $("#sideBarAdminPanel li a").each(function () {
        // checks if its the same on the address bar
        if (url == (this.href)) {
            $(this).addClass("active");
        }
    });

    $(".sub-menu ul").hide();
    $(".sub-menu a").click(function () {
        $(this).parent(".sub-menu").children("ul").slideToggle("100");
        $(this).find(".arrow-icon").parent("a").parent("li").toggleClass("open");
    });

    $("#txtFromDate").blur(function () {
        if (!$("#txtFromDate").val()) {
            $('#txtFromDate').attr('type', 'text');
        } else {
            $('#txtFromDate').attr('type', 'date');
        }
    });

    $("#txtToDate").blur(function () {
        if (!$("#txtToDate").val()) {
            $('#txtToDate').attr('type', 'text');
        } else {
            $('#txtToDate').attr('type', 'date');
        }
    });

});

function DateFormatDDMMYYYY(inputDate) {
    const date = new Date(inputDate);
    return AppendZero(date.getDate().toString()) + "/" + AppendZero((date.getMonth() + 1).toString()) + "/" + date.getFullYear().toString();
}

function ServiceTime(inputDate, totalHour) {
    const date = new Date(inputDate);
    date.setMinutes(date.getMinutes() + (totalHour * 60));
    return AppendZero(date.getHours().toString()) + ":" + AppendZero(date.getMinutes().toString());
}

//appent 0 to single digit number for month and date
function AppendZero(input) {
    if (input.length == 1) {
        return '0' + input;
    }
    return input;
}

//Bootstrap alert
function BootstrapAlert(id, message, type) {
    var wrapper = document.createElement('div')
    wrapper.innerHTML = '<div class="alert alert-' + type + ' alert-dismissible" role="alert">' + message + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>'
    $('#' + id).html(wrapper);
}