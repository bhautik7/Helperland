
//global variables
var _serviceHourlyRate = 18.0;
var _basicServiceHour = 3;
var _extraServiceCount = 0;
var _logInUserId = 0;
var _oldServiceHour = 0;


//Manage Tab - Fill, Active, Disabled
function ManageBookServiceTab() {

    console.log("in manage book service tab");

    var bookServiceTabs = document.querySelectorAll('#book-service-tab button');
    var activeTab = false;
    for (const tab of bookServiceTabs) {
        if ($("#" + tab.id).hasClass("active")) {
            activeTab = true;
        }
        else if (activeTab == false) {
            $("#" + tab.id).addClass("fill");
            $("#" + tab.id).removeClass("disabled");
        }
        else if (activeTab == true) {
            $("#" + tab.id).addClass("disabled");
            $("#" + tab.id).removeClass("fill");
        }
    }
}

function SetupServiceTabClicked() {
    ManageBookServiceTab();
    ResetScheduleAndPlanTab();
    ResetYourDetailsTab();
    ResetMakePaymentTab();
}

function ScheduleAndPlanTabClicked() {
    ManageBookServiceTab();
    ResetYourDetailsTab();
    ResetMakePaymentTab();
}

function YourDetailsTabClicked() {
    ManageBookServiceTab();
    ResetMakePaymentTab();
}



//Reset Tab - 2 : Schedule & plan - start
function ResetYourDetailsTab() {
    $('input:radio[name=UserAddressListYourDetailTab]:checked').prop('checked', false);
    ResetAddressBoxYourDetailsTab();
}

function CheckServiceTimeLimit() {
    var totalHour = 0;
    startHour = parseFloat($('#ServiceTime').val());
    totalHour = _basicServiceHour + (_extraServiceCount * 0.5);
    if ((startHour + totalHour) > 21) {
        $('#lblServiceTimeErrorMessage').html('Could not completed the service request, because service booking request is must be completed within 21:00 time');
        $('#lblServiceTimeErrorMessage').removeClass('d-none');
    }
    //else if (_basicServiceHour < 3 || (_basicServiceHour + (_extraServiceCount * 0.5) > 12)) {
    else if (_basicServiceHour < 3) {
        $('#ConfirmServiceTimeModal').modal({
            backdrop: 'static',
            keyboard: false
        });
        $('#ConfirmServiceTimeModal').modal('show');
    }
    else {
        $('#lblServiceTimeErrorMessage').html('');
        $('#lblServiceTimeErrorMessage').addClass('d-none');
    }
}


function ResetAddressBoxYourDetailsTab() {
    $("#UserStreetName").val("");
    $("#UserHouseNumber").val("");
    $("#UserPhoneNumber").val("");
    $("#UserCity").val($("#UserCity option:first").val());
}
//Reset Tab - 3 : Schedule & plan - end

//Reset Tab - 4 : Make Payment - start
function ResetMakePaymentTab() {
    $("#PromoCode").val("");
    $("#CardNumber").val("");
    $("#CardExpiryDate").val("");
    $("#CardCVC").val("");
    $("#TermsAndCondition").prop("checked", false);
}

//Reset Tab - 2 : Schedule & plan - start
function ResetScheduleAndPlanTab() {
    console.log("in reset and reschedule");
    _basicServiceHour = 3;

    var date = new Date();
    date.setDate(date.getDate() + 1);
    var nextDate = date.getFullYear().toString() + "-" + AppendZero((date.getMonth() + 1).toString()) + "-" + AppendZero(date.getDate().toString());

    //set value to input type date for ServiceDate  
    $('#ServiceDate').val(nextDate);

    $("#ServiceTime").val($("#ServiceTime option:first").val());
    $("#ServiceHours").val($("#ServiceHours option:first").val());
    $("#txtComment").val("");
    $("#chkHasPet").prop("checked", false);

    ResetExtraServicesScheduleAndPlanTab();

    FillServiceDateTimePaymentSummary();
    console.log("Basic hours:" + _basicServiceHour);
    $('#lblBasicServiceHours').html(_basicServiceHour + " Hrs");
    TotalPayment();
}

