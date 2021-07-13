const uri = 'api/events';

function getEvents() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayEvents(data))
        .catch(error => console.error('Unable to get events.', error));
}

function post() {
    const addIdTextbox = document.getElementById('add-id');
    const addUserIdTextbox = document.getElementById('add-userId');
    const addEventTypeIdTextbox = document.getElementById('add-eventTypeId');
    const addDescriptionTextbox = document.getElementById('add-description');

    const event = {
        id: parseInt(addIdTextbox.value.trim(), 10),
        userId: addUserIdTextbox.value.trim(),
        eventTypeId: addEventTypeIdTextbox.value.trim(),
        description: addDescriptionTextbox.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(event)        
    })
        .then(response => {
            alert(response.headers.get("Location"));
            response.json();
        })
        .then(() => {
            getEvents();
            addIdTextbox.value = '';
            addUserIdTextbox.value = '';
            addEventTypeIdTextbox.value = '';
            addDescriptionTextbox.value = '';
        })
        .catch(error => console.error('Unable to add event.', error));
}

function _displayEvents(data) {
    const tBody = document.getElementById('events');
    tBody.innerHTML = '';

    data.forEach(event => {
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(event.id);
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(event.userId);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(event.eventTypeId);
        td3.appendChild(textNode3);

        let td4 = tr.insertCell(3);
        let textNode4 = document.createTextNode(event.description);
        td4.appendChild(textNode4);
    });
}
