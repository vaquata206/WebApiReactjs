import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../../../store/Menu';
import MenuItem from './MenuItem';
import axios from 'axios';

class Menu extends React.Component {

    constructor(props) {
        super(props);
    }

    componentWillMount() {
        axios.get("api/feature/getmenu").then(response => {
            response.data.unshift({
                m_ID: 0,
                controler_Name: "/",
                action_Name: null,
                m_Name: "Trang chủ",
                actived: true,
                opened: false
            });

            this.props.SetMenu(response.data);
        });
    }

    isActived(key) {
        return (this.props.menu.activedItems || []).indexOf(key) >= 0;
    }

    render() {
        const { menu } = this.props;
        return (
            <aside className="main-sidebar">
                <section className="sidebar">
                    <ul className="sidebar-menu">
                        <li className="header">Danh sách chức năng</li>
                        {(menu.menuItems || []).length > 0 ? menu.menuItems.map(item =>
                            <MenuItem item={item} key={item.m_ID} CloseMenuItem={this.props.CloseMenuItem} OpenMenuItem={this.props.OpenMenuItem}/>) :
                            null}
                    </ul>
                </section>
            </aside>
        );
    }
}

export default connect(
    state => state.menu ? { menu: state.menu } : { menu: null },
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Menu);