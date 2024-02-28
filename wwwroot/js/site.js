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
    var popup = document.getElementById("myPopup4_" + appointmentId);
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
function toggleViewPopup(Id) {
    var popup3 = document.getElementById("myPopup1_" + Id);
    
    if (popup3.style.display === "none") {
        popup3.style.display = "block";
    } else {
        popup3.style.display = "none";
    }
}

function toggleEditPopup(Id) {
    var popup3 = document.getElementById("myPopup2_" + Id);
    
    if (popup3.style.display === "none") {
        popup3.style.display = "block";
    } else {
        popup3.style.display = "none";
    }
}

function toggleDeletePopup(Id) {
    var popup3 = document.getElementById("myPopup3_" + Id);
    
    if (popup3.style.display === "none") {
        popup3.style.display = "block";
    } else {
        popup3.style.display = "none";
    }
}

const connection = new signalR.HubConnectionBuilder() // Use 'HubConnectionBuilder' instead of 'HubConnectionbuilder'
    .withUrl("/jobshub")
    .configureLogging(signalR.LogLevel.Information) // Use 'LogLevel' instead of 'Loglevel'
    .build();

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
}

connection.onclose(async () => {
    await start();
});

start();

connection.on("ConcurrentJobs", function (message) {
    var li = document.createElement("li");
    document.getElementById("concurrentJobs").appendChild(li);
    li.textContent = `${message}`;
});

connection.on("NonConcurrentJobs", function (message) {
    var li = document.createElement("li");
    document.getElementById("nonConcurrentJobs").appendChild(li); 
    li.textContent = `${message}`;
});
