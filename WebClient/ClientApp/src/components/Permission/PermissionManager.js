﻿import React from 'react';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { alertHelper, titleHeader } from "../../helpers/utils";
import { Row, Col, Table, Spinner } from 'react-bootstrap';
import FeatureTree from "../Utils/FeatureTree";
import { Link } from 'react-router-dom';
import SelectProgram from "./../Utils/SelectProgram";

class PermissionManager extends React.Component {
    constructor(props) {
        super(props);
        titleHeader.set("Quản lý quyền");
        this.state = {
            loading: false,
            permissions: [],
            appId: 0
        };

        this.onPropgramChange = this.onPropgramChange.bind(this);
        this.loadPermissions = this.loadPermissions.bind(this);
    }

    componentWillMount() {
        this.loadPermissions(this.state.appId);
    }

    loadPermissions(appId) {
        this.setState({ loading: true });
        axios.get(ApiPaths.permissions.GetAll + "?id=" + appId).then(response => {
            this.setState({ permissions: response.data });
        }).catch(error => {
            alertHelper.show({
                variant: "danger",
                content: "Lấy danh sách quyền không thành công. Xin vui lòng thử lại sau"
            });
        }).then(() => {
            this.setState({ loading: false });
        });
    }

    onPropgramChange(event) {
        this.setState({ appId: event.target.value, feature: {} });
        this.loadPermissions(event.target.value);
    }

    render() {
        const { permissions, loading, appId } = this.state;
        return (
            <section className="content">
                <Row>
                    <Col xs lg>
                        <div className="box box-primary">
                            <div className="box-header with-border">
                                <h3 className="box-title">Danh sách quyền</h3>
                                <SelectProgram className="pull-right" value={appId} onChange={this.onPropgramChange}/>
                            </div>
                            <div className="box-body">
                                <Table striped bordered hover size="sm">
                                    <thead>
                                        <tr>
                                            <th>STT</th>
                                            <th>Mã quyền</th>
                                            <th>Tên quyền</th>
                                            <th>Chi tiết</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {loading ?
                                            <tr style={{ textAlign: "center" }}>
                                                <td colSpan="4">
                                                    <Spinner animation="border" role="status" size="sm">
                                                        <span className="sr-only">Loading...</span>
                                                    </Spinner>
                                                </td>
                                            </tr> :
                                            (permissions || []).length > 0 ? permissions.map((value, index) => (
                                                <tr key={value.id_Quyen}>
                                                    <td>{index + 1}</td>
                                                    <td>{value.ma_Quyen}</td>
                                                    <td>{value.ten_Quyen}</td>
                                                    <td><Link to={"/permission/detail/" + value.id_Quyen} className="btn btn-success btn-sm">Chi tiết</Link></td>
                                                </tr>
                                            )) :
                                                <tr style={{ textAlign: "center" }}>
                                                    <td colSpan="4">Không có dữ liệu</td>
                                                </tr>
                                        }
                                    </tbody>
                                </Table>
                            </div>
                            <div className="box box-footer">
                                <Link to="/permission/create" title="Thêm quyền" className="btn btn-primary btn-sm pull-right"><i className="fa fa-plus" /></Link>
                            </div>
                        </div>
                    </Col>
                    <Col xs lg>
                        <div className="box box-primary">
                            <div className="box-header with-border">
                                <h3 className="box-title">Danh sách quyền</h3>
                            </div>
                            <div className="box-body">
                                <FeatureTree appId={appId} />
                            </div>
                        </div>
                    </Col>
                </Row>
            </section>
            );
    }
}

export default PermissionManager;