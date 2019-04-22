import React from 'react';
import { connect } from 'react-redux';
import { Redirect } from 'react-router';
import { Route, Switch } from 'react-router-dom';
import Layout from '../Layout';
import Home from '../Home';
import Employee from "../Employee/Employee";
import EmployeeDetail from "../Employee/EmployeeDetail";
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
                    <Switch>
                        <Route path='/employee/detail/:code' component={EmployeeDetail} />
                        <Route path='/employee/create' component={EmployeeDetail} />
                        <Route path='/employee' component={Employee} />
                    </Switch>
                    <Switch>
                        <Route path='/department/detail/:id' component={DepartmentDetail} />
                        <Route path='/department/create' component={DepartmentDetail} />
                        <Route path='/department' component={Department} />
                    </Switch>
                </Layout>
            );
        }
    }
}

export default connect(
    state => state.auth ? { user: state.auth.user } : { user: null }
)(MainPage);