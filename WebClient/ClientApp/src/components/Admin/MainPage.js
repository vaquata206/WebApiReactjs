import React from 'react';
import { connect } from 'react-redux';
import { Redirect } from 'react-router';
import { Route } from 'react-router-dom';
import Layout from '../Layout';
import Home from '../Home';
import Counter from '../Counter';
import FetchData from '../FetchData';

class MainPage extends React.Component {

    componentWillMount() {
        
    }

    render() {
        const { user } = this.props;
        if (!user || !user.token) {
            return (
                <Redirect to="/login" />
            );
        } else {
            return (
                <Layout>
                    <Route exact path='/' component={Home} />
                    <Route path='/counter' component={Counter} />
                    <Route path='/fetchdata/:startDateIndex?' component={FetchData} />
                </Layout>
            );
        }
    }
}

export default connect(
    state => state.auth ? { user: state.auth.user } : { user: null }
)(MainPage);