function AppendZero(input) {
    if (input.length == 1) {
        return '0' + input;
    }
    return input;
}


function TotalPayment() {
    
    var tatalHour = 0;
    var totalPayment = 0;

    tatalHour = _basicServiceHour + (_extraServiceCount * 0.5);
    totalPayment = tatalHour * _serviceHourlyRate;

    console.log("total payment:" + totalPayment);

    $('#lblTotalServiceTime').html(tatalHour + " Hrs");
    $('#lblPerCleaning').html("$ " + totalPayment);
    $('#lblTotalPayment').html("$ " + totalPayment);
}


function ResetExtraServicesScheduleAndPlanTab() {
    _extraServiceCount =0;
    $('input[name="ExtraServices"]').each(function () {
        this.checked = false;
    });
    $('#lblExtraServices').html("");
    $('#lblExtraServices').addClass('d-none');
}


function FillServiceDateTimePaymentSummary() {
    console.log("in fill service date time payment summary");
    var ServiceDate = new Date($('#ServiceDate').val());
    console.log("service date:" + ServiceDate);
    $('#lblServiceDate').html(AppendZero(ServiceDate.getDate().toString()) + "/" + AppendZero((ServiceDate.getMonth() + 1).toString()) + "/" + ServiceDate.getFullYear() + " " + $('#ServiceTime option:selected').text());
}

function ScrollToBookServiceTab() {
    $('html,body').animate({ scrollTop: $("#book-service-tab").offset().top - 215 }, 300);
}


function CheckPostalCode() {
    var PostalCode = $('#PostalCode').val();
    
    var RegExpressPostalCode = new RegExp("^[1-9][0-9]{5}$");

    if (PostalCode == "") {
        $('#PostalCodeErrorMessage').html("Please Enter Postal Code.");
        return;
    }
    else if (!RegExpressPostalCode.test(PostalCode)) {
        $('PostalCodeErrorMesage').html("please enter valid postal code");
    }
    else {
        $.ajax({
            url: '/Home/CheckPostalCode',
            type: 'post',
            dataType: 'json',
            data: { "PostalCode": PostalCode },

            

            success: function (result) {
                if (result) {
                    $('#Schedule-and-Plan-tab').tab('show');
                    ManageBookServiceTab();
                    ScrollToBookServiceTab();

                    console.log("after scroll to book service tab");

                    var date = new Date();

                    console.log("before date:" + date);

                    date.setDate(date.getDate() + 1);

                    console.log("after date:" + date);

                    var nextDate = date.getFullYear().toString() + "-" + AppendZero((date.getMonth() + 1).toString()) + "-" + AppendZero(date.getDate().toString());

                    //set value to input type date for ServiceDate  
                    $('#ServiceDate').val(nextDate);
                    $('#ServiceDate').attr("min", nextDate);

                    FillServiceDateTimePaymentSummary();

                    var basic = $('#lblBasicServiceHours').html();
                    console.log("before :" + basic);

                    
                    $('#lblBasicServiceHours').html(_basicServiceHour + " Hrs");

                    basic = $('#lblBasicServiceHours').html();
                    console.log("after :" + basic);

                    TotalPayment();
                }
                
                else {
                    $('#PostalCodeErrorMessage').html("We are not providing service in this area. We’ll notify you if any helper would start working near your area.");
                }
            },

            error: function (err) {
                console.log(err)
            }

        });
    }
}


function ScheduleAndPlanCompleted(userId) {
    console.log("user id:" + userId);
    if ($('#lblServiceTimeErrorMessage').html() == "") {
        $('#Your-Details-tab').tab('show');
        ManageBookServiceTab();
        ScrollToBookServiceTab();

        _logInUserId = userId;

        //Load log in user Address List And City DropDown in your Detail tab 
        FillCustomerAddressList();
       FillCityDropDown();

        //clear div error message for your Detail Tab
        $('#Your-Details-tabContent-ErrorMessage').html("");
    }
}



