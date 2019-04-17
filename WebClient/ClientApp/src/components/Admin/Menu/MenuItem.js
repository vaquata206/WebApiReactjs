import React from 'react';
import { Link } from 'react-router-dom';

class MenuItem extends React.Component {

    constructor(props) {
        super(props);
        this.eventClickCollap = this.eventClickCollap.bind(this);
    }

    eventClickCollap() {
        const item = this.props.item;
        if (item.opened) {
            this.props.CloseMenuItem(item.m_ID);
        } else {
            this.props.OpenMenuItem(item.m_ID);
        }
    }

    render() {
        const { item } = this.props;
        const styleChildren = {
            display: "block"
        };

        const classLi = (item.children ? "treeview" : "") + (item.opened ? " menu-open" : "") + (item.actived ? " active" : "");
        const path = (item.controler_Name !== "/") ? "/" + item.controler_Name + "/" + (item.action_Name ? item.action_Name : "") : "/";
        return (
            <li className={classLi} key={item.m_ID}>
                {item.controler_Name ?
                    <Link to={path}>
                        <i className="fa fa-circle-o" />
                        <span>{item.m_Name}</span>
                    </Link> :
                    <a title={item.m_Name} onClick={this.eventClickCollap}>
                        <i className="fa fa-dashboard" />
                        <span>{item.m_Name}</span>
                        <span className="pull-right-container">
                            <i className="fa fa-angle-left pull-right" />
                        </span>
                    </a>
                }
                {item.children ?
                    <ul className="treeview-menu" style={item.opened ? styleChildren : {}} key={"c" + item.m_ID}>
                        {item.children.map(i => <MenuItem item={i} key={i.m_ID} menu={this.props.menu} />)}
                    </ul>
                    : null}
            </li>
            );
    }
}

export default MenuItem;