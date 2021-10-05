"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var scores = [];
var descriptionsListed = [];

//Den här metoden verkar inte fylla någon funktion i digikalja.js
//utan är här ersatt med "CreateUserElement" som motsvarigheten 
//till den här metoden med "ReceiveMessage" i chat.js

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
});

async function initConnection() {
    await connection.start().catch((err) => console.error(err.toString()));
    newUserJoined();
}

function newUserJoined() {
    var url = window.location.href;
    var urlArray = url.split("/");
    var group = urlArray[urlArray.length - 1];
    connection.invoke("NewUserJoined", group).catch((err) => console.error(err.toString()));
}

//Skicka sin beskrivning av ordet till gruppen, vilken sedan ska visas  upp.
//Motsvarar i chat.js "sendButton" med connection.invoke("SendMessageToGroup")
var btnDescriptionDone = document.getElementById("btn-description-done");

btnDescriptionDone.addEventListener("click", (event) => {
    var user = "Anonym spelare"; //TODO om vi hinner lägga in "Beskrivning #1, Beskrivning #2 osv
    var description = document.getElementById("description-text").value;
    var url = window.location.href;
    var urlArray = url.split("/");
    var group = urlArray[urlArray.length - 1];
    var divWaiting = document.getElementById("div-waiting");
    divWaiting.style.display = "block";

    modalDescribeWord.style.display = "none";
    

    var rightDesc = document.getElementById("description").innerText;
    var rightWord = document.getElementById("word-to-describe").innerText;
    document.getElementById("theWord").innerText = `ORD: ${rightWord}`;

    

    connection.invoke("SendRightDescription", group, rightDesc, "Computer", 200000).catch((err) => console.error(err.toString()));
    connection.invoke("SendDescriptionToGroup", group, description, user).catch((err) => console.error(err.toString()));
    
    
    
    event.preventDefault();
});


// Get the modal
var modalDescribeWord = document.getElementById("modalDescribeWord");
var modalShowAllDescriptions = document.getElementById("modalShowAllDescriptions");
var modalShowResult = document.getElementById("modal-show-result");

// Get the button that opens the modal
var btnShowModal = document.getElementById("btn-show-modal");
var btnSendGuess = document.getElementById("btn-send-guess");
var btnNewRound = document.getElementById("btn-new-round");
var toggleCards;
var quitBtn = document.getElementById("btn-quit-game");

//Get thu DigiKalja-info div
var digiKaljaInfo = document.getElementById("DigiKaljaInfo");

//Skapa cards som visar spelares ordbeskrivningar
connection.on("ReceiveDescription", function (user, description, hiddenUserName, id) {
    //document.getElementById("theWord").innerText = `ORD: ${word}`;

    if (!descriptionsListed.includes(description)){
        descriptionsListed.push(description);
         

        var tempScore = 0;
        var heading = user.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var msg = description.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        let toggle = false;
        var divWaiting = document.getElementById("div-waiting");
        divWaiting.style.display = "none";

        modalShowAllDescriptions.style.display = "block";
        var p = document.createElement("p");
        p.textContent = hiddenUserName;
        p.id = 'hidden';
        p.style.display = "none";


        var div1 = document.createElement("div");
        div1.className = 'col-sm-6';

        var div2 = document.createElement("div");
        div2.className = 'card';
        div1.appendChild(div2);

        var cardHeading = document.createElement("h5");
        cardHeading.className = 'card-title';
        cardHeading.textContent = `${id}`;
        cardHeading.style.display = "none";

        var div3 = document.createElement("div");
        div3.className = 'card-body';
        div3.style.color = 'black';
        div3.addEventListener("click", (event) => {

            toggle = !toggle;
            if (toggle) {
                div3.style.backgroundColor = "lightgreen";
                cardHeading.id = 'chosen';
                tempScore += 1;
            }
            else {
                div3.style.backgroundColor = "white";
                cardHeading.id = 'notChosen';
                tempScore -= 1;
            }

            event.preventDefault();
        });

        div2.appendChild(div3);


        div3.appendChild(cardHeading);
        div3.appendChild(p);

        var pDescription = document.createElement("p");
        pDescription.className = 'card-text';
        pDescription.textContent = msg;
        div3.appendChild(pDescription);

        document.getElementById("div-card-container").appendChild(div1);

        toggleCards += document.getElementsByClassName("card-body");
    //Skapa resultattabell när man skickar in sin gissning på beskrivning
    }
});

quitBtn.addEventListener("click", (event) => {
    var url = window.location.href;
    var urlArray = url.split("/");
    var group = urlArray[urlArray.length - 1]
    connection.invoke("GivePointsToUserInvoke", group).catch((err) => console.error(err.toString()));
    //urlArray[urlArray.length - 2] = 'lounge';
    //var realUrl = urlArray.join('/');
    //console.log(realUrl);
    //window.location.href = realUrl;
    //event.preventDefault();
})

connection.on("EndGame", function () {
    var url = window.location.href;
    var urlArray = url.split("/");
    urlArray[urlArray.length - 2] = 'lounge';
    var realUrl = urlArray.join('/');
    console.log(realUrl);
    window.location.href = realUrl;
    event.preventDefault();
})

