import axios from 'axios';

export const AUTH_SUCCESS = 'AUTH_SUCCESS';
export const AUTH_FAILURE = 'AUTH_FAILURE';

export const authenticateUser = () => async (dispatch) => {
    try {
        const response = await axios.post('http://localhost:8000/CustomerAuthen', {
            email: "Guest",
            password: "Guest",
            site: "public"
        });
        
        const token = response.data.jwtToken;
        localStorage.setItem('token', token); // Save token to localStorage

        // Dispatch success action with token as payload
        dispatch({ type: AUTH_SUCCESS, payload: token });
        
        return Promise.resolve(true);  // Indicate success
    } catch (error) {
        // Dispatch failure action with error message as payload
        dispatch({ type: AUTH_FAILURE, error: error.message });
        
        return Promise.resolve(false); // Indicate failure
    }
};
