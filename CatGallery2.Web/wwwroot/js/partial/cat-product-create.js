let photoFiles = [];
const maxCatPhotos = 4;
let mainPhotoIndex = 0;

document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('photosSlots').addEventListener('click', function(e) {
        if (e.target.classList.contains('cat-photo')) {
            const index = parseInt(e.target.getAttribute('data-index'));
            handleAdditionalPhotoClick(index, e.target);
        }
    });

    document.getElementById('main-preview-photo').addEventListener('click', function() {
        handleMainPhotoClick();
    });
});

function openCatPhotosPicker() {
    document.getElementById('catPhotosInput').click();
}

document.getElementById('catPhotosInput').addEventListener('change', function(e) {
    const files = Array.from(e.target.files);
    const availableSlots = maxCatPhotos - photoFiles.length;
    const filesToAdd = files.slice(0, availableSlots);

    filesToAdd.forEach((file, i) => {
        if (photoFiles.length < maxCatPhotos) {
            const index = photoFiles.length;
            photoFiles.push(file);
            displayAdditionalPhotoPreview(file, index);
        }
    });

    this.value = '';
});

function displayAdditionalPhotoPreview(file, index) {
    const reader = new FileReader();

    const slotsContainer = document.getElementById('photosSlots');
    const imgs = slotsContainer.getElementsByClassName('cat-photo');

    reader.onload = (e) => {
        if (imgs[index]) {
            imgs[index].src = e.target.result;
            imgs[index].classList.remove('default-image');
            imgs[index].classList.add('uploaded-image');
            changeBtnVisibility(imgs[index], true);
        }

        if (index === 0 && photoFiles.length === 1) {
            mainPhotoIndex = 0;
            showOnMainImg(e.target.result);
        }
    };

    reader.readAsDataURL(file);
}

function handleAdditionalPhotoClick(index, imgElement) {
    if (!isDefaultImage(imgElement.src)) {
        mainPhotoIndex = index;
        showOnMainImg(imgElement.src);
    } else {
        openCatPhotosPicker();
    }
}

function handleMainPhotoClick() {
    const mainPhoto = document.getElementById('main-preview-photo');
    if (isDefaultImage(mainPhoto.src)) {
        openCatPhotosPicker();
    }
}

function isDefaultImage(src) {
    return src.includes('default_cat_image.svg');
}

function showOnMainImg(fileSrc){
    const mainPreviewPhoto = document.getElementById('main-preview-photo');
    mainPreviewPhoto.src = fileSrc;
    mainPreviewPhoto.classList.remove('default-image');
    mainPreviewPhoto.classList.add('uploaded-image');
}

function changeBtnVisibility(img, show) {
    let parent = img.parentElement;
    let removeBtn = parent.getElementsByClassName('remove-button')[0];
    if(show) {
        removeBtn.style.display = '';
    }
    else {
        removeBtn.style.display = 'none';
    }
}

function removeAdditionalPhoto(index) {
    if (photoFiles[index]) {
        if (index === mainPhotoIndex) {
            mainPhotoIndex = 0;
        } else if (index < mainPhotoIndex) {
            mainPhotoIndex--;
        }

        photoFiles.splice(index, 1);

        const slotsContainer = document.getElementById('photosSlots');
        const imgs = slotsContainer.getElementsByClassName('cat-photo');

        for (let i = 0; i < maxCatPhotos; i++) {
            if (i < photoFiles.length) {
                displayAdditionalPhotoPreview(photoFiles[i], i);
            } else {
                imgs[i].src = '/images/default_cat_image.svg';
                imgs[i].classList.remove('uploaded-image');
                imgs[i].classList.add('default-image');
                changeBtnVisibility(imgs[i], false);
            }
        }

        if (photoFiles.length > 0 && mainPhotoIndex < photoFiles.length) {
            const reader = new FileReader();
            reader.onload = (e) => {
                showOnMainImg(e.target.result);
            };
            reader.readAsDataURL(photoFiles[mainPhotoIndex]);
        } else if (photoFiles.length === 0) {
            mainPhotoIndex = 0;
            const mainPhoto = document.getElementById('main-preview-photo');
            mainPhoto.src = '/images/default_cat_image.svg';
            mainPhoto.classList.remove('uploaded-image');
            mainPhoto.classList.add('default-image');
        }
    }
}

document.getElementById('catForm').addEventListener('submit', function(e) {
    const mainPhoto = document.getElementById('main-preview-photo');
    if (isDefaultImage(mainPhoto.src) && photoFiles.length === 0) {
        e.preventDefault();
        alert('Пожалуйста, выберите хотя бы одно фото');
        return false;
    }

    const formData = new FormData(this);

    formData.delete('CatPhotos');

    photoFiles.forEach((file, index) => {
        formData.append('CatPhotos', file);
    });

    const tagInputs = document.querySelectorAll('.tag');
    const tags = Array.from(tagInputs)
        .map(input => input.value.trim())
        .filter(tag => tag !== '');

    formData.delete('Tags');
    tags.forEach(tag => {
        formData.append('Tags', tag);
    });

    formData.append('MainPhotoIndex', mainPhotoIndex.toString());

    fetch(this.action, {
        method: 'POST',
        body: formData,
    }).then(response => {
        if (response.ok) {
            window.location.href = '/gallery'; 
        } else {
            alert('Ошибка при создании кота');
        }
    }).catch(error => {
        console.error('Error:', error);
        alert('Ошибка при создании кота');
    });

    e.preventDefault();
    return false;
});

function addTag() {
    let tagsContainer = document.getElementById('tagsContainer');
    let tagInputs = tagsContainer.querySelectorAll('.tag');

    if(tagInputs.length >= 5) {
        alert("Нельзя установить более 5 тэгов");
        highlightTags();
        return;
    }

    let lastTag = tagInputs[tagInputs.length - 1];
    if (lastTag && lastTag.value.trim() === "") {
        highlightTag(lastTag);
        return;
    }

    let newTagHTML = `
        <div class="input-box tag-item new-tag">
            <input class="tag form-input" type="text" name="tag" placeholder="Введите тег">
            <button type="button" class="remove-tag-btn" onclick="removeTag(this)">×</button>
        </div>
    `;

    tagsContainer.insertAdjacentHTML('beforeend', newTagHTML);

    setTimeout(() => {
        const newTag = tagsContainer.lastElementChild;
        if (newTag) {
            newTag.classList.remove('new-tag');
            const input = newTag.querySelector('.tag');
            input.focus();
        }
    }, 300);
}

function removeTag(button) {
    let tagItem = button.closest('.tag-item');
    let tagsContainer = document.getElementById('tagsContainer');
    let allTags = tagsContainer.querySelectorAll('.tag-item');

    if (allTags.length <= 1) {
        let input = tagItem.querySelector('.tag');
        input.value = '';
        input.focus();
        highlightTag(input);
        return;
    }

    tagItem.style.transform = 'translateX(-100%)';
    tagItem.style.opacity = '0';

    setTimeout(() => {
        tagItem.remove();
    }, 300);
}

function highlightTag(element) {
    element.classList.add('tag-highlight');
    setTimeout(() => element.classList.remove('tag-highlight'), 1000);
}

function highlightTags() {
    let tags = document.querySelectorAll('.tag');
    tags.forEach(tag => highlightTag(tag));
}