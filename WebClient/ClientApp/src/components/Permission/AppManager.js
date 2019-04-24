import React from 'react';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { modalHelper, alertHelper } from "../../helpers/utils";
import { Row, Col, Table, Button, Spinner } from 'react-bootstrap';
import DepartmentTree from "./../Utils/DepartmentTree";
import { Link } from "react-router-dom";
import LinesEllipsis from 'react-lines-ellipsis';

class AppManager extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            account: null,
            loading: false,
            btnLoading: false,
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
                    app.selected = this.findAppById(response.data, app.id_ChuongTrinh) >= 0;
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
                        this.setState({ btnLoading: true });
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
                            this.setState({ btnLoading: false });
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
            if (list[i].id_ChuongTrinh === id) {
                return i;
            }
        }

        return -1;
    }

    render() {
        const { apps, loading, account, btnLoading } = this.state;
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
                                <Link to="/permission/apps/add" className="btn btn-success btn-sm pull-right"><i className="fa fa-plus" /></Link>
                            </div>
                            <div className="box-body">
                                <Table striped bordered hover size="sm">
                                    <thead>
                                        <tr>
                                            <th>STT</th>
                                            <th style={{ width: "150px" }}>Mã chương trình</th>
                                            <th>Tên chương trình</th>
                                            <th>Chi tiết</th>
                                            <th>Chọn</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {account ?
                                            loading ?
                                                <tr style={{ textAlign: "center" }}>
                                                    <td colSpan="5">
                                                        <Spinner animation="border" role="status" size="sm">
                                                            <span className="sr-only">Loading...</span>
                                                        </Spinner>
                                                    </td>
                                                </tr> :
                                                (apps || []).length > 0 ? apps.map((value, index) => (
                                                    <tr key={value.id_ChuongTrinh}>
                                                        <td>{index + 1}</td>
                                                        <td title={value.ma_ChuongTrinh}><LinesEllipsis text={value.ma_ChuongTrinh} maxLine="1" /></td>
                                                        <td title={value.ten_ChuongTrinh}><a href={value.url}><LinesEllipsis text={value.ten_ChuongTrinh} maxLine="1" /></a></td>
                                                        <td><Link to={"/permission/apps/" + value.id_ChuongTrinh}>Chi tiết</Link></td>
                                                        <td><input type="checkbox" checked={value.selected ? true : false} onChange={() => this.onChangeCheckbox(value)} /></td>
                                                    </tr>
                                                )) :
                                                    <tr style={{ textAlign: "center" }}>
                                                        <td colSpan="5">Không có dữ liệu</td>
                                                    </tr>
                                            : <tr style={{ textAlign: "center" }}>
                                                <td colSpan="5">Vui lòng chọn một tài khoản</td>
                                            </tr>
                                        }
                                    </tbody>
                                </Table>
                            </div>
                            <div className="box-footer">
                                <Row>
                                    <Col>
                                        <Button variant="primary" className="pull-right" size="sm" disabled={loading || !account || btnLoading} onClick={this.onSaveApps}>
                                            {btnLoading ?
                                                <Spinner
                                                    as="span"
                                                    animation="border"
                                                    size="sm"
                                                    role="status"
                                                    aria-hidden="true"
                                                />
                                                : "Cấp quyền"}
                                        </Button>
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