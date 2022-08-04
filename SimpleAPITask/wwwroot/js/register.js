function registerUser() {
    const registerUserNameTextBox = document.getElementById('register-userName');
    const registerEmailTextBox = document.getElementById('register-email');
    const registerPasswordTextBox = document.getElementById('register-password');

    const item = {
        userName: registerUserNameTextBox.value.trim(),
        email: registerEmailTextBox.value.trim(),
        password: registerPasswordTextBox.value.trim()
    };

    fetch('api/users', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then((response) => {
            registerUserNameTextBox.value = '';
            registerEmailTextBox.value = '';
            registerPasswordTextBox.value = '';

            console.log(response);

            if(response.status === 200){
                document.getElementById('errorname').innerHTML=""; 
                loadLoginPage();   
            } else {
                document.getElementById('errorname').innerHTML="Email already taken!";  
            }
        })
        .catch(error => console.log('Unable to register user.', error));
}

function loadLoginPage(){
    window.location.assign("https://localhost:44300/login.html");
}
