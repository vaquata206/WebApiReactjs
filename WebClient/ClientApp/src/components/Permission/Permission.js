import React from 'react';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { modalHelper, alertHelper } from "../../helpers/utils";
import { Row, Col, Table, Button, Spinner  } from 'react-bootstrap';
import DepartmentTree from "./../Utils/DepartmentTree";

class Permission extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            permissions: []
        };
        this.onDepartmentChange = this.onDepartmentChange.bind(this);
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

    onDepartmentChange(node) {
        const { permissions, loading } = this.state;
        if (loading || (permissions || []).length === 0) {
            return;
        }
    }

    render() {
        const { permissions, loading } = this.state;
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
                </Row>
            </section>
            );
    }
}

export default Permission;