btnNewRound.addEventListener("click", (event) => {
    var url = window.location.href;
    var urlArray = url.split("/");
    var group = urlArray[urlArray.length - 1];
    console.log(group);
    connection.invoke("StartNewRound", group).catch((err) => console.error(err.toString()));
    event.preventDefault();
})

btnSendGuess.addEventListener("click", (event) => {
    
    var url = window.location.href;
    var urlArray = url.split("/");
    var group = urlArray[urlArray.length - 1];
    var id = document.getElementById("chosen").textContent;
    var div = document.getElementById("chosen");
    var hiddenUserName = div.nextSibling.textContent;
    var hiddenProfileName = document.getElementById("hiddenProfileName").textContent;
    if (hiddenUserName == hiddenProfileName) {
        var ele = document.getElementById("theWord");
        var p = document.createElement("p");
        p.innerHTML = "Hördu fuskis! Du får inte rösta på din egen definition!";
        ele.insertAdjacentElement('afterend', p);
        return;
    }
    console.log(group);
    console.log(id);
    console.log(hiddenUserName)
    modalShowAllDescriptions.style.display = "none";
    modalShowResult.style.display = "block";
    var isComputer = false;
    if (hiddenUserName === "Computer") {
        isComputer = true;
    }
    connection.invoke("SetScoreById", id, group, hiddenUserName, isComputer).catch((err) => console.error(err.toString()));
    event.preventDefault();
})

connection.on("GetScoreById", function (score, hiddenUserName, msg, id, userScore) {
    if (id != 200000) {
        if (!scores.includes(hiddenUserName)) {
            scores.push(hiddenUserName);


            var tableRow = document.createElement("tr");
            var tdPlayer = document.createElement("td");
            tdPlayer.innerText = hiddenUserName; //Hämta från AspNetUser eller UserInfo (se ChautHub)
            tableRow.appendChild(tdPlayer);


            var tdDescription = document.createElement("td");
            tdDescription.innerText = msg;  //Hämta beskrvining kopplat till player
            tableRow.appendChild(tdDescription);
            var tdScore = document.createElement("td");
            tdScore.id = `${hiddenUserName}`
            tdScore.innerHTML = score; //Tilldela poäng till berörd beskrivning
            tableRow.appendChild(tdScore);
            document.getElementById("resultTable").appendChild(tableRow);

            var scoreElement = document.getElementById(`${hiddenUserName}-score`);
            console.log(score);
            var earlyScore = parseInt(scoreElement.innerText);
            console.log(earlyScore);
            scoreElement.innerHTML = /*userScore*/earlyScore + 1;

        } else {
            document.getElementById(hiddenUserName).innerHTML = score;
            var scoreElement = document.getElementById(`${hiddenUserName}-score`);
            var earlyScore = parseInt(scoreElement.innerText);
            console.log(earlyScore);
            scoreElement.innerHTML = /*userScore*/earlyScore + 1;
        }
        }else {
        var comp = "RÄTT SVAR";
        var realScore = 1;
            if (!scores.includes(comp)) {
                scores.push(comp);


                var tableRow = document.createElement("tr");
                var tdPlayer = document.createElement("td");
                tdPlayer.innerText = comp; //Hämta från AspNetUser eller UserInfo (se ChautHub)
                tableRow.appendChild(tdPlayer);


                var tdDescription = document.createElement("td");
                tdDescription.innerText = msg;  //Hämta beskrvining kopplat till player
                tableRow.appendChild(tdDescription);
                var tdScore = document.createElement("td");
                tdScore.id = `${comp}`
                tdScore.innerHTML = realScore; //Tilldela poäng till berörd beskrivning
                tableRow.appendChild(tdScore);
                document.getElementById("resultTable").appendChild(tableRow);

                var scoreElement = document.getElementById(`${hiddenUserName}-score`);
                var earlyScore = parseInt(scoreElement.innerText);
                console.log(earlyScore);
                scoreElement.innerHTML = /*userScore*/earlyScore + 2;

            } else {
                var scoreElementTwo = document.getElementById(`${comp}`);
                var scoreTwo = parseInt(scoreElementTwo.innerText);
                document.getElementById(comp).innerHTML = scoreTwo + 1;
                var scoreElement = document.getElementById(`${hiddenUserName}-score`);
                var earlyScore = parseInt(scoreElement.innerText);
                console.log(earlyScore);
                scoreElement.innerHTML = /*userScore*/earlyScore + 2;
            }
        }
    })


connection.on("StartNewGame", function ()
{
    location.reload();
    digiKaljaInfo.style.display = "none";
    document.getElementById("description-text").value = "";
})

connection.on("AddPointsToDBInvoke", function () {
    var url = window.location.href;
    var urlArray = url.split("/");
    var group = urlArray[urlArray.length - 1]
    connection.invoke("AddPointsToDB", group).catch((err) => console.error(err.toString()));

})


//Kontroll för att se i console om funktionen körs.
connection.on("Foo", function (message) {
    console.log(message);
});

// When the user clicks on the button, open the modal
btnShowModal.onclick = function () {
    modalDescribeWord.style.display = "block";
    btnShowModal.style.display = "none";
    digiKaljaInfo.style.display = "none";
}


