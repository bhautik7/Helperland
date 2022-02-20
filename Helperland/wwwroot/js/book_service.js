
function CheckPostalCode() {
    var PostalCode = $('#PostalCode').val();
    console.log(PostalCode);
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
                    openTab(event, 'step-2');
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

function FillCustomerAddressList() {
    $("#customerAddressList").html("Loading Addresses...").load('/Home/GetCustomerAddressList?userId=' + _logInUserId);
    $('#UserPostalCode').val($('#PostalCode').val());
}


function AddressBox(show) {
    if (show == true) {
        $('#btn-Add-address').hide();
        $('#address_form').show();
    }
    else {
        $('#address_form').hide();
        $('#btn-Add-address').show();
    }
}

function AddNewCustomerAddress() {
    var regularExpressionPhoneNumber = new RegExp("^[0-9]{10}$");

    if ($('Userinputstreet').val() == "") {
        $('#ErrorMessageUserinputstreet').html("Please Enter Street Name");
    }
    else {
        $('#ErrorMessageUserinputstreet').html("");
    }

    if ($('Userinputphone').val() == "") {
        if (regularExpressionPhoneNumber.test($('Userinputphone'))) {
            $('#ErrorMessageUserinputphone').html("Please Enter Valid Phone number.");
        }
        else {
            $('#ErrorMessageUserinputphone').html("");
        }
    }

    if ($('#ErrorMessageUserinputstreet').val() != "" || $('#ErrorMessageUserinputphone').val() != "") {
        return;
    }

    var userAddress = {};

    userAddress.streetName = $("#Userinputstreet").val();
    userAddress.houseNumber = $("#Userinputhouse").val();
    userAddress.postalCode = $("#Userinputpostal").val();
    userAddress.city = $("#Userinputcity").val();
    userAddress.phoneNumber = $("#Userinputphone").val();


    $.ajax({
        url: '/Home/AddCustomerAddress',
        type: 'post',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(userAddress),
        success: function (result) {
            /*ResetAddressBoxYourDetailsTab();*/
            /*AddressBox(false);*/
            FillCustomerAddressList();
        },
        error: function (err) {
            console.log(err);
        }
    });

}







$(document).ready(function () {
        $("#datepicker").datepicker();
 });

function openTab(evt, tabName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("nav-item");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tabName).style.display = "block";
    evt.currentTarget.className += " active";
}

// Get the element with id="defaultOpen" and click on it
//document.getElementById("defaultOpen").click();

function SelectBeds() {
    var x = document.getElementById("bedSelect").value;
    document.getElementById("numberOfbeds").innerHTML = x;
}

function SelectBaths() {
    var x = document.getElementById("bathSelect").value;
    document.getElementById("numberOfbaths").innerHTML = x;
}

function SelectDate() {
    var x = document.getElementById("DateSelect").value;
    document.getElementById("serviceDate").innerHTML = x;
}

function SelectTime() {
    var x = document.getElementById("TimeSelect").value;
    var [h, m] = x.split(":");
    var time = h >= 12 ? 'PM' : 'AM';
    document.getElementById("serviceTime").innerHTML = (h % 12 + 12 * (h % 12 == 0)) + ":" + m + time;
}

var prevBasic = 0;
function SelectHours() {
    console.log(prevBasic);
    var x = parseFloat(document.getElementById("hourSelect").value);
    document.getElementById("BasicHours").innerHTML = x;
    var totalTime = parseFloat(document.getElementById("totalTime").innerHTML);
    document.getElementById("totalTime").innerHTML = totalTime + x - prevBasic;
    var prevBasic = document.getElementById("BasicHours").innerHTML;
}

function addExtra(ele, row) {
    var x = ele.children[0];
    console.log(row);
    var y = document.getElementById(row);
    console.log(y);

    if (x.classList.contains('active')) {
        x.classList.remove('active')
        if (!y.classList.contains('hidden')) { y.classList.add('hidden') }
        var totalTime = parseFloat(document.getElementById("totalTime").innerHTML);
        document.getElementById("totalTime").innerHTML = totalTime - 0.5;
    }
    else {
        x.classList.add("active");
        if (y.classList.contains('hidden')) { y.classList.remove('hidden') }
        var totalTime = parseFloat(document.getElementById("totalTime").innerHTML);
        document.getElementById("totalTime").innerHTML = totalTime + 0.5;
    }
}

var address;
function show_form() {
    if (address == 1) {
        document.getElementById("address_form").style.display = "block";
        return address = 0;
    } else {
        document.getElementById("address_form").style.display = "none";
        return address = 1;
    }

}
function cancel_form() {
    if (address == 1) {
        document.getElementById("address_form").style.display = "inline";
        return address = 0;
    } else {
        document.getElementById("address_form").style.display = "none";
        return address = 1;
    }
}
