

function BindSearchGridData(id, columns, url, search, initialstart, initiallength, initialSortCol, initialSortType, callback) {
    $('#' + id + "_NoRecordFound").hide();
    $('.' + id).show();
    ShowLoadingPannel(id)

    if (!columns[0].bVisible) { columns[0].sClass = 'hide'; }

    var options = {};

    options = {
        "bProcessing": true,
        "serverSide": true,
        "sort": true,
        "columns": columns,
        "bDestroy": true,
        "bFilter": false,
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,

        }],
        "language": {
            "paginate": {
                "first": '<i class="fa fa-fast-backward" aria-hidden="true"></i>',
                "previous": '<i class="fa fa-step-backward" aria-hidden="true"></i>',
                "next": '<i class="fa fa-step-forward" aria-hidden="true"></i></i>',
                "last": '<i class="fa fa-fast-forward" aria-hidden="true"></i>'
            }
        },
        "ajax": {
            url: url, // json datasource
            data: search,
            type: "post",  // type of method  , by default would be get
            error: function (e) {  // error handling code
                setTimeout(function () {
                    $("#divLoad").remove();
                    $('#' + id + "_NoRecordFound").show();
                    $('#' + id + "_NoRecordFoundButton").hide();
                    $('#' + id + '_wrapper').css("display", "none");
                    $('#' + id + '_processing').css("display", "none");
                    $('.' + id).hide();
                }, 500);
            }
        },
        "initComplete": function (settings, data) {
            HideLoadingPannel();
        },

        "fnDrawCallback": function () {

            if (this.fnSettings()._iDisplayStart == 0 && this.fnSettings()._iDisplayLength == 10 && this.fnSettings().fnRecordsDisplay() <= this.fnSettings()._iDisplayLength) {

                //-Note : When more then one grid in singal same page that time paging functionality creating conflict output.       
                $('#' + id + '_wrapper .dataTables_paginate').css("display", "none");
                $('#' + id + '_wrapper .dataTables_length').css("display", "none");
                $('#' + id).css("margin-top", "35px");

            }
            else {
                if (this.fnSettings().fnRecordsDisplay() < 11) {
                    $('#' + id + '_wrapper .dataTables_paginate').css("display", "none");
                    $('#' + id + '_wrapper .dataTables_length').css("display", "none");
                }
                else {
                    $('#' + id + '_wrapper .dataTables_paginate').css("display", "block");
                    $('#' + id + '_wrapper .dataTables_length').css("display", "block");
                }
            }

            if (typeof (callback) != 'undefined') {
                callback();
            }
        }
    }


    if (typeof (initialstart) != 'undefined' && parseInt(initialstart) != NaN) {
        options.displayStart = initialstart;
    }
    if (typeof (initiallength) != 'undefined' && parseInt(initiallength) != NaN) {
        options.pageLength = initiallength;
    }
    if (typeof (initialSortCol) != 'undefined' && parseInt(initialSortCol) != NaN) {
        options.order = [[initialSortCol, initialSortType]];
    }

    return $('#' + id).dataTable(options);
}

//function ShowLoadingPannel(id) {
//    if (typeof (id) != 'undefined') {
//        $('#' + id).append('<div class="div-loader" style="display:none" id="divLoad"></div>');
//    }
//    else {
//        $("body").append('<div class="loader" style="display:none" id="divLoad"></div>');
//    }
//    $("#divLoad").show();
//}

//function HideLoadingPannel() {
//    setTimeout(function () { $("#divLoad").remove(); }, 500);
//}

function ShowLoadingPannel(id) {
    $(".preloader").fadeIn();
}

function HideLoadingPannel() {
    $(".preloader").fadeOut(1000);
}

