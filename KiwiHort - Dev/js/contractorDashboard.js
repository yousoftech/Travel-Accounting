





var notificationWindow = 0;
var accountDropdown = 0;
var notificationModal = 0;
var responsiveNav = 0;


function toggleNotifications() {

    var ease = 'easeOutQuart';

    if (notificationWindow == 0) {
        $('.notification-window').animate({ 'width': '300px' }, 700, ease);
        notificationWindow = 1;
    } else {
        $('.notification-window').animate({ 'width': '0px' }, 700, ease);
        notificationWindow = 0;
    }
}

function toggleAccountDropdown() {

    var browserWidth = $(window).width();

    var ease = 'easeOutQuart';

    if (accountDropdown == 0) {
        $('.account-dropdown').animate({ 'height': '74px' }, 400, ease);
        $('.down-arrow').css('transform', 'rotate(180deg)');

        if (browserWidth < 400) {
            $('.main-content').animate({ 'margin-top': '123px' }, 400, ease);
        }

        accountDropdown = 1;
    } else {
        $('.account-dropdown').animate({ 'height': '-1px' }, 400, ease);
        $('.down-arrow').css('transform', 'rotate(0deg)');

        if (browserWidth < 400) {
            $('.main-content').animate({ 'margin-top': '45px' }, 400, ease);
        }

        accountDropdown = 0;
    }
}

function generateNotifications(amount) {

    var notificationList = document.getElementsByClassName("notification-list");

    for (i = 0; i < amount; i++) {
        notificationList.innerHTML += '<div class="single-notification"><div class="single-notification-image"><img src="../img/notification-icon-alert.png" class="single-notification-icon"></div><div class="single-notification-text"><div class="single-notification-text-top"><h3 style="float: left">Notification Title</h3><h4 style="float: right">20mins ago</h4></div><div class="single-notification-text-bottom"><h3>Notification Description Here</h3></div></div></div>';
    }

}


generateNotifications(3);

function toggleNotificationModal() {

    var ease = 'easeInOutCubic';

    if (notificationModal == 0) {
        $('.notification-modal').animate({ 'margin-top': '35vh' }, 800, ease);
        notificationModal = 1;
    } else {
        $('.notification-modal').animate({ 'margin-top': '-35vh' }, 800, ease);
        notificationModal = 0;
    }
}











function moveIcon(value) {

    $('.nav-active-arrow').show();

    var ease = 'easeInOutCubic';
    var moveVal = [10, 22, 38, 48.4, 84, 80.4];

    $('.nav-active-arrow').animate({ 'margin-top': moveVal[value] + 'vh' }, 0, ease);

    $('.left-nav-list ul li').removeClass('li-active');
    $('.left-nav-list ul li').eq(value).addClass('li-active');

}




function hideIcon() {
    $('.nav-active-arrow').hide();
    $('.left-nav-list ul li').removeClass('li-active');
}










function toggleLeftNav() {

    var ease = 'easeInOutCubic';

    if (responsiveNav == 0) {
        responsiveNav = 1;

        $('.left-nav').animate({ 'width': '180px' }, { duration: 200, queue: false });
        //$('.menu-icon').animate({ 'margin-left': '180px' }, { duration: 200, queue: false });

        $('.overlay').show();


    } else {
        responsiveNav = 0;

        $('.left-nav').animate({ 'width': '0px' }, { duration: 200, queue: false });
        //$('.menu-icon').animate({ 'margin-left': '0px' }, { duration: 200, queue: false });

        $('.overlay').hide();
    }
}
