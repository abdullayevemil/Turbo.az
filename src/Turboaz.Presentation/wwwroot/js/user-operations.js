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

async function deleteUser(id)
{
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