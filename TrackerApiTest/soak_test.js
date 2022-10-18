import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
    stages: [
        { duration: '2m', target: 400 }, // ramp up to 400 users
        { duration: '3h56m', target: 400 }, // stay at 400 for ~4 hours
        { duration: '2m', target: 0 }, // scale down. (optional)
    ],
};

const API_BASE_URL = "https://localhost:5001/v1/";

export default function () {
    http.batch([
        ['GET', `tvshows/skip/0/take/25`],
    ]);

    sleep(1);
}
