$(document).ready()
{
    var validProfilePassword = false;
    var validProfileConfirmPassword = false;


    $('#btnValidatePassword').on("click", function () {
        var CurrentPassword = $('#CurrentPassword').val();
        var ControllerName = $('#hdnControllerName').val();
        var ActionUrl = '/' + ControllerName + '/ValidatePassword';
        ///Learner/ValidatePassword
        $.ajax({
            url: ActionUrl,
            contentType: 'application/json',
            cache: false,
            data: { Password: CurrentPassword },
            dataType: "json",
            type: "GET",
            success: function (data) {
                if (data) {
                    $('#NewPasswordBlock').css({ 'display': 'block' });
                    $('#spInvalidPassword').css({ 'display': 'none' });
                }
                else {
                    $('#NewPasswordBlock').css({ 'display': 'none' });
                    $('#spInvalidPassword').css({ 'display': 'block' });
                }
            }
        });
        $('#CurrentPassword').on('click', function () {
            $('#spInvalidPassword').css({ 'display': 'none' });
        })
    });
    function setScreenValidVariables(divId) {
        if (divId[0].getAttribute('id') === 'ProfilePasswordError') {
            validProfilePassword = true;
        }
        else if (divId[0].getAttribute('id') === 'ProfileConfirmPasswordError') {
            validProfileConfirmPassword = true;
        }
    }
    function AddRemoveControlValidations(valid, controlName, DivId) {
        if (valid == 0) {
            controlName.css('border-color', 'red');
        }
        else {
            controlName.css('border-color', '');
            DivId.css({ 'display': 'none' });
            setScreenValidVariables(DivId);
        }
    }

    $('#UserLoginDetails_Password').focusout(function () {
        var controlName = $('#UserLoginDetails_Password');
        var divId = $('#ProfilePasswordError')
        validProfilePassword = false;
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
    $('#UserLoginDetails_ConfirmPassword').focusout(function () {
        var controlName = $('#UserLoginDetails_ConfirmPassword');
        var divId = $('#ProfileConfirmPasswordError')
        validProfileConfirmPassword = false;
        var InvalidConfirmPasswordText = "";
        var ConfrimPassword = $(this).val();
        var PasswordEntered = $('#UserLoginDetails_Password').val();
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


    function IsValidPasswordsEntered() {
        if (!validProfilePassword || !validProfileConfirmPassword) {
            alert('Please Fill the details Correctly')
            return false;
        }
        else {
            return true;
        }
    }
    var LoadVideoPopup = $('.LoadVideoPopupClass');
    for (var i = 0; i < LoadVideoPopup.length; i++) {
        LoadVideoPopup[i].addEventListener("click", function () {
            var FileName = $(this).data('id');
            var TutorialName = $(this).data('tutorialname');
            //var SrcPath = "/UploadedFiles/" + FileName;
            //Commented above SrcPath CloudCode Start
            var SrcPath = $(this).data('cloudsrc');
            //CloudCode End
            $("#modalPlayVideoIdForSRC").html('<source id="srcTest" src="' + SrcPath + '" type="video/mp4"></source>');
            $('#modalPlayVideoTutorialHeader').text(TutorialName);
            $("#PlayVideoModalTest").modal();

        })
    }
    $('#PlayVideoModalTest').on('shown.bs.modal', function () {
        $('#modalPlayVideoIdForSRC')[0].play();
    })
    $('#PlayVideoModalTest').on('hidden.bs.modal', function () {
        $('#modalPlayVideoIdForSRC')[0].pause();
        var video = $("#srcTest").attr("src");
        $("#srcTest").attr("src", "");
        location.reload();
    })

}