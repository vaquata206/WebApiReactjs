import React from 'react';
import DepartmentTree from "../Utils/DepartmentTree";
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { actionCreators } from "../../store/ConfirmationModal";
import { bindActionCreators } from 'redux';
import { store } from "../../store/store";
import { Link } from 'react-router-dom';
import { Button } from 'react-bootstrap';

class Department extends React.Component {

    constructor(props) {
        super(props);
        this.boundActionCreators = bindActionCreators(actionCreators, store.dispatch);
        this.changeDepartmentSelected = this.changeDepartmentSelected.bind(this);
        this.confirmDelete = this.confirmDelete.bind(this);
        this.state = {
            department: {}
        };
    }

    confirmDelete() {
        const { department } = this.state;
        
        if (!department.id_DonVi) {
            alert("Vui lòng chọn một đơn vị trước khi thực hiện.");
            return;
        }

        this.boundActionCreators.showModal({
            title: "Xóa đơn vị",
            body: <span>Bạn có muốn xóa đơn vị <strong>{department.ten_DonVi || ""}</strong> không?</span>,
            cancelButton: {
                title: "Hủy"
            },
            okButton: {
                title: "Đồng ý"
            }
        });
    }

    changeDepartmentSelected(node) {
        let url = ApiPaths.GetDepartmentById + "?id=" + node.id;
        axios.get(url).then(response => {
            this.setState({ department: response.data });
        });
    }

    render() {
        const department = this.state.department || {};
        return (
            <section className="content">
                <div className="row">
                    <div className="col-lg-12">
                        <div className="box box-primary">
                            <div className="box-header with-border">
                                <h3 className="box-title">Danh sách đơn vị</h3>
                                <Link to="0" title="Thêm đơn vị" className="btn btn-primary btn-sm pull-right"><i className="fa fa-plus" /></Link>
                            </div>
                            <div className="box-body row">
                                <div className="col-md-6" style={{ minHeight: "400px", borderRight: "1px solid silver" }}>
                                    <DepartmentTree onChange={this.changeDepartmentSelected} />
                                </div>
                                <div className="col-md-6">
                                    <div className="form-horizontal col-sm-12">
                                        <div className="form-group row">
                                            <label className="col-sm-4 control-label">Mã đơn vị:</label>
                                            <div className="col-sm-8">
                                                <input type="text" className="form-control" name="Ma_DonVi" readOnly value={department.ma_DonVi || ""} />
                                            </div>
                                        </div>
                                        <div className="form-group row">
                                            <label className="col-sm-4 control-label">Tên đơn vị:</label>
                                            <div className="col-sm-8">
                                                <input type="text" className="form-control" name="Ten_DonVi" readOnly value={department.ten_DonVi || ""} />
                                            </div>
                                        </div>
                                        <div className="form-group row">
                                            <label className="col-sm-4 control-label">Mã số thuế:</label>
                                            <div className="col-sm-8">
                                                <input type="text" className="form-control" name="Ten_DonVi" readOnly value={department.maSoThue || ""} />
                                            </div>
                                        </div>
                                        <div className="form-group row">
                                            <label className="col-sm-4 control-label">Người đại diện:</label>
                                            <div className="col-sm-8">
                                                <input type="text" className="form-control" name="Nguoi_DaiDien" readOnly value={department.tenNguoi_DaiDien || ""} />
                                            </div>
                                        </div>
                                        <div className="box-footer">
                                            <button className="btn btn-danger btn-sm" onClick={this.confirmDelete} disabled={!department.id_DonVi}>Xóa</button>
                                            {department.id_DonVi ?
                                                <Link to={"" + department.id_DonVi} className="btn btn-success btn-sm pull-right">Chi tiết</Link> :
                                                null
                                            }
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            );
    }
}

export default Department;