import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../../../store/Menu';
import { Link } from 'react-router-dom';

class MenuItem extends React.Component {

    constructor(props) {
        super(props);
        this.clickMenu = this.clickMenu.bind(this);
    }

    componentWillMount() {
        console.log("aaa");
        if (this.props.menu.menuOpen === null && this.isActived(this.props.item.m_ID) && this.props.children) {
            this.props.OpenMenuItem(this.props.item.m_ID);
        }
    }

    isActived(key) {
        var menu = this.props.menu || {};
        return (menu.activedItems || []).indexOf(key) >= 0;
    }

    clickMenu() {
        if (this.props.item.m_ID === this.props.menu.menuOpen) {
            this.props.CloseMenuItem();
        } else {
            this.props.OpenMenuItem(this.props.item.m_ID);
        }
    }

    render() {
        const { item } = this.props;
        const isActive = this.isActived(item.m_ID);
        let isOpen = this.props.menu && this.props.menu.menuOpen === item.m_ID;
        const styleChildren = {
            display: "block"
        };

        const classLi = (item.children ? "treeview" : "") + (isOpen ? " menu-open" : "") + (isActive(item.m_ID) ? " active" : "");
        return (
            <li className={classLi} key={item.m_ID}>
                {item.controler_Name ?
                    <Link to={"/" + item.controler_Name + "/" + (item.action_Name ? item.action_Name: "")}>
                        <i className="fa fa-circle-o" />
                        <span>{item.m_Name}</span>
                    </Link> :
                    <a title={item.m_Name} onClick={this.clickMenu}>
                        <i className="fa fa-dashboard" />
                        <span>{item.m_Name}</span>
                        <span className="pull-right-container">
                            <i className="fa fa-angle-left pull-right" />
                        </span>
                    </a>
                }
                {item.children ?
                    <ul className="treeview-menu" style={isOpen ? styleChildren : {}} key={"c" + item.m_ID}>
                        {item.children.map(i => <MenuItem item={i} key={i.m_ID} menu={this.props.menu} />)}
                    </ul>
                    : null}
            </li>
            );
    }
}

export default connect(
    state => state.menu ? { menu: state.menu } : { menu: null },
    dispatch => bindActionCreators(actionCreators, dispatch)
)(MenuItem);