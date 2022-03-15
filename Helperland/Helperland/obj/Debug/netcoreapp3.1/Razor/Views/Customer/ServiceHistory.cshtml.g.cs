#pragma checksum "F:\Helperland\Helperland\Views\Customer\ServiceHistory.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8af7167bf6d4471bc09b5ff1de4b5d7acdf9e1d4"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Customer_ServiceHistory), @"mvc.1.0.view", @"/Views/Customer/ServiceHistory.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "F:\Helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\Helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "F:\Helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "F:\Helperland\Helperland\Views\Customer\ServiceHistory.cshtml"
using Helperland.Enums;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8af7167bf6d4471bc09b5ff1de4b5d7acdf9e1d4", @"/Views/Customer/ServiceHistory.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"795b70a04da20bf42f4a934e3cf3714a4f4e6a79", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Customer_ServiceHistory : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/cap.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("cap-img float-start"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString("cap image"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "F:\Helperland\Helperland\Views\Customer\ServiceHistory.cshtml"
  
    ViewData["Title"] = "Service History";
    Layout = "~/Views/Shared/_UserLayout.cshtml";
    var compeleted = (int)ServiceRequestStatusEnum.Completed;
    var cancelled = (int)ServiceRequestStatusEnum.Cancelled;

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<!-- Service-history List -->
<div class=""d-flex justify-content-between align-items-center"">
    <h2 class=""title"">Service History</h2>
    <button class=""export-btn"" onclick=""ExportExcel();"">Export</button>
</div>
<div class=""div-table"">
    <table class=""table"" id=""tblServiceHistory"">
        <thead>
            <tr>
                <td>Service Id <img class=""sort-img""");
            BeginWriteAttribute("alt", " alt=\"", 637, "\"", 643, 0);
            EndWriteAttribute();
            WriteLiteral("> </td>\r\n                <td>Service Details <img class=\"sort-img\"");
            BeginWriteAttribute("alt", " alt=\"", 710, "\"", 716, 0);
            EndWriteAttribute();
            WriteLiteral("> </td>\r\n                <td>Service Provider <img class=\"sort-img\"");
            BeginWriteAttribute("alt", " alt=\"", 784, "\"", 790, 0);
            EndWriteAttribute();
            WriteLiteral("> </td>\r\n                <td>Payment <img class=\"sort-img\"");
            BeginWriteAttribute("alt", " alt=\"", 849, "\"", 855, 0);
            EndWriteAttribute();
            WriteLiteral(@"></td>
                <td>Status</td>
                <td>Rate SP</td>
            </tr>
        </thead>
    </table>
</div>


<!-- Service Provider Rating Modal -->
<div class=""modal"" id=""ServiceProviderRatingModel"" tabindex=""-1"" aria-labelledby=""exampleModalLabel"" aria-hidden=""true"">
    <div class=""modal-dialog modal-dialog-centered "">
        <div class=""modal-content"">
            <div class=""modal-body"">
                <button type=""button"" class=""btn-close model-btn-close"" data-bs-dismiss=""modal"" aria-label=""Close""></button>
                <div class=""clearfix mb-3 align-items-center d-flex"">
                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "8af7167bf6d4471bc09b5ff1de4b5d7acdf9e1d46684", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                    <div class=""name-rating"">
                        <label id=""lblServiceProviderName""></label>
                        <div id=""divDisplayRatingServiceProviderInModel"">
                        </div>
                    </div>
                </div>
                <h3 class=""mb-3"">Rate your service provider</h3>
                <hr />

                <input id=""hiddenInputServiceRequestId"" type=""hidden""/>
                <input id=""hiddenInputRatingTo"" type=""hidden""/>

                <div class=""row mb-3"">
                    <div class=""col-sm-12 col-md-4 mb-1 d-flex align-items-center"">
                        On time arrival
                    </div>
                    <div class=""col-sm-12 col-md-8 mb-1"" id=""divRatingOnTimeArrival"">
                    </div>
                </div>
                <div class=""row mb-3"">
                    <div class=""col-sm-12 col-md-4 mb-1 d-flex align-items-center"">
                        Friendly
                    </d");
            WriteLiteral(@"iv>
                    <div class=""col-sm-12 col-md-8 mb-1"" id=""divRatingFriendly"">
                    </div>
                </div>
                <div class=""row mb-3"">
                    <div class=""col-sm-12 col-md-4 mb-1 d-flex align-items-center"">
                        Quality of service
                    </div>
                    <div class=""col-sm-12 col-md-8 mb-1"" id=""divRatingQualityOfService"">
                    </div>
                </div>
                <div class=""row mb-3"">
                    <div class=""col-sm-12 col-md-12 mb-1"">
                        Feedback on service provider
                    </div>
                    <div class=""col-sm-12 col-md-12 mb-1"">
                        <textarea id=""feedback"" class=""form-control"" rows=""3""></textarea>
                    </div>
                </div>

                <div>
                    <button id=""btnRatingSubmit"" class=""btn rounded-pill btn-reschedule"" onclick=""SubmitRating()"">Submit</button>
     ");
            WriteLiteral(@"               <span id=""ratingMessage"" class=""text-danger""></span>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Service Detail Modal -->
<div class=""modal"" id=""ServiceDetailModelServiceHistoryPage"" tabindex=""-1"" aria-labelledby=""exampleModalLabel"" aria-hidden=""true"">
    <div class=""modal-dialog modal-dialog-centered modal-lg"">
        <div class=""modal-content"">
            <div class=""modal-body"">
                <button type=""button"" class=""btn-close model-btn-close"" data-bs-dismiss=""modal"" aria-label=""Close""></button>
                <h3 class=""mb-3"">Service Details</h3>
                <span class=""text service-date""><label id=""lblServiceRequestDateTime""></label></span><br>
                <span class=""text""><b>Duration:</b> <label id=""lblDuration""></label> Hrs</span>
                <hr />
                <span class=""text""><b>Service Id:</b> <label id=""lblServiceRequestId""></label></span><br>
                <span class=""text""><b>Extras:</b> <label");
            WriteLiteral(@" id=""lblExtraServices"" class=""d-inline""></label></span><br>
                <span class=""text""><b>Net Amount :</b> <label id=""lblTotalAmount"" class=""primary fs-20 ms-3""></label></span>
                <hr />
                <span class=""text""><b>Service Address:</b> <label id=""lblServiceAddress""></label></span><br>
                <span class=""text""><b>Phone:</b> <label id=""lblPhone""></label></span><br>
                <span class=""text""><b>Email:</b> <label id=""lblEmail""></label></span>
                <hr />
                <span class=""text""><b>Comments:</b> <label id=""lblComments""></label></span><br>
                <span class=""text""><label id=""lblHasPet""></label></span>
            </div>
        </div>
    </div>
</div>

");
            DefineSection("UserLayoutScripts", async() => {
                WriteLiteral(@"
    <script type=""text/javascript"">
        var table;

        $(document).ready(function () {
            $('#tblServiceHistory').on('draw.dt', function () { //every time ajax call, this code execute
                $("".spRating"").rating({
                    min: 0,
                    max: 5,
                    step: 0.1,
                    size: ""xs"",
                    stars: ""5"",
                    showClear: false,
                    readonly: true,
                    starCaptions: function (val) {
                        return val;
                    },
                    starCaptionClasses: function () {
                        return ""fs-16"";
                    },
                });
            });

            table = $('#tblServiceHistory').DataTable({
                paging: true,
                pagingType: ""full_numbers"",
                processing: true,
                serverSide: true,
                filter: true,
                ordering: true,
  ");
                WriteLiteral(@"              searching: false,
                info: true,
                ajax: {
                    url: ""/Customer/GetServiceRequestHistoryList"",
                    type: ""POST"",
                    datatype: ""json"",
                    dataSrc: 'data'
                },
                oLanguage: {
                    sInfo: ""Total Records: _TOTAL_"",
                    sProcessing: ""<div id='preloader'><div id='loader'></div></div>""
                },
                columnDefs: [
                    { ""orderable"": false, ""targets"": 4 },
                    { ""orderable"": false, ""targets"": 5 }
                ],
                columns: [
                    {
                        data: 'serviceRequestId',
                        name: 'ServiceRequestId',
                    },
                    {
                        name: 'ServiceDateTime',
                        render: function (data, type, row) {
                            return ""<img src='../images/calendar2.pn");
                WriteLiteral(@"g' class='me-1' alt='calenderIcon'><span> "" +
                                ServiceStartDate(row.serviceStartDate) + ""</span ><br> "" +
                                ""<img src='../images/layer-14.png' class='me-1' alt='clockIcon'>"" +
                                ServiceTime(row.serviceStartDate, 0) + "" - "" +
                                ServiceTime(row.serviceStartDate, row.serviceHours);
                        }
                    },
                    {
                        name: 'ServiceProvider',
                        render: function (data, type, row) {

                            if (row.user == null) {
                                return """";
                            }
                            else {
                                var totalRating = 0;
                                var spRating = 0;
                                var count = 0;

                                row.ratings.forEach(function (element) {
                                    t");
                WriteLiteral(@"otalRating = totalRating + element.ratings;
                                    count = count + 1;
                                });


                                if (count == 0) {
                                    return ""<div><img src='../images/cap.png' class='cap-img float-start' alt='cap image'> "" +
                                        ""<div class='name-rating'> <label>"" + row.user.firstName + "" "" + row.user.lastName + ""</label> <div> "" + ""Not Rated"";
                                }

                                spRating = (totalRating / count);

                                var spRatingRounded = Math.round(spRating * 10) / 10;

                                return ""<div><img src='../images/cap.png' class='cap-img float-start' alt='cap image'> "" +
                                    ""<div class='name-rating'> <label>"" + row.user.firstName + "" "" + row.user.lastName + ""</label> <div> "" +
                                    ""<input id='sprating_"" + row.user.userId + ""_"" ");
                WriteLiteral(@"+ row.serviceRequestId + ""' class='spRating' value='"" + spRatingRounded + ""' type='text' title='' hidden>"";
                            }
                        }
                    },
                    {
                        name: 'TotalCost',
                        render: function (data, type, row) {
                            return ""<span class='money'>"" + row.totalCost + ""$</span>"";
                        }
                    },
                    {
                        render: function (data, type, row) {
                            if (row.status == ");
#nullable restore
#line 217 "F:\Helperland\Helperland\Views\Customer\ServiceHistory.cshtml"
                                         Write(compeleted);

#line default
#line hidden
#nullable disable
                WriteLiteral(") {\r\n                                return \"<span class=\'compeleted-btn\'>Completed</span>\";\r\n                            }\r\n                            else if (row.status == ");
#nullable restore
#line 220 "F:\Helperland\Helperland\Views\Customer\ServiceHistory.cshtml"
                                              Write(cancelled);

#line default
#line hidden
#nullable disable
                WriteLiteral(@") {
                                return ""<span class='cancelled-btn'>Cancelled</span>"";
                            }
                            else {
                                return """";
                            }
                        }
                    },
                    {
                        render: function (data, type, row) {
                            
                            return ""<button class='btn rounded-pill btn-reschedule p-1 fs-14' onclick='OpenServiceProviderRatingModel("" + row.serviceRequestId + "")' "" + ((row.status == ");
#nullable restore
#line 231 "F:\Helperland\Helperland\Views\Customer\ServiceHistory.cshtml"
                                                                                                                                                                                    Write(cancelled);

#line default
#line hidden
#nullable disable
                WriteLiteral(@") ? ""disabled"" : """"/*(customerRated == true ? ""disabled"" : """" )*/) + "" >Rate SP</button>"";
                        }
                    }
                ],
                dom: '<""top"">rt<""bottom""lip><""clear"">',
                responsive: true,
                order: [[0, ""desc""]]
            });

            //click on row column
            $('#tblServiceHistory tbody').on('click', 'td', function () {

                if ($(this).index() == 5) { // prevent click on 4th column
                    return;
                }

                var rowData = table.row(this).data();

                $('#preloader').removeClass(""d-none"");

                $.ajax({
                    url: '/Customer/GetServiceRequest',
                    type: 'post',
                    dataType: 'json',
                    data: { ""serviceRequestId"": rowData.serviceRequestId },
                    success: function (resp) {

                        $('#preloader').addClass(""d-none"");

             ");
                WriteLiteral(@"           $(""#lblServiceRequestDateTime"").html(""<b>"" + ServiceStartDate(resp.data.serviceStartDate) + ""</b> "" +
                            ""<b>"" + ServiceTime(resp.data.serviceStartDate, 0) + ""-"" +
                            ServiceTime(resp.data.serviceStartDate, resp.data.serviceHours) + ""</b>"");
                        $(""#lblDuration"").text(resp.data.serviceHours);
                        $(""#lblServiceRequestId"").text(resp.data.serviceRequestId);
                        $(""#lblExtraServices"").text(resp.extraServiceRequest);
                        $(""#lblTotalAmount"").html(""<b>"" + resp.data.totalCost + "" $</b>"");

                        var serviceAddress = resp.data.serviceRequestAddress[0];

                        $(""#lblServiceAddress"").text(serviceAddress.addressLine1 + serviceAddress.addressLine2 + "", "" + serviceAddress.postalCode + serviceAddress.city)
                        $(""#lblPhone"").text(serviceAddress.mobile);   // User Or ServiceAddress
                        $(""#lblEmai");
                WriteLiteral(@"l"").text(resp.data.user.email);

                        $(""#lblComments"").text(resp.data.comments);

                        if (resp.data.hasPets == false) {
                            $(""#lblHasPet"").html(""<img src='../images/not-included.png'/> I don't have pets at home"");
                        }
                        else {
                            $(""#lblHasPet"").html(""<img src='../images/included.png'/> I have pets at home"");
                        }

                        $('#ServiceDetailModelServiceHistoryPage').modal({
                            backdrop: 'static',
                            keyboard: false
                        });
                        $('#ServiceDetailModelServiceHistoryPage').modal('show');
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            });

        });


     

        function OpenServiceProviderRatingModel(serviceReq");
                WriteLiteral(@"uestId) {

            $('#preloader').removeClass(""d-none"");

            $.ajax({
                url: '/Customer/GetServiceProvicerRatingFromServiceRequest',
                type: 'post',
                dataType: 'json',
                data: { ""serviceRequestId"": serviceRequestId },
                success: function (resp) {

                    $('#preloader').addClass(""d-none"");

                    $(""#lblServiceProviderName"").html(resp.result.serviceProvider.firstName + "" "" + resp.result.serviceProvider.lastName);

                    if (resp.result.serviceProviderRating == 0) {
                        $(""#divDisplayRatingServiceProviderInModel"").html(""Not Rated"");
                    }
                    else {
                        $(""#divDisplayRatingServiceProviderInModel"").html(""<input id='spratingInModel_"" + resp.result.ratingId + ""' class='spRatingInModel' value='"" + resp.result.serviceProviderRating + ""' type='text' title='' hidden>"");

                        $("".spR");
                WriteLiteral(@"atingInModel"").rating({
                            min: 0,
                            max: 5,
                            step: 0.1,
                            size: ""xs"",
                            stars: ""5"",
                            showClear: false,
                            readonly: true,
                            starCaptions: function (val) {
                                return val;
                            },
                            starCaptionClasses: function () {
                                return ""fs-16"";
                            },
                        });
                    }

                    $(""#hiddenInputServiceRequestId"").val(resp.result.serviceRequestId);
                    $(""#hiddenInputRatingTo"").val(resp.result.serviceProvider.userId);

                    if (resp.result.ratingId == 0) {
                        $(""#divRatingOnTimeArrival"").html(""<input id='RatingOnTimeArrival' class='rating-serviceProvider' value='"" + 0 + ""' ty");
                WriteLiteral(@"pe='text' title='' hidden>"")
                        $(""#divRatingFriendly"").html(""<input id='RatingFriendly' class='rating-serviceProvider' value='"" + 0 + ""' type='text' title='' hidden>"")
                        $(""#divRatingQualityOfService"").html(""<input id='RatingQualityOfService' class='rating-serviceProvider' value='"" + 0 + ""' type='text' title='' hidden>"")

                        $("".rating-serviceProvider"").rating({
                            min: 0,
                            max: 5,
                            step: 0.5,
                            size: ""sm"",
                            stars: ""5"",
                            showClear: false,
                            showCaption: false
                        });

                        $(""#feedback"").val("""");
                        $(""#feedback"").removeAttr(""disabled"");

                        $(""#btnRatingSubmit"").removeClass(""d-none"");
                        $(""#ratingMessage"").html("""");
                    }
    ");
                WriteLiteral(@"                else {

                        $(""#divRatingOnTimeArrival"").html(""<input id='RatingOnTimeArrival' class='rating-serviceProvider-disabled' value='"" + parseFloat(resp.result.onTimeArrival) + ""' type='text' title='' hidden>"")
                        $(""#divRatingFriendly"").html(""<input id='RatingFriendly' class='rating-serviceProvider-disabled' value='"" + parseFloat(resp.result.friendly) + ""' type='text' title='' hidden>"")
                        $(""#divRatingQualityOfService"").html(""<input id='RatingQualityOfService' class='rating-serviceProvider-disabled' value='"" + parseFloat(resp.result.qualityOfService) + ""' type='text' title='' hidden>"")

                        $("".rating-serviceProvider-disabled"").rating({
                            min: 0,
                            max: 5,
                            step: 0.5,
                            size: ""sm"",
                            stars: ""5"",
                            showClear: false,
                            readonly");
                WriteLiteral(@": true,
                            showCaption: false
                        });

                        $(""#feedback"").val(resp.result.comments);
                        $(""#feedback"").attr(""disabled"", ""disabled"");

                        $(""#btnRatingSubmit"").addClass(""d-none"");
                        $(""#ratingMessage"").html(""You have already rated on "" + ServiceStartDate(resp.result.ratingDate) + "" "" + ServiceTime(resp.result.ratingDate, 0)) + ""."";
                    }

                    $('#ServiceProviderRatingModel').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $('#ServiceProviderRatingModel').modal('show');
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }

        function SubmitRating() {

            var rating = {};

            rating.serviceRequestId = $(""#hiddenInputServiceRequestI");
                WriteLiteral(@"d"").val();
            rating.ratingTo = parseInt($(""#hiddenInputRatingTo"").val());
            rating.OnTimeArrival = $(""#RatingOnTimeArrival"").val();
            rating.Friendly = $(""#RatingFriendly"").val();
            rating.QualityOfService = $(""#RatingQualityOfService"").val();

            var totalRatingSp = (parseFloat(rating.OnTimeArrival) + parseFloat(rating.Friendly) + parseFloat(rating.QualityOfService)) / 3.0;

            rating.ratings = Math.round(totalRatingSp * 10) / 10;

            rating.comments = $(""#feedback"").val();

            $('#preloader').removeClass(""d-none"");

            $.ajax({
                url: '/Customer/SubmitRatingFromCustomer',
                type: 'post',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(rating),
                success: function (resp) {

                    $('#preloader').addClass(""d-none"");

                    $('#ServiceProviderRatingModel').modal('");
                WriteLiteral(@"hide');

                    $('#tblServiceHistory').DataTable().ajax.reload();
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }

           function ExportExcel() {
            var tblData = table.data();
            let csvContent = ""data:text/csv;charset=utf-8,"";

            csvContent += ""Service Id, Service Details, Service Provider, Payment, Status \n"";

            for (let i = 0; i < tblData.length; i++) {
                csvContent += tblData[i].serviceRequestId + "","" +
                    ServiceStartDate(tblData[i].serviceStartDate) + "" "" + ServiceTime(tblData[i].serviceStartDate, 0) + "" - "" +
                    ServiceTime(tblData[i].serviceStartDate, tblData[i].serviceHours) + "","";

                  
                
                if (tblData[i].user == null) {
                    csvContent += "","";
                }
                else {
                    csvContent += t");
                WriteLiteral("blData[i].user.firstName + \" \" + tblData[i].user.lastName + \",\";\r\n                }\r\n\r\n                csvContent += tblData[i].totalCost + \"$,\";\r\n\r\n                if (tblData[i].status == ");
#nullable restore
#line 458 "F:\Helperland\Helperland\Views\Customer\ServiceHistory.cshtml"
                                    Write(compeleted);

#line default
#line hidden
#nullable disable
                WriteLiteral(") {\r\n                    csvContent += \"Completed\\n\";\r\n                }\r\n                else if (tblData[i].status == ");
#nullable restore
#line 461 "F:\Helperland\Helperland\Views\Customer\ServiceHistory.cshtml"
                                         Write(cancelled);

#line default
#line hidden
#nullable disable
                WriteLiteral(@") {
                    csvContent += ""Cancelled\n"";
                }
                else {
                    csvContent += ""\n"";
                }
            }

            var encodedUri = encodeURI(csvContent);
            var link = document.createElement(""a"");
            link.setAttribute(""href"", encodedUri);
            link.setAttribute(""download"", ""serviceRequestHistory.csv"");
            document.body.appendChild(link); // Required for FF
            link.click();
        }

    </script>
");
            }
            );
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
