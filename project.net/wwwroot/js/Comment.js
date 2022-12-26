const deleteComment = (commentId) => {
    const comment = {
        Id: commentId
    }
    $.ajax({
        url: `/delete-comment`,
        data: JSON.stringify(comment),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            const deletedComment = document.getElementById(`comment_${result.commentId}`);
            deletedComment.remove();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}