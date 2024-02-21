// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function togglePopup() {
    var popup = document.getElementById("myPopup");
    if (popup.style.display === "none") {
        console.log("none");
        popup.style.display = "block";
    } else {
        popup.style.display = "none";
        console.log("block");
    }
}

function togglePopup1() {
    var popup1 = document.getElementById("myPopup1");
    
    if (popup1.style.display === "none") {
        popup1.style.display = "block";
    } else {
        popup1.style.display = "none";
    }
}

function togglePopup2() {
    var popup2 = document.getElementById("myPopup2");
    
    if (popup2.style.display === "none") {
        popup2.style.display = "block";
    } else {
        popup2.style.display = "none";
    }
}

function togglePopup3() {
    var popup3 = document.getElementById("myPopup3");
    
    if (popup3.style.display === "none") {
        popup3.style.display = "block";
    } else {
        popup3.style.display = "none";
    }
}

function toggleAppointmentPopup(appointmentId) {
    var popup = document.getElementById("myPopup3_" + appointmentId);
    if (popup.style.display === "none") {
        popup.style.display = "block";
    } else {
        popup.style.display = "none";
    }
}


function toggleViewRolePopup(roleId)
{
    var popup = document.getElementById("myPopup1_" + roleId);
    if (popup.style.display === "none") {
        popup.style.display = "block";
    } else {
        popup.style.display = "none";
    }
}