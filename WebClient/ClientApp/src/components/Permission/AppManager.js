﻿import React from 'react';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { modalHelper, alertHelper } from "../../helpers/utils";
import { Row, Col, Table, Button, Spinner } from 'react-bootstrap';
import DepartmentTree from "./../Utils/DepartmentTree";

class AppManager extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            account: null,
            loading: false,
            apps: []
        };
        this.onDepartmentChange = this.onDepartmentChange.bind(this);
        this.onChangeCheckbox = this.onChangeCheckbox.bind(this);
        this.onSaveApps = this.onSaveApps.bind(this);
    }

    componentWillMount() {
        this.setState({ loading: true });
        axios.get(ApiPaths.GetApps).then(response => {
            this.setState({ apps: response.data });
        }).catch(error => {
            alertHelper.show({
                variant: "danger",
                content: "Lấy danh sách chương trình không thành công"
            });
        }).then(() => {
            this.setState({ loading: false });
        });
    }

    onDepartmentChange(node) {
        const { apps, loading } = this.state;
        if (loading || (apps || []).length === 0) {
            return;
        }

        if (node.typeNode === "Account") {
            this.setState({ loading: true, account: node });
            axios.get(ApiPaths.GetUserApps + "?id=" + node.id).then(response => {
                (apps || []).forEach(app => {
                    app.selected = this.findAppById(response.data, app.id) >= 0;
                });
            }).catch(error => {
                let message = typeof error.response.data === "string" ? error.response.data : "";
                alertHelper.show({
                    variant: "danger",
                    content: "Không thể lấy chương trình của user" + node.title + ". " + message
                });
            }).then(() => {
                this.setState({ loading: false });
            });
        } else {
            this.setState({ account: null });   
        }
    }

    onChangeCheckbox(node) {
        node.selected = !node.selected;
        this.setState({});
    }

    onSaveApps() {
        const { account, apps } = this.state;
        if (account) {
            modalHelper.show({
                title: "Cấp quyền chương trình",
                body: <p className="mb-0">Bạn có muốn cấp quyền đối với tài khoản <strong>{account.title}</strong> không?</p>,
                okButton: {
                    title: "Cấp quyền",
                    handle: () => {
                        this.setState({ loading: true });
                        modalHelper.hide();
                        axios.post(ApiPaths.SetUserApps, {
                            Id_NguoiDung: account.id,
                            Id_ChuongTrinhs: this.getAppIdsSelected(apps)
                        }).then(response => {
                            alertHelper.show({
                                variant: "success",
                                content: <p className="mb-0">Cấp quyền cho tài khoản <strong>{account.title}</strong> thành công.</p>
                            });
                        }).catch(error => {
                            let message = typeof error.response.data === "string" ? error.response.data : "";
                            alertHelper.show({
                                variant: "danger",
                                content: <p className="mb-0">Cấp quyền cho tài khoản <strong>{account.title}</strong> không thành công. {message}</p>
                            });
                        }).then(() => {
                            this.setState({ loading: false });
                        });
                    }
                }
            });
        }
    }

    getAppIdsSelected(apps) {
        var ids = [];
        (apps || []).forEach(value => {
            if (value.selected) {
                ids.push(value.id_ChuongTrinh);
            }
        });
        return ids;
    }

    findAppById(list, id) {
        const length = (list || []).length;
        for (let i = 0; i < length; i++) {
            if (list[i].id === id) {
                return i;
            }
        }

        return -1;
    }

    render() {
        const { apps, loading, account } = this.state;
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
                                <h3 className="box-title">Danh sách chương trình</h3>
                            </div>
                            <div className="box-body">
                                <Table striped bordered hover size="sm">
                                    <thead>
                                        <tr>
                                            <th>STT</th>
                                            <th>Mã chương trình</th>
                                            <th>Tên chương trình</th>
                                            <th>Chọn</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {account ?
                                            loading ?
                                                <tr style={{ textAlign: "center" }}>
                                                    <td colSpan="4">
                                                        <Spinner animation="border" role="status" size="sm">
                                                            <span className="sr-only">Loading...</span>
                                                        </Spinner>
                                                    </td>
                                                </tr> :
                                                (apps || []).length > 0 ? apps.map((value, index) => (
                                                    <tr key={value.id_ChuongTrinh}>
                                                        <td>{index + 1}</td>
                                                        <td>{value.ma_ChuongTrinh}</td>
                                                        <td><a href={value.url}>{value.ten_ChuongTrinh}</a></td>
                                                        <td><input type="checkbox" checked={value.selected ? true : false} onChange={() => this.onChangeCheckbox(value)} /></td>
                                                    </tr>
                                                )) :
                                                    <tr style={{ textAlign: "center" }}>
                                                        <td colSpan="4">Không có dữ liệu</td>
                                                    </tr>
                                            : <tr style={{ textAlign: "center" }}>
                                                <td colSpan="4">Vui lòng chọn một tài khoản</td>
                                            </tr>
                                        }
                                    </tbody>
                                </Table>
                            </div>
                            <div className="box-footer">
                                <Row>
                                    <Col>
                                        <Button variant="primary" className="pull-right" size="sm" disabled={loading || !account} onClick={this.onSaveApps}>Cấp quyền</Button>
                                    </Col>
                                </Row>
                            </div>
                        </div>
                    </Col>
                </Row>
            </section>
        );
    }
}

export default AppManager;