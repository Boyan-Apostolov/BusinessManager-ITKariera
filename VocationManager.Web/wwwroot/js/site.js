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
                                'User was successfully assigned to the team!',
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