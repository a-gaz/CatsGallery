document.getElementById("cat-gallery-container")?.addEventListener("click", async (e) => {
    const button = e.target.closest('.like-btn');
    if (!button) {
        return;
    }

    const url = button.getAttribute('data-action');
    const isCurrentlyLiked = button.getAttribute('data-is-liked') === 'true';

    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            }
        });

        if (response.status === 404) {
            window.location.href = '/Auth/Login?returnUrl=' + encodeURIComponent(window.location.pathname);
            return;
        }

        const result = await response.json();

        if (result.success) {
            const newIsLiked = !isCurrentlyLiked;
            button.setAttribute('data-is-liked', newIsLiked.toString());

            const icon = button.querySelector('.like-icon');
            if (icon) {
                if (newIsLiked) {
                    icon.classList.remove('icon-heart-black');
                    icon.classList.add('icon-heart-red');
                } else {
                    icon.classList.remove('icon-heart-red');
                    icon.classList.add('icon-heart-black');
                }
            }

            if (newIsLiked) {
                button.classList.add('btn-success');
            } else {
                button.classList.remove('btn-success');
            }

            console.log('Лайк = ', newIsLiked);

        } else {
            console.log('Произошла ошибка: ' + result.message);
            button.classList.add('btn-danger');
            setTimeout(() => {
                button.classList.remove('btn-danger');
            }, 1000);
        }

    } catch (error) {
        console.log('Произошла ошибка: ' + error.message);
        button.classList.add('btn-danger');
        setTimeout(() => {
            button.classList.remove('btn-danger');
        }, 1000);
    }
});