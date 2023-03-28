let halfDayHolder = $('.half-day-holder');

let toDateHolder = $('.to-date-holder');
let toDate = $('#To');

let fileHolder = $('.file-holder-holder');

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
        }
    });

$(function () {
    $('#Type').trigger('change');
    isHalfDayCheck();
})