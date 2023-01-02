const removeURLParameter = (url, parameter) => {
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

function updateQueryStringParameter(key, value) {
    const uri = window.location.href
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        window.location.href = uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        window.location.href = uri + separator + key + "=" + value;
    }
}


const getHeight = () => {
    divElement = document.querySelector("#img-container");
    if (!divElement) return;
    elemHeight = divElement.offsetHeight;
    document.querySelector(".modal-img").style.maxHeight = elemHeight + "px";
    document.body.classList.add('stop-scrolling');
}

const handleSearchBar = () => {
    const searchBar = document.getElementById('search-bar');
    const searchBarButton = document.getElementById('search-bar-button');
    searchBarButton.onclick = () => updateQueryStringParameter('search', searchBar.value);
    searchBar.onkeydown = (event) => {
        if (event.key == 'Enter')
            updateQueryStringParameter('search', searchBar.value);
    }
}

window.onload = () => {
    getHeight();
    handleSearchBar();
}
window.onresize = getHeight;