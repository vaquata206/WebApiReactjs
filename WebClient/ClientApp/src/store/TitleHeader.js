const setTitleType = "SET_TITLE";

const initialState = {
    title: ""
};

function setTitle(title) {
    return {
        type: setTitleType,
        title: title
    };
}

export const actionCreators = {
    setTitle: (title) => (dispatch) => {
        dispatch(setTitle(title));
    }
};

export const reducer = (state, action) => {
    state = state || initialState;
    switch (action.type) {
        case setTitleType:
            return {
                ...state,
                title: action.title
            };
        default:
            return state;
    }
};