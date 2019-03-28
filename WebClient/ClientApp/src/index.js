import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap-theme.css';
import './assets/css/site.css';
import './index.css';
import './assets/css/google-font.css';
import './assets/dist/css/AdminLTE.min.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'react-router-redux';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import history from './store/history';
import { store, persistor } from './store/store';
import { PersistGate } from 'redux-persist/lib/integration/react';

const rootElement = document.getElementById('root');

ReactDOM.render(
    <Provider store={store}>
        <PersistGate loading={null} persistor={persistor}>
            <ConnectedRouter history={history}>
                <App />
            </ConnectedRouter>
        </PersistGate>
    </Provider>,
    rootElement);

registerServiceWorker();
