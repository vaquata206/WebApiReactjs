import React, { Component } from 'react';
import { isEmpty } from 'validator';

export const required = (value) => {
    if (isEmpty(value)) {
        return (<span className="form-text text-danger">Trường này bắt buộc</span>);
    }
};

export const maxLength = (value, max = 50) => {
    if (value.trim().length > max) {
        return (<span className="form-text text-danger">Tối đa {max} kí tự</span>);
    }
};