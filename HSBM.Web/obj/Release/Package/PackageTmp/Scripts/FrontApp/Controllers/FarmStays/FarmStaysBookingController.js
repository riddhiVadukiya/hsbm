angular.module('FrontApp').controller('FarmStaysBookingController', ['$scope', '$compile', '$filter', '$timeout', 'CommonFactory','$sce',
function ($scope, $compile, $filter, $timeout, CommonFactory, $sce) {

    var DetailURL = '/FarmStaysBooking/';
    $scope.Search = {};

    if (FarmStayId != '') {
        $scope.Search.FarmStayId = FarmStayId;
    }
    if (RoomId != '') {
        $scope.Search.RoomId = RoomId;
    }
    if (CheckIn != '') {
        $scope.Search.CheckIn = CheckIn;
    }
    if (CheckOut != '') {
        $scope.Search.CheckOut = CheckOut;
    }
    if (Guests != '') {
        $scope.Search.Guests = Guests;
    }
    if (Child != '') {
        $scope.Search.Child = Child;
    }
    if (RatePlanId != '') {
        $scope.Search.RatePlanId = RatePlanId;
    }
    if (IsSolo) {
        $scope.Search.IsSolo = IsSolo;
    }
  
        var response = CommonFactory.GetAllCountry();
        response.then(function (successdata) {
            $scope.ListofCountry = successdata.data;
        }).catch(function (data, status) {
            console.error('Error', response.status, response.data);
        }).finally(function () {
            console.log("finally finished");
        });
    
        $scope.GetBookingDetail = function () {
        //$scope.Search = {
        //    FarmStayId: $('#FarmStayId').val(),
        //    RoomId: $('#RoomId').val(),
        //    CheckIn: $('#CheckIn').val(),
        //    CheckOut: $('#CheckOut').val(),
        //    Guests: $('#Guests').val(),
        //    IsSolo: $('#IsSolo').val()
        //};
        $.ajax({

            url: DetailURL + 'GetRoomBookingDetail',
            type: 'Post',
            data: {
                SearchFarmStaysRequest: $scope.Search
            },
            success: function (result) {

                if (result != null)
                {
                    $scope.BookingDetail = result;
                    $scope.BookingDetail.DiscountPrice = convertToPrice(result.DiscountPrice);
                    $scope.BookingDetail.Price = convertToPrice(result.Price);
                    $scope.DisplayDiscount = convertToPrice($scope.BookingDetail.Price - $scope.BookingDetail.DiscountPrice);

                    $scope.BookingDiscountPrice = convertToPrice(result.DiscountPrice);
                    $scope.BookingPrice = convertToPrice(result.Price);

                    // $scope.BookingDetail.LeadTraveler = {};
                    
                    if ($scope.BookingDetail.LeadTraveler.IsMale != null) {
                        if (result.LeadTraveler.IsMale == true) {
                            $scope.BookingDetail.LeadTraveler.IsMale = "true";
                        } else if(result.LeadTraveler.IsMale == false) {
                            $scope.BookingDetail.LeadTraveler.IsMale = "false";
                        }
                    }
                    //$scope.BookingDetail.LeadTraveler.IsMale = result.IsMale!=null? ("" + result.IsMale):"";
                    if (result.IsMale == true || result.IsMale == false) {
                        $scope.BookingDetail.IsGenderReadOnly = true;
                    }
                    $scope.BookingDetail.LeadTraveler.GuestCountryId = "0";

                    $scope.$digest();
                }
                else {
                    window.location.href = "/";
                }
            },
            error: function (ex) { window.location.href = "/"; }
        });
    }
    $scope.GetBookingDetail();

    $scope.BookRoom = function () {
        
        var _BookingDetail = angular.copy($scope.BookingDetail);
        _BookingDetail.TermsAndConditions = null;

        $.ajax({

            url: DetailURL + 'BookFarmStayRoom',
            type: 'Post',
            data: {
                BookingResponse: _BookingDetail
            },
            success: function (result) {
                //window.location.href = '/FarmStaysBooking/ThankYou?Id='+result;
                $scope.BookingDetail.OrderID = result;
                $scope.launchBOLT();
            },
            error: function (ex) {
                alert("Error in Booking")
            }
        });
    }

    $scope.ConvertHTML = function (html_code) {
        return $sce.trustAsHtml(html_code);
    }


    $scope.OpenPopup=function(){
        // alert(parseFloat($scope.BookingDetail.DiscountPrice > 0 ? ($scope.BookingDetail.DiscountPrice) : ($scope.BookingDetail.Price)))
   
        bolt.launch({
            key: $scope.BookingDetail.PaymentKey,
            txnid: $scope.BookingDetail.OrderID.toString(),
            hash: $scope.BookingDetail.hash,
            amount: parseFloat($scope.BookingDiscountPrice > 0 ? ($scope.BookingDiscountPrice) : ($scope.BookingPrice)),
            firstname: $scope.BookingDetail.LeadTraveler.GuestFirstName,
            email: $scope.BookingDetail.LeadTraveler.GuestEmail,
            phone: $scope.BookingDetail.LeadTraveler.GuestMobile,
            productinfo: $scope.BookingDetail.FarmStayName,
            udf5: "/FarmStaysBooking/ThankYou",
            surl: "/FarmStaysBooking/ThankYou",
            furl: "/FarmStaysBooking/ThankYou"
        }, {
            responseHandler: function (BOLT) {
                if (BOLT.response.txnStatus != 'CANCEL') {
                    $scope.BookingDetail.GuestFirstName = $scope.BookingDetail.LeadTraveler.GuestFirstName;
                    $scope.BookingDetail.GuestEmail = $scope.BookingDetail.LeadTraveler.GuestEmail;
                    $scope.BookingDetail.Status = BOLT.response.status;
                    $scope.BookingDetail.PayuMoneyId = BOLT.response.payuMoneyId;
                    $scope.BookingDetail.PaymentResponse = JSON.stringify(BOLT.response);
                   // var _BookingDetail = angular.copy($scope.BookingDetail);
                    $scope.BookingDetail.DiscountPrice = $scope.BookingDiscountPrice;
                    $scope.BookingDetail.Price = $scope.BookingPrice;

                    $.ajax({

                        url: DetailURL + 'OrderPayment',
                        type: 'Post',
                        data: JSON.stringify($scope.BookingDetail),
                        cache: false,
                        contentType: "application/json",
                        dataType: 'json',
                        success: function (result) {
                            window.location.href = '/FarmStaysBooking/ThankYou?Id=' + result;
                        },
                        error: function (ex) {
                            alert("Error in Booking")
                        }
                    });
                }
                //if (BOLT.response.txnStatus != 'CANCEL') {
                //    //Salt is passd here for demo purpose only. For practical use keep salt at server side only.
                //    var fr = '<form action=\"' + $('#surl').val() + '\" method=\"post\">' +
                //                '<input type=\"hidden\" name=\"key\" value=\"' + BOLT.response.key + '\" />' +
                //                '<input type=\"hidden\" name=\"salt\" value=\"' + $('#salt').val() + '\" />' +
                //                '<input type=\"hidden\" name=\"txnid\" value=\"' + BOLT.response.txnid + '\" />' +
                //                '<input type=\"hidden\" name=\"amount\" value=\"' + BOLT.response.amount + '\" />' +
                //                '<input type=\"hidden\" name=\"productinfo\" value=\"' + BOLT.response.productinfo + '\" />' +
                //                '<input type=\"hidden\" name=\"firstname\" value=\"' + BOLT.response.firstname + '\" />' +
                //                '<input type=\"hidden\" name=\"email\" value=\"' + BOLT.response.email + '\" />' +
                //                '<input type=\"hidden\" name=\"udf5\" value=\"' + BOLT.response.udf5 + '\" />' +
                //                '<input type=\"hidden\" name=\"mihpayid\" value=\"' + BOLT.response.mihpayid + '\" />' +
                //                '<input type=\"hidden\" name=\"status\" value=\"' + BOLT.response.status + '\" />' +
                //                '<input type=\"hidden\" name=\"hash\" value=\"' + BOLT.response.hash + '\" />' +
                //                '</form>';
                //    alert(fr)
                //    var form = jQuery(fr);
                //    jQuery('#Payment').append(form);
                //    form.submit();
                //}
            },
            catchException: function (BOLT) {
                alert(BOLT.message);
            }

        });
    }

    $scope.launchBOLT = function () {

        $.ajax({
            url: 'FarmStaysBooking/GetHashKey',
            type: 'post',
            data: JSON.stringify({
                key: $scope.BookingDetail.PaymentKey,
                salt: $scope.BookingDetail.PaymentSalt,
                txnid: $scope.BookingDetail.OrderID.toString(),
                amount: parseFloat($scope.BookingDiscountPrice > 0 ? ($scope.BookingDiscountPrice) : ($scope.BookingPrice)),
                pinfo: $scope.BookingDetail.FarmStayName,
                fname: $scope.BookingDetail.LeadTraveler.GuestFirstName,
                email: $scope.BookingDetail.LeadTraveler.GuestEmail,
                mobile: $scope.BookingDetail.LeadTraveler.GuestMobile,
                udf5: "/FarmStaysBooking/ThankYou"
            }),
            contentType: "application/json",
            dataType: 'json',
            success: function (result) {
                if(result != null)
                {
                    $scope.BookingDetail.hash = result;
                    $scope.OpenPopup();
                }
            }
        });
    }

}]);

