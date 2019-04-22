import React from 'react';
import Tree from "./Tree";
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";

class DepartmentTree extends React.Component {

    constructor(props) {
        super(props);
        this.getDataTree = this.getDataTree.bind(this);
        this.refresh = this.refresh.bind(this);
    }

    refresh() {
        this.tree.refresh();
    }

    getDataTree(node) {
        var parentId = (node || {}).id || 0;

        if (!this.props.showEmployee) {
            return axios.get(ApiPaths.GetDepartmentByParentId + "?id=" + parentId).then(response => {
                var list = [];
                response.data.forEach(value => {
                    list.push({
                        id: value.id_DonVi,
                        title: value.ten_DonVi,
                        subtitle: value.ten_DonVi,
                        children: value.soLuong_DV_Con > 0,
                        open: false,
                        icon: "treenode-folder"
                    });
                });

                return list;
            });
        } else {
            let url = (node || {}).typeNode === "Employee" ? ApiPaths.GetTreeNodeAccounts : ApiPaths.GetChildNodes;
            return axios.get(url + "?id=" + parentId).then(response => {
                response.data.forEach((value, index) => {
                    if (value.typeNode === "Department") {
                        value.icon = "treenode-folder";
                    } else if (value.typeNode === "Employee") {
                        value.icon = "treenode-file";
                    }
                });

                return response.data;
            });
        }
    }

    render() {
        return (
            <Tree nodes={this.getDataTree} onChange={this.props.onChange} ref={c => { this.tree = c;}} />
            );
    }
}

export default DepartmentTree;