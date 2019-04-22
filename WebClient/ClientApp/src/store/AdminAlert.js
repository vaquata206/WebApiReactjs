const showAlertType = "SHOW_ALERT";
const hideAlertType = "HIDE_ALERT";

const initialState = {
    show: false,
    variant: "warning",
    content: ""
};

function showAlert(payload) {
    return {
        type: showAlertType,
        config: payload
    };
}

function hideAlert() {
    return {
        type: hideAlertType
    };
}

export const actionCreators = {
    showAlert: (config) => (dispatch) => {
        dispatch(showAlert(config));
    },
    hideAlert: () => (dispatch) => {
        dispatch(hideAlert());
    }
};

export const reducer = (state, action) => {
    state = state || initialState;
    switch (action.type) {
        case showAlertType:
            return {
                ...state,
                ...action.config,
                show: true
            };
        case hideAlertType:
            return {
                ...initialState
            };
        default:
            return state;
    }
};