// <reference path="../Scripts/jquery-3.1.1.js" />
// <reference path="../Scripts/jquery.validate.js" />
// <reference path="../Scripts/jquery.validate-vsdoc.js" />

$(document).ready(function () {

    $("#login").validate({

        rules: {

            Email: {
                email: true,         //makes first name required
            },
            Password: {
                required: true,         //requires password field
                minlength: 8,           //requires at least 8 characters
                strongpassword: true    //uses custom validator for strong password
            },
        },

        errorClass: "error",
        validClass: "valid"

    });
    $("#register").validate({

        rules: {

            Email: {
                email: true,
            },
            Password: {
                required: true,
                minlength: 8,
                strongpassword: true
            },
            ConfirmPassword: {
                equalTo: "#Password"
            }
        },
        messages: {
            Email: {
                required: "You must provide a valid email address."
            },
            Password: {
                required: "You must provide a valid password",
            },
            ConfirmPassword: {
                equalTo: "This did not match the previous password",
            }
        },
        errorClass: "error",
        validClass: "valid"

    });
    $.validator.addMethod("strongpassword", function (value, index) {
        return value.match(/[A-Z]/) && value.match(/[a-z]/) && value.match(/\d/);  //check for one capital letter, one lower case letter, one num
    }, "Please enter a strong password (one capital, one lower case, and one number)");
});