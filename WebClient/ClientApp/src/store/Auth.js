import storage from 'redux-persist/lib/storage';
import history from './history';
import axios from 'axios';

const requestLoginType = "REQUEST_LOGIN";
const requestLogoutType = "REQUEST_LOGOUT";
const responseLoginType = "RESPONSE_LOGIN";

const initialState = {
    user: null,
    loginMessage: null,
    isLoggingIn: false
};

function loginRequest() {
    return {
        type: requestLoginType,
        isLoggingIn: true
    };
}

function loginSuccess(payload) {
    return {
        type: responseLoginType,
        user: payload,
        loginMessage: "Thành công",
        isLoggingIn: false
    };
}

function loginFailed() {
    return {
        type: responseLoginType,
        user: null,
        loginMessage: "Tên đăng nhập hoặc mật khẩu không đúng",
        isLoggingIn: false
    };
}

export function logout() {
    return {
        type: requestLogoutType,
        user: null,
        loginMessage: null,
        isLoggingIn: false
    };
}

export const actionCreators = {
    requestLogin: (username, password) => (dispatch) => {
        dispatch(loginRequest());
        axios.post("/api/account/login", {
            username: username,
            password: password
        }).then(json => {
            dispatch(loginSuccess(json.data));
            history.push("/");
        }).catch(error => {
            dispatch(loginFailed());
        });
    },

    requestLogout: () => (dispatch) => {
        dispatch(logout());
        history.push("/login");
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    switch (action.type) {
        case responseLoginType:
            return {
                ...state,
                user: action.user,
                loginMessage: action.loginMessage,
                isLoggingIn: action.isLoggingIn
            };
        case requestLoginType:
            return {
                ...state,
                isLoggingIn: action.isLoggingIn
            };
        case requestLogoutType:
            return {
                ...state,
                user: action.user,
                loginMessage: action.loginMessage,
                isLoggingIn: action.isLoggingIn
            };
        default:
            return state;
    }
};

export const persistConfig = {
    key: 'auth',
    storage: storage,
    blacklist: ['isLoggingIn', 'loginMessage']
};