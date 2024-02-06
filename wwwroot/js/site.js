// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function togglePartialView() {
    var popup = document.getElementById("myPopup");
    if (popup.style.display === "none") {
        console.log("none");
        popup.style.display = "block";
    } else {
        popup.style.display = "none";
        console.log("block");
    }
}
