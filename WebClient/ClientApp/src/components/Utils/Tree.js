import React from 'react';
import TreeNode from "./TreeNode";
import ReactLoading from 'react-loading';


class Tree extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            nodes: [],
            loading: false
        };
        this.changeActived = this.changeActived.bind(this);
        this.refresh = this.refresh.bind(this);
    }

    componentWillMount() {
        this.refresh();
    }

    refresh() {
        const { nodes } = this.props;
        if (Array.isArray(nodes)) {
            this.setState({ nodes: nodes });
        } else {
            this.setState({ loading: true });
            nodes(null).then(data => {
                this.setState({ nodes: data, loading: false });
            });
        }
    }

    getChildren(node) {
        const { nodes } = this.props;
        if (node.children) {
            if (Array.isArray(nodes)) {
                return node.children;
            } else {
                return nodes;
            }
        } else {
            return node.children;
        }
    }

    changeActived(node) {
        let { nodes } = this.state;
        const { multiSelect } = this.props;
        if (!multiSelect) {
            this.selectOnlyNode(nodes, node);
        }

        this.setState({ nodes: nodes });

        if (typeof this.props.onChange === "function") {
            this.props.onChange(node);
        }
    }

    selectOnlyNode(nodes, node) {
        nodes.forEach(value => {
            if (node.id !== value.id || node.typeNode !== value.typeNode) {
                value.actived = false;
            } else {
                value.actived = !value.actived;
            }

            if (value.children && Array.isArray(value.children)) {
                this.selectOnlyNode(value.children, node);
            }
        });
    }
   
    render() {
        const { nodes, loading } = this.state;
        return (
            <div className="treeview">
                {loading ?
                    <div className="treenode-loading" style={{ marginLeft: "50px" }}>
                        <ReactLoading type="spinningBubbles" color={"#000000"} height={15} width={15} />
                        <span>Đang tải</span>
                    </div> :
                    (nodes || []).length > 0 ?
                    nodes.map(
                        (node, index) => {
                            return (
                                <TreeNode node={node}
                                    children={this.getChildren(node)}
                                    key={node.id}
                                    isLast={index === nodes.length - 1}
                                    onClick={this.changeActived}
                                />);
                            }
                        ) :
                        <span>Không có dữ liệu</span>}
            </div>
        );
    }
}

export default Tree;
