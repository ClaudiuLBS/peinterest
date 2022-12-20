﻿const removeURLParameter = (url, parameter) => {
    //prefer to use l.search if you have a location/link object
    var urlparts = url.split('?');
    if (urlparts.length >= 2) {

        var prefix = encodeURIComponent(parameter) + '=';
        var pars = urlparts[1].split(/[&;]/g);

        //reverse iteration as may be destructive
        for (var i = pars.length; i-- > 0;) {
            //idiom for string.startsWith
            if (pars[i].lastIndexOf(prefix, 0) !== -1) {
                pars.splice(i, 1);
            }
        }

        return urlparts[0] + (pars.length > 0 ? '?' + pars.join('&') : '');
    }
    return url;
}
const closeBookmarkModal = () => {
    const currentUrl = window.location.href;
    const newUrl = removeURLParameter(currentUrl, "bookmarkId");
    window.location.href = newUrl;
}

function getHeight() {
    divElement = document.querySelector("#img-container");

    elemHeight = divElement.offsetHeight;

    document.querySelector(".modal-img").style.maxHeight = elemHeight + "px";
    commSection = document.getElementById("comment-section");
    //commSection.style.height = elemHeight * 0.4 + "px";
    console.log(commSection);
}
window.onload = () => {
    getHeight();
}
window.onresize = getHeight;