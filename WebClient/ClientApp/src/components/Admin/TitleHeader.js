import React from 'react';
import { connect } from 'react-redux';

class TitleHeader extends React.Component {
    render() {
        return (
            <section className="content-header">
                <h1>{this.props.titleHeader.title}</h1>
            </section>
        );
    }
}

export default connect(state => state.titleHeader ? { titleHeader: state.titleHeader } : { titleHeader: null })(TitleHeader);