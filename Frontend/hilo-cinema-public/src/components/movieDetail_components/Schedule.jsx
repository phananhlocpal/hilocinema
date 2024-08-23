import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchSchedule } from "../../redux/actions/movieDetail/scheduleAction";
import PropTypes from 'prop-types';
import { useNavigate } from "react-router-dom";

const Schedule = ({ movieUrl, selectedDate }) => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const { loading, schedule = {}, error } = useSelector(state => state.schedule);

    useEffect(() => {
        dispatch(fetchSchedule(movieUrl));
    }, [dispatch, movieUrl]);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error: {error}</p>;

    const schedules = Array.isArray(schedule.MovieSchedule) ? schedule.MovieSchedule : [];
    console.log(schedules);

    // Lọc lịch trình theo ngày đã chọn
    const filteredSchedules = schedules
        // .filter(scheduleItem => scheduleItem.date === selectedDate)
        ;

        const handleTimeClick = (movieId, movieUrl, theaterId, roomId, time) => {
            navigate(`/dat-ve/${movieUrl}`, {
                state: {
                    movieId,
                    date: selectedDate,
                    theaterId,
                    roomId,
                    time
                }
            });
        };

    return (
        <div>
            {filteredSchedules.length === 0 ? (
                <p>No schedules available for this date.</p>
            ) : (
                filteredSchedules.map((scheduleItem, index) => (
                    <div key={index}>
                        {scheduleItem.theaterSchedule.map((theaterScheduleItem, theaterIndex) => (
                            <div key={theaterIndex}>
                                <label className="text-sm font-semibold text-grey-10 mt-2 w-[150px]">
                                    {theaterScheduleItem.theater}
                                </label>
                                <div className="time__show flex flex-1 flex-row gap-x-3 gap-y-1 flex-wrap">
                                    {theaterScheduleItem.RoomSchedule.map((roomSchedule, roomIndex) => (
                                        <div key={roomIndex}>
                                            {roomSchedule.RoomSchedule.map((time, timeIndex) => (
                                                <button
                                                    key={timeIndex}
                                                    onClick={() => handleTimeClick(
                                                        schedule.movie.id,
                                                        schedule.movie.movieUrl,
                                                        theaterScheduleItem.theaterId,
                                                        roomSchedule.roomId,
                                                        time)}
                                                    className="py-2 md:px-8 px-6 border rounded text-sm font-normal text-black-10 hover:bg-blue-10 active:bg-blue-10 transition-all duration-500 ease-in-out hover:text-white"
                                                >
                                                    {time}
                                                </button>
                                            ))}
                                        </div>
                                    ))}
                                </div>
                            </div>
                        ))}
                    </div>
                ))
            )}
        </div>
    );
};

Schedule.propTypes = {
    movieUrl: PropTypes.string.isRequired,
    selectedDate: PropTypes.string.isRequired,
};

export default Schedule;
