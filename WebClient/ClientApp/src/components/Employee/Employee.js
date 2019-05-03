import React from 'react';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { Link } from "react-router-dom";
import { Row, Col, Table, Button } from 'react-bootstrap';
import DepartmentTree from "./../Utils/DepartmentTree";
import { LoadingOverlay, Loader } from 'react-overlay-loader';
import { modalHelper, alertHelper } from "../../helpers/utils";

class Employee extends React.Component {

    constructor(props) {
        super(props);
        this.changeDepartmentSelected = this.changeDepartmentSelected.bind(this);
        this.state = {
            employees: [],
            loading: false
        };
        this.clickDeleteEmployee = this.clickDeleteEmployee.bind(this);
    }

    changeDepartmentSelected(d) {
        this.setState({ loading: true });
        axios.get(ApiPaths.GetEmployeesByDepartmentId + "?id=" + d.id).then(response => {
            this.setState({ employees: response.data });
        }).catch(error => {
            alertHelper.show({
                variant: "danger",
                content: error.response.data
            });
        }).then(() => {
            this.setState({ loading: false });
        });
    }

    clickDeleteEmployee(employee) {
        const { loading } = this.state;

        if (loading || !employee.ma_NhanVien) {
            return;
        }

        modalHelper.show({
            title: "Xóa nhân viên",
            body: <p>Bạn có muốn xóa nhân viên <strong>{employee.ho_Ten}</strong> không?</p>,
            okButton: {
                title: "Xóa",
                handle: () => {
                    this.setState({ loading: true });
                    modalHelper.hide();

                    axios.get(ApiPaths.DeleteEmployee + "?code=" + employee.ma_NhanVien).then(response => {
                        alertHelper.show({
                            variant: "success",
                            content: <p className="mb-0">Xóa nhân viên <strong>{employee.ho_Ten}</strong> thành công.</p>
                        });
                    }).catch(error => {
                        const message = typeof error.response.data === "string" ? error.response.data : "";
                        alertHelper.show({
                            variant: "danger",
                            content: <p className="mb-0">Xóa nhân viên <strong>{employee.ho_Ten}</strong> không thành công. {message}</p>
                        });
                    }).then(() => {
                        this.setState({ loading: false });
                    });
                }
            }
        });
    }

    render() {
        const { employees, loading } = this.state;
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
                                        <DepartmentTree onChange={this.changeDepartmentSelected}/>
                                    </Col>
                                </Row>
                            </div>
                        </div>
                    </Col>
                    <Col xs lg>
                        <div className="box box-primary">
                            <div className="box-header with-border">
                                <h3 className="box-title">Danh sách nhân viên</h3>
                                <Link to="create" title="Thêm nhân viên" className="btn btn-primary btn-sm pull-right"><i className="fa fa-plus" /></Link>
                            </div>
                            <div className="box-body">
                                <LoadingOverlay>
                                    <Row>
                                    <Col>
                                        <Table striped bordered hover size="sm">
                                            <thead>
                                                <tr>
                                                    <th>STT</th>
                                                    <th>Mã nhân viên</th>
                                                    <th>Tên nhân viên</th>
                                                    <th>Chi tiết</th>
                                                    <th>Xóa</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {(employees || []).length > 0 ?
                                                    employees.map((e, index) =>
                                                        (<tr key={e.id}>
                                                            <td>{index + 1}</td>
                                                            <td>{e.ma_NhanVien}</td>
                                                            <td>{e.ho_Ten}</td>
                                                            <td><Link to={"detail/" + e.ma_NhanVien} className="btn btn-success btn-sm">Xem</Link></td>
                                                            <td><Button variant="danger" size="sm" onClick={() => this.clickDeleteEmployee(e)}>Xóa</Button></td>
                                                        </tr>)
                                                    ) :
                                                    <tr><td colSpan="5" style={{ textAlign: "center" }}><span>Không có nhân viên</span></td></tr>
                                                }
                                            </tbody>
                                        </Table>
                                    </Col>
                                    </Row>
                                    <Loader loading={loading} />
                                </LoadingOverlay>
                            </div>
                        </div>
                    </Col>
                </Row>
            </section>
        );
    }
}

export default Employee;