//3rd Tab - Your Detail -- start
function FillCustomerAddressList() {
    $("#customerAddressList").html("Loading Addresses...").load('/Home/GetCustomerAddressList?userId=' + _logInUserId);
    $('#UserPostalCode').val($('#PostalCode').val());
}
function FillCityDropDown() {
    var postalCode = $('#PostalCode').val();

    $('#preloader').removeClass("d-none");

    $.ajax({
        url: '/Home/GetCitiesByPostalCode',
        type: 'post',
        dataType: 'json',
        data: { "postalCode": postalCode },
        success: function (resp) {

            $('#preloader').addClass("d-none");

            $('#UserCity').empty();
            resp.forEach((city) => {
                $('#UserCity').append($("<option></option>").val(city.cityName).html(city.cityName));
            });
        },
        error: function (err) {
            console.log(err);
        }
    });
}


function AddressBox(show) {
    if (show == true) {
        $('#btn-Add-address').hide();
        $('#address-form').show();
    }
    else {
        $('#address-form').hide();
        $('#btn-Add-address').show();
    }
}

function AddNewCustomerAddress() {
    var regularExpressionPhoneNumber = new RegExp("^[0-9]{10}$");

    if ($('#UserStreetName').val() == '') {
        $('#ErrorMessageUserStreetName').html("Please Enter Street Name");
    }
    else {
        $('#ErrorMessageUserStreetName').html("");
    }

    if ($('#UserPhoneNumber').val() != "") {
        if (!regularExpressionPhoneNumber.test($('#UserPhoneNumber').val())) {
            $('#ErrorMessageUserPhoneNumber').html("Please Enter Valid Phone number.");
        }
        else {
            $('#ErrorMessageUserPhoneNumber').html("");
        }
    }
    else {
        $('#ErrorMessageUserPhoneNumber').html("");
    }

    if ($('#ErrorMessageUserStreetName').text() != "" || $('#ErrorMessageUserPhoneNumber').text() != "") {
        return;
    }
    var userAddress = {};

    userAddress.streetName = $("#UserStreetName").val();
    userAddress.houseNumber = $("#UserHouseNumber").val();
    userAddress.postalCode = $("#UserPostalCode").val();
    userAddress.city = $("#UserCity").val();
    userAddress.phoneNumber = $("#UserPhoneNumber").val();

    $.ajax({
        url: '/Home/AddCustomerAddress',
        type: 'post',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(userAddress),
        success: function (resp) {
            ResetAddressBoxYourDetailsTab();
            AddressBox(false);
            FillCustomerAddressList();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function SelectCustomerAddress(isAddressListAvailable) {
    if ($('#isUserAddressListAvailable').val() == 'false') {
        $('#Your-Details-tabContent-ErrorMessage').removeClass("d-none");
        BootstrapAlert('Your-Details-tabContent-ErrorMessage', "Please add an address", "danger");
    }
    else {
        if ($('input[name=UserAddressListYourDetailTab]:checked').length > 0) {
            $('#Make-Payment-tab').tab('show');
            ManageBookServiceTab();
            ScrollToBookServiceTab();

            $('#Your-Details-tabContent-ErrorMessage').addClass("d-none");
        }
        else {
            $('#Your-Details-tabContent-ErrorMessage').removeClass("d-none");
            BootstrapAlert('Your-Details-tabContent-ErrorMessage', "Please Select Address.", "danger");
            ScrollToBookServiceTab();
        }
    }
}

function CompleteBooking() {

    console.log("in complete booking");

    
    var ServiceRequest = {};

    ServiceRequest.userId = _logInUserId;

    console.log("service request id:" + _logInUserId);

    //Tab - 1
    ServiceRequest.postalCode = $("#PostalCode").val();

    ServiceRequest.extraServicesName = [];

    $('input[name="ExtraServices"]').each(function () {
        if (this.checked) {
            ServiceRequest.extraServicesName.push(this.value);
            _extraServiceCount += 1;//side
        }
    });

    console.log("extra service:" + ServiceRequest.extraServicesName);


    //Tab - 2
    ServiceRequest.serviceStartDate = $("#ServiceDate").val();
    ServiceRequest.serviceStartTime = $("#ServiceTime option:selected").text(); //text value
    ServiceRequest.serviceHourlyRate = _serviceHourlyRate;
    ServiceRequest.serviceHours = _basicServiceHour + (_extraServiceCount * 0.5);
    ServiceRequest.extraHours = _extraServiceCount * 0.5;

    console.log("extra hours:" + ServiceRequest.extraHours);
    console.log("service time:" + ServiceRequest.serviceTime);
    console.log("service date:" + ServiceRequest.serviceDate);
  
    totalAmount = (_basicServiceHour + (_extraServiceCount * 0.5)) * _serviceHourlyRate;

    ServiceRequest.subTotal = totalAmount;
    ServiceRequest.totalCost = totalAmount;
    ServiceRequest.comments = $("#txtComment").val();
    ServiceRequest.hasPets = $('#chkHasPet').prop('checked');

    //Tab - 3

    ServiceRequest.userAddressId = $('input[name=UserAddressListYourDetailTab]:checked').attr("id");

    //Tab - 4

    ServiceRequest.paymentDone = true;

    $.ajax({
        url: '/Home/BookCustomerServiceRequest',
        type: 'post',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(ServiceRequest),
        success: function (resp) {
            console.log("service request id:" + resp.serviceRequestId);
            $("#lblServiceRequestId").html(resp.serviceRequestId);
            $('#BookServiceMessageModal').modal({
                backdrop: 'static',
                keyboard: false
            });
            $('#BookServiceMessageModal').modal('show');
        },
        error: function (err) {
            console.log(err);
        }
    });
}


//Fill Payment Summary
$('#ServiceDate').change(function () {
    var serviceDate = $('#ServiceDate').val();
    console.log("service date:"+serviceDate);
    FillServiceDateTimePaymentSummary();
});
$('#ServiceTime').change(function () {
    var serviceTime = $('#ServiceTime').val();
    console.log("service time:" + serviceTime);
    FillServiceDateTimePaymentSummary();
    CheckServiceTimeLimit();
});

function FillServiceDateTimePaymentSummary() {
    var ServiceDate = new Date($('#ServiceDate').val());
    $('#lblServiceDate').html(AppendZero(ServiceDate.getDate().toString()) + "/" + AppendZero((ServiceDate.getMonth() + 1).toString()) + "/" + ServiceDate.getFullYear() + " " + $('#ServiceTime option:selected').text());
}


$('#ServiceHours').focus(function () {
    _oldServiceHour = this.value;
});

$('#ServiceHours').change(function () {
    var serviceHour = parseFloat($('#ServiceHours').val());

    if (_extraServiceCount > 0) {
        serviceHour = serviceHour - (_extraServiceCount * 0.5);
    }

    _basicServiceHour = serviceHour;

    CheckServiceTimeLimit();
    TotalPayment();
    $('#lblBasicServiceHours').html(_basicServiceHour + " Hrs");
});

$('input[name="ExtraServices"]').change(function () {

    var htmlContent = '';
    var newExtraServiceCount = 0;

    $.each($("input[name='ExtraServices']:checked"), function () {
        htmlContent = htmlContent + '<p class="mb-1"><label>' + $(this).next('label').find("span.heading").text() + '</label ><label> 30 Mins</label > </p >';
        newExtraServiceCount = newExtraServiceCount + 1;
    });

    var totalHour = _basicServiceHour + (_extraServiceCount * 0.5);

    if (newExtraServiceCount > _extraServiceCount) { //on extra Service Added, Add it to total Hour
        totalHour = totalHour + ((newExtraServiceCount - _extraServiceCount) * 0.5);
    }
    else { //on extra Service remove, Substract it to total Hour
        totalHour = totalHour - ((_extraServiceCount - newExtraServiceCount) * 0.5);
    }

    _extraServiceCount = newExtraServiceCount;

    $("#ServiceHours").val(totalHour);

    if (htmlContent == "") {
        $('#lblExtraServices').addClass('d-none');
    }
    else {
        $('#lblExtraServices').removeClass('d-none');
    }
    $('#lblExtraServices').html('<p class="mb-1">Extras</p>' + htmlContent);

    TotalPayment();
    CheckServiceTimeLimit();
});



//Bootstrap alert
function BootstrapAlert(id, message, type) {
    var wrapper = document.createElement('div')
    wrapper.innerHTML = '<div class="alert alert-' + type + ' alert-dismissible" role="alert">' + message + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>'
    $('#' + id).html(wrapper);
}



//$(document).ready(function () {
//        $("#datepicker").datepicker();
// });

//function openTab(evt, tabName) {
//    var i, tabcontent, tablinks;
//    tabcontent = document.getElementsByClassName("tabcontent");
//    for (i = 0; i < tabcontent.length; i++) {
//        tabcontent[i].style.display = "none";
//    }
//    tablinks = document.getElementsByClassName("nav-item");
//    for (i = 0; i < tablinks.length; i++) {
//        tablinks[i].className = tablinks[i].className.replace(" active", "");
//    }
//    document.getElementById(tabName).style.display = "block";
//    evt.currentTarget.className += " active";
//}

//// Get the element with id="defaultOpen" and click on it
////document.getElementById("defaultOpen").click();

//function SelectBeds() {
//    var x = document.getElementById("bedSelect").value;
//    document.getElementById("numberOfbeds").innerHTML = x;
//}

//function SelectBaths() {
//    var x = document.getElementById("bathSelect").value;
//    document.getElementById("numberOfbaths").innerHTML = x;
//}

//function SelectDate() {
//    var x = document.getElementById("DateSelect").value;
//    document.getElementById("serviceDate").innerHTML = x;
//}

//function SelectTime() {
//    var x = document.getElementById("TimeSelect").value;
//    var [h, m] = x.split(":");
//    var time = h >= 12 ? 'PM' : 'AM';
//    document.getElementById("serviceTime").innerHTML = (h % 12 + 12 * (h % 12 == 0)) + ":" + m + time;
//}

//var prevBasic = 0;
//function SelectHours() {
//    console.log(prevBasic);
//    var x = parseFloat(document.getElementById("hourSelect").value);
//    document.getElementById("BasicHours").innerHTML = x;
//    var totalTime = parseFloat(document.getElementById("totalTime").innerHTML);
//    document.getElementById("totalTime").innerHTML = totalTime + x - prevBasic;
//    var prevBasic = document.getElementById("BasicHours").innerHTML;
//}

//function addExtra(ele, row) {
//    var x = ele.children[0];
//    console.log(row);
//    var y = document.getElementById(row);
//    console.log(y);

//    if (x.classList.contains('active')) {
//        x.classList.remove('active')
//        if (!y.classList.contains('hidden')) { y.classList.add('hidden') }
//        var totalTime = parseFloat(document.getElementById("totalTime").innerHTML);
//        document.getElementById("totalTime").innerHTML = totalTime - 0.5;
//    }
//    else {
//        x.classList.add("active");
//        if (y.classList.contains('hidden')) { y.classList.remove('hidden') }
//        var totalTime = parseFloat(document.getElementById("totalTime").innerHTML);
//        document.getElementById("totalTime").innerHTML = totalTime + 0.5;
//    }
//}

//var address;
//function show_form() {
//    if (address == 1) {
//        document.getElementById("address_form").style.display = "block";
//        return address = 0;
//    } else {
//        document.getElementById("address_form").style.display = "none";
//        return address = 1;
//    }

//}
//function cancel_form() {
//    if (address == 1) {
//        document.getElementById("address_form").style.display = "inline";
//        return address = 0;
//    } else {
//        document.getElementById("address_form").style.display = "none";
//        return address = 1;
//    }
//}
