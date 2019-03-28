import React from 'react';
import { connect } from 'react-redux';
import { Route, Redirect } from 'react-router-dom';
import Layout from '../../components/Layout';
import Home from '../../components/Home';
import Counter from '../../components/Counter';
import FetchData from '../../components/FetchData';

class Dashboard extends React.Component {

    componentWillMount() {
        const { history, user } = this.props;
        if (!user) {
            history.push("/login");
        }
    }

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route path='/counter' component={Counter} />
                <Route path='/fetchdata/:startDateIndex?' component={FetchData} />
            </Layout>
        );
    }
}

export default connect(
    state => state.auth ? { user: state.auth.user } : { user: null }
)(Dashboard);