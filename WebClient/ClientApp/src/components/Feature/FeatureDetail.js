import React from 'react';
import axios from 'axios';
import history from "../../store/history";
import { ApiPaths } from "../../helpers/api";
import { Container, Row, Col, Button } from 'react-bootstrap';
import Form from 'react-validation/build/form';
import Input from 'react-validation/build/input';
import CheckButton from 'react-validation/build/button';
import { required, maxLength50, maxLength100, maxLength200 } from "./../../helpers/ValidatorTypes";
import { LoadingOverlay, Loader } from 'react-overlay-loader';
import { alertHelper, modalHelper } from "./../../helpers/utils";

class FeatureDetail extends React.Component {

    constructor(props) {
        super(props);       
        this.state = {
            loading: false,
            isCreate: true,
            feature: {}
        };

        this.handleChange = this.handleChange.bind(this);
    }

    componentWillMount() {
        const id = this.props.match.params.id;
        if (id) {
            this.setState({ loading: true });
            axios.get(ApiPaths.features.getFeature + "?id=" + id).then(response => {
                this.setState({ feature: response.data, isCreate: false });
            }).catch(error => {
                alertHelper.showError(error);
            }).then(() => {
                this.setState({ loading: false });
            });
        }
    }

    handleChange(event) {
        let { name, value } = event.target;
        let { feature } = this.state;
        this.setState(
            {
                feature: {
                    ...feature,
                    [name]: value
                }
            }
        );
    }

    render() {
        const { loading, feature, isCreate } = this.state;
        return (
            <LoadingOverlay >
                <section className="content">
                    <Row>
                        <Col xs lg>
                            <div className="box box-primary">
                                <div className="box-header with-border">
                                    <h3 className="box-title">Chức năng</h3>
                                </div>
                                <div className="box-body">
                                    <Form ref={c => this.form = c}>
                                        <Container>
                                            <Row>
                                                <Col>
                                                    <div className="form-group row">
                                                        <label htmlFor="ma_ChucNang" className="col-sm-3 col-form-label">Mã chức năng:</label>
                                                        <div className="col-sm-9">
                                                            <Input
                                                                type="text"
                                                                className="form-control"
                                                                placeholder="Mã chức năng"
                                                                name="ma_ChucNang"
                                                                disabled
                                                                value={feature.ma_ChucNang || ""}
                                                            />
                                                        </div>
                                                    </div>
                                                    <div className="form-group row">
                                                        <label htmlFor="ten_ChucNang" className="col-sm-3 col-form-label">Tên chức năng: <span className="text-red">*</span></label>
                                                        <div className="col-sm-9">
                                                            <Input
                                                                type="text"
                                                                className="form-control"
                                                                placeholder="Tên chức năng"
                                                                name="ten_ChucNang"
                                                                value={feature.ten_ChucNang || ""}
                                                                validations={[required, maxLength50]}
                                                                onChange={this.handleChange}
                                                            />
                                                        </div>
                                                    </div>
                                                    <div className="form-group row">
                                                        <label htmlFor="moTa_ChucNang" className="col-sm-3 col-form-label">Mô tả:</label>
                                                        <div className="col-sm-9">
                                                            <Input
                                                                type="text"
                                                                className="form-control"
                                                                placeholder="Mô tả chức năng"
                                                                name="moTa_ChucNang"
                                                                value={feature.moTa_ChucNang || ""}
                                                                validations={[maxLength200]}
                                                                onChange={this.handleChange}
                                                            />
                                                        </div>
                                                    </div>
                                                    <div className="form-group row">
                                                        <label htmlFor="tooltip" className="col-sm-3 col-form-label">Tooltip:</label>
                                                        <div className="col-sm-9">
                                                            <Input
                                                                type="text"
                                                                className="form-control"
                                                                placeholder="Tooltip"
                                                                name="tooltip"
                                                                value={feature.tooltip || ""}
                                                                validations={[maxLength100]}
                                                                onChange={this.handleChange}
                                                            />
                                                        </div>
                                                    </div>
                                                    <div className="form-group row">
                                                        <label htmlFor="controller_Name" className="col-sm-3 col-form-label">Controller:</label>
                                                        <div className="col-sm-9">
                                                            <Input
                                                                type="text"
                                                                className="form-control"
                                                                placeholder="Tên controller"
                                                                name="controller_Name"
                                                                value={feature.controller_Name || ""}
                                                                validations={[maxLength100]}
                                                                onChange={this.handleChange}
                                                            />
                                                        </div>
                                                    </div>
                                                    <div className="form-group row">
                                                        <label htmlFor="action_Name" className="col-sm-3 col-form-label">Action:</label>
                                                        <div className="col-sm-9">
                                                            <Input
                                                                type="text"
                                                                className="form-control"
                                                                placeholder="Tên action"
                                                                name="action_Name"
                                                                value={feature.action_Name || ""}
                                                                validations={[maxLength100]}
                                                                onChange={this.handleChange}
                                                            />
                                                        </div>
                                                    </div>
                                                    <div className="form-group row">
                                                        <label htmlFor="action_Name" className="col-sm-3 col-form-label">Hiển thị menu:</label>
                                                        <div className="col-sm-9">
                                                            <Input
                                                                type="checkbox"
                                                                name="action_Name"
                                                                checked={feature.hienThi_Menu === 1}
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
                                                {!isCreate ? <Button disabled variant="danger">Xóa</Button> : null}
                                                <Button disabled className="pull-right" variant="primary">{isCreate ? "Khởi tạo" : "Cập nhật"}</Button>
                                            </Col>
                                        </Row>
                                    </Container>
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

export default FeatureDetail;