import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { actionCreators as menuActionCreators } from '../store/Menu';
import ConfirmationModal from "./Utils/ConfirmationModal";
import Menu from './Admin/Menu/Menu';
import UserNav from "./Admin/UserNav/UserNav";
import AdminAlert from "./Utils/AdminAlert";

const logoContent = {
    fontSize: "27px",
    fontWeight: "bolder"
};

class Layout extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isSidebarCollapsed: false,
            menu: {}
        };
        this.handleHamburger = this.handleHamburger.bind(this);
    }

    componentDidMount() {
        var heightContent = window.innerHeight - this.mainHeader.offsetHeight - this.mainFooter.offsetHeight;
        this.contentWrapper.setAttribute("style", "min-height: " + heightContent+"px;");
    }

    shouldComponentUpdate(nextProps, nextState) {
        nextProps.ActiveItemMapPath();
        return true;
    }

    handleHamburger() {
        this.setState({ isSidebarCollapsed: !this.state.isSidebarCollapsed });
    }

    render() {
        return (
            <div className={"hold-transition skin-blue sidebar-mini" + (this.state.isSidebarCollapsed ? " sidebar-collapse" : "")}>
                <div className="wrapper" >
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
                            <UserNav />
                        </nav>
                    </header>
                    <Menu />
                    <div className="content-wrapper" ref={c => this.contentWrapper = c}>
                        <AdminAlert />
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
                <ConfirmationModal />
            </div>
        );
    }
}
export default connect(
    state => state.auth ? { user: state.auth.user } : { user: null },
    dispatch => bindActionCreators(menuActionCreators, dispatch))(Layout);
