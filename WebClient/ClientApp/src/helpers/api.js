import axios from 'axios';
import { checkStatus, parseJSON } from './utils';

export function setAuthorizationToken(token) {
    if (token) {
        axios.defaults.headers.common["Authorization"] = "Bearer " + token;
    } else {
        delete axios.defaults.headers.common["Authorization"];
    }
}

export function setDefaultAPI() {
    axios.interceptors.response.use(function (response) {
        return response;
    }, function (error) {
        debugger;
        return Promise.reject(error);
    });
}
