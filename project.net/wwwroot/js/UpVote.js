const addUpvote = (bookmarkId, rating) => {

    //obiectul trimis in body
    var upvoteObj = {
        BookmarkId: bookmarkId,
        Rating: rating
    };

    //butoane
    const likeButton = document.getElementById("like");
    const dislikeButton = document.getElementById("dislike");
    const ratingDisplay = document.getElementById("rating-text");

    //clase pt a stiliza like/dislike cand apasam pe ele
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
            //diferenta dintre like-uri si dislike-uri
            ratingDisplay.innerHTML = result.rating;

            //actualizam style-ul butoanelor de like/dislike
            if (result.userRating == 0) {
                likeButton.classList.remove(classes.likeFill);
                likeButton.classList.add(classes.likeEmpty);

                dislikeButton.classList.remove(classes.dislikeFill);
                dislikeButton.classList.add(classes.dislikeEmpty);
            } else if (result.userRating == 1) {
                likeButton.classList.remove(classes.likeEmpty);
                likeButton.classList.add(classes.likeFill);

                dislikeButton.classList.remove(classes.dislikeFill);
                dislikeButton.classList.add(classes.dislikeEmpty);
            } else if (result.userRating == -1) {
                likeButton.classList.remove(classes.likeFill);
                likeButton.classList.add(classes.likeEmpty);

                dislikeButton.classList.remove(classes.dislikeEmpty);
                dislikeButton.classList.add(classes.dislikeFill);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}  
