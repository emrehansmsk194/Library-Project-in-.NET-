function viewDetails(eventId) {
    let details_paragraph = document.getElementById("details-" + eventId);
    if (!details_paragraph) {
        var details = document.createElement("p");
        details.id = "details-" + eventId;
        details.className = "card-text text-dark";

        var description = document.getElementById("detail-" + eventId).innerText;
        var textNode = document.createTextNode(description);

        details.appendChild(textNode);
        document.getElementById("divFooter-" + eventId).appendChild(details);
    }
    else {
        details_paragraph.style.display = "block";
    }

    document.getElementById("myButton-" + eventId).style.display = "none";

    let closeButton = document.getElementById("myButton2-" +eventId);
    if (!closeButton) {
        var button = document.createElement("button");
        button.id = "myButton2-" + eventId;
        button.className = "btn btn-light text-dark w-100";
        button.textContent = "Close Details";
        button.onclick = function () {
            closeDetails(eventId);
        };
        document.getElementById("divFooter-" + eventId).appendChild(button);
    }
    else {
        closeButton.style.display = "block";
    }
  
}

function closeDetails(eventId) {
   
    const detailsElement = document.getElementById("details-" + eventId);
    if (detailsElement) {
        detailsElement.style.display = "none";
    }

    const closeButton = document.getElementById("myButton2-" + eventId); 
    if (closeButton) {
        closeButton.style.display = "none";
    }


    const viewButton = document.getElementById("myButton-" + eventId); 
    if (viewButton) {
        viewButton.style.display = "block";
    }
}