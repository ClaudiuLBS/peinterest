const editComment = (commentId) => {
    //Make a backup of the comment element and delete its content
    const commentElement = document.getElementById(`comment_${commentId}`);
    const commentContent = document.getElementById(`comment_content_${commentId}`);
    const commentBackup = commentElement.cloneNode(true);
    commentElement.innerHTML = "";
    //Create input element
    const commentInput = document.createElement('textarea');
    commentInput.classList.add('form-control');
    commentInput.innerHTML = commentContent.innerHTML;
    commentElement.appendChild(commentInput);
    commentInput.focus();

    commentInput.onkeydown = (event) => {
        if (event.key == 'Enter' && !event.shiftKey) {
            const comment = {
                Id: commentId,
                Content: commentInput.value
            }

            $.ajax({
                url: `/edit-comment`,
                data: JSON.stringify(comment),
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    console.log(result);
                    commentElement.innerHTML = commentBackup.innerHTML; 
                    document.getElementById(`comment_content_${commentId}`).innerHTML = commentInput.value;
                },
                error: function (errormessage) {
                    alert(errormessage.responseText);
                }
            });
            //update comment
        } else if (event.key == 'Escape') {
            //cancel edit
            commentElement.innerHTML = commentBackup.innerHTML;
        }
    }
}

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