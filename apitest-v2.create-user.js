import { check } from 'k6';
import http from 'k6/http';

export const options = {
    stages: [
        { duration: "10s", target: 20 },
        { duration: "50s", target: 20 }
    ]
};

export default function () {
    const apiUrl = "http://localhost:5000";

    const request = {
        firstName: "",
        lastName: "",
        email: ""
    }

    const response = http.post(`${apiUrl}/v-fail/users`, JSON.stringify(request), {
        headers: { "Content-Type": "application/json" }
    });

    check(response, {
        "response code was 400": (res) => res.status == 400,
        "response code was 409": (res) => res.status == 409
    })
}
