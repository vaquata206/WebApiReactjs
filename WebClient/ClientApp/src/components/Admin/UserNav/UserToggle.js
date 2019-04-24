import React from 'react';
import { connect } from 'react-redux';

class UserTrigger extends React.Component {
    constructor(props, context) {
        super(props, context);

        this.handleClick = this.handleClick.bind(this);
    }

    handleClick(e) {
        e.preventDefault();

        this.props.onClick(e);
    }

    render() {
        const { username } = this.props.user;
        return (
            <a className="user-toggle" href="" onClick={this.handleClick}>
                <img src="/images/user2-160x160.png" className="user-image" alt="User" />
                <span className="hidden-xs">{username}</span>
            </a>
        );
    }
}

export default connect(
    state => state.auth ? { user: state.auth.user } : { user: null }
)(UserTrigger);