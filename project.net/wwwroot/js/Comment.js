commentInput = document.getElementById('new-comment-content');

onCommentInputKeyDown = (event, bookmarkId) => {
    if (event.key == 'Enter' && !event.shiftKey) {
        event.preventDefault();
        event.stopPropagation()
        newComment(bookmarkId)
    }
}

const newComment = (bookmarkId) => {
    const Comment = {
        BookmarkId: bookmarkId,
        Content: commentInput.value
    };

    if (Comment.Content.length == 0) return;

    commentInput.value = "";
    $.ajax({
        url: `/new-comment`,
        data: JSON.stringify(Comment),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: result => {
            location.reload();
        },
        error: errormessage => {
            alert(errormessage.responseText);
        }
    });
}

const editComment = (commentId) => {
    //Make a backup of the comment element and delete its content
    const commentElement = document.getElementById(`comment_${commentId}`);
    const commentContent = document.getElementById(`comment_content_${commentId}`);
    const commentBackup = commentElement.cloneNode(true);
    commentElement.innerHTML = "";

    //Create input element and focus it
    const commentInput = document.createElement('textarea');
    commentInput.classList.add('form-control');
    commentInput.innerHTML = commentContent.innerHTML;
    commentElement.appendChild(commentInput);
    commentInput.focus();

    commentInput.onkeydown = (event) => {
        if (event.key == 'Enter' && !event.shiftKey) {
            event.preventDefault();
            event.stopPropagation()
            //save edit
            const comment = {
                Id: commentId,
                Content: commentInput.value
            }
            if (comment.Content.length == 0) return;

            $.ajax({
                url: `/edit-comment`,
                data: JSON.stringify(comment),
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: result => {
                    commentElement.innerHTML = commentBackup.innerHTML; 
                    document.getElementById(`comment_content_${commentId}`).innerHTML = commentInput.value;
                    commentElement.children[0].children[0].classList.remove('show');
                    commentElement.children[0].children[1].classList.remove('show');
                },
                error: errormessage => {
                    alert(errormessage.responseText);
                }
            });
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