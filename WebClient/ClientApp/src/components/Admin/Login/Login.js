import React from 'react';
import Validator from 'react-forms-validator';
import { LoadingOverlay, Loader } from 'react-overlay-loader';
import 'react-overlay-loader/styles.css';

class Login extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            username: "",
            password: "",
            isFormValidationErrors: true,
            submitted: false,
            loading: false
        };
        console.log("adasd");
        this.handleChange = this.handleChange.bind(this);
        this.handleFormSubmit = this.handleFormSubmit.bind(this);
        this.isValidationError = this.isValidationError.bind(this);
    }

    handleChange(event) {
        let { name, value } = event.target;
        this.setState({ [name]: value });
        let { submitted } = this.state;
    }

    isValidationError(flag) {
        this.setState({ isFormValidationErrors: flag });
    }

    handleFormSubmit(event) {
        event.preventDefault();
        this.setState({ submitted: true });
        let { username, password, isFormValidationErrors } = this.state;
        if (!isFormValidationErrors) {
            this.setState({ loading: true });
            //you are ready to perform your action here like dispatch
            // let { dispatch, login } = this.props;
            // dispatch( login( { params:{},data:{ contact_no, password } } ) );
        }
    }

    render() {
        let { username, password, submitted, loading } = this.state;

        return (
            <div className="hold-transition login-page">
                <div className="login-box">
                    <div className="login-logo">
                        <a><b>BIÊN LAI ĐIỆN TỬ</b><br />QUẬN HẢI CHÂU</a>
                    </div>
                    <LoadingOverlay >
                        <div className="login-box-body">
                            <p className="login-box-msg">Đăng nhập tài khoản để bắt đầu</p>
                            <form noValidate onSubmit={this.handleFormSubmit}>

                                <div className="form-group has-feedback">
                                    <span className="glyphicon glyphicon-envelope form-control-feedback" />
                                    <input type="text" className="form-control" placeholder="Tên đăng nhập" name="username" value={username} onChange={this.handleChange} />
                                    <Validator
                                        isValidationError={this.isValidationError}
                                        isFormSubmitted={submitted}
                                        reference={{ username: username }}
                                        validationRules={{ required: true, maxLength: 50 }}
                                        validationMessages={{ required: "Trường này bắt buộc", maxLength: "Tối đa 50 kí tự" }}
                                    />
                                </div>
                                <div className="form-group has-feedback">
                                    <span className="glyphicon glyphicon-lock form-control-feedback" />
                                    <input type="password" className="form-control" placeholder="Mật khẩu" name="password" value={password} onChange={this.handleChange} />
                                    <Validator
                                        isValidationError={this.isValidationError}
                                        isFormSubmitted={submitted}
                                        reference={{ password: password }}
                                        validationRules={{ required: true, maxLength: 50 }}
                                        validationMessages={{ required: "Trường này bắt buộc", maxLength: "Tối đa 50 kí tự" }}
                                    />
                                </div>
                                <div className="row">
                                    <div className="col-xs-4 col-xs-offset-8">
                                        <button type="submit" className="btn btn-primary btn-block btn-flat">Đăng nhập</button>
                                    </div>
                                </div>
                            </form>
                        </div>
                        <Loader loading={loading} />
                    </LoadingOverlay>
                </div>
            </div>
        );
    }
}

export default Login;