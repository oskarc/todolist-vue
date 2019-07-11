import { authHeader } from '../_helpers/auth-header';
import store from '../store';
import {User} from '../models/types'

export const userService = {
    login,
    logout,
    getAll,
    register
};
var url = 'https://localhost:44366/api';
function login(username, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    };

    return fetch(`${url}/users/authenticate`, requestOptions)
        .then(handleResponse)
        .then(user => {
            // login successful if there's a user in the response
            if (user) {
                // store user details and basic auth credentials in session storage 
                // to keep user logged in between page refreshes
                store.state.user['FirstName'] = user['firstName'];
                store.state.user['LastName'] = user['lastName'];
                store.state.user['UserName'] = user['userName'];
                store.state.user['Id'] = user['id'];
                store.state.user['Token'] = user['token'];
                store.state.isLoggedIn = true;
                store.state.Login = "Logga out";
                console.log(`user: ${store.state.user['FirstName']}`)
                sessionStorage.setItem('user', 'you are logged in');
            }
            console.log(user);
            return user;
        });
}

function register(firstname, lastname, username, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ firstname, lastname, username, password })
    };

    return fetch(`${url}/users/register`, requestOptions)
        .then(handleResponse)
        .then(
            console.log(requestOptions.body)
        );
}

function logout() {
    // remove user from local storage to log user out
    store.state.user = new User;
    store.state.isLoggedIn = false;
    store.state.Login = "Logga in";
    sessionStorage.removeItem('user');
}

function getAll() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${url}/users`, requestOptions).then(handleResponse);
}

function handleResponse(response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                logout();
                location.reload(true);
            }

            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });
}