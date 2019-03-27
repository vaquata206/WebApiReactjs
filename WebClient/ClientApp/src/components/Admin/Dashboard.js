import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import Layout from '../../components/Layout';
import Home from '../../components/Home';
import Counter from '../../components/Counter';
import FetchData from '../../components/FetchData';

class Dashboard extends React.Component {
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

export default Dashboard;