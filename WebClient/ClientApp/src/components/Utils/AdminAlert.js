import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Alert } from 'react-bootstrap';
import { actionCreators } from "./../../store/AdminAlert";

class AdminAlert extends React.Component {

    constructor(props) {
        super(props);
        this.handleDismiss = this.handleDismiss.bind(this);
    }

    handleDismiss() {
        this.props.hideAlert();
    }

    render() {
        const { adminAlert } = this.props;
        return (adminAlert.show ?
            <Alert variant={adminAlert.variant} onClose={this.handleDismiss} dismissible>
                {adminAlert.content}
            </Alert> : null
            );
    }
}

export default connect(
    state => state.adminAlert ? { adminAlert: state.adminAlert } : { adminAlert: null },
    dispatch => bindActionCreators(actionCreators, dispatch)
)(AdminAlert);