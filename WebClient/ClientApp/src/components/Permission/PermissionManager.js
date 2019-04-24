import React from 'react';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { alertHelper } from "../../helpers/utils";
import { Row, Col, Table, Spinner } from 'react-bootstrap';
import FeatureTree from "../Utils/FeatureTree";

class PermissionManager extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            permissions: []
        };
    }

    componentWillMount() {
        this.setState({ loading: true });
        axios.get(ApiPaths.GetPermisisons).then(response => {
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

    render() {
        const { permissions, loading } = this.state;
        return (
            <section className="content">
                <Row>
                    <Col xs lg>
                        <div className="box box-primary">
                            <div className="box-header with-border">
                                <h3 className="box-title">Danh sách quyền</h3>
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
                                            (permissions || []).length > 0 ? permissions.map((value, index) => (
                                                <tr>
                                                    <td>{index + 1}</td>
                                                    <td>{value.ma_Quyen}</td>
                                                    <td>{value.ten_Quyen}</td>
                                                    <td><input type="checkbox" /></td>
                                                </tr>
                                            )) :
                                                <tr style={{ textAlign: "center" }}>
                                                    <td colSpan="4">Không có dữ liệu</td>
                                                </tr>
                                        }
                                    </tbody>
                                </Table>
                            </div>
                        </div>
                    </Col>
                    <Col xs lg>
                        <div className="box box-primary">
                            <div className="box-header with-border">
                                <h3 className="box-title">Danh sách quyền</h3>
                            </div>
                            <div className="box-body">
                                <FeatureTree />
                            </div>
                        </div>
                    </Col>
                </Row>
            </section>
            );
    }
}

export default PermissionManager;