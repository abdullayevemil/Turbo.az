async function banUser(id) {
    let link = document.getElementById('link-' + id);

    let text = "";

    if (link.textContent.includes("activate")) {
        text = "activate user account";
    }
    else {
        text = "ban user account";
    }

    if (confirm("Confirm that you want to " + text)) {
        await fetch('http://localhost:8080/User/Ban/' + id,
            {
                method: 'POST'
            }).then(res => {
                if (res.ok) {
                    if (link.textContent.includes("activate")) {
                        link.style.color = "red";
                        link.innerText = "ban user account";
                    }
                    else {
                        link.style.color = "green";
                        link.innerText = "activate user account";
                    }
                } else if (res.status === 404) {
                    alert('Item not found.');
                } else {
                    alert('Failed to delete item.');
                }
            });
    }
}

async function deleteUser(id) {
    if (confirm("Confirm that you want the user be deleted.")) {
        await fetch('http://localhost:8080/User/Delete/' + id,
            {
                method: 'DELETE'
            }).then(res => {
                if (res.ok) {
                    let child = document.getElementById(id);

                    child.parentElement.removeChild(child);
                } else if (res.status === 404) {
                    alert('Item not found.');
                } else {
                    alert('Failed to delete item.');
                }
            });
    }
}

async function updateProfile(id) {
    event.preventDefault();
    const form = document.getElementById('update-profile');
    const formData = new FormData(form);
    let jsonData = {};
    for (let [key, value] of formData.entries()) {
        jsonData[key] = value;
    }

    await fetch('/User/UpdateProfile/' + id, {
        method: "PUT",
        body: JSON.stringify(jsonData),
        headers: {
            "Content-Type": "application/json"
        }
    }).then(data => {
        fetch('http://localhost:8080/Identity/LogOut', {
            method: 'DELETE'
        }).then(res => window.location.href = "http://localhost:8080/Identity/Login")
    });
}