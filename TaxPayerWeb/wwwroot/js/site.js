$(function () {
    const $register = $("#Register");
    const $login = $("#Login");

    // Assuming 'login' is a boolean variable that determines which form to show
    if (login) {
        // Show the Login form, hide Register form
        $register.css("display", "none");
        $login.css("display", "block");
    } else {
        // Show the Register form, hide Login form
        $register.css("display", "block");
        $login.css("display", "none");
    }
    $(".ModalToggle").on("click", function (event) {
        event.stopPropagation(); // Prevent event bubbling
        // Get current display values
        var registerDisplay = $register.css("display");
        var loginDisplay = $login.css("display");

        // Swap the display values: one should be block, the other should be none
        if (registerDisplay === "none") {
            $register.css("display", "block");
            $login.css("display", "none");
        } else {
            $register.css("display", "none");
            $login.css("display", "block");
        }

        // Log to verify the changes
        console.log("Register display: " + $register.css("display"));
        console.log("Login display: " + $login.css("display"));
    });
});

(function () {
    var forms = document.querySelectorAll('.needs-validation');
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault();
                    event.stopPropagation();
                }

                form.classList.add('was-validated');
            }, false);
        });
})();

var toastEl = document.getElementById('liveToast');
var toast = new bootstrap.Toast(toastEl);
toast.show();