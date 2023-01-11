const hideAllDropdowns = () => {
    const objs = document.querySelectorAll('.show');
    objs.forEach(e => e.classList.remove('show'))
}

const addNewCategory = (event, bookmarkId) => {
    if (event.key != 'Enter') return;

    let categoryObj = {
        Name: event.target.value,
        BookmarkId: bookmarkId
    }

    if (categoryObj.Name.length == 0) return;

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
            newCategory.value = result.id;
            newCategory.onclick = (event) => addBookmarkToCategory(event, bookmarkId);
            newCategory.innerHTML = `<i class="bi bi-bookmark-fill" id="category_${result.id}"></i> ${result.name}`;
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
    console.log(data);
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

const deleteCategory = (event, Id) => {
    event.preventDefault();
    event.stopPropagation();
    const Category = { Id };
    $.ajax({
        url: "/delete-category",
        data: JSON.stringify(Category),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            deletedItem = document.getElementById(`category_${Id}`)
            deletedItem.remove();
        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }
    });
}

const editCategory = (event, Id) => {
    event.preventDefault();
    event.stopPropagation();
    hideAllDropdowns();

    const categoryTitle = document.getElementById(`category-title-${Id}`);
    const previousName = categoryTitle.innerHTML;
    if (categoryTitle.innerHTML.includes('<input class="form-control">')) return;
    const categoryNameInput = document.createElement('input');

    categoryNameInput.classList.add('form-control');
    categoryNameInput.value = previousName;
    categoryNameInput.onclick = (e) => {
        e.preventDefault();
        e.stopPropagation();
    }

    categoryTitle.innerHTML = "";
    categoryTitle.appendChild(categoryNameInput);
    categoryNameInput.focus();

    categoryNameInput.onkeydown = (event) => {
        if (event.key == 'Enter' && !event.shiftKey) {
            const category = { Id, Name: categoryNameInput.value }
            if (category.Name.length == 0) return;

            $.ajax({
                url: '/edit-category',
                data: JSON.stringify(category),
                type: 'POST',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    console.log(result)
                    categoryTitle.innerHTML = result.categoryName;
                },
                error: function (errormessage) {
                    console.log(errormessage.responseText);
                }
            })
        } else if (event.key == 'Escape') {
            categoryTitle.innerHTML = previousName;
            console.log('not ok')
        }
    }
    //<input class="form-control" />
}