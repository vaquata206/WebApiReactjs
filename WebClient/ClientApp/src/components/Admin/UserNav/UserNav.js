import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import UserTrigger from "./UserToggle";
import { Dropdown } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { actionCreators } from './../../../store/Auth';

class UserNav extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.logout = this.logout.bind(this);
    }

    logout() {
        this.props.requestLogout();
    }

    render() {
        let { name, department } = this.props.user;
        return (
            <Dropdown className="user-menu-nav">
                <Dropdown.Toggle as={UserTrigger} id="dropdown-custom-components">
                    Custom toggle
                </Dropdown.Toggle>

                <Dropdown.Menu>
                    <div className="user-menu-body">
                        <li className="user-header">
                            <img src="/images/user2-160x160.png" className="rounded-circle" alt="User" />
                            <p>
                                {name}
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
                    </div>
                </Dropdown.Menu>
            </Dropdown>
        );
    }
}

export default connect(
    state => state.auth ? { user: state.auth.user } : { user: null },
    dispatch => bindActionCreators(actionCreators, dispatch)
)(UserNav);