const requestLoginType = "REQUEST_LOGIN";
const requestLogout = "REQUEST_LOGOUT";

const initialState = {
    userId: null,
    token: null,
    userName: null
};

function loginSuccess(payload) {
    return {
        type: requestLoginType,
        user: payload
    };
}

function loginFailed() {
    return {
        type: requestLoginType,
        user: null
    };
}

export const actionCreators = {
    requestLogin: (username, password) => (dispatch) => {
        if (username === "superadmin" && password === "123456") {
            dispatch(loginSuccess({ ...initialState, userName: "admin", userId: 1 }));
        } else {
            dispatch(loginFailed());
        }
    }
};

export const reducer = (state, action) => {
    state = state || { user: initialState };

    switch (action.type) {
        case requestLoginType:
            return {
                ...state,
                user: action.user
            };

        default:
            return state;
    }
};