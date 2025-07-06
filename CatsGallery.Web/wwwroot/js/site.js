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