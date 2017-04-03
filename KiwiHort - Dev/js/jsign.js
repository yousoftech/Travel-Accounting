$(function test() {
    var canvas = document.getElementById('thecanvas');

    var image = document.getElementById("thecanvas").toDataURL("image/png");

    image = image.replace('data:image/png;base64,', '');

    $.ajax({
        type: 'POST', url: 'SupervisorHome.aspx/UploadImage', data: '{ "imageData" : "' + image + '" }', contentType: 'application/json; charset=utf-8', dataType: 'json',
        success: function (msg) {

            alert('Image saved successfully!');

        }



    });

});