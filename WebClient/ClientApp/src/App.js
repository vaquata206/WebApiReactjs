import React from 'react';
import { Route, Switch, Redirect } from 'react-router';
import Login from './components/Admin/Login/Login';
import Dashboard from './components/Admin/Dashboard';

class App extends React.Component {

    constructor(props) {
        super(props);
    }

    isLoggedIn() {
        const user = this.props.user;
        return user ? true : false;
    }

    render() {
        return (
            <Switch>
                <Route path='/login' component={Login} />
                <Route path='/' render={() => this.isLoggedIn() ? <Dashboard /> : <Redirect to="/login" />} />
            </Switch>
        );
    }
}

export default App;
