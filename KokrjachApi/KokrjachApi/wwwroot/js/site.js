const uri = 'api/events';
let events = [];

function getEvents() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayEvents(data))
        .catch(error => console.error('Unable to get events.', error));
}

function post() {
    const addUserIdTextbox = document.getElementById('add-userId');
    const addDescriptionTextbox = document.getElementById('add-description');

    const event = {
        userId: addUserIdTextbox.value.trim(),
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
            getEvents();
            addUserIdTextbox.value = '';
            addDescriptionTextbox.value = '';

            if (response.ok) {
                alert('Status: ' + response.status + '\n' + 'Location: ' + response.headers.get('Location'));
            } else {
                response.json().then(data => {
                    alert('Status: ' + response.status + ': ' + data.message);
                });
            }
        })
        .catch(error => console.error('Unable to add event.', error));
}

function displayEditForm(id) {
    const item = events.find(item => item.id === id);

    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-description').value = item.description;
    document.getElementById('editForm').style.display = 'block';
}

function put() {
    const eventId = parseInt(document.getElementById('edit-id').value, 10);
    const eventUpdate = {
        description: document.getElementById('edit-description').value.trim()
    };

    fetch(`${uri}/${eventId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(eventUpdate)
    })
        .then(() => getEvents())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayEvents(data) {
    const tBody = document.getElementById('events');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(event => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${event.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(event.id);
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(event.userId);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(event.description);
        td3.appendChild(textNode3);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);
    });

    events = data;
}
