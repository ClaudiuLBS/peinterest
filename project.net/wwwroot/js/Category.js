const addNewCategory = (event, userId, bookmarkId) => {
    if (event.key != 'Enter') return;

    let categoryObj = {
        UserId: userId,
        Name: event.target.value,
        BookmarkId: bookmarkId
    }

    $.ajax({
        url: "/add-category",
        data: JSON.stringify(categoryObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(result);
            const categoriesList = document.getElementById('categories-list');
            const newCategory = document.createElement('li');
            newCategory.classList.add('dropdown-item');
            newCategory.innerHTML = `<i class="bi bi-bookmark-fill"></i> ${result.name}`;
            categoriesList.appendChild(newCategory);
            event.target.value = '';
        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }
    });
}

const addBookmarkToCategory = (event, BookmarkId) => {
    const data = { CategoryId: event.target.value, BookmarkId }
    $.ajax({
        url: "/add-bookmark-to-category",
        data: JSON.stringify(data),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            const bookmarkIcon = document.getElementById(`category_${data.CategoryId}`)
            if (result.action == 'added') {
                bookmarkIcon.classList.remove('bi-bookmark')
                bookmarkIcon.classList.add('bi-bookmark-fill')
            } else {
                bookmarkIcon.classList.remove('bi-bookmark-fill')
                bookmarkIcon.classList.add('bi-bookmark')
            }
        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }
    });
}