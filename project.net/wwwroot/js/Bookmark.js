const handleEditBookmark = () => {
    //get parents
    const titleSection = document.getElementById('title-section');
    const descriptionSection = document.getElementById('description-section');

    //save backup
    const currentTitle = titleSection.cloneNode(true);
    const currentDescription = descriptionSection.cloneNode(true);

    //delete content
    titleSection.innerHTML = '';
    descriptionSection.innerHTML = '';

    //add title input
    const titleInput = document.createElement('input');
    titleInput.type = 'text';
    titleInput.classList.add('form-control');
    titleInput.value = currentTitle.innerHTML;
    titleSection.appendChild(titleInput);

    //add description input
    const descriptionInput = document.createElement('textarea');
    descriptionInput.ariaRowCount = 2;
    descriptionInput.classList.add('form-control');
    descriptionInput.value = currentDescription.children[0].innerHTML;
    descriptionSection.appendChild(descriptionInput);

    //add confirm button
    const confirmButton = document.createElement('div');
    confirmButton.id = 'confirm-bookmark-edit';
    confirmButton.innerHTML = 'Confirm <i class="bi bi-check-circle"></i>';

    //add cancel button
    const cancelButton = document.createElement('div');
    cancelButton.id = 'cancel-bookmark-edit';
    cancelButton.innerHTML = 'Cancel <i class="bi bi-x-circle"></i>'
    
    //add buttons container
    const buttonsContainer = document.createElement('div');
    buttonsContainer.id = "bookmark-edit-container";
    buttonsContainer.appendChild(confirmButton);
    buttonsContainer.appendChild(cancelButton);
    document.body.appendChild(buttonsContainer);

    //function to close inputs and go back to texts
    const exitEditSpace = () => {
        titleSection.innerHTML = currentTitle.innerHTML;
        descriptionSection.innerHTML = currentDescription.innerHTML;
        document.body.removeChild(buttonsContainer);
    }

    confirmButton.onclick = () => {
        const params = new Proxy(new URLSearchParams(window.location.search), {
            get: (searchParams, prop) => searchParams.get(prop),
        });

        const bookmarkObj = {
            Id: params.bookmarkId,
            Title: titleInput.value,
            Description: descriptionInput.value,
        };
        $.ajax({
            url: "/edit-bookmark",
            data: JSON.stringify(bookmarkObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                currentTitle.innerHTML = result.title;
                currentDescription.children[0].innerHTML = result.description;
                exitEditSpace();
                console.log(result);
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });
    }

    //cancel button function
    cancelButton.onclick = exitEditSpace
}

//const handleSearch = () => {
//    const searchBar = document.getElementById('search-bar');
//    const bookmarks = document.getElementsByClassName('bookmark-card');
//    console.log(bookmarks);
//    searchBar.onkeyup = (event) => {
//        for (let item of bookmarks) {
//            const title = item.children[1].children[0].innerHTML;
//            const description = item.children[1].children[1].innerHTML;
//            if (title.includes(searchBar.value) || description.includes(searchBar.value))
//                item.style.display = 'block';
//            else
//                item.style.display = 'none';
//        }
//    }
//}

//window.onload = () => {
//    handleSearch();
//}