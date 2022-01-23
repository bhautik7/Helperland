// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    var scroll_start = 0;
    var offSet = 0;
    var startchange = $('#header');
    offSet = startchange.offset();
    $(document).scroll(function () {
        scroll_start = $(this).scrollTop();
        if (scroll_start > offSet.top) {
            $('#nav').css({ "background-color": "rgba(82, 82, 82, 0.9)" });
            $('#navlogo').css({ "height": "54px", "width": "73px" })
        } else {
            $('#nav').css('background-color', 'transparent');
            $('#navlogo').css({ "height": "130px", "width": "175px" });
        }
    });

    $("#policy-button").click(function () {
        $("#policy").addClass("d-none");
    });
});

jQuery('.dropdown-menu li a').click(function () {
    var _this_img = jQuery(this).attr('data-img');
    jQuery(this).closest('.btn-group').find(' .dropdown-toggle img').attr('src', _this_img);
})


// for FAQ page start

$(document).ready(function () {
    $("#accordionExample2").css({ "display": "none" })
    $("#accordionExample").css({ "display": "block" })

    $("#labelCustomer").click(function () {
        $(this).css({ "background-color": "#1d7a8c", "color": "white" });
        $("#labelServiceProvider").css({ "background-color": "#f6f6f6", "color": "#666666" });
        $("#accordionExample2").css({ "display": "none" });
        $("#accordionExample").css({ "display": "block" });
    })

    $("#labelServiceProvider").click(function () {
        $(this).css({ "background-color": "#1d7a8c", "color": "white", "cursor": "pointer" });
        $("#labelCustomer").css({ "background-color": "#f6f6f6", "color": "#666666", "cursor": "pointer" });
        $("#accordionExample").css({ "display": "none" })
        $("#accordionExample2").css({ "display": "block" })
    })
})


// for FAQ page end

// price page start
function scrollToTop() {
    window.scrollTo(0, 0);
}

// price page end

// upcoming page start

function openTab(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}

// Get the element with id="defaultOpen" and click on it


// upcoming page end

