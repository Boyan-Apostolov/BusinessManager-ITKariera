let halfDayHolder = $('.half-day-holder');

let toDateHolder = $('.to-date-holder');
let toDate = $('#To');

let fileHolder = $('.file-holder-holder');
let externalFile = $('#ExternalFile');

$('#Type').on('change',
    function () {
        let selectedVal = $(this).val();

        if (selectedVal == "Sick") {
            halfDayHolder.hide();
            $('#IsHalfDay').prop('checked', false);
            toDateHolder.show();

            fileHolder.show();
        } else {
            halfDayHolder.show();
            fileHolder.hide();
        }
    });

$('#IsHalfDay').on('click', isHalfDayCheck);

function isHalfDayCheck() {
    if ($('#IsHalfDay').is(":checked")) {
        toDateHolder.hide();
    } else {
        toDateHolder.show();
    }
}

$('#submit-btn').on('click',
    function (e) {
        if (!toDate.val() && toDate.is(":visible")) {
            e.preventDefault();
            Swal.fire(
                'Error!',
                "'To date' is required!",
                'error'
            );
        } else if (fileHolder.is(":visible") && !externalFile.val()) {
            e.preventDefault();
            Swal.fire(
                'Error!',
                "'External file' is required!",
                'error'
            );
            return;
        }
    });

$(function () {
    $('#Type').trigger('change');
    isHalfDayCheck();
})