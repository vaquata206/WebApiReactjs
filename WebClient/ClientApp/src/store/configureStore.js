import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { routerReducer, routerMiddleware } from 'react-router-redux';
import { persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import * as Auth from './Auth';
import * as Menu from './Menu';
import * as ConfirmationModal from './ConfirmationModal';
import * as AdminAlert from './AdminAlert';
import * as TitleHeader from './TitleHeader';

export default function configureStore(history, initialState) {
    const reducers = {
        auth: persistReducer(Auth.persistConfig, Auth.reducer),
        menu: Menu.reducer,
        confirmationModal: ConfirmationModal.reducer,
        adminAlert: AdminAlert.reducer,
        titleHeader: TitleHeader.reducer
    };

    const middleware = [
        thunk,
        routerMiddleware(history)
    ];

    // In development, use the browser's Redux dev tools extension if installed
    const enhancers = [];
    const isDevelopment = process.env.NODE_ENV === 'development';
    if (isDevelopment && typeof window !== 'undefined' && window.devToolsExtension) {
        enhancers.push(window.devToolsExtension());
    }

    const rootReducer = combineReducers({
        ...reducers,
        routing: routerReducer
    });

    const persistConfig = {
        key: 'root',
        storage: storage,
        whitelist: ['auth']
    };

    const pReducer = persistReducer(persistConfig, rootReducer);

    return createStore(
        pReducer,
        initialState,
        compose(applyMiddleware(...middleware), ...enhancers)
    );
}
