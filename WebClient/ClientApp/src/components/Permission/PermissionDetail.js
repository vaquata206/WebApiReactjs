import React from 'react';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { modalHelper, alertHelper } from "../../helpers/utils";
import Form from 'react-validation/build/form';
import Input from 'react-validation/build/input';
import CheckButton from 'react-validation/build/button';
import TextArea from "react-validation/build/textarea";
import { required, maxLength100, maxLength200 } from "./../../helpers/ValidatorTypes";
import { LoadingOverlay, Loader } from 'react-overlay-loader';
import { Row, Col, Button } from "react-bootstrap";
import history from "../../store/history";

class PermissionDetail extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            isCreate: true,
            permission: {}
        };

        this.onClickSave = this.onClickSave.bind(this);
        this.onClickDelete = this.onClickDelete.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }

    componentWillMount() {
        var id = this.props.match.params.id;
        if (id) {
            this.setState({ loading: true });
            axios.get(ApiPaths.permissions.Get + "?id=" + id).then(response => {
                this.setState({ permission: response.data, isCreate: false });
            }).catch(error => {
                alertHelper.showError(error, "Lấy quyền không thành công");
            }).then(() => {
                this.setState({ loading: false });
            });
        }
    }

    onClickSave() {
        const { loading, permission, isCreate } = this.state;
        this.form.validateAll();
        if (this.checkBtn.context._errors.length !== 0 || loading) {
            return;
        }

        modalHelper.show({
            title: (isCreate ? "Tạo mới" : "Cập nhật") + " quyền",
            body: "Bạn có muốn " + (isCreate ? "tạo mới" : "cập nhật") + " quyền này không?",
            okButton: {
                title: isCreate ? "Tạo mới" : "Cập nhật",
                handle: () => {
                    this.setState({ loading: true });
                    modalHelper.hide();
                    axios.post(ApiPaths.permissions.Save, permission).then(response => {
                        alertHelper.show({
                            variant: "success",
                            content: <p className="mb-0">{isCreate ? "Tạo mới" : "Cập nhật"} quyền <strong>{permission.ten_ChuongTrinh}</strong> thành công.</p>
                        });

                        if (isCreate) {
                            history.replace("/permission/list");
                        }
                    }).catch(error => {
                        alertHelper.showError(error, (isCreate ? "Tạo mới" : "Cập nhật") + "quyền không thành công");
                    }).then(() => {
                        this.setState({ loading: false });
                    });
                }
            }
        });
    }

    onClickDelete() {
        const { isCreate, permission } = this.state;
        if (isCreate || !permission.id_Quyen) {
            return;
        }

        modalHelper.show({
            title: "Xóa quyền",
            body: "Bạn có muốn xóa quyền này không.",
            okButton: {
                title: "Xóa",
                handle: () => {
                    modalHelper.hide();
                    this.setState({ loading: true });
                    axios.delete(ApiPaths.permissions.Delete + "?id=" + permission.id_Quyen).then(response => {
                        alertHelper.show({
                            vairant: "success",
                            content: <p className="mb-0">Xóa quyền <strong>{permission.ten_ChuongTrinh}</strong> thành công.</p>
                        });

                        history.replace("/permission/list");
                    }).catch(error => {
                        alertHelper.showError(error, "Xóa chương trình không thành công");
                    }).then(() => {
                        this.setState({ loading: false });
                    });
                }
            }
        });
    }

    handleChange(event) {
        let { name, value } = event.target;
        let { permission } = this.state;
        this.setState(
            {
                permission: {
                    ...permission,
                    [name]: value
                }
            }
        );
    }

    render() {
        const { loading, permission, isCreate } = this.state;
        return (
            <LoadingOverlay>
                <section className="content">
                    <Row>
                        <Col xs lg>
                            <div className="box box-primary">
                                <div className="box-header with-border">
                                    <h3 className="box-title">Quyền</h3>
                                </div>
                                <div className="box-body">
                                    <Form ref={c => { this.form = c; }}>
                                        <Row className="justify-content-md-center">
                                            <Col xs lg="8">
                                                <div className="form-group row">
                                                    <label htmlFor="ma_Quyen" className="col-sm-3 col-form-label">Mã quyền:</label>
                                                    <div className="col-sm-9">
                                                        <Input
                                                            type="text"
                                                            className="form-control"
                                                            placeholder="Mã quyền"
                                                            name="ma_Quyen"
                                                            disabled
                                                            value={permission.ma_Quyen || ""}
                                                        />
                                                    </div>
                                                </div>
                                                <div className="form-group row">
                                                    <label htmlFor="ten_Quyen" className="col-sm-3 col-form-label">Tên quyền:<span className="text-red">*</span></label>
                                                    <div className="col-sm-9">
                                                        <Input
                                                            type="text"
                                                            className="form-control"
                                                            placeholder="Tên quyền"
                                                            name="ten_Quyen"
                                                            value={permission.ten_Quyen || ""}
                                                            validations={[required, maxLength100]}
                                                            onChange={this.handleChange}
                                                        />
                                                    </div>
                                                </div>
                                                <div className="form-group row">
                                                    <label htmlFor="ghi_Chu" className="col-sm-3 col-form-label">Ghi chú:</label>
                                                    <div className="col-sm-9">
                                                        <TextArea
                                                            className="form-control"
                                                            placeholder="Ghi chú"
                                                            name="ghi_Chu"
                                                            value={permission.ghi_Chu || ""}
                                                            validations={[maxLength200]}
                                                            onChange={this.handleChange}
                                                        />
                                                    </div>
                                                </div>
                                                <CheckButton style={{ display: "none" }} ref={c => { this.checkBtn = c; }} />
                                            </Col>
                                        </Row>
                                    </Form>
                                </div>
                                <div className="box-footer">
                                    <Row>
                                        <Col>
                                            <Button className="pull-right" variant="primary" onClick={this.onClickSave}>{isCreate ? "Tạo mới" : "Cập nhật"}</Button>
                                            {!isCreate ? <Button variant="danger" onClick={this.onClickDelete} >Xóa</Button> : null}
                                        </Col>
                                    </Row>
                                </div>
                            </div>
                        </Col>
                    </Row>
                </section>
                <Loader loading={loading} />
            </LoadingOverlay>
        );
    }
}

export default PermissionDetail;