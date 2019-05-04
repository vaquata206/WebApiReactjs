import React from 'react';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { alertHelper } from "../../helpers/utils";
import { Row, Col, Table, Spinner  } from 'react-bootstrap';
import DepartmentTree from "./../Utils/DepartmentTree";
import SelectProgram from "./../Utils/SelectProgram";
import { Link } from "react-router-dom";

class Permission extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            appId: 0,
            loading: false,
            permissions: []
        };
        this.onDepartmentChange = this.onDepartmentChange.bind(this);
        this.onPropgramChange = this.onPropgramChange.bind(this);
        this.loadPermissions = this.loadPermissions.bind(this);
        this.changeSelectPermission = this.changeSelectPermission.bind(this);
    }

    componentWillMount() {
        this.loadPermissions(0);
    }

    loadPermissions(appId) {
        this.setState({ loading: true });
        axios.get(ApiPaths.GetPermisisons + "?id=" + appId).then(response => {
            this.setState({ permissions: response.data });
        }).catch(error => {
            alertHelper.showError(error, "Lấy danh sách quyền không thành công");
        }).then(() => {
            this.setState({ loading: false });
        });
    }

    onPropgramChange(event) {
        this.setState({ appId: event.target.value });
        this.loadPermissions(event.target.value);
    }

    onDepartmentChange(node) {
        const { permissions, loading, appId } = this.state;
        if (loading || (permissions || []).length === 0) {
            return;
        }

        if (node.typeNode === "Account") {
            this.setState({ account: node, loading: true });
            axios.get(ApiPaths.permissions.GetUserPermissions + "?userId=" + node.id + "&appId=" + appId).then(response => {
                (permissions || []).forEach(value => {
                    value.selected = (response.data || []).indexOf(value.id_Quyen) >= 0;
                });
            }).catch(error => {
                alertHelper.showError(error, "Không thể lấy quyền.");
            }).then(() => {
                this.setState({ loading: false });
            });
        } else {
            this.setState({ account: null });
        }
    }

    changeSelectPermission(permission) {
        permission.selected = !permission.selected;
        this.setState({});
    }

    render() {
        const { permissions, loading, account, appId } = this.state;
        return (
            <section className="content">
                <Row>
                    <Col xs lg>
                        <div className="box box-primary">
                            <div className="box-header with-border">
                                <h3 className="box-title">Danh sách đơn vị</h3>
                            </div>
                            <div className="box-body">
                                <Row>
                                    <Col>
                                        <DepartmentTree showEmployee onChange={this.onDepartmentChange} />
                                    </Col>
                                </Row>
                            </div>
                        </div>
                    </Col>
                    <Col xs lg>
                        <div className="box box-primary">
                            <div className="box-header with-border">
                                <h3 className="box-title">Danh sách quyền</h3>
                                <SelectProgram className="pull-right" value={appId} onChange={this.onPropgramChange} />
                            </div>
                            <div className="box-body">
                                <Table striped bordered hover size="sm">
                                    <thead>
                                        <tr>
                                            <th>STT</th>
                                            <th>Mã quyền</th>
                                            <th>Tên quyền</th>
                                            <th>Chọn</th>
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
                                            (account ?
                                                (permissions || []).length > 0 ? permissions.map((value, index) => (
                                                    <tr key={value.id_Quyen}>
                                                        <td>{index + 1}</td>
                                                        <td><Link to={"/permission/detail/" + value.id_Quyen}>{value.ma_Quyen}</Link></td>
                                                        <td>{value.ten_Quyen}</td>
                                                        <td><input type="checkbox" checked={value.selected} onChange={()=> this.changeSelectPermission(value)} /></td>
                                                    </tr>
                                                )) :
                                                    <tr style={{ textAlign: "center" }}>
                                                        <td colSpan="4">Không có dữ liệu</td>
                                                    </tr>
                                                : <tr style={{ textAlign: "center" }}><td colSpan="4">Vui lòng chọn một tài khoản</td></tr>)
                                        }
                                    </tbody>
                                </Table>
                            </div>
                        </div>
                    </Col>
                </Row>
            </section>
            );
    }
}

export default Permission;