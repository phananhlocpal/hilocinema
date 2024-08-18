import axios from 'axios';

export const FETCH_SCHEDULE_REQUEST = 'FETCH_SCHEDULE_REQUEST';
export const FETCH_SCHEDULE_SUCCESS = 'FETCH_SCHEDULE_SUCCESS';
export const FETCH_SCHEDULE_FAILURE = 'FETCH_SCHEDULE_FAILURE';

export const fetchSchedule = (movieUrl) => async (dispatch) => {
    dispatch({ type: FETCH_SCHEDULE_REQUEST });
    try {
        const response = await axios.get(`http://localhost:8000/ScheduleService/movieUrl/${movieUrl}`);
        const { movie, schedules } = response.data;

        const formattedSchedules = formatSchedule(schedules);

        const result = {
            movie,
            MovieSchedule: formattedSchedules
        };

        console.log(result);

        dispatch({ type: FETCH_SCHEDULE_SUCCESS, payload: result });
    } catch (error) {
        dispatch({ type: FETCH_SCHEDULE_FAILURE, error: error.message });
    }
};

const formatSchedule = (data) => {
    if (!Array.isArray(data)) {
        throw new Error('Expected data to be an array');
    }

    const today = new Date();
    const scheduleMap = new Map();
    
    data.forEach(item => {
        const { date, time, seat: { room, room: { theater } } } = item;
        const scheduleDate = new Date(date);
        
        if (scheduleDate >= today) {
            if (!scheduleMap.has(date)) {
                scheduleMap.set(date, new Map());
            }

            const theaterMap = scheduleMap.get(date);
            if (!theaterMap.has(theater.id)) {
                theaterMap.set(theater.id, {
                    name: theater.name,
                    rooms: new Map()
                });
            }

            const theaterEntry = theaterMap.get(theater.id);
            if (!theaterEntry.rooms.has(room.id)) {
                theaterEntry.rooms.set(room.id, {
                    name: room.name,
                    times: []
                });
            }

            const roomEntry = theaterEntry.rooms.get(room.id);
            if (!roomEntry.times.includes(time)) {
                roomEntry.times.push(time);
            }
        }
    });

    const formattedSchedule = Array.from(scheduleMap, ([date, theaterMap]) => ({
        date,
        theaterSchedule: Array.from(theaterMap, ([theaterId, theaterEntry]) => ({
            theaterId,
            theater: theaterEntry.name,
            RoomSchedule: Array.from(theaterEntry.rooms, ([roomId, roomEntry]) => ({
                roomId,
                room: roomEntry.name,
                RoomSchedule: roomEntry.times
            }))
        }))
    }));

    return formattedSchedule;
};

