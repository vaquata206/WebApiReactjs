import React from 'react';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { Dropdown } from 'react-bootstrap';

class CbbDepartments extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            departments: [],
            value: 0
        };

        this.changSelected = this.changSelected.bind(this);
    }

    componentWillMount() {
        this.setState({ value: this.props.value || 0 });
        axios.get(ApiPaths.GetDepartmentSelectItems).then(response => {
            (response.data || []).forEach((value) => {
                if (value.id_DonVi + "" === this.state.value) {
                    this.props.onChange(value);
                }
            });

            this.setState({ departments: response.data });
        });
    }

    componentWillReceiveProps(nextProps) {
        (this.state.departments || []).forEach((value) => {
            if (value.id_DonVi + "" === nextProps.value) {
                this.props.onChange(value);
            }  
        });
        this.setState({ value: nextProps.value || 0 });
    }

    changSelected(event) {
        const { departments } = this.state;
        let item = null;
        if ((departments || []).length > 0 && event.target.selectedIndex > 0) {
            item = departments[event.target.selectedIndex - 1];
        }

        if (item) {
            this.setState({ value: item.id_DonVi });
        } else {
            this.setState({ value: 0 });
        }

        if (typeof this.props.onChange === "function") {
            this.props.onChange(item);
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

    render() {
        const { departments, value } = this.state;
        return (
            <select className={this.props.className} onChange={this.changSelected} value={value || 0} >
                <option key={0} value={0}>--Không có đơn vị cha--</option>
                {
                    (departments || []).map(value => this.renderItem(value))
                }
            </select>
        );
    }
}

export default CbbDepartments;