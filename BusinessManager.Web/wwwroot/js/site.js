function parseTeamInputs(teamsSelector) {
    var availableTeams = $(teamsSelector).val();

    var teamInputs = {};
    if (!availableTeams.includes("#")) {
        let teamTokens = availableTeams.split("+=+");
        teamInputs[teamTokens[0]] = teamTokens[1];
    } else {
        for (var team of availableTeams.split("#")) {
            let teamTokens = team.split("+=+");

            teamInputs[teamTokens[0]] = teamTokens[1];
        }
    }

    return teamInputs;
}

function startAssigningToTeam(teamInputs, model, endpoint) {
    Swal.fire({
        title: 'Select a team',
        input: 'select',
        inputOptions: teamInputs,
        inputPlaceholder: 'Select a team',
        showCancelButton: true,
        inputValidator: (value) => {

            if (value == "") {
                return Swal.fire(
                    'Error!',
                    'Please choose a team!',
                    'error'
                );
            } else {
                return new Promise((resolve) => {
                    model.teamId = value;
                    $.ajax({
                        type: "POST",
                        url: endpoint,
                        data: model,
                        dataType: "json",
                        success: function (response) {
                            Swal.fire(
                                'Success!',
                                'Successfully assigned to the team!',
                                'success'
                            ).then(() => window.location.reload());
                        },
                        error: function (event, jqxhr, settings, thrownError) {
                            if (event.status != 200) {
                                Swal.fire(
                                    'Error!',
                                    event.responseText.split(': ')[1].split("\r\n")[0],
                                    'error'
                                );
                            }
                        }
                    });
                });
            }
        }
    });
}

$('form input[type="submit"]').on("click", function (e) {
    var inputs = $('form')
        .find('input.form-control')
        .not('.button')
        .not('select');

    inputs.each((i, el) => {
        var t = $(el);
        var elValue = t.val();

        if (elValue && elValue.length < 3) {
            Swal.fire(
                'Error!',
                `${t.attr('Name')} requires at least 3 characters`,
                'error'
            );
            e.preventDefault();
            return false;
        }

        if (elValue && elValue.length > 250) {
            Swal.fire(
                'Error!',
                `${t.attr('Name')} has a maximum of 250 characters`,
                'error'
            );
            e.preventDefault();
            return false;
        }
    });
});