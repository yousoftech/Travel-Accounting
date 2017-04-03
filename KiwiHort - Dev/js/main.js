/* 
    Title: main.js
    Location: js/main.js
*/



//Global Variables
var startDateEndDate = ['', ''];
var navState = 0;
var detectBroswer = 0;










function msieversion() {

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");
    if (detectBroswer == 1) {
        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
            alert('Using IE');
        }
        else {
            alert('otherbrowser');
        }
    }
    return false;
}



msieversion();




function updateDate() {
    var date1 = $('#dateInput1').val().split('-');
    var year1 = date1[0];
    var month1 = date1[1];
    var day1 = date1[2];
    //alert(day1 + " " + month1 + " " + year1);
    var date2 = $('#dateInput2').val().split('-');
    var year2 = date1[0];
    var month2 = date1[1];
    var day2 = date1[2];


    startDateEndDate[0] = date1 + "/" + month1 + "/" + year1;
    startDateEndDate[1] = date2 + "/" + month2 + "/" + year2;
    checkDays();

}




function closeAllModals() {
    var allModals = ['loginModal', 'signupModal', 'forgotPasswordModal', 'employeeInfoModal'];
    modalBackground(false);

    for (i = 0; i < allModals.length; i++) {

        if (document.getElementById(allModals[i]) !== null) {
            document.getElementById(allModals[i]).style.opacity = 0;
            document.getElementById(allModals[i]).style.marginTop = '-100vh';
            document.getElementById(allModals[i]).style['-webkit-transition-duration'] = '1.2s';
        }
    }
}

function modalBackground(show) {
    if (show == true) {
        document.getElementById('modalBackground').style.opacity = 0.9;
        document.getElementById('modalBackground').style.zIndex = 2;
        document.getElementById('modalBackground').style.display = 'block';
    } else if (show == false) {
        document.getElementById('modalBackground').style.opacity = 0;
        document.getElementById('modalBackground').style.zIndex = -10;
        document.getElementById('modalBackground').style.display = 'none';
    }
}

function modal(modal, show, speed) {
    if (show == true) {
        modalBackground(true);

        document.getElementById(modal).style.display = 'block';
        //document.getElementById('#employeeInfoModal').style.display = 'block';

        setTimeout(function () {
            document.getElementById(modal).style.opacity = 1;
            document.getElementById(modal).style.marginTop = '3vh';
            document.getElementById(modal).style['-webkit-transition-duration'] = speed;
        }, 100);

    } else if (show == false) {
        modalBackground(false);

        document.getElementById(modal).style.display = 'none';
        document.getElementById('#employeeInfoModal').style.display = 'none';

        setTimeout(function () {
            document.getElementById(modal).style.opacity = 0;
            document.getElementById(modal).style.marginTop = '-100vh';
            document.getElementById(modal).style['-webkit-transition-duration'] = speed;
        }, 100);
    }
}

function clearAllDays() {
    var days = ['#mondayForm', '#tuesdayForm', '#wednesdayForm', '#thursdayForm', '#fridayForm', '#saturdayForm', '#sundayForm'];

    for (i = 0; i < days.length() ; i++) {
        $(days[i]).css('display', 'none');
    }
}

function changeDayTo(day) {
    //clearAllDays();
    $('#mondayForm').css({ display: 'none' });
    $('#tuesdayForm').css({ display: 'none' });
    $('#wednesdayForm').css({ display: 'none' });
    $('#thursdayForm').css({ display: 'none' });
    $('#fridayForm').css({ display: 'none' });
    $('#saturdayForm').css({ display: 'none' });
    $('#sundayForm').css({ display: 'none' });
    $(day).css({ display: 'block' });
}

function nextDay() {
    var days = ['#mondayForm', '#tuesdayForm', '#wednesdayForm', '#thursdayForm', '#fridayForm', '#saturdayForm', '#sundayForm'];

    for (i = 0; i < days.length; i++) {
        if ($(days[i]).css('display') == 'block') {
            if (days[i] != '#sundayForm') {
                changeDayTo(days[i + 1]);
            } else {
                changeDayTo(days[i - 6]);
            }
            break;
        }
    }
}

function prevDay() {
    var days = ['#mondayForm', '#tuesdayForm', '#wednesdayForm', '#thursdayForm', '#fridayForm', '#saturdayForm', '#sundayForm'];

    for (i = 7; i > 0; i--) {
        if ($(days[i]).css('display') == 'block') {
            if (days[i] == '#mondayForm') {
                changeDayTo(days[i + 6]);
            } else {
                changeDayTo(days[i - 1]);
            }
            break;
        }
    }
}

function closeNav() {
    $('.responsiveNav').animate({ 'height': '-100px' }, 350);
    navState = 0;
}


function scrollTo(x) {
    $(function () {
        if (x == 'top') {
            var goTo = 0;
        } else {
            var goTo = $('.' + x).offset().top - 77;
        }

        closeNav();
        $('html, body').stop().animate({ scrollTop: goTo }, 700);
    });
}




