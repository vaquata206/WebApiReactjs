import React from 'react';
import { connect } from 'react-redux';
import { Redirect } from 'react-router';
import { Route } from 'react-router-dom';
import Layout from '../Layout';
import Home from '../Home';
import Feature from '../Feature/Feature';
import Department from '../Department/Department';
import DepartmentDetail from '../Department/DepartmentDetail';

class MainPage extends React.Component {

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
                    <Route path='/feature' component={Feature} />
                    <Route exact path='/department' component={Department} />
                    <Route path='/department/:id' component={DepartmentDetail}/>
                </Layout>
            );
        }
    }
}

export default connect(
    state => state.auth ? { user: state.auth.user } : { user: null }
)(MainPage);