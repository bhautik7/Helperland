
$(document).ready(function () {

    $('#user-management').DataTable({
        paging: true,
        ordering: true,
        searching: false,
        info: false,
        "columnDefs": [
            { "orderable": false, "targets": 1 },
            { "orderable": false, "targets": 2 },
            { "orderable": false, "targets": 4 },
            { "orderable": false, "targets": 7 }
        ],
        "oLanguage": {
            "sInfo": "Total Records: _TOTAL_"
        },
        "dom": '<"top">rt<"bottom"lip><"clear">',
        responsive: true,
        "order": []
    });

    $('#service-requests').DataTable({
        paging: true,
        ordering: true,
        searching: false,
        info: false,
        "columnDefs": [
            { "orderable": false, "targets": 5 }
        ],
        "oLanguage": {
            "sInfo": "Total Records: _TOTAL_"
        },
        "dom": '<"top">rt<"bottom"lip><"clear">',
        responsive: true,
        "order": []
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