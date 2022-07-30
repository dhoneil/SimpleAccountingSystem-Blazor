(function () {
    window.Select2Init = function () {
        $('.select2').select2()
    },
    window.Select2Trigger = function (modalid) {
        $('.select2').select2({
            dropdownParent: $(`#${modalid}`)
        });
    },
    window.triggerClick = function (elt) {
        document.getElementById(elt).click()
    },
    window.LoadDataTable = function (tableid, modalaction) {
        $(`#${tableid}`).DataTable();
    }
    window.ModalAction = function (modalid, modalaction) {
        $(`#${modalid}`).modal(modalaction);
    },
    window.focusInputFromBlazor = function (selector) {
        var input = document.querySelector(selector);
        if (!focusInput(input)) {
            input = input.querySelector("input");
            focusInput(input);
        }
    },
    window.focusInput = function (input) {
        if (input && input.focus) {
            input.focus();
        }
        else {
            return false;
        }
        console.log("test");
    },
    window.showHidePassword = function () {
        $('.togglepwd').toggleClass("show");
        if ($('.togglepwd').hasClass("show")) {
            $(".password").attr("type", "text");
            $(".show img").attr("src", "images/EyeSlash.svg");
        } else {
            $(".password").attr("type", "password");
            $(".togglepwd img").attr("src", "images/Eye.svg");
        }
    }


})();
