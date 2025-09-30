
$(document).ready(function () {
    // Auto hide after 3 seconds
    setTimeout(function () {
        $(".alert").fadeOut("slow", function () {
            $(this).remove();
        });
    }, 3000); // 3 seconds
});

$(document).ready(function () {
    $('#documentTable').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true
    });
});

toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-middle-center",
    "preventDuplicates": true,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

function showLoader() {
    document.getElementById("loader-overlay").style.display = "flex";
}

function hideLoader() {
    document.getElementById("loader-overlay").style.display = "none";
}

// Optional: Auto-show loader on all form submits
$(document).on("submit", "form", function () {
    showLoader();
});

// Optional: Show loader during all AJAX calls
$(document).ajaxStart(function () {
    showLoader();
}).ajaxStop(function () {
    hideLoader();
});

function updateLoaderPercentage(value) {
    const el = document.getElementById("loader-percentage");
    if (el) el.textContent ='Loading....';
}

function confirmAction({ title, text, confirmText, cancelText, icon = 'warning' }) {
    return Swal.fire({
        title: title || 'Are you sure?',
        text: text || 'This action cannot be undone.',
        icon: icon,
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: confirmText || 'Yes, do it!',
        cancelButtonText: cancelText || 'Cancel'
    });
}
