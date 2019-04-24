import { bindActionCreators } from "redux";
import { store } from "./../store/store";
import { actionCreators as creatorConfirmationModal } from "./../store/ConfirmationModal";
import { actionCreators as creatorAdminAlert } from "./../store/AdminAlert";

export function checkStatus(response) {
    if (!response.ok) {   // (response.status < 200 || response.status > 300)
        const error = new Error(response.statusText);
        error.response = response;
        throw error;
    }
    return response;
}

export function parseJSON(response) {
    return response.json();
}

export const dateHelper = {
    FormatDate: (date, format) => {
        if (!format) {
            format = "dd/MM/yyyy";
        }

        return format.replace(/dd/, ("0" + date.getDate()).slice(-2)).replace(/MM/, ("0" + (date.getMonth() + 1)).slice(-2)).replace(/yyyy/, date.getFullYear());
    },
    ConvertStringToDate: (date, format) => {
        let d = date.split(/[.,/ -]/);
        let f = format.split(/[.,/ -]/);
        return new Date(d[f.indexOf("yyyy")], d[f.indexOf("MM")], d[f.indexOf("dd")]);
    }
};

export const modalHelper = {
    show: (config) => {
        var boundActionCreators = bindActionCreators(creatorConfirmationModal, store.dispatch);
        return boundActionCreators.showModal(config);
    },
    hide: () => {
        var boundActionCreators = bindActionCreators(creatorConfirmationModal, store.dispatch);
        return boundActionCreators.hideModal();
    }
};

export const alertHelper = {
    show: (config) => {
        var boundActionCreators = bindActionCreators(creatorAdminAlert, store.dispatch);
        return boundActionCreators.showAlert(config);
    },
    hide: () => {
        var boundActionCreators = bindActionCreators(creatorAdminAlert, store.dispatch);
        return boundActionCreators.hideAlert();
    },
    showError: (error, message) => {
        const m = typeof error.response.data === "string" ? error.response.data : "";
        var config = {
            variant: "danger",
            content: message + "." + m
        };

        var boundActionCreators = bindActionCreators(creatorAdminAlert, store.dispatch);
        return boundActionCreators.showAlert(config);
    }
};