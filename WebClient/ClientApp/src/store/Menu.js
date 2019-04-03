const openMenuItemType = "OPEN_MENU_ITEM";
const closeMenuItemType = "CLOSE_MENU_ITEM";
const activeMenuItemsType = "ACTIVE_MENU_ITEMS";

const initialState = {
    menuOpen: null,
    activedItems: [0]
};

function OpenMenuItem(payload) {
    return {
        type: openMenuItemType,
        menuOpen: payload
    };
}

function CloseMenuItem() {
    return {
        type: closeMenuItemType
    };
}

function ActiveMenuItems(payload) {
    return {
        type: activeMenuItemsType,
        activedItems: payload || [0]
    };
}

export const actionCreators = {
    OpenMenuItem: (key) => (dispatch) => {
        dispatch(OpenMenuItem(key));
    },
    CloseMenuItem: () => (dispatch) => {
        dispatch(CloseMenuItem());
    },
    ActiveMenuItems: (items) => (dispatch) => {
        dispatch(ActiveMenuItems(items));
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    switch (action.type) {
        case openMenuItemType:
            return {
                ...state,
                menuOpen: action.menuOpen
            };
        case closeMenuItemType:
            return {
                ...state,
                menuOpen: null
            };
        case activeMenuItemsType:
            return {
                ...state,
                activedItems: action.activedItems
            };
        default:
            return state;
    }
};