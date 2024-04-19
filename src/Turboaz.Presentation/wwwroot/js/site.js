document.getElementById("logout-link").addEventListener("click", async (e) => {
    e.preventDefault();

    if (confirm("Confirm that you want to log out.")) {
        await fetch('http://localhost:8080/Identity/LogOut', {
            method: 'DELETE'
        }).then(res => window.location.href = "http://localhost:8080/Identity/Login")
    }
});