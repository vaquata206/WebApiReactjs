import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { actionCreators as authActionCreators } from '../store/Auth';
import { actionCreators as menuActionCreators } from '../store/Menu';
import axios from 'axios';
import Menu from './Admin/Menu/Menu';

const logoContent = {
    fontSize: "27px",
    fontWeight: "bolder"
};

class Layout extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isSidebarCollapsed: false,
            isShowProfile: false,
            menu: {}
        };
        this.handleHamburger = this.handleHamburger.bind(this);
        this.clickProfile = this.clickProfile.bind(this);
        this.logout = this.logout.bind(this);
        this.findActivedItems = this.findActivedItems.bind(this);
        this.activeMenuItems = this.activeMenuItems.bind(this);

    }

    componentWillMount() {
        axios.get("api/feature/getmenu").then(response => {
            this.setState({ menu: response.data });
        });

        // Home page is default;
        this.props.ActiveMenuItems();
    }

    componentDidMount() {
        var heightContent = window.innerHeight - this.mainHeader.offsetHeight - this.mainFooter.offsetHeight;
        this.contentWrapper.setAttribute("style", "min-height: " + heightContent+"px;");
    }

    shouldComponentUpdate(nextProps, nextState) {
        this.activeMenuItems(nextState.menu);
        return true;
    }

    handleHamburger() {
        this.setState({ isSidebarCollapsed: !this.state.isSidebarCollapsed });
    }

    clickProfile() {
        this.setState({ isShowProfile: !this.state.isShowProfile });
    }

    logout() {
        this.props.requestLogout();
    }

    activeMenuItems(menu) {
        const paths = window.location.pathname.replace(/(^\/+)|(\/+$)/g, "").split("/");
        let controller;
        let actionName;
        let l = (paths || []).length;
        l = l > 2 ? 2 : l;
        switch (l) {
            case 2:
                actionName = paths[1] || null;
                break;
            case 1:
                controller = paths[0] || null;
                actionName = paths[1] || null;
                break;
            default:
                controller = null;
                actionName = null;
        }

        if (actionName === "index" || actionName === "#") {
            actionName = "";
        }

        this.props.CloseMenuItem();
        if (!controller && !actionName) {
            this.props.ActiveMenuItems();
        } else {
            var items = this.findActivedItems(controller, actionName, menu);
            if ((items || []).length > 0) {
                this.props.ActiveMenuItems(items);
            } else {
                this.props.ActiveMenuItems([-1]);
            }
        }
    }

    findActivedItems(controller, actionName, menu) {
        var activedItems = [];
        (menu || []).forEach((value) => {
            let l = [];
            if ((value.children || []).length > 0) {
                l = this.findActivedItems(controller, actionName, value.children);
            }

            if (l.length > 0 || !(value.controler_Name !== controller || value.action_Name !== actionName)) {
                l.push(value.m_ID);
            }

            activedItems = activedItems.concat(l);
        });

        return activedItems;
    }

    render() {
        let { userName, department } = this.props.user;
        let menu = this.state.menu;
        return (
            <div className={"hold-transition skin-blue sidebar-mini" + (this.state.isSidebarCollapsed ? " sidebar-collapse":"")}>
                <div className="wrapper">
                    <header className="main-header" ref={c => this.mainHeader = c}>
                        <Link to="/" className="logo" >
                            <span className="logo-mini">
                                <img src="/images/logodn.jpg" alt="Logo" />
                            </span>
                            <img className="logo-image" src="/images/logodn.jpg" alt="Logo" />
                            <span style={logoContent}>Đà Nẵng</span>
                        </Link>
                        <nav className="navbar navbar-static-top">
                            <a className="sidebar-toggle" onClick={this.handleHamburger}>
                                <span className="sr-only">Toggle navigation</span>
                            </a>
                            <div className="header-name-app">
                                <p id="headerTitle">BIÊN LAI ĐIỆN TỬ</p>
                            </div>
                            <div className="navbar-custom-menu">
                                <ul className="nav navbar-nav">
                                    <li className={"dropdown user user-menu" + (this.state.isShowProfile?" open":"")}>
                                        <a className="dropdown-toggle" onClick={this.clickProfile}>
                                            <img src="/images/user2-160x160.png" className="user-image" alt="User" />
                                            <span className="hidden-xs">{userName}</span>
                                        </a>
                                        <ul className="dropdown-menu">
                                            <li className="user-header">
                                                <img src="/images/user2-160x160.png" className="img-circle" alt="User" />
                                                <p>
                                                    {userName}
                                                    <strong style={{ display: "block" }}>
                                                        {department}
                                                    </strong>
                                                </p>
                                            </li>
                                            <li className="user-footer">
                                                <div className="pull-left">
                                                    <Link to="/" className="btn btn-default btn-flat">Hồ sơ</Link>
                                                </div>
                                                <div className="pull-right">
                                                    <a className="btn btn-default btn-flat" onClick={this.logout}>Đăng xuất</a>
                                                </div>
                                            </li>
                                        </ul>
                                    </li>
                                </ul>
                            </div>
                        </nav>
                    </header>
                    <Menu menuItems={menu} />
                    <div className="content-wrapper" ref={c => this.contentWrapper = c}>
                        <section className="content-header">
                            <h1>Page 1</h1>
                        </section>
                        {this.props.children}
                    </div>
                    <footer className="main-footer" ref={c => this.mainFooter = c}>
                        <div className="pull-right hidden-xs">
                            <b>Phiên bản</b> 1.0.0
                        </div>
                        <strong>Bản quyền ©<a href="https://danang.vnpt.vn/">VNPT Đà Nẵng</a></strong>
                    </footer>
                </div>
            </div>
        );
    }
}
export default connect(
    state => state.auth ? { user: state.auth.user } : { user: null },
    dispatch => bindActionCreators({ ...authActionCreators, ...menuActionCreators }, dispatch))(Layout);
