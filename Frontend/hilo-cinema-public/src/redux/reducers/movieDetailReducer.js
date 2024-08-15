// reducers/movieReducer.js
import { FETCH_MOVIE_REQUEST, FETCH_MOVIE_SUCCESS, FETCH_MOVIE_FAILURE } from '../actions/movieDetailAction';

const initialState = {
    movie: null,
    loading: false,
    error: null,
};

const movieDetailReducer = (state = initialState, action) => {
    switch (action.type) {
        case FETCH_MOVIE_REQUEST:
            return { ...state, loading: true };
        case FETCH_MOVIE_SUCCESS:
            return { ...state, loading: false, movie: action.payload };
        case FETCH_MOVIE_FAILURE:
            return { ...state, loading: false, error: action.error };
        default:
            return state;
    }
};

export default movieDetailReducer;
