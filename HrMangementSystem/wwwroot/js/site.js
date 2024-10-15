function toggleProfileMenu() {
    const profileMenu = document.getElementById('profileMenu');
    profileMenu.classList.toggle('show');
}

function toggleLanguageMenu() {
    const languageMenu = document.getElementById('languageMenu');
    languageMenu.classList.toggle('show');
}

// Close the dropdowns when clicking outside
window.onclick = function (event) {
    if (!event.target.matches('.dash-head-link')) {
        const dropdowns = document.querySelectorAll('.dropdown-menu');
        dropdowns.forEach(dropdown => {
            dropdown.classList.remove('show');
        });
    }
}
function toggleSubmenu() {
    var submenu = document.getElementById('staff-submenu');
    // Toggle display property
    if (submenu.style.display === "block") {
        submenu.style.display = "none"; // Hide the submenu
    } else {
        submenu.style.display = "block"; // Show the submenu
    }
}
document.querySelectorAll('.bs-pass-para').forEach(item => {
    item.addEventListener('click', event => {
        const confirmText = item.getAttribute('data-text');
        if (!confirm(confirmText)) {
            event.preventDefault();
        }
    });
});
$(document).ready(function () {
    // Tooltip initialization
    $('[data-bs-toggle="tooltip"]').tooltip();

    // Delete confirmation
    $('.bs-pass-para').on('click', function (e) {
        e.preventDefault();
        var formId = $(this).data('confirm-yes');
        var confirmation = confirm($(this).data('text'));
        if (confirmation) {
            $('#' + formId).submit();
        }
    });
});
$(document).ready(function () {
    $('#createUserModal').modal('show');
});
document.getElementById("password_switch").addEventListener("change", function () {
    const passwordField = document.getElementById("password");
    passwordField.disabled = !this.checked;
});
document.querySelector('form').addEventListener('submit', function (event) {
    console.log('Form submitted');
});
