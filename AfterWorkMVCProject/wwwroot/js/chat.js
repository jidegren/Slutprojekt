"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var listOfParticipants = [];

document.getElementById("sendButton").addEventListener("click", (event) => {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var url = window.location.href;
    var urlArray = url.split("/");
    var group = urlArray[urlArray.length - 1];
    connection.invoke("SendMessageToGroup", group, message, user).catch((err) => console.error(err.toString()));
    event.preventDefault();
    document.getElementById("messageInput").value = "";

});

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
    document.getElementById("DigiKaljaInfo").style.display = "block";

});

connection.on("AddToAWList", function (userName) {
    if (!listOfParticipants.includes(userName)) {
        listOfParticipants.push(userName);
        var table = document.getElementById("participant-table");
        var tr = document.createElement("tr");
        var userNameTd = document.createElement("td");
        var scoreTd = document.createElement("td");
        userNameTd.innerHTML = userName;
        scoreTd.innerHTML = 0;
        tr.appendChild(userNameTd);
        tr.appendChild(scoreTd);
        table.appendChild(tr);
    }

});

connection.on("Foo", function (message) {
    console.log(message);
});

document.getElementById("startDigiKalja").addEventListener("click", (event) => {
    var url = window.location.href;
    var urlArray = url.split("/");
    var group = urlArray[urlArray.length - 1];
    var user = document.getElementById("userInput").value;
    connection.invoke("JoinDigiKalja", group, user).catch(function (err) {
        console.error(err.toString())
    });
    event.prefentDefault();
});

async function initConnection() {
    await connection.start().catch((err) => console.error(err.toString()));
    joinGroup();
}

function joinGroup() {
    var user = document.getElementById("userInput").value;
    var url = window.location.href;
    var urlArray = url.split("/");
    var group = urlArray[urlArray.length - 1];
    connection.invoke("JoinGroup", group, user).catch(function (err) { console.error(err.toString()) });
    
}

function LoadWelcomeMsg () {
    //initConnection();
    $('#myModal').modal("show");
};







