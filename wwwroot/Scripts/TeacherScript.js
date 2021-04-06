$(document).ready()
{
    hideSuccessfulUploadedMessage();

    function AddTableFilesRow(table, files) {

        $.each(files, function (index, file) {
            var fileName = file["tutorialName"];
            $row = $('<tr/>');
            var topicId = "Topic " + (index+1);
            $col1 = $('<td/>').append(topicId);
            $row.append($col1);
            $col2 = $('<td/>').append(fileName);
            $row.append($col2);
            table.append($row);
        });
    }


    function ClearAllTopicTableRows() {
        var table = $('table[id="tblTopicsRight"]');
        table.find("tr:gt(0)").remove();
    }


    function ResponseAddFilesToTable(data) {
        var table = $('table[id="tblTopicsRight"]');
        ClearAllTopicTableRows();
        var ReplacementDiv = $('#TopicsRightTableReplacementForNoData')
        if (data.length > 0) {
            ReplacementDiv.css({ 'display': 'none' });
            AddTableFilesRow(table, data);
        }
        else {
            table.css({ 'display': 'none' });
            ReplacementDiv.css({ 'display': 'block' });
            ReplacementDiv.text('Sorry No Tutorials added to this course yet')
        }
    }
    $('#TutorialFile_CourseId').change(function () {
        hideSuccessfulUploadedMessage();
        var id = $(this).val();
        $.ajax({
            url: '/Teacher/GetTutorialFilesForCourseId',
            contenttype: 'application/json',
            cache: false,
            datatype: "json",
            data: { CourseId: id },
            type: "get",
            success: function (data) {
                ResponseAddFilesToTable(data);
            },
            error: function (xhr, status, error) {
                success = false;//doesnt goes here
            }
        });
    });

    $('#TutorialFile_CourseId').trigger('change');

    var CourseIdOnPost = $('#CourseIdOnPost').val();
    var UploadedTutorialNameOnPost = $('#UploadedTutorialNameOnPost').val();
    if (CourseIdOnPost && UploadedTutorialNameOnPost) {
        $('#TutorialFile_CourseId').val(CourseIdOnPost);
        showSuccessfulUploadedMessage(UploadedTutorialNameOnPost);
    }
    function showSuccessfulUploadedMessage(UploadedTutorialNameOnPost) {
        $('#TeacherTutorialUploadedSuccessfullId').css({ 'display': 'block' });
        $('#TeacherTutorialUploadedSuccessfullId').text('The Tutorial File ' + UploadedTutorialNameOnPost + ' is Uploaded Successfully');
    }
    function hideSuccessfulUploadedMessage() {
        $('#TeacherTutorialUploadedSuccessfullId').css({ 'display': 'none' });
    }



}