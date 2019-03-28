import history from './history';
import configureStore from './configureStore';
import { persistStore } from 'redux-persist';

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = window.initialReduxState;

export const store = configureStore(history, initialState);
export const persistor = persistStore(store);
