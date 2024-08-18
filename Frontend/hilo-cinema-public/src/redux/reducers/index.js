import { combineReducers } from "redux";
import bookingReducer from "./bookingReducer";
import detailPostReducer from "./detailPostReducer";
import homeReducer from "./homeReducer";
import movieDetailReducer from "./movieDetail/movieDetailReducer";
import postListReducer from "./postListReducer";
import theaterReducer from "./theaterReducer";
import scheduleReducer from "./movieDetail/scheduleReducer";

export const allReducers = combineReducers({
  booking: bookingReducer,
  detailPost: detailPostReducer,
  home: homeReducer,
  movieDetail: movieDetailReducer,
  schedule: scheduleReducer,
  postList: postListReducer,
  theater: theaterReducer,
});
