/*const baseUrl = "https://localhost:7096/api/";*/
// BS-Stepper Init

//$("#getCodeButton").on("click", function (e) {
//    e.preventDefault();
//    var formData = {
//        phone: $("input[name='Phone']").val(),
//        password: $("input[name='Password']").val()
//    };
//    if (formData.phone == "" || formData.password == "" || isNaN(formData.phone)) {
//        alert("Please check your credentials");
//        return;
//    }
//    $.ajax({
//        url: baseUrl + "Login/Code",
//        type: "GET",
//        contentType: "application/json",
//        data:formData,
//        success: function (response) {
//            alert("Check your messages");
//            console.log(response);
//            $("#getCodeButton").hide()
//            $("#submitButton").attr("disabled", false)
//        },
//        error: function (xhr, status, error) {

//            alert("Failed" + error);
//            console.error(xhr.responseText);
//        }
//    });
//});
//$("#submitButton").on("click", function () {
//    var formData = {
//        phone: $("input[name='Phone']").val(),
//        password: $("input[name='Password']").val(),
//        otp: $("input[name='Otp']").val(),
//    };
//    $.ajax({
//        url: baseUrl + "Login",
//        type: "POST",
//        contentType: "application/json",
//        data: JSON.stringify(formData),
//        success: function (loginResponse) {
//            if (loginResponse.isSuccess) {
//                alert("Login successful!");
//                console.log(loginResponse.data)
//                window.location.href = "/Home/Index";
//            } else {
//                alert("Login failed: " + loginResponse.message);
//            }
//        },
//        error: function () {
//            alert("An error occurred during login.");
//        }
//    });
//});