import React from 'react';
import axios from 'axios';
import { bindActionCreators } from 'redux';
import { store } from "../../store/store";
import history from "../../store/history";
import { ApiPaths } from "../../helpers/api";
import { Tabs, Tab, Container, Row, Col, Button } from 'react-bootstrap';
import Form from 'react-validation/build/form';
import Input from 'react-validation/build/input';
import CheckButton from 'react-validation/build/button';
import { required, maxLength, maxLength50, maxLength100, maxLength200 } from "./../../helpers/ValidatorTypes";
import CbbDepartments from "./../Utils/CbbDepartments";
import { actionCreators as creatorConfirmationModal } from "../../store/ConfirmationModal";
import { actionCreators as creatorAdminAlert } from "../../store/AdminAlert";
import { LoadingOverlay, Loader } from 'react-overlay-loader';

const maxLength32 = (value) => {
    return maxLength(value, 32);
};
const maxLength40 = (value) => {
    return maxLength(value, 40);
};

class DepartmentDetail extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            department: {},
            isCreate: true
        };
        this.handleChange = this.handleChange.bind(this);
        this.clickSaveDepartment = this.clickSaveDepartment.bind(this);
        this.clickUpdateEmail = this.clickUpdateEmail.bind(this);
        this.deleteDepartment = this.deleteDepartment.bind(this);
        this.boundActionCreators = bindActionCreators({ ...creatorConfirmationModal, ...creatorAdminAlert }, store.dispatch);
    }

    componentWillMount() {
        const id = this.props.match.params.id;
        if (id) {
            this.setState({ loading: true });
            axios.get(ApiPaths.GetDepartmentById + "?id=" + id).then(response => {
                this.setState({ department: response.data, isCreate: false });
            }).catch(error => {
                this.boundActionCreators.showAlert({
                    variant: "danger",
                    content: error.response.data
                });
                this.setState({ isCreate: true });
            }).then(() => {
                this.setState({ loading: false });
            });
        }
    }

    handleChange(event) {
        let { name, value } = event.target;
        let { department } = this.state;
        this.setState(
            {
                department: {
                    ...department,
                    [name]: value
                }
            }
        );
    }

    clickSaveDepartment(event) {
        event.preventDefault();
        const { department, isCreate } = this.state;
        var parent = this.cbbDepartments.getSelected() || {};
        this.form.validateAll();
        if (this.checkBtn.context._errors.length === 0) {
            this.boundActionCreators.showModal({
                title: (isCreate ? "Khởi tạo" : "Cập nhật") + " đơn vị",
                body: <span>Bạn có muốn {isCreate? "khởi tạo": "cập nhật"} đơn vị <strong>{department.ten_DonVi || ""}</strong> không?</span>,
                cancelButton: {
                    title: "Hủy"
                },
                okButton: {
                    title: isCreate? "Khởi tạo": "Cập nhật",
                    handle: () => {
                        this.boundActionCreators.hideModal();
                        this.setState({ loading: true });
                        axios.post(ApiPaths.SaveDepartment, {
                            Ma_DonVi: department.ma_DonVi,
                            Ten_DonVi: department.ten_DonVi,
                            Dia_Chi: department.dia_Chi,
                            MaSoThue: department.maSoThue,
                            Dien_Thoai: department.dien_Thoai,
                            Website: department.website,
                            TenNguoi_DaiDien: department.tenNguoi_DaiDien,
                            Loai_DonVi: 0,
                            Ma_DV_Cha: parent.ma_DonVi,
                            Cap_DonVi: 0,
                            Ghi_chu: department.ghi_Chu
                        }).then(response => {
                            this.setState({ department: response.data, isCreate: false });
                            this.boundActionCreators.showAlert({
                                variant: "success",
                                content: <p className="mb-0">{isCreate ? "Khởi tạo" : "Cập nhật"} đơn vị <strong>{department.ten_DonVi}</strong> thành công.</p>
                            });

                            if (isCreate) {
                                history.replace("/department/detail/" + response.data.id_DonVi);
                            }
                        }).catch(error => {
                            const message = typeof error.response.data === "string" ? error.response.data : "";
                            this.boundActionCreators.showAlert({
                                variant: "danger",
                                content: (isCreate ? "Khởi tạo" : "Cập nhật") + " không thành công. " + message
                            });
                        }).then(() => {
                            this.setState({ loading: false });
                        });
                    }
                }
            });
        }
    }

    clickUpdateEmail(event) {
        event.preventDefault();
        const { department } = this.state;
        this.formEmail.validateAll();
        if (this.checkBtnEmail.context._errors.length === 0) {
            this.boundActionCreators.showModal({
                title: "Lưu đơn vị",
                body: <span>Bạn có muốn lưu thông tin email của đơn vị <strong>{department.ten_DonVi || ""}</strong> không?</span>,
                cancelButton: {
                    title: "Hủy"
                },
                okButton: {
                    title: "Lưu",
                    handle: () => {
                        this.boundActionCreators.hideModal();
                        this.setState({ loading: true });
                        axios.post(ApiPaths.UpdateEmailDepartment, {
                            Ma_DonVi: department.ma_DonVi,
                            Email: department.email,
                            SMTP_Email: department.smtP_Email,
                            Port_Email: department.port_Email,
                            Account_Email: department.account_Email,
                            Pass_Email: department.pass_Email
                        }).then(response => {
                            this.boundActionCreators.showAlert({
                                variant: "success",
                                content: "Cập nhập thông tin email thành công"
                            });
                            this.setState({ department: response.data });
                        }).catch(error => {
                            this.boundActionCreators.showAlert({
                                variant: "danger",
                                content: "Cập nhập thông tin email không thành công. " + error.response.data
                            });
                        }).then(() => {
                            this.setState({ loading: false });
                        });
                    }
                }
            });
        }
    }

    deleteDepartment(event) {
        event.preventDefault();
        const { isCreate, department } = this.state;
        if (isCreate) {
            return;
        }

        this.boundActionCreators.showModal({
            title: "Xóa đơn vị",
            body: "Bạn có muốn xóa đơn vị này không?",
            okButton: {
                title: "Xóa",
                handle: () => {
                    this.boundActionCreators.hideModal();
                    this.setState({ loading: true });
                    axios.get(ApiPaths.DeleteDepartment + "?code=" + department.ma_DonVi).then(response => {
                        this.boundActionCreators.showAlert({
                            variant: "success",
                            content: <p className="mb-0">Xóa đơn vị <strong>{department.ten_DonVi}</strong> thành công.</p>
                        });
                        history.replace("/department");
                    }).catch(error => {
                        const message = typeof error.response.data === "string" ? error.response.data : "";
                        this.boundActionCreators.showAlert({
                            variant: "danger",
                            content: "Xóa đơn vị không thành công. " + message
                        });
                    }).then(() => {
                        this.setState({ loading: false });
                    });
                }
            }
        });
    }

    render() {
        const { department, loading, isCreate } = this.state;
        return (
            <LoadingOverlay >
                <section className="content">
                    <div className="row">
                        <div className="col-lg-12">
                            <div className="nav-tabs-custom">
                                <Tabs defaultActiveKey="content">
                                    <Tab eventKey="content" title="Thông tin chi tiết">
                                        <div className="tab-content">
                                            <div className="box-body">
                                                <Form ref={c => this.form = c}>
                                                    <Container>
                                                        <Row>
                                                            <Col>
                                                                <div className="form-group row">
                                                                    <label htmlFor="ma_DonVi" className="col-sm-3 col-form-label">Mã đơn vị:</label>
                                                                    <div className="col-sm-9">
                                                                        <Input
                                                                            type="text"
                                                                            className="form-control"
                                                                            placeholder="Mã đơn vị"
                                                                            name="ma_DonVi"
                                                                            disabled
                                                                            value={department.ma_DonVi || ""}
                                                                            onChange={this.handleChange}
                                                                        />
                                                                    </div>
                                                                </div>
                                                                <div className="form-group row">
                                                                    <label htmlFor="ten_DonVi" className="col-sm-3 col-form-label">Tên đơn vị: <span className="text-red">*</span></label>
                                                                    <div className="col-sm-9">
                                                                        <Input
                                                                            type="text"
                                                                            className="form-control"
                                                                            placeholder="Tên đơn vị"
                                                                            name="ten_DonVi"
                                                                            value={department.ten_DonVi || ""}
                                                                            validations={[required, maxLength50]}
                                                                            onChange={this.handleChange}
                                                                        />
                                                                    </div>
                                                                </div>
                                                                <div className="form-group row">
                                                                    <label htmlFor="tenNguoi_DaiDien" className="col-sm-3 col-form-label">Đại diện: <span className="text-red">*</span></label>
                                                                    <div className="col-sm-9">
                                                                        <Input
                                                                            type="text"
                                                                            className="form-control"
                                                                            placeholder="Người đại diện"
                                                                            name="tenNguoi_DaiDien"
                                                                            value={department.tenNguoi_DaiDien || ""}
                                                                            validations={[required, maxLength50]}
                                                                            onChange={this.handleChange}
                                                                        />
                                                                    </div>
                                                                </div>
                                                                <div className="form-group row">
                                                                    <label htmlFor="maSoThue" className="col-sm-3 col-form-label">Mã số thuế: <span className="text-red">*</span></label>
                                                                    <div className="col-sm-9">
                                                                        <Input
                                                                            type="text"
                                                                            className="form-control"
                                                                            placeholder="Mã số thuế"
                                                                            name="maSoThue"
                                                                            value={department.maSoThue || ""}
                                                                            validations={[required, maxLength40]}
                                                                            onChange={this.handleChange}
                                                                        />
                                                                    </div>
                                                                </div>
                                                            </Col>
                                                            <Col>
                                                                <div className="form-group row">
                                                                    <label htmlFor="id_DV_Cha" className="col-sm-3 col-form-label">Đơn vị cha:</label>
                                                                    <div className="col-sm-9">
                                                                        <CbbDepartments
                                                                            name="id_DV_Cha"
                                                                            className="form-control"
                                                                            defaultOption
                                                                            value={department.id_DV_Cha}
                                                                            onChange={this.handleChange}
                                                                            ref={c => { this.cbbDepartments = c; }}
                                                                        />
                                                                    </div>
                                                                </div>
                                                                <div className="form-group row">
                                                                    <label htmlFor="website" className="col-sm-3 col-form-label">Website:</label>
                                                                    <div className="col-sm-9">
                                                                        <Input
                                                                            type="text"
                                                                            className="form-control"
                                                                            placeholder="Trang web"
                                                                            name="website"
                                                                            value={department.website || ""}
                                                                            validations={[maxLength50]}
                                                                            onChange={this.handleChange}
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
                                                                            value={department.dia_Chi || ""}
                                                                            validations={[required, maxLength200]}
                                                                            onChange={this.handleChange}
                                                                        />
                                                                    </div>
                                                                </div>
                                                                <div className="form-group row">
                                                                    <label htmlFor="dien_Thoai" className="col-sm-3 col-form-label">Điện thoại: <span className="text-red">*</span></label>
                                                                    <div className="col-sm-9">
                                                                        <Input
                                                                            type="text"
                                                                            className="form-control"
                                                                            placeholder="Số điện thoại"
                                                                            name="dien_Thoai"
                                                                            value={department.dien_Thoai || ""}
                                                                            validations={[required, maxLength50]}
                                                                            onChange={this.handleChange}
                                                                        />
                                                                    </div>
                                                                </div>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col>
                                                                <div className="form-group row">
                                                                    <label htmlFor="ghi_Chu" className="col-sm-2 col-form-label">Ghi chú:</label>
                                                                    <div className="col-sm-10">
                                                                        <Input
                                                                            type="text"
                                                                            className="form-control"
                                                                            placeholder="Ghi chú"
                                                                            name="ghi_Chu"
                                                                            value={department.ghi_Chu || ""}
                                                                            onChange={this.handleChange}
                                                                        />
                                                                    </div>
                                                                </div>
                                                            </Col>
                                                        </Row>
                                                        <CheckButton style={{ display: 'none' }} ref={c => { this.checkBtn = c; }} />
                                                    </Container>
                                                </Form>
                                            </div>
                                            <div className="box-footer">
                                                <Container>
                                                    <Row>
                                                        <Col>
                                                            {!isCreate ? <Button variant="danger" onClick={this.deleteDepartment}>Xóa</Button> : null}
                                                            <Button className="pull-right" variant="primary" onClick={this.clickSaveDepartment}>{isCreate?"Khởi tạo": "Cập nhật"}</Button>
                                                        </Col>
                                                    </Row>
                                                </Container>
                                            </div>
                                        </div>
                                    </Tab>
                                    {department.ma_DonVi ?
                                        <Tab eventKey="email" title="Email">
                                            <div className="tab-content">
                                                <div className="box-body">
                                                    <Form ref={c => this.formEmail = c}>
                                                        <Container>
                                                            <Row className="justify-content-md-center">
                                                                <Col xs lg="6">
                                                                    <div className="form-group row">
                                                                        <label htmlFor="email" className="col-sm-4 col-form-label">Email: <span className="text-red">*</span></label>
                                                                        <div className="col-sm-8">
                                                                            <Input
                                                                                type="text"
                                                                                className="form-control"
                                                                                placeholder="Email"
                                                                                name="email"
                                                                                value={department.email || ""}
                                                                                validations={[required, maxLength100]}
                                                                                onChange={this.handleChange}
                                                                            />
                                                                        </div>
                                                                    </div>
                                                                    <div className="form-group row">
                                                                        <label htmlFor="smtP_Email" className="col-sm-4 col-form-label">SMTP Email: <span className="text-red">*</span></label>
                                                                        <div className="col-sm-8">
                                                                            <Input
                                                                                type="text"
                                                                                className="form-control"
                                                                                placeholder="SMTP Email"
                                                                                name="smtP_Email"
                                                                                value={department.smtP_Email || ""}
                                                                                onChange={this.handleChange}
                                                                                validations={[required, maxLength100]}
                                                                            />
                                                                        </div>
                                                                    </div>
                                                                    <div className="form-group row">
                                                                        <label htmlFor="port_Email" className="col-sm-4 col-form-label">Port Email: <span className="text-red">*</span></label>
                                                                        <div className="col-sm-8">
                                                                            <Input
                                                                                type="number"
                                                                                className="form-control"
                                                                                placeholder="Port Email"
                                                                                name="port_Email"
                                                                                value={department.port_Email || 0}
                                                                                onChange={this.handleChange}
                                                                                validations={[required]}
                                                                            />
                                                                        </div>
                                                                    </div>
                                                                    <div className="form-group row">
                                                                        <label htmlFor="account_Email" className="col-sm-4 col-form-label">Tài khoản Email: <span className="text-red">*</span></label>
                                                                        <div className="col-sm-8">
                                                                            <Input
                                                                                type="text"
                                                                                className="form-control"
                                                                                placeholder="Tài khoản Email"
                                                                                name="account_Email"
                                                                                value={department.account_Email || ""}
                                                                                onChange={this.handleChange}
                                                                                validations={[required, maxLength32]}
                                                                            />
                                                                        </div>
                                                                    </div>
                                                                    <div className="form-group row">
                                                                        <label htmlFor="pass_Email" className="col-sm-4 col-form-label">Mật khẩu Email: <span className="text-red">*</span></label>
                                                                        <div className="col-sm-8">
                                                                            <Input
                                                                                type="password"
                                                                                className="form-control"
                                                                                placeholder="Mật khẩu Email"
                                                                                name="pass_Email"
                                                                                value={department.pass_Email || ""}
                                                                                onChange={this.handleChange}
                                                                                validations={[required, maxLength32]}
                                                                            />
                                                                        </div>
                                                                    </div>
                                                                    <CheckButton style={{ display: 'none' }} ref={c => { this.checkBtnEmail = c; }} />
                                                                </Col>
                                                            </Row>
                                                        </Container>
                                                    </Form>
                                                </div>
                                                <div className="box-footer">
                                                    <Container>
                                                        <Row>
                                                            <Col md={{ span: 2, offset: 10 }}>
                                                                <Button onClick={this.clickUpdateEmail} className="pull-right" variant="primary">Lưu</Button>
                                                            </Col>
                                                        </Row>
                                                    </Container>
                                                </div>
                                            </div>
                                        </Tab> : null}
                                </Tabs>
                            </div>
                        </div>
                    </div>
                </section>
                <Loader loading={loading} />
            </LoadingOverlay>
        );
    }
}

export default DepartmentDetail;