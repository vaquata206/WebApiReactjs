const showModalType = "SHOW_MODAL";
const hideModalType = "HIDE_MODAL";

const initialState = {
    show: false,
    title: "",
    body: "",
    cancelButton: {
        title: "Hủy",
        handle: null
    },
    okButton: {
        title: "Đồng ý",
        handle: null
    }
};

function showModal(payload) {
    return {
        type: showModalType,
        config: payload
    };
}

function hideModal() {
    return {
        type: hideModalType
    };
}

export const actionCreators = {
    showModal: (config) => (dispatch) => {
        dispatch(showModal(config));
    },
    hideModal: () => (dispatch) => {
        dispatch(hideModal());
    }
};

export const reducer = (state, action) => {
    state = state || initialState;
    switch (action.type) {
        case showModalType:
            return {
                ...state,
                ...action.config,
                show: true
            };
        case hideModalType:
            return {
                ...initialState
            };
        default:
            return state;
    }
};