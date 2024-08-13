import { combineReducers } from "redux";
import movieReducer from "./movieReducer";

export const allReducers = combineReducers({
    movie: movieReducer,
});
