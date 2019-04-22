import React from 'react';
import { isEmpty } from 'validator';

export const required = (value) => {
    if (isEmpty(value + "")) {
        return <span className="form-text text-danger">Trường này bắt buộc</span>;
    }
};

export const maxLength = (value, max = 50) => {
    if ((value||"").trim().length > max) {
        return <span className="form-text text-danger">Tối đa {max} kí tự</span>;
    }
};

export const maxLength50 = (value) => {
    return maxLength(value, 50);
};

export const maxLength100 = (value) => {
    return maxLength(value, 100);
};

export const maxLength200 = (value) => {
    return maxLength(value, 200);
};