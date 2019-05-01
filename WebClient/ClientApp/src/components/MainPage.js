import React from 'react';
import { connect } from 'react-redux';
import { Redirect } from 'react-router';
import { Route, Switch } from 'react-router-dom';
import { actionCreators } from "../store/Menu";
import { bindActionCreators } from 'redux';
import Layout from './Layout';
import Home from './Home/Home';
import Permission from "./Permission/Permission";
import PermissionManager from "./Permission/PermissionManager";
import PermissionDetail from "./Permission/PermissionDetail";
import AppManager from "./Permission/AppManager";
import AppDetail from "./Permission/AppDetail";
import Employee from "./Employee/Employee";
import EmployeeDetail from "./Employee/EmployeeDetail";
import Department from './Department/Department';
import DepartmentDetail from './Department/DepartmentDetail';
import axios from 'axios';
import { ApiPaths } from '../helpers/api';

class MainPage extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: false
        };
    }

    componentWillMount() {
        const { user } = this.props;
        if (user && user.token) {
            this.setState({ loading: true });
            axios.get(ApiPaths.features.getMenu).then(response => {
                response.data.unshift({
                    m_ID: 0,
                    controler_Name: "/",
                    action_Name: null,
                    m_Name: "Trang chủ",
                    actived: true,
                    opened: false
                });
                this.props.SetMenu(response.data);
                this.setState({ loading: false });
            });
        }
    }

    render() {
        const { loading } = this.state;
        const { user } = this.props;
        if (!user || !user.token) {
            return (
                <Redirect to="/login" />
            );
        } else if (loading) {
            return (
                <div className="wrap-loading-page">
                    <div className="loading">
                        <div className="bounceball" />
                        <div className="text">NOW LOADING</div>
                    </div>
                </div>);
        } else {
            return (
                <Layout>
                    <Route exact path='/' component={Home} />
                    <Switch>
                        <Route path="/permission/apps/add" component={AppDetail} />
                        <Route path="/permission/apps/:id" component={AppDetail} />
                        <Route path="/permission/apps" component={AppManager} />
                        <Route path='/permission/list' component={PermissionManager} />
                        <Route path='/permission/create' component={PermissionDetail} />
                        <Route path='/permission/detail/:id' component={PermissionDetail} />
                        <Route path='/permission' component={Permission} />
                    </Switch>
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
    state => state.auth ? { user: state.auth.user } : { user: null },
    dispatch => bindActionCreators(actionCreators, dispatch)
)(MainPage);