import React from 'react';
import Tree from "./Tree";
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";

class DepartmentTree extends React.Component {

    constructor(props) {
        super(props);
        this.getDataTree = this.getDataTree.bind(this);
    }

    getDataTree(node) {
        var parentId = (node || {}).id || 0;
        var url = ApiPaths.GetDepartmentByParentId + "?id=" + parentId;
        return axios.get(url).then(response => {
            var list = [];
            response.data.forEach(value => {
                list.push({
                    id: value.id_DonVi,
                    title: value.ten_DonVi,
                    subtitle: value.ten_DonVi,
                    children: value.soLuong_DV_Con > 0,
                    open: false
                });
            });

            return list;
        });
    }

    render() {
        return (
            <Tree nodes={this.getDataTree} onChange={this.props.onChange} />
            );
    }
}

export default DepartmentTree;