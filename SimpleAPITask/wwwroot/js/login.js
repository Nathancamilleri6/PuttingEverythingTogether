async function loginUser() {
    const loginEmailTextBox = document.getElementById('login-email');
    const loginPasswordTextBox = document.getElementById('login-password');

    const item = {
        email: loginEmailTextBox.value.trim(),
        password: loginPasswordTextBox.value.trim()
    };

    await fetch('api/users/authenticate', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => {
            document.cookie = `email=${loginEmailTextBox.value.trim()}`
            loginEmailTextBox.value = '';
            loginPasswordTextBox.value = '';
            loadHomePage();
        })
        .catch(error => console.error('Unable to login user.', error));
}

async function refreshToken(token){
    await fetch('api/users/refresh', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(token)
    })
        .catch(error => console.error('Unable to refresh token.', error));
}

function loadHomePage(){
    window.location.assign("https://localhost:44300/home.html");
}