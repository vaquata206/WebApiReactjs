import React from 'react';
import axios from 'axios';
import { bindActionCreators } from 'redux';
import { store } from "../../store/store";
import history from "../../store/history";
import { ApiPaths } from "../../helpers/api";
import { dateHelper } from "../../helpers/utils";
import { LoadingOverlay, Loader } from 'react-overlay-loader';
import { Row, Col, Tabs, Tab, Button, Table } from 'react-bootstrap';
import Form from 'react-validation/build/form';
import Input from 'react-validation/build/input';
import CheckButton from 'react-validation/build/button';
import DatePicker from 'react-datepicker';
import { required, maxLength, maxLength50, maxLength100, maxLength200 } from "./../../helpers/ValidatorTypes";
import { actionCreators as creatorConfirmationModal } from "../../store/ConfirmationModal";
import { actionCreators as creatorAdminAlert } from "../../store/AdminAlert";
import CbbDepartments from "../Utils/CbbDepartments";
import AccountManagement from "./AccountManagement";

import "react-datepicker/dist/react-datepicker.css";
import 'react-overlay-loader/styles.css';

const maxLength10 = (value) => {
    return maxLength(value, 10);
};

class EmployeeDetail extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            employee: {
                nam_Sinh: new Date(),
                ngayCap_CMND: new Date()
            },
            isCreate: true
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleChangeBirthday = this.handleChangeBirthday.bind(this);
        this.clickSaveEmployee = this.clickSaveEmployee.bind(this);
        this.setStateEmployee = this.setStateEmployee.bind(this);
        this.boundActionCreators = bindActionCreators({ ...creatorConfirmationModal, ...creatorAdminAlert }, store.dispatch);
    }

    componentWillMount() {
        var code = this.props.match.params.code;
        if (code) {
            this.setState({ loading: true });
            axios.get(ApiPaths.GetEmployeeByCode + "?code=" + code).then(response => {
                this.setStateEmployee(response.data);
            }).catch(error => {
                let message = typeof error.response.data === "string" ? error.response.data : "Không thể lấy thông tin nhân viên này";
                this.boundActionCreators.showAlert({
                    variant: "danger",
                    content: message
                });

                this.setState({ isCreate: true });
            }).then(() => {
                this.setState({ loading: false });
            });
        }
    }

    setStateEmployee(data) {
        this.setState({
            employee: {
                ...data,
                nam_Sinh: new Date(data.nam_Sinh),
                ngayCap_CMND: new Date(data.ngayCap_CMND)
            },
            isCreate: false
        });
    }

    handleChange(event) {
        let { name, value } = event.target;
        let { employee } = this.state;
        this.setState(
            {
                employee: {
                    ...employee,
                    [name]: value
                }
            }
        );
    }

    handleChangeBirthday(date) {
        let { employee } = this.state;
        this.setState(
            {
                employee: {
                    ...employee,
                    namSinh: date
                }
            }
        );
    }

    clickSaveEmployee(event) {
        event.preventDefault();
        const { employee, isCreate } = this.state;
        var department = this.cbbDepartment.getSelected();
        this.form.validateAll();
        if (this.checkBtn.context._errors.length === 0) {
            this.boundActionCreators.showModal({
                title: "Lưu nhân viên",
                body: <span>Bạn có muốn <strong>{isCreate ? "khởi tạo" : "cập nhật"}</strong> nhân viên <strong>{employee.ho_Ten || ""}</strong> không?</span>,
                okButton: {
                    title: isCreate? "Khởi tạo" : "Cập nhật",
                    handle: () => {
                        this.boundActionCreators.hideModal();
                        this.setState({ loading: true });
                        axios.post(ApiPaths.SaveEmployee, {
                            MaNhanVien: employee.ma_NhanVien,
                            HoTen: employee.ho_Ten,
                            DiaChi: employee.dia_Chi,
                            DienThoai: employee.dien_Thoai,
                            Email: employee.email,
                            NamSinh: dateHelper.FormatDate(employee.nam_Sinh),
                            SoCMND: employee.so_CMND,
                            NgayCapCMND: dateHelper.FormatDate(employee.ngayCap_CMND),
                            NoiCapCMND: employee.noiCap_CMND,
                            Ma_DonVi: department.ma_DonVi,
                            Chuc_Vu: employee.chuc_Vu,
                            GhiChu: employee.ghi_Chu
                        }).then(response => {
                            this.boundActionCreators.showAlert({
                                variant: "success",
                                content: <p><strong>{isCreate ? "Khởi tạo" : "Cập nhật"}</strong> nhân viên <strong>{response.data.ho_Ten}</strong> thành công.</p>
                            });
                            if (isCreate) {
                                history.replace("/employee/detail/" + response.data.ma_NhanVien);
                            }
                            this.setStateEmployee(response.data);
                        }).catch(error => {
                            let message = typeof error.response.data === "string" ? error.response.data : "";
                            this.boundActionCreators.showAlert({
                                variant: "danger",
                                content: (isCreate ? "Khởi tạo" : "Cập nhật") + " nhân viên không thành công. " + message
                            });
                        }).then(() => {
                            this.setState({ loading: false });
                        });
                    }
                }
            });
        }
    }

    render() {
        const { loading, employee, isCreate } = this.state;
        return (
            <LoadingOverlay>
                <section className="content">
                    <Row>
                        <Col>
                            <div className="nav-tabs-custom">
                                <Tabs defaultActiveKey="content">
                                    <Tab eventKey="content" title="Thông tin nhân viên">
                                        <div className="tab-content">
                                            <div className="box-body">
                                                <Form ref={c => this.form = c}>
                                                    <Row>
                                                        <Col>
                                                            <div className="form-group row">
                                                                <label htmlFor="ma_NhanVien" className="col-sm-3 col-form-label">Mã nhân viên:</label>
                                                                <div className="col-sm-9">
                                                                    <Input
                                                                        type="text"
                                                                        className="form-control"
                                                                        placeholder="Mã nhân viên"
                                                                        name="ma_NhanVien"
                                                                        disabled
                                                                        value={employee.ma_NhanVien || ""}
                                                                    />
                                                                </div>
                                                            </div>
                                                            <div className="form-group row">
                                                                <label htmlFor="ho_Ten" className="col-sm-3 col-form-label">Họ tên: <span className="text-red">*</span></label>
                                                                <div className="col-sm-9">
                                                                    <Input
                                                                        type="text"
                                                                        className="form-control"
                                                                        placeholder="Họ tên"
                                                                        name="ho_Ten"
                                                                        value={employee.ho_Ten || ""}
                                                                        onChange={this.handleChange}
                                                                        validations={[required, maxLength100]}
                                                                    />
                                                                </div>
                                                            </div>
                                                            <div className="form-group row">
                                                                <label htmlFor="dia_Chi" className="col-sm-3 col-form-label">Địa chỉ: <span className="text-red">*</span></label>
                                                                <div className="col-sm-9">
                                                                    <Input
                                                                        type="text"
                                                                        className="form-control"
                                                                        placeholder="Địa chỉ"
                                                                        name="dia_Chi"
                                                                        value={employee.dia_Chi || ""}
                                                                        onChange={this.handleChange}
                                                                        validations={[required, maxLength200]}
                                                                    />
                                                                </div>
                                                            </div>
                                                            <div className="form-group row">
                                                                <label htmlFor="nam_Sinh" className="col-sm-3 col-form-label">Ngày sinh: <span className="text-red">*</span></label>
                                                                <div className="col-sm-9">
                                                                    <DatePicker
                                                                        dateFormat="dd/MM/yyyy"
                                                                        selected={employee.nam_Sinh}
                                                                        className="form-control"
                                                                        maxDate={new Date()}
                                                                        onChange={this.handleChangeBirthday}
                                                                    />
                                                                </div>
                                                            </div>
                                                            <div className="form-group row">
                                                                <label htmlFor="dien_Thoai" className="col-sm-3 col-form-label">Điện thoại: <span className="text-red">*</span></label>
                                                                <div className="col-sm-9">
                                                                    <Input
                                                                        type="text"
                                                                        className="form-control"
                                                                        placeholder="Điện thoại"
                                                                        name="dien_Thoai"
                                                                        value={employee.dien_Thoai || ""}
                                                                        onChange={this.handleChange}
                                                                        validations={[required, maxLength50]}
                                                                    />
                                                                </div>
                                                            </div>
                                                            <div className="form-group row">
                                                                <label htmlFor="email" className="col-sm-3 col-form-label">Email: <span className="text-red">*</span></label>
                                                                <div className="col-sm-9">
                                                                    <Input
                                                                        type="text"
                                                                        className="form-control"
                                                                        placeholder="Email"
                                                                        name="email"
                                                                        value={employee.email || ""}
                                                                        onChange={this.handleChange}
                                                                        validations={[required, maxLength50]}
                                                                    />
                                                                </div>
                                                            </div>
                                                        </Col>
                                                        <Col>
                                                            <div className="form-group row">
                                                                <label htmlFor="id_DonVi" className="col-sm-3 col-form-label">Đơn vị: <span className="text-red">*</span></label>
                                                                <div className="col-sm-9">
                                                                    <CbbDepartments className="form-control" name="id_DonVi" value={employee.id_DonVi} ref={c => { this.cbbDepartment = c; }} onChange={this.handleChange} />
                                                                </div>
                                                            </div>
                                                            <div className="form-group row">
                                                                <label className="col-sm-3 col-form-label">Chức vụ: <span className="text-red">*</span></label>
                                                                <div className="col-sm-9">
                                                                    <select name="chuc_Vu" className="form-control" value={employee.chuc_Vu || 1} onChange={this.handleChange}>
                                                                        <option value="1">Nhân viên</option>
                                                                        <option value="2">Admin</option>
                                                                        <option value="3">Giám đốc</option>
                                                                    </select>
                                                                </div>
                                                            </div>
                                                            <div className="form-group row">
                                                                <label htmlFor="so_CMND" className="col-sm-3 col-form-label">Số CMND: <span className="text-red">*</span></label>
                                                                <div className="col-sm-9">
                                                                    <Input
                                                                        type="text"
                                                                        className="form-control"
                                                                        placeholder="Số chứng minh nhân dân"
                                                                        name="so_CMND"
                                                                        value={employee.so_CMND || ""}
                                                                        onChange={this.handleChange}
                                                                        validations={[required, maxLength10]}
                                                                    />
                                                                </div>
                                                            </div>
                                                            <div className="form-group row">
                                                                <label htmlFor="noiCap_CMND" className="col-sm-3 col-form-label">Nơi cấp: <span className="text-red">*</span></label>
                                                                <div className="col-sm-9">
                                                                    <Input
                                                                        type="text"
                                                                        className="form-control"
                                                                        placeholder="Nơi cấp chứng minh nhân dân"
                                                                        name="noiCap_CMND"
                                                                        value={employee.noiCap_CMND || ""}
                                                                        onChange={this.handleChange}
                                                                        validations={[required, maxLength200]}
                                                                    />
                                                                </div>
                                                            </div>
                                                            <div className="form-group row">
                                                                <label htmlFor="ngayCapCMND" className="col-sm-3 col-form-label">Ngày cấp: <span className="text-red">*</span></label>
                                                                <div className="col-sm-9">
                                                                    <DatePicker
                                                                        dateFormat="dd/MM/yyyy"
                                                                        selected={employee.ngayCap_CMND}
                                                                        className="form-control"
                                                                        maxDate={new Date()}
                                                                    />
                                                                </div>
                                                            </div>
                                                            <div className="form-group row">
                                                                <label htmlFor="ghi_Chu" className="col-sm-3 col-form-label">Ghi chú: </label>
                                                                <div className="col-sm-9">
                                                                    <Input
                                                                        type="text"
                                                                        className="form-control"
                                                                        placeholder="Ghi chú"
                                                                        name="ghi_Chu"
                                                                        value={employee.ghi_Chu || ""}
                                                                        onChange={this.handleChange}
                                                                        validations={[maxLength200]}
                                                                    />
                                                                </div>
                                                            </div>
                                                            <CheckButton style={{ display: 'none' }} ref={c => { this.checkBtn = c; }} />
                                                        </Col>
                                                    </Row>
                                                </Form>
                                            </div>
                                            <div className="box-footer">
                                                <Row>
                                                    <Col>
                                                        <Button variant="danger">Xóa</Button>
                                                        <Button variant="primary" className="pull-right" onClick={this.clickSaveEmployee}>{isCreate? "Khởi tạo": "Cập nhật"}</Button>
                                                    </Col>
                                                </Row>
                                            </div>
                                        </div>
                                    </Tab>
                                    {!isCreate ?
                                        <Tab eventKey="account" title="Thông tin tài khoản">
                                            <div className="tab-content">
                                                {employee.id_NhanVien ?
                                                    <AccountManagement employee={employee} /> : null}
                                            </div>
                                        </Tab> : null
                                        }
                                </Tabs>
                            </div>
                        </Col>
                    </Row>
                </section>
                <Loader loading={loading} />
            </LoadingOverlay>
        );
    }
}

export default EmployeeDetail;