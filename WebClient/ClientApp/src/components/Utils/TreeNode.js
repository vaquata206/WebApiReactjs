import React from 'react';
import ReactLoading from 'react-loading';

class TreeNode extends React.Component {

    constructor(props) {
        super(props);
        this.eventClickCollap = this.eventClickCollap.bind(this);
        this.eventClickItem = this.eventClickItem.bind(this);
        this.state = {
            children: this.props.children,
            open: this.props.node.open,
            loading: false
        };
    }

    eventClickCollap() {
        const { node } = this.props;
        const { children, open, loading } = this.state;

        if (loading || !children) {
            return;
        }

        if (!open && typeof children === "function") {
            this.setState({ loading: true });
            children(node).then(data => {
                this.props.node.children = (data || []).length > 0 ? data : null;
                this.setState({
                    children: this.props.node.children,
                    open: (this.props.node.children || []).length > 0,
                    loading: false
                });
            });
        }

        this.setState({ open: !open });
    }

    eventClickItem() {
        this.props.onClick(this.props.node);
    }

    getChildren(node) {
        const { children } = this.props;
        if (node.children && typeof children === "function") {
            return children;
        } else {
            return node.children;
        }
    }

    renderChildren() {
        const { children, open, loading } = this.state;
        if (open && children) {
            return (
                <div className="treenode-children">
                    {loading ?
                        <div className="treenode-loading" style={{ marginLeft: "50px" }}>
                            <ReactLoading type="spinningBubbles" color={"#000000"} height={15} width={15} />
                            <span>Đang tải</span>
                        </div> :
                        Array.isArray(children) ?
                            (children || []).map((child, index) => {
                                return (
                                    <TreeNode
                                        node={child}
                                        children={this.getChildren(child)}
                                        key={child.id}
                                        isLast={index === children.length - 1}
                                        onClick={this.props.onClick}
                                    />);
                            }) :
                            null
                    }
                </div>
            );
        }

        return null;
    }

    render() {
        const { node, isLast } = this.props;
        const { children, open } = this.state;

        let classItem = "";
        if (open && (children || []).length > 0 || isLast) {
            classItem += "treenode-last";
        }

        classItem += " treenode-item";
        if (children) {
            classItem += open ? " treenode-open" : " treenode-close";
        } else {
            classItem += " treenode-leaf";
        }

        return (
            <div className="treenode-group">
                <div className={classItem}>
                    <i className="treenode-icon treenode-ocl" onClick={this.eventClickCollap} />
                    <div className={"treenode-anchor" + (node.actived ? " actived" : "")} title={node.subtitle} onClick={this.eventClickItem}>
                        <i className="treenode-icon treenode-folder " />
                        <span>{node.title}</span>
                    </div>
                </div>
                {this.renderChildren()}
            </div>
        );
    }
}

export default TreeNode;
