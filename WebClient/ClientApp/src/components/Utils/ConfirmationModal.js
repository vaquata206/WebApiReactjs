import React from 'react';
import { connect } from 'react-redux';
import { Modal, Button } from 'react-bootstrap';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../../store/ConfirmationModal';

class ConfirmationModal extends React.Component {

    constructor(props) {
        super(props);
        this.handleClose = this.handleClose.bind(this);
    }

    handleClose() {
        this.props.hideModal();
    }

    render() {
        const { confirmationModal } = this.props;
        return (
            <Modal show={confirmationModal.show} onHide={this.handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>{confirmationModal.title}</Modal.Title>
                </Modal.Header>
                <Modal.Body>{confirmationModal.body}</Modal.Body>
                <Modal.Footer>
                    <Button variant="danger" onClick={confirmationModal.cancelButton.handle || this.handleClose}>
                        {confirmationModal.cancelButton.title}
                    </Button>
                    <Button variant="primary" onClick={confirmationModal.okButton.handle || this.handleClose}>
                        {confirmationModal.okButton.title}
                    </Button>
                </Modal.Footer>
            </Modal>
        );
    }
}

export default connect(
    state => state.confirmationModal ? { confirmationModal: state.confirmationModal } : { confirmationModal: null },
    dispatch => bindActionCreators(actionCreators, dispatch))
    (ConfirmationModal);