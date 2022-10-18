import http from 'k6/http';
import { check, group, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '5m', target: 100 }, // simulate ramp-up of traffic from 1 to 100 users over 5 minutes.
        { duration: '10m', target: 100 }, // stay at 100 users for 10 minutes
        { duration: '5m', target: 0 }, // ramp-down to 0 users
    ],
    thresholds: {
        'http_req_duration': ['p(99)<1500'], // 99% of requests must complete below 1.5s
        'logged in successfully': ['p(99)<1500'], // 99% of requests must complete below 1.5s
    },
};

const BASE_URL = "https://localhost:5001/v1/";
const NAME = 'admin';
const EMAIL = 'admin@gmail.com';
const PASSWORD = '123432323323';

export default () => {
    const loginRes = http.post(`${BASE_URL}login/user`, {
        Name:NAME,
        Email: EMAIL,
        Password: PASSWORD,
    });

    check(loginRes, (resp) => resp.json('token') !== '');

    const authHeaders = {
        headers: {
            Authorization: `Bearer ${loginRes.json('access')}`,
        },
    };

    const myObjects = http.get(`${BASE_URL}users/skip/0/take/25`, authHeaders).json();
    check(myObjects, { 'retrieved users': (obj) => obj.length > 0 });

    sleep(1);
};
