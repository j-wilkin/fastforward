var fastforward = fastforward || {};

fastforward.fastForwardMe = function () {
    var $idCookie = $.cookie('careerId');
    if ($idCookie != undefined) {
        window.location = "/Home/Timeline?careerId=" + $idCookie;
    } else {
        window.location = "/Home/Index";
    }
};

fastforward.initActiveMenuItem = function(tab) {

    $("#Home").removeClass("active");
    $("#Survey").removeClass("active");
    $("#Result").removeClass("active");
    //$("#Timeline").removeClass("active");
    //$('#Resources').removeClass("active");
    $("#Network").removeClass("active");
    $("#Search").removeClass("active");
    $("#Learn").removeClass("active");

    switch (tab.toLowerCase()) {
    case 'home':
        $("#Home").addClass("active");
        break;
    case 'survey':
        $("#Survey").addClass("active");
        break;
    case 'results':
        $("#Result").addClass("active");
        break;
    //case 'timeline':
    //    $("#Timeline").addClass("active");
    //    break;
    //case 'resources':
    //    $('#Resources').addClass("active");
    //    break;
    case 'network':
        $("#Network").addClass("active");
        break;
    case 'search':
        $("#Search").addClass("active");
        break;
    case 'learn':
        $("#Learn").addClass("active");
        break;
    }
};

$(function() {

    // Disable clicks on active menu items
    $('body').on('click', 'li.active a', function (e) {
        return false;
    });
    
    //Loads all videos from their thumbnail
    $('.videoThumb').click(function () {
        var videoId = $(this).attr('id');
        $(this).addClass('hide');
        $('.' + videoId).removeClass('hide');
    });
    
});
