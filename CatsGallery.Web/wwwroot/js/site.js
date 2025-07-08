// TODO > jquery в 2025 ладно. Ну жквери не нужен потому что это все есть в современном  чистом js

$('#gallery-container').on('click', '.gallery-btn', function(e) {
    e.preventDefault();
    var actionUrl = $(this).data('action');

    $.ajax({
        url: actionUrl,
        type: 'POST',
        success: function(result) {
            $('#gallery-container').html(result);
        },
        error: function(xhr, status, error) {
            console.error("Ошибка:", error);
        }
    });
});