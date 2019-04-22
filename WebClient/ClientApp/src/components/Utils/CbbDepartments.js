import React from 'react';
import axios from 'axios';
import { Spinner, Button } from "react-bootstrap";
import { ApiPaths } from "../../helpers/api";
import { store } from "../../store/store";
import { bindActionCreators } from 'redux';
import { actionCreators } from "../../store/AdminAlert";

class CbbDepartments extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            departments: [],
            loading: false
        };

        this.changSelected = this.changSelected.bind(this);
        this.getSelected = this.getSelected.bind(this);
        this.boundActionCreators = bindActionCreators(actionCreators, store.dispatch);
    }

    componentWillMount() {
        this.setState({ loading: true });
        axios.get(ApiPaths.GetDepartmentSelectItems).then(response => {

            if (this.props.defaultOption) {
                response.data.unshift({
                    id_DonVi: 0,
                    ten_DonVi: "--Không có đơn vị--"
                });
            }

            this.setState({ departments: response.data });
        }).catch(error => {
            this.boundActionCreators.showAlert({
                variant: "danger",
                content: "Không thể lấy danh sách đơn vị."
            });
        }).then(() => {
            this.setState({ loading: false });
        });
    }

    changSelected(event) {
        if (typeof this.props.onChange === "function") {
            this.props.onChange(event);
        }
    }

    renderItem(department) {
        var prefix = "";
        for (let i = 0; i < department.level - 1; i++) {
            prefix += "--";
        }
        return (
            <option key={department.id_DonVi} value={department.id_DonVi}>{prefix + department.ten_DonVi}</option>
        );
    }

    getSelected() {
        const { departments } = this.state;
        const value = parseInt(this.props.value);
        
        var length = (departments || []).length;
        for (let i = 0; i < length; i++) {
            if (value === departments[i].id_DonVi) {
                return departments[i];
            }
        }

        if (length > 0) {
            return departments[0];
        }

        return null;
    }

    render() {
        const { departments, loading } = this.state;
        const { value } = this.props;
        return (
            loading ?
                <div className={this.props.className}><Spinner animation="border" size="sm" /></div>
                :
            <select name={this.props.name} className={this.props.className} onChange={this.changSelected} value={value || 0}  >
                {
                    (departments || []).map(value => this.renderItem(value))
                }
            </select>
        );
    }
}

export default CbbDepartments;