function selectChange(selection, thing) {
    if (selection.value == 'Yes') {
        $('.timeInputWork1').eq(thing).css({ display: 'inline' });
        $('.rightArrowWork').eq(thing).css({ display: 'inline' });
        $('.timeInputWork2').eq(thing).css({ display: 'inline' });
    } else {
        $('.timeInputWork1').eq(thing).css({ display: 'none' });
        $('.rightArrowWork').eq(thing).css({ display: 'none' });
        $('.timeInputWork2').eq(thing).css({ display: 'none' });
    }
}



function validate() {
    var dropDowns = ['dropDownWork', 'dropDownLunch'];

    for (i = 0; i < dropDowns.length; i++) {
        var selectedValue = document.getElementById(dropDowns[i]);
        if (selectedValue.value == 'pleaseSelect') {
            selectedValue.style.color = 'red';
        }
    }
}


function checkDays() {
    dayExtractor(startDateEndDate[0], startDateEndDate[1]);
}


function dayExtractor(startDate, endDate) {

    var days = 0;
    var msPerDay = 1000 * 60 * 60 * 24;

    var nextDay = function (day, dayAfter) {
        return Date.parse(day) + (msPerDay * dayAfter);
    }

    var daysBetween = function (from, to) {
        return ((Date.parse(to) - Date.parse(from)) / msPerDay) - 1;
    }

    var weekday = ["sunday", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday"];

    var getInbetweenWeekdays = function () {
        var weekDays = [];
        for (var i = -1; i <= daysBetween(startDate, endDate) ; i++) {
            weekDays.push(weekday[new Date(nextDay(startDate, i + 1)).getDay()]);

            alert(weekDays[i + 1]);

            $('#' + weekDays[i + 1] + 'Form .timeSection #dropDownWork').val('Yes');
            $('#' + weekDays[i + 1] + 'Form .employeeSingleThing .timeSection:eq(0) .timeInputWork1').css({ display: 'inline' });
            $('#' + weekDays[i + 1] + 'Form .employeeSingleThing .timeSection:eq(0) .arrowRight').css({ display: 'inline' });
            $('#' + weekDays[i + 1] + 'Form .employeeSingleThing .timeSection:eq(0) .timeInputWork2').css({ display: 'inline' });
            $('.employeeSingle .employeeSingleTop .dateInput').eq(0).val(startDateEndDate[0]);
            $('.employeeSingle .employeeSingleTop .dateInput').eq(1).val(startDateEndDate[1]);
        }
        return weekDays, startDate, endDate;
    }

    getInbetweenWeekdays();

}



/*

if (navState == 1) {
    $('#nav').animate({ 'margin-top': 0 +'px' }, 350);
    $('.plusIconLight').css({ WebkitTransform: 'rotate(' + 135 + 'deg)'}); 
    $('.plusIconDark').css({ WebkitTransform: 'rotate(' + 135 + 'deg)'}); 
} else {
    $('#nav').animate({ 'margin-top': -65 +'px' }, 350);
    $('.plusIconBox').animate({ 'margin-top': 0 +'px' }, 350);
    $('.plusIcon').css({ WebkitTransform: 'rotate(' + 0 + 'deg)'});
}


*/


var scrollTop = $('newNav').scrollTop();



function responsiveIcon() {
    if (navState == 0) {
        //$('.responsiveIcon').animate({ 'margin-top': 230 +'px' }, 350);
        //$('.responsiveIcon a').css({color: '#FFF'});
        $('.responsiveNav').animate({ 'height': '470px' }, 350);
        //$('.moveAll').animate({ 'padding-top':'460px' }, 350);
        navState = 1;
    } else {
        //$('.responsiveIcon').animate({ 'margin-top': 0 +'px' }, 350);
        //$('.responsiveIcon a').css({color: '#747474'});
        $('.responsiveNav').animate({ 'height': '0%' }, 350);
        //$('.moveAll').animate({ 'padding-top':'65px' }, 350);
        navState = 0;
    }
}










function validateForm(formName) {
    //    var x = document.forms["loginForm"]["email"].value;
    //    if (x == null || x == "") {
    //        alert("Name must be filled out");
    //        return false;
    //    }

    var container, inputs, index, endValue;
    container = document.getElementById(formName);

    inputs = container.getElementsByTagName('input');

    for (index = 0; index < inputs.length; ++index) {
        if (inputs[index].value != '') {
            //alert((inputs[index].value) + ' Field is filled');
            //return true;
            endValue += 1;
            alert('HAS SOMETHING');
        } else {
            //return false;
            alert('NOTHING');

            endValue -= 1;
        }
    }

    if (endValue == inputs.length - 1) {
        alert('Everything Filled');
    } else {
        alert('Some not filled');
    }

}























