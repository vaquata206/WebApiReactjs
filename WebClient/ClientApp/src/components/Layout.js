import React from 'react';
import { Link } from 'react-router-dom';

const logoContent = {
    fontSize: "27px",
    fontWeight: "bolder"
};

class Layout extends React.Component {
    render() {
        return (
            <div className="hold-transition skin-blue sidebar-mini">
                <div className="wrapper">
                    <header className="main-header">
                        <Link to="/" className="logo" >
                            <span className="logo-mini">
                                <img src="/images/logodn.jpg" alt="Logo" />
                            </span>
                            <img className="logo-image" src="/images/logodn.jpg" alt="Logo" />
                            <span style={logoContent}>Đà Nẵng</span>
                        </Link>
                    </header>
                    {this.props.children}
                </div>
            </div>
        );
    }
}
export default Layout;
