import React from 'react';
import { Route, Switch } from 'react-router';
import Login from './components/Admin/Login/Login';
import Dashboard from './components/Admin/Dashboard';

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
