const addUpvote = (userId, bookmarkId, rating) => {

    var upvoteObj = {
        UserId: userId,
        BookmarkId: bookmarkId,
        Rating: rating
    };
    console.log(upvoteObj)
    const likeButton = document.getElementById("like");
    const dislikeButton = document.getElementById("dislike");
    const ratingDisplay = document.getElementById("rating-text");
    
    const classes = {
        likeFill: 'bi-hand-thumbs-up-fill',
        likeEmpty: 'bi-hand-thumbs-up',
        dislikeFill: 'bi-hand-thumbs-down-fill',
        dislikeEmpty: 'bi-hand-thumbs-down'
    }

    $.ajax({
        url: "/upvote",
        data: JSON.stringify(upvoteObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            ratingDisplay.innerHTML = result.rating;
            if (result.rating == 0) {
                likeButton.classList.remove(classes.likeFill);
                likeButton.classList.add(classes.likeEmpty);

                dislikeButton.classList.remove(classes.dislikeFill);
                dislikeButton.classList.add(classes.dislikeEmpty);
            } else if (result.rating == 1) {
                likeButton.classList.remove(classes.likeEmpty);
                likeButton.classList.add(classes.likeFill);

                dislikeButton.classList.remove(classes.dislikeFill);
                dislikeButton.classList.add(classes.dislikeEmpty);
            } else {
                likeButton.classList.remove(classes.likeFill);
                likeButton.classList.add(classes.likeEmpty);

                dislikeButton.classList.remove(classes.dislikeEmpty);
                dislikeButton.classList.add(classes.dislikeFill);
            }
            console.log(result)
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}  
