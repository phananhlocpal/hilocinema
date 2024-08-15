import axios from 'axios';

export const FETCH_MOVIE_REQUEST = 'FETCH_MOVIE_REQUEST';
export const FETCH_MOVIE_SUCCESS = 'FETCH_MOVIE_SUCCESS';
export const FETCH_MOVIE_FAILURE = 'FETCH_MOVIE_FAILURE';

export const fetchMovie = (url) => async (dispatch) => {
    dispatch({ type: FETCH_MOVIE_REQUEST });
    try {
        const token = localStorage.getItem('token');
        if (!token) {
            throw new Error('No token found. Please authenticate first.');
        }

        const response = await axios.get(`http://localhost:8000/MovieService/url/${url}`, {
            headers: {
                Authorization: `Bearer ${token}`,
                Site: 'public'
            }
        });

        dispatch({ type: FETCH_MOVIE_SUCCESS, payload: response.data });
    } catch (error) {
        dispatch({ type: FETCH_MOVIE_FAILURE, error: error.message });
    }
};