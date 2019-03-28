import React from 'react';
import { connect } from 'react-redux';
import { Route, Switch, Redirect } from 'react-router';
import Login from './components/Admin/Login/Login';
import Dashboard from './components/Admin/Dashboard';

function aa() {

}

class App extends React.Component {

    constructor(props) {
        super(props);
    }

    isLoggedIn() {
        debugger;
        const user = this.props.user;
        return user ? true : false;
    }

    render() {
        //return (
        //    <Switch>
        //        <Route path='/login' component={Login} />
        //        <Route path='/' render={() => this.isLoggedIn() ? <Dashboard /> : <Redirect to="/login" />} />
        //    </Switch>
        //);

        return (
            <Switch>
                <Route path='/login' component={Login} />
                <Route path='/' component={Dashboard} />
            </Switch>
        );
    }
}

export default App;
