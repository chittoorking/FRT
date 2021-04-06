$(document).ready()
{
    //alert('Working')
    var validLearnerFirstName = true;
    var validLearnerLastName = true;
    var validLearnerEmail = false;
    var validLearnerPhone = false;
    var validLearnerSecurityAnswer = true;
    var validLearnerPassword = false;
    var validLearnerConfirmPassword = false;
    var validLearnerSecurityQuestion = false;


    var validTeacherEmail = false;
    var validTeacherPhone = false;

    var validTeacherPassword = false;
    var validTeacherConfirmPassword = false;
    var validTeacherSecurityQuestion = false;

    

    function setScreenValidVariables(divId) {
        if (divId[0].getAttribute('id') === 'LearnerPhoneError') {
            validLearnerPhone = true;
        }
        else if (divId[0].getAttribute('id') === 'LearnerEmailError') {
            validLearnerEmail = true;
        }
        else if (divId[0].getAttribute('id') === 'TeacherEmailError') {
            validTeacherEmail = true;
        }
        else if (divId[0].getAttribute('id') === 'TeacherPhoneError') {
            validTeacherPhone = true;
        }
        else if (divId[0].getAttribute('id') === 'LearnerPasswordError') {
            validLearnerPassword = true;
        }
        else if (divId[0].getAttribute('id') === 'LearnerConfirmPasswordError') {
            validLearnerConfirmPassword = true;
        }
        else if (divId[0].getAttribute('id') === 'LearnerSecurityQuestionError') {
            validLearnerSecurityQuestion = true;
        }
        else if (divId[0].getAttribute('id') === 'TeacherPasswordError') {
            validTeacherPassword = true;
        }
        else if (divId[0].getAttribute('id') === 'TeacherConfirmPasswordError') {
            validTeacherConfirmPassword = true;
        }
        else if (divId[0].getAttribute('id') === 'TeacherSecurityQuestionError') {
            validTeacherSecurityQuestion = true;
        }

        
        
    }

    function ControlValidationPostBack(data, controlName, divId, controlScreenName) {
        if (divId.css('display')=='none') {
            divId.css({ 'display': 'block' });
        }
        ////1 Means Unique, 0 Means Not-Unique, -1 Means Invalid operation
        var valid = 1;
        if (data === 1) {
            valid = 1;
        }
        else if (data === 0) {
            divId.text("The " + controlScreenName +" Already Exists");
            valid = 0;
        }
        else if (data === -1) {
            divId.text("Error in The " + controlScreenName);
            valid = 0;
        }
        AddRemoveControlValidations(valid,controlName,divId);
    }

    function AddRemoveControlValidations(valid,controlName,DivId) {
        if (valid == 0) {
            controlName.css('border-color', 'red');
        }
        else {
            controlName.css('border-color', '');
            DivId.css({ 'display': 'none' });
            setScreenValidVariables(DivId);
        }
    }


    $('#UserLearner_Phone').focusout(function () {
        var controlName = $('#UserLearner_Phone');
        var divId = $('#LearnerPhoneError')
        var controlScreenName = "Phone Number"
        var Phone = $(this).val();
        var valid = 1;
        if (Phone.length > 15 || Phone.length < 7) {
            if (divId.css('display') == 'none') {
                divId.css({ 'display': 'block' });
            }
            divId.text("Invalid "+ controlScreenName + " Lenght");
            valid = 0;
            AddRemoveControlValidations(valid, controlName, divId)
        }
        else {
            $.ajax({
                url: '/Home/IsPhoneNumberUnique',
                contentType: 'application/json',
                cache: false,
                data: { Phone: Phone },
                dataType: "json",
                type: "GET",
                success: function (data) {
                    ControlValidationPostBack(data, controlName, divId, controlScreenName);
                }
            });
        }
    });
    $('#UserLearner_Email').focusout(function () {
        var controlName = $('#UserLearner_Email');
        var divId = $('#LearnerEmailError')
        var controlScreenName = "Email Address"
        var Email = $(this).val();
        var valid = 1;
        if (!Email.includes("@")) {
            if (divId.css('display') == 'none') {
                divId.css({ 'display': 'block' });
            }
            divId.text("Invalid " + controlScreenName);
            valid = 0;
            AddRemoveControlValidations(valid, controlName, divId)
        }
        else {
            $.ajax({
                url: '/Home/IsEmailAddressUnique',
                contentType: 'application/json',
                cache: false,
                data: { EmailAddress: Email },
                dataType: "json",
                type: "GET",
                success: function (data) {
                    ControlValidationPostBack(data, controlName, divId, controlScreenName);
                }
            });
        }
    });
    $('#UserLearnerLoginDetails_Password').focusout(function () {
        var controlName = $('#UserLearnerLoginDetails_Password');
        var divId = $('#LearnerPasswordError')        
        var InvalidPasswordText = "";
        var Password = $(this).val();
        valid = 1;
        if (Password.length < 8) {
            InvalidPasswordText = "Password Length Should be minimum 8 Characters";
            valid = 0;
        }
        else if (!((Password.match(/[a-z]/i)) && (Password.match(/\d+/)))) {
            valid = 0;
            InvalidPasswordText = "Password Should Contain at least One Digit and One Alphabet";
        }
        if (valid == 0) {
            if (divId.css('display') == 'none') {
                divId.css({ 'display': 'block' });
            }
            divId.text(InvalidPasswordText);
            AddRemoveControlValidations(valid, controlName, divId)
        }
        else {
            AddRemoveControlValidations(valid, controlName, divId)
        }
    });
    $('#UserLearnerLoginDetails_ConfirmPassword').focusout(function () {
        var controlName = $('#UserLearnerLoginDetails_ConfirmPassword');
        var divId = $('#LearnerConfirmPasswordError')       
        var InvalidConfirmPasswordText = "";
        var ConfrimPassword = $(this).val();
        var PasswordEntered = $('#UserLearnerLoginDetails_Password').val();
        valid = 1;
        if (PasswordEntered !== ConfrimPassword) {
            InvalidConfirmPasswordText = "Password and Confirm Password does not match";
            valid = 0;
        }
        if (valid == 0) {
            if (divId.css('display') == 'none') {
                divId.css({ 'display': 'block' });
            }
            divId.text(InvalidConfirmPasswordText);
            AddRemoveControlValidations(valid, controlName, divId)
        }
        else {
            AddRemoveControlValidations(valid, controlName, divId)
        }
    });

    $('#UserLearnerLoginDetails_SecurityQuestion').focusout(function () {
        var controlName = $('#UserLearnerLoginDetails_SecurityQuestion');
        var divId = $('#LearnerSecurityQuestionError')
        var InvalidSecurityQuestionText = "";
        var SecurityQuestion = $(this).val();
        valid = 1;
        if (SecurityQuestion==null) {
            InvalidSecurityQuestionText = "Please Select a Security Question";
            valid = 0;
        }
        if (valid == 0) {
            if (divId.css('display') == 'none') {
                divId.css({ 'display': 'block' });
            }
            divId.text(InvalidSecurityQuestionText);
            AddRemoveControlValidations(valid, controlName, divId)
        }
        else {
            AddRemoveControlValidations(valid, controlName, divId)
        }
    });


    function IsValidLearnerDetails() {
        var validNameExpressionFalse = (validLearnerFirstName == false || validLearnerLastName == false);
        var validLearnerEmailPhoneSecurityAnsExpressionFalse = (validLearnerEmail == false || validLearnerPhone == false || validLearnerSecurityAnswer == false);

        if (validNameExpressionFalse || validLearnerEmailPhoneSecurityAnsExpressionFalse) {
            alert('Please Fill the details Correctly')
            return false;
        }
        else {
            return true;
        }
    }

    $('#UserTeacher_Email').focusout(function () {
        var controlName = $('#UserTeacher_Email');
        var divId = $('#TeacherEmailError')
        var controlScreenName = "Email Address"
        var Email = $(this).val();
        var valid = 1;
        if (!Email.includes("@")) {
            if (divId.css('display') == 'none') {
                divId.css({ 'display': 'block' });
            }
            divId.text("Invalid " + controlScreenName);
            valid = 0;
            AddRemoveControlValidations(valid, controlName, divId)
        }
        else {
            $.ajax({
                url: '/Home/IsEmailAddressUnique',
                contentType: 'application/json',
                cache: false,
                data: { EmailAddress: Email },
                dataType: "json",
                type: "GET",
                success: function (data) {
                    ControlValidationPostBack(data, controlName, divId, controlScreenName);
                }
            });
        }
    });

    $('#UserTeacher_Phone').focusout(function () {
        var controlName = $('#UserTeacher_Phone');
        var divId = $('#TeacherPhoneError')
        var controlScreenName = "Phone Number"
        var Phone = $(this).val();
        var valid = 1;
        if (Phone.length > 15 || Phone.length < 7) {
            if (divId.css('display') == 'none') {
                divId.css({ 'display': 'block' });
            }
            divId.text("Invalid " + controlScreenName + " Lenght");
            valid = 0;
            AddRemoveControlValidations(valid, controlName, divId)
        }
        else {
            $.ajax({
                url: '/Home/IsPhoneNumberUnique',
                contentType: 'application/json',
                cache: false,
                data: { Phone: Phone },
                dataType: "json",
                type: "GET",
                success: function (data) {
                    ControlValidationPostBack(data, controlName, divId, controlScreenName);
                }
            });
        }
    });
    $('#UserTeacherLoginDetails_Password').focusout(function () {
        var controlName = $('#UserTeacherLoginDetails_Password');
        var divId = $('#TeacherPasswordError')
        var InvalidPasswordText = "";
        var Password = $(this).val();
        valid = 1;
        if (Password.length < 8) {
            InvalidPasswordText = "Password Length Should be minimum 8 Characters";
            valid = 0;
        }
        else if (!((Password.match(/[a-z]/i)) && (Password.match(/\d+/)))) {
            valid = 0;
            InvalidPasswordText = "Password Contain at least One Digit and One Alphabet";
        }
        if (valid == 0) {
            if (divId.css('display') == 'none') {
                divId.css({ 'display': 'block' });
            }
            divId.text(InvalidPasswordText);
            AddRemoveControlValidations(valid, controlName, divId)
        }
        else {
            AddRemoveControlValidations(valid, controlName, divId)
        }
    });
    $('#UserTeacherLoginDetails_ConfirmPassword').focusout(function () {
        var controlName = $('#UserTeacherLoginDetails_ConfirmPassword');
        var divId = $('#TeacherConfirmPasswordError')
        var InvalidConfirmPasswordText = "";
        var ConfrimPassword = $(this).val();
        var PasswordEntered = $('#UserTeacherLoginDetails_Password').val();
        valid = 1;
        if (PasswordEntered !== ConfrimPassword) {
            InvalidConfirmPasswordText = "Password and Confirm Password does not match";
            valid = 0;
        }
        if (valid == 0) {
            if (divId.css('display') == 'none') {
                divId.css({ 'display': 'block' });
            }
            divId.text(InvalidConfirmPasswordText);
            AddRemoveControlValidations(valid, controlName, divId)
        }
        else {
            AddRemoveControlValidations(valid, controlName, divId)
        }
    });

    $('#UserTeacherLoginDetails_SecurityQuestion').focusout(function () {
        var controlName = $('#UserTeacherLoginDetails_SecurityQuestion');
        var divId = $('#TeacherSecurityQuestionError')
        var InvalidSecurityQuestionText = "";
        var SecurityQuestion = $(this).val();
        valid = 1;
        if (SecurityQuestion == null) {
            InvalidSecurityQuestionText = "Please Select a Security Question";
            valid = 0;
        }
        if (valid == 0) {
            if (divId.css('display') == 'none') {
                divId.css({ 'display': 'block' });
            }
            divId.text(InvalidSecurityQuestionText);
            AddRemoveControlValidations(valid, controlName, divId)
        }
        else {
            AddRemoveControlValidations(valid, controlName, divId)
        }
    });

    $("a[id *= 'DonationAmountButton']").on('click', function () {
        var sel = $(this).data('title');
        var tog = $(this).data('toggle');
        $('#' + tog).prop('value', sel);

        $('a[data-toggle="' + tog + '"]').not('[data-title="' + sel + '"]').removeClass('activeDonationAmount').addClass('notActiveDonationAmount');
        $('a[data-toggle="' + tog + '"][data-title="' + sel + '"]').removeClass('notActiveDonationAmount').addClass('activeDonationAmount');
        if (sel !== "UserOtherDonationBtn") {
            $('#UserOtherDonationAmount').css({ 'display': 'none' });
        }
    })

    $('#DonationAmountButtonOther').on('click', function () {
        $('#UserOtherDonationAmount').css({ 'display': 'block' });
    })
}