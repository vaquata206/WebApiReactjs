import axios from 'axios';
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
    GetDepartmentByParentId: "api/department/getChildNodes",
    GetDepartmentById: "api/department/get",
    GetDepartmentSelectItems: "api/department/getAllSelectItems",
    SaveDepartment: "api/department/save",
    UpdateEmailDepartment: "api/department/updateEmail"
};
