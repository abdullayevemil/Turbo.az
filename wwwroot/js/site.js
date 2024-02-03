document.getElementById("logout-link").addEventListener("click", async (e) => {
    e.preventDefault();
    await fetch('http://localhost:8080/Identity/LogOut', {
        method: 'DELETE'
    }).then(res => window.location.href = "http://localhost:8080/Identity/Login");
});