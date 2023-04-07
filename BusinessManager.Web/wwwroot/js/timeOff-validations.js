let halfDayHolder = $('.half-day-holder');

let toDateHolder = $('.to-date-holder');
let toDate = $('#To');
let fromDate = $('#From');

let fileHolder = $('.file-holder-holder');
let externalFile = $('#ExternalFile');

$('#Type').on('change', updateSelectedType);

function updateSelectedType() {
    let selectedVal = $('#Type').val();

    if (selectedVal == "Sick") {
        halfDayHolder.hide();
        $('#IsHalfDay').prop('checked', false);
        toDateHolder.show();

        fileHolder.show();
    } else {
        halfDayHolder.show();
        fileHolder.hide();
    }
}

$('#IsHalfDay').on('click', isHalfDayCheck);

function isHalfDayCheck() {
    if ($('#IsHalfDay').is(":checked")) {
        toDateHolder.hide();
        toDate.val('');
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

toDate.on('blur', validateDateTimes);
fromDate.on('blur', validateDateTimes);

function validateDateTimes() {
    var toDateVal = moment(toDate.val());
    var fromDateVal = moment(fromDate.val());

    if (toDate.is(":visible") && toDateVal.isBefore(fromDateVal)) {
        Swal.fire(
            'Error!',
            "'To Date' cannot be before 'From Date'!",
            'error'
        );

        toDate.val('');
    }
}

$(function () {
    updateSelectedType();
    isHalfDayCheck();
})