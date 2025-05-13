const apiUrl = "http://localhost:5187/login"; // Replace with your login API URL

document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const response = await fetch(apiUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ username, password })
    });

    if (response.ok) {
        const data = await response.json();
        // Save the token in a cookie
        document.cookie = `AuthToken=${data.token}; Secure; HttpOnly`;
        // Redirect to dashboard
        window.location.href = "/dashboard.html";
    } else {
        document.getElementById("errorMessage").style.display = "block";
    }
});