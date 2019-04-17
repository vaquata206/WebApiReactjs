const openMenuItemType = "OPEN_MENU_ITEM";
const closeMenuItemType = "CLOSE_MENU_ITEM";
const setMenuType = "SET_MENU";
const activeItemMapPathType = "ACTIVE_MENU_ITEM_MAP_PATH";

const initialState = {
    menuItems: []
};

function OpenMenuItem(payload) {
    return {
        type: openMenuItemType,
        menuOpen: payload
    };
}
function CloseMenuItem(payload) {
    return {
        type: closeMenuItemType,
        menuClose: payload
    };
}
function SetMenu(payload) {
    return {
        type: setMenuType,
        menuItems: payload
    };
}
function ActiveItemMapPath() {
    return {
        type: activeItemMapPathType,
    };
}
export const actionCreators = {
    OpenMenuItem: (key) => (dispatch) => {
        dispatch(OpenMenuItem(key));
    },
    CloseMenuItem: (key) => (dispatch) => {
        dispatch(CloseMenuItem(key));
    },
    SetMenu: (menuItems) => (dispatch) => {
        dispatch(SetMenu(menuItems));
    },
    ActiveItemMapPath: () => (dispatch) => {
        dispatch(ActiveItemMapPath());
    }
};

export const reducer = (state, action) => {
    state = state || initialState;

    switch (action.type) {
        case openMenuItemType:
            OpenMenu(state.menuItems, action.menuOpen);
            return {
                ...state
            };
        case closeMenuItemType:
            CloseMenu(state.menuItems, action.menuClose);
            return {
                ...state
            };
        case setMenuType:
            return {
                ...state,
                menuItems: action.menuItems
            };
        case activeItemMapPathType:
            ActiveMenuItems(state.menuItems);
            return {
                ...state
            };
        default:
            return state;
    }
};

function ActiveMenuItems(menu) {
    const paths = window.location.pathname.replace(/(^\/+)|(\/+$)/g, "").split("/");
    let controller;
    let actionName;
    let l = (paths || []).length;
    l = l > 2 ? 2 : l;
    switch (l) {
        case 2:
            actionName = paths[1] || null;
            break;
        case 1:
            controller = paths[0] || null;
            actionName = paths[1] || null;
            break;
        default:
            controller = null;
            actionName = null;
    }

    if (actionName === "index" || actionName === "#") {
        actionName = "";
    }

    if (!controller) {
        controller = "/";
    }

    SetupMenus(controller, actionName, menu);
}

function SetupMenus(controller, actionName, menu) {
    let isActived = false;
    (menu || []).forEach(value => {
        var childrenActived = false;
        if ((value.children || []).length > 0) {
            childrenActived = SetupMenus(controller, actionName, value.children);
        }

        value.actived = childrenActived || !(value.controler_Name !== controller || value.action_Name !== actionName);
        value.opened = childrenActived;

        isActived = isActived || value.actived;
    });

    return isActived;
}

function OpenMenu(menuItems, itemOpened) {
    (menuItems || []).forEach(item => {
        item.opened = item.m_ID === itemOpened;
    });
}

function CloseMenu(menuItems, itemClosed) {
    (menuItems || []).forEach(item => {
        if (item.m_ID === itemClosed) {
            item.opened = false;
        }
    });
}