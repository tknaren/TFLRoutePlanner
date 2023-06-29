// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('#inputStart').bind('keyup', function () {

    // Get the current value of the contents within the text box
    var val = $('#inputStart').val().toUpperCase();

    // Reset the current value to the Upper Case Value
    $('#inputStart').val(val);

});

$('#inputEnd').bind('keyup', function () {

    // Get the current value of the contents within the text box
    var val = $('#inputEnd').val().toUpperCase();

    // Reset the current value to the Upper Case Value
    $('#inputEnd').val(val);

});

$('#inputViaStation').bind('keyup', function () {

    // Get the current value of the contents within the text box
    var val = $('#inputViaStation').val().toUpperCase();

    // Reset the current value to the Upper Case Value
    $('#inputViaStation').val(val);

});

$('#inputExStation').bind('keyup', function () {

    // Get the current value of the contents within the text box
    var val = $('#inputExStation').val().toUpperCase();

    // Reset the current value to the Upper Case Value
    $('#inputExStation').val(val);

});

//$('#frmRoutePlanner').on('submit', function () {
//    var lngtxt = ($(this).find('input[name="txt_jobid[]"]').val()).length;
//    console.log(lngtxt);
//    if (lngtxt == 0) {
//        alert('please enter value');
//        return false;
//    } else {
//        //submit
//    }

//});