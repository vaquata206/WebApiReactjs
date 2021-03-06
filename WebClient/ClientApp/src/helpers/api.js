﻿import axios from 'axios';
import { store } from '../store/store';
import { logout } from '../store/Auth';

export function configAxios() {
    // Add a request interceptor
    axios.interceptors.request.use(function (config) {
        const { auth } = store.getState();
        if (auth && auth.user && auth.user.token) {
            config.headers.common['Authorization'] = "Bearer " + auth.user.token;
        }

        return config;
    }, function (error) {
        return Promise.reject(error);
    });

    axios.interceptors.response.use(function (response) {
        return response;
    }, function (error) {
        if (error.response.status === 401) {
            store.dispatch(logout());
        }

        return Promise.reject(error);
    });
}

export const ApiPaths = {
    features: {
        getMenu: "api/feature/getmenu",
        getFeature: "api/feature/GetFeature",
        getFeatureNodes: "api/feature/GetAllNodes"
    },
    permissions: {
        GetAll: "api/permission/getAll",
        Get: "api/permission/get",
        Delete: "api/permission/delete",
        Save: "api/permission/save",
        GetUserPermissions: "api/permission/GetUserPermissions"
    },
    departments: {
        updateEmail: "api/department/updateEmail",
        delete: "api/department/delete",
        saveDepartment: "api/department/save",
        getDepartmentSelectItems: "api/department/getAllSelectItems",
        getDepartmentById: "api/department/get",
        getDepartmentByParentId: "api/department/getDepartmentsByParent",
        getChildNodes: "api/department/getChildNodes"
    },

    GetEmployeesByDepartmentId: "api/employee/GetByDepartmentId",
    GetEmployeeByCode: "api/employee/Get",
    SaveEmployee: "api/employee/save",
    DeleteEmployee: "api/employee/delete",

    GetAccountsByEmployeeId: "api/account/getByEmployeeId",
    CreateAccount: "api/account/create",
    ResetPassword: "api/account/resetPassword",
    DeleteAccount: "api/account/delete",
    GetTreeNodeAccounts: "api/account/GetTreeNodeAccounts",

    GetPermisisons: "api/permission/getAll",

    GetApps: "api/app/getAll",
    GetUserApps: "api/app/getUserApps",
    SetUserApps: "api/app/setPermission",
    SaveApp: "api/app/save",
    GetAppById: "api/app/get",
    DeleteApp: "api/app/delete"
};
