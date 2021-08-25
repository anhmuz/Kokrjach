const uri = 'api/events';
let events = [];

async function getEvents() {
    let response = await fetch(uri);
    let data = await response.json();
    _displayEvents(data);
}

async function onAddEvent() {
    try {
        const addUserIdTextbox = document.getElementById('add-userId');
        const addDescriptionTextbox = document.getElementById('add-description');

        const event = {
            userId: addUserIdTextbox.value.trim(),
            description: addDescriptionTextbox.value.trim()
        };

        let response = await fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(event)
        });

        if (response.ok) {
            alert('Status: ' + response.status + '\n' + 'Location: ' + response.headers.get('Location'));
        } else {
            let data = await response.json();
            alert('Status: ' + response.status + ': ' + data.message);
        }

        await getEvents();
        addUserIdTextbox.value = '';
        addDescriptionTextbox.value = '';
    } catch (error) {
        console.error('Unable to add event.', error);
    }
}

async function onDeleteEvent(id) {
    try {
        await fetch(`${uri}/${id}`, {
            method: 'DELETE'
        });

        await getEvents();
    } catch (error) {
        console.error('Unable to delete event.', error);
    }
}

function onDisplayEditForm(id) {
    const item = events.find(item => item.id === id);

    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-description').value = item.description;
    document.getElementById('editForm').style.display = 'block';
}

async function onEditEvent() {
    try {
        const eventId = parseInt(document.getElementById('edit-id').value, 10);
        const eventUpdate = {
            description: document.getElementById('edit-description').value.trim()
        };

        await fetch(`${uri}/${eventId}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(eventUpdate)
        });

        await getEvents();
    } catch (error) {
        console.error('Unable to update item.', error);
    }

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
        editButton.setAttribute('onclick', `onDisplayEditForm(${event.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `onDeleteEvent(${event.id})`);

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

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    events = data;
}
