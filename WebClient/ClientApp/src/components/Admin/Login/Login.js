import React from 'react';
import { connect } from 'react-redux';
import Form from 'react-validation/build/form';
import Input from 'react-validation/build/input';
import CheckButton from 'react-validation/build/button';
import { maxLength, required } from '../../../helpers/ValidatorTypes';
import { LoadingOverlay, Loader } from 'react-overlay-loader';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../../../store/Auth';

import 'react-overlay-loader/styles.css';

const maxLength50 = (value) => {
    return maxLength(value, 50);
};

class Login extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            username: "",
            password: "",
            loading: false,
            loginMessage: ""
        };
        
        this.handleChange = this.handleChange.bind(this);
        this.handleFormSubmit = this.handleFormSubmit.bind(this);
    }

    componentWillReceiveProps(nextProps) {
        this.setState({ loading: false, loginMessage: nextProps.loginMessage });
    }

    handleChange(event) {
        let { name, value } = event.target;
        this.setState({ [name]: value });
    }

    handleFormSubmit(event) {
        event.preventDefault();
        this.form.validateAll();
        let { username, password } = this.state;
        if (this.checkBtn.context._errors.length === 0 ) {
            this.setState({ loading: true });
            this.props.requestLogin(username, password);
        }
    }

    render() {
        let { username, password, loginMessage } = this.state;
        let loading = this.props.isLoggingIn;
        return (
            <div className="hold-transition login-page">
                <div className="login-box">
                    <div className="login-logo">
                        <a><b>BIÊN LAI ĐIỆN TỬ</b><br />QUẬN HẢI CHÂU</a>
                    </div>
                    <LoadingOverlay >
                        <div className="login-box-body">
                            <p className="login-box-msg">Đăng nhập tài khoản để bắt đầu</p>
                            <Form onSubmit={this.handleFormSubmit} ref={c => this.form = c}>
                                <div className="form-group has-feedback">
                                    <span className="glyphicon glyphicon-envelope form-control-feedback" />
                                    <Input
                                        name="username"
                                        onChange={this.handleChange}
                                        type="text"
                                        placeholder="Tên đăng nhập"
                                        className="form-control"
                                        value={username}
                                        validations={[required, maxLength50]}
                                    />
                                </div>
                                <div className="form-group has-feedback">
                                    <span className="glyphicon glyphicon-lock form-control-feedback" />
                                    <Input
                                        name="password"
                                        onChange={this.handleChange}
                                        type="password"
                                        placeholder="Mật khẩu"
                                        className="form-control"
                                        value={password}
                                        validations={[required, maxLength50]}
                                    />
                                </div>
                                {
                                    loginMessage ?
                                        <span className="form-text text-danger">{loginMessage}</span> :
                                        null
                                }
                                <div className="row">
                                    <div className="col-xs-4 col-xs-offset-8">
                                        <button type="submit" className="btn btn-primary btn-block btn-flat">Đăng nhập</button>
                                        <CheckButton style={{ display: 'none' }} ref={c => { this.checkBtn = c; }} />
                                    </div>
                                </div>
                            </Form>
                        </div>
                        <Loader loading={loading} />
                    </LoadingOverlay>
                </div>
            </div>
        );
    }
}

export default connect(
    state => {
        const { auth } = state;
        if (auth) {
            return { user: auth.user, loginMessage: auth.loginMessage, isLoggingIn: auth.isLoggingIn };
        }

        return { user: null };
    },
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Login);