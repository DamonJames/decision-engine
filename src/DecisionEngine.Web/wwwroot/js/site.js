
document.getElementById("submit").onclick = function () {
    firstName = document.getElementById("first-name");
    lastName = document.getElementById("last-name");
    dateOfBirth = document.getElementById("dob");
    submitButton = document.getElementById("submit");

    hideValidation();
    disableInputs();
    statusUpdater(processing);
    validateAndSubmit();
};

var processing = 0;
var accepted = 1;
var declined = 2;
var errored = 3;
var userError = 4;

var statusColour = document.getElementById("status");
var statusText = document.getElementById("status-text");

var firstName;
var lastName;
var dateOfBirth;
var submitButton;

var firstNameValidation = document.getElementById("first-name-validation");
var lastNameValidation = document.getElementById("last-name-validation");
var dobValidation = document.getElementById("dob-validation");

function disableInputs() {
    firstName.disabled = true;
    lastName.disabled = true;
    dateOfBirth.disabled = true;
    submitButton.disabled = true;
}

function enableInputs() {
    firstName.disabled = false;
    lastName.disabled = false;
    dateOfBirth.disabled = false;
    submitButton.disabled = false;
}

function hideValidation() {
    firstNameValidation.innerHTML = "";
    lastNameValidation.innerHTML = "";
    dobValidation.innerHTML = "";
}

function statusUpdater(status) {
    switch (status) {
        case processing:
            statusColour.style.backgroundColor = "#3568ba";
            statusText.innerHTML = "Processing...";
            break;
        case accepted:
            statusColour.style.backgroundColor = "#3cc45a";
            statusText.innerHTML = "Accepted!";
            break;
        case declined:
            statusColour.style.backgroundColor = "#ce2d2d";
            statusText.innerHTML = "Declined";
            break;
        case errored:
            statusColour.style.backgroundColor = "#d87829";
            statusText.innerHTML = "Error";
            break;
        case userError:
            statusColour.style.backgroundColor = "#dbad18";
            statusText.innerHTML = "Input error";
            break;
    }
}

function alerter(data) {
    switch (data) {
        case accepted:
            statusUpdater(accepted);
            alert("Congratulations! You have been accepted");
            break;
        case declined:
            statusUpdater(declined);
            alert("Unfortunately you have been declined");
            break;
        case errored:
            statusUpdater(errored);
            alert("Something went wrong! Please try again");
            break;
        case userError:
            statusUpdater(userError);
            alert("Something is wrong with your submission, please review and try again");
            break;
        default:
            statusUpdater(errored);
            alert("Something went wrong! Please try again");
            break;
    }
}

function validateAndSubmit() {
    var isValid = true;

    if (!firstName.checkValidity() || firstName.value === "") {
        firstNameValidation.innerHTML = "This field is required";
        isValid = false;
    }

    if (!lastName.checkValidity() || lastName.value === "") {
        lastNameValidation.innerHTML = "This field is required";
        isValid = false;
    }

    if (!dateOfBirth.checkValidity() || dateOfBirth.value === "") {
        dobValidation.innerHTML = "This field is required";
        isValid = false;
    }

    if (!isValid) {
        enableInputs();
        statusUpdater(userError);
        return;
    }

    submit();
}

function submit() {
    $.ajax({
        type: "POST",
        url: "Input/SubmitAsync",
        data: {
            firstName: firstName.value,
            lastName: lastName.value,
            dateOfBirth: dateOfBirth.value
        },
        success: function (data) {
            enableInputs();
            alerter(data.status);
        },
        error: function () {
            enableInputs();
            statusUpdater(errored);
            alerter(errored);
        }
    });
}