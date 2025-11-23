document.getElementById("cat-gallery-container")?.addEventListener("click", async (e) => {
    const button = e.target.closest('.gallery-btn');
    if (!button) {
        return;
    }

    e.preventDefault();
    const actionUrl = button.dataset.action;

    try {
        const response = await fetch(actionUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            }
        });
        
        document.getElementById("cat-gallery-container").innerHTML = await response.text();
    } catch (error) {
        console.error("Произошла ошибка: ", error);
    }
});
