
document.querySelectorAll(".ModalToggle").forEach(function (toggleButton) {
    toggleButton.addEventListener("click", function (event) {
        event.stopPropagation(); 

        alert("click");
        const register = document.querySelector("#Register");
        const login = document.querySelector("#Login");

        var swapVal = login.style.display;
        login.style.display = register.style.display;
        register.style.display = swapVal;

        console.log(register.classList);
    });
});
var toastEl = document.getElementById('liveToast');
var toast = new bootstrap.Toast(toastEl);
toast.show();

(function () {
    'use strict';
		if (login) {
			document.querySelector("#Register").style.display = "none";
			document.querySelector("#Login").style.display = "block";
		} else {
			document.querySelector("#Login").style.display = "none";
			document.querySelector("#Register").style.display = "block";
		}
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
