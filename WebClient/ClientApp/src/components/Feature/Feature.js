import React from 'react';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { alertHelper } from "../../helpers/utils";
import FeatureTree from "./../Utils/FeatureTree";
import SelectProgram from "./../Utils/SelectProgram";
import { Row, Col } from 'react-bootstrap';
import { Link } from "react-router-dom";
import { LoadingOverlay, Loader } from 'react-overlay-loader';

class Feature extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            appId: 0,
            feature: {}
        };

        this.onPropgramChange = this.onPropgramChange.bind(this);
        this.onFeatureTreeChange = this.onFeatureTreeChange.bind(this);
    }

    onPropgramChange(event) {
        this.setState({ appId: event.target.value, feature: {} });
    }

    onFeatureTreeChange(node) {
        this.setState({ loading: true });
        axios.get(ApiPaths.features.getFeature + "?id=" + node.id).then(response => {
            this.setState({ feature: response.data });
        }).catch(error => {
            alertHelper.showError(error, "Lấy chức năng không thành công");
        }).then(() => {
            this.setState({ loading: false });
        });
    }

    render() {
        const { appId, feature, loading } = this.state;
        return (
            <section className="content">
                <Row>
                    <Col xs lg>
                        <div className="box box-primary">
                            <div className="box-header with-border">
                                <h3 className="box-title">Danh sách chức năng</h3>
                                <SelectProgram className="pull-right" value={appId} onChange={this.onPropgramChange} />
                                <span className="pull-right">Chương trình </span>
                            </div>
                            <div className="box-body">
                                <Row>
                                    <Col>
                                        <FeatureTree appId={appId} onChange={this.onFeatureTreeChange} />
                                    </Col>
                                </Row>
                            </div>
                        </div>
                    </Col>
                    <Col xs lg>
                        <div className="box box-primary">
                            <div className="box-header with-border">
                                <h3 className="box-title">Chức năng</h3>
                                <Link to="create" title="Thêm chức năng" className="btn btn-primary btn-sm pull-right"><i className="fa fa-plus" /></Link>
                            </div>
                            <div className="box-body">
                                <LoadingOverlay>
                                    <div className="form-horizontal col-sm-12">
                                        <div className="form-group row">
                                            <label className="col-sm-4 control-label">Mã chức năng:</label>
                                            <div className="col-sm-8">
                                                <input type="text" className="form-control" readOnly value={feature.ma_ChucNang || ""} />
                                            </div>
                                        </div>
                                        <div className="form-group row">
                                            <label className="col-sm-4 control-label">Tên chức năng:</label>
                                            <div className="col-sm-8">
                                                <input type="text" className="form-control" readOnly value={feature.ten_ChucNang || ""} />
                                            </div>
                                        </div>
                                        <div className="form-group row">
                                            <label className="col-sm-4 control-label">Mô tả:</label>
                                            <div className="col-sm-8">
                                                <input type="text" className="form-control" readOnly value={feature.mota_ChucNang || ""} />
                                            </div>
                                        </div>
                                        <div className="form-group row">
                                            <div className="col-sm-8 offset-sm-4">
                                                <input type="checkbox" readOnly checked={feature.hienThi_Menu !== 0} />
                                                Hiển thị trên menu
                                            </div>
                                        </div>
                                        {
                                            (feature.id_ChucNang ?
                                                <div className="box-footer">
                                                    <Link to={"/feature/details/" + feature.id_ChucNang} title="Thêm chức năng" className="btn btn-success btn-sm pull-right">Chi tiết</Link>
                                                </div> : null
                                            )
                                        }
                                    </div>
                                    <Loader loading={loading} />
                                </LoadingOverlay>
                            </div>
                        </div>
                    </Col>
                </Row>
            </section>);
    }
}

export default Feature;