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

class AppDetail extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            isCreate: true,
            app: {}
        };

        this.onClickSave = this.onClickSave.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }

    onClickSave() {
        const { loading, app, isCreate } = this.state;
        this.form.validateAll();
        if (this.checkBtn.context._errors.length !== 0 || loading) {
            return;
        }

        modalHelper.show({
            title: (isCreate?"Tạo mới": "Cập nhật") + " chương trình",
            body: "Bạn có muốn " + (isCreate ? "tạo mới" : "cập nhật") +" chương trình này không?",
            okButton: {
                title: isCreate ? "Tạo mới" : "Cập nhật",
                handle: () => {
                    this.setState({ loading: true });
                    modalHelper.hide();
                    axios.post(ApiPaths.SaveApp, app).then(response => {
                        alertHelper.show({
                            variant: "success",
                            content: <p className="mb-0">{isCreate ? "Tạo mới" : "Cập nhật"} chương trình <strong>{app.ten_ChuongTrinh}</strong> thành công.</p>
                        });

                        if (isCreate) {
                            history.replace("/permission/apps");
                        }
                    }).catch(error => {
                        const message = typeof error.response.data === "string" ? error.response.data : "";
                        alertHelper.show({
                            variant: "danger",
                            content: <p className="mb-0">{isCreate ? "Tạo mới" : "Cập nhật"} chương trình <strong>{app.ten_ChuongTrinh}</strong> không thành công. {message}</p>
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
        let { app } = this.state;
        this.setState(
            {
                app: {
                    ...app,
                    [name]: value
                }
            }
        );
    }

    render() {
        const { loading, app, isCreate } = this.state;
        return (
            <LoadingOverlay>
                <section className="content">
                    <Row>
                        <Col xs lg>
                            <div className="box box-primary">
                                <div className="box-header with-border">
                                    <h3 className="box-title">Chương trình</h3>
                                </div>
                                <div className="box-body">                            
                                    <Form ref={c => { this.form = c; }}>
                                        <Row className="justify-content-md-center">
                                            <Col xs lg="8">
                                                <div className="form-group row">
                                                    <label htmlFor="ma_ChuongTrinh" className="col-sm-3 col-form-label">Mã chương trình:</label>
                                                    <div className="col-sm-9">
                                                        <Input
                                                            type="text"
                                                            className="form-control"
                                                            placeholder="Mã chương trình"
                                                            name="ma_ChuongTrinh"
                                                            disabled
                                                            value={app.ma_ChuongTrinh || ""}
                                                        />
                                                    </div>
                                                </div>
                                                <div className="form-group row">
                                                    <label htmlFor="ten_ChuongTrinh" className="col-sm-3 col-form-label">Tên chương trình:<span className="text-red">*</span></label>
                                                    <div className="col-sm-9">
                                                        <Input
                                                            type="text"
                                                            className="form-control"
                                                            placeholder="Tên chương trình"
                                                            name="ten_ChuongTrinh"
                                                            value={app.ten_ChuongTrinh || ""}
                                                            validations={[required, maxLength100]}
                                                            onChange={this.handleChange}
                                                        />
                                                    </div>
                                                </div>
                                                <div className="form-group row">
                                                    <label htmlFor="url" className="col-sm-3 col-form-label">Trang web:<span className="text-red">*</span></label>
                                                    <div className="col-sm-9">
                                                        <Input
                                                            type="text"
                                                            className="form-control"
                                                            placeholder="Trang web"
                                                            name="url"
                                                            value={app.url || ""}
                                                            validations={[required, maxLength200]}
                                                            onChange={this.handleChange}
                                                        />
                                                    </div>
                                                </div>
                                                <div className="form-group row">
                                                    <label htmlFor="url" className="col-sm-3 col-form-label">Mô tả:</label>
                                                    <div className="col-sm-9">
                                                        <TextArea
                                                            className="form-control"
                                                            placeholder="Mô tả"
                                                            name="mo_Ta"
                                                            value={app.mo_Ta || ""}
                                                            validations={[maxLength200]}
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
                                                            value={app.ghi_Chu || ""}
                                                            validations={[maxLength200]}
                                                            onChange={this.handleChange}
                                                        />
                                                    </div>
                                                </div>
                                                <CheckButton style={{ display: "none" }} ref={c => { this.checkBtn = c;}}/>
                                            </Col>
                                        </Row>
                                    </Form>
                                </div>
                                <div className="box-footer">
                                    <Row>
                                        <Col>
                                            <Button className="pull-right" variant="primary" onClick={this.onClickSave}>{isCreate ? "Tạo mới" : "Cập nhật"}</Button>
                                            {!isCreate? <Button variant="danger" >Xóa</Button>: null}
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

export default AppDetail;