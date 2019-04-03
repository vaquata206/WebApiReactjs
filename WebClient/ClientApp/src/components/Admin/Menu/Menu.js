import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import MenuItem from './MenuItem';

class Menu extends React.Component {

    isActived(key) {
        return (this.props.menu.activedItems || []).indexOf(key) >= 0;
    }

    render() {
        const menu = this.props.menuItems;
        return (
            <aside className="main-sidebar">
                <section className="sidebar">
                    <ul className="sidebar-menu">
                        <li className="header">Danh sách chức năng</li>
                        <li className={this.isActived(0)? "active": ""}>
                            <Link to="/"><i className="fa fa-dashboard" /><span>Trang chủ</span></Link>
                        </li>
                        {(menu || []).length > 0 ? menu.map(item => <MenuItem item={item} key={item.m_ID} />) : null}
                    </ul>
                </section>
            </aside>
        );
    }
}

export default connect(
    state => state.menu ? { menu: state.menu } : { menu: null }
)(Menu);