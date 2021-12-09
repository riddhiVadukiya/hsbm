//$(function () {
//    var chat = $.connection.AppHub;
//    registerClientMethods(chat)
//    $.connection.hub.start().done(function () {
       
//    });;
//});
//function registerClientMethods(chatHub) {
//    chatHub.client.BroadcastNewRole = function (data) {
//        $.post('/Admin/Common/UpdateRole', { roledata: data }, function (a) {
//            if (a != null && a != '') {
//                //document.cookie = a.name + "=" + a.data;
//                alert('Administrator have changed role settings');
//            }
//        });
//    };
//}