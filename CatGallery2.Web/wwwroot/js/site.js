document.getElementById("gallery-container")?.addEventListener("click", async (e) => {
    const button = e.target.closest('.gallery-btn');
    if (!button) return;

    e.preventDefault();
    const actionUrl = button.dataset.action;

    try {
        const response = await fetch(actionUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            }
        });

        const result = await response.text();
        document.getElementById("gallery-container").innerHTML = result;
    } catch (error) {
        console.error("Error:", error);
    }
});