import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { actionCreators } from '../store/Auth';

const logoContent = {
    fontSize: "27px",
    fontWeight: "bolder"
};

class Layout extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isSidebarCollapsed: false,
            isShowProfile: false
        };
        this.handleHamburger = this.handleHamburger.bind(this);
        this.clickProfile = this.clickProfile.bind(this);
        this.logout = this.logout.bind(this);
    }

    componentDidMount() {
        var heightContent = window.innerHeight - this.mainHeader.offsetHeight - this.mainFooter.offsetHeight;
        this.contentWrapper.setAttribute("style", "min-height: " + heightContent+"px;");
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

    render() {
        var pathName = window.location.pathname;
        let { userName, department } = this.props.user;
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
                                            <img src="/images/user2-160x160.png" className="user-image" alt="User Image" />
                                            <span className="hidden-xs">{userName}</span>
                                        </a>
                                        <ul className="dropdown-menu">
                                            <li className="user-header">
                                                <img src="/images/user2-160x160.png" className="img-circle" alt="User Image" />
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
                    <aside className="main-sidebar">
                        <section className="sidebar">
                            <ul className="sidebar-menu">
                                <li className="header">Danh sách chức năng</li>
                                <li className="active">
                                    <Link to="/"><i className="fa fa-dashboard" /><span>Trang chủ</span></Link>
                                </li>
                                <li>
                                    <Link to="/counter"><i className="fa fa-dashboard" /><span>counter</span></Link>
                                </li>
                            </ul>
                        </section>
                    </aside>
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
    dispatch => bindActionCreators(actionCreators, dispatch))(Layout);
