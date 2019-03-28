import React from 'react';
import { Route, Switch, Redirect } from 'react-router';
import Login from './components/Admin/Login/Login';
import Dashboard from './components/Admin/Dashboard';
import { store } from './store/store';

class App extends React.Component {
    render() {
        return (
            <Switch>
                <Route path='/login' component={Login} />
                <Route path='/' component={Dashboard} />
            </Switch>
        );
    }
}

export default App;
