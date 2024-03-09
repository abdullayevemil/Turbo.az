async function deleteVehicle(id) {
    if (confirm("Confirm that you want this vehicle advertisement to be deleted.")) {
        await fetch('http://localhost:8080/Vehicle/Delete/' + id,
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
};

async function updateVehicle(id) {
    event.preventDefault();
    const form = document.getElementById('update-form');
const formData = new FormData(form);
    let jsonData = {};
    for (let [key, value] of formData.entries()) {
        jsonData[key] = value;
    }

    await fetch('/Vehicle/Update/' + id, {
        method: "PUT",
        body: JSON.stringify(jsonData),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(data => {
            window.location.href = '/Vehicle/UserVehicles';
        });
}