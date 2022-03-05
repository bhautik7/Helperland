﻿$(document).ready(function () {

    //Customer --Service History Start

    $('#ServiceProviderRatingModel').modal({
        backdrop: 'static',
        keyboard: false
    });
    $('#ServiceProviderRatingModel').modal('show');

    $("#DisplayRatingServiceProviderInModel").val(3.3);

    $("#DisplayRatingServiceProviderInModel").rating({
        min: 0,
        max: 5,
        step: 0.1,
        size: "xs",
        stars: "5",
        showClear: false,
        showCaption: false,
        readonly: true
    });

    $(".rating-serviceProvider").rating({
        min: 0,
        max: 5,
        step: 0.5,
        size: "sm",
        stars: "5",
        showClear: false,
        showCaption: false
    });
});


function ServiceStartDate(inputDate) {
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
//Customer -- Current Service Requests -- Dashboard Start

//Customer -- Current Service Requests -- Dashboard End