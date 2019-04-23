import React from 'react';
import { Route, Switch } from 'react-router';
import Login from './components/Admin/Login/Login';
import MainPage from './components/MainPage';

class App extends React.Component {
    render() {
        return (
            <Switch>
                <Route path='/login' component={Login} />
                <Route path='/' component={MainPage} />
            </Switch>
        );
    }
}

export default App;