function SetLocalStorageData(data, id) {
    if (typeof (data) != 'undefined' && data != {}) {
        var table = $('#' + id).DataTable();
        var ordering = table.order();
        if (!(typeof (table.page.info()) == 'undefined')) {
            data.InitialStart = table.page.info().start;
            data.InitialLen = table.page.info().length;
        }
        if (table.order() != null && typeof (table.order()) != 'undefined') {
            data.InitialOrderIndex = ordering[0][0];
            data.InitialOrderType = ordering[0][1];
        }
        localStorage.setItem(id, JSON.stringify(data));
    }
};

function GettLocalStorageData(id) {
    var data = localStorage.getItem(id);
    if (data != null && typeof (data) != undefined) {
        return JSON.parse(data);
    }
    return null;
};

function ResetLocalStorage(id) {
    localStorage.removeItem(id);
}

function CheckLocalStorageData(id) {
    var data = localStorage.getItem(id);
    if (data != null || typeof (data) != undefined) {
        return true;
    }
    return false;
};


function SaveOnArchive(data, id, isActive, isNotTochange) {

    var table = $('#' + id).DataTable();
    var ordering = table.order();
    if (table.order() != null && typeof (table.order()) != 'undefined') {
        data.InitialOrderIndex = ordering[0][0];
        data.InitialOrderType = ordering[0][1];
    }
    var info = table.page.info();

    if (isActive == true) {
        data.InitialStart = info.start;
        data.InitialLen = info.length;
    }
    else {

        if (isNotTochange == true) {
            data.InitialStart = info.start;
            data.InitialLen = info.length;
        }
        else {
            if (info.recordsTotal < 12) {
                data.InitialStart = 0;
                data.InitialLen = 10;
            }

            else if (info.page + 1 == info.pages && info.start != 0 && Math.ceil((info.recordsDisplay - 1) / (info.length * info.page)) == 1) {
                data.InitialStart = info.start - info.length;
                data.InitialLen = info.length;
            }
            else {

                data.InitialStart = info.start;
                data.InitialLen = info.length;
            }
        }

    }

    localStorage.setItem(id, JSON.stringify(data));
}

function convertToPrice(str) {
    //var n = Number.parseFloat(str);
    var n = parseFloat(str);
    if(!str || isNaN(n)) return 0;
    return n.toFixed(2);
}

function setLocation(mapDivId, lat, long, callback) {
    if (lat == '') {
        lat = '12.4400805';
    }
    if (long == '') {
        long = '37.3513522'
    }
    var map = $('#' + mapDivId).locationpicker({
        location: {
            latitude: lat,
            longitude: long
        },
        radius: 10,
        zoom: 7,
        scrollwheel: false,
        enableAutocomplete: true,
        enableReverseGeocode: true,
        inputBinding: {
            latitudeInput: $('#Latitude'),
            longitudeInput: $('#Longitude'),
            radiusInput: null,
            locationNameInput: $('#address-in')
        },
        oninitialized: function (component) {
            $('#address-in').val('');
        },
        onchanged: function (currentLocation, radius, isMarkerDropped) {
            var addressComponents = $(this).locationpicker('map').location.addressComponents;
            var add = '';
            add = [
                      (addressComponents.addressLine1 || ''),
                      (addressComponents.district || ''),
                      (addressComponents.city || '')
            ].join(' ');

            $('#address-in').val(add);
            ///add = addressComponents.addressLine1 + ", " + (addressComponents.district || '') + ", " + addressComponents.city;            
            var mapResponse = { lat: currentLocation.latitude, long: currentLocation.longitude, address: add, ZipCode: addressComponents.postalCode }
            callback(mapResponse);
        }
    });


}

function stringToDate(date, format, delimiter) {

    var formatLowerCase = format.toLowerCase();
    var formatItems = formatLowerCase.split(delimiter);
    var dateItems = date.split(delimiter);
    var monthIndex = formatItems.indexOf("mm");
    var dayIndex = formatItems.indexOf("dd");
    var yearIndex = formatItems.indexOf("yyyy");
    var month = parseInt(dateItems[monthIndex]);
    month -= 1;
    var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
    return formatedDate;
}

toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": true,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}


