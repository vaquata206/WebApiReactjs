import React from "react";
import axios from "axios";
import { Row, Col, Button, Table } from 'react-bootstrap';
import Form from 'react-validation/build/form';
import Input from 'react-validation/build/input';
import CheckButton from 'react-validation/build/button';
import { ApiPaths } from "../../helpers/api";
import { LoadingOverlay, Loader } from 'react-overlay-loader';
import { required, maxLength, maxLength50 } from "./../../helpers/ValidatorTypes";
import { modalHelper, alertHelper } from "./../../helpers/utils";

const maxLength30 = (value) => {
    return maxLength(value, 30);
};

const mapMatKhau = (value, props, components) => {
    if (value !== components["MatKhau"][0].value) {
        return <span className="form-text text-danger">Mật khẩu không khớp.</span>;
    }
};

class AccountManagement extends React.Component{
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            accounts: [],
            newAccount: {}
        };

        this.createAccount = this.createAccount.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.deleteAccount = this.deleteAccount.bind(this);
        this.resetPassword = this.resetPassword.bind(this);
    }

    componentWillMount() {
        this.setState({ loading: true });

        axios.get(ApiPaths.GetAccountsByEmployeeId + "?id=" + this.props.employee.id_NhanVien).then(response => {
            this.setState({
                accounts: response.data,
                loading: false
            });
        });
    }

    createAccount() {
        const { newAccount } = this.state;
        const employee = this.props;
        this.form.validateAll();
        if (this.checkBtn.context._errors.length === 0) {
            modalHelper.show({
                title: "Tạo tài khoản",
                body: <p>Bạn có muốn tạo tài khoản <strong>{newAccount.UserName}</strong> không.</p>,
                okButton: {
                    title: "Tạo tài khoản",
                    handle: () => {
                        modalHelper.hide();
                        this.setState({ loading: true });
                        axios.post(ApiPaths.CreateAccount, {
                            UserName: newAccount.UserName,
                            MatKhau: newAccount.MatKhau,
                            Ma_NhanVien: employee.employee.ma_NhanVien
                        }).then((response) => {
                            let accounts = this.state.accounts;
                            accounts.push(response.data);
                            this.setState({ accounts: accounts, newAccount: {} });
                            alertHelper.show({
                                variant: "success",
                                content: <p className="mb-0">Tạo tài khoản <strong>{newAccount.UserName}</strong> thành công.</p>
                            });
                            }).catch(error => {
                                let message = typeof error.response.data === "string" ? error.response.data : "";
                            alertHelper.show({
                                variant: "danger",
                                content: "Tạo tài khoản " + newAccount.UserName + " không thành công." + message
                            });
                        }).then(() => {
                            this.setState({ loading: false });
                        });
                    }
                }
            });
        }
    }

    deleteAccount(account) {
        modalHelper.show({
            title: "Xóa tài khoản",
            body: <p>Bạn có muốn xóa tài khoản <strong>{account.userName}</strong> không.</p>,
            okButton: {
                title: "Xóa",
                handle: () => {
                    modalHelper.hide();
                    this.setState({ loading: true });
                    axios.get(ApiPaths.DeleteAccount + "?code=" + account.ma_NguoiDung).then(response => {
                        let { accounts } = this.state;
                        var index = accounts.indexOf(account);

                        if (index >= 0) {
                            accounts.splice(index, 1);
                        }

                        this.setState({ accounts: accounts });

                        alertHelper.show({
                            variant: "success",
                            content: <p className="mb-0">Xóa tài khoản <strong>{account.userName}</strong> thành công.</p>
                        });
                    }).catch(error => {
                        let message = typeof error.response.data === "string" ? error.response.data : "";
                        alertHelper.show({
                            variant: "danger",
                            content: <p className="mb-0">Xóa tài khoản <strong>{account.userName}</strong> không thành công. {message}</p>
                        });
                    }).then(() => {
                        this.setState({ loading: false });
                    });
                }
            }
        });
    }

    resetPassword(account) {
        modalHelper.show({
            title: "Đặt lại mật khẩu",
            body: <p>Bạn có muốn đặt lại mật khẩu cho tài khoản <strong>{account.userName}</strong> không.</p>,
            okButton: {
                title: "Đặt lại",
                handle: () => {
                    modalHelper.hide();
                    this.setState({ loading: true });
                    axios.get(ApiPaths.ResetPassword + "?code=" + account.ma_NguoiDung).then(response => {
                        alertHelper.show({
                            variant: "success",
                            content: <p className="mb-0">Đặt lại mật khẩu tài khoản <strong>{account.userName}</strong> thành công. Mật khẩu mới lai: <strong>{response.data}</strong></p>
                        });
                    }).catch(error => {
                        let message = typeof error.data.response === "string" ? error.data.response : "";
                        alertHelper.show({
                            variant: "danger",
                            content: <p className="mb-0">Đặt lại mật khẩu tài khoản <strong>{account.userName}</strong> không thành công. {message}</p>
                        });
                    }).then(() => {
                        this.setState({ loading: false });
                    });
                }
            }
        });
    }

    handleChange(event) {
        let { name, value } = event.target;
        let { newAccount } = this.state;
        this.setState(
            {
                newAccount: {
                    ...newAccount,
                    [name]: value
                }
            }
        );
    }

    render() {
        const { loading, accounts, newAccount } = this.state;
        return (
            <div className="box-body">
                <LoadingOverlay>
                    <Row>
                        <Col>
                            <div className="box box-success">
                                <div className="box-header">
                                    Tạo tài khoản
                                </div>
                                <div className="box-body">
                                    <Form ref={c => { this.form = c; }}>
                                        <Row>
                                            <Col>
                                                <div className="form-group row">
                                                    <label htmlFor="UserName" className="col-sm-4 col-form-label">Tài khoản:</label>
                                                    <div className="col-sm-8">
                                                        <Input
                                                            type="text"
                                                            name="UserName"
                                                            className="form-control"
                                                            placeholder="Tài khoản"
                                                            validations={[required, maxLength30]}
                                                            value={newAccount.UserName || ""}
                                                            onChange={this.handleChange}
                                                        />
                                                    </div>
                                                </div>
                                            </Col>
                                            <Col>
                                                <div className="form-group row">
                                                    <label htmlFor="MatKhau" className="col-sm-4 col-form-label">Mật khẩu:</label>
                                                    <div className="col-sm-8">
                                                        <Input
                                                            ref={c => { this.password = c; }}
                                                            type="password"
                                                            name="MatKhau"
                                                            className="form-control"
                                                            placeholder="Mật khẩu"
                                                            validations={[required, maxLength50]}
                                                            value={newAccount.MatKhau || ""}
                                                            onChange={this.handleChange}
                                                        />
                                                    </div>
                                                </div>
                                            </Col>
                                            <Col>
                                                <div className="form-group row">
                                                    <label htmlFor="NhapLai" className="col-sm-4 col-form-label">Nhập lại:</label>
                                                    <div className="col-sm-8">
                                                        <Input
                                                            type="password"
                                                            name="NhapLai"
                                                            className="form-control"
                                                            placeholder="Nhập lại mật khẩu"
                                                            validations={[required, maxLength50, mapMatKhau]}
                                                            value={newAccount.NhapLai || ""}
                                                            onChange={this.handleChange}
                                                        />
                                                    </div>
                                                </div>
                                            </Col>
                                        </Row>
                                        <CheckButton style={{ display: 'none' }} ref={c => { this.checkBtn = c; }} />
                                    </Form>
                                </div>
                                <div className="box-footer">
                                    <Button className="pull-right" variant="primary" onClick={this.createAccount}>Tạo tài khoản</Button>
                                </div>
                            </div>
                            <Table striped bordered hover size="sm">
                                <thead>
                                    <tr>
                                        <th>STT</th>
                                        <th>Mã tài khoản</th>
                                        <th>Tên đăng nhập</th>
                                        <th>Ngày tạo</th>
                                        <th>Ngày đăng nhập</th>
                                        <th>Mật khẩu</th>
                                        <th>Xóa</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {
                                        (accounts || []).length > 0 ?
                                            accounts.map((value, index) => (
                                                <tr key={value.id_NguoiDung}>
                                                    <td>{index + 1}</td>
                                                    <td>{value.ma_NguoiDung}</td>
                                                    <td>{value.userName}</td>
                                                    <td>{value.ngay_KhoiTao}</td>
                                                    <td>{value.ngay_Login}</td>
                                                    <td><Button variant="success" onClick={() => this.resetPassword(value)}>Đặt lại</Button></td>
                                                    <td><Button variant="danger" onClick={() => this.deleteAccount(value)}>Xóa</Button></td>
                                                </tr>)
                                            ) : <tr style={{ textAlign: "center" }}><td colSpan="7">Không có dữ liệu</td></tr>
                                    }
                                </tbody>
                            </Table>
                        </Col>
                    </Row>
                    <Loader loading={loading} />
                </LoadingOverlay>
            </div>
            );
    }
}

export default AccountManagement;