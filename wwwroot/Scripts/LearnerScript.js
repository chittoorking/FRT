$(document).ready()
{
    var hdnIsLearnerProfileComplete = $('#hdnIsLearnerProfileComplete').val();

    if (hdnIsLearnerProfileComplete && hdnIsLearnerProfileComplete === "False") {
        $("#IncompleteProfileModal").modal();
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

    $('#btnLearnerCancelEnrollment').click(function () {
        if (confirm('Are you sure you want to cancel the enrollment?')) {
            var url = $(this).attr('href');
            $('#content').load(url);
        }

    });
    var clsCancelEnrollment = $('.clsLearnerCancelEnrollment');
    for (var i = 0; i < clsCancelEnrollment.length; i++) {
        clsCancelEnrollment[i].addEventListener("click", function () {
            var CourseId = $(this).data('id');
            var CourseName = $(this).data('coursename');
            if (confirm('Are you sure you want to cancel the enrollment for the course '+ CourseName+' ?')) {
                $.ajax({
                    url: '/Learner/CancelEnrollmentClicked',
                    contenttype: 'application/json',
                    cache: false,
                    datatype: "json",
                    data: { CourseId: CourseId },
                    type: "get",
                    success: function (data) {
                        if (data === 1) {
                            window.location.href = '/Learner/Courses';
                        }
                    },
                    error: function (xhr, status, error) {
                        success = false;//doesnt goes here
                        alert('Oops! Something went wrong');
                    }
                });
            }
        })
    }
    $("span[id*='spanRating']").on("click", function () {
        var RatingSelected = this.id;
        var currentRating = parseInt(RatingSelected.slice(-1));
        $("#Rating").val(currentRating);
        for (var i = 1; i <= currentRating; i++) {
            $("#spanRating" + i).attr('class', 'starGlowRatingForm');
        }
        // unselect remaining
        for (var i = currentRating + 1; i <= 5; i++) {
            $("#spanRating" + i).attr('class', 'starFadeRatingForm');
        }
    });
    $("span[id*='spanRating']").on("mouseover", function () {
        var RatingSelected = this.id;
        var currentRating = parseInt(RatingSelected.slice(-1));
        for (var i = 1; i <= currentRating; i++) {
            $("#spanRating" + i).attr('class', 'starGlowRatingForm');
        }
    });
    $("span[id*='spanRating']").on("mouseout", function () {
        var RatingSelected = this.id;
        var currentRating = parseInt(RatingSelected.slice(-1));
        for (var i = 1; i <= currentRating; i++) {
            $("#spanRating" + i).attr('class', 'starFadeRatingForm');
        }
    });
    $('#StarRatingBlockDiv').on("mouseout", function () {
        var setRating = $("#Rating").val();
        for (var i = 1; i <= setRating; i++) {
            $("#spanRating" + i).attr('class', 'starGlowRatingForm');
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